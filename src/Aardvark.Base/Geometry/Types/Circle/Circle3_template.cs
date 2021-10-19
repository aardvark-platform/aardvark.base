using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Circle3" + tc;
    //#   var type2 = "Circle3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var iboundingbox = "IBoundingBox3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var dotnine = isDouble ? "0.9" : "0.9f";
    //#   var constant = isDouble ? "Constant" : "ConstantF";
    #region __type__

    /// <summary>
    /// A circle in 3-space represented by its center, normal (normalized), and a radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __type__ : IEquatable<__type__>, __iboundingbox__, IValidity
    {
        [DataMember]
        public __v3t__ Center;

        [DataMember]
        public __v3t__ Normal;

        [DataMember]
        public __ftype__ Radius;

        #region Constructors

        /// <summary>
        /// Creates a circle from it's center, a normal vector (normalized) and a radius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v3t__ center, __v3t__ normal, __ftype__ radius)
        {
            Center = center;
            Normal = normal;
            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ circle)
		{
            Center = circle.Center;
            Normal = circle.Normal;
            Radius = circle.Radius;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ circle)
        {
            Center = (__v3t__)circle.Center;
            Normal = (__v3t__)circle.Normal;
            Radius = (__ftype__)circle.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type2__ c)
            => new __type__(c);

        #endregion

        #region Constants

        public static __type__ Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v3t__.Zero, __v3t__.ZAxis, 0);
        }

        public static __type__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v3t__.NaN, __v3t__.NaN, -1);
        }

        #endregion

        #region Properties

        public __ftype__ RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius.Square();
        }

        public __ftype__ Circumference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2 * Radius * __constant__.Pi;
        }

        public __ftype__ Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => RadiusSquared * __constant__.Pi;
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0;
        }

        /// <summary>
        /// Returns a point on the circumference (AxisU).
        /// </summary>
        public __v3t__ Point
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Center + AxisU * Radius;
        }

        /// <summary>
        /// Returns an axis aligned vector pointing from the center to the circumference.
        /// </summary>
        public __v3t__ AxisU
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                __v3t__ dir = Normal.Dot(__v3t__.XAxis).Abs() > __dotnine__ ? __v3t__.YAxis : __v3t__.XAxis;
                var pdir = Normal.Cross(dir);
                return pdir.Normalized * Radius;
            }
        }

        /// <summary>
        /// Returns an axis aligned vector pointing from the center to the circumference.
        /// </summary>
        public __v3t__ AxisV
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var axisU = AxisU;
                return AxisU.Cross(Normal).Normalized * Radius;
            }
        }

		/// <summary>
		/// Return an IEnumerable of points on the circle's circumference, optionally repeating the first point as the last.
		/// </summary>
		/// <param name="tesselation">number of distinct points to generate. the actual number of points returned depends on the <para>duplicateClosePoint</para> parameter. must be 3 or larger.</param>
		/// <param name="duplicateClosePoint">if true, the first point is repeated as the last</param>
		/// <returns>IEnumerable of points on the circle's circumference. if diplicateClosePoint is true, <para>tesselation</para>+1 points are returned.</returns>
		public IEnumerable<__v3t__> Points(int tesselation, bool duplicateClosePoint = false)
		{
			if (tesselation < 3)
				throw new ArgumentOutOfRangeException("tesselation", "tesselation must be at least 3.");

			var off = 0;
			if (duplicateClosePoint)
				off = 1;
			for (int i = 0; i < tesselation + off; i++)
			{
				var angle = ((__ftype__)i) / tesselation * __constant__.PiTimesTwo;
				yield return Center + AxisU * Fun.Cos(angle) + AxisV * Fun.Sin(angle);
			}
		}

        public __plane3t__ Plane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(Normal, Center);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __v3t__ GetPoint(__ftype__ angle)
            => Center + AxisU * angle.Cos() + AxisV * angle.Sin();

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
            => (a.Center == b.Center) && (a.Normal == b.Normal) && (a.Radius == b.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Center.Equals(other.Center) && Normal.Equals(other.Normal) && Radius.Equals(other.Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Normal, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__v3t__.Parse(x[0]), __v3t__.Parse(x[1]), __ftype__.Parse(x[2], CultureInfo.InvariantCulture));
        }

        #endregion

        #region __iboundingbox__ Members

        public __box3t__ BoundingBox3__tc__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var u = AxisU; var v = AxisV;
                return new __box3t__(Center + u, Center - u, Center + v, Center - v);
            }
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    //# }
}