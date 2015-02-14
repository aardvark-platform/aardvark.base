using System;

namespace Aardvark.Base
{
    public enum IndexedGeometryMode
    {
        PointList = 0,
        LineStrip = 1,
        LineList = 2,
        TriangleStrip = 3,
        TriangleList = 4,
    }

    public class IndexedGeometry
    {
        public IndexedGeometryMode Mode;
        public Array IndexArray;
        public SymbolDict<Array> IndexedAttributes;
        public SymbolDict<object> SingleAttributes;

        #region Constructors

        public IndexedGeometry()
            : this(IndexedGeometryMode.TriangleList, null, null, null)
        { }

        public IndexedGeometry(
                Array indexArray,
                SymbolDict<Array> indexedAttributes,
                SymbolDict<object> singleAttributes)
            : this(IndexedGeometryMode.TriangleList, indexArray,
                    indexedAttributes, singleAttributes)
        { }

        public IndexedGeometry(
                IndexedGeometryMode mode,
                Array indexArray,
                SymbolDict<Array> indexedAttributes,
                SymbolDict<object> singleAttributes)
        {
            Mode = mode;
            IndexArray = indexArray;
            IndexedAttributes = indexedAttributes;
            SingleAttributes = singleAttributes;
        }

        #endregion

        #region Properties

        public bool IsIndexed { get { return IndexArray != null; } }

        #endregion
    }
}