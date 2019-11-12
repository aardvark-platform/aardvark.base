namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base
open System.Collections.Concurrent
open System.Threading
open System.Linq
open System.Runtime.InteropServices

[<AutoOpen>]
module VolatileCollection = 
    [<Literal>]
    let SetThreshold = 8

[<AllowNullLiteral>]
type IWeakable<'a when 'a : not struct> =
    abstract member Weak : WeakReference<'a>

#nowarn "9"
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

    abstract member Reevaluate : bool with get, set

    /// <summary>
    /// the adaptive inputs for the object
    /// </summary>
    abstract member Inputs : seq<IAdaptiveObject>

    /// <summary>
    /// the adaptive outputs for the object which are recommended
    /// to be represented by Weak references in order to allow for
    /// unused parts of the graph to be garbage collected.
    /// </summary>
    abstract member Outputs : VolatileCollection


    abstract member InputChanged : obj * IAdaptiveObject -> unit
    abstract member AllInputsProcessed : obj -> unit

    abstract member ReaderCount : int with get, set


and [<StructLayout(LayoutKind.Explicit)>] MultiPtr =
    struct
        [<FieldOffset(0)>]
        val mutable public Single : WeakReference<IAdaptiveObject>
        [<FieldOffset(0)>]
        val mutable public Array : WeakReference<IAdaptiveObject>[]
        [<FieldOffset(0)>]
        val mutable public Set : HashSet<WeakReference<IAdaptiveObject>>
    end

/// <summary>
/// Collection of WeakReferences using a switching internal representation: Single(0-1), List (2-8), HashSet(>8)
/// This collection has improved performance to VolatileCollectionOld in cases where there are 0 or 1 references
/// NOTE: using mode switch omits using types checks to determine used implementation
///       using matches instead of downcasts uses OpCode "isinst" instead of "unbox.any" and avoids code to be able to throw exception
/// </summary>
and VolatileCollection() =
    
    let mutable handles : MultiPtr = Unchecked.defaultof<_>
    let mutable count : int = 0 // Note: could replace mode, but would require Array or HashSet to be dropped on Consume
    let mutable mode : byte = 0uy // using mode to avoid "expensive" type checks

    // NOTE: myIndexOf > Array.IndexOf<_>(arr, value, 0, count) > Array.IndexOf(arr, value, 0, count)
    let myIndexOf(arr : WeakReference<_>[], v : WeakReference<_>, count : int) : int =
        let mutable i = 0
        let mutable search = true
        while search && i < count do
            if Object.ReferenceEquals(arr.[i], v) then
                search <- false
            else
                i <- i + 1
        if search then
            -1
        else
            i

    member x.IsEmpty = 
        count = 0
        
    member x.Count = 
        count
            
    member x.Consume(res : byref<array<IAdaptiveObject>>) : int =
        let mutable length = 0
        if not (isNull handles.Single) then 
            let mutable v : IAdaptiveObject = Unchecked.defaultof<_>
            if mode = 0uy then
                if res.Length < 1 then
                    res <- Array.zeroCreate SetThreshold
                if handles.Single.TryGetTarget(&v) then
                    res.[0] <- v
                    length <- 1
                handles.Single <- Unchecked.defaultof<_>
            elif mode = 1uy then
                let arr = handles.Array
                if res.Length < count then
                    res <- Array.zeroCreate SetThreshold
                for i in 0..count-1 do
                    if arr.[i].TryGetTarget(&v) then
                        res.[length] <- v 
                        length <- length + 1
                    arr.[i] <- Unchecked.defaultof<_>
            else
                let set = handles.Set
                if res.Length < set.Count then
                    res <- Array.zeroCreate (set.Count * 3 / 2)
                for ref in set do 
                    if ref.TryGetTarget(&v) then
                        res.[length] <- v
                        length <- length + 1
                set.Clear()
        count <- 0
        length

    member x.Add(value : IAdaptiveObject) : bool =
        let value = value.Weak
        if mode = 0uy then
            if isNull handles.Single then
                handles.Single <- value
                count <- 1
                true
            elif Object.ReferenceEquals(handles.Single, value) then
                false
            else
                let arr = Array.zeroCreate SetThreshold
                arr.[0] <- handles.Single
                arr.[1] <- value
                handles.Array <- arr
                mode <- 1uy
                count <- 2
                true
        elif mode = 1uy then 
            let arr = handles.Array
            if myIndexOf(arr, value, count) >= 0 then
                false
            elif count = SetThreshold then
                let set = HashSet arr
                set.Add(value) |> ignore
                handles.Set <- set
                mode <- 2uy
                count <- SetThreshold + 1
                true
            else
                arr.[count] <- value
                count <- count + 1
                true
        else
            if handles.Set.Add(value) then
                count <- count + 1
                true
            else
                false

    member x.Remove(value : IAdaptiveObject) : bool =
        let mutable res = false
        if count > 0 then
            let value = value.Weak
            if mode = 0uy then
                if Object.ReferenceEquals(handles.Single, value) then
                    handles.Single <- null
                    count <- 0
                    res <- true
            elif mode = 1uy then
                let arr = handles.Array;
                let i = myIndexOf(arr, value, count)
                if i >= 0 then
                    for j in i..count-2 do
                        arr.[j] <- arr.[j+1]
                    count <- count - 1
                    res <- true
            else
                if handles.Set.Remove value then
                    count <- count - 1
                    res <- true
        res

    member x.Clear() =
        handles.Single <- null
        count <- 0
        mode <- 0uy
     
#endnowarn "9"

[<AbstractClass; Sealed; Extension>]
type AdaptiveObjectExtensions private() =

    static let equality =
        { new IEqualityComparer<IAdaptiveObject> with
            member x.GetHashCode(o : IAdaptiveObject) =
                System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(o)

            member x.Equals(l : IAdaptiveObject, r : IAdaptiveObject) =
                System.Object.ReferenceEquals(l,r)
        }

    static member EqualityComparer = equality

    [<Extension>]
    static member EnterWrite(o : IAdaptiveObject) =
        Monitor.Enter o
        while o.ReaderCount > 0 do
            Monitor.Wait o |> ignore
            
    [<Extension>]
    static member ExitWrite(o : IAdaptiveObject) =
        Monitor.Exit o
        
    [<Extension>]
    static member IsOutdatedCaller(o : IAdaptiveObject) =
        Monitor.IsEntered o && o.OutOfDate

type AdaptiveToken =
    struct
        val mutable public Caller : IAdaptiveObject
        val mutable public Locked : HashSet<IAdaptiveObject>

        member inline x.EnterRead(o : IAdaptiveObject) =
            Monitor.Enter o
                
        member inline x.ExitFaultedRead(o : IAdaptiveObject) =
            Monitor.Exit o

        member inline x.Downgrade(o : IAdaptiveObject) =
            if not o.Reevaluate && x.Locked.Add o then
                o.ReaderCount <- o.ReaderCount + 1
            Monitor.Exit o

        member inline x.ExitRead(o : IAdaptiveObject) =
            if x.Locked.Remove o then
                lock o (fun () ->
                    let rc = o.ReaderCount - 1
                    o.ReaderCount <- rc
                    if rc = 0 then Monitor.PulseAll o
                )

        member inline x.Release() =
            for o in x.Locked do
                lock o (fun () ->
                    let rc = o.ReaderCount - 1
                    o.ReaderCount <- rc
                    if rc = 0 then Monitor.PulseAll o
                )
            x.Locked.Clear()



        member inline x.WithCaller (c : IAdaptiveObject) =
            AdaptiveToken(c, x.Locked)

        member inline x.WithTag (t : obj) =
            AdaptiveToken(x.Caller, x.Locked)


        member inline x.Isolated =
            AdaptiveToken(x.Caller, HashSet())

        static member inline Top = AdaptiveToken(null, HashSet())
        static member inline Empty = Unchecked.defaultof<AdaptiveToken>

        new(caller : IAdaptiveObject, locked : HashSet<IAdaptiveObject>) =
            {
                Caller = caller
                Locked = locked
            }
    end

/// <summary>
/// LevelChangedException is internally used by the system
/// to handle level changes during the change propagation.
/// </summary>
exception LevelChangedException of changedObject : IAdaptiveObject * newLevel : int * distanceFromRoot : int

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

    // each thread may have its own running transaction
    [<ThreadStatic; DefaultValue>]
    static val mutable private RunningTransaction : Option<Transaction>

    [<ThreadStatic; DefaultValue>]
    static val mutable private CurrentTransaction : Option<Transaction>

    #if DEBUG
    let mutable isDisposed = false
    #endif


//    // each thread may have its own running transaction
//    static let running = new TrackAllThreadLocal<Option<Transaction>>(fun () -> None)
//    
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
        
    let mutable finalizers : list<unit->unit> = []

    let runFinalizers () =
        let fs = Interlocked.Exchange(&finalizers, [])
        for f in fs do f()
        
    member x.AddFinalizer (f : unit->unit) =
        Interlocked.Change(&finalizers, (fun a -> f::a) ) |> ignore

    member x.IsContained e = contained.Contains e
//    static member internal InAnyOfTheTransactionsInternal e =
//        running.Values 
//            |> Seq.toList 
//            |> List.choose id 
//            |> List.exists (fun t -> t.IsContained e)

    static member Running
        with get() = Transaction.RunningTransaction
        and set r = Transaction.RunningTransaction <- r

    static member Current
        with get() = Transaction.CurrentTransaction
        and set r = Transaction.CurrentTransaction <- r

    static member HasRunning =
        Transaction.RunningTransaction.IsSome
       
    static member RunningLevel =
        match Transaction.RunningTransaction with
            | Some t -> t.CurrentLevel
            | _ -> Int32.MaxValue - 1


    member x.CurrentLevel = currentLevel

    /// <summary>
    /// enqueues an adaptive object for marking
    /// </summary>
    member x.Enqueue(e : IAdaptiveObject) =
        #if DEBUG
        if isDisposed then failwith "Invalid Enqueue! Transaction already disposed."
        #endif
        
        if contained.Add e then
            q.Enqueue e

    member x.Enqueue(e : IAdaptiveObject, cause : Option<IAdaptiveObject>) =
        #if DEBUG
        if isDisposed then failwith "Invalid Enqueue! Transaction already disposed."
        #endif
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

    member x.CurrentAdapiveObject = 
        if isNull current then None
        else Some current
        

    /// <summary>
    /// performs the entire marking process causing
    /// all affected objects to be made consistent with
    /// the enqueued changes.
    /// </summary>
    member x.Commit() =

        #if DEBUG
        if isDisposed then failwith "Invalid Commit Transaction already disposed."
        #endif


        // cache the currently running transaction (if any)
        // and make ourselves current.
        let old = Transaction.RunningTransaction
        Transaction.RunningTransaction <- Some x
        let mutable level = 0
        let myCauses = ref null
        
        let mutable markCount = 0
        let mutable traverseCount = 0
        let mutable levelChangeCount = 0
        let mutable outputCount = 0
        let mutable outputs = Array.zeroCreate 8
        while q.Count > 0 do
            // dequeue the next element (having the minimal level)
            let e = q.Dequeue(&currentLevel)
            current <- e

            traverseCount <- traverseCount + 1

            outputCount <- 0


            // since we're about to access the outOfDate flag
            // for this object we must acquire a lock here.
            // Note that the transaction will at most hold one
            // lock at a time.
            //Monitor.Enter e

            if e.IsOutdatedCaller() then
                e.AllInputsProcessed(x)

            else
                e.EnterWrite()
                try
                    // if the element is already outOfDate we
                    // do not traverse the graph further.
                    if e.OutOfDate then
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
                        else
                            if causes.TryRemove(e, &myCauses.contents) then
                                !myCauses |> Seq.iter (fun i -> e.InputChanged(x,i))

                            // however if the level is consistent we may proceed
                            // by marking the object as outOfDate
                            e.OutOfDate <- true
                            e.AllInputsProcessed(x)
                            markCount <- markCount + 1
                
                            try 
                                // here mark and the callbacks are allowed to evaluate
                                // the adaptive object but must expect any call to AddOutput to 
                                // raise a LevelChangedException whenever a level has been changed
                                if e.Mark() then
                                    // if everything succeeded we return all current outputs
                                    // which will cause them to be enqueued 
                                    outputCount <- e.Outputs.Consume(&outputs)

                                else
                                    // if Mark told us not to continue we're done here
                                    ()

                            with LevelChangedException(obj, objLevel, distance) ->
                                // if the level was changed either by a callback
                                // or Mark we re-enqueue the object with the new level and
                                // mark it upToDate again (since it would otherwise not be processed again)
                                e.Level <- max e.Level (objLevel + distance)
                                e.OutOfDate <- false

                                levelChangeCount <- levelChangeCount + 1

                                q.Enqueue e
                
                finally 
                    e.ExitWrite()

                // finally we enqueue all returned outputs
                for i in 0..outputCount - 1 do
                    let o = outputs.[i]
                    o.InputChanged(x,e)
                    x.Enqueue o

            contained.Remove e |> ignore
            current <- null
            


        // when the commit is over we restore the old
        // running transaction (if any)
        Transaction.RunningTransaction <- old
        currentLevel <- 0


    member x.Dispose() = 
        runFinalizers()
        #if DEBUG
        isDisposed <- true
        #endif

    interface IDisposable with
        member x.Dispose() = x.Dispose()


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

        [<DefaultValue; ThreadStatic>]
        static val mutable private EvaluationDepthValue : int

        val mutable public Id : int
        val mutable public OutOfDateValue : bool
        val mutable public LevelValue : int 
        val mutable public Outputs : VolatileCollection
        val mutable public WeakThis : WeakReference<IAdaptiveObject>
        val mutable public ReaderCountValue : int
        val mutable public Reevaluate : bool

        /// used for reseting EvaluationDepth in eager evaluation
        static member internal UnsaveEvaluationDepth
            with get() = AdaptiveObject.EvaluationDepthValue
            and set v = AdaptiveObject.EvaluationDepthValue <- v

        member x.Level
            with inline get() = if x.Reevaluate then 0 else x.LevelValue
            and inline set v = if not x.Reevaluate then x.LevelValue <- v

        member x.OutOfDate 
            with inline get() = 
                x.Reevaluate || x.OutOfDateValue
            and inline set v = 
                if not x.Reevaluate then 
                    x.OutOfDateValue <- v

        new() =
            { Id = newId(); OutOfDateValue = true; 
              LevelValue = 0; Outputs = VolatileCollection(); WeakThis = null;
              ReaderCountValue = 0; Reevaluate = false }

        static member inline private markTime() =
            let time = AdaptiveObject.time
            if not (isNull time) then
                Monitor.Enter time
                if not time.Outputs.IsEmpty then
                    let mutable outputs = Array.zeroCreate (VolatileCollection.SetThreshold)
                    let outputCount = time.Outputs.Consume(&outputs)
                    Monitor.Exit time

                    let t = new Transaction()
                    for i in 0..outputCount-1 do
                        let o = outputs.[i]
                        t.Enqueue(o)
                    t.Commit()
                    t.Dispose()
                else
                     Monitor.Exit time

        member this.evaluate (token : AdaptiveToken) (f : AdaptiveToken -> 'a) =
            let depth = AdaptiveObject.EvaluationDepthValue

            let mutable res = Unchecked.defaultof<_>
            token.EnterRead this

            this.Reevaluate <- false

            try
                AdaptiveObject.EvaluationDepthValue <- depth + 1

                // this evaluation is performed optimistically
                // meaning that the "top-level" object needs to be allowed to
                // pull at least one value on every path.
                // This property must therefore be maintained for every
                // path in the entire system.
                res <- f(token.WithCaller this)
                this.OutOfDate <- false

                // if the object's level just got greater than or equal to
                // the level of the running transaction (if any)
                // we raise an exception since the evaluation
                // could be inconsistent atm.
                // the only exception to that is the top-level object itself
                let maxAllowedLevel =
                    if depth > 1 then Transaction.RunningLevel - 1
                    else Transaction.RunningLevel

                if this.Level > maxAllowedLevel then
                    //printfn "%A tried to pull from level %A but has level %A" top.Id level top.Level
                    // all greater pulls would be from the future
                    raise <| LevelChangedException(this, this.Level, depth - 1)
                        
                let caller = token.Caller
                if not (isNull caller) then
                    if this.Reevaluate then
                        caller.Reevaluate <- true
                        caller.InputChanged(this, this)
                        caller.AllInputsProcessed(this)
                    else
                        this.Outputs.Add caller |> ignore
                        caller.Level <- max caller.Level (this.Level + 1)

            with _ ->
                AdaptiveObject.EvaluationDepthValue <- depth
                token.ExitFaultedRead this
                reraise()
                
            AdaptiveObject.EvaluationDepthValue <- depth
            // downgrade to read
            token.Downgrade this

            if isNull token.Caller then
                token.Release()

                if depth = 0 && not Transaction.HasRunning then 
                    AdaptiveObject.markTime()

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
        member x.EvaluateIfNeeded (token : AdaptiveToken) (otherwise : 'a) (f : AdaptiveToken -> 'a) =
            x.evaluate token (fun token ->
                if x.OutOfDate then 
                    f token
                else
                    otherwise
            )

        /// <summary>
        /// utility function for evaluating an object even if it
        /// is not marked as outOfDate.
        /// Note that this function takes care of appropriate locking
        /// </summary>
        member x.EvaluateAlways (token : AdaptiveToken) (f : AdaptiveToken -> 'a) =
            x.evaluate token f

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

                
            member x.Reevaluate
                with get() = x.Reevaluate
                and set v = x.Reevaluate <- v


            member x.Outputs = x.Outputs
            member x.Inputs = x.Inputs
            member x.Level 
                with get() = x.Level
                and set l = x.Level <- l

            member x.Mark () =
                x.Mark ()

            member x.InputChanged(o,ip) = x.InputChanged(o, ip)
            member x.AllInputsProcessed(o) = x.AllInputsProcessed(o)
            member x.ReaderCount
                with get() = x.ReaderCountValue
                and set v = x.ReaderCountValue <- v

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


//
//        member x.EvaluateIfNeeded' (token : AdaptiveToken) (otherwise : 'b) (compute : AdaptiveToken -> HashSet<'a> -> 'b) =
//            x.EvaluateIfNeeded token otherwise (fun token ->
//                let d = x.Dirty
//                let res = compute token d
//                d.Clear()
//                res
//            )

        member x.EvaluateAlways' (token : AdaptiveToken) (compute : AdaptiveToken -> HashSet<'a> -> 'b) =
            x.EvaluateAlways token (fun token ->
                let d = x.Dirty
                x.Dirty <- HashSet()
                let res = compute token d
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
    let mutable readerCount = 0

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

        member x.Reevaluate
            with get() = false
            and set o = failwith "cannot mark constant outOfDate"

        member x.Inputs = Seq.empty
        member x.Outputs = VolatileCollection()
        member x.InputChanged(o,ip) = ()
        member x.AllInputsProcessed(o) = ()
        member x.ReaderCount
            with get() = readerCount
            and set v = readerCount <- v




[<AutoOpen>]
module Marking =



    // since changeable inputs need a transaction
    // for enqueing their changes we use a thread local 
    // current transaction which basically allows for 
    // an implicit argument.
    //let internal current = new Threading.ThreadLocal<Option<Transaction>>(fun () -> None)

    /// <summary>
    /// returns the currently running transaction or (if none)
    /// the current transaction for the calling thread
    /// </summary>
    let getCurrentTransaction() =
        match Transaction.Running with
            | Some r -> Some r
            | None ->
                match Transaction.Current with
                    | Some c -> Some c
                    | None -> None

    let inline setCurrentTransaction t =
        Transaction.Current <- t

    /// <summary>
    /// executes a function "inside" a newly created
    /// transaction and commits the transaction
    /// </summary>
    let transact (f : unit -> 'a) =
        use t = new Transaction()
        let old = Transaction.Current
        Transaction.Current <- Some t
        let r = f()
        Transaction.Current <- old
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


        member x.MarkOutdated (cause : Option<IAdaptiveObject>, fin : Option<unit -> unit>) =
            match getCurrentTransaction() with
                | Some t -> 
                    t.Enqueue(x, cause)
                    match fin with
                        | Some fin -> t.AddFinalizer(fin)
                        | None -> ()
                | None -> 
                    lock x (fun () -> 
                        if x.OutOfDate then ()
                        elif x.Outputs.IsEmpty then x.OutOfDate <- true
                        else failwith "cannot mark object without transaction"
                    )
                    match fin with
                        | Some fin -> fin()
                        | None -> ()

        member x.MarkOutdated () =
            x.MarkOutdated None
               
        member x.MarkOutdated (fin : Option<unit -> unit>) =
            x.MarkOutdated(None, fin)
                            
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
        let mutable readerCount = 0
        do lock inner (fun () -> inner.Outputs.Add this |> ignore)

        do lock undyingMarkingCallbacks (fun () -> undyingMarkingCallbacks.GetOrCreateValue(inner).Add this |> ignore )

        member x.Mark() =
//            let old = AdaptiveSystemState.pushReadLocks()
//            try
            if live = 1 then
                Ag.useScope scope (fun () ->
                    callback x
                )
//            finally
//                AdaptiveSystemState.popReadLocks old

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

            member x.Reevaluate
                with get() = false
                and set o = ()

            member x.Inputs = Seq.singleton inner
            member x.Outputs = VolatileCollection()
            member x.InputChanged(o,ip) = ()
            member x.AllInputsProcessed(o) = ()
            member x.ReaderCount
                with get() = readerCount
                and set c = readerCount <- c

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
                match inner with
                    | :? IDisposable as d -> d.Dispose()
                    | _ -> ()
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
                    lock x (fun _ -> 
                        try
                            f ()
                        finally 
                            x.Outputs.Add self |> ignore
                    )
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
 

        member x.AddEvaluationCallback(f : AdaptiveToken -> unit) =
            let res = 
                new CallbackObject(x, fun self ->
                    try
                        f AdaptiveToken.Top
                    finally 
                        lock x (fun () -> x.Outputs.Add self |> ignore)
                )

            res.Mark() |> ignore

            res :> IDisposable //{ new IDisposable with member __.Dispose() = live := false; x.MarkingCallbacks.Remove !self |> ignore}
 

  
/// <summary>
/// defines a base class for all decorated mods
/// </summary>
type AdaptiveDecorator(o : IAdaptiveObject) =
    let mutable o = o
    let id = newId()
    let mutable weakThis = null
    let mutable readerCount = 0
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

        member x.Reevaluate
            with get() = o.Reevaluate
            and set v = o.Reevaluate <- v


        member x.Outputs = o.Outputs
        member x.Inputs = o.Inputs
        member x.Level 
            with get() = o.Level
            and set l = o.Level <- l

        member x.Mark () = o.Mark()

        member x.InputChanged(t,ip) = o.InputChanged (t,ip)
        member x.AllInputsProcessed(t) = o.AllInputsProcessed(t)
        member x.ReaderCount
            with get() = readerCount
            and set c = readerCount <- c
 
type VolatileDirtySet<'a, 'b when 'a :> IAdaptiveObject and 'a : equality and 'a : not struct>(eval : 'a -> 'b) =
    static let empty = ref HSet.empty
    let mutable set : ref<hset<'a>> = ref HSet.empty

    member x.Evaluate() =
        let local = !Interlocked.Exchange(&set, empty) 
        try
            local 
                |> HSet.toList
                |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                |> List.map (fun o -> eval o)

        with :? LevelChangedException as l ->
            Interlocked.Change(&set, fun s -> ref (HSet.union local !s)) |> ignore
            raise l

    member x.Push(i : 'a) =
        lock i (fun () ->
            if i.OutOfDate then
                Interlocked.Change(&set, fun s -> ref (HSet.add i !s)) |> ignore
        )

    member x.Add(i : 'a) =
        x.Push(i)

    member x.Remove(i : 'a) =
        Interlocked.Change(&set, fun s -> ref (HSet.remove i !s)) |> ignore
 
    member x.Clear() =
        Interlocked.Exchange(&set, empty) |> ignore

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
    static let empty = ref HSet.empty
    let mutable set : ref<hset<'a>> = empty
    let tagDict = Dictionary<'a, HashSet<'t>>()

    member x.Evaluate() =
        lock tagDict (fun () ->
            let local = !Interlocked.Exchange(&set, empty) 
            try
                local |> HSet.toList
                      |> List.filter (fun o -> lock o (fun () -> o.OutOfDate))
                      |> List.map (fun o ->
                            match tagDict.TryGetValue o with
                                | (true, tags) -> o, Seq.toList tags
                                | _ -> o, []
                         )
                      |> List.map (fun (o, tags) -> eval o, tags)

            with :? LevelChangedException as l ->
                Interlocked.Change(&set, fun s -> ref (HSet.union local !s)) |> ignore
                raise l
        )

    member x.Push(i : 'a) =
        lock tagDict (fun () ->
            lock i (fun () ->
                if i.OutOfDate && tagDict.ContainsKey i then
                    Interlocked.Change(&set, fun s -> ref (HSet.add i !s)) |> ignore
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
                            Interlocked.Change(&set, fun s -> ref (HSet.remove i !s)) |> ignore
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
            Interlocked.Exchange(&set, empty) |> ignore
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
