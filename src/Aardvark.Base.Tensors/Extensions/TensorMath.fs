namespace Aardvark.Base

open System

[<AutoOpen>]
module FSharpTensorMath =

    module Vector =

        /// Computes the dot product of two vectors.
        let inline dot (v1: Vector<'T>) (v2: Vector<'T>) : 'U =
            Vector.mapReduce2 (*) (+) v1 v2

        /// Computes the squared Euclidean distance between two vectors.
        let inline distanceSquared (v1: Vector<'T>) (v2: Vector<'T>) : 'U =
            Vector.mapReduce2 (fun x y -> sqr (x - y)) (+) v1 v2

        /// Computes the Euclidean distance between two vectors.
        let inline distance (v1: Vector<'T>) (v2: Vector<'T>) : 'U =
            sqrt (distanceSquared v1 v2)

        /// Computes the Manhatten (or 1-) distance between two vectors.
        let inline distance1 (v1: Vector<'T>) (v2: Vector<'T>) : 'U =
            Vector.mapReduce2 (fun x y -> if x > y then x - y else y - x) (+) v1 v2

        /// Computes the minimum distance between two vectors.
        let inline distanceMin (v1: Vector<'T>) (v2: Vector<'T>) : 'U =
            Vector.mapReduce2 (fun x y -> if x > y then x - y else y - x) min v1 v2

        /// Computes the maximum distance between two vectors.
        let inline distanceMax (v1: Vector<'T>) (v2: Vector<'T>) : 'U =
            Vector.mapReduce2 (fun x y -> if x > y then x - y else y - x) max v1 v2

        /// Computes the squared length of a vector.
        let inline lengthSquared (v: Vector<'T>) : 'U =
            Vector.mapReduce sqr (+) v

        /// Computes the length of a vector.
        let inline length (v: Vector<'T>) : 'U =
            sqrt (lengthSquared v)

        /// Computes the Manhatten (or 1-) norm of a vector.
        let inline norm1 (v: Vector<'T>) : 'T =
            Vector.mapReduce (fun x -> if x < LanguagePrimitives.GenericZero<'T> then -x else x) (+) v

        /// Computes the minimum norm of a vector.
        let inline normMin (v: Vector<'T>) : 'T =
            Vector.mapReduce (fun x -> if x < LanguagePrimitives.GenericZero<'T> then -x else x) min v

        /// Computes the maximum norm of a vector.
        let inline normMax (v: Vector<'T>) : 'T =
            Vector.mapReduce (fun x -> if x < LanguagePrimitives.GenericZero<'T> then -x else x) max v

        /// Computes the component-wise sum of two vectors.
        let inline add (v1: Vector<'T1>) (v2: Vector<'T2>) : Vector<'T3> =
            Vector.map2 (+) v1 v2

        /// Computes the component-wise difference of two vectors.
        let inline subtract (v1: Vector<'T1>) (v2: Vector<'T2>) : Vector<'T3> =
            Vector.map2 (-) v1 v2

        /// Computes the component-wise product of two vectors.
        let inline multiply (v1: Vector<'T1>) (v2: Vector<'T2>) : Vector<'T3> =
            Vector.map2 (*) v1 v2

        /// Computes the component-wise fraction of two vectors.
        let inline divide (v1: Vector<'T1>) (v2: Vector<'T2>) : Vector<'T3> =
            Vector.map2 (/) v1 v2

        /// Computes the matrix product of a column vector and a row vector.
        let inline multiplyTransposed (v1: Vector<'T1>) (v2: Vector<'T2>) : Matrix<'T3> =
            let data = Array.zeroCreate (int v1.Size * int v2.Size)
            let mutable j = 0

            // Manually inline the inner loop to avoid unnecessary recomputations
            let f1 = v2.FirstIndex
            let x1e = f1 + v2.DSX
            let d1 = v2.Data
            let xj1 = v2.JX

            v1 |> Vector.iter (fun x ->
                let mutable i1 = f1

                while i1 <> x1e do
                    data.[j] <- x * d1.[int i1]
                    j <- j + 1
                    i1 <- i1 + xj1
            )

            Matrix<'T3>(data, v2.Size, v1.Size)

    module Matrix =

        /// Computes the component-wise sum of two matrices.
        let inline add (v1: Matrix<'T1>) (v2: Matrix<'T2>) : Matrix<'T3> =
            Matrix.map2 (+) v1 v2

        /// Computes the component-wise difference of two matrices.
        let inline subtract (v1: Matrix<'T1>) (v2: Matrix<'T2>) : Matrix<'T3> =
            Matrix.map2 (-) v1 v2

        /// Computes the component-wise fraction of two matrices.
        let inline divide (v1: Matrix<'T1>) (v2: Matrix<'T2>) : Matrix<'T3> =
            Matrix.map2 (/) v1 v2

        /// Transforms a column vector by a matrix, i.e. computes m x v.
        let inline transform (m: Matrix<'T1>) (v: Vector<'T2>) : Vector<'T3> =
            if m.SX <> v.Size then
                raise <| ArgumentException($"Cannot multiply matrix of size {m.Size} with column vector of size {v.Size}.")

            let dm = m.Data
            let dv = v.Data
            let dmX = m.DX
            let dmY = m.DY
            let dvX = v.DX

            let mutable fim = m.FirstIndex
            let em = fim + m.DSY
            let fiv = v.FirstIndex
            let ev = fiv + v.DSX

            let result = Array.zeroCreate <| int m.SY
            let mutable j = 0

            while fim <> em do
                let mutable im = fim
                let mutable iv = fiv
                let mutable sum = Unchecked.defaultof<'T3>

                while iv <> ev do
                    sum <- sum + dv.[int iv] * dm.[int im]
                    im <- im + dmX
                    iv <- iv + dvX

                result.[j] <- sum
                j <- j + 1
                fim <- fim + dmY

            Vector<'T3> result

        /// Computes the product of two matrices.
        let inline multiply (m1: Matrix<'T1>) (m2: Matrix<'T2>) : Matrix<'T3> =
            if m1.SX <> m2.SY then
                raise <| ArgumentException($"Cannot multiply matrices of size {m1.Size} and {m2.Size}.")

            let d1 = m1.Data
            let d2 = m2.Data
            let dX1 = m1.DX
            let dY1 = m1.DY
            let dX2 = m2.DX
            let dY2 = m2.DY
            let fi2 = m2.FirstIndex
            let eX2 = fi2 + m2.DSX

            let result = Array.zeroCreate (int m1.SY * int m2.SX)
            let mutable j = 0

            let mutable r1 = m1.FirstIndex
            let mutable eX1 = r1 + m1.DSX
            let eY1 = r1 + m1.DSY

            while r1 <> eY1 do
                let mutable c2 = fi2

                while c2 <> eX2 do
                    let mutable i1 = r1
                    let mutable i2 = c2
                    let mutable sum = Unchecked.defaultof<'T3>

                    while i1 <> eX1 do
                        sum <- sum + d1.[int i1] * d2.[int i2]
                        i1 <- i1 + dX1
                        i2 <- i2 + dY2

                    result.[j] <- sum
                    j <- j + 1
                    c2 <- c2 + dX2

                r1 <- r1 + dY1
                eX1 <- eX1 + dY1

            Matrix<'T3>(result, m2.SX, m1.SY)

    module Volume =

        /// Computes the component-wise sum of two volumes.
        let inline add (v1: Volume<'T1>) (v2: Volume<'T2>) : Volume<'T3> =
            Volume.map2 (+) v1 v2

        /// Computes the component-wise difference of two volumes.
        let inline subtract (v1: Volume<'T1>) (v2: Volume<'T2>) : Volume<'T3> =
            Volume.map2 (-) v1 v2

        /// Computes the component-wise product of two volumes.
        let inline multiply (v1: Volume<'T1>) (v2: Volume<'T2>) : Volume<'T3> =
            Volume.map2 (*) v1 v2

        /// Computes the component-wise fraction of two volumes.
        let inline divide (v1: Volume<'T1>) (v2: Volume<'T2>) : Volume<'T3> =
            Volume.map2 (/) v1 v2

    module Tensor4 =

        /// Computes the component-wise sum of two 4D tensors.
        let inline add (v1: Tensor4<'T1>) (v2: Tensor4<'T2>) : Tensor4<'T3> =
            Tensor4.map2 (+) v1 v2

        /// Computes the component-wise difference of two 4D tensors.
        let inline subtract (v1: Tensor4<'T1>) (v2: Tensor4<'T2>) : Tensor4<'T3> =
            Tensor4.map2 (-) v1 v2

        /// Computes the component-wise product of two 4D tensors.
        let inline multiply (v1: Tensor4<'T1>) (v2: Tensor4<'T2>) : Tensor4<'T3> =
            Tensor4.map2 (*) v1 v2

        /// Computes the component-wise fraction of two 4D tensors.
        let inline divide (v1: Tensor4<'T1>) (v2: Tensor4<'T2>) : Tensor4<'T3> =
            Tensor4.map2 (/) v1 v2