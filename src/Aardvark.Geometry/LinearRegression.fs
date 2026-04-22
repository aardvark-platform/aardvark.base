namespace Aardvark.Geometry

open System
open Aardvark.Base
open System.Collections.Generic

[<AutoOpen>]
module private RegressionHelpers =

    /// Covariance entries + sorted eigenvalues (|l0| ≤ |l1| ≤ |l2|) + centroid offset.
    /// Shared by all 3D regression getters that depend on the PCA of the point set.
    [<Struct>]
    type EigenData3d = {
        Valid : bool
        Xx : float; Yy : float; Zz : float
        Xy : float; Xz : float; Yz : float
        L0 : float; L1 : float; L2 : float
        Avg : V3d
    }

    let emptyEigen3d =
        { Valid = false
          Xx = 0.0; Yy = 0.0; Zz = 0.0
          Xy = 0.0; Xz = 0.0; Yz = 0.0
          L0 = 0.0; L1 = 0.0; L2 = 0.0
          Avg = V3d.Zero }

    /// Eigenvector of `M` for eigenvalue `lam`: the null space of (M - lam·I), recovered by
    /// crossing the two longest columns of that matrix (they span the orthogonal complement).
    let eigenvector3d (d : EigenData3d) (lam : float) =
        let c0 = V3d(d.Xx - lam, d.Xy, d.Xz)
        let c1 = V3d(d.Xy, d.Yy - lam, d.Yz)
        let c2 = V3d(d.Xz, d.Yz, d.Zz - lam)
        let len0 = Vec.lengthSquared c0
        let len1 = Vec.lengthSquared c1
        let len2 = Vec.lengthSquared c2
        if len0 > len1 then
            if len2 > len1 then Vec.cross c0 c2 |> Vec.normalize
            else Vec.cross c0 c1 |> Vec.normalize
        else
            if len2 > len0 then Vec.cross c1 c2 |> Vec.normalize
            else Vec.cross c0 c1 |> Vec.normalize

    let inline sortTupleAbs (struct(a,b)) =
        if abs a < abs b then struct(a, b)
        else struct(b, a)

    let inline sortTripleAbs (struct(a,b,c)) =
        let aa = abs a
        let bb = abs b
        let cc = abs c
        if aa < bb then   
            // a < b
            if bb < cc then struct(a,b,c)
            elif aa < cc then struct(a,c,b)
            else struct(c,a,b)
        else
            // b < a
            if aa < cc then struct(b,a,c)
            elif bb < cc then struct(b,c,a)
            else struct(c,b,a)



[<Struct>]
type LinearRegression2d =
    
    val mutable private _reference : V2d
    val mutable private _sum : V2d
    val mutable private _sumSq : V2d
    val mutable private _off : float
    val mutable private _count : int
    
    private new(reference : V2d, sum : V2d, sumSq : V2d, off : float, count : int) =
        { _reference = reference; _sum = sum; _sumSq = sumSq; _off = off; _count = count }
        
    /// Creates a new regression with a single point.
    new(point : V2d) =
        { _reference = point; _sum = V2d.Zero; _sumSq = V2d.Zero; _off = 0.0; _count = 1 }
        
    /// Creates a new regression from the given points.
    new([<ParamArray>] points : V2d[]) =
        if points.Length = 0 then
            { _reference = V2d.Zero; _sum = V2d.Zero; _sumSq = V2d.Zero; _off = 0.0; _count = 0 }
        else
            let reference = points.[0]
            let mutable sum = V2d.Zero
            let mutable sumSq = V2d.Zero
            let mutable off = 0.0
            let mutable count = 1

            for i in 1 .. points.Length - 1 do
                let pt = points.[i] - reference
                sum <- sum + pt
                sumSq <- sumSq + sqr pt
                off <- off + pt.X*pt.Y
                count <- count + 1
            
            { _reference = reference; _sum = sum; _sumSq = sumSq; _off = off; _count = count }
         
    /// Creates a new regression from the given points.
    new(s : seq<V2d>) = LinearRegression2d(Seq.toArray s)
    
    /// Creates a new regression from the given points.
    new(s : list<V2d>) = LinearRegression2d(List.toArray s)
    
    /// A regression holding no points.
    static member Zero = LinearRegression2d(V2d.Zero, V2d.Zero, V2d.Zero, 0.0, 0)

    /// A regression holding no points.
    static member Empty = LinearRegression2d(V2d.Zero, V2d.Zero, V2d.Zero, 0.0, 0)
   
    /// The total number of points added. Note that at least 3 points are required to fit a plane.
    member x.Count = x._count

    member private x.WithReferencePoint(r : V2d) =
        let d = x._reference - r
        if r + d = r then
            x
        else
            let n = float x._count

            // sum ((xi+dx)*(yi+dy))
            // sum (xi*yi + dx*yi + dy*xi + dx*dy)
            // sum (xi*yi) + dx*sum(yi) + dy*sum(xi) + n*dx*dy
            let vxy = d.X*x._sum.Y + d.Y*x._sum.X + n*d.X*d.Y

            // sum (xi+d)^2 
            // sum (xi^2 + 2xi*d + d^2)
            // sum (xi^2) + 2*d*sum(xi) + n*d^2
            LinearRegression2d(
                r, 
                x._sum + n*d,
                x._sumSq + 2.0*d*x._sum + n*sqr d,
                x._off + vxy,
                x._count
            )

    /// The centroid of all added points.
    member x.Centroid = 
        if x._count = 0 then V2d.Zero
        else x._reference + x._sum / float x._count


    /// Adds the given point to the regression.
    member x.Add(pt : V2d) =
        if x._count = 0 then
            LinearRegression2d(pt, V2d.Zero, V2d.Zero, 0.0, 1)
        else
            let pt = pt - x._reference
            LinearRegression2d(
                x._reference,
                x._sum + pt,
                x._sumSq + sqr pt,
                x._off + pt.X*pt.Y,
                x._count + 1
            )

    /// Adds the given point to the regression (mutating the regression).
    member x.AddInPlace(pt : V2d) =
        if x._count = 0 then
            x._reference <- pt
            x._sum <- V2d.Zero
            x._sumSq <- V2d.Zero
            x._off <- 0.0
            x._count <- 1
        else
            let pt = pt - x._reference
            x._sum <- x._sum + pt
            x._sumSq <- x._sumSq + sqr pt
            x._off <- x._off + pt.X*pt.Y
            x._count <- x._count + 1
    
            
    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.Remove(pt : V2d) =
        if x._count <= 1 then
            LinearRegression2d.Empty
        else
            let pt = pt - x._reference
            LinearRegression2d(
                x._reference,
                x._sum - pt,
                x._sumSq - sqr pt |> max V2d.Zero,
                x._off - pt.X*pt.Y,
                x._count - 1
            )
 
    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.RemoveInPlace(pt : V2d) =
        if x._count <= 1 then
            x._reference <- V2d.Zero
            x._sum <- V2d.Zero
            x._sumSq <- V2d.Zero
            x._off <- 0.0
            x._count <- 0
        else
            let pt = pt - x._reference
            x._sum <- x._sum - pt
            x._sumSq <- x._sumSq - sqr pt
            x._off <- x._off - pt.X*pt.Y
            x._count <- x._count - 1

 
    /// Gets the variances as [Var(X), Var(Y), Var(Z)]
    member x.Variance =
        if x._count < 2 then
            V2d.Zero
        else
            let n = float x._count
            let avg = x._sum / n
            let xx = (x._sumSq.X - avg.X * x._sum.X) / (n - 1.0)
            let yy = (x._sumSq.Y - avg.Y * x._sum.Y) / (n - 1.0)
            V2d(xx, yy)

    /// Gets the covariance.
    member x.Covariance =
        if x._count < 2 then
            0.0
        else
            let n = float x._count
            let avg = x._sum / n
            let xy = (x._off - avg.X * x._sum.Y) / (n - 1.0)
            xy
 

    /// Gets the least-squares plane and a confidence value in [0,1] where 1 stands for a perfect plane.
    member x.GetPlaneAndConfidence() =
        if x._count < 2 then
            struct(Plane2d.Invalid, 0.0)
        else
            let n = float x._count
            let avg = x._sum / n
            let xx = (x._sumSq.X - avg.X * x._sum.X) / (n - 1.0)
            let yy = (x._sumSq.Y - avg.Y * x._sum.Y) / (n - 1.0)

            let xy = (x._off - avg.X * x._sum.Y) / (n - 1.0)
            
            // (xx-l)*(yy-l) - xy^2 = 0
            // l^2 - l*(xx+yy) + xx*yy - xy^2 = 0


            let _a = 1.0
            let b = -(xx + yy)
            let c = xx*yy - sqr xy


            let struct (l0, l1) = sortTupleAbs (Polynomial.RealRootsOfNormed(b, c))
            if Fun.IsTiny l1 then
                struct(Plane2d.Invalid, 0.0)
            else
                let goodness = 1.0 - abs l0 / abs l1
                
                if goodness > 1E-5 then
                    let a = V2d(xx - l0, xy)
                    let b = V2d(xy, yy - l0)
                    
                    let dd = 
                        if Vec.length a > Vec.length b then a
                        else b
                        
                    let normal = V2d(-dd.Y, dd.X) |> Vec.normalize
                    struct(Plane2d(normal, x._reference + avg), goodness)
                else
                    struct(Plane2d.Invalid, goodness)

    /// Gets a Trafo3d for the entire plane (using its principal components) and the respective standard deviations for each axis.
    member x.GetTrafoAndSizes() =
        if x._count < 2 then
            struct(Trafo2d.Identity, V2d.Zero)
        else
            let n = float x._count
            let avg = x._sum / n
            let xx = (x._sumSq.X - avg.X * x._sum.X) / (n - 1.0)
            let yy = (x._sumSq.Y - avg.Y * x._sum.Y) / (n - 1.0)

            let xy = (x._off - avg.X * x._sum.Y) / (n - 1.0)
            
            let _a = 1.0
            let b = -(xx + yy)
            let c = xx*yy - sqr xy

            let struct (l0, l1) = sortTupleAbs (Polynomial.RealRootsOfNormed(b, c))
            if Fun.IsTiny l1 then
                struct (Trafo2d.Identity, x._reference + avg)
            else
                let vx =
                    let a = V2d(xx - l0, xy)
                    let b = V2d(xy, yy - l0)
                    
                    let dd = 
                        if Vec.length a > Vec.length b then a
                        else b
                    Vec.normalize dd
                let vy = V2d(-vx.Y, vx.X)
               
                let s0 = if l0 < 0.0 then 0.0 else sqrt l0
                let s1 = if l1 < 0.0 then 0.0 else sqrt l1
                let dev = V2d(s1,s0)

                let trafo =
                    let o = x._reference + avg
                    Trafo2d(
                        M33d(
                            vx.X, vy.X, o.X,
                            vx.Y, vy.Y, o.Y,
                            0.0,  0.0,  1.0
                        ),
                        M33d(
                            vx.X, vx.Y, -Vec.dot vx o,
                            vy.X, vy.Y, -Vec.dot vy o,
                            0.0,  0.0,   1.0
                        )
                    )
                struct(trafo, dev)
   
    /// Gets the least-squares plane.
    member x.GetPlane() =
        let struct(p, _) = x.GetPlaneAndConfidence()
        p
        
    /// Gets a Trafo3d for the entire plane using its principal components.
    member x.GetTrafo() =
        let struct(t, _) = x.GetTrafoAndSizes()
        t
  
    static member (+) (l : LinearRegression2d, r : LinearRegression2d) =
        if r.Count <= 0 then l
        elif l.Count <= 0 then r
        elif l.Count = 1 then r.Add l._reference
        elif r.Count = 1 then l.Add r._reference
        else
            let r1 = r.WithReferencePoint l._reference
            LinearRegression2d(
                l._reference,
                l._sum + r1._sum,
                l._sumSq + r1._sumSq,
                l._off + r1._off,
                l._count + r1._count
            )
        
    static member (-) (l : LinearRegression2d, r : LinearRegression2d) =
        if r.Count <= 0 then l
        elif l.Count <= r.Count then LinearRegression2d.Zero
        elif r.Count = 1 then l.Remove r._reference
        else
            let r = r.WithReferencePoint l._reference
            LinearRegression2d(
                l._reference,
                l._sum - r._sum,
                l._sumSq - r._sumSq,
                l._off - r._off,
                l._count - r._count
            )
        
    static member (+) (l : LinearRegression2d, r : V2d) =
        l.Add r
        
    static member (+) (l : V2d, r : LinearRegression2d) =
        r.Add l
        
    static member (-) (l : LinearRegression2d, r : V2d) =
        l.Remove r

module LinearRegression2d =

    /// A regression holding no points.
    let empty = LinearRegression2d.Zero

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    let inline remove (pt : V2d) (s : LinearRegression2d) = s.Remove pt

    /// Adds the given point to the regression.
    let inline add (pt : V2d) (s : LinearRegression2d) = s.Add pt

    /// Gets the least-squares plane.
    let inline getPlane (s : LinearRegression2d) = s.GetPlane()

    /// Gets a Trafo2d for the entire plane using its principal components.
    let inline getTrafo (s : LinearRegression2d) = s.GetTrafo()
    
    /// The total number of points added. Note that at least 2 points are required to fit a line.
    let inline count (s : LinearRegression2d) = s.Count
    
    /// The centroid of all added points.
    let inline centroid (s : LinearRegression2d) = s.Centroid
    
    /// Gets the variances as [Var(X), Var(Y)]
    let inline variance (s : LinearRegression2d) = s.Variance

    /// Gets the covariance.
    let inline covariance (s : LinearRegression2d) = s.Covariance

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



/// Combined result of a 3D linear regression, summarizing the principal-component
/// analysis of the point cloud. `Linearity`, `Planarity`, `Sphericity` ∈ [0,1] sum
/// to 1 and say which of {line, plane, isotropic blob} the data best resembles.
[<Struct>]
type LinearRegression3dResult = {
    /// Least-squares plane through the centroid; best when `Planarity` is near 1.
    Plane      : Plane3d
    /// Least-squares line through the centroid (as a Ray3d used as an unbounded
    /// line); best when `Linearity` is near 1.
    Line       : Ray3d
    /// Close to 1 when points lie along a line (λ₂ ≫ λ₁ ≈ λ₀).
    Linearity  : float
    /// Close to 1 when points lie in a plane (λ₂ ≈ λ₁ ≫ λ₀).
    Planarity  : float
    /// Close to 1 when points are isotropically distributed (λ₂ ≈ λ₁ ≈ λ₀).
    Sphericity : float
}

[<Struct>]
type LinearRegression3d =
    val mutable public Reference : V3d
    val mutable public Sum : V3d
    val mutable public SumSq : V3d
    val mutable public Off : V3d
    
    /// The total number of points added. Note that at least 3 points are required to fit a plane.
    val mutable public Count : int

    new(reference : V3d, sum : V3d, sumSq : V3d, off : V3d, count : int) =
        { Reference = reference; Sum = sum; SumSq = sumSq; Off = off; Count = count }

    /// Creates a new regression with a single point.
    new(point : V3d) =
        { Reference = point; Sum = V3d.Zero; SumSq = V3d.Zero; Off = V3d.Zero; Count = 1 }
        
    /// Creates a new regression from the given points.
    new([<ParamArray>] points : V3d[]) =
        if points.Length = 0 then
            { Reference = V3d.Zero; Sum = V3d.Zero; SumSq = V3d.Zero; Off = V3d.Zero; Count = 0 }
        else
            let reference = points.[0]
            let mutable sum = V3d.Zero
            let mutable sumSq = V3d.Zero
            let mutable off = V3d.Zero
            let mutable count = 1

            for i in 1 .. points.Length - 1 do
                let pt = points.[i] - reference
                sum <- sum + pt
                sumSq <- sumSq + sqr pt
                off <- off + V3d(pt.Y*pt.Z, pt.X*pt.Z, pt.X*pt.Y)
                count <- count + 1
            
            { Reference = reference; Sum = sum; SumSq = sumSq; Off = off; Count = count }
        
    /// Creates a new regression from the given points.
    new(s : seq<V3d>) = LinearRegression3d(Seq.toArray s)
    
    /// Creates a new regression from the given points.
    new(s : list<V3d>) = LinearRegression3d(List.toArray s)


    /// A regression holding no points.
    static member Zero = LinearRegression3d(V3d.Zero, V3d.Zero, V3d.Zero, V3d.Zero, 0)

    /// A regression holding no points.
    static member Empty = LinearRegression3d(V3d.Zero, V3d.Zero, V3d.Zero, V3d.Zero, 0)
    
    /// The total number of points added. Note that at least 3 points are required to fit a plane.
    // member x.Count = x._count

    member private x.WithReferencePoint(r : V3d) =
        let d = x.Reference - r
        if r + d = r then
            x
        else
            let n = float x.Count

            // sum ((xi+dx)*(yi+dy))
            // sum (xi*yi + dx*yi + dy*xi + dx*dy)
            // sum (xi*yi) + dx*sum(yi) + dy*sum(xi) + n*dx*dy
            let vxy = d.X*x.Sum.Y + d.Y*x.Sum.X + n*d.X*d.Y
            let vxz = d.X*x.Sum.Z + d.Z*x.Sum.X + n*d.X*d.Z
            let vyz = d.Y*x.Sum.Z + d.Z*x.Sum.Y + n*d.Y*d.Z

            // sum (xi+d)^2 
            // sum (xi^2 + 2xi*d + d^2)
            // sum (xi^2) + 2*d*sum(xi) + n*d^2

            LinearRegression3d(
                r, 
                x.Sum + n*d,
                x.SumSq + 2.0*d*x.Sum + n*sqr d,
                x.Off + V3d(vyz, vxz, vxy),
                x.Count
            )

    /// The centroid of all added points.
    member x.Centroid = 
        if x.Count = 0 then V3d.Zero
        else x.Reference + x.Sum / float x.Count

    /// Adds the given point to the regression.
    member x.Add(pt : V3d) =
        if x.Count = 0 then
            LinearRegression3d(pt, V3d.Zero, V3d.Zero, V3d.Zero, 1)
        else
            let pt = pt - x.Reference
            LinearRegression3d(
                x.Reference,
                x.Sum + pt,
                x.SumSq + sqr pt,
                x.Off + V3d(pt.Y*pt.Z, pt.X*pt.Z, pt.X*pt.Y),
                x.Count + 1
            )

    /// Adds the given point to the regression (mutating the regression).
    member x.AddInPlace(pt : V3d) =
        if x.Count = 0 then
            x.Reference <- pt
            x.Sum <- V3d.Zero
            x.SumSq <- V3d.Zero
            x.Off <- V3d.Zero
            x.Count <- 1
        else
            let pt = pt - x.Reference
            x.Sum <- x.Sum + pt
            x.SumSq <- x.SumSq + sqr pt
            x.Off <- x.Off + V3d(pt.Y*pt.Z, pt.X*pt.Z, pt.X*pt.Y)
            x.Count <- x.Count + 1
            
            
    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.Remove(pt : V3d) =
        if x.Count <= 1 then
            LinearRegression3d.Empty
        else
            let pt = pt - x.Reference
            LinearRegression3d(
                x.Reference,
                x.Sum - pt,
                x.SumSq - sqr pt |> max V3d.Zero,
                x.Off - V3d(pt.Y*pt.Z, pt.X*pt.Z, pt.X*pt.Y),
                x.Count - 1
            )
 
    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    member x.RemoveInPlace(pt : V3d) =
        if x.Count <= 1 then
            x.Reference <- V3d.Zero
            x.Sum <- V3d.Zero
            x.SumSq <- V3d.Zero
            x.Off <- V3d.Zero
            x.Count <- 0
        else
            let pt = pt - x.Reference
            x.Sum <- x.Sum - pt
            x.SumSq <- x.SumSq - sqr pt
            x.Off <- x.Off - V3d(pt.Y*pt.Z, pt.X*pt.Z, pt.X*pt.Y)
            x.Count <- x.Count - 1


    /// Gets the variances as [Var(X), Var(Y), Var(Z)]
    member x.Variance =
        if x.Count < 2 then
            V3d.Zero
        else
            let n = float x.Count
            let avg = x.Sum / n
            let xx = (x.SumSq.X - avg.X * x.Sum.X) / (n - 1.0)
            let yy = (x.SumSq.Y - avg.Y * x.Sum.Y) / (n - 1.0)
            let zz = (x.SumSq.Z - avg.Z * x.Sum.Z) / (n - 1.0)
            V3d(xx, yy, zz)

    /// Gets the covariances as [Cov(Y,Z), Cov(X,Z), Cov(X,Y)]
    member x.Covariance =
        if x.Count < 2 then
            V3d.Zero
        else
            let n = float x.Count
            let avg = x.Sum / n
            let xy = (x.Off.Z - avg.X * x.Sum.Y) / (n - 1.0)
            let xz = (x.Off.Y - avg.X * x.Sum.Z) / (n - 1.0)
            let yz = (x.Off.X - avg.Y * x.Sum.Z) / (n - 1.0)
            V3d(yz, xz, xy)

    /// Gets the covariance matrix as
    /// | Cov(X,X) | Cov(X,Y) | Cov(X,Z) |
    /// | Cov(X,Y) | Cov(Y,Y) | Cov(Y,Z) |
    /// | Cov(X,Z) | Cov(Y,Z) | Cov(Z,Z) |
    member x.CovarianceMatrix =
        if x.Count < 2  then
            M33d.Zero
        else
            let n = float x.Count
            let avg = x.Sum / n
            let xx = (x.SumSq.X - avg.X * x.Sum.X) / (n - 1.0)
            let yy = (x.SumSq.Y - avg.Y * x.Sum.Y) / (n - 1.0)
            let zz = (x.SumSq.Z - avg.Z * x.Sum.Z) / (n - 1.0)

            let xy = (x.Off.Z - avg.X * x.Sum.Y) / (n - 1.0)
            let xz = (x.Off.Y - avg.X * x.Sum.Z) / (n - 1.0)
            let yz = (x.Off.X - avg.Y * x.Sum.Z) / (n - 1.0)
        
            M33d(
                xx, xy, xz,
                xy, yy, yz,
                xz, yz, zz
            )

    /// Single source of truth for the PCA-based computations. Computes the
    /// covariance matrix and its eigenvalues once; all public getters derive
    /// their answers from this.
    member private x.ComputeEigen() : EigenData3d =
        if x.Count < 2 then emptyEigen3d
        else
            let n = float x.Count
            let avg = x.Sum / n
            let xx = (x.SumSq.X - avg.X * x.Sum.X) / (n - 1.0)
            let yy = (x.SumSq.Y - avg.Y * x.Sum.Y) / (n - 1.0)
            let zz = (x.SumSq.Z - avg.Z * x.Sum.Z) / (n - 1.0)

            let xy = (x.Off.Z - avg.X * x.Sum.Y) / (n - 1.0)
            let xz = (x.Off.Y - avg.X * x.Sum.Z) / (n - 1.0)
            let yz = (x.Off.X - avg.Y * x.Sum.Z) / (n - 1.0)

            let b = -xx - yy - zz
            let c = -sqr xy - sqr xz - sqr yz + xx*yy + xx*zz + yy*zz
            let d = -xx*yy*zz - 2.0*xy*xz*yz + sqr xz*yy + sqr xy*zz + sqr yz*xx

            let struct (l0, l1, l2) = sortTripleAbs (Polynomial.RealRootsOfNormed(b,c,d))
            { Valid = true
              Xx = xx; Yy = yy; Zz = zz
              Xy = xy; Xz = xz; Yz = yz
              L0 = l0; L1 = l1; L2 = l2
              Avg = avg }

    /// Gets the least-squares plane and a confidence value in [0,1] where 1 stands for a perfect plane.
    /// Confidence = 1 - |λ₀|/|λ₁|: how much smaller the tightest variance is than the second tightest.
    member x.GetPlaneAndConfidence() =
        let d = x.ComputeEigen()
        if not d.Valid || x.Count < 3 || Fun.IsTiny d.L1 then
            struct(Plane3d.Invalid, 0.0)
        else
            let goodness = 1.0 - abs d.L0 / abs d.L1
            if goodness > 1E-5 then
                struct(Plane3d(eigenvector3d d d.L0, x.Reference + d.Avg), goodness)
            else
                struct(Plane3d.Invalid, goodness)

    /// Gets the least-squares line (as a Ray3d through the centroid) and a
    /// confidence value in [0,1] where 1 stands for a perfectly linear point distribution.
    /// Confidence = 1 - |λ₁|/|λ₂|: equivalent to the Westin linearity measure.
    member x.GetLineAndConfidence() =
        let d = x.ComputeEigen()
        if not d.Valid || Fun.IsTiny d.L2 then
            struct(Ray3d.Invalid, 0.0)
        else
            let goodness = 1.0 - abs d.L1 / abs d.L2
            if goodness > 1E-5 then
                struct(Ray3d(x.Reference + d.Avg, eigenvector3d d d.L2), goodness)
            else
                struct(Ray3d.Invalid, goodness)

    /// Westin-style shape measures computed from the covariance eigenvalues
    /// λ₀ ≤ λ₁ ≤ λ₂ (principal variances). Returns (linearity, planarity, sphericity),
    /// each in [0, 1], summing to 1:
    ///   linearity  = (λ₂ - λ₁) / λ₂  — close to 1 when points lie along a line
    ///   planarity  = (λ₁ - λ₀) / λ₂  — close to 1 when points lie in a plane
    ///   sphericity =  λ₀      / λ₂  — close to 1 when points are isotropically distributed
    /// Returns (0,0,0) when fewer than 3 points or the data is degenerate.
    member x.GetShape() =
        let r = x.GetResult()
        struct(r.Linearity, r.Planarity, r.Sphericity)

    /// Combined result of the regression: least-squares plane, least-squares line
    /// (as a Ray3d), and the three Westin shape measures. All derived from a single
    /// eigendecomposition of the covariance matrix. Prefer this over calling
    /// GetPlane/GetLine/GetShape separately when you need more than one.
    member x.GetResult() =
        let d = x.ComputeEigen()
        if not d.Valid || x.Count < 3 then
            { Plane = Plane3d.Invalid; Line = Ray3d.Invalid
              Linearity = 0.0; Planarity = 0.0; Sphericity = 0.0 }
        else
            let a0 = abs d.L0
            let a1 = abs d.L1
            let a2 = abs d.L2
            let origin = x.Reference + d.Avg

            let plane =
                if Fun.IsTiny a1 then Plane3d.Invalid
                else Plane3d(eigenvector3d d d.L0, origin)

            let line =
                if Fun.IsTiny a2 then Ray3d.Invalid
                else Ray3d(origin, eigenvector3d d d.L2)

            let cl, cp, cs =
                if Fun.IsTiny a2 then 0.0, 0.0, 0.0
                else (a2 - a1) / a2, (a1 - a0) / a2, a0 / a2

            { Plane = plane; Line = line
              Linearity = cl; Planarity = cp; Sphericity = cs }

    /// Gets a Trafo3d for the entire plane (using its principal components) and the respective standard deviations for each axis.
    member x.GetTrafoAndSizes() =
        let d = x.ComputeEigen()
        if not d.Valid || x.Count < 3 || Fun.IsTiny d.L1 then
            struct(Trafo3d.Identity, x.Reference + d.Avg)
        else
            // We want an orthonormal frame aligned with the principal axes. Rather than
            // cross-producting the two longest (M - L0·I) columns to get the eigenvector
            // for L0 (which GetPlane does), here we keep the original "pick any two
            // non-parallel columns, use them as vx & vy, and the third as vz" approach
            // because that ordering drives the orthonormalization below.
            let c0 = V3d(d.Xx - d.L0, d.Xy, d.Xz)
            let c1 = V3d(d.Xy, d.Yy - d.L0, d.Yz)
            let c2 = V3d(d.Xz, d.Yz, d.Zz - d.L0)
            let struct(vx, vy, vz) =
                let len0 = Vec.lengthSquared c0
                let len1 = Vec.lengthSquared c1
                let len2 = Vec.lengthSquared c2

                if len0 > len1 then
                    if len1 > len2 then struct(c0, c1, Vec.cross c0 c1)
                    elif len0 > len2 then struct(c0, c2, Vec.cross c0 c2)
                    else struct(c2, c0, Vec.cross c2 c0)
                else (* len1 >= len0*)
                    if len0 > len2 then struct(c1, c0, Vec.cross c1 c0)
                    elif len1 > len2 then struct(c1, c2, Vec.cross c1 c2)
                    else struct(c2, c1, Vec.cross c2 c1)

            // orthonormalize the system (minimal numerical errors)
            let vz = Vec.normalize vz
            let vx = Vec.normalize (vx - vz * Vec.dot vx vz)
            let vy = Vec.normalize (vy - vz * Vec.dot vy vz - vx * Vec.dot vy vx)

            // get the stddevs for the main axes
            let s0 = if d.L0 < 0.0 then 0.0 else sqrt d.L0
            let s1 = if d.L1 < 0.0 then 0.0 else sqrt d.L1
            let s2 = if d.L2 < 0.0 then 0.0 else sqrt d.L2
            let dev = V3d(s2, s1, s0)

            let trafo =
                let o = x.Reference + d.Avg
                Trafo3d(
                    M44d(
                        vx.X, vy.X, vz.X, o.X,
                        vx.Y, vy.Y, vz.Y, o.Y,
                        vx.Z, vy.Z, vz.Z, o.Z,
                        0.0,  0.0,  0.0,  1.0
                    ),
                    M44d(
                        vx.X, vx.Y, vx.Z, -Vec.dot vx o,
                        vy.X, vy.Y, vy.Z, -Vec.dot vy o,
                        vz.X, vz.Y, vz.Z, -Vec.dot vz o,
                        0.0,  0.0,  0.0,   1.0
                    )
                )
            struct(trafo, dev)
    
    /// Gets the least-squares plane.
    member x.GetPlane() =
        let struct(p, _) = x.GetPlaneAndConfidence()
        p

    /// Gets the least-squares line through the centroid as a Ray3d. Aardvark has no
    /// dedicated unbounded-line type, so a Ray3d (origin + direction) is used — treat
    /// its parameter t as an arbitrary real, not as t ≥ 0.
    member x.GetLine() =
        let struct(r, _) = x.GetLineAndConfidence()
        r

    /// Gets a Trafo3d for the entire plane using its principal components.
    member x.GetTrafo() =
        let struct(t, _) = x.GetTrafoAndSizes()
        t

    static member (+) (l : LinearRegression3d, r : LinearRegression3d) =
        if r.Count <= 0 then l
        elif l.Count <= 0 then r
        elif l.Count = 1 then r.Add l.Reference
        elif r.Count = 1 then l.Add r.Reference
        else
            let r1 = r.WithReferencePoint l.Reference
            LinearRegression3d(
                l.Reference,
                l.Sum + r1.Sum,
                l.SumSq + r1.SumSq,
                l.Off + r1.Off,
                l.Count + r1.Count
            )
        
    static member (-) (l : LinearRegression3d, r : LinearRegression3d) =
        if r.Count <= 0 then l
        elif l.Count <= r.Count then LinearRegression3d.Zero
        elif r.Count = 1 then l.Remove r.Reference
        else
            let r = r.WithReferencePoint l.Reference
            LinearRegression3d(
                l.Reference,
                l.Sum - r.Sum,
                l.SumSq - r.SumSq,
                l.Off - r.Off,
                l.Count - r.Count
            )
        
    static member (+) (l : LinearRegression3d, r : V3d) =
        l.Add r
        
    static member (+) (l : V3d, r : LinearRegression3d) =
        r.Add l
        
    static member (-) (l : LinearRegression3d, r : V3d) =
        l.Remove r

module LinearRegression3d =

    /// A regression holding no points.
    let empty = LinearRegression3d.Zero

    /// Removes the given point from the regression assuming that it has previously been added.
    /// NOTE: when removing non-added points the regression will produce inconsistent results.
    let inline remove (pt : V3d) (s : LinearRegression3d) = s.Remove pt

    /// Adds the given point to the regression.
    let inline add (pt : V3d) (s : LinearRegression3d) = s.Add pt

    /// Gets the least-squares plane.
    let inline getPlane (s : LinearRegression3d) = s.GetPlane()

    /// Gets the least-squares line through the centroid as a Ray3d.
    let inline getLine (s : LinearRegression3d) = s.GetLine()

    /// Westin-style shape measures: (linearity, planarity, sphericity) in [0,1], summing to 1.
    let inline getShape (s : LinearRegression3d) = s.GetShape()

    /// Combined result of the regression: plane, line, and shape measures in one record.
    let inline getResult (s : LinearRegression3d) = s.GetResult()

    /// Gets a Trafo3d for the entire plane using its principal components.
    let inline getTrafo (s : LinearRegression3d) = s.GetTrafo()
    
    /// The total number of points added. Note that at least 3 points are required to fit a plane.
    let inline count (s : LinearRegression3d) = s.Count
    
    /// The centroid of all added points.
    let inline centroid (s : LinearRegression3d) = s.Centroid
    
    /// Gets the variances as [Var(X), Var(Y), Var(Z)]
    let inline variance (s : LinearRegression3d) = s.Variance

    /// Gets the covariances as [Cov(Y,Z), Cov(X,Z), Cov(X,Y)]
    let inline covariance (s : LinearRegression3d) = s.Covariance

    /// Gets the covariance matrix as
    /// | Cov(X,X) | Cov(X,Y) | Cov(X,Z) |
    /// | Cov(X,Y) | Cov(Y,Y) | Cov(Y,Z) |
    /// | Cov(X,Z) | Cov(Y,Z) | Cov(Z,Z) |
    let inline covarianceMatrix (pt : V3d) (s : LinearRegression3d) = s.CovarianceMatrix

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

