namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open Aardvark.Base

[<AllowNullLiteral>]
type IAdaptiveObject =
    inherit IComparable
    abstract member Id : int
    abstract member Level : int with get, set
    abstract member Mark : unit -> bool
    abstract member OutOfDate : bool with get, set
    abstract member Inputs : HashSet<IAdaptiveObject>
    abstract member Outputs : WeakSet<IAdaptiveObject>
    abstract member MarkingCallbacks : HashSet<unit -> unit>


exception LevelChangedException of IAdaptiveObject

type Transaction() =
    static let running = new Threading.ThreadLocal<Option<Transaction>>(fun () -> None)
    
    let q = DuplicatePriorityQueue<IAdaptiveObject, int>(fun o -> o.Level)
    let contained = HashSet<IAdaptiveObject>()

    let getAndClear (set : HashSet<'a>) =
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
            if not e.OutOfDate then
                q.Enqueue e

    member x.Commit() =
        let old = running.Value
        running.Value <- Some x
        while q.Count > 0 do
            let l, e = q.Dequeue()
            contained.Remove e |> ignore

            if l <> e.Level then
                q.Enqueue e
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

                        for o in e.Outputs do
                            if not o.OutOfDate then
                                q.Enqueue o

                with :? LevelChangedException ->
                    q.Enqueue e

        running.Value <- old

[<AbstractClass>]
type AdaptiveObject() =
    let id = newId()
    let mutable outOfDate = true
    let mutable level = 0
    let inputs = HashSet<IAdaptiveObject>()
    let outputs = WeakSet<IAdaptiveObject>()
    let callbacks = HashSet<unit -> unit>()

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
    let rec private relabel (m : IAdaptiveObject) (minLevel : int) =
        if m.Level < minLevel then
            m.Level <- minLevel
            for o in m.Outputs do
                relabel o (minLevel + 1) |> ignore
            true
        else
            false

    let private current = new Threading.ThreadLocal<Option<Transaction>>(fun () -> None)

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
            if not x.OutOfDate then 
                match Transaction.Running with
                    | Some t -> t.Enqueue x
                    | None -> 
                        match current.Value with
                            | Some c -> c.Enqueue x
                            | _ -> 
                                failwith "cannot mark object without transaction"

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
            let self = ref id
            self := fun () ->
                f()
                x.MarkingCallbacks.Add(!self) |> ignore

            x.MarkingCallbacks.Add(!self) |> ignore

            { new IDisposable with member __.Dispose() = x.MarkingCallbacks.Remove !self |> ignore}
                

