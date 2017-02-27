namespace Aardvark.Base

open System

#nowarn "44" // obsolete

[<Obsolete("use hmap instead")>]
[<StructuredFormatDisplay("{AsString}")>]
type HashMap<'k, 'v> = internal HashMap of Map<int, list<'k * 'v>> with
    member x.AsString =
        let (HashMap x) = x
        let l = x |> Map.toList |> List.collect (fun (_,v) -> v)
        sprintf "map %A" l

/// <summary>
/// Extends the F# Library with a hash-based map allowing non-comparable
/// types to be used as keys. It internally creates a map keyed with hashcodes
/// each containing a linked list of key-value-pairs. For "good" hash-codes
/// the runtime of all operations should be similar to the corresponding Map-functions.
/// </summary>
[<Obsolete("use hmap instead")>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HashMap =
    
    // Tools:
    let rec private listWith (k : 'k) (v : 'v) (l : list<'k * 'v>) =
        match l with
            | (ki,vi)::xs ->
                if ki.Equals k then (k,v)::xs
                else (ki,vi)::listWith k v xs
            | [] ->
                [k,v]

    let rec private listWithout (k : 'k) (l : list<'k * 'v>) =
        match l with
            | (ki,vi)::xs ->
                if ki.Equals k then true, xs
                else 
                    let success, rest = listWithout k xs
                    success, (ki,vi)::rest
            | [] ->
                false, []

    let inline private (!) (h : HashMap<'k, 'v>) =
        let (HashMap m) = h
        m

    let inline private hm (m : Map<int, list<'k * 'v>>) =
        HashMap m


    // Operations:

    let add (key : 'k) (value : 'v) (map : HashMap<'k, 'v>) : HashMap<'k, 'v> =
        let hash = key.GetHashCode()
        let map = !map

        match Map.tryFind hash map with
            | Some list ->
                Map.add hash (listWith key value list) map |> hm
            | None ->
                Map.add hash [key, value] map |> hm

    let containsKey (key : 'k) (map : HashMap<'k, 'v>) =
        let hash = key.GetHashCode()
        match Map.tryFind hash !map with
            | Some list ->
                list |> List.exists (fun (k,_) -> k.Equals key)
            | None ->
                false

    let empty<'k, 'v> : HashMap<'k, 'v> = hm Map.empty

    let exists (f : 'k -> 'v -> bool) (map : HashMap<'k, 'v>) =
        !map |> Map.exists (fun _ l ->
            l |> List.exists (fun (k,v) -> f k v)
        )

    let filter (predicate : 'k -> 'v -> bool) (map : HashMap<'k, 'v>) : HashMap<'k, 'v> =
        let newMap =
            !map |> Map.map (fun hash list ->
                let l = list |> List.filter (fun (k,v) -> predicate k v)
                l
            )

        newMap |> Map.filter (fun k l ->
            List.isEmpty l |> not
        ) |> hm

    let find (key : 'k) (map : HashMap<'k, 'v>) : 'v =
        let hash = key.GetHashCode()
        let list = Map.find hash !map
        list |> List.pick (fun (k,v) -> if k.Equals key then Some v else None)

    let isEmpty (m : HashMap<'k, 'v>) =
        Map.isEmpty !m
    
    let map (f : 'k -> 'v -> 'x) (map : HashMap<'k, 'v>) : HashMap<'k, 'x> =
        !map |> Map.map (fun hash list ->
            list |> List.map (fun (k,v) -> k, f k v)
        ) |> hm

    let ofSeq (s : seq<'k * 'v>) : HashMap<'k, 'v> =
        let map = s |> Seq.groupBy (fun (k,v) -> k.GetHashCode()) |> Seq.map (fun (k,values) -> k, values |> Seq.toList)
        Map.ofSeq map |> hm

    let ofList (l : list<'k * 'v>) : HashMap<'k, 'v> =
        ofSeq l

    let ofArray (l : array<'k * 'v>) : HashMap<'k, 'v> =
        ofSeq l

    let toSeq (map : HashMap<'k, 'v>) =
        !map |> Map.toSeq |> Seq.collect (fun (_,l) -> l)

    let toList (map : HashMap<'k, 'v>) =
        !map |> Map.toList |> List.collect (fun (_,l) -> l)

    let toArray (map : HashMap<'k, 'v>) =
        map |> toSeq |> Seq.toArray

    let remove (key : 'k) (map : HashMap<'k, 'v>) : HashMap<'k, 'v> =
        let hash = key.GetHashCode()
        let map = !map

        match Map.tryFind hash map with
            | Some list ->
                let success,newList = listWithout key list
                if success then
                    if List.isEmpty newList then
                        Map.remove hash map |> hm
                    else
                        Map.add hash newList map |> hm
                else
                    map |> hm
            | None ->
                map |> hm

    let tryFind (key : 'k) (map : HashMap<'k, 'v>) : Option<'v> =
        let hash = key.GetHashCode()
        match Map.tryFind hash !map with
            | Some list ->
                list |> List.tryPick (fun (k,v) -> if k.Equals key then Some v else None)
            | None ->
                None