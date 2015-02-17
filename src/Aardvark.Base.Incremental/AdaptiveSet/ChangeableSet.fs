namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental.ASetReaders

[<CompiledName("ChangeableSet")>]
type cset<'a>(initial : seq<'a>) =
    let content = HashSet initial
    let readers = WeakSet<BufferedReader<'a>>()

    interface aset<'a> with
        member x.GetReader() =
            let r = new BufferedReader<'a>(fun r -> readers.Remove r |> ignore)
            r.Emit (content |> Seq.map Add |> Seq.toList)
            readers.Add r |> ignore
            r :> _

    member x.Count = content.Count

    member x.Contains v = content.Contains v

    member x.UnionWith(s : seq<'a>) =
        let res = s |> Seq.filter content.Add |> Seq.map Add |> Seq.toList
        for r in readers do 
            if r.IsIncremental then r.Emit res
            else r.Reset content

    member x.ExceptWith(s : seq<'a>) =
        let res = s |> Seq.filter content.Remove |> Seq.map Rem |> Seq.toList
        for r in readers do 
            if r.IsIncremental then r.Emit res
            else r.Reset content

    member x.IntersectWith(s : seq<'a>) =
        let s = HashSet(s)
        let res = 
            content |> Seq.toList
                    |> List.filter (not << s.Contains) 
                    |> List.map (fun a -> content.Remove a |> ignore; Rem a) 

        for r in readers do 
            if r.IsIncremental then r.Emit res
            else r.Reset content

    member x.SymmetricExceptWith(s : seq<'a>) =
        let s = HashSet(s)
        let removed = 
            content |> Seq.toList
                    |> List.choose (fun a -> 
                            if s.Contains a then 
                                content.Remove a |> ignore
                                s.Remove a |> ignore
                                Some (Rem a)
                            else None
                        ) 

        let added = s |> Seq.filter content.Add |> Seq.map Add |> Seq.toList
        let res = List.append removed added

        for r in readers do 
            if r.IsIncremental then r.Emit res
            else r.Reset content

    member x.Clear() =
        let deltas = content |> Seq.map Rem |> Seq.toList
        for r in readers do 
            if r.IsIncremental then r.Emit deltas
            else r.Reset content

    member x.Add v =
        if content.Contains v then 
            false
        else
            content.Add v |> ignore
            for r in readers do 
                if r.IsIncremental then r.Emit [Add v]
                else r.Reset content

            true

    member x.Remove v =
        if content.Contains v then 
            content.Remove v |> ignore
            for r in readers do
                if r.IsIncremental then r.Emit [Rem v]
                else r.Reset content

            true
        else
            false

    new() = cset Seq.empty

    interface ISet<'a> with
        member x.Add(item: 'a): bool = x.Add item
        member x.Add(item: 'a): unit = x.Add item |> ignore
        member x.Clear(): unit = x.Clear()
        member x.Contains(item: 'a): bool = x.Contains item
        member x.CopyTo(array: 'a [], arrayIndex: int): unit = content.CopyTo(array, arrayIndex)
        member x.Count: int = x.Count
        member x.ExceptWith(other: IEnumerable<'a>): unit = x.ExceptWith other
        member x.GetEnumerator(): IEnumerator = content.GetEnumerator() :> IEnumerator
        member x.GetEnumerator(): IEnumerator<'a> = content.GetEnumerator() :> IEnumerator<'a>
        member x.IntersectWith(other: IEnumerable<'a>): unit = x.IntersectWith other
        member x.IsProperSubsetOf(other: IEnumerable<'a>): bool = content.IsProperSubsetOf other
        member x.IsProperSupersetOf(other: IEnumerable<'a>): bool = content.IsProperSupersetOf other
        member x.IsReadOnly: bool = false
        member x.IsSubsetOf(other: IEnumerable<'a>): bool = content.IsSubsetOf other
        member x.IsSupersetOf(other: IEnumerable<'a>): bool = content.IsSubsetOf other
        member x.Overlaps(other: IEnumerable<'a>): bool = content.Overlaps other
        member x.Remove(item: 'a): bool = x.Remove item
        member x.SetEquals(other: IEnumerable<'a>): bool = content.SetEquals other
        member x.SymmetricExceptWith(other: IEnumerable<'a>): unit = x.SymmetricExceptWith other
        member x.UnionWith(other: IEnumerable<'a>): unit = x.UnionWith other


module CSet =
    let empty<'a> = cset<'a>()

    let add (a : 'a) (set : cset<'a>) = set.Add a
    let remove (a : 'a) (set : cset<'a>) = set.Remove a
