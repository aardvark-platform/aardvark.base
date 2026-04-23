namespace Aardvark.Geometry

open System
open Aardvark.Base

[<AutoOpen>]
module private D6Math =

    [<Struct>]
    type V6d =
        val mutable public C0 : float
        val mutable public C1 : float
        val mutable public C2 : float
        val mutable public C3 : float
        val mutable public C4 : float
        val mutable public C5 : float

        new(c0, c1, c2, c3, c4, c5) =
            { C0 = c0; C1 = c1; C2 = c2; C3 = c3; C4 = c4; C5 = c5 }

        static member Zero = V6d(0.0, 0.0, 0.0, 0.0, 0.0, 0.0)

    /// Symmetric 6×6 matrix, storing only the 21 unique upper-triangular entries.
    /// Used as a running accumulator of Σ vᵢvᵢᵀ for the ellipse Fitzgibbon /
    /// Halir-Flusser fit where the per-point feature vector v has 6 entries.
    [<Struct>]
    type SymmetricM66d =
        val mutable public M00 : float
        val mutable public M01 : float
        val mutable public M02 : float
        val mutable public M03 : float
        val mutable public M04 : float
        val mutable public M05 : float
        val mutable public M11 : float
        val mutable public M12 : float
        val mutable public M13 : float
        val mutable public M14 : float
        val mutable public M15 : float
        val mutable public M22 : float
        val mutable public M23 : float
        val mutable public M24 : float
        val mutable public M25 : float
        val mutable public M33 : float
        val mutable public M34 : float
        val mutable public M35 : float
        val mutable public M44 : float
        val mutable public M45 : float
        val mutable public M55 : float

        new(m00, m01, m02, m03, m04, m05,
                 m11, m12, m13, m14, m15,
                      m22, m23, m24, m25,
                           m33, m34, m35,
                                m44, m45,
                                     m55) =
            { M00 = m00; M01 = m01; M02 = m02; M03 = m03; M04 = m04; M05 = m05
              M11 = m11; M12 = m12; M13 = m13; M14 = m14; M15 = m15
              M22 = m22; M23 = m23; M24 = m24; M25 = m25
              M33 = m33; M34 = m34; M35 = m35
              M44 = m44; M45 = m45
              M55 = m55 }

        static member Zero =
            SymmetricM66d(0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                               0.0, 0.0, 0.0, 0.0, 0.0,
                                    0.0, 0.0, 0.0, 0.0,
                                         0.0, 0.0, 0.0,
                                              0.0, 0.0,
                                                   0.0)

        member x.AddOuterProduct(v : V6d) =
            x.M00 <- x.M00 + v.C0*v.C0; x.M01 <- x.M01 + v.C0*v.C1; x.M02 <- x.M02 + v.C0*v.C2
            x.M03 <- x.M03 + v.C0*v.C3; x.M04 <- x.M04 + v.C0*v.C4; x.M05 <- x.M05 + v.C0*v.C5
            x.M11 <- x.M11 + v.C1*v.C1; x.M12 <- x.M12 + v.C1*v.C2; x.M13 <- x.M13 + v.C1*v.C3
            x.M14 <- x.M14 + v.C1*v.C4; x.M15 <- x.M15 + v.C1*v.C5
            x.M22 <- x.M22 + v.C2*v.C2; x.M23 <- x.M23 + v.C2*v.C3; x.M24 <- x.M24 + v.C2*v.C4
            x.M25 <- x.M25 + v.C2*v.C5
            x.M33 <- x.M33 + v.C3*v.C3; x.M34 <- x.M34 + v.C3*v.C4; x.M35 <- x.M35 + v.C3*v.C5
            x.M44 <- x.M44 + v.C4*v.C4; x.M45 <- x.M45 + v.C4*v.C5
            x.M55 <- x.M55 + v.C5*v.C5

        member x.SubOuterProduct(v : V6d) =
            x.M00 <- x.M00 - v.C0*v.C0; x.M01 <- x.M01 - v.C0*v.C1; x.M02 <- x.M02 - v.C0*v.C2
            x.M03 <- x.M03 - v.C0*v.C3; x.M04 <- x.M04 - v.C0*v.C4; x.M05 <- x.M05 - v.C0*v.C5
            x.M11 <- x.M11 - v.C1*v.C1; x.M12 <- x.M12 - v.C1*v.C2; x.M13 <- x.M13 - v.C1*v.C3
            x.M14 <- x.M14 - v.C1*v.C4; x.M15 <- x.M15 - v.C1*v.C5
            x.M22 <- x.M22 - v.C2*v.C2; x.M23 <- x.M23 - v.C2*v.C3; x.M24 <- x.M24 - v.C2*v.C4
            x.M25 <- x.M25 - v.C2*v.C5
            x.M33 <- x.M33 - v.C3*v.C3; x.M34 <- x.M34 - v.C3*v.C4; x.M35 <- x.M35 - v.C3*v.C5
            x.M44 <- x.M44 - v.C4*v.C4; x.M45 <- x.M45 - v.C4*v.C5
            x.M55 <- x.M55 - v.C5*v.C5

        member x.SetZero() =
            x.M00 <- 0.0; x.M01 <- 0.0; x.M02 <- 0.0; x.M03 <- 0.0; x.M04 <- 0.0; x.M05 <- 0.0
            x.M11 <- 0.0; x.M12 <- 0.0; x.M13 <- 0.0; x.M14 <- 0.0; x.M15 <- 0.0
            x.M22 <- 0.0; x.M23 <- 0.0; x.M24 <- 0.0; x.M25 <- 0.0
            x.M33 <- 0.0; x.M34 <- 0.0; x.M35 <- 0.0
            x.M44 <- 0.0; x.M45 <- 0.0
            x.M55 <- 0.0

        member x.AddInPlace(o : SymmetricM66d) =
            x.M00 <- x.M00 + o.M00; x.M01 <- x.M01 + o.M01; x.M02 <- x.M02 + o.M02
            x.M03 <- x.M03 + o.M03; x.M04 <- x.M04 + o.M04; x.M05 <- x.M05 + o.M05
            x.M11 <- x.M11 + o.M11; x.M12 <- x.M12 + o.M12; x.M13 <- x.M13 + o.M13
            x.M14 <- x.M14 + o.M14; x.M15 <- x.M15 + o.M15
            x.M22 <- x.M22 + o.M22; x.M23 <- x.M23 + o.M23; x.M24 <- x.M24 + o.M24; x.M25 <- x.M25 + o.M25
            x.M33 <- x.M33 + o.M33; x.M34 <- x.M34 + o.M34; x.M35 <- x.M35 + o.M35
            x.M44 <- x.M44 + o.M44; x.M45 <- x.M45 + o.M45
            x.M55 <- x.M55 + o.M55

        member x.SubInPlace(o : SymmetricM66d) =
            x.M00 <- x.M00 - o.M00; x.M01 <- x.M01 - o.M01; x.M02 <- x.M02 - o.M02
            x.M03 <- x.M03 - o.M03; x.M04 <- x.M04 - o.M04; x.M05 <- x.M05 - o.M05
            x.M11 <- x.M11 - o.M11; x.M12 <- x.M12 - o.M12; x.M13 <- x.M13 - o.M13
            x.M14 <- x.M14 - o.M14; x.M15 <- x.M15 - o.M15
            x.M22 <- x.M22 - o.M22; x.M23 <- x.M23 - o.M23; x.M24 <- x.M24 - o.M24; x.M25 <- x.M25 - o.M25
            x.M33 <- x.M33 - o.M33; x.M34 <- x.M34 - o.M34; x.M35 <- x.M35 - o.M35
            x.M44 <- x.M44 - o.M44; x.M45 <- x.M45 - o.M45
            x.M55 <- x.M55 - o.M55

        /// Top-left 3×3 block, corresponding to the quadratic coefficients (A,B,C).
        member x.BlockS1 =
            M33d(x.M00, x.M01, x.M02,
                 x.M01, x.M11, x.M12,
                 x.M02, x.M12, x.M22)

        /// Top-right 3×3 block — note this block is not symmetric (mixes quadratic
        /// and linear coefficients).
        member x.BlockS2 =
            M33d(x.M03, x.M04, x.M05,
                 x.M13, x.M14, x.M15,
                 x.M23, x.M24, x.M25)

        /// Bottom-right 3×3 block, corresponding to the linear coefficients (D,E,F).
        member x.BlockS3 =
            M33d(x.M33, x.M34, x.M35,
                 x.M34, x.M44, x.M45,
                 x.M35, x.M45, x.M55)

    /// Inverse of Fitzgibbon's constraint matrix C1 = [[0,0,2],[0,-1,0],[2,0,0]].
    let c1Inverse =
        M33d(0.0, 0.0, 0.5,
             0.0,-1.0, 0.0,
             0.5, 0.0, 0.0)


/// Incremental least-squares ellipse fit in 2D.
///
/// Fits the general conic  A*x² + B*x*y + C*y² + D*x + E*y + F = 0
/// using the Fitzgibbon / Halir-Flusser direct ellipse-specific algebraic fit:
/// the ellipticity constraint  4·A·C − B² = 1  is imposed during the fit, so
/// the result is *always* an ellipse — unlike a generic conic fit, the returned
/// parameters never land on a hyperbola or parabola.
///
/// Per-point feature vector v = (x², x·y, y², x, y, 1). The accumulator stores
/// Σ vᵢvᵢᵀ as a symmetric 6×6 matrix. Add/Remove (and combine operators) are
/// O(1); GetEllipse is O(1) in point count but involves a cubic root-finding
/// step (via Polynomial.RealRootsOfNormed) on a 3×3 reduced matrix.
///
/// NOTE: this is still an *algebraic* fit — it minimises algebraic rather than
/// geometric distance. For strictly elliptical input the returned ellipse is
/// essentially exact; on noisy/partial data it is a good initial guess for a
/// non-linear (e.g. Ceres) refinement.
[<Struct>]
type EllipseRegression2d =

    val mutable private _reference : V2d
    val mutable private _lhs : SymmetricM66d
    val mutable private _count : int

    private new(reference : V2d, lhs : SymmetricM66d, count : int) =
        { _reference = reference; _lhs = lhs; _count = count }

    /// Per-point feature vector: v = (x², x·y, y², x, y, 1)
    static member private Feature(px : float, py : float) =
        V6d(px*px, px*py, py*py, px, py, 1.0)

    /// Creates a new regression from the given points.
    new([<ParamArray>] pts : V2d[]) =
        if pts.Length <= 0 then
            { _reference = V2d.Zero; _lhs = SymmetricM66d.Zero; _count = 0 }
        else
            let reference = pts.[0]
            // First point contributes v = (0,0,0,0,0,1); outer product adds 1 to M55 only.
            let mutable lhs = SymmetricM66d(M55 = 1.0)
            let mutable count = 1
            for i in 1 .. pts.Length - 1 do
                let p = pts.[i] - reference
                lhs.AddOuterProduct (EllipseRegression2d.Feature(p.X, p.Y))
                count <- count + 1
            { _reference = reference; _lhs = lhs; _count = count }

    /// Creates a new regression from the given points.
    new(pts : seq<V2d>) = EllipseRegression2d(Seq.toArray pts)
    /// Creates a new regression from the given points.
    new(pts : list<V2d>) = EllipseRegression2d(List.toArray pts)
    /// Creates a new regression with a single point.
    new(point : V2d) =
        { _reference = point
          _lhs = SymmetricM66d(M55 = 1.0)
          _count = 1 }

    /// A regression holding no points.
    static member Empty =
        EllipseRegression2d(V2d.Zero, SymmetricM66d.Zero, 0)

    /// A regression holding no points.
    static member Zero = EllipseRegression2d.Empty

    /// The total number of points added. At least 5 are required to fit an ellipse.
    member x.Count = x._count

    /// The centroid of all added points. Recovered from the accumulator: the
    /// bottom-right 3×3 block contains Σx, Σy and the point count.
    member x.Centroid =
        if x._count = 0 then V2d.Zero
        else x._reference + V2d(x._lhs.M35, x._lhs.M45) / x._lhs.M55

    /// Re-expresses the accumulated statistics relative to a new reference point.
    /// The underlying fit is unchanged; this only moves the local origin.
    ///
    /// Math: v' = A · v for a 6×6 matrix A encoding the feature-vector change
    /// under (x, y) → (x + cx, y + cy) (where c = old_ref − new_ref).
    /// Hence  lhs' = A · lhs · Aᵀ.  There is no separate rhs to update — the
    /// problem is homogeneous (no per-point target).
    member private x.WithReferencePoint(r : V2d) =
        let c = x._reference - r
        if x._reference + c = x._reference then x
        elif x._count = 0 then
            EllipseRegression2d(r, SymmetricM66d.Zero, 0)
        else
            let cx = c.X
            let cy = c.Y

            // A (6×6) as dense array
            let a = Array2D.zeroCreate 6 6
            a.[0,0] <- 1.0;                 a.[0,3] <- 2.0*cx;                   a.[0,5] <- cx*cx
            a.[1,1] <- 1.0;                 a.[1,3] <- cy;      a.[1,4] <- cx;   a.[1,5] <- cx*cy
            a.[2,2] <- 1.0;                                     a.[2,4] <- 2.0*cy; a.[2,5] <- cy*cy
            a.[3,3] <- 1.0;                                                      a.[3,5] <- cx
            a.[4,4] <- 1.0;                                                      a.[4,5] <- cy
            a.[5,5] <- 1.0

            // L (dense 6×6 from symmetric lhs)
            let lhs = x._lhs
            let l = Array2D.zeroCreate 6 6
            l.[0,0] <- lhs.M00; l.[0,1] <- lhs.M01; l.[0,2] <- lhs.M02; l.[0,3] <- lhs.M03; l.[0,4] <- lhs.M04; l.[0,5] <- lhs.M05
            l.[1,0] <- lhs.M01; l.[1,1] <- lhs.M11; l.[1,2] <- lhs.M12; l.[1,3] <- lhs.M13; l.[1,4] <- lhs.M14; l.[1,5] <- lhs.M15
            l.[2,0] <- lhs.M02; l.[2,1] <- lhs.M12; l.[2,2] <- lhs.M22; l.[2,3] <- lhs.M23; l.[2,4] <- lhs.M24; l.[2,5] <- lhs.M25
            l.[3,0] <- lhs.M03; l.[3,1] <- lhs.M13; l.[3,2] <- lhs.M23; l.[3,3] <- lhs.M33; l.[3,4] <- lhs.M34; l.[3,5] <- lhs.M35
            l.[4,0] <- lhs.M04; l.[4,1] <- lhs.M14; l.[4,2] <- lhs.M24; l.[4,3] <- lhs.M34; l.[4,4] <- lhs.M44; l.[4,5] <- lhs.M45
            l.[5,0] <- lhs.M05; l.[5,1] <- lhs.M15; l.[5,2] <- lhs.M25; l.[5,3] <- lhs.M35; l.[5,4] <- lhs.M45; l.[5,5] <- lhs.M55

            // AL = A · L
            let al = Array2D.zeroCreate 6 6
            for i in 0..5 do
                for j in 0..5 do
                    let mutable s = 0.0
                    for k in 0..5 do s <- s + a.[i,k] * l.[k,j]
                    al.[i,j] <- s

            // ALAᵀ (symmetric; we read upper triangle)
            let e i j =
                let mutable s = 0.0
                for k in 0..5 do s <- s + al.[i,k] * a.[j,k]
                s

            let newLhs =
                SymmetricM66d(
                    e 0 0, e 0 1, e 0 2, e 0 3, e 0 4, e 0 5,
                           e 1 1, e 1 2, e 1 3, e 1 4, e 1 5,
                                  e 2 2, e 2 3, e 2 4, e 2 5,
                                         e 3 3, e 3 4, e 3 5,
                                                e 4 4, e 4 5,
                                                       e 5 5)

            EllipseRegression2d(r, newLhs, x._count)

    /// Adds the given point to the regression.
    member x.Add(pt : V2d) =
        if x._count <= 0 then
            EllipseRegression2d(pt, SymmetricM66d(M55 = 1.0), 1)
        else
            let p = pt - x._reference
            let mutable lhs = x._lhs
            lhs.AddOuterProduct (EllipseRegression2d.Feature(p.X, p.Y))
            EllipseRegression2d(x._reference, lhs, x._count + 1)

    /// Adds the given point to the regression (mutating the regression).
    member x.AddInPlace(pt : V2d) =
        if x._count <= 0 then
            x._reference <- pt
            x._lhs.SetZero()
            x._lhs.M55 <- 1.0
            x._count <- 1
        else
            let p = pt - x._reference
            x._lhs.AddOuterProduct (EllipseRegression2d.Feature(p.X, p.Y))
            x._count <- x._count + 1

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.Remove(pt : V2d) =
        if x._count <= 1 then
            EllipseRegression2d.Empty
        else
            let p = pt - x._reference
            let mutable lhs = x._lhs
            lhs.SubOuterProduct (EllipseRegression2d.Feature(p.X, p.Y))
            EllipseRegression2d(x._reference, lhs, x._count - 1)

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.RemoveInPlace(pt : V2d) =
        if x._count <= 1 then
            x._reference <- V2d.Zero
            x._lhs.SetZero()
            x._count <- 0
        else
            let p = pt - x._reference
            x._lhs.SubOuterProduct (EllipseRegression2d.Feature(p.X, p.Y))
            x._count <- x._count - 1

    /// Gets the least-squares ellipse via Fitzgibbon / Halir-Flusser direct fit.
    ///
    /// Pipeline:
    ///   1. Partition S = Σvvᵀ into 3×3 blocks  [[S1, S2], [S2ᵀ, S3]].
    ///   2. Reduce the 6×6 generalized eigenvalue problem to a 3×3 standard one:
    ///        M = inv(C1) · (S1 − S2 · inv(S3) · S2ᵀ)
    ///      where C1 = [[0,0,2],[0,−1,0],[2,0,0]] is the top block of Fitzgibbon's
    ///      constraint matrix.
    ///   3. Find M's eigenvalues via `Polynomial.RealRootsOfNormed`.
    ///   4. Pick the one positive real eigenvalue; its eigenvector is (A,B,C).
    ///   5. Recover (D,E,F) = −inv(S3) · S2ᵀ · (A,B,C).
    ///   6. Extract center + axes + orientation from the 6 conic parameters.
    member x.GetEllipse() =
        if x._count < 5 then
            Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero)
        else
            let s1 = x._lhs.BlockS1
            let s2 = x._lhs.BlockS2
            let s3 = x._lhs.BlockS3
            let s3inv = s3.Inverse
            // T such that (D,E,F) = T · (A,B,C)
            let t = -s3inv * s2.Transposed
            // Reduced scatter: S1 − S2 · inv(S3) · S2ᵀ
            let reduced = s1 + s2 * t
            // M = inv(C1) · reduced
            let m = c1Inverse * reduced

            // Characteristic polynomial of M: λ³ + b λ² + c λ + d = 0
            //   b = −tr(M)
            //   c = sum of principal 2×2 minors
            //   d = −det(M)
            let tr = m.M00 + m.M11 + m.M22
            let pm00 = m.M11 * m.M22 - m.M12 * m.M21
            let pm11 = m.M00 * m.M22 - m.M02 * m.M20
            let pm22 = m.M00 * m.M11 - m.M01 * m.M10
            let pb = -tr
            let pc = pm00 + pm11 + pm22
            let pd = -m.Determinant

            let struct(r0, r1, r2) = Polynomial.RealRootsOfNormed(pb, pc, pd)

            // Halir-Flusser selection: among the three eigenvectors, exactly one
            // satisfies the ellipticity condition 4AC − B² > 0. For exact
            // elliptical data the corresponding eigenvalue is numerically ≈ 0, so
            // picking "smallest positive eigenvalue" doesn't work; the ellipticity
            // test does and is what the original Fitzgibbon paper prescribes.
            // Null space of (M − λI). Since M here is NOT symmetric (it is
            // C1⁻¹·symmetric), we recover the null space as the orthogonal
            // complement of the rowspace: cross two linearly-independent rows.
            let computeEigenvector (lam : float) =
                let r0 = V3d(m.M00 - lam, m.M01,       m.M02)
                let r1 = V3d(m.M10,       m.M11 - lam, m.M12)
                let r2 = V3d(m.M20,       m.M21,       m.M22 - lam)
                let len0 = Vec.lengthSquared r0
                let len1 = Vec.lengthSquared r1
                let len2 = Vec.lengthSquared r2
                if len0 > len1 then
                    if len2 > len1 then Vec.cross r0 r2
                    else Vec.cross r0 r1
                else
                    if len2 > len0 then Vec.cross r1 r2
                    else Vec.cross r0 r1

            // For each real eigenvalue, compute its eigenvector and keep the one
            // with the largest ellipticity score 4AC - B² (must be strictly > 0).
            let mutable best = V3d.Zero
            let mutable bestScore = 0.0
            let tryLam (lam : float) =
                if not (Double.IsNaN lam) then
                    let v = computeEigenvector lam
                    if Vec.lengthSquared v > 0.0 then
                        let score = 4.0 * v.X * v.Z - v.Y * v.Y
                        if score > bestScore then
                            best <- v
                            bestScore <- score
            tryLam r0
            tryLam r1
            tryLam r2

            if bestScore <= 0.0 then
                Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero)
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
                if det4 <= 0.0 then
                    Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero)
                else
                    let cx = (B * E - 2.0 * C * D) / det4
                    let cy = (B * D - 2.0 * A * E) / det4
                    // Centered form:  A·x'² + B·x'y' + C·y'² + F' = 0
                    //              ⇔  (p')ᵀ M (p') = −F',  M = [[A, B/2],[B/2, C]]
                    let F' = F + 0.5 * (D * cx + E * cy)

                    let a = A
                    let b = B / 2.0
                    let c = C
                    let trM = a + c
                    let dt = a * c - b * b
                    let disc = trM * trM - 4.0 * dt |> max 0.0
                    let s = sqrt disc
                    // Both eigenvalues have the same sign under Fitzgibbon's constraint.
                    // We sort so lam0 has the smaller magnitude → larger semi-axis.
                    let lamA = 0.5 * (trM + s)
                    let lamB = 0.5 * (trM - s)
                    let lam0, lam1 =
                        if abs lamA <= abs lamB then lamA, lamB
                        else lamB, lamA

                    let r0sq = -F' / lam0
                    let r1sq = -F' / lam1
                    if r0sq <= 0.0 || r1sq <= 0.0 then
                        Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero)
                    else
                        let rMajor = sqrt r0sq
                        let rMinor = sqrt r1sq

                        let majorDir =
                            if abs b > 1e-12 then
                                V2d(-b, a - lam0) |> Vec.normalize
                            elif abs a <= abs c then
                                V2d.IO
                            else
                                V2d.OI
                        let minorDir = V2d(-majorDir.Y, majorDir.X)

                        let center = x._reference + V2d(cx, cy)
                        Ellipse2d(center, majorDir * rMajor, minorDir * rMinor)

    static member (+) (l : EllipseRegression2d, r : EllipseRegression2d) =
        if r._count <= 0 then l
        elif l._count <= 0 then r
        elif l._count = 1 then r.Add l._reference
        elif r._count = 1 then l.Add r._reference
        else
            let r2 = r.WithReferencePoint l._reference
            let mutable lhs = l._lhs
            lhs.AddInPlace r2._lhs
            EllipseRegression2d(l._reference, lhs, l._count + r2._count)

    static member (-) (l : EllipseRegression2d, r : EllipseRegression2d) =
        if r._count <= 0 then l
        elif l._count <= r._count then EllipseRegression2d.Empty
        elif r._count = 1 then l.Remove r._reference
        else
            let r2 = r.WithReferencePoint l._reference
            let mutable lhs = l._lhs
            lhs.SubInPlace r2._lhs
            EllipseRegression2d(l._reference, lhs, l._count - r2._count)

    static member (+) (l : EllipseRegression2d, r : V2d) = l.Add r
    static member (+) (l : V2d, r : EllipseRegression2d) = r.Add l
    static member (-) (l : EllipseRegression2d, r : V2d) = l.Remove r


module EllipseRegression2d =

    /// A regression holding no points.
    let empty = EllipseRegression2d.Empty

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    let inline remove (pt : V2d) (s : EllipseRegression2d) = s.Remove pt

    /// Adds the given point to the regression.
    let inline add (pt : V2d) (s : EllipseRegression2d) = s.Add pt

    /// Gets the least-squares ellipse.
    let inline getEllipse (s : EllipseRegression2d) = s.GetEllipse()

    /// The total number of points added. At least 5 are required to fit an ellipse.
    let inline count (s : EllipseRegression2d) = s.Count

    /// The centroid of all added points.
    let inline centroid (s : EllipseRegression2d) = s.Centroid

    /// Creates a regression from the given points.
    let ofSeq (points : seq<V2d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r

    /// Creates a regression from the given points.
    let ofList (points : list<V2d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r

    /// Creates a regression from the given points.
    let ofArray (points : V2d[]) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r


[<Struct>]
type CircleRegression2d =

    val mutable private _reference : V2d
    /// Σ (p_i - reference) · (p_i - reference)ᵀ, symmetric 2×2 stored as 3 floats.
    val mutable private _lhsM00 : float
    val mutable private _lhsM01 : float
    val mutable private _lhsM11 : float
    /// Σ (p_i - reference)
    val mutable private _sum : V2d
    /// (-Σ xᵢ·|pᵢ|², -Σ yᵢ·|pᵢ|², -Σ |pᵢ|²)  where pᵢ = point - reference
    val mutable private _rhs : V3d
    val mutable private _count : int

    private new(reference : V2d, lhsM00 : float, lhsM01 : float, lhsM11 : float, sum : V2d, rhs : V3d, count : int) =
        { _reference = reference; _lhsM00 = lhsM00; _lhsM01 = lhsM01; _lhsM11 = lhsM11; _sum = sum; _rhs = rhs; _count = count }

    /// Creates a new regression with a single point.
    new(point : V2d) =
        { _reference = point; _lhsM00 = 0.0; _lhsM01 = 0.0; _lhsM11 = 0.0; _sum = V2d.Zero; _rhs = V3d.Zero; _count = 1 }

    /// Creates a new regression from the given points.
    new([<ParamArray>] pts : V2d[]) =
        if pts.Length <= 0 then
            { _reference = V2d.Zero; _lhsM00 = 0.0; _lhsM01 = 0.0; _lhsM11 = 0.0; _sum = V2d.Zero; _rhs = V3d.Zero; _count = 0 }
        else
            let reference = pts.[0]
            let mutable m00 = 0.0
            let mutable m01 = 0.0
            let mutable m11 = 0.0
            let mutable sum = V2d.Zero
            let mutable rhs = V3d.Zero
            let mutable count = 1
            for i in 1 .. pts.Length - 1 do
                let p = pts.[i] - reference
                m00 <- m00 + p.X * p.X
                m01 <- m01 + p.X * p.Y
                m11 <- m11 + p.Y * p.Y
                sum <- sum + p
                let l2 = p.X * p.X + p.Y * p.Y
                rhs <- rhs - V3d(p.X, p.Y, 1.0) * l2
                count <- count + 1
            { _reference = reference; _lhsM00 = m00; _lhsM01 = m01; _lhsM11 = m11; _sum = sum; _rhs = rhs; _count = count }

    new(pts : seq<V2d>) = CircleRegression2d(Seq.toArray pts)
    new(pts : list<V2d>) = CircleRegression2d(List.toArray pts)

    /// A regression holding no points.
    static member Empty =
        CircleRegression2d(V2d.Zero, 0.0, 0.0, 0.0, V2d.Zero, V3d.Zero, 0)

    /// A regression holding no points.
    static member Zero = CircleRegression2d.Empty

    /// The total number of points added. At least 3 are required to fit a circle.
    member x.Count = x._count

    /// The centroid of all added points.
    member x.Centroid =
        if x._count = 0 then V2d.Zero
        else x._reference + x._sum / float x._count

    /// Re-expresses the accumulated statistics relative to a new reference point.
    member private x.WithReferencePoint(r : V2d) =
        let c = x._reference - r
        if x._reference + c = x._reference then x
        elif x._count = 0 then
            CircleRegression2d(r, 0.0, 0.0, 0.0, V2d.Zero, V3d.Zero, 0)
        else
            let sum = x._sum
            let sumSq = V2d(x._lhsM00, x._lhsM11)
            let cov = x._lhsM01
            let c2 = sqr c
            let lc2 = c2.X + c2.Y
            let cs = c * sum
            let n = float x._count

            let dM00 = 2.0 * cs.X + n * c2.X
            let dM11 = 2.0 * cs.Y + n * c2.Y
            let dM01 = c.X * sum.Y + c.Y * sum.X + n * c.X * c.Y

            let drx =
                3.0 * c.X * sumSq.X + c.X * sumSq.Y +
                (3.0 * c2.X + c2.Y) * sum.X +
                2.0 * c.Y * cov +
                2.0 * c.X * cs.Y +
                n * c.X * lc2

            let dry =
                3.0 * c.Y * sumSq.Y + c.Y * sumSq.X +
                (3.0 * c2.Y + c2.X) * sum.Y +
                2.0 * c.X * cov +
                2.0 * c.Y * cs.X +
                n * c.Y * lc2

            let drw =
                2.0 * (cs.X + cs.Y) + n * lc2

            CircleRegression2d(
                r,
                x._lhsM00 + dM00, x._lhsM01 + dM01, x._lhsM11 + dM11,
                sum + n * c,
                x._rhs - V3d(drx, dry, drw),
                x._count)

    /// Adds the given point to the regression.
    member x.Add(pt : V2d) =
        if x._count <= 0 then
            CircleRegression2d(pt, 0.0, 0.0, 0.0, V2d.Zero, V3d.Zero, 1)
        else
            let p = pt - x._reference
            let l2 = p.X * p.X + p.Y * p.Y
            CircleRegression2d(
                x._reference,
                x._lhsM00 + p.X * p.X, x._lhsM01 + p.X * p.Y, x._lhsM11 + p.Y * p.Y,
                x._sum + p,
                x._rhs - V3d(p.X, p.Y, 1.0) * l2,
                x._count + 1)

    /// Adds the given point to the regression (mutating the regression).
    member x.AddInPlace(pt : V2d) =
        if x._count <= 0 then
            x._reference <- pt
            x._lhsM00 <- 0.0; x._lhsM01 <- 0.0; x._lhsM11 <- 0.0
            x._sum <- V2d.Zero
            x._rhs <- V3d.Zero
            x._count <- 1
        else
            let p = pt - x._reference
            let l2 = p.X * p.X + p.Y * p.Y
            x._lhsM00 <- x._lhsM00 + p.X * p.X
            x._lhsM01 <- x._lhsM01 + p.X * p.Y
            x._lhsM11 <- x._lhsM11 + p.Y * p.Y
            x._sum <- x._sum + p
            x._rhs <- x._rhs - V3d(p.X, p.Y, 1.0) * l2
            x._count <- x._count + 1

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.Remove(pt : V2d) =
        if x._count <= 1 then
            CircleRegression2d.Empty
        elif x._count = 2 then
            // the remaining single point was at 2*reference - pt (recover from _sum = other - reference)
            let other = x._sum - (pt - x._reference) + x._reference
            CircleRegression2d(other, 0.0, 0.0, 0.0, V2d.Zero, V3d.Zero, 1)
        else
            let p = pt - x._reference
            let l2 = p.X * p.X + p.Y * p.Y
            CircleRegression2d(
                x._reference,
                x._lhsM00 - p.X * p.X, x._lhsM01 - p.X * p.Y, x._lhsM11 - p.Y * p.Y,
                x._sum - p,
                x._rhs + V3d(p.X, p.Y, 1.0) * l2,
                x._count - 1)

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.RemoveInPlace(pt : V2d) =
        if x._count <= 1 then
            x._reference <- V2d.Zero
            x._lhsM00 <- 0.0; x._lhsM01 <- 0.0; x._lhsM11 <- 0.0
            x._sum <- V2d.Zero
            x._rhs <- V3d.Zero
            x._count <- 0
        elif x._count = 2 then
            let other = x._sum - (pt - x._reference) + x._reference
            x._reference <- other
            x._lhsM00 <- 0.0; x._lhsM01 <- 0.0; x._lhsM11 <- 0.0
            x._sum <- V2d.Zero
            x._rhs <- V3d.Zero
            x._count <- 1
        else
            let p = pt - x._reference
            let l2 = p.X * p.X + p.Y * p.Y
            x._lhsM00 <- x._lhsM00 - p.X * p.X
            x._lhsM01 <- x._lhsM01 - p.X * p.Y
            x._lhsM11 <- x._lhsM11 - p.Y * p.Y
            x._sum <- x._sum - p
            x._rhs <- x._rhs + V3d(p.X, p.Y, 1.0) * l2
            x._count <- x._count - 1

    /// Gets the least-squares circle.
    member x.GetCircle() =
        if x._count < 3 then
            Circle2d(V2d.Zero, 0.0)
        else
            let m =
                M33d(
                    x._lhsM00, x._lhsM01, x._sum.X,
                    x._lhsM01, x._lhsM11, x._sum.Y,
                    x._sum.X,  x._sum.Y,  float x._count)
            let u = m.Inverse * x._rhs
            let center = V2d(-u.X / 2.0, -u.Y / 2.0)
            let r2 = center.LengthSquared - u.Z
            if r2 <= 0.0 then Circle2d(V2d.Zero, 0.0)
            else Circle2d(x._reference + center, sqrt r2)

    static member (+) (l : CircleRegression2d, r : CircleRegression2d) =
        if r._count <= 0 then l
        elif l._count <= 0 then r
        elif l._count = 1 then r.Add l._reference
        elif r._count = 1 then l.Add r._reference
        else
            let r2 = r.WithReferencePoint l._reference
            CircleRegression2d(
                l._reference,
                l._lhsM00 + r2._lhsM00, l._lhsM01 + r2._lhsM01, l._lhsM11 + r2._lhsM11,
                l._sum + r2._sum,
                l._rhs + r2._rhs,
                l._count + r2._count)

    static member (-) (l : CircleRegression2d, r : CircleRegression2d) =
        if r._count <= 0 then l
        elif l._count <= r._count then CircleRegression2d.Empty
        elif r._count = 1 then l.Remove r._reference
        else
            let r2 = r.WithReferencePoint l._reference
            CircleRegression2d(
                l._reference,
                l._lhsM00 - r2._lhsM00, l._lhsM01 - r2._lhsM01, l._lhsM11 - r2._lhsM11,
                l._sum - r2._sum,
                l._rhs - r2._rhs,
                l._count - r2._count)

    static member (+) (l : CircleRegression2d, r : V2d) = l.Add r
    static member (+) (l : V2d, r : CircleRegression2d) = r.Add l
    static member (-) (l : CircleRegression2d, r : V2d) = l.Remove r


module CircleRegression2d =

    /// A regression holding no points.
    let empty = CircleRegression2d.Empty

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    let inline remove (pt : V2d) (s : CircleRegression2d) = s.Remove pt

    /// Adds the given point to the regression.
    let inline add (pt : V2d) (s : CircleRegression2d) = s.Add pt

    /// Gets the least-squares circle.
    let inline getCircle (s : CircleRegression2d) = s.GetCircle()

    /// The total number of points added. At least 3 are required to fit a circle.
    let inline count (s : CircleRegression2d) = s.Count

    /// The centroid of all added points.
    let inline centroid (s : CircleRegression2d) = s.Centroid

    let ofSeq (points : seq<V2d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r

    let ofList (points : list<V2d>) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r

    let ofArray (points : V2d[]) =
        let mutable r = empty
        for p in points do r.AddInPlace p
        r
