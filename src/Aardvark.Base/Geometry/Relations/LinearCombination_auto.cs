using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    public static class LinearCombination
    {
        // AUTO GENERATED CODE - DO NOT CHANGE!


        #region V3f - V3f

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3f x, V3f u, V3f v)
        {
            V3f n = u.Cross(v);
            return n.IsOrthogonalTo(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3f x, V3f u)
            => x.IsParallelTo(u);

        public static bool IsLinearCombinationOf(this V3f x, V3f u, V3f v, out float t0, out float t1)
        {
            //x == t2*u + t1*v
            V3f n = u.Cross(v);

            float[,] mat = new float[3, 3]
            {
                {u.X,v.X,n.X},
                {u.Y,v.Y,n.Y},
                {u.Z,v.Z,n.Z}
            };

            float[] result = new float[3] { x.X, x.Y, x.Z };

            int[] perm = mat.LuFactorize();
            V3f t = new V3f(mat.LuSolve(perm, result));

            if (Fun.IsTiny(t.Z))
            {
                t0 = t.X;
                t1 = t.Y;

                return true;
            }
            else
            {
                t0 = float.NaN;
                t1 = float.NaN;

                return false;
            }

            //x == 
        }

        #endregion


        #region V3d - V3d

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3d x, V3d u, V3d v)
        {
            V3d n = u.Cross(v);
            return n.IsOrthogonalTo(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3d x, V3d u)
            => x.IsParallelTo(u);

        public static bool IsLinearCombinationOf(this V3d x, V3d u, V3d v, out double t0, out double t1)
        {
            //x == t2*u + t1*v
            V3d n = u.Cross(v);

            double[,] mat = new double[3, 3]
            {
                {u.X,v.X,n.X},
                {u.Y,v.Y,n.Y},
                {u.Z,v.Z,n.Z}
            };

            double[] result = new double[3] { x.X, x.Y, x.Z };

            int[] perm = mat.LuFactorize();
            V3d t = new V3d(mat.LuSolve(perm, result));

            if (Fun.IsTiny(t.Z))
            {
                t0 = t.X;
                t1 = t.Y;

                return true;
            }
            else
            {
                t0 = double.NaN;
                t1 = double.NaN;

                return false;
            }

            //x == 
        }

        #endregion

    }
}
