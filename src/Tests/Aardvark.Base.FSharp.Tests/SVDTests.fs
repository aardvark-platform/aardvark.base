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

    type DataDim =
        | Mat of rows : int * cols : int
        | M22
        | M23
        | M33
        | M34
        | M44

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




        
    type MatrixChoice =
        | RealMatrix of Matrix<float>
        | M22 of M22d
        | M23 of M23d
        | M33 of M33d
        | M34 of M34d
        | M44 of M44d
        
    type MatrixChoice32 =
        | FRealMatrix of Matrix<float32>
        | FM22 of M22f
        | FM23 of M23f
        | FM33 of M33f
        | FM34 of M34f
        | FM44 of M44f

    type WideMatrix<'a> = 
        {
            shape : MatrixShape
            kind : MatrixKind<'a>
            trafo : MatrixTrafo
            dim : DataDim
            value : MatrixChoice
        }    

    type PrettyMatrix<'a> = 
        {
            shape : MatrixShape
            kind : MatrixKind<'a>
            trafo : MatrixTrafo
            dim : DataDim
            value : MatrixChoice
        }    

    type WideMatrix32<'a> = 
        {
            shape : MatrixShape
            kind : MatrixKind<'a>
            trafo : MatrixTrafo
            dim : DataDim
            value : MatrixChoice32
        }    

    type PrettyMatrix32<'a> = 
        {
            shape : MatrixShape
            kind : MatrixKind<'a>
            trafo : MatrixTrafo
            dim : DataDim
            value : MatrixChoice32
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


    let createMatrix (d : DataDim) (fs : float[]) =
        match d with
        | DataDim.Mat(r,c) -> Matrix<float>(fs, V2l(c,r)) |> RealMatrix
        | DataDim.M22 -> M22d(fs) |> M22
        | DataDim.M23 -> M23d(fs) |> M23
        | DataDim.M33 -> M33d(fs) |> M33
        | DataDim.M34 -> M34d(fs) |> M34
        | DataDim.M44 -> M44d(fs) |> M44
        
    let createZeroMatrix (d : DataDim)  =
        match d with
        | DataDim.Mat(r,c) -> Matrix<float>(V2l(c,r)) |> RealMatrix
        | DataDim.M22 -> M22d() |> M22
        | DataDim.M23 -> M23d() |> M23
        | DataDim.M33 -> M33d() |> M33
        | DataDim.M34 -> M34d() |> M34
        | DataDim.M44 -> M44d() |> M44

    module DataDim =
        let getRows (d : DataDim) =
            match d with
            | DataDim.Mat(r,c) -> r
            | DataDim.M22 -> 2
            | DataDim.M23 -> 2
            | DataDim.M33 -> 3
            | DataDim.M34 -> 3
            | DataDim.M44 -> 4
            
        let getCols (d : DataDim) =
            match d with
            | DataDim.Mat(r,c) -> c
            | DataDim.M22 -> 2
            | DataDim.M23 -> 3
            | DataDim.M33 -> 3
            | DataDim.M34 -> 4
            | DataDim.M44 -> 4

    module MatrixChoice =
        let toRealMat (mat : MatrixChoice) =
            match mat with
            | RealMatrix mat -> mat
            | M22 mat ->       
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float>(fs, sizes)
            | M33 mat ->           
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float>(fs, sizes)
            | M44 mat ->        
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float>(fs, sizes)      
            | M23 mat ->        
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float>(fs, sizes)
            | M34 mat ->        
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float>(fs, sizes)
        let toRealMat32 (mat : MatrixChoice32) =
            match mat with
            | FRealMatrix mat -> mat
            | FM22 mat ->       
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float32>(fs, sizes)
            | FM33 mat ->           
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float32>(fs, sizes)   
            | FM44 mat ->        
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float32>(fs, sizes)      
            | FM23 mat ->        
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float32>(fs, sizes)
            | FM34 mat ->        
                let fs = mat.ToArray()
                let sizes = mat.Dim.YX
                Matrix<float32>(fs, sizes)
            
        let to32 (m : MatrixChoice) =
            match m with
            | RealMatrix mat -> FRealMatrix (mat.Map(float32))
            | M22 mat ->
                FM22 <| M22f( 
                    float32 mat.M00, float32 mat.M01,
                    float32 mat.M10, float32 mat.M11
                )
            | M23 mat ->
                FM23 <| M23f( 
                    float32 mat.M00, float32 mat.M01, float32 mat.M02,
                    float32 mat.M10, float32 mat.M11, float32 mat.M12
                )
            | M33 mat ->
                FM33 <| M33f( 
                    float32 mat.M00, float32 mat.M01, float32 mat.M02,
                    float32 mat.M10, float32 mat.M11, float32 mat.M12,
                    float32 mat.M20, float32 mat.M21, float32 mat.M22
                )
            | M34 mat ->
                FM34 <| M34f( 
                    float32 mat.M00, float32 mat.M01, float32 mat.M02, float32 mat.M03,
                    float32 mat.M10, float32 mat.M11, float32 mat.M12, float32 mat.M13,
                    float32 mat.M20, float32 mat.M21, float32 mat.M22, float32 mat.M23
                )
            | M44 mat ->
                FM44 <| M44f( 
                    float32 mat.M00, float32 mat.M01, float32 mat.M02, float32 mat.M03,
                    float32 mat.M10, float32 mat.M11, float32 mat.M12, float32 mat.M13,
                    float32 mat.M20, float32 mat.M21, float32 mat.M22, float32 mat.M23,
                    float32 mat.M30, float32 mat.M31, float32 mat.M32, float32 mat.M33
                )

        let copy (m : MatrixChoice) =
            match m with
            | RealMatrix mat -> RealMatrix (mat.Copy())
            | M22 mat -> M22 (mat.Copy(id))
            | M33 mat -> M33 (mat.Copy(id))
            | M44 mat -> M44 (mat.Copy(id))
            | M23 mat -> M23 (mat.Copy(id)) 
            | M34 mat -> M34 (mat.Copy(id)) 

        let get (i : V2l) (m : MatrixChoice) =
            match m with
            | RealMatrix mat -> mat.[i]
            | M22 mat -> mat.[i]
            | M33 mat -> mat.[i]
            | M44 mat -> mat.[i]
            | M23 mat -> mat.[i] 
            | M34 mat -> mat.[i] 
            
        let geti (i : int64) (m : MatrixChoice) =
            match m with
            | RealMatrix mat -> mat.[i]
            | M22 mat -> mat.ToArray().[int i]
            | M33 mat -> mat.ToArray().[int i]
            | M44 mat -> mat.ToArray().[int i]
            | M23 mat -> mat.ToArray().[int i] 
            | M34 mat -> mat.ToArray().[int i] 

        let setByCoord (f : V2l -> float) (m : MatrixChoice) =
            match m with
            | RealMatrix mat -> mat.SetByCoord f |> RealMatrix
            | M22 mat ->
                M22 <| M22d( 
                    f(V2l(0,0)), f(V2l(1,0)), 
                    f(V2l(0,1)), f(V2l(1,1))
                )
            | M23 mat ->
                M23 <| M23d( 
                    f(V2l(0,0)), f(V2l(1,0)), f(V2l(2,0)), 
                    f(V2l(0,1)), f(V2l(1,1)), f(V2l(2,1))
                )
            | M33 mat ->
                M33 <| M33d( 
                    f(V2l(0,0)), f(V2l(1,0)), f(V2l(2,0)), 
                    f(V2l(0,1)), f(V2l(1,1)), f(V2l(2,1)),
                    f(V2l(0,2)), f(V2l(1,2)), f(V2l(2,2))
                )
            | M34 mat ->
                M34 <| M34d( 
                    f(V2l(0,0)), f(V2l(1,0)), f(V2l(2,0)), f(V2l(3,0)),
                    f(V2l(0,1)), f(V2l(1,1)), f(V2l(2,1)), f(V2l(3,1)),
                    f(V2l(0,2)), f(V2l(1,2)), f(V2l(2,2)), f(V2l(3,2))
                )
            | M44 mat ->
                M44 <| M44d( 
                    f(V2l(0,0)), f(V2l(1,0)), f(V2l(2,0)), f(V2l(3,0)),
                    f(V2l(0,1)), f(V2l(1,1)), f(V2l(2,1)), f(V2l(3,1)),
                    f(V2l(0,2)), f(V2l(1,2)), f(V2l(2,2)), f(V2l(3,2)),
                    f(V2l(0,3)), f(V2l(1,3)), f(V2l(2,3)), f(V2l(3,3))
                )
                
        let setByIndex (f : int64 -> float) (m : MatrixChoice) =
            match m with
            | RealMatrix mat -> mat.SetByIndex f |> RealMatrix
            | M22 mat ->
                M22 <| M22d( 
                    f(0L), f(1L), 
                    f(2L), f(3L)
                )
            | M23 mat ->
                M23 <| M23d( 
                    f(0L), f(1L), f(2L),
                    f(3L), f(4L), f(5L)
                )
            | M33 mat ->
                M33 <| M33d( 
                    f(0L), f(1L), f(2L),
                    f(3L), f(4L), f(5L),
                    f(6L), f(7L), f(8L)
                )
            | M34 mat ->
                M34 <| M34d( 
                    f(0L), f(1L), f(2L), f(3L), 
                    f(4L), f(5L), f(6L), f(7L), 
                    f(8L), f(9L), f(10L), f(11L)
                )
            | M44 mat ->
                M44 <| M44d( 
                    f(0L), f(1L), f(2L), f(3L), 
                    f(4L), f(5L), f(6L), f(7L), 
                    f(8L), f(9L), f(10L), f(11L),
                    f(12L), f(13L), f(14L), f(15L)
                )

        let transposed (m : MatrixChoice) =
            match m with 
            | RealMatrix mat -> RealMatrix mat.Transposed
            | M22 mat ->        M22 mat.Transposed
            | M33 mat ->        M33 mat.Transposed
            | M44 mat ->        M44 mat.Transposed
            | M23 mat -> 
                let fs = mat.ToArray()
                let sizes = mat.Dim
                RealMatrix <| Matrix<float>(fs, sizes).Transposed
            | M34 mat -> 
                let fs = mat.ToArray()
                let sizes = mat.Dim
                RealMatrix <| Matrix<float>(fs, sizes).Transposed

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

        static member GenerateMatrix (d : DataDim, s : MatrixShape, k : MatrixKind<float>, t : MatrixTrafo) =
            gen {
                let r = DataDim.getRows d
                let c = DataDim.getCols d
                let arrGen = arrayGen k
                let! mat = 
                    gen {
                        match s with
                        | Full -> 
                            let! fs = arrGen (r*c)
                            return createMatrix d fs
                        | Diagonal -> 
                            let! fs = arrGen (min r c)
                            return createZeroMatrix d 
                                    |> MatrixChoice.setByCoord (fun (c : V2l) -> if c.X = c.Y then fs.[int c.X] else 0.0 )
                        | CopyCols -> 
                            let! fs = arrGen r
                            return createZeroMatrix d 
                                    |> MatrixChoice.setByCoord (fun (c : V2l) -> fs.[int c.Y])    
                        | CopyRows -> 
                            let! fs = arrGen c
                            return createZeroMatrix d 
                                    |> MatrixChoice.setByCoord (fun (c : V2l) -> fs.[int c.X])                              
                        | Triangular(upper) -> 
                            let ct =
                                match r <= c, upper with 
                                | true, true -> (r*(r+1))/2 + (c-r)*r
                                | true, false -> (r*(r+1))/2
                                | false, true -> (c*(c+1))/2
                                | false, false -> (c*(c+1))/2 + (r-c)*c

                            let! fs = arrGen ct
                            let mutable i = 0
                            return 
                                createZeroMatrix d
                                |> MatrixChoice.setByCoord(fun (c : V2l) ->
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
                            return 
                                createZeroMatrix d
                                |> MatrixChoice.setByCoord(fun (c : V2l) ->
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

                            return 
                                createZeroMatrix d
                                |> MatrixChoice.setByCoord get                       
                    }                   
                let! trafoed = 
                    gen {
                        match t with
                        | Id -> return mat
                        | FlipX -> 
                            let mo = mat |> MatrixChoice.copy
                            return mat |> MatrixChoice.setByCoord(fun (p : V2l) -> mo |> MatrixChoice.get (V2l(int64 c-1L-p.X, p.Y)))
                        | FlipY -> 
                            let mo = mat |> MatrixChoice.copy
                            return mat |> MatrixChoice.setByCoord(fun (p : V2l) -> mo |> MatrixChoice.get (V2l(p.X, int64 r-1L-p.Y)))
                        | Transposed ->
                            return mat |> MatrixChoice.transposed
                        | RandomMask ->
                            let! mask = Gen.arrayOfLength (r*c) Arb.generate<bool>
                            return mat |> MatrixChoice.setByIndex(fun i -> if mask.[int i] then mat |> MatrixChoice.geti i else 0.0)
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
                let! d = 
                    Gen.frequency [
                        5, Gen.constant (DataDim.Mat(r,c))
                        1, Gen.constant (DataDim.M22)
                        1, Gen.constant (DataDim.M23)
                        1, Gen.constant (DataDim.M33)
                        1, Gen.constant (DataDim.M34)
                        1, Gen.constant (DataDim.M44)
                    ]
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo>
                


                let! value = Arbitraries.GenerateMatrix(d, s, k,t)
                return 
                    {
                        PrettyMatrix.shape = s
                        PrettyMatrix.kind = k                    
                        PrettyMatrix.trafo = t
                        PrettyMatrix.dim = d
                        PrettyMatrix.value = value
                    }                
            } |> Arb.fromGen

        static member MatrixFloat() =
            gen {
                let! square = Gen.frequency [1, Gen.constant true; 10, Gen.constant false]      
                let! (r : int) = Gen.choose(1,25)
                let! (c : int) = if square then Gen.constant r else Gen.choose(1,25)
                let! d = 
                    Gen.frequency [
                        5, Gen.constant (DataDim.Mat(r,c))
                        1, Gen.constant (DataDim.M22)
                        1, Gen.constant (DataDim.M23)
                        1, Gen.constant (DataDim.M33)
                        1, Gen.constant (DataDim.M34)
                        1, Gen.constant (DataDim.M44)
                    ]
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo>

                let! value = Arbitraries.GenerateMatrix(d, s, k,t)
                let k =
                    match k with
                    | Zeros -> Zeros
                    | Ones -> Ones
                    | Constant v -> Constant (float32 v)
                    | Arbitrary -> Arbitrary

                return 
                    {
                        PrettyMatrix32.shape = s
                        PrettyMatrix32.kind = k                    
                        PrettyMatrix32.trafo = t
                        PrettyMatrix32.dim = d
                        PrettyMatrix32.value = MatrixChoice.to32 value
                    }                
            } |> Arb.fromGen
        
        static member WideMatrix() =
            gen {
                let! square = Gen.frequency [1, Gen.constant true; 10, Gen.constant false]      
                let! (r : int) = Gen.choose(1,25)
                let! (c : int) = if square then Gen.constant r else Gen.choose(r,26)
                let! d = 
                    Gen.frequency [
                        5, Gen.constant (DataDim.Mat(r,c))
                        1, Gen.constant (DataDim.M22)
                        1, Gen.constant (DataDim.M23)
                        1, Gen.constant (DataDim.M33)
                        1, Gen.constant (DataDim.M34)
                        1, Gen.constant (DataDim.M44)
                    ]
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo> |> Gen.filter (fun trafo -> trafo <> Transposed)

                let! value = Arbitraries.GenerateMatrix(d, s, k,t)
                return 
                    {
                        WideMatrix.shape = s
                        WideMatrix.kind = k                    
                        WideMatrix.trafo = t
                        WideMatrix.dim = d
                        WideMatrix.value = value
                    }                
            } |> Arb.fromGen

        static member WideMatrixFloat() =
            gen {
                let! square = Gen.frequency [1, Gen.constant true; 10, Gen.constant false]      
                let! (r : int) = Gen.choose(1,25)
                let! (c : int) = if square then Gen.constant r else Gen.choose(r,26)
                let! d = 
                    Gen.frequency [
                        5, Gen.constant (DataDim.Mat(r,c))
                        1, Gen.constant (DataDim.M22)
                        1, Gen.constant (DataDim.M23)
                        1, Gen.constant (DataDim.M33)
                        1, Gen.constant (DataDim.M34)
                        1, Gen.constant (DataDim.M44)
                    ]
                let! s = Arb.generate<MatrixShape>
                let! k = Arb.generate<MatrixKind<float>>
                let! t = Arb.generate<MatrixTrafo> |> Gen.filter (fun trafo -> trafo <> Transposed)


                let! value = Arbitraries.GenerateMatrix(d, s, k,t)            
            
                let k =
                    match k with
                    | Zeros -> Zeros
                    | Ones -> Ones
                    | Constant v -> Constant (float32 v)
                    | Arbitrary -> Arbitrary

                return 
                    {
                        WideMatrix32.shape = s
                        WideMatrix32.kind = k                    
                        WideMatrix32.trafo = t
                        WideMatrix32.dim = d
                        WideMatrix32.value = MatrixChoice.to32 value
                    }                             
            } |> Arb.fromGen

    let cfg : FsCheckConfig =
        { FsCheckConfig.defaultConfig with 
            maxTest = 10000
            arbitrary = [ typeof<Arbitraries> ] 
        }

    let epsilon = 1E-8
    let floatEpsilon = float32 1E-1

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
                match mat.value with
                | RealMatrix mat ->     
                    let (q,r) = QR.decompose mat
                    let res = q.Multiply(r)
                    res.ApproximateEquals(mat)
                | M22 mat ->      
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M22d.DistanceMax(res,mat) < epsilon
                | M33 mat -> 
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M33d.DistanceMax(res,mat) < epsilon
                | M44 mat -> 
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M44d.DistanceMax(res,mat) < epsilon
                | M23 mat -> 
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M23d.DistanceMax(res,mat) < epsilon
                | M34 mat ->
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M34d.DistanceMax(res,mat) < epsilon
            )

            testPropertyWithConfig cfg "[QR64] Q*Qt = ID" (fun (mat : PrettyMatrix<float> ) -> 
                match mat.value with
                | RealMatrix mat ->     
                    let (q,r) = QR.decompose mat
                    q.IsOrtho()
                | M22 mat ->      
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M22 q)).IsOrtho()
                | M33 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M33 q)).IsOrtho()
                | M44 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M44 q)).IsOrtho()
                | M23 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M22 q)).IsOrtho()
                | M34 mat ->
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M33 q)).IsOrtho()
            )

            testPropertyWithConfig cfg "[QR64] R = right upper" (fun (mat : PrettyMatrix<float> ) -> 
                match mat.value with
                | RealMatrix mat ->     
                    let (q,r) = QR.decompose mat
                    r.IsUpperRight()
                | M22 mat ->      
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M22 r)).IsUpperRight()
                | M33 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M33 r)).IsUpperRight()
                | M44 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M44 r)).IsUpperRight()
                | M23 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M23 r)).IsUpperRight()
                | M34 mat ->
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat (M34 r)).IsUpperRight()
            )
        ]    

    let rq =
        testList "[RQ64] decompose" [
            testPropertyWithConfig cfg "[RQ64] R*Q = M" (fun (mat : WideMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat ->     
                    let (r,q) = RQ.decompose mat
                    let res = r.Multiply(q)
                    res.ApproximateEquals(mat)
                | M22 mat ->      
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M22d.DistanceMax(res,mat) < epsilon
                | M33 mat -> 
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M33d.DistanceMax(res,mat) < epsilon
                | M44 mat -> 
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M44d.DistanceMax(res,mat) < epsilon
                | M23 mat -> 
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M23d.DistanceMax(res,mat) < epsilon
                | M34 mat ->
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M34d.DistanceMax(res,mat) < epsilon
            )

            testPropertyWithConfig cfg "[RQ64] Q*Qt = ID" (fun (mat : WideMatrix<float> ) -> 
                match mat.value with
                | RealMatrix mat ->     
                    let (r,q) = RQ.decompose mat
                    q.IsOrtho()
                | M22 mat ->      
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M22 q)).IsOrtho()
                | M33 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M33 q)).IsOrtho()
                | M44 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M44 q)).IsOrtho()
                | M23 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M33 q)).IsOrtho()
                | M34 mat ->
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M44 q)).IsOrtho()
            )

            testPropertyWithConfig cfg "[RQ64] R = right upper" (fun (mat : WideMatrix<float> ) -> 
                match mat.value with
                | RealMatrix mat ->     
                    let (r,q) = RQ.decompose mat
                    r.IsUpperRight()
                | M22 mat ->      
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M22 r)).IsUpperRight()
                | M33 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M33 r)).IsUpperRight()
                | M44 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M44 r)).IsUpperRight()
                | M23 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M23 r)).IsUpperRight()
                | M34 mat ->
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat (M34 r)).IsUpperRight()
            )
        ]    

    let qrBidiag = 
        testList "[QR64] bidiagonalize" [
            testPropertyWithConfig cfg "[QR64] U*Ut = ID " (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    u.IsOrtho()
                | M22 mat ->      
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M22 u)).IsOrtho()
                | M33 mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M33 u)).IsOrtho()
                | M44 mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M44 u)).IsOrtho()
                | M23 mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M22 u)).IsOrtho()
                | M34 mat ->
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M33 u)).IsOrtho()
            )

            testPropertyWithConfig cfg "[QR64] D is bidiagonal" (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    d.IsBidiagonal()
                | M22 mat ->      
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M22 d)).IsBidiagonal()
                | M33 mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M33 d)).IsBidiagonal()
                | M44 mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M44 d)).IsBidiagonal()
                | M23 mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M23 d)).IsBidiagonal()
                | M34 mat ->
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M34 d)).IsBidiagonal()
            )

            testPropertyWithConfig cfg "[QR64] V*Vt = ID" (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    vt.IsOrtho()
                | M22 mat ->      
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M22 vt)).IsOrtho()
                | M33 mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M33 vt)).IsOrtho()
                | M44 mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M44 vt)).IsOrtho()
                | M23 mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M33 vt)).IsOrtho()
                | M34 mat ->
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat (M44 vt)).IsOrtho()
            )        

            testPropertyWithConfig cfg "[QR64] U*D*Vt = M" (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u.Multiply(d.Multiply(vt))
                    res.ApproximateEquals(mat)
                | M22 mat ->      
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat (M22 res)).ApproximateEquals(MatrixChoice.toRealMat (M22 mat))
                | M33 mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat (M33 res)).ApproximateEquals(MatrixChoice.toRealMat (M33 mat))
                | M44 mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat (M44 res)).ApproximateEquals(MatrixChoice.toRealMat (M44 mat))
                | M23 mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat (M23 res)).ApproximateEquals(MatrixChoice.toRealMat (M23 mat))
                | M34 mat ->
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat (M34 res)).ApproximateEquals(MatrixChoice.toRealMat (M34 mat))
            )
        ]    

    let svd = 
        testList "[SVD64] decompose" [
            testPropertyWithConfig cfg "[SVD64] U*Ut = ID " (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> u.IsOrtho())
                    |> Option.defaultValue true
                | M22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat (M22 u)).IsOrtho())
                    |> Option.defaultValue true
                | M33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat (M33 u)).IsOrtho())
                    |> Option.defaultValue true
                | M44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat (M44 u)).IsOrtho())
                    |> Option.defaultValue true
                | M23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat (M22 u)).IsOrtho())
                    |> Option.defaultValue true
                | M34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat (M33 u)).IsOrtho())
                    |> Option.defaultValue true
            )

            testPropertyWithConfig cfg "[SVD64] S is diagonal" (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> s.IsDiagonal())
                    |> Option.defaultValue true
                | M22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M22 s)).IsDiagonal())
                    |> Option.defaultValue true
                | M33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M33 s)).IsDiagonal())
                    |> Option.defaultValue true
                | M44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M44 s)).IsDiagonal())
                    |> Option.defaultValue true
                | M23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M23 s)).IsDiagonal())
                    |> Option.defaultValue true
                | M34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M34 s)).IsDiagonal())
                    |> Option.defaultValue true
            )
        
            testPropertyWithConfig cfg "[SVD64] S is decreasing" (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> s.DecreasingDiagonal())
                    |> Option.defaultValue true
                | M22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M22 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | M33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M33 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | M44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M44 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | M23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M23 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | M34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat (M34 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
            )

            testPropertyWithConfig cfg "[SVD64] V*Vt = ID" (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> vt.IsOrtho())
                    |> Option.defaultValue true
                | M22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat (M22 vt)).IsOrtho())
                    |> Option.defaultValue true
                | M33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat (M33 vt)).IsOrtho())
                    |> Option.defaultValue true
                | M44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat (M44 vt)).IsOrtho())
                    |> Option.defaultValue true
                | M23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat (M33 vt)).IsOrtho())
                    |> Option.defaultValue true
                | M34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat (M44 vt)).IsOrtho())
                    |> Option.defaultValue true
            )        

            testPropertyWithConfig cfg "[SVD64] U*S*Vt = M" (fun (mat : PrettyMatrix<float>) -> 
                match mat.value with
                | RealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u.Multiply(s.Multiply(vt)) in res.ApproximateEquals(mat))
                    |> Option.defaultValue true
                | M22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat (M22 res)).ApproximateEquals(MatrixChoice.toRealMat (M22 mat)))
                    |> Option.defaultValue true
                | M33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat (M33 res)).ApproximateEquals(MatrixChoice.toRealMat (M33 mat)))
                    |> Option.defaultValue true
                | M44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat (M44 res)).ApproximateEquals(MatrixChoice.toRealMat (M44 mat)))
                    |> Option.defaultValue true
                | M23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat (M23 res)).ApproximateEquals(MatrixChoice.toRealMat (M23 mat)))
                    |> Option.defaultValue true
                | M34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat (M34 res)).ApproximateEquals(MatrixChoice.toRealMat (M34 mat)))
                    |> Option.defaultValue true
            )
        ]   
  
    let qr32 =
        testList "[QR32] decompose" [
            testPropertyWithConfig cfg "[QR32] Q*R = M" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat ->     
                    let (q,r) = QR.decompose mat
                    let res = q.Multiply(r)
                    res.ApproximateEquals(mat)
                | FM22 mat ->      
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M22f.DistanceMax(res,mat) < floatEpsilon
                | FM33 mat -> 
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M33f.DistanceMax(res,mat) < floatEpsilon
                | FM44 mat -> 
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M44f.DistanceMax(res,mat) < floatEpsilon
                | FM23 mat -> 
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M23f.DistanceMax(res,mat) < floatEpsilon
                | FM34 mat ->
                    let (q,r) = QR.decompose mat
                    let res = q * r
                    M34f.DistanceMax(res,mat) < floatEpsilon
            )

            testPropertyWithConfig cfg "[QR32] Q*Qt = ID" (fun (mat : PrettyMatrix32<float32> ) -> 
                match mat.value with
                | FRealMatrix mat ->     
                    let (q,r) = QR.decompose mat
                    q.IsOrtho()
                | FM22 mat ->      
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM22 q)).IsOrtho()
                | FM33 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM33 q)).IsOrtho()
                | FM44 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM44 q)).IsOrtho()
                | FM23 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM22 q)).IsOrtho()
                | FM34 mat ->
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM33 q)).IsOrtho()
            )

            testPropertyWithConfig cfg "[QR32] R = right upper" (fun (mat : PrettyMatrix32<float32> ) -> 
                match mat.value with
                | FRealMatrix mat ->     
                    let (q,r) = QR.decompose mat
                    r.IsUpperRight()
                | FM22 mat ->      
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM22 r)).IsUpperRight()
                | FM33 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM33 r)).IsUpperRight()
                | FM44 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM44 r)).IsUpperRight()
                | FM23 mat -> 
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM23 r)).IsUpperRight()
                | FM34 mat ->
                    let (q,r) = QR.decompose mat
                    (MatrixChoice.toRealMat32 (FM34 r)).IsUpperRight()
            )
        ]    

    let rq32 =
        testList "[RQ32] decompose" [
            testPropertyWithConfig cfg "[RQ32] R*Q = M" (fun (mat : WideMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat ->   
                    let (r,q) = RQ.decompose mat
                    let res = r.Multiply(q)
                    res.ApproximateEquals(mat)
                | FM22 mat ->      
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M22f.DistanceMax(res,mat) < floatEpsilon
                | FM33 mat -> 
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M33f.DistanceMax(res,mat) < floatEpsilon
                | FM44 mat -> 
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M44f.DistanceMax(res,mat) < floatEpsilon
                | FM23 mat -> 
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M23f.DistanceMax(res,mat) < floatEpsilon
                | FM34 mat ->
                    let (r,q) = RQ.decompose mat
                    let res = r * q
                    M34f.DistanceMax(res,mat) < floatEpsilon
            )

            testPropertyWithConfig cfg "[RQ32] Q*Qt = ID" (fun (mat : WideMatrix32<float32> ) -> 
                match mat.value with
                | FRealMatrix mat ->     
                    let (r,q) = RQ.decompose mat
                    q.IsOrtho()
                | FM22 mat ->      
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM22 q)).IsOrtho()
                | FM33 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM33 q)).IsOrtho()
                | FM44 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM44 q)).IsOrtho()
                | FM23 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM33 q)).IsOrtho()
                | FM34 mat ->
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM44 q)).IsOrtho()
            )

            testPropertyWithConfig cfg "[RQ32] R = right upper" (fun (mat : WideMatrix32<float32> ) -> 
                match mat.value with
                | FRealMatrix mat ->     
                    let (r,q) = RQ.decompose mat
                    r.IsUpperRight()
                | FM22 mat ->      
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM22 r)).IsUpperRight()
                | FM33 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM33 r)).IsUpperRight()
                | FM44 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM44 r)).IsUpperRight()
                | FM23 mat -> 
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM23 r)).IsUpperRight()
                | FM34 mat ->
                    let (r,q) = RQ.decompose mat
                    (MatrixChoice.toRealMat32 (FM34 r)).IsUpperRight()
            )
        ]

    let qrBidiag32 = 
        testList "[QR32] Bidiagonalize" [
            testPropertyWithConfig cfg "[QR32] U*Ut = ID " (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    u.IsOrtho()
                | FM22 mat ->      
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM22 u)).IsOrtho()
                | FM33 mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM33 u)).IsOrtho()
                | FM44 mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM44 u)).IsOrtho()
                | FM23 mat -> 
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM22 u)).IsOrtho()
                | FM34 mat ->
                    let (u,_,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM33 u)).IsOrtho()
            )

            testPropertyWithConfig cfg "[QR32] D is bidiagonal" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    d.IsBidiagonal()
                | FM22 mat ->      
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM22 d)).IsBidiagonal()
                | FM33 mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM33 d)).IsBidiagonal()
                | FM44 mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM44 d)).IsBidiagonal()
                | FM23 mat -> 
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM23 d)).IsBidiagonal()
                | FM34 mat ->
                    let (_,d,_) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM34 d)).IsBidiagonal()
            )

            testPropertyWithConfig cfg "[QR32] V*Vt = ID" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    vt.IsOrtho()
                | FM22 mat ->      
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM22 vt)).IsOrtho()
                | FM33 mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM33 vt)).IsOrtho()
                | FM44 mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM44 vt)).IsOrtho()
                | FM23 mat -> 
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM33 vt)).IsOrtho()
                | FM34 mat ->
                    let (_,_,vt) = QR.Bidiagonalize mat
                    (MatrixChoice.toRealMat32 (FM44 vt)).IsOrtho()
            )        

            testPropertyWithConfig cfg "[QR32] U*D*Vt = M" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u.Multiply(d.Multiply(vt))
                    res.ApproximateEquals(mat)
                | FM22 mat ->      
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat32 (FM22 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM22 mat))
                | FM33 mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat32 (FM33 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM33 mat))
                | FM44 mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat32 (FM44 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM44 mat))
                | FM23 mat -> 
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat32 (FM23 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM23 mat))
                | FM34 mat ->
                    let (u,d,vt) = QR.Bidiagonalize mat
                    let res = u * d * vt
                    (MatrixChoice.toRealMat32 (FM34 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM34 mat))
            )
        ]   

    let svd32 = 
        testList "[SVD32] decompose" [
            testPropertyWithConfig cfg "[SVD32] U*Ut = ID " (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> u.IsOrtho())
                    |> Option.defaultValue true
                | FM22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat32 (FM22 u)).IsOrtho())
                    |> Option.defaultValue true
                | FM33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat32 (FM33 u)).IsOrtho())
                    |> Option.defaultValue true
                | FM44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat32 (FM44 u)).IsOrtho())
                    |> Option.defaultValue true
                | FM23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat32 (FM22 u)).IsOrtho())
                    |> Option.defaultValue true
                | FM34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (u,_,_) -> (MatrixChoice.toRealMat32 (FM33 u)).IsOrtho())
                    |> Option.defaultValue true
            )

            testPropertyWithConfig cfg "[SVD32] S is diagonal" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> s.IsDiagonal())
                    |> Option.defaultValue true
                | FM22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM22 s)).IsDiagonal())
                    |> Option.defaultValue true
                | FM33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM33 s)).IsDiagonal())
                    |> Option.defaultValue true
                | FM44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM44 s)).IsDiagonal())
                    |> Option.defaultValue true
                | FM23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM23 s)).IsDiagonal())
                    |> Option.defaultValue true
                | FM34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM34 s)).IsDiagonal())
                    |> Option.defaultValue true
            )
        
            testPropertyWithConfig cfg "[SVD32] S is decreasing" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> s.DecreasingDiagonal())
                    |> Option.defaultValue true
                | FM22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM22 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | FM33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM33 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | FM44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM44 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | FM23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM23 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
                | FM34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (_,s,_) -> (MatrixChoice.toRealMat32 (FM34 s)).DecreasingDiagonal())
                    |> Option.defaultValue true
            )

            testPropertyWithConfig cfg "[SVD32] V*Vt = ID" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> vt.IsOrtho())
                    |> Option.defaultValue true
                | FM22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat32 (FM22 vt)).IsOrtho())
                    |> Option.defaultValue true
                | FM33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat32 (FM33 vt)).IsOrtho())
                    |> Option.defaultValue true
                | FM44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat32 (FM44 vt)).IsOrtho())
                    |> Option.defaultValue true
                | FM23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat32 (FM33 vt)).IsOrtho())
                    |> Option.defaultValue true
                | FM34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (_,_,vt) -> (MatrixChoice.toRealMat32 (FM44 vt)).IsOrtho())
                    |> Option.defaultValue true
            )        

            testPropertyWithConfig cfg "[SVD32] U*S*Vt = M" (fun (mat : PrettyMatrix32<float32>) -> 
                match mat.value with
                | FRealMatrix mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u.Multiply(s.Multiply(vt)) in res.ApproximateEquals(mat))
                    |> Option.defaultValue true
                | FM22 mat ->      
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat32 (FM22 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM22 mat)))
                    |> Option.defaultValue true
                | FM33 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat32 (FM33 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM33 mat)))
                    |> Option.defaultValue true
                | FM44 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat32 (FM44 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM44 mat)))
                    |> Option.defaultValue true
                | FM23 mat -> 
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat32 (FM23 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM23 mat)))
                    |> Option.defaultValue true
                | FM34 mat ->
                    SVD.decompose mat 
                    |> Option.map (fun (u,s,vt) -> let res = u * s * vt in (MatrixChoice.toRealMat32 (FM34 res)).ApproximateEquals(MatrixChoice.toRealMat32 (FM34 mat)))
                    |> Option.defaultValue true
            )
        ]   

 
    [<Tests>]
    let all =
        testList "QR and SVD" [
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
                                if not (Fun.ApproximateEquals(test, mat, 1e-2f)) then
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