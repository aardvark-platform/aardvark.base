using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Aardvark.Base
{
    public enum Winding
    {
        CCW = 0,
        CW = 1,
    }

    /// <summary>
    /// This enum can be used to store an aggregate sign classification
    /// of multiple entities whose sign ccan be classified.
    /// </summary>
    [Flags]
    public enum Signs
    {
        None = 0x00,
        Zero = 0x01,
        Negative = 0x02,
        Positive = 0x04,

        NonPositive = Negative | Zero,
        NonNegative = Positive | Zero,
    }

    public static partial class Fun
    {
        public static T Identity<T>(T value) { return value; }

        #region Array Indices of Certain Elements

        /// <summary>
        /// For an array of monotonically increasing values, returns
        /// the index i of the largest value such that array[i] &lt;= x.
        /// If x is smaller than the first value, then -1 is returned.
        /// If x is greater than the last value, then a.Length is returned.
        /// </summary>
        public static int IndexOfLargestLessOrEqual<T>(this T[] a, T x)
            where T : IComparable
        {
            var r = new Range1i(0, a.Length - 1);
            if (x.CompareTo(a[0]) < 0) return -1;
            if (x.CompareTo(a[r.Max]) > 0) return a.Length;
            while (r.Size > 1)
            {
                var half = r.Center;
                if (x.CompareTo(a[half]) < 0) r.Max = half; else r.Min = half;
            }
            return r.Min;
        }

        /// <summary>
        /// For an array of monotonically increasing values, returns
        /// the index i of the largest value such that array[i] &lt;= x.
        /// If x is smaller than the first value, then -1 is returned.
        /// If x is greater than the last value, then a.Length is returned.
        /// </summary>
        public static long LongIndexOfLargestLessOrEqual<T>(this T[] a, T x)
            where T : IComparable
        {
            var r = new Range1l(0, a.Length - 1);
            if (x.CompareTo(a[0]) < 0) return -1;
            if (x.CompareTo(a[r.Max]) > 0) return a.Length;
            while (r.Size > 1)
            {
                var half = r.Center;
                if (x.CompareTo(a[half]) < 0) r.Max = half; else r.Min = half;
            }
            return r.Min;
        }

        /// <summary>
        /// For a list of monotonically increasing values, returns
        /// the index i of the largest value such that list[i] &lt;= x.
        /// If x is smaller than the first value, then -1 is returned.
        /// If x is greater than the last value, then a.Count is returned.
        /// </summary>
        public static int IndexOfLargestLessOrEqual<T>(this List<T> a, T x)
            where T : IComparable
        {
            var r = new Range1i(0, a.Count - 1);
            if (x.CompareTo(a[0]) < 0) return -1;
            if (x.CompareTo(a[r.Max]) > 0) return a.Count;
            while (r.Size > 1)
            {
                int half = r.Center;
                if (x.CompareTo(a[half]) < 0) r.Max = half; else r.Min = half;
            }
            return r.Min;
        }

        #endregion

        #region Bit Operations

        public static int HighestBit(this int i)
        {
            if (i == 0) return -1;

            int bit = 31;

            if ((i & 0xFFFFFF00) == 0) { i <<= 24; bit = 7; }
            else if ((i & 0xFFFF0000) == 0) { i <<= 16; bit = 15; }
            else if ((i & 0xFF000000) == 0) { i <<= 8; bit = 23; }

            if ((i & 0xF0000000) == 0) { i <<= 4; bit -= 4; }
            while ((i & 0x80000000) == 0) { i <<= 1; bit--; }
            return bit;
        }

        public static int TrailingZeroBitCount(this long x)
        {
            x &= -x;
            int c = (x == 0) ? 1 : 0;
            if ((x & 0x00000000ffffffffL) == 0) c += 32;
            if ((x & 0x0000ffff0000ffffL) == 0) c += 16;
            if ((x & 0x00ff00ff00ff00ffL) == 0) c += 8;
            if ((x & 0x0f0f0f0f0f0f0f0fL) == 0) c += 4;
            if ((x & 0x3333333333333333L) == 0) c += 2;
            if ((x & 0x5555555555555555L) == 0) c += 1;
            return c;
        }

        public static int TrailingZeroBitCount(this int x)
        {
            x &= -x;
            int c = (x == 0) ? 1 : 0;
            if ((x & 0x0000ffff) == 0) c += 16;
            if ((x & 0x00ff00ff) == 0) c += 8;
            if ((x & 0x0f0f0f0f) == 0) c += 4;
            if ((x & 0x33333333) == 0) c += 2;
            if ((x & 0x55555555) == 0) c += 1;
            return c;
        }

        #endregion

        #region Bitwise Rotation

        /// <summary>
        /// Bitwise circular left shift.
        /// </summary>
        public static int BitwiseRotateLeft(this int value, int numberOfBits)
        {
            return (value << numberOfBits) | (value >> (32 - numberOfBits));
        }

        /// <summary>
        /// Bitwise circular left shift.
        /// </summary>
        public static uint BitwiseRotateLeft(this uint value, int numberOfBits)
        {
            return (value << numberOfBits) | (value >> (32 - numberOfBits));
        }

        /// <summary>
        /// Bitwise circular left shift.
        /// </summary>
        public static long BitwiseRotateLeft(this long value, int numberOfBits)
        {
            return (value << numberOfBits) | (value >> (64 - numberOfBits));
        }

        /// <summary>
        /// Bitwise circular left shift.
        /// </summary>
        public static ulong BitwiseRotateLeft(this ulong value, int numberOfBits)
        {
            return (value << numberOfBits) | (value >> (64 - numberOfBits));
        }

        #endregion

        #region Digits Counts of Integer/Fractional Parts

        public static int DigitCountOfIntegerPart(this double value)
        {
            var str = value.ToString(Localization.FormatEnUS);
            var x = str.Split('.');
            return x[0].Length;
        }

        public static int DigitCountOfFractionalPart(this double value)
        {
            var str = value.ToString(Localization.FormatEnUS);
            var x = str.Split('.');
            return x.Length < 2 ? 0 : x[1].Length;
        }

        public static int DigitCountOfIntegerPart(this float value)
        {
            return ((double)value).DigitCountOfIntegerPart();
        }

        public static int DigitCountOfFractionalPart(this float value)
        {
            return ((double)value).DigitCountOfFractionalPart();
        }

        #endregion

        #region InvLerp

        /// <summary>
        /// The inverse of Lerp.
        /// </summary>
        [Pure]
        public static float InvLerp(this float y, float a, float b)
        { return (a - y) / (a - b); }

        /// <summary>
        /// The inverse of Lerp.
        /// </summary>
        [Pure]
        public static double InvLerp(this float y, double a, double b)
        { return (a - (double)y) / (a - b); }

        /// <summary>
        /// The inverse of Lerp.
        /// </summary>
        [Pure]
        public static float InvLerp(this double y, float a, float b)
        { return (a - (float)y) / (a - b); }

        /// <summary>
        /// The inverse of Lerp.
        /// </summary>
        [Pure]
        public static double InvLerp(this double y, double a, double b)
        { return (a - y) / (a - b); }

        #endregion

        #region Interpolation

        [Pure]
        public static byte Lerp(this float x, byte a, byte b)
        { return (byte)(a + (b - a) * x); }

        [Pure]
        public static ushort Lerp(this float x, ushort a, ushort b)
        { return (ushort)(a + (b - a) * x); }

        [Pure]
        public static short Lerp(this float x, short a, short b)
        { return (short)(a + (b - a) * x); }

        [Pure]
        public static uint Lerp(this float x, uint a, uint b)
        { return (uint)(a + (b - a) * x); }

        [Pure]
        public static int Lerp(this float x, int a, int b)
        { return (int)(a + (b - a) * x); }

        [Pure]
        public static float Lerp(this float x, float a, float b)
        { return a + (b - a) * x; }

        [Pure]
        public static double Lerp(this float x, double a, double b)
        { return a + (b - a) * (double)x; }

        [Pure]
        public static byte Lerp(this double x, byte a, byte b)
        { return (byte)(a + (b - a) * x); }

        [Pure]
        public static ushort Lerp(this double x, ushort a, ushort b)
        { return (ushort)(a + (b - a) * x); }

        [Pure]
        public static short Lerp(this double x, short a, short b)
        { return (short)(a + (b - a) * x); }

        [Pure]
        public static uint Lerp(this double x, uint a, uint b)
        { return (uint)(a + (b - a) * x); }

        [Pure]
        public static int Lerp(this double x, int a, int b)
        { return (int)(a + (b - a) * x); }

        [Pure]
        public static float Lerp(this double x, float a, float b)
        { return a + (b - a) * (float)x; }

        [Pure]
        public static double Lerp(this double x, double a, double b)
        { return a + (b - a) * x; }

        #endregion

        #region Monotony

        public static bool IsMonotonicallyIncreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.E0.CompareTo(p.E1) > 0) return false;
            return true;
        }

        public static bool IsStrictlyIncreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.E0.CompareTo(p.E1) >= 0) return false;
            return true;
        }

        public static bool IsMonotonicallyDecreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.E0.CompareTo(p.E1) < 0) return false;
            return true;
        }

        public static bool IsStrictlyDecreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.E0.CompareTo(p.E1) <= 0) return false;
            return true;
        }

        #endregion

        #region Outer Sum

        public static Tup4<long> OuterSum(Tup2<long> x, Tup2<long> y)
        {
            return new Tup4<long>(x.E0 + y.E0, x.E1 + y.E0,
                                  x.E0 + y.E1, x.E1 + y.E1);
        }

        public static Tup8<long> OuterSum(Tup2<long> x, Tup2<long> y, Tup2<long> z)
        {
            long y0z0 = y.E0 + z.E0, y1z0 = y.E1 + z.E0, y0z1 = y.E0 + z.E1, y1z1 = y.E1 + z.E1;
            return new Tup8<long>(x.E0 + y0z0, x.E1 + y0z0, x.E0 + y1z0, x.E1 + y1z0,
                                  x.E0 + y0z1, x.E1 + y0z1, x.E0 + y1z1, x.E1 + y1z1);
        }

        public static Tup16<long> OuterSum(Tup4<long> x, Tup4<long> y)
        {
            return new Tup16<long>(x.E0 + y.E0, x.E1 + y.E0, x.E2 + y.E0, x.E3 + y.E0,
                                   x.E0 + y.E1, x.E1 + y.E1, x.E2 + y.E1, x.E3 + y.E1,
                                   x.E0 + y.E2, x.E1 + y.E2, x.E2 + y.E2, x.E3 + y.E2,
                                   x.E0 + y.E3, x.E1 + y.E3, x.E2 + y.E3, x.E3 + y.E3);
        }

        #endregion

        #region Relative Epsilon

        public static float PlusRelativeEps(this float value, float eps)
        {
            if (value > 0) return value + eps * value;
            if (value < 0) return value - eps * value;
            return eps > 0
                    ? value + Constant<float>.PositiveTinyValue
                    : value - Constant<float>.PositiveTinyValue;
        }

        public static float MinusRelativeEps(this float value, float eps)
        {
            if (value > 0) return value - eps * value;
            if (value < 0) return value + eps * value;
            return eps > 0
                    ? value - Constant<float>.PositiveTinyValue
                    : value + Constant<float>.PositiveTinyValue;
        }

        public static double PlusRelativeEps(this double value, float eps)
        {
            if (value > 0) return value + eps * value;
            if (value < 0) return value - eps * value;
            return eps > 0
                    ? value + Constant<float>.PositiveTinyValue
                    : value - Constant<float>.PositiveTinyValue;
        }

        public static double MinusRelativeEps(this double value, float eps)
        {
            if (value > 0) return value - eps * value;
            if (value < 0) return value + eps * value;
            return eps > 0
                    ? value - Constant<double>.PositiveTinyValue
                    : value + Constant<double>.PositiveTinyValue;
        }


        #endregion

        #region Shannon Entropy

        /// <summary>
        /// Shannon entropy of values.
        /// </summary>
        public static double Entropy<T>(this IEnumerable<T> xs)
        {
            double total = xs.Count();
            return xs.GroupBy(x => x)
                .Select(g => { double p = g.Count() / total; return p * -p.Log2(); })
                .Sum();
        }

        /// <summary>
        /// Shannon entropy of bipartite set.
        /// </summary>
        public static double Entropy<T>(this bool[] xs)
        {
            if (xs == null) throw new ArgumentNullException("xs");

            var count = xs.Length;
            var countPos = 0;
            var countNeg = 0;

            for (var i = 0; i < count; i++)
            {
                if (xs[i])
                {
                    countPos++;
                }
                else
                {
                    countNeg++;
                }
            }
            if (countPos == 0 || countNeg == 0) return 0.0;

            var pPos = countPos / (double)count;
            var pNeg = countNeg / (double)count;
            var hPos = -Math.Log(pPos, 2);
            var hNeg = -Math.Log(pNeg, 2);
            var H = pPos * hPos + pNeg * hNeg;
            return H;
        }

        /// <summary>
        /// Shannon entropy of weighted bipartite set.
        /// </summary>
        public static double Entropy(this bool[] xs, double[] weights)
        {
            if (weights == null) throw new ArgumentNullException("weights");
            if (xs == null) throw new ArgumentNullException("xs");
            if (weights.Length != xs.Length) throw new ArgumentException("weights.Length != xs.Length");

            var count = xs.Length;
            var countPos = 0.0;
            var countNeg = 0.0;

            for (var i = 0; i < count; i++)
            {
                if (xs[i])
                {
                    countPos += weights[i];
                }
                else
                {
                    countNeg += weights[i];
                }
            }
            if (countPos == 0 || countNeg == 0) return 0.0;

            var pPos = countPos / (double)count;
            var pNeg = countNeg / (double)count;
            var hPos = -Math.Log(pPos, 2);
            var hNeg = -Math.Log(pNeg, 2);
            var H = pPos * hPos + pNeg * hNeg;
            return H;
        }

        #endregion

        #region Signs

        public static Signs GetSigns(this double value, double epsilon)
        {
            if (value < -epsilon) return Signs.Negative;
            if (value > +epsilon) return Signs.Positive;
            return Signs.Zero;
        }

        public static void AggregateSigns(
                this IEnumerable<double> values, double epsilon,
                out int negativeCount, out int zeroCount, out int positiveCount)
        {
            int nc = 0, zc = 0, pc = 0;
            foreach (var v in values)
            {
                if (v < -epsilon) { ++nc; continue; }
                if (v > +epsilon) { ++pc; continue; }
                ++zc;
            }
            negativeCount = nc;
            zeroCount = zc;
            positiveCount = pc;
        }


        public static Signs AggregateSigns(this IEnumerable<double> values, double epsilon)
        {
            var signs = Signs.None;
            foreach (var v in values)
            {
                if (v < -epsilon) { signs |= Signs.Negative; continue; }
                if (v > +epsilon) { signs |= Signs.Positive; continue; }
                signs |= Signs.Zero;
            }
            return signs;
        }

        #endregion

        #region Step

        [Pure]
        public static T Step<T>(this float x, T a, T b)
        { return x < 0.5f ? a : b; }

        [Pure]
        public static T Step<T>(this double x, T a, T b)
        { return x < 0.5 ? a : b; }

        #endregion

        #region Weight Functions for Interpolation

        /// <summary>
        /// Creates weight function for cubic interpolation according to
        /// "Comparison of Interpolating for Image Resampling" by
        /// J.A.Parker, R.V.Kenyon &amp; D.E.Troxel, IEEE Transactions on
        /// Medical Imaging Vol. 2, 1983. Recommended values for a:
        /// -0.5 for medical/astronomical images, -1.0 for other photos.
        /// </summary>
        public static Func<double, Tup4<double>> CreateCubicTup4d(double a)
        {
            double ap2 = a + 2;
            double ap3 = a + 3;
            return t =>
            {
                double tt = t * t;
                double tastta = (t - tt) * a, ttasttta = t * tastta;
                double ttap3stttap2 = tt * (ap3 - t * ap2);
                return new Tup4<double>(tastta - ttasttta, 1 - ttap3stttap2,
                                         ttap3stttap2 - tastta, ttasttta);
            };
        }

        /// <summary>
        /// Creates weight function for cubic interpolation according to
        /// "Comparison of Interpolating for Image Resampling" by
        /// J.A.Parker, R.V.Kenyon &amp; D.E.Troxel, IEEE Transactions on
        /// Medical Imaging Vol. 2, 1983. Recommended values for a:
        /// -0.5 for medical/astronomical images, -1.0 for other photos.
        /// a = -0.5 exactly reconstructs second degree polynomials,
        /// a = -0.75 results in a continuous second derivative of the 
        /// two polynomials used
        /// a = -1.0 matches the slope of the sinc function at 1
        /// (amplifying frequencies just below the cutoff frequency)
        /// </summary>
        public static Func<double, Tup4<float>> CreateCubicTup4f(double a)
        {
            double ap2 = a + 2;
            double ap3 = a + 3;
            return t =>
            {
                double tt = t * t;
                double tastta = (t - tt) * a, ttasttta = t * tastta;
                double ttap3stttap2 = tt * (ap3 - t * ap2);
                return new Tup4<float>((float)(tastta - ttasttta), (float)(1 - ttap3stttap2),
                                       (float)(ttap3stttap2 - tastta), (float)ttasttta);

                // Weights:
                // a t^3 - 2 a t^2 + a t            = t ( a - a t (2 - t))
                // (a+2) t^3 - (a+3) t^2 + 1        = 1 - t^2 ((a+3) - (a+2) t)
                // -(a+2) t^3 + (2a+3) t^2 - a t    = t ( -a + t ((2a + 3) - (a+2) t))
                // -a t^3 + a t^2                   = t^2 a (1 - t)
            };
        }

        public static Tup6<double> Lanczos3d(double x)
        {
            const double a = 1.0/3.0;
            double px = Constant.Pi * x;
            double pxa = px * a;
            if (pxa.IsTiny()) return new Tup6<double>(0.0, 0.0, 1.0, 0.0, 0.0, 0.0);

            double p1px = Constant.Pi + px;
            double p2px = Constant.PiTimesTwo + px;
            double p1sx = Constant.Pi - px;
            double p2sx = Constant.PiTimesTwo - px;
            double p3sx = Constant.PiTimesThree - px;
            double spx = Fun.Sin(px);
            double spxa = Fun.Sin(px * a);
            double sp1pxa = Fun.Sin(p1px * a);
            double sp2pxa = Fun.Sin(p2px * a);

            return new Tup6<double>(
                    spx * sp2pxa / (p2px * p2px * a),
                    -spx * sp1pxa / (p1px * p1px * a),
                    (spx / px) * (spxa / (px * a)),
                    (spx / p1sx) * (sp2pxa / (p1sx * a)),
                    -spx * sp1pxa / (p2sx * p2sx * a),
                    spx * spxa / (p3sx * p3sx * a));
        }

        public static Tup6<float> Lanczos3f(double x)
        {
            const double a = 1.0 / 3.0;
            double px = Constant.Pi * x;
            double pxa = px * a;
            if (pxa.IsTiny()) return new Tup6<float>(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f);

            double p1px = Constant.Pi + px;
            double p2px = Constant.PiTimesTwo + px;
            double p1sx = Constant.Pi - px;
            double p2sx = Constant.PiTimesTwo - px;
            double p3sx = Constant.PiTimesThree - px;
            double spx = Fun.Sin(px);
            double spxa = Fun.Sin(px * a);
            double sp1pxa = Fun.Sin(p1px * a);
            double sp2pxa = Fun.Sin(p2px * a);

            return new Tup6<float>(
                    (float)(spx * sp2pxa / (p2px * p2px * a)),
                    (float)(-spx * sp1pxa / (p1px * p1px * a)),
                    (float)((spx / px) * (spxa / (px * a))),
                    (float)((spx / p1sx) * (sp2pxa / (p1sx * a))),
                    (float)(-spx * sp1pxa / (p2sx * p2sx * a)),
                    (float)(spx * spxa / (p3sx * p3sx * a)));
        }

        #endregion

        #region Pythagoras

        /// <summary>
        /// Computes sqrt(a²+b²) without destructive underflow or overflow.
        /// </summary>
        public static double Pythag(double a, double b)
        {
            double at = a > 0 ? a : -a, bt = b > 0 ? b : -b, ct, result;

            if (at > bt) { ct = bt / at; result = at * Math.Sqrt(1.0 + ct * ct); }
            else if (bt > 0.0) { ct = at / bt; result = bt * Math.Sqrt(1.0 + ct * ct); }
            else result = 0.0;
            return result;
        }

        #endregion

        #region IsNaN, Infinity, +/-Infinity

        public static bool IsNaN(this double v)
        {
            return Double.IsNaN(v);
        }

        public static bool IsInfinity(this double v)
        {
            return Double.IsInfinity(v);
        }

        public static bool IsNegativeInfinity(this double v)
        {
            return Double.IsNegativeInfinity(v);
        }

        public static bool IsPositiveInfinity(this double v)
        {
            return Double.IsPositiveInfinity(v);
        }

        public static bool IsNaN(this float v)
        {
            return Single.IsNaN(v);
        }

        public static bool IsInfinity(this float v)
        {
            return Single.IsInfinity(v);
        }

        public static bool IsNegativeInfinity(this float v)
        {
            return Single.IsNegativeInfinity(v);
        }

        public static bool IsPositiveInfinity(this float v)
        {
            return Single.IsPositiveInfinity(v);
        }

        #endregion IsNaN

        #region Log2Int

        /// <summary>
        /// the number of bits used for storing the double-mantissa
        /// </summary>
        public const int DoubleMantissaBits = 52;

        /// <summary>
        /// the bitmask used for the double exponent
        /// </summary>
        public const ulong DoubleExponentMask = 0x7FF0000000000000;

        /// <summary>
        /// the bitmask used for the double mantissa
        /// </summary>
        public const ulong DoubleMantissaMask = 0x000FFFFFFFFFFFFF;

        /// <summary>
        /// the number of bits used for storing the float-mantissa
        /// </summary>
        public const int FloatMantissaBits = 23;

        /// <summary>
        /// the bitmask used for the float exponent
        /// </summary>
        public const uint FloatExponentMask = 0x7F000000;

        /// <summary>
        /// the bitmask used for the float mantissa
        /// </summary>
        public const uint FloatMantissaMask = 0x00FFFFFF;


        private static unsafe int Log2IntRef(ref double v)
        {
            fixed (double* ptr = &v)
            {
                var a = (ulong*)ptr;
                var shift = 1022;

                if ((*a & DoubleMantissaMask) == 0)
                    shift = 1023;

                return (int)(((*a & DoubleExponentMask) >> DoubleMantissaBits)) - shift;
            }
        }

        private static unsafe int Log2IntRef(ref float v)
        {
            fixed (float* ptr = &v)
            {
                var a = (uint*)ptr;
                var shift = 126;

                if ((*a & FloatMantissaMask) == 0)
                    shift = 127;

                return (int)(((*a & FloatExponentMask) >> FloatMantissaBits)) - shift;
            }
        }

        /// <summary>
        /// efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        public static unsafe int Log2Int(this double v)
        {
            return Log2IntRef(ref v);
        }

        /// <summary>
        /// efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        private static unsafe int Log2Int(this float v)
        {
            return Log2IntRef(ref v);
        }

        #endregion
    }

    #region KahanSum

    /// <summary>
    /// A simple quadruple precision sum (around 108 bit) based on Kahan's
    /// summation method. This is used in Stats, to maintain higher precision
    /// sums for statistical moment computation.
    /// </summary>
    public struct KahanSum
    {
        private double m_sum;
        private double m_carry;

        public KahanSum(double sum, double carry)
        {
            m_sum = sum;
            m_carry = carry;
        }

        public KahanSum(double sum)
            : this(sum, 0.0)
        { }

        public void Add(double value)
        {
            double y = value - m_carry;
            double t = m_sum + y;
            m_carry = (t - m_sum) - y;
            m_sum = t;
        }

        public void Sub(double value)
        {
            double y = -value - m_carry;
            double t = m_sum + y;
            m_carry = (t - m_sum) - y;
            m_sum = t;
        }

        public void Add(KahanSum sum)
        {
            Sub(sum.m_carry);
            Add(sum.m_sum);
        }

        public void Sub(KahanSum sum)
        {
            Add(sum.m_carry);
            Sub(sum.m_sum);
        }

        public double Value
        {
            get { return m_sum; }
        }

        public static KahanSum operator +(KahanSum sum, double value)
        {
            double y = value - sum.m_carry;
            double t = sum.m_sum + y;
            return new KahanSum(t, (t - sum.m_sum) - y);
        }

        public static KahanSum operator -(KahanSum sum, double value)
        {
            double y = -value - sum.m_carry;
            double t = sum.m_sum + y;
            return new KahanSum(t, (t - sum.m_sum) - y);
        }

        public static KahanSum operator +(KahanSum sum0, KahanSum sum1)
        {
            var r = sum0 - sum1.m_carry;
            r.Add(sum1.m_sum);
            return r;
        }

        public static KahanSum operator -(KahanSum sum0, KahanSum sum1)
        {
            var r = sum0 + sum1.m_carry;
            r.Sub(sum1.m_sum);
            return r;
        }

        public static readonly KahanSum Zero = new KahanSum(0.0);
    }

    #endregion


}
