namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Mat =

    let inline private transposeAux (_ : ^z) (m : ^Matrix)  =
        ((^z or ^Matrix) : (static member Transposed : ^Matrix -> ^Matrix') m)

    let inline private determinantAux (_ : ^z) (m : ^Matrix)  =
        ((^z or ^Matrix) : (static member Determinant : ^Matrix -> ^Scalar) m)

    let inline private inverseAux (_ : ^z) (m : ^Matrix)  =
        ((^z or ^Matrix) : (static member Inverse : ^Matrix -> ^Matrix) m)

    let inline private transformAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
        ((^z or ^Matrix or ^Vector) : (static member Transform : ^Matrix * ^Vector -> ^Vector') (m, v))

    let inline private transformDirAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
        ((^z or ^Matrix or ^Vector) : (static member TransformDir : ^Matrix * ^Vector -> ^Vector) (m, v))

    let inline private transformPosAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
        ((^z or ^Matrix or ^Vector) : (static member TransformPos : ^Matrix * ^Vector -> ^Vector) (m, v))

    let inline private transformPosProjAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
        ((^z or ^Matrix or ^Vector) : (static member TransformPosProj : ^Matrix * ^Vector -> ^Vector) (m, v))

    /// Returns the transpose of a matrix.
    let inline transpose (m : ^Matrix) : ^Matrix' =
        transposeAux Unchecked.defaultof<Mat> m

    /// Returns the determinant of a matrix.
    let inline det (m : ^Matrix) : ^Scalar =
        determinantAux Unchecked.defaultof<Mat> m

    /// Returns the inverse of a matrix.
    let inline inverse (m : ^Matrix) : ^Matrix =
        inverseAux Unchecked.defaultof<Mat> m

    /// Transforms a vector by a matrix.
    let inline transform (m : ^Matrix) (v : ^Vector) : ^Vector' =
        transformAux Unchecked.defaultof<Mat> m v

    /// Transforms a point vector by a matrix (the last element of the vector is presumed 1)
    let inline transformPos (m : ^Matrix) (v : ^Vector) : ^Vector =
        transformPosAux Unchecked.defaultof<Mat> m v

    /// Transforms a direction vector by a matrix (the last element of the vector is presumed 0)
    let inline transformDir (m : ^Matrix) (v : ^Vector) : ^Vector =
        transformDirAux Unchecked.defaultof<Mat> m v

    /// Transforms a point vector by a matrix (the last element of the vector is presumed 1).
    /// Projective transform is performed. Perspective Division is performed.
    let inline transformPosProj (m : ^Matrix) (v : ^Vector) : ^Vector =
        transformPosProjAux Unchecked.defaultof<Mat> m v

    module private CompilerTests =

        let working () =
            let a = (transpose M33d.Identity) * 0.5
            let a = (det M22l.Identity) * 5L
            let a = inverse M22f.Identity

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

    let inline private inverseAux (_ : ^z) (t : ^Trafo) =
        ((^z or ^Trafo) : (static member Inverse : ^Trafo -> ^Trafo) t)

    let inline private forwardAux (_ : ^z) (t : ^Trafo) =
        ((^z or ^Trafo) : (static member Forward : ^Trafo -> ^Matrix) t)

    let inline private backwardAux (_ : ^z) (t : ^Trafo) =
        ((^z or ^Trafo) : (static member Backward : ^Trafo -> ^Matrix) t)

    /// Returns the inverse of a transformation.
    let inline inverse (t : ^Trafo) : ^Trafo =
        t |> inverseAux Unchecked.defaultof<Trafo>

    /// Returns the forward matrix of a transformation.
    let inline forward (t : ^Trafo) : ^Matrix =
        t |> forwardAux Unchecked.defaultof<Trafo>

    /// Returns the backward matrix of a transformation.
    let inline backward (t : ^Trafo) : ^Matrix =
        t |> backwardAux Unchecked.defaultof<Trafo>

    module private CompilerTests =

        let working () =
            let a : Trafo3d = inverse Trafo3d.Identity
            let a = V3d.Zero |> Mat.transformPos (Trafo3d.Identity |> inverse |> forward)

            let a : M44d = forward Trafo3d.Identity
            let a : M33f = forward Trafo2f.Identity
            let a = (forward Trafo3d.Identity) * 5.0
            let a = V3d.Zero |> Mat.transformPos (Trafo3d.Identity |> forward)

            let a : M44d = backward Trafo3d.Identity
            let a : M33f = backward Trafo2f.Identity
            let a = (backward Trafo3d.Identity) * 5.0
            let a = V3d.Zero |> Mat.transformPos (Trafo3d.Identity |> backward)

            ()