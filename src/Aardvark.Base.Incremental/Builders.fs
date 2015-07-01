namespace Aardvark.Base.Incremental


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
