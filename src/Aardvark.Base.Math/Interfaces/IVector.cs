using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    /// <summary>
    /// Non-generic Interface for Dim - dimensional vectors.
    /// 
    /// </summary>
    public interface IVector
    {
        long Dim { get; }
        object GetValue(long index);
        void SetValue(object value, long index);
    }

    /// <summary>
    /// Generic Interface for N-dimensional vector of Type T.
    /// The indexer of this interface has arguments of type long.
    /// </summary>
    public interface IVector<T> : IVector
    {
        T this[long i] { get; set; }
    }

}
