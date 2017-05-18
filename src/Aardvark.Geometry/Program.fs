namespace Aardvark.Geometry

module internal Demo =
    open Aardvark.Geometry
    open Aardvark.Base
    open System.Drawing
    open System.Drawing.Imaging

    let transform (s : Size) (bounds : Box2d) (p : V2d) =
        let r = (p - bounds.Min) / bounds.Size
        Point(int (r.X * float s.Width), int (r.Y * float s.Height))

    let closed (s : Size) (bounds : Box2d) (path : array<V2d>) =
        let isClosed = path.[0] = path.[path.Length-1]
           
        let arr = Array.zeroCreate (path.Length + (if isClosed then 0 else 1))

        for i in 0 .. path.Length - 1 do
            let pt = path.[i]
            arr.[i] <- transform s bounds pt
                        
        if not isClosed then
            arr.[path.Length] <- arr.[0]


        arr

    type Graphics with
        member x.DrawPolygon(pen : Pen, p : Polygon2d, bounds : Box2d) =
            let size = Size(int x.VisibleClipBounds.Width, int x.VisibleClipBounds.Height)
            let pts = p.Points |> Seq.toArray |> closed size bounds
            x.DrawPolygon(pen, pts)
        
        member x.DrawPolygon(pen : Pen, p : PolyRegion, bounds : Box2d) =
            for p in p.Polygons do
                x.DrawPolygon(pen, p, bounds)

        member x.FillPolygon(pen : Brush, p : Polygon2d, bounds : Box2d) =
            let size = Size(int x.VisibleClipBounds.Width, int x.VisibleClipBounds.Height)
            let pts = p.Points |> Seq.toArray |> closed size bounds
            x.FillPolygon(pen, pts)
        
        member x.FillPolygon(pen : Brush, p : PolyRegion, bounds : Box2d) =
            for p in p.Polygons do
                x.FillPolygon(pen, p, bounds)

    //[<EntryPoint>]
    let main args =

        let p0 = PolyRegion.ofArray [| V2d(0.0,0.0); V2d(1.0,0.0); V2d(1.0,1.0); V2d(0.0,1.0) |]
        let p1 = p0 |> PolyRegion.transformed (M33d.Translation(0.5, 0.5))
        
        let res = PolyRegion.union p0 p1


        let bounds = Box2d [ PolyRegion.bounds p0; PolyRegion.bounds p1 ]
        let bounds = bounds.EnlargedByRelativeEps(0.2)

        let triangles = PolyRegion.toTriangles res

        use bmp = new Bitmap(1024, 768, PixelFormat.Format24bppRgb)
        use g = Graphics.FromImage bmp

        g.Clear(Color.Black)
        g.DrawPolygon(Pens.Yellow, p0, bounds)
        g.DrawPolygon(Pens.Yellow, p1, bounds)

        g.FillPolygon(Brushes.Red, res, bounds)

        for t in triangles do
            g.DrawPolygon(Pens.White, t.ToPolygon2d(id), bounds)

        g.DrawPolygon(new Pen(Color.Blue, 3.0f), res, bounds)
        let s = 16
        for p in res.Polygons do
            for p in p.Points do
                let pp = transform bmp.Size bounds p
                g.FillEllipse(Brushes.Green, Rectangle(pp.X - s / 2, pp.Y - s / 2, s, s))



        bmp.Save @"C:\Users\Schorsch\Desktop\poly.jpg"

        0