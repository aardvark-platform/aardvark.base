namespace Aardvark.Base.Incremental

/// provides special operators for composing IMods
module Operators =
    
    /// adaptively adds two values
    let inline (%+) (l : IMod<'a>) (r : IMod<'b>) = Mod.map2 (+) l r

    /// adaptively subtracts two values
    let inline (%-) (l : IMod<'a>) (r : IMod<'b>) = Mod.map2 (-) l r

    /// adaptively mutiplies two values
    let inline (%*) (l : IMod<'a>) (r : IMod<'b>) = Mod.map2 (*) l r

    /// adaptively divides two values
    let inline (%/) (l : IMod<'a>) (r : IMod<'b>) = Mod.map2 (/) l r

    /// creates an adaptive cell providing the value of "l && r"
    let inline (%&&) (l : IMod<bool>) (r : IMod<bool>) = Mod.map2 (&&) l r

    /// creates an adaptive cell providing the value of "l || r"
    let inline (%||) (l : IMod<bool>) (r : IMod<bool>) = Mod.map2 (||) l r

    /// expresses an adaptive "if then else" expression (e.g. m %? a %. b <=> if m then a else b)
    let (%?) (l : IMod<bool>) (vt : 'a) : 'a -> IMod<'a> = fun vf -> l |> Mod.map (fun v -> if v then vt else vf)

    /// expresses an adaptive "if then else" expression (e.g. m %? a %. b <=> if m then a else b)
    let (%.) (l : 'a -> IMod<'a>) (r : 'a) =  l r

    /// forces the value of a cell
    let inline (!!) (m : IMod<'a>) = Mod.force m

    /// creates a constant cell containing the given value
    let inline (~~) (v : 'a) = Mod.constant v

    /// creates a changeable cell containing the given value
    let inline mref (v : 'a) = Mod.init v



[<AutoOpen>]
module ``Computation Expression Builders`` =
    
    type AdaptiveBuilder() =
        let constantUnit = Mod.constant ()

        member x.Bind(tup : IMod<'a> * IMod<'b>, f : 'a * 'b -> IMod<'c>) : IMod<'c> =
            Mod.bind2 (fun a b -> f(a,b)) (fst tup) (snd tup)

        member x.Bind(m : IMod<'a>, f : 'a -> IMod<'b>) =
            Mod.bind f m

        member x.Return (v : 'a) =
            Mod.constant v :> IMod<_>

        member x.ReturnFrom(m : IMod<'a>) = 
            m

        member x.Zero() = constantUnit

        member x.Combine(l : IMod<unit>, r : IMod<'a>) =
            Mod.map2 (fun () r -> r) l r

        member x.Delay (f : unit -> IMod<'a>) =
            constantUnit |> Mod.bind f

        

    type ASetBuilder() =
        member x.Yield (v : 'a) =
            ASet.single v

        member x.YieldFrom (set : aset<'a>) =
            set

        member x.Bind(m : IMod<'a> * IMod<'b>, f : 'a * 'b -> aset<'c>) =
            ASet.bind2 (fun a b -> f(a,b)) (fst m) (snd m)

        member x.Bind(m : IMod<'a>, f : 'a -> aset<'b>) =
            ASet.bind f m

        member x.For(s : aset<'a>, f : 'a -> aset<'b>) =
            ASet.collect f s

        member x.For(s : seq<'a>, f : 'a -> aset<'b>) =
            ASet.collect' f s

        member x.Zero() =
            ASet.empty

        member x.Delay(f : unit -> aset<'a>) =
            f()

        member x.Combine(l : aset<'a>, r : aset<'a>) =
            ASet.union' [l;r]

    type AListBuilder() =
        member x.Yield (v : 'a) =
            AList.single v

        member x.YieldFrom (set : alist<'a>) =
            set

        member x.Bind(m : IMod<'a> * IMod<'b>, f : 'a * 'b -> alist<'c>) =
            AList.bind2 (fun a b -> f(a,b)) (fst m) (snd m)

        member x.Bind(m : IMod<'a>, f : 'a -> alist<'b>) =
            AList.bind f m

        member x.For(s : alist<'a>, f : 'a -> alist<'b>) =
            AList.collect f s

        member x.For(s : seq<'a>, f : 'a -> alist<'b>) =
            AList.collect' f s

        member x.Zero() =
            AList.empty

        member x.Delay(f : unit -> alist<'a>) =
            f()

        member x.Combine(l : alist<'a>, r : alist<'a>) =
            AList.concat' [l;r]


    module Mod =
        let toASet (m : IMod<'a>) =
            ASet.ofMod m

        let toAList (m : IMod<'a>) =
            AList.ofMod m


    let adaptive = AdaptiveBuilder()
    let aset = ASetBuilder()
    let alist = AListBuilder()

open System.Runtime.CompilerServices

[<AbstractClass; Sealed; Extension>]
type EvaluationExtensions() =
    [<Extension>]
    static member inline GetValue(x : IMod<'a>) = x.GetValue(null)

    [<Extension>]
    static member inline GetDelta(x : IReader<'a>) = x.GetDelta(null)
    [<Extension>]
    static member inline Update(x : IReader<'a>) = x.Update(null)

    [<Extension>]
    static member inline GetDelta(x : IListReader<'a>) = x.GetDelta(null)
    [<Extension>]
    static member inline Update(x : IListReader<'a>) = x.Update(null)

    [<Extension>]
    static member inline GetHistory(x : IStreamReader<'a>) = x.GetHistory(null)
