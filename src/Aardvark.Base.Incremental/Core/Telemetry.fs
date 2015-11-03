namespace Aardvark.Base.Incremental

open System
open System.Threading
open System.Diagnostics
open Aardvark.Base

[<CustomComparison; CustomEquality>]
type TimeDiff =
    struct
        [<DefaultValue>]
        static val mutable private ticksPerMicrosecond : float

        static member TicksPerMicrosecond =
            if TimeDiff.ticksPerMicrosecond = 0.0 then 
                TimeDiff.ticksPerMicrosecond <- float TimeSpan.TicksPerMillisecond / 1000.0
            TimeDiff.ticksPerMicrosecond


        val mutable public Ticks : float

        member x.Hours = x.Ticks / float TimeSpan.TicksPerHour
        member x.Minutes = x.Ticks / float TimeSpan.TicksPerMinute
        member x.Seconds = x.Ticks / float TimeSpan.TicksPerSecond
        member x.Milliseconds = x.Ticks / float TimeSpan.TicksPerMillisecond
        member x.Microseconds = x.Ticks / TimeDiff.TicksPerMicrosecond

        override x.GetHashCode() =
            x.Ticks.GetHashCode()

        override x.Equals(o) =
            match o with
                | :? TimeDiff as o -> x.Ticks = o.Ticks
                | _ -> false

        interface IComparable with
            member x.CompareTo(o : obj) =
                match o with
                    | :? TimeDiff as o -> compare x.Ticks o.Ticks
                    | _ -> failwithf "cannot compare TimeDiff to %A" o

        override x.ToString() =
            if x.Ticks <= 0.0 then "0"
            elif x.Hours > 1.0 then sprintf "%.3f" x.Hours
            elif x.Minutes > 1.0 then sprintf "%.3fm" x.Minutes
            elif x.Seconds > 0.5 then sprintf "%.3fs" x.Seconds
            elif x.Milliseconds > 1.0 then sprintf "%.2fms" x.Milliseconds
            else sprintf "%.1fµs" x.Microseconds
            
        static member Zero = TimeDiff(0.0)

        static member DivideByInt(l : TimeDiff, v : int) =
            TimeDiff(l.Ticks / float v)

        static member (+) (l : TimeDiff, r : TimeDiff) =
            TimeDiff(l.Ticks + r.Ticks)

        static member (-) (l : TimeDiff, r : TimeDiff) =
            TimeDiff(l.Ticks - r.Ticks)

        static member (*) (l : TimeDiff, r : float) =
            TimeDiff(l.Ticks * r)

        static member (*) (l : float, r : TimeDiff) =
            TimeDiff(l * r.Ticks)

        static member (*) (l : TimeDiff, r : int) =
            TimeDiff(l.Ticks * float r)

        static member (*) (l : int, r : TimeDiff) =
            TimeDiff(float l * r.Ticks)

        static member (/) (l : TimeDiff, r : float) =
            TimeDiff(l.Ticks / r)

        static member (/) (l : TimeDiff, r : int) =
            TimeDiff(l.Ticks / float r)

        static member (/) (l : TimeDiff, r : TimeDiff) =
            l.Ticks / r.Ticks


        new(ticks : float) = { Ticks = ticks }
        new(ts : TimeSpan) = { Ticks = float ts.Ticks }

    end

[<AutoOpen>]
module StopwatchExtensions =
    type Stopwatch with
        member x.TimeDiff = TimeDiff x.Elapsed

type TelemetryReport =
    {
        totalTime : TimeDiff
        probeTimes : Map<Symbol, TimeDiff>
    }

module Telemetry =
    let running = new ThreadLocal<Option<Stopwatch>>(fun () -> None)
    let probes = SymbolDict<Stopwatch>()

    let inline timed (s : Symbol) (f : unit -> 'a) =
        #if TRACE
        let sw = lock probes (fun () -> probes.GetOrCreate(s, fun s -> Stopwatch()))

        let current = running.Value
        let hadRunning = current.IsSome && current.Value <> sw

        let restartCurrent, start = 
            match current with
                | Some o when o <> sw -> true, true
                | None -> false,true
                | _ -> false, false
                    
        if restartCurrent then
            current.Value.Stop()
            running.Value <- Some sw


            
        if start then sw.Start()
        let res = f()
        if start then sw.Stop()

        running.Value <- current
        if restartCurrent then
            current.Value.Start() 

        res
        #else
        f()
        #endif

    let inline resetAndGetReport() =
        #if TRACE

        let mine =
            lock probes (fun () -> 
                let arr = probes.ToArray()
                probes.Clear()
                arr
            )

        let totalTime = ref TimeDiff.Zero
        let times = 
            Map.ofList [
                for (KeyValue(s,sw)) in mine do
                    let t = sw.TimeDiff
                    totalTime := !totalTime + t
                    yield (s,t)
            ]

        { totalTime = !totalTime; probeTimes = times }
        #else
        { totalTime = TimeDiff.Zero; probeTimes = Map.empty }
        #endif

    let inline print (r : TelemetryReport) =
        #if TRACE
        Log.start "Telemetry"

        let entries = 
            r.probeTimes 
                |> Map.toList
                |> List.map (fun (s,v) -> s.ToString(), v)
                |> List.sortWith (fun (a,_) (b,_) -> String.Compare(a, b))

        for (name, dt) in entries do
            Log.line "%s: %A (%.2f%%)" name dt (100.0 * dt / r.totalTime)


        Log.line "total: %A" r.totalTime
        Log.stop()
        #else
        ()
        #endif

    let inline resetAndPrint() =
        #if TRACE
        let r = resetAndGetReport()
        print r
        #else
        ()
        #endif
