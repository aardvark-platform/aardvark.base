using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Hull2f

    /// <summary>
    /// A hull is an alternative representation of a convex polygon.
    /// </summary>
    public partial struct Hull2f : IEquatable<Hull2f>, IValidity
    {
        public Plane2f[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty Hull3d with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2f(int count)
        {
            PlaneArray = new Plane2f[count];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2f(Plane2f[] planeArray)
        {
            PlaneArray = planeArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2f(Box2f box)
        {
            PlaneArray = new[]
            {
                new Plane2f(V2f.XAxis, box.Min),
                new Plane2f(V2f.YAxis, box.Min),
                new Plane2f(-V2f.XAxis, box.Max),
                new Plane2f(-V2f.YAxis, box.Max),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2f(Hull2f hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2f(Hull2d hull)
        {
            PlaneArray = hull.PlaneArray.Map(p => new Plane2f(p));
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Hull2f(Hull2d h)
            => new Hull2f(h);

        #endregion

        #region Constants

        public static Hull2f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Hull2f(null);
        }

        #endregion

        #region Properties

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray != null;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray == null;
        }

        public readonly int PlaneCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray.Length;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Hull2f a, Hull2f b)
            => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Hull2f a, Hull2f b)
            => !a.Equals(b);

        #endregion

        #region Override

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
        {
            if (PlaneArray == null || PlaneArray.Length == 0) return 0;
            var h = PlaneArray[0].GetHashCode();
            for (var i = 1; i < PlaneArray.Length; i++) HashCode.GetCombined(h, PlaneArray[i].GetHashCode());
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Hull2f other)
        {
            if (PlaneArray == null || other.PlaneArray == null) return false;
            for (var i = 0; i < PlaneArray.Length; i++) if (PlaneArray[i] != other.PlaneArray[i]) return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Hull2f o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => PlaneArray != null
                ? string.Format(CultureInfo.InvariantCulture, "[{0}]", string.Join(",", PlaneArray.Map(x => x.ToString())))
                : "[null]"
                ;

        /// <summary>
        /// Parses Hull2f from a string created with Hull2f.ToString().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Hull2f Parse(string s)
        {
            if (s == "[null]") return Hull2f.Invalid;
            var planes = s.NestedBracketSplitLevelOne().Select(Plane2f.Parse).ToArray();
            return new Hull2f(planes);
        }

        #endregion

        #region Transformation

        public readonly Hull2f Transformed(Trafo2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2f(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Plane2f TransformPlaneNormalized(Plane2f plane, V2f rawNormal, V2f translation)
        {
            var inverseLength = 1 / rawNormal.Length;
            var normal = rawNormal * inverseLength;
            return new Plane2f(normal, plane.Distance * inverseLength + translation.Dot(normal));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f Transformed(Euclidean2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var linear = (M22f)trafo.Rot;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), trafo.Trans);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f Transformed(Similarity2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var linear = (M22f)trafo.Rot;
            var inverseScale = 1 / trafo.Scale;
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                var normal = plane.Normal;
                hull.PlaneArray[i] = TransformPlaneNormalized(
                    plane,
                    new V2f(
                        (linear.M00 * normal.X + linear.M01 * normal.Y) * inverseScale,
                        (linear.M10 * normal.X + linear.M11 * normal.Y) * inverseScale),
                    trafo.Trans);
            }
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f Transformed(Affine2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var inverse = trafo.Inverse.Linear;
            var normalMatrix = new M22f(inverse.M00, inverse.M10, inverse.M01, inverse.M11);
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], normalMatrix.Transform(PlaneArray[i].Normal), trafo.Trans);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f Transformed(Shift2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                hull.PlaneArray[i] = new Plane2f(plane.Normal, plane.Distance + trafo.V.Dot(plane.Normal));
            }
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f Transformed(Rot2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var linear = (M22f)trafo;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), V2f.Zero);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f Transformed(Scale2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                var normal = plane.Normal;
                hull.PlaneArray[i] = TransformPlaneNormalized(plane, new V2f(normal.X / trafo.X, normal.Y / trafo.Y), V2f.Zero);
            }
            return hull;
        }

        public readonly Hull2f InvTransformed(Trafo2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var invTr = trafo.Forward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2f(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Backward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f InvTransformed(Euclidean2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var linear = (M22f)trafo.Inverse;
            var translation = -linear.Transform(trafo.Trans);
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), translation);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f InvTransformed(Similarity2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var inverseRot = (M22f)trafo.Rot.Inverse;
            var translation = -inverseRot.Transform(trafo.Trans) / trafo.Scale;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], inverseRot.Transform(PlaneArray[i].Normal) * trafo.Scale, translation);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f InvTransformed(Shift2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                hull.PlaneArray[i] = new Plane2f(plane.Normal, plane.Distance - trafo.V.Dot(plane.Normal));
            }
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f InvTransformed(Rot2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            var linear = (M22f)trafo.Inverse;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), V2f.Zero);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f InvTransformed(Scale2f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2f(new Plane2f[count]);
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], trafo.Transform(PlaneArray[i].Normal), V2f.Zero);
            return hull;
        }

        public readonly void TransformInto(Trafo2f trafo, ref Hull2f hull)
        {
            int count = PlaneCount;
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2f(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2f Reversed()
            => new Hull2f(PlaneArray.Map(p => p.Reversed));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Reverse()
            => PlaneArray.Apply(p => p.Reversed);

        #endregion
    }

    public static class Hull2fExtensions
    {
        /// <summary>
        /// Creates an inward hull (i.e. a hull whose normal vectors point
        /// inside) from a counter-clockwise enumerated array of polygon
        /// points.
        /// </summary>
        public static Hull2f ToInwardHull(this V2f[] polygon, int pointCount = 0)
        {
            if (pointCount == 0) pointCount = polygon.Length;
            var planeArray = new Plane2f[pointCount];
            var p0 = polygon[pointCount - 1];
            for (int i = 0; i < pointCount; i++)
            {
                var p1 = polygon[i];
                planeArray[i] = new Plane2f((p1 - p0).Rot90.Normalized, p0);
                p0 = p1;
            }
            return new Hull2f(planeArray);
        }

        /// <summary>
        /// Returns true if the supplied point is inside a hull with
        /// planes whose normal vectors point to the inside of the hull.
        /// The optional offset parameter is measured in normal direction,
        /// i.e. a positive offset makes the hull smaller.
        /// </summary>
        public static bool IsInsideInwardHull(this V2f point, Hull2f hull, float offset = 0)
        {
            for (int i = 0; i < hull.PlaneCount; i++)
                if (hull.PlaneArray[i].Height(point) < offset) return false;
            return true;
        }

        /// <summary>
        /// Returns unordered set of corners of this hull.
        /// </summary>
        public static HashSet<V2f> ComputeCorners(this Hull2f hull)
        {
            var corners = new HashSet<V2f>();
            int count = hull.PlaneArray.Length;
            for (var i0 = 0; i0 < count; i0++)
            {
                for (var i1 = i0 + 1; i1 < count; i1++)
                {
                    if (hull.PlaneArray[i0].Intersects(hull.PlaneArray[i1], out V2f temp))
                    {
                        if (temp.IsNaN || temp.AnyInfinity) continue;

                        var inside = true;
                        for (var j = 0; j < count; j++)
                        {
                            if (j == i0 || j == i1) continue;
                            var h = hull.PlaneArray[j].Height(temp);
                            if (h > 0) { inside = false; break; }
                        }

                        if (inside)
                        {
                            corners.Add(temp);
                        }
                    }
                }
            }

            return corners;
        }
    }

    #endregion

    #region Hull2d

    /// <summary>
    /// A hull is an alternative representation of a convex polygon.
    /// </summary>
    public partial struct Hull2d : IEquatable<Hull2d>, IValidity
    {
        public Plane2d[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty Hull3d with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2d(int count)
        {
            PlaneArray = new Plane2d[count];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2d(Plane2d[] planeArray)
        {
            PlaneArray = planeArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2d(Box2d box)
        {
            PlaneArray = new[]
            {
                new Plane2d(V2d.XAxis, box.Min),
                new Plane2d(V2d.YAxis, box.Min),
                new Plane2d(-V2d.XAxis, box.Max),
                new Plane2d(-V2d.YAxis, box.Max),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2d(Hull2d hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull2d(Hull2f hull)
        {
            PlaneArray = hull.PlaneArray.Map(p => new Plane2d(p));
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Hull2d(Hull2f h)
            => new Hull2d(h);

        #endregion

        #region Constants

        public static Hull2d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Hull2d(null);
        }

        #endregion

        #region Properties

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray != null;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray == null;
        }

        public readonly int PlaneCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray.Length;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Hull2d a, Hull2d b)
            => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Hull2d a, Hull2d b)
            => !a.Equals(b);

        #endregion

        #region Override

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
        {
            if (PlaneArray == null || PlaneArray.Length == 0) return 0;
            var h = PlaneArray[0].GetHashCode();
            for (var i = 1; i < PlaneArray.Length; i++) HashCode.GetCombined(h, PlaneArray[i].GetHashCode());
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Hull2d other)
        {
            if (PlaneArray == null || other.PlaneArray == null) return false;
            for (var i = 0; i < PlaneArray.Length; i++) if (PlaneArray[i] != other.PlaneArray[i]) return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object other)
            => (other is Hull2d o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
            => PlaneArray != null
                ? string.Format(CultureInfo.InvariantCulture, "[{0}]", string.Join(",", PlaneArray.Map(x => x.ToString())))
                : "[null]"
                ;

        /// <summary>
        /// Parses Hull2d from a string created with Hull2d.ToString().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Hull2d Parse(string s)
        {
            if (s == "[null]") return Hull2d.Invalid;
            var planes = s.NestedBracketSplitLevelOne().Select(Plane2d.Parse).ToArray();
            return new Hull2d(planes);
        }

        #endregion

        #region Transformation

        public readonly Hull2d Transformed(Trafo2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2d(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Plane2d TransformPlaneNormalized(Plane2d plane, V2d rawNormal, V2d translation)
        {
            var inverseLength = 1 / rawNormal.Length;
            var normal = rawNormal * inverseLength;
            return new Plane2d(normal, plane.Distance * inverseLength + translation.Dot(normal));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d Transformed(Euclidean2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var linear = (M22d)trafo.Rot;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), trafo.Trans);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d Transformed(Similarity2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var linear = (M22d)trafo.Rot;
            var inverseScale = 1 / trafo.Scale;
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                var normal = plane.Normal;
                hull.PlaneArray[i] = TransformPlaneNormalized(
                    plane,
                    new V2d(
                        (linear.M00 * normal.X + linear.M01 * normal.Y) * inverseScale,
                        (linear.M10 * normal.X + linear.M11 * normal.Y) * inverseScale),
                    trafo.Trans);
            }
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d Transformed(Affine2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var inverse = trafo.Inverse.Linear;
            var normalMatrix = new M22d(inverse.M00, inverse.M10, inverse.M01, inverse.M11);
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], normalMatrix.Transform(PlaneArray[i].Normal), trafo.Trans);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d Transformed(Shift2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                hull.PlaneArray[i] = new Plane2d(plane.Normal, plane.Distance + trafo.V.Dot(plane.Normal));
            }
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d Transformed(Rot2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var linear = (M22d)trafo;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), V2d.Zero);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d Transformed(Scale2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                var normal = plane.Normal;
                hull.PlaneArray[i] = TransformPlaneNormalized(plane, new V2d(normal.X / trafo.X, normal.Y / trafo.Y), V2d.Zero);
            }
            return hull;
        }

        public readonly Hull2d InvTransformed(Trafo2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var invTr = trafo.Forward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2d(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Backward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d InvTransformed(Euclidean2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var linear = (M22d)trafo.Inverse;
            var translation = -linear.Transform(trafo.Trans);
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), translation);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d InvTransformed(Similarity2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var inverseRot = (M22d)trafo.Rot.Inverse;
            var translation = -inverseRot.Transform(trafo.Trans) / trafo.Scale;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], inverseRot.Transform(PlaneArray[i].Normal) * trafo.Scale, translation);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d InvTransformed(Shift2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            for (int i = 0; i < count; i++)
            {
                var plane = PlaneArray[i];
                hull.PlaneArray[i] = new Plane2d(plane.Normal, plane.Distance - trafo.V.Dot(plane.Normal));
            }
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d InvTransformed(Rot2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var linear = (M22d)trafo.Inverse;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], linear.Transform(PlaneArray[i].Normal), V2d.Zero);
            return hull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d InvTransformed(Scale2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i] = TransformPlaneNormalized(PlaneArray[i], trafo.Transform(PlaneArray[i].Normal), V2d.Zero);
            return hull;
        }

        public readonly void TransformInto(Trafo2d trafo, ref Hull2d hull)
        {
            int count = PlaneCount;
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2d(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Hull2d Reversed()
            => new Hull2d(PlaneArray.Map(p => p.Reversed));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Reverse()
            => PlaneArray.Apply(p => p.Reversed);

        #endregion
    }

    public static class Hull2dExtensions
    {
        /// <summary>
        /// Creates an inward hull (i.e. a hull whose normal vectors point
        /// inside) from a counter-clockwise enumerated array of polygon
        /// points.
        /// </summary>
        public static Hull2d ToInwardHull(this V2d[] polygon, int pointCount = 0)
        {
            if (pointCount == 0) pointCount = polygon.Length;
            var planeArray = new Plane2d[pointCount];
            var p0 = polygon[pointCount - 1];
            for (int i = 0; i < pointCount; i++)
            {
                var p1 = polygon[i];
                planeArray[i] = new Plane2d((p1 - p0).Rot90.Normalized, p0);
                p0 = p1;
            }
            return new Hull2d(planeArray);
        }

        /// <summary>
        /// Returns true if the supplied point is inside a hull with
        /// planes whose normal vectors point to the inside of the hull.
        /// The optional offset parameter is measured in normal direction,
        /// i.e. a positive offset makes the hull smaller.
        /// </summary>
        public static bool IsInsideInwardHull(this V2d point, Hull2d hull, double offset = 0)
        {
            for (int i = 0; i < hull.PlaneCount; i++)
                if (hull.PlaneArray[i].Height(point) < offset) return false;
            return true;
        }

        /// <summary>
        /// Returns unordered set of corners of this hull.
        /// </summary>
        public static HashSet<V2d> ComputeCorners(this Hull2d hull)
        {
            var corners = new HashSet<V2d>();
            int count = hull.PlaneArray.Length;
            for (var i0 = 0; i0 < count; i0++)
            {
                for (var i1 = i0 + 1; i1 < count; i1++)
                {
                    if (hull.PlaneArray[i0].Intersects(hull.PlaneArray[i1], out V2d temp))
                    {
                        if (temp.IsNaN || temp.AnyInfinity) continue;

                        var inside = true;
                        for (var j = 0; j < count; j++)
                        {
                            if (j == i0 || j == i1) continue;
                            var h = hull.PlaneArray[j].Height(temp);
                            if (h > 0) { inside = false; break; }
                        }

                        if (inside)
                        {
                            corners.Add(temp);
                        }
                    }
                }
            }

            return corners;
        }
    }

    #endregion

}
