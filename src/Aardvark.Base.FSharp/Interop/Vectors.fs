namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vec =

    let inline dot< ^a, ^b when ^a : (static member Dot : ^a -> ^a -> ^b)> (a : ^a) (b : ^a) =
        (^a : (static member Dot : ^a -> ^a -> ^b) (a,b))

    let inline cross< ^a, ^b when ^a : (static member Cross : ^a -> ^a -> ^b)> (a : ^a) (b : ^a) =
        (^a : (static member Cross : ^a -> ^a -> ^b) (a,b))

    let inline length< ^a, ^b when ^a : (member Length : ^b )> (v : ^a) =
        (^a : (member Length : ^b ) (v))

    let inline lengthSquared< ^a, ^b when ^a : (member LengthSquared : ^b )> (v : ^a) =
        (^a : (member LengthSquared : ^b ) (v))

    let inline normalize< ^a, ^b when ^a : (member Normalized : ^b )> (v : ^a) =
        (^a : (member Normalized : ^b ) v)

    let inline reflect< ^a, ^b when ^a : (member Reflected : ^a -> ^a)> (v : ^a) (n : ^a) : ^a =
        (^a : (member Reflected : ^a -> ^a ) (v, n))

    let inline refract< ^a, ^b when ^a : (member Refracted : ^a * ^b -> ^a)> (v : ^a) (n : ^a) (eta : ^b) =
        (^a : (member Refracted : ^a * ^b -> ^a ) (v, n, eta))

    let inline x< ^a, ^b when ^a : (member X : ^b)> (v : ^a) =
        (^a : (member X : ^b) v)

    let inline y< ^a, ^b when ^a : (member Y : ^b)> (v : ^a) =
        (^a : (member Y : ^b) v)

    let inline z< ^a, ^b when ^a : (member Z : ^b)> (v : ^a) =
        (^a : (member Z : ^b) v)

    let inline xy< ^a, ^b when ^a : (member XY : ^b)> (v : ^a) =
        (^a : (member XY : ^b) v)

    let inline yz< ^a, ^b when ^a : (member YZ : ^b)> (v : ^a) =
        (^a : (member YZ : ^b) v)

    let inline zw< ^a, ^b when ^a : (member ZW : ^b)> (v : ^a) =
        (^a : (member ZW : ^b) v)

    let inline xyz< ^a, ^b when ^a : (member XYZ : ^b)> (v : ^a) =
        (^a : (member XYZ : ^b) v)

    let inline yzw< ^a, ^b when ^a : (member YZW : ^b)> (v : ^a) =
        (^a : (member YZW : ^b) v)

    let inline anySmaller< ^a, ^b when ^a : (member AnySmaller : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AnySmaller : ^b -> bool) (v,value))

    let inline anyGreater< ^a, ^b when ^a : (member AnyGreater : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AnyGreater : ^b -> bool) (v,value))

    let inline allSmaller< ^a, ^b when ^a : (member AllSmaller : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AllSmaller : ^b -> bool) (v,value))

    let inline allGreater< ^a, ^b when ^a : (member AllGreater : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AllGreater : ^b -> bool) (v,value))

    let inline anySmallerOrEqual< ^a, ^b when ^a : (member AnySmallerOrEqual : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AnySmallerOrEqual : ^b -> bool) (v,value))

    let inline anyGreaterOrEqual< ^a, ^b when ^a : (member AnyGreaterOrEqual : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AnyGreaterOrEqual : ^b -> bool) (v,value))

    let inline allSmallerOrEqual< ^a, ^b when ^a : (member AllSmallerOrEqual : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AllSmallerOrEqual : ^b -> bool) (v,value))

    let inline allGreaterOrEqual< ^a, ^b when ^a : (member AllGreaterOrEqual : ^b -> bool)> (v : ^a) (value : ^b) =
        (^a : (member AllGreaterOrEqual : ^b -> bool) (v,value))