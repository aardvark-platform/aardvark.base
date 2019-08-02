namespace Aardvark.Base.FSharp.Tests
#nowarn "44"

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework

module ``Control tests`` =

    open Aardvark.Base.Cancellable
    open Aardvark.Base.Cancellable.StatefulStepVar
    
    [<Test>]
    [<Ignore("nondeterministic")>]
    let ``[Control] stateful step var cancellation test``() =

        let r = System.Random()
        let perStep = 10

        let test n wait =
            let activations = ref []
            let undos = ref []
            let perform (v:int) = 
                printfn "performed: %d" v
                activations := v :: !activations
            let unperform (v:int) =   
                printfn "activations: %A" activations  
                printfn "undid: %d" v
                undos := v :: !undos

            let things = [ for i in 0 .. n do 
                            let step = 
                                cancel {
                                    try
                                        System.Threading.Thread.Sleep perStep
                                        return i
                                    finally 
                                        perform i, fun i -> unperform i
                                        
                                }
                            yield Step.ofFun (
                                fun i -> step ) 
                         ]

            let all = things |> List.fold (( *>)) (Step.ofFun (fun () -> cancel { return 1 }) )

            let cts = new System.Threading.CancellationTokenSource()
            cts.CancelAfter(r.Next(0,(n*2)*perStep))
            let result = Step.runRef all cts.Token
            System.Threading.Thread.Sleep ((n*2)*perStep+10)
            match !result with
                | Some v ->
                    printfn "compleded, activations: %A" activations
                    !undos |> should equal []
                    (!activations |> List.sort) |> should equal [ 0 .. n ]
                | None -> 
                    printfn "undid: %A, activated: %A" !undos !activations
                    (!activations |> List.sort) |> should equal (!undos |> List.sort)

        test 10 10