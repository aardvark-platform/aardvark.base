namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open FsUnit
open NUnit.Framework

module ``Tensor Math Tests`` =

    type Tensor =
        | Full = 0
        | Sub = 1
        | Window = 2

    let mutable rnd = Unchecked.defaultof<RandomSystem>
    let equalApprox exp = equalWithin 0.0001 exp

    module Vector =

        module Ref =

            let inline dot (a: Vector< ^T>) (b: Vector< ^T>) =
                a.InnerProduct(b, (*), LanguagePrimitives.GenericZero< ^T>, (+))

        let inline approxEqual (a: Vector<'T>) (b: Vector<'T>) =
            if a.S <> b.S then
                invalidArg "size" "Mismatching size"

            (a.Elements, b.Elements)
            ||> Seq.iter2 (fun a b -> a |> should equalApprox b)

    module Matrix =

        let inline approxEqual (a: Matrix<'T>) (b: Matrix<'T>) =
            if a.S <> b.S then
                invalidArg "size" "Mismatching size"

            (a.Elements, b.Elements)
            ||> Seq.iter2 (fun a b -> a |> should equalApprox b)

    [<Literal>]
    let iterations = 100

    [<AutoOpen>]
    module Generation =
        let inline getVectorOfSize (t: Tensor) (n: ^Int) =
            let n = int n

            match t with
            | Tensor.Full ->
                Vector.Create <| Array.init n (ignore >> rnd.UniformDouble)

            | Tensor.Sub ->
                let arr = Array.init (n + 512) (ignore >> rnd.UniformDouble)
                let d = 1 + rnd.UniformInt 4
                let f = rnd.UniformInt (arr.Length - (n - 1) * d)
                Vector.Create(arr).SubVector(f, n, d)

            | _ ->
                let arr = Array.init (n + 512) (ignore >> rnd.UniformDouble)
                let d = 1 + rnd.UniformInt 4
                let f = rnd.UniformInt ((arr.Length - (n - 1) * d) / d)
                Vector.Create(arr).SubVectorWindow(f, n, d)

        let getVector (t: Tensor) =
            getVectorOfSize t (32 + rnd.UniformInt 128)

        let inline getMatrixOfSize (t: Tensor) (s: ^V2i) = 
            let s = v2i s

            match t with
            | Tensor.Full ->
                let arr = Array.init (s.X * s.Y) (ignore >> rnd.UniformDouble)
                Matrix(arr, s)

            | Tensor.Sub ->
                let S = s + 256
                let arr = Array.init (S.X * S.Y) (ignore >> rnd.UniformDouble)
                let d = 1 + rnd.UniformV2i 4
                let f = rnd.UniformV2i (S - (s - 1) * d)
                Matrix(arr, S).SubMatrix(f, s, V2i(d.X, d.Y * S.X))

            | _ ->
                let S = s + 256
                let arr = Array.init (S.X * S.Y) (ignore >> rnd.UniformDouble)
                let d = 1 + rnd.UniformV2i 4
                let f = rnd.UniformV2i ((S - (s - 1) * d) / d)
                Matrix(arr, S).SubMatrixWindow(f, s, V2i(d.X, d.Y * S.X))

        let getMatrix (t: Tensor) =
            getMatrixOfSize t (16 + rnd.UniformV2i 64)

        let inline getMatrixOfHeight (t: Tensor) (h: ^Int) =
            getMatrixOfSize t (V2i(16 + rnd.UniformInt 64, int h))

    [<SetUp>]
    let setup() =
        rnd <- RandomSystem(0)

    [<Theory>]
    let ``[Vector] Dot product`` (tensor: Tensor) =
        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            a.DotProduct(b) |> should equalApprox (Vector.Ref.dot a b)

    [<Theory>]
    let ``[Vector] Norm2 squared`` (tensor: Tensor) =
        let inline ref (v: Vector< ^T>) =
            v.Norm(sqr, LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let v = getVector tensor
            v.NormSquared() |> should equalApprox (ref v)

    [<Theory>]
    let ``[Vector] Dist1`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            a.InnerProduct(b, (fun x y -> abs (x - y)), LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            a.Dist1(b) |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] Dist2`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            sqrt <| a.InnerProduct(b, (fun x y -> sqr (x - y)), LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            a.Dist2(b) |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] Dist2 squared`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            a.InnerProduct(b, (fun x y -> sqr (x - y)), LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            a.Dist2Squared(b) |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] DistMax`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            a.InnerProduct(b, (fun x y -> abs (x - y)), LanguagePrimitives.GenericZero< ^T>, max)

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            a.DistMax(b) |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] Multiply`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            let a = Array.ofSeq a.Elements
            let b = Array.ofSeq b.Elements
            (a, b) ||> Array.map2 (*) |> Vector.Create

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            Vector.approxEqual (a.Multiply(b)) (ref a b)

    [<Theory>]
    let ``[Vector] Multiply transposed`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            let a = Array.ofSeq a.Elements
            let b = Array.ofSeq b.Elements
            let r = Array.zeroCreate (a.Length * b.Length)

            let mutable i = 0
            for y = 0 to a.Length - 1 do
                for x = 0 to b.Length - 1 do
                    r.[i] <- a.[y] * b.[x]
                    inc &i

            Matrix.Create(r, b.Length, a.Length)

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            Matrix.approxEqual (a.MultiplyTransposed(b)) (ref a b)

    [<Theory>]
    let ``[Vector] Multiply scalar`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: ^T) =
            a.Elements |> Array.ofSeq |> Array.map ((*) b) |> Vector.Create

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = rnd.UniformDouble() * 100.0
            Vector.approxEqual (a.Multiply(b)) (ref a b)

    [<Theory>]
    let ``[Vector] Subtract`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            let a = Array.ofSeq a.Elements
            let b = Array.ofSeq b.Elements
            (a, b) ||> Array.map2 (-) |> Vector.Create

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            Vector.approxEqual (a.Subtract(b)) (ref a b)

    [<Theory>]
    let ``[Vector] Subtract scalar`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: ^T) =
            a.Elements |> Array.ofSeq |> Array.map (fun x -> x - b) |> Vector.Create

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = rnd.UniformDouble() * 100.0
            Vector.approxEqual (a.Subtract(b)) (ref a b)

    [<Theory>]
    let ``[Vector] Add`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            let a = Array.ofSeq a.Elements
            let b = Array.ofSeq b.Elements
            (a, b) ||> Array.map2 (+) |> Vector.Create

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = getVectorOfSize tensor a.Size
            Vector.approxEqual (a.Add(b)) (ref a b)

    [<Theory>]
    let ``[Vector] Add scalar`` (tensor: Tensor) =
        let inline ref (a: Vector< ^T>) (b: ^T) =
            a.Elements |> Array.ofSeq |> Array.map ((+) b) |> Vector.Create

        for _ = 1 to iterations do
            let a = getVector tensor
            let b = rnd.UniformDouble() * 100.0
            Vector.approxEqual (a.Add(b)) (ref a b)

    [<Theory>]
    let ``[Matrix] Transform`` (tensor: Tensor) =
        let inline ref (a: Matrix< ^T>) (b: Vector< ^T>) =
            if a.SX <> b.S then
                invalidArg "size" "Size mismatch"

            Array.init (int a.SY) (fun i ->
                Vector.Ref.dot (a.Row (int64 i)) b
            )
            |> Vector.Create

        for _ = 1 to iterations do
            let a = getMatrix tensor
            let b = getVectorOfSize tensor a.SX
            Vector.approxEqual (a.Multiply(b)) (ref a b)

    [<Theory>]
    let ``[Matrix] Multiply`` (tensor: Tensor) =
        let inline ref (a: Matrix< ^T>) (b: Matrix< ^T>) =
            if a.SX <> b.SY then
                invalidArg "size" "Size mismatch"

            let r = Array.zeroCreate (int (b.SX * a.SY))
            for y = 0 to int a.SY - 1 do
                for x = 0 to int b.SX - 1 do
                    r.[y * int b.SX + x] <- Vector.Ref.dot (a.Row(int64 y)) (b.Col(int64 x))

            Matrix(r, V2l(b.SX, a.SY))

        for _ = 1 to iterations do
            let a = getMatrix tensor
            let b = getMatrixOfHeight tensor a.SX
            Matrix.approxEqual (a.Multiply(b)) (ref a b)

    [<Theory>]
    let ``[Matrix] Subtract`` (tensor: Tensor) =
        let inline ref (a: Matrix< ^T>) (b: Matrix< ^T>) =
            if a.S <> b.S then
                invalidArg "size" "Size mismatch"

            let r = (a.Elements, b.Elements) ||> Seq.map2 (-) |> Array.ofSeq
            Matrix(r, a.S)

        for _ = 1 to iterations do
            let a = getMatrix tensor
            let b = getMatrixOfSize tensor a.S
            Matrix.approxEqual (a.Subtract(b)) (ref a b)

    [<Theory>]
    let ``[Matrix] Subtract scalar`` (tensor: Tensor) =
        let inline ref (a: Matrix< ^T>) (b: ^T) =
            let r = a.Elements |> Seq.map (fun x -> x - b) |> Array.ofSeq
            Matrix(r, a.S)

        for _ = 1 to iterations do
            let a = getMatrix tensor
            let b = rnd.UniformDouble() * 100.0
            Matrix.approxEqual (a.Subtract(b)) (ref a b)

    [<Theory>]
    let ``[Matrix] Add`` (tensor: Tensor) =
        let inline ref (a: Matrix< ^T>) (b: Matrix< ^T>) =
            if a.S <> b.S then
                invalidArg "size" "Size mismatch"

            let r = (a.Elements, b.Elements) ||> Seq.map2 (+) |> Array.ofSeq
            Matrix(r, a.S)

        for _ = 1 to iterations do
            let a = getMatrix tensor
            let b = getMatrixOfSize tensor a.S
            Matrix.approxEqual (a.Add(b)) (ref a b)

    [<Theory>]
    let ``[Matrix] Add scalar`` (tensor: Tensor) =
        let inline ref (a: Matrix< ^T>) (b: ^T) =
            let r = a.Elements |> Seq.map ((+) b) |> Array.ofSeq
            Matrix(r, a.S)

        for _ = 1 to iterations do
            let a = getMatrix tensor
            let b = rnd.UniformDouble() * 100.0
            Matrix.approxEqual (a.Add(b)) (ref a b)