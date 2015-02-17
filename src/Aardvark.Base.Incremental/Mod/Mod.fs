namespace Aardvark.Base.Incremental

open System

[<AllowNullLiteral>]
type IMod =
    inherit IAdaptiveObject
    abstract member GetValue : unit -> obj

[<AllowNullLiteral>]
type IMod<'a> =
    inherit IMod
    abstract member GetValue : unit -> 'a

type ModRef<'a>(value : 'a) =
    inherit AdaptiveObject()
    let mutable value = value

    member x.Value
        with get() = value
        and set v =
            if not <| Object.Equals(v, value) then
                value <- v
                x.MarkOutdated()

    interface IMod with
        member x.GetValue() = 
            x.OutOfDate <- false
            x.Value :> obj

    interface IMod<'a> with
        member x.GetValue() = 
            x.OutOfDate <- false
            x.Value

module Mod =
    
    type private LazyMod<'a> =
        class
            inherit AdaptiveObject
            val mutable public cache : 'a
            val mutable public compute : unit -> 'a

            member x.GetValue() =
                if x.OutOfDate then
                    x.cache <- x.compute()
                    x.OutOfDate <- false

                x.cache


            interface IMod with
                member x.GetValue() = x.GetValue() :> obj

            interface IMod<'a> with
                member x.GetValue() = x.GetValue()

            new(compute) =
                { cache = Unchecked.defaultof<'a>; compute = compute }
        end

    type private EagerMod<'a>(compute : unit -> 'a, eq : Option<'a -> 'a -> bool>) =
        inherit LazyMod<'a>(compute)

        let eq = defaultArg eq (fun a b -> System.Object.Equals(a,b))

        override x.Mark() =
            let newValue = compute()
            x.OutOfDate <- false

            if eq newValue x.cache then
                false
            else
                x.cache <- newValue
                true

        new(compute) = EagerMod(compute, None)


    let registerCallback (f : 'a -> unit) (m : IMod<'a>) =
        let self = ref id
        self := fun () ->
            try
                m.GetValue() |> f
            finally 
                m.MarkingCallbacks.Add !self |> ignore
        
        !self ()

        { new IDisposable with
            member x.Dispose() = m.MarkingCallbacks.Remove !self |> ignore
        }


    let initMod (v : 'a) =
        ModRef v

    let map (f : 'a -> 'b) (m : IMod<'a>) =
        let res = LazyMod(fun () -> m.GetValue() |> f)
        m.AddOutput res
        res :> IMod<_>

    let map2 (f : 'a -> 'b -> 'c) (m1 : IMod<'a>) (m2 : IMod<'b>)=
        let res = LazyMod(fun () -> f (m1.GetValue()) (m2.GetValue()))
        m1.AddOutput res
        m2.AddOutput res
        res :> IMod<_>


    let bind (f : 'a -> IMod<'b>) (m : IMod<'a>) =
        let res = ref <| Unchecked.defaultof<LazyMod<'b>>
        let inner : ref<Option<'a * IMod<'b>>> = ref None
        res := 
            LazyMod(fun () -> 
                let v = m.GetValue()

                match !inner with
                    | Some (v', inner) when Object.Equals(v, v') ->
                        inner.GetValue()
                    | _ ->
                        let i = f v
                        let old = !inner
                        inner := Some (v, i)

                        match old with
                            | None -> i.AddOutput !res |> ignore
                            | Some (_,old) when old <> i -> 
                                old.RemoveOutput !res |> ignore
                                i.AddOutput !res |> ignore

                            | _ -> ()

                        
                        i.GetValue()
                        

            )
        m.AddOutput !res |> ignore
        !res :> IMod<_>

    let force (m : IMod<'a>) =
        m.GetValue()

    let outOfDate (m : IMod<'a>) =
        m.OutOfDate

    let always (m : IMod<'a>) =
        match m with
            | :? EagerMod<'a> -> m
            | _ ->
                let res = EagerMod(fun () -> m.GetValue())
                m.AddOutput(res)
                res.GetValue() |> ignore
                res :> IMod<_>