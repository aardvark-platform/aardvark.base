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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base;

/// <summary>
/// Represents an integral fraction.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public struct Fraction
{
    [DataMember]
    public long Numerator;
    [DataMember]
    public long Denominator;

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Fraction(long value)
    {
        Numerator = value;
        Denominator = 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Fraction(long numerator, long denominator)
    {
        // ensure positive denominator
        Numerator = denominator < 0 ? -numerator : numerator;
        Denominator = Fun.Abs(denominator);
    }

    #endregion

    #region Properties

    public readonly double Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return (double)Numerator / Denominator; }
    }

    public readonly Fraction Reduced
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            long gcd = Fun.GreatestCommonDivisor(Numerator, Denominator);
            return new Fraction(Numerator / gcd, Denominator / gcd);
        }
    }

    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fraction operator +(Fraction a)
    {
        return a;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fraction operator -(Fraction a)
    {
        return new Fraction(-a.Numerator, a.Denominator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fraction operator +(Fraction a, Fraction b)
    {
        long gcd = Fun.GreatestCommonDivisor(a.Denominator, b.Denominator);
        long aDenomDivGcd = a.Denominator / gcd;

        return new Fraction(
            a.Numerator * (b.Denominator / gcd)
            + b.Numerator * aDenomDivGcd,
            aDenomDivGcd * b.Denominator
            );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fraction operator -(Fraction a, Fraction b)
    {
        return a + (-b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fraction operator *(Fraction a, Fraction b)
    {
        return new Fraction(
            a.Numerator * b.Numerator,
            a.Denominator * b.Denominator
            );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fraction operator /(Fraction a, Fraction b)
    {
        if (b.Numerator < 0)        // ensure positive denominator
            return new Fraction(
                -a.Numerator * b.Denominator,
                -b.Numerator * a.Denominator
                );

        return new Fraction(
            a.Numerator * b.Denominator,
            a.Denominator * b.Numerator
            );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Fraction a, Fraction b)
    {
        return a.Value < b.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Fraction a, Fraction b)
    {
        return a.Value <= b.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Fraction a, Fraction b)
    {
        return a.Value == b.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Fraction a, Fraction b)
    {
        return a.Value != b.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Fraction a, Fraction b)
    {
        return a.Value >= b.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Fraction a, Fraction b)
    {
        return a.Value > b.Value;
    }

    #endregion

    #region Constants

    /// <summary>
    /// Gets a <see cref="Fraction"/> that evaluates to 0.
    /// </summary>
    public static Fraction Zero => new(0, 1);

    /// <summary>
    /// Gets a <see cref="Fraction"/> that evaluates to 1.
    /// </summary>
    public static Fraction One => new(1, 1);

    /// <summary>
    /// Gets the smallest positive <see cref="Fraction"/> value greater than zero.
    /// </summary>
    public static Fraction Epsilon => new(1, long.MaxValue);

    /// <summary>
    /// Gets the smallest possible value of a <see cref="Fraction"/>.
    /// </summary>
    public static Fraction MinValue => new(long.MinValue, 1);

    /// <summary>
    /// Gets the largest possible value of a <see cref="Fraction"/>.
    /// </summary>
    public static Fraction MaxValue => new(long.MaxValue, 1);

    /// <summary>
    /// Gets a value that is not a number (NaN).
    /// </summary>
    public static Fraction NaN => new(0, 0);

    /// <summary>
    /// Represents negative infinity.
    /// </summary>
    public static Fraction NegativeInfinity => new(-1, 0);
   
    /// <summary>
    /// Represents positive infinity.
    /// </summary>
    public static Fraction PositiveInfinity => new(+1, 0);

    #endregion

    /// <summary>
    /// Returns whether the specified <see cref="Fraction"/>
    /// evaluates to negative or positive infinity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInfinity(Fraction f)
    {
        return f.Denominator == 0;
    }

    /// <summary>
    /// Returns whether the specified <see cref="Fraction"/>
    /// evaluates to negative infinity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegativeInfinity(Fraction f)
    {
        return f.Denominator == 0 && f.Numerator < 0;
    }

    /// <summary>
    /// Returns whether the specified <see cref="Fraction"/>
    /// evaluates to positive infinity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPositiveInfinity(Fraction f)
    {
        return f.Denominator == 0 && f.Numerator > 0;
    }

    /// <summary>
    /// Returns whether the specified <see cref="Fraction"/>
    /// evaluates to a value that is not a number (Fraction.NaN).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(Fraction f)
    {
        return f.Denominator == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Fraction other)
        => Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);

    public override readonly bool Equals(object obj)
        => (obj is Fraction o) && Equals(o);

    public override readonly int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override readonly string ToString()
    {
        return Numerator + "/" + Denominator;
    }

    public static Fraction Parse(string s)
    {
        int sep = s.IndexOf('/');
        if (sep < 0) return new Fraction(long.Parse(s));
        return new Fraction(
#if NETSTANDARD2_0
            long.Parse(s.Substring(0, sep)),
#else
            long.Parse(s[..sep]),
#endif
            long.Parse(s.Substring(sep+1, s.Length-sep-1))
            );
    }
}

public static partial class Fun
{
    #region ApproximateEquals

    /// <summary>
    /// Returns whether the given <see cref="Fraction"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Fraction a, Fraction b, double tolerance)
        => ApproximateEquals(a.Value, b.Value, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Fraction"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Fraction a, Fraction b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);

    #endregion
}
