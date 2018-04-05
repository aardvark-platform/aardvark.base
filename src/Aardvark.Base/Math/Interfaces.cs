using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region IBoundingCircle2d

    public interface IBoundingCircle2d
    {
        Circle2d BoundingCircle2d { get; }
    }

    #endregion

    #region IBoundingSphere3d

    public interface IBoundingSphere3d
    {
        Sphere3d BoundingSphere3d { get; }
    }

    #endregion

    #region IPolygon<T>

    public interface IPolygon<T>
    {
        int PointCount { get; }
        T this[int index] { get; } // counter clockwise access
    }

    #endregion

    #region IMutablePolygon<T>

    public interface IMutablePolygon<T> : IPolygon<T>
    {
        void Set(int index, T value);
    }

    #endregion
}
