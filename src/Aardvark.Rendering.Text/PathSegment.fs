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
            

    type Ellipse2d with

        member x.IsCcw =
            let n = V2d(-x.Axis0.Y, x.Axis0.X)
            Vec.dot n x.Axis1 > 0.0

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

            

    /// creates a line segment
    let line (p0 : V2d) (p1 : V2d) = 
        if Fun.ApproximateEquals(p0, p1, epsilon) then
            failwithf "[PathSegment] degenerate line at: %A" p0

        LineSeg(p0, p1)

    /// creates a quadratic bezier segment
    let bezier2 (p0 : V2d) (p1 : V2d) (p2 : V2d) = 
        // check if the spline is actually a line
        if Fun.IsTiny(p1.PosLeftOfLineValue(p0, p2), epsilon) then 
            line p0 p2
        else 
            Bezier2Seg(p0, p1, p2)

    /// creates a cubic bezier segment
    let bezier3 (p0 : V2d) (p1 : V2d) (p2 : V2d) (p3 : V2d) =
        // check if the cubic spline is actually quadratic
        // in order to do so let's start by getting the potential control-point by setting the difference
        // of both curves to zero:
        //   p0*(1-t)^3 + 3*p1*(1-t)^2*t + 3*p2*(1-t)*t^2 + p3*t^3 - p0*(1-t)^2 - 2*(1-t)*t*pc - p3*t^2 = 0

        // so let's insert "t = 0.5" :
        //   p0*0.5^3 + 3*p1*0.5^3 + 3*p2*0.5^3 + p3*0.5^3 - p0*0.5^2 - 2*0.5^2*pc - p3*0.5^2 = 0  
        //   p0*0.5^2 + 3*p1*0.5^2 + 3*p2*0.5^2 + p3*0.5^2 - p0*0.5 - 2*0.5*pc - p3*0.5 = 0  
        //   p0*(0.5^2 - 0.5) + p1*(3*0.5^2) + p2*(3*0.5^2) + p3*(0.5^2 - 0.5) - pc = 0
        //   pc = -p0/4 + p1*(3/4) + p2*(3/4) - p3/4


        // in order to determine the quality of the approximation we use the distance function:
        // distance(t) = cubic(t) - approximatedquadratic(t)
        //   distance(t) = p0*(1-t)^3 + 3*p1*(1-t)^2*t + 3*p2*(1-t)*t^2 + p3*t^3 - p0*(1-t)^2 - 2*t*(1-t)*(-p0/4 + p1*(3/4) + p2*(3/4) - p3/4) - p3*t^2 = 0
        // since distance might erase itself (negative values, etc.) we want to calculate the area between both curves and
        // therefore use the formula:
        //   F = integrate [ distance(t)^2; 0; 1] = 0      
        // which can be integrated using Wolframalpha and yields the simple forumla:
        // http://www.wolframalpha.com/input/?i=integrate+(p0*(1-t)%5E3+%2B+3*p1*(1-t)%5E2*t+%2B+3*p2*(1-t)*t%5E2+%2B+p3*t%5E3+-+p0*(1-t)%5E2+-+2*t*(1-t)*(-p0%2F4+%2B+p1*(3%2F4)+%2B+p2*(3%2F4)+-+p3%2F4)+-+p3*t%5E2)%5E2+from+0+to+1
        //   F = (1 / 840) * (p0 - 3*p1 + 3*p2 - p3)^2 
        // finally (in order to get the area) we need to compute the sqrt of F
        let areaBetween = 
            sqrt(
                let vec = p0 - 3.0*p1 + 3.0*p2 - p3 
                Vec.dot vec vec / 840.0
            )

        if Fun.IsTiny(areaBetween, epsilon) then
            let pc = (3.0*(p1 + p2) - p0 - p3)/4.0
            Bezier2Seg(p0, pc, p3)
        else
            Bezier3Seg(p0, p1, p2, p3)
            


    /// creates a line segment (if not degenerate)
    let tryLine (p0 : V2d) (p1 : V2d) =
        if Fun.ApproximateEquals(p0, p1, epsilon) then None
        else Some (LineSeg(p0, p1))

    /// creates a qudratic bezier segment (if not degenerate)
    let tryBezier2 (p0 : V2d) (p1 : V2d) (p2 : V2d) = 
        // check if the spline is actually a line
        if Fun.IsTiny(p1.PosLeftOfLineValue(p0, p2), epsilon) then 
            tryLine p0 p2
        else 
            Bezier2Seg(p0, p1, p2) |> Some

    /// creates a cubic bezier segment (if not degenerate)
    let tryBezier3 (p0 : V2d) (p1 : V2d) (p2 : V2d) (p3 : V2d) =
        let areaBetween = 
            sqrt(
                let vec = p0 - 3.0*p1 + 3.0*p2 - p3 
                Vec.dot vec vec / 840.0
            )

        if Fun.IsTiny(areaBetween, epsilon) then
            let pc = (3.0*(p1 + p2) - p0 - p3)/4.0
            tryBezier2 p0 pc p3
        else
            Bezier3Seg(p0, p1, p2, p3) |> Some


    let private createArc (p0 : V2d) (p1 : V2d) (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        let newEllipse = createEllipse ellipse.Center ellipse.Axis0 ellipse.Axis1
        let a0 = newEllipse.GetAlpha p0
        let da =
            if not ellipse.IsCcw then -dAlpha
            else dAlpha

        let pm = newEllipse.GetPoint(a0 + da / 2.0)
        if not (Fun.ApproximateEquals(p0, p1, epsilon)) && Fun.IsTiny(pm.PosLeftOfLineValue(p0, p1), epsilon) then
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
            createArc p0 p1 alpha0 dAlpha ellipse
        else
            None
    /// creates an arc using the an angle alpha0 and a (signed) dAlpha.
    /// in order to avoid precision issues p0,p1 users may redundantly supply p0 and p1.
    /// the implementation validates that p0~ellipse.GetPoint(alpha0) and p1~ellipse.GetPoint(alpha0+dAlpha) respectively.
    let arcWithPoints (p0 : V2d) (p1 : V2d) (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        tryArcWithPoints p0 p1 alpha0 dAlpha ellipse |> Option.get

    /// creates an arc using the an angle alpha0 and a (signed) dAlpha
    let tryArc (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        if Fun.IsTiny(dAlpha, epsilon) then
            None
        else
            let p0 = ellipse.GetPoint alpha0
            let p1 = ellipse.GetPoint (alpha0 + dAlpha)
            createArc p0 p1 alpha0 dAlpha ellipse
            
    /// creates an arc using the an angle alpha0 and a (signed) dAlpha
    let arc (alpha0 : float) (dAlpha : float) (ellipse : Ellipse2d) =
        tryArc alpha0 dAlpha ellipse |> Option.get

    /// creates an arc-segment passing through p0 and p2. 
    /// the tangents are made parallel to (p1-p0) and (p2-p1) respectively.
    /// since this configuration is ambigous the ellipse with minimal eccentricity is chosen.
    let tryArcSegment (p0 : V2d) (p1 : V2d) (p2 : V2d) =
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
            
    /// evaluates the curve-derivative for the given parameter (t <- [0;1])
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
            
    /// evaluates the curvature for the given parameter (t <- [0;1]).
    /// the curvature is defined as: c(t) := (x'(t)*y''(t) - y'(t)*x''(t)) / (x'(t)^2 + y'(t)^2)^(3/2).
    /// see https://en.wikipedia.org/wiki/Curvature#In_terms_of_a_general_parametrization
    let curvature (t : float) (seg : PathSegment) =
        match seg with
        | LineSeg _ -> 
            0.0

        | _ ->
            let df = derivative t seg
            let ddf = secondDerivative t seg
            (df.X*ddf.Y - df.Y*ddf.X) / (df.LengthSquared ** 0.75)

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
        | LineSeg(p1, p0) ->
            p1 - p0 |> Vec.normalize

        | ArcSeg(p0, p1, a0, da, e) ->
            let t = clamp 0.0 1.0 t
            let alpha = a0 + t * da
            (cos alpha * e.Axis1 - sin alpha * e.Axis0) |> Vec.normalize

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
                if Fun.ApproximateEquals(p0, p1, epsilon) then Vec.normalize (p2 - p0)
                else Vec.normalize (p1 - p0)
            elif t >= 1.0 then
                if Fun.ApproximateEquals(p2, p3, epsilon) then Vec.normalize (p3 - p1)
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
        match seg with
        | LineSeg(p0, p1) -> Box2d [|p0; p1|]
        | Bezier2Seg(p0, p1, p2) -> Box2d [|p0; p1; p2|]
        | Bezier3Seg(p0, p1, p2, p3) -> Box2d [|p0; p1; p2; p3|]
        | ArcSeg(p0, p1, a0, da, ellipse) -> 
            let steps = abs da / Constant.PiHalf |> round |> int
            let step = da / float steps
            let mutable bb = Box2d [|p0; p1|]
            let mutable last = a0
            for s in 1 .. steps do
                let alpha = last + step
                let pt = ellipse.GetControlPoint(last, alpha)
                bb.ExtendBy pt
                let p = ellipse.GetPoint alpha
                bb.ExtendBy p
                last <- alpha

            bb

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
            let e = Ellipse2d(c, ax0, ax1)
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
                // t=0 => d0 = 2.0 * (p1 - p0) => p1 = p0 + d0/2.0
                // t=1 => d1 = 2.0 * (p2 - p1) => p1 = p2 - d1/2.0
                let p0 = point t0 s
                let p1 = point t1 s
                let d0 = derivative t0 s
                let d1 = derivative t1 s

                let c0 = p0 + d0 / 2.0
                let c1 = p1 - d1 / 2.0
                if not (Fun.ApproximateEquals(c0, c1, epsilon)) then
                    failwithf "that was unexpected: %A vs %A" c0 c1

                tryBezier2 p0 c0 p1

            | Bezier3Seg _ ->
                let p0 = point t0 s
                let p1 = point t1 s
                let d0 = derivative t0 s
                let d1 = derivative t1 s

                //t=0 => d0 = 3.0 * (p1 - p0)
                //t=1 => d1 = 3.0 * (p3 - p2)
                // p1 = p0 + d0/3.0
                // p2 = p3 - d1/3.0

                let c0 = p0 + d0 / 3.0
                let c1 = p1 - d1 / 3.0
                tryBezier3 p0 c0 c1 p1
        else
            None

    /// tries to split the segment into two new segments having ranges [0;t] and [t;1] respectively.
    let split (t : float) (s : PathSegment) =
        if t <= 0.0 then (None, Some s)
        elif t >= 1.0 then (Some s, None)
        else
            match s with
            | LineSeg(p0, p1) ->
                let m = lerp p0 p1 t
                tryLine p0 m, tryLine m p1

            | ArcSeg(p0, p1, a0, da, e) ->
                let d = t * da
                let pm = e.GetPoint(a0 + d)

                let left =
                    match tryArc a0 d e with
                    | Some(ArcSeg(_,_,a0,da,e)) -> Some (ArcSeg(p0, pm, a0, da, e))
                    | Some(LineSeg _) -> Some (LineSeg(p0, pm))
                    | o -> o

                let right =
                    match tryArc (a0 + d) (da - d)  e with
                    | Some(ArcSeg(_,_,a0,da,e)) -> Some (ArcSeg(pm, p1, a0, da, e))
                    | Some(LineSeg _) -> Some (LineSeg(pm, p1))
                    | o -> o

                left, right


            | Bezier2Seg(p0, p1, p2) ->
                let q0 = lerp p0 p1 t
                let q1 = lerp p1 p2 t
                let c = lerp q0 q1 t
                tryBezier2 p0 q0 c, tryBezier2 c q1 p2

            | Bezier3Seg(p0, p1, p2, p3) ->
                let q0 = lerp p0 p1 t
                let q1 = lerp p1 p2 t
                let q2 = lerp p2 p3 t
                let m0 = lerp q0 q1 t
                let m1 = lerp q1 q2 t
                let c = lerp m0 m1 t
                tryBezier3 p0 q0 m0 c, tryBezier3 c m1 q2 p3



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

    let inline Line(p0, p1) = PathSegment.line p0 p1
    let inline Bezier2(p0, p1, p2) = PathSegment.bezier2 p0 p1 p2
    let inline Bezier3(p0, p1, p2, p3) = PathSegment.bezier3 p0 p1 p2 p3
    let inline Arc(a0, da, p3) = PathSegment.arc a0 da p3



