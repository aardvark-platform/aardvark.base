namespace Aardvark.Rendering.Text


open Aardvark.Base

type PathSegment =
    private
    | LineSeg of p0 : V2d * p1 : V2d
    | Bezier2Seg of p0 : V2d * p1 : V2d * p2 : V2d
    | Bezier3Seg of p0 : V2d * p1 : V2d * p2 : V2d * p3 : V2d


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PathSegment =
    /// creates a line segment
    let line (p0 : V2d) (p1 : V2d) = 
        if V2d.ApproxEqual(p0, p1) then
            failwithf "[PathSegment] degenerate line at: %A" p0

        LineSeg(p0, p1)

    /// creates a quadratic bezier segment
    let bezier2 (p0 : V2d) (p1 : V2d) (p2 : V2d) = 
        // check if the spline is actually a line
        if Fun.IsTiny(p1.PosLeftOfLineValue(p0, p2)) then 
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

        if Fun.IsTiny(areaBetween, 1.0E-6) then
            let pc = (3.0*(p1 + p2) - p0 - p3)/4.0
            Bezier2Seg(p0, pc, p3)
        else
            Bezier3Seg(p0, p1, p2, p3)

    /// evaluates the curve-position for the given parameter (t <- [0;1])
    let point (t : float) (seg : PathSegment) =
        let t = clamp 0.0 1.0 t

        match seg with
            | LineSeg(p0, p1) -> 
                (1.0 - t) * p0 + (t) * p1

            | Bezier2Seg(p0, p1, p2) ->
                let u = 1.0 - t
                (u * u) * p0 + (2.0 * t * u) * p1 + (t * t) * p2

            | Bezier3Seg(p0, p1, p2, p3) ->
                let u = 1.0 - t
                let u2 = u * u
                let t2 = t * t
                (u * u2) * p0 + (3.0 * u2 * t) * p1 + (3.0 * u * t2) * p2 + (t * t2) * p2

    /// evaluates the curve-derivative for the given parameter (t <- [0;1])
    let derivative (t : float) (seg : PathSegment) =
        let t = clamp 0.0 1.0 t

        match seg with
            | LineSeg(p0, p1) -> 
                p1 - p0

            | Bezier2Seg(p0, p1, p2) ->
                let u = 1.0 - t
                (2.0 * u) * (p1 - p0) + (2.0 * t) * (p2 - p1)

            | Bezier3Seg(p0, p1, p2, p3) ->
                let u = 1.0 - t
                (3.0 * u * u) * (p1 - p0) + (6.0 * u * t) * (p2 - p1) + (3.0 * t * t) * (p3 - p2)

    /// evaluates the curvature for the given parameter (t <- [0;1])
    let curvature (t : float) (seg : PathSegment) =
        match seg with
            | LineSeg _ ->
                V2d.Zero

            | Bezier2Seg(p0, p1, p2) -> 
                2.0 * (p0 - 2.0*p1 + p2)

            | Bezier3Seg(p0, p1, p2, p3) ->
                let t = clamp 0.0 1.0 t
                let u = 1.0 - t
                (6.0 * u) * (p2 - 2.0*p1 + p0) + (6.0 * t) * (p3 - 2.0*p2 + p1)

    /// gets an axis aligned bounding box for the given segment
    let bounds (seg : PathSegment) =
        match seg with
            | LineSeg(p0, p1) -> Box2d [|p0; p1|]
            | Bezier2Seg(p0, p1, p2) -> Box2d [|p0; p1; p2|]
            | Bezier3Seg(p0, p1, p2, p3) -> Box2d [|p0; p1; p2; p3|]

    /// reverses the given segment ([p0;p1] -> [p1;p0])
    let reverse (seg : PathSegment) =
        match seg with
            | LineSeg(p0, p1) -> LineSeg(p1, p0)
            | Bezier2Seg(p0, p1, p2) -> Bezier2Seg(p2, p1, p0)
            | Bezier3Seg(p0, p1, p2, p3) -> Bezier3Seg(p3, p2, p1, p0)

    /// applies a transformation to all the points in the segment
    let transform (f : V2d -> V2d) (seg : PathSegment) =
        match seg with
            | LineSeg(p0, p1) -> line (f p0) (f p1)
            | Bezier2Seg(p0, p1, p2) -> bezier2 (f p0) (f p1) (f p2)
            | Bezier3Seg(p0, p1, p2, p3) -> bezier3 (f p0) (f p1) (f p2) (f p3)

    /// evaluates a normalized tangent at the given parameter (t <- [0;1])
    let inline tangent (t : float) (seg : PathSegment) =
        derivative t seg |> Vec.normalize
        
    /// evaluates a normalized (left) normal at the given parameter (t <- [0;1])
    let inline normal (t : float) (seg : PathSegment) =
        let t = tangent t seg
        V2d(-t.Y, t.X)

[<AutoOpen>]
module ``PathSegment Public API`` =
    let (|Line|Bezier2|Bezier3|) (s : PathSegment) =
        match s with
            | LineSeg(p0, p1) -> Line(p0, p1)
            | Bezier2Seg(p0, p1, p2) -> Bezier2(p0, p1, p2)
            | Bezier3Seg(p0, p1, p2, p3) -> Bezier3(p0, p1, p2, p3)

    type PathSegment with
        static member inline Line(p0, p1) = PathSegment.line p0 p1
        static member inline Bezier2(p0, p1, p2) = PathSegment.bezier2 p0 p1 p2
        static member inline Bezier3(p0, p1, p2, p3) = PathSegment.bezier3 p0 p1 p2 p3

    let inline Line(p0, p1) = PathSegment.line p0 p1
    let inline Bezier2(p0, p1, p2) = PathSegment.bezier2 p0 p1 p2
    let inline Bezier3(p0, p1, p2, p3) = PathSegment.bezier3 p0 p1 p2 p3

