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

[<Extension; AbstractClass; Sealed>]
type ModExtensions private() =
    [<Extension>]
    static member Apply (this : IMod<'a>, f : Func<'a, 'b>) =
        Mod.map f.Invoke this

    [<Extension>]
    static member Select (this : IMod<'a>, f : Func<'a, 'b>) =
        Mod.map f.Invoke this

    [<Extension>]
    static member Compose (this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, 'c>) =
        Mod.map2 (fun a b -> f.Invoke(a,b)) this other

    [<Extension>]
    static member Bind(this : IMod<'a>, f : Func<'a, IMod<'b>>) =
        Mod.bind f.Invoke this

    [<Extension>]
    static member Bind(this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, IMod<'c>>) =
        Mod.bind2 (fun l r -> f.Invoke(l,r)) this other

    [<Extension>]
    static member Bind(this : IMod<'a>, f : Func<'a, aset<'b>>) =
        ASet.bind f.Invoke this

    [<Extension>]
    static member Bind(this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, aset<'c>>) =
        ASet.bind2 (fun l r -> f.Invoke(l,r)) this other

    [<Extension>]
    static member Bind(this : IMod<'a>, f : Func<'a, alist<'b>>) =
        AList.bind f.Invoke this

    [<Extension>]
    static member Bind(this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, alist<'c>>) =
        AList.bind2 (fun l r -> f.Invoke(l,r)) this other

    [<Extension>]
    static member Always(this : IMod<'a>) =
        Mod.always this

    [<Extension>]
    static member ToAdaptiveSet(this : IMod<'a>) =
        Mod.toASet this

    [<Extension>]
    static member ToAdaptiveList(this : IMod<'a>) =
        Mod.toAList this


[<Extension; AbstractClass; Sealed>]
type AdaptiveSetExtensions private() =

    [<Extension>]
    static member Select (this : aset<'a>, f : Func<'a, 'b>) =
        ASet.map f.Invoke this

    [<Extension>]
    static member SelectMany (this : aset<'a>, f : Func<'a, aset<'b>>) =
        ASet.collect f.Invoke this

    [<Extension>]
    static member SelectMany (this : seq<'a>, f : Func<'a, aset<'b>>) =
        ASet.collect' f.Invoke this

    [<Extension>]
    static member Flatten (this : seq<aset<'a>>) =
        ASet.concat' this

    [<Extension>]
    static member Flatten (this : aset<aset<'a>>) =
        ASet.concat this

    [<Extension>]
    static member Where (this : aset<'a>, f : Func<'a, bool>) =
        ASet.filter f.Invoke this

    [<Extension>]
    static member Where (this : aset<'a>, f : Func<'a, IMod<bool>>) =
        ASet.filterM f.Invoke this

    [<Extension>]
    static member Union (this : aset<'a>, other : aset<'a>) =
        ASet.concat' [this; other]

[<Extension; AbstractClass; Sealed>]
type AdaptiveListExtensions private() =

    [<Extension>]
    static member Select (this : alist<'a>, f : Func<'a, 'b>) =
        AList.map f.Invoke this

    [<Extension>]
    static member SelectMany (this : alist<'a>, f : Func<'a, alist<'b>>) =
        AList.collect f.Invoke this

    [<Extension>]
    static member SelectMany (this : seq<'a>, f : Func<'a, alist<'b>>) =
        AList.collect' f.Invoke this

    [<Extension>]
    static member Concat (this : seq<alist<'a>>) =
        AList.concat' this

    [<Extension>]
    static member Concat (this : alist<alist<'a>>) =
        AList.concat this

    [<Extension>]
    static member Where (this : alist<'a>, f : Func<'a, bool>) =
        AList.filter f.Invoke this

    [<Extension>]
    static member Where (this : alist<'a>, f : Func<'a, IMod<bool>>) =
        AList.filterM f.Invoke this

    [<Extension>]
    static member Concat (this : alist<'a>, other : alist<'a>) =
        AList.concat' [this; other]

