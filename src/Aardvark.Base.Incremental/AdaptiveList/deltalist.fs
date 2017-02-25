namespace Aardvark.Base

open System.Collections
open System.Collections.Generic

type ListOperation<'a> =
    | Set of 'a
    | Remove
    
[<StructuredFormatDisplay("{AsString}")>]
type deltalist<'a> internal(content : MapExt<Index, ListOperation<'a>>) =
    static let empty = deltalist<'a>(MapExt.empty)

    static member Empty = empty

    member private x.Content = content

    member x.Count = content.Count

    member x.IsEmpty = content.IsEmpty


    member x.Add(i : Index, op : ListOperation<'a>) =
        deltalist(MapExt.add i op content)

    member x.Remove(i : Index) =
        deltalist(MapExt.remove i content)

    member x.ToSeq() = content |> MapExt.toSeq
    member x.ToList() = content |> MapExt.toList
    member x.ToArray() = content |> MapExt.toArray

    static member Combine(l : deltalist<'a>, r : deltalist<'a>) =
        if l.IsEmpty then r
        elif r.IsEmpty then l
        else MapExt.unionWith (fun l r -> r) l.Content r.Content |> deltalist

    member x.Map(f : Index -> ListOperation<'a> -> ListOperation<'b>) =
        deltalist(MapExt.map f content)
        
    member x.Choose(f : Index -> ListOperation<'a> -> Option<ListOperation<'b>>) =
        deltalist(MapExt.choose f content)

    member x.MapMonotonic(f : Index -> ListOperation<'a> -> Index * ListOperation<'b>) =
        deltalist(MapExt.mapMonotonic f content)

    member x.Filter(f : Index -> ListOperation<'a> -> bool) =
        deltalist(MapExt.filter f content)

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

        "deltalist [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    member x.Collect (f : Index -> ListOperation<'a> -> deltalist<'b>) =
        let mutable res = deltalist<'b>.Empty
        for (KeyValue(i,v)) in content do
            res <- deltalist.Combine(res, f i v)

        res

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module DeltaList =
    let empty<'a> = deltalist<'a>.Empty

    let inline isEmpty (l : deltalist<'a>) = l.IsEmpty

    let inline add (i : Index) (v : ListOperation<'a>) (l : deltalist<'a>) = l.Add(i, v)
    let inline remove (i : Index) (l : deltalist<'a>) = l.Remove(i)

    let ofMap (m : MapExt<Index, ListOperation<'a>>) = deltalist(m)
    
    let single (i : Index) (op : ListOperation<'a>) = deltalist(MapExt.singleton i op)
    let ofSeq (s : seq<Index * ListOperation<'a>>) = deltalist(MapExt.ofSeq s)
    let ofList (s : list<Index * ListOperation<'a>>) = deltalist(MapExt.ofList s)
    let ofArray (s : array<Index * ListOperation<'a>>) = deltalist(MapExt.ofArray s)

    let inline toSeq (l : deltalist<'a>) = l.ToSeq()
    let inline toList (l : deltalist<'a>) = l.ToList()
    let inline toArray (l : deltalist<'a>) = l.ToArray()

    
    let inline mapMonotonic (mapping : Index -> ListOperation<'a> -> Index * ListOperation<'b>) (l : deltalist<'a>) = 
        l.MapMonotonic mapping

    let inline map (mapping : Index -> ListOperation<'a> -> ListOperation<'b>) (l : deltalist<'a>) = 
        l.Map mapping
        
    let inline choose (mapping : Index -> ListOperation<'a> -> Option<ListOperation<'b>>) (l : deltalist<'a>) = 
        l.Choose mapping

    let inline combine (l : deltalist<'a>) (r : deltalist<'a>) =
        deltalist.Combine(l, r)

    let inline collect (mapping : Index -> ListOperation<'a> -> deltalist<'b>) (l : deltalist<'a>) = 
        l.Collect mapping
        
    let inline filter (predicate : Index -> ListOperation<'a> -> bool) (l : deltalist<'a>) =
        l.Filter predicate

    type private MonoidInstance<'a>() =
        static let instance : Monoid<deltalist<'a>> =
            {
                misEmpty = isEmpty
                mempty = empty
                mappend = combine
            }
        static member Instance = instance


    let monoid<'a> = MonoidInstance<'a>.Instance