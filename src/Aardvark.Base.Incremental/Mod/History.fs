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
    abstract member GetOperations : AdaptiveToken -> 'ops

type IOpReader<'s, 'ops> =
    inherit IOpReader<'ops>
    abstract member State : 's

[<AbstractClass>]
type AbstractReader<'ops>(scope : Ag.Scope, t : Monoid<'ops>) =
    inherit AdaptiveObject()

    abstract member Release : unit -> unit
    abstract member Compute : AdaptiveToken -> 'ops

    abstract member Apply : 'ops -> 'ops
    default x.Apply o = o

    member x.GetOperations(token : AdaptiveToken) =
        x.EvaluateAlways token (fun token ->
            if x.OutOfDate then
                Ag.useScope scope (fun () -> 
                    x.Compute token |> x.Apply
                )
            else
                t.mempty
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
    inherit AbstractReader<'ops>(scope, t.tops)

    let mutable state = t.tempty

    override x.Apply o =
        let (s, o) = t.tapply state o
        state <- s
        o
    member x.State = state

    interface IOpReader<'s, 'ops> with
        member x.State = state

[<AbstractClass>]
type AbstractDirtyReader<'t, 'ops when 't :> IAdaptiveObject>(scope : Ag.Scope, t : Monoid<'ops>) =
    inherit DirtyTrackingAdaptiveObject<'t>()

    abstract member Release : unit -> unit
    abstract member Compute : AdaptiveToken * HashSet<'t> -> 'ops

    abstract member Apply : 'ops -> 'ops
    default x.Apply o = o

    member x.GetOperations(token : AdaptiveToken) =
        x.EvaluateAlways' token (fun token dirty ->
            if x.OutOfDate then
                Ag.useScope scope (fun () -> 
                    x.Compute(token, dirty) |> x.Apply
                )
            else
                t.mempty
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

[<AutoOpen>]
module NewHistory =

    [<AllowNullLiteral>]
    type RelevantNode<'s, 'a> =
        class
            val mutable public Prev : RelevantNode<'s, 'a>
            val mutable public Next : RelevantNode<'s, 'a>
            val mutable public RefCount : int
            val mutable public BaseState : 's
            val mutable public Value : 'a
            
            new(p, s, v, n) = { Prev = p; Next = n; RefCount = 0; BaseState = s; Value = v }
        end

    type History<'s, 'op> private(compute : Option<AdaptiveToken -> 'op>, t : Traceable<'s, 'op>) =
        inherit AdaptiveObject()


        let mutable state = t.tempty
        let mutable first   : RelevantNode<'s, 'op> = null
        let mutable last    : RelevantNode<'s, 'op> = null

        let rec tryCollapse (node : RelevantNode<'s, 'op>) =
            if t.tcollapse node.BaseState node.Value then
                let next = node.Next
                let prev = node.Prev
                node.Value <- Unchecked.defaultof<_>
                node.Prev <- null
                node.Next <- null
                node.RefCount <- -1

                
                if isNull next then last <- prev
                else next.Prev <- prev
                if isNull prev then first <- next
                else prev.Next <- next

                tryCollapse next

        let append (op : 'op) =
            // only append non-empty ops
            if not (t.tops.misEmpty op) then
                // apply the op to the state
                let s, op = t.tapply state op
                state <- s

                // if op got empty do not append it
                if not (t.tops.misEmpty op) then

                    // it last is null no reader is interested in ops.
                    // therefore we simply discard them here
                    if not (isNull last) then

                        // last is non-null and no one pulled it yet
                        // so we can append our op to it
                        last.Value <- t.tops.mappend last.Value op

                        tryCollapse first
                    true

                else
                    false
            else
                false

        let addRefToLast() =
            if isNull last then
                // if there is no last (the history is empty) we append
                // a new empty last with no ops and set its refcount to 1
                let n = RelevantNode(null, state, t.tops.mempty, null)
                n.RefCount <- 1
                last <- n
                first <- n
                n
            else
                
                if t.tops.misEmpty last.Value then
                    // if last has no ops we can reuse it here
                    last.RefCount <- last.RefCount + 1
                    last
                else
                    // if last contains ops we just consumed it and therefore
                    // need a new empty last
                    let n = RelevantNode(last, state, t.tops.mempty, null)
                    last.Next <- n
                    last <- n
                    n.RefCount <- 1
                    n
              
        let mergeIntoLast (node : RelevantNode<'s, 'op>) =
            if node.RefCount = 1 then
                let res = node.Value
                let next = node.Next
                let prev = node.Prev

                node.Value <- Unchecked.defaultof<_>
                node.Prev <- null
                node.Next <- null
                node.RefCount <- -1
                
                if isNull next then last <- prev
                else next.Prev <- prev

                if isNull prev then 
                    first <- next
                    res, next
                else 
                    prev.Next <- next
                    prev.Value <- t.tops.mappend prev.Value res
                    res, next

            else
                node.RefCount <- node.RefCount - 1
                node.Value, node.Next      


        let isInvalid (node : RelevantNode<'s, 'op>) =
            isNull node || node.RefCount < 0

        let isValid (node : RelevantNode<'s, 'op>) =
            not (isNull node) && node.RefCount >= 0

        member private x.Update (self : AdaptiveToken) =
            if x.OutOfDate then
                match compute with
                    | Some c -> 
                        let v = c self
                        append v |> ignore
                    | None ->
                        ()

        member x.State = state

        member x.Trace = t

        member x.Perform(op : 'op) =
            lock x (fun () ->
                let changed = append op
                if changed then x.MarkOutdated()
                changed
            )

        member x.Remove(token : RelevantNode<'s, 'op>) =
            lock x (fun () ->
                if isValid token then
                    mergeIntoLast token |> ignore
            )

        member x.Read(token : AdaptiveToken, old : RelevantNode<'s, 'op>, oldState : 's) =
            x.EvaluateWithCaptured token (fun cap token ->
                x.Update token

                match cap with
                    | Some lastNode ->
                        let lastNode = unbox<RelevantNode<'s, 'op>> lastNode

                        if isInvalid old then
                            let ops = t.tcompute oldState lastNode.BaseState
                            lastNode.RefCount <- lastNode.RefCount + 1
                            lastNode, ops
                        else
                            let mutable res, current = mergeIntoLast old

                            while current <> lastNode do
                                res <- t.tops.mappend res current.Value
                                current <- current.Next

                            lastNode.RefCount <- lastNode.RefCount + 1
                            lastNode, res

                    | None ->
                
                        if isInvalid old then
                            let ops = t.tcompute oldState state
                            let node = addRefToLast()

                            node, ops
                        else
                            let mutable res, current = mergeIntoLast old

                            while not (isNull current) do
                                res <- t.tops.mappend res current.Value
                                current <- current.Next

                            let node = addRefToLast()
                            node, res
            )
        
        member x.GetValue(token : AdaptiveToken) =
            x.EvaluateWithCaptured token (fun cap token ->
                match cap with
                    | Some lastNode ->
                        let lastNode = unbox<RelevantNode<'s, 'op>> lastNode
                        lastNode.BaseState
                    | None ->  
                        x.Update token
                        state
            )

        member x.NewReader() =
            new HistoryReader<'s, 'op>(x) :> IOpReader<'s, 'op>

        new (t : Traceable<'s, 'op>) = History<'s, 'op>(None, t)
        new (compute : AdaptiveToken -> 'op, t : Traceable<'s, 'op>) = History<'s, 'op>(Some compute, t)

        interface IMod with
            member x.IsConstant = false
            member x.GetValue c = x.GetValue c :> obj
            
        interface IMod<'s> with
            member x.GetValue c = x.GetValue c

    and HistoryReader<'s, 'op>(h : History<'s, 'op>) =
        inherit AdaptiveObject()
        let trace = h.Trace
        let mutable node : RelevantNode<'s, 'op> = null
        let mutable state = trace.tempty

        member x.GetOperations(token : AdaptiveToken) =
            x.EvaluateAlways token (fun token ->
                if x.OutOfDate then
                    let nt, ops = h.Read(token, node, state)
                    node <- nt
                    state <- h.State
                    ops
                else
                    trace.tops.mempty
            )

        member x.Dispose() =
            h.Outputs.Remove x |> ignore
            h.Remove node
            node <- null
            state <- trace.tempty

        interface IOpReader<'op> with
            member x.Dispose() = x.Dispose()
            member x.GetOperations c = x.GetOperations c

        interface IOpReader<'s, 'op> with
            member x.State = state


////[<AutoOpen>]
//module OldHistory = 
//    type private HistoryEntry<'s, 'ops> = HeapEntry<uint64, WeakReference<HistoryReader<'s, 'ops>>>
//    and private HistoryHeap<'s, 'ops> = Heap<uint64, WeakReference<HistoryReader<'s, 'ops>>>
//
//    and History<'s, 'ops>(compute : History<'s, 'ops> -> 'ops, t : Traceable<'s, 'ops>) =
//        inherit AdaptiveObject()
//
//        let relevant = HistoryHeap<'s, 'ops>()
//        let mutable minVersion = 0UL
//        let mutable version = 0UL
//        let mutable lastPullVersion = 0UL
//        let mutable buffer : 'ops[] = Array.zeroCreate 16
//        let mutable start = 0
//        let mutable count = 0
//        let mutable state = t.tempty
//
//        let bufferSize (cnt : int) =
//            if cnt < 16 then 16
//            else Fun.NextPowerOfTwo cnt
//
//        let adjust (newMinVersion : Option<uint64>) =
//            match newMinVersion with
//                | Some newMinVersion ->
//                    if newMinVersion <> minVersion then
//                        let removedVersions = int (newMinVersion - minVersion)
//                        start <- (start + removedVersions) % buffer.Length
//                        count <- count - removedVersions
//                        minVersion <- newMinVersion
//
//                        // shrink if possible
//                        let newCap = bufferSize count
//                        if newCap <> buffer.Length then
//                            let arr = Array.zeroCreate newCap
//                            let mutable index = start
//                            for i in 0 .. count - 1 do
//                                arr.[i] <- buffer.[index]
//                                index <- (index + 1) % buffer.Length
//
//                            buffer <- arr
//                            start <- 0
//
//                | None ->   
//                    minVersion <- 0UL
//                    version <- 0UL
//                    buffer <- Array.zeroCreate 16
//                    start <- 0
//                    count <- 0 
//
//        let grow(cnt : int) =
//            let newCapacity = bufferSize (count + cnt)
//            if newCapacity <> buffer.Length then
//                let arr = Array.zeroCreate newCapacity
//
//                let mutable index = start
//                for i in 0 .. count - 1 do
//                    arr.[i] <- buffer.[index]
//                    index <- (index + 1) % buffer.Length
//
//                buffer <- arr
//                start <- 0
//
//        let rec collapse() =
//            if relevant.Count > 0 then
//                let state = 
//                    match relevant.Peek.Value.TryGetTarget() with
//                        | (true, v) -> v.State
//                        | _ -> t.tempty
//
//                if t.tcollapse state t.tops.mempty then
//                    printfn "collapse"
//                    let minEntry = relevant.Dequeue()
//                    minEntry.Index <- -1
//                    adjust relevant.Min
//                    collapse()
//    
//        let perform (ops : 'ops) =
//            if t.tops.misEmpty ops then
//                false
//            else
//                let s, ops = t.tapply state ops
//                state <- s
//                if t.tops.misEmpty ops then
//                    false
//
//                else
//                    if relevant.Count <> 0 then
//                        if count > 0 && lastPullVersion < version then
//                            let i = (start + count - 1) % buffer.Length
//                            buffer.[i] <- t.tops.mappend buffer.[i] ops
//                            //Log.line "merged versions"
//                        else
//                            let mutable changed = false
//                            grow 1
//                            let i = (start + count) % buffer.Length
//                            buffer.[i] <- ops
//                            count <- count + 1
//                            version <- version + 1UL
//                            collapse()
//                    true
//
//        new(t : Traceable<'s, 'ops>) = 
//            let empty = t.tops.mempty
//            History<'s, 'ops>((fun _ -> empty), t)
//
//        member x.Traceable = t
//
//        member x.Perform(ops : 'ops) =
//            if lock x (fun () -> perform ops) then
//                x.MarkOutdated()
//                true
//            else
//                false
//
//        member x.NewReader() : IOpReader<'s, 'ops> =
//            lock x (fun () ->
//                let entry = HeapEntry(version, Unchecked.defaultof<_>, -1) //.Enqueue(version, Unchecked.defaultof<_>)
//                let r = new HistoryReader<'s, 'ops>(t.tops, x, t.tempty, entry)
//                entry.Value <- WeakReference<_>(r)
//                r :> IOpReader<_,_>
//            )
//
//        member internal x.Remove(e : HistoryEntry<'s, 'ops>) =
//            lock x (fun () ->
//                relevant.Remove e |> ignore
//                if relevant.Count > 0 then
//                    adjust relevant.Min
//            )
//
//        member internal x.GetOperationsSince(reader : HistoryReader<'s, 'ops>) : 's * 'ops =
//            x.EvaluateAlways reader (fun () ->
//                if x.OutOfDate then
//                    let ops = compute x
//                    perform ops |> ignore
//
//                let old = reader.Entry
//                lastPullVersion <- version
//
//                if old.Index < 0 then
//                    let ops = t.tcompute reader.State state
//                    relevant.ChangeKey(old, version) |> ignore
//                    state, ops
//                else
//                    let oldVersion = old.Key
//                    let cnt = int (version - oldVersion)
//                    let newMin = relevant.ChangeKey(old, version)
//        
//                    let mutable index = (start + int (oldVersion - minVersion)) % buffer.Length
//                    let mutable operations = t.tops.mempty
//                    for _ in 1 .. cnt do
//                        let res = buffer.[index]  
//                        index <- (index + 1) % buffer.Length
//                        operations <- t.tops.mappend operations res 
//                    
//                    adjust (Some newMin)
//                    state, operations
//            )
//
//        member x.GetValue(caller : IAdaptiveObject) =
//            x.EvaluateAlways caller (fun () ->
//                if x.OutOfDate then
//                    let ops = compute x
//                    perform ops |> ignore    
//           
//                state
//            )   
//
//        member x.Count = count
//        member x.State = state
//
//        interface IMod with
//            member x.IsConstant = false
//            member x.GetValue c = x.GetValue c :> obj
//
//        interface IMod<'s> with
//            member x.GetValue c = x.GetValue c
//
//    and internal HistoryReader<'s, 'ops>(ops : Monoid<'ops>, history : History<'s, 'ops>, state : 's, entry : HistoryEntry<'s, 'ops>) =
//        inherit AdaptiveObject()
//
//        let mutable entry = Some entry
//        let mutable state = state
//
//        member x.Entry : HistoryEntry<'s, 'ops> = entry.Value
//
//        member private x.Dispose(disposing : bool) = 
//            if disposing then GC.SuppressFinalize x
//            match entry with
//                | Some e -> 
//                    let mutable foo = 0
//                    x.Outputs.Consume(&foo) |> ignore
//                    history.Remove e
//                    entry <- None
//                | None ->
//                    ()
//
//        member x.Dispose() = x.Dispose(true)
//
//        member x.State : 's = state
//
//        member x.GetOperations(caller : IAdaptiveObject) =
//            x.EvaluateIfNeeded caller ops.mempty (fun () ->
//                match entry with
//                    | Some e -> 
//                        let s, ops = history.GetOperationsSince(x)
//                        state <- s
//                        ops
//                    | None ->
//                        failwith "[Reader] cannot pull disposed reader"
//            )
//
//        interface IDisposable with
//            member x.Dispose() = x.Dispose()
//
//        interface IOpReader<'s, 'ops> with
//            member x.State = state
//            member x.GetOperations c = x.GetOperations c

module History =

    module Readers =
        type EmptyReader<'s, 'ops>(t : Traceable<'s, 'ops>) =
            inherit ConstantObject()

            interface IOpReader<'ops> with
                member x.Dispose() = ()
                member x.GetOperations(caller) = t.tops.mempty
    
            interface IOpReader<'s, 'ops> with
                member x.State = t.tempty

        type ConstantReader<'s, 'ops>(t : Traceable<'s, 'ops>, ops : Lazy<'ops>, finalState : Lazy<'s>) =
            inherit ConstantObject()
            
            let mutable state = t.tempty
            let mutable initial = true

            interface IOpReader<'ops> with
                member x.Dispose() = ()
                member x.GetOperations(caller) =
                    lock x (fun () ->
                        if initial then
                            initial <- false
                            state <- finalState.Value
                            ops.Value
                        else
                            t.tops.mempty
                    )

            interface IOpReader<'s, 'ops> with
                member x.State = state
    
    let ofReader (t : Traceable<'s, 'ops>) (newReader : unit -> IOpReader<'ops>) =
        let reader = lazy (newReader())
        
        let compute(token : AdaptiveToken) =
            reader.Value.GetOperations(token)

        History<'s, 'ops>(compute, t)

