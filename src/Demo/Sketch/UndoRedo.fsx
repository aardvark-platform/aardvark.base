#I @"..\..\..\bin\Release"
#r "Aardvark.Base.dll"
#r "Aardvark.Base.Essentials.dll"
#r "Aardvark.Base.TypeProviders.dll"
#r "Aardvark.Base.FSharp.dll"
#r "Aardvark.Base.Incremental.dll"
#r "Aardvark.Base.Incremental.Tests.exe"
#r "System.Reactive.Core.dll"
#r "System.Reactive.Interfaces.dll"
#r "System.Reactive.Linq.dll"

(*
Goal: implement tree like structure

a) provide operations for modifying the tree.
b) provide some traversal which extracts leaf nodes from the tree.
c) leaf node extraction should be incremental
d) provide functional snapshots
e) undelimited undo/redo while maintainging efficient updates.
*)

open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Incremental.Tests

let test a = if a then printfn "OK" else failwith "assertion failed." a 

// Let us start with a binary tree
type PTree<'a> = PNode of left : PTree<'a> * right : PTree<'a>
               | PLeaf of 'a

// (a) done.

// lets define a traversal which extracts leaf nodes from the tree
let rec leafNodesP t =
    seq {
        match t with 
         | PNode(l,r) -> 
            yield! leafNodesP l
            yield! leafNodesP r
         | PLeaf v ->
            yield v
    }

let testTree1 = PNode( PNode(PLeaf 1, PLeaf 2), PNode (PLeaf 3, PLeaf 4))
test ((testTree1 |> leafNodesP |> Seq.toList) = [1;2;3;4])
                         
// (b) done.

// Let us convert the tree to a adaptive one
type Tree<'a> = Node of left : IMod<Tree<'a>> * right : IMod<Tree<'a>> 
              | Leaf of IMod<'a>

// Let us define the adaptive version of leafNodes
let rec leafNodes t =
    aset {
        match t with 
         | Node(l,r) -> 
            let! l = l
            let! r = r
            yield! leafNodes l
            yield! leafNodes r
         | Leaf v ->
            yield v
    } 

// there are many mods around. let us make convinience functions
// for constructing mod trees from values more easily
let leaf v = Leaf (Mod.init v)
let node l r = Node (Mod.init l, Mod.init r)

let l1 = node (leaf 1) (leaf 2) |> Mod.init
let l2Original = node (leaf 3) (leaf 4) // just store the l2 for later usage
let l2 = l2Original |> Mod.init
let testTree2 = Node (l1, l2)

// since we have asets, we have unordered outpus, so we need a utility for comparing
// our results with expected results while respecting set semantics.
let setToSet aset = aset |> ASet.toList |> List.map Mod.force |> Set.ofList 
let setEqual xs aset = (setToSet aset) = (Set.ofList xs)

let resultingLeafNodes = testTree2 |> leafNodes

test (resultingLeafNodes|> setEqual [1;2;3;4])

// let verify that mod works as expected. Let us modify the tree by replacing the right subtrree with a leaf 10
transact (fun () -> Mod.change l2 (leaf 10)) 

test (resultingLeafNodes |> setEqual [1;2;10])

// (c) done. (incremental extraction of modifiable trees)

// Okay, we have incremental updates to trees. But what if we want functional snapshots?
// let us adaptively convert our mod tree to a persistent one!

let rec buildPeristent t =
    adaptive {
        match t with 
         | Node(l,r) -> 
            let! l = l
            let! r = r
            let! lTree = buildPeristent l
            let! rTree = buildPeristent r
            return PNode(lTree, rTree)
         | Leaf v ->
            let! v = v
            return PLeaf v
    } 

let testTree2InPersistent = testTree2 |> buildPeristent
test ((Mod.force testTree2InPersistent) = PNode(PNode(PLeaf(1),PLeaf(2)), PLeaf 10))

// let us modify the mod tree and see how the persistent one looks like
transact (fun () -> Mod.change l2 l2Original)

test ((Mod.force testTree2InPersistent) = PNode(PNode(PLeaf 1, PLeaf 2), PNode(PLeaf 3, PLeaf 4)))

// (d) done. we have functional snapshots (we will see this is not sufficient for (e))

// by the way, our modification also updated our leaf list
test (testTree2 |> leafNodes |> setEqual [1;2;3;4])

// since i want to use the same variable names as before i use a fresh module.... here
// (just for presentation purpose)
module UndoRedo =
    // lets go move on to undo redo
    // in order to implement undo redo generically, we need a way to tell some undo/redo
    // scope what mods should be tracked (e.g. camera moves shall not be tracked globally)
    // please note that undo redo here was just a 40 line sketch by gh. But as you will see,
    // this solution is so beautiful, concise, efficient and awesome.

    // let us now create a tree just as before but we use special mod ctors 

    let scope = UndoRedoScope()

    // let us now create the tree just like before but by using the undo redo scope
    let leaf v = Leaf (scope.initMod v)
    let node l r = Node (scope.initMod l, scope.initMod r)

    let l1 = node (leaf 1) (leaf 2) |> scope.initMod
    let l2Original = node (leaf 3) (leaf 4) 
    let l2 = l2Original |> scope.initMod
    let testTree2 = Node (l1, l2)

    let adaptiveLeafNodes = testTree2 |> leafNodes

    // again, let us synthesize the leafs
    test (adaptiveLeafNodes |> setEqual [1;2;3;4])

    // Lets say, this tree should be saved as snapshot - project1
    let project1 = scope.Snapshot()

    // let us do some modification
    transact (fun () -> Mod.change l2 (leaf 10))

    test (adaptiveLeafNodes |> setEqual [1;2;10])

    let project2 = scope.Snapshot()

    // let us do some modification
    transact (fun () -> Mod.change l1 (leaf 11))

    test (adaptiveLeafNodes |> setEqual [11;10])

    let project3 = scope.Snapshot()

    // let us go back to project 1
    transact (fun () -> project1.Restore())
    test (adaptiveLeafNodes |> setEqual [1;2;3;4])

    // again to project 2
    transact (fun () -> project2.Restore())
    test (adaptiveLeafNodes |> setEqual [1;2;10])

    transact (fun () -> project3.Restore())
    printfn "%A" (adaptiveLeafNodes |> ASet.toList |> List.map Mod.force)
    test (adaptiveLeafNodes |> setEqual [11;10])
