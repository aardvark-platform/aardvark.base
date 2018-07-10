namespace Aardvark.Geometry

open Aardvark.Base

[<AbstractClass>]
type Intersectable3d() =
    abstract member Intersects : Box3d -> bool
    abstract member Contains : Box3d -> bool
    abstract member Contains : V3d -> bool
    default x.Contains(v : V3d) = x.Contains(Box3d(v,v))

[<RequireQualifiedAccess>]
type Region3d = 
    | Single      of            Intersectable3d
    | And         of Region3d * Region3d 
    | Or          of Region3d * Region3d
    | Xor         of Region3d * Region3d
    | Subtract    of Region3d * Region3d
    | Complement  of Region3d
    | Empty 

module Intersectable3d =
    
    let ofBox3d (b : Box3d) =
        {
            new Intersectable3d() with
                member x.Intersects(o : Box3d) = b.Intersects(o)
                member x.Contains(o : Box3d) = b.Contains(o)
                member x.Contains(o : V3d) = b.Contains(o)
        }

    let ofSphere3d (s : Sphere3d) =
        {
            new Intersectable3d() with
                member x.Intersects(o : Box3d) =    o.Intersects(s)
                member x.Contains(o : Box3d) =      o.ComputeCorners() |> Array.forall x.Contains
                member x.Contains(o : V3d) =        V3d.Distance(o, s.Center) <= s.Radius
        }

    let ofNearPlanePolygon (viewProj : Trafo3d) (npp : PolyRegion) =

        let hull = PolyRegion.toHull3d viewProj

        let intersectsBox (box : Box3d) =
            if hull.Intersects box then
                box 
                |> PolyRegion.ofProjectedBox viewProj
                |> PolyRegion.overlaps npp
            else
                false

        let containsPoint (v : V3d) =
            let pp = viewProj.Forward.TransformPosProj v
            if pp.Z >= -1.0 then
                PolyRegion.containsPoint pp.XY npp
            else
                false
        
        let containsBox (b : Box3d) =
            let corners = b.ComputeCorners()
            corners |> Array.forall (fun p -> hull.Contains p && containsPoint p)
            
        {
            new Intersectable3d() with
                member x.Intersects(o : Box3d) =    intersectsBox o
                member x.Contains(o : Box3d) =      containsBox   o
                member x.Contains(o : V3d) =        containsPoint o
        }

module Region3d =

    let empty = Region3d.Empty
    let inline ofIntersectable x = Region3d.Single x
    let inline ofBox3d b = Region3d.Single (Intersectable3d.ofBox3d b)
    let inline ofSphere3d s = Region3d.Single (Intersectable3d.ofSphere3d s)
    let inline ofNearPlanePolygon viewProj p = Region3d.Single (Intersectable3d.ofNearPlanePolygon viewProj p)

    let inline intersect l r = Region3d.And(l,r)
    let inline union l r = Region3d.Or(l,r)
    let inline xor l r = Region3d.Xor(l,r)
    let inline subtract l r = Region3d.Subtract(l,r)
    let inline complement r = Region3d.Complement r
    
    let rec intersects (b : Box3d) (s : Region3d) =
        match s with
        | Region3d.Empty ->    false
        | Region3d.Single x ->       x.Intersects b
        | Region3d.Or (l,r) ->       intersects b r || intersects b l
        | Region3d.And (l,r) ->      intersects b r && intersects b l
        | Region3d.Xor (l,r) ->      intersects b (Region3d.Or(l,r)) && not (containsBox b (Region3d.And(l,r)))
        | Region3d.Subtract (l,r) -> (not (containsBox b r)) && intersects b l
        | Region3d.Complement (l) ->     not (containsBox b l)

    and containsBox (b : Box3d) (s : Region3d) =
        match s with
        | Region3d.Empty ->    false
        | Region3d.Single x ->       x.Contains b
        | Region3d.Or (l,r) ->       intersects b (Region3d.And(Region3d.Complement(l),Region3d.Complement(r))) |> not
        | Region3d.And (l,r) ->      containsBox b r && containsBox b l
        | Region3d.Xor (l,r) ->      containsBox b (Region3d.Or(l,r)) && not (intersects b (Region3d.And(l,r)))
        | Region3d.Subtract (l,r) -> (not (intersects b r)) && containsBox b l
        | Region3d.Complement (l) -> not (intersects b l)

    let rec contains (v : V3d) (s : Region3d) =
        match s with
        | Region3d.Empty ->    false
        | Region3d.Single x ->       x.Contains v
        | Region3d.Or (l,r) ->       contains v r || contains v l
        | Region3d.And (l,r) ->      contains v r && contains v l
        | Region3d.Xor (l,r) ->      contains v r <> contains v l
        | Region3d.Subtract (l,r) -> not (contains v r) && contains v l
        | Region3d.Complement (l) -> not (contains v l)