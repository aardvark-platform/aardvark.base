namespace Aardvark.Base

#nowarn "77"

// Statically resolved, generic converters
[<AutoOpen>]
module Converters =

    module Identity =
        type Converter() =
            static member inline op_Explicit (x : ^a) : ^a = x

    let inline private conv< ^z, ^a, ^b when (^z or ^a or ^b) : (static member op_Explicit : ^a -> ^b) > (a : ^a) : ^b =
        ((^z or ^a or ^b) : (static member op_Explicit : ^a -> ^b) (a))

    [<AutoOpen>]
    module Vector =
        let inline v2d (v : ^a) = conv< Identity.Converter, ^a, V2d > v
        let inline v2f (v : ^a) = conv< Identity.Converter, ^a, V2f > v
        let inline v2i (v : ^a) = conv< Identity.Converter, ^a, V2i > v
        let inline v2l (v : ^a) = conv< Identity.Converter, ^a, V2l > v

        let inline v3d (v : ^a) = conv< Identity.Converter, ^a, V3d > v
        let inline v3f (v : ^a) = conv< Identity.Converter, ^a, V3f > v
        let inline v3i (v : ^a) = conv< Identity.Converter, ^a, V3i > v
        let inline v3l (v : ^a) = conv< Identity.Converter, ^a, V3l > v

        let inline v4d (v : ^a) = conv< Identity.Converter, ^a, V4d > v
        let inline v4f (v : ^a) = conv< Identity.Converter, ^a, V4f > v
        let inline v4i (v : ^a) = conv< Identity.Converter, ^a, V4i > v
        let inline v4l (v : ^a) = conv< Identity.Converter, ^a, V4l > v

        module private CompilerTests =

            let working() =
                let a : V3d = v3d V2i.IO
                let a : V3d = v3d V3f.IOI
                let a : V3d = v3d V3d.IOI
                let a : V3d = v3d V4l.IOII
                let a : V3d = v3d [| 0l; 1l; 2l |]
                let a : V3d = v3d C3ui.BlueViolet
                let a : V4d = v4d C3ui.BlueViolet
                let a : V3d = v3d C4f.BlueViolet
                ()

    [<AutoOpen>]
    module Color =
        let inline c3b  (v : ^a) = conv< Identity.Converter, ^a, C3b  > v
        let inline c3us (v : ^a) = conv< Identity.Converter, ^a, C3us > v
        let inline c3ui (v : ^a) = conv< Identity.Converter, ^a, C3ui > v
        let inline c3f  (v : ^a) = conv< Identity.Converter, ^a, C3f  > v
        let inline c3d  (v : ^a) = conv< Identity.Converter, ^a, C3d  > v

        let inline c4b  (v : ^a) = conv< Identity.Converter, ^a, C4b  > v
        let inline c4us (v : ^a) = conv< Identity.Converter, ^a, C4us > v
        let inline c4ui (v : ^a) = conv< Identity.Converter, ^a, C4ui > v
        let inline c4f  (v : ^a) = conv< Identity.Converter, ^a, C4f  > v
        let inline c4d  (v : ^a) = conv< Identity.Converter, ^a, C4d  > v

        let working() =
            let a : C3d = c3d V3f.IOI
            let a : C3d = c3d V4d.IOII
            let a : C3ui = c3ui [| 0us; 1us; 2us |]
            let a : C3d = c3d C3ui.BlueViolet
            let a : C4d = c4d C3ui.BlueViolet
            ()

    [<AutoOpen>]
    module Matrix =

        let inline m22d (v : ^a) = conv< Identity.Converter, ^a, M22d > v
        let inline m23d (v : ^a) = conv< Identity.Converter, ^a, M23d > v
        let inline m33d (v : ^a) = conv< Identity.Converter, ^a, M33d > v
        let inline m34d (v : ^a) = conv< Identity.Converter, ^a, M34d > v
        let inline m44d (v : ^a) = conv< Identity.Converter, ^a, M44d > v

        let inline m22f (v : ^a) = conv< Identity.Converter, ^a, M22f > v
        let inline m23f (v : ^a) = conv< Identity.Converter, ^a, M23f > v
        let inline m33f (v : ^a) = conv< Identity.Converter, ^a, M33f > v
        let inline m34f (v : ^a) = conv< Identity.Converter, ^a, M34f > v
        let inline m44f (v : ^a) = conv< Identity.Converter, ^a, M44f > v

        let inline m22i (v : ^a) = conv< Identity.Converter, ^a, M22i > v
        let inline m23i (v : ^a) = conv< Identity.Converter, ^a, M23i > v
        let inline m33i (v : ^a) = conv< Identity.Converter, ^a, M33i > v
        let inline m34i (v : ^a) = conv< Identity.Converter, ^a, M34i > v
        let inline m44i (v : ^a) = conv< Identity.Converter, ^a, M44i > v

        let inline m22l (v : ^a) = conv< Identity.Converter, ^a, M22l > v
        let inline m23l (v : ^a) = conv< Identity.Converter, ^a, M23l > v
        let inline m33l (v : ^a) = conv< Identity.Converter, ^a, M33l > v
        let inline m34l (v : ^a) = conv< Identity.Converter, ^a, M34l > v
        let inline m44l (v : ^a) = conv< Identity.Converter, ^a, M44l > v

        module private CompilerTests =

            let working() =
                let a : M33d = m33d M23i.Zero
                let a : M33d = m33d M33f.Zero
                let a : M33d = m33d M33d.Zero
                let a : M33d = m33d M34l.Zero
                let a : M22d = m22d [| 0l; 1l; 2l; 3l |]
                let a : M22d = m22d <| Array2D.create 2 2 2.0f
                ()