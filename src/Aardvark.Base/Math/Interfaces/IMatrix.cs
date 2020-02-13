using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    /// <summary>
     /// Non-generic Interface for Dim.X x Dim.Y - dimensional matrices.
     /// </summary>
    public interface IMatrix
    {
        V2l Dim { get; }
        object GetValue(long x, long y);
        void SetValue(object value, long x, long y);
        object GetValue(V2l v);
        void SetValue(object value, V2l v);
    }

    /// <summary>
    /// Generic Interface for NxM-dimensional matrix of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface IMatrix<T> : IMatrix
    {
        T this[long x, long y] { get; set; }
        T this[V2l v] { get; set; }
    }
}
