namespace Aardvark.Base


type DrawCallInfo =
    struct
        val mutable public Mode : IndexedGeometryMode
        val mutable public FirstIndex : int
        val mutable public FaceVertexCount : int
        val mutable public FirstInstance : int
        val mutable public InstanceCount : int

    end