namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HashSet =
    open System.Collections.Generic
    open System.Linq

    let empty<'k> = HashSet<'k>()

    let inline add (k : 'a) (h : HashSet<_>) =
        h.Add k
        
    let inline remove a (h : HashSet<_>) =
        h.Remove a

    let inline clear (h : HashSet<_>) =
        h.Clear()

    let inline map ([<InlineIfLambda>] f) (h : HashSet<_>) =
        let r = HashSet()
        for v in h do r.Add (f v) |> ignore
        r

    let inline union (dicts : #seq<HashSet<'a>>) =
        let result = HashSet()
        for d in dicts do
            result.UnionWith d
        result

    let inline unionWith (xs : seq<'a>) (s : HashSet<'a>) =
        s.UnionWith xs

    let inline exceptWith (xs : seq<'a>) (s : HashSet<'a>) =
        s.ExceptWith xs

    let inline intersectWith (xs : seq<'a>) (s : HashSet<'a>) =
        s.IntersectWith xs

    let inline contains v (h : HashSet<_>) =
        h.Contains v

    let inline ofSeq (elements : seq<'a>) =
        let result = HashSet()
        for v in elements do
            result.Add v |> ignore
        result

    let inline ofList (elements : list<'a>) =
        ofSeq elements

    let inline ofArray (elements : ('a)[]) =
        ofSeq elements

    let inline toSeq (d : HashSet<'a>) =
        d :> seq<_>

    let inline toList (d : HashSet<'a>) =
        let mutable l = []
        for x in d do
            l <- x :: l
        l

    let inline toArray (d : HashSet<'a>) =
       d.ToArray()

    let inline toSet (d : HashSet<'a>) =
        Set.ofSeq d