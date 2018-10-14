using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class VectorIEnumerableExtensions
    {
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


        #endregion
    }
}
