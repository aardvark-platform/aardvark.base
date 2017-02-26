namespace Aardvark.Base.Incremental

open System
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base
open System.Threading
open System.Collections
open System.Collections.Generic

[<AutoOpen>]
module internal Heap = 
    type HeapEntry<'k, 'v> internal(key : 'k, value : 'v, index : int) =
        let mutable key = key
        let mutable index = index
        let mutable value = value


        member x.Key
            with get() = key
            and set k = key <- k

        member internal x.Index
            with get() = index
            and set v = index <- v

        member x.Value
            with get() = value
            and set v = value <- v

    type Heap<'k, 'v>(comparer : IComparer<'k>) =
        let store = List<HeapEntry<'k, 'v>>()

        let parent (i : int) =
            (i - 1) / 2

        let swap (i : int) (j : int) =
            let vi = store.[i]
            let vj = store.[j]
            vi.Index <- j
            vj.Index <- i
            store.[i] <- vj
            store.[j] <- vi

        let rec bubbleUp (entry : HeapEntry<'k, 'v>) (i : int) =
            if i > 0 then
                let p = parent i
                let c = comparer.Compare(store.[p].Key, entry.Key)
                if c > 0 then 
                    swap i p
                    bubbleUp entry p

        let rec pushDown (entry : HeapEntry<'k, 'v>) (i : int) =
            let lc = 2 * i + 1
            let rc = 2 * i + 2

            let ccl = if lc < store.Count then comparer.Compare(entry.Key, store.[lc].Key) else -1
            let ccr = if rc < store.Count then comparer.Compare(entry.Key, store.[rc].Key) else -1

            match ccl, ccr with
                | 1, 1 ->
                    let c = comparer.Compare(store.[lc].Key, store.[rc].Key)
                    if c < 0 then
                        swap lc i
                        pushDown entry lc
                    else
                        swap rc i
                        pushDown entry rc

                | 1, _ ->
                    swap lc i
                    pushDown entry lc

                | _, 1 ->
                    swap rc i
                    pushDown entry rc

                | _ -> ()

        member x.Count = store.Count

        member x.Enqueue(key : 'k, value : 'v) =
            let entry = HeapEntry(key, value, store.Count)
            store.Add(entry)
            bubbleUp entry entry.Index
            entry

        member x.Dequeue() =
            if store.Count > 0 then
                let entry = store.[0]
                let last = store.Count - 1
                swap 0 last
                store.RemoveAt last
                if store.Count > 0 then
                    pushDown store.[0] 0

                entry.Index <- -1
                entry
            else
                failwith "sadasd"

        member x.Remove(e : HeapEntry<'k, 'v>) =
            if e.Index >= 0 && e.Index < store.Count then
                let index = e.Index
                let last = store.Count - 1
                if index = last then
                    store.RemoveAt last
                else
                    swap index last
                    store.RemoveAt last
                    pushDown store.[index] index

                e.Index <- -1
                true
            else
                false

        member x.Peek =
            store.[0]

        member x.Min =
            if store.Count > 0 then 
                Some (store.[0].Key)
            else
                None

        member x.ChangeKey(e : HeapEntry<'k, 'v>, newKey : 'k) =
            if e.Index < 0 then
                e.Index <- store.Count
                e.Key <- newKey
                store.Add(e)
                bubbleUp e e.Index
            else
                let c = comparer.Compare(e.Key, newKey)
                if c < 0 then
                    e.Key <- newKey
                    pushDown e e.Index
                elif c > 0 then
                    e.Key <- newKey
                    bubbleUp e e.Index
        
            store.[0].Key

        new() = Heap<'k, 'v>(Comparer<'k>.Default)


type IOpReader<'ops> =
    inherit IAdaptiveObject
    inherit IDisposable
    abstract member GetOperations : IAdaptiveObject -> 'ops

type IOpReader<'s, 'ops> =
    inherit IOpReader<'ops>
    abstract member State : 's

[<AbstractClass>]
type AbstractReader<'ops>(scope : Ag.Scope, t : Monoid<'ops>) =
    inherit AdaptiveObject()

    abstract member Release : unit -> unit
    abstract member Compute : unit -> 'ops

    abstract member Apply : 'ops -> 'ops
    default x.Apply o = o

    member x.GetOperations(caller : IAdaptiveObject) =
        x.EvaluateIfNeeded caller t.mempty (fun () ->
            Ag.useScope scope (fun () -> 
                x.Compute() |> x.Apply
            )
        )   

    member x.Dispose() =
        x.Release()
        let mutable foo = 0
        x.Outputs.Consume(&foo) |> ignore

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface IOpReader<'ops> with
        member x.GetOperations c = x.GetOperations c

[<AbstractClass>]
type AbstractReader<'s, 'ops>(scope : Ag.Scope, t : Traceable<'s, 'ops>) =
    inherit AbstractReader<'ops>(scope, t.ops)

    let mutable state = t.empty

    override x.Apply o =
        let (s, o) = t.apply state o
        state <- s
        o

    interface IOpReader<'s, 'ops> with
        member x.State = state

[<AbstractClass>]
type AbstractDirtyReader<'t, 'ops when 't :> IAdaptiveObject>(scope : Ag.Scope, t : Monoid<'ops>) =
    inherit DirtyTrackingAdaptiveObject<'t>()

    abstract member Release : unit -> unit
    abstract member Compute : HashSet<'t> -> 'ops

    abstract member Apply : 'ops -> 'ops
    default x.Apply o = o

    member x.GetOperations(caller : IAdaptiveObject) =
        x.EvaluateIfNeeded' caller t.mempty (fun dirty ->
            Ag.useScope scope (fun () -> 
                x.Compute dirty |> x.Apply
            )
        )   

    member x.Dispose() =
        x.Release()
        let mutable foo = 0
        x.Outputs.Consume(&foo) |> ignore

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface IOpReader<'ops> with
        member x.GetOperations c = x.GetOperations c

//[<AbstractClass; Sealed; Extension>]
//type IReaderExtensions private() =
//    [<Extension>]
//    static member GetOperations(this : IOpReader<'ops>) =
//        this.GetOperations null

module private NewHistory =

    [<AllowNullLiteral>]
    type RelevantNode<'a> =
        class
            val mutable public Prev : RelevantNode<'a>
            val mutable public Next : RelevantNode<'a>
            val mutable public RefCount : int
            val mutable public Value : 'a
            
            new(p, v, n) = { Prev = p; Next = n; RefCount = 0; Value = v }
        end

    type RelevantSet<'v>(m : Monoid<'v>) =
        
        let mutable first : RelevantNode<'v> = null
        let mutable last : RelevantNode<'v> = null

        /// O(1)
        member x.Append(v : 'v) =
            if isNull last || last.RefCount > 0 then
                let n = RelevantNode(last, v, null)
                last.Next <- n
                last <- n
            else
                last.Value <- m.mappend last.Value v

        // O(n) [where n is the number of relevant entries]
        member x.Read(node : byref<RelevantNode<'v>>) =
            let mutable start = node
            let mutable res = start.Value

            if start.RefCount = 1 then
                let p = start.Prev
                let n = start.Next
                start.RefCount <- 0
                start.Prev <- null
                start.Next <- null
                start <- n
                let start = ()

                if isNull p then
                    first <- n
                    if isNull n then last <- null
                    else n.Prev <- null
                else
                    p.Value <- m.mappend p.Value res
                    p.Next <- n
                    if isNull n then last <- p
                    else n.Prev <- p
            else
                start.RefCount <- start.RefCount - 1
                

            let mutable current = start
            while not (isNull current) do
                res <- m.mappend res current.Value
                current <- current.Next

            if not (isNull last) then
                last.RefCount <- last.RefCount + 1

            node <- last
            res

    
    type History<'s, 'op>(t : Traceable<'s, 'op>) =
        inherit AdaptiveObject()


        let mutable first : RelevantNode<'op> = null
        let mutable last : RelevantNode<'op> = null
        let mutable state = t.empty


        let mergeIntoLast (start : RelevantNode<'op>) =
            let res = start.Value
            let p = start.Prev
            let n = start.Next
            if start.RefCount = 1 then
                start.RefCount <- 0
                start.Prev <- null
                start.Next <- null
                
                let start = ()

                if isNull p then
                    first <- n
                    if isNull n then last <- null
                    else n.Prev <- null
                else
                    p.Value <- t.ops.mappend p.Value res
                    p.Next <- n
                    if isNull n then last <- p
                    else n.Prev <- p

            else 
                start.RefCount <- start.RefCount - 1

            res, n

        member x.Append(op : 'op) =
            // only append non-empty ops
            if not (t.ops.misEmpty op) then
                lock x (fun () ->
                    // apply the op to the state
                    let s, op = t.apply state op
                    state <- s

                    // if op got empty do not append it
                    if not (t.ops.misEmpty op) then

                        if isNull last || last.RefCount > 0 then
                            // if there is no last or last is relevant
                            // we need to create a new node
                            let n = RelevantNode(last, op, null)

                            // if there was no last we're the only element
                            if isNull last then first <- n
                            else last.Next <- n

                            // we're the new last
                            last <- n
                        else
                            // last is non-null and not relevant (no one pulled it)
                            // so we can append our op to it
                            last.Value <- t.ops.mappend last.Value op
                )

        member x.Read(caller : IAdaptiveObject, old : RelevantNode<'op>, oldState : 's) =
            x.EvaluateAlways caller (fun () ->
                if isNull old then
                    let ops = t.compute oldState state
                    let token = x.Append t.ops.mempty
                    token, ops
                else
                    let mutable res, current = mergeIntoLast old
                    let mutable prev = null

                    while not (isNull current) do
                        res <- t.ops.mappend res current.Value
                        prev <- current
                        current <- current.Next


                    if isNull prev then
                        let t = x.Append t.ops.mempty
                        t, res
                    else
                        prev.RefCount <- prev.RefCount + 1
                        prev, res
                        



                    
            )
        






type private HistoryEntry<'s, 'ops> = HeapEntry<uint64, WeakReference<HistoryReader<'s, 'ops>>>
and private HistoryHeap<'s, 'ops> = Heap<uint64, WeakReference<HistoryReader<'s, 'ops>>>

and History<'s, 'ops>(compute : History<'s, 'ops> -> 'ops, t : Traceable<'s, 'ops>) =
    inherit AdaptiveObject()

    let relevant = HistoryHeap<'s, 'ops>()
    let mutable minVersion = 0UL
    let mutable version = 0UL
    let mutable lastPullVersion = 0UL
    let mutable buffer : 'ops[] = Array.zeroCreate 16
    let mutable start = 0
    let mutable count = 0
    let mutable state = t.empty

    let bufferSize (cnt : int) =
        if cnt < 16 then 16
        else Fun.NextPowerOfTwo cnt

    let adjust (newMinVersion : Option<uint64>) =
        match newMinVersion with
            | Some newMinVersion ->
                if newMinVersion <> minVersion then
                    let removedVersions = int (newMinVersion - minVersion)
                    start <- (start + removedVersions) % buffer.Length
                    count <- count - removedVersions
                    minVersion <- newMinVersion

                    // shrink if possible
                    let newCap = bufferSize count
                    if newCap <> buffer.Length then
                        let arr = Array.zeroCreate newCap
                        let mutable index = start
                        for i in 0 .. count - 1 do
                            arr.[i] <- buffer.[index]
                            index <- (index + 1) % buffer.Length

                        buffer <- arr
                        start <- 0

            | None ->   
                minVersion <- 0UL
                version <- 0UL
                buffer <- Array.zeroCreate 16
                start <- 0
                count <- 0 

    let grow(cnt : int) =
        let newCapacity = bufferSize (count + cnt)
        if newCapacity <> buffer.Length then
            let arr = Array.zeroCreate newCapacity

            let mutable index = start
            for i in 0 .. count - 1 do
                arr.[i] <- buffer.[index]
                index <- (index + 1) % buffer.Length

            buffer <- arr
            start <- 0

    let rec collapse() =
        if relevant.Count > 0 then
            let state = 
                match relevant.Peek.Value.TryGetTarget() with
                    | (true, v) -> v.State
                    | _ -> t.empty

            if t.collapse state count then
                printfn "collapse"
                let minEntry = relevant.Dequeue()
                minEntry.Index <- -1
                adjust relevant.Min
                collapse()
    
    let perform (ops : 'ops) =
        if t.ops.misEmpty ops then
            false
        else
            let s, ops = t.apply state ops
            state <- s
            if t.ops.misEmpty ops then
                false

            else
                if relevant.Count <> 0 then
                    if count > 0 && lastPullVersion < version then
                        let i = (start + count - 1) % buffer.Length
                        buffer.[i] <- t.ops.mappend buffer.[i] ops
                        //Log.line "merged versions"
                    else
                        let mutable changed = false
                        grow 1
                        let i = (start + count) % buffer.Length
                        buffer.[i] <- ops
                        count <- count + 1
                        version <- version + 1UL
                        collapse()
                true

    new(t : Traceable<'s, 'ops>) = 
        let empty = t.ops.mempty
        History<'s, 'ops>((fun _ -> empty), t)

    member x.Traceable = t

    member x.Perform(ops : 'ops) =
        if lock x (fun () -> perform ops) then
            x.MarkOutdated()
            true
        else
            false

    member x.NewReader() : IOpReader<'s, 'ops> =
        lock x (fun () ->
            let entry = HeapEntry(version, Unchecked.defaultof<_>, -1) //.Enqueue(version, Unchecked.defaultof<_>)
            let r = new HistoryReader<'s, 'ops>(t.ops, x, t.empty, entry)
            entry.Value <- WeakReference<_>(r)
            r :> IOpReader<_,_>
        )

    member internal x.Remove(e : HistoryEntry<'s, 'ops>) =
        lock x (fun () ->
            relevant.Remove e |> ignore
            if relevant.Count > 0 then
                adjust relevant.Min
        )

    member internal x.GetOperationsSince(reader : HistoryReader<'s, 'ops>) : 's * 'ops =
        x.EvaluateAlways reader (fun () ->
            if x.OutOfDate then
                let ops = compute x
                perform ops |> ignore

            let old = reader.Entry
            lastPullVersion <- version

            if old.Index < 0 then
                let ops = t.compute reader.State state
                relevant.ChangeKey(old, version) |> ignore
                state, ops
            else
                let oldVersion = old.Key
                let cnt = int (version - oldVersion)
                let newMin = relevant.ChangeKey(old, version)
        
                let mutable index = (start + int (oldVersion - minVersion)) % buffer.Length
                let mutable operations = t.ops.mempty
                for _ in 1 .. cnt do
                    let res = buffer.[index]  
                    index <- (index + 1) % buffer.Length
                    operations <- t.ops.mappend operations res 
                    
                adjust (Some newMin)
                state, operations
        )

    member x.GetValue(caller : IAdaptiveObject) =
        x.EvaluateAlways caller (fun () ->
            if x.OutOfDate then
                let ops = compute x
                perform ops |> ignore    
           
            state
        )   

    member x.Count = count
    member x.State = state

    interface IMod with
        member x.IsConstant = false
        member x.GetValue c = x.GetValue c :> obj

    interface IMod<'s> with
        member x.GetValue c = x.GetValue c

and internal HistoryReader<'s, 'ops>(ops : Monoid<'ops>, history : History<'s, 'ops>, state : 's, entry : HistoryEntry<'s, 'ops>) =
    inherit AdaptiveObject()

    let mutable entry = Some entry
    let mutable state = state

    member x.Entry : HistoryEntry<'s, 'ops> = entry.Value

    member private x.Dispose(disposing : bool) = 
        if disposing then GC.SuppressFinalize x
        match entry with
            | Some e -> 
                let mutable foo = 0
                x.Outputs.Consume(&foo) |> ignore
                history.Remove e
                entry <- None
            | None ->
                ()

    member x.Dispose() = x.Dispose(true)

    member x.State : 's = state

    member x.GetOperations(caller : IAdaptiveObject) =
        x.EvaluateIfNeeded caller ops.mempty (fun () ->
            match entry with
                | Some e -> 
                    let s, ops = history.GetOperationsSince(x)
                    state <- s
                    ops
                | None ->
                    failwith "[Reader] cannot pull disposed reader"
        )

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface IOpReader<'s, 'ops> with
        member x.State = state
        member x.GetOperations c = x.GetOperations c

module History =

    module Readers =
        type EmptyReader<'s, 'ops>(t : Traceable<'s, 'ops>) =
            inherit ConstantObject()

            interface IOpReader<'ops> with
                member x.Dispose() = ()
                member x.GetOperations(caller) = t.ops.mempty
    
            interface IOpReader<'s, 'ops> with
                member x.State = t.empty

        type ConstantReader<'s, 'ops>(t : Traceable<'s, 'ops>, ops : Lazy<'ops>, finalState : Lazy<'s>) =
            inherit ConstantObject()
            
            let mutable state = t.empty
            let mutable initial = true

            interface IOpReader<'ops> with
                member x.Dispose() = ()
                member x.GetOperations(caller) =
                    lock x (fun () ->
                        if initial then
                            state <- finalState.Value
                            ops.Value
                        else
                            t.ops.mempty
                    )

            interface IOpReader<'s, 'ops> with
                member x.State = state
    
    let ofReader (t : Traceable<'s, 'ops>) (newReader : unit -> IOpReader<'ops>) =
        let reader = lazy (newReader())
        
        let compute(self : History<'s, 'ops>) =
            reader.Value.GetOperations(self)

        History<'s, 'ops>(compute, t)

