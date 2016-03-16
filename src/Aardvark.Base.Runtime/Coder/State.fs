namespace Aardvark.Base

open System
open System.Runtime.CompilerServices
open Aardvark.Base


[<AbstractClass>]
type State<'s, 'a>() =
    abstract member Run : byref<'s> -> 'a
    abstract member RunUnit : byref<'s> -> unit

    default x.Run(s) = x.RunUnit(&s); Unchecked.defaultof<'a>
    default x.RunUnit(s) = x.Run(&s) |> ignore


module State =
    

    type private EmptyState<'s>() =
        inherit State<'s, unit>()
        static let instance = EmptyState<'s>() :> State<'s, unit>
        override x.RunUnit(s) = ()
        static member Instance = instance

    type private GetState<'s>() =
        inherit State<'s, 's>()
        static let instance = GetState<'s>() :> State<'s, 's>
        override x.Run(s) = s
        static member Instance = instance

    type private PutState<'s>(value : 's) =
        inherit State<'s, unit>()
        override x.RunUnit(s) = s <- value

    type private ModifyState<'s>(value : 's -> 's) =
        inherit State<'s, unit>()
        override x.RunUnit(s) = s <- value s

    let empty<'s> = EmptyState<'s>.Instance

    let ofValue (v : 'a) =
        { new State<'s, 'a>() with
            override x.Run(s) = v
        }

    let map (f : 'a -> 'b) (m : State<'s, 'a>) =
        { new State<'s, 'b>() with
            override x.Run(s) = m.Run(&s) |> f
        }

    let bind (f : 'a -> State<'s, 'b>) (m : State<'s, 'a>) =
        { new State<'s, 'b>() with
            override x.Run(s) = 
                let v = m.Run(&s)
                (f v).Run(&s)
        }

    let delay (f : unit -> State<'s, 'a>) =
        { new State<'s, 'a>() with
            override x.Run(s) = f().Run(&s)
        }

    let combine (l : State<'s, unit>) (r : State<'s, 'a>) =
        { new State<'s, 'a>() with
            override x.Run(s) =
                l.RunUnit(&s)
                r.Run(&s)
        }


    let get<'s> = GetState<'s>.Instance

    let put (s : 's) = PutState(s) :> State<_,_>

    let modify (f : 's -> 's) = ModifyState(f) :> State<_,_>

    let run (state : 's) (m : State<'s, 'a>) =
        let mutable state = state
        let v = m.Run(&state)
        state, v

module StateSeq =
    open System.Collections.Generic

    let map (f : 'a -> State<'s, 'b>) (elements : seq<'a>) =
        { new State<'s, seq<'b>>() with
            override x.Run(s) =
                let res = List<'b>()
                for e in elements do
                    (f e).Run(&s) |> res.Add

                res :> seq<_>
        }

    let choose (f : 'a -> State<'s, Option<'b>>) (elements : seq<'a>) =
        { new State<'s, seq<'b>>() with
            override x.Run(s) =
                let res = List<'b>()
                for e in elements do
                    match (f e).Run(&s) with
                        | Some v -> res.Add v
                        | None -> ()

                res :> seq<_>
        }

    let filter (f : 'a -> State<'s, bool>) (elements : seq<'a>) =
        { new State<'s, seq<'a>>() with
            override x.Run(s) =
                let res = List<'a>()
                for e in elements do
                    if (f e).Run(&s) then
                        res.Add e

                res :> seq<_>
        }

    let collect (f : 'a -> State<'s, #seq<'b>>) (elements : seq<'a>) =
        { new State<'s, seq<'b>>() with
            override x.Run(s) =
                let res = List<'b>()
                for e in elements do
                    let elems = (f e).Run(&s)
                    res.AddRange elems

                res :> seq<_>
        }

module StateList =
    open System.Collections.Generic

    let map (f : 'a -> State<'s, 'b>) (elements : list<'a>) =
        { new State<'s, list<'b>>() with
            override x.Run(s) =
                let res = List<'b>()
                for e in elements do
                    (f e).Run(&s) |> res.Add

                res |> CSharpList.toList
        }

    let choose (f : 'a -> State<'s, Option<'b>>) (elements : list<'a>) =
        { new State<'s, list<'b>>() with
            override x.Run(s) =
                let res = List<'b>()
                for e in elements do
                    match (f e).Run(&s) with
                        | Some v -> res.Add v
                        | None -> ()

                res |> CSharpList.toList
        }

    let filter (f : 'a -> State<'s, bool>) (elements : list<'a>) =
        { new State<'s, list<'a>>() with
            override x.Run(s) =
                let res = List<'a>()
                for e in elements do
                    if (f e).Run(&s) then
                        res.Add e

                res |> CSharpList.toList
        }

    let collect (f : 'a -> State<'s, list<'b>>) (elements : list<'a>) =
        { new State<'s, list<'b>>() with
            override x.Run(s) =
                let res = List<'b>()
                for e in elements do
                    let elems = (f e).Run(&s)
                    res.AddRange elems

                res |> CSharpList.toList
        }

[<AbstractClass; Sealed; Extension>]
type StateExtensions private() =
    
    [<Extension>]
    static member run(this : State<'s, 'a>, state : 's) =
        let mutable state = state
        let res = this.Run(&state)
        state, res



[<AutoOpen>]
module StateBuilder =
    

    type StateBuilder() =
        member inline x.Bind(m : State<'s, 'a>, f : 'a -> State<'s, 'b>) = State.bind f m
        member inline x.Return(v : 'a) = State.ofValue v
        member inline x.ReturnFrom(s : State<'s, 'a>) = s
        member inline x.Delay(f : unit -> State<'s, 'a>) = State.delay f
        member inline x.Combine(l : State<'s, unit>, r : State<'s, 'a>) = State.combine l r
        member inline x.Zero() = State.empty
        member inline x.For (elements : seq<'a>, f : 'a -> State<'s, unit>) =
            { new State<'s, unit>() with
                override x.RunUnit(s) =
                    for e in elements do
                        (f e).RunUnit(&s)
            }
        member inline x.While (guard : unit -> bool, body : State<'s, unit>) =
            { new State<'s, unit>() with
                override x.RunUnit(s) =
                    while guard() do
                        body.Run(&s)
            }

    type SpecificStateBuilder<'s>() =
        member inline x.Bind(m : State<'s, 'a>, f : 'a -> State<'s, 'b>) = State.bind f m
        member inline x.Return(v : 'a) = State.ofValue v
        member inline x.ReturnFrom(s : State<'s, 'a>) = s
        member inline x.Delay(f : unit -> State<'s, 'a>) = State.delay f
        member inline x.Combine(l : State<'s, unit>, r : State<'s, 'a>) = State.combine l r
        member inline x.Zero() = State.empty
        member inline x.For (elements : seq<'a>, f : 'a -> State<'s, unit>) =
            { new State<'s, unit>() with
                override x.RunUnit(s) =
                    for e in elements do
                        (f e).RunUnit(&s)
            }
        member inline x.While (guard : unit -> bool, body : State<'s, unit>) =
            { new State<'s, unit>() with
                override x.RunUnit(s) =
                    while guard() do
                        body.Run(&s)
            }


    let state = StateBuilder()
