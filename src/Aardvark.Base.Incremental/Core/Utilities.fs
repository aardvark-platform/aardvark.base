namespace Aardvark.Base.Incremental

open Aardvark.Base
open System.Collections
open System.Collections.Generic
open System.Threading
open System.Runtime.InteropServices

type IVersioned =
    abstract member Version : int

type IVersionedSet<'a> =
    inherit ISet<'a>
    inherit IVersioned

type IVersionedDictionary<'k, 'v> =
    inherit IDictionary<'k, 'v>
    inherit IVersioned

type VersionedSet<'a>(s : ISet<'a>) =
    let mutable version = 0
    let hasChanged() = Interlocked.Increment &version |> ignore
        
    let trackCount (f : unit -> 'x) =
        let old = s.Count
        let res = f()
        if s.Count <> old then
            hasChanged()
        res

    member x.Remove v = if s.Remove v then hasChanged(); true else false
    member x.Clear() = if s.Count <> 0 then hasChanged(); s.Clear()
    member x.Count = s.Count
    member x.CopyTo(arr, index) = s.CopyTo(arr, index)
    member x.IsReadOnly = s.IsReadOnly
    member x.Contains v = s.Contains v
    member x.Add item = if s.Add item then hasChanged(); true else false
    member x.ExceptWith other = trackCount (fun () -> s.ExceptWith other)
    member x.IntersectWith other = trackCount (fun () -> s.IntersectWith other)
    member x.UnionWith other = trackCount (fun () -> s.UnionWith other)
    member x.SymmetricExceptWith other = trackCount (fun () -> s.SymmetricExceptWith other)
    member x.IsProperSubsetOf other = s.IsProperSubsetOf other
    member x.IsProperSupersetOf other = s.IsProperSupersetOf other
    member x.IsSubsetOf other = s.IsSubsetOf other
    member x.IsSupersetOf other = s.IsSupersetOf other
    member x.Overlaps other = s.Overlaps other
    member x.SetEquals other = s.SetEquals other

    member x.GetEnumerator() = s.GetEnumerator()

    interface IVersionedSet<'a> with
        member x.Version = version

    interface ICollection<'a> with
        member x.Add v = x.Add v |> ignore
        member x.Remove v = x.Remove v
        member x.Clear() = x.Clear()
        member x.Count = x.Count
        member x.CopyTo(arr, index) = x.CopyTo(arr, index)

        member x.IsReadOnly = x.IsReadOnly
        member x.Contains v = x.Contains v

    interface ISet<'a> with
        member x.Add item = x.Add item
        member x.ExceptWith other = x.ExceptWith other
        member x.IntersectWith other = x.IntersectWith other
        member x.UnionWith other = x.UnionWith other
        member x.SymmetricExceptWith other = x.SymmetricExceptWith other

        member x.IsProperSubsetOf other = x.IsProperSubsetOf other
        member x.IsProperSupersetOf other = x.IsProperSupersetOf other
        member x.IsSubsetOf other = x.IsSubsetOf other
        member x.IsSupersetOf other = x.IsSupersetOf other
        member x.Overlaps other = x.Overlaps other
        member x.SetEquals other = x.SetEquals other


    interface IEnumerable with
        member x.GetEnumerator() = s.GetEnumerator() :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() = x.GetEnumerator()

type VersionedDictionary<'k, 'v when 'k : equality> (d : IDictionary<'k, 'v>) =
    let dcoll = d :> ICollection<_>
    let mutable version = 0
    let hasChanged() = Interlocked.Increment &version |> ignore
  
  
    member x.Add(k,v) = d.Add(k,v); hasChanged()
    member x.Remove(k : 'k) = if d.Remove(k) then hasChanged(); true else false
    member x.Item
        with get k = d.[k]
        and set k v =
            match d.TryGetValue k with
                | (true, o) when System.Object.Equals(o, v) ->
                    ()
                | _ ->
                    d.[k] <- v
                    hasChanged()


    member x.TryGetValue(k : 'k,[<Out>] v : byref<'v>) =
        d.TryGetValue(k, &v)

    member x.ContainsKey(k) = d.ContainsKey k
    member x.Keys = d.Keys :> ICollection<_>
    member x.Values = d.Values :> ICollection<_>
    member x.Version = version     
    member x.Count = d.Count
    member x.Clear() = 
        let oldCount = x.Count
        d.Clear()
        if oldCount <> 0 then hasChanged()

    interface ICollection<KeyValuePair<'k, 'v>> with
        member x.IsReadOnly = d.IsReadOnly
        member x.Count = x.Count
        member x.Add(kvp) = dcoll.Add(kvp); hasChanged()
        member x.Clear() = x.Clear()
        member x.Contains(kvp) = dcoll.Contains kvp
        member x.CopyTo(arr, index) = dcoll.CopyTo(arr, index)
        member x.Remove(kvp) = if dcoll.Remove kvp then hasChanged(); true else false
        member x.GetEnumerator() = dcoll.GetEnumerator()
        member x.GetEnumerator() = dcoll.GetEnumerator() :> IEnumerator

    interface IDictionary<'k, 'v> with
        member x.Add(k,v) = x.Add(k,v)
        member x.Remove(k) = x.Remove(k)
        member x.Item
            with get k = x.[k]
            and set k v = x.[k] <- v


        member x.TryGetValue(k : 'k, v : byref<'v>) = x.TryGetValue(k, &v)
        member x.ContainsKey(k) = x.ContainsKey k
        member x.Keys = x.Keys 
        member x.Values = x.Values

    interface IVersionedDictionary<'k, 'v> with
        member x.Version = version

    new() = VersionedDictionary<'k, 'v>(Dictionary())

module ChangeTracker =
    type private ChangeTracker<'a>() =
        static let getVersion (a : 'a) =
            (a |> unbox<IVersioned>).Version

        static let createSimpleTracker : (Option<'a -> 'a -> bool>) -> 'a -> bool =
            fun eq ->
                let eq = defaultArg eq (fun a b -> System.Object.Equals(a,b))
                let old = ref None
                fun n ->
                    match !old with
                        | None -> 
                            old := Some n
                            true
                        | Some o ->
                            if eq o n then 
                                false
                            else 
                                old := Some n
                                true

        static let createTracker : (Option<'a -> 'a -> bool>) -> 'a -> bool =
            
            if typeof<IVersioned>.IsAssignableFrom typeof<'a> then
                fun eq ->
                    let eq = defaultArg eq (fun a b -> System.Object.Equals(a,b))
                    let old = ref None

                    fun n ->
                        let v = getVersion n
                        match !old with
                            | None -> 
                                old := Some (n, v)
                                true
                            | Some (o,ov) ->
                                if ov = v && eq o n then
                                    false
                                else
                                    old := Some (n, v)
                                    true

            else createSimpleTracker

        static let createVersionOnlyTracker : unit -> 'a -> bool =
            
            if typeof<IVersioned>.IsAssignableFrom typeof<'a> then
                fun () ->
                    let old = ref None

                    fun n ->
                        let v = getVersion n
                        match !old with
                            | None -> 
                                old := Some v
                                true
                            | Some ov ->
                                if ov = v then
                                    false
                                else
                                    old := Some v
                                    true
            else
                fun () n -> false


              

        static member CreateTracker eq = createTracker eq
        static member CreateDefaultTracker() = createTracker None
        static member CreateCustomTracker eq = createTracker (Some eq)
        static member CreateVersionOnlyTracker() = createVersionOnlyTracker ()

    let track<'a> : 'a -> bool =
        ChangeTracker<'a>.CreateDefaultTracker()

    let trackCustom<'a> (eq : Option<'a -> 'a -> bool>) : 'a -> bool =
        ChangeTracker<'a>.CreateTracker eq

    let trackVersion<'a> : 'a -> bool =
        ChangeTracker<'a>.CreateVersionOnlyTracker()


type AdaptiveLocksadasd() =
    let rw = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion)
    
    member x.EnterRead(o : obj) = 
        rw.EnterReadLock()
        Monitor.Enter o

    member x.ExitRead() = 
        rw.ExitReadLock()

    member x.Downgrade(o : obj) =
        Monitor.Exit o

    member x.EnterWrite(o : obj) = 
        rw.EnterWriteLock()
        Monitor.Enter o

    member x.ExitWrite(o : obj) = 
        Monitor.Exit o
        rw.ExitWriteLock()


type AdaptiveLock() =
    let mutable readerCount = 0

    member x.EnterRead(o : obj) = 
        Monitor.Enter o
        Interlocked.Increment(&readerCount) |> ignore

    member x.Downgrade(o : obj) = 
        Monitor.Exit o

    member x.ExitRead() = 
        Interlocked.Decrement(&readerCount) |> ignore 

    member x.EnterWrite(o : obj) = 
        let rec enter(level : int) =
            while Volatile.Read(&readerCount) > 0 do
                Thread.Sleep(0)

            Monitor.Enter o
            if readerCount > 0 then
                if level > 10 then Log.warn "yehaaa"
                Monitor.Exit o
                enter(level + 1)
            else
                ()

        if Monitor.IsEntered o && readerCount <= 1 then
            Monitor.Enter o
        else
            enter 0

    member x.ExitWrite(o : obj) = 
        Monitor.Exit o