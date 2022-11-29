using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Plane2f

    /// <summary>
    /// A line represented by a (possibly) normalized normal vector and the
    /// distance to the origin. Note that the plane does not enforce the
    /// normalized normal vector.
    /// Equation for points p on the plane: Normal dot p == Distance
    /// </summary>
    public partial struct Plane2f : IEquatable<Plane2f>, IValidity // should be InfiniteLine2d
    {
        public V2f Normal;
        public float Distance;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and constant. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane2f(V2f normalizedNormal, float distance)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane2f(V2f normalizedNormal, V2f point)
        {
            Normal = normalizedNormal;
            Distance = Vec.Dot(normalizedNormal, point);
        }

        /// <summary>
        /// Creates a <see cref="Plane2f"/> from another <see cref="Plane2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane2f(Plane2f o)
        {
            Normal = o.Normal;
            Distance = o.Distance;
        }

        /// <summary>
        /// Creates a <see cref="Plane2f"/> from a <see cref="Plane2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane2f(Plane2d o)
        {
            Normal = (V2f)o.Normal;
            Distance = (float)o.Distance;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Plane2f(Plane2d o)
            => new Plane2f(o);

        #endregion

        #region Constants

        public static Plane2f XAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2f(V2f.OI, 0);
        }

        public static Plane2f YAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2f(-V2f.IO, 0);
        }

        public static Plane2f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2f(V2f.Zero, 0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The point on the plane which is closest to the origin.
        /// </summary>
        [XmlIgnore]
        public V2f Point
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Normal * Distance; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Distance = Vec.Dot(Normal, value); }
        }

        /// <summary>
        /// Returns true if the normal of the plane is not the zero-vector.
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal != V2f.Zero;
        }

        /// <summary>
        /// Returns true if the normal of the plane is the zero-vector.
        /// </summary>
        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == V2f.Zero;
        }

        /// <summary>
        /// Returns a Plane3f whose cutting-line with the XY-Plane
        /// is represented by the Plane2f
        /// </summary>
        public Plane3f PlaneXY
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3f(Normal.XYO, Distance);
        }

        /// <summary>
        /// Returns a Plane3f whose cutting-line with the XZ-Plane
        /// is represented by the Plane2f
        /// </summary>
        public Plane3f PlaneXZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3f(Normal.XOY, Distance);
        }

        /// <summary>
        /// Returns a Plane3f whose cutting-line with the YZ-Plane
        /// is represented by the Plane2f
        /// </summary>
        public Plane3f PlaneYZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3f(Normal.OXY, Distance);
        }

        #endregion

        #region Plane Arithmetics

        /// <summary>
        /// Returns the normalized <see cref="Plane2f"/> as new <see cref="Plane2f"/>.
        /// </summary>
        public Plane2f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                float scale = Normal.Length;
                return new Plane2f(Normal / scale, Distance / scale);
            }
        }

        /// <summary>
        /// Calculates the nomalized plane of this <see cref="Plane2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float scale =  Normal.Length;
            Normal /= scale;
            Distance /= scale;
        }

        /// <summary>
        /// Changes sign of normal vector
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            Normal = -Normal;
            Distance = -Distance;
        }

        /// <summary>
        /// Returns <see cref="Plane2f"/> with normal vector in opposing direction.
        /// </summary>
        /// <returns></returns>
        public Plane2f Reversed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2f(-Normal, -Distance);
        }

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// IMPORTANT: If the plane is not normalized the returned height is scaled with the magnitued of the plane normal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Height(V2f p) => Vec.Dot(Normal, p) - Distance;

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign(V2f p) => Height(p).Sign();

        /// <summary>
        /// Projets the given point x perpendicular on the plane
        /// and returns the nearest point on the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V2f NearestPoint(V2f x)
        {
            var p = Point;
            return (x - Normal.Dot(x - p) * Normal);
        }

        /// <summary>
        /// Returns the coefficients (a, b, c, d) of the normal equation.
        /// </summary>
        public V3f Coefficients
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new V3f(Normal, -Distance);
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Plane2f a, Plane2f b)
            => (a.Normal == b.Normal) && (a.Distance == b.Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Plane2f a, Plane2f b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Plane2f other)
            => Normal.Equals(other.Normal) && Distance.Equals(other.Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Plane2f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Plane2f(V2f.Parse(x[0]), float.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Plane2f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Plane2f a, Plane2f b, float tolerance) =>
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Distance, b.Distance, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Plane2f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Plane2f a, Plane2f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Plane2d

    /// <summary>
    /// A line represented by a (possibly) normalized normal vector and the
    /// distance to the origin. Note that the plane does not enforce the
    /// normalized normal vector.
    /// Equation for points p on the plane: Normal dot p == Distance
    /// </summary>
    public partial struct Plane2d : IEquatable<Plane2d>, IValidity // should be InfiniteLine2d
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane2d(V2d normalizedNormal, V2d point)
        {
            Normal = normalizedNormal;
            Distance = Vec.Dot(normalizedNormal, point);
        }

        /// <summary>
        /// Creates a <see cref="Plane2d"/> from another <see cref="Plane2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane2d(Plane2d o)
        {
            Normal = o.Normal;
            Distance = o.Distance;
        }

        /// <summary>
        /// Creates a <see cref="Plane2d"/> from a <see cref="Plane2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Plane2d(Plane2f o)
        {
            Normal = (V2d)o.Normal;
            Distance = (double)o.Distance;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Plane2d(Plane2f o)
            => new Plane2d(o);

        #endregion

        #region Constants

        public static Plane2d XAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2d(V2d.OI, 0);
        }

        public static Plane2d YAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2d(-V2d.IO, 0);
        }

        public static Plane2d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2d(V2d.Zero, 0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The point on the plane which is closest to the origin.
        /// </summary>
        [XmlIgnore]
        public V2d Point
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Normal * Distance; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Distance = Vec.Dot(Normal, value); }
        }

        /// <summary>
        /// Returns true if the normal of the plane is not the zero-vector.
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal != V2d.Zero;
        }

        /// <summary>
        /// Returns true if the normal of the plane is the zero-vector.
        /// </summary>
        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == V2d.Zero;
        }

        /// <summary>
        /// Returns a Plane3d whose cutting-line with the XY-Plane
        /// is represented by the Plane2d
        /// </summary>
        public Plane3d PlaneXY
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3d(Normal.XYO, Distance);
        }

        /// <summary>
        /// Returns a Plane3d whose cutting-line with the XZ-Plane
        /// is represented by the Plane2d
        /// </summary>
        public Plane3d PlaneXZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3d(Normal.XOY, Distance);
        }

        /// <summary>
        /// Returns a Plane3d whose cutting-line with the YZ-Plane
        /// is represented by the Plane2d
        /// </summary>
        public Plane3d PlaneYZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane3d(Normal.OXY, Distance);
        }

        #endregion

        #region Plane Arithmetics

        /// <summary>
        /// Returns the normalized <see cref="Plane2d"/> as new <see cref="Plane2d"/>.
        /// </summary>
        public Plane2d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                double scale = Normal.Length;
                return new Plane2d(Normal / scale, Distance / scale);
            }
        }

        /// <summary>
        /// Calculates the nomalized plane of this <see cref="Plane2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            double scale =  Normal.Length;
            Normal /= scale;
            Distance /= scale;
        }

        /// <summary>
        /// Changes sign of normal vector
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            Normal = -Normal;
            Distance = -Distance;
        }

        /// <summary>
        /// Returns <see cref="Plane2d"/> with normal vector in opposing direction.
        /// </summary>
        /// <returns></returns>
        public Plane2d Reversed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Plane2d(-Normal, -Distance);
        }

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// IMPORTANT: If the plane is not normalized the returned height is scaled with the magnitued of the plane normal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Height(V2d p) => Vec.Dot(Normal, p) - Distance;

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign(V2d p) => Height(p).Sign();

        /// <summary>
        /// Projets the given point x perpendicular on the plane
        /// and returns the nearest point on the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public V2d NearestPoint(V2d x)
        {
            var p = Point;
            return (x - Normal.Dot(x - p) * Normal);
        }

        /// <summary>
        /// Returns the coefficients (a, b, c, d) of the normal equation.
        /// </summary>
        public V3d Coefficients
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new V3d(Normal, -Distance);
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Plane2d a, Plane2d b)
            => (a.Normal == b.Normal) && (a.Distance == b.Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Plane2d a, Plane2d b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Plane2d other)
            => Normal.Equals(other.Normal) && Distance.Equals(other.Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is Plane2d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Plane2d(V2d.Parse(x[0]), double.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Plane2d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Plane2d a, Plane2d b, double tolerance) =>
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Distance, b.Distance, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Plane2d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Plane2d a, Plane2d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

}
