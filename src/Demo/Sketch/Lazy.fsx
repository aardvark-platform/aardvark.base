#I @"..\..\..\bin\Release"

#r "Aardvark.Base.dll"
#r "Aardvark.Base.Essentials.dll"
#r "Aardvark.Base.TypeProviders.dll"
#r "Aardvark.Base.FSharp.dll"
#r "Aardvark.Base.Incremental.dll"

open System
open System.Threading
open System.Linq
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental



type Time = DateTime

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Time =
    let now() : Time = DateTime.Now

    let max (a : Time) (b : Time) : Time =
        max a b

    let min (a : Time) (b : Time) : Time =
        min a b

    let maxValue : Time = DateTime.MaxValue
    let minValue : Time = DateTime.MinValue

type EndReason =
    | Error of Exception
    | Finished
    | Continue

type History<'a> = { values : list<Time * 'a>; finished : bool }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module History =
    let rec private intersperse (l : list<Time * 'a>) (r : list<Time * 'a>) =
        match l, r with
            | [], r -> r
            | l, [] -> l
            | (tl,vl)::rl, (tr,vr)::rr ->
                if tl < tr then (tl,vl)::intersperse rl r
                else (tr,vr)::intersperse l rr

    type private EmptyImpl<'a>() =
        static let empty : History<'a> = { values = []; finished = false }
        static member Empty = empty

    let empty<'a> = EmptyImpl<'a>.Empty

    let isEmpty (h : History<'a>) =
        not h.finished && List.isEmpty h.values

    let par (l : History<'a>) (r : History<'a>) =
        { values = intersperse l.values r.values; finished = l.finished && r.finished }

    let seq (l : History<'a>) (r : History<'a>) =
        if l.finished then l
        else { values = intersperse l.values r.values; finished = r.finished }

    let concatPar (l : seq<History<'a>>) =
        l |> Seq.fold par empty

    let concatSeq (l : seq<History<'a>>) =
        l |> Seq.fold seq empty

    let map (f : 'a -> 'b) (h : History<'a>) =
        { values = List.map (fun (t,v) -> t, f v) h.values; finished = h.finished }      



type IEventReader<'a> =
    inherit IAdaptiveObject
    inherit IDisposable
    abstract member SubscribeOnEvaluate : (History<'a> -> unit) -> IDisposable
    abstract member GetHistory : IAdaptiveObject -> History<'a>
    abstract member IsFinished : bool

type evt<'a> =
    abstract member GetReader : unit -> IEventReader<'a>


module EventReaders =
    
    let private wrap (scope : Ag.Scope) (f : 'a -> 'b) =
        fun v -> Ag.useScope scope (fun () -> f v)

    [<AbstractClass>]
    type AbstractReader<'a>() =
        inherit AdaptiveObject()

        let lockObj = obj()
        let mutable subscriptions : HashSet<History<'a> -> unit> = null

        let addSubscription (cb : History<'a> -> unit) =
            lock lockObj (fun () ->
                if isNull subscriptions then
                    subscriptions <- HashSet()

                subscriptions.Add cb |> ignore
            )

        let removeSubscription (cb : History<'a> -> unit) =
            lock lockObj (fun () ->
                if not (isNull subscriptions) then
                    subscriptions.Remove cb |> ignore
                    if subscriptions.Count = 0 then 
                        subscriptions <- null
            )


        abstract member ComputeHistory : unit -> History<'a>
        abstract member Release : unit -> unit
        abstract member IsFinished : bool

        member x.SubscribeOnEvaluate(cb : History<'a> -> unit) =
            addSubscription cb
            { new IDisposable with member x.Dispose() = removeSubscription cb }

        member x.GetHistory(caller : IAdaptiveObject) =
            x.EvaluateIfNeeded caller History.empty (fun () ->
                let h = x.ComputeHistory()

                if not (History.isEmpty h || isNull subscriptions) then
                    let s = lock lockObj (fun () -> subscriptions.ToArray())
                    for cb in s do cb h

                h
            )

        member x.Dispose() = 
            subscriptions <- null
            x.Release()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IEventReader<'a> with
            member x.GetHistory caller = x.GetHistory caller
            member x.SubscribeOnEvaluate cb = x.SubscribeOnEvaluate cb
            member x.IsFinished = x.IsFinished

    type MapReader<'a, 'b>(scope : Ag.Scope, input : IEventReader<'a>, f : 'a -> 'b) =
        inherit AbstractReader<'b>()
        let f = wrap scope f

        override x.IsFinished =
            input.IsFinished

        override x.Release() =
            input.Dispose()

        override x.ComputeHistory() =
            input.GetHistory x |> History.map f

        override x.Inputs =
            Seq.singleton (input :> IAdaptiveObject)

    type CollectReader<'a, 'b>(scope : Ag.Scope, input : IEventReader<'a>, f : 'a -> IEventReader<'b>) =
        inherit AbstractReader<'b>()
        let f = wrap scope f

        let mutable outerHistory = History.empty
        let mutable currentInnerTime = Time.minValue
        let mutable currentInner : Option<IEventReader<'b>> = None

        member private x.stepOuterNoEvaluate(pullOuter : bool) =
            match outerHistory.values with
                | (ot,ov)::rest ->

                    outerHistory <- { outerHistory with values = rest }
                    let inner = f ov
                    currentInnerTime <- ot
                    currentInner <- Some inner

                    true

                | [] -> 
                    if pullOuter then
                        let o = input.GetHistory x
                        outerHistory <- History.seq outerHistory o
                        x.stepOuterNoEvaluate false
                    else
                        false

        member private x.stepOuter() =
            match outerHistory.values with
                | (ot,ov)::rest ->

                    outerHistory <- { outerHistory with values = rest }
                    let inner = f ov
                    currentInnerTime <- ot
                    currentInner <- Some inner

                    let ih = 
                        let h = inner.GetHistory x
                        { h with values = h.values |> List.map (fun (t,v) -> (Time.max t ot), v) }

                    if ih.finished then
                        currentInner <- None

                        let (next,f) = x.stepOuter()
                        ih.values @ next, f
                    else
                        ih.values, false


                | [] -> 
                    [], input.IsFinished

        override x.InputChanged(o : IAdaptiveObject) =
            match o with
                | :? IEventReader<'b> as o when Some o = currentInner ->
                    if o.IsFinished then
                        let changed = x.stepOuterNoEvaluate(true)
                        ()


                | _ -> ()

        override x.IsFinished =
            (match currentInner with | Some i -> i.IsFinished | _ -> true) &&
            input.IsFinished &&
            List.isEmpty outerHistory.values

        override x.Inputs =
            seq {
                yield input :> IAdaptiveObject
                match currentInner with
                    | Some i -> yield i :> IAdaptiveObject
                    | _ -> ()
            }

        override x.Release() =
            input.Dispose()

        override x.ComputeHistory() =
            let o = input.GetHistory x
            outerHistory <- History.seq outerHistory o

            match currentInner with
                | Some inner ->
                    let h = 
                        let h = inner.GetHistory x
                        { h with values = h.values |> List.map (fun (t,v) -> (Time.max t currentInnerTime), v) }

                    if h.finished then
                        let (values, finished) = x.stepOuter()
                        { values = h.values @ values; finished = finished }
                    else
                        h
                | None ->
                    let (values, finished) = x.stepOuter()
                    { values = values; finished = finished }
                    
    type ParallelReader<'a, 'b>(scope : Ag.Scope, input : IReader<'a>, f : 'a -> IEventReader<'b>) as this =
        inherit AbstractReader<'b>()
        
        let cache = Cache(scope, f)
        let dirtySet = MutableVolatileDirtySet(fun (r : IEventReader<'b>) -> r.GetHistory this)

        override x.IsFinished =
            false

        override x.Inputs =
            seq {
                yield input :> IAdaptiveObject
                for r in cache.Values do
                    yield r :> IAdaptiveObject
            }

        override x.Release() =
            input.Dispose()    
            cache.Clear(fun r -> r.RemoveOutput x)     
            dirtySet.Clear()

        override x.InputChanged(o : IAdaptiveObject) =
            match o with
                | :? IEventReader<'b> as r -> dirtySet.Push r
                | _ -> ()

        override x.ComputeHistory() =
            let outer = input.GetDelta x

            let outerHistories =
                [ for o in outer do
                    match o with
                        | Add v ->
                            let r = cache.Invoke v

                            let res = r.GetHistory x
                            dirtySet.Add r

                            yield res

                        | Rem v ->
                            let r = cache.Revoke v

                            dirtySet.Remove r

                            r.Dispose()

                            ()
                ]
            

            let inner = dirtySet.Evaluate()

            let history = outerHistories @ inner |> History.concatPar
            { history with finished = false }

    type ModReader<'a>(input : IMod<'a>) as this =
        inherit AbstractReader<'a>()

        let historyValues = List<Time * 'a> [Time.now(), input.GetValue this]

        override x.IsFinished =
            false

        override x.Inputs =
            Seq.singleton (input :> IAdaptiveObject)

        override x.Mark() =
            let v = input.GetValue x
            historyValues.Add(Time.now(), v)

            true

        override x.Release() =
            input.RemoveOutput x
            historyValues.Clear()

        override x.ComputeHistory() =
            let l = historyValues |> Seq.toList
            historyValues.Clear()
            { values = l; finished = false }

    type ConstantReader<'a>(values : list<Time * 'a>) =
        inherit AbstractReader<'a>()
        let mutable values = values

        override x.IsFinished =
            true

        override x.Release() = values <- []

        override x.ComputeHistory() =
            let v = values
            values <- []
            { values = v; finished = true }

    type EmitReader<'a>(remove : EmitReader<'a> -> unit) =
        inherit AbstractReader<'a>()


        let values = List<Time * 'a>()
        let mutable finished = false

        member x.Emit(v : seq<Time * 'a>) =
            values.AddRange v
            x.MarkOutdated()

        member x.Finish() =
            if not finished then
                finished <- true
                x.MarkOutdated()

        override x.IsFinished =
            finished

        override x.Release() =
            values.Clear()
            finished <- true
            remove x

        override x.ComputeHistory() =
            let l = values |> Seq.toList
            values.Clear()
            { values = l; finished = finished }

    type CopyReader<'a>(input : IEventReader<'a>, remove : CopyReader<'a> -> unit) =
        inherit AbstractReader<'a>()

        let mutable buffer = History.empty
        let s = input.SubscribeOnEvaluate (fun h -> buffer <- History.seq buffer h)

        override x.IsFinished =
            input.IsFinished

        override x.Inputs = Seq.singleton (input :> IAdaptiveObject)

        override x.ComputeHistory() =
            input.GetHistory x |> ignore

            let h = buffer
            buffer <- History.empty
            h

        override x.Release() =
            s.Dispose()
            buffer <- History.empty
            remove x





module Evt =
    
    type Source<'a>() =
        let readers = HashSet<EventReaders.EmitReader<'a>>()

        let remove (r : EventReaders.EmitReader<'a>) =
            lock readers (fun () ->
                readers.Remove r |> ignore
            )

        let newReader () =
            lock readers (fun () ->
                let r = new EventReaders.EmitReader<'a>(remove)
                readers.Add r

                r :> IEventReader<_>
            )
        
        let pushValues (values : seq<Time * 'a>) =
            lock readers (fun () ->
                for r in readers do r.Emit values
            )

        let finish() =
            lock readers (fun () ->
                for r in readers do r.Finish()
                readers.Clear()
            )

        member x.GetReader() =
            newReader()

        member x.Emit(values : seq<'a>) =
            let t = Time.now()
            pushValues (Seq.map (fun v -> t,v) values)

        member x.Emit(value : 'a) =
            let t = Time.now()
            pushValues [t,value]

        member x.Finish() =
            transact (fun () -> finish())

        interface evt<'a> with
            member x.GetReader() = x.GetReader()

    type Evt<'a>(newReader : unit -> IEventReader<'a>) =
        
        let mutable inputReader : Option<IEventReader<'a>> = Some (newReader())
        let readers = HashSet<EventReaders.CopyReader<'a>>()

        let remove (r : EventReaders.CopyReader<'a>) =
            lock readers (fun () ->
                readers.Remove r |> ignore
                if readers.Count = 0 then
                    match inputReader with
                        | Some i -> 
                            i.Dispose() 
                            inputReader <- None
                        | _ -> ()

            )

        let create() =
            lock readers (fun () ->
                let i = 
                    match inputReader with
                        | Some i -> i
                        | None ->
                            let i = newReader()
                            inputReader <- Some i
                            i

                let cp = new EventReaders.CopyReader<'a>(i, remove)
                readers.Add cp |> ignore
                cp :> IEventReader<_>
            )


        member x.GetReader() =
            create()

        interface evt<'a> with
            member x.GetReader() = x.GetReader()

    type Constant<'a>(values : list<'a>) =
        let values = List.map (fun v -> Time.minValue, v) values

        member x.GetReader() =
            new EventReaders.ConstantReader<_>(values) :> IEventReader<_>

        interface evt<'a> with
            member x.GetReader() = x.GetReader()


    let inline private evt (f : Ag.Scope -> #IEventReader<'a>) =
        let scope = Ag.getContext()
        Evt(fun () -> f scope :> IEventReader<_>) :> evt<_>


    type private Empty<'a>() =
        static let instance : evt<'a> = Constant [] :> evt<_>
        static member Instance = instance

    let empty<'a> = Empty<'a>.Instance
    
    let single(v : 'a) : evt<'a> = Constant [v] :> evt<_>
   
    let ofList(v : list<'a>) : evt<'a> = Constant v :> evt<_>
    let inline ofSeq(v : seq<'a>) = v |> Seq.toList |> ofList
    let inline ofArray(v : 'a[]) = v |> Array.toList |> ofList
    

    let source() : Source<'a> =
        Source()

    let map (f : 'a -> 'b) (e : evt<'a>) =
        evt (fun scope -> new EventReaders.MapReader<_,_>(scope, e.GetReader(), f))

    let forall (f : 'a -> evt<'b>) (e : aset<'a>) =
        evt (fun scope -> new EventReaders.ParallelReader<_,_>(scope, e.GetReader(), fun v -> (f v).GetReader()))

    let ofMod (e : IMod<'a>) =
        evt (fun scope -> new EventReaders.ModReader<_>(e))

    let collect (f : 'a -> evt<'b>) (e : evt<'a>) =
        evt (fun scope -> new EventReaders.CollectReader<_,_>(scope, e.GetReader(), fun v -> (f v).GetReader()))

    let choose (f : 'a -> Option<'b>) (e : evt<'a>) =
        e |> collect (fun v ->
            match f v with
                | Some v -> single v
                | None -> empty
        )


module Test =
    

    let run() =
        let s0 = Evt.source()
        let s1 = Evt.source()
        let s2 = Mod.init 10

        let both = Evt.ofList [s0 :> evt<_>; s1 :> evt<_>; Evt.ofMod s2]

        let res = both |> Evt.collect id

        let sepp = res |> Evt.collect (fun v -> Evt.ofList [v; -v])

        let r = sepp.GetReader()
        r.GetHistory null |> printfn "history: %A"


        transact(fun () ->
            s0.Emit 1
            s1.Emit 221323 // missed since no one listening (s0 not finished yet)
            s0.Emit 10
        )
        r.GetHistory null |> printfn "history: %A"

        transact(fun () ->
            s1.Emit 1000 // missed since no one listening (s0 not finished yet)
            s0.Finish()
            s1.Emit 2 // should be seen (s0 now finished)
        )
        r.GetHistory null |> printfn "history: %A"


        transact(fun () ->
            s1.Finish()
        )
        r.GetHistory null |> printfn "history: %A"


        transact(fun () ->
            Mod.change s2 100
        )
        r.GetHistory null |> printfn "history: %A"



