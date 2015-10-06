namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open Aardvark.Base
open System.Collections.Concurrent

/// <summary>
/// IAdaptiveObject represents the core interface for all
/// adaptive objects and contains everything necessary for
/// tracking OutOfDate flags and managing in-/outputs in the
/// dependency tree.
///
/// Since eager evalutation might be desirable in some scenarios
/// the interface also contains a Level representing the execution
/// order when evaluating inside a transaction and a function called
/// Mark allowing implementations to actually perform the evaluation.
/// Mark returns a bool since eager evaluation might cause the change
/// propagation process to exit early (if the actual value was unchanged)
/// In order to make adaptive objects easily identifiable all adaptive
/// objects must also provide a globally unique id (Id)
/// </summary>
[<AllowNullLiteral>]
type IAdaptiveObject =

    /// <summary>
    /// the globally unique id for the adaptive object
    /// </summary>
    abstract member Id : int

    /// <summary>
    /// the level for an adaptive object represents the
    /// maximal distance from an input cell in the depdency graph
    /// Note that this level is entirely managed by the system 
    /// and shall not be accessed directly by users of the system.
    /// </summary>
    abstract member Level : int with get, set

    /// <summary>
    /// Mark allows a specific implementation to
    /// evaluate the cell during the change propagation process.
    /// </summary>
    abstract member Mark : unit -> bool

    /// <summary>
    /// the outOfDate flag for the object is true
    /// whenever the object has been marked and shall
    /// be set to false by specific implementations.
    /// Note that this flag shall only be accessed when holding
    /// a lock on the adaptive object (allowing for concurrency)
    /// </summary>
    abstract member OutOfDate : bool with get, set

    /// <summary>
    /// the adaptive inputs for the object
    /// </summary>
    abstract member Inputs : ICollection<IAdaptiveObject>

    /// <summary>
    /// the adaptive outputs for the object which are recommended
    /// to be represented by Weak references in order to allow for
    /// unused parts of the graph to be garbage collected.
    /// </summary>
    abstract member Outputs : ICollection<IAdaptiveObject>

    /// <summary>
    /// a set of callbacks which shall be executed when the object
    /// is marked as OutOfDate for the next time. Note that this set 
    /// will be cleared once the callbacks have been executed.
    /// "persistent" callbacks must therefore re-subscribe themselves 
    /// upon every execution.
    /// </summary>
    abstract member MarkingCallbacks : ICollection<unit -> unit>

    abstract member InputChanged : IAdaptiveObject -> unit
    

/// <summary>
/// IncrementalLog logs a the sequence of fundamental adaptive operations
/// performed by the system. This allows to investigate bugs and concurrency
/// issues. Since this logging is relatively expensive it is disabled by default.
/// </summary>
module IncrementalLog =
    open System.Threading

    type Operation =
        | StartEvaluate = 1
        | EndEvaluate = 2
        | StartMark = 3
        | EndMark = 4

    let private log = List<Operation * int>()
    let private logLock = SpinLock()

    let append (op : Operation) (o : IAdaptiveObject) =
        #if DEBUG
        lock log (fun () ->
            log.Add(op,o.Id)
        )
        #else
        ()
        #endif

    let inline startEvaluate(o : IAdaptiveObject) =
        append Operation.StartEvaluate o

    let inline endEvaluate(o : IAdaptiveObject) =
        append Operation.EndEvaluate o

    let inline startMark(o : IAdaptiveObject) =
        append Operation.StartMark o

    let inline endMark(o : IAdaptiveObject) =
        append Operation.EndMark o

    let getLog() =
        log |> Seq.map (fun (op, o) -> 
            match op with
                | Operation.StartEvaluate -> sprintf "S(%d)" o
                | Operation.EndEvaluate -> sprintf "E(%d)" o
                | Operation.StartMark -> sprintf "MS(%d)" o
                | Operation.EndMark -> sprintf "ME(%d)" o
                | _ -> failwithf "unknown operation: %A" op
        ) |> String.concat "\r\n"

/// <summary>
/// LevelChangedException is internally used by the system
/// to handle level changes during the change propagation.
/// </summary>
exception LevelChangedException of IAdaptiveObject

[<AutoOpen>]
module private AdaptiveSystemState =
    let isTopLevel = new System.Threading.ThreadLocal<bool>(fun () -> true)

/// <summary>
/// Transaction holds a set of adaptive objects which
/// have been changed and shall therefore be marked as outOfDate.
/// Commit "propagates" these changes into the dependency-graph, takes
/// care of the correct execution-order and acquires appropriate locks
/// for all objects affected.
/// </summary>
type Transaction() =

    // each thread may have its own running transaction
    static let running = new Threading.ThreadLocal<Option<Transaction>>((fun () -> None),true)
    
    // we use a duplicate-queue here since we expect levels to be very similar 
    let q = DuplicatePriorityQueue<IAdaptiveObject, int>(fun o -> o.Level)
    let causes = Dict<IAdaptiveObject, HashSet<IAdaptiveObject>>()

    // the contained set is useful for determinig if an element has
    // already been enqueued
    let contained = HashSet<IAdaptiveObject>()
    let mutable current = None

    let getAndClear (set : ICollection<'a>) =
        let mutable content = []
        for e in set do content <- e::content
        set.Clear()
        content

    member x.IsContained e = contained.Contains e
    static member InAnyOfTheTransactionsInternal e =     
        let values = running.Values
        let b = List.init values.Count (fun i -> values.[i])
        b |> List.choose id |> List.exists (fun t -> t.IsContained e)

    static member Running =
        running.Value

    static member HasRunning =
        running.Value.IsSome
       
    /// <summary>
    /// enqueues an adaptive object for marking
    /// </summary>
    member x.Enqueue(e : IAdaptiveObject) =
        if contained.Add e then
            q.Enqueue e

    member x.Enqueue(e : IAdaptiveObject, cause : Option<IAdaptiveObject>) =
        if contained.Add e then
            q.Enqueue e
            match cause with
                | Some cause ->
                    match causes.TryGetValue e with
                        | (true, set) -> 
                            set.Add cause |> ignore
                        | _ ->
                            let set = HashSet [cause]
                            causes.[e] <- set
                | None -> ()

    member x.CurrentAdapiveObject = current
        

    /// <summary>
    /// performs the entire marking process causing
    /// all affected objects to be made consistent with
    /// the enqueued changes.
    /// </summary>
    member x.Commit() =
        let top = isTopLevel.Value
        isTopLevel.Value <- false


        // cache the currently running transaction (if any)
        // and make tourselves current.
        let old = running.Value
        running.Value <- Some x

        
        while q.Count > 0 do
            // dequeue the next element (having the minimal level)
            let l, e = q.Dequeue()
            current <- Some e

            

            let outputs = 
                // since we're about to access the outOfDate flag
                // for this object we must acquire a lock here.
                // Note that the transaction will at most hold one
                // lock at a time.
                lock e (fun () ->
                    // if the element is already outOfDate we
                    // do not traverse the graph further.
                    if e.OutOfDate then
                        Seq.empty

                    else
                        try
                            IncrementalLog.startMark e

                            // if the object's level has changed since it
                            // was added to the queue we re-enqueue it with the new level
                            // Note that this may of course cause runtime overhead and
                            // might even change the asymptotic runtime behaviour of the entire
                            // system in the worst case but we opted for this approach since
                            // it is relatively simple to implement.
                            if l <> e.Level then
                                q.Enqueue e
                                Seq.empty
                            else
                                match causes.TryRemove e with
                                    | (true, causes) -> causes |> Seq.iter e.InputChanged
                                    | _ -> ()

                                // however if the level is consistent we may proceed
                                // by marking the object as outOfDate
                                e.OutOfDate <- true
                
                                try 
                                    // here mark and the callbacks are allowed to evaluate
                                    // the adaptive object but must expect any call to AddOutput to 
                                    // raise a LevelChangedException whenever a level has been changed
                                    if e.Mark() then
                                        let mutable failed = false
                                        let callbacks = e.MarkingCallbacks |> getAndClear
                                        for cb in callbacks do 
                                            try cb()
                                            with :? LevelChangedException -> failed <- true
                                        if failed then raise <| LevelChangedException e

                                        // if everything succeeded we return all current outputs
                                        // which will cause them to be enqueued 
                                        e.Outputs :> seq<IAdaptiveObject>

                                    else
                                        // if Mark told us not to continue we're done here
                                        Seq.empty

                                with :? LevelChangedException ->
                                    // if the level was changed either by a callback
                                    // or Mark we re-enqueue the object with the new level and
                                    // mark it upToDate again (since it would otherwise not be processed again)
                                    e.OutOfDate <- false
                                    q.Enqueue e
                                    Seq.empty
                        finally
                            IncrementalLog.endMark e
                )

            // finally we enqueue all returned outputs
            for o in outputs do
                o.InputChanged e
                x.Enqueue o

            contained.Remove e |> ignore
            current <- None
            

        // when the commit is over we restore the old
        // running transaction (if any)
        running.Value <- old
        isTopLevel.Value <- top

/// <summary>
/// defines a base class for all adaptive objects implementing
/// IAdaptiveObject.
/// </summary>
type AdaptiveObject() =
    let id = newId()
    let mutable outOfDate = true
    let mutable level = 0
    let inputs = HashSet<IAdaptiveObject>() :> ICollection<_>
    let outputs = WeakSet<IAdaptiveObject>() :> ICollection<_>
    let callbacks = ConcurrentHashSet<unit -> unit>() :> ICollection<_>

    static let time = AdaptiveObject() :> IAdaptiveObject

    static member Time = time

    /// <summary>
    /// utility function for evaluating an object if
    /// it is marked as outOfDate. If the object is actually
    /// outOfDate the given function is executed and otherwise
    /// the given default value is returned.
    /// Note that this function takes care of appropriate locking
    /// </summary>
    member x.EvaluateIfNeeded (otherwise : 'a) (f : unit -> 'a) =
        let top = isTopLevel.Value
        if top then isTopLevel.Value <- false

        let res =
            lock x (fun () ->
                if x.OutOfDate then
                    IncrementalLog.startEvaluate x
                    let r = f()
                    x.OutOfDate <- false
                    IncrementalLog.endEvaluate x
                    r
                else
                    otherwise
            )

        if top then 
            isTopLevel.Value <- true
            if time.Outputs.Count > 0 then
                let t = Transaction()
                for o in time.Outputs do
                    t.Enqueue(o)
                t.Commit()

        res

    /// <summary>
    /// utility function for evaluating an object even if it
    /// is not marked as outOfDate.
    /// Note that this function takes care of appropriate locking
    /// </summary>
    member x.EvaluateAlways (f : unit -> 'a) =
        let top = isTopLevel.Value
        if top then isTopLevel.Value <- false

        let res =
            lock x (fun () ->
                IncrementalLog.startEvaluate x
                let res = f()
                x.OutOfDate <- false
                IncrementalLog.endEvaluate x
                res
            )

        if top then 
            isTopLevel.Value <- true
            if time.Outputs.Count > 0 then
                let t = Transaction()
                for o in time.Outputs do
                    t.Enqueue(o)
                t.Commit()

        res


    member x.Id = id
    member x.OutOfDate
        with get() = outOfDate
        and set v = outOfDate <- v

    member x.Outputs = outputs
    member x.Inputs = inputs
    member x.MarkingCallbacks = callbacks
    member x.Level 
        with get() = level
        and set l = level <- l

    abstract member Mark : unit -> bool
    default x.Mark () = true
    
    abstract member InputChanged : IAdaptiveObject -> unit
    default x.InputChanged ip = ()

    override x.GetHashCode() = id
    override x.Equals o =
        match o with
            | :? IAdaptiveObject as o -> id = o.Id
            | _ -> false

    interface IAdaptiveObject with
        member x.Id = id
        member x.OutOfDate
            with get() = outOfDate
            and set v = outOfDate <- v

        member x.Outputs = outputs
        member x.Inputs = inputs
        member x.MarkingCallbacks = callbacks
        member x.Level 
            with get() = level
            and set l = level <- l

        member x.Mark () =
            x.Mark ()

        member x.InputChanged ip = x.InputChanged ip

/// <summary>
/// defines a base class for all decorated mods
/// </summary>
type AdaptiveDecorator(o : IAdaptiveObject) =
    let id = newId()
    
    member x.Id = id
    member x.OutOfDate
        with get() = o.OutOfDate
        and set v = o.OutOfDate <- v

    member x.Outputs = o.Outputs
    member x.Inputs = o.Inputs
    member x.MarkingCallbacks = o.MarkingCallbacks
    member x.Level 
        with get() = o.Level
        and set l = o.Level <- l

    member x.Mark() = o.Mark()

    override x.GetHashCode() = o.GetHashCode()
    override x.Equals o =
        match o with
            | :? IAdaptiveObject as o -> x.Id = o.Id
            | _ -> false

    interface IAdaptiveObject with
        member x.Id = id
        member x.OutOfDate
            with get() = o.OutOfDate
            and set v = o.OutOfDate <- v

        member x.Outputs = o.Outputs
        member x.Inputs = o.Inputs
        member x.MarkingCallbacks = o.MarkingCallbacks
        member x.Level 
            with get() = o.Level
            and set l = o.Level <- l

        member x.Mark () = o.Mark()

        member x.InputChanged ip = o.InputChanged ip

/// <summary>
/// defines a base class for all adaptive objects which are
/// actually constant.
/// Note that this class provides "dummy" implementations
/// for all memebers defined in IAdaptiveObject and does not 
/// keep track of in-/outputs.
/// </summary>
[<AbstractClass>]
type ConstantObject() =
    static let emptySet = EmptyCollection<IAdaptiveObject>() :> ICollection<_>
    static let emptyCallbacks = EmptyCollection<unit -> unit>() :> ICollection<_>

    interface IAdaptiveObject with
        member x.Id = -1
        member x.Level
            with get() = 0
            and set l = failwith "cannot set level for constant"

        member x.Mark() = false
        member x.OutOfDate
            with get() = false
            and set o = failwith "cannot mark constant outOfDate"

        member x.Inputs = emptySet
        member x.Outputs = emptySet
        member x.MarkingCallbacks = emptyCallbacks
        member x.InputChanged ip = ()


and EmptyCollection<'a>() =
    interface ICollection<'a> with
        member x.Add _ = ()
        member x.Clear() = ()
        member x.Count = 0
        member x.Contains _ = false
        member x.Remove _ = false
        member x.CopyTo(arr, idx) = ()
        member x.IsReadOnly = false
        member x.GetEnumerator() : IEnumerator<'a> = Seq.empty.GetEnumerator()
        member x.GetEnumerator() : System.Collections.IEnumerator = Seq.empty.GetEnumerator() :> _


type DirtySet<'a, 'b when 'a :> IAdaptiveObject and 'a : not struct>(evaluate : seq<'a> -> 'b) =
    let l = obj()
    let all = HashSet<'a>()
    let mutable dirty = HashSet<'a>()
    let subscriptions = Dictionary<IAdaptiveObject, unit -> unit>()


    let addDirty (v : 'a) () =
        lock l (fun () ->
            dirty.Add v |> ignore
        )

    member x.Evaluate() =
        let mine = 
            lock l (fun () ->
                let arr = dirty |> Seq.toArray
                dirty <- HashSet<'a>()
                arr
            )

        evaluate mine


    member x.Add(v : 'a) =
        lock v (fun () ->
            lock l (fun () ->
                if all.Add v && v.OutOfDate then
                    dirty.Add v |> ignore
                else
                    let cb = addDirty v
                    v.MarkingCallbacks.Add cb
                    subscriptions.Add(v, cb)
            )   
        )

    member x.Remove(v : 'a) =
        lock v (fun () ->
            lock l (fun () ->
                if all.Remove v then
                    dirty.Remove v |> ignore
                    match subscriptions.TryGetValue v with
                        | (true, cb) -> v.MarkingCallbacks.Remove cb |> ignore
                        | _ -> ()
            )   
        )

    member x.Dispose() =
        lock l (fun () ->
            all.Clear()
            dirty.Clear()
            for (KeyValue(k,v)) in subscriptions do
                k.MarkingCallbacks.Remove v |> ignore
        )

    interface IDisposable with
        member x.Dispose() = x.Dispose()






[<AutoOpen>]
module Marking =

    // changes the level of an adaptive object if it is below
    // minLevel and transitively changes all outputs if needed.
    // Note that this implementation has very poor runtime performance
    // and might possibly be improved using some kind of order-maintenance
    // structure instead of integers.
    let rec relabel (m : IAdaptiveObject) (minLevel : int) =
        let old = m.Level
        if old < minLevel then
            m.Level <- minLevel
            for o in m.Outputs do
                relabel o (minLevel + 1) |> ignore
            old <> -1
        else
            false

    // since changeable inputs need a transaction
    // for enqueing their changes we use a thread local 
    // current transaction which basically allows for 
    // an implicit argument.
    let private current = new Threading.ThreadLocal<Option<Transaction>>(fun () -> None)

    /// <summary>
    /// returns the currently running transaction or (if none)
    /// the current transaction for the calling thread
    /// </summary>
    let getCurrentTransaction() =
        match Transaction.Running with
            | Some r -> Some r
            | None ->
                match current.Value with
                    | Some c -> Some c
                    | None -> None

    let setCurrentTransaction t =
        current.Value <- t

    /// <summary>
    /// executes a function "inside" a newly created
    /// transaction and commits the transaction
    /// </summary>
    let transact (f : unit -> 'a) =
        let t = Transaction()
        let old = current.Value
        current.Value <- Some t
        let r = f()
        current.Value <- old
        t.Commit()
        r

    // defines some extension utilites for
    // IAdaptiveObjects
    type IAdaptiveObject with
        /// <summary>
        /// utility for marking adaptive object as outOfDate.
        /// Note that this function will actually enqueue the
        /// object to the current transaction and will fail if
        /// no current transaction can be found.
        /// However objects which are already outOfDate might
        /// also be "marked" when not having a current transaction.
        /// </summary>
        member x.MarkOutdated (cause : Option<IAdaptiveObject>) =
            match getCurrentTransaction() with
                | Some t -> t.Enqueue(x, cause)
                | None -> 
                    lock x (fun () -> 
                        if x.OutOfDate then ()
                        else failwith "cannot mark object without transaction"
                    )

        member x.MarkOutdated () =
            x.MarkOutdated None
                            
        /// <summary>
        /// utility for adding an output to the object.
        /// Note that this will cause the output to be marked
        /// using MarkOutdated and may therefore only be used
        /// on objects being outOfDate or inside a transaction.
        /// </summary>
        member x.AddOutput(m : IAdaptiveObject) =
            m.Inputs.Add x |> ignore
            x.Outputs.Add m |> ignore

            // if the element was actually relabeled and we're
            // currently inside a running transaction we need to
            // raise a LevelChangedException.
            if relabel m (x.Level + 1) then
                match Transaction.Running with
                    | Some t ->
                        match t.CurrentAdapiveObject with
                            | Some m' when m = m' -> raise <| LevelChangedException m
                            | _ -> ()
                    | _ -> ()

            m.MarkOutdated ( Some x )

        /// <summary>
        /// utility for removing an output from the object
        /// </summary>
        member x.RemoveOutput (m : IAdaptiveObject) =
            m.Inputs.Remove x |> ignore
            x.Outputs.Remove m |> ignore

        /// <summary>
        /// utility for adding a "persistent" callback to
        /// the object. returns a disposable "subscription" which
        /// allows to destroy the callback.
        /// </summary>
        member x.AddMarkingCallback(f : unit -> unit) =
            let live = ref true
            let self = ref id
            self := fun () ->
                if !live then
                    f()
                    x.MarkingCallbacks.Add(!self) |> ignore

            lock x (fun () ->
                x.MarkingCallbacks.Add(!self) |> ignore
            )

            { new IDisposable with member __.Dispose() = live := false; x.MarkingCallbacks.Remove !self |> ignore}
 

open System.Threading
 

 
type VolatileDirtySet<'a, 'b when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    let mutable set : PersistentHashSet<'a> = PersistentHashSet.empty

    member x.Evaluate() =
        let local = Interlocked.Exchange(&set, PersistentHashSet.empty) 
        try
            local |> PersistentHashSet.toList
                    |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                    |> List.map (fun o -> eval o)

        with :? LevelChangedException as l ->
            Interlocked.Change(&set, PersistentHashSet.union local)
            raise l

    member x.Push(i : 'a) =
        lock i (fun () ->
            if i.OutOfDate then
                Interlocked.Change(&set, PersistentHashSet.add i)
        )

    member x.Add(i : 'a) =
        x.Push(i)

    member x.Remove(i : 'a) =
        Interlocked.Change(&set, PersistentHashSet.remove i)
 
    member x.Clear() =
        Interlocked.Exchange(&set, PersistentHashSet.empty) |> ignore

type VolatileTaggedDirtySet<'a, 'b, 't when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    let mutable set : PersistentHashSet<'a> = PersistentHashSet.empty
    let tagDict = Dictionary<'a, HashSet<'t>>()

    member x.Evaluate() =
        let local = Interlocked.Exchange(&set, PersistentHashSet.empty) 
        try
            local |> PersistentHashSet.toList
                  |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                  |> List.map (fun o ->
                        match tagDict.TryGetValue o with
                            | (true, tags) -> o, Seq.toList tags
                            | _ -> o, []
                     )
                  |> List.map (fun (o, tags) -> eval o, tags)

        with :? LevelChangedException as l ->
            Interlocked.Change(&set, PersistentHashSet.union local)
            raise l

    member x.Push(i : 'a) =
        lock i (fun () ->
            if i.OutOfDate then
                Interlocked.Change(&set, PersistentHashSet.add i)
        )

    member x.Add(tag : 't, i : 'a) =
        match tagDict.TryGetValue i with
            | (true, set) -> 
                set.Add tag |> ignore
                false
            | _ ->
                tagDict.[i] <- HashSet [tag]
                x.Push i
                true

    member x.Remove(tag : 't, i : 'a) =
        match tagDict.TryGetValue i with
            | (true, tags) -> 
                if tags.Remove tag then
                    if tags.Count = 0 then
                        Interlocked.Change(&set, PersistentHashSet.remove i)  
                        true
                    else
                        false
                else
                    failwithf "[VolatileTaggedDirtySet] could not remove tag %A for element %A" tag i
                                      
            | _ ->
                failwithf "[VolatileTaggedDirtySet] could not remove element: %A" i

    member x.Clear() =
        tagDict.Clear()
        Interlocked.Exchange(&set, PersistentHashSet.empty) |> ignore


type DirtySetNewWithData<'a, 'b, 'x when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    inherit AdaptiveObject()

    let mutable data = Dict<'a, HashSet<'x>>()
    let mutable set : PersistentHashSet<'a> = PersistentHashSet.empty

    member x.Evaluate() =
        x.EvaluateIfNeeded [] (fun () ->
            let local = Interlocked.Exchange(&set, PersistentHashSet.empty) 
            try
                local |> PersistentHashSet.toList
                      |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                      |> List.map (fun o -> 
                        match data.TryGetValue o with
                            | (true, v) -> (eval o,Seq.toList v)
                            | _ -> (eval o, [])
                      )
            with :? LevelChangedException as l ->
                Interlocked.Change(&set, PersistentHashSet.union local)
                raise l
        )

    override x.InputChanged (o : IAdaptiveObject) =
        match o with
            | :? 'a as a -> Interlocked.Change(&set, PersistentHashSet.add a)
            | _ -> failwithf "unexpected input-change: %A" o

    member x.Listen (tag : 'x, i : 'a)  =
        lock x (fun () ->
            match data.TryGetValue i with
                | (true, tags) -> tags.Add tag |> ignore
                | _ ->
                    let tags = HashSet [tag]
                    data.[i] <- tags

                    i.AddOutput x
                    lock i (fun () ->
                        if i.OutOfDate then
                            Interlocked.Change(&set, PersistentHashSet.add i)
                    )
        )

    member x.Unlisten (tag : 'x, i : 'a) =
        lock x (fun () ->
            match data.TryGetValue i with
                | (true, tags) ->
                    if not <| tags.Remove tag then failwith "DirtySet: could not remove tag"

                    if tags.Count = 0 then
                        data.Remove i |> ignore
                        i.RemoveOutput x
                        Interlocked.Change(&set, PersistentHashSet.remove i)

                | _ -> failwith "DirtySet: could not remove input"
        )
                
    member x.Dispose() =
        let ips = x.Inputs |> Seq.toList
        for i in ips do i.RemoveOutput x

module Validation =
    
    open System.Threading

//    member x.EvaluateAlways (f : unit -> 'a) =
//        let top = isTopLevel.Value
//        if top then isTopLevel.Value <- false
//
//        let res =
//            lock x (fun () ->
//                IncrementalLog.startEvaluate x
//                let res = f()
//                x.OutOfDate <- false
//                IncrementalLog.endEvaluate x
//                res
//            )
//
//        if top then 
//            isTopLevel.Value <- true
//            if time.Outputs.Count > 0 then
//                let t = Transaction()
//                for o in time.Outputs do
//                    t.Enqueue(o)
//                t.Commit()
//
//        res

    type IAdaptiveObject with
        
        member x.Validate() =
            lock x (fun () ->
                try
                    x.Inputs |> Seq.iter Monitor.Enter

                    if not x.OutOfDate then
                        let invalid = x.Inputs |> Seq.exists (fun i -> i.OutOfDate) && not x.OutOfDate
                        if invalid then
                            let iAmInTransaction = Transaction.InAnyOfTheTransactionsInternal x
                            //let inTransacList.exists Transaction.AllRunning.
                            if not iAmInTransaction then
                                System.Diagnostics.Debugger.Break()
                                failwith "invalid state" 


                finally
                    x.Inputs |> Seq.iter Monitor.Exit
            )
            x.Inputs |> Seq.iter (fun i -> i.Validate())