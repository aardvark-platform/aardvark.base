namespace Aardvark.Base

open System
open System.Threading
open System.Collections
open System.Collections.Generic
open MBrace.FsPickler


[<Struct; StructuralEquality; NoComparison>]
[<StructuredFormatDisplay("{AsString}"); CustomPickler>]
type plist< [<EqualityConditionalOn>] 'a>(l : Index, h : Index, content : MapExt<Index, 'a>) =
    
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

    static member private CreatePickler(r : IPicklerResolver) =
        let pint = r.Resolve<int>()
        let pv = r.Resolve<'a>()
        let parr = r.Resolve<'a[]>()

        let read (rs : ReadState) =
            let cnt = pint.Read rs "count"
            let elements = parr.Read rs "elements"
            let mutable res = empty
            for e in elements do res <- res.Append(e)
            res

        let write (ws : WriteState) (s : plist<'a>) =
            pint.Write ws "count" s.Count
            parr.Write ws "elements" (s.AsArray)

        let clone (cs : CloneState) (s : plist<'a>) =
            s.Map(fun _ v -> pv.Clone cs v)

        let accept (vs : VisitState) (s : plist<'a>) =
            s |> Seq.iter (pv.Accept vs)

        Pickler.FromPrimitives(read, write, clone, accept)
        

    member x.MinIndex = l
    member x.MaxIndex = h

    member x.Count = content.Count

    member x.Content = content


    member x.Apply(deltas : pdeltalist<'a>) : plist<'a> * pdeltalist<'a> =
        if deltas.Count = 0 then
            x, deltas
        else
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
                if l.Content == r.Content then
                    PDeltaList.empty
                else
                    // TODO: one small???
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

    member x.Map<'b>(mapping : Index -> 'a -> 'b) : plist<'b> =
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

    // O(n)
    member x.TryFind(item : 'a) : Option<Index> =
        match content |> MapExt.toSeq |> Seq.tryFind (fun (k,v) -> Unchecked.equals v item) with
        | Some (k, v) -> Some k
        | _ -> None

    // O(n)
    member x.Remove(item : 'a) : plist<'a> =
        match x.TryFind(item) with
        | Some index -> x.Remove(index)
        | None -> x
          
    member x.AsSeqBackward =
        content |> MapExt.toSeqBack |> Seq.map snd
        
    member x.AsListBackward =
        x.AsSeqBackward |> Seq.toList

    member x.AsArrayBackward =
        x.AsSeqBackward |> Seq.toArray

    member x.AsSeq =
        content |> MapExt.toSeq |> Seq.map snd

    member x.AsList =
        content |> MapExt.toList |> List.map snd

    member x.AsArray =
        content |> MapExt.toArray |> Array.map snd
        
    member x.AsMap =
        content

    override x.ToString() =
        content |> MapExt.toSeq |> Seq.map (snd >> sprintf "%A") |> String.concat "; " |> sprintf "plist [%s]"

    member private x.AsString = x.ToString()
    
    member x.CopyTo(arr : 'a[], i : int) = 
        let mutable i = i
        content |> MapExt.iter (fun k v -> arr.[i] <- v; i <- i + 1)

    member x.IndexOf(item : 'a) =
        x |> Seq.tryFindIndex (Unchecked.equals item) |> Option.defaultValue -1

    member x.IndexOf(index : Index) =
        MapExt.tryIndexOf index content |> Option.defaultValue -1
        
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

    let inline isEmpty (list : plist<'a>) = list.Count = 0
    let inline count (list : plist<'a>) = list.Count
    let inline append (v : 'a) (list : plist<'a>) = list.Append v
    let inline prepend (v : 'a) (list : plist<'a>) = list.Prepend v

    let inline set (id : Index) (v : 'a) (list : plist<'a>) = list.Set(id, v)
    let inline setAt (index : int) (v : 'a) (list : plist<'a>) = list.Set(index, v)
    let inline remove (id : Index) (list : plist<'a>) = list.Remove(id)
    let inline removeAt (index : int) (list : plist<'a>) = list.RemoveAt(index)
    let inline insertAt (index : int) (value : 'a) (list : plist<'a>) = list.InsertAt(index, value)


    let inline insertAfter (index : Index) (value : 'a) (list : plist<'a>) = list.InsertAfter(index, value)
    let inline insertBefore (index : Index) (value : 'a) (list : plist<'a>) = list.InsertBefore(index, value)
    let inline tryAt (index : int) (list : plist<'a>) = list.TryGet index
    let inline tryGet (index : Index) (list : plist<'a>) = list.TryGet index
    
    let inline tryFirstIndex (list : plist<'a>) = list.Content.TryMinKey
    let inline tryLastIndex (list : plist<'a>) = list.Content.TryMaxKey
    let inline firstIndex (list : plist<'a>) = tryFirstIndex list |> Option.get
    let inline lastIndex (list : plist<'a>) = tryLastIndex list |> Option.get

    let inline tryFirst (list : plist<'a>) = list.Content.TryMinValue
    let inline tryLast (list : plist<'a>) = list.Content.TryMaxValue
    let inline first (list : plist<'a>) = tryFirst list |> Option.get
    let inline last (list : plist<'a>) = tryLast list |> Option.get

    let tryFindIndex (element : 'a) (list : plist<'a>) = list.Content |> MapExt.tryPick (fun k v -> if v = element then Some k else None)
    let tryFindIndexBack (element : 'a) (list : plist<'a>) = list.Content |> MapExt.tryPickBack (fun k v -> if v = element then Some k else None)
    let findIndex (element : 'a) (list : plist<'a>) = tryFindIndex element list |> Option.get
    let findIndexBack (element : 'a) (list : plist<'a>) = tryFindIndexBack element list |> Option.get

    let exists (f : Index -> 'a -> bool) (list : plist<'a>) = list.Content |> Seq.exists (fun kv -> f kv.Key kv.Value)
    let forall (f : Index -> 'a -> bool) (list : plist<'a>) = list.Content |> Seq.forall (fun kv -> f kv.Key kv.Value)
    
    let tryFind (predicate : Index -> 'a -> bool) (list : plist<'a>) = list.Content |> MapExt.tryPick (fun k v -> if predicate k v then Some v else None)
    let tryFindBack (predicate : Index -> 'a -> bool) (list : plist<'a>) = list.Content |> MapExt.tryPickBack (fun k v -> if predicate k v then Some v else None)
    let find (predicate : Index -> 'a -> bool) (list : plist<'a>) = tryFind predicate list |> Option.get
    let findBack (predicate : Index -> 'a -> bool) (list : plist<'a>) = tryFindBack predicate list |> Option.get
    
    let tryPick (predicate : Index -> 'a -> Option<'b>) (list : plist<'a>) = list.Content |> MapExt.tryPick predicate
    let tryPickBack (predicate : Index -> 'a -> Option<'b>) (list : plist<'a>) = list.Content |> MapExt.tryPickBack predicate
    
    let concat2 (l : plist<'a>) (r : plist<'a>) =
        if l.Count = 0 then r
        elif r.Count = 0 then l

        elif l.MaxIndex < r.MinIndex then
            plist<'a>(l.MinIndex, r.MaxIndex, MapExt.union l.Content r.Content)
            
        elif r.MaxIndex < l.MinIndex then
            plist<'a>(r.MinIndex, l.MaxIndex, MapExt.union l.Content r.Content)

        elif l.Count < r.Count then
            let mutable res = r
            for lv in l.AsSeqBackward do
                res <- res.Prepend(lv)
            res
        else
            let mutable res = l
            for rv in r.AsSeq do
                res <- res.Append(rv)
            res

    let concat (s : #seq<plist<'a>>) =
        s |> Seq.fold concat2 empty
        
    let ofMap (m : MapExt<Index, 'a>) =
        if MapExt.isEmpty m then
            empty
        else
            let min = m.TryMinKey
            let max = m.TryMaxKey
            plist<'a>(min.Value, max.Value, m)
        

    let alter (index : Index) (mapping : Option<'a> -> Option<'a>) (l : plist<'a>) =
        MapExt.alter index mapping l.Content |> ofMap

    let alterAt (i : int) (mapping : Option<'a> -> Option<'a>) (list : plist<'a>) =
        if i < -1 || i > list.Count then
            list
        else
            let l, s, r = MapExt.neighboursAt i list.Content
            match s with
            | Some (si, sv) ->
                match mapping (Some sv) with
                | Some r ->
                    plist<'a>(list.MinIndex, list.MaxIndex, MapExt.add si r list.Content)
                | None ->
                    let m = MapExt.remove si list.Content
                    let min = match l with | None -> MapExt.tryMin m |> Option.get | Some _ -> list.MinIndex
                    let max = match r with | None -> MapExt.tryMax m |> Option.get | Some _ -> list.MaxIndex
                    plist<'a>(min, max, m)
            | None ->
                match mapping None with
                | Some res ->
                    let mutable minChanged = false
                    let mutable maxChanged = false
                    let idx =
                        match l, r with
                        | None, None -> 
                            minChanged <- true
                            maxChanged <- true
                            Index.zero
                        | Some (l,_), None -> 
                            maxChanged <- true
                            Index.after l
                        | None, Some (r,_) -> 
                            minChanged <- true
                            Index.before r
                        | Some (l,_), Some (r,_) -> 
                            Index.between l r

                    let min = if minChanged then idx else list.MinIndex
                    let max = if maxChanged then idx else list.MaxIndex
                    plist<'a>(min, max, MapExt.add idx res list.Content)
                | None ->
                    list
      
    let update (index : Index) (mapping : 'a -> 'a) (l : plist<'a>) =
        alter index (Option.map mapping) l
        
    let updateAt (index : int) (mapping : 'a -> 'a) (l : plist<'a>) =
        alterAt index (Option.map mapping) l

    let split (index : Index) (list : plist<'a>) =
        let (l,s,r) = list.Content.Split(index)

        match MapExt.isEmpty l, MapExt.isEmpty r with
        | true, true -> empty, s, empty
        | true, false -> 
            let rmin = r.TryMinKey |> Option.defaultValue Index.zero
            empty, s, plist<'a>(rmin, list.MaxIndex, r)
        | false, true ->
            let lmax = l.TryMaxKey |> Option.defaultValue Index.zero
            plist<'a>(list.MinIndex, lmax, l), s, empty
        | false, false ->
            let lmax = l.TryMaxKey |> Option.defaultValue Index.zero
            let rmin = r.TryMinKey |> Option.defaultValue Index.zero
            plist<'a>(list.MinIndex, lmax, l), s, plist<'a>(rmin, list.MaxIndex, r)
       
    let splitAt (index : int) (list : plist<'a>) =
        if index < 0 then
            empty, None, list
        elif index >= list.Count then
            list, None, empty
        else
            let index,_ = list.Content.TryAt(index) |> Option.get
            split index list
          
    let take (n : int) (list : plist<'a>) =
        if n <= 0 then empty
        elif n > list.Count then list
        else
            let l,_,_ = splitAt n list
            l

    let skip (n : int) (list : plist<'a>) =
        if n <= 0 then list
        elif n > list.Count then empty
        else
            let _,_,r = splitAt (n - 1) list
            r

    let single (v : 'a) =
        let t = Index.after Index.zero
        plist(t, t, MapExt.ofList [t, v])

    let inline toSeq (list : plist<'a>) = list :> seq<_>
    let inline toList (list : plist<'a>) = list.AsList
    let inline toArray (list : plist<'a>) = list.AsArray

    let inline toSeqBack (list : plist<'a>) = list.AsSeqBackward :> seq<_>
    let inline toListBack (list : plist<'a>) = list.AsListBackward
    let inline toArrayBack (list : plist<'a>) = list.AsArrayBackward

    let inline toMap (list : plist<'a>) = list.AsMap
    let ofSeq (seq : seq<'a>) =
        let mutable res = empty
        for e in seq do res <- append e res
        res

    let collecti (mapping : Index -> 'a -> plist<'b>) (l : plist<'a>) = l.Map(mapping) |> concat
    let collect (mapping : 'a -> plist<'b>) (l : plist<'a>) = l.Map(fun _ v -> mapping v) |> concat

    let inline ofList (list : list<'a>) = ofSeq list
    let inline ofArray (arr : 'a[]) = ofSeq arr

    let inline mapi (mapping : Index -> 'a -> 'b) (list : plist<'a>) = list.Map mapping
    let inline map (mapping : 'a -> 'b) (list : plist<'a>) = list.Map (fun _ v -> mapping v)

    let inline choosei (mapping : Index -> 'a -> Option<'b>) (list : plist<'a>) = list.Choose mapping
    let inline choose (mapping : 'a -> Option<'b>) (list : plist<'a>) = list.Choose (fun _ v -> mapping v)
    
    let inline filteri (predicate : Index -> 'a -> bool) (list : plist<'a>) = list.Filter predicate
    let inline filter (predicate : 'a -> bool) (list : plist<'a>) = list.Filter (fun _ v -> predicate v)

    let sortBy (mapping : 'a -> 'b) (l : plist<'a>) =
        let arr = l.AsArray
        Array.sortInPlaceBy mapping arr
        ofArray arr
        
    let sortWith (compare : 'a -> 'a -> int) (l : plist<'a>) =
        let arr = l.AsArray
        Array.sortInPlaceWith compare arr
        ofArray arr

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