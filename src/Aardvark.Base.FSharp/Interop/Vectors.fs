namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vec =

    [<AutoOpen>]
    module private Aux =
        let inline dotAux (_ : ^z) (a : ^a) (b : ^a) =
            ((^z or ^a) : (static member Dot : ^a * ^a -> ^b) (a, b))

        let inline crossAux (_ : ^z) (a : ^a) (b : ^a) =
            ((^z or ^a) : (static member Cross : ^a * ^a -> ^a) (a, b))

        let inline distanceAux (_ : ^z) (a : ^a) (b : ^a) =
            ((^z or ^a) : (static member Distance : ^a * ^a -> ^b) (a, b))

        let inline distanceSquaredAux (_ : ^z) (a : ^a) (b : ^a) =
            ((^z or ^a) : (static member DistanceSquared : ^a * ^a -> ^b) (a, b))

        let inline reflectAux (_ : ^z) (n : ^a) (v : ^a) =
            ((^z or ^a) : (static member Reflect : ^a * ^a -> ^a) (v, n))

        let inline refractAux (_ : ^z) (eta : ^b) (n : ^a) (v : ^a) =
            ((^z or ^a) : (static member Refract : ^a * ^a * ^b -> ^a) (v, n, eta))

        let inline anySmallerAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnySmaller : ^a * ^b -> bool) (a, b))

        let inline anyGreaterAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnyGreater : ^a * ^b -> bool) (a, b))

        let inline anySmallerOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnySmallerOrEqual : ^a * ^b -> bool) (a, b))

        let inline anyGreaterOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AnyGreaterOrEqual : ^a * ^b -> bool) (a, b))

        let inline allSmallerAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllSmaller : ^a * ^b -> bool) (a, b))

        let inline allGreaterAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllGreater : ^a * ^b -> bool) (a, b))

        let inline allSmallerOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllSmallerOrEqual : ^a * ^b -> bool) (a, b))

        let inline allGreaterOrEqualAux (_ : ^z) (a : ^a) (b : ^b) =
            ((^z or ^a or ^b) : (static member AllGreaterOrEqual : ^a * ^b -> bool) (a, b))

    /// Computes the dot product of two vectors a and b.
    let inline dot a b =
        dotAux Unchecked.defaultof<Vec> a b

    /// Computes the cross product of two 3D vectors a and b.
    let inline cross a b =
        crossAux Unchecked.defaultof<Vec> a b

    /// Computes the distance between a and b
    let inline distance a b =
        distanceAux Unchecked.defaultof<Vec> a b

    /// Computes the squared distance between a and b
    let inline distanceSquared a b =
        distanceSquaredAux Unchecked.defaultof<Vec> a b

    /// Computes the length of the vector v.
    let inline length< ^a, ^b when ^a : (member Length : ^b )> (v : ^a) =
        (^a : (member Length : ^b ) (v))

    /// Computes the squared length of the vector v.
    let inline lengthSquared< ^a, ^b when ^a : (member LengthSquared : ^b )> (v : ^a) =
        (^a : (member LengthSquared : ^b ) (v))

    /// Returns the vector v with unit length.
    let inline normalize< ^a, ^b when ^a : (member Normalized : ^b )> (v : ^a) =
        (^a : (member Normalized : ^b ) v)

    /// Returns the reflection direction of v for the normal n (should be normalized).
    let inline reflect n v =
        reflectAux Unchecked.defaultof<Vec> n v

    /// Returns the refraction direction of v for the normal n and ratio of refraction indices eta.
    /// v and n should be normalized.
    let inline refract eta n v =
        refractAux Unchecked.defaultof<Vec> eta n v

    /// Returns the x-component of the vector v.
    let inline x< ^a, ^b when ^a : (member P_X : ^b)> (v : ^a) =
        (^a : (member P_X : ^b) v)

    /// Returns the y-component of the vector v.
    let inline y< ^a, ^b when ^a : (member P_Y : ^b)> (v : ^a) =
        (^a : (member P_Y : ^b) v)

    /// Returns the z-component of the vector v.
    let inline z< ^a, ^b when ^a : (member P_Z : ^b)> (v : ^a) =
        (^a : (member P_Z : ^b) v)

    /// Returns the xy-components of the vector v.
    let inline xy< ^a, ^b when ^a : (member XY : ^b)> (v : ^a) =
        (^a : (member XY : ^b) v)

    /// Returns the yz-components of the vector v.
    let inline yz< ^a, ^b when ^a : (member YZ : ^b)> (v : ^a) =
        (^a : (member YZ : ^b) v)

    /// Returns the zw-components of the vector v.
    let inline zw< ^a, ^b when ^a : (member ZW : ^b)> (v : ^a) =
        (^a : (member ZW : ^b) v)

    /// Returns the xyz-components of the vector v.
    let inline xyz< ^a, ^b when ^a : (member XYZ : ^b)> (v : ^a) =
        (^a : (member XYZ : ^b) v)

    /// Returns the yzw-components of the vector v.
    let inline yzw< ^a, ^b when ^a : (member YZW : ^b)> (v : ^a) =
        (^a : (member YZW : ^b) v)

    /// Returns if a < b for any component. One or both of a and b have to be a vector.
    let inline anySmaller a b =
        anySmallerAux Unchecked.defaultof<Vec> a b

    /// Returns if a > b for any component. One or both of a and b have to be a vector.
    let inline anyGreater a b =
        anyGreaterAux Unchecked.defaultof<Vec> a b

    /// Returns if a < b for all components. One or both of a and b have to be a vector.
    let inline allSmaller a b =
        allSmallerAux Unchecked.defaultof<Vec> a b

    /// Returns if a > b for all components. One or both of a and b have to be a vector.
    let inline allGreater a b =
        allGreaterAux Unchecked.defaultof<Vec> a b

    /// Returns if a <= b for any component. One or both of a and b have to be a vector.
    let inline anySmallerOrEqual a b =
        anySmallerOrEqualAux Unchecked.defaultof<Vec> a b

    /// Returns if a >= b for any component. One or both of a and b have to be a vector.
    let inline anyGreaterOrEqual a b =
        anyGreaterOrEqualAux Unchecked.defaultof<Vec> a b

    /// Returns if a <= b for all components. One or both of a and b have to be a vector.
    let inline allSmallerOrEqual a b =
        allSmallerOrEqualAux Unchecked.defaultof<Vec> a b

    /// Returns if a >= b for all components. One or both of a and b have to be a vector.
    let inline allGreaterOrEqual a b =
        allGreaterOrEqualAux Unchecked.defaultof<Vec> a b

    module private CompilerTests =

        let dotWorking () =
            let a : float = dot V3d.One V3d.Zero
            let a : int = dot V3i.One V3i.Zero
            ()
        
        let crossWorking () =
            let a : V3d = cross V3d.One V3d.Zero
            let a : V3i = cross V3i.One V3i.Zero
            ()

        let distanceWorking () =
            let a : float = distance V3d.One V3d.Zero
            let a : float32 = distance V3f.One V3f.Zero
            let a : float = distance V3i.One V3i.Zero
            ()

        let distanceSquaredWorking () =
            let a : float = distanceSquared V3d.One V3d.Zero
            let a : float32 = distanceSquared V3f.One V3f.Zero
            let a : int = distanceSquared V3i.One V3i.Zero
            ()

        let lengthWorking () =
            let a : float32 = length V3f.One
            let a : float = length V3d.One
            let a : float = length V3i.One
            ()

        let lengthSquaredWorking () =
            let a : float32 = lengthSquared V3f.One
            let a : float = lengthSquared V3d.One
            let a : float = lengthSquared V3i.One
            ()

        let normalizeWorking () =
            let a : V3f = normalize V3f.One
            let a : V3d = normalize V3d.One
            let a : V3d = normalize V3i.One
            ()

        let reflectWorking () =
            let a : V3f = reflect V3f.One V3f.Zero
            let a : V3d = reflect V3d.One V3d.Zero
            ()
            
        let refractWorking () =
            let a : V3f = refract 0.0f V3f.One V3f.Zero 
            let a : V3d = refract 0.0 V3d.One V3d.Zero 
            ()

        let swizzlesWorking () =
            let a : float = x V3d.One
            let a : float = y V3d.One
            let a : float = z V3d.One
            let a : V2d = xy V3d.One
            let a : V2d = yz V3d.One
            let a : V2d = zw V4d.One
            let a : V3d = xyz V4d.One
            let a : V3d = yzw V4d.One
            ()

        let comparisonsWorking () =
            let a : bool = anySmaller V3i.One V3i.Zero
            let a : bool = anySmaller V3i.One 0
            let a : bool = anySmaller 1 V3i.Zero
            let a : bool = anyGreater V3i.One V3i.Zero
            let a : bool = anyGreater V3i.One 0
            let a : bool = anyGreater 1 V3i.Zero
            let a : bool = anySmallerOrEqual V3i.One V3i.Zero
            let a : bool = anySmallerOrEqual V3i.One 0
            let a : bool = anySmallerOrEqual 1 V3i.Zero
            let a : bool = anyGreaterOrEqual V3i.One V3i.Zero
            let a : bool = anyGreaterOrEqual V3i.One 0
            let a : bool = anyGreaterOrEqual 1 V3i.Zero
            let a : bool = allSmaller V3i.One V3i.Zero
            let a : bool = allSmaller V3i.One 0
            let a : bool = allSmaller 1 V3i.Zero
            let a : bool = allGreater V3i.One V3i.Zero
            let a : bool = allGreater V3i.One 0
            let a : bool = allGreater 1 V3i.Zero
            let a : bool = allSmallerOrEqual V3i.One V3i.Zero
            let a : bool = allSmallerOrEqual V3i.One 0
            let a : bool = allSmallerOrEqual 1 V3i.Zero
            let a : bool = allGreaterOrEqual V3i.One V3i.Zero
            let a : bool = allGreaterOrEqual V3i.One 0
            let a : bool = allGreaterOrEqual 1 V3i.Zero
            ()