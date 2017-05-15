namespace Aardvark.Base

open System
open System.Threading
open System.Collections
open System.Collections.Generic


[<StructuredFormatDisplay("{AsString}")>]
type plist<'a>(l : Index, h : Index, content : MapExt<Index, 'a>) =
    
    static let empty = plist<'a>(Index.zero, Index.zero, MapExt.empty)

    static let trace =
        {
            tops = PDeltaList.monoid
            tempty = empty
            tapply = fun a b -> a.Apply(b)
            tcompute = fun l r -> plist.ComputeDeltas(l,r)
            tcollapse = fun _ _ -> false
        }

    static member Empty = empty
    static member Trace = trace

    member x.MinIndex = l
    member x.MaxIndex = h

    member x.Count = content.Count

    member x.Content = content


    member x.Apply(deltas : pdeltalist<'a>) : plist<'a> * pdeltalist<'a> =
        let mutable res = x
        let finalDeltas =
            deltas |> PDeltaList.filter (fun i op ->
                match op with
                    | Remove -> 
                        res <- res.Remove i
                        true
                    | Set v -> 
                        match res.TryGet i with
                            | Some o when Object.Equals(o,v) -> 
                                false
                            | _ -> 
                                res <- res.Set(i,v)
                                true
            )

        res, finalDeltas

    static member ComputeDeltas(l : plist<'a>, r : plist<'a>) : pdeltalist<'a> =
        match l.Count, r.Count with
            | 0, 0 -> 
                PDeltaList.empty

            | 0, _ -> 
                r.Content |> MapExt.map (fun i v -> Set v) |> PDeltaList.ofMap

            | _, 0 ->
                l.Content |> MapExt.map (fun i v -> Remove) |> PDeltaList.ofMap

            | _, _ ->
                let merge (k : Index) (l : Option<'a>) (r : Option<'a>) =
                    match l, r with
                        | Some l, Some r when Unchecked.equals l r -> 
                            None
                        | _, Some r -> 
                            Some (Set r)
                        | Some l, None -> 
                            Some Remove
                        | None, None ->
                            None

                MapExt.choose2 merge l.Content r.Content |> PDeltaList.ofMap

    member x.TryGet (i : Index) =
        MapExt.tryFind i content
        
    member x.TryGet (i : int) =
        match MapExt.tryItem i content with
            | Some (_,v) -> Some v
            | None -> None

    member x.Item
        with get(i : Index) = MapExt.find i content
        
    // O(log(n))
    member x.Item
        with get(i : int) = MapExt.item i content |> snd

    member x.Append(v : 'a) =
        if content.Count = 0 then
            let t = Index.after Index.zero
            plist(t, t, MapExt.ofList [t, v])
        else
            let t = Index.after h
            plist(l, t, MapExt.add t v content)
        
    member x.Prepend(v : 'a) =
        if content.Count = 0 then
            let t = Index.after Index.zero
            plist(t, t, MapExt.ofList [t, v])
        else
            let t = Index.before l
            plist(t, h, MapExt.add t v content)

    member x.Set(key : Index, value : 'a) =
        if content.Count = 0 then
            plist(key, key, MapExt.ofList [key, value])

        elif key < l then
            plist(key, h, MapExt.add key value content)

        elif key > h then
            plist(l, key, MapExt.add key value content)

        else 
            plist(l, h, MapExt.add key value content)

    member x.Set(i : int, value : 'a) =
        match MapExt.tryItem i content with
            | Some (id,_) -> x.Set(id, value)
            | None -> x

    member x.Update(i : int, f : 'a -> 'a) =
        match MapExt.tryItem i content with
            | Some (id,v) -> 
                let newContent = MapExt.add id (f v) content
                plist(l, h, newContent)
            | None -> 
                x

    member x.InsertAt(i : int, value : 'a) =
        if i < 0 || i > content.Count then
            x
        else
            let l, s, r = MapExt.neighboursAt i content

            let r = 
                match s with
                    | Some s -> Some s
                    | None -> r

            let index = 
                match l, r with
                    | Some (before,_), Some (after,_) -> Index.between before after
                    | None,            Some (after,_) -> Index.before after
                    | Some (before,_), None           -> Index.after before
                    | None,            None           -> Index.after Index.zero
            x.Set(index, value)

    member x.InsertBefore(i : Index, value : 'a) =
        let str = Guid.NewGuid() |> string
        let l, s, r = MapExt.neighbours i content
        match s with
            | None ->
                x.Set(i, value)
            | Some _ ->
                let index = 
                    match l with
                        | Some (before,_) -> Index.between before i
                        | None -> Index.before i
                x.Set(index, value)

    member x.InsertAfter(i : Index, value : 'a) =
        let str = Guid.NewGuid() |> string
        let l, s, r = MapExt.neighbours i content
        match s with
            | None ->
                x.Set(i, value)
            | Some _ ->
                let index =
                    match r with
                        | Some (after,_) -> Index.between i after
                        | None -> Index.after i
                x.Set(index, value)

    member x.TryGetIndex(i : int) =
        match MapExt.tryItem i content with
            | Some (id,_) -> Some id
            | None -> None

    member x.Remove(key : Index) =
        let c = MapExt.remove key content
        if c.Count = 0 then empty
        elif l = key then plist(MapExt.min c, h, c)
        elif h = key then plist(l, MapExt.max c, c)
        else plist(l, h, c)

    member x.RemoveAt(i : int) =
        match MapExt.tryItem i content with
            | Some (id, _) -> x.Remove id
            | _ -> x

    member x.Map(mapping : Index -> 'a -> 'b) =
        plist(l, h, MapExt.map mapping content)
        
    member x.Choose(mapping : Index -> 'a -> Option<'b>) =
        let res = MapExt.choose mapping content
        if res.IsEmpty then 
            plist<'b>.Empty
        else
            plist(MapExt.min res, MapExt.max res, res)

    member x.Filter(predicate : Index -> 'a -> bool) =
        let res = MapExt.filter predicate content
        if res.IsEmpty then 
            plist<'a>.Empty
        else
            plist(MapExt.min res, MapExt.max res, res)
          
    member x.AsSeq =
        content |> MapExt.toSeq |> Seq.map snd

    member x.AsList =
        content |> MapExt.toList |> List.map snd

    member x.AsArray =
        content |> MapExt.toArray |> Array.map snd

    override x.ToString() =
        content |> MapExt.toSeq |> Seq.map (snd >> sprintf "%A") |> String.concat "; " |> sprintf "plist [%s]"

    member private x.AsString = x.ToString()
    
    member x.CopyTo(arr : 'a[], i : int) = 
        let mutable i = i
        content |> MapExt.iter (fun k v -> arr.[i] <- v; i <- i + 1)

    member x.IndexOf(item : 'a) =
        x |> Seq.tryFindIndex (Unchecked.equals item) |> Option.defaultValue -1

    interface ICollection<'a> with 
        member x.Add(v) = raise (NotSupportedException("plist cannot be mutated"))
        member x.Clear() = raise (NotSupportedException("plist cannot be mutated"))
        member x.Remove(v) = raise (NotSupportedException("plist cannot be mutated"))
        member x.Contains(v) = content |> MapExt.exists (fun _ vi -> Unchecked.equals vi v)
        member x.CopyTo(arr,i) = x.CopyTo(arr, i)
        member x.IsReadOnly = true
        member x.Count = x.Count

    interface IList<'a> with
        member x.RemoveAt(i) = raise (NotSupportedException("plist cannot be mutated"))
        member x.IndexOf(item : 'a) = x.IndexOf item
        member x.Item
            with get(i : int) = x.[i]
            and set (i : int) (v : 'a) = raise (NotSupportedException("plist cannot be mutated"))
        member x.Insert(i,v) = raise (NotSupportedException("plist cannot be mutated"))

    interface IEnumerable with
        member x.GetEnumerator() = new PListEnumerator<'a>((content :> seq<_>).GetEnumerator()) :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = new PListEnumerator<'a>((content :> seq<_>).GetEnumerator()) :> _

and private PListEnumerator<'a>(r : IEnumerator<KeyValuePair<Index, 'a>>) =
    
    member x.Current =
        r.Current.Value

    interface IEnumerator with
        member x.MoveNext() = r.MoveNext()
        member x.Current = x.Current :> obj
        member x.Reset() = r.Reset()

    interface IEnumerator<'a> with
        member x.Current = x.Current
        member x.Dispose() = r.Dispose()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PList =
    let empty<'a> = plist<'a>.Empty

    let inline count (list : plist<'a>) = list.Count
    let inline append (v : 'a) (list : plist<'a>) = list.Append v
    let inline prepend (v : 'a) (list : plist<'a>) = list.Prepend v
    let inline set (id : Index) (v : 'a) (list : plist<'a>) = list.Set(id, v)
    let inline setAt (index : int) (v : 'a) (list : plist<'a>) = list.Set(index, v)
    let inline remove (id : Index) (list : plist<'a>) = list.Remove(id)
    let inline removeAt (index : int) (list : plist<'a>) = list.RemoveAt(index)
    let inline insertAt (index : int) (value : 'a) (list : plist<'a>) = list.InsertAt(index, value)
    let inline toSeq (list : plist<'a>) = list :> seq<_>
    let inline toList (list : plist<'a>) = list.AsList
    let inline toArray (list : plist<'a>) = list.AsArray

    let inline insertAfter (index : Index) (value : 'a) (list : plist<'a>) = list.InsertAfter(index, value)
    let inline insertBefore (index : Index) (value : 'a) (list : plist<'a>) = list.InsertBefore(index, value)
    let inline tryAt (index : int) (list : plist<'a>) = list.TryGet index
    let inline tryGet (index : Index) (list : plist<'a>) = list.TryGet index

    let single (v : 'a) =
        let t = Index.after Index.zero
        plist(t, t, MapExt.ofList [t, v])

    let ofSeq (seq : seq<'a>) =
        let mutable res = empty
        for e in seq do res <- append e res
        res

    let ofList (list : list<'a>) =
        ofSeq list

    let ofArray (arr : 'a[]) =
        ofSeq arr

    let inline mapi (mapping : Index -> 'a -> 'b) (list : plist<'a>) = list.Map mapping
    let inline map (mapping : 'a -> 'b) (list : plist<'a>) = list.Map (fun _ v -> mapping v)

    let inline choosei (mapping : Index -> 'a -> Option<'b>) (list : plist<'a>) = list.Choose mapping
    let inline choose (mapping : 'a -> Option<'b>) (list : plist<'a>) = list.Choose (fun _ v -> mapping v)
    
    let inline filteri (predicate : Index -> 'a -> bool) (list : plist<'a>) = list.Filter predicate
    let inline filter (predicate : 'a -> bool) (list : plist<'a>) = list.Filter (fun _ v -> predicate v)


    let inline computeDelta (l : plist<'a>) (r : plist<'a>) = plist.ComputeDeltas(l, r)
    let inline applyDelta (l : plist<'a>) (d : pdeltalist<'a>) = l.Apply(d)

    let trace<'a> = plist<'a>.Trace

    module Lens =
        let item (i : int) : Lens<plist<'a>, 'a> =
            { new Lens<_, _>() with
                member x.Get s = 
                    s.[i]

                member x.Set(s,r) =
                    s.Set(i, r)

                member x.Update(s,f) =
                    s.Update(i, f)
            }  