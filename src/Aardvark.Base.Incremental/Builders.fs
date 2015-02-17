namespace Aardvark.Base.Incremental


[<AutoOpen>]
module ``Computation Expression Builders`` =
    
    type AdaptiveBuilder() =
        member x.Bind(m : IMod<'a>, f : 'a -> IMod<'b>) =
            Mod.bind f m

        member x.Return (v : 'a) =
            Mod.initMod v :> IMod<_>

    type ASetBuilder() =
        member x.Yield (v : 'a) =
            ASet.single v

        member x.YieldFrom (set : aset<'a>) =
            set

        member x.For(s : aset<'a>, f : 'a -> aset<'b>) =
            ASet.collect f s

        member x.For(s : seq<'a>, f : 'a -> aset<'b>) =
            ASet.collect' f s

        member x.Zero() =
            ASet.empty

        member x.Delay(f : unit -> aset<'a>) =
            f()

        member x.Combine(l : aset<'a>, r : aset<'a>) =
            ASet.concat' [l;r]

    module Mod =
        let toASet (m : IMod<'a>) =
            ASet.ofMod m



    let adaptive = AdaptiveBuilder()
    let aset = ASetBuilder()
