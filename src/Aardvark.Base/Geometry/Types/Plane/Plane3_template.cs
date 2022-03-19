using System;
using System.Collections.Generic;
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
    //#   var plane3t2 = "Plane3" + tc2;
    //#   var planewithpoint3t = "PlaneWithPoint3" + tc;
    //#   var planewithpoint3t2 = "PlaneWithPoint3" + tc2;
    //#   var planepair3t = "PlanePair3" + tc;
    //#   var planepair3t2 = "PlanePair3" + tc2;
    //#   var planetriple3t = "PlaneTriple3" + tc;
    //#   var planetriple3t2 = "PlaneTriple3" + tc2;
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var v4t = "V4" + tc;
    //#   var m44t = "M44" + tc;
    //#   var euclidean3t = "Euclidean3" + tc;
    //#   var trafo3t = "Trafo3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var ray3t = "Ray3" + tc;
    //#   var iboundingbox3t = "IBoundingBox3" + tc;
    #region __plane3t__

    /// <summary>
    /// A plane represented by a (possibly) normalized normal vector and the
    /// distance to the origin. Note that the plane does not enforce the
    /// normalized normal vector.
    /// Equation for points p on the plane: Normal dot p == Distance
    /// </summary>
    public struct __plane3t__ : IEquatable<__plane3t__>, IValidity, __iboundingbox3t__
    {
        /// <summary>
        /// Plane normal.
        /// </summary>
        public __v3t__ Normal;

        /// <summary>
        /// Distance from origin to plane.
        /// </summary>
        public __ftype__ Distance;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and distance to origin.
        /// IMPORTANT: The normal has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__(__v3t__ normalizedNormal, __ftype__ distance)
        {
            Normal = normalizedNormal;
            Distance = distance;
        }

        /// <summary>
        /// Creates plane from normal vector and point.
        /// IMPORTANT: The normal has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__(__v3t__ normalizedNormal, __v3t__ point)
        {
            Normal = normalizedNormal;
            Distance = Vec.Dot(normalizedNormal, point);
        }

        /// <summary>
        /// Creates a plane from 3 independent points.
        /// A normalized normal vector is computed and stored.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__(__v3t__ p0, __v3t__ p1, __v3t__ p2)
        {
            Normal = Vec.Cross(p1 - p0, p2 - p0).Normalized;
            Distance = Vec.Dot(Normal, p0);
        }

        /// <summary>
        /// Create a plane from coefficients (a, b, c, d) of the normal equation: ax + by + cz + d = 0
        /// Normal = [a, b, c]; Distance = -d
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__(__v4t__ coefficients)
        {
            Normal = coefficients.XYZ;
            Distance = -coefficients.W;
        }

        /// <summary>
        /// Creates a <see cref="__plane3t__"/> from another <see cref="__plane3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__(__plane3t__ o)
        {
            Normal = o.Normal;
            Distance = o.Distance;
        }

        /// <summary>
        /// Creates a <see cref="__plane3t__"/> from a <see cref="__plane3t2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__(__plane3t2__ o)
        {
            Normal = (__v3t__)o.Normal;
            Distance = (__ftype__)o.Distance;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __plane3t__(__plane3t2__ o)
            => new __plane3t__(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __plane3t__(__planewithpoint3t__ o)
            => new __plane3t__(o.Normal, o.Point);

        #endregion

        #region Constants

        /// <summary>YZ plane.</summary>
        public static __plane3t__ XPlane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(__v3t__.XAxis, __v3t__.Zero);
        }

        /// <summary>XZ plane.</summary>
        public static __plane3t__ YPlane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(__v3t__.YAxis, __v3t__.Zero);
        }

        /// <summary>XY plane.</summary>
        public static __plane3t__ ZPlane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(__v3t__.ZAxis, __v3t__.Zero);
        }

        /// <summary>Invalid plane.</summary>
        public static __plane3t__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(__v3t__.Zero, 0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The point on the plane which is closest to the origin.
        /// </summary>
        [XmlIgnore]
        public __v3t__ Point
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Normal * Distance / Normal.LengthSquared; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Distance = Vec.Dot(Normal, value); }
        }

        /// <summary>
        /// Returns true if the normal of the plane is not the zero-vector.
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal != __v3t__.Zero;
        }

        /// <summary>
        /// Returns true if the normal of the plane is the zero-vector.
        /// </summary>
        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == __v3t__.Zero;
        }

        /// <summary>
        /// Returns the normalized <see cref="__plane3t__"/> as new <see cref="__plane3t__"/>.
        /// </summary>
        public __plane3t__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                __ftype__ scale = Normal.Length;
                return new __plane3t__(Normal / scale, Distance / scale);
            }
        }

        /// <summary>
        /// Returns the coefficients (a, b, c, d) of the normal equation: ax + by + cz + d = 0
        /// </summary>
        public __v4t__ Coefficients
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __v4t__(Normal, -Distance);
        }

        #endregion

        #region Arithmetics

        /// <summary>
        /// Normalizes this <see cref="__plane3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            __ftype__ scale = Normal.Length;
            Normal /= scale;
            Distance /= scale;
        }

        /// <summary>
        /// Changes sign of normal vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            Normal = -Normal;
            Distance = -Distance;
        }

        /// <summary>
        /// Returns <see cref="__plane3t__"/> with normal vector in opposing direction.
        /// </summary>
        public __plane3t__ Reversed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(-Normal, -Distance);
        }

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// IMPORTANT: If the plane is not normalized the returned height is scaled by the magnitued of the plane normal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ftype__ Height(__v3t__ p)
            => Vec.Dot(Normal, p) - Distance;

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign(__v3t__ p) => Height(p).Sign();

        /// <summary>
        /// Projets the given point x perpendicular on the plane
        /// and returns the nearest point on the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __v3t__ NearestPoint(__v3t__ x)
        {
            var p = Point;
            return (x - Normal.Dot(x - p) * Normal);
        }

        /// <summary>
        /// Transforms the plane with a given trafo using the inverse
        /// transposed matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__ Transformed(__trafo3t__ trafo) 
        {
            return new __plane3t__(
                new __v3t__(
                    trafo.Backward.M00 * Normal.X + trafo.Backward.M10 * Normal.Y + trafo.Backward.M20 * Normal.Z - trafo.Backward.M30 * Distance,
                    trafo.Backward.M01 * Normal.X + trafo.Backward.M11 * Normal.Y + trafo.Backward.M21 * Normal.Z - trafo.Backward.M31 * Distance,
                    trafo.Backward.M02 * Normal.X + trafo.Backward.M12 * Normal.Y + trafo.Backward.M22 * Normal.Z - trafo.Backward.M32 * Distance
                ),
                trafo.Backward.M33 * Distance - trafo.Backward.M03 * Normal.X - trafo.Backward.M13 * Normal.Y - trafo.Backward.M23 * Normal.Z
            );
        }

        /// <summary>
        /// Transforms the plane with a given matrix. The matrix is assumed
        /// to represent a euclidean transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__ Transformed(__m44t__ trafo)
        {
            var n = trafo.TransformDir(Normal);
            var d = Distance + trafo.C0.Dot(trafo.C3) * Normal.X + trafo.C1.Dot(trafo.C3) * Normal.Y + trafo.C2.Dot(trafo.C3) * Normal.Z;
            return new __plane3t__(n, d);
        }

        /// <summary>
        /// Transforms the plane with a given __euclidean3t__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __plane3t__ Transformed(__euclidean3t__ trafo)
        {
            var n1 = trafo.TransformDir(Normal);
            return new __plane3t__(n1, Distance + trafo.Trans.Dot(n1));
        }

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__plane3t__ a, __plane3t__ b)
            => a.Normal == b.Normal && a.Distance == b.Distance;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__plane3t__ a, __plane3t__ b)
            => a.Normal != b.Normal || a.Distance != b.Distance;

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__plane3t__ other)
            => Normal.Equals(other.Normal) && Distance.Equals(other.Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __plane3t__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Distance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __plane3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __plane3t__(__v3t__.Parse(x[0]), __ftype__.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region __iboundingbox3t__ Members

        /// <summary>
        /// Gets entire __ftype__ space as bounding box.
        /// </summary>
        public __box3t__ BoundingBox3__tc__
        {
            get
            {
                var box = new __box3t__(__v3t__.MinValue, __v3t__.MaxValue);

                if (Normal == __v3t__.XAxis)
                {
                    box.Min.X = Distance; box.Max.X = Distance; return box;
                }
                if (Normal == __v3t__.YAxis)
                {
                    box.Min.Y = Distance; box.Max.Y = Distance; return box;
                }
                if (Normal == __v3t__.ZAxis)
                {
                    box.Min.Z = Distance; box.Max.Z = Distance; return box;
                }

                return box;
            }
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__plane3t__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __plane3t__ a, __plane3t__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Distance, b.Distance, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__plane3t__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __plane3t__ a, __plane3t__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    #region __planewithpoint3t__

    /// <summary>
    /// A plane with a specific point that can be retrieved later.
    /// </summary>
    public struct __planewithpoint3t__ : __iboundingbox3t__
    {
        public __v3t__ Normal;
        public __v3t__ Point;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and point. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planewithpoint3t__(__v3t__ normalizedNormal, __v3t__ point)
        {
            Normal = normalizedNormal;
            Point = point;
        }

        /// <summary>
        /// Creates a <see cref="__plane3t__"/> from another <see cref="__plane3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planewithpoint3t__(__planewithpoint3t__ o)
        {
            Normal = o.Normal;
            Point = o.Point;
        }

        /// <summary>
        /// Creates a <see cref="__plane3t__"/> from a <see cref="__plane3t2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planewithpoint3t__(__planewithpoint3t2__ o)
        {
            Normal = (__v3t__)o.Normal;
            Point = (__v3t__)o.Point;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __planewithpoint3t__(__planewithpoint3t2__ o)
            => new __planewithpoint3t__(o);

        #endregion

        #region Constants

        public static __planewithpoint3t__ XPlane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planewithpoint3t__(__v3t__.XAxis, __v3t__.Zero);
        }

        public static __planewithpoint3t__ YPlane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planewithpoint3t__(__v3t__.YAxis, __v3t__.Zero);
        }

        public static __planewithpoint3t__ ZPlane
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planewithpoint3t__(__v3t__.ZAxis, __v3t__.Zero);
        }

        public static __planewithpoint3t__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planewithpoint3t__(__v3t__.Zero, __v3t__.Zero);
        }

        #endregion

        #region Properties

        public __planewithpoint3t__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planewithpoint3t__(Normal.Normalized, Point);
        }

        public __plane3t__ __plane3t__
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __plane3t__(Normal, Point);
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == __v3t__.Zero;
        }

        public __planewithpoint3t__ Reversed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planewithpoint3t__(-Normal, Point);
        }

        #endregion

        #region Arithmetics

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Normal.Normalize();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse() => Normal = -Normal;

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ftype__ Height(__v3t__ p) => Vec.Dot(Normal, p - Point);

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign(__v3t__ p) => Height(p).Sign();

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.GetCombined(Normal, Point);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__planewithpoint3t__ other)
            => Normal.Equals(other.Normal) && Point.Equals(other.Point);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __planewithpoint3t__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Point);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __planewithpoint3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __planewithpoint3t__(__v3t__.Parse(x[0]), __v3t__.Parse(x[1]));
        }

        #endregion

        #region __iboundingbox3t__ Members

        /// <summary>
        /// Gets entire __ftype__ space as bounding box.
        /// </summary>
        public __box3t__ BoundingBox3__tc__
        {
            get
            {
                var box = new __box3t__(__v3t__.MinValue, __v3t__.MaxValue);

                if (Normal == __v3t__.XAxis)
                {
                    box.Min.X = Point.X; box.Max.X = Point.X; return box;
                }
                if (Normal == __v3t__.YAxis)
                {
                    box.Min.Y = Point.Y; box.Max.Y = Point.Y; return box;
                }
                if (Normal == __v3t__.ZAxis)
                {
                    box.Min.Z = Point.Z; box.Max.Z = Point.Z; return box;
                }

                return box;
            }
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__planewithpoint3t__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __planewithpoint3t__ a, __planewithpoint3t__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Point, b.Point, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__planewithpoint3t__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __planewithpoint3t__ a, __planewithpoint3t__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    #region __planepair3t__

    /// <summary>
    /// A plane pair defines a ray at their intersection.
    /// </summary>
    public struct __planepair3t__
    {
        public __plane3t__ Plane0;
        public __plane3t__ Plane1;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planepair3t__(__plane3t__ plane0, __plane3t__ plane1)
        {
            Plane0 = plane0; Plane1 = plane1;
        }

        /// <summary>
        /// Creates a <see cref="__planepair3t__"/> from another <see cref="__planepair3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planepair3t__(__planepair3t__ o)
        {
            Plane0 = o.Plane0;
            Plane1 = o.Plane1;
        }

        /// <summary>
        /// Creates a <see cref="__planepair3t__"/> from a <see cref="__planepair3t2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planepair3t__(__planepair3t2__ o)
        {
            Plane0 = (__plane3t__)o.Plane0;
            Plane1 = (__plane3t__)o.Plane1;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __planepair3t__(__planepair3t2__ o)
            => new __planepair3t__(o);

        #endregion

        #region Arithmetics

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ray3t__ Get__ray3t__()
        {
            Plane0.Intersects(Plane1, out __ray3t__ ray);
            return ray;
        }

        #endregion
    }

    #endregion

    #region __planetriple3t__

    /// <summary>
    /// A plane triple defines a point at their intersection.
    /// </summary>
    public struct __planetriple3t__
    {
        public __plane3t__ Plane0;
        public __plane3t__ Plane1;
        public __plane3t__ Plane2;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planetriple3t__(__plane3t__ plane0, __plane3t__ plane1, __plane3t__ plane2)
        {
            Plane0 = plane0; Plane1 = plane1; Plane2 = plane2;
        }

        /// <summary>
        /// Creates a <see cref="__planetriple3t__"/> from another <see cref="__planetriple3t__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planetriple3t__(__planetriple3t__ o)
        {
            Plane0 = o.Plane0;
            Plane1 = o.Plane1;
            Plane2 = o.Plane2;
        }

        /// <summary>
        /// Creates a <see cref="__planetriple3t__"/> from a <see cref="__planetriple3t2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __planetriple3t__(__planetriple3t2__ o)
        {
            Plane0 = (__plane3t__)o.Plane0;
            Plane1 = (__plane3t__)o.Plane1;
            Plane2 = (__plane3t__)o.Plane2;
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __planetriple3t__(__planetriple3t2__ o)
            => new __planetriple3t__(o);

        #endregion

        #region Properties

        __planepair3t__ Pair01
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planepair3t__(Plane0, Plane1);
        }

        __planepair3t__ Pair02
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planepair3t__(Plane0, Plane2);
        }

        __planepair3t__ Pair12
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planepair3t__(Plane1, Plane2);
        }

        __planepair3t__ Pair10
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planepair3t__(Plane1, Plane0);
        }

        __planepair3t__ Pair20
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planepair3t__(Plane2, Plane0);
        }

        __planepair3t__ Pair21
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __planepair3t__(Plane2, Plane1);
        }

        #endregion

        #region Arithmetics

        public __v3t__ GetPoint()
        {
            Plane0.Intersects(Plane1, Plane2, out __v3t__ point);
            return point;
        }

        #endregion
    }

    #endregion

    #region __plane3t__Extensions

    public static class __plane3t__Extensions
    {
        /// <summary>
        /// Returns a transformation of an orthonormal basis in Plane- to WorldSpace.
        /// </summary>
        public static __trafo3t__ GetPlaneSpaceTransform(this __plane3t__ self)
            => __trafo3t__.FromNormalFrame(self.Point, self.Normal);

        /// <summary>
        /// 3D world space to 2D plane space.
        /// Plane space is defined by a normal-frame from Point and Normal of the plane.
        /// </summary>
        public static __m44t__ GetWorldToPlane(this __plane3t__ self)
        {
            __m44t__.NormalFrame(self.Point, self.Normal, out __m44t__ _, out __m44t__ global2local);
            return global2local;
        }

        /// <summary>
        /// 2D plane space to 3D world space.
        /// Plane space is defined by a normal-frame from Point and Normal of the plane.
        /// </summary>
        public static __m44t__ GetPlaneToWorld(this __plane3t__ self)
        {
            __m44t__.NormalFrame(self.Point, self.Normal, out __m44t__ local2global, out __m44t__ _);
            return local2global;
        }

        /// <summary>
        /// Projects a point onto the plane (shortest distance).
        /// </summary>
        public static __v3t__ Project(this __plane3t__ plane, __v3t__ p) => p - plane.Height(p) * plane.Normal;

        /// <summary>
        /// Projects a point onto the plane along given direction.
        /// </summary>
        public static __v3t__ Project(this __plane3t__ plane, __v3t__ p, __v3t__ direction)
        {
            var r = new __ray3t__(p, direction);
            if (r.Intersects(plane, out __ftype__ t))
            {
                return r.GetPointOnRay(t);
            }
            else
            {
                throw new Exception(string.Format(
                    "Failed to project point {0} onto plane {1} along direction {2}.", p, plane, direction)
                    );
            }
        }

        /// <summary>
        /// Projects points onto plane (shortest distance).
        /// </summary>
        public static __v3t__[] Project(this __plane3t__ plane, __v3t__[] pointArray, int startIndex = 0, int count = 0)
        {
            if (pointArray == null) throw new ArgumentNullException();
            if (startIndex < 0 || startIndex >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count < 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count == 0) count = pointArray.Length - startIndex;

            var normal = plane.Normal;
            var result = new __v3t__[count];
            for (int i = startIndex, j = 0; j < count; i++, j++)
            {
                var p = pointArray[i];
                var height = plane.Height(p);
                result[j] = p - height * normal;
            }
            return result;
        }

        /// <summary>
        /// Projects points onto plane along given direction.
        /// </summary>
        public static __v3t__[] Project(this __plane3t__ plane, __v3t__[] pointArray, __v3t__ direction, int startIndex = 0, int count = 0)
        {
            if (pointArray == null) throw new ArgumentNullException();
            if (startIndex < 0 || startIndex >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count < 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count == 0) count = pointArray.Length - startIndex;

            var result = new __v3t__[count];
            for (int i = startIndex, j = 0; j < count; i++, j++)
            {
                var r = new __ray3t__(pointArray[i], direction);
                if (r.Intersects(plane, out __ftype__ t))
                {
                    result[j] = r.GetPointOnRay(t);
                }
                else
                {
                    throw new Exception(string.Format(
                        "Failed to project point {0} onto plane {1} along direction {2}.", pointArray[i], plane, direction)
                        );
                }
            };
            return result;
        }

        /// <summary>
        /// Projects a point from world space to plane space (shortest distance).
        /// </summary>
        public static __v2t__ ProjectToPlaneSpace(this __plane3t__ plane, __v3t__ p)
            => plane.GetWorldToPlane().TransformPos(p).XY;

        /// <summary>
        /// Projects points from world space to plane space (shortest distance).
        /// </summary>
        public static __v2t__[] ProjectToPlaneSpace(this __plane3t__ plane, __v3t__[] points)
        {
            var global2local = plane.GetWorldToPlane();
            return points.Map(p => global2local.TransformPos(p).XY);
        }

        /// <summary>
        /// Transforms point from plane space to world space.
        /// </summary>
        public static __v3t__ Unproject(this __plane3t__ plane, __v2t__ point)
        {
            var local2global = plane.GetPlaneToWorld();
            return local2global.TransformPos(point.XYO);
        }

        /// <summary>
        /// Transforms points from plane space to world space.
        /// </summary>
        public static __v3t__[] Unproject(this __plane3t__ plane, __v2t__[] points)
        {
            var local2global = plane.GetPlaneToWorld();
            return points.Map(p => local2global.TransformPos(p.XYO));
        }

        /// <summary>
        /// Transforms points from plane space to world space.
        /// </summary>
        public static __v3t__[] Unproject(this __plane3t__ plane, IReadOnlyList<__v2t__> points)
        {
            var local2global = plane.GetPlaneToWorld();
            var xs = new __v3t__[points.Count];
            for (var i = 0; i < points.Count; i++)
                xs[i] = local2global.TransformPos(points[i].XYO);
            return xs;
        }
    }

    #endregion

    //# }
}
