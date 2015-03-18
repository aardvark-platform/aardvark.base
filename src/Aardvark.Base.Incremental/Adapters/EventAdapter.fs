namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open System.Threading
open Aardvark.Base
open System.Diagnostics
open System.Runtime.CompilerServices

type EventSampler(frequency : int) =
    let m_lock = obj()
    let m_maxFrequency = frequency
    let m_minSampleTime = if m_maxFrequency = 0 then 0 else int (1000.0 / float m_maxFrequency)
    let mutable m_thread = null
    let m_sem = new ManualResetEventSlim()
    let mutable actions : list<unit -> unit> = []

    member x.Run() =
        let sw = Stopwatch()
        sw.Start()

        while true do
            x.Process()
            sw.Stop()
            let t = sw.Elapsed.TotalMilliseconds
            sw.Restart()

            if m_minSampleTime > 0 then
                let wait = m_minSampleTime - Fun.Max(0, int t)
                if wait > 0 then Thread.Sleep wait

    member x.Process() =    
        m_sem.Wait()
        m_sem.Reset()
        let myActions = Interlocked.Exchange(&actions, [])

        transact (fun () ->
            for a in myActions do
                a()
        )



    member x.Enqueue<'a>(c : ModRef<'a>, value : 'a) : unit =
        match m_thread with
            | null -> 
                m_thread <- Thread(ThreadStart(x.Run))
                m_thread.IsBackground <- true
                m_thread.Start()
            | _ -> ()

        let entry = fun () -> c.Value <- value

        let mutable ex = Unchecked.defaultof<_>
        let mutable current = actions

        while not <| Object.ReferenceEquals(ex, current) do
            current <- actions
            let newActions = entry::current
            ex <- Interlocked.CompareExchange(&actions, newActions, current)
//
//        actions <- (fun () -> c.Value <- value)::actions
//        lock(a) (fun () -> a.Add (fun () -> c.Value <- value))
        m_sem.Set()
        

    member x.Dispose() =
        if m_thread <> null then
            m_thread.Abort()

    interface IDisposable with
        member x.Dispose() = x.Dispose()

module EventAdapters =

    type private AdapterMod<'a>(e : IEvent<'a>, value : 'a, s : IDisposable) =
        inherit ModRef<'a>(value)

        member x.Event = e

        override x.Finalize() =
            s.Dispose()

    type private AdapterEvent<'a>(m : IMod<'a>, value : 'a) =
        inherit EventSource<'a>(value)
        let mutable s : IDisposable = null


        member x.Subscription 
            with get() = s
            and set v = s <- v

        member x.Mod = m

        override x.Finalize() =
            if s <> null then
                s.Dispose()

    let (|EventOf|_|) (t : Type) =
        if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<IEvent<_>> then
            Some (t.GetGenericArguments().[0])
        else
            let iface = t.GetInterface(typedefof<IEvent<_>>.FullName)
            if iface <> null then
                Some (iface.GetGenericArguments().[0])
            else
                None

    let (|ModOf|_|) (t : Type) =
        if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<IMod<_>> then
            Some (t.GetGenericArguments().[0])
        else
            let iface = t.GetInterface(typedefof<IMod<_>>.FullName)
            if iface <> null then
                Some (iface.GetGenericArguments().[0])
            else
                None



    let private sampler = new EventSampler(1000)
    let private modCache = ConditionalWeakTable<IEvent, IMod>()
    let private eventCache = ConditionalWeakTable<IMod, IEvent>()
    let private myType = typeof<AdapterMod<obj>>.DeclaringType
    let private toModMethod = myType.GetMethod("toMod").GetGenericMethodDefinition()
    let private toEventMethod = myType.GetMethod("toEvent").GetGenericMethodDefinition()

    let private change (m : AdapterMod<'a>) (value : 'a) =
        match Marking.getCurrentTransaction() with
            | Some r ->
                // implicitly enqueues the AdapterMod 
                m.Value <- value
            | None ->
                sampler.Enqueue(m, value)

    let toMod (e : Aardvark.Base.IEvent<'a>) : IMod<'a> =
        match e with
            | :? AdapterEvent<'a> as a -> a.Mod
            | _ ->
                match modCache.TryGetValue e with
                    | (true, m) -> m |> unbox<IMod<_>>
                    | _ ->
                        let r = ref <| Unchecked.defaultof<_>
                        let s = e.Values.Subscribe(fun v ->
                            change !r v
                        )
                        r := AdapterMod(e, e.Latest, s)
                        modCache.Add(e, !r)

                        !r :> IMod<_>

    let toEvent (m : IMod<'a>) : IEvent<'a> =
        match m with
            | :? AdapterMod<'a> as m -> m.Event
            | _ ->
                match eventCache.TryGetValue m with
                    | (true, e) -> e |> unbox
                    | _ ->
                        let a = AdapterEvent(m, Unchecked.defaultof<_>) 

                        let s = m |> Mod.registerCallback (fun v ->
                            a.Emit v
                        )

                        a.Subscription <- s
                        eventCache.Add(m, a)
                        a :> IEvent<_>

    let toModUntyped (e : Aardvark.Base.IEvent) : IMod =
        let t = e.GetType()
        match t with
            | EventOf(inner) ->
                let m = toModMethod.MakeGenericMethod [|inner|]
                m.Invoke(null, [|e :> obj|]) |> unbox<IMod>

            | _ ->
                failwithf "could not determine Event-Type for: %A" e

    let toEventUntyped (m : IMod) : IEvent =
        let t = m.GetType()
        match t with
            | ModOf(inner) ->
                let m = toEventMethod.MakeGenericMethod [|inner|]
                m.Invoke(null, [|m :> obj|]) |> unbox<IEvent>

            | _ ->
                failwithf "could not determine Mod-Type for: %A" m

[<AutoOpen>]
module FSharpEventExtensions =
    type IEvent<'a> with
        member x.Mod =
            EventAdapters.toMod x

    type IMod<'a> with
        member x.Event =
            EventAdapters.toEvent x

    type IEvent with
        member x.Mod =
            EventAdapters.toModUntyped x

    type IMod with
        member x.Event =
            EventAdapters.toEventUntyped x


    module Mod =
        let fromEvent (e : IEvent<'a>) =
            EventAdapters.toMod e

        let toEvent (m : IMod<'a>) =
            EventAdapters.toEvent m

[<AbstractClass; Sealed; Extension>]
type CSharpEventExtensions private() =
    [<Extension>]
    static member ToMod(this : IEvent<'a>) : IMod<'a> =
        EventAdapters.toMod this

    [<Extension>]
    static member ToMod(this : IEvent) : IMod =
        EventAdapters.toModUntyped this

    [<Extension>]
    static member ToAdaptiveSet(this : IEvent<'a>) : aset<'a> =
        this |> EventAdapters.toMod |> Mod.toASet

    [<Extension>]
    static member ToAdaptiveList(this : IEvent<'a>) : alist<'a> =
        this |> EventAdapters.toMod |> Mod.toAList


    [<Extension>]
    static member ToEvent(this : IMod<'a>) : IEvent<'a> =
        EventAdapters.toEvent this

    [<Extension>]
    static member ToEvent(this : IMod) : IEvent =
        EventAdapters.toEventUntyped this

    [<Extension>]
    static member ToEvent(this : aset<'a>) : IEvent<ReferenceCountingSet<'a>> =
        this |> ASet.toMod |> EventAdapters.toEvent

    [<Extension>]
    static member ToEvent(this : alist<'a>) : IEvent<TimeList<'a>> =
        this |> AList.toMod |> EventAdapters.toEvent