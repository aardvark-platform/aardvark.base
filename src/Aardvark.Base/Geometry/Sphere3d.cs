using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    /// <summary>
    /// A three dimensional sphere represented by center and radius.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Sphere3d : IEquatable<Sphere3d>, IBoundingBox3d, IValidity
    {
        [DataMember]
        public V3d Center;
        [DataMember]
        public double Radius;

        #region Constructors

        /// <summary>
		/// Initializes a new instance of the <see cref="Sphere3d"/> class using center and radius values.
		/// </summary>
		public Sphere3d(V3d center, double radius)
		{
			Center = center;
			Radius = radius;
		}

        /// <summary>
        /// Creates a sphere from its center, and a point on its surface.
        /// </summary>
        public Sphere3d(V3d center, V3d pointOnSurface)
        {
            Center = center;
            Radius = (pointOnSurface - center).Length;
        }
        
        /// <summary>
        /// Uses the first 2 points in the sequence as the
        /// sphere's center and a point on the sphere's surface.
        /// </summary>
        public Sphere3d(IEnumerable<V3d> points)
        {
            var va = points.TakeToArray(2);
            Center = va[0];
            Radius = (va[1] - va[0]).Length;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Sphere3d"/> class using values from another sphere instance.
		/// </summary>
		public Sphere3d(Sphere3d sphere)
		{
			Center = sphere.Center;
			Radius = sphere.Radius;
		}

        #endregion

        #region Static Factories

        public static Sphere3d FromRadius(double radius)
            => new Sphere3d(V3d.Zero, radius);

        public static Sphere3d FromCenterAndRadius(V3d center, double radius)
            => new Sphere3d(center, radius);

        public static Sphere3d FromPoints(IEnumerable<V3d> points)
            => new Sphere3d(points);

        #endregion

        #region Properties

        public bool IsInvalid => Radius < 0.0;

        public bool IsValid => Radius >= 0.0;

        public double RadiusSquared => Radius * Radius;

        public double SurfaceArea => (4.0 * RadiusSquared * System.Math.PI);

        public double Volume => (4.0 / 3.0) * System.Math.PI * Radius * Radius * Radius;

        #endregion

        #region Constants

        /// <summary>
        /// Sphere at origin with radius 0.
        /// </summary>
        public static readonly Sphere3d Zero = new Sphere3d(V3d.Zero, 0.0);

        /// <summary>
        /// Sphere at origin with radius 1.
        /// </summary>
        public static readonly Sphere3d Unit = new Sphere3d(V3d.Zero, 1.0);

        /// <summary>
        /// Sphere at NaN with radius -1.
        /// </summary>
        public static readonly Sphere3d Invalid = new Sphere3d(V3d.NaN, -1.0);

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

        public override int GetHashCode() => HashCode.GetCombined(Center, Radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Sphere3d other)
            => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        public override bool Equals(object other)
            => (other is Sphere3d o) ? Equals(o) : false;

        /// <summary>
        /// Writes a sphere to String.
        /// </summary>
        /// <returns>String representing the sphere.</returns>
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Center, Radius);

        public static Sphere3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Sphere3d(V3d.Parse(x[0]), double.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d => new Box3d(Center - Radius, Center + Radius);

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

    public static class Box3dExtensions
    {
        public static Sphere3d GetBoundingSphere3d(this Box3d box)
            => box.IsInvalid ? Sphere3d.Invalid : new Sphere3d(box.Center, 0.5 * box.Size.Length);
    }
}