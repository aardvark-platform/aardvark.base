using Aardvark.Base;

namespace Aardvark.Base
{
    /// <summary>
    /// Position (x,y) in normalized device coordinates.
    /// The canonical view volume is defined as [(-1, -1, 0), (+1, +1, 1)].
    /// </summary>
    public struct Ndc2d
    {
        /// <summary>
        /// Normalized device coordinates.
        /// </summary>
        public readonly V2d Position;

        /// <summary>
        /// Position (xy) in normalized device coordinates.
        /// </summary>
        public Ndc2d(V2d position)
        {
            Position = position;
        }

        /// <summary>
        /// Position (xy) in normalized device coordinates.
        /// </summary>
        public Ndc2d(double x, double y)
        {
            Position = new V2d(x, y);
        }

        /// <summary>
        /// Position (xy) in normalized device coordinates.
        /// </summary>
        public Ndc2d(PixelPosition p)
        {
            var np = p.NormalizedPosition;
            Position = new V2d(
                -1.0 + np.X * 2.0,
                +1.0 - np.Y * 2.0
                );
        }
    }

    /// <summary>
    /// Position (xyz) in normalized device coordinates.
    /// The canonical view volume is defined as [(-1, -1, 0), (+1, +1, 1)].
    /// </summary>
    public struct Ndc3d
    {
        /// <summary>
        /// Normalized device coordinates.
        /// </summary>
        public readonly V3d Position;

        /// <summary>
        /// Position (xyz) in normalized device coordinates.
        /// </summary>
        public Ndc3d(V3d position)
        {
            Position = position;
        }

        /// <summary>
        /// Position (xy) in normalized device coordinates.
        /// </summary>
        public Ndc3d(double x, double y, double z)
        {
            Position = new V3d(x, y, z);
        }
    }
}
