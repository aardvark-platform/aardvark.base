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

    module State =
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

    module Maybe =
        type Maybe<'a> = Just of 'a | Nothing

        type MaybeBuilder() =
            member x.Bind(m : Maybe<'a>, f : 'a -> Maybe<'b>) : Maybe<'b> =
                match m with
                    | Just v -> f v
                    | Nothing -> Nothing

            member x.Return(v : 'a) =
                Just v

            member x.ReturnFrom(m : Maybe<'a>) =
                m

            member x.Zero() : Maybe<unit> =
                Just ()

            member x.Combine(l : Maybe<unit>, r : Maybe<'a>) : Maybe<'a> =
                match l with
                    | Just () -> r
                    | Nothing -> Nothing

            member x.For(elements : seq<'a>, f : 'a -> Maybe<unit>) : Maybe<unit> =
                let mutable r = Just ()
                for e in elements do
                    match r, f e with
                        | Nothing,_ -> ()
                        | _, Just () -> ()
                        | _, Nothing -> r <- Nothing
                r

        let maybe = MaybeBuilder()

    



    module StringBuilder =
        open System.Text

        type BuilderType = StringBuilder -> StringBuilder

        type StringBuilderType() =

            member x.Yield(v : string) : BuilderType =
                fun b -> b.Append v

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


        let test() =
            string {
                yield "asdsd"
            }
        

    let cont = Cont.cont
    let state = State.state
    let scont = StateCont.scont
    let maybe = Maybe.maybe
    let sopt = StateOpt.state
    let sseq = StateSeq.sseq
    
    let test() = 
        let a = state { return 1; }
        let b = state { return 3;}
        let c = state {
                    let! a = a
                    let! b = b

                    return a+b
                }

        printfn "%A" (c.runState 2)

    

