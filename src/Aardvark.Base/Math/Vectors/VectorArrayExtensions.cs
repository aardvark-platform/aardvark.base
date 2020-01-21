using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    #region ArrayVectorExtensions

    public static class ArrayVectorExtensions // this *must not* be named 'ArrayExtensions'
    {
        public static float Lerp(this float[] array, WeightedIndex[] wia)
        {
            var result = default(float);
            foreach (var wi in wia) result += (float)wi.Weight * array[wi.Index];
            return result;
        }
        public static double Lerp(this double[] array, WeightedIndex[] wia)
        {
            var result = default(double);
            foreach (var wi in wia) result += wi.Weight * array[wi.Index];
            return result;
        }
        public static C3b Lerp(this C3b[] array, WeightedIndex[] wia)
        {
            var result = default(V3d);
            foreach (var wi in wia) result += wi.Weight * (C3d)array[wi.Index];
            return (C3b)(C3d)result;
        }
        public static C3us Lerp(this C3us[] array, WeightedIndex[] wia)
        {
            var result = default(V3d);
            foreach (var wi in wia) result += wi.Weight * (C3d)array[wi.Index];
            return (C3us)(C3d)result;
        }
        public static C3ui Lerp(this C3ui[] array, WeightedIndex[] wia)
        {
            var result = default(V3d);
            foreach (var wi in wia) result += wi.Weight * (C3d)array[wi.Index];
            return (C3ui)(C3d)result;
        }
        public static C3f Lerp(this C3f[] array, WeightedIndex[] wia)
        {
            var result = default(V3d);
            foreach (var wi in wia) result += wi.Weight * (C3d)array[wi.Index];
            return (C3f)(C3d)result;
        }
        public static C3d Lerp(this C3d[] array, WeightedIndex[] wia)
        {
            var result = default(V3d);
            foreach (var wi in wia) result += wi.Weight * array[wi.Index];
            return (C3d)result;
        }
        public static C4b Lerp(this C4b[] array, WeightedIndex[] wia)
        {
            var result = default(V4d);
            foreach (var wi in wia) result += wi.Weight * (C4d)array[wi.Index];
            return (C4b)(C4d)result;
        }
        public static C4us Lerp(this C4us[] array, WeightedIndex[] wia)
        {
            var result = default(V4d);
            foreach (var wi in wia) result += wi.Weight * (C4d)array[wi.Index];
            return (C4us)(C4d)result;
        }
        public static C4ui Lerp(this C4ui[] array, WeightedIndex[] wia)
        {
            var result = default(V4d);
            foreach (var wi in wia) result += wi.Weight * (C4d)array[wi.Index];
            return (C4ui)(C4d)result;
        }
        public static C4f Lerp(this C4f[] array, WeightedIndex[] wia)
        {
            var result = default(V4d);
            foreach (var wi in wia) result += wi.Weight * (C4d)array[wi.Index];
            return (C4f)(C4d)result;
        }
        public static C4d Lerp(this C4d[] array, WeightedIndex[] wia)
        {
            var result = default(V4d);
            foreach (var wi in wia) result += wi.Weight * array[wi.Index];
            return (C4d)result;
        }
        public static V2f Lerp(this V2f[] array, WeightedIndex[] wia)
        {
            var result = default(V2d);
            foreach (var wi in wia) result += wi.Weight * (V2d)array[wi.Index];
            return (V2f)result;
        }
        public static V2d Lerp(this V2d[] array, WeightedIndex[] wia)
        {
            var result = default(V2d);
            foreach (var wi in wia) result += wi.Weight * array[wi.Index];
            return result;
        }
        public static V3f Lerp(this V3f[] array, WeightedIndex[] wia)
        {
            var result = default(V3d);
            foreach (var wi in wia) result += wi.Weight * (V3d)array[wi.Index];
            return (V3f)result;
        }
        public static V3d Lerp(this V3d[] array, WeightedIndex[] wia)
        {
            var result = default(V3d);
            foreach (var wi in wia) result += wi.Weight * array[wi.Index];
            return result;
        }
        public static V4f Lerp(this V4f[] array, WeightedIndex[] wia)
        {
            var result = default(V4d);
            foreach (var wi in wia) result += wi.Weight * (V4d)array[wi.Index];
            return (V4f)result;
        }
        public static V4d Lerp(this V4d[] array, WeightedIndex[] wia)
        {
            var result = default(V4d);
            foreach (var wi in wia) result += wi.Weight * array[wi.Index];
            return result;
        }

        static Dictionary<Type, Func<Array, int[], Dictionary<int, Line1iPoint>, Array>>
            s_backwardIndexedLerpMap
            = new Dictionary<Type, Func<Array, int[], Dictionary<int, Line1iPoint>, Array>>
            {
                { typeof(float[]), (a, bm, im) => (Array)BackwardIndexedCopy((float[])a, bm, im, (t, u, v) => Fun.Lerp((float) t, u, v)) },
                { typeof(double[]), (a, bm, im) => (Array)BackwardIndexedCopy((double[])a, bm, im, Fun.Lerp) },
                { typeof(C3b[]), (a, bm, im) => (Array)BackwardIndexedCopy((C3b[])a, bm, im, Fun.Lerp) },
                { typeof(C3us[]), (a, bm, im) => (Array)BackwardIndexedCopy((C3us[])a, bm, im, Fun.Lerp) },
                { typeof(C3ui[]), (a, bm, im) => (Array)BackwardIndexedCopy((C3ui[])a, bm, im, Fun.Lerp) },
                { typeof(C3f[]), (a, bm, im) => (Array)BackwardIndexedCopy((C3f[])a, bm, im, (t, u, v) => Fun.Lerp((float) t, u, v)) },
                { typeof(C3d[]), (a, bm, im) => (Array)BackwardIndexedCopy((C3d[])a, bm, im, Fun.Lerp) },
                { typeof(C4b[]), (a, bm, im) => (Array)BackwardIndexedCopy((C4b[])a, bm, im, Fun.Lerp) },
                { typeof(C4us[]), (a, bm, im) => (Array)BackwardIndexedCopy((C4us[])a, bm, im, Fun.Lerp) },
                { typeof(C4ui[]), (a, bm, im) => (Array)BackwardIndexedCopy((C4ui[])a, bm, im, Fun.Lerp) },
                { typeof(C4f[]), (a, bm, im) => (Array)BackwardIndexedCopy((C4f[])a, bm, im, (t, u, v) => Fun.Lerp((float) t, u, v)) },
                { typeof(C4d[]), (a, bm, im) => (Array)BackwardIndexedCopy((C4d[])a, bm, im, Fun.Lerp) },
                { typeof(V2f[]), (a, bm, im) => (Array)BackwardIndexedCopy((V2f[])a, bm, im, (t, u, v) => Fun.Lerp((float) t, u, v)) },
                { typeof(V2d[]), (a, bm, im) => (Array)BackwardIndexedCopy((V2d[])a, bm, im, Fun.Lerp) },
                { typeof(V3f[]), (a, bm, im) => (Array)BackwardIndexedCopy((V3f[])a, bm, im, (t, u, v) => Fun.Lerp((float) t, u, v)) },
                { typeof(V3d[]), (a, bm, im) => (Array)BackwardIndexedCopy((V3d[])a, bm, im, Fun.Lerp) },
                { typeof(V4f[]), (a, bm, im) => (Array)BackwardIndexedCopy((V4f[])a, bm, im, (t, u, v) => Fun.Lerp((float) t, u, v)) },
                { typeof(V4d[]), (a, bm, im) => (Array)BackwardIndexedCopy((V4d[])a, bm, im, Fun.Lerp) },
            };


        public static Array BackwardIndexedLerpCopy(
            this Array array,
            int[] backwardMap,
            Dictionary<int, Line1iPoint> interpolationMap)
        {
            var type = array.GetType();
            return s_backwardIndexedLerpMap[type](
                            array, backwardMap, interpolationMap);
        }


        static Dictionary<Type, Func<Array, int[], Dictionary<int, Line1iPoint>, Array>>
            s_backwardIndexedNormalMap
            = new Dictionary<Type, Func<Array, int[], Dictionary<int, Line1iPoint>, Array>>
            {
                { typeof(V3f[]), (a, bm, im) =>
                    (Array)BackwardIndexedCopy(
                            (V3f[])a, bm, im, (t, v0, v1) => Fun.Lerp((float) t, v0, v1).Normalized) },
                { typeof(V3d[]), (a, bm, im) =>
                    (Array)BackwardIndexedCopy(
                            (V3d[])a, bm, im, (t, v0, v1) => Fun.Lerp((float) t, v0, v1).Normalized) },
            };

        public static Array BackwardIndexedNormalCopy(
            this Array array,
            int[] backwardMap,
            Dictionary<int, Line1iPoint> interpolationMap)
        {
            var type = array.GetType();
            return s_backwardIndexedNormalMap[type](
                            array, backwardMap, interpolationMap);
        }

        public static T[] BackwardIndexedCopy<T>(
            this T[] array,
            int[] backwardMap,
            Dictionary<int, Line1iPoint> interpolationMap,
            Func<double, T, T, T> interpolator)
        {
            int length = backwardMap.Length;
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                int si = backwardMap[i];
                if (si < 0)
                {
                    var sp = interpolationMap[i];
                    result[i] = interpolator(sp.T,
                        array[sp.Line.I0], array[sp.Line.I1]);
                }
                else
                    result[i] = array[si];
            }
            return result;
        }

        public static void CombineTo<T>(
                this T[] source, IEnumerable<WeightedIndex[]> weightedIndexArrays,
                Func<T[], WeightedIndex[], T> combine,
                T[] target, int offset, int length)
        {
            length += offset;
            foreach (var wia in weightedIndexArrays)
            {
                target[offset++] = combine(source, wia);
                if (offset == length) break;
            }
        }

        private static Dictionary<Type,
            Action<Array, IEnumerable<WeightedIndex[]>, Array, int, int>>
                s_lerpToOffsetMap =
            new Dictionary<Type,
                    Action<Array, IEnumerable<WeightedIndex[]>, Array, int, int>>
            {
                { typeof(float[]), (s, w, t, o, l) => CombineTo((float[])s, w, Lerp, (float[])t, o, l) },
                { typeof(double[]), (s, w, t, o, l) => CombineTo((double[])s, w, Lerp, (double[])t, o, l) },
                { typeof(C3b[]), (s, w, t, o, l) => CombineTo((C3b[])s, w, Lerp, (C3b[])t, o, l) },
                { typeof(C3us[]), (s, w, t, o, l) => CombineTo((C3us[])s, w, Lerp, (C3us[])t, o, l) },
                { typeof(C3ui[]), (s, w, t, o, l) => CombineTo((C3ui[])s, w, Lerp, (C3ui[])t, o, l) },
                { typeof(C3f[]), (s, w, t, o, l) => CombineTo((C3f[])s, w, Lerp, (C3f[])t, o, l) },
                { typeof(C3d[]), (s, w, t, o, l) => CombineTo((C3d[])s, w, Lerp, (C3d[])t, o, l) },
                { typeof(C4b[]), (s, w, t, o, l) => CombineTo((C4b[])s, w, Lerp, (C4b[])t, o, l) },
                { typeof(C4us[]), (s, w, t, o, l) => CombineTo((C4us[])s, w, Lerp, (C4us[])t, o, l) },
                { typeof(C4ui[]), (s, w, t, o, l) => CombineTo((C4ui[])s, w, Lerp, (C4ui[])t, o, l) },
                { typeof(C4f[]), (s, w, t, o, l) => CombineTo((C4f[])s, w, Lerp, (C4f[])t, o, l) },
                { typeof(C4d[]), (s, w, t, o, l) => CombineTo((C4d[])s, w, Lerp, (C4d[])t, o, l) },
                { typeof(V2f[]), (s, w, t, o, l) => CombineTo((V2f[])s, w, Lerp, (V2f[])t, o, l) },
                { typeof(V2d[]), (s, w, t, o, l) => CombineTo((V2d[])s, w, Lerp, (V2d[])t, o, l) },
                { typeof(V3f[]), (s, w, t, o, l) => CombineTo((V3f[])s, w, Lerp, (V3f[])t, o, l) },
                { typeof(V3d[]), (s, w, t, o, l) => CombineTo((V3d[])s, w, Lerp, (V3d[])t, o, l) },
                { typeof(V4f[]), (s, w, t, o, l) => CombineTo((V4f[])s, w, Lerp, (V4f[])t, o, l) },
                { typeof(V4d[]), (s, w, t, o, l) => CombineTo((V4d[])s, w, Lerp, (V4d[])t, o, l) },
            };

        /// <summary>
        /// Fills the target array starting at offset for length entries
        /// with Lerp/weighted combinations of the source array.
        /// </summary>
        public static Array LerpTo(
                this Array source,
                IEnumerable<WeightedIndex[]> weightedIndexArrays,
                Array target, int offset, int length)
        {
            var type = source.GetType();
            s_lerpToOffsetMap[type](
                    source, weightedIndexArrays, target, offset, length);
            return target;
        }

        /// <summary>
        /// Fills the target array starting at offset with Lerp/weighted
        /// combinations of the source array.
        /// </summary>
        public static Array LerpTo(
                this Array source,
                IEnumerable<WeightedIndex[]> weightedIndexArrays,
                Array target, int offset)
        {
            int length = Fun.Min(weightedIndexArrays.Count(), target.Length - offset);
            return source.LerpTo(weightedIndexArrays, target, offset, length);
        }

        private static Dictionary<Type, Action<Array, IEnumerable<WeightedIndex[]>,
                                             Array, int, int>> s_lerpAndNormalizeToOffsetMap =
            new Dictionary<Type, Action<Array, IEnumerable<WeightedIndex[]>, Array, int, int>>
            {
                { typeof(V3f[]), (s, w, t, o, l) =>
                    CombineTo((V3f[])s, w, (wi,sa) => Lerp(wi,sa).Normalized, (V3f[])t, o, l) },
                { typeof(V3d[]), (s, w, t, o, l) =>
                    CombineTo((V3d[])s, w, (wi,sa) => Lerp(wi,sa).Normalized, (V3d[])t, o, l) },
            };

        /// <summary>
        /// Fills the target array starting at offset for length entries
        /// with normalized Lerp/weighted combinations of the source array.
        /// </summary>
        public static Array LerpAndNormalizeTo(
                this Array source,
                IEnumerable<WeightedIndex[]> weightedIndexArrays,
                Array target, int offset, int length)
        {
            var type = source.GetType();
            s_lerpAndNormalizeToOffsetMap[type](
                    source, weightedIndexArrays, target, offset, length);
            return target;
        }

        /// <summary>
        /// Fills the target array starting at offset with normalized
        /// Lerp/weighted combinations of the source array.
        /// </summary>
        public static Array LerpAndNormalizeTo(
                this Array source,
                IEnumerable<WeightedIndex[]> weightedIndexArrays,
                Array target, int offset)
        {
            int length = Fun.Min(weightedIndexArrays.Count(), target.Length - offset);
            return source.LerpAndNormalizeTo(weightedIndexArrays, target, offset, length);
        }

        public static V3d[] Transformed(this V3d[] points, M33d matrix)
        {
            return points.Map(p => matrix.Transform(p));
        }

        public static V3d[] TransformedPos(this V3d[] points, M44d matrix)
        {
            return points.Map(p => matrix.TransformPos(p));
        }
    }

    public static class V2dArrayExtensions
    {
        #region Transformations

        /// <summary>
        /// Gets centroid of this polygon's vertices.
        /// </summary>
        [Obsolete("Use VectorIEnumerableExtensions.ComputeCentroid instead.")]
        public static V2d ComputeCentroid(this V2d[] points)
        {
            var sum = points[0];
            long count = points.LongLength;
            for (long i = 1; i < count; i++) sum += points[i];
            double d = 1.0 / count;
            return sum * d;
        }

        /// <summary>
        /// Returns a version of the point array scaled by a factor of s about
        /// the supplied center.
        /// </summary>
        public static V2d[] Scaled(this V2d[] pointArray, V2d center, double s)
        {
            long count = pointArray.LongLength;
            var pa = new V2d[count];
            for (long i = 0; i < count; i++) pa[i] = center + (pointArray[i] - center) * s;
            return pa;
        }

        /// <summary>
        /// Returns a version of the point array scaled by a factor of s about
        /// the supplied center.
        /// </summary>
        public static V2d[] ScaledAboutCentroid(this V2d[] pointArray, double s)
        {
            return pointArray.Scaled(VectorIEnumerableExtensions.ComputeCentroid(pointArray), s);
        }

        /// <summary>
        /// Returns new array containing transformed points.
        /// </summary>
        public static V2d[] Transformed(this V2d[] pointArray, M22d m)
        {
            return new V2d[pointArray.LongLength].SetByIndexLong(i => m.Transform(pointArray[i]));
        }

        /// <summary>
        /// Returns new array containing transformed points.
        /// </summary>
        public static V2d[] TransformedPos(this V2d[] pointArray, M33d m)
        {
            return new V2d[pointArray.LongLength].SetByIndexLong(i => m.TransformPos(pointArray[i]));
        }

        /// <summary>
        /// Returns new array containing transformed points.
        /// </summary>
        public static V2d[] TransformedDir(this V2d[] pointArray, M33d m)
        {
            return new V2d[pointArray.LongLength].SetByIndexLong(i => m.TransformDir(pointArray[i]));
        }

        #endregion
    }

    #endregion
}
