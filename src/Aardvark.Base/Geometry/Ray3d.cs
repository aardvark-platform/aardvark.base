using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    /// <summary>
    /// A three-dimensional ray with an origin and a direction.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray3d : IValidity, IBoundingBox3d
    {
        public V3d Origin;
        public V3d Direction;

        #region Constructors

        /// <summary>
        /// Creates Ray from origin point and directional vector
        /// </summary>
        public Ray3d(V3d origin, V3d direction)
        {
            Origin = origin;
            Direction = direction;
        }


        public static Ray3d FromEndPoints(V3d origin, V3d target)
        {
            return new Ray3d(origin, target - origin);
        }
            
        #endregion

        #region Constants

        /// <summary>
        /// An invalid ray has a zero direction.
        /// </summary>
        public static readonly Ray3d Invalid = new Ray3d(V3d.NaN, V3d.Zero);

        #endregion

        #region Properties

        public bool IsValid { get { return Direction != V3d.Zero; } }
        public bool IsInvalid { get { return Direction == V3d.Zero; } }

        public Line3d Line3d
        {
            get { return new Line3d(Origin, Origin + Direction); }
        }

        public Ray3d Reversed
        {
            get { return new Ray3d(Origin, -Direction); }
        }

        #endregion

        #region Ray Arithmetics

        /// <summary>
        /// Gets the point on the ray that is t * Direction from Origin.
        /// </summary>
        public V3d GetPointOnRay(double t)
        {
            return Origin + Direction * t;
        }

        /// <summary>
        /// Gets the t for a point p on this ray. 
        /// </summary>
        public double GetT(V3d p)
        {
            var v = p - Origin;
            var d = Direction.Abs;

            if (d.X > d.Y)
                return (d.X > d.Z) ? (v.X / Direction.X) : (v.Z / Direction.Z);
            else
                return (d.Y > d.Z) ? (v.Y / Direction.Y) : (v.Z / Direction.Z);
        }

        /// <summary>
        /// Gets the t of the closest point on the ray for any point p.
        /// </summary>
        public double GetTOfProjectedPoint(V3d p)
        {
            var v = p - Origin;
            return v.Dot(Direction) / Direction.LengthSquared;
        }

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public bool Hits(
            Ray3d ray, ref RayHit3d hit
            )
        {
            return HitsRay(ray, double.MinValue, double.MaxValue, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public bool Hits(
            Ray3d ray,
            double tmin, double tmax,
            ref RayHit3d hit
            )
        {
            return HitsRay(ray, tmin, tmax, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the other ray before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public bool HitsRay(
            Ray3d ray,
            double tmin, double tmax,
            ref RayHit3d hit
            )
        {
            V3d d = Origin - ray.Origin;
            V3d u = Direction;
            V3d v = ray.Direction;
            V3d n = u.Cross(v);

            if (Fun.IsTiny(d.Length)) return true;
            else if (Fun.IsTiny(u.Cross(v).Length)) return false;
            else
            {
                //-t0*u + t1*v + t2*n == d
                //M = {-u,v,n}
                //M*{t0,t1,t2}T == d
                //{t0,t1,t2}T == M^-1 * d

                M33d M = new M33d();
                M.C0 = -u;
                M.C1 = v;
                M.C2 = n;

                if (M.Invertible)
                {
                    V3d t = M.Inverse * d;
                    if (Fun.IsTiny(t.Z))
                    {
                        ProcessHits(t.X, double.MaxValue, tmin, tmax, ref hit);
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
        }

        /// <summary>
        /// Returns true if the ray hits the triangle before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit.
        /// </summary>
        public bool Hits(
            Triangle3d triangle, ref RayHit3d hit
            )
        {
            return HitsTrianglePointAndEdges(
                        triangle.P0, triangle.Edge01, triangle.Edge02,
                        double.MinValue, double.MaxValue, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
        /// </summary>
        public bool Hits(
            Triangle3d triangle,
            double tmin, double tmax,
            ref RayHit3d hit
            )
        {
            return HitsTrianglePointAndEdges(
                        triangle.P0, triangle.Edge01, triangle.Edge02,
                        tmin, tmax, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
        /// </summary>
        public bool HitsTriangle(
            V3d p0, V3d p1, V3d p2,
            double tmin, double tmax,
            ref RayHit3d hit
            )
        {
            V3d edge01 = p1 - p0;
            V3d edge02 = p2 - p0;
            V3d plane = V3d.Cross(Direction, edge02);
            double det = V3d.Dot(edge01, plane);
            if (det > -0.0000001 && det < 0.0000001) return false;
            // ray ~= paralell / Triangle
            V3d tv = Origin - p0;
            det = 1.0 / det;  // det is now inverse det
            double u = V3d.Dot(tv, plane) * det;
            if (u < 0.0 || u > 1.0) return false;
            plane = V3d.Cross(tv, edge01); // plane is now qv
            double v = V3d.Dot(Direction, plane) * det;
            if (v < 0.0 || u + v > 1.0) return false;
            double t = V3d.Dot(edge02, plane) * det;
            if (t < tmin || t >= tmax || t >= hit.T) return false;
            hit.T = t;
            hit.Point = Origin + t * Direction;
            hit.Coord.X = u; hit.Coord.Y = v;
            hit.BackSide = (det < 0.0);
            return true;
        }

        /// <summary>
        /// Returns true if the ray hits the triangle within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
        /// </summary>
        public bool HitsTrianglePointAndEdges(
            V3d p0, V3d edge01, V3d edge02,
            double tmin, double tmax,
            ref RayHit3d hit
            )
        {
            V3d plane = V3d.Cross(Direction, edge02);
            double det = V3d.Dot(edge01, plane);
            if (det > -0.0000001 && det < 0.0000001) return false;
            // ray ~= paralell / Triangle
            V3d tv = Origin - p0;
            det = 1.0 / det;  // det is now inverse det
            double u = V3d.Dot(tv, plane) * det;
            if (u < 0.0 || u > 1.0) return false;
            plane = V3d.Cross(tv, edge01); // plane is now qv
            double v = V3d.Dot(Direction, plane) * det;
            if (v < 0.0 || u + v > 1.0) return false;
            double t = V3d.Dot(edge02, plane) * det;
            if (t < tmin || t >= tmax || t >= hit.T) return false;
            hit.T = t;
            hit.Point = Origin + t * Direction;
            hit.Coord.X = u; hit.Coord.Y = v;
            hit.BackSide = (det < 0.0);
            return true;
        }

        /// <summary>
        /// Returns true if the ray hits the quad before the parameter
        /// value contained in the supplied hit. Detailed information about
        /// the hit is returned in the supplied hit. In order to obtain all
        /// potential hits, the supplied hit can be initialized with
        /// RayHit3d.MaxRange.
        /// </summary>
        public bool Hits(
            Quad3d quad, ref RayHit3d hit
            )
        {
            return HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3,
                            double.MinValue, double.MaxValue, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. Detailed information about the hit is
        /// returned in the supplied hit. In order to obtain all potential
        /// hits, the supplied hit can be initialized with RayHit3d.MaxRange.
        /// </summary>
        public bool Hits(
            Quad3d quad,
            double tmin, double tmax,
            ref RayHit3d hit
            )
        {
            return HitsQuad(quad.P0, quad.P1, quad.P2, quad.P3,
                            tmin, tmax, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the quad within the supplied
        /// parameter interval and before the parameter value contained
        /// in the supplied hit. The quad is considered to consist of the
        /// two triangles [p0,p1,p2] and [p0,p2,p3]. Detailed information
        /// about the hit is returned in the supplied hit. In order to obtain
        /// all potential hits, the supplied hit can be initialized with
        /// RayHit3d.MaxRange.
        /// </summary>
        public bool HitsQuad(
            V3d p0, V3d p1, V3d p2, V3d p3,
            double tmin, double tmax,
            ref RayHit3d hit
            )
        {
            V3d e02 = p2 - p0;
            bool result = false;
            if (HitsTrianglePointAndEdges(p0, p1 - p0, e02, tmin, tmax,
                                          ref hit))
            {
                hit.Coord.X += hit.Coord.Y;
                result = true;
            }
            if (HitsTrianglePointAndEdges(p0, e02, p3 - p0, tmin, tmax,
                                          ref hit))
            {
                hit.Coord.Y += hit.Coord.X;
                result = true;
            }
            return result;
        }

        private bool ComputeHit(
              double t,
              double tmin, double tmax,
              ref RayHit3d hit)
        {
            if (t >= tmin)
            {
                if (t < tmax && t < hit.T)
                {
                    hit.T = t;
                    hit.Point = GetPointOnRay(t);
                    hit.Coord = V2d.NaN;
                    hit.BackSide = false;
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool GetClosestHit(
                double t1, double t2,
                double tmin, double tmax,
                ref RayHit3d hit)
        {
            return t1 < t2
                  ? ProcessHits(t1, t2, tmin, tmax, ref hit)
                  : ProcessHits(t2, t1, tmin, tmax, ref hit);
        }


        private bool ProcessHits(
                double t1, double t2,
                double tmin, double tmax,
                ref RayHit3d hit)
        {
            if (t1 >= tmin)
            {
                if (t1 < tmax && t1 < hit.T)
                {
                    hit.T = t1;
                    hit.Point = GetPointOnRay(t1);
                    hit.Coord = V2d.NaN;
                    hit.BackSide = false;
                    return true;
                }
                return false;
            }
            if (t2 >= tmin)
            {
                if (t2 < tmax && t2 < hit.T)
                {
                    hit.T = t2;
                    hit.Point = GetPointOnRay(t2);
                    hit.Coord = V2d.NaN;
                    hit.BackSide = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the ray hits the sphere given by center and
        /// radius within the supplied parameter interval and before the
        /// parameter value contained in the supplied hit. Note that a
        /// hit is only registered if the front or the backsurface is
        /// encountered within the interval.
        /// </summary>
        public bool HitsSphere(
                V3d center, double radius,
                double tmin, double tmax,
                ref RayHit3d hit)
        {
            V3d originSubCenter = Origin - center;
            double a = Direction.LengthSquared;
            double b = Direction.Dot(originSubCenter);
            double c = originSubCenter.LengthSquared - radius * radius;

            // --------------------- quadric equation : a t^2  + 2b t + c = 0
            double d = b * b - a * c;           // factor 2 was eliminated

            if (d < Constant<double>.PositiveTinyValue) // no root ?
                return false;                           // then exit

            if (b > 0.0)                        // stable way to calculate
                d = -Fun.Sqrt(d) - b;           // the roots of a quadratic
            else                                // equation
                d = Fun.Sqrt(d) - b;

            double t1 = d / a;
            double t2 = c / d;  // Vieta : t1 * t2 == c/a

            return t1 < t2
                    ? ProcessHits(t1, t2, tmin, tmax, ref hit)
                    : ProcessHits(t2, t1, tmin, tmax, ref hit);
        }

        /// <summary>
        /// Returns true if the ray hits the supplied sphere within the
        /// supplied parameter interval and before the parameter value
        /// contained in the supplied hit. Note that a hit is only
        /// registered if the front or the backsurface is encountered
        /// within the interval.
        /// </summary>
        public bool Hits(
                Sphere3d sphere,
                double tmin, double tmax,
                ref RayHit3d hit)
        {
            return HitsSphere(sphere.Center, sphere.Radius, tmin, tmax, ref hit);
        }

        public bool HitsPlane(Plane3d plane, double tmin, double tmax, ref RayHit3d hit)
        {
            var dc = plane.Normal.Dot(Direction);
            var dw = plane.Distance - plane.Normal.Dot(Origin);

            // If parallel to plane
            if (dc == 0.0)
                return false;
            
            var t = dw / dc;
            return (ComputeHit(t, tmin, tmax, ref hit));
        }

        public bool HitsCircle(Circle3d circle, double tmin, double tmax, ref RayHit3d hit)
        {
            var dc = circle.Normal.Dot(Direction);
            var dw = circle.Normal.Dot(circle.Center - Origin);

            // If parallel to plane
            if (dc == 0.0)
                return false;

            var t = dw / dc;
            if (!ComputeHit(t, tmin, tmax, ref hit))
                return false;

            if (V3d.Distance(hit.Point, circle.Center) > circle.Radius)
            {
                hit.Point = V3d.NaN;
                hit.T = tmax;
                return false;
            }
            return true;
        }
        
        public bool HitsCylinder(Cylinder3d cylinder,
                double tmin, double tmax,
                ref RayHit3d hit)
        {
            var axisDir = cylinder.Axis.Direction.Normalized;

            // Vector Cyl.P0 -> Ray.Origin
            var op = Origin - cylinder.P0;

            // normal RayDirection - CylinderAxis
            var normal = Direction.Cross(axisDir);
            var unitNormal = normal.Normalized;

            // normal (Vec Cyl.P0 -> Ray.Origin) - CylinderAxis
            var normal2 = op.Cross(axisDir);
            var t = -normal2.Dot(unitNormal) / normal.Length;

            var radius = cylinder.Radius;
            if (cylinder.DistanceScale != 0)
            {   // cylinder gets bigger, the further away it is
                var pnt = GetPointOnRay(t);

                var dis = V3d.Distance(pnt, this.Origin);
                radius = ((cylinder.Radius / cylinder.DistanceScale) * dis) * 2;
            }

            // between enitre rays (caps are ignored)
            var shortestDistance = Fun.Abs(op.Dot(unitNormal));
            if (shortestDistance <= radius)
            {
                var s = Fun.Abs(Fun.Sqrt(radius.Square() - shortestDistance.Square()) / Direction.Length);

                var t1 = t - s; // first hit of Cylinder shell
                var t2 = t + s; // second hit of Cylinder shell

                if (t1 > tmin && t1 < tmax) tmin = t1;
                if (t2 < tmax && t2 > tmin) tmax = t2;

                hit.T = t1;
                hit.Point = GetPointOnRay(t1);

                // check if found point is outside of Cylinder Caps
                var bottomPlane = new Plane3d(cylinder.Circle0.Normal, cylinder.Circle0.Center);
                var topPlane = new Plane3d(cylinder.Circle1.Normal, cylinder.Circle1.Center);
                var heightBottom = bottomPlane.Height(hit.Point);
                var heightTop = topPlane.Height(hit.Point);
                // t1 lies outside of caps => find closest cap hit
                if (heightBottom > 0 || heightTop > 0)
                {
                    hit.T = tmax;
                    // intersect with bottom Cylinder Cap
                    var bottomHit = HitsPlane(bottomPlane, tmin, tmax, ref hit);
                    // intersect with top Cylinder Cap
                    var topHit = HitsPlane(topPlane, tmin, tmax, ref hit);
                    
                    // hit still close enough to cylinder axis?
                    var distance = cylinder.Axis.Ray3d.GetMinimalDistanceTo(hit.Point);

                    if (distance <= radius && (bottomHit || topHit))
                        return true;
                }
                else
                    return true;
            }

            hit.T = tmax;
            hit.Point = V3d.NaN;
            return false;
        }

        public bool Hits(
                Cylinder3d cylinder,
                double tmin, double tmax,
                ref RayHit3d hit)
        {
            var intersects = HitsCylinder(cylinder, tmin, tmax, ref hit);

            return intersects;
        }

        /// <summary>
        /// Returns the ray transformed with the given matrix.
        /// </summary>
        public Ray3d Transformed(M44d mat)
        {
            return new Ray3d(mat.TransformPos(Origin),
                             mat.TransformDir(Direction));
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Ray3d a, Ray3d b)
        {
            return (a.Origin == b.Origin) && (a.Direction == b.Direction);
        }

        public static bool operator !=(Ray3d a, Ray3d b)
        {
            return !((a.Origin == b.Origin) && (a.Direction == b.Direction));
        }

        public int LexicalCompare(Ray3d other)
        {
            var cmp = Origin.LexicalCompare(other.Origin);
            if (cmp != 0) return cmp;
            return Direction.LexicalCompare(other.Direction);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Calculates Hash-code of the given ray.
        /// </summary>
        /// <returns>Hash-code.</returns>
        public override int GetHashCode()
        {
            return HashCode.GetCombined(Origin, Direction);
        }

        /// <summary>
        /// Checks if 2 objects are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        public override bool Equals(object other)
        {
            if (other is Ray3d)
            {
                var value = (Ray3d)other;
                return (Origin == value.Origin) && (Direction == value.Direction);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(Localization.FormatEnUS, "[{0}, {1}]", Origin, Direction);
        }

        public static Ray3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ray3d(V3d.Parse(x[0]), V3d.Parse(x[1]));
        }

        #endregion

        #region IBoundingBox3d

        public Box3d BoundingBox3d
        {
            get { return Box3d.FromPoints(Origin, Direction + Origin); }
        }

        #endregion
    }

    /// <summary>
    /// A ray hit represents the hit of a ray on a primitive object such as
    /// a triangle. It stores the ray parameter of the hit, the hit point,
    /// the hit point's coordinates, and a flag indicating if the backside
    /// of the primitive was hit. Optionally the part field can be used to
    /// store which part of a multi-part object was hit. If no multi-part
    /// objects are used, this field remains 0.
    /// </summary>
    public struct RayHit3d
    {
        public double T;
        public V3d Point;
        public V2d Coord;
        public bool BackSide;
        public int Part;

        #region Constructor

        public RayHit3d(double tMax)
        {
            T = tMax;
            Point = V3d.NaN;
            Coord = V2d.NaN;
            BackSide = false;
            Part = 0;
        }

        #endregion

        #region Constants

        public static readonly RayHit3d MaxRange = new RayHit3d(double.MaxValue);

        #endregion
    }

    /// <summary>
    /// A fast ray contains a ray and a number of precomputed flags and
    /// fields for fast intersection computation with bounding boxes and
    /// other axis-aligned sturctures such as kd-Trees.
    /// </summary>
    public struct FastRay3d
    {
        public readonly Ray3d Ray;
        public readonly Vec.DirFlags DirFlags;
        public readonly V3d InvDir;

        #region Constructors

        public FastRay3d(Ray3d ray)
        {
            Ray = ray;
            DirFlags = ray.Direction.DirFlags();
            InvDir = 1.0 / ray.Direction;
        }

        public FastRay3d(V3d origin, V3d direction)
            : this(new Ray3d(origin, direction))
        { }

        #endregion

        #region Ray Arithmetics

        public bool Intersects(
            Box3d box,
            ref double tmin,
            ref double tmax
            )
        {
            var dirFlags = DirFlags;

            if ((dirFlags & Vec.DirFlags.PositiveX) != 0)
            {
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & Vec.DirFlags.NegativeX) != 0)
            {
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & Vec.DirFlags.PositiveY) != 0)
            {
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & Vec.DirFlags.NegativeY) != 0)
            {
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if ((dirFlags & Vec.DirFlags.PositiveZ) != 0)
            {
                {
                    double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else if ((dirFlags & Vec.DirFlags.NegativeZ) != 0)
            {
                {
                    double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) tmax = t;
                }
                {
                    double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) tmin = t;
                }
            }
            else	// ray parallel to Z-plane
            {
                if (Ray.Origin.Z < box.Min.Z || Ray.Origin.Z > box.Max.Z)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        /// <summary>
        /// This variant of the intersection method returns the affected
        /// planes of the box if the box was hit.
        /// </summary>
        public bool Intersects(
            Box3d box,
            ref double tmin,
            ref double tmax,
            out Box.Flags tminFlags,
            out Box.Flags tmaxFlags
            )
        {
            var dirFlags = DirFlags;
            tminFlags = Box.Flags.None;
            tmaxFlags = Box.Flags.None;

            if ((dirFlags & Vec.DirFlags.PositiveX) != 0)
            {
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
                }
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
                }
            }
            else if ((dirFlags & Vec.DirFlags.NegativeX) != 0)
            {
                {
                    double t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
                }
                {
                    double t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
                }
            }
            else	// ray parallel to X-plane
            {
                if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                    return false;
            }

            if ((dirFlags & Vec.DirFlags.PositiveY) != 0)
            {
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
                }
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
                }
            }
            else if ((dirFlags & Vec.DirFlags.NegativeY) != 0)
            {
                {
                    double t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
                }
                {
                    double t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
                }
            }
            else	// ray parallel to Y-plane
            {
                if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                    return false;
            }

            if ((dirFlags & Vec.DirFlags.PositiveZ) != 0)
            {
                {
                    double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxZ; }
                }
                {
                    double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinZ; }
                }
            }
            else if ((dirFlags & Vec.DirFlags.NegativeZ) != 0)
            {
                {
                    double t = (box.Min.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t <= tmin) return false;
                    if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinZ; }
                }
                {
                    double t = (box.Max.Z - Ray.Origin.Z) * InvDir.Z;
                    if (t >= tmax) return false;
                    if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxZ; }
                }
            }
            else	// ray parallel to Z-plane
            {
                if (Ray.Origin.Z < box.Min.Z || Ray.Origin.Z > box.Max.Z)
                    return false;
            }

            if (tmin > tmax) return false;

            return true;
        }

        #endregion
    }

}
