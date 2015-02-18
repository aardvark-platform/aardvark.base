namespace Aardvark.Base.Incremental

open System.Collections
open System.Collections.Generic


[<AbstractClass>]
type DerivedSet<'a>(s : ISet<'a>) =

    member x.InnerSet = s

    abstract member Add : 'a -> bool
    abstract member Remove : 'a -> bool
    abstract member Contains : 'a -> bool
    abstract member Clear : unit -> unit
    abstract member CopyTo : 'a[] * int -> unit
    abstract member Count : int
    abstract member IsReadOnly : bool
    abstract member ExceptWith : seq<'a> -> unit
    abstract member IntersectWith : seq<'a> -> unit
    abstract member IsProperSubsetOf : seq<'a> -> bool
    abstract member IsProperSupersetOf : seq<'a> -> bool
    abstract member IsSubsetOf : seq<'a> -> bool
    abstract member IsSupersetOf : seq<'a> -> bool
    abstract member UnionWith : seq<'a> -> unit
    abstract member SymmetricExceptWith : seq<'a> -> unit
    abstract member Overlaps : seq<'a> -> bool
    abstract member SetEquals : seq<'a> -> bool
    abstract member GetEnumerator : unit -> IEnumerator<'a>

    default x.Add v = s.Add v
    default x.Remove v = s.Remove v
    default x.Contains v = s.Contains v
    default x.Clear() = s.Clear()
    default x.CopyTo(arr,idx) = s.CopyTo(arr,idx)
    default x.Count = s.Count
    default x.IsReadOnly = s.IsReadOnly
    default x.ExceptWith o = s.ExceptWith o
    default x.IntersectWith o = s.IntersectWith o
    default x.IsProperSubsetOf o = s.IsProperSubsetOf o
    default x.IsProperSupersetOf o = s.IsProperSupersetOf o
    default x.IsSubsetOf o = s.IsSubsetOf o
    default x.IsSupersetOf o = s.IsSupersetOf o
    default x.UnionWith o = s.UnionWith o
    default x.SymmetricExceptWith o = s.SymmetricExceptWith o
    default x.Overlaps o = s.Overlaps o
    default x.SetEquals o = s.SetEquals o
    default x.GetEnumerator() = s.GetEnumerator()

    interface ISet<'a> with
        member x.Add v = x.Add v
        member x.Add v = x.Add v |> ignore
        member x.Remove v = x.Remove v
        member x.Contains v = x.Contains v
        member x.Clear() = x.Clear()
        member x.CopyTo(arr,idx) = x.CopyTo(arr,idx)
        member x.Count = x.Count
        member x.IsReadOnly = x.IsReadOnly
        member x.ExceptWith o = x.ExceptWith o
        member x.IntersectWith o = x.IntersectWith o
        member x.IsProperSubsetOf o = x.IsProperSubsetOf o
        member x.IsProperSupersetOf o = x.IsProperSupersetOf o
        member x.IsSubsetOf o = x.IsSubsetOf o
        member x.IsSupersetOf o = x.IsSupersetOf o
        member x.UnionWith o = x.UnionWith o
        member x.SymmetricExceptWith o = x.SymmetricExceptWith o
        member x.Overlaps o = x.Overlaps o
        member x.SetEquals o = x.SetEquals o
        member x.GetEnumerator() = x.GetEnumerator()
        member x.GetEnumerator() = x.GetEnumerator() :> System.Collections.IEnumerator