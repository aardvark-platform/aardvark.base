namespace Aardvark.Base.FSharp.Tests

open System
open System.Collections
open System.IO
open System.Reflection
open System.Runtime.Versioning
open Microsoft.FSharp.Reflection
open Aardvark.Base
open NUnit.Framework

module UndirectedGraphTests =

    type private GraphApi(target : string) =
        let path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.FSharp.dll"))
        do Assert.That(File.Exists path, Is.True, "Required actual FSharp target: " + path)
        let assembly = Assembly.LoadFile path
        do
            Assert.That(assembly.Location, Is.EqualTo path)
            Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                Is.EqualTo(if target = "net10.0" then ".NETCoreApp,Version=v10.0" else ".NETStandard,Version=v2.0"))
        let moduleType = assembly.GetType("Aardvark.Base.UndirectedGraphModule", true)
        let graphType = assembly.GetType("Aardvark.Base.UndirectedGraph`1", true).MakeGenericType [|typeof<int>|]
        let treeType = assembly.GetType("Aardvark.Base.Tree`1", true).MakeGenericType [|typeof<int>|]
        let method name =
            let m = moduleType.GetMethod(name).MakeGenericMethod [|typeof<int>|]
            Assert.That(m.DeclaringType.Assembly, Is.SameAs assembly, target + "/" + name)
            m
        let ofNodes, ofEdges = method "ofNodes", method "ofEdges"
        let weighted = [|method "minimumSpanningTree"; method "maximumSpanningTree"|]
        do
            Assert.That(ofNodes.ReturnType, Is.EqualTo graphType, target)
            Assert.That(ofEdges.ReturnType, Is.EqualTo graphType, target)
            for m in weighted do
                Assert.That(m.ReturnType, Is.EqualTo treeType, target)
                Assert.That(m.GetParameters().[1].ParameterType, Is.EqualTo graphType, target)
        let rec readTree (value : obj) =
            Assert.That(value.GetType().Assembly, Is.SameAs assembly, target + " tree owner")
            let case, fields = FSharpValue.GetUnionFields(value, treeType)
            match case.Name with
            | "Empty" -> Tree.Empty
            | "Node" ->
                let children =
                    fields.[1] :?> IEnumerable |> Seq.cast<obj> |> Seq.map (fun pair ->
                        let values = FSharpValue.GetTupleFields pair
                        unbox<int> values.[0], readTree values.[1]) |> Seq.toList
                Tree.Node(unbox<int> fields.[0], children)
            | other -> failwithf "Unexpected tree case: %s" other
        member _.Name = target
        member _.OfNodes(nodes : Set<int>, edge : int -> int -> int option) =
            ofNodes.Invoke(null, [|box nodes; box edge|])
        member _.OfEdges(edges : (int * int * int) list) =
            ofEdges.Invoke(null, [|box (edges :> seq<_>)|])
        member _.Weighted(mode, cmp : int -> int -> int, graph) =
            weighted.[mode].Invoke(null, [|box cmp; graph|]) |> readTree

    let private implementations () =
        [|GraphApi "net10.0"; GraphApi "netstandard2.0"|]

    [<Test>]
    let WeightedSingletonsRetainTheirVertexWithoutChildren () =
        for api in implementations () do
            for id in [Int32.MinValue; Int32.MinValue + 1; -65537; -1; 0; 1; 65537; Int32.MaxValue - 1; Int32.MaxValue] do
                let graphs = [
                    "nodes", api.OfNodes(Set.singleton id, fun _ _ -> failwith "Singleton edge callback")
                    "loop", api.OfEdges [id, id, -7]
                    "loops", api.OfEdges [id, id, Int32.MinValue; id, id, 0; id, id, Int32.MaxValue]
                ]
                for construction, graph in graphs do
                    for mode in 0 .. 1 do
                        let context = sprintf "%s: vertex=%d construction=%s maximum=%b" api.Name id construction (mode = 1)
                        let actual = api.Weighted(mode, (fun _ _ -> failwith "Singleton comparator"), graph)
                        Assert.That(actual, Is.EqualTo(Tree.Node(id, []) : Tree<int>), context)
                        Assert.That(Tree.count actual, Is.EqualTo 1, context)

    [<Test>]
    let EmptyAndDisconnectedGraphsKeepTheirExistingContracts () =
        for api in implementations () do
            let empty = [api.OfNodes(Set.empty, fun _ _ -> failwith "Empty edge callback"); api.OfEdges []]
            let isolated = [
                api.OfNodes(Set.ofList [-3; 7], fun _ _ -> None)
                api.OfEdges [-3, -3, 1; 7, 7, 2]
            ]
            for graph in empty @ isolated do
                for mode in 0 .. 1 do
                    Assert.That(api.Weighted(mode, (fun _ _ -> failwith "Edgeless comparator"), graph), Is.EqualTo(Tree.Empty : Tree<int>), api.Name)
            // The edge-only multi-node pipeline still discards isolated vertices.
            let graph = api.OfNodes(Set.ofList [-5; 2; 99], fun l r -> if l = -5 && r = 2 then Some 17 else None)
            for mode in 0 .. 1 do
                Assert.That(api.Weighted(mode, compare, graph), Is.EqualTo(Tree.Node(-5, [17, Tree.Node(2, [])])), api.Name + " disconnected isolated vertex")

    // Enumerate edge subsets and test connectivity, independently of Kruskal/sorting/union-find.
    let private extrema (nodes : Set<int>) (edges : (int * int * int) array) =
        let weights = [
            for mask in 0 .. (1 <<< edges.Length) - 1 do
                let chosen = edges |> Array.mapi (fun i e -> i,e) |> Array.choose (fun (i,e) -> if mask &&& (1 <<< i) <> 0 then Some e else None)
                if chosen.Length = nodes.Count - 1 then
                    let mutable reached = Set.singleton (Set.minElement nodes)
                    let mutable changed = true
                    while changed do
                        let before = reached
                        for l,r,_ in chosen do
                            if reached.Contains l then reached <- Set.add r reached
                            if reached.Contains r then reached <- Set.add l reached
                        changed <- before <> reached
                    if reached = nodes then yield chosen |> Array.sumBy (fun (_,_,w) -> int64 w)
        ]
        Assert.That(weights, Is.Not.Empty, "Independent spanning-tree oracle")
        List.min weights, List.max weights

    let private treeEdges tree =
        let rec collect = function
            | Tree.Empty -> []
            | Tree.Node(root, children) -> children |> List.collect (fun (w, child) ->
                match child with
                | Tree.Empty -> failwith "Empty child in spanning tree"
                | Tree.Node(id, _) -> (min root id, max root id, w) :: collect child)
        collect tree

    [<Test>]
    let ConnectedWeightedTreesMatchIndependentExtremaAndStableTies () =
        let complete (ids : int array) (weights : int array) =
            [|for i in 0 .. Array.length ids - 1 do for j in i + 1 .. Array.length ids - 1 do yield ids.[i], ids.[j]|]
            |> Array.mapi (fun i (l,r) -> l,r,weights.[i])
        let cases = [
            [|-4,-1,-9; -1,2,-3; 2,10,4; 10,100,17|]
            complete [|-3; 0; 7|] [|-2; 0; 7|]
            complete [|Int32.MinValue; -1; 0; Int32.MaxValue|] [|-9; -5; -1; 0; 2; 7|]
            complete [|Int32.MinValue; -1; 0; Int32.MaxValue|] [|Int32.MinValue; -17; -1; 0; 31; Int32.MaxValue|]
            complete [|-11; -3; 0; 9; 101|] [|-11 .. -2|]
        ]
        for api in implementations () do
            for edges in cases do
                let nodes = edges |> Seq.collect (fun (l,r,_) -> [l;r]) |> Set.ofSeq
                let minimum, maximum = extrema nodes edges
                let graph = api.OfEdges(Array.toList edges)
                for mode in 0 .. 1 do
                    for reversed in [false;true] do
                        let mutable comparisons = 0
                        let cmp l r = comparisons <- comparisons + 1; if reversed then compare r l else compare l r
                        let actual = api.Weighted(mode, cmp, graph)
                        let context = sprintf "%s: edges=%A maximum=%b reversed=%b" api.Name edges (mode = 1) reversed
                        let selected = treeEdges actual
                        let actualNodes = Tree.foldNodes (fun s id -> Set.add id s) Set.empty actual
                        Assert.That(actualNodes, Is.EqualTo nodes, context + " vertices")
                        Assert.That(Tree.count actual, Is.EqualTo nodes.Count, context + " unique vertices")
                        Assert.That(selected.Length, Is.EqualTo(nodes.Count - 1), context + " edges")
                        for edge in selected do Assert.That(edges, Does.Contain edge, context + " selected edge")
                        let total = selected |> List.sumBy (fun (_,_,w) -> int64 w)
                        Assert.That(total, Is.EqualTo(if (mode = 1) <> reversed then maximum else minimum), context + " independent weight")
                        Assert.That(comparisons, Is.GreaterThan 0, context + " comparator")
            let ids = [|-3; 0; 7; Int32.MaxValue|]
            let graph = api.OfEdges(complete ids [|4;4;4;4;4;4|] |> Array.toList)
            let expected = Tree.Node(-3, [4, Tree.Node(0, []); 4, Tree.Node(7, []); 4, Tree.Node(Int32.MaxValue, [])])
            for mode in 0 .. 1 do
                Assert.That(api.Weighted(mode, compare, graph), Is.EqualTo expected, api.Name + " stable equal-weight selection")
