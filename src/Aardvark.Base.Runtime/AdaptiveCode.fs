namespace Aardvark.Base.Runtime

open System
open System.Threading
open System.Runtime.InteropServices
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental


type NativeCalls = list<NativeCall>

[<AllowNullLiteral>]
type AdaptiveCode(content : list<IMod<NativeCalls>>) =
    member x.Content = content

    abstract member Dispose : unit -> unit
    default x.Dispose() = ()

    interface IDisposable with
        member x.Dispose() = x.Dispose()


type DynamicProgramStatistics =
    struct
        val mutable public DeltaProcessTime : TimeSpan
        val mutable public CompileTime : TimeSpan
        val mutable public WriteTime : TimeSpan
        
        val mutable public AddedFragmentCount : int
        val mutable public RemovedFragmentCount : int
        val mutable public CompiledFragmentCount : int
        val mutable public UpdatedFragmentCount : int
        val mutable public UpdatedJumpCount : int

        static member Zero =
            DynamicProgramStatistics(
                DeltaProcessTime = TimeSpan.Zero,
                CompileTime = TimeSpan.Zero,
                WriteTime = TimeSpan.Zero,
                AddedFragmentCount = 0,
                RemovedFragmentCount = 0,
                CompiledFragmentCount = 0,
                UpdatedFragmentCount = 0,
                UpdatedJumpCount = 0
            )

        static member (+) (l : DynamicProgramStatistics, r : DynamicProgramStatistics) =
            DynamicProgramStatistics(
                DeltaProcessTime = l.DeltaProcessTime + r.DeltaProcessTime,
                CompileTime = l.CompileTime + r.CompileTime,
                WriteTime = l.WriteTime + r.WriteTime,
                AddedFragmentCount = l.AddedFragmentCount + r.AddedFragmentCount,
                RemovedFragmentCount = l.RemovedFragmentCount + r.RemovedFragmentCount,
                CompiledFragmentCount = l.CompiledFragmentCount + r.CompiledFragmentCount,
                UpdatedFragmentCount = l.UpdatedFragmentCount + r.UpdatedFragmentCount,
                UpdatedJumpCount = l.UpdatedJumpCount + r.UpdatedJumpCount
            )

        static member (-) (l : DynamicProgramStatistics, r : DynamicProgramStatistics) =
            DynamicProgramStatistics(
                DeltaProcessTime = l.DeltaProcessTime - r.DeltaProcessTime,
                CompileTime = l.CompileTime - r.CompileTime,
                WriteTime = l.WriteTime - r.WriteTime,
                AddedFragmentCount = l.AddedFragmentCount - r.AddedFragmentCount,
                RemovedFragmentCount = l.RemovedFragmentCount - r.RemovedFragmentCount,
                CompiledFragmentCount = l.CompiledFragmentCount - r.CompiledFragmentCount,
                UpdatedFragmentCount = l.UpdatedFragmentCount - r.UpdatedFragmentCount,
                UpdatedJumpCount = l.UpdatedJumpCount - r.UpdatedJumpCount
            )


        override x.ToString() =
            String.concat "\r\n" [
                "DynamicProgramStatistics {"
                sprintf "   DeltaProcessTime = %A" x.DeltaProcessTime
                sprintf "   CompileTime = %A" x.CompileTime
                sprintf "   WriteTime = %A" x.WriteTime
                sprintf "   AddedFragmentCount = %A" x.AddedFragmentCount
                sprintf "   RemovedFragmentCount = %A" x.RemovedFragmentCount
                sprintf "   CompiledFragmentCount = %A" x.CompiledFragmentCount
                sprintf "   UpdatedFragmentCount = %A" x.UpdatedFragmentCount
                sprintf "   UpdatedJumpCount = %A" x.UpdatedJumpCount
                "}"
            ]

    end

   

type IDynamicProgram =
    inherit IAdaptiveObject
    inherit IDisposable
    
    abstract member Update : IAdaptiveObject -> DynamicProgramStatistics
    abstract member Run : unit -> unit

    abstract member NativeCallCount : int
    abstract member FragmentCount : int
    abstract member ProgramSizeInBytes : int64
    abstract member TotalJumpDistanceInBytes : int64




type private OptimizedDynamicFragmentContext<'a> =
    {
        compileDelta : Option<'a> -> 'a -> AdaptiveCode
        memory : MemoryManager
        nativeCallCount : ref<int>
        jumpDistance : ref<int>
    }

[<AllowNullLiteral>]
type private OptimizedDynamicFragment<'a> =
    class
        inherit AdaptiveObject

        val mutable public Context : OptimizedDynamicFragmentContext<'a>
        val mutable public Storage : CodeFragment
        val mutable public Tag : Option<'a>
        val mutable public Prev : OptimizedDynamicFragment<'a>
        val mutable public Next : OptimizedDynamicFragment<'a>
        val mutable public Code : AdaptiveCode
        val mutable public CodePrevTag : Option<'a>
        val mutable public CallCount : int
        val mutable public JumpDistance : int

        member x.Recompile (caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                let hasCode = not (isNull x.Code)
                let upToDate = hasCode && Object.Equals(x.CodePrevTag, x.Prev.Tag)

                if not upToDate then
                    if hasCode then x.Code.Dispose()
                    x.Code <- x.Context.compileDelta x.Prev.Tag x.Tag.Value
                    x.CodePrevTag <- x.Prev.Tag
                    true
                else
                    false
            )

        member x.WriteContent (caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                let code =
                    x.Code.Content
                        |> List.collect (fun c -> c.GetValue x)
                        |> List.toArray

                Interlocked.Add(x.Context.nativeCallCount, code.Length - x.CallCount) |> ignore
                x.CallCount <- code.Length

                if isNull x.Storage then
                    x.Storage <- CodeFragment(x.Context.memory, code)
                    true
                else
                    let ptr = x.Storage.Offset

                    // TODO: maybe partial updates here
                    x.Storage.Write(code)
                    ptr <> x.Storage.Offset
            )

        member x.LinkPrev(caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                let prevFragment = x.Prev.Storage
                let myFragment = x.Storage

                if prevFragment.ReadNextPointer() <> myFragment.Offset then
                    let distance = prevFragment.WriteNextPointer(myFragment.Offset)
                    Interlocked.Add(x.Context.jumpDistance, distance - x.JumpDistance) |> ignore
                    x.JumpDistance <- distance
            )

        member x.LinkNext(caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                let nextFragment = x.Next.Storage
                let myFragment = x.Storage

                if myFragment.ReadNextPointer() <> nextFragment.Offset then
                    let distance = myFragment.WriteNextPointer(nextFragment.Offset)
                    Interlocked.Add(x.Context.jumpDistance, distance - x.JumpDistance) |> ignore
                    x.JumpDistance <- distance
            )

        member x.Dispose() =
            x.Prev <- null
            x.Next <- null

            Interlocked.Add(x.Context.jumpDistance, -x.JumpDistance) |> ignore
            x.JumpDistance <- 0

            Interlocked.Add(x.Context.nativeCallCount, -x.CallCount) |> ignore
            x.CallCount <- 0

            if not (isNull x.Storage) then
                x.Storage.Dispose()
                x.Storage <- null
            
            if not (isNull x.Code) then
                x.Code.Dispose()
                x.CodePrevTag <- None
                x.Code <- null

        interface IDisposable with
            member x.Dispose() = x.Dispose()


        new(context, tag) = 
            { Context = context
              Storage = null; Next = null; 
              Prev = null; Tag = Some tag; 
              Code = null; CodePrevTag = None
              CallCount = 0; JumpDistance = 0 }

        new(context, storage) = 
            { Context = context
              Storage = storage; Next = null; 
              Prev = null; Tag = None; 
              Code = null; CodePrevTag = None
              CallCount = 0; JumpDistance = 0 }
    end


type private OptimizedDynamicProgram<'k, 'a when 'k : equality>(input : amap<'k, 'a>, maxArgumentCount : int,
                                                                keyComparer : IComparer<'k>, 
                                                                compileDelta : Option<'a> -> 'a -> AdaptiveCode ) =
    inherit AdaptiveObject()

    let reader = input.ASet.GetReader()
    let memory = MemoryManager.createExecutable()

    let cache = Dict<'k * 'a, OptimizedDynamicFragment<'a>>()
    let fragments = SortedDictionaryExt<'k, StableSet<OptimizedDynamicFragment<'a>>>(keyComparer)

    let nativeCallCount = ref 0
    let jumpDistance = ref 0
    let context = { memory = memory; compileDelta = compileDelta; nativeCallCount = nativeCallCount; jumpDistance = jumpDistance }
    let prolog = new OptimizedDynamicFragment<_>(context, CodeFragment(memory, Assembler.functionProlog maxArgumentCount))
    let epilog = new OptimizedDynamicFragment<_>(context, CodeFragment(memory, Assembler.functionEpilog maxArgumentCount))

    do prolog.Next <- epilog
       epilog.Prev <- prolog

    let dirtyLock = obj()
    let mutable dirtySet = HashSet<OptimizedDynamicFragment<'a>>()

    let run = CodeFragment.wrap prolog.Storage


    let deltaProcessWatch = System.Diagnostics.Stopwatch()
    let compileWatch = System.Diagnostics.Stopwatch()
    let writeWatch = System.Diagnostics.Stopwatch()



    override x.InputChanged(o : IAdaptiveObject) =
        match o with
            | :? OptimizedDynamicFragment<'a> as o ->
                lock dirtyLock (fun () -> dirtySet.Add o |> ignore)
            | _ ->
                ()

    member x.Run() =
        lock x (fun () ->        
            run()
        )

    member x.Update caller = 
        x.EvaluateIfNeeded caller DynamicProgramStatistics.Zero (fun v ->
            let deltas = reader.GetDelta x

            let dirtySet = 
                lock dirtyLock (fun () ->
                    let set = dirtySet
                    dirtySet <- HashSet()
                    set
                )

            let recompileSet = HashSet()
            let relinkSet = HashSet()

            let mutable added = 0
            let mutable removed = 0
            
            let prologNextPtr = 
                let store = prolog.Next.Storage
                if isNull store then -1n
                else store.Offset

            let epilogPrevPtr = 
                let store = epilog.Prev.Storage
                if isNull store then -1n
                else store.Offset


            let createBetween (prev : Option<OptimizedDynamicFragment<'a>>) (v : 'a) (next : Option<OptimizedDynamicFragment<'a>>)  =

                match next with
                    | Some n -> recompileSet.Add n |> ignore
                    | _ -> ()

                //let code = desc.compileDelta (Option.map DynamicFragment.tag prev) v (Option.map DynamicFragment.tag next)
                let fragment = new OptimizedDynamicFragment<_>(context, v)

                let l = match prev with | Some l -> l | None -> prolog
                let r = match next with | Some r -> r | None -> epilog

                r.Prev <- fragment
                fragment.Prev <- l
                fragment.Next <- r
                l.Next <- fragment

                fragment

            deltaProcessWatch.Restart()
            for d in deltas do
                match d with
                    | Add (k,v) ->
                        added <- added + 1

                        let l,s,r = SortedDictionary.neighbourhood k fragments

                        let next =
                            match r with
                                | Some(_,r) -> r.First
                                | None -> None

                        let prev =
                            match l with
                                | Some(_,l) -> l.Last
                                | None -> None

                        let fragment = 
                            match s with
                                | Some self ->
                                    let created =
                                        self.AddWithPrev (fun p -> createBetween p v next) 

                                    match created with
                                        | Some f -> f
                                        | None -> failwithf "duplicated key: %A" k

                                | None ->
                                    let frag = createBetween prev v next
                                    let set = StableSet()
                                    set.Add frag |> ignore
                                    fragments.[k] <- set
                                    frag

                        cache.[(k,v)] <- fragment
                        recompileSet.Add fragment |> ignore

                    | Rem (k,v) ->
                        
                        match fragments.TryGetValue k with
                            | (true, set) ->
                                match cache.TryRemove ((k,v)) with
                                    | (true, fragment) ->

                                        if set.Remove fragment then
                                            removed <- removed + 1 
                                            fragment.Next.Prev <- fragment.Prev
                                            fragment.Prev.Next <- fragment.Next

                                            recompileSet.Add fragment.Next |> ignore

                                            fragment.Dispose()
                                            dirtySet.Remove fragment |> ignore
                                            recompileSet.Remove fragment |> ignore

                                            if set.Count = 0 then
                                                fragments.Remove k |> ignore

                                        ()
                                    | _ ->
                                        failwithf "could not find Fragment for: %A" k
                            | _ -> 
                                failwithf "could not find container for: %A" k
            deltaProcessWatch.Stop()



            compileWatch.Restart()
            for r in recompileSet do
                if r.Recompile x then
                    relinkSet.Add r |> ignore
                    dirtySet.Add r |> ignore

            compileWatch.Stop()



            writeWatch.Restart()
            for d in dirtySet do
                if d.WriteContent x then
                    relinkSet.Add d |> ignore

            for d in relinkSet do
                d.LinkPrev x

            prolog.LinkNext x
            epilog.LinkPrev x
            writeWatch.Stop()

            DynamicProgramStatistics (
                DeltaProcessTime = deltaProcessWatch.Elapsed,
                CompileTime = compileWatch.Elapsed,
                WriteTime = writeWatch.Elapsed,

                AddedFragmentCount = added,
                RemovedFragmentCount = removed,
                CompiledFragmentCount = recompileSet.Count,
                UpdatedFragmentCount = dirtySet.Count,
                UpdatedJumpCount = relinkSet.Count
            )
        ) 

    member x.Dispose() =
        dirtySet.Clear()
        reader.Dispose()
        memory.Dispose()
        cache.Clear()
        fragments.Clear()

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface IDynamicProgram with
        member x.Update(caller) = x.Update(caller)
        member x.Run() = x.Run()

        member x.FragmentCount = cache.Count
        member x.NativeCallCount = !nativeCallCount
        member x.ProgramSizeInBytes = int64 (memory.AllocatedBytes - prolog.Storage.Memory.Size - epilog.Storage.Memory.Size)
        member x.TotalJumpDistanceInBytes = int64 (!jumpDistance - prolog.JumpDistance - epilog.JumpDistance)



module DynamicProgram =
    
    let optimized (maxArgs : int) (comparer : IComparer<'k>) (compileDelta : Option<'v> -> 'v -> AdaptiveCode) (input : amap<'k, 'v>) =
        new OptimizedDynamicProgram<_,_>(input, maxArgs, comparer, compileDelta) :> IDynamicProgram

    let inline run (p : IDynamicProgram) =
        p.Run()

    let inline update (p : IDynamicProgram) =
        p.Update(null)


    let inline nativeCallCount (p : IDynamicProgram) = p.NativeCallCount
    let inline fragmentCount (p : IDynamicProgram) = p.FragmentCount
    let inline programSizeInBytes (p : IDynamicProgram) = p.ProgramSizeInBytes
    let inline totalJumpDistanceInBytes (p : IDynamicProgram) = p.TotalJumpDistanceInBytes


module Tests =
    
    let printFunction (v : int) =
        printf "%A " v

    type Print = delegate of int -> unit
    let dPrint = Print printFunction
    let pPrint = Marshal.GetFunctionPointerForDelegate dPrint

    let run() =
        let calls  = 
            CSet.ofList [
                0,0
                3,3
                2,6
                1,9
            ]


        let compile =
            let compileDelta p s =
                match p with
                    | Some p -> new AdaptiveCode([Mod.constant [pPrint, [|s - p :> obj|]]])
                    | None -> new AdaptiveCode([Mod.constant [pPrint, [|s :> obj|]]])

            DynamicProgram.optimized 6 Comparer.Default compileDelta

        let prog = calls |> AMap.ofASet |> compile

        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""

        transact (fun () ->
            calls |> CSet.remove (0,0) |> ignore
        )

        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""


        transact (fun () ->
            calls |> CSet.add (1,10) |> ignore
        )
        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""


        transact (fun () ->
            calls |> CSet.unionWith (List.init 10000 (fun i -> (-1, i)))
        )


        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""


        printfn "fragments:         %A" prog.FragmentCount
        printfn "calls:             %A" prog.NativeCallCount
        printfn "jump distance:     %A" prog.TotalJumpDistanceInBytes
        printfn "program size:      %A" prog.ProgramSizeInBytes





