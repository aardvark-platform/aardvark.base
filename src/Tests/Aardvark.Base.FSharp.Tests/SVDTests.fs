namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework
open System.Runtime.InteropServices
open System.Diagnostics
open System.Collections.Generic

#nowarn "9"

module ExpectoSvdTests =
    open System
    open Aardvark.Base
    open Expecto
    open FsCheck

    type MatrixTrafo =
        | Id 
        | FlipX
        | FlipY 
        | Transposed
        | RandomMask

    type MatrixShape =
        | Full
        | Diagonal
        | Triangular of upper : bool
        | ProperTriangular of upper : bool
        | CopyRows
        | CopyCols
        | Symmetric

    type MatrixKind<'a> =
        | Arbitrary
        | Constant of 'a
        | Ones     
        | Zeros

    type WideMatrix<'a> = 
        {
            shape : MatrixShape
            kind : MatrixKind<'a>
            trafo : MatrixTrafo
            rows : int
            cols : int
            value : Matrix<'a>
        }    

    type PrettyMatrix<'a> = 
        {
            shape : MatrixShape
            kind : MatrixKind<'a>
            trafo : MatrixTrafo
            rows : int
            cols : int
            value : Matrix<'a>
        }    

    let lineRx = System.Text.RegularExpressions.Regex @"(\r\n)|\n"

    let indent (str : string) =
        lineRx.Split(str) |> Array.map ((+) "    ") |> String.concat "\n"

    let inline matStr (m : Matrix< ^a >) =
        let res = 
            Array.init (int m.SX) (fun c -> Array.init (int m.SY) (fun r -> 
                let str = sprintf "%.3f" (float m.[c,r])
                if str.StartsWith "-" then str
                else " " + str
            ))
        let lens = res |> Array.map (fun col -> col |> Seq.map String.length |> Seq.max)
        let pad (len : int) (s : string) =
            if s.Length < len then s + (System.String(' ',len-s.Length))
            else s

        let padded =
            res |> Array.mapi (fun i col -> 
                col |> Array.map (pad lens.[i])
            )

        String.concat "\n" [
            for r in 0..int m.SY-1 do
                let mutable line = ""
                for c in 0..int m.SX-1 do
                    line <- line + " " + padded.[c].[r]
                yield line.Trim()
        ]    

    let inline printMat (name : string) (m : NativeMatrix< ^a >) =
        let res = 
            Array.init (int m.SX) (fun c -> Array.init (int m.SY) (fun r -> 
                let str = sprintf "%.3f" (float m.[c,r])
                if str.StartsWith "-" then str
                else " " + str
            ))
        let lens = res |> Array.map (fun col -> col |> Seq.map String.length |> Seq.max)
        let pad (len : int) (s : string) =
            if s.Length < len then s + (System.String(' ',len-s.Length))
            else s

        let padded =
            res |> Array.mapi (fun i col -> 
                col |> Array.map (pad lens.[i])
            )
            
        System.Console.WriteLine("{0}", name)
        for r in 0..int m.SY-1 do
            let mutable line = ""
            for c in 0..int m.SX-1 do
                line <- line + " " + padded.[c].[r]
            System.Console.WriteLine("    {0}", line.Trim())



    type Arbitraries () = 

        static let properFloat =
            Arb.generate |> Gen.filter (fun v -> not (System.Double.IsNaN(v) || System.Double.IsInfinity(v) || abs(v) > 1E20 || abs(v) < 1E-20))

        static let arrayGen (k : MatrixKind<float>) (len : int)=
            match k with
            | Arbitrary -> 
                Gen.arrayOfLength len properFloat
            | Constant v -> 
                Gen.constant (Array.create len v)
            | Ones -> 
                Gen.constant (Array.create len 1.0)
            | Zeros -> 
                Gen.constant (Array.zeroCreate len)

        static member GenerateMatrix (r : int, c : int, s : MatrixShape, k : MatrixKind<float>, t : MatrixTrafo) =
            gen {
                let arrGen = arrayGen k
                let! mat = 
                    gen {
                        match s with
                        | Full -> 
                            let! fs = arrGen (r*c)
                            return Matrix<float>(fs, V2l(c,r))
                        | Diagonal -> 
                            let! fs = arrGen (min r c)
                            return Matrix<float>(V2l(c,r)).SetByCoord(fun (c : V2l) -> if c.X = c.Y then fs.[int c.X] else 0.0 )
                        | CopyCols -> 
                            let! fs = arrGen r
                            return Matrix<float>(V2l(c,r)).SetByCoord(fun (c : V2l) -> fs.[int c.Y])    
                        | CopyRows -> 
                            let! fs = arrGen c
                            return Matrix<float>(V2l(c,r)).SetByCoord(fun (c : V2l) -> fs.[int c.X])                             
                        | Triangular(upper) -> 
                            let ct =
                                match r <= c, upper with 
                                | true, true -> (r*(r+1))/2+(c-r)*r
                                | true, false -> (r*(r+1))/2
                                | false, true -> (c*(c+1))/2
                                | false, false -> (c*(c+1))/2+(r-c)*c

                            let! fs = arrGen ct
                            let mutable i = 0
                            return Matrix<float>(V2l(c,r)).SetByCoord(fun (c : V2l) ->
                                    if upper then
                                        if c.Y <= c.X then
                                            let v = fs.[i]
                                            i <- i+1
                                            v
                                        else 0.0                        
                                    else 
                                        if c.Y >= c.X then
                                            let v = fs.[i]
                                            i <- i+1
                                            v    
                                        else 0.0                       
                                   )
                           
                        | ProperTriangular(upper) -> 
                            let ct =
                                match r <= c, upper with 
                                | true, true -> (r*  (r-1))/2+(c-r)*r
                                | true, false -> (r* (r-1))/2
                                | false, true -> (c* (c-1))/2
                                | false, false -> (c*(c-1))/2+(r-c)*c

                            let! fs = arrGen ct
                            let mutable i = 0
                            return Matrix<float>(V2l(c,r)).SetByCoord(fun (c : V2l) ->
                                    if upper then
                                        if c.Y < c.X then
                                            let v = fs.[i]
                                            i <- i+1
                                            v
                                        else 0.0                        
                                    else 
                                        if c.Y > c.X then
                                            let v = fs.[i]
                                            i <- i+1
                                            v    
                                        else 0.0                       
                                   )
                        | Symmetric ->
                            let ct =
                                match r <= c with 
                                | true -> (r*(r+1))/2+(c-r)*r
                                | false -> (c*(c+1))/2+(r-c)*c
                            let! arr = arrGen ct

                            let mutable idx = 0
                            let cache = Dict()

                            let get (c : V2l) =
                                let k = 
                                    if c.X < c.Y then (c.X, c.Y)
                                    else (c.Y, c.X)

                                cache.GetOrCreate(k, fun k ->
                                    let i = idx
                                    idx <- idx + 1
                                    arr.[i]
                                )


                            return Matrix<float>(V2l(c,r)).SetByCoord get                       
                    }                   
                let! trafoed = 
                    gen {
                        match t with
                        | Id -> return mat
                        | FlipX -> 
                            let mo : Matrix<float> = mat.Copy()
                            return mat.SetByCoord(fun (c : V2l) -> mo.[mo.SX-1L-c.X, c.Y])
                        | FlipY -> 
                            let mo : Matrix<float> = mat.Copy()
                            return mat.SetByCoord(fun (c : V2l) -> mo.[c.X, mo.SY-1L-c.Y])
                        | Transposed ->
                            return mat.Transposed
                        | RandomMask ->
                            let! mask = Gen.arrayOfLength (r*c) Arb.generate<bool>
                            return mat.SetByIndex(fun i -> if mask.[int i] then mat.[i] else 0.0)
                    }
                
                return trafoed            
            }
        
        static member MatrixKind() = 
            gen {
                let! ctor = Gen.frequency [1, Gen.constant Arbitrary; 1, Gen.constant Ones; 1, Gen.constant Zeros; 1, Gen.constant (Constant 0.0)]      
                match ctor with
                | Constant _ ->
                    let! f = properFloat
                    return Constant f
                | e ->
                    return e
            } |> Arb.fromGen
        
        static member Matrix() =
            gen {
                let! square = Gen.frequency [1, Gen.constant true; 10, Gen.constant false]      
                let! (r : int) = Gen.choose(1,25)
                let! (c : int) = if square then Gen.constant r else Gen.choose(1,25)
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo>


                let! value = Arbitraries.GenerateMatrix(r, c, s, k,t)
                return 
                    {
                        shape = s
                        kind = k                    
                        trafo = t
                        rows = r
                        cols = c
                        value = value
                    }                
            } |> Arb.fromGen

        static member MatrixFloat() =
            gen {
                let! square = Gen.frequency [1, Gen.constant true; 10, Gen.constant false]      
                let! (r : int) = Gen.choose(1,25)
                let! (c : int) = if square then Gen.constant r else Gen.choose(1,25)
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo>


                let! value = Arbitraries.GenerateMatrix(r, c, s, k,t)
                let k =
                    match k with
                    | Zeros -> Zeros
                    | Ones -> Ones
                    | Constant v -> Constant (float32 v)
                    | Arbitrary -> Arbitrary

                return 
                    {
                        shape = s
                        kind = k                    
                        trafo = t
                        rows = r
                        cols = c
                        value = value.Map(float32)
                    }                
            } |> Arb.fromGen
        
        static member WideMatrix() =
            gen {
                let! square = Gen.frequency [1, Gen.constant true; 10, Gen.constant false]      
                let! (r : int) = Gen.choose(1,25)
                let! (c : int) = if square then Gen.constant r else Gen.choose(r,26)
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo> |> Gen.filter (fun trafo -> trafo <> Transposed)


                let! value = Arbitraries.GenerateMatrix(r, c, s, k,t)
                return 
                    {
                        WideMatrix.shape = s
                        WideMatrix.kind = k                    
                        WideMatrix.trafo = t
                        WideMatrix.rows = r
                        WideMatrix.cols = c
                        WideMatrix.value = value
                    }                
            } |> Arb.fromGen

        static member WideMatrixFloat() =
            gen {
                let! square = Gen.frequency [1, Gen.constant true; 10, Gen.constant false]      
                let! (r : int) = Gen.choose(1,25)
                let! (c : int) = if square then Gen.constant r else Gen.choose(r,26)
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo> |> Gen.filter (fun trafo -> trafo <> Transposed)


                let! value = Arbitraries.GenerateMatrix(r, c, s, k,t)            
            
                let k =
                    match k with
                    | Zeros -> Zeros
                    | Ones -> Ones
                    | Constant v -> Constant (float32 v)
                    | Arbitrary -> Arbitrary

                return 
                    {
                        WideMatrix.shape = s
                        WideMatrix.kind = k                    
                        WideMatrix.trafo = t
                        WideMatrix.rows = r
                        WideMatrix.cols = c
                        WideMatrix.value = value.Map(float32)
                    }                             
            } |> Arb.fromGen

    let cfg : FsCheckConfig =
        { FsCheckConfig.defaultConfig with 
            maxTest = 10000
            arbitrary = [ typeof<Arbitraries> ] 
        }

    let epsilon = 1E-8
    let floatEpsilon = 0.1f

    open System.Runtime.CompilerServices

    [<Extension; Sealed; AbstractClass>]
    type MatrixProperties private() =

        [<Extension>]
        static member IsIdentity(m : Matrix<float>) =
            let mutable maxError = 0.0
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                let v = if x = y then 1.0 else 0.0
                let err = abs (m.[i] - v)
                maxError <- max maxError err
            )
            if maxError > epsilon then
                printfn "ERROR: %A" maxError
            maxError <= epsilon

        [<Extension>]
        static member IsIdentity(m : Matrix<float32>) =
            let mutable maxError = 0.0f
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                let v = if x = y then 1.0f else 0.0f
                let err = abs (m.[i] - v)
                maxError <- max maxError err
            )
            if maxError > floatEpsilon then
                printfn "ERROR: %A" maxError
            maxError <= floatEpsilon

        [<Extension>]
        static member IsOrtho(m : Matrix<float>) =
            m.Multiply(m.Transposed).IsIdentity()

        [<Extension>]
        static member IsOrtho(m : Matrix<float32>) =
            m.Multiply(m.Transposed).IsIdentity()


        [<Extension>]
        static member IsUpperRight(m : Matrix<float>) =
            let mutable maxError = 0.0
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                if x < y then 
                    let err = abs m.[i]
                    maxError <- max maxError err
            )
            if maxError > epsilon then
                printfn "ERROR: %A" maxError
            maxError <= epsilon

        [<Extension>]
        static member IsUpperRight(m : Matrix<float32>) =
            let mutable maxError = 0.0f
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                if x < y then 
                    let err = abs m.[i]
                    maxError <- max maxError err
            )
            if maxError > floatEpsilon then
                printfn "ERROR: %A" maxError
            maxError <= floatEpsilon
    

        [<Extension>]
        static member ApproximateEquals(a : Matrix<float>, b : Matrix<float>) =
            let err = a.InnerProduct(b,(fun a b -> abs (a-b)),0.0,max)
            if err > epsilon then
                printfn "ERROR: %A" err
            err <= epsilon 

        [<Extension>]
        static member ApproximateEquals(a : Matrix<float32>, b : Matrix<float32>) =
            let err = a.InnerProduct(b,(fun a b -> abs (a-b)),0.0f,max)
            if err > floatEpsilon then
                printfn "ERROR: %A" err
            err <= floatEpsilon    

        [<Extension>]
        static member MaxDiff(a : Matrix<float>, b : Matrix<float>) =
            a.InnerProduct(b,(fun a b -> abs (a-b)),0.0,max)
        
        [<Extension>]
        static member IsBidiagonal(m : Matrix<float>) =
            let mutable maxError = 0.0
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                let err = if (x = y || x = y+1L) then 0.0 else abs m.[i]
                maxError <- max maxError err
            )
            if maxError > epsilon then
                printfn "ERROR: %A" maxError
            maxError <= epsilon 


        [<Extension>]
        static member IsBidiagonal(m : Matrix<float32>) =
            let mutable maxError = 0.0f
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                let err = if (x = y || x = y+1L) then 0.0f else abs m.[i]
                maxError <- max maxError err
            )
            if maxError > floatEpsilon then
                printfn "ERROR: %A" maxError
            maxError <= floatEpsilon


        [<Extension>]
        static member IsDiagonal(m : Matrix<float>) =
            let mutable maxError = 0.0
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                let err = if x = y then 0.0 else abs m.[i]
                maxError <- max maxError err
            )
            if maxError > epsilon then
                printfn "ERROR: %A" maxError
            maxError <= epsilon 


        [<Extension>]
        static member IsDiagonal(m : Matrix<float32>) =
            let mutable maxError = 0.0f
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                let err = if x = y then 0.0f else abs m.[i]
                maxError <- max maxError err
            )
            if maxError > floatEpsilon then
                printfn "ERROR: %A" maxError
            maxError <= floatEpsilon


        [<Extension>]
        static member DecreasingDiagonal(m : Matrix<float>) =
            let mutable wrong = []
            let mutable last = System.Double.PositiveInfinity
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                if x = y then 
                    if m.[i] > last then
                        wrong <- (last, m.[i]) :: wrong
                    last <- m.[i]
            )
            match wrong with
            | [] -> 
                true
            | wrong ->  
                printfn "ERROR: %A" wrong
                false


        [<Extension>]
        static member DecreasingDiagonal(m : Matrix<float32>) =
            let mutable wrong = []
            let mutable last = System.Single.PositiveInfinity
            m.ForeachXYIndex (fun (x : int64) (y : int64) (i : int64) ->
                if x = y then 
                    if m.[i] > last then
                        wrong <- (last, m.[i]) :: wrong
                    last <- m.[i]
            )
            match wrong with
            | [] -> 
                true
            | wrong ->  
                printfn "ERROR: %A" wrong
                false

    let qr =
        testList "[QR64] decompose" [
            testPropertyWithConfig cfg "[QR64] Q*R = M" (fun (mat : PrettyMatrix<float>) -> 
                let (q,r) = QR.decompose mat.value
                let res = q.Multiply(r)
                res.ApproximateEquals(mat.value)
            )

            testPropertyWithConfig cfg "[QR64] Q*Qt = ID" (fun (mat : PrettyMatrix<float> ) -> 
                let (q,_) = QR.decompose mat.value
                q.IsOrtho()
            )

            testPropertyWithConfig cfg "[QR64] R = right upper" (fun (mat : PrettyMatrix<float> ) -> 
                let (_,r) = QR.decompose mat.value
                r.IsUpperRight()
            )
        ]    

    let rq =
        testList "[RQ64] decompose" [
            testPropertyWithConfig cfg "[RQ64] R*Q = M" (fun (mat : WideMatrix<float>) -> 
                if mat.value.SY > mat.value.SX then failwith "asuofhasiofh"
                let (r,q) = RQ.decompose mat.value
                let res = r.Multiply(q)
                res.ApproximateEquals(mat.value)
            )

            testPropertyWithConfig cfg "[RQ64] Q*Qt = ID" (fun (mat : WideMatrix<float> ) -> 
                let (_,q) = RQ.decompose mat.value
                q.IsOrtho()
            )

            testPropertyWithConfig cfg "[RQ64] R = right upper" (fun (mat : WideMatrix<float> ) -> 
                let (r,_) = RQ.decompose mat.value
                r.IsUpperRight()
            )
        ]    

    let rq32 =
        testList "[RQ32] decompose" [
            testPropertyWithConfig cfg "[RQ32] R*Q = M" (fun (mat : WideMatrix<float32>) -> 
                if mat.value.SY > mat.value.SX then failwith "asuofhasiofh"
                let (r,q) = RQ.decompose mat.value
                let res = r.Multiply(q)
                res.ApproximateEquals(mat.value)
            )

            testPropertyWithConfig cfg "[RQ32] Q*Qt = ID" (fun (mat : WideMatrix<float32> ) -> 
                let (_,q) = RQ.decompose mat.value
                q.IsOrtho()
            )

            testPropertyWithConfig cfg "[RQ32] R = right upper" (fun (mat : WideMatrix<float32> ) -> 
                let (r,_) = RQ.decompose mat.value
                r.IsUpperRight()
            )
        ]

    let qrBidiag = 
        testList "[QR64] bidiagonalize" [
            testPropertyWithConfig cfg "[QR64] U*Ut = ID " (fun (mat : PrettyMatrix<float>) -> 
                let (u,_,_) = QR.Bidiagonalize mat.value
                u.IsOrtho()
            )
            testPropertyWithConfig cfg "[QR64] D is bidiagonal" (fun (mat : PrettyMatrix<float>) -> 
                let (_,d,_) = QR.Bidiagonalize mat.value
                d.IsBidiagonal()
            )

            testPropertyWithConfig cfg "[QR64] V*Vt = ID" (fun (mat : PrettyMatrix<float>) -> 
                let (_,_,vt) = QR.Bidiagonalize mat.value
                vt.IsOrtho()
            )        
            testPropertyWithConfig cfg "[QR64] U*D*Vt = M" (fun (mat : PrettyMatrix<float>) -> 
                let (u,d,vt) = QR.Bidiagonalize mat.value
                let res = u.Multiply(d.Multiply(vt))
                res.ApproximateEquals(mat.value)
            )
        ]    

    let svd = 
        testList "[SVD64] decompose" [
            testPropertyWithConfig cfg "[SVD64] U*Ut = ID " (fun (mat : PrettyMatrix<float>) -> 
                match SVD.decompose mat.value with
                | Some (u,_,_) -> u.IsOrtho()
                | None -> true
            )
            testPropertyWithConfig cfg "[SVD64] S is diagonal" (fun (mat : PrettyMatrix<float>) -> 
                match SVD.decompose mat.value with
                | Some (_,s,_) -> s.IsDiagonal()
                | None -> true
            )
        
            testPropertyWithConfig cfg "[SVD64] S is decreasing" (fun (mat : PrettyMatrix<float>) -> 
                match SVD.decompose mat.value with
                | Some (_,s,_) -> s.DecreasingDiagonal()
                | None -> true
            )

            testPropertyWithConfig cfg "[SVD64] V*Vt = ID" (fun (mat : PrettyMatrix<float>) -> 
                match SVD.decompose mat.value with
                | Some (_,_,vt) -> vt.IsOrtho()
                | None -> true
            )        
            testPropertyWithConfig cfg "[SVD64] U*S*Vt = M" (fun (mat : PrettyMatrix<float>) -> 
                match SVD.decompose mat.value with
                | Some (u,s,vt) -> 
                    let res = u.Multiply(s.Multiply(vt))
                    res.ApproximateEquals(mat.value)
                | None ->
                    true            
            )
        ]   
  

    let svd32 = 
        testList "[SVD32] decompose" [
            testPropertyWithConfig cfg "[SVD32] U*Ut = ID " (fun (mat : PrettyMatrix<float32>) -> 
                match SVD.decompose mat.value with
                | Some (u,_,_) -> u.IsOrtho()
                | None -> true
            )
            testPropertyWithConfig cfg "[SVD32] S is diagonal" (fun (mat : PrettyMatrix<float32>) -> 
                match SVD.decompose mat.value with
                | Some (_,s,_) -> s.IsDiagonal()
                | None -> true
            )
        
            testPropertyWithConfig cfg "[SVD32] S is decreasing" (fun (mat : PrettyMatrix<float32>) -> 
                match SVD.decompose mat.value with
                | Some (_,s,_) -> s.DecreasingDiagonal()
                | None -> true
            )

            testPropertyWithConfig cfg "[SVD32] V*Vt = ID" (fun (mat : PrettyMatrix<float32>) -> 
                match SVD.decompose mat.value with
                | Some (_,_,vt) -> vt.IsOrtho()
                | None -> true
            )        
            testPropertyWithConfig cfg "[SVD32] U*S*Vt = M" (fun (mat : PrettyMatrix<float32>) -> 
                match SVD.decompose mat.value with
                | Some (u,s,vt) -> 
                    let res = u.Multiply(s.Multiply(vt))
                    res.ApproximateEquals(mat.value)
                | None ->
                    true            
            )
        ]   

    let qr32 =
        testList "[QR32] decompose" [
            testPropertyWithConfig cfg "[QR32] Q*R = M" (fun (mat : PrettyMatrix<float32>) -> 
                let (q,r) = QR.decompose mat.value
                let res = q.Multiply(r)
                res.ApproximateEquals(mat.value)
            )

            testPropertyWithConfig cfg "[QR32] Q*Qt = ID" (fun (mat : PrettyMatrix<float32> ) -> 
                let (q,_) = QR.decompose mat.value
                q.IsOrtho()
            )

            testPropertyWithConfig cfg "[QR32] R = right upper" (fun (mat : PrettyMatrix<float32> ) -> 
                let (_,r) = QR.decompose mat.value
                r.IsUpperRight()
            )
        ]    

    let qrBidiag32 = 
        testList "[QR32] Bidiagonalize" [
            testPropertyWithConfig cfg "[QR32] U*Ut = ID " (fun (mat : PrettyMatrix<float32>) -> 
                let (u,_,_) = QR.Bidiagonalize mat.value
                u.IsOrtho()
            )
            testPropertyWithConfig cfg "[QR32] D is bidiagonal" (fun (mat : PrettyMatrix<float32>) -> 
                let (_,d,_) = QR.Bidiagonalize mat.value
                d.IsBidiagonal()
            )

            testPropertyWithConfig cfg "[QR32] V*Vt = ID" (fun (mat : PrettyMatrix<float32>) -> 
                let (_,_,vt) = QR.Bidiagonalize mat.value
                vt.IsOrtho()
            )        
            testPropertyWithConfig cfg "[QR32] U*D*Vt = M" (fun (mat : PrettyMatrix<float32>) -> 
                let (u,d,vt) = QR.Bidiagonalize mat.value
                let res = u.Multiply(d.Multiply(vt))
                res.ApproximateEquals(mat.value)
            )
        ]    
    let all =
        testList "AllTests" [
            qr
            rq
            qrBidiag
            svd
            //qr32
            //rq32
            //qrBidiag32
            //svd32
        ]




module SVDTests =
    open Microsoft.FSharp.NativeInterop
    open System.Runtime.InteropServices

    let DecomposeWithArraySingle(m : M33d) =
        let U = [| M33d() |]
        let S = [| m |]
        let V = [| M33d() |]

        use pU = fixed U
        let pS = fixed S
        let pV = fixed V

        let tU  = NativeMatrix<float>(NativePtr.cast pU, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tVt = NativeMatrix<float>(NativePtr.cast pV, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        
        if SVD.DecomposeInPlace(tU, tS, tVt) then 
            Some (U.[0], S.[0], V.[0])
        else
            None

    let DecomposeWithArray(m : M33d) =
        let tmp = [| M33d(); m; M33d() |]

        use pTmp = fixed tmp
        let pS = NativePtr.add pTmp 1
        let pV = NativePtr.add pTmp 2

        let tU  = NativeMatrix<float>(NativePtr.cast pTmp,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tVt = NativeMatrix<float>(NativePtr.cast pV, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        
        if SVD.DecomposeInPlace(tU, tS, tVt) then 
            Some (tmp.[0], tmp.[1], tmp.[2])
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

    let DecomposeWithNativeFloat32(m : M33f) =
        let pU : nativeptr<M33f> = NativePtr.alloc 3
        let pS : nativeptr<M33f> = NativePtr.alloc 3
        let pVt : nativeptr<M33f> = NativePtr.alloc 3

        try
            let pU1 = NativePtr.add pU 1
            let pS1 = NativePtr.add pS 1
            let pV1 = NativePtr.add pVt 1

            let pU2 = NativePtr.add pU 2
            let pS2 = NativePtr.add pS 2
            let pV2 = NativePtr.add pVt 2

            let nanM33d = M33f(Array.create 9 Single.NaN)
            NativePtr.write pU nanM33d
            NativePtr.write pS nanM33d
            NativePtr.write pVt nanM33d
            NativePtr.write pU2 nanM33d
            NativePtr.write pS2 nanM33d
            NativePtr.write pV2 nanM33d

            NativePtr.write pU1 (M33f())
            NativePtr.write pS1 m
            NativePtr.write pV1 (M33f())

            let tU  = NativeMatrix<float32>(NativePtr.cast pU1, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
            let tS  = NativeMatrix<float32>(NativePtr.cast pS1, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
            let tVt = NativeMatrix<float32>(NativePtr.cast pV1, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))

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
    let ``[SVD] DecomposeWithArraySingle``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M33d.FromCols(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d())

            let svd = DecomposeWithArraySingle mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

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
    let ``[SVD] DecomposeWithNative``() =

        let rnd = new RandomSystem(1)
        let iter = 100000
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
    let ``[SVD] DecomposeWithNativeFloat32``() =

        let rnd = new RandomSystem(1)
        let iter = 100000
        let mutable last = 0.0
        for i in 0..iter do
            let p = float i / float iter
            if p > last + 0.05 then 
                printfn "%.2f%%" (100.0 * p)
                last <- p

            let mat = M33f.FromCols(rnd.UniformV3f(), rnd.UniformV3f(), rnd.UniformV3f())

            let svd = DecomposeWithNativeFloat32 mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-3f)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwithf "NONE at %d" i

    [<Test; Ignore("Builder pattern is illegal for structs")>]
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

    [<Test>]
    let ``[SVD] random M22f``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M22f.FromCols(rnd.UniformV2f(), rnd.UniformV2f())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6f)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"


    [<Test>]
    let ``[SVD] random M23f``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M23f.FromCols(rnd.UniformV2f(), rnd.UniformV2f(), rnd.UniformV2f())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6f)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"


    [<Test>]
    let ``[SVD] random M33f``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M33f.FromCols(rnd.UniformV3f(), rnd.UniformV3f(), rnd.UniformV3f())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6f)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] random M34f``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M34f.FromCols(rnd.UniformV3f(), rnd.UniformV3f(), rnd.UniformV3f(), rnd.UniformV3f())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-6f)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"

    [<Test>]
    let ``[SVD] random M44f``() =

        let rnd = new RandomSystem(1)
        for i in 0..100000 do
            let mat = M44f.FromCols(rnd.UniformV4f(), rnd.UniformV4f(), rnd.UniformV4f(), rnd.UniformV4f())

            let svd = SVD.decompose mat

            match svd with
            | Some (q, s, v) -> let test = q * s * v
                                if not (Fun.ApproximateEquals(test, mat, 1e-2f)) then
                                    failwithf "Invalid SVD at %d" i
            | _ -> failwith "NONE"