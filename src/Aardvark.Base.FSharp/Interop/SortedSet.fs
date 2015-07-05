namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Linq

module SortedSet =
    
    let inline private optionalToOpt (o : Optional<'a>) =
        if o.HasValue then Some o.Value
        else None


    let empty<'a> = SortedSetExt<'a>()

    let cusom (cmp : 'a -> 'a -> int) =
        SortedSetExt<'a>({ new IComparer<'a> with member x.Compare(l,r) = cmp l r })

    let inline count (s : SortedSetExt<'a>) =
        s.Count

    let inline add (value : 'a) (s : SortedSetExt<'a>) =
        s.Add(value)

    let inline remove (value : 'a) (s : SortedSetExt<'a>) =
        s.Remove(value)

    let clear (s : SortedSetExt<'a>) =
        s.Clear()

    let inline contains (value : 'a) (s : SortedSetExt<'a>) =
        s.Contains(value)

    let inline unionWith (values : seq<'a>) (s : SortedSetExt<'a>) =
        s.UnionWith values

    let inline exceptWith (values : seq<'a>) (s : SortedSetExt<'a>) =
        s.ExceptWith values

    let inline intersectWith (values : seq<'a>) (s : SortedSetExt<'a>) =
        s.IntersectWith values

    let inline symmetricExceptWith (values : seq<'a>) (s : SortedSetExt<'a>) =
        s.SymmetricExceptWith values

    let neighbourhood (value : 'a) (s : SortedSetExt<'a>) =
        let mutable lower = Optional<'a>.None
        let mutable upper = Optional<'a>.None
        let mutable self = Optional<'a>.None
        s.FindNeighbours(value, &lower, &self, &upper)

        (optionalToOpt lower, optionalToOpt self, optionalToOpt upper)

    let addWithNeighbours (value : 'a) (f : Option<'a> -> Option<'a> -> 'a) (s : SortedSetExt<'a>) =
        let (p,self,n) = neighbourhood value s
        match self with
            | Some v -> s.Remove(v) |> ignore
            | None -> ()

        let res = f p n
        s.Add res |> ignore

type SortedDictionaryExt<'k, 'v>(cmp : 'k -> 'k -> int) =
    let set = SortedSetExt<'k * ref<'v>>({ new IComparer<'k * ref<'v>> with member x.Compare((l,_),(r,_)) = cmp l r })

    member internal x.Set = set

    member x.TryGetValue (key : 'k, [<Out>] result : byref<'v>) =
        match set |> SortedSet.neighbourhood (key, Unchecked.defaultof<_>) with
            | _,Some (_,v),_ -> 
                result <- !v
                true
            | _ ->
                false

    member x.Count = set.Count

    member x.Add (key : 'k, value : 'v) =
        if not (set |> SortedSet.add (key, ref value)) then
            raise <| ArgumentException("An item with the same key has already been added.")

    member x.Remove (key : 'k) =
        set.Remove (key, Unchecked.defaultof<_>)

    member x.Clear() =
        set.Clear()

    member x.ContainsKey (key : 'k) =
        set.Contains ((key, Unchecked.defaultof<_>))

    member x.Item
        with get (key : 'k) =
             match set |> SortedSet.neighbourhood (key, Unchecked.defaultof<_>) with
                | _,Some (_,v),_ -> !v
                | _ -> raise <| KeyNotFoundException()

        and set (key : 'k) (value : 'v) =
            match set |> SortedSet.neighbourhood (key, Unchecked.defaultof<_>) with
                | _,Some (_,v),_ ->
                    v := value
                | _ ->
                    set |> SortedSet.add (key, ref value) |> ignore

    interface IEnumerable with
        member x.GetEnumerator() = (set |> Seq.map (fun (k,v) -> KeyValuePair(k,!v))).GetEnumerator() :> IEnumerator

    interface IEnumerable<KeyValuePair<'k, 'v>> with
        member x.GetEnumerator() = (set |> Seq.map (fun (k,v) -> KeyValuePair(k,!v))).GetEnumerator() :> IEnumerator<_>

module SortedDictionary =
  
    let inline private optionalToOpt (o : Optional<'k * ref<'v>>) =
        if o.HasValue then
            let (k,v) = o.Value
            Some(k, v)
        else 
            None

    let inline private optionalToOptValue (o : Optional<'k * ref<'v>>) =
        if o.HasValue then
            let (k,v) = o.Value
            Some(k, !v)
        else 
            None

    let inline private neighbourhoodInt (key : 'k) (s : SortedDictionaryExt<'k, 'v>) =
        let mutable lower = Optional<'k * ref<'v>>.None
        let mutable upper = Optional<'k * ref<'v>>.None
        let mutable self = Optional<'k * ref<'v>>.None
        s.Set.FindNeighbours((key, Unchecked.defaultof<_>), &lower, &self, &upper)

        (optionalToOptValue lower, optionalToOpt self |> Option.map snd, optionalToOptValue upper)


    let empty<'k, 'v when 'k : comparison> = SortedDictionaryExt<'k, 'v>(compare)

    let custom (cmp : 'k -> 'k -> int) = SortedDictionaryExt<'k, 'v>(cmp)

    let inline count (s : SortedDictionaryExt<'k, 'v>) =
        s.Count

    let inline add (key : 'k) (value : 'v) (s : SortedDictionaryExt<'k, 'v>) =
        s.Add(key, value)

    let inline remove (key : 'k) (s : SortedDictionaryExt<'k, 'v>) =
        s.Remove(key)

    let inline tryFind (key : 'k) (s : SortedDictionaryExt<'k, 'v>) =
        match s.TryGetValue(key) with
            | (true, v) -> Some v
            | _ -> None

    let clear (s : SortedDictionaryExt<'k, 'v>) =
        s.Clear()

    let neighbourhood (key : 'k) (s : SortedDictionaryExt<'k, 'v>) =
        let (p,s,n) = neighbourhoodInt key s

        match s with
            | Some s -> (p,Some !s, n)
            | None -> (p,None,n)

    let setWithNeighbours (key : 'k) (f : Option<'k * 'v> -> Option<'v> -> Option<'k * 'v> -> 'v) (s : SortedDictionaryExt<'k, 'v>) =
        let (p,self,n) = neighbourhoodInt key s
        match self with
            | Some s -> 
                let res = f p (Some !s) n
                s := res
                res
            | None -> 
                let res = f p None n
                s.Add(key, res)
                res
