namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open Aardvark.Base
open System.Collections.Concurrent
open System.Threading
open System.Linq
open Aardvark.Base.Incremental.Telemetry

[<AllowNullLiteral>]
type IWeakable<'a when 'a : not struct> =
    abstract member Weak : WeakReference<'a>

type VolatileCollection<'a when 'a :> IWeakable<'a> and 'a : not struct>() =
    let mutable set : HashSet<WeakReference<'a>> = null
    let mutable pset : List<WeakReference<'a>> = List()

    let mutable v = Unchecked.defaultof<_>

    member x.IsEmpty = 
        if isNull set then pset.Count = 0
        else set.Count = 0

    member x.Consume(length : byref<int>) : array<'a> =
        if isNull set then
            let res = Array.zeroCreate pset.Count
            length <- 0
            for i in 0 .. pset.Count - 1 do
                if pset.[i].TryGetTarget(&v) then
                    res.[length] <- v; 
                    length <- length + 1
            v <- Unchecked.defaultof<_>
            pset.Clear()
            res
        else
            let res = Array.zeroCreate set.Count
            length <- 0
            for e in set do
                if e.TryGetTarget(&v) then
                    res.[length] <- v; 
                    length <- length + 1
            v <- Unchecked.defaultof<_>
            set.Clear()
            res

    member x.Add(value : 'a) : bool =
        let value = value.Weak
        if isNull set then 
            let id = pset.IndexOf(value)
            if id < 0 then 
                pset.Add(value)
                if pset.Count > 8 then
                    set <- HashSet pset
                    pset <- null
                true
            else 
                false
        else
            set.Add value
        

    member x.Remove(value : 'a) : bool =
        let value = value.Weak
        if isNull set then 
            pset.Remove(value)
        else
            set.Remove value

type VolatileCollectionStrong<'a>() =
    let mutable set : HashSet<'a> = null
    let mutable pset : List<'a> = List()

    member x.IsEmpty = 
        if isNull set then pset.Count = 0
        else set.Count = 0

    member x.Consume(length : byref<int>) : array<'a> =
        if isNull set then
            let res = pset.ToArray()
            length <- res.Length
            pset.Clear()
            res
        else
            let res = set.ToArray()
            length <- res.Length
            set.Clear()
            res

    member x.Add(value : 'a) : bool =
        if isNull set then 
            let id = pset.IndexOf(value)
            if id < 0 then 
                pset.Add(value)
                if pset.Count > 8 then
                    set <- HashSet pset
                    pset <- null
                true
            else 
                false
        else
            set.Add value
        

    member x.Remove(value : 'a) : bool =
        if isNull set then 
            pset.Remove(value)
        else
            set.Remove value

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
    inherit IWeakable<IAdaptiveObject>

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
    abstract member Inputs : seq<IAdaptiveObject>

    /// <summary>
    /// the adaptive outputs for the object which are recommended
    /// to be represented by Weak references in order to allow for
    /// unused parts of the graph to be garbage collected.
    /// </summary>
    abstract member Outputs : VolatileCollection<IAdaptiveObject>


    abstract member InputChanged : obj * IAdaptiveObject -> unit
    abstract member AllInputsProcessed : obj -> unit


    abstract member Lock : AdaptiveLock

/// <summary>
/// LevelChangedException is internally used by the system
/// to handle level changes during the change propagation.
/// </summary>
exception LevelChangedException of changedObject : IAdaptiveObject * newLevel : int * distanceFromRoot : int

module AdaptiveSystemState =
    let curerntEvaluationDepth = new ThreadLocal<ref<int>>(fun _ -> ref 0)

    let private currentLocks = new ThreadLocal<ref<list<AdaptiveLock>>>(fun () -> ref [])

    let addReadLock (l : AdaptiveLock) =
        let r = currentLocks.Value
        r := l::!r

    let pushReadLocks() =
        let r = currentLocks.Value
        let old = !r
        r := []
        old

    let popReadLocks (old : list<AdaptiveLock>) =
        let r = currentLocks.Value
        let v = !r
        r := old
        for l in v do l.ExitRead()





    //let currentEvaluationPath = new ThreadLocal<Stack<IAdaptiveObject>>(fun _ -> Stack(100))

type TrackAllThreadLocal<'a>(creator : unit -> 'a) =
    let mutable values : Map<int, 'a> = Map.empty

    let create() =
        let v = creator()
        Threading.Interlocked.Change(&values, Map.add Threading.Thread.CurrentThread.ManagedThreadId v) |> ignore
        v

    let inner = new Threading.ThreadLocal<'a>(create)

    member x.Value
        with get() = inner.Value
        and set v =
            inner.Value <- v
            Threading.Interlocked.Change(&values, Map.add Threading.Thread.CurrentThread.ManagedThreadId v) |> ignore

    member x.Values =
        values |> Map.toSeq |> Seq.map snd

    member x.Dispose() =
        values <- Map.empty
        inner.Dispose()

    interface IDisposable with
        member x.Dispose() = x.Dispose()


/// <summary>
/// Transaction holds a set of adaptive objects which
/// have been changed and shall therefore be marked as outOfDate.
/// Commit "propagates" these changes into the dependency-graph, takes
/// care of the correct execution-order and acquires appropriate locks
/// for all objects affected.
/// </summary>
type Transaction() =
    static let EnqueueProbe = Symbol.Create "[Transaction] Enqueue"
    static let CommitProbe = Symbol.Create "[Transaction] Commit"
    static let emptyArray : IAdaptiveObject[] = Array.empty
    let mutable outputs = emptyArray

    // each thread may have its own running transaction
    static let running = new TrackAllThreadLocal<Option<Transaction>>(fun () -> None)
    
    // we use a duplicate-queue here since we expect levels to be very similar 
    let q = DuplicatePriorityQueue<IAdaptiveObject, int>(fun o -> o.Level)
    let causes = Dict<IAdaptiveObject, HashSet<IAdaptiveObject>>()

    // the contained set is useful for determinig if an element has
    // already been enqueued
    let contained = HashSet<IAdaptiveObject>()
    let mutable current : IAdaptiveObject = null
    let mutable currentLevel = 0

    let getAndClear (set : ICollection<'a>) =
        let mutable content = []
        for e in set do content <- e::content
        set.Clear()
        content

    member x.IsContained e = contained.Contains e
    static member internal InAnyOfTheTransactionsInternal e =
        running.Values 
            |> Seq.toList 
            |> List.choose id 
            |> List.exists (fun t -> t.IsContained e)

    static member Running
        with get() = running.Value
        and set r = running.Value <- r

    static member HasRunning =
        running.Value.IsSome
       
    static member RunningLevel =
        match running.Value with
            | Some t -> t.CurrentLevel
            | _ -> Int32.MaxValue - 1


    member x.CurrentLevel = currentLevel

    /// <summary>
    /// enqueues an adaptive object for marking
    /// </summary>
    member x.Enqueue(e : IAdaptiveObject) =
        Telemetry.timed EnqueueProbe (fun () ->
            if contained.Add e then
                q.Enqueue e
        )

    member x.Enqueue(e : IAdaptiveObject, cause : Option<IAdaptiveObject>) =
        Telemetry.timed EnqueueProbe (fun () ->
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
        )

    member x.CurrentAdapiveObject = 
        if isNull current then None
        else Some current
        

    /// <summary>
    /// performs the entire marking process causing
    /// all affected objects to be made consistent with
    /// the enqueued changes.
    /// </summary>
    member x.Commit() =
        Telemetry.timed CommitProbe (fun () ->
            // cache the currently running transaction (if any)
            // and make tourselves current.
            let old = running.Value
            running.Value <- Some x
            let mutable level = 0
            let myCauses = ref null
        
            let markCount = ref 0
            let traverseCount = ref 0
            let levelChangeCount = ref 0
            let outputCount = ref 0
            while q.Count > 0 do
                // dequeue the next element (having the minimal level)
                let e = q.Dequeue(&currentLevel)
                current <- e

                traverseCount := !traverseCount + 1

                outputCount := 0


                // since we're about to access the outOfDate flag
                // for this object we must acquire a lock here.
                // Note that the transaction will at most hold one
                // lock at a time.
                //Monitor.Enter e
                e.Lock.EnterWrite(e)
                try
                    // if the element is already outOfDate we
                    // do not traverse the graph further.
                    if e.OutOfDate then
                        outputCount := 0
                        e.AllInputsProcessed(x)

                    else
                        

                        // if the object's level has changed since it
                        // was added to the queue we re-enqueue it with the new level
                        // Note that this may of course cause runtime overhead and
                        // might even change the asymptotic runtime behaviour of the entire
                        // system in the worst case but we opted for this approach since
                        // it is relatively simple to implement.
                        if currentLevel <> e.Level then
                            q.Enqueue e
                            outputCount := 0
                        else
                            if causes.TryRemove(e, &myCauses.contents) then
                                !myCauses |> Seq.iter (fun i -> e.InputChanged(x,i))

                            // however if the level is consistent we may proceed
                            // by marking the object as outOfDate
                            e.OutOfDate <- true
                            e.AllInputsProcessed(x)
                            markCount := !markCount + 1
                
                            try 
                                // here mark and the callbacks are allowed to evaluate
                                // the adaptive object but must expect any call to AddOutput to 
                                // raise a LevelChangedException whenever a level has been changed
                                if e.Mark() then
                                    // if everything succeeded we return all current outputs
                                    // which will cause them to be enqueued 
                                    outputs <- e.Outputs.Consume(outputCount)

                                else
                                    // if Mark told us not to continue we're done here
                                    outputCount := 0

                            with LevelChangedException(obj, objLevel, distance) ->
                                // if the level was changed either by a callback
                                // or Mark we re-enqueue the object with the new level and
                                // mark it upToDate again (since it would otherwise not be processed again)
                                e.Level <- max e.Level (objLevel + distance)
                                e.OutOfDate <- false

                                levelChangeCount := !levelChangeCount + 1

                                q.Enqueue e
                                outputCount := 0
                
                finally 
                    e.Lock.ExitWrite(e)
                    //Monitor.Exit e

                // finally we enqueue all returned outputs
                for i in 0..!outputCount - 1 do
                    let o = outputs.[i]
                    o.InputChanged(x,e)
                    x.Enqueue o

                contained.Remove e |> ignore
                current <- null
            


            // when the commit is over we restore the old
            // running transaction (if any)
            running.Value <- old
            currentLevel <- 0
        )



type private EmptyCollection<'a>() =
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


/// <summary>
/// defines a base class for all adaptive objects implementing
/// IAdaptiveObject.
/// </summary>
[<AllowNullLiteral>]
type AdaptiveObject =
    class
        [<DefaultValue>]
        static val mutable private time : IAdaptiveObject 
        val mutable public Id : int
        val mutable public OutOfDate : bool
        val mutable public Level : int 
        val mutable public Outputs : VolatileCollection<IAdaptiveObject>
        val mutable public WeakThis : WeakReference<IAdaptiveObject>
        val mutable public Lock : AdaptiveLock

        new() =
            { Id = newId(); OutOfDate = true; 
              Level = 0; Outputs = VolatileCollection<IAdaptiveObject>(); WeakThis = null;
              Lock = new AdaptiveLock() }

    
        member inline this.evaluate (caller : IAdaptiveObject) (otherwise : Option<'a>) (f : unit -> 'a) =
            let depth = AdaptiveSystemState.curerntEvaluationDepth.Value
            let top = isNull caller && !depth = 0 && not Transaction.HasRunning

            let oldLocks =
                if top then AdaptiveSystemState.pushReadLocks()
                else []

            let mutable res = Unchecked.defaultof<_>
            this.Lock.EnterRead this

            AdaptiveSystemState.addReadLock this.Lock

            depth := !depth + 1

            try
                match otherwise with
                    | Some v when not this.OutOfDate -> 
                        res <- v
                    | _ ->
                        // this evaluation is performed optimistically
                        // meaning that the "top-level" object needs to be allowed to
                        // pull at least one value on every path.
                        // This property must therefore be maintained for every
                        // path in the entire system.
                        let r = f()
                        this.OutOfDate <- false

                        // if the object's level just got greater than or equal to
                        // the level of the running transaction (if any)
                        // we raise an exception since the evaluation
                        // could be inconsistent atm.
                        // the only exception to that is the top-level object itself
                        let maxAllowedLevel =
                            if !depth > 1 then Transaction.RunningLevel - 1
                            else Transaction.RunningLevel

                        if this.Level > maxAllowedLevel then
                            //printfn "%A tried to pull from level %A but has level %A" top.Id level top.Level
                            // all greater pulls would be from the future
                            raise <| LevelChangedException(this, this.Level, !depth - 1)
                            
                        res <- r


                if not (isNull caller) then
                    this.Outputs.Add caller |> ignore
                    caller.Level <- max caller.Level (this.Level + 1)


            finally
                depth := !depth - 1
                this.Lock.Downgrade this

            if top then 
                AdaptiveSystemState.popReadLocks oldLocks
                let time = AdaptiveObject.Time
                Monitor.Enter time
                if not time.Outputs.IsEmpty then
                    let mutable outputCount = 0
                    let outputs = time.Outputs.Consume(&outputCount)
                    Monitor.Exit time

                    let t = Transaction()
                    for i in 0..outputCount-1 do
                        let o = outputs.[i]
                        t.Enqueue(o)
                    t.Commit()
                else
                    Monitor.Exit time

            res


        static member Time : IAdaptiveObject = 
            if isNull AdaptiveObject.time then
                AdaptiveObject.time <- AdaptiveObject() :> IAdaptiveObject
            AdaptiveObject.time


        /// <summary>
        /// utility function for evaluating an object if
        /// it is marked as outOfDate. If the object is actually
        /// outOfDate the given function is executed and otherwise
        /// the given default value is returned.
        /// Note that this function takes care of appropriate locking
        /// </summary>
        member inline x.EvaluateIfNeeded (caller : IAdaptiveObject) (otherwise : 'a) (f : unit -> 'a) =
            x.evaluate caller (Some otherwise) f

        /// <summary>
        /// utility function for evaluating an object even if it
        /// is not marked as outOfDate.
        /// Note that this function takes care of appropriate locking
        /// </summary>
        member inline x.EvaluateAlways (caller : IAdaptiveObject) (f : unit -> 'a) =
            x.evaluate caller None f

        abstract member Mark : unit -> bool
        default x.Mark () = true
    
        abstract member InputChanged : obj * IAdaptiveObject -> unit
        default x.InputChanged(t,ip) = ()

        abstract member AllInputsProcessed : obj -> unit
        default x.AllInputsProcessed(t) = ()


        abstract member Inputs : seq<IAdaptiveObject>
        [<System.ComponentModel.Browsable(false)>]
        default x.Inputs = Seq.empty

        override x.GetHashCode() = x.Id
        override x.Equals o =
            match o with
                | :? IAdaptiveObject as o -> x.Id = o.Id
                | _ -> false

        member x.Weak =
            let o = x.WeakThis
            if isNull o then
                let r = new WeakReference<IAdaptiveObject>(x)
                x.WeakThis <- r
                r
            else
                o

        interface IWeakable<IAdaptiveObject> with
            member x.Weak = x.Weak

        interface IAdaptiveObject with
            member x.Id = x.Id
            member x.OutOfDate
                with get() = x.OutOfDate
                and set v = x.OutOfDate <- v

            member x.Outputs = x.Outputs
            member x.Inputs = x.Inputs
            member x.Level 
                with get() = x.Level
                and set l = x.Level <- l

            member x.Mark () =
                x.Mark ()

            member x.InputChanged(o,ip) = x.InputChanged(o, ip)
            member x.AllInputsProcessed(o) = x.AllInputsProcessed(o)
            member x.Lock = x.Lock

    end

/// <summary>
/// defines a base class for all adaptive objects implementing
/// IAdaptiveObject and providing dirty-inputs for evaluation.
/// </summary>
[<AllowNullLiteral>]
type DirtyTrackingAdaptiveObject<'a when 'a :> IAdaptiveObject> =
    class 
        inherit AdaptiveObject

        val mutable public Scratch : Dict<obj, HashSet<'a>>
        val mutable public Dirty : HashSet<'a>

        override x.InputChanged(t,o) =
            match o with
                | :? 'a as o ->
                    lock x.Scratch (fun () ->
                        let set = x.Scratch.GetOrCreate(t, fun t -> HashSet())
                        set.Add o |> ignore
                    )
                | _ -> ()

        override x.AllInputsProcessed(t) =
            match lock x.Scratch (fun () -> x.Scratch.TryRemove t) with
                | (true, s) -> x.Dirty.UnionWith s
                | _ -> ()



        member x.EvaluateIfNeeded' (caller : IAdaptiveObject) (otherwise : 'b) (compute : HashSet<'a> -> 'b) =
            x.EvaluateIfNeeded caller otherwise (fun () ->
                let d = x.Dirty
                let res = compute d
                d.Clear()
                res
            )

        member x.EvaluateAlways' (caller : IAdaptiveObject) (compute : HashSet<'a> -> 'b) =
            x.EvaluateAlways caller (fun () ->
                let d = x.Dirty
                let res = compute d
                d.Clear()
                res
            )

        new() = { Scratch = Dict(); Dirty = HashSet() }


    end




/// <summary>
/// defines a base class for all adaptive objects which are
/// actually constant.
/// Note that this class provides "dummy" implementations
/// for all memebers defined in IAdaptiveObject and does not 
/// keep track of in-/outputs.
/// </summary>
[<AbstractClass>]
type ConstantObject() =

    let mutable weakThis : WeakReference<IAdaptiveObject> = null
    let lock = new AdaptiveLock()

    interface IWeakable<IAdaptiveObject> with
        member x.Weak =
            let w = weakThis
            if isNull w then 
                let w = WeakReference<IAdaptiveObject>(x)
                weakThis <- w
                w
            else
                weakThis


    interface IAdaptiveObject with
        member x.Id = -1
        member x.Level
            with get() = 0
            and set l = failwith "cannot set level for constant"

        member x.Mark() = false
        member x.OutOfDate
            with get() = false
            and set o = failwith "cannot mark constant outOfDate"

        member x.Inputs = Seq.empty
        member x.Outputs = VolatileCollection()
        member x.InputChanged(o,ip) = ()
        member x.AllInputsProcessed(o) = ()
        member x.Lock = lock




[<AutoOpen>]
module Marking =



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
                        elif x.Outputs.IsEmpty then x.OutOfDate <- true
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
            m.MarkOutdated ( Some x )

        /// <summary>
        /// utility for removing an output from the object
        /// </summary>
        member x.RemoveOutput (m : IAdaptiveObject) =
            lock x (fun () -> x.Outputs.Remove m |> ignore)

[<AutoOpen>]
module CallbackExtensions =
    
    let private undyingMarkingCallbacks = System.Runtime.CompilerServices.ConditionalWeakTable<IAdaptiveObject,HashSet<obj>>()

    type private CallbackObject(inner : IAdaptiveObject, callback : CallbackObject -> unit) as this =

        let modId = newId()
        let mutable level = inner.Level + 1
        let mutable live = 1
        let mutable scope = Ag.getContext()
        let mutable inner = inner
        let mutable weakThis = null
        let rw = new AdaptiveLock()
        do lock inner (fun () -> inner.Outputs.Add this |> ignore)

        do lock undyingMarkingCallbacks (fun () -> undyingMarkingCallbacks.GetOrCreateValue(inner).Add this |> ignore )

        member x.Mark() =
            Ag.useScope scope (fun () ->
                callback x
            )
            false

        interface IWeakable<IAdaptiveObject> with
            member x.Weak =
                let w = weakThis
                if isNull w then 
                    let w = WeakReference<IAdaptiveObject>(x)
                    weakThis <- w
                    w
                else
                    weakThis

        interface IAdaptiveObject with
            member x.Id = modId
            member x.Level
                with get() = System.Int32.MaxValue - 1
                and set l = ()

            member x.Mark() =
                x.Mark()

            member x.OutOfDate
                with get() = false
                and set o = ()

            member x.Inputs = Seq.singleton inner
            member x.Outputs = VolatileCollection()
            member x.InputChanged(o,ip) = ()
            member x.AllInputsProcessed(o) = ()
            member x.Lock = rw

        member x.Dispose() =
            if Interlocked.Exchange(&live, 0) = 1 then
                lock undyingMarkingCallbacks (fun () -> 
                    match undyingMarkingCallbacks.TryGetValue(inner) with
                        | (true,v) -> 
                            v.Remove x |> ignore
                            if v.Count = 0 then undyingMarkingCallbacks.Remove inner |> ignore
                        | _ -> ()
                )
                inner.RemoveOutput x
                scope <- Unchecked.defaultof<_>
                inner <- null

        interface IDisposable with
            member x.Dispose() = x.Dispose()

    type IAdaptiveObject with

        /// <summary>
        /// utility for adding a "persistent" callback to
        /// the object. returns a disposable "subscription" which
        /// allows to destroy the callback.
        /// </summary>
        member x.AddMarkingCallback(f : unit -> unit) =
            let res =
                new CallbackObject(x, fun self ->
                    try
                        f ()
                    finally 
                        lock x (fun () -> x.Outputs.Add self |> ignore)
                )

            lock x (fun () -> x.Outputs.Add res |> ignore)

            res :> IDisposable //{ new IDisposable with member __.Dispose() = live := false; x.MarkingCallbacks.Remove !self |> ignore}
 
        /// <summary>
        /// utility for adding a "persistent" callback to
        /// the object. returns a disposable "subscription" which
        /// allows to destroy the callback.
        /// </summary>
        member x.AddVolatileMarkingCallback(f : unit -> unit) =
            let res =
                new CallbackObject(x, fun self ->
                    try
                        f ()
                        self.Dispose()
                    with :? LevelChangedException as ex ->
                        lock x (fun () -> x.Outputs.Add self |> ignore)
                        raise ex

                )

            lock x (fun () -> x.Outputs.Add res |> ignore)

            res :> IDisposable //{ new IDisposable with member __.Dispose() = live := false; x.MarkingCallbacks.Remove !self |> ignore}
 

        member x.AddEvaluationCallback(f : IAdaptiveObject -> unit) =
            let res = 
                new CallbackObject(x, fun self ->
                    try
                        f self
                    finally 
                        lock x (fun () -> x.Outputs.Add self |> ignore)
                )

            res.Mark() |> ignore

            res :> IDisposable //{ new IDisposable with member __.Dispose() = live := false; x.MarkingCallbacks.Remove !self |> ignore}
 


open System.Threading
 
/// <summary>
/// defines a base class for all decorated mods
/// </summary>
type AdaptiveDecorator(o : IAdaptiveObject) =
    let mutable o = o
    let id = newId()
    let mutable weakThis = null

    member x.Id = id
    member x.OutOfDate
        with get() = o.OutOfDate
        and set v = o.OutOfDate <- v

    member x.Outputs = o.Outputs
    member x.Inputs = o.Inputs
    member x.Level 
        with get() = o.Level
        and set l = o.Level <- l

    member x.Mark() = o.Mark()

    override x.GetHashCode() = id
    override x.Equals o =
        match o with
            | :? IAdaptiveObject as o -> x.Id = id
            | _ -> false

    interface IWeakable<IAdaptiveObject> with
        member x.Weak =
            let w = weakThis
            if isNull w then 
                let w = WeakReference<IAdaptiveObject>(x)
                weakThis <- w
                w
            else
                weakThis


    interface IAdaptiveObject with
        member x.Id = id
        member x.OutOfDate
            with get() = o.OutOfDate
            and set v = o.OutOfDate <- v

        member x.Outputs = o.Outputs
        member x.Inputs = o.Inputs
        member x.Level 
            with get() = o.Level
            and set l = o.Level <- l

        member x.Mark () = o.Mark()

        member x.InputChanged(t,ip) = o.InputChanged (t,ip)
        member x.AllInputsProcessed(t) = o.AllInputsProcessed(t)
        member x.Lock = o.Lock
 
type VolatileDirtySet<'a, 'b when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    let mutable set : PersistentHashSet<'a> = PersistentHashSet.empty

    member x.Evaluate() =
        let local = Interlocked.Exchange(&set, PersistentHashSet.empty) 
        try
            local |> PersistentHashSet.toList
                    |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                    |> List.map (fun o -> eval o)

        with :? LevelChangedException as l ->
            Interlocked.Change(&set, PersistentHashSet.union local) |> ignore
            raise l

    member x.Push(i : 'a) =
        lock i (fun () ->
            if i.OutOfDate then
                Interlocked.Change(&set, PersistentHashSet.add i) |> ignore
        )

    member x.Add(i : 'a) =
        x.Push(i)

    member x.Remove(i : 'a) =
        Interlocked.Change(&set, PersistentHashSet.remove i) |> ignore
 
    member x.Clear() =
        Interlocked.Exchange(&set, PersistentHashSet.empty) |> ignore

type MutableVolatileDirtySet<'a, 'b when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    let lockObj = obj()
    let set = HashSet<'a>()

    member x.Evaluate() =
        lock lockObj (fun () ->
            let res = set |> Seq.toList
            set.Clear()
            res |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                |> List.map (fun o -> eval o)
        )

    member x.Push(i : 'a) =
        lock lockObj (fun () ->
            lock i (fun () ->
                if i.OutOfDate then
                    set.Add i |> ignore
            )
        )

    member x.Add(i : 'a) =
        x.Push(i)

    member x.Remove(i : 'a) =
        lock lockObj (fun () ->
            set.Remove i |> ignore
        )
 
    member x.Clear() =
        lock lockObj (fun () ->
            set.Clear()
        )


type VolatileTaggedDirtySet<'a, 'b, 't when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    let mutable set : PersistentHashSet<'a> = PersistentHashSet.empty
    let tagDict = Dictionary<'a, HashSet<'t>>()

    member x.Evaluate() =
        lock tagDict (fun () ->
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
                Interlocked.Change(&set, PersistentHashSet.union local) |> ignore
                raise l
        )

    member x.Push(i : 'a) =
        lock tagDict (fun () ->
            lock i (fun () ->
                if i.OutOfDate && tagDict.ContainsKey i then
                    Interlocked.Change(&set, PersistentHashSet.add i) |> ignore
            )
        )

    member x.Add(tag : 't, i : 'a) =
        lock tagDict (fun () ->
            match tagDict.TryGetValue i with
                | (true, set) -> 
                    set.Add tag |> ignore
                    false
                | _ ->
                    tagDict.[i] <- HashSet [tag]
                    x.Push i
                    true
        )

    member x.Remove(tag : 't, i : 'a) =
        lock tagDict (fun () ->
            match tagDict.TryGetValue i with
                | (true, tags) -> 
                    if tags.Remove tag then
                        if tags.Count = 0 then
                            Interlocked.Change(&set, PersistentHashSet.remove i) |> ignore
                            true
                        else
                            false
                    else
                        failwithf "[VolatileTaggedDirtySet] could not remove tag %A for element %A" tag i
                                      
                | _ ->
                    failwithf "[VolatileTaggedDirtySet] could not remove element: %A" i
        )

    member x.Clear() =
        lock tagDict (fun () ->
            tagDict.Clear()
            Interlocked.Exchange(&set, PersistentHashSet.empty) |> ignore
        )

type MutableVolatileTaggedDirtySet<'a, 'b, 't when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    let set = HashSet<'a>()
    let tagDict = Dictionary<'a, HashSet<'t>>()

    member x.Evaluate() =
        lock tagDict (fun () ->
            try
                let result = 
                    set |> Seq.toList
                        |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                        |> List.choose (fun o ->
                              match tagDict.TryGetValue o with
                                  | (true, tags) -> Some(o, Seq.toList tags)
                                  | _ -> None
                           )
                        |> List.map (fun (o, tags) -> eval o, tags)

                set.Clear()
                result

            with :? LevelChangedException as l ->
                raise l
        )

    member x.Push(i : 'a) =
        lock tagDict (fun () ->
            lock i (fun () ->
                if i.OutOfDate && tagDict.ContainsKey i then
                    set.Add i |> ignore
            )
        )

    member x.Add(tag : 't, i : 'a) =
        lock tagDict (fun () ->
            match tagDict.TryGetValue i with
                | (true, set) -> 
                    set.Add tag |> ignore
                    false
                | _ ->
                    tagDict.[i] <- HashSet [tag]
                    x.Push i
                    true
        )

    member x.Remove(tag : 't, i : 'a) =
        lock tagDict (fun () ->
            match tagDict.TryGetValue i with
                | (true, tags) -> 
                    if tags.Remove tag then
                        if tags.Count = 0 then
                            set.Remove i |> ignore
                            true
                        else
                            false
                    else
                        failwithf "[VolatileTaggedDirtySet] could not remove tag %A for element %A" tag i
                                      
                | _ ->
                    failwithf "[VolatileTaggedDirtySet] could not remove element: %A" i
        )

    member x.Clear() =
        lock tagDict (fun () ->
            tagDict.Clear()
            set.Clear()
        )
