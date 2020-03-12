using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// A plane represented by a (possibly) normalized normal vector and the
    /// distance to the origin. Note that the plane does not enforce the
    /// normalized normal vector.
    /// Equation for points p on the plane: Normal dot p == Distance
    /// </summary>
    public struct Plane3d : IValidity, IBoundingBox3d
    {
        /// <summary>
        /// Plane normal.
        /// </summary>
        public V3d Normal;

        /// <summary>
        /// Distance from origin to plane.
        /// </summary>
        public double Distance;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and distance to origin.
        /// IMPORTANT: The normal has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        public Plane3d(V3d normalizedNormal, double distance)
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
        public Plane3d(V3d normalizedNormal, V3d point)
        {
            Normal = normalizedNormal;
            Distance = Vec.Dot(normalizedNormal, point);
        }

        /// <summary>
        /// Creates a plane from 3 independent points.
        /// A normalized normal vector is computed and stored.
        /// </summary>
        public Plane3d(V3d p0, V3d p1, V3d p2)
        {
            Normal = Vec.Cross(p1 - p0, p2 - p0).Normalized;
            Distance = Vec.Dot(Normal, p0);
        }

        /// <summary>
        /// Create a plane from coefficients (a, b, c, d) of the normal equation: ax + by + cz + d = 0
        /// Normal = [a, b, c]; Distance = -d
        /// </summary>
        public Plane3d(V4d coefficients)
        {
            Normal = coefficients.XYZ;
            Distance = -coefficients.W;
        }
        
        #endregion

        #region Constants

        /// <summary>YZ plane.</summary>
        public static readonly Plane3d XPlane = new Plane3d(V3d.XAxis, V3d.Zero);
        /// <summary>XZ plane.</summary>
        public static readonly Plane3d YPlane = new Plane3d(V3d.YAxis, V3d.Zero);
        /// <summary>XY plane.</summary>
        public static readonly Plane3d ZPlane = new Plane3d(V3d.ZAxis, V3d.Zero);
        /// <summary>Invalid plane.</summary>
        public static readonly Plane3d Invalid = new Plane3d(V3d.Zero, 0.0);

        #endregion

        #region Properties

        /// <summary>
        /// The point on the plane which is closest to the origin.
        /// </summary>
        public V3d Point
        {
            get { return Normal * Distance; }
            set { Distance = Vec.Dot(Normal, value); }
        }

        /// <summary>
        /// Returns true if the normal of the plane is not the zero-vector.
        /// </summary>
        public bool IsValid => Normal != V3d.Zero;

        /// <summary>
        /// Returns true if the normal of the plane is the zero-vector.
        /// </summary>
        public bool IsInvalid => Normal == V3d.Zero;

        /// <summary>
        /// Returns the normalized <see cref="Plane3d"/> as new <see cref="Plane3d"/>.
        /// </summary>
        public Plane3d Normalized
        {
            get
            {
                double scale = Normal.Length;
                return new Plane3d(Normal / scale, Distance / scale);
            }
        }

        /// <summary>
        /// Returns the coefficients (a, b, c, d) of the normal equation: ax + by + cz + d = 0
        /// </summary>
        public V4d Coefficients => new V4d(Normal, -Distance);

        #endregion

        #region Arithmetics

        /// <summary>
        /// Normalizes this <see cref="Plane3d"/>.
        /// </summary>
        public void Normalize()
        {
            double scale = Normal.Length;
            Normal /= scale;
            Distance /= scale;
        }

        /// <summary>
        /// Changes sign of normal vector.
        /// </summary>
        public void Reverse()
        {
            Normal = -Normal;
            Distance = -Distance;
        }

        /// <summary>
        /// Returns <see cref="Plane3d"/> with normal vector in opposing direction.
        /// </summary>
        public Plane3d Reversed => new Plane3d(-Normal, -Distance);

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// </summary>
        public double Height(V3d p) => Vec.Dot(Normal, p) - Distance;

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        public int Sign(V3d p) => Height(p).Sign();

        /// <summary>
        /// Projets the given point x perpendicular on the plane
        /// and returns the nearest point on the plane.
        /// </summary>
        public V3d NearestPoint(V3d x)
        {
            var p = Point;
            return (x - Normal.Dot(x - p) * Normal);
        }

        /// <summary>
        /// Transforms the plane with a given trafo using the inverse
        /// transposed matrix.
        /// </summary>
        public Plane3d Transformed(Trafo3d trafo) => new Plane3d(
            trafo.Backward.TransposedTransformDir(Normal),
            trafo.Forward.TransformPos(Point));

        /// <summary>
        /// Transforms the plane with a given matrix. The matrix is assumed
        /// to be the inverse transpose of the transformation.
        /// The returned plane also gets normalized.
        /// </summary>
        public Plane3d Transformed(M44d trafo)
        {
            var p = trafo.Transform(Coefficients);
            return new Plane3d(p.XYZ, -p.W).Normalized; 
        }

        #endregion

        #region Operators

        public static bool operator ==(Plane3d a, Plane3d b)
            => a.Normal == b.Normal && a.Distance == b.Distance;

        public static bool operator !=(Plane3d a, Plane3d b)
            => a.Normal != b.Normal || a.Distance != b.Distance;

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Normal, Distance);

        public override bool Equals(object other) => (other is Plane3d value)
            ? Normal.Equals(value.Normal) && Distance.Equals(value.Distance)
            : false;

        public override string ToString() => 
            string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Distance);

        public static Plane3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Plane3d(V3d.Parse(x[0]), double.Parse(x[1], CultureInfo.InvariantCulture));
        }

        #endregion

        #region IBoundingBox3d Members

        /// <summary>
        /// Gets entire double space as bounding box.
        /// </summary>
        public Box3d BoundingBox3d
        {
            get
            {
                var box = new Box3d(V3d.MinValue, V3d.MaxValue);

                if (Normal == V3d.XAxis)
                {
                    box.Min.X = Distance; box.Max.X = Distance; return box;
                }
                if (Normal == V3d.YAxis)
                {
                    box.Min.Y = Distance; box.Max.Y = Distance; return box;
                }
                if (Normal == V3d.ZAxis)
                {
                    box.Min.Z = Distance; box.Max.Z = Distance; return box;
                }

                return box;
            }
        }

        #endregion
    }

    /// <summary>
    /// A plane with a specific point that can be retrieved later.
    /// </summary>
    public struct PlaneWithPoint3d : IBoundingBox3d
    {
        public V3d Normal;
        public V3d Point;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and point. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        public PlaneWithPoint3d(V3d normalizedNormal, V3d point)
        {
            Normal = normalizedNormal;
            Point = point;
        }

        #endregion

        #region Constants

        public static readonly PlaneWithPoint3d XPlane = new PlaneWithPoint3d(V3d.XAxis, V3d.Zero);
        public static readonly PlaneWithPoint3d YPlane = new PlaneWithPoint3d(V3d.YAxis, V3d.Zero);
        public static readonly PlaneWithPoint3d ZPlane = new PlaneWithPoint3d(V3d.ZAxis, V3d.Zero);
        public static readonly PlaneWithPoint3d Invalid = new PlaneWithPoint3d(V3d.Zero, V3d.Zero);

        #endregion

        #region Properties

        public PlaneWithPoint3d Normalized => new PlaneWithPoint3d(Normal.Normalized, Point);

        public Plane3d Plane3d => new Plane3d(Normal, Point);

        public bool IsInvalid => Normal == V3d.Zero;

        public PlaneWithPoint3d Reversed => new PlaneWithPoint3d(-Normal, Point);

        #endregion

        #region Arithmetics

        public void Normalize() => Normal.Normalize();

        public void Reverse() => Normal = -Normal;

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// </summary>
        public double Height(V3d p) => Vec.Dot(Normal, p - Point);

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        public int Sign(V3d p) => Height(p).Sign();

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Normal, Point);

        public override bool Equals(object other) => (other is PlaneWithPoint3d value)
            ? Normal.Equals(value.Normal) && Point.Equals(value.Point)
            : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Normal, Point);

        public static PlaneWithPoint3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new PlaneWithPoint3d(V3d.Parse(x[0]), V3d.Parse(x[1]));
        }

        #endregion

        #region IBoundingBox3d Members

        /// <summary>
        /// Gets entire double space as bounding box.
        /// </summary>
        public Box3d BoundingBox3d
        {
            get
            {
                var box = new Box3d(V3d.MinValue, V3d.MaxValue);

                if (Normal == V3d.XAxis)
                {
                    box.Min.X = Point.X; box.Max.X = Point.X; return box;
                }
                if (Normal == V3d.YAxis)
                {
                    box.Min.Y = Point.Y; box.Max.Y = Point.Y; return box;
                }
                if (Normal == V3d.ZAxis)
                {
                    box.Min.Z = Point.Z; box.Max.Z = Point.Z; return box;
                }

                return box;
            }
        }

        #endregion
    }

    /// <summary>
    /// A plane pair defines a ray at their intersection.
    /// </summary>
    public struct PlanePair3d
    {
        public Plane3d Plane0;
        public Plane3d Plane1;

        #region Constructors

        public PlanePair3d(Plane3d plane0, Plane3d plane1)
        {
            Plane0 = plane0; Plane1 = plane1;
        }

        #endregion

        #region Arithmetics

        public Ray3d GetRay3d()
        {
            Plane0.Intersects(Plane1, out Ray3d ray);
            return ray;
        }

        #endregion
    }

    /// <summary>
    /// A plane triple defines a point at their intersection.
    /// </summary>
    public struct PlaneTriple3d
    {
        public Plane3d Plane0;
        public Plane3d Plane1;
        public Plane3d Plane2;

        #region Constructors

        public PlaneTriple3d(Plane3d plane0, Plane3d plane1, Plane3d plane2)
        {
            Plane0 = plane0; Plane1 = plane1; Plane2 = plane2;
        }

        #endregion

        #region Properties

        PlanePair3d Pair01 => new PlanePair3d(Plane0, Plane1);
        PlanePair3d Pair02 => new PlanePair3d(Plane0, Plane2);
        PlanePair3d Pair12 => new PlanePair3d(Plane1, Plane2);
        PlanePair3d Pair10 => new PlanePair3d(Plane1, Plane0);
        PlanePair3d Pair20 => new PlanePair3d(Plane2, Plane0);
        PlanePair3d Pair21 => new PlanePair3d(Plane2, Plane1);    

        #endregion

        #region Arithmetics

        public V3d GetPoint()
        {
            Plane0.Intersects(Plane1, Plane2, out V3d point);
            return point;
        }

        #endregion
    }

    public static class Plane3dExtensions
    {
        /// <summary>
        /// Returns a transformation of an orthonormal basis in Plane- to WorldSpace.
        /// </summary>
        public static Trafo3d GetPlaneSpaceTransform(this Plane3d self)
            => Trafo3d.FromNormalFrame(self.Point, self.Normal);

        /// <summary>
        /// 3D world space to 2D plane space.
        /// Plane space is defined by a normal-frame from Point and Normal of the plane.
        /// </summary>
        public static M44d GetWorldToPlane(this Plane3d self)
        {
            M44d.NormalFrame(self.Point, self.Normal, out M44d _, out M44d global2local);
            return global2local;
        }

        /// <summary>
        /// 2D plane space to 3D world space.
        /// Plane space is defined by a normal-frame from Point and Normal of the plane.
        /// </summary>
        public static M44d GetPlaneToWorld(this Plane3d self)
        {
            M44d.NormalFrame(self.Point, self.Normal, out M44d local2global, out M44d _);
            return local2global;
        }

        /// <summary>
        /// Projects a point onto the plane (shortest distance).
        /// </summary>
        public static V3d Project(this Plane3d plane, V3d p) => p - plane.Height(p) * plane.Normal;

        /// <summary>
        /// Projects a point onto the plane along given direction.
        /// </summary>
        public static V3d Project(this Plane3d plane, V3d p, V3d direction)
        {
            var r = new Ray3d(p, direction);
            if (r.Intersects(plane, out double t))
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
        public static V3d[] Project(this Plane3d plane, V3d[] pointArray, int startIndex = 0, int count = 0)
        {
            if (pointArray == null) throw new ArgumentNullException();
            if (startIndex < 0 || startIndex >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count < 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count == 0) count = pointArray.Length - startIndex;

            var normal = plane.Normal;
            var result = new V3d[count];
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
        public static V3d[] Project(this Plane3d plane, V3d[] pointArray, V3d direction, int startIndex = 0, int count = 0)
        {
            if (pointArray == null) throw new ArgumentNullException();
            if (startIndex < 0 || startIndex >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count < 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException();
            if (count == 0) count = pointArray.Length - startIndex;

            var result = new V3d[count];
            for (int i = startIndex, j = 0; j < count; i++, j++)
            {
                var r = new Ray3d(pointArray[i], direction);
                if (r.Intersects(plane, out double t))
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
        public static V2d ProjectToPlaneSpace(this Plane3d plane, V3d p)
            => plane.GetWorldToPlane().TransformPos(p).XY;

        /// <summary>
        /// Projects points from world space to plane space (shortest distance).
        /// </summary>
        public static V2d[] ProjectToPlaneSpace(this Plane3d plane, V3d[] points)
        {
            var global2local = plane.GetWorldToPlane();
            return points.Map(p => global2local.TransformPos(p).XY);
        }

        /// <summary>
        /// Transforms point from plane space to world space.
        /// </summary>
        public static V3d Unproject(this Plane3d plane, V2d point)
        {
            var local2global = plane.GetPlaneToWorld();
            return local2global.TransformPos(point.XYO);
        }

        /// <summary>
        /// Transforms points from plane space to world space.
        /// </summary>
        public static V3d[] Unproject(this Plane3d plane, V2d[] points)
        {
            var local2global = plane.GetPlaneToWorld();
            return points.Map(p => local2global.TransformPos(p.XYO));
        }

        /// <summary>
        /// Transforms points from plane space to world space.
        /// </summary>
        public static V3d[] Unproject(this Plane3d plane, IReadOnlyList<V2d> points)
        {
            var local2global = plane.GetPlaneToWorld();
            var xs = new V3d[points.Count];
            for (var i = 0; i < points.Count; i++)
                xs[i] = local2global.TransformPos(points[i].XYO);
            return xs;
        }
    }
}
