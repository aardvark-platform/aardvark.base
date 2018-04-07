using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    /// <summary>
    /// A Size2d reprents the size of an object in two dimensions.
    /// </summary>
    public interface ISize2d
    {
        V2d Size2d { get; }
    }

    /// <summary>
    /// A Size3d reprents the size of an object in three dimensions.
    /// </summary>
    public interface ISize3d
    {
        V3d Size3d { get; }
    }

    /// <summary>
    /// A Size4d reprents the size of an object in four dimensions.
    /// </summary>
    public interface ISize4d
    {
        V4d Size4d { get; }
    }
}
