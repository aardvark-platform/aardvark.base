
module Program

    open Aardvark.Base.FSharp.Tests
    open Aardvark.Base
    open ExpectoSvdTests
    open Expecto
    open FsCheck
    [<EntryPoint>]
    let main argv = 

        //Caches.``[Caches] BinaryCache partial backward with ResultObject``()
        //Caches.``[Caches] BinaryCache forward``()
        //Caches.``[Caches] BinaryCache forward with ResultObject``()
        //SVDTests.``[SVD] DecomposeWithArrayWeirdTranspose``()

        //let m =
        //    let mat = 
        //        Arbitraries.GenerateMatrix(DataDim.M44,Symmetric,Constant -0.5,FlipX) 
        //            |> Gen.eval 0 (FsCheck.Random.StdGen(889377570, 296706039))
        //    match mat with
        //        | RealMatrix mat ->     
        //            let (q,r) = QR.decompose mat
        //            q.IsUpperRight()
        //        | M22 mat ->      
        //            let (q,r) = QR.decompose mat
        //            (MatrixChoice.toRealMat (M22 q)).IsUpperRight()
        //        | M33 mat -> 
        //            let (q,r) = QR.decompose mat
        //            (MatrixChoice.toRealMat (M33 q)).IsUpperRight()
        //        | M44 mat -> 
        //            let (q,r) = QR.decompose mat
        //            let real = MatrixChoice.toRealMat (M44 q)
        //            real.IsUpperRight()
        //        | M23 mat -> 
        //            let (q,r) = QR.decompose mat
        //            (MatrixChoice.toRealMat (M22 q)).IsUpperRight()
        //        | M34 mat ->
        //            let (q,r) = QR.decompose mat
        //            (MatrixChoice.toRealMat (M33 q)).IsUpperRight()
        //Log.warn "%A" m
        try
            SVDTests.``[SVD] DecomposeWithNative``()
        with e ->
            printfn "%s" (e.ToString())
            ()

        0