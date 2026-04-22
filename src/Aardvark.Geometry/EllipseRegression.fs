namespace Aardvark.Geometry

open System
open Aardvark.Base

[<AutoOpen>]
module private D5Math =

    [<Struct>]
    type V5d =
        val mutable public C0 : float
        val mutable public C1 : float
        val mutable public C2 : float
        val mutable public C3 : float
        val mutable public C4 : float

        new(c0, c1, c2, c3, c4) =
            { C0 = c0; C1 = c1; C2 = c2; C3 = c3; C4 = c4 }

        static member Zero = V5d(0.0, 0.0, 0.0, 0.0, 0.0)

        member x.MultiplyAdd(v : V5d, f : float) =
            x.C0 <- x.C0 + v.C0 * f
            x.C1 <- x.C1 + v.C1 * f
            x.C2 <- x.C2 + v.C2 * f
            x.C3 <- x.C3 + v.C3 * f
            x.C4 <- x.C4 + v.C4 * f

        member x.SetZero() =
            x.C0 <- 0.0
            x.C1 <- 0.0
            x.C2 <- 0.0
            x.C3 <- 0.0
            x.C4 <- 0.0

    [<Struct>]
    type SymmetricM55d =
        val mutable public M00 : float
        val mutable public M01 : float
        val mutable public M02 : float
        val mutable public M03 : float
        val mutable public M04 : float
        val mutable public M11 : float
        val mutable public M12 : float
        val mutable public M13 : float
        val mutable public M14 : float
        val mutable public M22 : float
        val mutable public M23 : float
        val mutable public M24 : float
        val mutable public M33 : float
        val mutable public M34 : float
        val mutable public M44 : float

        new(m00, m01, m02, m03, m04, m11, m12, m13, m14, m22, m23, m24, m33, m34, m44) =
            { M00 = m00; M01 = m01; M02 = m02; M03 = m03; M04 = m04
              M11 = m11; M12 = m12; M13 = m13; M14 = m14
              M22 = m22; M23 = m23; M24 = m24
              M33 = m33; M34 = m34
              M44 = m44 }

        static member Zero =
            SymmetricM55d(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0)

        member x.AddOuterProduct(v : V5d) =
            x.M00 <- x.M00 + v.C0*v.C0
            x.M01 <- x.M01 + v.C0*v.C1
            x.M02 <- x.M02 + v.C0*v.C2
            x.M03 <- x.M03 + v.C0*v.C3
            x.M04 <- x.M04 + v.C0*v.C4
            x.M11 <- x.M11 + v.C1*v.C1
            x.M12 <- x.M12 + v.C1*v.C2
            x.M13 <- x.M13 + v.C1*v.C3
            x.M14 <- x.M14 + v.C1*v.C4
            x.M22 <- x.M22 + v.C2*v.C2
            x.M23 <- x.M23 + v.C2*v.C3
            x.M24 <- x.M24 + v.C2*v.C4
            x.M33 <- x.M33 + v.C3*v.C3
            x.M34 <- x.M34 + v.C3*v.C4
            x.M44 <- x.M44 + v.C4*v.C4

        member x.SubOuterProduct(v : V5d) =
            x.M00 <- x.M00 - v.C0*v.C0
            x.M01 <- x.M01 - v.C0*v.C1
            x.M02 <- x.M02 - v.C0*v.C2
            x.M03 <- x.M03 - v.C0*v.C3
            x.M04 <- x.M04 - v.C0*v.C4
            x.M11 <- x.M11 - v.C1*v.C1
            x.M12 <- x.M12 - v.C1*v.C2
            x.M13 <- x.M13 - v.C1*v.C3
            x.M14 <- x.M14 - v.C1*v.C4
            x.M22 <- x.M22 - v.C2*v.C2
            x.M23 <- x.M23 - v.C2*v.C3
            x.M24 <- x.M24 - v.C2*v.C4
            x.M33 <- x.M33 - v.C3*v.C3
            x.M34 <- x.M34 - v.C3*v.C4
            x.M44 <- x.M44 - v.C4*v.C4

        member x.SetZero() =
            x.M00 <- 0.0; x.M01 <- 0.0; x.M02 <- 0.0; x.M03 <- 0.0; x.M04 <- 0.0
            x.M11 <- 0.0; x.M12 <- 0.0; x.M13 <- 0.0; x.M14 <- 0.0
            x.M22 <- 0.0; x.M23 <- 0.0; x.M24 <- 0.0
            x.M33 <- 0.0; x.M34 <- 0.0
            x.M44 <- 0.0

        member x.AddInPlace(o : SymmetricM55d) =
            x.M00 <- x.M00 + o.M00; x.M01 <- x.M01 + o.M01; x.M02 <- x.M02 + o.M02; x.M03 <- x.M03 + o.M03; x.M04 <- x.M04 + o.M04
            x.M11 <- x.M11 + o.M11; x.M12 <- x.M12 + o.M12; x.M13 <- x.M13 + o.M13; x.M14 <- x.M14 + o.M14
            x.M22 <- x.M22 + o.M22; x.M23 <- x.M23 + o.M23; x.M24 <- x.M24 + o.M24
            x.M33 <- x.M33 + o.M33; x.M34 <- x.M34 + o.M34
            x.M44 <- x.M44 + o.M44

        member x.SubInPlace(o : SymmetricM55d) =
            x.M00 <- x.M00 - o.M00; x.M01 <- x.M01 - o.M01; x.M02 <- x.M02 - o.M02; x.M03 <- x.M03 - o.M03; x.M04 <- x.M04 - o.M04
            x.M11 <- x.M11 - o.M11; x.M12 <- x.M12 - o.M12; x.M13 <- x.M13 - o.M13; x.M14 <- x.M14 - o.M14
            x.M22 <- x.M22 - o.M22; x.M23 <- x.M23 - o.M23; x.M24 <- x.M24 - o.M24
            x.M33 <- x.M33 - o.M33; x.M34 <- x.M34 - o.M34
            x.M44 <- x.M44 - o.M44

        member x.ToMatrix() =
            let arr = Array.zeroCreate 25
            arr.[ 0] <- x.M00; arr.[ 1] <- x.M01; arr.[ 2] <- x.M02; arr.[ 3] <- x.M03; arr.[ 4] <- x.M04
            arr.[ 5] <- x.M01; arr.[ 6] <- x.M11; arr.[ 7] <- x.M12; arr.[ 8] <- x.M13; arr.[ 9] <- x.M14
            arr.[10] <- x.M02; arr.[11] <- x.M12; arr.[12] <- x.M22; arr.[13] <- x.M23; arr.[14] <- x.M24
            arr.[15] <- x.M03; arr.[16] <- x.M13; arr.[17] <- x.M23; arr.[18] <- x.M33; arr.[19] <- x.M34
            arr.[20] <- x.M04; arr.[21] <- x.M14; arr.[22] <- x.M24; arr.[23] <- x.M34; arr.[24] <- x.M44
            Matrix<float>(arr, V2l(5L, 5L))

        /// Solves `x * result = rhs` via Aardvark.Base LuFactorize/LuSolve.
        /// Returns `V5d.Zero` if singular.
        member x.LuSolve(rhs : V5d) =
            let m = x.ToMatrix()
            let b = Vector<float>([| rhs.C0; rhs.C1; rhs.C2; rhs.C3; rhs.C4 |])
            let perm = m.LuFactorize()
            if isNull perm then V5d.Zero
            else
                let sol = m.LuSolve(perm, b)
                V5d(sol.[0L], sol.[1L], sol.[2L], sol.[3L], sol.[4L])


/// Incremental least-squares ellipse fit in 2D.
///
/// Fits the general conic  A*x² + B*x*y + C*y² + D*x + E*y + F = 0
/// using a trace-fixed normalization (A + C = -2), yielding a 5×5 linear system.
/// Supports incremental Add/Remove and a GetEllipse extraction.
///
/// NOTE: this is an algebraic fit to a general conic (same caveat as
/// EllipsoidRegression3d) — data that is actually elliptical will fit to an
/// ellipse; strongly non-elliptical data can fall through to a non-ellipse
/// parameter set in which case GetEllipse returns Ellipse2d with zero radii.
[<Struct>]
type EllipseRegression2d =

    val mutable private _reference : V2d
    val mutable private _lhs : SymmetricM55d
    val mutable private _rhs : V5d
    val mutable private _sum : V2d
    val mutable private _count : int

    private new(reference : V2d, lhs : SymmetricM55d, rhs : V5d, sum : V2d, count : int) =
        { _reference = reference; _lhs = lhs; _rhs = rhs; _sum = sum; _count = count }

    /// Per-point feature vector v and target t such that
    /// v · u = t  encodes  A*x² + B*x*y + C*y² + D*x + E*y + F = 0
    /// with  A = u0 - 1, B = 2*u1, C = -u0 - 1, D = 2*u2, E = 2*u3, F = u4.
    static member private Feature(px : float, py : float) =
        V5d(px*px - py*py, 2.0*px*py, 2.0*px, 2.0*py, 1.0)

    static member private Target(px : float, py : float) = px*px + py*py

    /// Creates a new regression from the given points.
    new([<ParamArray>] pts : V2d[]) =
        if pts.Length <= 0 then
            { _reference = V2d.Zero; _lhs = SymmetricM55d.Zero; _rhs = V5d.Zero; _sum = V2d.Zero; _count = 0 }
        else
            let reference = pts.[0]
            // first point contributes v = (0,0,0,0,1), t = 0 → only M44 += 1
            let mutable lhs = SymmetricM55d(M44 = 1.0)
            let mutable rhs = V5d.Zero
            let mutable sum = V2d.Zero
            let mutable count = 1
            for i in 1 .. pts.Length - 1 do
                let pt = pts.[i] - reference
                let v = EllipseRegression2d.Feature(pt.X, pt.Y)
                let t = EllipseRegression2d.Target(pt.X, pt.Y)
                lhs.AddOuterProduct v
                rhs.MultiplyAdd(v, t)
                sum <- sum + pt
                count <- count + 1
            { _reference = reference; _lhs = lhs; _rhs = rhs; _sum = sum; _count = count }

    /// Creates a new regression from the given points.
    new(pts : seq<V2d>) = EllipseRegression2d(Seq.toArray pts)
    /// Creates a new regression from the given points.
    new(pts : list<V2d>) = EllipseRegression2d(List.toArray pts)
    /// Creates a new regression with a single point.
    new(point : V2d) =
        { _reference = point
          _lhs = SymmetricM55d(M44 = 1.0)
          _rhs = V5d.Zero
          _sum = V2d.Zero
          _count = 1 }

    /// A regression holding no points.
    static member Empty =
        EllipseRegression2d(V2d.Zero, SymmetricM55d.Zero, V5d.Zero, V2d.Zero, 0)

    /// A regression holding no points.
    static member Zero = EllipseRegression2d.Empty

    /// The total number of points added. At least 5 are required to fit an ellipse.
    member x.Count = x._count

    /// The centroid of all added points.
    member x.Centroid =
        if x._count = 0 then V2d.Zero
        else x._reference + x._sum / float x._count

    /// Re-expresses the accumulated statistics relative to a new reference point.
    /// The underlying fit is unchanged; this only moves the local origin.
    ///
    /// Math: for feature v = (x²-y², 2xy, 2x, 2y, 1) and target t = x²+y²
    /// under a shift x = x' - c.X, y = y' - c.Y (c = old_reference - new_reference):
    ///   v' = A · v           (A is 5×5 upper-triangular-ish)
    ///   t' = t + b · v       (b = (0, 0, cx, cy, cx²+cy²))
    /// hence
    ///   lhs' = A · lhs · Aᵀ
    ///   rhs' = A · rhs + A · lhs · b
    member private x.WithReferencePoint(r : V2d) =
        let c = x._reference - r
        if x._reference + c = x._reference then x
        elif x._count = 0 then
            EllipseRegression2d(r, SymmetricM55d.Zero, V5d.Zero, V2d.Zero, 0)
        else
            let cx = c.X
            let cy = c.Y

            // A
            let a = Array2D.zeroCreate 5 5
            a.[0,0] <- 1.0; a.[0,2] <- cx;  a.[0,3] <- -cy; a.[0,4] <- cx*cx - cy*cy
            a.[1,1] <- 1.0; a.[1,2] <- cy;  a.[1,3] <-  cx; a.[1,4] <- 2.0*cx*cy
            a.[2,2] <- 1.0;                                 a.[2,4] <- 2.0*cx
            a.[3,3] <- 1.0;                                 a.[3,4] <- 2.0*cy
            a.[4,4] <- 1.0

            // L (dense 5×5 from symmetric lhs)
            let l = Array2D.zeroCreate 5 5
            let lhs = x._lhs
            l.[0,0] <- lhs.M00; l.[0,1] <- lhs.M01; l.[0,2] <- lhs.M02; l.[0,3] <- lhs.M03; l.[0,4] <- lhs.M04
            l.[1,0] <- lhs.M01; l.[1,1] <- lhs.M11; l.[1,2] <- lhs.M12; l.[1,3] <- lhs.M13; l.[1,4] <- lhs.M14
            l.[2,0] <- lhs.M02; l.[2,1] <- lhs.M12; l.[2,2] <- lhs.M22; l.[2,3] <- lhs.M23; l.[2,4] <- lhs.M24
            l.[3,0] <- lhs.M03; l.[3,1] <- lhs.M13; l.[3,2] <- lhs.M23; l.[3,3] <- lhs.M33; l.[3,4] <- lhs.M34
            l.[4,0] <- lhs.M04; l.[4,1] <- lhs.M14; l.[4,2] <- lhs.M24; l.[4,3] <- lhs.M34; l.[4,4] <- lhs.M44

            // AL = A · L
            let al = Array2D.zeroCreate 5 5
            for i in 0..4 do
                for j in 0..4 do
                    let mutable s = 0.0
                    for k in 0..4 do s <- s + a.[i,k] * l.[k,j]
                    al.[i,j] <- s

            // ALAt = AL · Aᵀ (symmetric; we read upper triangle)
            let e i j =
                let mutable s = 0.0
                for k in 0..4 do s <- s + al.[i,k] * a.[j,k]
                s

            let newLhs =
                SymmetricM55d(
                    e 0 0, e 0 1, e 0 2, e 0 3, e 0 4,
                           e 1 1, e 1 2, e 1 3, e 1 4,
                                  e 2 2, e 2 3, e 2 4,
                                         e 3 3, e 3 4,
                                                e 4 4)

            // b and rhs update: rhs' = A·rhs + AL·b
            let b = [| 0.0; 0.0; cx; cy; cx*cx + cy*cy |]
            let r0 = x._rhs
            let rarr = [| r0.C0; r0.C1; r0.C2; r0.C3; r0.C4 |]
            let outRhs = Array.zeroCreate 5
            for i in 0..4 do
                let mutable s = 0.0
                for k in 0..4 do s <- s + a.[i,k] * rarr.[k] + al.[i,k] * b.[k]
                outRhs.[i] <- s
            let newRhs = V5d(outRhs.[0], outRhs.[1], outRhs.[2], outRhs.[3], outRhs.[4])

            let newSum = x._sum + float x._count * c

            EllipseRegression2d(r, newLhs, newRhs, newSum, x._count)

    /// Adds the given point to the regression.
    member x.Add(pt : V2d) =
        if x._count <= 0 then
            EllipseRegression2d(pt, SymmetricM55d(M44 = 1.0), V5d.Zero, V2d.Zero, 1)
        else
            let pt = pt - x._reference
            let v = EllipseRegression2d.Feature(pt.X, pt.Y)
            let t = EllipseRegression2d.Target(pt.X, pt.Y)
            let mutable lhs = x._lhs
            let mutable rhs = x._rhs
            lhs.AddOuterProduct v
            rhs.MultiplyAdd(v, t)
            EllipseRegression2d(x._reference, lhs, rhs, x._sum + pt, x._count + 1)

    /// Adds the given point to the regression (mutating the regression).
    member x.AddInPlace(pt : V2d) =
        if x._count <= 0 then
            x._reference <- pt
            x._lhs.SetZero()
            x._lhs.M44 <- 1.0
            x._rhs.SetZero()
            x._sum <- V2d.Zero
            x._count <- 1
        else
            let pt = pt - x._reference
            let v = EllipseRegression2d.Feature(pt.X, pt.Y)
            let t = EllipseRegression2d.Target(pt.X, pt.Y)
            x._lhs.AddOuterProduct v
            x._rhs.MultiplyAdd(v, t)
            x._sum <- x._sum + pt
            x._count <- x._count + 1

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.Remove(pt : V2d) =
        if x._count <= 1 then
            EllipseRegression2d.Empty
        else
            let pt = pt - x._reference
            let v = EllipseRegression2d.Feature(pt.X, pt.Y)
            let t = EllipseRegression2d.Target(pt.X, pt.Y)
            let mutable lhs = x._lhs
            let mutable rhs = x._rhs
            lhs.SubOuterProduct v
            rhs.MultiplyAdd(v, -t)
            EllipseRegression2d(x._reference, lhs, rhs, x._sum - pt, x._count - 1)

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.RemoveInPlace(pt : V2d) =
        if x._count <= 1 then
            x._reference <- V2d.Zero
            x._lhs.SetZero()
            x._rhs.SetZero()
            x._sum <- V2d.Zero
            x._count <- 0
        else
            let pt = pt - x._reference
            let v = EllipseRegression2d.Feature(pt.X, pt.Y)
            let t = EllipseRegression2d.Target(pt.X, pt.Y)
            x._lhs.SubOuterProduct v
            x._rhs.MultiplyAdd(v, -t)
            x._sum <- x._sum - pt
            x._count <- x._count - 1

    /// Gets the least-squares ellipse.
    /// Returns Ellipse2d with zero radii when the fit is degenerate or the
    /// conic is not an ellipse (e.g. 4AC - B² ≤ 0).
    member x.GetEllipse() =
        if x._count < 5 then
            Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero)
        else
            let u = x._lhs.LuSolve x._rhs

            // recover conic coefficients
            let A =  u.C0 - 1.0
            let B =  2.0 * u.C1
            let C = -u.C0 - 1.0
            let D =  2.0 * u.C2
            let E =  2.0 * u.C3
            let F =  u.C4

            // discriminant of the quadratic form
            let det4 = 4.0 * A * C - B * B
            if Fun.IsTiny det4 || det4 <= 0.0 then
                // parabolic / hyperbolic / degenerate
                Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero)
            else
                // center: [2A B; B 2C] [cx; cy] = -[D; E]
                let cx = (B * E - 2.0 * C * D) / det4
                let cy = (B * D - 2.0 * A * E) / det4

                // constant after translating to center: F' = F + (D*cx + E*cy) / 2
                let F' = F + 0.5 * (D * cx + E * cy)

                // centered form: (p')^T M (p') = -F'  where M = [[A, B/2],[B/2, C]]
                // semi-axis along eigenvector of eigenvalue λ has length² = -F' / λ.
                let a = A
                let b = B / 2.0
                let c = C
                let tr = a + c
                let dt = a * c - b * b
                let disc = tr * tr - 4.0 * dt |> max 0.0
                let s = sqrt disc
                let l0 = 0.5 * (tr + s)       // larger (closer to 0 since both negative)
                let l1 = 0.5 * (tr - s)       // smaller (more negative)

                let r0sq = -F' / l0           // squared semi-axis along eigvec(l0) → major
                let r1sq = -F' / l1           // squared semi-axis along eigvec(l1) → minor

                if r0sq <= 0.0 || r1sq <= 0.0 then
                    Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero)
                else
                    let rMajor = sqrt r0sq
                    let rMinor = sqrt r1sq

                    // eigenvector for λ: M v = λ v ⇒ (a - λ) vx + b vy = 0 ⇒ v = (-b, a - λ) or (λ - c, b)
                    let majorDir =
                        if abs b > 1e-12 then
                            V2d(-b, a - l0) |> Vec.normalize
                        elif a >= c then
                            V2d.IO            // a is the larger eigenvalue → x-axis is major
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
            let rhs = V5d(l._rhs.C0 + r2._rhs.C0, l._rhs.C1 + r2._rhs.C1,
                          l._rhs.C2 + r2._rhs.C2, l._rhs.C3 + r2._rhs.C3,
                          l._rhs.C4 + r2._rhs.C4)
            EllipseRegression2d(l._reference, lhs, rhs, l._sum + r2._sum, l._count + r2._count)

    static member (-) (l : EllipseRegression2d, r : EllipseRegression2d) =
        if r._count <= 0 then l
        elif l._count <= r._count then EllipseRegression2d.Empty
        elif r._count = 1 then l.Remove r._reference
        else
            let r2 = r.WithReferencePoint l._reference
            let mutable lhs = l._lhs
            lhs.SubInPlace r2._lhs
            let rhs = V5d(l._rhs.C0 - r2._rhs.C0, l._rhs.C1 - r2._rhs.C1,
                          l._rhs.C2 - r2._rhs.C2, l._rhs.C3 - r2._rhs.C3,
                          l._rhs.C4 - r2._rhs.C4)
            EllipseRegression2d(l._reference, lhs, rhs, l._sum - r2._sum, l._count - r2._count)

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
