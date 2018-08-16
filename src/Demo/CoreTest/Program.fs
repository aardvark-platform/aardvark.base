// Learn more about F# at http://fsharp.org

open System
open Aardvark.Base

let svdtest() =
    let szx = 9
    let szy = 5

    let rand = Random()
    let vals = 
        [|
            for x in 0..szx-1 do
                yield [| for y in 0..szy-1 do yield (rand.NextDouble() - 0.5) * 0.1 |]
        |]

    let M = Array2D.init szx szy (fun i j -> vals.[i].[j])
    let U = Array2D.zeroCreate szx szx
    let Vt = Array2D.zeroCreate szy szy

    if SVD.decomposeInPlace U M Vt then
        Log.line "original: \n%A\n\n" vals
        Log.line "eigenvalues:\n"
        for i in 0..(min szx szy)-1 do 
            Log.line "(%d) %f" i M.[i,i]
    else
        Log.error "bad"

    
    
    
    Log.line "\ndone"

    Console.ReadLine() |> ignore




[<EntryPoint>]
let main argv =
    Aardvark.Init()

    svdtest()
    //let pi = PixImage.Create @"image.jpg"
    //pi.SaveAsImage @"sepp.png"
    0 
