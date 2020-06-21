namespace Aardvark.Geometry

open System
open Microsoft.FSharp.NativeInterop
open Aardvark.Base
open System.Runtime.CompilerServices

#nowarn "9"
#nowarn "51"

[<AutoOpen>]
module private rec D9Math =
    [<Struct>]
    type V9d = 
        val mutable public C0 : float
        val mutable public C1 : float
        val mutable public C2 : float
        val mutable public C3 : float
        val mutable public C4 : float
        val mutable public C5 : float
        val mutable public C6 : float
        val mutable public C7 : float
        val mutable public C8 : float

        member x.MultiplyAdd(v : V9d, f : float) =
            x.C0 <- x.C0 + v.C0 * f
            x.C1 <- x.C1 + v.C1 * f
            x.C2 <- x.C2 + v.C2 * f
            x.C3 <- x.C3 + v.C3 * f
            x.C4 <- x.C4 + v.C4 * f
            x.C5 <- x.C5 + v.C5 * f
            x.C6 <- x.C6 + v.C6 * f
            x.C7 <- x.C7 + v.C7 * f
            x.C8 <- x.C8 + v.C8 * f

        member internal x.ReadUnsafe(i : int) : float =
            NativePtr.get (NativePtr.cast &&x) i 
            
        member internal x.WriteUnsafe(i : int, v : float)  =
            NativePtr.set (NativePtr.cast &&x) i v
            

        member x.SetZero() =
            x.C0 <- 0.0
            x.C1 <- 0.0
            x.C2 <- 0.0
            x.C3 <- 0.0
            x.C4 <- 0.0
            x.C5 <- 0.0
            x.C6 <- 0.0
            x.C7 <- 0.0
            x.C8 <- 0.0

        member x.ToVector() = Vector([| x.C0; x.C1; x.C2; x.C3; x.C4; x.C5; x.C6; x.C7; x.C8 |], 9L)

        new(c0 : float, c1 : float, c2 : float, c3 : float, c4 : float, c5 : float, c6 : float, c7 : float, c8 : float) = { C0 = c0; C1 = c1; C2 = c2; C3 = c3; C4 = c4; C5 = c5; C6 = c6; C7 = c7; C8 = c8 }

    [<Struct>]
    type SymmetricM99d = 
        val mutable public M00 : float
        val mutable public M01 : float
        val mutable public M02 : float
        val mutable public M03 : float
        val mutable public M04 : float
        val mutable public M05 : float
        val mutable public M06 : float
        val mutable public M07 : float
        val mutable public M08 : float
        val mutable public M11 : float
        val mutable public M12 : float
        val mutable public M13 : float
        val mutable public M14 : float
        val mutable public M15 : float
        val mutable public M16 : float
        val mutable public M17 : float
        val mutable public M18 : float
        val mutable public M22 : float
        val mutable public M23 : float
        val mutable public M24 : float
        val mutable public M25 : float
        val mutable public M26 : float
        val mutable public M27 : float
        val mutable public M28 : float
        val mutable public M33 : float
        val mutable public M34 : float
        val mutable public M35 : float
        val mutable public M36 : float
        val mutable public M37 : float
        val mutable public M38 : float
        val mutable public M44 : float
        val mutable public M45 : float
        val mutable public M46 : float
        val mutable public M47 : float
        val mutable public M48 : float
        val mutable public M55 : float
        val mutable public M56 : float
        val mutable public M57 : float
        val mutable public M58 : float
        val mutable public M66 : float
        val mutable public M67 : float
        val mutable public M68 : float
        val mutable public M77 : float
        val mutable public M78 : float
        val mutable public M88 : float
        member inline x.M10 = x.M01
        member inline x.M20 = x.M02
        member inline x.M21 = x.M12
        member inline x.M30 = x.M03
        member inline x.M31 = x.M13
        member inline x.M32 = x.M23
        member inline x.M40 = x.M04
        member inline x.M41 = x.M14
        member inline x.M42 = x.M24
        member inline x.M43 = x.M34
        member inline x.M50 = x.M05
        member inline x.M51 = x.M15
        member inline x.M52 = x.M25
        member inline x.M53 = x.M35
        member inline x.M54 = x.M45
        member inline x.M60 = x.M06
        member inline x.M61 = x.M16
        member inline x.M62 = x.M26
        member inline x.M63 = x.M36
        member inline x.M64 = x.M46
        member inline x.M65 = x.M56
        member inline x.M70 = x.M07
        member inline x.M71 = x.M17
        member inline x.M72 = x.M27
        member inline x.M73 = x.M37
        member inline x.M74 = x.M47
        member inline x.M75 = x.M57
        member inline x.M76 = x.M67
        member inline x.M80 = x.M08
        member inline x.M81 = x.M18
        member inline x.M82 = x.M28
        member inline x.M83 = x.M38
        member inline x.M84 = x.M48
        member inline x.M85 = x.M58
        member inline x.M86 = x.M68
        member inline x.M87 = x.M78
        static member Zero = SymmetricM99d(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0)
        
        member x.AddOuterProduct(v : V9d) =
            x.M00 <- x.M00 + v.C0*v.C0
            x.M01 <- x.M01 + v.C0*v.C1
            x.M02 <- x.M02 + v.C0*v.C2
            x.M03 <- x.M03 + v.C0*v.C3
            x.M04 <- x.M04 + v.C0*v.C4
            x.M05 <- x.M05 + v.C0*v.C5
            x.M06 <- x.M06 + v.C0*v.C6
            x.M07 <- x.M07 + v.C0*v.C7
            x.M08 <- x.M08 + v.C0*v.C8
            x.M11 <- x.M11 + v.C1*v.C1
            x.M12 <- x.M12 + v.C1*v.C2
            x.M13 <- x.M13 + v.C1*v.C3
            x.M14 <- x.M14 + v.C1*v.C4
            x.M15 <- x.M15 + v.C1*v.C5
            x.M16 <- x.M16 + v.C1*v.C6
            x.M17 <- x.M17 + v.C1*v.C7
            x.M18 <- x.M18 + v.C1*v.C8
            x.M22 <- x.M22 + v.C2*v.C2
            x.M23 <- x.M23 + v.C2*v.C3
            x.M24 <- x.M24 + v.C2*v.C4
            x.M25 <- x.M25 + v.C2*v.C5
            x.M26 <- x.M26 + v.C2*v.C6
            x.M27 <- x.M27 + v.C2*v.C7
            x.M28 <- x.M28 + v.C2*v.C8
            x.M33 <- x.M33 + v.C3*v.C3
            x.M34 <- x.M34 + v.C3*v.C4
            x.M35 <- x.M35 + v.C3*v.C5
            x.M36 <- x.M36 + v.C3*v.C6
            x.M37 <- x.M37 + v.C3*v.C7
            x.M38 <- x.M38 + v.C3*v.C8
            x.M44 <- x.M44 + v.C4*v.C4
            x.M45 <- x.M45 + v.C4*v.C5
            x.M46 <- x.M46 + v.C4*v.C6
            x.M47 <- x.M47 + v.C4*v.C7
            x.M48 <- x.M48 + v.C4*v.C8
            x.M55 <- x.M55 + v.C5*v.C5
            x.M56 <- x.M56 + v.C5*v.C6
            x.M57 <- x.M57 + v.C5*v.C7
            x.M58 <- x.M58 + v.C5*v.C8
            x.M66 <- x.M66 + v.C6*v.C6
            x.M67 <- x.M67 + v.C6*v.C7
            x.M68 <- x.M68 + v.C6*v.C8
            x.M77 <- x.M77 + v.C7*v.C7
            x.M78 <- x.M78 + v.C7*v.C8
            x.M88 <- x.M88 + v.C8*v.C8
        
        member x.SubOuterProduct(v : V9d) =
            x.M00 <- x.M00 - v.C0*v.C0
            x.M01 <- x.M01 - v.C0*v.C1
            x.M02 <- x.M02 - v.C0*v.C2
            x.M03 <- x.M03 - v.C0*v.C3
            x.M04 <- x.M04 - v.C0*v.C4
            x.M05 <- x.M05 - v.C0*v.C5
            x.M06 <- x.M06 - v.C0*v.C6
            x.M07 <- x.M07 - v.C0*v.C7
            x.M08 <- x.M08 - v.C0*v.C8
            x.M11 <- x.M11 - v.C1*v.C1
            x.M12 <- x.M12 - v.C1*v.C2
            x.M13 <- x.M13 - v.C1*v.C3
            x.M14 <- x.M14 - v.C1*v.C4
            x.M15 <- x.M15 - v.C1*v.C5
            x.M16 <- x.M16 - v.C1*v.C6
            x.M17 <- x.M17 - v.C1*v.C7
            x.M18 <- x.M18 - v.C1*v.C8
            x.M22 <- x.M22 - v.C2*v.C2
            x.M23 <- x.M23 - v.C2*v.C3
            x.M24 <- x.M24 - v.C2*v.C4
            x.M25 <- x.M25 - v.C2*v.C5
            x.M26 <- x.M26 - v.C2*v.C6
            x.M27 <- x.M27 - v.C2*v.C7
            x.M28 <- x.M28 - v.C2*v.C8
            x.M33 <- x.M33 - v.C3*v.C3
            x.M34 <- x.M34 - v.C3*v.C4
            x.M35 <- x.M35 - v.C3*v.C5
            x.M36 <- x.M36 - v.C3*v.C6
            x.M37 <- x.M37 - v.C3*v.C7
            x.M38 <- x.M38 - v.C3*v.C8
            x.M44 <- x.M44 - v.C4*v.C4
            x.M45 <- x.M45 - v.C4*v.C5
            x.M46 <- x.M46 - v.C4*v.C6
            x.M47 <- x.M47 - v.C4*v.C7
            x.M48 <- x.M48 - v.C4*v.C8
            x.M55 <- x.M55 - v.C5*v.C5
            x.M56 <- x.M56 - v.C5*v.C6
            x.M57 <- x.M57 - v.C5*v.C7
            x.M58 <- x.M58 - v.C5*v.C8
            x.M66 <- x.M66 - v.C6*v.C6
            x.M67 <- x.M67 - v.C6*v.C7
            x.M68 <- x.M68 - v.C6*v.C8
            x.M77 <- x.M77 - v.C7*v.C7
            x.M78 <- x.M78 - v.C7*v.C8
            x.M88 <- x.M88 - v.C8*v.C8
        
        static member FromMatrix(m : Matrix<float>) =
            if m.SX < 9L || m.SY < 9L then raise <| IndexOutOfRangeException()
            SymmetricM99d(m.[0,0], m.[0,1], m.[0,2], m.[0,3], m.[0,4], m.[0,5], m.[0,6], m.[0,7], m.[0,8], m.[1,1], m.[1,2], m.[1,3], m.[1,4], m.[1,5], m.[1,6], m.[1,7], m.[1,8], m.[2,2], m.[2,3], m.[2,4], m.[2,5], m.[2,6], m.[2,7], m.[2,8], m.[3,3], m.[3,4], m.[3,5], m.[3,6], m.[3,7], m.[3,8], m.[4,4], m.[4,5], m.[4,6], m.[4,7], m.[4,8], m.[5,5], m.[5,6], m.[5,7], m.[5,8], m.[6,6], m.[6,7], m.[6,8], m.[7,7], m.[7,8], m.[8,8])

        [<MethodImpl(MethodImplOptions.NoInlining)>]
        member x.LuSolve(rhs : V9d) =
            let mutable rhs = rhs

            let ptr = NativePtr.stackalloc<V9d> 9
            NativePtr.set ptr 0 (V9d(x.M00, x.M01, x.M02, x.M03, x.M04, x.M05, x.M06, x.M07, x.M08))
            NativePtr.set ptr 1 (V9d(x.M10, x.M11, x.M12, x.M13, x.M14, x.M15, x.M16, x.M17, x.M18))
            NativePtr.set ptr 2 (V9d(x.M20, x.M21, x.M22, x.M23, x.M24, x.M25, x.M26, x.M27, x.M28))
            NativePtr.set ptr 3 (V9d(x.M30, x.M31, x.M32, x.M33, x.M34, x.M35, x.M36, x.M37, x.M38))
            NativePtr.set ptr 4 (V9d(x.M40, x.M41, x.M42, x.M43, x.M44, x.M45, x.M46, x.M47, x.M48))
            NativePtr.set ptr 5 (V9d(x.M50, x.M51, x.M52, x.M53, x.M54, x.M55, x.M56, x.M57, x.M58))
            NativePtr.set ptr 6 (V9d(x.M60, x.M61, x.M62, x.M63, x.M64, x.M65, x.M66, x.M67, x.M68))
            NativePtr.set ptr 7 (V9d(x.M70, x.M71, x.M72, x.M73, x.M74, x.M75, x.M76, x.M77, x.M78))
            NativePtr.set ptr 8 (V9d(x.M80, x.M81, x.M82, x.M83, x.M84, x.M85, x.M86, x.M87, x.M88))
            let pFloat : nativeptr<float> = NativePtr.cast ptr
            
            let inline get (r : int) (c : int) =
                NativePtr.get pFloat (r * 9 + c)

            let inline set (r : int) (c : int) (v : float) =
                NativePtr.set pFloat (r * 9 + c) v
                
            let inline swapRows (ri : int) (rj : int) =
                if ri <> rj then
                    for c in 0 .. 8 do
                        let t = get ri c
                        set ri c (get rj c)
                        set rj c t

                    let t = rhs.ReadUnsafe ri
                    rhs.WriteUnsafe(ri, rhs.ReadUnsafe rj)
                    rhs.WriteUnsafe(rj, t)


            for i in 0 .. 8 do
                // pivoting
                let vii = 
                    let mutable j = i
                    let mutable vmax = get i i
                    for ii in i+1 .. 8 do
                        let vii = get ii i
                        if abs vii > abs vmax then
                            j <- ii
                            vmax <- vii

                    swapRows i j
                    vmax

                // elimination
                if not (Fun.IsTiny vii) then
                    for j in i + 1 .. 8 do
                        let vji = get j i
                        let f = -vji / vii

                        rhs.WriteUnsafe(j, rhs.ReadUnsafe j + rhs.ReadUnsafe i * f)

                        set j i 5.0
                        for c in i + 1 .. 8 do
                            set j c (get j c + get i c * f) 


            // back substitution
            let mutable res = V9d()
            for ri in 0 .. 8 do
                let ri = 8 - ri

                let mutable s = rhs.ReadUnsafe(ri)
                for c in ri + 1 .. 8 do
                    s <- s - get ri c * res.ReadUnsafe(c)

                res.WriteUnsafe(ri, s / get ri ri)


            res


        member x.ToMatrix() =
            let arr = Array.zeroCreate 81
            arr.[0] <- x.M00
            arr.[1] <- x.M01
            arr.[2] <- x.M02
            arr.[3] <- x.M03
            arr.[4] <- x.M04
            arr.[5] <- x.M05
            arr.[6] <- x.M06
            arr.[7] <- x.M07
            arr.[8] <- x.M08
            arr.[9] <- x.M01
            arr.[10] <- x.M11
            arr.[11] <- x.M12
            arr.[12] <- x.M13
            arr.[13] <- x.M14
            arr.[14] <- x.M15
            arr.[15] <- x.M16
            arr.[16] <- x.M17
            arr.[17] <- x.M18
            arr.[18] <- x.M02
            arr.[19] <- x.M12
            arr.[20] <- x.M22
            arr.[21] <- x.M23
            arr.[22] <- x.M24
            arr.[23] <- x.M25
            arr.[24] <- x.M26
            arr.[25] <- x.M27
            arr.[26] <- x.M28
            arr.[27] <- x.M03
            arr.[28] <- x.M13
            arr.[29] <- x.M23
            arr.[30] <- x.M33
            arr.[31] <- x.M34
            arr.[32] <- x.M35
            arr.[33] <- x.M36
            arr.[34] <- x.M37
            arr.[35] <- x.M38
            arr.[36] <- x.M04
            arr.[37] <- x.M14
            arr.[38] <- x.M24
            arr.[39] <- x.M34
            arr.[40] <- x.M44
            arr.[41] <- x.M45
            arr.[42] <- x.M46
            arr.[43] <- x.M47
            arr.[44] <- x.M48
            arr.[45] <- x.M05
            arr.[46] <- x.M15
            arr.[47] <- x.M25
            arr.[48] <- x.M35
            arr.[49] <- x.M45
            arr.[50] <- x.M55
            arr.[51] <- x.M56
            arr.[52] <- x.M57
            arr.[53] <- x.M58
            arr.[54] <- x.M06
            arr.[55] <- x.M16
            arr.[56] <- x.M26
            arr.[57] <- x.M36
            arr.[58] <- x.M46
            arr.[59] <- x.M56
            arr.[60] <- x.M66
            arr.[61] <- x.M67
            arr.[62] <- x.M68
            arr.[63] <- x.M07
            arr.[64] <- x.M17
            arr.[65] <- x.M27
            arr.[66] <- x.M37
            arr.[67] <- x.M47
            arr.[68] <- x.M57
            arr.[69] <- x.M67
            arr.[70] <- x.M77
            arr.[71] <- x.M78
            arr.[72] <- x.M08
            arr.[73] <- x.M18
            arr.[74] <- x.M28
            arr.[75] <- x.M38
            arr.[76] <- x.M48
            arr.[77] <- x.M58
            arr.[78] <- x.M68
            arr.[79] <- x.M78
            arr.[80] <- x.M88
            Matrix(arr, V2l(9L, 9L))

        member x.SetZero() =
            x.M00 <- 0.0
            x.M01 <- 0.0
            x.M02 <- 0.0
            x.M03 <- 0.0
            x.M04 <- 0.0
            x.M05 <- 0.0
            x.M06 <- 0.0
            x.M07 <- 0.0
            x.M08 <- 0.0
            x.M11 <- 0.0
            x.M12 <- 0.0
            x.M13 <- 0.0
            x.M14 <- 0.0
            x.M15 <- 0.0
            x.M16 <- 0.0
            x.M17 <- 0.0
            x.M18 <- 0.0
            x.M22 <- 0.0
            x.M23 <- 0.0
            x.M24 <- 0.0
            x.M25 <- 0.0
            x.M26 <- 0.0
            x.M27 <- 0.0
            x.M28 <- 0.0
            x.M33 <- 0.0
            x.M34 <- 0.0
            x.M35 <- 0.0
            x.M36 <- 0.0
            x.M37 <- 0.0
            x.M38 <- 0.0
            x.M44 <- 0.0
            x.M45 <- 0.0
            x.M46 <- 0.0
            x.M47 <- 0.0
            x.M48 <- 0.0
            x.M55 <- 0.0
            x.M56 <- 0.0
            x.M57 <- 0.0
            x.M58 <- 0.0
            x.M66 <- 0.0
            x.M67 <- 0.0
            x.M68 <- 0.0
            x.M77 <- 0.0
            x.M78 <- 0.0
            x.M88 <- 0.0

        new(m00 : float, m01 : float, m02 : float, m03 : float, m04 : float, m05 : float, m06 : float, m07 : float, m08 : float, m11 : float, m12 : float, m13 : float, m14 : float, m15 : float, m16 : float, m17 : float, m18 : float, m22 : float, m23 : float, m24 : float, m25 : float, m26 : float, m27 : float, m28 : float, m33 : float, m34 : float, m35 : float, m36 : float, m37 : float, m38 : float, m44 : float, m45 : float, m46 : float, m47 : float, m48 : float, m55 : float, m56 : float, m57 : float, m58 : float, m66 : float, m67 : float, m68 : float, m77 : float, m78 : float, m88 : float) = { M00 = m00; M01 = m01; M02 = m02; M03 = m03; M04 = m04; M05 = m05; M06 = m06; M07 = m07; M08 = m08; M11 = m11; M12 = m12; M13 = m13; M14 = m14; M15 = m15; M16 = m16; M17 = m17; M18 = m18; M22 = m22; M23 = m23; M24 = m24; M25 = m25; M26 = m26; M27 = m27; M28 = m28; M33 = m33; M34 = m34; M35 = m35; M36 = m36; M37 = m37; M38 = m38; M44 = m44; M45 = m45; M46 = m46; M47 = m47; M48 = m48; M55 = m55; M56 = m56; M57 = m57; M58 = m58; M66 = m66; M67 = m67; M68 = m68; M77 = m77; M78 = m78; M88 = m88 }

[<Struct>]
type Ellipsoid3d (euclidean : Euclidean3d, radii : V3d) =
    member x.Euclidean = euclidean
    member x.Radii = radii
    member x.Center = euclidean.Trans

    member x.BoundingBox3d =
        // TODO: tighter BB possible?
        let m : M44d = Euclidean3d.op_Explicit euclidean
        Box3d(-radii, radii).Transformed m

    member x.Contains(pt : V3d) =
        Vec.length (euclidean.InvTransformPos pt / radii) <= 1.0

    member x.GetClosestPoint(pt : V3d) =
        Vec.normalize (euclidean.InvTransformPos pt / radii) * radii |> euclidean.TransformPos

    member x.Distance(pt : V3d) =
        let c = Vec.normalize (euclidean.InvTransformPos pt / radii) * radii |> euclidean.TransformPos
        Vec.distance c pt

    member x.Height(pt : V3d) =
        let n = Vec.normalize (euclidean.InvTransformPos pt / radii) * radii |> euclidean.TransformDir
        let c = n + euclidean.Trans
        Vec.dot (pt - c) (Vec.normalize n)

    member x.SphereTrafo =
        Trafo3d.Scale radii * Trafo3d euclidean

[<Struct>]
type EllipsoidRegression3d =

    val mutable private _reference : V3d
    val mutable private _lhs : SymmetricM99d
    val mutable private _rhs : V9d
    val mutable private _count : int
    
    private new(reference : V3d, lhs : SymmetricM99d, rhs : V9d, count : int) =
        { _reference = reference; _lhs = lhs; _rhs = rhs; _count = count}

    new([<ParamArray>] pts : V3d[]) =
        if pts.Length <= 0 then
            { _reference = V3d.Zero; _lhs = SymmetricM99d(); _rhs = V9d(); _count = 0 }
        else

            let reference = pts.[0]
            let mutable lhs = SymmetricM99d()
            let mutable rhs = V9d()
            let mutable count = 1
            for i in 1 .. pts.Length - 1 do
                let pt = pts.[i] - reference
                let px = pt.X
                let py = pt.Y
                let pz = pt.Z
            
                let a = sqr px + sqr py - 2.0*sqr pz
                let b = sqr px + sqr pz - 2.0*sqr py
                let c = 2.0 * px * py
                let d = 2.0 * px * pz
                let e = 2.0 * py * pz
                let f = 2.0 * px
                let g = 2.0 * py
                let h = 2.0 * pz
                let j = 1.0
                let v = V9d(a, b, c, d, e, f, g, h, j)
                lhs.AddOuterProduct v
                rhs.MultiplyAdd(v, sqr px + sqr py + sqr pz)
                count <- count + 1

            { _reference = reference; _lhs = lhs; _rhs = rhs; _count = count }

    new(pts : seq<V3d>) = EllipsoidRegression3d(Seq.toArray pts)
    new(pts : list<V3d>) = EllipsoidRegression3d(List.toArray pts)

    static member Empty = 
        EllipsoidRegression3d(V3d.Zero, SymmetricM99d(), V9d(), 0)

    member x.Count = x._count

    member x.Add(pt : V3d) =
        if x._count <= 0 then
            EllipsoidRegression3d(pt, D9Math.SymmetricM99d(), D9Math.V9d(), 1)
        else
            let pt = pt - x._reference
            let px = pt.X
            let py = pt.Y
            let pz = pt.Z

            let a = sqr px + sqr py - 2.0*sqr pz
            let b = sqr px + sqr pz - 2.0*sqr py
            let c = 2.0 * px * py
            let d = 2.0 * px * pz
            let e = 2.0 * py * pz
            let f = 2.0 * px
            let g = 2.0 * py
            let h = 2.0 * pz
            let j = 1.0

            let mutable lhs = x._lhs
            let mutable rhs = x._rhs
            let v = V9d(a, b, c, d, e, f, g, h, j)
            lhs.AddOuterProduct v
            rhs.MultiplyAdd(v, sqr px + sqr py + sqr pz)
            EllipsoidRegression3d(x._reference, lhs, rhs, x._count + 1)
       
    member x.AddInPlace(pt : V3d) =
        if x._count <= 0 then
            x._reference <- pt
            x._lhs.SetZero()
            x._rhs.SetZero()
            x._count <- 1
        else
            let pt = pt - x._reference
            let px = pt.X
            let py = pt.Y
            let pz = pt.Z
            
            let a = sqr px + sqr py - 2.0*sqr pz
            let b = sqr px + sqr pz - 2.0*sqr py
            let c = 2.0 * px * py
            let d = 2.0 * px * pz
            let e = 2.0 * py * pz
            let f = 2.0 * px
            let g = 2.0 * py
            let h = 2.0 * pz
            let j = 1.0
            let v = V9d(a, b, c, d, e, f, g, h, j)
            x._lhs.AddOuterProduct v
            x._rhs.MultiplyAdd(v, sqr px + sqr py + sqr pz)
            x._count <- x._count + 1

    member x.Remove(pt : V3d) =
        if x._count <= 1 then
            EllipsoidRegression3d.Empty
        else
            let pt = pt - x._reference
            let px = pt.X
            let py = pt.Y
            let pz = pt.Z

            let a = sqr px + sqr py - 2.0*sqr pz
            let b = sqr px + sqr pz - 2.0*sqr py
            let c = 2.0 * px * py
            let d = 2.0 * px * pz
            let e = 2.0 * py * pz
            let f = 2.0 * px
            let g = 2.0 * py
            let h = 2.0 * pz
            let j = 1.0

            let mutable lhs = x._lhs
            let mutable rhs = x._rhs
            let v = V9d(a, b, c, d, e, f, g, h, j)
            lhs.SubOuterProduct v
            rhs.MultiplyAdd(v, -(sqr px + sqr py + sqr pz))
            EllipsoidRegression3d(x._reference, lhs, rhs, x._count - 1)

    member x.RemoveInPlace(pt : V3d) =
        if x._count <= 1 then
            x._lhs.SetZero()
            x._rhs.SetZero()
            x._reference <- V3d.Zero
            x._count <- 0
        else
            let pt = pt - x._reference
            let px = pt.X
            let py = pt.Y
            let pz = pt.Z
            
            let a = sqr px + sqr py - 2.0*sqr pz
            let b = sqr px + sqr pz - 2.0*sqr py
            let c = 2.0 * px * py
            let d = 2.0 * px * pz
            let e = 2.0 * py * pz
            let f = 2.0 * px
            let g = 2.0 * py
            let h = 2.0 * pz
            let j = 1.0
            let v = V9d(a, b, c, d, e, f, g, h, j)
            x._lhs.SubOuterProduct v
            x._rhs.MultiplyAdd(v, -(sqr px + sqr py + sqr pz))
            x._count <- x._count - 1

    member x.GetEllipsoid() =
        if x._count < 9 then   
            Ellipsoid3d(Euclidean3d.Identity, V3d.Zero)
        else
            let u = x._lhs.LuSolve x._rhs

            let a = u.C0 + u.C1 - 1.0
            let b = u.C0 - 2.0*u.C1 - 1.0
            let c = u.C1 - 2.0*u.C0 - 1.0

            let A = 
                M44d(
                    a,      u.C2,   u.C3,   u.C5,
                    u.C2,   b,      u.C4,   u.C6,
                    u.C3,   u.C4,   c,      u.C7,
                    u.C5,   u.C6,   u.C7,   u.C8
                )


            let lhs = -A.UpperLeftM33()
            let rhs = V3d(u.C5, u.C6, u.C7)
            let center = lhs.Inverse * rhs

            let transMat =
                M44d.FromCols(V4d.IOOO, V4d.OIOO, V4d.OOIO, V4d(center, 1.0))

            let R = transMat.Transposed * A * transMat

            match SVD.Decompose (R.UpperLeftM33() / -R.M33) with
            | Some(u, s, _vt) ->
                let evals = s.Diagonal
                let r = sqrt (1.0 / evals.Abs())
                //let r = r * (V3d (evals.Sign()))

                let trafo = Euclidean3d.FromM33dAndV3d(u, x._reference + center)
                let scale = r
                Ellipsoid3d(trafo, scale)
                //Trafo3d.Scale(r) * Trafo3d.FromOrthoNormalBasis(u.C0, u.C1, u.C2) * Trafo3d.Translation (reference + center)
            | None ->
                Ellipsoid3d(Euclidean3d.Identity, V3d.Zero)

module EllipsoidRegression3d =

    /// A regression holding no points.
    let empty = EllipsoidRegression3d.Empty

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    let inline remove (pt : V3d) (s : EllipsoidRegression3d) = s.Remove pt

    /// Adds the given point to the regression.
    let inline add (pt : V3d) (s : EllipsoidRegression3d) = s.Add pt

    /// Gets the least-squares ellipsoid.
    let inline getEllipsoid (s : EllipsoidRegression3d) = s.GetEllipsoid()

    /// The total number of points added. Note that at least 9 points are required to fit an ellipsoid.
    let inline count (s : EllipsoidRegression3d) = s.Count
    
    /// Creates a regression from the given points.
    let ofSeq (points : seq<V3d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
        
    /// Creates a regression from the given points.
    let ofList (points : list<V3d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
        
    /// Creates a regression from the given points.
    let ofArray (points : V3d[]) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r