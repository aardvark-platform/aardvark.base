namespace Aardvark.Geometry.Tests

open System
open NUnit.Framework
open FsUnit
open FsCheck
open FsCheck.NUnit

open FSharp.Data.Adaptive
open Aardvark.Base
open Aardvark.Geometry

module TexturePackingTests =

    [<Struct>]
    type Size = Size of V2i

    type SizeGenerator() =
        static member SizeGen =
            (Gen.choose (1, 64), Gen.choose (1,64)) ||> Gen.map2 (fun x y -> V2i(x,y) |> Size)

        static member Size() =
            (Gen.choose (1, 64), Gen.choose (1,64)) ||> Gen.map2 (fun x y -> V2i(x,y) |> Size) |> Arb.fromGen


    [<Property(Arbitrary = [| typeof<SizeGenerator> |], Verbose = true)>]
    let ``square producing correct results``(NonEmptyArray (sizes)) =
        let sizes = sizes |> Array.mapi (fun i (Size s) -> i, s)
        let packing = TexturePacking.square sizes

        let mutable res = packing
        for i,_ in sizes do
            res <- res.Remove i


        let bb = Box2i(V2i.Zero, packing.Size - V2i.II)

        // non overlapping and in range
        let rects = packing |> TexturePacking.toArray
        for i in 0 .. rects.Length - 1 do
            let (_, ri) = rects.[i]
            bb.Contains ri |> should be True
            for j in i + 1 .. rects.Length - 1 do
                let (_, rj) = rects.[j]
                ri.Intersects rj |> should be False

        let mutable all = packing.Used

        // matching sizes
        for id, size in sizes do
            match HashMap.tryFind id packing.Used with
            | Some box ->
                all <- HashMap.remove id all
                let s = box.Max - box.Min + V2i.II
                if s <> size && s.YX <> size then
                    failwithf "inconsistent sizes: %A vs %A" s size

            | None ->
                failwithf "missing id: %A" id

        // no extra rects
        all.IsEmpty |> should be True

    [<EntryPoint>]
    let run args =
        
        let gen = SizeGenerator()
        let seed = Random.mkStdGen 239856472398472L


        for size in [1;2;5;10;30] do
            let tests = Gen.eval size seed (Gen.listOfLength 1000 (Gen.nonEmptyListOf SizeGenerator.SizeGen))

            let tests = ( List.init size (fun _ -> Size(V2i(37,37))) ) :: tests

            for t in tests do
                ``square producing correct results`` (NonEmptyArray (List.toArray t))
                printfn "%A" t
        0

    //[<Test>]
    //let ``occupancy``() =

    //    let rand = RandomSystem()

    //    for cnt in [10;50;100] do
    //        let histogram = Array.zeroCreate 20
    //        let binSize = 1.0 / float histogram.Length

    //        printfn "%d:" cnt
    //        let iter = 100
    //        let mutable oMin = 1.0
    //        let mutable oMax = 0.0
    //        let mutable oSum = 0.0
    //        for i in 1 .. iter do
    //            let sizes = Array.init cnt (fun i -> i, rand.UniformV2i(63) + V2i.II)
    //            let packing = TexturePacking.square sizes

    //            let o = packing.Occupancy

    //            let bin = floor (o * float histogram.Length) |> int |> clamp 0 (histogram.Length - 1)
    //            histogram.[bin] <- histogram.[bin] + 1

    //            oMin <- min oMin o
    //            oMax <- max oMax o
    //            oSum <- oSum + o

    //        let oAvg = oSum / float iter

    //        printfn "  min: %.2f%%" (100.0 * oMin)
    //        printfn "  max: %.2f%%" (100.0 * oMax)
    //        printfn "  avg: %.2f%%" (100.0 * oAvg)


    //        let maxHisto = Array.max histogram
    //        let w = 10
    //        let mutable minValue = 0.0
    //        let mutable maxValue = binSize
    //        let mutable started = false
    //        for h in histogram do
    //            let p = float h / float iter
    //            if h > 0 then started <- true

    //            if started then
    //                let barWidth = (float w * float h / float maxHisto) |> round |> int
    //                let bar = System.String('#', barWidth) + System.String(' ', w - barWidth)

    //                if maxValue >= 0.999999 then
    //                    printfn "   >= %.0f%%    %s %.2f%%" (100.0 * minValue) bar (100.0 * p)

    //                else 
    //                    printfn "  [%.0f%%, %.0f%%) %s %.2f%%" (100.0 * minValue) (100.0 * maxValue) bar (100.0 * p)
    //            minValue <- minValue + binSize
    //            maxValue <- maxValue + binSize