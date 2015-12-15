(*
Challenge:

a) create a binary tree which stores single elements in leafs.
b) provide a traversal function which extracts leaf nodes from the tree (fold).
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

// Let us convert the tree to an adaptive one
type Tree<'a> = Node of left : IMod<Tree<'a>> * right : IMod<Tree<'a>> 
              | Leaf of IMod<'a>

// Let us define the adaptive version of leafNodes
let rec leafNodes t =
    aset {
        match t with 
         | Node(l,r) -> 
            let! (l,r) = l,r // children are mod, read them adaptively
            yield! leafNodes l // then recurse and use yield! 
                               // since the rec call returns an aset
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

// (c) done. (incremental fold for adaptive binary trees)

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

// let us modify the mod tree and see how the persistent representation looks like
transact (fun () -> Mod.change l2 l2Original)

test ((Mod.force testTree2InPersistent) = PNode(PNode(PLeaf 1, PLeaf 2), PNode(PLeaf 3, PLeaf 4)))

// (d) done. we have functional snapshots (we will see this is not sufficient for (e))

// by the way, our modification also updated our leaf list
test (testTree2 |> leafNodes |> setEqual [1;2;3;4])

// (just for presentation purpose i use a separate module which allows me to reuse variable names for top level bindings)
module UndoRedo =
    // lets go on and move on to undo-redo
    // in order to implement undo redo generically, but not globally we introduce
    // a scope (e.g. camera moves shall not be tracked globally).
    // the undo redo scope acts as factory for IModRef values but tracks snapshots for them.
    // please note that the undo-redo s<stem here was just a 40 line sketch by gh. But as you will see,
    // this solution is so beautiful, concise, efficient and awesome.

    let scope = UndoRedoScope()
    // let us now create a tree just as before but we use special mod ctors 

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

    // as you can see, adaptively synthesized leaf nodes automatically update,
    // functional snapshots automatically update and we have time travel.
    // this completes challange (d).

    
open System.Diagnostics
// let us create a function for measuring performance
let timed f = 
    let sw = Stopwatch()
    sw.Start()
    let r = f ()
    sw.Stop()
    printfn "function took: %f seconds" sw.Elapsed.TotalSeconds
    r,sw.Elapsed.TotalSeconds

module Persistency =

    // Let us now add persistency for adaptive undoable tree's.
    // First, let us create a new subclass of serializationBean,
    // register interceptors via dependency injection etc.
    // :P

    // No, let us define picklers for pickling and unpickling non-adaptive 
    // persistent binary trees
    open Nessos.FsPickler
    open Nessos.FsPickler.Combinators
    open Nessos.FsPickler.Json
    open System.Diagnostics
    open System.IO
    open System.Runtime.Serialization.Formatters.Binary

    let p pElement =
        Pickler.fix (fun self ->
            Pickler.sum (fun t node leaf ->
                match t with
                | PNode (l,r) -> node (l, r)
                | PLeaf  v    -> leaf v )
            ^+ Pickler.case PNode (Pickler.pair self self) 
            ^. Pickler.case PLeaf pElement
        )
    let pPTree<'a> = p Pickler.auto<'a>

    let bin = FsPickler.CreateBinarySerializer()
    let json = FsPickler.CreateJsonSerializer()
    let serialized = json.PickleToString(testTree1,pPTree)
    printfn "%s" serialized

    // let us test performance of this simple serialization function
    let mutable root = PLeaf 1
    let depth = 2
    for i in 0 .. depth do
        root <- PNode(root,root)


    printfn "pickling"
    let bytes,time = timed (fun () -> bin.Pickle(root,pPTree))
    printfn "pickling 2"
    let bytes3,time3 = timed (fun () -> bin.Pickle(root,pPTree))
    printfn "running .net"

    let bytes2, time2 = 
        timed (fun () ->
            let formatter = new BinaryFormatter();
            let stream = new MemoryStream()
            formatter.Serialize(stream, obj);
            stream.Close();
            stream.GetBuffer()
    )

    root <- PLeaf 2
    printfn "serialization performed: %d bytes total, %d nodes, in %f seconds (%f mb/s)" bytes.Length (pow 2 depth) time (float (float bytes.Length / float (1024 * 1024)) / float time)
    printfn "serialization performed: %d bytes total, %d nodes, in %f seconds (%f mb/s)" bytes2.Length (pow 2 depth) time (float (float bytes2.Length / float (1024 * 1024)) / float time2)
    printfn "serialization performed: %d bytes total, %d nodes, in %f seconds (%f mb/s)" bytes3.Length (pow 2 depth) time (float (float bytes2.Length / float (1024 * 1024)) / float time3)


module Performance =
    open System.Collections.Generic

    let leaf v = Leaf (Mod.init v)
    let node l r = Node (Mod.init l, Mod.init r)

    let rec buildBigTree current = // 2^current inner nodes - 1, 2^current leafs
        if current = 0 then 
            leaf 1, []
        else
            let l,lrefs = buildBigTree (current - 1) 
            let r,rrefs = buildBigTree (current - 1) 
            let lm = Mod.init l
            let rm = Mod.init r
            Node(lm, rm), lm::rm::(lrefs @ rrefs)

    let rec extractLeafs t =    
        seq {
            match t with
             | Node(l,r) ->
                yield! extractLeafs (Mod.force l) 
                yield! extractLeafs (Mod.force r)
             | Leaf v -> yield Mod.force v
        }

    let test () =


        let (buildBigTree0,refs),elapsedBuild = timed (fun () -> buildBigTree 3)

        let r,s = timed (fun () -> buildBigTree0 |> extractLeafs |> Seq.toArray)
        printfn "rebuild took: %As" s
    

        let r = buildBigTree0 |> leafNodes


        let reader = r |> ASet.toMod 
        reader |> Mod.force |> ignore
        Aardvark.Base.Incremental.Validation.IAdaptiveObjectValidationExtensions.Dump(reader,10000) |> File.writeAllText @"C:\Aardwork\dump.txt"
        Aardvark.Base.Incremental.Validation.IAdaptiveObjectValidationExtensions.DumpDgml(reader,1000,@"C:\Aardwork\Graph.dgml") |> ignore

        let list,elapsedFold = timed (fun () -> reader.GetValue() )
        let l,elapsedPull = timed (fun () -> list |> Seq.toList |> List.map Mod.force)

        let l,elapsedD = 
            timed (fun () -> 
                let x = HashSet(list |> Seq.toList)
                let y = HashSet(list |> Seq.toList)
                let a = x |> Seq.filter (fun s -> y.Contains s |> not) |> Seq.toList
                let b = y |> Seq.filter (fun s -> x.Contains s |> not) |> Seq.toList
                ())

        printfn "differnece took: %As" elapsedD
        printfn "build took: %Ams, fold too: %As" elapsedBuild elapsedFold 
        printfn "force took: %As" elapsedPull

        let last = refs |> List.last 
        let _,reexElapsed = timed (fun () -> transact (fun () -> Mod.change last (leaf 10)); reader.GetValue() )
        printfn "reex: %As" reexElapsed

open Aardvark.Base.Incremental
open System.Collections.Generic

open System.Runtime.InteropServices 
type Dictionary<'a,'b> with
    member x.TryRemove(k,[<Out>] r: byref<'b>) =
        match x.TryGetValue k with
         | (true,v) -> r <- v; true
         | _ -> false

module Delta =
    let map f x =
        match x with 
         | Add v -> Add <| f v
         | Rem v -> Rem <| f v

type Observation<'a>() =
    let content = HashSet()
    member x.Content = content |> Seq.toList
    member x.Continue(d : Delta<'a>) = 
        match d with
            | Add v -> content.Add v |> ignore; fun () -> (content.Remove v |> ignore)
            | Rem v -> content.Remove v |> ignore; id

type Cont<'a> = Delta<'a> -> (Observation<'a> -> unit)

type IUList<'a> = 
    abstract member GetValue : Cont<'a>

type Source<'a when 'a : equality>(output : IUList<'a>) =
    let buffer = Dictionary<'a,unit->unit>()

    member x.Add (v:'a) = 
        buffer.Add v
    member x.Rem v = 
        match content.TryRemove v with
         | (true,d) -> d(); true
         | _ -> false

    member x.GetValue()

type Map<'a,'b>(output : IUList<'b>,f ) =
    interface IUList<'a> with
        member x.Continue d =
            match d with
             | Add v -> output.Continue (Add <| f v)
             | Rem v -> output.Continue (Rem <| f v)



type CUList<'a when 'a : equality>() =
    let sources = List<Source<'a>>()
    member x.Add v =    
        for s in sources do s.Add v |> ignore
    member x.Remove v = 
        for s in sources do s.Rem v |> ignore

let test2 () = 
    let result = Observation<int>()
    let s = Source<int>(Map(result, ((+)1)))
    s.Add 1 |> ignore
    s.Rem 2 |> ignore
    printfn "%A" result.Content

[<EntryPoint>]
let main args = 
    test2 ()
    //Performance.test(); 0
    0