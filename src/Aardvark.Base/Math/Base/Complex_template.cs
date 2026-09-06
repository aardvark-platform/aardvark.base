using System;
using System.Linq;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    //# var signedtypes = Meta.SignedTypes;
    //# var numtypes = Meta.StandardNumericTypes;
    //# var dreptypes = Meta.DoubleRepresentableTypes;
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? Meta.DoubleType : Meta.FloatType;
    //#   var ftype2 = isDouble ? Meta.FloatType : Meta.DoubleType;
    //#   var ft = ftype.Name;
    //#   var ft2 = ftype2.Name;
    //#   var ct = isDouble ? "ComplexD" : "ComplexF";
    //#   var ct2 = isDouble ? "ComplexF" : "ComplexD";
    //#   var constant = isDouble ? "Constant" : "ConstantF";
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var minNormal = isDouble ? "2.2250738585072014e-308" : "1.17549435e-38f";
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __ct__ : IEquatable<__ct__>
    {
        [DataMember]
        public __ft__ Real;
        [DataMember]
        public __ft__ Imag;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="__ct__"/> from a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ct__(__ft__ real)
        {
            Real = real;
            Imag = 0;
        }

        /// <summary>
        /// Constructs a <see cref="__ct__"/> from a real and an imaginary part.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ct__(__ft__ real, __ft__ imag)
        {
            Real = real;
            Imag = imag;
        }

        /// <summary>
        /// Constructs a <see cref="__ct__"/> from a <see cref="__ct2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __ct__(__ct2__ complex)
        {
            Real = (__ft__)complex.Real;
            Imag = (__ft__)complex.Imag;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Returns 0 + 0i.
        /// </summary>
        public static __ct__ Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __ct__(0, 0);
        }

        /// <summary>
        /// Returns 1 + 0i.
        /// </summary>
        public static __ct__ One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __ct__(1, 0);
        }

        /// <summary>
        /// Returns 0 + 1i.
        /// </summary>
        public static __ct__ I
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __ct__(0, 1);
        }

        /// <summary>
        /// Returns ∞ + 0i.
        /// </summary>
        public static __ct__ PositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __ct__(__ft__.PositiveInfinity);
        }

        /// <summary>
        /// Returns -∞ + 0i.
        /// </summary>
        public static __ct__ NegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __ct__(__ft__.NegativeInfinity);
        }

        /// <summary>
        /// Returns 0 + ∞i.
        /// </summary>
        public static __ct__ PositiveInfinityI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __ct__(0, __ft__.PositiveInfinity);
        }

        /// <summary>
        /// Returns 0 - ∞i.
        /// </summary>
        public static __ct__ NegativeInfinityI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __ct__(0, __ft__.NegativeInfinity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the conjugated of the complex number.
        /// </summary>
        public readonly __ct__ Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __ct__(Real, -Imag); }
        }

        /// <summary>
        /// Returns the reciprocal of the complex number.
        /// </summary>
        public readonly __ct__ Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                __ft__ normSquared = Fun.MultiplyAdd(Real, Real, Imag * Imag);
                __ft__ t = 1 / normSquared;
                var result = new __ct__(Real * t, -Imag * t);

                if (IsNormalValue(normSquared))
                    return result;

                return GetScaledReciprocal(this, result);
            }
        }

        /// <summary>
        /// Returns the squared Gaussian Norm (modulus) of the complex number.
        /// </summary>
        public readonly __ft__ NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Returns the Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [XmlIgnore]
        public __ft__ Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                __ft__ squared = Fun.MultiplyAdd(Real, Real, Imag * Imag);
                if (IsNormalValue(squared) || (squared == 0 && Real == 0 && Imag == 0))
                    return Fun.Sqrt(squared);

                return GetScaledNorm(Real, Imag, squared);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                __ft__ r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsFiniteValue(__ft__ value)
        {
        //# if (isDouble) {
            ulong bits = (ulong)Fun.FloatToBits(value) & 0x7fffffffffffffffUL;
            return bits < 0x7ff0000000000000UL;
        //# } else {
            uint bits = (uint)Fun.FloatToBits(value) & 0x7fffffffU;
            return bits < 0x7f800000U;
        //# }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNormalValue(__ft__ value)
        {
        //# if (isDouble) {
            ulong bits = (ulong)Fun.FloatToBits(value) & 0x7fffffffffffffffUL;
            return bits - 0x0010000000000000UL < 0x7ff0000000000000UL - 0x0010000000000000UL;
        //# } else {
            uint bits = (uint)Fun.FloatToBits(value) & 0x7fffffffU;
            return bits - 0x00800000U < 0x7f800000U - 0x00800000U;
        //# }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static __ft__ GetScaledNorm(__ft__ real, __ft__ imag, __ft__ squared)
        {
            if (!IsFiniteValue(real) || !IsFiniteValue(imag))
                return Fun.Sqrt(squared);

            __ft__ ar = Fun.Abs(real);
            __ft__ ai = Fun.Abs(imag);
            __ft__ max = Fun.Max(ar, ai);
            __ft__ min = Fun.Min(ar, ai);
            __ft__ ratio = min / max;
            return max * Fun.Sqrt(1 + ratio * ratio);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static __ct__ GetScaledReciprocal(__ct__ value, __ct__ direct)
        {
            if (!IsFiniteValue(value.Real) || !IsFiniteValue(value.Imag))
                return direct;

            __ft__ scale = Fun.Max(Fun.Abs(value.Real), Fun.Abs(value.Imag));
            if (scale == 0)
                return direct;

            __ft__ real = value.Real / scale;
            __ft__ imag = value.Imag / scale;
            __ft__ denominator = real * real + imag * imag;
            return new __ct__(
                ScaleQuotient(real / denominator, 1, scale),
                ScaleQuotient(-imag / denominator, 1, scale));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static __ct__ GetScaledDivision(__ct__ numerator, __ct__ denominator, __ct__ direct)
        {
            if (!IsFiniteValue(numerator.Real) || !IsFiniteValue(numerator.Imag) ||
                !IsFiniteValue(denominator.Real) || !IsFiniteValue(denominator.Imag))
                return direct;

            __ft__ numeratorScale = Fun.Max(Fun.Abs(numerator.Real), Fun.Abs(numerator.Imag));
            __ft__ denominatorScale = Fun.Max(Fun.Abs(denominator.Real), Fun.Abs(denominator.Imag));
            if (numeratorScale == 0 || denominatorScale == 0)
                return direct;

            __ft__ ar = numerator.Real / numeratorScale;
            __ft__ ai = numerator.Imag / numeratorScale;
            __ft__ br = denominator.Real / denominatorScale;
            __ft__ bi = denominator.Imag / denominatorScale;
            __ft__ scaledDenominator = br * br + bi * bi;
            __ft__ real = (ar * br + ai * bi) / scaledDenominator;
            __ft__ imag = (ai * br - ar * bi) / scaledDenominator;

            return new __ct__(
                ScaleQuotient(real, numeratorScale, denominatorScale),
                ScaleQuotient(imag, numeratorScale, denominatorScale));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static __ft__ ScaleQuotient(__ft__ value, __ft__ numeratorScale, __ft__ denominatorScale)
        {
            if (value == 0)
                return value;

            __ft__ product = value * numeratorScale;
            if (product != 0 && IsFiniteValue(product))
                return product / denominatorScale;

            __ft__ quotient = value / denominatorScale;
            if (quotient != 0 && IsFiniteValue(quotient))
                return quotient * numeratorScale;

            return value * (numeratorScale / denominatorScale);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static __ct__ GetScaledSquareRoot(__ct__ value, __ct__ direct)
        {
            if (!IsFiniteValue(value.Real) || !IsFiniteValue(value.Imag))
                return direct;

            __ft__ scale = Fun.Max(Fun.Abs(value.Real), Fun.Abs(value.Imag));
            __ft__ real = value.Real / scale;
            __ft__ imag = value.Imag / scale;
            __ft__ norm = Fun.Sqrt(real * real + imag * imag);
            __ft__ component = Fun.Sqrt((norm + Fun.Abs(real)) * __half__) * Fun.Sqrt(scale);

            return value.Real >= 0
                ? new __ct__(component, value.Imag / (2 * component))
                : new __ct__(Fun.Abs(value.Imag) / (2 * component), Fun.CopySign(component, value.Imag));
        }

        /// <summary>
        /// Retruns the argument of the complex number.
        /// </summary>
        [XmlIgnore]
        public __ft__ Argument
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get { return Fun.Atan2(Imag, Real); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                __ft__ r = Norm;

                Real = r * Fun.Cos(value);
                Imag = r * Fun.Sin(value);
            }
        }

        /// <summary>
        /// Returns whether the complex number has no imaginary part.
        /// </summary>
        public readonly bool IsReal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number has no real part.
        /// </summary>
        public readonly bool IsImaginary
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is 1 + 0i.
        /// </summary>
        public readonly bool IsOne
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.ApproximateEquals(1) && Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is zero.
        /// </summary>
        public readonly bool IsZero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny() && Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is 0 + 1i.
        /// </summary>
        public readonly bool IsI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny(Real) && Imag.ApproximateEquals(1); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is NaN.
        /// </summary>
        public readonly bool IsNaN
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (__ft__.IsNaN(Real) || __ft__.IsNaN(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite (positive or negative).
        /// </summary>
        public readonly bool IsInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (__ft__.IsInfinity(Real) || __ft__.IsInfinity(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite and positive.
        /// </summary>
        public readonly bool IsPositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (__ft__.IsPositiveInfinity(Real) || __ft__.IsPositiveInfinity(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite and negative.
        /// </summary>
        public readonly bool IsNegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (__ft__.IsNegativeInfinity(Real) || __ft__.IsNegativeInfinity(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number is finite (i.e. not NaN and not infinity).
        /// </summary>
        public readonly bool IsFinite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return !(IsNaN || IsInfinity); }
        }

        #endregion

        #region Static factories

        /// <summary>
        /// Creates a Radial Complex
        /// </summary>
        /// <param name="r">Norm of the complex number</param>
        /// <param name="phi">Argument of the complex number</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ CreateRadial(__ft__ r, __ft__ phi)
            => new __ct__(r * Fun.Cos(phi), r * Fun.Sin(phi));

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ CreateOrthogonal(__ft__ real, __ft__ imag)
            => new __ct__(real, imag);

        #endregion

        #region Static methods for F# core and Aardvark library support

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Acos(__ct__ x)
            => x.Acos();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Acoshb(__ct__ x)
            => x.Acosh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Cos(__ct__ x)
            => x.Cos();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Cosh(__ct__ x)
            => x.Cosh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Asin(__ct__ x)
            => x.Asin();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Asinhb(__ct__ x)
            => x.Asinh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sin(__ct__ x)
            => x.Sin();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sinh(__ct__ x)
            => x.Sinh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Atan(__ct__ x)
            => x.Atan();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Atanhb(__ct__ x)
            => x.Atanh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Tan(__ct__ x)
            => x.Tan();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Tanh(__ct__ x)
            => x.Tanh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sqrt(__ct__ x)
            => x.Sqrt();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ CubeRoot(__ct__ x)
            => x.Cbrt();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Exp(__ct__ x)
            => x.Exp();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log(__ct__ x)
            => x.Log();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ LogBinary(__ct__ x)
            => x.Log2();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log10(__ct__ x)
            => x.Log10();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ DivideByInt(__ct__ x, int y)
            => x / y;

        #endregion

        #region Operators

        /// <summary>
        /// Conversion from a <see cref="__ct__"/> to a <see cref="__ct2__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __ct2__(__ct__ c)
            => new __ct2__(c);

        /// <summary>
        /// Implicit conversion from a <see cref="__ft__"/> to a <see cref="__ct__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator __ct__(__ft__ a)
            => new __ct__(a);

        /// <summary>
        /// Adds two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator +(__ct__ a, __ct__ b)
            => new __ct__(a.Real + b.Real, a.Imag + b.Imag);

        /// <summary>
        /// Adds a complex number and a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator +(__ct__ a, __ft__ b)
            => new __ct__(a.Real + b, a.Imag);

        /// <summary>
        /// Adds a real number and a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator +(__ft__ a, __ct__ b)
            => new __ct__(a + b.Real, b.Imag);

        /// <summary>
        /// Subtracts two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator -(__ct__ a, __ct__ b)
            => new __ct__(a.Real - b.Real, a.Imag - b.Imag);

        /// <summary>
        /// Subtracts a real number from a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator -(__ct__ a, __ft__ b)
            => new __ct__(a.Real - b, a.Imag);

        /// <summary>
        /// Subtracts a complex number from a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator -(__ft__ a, __ct__ b)
            => new __ct__(a - b.Real, -b.Imag);

        /// <summary>
        /// Multiplies two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator *(__ct__ a, __ct__ b)
            => new __ct__(
                a.Real * b.Real - a.Imag * b.Imag,
                a.Real * b.Imag + a.Imag * b.Real);

        /// <summary>
        /// Multiplies a complex number and a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator *(__ct__ a, __ft__ b)
            => new __ct__(a.Real * b, a.Imag * b);

        /// <summary>
        /// Multiplies a real number and a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator *(__ft__ a, __ct__ b)
            => new __ct__(a * b.Real, a * b.Imag);

        /// <summary>
        /// Divides two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator /(__ct__ a, __ct__ b)
        {
            __ft__ normSquared = Fun.MultiplyAdd(b.Real, b.Real, b.Imag * b.Imag);
            __ft__ t = 1 / normSquared;
            __ft__ real = b.Real * t;
            __ft__ imag = b.Imag * t;
            var result = new __ct__(
                Fun.MultiplyAdd(a.Real, real, a.Imag * imag),
                Fun.MultiplyAdd(a.Imag, real, -a.Real * imag));

            if (IsNormalValue(normSquared))
                return result;

            return GetScaledDivision(a, b, result);
        }

        /// <summary>
        /// Divides a complex number by a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator /(__ct__ a, __ft__ b)
            => new __ct__(a.Real / b, a.Imag / b);

        /// <summary>
        /// Divides a real number by a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator /(__ft__ a, __ct__ b)
        {
            __ft__ normSquared = Fun.MultiplyAdd(b.Real, b.Real, b.Imag * b.Imag);
            __ft__ t = 1 / normSquared;
            var result = new __ct__(a * (b.Real * t), a * (-b.Imag * t));

            if (IsNormalValue(normSquared))
                return result;

            return GetScaledDivision(new __ct__(a, 0), b, result);
        }

        /// <summary>
        /// Negates a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator -(__ct__ a)
            => new __ct__(-a.Real, -a.Imag);

        /// <summary>
        /// Returns the conjugate of a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ operator !(__ct__ a)
            => a.Conjugated;

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Returns whether two <see cref="__ct__"/> are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__ct__ a, __ct__ b)
            => a.Real == b.Real && a.Imag == b.Imag;

        /// <summary>
        /// Returns whether two <see cref="__ct__"/> are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__ct__ a, __ct__ b)
            => !(a == b);

        #endregion

        #region Overrides

        public override readonly int GetHashCode()
            => HashCode.GetCombined(Real, Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(__ct__ other)
            => Real.Equals(other.Real) && Imag.Equals(other.Imag);

        public override readonly bool Equals(object other)
        {
            if (other is __ct__ obj)
                return Equals(obj);
            else
                return false;
        }

        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Real, Imag);
        }

        public static __ct__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray(2);
            return new __ct__(
                __ft__.Parse(x[0], CultureInfo.InvariantCulture),
                __ft__.Parse(x[1], CultureInfo.InvariantCulture)
            );
        }

        #endregion
    }

    public static partial class Complex
    {
        #region Conjugate

        /// <summary>
        /// Returns the conjugate of a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Conjugated(__ct__ c)
            => c.Conjugated;

        #endregion

        #region Norm

        /// <summary>
        /// Returns the squared Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ft__ NormSquared(__ct__ c)
            => c.NormSquared;

        /// <summary>
        /// Returns the Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ft__ Norm(__ct__ c)
            => c.Norm;

        #endregion

        #region Argument

        /// <summary>
        /// Retruns the argument of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ft__ Argument(__ct__ c)
            => c.Argument;

        #endregion
    }

    public static partial class Fun
    {
        #region Power

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Pow(this __ct__ number, __ct__ exponent)
        {
            if (number.IsZero)
                return __ct__.Zero;
            else if (exponent.IsZero)
                return __ct__.One;
            else
            {
                __ft__ r = number.Norm;
                __ft__ phi = number.Argument;

                __ft__ a = exponent.Real;
                __ft__ b = exponent.Imag;

                return __ct__.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
            }
        }

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Pow(this __ct__ number, __ft__ exponent)
        {
            if (number.IsZero)
                return __ct__.Zero;
            else
            {
                __ft__ r = number.Norm;
                __ft__ phi = number.Argument;
                return __ct__.CreateRadial(Pow(r, exponent), exponent * phi);
            }
        }

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Pow(this __ft__ number, __ct__ exponent)
        {
            if (number == 0)
                return __ct__.Zero;
            else
            {
                __ft__ a = exponent.Real;
                __ft__ b = exponent.Imag;

                if (number < 0)
                {
                    var phi = __constant__.Pi;
                    return __ct__.CreateRadial(Exp(Log(-number) * a - b * phi), a * phi + b * Log(-number));
                }
                else
                    return __ct__.CreateRadial(Pow(number, a), b * Log(number));
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Power(this __ct__ number, __ct__ exponent)
            => Pow(number, exponent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Power(this __ct__ number, __ft__ exponent)
            => Pow(number, exponent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Power(this __ft__ number, __ct__ exponent)
            => Pow(number, exponent);

        #endregion

        #region Trigonometry

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Acos(this __ct__ x)
        {
            var t = Log(new __ct__(-x.Imag, x.Real) + Sqrt(1 - x * x));
            return new __ct__(-t.Imag + __constant__.PiHalf, t.Real);
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Acosh(this __ct__ x)
            => Log(x + Sqrt(x * x - 1));

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Cos(this __ct__ x)
            => (
                Exp(new __ct__(-x.Imag, x.Real)) +
                Exp(new __ct__(x.Imag, -x.Real))
            ) * __half__;

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Cosh(this __ct__ x)
            => Cos(new __ct__(-x.Imag, x.Real));

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Asin(this __ct__ x)
        {
            var t = Log(new __ct__(-x.Imag, x.Real) + Sqrt(1 - x * x));
            return new __ct__(t.Imag, -t.Real);
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Asinh(this __ct__ x)
            => Log(x + Sqrt(1 + x * x));

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sin(this __ct__ x)
        {
            var a = Exp(new __ct__(-x.Imag, x.Real)) - Exp(new __ct__(x.Imag, -x.Real));
            return new __ct__(a.Imag, -a.Real) * __half__;
        }

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sinh(this __ct__ x)
        {
            var sin = Sin(new __ct__(-x.Imag, x.Real));
            return new __ct__(sin.Imag, -sin.Real);
        }

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Atan(this __ct__ x)
        {
            if (x == __ct__.I)
                return __ct__.PositiveInfinityI;
            else if (x == -__ct__.I)
                return __ct__.NegativeInfinityI;
            else if (x == __ct__.PositiveInfinity)
                return new __ct__(__constant__.PiHalf);
            else if (x == __ct__.NegativeInfinity)
                return new __ct__(-__constant__.PiHalf);
            else
                return new __ct__(0, __half__) * Log((__ct__.I + x) / (__ct__.I - x));
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Atanh(this __ct__ x)
        {
            if (x == __ct__.Zero)
                return __ct__.Zero;
            else if (x == __ct__.One)
                return __ct__.PositiveInfinity;
            else if (x == __ct__.PositiveInfinity)
                return new __ct__(0, -__constant__.PiHalf);
            else if (x == __ct__.I)
                return new __ct__(0, __constant__.PiQuarter);
            else
                return __half__ * (Log(1 + x) - Log(1 - x));
        }

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Tan(this __ct__ x)
        {
            if (x == __ct__.PositiveInfinityI)
                return __ct__.I;
            else if (x == __ct__.NegativeInfinityI)
                return -__ct__.I;
            else
                return Sin(x) / Cos(x);
        }

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Tanh(this __ct__ x)
        {
            var tan = Tan(new __ct__(-x.Imag, x.Real));
            return new __ct__(tan.Imag, -tan.Real);
        }

        #endregion

        #region Exp, Log

        /// <summary>
        /// Returns e raised to the power of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Exp(this __ct__ x)
            => new __ct__(Cos(x.Imag), Sin(x.Imag)) * Exp(x.Real);

        /// <summary>
        /// Returns the natural logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log(this __ct__ x)
            => new __ct__(Log(x.Norm), x.Argument);

        /// <summary>
        /// Returns the logarithm of the complex number <paramref name="x"/> in the given basis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log(this __ct__ x, __ft__ basis)
            => x.Log() / basis.Log();

        /// <summary>
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log10(this __ct__ x)
            => Log(x, 10);

        /// <summary>
        /// Returns the base-2 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log2(this __ct__ x)
            => x.Log() * __constant__.Ln2Inv;

        #endregion

        #region Roots

        /// <summary>
        /// Returns the principal square root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sqrt(this __ct__ x)
        {
            if (x.Imag == 0)
            {
                if (x.Real < 0)
                    return new __ct__(0, Sqrt(-x.Real));
                else
                    return new __ct__(Sqrt(x.Real), 0);
            }

            __ft__ norm = x.Norm;
            __ft__ halfSum = (norm + Abs(x.Real)) * __half__;
            if (halfSum >= __minNormal__ && halfSum < __ft__.PositiveInfinity)
            {
                __ft__ component = Sqrt(halfSum);
                return x.Real >= 0
                    ? new __ct__(component, x.Imag / (2 * component))
                    : new __ct__(Abs(x.Imag) / (2 * component), CopySign(component, x.Imag));
            }

            var a = norm;
            var b = x + a;
            var direct = a.Sqrt() * (b / b.Norm);
            return __ct__.GetScaledSquareRoot(x, direct);
        }

        /// <summary>
        /// Returns the principal cubic root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Cbrt(this __ct__ x)
            => __ct__.CreateRadial(Cbrt(x.Norm), x.Argument / 3);

        //# signedtypes.ForEach(t => { if (t != Meta.DecimalType) {
        //# if (ftype == Meta.DoubleType ^ t == Meta.FloatType) {
        /// <summary>
        /// Returns the square root of the given real number and returns a complex number.
        //# if (!dreptypes.Contains(t)) {
        /// Note: This function uses a double representation internally, but not all __t.Name__ values can be represented exactly as double.
        //# }
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Csqrt(this __t.Name__ number)
        {
            if (number >= 0)
            {
                return new __ct__(Sqrt(number), 0);
            }
            else
            {
                return new __ct__(0, Sqrt(-number));
            }
        }
        //# }}});

        /// <summary>
        /// Calculates both square roots of a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__[] Csqrt(this __ct__ number)
        {
            __ct__ res0 = __ct__.CreateRadial(Sqrt(number.Norm), number.Argument / 2);
            __ct__ res1 = __ct__.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + __constant__.Pi);

            return new __ct__[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th root of a complex number and returns n solutions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__[] Root(this __ct__ number, int n)
        {
            __ct__[] values = new __ct__[n];

            __ft__ invN = 1 / (__ft__)n;
            __ft__ phi = number.Argument / n;
            __ft__ dphi = __constant__.PiTimesTwo * invN;
            __ft__ r = Pow(number.Norm, invN);

            for (int i = 0; i < n; i++)
            {
                values[i] = __ct__.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }

        #endregion

        #region ApproximateEquals

        /// <summary>
        /// Returns whether the given complex numbers are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __ct__ a, __ct__ b, __ft__ tolerance)
        {
            return ApproximateEquals(a.Real, b.Real, tolerance) && ApproximateEquals(a.Imag, b.Imag, tolerance);
        }

        /// <summary>
        /// Returns whether the given complex numbers are equal within
        /// Constant&lt;__ft__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __ct__ a, __ct__ b)
        {
            return ApproximateEquals(a, b, Constant<__ft__>.PositiveTinyValue);
        }

        #endregion

        #region Special Floating Point Value Checks

        /// <summary>
        /// Returns whether the given <see cref="__ct__"/> is NaN.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(__ct__ v)
            => v.IsNaN;

        /// <summary>
        /// Returns whether the given <see cref="__ct__"/> is infinity (positive or negative).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(__ct__ v)
            => v.IsInfinity;

        /// <summary>
        /// Returns whether the given <see cref="__ct__"/> is positive infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(__ct__ v)
            => v.IsPositiveInfinity;

        /// <summary>
        /// Returns whether the given <see cref="__ct__"/> is negative infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(__ct__ v)
            => v.IsNegativeInfinity;

        /// <summary>
        /// Returns whether the given <see cref="__ct__"/> is finite (i.e. not NaN and not infinity).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(__ct__ v)
            => v.IsFinite;

        #endregion
    }

    //# } // isDouble
}
