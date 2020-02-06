namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework
open System.Runtime.InteropServices
open System.Diagnostics
open System.Collections.Generic



module SVDTests =
    open Microsoft.FSharp.NativeInterop
    open System.Runtime.InteropServices

    let DecomposeWithArray(m : M33d) =
        let U = [| M33d() |]
        let S = [| m |]
        let Vt = [| M33d() |]

        use pU = fixed U
        use pS = fixed S
        use pVt = fixed Vt

        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        
        if SVD.DecomposeInPlace(tU, tS, tVt) then 
            Some (U.[0], S.[0], Vt.[0])
        else
            None

    let DecomposeWithArrayWeirdTranspose(m : M33d) =
        // works:
        let U = [| M33d() |]
        let S = [| m.Transposed |]
        let Vt = [| M33d() |]

        use pU = fixed U
        use pS = fixed S
        use pVt = fixed Vt

        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(3, 1)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,3), V2l(3, 1)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(3, 1)))
        
        if SVD.DecomposeInPlace(tU, tS, tVt) then 
            Some (U.[0].Transposed, S.[0].Transposed, Vt.[0].Transposed)
        else
            None

    let DecomposeWithNative(m : M33d) =
        let pU : nativeptr<M33d> = NativePtr.alloc 3
        let pS : nativeptr<M33d> = NativePtr.alloc 3
        let pVt : nativeptr<M33d> = NativePtr.alloc 3

        try
            let pU1 = NativePtr.add pU 1
            let pS1 = NativePtr.add pS 1
            let pV1 = NativePtr.add pVt 1

            let pU2 = NativePtr.add pU 2
            let pS2 = NativePtr.add pS 2
            let pV2 = NativePtr.add pVt 2

            let nanM33d = M33d(Array.create 9 Double.NaN)
            NativePtr.write pU nanM33d
            NativePtr.write pS nanM33d
            NativePtr.write pVt nanM33d
            NativePtr.write pU2 nanM33d
            NativePtr.write pS2 nanM33d
            NativePtr.write pV2 nanM33d

            NativePtr.write pU1 (M33d())
            NativePtr.write pS1 m
            NativePtr.write pV1 (M33d())

            let tU  = NativeMatrix<float>(NativePtr.cast pU1, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
            let tS  = NativeMatrix<float>(NativePtr.cast pS1, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
            let tVt = NativeMatrix<float>(NativePtr.cast pV1, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))

            let suc : bool = SVD.DecomposeInPlace(tU, tS, tVt)

            let testUl = NativePtr.read (pU)
            let testUh = NativePtr.read (pU2)
            let testSl = NativePtr.read (pS)
            let testSh = NativePtr.read (pS2)
            let testVl = NativePtr.read (pVt)
            let testVh = NativePtr.read (pV2)
            if testUl.Elements |> Seq.exists (fun x -> not (x.IsNaN())) then failwith "not NaN"
            if testUh.Elements |> Seq.exists (fun x -> not (x.IsNaN())) then failwith "not NaN"
            if testSl.Elements |> Seq.exists (fun x -> not (x.IsNaN())) then failwith "not NaN"
            if testSh.Elements |> Seq.exists (fun x -> not (x.IsNaN())) then failwith "not NaN"
            if testVl.Elements |> Seq.exists (fun x -> not (x.IsNaN())) then failwith "not NaN"
            if testVh.Elements |> Seq.exists (fun x -> not (x.IsNaN())) then failwith "not NaN"

            let Ures = NativePtr.read pU1
            let Sres =  NativePtr.read pS1
            let Vres = NativePtr.read pV1

            if Ures.Elements |> Seq.exists (fun x -> x.IsNaN()) then failwith "any NaN"
            if Sres.Elements |> Seq.exists (fun x -> x.IsNaN()) then failwith "any NaN"
            if Vres.Elements |> Seq.exists (fun x -> x.IsNaN()) then failwith "any NaN"

            
            if suc then 
                Some (Ures, Sres, Vres)
            else
                None
        finally
            NativePtr.free pU
            NativePtr.free pS
            NativePtr.free pVt

    let DecomposeWithBuilder(m : M33d) =
        let mutable S = m
        let mutable U = M33d()
        let mutable Vt = M33d()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    [<Test>]
    let ``[SVD] DecomposeWithArray``() =

        let rnd = new RandomSystem(1)
        for i in 0..10000000 do
            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = DecomposeWithArray mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] DecomposeWithArrayWeirdTranspose``() =

        let rnd = new RandomSystem(1)
        for i in 0..1000000 do
            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = DecomposeWithArrayWeirdTranspose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] DecomposeWithNative``() =

        let rnd = new RandomSystem(1)
        let iter = 10000000
        let mutable last = 0.0
        for i in 0..iter do
            let p = float i / float iter
            if p > last + 0.05 then 
                printfn "%.2f%%" (100.0 * p)
                last <- p

            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = DecomposeWithNative mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwithf "NONE at %d" i

    [<Test>]
    let ``[SVD] DecomposeWithBuilder``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = DecomposeWithBuilder mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] not 0``() =
        for i in 0..100000 do
            let mat = new M33d(100.0 + (float)i*1e-9, 0.0, 0.0,
                                 0.0, 0.998672148110852, 0.051516410857669677,
                                 0.0, -0.051516410857669677, 0.998672148110852)

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> if q = M33d.Zero then
                                    failwithf "ALL 0 at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] random M22d``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M22d.FromCols(rnd.UniformV2d(), rnd.UniformV2d())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    
    [<Test>]
    let ``[SVD] random M23d``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M23d.FromCols(rnd.UniformV2d(), rnd.UniformV2d(), rnd.UniformV2d())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"


    [<Test>]
    let ``[SVD] random M33d``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] random M34d``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M34d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] random M44d``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M44d.FromCols(rnd.UniformV4d(), rnd.UniformV4d(), rnd.UniformV4d(), rnd.UniformV4d())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

