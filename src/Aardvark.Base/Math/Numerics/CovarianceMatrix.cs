using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// Covariance matrix.
    /// </summary>
    public static class CovarianceMatrixExtensions
    {
        #region V3d

        /// <summary>
        /// Computes outer product (tensor product) of vector v with itself.
        /// </summary>
        public static M33d OuterProduct(this in V3d v) => new M33d(
            v.X * v.X, v.X * v.Y, v.X * v.Z,
            v.Y * v.X, v.Y * v.Y, v.Y * v.Z,
            v.Z * v.X, v.Z * v.Y, v.Z * v.Z
            );

        /// <summary>
        /// Adds outer product (tensor product) of vector v with itself to matrix m.
        /// </summary>
        public static void AddOuterProduct(this ref M33d m, in V3d v)
        {
            m.M00 += v.X * v.X; m.M01 += v.X * v.Y; m.M02 += v.X * v.Z;
            m.M10 += v.Y * v.X; m.M11 += v.Y * v.Y; m.M12 += v.Y * v.Z;
            m.M20 += v.Z * v.X; m.M21 += v.Z * v.Y; m.M22 += v.Z * v.Z;
        }

        /// <summary>
        /// Computes covariance matrix for given points.
        /// </summary>
        public static M33d ComputeCovarianceMatrix(this V3d[] points)
        {
            var cvm = M33d.Zero;
            for (var i = 0; i < points.Length; i++) cvm.AddOuterProduct(points[i]);
            return cvm / points.Length;
        }

        /// <summary>
        /// Computes covariance matrix for points given by indices into points array.
        /// </summary>
        public static M33d ComputeCovarianceMatrix(this V3d[] points, int[] indices)
        {
            var cvm = M33d.Zero;
            for (var i = 0; i < indices.Length; i++) cvm.AddOuterProduct(points[indices[i]]);
            return cvm / indices.Length;
        }

        /// <summary>
        /// Computes covariance matrix from points relative to given center.
        /// </summary>
        public static M33d ComputeCovarianceMatrix(this V3d[] points, V3d center)
        {
            var cvm = M33d.Zero;
            for (var i = 0; i < points.Length; i++) cvm.AddOuterProduct(points[i] - center);
            return cvm / points.Length;
        }

        /// <summary>
        /// Computes covariance matrix for points given by indices into points array, relative to given center.
        /// </summary>
        public static M33d ComputeCovarianceMatrix(this V3d[] points, int[] indices, V3d center)
        {
            var cvm = M33d.Zero;
            for (var i = 0; i < indices.Length; i++) cvm.AddOuterProduct(points[indices[i]] - center);
            return cvm / indices.Length;
        }

        /// <summary>
        /// Computes covariance matrix for given points.
        /// </summary>
        public static M33d ComputeCovarianceMatrix(this IEnumerable<V3d> points)
        {
            var count = 0;
            var cvm = M33d.Zero;
            foreach (var p in points) { cvm.AddOuterProduct(p); count++; }
            return cvm / count;
        }

        /// <summary>
        /// Computes covariance matrix from points relative to given center.
        /// </summary>
        public static M33d ComputeCovarianceMatrix(this IEnumerable<V3d> points, V3d center)
        {
            var count = 0;
            var cvm = M33d.Zero;
            foreach (var p in points) { cvm.AddOuterProduct(p - center); count++; }
            return cvm / count;
        }

        #endregion

        #region V3f

        /// <summary>
        /// Computes outer product (tensor product) of vector v with itself.
        /// </summary>
        public static M33f OuterProduct(this in V3f v) => new M33f(
            v.X * v.X, v.X * v.Y, v.X * v.Z,
            v.Y * v.X, v.Y * v.Y, v.Y * v.Z,
            v.Z * v.X, v.Z * v.Y, v.Z * v.Z
            );

        /// <summary>
        /// Adds outer product (tensor product) of vector v with itself to matrix m.
        /// </summary>
        public static void AddOuterProduct(this ref M33f m, in V3f v)
        {
            m.M00 += v.X * v.X; m.M01 += v.X * v.Y; m.M02 += v.X * v.Z;
            m.M10 += v.Y * v.X; m.M11 += v.Y * v.Y; m.M12 += v.Y * v.Z;
            m.M20 += v.Z * v.X; m.M21 += v.Z * v.Y; m.M22 += v.Z * v.Z;
        }

        /// <summary>
        /// Computes covariance matrix for given points.
        /// </summary>
        public static M33f ComputeCovarianceMatrix(this V3f[] points)
        {
            var cvm = M33f.Zero;
            for (var i = 0; i < points.Length; i++) cvm.AddOuterProduct(points[i]);
            return cvm / points.Length;
        }
        
        /// <summary>
        /// Computes covariance matrix for points given by indices into points array.
        /// </summary>
        public static M33f ComputeCovarianceMatrix(this V3f[] points, int[] indices)
        {
            var cvm = M33f.Zero;
            for (var i = 0; i < indices.Length; i++) cvm.AddOuterProduct(points[indices[i]]);
            return cvm / indices.Length;
        }
        
        /// <summary>
        /// Computes covariance matrix from points relative to given center.
        /// </summary>
        public static M33f ComputeCovarianceMatrix(this V3f[] points, V3f center)
        {
            var cvm = M33f.Zero;
            for (var i = 0; i < points.Length; i++) cvm.AddOuterProduct(points[i] - center);
            return cvm / points.Length;
        }

        /// <summary>
        /// Computes covariance matrix for points given by indices into points array, relative to given center.
        /// </summary>
        public static M33f ComputeCovarianceMatrix(this V3f[] points, int[] indices, V3f center)
        {
            var cvm = M33f.Zero;
            for (var i = 0; i < indices.Length; i++) cvm.AddOuterProduct(points[indices[i]] - center);
            return cvm / indices.Length;
        }

        /// <summary>
        /// Computes covariance matrix for given points.
        /// </summary>
        public static M33f ComputeCovarianceMatrix(this IEnumerable<V3f> points)
        {
            var count = 0;
            var cvm = M33f.Zero;
            foreach (var p in points) { cvm.AddOuterProduct(p); count++; }
            return cvm / count;
        }

        /// <summary>
        /// Computes covariance matrix from points relative to given center.
        /// </summary>
        public static M33f ComputeCovarianceMatrix(this IEnumerable<V3f> points, V3f center)
        {
            var count = 0;
            var cvm = M33f.Zero;
            foreach (var p in points) { cvm.AddOuterProduct(p - center); count++; }
            return cvm / count;
        }

        #endregion
    }
}
