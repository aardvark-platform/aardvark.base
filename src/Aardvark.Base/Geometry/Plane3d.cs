using System;
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
        public V3d Normal;
        public double Distance;

        #region Constructors

        /// <summary>
        /// Creates plane from normal vector and constant. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
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
        /// Creates plane from point and normal vector. IMPORTANT: The
        /// supplied vector has to be normalized in order for all methods
        /// to work correctly, however if only relative height computations
        /// using the <see cref="Height"/> method are necessary, the normal
        /// vector need not be normalized.
        /// </summary>
        public Plane3d(V3d normalizedNormal, V3d point)
        {
            Normal = normalizedNormal;
            Distance = V3d.Dot(normalizedNormal, point);
        }

        /// <summary>
        /// Creates a plane from 3 independent points. A normalized normal
        /// vector is computed and stored.
        /// </summary>
        public Plane3d(V3d p0, V3d p1, V3d p2)
        {
            Normal = V3d.Cross(p1 - p0, p2 - p0).Normalized;
            Distance = V3d.Dot(Normal, p0);
        }

        #endregion

        #region Constants

        public static readonly Plane3d XPlane = new Plane3d(V3d.XAxis, V3d.Zero);
        public static readonly Plane3d YPlane = new Plane3d(V3d.YAxis, V3d.Zero);
        public static readonly Plane3d ZPlane = new Plane3d(V3d.ZAxis, V3d.Zero);
        public static readonly Plane3d Invalid = new Plane3d(V3d.Zero, 0.0);

        #endregion

        #region Properties

        /// <summary>
        /// The point on the plane which is closest to the origin.
        /// </summary>
        public V3d Point
        {
            get { return Normal * Distance; }
            set { Distance = V3d.Dot(Normal, value); }
        }

        /// <summary>
        /// Returns true if the normal of the plane is not the zero-vector.
        /// </summary>
        public bool IsValid {  get { return Normal != V3d.Zero; } }

        /// <summary>
        /// Returns true if the normal of the plane is the zero-vector.
        /// </summary>
        public bool IsInvalid  { get { return Normal == V3d.Zero; } }

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
        /// Returns the coefficients (a, b, c, d) of the normal equation.
        /// </summary>
        public V4d Coefficients
        {
            get { return new V4d(Normal, -Distance); }
        }

        #endregion

        #region Arithmetics

        /// <summary>
        /// Calculates the nomalized plane of this <see cref="Plane3d"/>.
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
        /// Returns <see cref="Plane3d"/> with normal vector in opposing direction.
        /// </summary>
        /// <returns></returns>
        public Plane3d Reversed
        {
            get { return new Plane3d(-Normal, -Distance); }
        }

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// </summary>
        public double Height(V3d p)
        {
            return V3d.Dot(Normal, p) - Distance;
        }

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        public int Sign(V3d p)
        {
            return Height(p).Sign();
        }

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
        public Plane3d Transformed(Trafo3d trafo)
        {
            return new Plane3d(
                trafo.Backward.TransposedTransformDir(Normal),
                trafo.Forward.TransformPos(Point));
        }

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
        {
            return a.Normal == b.Normal && a.Distance == b.Distance;
        }

        public static bool operator !=(Plane3d a, Plane3d b)
        {
            return a.Normal != b.Normal || a.Distance != b.Distance;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Normal, Distance);
        }

        public override bool Equals(object other)
        {
            if (other is Plane3d)
            {
                var value = (Plane3d)other;
                return (Normal == value.Normal) && (Distance == value.Distance);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(Localization.FormatEnUS,
                                 "[{0}, {1}]", Normal, Distance);
        }

        public static Plane3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Plane3d(V3d.Parse(x[0]), double.Parse(x[1]));
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

        public PlaneWithPoint3d Normalized
        {
            get { return new PlaneWithPoint3d(Normal.Normalized, Point); }
        }

        public Plane3d Plane3d
        {
            get { return new Plane3d(Normal, Point); }
        }

        public bool IsInvalid
        {
            get { return Normal == V3d.Zero; }
        }

        public PlaneWithPoint3d Reversed
        {
            get { return new PlaneWithPoint3d(-Normal, Point); }
        }

        #endregion

        #region Arithmetics

        public void Normalize()
        {
            Normal.Normalize();
        }

        public void Reverse()
        {
            Normal = -Normal;
        }

        /// <summary>
        /// The signed height of the supplied point over the plane.
        /// </summary>
        public double Height(V3d p)
        {
            return V3d.Dot(Normal, p - Point);
        }

        /// <summary>
        /// The sign of the height of the point over the plane.
        /// </summary>
        public int Sign(V3d p)
        {
            return Height(p).Sign();
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Normal, Point);
        }

        public override bool Equals(object other)
        {
            if (other is PlaneWithPoint3d)
            {
                var value = (PlaneWithPoint3d)other;
                return (Normal == value.Normal) && (Point == value.Point);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(Localization.FormatEnUS,
                                 "[{0}, {1}]", Normal, Point);
        }

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
            Ray3d ray;
            Plane0.Intersects(Plane1, out ray);
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

        PlanePair3d Pair01 { get { return new PlanePair3d(Plane0, Plane1); } }
        PlanePair3d Pair02 { get { return new PlanePair3d(Plane0, Plane2); } }
        PlanePair3d Pair12 { get { return new PlanePair3d(Plane1, Plane2); } }
        PlanePair3d Pair10 { get { return new PlanePair3d(Plane1, Plane0); } }
        PlanePair3d Pair20 { get { return new PlanePair3d(Plane2, Plane0); } }
        PlanePair3d Pair21 { get { return new PlanePair3d(Plane2, Plane1); } }

        #endregion

        #region Arithmetics

        public V3d GetPoint()
        {
            V3d point;
            Plane0.Intersects(Plane1, Plane2, out point);
            return point;
        }

        #endregion
    }

    public static class Plane3dExtensions
    {
        /// <summary>
        /// 3D world space to 2D plane space.
        /// Plane space is defined by a normal-frame from Point and Normal of the plane.
        /// </summary>
        public static Euclidean3d GetWorldToPlane(this Plane3d self)
        {
            M44d _, global2local;
            M44d.NormalFrame(self.Point, self.Normal, out _, out global2local);
            return new Euclidean3d(global2local);
        }

        /// <summary>
        /// 2D plane space to 3D world space.
        /// Plane space is defined by a normal-frame from Point and Normal of the plane.
        /// </summary>
        public static Euclidean3d GetPlaneToWorld(this Plane3d self)
        {
            M44d local2global, _;
            M44d.NormalFrame(self.Point, self.Normal, out local2global, out _);
            return new Euclidean3d(local2global);
        }

        /// <summary>
        /// Projects a point onto the plane (shortest distance).
        /// </summary>
        public static V3d Project(this Plane3d plane, V3d p)
        {
            return p - plane.Height(p) * plane.Normal;
        }

        /// <summary>
        /// Projects a point onto the plane along given direction.
        /// </summary>
        public static V3d Project(this Plane3d plane, V3d p, V3d direction)
        {
            double t = 0.0;
            var r = new Ray3d(p, direction);
            if (r.Intersects(plane, out t))
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
                double t = 0.0;
                var r = new Ray3d(pointArray[i], direction);
                if (r.Intersects(plane, out t))
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
        {
            return plane.GetWorldToPlane().TransformPos(p).XY;
        }

        /// <summary>
        /// Projects points from world space to plane space (shortest distance).
        /// </summary>
        public static V2d[] ProjectToPlaneSpace(this Plane3d plane, V3d[] points)
        {
            var global2local = plane.GetWorldToPlane();
            return points.Copy(p => global2local.TransformPos(p).XY);
        }

        /// <summary>
        /// Transforms points from plane space to world space.
        /// </summary>
        public static V3d[] Unproject(this Plane3d plane, V2d[] points)
        {
            var local2global = plane.GetPlaneToWorld();
            return points.Select(p => local2global.TransformPos(p.XYO)).ToArray();
        }
    }
}
