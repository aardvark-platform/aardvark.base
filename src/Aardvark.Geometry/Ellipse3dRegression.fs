namespace Aardvark.Geometry

open System
open Aardvark.Base

[<AutoOpen>]
module private Tensor3dHelpers =

    /// Symmetric rank-2 tensor in 3D (i.e. 3×3 symmetric matrix) — 6 unique entries.
    [<Struct>]
    type SymmetricMatrix3 =
        val mutable public M00 : float
        val mutable public M01 : float
        val mutable public M02 : float
        val mutable public M11 : float
        val mutable public M12 : float
        val mutable public M22 : float

        static member Zero = SymmetricMatrix3()

        member x.AddOuter(p : V3d) =
            x.M00 <- x.M00 + p.X * p.X
            x.M01 <- x.M01 + p.X * p.Y
            x.M02 <- x.M02 + p.X * p.Z
            x.M11 <- x.M11 + p.Y * p.Y
            x.M12 <- x.M12 + p.Y * p.Z
            x.M22 <- x.M22 + p.Z * p.Z

        member x.SubOuter(p : V3d) =
            x.M00 <- x.M00 - p.X * p.X
            x.M01 <- x.M01 - p.X * p.Y
            x.M02 <- x.M02 - p.X * p.Z
            x.M11 <- x.M11 - p.Y * p.Y
            x.M12 <- x.M12 - p.Y * p.Z
            x.M22 <- x.M22 - p.Z * p.Z

        member x.SetZero() =
            x.M00 <- 0.0; x.M01 <- 0.0; x.M02 <- 0.0
            x.M11 <- 0.0; x.M12 <- 0.0; x.M22 <- 0.0

        member x.Get(i : int, j : int) =
            let a = min i j
            let b = max i j
            match a, b with
            | 0, 0 -> x.M00
            | 0, 1 -> x.M01
            | 0, 2 -> x.M02
            | 1, 1 -> x.M11
            | 1, 2 -> x.M12
            | _    -> x.M22

        member x.ToM33d() =
            M33d(x.M00, x.M01, x.M02,
                 x.M01, x.M11, x.M12,
                 x.M02, x.M12, x.M22)


    /// Symmetric rank-3 tensor in 3D — 10 unique entries.
    /// Field names use (i,j,k) sorted with i ≤ j ≤ k.
    [<Struct>]
    type SymTensor3 =
        val mutable public Txxx : float
        val mutable public Txxy : float
        val mutable public Txxz : float
        val mutable public Txyy : float
        val mutable public Txyz : float
        val mutable public Txzz : float
        val mutable public Tyyy : float
        val mutable public Tyyz : float
        val mutable public Tyzz : float
        val mutable public Tzzz : float

        static member Zero = SymTensor3()

        /// Add p ⊗ p ⊗ p (symmetric outer product) to the tensor.
        member t.AddOuter(p : V3d) =
            let x, y, z = p.X, p.Y, p.Z
            t.Txxx <- t.Txxx + x*x*x
            t.Txxy <- t.Txxy + x*x*y
            t.Txxz <- t.Txxz + x*x*z
            t.Txyy <- t.Txyy + x*y*y
            t.Txyz <- t.Txyz + x*y*z
            t.Txzz <- t.Txzz + x*z*z
            t.Tyyy <- t.Tyyy + y*y*y
            t.Tyyz <- t.Tyyz + y*y*z
            t.Tyzz <- t.Tyzz + y*z*z
            t.Tzzz <- t.Tzzz + z*z*z

        member t.SubOuter(p : V3d) =
            let x, y, z = p.X, p.Y, p.Z
            t.Txxx <- t.Txxx - x*x*x
            t.Txxy <- t.Txxy - x*x*y
            t.Txxz <- t.Txxz - x*x*z
            t.Txyy <- t.Txyy - x*y*y
            t.Txyz <- t.Txyz - x*y*z
            t.Txzz <- t.Txzz - x*z*z
            t.Tyyy <- t.Tyyy - y*y*y
            t.Tyyz <- t.Tyyz - y*y*z
            t.Tyzz <- t.Tyzz - y*z*z
            t.Tzzz <- t.Tzzz - z*z*z

        member t.SetZero() =
            t.Txxx <- 0.0; t.Txxy <- 0.0; t.Txxz <- 0.0
            t.Txyy <- 0.0; t.Txyz <- 0.0; t.Txzz <- 0.0
            t.Tyyy <- 0.0; t.Tyyz <- 0.0; t.Tyzz <- 0.0
            t.Tzzz <- 0.0

        /// T[i,j,k] for any (possibly unsorted) index triple.
        member t.Get(i : int, j : int, k : int) =
            // sort i ≤ j ≤ k
            let a, b, c =
                let aa = min (min i j) k
                let cc = max (max i j) k
                aa, i + j + k - aa - cc, cc
            match a, b, c with
            | 0, 0, 0 -> t.Txxx
            | 0, 0, 1 -> t.Txxy
            | 0, 0, 2 -> t.Txxz
            | 0, 1, 1 -> t.Txyy
            | 0, 1, 2 -> t.Txyz
            | 0, 2, 2 -> t.Txzz
            | 1, 1, 1 -> t.Tyyy
            | 1, 1, 2 -> t.Tyyz
            | 1, 2, 2 -> t.Tyzz
            | _       -> t.Tzzz

        /// Triple contraction T : a ⊗ b ⊗ c.
        member t.Contract(a : V3d, b : V3d, c : V3d) =
            let inline ai i = match i with | 0 -> a.X | 1 -> a.Y | _ -> a.Z
            let inline bj j = match j with | 0 -> b.X | 1 -> b.Y | _ -> b.Z
            let inline ck k = match k with | 0 -> c.X | 1 -> c.Y | _ -> c.Z
            let mutable s = 0.0
            for i in 0..2 do
                for j in 0..2 do
                    for k in 0..2 do
                        s <- s + ai i * bj j * ck k * t.Get(i, j, k)
            s


    /// Symmetric rank-4 tensor in 3D — 15 unique entries.
    [<Struct>]
    type SymTensor4 =
        val mutable public Txxxx : float
        val mutable public Txxxy : float
        val mutable public Txxxz : float
        val mutable public Txxyy : float
        val mutable public Txxyz : float
        val mutable public Txxzz : float
        val mutable public Txyyy : float
        val mutable public Txyyz : float
        val mutable public Txyzz : float
        val mutable public Txzzz : float
        val mutable public Tyyyy : float
        val mutable public Tyyyz : float
        val mutable public Tyyzz : float
        val mutable public Tyzzz : float
        val mutable public Tzzzz : float

        static member Zero = SymTensor4()

        member t.AddOuter(p : V3d) =
            let x, y, z = p.X, p.Y, p.Z
            t.Txxxx <- t.Txxxx + x*x*x*x
            t.Txxxy <- t.Txxxy + x*x*x*y
            t.Txxxz <- t.Txxxz + x*x*x*z
            t.Txxyy <- t.Txxyy + x*x*y*y
            t.Txxyz <- t.Txxyz + x*x*y*z
            t.Txxzz <- t.Txxzz + x*x*z*z
            t.Txyyy <- t.Txyyy + x*y*y*y
            t.Txyyz <- t.Txyyz + x*y*y*z
            t.Txyzz <- t.Txyzz + x*y*z*z
            t.Txzzz <- t.Txzzz + x*z*z*z
            t.Tyyyy <- t.Tyyyy + y*y*y*y
            t.Tyyyz <- t.Tyyyz + y*y*y*z
            t.Tyyzz <- t.Tyyzz + y*y*z*z
            t.Tyzzz <- t.Tyzzz + y*z*z*z
            t.Tzzzz <- t.Tzzzz + z*z*z*z

        member t.SubOuter(p : V3d) =
            let x, y, z = p.X, p.Y, p.Z
            t.Txxxx <- t.Txxxx - x*x*x*x
            t.Txxxy <- t.Txxxy - x*x*x*y
            t.Txxxz <- t.Txxxz - x*x*x*z
            t.Txxyy <- t.Txxyy - x*x*y*y
            t.Txxyz <- t.Txxyz - x*x*y*z
            t.Txxzz <- t.Txxzz - x*x*z*z
            t.Txyyy <- t.Txyyy - x*y*y*y
            t.Txyyz <- t.Txyyz - x*y*y*z
            t.Txyzz <- t.Txyzz - x*y*z*z
            t.Txzzz <- t.Txzzz - x*z*z*z
            t.Tyyyy <- t.Tyyyy - y*y*y*y
            t.Tyyyz <- t.Tyyyz - y*y*y*z
            t.Tyyzz <- t.Tyyzz - y*y*z*z
            t.Tyzzz <- t.Tyzzz - y*z*z*z
            t.Tzzzz <- t.Tzzzz - z*z*z*z

        member t.SetZero() =
            t.Txxxx <- 0.0; t.Txxxy <- 0.0; t.Txxxz <- 0.0
            t.Txxyy <- 0.0; t.Txxyz <- 0.0; t.Txxzz <- 0.0
            t.Txyyy <- 0.0; t.Txyyz <- 0.0; t.Txyzz <- 0.0
            t.Txzzz <- 0.0
            t.Tyyyy <- 0.0; t.Tyyyz <- 0.0; t.Tyyzz <- 0.0
            t.Tyzzz <- 0.0
            t.Tzzzz <- 0.0

        member t.Get(i : int, j : int, k : int, l : int) =
            // sort indices ascending
            let a = [| i; j; k; l |]
            Array.sortInPlace a
            match a.[0], a.[1], a.[2], a.[3] with
            | 0,0,0,0 -> t.Txxxx
            | 0,0,0,1 -> t.Txxxy
            | 0,0,0,2 -> t.Txxxz
            | 0,0,1,1 -> t.Txxyy
            | 0,0,1,2 -> t.Txxyz
            | 0,0,2,2 -> t.Txxzz
            | 0,1,1,1 -> t.Txyyy
            | 0,1,1,2 -> t.Txyyz
            | 0,1,2,2 -> t.Txyzz
            | 0,2,2,2 -> t.Txzzz
            | 1,1,1,1 -> t.Tyyyy
            | 1,1,1,2 -> t.Tyyyz
            | 1,1,2,2 -> t.Tyyzz
            | 1,2,2,2 -> t.Tyzzz
            | _       -> t.Tzzzz

        /// Quadruple contraction T : a ⊗ b ⊗ c ⊗ d.
        member t.Contract(a : V3d, b : V3d, c : V3d, d : V3d) =
            let inline ai i = match i with | 0 -> a.X | 1 -> a.Y | _ -> a.Z
            let inline bj j = match j with | 0 -> b.X | 1 -> b.Y | _ -> b.Z
            let inline ck k = match k with | 0 -> c.X | 1 -> c.Y | _ -> c.Z
            let inline dl l = match l with | 0 -> d.X | 1 -> d.Y | _ -> d.Z
            let mutable s = 0.0
            for i in 0..2 do
                for j in 0..2 do
                    for k in 0..2 do
                        for l in 0..2 do
                            s <- s + ai i * bj j * ck k * dl l * t.Get(i, j, k, l)
            s

    /// PCA plane of a centered point cloud given its 3×3 covariance matrix.
    /// Returns (normal, u_axis, v_axis) — normal is the smallest-eigenvalue direction.
    let principalPlane (cov : M33d) =
        let xx = cov.M00
        let yy = cov.M11
        let zz = cov.M22
        let xy = cov.M01
        let xz = cov.M02
        let yz = cov.M12
        let b = -xx - yy - zz
        let c = -sqr xy - sqr xz - sqr yz + xx*yy + xx*zz + yy*zz
        let d = -xx*yy*zz - 2.0*xy*xz*yz + sqr xz*yy + sqr xy*zz + sqr yz*xx
        let struct (l0, _l1, _l2) = sortTripleAbs (Polynomial.RealRootsOfNormed(b, c, d))

        let r0 = V3d(xx - l0, xy,       xz)
        let r1 = V3d(xy,      yy - l0,  yz)
        let r2 = V3d(xz,      yz,       zz - l0)
        let len0 = Vec.lengthSquared r0
        let len1 = Vec.lengthSquared r1
        let len2 = Vec.lengthSquared r2
        let normal =
            let v =
                if len0 > len1 then
                    if len2 > len1 then Vec.cross r0 r2
                    else Vec.cross r0 r1
                else
                    if len2 > len0 then Vec.cross r1 r2
                    else Vec.cross r0 r1
            if Vec.lengthSquared v > 0.0 then Vec.normalize v
            else V3d.OOI
        // Build orthonormal in-plane basis (u, v).
        let probe = if abs normal.Z < 0.9 then V3d.OOI else V3d.IOO
        let u = Vec.normalize (Vec.cross normal probe)
        let v = Vec.cross normal u
        normal, u, v


/// Incremental algebraic 3D circle (curve) fit.
///
/// Pipeline:
///   • PCA the 3×3 covariance to find the curve's plane (smallest-eigenvalue normal).
///   • Contract the accumulated 1st/2nd/3rd-order moments with the in-plane basis
///     to obtain the 2D moments needed for the standard algebraic circle fit.
///   • Solve the 3×3 linear system for (D, E, F) in the in-plane circle equation,
///     then lift the recovered (centre, radius) back to 3D.
///
/// All steps are O(1) in point count.  Same algebraic-fit caveat as `CircleRegression2d`.
[<Struct>]
type CircleRegression3d =

    val mutable private _reference : V3d
    val mutable private _sum    : V3d        // Σ p
    val mutable private _sumSq  : SymmetricMatrix3 // Σ p pᵀ (covariance source)
    val mutable private _t3     : SymTensor3   // Σ p ⊗ p ⊗ p
    val mutable private _count  : int

    private new(reference, sum, sumSq, t3, count) =
        { _reference = reference; _sum = sum; _sumSq = sumSq; _t3 = t3; _count = count }

    new(point : V3d) =
        { _reference = point; _sum = V3d.Zero; _sumSq = SymmetricMatrix3.Zero
          _t3 = SymTensor3.Zero; _count = 1 }

    new([<ParamArray>] pts : V3d[]) =
        if pts.Length <= 0 then
            { _reference = V3d.Zero; _sum = V3d.Zero; _sumSq = SymmetricMatrix3.Zero
              _t3 = SymTensor3.Zero; _count = 0 }
        else
            let reference = pts.[0]
            let mutable sum = V3d.Zero
            let mutable sumSq = SymmetricMatrix3.Zero
            let mutable t3 = SymTensor3.Zero
            let mutable count = 1
            for i in 1 .. pts.Length - 1 do
                let p = pts.[i] - reference
                sum <- sum + p
                sumSq.AddOuter p
                t3.AddOuter p
                count <- count + 1
            { _reference = reference; _sum = sum; _sumSq = sumSq; _t3 = t3; _count = count }

    new(pts : seq<V3d>) = CircleRegression3d(Seq.toArray pts)
    new(pts : list<V3d>) = CircleRegression3d(List.toArray pts)

    static member Empty =
        CircleRegression3d(V3d.Zero, V3d.Zero, SymmetricMatrix3.Zero, SymTensor3.Zero, 0)
    static member Zero = CircleRegression3d.Empty

    member x.Count = x._count
    member x.Centroid =
        if x._count = 0 then V3d.Zero
        else x._reference + x._sum / float x._count

    member x.Add(pt : V3d) =
        if x._count <= 0 then
            CircleRegression3d(pt, V3d.Zero, SymmetricMatrix3.Zero, SymTensor3.Zero, 1)
        else
            let p = pt - x._reference
            let mutable sumSq = x._sumSq
            let mutable t3 = x._t3
            sumSq.AddOuter p
            t3.AddOuter p
            CircleRegression3d(x._reference, x._sum + p, sumSq, t3, x._count + 1)

    member x.AddInPlace(pt : V3d) =
        if x._count <= 0 then
            x._reference <- pt
            x._sum <- V3d.Zero
            x._sumSq.SetZero()
            x._t3.SetZero()
            x._count <- 1
        else
            let p = pt - x._reference
            x._sum <- x._sum + p
            x._sumSq.AddOuter p
            x._t3.AddOuter p
            x._count <- x._count + 1

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.Remove(pt : V3d) =
        if x._count <= 1 then CircleRegression3d.Empty
        else
            let p = pt - x._reference
            let mutable sumSq = x._sumSq
            let mutable t3 = x._t3
            sumSq.SubOuter p
            t3.SubOuter p
            CircleRegression3d(x._reference, x._sum - p, sumSq, t3, x._count - 1)

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.RemoveInPlace(pt : V3d) =
        if x._count <= 1 then
            x._reference <- V3d.Zero
            x._sum <- V3d.Zero
            x._sumSq.SetZero()
            x._t3.SetZero()
            x._count <- 0
        else
            let p = pt - x._reference
            x._sum <- x._sum - p
            x._sumSq.SubOuter p
            x._t3.SubOuter p
            x._count <- x._count - 1

    member x.GetCircle() =
        if x._count < 4 then Circle3d.Invalid
        else
            let n = float x._count
            let avg = x._sum / n

            // Centred 2nd-order tensor (covariance × n)
            let cov =
                let s = x._sumSq.ToM33d()
                M33d(
                    s.M00 - avg.X * x._sum.X, s.M01 - avg.X * x._sum.Y, s.M02 - avg.X * x._sum.Z,
                    s.M01 - avg.X * x._sum.Y, s.M11 - avg.Y * x._sum.Y, s.M12 - avg.Y * x._sum.Z,
                    s.M02 - avg.X * x._sum.Z, s.M12 - avg.Y * x._sum.Z, s.M22 - avg.Z * x._sum.Z)
            let normal, u, v = principalPlane cov

            // ----- 2D in-plane moments after centring at the centroid -----
            // For each point  q = p − avg,  let x̂ = q·u, ŷ = q·v.
            //
            //   Σ x̂ = 0,  Σ ŷ = 0          (centred)
            //   Σ x̂² = uᵀ·Cov·u,  Σ ŷ² = vᵀ·Cov·v,  Σ x̂ŷ = uᵀ·Cov·v
            //   Σ |q̂|² = Σ x̂² + Σ ŷ²
            //
            //   Σ x̂(x̂²+ŷ²) = a third-order centred moment along u.
            // We need centred third-order moments — derive from the raw _t3 tensor
            // and lower moments via the standard de-centring identity:
            //   Σ q_i q_j q_k = T3_ijk − μ_i T2_jk − μ_j T2_ik − μ_k T2_ij + 2 n μ_i μ_j μ_k
            // Hoist struct fields to immutable locals so inner closures don't capture
            // the byref `x` (F# struct rule).
            let lhsSumSq = x._sumSq
            let lhsT3 = x._t3

            let t3Centred (i : int) (j : int) (k : int) =
                let mu = avg
                let mui = match i with | 0 -> mu.X | 1 -> mu.Y | _ -> mu.Z
                let muj = match j with | 0 -> mu.X | 1 -> mu.Y | _ -> mu.Z
                let muk = match k with | 0 -> mu.X | 1 -> mu.Y | _ -> mu.Z
                let s2ij = lhsSumSq.Get(i, j)
                let s2ik = lhsSumSq.Get(i, k)
                let s2jk = lhsSumSq.Get(j, k)
                lhsT3.Get(i, j, k) - mui * s2jk - muj * s2ik - muk * s2ij + 2.0 * n * mui * muj * muk

            // Contract the centred 3rd-order tensor with three vectors.
            let contract3c (a : V3d) (b : V3d) (c : V3d) =
                let inline gi i = match i with | 0 -> a.X | 1 -> a.Y | _ -> a.Z
                let inline gj j = match j with | 0 -> b.X | 1 -> b.Y | _ -> b.Z
                let inline gk k = match k with | 0 -> c.X | 1 -> c.Y | _ -> c.Z
                let mutable s = 0.0
                for i in 0..2 do
                    for j in 0..2 do
                        for k in 0..2 do
                            s <- s + gi i * gj j * gk k * t3Centred i j k
                s

            let sxx  = Vec.dot u (cov * u)
            let syy  = Vec.dot v (cov * v)
            let sxy  = Vec.dot u (cov * v)
            let sl2  = sxx + syy
            // Σ x̂ · |q̂|²  =  Σ x̂ · (x̂² + ŷ²)  =  contract3(u, u, u) + contract3(u, v, v)
            let sx_l2 = contract3c u u u + contract3c u v v
            let sy_l2 = contract3c v u u + contract3c v v v

            // Solve the standard algebraic circle fit (Kasa) in the centred 2D frame.
            //   x̂² + ŷ² + D·x̂ + E·ŷ + F = 0
            //   sums (after centring) reduce to  [[sxx sxy 0] [sxy syy 0] [0 0 n]] [D;E;F]
            //                                              = -[sx_l2; sy_l2; sl2]
            let m =
                M33d(
                    sxx, sxy, 0.0,
                    sxy, syy, 0.0,
                    0.0, 0.0, n)
            let rhs = V3d(-sx_l2, -sy_l2, -sl2)
            let det = m.Determinant
            if Fun.IsTiny det then Circle3d.Invalid
            else
                let DEF = m.Inverse * rhs
                let cu = -DEF.X * 0.5
                let cv = -DEF.Y * 0.5
                let r2 = cu * cu + cv * cv - DEF.Z
                if r2 <= 0.0 then Circle3d.Invalid
                else
                    let centre = x._reference + avg + u * cu + v * cv
                    Circle3d(centre, normal, sqrt r2)

    static member (+) (l : CircleRegression3d, r : V3d) = l.Add r
    static member (+) (l : V3d, r : CircleRegression3d) = r.Add l
    static member (-) (l : CircleRegression3d, r : V3d) = l.Remove r


module CircleRegression3d =
    let empty = CircleRegression3d.Empty
    let inline remove (pt : V3d) (s : CircleRegression3d) = s.Remove pt
    let inline add    (pt : V3d) (s : CircleRegression3d) = s.Add pt
    let inline getCircle (s : CircleRegression3d) = s.GetCircle()
    let inline count     (s : CircleRegression3d) = s.Count
    let inline centroid  (s : CircleRegression3d) = s.Centroid
    let ofSeq (points : seq<V3d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
    let ofList (points : list<V3d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
    let ofArray (points : V3d[]) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r


/// Incremental algebraic 3D ellipse (curve) fit.
///
/// Same idea as `CircleRegression3d` but uses the in-plane Fitzgibbon /
/// Halir-Flusser fit, which requires up to 4th-order moments of the 3D point
/// cloud (in addition to 1st/2nd/3rd-order). That extra storage (15 floats for
/// the symmetric 4th-order tensor) is the main cost compared to the circle fit.
[<Struct>]
type EllipseRegression3d =

    val mutable private _reference : V3d
    val mutable private _sum    : V3d
    val mutable private _sumSq  : SymmetricMatrix3
    val mutable private _t3     : SymTensor3
    val mutable private _t4     : SymTensor4
    val mutable private _count  : int

    private new(reference, sum, sumSq, t3, t4, count) =
        { _reference = reference; _sum = sum; _sumSq = sumSq
          _t3 = t3; _t4 = t4; _count = count }

    new(point : V3d) =
        { _reference = point; _sum = V3d.Zero; _sumSq = SymmetricMatrix3.Zero
          _t3 = SymTensor3.Zero; _t4 = SymTensor4.Zero; _count = 1 }

    new([<ParamArray>] pts : V3d[]) =
        if pts.Length <= 0 then
            { _reference = V3d.Zero; _sum = V3d.Zero; _sumSq = SymmetricMatrix3.Zero
              _t3 = SymTensor3.Zero; _t4 = SymTensor4.Zero; _count = 0 }
        else
            let reference = pts.[0]
            let mutable sum = V3d.Zero
            let mutable sumSq = SymmetricMatrix3.Zero
            let mutable t3 = SymTensor3.Zero
            let mutable t4 = SymTensor4.Zero
            let mutable count = 1
            for i in 1 .. pts.Length - 1 do
                let p = pts.[i] - reference
                sum <- sum + p
                sumSq.AddOuter p
                t3.AddOuter p
                t4.AddOuter p
                count <- count + 1
            { _reference = reference; _sum = sum; _sumSq = sumSq
              _t3 = t3; _t4 = t4; _count = count }

    new(pts : seq<V3d>) = EllipseRegression3d(Seq.toArray pts)
    new(pts : list<V3d>) = EllipseRegression3d(List.toArray pts)

    static member Empty =
        EllipseRegression3d(V3d.Zero, V3d.Zero, SymmetricMatrix3.Zero,
                            SymTensor3.Zero, SymTensor4.Zero, 0)
    static member Zero = EllipseRegression3d.Empty

    member x.Count = x._count
    member x.Centroid =
        if x._count = 0 then V3d.Zero
        else x._reference + x._sum / float x._count

    member x.Add(pt : V3d) =
        if x._count <= 0 then
            EllipseRegression3d(
                pt, V3d.Zero, SymmetricMatrix3.Zero, SymTensor3.Zero, SymTensor4.Zero, 1)
        else
            let p = pt - x._reference
            let mutable sumSq = x._sumSq
            let mutable t3 = x._t3
            let mutable t4 = x._t4
            sumSq.AddOuter p
            t3.AddOuter p
            t4.AddOuter p
            EllipseRegression3d(x._reference, x._sum + p, sumSq, t3, t4, x._count + 1)

    member x.AddInPlace(pt : V3d) =
        if x._count <= 0 then
            x._reference <- pt
            x._sum <- V3d.Zero
            x._sumSq.SetZero()
            x._t3.SetZero()
            x._t4.SetZero()
            x._count <- 1
        else
            let p = pt - x._reference
            x._sum <- x._sum + p
            x._sumSq.AddOuter p
            x._t3.AddOuter p
            x._t4.AddOuter p
            x._count <- x._count + 1

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.Remove(pt : V3d) =
        if x._count <= 1 then EllipseRegression3d.Empty
        else
            let p = pt - x._reference
            let mutable sumSq = x._sumSq
            let mutable t3 = x._t3
            let mutable t4 = x._t4
            sumSq.SubOuter p
            t3.SubOuter p
            t4.SubOuter p
            EllipseRegression3d(x._reference, x._sum - p, sumSq, t3, t4, x._count - 1)

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.RemoveInPlace(pt : V3d) =
        if x._count <= 1 then
            x._reference <- V3d.Zero
            x._sum <- V3d.Zero
            x._sumSq.SetZero()
            x._t3.SetZero()
            x._t4.SetZero()
            x._count <- 0
        else
            let p = pt - x._reference
            x._sum <- x._sum - p
            x._sumSq.SubOuter p
            x._t3.SubOuter p
            x._t4.SubOuter p
            x._count <- x._count - 1

    member x.GetEllipse() =
        if x._count < 6 then Ellipse3d.Invalid
        else
            let n = float x._count
            let avg = x._sum / n

            // covariance for plane PCA
            let cov =
                let s = x._sumSq.ToM33d()
                M33d(
                    s.M00 - avg.X * x._sum.X, s.M01 - avg.X * x._sum.Y, s.M02 - avg.X * x._sum.Z,
                    s.M01 - avg.X * x._sum.Y, s.M11 - avg.Y * x._sum.Y, s.M12 - avg.Y * x._sum.Z,
                    s.M02 - avg.X * x._sum.Z, s.M12 - avg.Y * x._sum.Z, s.M22 - avg.Z * x._sum.Z)
            let normal, u, v = principalPlane cov

            // We project to (x̂, ŷ) = ((p − μ)·u, (p − μ)·v) where μ is the centroid.
            // The 2D in-plane points are centred (Σ x̂ = Σ ŷ = 0).  The full 6×6 Fitzgibbon
            // scatter is computed by contracting the *centred* 3rd- and 4th-order moments
            // with the plane basis (u, v).

            // Hoist struct fields to immutable locals (F# byref-this rule).
            let lhsSum = x._sum
            let lhsSumSq = x._sumSq
            let lhsT3 = x._t3
            let lhsT4 = x._t4

            // raw moment getters
            let inline mu i = match i with | 0 -> avg.X | 1 -> avg.Y | _ -> avg.Z
            let s1 i = match i with | 0 -> lhsSum.X | 1 -> lhsSum.Y | _ -> lhsSum.Z
            let s2 i j = lhsSumSq.Get(i, j)
            let s3 i j k = lhsT3.Get(i, j, k)
            let s4 i j k l = lhsT4.Get(i, j, k, l)

            // centred 2nd-order: c2_ij = s2_ij − n μ_i μ_j
            let c2 i j = s2 i j - n * mu i * mu j

            // centred 3rd-order:
            //   c3_ijk = s3_ijk − μ_i s2_jk − μ_j s2_ik − μ_k s2_ij + 2 n μ_i μ_j μ_k
            let c3 i j k =
                s3 i j k
                - mu i * s2 j k
                - mu j * s2 i k
                - mu k * s2 i j
                + 2.0 * n * mu i * mu j * mu k

            // centred 4th-order:
            //   c4_ijkl = s4_ijkl
            //           − μ_i s3_jkl − μ_j s3_ikl − μ_k s3_ijl − μ_l s3_ijk
            //           + (μ_i μ_j s2_kl + μ_i μ_k s2_jl + μ_i μ_l s2_jk
            //            + μ_j μ_k s2_il + μ_j μ_l s2_ik + μ_k μ_l s2_ij)
            //           − 3 n μ_i μ_j μ_k μ_l
            let c4 i j k l =
                s4 i j k l
                - mu i * s3 j k l
                - mu j * s3 i k l
                - mu k * s3 i j l
                - mu l * s3 i j k
                + mu i * mu j * s2 k l
                + mu i * mu k * s2 j l
                + mu i * mu l * s2 j k
                + mu j * mu k * s2 i l
                + mu j * mu l * s2 i k
                + mu k * mu l * s2 i j
                - 3.0 * n * mu i * mu j * mu k * mu l

            // contractors over centred tensors
            let inline c2c (a : V3d) (b : V3d) =
                let inline ai i = match i with | 0 -> a.X | 1 -> a.Y | _ -> a.Z
                let inline bj j = match j with | 0 -> b.X | 1 -> b.Y | _ -> b.Z
                let mutable s = 0.0
                for i in 0..2 do for j in 0..2 do s <- s + ai i * bj j * c2 i j
                s

            let inline c3c (a : V3d) (b : V3d) (cc : V3d) =
                let inline ai i = match i with | 0 -> a.X | 1 -> a.Y | _ -> a.Z
                let inline bj j = match j with | 0 -> b.X | 1 -> b.Y | _ -> b.Z
                let inline ck k = match k with | 0 -> cc.X | 1 -> cc.Y | _ -> cc.Z
                let mutable s = 0.0
                for i in 0..2 do
                    for j in 0..2 do
                        for k in 0..2 do
                            s <- s + ai i * bj j * ck k * c3 i j k
                s

            let inline c4c (a : V3d) (b : V3d) (cc : V3d) (d : V3d) =
                let inline ai i = match i with | 0 -> a.X | 1 -> a.Y | _ -> a.Z
                let inline bj j = match j with | 0 -> b.X | 1 -> b.Y | _ -> b.Z
                let inline ck k = match k with | 0 -> cc.X | 1 -> cc.Y | _ -> cc.Z
                let inline dl l = match l with | 0 -> d.X | 1 -> d.Y | _ -> d.Z
                let mutable s = 0.0
                for i in 0..2 do
                    for j in 0..2 do
                        for k in 0..2 do
                            for l in 0..2 do
                                s <- s + ai i * bj j * ck k * dl l * c4 i j k l
                s

            // Required 2D moments for the Fitzgibbon scatter (centred frame):
            // Σ x̂ⁿŷᵐ derived from contractions of (u, v) with centred 2/3/4-order tensors.
            let m_x4   = c4c u u u u                 // Σ x̂⁴
            let m_x3y  = c4c u u u v
            let m_x2y2 = c4c u u v v
            let m_xy3  = c4c u v v v
            let m_y4   = c4c v v v v
            let m_x3   = c3c u u u
            let m_x2y  = c3c u u v
            let m_xy2  = c3c u v v
            let m_y3   = c3c v v v
            let m_x2   = c2c u u
            let m_xy   = c2c u v
            let m_y2   = c2c v v
            // centred ⇒ Σ x̂ = Σ ŷ = 0
            let m_x = 0.0
            let m_y = 0.0

            // Build the 6×6 Fitzgibbon scatter S for v = (x̂², x̂ŷ, ŷ², x̂, ŷ, 1).
            // Use a flat 6×6 array since we already trust the EllipseRegression2d
            // algorithm — we just feed it the same shape of input.
            let S = Array2D.zeroCreate 6 6
            S.[0,0] <- m_x4;   S.[0,1] <- m_x3y;  S.[0,2] <- m_x2y2; S.[0,3] <- m_x3;   S.[0,4] <- m_x2y;  S.[0,5] <- m_x2
            S.[1,1] <- m_x2y2; S.[1,2] <- m_xy3;  S.[1,3] <- m_x2y;  S.[1,4] <- m_xy2;  S.[1,5] <- m_xy
            S.[2,2] <- m_y4;   S.[2,3] <- m_xy2;  S.[2,4] <- m_y3;   S.[2,5] <- m_y2
            S.[3,3] <- m_x2;   S.[3,4] <- m_xy;   S.[3,5] <- m_x
            S.[4,4] <- m_y2;   S.[4,5] <- m_y
            S.[5,5] <- n
            for i in 0..5 do for j in 0..i-1 do S.[i,j] <- S.[j,i]

            // Halir-Flusser reduction:  M = inv(C1) · (S1 − S2 inv(S3) S2ᵀ)
            // S1 = top-left 3×3, S2 = top-right 3×3, S3 = bottom-right 3×3
            let s1 = M33d(S.[0,0], S.[0,1], S.[0,2],
                          S.[1,0], S.[1,1], S.[1,2],
                          S.[2,0], S.[2,1], S.[2,2])
            let s2 = M33d(S.[0,3], S.[0,4], S.[0,5],
                          S.[1,3], S.[1,4], S.[1,5],
                          S.[2,3], S.[2,4], S.[2,5])
            let s3 = M33d(S.[3,3], S.[3,4], S.[3,5],
                          S.[4,3], S.[4,4], S.[4,5],
                          S.[5,3], S.[5,4], S.[5,5])
            let s3inv = s3.Inverse
            let t = -s3inv * s2.Transposed
            let reduced = s1 + s2 * t
            let c1Inv = M33d(0.0, 0.0, 0.5,  0.0,-1.0, 0.0,  0.5, 0.0, 0.0)
            let m = c1Inv * reduced

            // Eigenvalues of m via cubic
            let pb = -(m.M00 + m.M11 + m.M22)
            let pm00 = m.M11 * m.M22 - m.M12 * m.M21
            let pm11 = m.M00 * m.M22 - m.M02 * m.M20
            let pm22 = m.M00 * m.M11 - m.M01 * m.M10
            let pc = pm00 + pm11 + pm22
            let pd = -m.Determinant
            let struct(r0, r1, r2) = Polynomial.RealRootsOfNormed(pb, pc, pd)

            let computeEigenvector (lam : float) =
                let r0v = V3d(m.M00 - lam, m.M01,       m.M02)
                let r1v = V3d(m.M10,       m.M11 - lam, m.M12)
                let r2v = V3d(m.M20,       m.M21,       m.M22 - lam)
                let len0 = Vec.lengthSquared r0v
                let len1 = Vec.lengthSquared r1v
                let len2 = Vec.lengthSquared r2v
                if len0 > len1 then
                    if len2 > len1 then Vec.cross r0v r2v
                    else Vec.cross r0v r1v
                else
                    if len2 > len0 then Vec.cross r1v r2v
                    else Vec.cross r0v r1v

            let mutable best = V3d.Zero
            let mutable bestScore = 0.0
            let tryLam (lam : float) =
                if not (Double.IsNaN lam) then
                    let vEig = computeEigenvector lam
                    if Vec.lengthSquared vEig > 0.0 then
                        let score = 4.0 * vEig.X * vEig.Z - vEig.Y * vEig.Y
                        if score > bestScore then
                            best <- vEig; bestScore <- score
            tryLam r0; tryLam r1; tryLam r2

            if bestScore <= 0.0 then Ellipse3d.Invalid
            else
                let abc = best
                let def = t * abc
                let A = abc.X
                let B = abc.Y
                let C = abc.Z
                let D = def.X
                let E = def.Y
                let F = def.Z
                let det4 = 4.0 * A * C - B * B
                if det4 <= 0.0 then Ellipse3d.Invalid
                else
                    // 2D centre + axes (in (x̂, ŷ) coordinates), then lift to 3D
                    let cx = (B * E - 2.0 * C * D) / det4
                    let cy = (B * D - 2.0 * A * E) / det4
                    let F' = F + 0.5 * (D * cx + E * cy)
                    let a = A
                    let b = B / 2.0
                    let c = C
                    let trM = a + c
                    let dt = a * c - b * b
                    let disc = trM * trM - 4.0 * dt |> max 0.0
                    let s = sqrt disc
                    let lamA = 0.5 * (trM + s)
                    let lamB = 0.5 * (trM - s)
                    let lam0, lam1 =
                        if abs lamA <= abs lamB then lamA, lamB
                        else lamB, lamA
                    let r0sq = -F' / lam0
                    let r1sq = -F' / lam1
                    if r0sq <= 0.0 || r1sq <= 0.0 then Ellipse3d.Invalid
                    else
                        let rMajor = sqrt r0sq
                        let rMinor = sqrt r1sq
                        // 2D direction of major axis (in (x̂, ŷ))
                        let majorDir2 =
                            if abs b > 1e-12 then
                                let d2 = V2d(-b, a - lam0)
                                d2 / Vec.length d2
                            elif abs a <= abs c then V2d.IO
                            else V2d.OI
                        let minorDir2 = V2d(-majorDir2.Y, majorDir2.X)
                        // Lift to 3D: x̂·u + ŷ·v
                        let majorAxis3d = u * majorDir2.X + v * majorDir2.Y
                        let minorAxis3d = u * minorDir2.X + v * minorDir2.Y
                        // 3D centre
                        let centre3d = x._reference + avg + u * cx + v * cy
                        Ellipse3d(centre3d, normal, majorAxis3d * rMajor, minorAxis3d * rMinor)

    static member (+) (l : EllipseRegression3d, r : V3d) = l.Add r
    static member (+) (l : V3d, r : EllipseRegression3d) = r.Add l
    static member (-) (l : EllipseRegression3d, r : V3d) = l.Remove r


module EllipseRegression3d =
    let empty = EllipseRegression3d.Empty
    let inline remove (pt : V3d) (s : EllipseRegression3d) = s.Remove pt
    let inline add    (pt : V3d) (s : EllipseRegression3d) = s.Add pt
    let inline getEllipse (s : EllipseRegression3d) = s.GetEllipse()
    let inline count      (s : EllipseRegression3d) = s.Count
    let inline centroid   (s : EllipseRegression3d) = s.Centroid
    let ofSeq (points : seq<V3d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
    let ofList (points : list<V3d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
    let ofArray (points : V3d[]) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
