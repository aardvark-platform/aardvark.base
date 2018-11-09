namespace Aardvark.Base

open System.Collections
open System.Collections.Generic

[<StructuredFormatDisplay("{AsString}")>]
[<Struct; StructuralEquality; NoComparison>]
type pdeltalist< [<EqualityConditionalOn>] 'a> internal(content : MapExt<Index, ElementOperation<'a>>) =

    static let monoid : Monoid<pdeltalist<'a>> =
        {
            misEmpty = fun l -> l.IsEmpty
            mempty = pdeltalist<'a>(MapExt.empty)
            mappend = fun l r -> l.Combine r
        }

    static member Monoid = monoid

    static member Empty = pdeltalist<'a>(MapExt.empty)

    member private x.Content = content

    member x.Count = content.Count

    member x.IsEmpty = content.IsEmpty


    member x.Add(i : Index, op : ElementOperation<'a>) =
        pdeltalist(MapExt.add i op content)

    member x.Remove(i : Index) =
        pdeltalist(MapExt.remove i content)

    member x.ToSeq() = content |> MapExt.toSeq
    member x.ToList() = content |> MapExt.toList
    member x.ToArray() = content |> MapExt.toArray

    member x.Combine(r : pdeltalist<'a>) =
        if x.IsEmpty then r
        elif r.IsEmpty then x
        else MapExt.unionWith (fun l r -> r) x.Content r.Content |> pdeltalist

    member x.Map(f : Index -> ElementOperation<'a> -> ElementOperation<'b>) =
        pdeltalist(MapExt.map f content)
        
    member x.Choose(f : Index -> ElementOperation<'a> -> Option<ElementOperation<'b>>) =
        pdeltalist(MapExt.choose f content)

    member x.MapMonotonic(f : Index -> ElementOperation<'a> -> Index * ElementOperation<'b>) =
        pdeltalist(MapExt.mapMonotonic f content)

    member x.Filter(f : Index -> ElementOperation<'a> -> bool) =
        pdeltalist(MapExt.filter f content)
        
    override x.ToString() =
        let suffix =
            if content.Count > 4 then "; ..."
            else ""
        
        let content =
            content |> Seq.truncate 4 |> Seq.map (fun (KeyValue(i,op)) ->
                match op with
                    | Set v -> sprintf "set(%A,%A)" i v
                    | Remove -> sprintf "rem(%A)" i
            ) |> String.concat "; "

        "pdeltalist [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    member x.Collect (f : Index -> ElementOperation<'a> -> pdeltalist<'b>) =
        let mutable res = pdeltalist<'b>.Empty
        for (KeyValue(i,v)) in content do
            res <- res.Combine(f i v)

        res

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PDeltaList =
    let empty<'a> = pdeltalist<'a>.Empty

    let inline isEmpty (l : pdeltalist<'a>) = l.IsEmpty

    let inline add (i : Index) (v : ElementOperation<'a>) (l : pdeltalist<'a>) = l.Add(i, v)
    let inline remove (i : Index) (l : pdeltalist<'a>) = l.Remove(i)

    let ofMap (m : MapExt<Index, ElementOperation<'a>>) = pdeltalist(m)
    
    let single (i : Index) (op : ElementOperation<'a>) = pdeltalist(MapExt.singleton i op)
    let ofSeq (s : seq<Index * ElementOperation<'a>>) = pdeltalist(MapExt.ofSeq s)
    let ofList (s : list<Index * ElementOperation<'a>>) = pdeltalist(MapExt.ofList s)
    let ofArray (s : array<Index * ElementOperation<'a>>) = pdeltalist(MapExt.ofArray s)

    let inline toSeq (l : pdeltalist<'a>) = l.ToSeq()
    let inline toList (l : pdeltalist<'a>) = l.ToList()
    let inline toArray (l : pdeltalist<'a>) = l.ToArray()

    
    let inline mapMonotonic (mapping : Index -> ElementOperation<'a> -> Index * ElementOperation<'b>) (l : pdeltalist<'a>) = 
        l.MapMonotonic mapping

    let inline map (mapping : Index -> ElementOperation<'a> -> ElementOperation<'b>) (l : pdeltalist<'a>) = 
        l.Map mapping
        
    let inline choose (mapping : Index -> ElementOperation<'a> -> Option<ElementOperation<'b>>) (l : pdeltalist<'a>) = 
        l.Choose mapping

    let inline combine (l : pdeltalist<'a>) (r : pdeltalist<'a>) =
        l.Combine(r)

    let inline collect (mapping : Index -> ElementOperation<'a> -> pdeltalist<'b>) (l : pdeltalist<'a>) = 
        l.Collect mapping
        
    let inline filter (predicate : Index -> ElementOperation<'a> -> bool) (l : pdeltalist<'a>) =
        l.Filter predicate

    type private MonoidInstance<'a>() =
        static let instance : Monoid<pdeltalist<'a>> =
            {
                misEmpty = isEmpty
                mempty = empty
                mappend = combine
            }
        static member Instance = instance


    let monoid<'a> = MonoidInstance<'a>.Instance