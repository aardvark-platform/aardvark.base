using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    public static class LinearCombination
    {
        // AUTO GENERATED CODE - DO NOT CHANGE!

        //# foreach (var isDouble in new[] { false, true }) {
        //#   var ftype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var v2t = "V2" + tc;
        //#   var v3t = "V3" + tc;
        //#   var half = isDouble ? "0.5" : "0.5f";

        #region __v3t__ - __v3t__

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this __v3t__ x, __v3t__ u, __v3t__ v)
        {
            __v3t__ n = u.Cross(v);
            return n.IsOrthogonalTo(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this __v3t__ x, __v3t__ u)
            => x.IsParallelTo(u);

        public static bool IsLinearCombinationOf(this __v3t__ x, __v3t__ u, __v3t__ v, out __ftype__ t0, out __ftype__ t1)
        {
            //x == t2*u + t1*v
            __v3t__ n = u.Cross(v);

            __ftype__[,] mat = new __ftype__[3, 3]
            {
                {u.X,v.X,n.X},
                {u.Y,v.Y,n.Y},
                {u.Z,v.Z,n.Z}
            };

            __ftype__[] result = new __ftype__[3] { x.X, x.Y, x.Z };

            int[] perm = mat.LuFactorize();
            __v3t__ t = new __v3t__(mat.LuSolve(perm, result));

            if (Fun.IsTiny(t.Z))
            {
                t0 = t.X;
                t1 = t.Y;

                return true;
            }
            else
            {
                t0 = __ftype__.NaN;
                t1 = __ftype__.NaN;

                return false;
            }

            //x == 
        }

        #endregion

        //# }
    }
}
