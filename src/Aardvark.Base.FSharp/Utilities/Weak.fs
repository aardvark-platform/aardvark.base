#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif


open System
open System.Threading

[<AutoOpen>]
module Weak =
    open System
    open System.Collections.Generic
    open System.Linq
    open Aardvark.Base
    open System.Threading
    open System.Runtime.CompilerServices

    (*============================ Statistics =================================*)
    let private lastValues = Dictionary<obj, int>() 
    let mutable private lastNone = 0

    let private statistics = Dictionary<obj, int>() 
    let mutable private statisticsNone = 0

    let convertArray (f : ('a -> 'b)) (arr : Array) =
        if isNull arr || arr.GetType().GetElementType() = typeof<'b> then
            arr
        else
            let result = Array.CreateInstance(typeof<'b>, arr.Length)
            for i in 0..(arr.Length-1) do
                result.SetValue(f(arr.GetValue(i) |> unbox<'a>), i)
            result

    let private registerInStatistics(data : Option<obj>) =
        match data with
            | Some(key) -> if not (isNull key) then
                                 Monitor.Enter(statistics)
                                 match statistics.TryGetValue(key) with
                                     | (true,v) -> statistics.[key] <- v + 1
                                     | _ -> statistics.Add(key, 1)
                                 Monitor.Exit(statistics)
                            else
                                 Report.Warn("Some(null)")
            | _ -> statisticsNone <- statisticsNone + 1

    let printStatistics() =
        let mem = System.GC.GetTotalMemory(true)
        System.GC.WaitForPendingFinalizers()

        Monitor.Enter(statistics)
        Log.line "============================= Finalizer Statistics =========================="
        Log.line "    Total:"
        for kvp in statistics do
            Log.line "        %A: %A" kvp.Key kvp.Value
        Log.line "        \"NONE\": %A" statisticsNone


        Log.line "    Relative:"
        for kvp in statistics do
            let old = match lastValues.TryGetValue(kvp.Key) with
                        | (true,v) -> v
                        | _ -> 0
            Log.line "        %A: +%A" kvp.Key (kvp.Value - old)
        Log.line "        \"NONE\": +%A" (statisticsNone - lastNone)

        Log.line "    Total used Memory: %dMB" (mem >>> 20)

        Log.line "============================================================================="

        statistics |> Seq.iter (fun kvp -> lastValues.[kvp.Key] <- kvp.Value)
        lastNone <- statisticsNone

        Monitor.Exit(statistics)



    (*============================ Registrations ==============================*)

    type private Finalizable() =
        let mutable finalizers = List<(obj -> unit) * Option<obj>>()

        let finalizableFinalize(f,d) =
            registerInStatistics(d)
            match d with
                | None -> f(null)
                | Some(v) -> f(v)

        member x.Add(f : obj -> unit, data : Option<obj>) =
            finalizers.Add((f,data))

        override x.Finalize() =
            try
                finalizers |> Seq.iter finalizableFinalize
                finalizers.Clear()
                finalizers <- null
            with e -> Log.warn "Finalizable threw an exception: %A" e

    let private registrations = ConditionalWeakTable<obj, Finalizable>()

    let registerFinalizer<'a> (obj : 'a, finalizer : obj -> unit, data : Option<obj>) =
        match registrations.TryGetValue(obj) with
            | (true, v) -> v.Add(finalizer, data)
            | _ -> let v = Finalizable()
                   registrations.Add(obj,v)
                   v.Add(finalizer, data)


    (*============================ Weak Types =================================*)
    [<AllowNullLiteral>]
    type Weak<'a when 'a : not struct>(obj : 'a) =
        do if isNull (obj :> obj) then failwith "created null weak"
        let m_weak = System.WeakReference<'a>(obj)
        let m_hashCode = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj)

        member private x.WeakReference = m_weak

        member x.IsLife = match m_weak.TryGetTarget() with
                            | (true,o) when not (isNull (o :> obj)) -> true
                            | _ -> false

        member x.Target = match m_weak.TryGetTarget() with
                            | (true,v) when not (isNull (v :> obj)) -> v
                            | _ -> failwith "Weak is no longer accessible"

        member x.TryGetTarget([<System.Runtime.InteropServices.OutAttribute>] a : byref<'a>) = 
            m_weak.TryGetTarget(&a) && not (isNull (a :> obj))

        member x.TargetOption = 
            match m_weak.TryGetTarget() with
                | (true, v) -> Some v
                | _ -> None


        member x.RegisterFinalizer(f : obj -> unit, ?data) =
            match m_weak.TryGetTarget() with
                | (true, v) -> registerFinalizer(v, f, data)
                | _ -> f()

        member x.RegisterFinalizer(f : unit -> unit) =
            match m_weak.TryGetTarget() with
                | (true, v) -> registerFinalizer(v, (fun _ -> f()), None)
                | _ -> f()


        override x.GetHashCode() = m_hashCode
        override x.Equals(o : obj) =
            if System.Object.ReferenceEquals(o, x) then
                true
            else
                match o with
                    | :? Weak<'a> as other -> match (m_weak.TryGetTarget(), other.WeakReference.TryGetTarget()) with
                                                | ((true, l), (true, r)) -> System.Object.ReferenceEquals(l, r)
                                                | ((false,_), (false,_)) -> true
                                                | _ -> false 
                    | _ -> false
 
    let (|Strong|_|) (weak : Weak<'a>) = 
        match weak.TryGetTarget() with
            | (true,v) -> Some(v)
            | _ -> None
        

    
    let registerWeakFinalizer (w : Weak<obj>, finalizer : obj -> unit, data : Option<obj>) =
        match w with
            | Strong(v) -> registerFinalizer(v, finalizer, data)
            | _ -> match data with
                    | Some(d) -> finalizer(d)
                    | _ -> finalizer(null)


    type System.Object with
        member x.RegisterFinalizer(f : obj -> unit, ?data) =
            registerFinalizer(x, f, data)

        member x.RegisterFinalizer(f : unit -> unit) =
            registerFinalizer(x, (fun _ -> f()), None)

    type System.Collections.Generic.IEnumerable<'a> with
        member x.RegisterAnyFinalizer(f : obj -> unit, ?data) =
            let fRef = ref <| Some(f)
            let finalize(_) =
                match !fRef with
                    | Some(f) -> match data with
                                    | Some(v) -> f(v)
                                    | _ -> f(null)

                                 registerInStatistics(data)
                                 fRef := None
                    | None -> ()

            x |> Seq.iter (fun o -> registerFinalizer(o, finalize, None))

        member x.RegisterAllFinalizer(f : obj -> unit, ?data) =
            let countRef = ref (x.Count())
            let finalize(_) =
                if !countRef = 1 then
                    match data with
                        | Some(v) -> f(v)
                        | _ -> f(null)
                    registerInStatistics(data)
                else 
                    countRef := !countRef - 1

            x |> Seq.iter (fun o -> registerFinalizer(o, finalize, None))

        member x.RegisterAnyFinalizer(f : unit -> unit) =
            x.RegisterAnyFinalizer(fun (o:obj) -> f())

        member x.RegisterAllFinalizer(f : unit -> unit, ?data) =
            x.RegisterAllFinalizer(fun (o:obj) -> f())


    type WeakList<'a when 'a : not struct and 'a : equality>(l : list<Weak<'a>>) =
        let m_elements = l
        let m_isLife = ref true
        let m_hashcode = l |> List.fold (fun c e -> c ^^^ e.GetHashCode()) 0


        new(l : list<'a>) = WeakList<'a>(l |> List.map (fun e -> Weak(e)))

        member private x.Target = m_elements
        
        member x.IsLife = m_elements |> List.fold (fun l e -> l && e.IsLife) true

        member x.Elements = m_elements |> List.map (fun w -> w.Target)
        

        member x.RegisterAnyFinalizer(f : obj -> unit, ?data) =
            let fRef = ref (Some(f))
            let finalizer(_) =
                match !fRef with
                    | Some(f) -> match data with
                                    | Some(v) -> f(v)
                                    | _ -> f(null)

                                 registerInStatistics(data)
                                 fRef := None
                    | _ -> ()

            m_elements |> List.iter (fun e -> match e.TryGetTarget() with
                                                    | (true,v) -> if (!fRef).IsSome then 
                                                                    registerFinalizer(v, finalizer, None)            
                                                    | _ -> finalizer())

        member x.RegisterAllFinalizer(f : obj -> unit, ?data) =
            let countRef = ref (m_elements.Length)
            let finalizer(_) =
                if !countRef = 1 then
                    registerInStatistics(data)
                    match data with
                        | Some(v) -> f(v)
                        | _ -> f(null)
                else
                    countRef := !countRef - 1

            m_elements |> List.iter (fun e -> match e.TryGetTarget() with
                                                    | (true,v) -> registerFinalizer(v, finalizer, None)
                                                    | _ -> finalizer())

        member x.RegisterAnyFinalizer(f : unit -> unit) =
            x.RegisterAnyFinalizer(fun (o:obj) -> f())

        member x.RegisterAllFinalizer(f : unit -> unit) =
            x.RegisterAllFinalizer(fun (o:obj) -> f())




        override x.GetHashCode() = m_hashcode
        override x.Equals(o) =
            if System.Object.ReferenceEquals(x,o) then
                true
            else
                match o with
                    | :? WeakList<'a> as other -> if other.Target.Length <> m_elements.Length then
                                                        false
                                                   else
                                                        List.fold2 (fun b l r -> b && (l.Equals(r))) true m_elements other.Target
                    | _ -> false 