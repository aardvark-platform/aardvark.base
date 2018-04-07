using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class MatrixArrayExtensions
    {
        #region Transform position arrays and collections

        public static void TransformPosArray(this M44d mat, V3d[] points)
        {
            for (var i = 0; i < points.Length; i++)
                points[i] = mat.TransformPos(points[i]);
        }

        public static V3d[] TransformedPosArray(this M44d mat, ICollection<V3d> points)
        {
            var result = new V3d[points.Count];
            var i = 0;
            foreach (var p in points)
                result[i++] = mat.TransformPos(p);
            return result;
        }

        public static V3d[] TransformedDirArray(this M44d mat, V3d[] directions)
        {
            var result = new V3d[directions.Length];
            for (var i = 0; i < directions.Length; i++)
                result[i] = mat.TransformDir(directions[i]);
            return result;
        }

        #endregion

        #region Indexed Copy Operations

        /// <summary>
        /// Copies from the position array indexed by a backward map into
        /// a target array, starting at the supplied offset, thereby
        /// transforming all positions using the supplied matrix.
        /// </summary>
        /// <returns>target array</returns>
        public static V3f[] BackwardIndexedTransformPosAndCopyTo(
            this V3f[] source,
            V3f[] target,
            int[] backwardMap,
            int offset,
            M44d m44d)
        {
            var count = backwardMap.Length;
            for (var i = 0; i < count; i++)
                target[i + offset] = (V3f)m44d.TransformPos((V3d)source[backwardMap[i]]);
            return target;
        }

        /// <summary>
        /// Copies from the position array indexed by a backward map into
        /// a target array, starting at the supplied offset, thereby
        /// transforming all positions using the supplied matrix.
        /// </summary>
        /// <returns>target array</returns>
        public static V3d[] BackwardIndexedTransformPosAndCopyTo(
            this V3d[] source,
            V3d[] target,
            int[] backwardMap,
            int offset,
            M44d m44d)
        {
            var count = backwardMap.Length;
            for (var i = 0; i < count; i++)
                target[i + offset] = m44d.TransformPos(source[backwardMap[i]]);

            return target;
        }

        public static Array BackwardIndexedTransformPosAndCopyTo(
            this Array source, Array target,
            int[] backwardMap, int offset,
            M44d m44d)
        {
            var type = source.GetType();

            if (type == typeof(V3f[]))
                return BackwardIndexedTransformPosAndCopyTo((V3f[])source, (V3f[])target,
                            backwardMap, offset, m44d);
            if (type == typeof(V3d[]))
                return BackwardIndexedTransformPosAndCopyTo((V3d[])source, (V3d[])target,
                            backwardMap, offset, m44d);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Copies from the direction array indexed by a backward map into
        /// a target array, starting at the supplied offset, thereby
        /// transforming all directions using the supplied matrix.
        /// </summary>
        /// <returns>target array</returns>
        public static V3f[] BackwardIndexedTransformDirAndCopyTo(
            this V3f[] source,
            V3f[] target,
            int[] backwardMap,
            int offset,
            M44d m44d)
        {
            var count = backwardMap.Length;
            for (var i = 0; i < count; i++)
                target[i + offset] = (V3f)m44d.TransformDir((V3d)source[backwardMap[i]]);
            return target;
        }

        /// <summary>
        /// Copies from the direction array indexed by a backward map into
        /// a target array, starting at the supplied offset, thereby
        /// transforming all directions using the supplied matrix.
        /// </summary>
        /// <returns>target array</returns>
        public static V3d[] BackwardIndexedTransformDirAndCopyTo(
            this V3d[] source,
            V3d[] target,
            int[] backwardMap,
            int offset,
            M44d m44d)
        {
            var count = backwardMap.Length;
            for (var i = 0; i < count; i++)
                target[i + offset] = m44d.TransformDir(source[backwardMap[i]]);

            return target;
        }

        public static Array BackwardIndexedTransformDirAndCopyTo(
            this Array source, Array target,
            int[] backwardMap, int offset,
            M44d m44d)
        {
            var type = source.GetType();

            if (type == typeof(V3f[]))
                return BackwardIndexedTransformDirAndCopyTo((V3f[])source, (V3f[])target,
                            backwardMap, offset, m44d);
            if (type == typeof(V3d[]))
                return BackwardIndexedTransformDirAndCopyTo((V3d[])source, (V3d[])target,
                            backwardMap, offset, m44d);

            throw new InvalidOperationException();
        }

        #endregion
    }
}
