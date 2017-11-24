namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Mat =
    let inline transpose (m : ^a) : ^b =
        (^a : (member Transposed : ^b) m)

    let inline det (m : ^a) : ^b =
        (^a : (member Det : ^b) m)

    let inline inverse (m : ^a) : ^b =
        (^a : (member Inverse : ^b) m)

    let inline transformPos (m : ^a) (v : ^b) : ^b =
        (^a : (member TransformPos : ^b -> ^b) (m,v))

    let inline transformDir (m : ^a) (v : ^b) : ^b =
        (^a : (member TransformDir : ^b -> ^b) (m,v))

    let inline transformPosProj (m : ^a) (v : ^b) : ^b =
        (^a : (member TransformPosProj : ^b -> ^b) (m,v))

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
