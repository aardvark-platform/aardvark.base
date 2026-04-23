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

    // -----------------------------------------------------------------------------
    //  EllipseRegression2d stress scan
    //
    //  Sweeps point counts, noise levels, eccentricity, arc coverage and scale;
    //  prints the medians/p95 of centre/axis/angle error per configuration as
    //  an informational report. A handful of very basic cases are asserted.
    // -----------------------------------------------------------------------------

    let private sampleEllipse (c : V2d) (a : float) (b : float) (theta : float) (n : int) (noise : float) (rand : Random) =
        let cs, sn = cos theta, sin theta
        let phi = rand.NextDouble() * 2.0 * Math.PI
        Array.init n (fun i ->
            let t  = phi + 2.0 * Math.PI * float i / float n
            let x  = a * cos t
            let y  = b * sin t
            let px = x * cs - y * sn
            let py = x * sn + y * cs
            let dx = (rand.NextDouble() - 0.5) * 2.0 * noise
            let dy = (rand.NextDouble() - 0.5) * 2.0 * noise
            V2d(c.X + px + dx, c.Y + py + dy))

    let private canonical (e : Ellipse2d) =
        let l0 = Vec.length e.Axis0
        let l1 = Vec.length e.Axis1
        let major, minor, dir =
            if l0 >= l1 then l0, l1, (if l0 > 0.0 then e.Axis0 / l0 else V2d.IO)
            else             l1, l0, (if l1 > 0.0 then e.Axis1 / l1 else V2d.IO)
        let ang = atan2 dir.Y dir.X
        let ang = if ang < 0.0 then ang + Math.PI else ang
        e.Center, major, minor, ang % Math.PI

    let private truth (c : V2d, a : float, b : float, th : float) =
        let major, minor, ang =
            if a >= b then a, b, th else b, a, th + Math.PI / 2.0
        let ang = if ang < 0.0 then ang + Math.PI else ang
        c, major, minor, ang % Math.PI

    let private angleDiff a b =
        let d = abs (a - b) % Math.PI
        min d (Math.PI - d)

    let private pct (q : float) (xs : float seq) =
        let arr = xs |> Seq.toArray
        if arr.Length = 0 then nan
        else
            Array.sortInPlace arr
            arr.[int (float (arr.Length - 1) * q)]

    // NUnit swallows stdout; route informational prints through TestContext.Progress
    // so they show up under `dotnet test --logger console;verbosity=detailed`.
    let private tprintfn fmt =
        Printf.kprintf (fun s -> NUnit.Framework.TestContext.Progress.WriteLine s) fmt

    [<Test>]
    let ``EllipseRegression2d stress scan`` () =
        let rand = Random(1)

        let randomEllipse () =
            let cx = rand.NextDouble() * 40.0 - 20.0
            let cy = rand.NextDouble() * 40.0 - 20.0
            let a  = 0.5 + rand.NextDouble() * 4.5
            let b  = 0.5 + rand.NextDouble() * 4.5
            let th = rand.NextDouble() * Math.PI
            V2d(cx, cy), a, b, th

        let sweep label (run : int -> (float * float * float * float)) trials =
            let dCs = ResizeArray()
            let dAs = ResizeArray()
            let dBs = ResizeArray()
            let dTs = ResizeArray()
            for i in 1 .. trials do
                let dC, dA, dB, dT = run i
                dCs.Add dC; dAs.Add dA; dBs.Add dB; dTs.Add dT
            tprintfn "  %-36s | Δc med=%.2e p95=%.2e | Δa p95=%.2e | Δb p95=%.2e | Δθ p95=%.2e"
                label (pct 0.5 dCs) (pct 0.95 dCs) (pct 0.95 dAs) (pct 0.95 dBs) (pct 0.95 dTs)
            pct 0.95 dCs, pct 0.95 dAs, pct 0.95 dTs

        let runOne nPoints noise _ =
            let c, a, b, th = randomEllipse ()
            let pts = sampleEllipse c a b th nPoints noise rand
            let e = EllipseRegression2d(pts).GetEllipse()
            let fc, fa, fb, fth = canonical e
            let tc, ta, tb, tth = truth (c, a, b, th)
            Vec.distance tc fc, abs (ta - fa), abs (tb - fb), angleDiff tth fth

        tprintfn ""
        tprintfn "── point-count sweep (noise=0.05) ──"
        for n in [ 5; 10; 50; 500 ] do
            sweep (sprintf "n=%d" n) (runOne n 0.05) 200 |> ignore

        tprintfn "── noise sweep (n=100) ──"
        for noise in [ 0.0; 0.001; 0.01; 0.1; 1.0 ] do
            sweep (sprintf "noise=%g" noise) (runOne 100 noise) 200 |> ignore

        tprintfn "── arc-coverage sweep (n=100, noise=0.01) ──"
        let sampleArc (c : V2d) (a : float) (b : float) (theta : float) (n : int) (arcFrac : float) (noise : float) =
            let cs, sn = cos theta, sin theta
            let phi = rand.NextDouble() * 2.0 * Math.PI
            let arc = arcFrac * 2.0 * Math.PI
            Array.init n (fun i ->
                let t = phi + arc * float i / float (n - 1)
                let x = a * cos t
                let y = b * sin t
                let px = x * cs - y * sn
                let py = x * sn + y * cs
                let dx = (rand.NextDouble() - 0.5) * 2.0 * noise
                let dy = (rand.NextDouble() - 0.5) * 2.0 * noise
                V2d(c.X + px + dx, c.Y + py + dy))

        for frac in [ 1.0; 0.5; 0.3; 0.1 ] do
            sweep (sprintf "arc=%.0f%%" (frac * 100.0)) (fun _ ->
                let c, a, b, th = randomEllipse ()
                let pts = sampleArc c a b th 100 frac 0.01
                let e = EllipseRegression2d(pts).GetEllipse()
                let fc, fa, fb, fth = canonical e
                let tc, ta, tb, tth = truth (c, a, b, th)
                Vec.distance tc fc, abs (ta - fa), abs (tb - fb), angleDiff tth fth) 200
            |> ignore

        // --- Basic-case validation: noise-free axis-aligned and rotated ellipses ---
        let n = 80

        // (1) axis-aligned unit circle at origin
        let pts1 = Array.init n (fun i ->
            let t = 2.0 * Math.PI * float i / float n
            V2d(cos t, sin t))
        let e1 = EllipseRegression2d(pts1).GetEllipse()
        Vec.distance e1.Center V2d.Zero |> should be (lessThanOrEqualTo 1E-10)
        abs (Vec.length e1.Axis0 - 1.0) |> should be (lessThanOrEqualTo 1E-10)
        abs (Vec.length e1.Axis1 - 1.0) |> should be (lessThanOrEqualTo 1E-10)

        // (2) axis-aligned ellipse a=3, b=1 at (5, -2)
        let c = V2d(5.0, -2.0)
        let pts2 = Array.init n (fun i ->
            let t = 2.0 * Math.PI * float i / float n
            V2d(c.X + 3.0 * cos t, c.Y + 1.0 * sin t))
        let e2 = EllipseRegression2d(pts2).GetEllipse()
        Vec.distance e2.Center c |> should be (lessThanOrEqualTo 1E-10)
        let l0 = Vec.length e2.Axis0
        let l1 = Vec.length e2.Axis1
        let major = max l0 l1
        let minor = min l0 l1
        abs (major - 3.0) |> should be (lessThanOrEqualTo 1E-10)
        abs (minor - 1.0) |> should be (lessThanOrEqualTo 1E-10)

        // (3) rotated ellipse a=3, b=1, θ=30° at origin
        let th = Math.PI / 6.0
        let cs, sn = cos th, sin th
        let pts3 = Array.init n (fun i ->
            let t = 2.0 * Math.PI * float i / float n
            let x = 3.0 * cos t
            let y = 1.0 * sin t
            V2d(x * cs - y * sn, x * sn + y * cs))
        let e3 = EllipseRegression2d(pts3).GetEllipse()
        Vec.distance e3.Center V2d.Zero |> should be (lessThanOrEqualTo 1E-10)
        let majorV = if Vec.length e3.Axis0 >= Vec.length e3.Axis1 then e3.Axis0 else e3.Axis1
        abs (Vec.length majorV - 3.0) |> should be (lessThanOrEqualTo 1E-10)
        // major axis direction should align with (cos th, sin th) up to sign
        abs (abs (Vec.dot (Vec.normalize majorV) (V2d(cs, sn))) - 1.0) |> should be (lessThanOrEqualTo 1E-10)
        

