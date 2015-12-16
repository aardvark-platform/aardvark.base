namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base.Incremental

module TreeFlattenPerformance =

    open System.Diagnostics
    // let us create a function for measuring performance
    let timed f = 
        let sw = Stopwatch()
        sw.Start()
        let r = f ()
        sw.Stop()
        r,sw.Elapsed.TotalSeconds

        // Let us convert the tree to an adaptive one
    type Tree<'a> = Node of left : IMod<Tree<'a>> * right : IMod<Tree<'a>> 
                  | Leaf of IMod<'a>

    // Let us define the adaptive version of leafNodes
    let rec leafNodes t =
        aset {
            match t with 
             | Node(l,r) ->
                let s = ASet.ofList [l;r] |> ASet.flattenM
                for e in s do
                    yield! leafNodes e // then recurse and use yield! 
                                   // since the rec call returns an aset
             | Leaf v ->
                yield v
        } 

    let rec leafNodesComb t =
        match t with
         | Node(a,b) ->
            let a = a |> ASet.bind leafNodesComb
            let b = b |> ASet.bind leafNodesComb
            ASet.unionTwo a b
         | Leaf v -> ASet.single v

    open System.Collections.Generic
    open System.IO

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

        // test with 15, profile with 18
        let (buildBigTree0,refs),elapsedBuild = timed (fun () -> buildBigTree 15)

        let r,s = timed (fun () -> buildBigTree0 |> extractLeafs |> Seq.toArray)
        printfn "rebuild took: %As" s
    

        let r = buildBigTree0 |> leafNodesComb

        let reader = r |> ASet.toMod 

        let list,elapsedFold = timed (fun () -> reader.GetValue() )
        //let l,elapsedPull = timed (fun () -> list |> Seq.toList |> List.map Mod.force)


        printfn "build took,%As\nfold took: %As" elapsedBuild elapsedFold 

        let last = refs |> List.rev |> List.head
        let _,reexElapsed = timed (fun () -> transact (fun () -> Mod.change last (leaf 10)); reader.GetValue() )
        printfn "rexecution: %As" reexElapsed

        let measurement = sprintf "%A;%f;%f;%f" System.DateTime.Now elapsedBuild elapsedFold reexElapsed
        let perfName = Path.Combine(__SOURCE_DIRECTORY__, "ASetFoldTest.csv")
        if File.Exists perfName then File.AppendAllLines(perfName,[|measurement|])
        else File.WriteAllLines(perfName,[|"Date;ElapsedBuild;elapsedFold;reexElapsed";measurement|])
            