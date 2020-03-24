namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Mat =

    let inline private transformAux (a : ^a) (b : ^b) (c : ^c) =
        ((^a or ^b or ^c) : (static member Transform : ^b * ^c -> ^d) (b, c))
        
    let inline private transformDirAux (a : ^a) (b : ^b) (c : ^c) =
        ((^a or ^b or ^c) : (static member TransformDir : ^b * ^c -> ^d) (b, c))
        
    let inline private transformPosAux (a : ^a) (b : ^b) (c : ^c) =
        ((^a or ^b or ^c) : (static member TransformPos : ^b * ^c -> ^d) (b, c))
        
    let inline private transformPosProjAux (a : ^a) (b : ^b) (c : ^c) =
        ((^a or ^b or ^c) : (static member TransformPosProj : ^b * ^c -> ^d) (b, c))

    let inline transpose (m : ^a) : ^b =
        (^a : (member Transposed : ^b) m)

    let inline det (m : ^a) : ^b =
        (^a : (member Determinant : ^b) m)

    let inline inverse (m : ^a) : ^b =
        (^a : (member Inverse : ^b) m)
        
    let inline transform (m : ^a) (v : ^b) =
        transformAux Unchecked.defaultof<Mat> m v

    let inline transformPos (m : ^a) (v : ^b) : ^b =
        transformPosAux Unchecked.defaultof<Mat> m v

    let inline transformDir (m : ^a) (v : ^b) : ^b =
        transformDirAux Unchecked.defaultof<Mat> m v

    let inline transformPosProj (m : ^a) (v : ^b) : ^b =
        transformPosProjAux Unchecked.defaultof<Mat> m v

    module private CompilerTests =

        let transformWorking () =
            let a : V4d = transform M44d.Identity V4d.Zero
            let a : V3d = transform M34d.Zero V4d.Zero
            let a = (transform M34d.Zero V4d.Zero) * 0.5
            let a = log (0.5 * (transform M22d.Identity V2d.Zero))

            let a : V3d = transformPos M44d.Identity V3d.Zero
            let a : V3d = transformPos M34d.Zero V3d.Zero
            let a = (transformPos M34d.Zero V3d.Zero) * 0.5
            let a = log (0.5 * (transformPos M33d.Identity V2d.Zero))

            let a : V3d = transformDir M44d.Identity V3d.Zero
            let a : V3d = transformDir M34d.Zero V3d.Zero
            let a = (transformDir M34d.Zero V3d.Zero) * 0.5
            let a = log (0.5 * (transformDir M33d.Identity V2d.Zero))

            let a : V3d = transformPosProj M44d.Identity V3d.Zero
            let a = log (0.5 * (transformPosProj M33d.Identity V2d.Zero))

            ()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Trafo =
    
    let inline forward (t : Trafo3d) = t.Forward
    let inline backward (t : Trafo3d) = t.Backward


#nowarn "77"

[<AutoOpen>]
module MatrixConverters =

    let inline private conv< ^a, ^b when (^a or ^b) : (static member op_Explicit : ^a -> ^b) > (a : ^a) : ^b =
        ((^a or ^b) : (static member op_Explicit : ^a -> ^b) (a))
    

    let inline m22d (v : ^a) = conv< ^a, M22d > v
    let inline m33d (v : ^a) = conv< ^a, M33d > v
    let inline m34d (v : ^a) = conv< ^a, M34d > v
    let inline m44d (v : ^a) = conv< ^a, M44d > v
    
    let inline m22f (v : ^a) = conv< ^a, M22f > v
    let inline m33f (v : ^a) = conv< ^a, M33f > v
    let inline m34f (v : ^a) = conv< ^a, M34f > v
    let inline m44f (v : ^a) = conv< ^a, M44f > v

    let inline m22i (v : ^a) = conv< ^a, M22i > v
    let inline m33i (v : ^a) = conv< ^a, M33i > v
    let inline m34i (v : ^a) = conv< ^a, M34i > v
    let inline m44i (v : ^a) = conv< ^a, M44i > v

    let inline m22l (v : ^a) = conv< ^a, M22l > v
    let inline m33l (v : ^a) = conv< ^a, M33l > v
    let inline m34l (v : ^a) = conv< ^a, M34l > v
    let inline m44l (v : ^a) = conv< ^a, M44l > v
