namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open FsUnit
open NUnit.Framework

module ``Tensor Math Tests`` =

    let equalApprox exp = equalWithin 0.0001 exp

    module Vector =

        module Ref =

            let inline dot (a: Vector< ^T>) (b: Vector< ^T>) =
                a.InnerProduct(b, (*), LanguagePrimitives.GenericZero< ^T>, (+))

        let inline approxEqual (a: Vector<'T>) (b: Vector<'T>) =
            if a.S <> b.S then
                invalidArg "size" "Mismatching size"

            a.ForeachIndex(b.Info, fun i1 i2 ->
                a.Data.[int i1] |> should equalApprox b.Data.[int i2]
            )

    module Matrix =

        module Ref =

            let map2 (f: 'T1 -> 'T2 -> 'T3) (a: Matrix<'T1>) (b: Matrix<'T2>) =
                if a.S <> b.S then
                    invalidArg "size" "Size mismatch"

                let c = Array.zeroCreate a.Data.Length
                a.ForeachIndex(b.Info, fun i1 i2 ->
                    c.[int i1] <- f a.Data.[int i1] b.Data.[int i2]
                )
                Matrix<'T3>(c, a.Origin, a.S, a.D, a.F)

        let inline approxEqual (a: Matrix<'T>) (b: Matrix<'T>) =
            if a.S <> b.S then
                invalidArg "size" "Mismatching size"

            a.ForeachIndex(b.Info, fun i1 i2 ->
                a.Data.[int i1] |> should equalApprox b.Data.[int i2]
            )

    let iterations = 100
    let oldImpl = false

    [<SetUp>]
    let setup() =
        TensorGeneration.init 0

    [<Theory>]
    let ``[Vector] Dot product`` (tensor: TensorKind) =
        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.DotProduct(b) else Vector.dot a b
            r |> should equalApprox (Vector.Ref.dot a b)

    [<Theory>]
    let ``[Vector] Norm2 squared`` (tensor: TensorKind) =
        let inline ref (v: Vector< ^T>) =
            v.Norm(sqr, LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let v = TensorGeneration.getVector tensor
            let r = if oldImpl then v.NormSquared() else Vector.lengthSquared v
            r |> should equalApprox (ref v)

    [<Theory>]
    let ``[Vector] Dist1`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            a.InnerProduct(b, (fun x y -> abs (x - y)), LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.Dist1(b) else Vector.distance1 a b
            r |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] Dist2`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            sqrt <| a.InnerProduct(b, (fun x y -> sqr (x - y)), LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.Dist2(b) else Vector.distance a b
            r |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] Dist2 squared`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            a.InnerProduct(b, (fun x y -> sqr (x - y)), LanguagePrimitives.GenericZero< ^T>, (+))

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.Dist2Squared(b) else Vector.distanceSquared a b
            r |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] DistMax`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            a.InnerProduct(b, (fun x y -> abs (x - y)), LanguagePrimitives.GenericZero< ^T>, max)

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.DistMax(b) else Vector.distanceMax a b
            r |> should equalApprox (ref a b)

    [<Theory>]
    let ``[Vector] Multiply`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            let a = Array.ofSeq a.Elements
            let b = Array.ofSeq b.Elements
            (a, b) ||> Array.map2 (*) |> Vector.Create

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.Multiply(b) else Vector.multiply a b
            Vector.approxEqual r (ref a b)

    [<Theory>]
    let ``[Vector] Multiply transposed`` (tensor: TensorKind) =
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
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.MultiplyTransposed(b) else Vector.multiplyTransposed a b
            Matrix.approxEqual r (ref a b)

    [<Theory>]
    let ``[Vector] Multiply scalar`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: ^T) =
            a.Elements |> Array.ofSeq |> Array.map ((*) b) |> Vector.Create

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.rnd.UniformDouble() * 100.0
            let r = if oldImpl then a.Multiply(b) else Vector.map ((*) b) a
            Vector.approxEqual r (ref a b)

    [<Theory>]
    let ``[Vector] Subtract`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            let a = Array.ofSeq a.Elements
            let b = Array.ofSeq b.Elements
            (a, b) ||> Array.map2 (-) |> Vector.Create

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.Subtract(b) else Vector.subtract a b
            Vector.approxEqual r (ref a b)

    [<Theory>]
    let ``[Vector] Subtract scalar`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: ^T) =
            a.Elements |> Array.ofSeq |> Array.map (fun x -> x - b) |> Vector.Create

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.rnd.UniformDouble() * 100.0
            let r = if oldImpl then a.Subtract(b) else Vector.map (fun a -> a - b) a
            Vector.approxEqual r (ref a b)

    [<Theory>]
    let ``[Vector] Add`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: Vector< ^T>) =
            let a = Array.ofSeq a.Elements
            let b = Array.ofSeq b.Elements
            (a, b) ||> Array.map2 (+) |> Vector.Create

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.getVectorOfSize tensor a.Size
            let r = if oldImpl then a.Add(b) else Vector.add a b
            Vector.approxEqual r (ref a b)

    [<Theory>]
    let ``[Vector] Add scalar`` (tensor: TensorKind) =
        let inline ref (a: Vector< ^T>) (b: ^T) =
            a.Elements |> Array.ofSeq |> Array.map ((+) b) |> Vector.Create

        for _ = 1 to iterations do
            let a = TensorGeneration.getVector tensor
            let b = TensorGeneration.rnd.UniformDouble() * 100.0
            let r = if oldImpl then a.Add(b) else Vector.map ((+) b) a
            Vector.approxEqual r (ref a b)

    [<Theory>]
    let ``[Matrix] Transform`` (tensor: TensorKind) (rowMajor: bool) =
        let inline ref (a: Matrix< ^T>) (b: Vector< ^T>) =
            if a.SX <> b.S then
                invalidArg "size" "Size mismatch"

            Array.init (int a.SY) (fun i ->
                Vector.Ref.dot (a.Row (a.FY + int64 i)) b
            )
            |> Vector.Create

        for _ = 1 to iterations do
            let a = TensorGeneration.getMatrix tensor rowMajor
            let b = TensorGeneration.getVectorOfSize tensor a.SX
            let r = if oldImpl then a.Multiply(b) else Matrix.transform a b
            Vector.approxEqual r (ref a b)

    [<Theory>]
    let ``[Matrix] Multiply`` (tensor: TensorKind) (rowMajorLeft: bool) (rowMajorRight: bool) =
        let inline ref (a: Matrix< ^T>) (b: Matrix< ^T>) =
            if a.SX <> b.SY then
                invalidArg "size" "Size mismatch"

            let r = Array.zeroCreate (int (b.SX * a.SY))
            for y = 0 to int a.SY - 1 do
                for x = 0 to int b.SX - 1 do
                    r.[y * int b.SX + x] <- Vector.Ref.dot (a.Row(a.FY + int64 y)) (b.Col(b.FX + int64 x))

            Matrix(r, V2l(b.SX, a.SY))

        for _ = 1 to iterations do
            let a = TensorGeneration.getMatrix tensor rowMajorLeft
            let b = TensorGeneration.getMatrixOfHeight tensor rowMajorRight a.SX
            let r = if oldImpl then a.Multiply(b) else Matrix.multiply a b
            Matrix.approxEqual r (ref a b)

    [<Theory>]
    let ``[Matrix] Subtract`` (tensor: TensorKind) (rowMajorLeft: bool) (rowMajorRight: bool) =
        let inline ref (a: Matrix< ^T>) (b: Matrix< ^T>) =
            Matrix.Ref.map2 (-) a b

        for _ = 1 to iterations do
            let a = TensorGeneration.getMatrix tensor rowMajorLeft
            let b = TensorGeneration.getMatrixOfSize tensor rowMajorRight a.S
            let r = if oldImpl then a.Subtract(b) else Matrix.subtract a b
            Matrix.approxEqual r (ref a b)

    [<Theory>]
    let ``[Matrix] Subtract scalar`` (tensor: TensorKind) (rowMajor: bool) =
        let inline ref (a: Matrix< ^T>) (b: ^T) =
            a.Map(fun x -> x - b)

        for _ = 1 to iterations do
            let a = TensorGeneration.getMatrix tensor rowMajor
            let b = TensorGeneration.rnd.UniformDouble() * 100.0
            let r = if oldImpl then a.Subtract(b) else Matrix.map (fun a -> a - b) a
            Matrix.approxEqual r (ref a b)

    [<Theory>]
    let ``[Matrix] Add`` (tensor: TensorKind) (rowMajorLeft: bool) (rowMajorRight: bool) =
        let inline ref (a: Matrix< ^T>) (b: Matrix< ^T>) =
            Matrix.Ref.map2 (+) a b

        for _ = 1 to iterations do
            let a = TensorGeneration.getMatrix tensor rowMajorLeft
            let b = TensorGeneration.getMatrixOfSize tensor rowMajorRight a.S
            let r = if oldImpl then a.Add(b) else Matrix.add a b
            Matrix.approxEqual r (ref a b)

    [<Theory>]
    let ``[Matrix] Add scalar`` (tensor: TensorKind) (rowMajor: bool) =
        let inline ref (a: Matrix< ^T>) (b: ^T) =
            a.Map((+) b)

        for _ = 1 to iterations do
            let a = TensorGeneration.getMatrix tensor rowMajor
            let b = TensorGeneration.rnd.UniformDouble() * 100.0
            let r = if oldImpl then a.Add(b) else Matrix.map ((+) b) a
            Matrix.approxEqual r (ref a b)