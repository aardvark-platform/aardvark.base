﻿namespace Aardvark.Base.Runtime

open System
open System.Collections.Generic
open System.Threading
open System.Runtime.InteropServices
open System.IO
open Aardvark.Base
open Aardvark.Base.Incremental
open Microsoft.FSharp.NativeInterop

#nowarn "9"

[<AutoOpen>]
module private Helpers = 
    let jumpInt =
        use ms = new MemoryStream()
        use w = AssemblerStream.ofStream ms
        w.Jump(0)
        ms.ToArray() |> Array.take (int ms.Length - 4)

    let ret =
        use ms = new MemoryStream()
        use w = AssemblerStream.ofStream ms
        w.EndFunction()
        w.Ret()
        ms.ToArray()

    let jumpSize = nativeint jumpInt.Length + 4n
    
[<AllowNullLiteral>]
type private Fragment<'a, 'b> =
    class
        val mutable prev : Fragment<'a, 'b>
        val mutable next : Fragment<'a, 'b>
        val mutable public TotalJumpDistance : ref<int64>
        val mutable public Manager : MemoryManager
        val mutable public Pointer : managedptr
        val mutable public Tag : 'a
        val mutable public JumpDistance : int64
        val mutable public Stats : 'b

        member x.Dispose() =
            x.Manager.Free x.Pointer
            if not (isNull x.prev) then
                x.prev.Next <- x.next
                Interlocked.Add(x.TotalJumpDistance, -x.JumpDistance) |> ignore
                x.JumpDistance <- 0L

        member private x.writeJump() =
            if not (isNull x.next) then
                let target = x.next.Pointer.Offset
                let source = x.Pointer.Offset + x.Pointer.Size - jumpSize
                let offset = target - source - 5n |> int
                let dist = abs (int64 offset)
                Interlocked.Add(x.TotalJumpDistance, dist - x.JumpDistance) |> ignore
                x.JumpDistance <- dist

                let off = x.Pointer.Size - jumpSize |> int
                x.Pointer.Write(off, jumpInt)
                x.Pointer.Write(off + jumpInt.Length, offset)
            else
                Interlocked.Add(x.TotalJumpDistance, -x.JumpDistance) |> ignore
                x.JumpDistance <- 0L

                let off = x.Pointer.Size - jumpSize |> int
                x.Pointer.Write(off, ret)
                    


        member x.Capacity = x.Pointer.Size - jumpSize

        member x.EntryPointer =
            x.Pointer.Parent.Pointer + x.Pointer.Offset

        member x.Realloc(newCapacity : nativeint) : unit =
            let newCapacity = newCapacity + jumpSize
            if newCapacity <> x.Pointer.Size then
                let moved = 
                    if x.Pointer.Free then 
                        x.Pointer <- x.Manager.Alloc(newCapacity)
                        x.writeJump()
                        true
                    else
                        x.Pointer |> ManagedPtr.realloc newCapacity

                x.writeJump()
                if moved && not (isNull x.prev) then
                    x.prev.writeJump()
                        
        member x.Prev
            with get() = x.prev

        member x.Next
            with get() = x.next
            and set n =
                x.next <- n
                if not (isNull n) then 
                    n.prev <- x

                x.writeJump()

        member x.GetStream() : Stream =
            new FragmentStream<'a, 'b>(x) :> Stream

        member x.AssemblerStream =
            AssemblerStream.ofStream (x.GetStream())

        new(totalJumps, manager, tag, stats) = { TotalJumpDistance = totalJumps; JumpDistance = 0L; Manager = manager; Tag = tag; Pointer = manager.Alloc(jumpSize); prev = null; next = null; Stats = stats }
    end

and private FragmentStream<'a, 'b>(f : Fragment<'a, 'b>) =
    inherit Stream()

    let mutable capacity = f.Capacity
    let mutable offset = 0n
    let mutable additional : MemoryStream = null //new MemoryStream()

    override x.CanRead = false
    override x.CanWrite = true
    override x.CanSeek = true

    override x.Dispose(disposing) =
        x.Flush()

        base.Dispose(disposing)
        let o = Interlocked.Exchange(&additional, null)
        if not (isNull o) then
            o.Dispose()
            capacity <- 0n
            offset <- 0n

    override x.Position
        with get() = int64 offset
        and set v = offset <- nativeint v
            
    override x.Length = int64 capacity + (if isNull additional then 0L else additional.Length)

    override x.Write(d, o, c) =
        let newOffset = offset + nativeint c
        if newOffset <= capacity then
            f.Pointer.Use (fun ptr ->
                Marshal.Copy(d, o, ptr + offset, c)
            )
            offset <- newOffset
        else
            let additional =
                match additional with
                    | null ->
                        let s = new MemoryStream()
                        additional <- s
                        s
                    | s -> s

            if offset < capacity then
                let storable = capacity - offset
                f.Pointer.Use (fun ptr ->
                    Marshal.Copy(d, o, ptr + offset, int storable)
                )

                additional.Position <- 0L
                if c > int storable then
                    additional.Write(d, o + int storable, c - int storable)

                offset <- newOffset

            else
                additional.Position <- int64 (offset - capacity)
                additional.Write(d, o, c)
                offset <- newOffset


                


        ()

    override x.Read(d, o, c) =
        failwith ""

    override x.SetLength(l : int64) =
        if nativeint l > capacity then
            let additional =
                match additional with
                    | null ->
                        let s = new MemoryStream()
                        additional <- s
                        s
                    | s -> s
                
            additional.SetLength(l - int64 capacity)

        else
            if not (isNull additional) then
                additional.Dispose()
                additional <- null

            f.Realloc(nativeint l)
            capacity <- nativeint l

    override x.Seek(o : int64, origin : SeekOrigin) =
        match origin with
            | SeekOrigin.Begin -> offset <- nativeint o; int64 offset
            | SeekOrigin.Current -> offset <- offset + nativeint o; int64 offset
            | _ -> offset <- nativeint (x.Length - o); int64 offset

    override x.Flush() =
        if not (isNull additional) then
            additional.Flush()
            f.Realloc(offset)

            let arr = additional.ToArray()
            f.Pointer.Use (fun ptr ->
                Marshal.Copy(arr, 0, ptr + capacity, arr.Length)
            )
            additional.Dispose()
            additional <- null
            capacity <- f.Capacity
        elif offset <> capacity then
            f.Realloc(offset)
            capacity <- f.Capacity

type NativeProgramUpdateStatistics =
    struct
        val mutable public Added : int
        val mutable public Removed : int
        val mutable public Updated : int
        val mutable public Compiled : int
        val mutable public Count : int
        val mutable public JumpDistance : int64
        static member Zero = NativeProgramUpdateStatistics()
    end

type NativeProgram<'a, 'b> private(data : alist<'a>, isDifferential : bool, compileDelta : Option<'a> -> 'a -> IAssemblerStream -> 'b, zero : 'b, add : 'b -> 'b -> 'b, sub : 'b -> 'b -> 'b) =
    inherit AdaptiveObject()
    let compileDelta = OptimizedClosures.FSharpFunc<_,_,_,_>.Adapt(compileDelta)
        
    let mutable disposed = false
    let manager = MemoryManager.createExecutable()

    let jumpDistance = ref 0L
    let mutable count = 0
    let mutable stats : 'b = zero

    let mutable prolog = 
        let f = new Fragment<'a, 'b>(jumpDistance, manager, Unchecked.defaultof<'a>, zero)
        use s = f.AssemblerStream
        s.BeginFunction()
        f

    let reader = data.GetReader()
    let cache : SortedDictionaryExt<Index, Fragment<'a, 'b>> = SortedDictionary.empty

        
    let mutable lastEntryPointer = prolog.EntryPointer
    let mutable run : unit -> unit = UnmanagedFunctions.wrap lastEntryPointer
    let entryPointerStore = NativePtr.alloc 1
    do NativePtr.write entryPointerStore prolog.EntryPointer

    let release() =
        if not disposed then
            disposed <- true
            cache.Clear()
            reader.Dispose()
            manager.Dispose()
            prolog <- null
            NativePtr.write entryPointerStore 0n
            run <- id
            jumpDistance := 0L
            count <- 0
            stats <- zero

    member x.FragmentCount = count

    member x.Stats = stats

    member x.EntryPointer = entryPointerStore

    member x.AverageJumpDistance = 
        if !jumpDistance = 0L then 0.0
        else float !jumpDistance / float count

    member x.TotalJumpDistance = !jumpDistance

    member x.Update(token : AdaptiveToken) : NativeProgramUpdateStatistics =
        x.EvaluateIfNeeded token NativeProgramUpdateStatistics.Zero (fun token ->
            if disposed then
                raise <| ObjectDisposedException("AdaptiveProgram")

            let ops = reader.GetOperations token

            let dirty = 
                if isDifferential then HashSet<Fragment<'a, 'b>>()
                else null

            let mutable added = 0
            let mutable removed = 0
            let mutable updated = 0
            let mutable compiled = 0

            for i, op in PDeltaList.toSeq ops do
                match op with
                    | Remove ->
                        match cache.TryGetValue i with
                            | (true, f) -> 
                                let n = f.Next
                                f.Dispose()
                                cache.Remove i |> ignore
                                if isDifferential then 
                                    if not (isNull n) then dirty.Add n |> ignore
                                    dirty.Remove f |> ignore
                                count <- count - 1
                                removed <- removed + 1
                                stats <- sub stats f.Stats
                                f.Stats <- zero
                            | _ ->
                                ()

                    | Set v ->
                        cache |> SortedDictionary.setWithNeighbours i (fun l s r ->
                            let l = l |> Option.map snd
                            let r = r |> Option.map snd

                            let prev = 
                                if isDifferential then
                                    match l with
                                        | Some f ->
                                            if f = prolog then None
                                            else Some f.Tag
                                        | None ->
                                            None
                                else
                                    None

                            match s with
                                | Some f ->
                                    f.Tag <- v
                                    let o = f.Stats
                                    let s = using f.AssemblerStream (fun s -> compileDelta.Invoke(prev, v, s))
                                    if isDifferential && not (isNull f.next) then dirty.Add f.next |> ignore
                                    stats <- add (sub stats o) s
                                    updated <- updated + 1
                                    compiled <- compiled + 1
                                    f.Stats <- s
                                    f

                                | None ->
                                    let f = new Fragment<'a, 'b>(jumpDistance, manager, v, zero)
                                    let s = using f.AssemblerStream (fun s -> compileDelta.Invoke(prev, v, s))
                                    stats <- add stats s
                                    count <- count + 1
                                    f.Stats <- s
                                    match l with
                                        | None -> prolog.Next <- f
                                        | Some(p) -> p.Next <- f

                                    match r with
                                        | None -> f.Next <- null
                                        | Some(n) ->    
                                            if isDifferential then dirty.Add n |> ignore
                                            f.Next <- n
                                            
                                    added <- added + 1
                                    compiled <- compiled + 1
                                    f

                        ) |> ignore

            if isDifferential then
                dirty.Remove prolog |> ignore
                for d in dirty do
                    let prev =
                        if d.Prev = prolog then None
                        else Some d.Prev.Tag

                    let o = d.Stats
                    let s = using d.AssemblerStream (fun s -> compileDelta.Invoke(prev, d.Tag, s))
                    compiled <- compiled + 1
                    stats <- add (sub stats o) s
                    d.Stats <- s

            let ptr = prolog.EntryPointer
            if ptr <> lastEntryPointer then
                lastEntryPointer <- ptr
                NativePtr.write entryPointerStore ptr
                run <- UnmanagedFunctions.wrap ptr

            new NativeProgramUpdateStatistics(
                Added = added,
                Removed = removed,
                Updated = updated,
                Compiled = compiled,
                Count = count,
                JumpDistance = !jumpDistance
            )

        )

    member x.Update() = x.Update(AdaptiveToken.Top)

    member x.Run() =
        lock x (fun () ->
            if disposed then
                raise <| ObjectDisposedException("AdaptiveProgram")

            run()
        )

    member private x.Dispose(disposing : bool) =
        if disposing then
            GC.SuppressFinalize x
            lock x release
        else
            release()

    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)


    interface IDisposable with
        member x.Dispose() = x.Dispose()

    new(data : alist<'a>, compile : Option<'a> -> 'a -> IAssemblerStream -> 'b, zero, add, sub) = new NativeProgram<'a, 'b>(data, true, compile, zero, add, sub)
    new(data : alist<'a>, compile : 'a -> IAssemblerStream -> 'b, zero, add, sub) = new NativeProgram<'a, 'b>(data, false, (fun _ v s -> compile v s), zero, add, sub)
    
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module NativeProgram =
    let differential (compile : Option<'a> -> 'a -> IAssemblerStream -> 'b, zero, add, sub) (values : alist<'a>) =
        new NativeProgram<'a, 'b>(values, compile, zero, add, sub)

    let simple (compile : 'a -> IAssemblerStream -> 'b, zero, add, sub) (values : alist<'a>) =
        new NativeProgram<'a, 'b>(values, compile, zero, add, sub)

type ChangeableNativeProgram<'a, 'b>(compile : 'a -> IAssemblerStream -> 'b, zero, add, sub) =
    
    let manager = MemoryManager.createExecutable()

    let entryPointer = NativePtr.alloc 1
    let jumpDistance = ref 0L
    let mutable prolog = 
        let f = new Fragment<'a, 'b>(jumpDistance, manager, Unchecked.defaultof<'a>, zero)
        use s = f.AssemblerStream
        s.BeginFunction()
        f

    let mutable lastEntry = 0n
    let mutable run : unit -> unit = id
    let mutable stats = zero

    let updateEntry() = 
        if prolog.EntryPointer <> lastEntry then
            lastEntry <- prolog.EntryPointer
            NativePtr.write entryPointer lastEntry
            run <- UnmanagedFunctions.wrap lastEntry

    do prolog.Next <- null; updateEntry()

    let mutable last = prolog
    let entries = Dict<'a, Fragment<'a, 'b>>()

    member x.Stats = stats

    member x.Add(value : 'a) =
        if isNull prolog then raise <| ObjectDisposedException("NativeProgram")

        if entries.ContainsKey value then
            false
        else
            let f = new Fragment<'a, 'b>(jumpDistance, manager, value, zero)
            let s = using f.AssemblerStream (fun s -> compile value s)
            stats <- add stats s
            last.Next <- f
            f.Next <- null
            f.Stats <- s
            last <- f
            entries.[value] <- f
            updateEntry()
            true

    member x.Remove(value : 'a) =
        if isNull prolog then raise <| ObjectDisposedException("NativeProgram")

        match entries.TryRemove value with
            | (true, f) ->
                let prev = f.Prev
                let next = f.Next
                prev.Next <- next
                if isNull next then last <- prev
                f.Dispose()
                stats <- sub stats f.Stats
                f.Stats <- zero
                updateEntry()
                true
            | _ ->
                false
        
    member x.EntryPointer = 
        if isNull prolog then raise <| ObjectDisposedException("NativeProgram")
        entryPointer

    member x.Clear() =
        if not (isNull prolog) then
            for f in entries.Values do f.Dispose()
            entries.Clear()
            prolog.Next <- null
            last <- prolog
            stats <- zero
            updateEntry()

    member x.Run() = run()

    member x.Dispose() =
        if not (isNull prolog) then
            manager.Dispose()
            run <- id
            lastEntry <- 0n
            entries.Clear()
            prolog <- null
            NativePtr.free entryPointer
            last <- null
            stats <- zero
            jumpDistance := 0L

    interface IDisposable with
        member x.Dispose() = x.Dispose()
