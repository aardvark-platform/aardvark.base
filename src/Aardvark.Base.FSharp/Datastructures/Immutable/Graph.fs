namespace Aardvark.Base

type Tree<'a> =
    | Empty
    | Node of int * list<'a * Tree<'a>>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Tree =
    
    let isEmpty (t : Tree<'a>) =
        match t with
            | Empty -> true
            | _ -> false

    let rec map (mapping : 'a -> 'b) (t : Tree<'a>) =
        match t with
            | Empty -> Empty
            | Node(ni, children) -> Node(ni, children |> List.map (fun (e,c) -> mapping e, map mapping c))

    let rec filter (predicate : 'a -> bool) (t : Tree<'a>) =
        match t with
            | Empty -> Empty
            | Node(ni, children) ->
                let newChildren = 
                    children |> List.choose (fun (e,c) -> 
                        if predicate e then 
                            let c = filter predicate c
                            Some (e,c)
                        else
                            None
                    )
                Node(ni, newChildren)

    let rec foldEdges (folder : 's -> 'a -> 's) (seed : 's) (t : Tree<'a>) =
        match t with
            | Empty -> 
                seed

            | Node(_, edges) ->
                edges |> List.fold (fun s (e,c) -> c |> foldEdges folder (folder s e)) seed

    let rec foldNodes (folder : 's -> int -> 's) (seed : 's) (t : Tree<'a>) =
        match t with
            | Empty -> 
                seed

            | Node(id, edges) ->
                match edges with
                    | [] -> 
                        folder seed id
                    | edges ->
                        edges |> List.fold (fun s (e,c) -> c |> foldNodes folder s) (folder seed id)

    let count (t : Tree<'a>) =
        foldNodes (fun c _ -> c + 1) 0 t

    let inline weight (t : Tree<'a>) =
        foldEdges (+) LanguagePrimitives.GenericZero t

type UndirectedGraph<'e> =
    {
        nodes       : Set<int>
        adjacency   : MapExt<int, MapExt<int, 'e>>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module UndirectedGraph =
    open Aardvark.Base.Sorting
    open System.Collections.Generic

    type private UnionNode(id : int) as this =
        let mutable parent = this
        let mutable rank = 1

        member x.Parent
            with get() = parent
            and set p = parent <- p

        member x.Rank
            with get() = rank
            and set r = rank <- r

        member x.Id = id

    type private UnionFind() =
        let nodes = Dict()

        let node (id : int) = nodes.GetOrCreate(id, fun id -> UnionNode(id))

        let rec rep (n : UnionNode) =
            if n.Parent = n then
                n
            else
                let r = rep n.Parent
                n.Parent <- r
                r

        member x.Add(li : int, ri : int) =
            let ln = node li
            let rn = node ri

            let lr = rep ln
            let rr = rep rn

            if lr = rr then
                false
            else
                if lr.Rank < rr.Rank then
                    lr.Parent <- rr
                    true
                elif rr.Rank < lr.Rank then
                    rr.Parent <- lr.Parent
                    true
                else
                    rr.Parent <- lr.Parent
                    lr.Rank <- lr.Rank + 1
                    true


    let ofNodes (nodes : Set<int>) (getEdge : int -> int -> Option<'e>) =
        
        let mutable adjacency = MapExt.empty

        let add (li : int) (ri : int) (e : 'e) =
            adjacency <- MapExt.alter li (function Some o -> MapExt.add ri e o |> Some | None -> MapExt.ofList [ri, e] |> Some ) adjacency
            adjacency <- MapExt.alter ri (function Some o -> MapExt.add li e o |> Some | None -> MapExt.ofList [li, e] |> Some ) adjacency


        let nodeArr = Set.toArray nodes
        for i in 0 .. nodeArr.Length - 1 do
            let li = nodeArr.[i]
            for j in i + 1 .. nodeArr.Length - 1 do
                let ri = nodeArr.[j]
                match getEdge li ri with
                    | Some e -> add li ri e
                    | None -> ()

        {
            nodes = nodes
            adjacency = adjacency
        }

    let ofEdges (edges : seq<int * int * 'e>) =
        let mutable nodes = Set.empty
        let mutable adjacency = MapExt.empty

        let add (li : int) (ri : int) (e : 'e) =
            adjacency <- MapExt.alter li (function Some o -> MapExt.add ri e o |> Some | None -> MapExt.ofList [ri, e] |> Some ) adjacency
            adjacency <- MapExt.alter ri (function Some o -> MapExt.add li e o |> Some | None -> MapExt.ofList [li, e] |> Some ) adjacency


        for (li, ri, e) in edges do
            nodes <- Set.add li (Set.add ri nodes)
            add li ri e

        {
            nodes = nodes
            adjacency = adjacency
        }

    let inline toNodes (g : UndirectedGraph<'e>) =
        g.nodes

    let toEdges (g : UndirectedGraph<'e>) =
        g.adjacency 
            |> MapExt.toSeq 
            |> Seq.collect (fun (li,a) ->
                let _, _, r = a |> MapExt.split li
                r |> MapExt.toSeq |> Seq.map (fun (ri, e) -> (li, ri, e))
            )
            |> Seq.toList

    let spanningTree (g : UndirectedGraph<'e>) =
        if Set.isEmpty g.nodes then
            Empty
        else
            let root = Seq.head g.nodes

            let rec traverse (edgeCount : ref<int>) (visited : HashSet<int>) (n : int) =
                if visited.Add n then
                    let neighbours = MapExt.tryFind n g.adjacency |> Option.defaultValue MapExt.empty 

                    let children = 
                        neighbours 
                            |> MapExt.toList 
                            |> List.choose (fun (ri, e) -> 
                                match traverse edgeCount visited ri with
                                    | Some r -> 
                                        edgeCount := !edgeCount + 1
                                        Some (e, r)
                                    | None -> 
                                        None
                            )

                    Tree.Node(n, children) |> Some
                else
                    None
                
            let cnt = ref 0
            match traverse cnt (HashSet()) root with
                | Some t -> 
                    assert (!cnt = Set.count g.nodes - 1)
                    t
                | None -> 
                    Empty

    let minimumSpanningTree (cmp : 'e -> 'e -> int) (g : UndirectedGraph<'e>) =
        let edges = g |> toEdges |> List.toArray
        edges.TimSort(fun (_,_,l) (_,_,r) -> cmp l r)

        let uf = UnionFind()
        let finalEdges = List<int * int * 'e>()

        for (li, ri, e) in edges do
            if uf.Add(li, ri) then
                finalEdges.Add(li, ri, e)


        finalEdges
            |> ofEdges
            |> spanningTree

    let maximumSpanningTree (cmp : 'e -> 'e -> int) (g : UndirectedGraph<'e>) =
        minimumSpanningTree (fun l r -> cmp r l) g









