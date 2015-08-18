namespace Aardvark.Base.Incremental.Tests

open System
open System.Threading
open System.Threading.Tasks
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Ag
open Aardvark.Base.AgHelpers
open NUnit.Framework
open FsUnit

module AgTests =
    
    type TreeNode(name : string) =
        let mutable children = List.empty<TreeNode>
        member x.Children 
            with get() = children
            and set value =
                children <- value
        member x.Name = name
        member x.Garbage = Array.zeroCreate(1 <<< 10)
        
    [<Semantic>]
    type AgTestSemantics() =
       
        //let fakeChild = obj()



        member x.FlattenTree(node : TreeNode) : TreeNode[] =
            let mutable temp = [ node ]
            for x in node.Children do
                let sub : TreeNode[] = x?FlattenTree()
                temp <- List.append temp (List.ofArray sub)
            temp |> List.toArray

        member x.TraverseTree(node : TreeNode) : obj =
            for x in node.Children do
                let sub : obj = x?TraverseTree()
                ()
            obj()

        member x.ConcatNameList(node : TreeNode) : list<string> =
//            let fakeChildScope = Ag.getContext().GetChildScope(node)
//            let mutable temp = [ fakeChildScope?ConcatName ]
            let mutable temp = [ node.AllChildren?ConcatName ]
            for x in node.Children do
                let sub : list<string> = x?ConcatNameList()
                temp <- List.append temp sub
            temp

        member x.DoAnyObjectStuff(node : TreeNode) : string =
            //let fakeChildScope = Ag.getContext().GetChildScope(node)
            let temp : string = node.AllChildren?ConcatName
            //let temp : string = node?ConcatName
            printfn "concatName: %s" temp
            for c in node.Children do
                let foo : string = c?DoAnyObjectStuff()
                ()
            temp

        member x.ConcatName(r : Root) =
            x.AllChildren?ConcatName <- "0"

        member x.ConcatName(node : TreeNode) : unit =
            x.AllChildren?ConcatName <- node?ConcatName + "|" + node.Name

//        member x.ConcatName(node : TreeNode) : string =
//            node?ConcatNameInh + "|" + node.Name

    let generateNewStuff (root : TreeNode, nodeRegistry : HashSet<WeakReference<TreeNode>>, gen : int) =
        let someNode = TreeNode ("node" + gen.ToString())
        let someOther = TreeNode ("other" + gen.ToString())
        let child = TreeNode ("child" + gen.ToString())

        nodeRegistry.Add (WeakReference<TreeNode>(someNode)) |> ignore
        nodeRegistry.Add (WeakReference<TreeNode>(someOther)) |> ignore
        nodeRegistry.Add (WeakReference<TreeNode>(child)) |> ignore

        someOther.Children <- [ child ]

        root.Children <- [ someNode; someOther ]

    [<Test>]
    let AnyObjectTest() =

        Ag.initialize()

        let mutable root = TreeNode "root"
        let nodeRegistry = new HashSet<WeakReference<TreeNode>>()

        let rec printTree (n : TreeNode ) =
            printfn "%s" n.Name
            for c in n.Children do
                printTree c

        let mutable root = TreeNode "root"
        for i in 1..10 do
        //let mutable i = 0
        //while true do

            root <- TreeNode "root"

            //i <- i + 1
            generateNewStuff(root, nodeRegistry, i)

            let foo = root?DoAnyObjectStuff()
            //let foo = root?FlattenTree()
            
            GC.Collect()
            Thread.Sleep(1)
//
//            GC.AddMemoryPressure(1L <<< 20);
//            GC.Collect()
//            Thread.Sleep(1000)

            let aliveCount = nodeRegistry |> (Seq.filter (fun w ->  match w.TryGetTarget() with
                                                                    | (true, v) -> true
                                                                    | _ -> false)) |> Seq.toArray |> Array.length

            printfn "Tree:"
            printTree root

            //let flatTree : list<TreeNode> = root?FlattenTree()
            //let nameList : list<string> = root?ConcatNameList()

            // do ag queries
//            printfn "flatNodeCount=%d" flatTree.Length
            printfn "generated=%d alive=%d" nodeRegistry.Count aliveCount

            //let xxx = Console.ReadLine()

            ()

        ()
        
        
//    [<EntryPoint>]
//    let main args =
//        AnyObjectTest()
//        0