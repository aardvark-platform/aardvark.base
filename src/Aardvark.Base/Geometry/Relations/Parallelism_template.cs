using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// Provides various methods determining parallelism
    /// </summary>
    public static class Parallelism
    {
        //# foreach (var isDouble in new[] { false, true }) {
        //#   var rtype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var v2t = "V2" + tc;
        //#   var v3t = "V3" + tc;
        //#   var plane3t = "Plane3" + tc;
        //#   var ray2t = "Ray2" + tc;
        //#   var ray3t = "Ray3" + tc;
        //#   var line2t = "Line2" + tc;
        //#   var line3t = "Line3" + tc;
        //#   var eps = isDouble ? "1e-6" : "1e-4f";
        // 2-Dimensional

        #region __v2t__ - __v2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __v2t__ u, __v2t__ v)
            => Fun.IsTiny(u.X * v.Y - u.Y * v.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __v2t__ u, __v2t__ v, __rtype__ epsilon = __eps__)
        {
            var un = u.Normalized;
            var vn = v.Normalized;
            return (un - vn).Length < epsilon || (un + vn).Length < epsilon;
        }

        #endregion

        #region __ray2t__ - __v2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray2t__ ray, __v2t__ v)
            => ray.Direction.IsParallelTo(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray2t__ ray, __v2t__ v, __rtype__ epsilon = __eps__)
            => ray.Direction.IsParallelTo(v, epsilon);

        #endregion

        #region __ray2t__ - __ray2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray2t__ r0, __ray2t__ r1)
            => r0.Direction.IsParallelTo(r1.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray2t__ r0, __ray2t__ r1, __rtype__ epsilon = __eps__)
            => r0.Direction.IsParallelTo(r1.Direction, epsilon);

        #endregion

        #region __line2t__ - __line2t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __line2t__ l0, __line2t__ l1)
            => l0.Direction.IsParallelTo(l1.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __line2t__ l0, __line2t__ l1, __rtype__ epsilon = __eps__)
            => l0.Direction.IsParallelTo(l1.Direction, epsilon);

        #endregion

        // 3-Dimensional

        #region __v3t__ - __v3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __v3t__ u, __v3t__ v)
            => Fun.IsTiny(u.Cross(v).Norm1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __v3t__ u, __v3t__ v, __rtype__ epsilon = __eps__)
        {
            var un = u.Normalized;
            var vn = v.Normalized;

            return (un - vn).Length < epsilon || (un + vn).Length < epsilon;
        }

        #endregion

        #region __ray3t__ - __v3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray3t__ ray, __v3t__ vec)
            => ray.Direction.IsParallelTo(vec);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray3t__ ray, __v3t__ vec, __rtype__ epsilon = __eps__)
            => ray.Direction.IsParallelTo(vec, epsilon);

        #endregion

        #region __ray3t__ - __ray3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray3t__ r0, __ray3t__ r1)
            => r0.Direction.IsParallelTo(r1.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray3t__ r0, __ray3t__ r1, __rtype__ epsilon = __eps__)
            => r0.Direction.IsParallelTo(r1.Direction, epsilon);

        #endregion

        #region __plane3t__ - __plane3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __plane3t__ p0, __plane3t__ p1)
            => p0.Normal.IsParallelTo(p1.Normal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __plane3t__ p0, __plane3t__ p1, __rtype__ epsilon = __eps__)
            => p0.Normal.IsParallelTo(p1.Normal, epsilon);

        #endregion

        #region __ray3t__ - __plane3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __ray3t__ ray, __plane3t__ plane)
            => ray.Direction.IsOrthogonalTo(plane.Normal);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __plane3t__ plane, __ray3t__ ray)
            => ray.Direction.IsOrthogonalTo(plane.Normal);

        #endregion

        #region __line3t__ - __line3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __line3t__ l0, __line3t__ l1)
            => l0.Direction.IsParallelTo(l1.Direction);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsParallelTo(this __line3t__ l0, __line3t__ l1, __rtype__ epsilon = __eps__)
            => l0.Direction.IsParallelTo(l1.Direction, epsilon);

        #endregion

        //# }
    }
}
