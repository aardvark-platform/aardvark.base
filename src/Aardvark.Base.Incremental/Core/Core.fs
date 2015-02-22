namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open Aardvark.Base
open System.Collections.Concurrent

[<AllowNullLiteral>]
type IAdaptiveObject =
    inherit IComparable
    abstract member Id : int
    abstract member Level : int with get, set
    abstract member Mark : unit -> bool
    abstract member OutOfDate : bool with get, set
    abstract member Inputs : HashSet<IAdaptiveObject>
    abstract member Outputs : WeakSet<IAdaptiveObject>
    abstract member MarkingCallbacks : ConcurrentHashSet<unit -> unit>

module IncrementalLog =
    open System.Threading

    type Operation =
        | StartEvaluate = 1
        | EndEvaluate = 2
        | StartMark = 3
        | EndMark = 4

    let private log = List<Operation * IAdaptiveObject>()
    let private logLock = SpinLock()

    let append (op : Operation) (o : IAdaptiveObject) =
        //let mutable taken = false
        ()
//        lock log (fun () ->
//            log.Add(op,o)
//        )
//        
//        try
//            logLock.TryEnter(&taken)
//            log.Add(op, o)
//
//        finally
//            if taken then logLock.Exit()

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
                | Operation.StartEvaluate -> sprintf "S(%d)" o.Id
                | Operation.EndEvaluate -> sprintf "E(%d)" o.Id
                | Operation.StartMark -> sprintf "MS(%d)" o.Id
                | Operation.EndMark -> sprintf "ME(%d)" o.Id
                | _ -> failwithf "unknown operation: %A" op
        ) |> String.concat "\r\n"

exception LevelChangedException of IAdaptiveObject

type Transaction() =
    static let running = new Threading.ThreadLocal<Option<Transaction>>(fun () -> None)
    
    let q = DuplicatePriorityQueue<IAdaptiveObject, int>(fun o -> o.Level)
    let contained = HashSet<IAdaptiveObject>()

    let getAndClear (set : ConcurrentHashSet<'a>) =
        let mutable content = []
        for e in set do content <- e::content
        set.Clear()
        content

    static member Running =
        running.Value

    static member HasRunning =
        running.Value.IsSome
        

    member x.Enqueue(e : IAdaptiveObject) =
        if contained.Add e then
            q.Enqueue e

    member x.Commit() =
        let old = running.Value
        running.Value <- Some x
        while q.Count > 0 do
            let l, e = q.Dequeue()
            contained.Remove e |> ignore

            let outputs = 
                lock e (fun () ->
                    if not e.OutOfDate then
                        try
                            IncrementalLog.startMark e

                            if l <> e.Level then
                                q.Enqueue e
                                Seq.empty
                            else
                                e.OutOfDate <- true
                
                                try 
                                    if e.Mark() then
                                        let mutable failed = false
                                        let callbacks = e.MarkingCallbacks |> getAndClear
                                        for cb in callbacks do 
                                            try cb()
                                            with :? LevelChangedException -> failed <- true

                                        if failed then raise <| LevelChangedException e

                                        e.Outputs :> seq<IAdaptiveObject>

                                    else
                                        Seq.empty

                                with :? LevelChangedException ->
                                    e.OutOfDate <- false
                                    q.Enqueue e
                                    Seq.empty
                        finally
                            IncrementalLog.endMark e
                    else
                        Seq.empty
                )

            for o in outputs do
                q.Enqueue o

        running.Value <- old

[<AbstractClass>]
type AdaptiveObject() =
    let id = newId()
    let mutable outOfDate = true
    let mutable level = 0
    let inputs = HashSet<IAdaptiveObject>()
    let outputs = WeakSet<IAdaptiveObject>()
    let callbacks = ConcurrentHashSet<unit -> unit>()

    member x.EvaluateIfNeeded (otherwise : 'a) (f : unit -> 'a) =
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

    member x.EvaluateAlways (f : unit -> 'a) =
        lock x (fun () ->
            IncrementalLog.startEvaluate x
            let res = f()
            x.OutOfDate <- false
            IncrementalLog.endEvaluate x
            res
        )


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
    

    override x.GetHashCode() = id
    override x.Equals o =
        match o with
            | :? IAdaptiveObject as o -> id = o.Id
            | _ -> false

    interface IComparable with
        member x.CompareTo o =
            match o with
                | :? IAdaptiveObject as o -> compare id o.Id
                | _ -> failwith "uncomparable"

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

[<AutoOpen>]
module Marking =
   
    let changePropagationLock = obj()

    let rec private relabel (m : IAdaptiveObject) (minLevel : int) =
        if m.Level < minLevel then
            m.Level <- minLevel
            for o in m.Outputs do
                relabel o (minLevel + 1) |> ignore
            true
        else
            false

    let private current = new Threading.ThreadLocal<Option<Transaction>>(fun () -> None)

    let getCurrentTransaction() =
        match Transaction.Running with
            | Some r -> Some r
            | None ->
                match current.Value with
                    | Some c -> Some c
                    | None -> None


    let transact (f : unit -> 'a) =
        let t = Transaction()
        let old = current.Value
        current.Value <- Some t
        let r = f()
        current.Value <- old
        t.Commit()
        r

    type TransactionBuilder() =
        member x.Zero() : unit -> unit = id
        member x.For(s : seq<'a>, f : 'a -> unit -> unit) : unit -> unit =
            fun () ->
                for e in s do
                    f e ()

        member x.Delay(f : unit -> unit -> unit) =
            fun () -> f () ()

        member x.Combine(l : unit -> unit, r : unit -> unit) =
            fun () ->
                l(); r()

        member x.Run(f : unit -> unit) =
            transact f

    let commit = TransactionBuilder()

    type IAdaptiveObject with
        member x.MarkOutdated () =
            match getCurrentTransaction() with
                | Some t -> t.Enqueue x
                | None -> 
                    lock x (fun () -> 
                        if x.OutOfDate then ()
                        else failwith "cannot mark object without transaction"
                    )
                            

        member x.AddOutput(m : IAdaptiveObject) =
            m.Inputs.Add x |> ignore
            x.Outputs.Add m |> ignore

            if relabel m (x.Level + 1) then
                if Transaction.HasRunning then raise <| LevelChangedException m

            m.MarkOutdated()

        member x.RemoveOutput (m : IAdaptiveObject) =
            m.Inputs.Remove x |> ignore
            x.Outputs.Remove m |> ignore



    type IAdaptiveObject with
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
                

