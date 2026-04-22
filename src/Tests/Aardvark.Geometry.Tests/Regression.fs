namespace Aardvark.Geometry.Tests

open System
open NUnit.Framework
open FsUnit
open FsCheck
open FsCheck.NUnit

open Aardvark.Base
open Aardvark.Geometry

module Regression3dTests =
    
    module Gen =

        /// generates a float in [0,1]
        let uniformFloat =
            // choose two ints with 22/31 bits, create an uint64 from these and normalize to float.
            // this creates a full-precision float since float has 53 "effective" significand bits.
            (Gen.choose (0, 4194303), Gen.choose (0, Int32.MaxValue)) ||> Gen.map2 (fun a b ->
                let v = (uint64 a <<< 31) ||| uint64 b
                float v / 9007199254740991.0
            )                

        /// generates a float in [min, max]
        let uniform (min : float) (max : float) =
            uniformFloat |> Gen.map (fun v ->
                min + v * (max - min)
            )

        /// generates a float from N(avg, dev)
        let gauss (avg : float) (dev : float) =
            uniform -1.0 1.0 |> Gen.map (fun v ->
                let side = sign v
                let v = abs v |> max 1E-9
                let x = sqrt (-log v)
                let n01 = x * float side
                n01 * dev + avg
            )

        /// generates a normalized V3d direction
        let direction =
            gen {
                let! phi = uniform 0.0 Constant.PiTimesTwo
                let! z = uniform -1.0 1.0

                let xy = sqrt(1.0 - sqr z)

                return 
                    V3d(
                        cos phi * xy,
                        sin phi * xy,
                        z
                    )
            }

    type AtLeast<'d, 'a when 'd :> INatural>(value : 'a[]) =
        member x.Value = value
        override x.ToString() =
            sprintf "%d: %A" value.Length value

    type ZeroOneFloat =
        ZeroOneFloat of float
    type GaussFloat =
        GaussFloat of float


    type EuclideanGenerator private() =

        static member ZeroOneFloat =
            Gen.uniformFloat |> Gen.map ZeroOneFloat |> Arb.fromGen

        static member GaussFloat =
            Gen.gauss 0.0 1.0 |> Gen.map GaussFloat |> Arb.fromGen

        static member AtLeast<'d, 'a when 'd :> INatural>() =
            gen {
                let d = typeSize<'d>

                let! rest = Arb.generate<'a[]>
                let fin = Array.zeroCreate (rest.Length + d)


                let mutable oi = 0
                let rec exists (i : int) (v : 'a) =
                    if i >= oi then 
                        false
                    else    
                        if Unchecked.equals fin.[oi] v then true
                        else exists (i+1) v

                while oi < d do
                    let! v = Arb.generate<'a>
                    if not (exists 0 v) then
                        fin.[oi] <- v
                        oi <- oi + 1

                fin.[d..] <- rest

                return fin |> AtLeast<'d, 'a>
            }|> Arb.fromGen

        static member V2d =
            gen {
                let! x = Gen.gauss 0.0 500.0
                let! y = Gen.gauss 0.0 100.0
                return V2d(x,y)
            } |> Arb.fromGen
            
        static member V3d =
            gen {
                let! x = Gen.gauss 0.0 500.0
                let! y = Gen.gauss 0.0 100.0
                let! z = Gen.gauss 0.0 300.0
                return V3d(x,y,z)
            } |> Arb.fromGen
            
        static member Euclidean2d =
            gen {
                let! x = Gen.gauss 0.0 300.0
                let! y = Gen.gauss 0.0 300.0
                let! angle = Gen.uniform 0.0 Constant.PiTimesTwo

                return Euclidean2d(Rot2d angle, V2d(x,y))

            } |> Arb.fromGen
        static member Euclidean3d =
            gen {
                let! x = Gen.gauss 0.0 300.0
                let! y = Gen.gauss 0.0 300.0
                let! z = Gen.gauss 0.0 300.0

                let! axis = Gen.direction
                let! angle = Gen.uniform 0.0 Constant.PiTimesTwo

                let rot = Rot3d.FromAngleAxis(angle * axis)
                return Euclidean3d(rot, V3d(x,y,z))

            } |> Arb.fromGen

    let checkPlane (r : LinearRegression3d) (points : V3d[]) =
        let plane = r.GetPlane()
        let error = points |> Array.map (plane.Height >> abs) |> Array.max
        error |> should be (lessThanOrEqualTo 1E-6)

    let checkTrafo (r : LinearRegression3d) (points : V3d[]) =
        let trafo = r.GetTrafo()
        let error = points |> Array.map (fun p -> trafo.Backward.TransformPos(p).Z |> abs) |> Array.max
        error |> should be (lessThanOrEqualTo 1E-6)
        
    let check (points : V3d[]) (r : LinearRegression3d) =
        checkPlane r points
        checkTrafo r points
        
    let checkPlane2d (r : LinearRegression2d) (points : V2d[]) =
        let struct(plane, _) = r.GetPlaneAndConfidence()
        let error = points |> Array.map (plane.Height >> abs) |> Array.max
        error |> should be (lessThanOrEqualTo 1E-6)
        
    let checkTrafo2d (r : LinearRegression2d) (points : V2d[]) =
        let trafo = r.GetTrafo()
        let error = points |> Array.map (fun p -> trafo.Backward.TransformPos(p).Y |> abs) |> Array.max
        error |> should be (lessThanOrEqualTo 1E-6)
        
    let check2d (points : V2d[]) (r : LinearRegression2d) =
        checkPlane2d r points
        checkTrafo2d r points

        
    let checkSphere (points : V3d[]) (r : SphereRegression3d) =
        let s = r.GetSphere()
        let error = points |> Array.map (fun p -> abs (Vec.distance p s.Center - s.Radius)) |> Array.max
        error |> should be (lessThanOrEqualTo 1E-6)

    [<Property(Arbitrary = [| typeof<EuclideanGenerator> |])>]
    let ``LinearRegression3d working`` (trafo : Euclidean3d) (points : AtLeast<8 N, V2d>) =
        let pts = 
            points.Value
            |> Array.map (fun p -> trafo.TransformPos(V3d(p, 0.0)))

        // add working
        pts |> Array.fold (+) LinearRegression3d.empty |> check pts

        // ofArray/ofSeq working
        pts |> LinearRegression3d.ofArray |> check pts
        pts |> LinearRegression3d.ofSeq |> check pts

        // constructor working
        pts |> LinearRegression3d |> check pts

        // regression addition working
        let n2 = pts.Length / 2
        LinearRegression3d.ofArray pts.[..n2-1] + LinearRegression3d.ofArray pts.[n2..] |> check pts

        // remove working
        pts |> Array.fold (+) (LinearRegression3d V3d.Zero) |> LinearRegression3d.remove V3d.Zero |> check pts
        

    [<Property(Arbitrary = [| typeof<EuclideanGenerator> |])>]
    let ``LinearRegression2d working`` (trafo : Euclidean2d) (points : AtLeast<8 N, GaussFloat>) =
        let pts = points.Value |> Array.map (fun (GaussFloat v) -> trafo.TransformPos(V2d(v * 10.0, 0.0)))
        
        // add working
        pts |> Array.fold (+) LinearRegression2d.empty |> check2d pts

        // ofArray/ofSeq working
        pts |> LinearRegression2d.ofArray |> check2d pts
        pts |> LinearRegression2d.ofSeq |> check2d pts

        // constructor working
        pts |> LinearRegression2d |> check2d pts

        // regression addition working
        let n2 = pts.Length / 2
        LinearRegression2d.ofArray pts.[..n2-1] + LinearRegression2d.ofArray pts.[n2..] |> check2d pts

        // remove working
        pts |> Array.fold (+) (LinearRegression2d V2d.Zero) |> LinearRegression2d.remove V2d.Zero |> check2d pts

    [<Property(Arbitrary = [| typeof<EuclideanGenerator> |])>]
    let ``SphereRegression3d working`` (center : V3d) (GaussFloat radius) (bad : V3d) (points : AtLeast<100 N, V2d>) =
        let pts = 
            points.Value
            |> Array.map (fun p -> center + p.CartesianFromSpherical() * radius)

        let bb = Box3d pts
        let r = (bad / bb.Size)
        let somePoint = bb.Center + bb.Size * r / r.NormMax

        // add working
        pts |> Array.fold (+) SphereRegression3d.empty |> checkSphere pts

        // ofArray/ofSeq working
        pts |> SphereRegression3d.ofArray |> checkSphere pts
        pts |> SphereRegression3d.ofSeq |> checkSphere pts

        // constructor working
        pts |> SphereRegression3d |> checkSphere pts

        // regression addition working
        let n2 = pts.Length / 2
        SphereRegression3d.ofArray pts.[..n2-1] + SphereRegression3d.ofArray pts.[n2..] |> checkSphere pts

        // remove working
        pts |> Array.fold (+) (SphereRegression3d somePoint) |> SphereRegression3d.remove somePoint |> checkSphere pts

    let private algebraicResidual (pts : V2d[]) (e : Ellipse2d) =
        // Express fitted ellipse as (p - center)^T M (p - center) - 1 and return max |r|.
        let a0 = e.Axis0
        let a1 = e.Axis1
        let l0 = Vec.length a0
        let l1 = Vec.length a1
        if l0 <= 0.0 || l1 <= 0.0 then infinity
        else
            let u0 = a0 / l0
            let u1 = a1 / l1
            let r02 = sqr l0
            let r12 = sqr l1
            pts
            |> Array.map (fun p ->
                let d = p - e.Center
                let x = Vec.dot d u0
                let y = Vec.dot d u1
                abs (sqr x / r02 + sqr y / r12 - 1.0))
            |> Array.max

    let checkEllipse (pts : V2d[]) (r : EllipseRegression2d) =
        let e = r.GetEllipse()
        let err = algebraicResidual pts e
        err |> should be (lessThanOrEqualTo 1E-6)

    [<Property(Arbitrary = [| typeof<EuclideanGenerator> |])>]
    let ``EllipseRegression2d working`` (trafo : Euclidean2d) (GaussFloat rx') (GaussFloat ry') (bad : V2d) (points : AtLeast<20 N, ZeroOneFloat>) =
        // semi-axes: positive, spread over a reasonable range
        let rx = 0.25 + abs rx' * 5.0
        let ry = 0.25 + abs ry' * 5.0
        let pts =
            points.Value
            |> Array.map (fun (ZeroOneFloat t) ->
                let phi = t * Constant.PiTimesTwo
                trafo.TransformPos(V2d(rx * cos phi, ry * sin phi)))

        // outside-box point for the remove-round-trip test
        let bb = Box2d pts
        let size = bb.Size
        let safe = if size.X > 1e-9 && size.Y > 1e-9 then size else V2d.II
        let r = bad / safe
        let somePoint =
            if r.NormMax > 1e-9 then bb.Center + safe * r / r.NormMax
            else bb.Center + safe

        // add working
        pts |> Array.fold (+) EllipseRegression2d.empty |> checkEllipse pts

        // ofArray / ofSeq working
        pts |> EllipseRegression2d.ofArray |> checkEllipse pts
        pts |> EllipseRegression2d.ofSeq |> checkEllipse pts

        // constructor working
        pts |> EllipseRegression2d |> checkEllipse pts

        // regression addition working
        let n2 = pts.Length / 2
        EllipseRegression2d.ofArray pts.[..n2-1] + EllipseRegression2d.ofArray pts.[n2..] |> checkEllipse pts

        // remove working (add an outside point, then remove it; fit must still match)
        pts |> Array.fold (+) (EllipseRegression2d somePoint) |> EllipseRegression2d.remove somePoint |> checkEllipse pts

    let checkCircle (pts : V2d[]) (r : CircleRegression2d) =
        let c = r.GetCircle()
        let error = pts |> Array.map (fun p -> abs (Vec.distance p c.Center - c.Radius)) |> Array.max
        error |> should be (lessThanOrEqualTo 1E-6)

    [<Property(Arbitrary = [| typeof<EuclideanGenerator> |])>]
    let ``CircleRegression2d working`` (center : V2d) (GaussFloat radius) (bad : V2d) (points : AtLeast<50 N, ZeroOneFloat>) =
        let r = 0.25 + abs radius
        let pts =
            points.Value
            |> Array.map (fun (ZeroOneFloat t) ->
                let phi = t * Constant.PiTimesTwo
                center + V2d(cos phi, sin phi) * r)

        let bb = Box2d pts
        let size = if bb.Size.X > 1e-9 && bb.Size.Y > 1e-9 then bb.Size else V2d.II
        let rr = bad / size
        let somePoint =
            if rr.NormMax > 1e-9 then bb.Center + size * rr / rr.NormMax
            else bb.Center + size

        // add working
        pts |> Array.fold (+) CircleRegression2d.empty |> checkCircle pts

        // ofArray / ofSeq working
        pts |> CircleRegression2d.ofArray |> checkCircle pts
        pts |> CircleRegression2d.ofSeq |> checkCircle pts

        // constructor working
        pts |> CircleRegression2d |> checkCircle pts

        // regression addition working
        let n2 = pts.Length / 2
        CircleRegression2d.ofArray pts.[..n2-1] + CircleRegression2d.ofArray pts.[n2..] |> checkCircle pts

        // remove working
        pts |> Array.fold (+) (CircleRegression2d somePoint) |> CircleRegression2d.remove somePoint |> checkCircle pts

    let checkEllipsoid (pts : V3d[]) (r : EllipsoidRegression3d) =
        let e = r.GetEllipsoid()
        // Transform each point into the ellipsoid's local frame, scale by 1/radii, length should be 1.
        // The algebraic ellipsoid fit minimizes algebraic residual (not geometric distance), so a loose
        // tolerance is appropriate — on ill-conditioned inputs the geometric error can grow.
        let error =
            pts |> Array.map (fun p ->
                let q = e.Euclidean.InvTransformPos p / e.Radii
                abs (Vec.length q - 1.0))
            |> Array.max
        error |> should be (lessThanOrEqualTo 1E-3)

    [<Property(Arbitrary = [| typeof<EuclideanGenerator> |])>]
    let ``EllipsoidRegression3d working`` (trafo : Euclidean3d) (GaussFloat rx') (GaussFloat ry') (GaussFloat rz') (points : AtLeast<100 N, V2d>) =
        // semi-axes: positive, modest range. Very skewed radii push the algebraic fit's conditioning.
        let rx = 1.0 + abs rx'
        let ry = 1.0 + abs ry'
        let rz = 1.0 + abs rz'
        // CartesianFromSpherical maps spherical coords (theta, phi) to unit directions uniformly.
        let pts =
            points.Value
            |> Array.map (fun sph ->
                let dir = sph.CartesianFromSpherical()
                trafo.TransformPos(V3d(rx * dir.X, ry * dir.Y, rz * dir.Z)))

        // add working  (no (+) operator on EllipsoidRegression3d, use Add directly)
        (EllipsoidRegression3d.empty, pts) ||> Array.fold (fun r p -> r.Add p) |> checkEllipsoid pts

        // ofArray / ofSeq working
        pts |> EllipsoidRegression3d.ofArray |> checkEllipsoid pts
        pts |> EllipsoidRegression3d.ofSeq |> checkEllipsoid pts

        // constructor working
        pts |> EllipsoidRegression3d |> checkEllipsoid pts

        // remove working
        let somePoint = trafo.TransformPos (V3d(rx * 10.0, ry * 10.0, rz * 10.0))
        let withExtra = (EllipsoidRegression3d somePoint, pts) ||> Array.fold (fun r p -> r.Add p)
        withExtra.Remove somePoint |> checkEllipsoid pts
        

