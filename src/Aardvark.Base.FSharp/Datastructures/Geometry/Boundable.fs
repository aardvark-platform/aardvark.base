namespace Aardvark.Base.Geometry

open System
open System.Runtime.CompilerServices
open Aardvark.Base

type RayHit<'r> =
    struct
        val mutable public T : float
        val mutable public Value : 'r

        new(t, value) = { T = t; Value = value }
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RayHit =
    let inline create (t : float) (value : 'r) = RayHit(t, value)
    let inline t (hit : RayHit<'r>) = hit.T
    let inline value (hit : RayHit<'r>) = hit.Value

    let map (f : 'a -> 'b) (hit : RayHit<'a>) =
        RayHit(hit.T, f hit.Value)


type RayPart =
    struct
        val mutable public Ray  : FastRay3d
        val mutable public TMin : float
        val mutable public TMax : float
        
        static member Intersects(x : RayPart, tri : Triangle3d) =
            let ray = x.Ray.Ray
            let mutable t = 0.0
            if tri.Intersects(ray, x.TMin, x.TMax, &t) then
                Some t
            else
                None

        static member Intersects(x : RayPart, box : Box3d) =
            let ray = x.Ray
            let mutable tmin = x.TMin
            let mutable tmax = x.TMax
            if ray.Intersects(box, &tmin, &tmax) then
                Some tmin
            else
                None

        static member Intersects(x : RayPart, sphere : Sphere3d) =
            let ray = x.Ray.Ray
            //  | (o + t * d - c) |^2 = r^2
            // let x = o - c
            //  | (x + t * d) |^2 = r^2
            //  <x + t*d | x + t*d> = r^2
            //  <x|x>  + <t*d|x> + <x|t*d> + <t*d|t*d> = r^2
            //  t^2*(<d|d>) + t*(2*<d|x>) + (<x|x> - r^2) = 0

            let dp = ray.Origin - sphere.Center
            let d = ray.Direction
            let a = d.LengthSquared
            let b = 2.0 * Vec.dot d dp
            let c = dp.LengthSquared - sphere.RadiusSquared

            let s = b*b - 4.0*a*c
            if s < 0.0 then
                None
            else
                let s = sqrt s
                let t1 = (-b + s) / (2.0 * a)
                let t2 = (-b - s) / (2.0 * a)

                let t1v = t1 >= x.TMin && t1 <= x.TMax
                let t2v = t2 >= x.TMin && t2 <= x.TMax

                if t1v && t2v then Some (min t1 t2)
                elif t1v then Some t1
                elif t2v then Some t2
                else None

        static member Intersects(x : RayPart, cylinder : Cylinder3d) =
            let ray = x.Ray.Ray
            let tmin = x.TMin
            let tmax = x.TMax
            // taken from http://mrl.nyu.edu/~dzorin/rend05/lecture2.pdf
            let v = ray.Direction
            let va = cylinder.P1 - cylinder.P0
            let la = Vec.length va
            let va = va / la
            let dp = ray.Origin - cylinder.P0
            let r2 = cylinder.Radius * cylinder.Radius

            let v' = v - (Vec.dot v va * va)
            let dp' = dp - (Vec.dot dp va) * va

            let a = v' |> Vec.lengthSquared
            let b = 2.0 * (Vec.dot v' dp')
            let c = Vec.lengthSquared dp' - r2
            let s = b*b - 4.0*a*c
            if s < 0.0 then
                None
            else
                let s = sqrt s
                let t1 = (-b + s) / (2.0 * a)
                let t2 = (-b - s) / (2.0 * a)

                let p0 = Plane3d(va, cylinder.P0)
                let p1 = Plane3d(va, cylinder.P1)

                let inline isValid (t : float) =
                    let pt = ray.GetPointOnRay t
                    t1 >= tmin && t1 <= tmax && p0.Height pt >= 0.0 && p1.Height pt <= 0.0

                let t1 = if isValid t1 then Some t1 else None
                let t2 = if isValid t2 then Some t2 else None

                let t3 = 
                    let mutable t = 0.0
                    let mutable pt = V3d.Zero
                    if ray.Intersects(p0, &t, &pt) then
                        let d = Vec.cross (pt - cylinder.P0) va |> Vec.lengthSquared
                        if d < r2 then Some t
                        else None
                    else
                        None

                let t4 = 
                    let mutable t = 0.0
                    let mutable pt = V3d.Zero
                    if ray.Intersects(p1, &t, &pt) then
                        let d = Vec.cross (pt - cylinder.P0) va |> Vec.lengthSquared
                        if d < r2 then Some t
                        else None
                    else
                        None

                match [t1;t2;t3;t4] |> List.choose id with
                    | [] -> None
                    | all -> List.min all |> Some

        new(ray, min, max) = { Ray = ray; TMin = min; TMax = max }
        new(ray) = { Ray = ray; TMin = 0.0; TMax = Double.PositiveInfinity }
    end
    
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RayPart =
    let inline ray (part : RayPart) = part.Ray
    let inline tmin (part : RayPart) = part.TMin
    let inline tmax (part : RayPart) = part.TMax
    let inline ofRay (ray : FastRay3d) = RayPart(ray)
    let inline create (ray : FastRay3d) (tmin : float) (tmax : float) = RayPart(ray, tmin, tmax)

    let inline intersect (part : RayPart) a =
        let inline call (d : ^d) (a : ^a) = ((^d or ^a) : (static member Intersects : ^d * ^a -> Option<float>) (d, a))
        call part a

[<Flags>]
type PlaneSide =
    | None = 0x0
    | Above = 0x1
    | Below = 0x2
    | Both = 0x3

[<AbstractClass>]
type Spatial<'a>() =
    abstract member ComputeBounds : seq<'a> -> Box3d
    abstract member PlaneSide : Plane3d * 'a -> PlaneSide

module Spatial =
    let private side (pt : V3d) (plane : Plane3d) =
        let h = plane.Height pt
        if h > 0.0 then PlaneSide.Above
        elif h < 0.0 then PlaneSide.Below
        else PlaneSide.Both

    let triangle =
        { new Spatial<Triangle3d>() with
            member x.ComputeBounds tris =
                tris |> Seq.collect (fun t -> [t.P0; t.P1; t.P2]) |> Box3d

            member x.PlaneSide(plane, tri) =
                side tri.P0 plane ||| side tri.P1 plane ||| side tri.P2 plane
        }

    let box =
        { new Spatial<Box3d>() with
            member x.ComputeBounds boxes =
                Box3d boxes

            member x.PlaneSide(plane, box) =
                let hmin = plane.Height box.Min
                let hmax = plane.Height box.Max
                if hmin > 0.0 && hmax > 0.0 then
                    PlaneSide.Above
                elif hmin < 0.0 && hmax < 0.0 then
                    PlaneSide.Below
                else
                    PlaneSide.Both
        }

    let point =
        { new Spatial<V3d>() with
            member x.ComputeBounds pts =
                pts |> Box3d

            member x.PlaneSide(plane, pt) =
                side pt plane
        }

type IRayIntersectable<'a> =
    abstract member Intersect : tryIntersect : (RayPart -> 'a -> Option<RayHit<'r>>) * part : RayPart -> Option<RayHit<'r>>

type ICullable<'a> =
    abstract member Cull : hull : FastHull3d -> list<'a>


[<AbstractClass; Sealed; Extension>]
type GeometryExtensions private() =

    static let toPlane (v : V4d) =
        Plane3d(-v.XYZ, v.W)

    static let toHull3d (viewProj : Trafo3d) =
        let r0 = viewProj.Forward.R0
        let r1 = viewProj.Forward.R1
        let r2 = viewProj.Forward.R2
        let r3 = viewProj.Forward.R3


        Hull3d [|
            r3 + r0 |> toPlane  // left
            r3 - r0 |> toPlane  // right
            r3 + r1 |> toPlane  // bottom
            r3 - r1 |> toPlane  // top
            r3 + r2 |> toPlane  // near
            r3 - r2 |> toPlane  // far
        |]

    [<Extension>]
    static member inline Intersect(this : IRayIntersectable<_>, part : RayPart) =
        let intersect (part : RayPart) (value : 'a) = 
            match RayPart.intersect part value with
                | Some t -> Some (RayHit(t, value))
                | None -> None

        this.Intersect(intersect, part)
        
    [<Extension>]
    static member CullFrustum(this : ICullable<'a>, viewProj : Trafo3d) =
        let hull = toHull3d viewProj
        this.Cull(FastHull3d hull)