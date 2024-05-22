namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open FsUnit
open NUnit.Framework
open Microsoft.FSharp.NativeInterop
open Operators.Checked

#nowarn "9"

type TensorKind =
    | Full = 0
    | Sub = 1
    | Window = 2

module TensorGeneration =

    let mutable rnd = Unchecked.defaultof<RandomSystem>

    let init (seed: int) =
        rnd <- RandomSystem seed

    let inline getVectorOfSize' (f: RandomSystem -> int -> 'T) (t: TensorKind) (n: ^Int) =
        let n = int n

        match t with
        | TensorKind.Full ->
            Vector.Create <| Array.init n (f rnd)

        | TensorKind.Sub ->
            let d = 1 + rnd.UniformInt 4
            let arr = Array.init (n * d + 4) (f rnd)
            let f = rnd.UniformInt (arr.Length - (n - 1) * d)
            Vector.Create(arr).SubVector(f, n, d)

        | _ ->
            let d = 1 + rnd.UniformInt 4
            let arr = Array.init ((n + 1) * d) (f rnd)
            let f = rnd.UniformInt ((arr.Length - (n - 1) * d) / d)
            Vector.Create(arr).SubVectorWindow(f, n, d)

    let getVector' (f: RandomSystem -> int -> 'T) (t: TensorKind) =
        getVectorOfSize' f t (32 + rnd.UniformInt 128)

    let inline getMatrixOfSize' (f: RandomSystem -> int -> 'T) (t: TensorKind) (rowMajor: bool) (s: 'v2i) =
        let s = v2i s

        match t with
        | TensorKind.Full ->
            let arr = Array.init (s.X * s.Y) (f rnd)
            let d =
                if rowMajor then V2i(1, s.X)
                else V2i(s.Y, 1)

            Matrix(arr, 0L, V2l s, V2l d)

        | TensorKind.Sub ->
            let d = 1 + rnd.UniformV2i 4
            let S = s * d + 4
            let arr = Array.init (S.X * S.Y) (f rnd)

            let d =
                if rowMajor then V2i(d.X, d.Y * S.X)
                else V2i(d.X * S.Y, d.Y)

            let n = 1 + Vec.dot (s - 1) d
            let o = arr.Length - n
            let f = V2i(o % S.X, o / S.X)

            Matrix(arr, S).SubMatrix(f, s, d)

        | _ ->
            let d = 1 + rnd.UniformV2i 4
            let S = (s + 1) * d
            let arr = Array.init (S.X * S.Y) (f rnd)
            let f = rnd.UniformV2i ((S - (s - 1) * d) / d)

            let d =
                if rowMajor then V2i(d.X, d.Y * S.X)
                else V2i(d.X * S.Y, d.Y)

            Matrix(arr, S).SubMatrixWindow(f, s, d)

    let getMatrix' (f: RandomSystem -> int -> 'T) (t: TensorKind) (rowMajor: bool) =
        getMatrixOfSize' f t rowMajor (16 + rnd.UniformV2i 64)

    let inline getMatrixOfHeight' (f: RandomSystem -> int -> 'T) (t: TensorKind) (rowMajor: bool) (h: ^Int) =
        getMatrixOfSize' f t rowMajor (V2i(16 + rnd.UniformInt 64, int h))

    let inline getTensor4OfSize' (f: RandomSystem -> int -> 'T) (t: TensorKind) (innerW: bool) (s: ^V4i) =
        let s = v4i s

        match t with
        | TensorKind.Full ->
            let arr = Array.init (s.X * s.Y * s.Z * s.W) (f rnd)

            let d =
                if innerW then V4i(s.W, s.W * s.X, s.W * s.X * s.Y, 1)
                else V4i(1, s.X, s.X * s.Y, s.X * s.Y * s.Z)

            Tensor4<'T>(arr, 0L, V4l s, V4l d)

        | TensorKind.Sub ->
            let d = 1 + rnd.UniformV4i 4
            let S = s * d + 4
            let arr = Array.init (S.X * S.Y * S.Z * S.W) (f rnd)

            let d =
                if innerW then d * V4i(S.W, S.W * S.X, S.W * S.X * S.Y, 1)
                else d * V4i(1, S.X, S.X * S.Y, S.X * S.Y * S.Z)

            let n = 1 + Vec.dot (s - 1) d
            let o = arr.Length - n
            let f = V4i(o % S.X, (o / S.X) % S.Y, (o / (S.X * S.Y)) % S.Z, o / (S.X * S.Y * S.Z))

            Tensor4<'T>(arr, S).SubTensor4(f, s, d)

        | _ ->
            let d = 1 + rnd.UniformV4i 3
            let S = (s + 1) * d
            let arr = Array.init (S.X * S.Y * S.Z * S.W) (f rnd)
            let f = rnd.UniformV4i ((S - (s - 1) * d) / d)

            let d =
                if innerW then d * V4i(S.W, S.W * S.X, S.W * S.X * S.Y, 1)
                else d * V4i(1, S.X, S.X * S.Y, S.X * S.Y * S.Z)

            Tensor4<'T>(arr, S).SubTensor4Window(f, s, d)

    let getTensor4' (f: RandomSystem -> int -> 'T) (t: TensorKind) (innerW: bool) =
        getTensor4OfSize' f t innerW (4 + rnd.UniformV4i 4)

    let inline getVectorOfSize (t: TensorKind) (n: ^Int) = getVectorOfSize' (fun rnd _ -> rnd.UniformDouble()) t n
    let inline getVector (t: TensorKind) = getVector' (fun rnd _ -> rnd.UniformDouble()) t
    let inline getMatrixOfSize (t: TensorKind) (rowMajor: bool) (s: 'v2i) = getMatrixOfSize' (fun rnd _ -> rnd.UniformDouble()) t rowMajor s
    let inline getMatrix (t: TensorKind) (rowMajor: bool) = getMatrix' (fun rnd _ -> rnd.UniformDouble()) t rowMajor
    let inline getMatrixOfHeight (t: TensorKind) (rowMajor: bool) (h: ^Int) = getMatrixOfHeight' (fun rnd _ -> rnd.UniformDouble()) t rowMajor h
    let inline getTensor4OfSize (t: TensorKind) (innerW: bool) (s: ^V4i) = getTensor4OfSize' (fun rnd _ -> rnd.UniformDouble()) t innerW s
    let inline getTensor4 (t: TensorKind) (innerW: bool) = getTensor4' (fun rnd _ -> rnd.UniformDouble()) t innerW

    let inline getIntVectorOfSize (t: TensorKind) (n: ^Int) =
        let offset = rnd.UniformInt 128
        getVectorOfSize' (fun _ -> id >> (+) offset) t n

    let inline getIntVector (t: TensorKind) =
        let offset = rnd.UniformInt 128
        getVector' (fun _ -> id >> (+) offset) t

    let inline getIntMatrixOfSize (t: TensorKind) (rowMajor: bool) (s: 'v2i) =
        let offset = rnd.UniformInt 128
        getMatrixOfSize' (fun _ -> id >> (+) offset) t rowMajor s

    let inline getIntMatrix (t: TensorKind) (rowMajor: bool) =
        let offset = rnd.UniformInt 128
        getMatrix' (fun _ -> id >> (+) offset) t rowMajor

    let inline getIntMatrixOfHeight (t: TensorKind) (rowMajor: bool) (h: ^Int) =
        let offset = rnd.UniformInt 128
        getMatrixOfHeight' (fun _ -> id >> (+) offset) t rowMajor h

    let inline getIntTensor4OfSize (t: TensorKind) (innerW: bool) (s: ^V4i) =
        let offset = rnd.UniformInt 128
        getTensor4OfSize' (fun _ -> id >> (+) offset) t innerW s

    let inline getIntTensor4 (t: TensorKind) (innerW: bool) =
        let offset = rnd.UniformInt 128
        getTensor4' (fun _ -> id >> (+) offset) t innerW

module ``Tensor Tests`` =

    module private PixImage =

        let rng = RandomSystem(1)

        let random (size : V2i) =
            let pi = PixImage<uint8>(Col.Format.RGBA, size)
            for c in pi.ChannelArray do
                c.SetByIndex(ignore >> rng.UniformUInt >> uint8) |> ignore
            pi

    [<Test>]
    [<TestCase(2)>]
    [<TestCase(4)>]
    [<TestCase(16)>]
    [<TestCase(123)>]
    let ``[NativeTensor] PixImage copy mirrored as NativeTensor4``(size : int) =

        let size = V2i size
        let src = PixImage.random <| V2i size
        let dst = PixImage<uint8>(Col.Format.RGBA, size)

        PixImage.pin2 src dst (fun srcVolume dstVolume ->
            let srcTensor = srcVolume.MirrorY().ToXYWTensor4()
            let dstTensor = dstVolume.ToXYWTensor4()
            NativeTensor4.copy srcTensor dstTensor
        )

        let srcMatrix = src.GetMatrix<C4b>()
        let dstMatrix = dst.GetMatrix<C4b>()

        for x = 0 to size.X - 1 do
            for y = 0 to size.Y - 1 do
                dstMatrix.[x, y] |> should equal srcMatrix.[x, size.Y - 1 - y]

    [<Test>]
    let ``[NativeTensor] iterPtr with non-byte elements``() =
        let data = [| 42; 32; 108; -34 |]
        let info = Tensor4Info(V4l(2, 2, 1, 1), V4l(1, 2, 4, 4))

        pinned data (fun ptr ->
            let nv = NativeTensor4.ofNativeInt<int> info ptr

            let mutable index = 0
            nv |> NativeTensor4.iterPtr (fun coord ptr ->
                let value = NativePtr.read ptr
                value |> should equal data.[index]
                index <- index + 1
            )
        )

    [<Test>]
    let ``[NativeTensor] iterPtr2 with different element types``() =
        let srcArray = [| 42; 32; 108; -34 |]
        let dstArray = [| 0L; 0L; 0L; 0L |]
        let info = Tensor4Info(V4l(2, 2, 1, 1), V4l(2, 1, 4, 4))

        pinned srcArray (fun srcPtr ->
            pinned dstArray (fun dstPtr ->
                let src = NativeTensor4<int>(NativePtr.ofNativeInt srcPtr, info)
                let dst = NativeTensor4<int64>(NativePtr.ofNativeInt dstPtr, info)

                (src, dst) ||> NativeTensor4.iterPtr2 (fun coord src dst ->
                    let value = NativePtr.read src
                    value |> int64 |> NativePtr.write dst

                    value |> should equal srcArray.[int <| coord.X * info.DX + coord.Y * info.DY]
                )
            )
        )

        (srcArray, dstArray) ||> Array.iter2 (fun a b ->
            (int64 a) |> should equal b
        )

    [<Test>]
    let ``[NativeTensor] copy untyped PixImage``() =
        let size = V2i(123, 321)
        let src = PixImage.random <| V2i size
        let dst = PixImage<uint32>(Col.Format.RGBA, size)

        PixImage.pin dst (fun dst ->
            let info =
                let info = dst.MirrorY().Info
                VolumeInfo(info.Origin * 4L, info.Size, info.Delta * 4L)
            src |> PixImage.copyToNative dst.Address info
        )

        for y = 0 to size.Y - 1 do
            for x = 0 to size.X - 1 do
                for c in 0L .. 3L do
                    dst.GetChannel(c).[x, y] |> should equal (uint32 <| src.GetChannel(c).[x, size.Y - 1 - y])

    [<SetUp>]
    let setup() =
        TensorGeneration.init 0

    [<Theory>]
    let ``[Vector] Iter`` (tensor: TensorKind) =
        for _ = 1 to 100 do
            let m = TensorGeneration.getIntVector tensor

            let e = m.Elements |> HashSet.ofSeq

            m |> Vector.iter (fun x ->
                e |> HashSet.remove x |> should be True
            )

            e.Count |> should equal 0

    [<Theory>]
    let ``[Matrix] Iter`` (tensor: TensorKind) (rowMajor: bool) =
        for _ = 1 to 100 do
            let m = TensorGeneration.getIntMatrix tensor rowMajor

            let expectedJumps =
                if abs m.DX < abs m.DY then
                    [ m.DX; m.DY - (m.SX - 1L) * m.DX ]
                else
                    [ m.DY; m.DX - (m.SY - 1L) * m.DY ]

            let e = m.Elements |> HashSet.ofSeq
            let mutable li = -1L

            m |> Matrix.iteri (fun i ->
                e |> HashSet.remove m.Data.[int i] |> should be True

                if li <> -1L then
                    expectedJumps |> should contain (i - li)

                li <- i
            )

            e.Count |> should equal 0

    [<Theory>]
    let ``[Tensor4] Iter`` (tensor: TensorKind) (innerW: bool) =
        for _ = 1 to 100 do
            let m = TensorGeneration.getIntTensor4 tensor innerW
            let e = m.Elements |> HashSet.ofSeq

            m |> Tensor4.iter (fun x ->
                e |> HashSet.remove x |> should be True
            )

            e.Count |> should equal 0

    [<Theory>]
    let ``[Tensor4] Map`` (tensor: TensorKind) (innerW: bool) =
        for _ = 1 to 100 do
            let a = TensorGeneration.getIntTensor4 tensor innerW
            let b = TensorGeneration.rnd.UniformInt 100
            let c = Tensor4.map ((+) b) a

            let ref = a.Elements |> Seq.map ((+) b) |> HashSet.ofSeq

            for x in c.Elements do
                ref |> HashSet.remove x |> should be True

            ref.Count |> should equal 0

    [<Theory>]
    let ``[Tensor4] Map2`` (tensor: TensorKind) (innerW0: bool) (innerW1: bool) =
        for _ = 1 to 100 do
            let a = TensorGeneration.getIntTensor4 tensor innerW0
            let b = TensorGeneration.getIntTensor4OfSize tensor innerW1 a.Size
            let c = Tensor4.map2 (+) a b

            let ref = ReferenceCountingSet()
            a.ForeachIndex(b.Info, fun i0 i1 ->
                ref.Add (a.Data[int i0] + b.Data[int i1]) |> ignore
            )

            for x in c.Elements do
                ref.Contains x |> should be True
                ref.Remove x |> ignore

            ref.Count |> should equal 0