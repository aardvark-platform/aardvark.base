using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    /// <summary>
    /// An integral pixel position relative to specified bounds.
    /// The x-axis goes from left to right and y-axis from top to bottom.
    /// </summary>
    public struct PixelPosition
    {
        /// <summary>
        /// X goes from left to right and y from top to bottom.
        /// Integral pixel positions correspond to pixel centers.
        /// </summary>
        public readonly V2i Position;

        /// <summary>
        /// Minimum is top left pixel (inclusive) and maximum is bottom right pixel (exclusive).
        /// E.g. with bounds [(0, 0), (800, 600)] the top left pixel is (0,0) and the bottom
        /// right pixel is (799,599).
        /// </summary>
        public readonly Box2i Bounds;

        /// <summary>
        /// Pixel position relative to specified bounds.
        /// For example, a typical render window has bounds [(0, 0), (width, height)].
        /// </summary>
        public PixelPosition(V2i position, Box2i bounds)
        {
            Position = position;
            Bounds = bounds;
        }

        /// <summary>
        /// Pixel position relative to specified bounds.
        /// For example, a typical render window has bounds [(0, 0), (width, height)].
        /// </summary>
        public PixelPosition(V2i position, int width, int height)
        {
            Position = position;
            Bounds = Box2i.FromSize(width, height);
        }

        /// <summary>
        /// Pixel position relative to specified bounds.
        /// For example, a typical render window has bounds [(0, 0), (width, height)].
        /// </summary>
        public PixelPosition(int x, int y, int width, int height)
        {
            Position = new V2i(x, y);
            Bounds = Box2i.FromSize(width, height);
        }



        /// <summary>
        /// PixelPosition from normalized device coordinates.
        /// </summary>
        public PixelPosition(Ndc2d ndc, V2i renderTargetSize)
        {
            var d = (V2d)renderTargetSize / 2.0;
            Position = new V2i(
                (ndc.Position.X + 1.0) * d.X,
                (1.0 - ndc.Position.Y) * d.Y
                );
            Bounds = Box2i.FromSize(renderTargetSize);
        }

        /// <summary>
        /// PixelPosition from normalized device coordinates.
        /// </summary>
        public PixelPosition(Ndc3d ndc, V2i renderTargetSize)
        {
            var d = (V2d)renderTargetSize / 2.0;
            Position = new V2i(
                (ndc.Position.X + 1.0) * d.X,
                (1.0 - ndc.Position.Y) * d.Y
                );
            Bounds = Box2i.FromSize(renderTargetSize);
        }


        /// <summary>
        /// Maps integral pixel position to range ](0,0), (1,1)[,
        /// where integral pixel positions correspond to pixel centers.
        /// E.g. all pixel mappings for Bounds [(0,0), (3,2)] are as follows:
        /// (0,0) -> (0.17, 0.25)
        /// (1,0) -> (0.50, 0.25)
        /// (2,0) -> (0.83, 0.25)
        /// (0,1) -> (0.17, 0.75)
        /// (0,2) -> (0.50, 0.75)
        /// (0,3) -> (0.83, 0.75)
        /// </summary>
        public V2d NormalizedPosition
        {
            get
            {
                return new V2d(
                    (Position.X - Bounds.Min.X + 0.5) / (Bounds.Max.X - Bounds.Min.X),
                    (Position.Y - Bounds.Min.Y + 0.5) / (Bounds.Max.Y - Bounds.Min.Y)
                    );
            }
        }
    }
}
