namespace Aardvark.Base.Incremental.CSharp

open System
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental

[<Extension; AbstractClass; Sealed>]
type AdaptiveObjectExtensions private() =

    [<Extension>]
    static member AddOutput (this : IAdaptiveObject, o : IAdaptiveObject) =
        this.AddOutput(o)

    [<Extension>]
    static member RemoveOutput (this : IAdaptiveObject, o : IAdaptiveObject) =
        this.RemoveOutput(o)

    [<Extension>]
    static member Compose (this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, 'c>) =
        Mod.map2 (fun a b -> f.Invoke(a,b)) this other

    [<Extension>]
    static member Apply (this : IMod<'a>, f : Func<'a, 'b>) =
        Mod.map f.Invoke this

