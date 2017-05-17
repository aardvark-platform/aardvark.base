namespace Aardvark.Geometry

//open Aardvark.Geometry
//open Aardvark.Base
//open System.Drawing
//open System.Drawing.Imaging
//
//
//let transform (p : V2d) =
//    Point(int ((p.X * 400.0) + 25.0), int ((p.Y * 400.0) + 25.0))
//
//let closed (path : array<V2d>) =
//    let isClosed = path.[0] = path.[path.Length-1]
//           
//    let arr = Array.zeroCreate (path.Length + (if isClosed then 0 else 1))
//
//    for i in 0 .. path.Length - 1 do
//        let pt = path.[i]
//        arr.[i] <- transform pt
//                        
//    if not isClosed then
//        arr.[path.Length] <- arr.[0]
//
//
//    arr
//
//type Graphics with
//    member x.DrawPolygon(pen : Pen, p : Polygon2d) =
//        let pts = p.Points |> Seq.toArray |> closed
//        x.DrawPolygon(pen, pts)
//        
//    member x.DrawPolygon(pen : Pen, p : PolyRegion) =
//        for p in p.polygons do
//            x.DrawPolygon(pen, p)
//
//    member x.FillPolygon(pen : Brush, p : Polygon2d) =
//        let pts = p.Points |> Seq.toArray |> closed
//        x.FillPolygon(pen, pts)
//        
//    member x.FillPolygon(pen : Brush, p : PolyRegion) =
//        for p in p.polygons do
//            x.FillPolygon(pen, p)
//
//[<EntryPoint>]
//let main args =
//
//    let p0 = Polygon2d [| V2d(0.0,0.0); V2d(1.0,0.0); V2d(1.0,1.0); V2d(0.0,1.0) |] |> PolyRegion.ofPolygon
//    let p1 = p0 |> PolyRegion.transformed (M33d.Translation(0.5, 0.5))
//    //let res = PolyRegion.xor p0 p1
//    //let res = PolyRegion.difference p0 p0
//    let res = PolyRegion.union p0 p1
//
//
//    let triangles = PolyRegion.toTriangles res
//
//    use bmp = new Bitmap(1024, 768, PixelFormat.Format24bppRgb)
//    use g = Graphics.FromImage bmp
//
//    g.Clear(Color.Black)
//    g.DrawPolygon(Pens.Yellow, p0)
//    g.DrawPolygon(Pens.Yellow, p1)
//
//    g.FillPolygon(Brushes.Red, res)
//
//    for t in triangles do
//        g.DrawPolygon(Pens.White, t.ToPolygon2d(id))
//
//    g.DrawPolygon(new Pen(Color.Blue, 3.0f), res)
//    let s = 16
//    for p in res.polygons do
//        for p in p.Points do
//            let pp = transform p
//            g.FillEllipse(Brushes.Green, Rectangle(pp.X - s / 2, pp.Y - s / 2, s, s))
//
//
//
//    bmp.Save @"C:\Users\Schorsch\Desktop\poly.jpg"
//
//    0