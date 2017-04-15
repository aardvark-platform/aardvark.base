using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    /// <summary>
    /// A two dimensional circle represented by center and radius.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Circle2d : IBoundingBox2d, IValidity
    {
        public V2d Center;
        public double Radius;

        #region Constructors

        public Circle2d(V2d center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        #endregion

        #region Constants

        public static readonly Circle2d Zero = new Circle2d(V2d.Zero, 0.0);
        public static readonly Circle2d Invalid = new Circle2d(V2d.NaN, -1.0);

        #endregion

        #region Properties

        public bool IsInvalid
        {
            get { return Radius < 0.0; }
        }

        public bool IsValid
        {
            get { return Radius >= 0.0; }
        }

        public double RadiusSquared
        {
            get { return Radius * Radius; }
        }

        public double Circumference
        {
            get { return 2.0 * Radius * Constant.Pi; }
        }

        public double Area
        {
            get { return Radius.Square() * Constant.Pi; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Calculates Hash-code of the given circle.
        /// </summary>
        /// <returns>Hash-code.</returns>
        public override int GetHashCode()
        {
            return HashCode.GetCombined(Center, Radius);
        }

        /// <summary>
        /// Checks if 2 objects are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public override bool Equals(object other)
        {
            if (other is Circle2d)
            {
                var value = (Circle2d)other;
                return (Center == value.Center) && (Radius == value.Radius);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);
        }

        public static Circle2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Circle2d(V2d.Parse(x[0]), double.Parse(x[1]));
        }

        #endregion

        #region IBoundingBox2d Members

        public Box2d BoundingBox2d
        {
            get
            {
                return new Box2d(
                    new V2d(Center.X - Radius, Center.Y - Radius),
                    new V2d(Center.X + Radius, Center.Y + Radius)
                    );
            }
        }

        #endregion
    }

    public static class Box2dExtensions
    {

        public static Circle2d GetBoundingCircle2d(this Box2d box)
        {
            return (box.IsInvalid ? Circle2d.Invalid
                : new Circle2d(box.Center, 0.5 * box.Size.Length));
        }

    }
}
