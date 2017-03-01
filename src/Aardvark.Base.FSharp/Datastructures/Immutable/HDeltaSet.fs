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
 
[<Struct>]
[<StructuredFormatDisplay("{AsString}")>]
type hdeltaset<'a>(store : hmap<'a, int>) =

    static let monoid =
        {
            mempty = hdeltaset<'a>(HMap.empty)
            mappend = fun l r -> l.Combine r
            misEmpty = fun s -> s.IsEmpty
        }

    static member Empty = hdeltaset<'a>(HMap.empty)
    
    static member Monoid = monoid

    member private x.Store = store

    member x.Count = store.Count

    member x.IsEmpty = store.IsEmpty

    member x.Add (op : SetDelta<'a>) =
        if op.Count <> 0 then
            store |> HMap.alter op.Value (fun o ->
                let n = defaultArg o 0 + op.Count
                if n = 0 then None
                else Some n
            )
            |> hdeltaset
        else
            x

    member x.Remove (op : SetDelta<'a>) =
        x.Add op.Inverse

    member x.Combine (other : hdeltaset<'a>) =
        if store.IsEmpty then other
        elif other.IsEmpty then x
        else
            HMap.choose2 (fun k l r ->
                let newCount = 
                    match l, r with
                        | Some l, Some r -> l + r
                        | Some l, None -> l
                        | None, Some r -> r
                        | None, None -> 0
                if newCount = 0 then None
                else Some newCount
            ) store other.Store
            |> hdeltaset

    member x.Map (f : SetDelta<'a> -> SetDelta<'b>) =
        let mutable res = hdeltaset<'b>.Empty
        for (k,v) in store do
            res <- res.Add (f (SetDelta(k,v)))
        res

    member x.Choose (f : SetDelta<'a> -> Option<SetDelta<'b>>) =
        let mutable res = hdeltaset<'b>.Empty
        for (k,v) in store do
            match f (SetDelta(k,v)) with
                | Some r -> res <- res.Add r
                | _ -> ()
        res

    member x.Filter (f : SetDelta<'a> -> bool) =
        store |> HMap.filter (fun k v -> SetDelta(k,v) |> f) |> hdeltaset

    member x.Collect (f : SetDelta<'a> -> hdeltaset<'b>) =
        let mutable res = hdeltaset<'b>.Empty
        for (k,v) in store.ToSeq() do
            res <- res.Combine (f (SetDelta(k,v)))
        res


    member x.Iter (f : SetDelta<'a> -> unit) =
        store |> HMap.iter (fun k v ->
            f (SetDelta(k,v))
        )

    member x.Fold (seed : 's, f : 's -> SetDelta<'a> -> 's) =
        store |> HMap.fold (fun s k v ->
            f s (SetDelta(k,v))
        ) seed

    member x.Exists (f : SetDelta<'a> -> bool) =
        store |> HMap.exists (fun k v -> f (SetDelta(k,v)))
        
    member x.Forall (f : SetDelta<'a> -> bool) =
        store |> HMap.forall (fun k v -> f (SetDelta(k,v)))


    member x.ToSeq() =
        store |> HMap.toSeq |> Seq.map SetDelta

    member x.ToList() =
        store |> HMap.toList |> List.map SetDelta

    member x.ToArray() =
        x.ToSeq().ToArray(x.Count)

    member x.ToMap() = store

    static member OfSeq (seq : seq<SetDelta<'a>>) =
        let mutable res = hdeltaset<'a>.Empty
        for e in seq do
            res <- res.Add e
        res

    static member OfList (list : list<SetDelta<'a>>) =
        list |> hdeltaset.OfSeq

    static member OfArray (arr : array<SetDelta<'a>>) =
        arr |> hdeltaset.OfSeq
        

    override x.ToString() =
        let suffix =
            if x.Count > 5 then "; ..."
            else ""

        let content =
            x.ToSeq() |> Seq.truncate 5 |> Seq.map (sprintf "%A") |> String.concat "; "

        "hdeltaset [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    interface IEnumerable with
        member x.GetEnumerator() = new HDeltaSetEnumerator<_>(store) :> _

    interface IEnumerable<SetDelta<'a>> with
        member x.GetEnumerator() = new HDeltaSetEnumerator<_>(store) :> _

and private HDeltaSetEnumerator<'a>(store : hmap<'a, int>) =
    let e = (store :> seq<_>).GetEnumerator()

    member x.Current = 
        let (v,c) = e.Current
        SetDelta(v,c)

    interface IEnumerator with
        member x.MoveNext() = e.MoveNext()
        member x.Current = x.Current :> obj
        member x.Reset() = e.Reset()

    interface IEnumerator<SetDelta<'a>> with
        member x.Dispose() = e.Dispose()
        member x.Current = x.Current


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HDeltaSet =
    let inline monoid<'a> = hdeltaset<'a>.Monoid

    let inline empty<'a> = hdeltaset<'a>.Empty

    let inline isEmpty (set : hdeltaset<'a>) = set.IsEmpty
    
    let inline count (set : hdeltaset<'a>) = set.Count

    let inline single (op : SetDelta<'a>) =
        hdeltaset(HMap.single op.Value op.Count)

    let inline ofSeq (seq : seq<SetDelta<'a>>) =
        hdeltaset.OfSeq seq

    let inline ofList (list : list<SetDelta<'a>>) =
        hdeltaset.OfList list

    let inline ofArray (arr : array<SetDelta<'a>>) =
        hdeltaset.OfArray arr
        
    let inline ofHMap (map : hmap<'a, int>) =
        hdeltaset map


    let inline toSeq (set : hdeltaset<'a>) =
        set.ToSeq()

    let inline toList (set : hdeltaset<'a>) =
        set.ToList()
        
    let inline toArray (set : hdeltaset<'a>) =
        set.ToArray()

    let inline toHMap (set : hdeltaset<'a>) =
        set.ToMap()


    let inline add (value : SetDelta<'a>) (set : hdeltaset<'a>) =
        set.Add value

    let inline remove (value : SetDelta<'a>) (set : hdeltaset<'a>) =
        set.Remove value

    let inline combine (l : hdeltaset<'a>) (r : hdeltaset<'a>) =
        l.Combine r


    let inline map (f : SetDelta<'a> -> SetDelta<'b>) (set : hdeltaset<'a>) =
        set.Map f

    let inline choose (f : SetDelta<'a> -> Option<SetDelta<'b>>) (set : hdeltaset<'a>) =
        set.Choose f

    let inline filter (f : SetDelta<'a> -> bool) (set : hdeltaset<'a>) =
        set.Filter f

    let inline collect (f : SetDelta<'a> -> hdeltaset<'b>) (set : hdeltaset<'a>) =
        set.Collect f


    let inline iter (iterator : SetDelta<'a> -> unit) (set : hdeltaset<'a>) =
        set.Iter iterator

    let inline exists (predicate : SetDelta<'a> -> bool) (set : hdeltaset<'a>) =
        set.Exists predicate

    let inline forall (predicate : SetDelta<'a> -> bool) (set : hdeltaset<'a>) =
        set.Forall predicate

    let inline fold (folder : 's -> SetDelta<'a> -> 's) (seed : 's) (set : hdeltaset<'a>) =
        set.Fold(seed, folder)