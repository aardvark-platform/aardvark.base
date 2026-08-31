using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents a fraction as a serialized pair of 64-bit numerator and denominator fields.
    /// Finite equality and ordering are mathematically exact; ordinary comparisons use their
    /// rounded <see cref="Value"/> only as a fast ordering test. A zero denominator represents
    /// NaN when the numerator is zero and signed infinity otherwise. Comparison operators follow
    /// IEEE NaN semantics, while <see cref="Equals(Fraction)"/> groups NaNs for collections.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Fraction : IEquatable<Fraction>
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

        #region Exact helpers

        private const ulong LongMinMagnitude = 0x8000000000000000UL;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong UnsignedMagnitude(long value)
            => value < 0 ? unchecked(0UL - (ulong)value) : (ulong)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FiniteSign(long numerator, long denominator)
        {
            if (numerator == 0) return 0;
            return (numerator < 0) == (denominator < 0) ? 1 : -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong GreatestCommonDivisor(ulong a, ulong b)
        {
            while (b != 0)
            {
                ulong remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }

        private static int ComparePositive(ulong aNumerator, ulong aDenominator, ulong bNumerator, ulong bDenominator)
        {
            bool reverse = false;
            while (true)
            {
                ulong aQuotient = aNumerator / aDenominator;
                ulong bQuotient = bNumerator / bDenominator;
                if (aQuotient != bQuotient)
                {
                    int result = aQuotient < bQuotient ? -1 : 1;
                    return reverse ? -result : result;
                }

                ulong aRemainder = aNumerator % aDenominator;
                ulong bRemainder = bNumerator % bDenominator;
                if (aRemainder == 0 || bRemainder == 0)
                {
                    if (aRemainder == bRemainder) return 0;
                    int result = aRemainder == 0 ? -1 : 1;
                    return reverse ? -result : result;
                }

                aNumerator = aDenominator;
                aDenominator = aRemainder;
                bNumerator = bDenominator;
                bDenominator = bRemainder;
                reverse = !reverse;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int CompareFiniteExact(Fraction a, Fraction b)
        {
            int aSign = FiniteSign(a.Numerator, a.Denominator);
            int bSign = FiniteSign(b.Numerator, b.Denominator);
            if (aSign != bSign) return aSign < bSign ? -1 : 1;
            if (aSign == 0) return 0;

            int result = ComparePositive(
                UnsignedMagnitude(a.Numerator), UnsignedMagnitude(a.Denominator),
                UnsignedMagnitude(b.Numerator), UnsignedMagnitude(b.Denominator));
            return aSign < 0 ? -result : result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int CompareRoundedTie(Fraction a, Fraction b)
        {
            if (a.Numerator == b.Numerator && a.Denominator == b.Denominator) return 0;
            if (a.Denominator == 0 || b.Denominator == 0) return 0;
            return CompareFiniteExact(a, b);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool RoundedTieIsLess(Fraction a, Fraction b) => CompareRoundedTie(a, b) < 0;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool RoundedTieIsLessOrEqual(Fraction a, Fraction b) => CompareRoundedTie(a, b) <= 0;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool RoundedTieIsEqual(Fraction a, Fraction b) => CompareRoundedTie(a, b) == 0;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool RoundedTieIsGreaterOrEqual(Fraction a, Fraction b) => CompareRoundedTie(a, b) >= 0;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool RoundedTieIsGreater(Fraction a, Fraction b) => CompareRoundedTie(a, b) > 0;

        private static Fraction CreateReducedFinite(long numerator, long denominator)
        {
            ulong numeratorMagnitude = UnsignedMagnitude(numerator);
            ulong denominatorMagnitude = UnsignedMagnitude(denominator);
            ulong gcd = GreatestCommonDivisor(numeratorMagnitude, denominatorMagnitude);
            numeratorMagnitude /= gcd;
            denominatorMagnitude /= gcd;

            int sign = FiniteSign(numerator, denominator);
            bool denominatorIsNegative = denominatorMagnitude == LongMinMagnitude ||
                                         (numeratorMagnitude == LongMinMagnitude && sign > 0);

            long reducedDenominator = denominatorIsNegative
                ? denominatorMagnitude == LongMinMagnitude ? long.MinValue : -(long)denominatorMagnitude
                : (long)denominatorMagnitude;

            int rawNumeratorSign = denominatorIsNegative ? -sign : sign;
            long reducedNumerator = rawNumeratorSign < 0
                ? numeratorMagnitude == LongMinMagnitude ? long.MinValue : -(long)numeratorMagnitude
                : (long)numeratorMagnitude;

            return new Fraction
            {
                Numerator = reducedNumerator,
                Denominator = reducedDenominator,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Fraction CanonicalInfinity(Fraction value)
            => value.Numerator < 0 ? NegativeInfinity : PositiveInfinity;

        private static Fraction AddNonFinite(Fraction a, Fraction b)
        {
            if (IsNaN(a) || IsNaN(b)) return NaN;
            if (IsInfinity(a) && IsInfinity(b))
                return (a.Numerator < 0) == (b.Numerator < 0) ? CanonicalInfinity(a) : NaN;
            return IsInfinity(a) ? CanonicalInfinity(a) : CanonicalInfinity(b);
        }

        #endregion

        #region Properties

        public readonly double Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (double)Numerator / Denominator; }
        }

        /// <summary>
        /// Gets the numerically equivalent fraction in lowest terms. NaN and signed infinities
        /// are returned in canonical <c>0/0</c>, <c>-1/0</c>, or <c>1/0</c> form.
        /// </summary>
        public readonly Fraction Reduced
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (Denominator == 0)
                    return Numerator == 0 ? NaN : CanonicalInfinity(this);
                return CreateReducedFinite(Numerator, Denominator);
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
            if (a.Denominator == 0)
                return IsNaN(a) ? NaN : a.Numerator < 0 ? PositiveInfinity : NegativeInfinity;
            return new Fraction(-a.Numerator, a.Denominator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fraction operator +(Fraction a, Fraction b)
        {
            if (a.Denominator == 0 || b.Denominator == 0)
                return AddNonFinite(a, b);

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
            double aValue = a.Value;
            double bValue = b.Value;
            return aValue < bValue || aValue == bValue && RoundedTieIsLess(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fraction a, Fraction b)
        {
            double aValue = a.Value;
            double bValue = b.Value;
            return aValue < bValue || aValue == bValue && RoundedTieIsLessOrEqual(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fraction a, Fraction b)
        {
            double aValue = a.Value;
            double bValue = b.Value;
            return aValue == bValue && RoundedTieIsEqual(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fraction a, Fraction b)
        {
            double aValue = a.Value;
            double bValue = b.Value;
            return aValue > bValue || aValue == bValue && RoundedTieIsGreaterOrEqual(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fraction a, Fraction b)
        {
            double aValue = a.Value;
            double bValue = b.Value;
            return aValue > bValue || aValue == bValue && RoundedTieIsGreater(a, b);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets a <see cref="Fraction"/> that evaluates to 0.
        /// </summary>
        public static Fraction Zero => new Fraction(0, 1);

        /// <summary>
        /// Gets a <see cref="Fraction"/> that evaluates to 1.
        /// </summary>
        public static Fraction One => new Fraction(1, 1);

        /// <summary>
        /// Gets the smallest positive <see cref="Fraction"/> value greater than zero.
        /// </summary>
        public static Fraction Epsilon => new Fraction(1, long.MaxValue);

        /// <summary>
        /// Gets the smallest possible value of a <see cref="Fraction"/>.
        /// </summary>
        public static Fraction MinValue => new Fraction(long.MinValue, 1);

        /// <summary>
        /// Gets the largest possible value of a <see cref="Fraction"/>.
        /// </summary>
        public static Fraction MaxValue => new Fraction(long.MaxValue, 1);

        /// <summary>
        /// Gets a value that is not a number (NaN).
        /// </summary>
        public static Fraction NaN => new Fraction(0, 0);

        /// <summary>
        /// Represents negative infinity.
        /// </summary>
        public static Fraction NegativeInfinity => new Fraction(-1, 0);
       
        /// <summary>
        /// Represents positive infinity.
        /// </summary>
        public static Fraction PositiveInfinity => new Fraction(+1, 0);

        #endregion

        /// <summary>
        /// Returns whether the specified <see cref="Fraction"/> has a zero denominator
        /// and nonzero numerator and therefore evaluates to negative or positive infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Fraction f)
        {
            return f.Denominator == 0 && f.Numerator != 0;
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
        /// Returns whether the specified <see cref="Fraction"/> has both numerator and
        /// denominator equal to zero and therefore evaluates to <see cref="NaN"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(Fraction f)
        {
            return f.Denominator == 0 && f.Numerator == 0;
        }

        /// <summary>
        /// Tests numerical equality for finite values and signed infinities. Unlike <c>==</c>,
        /// this method treats NaN values as equal for collection semantics.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Fraction other)
        {
            if (Numerator == other.Numerator && Denominator == other.Denominator) return true;
            if (IsNaN(this) || IsNaN(other)) return IsNaN(this) && IsNaN(other);

            double value = Value;
            double otherValue = other.Value;
            if (value != otherValue) return false;
            if (Denominator == 0 || other.Denominator == 0) return true;
            return CompareFiniteExact(this, other) == 0;
        }

        public override readonly bool Equals(object obj)
            => obj is Fraction other && Equals(other);

        public override readonly int GetHashCode()
        {
            if (IsNaN(this)) return 0x7fc00000;
            if (IsPositiveInfinity(this)) return 0x7ff00000;
            if (IsNegativeInfinity(this)) return unchecked((int)0xfff00000);

            ulong numerator = UnsignedMagnitude(Numerator);
            ulong denominator = UnsignedMagnitude(Denominator);
            ulong gcd = GreatestCommonDivisor(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;

            unchecked
            {
                int hash = 17;
                hash = hash * 31 + FiniteSign(Numerator, Denominator);
                hash = hash * 31 + (int)numerator;
                hash = hash * 31 + (int)(numerator >> 32);
                hash = hash * 31 + (int)denominator;
                hash = hash * 31 + (int)(denominator >> 32);
                return hash;
            }
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
                long.Parse(s.Substring(0, sep)),
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
}
