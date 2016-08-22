namespace Aardvark.Base

module Monads =

    module Cont =
        type Cont<'r, 'a> = { runCont : ('a -> 'r) -> 'r }
    
        type ContBuilder() =
            member x.Bind(m : Cont<'r, 'a>, f : 'a -> Cont<'r, 'b>) : Cont<'r, 'b> =
                { runCont = fun c -> m.runCont (fun v -> (f v).runCont c) }

            member x.Return(v : 'a) : Cont<'r, 'a> =
                { runCont = fun c -> c v }

            member x.ReturnFrom(c : Cont<'r, 'a>) =
                c

            member x.Delay(f : unit -> Cont<'r, 'a>) : Cont<'r, 'a> =
                { runCont = fun c -> f().runCont c }

            member x.Combine(l : Cont<'r, unit>, r : Cont<'r, 'a>) : Cont<'r, 'a> =
                { runCont = fun c -> l.runCont (fun () -> r.runCont c) }

            member x.Zero() : Cont<'r, unit> =
                { runCont = fun c -> c () }
                  
            member x.For(elements : seq<'a>, f : 'a -> Cont<'r, unit>) : Cont<'r, unit> =
                { runCont = fun c ->

                    let mutable run = fun () -> c()
                    for e in elements do
                        let inner = run
                        run <- fun c -> (f e).runCont inner

                    run()
                }
                 
            member x.While(condition : unit -> bool, b : Cont<'r, unit>) : Cont<'r, unit> =
                { runCont = fun c ->
                    if condition() then
                        b.runCont (fun () -> x.While(condition, b).runCont c)
                    else
                        c ()
                }     

        let cont = ContBuilder()

        let test() =
            let a = cont { return 1 }
            let b = cont { return 2 }
            let c = cont {
                        let! a = a
                        let! b = b
                        return a + b 
                    }

            c.runCont (printfn "%d")

    module StateOld =
        type State<'s, 'a> = { runState : 's -> 'a * 's } 



        type StateBuilder() =
            member x.Bind(m : State<'s, 'a>, f : 'a -> State<'s, 'b>) : State<'s, 'b> =
                { runState = fun s -> let(v, s') = m.runState s in (f v).runState s' } 

            member x.Return(v : 'a) : State<'s, 'a> =
                { runState = fun s -> (v,s) }

            member x.ReturnFrom(s : State<'s, 'a>) =
                s

            member x.Delay(f : unit -> State<'s, 'a>) =
                { runState = fun s -> f().runState s }

            member x.Combine(l : State<'s, unit>, r : State<'s, 'a>) : State<'s, 'a> =
                x.Bind(l, fun () -> r)

            member x.Zero() : State<'s, unit> =
                { runState = fun s -> ((), s) }

            member x.For(elements : seq<'a>, f : 'a -> State<'s, unit>) : State<'s, unit> =
                { runState = fun s ->
                    let mutable current = s
                    for e in elements do
                        let ((), s') = (f e).runState current
                        current <- s'
                    ((), current)
                }

            member x.While(condition : unit -> bool, m : State<'s, unit>) : State<'s, unit> =
                { runState = fun s ->
                    let mutable current = s
                    while condition() do
                        let ((), s') = m.runState current
                        current <- s'
                    ((), current)
                }


        let state = StateBuilder()

        let putState s = { runState = fun _ -> ((), s) }
        let getState = { runState = fun s -> (s,s) }
        let modifyState f = { runState = fun s -> ((), f s) }


        let test() =
            let a = state { return 1 }
            let c = state {
                        let! s = getState
                        let! a = a
                        do! putState 10
                        return s + a
                    }

            let r = c.runState 2
            printfn "%A" r

    module State =

        [<AbstractClass>]
        type State<'s, 'a>() =
            abstract member Run : byref<'s> -> 'a
            abstract member RunUnit : byref<'s> -> unit

            default x.Run(s) = x.RunUnit(&s); Unchecked.defaultof<'a>
            default x.RunUnit(s) = x.Run(&s) |> ignore

        module State =
            
            let ignore (s : State<'s, 'a>) =
                { new State<'s, unit>() with
                    member x.RunUnit state =
                        s.RunUnit(&state)
                }

            let map (f : 'a -> 'b) (s : State<'s, 'a>) =
                { new State<'s, 'b>() with
                    member x.Run state =
                        let a = s.Run(&state)
                        f a
                }

            let bind (f : 'a -> State<'s, 'b>) (s : State<'s, 'a>) =
                { new State<'s, 'b>() with
                    member x.Run state =
                        let a = s.Run(&state) |> f
                        a.Run(&state)
                }

            let value (v : 'a) =
                { new State<'s, 'a>() with
                    member x.Run _ = v
                }

            let get<'s> =
                { new State<'s, 's>() with
                    member x.Run state = state
                }

            let put (s : 's) =
                { new State<'s, unit>() with
                    member x.RunUnit state = state <- s
                }

            let modify (f : 's -> 's) =
                { new State<'s, unit>() with
                    member x.RunUnit state = state <- f state
                }

            let custom (f : 's -> 's * 'a) =
                { new State<'s, 'a>() with
                    member x.Run state = 
                        let (s,r) = f state
                        state <- s
                        r
                }

            let run (state : 's) (m : State<'s, 'a>) =
                let mutable res = state
                let v = m.Run(&res)
                res, v

            let evaluate (state : 's) (m : State<'s, 'a>) =
                let mutable res = state
                let v = m.Run(&res)
                v

        module Seq =
            let mapS (f : 'a -> State<'s, 'b>) (input : seq<'a>) =
                { new State<'s, seq<'b>>() with
                    member x.Run state =
                        let result = System.Collections.Generic.List<'b>()
                        for e in input do
                            result.Add (f(e).Run(&state))
                        result :> seq<_>    
                }

            let collectS (f : 'a -> State<'s, seq<'b>>) (input : seq<'a>) =
                { new State<'s, seq<'b>>() with
                    member x.Run state =
                        let result = System.Collections.Generic.List<'b>()
                        for e in input do
                            result.AddRange (f(e).Run(&state))
                        result :> seq<_>    
                }

            let chooseS (f : 'a -> State<'s, Option<'b>>) (input : seq<'a>) =
                { new State<'s, seq<'b>>() with
                    member x.Run state =
                        let result = System.Collections.Generic.List<'b>()
                        for e in input do
                            match f(e).Run(&state) with
                                | Some res -> result.Add res
                                | _ -> ()
                        result :> seq<_>    
                }

        module List =
            let mapS (f : 'a -> State<'s, 'b>) (input : list<'a>) =
                { new State<'s, list<'b>>() with
                    member x.Run state =
                        let result = System.Collections.Generic.List<'b>()
                        for e in input do
                            result.Add (f(e).Run(&state))
                        result |> CSharpList.toList
                }

            let collectS (f : 'a -> State<'s, seq<'b>>) (input : list<'a>) =
                { new State<'s, list<'b>>() with
                    member x.Run state =
                        let result = System.Collections.Generic.List<'b>()
                        for e in input do
                            result.AddRange (f(e).Run(&state))
                        result |> CSharpList.toList
                }

            let chooseS (f : 'a -> State<'s, Option<'b>>) (input : list<'a>) =
                { new State<'s, list<'b>>() with
                    member x.Run state =
                        let result = System.Collections.Generic.List<'b>()
                        for e in input do
                            match f(e).Run(&state) with
                                | Some res -> result.Add res
                                | _ -> ()
                        result |> CSharpList.toList
                }


        type StateBuilder() =
            member x.Bind(m : State<'s, 'a>, f : 'a -> State<'s, 'b>) =
                State.bind f m

            member x.Return(v : 'a) = 
                State.value v

            member x.ReturnFrom(m : State<'s, 'a>) =
                m

            member x.Delay(f : unit -> State<'s, 'a>) =
                { new State<'s, 'a>() with
                    member x.Run(state) =
                        f().Run(&state)
                }

            member x.Combine(l : State<'s, unit>, r : State<'s, 'a>) =
                { new State<'s, 'a>() with
                    member x.Run(state) =
                        l.Run(&state)
                        r.Run(&state)
                }

            member x.Zero() : State<'s, unit> =
                { new State<'s, unit>() with
                    member x.RunUnit(state) =
                        ()
                }

            member x.For(input : seq<'a>, f : 'a -> State<'s, unit>) =
                { new State<'s, unit>() with
                    member x.RunUnit(state) =
                        for e in input do
                            f(e).Run(&state)
                }

            member x.While(guard : unit -> bool, body : State<'s, unit>) =
                { new State<'s, unit>() with
                    member x.RunUnit(state) =
                        while guard() do
                            body.Run(&state)
                }

            member x.TryFinally(m : State<'s, 'a>, fin : unit -> unit) =
                { new State<'s, 'a>() with
                    member x.Run(state) =
                        try m.Run(&state)
                        finally fin()
                }

            member x.TryWith(m : State<'s, 'a>, handler : exn -> State<'s, 'a>) =
                { new State<'s, 'a>() with
                    member x.Run(state) =
                        let mutable local = state
                        try 
                            let res = m.Run(&local)
                            state <- local
                            res
                        with e ->  
                            handler(e).Run(&state)
                }

            member x.Using(v : 'a, f : 'a -> State<'s, 'b>) =
                { new State<'s, 'b>() with
                    member x.Run(state) = 
                        use v = v
                        let res = f(v).Run(&state)
                        res
                }

            member x.TryFinally(m : State<'s, 'a>, fin : unit -> State<'s, unit>) =
                { new State<'s, 'a>() with
                    member x.Run(state) =
                        try m.Run(&state)
                        finally fin().Run(&state)
                }

            member x.TryFinally(m : State<'s, 'a>, fin : unit -> 's -> 's) =
                { new State<'s, 'a>() with
                    member x.Run(state) =
                        try m.Run(&state)
                        finally state <- fin () state
                }

        let state = StateBuilder()


    module StateOpt =

        type State<'s, 'a> = { runState : 's -> Option<'a * 's> } 

        type StateBuilder() =
            member x.Bind(m : State<'s, 'a>, f : 'a -> State<'s, 'b>) : State<'s, 'b> =
                { runState = fun s -> match m.runState s with
                                       | Some (v, s') -> (f v).runState s'
                                       | None -> None } 

            member x.Return(v : 'a) : State<'s, 'a> =
                { runState = fun s -> Some (v,s) }

            member x.ReturnFrom(s : State<'s, 'a>) =
                s

            member x.Delay(f : unit -> State<'s, 'a>) =
                { runState = fun s -> f().runState s }

            member x.Combine(l : State<'s, unit>, r : State<'s, 'a>) : State<'s, 'a> =
                x.Bind(l, fun () -> r)

            member x.Zero() : State<'s, unit> =
                { runState = fun s -> Some ((), s) }

            member x.For(elements : seq<'a>, f : 'a -> State<'s, unit>) : State<'s, unit> =
                { runState = fun s ->
                    if Seq.isEmpty elements then Some ((), s)
                    else 
                        match (f <| Seq.head elements).runState s with
                         | None -> None
                         | Some ((),s') -> x.For(Seq.skip 1 elements, f).runState s' 
                }


        let state = StateBuilder()

        let putState s = { runState = fun _ -> Some ((), s) }
        let getState = { runState = fun s -> Some (s,s) }
        let modifyState f = { runState = fun s -> Some ((), f s) }
        let evalState (m : State<'s,'a>) (state : 's) = m.runState state 

        let fail = { runState = fun s -> None }

        let rec map2M (f : 'a -> 'b -> State<'s,'c>) (l0 : list<'a>) (l1 : list<'b>) : State<'s,list<'c>> =
            state {
                match l0,l1 with
                 | x::xs, y::ys -> let! c = f x y
                                   let! rest = map2M f xs ys
                                   return c :: rest
                 | [],[] -> return []
                 | _ -> return failwith "map2M list counts differ."
            }

    module StateCont =
        type StateCont<'s, 'r, 'a> = { runStateCont : 's -> ('a * 's -> 'r) -> 'r }

        type StateContBuilder() =
            member x.Bind(m : StateCont<'s, 'r, 'a>, f : 'a -> StateCont<'s, 'r, 'b>) : StateCont<'s, 'r, 'b> =
                { runStateCont = fun s c -> m.runStateCont s (fun (v,s') -> (f v).runStateCont s' c) }
            
            member x.Return(v : 'a) : StateCont<'s, 'r, 'a> =
                { runStateCont = fun s c -> c(v, s) }

            member x.ReturnFrom(m : StateCont<'s, 'r, 'a>) =
                m

            member x.Delay(f : unit -> StateCont<'s, 'r, 'a>) : StateCont<'s, 'r, 'a> =
                { runStateCont = fun s c -> f().runStateCont s c }

            member x.Combine(l : StateCont<'s, 'r, unit>, r : StateCont<'s, 'r, 'a>) : StateCont<'s, 'r, 'a> =
                x.Bind(l, fun () -> r)

            member x.Zero() : StateCont<'s, 'r, unit> =
                { runStateCont = fun s c -> c((),s) }

            member x.For(elements : seq<'a>, f : 'a -> StateCont<'s, 'r, unit>) : StateCont<'s, 'r, unit> =
                { runStateCont = fun s c ->

                    let mutable run = fun s -> c((), s)
                    for e in elements do
                        let inner = run
                        run <- fun s -> (f e).runStateCont s (fun ((), s') -> inner s')

                    run s
                }
                 
            member x.While(condition : unit -> bool, b : StateCont<'s, 'r, unit>) : StateCont<'s, 'r, unit> =
                { runStateCont = fun s c ->
                    if condition() then
                        b.runStateCont s (fun ((),s') -> x.While(condition, b).runStateCont s' c)
                    else
                        c ((),s)
                }     


        let scont = StateContBuilder()

        let putState s = { runStateCont = fun _ c -> c((), s) }
        let getState = { runStateCont = fun s c -> c(s,s) }
        let modifyState f = { runStateCont = fun s c -> c((), f s) }

        let test() =
            let a = scont { return 1 }
            let c = scont {
                        let! s = getState
                        let! a = a
                        for i in 1..10 do
                            do! modifyState ((+)1)

                        let! s' = getState
                        return s' - s + a
                    }

            c.runStateCont 0 (printfn "%A")

    module StateSeq =
        type StateSeq<'s, 'a> = { runStateSeq : 's -> 's * Option<'a * StateSeq<'s, 'a>> }
        
        let empty<'s, 'a> : StateSeq<'s, 'a> =  { runStateSeq = fun s -> s,None }

        type StateSeqBuilder() =
            member x.Yield(v : 'a) =
                { runStateSeq = fun s -> s, Some(v, empty) }

            member x.Bind(m : StateSeq<'s, 'a>, f : 'a -> StateSeq<'s, 'b>) : StateSeq<'s, 'b> =
                { runStateSeq = fun s -> 
                    let s, o = m.runStateSeq s
                    match o with
                        | Some(v,_) -> (f v).runStateSeq s
                        | None -> failwith "empty sequence" }

            member x.Return( v : 'a ) = { runStateSeq = fun s -> s, Some(v, empty) }

            member x.Zero() = empty

            member x.Combine(l : StateSeq<'s, 'a>, r : StateSeq<'s, 'a>) =
                { runStateSeq = fun s ->
                    let s, o = l.runStateSeq s
                    match o with
                        | None -> r.runStateSeq s
                        | Some(v,c) -> s,Some(v, x.Combine(c,r))
                }

            member x.For(self : StateSeq<'s, 'a>, f : 'a -> StateSeq<'s, 'b>) : StateSeq<'s, 'b> =
                { runStateSeq = fun s ->
                    let s, o = self.runStateSeq s
                    match o with
                        | None -> s,None
                        | Some(h,cSelf) ->
                            let s,o = (f h).runStateSeq s
                            match o with
                                | Some(v,cResult) -> x.Combine(x.Return(v), x.Combine(cResult, x.For(cSelf, f))).runStateSeq s
                                | None -> x.For(cSelf, f).runStateSeq s

                }

            member x.Delay(f : unit -> StateSeq<'s, 'a>) =
                { runStateSeq = fun s -> (f ()).runStateSeq s }

            member x.For(self : seq<'a>, f : 'a -> StateSeq<'s, 'b>) : StateSeq<'s, 'b> =
                if Seq.isEmpty self then
                    empty
                else
                    let h = Seq.head self
                    x.Combine(f h, x.For(Seq.skip 1 self, f))

        let putState s = { runStateSeq = fun _ -> s,Some((), empty) }
        let getState = { runStateSeq = fun s -> s,Some (s,empty) }
        let rec toSeq (state : 's) (m : StateSeq<'s, 'a>) : seq<'s * 'a> =
            seq {
                let s,o = m.runStateSeq state
                match o with
                    | Some(v,c) ->
                        yield (s,v)
                        yield! toSeq s c
                    | None -> ()
            }

        let sseq = StateSeqBuilder()


        let a() =
            sseq {
                do! putState -12
                printfn "yield 1"
                yield 1
                
                do! putState 123
                printfn "yield 2"
                yield 2
            }
        let b() = 
            sseq {
                for a in a() do
                    printfn "loop for %d" a
                    yield 11 * a
            }

        let test() =
            b() |> toSeq 5 |> printfn "%A"

    module Option = 
        open System.Collections.Generic

        type OptionBuilder() =
            member x.Bind(m : Option<'a>, f : 'a -> Option<'b>) : Option<'b> =
                match m with
                    | Some v -> f v
                    | None -> None

            member x.Return(v : 'a) =
                Some v

            member x.ReturnFrom(m : Option<'a>) =
                m

            member x.Zero() : Option<unit> =
                Some ()

            member x.Combine(l : Option<unit>, r : unit -> Option<'a>) : Option<'a> =
                match l with
                    | Some () -> r ()
                    | None -> None

            member x.For(elements : seq<'a>, f : 'a -> Option<unit>) : Option<unit> =
                use enumerator = elements.GetEnumerator()
                let rec fold (e : IEnumerator<_>) =
                    if e.MoveNext() then
                        match f e.Current with
                         | Some v -> fold e
                         | None -> None
                    else Some ()
                fold enumerator

            member x.Delay(f : unit -> Option<'a>) =
                f

            member x.Run(f : unit -> Option<'a>) = f ()

            member x.While(g : unit -> bool, b : unit -> Option<unit>) =
                let rec run () =
                    if g () then
                        match b () with
                         | Some v -> run ()
                         | None -> None
                    else Some ()
                run ()

            member x.TryFinally(m : unit -> Option<'a>, fin : unit -> unit) =
                try m()
                finally fin()

            member x.TryWith(m : unit -> Option<'a>, handler : exn -> Option<'a>) =
                try m()
                with e -> handler e

        let option = OptionBuilder()

   


    module StringBuilder =
        open System.Text

        type BuilderType = StringBuilder -> StringBuilder

        type StringBuilderType() =

            member x.Yield(v : string) : BuilderType =
                fun b -> b.Append v

            member x.YieldFrom(v : seq<string>) : BuilderType =
                fun b -> v |> Seq.iter (ignore << b.Append); b

            member x.For(s : #seq<'a>, f : 'a -> BuilderType) : BuilderType =
                fun b ->
                    let mutable r = b
                    for e in s do
                        r <- f e r
                    r

            member x.Delay(f : unit -> BuilderType) : BuilderType = 
                fun b -> f () b

            member x.Zero() : BuilderType = 
                fun b -> b

            member x.Run(b : BuilderType) =
                let input = StringBuilder()
                let output = b input
                output.ToString()

        let string = StringBuilderType()
        

    let cont = Cont.cont
    let state = State.state
    let scont = StateCont.scont
    let option = Option.option
    let sopt = StateOpt.state
    let sseq = StateSeq.sseq
    

    


module Cancellable =

    type CancelState = { comp : list<unit->unit> ; ct : System.Threading.CancellationToken }
    type Cancellable<'a> = { runState : CancelState -> 'a * CancelState } 

    let throwAndRollback (s : CancelState) =
        if s.ct.IsCancellationRequested then 
            for c in s.comp do c ()
            s.ct.ThrowIfCancellationRequested()
        else ()

    module Cancellable =
        
        let onCancel (f : unit -> unit) : Cancellable<unit> =
            { runState = fun s -> (), { s with comp = f::s.comp } }

        let run (ct : System.Threading.CancellationToken) (m : Cancellable<'a>) =
            try
                let (v,s') = m.runState { comp = []; ct = ct }
                ct.Register(new System.Action(fun () -> for c in s'.comp do c())) |> ignore
                Some v
            with | :? System.OperationCanceledException as o -> 
                None

        let start (m : Cancellable<unit>) (ct : System.Threading.CancellationToken) =
            Async.Start (async { return run ct m |> ignore } )

        let ignore (m : Cancellable<'a>) : Cancellable<unit> =
            { runState = 
                fun s ->
                    let (_,s') = m.runState s 
                    (), s' 
            }

        let withCompensation (f : 'a -> unit) (m : Cancellable<'a>) =
           { runState = 
                fun s ->
                    let (r,s') = m.runState s 
                    let comp () = f r
                    r, { s' with comp = comp :: s'.comp }
            }

        let tryFinallyAndUndo (f : unit -> (unit * ('a -> unit))) (m : Cancellable<'a>) =
           { runState = 
                fun s ->
                    let (r,s') = m.runState s 
                    throwAndRollback s'
                    let (), undo = f ()
                    r, { s' with comp = (fun () -> undo r) :: s'.comp }
            }

        let tryFinally (f : unit -> unit) (m : Cancellable<'a>) =
           { runState = 
                fun s ->
                    let (r,s') = m.runState s 
                    throwAndRollback s'
                    f ()
                    r, s'
            }

        let cancellationToken () =  
            { runState =
                fun s -> s.ct, s 
            }

    
    type CancelBuilder() =

        member x.Bind(m : Cancellable<'a>, f : 'a -> Cancellable<'b>) : Cancellable<'b> =
            { runState = 
                fun s -> 
                    throwAndRollback s
                    let (v, s') = m.runState s 
                    throwAndRollback s'
                    (f v).runState s' } 

        member x.Return(v : 'a) : Cancellable<'a> =
            { runState = 
                fun s -> 
                    throwAndRollback s
                    (v,s) }

        member x.ReturnFrom(s : Cancellable<'a>) =
            s

        member x.TryFinally(m : Cancellable<'a>, f : unit -> (unit * ('a -> unit))) =
            Cancellable.tryFinallyAndUndo f m

        member x.TryFinally(m : Cancellable<'a>, f : unit -> unit) =
            Cancellable.tryFinally f m

        member x.Delay(f : unit -> Cancellable<'a>) =
            { runState = fun s -> throwAndRollback s; f().runState s }

        member x.Combine(l : Cancellable<unit>, r : Cancellable<'a>) : Cancellable<'a> =
            x.Bind(l, fun () -> r)

        member x.Zero() : Cancellable<unit> =
            { runState = fun s -> ((), s) }

    let cancel = CancelBuilder()

    module Test =
        open System.Threading

        let test () =
            let a = 
                cancel {
                    Thread.Sleep 100
                    do! Cancellable.onCancel (fun () -> printfn "undo a")
                    return 1
                }
            let b = 
                cancel {
                    Thread.Sleep 100
                    do! Cancellable.onCancel (fun () -> printfn "undo b")
                    return 2
                }
            let c = 
                cancel {
                    Thread.Sleep 100
                    do! Cancellable.onCancel (fun () -> printfn "undo c")
                    return 3
                }
            let d = 
                cancel {
                    let! a = a
                    printfn "a: %A" a
                    let! b = b
                    printfn "b: %A" b
                    let! c = c
                    printfn "c: %A" c
                    let r = a + b + c
                    printfn "r: %A" r
                    return r
                }
            use cts = new System.Threading.CancellationTokenSource()
            Cancellable.start (Cancellable.ignore d) cts.Token
            Thread.Sleep 300
            cts.Cancel()
            printfn "done"


    module StatefulStepVar =    
        open System
        open System.Threading

        type Step<'a,'b> = { run : 'a -> Cancellable<'b>; }

        let ofPrimitive (m : 'a -> Cancellable<'ba>) =
            { run = m }

        let compose (f : Step<'a,'b>) (g : Step<'b,'c>) (fuse : 'b -> 'c -> 'd) : Step<'a,'d> =
            { run = fun a -> 
                cancel {
                    let! a = f.run a
                    let! b = g.run a
                    return fuse a b
                }
            }

        module Step =
            let ofFun (f : 'a -> Cancellable<'b>) = { run = fun a -> f a }
            
            let run (f : Step<unit,'a>) (ct : System.Threading.CancellationToken) =
                let result = System.Threading.Tasks.TaskCompletionSource<_>()
                try
                    let r = f.run ()
                    result.SetResult r
                with | :? System.OperationCanceledException as o -> result.SetCanceled()
                     | e -> result.SetException e   
                result

            let runRef (f : Step<unit,'a>) (ct : System.Threading.CancellationToken) =
                let result = ref None
                try
                    let r = f.run () |> Cancellable.run ct
                    result := r
                with | e -> result := None
                result

        let (<*>) f g = compose f (Step.ofFun g) (fun a b -> a,b)
        let ( **>) f g = compose (Step.ofFun (fun () -> f)) (Step.ofFun g) (fun a b -> a,b)
        let ( *>) f g = compose f g (fun a b -> b)

        module Test =
            let test () =
                let multiState =
                    cancel { 
                            try 
                                Thread.Sleep 100; 
                                return 1 
                            finally 
                                printfn "do operation", fun i -> printfn "undo operation: %d" i
                        } 
                    **> (fun i  -> cancel { Thread.Sleep 100; return 1.0   } |> Cancellable.withCompensation (printfn "undid: %f")  ) 
                    <*> (fun d  -> cancel { Thread.Sleep 100; return "abc" } |> Cancellable.withCompensation (printfn "undid: %s")  )
                
                let cts = new System.Threading.CancellationTokenSource()
                cts.CancelAfter(200)
                let r = Step.runRef multiState cts.Token
                Console.ReadLine() |> ignore
                printfn "%A" r