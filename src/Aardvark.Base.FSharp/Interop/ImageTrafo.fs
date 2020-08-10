namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ImageTrafo =
    let private composeTable =
        Dictionary.ofList [
            (ImageTrafo.Identity, ImageTrafo.Identity), ImageTrafo.Identity
            (ImageTrafo.Identity, ImageTrafo.Rot90), ImageTrafo.Rot90
            (ImageTrafo.Identity, ImageTrafo.Rot180), ImageTrafo.Rot180
            (ImageTrafo.Identity, ImageTrafo.Rot270), ImageTrafo.Rot270
            (ImageTrafo.Identity, ImageTrafo.MirrorX), ImageTrafo.MirrorX
            (ImageTrafo.Identity, ImageTrafo.Transpose), ImageTrafo.Transpose
            (ImageTrafo.Identity, ImageTrafo.MirrorY), ImageTrafo.MirrorY
            (ImageTrafo.Identity, ImageTrafo.Transverse), ImageTrafo.Transverse
            (ImageTrafo.Rot90, ImageTrafo.Identity), ImageTrafo.Rot90
            (ImageTrafo.Rot90, ImageTrafo.Rot90), ImageTrafo.Rot180
            (ImageTrafo.Rot90, ImageTrafo.Rot180), ImageTrafo.Rot270
            (ImageTrafo.Rot90, ImageTrafo.Rot270), ImageTrafo.Identity
            (ImageTrafo.Rot90, ImageTrafo.MirrorX), ImageTrafo.Transverse
            (ImageTrafo.Rot90, ImageTrafo.Transpose), ImageTrafo.MirrorX
            (ImageTrafo.Rot90, ImageTrafo.MirrorY), ImageTrafo.Transpose
            (ImageTrafo.Rot90, ImageTrafo.Transverse), ImageTrafo.MirrorY
            (ImageTrafo.Rot180, ImageTrafo.Identity), ImageTrafo.Rot180
            (ImageTrafo.Rot180, ImageTrafo.Rot90), ImageTrafo.Rot270
            (ImageTrafo.Rot180, ImageTrafo.Rot180), ImageTrafo.Identity
            (ImageTrafo.Rot180, ImageTrafo.Rot270), ImageTrafo.Rot90
            (ImageTrafo.Rot180, ImageTrafo.MirrorX), ImageTrafo.MirrorY
            (ImageTrafo.Rot180, ImageTrafo.Transpose), ImageTrafo.Transverse
            (ImageTrafo.Rot180, ImageTrafo.MirrorY), ImageTrafo.MirrorX
            (ImageTrafo.Rot180, ImageTrafo.Transverse), ImageTrafo.Transpose
            (ImageTrafo.Rot270, ImageTrafo.Identity), ImageTrafo.Rot270
            (ImageTrafo.Rot270, ImageTrafo.Rot90), ImageTrafo.Identity
            (ImageTrafo.Rot270, ImageTrafo.Rot180), ImageTrafo.Rot90
            (ImageTrafo.Rot270, ImageTrafo.Rot270), ImageTrafo.Rot180
            (ImageTrafo.Rot270, ImageTrafo.MirrorX), ImageTrafo.Transpose
            (ImageTrafo.Rot270, ImageTrafo.Transpose), ImageTrafo.MirrorY
            (ImageTrafo.Rot270, ImageTrafo.MirrorY), ImageTrafo.Transverse
            (ImageTrafo.Rot270, ImageTrafo.Transverse), ImageTrafo.MirrorX
            (ImageTrafo.MirrorX, ImageTrafo.Identity), ImageTrafo.MirrorX
            (ImageTrafo.MirrorX, ImageTrafo.Rot90), ImageTrafo.Transpose
            (ImageTrafo.MirrorX, ImageTrafo.Rot180), ImageTrafo.MirrorY
            (ImageTrafo.MirrorX, ImageTrafo.Rot270), ImageTrafo.Transverse
            (ImageTrafo.MirrorX, ImageTrafo.MirrorX), ImageTrafo.Identity
            (ImageTrafo.MirrorX, ImageTrafo.Transpose), ImageTrafo.Rot90
            (ImageTrafo.MirrorX, ImageTrafo.MirrorY), ImageTrafo.Rot180
            (ImageTrafo.MirrorX, ImageTrafo.Transverse), ImageTrafo.Rot270
            (ImageTrafo.Transpose, ImageTrafo.Identity), ImageTrafo.Transpose
            (ImageTrafo.Transpose, ImageTrafo.Rot90), ImageTrafo.MirrorY
            (ImageTrafo.Transpose, ImageTrafo.Rot180), ImageTrafo.Transverse
            (ImageTrafo.Transpose, ImageTrafo.Rot270), ImageTrafo.MirrorX
            (ImageTrafo.Transpose, ImageTrafo.MirrorX), ImageTrafo.Rot270
            (ImageTrafo.Transpose, ImageTrafo.Transpose), ImageTrafo.Identity
            (ImageTrafo.Transpose, ImageTrafo.MirrorY), ImageTrafo.Rot90
            (ImageTrafo.Transpose, ImageTrafo.Transverse), ImageTrafo.Rot180
            (ImageTrafo.MirrorY, ImageTrafo.Identity), ImageTrafo.MirrorY
            (ImageTrafo.MirrorY, ImageTrafo.Rot90), ImageTrafo.Transverse
            (ImageTrafo.MirrorY, ImageTrafo.Rot180), ImageTrafo.MirrorX
            (ImageTrafo.MirrorY, ImageTrafo.Rot270), ImageTrafo.Transpose
            (ImageTrafo.MirrorY, ImageTrafo.MirrorX), ImageTrafo.Rot180
            (ImageTrafo.MirrorY, ImageTrafo.Transpose), ImageTrafo.Rot270
            (ImageTrafo.MirrorY, ImageTrafo.MirrorY), ImageTrafo.Identity
            (ImageTrafo.MirrorY, ImageTrafo.Transverse), ImageTrafo.Rot90
            (ImageTrafo.Transverse, ImageTrafo.Identity), ImageTrafo.Transverse
            (ImageTrafo.Transverse, ImageTrafo.Rot90), ImageTrafo.MirrorX
            (ImageTrafo.Transverse, ImageTrafo.Rot180), ImageTrafo.Transpose
            (ImageTrafo.Transverse, ImageTrafo.Rot270), ImageTrafo.MirrorY
            (ImageTrafo.Transverse, ImageTrafo.MirrorX), ImageTrafo.Rot90
            (ImageTrafo.Transverse, ImageTrafo.Transpose), ImageTrafo.Rot180
            (ImageTrafo.Transverse, ImageTrafo.MirrorY), ImageTrafo.Rot270
            (ImageTrafo.Transverse, ImageTrafo.Transverse), ImageTrafo.Identity
        ]

    let compose (l : ImageTrafo) (r : ImageTrafo) =
        composeTable.[(l,r)]

    let inverse =
        LookupTable.lookupTable [
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