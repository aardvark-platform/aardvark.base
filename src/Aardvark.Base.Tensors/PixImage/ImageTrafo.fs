namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ImageTrafo =
    let private composeTable =
        LookupTable.lookup [
            struct (ImageTrafo.Identity, ImageTrafo.Identity), ImageTrafo.Identity
            struct (ImageTrafo.Identity, ImageTrafo.Rot90), ImageTrafo.Rot90
            struct (ImageTrafo.Identity, ImageTrafo.Rot180), ImageTrafo.Rot180
            struct (ImageTrafo.Identity, ImageTrafo.Rot270), ImageTrafo.Rot270
            struct (ImageTrafo.Identity, ImageTrafo.MirrorX), ImageTrafo.MirrorX
            struct (ImageTrafo.Identity, ImageTrafo.Transpose), ImageTrafo.Transpose
            struct (ImageTrafo.Identity, ImageTrafo.MirrorY), ImageTrafo.MirrorY
            struct (ImageTrafo.Identity, ImageTrafo.Transverse), ImageTrafo.Transverse
            struct (ImageTrafo.Rot90, ImageTrafo.Identity), ImageTrafo.Rot90
            struct (ImageTrafo.Rot90, ImageTrafo.Rot90), ImageTrafo.Rot180
            struct (ImageTrafo.Rot90, ImageTrafo.Rot180), ImageTrafo.Rot270
            struct (ImageTrafo.Rot90, ImageTrafo.Rot270), ImageTrafo.Identity
            struct (ImageTrafo.Rot90, ImageTrafo.MirrorX), ImageTrafo.Transverse
            struct (ImageTrafo.Rot90, ImageTrafo.Transpose), ImageTrafo.MirrorX
            struct (ImageTrafo.Rot90, ImageTrafo.MirrorY), ImageTrafo.Transpose
            struct (ImageTrafo.Rot90, ImageTrafo.Transverse), ImageTrafo.MirrorY
            struct (ImageTrafo.Rot180, ImageTrafo.Identity), ImageTrafo.Rot180
            struct (ImageTrafo.Rot180, ImageTrafo.Rot90), ImageTrafo.Rot270
            struct (ImageTrafo.Rot180, ImageTrafo.Rot180), ImageTrafo.Identity
            struct (ImageTrafo.Rot180, ImageTrafo.Rot270), ImageTrafo.Rot90
            struct (ImageTrafo.Rot180, ImageTrafo.MirrorX), ImageTrafo.MirrorY
            struct (ImageTrafo.Rot180, ImageTrafo.Transpose), ImageTrafo.Transverse
            struct (ImageTrafo.Rot180, ImageTrafo.MirrorY), ImageTrafo.MirrorX
            struct (ImageTrafo.Rot180, ImageTrafo.Transverse), ImageTrafo.Transpose
            struct (ImageTrafo.Rot270, ImageTrafo.Identity), ImageTrafo.Rot270
            struct (ImageTrafo.Rot270, ImageTrafo.Rot90), ImageTrafo.Identity
            struct (ImageTrafo.Rot270, ImageTrafo.Rot180), ImageTrafo.Rot90
            struct (ImageTrafo.Rot270, ImageTrafo.Rot270), ImageTrafo.Rot180
            struct (ImageTrafo.Rot270, ImageTrafo.MirrorX), ImageTrafo.Transpose
            struct (ImageTrafo.Rot270, ImageTrafo.Transpose), ImageTrafo.MirrorY
            struct (ImageTrafo.Rot270, ImageTrafo.MirrorY), ImageTrafo.Transverse
            struct (ImageTrafo.Rot270, ImageTrafo.Transverse), ImageTrafo.MirrorX
            struct (ImageTrafo.MirrorX, ImageTrafo.Identity), ImageTrafo.MirrorX
            struct (ImageTrafo.MirrorX, ImageTrafo.Rot90), ImageTrafo.Transpose
            struct (ImageTrafo.MirrorX, ImageTrafo.Rot180), ImageTrafo.MirrorY
            struct (ImageTrafo.MirrorX, ImageTrafo.Rot270), ImageTrafo.Transverse
            struct (ImageTrafo.MirrorX, ImageTrafo.MirrorX), ImageTrafo.Identity
            struct (ImageTrafo.MirrorX, ImageTrafo.Transpose), ImageTrafo.Rot90
            struct (ImageTrafo.MirrorX, ImageTrafo.MirrorY), ImageTrafo.Rot180
            struct (ImageTrafo.MirrorX, ImageTrafo.Transverse), ImageTrafo.Rot270
            struct (ImageTrafo.Transpose, ImageTrafo.Identity), ImageTrafo.Transpose
            struct (ImageTrafo.Transpose, ImageTrafo.Rot90), ImageTrafo.MirrorY
            struct (ImageTrafo.Transpose, ImageTrafo.Rot180), ImageTrafo.Transverse
            struct (ImageTrafo.Transpose, ImageTrafo.Rot270), ImageTrafo.MirrorX
            struct (ImageTrafo.Transpose, ImageTrafo.MirrorX), ImageTrafo.Rot270
            struct (ImageTrafo.Transpose, ImageTrafo.Transpose), ImageTrafo.Identity
            struct (ImageTrafo.Transpose, ImageTrafo.MirrorY), ImageTrafo.Rot90
            struct (ImageTrafo.Transpose, ImageTrafo.Transverse), ImageTrafo.Rot180
            struct (ImageTrafo.MirrorY, ImageTrafo.Identity), ImageTrafo.MirrorY
            struct (ImageTrafo.MirrorY, ImageTrafo.Rot90), ImageTrafo.Transverse
            struct (ImageTrafo.MirrorY, ImageTrafo.Rot180), ImageTrafo.MirrorX
            struct (ImageTrafo.MirrorY, ImageTrafo.Rot270), ImageTrafo.Transpose
            struct (ImageTrafo.MirrorY, ImageTrafo.MirrorX), ImageTrafo.Rot180
            struct (ImageTrafo.MirrorY, ImageTrafo.Transpose), ImageTrafo.Rot270
            struct (ImageTrafo.MirrorY, ImageTrafo.MirrorY), ImageTrafo.Identity
            struct (ImageTrafo.MirrorY, ImageTrafo.Transverse), ImageTrafo.Rot90
            struct (ImageTrafo.Transverse, ImageTrafo.Identity), ImageTrafo.Transverse
            struct (ImageTrafo.Transverse, ImageTrafo.Rot90), ImageTrafo.MirrorX
            struct (ImageTrafo.Transverse, ImageTrafo.Rot180), ImageTrafo.Transpose
            struct (ImageTrafo.Transverse, ImageTrafo.Rot270), ImageTrafo.MirrorY
            struct (ImageTrafo.Transverse, ImageTrafo.MirrorX), ImageTrafo.Rot90
            struct (ImageTrafo.Transverse, ImageTrafo.Transpose), ImageTrafo.Rot180
            struct (ImageTrafo.Transverse, ImageTrafo.MirrorY), ImageTrafo.Rot270
            struct (ImageTrafo.Transverse, ImageTrafo.Transverse), ImageTrafo.Identity
        ]

    let compose (l : ImageTrafo) (r : ImageTrafo) =
        composeTable (struct (l, r))

    let inverse =
        LookupTable.lookup [
            ImageTrafo.MirrorX, ImageTrafo.MirrorX
            ImageTrafo.MirrorY, ImageTrafo.MirrorY
            ImageTrafo.Identity, ImageTrafo.Identity
            ImageTrafo.Rot180, ImageTrafo.Rot180
            ImageTrafo.Rot270, ImageTrafo.Rot90
            ImageTrafo.Rot90, ImageTrafo.Rot270
            ImageTrafo.Transpose, ImageTrafo.Transpose
            ImageTrafo.Transverse, ImageTrafo.Transverse
        ]

    let transformSize (s : V2i) (t : ImageTrafo) =
        match t with
        | ImageTrafo.Identity | ImageTrafo.Rot180 | ImageTrafo.MirrorX | ImageTrafo.MirrorY -> s
        | ImageTrafo.Rot270 | ImageTrafo.Rot90 | ImageTrafo.Transpose | ImageTrafo.Transverse -> V2i(s.Y, s.X)
        | _ -> failwithf "[ImageTrafo] unknown value %A" t

    let inverseTransformSize (s : V2i) (t : ImageTrafo) =
        transformSize s t