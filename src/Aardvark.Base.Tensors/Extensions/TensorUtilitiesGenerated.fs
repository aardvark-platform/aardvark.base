namespace Aardvark.Base

open System

[<AutoOpen>]
module FSharpTensorExtensions =

    module Vector =

        /// Returns if two vectors are equal according to the given comparison function.
        let inline equalsWith ([<InlineIfLambda>] compare : 'T1 -> 'T2 -> bool) (v1: Vector<'T1>) (v2: Vector<'T2>) =
            if v1.Size <> v2.Size then
                false
            else
                let s0_1 = v1.DSX
                let j0_1 = v1.JX
                let j0_2 = v2.JX
                let mutable i1 = v1.FirstIndex
                let mutable i2 = v2.FirstIndex

                let d1 = v1.Data
                let d2 = v2.Data
                let mutable result = true

                let e0_1 = i1 + s0_1
                while i1 <> e0_1 && result do
                    result <- compare d1.[int i1] d2.[int i2]
                    i1 <- i1 + j0_1
                    i2 <- i2 + j0_2

                result

        /// Returns if two vectors are equal.
        let inline equals (v1: Vector<'T>) (v2: Vector<'T>) =
            equalsWith (=) v1 v2

        /// Returns if two vectors are equal, i.e. the absolute difference between two corresponding elements does not exceed the given epsilon value.
        let inline equalsWithin (epsilon: 'T) (v1: Vector<'T>) (v2: Vector<'T>) =
            equalsWith (fun x y -> if x > y then x - y <= epsilon else y - x <= epsilon) v1 v2

        /// Invokes the given function for each index of a vector.
        let inline iteri ([<InlineIfLambda>] action: int64 -> unit) (v: Vector<'T>) =
            let s0 = v.DSX
            let j0 = v.JX
            let mutable i = v.FirstIndex

            let e0 = i + s0
            while i <> e0 do
                action i
                i <- i + j0

        /// Invokes the given function for each element of a vector.
        let inline iter ([<InlineIfLambda>] action: 'T -> unit) (v: Vector<'T>) =
            let d = v.Data
            v |> iteri (fun i -> action d.[int i])

        /// Computes a new vector by applying the given mapping function to each element of a vector.
        let inline map ([<InlineIfLambda>] mapping: 'T -> 'U) (v: Vector<'T>) =
            let result = Array.zeroCreate <| int (v.SX)
            let mutable j = 0

            v |> iter (fun x ->
                result.[j] <- mapping x
                j <- j + 1
            )

            Vector<'U>(result, 0L, v.Size, 1L)

        /// Applies the given mapping function to each element of a vector. Returns the modified input vector.
        let inline mapInPlace ([<InlineIfLambda>] mapping: 'T -> 'T) (v: Vector<'T>) =
            let d = v.Data
            v |> iteri (fun i -> d.[int i] <- mapping d.[int i])
            v

        /// Applies the given mapping function to each element of a vector, accumulating the results using the given reduction function.
        let inline mapReduce ([<InlineIfLambda>] mapping: 'T -> 'U) ([<InlineIfLambda>] reduction: 'U -> 'U -> 'U) (v: Vector<'T>) =
            let d = v.Data
            let fi = v.FirstIndex
            let j = v.JX
            let e = fi + v.DSX
            let mutable result = mapping d.[int fi]
            let mutable i = fi + j
            while i <> e do
                result <- reduction result (mapping d.[int i])
                i <- i + j
            result

        /// Invokes the given function for each index of two vectors. The vectors must match in size.
        let inline iteri2 ([<InlineIfLambda>] action: int64 -> int64 -> unit) (v1: Vector<'T1>) (v2: Vector<'T2>) =
            if v1.Size <> v2.Size then
                raise <| ArgumentException($"Mismatching Vector size ({v1.Size} != {v2.Size})")

            let s0_1 = v1.DSX
            let j0_1 = v1.JX
            let j0_2 = v2.JX
            let mutable i1 = v1.FirstIndex
            let mutable i2 = v2.FirstIndex

            let e0_1 = i1 + s0_1
            while i1 <> e0_1 do
                action i1 i2
                i1 <- i1 + j0_1
                i2 <- i2 + j0_2

        /// Invokes the given function for each element of two vectors. The vectors must match in size.
        let inline iter2 ([<InlineIfLambda>] action: 'T1 -> 'T2 -> unit) (v1: Vector<'T1>) (v2: Vector<'T2>) =
            let d1 = v1.Data
            let d2 = v2.Data
            iteri2 (fun i1 i2 -> action d1.[int i1] d2.[int i2]) v1 v2

        /// Computes a new vector by applying the given mapping function to each element of two vectors. The vectors must match in size.
        let inline map2 ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T3) (v1: Vector<'T1>) (v2: Vector<'T2>) =
            let result = Array.zeroCreate <| int (v1.SX)
            let mutable j = 0

            iter2 (fun x y ->
                result.[j] <- mapping x y
                j <- j + 1
            ) v1 v2

            Vector<'T3>(result, 0L, v1.Size, 1L)

        /// Applies the given mapping function to each element of two vectors. Returns v2 where the computed values are stored.
        /// The vectors must match in size.
        let inline map2InPlace ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T2) (v1: Vector<'T1>) (v2: Vector<'T2>) =
            let d1 = v1.Data
            let d2 = v2.Data
            iteri2 (fun i1 i2 -> d2.[int i2] <- mapping d1.[int i1] d2.[int i2]) v1 v2
            v2

        /// Applies the given mapping function to each element of two vectors, accumulating the results using the given reduction function.
        /// The vectors must match in size.
        let inline mapReduce2 ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T3) ([<InlineIfLambda>] reduction: 'T3 -> 'T3 -> 'T3) (v1: Vector<'T1>) (v2: Vector<'T2>) =
            if v1.Size <> v2.Size then
                raise <| ArgumentException($"Mismatching Vector size ({v1.Size} != {v2.Size})")
            let d1 = v1.Data
            let d2 = v2.Data
            let fi1 = v1.FirstIndex
            let fi2 = v2.FirstIndex
            let j1 = v1.JX
            let j2 = v2.JX
            let e1 = fi1 + v1.DSX
            let mutable result = mapping d1.[int fi1] d2.[int fi2]
            let mutable i1 = fi1 + j1
            let mutable i2 = fi2 + j2
            while i1 <> e1 do
                result <- reduction result (mapping d1.[int i1] d2.[int i2])
                i1 <- i1 + j1
                i2 <- i2 + j2
            result

    module Matrix =

        /// Returns if two matrices are equal according to the given comparison function.
        let inline equalsWith ([<InlineIfLambda>] compare : 'T1 -> 'T2 -> bool) (m1: Matrix<'T1>) (m2: Matrix<'T2>) =
            if m1.Size <> m2.Size then
                false
            else
                let mutable s0_1 = 0L
                let mutable j0_1 = 0L
                let mutable j0_2 = 0L
                let mutable s1_1 = 0L
                let mutable j1_1 = 0L
                let mutable j1_2 = 0L

                if abs m2.DX < abs m2.DY then
                    s0_1 <- m1.DSX; j0_1 <- m1.Info.JX0; j0_2 <- m2.Info.JX0
                    s1_1 <- m1.DSY; j1_1 <- m1.Info.JYX; j1_2 <- m2.Info.JYX
                else
                    s0_1 <- m1.DSY; j0_1 <- m1.Info.JY0; j0_2 <- m2.Info.JY0
                    s1_1 <- m1.DSX; j1_1 <- m1.Info.JXY; j1_2 <- m2.Info.JXY

                let mutable i1 = m1.FirstIndex
                let mutable i2 = m2.FirstIndex

                let d1 = m1.Data
                let d2 = m2.Data
                let mutable result = true

                let e1_1 = i1 + s1_1
                while i1 <> e1_1 && result do
                    let e0_1 = i1 + s0_1
                    while i1 <> e0_1 && result do
                        result <- compare d1.[int i1] d2.[int i2]
                        i1 <- i1 + j0_1
                        i2 <- i2 + j0_2
                    i1 <- i1 + j1_1
                    i2 <- i2 + j1_2

                result

        /// Returns if two matrices are equal.
        let inline equals (m1: Matrix<'T>) (m2: Matrix<'T>) =
            equalsWith (=) m1 m2

        /// Returns if two matrices are equal, i.e. the absolute difference between two corresponding elements does not exceed the given epsilon value.
        let inline equalsWithin (epsilon: 'T) (m1: Matrix<'T>) (m2: Matrix<'T>) =
            equalsWith (fun x y -> if x > y then x - y <= epsilon else y - x <= epsilon) m1 m2

        /// Invokes the given function for each index of a matrix.
        let inline iteri ([<InlineIfLambda>] action: int64 -> unit) (m: Matrix<'T>) =
            let mutable s0 = 0L
            let mutable j0 = 0L
            let mutable s1 = 0L
            let mutable j1 = 0L

            if abs m.DX < abs m.DY then
                s0 <- m.DSX; j0 <- m.Info.JX0
                s1 <- m.DSY; j1 <- m.Info.JYX
            else
                s0 <- m.DSY; j0 <- m.Info.JY0
                s1 <- m.DSX; j1 <- m.Info.JXY

            let mutable i = m.FirstIndex

            let e1 = i + s1
            while i <> e1 do
                let e0 = i + s0
                while i <> e0 do
                    action i
                    i <- i + j0
                i <- i + j1

        /// Invokes the given function for each element of a matrix.
        let inline iter ([<InlineIfLambda>] action: 'T -> unit) (m: Matrix<'T>) =
            let d = m.Data
            m |> iteri (fun i -> action d.[int i])

        /// Computes a new matrix by applying the given mapping function to each element of a matrix.
        let inline map ([<InlineIfLambda>] mapping: 'T -> 'U) (m: Matrix<'T>) =
            let result = Array.zeroCreate <| int (m.SX * m.SY)

            let delta =
                if abs m.DX < abs m.DY then
                    V2l(1L, m.SX)
                else
                    V2l(m.SY, 1L)

            let mutable j = 0

            m |> iter (fun x ->
                result.[j] <- mapping x
                j <- j + 1
            )

            Matrix<'U>(result, 0L, m.Size, delta)

        /// Applies the given mapping function to each element of a matrix. Returns the modified input matrix.
        let inline mapInPlace ([<InlineIfLambda>] mapping: 'T -> 'T) (m: Matrix<'T>) =
            let d = m.Data
            m |> iteri (fun i -> d.[int i] <- mapping d.[int i])
            m

        /// Invokes the given function for each index of two matrices. The matrices must match in size.
        let inline iteri2 ([<InlineIfLambda>] action: int64 -> int64 -> unit) (m1: Matrix<'T1>) (m2: Matrix<'T2>) =
            if m1.Size <> m2.Size then
                raise <| ArgumentException($"Mismatching Matrix size ({m1.Size} != {m2.Size})")

            let mutable s0_1 = 0L
            let mutable j0_1 = 0L
            let mutable j0_2 = 0L
            let mutable s1_1 = 0L
            let mutable j1_1 = 0L
            let mutable j1_2 = 0L

            if abs m2.DX < abs m2.DY then
                s0_1 <- m1.DSX; j0_1 <- m1.Info.JX0; j0_2 <- m2.Info.JX0
                s1_1 <- m1.DSY; j1_1 <- m1.Info.JYX; j1_2 <- m2.Info.JYX
            else
                s0_1 <- m1.DSY; j0_1 <- m1.Info.JY0; j0_2 <- m2.Info.JY0
                s1_1 <- m1.DSX; j1_1 <- m1.Info.JXY; j1_2 <- m2.Info.JXY

            let mutable i1 = m1.FirstIndex
            let mutable i2 = m2.FirstIndex

            let e1_1 = i1 + s1_1
            while i1 <> e1_1 do
                let e0_1 = i1 + s0_1
                while i1 <> e0_1 do
                    action i1 i2
                    i1 <- i1 + j0_1
                    i2 <- i2 + j0_2
                i1 <- i1 + j1_1
                i2 <- i2 + j1_2

        /// Invokes the given function for each element of two matrices. The matrices must match in size.
        let inline iter2 ([<InlineIfLambda>] action: 'T1 -> 'T2 -> unit) (m1: Matrix<'T1>) (m2: Matrix<'T2>) =
            let d1 = m1.Data
            let d2 = m2.Data
            iteri2 (fun i1 i2 -> action d1.[int i1] d2.[int i2]) m1 m2

        /// Computes a new matrix by applying the given mapping function to each element of two matrices. The matrices must match in size.
        let inline map2 ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T3) (m1: Matrix<'T1>) (m2: Matrix<'T2>) =
            let result = Array.zeroCreate <| int (m1.SX * m1.SY)

            let delta =
                if abs m2.DX < abs m2.DY then
                    V2l(1L, m2.SX)
                else
                    V2l(m2.SY, 1L)

            let mutable j = 0

            iter2 (fun x y ->
                result.[j] <- mapping x y
                j <- j + 1
            ) m1 m2

            Matrix<'T3>(result, 0L, m1.Size, delta)

        /// Applies the given mapping function to each element of two matrices. Returns m2 where the computed values are stored.
        /// The matrices must match in size.
        let inline map2InPlace ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T2) (m1: Matrix<'T1>) (m2: Matrix<'T2>) =
            let d1 = m1.Data
            let d2 = m2.Data
            iteri2 (fun i1 i2 -> d2.[int i2] <- mapping d1.[int i1] d2.[int i2]) m1 m2
            m2

    module Volume =

        /// Returns if two volumes are equal according to the given comparison function.
        let inline equalsWith ([<InlineIfLambda>] compare : 'T1 -> 'T2 -> bool) (v1: Volume<'T1>) (v2: Volume<'T2>) =
            if v1.Size <> v2.Size then
                false
            else
                let mutable s0_1 = 0L
                let mutable j0_1 = 0L
                let mutable j0_2 = 0L
                let mutable s1_1 = 0L
                let mutable j1_1 = 0L
                let mutable j1_2 = 0L
                let mutable s2_1 = 0L
                let mutable j2_1 = 0L
                let mutable j2_2 = 0L

                if abs v2.DX < abs v2.DZ then
                    if abs v2.DX < abs v2.DY then
                        s0_1 <- v1.DSX; j0_1 <- v1.Info.JX0; j0_2 <- v2.Info.JX0
                        s1_1 <- v1.DSY; j1_1 <- v1.Info.JYX; j1_2 <- v2.Info.JYX
                        s2_1 <- v1.DSZ; j2_1 <- v1.Info.JZY; j2_2 <- v2.Info.JZY
                    else
                        s0_1 <- v1.DSY; j0_1 <- v1.Info.JY0; j0_2 <- v2.Info.JY0
                        s1_1 <- v1.DSX; j1_1 <- v1.Info.JXY; j1_2 <- v2.Info.JXY
                        s2_1 <- v1.DSZ; j2_1 <- v1.Info.JZX; j2_2 <- v2.Info.JZX
                else
                    if abs v2.DX < abs v2.DY then
                        s0_1 <- v1.DSZ; j0_1 <- v1.Info.JZ0; j0_2 <- v2.Info.JZ0
                        s1_1 <- v1.DSX; j1_1 <- v1.Info.JXZ; j1_2 <- v2.Info.JXZ
                        s2_1 <- v1.DSY; j2_1 <- v1.Info.JYX; j2_2 <- v2.Info.JYX
                    else
                        s0_1 <- v1.DSZ; j0_1 <- v1.Info.JZ0; j0_2 <- v2.Info.JZ0
                        s1_1 <- v1.DSY; j1_1 <- v1.Info.JYZ; j1_2 <- v2.Info.JYZ
                        s2_1 <- v1.DSX; j2_1 <- v1.Info.JXY; j2_2 <- v2.Info.JXY

                let mutable i1 = v1.FirstIndex
                let mutable i2 = v2.FirstIndex

                let d1 = v1.Data
                let d2 = v2.Data
                let mutable result = true

                let e2_1 = i1 + s2_1
                while i1 <> e2_1 && result do
                    let e1_1 = i1 + s1_1
                    while i1 <> e1_1 && result do
                        let e0_1 = i1 + s0_1
                        while i1 <> e0_1 && result do
                            result <- compare d1.[int i1] d2.[int i2]
                            i1 <- i1 + j0_1
                            i2 <- i2 + j0_2
                        i1 <- i1 + j1_1
                        i2 <- i2 + j1_2
                    i1 <- i1 + j2_1
                    i2 <- i2 + j2_2

                result

        /// Returns if two volumes are equal.
        let inline equals (v1: Volume<'T>) (v2: Volume<'T>) =
            equalsWith (=) v1 v2

        /// Returns if two volumes are equal, i.e. the absolute difference between two corresponding elements does not exceed the given epsilon value.
        let inline equalsWithin (epsilon: 'T) (v1: Volume<'T>) (v2: Volume<'T>) =
            equalsWith (fun x y -> if x > y then x - y <= epsilon else y - x <= epsilon) v1 v2

        /// Invokes the given function for each index of a volume.
        let inline iteri ([<InlineIfLambda>] action: int64 -> unit) (v: Volume<'T>) =
            let mutable s0 = 0L
            let mutable j0 = 0L
            let mutable s1 = 0L
            let mutable j1 = 0L
            let mutable s2 = 0L
            let mutable j2 = 0L

            if abs v.DX < abs v.DZ then
                if abs v.DX < abs v.DY then
                    s0 <- v.DSX; j0 <- v.Info.JX0
                    s1 <- v.DSY; j1 <- v.Info.JYX
                    s2 <- v.DSZ; j2 <- v.Info.JZY
                else
                    s0 <- v.DSY; j0 <- v.Info.JY0
                    s1 <- v.DSX; j1 <- v.Info.JXY
                    s2 <- v.DSZ; j2 <- v.Info.JZX
            else
                if abs v.DX < abs v.DY then
                    s0 <- v.DSZ; j0 <- v.Info.JZ0
                    s1 <- v.DSX; j1 <- v.Info.JXZ
                    s2 <- v.DSY; j2 <- v.Info.JYX
                else
                    s0 <- v.DSZ; j0 <- v.Info.JZ0
                    s1 <- v.DSY; j1 <- v.Info.JYZ
                    s2 <- v.DSX; j2 <- v.Info.JXY

            let mutable i = v.FirstIndex

            let e2 = i + s2
            while i <> e2 do
                let e1 = i + s1
                while i <> e1 do
                    let e0 = i + s0
                    while i <> e0 do
                        action i
                        i <- i + j0
                    i <- i + j1
                i <- i + j2

        /// Invokes the given function for each element of a volume.
        let inline iter ([<InlineIfLambda>] action: 'T -> unit) (v: Volume<'T>) =
            let d = v.Data
            v |> iteri (fun i -> action d.[int i])

        /// Computes a new volume by applying the given mapping function to each element of a volume.
        let inline map ([<InlineIfLambda>] mapping: 'T -> 'U) (v: Volume<'T>) =
            let result = Array.zeroCreate <| int (v.SX * v.SY * v.SZ)

            let delta =
                if abs v.DX < abs v.DZ then
                    if abs v.DX < abs v.DY then
                        V3l(1L, v.SX, v.SX * v.SY)
                    else
                        V3l(v.SY, 1L, v.SY * v.SX)
                else
                    if abs v.DX < abs v.DY then
                        V3l(v.SZ, v.SZ * v.SX, 1L)
                    else
                        V3l(v.SZ * v.SY, v.SZ, 1L)

            let mutable j = 0

            v |> iter (fun x ->
                result.[j] <- mapping x
                j <- j + 1
            )

            Volume<'U>(result, 0L, v.Size, delta)

        /// Applies the given mapping function to each element of a volume. Returns the modified input volume.
        let inline mapInPlace ([<InlineIfLambda>] mapping: 'T -> 'T) (v: Volume<'T>) =
            let d = v.Data
            v |> iteri (fun i -> d.[int i] <- mapping d.[int i])
            v

        /// Invokes the given function for each index of two volumes. The volumes must match in size.
        let inline iteri2 ([<InlineIfLambda>] action: int64 -> int64 -> unit) (v1: Volume<'T1>) (v2: Volume<'T2>) =
            if v1.Size <> v2.Size then
                raise <| ArgumentException($"Mismatching Volume size ({v1.Size} != {v2.Size})")

            let mutable s0_1 = 0L
            let mutable j0_1 = 0L
            let mutable j0_2 = 0L
            let mutable s1_1 = 0L
            let mutable j1_1 = 0L
            let mutable j1_2 = 0L
            let mutable s2_1 = 0L
            let mutable j2_1 = 0L
            let mutable j2_2 = 0L

            if abs v2.DX < abs v2.DZ then
                if abs v2.DX < abs v2.DY then
                    s0_1 <- v1.DSX; j0_1 <- v1.Info.JX0; j0_2 <- v2.Info.JX0
                    s1_1 <- v1.DSY; j1_1 <- v1.Info.JYX; j1_2 <- v2.Info.JYX
                    s2_1 <- v1.DSZ; j2_1 <- v1.Info.JZY; j2_2 <- v2.Info.JZY
                else
                    s0_1 <- v1.DSY; j0_1 <- v1.Info.JY0; j0_2 <- v2.Info.JY0
                    s1_1 <- v1.DSX; j1_1 <- v1.Info.JXY; j1_2 <- v2.Info.JXY
                    s2_1 <- v1.DSZ; j2_1 <- v1.Info.JZX; j2_2 <- v2.Info.JZX
            else
                if abs v2.DX < abs v2.DY then
                    s0_1 <- v1.DSZ; j0_1 <- v1.Info.JZ0; j0_2 <- v2.Info.JZ0
                    s1_1 <- v1.DSX; j1_1 <- v1.Info.JXZ; j1_2 <- v2.Info.JXZ
                    s2_1 <- v1.DSY; j2_1 <- v1.Info.JYX; j2_2 <- v2.Info.JYX
                else
                    s0_1 <- v1.DSZ; j0_1 <- v1.Info.JZ0; j0_2 <- v2.Info.JZ0
                    s1_1 <- v1.DSY; j1_1 <- v1.Info.JYZ; j1_2 <- v2.Info.JYZ
                    s2_1 <- v1.DSX; j2_1 <- v1.Info.JXY; j2_2 <- v2.Info.JXY

            let mutable i1 = v1.FirstIndex
            let mutable i2 = v2.FirstIndex

            let e2_1 = i1 + s2_1
            while i1 <> e2_1 do
                let e1_1 = i1 + s1_1
                while i1 <> e1_1 do
                    let e0_1 = i1 + s0_1
                    while i1 <> e0_1 do
                        action i1 i2
                        i1 <- i1 + j0_1
                        i2 <- i2 + j0_2
                    i1 <- i1 + j1_1
                    i2 <- i2 + j1_2
                i1 <- i1 + j2_1
                i2 <- i2 + j2_2

        /// Invokes the given function for each element of two volumes. The volumes must match in size.
        let inline iter2 ([<InlineIfLambda>] action: 'T1 -> 'T2 -> unit) (v1: Volume<'T1>) (v2: Volume<'T2>) =
            let d1 = v1.Data
            let d2 = v2.Data
            iteri2 (fun i1 i2 -> action d1.[int i1] d2.[int i2]) v1 v2

        /// Computes a new volume by applying the given mapping function to each element of two volumes. The volumes must match in size.
        let inline map2 ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T3) (v1: Volume<'T1>) (v2: Volume<'T2>) =
            let result = Array.zeroCreate <| int (v1.SX * v1.SY * v1.SZ)

            let delta =
                if abs v2.DX < abs v2.DZ then
                    if abs v2.DX < abs v2.DY then
                        V3l(1L, v2.SX, v2.SX * v2.SY)
                    else
                        V3l(v2.SY, 1L, v2.SY * v2.SX)
                else
                    if abs v2.DX < abs v2.DY then
                        V3l(v2.SZ, v2.SZ * v2.SX, 1L)
                    else
                        V3l(v2.SZ * v2.SY, v2.SZ, 1L)

            let mutable j = 0

            iter2 (fun x y ->
                result.[j] <- mapping x y
                j <- j + 1
            ) v1 v2

            Volume<'T3>(result, 0L, v1.Size, delta)

        /// Applies the given mapping function to each element of two volumes. Returns v2 where the computed values are stored.
        /// The volumes must match in size.
        let inline map2InPlace ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T2) (v1: Volume<'T1>) (v2: Volume<'T2>) =
            let d1 = v1.Data
            let d2 = v2.Data
            iteri2 (fun i1 i2 -> d2.[int i2] <- mapping d1.[int i1] d2.[int i2]) v1 v2
            v2

    module Tensor4 =

        /// Returns if two 4D tensors are equal according to the given comparison function.
        let inline equalsWith ([<InlineIfLambda>] compare : 'T1 -> 'T2 -> bool) (t1: Tensor4<'T1>) (t2: Tensor4<'T2>) =
            if t1.Size <> t2.Size then
                false
            else
                let mutable s0_1 = 0L
                let mutable j0_1 = 0L
                let mutable j0_2 = 0L
                let mutable s1_1 = 0L
                let mutable j1_1 = 0L
                let mutable j1_2 = 0L
                let mutable s2_1 = 0L
                let mutable j2_1 = 0L
                let mutable j2_2 = 0L
                let mutable s3_1 = 0L
                let mutable j3_1 = 0L
                let mutable j3_2 = 0L

                if abs t2.DX < abs t2.DW then
                    if abs t2.DX < abs t2.DY then
                        s0_1 <- t1.DSX; j0_1 <- t1.Info.JX0; j0_2 <- t2.Info.JX0
                        s1_1 <- t1.DSY; j1_1 <- t1.Info.JYX; j1_2 <- t2.Info.JYX
                        s2_1 <- t1.DSZ; j2_1 <- t1.Info.JZY; j2_2 <- t2.Info.JZY
                        s3_1 <- t1.DSW; j3_1 <- t1.Info.JWZ; j3_2 <- t2.Info.JWZ
                    else
                        s0_1 <- t1.DSY; j0_1 <- t1.Info.JY0; j0_2 <- t2.Info.JY0
                        s1_1 <- t1.DSX; j1_1 <- t1.Info.JXY; j1_2 <- t2.Info.JXY
                        s2_1 <- t1.DSZ; j2_1 <- t1.Info.JZX; j2_2 <- t2.Info.JZX
                        s3_1 <- t1.DSW; j3_1 <- t1.Info.JWZ; j3_2 <- t2.Info.JWZ
                else
                    if abs t2.DX < abs t2.DY then
                        s0_1 <- t1.DSW; j0_1 <- t1.Info.JW0; j0_2 <- t2.Info.JW0
                        s1_1 <- t1.DSX; j1_1 <- t1.Info.JXW; j1_2 <- t2.Info.JXW
                        s2_1 <- t1.DSY; j2_1 <- t1.Info.JYX; j2_2 <- t2.Info.JYX
                        s3_1 <- t1.DSZ; j3_1 <- t1.Info.JZY; j3_2 <- t2.Info.JZY
                    else
                        s0_1 <- t1.DSW; j0_1 <- t1.Info.JW0; j0_2 <- t2.Info.JW0
                        s1_1 <- t1.DSY; j1_1 <- t1.Info.JYW; j1_2 <- t2.Info.JYW
                        s2_1 <- t1.DSX; j2_1 <- t1.Info.JXY; j2_2 <- t2.Info.JXY
                        s3_1 <- t1.DSZ; j3_1 <- t1.Info.JZX; j3_2 <- t2.Info.JZX

                let mutable i1 = t1.FirstIndex
                let mutable i2 = t2.FirstIndex

                let d1 = t1.Data
                let d2 = t2.Data
                let mutable result = true

                let e3_1 = i1 + s3_1
                while i1 <> e3_1 && result do
                    let e2_1 = i1 + s2_1
                    while i1 <> e2_1 && result do
                        let e1_1 = i1 + s1_1
                        while i1 <> e1_1 && result do
                            let e0_1 = i1 + s0_1
                            while i1 <> e0_1 && result do
                                result <- compare d1.[int i1] d2.[int i2]
                                i1 <- i1 + j0_1
                                i2 <- i2 + j0_2
                            i1 <- i1 + j1_1
                            i2 <- i2 + j1_2
                        i1 <- i1 + j2_1
                        i2 <- i2 + j2_2
                    i1 <- i1 + j3_1
                    i2 <- i2 + j3_2

                result

        /// Returns if two 4D tensors are equal.
        let inline equals (t1: Tensor4<'T>) (t2: Tensor4<'T>) =
            equalsWith (=) t1 t2

        /// Returns if two 4D tensors are equal, i.e. the absolute difference between two corresponding elements does not exceed the given epsilon value.
        let inline equalsWithin (epsilon: 'T) (t1: Tensor4<'T>) (t2: Tensor4<'T>) =
            equalsWith (fun x y -> if x > y then x - y <= epsilon else y - x <= epsilon) t1 t2

        /// Invokes the given function for each index of a 4D tensor.
        let inline iteri ([<InlineIfLambda>] action: int64 -> unit) (t: Tensor4<'T>) =
            let mutable s0 = 0L
            let mutable j0 = 0L
            let mutable s1 = 0L
            let mutable j1 = 0L
            let mutable s2 = 0L
            let mutable j2 = 0L
            let mutable s3 = 0L
            let mutable j3 = 0L

            if abs t.DX < abs t.DW then
                if abs t.DX < abs t.DY then
                    s0 <- t.DSX; j0 <- t.Info.JX0
                    s1 <- t.DSY; j1 <- t.Info.JYX
                    s2 <- t.DSZ; j2 <- t.Info.JZY
                    s3 <- t.DSW; j3 <- t.Info.JWZ
                else
                    s0 <- t.DSY; j0 <- t.Info.JY0
                    s1 <- t.DSX; j1 <- t.Info.JXY
                    s2 <- t.DSZ; j2 <- t.Info.JZX
                    s3 <- t.DSW; j3 <- t.Info.JWZ
            else
                if abs t.DX < abs t.DY then
                    s0 <- t.DSW; j0 <- t.Info.JW0
                    s1 <- t.DSX; j1 <- t.Info.JXW
                    s2 <- t.DSY; j2 <- t.Info.JYX
                    s3 <- t.DSZ; j3 <- t.Info.JZY
                else
                    s0 <- t.DSW; j0 <- t.Info.JW0
                    s1 <- t.DSY; j1 <- t.Info.JYW
                    s2 <- t.DSX; j2 <- t.Info.JXY
                    s3 <- t.DSZ; j3 <- t.Info.JZX

            let mutable i = t.FirstIndex

            let e3 = i + s3
            while i <> e3 do
                let e2 = i + s2
                while i <> e2 do
                    let e1 = i + s1
                    while i <> e1 do
                        let e0 = i + s0
                        while i <> e0 do
                            action i
                            i <- i + j0
                        i <- i + j1
                    i <- i + j2
                i <- i + j3

        /// Invokes the given function for each element of a 4D tensor.
        let inline iter ([<InlineIfLambda>] action: 'T -> unit) (t: Tensor4<'T>) =
            let d = t.Data
            t |> iteri (fun i -> action d.[int i])

        /// Computes a new 4D tensor by applying the given mapping function to each element of a 4D tensor.
        let inline map ([<InlineIfLambda>] mapping: 'T -> 'U) (t: Tensor4<'T>) =
            let result = Array.zeroCreate <| int (t.SX * t.SY * t.SZ * t.SW)

            let delta =
                if abs t.DX < abs t.DW then
                    if abs t.DX < abs t.DY then
                        V4l(1L, t.SX, t.SX * t.SY, t.SX * t.SY * t.SZ)
                    else
                        V4l(t.SY, 1L, t.SY * t.SX, t.SY * t.SX * t.SZ)
                else
                    if abs t.DX < abs t.DY then
                        V4l(t.SW, t.SW * t.SX, t.SW * t.SX * t.SY, 1L)
                    else
                        V4l(t.SW * t.SY, t.SW, t.SW * t.SY * t.SX, 1L)

            let mutable j = 0

            t |> iter (fun x ->
                result.[j] <- mapping x
                j <- j + 1
            )

            Tensor4<'U>(result, 0L, t.Size, delta)

        /// Applies the given mapping function to each element of a 4D tensor. Returns the modified input 4D tensor.
        let inline mapInPlace ([<InlineIfLambda>] mapping: 'T -> 'T) (t: Tensor4<'T>) =
            let d = t.Data
            t |> iteri (fun i -> d.[int i] <- mapping d.[int i])
            t

        /// Invokes the given function for each index of two 4D tensors. The 4D tensors must match in size.
        let inline iteri2 ([<InlineIfLambda>] action: int64 -> int64 -> unit) (t1: Tensor4<'T1>) (t2: Tensor4<'T2>) =
            if t1.Size <> t2.Size then
                raise <| ArgumentException($"Mismatching Tensor4 size ({t1.Size} != {t2.Size})")

            let mutable s0_1 = 0L
            let mutable j0_1 = 0L
            let mutable j0_2 = 0L
            let mutable s1_1 = 0L
            let mutable j1_1 = 0L
            let mutable j1_2 = 0L
            let mutable s2_1 = 0L
            let mutable j2_1 = 0L
            let mutable j2_2 = 0L
            let mutable s3_1 = 0L
            let mutable j3_1 = 0L
            let mutable j3_2 = 0L

            if abs t2.DX < abs t2.DW then
                if abs t2.DX < abs t2.DY then
                    s0_1 <- t1.DSX; j0_1 <- t1.Info.JX0; j0_2 <- t2.Info.JX0
                    s1_1 <- t1.DSY; j1_1 <- t1.Info.JYX; j1_2 <- t2.Info.JYX
                    s2_1 <- t1.DSZ; j2_1 <- t1.Info.JZY; j2_2 <- t2.Info.JZY
                    s3_1 <- t1.DSW; j3_1 <- t1.Info.JWZ; j3_2 <- t2.Info.JWZ
                else
                    s0_1 <- t1.DSY; j0_1 <- t1.Info.JY0; j0_2 <- t2.Info.JY0
                    s1_1 <- t1.DSX; j1_1 <- t1.Info.JXY; j1_2 <- t2.Info.JXY
                    s2_1 <- t1.DSZ; j2_1 <- t1.Info.JZX; j2_2 <- t2.Info.JZX
                    s3_1 <- t1.DSW; j3_1 <- t1.Info.JWZ; j3_2 <- t2.Info.JWZ
            else
                if abs t2.DX < abs t2.DY then
                    s0_1 <- t1.DSW; j0_1 <- t1.Info.JW0; j0_2 <- t2.Info.JW0
                    s1_1 <- t1.DSX; j1_1 <- t1.Info.JXW; j1_2 <- t2.Info.JXW
                    s2_1 <- t1.DSY; j2_1 <- t1.Info.JYX; j2_2 <- t2.Info.JYX
                    s3_1 <- t1.DSZ; j3_1 <- t1.Info.JZY; j3_2 <- t2.Info.JZY
                else
                    s0_1 <- t1.DSW; j0_1 <- t1.Info.JW0; j0_2 <- t2.Info.JW0
                    s1_1 <- t1.DSY; j1_1 <- t1.Info.JYW; j1_2 <- t2.Info.JYW
                    s2_1 <- t1.DSX; j2_1 <- t1.Info.JXY; j2_2 <- t2.Info.JXY
                    s3_1 <- t1.DSZ; j3_1 <- t1.Info.JZX; j3_2 <- t2.Info.JZX

            let mutable i1 = t1.FirstIndex
            let mutable i2 = t2.FirstIndex

            let e3_1 = i1 + s3_1
            while i1 <> e3_1 do
                let e2_1 = i1 + s2_1
                while i1 <> e2_1 do
                    let e1_1 = i1 + s1_1
                    while i1 <> e1_1 do
                        let e0_1 = i1 + s0_1
                        while i1 <> e0_1 do
                            action i1 i2
                            i1 <- i1 + j0_1
                            i2 <- i2 + j0_2
                        i1 <- i1 + j1_1
                        i2 <- i2 + j1_2
                    i1 <- i1 + j2_1
                    i2 <- i2 + j2_2
                i1 <- i1 + j3_1
                i2 <- i2 + j3_2

        /// Invokes the given function for each element of two 4D tensors. The 4D tensors must match in size.
        let inline iter2 ([<InlineIfLambda>] action: 'T1 -> 'T2 -> unit) (t1: Tensor4<'T1>) (t2: Tensor4<'T2>) =
            let d1 = t1.Data
            let d2 = t2.Data
            iteri2 (fun i1 i2 -> action d1.[int i1] d2.[int i2]) t1 t2

        /// Computes a new 4D tensor by applying the given mapping function to each element of two 4D tensors. The 4D tensors must match in size.
        let inline map2 ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T3) (t1: Tensor4<'T1>) (t2: Tensor4<'T2>) =
            let result = Array.zeroCreate <| int (t1.SX * t1.SY * t1.SZ * t1.SW)

            let delta =
                if abs t2.DX < abs t2.DW then
                    if abs t2.DX < abs t2.DY then
                        V4l(1L, t2.SX, t2.SX * t2.SY, t2.SX * t2.SY * t2.SZ)
                    else
                        V4l(t2.SY, 1L, t2.SY * t2.SX, t2.SY * t2.SX * t2.SZ)
                else
                    if abs t2.DX < abs t2.DY then
                        V4l(t2.SW, t2.SW * t2.SX, t2.SW * t2.SX * t2.SY, 1L)
                    else
                        V4l(t2.SW * t2.SY, t2.SW, t2.SW * t2.SY * t2.SX, 1L)

            let mutable j = 0

            iter2 (fun x y ->
                result.[j] <- mapping x y
                j <- j + 1
            ) t1 t2

            Tensor4<'T3>(result, 0L, t1.Size, delta)

        /// Applies the given mapping function to each element of two 4D tensors. Returns t2 where the computed values are stored.
        /// The 4D tensors must match in size.
        let inline map2InPlace ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T2) (t1: Tensor4<'T1>) (t2: Tensor4<'T2>) =
            let d1 = t1.Data
            let d2 = t2.Data
            iteri2 (fun i1 i2 -> d2.[int i2] <- mapping d1.[int i1] d2.[int i2]) t1 t2
            t2
