using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class VectorIEnumerableExtensions
    {
        #region V2i

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V2is.
        /// </summary>
        public static V2i Sum(this IEnumerable<V2i> vectors)
        {
            V2i sum = V2i.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V2is.
        /// </summary>
        public static V2i Sum(this V2i[] vectors)
        {
            V2i sum = V2i.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V2is.
        /// </summary>
        public static V2d ComputeCentroid(this IEnumerable<V2i> vectors)
        { 
            V2d sum = V2d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V2d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2is.
        /// </summary>
        public static V2d ComputeCentroid(this V2i[] vectors)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V2d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2is.
        /// </summary>
        public static V2d ComputeCentroid(this V2i[] vectors, int[] indices)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V2d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V2is.
        /// </summary>
        public static V2d ComputeCentroid(this V2i[] vectors, double[] weights)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V2d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V2d ComputeCentroid(this V2i[] vectors, double[] weights, int[] indices)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V2d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V3i

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V3is.
        /// </summary>
        public static V3i Sum(this IEnumerable<V3i> vectors)
        {
            V3i sum = V3i.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V3is.
        /// </summary>
        public static V3i Sum(this V3i[] vectors)
        {
            V3i sum = V3i.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V3is.
        /// </summary>
        public static V3d ComputeCentroid(this IEnumerable<V3i> vectors)
        { 
            V3d sum = V3d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V3d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3is.
        /// </summary>
        public static V3d ComputeCentroid(this V3i[] vectors)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V3d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3is.
        /// </summary>
        public static V3d ComputeCentroid(this V3i[] vectors, int[] indices)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V3d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V3is.
        /// </summary>
        public static V3d ComputeCentroid(this V3i[] vectors, double[] weights)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V3d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V3d ComputeCentroid(this V3i[] vectors, double[] weights, int[] indices)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V3d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V4i

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V4is.
        /// </summary>
        public static V4i Sum(this IEnumerable<V4i> vectors)
        {
            V4i sum = V4i.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V4is.
        /// </summary>
        public static V4i Sum(this V4i[] vectors)
        {
            V4i sum = V4i.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V4is.
        /// </summary>
        public static V4d ComputeCentroid(this IEnumerable<V4i> vectors)
        { 
            V4d sum = V4d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V4d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4is.
        /// </summary>
        public static V4d ComputeCentroid(this V4i[] vectors)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V4d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4is.
        /// </summary>
        public static V4d ComputeCentroid(this V4i[] vectors, int[] indices)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V4d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V4is.
        /// </summary>
        public static V4d ComputeCentroid(this V4i[] vectors, double[] weights)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V4d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V4d ComputeCentroid(this V4i[] vectors, double[] weights, int[] indices)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V4d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V2ui

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V2uis.
        /// </summary>
        public static V2ui Sum(this IEnumerable<V2ui> vectors)
        {
            V2ui sum = V2ui.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V2uis.
        /// </summary>
        public static V2ui Sum(this V2ui[] vectors)
        {
            V2ui sum = V2ui.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V2uis.
        /// </summary>
        public static V2d ComputeCentroid(this IEnumerable<V2ui> vectors)
        { 
            V2d sum = V2d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V2d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2uis.
        /// </summary>
        public static V2d ComputeCentroid(this V2ui[] vectors)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V2d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2uis.
        /// </summary>
        public static V2d ComputeCentroid(this V2ui[] vectors, int[] indices)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V2d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V2uis.
        /// </summary>
        public static V2d ComputeCentroid(this V2ui[] vectors, double[] weights)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V2d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V2d ComputeCentroid(this V2ui[] vectors, double[] weights, int[] indices)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V2d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V3ui

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V3uis.
        /// </summary>
        public static V3ui Sum(this IEnumerable<V3ui> vectors)
        {
            V3ui sum = V3ui.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V3uis.
        /// </summary>
        public static V3ui Sum(this V3ui[] vectors)
        {
            V3ui sum = V3ui.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V3uis.
        /// </summary>
        public static V3d ComputeCentroid(this IEnumerable<V3ui> vectors)
        { 
            V3d sum = V3d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V3d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3uis.
        /// </summary>
        public static V3d ComputeCentroid(this V3ui[] vectors)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V3d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3uis.
        /// </summary>
        public static V3d ComputeCentroid(this V3ui[] vectors, int[] indices)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V3d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V3uis.
        /// </summary>
        public static V3d ComputeCentroid(this V3ui[] vectors, double[] weights)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V3d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V3d ComputeCentroid(this V3ui[] vectors, double[] weights, int[] indices)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V3d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V4ui

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V4uis.
        /// </summary>
        public static V4ui Sum(this IEnumerable<V4ui> vectors)
        {
            V4ui sum = V4ui.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V4uis.
        /// </summary>
        public static V4ui Sum(this V4ui[] vectors)
        {
            V4ui sum = V4ui.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V4uis.
        /// </summary>
        public static V4d ComputeCentroid(this IEnumerable<V4ui> vectors)
        { 
            V4d sum = V4d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V4d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4uis.
        /// </summary>
        public static V4d ComputeCentroid(this V4ui[] vectors)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V4d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4uis.
        /// </summary>
        public static V4d ComputeCentroid(this V4ui[] vectors, int[] indices)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V4d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V4uis.
        /// </summary>
        public static V4d ComputeCentroid(this V4ui[] vectors, double[] weights)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V4d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V4d ComputeCentroid(this V4ui[] vectors, double[] weights, int[] indices)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V4d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V2l

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V2ls.
        /// </summary>
        public static V2l Sum(this IEnumerable<V2l> vectors)
        {
            V2l sum = V2l.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V2ls.
        /// </summary>
        public static V2l Sum(this V2l[] vectors)
        {
            V2l sum = V2l.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V2ls.
        /// </summary>
        public static V2d ComputeCentroid(this IEnumerable<V2l> vectors)
        { 
            V2d sum = V2d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V2d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2ls.
        /// </summary>
        public static V2d ComputeCentroid(this V2l[] vectors)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V2d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2ls.
        /// </summary>
        public static V2d ComputeCentroid(this V2l[] vectors, int[] indices)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V2d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V2ls.
        /// </summary>
        public static V2d ComputeCentroid(this V2l[] vectors, double[] weights)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V2d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V2d ComputeCentroid(this V2l[] vectors, double[] weights, int[] indices)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V2d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V3l

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V3ls.
        /// </summary>
        public static V3l Sum(this IEnumerable<V3l> vectors)
        {
            V3l sum = V3l.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V3ls.
        /// </summary>
        public static V3l Sum(this V3l[] vectors)
        {
            V3l sum = V3l.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V3ls.
        /// </summary>
        public static V3d ComputeCentroid(this IEnumerable<V3l> vectors)
        { 
            V3d sum = V3d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V3d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3ls.
        /// </summary>
        public static V3d ComputeCentroid(this V3l[] vectors)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V3d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3ls.
        /// </summary>
        public static V3d ComputeCentroid(this V3l[] vectors, int[] indices)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V3d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V3ls.
        /// </summary>
        public static V3d ComputeCentroid(this V3l[] vectors, double[] weights)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V3d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V3d ComputeCentroid(this V3l[] vectors, double[] weights, int[] indices)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V3d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V4l

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V4ls.
        /// </summary>
        public static V4l Sum(this IEnumerable<V4l> vectors)
        {
            V4l sum = V4l.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V4ls.
        /// </summary>
        public static V4l Sum(this V4l[] vectors)
        {
            V4l sum = V4l.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V4ls.
        /// </summary>
        public static V4d ComputeCentroid(this IEnumerable<V4l> vectors)
        { 
            V4d sum = V4d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += (V4d)e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4ls.
        /// </summary>
        public static V4d ComputeCentroid(this V4l[] vectors)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += (V4d)vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4ls.
        /// </summary>
        public static V4d ComputeCentroid(this V4l[] vectors, int[] indices)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += (V4d)vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V4ls.
        /// </summary>
        public static V4d ComputeCentroid(this V4l[] vectors, double[] weights)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * (V4d)vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V4d ComputeCentroid(this V4l[] vectors, double[] weights, int[] indices)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * (V4d)vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V2f

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V2fs.
        /// </summary>
        public static V2f Sum(this IEnumerable<V2f> vectors)
        {
            V2f sum = V2f.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V2fs.
        /// </summary>
        public static V2f Sum(this V2f[] vectors)
        {
            V2f sum = V2f.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V2fs.
        /// </summary>
        public static V2f ComputeCentroid(this IEnumerable<V2f> vectors)
        { 
            V2f sum = V2f.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += e;
                count++;
            }

            return sum / (float)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2fs.
        /// </summary>
        public static V2f ComputeCentroid(this V2f[] vectors)
        {
            V2f sum = V2f.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum / (float)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2fs.
        /// </summary>
        public static V2f ComputeCentroid(this V2f[] vectors, int[] indices)
        {
            V2f sum = V2f.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += vectors[indices[i]];
            }

            return sum / (float)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V2fs.
        /// </summary>
        public static V2f ComputeCentroid(this V2f[] vectors, float[] weights)
        {
            V2f sum = V2f.Zero;
            float weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V2f ComputeCentroid(this V2f[] vectors, float[] weights, int[] indices)
        {
            V2f sum = V2f.Zero;
            float weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V3f

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V3fs.
        /// </summary>
        public static V3f Sum(this IEnumerable<V3f> vectors)
        {
            V3f sum = V3f.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V3fs.
        /// </summary>
        public static V3f Sum(this V3f[] vectors)
        {
            V3f sum = V3f.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V3fs.
        /// </summary>
        public static V3f ComputeCentroid(this IEnumerable<V3f> vectors)
        { 
            V3f sum = V3f.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += e;
                count++;
            }

            return sum / (float)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3fs.
        /// </summary>
        public static V3f ComputeCentroid(this V3f[] vectors)
        {
            V3f sum = V3f.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum / (float)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3fs.
        /// </summary>
        public static V3f ComputeCentroid(this V3f[] vectors, int[] indices)
        {
            V3f sum = V3f.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += vectors[indices[i]];
            }

            return sum / (float)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V3fs.
        /// </summary>
        public static V3f ComputeCentroid(this V3f[] vectors, float[] weights)
        {
            V3f sum = V3f.Zero;
            float weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V3f ComputeCentroid(this V3f[] vectors, float[] weights, int[] indices)
        {
            V3f sum = V3f.Zero;
            float weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V4f

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V4fs.
        /// </summary>
        public static V4f Sum(this IEnumerable<V4f> vectors)
        {
            V4f sum = V4f.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V4fs.
        /// </summary>
        public static V4f Sum(this V4f[] vectors)
        {
            V4f sum = V4f.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V4fs.
        /// </summary>
        public static V4f ComputeCentroid(this IEnumerable<V4f> vectors)
        { 
            V4f sum = V4f.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += e;
                count++;
            }

            return sum / (float)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4fs.
        /// </summary>
        public static V4f ComputeCentroid(this V4f[] vectors)
        {
            V4f sum = V4f.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum / (float)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4fs.
        /// </summary>
        public static V4f ComputeCentroid(this V4f[] vectors, int[] indices)
        {
            V4f sum = V4f.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += vectors[indices[i]];
            }

            return sum / (float)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V4fs.
        /// </summary>
        public static V4f ComputeCentroid(this V4f[] vectors, float[] weights)
        {
            V4f sum = V4f.Zero;
            float weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V4f ComputeCentroid(this V4f[] vectors, float[] weights, int[] indices)
        {
            V4f sum = V4f.Zero;
            float weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V2d

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V2ds.
        /// </summary>
        public static V2d Sum(this IEnumerable<V2d> vectors)
        {
            V2d sum = V2d.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V2ds.
        /// </summary>
        public static V2d Sum(this V2d[] vectors)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V2ds.
        /// </summary>
        public static V2d ComputeCentroid(this IEnumerable<V2d> vectors)
        { 
            V2d sum = V2d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2ds.
        /// </summary>
        public static V2d ComputeCentroid(this V2d[] vectors)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V2ds.
        /// </summary>
        public static V2d ComputeCentroid(this V2d[] vectors, int[] indices)
        {
            V2d sum = V2d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V2ds.
        /// </summary>
        public static V2d ComputeCentroid(this V2d[] vectors, double[] weights)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V2d ComputeCentroid(this V2d[] vectors, double[] weights, int[] indices)
        {
            V2d sum = V2d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V3d

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V3ds.
        /// </summary>
        public static V3d Sum(this IEnumerable<V3d> vectors)
        {
            V3d sum = V3d.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V3ds.
        /// </summary>
        public static V3d Sum(this V3d[] vectors)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V3ds.
        /// </summary>
        public static V3d ComputeCentroid(this IEnumerable<V3d> vectors)
        { 
            V3d sum = V3d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3ds.
        /// </summary>
        public static V3d ComputeCentroid(this V3d[] vectors)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V3ds.
        /// </summary>
        public static V3d ComputeCentroid(this V3d[] vectors, int[] indices)
        {
            V3d sum = V3d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V3ds.
        /// </summary>
        public static V3d ComputeCentroid(this V3d[] vectors, double[] weights)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V3d ComputeCentroid(this V3d[] vectors, double[] weights, int[] indices)
        {
            V3d sum = V3d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

        #region V4d

        #region Sum
        /// <summary>
        /// Calculates the sum for a given set of V4ds.
        /// </summary>
        public static V4d Sum(this IEnumerable<V4d> vectors)
        {
            V4d sum = V4d.Zero;

            foreach (var e in vectors)
            {
                sum += e;
            }

            return sum;
        }

        /// <summary>
        /// Calculates the sum for a given set of V4ds.
        /// </summary>
        public static V4d Sum(this V4d[] vectors)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum;
        }

        #endregion

        #region Centroid
        /// <summary>
        /// Calculates the centroid for a given set of V4ds.
        /// </summary>
        public static V4d ComputeCentroid(this IEnumerable<V4d> vectors)
        { 
            V4d sum = V4d.Zero;
            int count = 0;

            foreach (var e in vectors)
            {
                sum += e;
                count++;
            }

            return sum / (double)count;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4ds.
        /// </summary>
        public static V4d ComputeCentroid(this V4d[] vectors)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }

            return sum / (double)vectors.Length;
        }

        /// <summary>
        /// Calculates the centroid for a given set of V4ds.
        /// </summary>
        public static V4d ComputeCentroid(this V4d[] vectors, int[] indices)
        {
            V4d sum = V4d.Zero;

            for (var i = 0; i < indices.Length; i++)
            {
                sum += vectors[indices[i]];
            }

            return sum / (double)indices.Length;
        }

        /// <summary>
        /// Calculates a weighted centroid for a given array of V4ds.
        /// </summary>
        public static V4d ComputeCentroid(this V4d[] vectors, double[] weights)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for(int i = 0; i < vectors.Length; i++)
            {
                sum += weights[i] * vectors[i];
                weightSum += weights[i];
            }

            return sum / weightSum;
        }

        /// <summary>
        /// Calculates a weighted centroid for vectors and weights given by indices.
        /// Sum(vectors[indices[i]] * weights[indices[i]]) / Sum(weights[indices[i]].
        /// </summary>
        public static V4d ComputeCentroid(this V4d[] vectors, double[] weights, int[] indices)
        {
            V4d sum = V4d.Zero;
            double weightSum = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                var w = weights[indices[i]];
                sum += w * vectors[indices[i]];
                weightSum += w;
            }

            return sum / weightSum;
        }

        #endregion

        #endregion

    }
}
