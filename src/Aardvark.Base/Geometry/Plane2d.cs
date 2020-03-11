using System.Globalization;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// A line represented by a (possibly) normalized normal vector and the
    /// distance to the origin. Note that the plane does not enforce the
    /// normalized normal vector.
    /// Equation for points p on the plane: Normal dot p == Distance
    /// </summary>
    public struct Plane2d : IValidity // should be InfiniteLine2d
    {
        public V2d Normal;
        public double Distance;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and constant. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        public Plane2d(V2d normalizedNormal, double distance)
        {
            Normal = normalizedNormal;
            Distance = distance;
        }

        /// <summary>
        /// Creates plane from point and normal vector. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        public Plane2d(V2d normalizedNormal, V2d point)
        {
            Normal = normalizedNormal;
            Distance = Vec.Dot(normalizedNormal, point);
        }

        #endregion

        #region Constants

        public static readonly Plane2d XAxis = new Plane2d(V2d.OI, 0.0);
        public static readonly Plane2d YAxis = new Plane2d(-V2d.IO, 0.0);
        public static readonly Plane2d Invalid = new Plane2d(V2d.Zero, 0.0);

        #endregion

        #region Properties

        /// <summary>
        /// The point on the plane which is closest to the origin.
        /// </summary>
        public V2d Point
        {
            get { return Normal * Distance; }
            set { Distance = Vec.Dot(Normal, value); }
        }

        /// <summary>
        /// Returns true if the normal of the plane is not the zero-vector.
        /// </summary>
        public bool IsValid => Normal != V2d.Zero;

        /// <summary>
        /// Returns true if the normal of the plane is the zero-vector.
        /// </summary>
        public bool IsInvalid => Normal == V2d.Zero;

        /// <summary>
        /// Returns a Plane3d whose cutting-line with the XY-Plane 
        /// is represented by the Plane2d
        /// </summary>
        public Plane3d PlaneXY => new Plane3d(Normal.XYO, Distance);

        /// <summary>
        /// Returns a Plane3d whose cutting-line with the XZ-Plane 
        /// is represented by the Plane2d
        /// </summary>
        public Plane3d PlaneXZ => new Plane3d(Normal.XOY, Distance);

        /// <summary>
        /// Returns a Plane3d whose cutting-line with the YZ-Plane 
        /// is represented by the Plane2d
        /// </summary>
        public Plane3d PlaneYZ => new Plane3d(Normal.OXY, Distance);

        #endregion

        #region Plane Arithmetics

        /// <summary>
        /// Returns the normalized <see cref="Plane2d"/> as new <see cref="Plane2d"/>.
        /// </summary>
        public Plane2d Normalized
        {
            get
            {
                double scale = Normal.Length;
                return new Plane2d(Normal / scale, Distance / scale);
            }
        }

        /// <summary>
        /// Calculates the nomalized plane of this <see cref="Plane2d"/>.
        /// </summary>
        public void Normalize()
        {
            double scale =  Normal.Length;
            Normal /= scale;
            Distance /= scale;
        }

        /// <summary>
        /// Changes sign of normal vector
        /// </summary>
        public void Reverse()
        {
            Normal = -Normal;
            Distance = -Distance;
        }

        /// <summary>
        /// Returns <see cref="Plane2d"/> with normal vector in opposing direction.
        /// </summary>
        /// <returns></returns>
        public Plane2d Reversed => new Plane2d(-Normal, -Distance);

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// </summary>
        public double Height(V2d p) => Vec.Dot(Normal, p) - Distance;

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        public int Sign(V2d p) => Height(p).Sign();

        /// <summary>
        /// Projets the given point x perpendicular on the plane
        /// and returns the nearest point on the plane.
        /// </summary>
        public V2d NearestPoint(V2d x)
        {
            var p = Point;
            return (x - Normal.Dot(x - p) * Normal);
        }

        /// <summary>
        /// Returns the coefficients (a, b, c, d) of the normal equation.
        /// </summary>
        public V3d Coefficients => new V3d(Normal, -Distance);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Normal, Distance);

        public override bool Equals(object other) => (other is Plane2d value)
            ? (Normal == value.Normal) && (Distance == value.Distance)
            : false;

        public override string ToString() => 
            string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Distance);

        public static Plane2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Plane2d(V2d.Parse(x[0]), double.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
