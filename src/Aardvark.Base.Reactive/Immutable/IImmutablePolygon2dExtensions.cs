using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Extensions for IImmutablePolygon2d.
    /// </summary>
    public static class IImmutablePolygon2dExtensions
    {
        /// <summary>
        /// Ensures that the outline is oriented counter-clockwise.
        /// </summary>
        public static IImmutablePolygon2d ToCounterClockwise(this IImmutablePolygon2d self)
        {
            return self.ToPolygon2d().IsCcw() ? self : new ImmutablePolygon2d(self.Points.Reverse());
        }

        /// <summary>
        /// Ensures that the outline is oriented clockwise.
        /// </summary>
        public static IImmutablePolygon2d ToClockwise(this IImmutablePolygon2d self)
        {
            return self.ToPolygon2d().IsCcw() ? new ImmutablePolygon2d(self.Points.Reverse()) : self;
        }
    }
}
