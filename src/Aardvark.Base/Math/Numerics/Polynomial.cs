/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Collections.Generic;

namespace Aardvark.Base;

public static class Polynomial
{
    /// <summary>
    /// Return the polynomial derivative of the supplied polynomial that
    /// is stored with ascending coefficients:
    /// coeff[0] + coeff[1] x + coeff[2] x^2 + ... + coeff[n-1] x^(n-1)
    /// </summary>
    public static double[] Derivative(this double[] coeff)
    {
        int len = coeff.Length - 1;
        if (len < 1) throw new ArgumentException("Need at least 2 coefficients.", nameof(coeff));
        var r = new double[len];
        r[0] = coeff[1]; if (len < 2) return r;
        for (int i = 1, j = 2; i < len; i = j++)
            r[i] = j * coeff[j];
        return r;
    }

    /// <summary>
    /// Return the polynomial product of two polynomials that are supplied
    /// with ascending coefficients:
    /// ( c0[0] + c0[1] x + c0[2] x^2 + ... + c0[n-1] x^(n-1) )
    /// * ( c1[0] + c1[1] x + c1[2] x^2 + ... + c1[n-1] x^(n-1) )
    /// </summary>
    public static double[] Multiply(this double[] c0, double[] c1)
    {
        int l0 = c0.Length;
        int l1 = c1.Length;
        var r = new double[l0 + l1 - 1].Set(0.0);
        for (int i0 = 0; i0 < l0; i0++)
            for (int i1 = 0; i1 < l1; i1++)
                r[i0 + i1] += c0[i0] * c1[i1];
        return r;
    }

    /// <summary>
    /// Evaluate the supplied polynomial that is stored with ascending
    /// coefficients at the supplied agrument x:
    /// coeff[0] + coeff[1] x + coeff[2] x^2 + ... + coeff[n-1] x^(n-1)
    /// </summary>
    public static double Evaluate(this double[] coeff, double x)
    {
        int i = coeff.Length - 1;
        if (i < 0) throw new ArgumentException("Need at least 1 coefficient.", nameof(coeff));
        double value = coeff[i--];
        while (i >= 0) value = x * value + coeff[i--];
        return value;
    }

    /// <summary>
    /// Evaluate the derivative of the supplied polynomial that is stored
    /// with ascending coefficients at the supplied agrument x:
    /// coeff[1] + 2 coeff[2] x + ... + (n-1) coeff[n-1] x^(n-2)
    /// </summary>
    public static double EvaluateDerivative(this double[] coeff, double x)
    {
        int i = coeff.Length - 1;
        if (i < 0) throw new ArgumentException("Need at least 1 coefficient.", nameof(coeff));
        double value = i * coeff[i];
        --i;
        while (i > 0) { value = x * value + i * coeff[i]; --i; }
        return value;
    }

#if !TRAVIS_CI
    /// <summary>
    /// Return the real roots of the supplied polynomial, that is stored
    /// with ascending coefficients:
    /// coeff[0] + coeff[1] x + coeff[2] x^2 + ... + coeff[n-1] x^(n-1)
    /// Note that double and triple roots are returned as repeated values.
    /// WARNING: currently only polynomials up to the 4th order can be solved.
    /// </summary>
    public static double[] RealRoots(this double[] coeff)
    {
        return coeff.Length switch
        {
            0 or 1 => [],
            2 => RealRootOf(coeff[1], coeff[2]).NonNanToArray(),
            3 => RealRootsOf(coeff[2], coeff[1], coeff[0]).NonNanToArray(),
            4 => RealRootsOf(coeff[3], coeff[2], coeff[1], coeff[0]).NonNanToArray(),
            5 => RealRootsOf(coeff[4], coeff[3], coeff[2], coeff[1], coeff[0]).NonNanToArray(),
            _ => throw new NotImplementedException(),
        };
    }

    /// <summary>
    /// Return the real roots of the supplied normalized polynomial, that
    /// is stored with ascending coefficients:
    /// coeff[0] + coeff[1] x + coeff[2] x^2 + coeff[3] x^3 ... + x^n
    /// The last coefficient with value 1.0 must not be part of the
    /// supplied array.
    /// Note that double and triple roots are returned as repeated values.
    /// WARNING: currently only polynomials up to the 4th order can be solved.
    /// </summary>
    public static double[] RealRootsNormed(this double[] coeff)
    {
        return coeff.Length switch
        {
            0 => [],
            1 => RealRootOfNormed(coeff[0]).IntoArray(),
            2 => RealRootsOfNormed(coeff[1], coeff[0]).NonNanToArray(),
            3 => RealRootsOfNormed(coeff[2], coeff[1], coeff[0]).NonNanToArray(),
            4 => RealRootsOfNormed(coeff[3], coeff[2], coeff[1],coeff[0]).NonNanToArray(),
            _ => throw new NotImplementedException(),
        };
    }
#endif

    private static double[] NonNanToArray(this double root)
    {
        if (double.IsNaN(root)) return [];
        return [root];
    }

    private static double[] NonNanToArray(this (double, double) p)
    {
        if (double.IsNaN(p.Item1))
        {
            if (double.IsNaN(p.Item2)) return [];
            return [p.Item2];
        }
        if (double.IsNaN(p.Item2)) return [p.Item1];
        return [p.Item1, p.Item2];
    }

    private static double[] NonNanToArray(this (double, double, double) t)
    {
        int c = 0;
        bool v0 = !double.IsNaN(t.Item1); if (v0) c++;
        bool v1 = !double.IsNaN(t.Item2); if (v1) c++;
        bool v2 = !double.IsNaN(t.Item3); if (v2) c++;
        var a = new double[c];
        if (v2) a[--c] = t.Item3;
        if (v1) a[--c] = t.Item2;
        if (v0) a[--c] = t.Item1;
        return a;
    }

    private static double[] NonNanToArray(this (double, double, double, double) q)
    {
        int c = 0;
        bool v0 = !double.IsNaN(q.Item1); if (v0) c++;
        bool v1 = !double.IsNaN(q.Item2); if (v1) c++;
        bool v2 = !double.IsNaN(q.Item3); if (v2) c++;
        bool v3 = !double.IsNaN(q.Item4); if (v3) c++;
        var a = new double[c];
        if (v3) a[--c] = q.Item4;
        if (v2) a[--c] = q.Item3;
        if (v1) a[--c] = q.Item2;
        if (v0) a[--c] = q.Item1;
        return a;
    }

    /// <summary>
    /// Return root of the equation ax + b = 0.
    /// Returns NaN if no root exists (a = 0).
    /// </summary>
    public static double RealRootOf(double a, double b)
    {
        if (Fun.IsTiny(a)) return double.NaN;
        return -b / a;
    }

    /// <summary>
    /// Return -p as the root of x + p = 0.
    /// </summary>
    public static double RealRootOfNormed(double p)
    {
        return -p;
    }

    /// <summary>
    /// Return real roots of the equation ax^2 + bx + c = 0.
    /// Double roots are returned as a pair of identical values.
    /// If only one (linear) roots exists, it is stored in the first
    /// entry and the second entry is NaN.
    /// If no real roots exists, then both entries are NaN.
    /// </summary>
    public static (double, double) RealRootsOf(double a, double b, double c)
    {
        if (Fun.IsTiny(a))
        {
            if (Fun.IsTiny(b))
                return (double.NaN, double.NaN);
            return (-c / b, double.NaN);
        }
        var r = b * b - 4 * a * c;
        if (r < 0)
            return (double.NaN, double.NaN);
        if (b < 0)                                 // prevent cancellation
        {
            double d = -b + Fun.Sqrt(r);
            return (2 * c / d, d / (2 * a));
        }
        else
        {
            double d = -b - Fun.Sqrt(r);
            return (d / (2 * a), 2 * c / d);
        }
    }

    /// <summary>
    /// Return real roots of the equation x^2 + px + q = 0.
    /// Double roots are returned as a pair of identical values.
    /// If no real roots exists, then both entries are NaN.
    /// </summary>
    public static (double, double) RealRootsOfNormed(double p, double q)
    {
        double p2 = p / 2.0;
        double d = p2 * p2 - q;

        if (d < 0)
            return (double.NaN, double.NaN);
        if (p2 > 0.0)				               // prevent cancellation
        {
            double r = -(p2 + Fun.Sqrt(d));
            return (r, q / r);
        }
        else
        {
            double r = Fun.Sqrt(d) - p2;
            return (q / r, r);
        }
    }

    /// <summary>
    /// Return real roots of the equation: a x^3 + b x^2 + c x + d = 0.
    /// Double and triple solutions are returned as replicated values.
    /// Imaginary and non existing solutions are returned as NaNs.
    /// </summary>
    public static (double, double, double) RealRootsOf(
            double a, double b, double c, double d)
    {
        if (Fun.IsTiny(a))
        {
            var r = RealRootsOf(b, c, d);
            return (r.Item1, r.Item2, double.NaN);
        }
        return RealRootsOfNormed(b / a, c / a, d / a);
    }

    /// <summary>
    /// Return real roots of the equation: x^3 + c2 x^2 + c1 x + c0 = 0
    /// Double and triple solutions are returned as replicated values.
    /// Imaginary and non existing solutions are returned as NaNs.
    /// </summary>
    public static (double, double, double) RealRootsOfNormed(
            double c2, double c1, double c0)
    {
        // ------ eliminate quadric term (x = y - c2/3): x^3 + p x + q = 0
        double d = c2 * c2;
        double p3 = 1/3.0 * /* p */(-1/3.0 * d + c1);
        double q2 = 1/2.0 * /* q */((2/27.0 * d - 1/3.0 * c1) * c2 + c0);
        double p3c = p3 * p3 * p3;
        double shift = 1/3.0 * c2;
        d = q2 * q2 + p3c;
        if (d < 0)            // casus irreducibilis: three real solutions
        {
            double phi = 1 / 3.0 * Fun.Acos(-q2 / Fun.Sqrt(-p3c));
            double t = 2 * Fun.Sqrt(-p3);
            double r0 = t * Fun.Cos(phi) - shift;
            double r1 = -t * Fun.Cos(phi + Constant.Pi / 3.0) - shift;
            double r2 = -t * Fun.Cos(phi - Constant.Pi / 3.0) - shift;
            return TupleExtensions.CreateAscending(r0, r1, r2);
        }
        // else if (Fun.IsTiny(q2))			           // one triple root
        // {                                           // too unlikely for
        //     double r = -1/3.0 * c2;                 // special handling
        //     return (r, r, r);                       // to pay off
        // }
        d = Fun.Sqrt(d);                 // one single and one double root
        double uav = Fun.Cbrt(d - q2) - Fun.Cbrt(d + q2);
        double s0 = uav - shift, s1 = -0.5 * uav - shift;
        return s0 < s1  ? (s0, s1, s1) : (s1, s1, s0);
    }

    /// <summary>
    /// Return real roots of the equation: x^3 + p x + q = 0
    /// Double and triple solutions are returned as replicated values.
    /// Imaginary and non existing solutions are returned as NaNs.
    /// </summary>
    public static (double, double, double) RealRootsOfDepressed(
            double p, double q)
    {
        double p3 = 1/3.0 * p, q2 = 1/2.0 * q;
        double p3c = p3 * p3 * p3, d = q2 * q2 + p3c;
        if (d < 0) // ---------- casus irreducibilis: three real solutions
        {
            double phi = 1 / 3.0 * Fun.Acos(-q2 / Fun.Sqrt(-p3c));
            double t = 2 * Fun.Sqrt(-p3);
            double r0 = t * Fun.Cos(phi);
            double r1 = -t * Fun.Cos(phi + Constant.Pi / 3.0);
            double r2 = -t * Fun.Cos(phi - Constant.Pi / 3.0);
            return TupleExtensions.CreateAscending(r0, r1, r2);
        }
        d = Fun.Sqrt(d);  // one triple root or a single and a double root
        double s0 = Fun.Cbrt(d - q2) - Fun.Cbrt(d + q2);
        double s1 = -0.5 * s0;
        return s0 < s1  ? (s0, s1, s1) : (s1, s1, s0);
    }

    /// <summary>
    /// One real root of the equation: x^3 + c2 x^2 + c1 x + c0 = 0.
    /// </summary>
    public static double OneRealRootOfNormed(
        double c2, double c1, double c0
        )
    {
        // ------ eliminate quadric term (x = y - c2/3): x^3 + p x + q = 0
        double d = c2 * c2;
        double p3 = 1/3.0 * /* p */(-1/3.0 * d + c1);
        double q2 = 1/2.0 * /* q */((2/27.0 * d - 1/3.0 * c1) * c2 + c0);
        double p3c = p3 * p3 * p3;
        d = q2 * q2 + p3c;
        if (d < 0) // -------------- casus irreducibilis: three real roots
            return 2 * Fun.Sqrt(-p3) * Fun.Cos(1/3.0
                          * Fun.Acos(-q2 / Fun.Sqrt(-p3c))) - 1/3.0 * c2;
        d = Fun.Sqrt(d);  // one triple root or a single and a double root
        return Fun.Cbrt(d - q2) - Fun.Cbrt(d + q2) - 1/3.0 * c2;
    }
#if !TRAVIS_CI
    /// <summary>
    /// Return real roots of: a x^4 + b x^3 + c x^2 + d x + e = 0
    /// Double and triple solutions are returned as replicated values.
    /// Imaginary and non existing solutions are returned as NaNs.
    /// </summary>
    public static (double, double, double, double) RealRootsOf(
            double a, double b, double c, double d, double e)
    {
        if (Fun.IsTiny(a))
        {
            var r = RealRootsOf(b, c, d, e);
            return (r.Item1, r.Item2, r.Item3, double.NaN);
        }
        return RealRootsOfNormed(b / a, c / a, d / a, e / a);
    }

    /// <summary>
    /// Return real roots of: x^4 + c3 x^3 + c2 x^2 + c1 x + c0 = 0.
    /// Double and triple solutions are returned as replicated values.
    /// Imaginary and non existing solutions are returned as NaNs.
    /// </summary>
    public static (double, double, double, double) RealRootsOfNormed(
            double c3, double c2, double c1, double c0)
    {
        // eliminate cubic term (x = y - c3/4):  x^4 + p x^2 + q x + r = 0
        double e = c3 * c3;
        double p = -3/8.0 * e + c2;
        double q = (1/8.0 * e - 1/2.0 * c2) * c3 + c1;
        double r = (1/16.0 * c2 - 3/256.0 * e) * e - 1/4.0 * c3 * c1 + c0;

        if (Fun.IsTiny(r)) // ---- no absolute term: y (y^3 + p y + q) = 0
            return MergeSortedAndShift(
                        RealRootsOfDepressed(p, q), 0.0, -1/4.0 * c3);
        // ----------------------- take one root of the resolvent cubic...
        double z = OneRealRootOfNormed(
                        -1/2.0 * p, -r, 1/2.0 * r * p - 1/8.0 * q * q);
        // --------------------------- ...to build two quadratic equations
        double u = z * z - r;
        double v = 2.0 * z - p;
        if (u < Constant<double>.PositiveTinyValue) // +tiny instead of 0
            u = 0.0;                                // improves unique
        else                                        // root accuracy by a
            u = Fun.Sqrt(u);                        // factor of 10^5!
        if (v < Constant<double>.PositiveTinyValue) // values greater than
            v = 0.0;                                // +tiny == 4 * eps
        else                                        // do not seem to
            v = Fun.Sqrt(v);                        // improve accuraccy!
        double q1 = q < 0 ? -v : v;
        return MergeSortedAndShift(RealRootsOfNormed(q1, z - u),
                           RealRootsOfNormed(-q1, z + u),
                           -1/4.0 * c3);
    }

    static (double, double, double, double) MergeSortedAndShift(
            (double, double, double) t, double d, double shift)
    {
        var q = (0.0, 0.0, 0.0, 0.0);
        int tc = t.CountNonNaNs();
        int i = 0, ti = 0;
        while (ti < tc)
        {
            if (t.Get(ti) < d)
                q.Set(i++, t.Get(ti++) + shift);
            else
            {
                q.Set(i++, d + shift);
                break;
            }
        }
        while (ti < tc) q.Set(i++, t.Get(ti++) + shift);
        while (i < 4) q.Set(i++, double.NaN);
        return q;
    }

    static (double, double, double, double) MergeSortedAndShift(
            (double, double) p0, (double, double) p1, double shift)
    {
        var q = (0.0, 0.0, 0.0, 0.0);
        int c0 = p0.CountNonNaNs();
        int c1 = p1.CountNonNaNs();
        int i = 0, i0 = 0, i1 = 0;
        while (i0 < c0 && i1 < c1)
        {
            if (p0.Get(i0) < p1.Get(i1))
                q.Set(i++, p0.Get(i0++) + shift);
            else
                q.Set(i++, p1.Get(i1++) + shift);
        }
        while (i0 < c0) q.Set(i++, p0.Get(i0++) + shift);
        while (i1 < c1) q.Set(i++, p1.Get(i1++) + shift);
        while (i < 4) q.Set(i++, double.NaN);
        return q;
    }
#endif

    /// <summary>
    /// Returns a copy of an array of roots without any double roots with
    /// an absolute difference that is smaller than the supplied epsilon.
    /// Roots with odd multiplicity will be left as single roots.
    /// </summary>
    public static double[] WithoutDoubleRoots(
            this double[] a, double epsilon)
    {
        int last = a.Length - 1;
        if (last < 4) return WithoutDoubleRoots4(a, epsilon);
        var r = new List<double>(last + 1);
        int i = 0;
        while (i < last)
        {
            int j = i + 1;
            if (Fun.Abs(a[i] - a[j]) < epsilon) { i += 2; continue; }
            r.Add(a[i]);
            i = j;
        }
        if (i == last) r.Add(a[i]);
        return [.. r];
    }

    private static double[] WithoutDoubleRoots4(double[] a, double eps)
    {
        int len = a.Length;
        if (len < 2) return a;
        if ((a[0] - a[1]).Abs() < eps)
        {
            if (len < 3) return [];
            if (len < 4) return [a[2]];
            if ((a[2] - a[3]).Abs() < eps)
                return [];
            else
                return [a[2], a[3]];
        }
        else
        {
            if (len < 3) return a;
            if ((a[1] - a[2]).Abs() < eps)
            {
                if (len < 4) return [a[0]];
                return [a[0], a[3]];
            }
            else
            {
                if (len < 4) return a;
                if ((a[2] - a[3]).Abs() < eps)
                    return [a[0], a[1]];
                else
                    return a;
            }
        }
    }
}
