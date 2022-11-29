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
    //#   var type = "Sphere3" + tc;
    //#   var type2 = "Sphere3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var iboundingbox = "IBoundingBox3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var fourbythree = isDouble ? "(4.0 / 3.0)" : "(4.0f / 3.0f)";
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    #region __type__

    /// <summary>
    /// A three dimensional sphere represented by center and radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>, __iboundingbox__, IValidity
    {
        [DataMember]
        public __v3t__ Center;
        [DataMember]
        public __ftype__ Radius;

        #region Constructors

        /// <summary>
		/// Initializes a new instance of the <see cref="__type__"/> class using center and radius values.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v3t__ center, __ftype__ radius)
		{
			Center = center;
			Radius = radius;
		}

        /// <summary>
        /// Creates a sphere from its center, and a point on its surface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__v3t__ center, __v3t__ pointOnSurface)
        {
            Center = center;
            Radius = (pointOnSurface - center).Length;
        }

        /// <summary>
        /// Uses the first 2 points in the sequence as the
        /// sphere's center and a point on the sphere's surface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(IEnumerable<__v3t__> points)
        {
            var va = points.TakeToArray(2);
            Center = va[0];
            Radius = (va[1] - va[0]).Length;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="__type__"/> class using values from another sphere instance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ sphere)
		{
			Center = sphere.Center;
			Radius = sphere.Radius;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="__type__"/> class using values from a <see cref="__type2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ sphere)
        {
            Center = (__v3t__)sphere.Center;
            Radius = (__ftype__)sphere.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type2__ s)
            => new __type__(s);

        #endregion

        #region Static Factories

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromRadius(__ftype__ radius)
            => new __type__(__v3t__.Zero, radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromCenterAndRadius(__v3t__ center, __ftype__ radius)
            => new __type__(center, radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromPoints(IEnumerable<__v3t__> points)
            => new __type__(points);

        #endregion

        #region Properties

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0.0;
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0.0;
        }

        public __ftype__ RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius;
        }

        public __ftype__ SurfaceArea
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (4 * RadiusSquared * __pi__);
        }

        public __ftype__ Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => __fourbythree__ * __pi__ * Radius * Radius * Radius;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Sphere at origin with radius 0.
        /// </summary>
        public static __type__ Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v3t__.Zero, 0);
        }

        /// <summary>
        /// Sphere at origin with radius 1.
        /// </summary>
        public static __type__ Unit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v3t__.Zero, 1);
        }

        /// <summary>
        /// Sphere at NaN with radius -1.
        /// </summary>
        public static __type__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v3t__.NaN, -1);
        }

        #endregion

        #region Comparison operators

        /// <summary>
        /// Tests whether two specified spheres are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
            => (a.Center == b.Center) && (a.Radius == b.Radius);

        /// <summary>
        /// Tests whether two specified spheres are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        /// <summary>
        /// Writes a sphere to String.
        /// </summary>
        /// <returns>String representing the sphere.</returns>
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__v3t__.Parse(x[0]), __ftype__.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region __iboundingbox__ Members

        public __box3t__ BoundingBox3__tc__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __box3t__(Center - Radius, Center + Radius);
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