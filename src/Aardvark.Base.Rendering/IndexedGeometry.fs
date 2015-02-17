namespace Aardvark.Base

open System

type IndexedGeometryMode =
    | PointList = 0
    | LineStrip = 1
    | LineList = 2
    | TriangleStrip = 3
    | TriangleList = 4

type IndexedGeometry =
    class
        val mutable public Mode : IndexedGeometryMode
        val mutable public IndexArray : Array
        val mutable public IndexedAttributes : SymbolDict<Array>
        val mutable public SingleAttributes : SymbolDict<obj>

        member x.IsIndexed = x.IndexArray <> null

        new() = 
            { Mode = IndexedGeometryMode.TriangleList; 
              IndexArray = null; 
              IndexedAttributes = null; 
              SingleAttributes = null }

        new(indexArray, indexedAttributes, singleAttributes) = 
            { Mode = IndexedGeometryMode.TriangleList; 
              IndexArray = indexArray; 
              IndexedAttributes = indexedAttributes; 
              SingleAttributes = singleAttributes }

        new(mode, indexArray, indexedAttributes, singleAttributes) = 
            { Mode = mode
              IndexArray = indexArray; 
              IndexedAttributes = indexedAttributes; 
              SingleAttributes = singleAttributes }
    end
