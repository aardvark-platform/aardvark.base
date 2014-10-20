namespace Aardvark.Base

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


module Trafo =
    
    let inline forward (t : Trafo3d) = t.Forward
    let inline backward (t : Trafo3d) = t.Backward