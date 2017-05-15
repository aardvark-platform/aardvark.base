namespace Aardvark.Base.Incremental.Tests
//
//open System
//open System.Threading
//open System.Threading.Tasks
//open System.Collections
//open System.Collections.Generic
//open Aardvark.Base
//open Aardvark.Base.Ag
//open NUnit.Framework
//open FsUnit
//
//module AgTests =
//    
//    type ITreeNode = interface end
//    type TreeNode(name : string) =
//        let mutable children = List.empty<TreeNode>
//        interface ITreeNode
//        member x.Children 
//            with get() = children
//            and set value =
//                children <- value
//        member x.Name = name
//        member x.Garbage = Array.zeroCreate(1 <<< 10)
//
//    type SynTreeNode(n : TreeNode, s : Ag.Scope) =
//        member x.Node = n
//        member x.Scope = s
//        
//    [<Semantic>]
//    type AgTestSemantics() =
//       
//        //let fakeChild = obj()
//
//        member x.SynTree(node : TreeNode) : SynTreeNode[] =
//            let sn = SynTreeNode(node, Ag.getContext())
//            let mutable temp = sn.IntoArray()
//            for x in node.Children do
//                let sub : SynTreeNode[] = x?SynTree()
//                temp <- Array.append temp sub
//            temp
//
//        member x.FlattenTree(node : TreeNode) : TreeNode[] =
//            let mutable temp = [ node ]
//            for x in node.Children do
//                let sub : TreeNode[] = x?FlattenTree()
//                temp <- List.append temp (List.ofArray sub)
//            temp |> List.toArray
//
//        member x.TraverseTree(node : TreeNode) : obj =
//            for x in node.Children do
//                let sub : obj = x?TraverseTree()
//                ()
//            obj()
//
//        member x.ConcatNameList(node : TreeNode) : list<string> =
////            let fakeChildScope = Ag.getContext().GetChildScope(node)
////            let mutable temp = [ fakeChildScope?ConcatName ]
//            let mutable temp = [ node.AllChildren?ConcatName ]
//            for x in node.Children do
//                let sub : list<string> = x?ConcatNameList()
//                temp <- List.append temp sub
//            temp
//
//        member x.DoAnyObjectStuff(node : TreeNode) : string =
//            let fakeChildScope = Ag.getContext().GetChildScope(node)
//            let temp : string = fakeChildScope?ConcatName
//            //let temp : string = node.AllChildren?ConcatName
//            printfn "concatName: %s" temp
//            for c in node.Children do
//                let foo : string = c?DoAnyObjectStuff()
//                ()
//            temp
//
//        member x.ConcatName(r : Root<ITreeNode>) =
//            x.AllChildren?ConcatName <- "0"
//
//        member x.ConcatName(node : TreeNode) : unit =
//            x.AllChildren?ConcatName <- node?ConcatName + "|" + node.Name
//
//        member x.SynName(r : Root<ITreeNode>) : string =
//            "0"
//
//        member x.SynName(n : SynTreeNode) : string =
//            n.Node.Name
//
//        member x.ScopeName(n : TreeNode) : string =
//            n.Name
//
////        member x.ConcatName(node : TreeNode) : string =
////            node?ConcatNameInh + "|" + node.Name
//
//    let generateNewStuff (root : TreeNode, nodeRegistry : HashSet<WeakReference<TreeNode>>, gen : int) =
//        let someNode = TreeNode ("node" + gen.ToString())
//        let someOther = TreeNode ("other" + gen.ToString())
//        let child = TreeNode ("child" + gen.ToString())
//        let subchild = TreeNode ("subchild" + gen.ToString())
//
//        nodeRegistry.Add (WeakReference<TreeNode>(someNode)) |> ignore
//        nodeRegistry.Add (WeakReference<TreeNode>(someOther)) |> ignore
//        nodeRegistry.Add (WeakReference<TreeNode>(child)) |> ignore
//        nodeRegistry.Add (WeakReference<TreeNode>(subchild)) |> ignore
//
//        child.Children <- [ subchild ]
//
//        someOther.Children <- [ child ]
//
//        root.Children <- [ someNode; someOther ]
//
//    [<Test>]
//    let ``[Ag] AnyObjectTest``() =
//
//        Ag.initialize()
//
//        let mutable root = TreeNode "root"
//        let nodeRegistry = new HashSet<WeakReference<TreeNode>>()
//
//        let rec printTree (n : TreeNode ) =
//            printfn "%s" n.Name
//            for c in n.Children do
//                printTree c
//
//        let mutable root = TreeNode "root"
//        for i in 1..10 do
//        //let mutable i = 0
//        //while true do
//
//            root <- TreeNode "root"
//
//            //i <- i + 1
//            generateNewStuff(root, nodeRegistry, i)
//
//            //let foo = root?DoAnyObjectStuff()
//            let st : SynTreeNode[] = root?SynTree()
//            
//            GC.Collect()
////            Thread.Sleep(1000)
////
////            GC.AddMemoryPressure(1L <<< 20);
////            GC.Collect()
////            Thread.Sleep(1000)
//
//            for x in st do
//                //printfn "%s" (Ag.useScope (x.Scope.GetChildScope x.Node) (fun () -> x?ConcatName))
//                //printfn "%s" (x?SynName())
//                printfn "%s" (x.Scope?ScopeName())
//
//            let aliveCount = nodeRegistry |> (Seq.filter (fun w ->  match w.TryGetTarget() with
//                                                                    | (true, v) -> true
//                                                                    | _ -> false)) |> Seq.toArray |> Array.length
//
//            //printfn "Tree:"
//            //printTree root
//
//            //let flatTree : list<TreeNode> = root?FlattenTree()
//            //let nameList : list<string> = root?ConcatNameList()
//
//            // do ag queries
////            printfn "flatNodeCount=%d" flatTree.Length
//            printfn "generated=%d alive=%d" nodeRegistry.Count aliveCount
//
//            ()
//
//        ()
//        
//    open Aardvark.Base.Incremental
//    type IL = interface end
//    type LNode(children : aset<IL>) =
//        interface IL
//        member x.Children = children
//    type LLeaf(v : int) =
//        interface IL
//        member x.V = v
//
//    type Leak(cnt : ref<int>) =
//        do cnt := !cnt + 1
//        override x.Finalize() = cnt := !cnt - 1
//
//        
//    let leakCnt = ref 0
//
//    type LS = Leak * Ag.Scope * IMod<int>* int
//
//    [<Semantic>]
//    type LSemantics() =
//        
//        member x.Flatten(n : LNode) : aset<Leak>=
//            aset {
//                for i in n.Children do
//                    yield! i?Flatten()
//            }
//
//        member x.Flatten(l : LLeaf) : aset<Leak> =
//            aset {
//                yield Leak leakCnt
//            }
//
//        member x.Flatten2(n : LNode) : aset<LS>=
//            aset {
//                for i in n.Children do
//                    let a : IMod<int> = n?SomeInhValue
//                    let! ab = a
//                    printfn "AB:%A" ab
//                    yield! i?Flatten2()
//            }
//
//        member x.Flatten2(l : LLeaf) : aset<LS> =
//            aset {
//                let vm : IMod<int> = l?SomeInhValue
//                let! v = vm
//                yield Leak leakCnt, Ag.getContext(), l?SomeInhValue, v
//            }
//
//        member x.SomeInhValue(r : Root<IL>) = r.Child?SomeInhValue <- Mod.init 1
//
//    [<Test>]
//    let ``[Ag] Leaky leaky test``() =
//
//        Ag.initialize()
//
//        let xs = CSet.ofList [ for i in 0 .. 10 do yield LLeaf i :> IL ]
//        let root = LNode (xs :> aset<_>)
//
//        let leaks : aset<Leak> = root?Flatten()
//        let r = leaks.GetReader()
//        let run () =
//            r.GetDelta() |> ignore
//        
//            !leakCnt |> should equal 11 
//
//            transact (fun () -> xs |> CSet.clear)
//
//            GC.Collect()
//            GC.WaitForPendingFinalizers()
//        
//            r.GetDelta() |> printfn "delta1: %A"
//
//        run ()
//
//        GC.Collect()
//        GC.WaitForPendingFinalizers()
//
//        !leakCnt |> should equal 0 
//
//        r.GetDelta() |> printfn "delta2=%A"
//        printfn "c=%A" r.Content
//        ()     
//
//
//    [<Test>]
//    let ``[Ag] Leaky leaky test 2``() =
//
//        Ag.initialize()
//
//        let xs = CSet.ofList [ for i in 0 .. 10 do yield LLeaf i :> IL ]
//        let root = LNode (xs :> aset<_>)
//
//        let leaks : aset<LS> = root?Flatten2()
//        let r = leaks.GetReader()
//        let run () =
//            r.GetDelta() |> ignore
//        
//            !leakCnt |> should equal 11 
//
//            transact (fun () -> xs |> CSet.clear)
//
//            GC.Collect()
//            GC.WaitForPendingFinalizers()
//        
//            r.GetDelta() |> printfn "delta1: %A"
//
//        run ()
//
//        GC.Collect()
//        GC.WaitForPendingFinalizers()
//
//        !leakCnt |> should equal 0 
//
//        r.GetDelta() |> printfn "delta2=%A"
//        printfn "c=%A" r.Content
//        ()    
//        
////    [<EntryPoint>]
////    let main args =
////        AnyObjectTest()
////        0