using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    public static class LinearCombination
    {
        // AUTO GENERATED CODE - DO NOT CHANGE!


        #region V3f - V3f

        /// <summary>
        /// Tests membership in the span of u and v within the default tolerance.
        /// Zero bases span only zero; dependent nonzero bases span their common line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3f x, V3f u, V3f v)
        {
            var n = u.Cross(v);
            if (!Fun.IsTiny(n.Dot(x))) return false;
            return n != V3f.Zero || x.IsLinearCombinationOf(u.NormMax >= v.NormMax ? u : v);
        }

        /// <summary>
        /// Tests parallelism to a nonzero basis within the default tolerance.
        /// A zero basis spans only zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3f x, V3f u)
            => u != V3f.Zero
                ? x.Cross(u).Norm1 < Constant<float>.PositiveTinyValue
                : x == V3f.Zero;

        /// <summary>
        /// Finds finite coefficients reconstructing x = t0*u + t1*v within tolerance.
        /// Dependent bases use the basis with the largest absolute component (first on ties),
        /// with the unused coefficient zero. Zero bases succeed only for zero, with both coefficients zero.
        /// Misses return false and set both coefficients to NaN.
        /// For independent bases, the coefficient of u.Cross(v) must be tiny.
        /// </summary>
        public static bool IsLinearCombinationOf(this V3f x, V3f u, V3f v, out float t0, out float t1)
        {
            var n = u.Cross(v);
            if (n == V3f.Zero) return TryDependentCombination(x, u, v, out t0, out t1);
            t0 = t1 = float.NaN;

            // Scalar elimination of [u v n], with partial pivoting and the original normal-residual test.
            var r0 = new V3f(u.X, v.X, n.X);
            var r1 = new V3f(u.Y, v.Y, n.Y);
            var r2 = new V3f(u.Z, v.Z, n.Z);
            if (Fun.Abs(r1.X) > Fun.Abs(r0.X)) { Fun.Swap(ref r0, ref r1); Fun.Swap(ref x.X, ref x.Y); }
            if (Fun.Abs(r2.X) > Fun.Abs(r0.X)) { Fun.Swap(ref r0, ref r2); Fun.Swap(ref x.X, ref x.Z); }

            var f1 = r1.X / r0.X;
            var f2 = r2.X / r0.X;
            r1.Y -= r0.Y * f1; r1.Z -= r0.Z * f1; x.Y -= x.X * f1;
            r2.Y -= r0.Y * f2; r2.Z -= r0.Z * f2; x.Z -= x.X * f2;
            if (Fun.Abs(r2.Y) > Fun.Abs(r1.Y)) { Fun.Swap(ref r1, ref r2); Fun.Swap(ref x.Y, ref x.Z); }

            var f = r2.Y / r1.Y;
            var normal = (x.Z - x.Y * f) / (r2.Z - r1.Z * f);
            if (!Fun.IsTiny(normal)) return false;

            var b = (x.Y - r1.Z * normal) / r1.Y;
            var a = (x.X - r0.Y * b - r0.Z * normal) / r0.X;
            if (!Fun.IsFinite(a) || !Fun.IsFinite(b)) return false;
            t0 = a;
            t1 = b;
            return true;
        }

        private static bool TryDependentCombination(V3f x, V3f u, V3f v, out float t0, out float t1)
        {
            t0 = t1 = float.NaN;
            bool useU = u.NormMax >= v.NormMax;
            var w = useU ? u : v;
            if (!x.IsLinearCombinationOf(w)) return false;
            float t = 0;
            if (w != V3f.Zero)
            {
                // Divide by the largest component, avoiding squared lengths and zero divisors.
                int i = 0;
                if (Fun.Abs(w.Y) > Fun.Abs(w.X)) i = 1;
                if (Fun.Abs(w.Z) > Fun.Abs(w[i])) i = 2;
                t = x[i] / w[i];
                if (!Fun.IsFinite(t)) return false;
            }
            t0 = useU ? t : 0;
            t1 = useU ? 0 : t;
            return true;
        }

        #endregion


        #region V3d - V3d

        /// <summary>
        /// Tests membership in the span of u and v within the default tolerance.
        /// Zero bases span only zero; dependent nonzero bases span their common line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3d x, V3d u, V3d v)
        {
            var n = u.Cross(v);
            if (!Fun.IsTiny(n.Dot(x))) return false;
            return n != V3d.Zero || x.IsLinearCombinationOf(u.NormMax >= v.NormMax ? u : v);
        }

        /// <summary>
        /// Tests parallelism to a nonzero basis within the default tolerance.
        /// A zero basis spans only zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLinearCombinationOf(this V3d x, V3d u)
            => u != V3d.Zero
                ? x.Cross(u).Norm1 < Constant<double>.PositiveTinyValue
                : x == V3d.Zero;

        /// <summary>
        /// Finds finite coefficients reconstructing x = t0*u + t1*v within tolerance.
        /// Dependent bases use the basis with the largest absolute component (first on ties),
        /// with the unused coefficient zero. Zero bases succeed only for zero, with both coefficients zero.
        /// Misses return false and set both coefficients to NaN.
        /// For independent bases, the coefficient of u.Cross(v) must be tiny.
        /// </summary>
        public static bool IsLinearCombinationOf(this V3d x, V3d u, V3d v, out double t0, out double t1)
        {
            var n = u.Cross(v);
            if (n == V3d.Zero) return TryDependentCombination(x, u, v, out t0, out t1);
            t0 = t1 = double.NaN;

            // Scalar elimination of [u v n], with partial pivoting and the original normal-residual test.
            var r0 = new V3d(u.X, v.X, n.X);
            var r1 = new V3d(u.Y, v.Y, n.Y);
            var r2 = new V3d(u.Z, v.Z, n.Z);
            if (Fun.Abs(r1.X) > Fun.Abs(r0.X)) { Fun.Swap(ref r0, ref r1); Fun.Swap(ref x.X, ref x.Y); }
            if (Fun.Abs(r2.X) > Fun.Abs(r0.X)) { Fun.Swap(ref r0, ref r2); Fun.Swap(ref x.X, ref x.Z); }

            var f1 = r1.X / r0.X;
            var f2 = r2.X / r0.X;
            r1.Y -= r0.Y * f1; r1.Z -= r0.Z * f1; x.Y -= x.X * f1;
            r2.Y -= r0.Y * f2; r2.Z -= r0.Z * f2; x.Z -= x.X * f2;
            if (Fun.Abs(r2.Y) > Fun.Abs(r1.Y)) { Fun.Swap(ref r1, ref r2); Fun.Swap(ref x.Y, ref x.Z); }

            var f = r2.Y / r1.Y;
            var normal = (x.Z - x.Y * f) / (r2.Z - r1.Z * f);
            if (!Fun.IsTiny(normal)) return false;

            var b = (x.Y - r1.Z * normal) / r1.Y;
            var a = (x.X - r0.Y * b - r0.Z * normal) / r0.X;
            if (!Fun.IsFinite(a) || !Fun.IsFinite(b)) return false;
            t0 = a;
            t1 = b;
            return true;
        }

        private static bool TryDependentCombination(V3d x, V3d u, V3d v, out double t0, out double t1)
        {
            t0 = t1 = double.NaN;
            bool useU = u.NormMax >= v.NormMax;
            var w = useU ? u : v;
            if (!x.IsLinearCombinationOf(w)) return false;
            double t = 0;
            if (w != V3d.Zero)
            {
                // Divide by the largest component, avoiding squared lengths and zero divisors.
                int i = 0;
                if (Fun.Abs(w.Y) > Fun.Abs(w.X)) i = 1;
                if (Fun.Abs(w.Z) > Fun.Abs(w[i])) i = 2;
                t = x[i] / w[i];
                if (!Fun.IsFinite(t)) return false;
            }
            t0 = useU ? t : 0;
            t1 = useU ? 0 : t;
            return true;
        }

        #endregion

    }
}
