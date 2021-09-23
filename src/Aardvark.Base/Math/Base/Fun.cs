using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

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
            var str = value.ToString(CultureInfo.InvariantCulture);
            var x = str.Split('.');
            return x[0].Length;
        }

        public static int DigitCountOfFractionalPart(this double value)
        {
            var str = value.ToString(CultureInfo.InvariantCulture);
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

        #region Monotony

        public static bool IsMonotonicallyIncreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.Item1.CompareTo(p.Item2) > 0) return false;
            return true;
        }

        public static bool IsStrictlyIncreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.Item1.CompareTo(p.Item2) >= 0) return false;
            return true;
        }

        public static bool IsMonotonicallyDecreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.Item1.CompareTo(p.Item2) < 0) return false;
            return true;
        }

        public static bool IsStrictlyDecreasing<T>(this IEnumerable<T> self)
            where T : IComparable
        {
            foreach (var p in self.PairChain())
                if (p.Item1.CompareTo(p.Item2) <= 0) return false;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        public static void AggregateSigns(
                this (double, double) values, double epsilon,
                out int negativeCount, out int zeroCount, out int positiveCount)
            => AggregateSigns(new[] { values.Item1, values.Item2 }, epsilon, out negativeCount, out zeroCount, out positiveCount);

        public static void AggregateSigns(
                this (double, double, double) values, double epsilon,
                out int negativeCount, out int zeroCount, out int positiveCount)
            => AggregateSigns(new[] { values.Item1, values.Item2, values.Item3 }, epsilon, out negativeCount, out zeroCount, out positiveCount);

        public static void AggregateSigns(
                this (double, double, double, double) values, double epsilon,
                out int negativeCount, out int zeroCount, out int positiveCount)
            => AggregateSigns(new[] { values.Item1, values.Item2, values.Item3, values.Item4 }, epsilon, out negativeCount, out zeroCount, out positiveCount);

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

        public static Signs AggregateSigns(this (double, double) values, double epsilon)
            => AggregateSigns(new[] { values.Item1, values.Item2 }, epsilon);

        public static Signs AggregateSigns(this (double, double, double) values, double epsilon)
            => AggregateSigns(new[] { values.Item1, values.Item2, values.Item3 }, epsilon);

        public static Signs AggregateSigns(this (double, double, double, double) values, double epsilon)
            => AggregateSigns(new[] { values.Item1, values.Item2, values.Item3, values.Item4 }, epsilon);

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
        /// a = -0.5 exactly reconstructs second degree polynomials,
        /// a = -0.75 results in a continuous second derivative of the 
        /// two polynomials used
        /// a = -1.0 matches the slope of the sinc function at 1
        /// (amplifying frequencies just below the cutoff frequency)
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
                // Weights:
                // a t^3 - 2 a t^2 + a t            = t ( a - a t (2 - t))
                // (a+2) t^3 - (a+3) t^2 + 1        = 1 - t^2 ((a+3) - (a+2) t)
                // -(a+2) t^3 + (2a+3) t^2 - a t    = t ( -a + t ((2a + 3) - (a+2) t))
                // -a t^3 + a t^2                   = t^2 a (1 - t)
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
            };
        }

        /// <summary>
        /// Returns the weights of the cubic B-Spline function for four
        /// equally distant function points to approximate. The supplied
        /// parameter t in the range [0..1] traverses the approximation
        /// between the two center points.
        /// </summary>
        public static Tup4<double> BSpline3d(double t) => new Tup4<double>(
            t*(t*(t*(-1/6.0) + 3/6.0) - 3/6.0) + 1/6.0,
            t*(t*(t*( 3/6.0) - 6/6.0)        ) + 4/6.0,
            t*(t*(t*(-3/6.0) + 3/6.0) + 3/6.0) + 1/6.0,
            t*(t*(t*( 1/6.0)        )        )        );

        /// <summary>
        /// Returns the weights of the cubic B-Spline function for four
        /// equally distant function points to approximate. The supplied
        /// parameter t in the range [0..1] traverses the approximation
        /// between the two center points.
        /// </summary>
        public static Tup4<float> BSpline3f(double t) => new Tup4<float>(
            (float)(t*(t*(t*(-1/6.0) + 3/6.0) - 3/6.0) + 1/6.0),
            (float)(t*(t*(t*( 3/6.0) - 6/6.0)        ) + 4/6.0),
            (float)(t*(t*(t*(-3/6.0) + 3/6.0) + 3/6.0) + 1/6.0),
            (float)(t*(t*(t*( 1/6.0)        )        )        ));

        /// <summary>
        /// Returns the weights of the order 5 B-Spline function for six
        /// equally distant function points to approximate. The supplied
        /// parameter t in the range [0..1] traverses the approximation
        /// between the two center points.
        /// </summary>
        public static Tup6<double> BSpline5d(double t) => new Tup6<double>(
            t*(t*(t*(t*(t*( -1/120.0)  +5/120.0) -10/120.0) +10/120.0)  -5/120.0)  +1/120.0,
            t*(t*(t*(t*(t*(  5/120.0) -20/120.0) +20/120.0) +20/120.0) -50/120.0) +26/120.0,
            t*(t*(t*(t*(t*(-10/120.0) +30/120.0)          ) -60/120.0)          ) +66/120.0,
            t*(t*(t*(t*(t*( 10/120.0) -20/120.0) -20/120.0) +20/120.0) +50/120.0) +26/120.0,
            t*(t*(t*(t*(t*( -5/120.0)  +5/120.0) +10/120.0) +10/120.0)  +5/120.0)  +1/120.0,
            t*(t*(t*(t*(t*(  1/120.0) 		   ) 		  ) 	         ) 	        )          );

        /// <summary>
        /// Returns the weights of the order 5 B-Spline function for six
        /// equally distant function points to approximate. The supplied
        /// parameter t in the range [0..1] traverses the approximation
        /// between the two center points.
        /// </summary>
        public static Tup6<float> BSpline5f(double t) => new Tup6<float>(
            (float)(t*(t*(t*(t*(t*( -1/120.0)  +5/120.0) -10/120.0) +10/120.0)  -5/120.0)  +1/120.0),
            (float)(t*(t*(t*(t*(t*(  5/120.0) -20/120.0) +20/120.0) +20/120.0) -50/120.0) +26/120.0),
            (float)(t*(t*(t*(t*(t*(-10/120.0) +30/120.0)          ) -60/120.0)          ) +66/120.0),
            (float)(t*(t*(t*(t*(t*( 10/120.0) -20/120.0) -20/120.0) +20/120.0) +50/120.0) +26/120.0),
            (float)(t*(t*(t*(t*(t*( -5/120.0)  +5/120.0) +10/120.0) +10/120.0)  +5/120.0)  +1/120.0),
            (float)(t*(t*(t*(t*(t*(  1/120.0) 		   ) 		  ) 	         ) 	        )          ));

        public static Tup6<double> Lanczos3d(double x)
        {
            const double a = 1.0/3.0;
            double px = Constant.Pi * x, pxa = px * a;
            if (pxa.IsTiny()) return new Tup6<double>(0.0, 0.0, 1.0, 0.0, 0.0, 0.0);
            double p1mx = Constant.Pi - px, p1mxa = p1mx * a;
            if (p1mxa.IsTiny()) return new Tup6<double>(0.0, 0.0, 0.0, 1.0, 0.0, 0.0);

            double p1px = Constant.Pi + px, p2px = Constant.PiTimesTwo + px;
            double p2mx = Constant.PiTimesTwo - px, p3mx = Constant.PiTimesThree - px;
            double spx = Fun.Sin(px), spxa = Fun.Sin(pxa);
            double sp1pxa = Fun.Sin(p1px * a), sp2pxa = Fun.Sin(p2px * a);

            return new Tup6<double>(spx * sp2pxa / (p2px * p2px * a),
                                    -spx * sp1pxa / (p1px * p1px * a),
                                    (spx / px) * (spxa / pxa),
                                    (spx / p1mx) * (sp2pxa / p1mxa),
                                    -spx * sp1pxa / (p2mx * p2mx * a),
                                    spx * spxa / (p3mx * p3mx * a));
        }

        public static Tup6<float> Lanczos3f(double x)
        {
            const double a = 1.0 / 3.0;
            double px = Constant.Pi * x, pxa = px * a;
            if (pxa.IsTiny()) return new Tup6<float>(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            double p1mx = Constant.Pi - px, p1mxa = p1mx * a;
            if (p1mxa.IsTiny()) return new Tup6<float>(0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);

            double p1px =   Constant.Pi + px,           p2px =  Constant.PiTimesTwo + px;
            double p2mx =   Constant.PiTimesTwo - px,   p3mx =  Constant.PiTimesThree - px;
            double spx =    Fun.Sin(px),                spxa =  Fun.Sin(pxa);
            double sp1pxa = Fun.Sin(p1px * a),          sp2pxa = Fun.Sin(p2px * a);

            return new Tup6<float>( (float)(spx * sp2pxa / (p2px * p2px * a)),
                                    (float)(-spx * sp1pxa / (p1px * p1px * a)),
                                    (float)((spx / px) * (spxa / pxa)),
                                    (float)((spx / p1mx) * (sp2pxa / p1mxa)),
                                    (float)(-spx * sp1pxa / (p2mx * p2mx * a)),
                                    (float)(spx * spxa / (p3mx * p3mx * a)));
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
        public const uint FloatExponentMask = 0x7F800000;

        /// <summary>
        /// the bitmask used for the float mantissa
        /// </summary>
        public const uint FloatMantissaMask = 0x007FFFFF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe int Log2IntRef(ref double v)
        {
            fixed (double* ptr = &v)
            {
                var a = (ulong*)ptr;
                return (int)(((*a & DoubleExponentMask) >> DoubleMantissaBits)) - 1023;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe int Log2IntRef(ref float v)
        {
            fixed (float* ptr = &v)
            {
                var a = (uint*)ptr;
                return (int)(((*a & FloatExponentMask) >> FloatMantissaBits)) - 127;
            }
        }

        /// <summary>
        /// Efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2Int(this double v)
        {
            return Log2IntRef(ref v);
        }

        /// <summary>
        /// Efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2Int(this float v)
        {
            return Log2IntRef(ref v);
        }

        /// <summary>
        /// Efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2Int(this int v)
        {
            #if NETCOREAPP3_1
                return System.Numerics.BitOperations.Log2((uint)v);
            #else
                return Log2Int((float)v);
            #endif
        }

        /// <summary>
        /// Efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2Int(this uint v)
        {
            #if NETCOREAPP3_1
                return System.Numerics.BitOperations.Log2(v);
            #else
                return Log2Int((float)v);
            #endif
        }

        /// <summary>
        /// Efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2Int(this long v)
        {
            #if NETCOREAPP3_1
                return System.Numerics.BitOperations.Log2((ulong)v);
            #else
                return Log2Int((double)v);
            #endif
        }

        /// <summary>
        /// Efficiently computes the Log2 for the given value rounded to the next integer towards -inf
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2Int(this ulong v)
        {
            #if NETCOREAPP3_1
                return System.Numerics.BitOperations.Log2(v);
            #else
                return Log2Int((double)v);
            #endif
        }

        #endregion

        #region Mipmap levels

        /// <summary>
        /// Computes the number of 3D images in a mipmap chain with the given base size.
        /// </summary>
        /// <param name="baseSize">The size of the image in the base level.</param>
        /// <returns>The total number of levels.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MipmapLevels(V3i baseSize)
            => 1 + Log2Int(baseSize.MaxElement);

        /// <summary>
        /// Computes the number of 2D images in a mipmap chain with the given base size.
        /// </summary>
        /// <param name="baseSize">The size of the image in the base level.</param>
        /// <returns>The total number of levels.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MipmapLevels(V2i baseSize)
            => 1 + Log2Int(baseSize.MaxElement);

        /// <summary>
        /// Computes the number of 1D images in a mipmap chain with the given base size.
        /// </summary>
        /// <param name="baseSize">The size of the image in the base level.</param>
        /// <returns>The total number of levels.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MipmapLevels(int baseSize)
            => 1 + Log2Int(baseSize);

        /// <summary>
        /// Computes the size of a 3D image in a mipmap chain.
        /// </summary>
        /// <param name="baseSize">The size of the image in the base level.</param>
        /// <param name="level">The level of the queried image (base level = 0).</param>
        /// <returns>The size of the image at the given level.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3i MipmapLevelSize(V3i baseSize, int level)
            => Max(baseSize >> level, 1);

        /// <summary>
        /// Computes the size of a 2D image in a mipmap chain.
        /// </summary>
        /// <param name="baseSize">The size of the image in the base level.</param>
        /// <param name="level">The level of the queried image (base level = 0).</param>
        /// <returns>The size of the image at the given level.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2i MipmapLevelSize(V2i baseSize, int level)
            => Max(baseSize >> level, 1);

        /// <summary>
        /// Computes the size of a 1D image in a mipmap chain.
        /// </summary>
        /// <param name="baseSize">The size of the image in the base level.</param>
        /// <param name="level">The level of the queried image (base level = 0).</param>
        /// <returns>The size of the image at the given level.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MipmapLevelSize(int baseSize, int level)
            => Max(baseSize >> level, 1);

        #endregion

        #region Gaussian

        /// <summary>
        /// Returns the function value of the normalized Gaussian distribution
        /// with the given standard deviation.
        /// </summary>
        public static double Gauss(double x, double stdDev)
        {
            var rcpS = 1.0 / stdDev;
            return rcpS / Constant.SqrtPiTimesTwo * Fun.Exp(-0.5 * (x * stdDev).Square());
        }

        /// <summary>
        /// Returns the function value of the normalized elliptical 2d Gaussian distribution
        /// with different standard deviations in x and y.
        /// </summary>
        public static double Gauss2d(double x, double y, double sx, double sy)
        {
            return 1.0 / (sx * sy * Constant.PiTimesTwo) * Fun.Exp(-0.5 * ((x / sx).Square() + (y / sy).Square()));
        }

        /// <summary>
        /// Returns the function value of the normalized elliptical 2d Gaussian distribution
        /// with different standard deviations in x and y.
        /// </summary>
        public static double Gauss2d(V2d p, double sx, double sy)
        {
            return 1.0 / (sx * sy * Constant.PiTimesTwo) * Fun.Exp(-0.5 * ((p.X / sx).Square() + (p.Y / sy).Square()));
        }

        /// <summary>
        /// Returns the function value of the normalized 2d Gaussian distribution with 
        /// given symmetrical standard deviation.
        /// </summary>
        public static double Gauss2d(double x, double y, double s)
        {
            var halfRcpS2 = 0.5 / s.Square();
            return halfRcpS2 * Constant.PiInv * Fun.Exp(-(x * x + y * y) * halfRcpS2);
        }

        /// <summary>
        /// Returns the function value of the normalized 2d Gaussian distribution with 
        /// given symmetrical standard deviation.
        /// </summary>
        public static double Gauss2d(V2d p, double s)
        {
            var halfRcpS2 = 0.5 / s.Square();
            return halfRcpS2 * Constant.PiInv * Fun.Exp(-p.LengthSquared * halfRcpS2);
        }

        /// <summary>
        /// Gaussian error function using a numerical approximation
        /// https://www.johndcook.com/blog/csharp_erf/
        /// </summary>
        public static double Erf(double x)
        {
            // constants
            double a1 =  0.254829592;
            double a2 = -0.284496736;
            double a3 =  1.421413741;
            double a4 = -1.453152027;
            double a5 =  1.061405429;
            double p  =  0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }

        /// <summary>
        /// Gaussian error function using a numerical approximation with 
        /// a maximum error of 1.2e-7
        /// Wikipedia https://en.wikipedia.org/wiki/Error_function#Numerical_approximations
        /// </summary>
        public static double Erf2(double x)
        {
            // constants
            double a = -1.26551223;
            double b =  1.00002368;
            double c =  0.37409196;
            double d =  0.09678418;
            double e = -0.18628806;
            double f =  0.27886807;
            double g = -1.13520398;
            double h =  1.48851587;
            double j = -0.82215223;
            double k =  0.17087277;

            var t = 1.0 / (1.0 + 0.5 * x.Abs());
            var r = t * Fun.Exp(-x.Square() + a + t * (b + t * (c + t * (d + t * (e + t * (f + t * (g + t * (h + t * (j + t * k)))))))));

            return x >= 0 ? 1 - r : r - 1;
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
