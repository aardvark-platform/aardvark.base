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
        // works:
        let U = [| M33d() |]
        let S = [| m |] // m.Transposed
        let Vt = [| M33d() |]

        use pU = fixed U
        use pS = fixed S
        use pVt = fixed Vt

        let floatPtr = NativePtr.cast<M33d,float> pS
        let ptrp1 = NativePtr.step 1 floatPtr 
        let m01 = NativePtr.read ptrp1

        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(1, 3))) // 3, 1
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,3), V2l(1, 3))) // 3, 1
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(1, 3))) // 3, 1
        
        if SVD.DecomposeInPlace(tU, tS, tVt) then 
            let floatPtr = NativePtr.cast<M33d,float> pU
            let ptrp1 = NativePtr.step 1 floatPtr 
            let um01 = NativePtr.read ptrp1
            sprintf "%f" um01
            Some (U.[0], S.[0], Vt.[0]) // *.Transpose
        else
            None

        //let U = Array.zeroCreate<float> 9 // [| M33d() |]
        //let S = m.ToArray() //[| m |]
        //let Vt = Array.zeroCreate<float> 9 // [| M33d() |]

        //use pU = fixed U
        //use pS = fixed S
        //use pVt = fixed Vt

        //let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        //let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        //let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))

        //if SVD.DecomposeInPlace(tU, tS, tVt) then
        //    Some (M33d(U), M33d(S), M33d(Vt))
        //else
        //    None

    let DecomposeWithBuilder(m : M33d) =
        let mutable S = m
        let mutable U = M33d()
        let mutable Vt = M33d()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt

            let ptrp1 = NativePtr.step 1 pS.Pointer
            let m01 = NativePtr.read ptrp1

            suc <- SVD.DecomposeInPlace(pU, pS, pVt)

            let ptrp1 = NativePtr.step 1 pU.Pointer
            let um01 = NativePtr.read ptrp1
            sprintf "%f" um01
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    [<Test>]
    let ``[SVD] DecomposeWithArray``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = DecomposeWithArray mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

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
    let ``[SVD] DecomposeWithCompare``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd1 = DecomposeWithBuilder mat
            let svd2 = DecomposeWithArray mat

            match (svd1, svd2) with
            | (Some (q, s, v), Some(q2, s2, v2)) -> 
                                if q <> q2 then
                                    failwithf "Q differs" i
                                if s <> s2 then
                                    failwithf "S differs" i
                                if v <> v2 then
                                    failwithf "V differs" i
                                let test = q * s * v
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

