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

    #region Sphere3f

    /// <summary>
    /// A three dimensional sphere represented by center and radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Sphere3f : IEquatable<Sphere3f>, IBoundingBox3f, IValidity
    {
        [DataMember]
        public V3f Center;
        [DataMember]
        public float Radius;

        #region Constructors

        /// <summary>
		/// Initializes a new instance of the <see cref="Sphere3f"/> class using center and radius values.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3f(V3f center, float radius)
		{
			Center = center;
			Radius = radius;
		}

        /// <summary>
        /// Creates a sphere from its center, and a point on its surface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3f(V3f center, V3f pointOnSurface)
        {
            Center = center;
            Radius = (pointOnSurface - center).Length;
        }

        /// <summary>
        /// Uses the first 2 points in the sequence as the
        /// sphere's center and a point on the sphere's surface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3f(IEnumerable<V3f> points)
        {
            var va = points.TakeToArray(2);
            Center = va[0];
            Radius = (va[1] - va[0]).Length;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Sphere3f"/> class using values from another sphere instance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3f(Sphere3f sphere)
		{
			Center = sphere.Center;
			Radius = sphere.Radius;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere3f"/> class using values from a <see cref="Sphere3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3f(Sphere3d sphere)
        {
            Center = (V3f)sphere.Center;
            Radius = (float)sphere.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Sphere3f(Sphere3d s)
            => new Sphere3f(s);

        #endregion

        #region Static Factories

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Sphere3f FromRadius(float radius)
            => new Sphere3f(V3f.Zero, radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Sphere3f FromCenterAndRadius(V3f center, float radius)
            => new Sphere3f(center, radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Sphere3f FromPoints(IEnumerable<V3f> points)
            => new Sphere3f(points);

        #endregion

        #region Properties

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0.0;
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0.0;
        }

        public readonly float RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius;
        }

        public readonly float SurfaceArea
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (4 * RadiusSquared * ConstantF.Pi);
        }

        public readonly float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (4.0f / 3.0f) * ConstantF.Pi * Radius * Radius * Radius;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Sphere at origin with radius 0.
        /// </summary>
        public static Sphere3f Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Sphere3f(V3f.Zero, 0);
        }

        /// <summary>
        /// Sphere at origin with radius 1.
        /// </summary>
        public static Sphere3f Unit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Sphere3f(V3f.Zero, 1);
        }

        /// <summary>
        /// Sphere at NaN with radius -1.
        /// </summary>
        public static Sphere3f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Sphere3f(V3f.NaN, -1);
        }

        #endregion

        #region Comparison operators

        /// <summary>
        /// Tests whether two specified spheres are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Sphere3f a, Sphere3f b)
            => (a.Center == b.Center) && (a.Radius == b.Radius);

        /// <summary>
        /// Tests whether two specified spheres are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Sphere3f a, Sphere3f b)
            => !(a == b);

        #endregion

        #region Overrides

        public override readonly int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Sphere3f other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        public override readonly bool Equals(object other)
            => (other is Sphere3f o) ? Equals(o) : false;

        /// <summary>
        /// Writes a sphere to String.
        /// </summary>
        /// <returns>String representing the sphere.</returns>
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        public static Sphere3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Sphere3f(V3f.Parse(x[0]), float.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox3f Members

        public readonly Box3f BoundingBox3f
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box3f(Center - Radius, Center + Radius);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Sphere3f"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Sphere3f a, Sphere3f b, float tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Sphere3f"/> are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Sphere3f a, Sphere3f b)
            => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
    }

    #endregion

    #region Sphere3d

    /// <summary>
    /// A three dimensional sphere represented by center and radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Sphere3d : IEquatable<Sphere3d>, IBoundingBox3d, IValidity
    {
        [DataMember]
        public V3d Center;
        [DataMember]
        public double Radius;

        #region Constructors

        /// <summary>
		/// Initializes a new instance of the <see cref="Sphere3d"/> class using center and radius values.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3d(V3d center, double radius)
		{
			Center = center;
			Radius = radius;
		}

        /// <summary>
        /// Creates a sphere from its center, and a point on its surface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3d(V3d center, V3d pointOnSurface)
        {
            Center = center;
            Radius = (pointOnSurface - center).Length;
        }

        /// <summary>
        /// Uses the first 2 points in the sequence as the
        /// sphere's center and a point on the sphere's surface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3d(IEnumerable<V3d> points)
        {
            var va = points.TakeToArray(2);
            Center = va[0];
            Radius = (va[1] - va[0]).Length;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Sphere3d"/> class using values from another sphere instance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3d(Sphere3d sphere)
		{
			Center = sphere.Center;
			Radius = sphere.Radius;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere3d"/> class using values from a <see cref="Sphere3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sphere3d(Sphere3f sphere)
        {
            Center = (V3d)sphere.Center;
            Radius = (double)sphere.Radius;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Sphere3d(Sphere3f s)
            => new Sphere3d(s);

        #endregion

        #region Static Factories

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Sphere3d FromRadius(double radius)
            => new Sphere3d(V3d.Zero, radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Sphere3d FromCenterAndRadius(V3d center, double radius)
            => new Sphere3d(center, radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Sphere3d FromPoints(IEnumerable<V3d> points)
            => new Sphere3d(points);

        #endregion

        #region Properties

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius < 0.0;
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius >= 0.0;
        }

        public readonly double RadiusSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Radius * Radius;
        }

        public readonly double SurfaceArea
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (4 * RadiusSquared * Constant.Pi);
        }

        public readonly double Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (4.0 / 3.0) * Constant.Pi * Radius * Radius * Radius;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Sphere at origin with radius 0.
        /// </summary>
        public static Sphere3d Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Sphere3d(V3d.Zero, 0);
        }

        /// <summary>
        /// Sphere at origin with radius 1.
        /// </summary>
        public static Sphere3d Unit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Sphere3d(V3d.Zero, 1);
        }

        /// <summary>
        /// Sphere at NaN with radius -1.
        /// </summary>
        public static Sphere3d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Sphere3d(V3d.NaN, -1);
        }

        #endregion

        #region Comparison operators

        /// <summary>
        /// Tests whether two specified spheres are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Sphere3d a, Sphere3d b)
            => (a.Center == b.Center) && (a.Radius == b.Radius);

        /// <summary>
        /// Tests whether two specified spheres are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Sphere3d a, Sphere3d b)
            => !(a == b);

        #endregion

        #region Overrides

        public override readonly int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Sphere3d other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        public override readonly bool Equals(object other)
            => (other is Sphere3d o) ? Equals(o) : false;

        /// <summary>
        /// Writes a sphere to String.
        /// </summary>
        /// <returns>String representing the sphere.</returns>
        public override readonly string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        public static Sphere3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Sphere3d(V3d.Parse(x[0]), double.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox3d Members

        public readonly Box3d BoundingBox3d
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Box3d(Center - Radius, Center + Radius);
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Sphere3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Sphere3d a, Sphere3d b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Radius, b.Radius, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Sphere3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Sphere3d a, Sphere3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

}