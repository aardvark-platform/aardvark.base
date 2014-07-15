using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    /// <summary>
    /// A three dimensional sphere represented by center and radius.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Sphere3d : IBoundingBox3d, IValidity
    {
        public V3d Center;
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
        {
            return new Sphere3d(V3d.Zero, radius);
        }

        public static Sphere3d FromCenterAndRadius(V3d center, double radius)
        {
            return new Sphere3d(center, radius);
        }

        public static Sphere3d FromPoints(IEnumerable<V3d> points)
        {
            return new Sphere3d(points);
        }

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

        public double SurfaceArea
        {
            get { return (4.0 * RadiusSquared * System.Math.PI); }
        }

        public double Volume
        {
            get
            {
                return (4.0 / 3.0) * System.Math.PI * Radius * Radius * Radius;
            }
        }

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
        public static bool operator ==(Sphere3d a, Sphere3d b)
        {
            if (Object.Equals(a, null) == true)
            {
                return Object.Equals(b, null);
            }

            if (Object.Equals(b, null) == true)
            {
                return Object.Equals(a, null);
            }

            return (a.Center == b.Center) && (a.Radius == b.Radius);
        }

        /// <summary>
        /// Tests whether two specified spheres are not equal.
        /// </summary>
        public static bool operator !=(Sphere3d a, Sphere3d b)
        {
            if (Object.Equals(a, null) == true)
            {
                return !Object.Equals(b, null);
            }
            else if (Object.Equals(b, null) == true)
            {
                return !Object.Equals(a, null);
            }
            return !((a.Center == b.Center) && (a.Radius == b.Radius));
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Center, Radius);
        }

        public override bool Equals(object other)
        {
            if (other is Sphere3d)
            {
                var value = (Sphere3d)other;
                return (Center == value.Center && Radius == value.Radius);
            }
            return false;
        }

        /// <summary>
        /// Writes a sphere to String.
        /// </summary>
        /// <returns>String representing the sphere.</returns>
        public override string ToString()
        {
            return string.Format("[{0}, {1}]", Center, Radius);
        }

        public static Sphere3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Sphere3d(V3d.Parse(x[0]), double.Parse(x[1]));
        }

        #endregion

        #region IBoundingBox3d Members

        public Box3d BoundingBox3d
        {
            get { return new Box3d(Center - Radius, Center + Radius); }
        }

        #endregion
    }

    public static class Box3dExtensions
    {

        public static Sphere3d GetBoundingSphere3d(this Box3d box)
        {
            return box.IsInvalid ? Sphere3d.Invalid
                : new Sphere3d(box.Center, 0.5 * box.Size.Length);
        }
    }
}