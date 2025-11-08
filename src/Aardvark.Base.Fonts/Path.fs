namespace Aardvark.Base.Fonts

open Aardvark.Base

[<RequireQualifiedAccess>]
type WindingRule =
    | NonZero
    | Positive
    | Negative
    | EvenOdd
    | AbsGreaterEqualTwo

type Path =
    private { bounds : Box2d; outline : PathSegment[] }
    member x.Bounds = x.bounds
    member x.Outline = x.outline

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Path =

    let empty =
        { bounds = Box2d.Invalid; outline = [||] }

    /// create a path using a single segment
    let single (seg : PathSegment) =
        { bounds = PathSegment.bounds seg; outline = [| seg |] }

    /// creates a path using the given segments
    let ofSeq (segments : seq<PathSegment>) =
        let arr = Seq.toArray segments
        let bounds = arr |> Array.fold (fun l r -> Box.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = arr }

    /// creates a path using the given segments
    let ofList (segments : list<PathSegment>) =
        let arr = List.toArray segments
        let bounds = arr |> Array.fold (fun l r -> Box.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = arr }

    /// creates a path using the given segments
    let ofArray (segments : PathSegment[]) =
        let bounds = segments |> Array.fold (fun l r -> Box.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = segments }

    /// returns all path segments
    let toSeq (p : Path) =
        p.outline :> seq<_>

    /// returns all path segments
    let toList (p : Path) =
        p.outline |> Array.toList

    /// returns all path segments
    let toArray (p : Path) =
        p.outline |> Array.copy

    /// concatenates two paths
    let append (l : Path) (r : Path) =
        { bounds = Box.Union(l.bounds, r.bounds); outline = Array.append l.outline r.outline }

    /// concatenates a sequence paths
    let concat (l : seq<Path>) =
        let bounds = l |> Seq.fold (fun l r -> Box.Union(l, r.bounds)) Box2d.Invalid
        let arr = l |> Seq.collect toArray |> Seq.toArray
        { bounds = bounds; outline = arr }

    /// reverses the entrie path
    let reverse (p : Path) =
        { bounds = p.bounds; outline = p.outline |> Array.map PathSegment.reverse |> Array.rev }

    /// gets an axis-aligned bounding box for the path
    let bounds (p : Path) =
        p.outline |> Seq.map PathSegment.bounds |> Box2d

    /// gets the segment count for the path
    let count (p : Path) =
        p.outline.Length

    /// gets the i-th segment from the path
    let item (i : int) (p : Path) =
        p.outline.[i]

    /// applies the given transformation to all points used by the path
    let transform (f : V2d -> V2d) (p : Path) =
        p.outline |> Array.map (PathSegment.transform f) |> ofArray

    type PathBuilderState =
        {
            currentStart : Option<V2d>
            current : Option<V2d>
            segments : list<PathSegment>
        }

    type PathBuilder() =
        member x.Yield(()) =
            {
                currentStart = None
                current = None
                segments = []
            }

        [<CustomOperation("start")>]
        member x.Start(s : PathBuilderState, pt : V2d) =
            { s with current = Some pt; currentStart = Some pt }

        [<CustomOperation("lineTo")>]
        member x.LineTo(s : PathBuilderState, p1 : V2d) =
            match s.current with
                | Some p0 ->
                    match PathSegment.tryLine p0 p1 with
                        | Some seg ->
                            { s with current = Some p1; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"

        [<CustomOperation("bezierTo")>]
        member x.BezierTo(s : PathBuilderState, pc : V2d, p1 : V2d) =
            match s.current with
                | Some p0 ->
                    match PathSegment.tryBezier2 p0 pc p1 with
                        | Some seg ->
                            { s with current = Some p1; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"

        [<CustomOperation("arc")>]
        member x.Arc(s : PathBuilderState, p1 : V2d, p2 : V2d) =
            match s.current with
                | Some p0 ->
                    match PathSegment.tryArcSegment p0 p1 p2 with
                        | Some seg ->
                            { s with current = Some p2; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"

        [<CustomOperation("bezierTo3")>]
        member x.BezierTo3(s : PathBuilderState, pc0 : V2d, pc1 : V2d, p1 : V2d) =
            match s.current with
                | Some p0 ->
                    match PathSegment.tryBezier3 p0 pc0 pc1 p1 with
                        | Some seg ->
                            { s with current = Some p1; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"

        [<CustomOperation("stop")>]
        member x.Stop(s : PathBuilderState) =
            { s with current = None; currentStart = None }

        [<CustomOperation("close")>]
        member x.CloseLine(s : PathBuilderState) =
            match s.current, s.currentStart with
                | Some current, Some start ->
                    let s = { s with current = None; currentStart = None }

                    match PathSegment.tryLine current start with
                        | Some seg -> { s with segments = seg :: s.segments }
                        | None -> s
                | _ ->
                    failwith "cannot close without starting the path"

        member x.Run(s : PathBuilderState) =
            ofList (List.rev s.segments)

    let build = PathBuilder()

    /// creates a geometry using the !!closed!! path which contains the left-hand-side of
    /// the outline.
    /// The returned geometry contains Positions and a 4-dimensional vector (KLMKind) describing the
    /// (k,l,m) coordinates for boundary triangles in its xyz components and
    /// the kind of the triangle (inner = 0, boundary = 1) in its w component
    let toGeometry (rule : WindingRule) (p : Path) =
        let rule =
            match rule with
            | WindingRule.Positive -> LibTessDotNet.Double.WindingRule.Positive
            | WindingRule.Negative -> LibTessDotNet.Double.WindingRule.Negative
            | WindingRule.NonZero -> LibTessDotNet.Double.WindingRule.NonZero
            | WindingRule.EvenOdd -> LibTessDotNet.Double.WindingRule.EvenOdd
            | WindingRule.AbsGreaterEqualTwo -> LibTessDotNet.Double.WindingRule.AbsGeqTwo

        Tessellator.toGeometry rule p.bounds p.outline