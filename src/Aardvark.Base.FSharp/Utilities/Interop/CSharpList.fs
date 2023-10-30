namespace Aardvark.Base

open System.Collections.Generic

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CSharpList =


    let inline add (v : 'a) (l : List<'a>) = l.Add v
    let inline addRange (elements : seq<'a>) (l : List<'a>) = l.AddRange elements
    let inline removeAt (index : int) (l : List<'a>) = l.RemoveAt index
    let inline clear (l : List<'a>) = l.Clear()
    let inline insert (index : int) (value : 'a) (l : List<'a>) = l.Insert(index, value)

    let inline map (f : 'a -> 'b) (l : List<'a>) = l.Map f

    let collect (f : 'a -> seq<'b>) (l : List<'a>) =
        let res = List()
        for e in l do
            res.AddRange (f e)
        res

    let choose (f : 'a -> Option<'b>) (l : List<'a>) =
        let res = List()
        for e in l do
            match f e with
                | Some v -> res.Add v
                | None -> ()

        res

    let filter (f : 'a -> bool) (l : List<'a>) =
        let res = List()
        for e in l do
            if f e then res.Add e

        res

    let append (l : List<'a>) (r : List<'a>) =
        let res = List(l)
        res.AddRange r
        res
        
    let concat (l : seq<List<'a>>) =
        let res = List()
        for e in l do
            res.AddRange e
        res


    let inline count (l : List<'a>) = l.Count

    let iter (f : 'a -> unit) (l : List<'a>) =
        for i in 0..l.Count-1 do
            f l.[i]

    let iteri (f : int -> 'a -> unit) (l : List<'a>) =
        for i in 0..l.Count-1 do
            f i l.[i]


    let empty<'a> : List<'a> = List()

    let ofSeq (elements : seq<'a>) : List<'a> =
        List elements

    let ofList (elements : list<'a>) : List<'a> =
        List elements

    let ofArray (elements : 'a[]) : List<'a> =
        List elements

    let toList (l : List<'a>) : list<'a> =
        let mutable res = []
        for i in 1..l.Count do
            let i = l.Count - i
            res <- l.[i]::res
        res

    let toArray (l : List<'a>) : 'a[] =
        l.ToArray()

    let toSeq (l : List<'a>) : seq<'a> =
        l :> _
