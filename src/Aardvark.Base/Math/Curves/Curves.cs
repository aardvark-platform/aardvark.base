using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    public static partial class Ipol
    {
        public static partial class CubicHermite
        {
            public static Func<double, V3d, V3d, V3d, V3d, V3d> Fun(int derivative)
            {
                if (derivative < 0) throw new IndexOutOfRangeException();
                switch (derivative)
                {
                    case 0: return Eval;
                    case 1: return EvalD1;
                    case 2: return EvalD2;
                    case 3: return EvalD3;
                    default: return (t, a, b, tIn, tOut) => V3d.Zero;
                }
            }

            public static double Eval(double t, double a, double b, double tIn, double tOut)
            {
                GetWeights(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V2d Eval(double t, V2d a, V2d b, V2d tIn, V2d tOut)
            {
                GetWeights(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V3d Eval(double t, V3d a, V3d b, V3d tIn, V3d tOut)
            {
                GetWeights(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }

            public static double EvalD1(double t, double a, double b, double tIn, double tOut)
            {
                GetWeightsD1(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V2d EvalD1(double t, V2d a, V2d b, V2d tIn, V2d tOut)
            {
                GetWeightsD1(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V3d EvalD1(double t, V3d a, V3d b, V3d tIn, V3d tOut)
            {
                GetWeightsD1(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }

            public static double EvalD2(double t, double a, double b, double tIn, double tOut)
            {
                GetWeightsD2(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V2d EvalD2(double t, V2d a, V2d b, V2d tIn, V2d tOut)
            {
                GetWeightsD2(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V3d EvalD2(double t, V3d a, V3d b, V3d tIn, V3d tOut)
            {
                GetWeightsD2(t, out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }

            public static double EvalD3(double t, double a, double b, double tIn, double tOut)
            {
                GetWeightsD3(out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V2d EvalD3(double t, V2d a, V2d b, V2d tIn, V2d tOut)
            {
                GetWeightsD3(out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }
            public static V3d EvalD3(double t, V3d a, V3d b, V3d tIn, V3d tOut)
            {
                GetWeightsD3(out var w0, out var w1, out var w2, out var w3);
                return w0 * a + w1 * tIn + w2 * tOut + w3 * b;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void GetWeights(
                double t, out double w0, out double w1, out double w2, out double w3)
            {
                var tt = t * t; var ttt = tt * t;
                var tt3 = tt * 3; var ttt2 = ttt * 2;
                w0 = ttt2 - tt3 + 1;
                w1 = ttt - 2 * tt + t;
                w2 = ttt - tt;
                w3 = -ttt2 + tt3;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void GetWeightsD1(
                double t, out double w0, out double w1, out double w2, out double w3)
            {
                var tt = t * t;
                var tt6 = tt * 6; var tt3 = tt * 3; var t6 = t * 6;
                w0 = tt6 - t6;
                w1 = tt3 - 4 * t + 1;
                w2 = tt3 - 2 * t;
                w3 = -tt6 + t6;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void GetWeightsD2(
                double t, out double w0, out double w1, out double w2, out double w3)
            {
                var t12 = t * 12; var t6 = t * 6;
                w0 = t12 - 6;
                w1 = t6 - 4;
                w2 = t6 - 2;
                w3 = -t12 + 6;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void GetWeightsD3(
                out double w0, out double w1, out double w2, out double w3)
            {
                w0 = 12;
                w1 = 6;
                w2 = 6;
                w3 = -12;
            }
        }

        public static partial class CatmullRom
        {
            public static Func<double, V3d, V3d, V3d, V3d, V3d> Fun(int derivative)
            {
                if (derivative < 0) throw new IndexOutOfRangeException();
                switch (derivative)
                {
                    case 0: return Eval;
                    case 1: return EvalD1;
                    case 2: return EvalD2;
                    case 3: return EvalD3;
                    default: return (t, p0, p1, p2, p3) => V3d.Zero;
                }
            }

            public static double Eval(double t, double p0, double p1, double p2, double p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.Eval(t, p1, p2, m0, m1);
            }
            public static V2d Eval(double t, V2d p0, V2d p1, V2d p2, V2d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.Eval(t, p1, p2, m0, m1);
            }
            public static V3d Eval(double t, V3d p0, V3d p1, V3d p2, V3d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.Eval(t, p1, p2, m0, m1);
            }

            public static double EvalD1(double t, double p0, double p1, double p2, double p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD1(t, p1, p2, m0, m1);
            }
            public static V2d EvalD1(double t, V2d p0, V2d p1, V2d p2, V2d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD1(t, p1, p2, m0, m1);
            }
            public static V3d EvalD1(double t, V3d p0, V3d p1, V3d p2, V3d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD1(t, p1, p2, m0, m1);
            }

            public static double EvalD2(double t, double p0, double p1, double p2, double p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD2(t, p1, p2, m0, m1);
            }
            public static V2d EvalD2(double t, V2d p0, V2d p1, V2d p2, V2d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD2(t, p1, p2, m0, m1);
            }
            public static V3d EvalD2(double t, V3d p0, V3d p1, V3d p2, V3d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD2(t, p1, p2, m0, m1);
            }

            public static double EvalD3(double t, double p0, double p1, double p2, double p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD3(t, p1, p2, m0, m1);
            }
            public static V2d EvalD3(double t, V2d p0, V2d p1, V2d p2, V2d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD3(t, p1, p2, m0, m1);
            }
            public static V3d EvalD3(double t, V3d p0, V3d p1, V3d p2, V3d p3)
            {
                var m0 = (p2 - p0) * 0.5;
                var m1 = (p3 - p1) * 0.5;
                return CubicHermite.EvalD3(t, p1, p2, m0, m1);
            }
        }

        public static partial class KochanekBartels
        {
            public static Func<double, V3d, V3d, V3d, V3d, double, double, V3d> Fun(int derivative)
            {
                if (derivative < 0) throw new IndexOutOfRangeException();
                switch (derivative)
                {
                    case 0: return Eval;
                    case 1: return EvalD1;
                    case 2: return EvalD2;
                    case 3: return EvalD3;
                    default: return (t, p0, p1, p2, p3, tension, bias) => V3d.Zero;
                }
            }

            public static double Eval(double t,
                double p0, double p1, double p2, double p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.Eval(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V2d Eval(double t,
                V2d p0, V2d p1, V2d p2, V2d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.Eval(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V3d Eval(double t,
                V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.Eval(t, p1, p2, tangents.Item1, tangents.Item2);
            }

            public static double EvalD1(double t,
                double p0, double p1, double p2, double p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD1(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V2d EvalD1(double t,
                V2d p0, V2d p1, V2d p2, V2d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD1(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V3d EvalD1(double t,
                V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD1(t, p1, p2, tangents.Item1, tangents.Item2);
            }

            public static double EvalD2(double t,
                double p0, double p1, double p2, double p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD2(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V2d EvalD2(double t,
                V2d p0, V2d p1, V2d p2, V2d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD2(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V3d EvalD2(double t,
                V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD2(t, p1, p2, tangents.Item1, tangents.Item2);
            }

            public static double EvalD3(double t,
                double p0, double p1, double p2, double p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD3(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V2d EvalD3(double t,
                V2d p0, V2d p1, V2d p2, V2d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD3(t, p1, p2, tangents.Item1, tangents.Item2);
            }
            public static V3d EvalD3(double t,
                V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            {
                var tangents = Tangents(p0, p1, p2, p3, tension, bias);
                return CubicHermite.EvalD3(t, p1, p2, tangents.Item1, tangents.Item2);
            }

            /// <summary>
            /// Computes the bounding box for interval [0, 1] of the
            /// Kochanek-Bartels spline defined by the specified points,
            /// tension, and bias.
            /// </summary>
            public static Box3d Bounds(
                V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            {
                return Bounds(Range1d.Unit, p0, p1, p2, p3, tension, bias);
            }

            /// <summary>
            /// Computes the bounding box for interval [0, 1] of the
            /// Kochanek-Bartels spline defined by the specified points.
            /// </summary>
            public static Box3d Bounds(V3d p0, V3d p1, V3d p2, V3d p3)
            {
                return Bounds(Range1d.Unit, p0, p1, p2, p3, 0, 0);
            }

            /// <summary>
            /// Computes the bounding box of the Kochanek-Bartels spline
            /// defined by points the specified points, tension, and bias.
            /// </summary>
            public static Box3d Bounds(Range1d domain,
                V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            {
                var oneMinusTensionHalf = (1 - tension) * 0.5;
                var x1 = oneMinusTensionHalf * (1 + bias);
                var x2 = oneMinusTensionHalf * (1 - bias);
                var s0 = p1 - p0; var s1 = p2 - p1; var s2 = p3 - p2;
                var m0 = x1 * s0 + x2 * s1;
                var m1 = x1 * s1 + x2 * s2;

                var bb = Box3d.Invalid;
                bb.ExtendBy(domain.Min > 0
                    ? Eval(domain.Min, p0, p1, p2, p3, tension, bias)
                    : p1);
                bb.ExtendBy(domain.Max < 1
                    ? Eval(domain.Max, p0, p1, p2, p3, tension, bias)
                    : p2);

                var a = 6 * p1 + 3 * m0 + 3 * m1 - 6 * p2;
                var b = -6 * p1 - 4 * m0 - 2 * m1 + 6 * p2;
                var c = m0;

                for (int i = 0; i < 3; i++)
                {
                    var t = Polynomial.RealRootsOf(a[i], b[i], c[i]);
                    var p0x = p0[i]; var p1x = p1[i]; var p2x = p2[i]; var p3x = p3[i];
                    if (t.Item1 > domain.Min && t.Item1 < domain.Max) bb.ExtendDimBy(i,
                        Eval(t.Item1, p0x, p1x, p2x, p3x, tension, bias)
                        );
                    if (t.Item2 > domain.Min && t.Item2 < domain.Max) bb.ExtendDimBy(i,
                        Eval(t.Item2, p0x, p1x, p2x, p3x, tension, bias)
                        );
                }

                return bb;
            }

            /// <summary>
            /// Computes incoming (Item1) and outgoing (Item2) tangent.
            /// </summary>
            private static (double, double) Tangents(
                double p0, double p1, double p2, double p3, double tension, double bias)
            {
                var oneMinusTensionHalf = (1 - tension) * 0.5;
                var x1 = oneMinusTensionHalf * (1 + bias);
                var x2 = oneMinusTensionHalf * (1 - bias);
                var s0 = p1 - p0; var s1 = p2 - p1; var s2 = p3 - p2;
                return (x1 * s0 + x2 * s1, x1 * s1 + x2 * s2);
            }
            /// <summary>
            /// Computes incoming (Item1) and outgoing (Item2) tangent.
            /// </summary>
            private static (V2d, V2d) Tangents(
                V2d p0, V2d p1, V2d p2, V2d p3, double tension, double bias)
            {
                var oneMinusTensionHalf = (1 - tension) * 0.5;
                var x1 = oneMinusTensionHalf * (1 + bias);
                var x2 = oneMinusTensionHalf * (1 - bias);
                var s0 = p1 - p0; var s1 = p2 - p1; var s2 = p3 - p2;
                return (x1 * s0 + x2 * s1, x1 * s1 + x2 * s2);
            }
            /// <summary>
            /// Computes incoming (Item1) and outgoing (Item2) tangent.
            /// </summary>
            private static (V3d, V3d) Tangents(
                V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            {
                var oneMinusTensionHalf = (1 - tension) * 0.5;
                var x1 = oneMinusTensionHalf * (1 + bias);
                var x2 = oneMinusTensionHalf * (1 - bias);
                var s0 = p1 - p0; var s1 = p2 - p1; var s2 = p3 - p2;
                return (x1 * s0 + x2 * s1, x1 * s1 + x2 * s2);
            }
        }
    }

    public class CurvePoints<T>
    {
        private readonly double[] m_params;
        private readonly T[] m_items;
        private Range1i m_indexes;

        public CurvePoints(double[] parameters, T[] items)
        {
            if (parameters.Length != items.Length) throw new ArgumentException();
            if (!parameters.IsStrictlyIncreasing()) throw new ArgumentException();

            m_params = parameters;
            m_items = items;
            Init();
        }

        public CurvePoints(IEnumerable<Tup<double, T>> items)
        {
            var ordered = items.OrderBy(item => item.E0).ToArray();
            m_params = ordered.Select(x => x.E0).ToArray();
            m_items = ordered.Select(x => x.E1).ToArray();
            Init();
        }

        private void Init()
        {
            m_indexes = new Range1i(0, m_items.Length - 1);
        }

        public Tup<double, T>[] Neighbourhood(double t)
        {
            int i = m_params.IndexOfLargestLessOrEqual(t).Clamp(m_indexes);
            int j = (i + 1).Clamp(m_indexes);
            return new Tup<double, T>[]
            {
                new Tup<double, T>(m_params[i], m_items[i]),
                new Tup<double, T>(m_params[j], m_items[j]),
            };
        }

        public Tup<double, T>[] Neighbourhood(
            double t, int extendLeft, int extendRight)
        {
            int floor = m_params.IndexOfLargestLessOrEqual(t).Clamp(m_indexes);
            var result = new Tup<double, T>[2 + extendLeft + extendRight];
            int imin = floor - extendLeft;
            for (int i = imin; i <= floor + 1 + extendRight; i++)
            {
                int index = i.Clamp(m_indexes);
                result[i - imin] = new Tup<double, T>(m_params[index], m_items[index]);
            }
            return result;
        }
    }
}
