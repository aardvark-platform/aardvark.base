namespace Aardvark.Base


open System.Runtime.CompilerServices

[<AbstractClass; Sealed; Extension>]
type PolygonExtensions private() =
    [<Extension>]
    static member Simplify(x : Polygon2d, eps : float) =
        if eps <= 0.0 || x.PointCount <= 3 then
            x
        else
            // find two points with maximum curvature
            let mutable angle0 = 0.0
            let mutable angle1 = 0.0
            let mutable id0 = -1
            let mutable id1 = -1

            let mutable i0 = x.PointCount - 2
            let mutable i1 = x.PointCount - 1
            let mutable d01 = Vec.normalize (x.[i1] - x.[i0])
            for i2 in 0 .. x.PointCount - 1 do
                let d12 = Vec.normalize (x.[i2] - x.[i1])
                let angle = Vec.AngleBetween(d01, d12) |> abs

                if angle > angle0 then
                    angle1 <- angle0
                    id1 <- id0
                    angle0 <- angle
                    id0 <- i1
                elif angle > angle1 then
                    angle1 <- angle
                    id1 <- i1

                i0 <- i1
                i1 <- i2
                d01 <- d12

            // if there are at least two points with curvature > 0
            if id0 >= 0 && id1 >= 0 then
                let inline iter (s : int) (e : int) (action : int -> unit) =
                    let mutable i = (s + x.PointCount) % x.PointCount
                    let e = (e + 1) % x.PointCount
                    while i <> e do
                        action i
                        i <- i + 1
                        if i >= x.PointCount then i <- i - x.PointCount

                let rec simplify (set : System.Collections.Generic.SortedSet<int>) (l : int) (r : int) =
                    set.Add l |> ignore
                    if l <> r then
                        let p0 = x.[l]
                        let p1 = x.[r]
                        let d = Vec.normalize (p1 - p0)
                        let n = V2d(-d.Y, d.X)

                        // find the worst point (max height)
                        let mutable hMax = 0.0
                        let mutable max = -1
                        iter (l+1) (r-1) (fun i ->
                            let h = Vec.dot n (x.[i] - p0) |> abs
                            if h > hMax then
                                hMax <- h
                                max <- i
                        )

                        // if not within tolerance continue recursively
                        if hMax > eps then
                            simplify set l max
                            simplify set max r

                let set = System.Collections.Generic.SortedSet()

                // simplify both curves
                simplify set id0 id1
                simplify set id1 id0

                let arr = Array.zeroCreate set.Count
                let mutable i = 0
                for i0 in set do 
                    arr.[i] <- x.[i0]
                    i <- i + 1

                Polygon2d arr
            else
                x
                
    [<Extension>]
    static member Simplify(x : Polygon3d, eps : float) =
        if eps <= 0.0 || x.PointCount <= 3 then
            x
        else
            // find two points with maximum curvature
            let mutable angle0 = 0.0
            let mutable angle1 = 0.0
            let mutable id0 = -1
            let mutable id1 = -1

            let mutable i0 = x.PointCount - 2
            let mutable i1 = x.PointCount - 1
            let mutable d01 = Vec.normalize (x.[i1] - x.[i0])
            for i2 in 0 .. x.PointCount - 1 do
                let d12 = Vec.normalize (x.[i2] - x.[i1])
                let angle = Vec.AngleBetween(d01, d12) |> abs

                if angle > angle0 then
                    angle1 <- angle0
                    id1 <- id0
                    angle0 <- angle
                    id0 <- i1
                elif angle > angle1 then
                    angle1 <- angle
                    id1 <- i1

                i0 <- i1
                i1 <- i2
                d01 <- d12

            // if there are at least two points with curvature > 0
            if id0 >= 0 && id1 >= 0 then
                let inline iter (s : int) (e : int) (action : int -> unit) =
                    let mutable i = (s + x.PointCount) % x.PointCount
                    let e = (e + 1) % x.PointCount
                    while i <> e do
                        action i
                        i <- i + 1
                        if i >= x.PointCount then i <- i - x.PointCount

                let rec simplify (set : System.Collections.Generic.SortedSet<int>) (l : int) (r : int) =
                    set.Add l |> ignore
                    if l <> r then
                        let p0 = x.[l]
                        let p1 = x.[r]
                        let d = Vec.normalize (p1 - p0)
                        let n = V2d(-d.Y, d.X)

                        // find the worst point (max height)
                        let mutable hMax = 0.0
                        let mutable max = -1
                        iter (l+1) (r-1) (fun i ->
                            let h = x.[i].DistanceToInfiniteLine(p0, p1)
                            if h > hMax then
                                hMax <- h
                                max <- i
                        )

                        // if not within tolerance continue recursively
                        if hMax > eps then
                            simplify set l max
                            simplify set max r

                let set = System.Collections.Generic.SortedSet()

                // simplify both curves
                simplify set id0 id1
                simplify set id1 id0

                let arr = Array.zeroCreate set.Count
                let mutable i = 0
                for i0 in set do 
                    arr.[i] <- x.[i0]
                    i <- i + 1

                Polygon3d arr
            else
                x

        
