using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var plane2t = "Plane2" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var plane2t2 = "Plane2" + tc2;
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    #region __plane2t__

    /// <summary>
    /// A line represented by a (possibly) normalized normal vector and the
    /// distance to the origin. Note that the plane does not enforce the
    /// normalized normal vector.
    /// Equation for points p on the plane: Normal dot p == Distance
    /// </summary>
    public struct __plane2t__ : IEquatable<__plane2t__>, IValidity // should be InfiniteLine2d
    {
        public __v2t__ Normal;
        public __ftype__ Distance;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and constant. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane2t__(__v2t__ normalizedNormal, __ftype__ distance)
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
        public __plane2t__(__v2t__ normalizedNormal, __v2t__ point)
        {
            Normal = normalizedNormal;
            Distance = Vec.Dot(normalizedNormal, point);
        }

        /// <summary>
        /// Creates a <see cref="__plane2t__"/> from another <see cref="__plane2t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane2t__(__plane2t__ o)
        {
            Normal = o.Normal;
            Distance = o.Distance;
        }

        /// <summary>
        /// Creates a <see cref="__plane2t__"/> from a <see cref="__plane2t2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane2t__(__plane2t2__ o)
        {
            Normal = (__v2t__)o.Normal;
            Distance = (__ftype__)o.Distance;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __plane2t__(__plane2t2__ o)
            => new __plane2t__(o);

        #endregion

        #region Constants

        public static __plane2t__ XAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane2t__(__v2t__.OI, 0);
        }

        public static __plane2t__ YAxis
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane2t__(-__v2t__.IO, 0);
        }

        public static __plane2t__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane2t__(__v2t__.Zero, 0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The point on the plane which is closest to the origin.
        /// </summary>
        [XmlIgnore]
        public __v2t__ Point
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
            get => Normal != __v2t__.Zero;
        }

        /// <summary>
        /// Returns true if the normal of the plane is the zero-vector.
        /// </summary>
        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == __v2t__.Zero;
        }

        /// <summary>
        /// Returns a __plane3t__ whose cutting-line with the XY-Plane
        /// is represented by the __plane2t__
        /// </summary>
        public __plane3t__ PlaneXY
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(Normal.XYO, Distance);
        }

        /// <summary>
        /// Returns a __plane3t__ whose cutting-line with the XZ-Plane
        /// is represented by the __plane2t__
        /// </summary>
        public __plane3t__ PlaneXZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(Normal.XOY, Distance);
        }

        /// <summary>
        /// Returns a __plane3t__ whose cutting-line with the YZ-Plane
        /// is represented by the __plane2t__
        /// </summary>
        public __plane3t__ PlaneYZ
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(Normal.OXY, Distance);
        }

        #endregion

        #region Plane Arithmetics

        /// <summary>
        /// Returns the normalized <see cref="__plane2t__"/> as new <see cref="__plane2t__"/>.
        /// </summary>
        public __plane2t__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                __ftype__ scale = Normal.Length;
                return new __plane2t__(Normal / scale, Distance / scale);
            }
        }

        /// <summary>
        /// Calculates the nomalized plane of this <see cref="__plane2t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            __ftype__ scale =  Normal.Length;
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
        /// Returns <see cref="__plane2t__"/> with normal vector in opposing direction.
        /// </summary>
        /// <returns></returns>
        public __plane2t__ Reversed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane2t__(-Normal, -Distance);
        }

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// IMPORTANT: If the plane is not normalized the returned height is scaled with the magnitued of the plane normal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ftype__ Height(__v2t__ p) => Vec.Dot(Normal, p) - Distance;

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign(__v2t__ p) => Height(p).Sign();

        /// <summary>
        /// Projets the given point x perpendicular on the plane
        /// and returns the nearest point on the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __v2t__ NearestPoint(__v2t__ x)
        {
            var p = Point;
            return (x - Normal.Dot(x - p) * Normal);
        }

        /// <summary>
        /// Returns the coefficients (a, b, c, d) of the normal equation.
        /// </summary>
        public __v3t__ Coefficients
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __v3t__(Normal, -Distance);
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__plane2t__ a, __plane2t__ b)
            => (a.Normal == b.Normal) && (a.Distance == b.Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__plane2t__ a, __plane2t__ b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__plane2t__ other)
            => Normal.Equals(other.Normal) && Distance.Equals(other.Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __plane2t__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __plane2t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __plane2t__(__v2t__.Parse(x[0]), __ftype__.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__plane2t__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __plane2t__ a, __plane2t__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Distance, b.Distance, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__plane2t__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __plane2t__ a, __plane2t__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    //# }
}
