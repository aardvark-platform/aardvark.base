namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base

[<Struct>]
type SetDelta<'a>(value : 'a, cnt : int) =
    member x.Value = value
    member x.Count = cnt
    member x.Inverse = SetDelta(value, -cnt)

    override x.ToString() =
        if cnt = 1 then sprintf "Add(%A)" value
        elif cnt = -1 then sprintf "Rem(%A)" value
        elif cnt > 0 then sprintf "Add%d(%A)" cnt value
        elif cnt < 0 then sprintf "Rem%d(%A)" -cnt value
        else "Nop"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetDelta =
    let inline create (cnt : int) (v : 'a) = SetDelta(v, cnt)
    let inline add (v : 'a) = SetDelta(v, 1)   
    let inline rem (v : 'a) = SetDelta(v, -1)     
    let inline inverse (d : SetDelta<'a>) = d.Inverse
    let map (f : 'a -> 'b) (d : SetDelta<'a>) = SetDelta<'b>(f d.Value, d.Count)


[<AutoOpen>]
module SetDeltaExtensions =
    let inline Add(v : 'a) = SetDelta(v, 1)
    let inline Rem(v : 'a) = SetDelta(v, -1)

    type SetDelta<'a> with
        static member inline Add v = SetDelta(v, 1)
        static member inline Rem v = SetDelta(v, -1)

    let inline (|Add|Rem|) (d : SetDelta<'a>) =
        if d.Count > 0 then Add(d.Count, d.Value)
        else Rem(-d.Count, d.Value)
 

[<StructuredFormatDisplay("{AsString}")>]
type deltaset<'a>(content : pmap<'a, int>) =
    static let empty = deltaset<'a>(PMap.empty)
    static member Empty = empty
    

    member private x.content = content

    member inline private x.alter(v : 'a, f : int -> int) =
        content
            |> PMap.alter v (fun o ->
                let o = defaultArg o 0
                let n = f o
                if n = 0 then None
                else Some n
               )
            |> deltaset

    member inline private x.alter2 (other : deltaset<'a>) (f : int -> int -> int) =
        let merge (key : 'a) (l : Option<int>) (r : Option<int>) =
            let l = defaultArg l 0
            let r = defaultArg r 0
            let n = f l r
            if n = 0 then None
            else Some n

        PMap.alter2 merge content other.content |> deltaset

    static member OfSeq (s : seq<SetDelta<'a>>) =
        let mutable res = empty
        for e in s do res <- res.Add e
        res

    static member OfList (s : list<SetDelta<'a>>) =
        let mutable res = empty
        for e in s do res <- res.Add e
        res

    static member OfArray (s : array<SetDelta<'a>>) =
        let mutable res = empty
        for e in s do res <- res.Add e
        res

    member private x.AsString = x.ToString()
    
    override x.ToString() =
        x.AsSeq |> Seq.map string |> String.concat "; " |> sprintf "deltaset [%s]"

    member x.Combine(other : deltaset<'a>) = x.alter2 other (+)
    member x.Add(v : SetDelta<'a>) = x.alter(v.Value, fun o -> o + v.Count)
    member x.Count = content.Count

    member x.AsSeq = content |> PMap.toSeq |> Seq.map SetDelta
    member x.AsList = content |> PMap.toList |> List.map SetDelta
    member x.AsArray = content |> PMap.toArray |> Array.map SetDelta
    
    member x.Map(f : SetDelta<'a> -> SetDelta<'b>) =
        let mutable res = deltaset<'b>.Empty
        for (v,c) in content do
            let r = f (SetDelta(v, c))
            res <- res.Add(r)
        res

    member x.Choose(f : SetDelta<'a> -> Option<SetDelta<'b>>) =
        let mutable res = deltaset<'b>.Empty
        for (v,c) in content do
            match f (SetDelta(v, c)) with
                | Some r ->
                    res <- res.Add(r)
                | None ->
                    ()
        res

    member x.Collect(f : SetDelta<'a> -> deltaset<'b>) =
        let mutable res = deltaset<'b>.Empty
        for (ov,oc) in content do
            let r = f (SetDelta(ov, oc))
            res <- res.Combine r
        res

    member x.Contains a =
        match PMap.tryFind a content with
            | Some _ -> true
            | _ -> false

    interface IEnumerable with
        member x.GetEnumerator() = (x.AsSeq :> IEnumerable).GetEnumerator()

    interface IEnumerable<SetDelta<'a>> with
        member x.GetEnumerator() = x.AsSeq.GetEnumerator()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module DeltaSet =

    let inline empty<'a> = deltaset<'a>.Empty
    let inline single (v : SetDelta<'a>) = deltaset<'a>.OfList [v]

    let inline ofSeq (s : seq<SetDelta<'a>>) = deltaset<'a>.OfSeq s
    let inline ofList (l : list<SetDelta<'a>>) = deltaset<'a>.OfList l
    let inline ofArray (a : array<SetDelta<'a>>) = deltaset<'a>.OfArray a
    let inline toSeq (s : deltaset<'a>) = s.AsSeq
    let inline toList (s : deltaset<'a>) = s.AsList
    let inline toArray (s : deltaset<'a>) = s.AsArray

    let inline combine (l : deltaset<'a>) (r : deltaset<'a>) = l.Combine r

    let inline count (l : deltaset<'a>) = l.Count 
    let inline isEmpty (l : deltaset<'a>) = l.Count = 0
    let inline contains (v : 'a) (s : deltaset<'a>) = s.Contains v

    let inline map (f : SetDelta<'a> -> SetDelta<'b>) (s : deltaset<'a>) = s.Map f
    let inline choose (f : SetDelta<'a> -> Option<SetDelta<'b>>) (s : deltaset<'a>) = s.Choose f
    let inline collect (f : SetDelta<'a> -> deltaset<'b>) (s : deltaset<'a>) = s.Collect f


    type private MonoidImpl<'a>() =
        static let ops : Monoid<deltaset<'a>> =
            {
                misEmpty = isEmpty
                mempty = empty
                mappend = combine
            }

        static member Instance = ops

    let monoid<'a> = MonoidImpl<'a>.Instance
        