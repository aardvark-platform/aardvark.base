namespace Aardvark.Rendering.Text


open Aardvark.Base

[<AutoOpen>]
module Helpers = 

    [<Literal>]
    let internal epsilon = 1E-9

    let internal arcCoords (p0 : V2d) (p1 : V2d) (p2 : V2d) =
        let p01 = Plane2d(Vec.normalize (p1 - p0), p0)
        let p12 = Plane2d(Vec.normalize (p2 - p1), p2)
        let mutable centerInWorld = V2d.Zero
        p01.Intersects(p12, &centerInWorld) |> ignore

        let u = p0 - p1
        let v = p2 - p1
        let uv2World = M33d.FromCols(V3d(u, 0.0), V3d(v, 0.0), V3d(p1, 1.0))
        let world2UV = uv2World.Inverse
                
        let cuv = world2UV.TransformPos centerInWorld
        let ruv = Vec.length (V2d.IO - cuv)
            
        let r0 = Plane2d(Vec.normalize (V2d.IO - cuv), V2d.IO)
        let r2 = Plane2d(Vec.normalize (V2d.OI - cuv), V2d.OI)
        let mutable uv1 = V2d.Zero
        r0.Intersects(r2, &uv1) |> ignore

        (V2d.IO - cuv) / ruv, (uv1 - cuv) / ruv, (V2d.OI - cuv) / ruv
   
    let flip (a,b) = b,a

    let findBezierT (epsilon : float) (pt : V2d) (p0 : V2d) (p1 : V2d) (p2 : V2d) : option<float> =
        let a = p0 - 2.0*p1 + p2
        let b = 2.0*p1 - 2.0*p0
        let c = p0 - pt

        let struct(t0, t1) = Polynomial.RealRootsOf(a.X, b.X, c.X)
        let struct(t2, t3) = Polynomial.RealRootsOf(a.Y, b.Y, c.Y)

        let inline check t = if t >= -epsilon && t <= 1.0 + epsilon then Some t else None

        if Fun.ApproximateEquals(t0, t2, epsilon) then check ((t0 + t2) / 2.0)
        elif Fun.ApproximateEquals(t0, t3, epsilon) then check ((t0 + t3) / 2.0)
        elif Fun.ApproximateEquals(t1, t2, epsilon) then check ((t1 + t2) / 2.0)
        elif Fun.ApproximateEquals(t1, t3, epsilon) then check ((t1 + t3) / 2.0)
        else None

    // TODO: check against real implementations
    let inline arcT (a0 : float) (da : float) (v : float) =
        let mutable a0 = a0
        let mutable v = v
        
        while a0 < 0.0 do a0 <- a0 + Constant.PiTimesTwo
        while v < 0.0 do v <- v + Constant.PiTimesTwo
        
        if da < 0.0 then
            if v > a0 + 1E-8 then v <- v - Constant.PiTimesTwo
            (v - a0) / da
        else
            if v < a0 - 1E-8  then v <- v + Constant.PiTimesTwo
            (v - a0) / da

    type Ellipse2d with

        member x.IsCcw =
            let n = V2d(-x.Axis0.Y, x.Axis0.X)
            Vec.dot n x.Axis1 > 0.0

        member x.TryGetAlpha(pt : V2d, eps : float) =
            let a0 = x.Axis0
            let a1 = x.Axis1
            assert (Fun.IsTiny(Vec.dot a0 a1, 1E-6))

            let c =
                V2d(
                    Vec.dot a0 (pt - x.Center) / a0.LengthSquared,
                    Vec.dot a1 (pt - x.Center) / a1.LengthSquared
                )

            let c0 = Vec.normalize c
            let pp = x.Center + x.Axis0 * c0.X + x.Axis1 * c0.Y
            if Fun.ApproximateEquals(pp, pt, eps) then
                Some (atan2 c0.Y c0.X)
            else
                None

        member x.GetAlpha(pt : V2d) =
            let l0 = x.Axis0.Length
            let l1 = x.Axis1.Length
            let a0 = x.Axis0 / l0
            let a1 = x.Axis1 / l1

            if Fun.IsTiny(Vec.dot a0 a1, epsilon) then
                let d = pt - x.Center
                let u = a0.Dot d / l0
                let v = a1.Dot d / l1
                atan2 v u
            else
                failwith "cannot get alpha for bad ellipse"

        member internal x.GetControlPoint(alpha0 : float, alpha1 : float) =
            let p0 = x.GetPoint alpha0
            let p1 = x.GetPoint alpha1
            let t0 = cos alpha0 * x.Axis1 - sin alpha0 * x.Axis0 |> Vec.normalize
            let t1 = cos alpha1 * x.Axis1 - sin alpha1 * x.Axis0 |> Vec.normalize
            let n0 = Ray2d(p0, t0)
            let n1 = Ray2d(p1, t1)
            let pc = n0.Intersect(n1)
            pc

            

type PathSegment =
    private
    | LineSeg of p0 : V2d * p1 : V2d
    | Bezier2Seg of p0 : V2d * p1 : V2d * p2 : V2d
    | Bezier3Seg of p0 : V2d * p1 : V2d * p2 : V2d * p3 : V2d
    | ArcSeg of p0 : V2d * p1 : V2d * alpha0 : float * dAlpha : float * e : Ellipse2d


module internal PathSegmentIntersections =

    let maxArcAngle = 95.0 * Constant.RadiansPerDegree

    /// gets an axis aligned bounding box for the given segment
    let bounds (seg : PathSegment) =
        match seg with
        | LineSeg(p0, p1) -> 
            let mutable b = Box2d(p0, p0)
            b.ExtendBy p1
            b
        | Bezier2Seg(p0, p1, p2) ->
            let mutable b = Box2d(p0, p0)
            b.ExtendBy p1
            b.ExtendBy p2
            b
        | Bezier3Seg(p0, p1, p2, p3) ->
            let mutable b = Box2d(p0, p0)
            b.ExtendBy p1
            b.ExtendBy p2
            b.ExtendBy p3
            b
        | ArcSeg(p0, p1, a0, da, ellipse) -> 
            let mutable bb = Box2d(p0, p0)
            bb.ExtendBy p1

            let steps = abs da / maxArcAngle |> ceil |> int
            if steps = 1 then
                let a1 = a0 + da
                let d0 = cos a0 * ellipse.Axis1 - sin a0 * ellipse.Axis0 |> Vec.normalize
                let d1 = cos a1 * ellipse.Axis1 - sin a1 * ellipse.Axis0 |> Vec.normalize
                let n0 = Ray2d(p0, d0)
                let n1 = Ray2d(p1, d1)
                let pc = n0.Intersect(n1)
                bb.ExtendBy pc
            elif steps > 0 then
                let step = da / float steps
                let mutable t0 = a0
                let mutable q0 = p0
                let mutable d0 = cos a0 * ellipse.Axis1 - sin a0 * ellipse.Axis0 |> Vec.normalize
                for s in 1 .. steps do
                    let t1 = t0 + step

                    let c1 = cos t1
                    let s1 = sin t1

                    let q1 = ellipse.Center + c1 * ellipse.Axis0 + s1 * ellipse.Axis1
                    let d1 = c1 * ellipse.Axis1 - s1 * ellipse.Axis0 |> Vec.normalize
                    let n0 = Ray2d(q0, d0)
                    let n1 = Ray2d(q1, d1)
                    let pc = n0.Intersect(n1)

                    bb.ExtendBy q1
                    bb.ExtendBy pc

                    t0 <- t1
                    q0 <- q1
                    d0 <- d1

            bb

    /// returns the start-point of the segment (t=0)
    let inline startPoint (seg : PathSegment) =
        match seg with
        | LineSeg(p0,_) -> p0
        | Bezier2Seg(p0,_,_) -> p0
        | Bezier3Seg(p0,_,_,_) -> p0
        | ArcSeg(p0,_,_,_,_) -> p0
        
    /// returns the end-point of the segment (t=1)
    let inline endPoint (seg : PathSegment) =
        match seg with
        | LineSeg(_,p1) -> p1
        | Bezier2Seg(_,_,p1) -> p1
        | Bezier3Seg(_,_,_,p1) -> p1
        | ArcSeg(_,p1,_,_,_) -> p1

    /// evaluates the curve-position for the given parameter (t <- [0;1])
    let point (t : float) (seg : PathSegment) =
        if t <= 0.0 then startPoint seg
        elif t >= 1.0 then endPoint seg
        else
            match seg with
            | LineSeg(p0, p1) -> 
                lerp p0 p1 t

            | Bezier2Seg(p0, p1, p2) ->
                let u = 1.0 - t
                (u * u) * p0 + (2.0 * t * u) * p1 + (t * t) * p2

            | Bezier3Seg(p0, p1, p2, p3) ->
                let u = 1.0 - t
                let u2 = u * u
                let t2 = t * t
                (u * u2) * p0 + (3.0 * u2 * t) * p1 + (3.0 * u * t2) * p2 + (t * t2) * p3

            | ArcSeg(p0, p1, a0, da, ellipse) ->
                if t <= 0.0 then p0
                elif t >= 1.0 then p1
                else ellipse.GetPoint(a0 + t * da)
 
    /// evaluates the curve-derivative for the given parameter (t <- [0;1])
    let derivative (t : float) (seg : PathSegment) =
        let t = clamp 0.0 1.0 t
        match seg with
        | LineSeg(p1, p0) ->
            p1 - p0

        | ArcSeg(p0, p1, a0, da, e) ->
            let alpha = a0 + t * da
            (cos alpha * e.Axis1 - sin alpha * e.Axis0) * da

        | Bezier2Seg(p0, p1, p2) ->
            let u = 1.0 - t
            2.0 * (u*(p1 - p0) + t*(p2 - p1))

        | Bezier3Seg(p0, p1, p2, p3) ->
            let s = 1.0 - t
            let u = p1 - p0
            let v = p2 - p1
            let w = p3 - p2
            3.0 * (s*s*u + 2.0*t*s*v + t*t*w)
       

    [<AutoOpen>]
    module private Implementations = 

        let private teps = 1E-6

        let lines (epsilon : float) (a0 : V2d) (a1 : V2d) (b0 : V2d) (b1 : V2d) =
            if Fun.ApproximateEquals(a0, b0, epsilon) then [0.0, 0.0]
            elif Fun.ApproximateEquals(a0, b1, epsilon) then [0.0, 1.0]
            elif Fun.ApproximateEquals(a1, b0, epsilon) then [1.0, 0.0]
            elif Fun.ApproximateEquals(a1, b1, epsilon) then [1.0, 1.0]
            else
                let ra = Ray2d(a0, a1 - a0)
                let rb = Ray2d(b0, b1 - b0)
                let mutable ta = 0.0
                let mutable tb = 0.0
                if ra.Intersects(rb, &ta) && rb.Intersects(ra, &tb) && ta <= 1.0 + teps && ta >= -teps && tb <= 1.0 + teps && tb >= -teps then
                    let ta = clamp 0.0 1.0 ta
                    let tb = clamp 0.0 1.0 tb
                    if Fun.ApproximateEquals(ra.GetPointOnRay ta, rb.GetPointOnRay tb, epsilon) then
                        [ta, tb]
                    else
                        []
                else
                    []

        let arcLine (epsilon : float) (c0 : V2d) (c1 : V2d) (alpha0: float) (dAlpha : float) (e : Ellipse2d) (b0 : V2d) (b1 : V2d) =
            // transform the ellipse to a unit-circle and solve the system in that space.
            let toGlobal = M33d.FromCols(V3d(e.Axis0, 0.0), V3d(e.Axis1, 0.0), V3d(e.Center, 1.0))
            let toLocal = toGlobal.Inverse

            let r = Ray2d(b0, b1 - b0)

            let p0 = toLocal.TransformPos b0
            let p1 = toLocal.TransformPos b1
            let o = p0
            let d = p1 - p0


            // | o + t*d | = 1
            // <o+t*d|o+t*d> = 1
            // <o|o+t*d> + t*<d|o+t*d> = 1
            // <o|o> + 2t*<o|d> + t^2*<d|d> = 1
            // t^2*(<d|d>) + t*(2*<o|d>) + (o^2 - 1) = 0
            let a = Vec.lengthSquared d
            let b = 2.0 * Vec.dot o d
            let c = Vec.lengthSquared o - 1.0


            let q2 = b*b - 4.0*a*c
            
            let inline test (t : float) =
                if t >= -teps && t <= 1.0 + teps then
                    let t = clamp 0.0 1.0 t
                    let pt = r.GetPointOnRay t 
                    match e.TryGetAlpha(pt, epsilon) with
                    | Some a ->
                        let ta = arcT alpha0 dAlpha a
                        if ta >= -teps && ta <= 1.0 + teps then 
                            let ta = clamp 0.0 1.0 ta
                            if Fun.ApproximateEquals(pt, e.GetPoint (alpha0 + ta*dAlpha), epsilon) then
                                Some (ta, t)
                            else
                                None
                        else 
                            None
                    | None -> 
                        None
                else
                    None

            if Fun.IsTiny(q2, epsilon) then
                match test (-b / (2.0 * a)) with
                | Some t -> [t]
                | None -> []

            elif q2 < 0.0 then
                []

            else
                let q = sqrt q2
                let t1 = (-b + q) / (2.0 * a)
                let t2 = (-b - q) / (2.0 * a)

                match test t1 with
                | Some (t0a, t0b) ->
                    match test t2 with
                    | Some (t1a, t1b) ->
                        if Fun.ApproximateEquals(t0a, t1a, epsilon) then [t0a, t0b]
                        else [(t0a, t0b); (t1a, t1b)]
                    | None ->
                        [t0a, t0b]
                | None ->
                    match test t2 with
                    | Some (t1a, t1b) -> [t1a, t1b]
                    | None -> []

        let bezier2Line (epsilon : float) (p0 : V2d) (p1 : V2d) (p2 : V2d) (q0 : V2d) (q1 : V2d) =
            // o+t*d = a*t^2 + b*t + c
            
            // o.X+s*d.X = a.X*t^2 + b.X*t + c.X
            // o.Y+s*d.Y = a.Y*t^2 + b.Y*t + c.Y

            let d = q1 - q0
            let a = p0 - 2.0*p1 + p2
            let b = 2.0*p1 - 2.0*p0

            //  o.X*d.Y + s*d.X*d.Y =  a.X*d.Y*t^2 + b.X*d.Y*t + c.X*d.Y
            // -o.Y*d.X - s*d.X*d.Y = -d.X*a.Y*t^2 - d.X*b.Y*t - d.X*c.Y
            // 0 = t^2*(a.X*d.Y - d.X*a.Y)  + t*(b.X*d.Y - d.X*b.Y) + (c.X*d.Y - d.X*c.Y + o.Y*d.X - o.X*d.Y)

            let f2 = a.X*d.Y - d.X*a.Y
            let f1 = b.X*d.Y - d.X*b.Y
            let f0 = (p0.X - q0.X)*d.Y - (p0.Y - q0.Y)*d.X
            let struct (t0, t1) = Polynomial.RealRootsOf(f2, f1, f0)

            let d2 = d.LengthSquared

            let inline getS (t : float) =
                let t2 = sqr t
                let pt = a*(t2) + b*t + p0
                Vec.dot (pt - q0) d / d2

            let mutable i0 = V2d.NaN
            let mutable result = []
            if t0 >= -teps && t0 <= 1.0 + teps then
                let s0 = getS t0
                if s0 >= -teps && s0 <= 1.0 + teps then
                    let t0 = clamp 0.0 1.0 t0
                    let s0 = clamp 0.0 1.0 s0

                    let pl = q0 + s0*d
                    let ps = p0 + t0*b + (t0*t0)*a
                    if Fun.ApproximateEquals(pl, ps, epsilon) then
                        i0 <- pl
                        result <- (t0, s0) :: result

            if t1 >= -teps && t1 <= 1.0 + teps && not (Fun.ApproximateEquals(t0, t1)) then
                let s1 = getS t1
                if s1 >= -teps && s1 <= 1.0 + teps then
                    let t1 = clamp 0.0 1.0 t1
                    let s1 = clamp 0.0 1.0 s1

                    let pl = q0 + s1*d
                    let ps = p0 + t1*b + (t1*t1)*a
                    if Fun.ApproximateEquals(pl, ps, epsilon) && not (Fun.ApproximateEquals(pl, i0)) then
                        result <- (t1, s1) :: result

            result

        let bezier2 (epsilon : float) (p0 : V2d) (p1 : V2d) (p2 : V2d) (q0 : V2d) (q1 : V2d) (q2 : V2d) =
            let f0 =
               -4.0*q0.Y*q1.X*q1.Y*q2.X + 4.0*q0.X*sqr(q1.Y)*q2.X + sqr(q0.Y)*sqr(q2.X) + sqr(p0.Y)*sqr(q0.X - 2.0*q1.X + q2.X) + 4.0*q0.Y*sqr(q1.X)*q2.Y - 4.0*q0.X*q1.X*q1.Y*q2.Y - 2.0*q0.X*q0.Y*q2.X*q2.Y + sqr(q0.X)*sqr(q2.Y) + sqr(p0.X)*sqr(q0.Y - 2.0*q1.Y + q2.Y) - 
                2.0*p0.Y*(-2.0*q0.X*q1.X*q1.Y + 4.0*q0.X*q1.Y*q2.X - 2.0*q1.X*q1.Y*q2.X + q0.Y*(2.0*sqr(q1.X) - q0.X*q2.X - 2.0*q1.X*q2.X + sqr(q2.X)) + sqr(q0.X)*q2.Y - 2.0*q0.X*q1.X*q2.Y + 2.0*sqr(q1.X)*q2.Y - q0.X*q2.X*q2.Y + p0.X*(q0.X - 2.0*q1.X + q2.X)*(q0.Y - 2.0*q1.Y + q2.Y)) + 
                2.0*p0.X*(-(sqr(q0.Y)*q2.X) + 2.0*q1.Y*(-(q1.Y*q2.X) + q1.X*q2.Y) + q0.Y*(2.0*q1.Y*q2.X + 2.0*q1.X*(q1.Y - 2.0*q2.Y) + (q0.X + q2.X)*q2.Y) - q0.X*(2.0*sqr(q1.Y) - 2.0*q1.Y*q2.Y + sqr(q2.Y)))
        
            let f1 =
                -4.0*(2.0*p1.Y*q0.Y*sqr(q1.X) - 2.0*p1.Y*q0.X*q1.X*q1.Y - 2.0*p1.X*q0.Y*q1.X*q1.Y + 2.0*p1.X*q0.X*sqr(q1.Y) - p1.Y*q0.X*q0.Y*q2.X + p1.X*sqr(q0.Y)*q2.X - 2.0*p1.Y*q0.Y*q1.X*q2.X + 4.0*p1.Y*q0.X*q1.Y*q2.X - 2.0*p1.X*q0.Y*q1.Y*q2.X - 2.0*p1.Y*q1.X*q1.Y*q2.X + 2.0*p1.X*sqr(q1.Y)*q2.X + 
                     p1.Y*q0.Y*sqr(q2.X) + sqr(p0.Y)*sqr(q0.X - 2.0*q1.X + q2.X) + p1.Y*sqr(q0.X)*q2.Y - p1.X*q0.X*q0.Y*q2.Y - 2.0*p1.Y*q0.X*q1.X*q2.Y + 4.0*p1.X*q0.Y*q1.X*q2.Y + 2.0*p1.Y*sqr(q1.X)*q2.Y - 2.0*p1.X*q0.X*q1.Y*q2.Y - 2.0*p1.X*q1.X*q1.Y*q2.Y - p1.Y*q0.X*q2.X*q2.Y - p1.X*q0.Y*q2.X*q2.Y + 
                     p1.X*q0.X*sqr(q2.Y) + sqr(p0.X)*sqr(q0.Y - 2.0*q1.Y + q2.Y) + p0.Y*(p1.X*q0.X*q0.Y - 2.0*p1.X*q0.Y*q1.X - 2.0*q0.Y*sqr(q1.X) - 2.0*p1.X*q0.X*q1.Y + 4.0*p1.X*q1.X*q1.Y + 2.0*q0.X*q1.X*q1.Y + p1.X*q0.Y*q2.X + q0.X*q0.Y*q2.X + 2.0*q0.Y*q1.X*q2.X - 2.0*p1.X*q1.Y*q2.X - 4.0*q0.X*q1.Y*q2.X + 
                        2.0*q1.X*q1.Y*q2.X - q0.Y*sqr(q2.X) - p1.Y*sqr(q0.X - 2.0*q1.X + q2.X) + p1.X*q0.X*q2.Y - sqr(q0.X)*q2.Y - 2.0*p1.X*q1.X*q2.Y + 2.0*q0.X*q1.X*q2.Y - 2.0*sqr(q1.X)*q2.Y + p1.X*q2.X*q2.Y + q0.X*q2.X*q2.Y - 2.0*p0.X*(q0.X - 2.0*q1.X + q2.X)*(q0.Y - 2.0*q1.Y + q2.Y)) + 
                     p0.X*(2.0*q0.Y*q1.X*q1.Y - 2.0*q0.X*sqr(q1.Y) - sqr(q0.Y)*q2.X + 2.0*q0.Y*q1.Y*q2.X - 2.0*sqr(q1.Y)*q2.X + q0.X*q0.Y*q2.Y - 4.0*q0.Y*q1.X*q2.Y + 2.0*q0.X*q1.Y*q2.Y + 2.0*q1.X*q1.Y*q2.Y + q0.Y*q2.X*q2.Y - q0.X*sqr(q2.Y) + p1.Y*(q0.X - 2.0*q1.X + q2.X)*(q0.Y - 2.0*q1.Y + q2.Y) - 
                        p1.X*sqr(q0.Y - 2.0*q1.Y + q2.Y)))

            let f2 =
              2.0*(-(p0.X*p2.Y*q0.X*q0.Y) + 3.0*sqr(p0.X)*sqr(q0.Y) - 6.0*p0.X*p1.X*sqr(q0.Y) + 2.0*sqr(p1.X)*sqr(q0.Y) + p0.X*p2.X*sqr(q0.Y) + 2.0*p0.X*p2.Y*q0.Y*q1.X - 2.0*p2.Y*q0.Y*sqr(q1.X) + 2.0*p0.X*p2.Y*q0.X*q1.Y - 12.0*sqr(p0.X)*q0.Y*q1.Y + 24.0*p0.X*p1.X*q0.Y*q1.Y - 
                 8.0*sqr(p1.X)*q0.Y*q1.Y - 4.0*p0.X*p2.X*q0.Y*q1.Y - 4.0*p0.X*p2.Y*q1.X*q1.Y + 2.0*p2.Y*q0.X*q1.X*q1.Y + 2.0*p0.X*q0.Y*q1.X*q1.Y - 4.0*p1.X*q0.Y*q1.X*q1.Y + 2.0*p2.X*q0.Y*q1.X*q1.Y + 12.0*sqr(p0.X)*sqr(q1.Y) - 24.0*p0.X*p1.X*sqr(q1.Y) + 8.0*sqr(p1.X)*sqr(q1.Y) + 
                 4.0*p0.X*p2.X*sqr(q1.Y) - 2.0*p0.X*q0.X*sqr(q1.Y) + 4.0*p1.X*q0.X*sqr(q1.Y) - 2.0*p2.X*q0.X*sqr(q1.Y) - p0.X*p2.Y*q0.Y*q2.X + p2.Y*q0.X*q0.Y*q2.X - p0.X*sqr(q0.Y)*q2.X + 2.0*p1.X*sqr(q0.Y)*q2.X - p2.X*sqr(q0.Y)*q2.X + 2.0*p2.Y*q0.Y*q1.X*q2.X + 2.0*p0.X*p2.Y*q1.Y*q2.X - 
                 4.0*p2.Y*q0.X*q1.Y*q2.X + 2.0*p0.X*q0.Y*q1.Y*q2.X - 4.0*p1.X*q0.Y*q1.Y*q2.X + 2.0*p2.X*q0.Y*q1.Y*q2.X + 2.0*p2.Y*q1.X*q1.Y*q2.X - 2.0*p0.X*sqr(q1.Y)*q2.X + 4.0*p1.X*sqr(q1.Y)*q2.X - 2.0*p2.X*sqr(q1.Y)*q2.X - p2.Y*q0.Y*sqr(q2.X) + 3.0*sqr(p0.Y)*sqr(q0.X - 2.0*q1.X + q2.X) + 
                 2.0*sqr(p1.Y)*sqr(q0.X - 2.0*q1.X + q2.X) - p0.X*p2.Y*q0.X*q2.Y - p2.Y*sqr(q0.X)*q2.Y + 6.0*sqr(p0.X)*q0.Y*q2.Y - 12.0*p0.X*p1.X*q0.Y*q2.Y + 4.0*sqr(p1.X)*q0.Y*q2.Y + 2.0*p0.X*p2.X*q0.Y*q2.Y + p0.X*q0.X*q0.Y*q2.Y - 2.0*p1.X*q0.X*q0.Y*q2.Y + p2.X*q0.X*q0.Y*q2.Y + 
                 2.0*p0.X*p2.Y*q1.X*q2.Y + 2.0*p2.Y*q0.X*q1.X*q2.Y - 4.0*p0.X*q0.Y*q1.X*q2.Y + 8.0*p1.X*q0.Y*q1.X*q2.Y - 4.0*p2.X*q0.Y*q1.X*q2.Y - 2.0*p2.Y*sqr(q1.X)*q2.Y - 12.0*sqr(p0.X)*q1.Y*q2.Y + 24.0*p0.X*p1.X*q1.Y*q2.Y - 8.0*sqr(p1.X)*q1.Y*q2.Y - 4.0*p0.X*p2.X*q1.Y*q2.Y + 2.0*p0.X*q0.X*q1.Y*q2.Y - 
                 4.0*p1.X*q0.X*q1.Y*q2.Y + 2.0*p2.X*q0.X*q1.Y*q2.Y + 2.0*p0.X*q1.X*q1.Y*q2.Y - 4.0*p1.X*q1.X*q1.Y*q2.Y + 2.0*p2.X*q1.X*q1.Y*q2.Y - p0.X*p2.Y*q2.X*q2.Y + p2.Y*q0.X*q2.X*q2.Y + p0.X*q0.Y*q2.X*q2.Y - 2.0*p1.X*q0.Y*q2.X*q2.Y + p2.X*q0.Y*q2.X*q2.Y + 3.0*sqr(p0.X)*sqr(q2.Y) - 6.0*p0.X*p1.X*sqr(q2.Y) + 
                 2.0*sqr(p1.X)*sqr(q2.Y) + p0.X*p2.X*sqr(q2.Y) - p0.X*q0.X*sqr(q2.Y) + 2.0*p1.X*q0.X*sqr(q2.Y) - p2.X*q0.X*sqr(q2.Y) - 
                 p0.Y*(6.0*p0.X*q0.X*q0.Y - 6.0*p1.X*q0.X*q0.Y + p2.X*q0.X*q0.Y - 12.0*p0.X*q0.Y*q1.X + 12.0*p1.X*q0.Y*q1.X - 2.0*p2.X*q0.Y*q1.X + 2.0*q0.Y*sqr(q1.X) - 12.0*p0.X*q0.X*q1.Y + 12.0*p1.X*q0.X*q1.Y - 2.0*p2.X*q0.X*q1.Y + 24.0*p0.X*q1.X*q1.Y - 24.0*p1.X*q1.X*q1.Y + 4.0*p2.X*q1.X*q1.Y - 2.0*q0.X*q1.X*q1.Y + 
                    6.0*p0.X*q0.Y*q2.X - 6.0*p1.X*q0.Y*q2.X + p2.X*q0.Y*q2.X - q0.X*q0.Y*q2.X - 2.0*q0.Y*q1.X*q2.X - 12.0*p0.X*q1.Y*q2.X + 12.0*p1.X*q1.Y*q2.X - 2.0*p2.X*q1.Y*q2.X + 4.0*q0.X*q1.Y*q2.X - 2.0*q1.X*q1.Y*q2.X + q0.Y*sqr(q2.X) + 6.0*p1.Y*sqr(q0.X - 2.0*q1.X + q2.X) - p2.Y*sqr(q0.X - 2.0*q1.X + q2.X) + 
                    6.0*p0.X*q0.X*q2.Y - 6.0*p1.X*q0.X*q2.Y + p2.X*q0.X*q2.Y + sqr(q0.X)*q2.Y - 12.0*p0.X*q1.X*q2.Y + 12.0*p1.X*q1.X*q2.Y - 2.0*p2.X*q1.X*q2.Y - 2.0*q0.X*q1.X*q2.Y + 2.0*sqr(q1.X)*q2.Y + 6.0*p0.X*q2.X*q2.Y - 6.0*p1.X*q2.X*q2.Y + p2.X*q2.X*q2.Y - q0.X*q2.X*q2.Y) + 
                 2.0*p1.Y*(2.0*q0.Y*sqr(q1.X) - 2.0*q0.X*q1.X*q1.Y - q0.X*q0.Y*q2.X - 2.0*q0.Y*q1.X*q2.X + 4.0*q0.X*q1.Y*q2.X - 2.0*q1.X*q1.Y*q2.X + q0.Y*sqr(q2.X) + sqr(q0.X)*q2.Y - 2.0*q0.X*q1.X*q2.Y + 2.0*sqr(q1.X)*q2.Y - q0.X*q2.X*q2.Y + 3.0*p0.X*(q0.X - 2.0*q1.X + q2.X)*(q0.Y - 2.0*q1.Y + q2.Y) - 
                    2.0*p1.X*(q0.X - 2.0*q1.X + q2.X)*(q0.Y - 2.0*q1.Y + q2.Y)))

            let f3 =
                -4.0*(p2.Y*q0.X - p0.X*q0.Y + 2.0*p1.X*q0.Y - p2.X*q0.Y - 2.0*p2.Y*q1.X + 2.0*p0.X*q1.Y - 4.0*p1.X*q1.Y + 2.0*p2.X*q1.Y + p2.Y*q2.X + p0.Y*(q0.X - 2.0*q1.X + q2.X) - 2.0*p1.Y*(q0.X - 2.0*q1.X + q2.X) - p0.X*q2.Y + 2.0*p1.X*q2.Y - p2.X*q2.Y)*
                    (p0.Y*(q0.X - 2.0*q1.X + q2.X) - p1.Y*(q0.X - 2.0*q1.X + q2.X) - (p0.X - p1.X)*(q0.Y - 2.0*q1.Y + q2.Y))
        
            let f4 = 
                sqr(-(p2.Y*q0.X) + p0.X*q0.Y - 2.0*p1.X*q0.Y + p2.X*q0.Y + 2.0*p2.Y*q1.X - 2.0*p0.X*q1.Y + 4.0*p1.X*q1.Y - 2.0*p2.X*q1.Y - p2.Y*q2.X - p0.Y*(q0.X - 2.0*q1.X + q2.X) + 2.0*p1.Y*(q0.X - 2.0*q1.X + q2.X) + p0.X*q2.Y - 2.0*p1.X*q2.Y + p2.X*q2.Y)

            let struct (t0, t1, t2, t3) = Polynomial.RealRootsOf(f4, f3, f2, f1, f0)

            let inline evalP (t : float) =
                if t >= -teps && t <= 1.0 + teps then
                    let t = clamp 0.0 1.0 t
                    let s = 1.0 - t
                    Some (t, p0*s*s + 2.0*p1*s*t + p2*t*t)
                else
                    None

            let inline evalQ (t : float) =
                if t >= -teps && t <= 1.0 + teps then
                    let t = clamp 0.0 1.0 t
                    let s = 1.0 - t
                    Some (t, q0*s*s + 2.0*q1*s*t + q2*t*t)
                else
                    None
                
            let test (tp : float) =
                match evalP tp with
                | Some(tp, pp) ->
                    match findBezierT teps pp q0 q1 q2 with
                    | Some tq ->
                        match evalQ tq with
                        | Some (tq, pq) ->
                            if Fun.ApproximateEquals(pq, pp, epsilon) then [tp, tq]
                            else []
                        | None ->
                            []
                    | None ->
                        []
                | None ->
                    []

            test t0 @ test t1 @ test t2 @ test t3

        let ellipses (eps : float) (e0 : Ellipse2d) (e1 : Ellipse2d) =
            let m = M33d.FromCols(V3d(e0.Axis0, 0.0), V3d(e0.Axis1, 0.0), V3d(e0.Center, 1.0))
            let mi = m.Inverse

            let c = mi.TransformPos e1.Center
            let a = mi.TransformDir e1.Axis0
            let b = mi.TransformDir e1.Axis1

            // |c + cos t * a + sin t * b| = 1
            // |c + cos t * a + sin t * b|^2 = 1
            // <c + cos(t)*a + sin(t)*b | c + cos(t)*a + sin(t)*b> = 1

            // <c|c> + cos(t)*<a|c> + sin(t)*<b|c> + 
            // cos(t)*<a|c> + cos(t)^2*<a|a> + sin(t)*cos(t)*<a|b> + 
            // sin(t)*<b|c> + sin(t)*cos(t)*<a|b> + sin(t)^2*<b|b> - 1 = 0

            // (cos(t)^2*<a|a> + sin(t)^2*<b|b>) + 2*(sin(t)*cos(t)*<a|b> + cos(t)*<a|c> + sin(t)*<b|c>) + <c|c> - 1 = 0

            let aa = a.LengthSquared
            let bb = b.LengthSquared
            let cc = c.LengthSquared
            let ab = Vec.dot a b
            let ac = Vec.dot a c
            let bc = Vec.dot b c


            // (c^2*<a|a> + s^2*<b|b>) + 2*(s*c*<a|b> + c*<a|c> + s*<b|c>) + <c|c> - 1 = 0
            // c^2*aa + s^2*bb + 2*(s*c*ab + c*ac + s*bc) + cc - 1 = 0

            // Mathematica:
            // s0 := Eliminate [c^2*aa + s^2*bb + 2*(s*c*ab + c*ac + s*bc) + cc - 1 == 0 && c^2 + s^2 == 1, s]
            // s1 := SubtractSides[s0][[1]]
            // FullSimplify[Coefficient[s1, c, 0]]
            let f0 = (-1.0 + bb - 2.0*bc + cc)*(-1.0 + bb + 2.0*bc + cc)
            let f1 = -8.0*ab*bc + 4.0*ac*(-1.0 + bb + cc)
            let f2 = 2.0*(-2.0*sqr(ab) + 2.0*sqr(ac) + bb + 2.0*sqr(bc) + aa*(-1.0 + bb + cc) - bb*(bb + cc))
            let f3 = 4.0*aa*ac - 4.0*ac*bb + 8.0*ab*bc
            let f4 = 4.0*sqr(ab) + sqr(aa - bb)
            let struct(c0, c1, c2, c3) = Polynomial.RealRootsOf(f4, f3, f2, f1, f0)

            let g0 = (-1.0 + aa - 2.0*ac + cc)*(-1.0 + aa + 2.0*ac + cc)
            let g1 = -8.0*ab*ac + 4.0*bc*(-1.0 + aa + cc)
            let g2 = -2.0*(sqr(aa) + 2.0*sqr(ab) - 2.0*sqr(ac) + bb - 2.0*sqr(bc) - bb*cc + aa*(-1.0 - bb + cc))
            let g3 = 8.0*ab*ac + 4.0*(-aa + bb)*bc
            let g4 = 4.0*sqr(ab) + sqr(aa - bb)
            let struct(s0, s1, s2, s3) = Polynomial.RealRootsOf(g4, g3, g2, g1, g0)

           
            let add (v0 : float, v1 : float) (l : list<float * float>) =
                let exists = l |> List.exists (fun (va, vb) -> Fun.ApproximateEquals(v0, va, 1E-8) && Fun.ApproximateEquals(v1, vb, 1E-8))
                if exists then l
                else (v0, v1) :: l

            let sols =
                let mutable sols = []
                for c in [c0;c1;c2;c3] do
                    if c >= -1.0 && c <= 1.0 then 
                        let s = sqrt(1.0 - sqr c)
                        sols <- sols |> add (c, s) |> add (c, -s)
                    
                for s in [s0;s1;s2;s3] do
                    if s >= -1.0 && s <= 1.0 then 
                        let c = sqrt(1.0 - sqr s)
                        sols <- sols |> add (c, s) |> add (-c, s)

                sols

            let rec getSolutions (acc : list<float * float>) (l : list<float * float>) =
                match l with
                | [] -> acc
                | (cos, sin) :: t ->
                    let p = c + cos*a + sin*b
                    let a0 = atan2 p.Y p.X
                    let a1 = atan2 sin cos

                    let p0 = e0.GetPoint a0
                    let p1 = e1.GetPoint a1
                    if Fun.ApproximateEquals(p0, p1, eps) then
                        getSolutions ((a0, a1) :: acc) t
                    else
                        getSolutions acc t

                    



            let solutions = getSolutions [] sols |> List.sort


            solutions

        let arcs (epsilon : float) (a0 : float) (da : float) (a : Ellipse2d) (b0 : float) (db : float) (b : Ellipse2d) =
            ellipses epsilon a b |> List.choose (fun (aa, ba) ->
                let ta = arcT a0 da aa
                let tb = arcT b0 db ba
                if ta >= -teps && ta <= 1.0 + teps && tb >= -teps && tb <= 1.0 + teps then
                    let ta = clamp 0.0 1.0 ta
                    let tb = clamp 0.0 1.0 tb
                    let pa = a.GetPoint(a0 + da*ta)
                    let pb = b.GetPoint(b0 + db*tb)
                    if Fun.ApproximateEquals(pa, pb, epsilon) then
                        Some (ta, tb)
                    else
                        None
                else
                    None
            )
        
        let private bezier2Ellipse (p0 : V2d) (p1 : V2d) (p2 : V2d) (e : Ellipse2d) =
            let m = M33d.FromCols(V3d(e.Axis0, 0.0), V3d(e.Axis1, 0.0), V3d(e.Center, 1.0))
            let mi = m.Inverse

            let q0 = mi.TransformPos p0
            let q1 = mi.TransformPos p1
            let q2 = mi.TransformPos p2

            let a = q0 - 2.0*q1 + q2
            let b = 2.0*q1 - 2.0*q0
            let c = q0

            // |a*t^2 + b*t + c| = 1
            // <a*t^2 + b*t + c | a*t^2 + b*t + c > = 1
        
            // t^4*<a|a> + t^3*<a|b> + t^2*<a|c> +
            // t^3*<a|b> + t^2*<b|b> + t*<b|c> +
            // t^2*<a|c> + t*<b|c> + <c|c> - 1 = 0

            // t^4*(<a|a>) + t^3*(2*<a|b>) + t^2*(2*<a|c> + <b|b>) + t*(2*<b|c>) + (<c|c> - 1) = 0

            let f0 = c.LengthSquared - 1.0
            let f1 = 2.0 * Vec.dot b c
            let f2 = 2.0 * Vec.dot a c + b.LengthSquared
            let f3 = 2.0 * Vec.dot a b
            let f4 = a.LengthSquared

            let struct (t0, t1, t2, t3) = Polynomial.RealRootsOf(f4, f3, f2, f1, f0)

            [t0; t1; t2; t3] |> List.choose (fun t ->
                if t >= -teps && t <= 1.0 + teps then
                    let t = clamp 0.0 1.0 t
                    let p = a*sqr t + b*t + c
                    let alpha = atan2 p.Y p.X
                    Some (t, alpha)
                else
                    None
            )

        let bezier2Arc (epsilon : float) (p0 : V2d) (p1 : V2d) (p2 : V2d) (alpha0 : float) (dAlpha : float) (e : Ellipse2d) =
            bezier2Ellipse p0 p1 p2 e |> List.choose (fun (t, a) ->
                let te = arcT alpha0 dAlpha a
                if te >= -teps && te <= 1.0 + teps then
                    let t = clamp 0.0 1.0 t
                    let te = clamp 0.0 1.0 te

                    let s = 1.0 - t
                    let pb = p0*(s*s) + 2.0*p1*(s*t) + p2*(t*t)
                    let pe = e.GetPoint(alpha0 + dAlpha*te)
                    if Fun.ApproximateEquals(pb, pe, epsilon) then
                        Some (t, te)
                    else
                        None
                else
                    None
            )
            
        let bezier3Line (epsilon : float) (p0 : V2d) (p1 : V2d) (p2 : V2d) (p3 : V2d) (q0 : V2d) (q1 : V2d) =
            
            let a = -p0 + 3.0*p1 - 3.0*p2 + p3
            let b = 3.0*p0 - 6.0*p1 + 3.0*p2
            let c = 3.0*p1 - 3.0*p0
            let d = p0

            let o = q0
            let v = q1 - q0

            // v*t + o = a*s^3 + b*s^2 + c*s + d

            // v.X*t + o.X = a.X*s^3 + b.X*s^2 + c.X*s + d.X
            // v.Y*t + o.Y = a.Y*s^3 + b.Y*s^2 + c.Y*s + d.Y

            
            // v.Y*v.X*t + v.Y*o.X = v.Y*a.X*s^3 + v.Y*b.X*s^2 + v.Y*c.X*s + v.Y*d.X
            // -v.X*v.Y*t - v.X*o.Y = -v.X*a.Y*s^3 - v.X*b.Y*s^2 - v.X*c.Y*s - v.X*d.Y

            // 0 = s^3*(v.Y*a.X - v.X*a.Y) + s^2*(v.Y*b.X - v.X*b.Y) + s*(v.Y*c.X - v.X*c.Y) + (v.Y*d.X - v.X*d.Y - v.Y*o.X + v.X*o.Y)

            let f3 = v.Y*a.X - v.X*a.Y
            let f2 = v.Y*b.X - v.X*b.Y
            let f1 = v.Y*c.X - v.X*c.Y
            let f0 = v.Y*d.X - v.X*d.Y - v.Y*o.X + v.X*o.Y
            let struct (s0, s1, s2) = Polynomial.RealRootsOf(f3, f2, f1, f0)

            [s0;s1;s2] |> List.choose (fun ts ->
                if ts >= -teps && ts <= 1.0 + teps then
                    let ts = clamp 0.0 1.0 ts
                    let ts2 = sqr ts
                    let pt = a*ts*ts2 + b*ts2 + c*ts + d
                    let tl = Vec.dot v (pt - o) / v.LengthSquared
                    if tl >= -teps && tl <= 1.0 + teps then
                        let tl = clamp 0.0 1.0 tl
                        let pl = o + v*tl
                        if Fun.ApproximateEquals(pt, pl, epsilon) then
                            Some (ts, tl)
                        else
                            None
                    else
                        None
                else
                    None
            )

        let private halves (s : PathSegment) =
            match s with
            | LineSeg(p0, p1) ->
                let m = (p0+p1)/2.0
                LineSeg(p0, m), LineSeg(m, p1)

            | ArcSeg(p0, p1, a0, da, e) ->
                let dh = da / 2.0
                let pm = e.GetPoint(a0 + dh)
                ArcSeg(p0, pm, a0, dh, e), ArcSeg(pm, p1, a0+dh, dh, e)


            | Bezier2Seg(p0, p1, p2) ->
                let q0 = (p0+p1) / 2.0
                let q1 = (p1+p2) / 2.0
                let c = (q0+q1)/2.0
                Bezier2Seg(p0, q0, c), Bezier2Seg(c, q1, p2)

            | Bezier3Seg(p0, p1, p2, p3) ->
                let q0 = (p0+p1) / 2.0
                let q1 = (p1+p2) / 2.0
                let q2 = (p2+p3) / 2.0
                let m0 = (q0+q1) / 2.0
                let m1 = (q1+q2) / 2.0
                let c = (m0+m1) / 2.0
                Bezier3Seg(p0, q0, m0, c), Bezier3Seg(c, m1, q2, p3)

        let numeric (epsilon : float) (l : PathSegment) (r : PathSegment) =
            
            let stack = System.Collections.Generic.Stack<struct(PathSegment * PathSegment * V4d)>(64)

            let mutable result = []
            let mutable points = []
            let mutable cnt = 0

            let newtonStep (t : V2d) =
                let vl = point t.X l
                let vr = point t.Y r
                let dl = derivative t.X l
                let dr = derivative t.Y r


                // v(x0, x1) = l(x0) - r(x1)
                // dv/dx0 = l'(x0)
                // dv/dx1 = -r'(x1)

                let jacobian =
                    M22d(
                        dl.X, -dr.X,
                        dl.Y, -dr.Y
                    )

                let inv = jacobian.Inverse
                let delta = inv * (vl - vr)
                -delta



            let add (atl : float) (atr : float) =

                let mutable t = V2d(atl, atr)
                let mutable lastStep = 1E10
                let mutable iter = 0
                while iter < 30 && lastStep > 1E-15 do
                    let dt = newtonStep t
                    t <- t + dt
                    lastStep <- dt.NormMax
                    iter <- iter + 1
                
                

                let tl = clamp 0.0 1.0 t.X
                let tr = clamp 0.0 1.0 t.Y
                let pl = point tl l
                let pr = point tr r
                let largeEps = 100.0 * epsilon


                if Fun.ApproximateEquals(pl, pr, largeEps) then
                    let pt = (pl + pr) / 2.0
                    let contained = points |> List.exists (fun p -> Fun.ApproximateEquals(p, pt, largeEps))
                    if not contained then 
                        result <- (tl, tr) :: result
                        points <- pt :: points
                        cnt <- cnt + 1

            stack.Push(struct(l, r, V4d(0.0,1.0,0.0,1.0)))
            let mutable maxLength = 0

            while stack.Count > 0 && cnt < 9 do
                maxLength <- max maxLength stack.Count
                let struct(l, r, ranges) = stack.Pop()
                let lo = ranges.X
                let ls = ranges.Y
                let ro = ranges.Z
                let rs = ranges.W

                let lb = bounds l
                let rb = bounds r
                if lb.Intersects rb then
                    if lb.Size.NormMax <= 1E-6 || rb.Size.NormMax <= 1E-6 then
                        add (lo + ls/2.0) (ro + rs/2.0)
                    else
                        let l0, l1 = halves l
                        let r0, r1 = halves r
                    
                        let lsh = ls / 2.0
                        let rsh = rs / 2.0

                        let l0o = lo
                        let l1o = lo + lsh
                        let r0o = ro
                        let r1o = ro + rsh
                        stack.Push(struct(l0, r0, V4d(l0o, lsh, r0o, rsh)))
                        stack.Push(struct(l0, r1, V4d(l0o, lsh, r1o, rsh)))
                        stack.Push(struct(l1, r0, V4d(l1o, lsh, r0o, rsh)))
                        stack.Push(struct(l1, r1, V4d(l1o, lsh, r1o, rsh)))

                ()

            if cnt > 9 then []
            else result

    let intersections (eps : float) (a : PathSegment) (b : PathSegment) : list<float * float> =
        match a, b with
        | LineSeg(a0, a1), LineSeg(b0, b1) ->
            lines eps a0 a1 b0 b1

        | LineSeg(a0, a1), Bezier2Seg(b0, b1, b2) ->
            bezier2Line eps b0 b1 b2 a0 a1 
            |> List.map flip
            |> List.sortBy fst
            
        | LineSeg(a0, a1), ArcSeg(c0, c1, b0, db, b) ->
            arcLine eps c0 c1 b0 db b a0 a1
            |> List.map flip
            |> List.sortBy fst

        | LineSeg(a0, a1), Bezier3Seg(b0, b1, b2, b3) ->
            bezier3Line eps b0 b1 b2 b3 a0 a1
            |> List.map flip
            |> List.sortBy fst

        | Bezier2Seg(a0, a1, a2), LineSeg(b0, b1) ->
            bezier2Line eps a0 a1 a2 b0 b1
            |> List.sortBy fst
            
        | Bezier2Seg(a0, a1, a2), Bezier2Seg(b0, b1, b2) ->
            bezier2 eps a0 a1 a2 b0 b1 b2
            |> List.sortBy fst

        | Bezier2Seg(a0, a1, a2), ArcSeg(_, _, b0, db, b) ->
            bezier2Arc eps a0 a1 a2 b0 db b
            |> List.sortBy fst
            
        | Bezier2Seg _, Bezier3Seg _ ->
            numeric eps a b
            |> List.sortBy fst

        | ArcSeg(c0, c1, a0, da, a), LineSeg(b0, b1) ->
            arcLine eps c0 c1 a0 da a b0 b1
            |> List.sortBy fst
        
        | ArcSeg(_, _, a0, da, a), Bezier2Seg(b0, b1, b2) ->
            bezier2Arc eps b0 b1 b2 a0 da a
            |> List.map flip
            |> List.sortBy fst
            
        | ArcSeg(ap0, ap1, a0, da, a), ArcSeg(bp0, bp1, b0, db, b) ->
            if Fun.ApproximateEquals(ap0, bp0, eps) then [0.0, 0.0]
            elif Fun.ApproximateEquals(ap0, bp1, eps) then [0.0, 1.0]
            elif Fun.ApproximateEquals(ap1, bp0, eps) then [1.0, 0.0]
            elif Fun.ApproximateEquals(ap1, bp1, eps) then [1.0, 1.0]
            else
                arcs eps a0 da a b0 db b
                |> List.sortBy fst
            
        | ArcSeg _, Bezier3Seg _ ->
            numeric eps a b
            |> List.sortBy fst

        | Bezier3Seg(a0, a1, a2, a3), LineSeg(b0, b1) ->
            bezier3Line eps a0 a1 a2 a3 b0 b1
            |> List.sortBy fst
            
        | Bezier3Seg _, Bezier2Seg _ ->
            numeric eps a b
            |> List.sortBy fst

        | Bezier3Seg _, ArcSeg _ ->
            numeric eps a b
            |> List.sortBy fst

        | Bezier3Seg _, Bezier3Seg _ ->
            numeric eps a b
            |> List.sortBy fst
            

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PathSegment =
    

    let inline private cc (a : V2d) (b : V2d) =
        a.X*b.Y - a.Y*b.X

    /// creates a clean (non-conjugate, non-clockwise) ellipse from two possilby conjugate diameters
    let private createEllipse (center : V2d) (a0 : V2d) (a1 : V2d) =
        let mutable a0 = a0
        let mutable a1 = a1
        if not (Fun.IsTiny(Vec.dot a0 a1, epsilon)) then
            let e = Ellipse2d.FromConjugateDiameters(center, a0, a1)
            a0 <- e.Axis0
            a1 <- e.Axis1

        // make the one with larger |X| Axis0
        if abs a1.X > abs a0.X then Fun.Swap(&a0, &a1)
        // flip the axis0 to positive x
        if a0.X < 0.0 then a0 <- -a0
        // ensure that Axis1 is left of Axis0
        if cc a0 a1 < 0.0 then a1 <- -a1

        Ellipse2d(center, a0, a1)

    /// unsafely replaces the start point of a path segment
    let withP0 (pt : V2d) (s : PathSegment) =
        match s with
        | LineSeg(_, p1) -> PathSegment.LineSeg(pt, p1)
        | Bezier2Seg(_, p1, p2) -> PathSegment.Bezier2Seg(pt, p1, p2)
        | Bezier3Seg(_, p1, p2, p3) -> PathSegment.Bezier3Seg(pt, p1, p2, p3)
        | ArcSeg(_, p1, a, da, e) -> PathSegment.ArcSeg(pt, p1, a, da, e)

    /// unsafely replaces the end point of a path segment
    let withP1 (pt : V2d) (s : PathSegment) =
        match s with
        | LineSeg(p0, _) -> PathSegment.LineSeg(p0, pt)
        | Bezier2Seg(p0, p1, p2) -> PathSegment.Bezier2Seg(p0, p1, pt)
        | Bezier3Seg(p0, p1, p2, p3) -> PathSegment.Bezier3Seg(p0, p1, p2, pt)
        | ArcSeg(p0, p1, a, da, e) -> PathSegment.ArcSeg(p0, pt, a, da, e)
     

    /// creates a line segment
    let line (p0 : V2d) (p1 : V2d) = 
        if Fun.ApproximateEquals(p0, p1, epsilon) then
            failwithf "[PathSegment] degenerate line at: %A" p0

        LineSeg(p0, p1)

    /// creates a quadratic bezier segment
    let bezier2 (p0 : V2d) (p1 : V2d) (p2 : V2d) = 
        // check if the spline is actually a line
        let d = p2 - p0 |> Vec.normalize
        let n = V2d(-d.Y, d.X)
        
        
        if Fun.IsTiny(Vec.dot (p1 - p0) n, epsilon) then 
            line p0 p2
        else 
            Bezier2Seg(p0, p1, p2)

    /// creates a cubic bezier segment
    let bezier3 (p0 : V2d) (p1 : V2d) (p2 : V2d) (p3 : V2d) =
        if Fun.ApproximateEquals(p0, p1, epsilon) && Fun.ApproximateEquals(p1, p2, epsilon) && Fun.ApproximateEquals(p2, p3, epsilon) then
            failwithf "[PathSegment] degenerate line at: %A" p0
        else
            let dd = (p3 - p0)
            let len = Vec.length dd
            let d03 = dd / len
            let d01 = Vec.normalize (p1 - p0)
            let d23 = Vec.normalize (p3 - p2)

            let n = V2d(-d03.Y, d03.X)
            let h1 = Vec.dot (p1 - p0) n
            let h2 = Vec.dot (p2 - p0) n

            if Fun.IsTiny(h1, epsilon) && Fun.IsTiny(h2, epsilon) then
                line p0 p1
            else
                let f3 = -p0 + 3.0*p1 - 3.0*p2 + p3
                if Fun.IsTiny(f3, epsilon) then
                    let ctrl = -p0 + 3.0*p1 - 1.5*p2 + 0.5*p3
                    bezier2 p0 ctrl p3
                else
                    Bezier3Seg(p0, p1, p2, p3)
            


    /// creates a line segment (if not degenerate)
    let tryLine (p0 : V2d) (p1 : V2d) =
        if Fun.ApproximateEquals(p0, p1, epsilon) then None
        else Some (LineSeg(p0, p1))

    /// creates a qudratic bezier segment (if not degenerate)
    let tryBezier2 (p0 : V2d) (p1 : V2d) (p2 : V2d) = 
        // check if the spline is actually a line
        let d = p2 - p0 |> Vec.normalize
        let n = V2d(-d.Y, d.X)
        
        
        if Fun.IsTiny(Vec.dot (p1 - p0) n, epsilon) then 
            tryLine p0 p2
        else 
            Bezier2Seg(p0, p1, p2) |> Some

    /// creates a cubic bezier segment (if not degenerate)
    let tryBezier3 (p0 : V2d) (p1 : V2d) (p2 : V2d) (p3 : V2d) =
        if Fun.ApproximateEquals(p0, p1, epsilon) && Fun.ApproximateEquals(p1, p2, epsilon) && Fun.ApproximateEquals(p2, p3, epsilon) then
            None
        else
            let dd = (p3 - p0)
            let len = Vec.length dd
            let d03 = dd / len
            
            let n = V2d(-d03.Y, d03.X)
            let h1 = Vec.dot (p1 - p0) n
            let h2 = Vec.dot (p2 - p0) n

            if Fun.IsTiny(h1, epsilon) && Fun.IsTiny(h2, epsilon) then
                tryLine p0 p1
            else        
                // let f3 = -p0 + 3.0*p1 - 3.0*p2 + p3
                // let f2 = 3.0*p0 - 6.0*p1 + 3.0*p2
                // let f1 = 3.0*p1 - 3.0*p0
                // let f0 = p0
                
                
                // let g2 = q2 + q0 - 2.0*q1
                // let g1 = 2.0*(q1 - q0)
                // let g0 = q0
                
                
                // f3 = 0
                // =>
                // q2 + q0 - 2.0*q1 = 3.0*p0 - 6.0*p1 + 3.0*p2
                // 3.0*p1 - 3.0*p0 = 2.0*q1 - 2.0*q0
                // p0 = q0
                
                // semantically: q2 = p3
                
                // q1 = -p0 + 3*p1 - 1.5*p2 + 0.5*p3
                let f3 = -p0 + 3.0*p1 - 3.0*p2 + p3
                if Fun.IsTiny(f3, epsilon) then
                    let ctrl = -p0 + 3.0*p1 - 1.5*p2 + 0.5*p3
                    tryBezier2 p0 ctrl p3
                else
                    Bezier3Seg(p0, p1, p2, p3) |> Some

    let private createArc (force : bool) (p0 : V2d) (p1 : V2d) (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        let newEllipse = createEllipse ellipse.Center ellipse.Axis0 ellipse.Axis1
        let a0 = newEllipse.GetAlpha p0
        let da =
            if not ellipse.IsCcw then -dAlpha
            else dAlpha

        let pm = newEllipse.GetPoint(a0 + da / 2.0)
        if not force && not (Fun.ApproximateEquals(p0, p1, epsilon)) && Fun.IsTiny(pm.PosLeftOfLineValue(p0, p1), epsilon) then
            tryLine p0 p1
        else
            ArcSeg(p0, p1, a0, da, newEllipse) |> Some

    
    /// creates an arc using the an angle alpha0 and a (signed) dAlpha.
    /// in order to avoid precision issues p0,p1 users may redundantly supply p0 and p1.
    /// the implementation validates that p0~ellipse.GetPoint(alpha0) and p1~ellipse.GetPoint(alpha0+dAlpha) respectively.
    let tryArcWithPoints (p0 : V2d) (p1 : V2d) (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        if Fun.ApproximateEquals(p0, p1, epsilon) then
            None
        elif Fun.ApproximateEquals(p0, ellipse.GetPoint alpha0, epsilon) && Fun.ApproximateEquals(p1, ellipse.GetPoint (alpha0 + dAlpha), epsilon) then
            createArc false p0 p1 alpha0 dAlpha ellipse
        else
            None
            
    /// creates an arc using the an angle alpha0 and a (signed) dAlpha.
    /// in order to avoid precision issues p0,p1 users may redundantly supply p0 and p1.
    /// the implementation validates that p0~ellipse.GetPoint(alpha0) and p1~ellipse.GetPoint(alpha0+dAlpha) respectively.
    let arcWithPoints (p0 : V2d) (p1 : V2d) (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        if Fun.ApproximateEquals(p0, ellipse.GetPoint alpha0, epsilon) && Fun.ApproximateEquals(p1, ellipse.GetPoint (alpha0 + dAlpha), epsilon) then
            createArc true p0 p1 alpha0 dAlpha ellipse |> Option.get
        else
            failwith "bad points for arc"

    /// creates an arc using the an angle alpha0 and a (signed) dAlpha
    let tryArc (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        if Fun.IsTiny(dAlpha, epsilon) then
            None
        else
            let p0 = ellipse.GetPoint alpha0
            let p1 = ellipse.GetPoint (alpha0 + dAlpha)
            createArc false p0 p1 alpha0 dAlpha ellipse
            
    /// creates an arc using the an angle alpha0 and a (signed) dAlpha
    let arc (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        tryArc alpha0 dAlpha ellipse |> Option.get

    /// creates an arc-segment passing through p0 and p2. 
    /// the tangents are made parallel to (p1-p0) and (p2-p1) respectively.
    /// since this configuration is ambigous the ellipse with minimal eccentricity is chosen.
    let tryArcSegment (p0 : V2d) (p1 : V2d) (p2 : V2d) =
        // check if the arc is actually a line
        let d = p2 - p0 |> Vec.normalize
        let n = V2d(-d.Y, d.X)
        
        if Fun.IsTiny(Vec.dot (p1 - p0) n, epsilon) then 
            tryLine p0 p2
        else 
            // see: https://math.stackexchange.com/questions/2204258/roundest-ellipse-with-specified-tangents
            let m = 0.5 * (p0 + p2)
            let am = m - p0
            let vm = m - p1
            let va = p1 - p0

            let lam = Vec.length am
            let lvm = Vec.length vm
            let lva = Vec.length va

            let lam2 = lam * lam
            let lmd = (lam2 + lam*sqrt (lam2 + lvm*lvm)) / lvm
            let d = m + vm * (lmd / lvm)

            let inline ln (v : V2d) = V2d(-v.Y, v.X)
            
            let pd = Plane2d(ln (am / lam), d)
            let pv = Plane2d(ln (va / lva), p0)
            let mutable e = V2d.Zero
            if pd.Intersects(pv, &e) then
                let f = 0.5 * (d + p0)
                let ef = f - e
                let lef = Vec.length ef
                let pef = Plane2d(ln (ef / lef), e)
                let pvm = Plane2d(ln (vm / lvm), d)

                let mutable c = V2d.Zero
                if pef.Intersects(pvm, &c) then
                    let lcd = Vec.length (d - c)
                    let g = c - am * (lcd/lam)
                    
                    let ellipse = createEllipse c (d - c) (g - c)
                    let a0 = ellipse.GetAlpha p0
                    let a1 = ellipse.GetAlpha p2
                    let da = a1 - a0
                    let da1 = if da > Constant.Pi then da - Constant.PiTimesTwo elif da < -Constant.Pi then Constant.PiTimesTwo + da else da

                    ArcSeg(p0, p2, a0, da1, ellipse) |> Some

                else
                    tryLine p0 p2

            else
                tryLine p0 p2
         
    /// creates an arc-segment passing through p0 and p2. 
    /// the tangents are made parallel to (p1-p0) and (p2-p1) respectively.
    /// since this configuration is ambigous the ellipse with minimal eccentricity is chosen.
    let arcSegment (p0 : V2d) (p1 : V2d) (p2 : V2d) =
        tryArcSegment p0 p1 p2 |> Option.get


    /// returns the start-point of the segment (t=0)
    let startPoint (seg : PathSegment) =
        match seg with
        | LineSeg(p0,_) -> p0
        | Bezier2Seg(p0,_,_) -> p0
        | Bezier3Seg(p0,_,_,_) -> p0
        | ArcSeg(p0,_,_,_,_) -> p0
        
    /// returns the end-point of the segment (t=1)
    let endPoint (seg : PathSegment) =
        match seg with
        | LineSeg(_,p1) -> p1
        | Bezier2Seg(_,_,p1) -> p1
        | Bezier3Seg(_,_,_,p1) -> p1
        | ArcSeg(_,p1,_,_,_) -> p1

    /// evaluates the curve-position for the given parameter (t <- [0;1])
    let point (t : float) (seg : PathSegment) =
        if t <= 0.0 then startPoint seg
        elif t >= 1.0 then endPoint seg
        else
            match seg with
            | LineSeg(p0, p1) -> 
                lerp p0 p1 t

            | Bezier2Seg(p0, p1, p2) ->
                let u = 1.0 - t
                (u * u) * p0 + (2.0 * t * u) * p1 + (t * t) * p2

            | Bezier3Seg(p0, p1, p2, p3) ->
                let u = 1.0 - t
                let u2 = u * u
                let t2 = t * t
                (u * u2) * p0 + (3.0 * u2 * t) * p1 + (3.0 * u * t2) * p2 + (t * t2) * p3

            | ArcSeg(p0, p1, a0, da, ellipse) ->
                if t <= 0.0 then p0
                elif t >= 1.0 then p1
                else ellipse.GetPoint(a0 + t * da)
 
    /// evaluates the curve-derivative for the given parameter (t <- [0;1])
    let derivative (t : float) (seg : PathSegment) =
        let t = clamp 0.0 1.0 t
        match seg with
        | LineSeg(p0, p1) ->
            p1 - p0

        | ArcSeg(_p0, _p1, a0, da, e) ->
            let alpha = a0 + t * da
            (cos alpha * e.Axis1 - sin alpha * e.Axis0) * da

        | Bezier2Seg(p0, p1, p2) ->
            let u = 1.0 - t
            2.0 * (u*(p1 - p0) + t*(p2 - p1))

        | Bezier3Seg(p0, p1, p2, p3) ->
            let s = 1.0 - t
            let u = p1 - p0
            let v = p2 - p1
            let w = p3 - p2
            3.0 * (s*s*u + 2.0*t*s*v + t*t*w)
            
    /// evaluates the second curve-derivative for the given parameter (t <- [0;1])
    let secondDerivative (t : float) (seg : PathSegment) =
        let t = clamp 0.0 1.0 t
        match seg with
        | LineSeg _ ->
            V2d.Zero

        | ArcSeg(p0, p1, a0, da, e) ->
            let alpha = a0 + t * da
            -sqr da * (cos alpha * e.Axis0 + sin alpha * e.Axis1)

        | Bezier2Seg(p0, p1, p2) ->
            2.0 * (p0 - 2.0*p1 + p2)

        | Bezier3Seg(p0, p1, p2, p3) ->
            let u = p1 - p0
            let v = p2 - p1
            let w = p3 - p2
            6.0 * ((1.0-t)*(v-u) + t*(w-v))
         
    /// evaluates the third curve-derivative for the given parameter (t <- [0;1])
    let thirdDerivative (t : float) (seg : PathSegment) =
        let t = clamp 0.0 1.0 t
        match seg with
        | LineSeg _ ->
            V2d.Zero

        | ArcSeg(p0, p1, a0, da, e) ->
            let alpha = a0 + t * da
            da*da*da * (sin alpha * e.Axis0 - cos alpha * e.Axis1)

        | Bezier2Seg(p0, p1, p2) ->
            V2d.Zero

        | Bezier3Seg(p0, p1, p2, p3) ->
            let u = p1 - p0
            let v = p2 - p1
            let w = p3 - p2
            6.0 * (u+w - 2.0*v)
            
    /// evaluates the curvature for the given parameter (t <- [0;1]).
    /// the curvature is defined as: c(t) := (x'(t)*y''(t) - y'(t)*x''(t)) / (x'(t)^2 + y'(t)^2)^(3/2).
    /// see https://en.wikipedia.org/wiki/Curvature#In_terms_of_a_general_parametrization
    let curvature (t : float) (seg : PathSegment) =
        match seg with
        | LineSeg _ -> 
            0.0

        | _ ->
            
            
            //(df.X*ddf.Y - df.Y*ddf.X) / (df.LengthSquared ** 1.5)
            
            
            // f = t^2 * (p0 - 2*p1 + p2) + t*2*(p1 - p0) + p0
            
            
            // df = 2.0 * ((1-t)*(p1 - p0) + t*(p2 - p1))
            // ddf = 2.0 * (p0 - 2.0*p1 + p2)
            
            
            // df/2 =(p1 - p0)- t*(p1 - p0) + t*(p2 - p1)
            
            
            // df/2 = (p1 - p0) - t*(p1 - p0) + t*(p2 - p1)
            // df = t * 2*(p2 - 2*p1 + p0) + 2*(p1 - p0)
            // ddf = 2*(p2 - 2*p1 + p0)
            
            
            // c = (df.X*ddf.Y - df.Y*ddf.X) / (df.LengthSquared ** 1.5)
            // c^2 = max
            
            // c^2 = (df.X*ddf.Y - df.Y*ddf.X)^2 / (df.LengthSquared ** 3)
            
            // a := 2*(p2 - 2*p1 + p0)
            // b := 2*(p1 - p0)
            
            
            // df = t*a + b
            // ddf = a
            
            // ((t*a.X + b.X)*a.Y - (t*a.Y + b.Y)*a.X)^2 / ((t*a + b)^2)^3
            
            // (t*a.X*a.Y + b.X*a.Y - t*a.Y*a.X - b.Y*a.X)^2 / ((t*a + b)^2)^3
            // c^2 = (b.X*a.Y - b.Y*a.X)^2 * ((t*a + b)^2)^-3
            
            // dc^2 / dt = -3 * (b.X*a.Y - b.Y*a.X)^2 * ((t*a + b)^2)^-4 * 2*(t*a + b) * a
            
            
            // (t*a + b) * a  = 0
            // t = -<b|a> / <a|a>
            
            
            
            // d := t0
            // a := 2.0*(p0 - 2*p1 + p2)
            
            // df = a*t + d
            // ddf = a
            
            // c^2 = [(a.X*t+d.X)*a.Y - (a.Y*t+d.Y)*a.X]^2 * [(a*t+d)^2]^-3
            // c^2 = [t*(a.X*a.Y - a.Y*a.X) + (d.X*a.Y - d.Y*a.X)]^2 * [(a*t+d)^2]^-3
            
            // f := a.X*a.Y - a.Y*a.X
            // g := d.X*a.Y - d.Y*a.X
            
            // f = 0
            
            // c^2 = [t*f + g]^2 * [(a*t+d)^2]^-3
            
            
            // c^2 = g^2 * [(a*t+d)^2]^-3
            
            // dc^2 / dt = -3*g^2 * [(a*t+d)^2]^-4
            
            
            // dc^2 / dt = 2*[t*f + g]*f * [(a*t+d)^2]^-3 - 6*[t*f + g]^2 * (a*t+d) * [(a*t+d)^2]^-4
            
            // [t*f + g]*f * [(a*t+d)^2]^-3 - 3*[t*f + g]^2 * (a*t+d) * [(a*t+d)^2]^-4 = 0 /// * [(a*t+d)^2]^4
            
            // [t*f + g]*f * [(a*t+d)^2] - 3*[t*f + g]^2 * (a*t+d) = 0 
            
            
            
            
            
            
            
            let df = derivative t seg
            let ddf = secondDerivative t seg
            (df.X*ddf.Y - df.Y*ddf.X) / (df.LengthSquared ** 1.5)
          
    /// evaluates the curvature-derivative for the given parameter (t <- [0;1]).
    let curvatureDerivative (t : float) (seg : PathSegment) =
        match seg with
        | LineSeg _ -> 
            0.0

        | _ ->
            
            // F = df/dt(t)
            // G = d2f/dt^2(t)
            
            // (F.X*G.Y - F.Y*G.X) * (F.X*G.X + F.Y*G.Y)
            
            // sqr F.X*G.X*G.Y - sqr F.Y*G.X*G.Y - F.X*F.Y*sqr G.X + F.X*F.Y*sqr G.Y
            
            // G.X*G.Y*(sqr F.X - sqr F.Y) + F.X*F.Y*(sqr G.Y - sqr G.X)
            
            // (Fx*Gy - Fy*Gx) * (Fx^2 + Fy^2)^(2/3)
            
            // (Gx*Gy + Fx*Hy - Gy*Gx - Fy*Hx) * (Fx^2 + Fy^2)^(2/3) +
            // (Fx*Gy - Fy*Gx) * (4/3) * (Fx^2 + Fy^2)^(-1/3) * (Fx*Gx + Fy*Gy)
            
            
            // (Fx*Hy - Fy*Hx) * (Fx^2 + Fy^2)^(2/3) +
            // (4/3) * (Fx*Gy - Fy*Gx) * (Fx*Gx + Fy*Gy) * (Fx^2 + Fy^2)^(-1/3)
            
            
            // (Fx*Hy - Fy*Hx) * (Fx^2 + Fy^2)^(2/3) + (Fx*Gy - Fy*Gx) * (2/3) * (Fx^2 + Fy^2)^(-1/3) * 2*(Fx*Gx + Fy*Gy)
            
            let F = derivative t seg
            let G = secondDerivative t seg
            let H = thirdDerivative t seg
            let lfSqCbrt = Fun.Cbrt F.LengthSquared
            
            (F.X*H.Y - F.Y*H.X) * sqr lfSqCbrt +
            (4.0/3.0) * (F.X*G.Y - F.Y*G.X) * (F.X*G.X + F.Y*G.Y) / lfSqCbrt


    /// finds all parameters t <- [0;1] where the curvature changes its sign.
    /// note that only Bezier3 segments may have inflection points.
    let inflectionParameters (seg : PathSegment) =
        // {t | t >= 0 && t <= 1 && f'(t).X*f''(t).Y - f'(t).Y*f''(t).X == 0 }
        match seg with
        | LineSeg _ 
        | ArcSeg _ 
        | Bezier2Seg _ ->
            []

        | Bezier3Seg(p0, p1, p2, p3) ->
            let u = p1 - p0
            let v = p2 - p1
            let w = p3 - p2

            let f0 = -18.0*u.Y*v.X + 18.0*u.X*v.Y
            let f1 =  36.0*u.Y*v.X - 36.0*u.X*v.Y - 18.0*u.Y*w.X + 18.0*u.X*w.Y
            let f2 = -18.0*u.Y*v.X + 18.0*u.X*v.Y + 18.0*u.Y*w.X - 18.0*v.Y*w.X - 18.0*u.X*w.Y + 18.0*v.X*w.Y

            let struct(t0, t1) = Polynomial.RealRootsOf(f2, f1, f0)

            let v0 = t0 >= 0.0 && t0 <= 1.0
            let v1 = t1 >= 0.0 && t1 <= 1.0

            if v0 then
                if v1 then
                    if Fun.ApproximateEquals(t0, t1, epsilon) then [t0]
                    elif t1 > t0 then [t0; t1]
                    else [t1; t0]
                else
                    [t0]
            elif v1 then [t1]
            else []
      
    /// gets a normalized tangent at the given parameter (t <- [0;1])
    let tangent (t : float) (seg : PathSegment) =
        match seg with
        | LineSeg(p0, p1) ->
            p1 - p0 |> Vec.normalize

        | ArcSeg(p0, p1, a0, da, e) ->
            let t = clamp 0.0 1.0 t
            let alpha = a0 + t * da
            let n = (cos alpha * e.Axis1 - sin alpha * e.Axis0) |> Vec.normalize
            if da > 0.0 then n else -n
            
        | Bezier2Seg(p0, p1, p2) ->
            if t <= 0.0 then 
                Vec.normalize (p1 - p0)
            elif t >= 1.0 then 
                Vec.normalize (p2 - p1)
            else
                let u = 1.0 - t
                Vec.normalize (u*(p1 - p0) + t*(p2 - p1))

        | Bezier3Seg(p0, p1, p2, p3) ->
            if t <= 0.0 then
                if Fun.ApproximateEquals(p0, p1, 1E-9) then Vec.normalize (p2 - p0)
                else Vec.normalize (p1 - p0)
            elif t >= 1.0 then
                if Fun.ApproximateEquals(p2, p3, 1E-9) then Vec.normalize (p3 - p1)
                else Vec.normalize (p3 - p2)
            else
                let s = 1.0 - t
                let u = p1 - p0
                let v = p2 - p1
                let w = p3 - p2
                Vec.normalize (s*s*u + 2.0*t*s*v + t*t*w)
        
    /// gets a normalized (left) normal at the given parameter (t <- [0;1])
    let inline normal (t : float) (seg : PathSegment) =
        let t = tangent t seg
        V2d(-t.Y, t.X)

    /// gets an axis aligned bounding box for the given segment
    let bounds (seg : PathSegment) = 
        PathSegmentIntersections.bounds seg

    /// reverses the given segment s.t. reversed(t) = original(1-t) 
    let reverse (seg : PathSegment) =
        match seg with
        | LineSeg(p0, p1) -> LineSeg(p1, p0)
        | Bezier2Seg(p0, p1, p2) -> Bezier2Seg(p2, p1, p0)
        | Bezier3Seg(p0, p1, p2, p3) -> Bezier3Seg(p3, p2, p1, p0)
        | ArcSeg(p0, p1, a0, da, ellipse) -> ArcSeg(p1, p0, a0 + da, -da, ellipse)

    /// applies a transformation to all the points in the segment
    let transform (f : V2d -> V2d) (seg : PathSegment) =
        match seg with
        | LineSeg(p0, p1) -> line (f p0) (f p1)
        | Bezier2Seg(p0, p1, p2) -> bezier2 (f p0) (f p1) (f p2)
        | Bezier3Seg(p0, p1, p2, p3) -> bezier3 (f p0) (f p1) (f p2) (f p3)
        | ArcSeg(p0, p1, a0, da, ellipse) -> 
            let c = f ellipse.Center
            let ax0 = f (ellipse.Center + ellipse.Axis0) - c
            let ax1 = f (ellipse.Center + ellipse.Axis1) - c
            let e = Ellipse2d.FromConjugateDiameters(c, ax0, ax1)
            let q0 = f p0
            let q1 = f p1
            let da = 
                if not e.IsCcw then -da
                else da
            let a0 = e.GetAlpha q0
            arcWithPoints q0 q1 a0 da e

    /// tries to create a sub-segment from range [0;t1] and returns None whenever the result would be degenerate.
    let withT1 (t1 : float) (s : PathSegment) =
        if t1 <= 0.0 then
            None
        elif t1 >= 1.0 then
            Some s
        else
            match s with
            | LineSeg(p0,_) -> 
                tryLine p0 (point t1 s)

            | ArcSeg(p0, _, a0, da, e) ->
                let d = t1*da
                let p = e.GetPoint (a0 + d)
                ArcSeg(p0, p, a0, d, e) |> Some

            | Bezier2Seg(p0, p1, p2) ->
                let q0 = lerp p0 p1 t1
                let q1 = lerp p1 p2 t1
                let m = lerp q0 q1 t1
                tryBezier2 p0 q0 m

            | Bezier3Seg(p0, p1, p2, p3) ->
                let q0 = lerp p0 p1 t1
                let q1 = lerp p1 p2 t1
                let q2 = lerp p2 p3 t1
                let m0 = lerp q0 q1 t1
                let m1 = lerp q1 q2 t1
                let m = lerp m0 m1 t1
                tryBezier3 p0 q0 m0 m

    /// tries to create a sub-segment from range [t0;1] and returns None whenever the result would be degenerate.
    let withT0 (t0 : float) (s : PathSegment) =
        if t0 >= 1.0 then
            None
        elif t0 <= 0.0 then
            Some s
        else
            match s with
            | LineSeg(_,p1) -> 
                tryLine (point t0 s) p1

            | ArcSeg(p0, p1, a0, da, e) ->
                let d = t0 * da
                let p = e.GetPoint (a0 + d)
                ArcSeg(p, p1, a0 + d, da - d, e) |> Some

            | Bezier2Seg(p0, p1, p2) ->
                let q0 = lerp p0 p1 t0
                let q1 = lerp p1 p2 t0
                let m = lerp q0 q1 t0
                tryBezier2 m q1 p2

            | Bezier3Seg(p0, p1, p2, p3) ->
                let q0 = lerp p0 p1 t0
                let q1 = lerp p1 p2 t0
                let q2 = lerp p2 p3 t0
                let m0 = lerp q0 q1 t0
                let m1 = lerp q1 q2 t0
                let m = lerp m0 m1 t0
                tryBezier3 m m1 q2 p3

    /// tries to create a sub-segment from range [t0;t1] and returns None whenever the result would be degenerate.
    let withRange (t0 : float) (t1 : float) (s : PathSegment) =
        if t0 <= 0.0 && t1 >= 1.0 then
            Some s
        elif t0 <= 0.0 then
            withT1 t1 s
        elif t1 >= 1.0 then
            withT0 t0 s
        elif t0 < t1 then
            match s with
            | LineSeg(_,_) -> 
                let p0 = point t0 s
                let p1 = point t1 s
                tryLine p0 p1

            | ArcSeg(p0, p1, a0, da, e) ->
                let p0 = a0 + t0 * da
                let p1 = a0 + t1 * da
                let d = p1 - p0
                tryArc p0 d e

            | Bezier2Seg _ ->
                let p0 = point t0 s
                let p1 = point t1 s
                let d0 = tangent t0 s
                let d1 = tangent t1 s

                let r0 = Ray2d(p0, d0)
                let r1 = Ray2d(p1, -d1)
                
                let mutable t = 0.0
                if r0.Intersects(r1, &t) then
                    let c = r0.GetPointOnRay t
                    tryBezier2 p0 c p1
                else
                    tryLine p0 p1

            | Bezier3Seg _ ->
                
                match withT0 t0 s with
                | Some r ->
                    withT1 ((t1 - t0) / (1.0 - t0)) r
                | None ->
                    None
        else
            None
        
      
    /// tries to split the segment into two new segments having ranges [0;t] and [t;1] respectively.
    let splitWithCenterPoint (t : float) (pt : V2d) (s : PathSegment) =
        if t <= 0.0 then (None, Some s)
        elif t >= 1.0 then (Some s, None)
        else
            match s with
            | LineSeg(p0, p1) ->
                tryLine p0 pt, tryLine pt p1

            | ArcSeg(p0, p1, a0, da, e) ->

                let l0 = a0
                let ld = da * t

                let r0 = a0 + ld
                let rd = da - ld

                let l = ArcSeg(p0, pt, l0, ld, e)
                let r = ArcSeg(pt, p1, r0, rd, e)

                Some l, Some r

            | Bezier2Seg(p0, p1, p2) ->
                let q0 = lerp p0 p1 t
                let q1 = lerp p1 p2 t
                tryBezier2 p0 q0 pt, tryBezier2 pt q1 p2

            | Bezier3Seg(p0, p1, p2, p3) ->
                let q0 = lerp p0 p1 t
                let q1 = lerp p1 p2 t
                let q2 = lerp p2 p3 t
                let m0 = lerp q0 q1 t
                let m1 = lerp q1 q2 t
                tryBezier3 p0 q0 m0 pt, tryBezier3 pt m1 q2 p3

    /// tries to split the segment into two new segments having ranges [0;t] and [t;1] respectively.
    let split (t : float) (s : PathSegment) =
        let pt = point t s
        splitWithCenterPoint t pt s
        
    /// tries to get the t-parameter for a point very near the segment (within eps)
    let tryGetT (eps : float) (pt : V2d) (segment : PathSegment) =
        
        let inline newtonImprove (t : float) =
            let mutable t = t
            let mutable dt = 1000.0
            let mutable iter = 10
            while iter > 0 && not (Fun.IsTiny (dt, 1E-15)) do
                // err^2 = min
                // 2*err * err' = 0
                
                // f = 2*err * err'
                // f' = 2*(err'^2 + err*err'')
                
                let err = point t segment - pt
                let d = derivative t segment
                let dd = secondDerivative t segment
                
                //let v = Vec.lengthSquared err
                let dv = 2.0 * Vec.dot err d
                let ddv = 2.0 * (Vec.dot d d + Vec.dot err dd)
                
                
                dt <- -dv / ddv
                //printfn "%d: %A %A -> %A" iter t dv dt
                t <- t + dt
                iter <- iter - 1
                
            let t = clamp 0.0 1.0 t
            let err = point t segment - pt
            if Fun.IsTiny(err, eps) then Some t
            else None
        
        match segment with
        | LineSeg(p0, p1) ->
            if Fun.ApproximateEquals(p0, pt, eps) then Some 0.0
            elif Fun.ApproximateEquals(p1, pt, eps) then Some 1.0
            else
                let u = p1 - p0
                let len = Vec.length u
                let n = V2d(-u.Y, u.X) / len
                let d = Vec.dot (pt - p0) n
                if abs d < eps then
                    let t = Vec.dot (pt - p0) u / sqr len
                    if t >= 0.0 && t <= 1.0 then Some t
                    else None
                else
                    None
        | Bezier2Seg(p0, p1, p2) ->
            if Fun.ApproximateEquals(p0, pt, eps) then Some 0.0
            elif Fun.ApproximateEquals(p2, pt, eps) then Some 1.0
            else
                let box = Box2d([p0; p1; p2]).EnlargedBy eps
                
                if box.Contains pt then
                    let a = p2 + p0 - 2.0*p1
                    let b = 2.0*(p1 - p0)
                    let c = p0 - pt
                    
                    let struct(t0, t1) = 
                        if box.Size.MajorDim = 0 then Polynomial.RealRootsOf(a.X, b.X, c.X)
                        else Polynomial.RealRootsOf(a.Y, b.Y, c.Y)
                        
                    let d0 = if t0 >= 0.0 && t0 <= 1.0 then Vec.length (a * sqr t0 + b * t0 + c) else System.Double.PositiveInfinity
                    let d1 = if t1 >= 0.0 && t1 <= 1.0 then Vec.length (a * sqr t1 + b * t1 + c) else System.Double.PositiveInfinity
                    
                    if d0 < d1 then
                        newtonImprove t0
                    else
                        if not (System.Double.IsPositiveInfinity d1) then newtonImprove t1
                        else None
                else
                    None
            
            //t^2 * (p2 + p0 - 2*p1) + t * 2*(p1 - p0) + p0
            
        | Bezier3Seg(p0, p1, p2, p3) ->
            if Fun.ApproximateEquals(p0, pt, eps) then Some 0.0
            elif Fun.ApproximateEquals(p3, pt, eps) then Some 1.0
            else
                let box = Box2d([p0; p1; p2; p3]).EnlargedBy eps
                
                if box.Contains pt then
                    let a = -p0 + 3.0*p1 - 3.0*p2 + p3
                    let b = 3.0*p0 - 6.0*p1 + 3.0*p2
                    let c = 3.0*p1 - 3.0*p0
                    let d = p0 - pt
                    
                    let struct(t0, t1, t2) = 
                        if box.Size.MajorDim = 0 then Polynomial.RealRootsOf(a.X, b.X, c.X, d.X)
                        else Polynomial.RealRootsOf(a.Y, b.Y, c.Y, d.Y)
                        
                    let d0 = if t0 >= 0.0 && t0 <= 1.0 then Vec.length (a * sqr t0 + b * t0 + c) else System.Double.PositiveInfinity
                    let d1 = if t1 >= 0.0 && t1 <= 1.0 then Vec.length (a * sqr t1 + b * t1 + c) else System.Double.PositiveInfinity
                    let d2 = if t2 >= 0.0 && t2 <= 1.0 then Vec.length (a * sqr t2 + b * t2 + c) else System.Double.PositiveInfinity
                    
                    
                    if d0 < d1 then
                        if d0 < d2 then
                            newtonImprove t0
                        else
                            if not (System.Double.IsPositiveInfinity d2) then newtonImprove t2
                            else None
                    else
                        if d1 < d2 then
                            newtonImprove t1
                        else
                            if not (System.Double.IsPositiveInfinity d2) then newtonImprove t2
                            else None
                    
                    
                else
                    None
        | ArcSeg(p0, p1, a, da, e) ->
            if Fun.ApproximateEquals(p0, pt, eps) then Some 0.0
            elif Fun.ApproximateEquals(p1, pt, eps) then Some 1.0
            else
                let m = M33d.FromCols(V3d(e.Axis0, 0.0), V3d(e.Axis1, 0.0), V3d(e.Center, 1.0))
                
                let c = m.Inverse.TransformPos(pt) |> Vec.normalize
                let angle = atan2 c.Y c.X
                
                let t = arcT a da angle
                
                if Fun.ApproximateEquals(m.TransformPos c, pt, eps) then
                    newtonImprove t
                else
                    None    
       
    /// tries to merge two segments together resulting in an optional new segment (or None if the segments cannot be merged)
    let tryMerge (eps : float) (a : PathSegment) (b : PathSegment) =
        match a with
        | LineSeg(a0, a1) ->
            match b with
            | LineSeg(b0, b1) when Fun.ApproximateEquals(a1, b0, eps) ->
                if Fun.ApproximateEquals(a0, b1) then
                    Some None
                else
                    let u = Vec.normalize (b1 - a0)
                    let n = V2d(-u.Y, u.X)
                    if Fun.IsTiny (Vec.dot n (b0 - a0), eps) then
                        Some (tryLine a0 b1)
                    else
                        None
            | _ ->
                None
        | Bezier2Seg(a0, a1, a2) ->
            match b with
            | Bezier2Seg(b0, b1, b2) when Fun.ApproximateEquals(a2, b0, eps) ->
                if Fun.ApproximateEquals(a0, b2, eps) then
                    if Fun.ApproximateEquals(a1, b1, eps) then Some None
                    else None
                else
                    let ta1 = a2 - a1 |> Vec.normalize
                    let tb0 = b1 - b0 |> Vec.normalize
                    
                    if Fun.ApproximateEquals(ta1, tb0, 1E-9) then
                        let ra = Ray2d(a0, a1 - a0)
                        let rb = Ray2d(b2, b2 - b1)
                        let mutable t = 0.0
                        if ra.Intersects(rb, &t) then
                            let c = ra.GetPointOnRay t
                            match tryBezier2 a0 c b2 with
                            | Some res ->
                                match tryGetT eps a2 res with
                                | Some _ -> Some (Some res)
                                | None -> None
                            | None ->
                                None
                        else
                            None
                    elif Fun.ApproximateEquals(ta1, -tb0, 1E-9) then
                        match tryGetT eps b2 a with
                        | Some _ ->
                            let ra = Ray2d(a0, a1 - a0)
                            let rb = Ray2d(b2, b2 - b1)
                            let mutable t = 0.0
                            if ra.Intersects(rb, &t) then
                                let c = ra.GetPointOnRay t
                                match tryBezier2 a0 c b2 with
                                | Some res ->
                                    Some (Some res)
                                | None ->
                                    None
                            else
                                None
                        | None ->
                            None
                    else
                        None
            | _ ->
                None
        | _ ->
            // TODO: proper merge for Arc/Bezier3
            None
     
     
    let private overlapTs = [| 0.25; 0.5; 0.75; 0.333 |]
     
    /// tries to find the overlapping t-ranges for two segments. The first range will be sorted whereas the second one may not be.
    let overlap (eps : float) (a : PathSegment) (b : PathSegment) =
        
        let isOnLine (p0 : V2d) (p1 : V2d) (p : V2d) =
            let u = p1 - p0
            let len = Vec.length u
            let n = V2d(-u.Y, u.X) / len
            let d = Vec.dot (p - p0) n
            if abs d < eps then
                let t = Vec.dot (p - p0) u / sqr len
                if t >= 0.0 && t <= 1.0 then
                    let test = lerp p0 p1 t
                    if not (Fun.ApproximateEquals(test, p, eps)) then printfn "asdsadsad"
                    Some t
                else
                    None
            else
                None
        
        
        let inline checkRange ((aRange : Range1d, bRange : V2d) as tup) =
            let isOverlapping = 
                overlapTs |> Array.forall (fun t ->
                    let ta = lerp aRange.Min aRange.Max t
                    let tb = lerp bRange.X bRange.Y t
                    Fun.ApproximateEquals(point ta a, point tb b)
                )
            if isOverlapping then
                Some tup
            else
                None
        
        
        let a0 = startPoint a
        let a1 = endPoint a
        
        let b0 = startPoint b
        let b1 = endPoint b

        match tryGetT eps b0 a with
        | Some tab0 ->
            match tryGetT eps b1 a with
            | Some tab1 ->
                // b is entirely on a
                if tab0 < tab1 then
                    checkRange (Range1d(tab0, tab1), V2d(0.0, 1.0))
                else
                    checkRange (Range1d(tab1, tab0), V2d(1.0, 0.0))
            | None ->
                match tryGetT eps a0 b with
                | Some tba0 ->
                    checkRange (Range1d(0.0, tab0), V2d(tba0, 0.0))
                | None ->
                    match tryGetT eps a1 b with
                    | Some tba1 ->
                        checkRange (Range1d(tab0, 1.0), V2d(0.0, tba1))
                    | None ->
                        None
        | None -> 
            match tryGetT eps b1 a with
            | Some tab1 ->
                match tryGetT eps a0 b with
                | Some tba0 ->
                    checkRange (Range1d(0.0, tab1), V2d(tba0, 1.0))
                | None ->
                    match tryGetT eps a1 b with
                    | Some tba1 ->
                        checkRange (Range1d(tab1, 1.0), V2d(1.0, tba1))
                    | None ->
                        None
            | None ->
                match tryGetT eps a0 b with
                | Some tba0 ->
                    match tryGetT eps a1 b with
                    | Some tba1 ->
                        checkRange (Range1d(0.0, 1.0), V2d(tba0, tba1))
                    | None ->
                        None
                | None ->
                    None
           
    /// splits a PathSegment given two ordered ts into possibly three parts (the range must not be empty or invalid)
    let splitRange (r : Range1d) (seg : PathSegment) : option<PathSegment> * PathSegment * option<PathSegment> =
        let l, rest = split r.Min seg
        let m, r = split ((r.Max - r.Min) / (1.0 - r.Min)) (Option.get rest)
        l, Option.get m, r 
             
        
    /// splits the segment at the given parameter values and returns the list of resulting segments.
    let splitMany (ts : list<float>) (s : PathSegment) =
        match ts with
        | [] -> [s]
        | _ ->
            let rec run (ts : list<float>) (s : PathSegment) =
                match ts with
                | [] -> [s]
                | t0 :: ts ->
                    if t0 <= 0.0 then run ts s
                    elif t0 >= 1.0 then [s]
                    else
                        let ts = ts |> List.map (fun t -> (t-t0)/(1.0-t0))
                        let l, r = split t0 s
                        let r = 
                            match r with
                            | Some r -> run ts r
                            | None -> []
                        match l with
                        | Some l -> l :: r
                        | None -> r
            let res = run (List.sort ts) s
            res

    /// gets all intersection parameter pairs for the two given segments sorted by the first parameter.
    let intersections (eps : float) (l : PathSegment) (r : PathSegment) =
        PathSegmentIntersections.intersections eps l r



[<AutoOpen>]
module ``PathSegment Public API`` =

    type PathSegment with
        member x.GetSlice(t0 : option<float>, t1 : option<float>) =
            match t0 with
            | Some t0 ->
                match t1 with
                | Some t1 -> PathSegment.withRange t0 t1 x
                | None -> PathSegment.withT0 t0 x
            | None ->
                match t1 with
                | Some t1 -> PathSegment.withT1 t1 x
                | None -> Some x
        
        member x.Item
            with inline get(t : float) = PathSegment.point t x

        


    let (|Line|Bezier2|Bezier3|Arc|) (s : PathSegment) =
        match s with
        | LineSeg(p0, p1) -> Line(p0, p1)
        | Bezier2Seg(p0, p1, p2) -> Bezier2(p0, p1, p2)
        | Bezier3Seg(p0, p1, p2, p3) -> Bezier3(p0, p1, p2, p3)
        | ArcSeg(alpha0, alpha1, a0, da, ellipse) -> Arc(alpha0, alpha1, a0, da, ellipse)

    type PathSegment with
        static member inline Line(p0, p1) = PathSegment.line p0 p1
        static member inline Bezier2(p0, p1, p2) = PathSegment.bezier2 p0 p1 p2
        static member inline Bezier3(p0, p1, p2, p3) = PathSegment.bezier3 p0 p1 p2 p3
        static member inline Arc(a0, da, p3) = PathSegment.arc a0 da p3

        static member ForceLine(p0, p1) = LineSeg(p0, p1)
        static member ForceBezier2(p0, p1, p2) = Bezier2Seg(p0, p1, p2)
        static member ForceBezier3(p0, p1, p2, p3) = Bezier3Seg(p0, p1, p2, p3)
        static member ForceArc(p0, p1, a0, da, e) = ArcSeg(p0, p1, a0, da, e)

    
    module PathSegment =
        let (|Line|Bezier2|Bezier3|Arc|) (s : PathSegment) =
            match s with
            | LineSeg(p0, p1) -> Line(p0, p1)
            | Bezier2Seg(p0, p1, p2) -> Bezier2(p0, p1, p2)
            | Bezier3Seg(p0, p1, p2, p3) -> Bezier3(p0, p1, p2, p3)
            | ArcSeg(alpha0, alpha1, a0, da, ellipse) -> Arc(alpha0, alpha1, a0, da, ellipse)


    let inline Line(p0, p1) = PathSegment.line p0 p1
    let inline Bezier2(p0, p1, p2) = PathSegment.bezier2 p0 p1 p2
    let inline Bezier3(p0, p1, p2, p3) = PathSegment.bezier3 p0 p1 p2 p3
    let inline Arc(a0, da, p3) = PathSegment.arc a0 da p3

