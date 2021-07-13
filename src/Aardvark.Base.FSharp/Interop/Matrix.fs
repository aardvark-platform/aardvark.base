namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Mat =

    [<AutoOpen>]
    module private Aux =
        let inline transposeAux (_ : ^z) (m : ^Matrix)  =
            ((^z or ^Matrix) : (static member Transposed : ^Matrix -> ^Matrix') m)

        let inline determinantAux (_ : ^z) (m : ^Matrix)  =
            ((^z or ^Matrix) : (static member Determinant : ^Matrix -> ^Scalar) m)

        let inline inverseAux (_ : ^z) (m : ^Matrix)  =
            ((^z or ^Matrix) : (static member Inverse : ^Matrix -> ^Matrix) m)

        let inline transformAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
            ((^z or ^Matrix or ^Vector) : (static member Transform : ^Matrix * ^Vector -> ^Vector') (m, v))

        let inline transformDirAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
            ((^z or ^Matrix or ^Vector) : (static member TransformDir : ^Matrix * ^Vector -> ^Vector) (m, v))

        let inline transformPosAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
            ((^z or ^Matrix or ^Vector) : (static member TransformPos : ^Matrix * ^Vector -> ^Vector) (m, v))

        let inline transformPosProjAux (_ : ^z) (m : ^Matrix) (v : ^Vector) =
            ((^z or ^Matrix or ^Vector) : (static member TransformPosProj : ^Matrix * ^Vector -> ^Vector) (m, v))

        let inline anyEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnyEqual : ^a * ^b -> bool) (a, b))

        let inline anyDifferentAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnyDifferent : ^a * ^b -> bool) (a, b))

        let inline anySmallerAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnySmaller : ^a * ^b -> bool) (a, b))

        let inline anyGreaterAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnyGreater : ^a * ^b -> bool) (a, b))

        let inline anySmallerOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnySmallerOrEqual : ^a * ^b -> bool) (a, b))

        let inline anyGreaterOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnyGreaterOrEqual : ^a * ^b -> bool) (a, b))

        let inline allEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllEqual : ^a * ^b -> bool) (a, b))

        let inline allDifferentAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllDifferent : ^a * ^b -> bool) (a, b))

        let inline allSmallerAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllSmaller : ^a * ^b -> bool) (a, b))

        let inline allGreaterAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllGreater : ^a * ^b -> bool) (a, b))

        let inline allSmallerOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllSmallerOrEqual : ^a * ^b -> bool) (a, b))

        let inline allGreaterOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllGreaterOrEqual : ^a * ^b -> bool) (a, b))

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

    /// Returns if a = b for any component. One or both of a and b have to be a matrix.
    let inline anyEqual a b =
        anyEqualAux Unchecked.defaultof<Mat> a b

    /// Returns if a <> b for any component. One or both of a and b have to be a matrix.
    let inline anyDifferent a b =
        anyDifferentAux Unchecked.defaultof<Mat> a b

    /// Returns if a = b for all components. One or both of a and b have to be a matrix.
    let inline allEqual a b =
        allEqualAux Unchecked.defaultof<Mat> a b

    /// Returns if a <> b for all components. One or both of a and b have to be a matrix.
    let inline allDifferent a b =
        allDifferentAux Unchecked.defaultof<Mat> a b

    /// Returns if a < b for any component. One or both of a and b have to be a matrix.
    let inline anySmaller a b =
        anySmallerAux Unchecked.defaultof<Mat> a b

    /// Returns if a > b for any component. One or both of a and b have to be a matrix.
    let inline anyGreater a b =
        anyGreaterAux Unchecked.defaultof<Mat> a b

    /// Returns if a < b for all components. One or both of a and b have to be a matrix.
    let inline allSmaller a b =
        allSmallerAux Unchecked.defaultof<Mat> a b

    /// Returns if a > b for all components. One or both of a and b have to be a matrix.
    let inline allGreater a b =
        allGreaterAux Unchecked.defaultof<Mat> a b

    /// Returns if a <= b for any component. One or both of a and b have to be a matrix.
    let inline anySmallerOrEqual a b =
        anySmallerOrEqualAux Unchecked.defaultof<Mat> a b

    /// Returns if a >= b for any component. One or both of a and b have to be a matrix.
    let inline anyGreaterOrEqual a b =
        anyGreaterOrEqualAux Unchecked.defaultof<Mat> a b

    /// Returns if a <= b for all components. One or both of a and b have to be a matrix.
    let inline allSmallerOrEqual a b =
        allSmallerOrEqualAux Unchecked.defaultof<Mat> a b

    /// Returns if a >= b for all components. One or both of a and b have to be a matrix.
    let inline allGreaterOrEqual a b =
        allGreaterOrEqualAux Unchecked.defaultof<Mat> a b

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

        let comparisonsWorking () =
            let a : bool = anyEqual M34i.Zero M34i.Zero
            let a : bool = anyEqual M34i.Zero 0
            let a : bool = anyEqual 1 M34i.Zero
            let a : bool = anyDifferent M34i.Zero M34i.Zero
            let a : bool = anyDifferent M34i.Zero 0
            let a : bool = anyDifferent 1 M34i.Zero
            let a : bool = anySmaller M34i.Zero M34i.Zero
            let a : bool = anySmaller M34i.Zero 0
            let a : bool = anySmaller 1 M34i.Zero
            let a : bool = anyGreater M34i.Zero M34i.Zero
            let a : bool = anyGreater M34i.Zero 0
            let a : bool = anyGreater 1 M34i.Zero
            let a : bool = anySmallerOrEqual M34i.Zero M34i.Zero
            let a : bool = anySmallerOrEqual M34i.Zero 0
            let a : bool = anySmallerOrEqual 1 M34i.Zero
            let a : bool = anyGreaterOrEqual M34i.Zero M34i.Zero
            let a : bool = anyGreaterOrEqual M34i.Zero 0
            let a : bool = anyGreaterOrEqual 1 M34i.Zero
            let a : bool = allEqual M34i.Zero M34i.Zero
            let a : bool = allEqual M34i.Zero 0
            let a : bool = allEqual 1 M34i.Zero
            let a : bool = allDifferent M34i.Zero M34i.Zero
            let a : bool = allDifferent M34i.Zero 0
            let a : bool = allDifferent 1 M34i.Zero
            let a : bool = allSmaller M34i.Zero M34i.Zero
            let a : bool = allSmaller M34i.Zero 0
            let a : bool = allSmaller 1 M34i.Zero
            let a : bool = allGreater M34i.Zero M34i.Zero
            let a : bool = allGreater M34i.Zero 0
            let a : bool = allGreater 1 M34i.Zero
            let a : bool = allSmallerOrEqual M34i.Zero M34i.Zero
            let a : bool = allSmallerOrEqual M34i.Zero 0
            let a : bool = allSmallerOrEqual 1 M34i.Zero
            let a : bool = allGreaterOrEqual M34i.Zero M34i.Zero
            let a : bool = allGreaterOrEqual M34i.Zero 0
            let a : bool = allGreaterOrEqual 1 M34i.Zero
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