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
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ComplexF : IEquatable<ComplexF>
    {
        [DataMember]
        public float Real;
        [DataMember]
        public float Imag;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="ComplexF"/> from a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComplexF(float real)
        {
            Real = real;
            Imag = 0;
        }

        /// <summary>
        /// Constructs a <see cref="ComplexF"/> from a real and an imaginary part.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComplexF(float real, float imag)
        {
            Real = real;
            Imag = imag;
        }

        /// <summary>
        /// Constructs a <see cref="ComplexF"/> from a <see cref="ComplexD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComplexF(ComplexD complex)
        {
            Real = (float)complex.Real;
            Imag = (float)complex.Imag;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Returns 0 + 0i.
        /// </summary>
        public static ComplexF Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexF(0, 0);
        }

        /// <summary>
        /// Returns 1 + 0i.
        /// </summary>
        public static ComplexF One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexF(1, 0);
        }

        /// <summary>
        /// Returns 0 + 1i.
        /// </summary>
        public static ComplexF I
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexF(0, 1);
        }

        /// <summary>
        /// Returns ∞ + 0i.
        /// </summary>
        public static ComplexF PositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexF(float.PositiveInfinity);
        }

        /// <summary>
        /// Returns -∞ + 0i.
        /// </summary>
        public static ComplexF NegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexF(float.NegativeInfinity);
        }

        /// <summary>
        /// Returns 0 + ∞i.
        /// </summary>
        public static ComplexF PositiveInfinityI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexF(0, float.PositiveInfinity);
        }

        /// <summary>
        /// Returns 0 - ∞i.
        /// </summary>
        public static ComplexF NegativeInfinityI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexF(0, float.NegativeInfinity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the conjugated of the complex number.
        /// </summary>
        public readonly ComplexF Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ComplexF(Real, -Imag); }
        }

        /// <summary>
        /// Returns the reciprocal of the complex number.
        /// </summary>
        public readonly ComplexF Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                float t = 1 / NormSquared;
                return new ComplexF(Real * t, -Imag * t);
            }
        }

        /// <summary>
        /// Returns the squared Gaussian Norm (modulus) of the complex number.
        /// </summary>
        public readonly float NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Returns the Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [XmlIgnore]
        public float Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get { return Fun.Sqrt(Real * Real + Imag * Imag); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                float r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Retruns the argument of the complex number.
        /// </summary>
        [XmlIgnore]
        public float Argument
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get { return Fun.Atan2(Imag, Real); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                float r = Norm;

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
            get { return (float.IsNaN(Real) || float.IsNaN(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite (positive or negative).
        /// </summary>
        public readonly bool IsInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (float.IsInfinity(Real) || float.IsInfinity(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite and positive.
        /// </summary>
        public readonly bool IsPositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (float.IsPositiveInfinity(Real) || float.IsPositiveInfinity(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite and negative.
        /// </summary>
        public readonly bool IsNegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (float.IsNegativeInfinity(Real) || float.IsNegativeInfinity(Imag)); }
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
        public static ComplexF CreateRadial(float r, float phi)
            => new ComplexF(r * Fun.Cos(phi), r * Fun.Sin(phi));

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF CreateOrthogonal(float real, float imag)
            => new ComplexF(real, imag);

        #endregion

        #region Static methods for F# core and Aardvark library support

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Acos(ComplexF x)
            => x.Acos();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Acoshb(ComplexF x)
            => x.Acosh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cos(ComplexF x)
            => x.Cos();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cosh(ComplexF x)
            => x.Cosh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Asin(ComplexF x)
            => x.Asin();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Asinhb(ComplexF x)
            => x.Asinh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sin(ComplexF x)
            => x.Sin();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sinh(ComplexF x)
            => x.Sinh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Atan(ComplexF x)
            => x.Atan();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Atanhb(ComplexF x)
            => x.Atanh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tan(ComplexF x)
            => x.Tan();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tanh(ComplexF x)
            => x.Tanh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sqrt(ComplexF x)
            => x.Sqrt();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF CubeRoot(ComplexF x)
            => x.Cbrt();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Exp(ComplexF x)
            => x.Exp();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log(ComplexF x)
            => x.Log();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF LogBinary(ComplexF x)
            => x.Log2();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log10(ComplexF x)
            => x.Log10();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF DivideByInt(ComplexF x, int y)
            => x / y;

        #endregion

        #region Operators

        /// <summary>
        /// Conversion from a <see cref="ComplexF"/> to a <see cref="ComplexD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ComplexD(ComplexF c)
            => new ComplexD(c);

        /// <summary>
        /// Implicit conversion from a <see cref="float"/> to a <see cref="ComplexF"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ComplexF(float a)
            => new ComplexF(a);

        /// <summary>
        /// Adds two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator +(ComplexF a, ComplexF b)
            => new ComplexF(a.Real + b.Real, a.Imag + b.Imag);

        /// <summary>
        /// Adds a complex number and a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator +(ComplexF a, float b)
            => new ComplexF(a.Real + b, a.Imag);

        /// <summary>
        /// Adds a real number and a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator +(float a, ComplexF b)
            => new ComplexF(a + b.Real, b.Imag);

        /// <summary>
        /// Subtracts two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(ComplexF a, ComplexF b)
            => new ComplexF(a.Real - b.Real, a.Imag - b.Imag);

        /// <summary>
        /// Subtracts a real number from a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(ComplexF a, float b)
            => new ComplexF(a.Real - b, a.Imag);

        /// <summary>
        /// Subtracts a complex number from a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(float a, ComplexF b)
            => new ComplexF(a - b.Real, -b.Imag);

        /// <summary>
        /// Multiplies two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator *(ComplexF a, ComplexF b)
            => new ComplexF(
                a.Real * b.Real - a.Imag * b.Imag,
                a.Real * b.Imag + a.Imag * b.Real);

        /// <summary>
        /// Multiplies a complex number and a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator *(ComplexF a, float b)
            => new ComplexF(a.Real * b, a.Imag * b);

        /// <summary>
        /// Multiplies a real number and a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator *(float a, ComplexF b)
            => new ComplexF(a * b.Real, a * b.Imag);

        /// <summary>
        /// Divides two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator /(ComplexF a, ComplexF b)
        {
            float t = 1 / b.NormSquared;
            return new ComplexF(
                t * (a.Real * b.Real + a.Imag * b.Imag),
                t * (a.Imag * b.Real - a.Real * b.Imag));
        }

        /// <summary>
        /// Divides a complex number by a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator /(ComplexF a, float b)
            => new ComplexF(a.Real / b, a.Imag / b);

        /// <summary>
        /// Divides a real number by a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator /(float a, ComplexF b)
        {
            float t = 1 / b.NormSquared;
            return new ComplexF(
                t * (a * b.Real),
                t * (-a * b.Imag));
        }

        /// <summary>
        /// Negates a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(ComplexF a)
            => new ComplexF(-a.Real, -a.Imag);

        /// <summary>
        /// Returns the conjugate of a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator !(ComplexF a)
            => a.Conjugated;

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Returns whether two <see cref="ComplexF"/> are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ComplexF a, ComplexF b)
            => a.Real == b.Real && a.Imag == b.Imag;

        /// <summary>
        /// Returns whether two <see cref="ComplexF"/> are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ComplexF a, ComplexF b)
            => !(a == b);

        #endregion

        #region Overrides

        public override readonly int GetHashCode()
            => HashCode.GetCombined(Real, Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(ComplexF other)
            => Real.Equals(other.Real) && Imag.Equals(other.Imag);

        public override readonly bool Equals(object other)
        {
            if (other is ComplexF obj)
                return Equals(obj);
            else
                return false;
        }

        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Real, Imag);
        }

        public static ComplexF Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray(2);
            return new ComplexF(
                float.Parse(x[0], CultureInfo.InvariantCulture),
                float.Parse(x[1], CultureInfo.InvariantCulture)
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
        public static ComplexF Conjugated(ComplexF c)
            => c.Conjugated;

        #endregion

        #region Norm

        /// <summary>
        /// Returns the squared Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NormSquared(ComplexF c)
            => c.NormSquared;

        /// <summary>
        /// Returns the Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Norm(ComplexF c)
            => c.Norm;

        #endregion

        #region Argument

        /// <summary>
        /// Retruns the argument of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Argument(ComplexF c)
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
        public static ComplexF Pow(this ComplexF number, ComplexF exponent)
        {
            if (number.IsZero)
                return ComplexF.Zero;
            else if (exponent.IsZero)
                return ComplexF.One;
            else
            {
                float r = number.Norm;
                float phi = number.Argument;

                float a = exponent.Real;
                float b = exponent.Imag;

                return ComplexF.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
            }
        }

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Pow(this ComplexF number, float exponent)
        {
            if (number.IsZero)
                return ComplexF.Zero;
            else
            {
                float r = number.Norm;
                float phi = number.Argument;
                return ComplexF.CreateRadial(Pow(r, exponent), exponent * phi);
            }
        }

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Pow(this float number, ComplexF exponent)
        {
            if (number == 0)
                return ComplexF.Zero;
            else
            {
                float a = exponent.Real;
                float b = exponent.Imag;

                if (number < 0)
                {
                    var phi = ConstantF.Pi;
                    return ComplexF.CreateRadial(Exp(Log(-number) * a - b * phi), a * phi + b * Log(-number));
                }
                else
                    return ComplexF.CreateRadial(Pow(number, a), b * Log(number));
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Power(this ComplexF number, ComplexF exponent)
            => Pow(number, exponent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Power(this ComplexF number, float exponent)
            => Pow(number, exponent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Power(this float number, ComplexF exponent)
            => Pow(number, exponent);

        #endregion

        #region Trigonometry

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Acos(this ComplexF x)
        {
            var t = Log(new ComplexF(-x.Imag, x.Real) + Sqrt(1 - x * x));
            return new ComplexF(-t.Imag + ConstantF.PiHalf, t.Real);
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Acosh(this ComplexF x)
            => Log(x + Sqrt(x * x - 1));

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cos(this ComplexF x)
            => (
                Exp(new ComplexF(-x.Imag, x.Real)) +
                Exp(new ComplexF(x.Imag, -x.Real))
            ) * 0.5f;

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cosh(this ComplexF x)
            => Cos(new ComplexF(-x.Imag, x.Real));

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Asin(this ComplexF x)
        {
            var t = Log(new ComplexF(-x.Imag, x.Real) + Sqrt(1 - x * x));
            return new ComplexF(t.Imag, -t.Real);
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Asinh(this ComplexF x)
            => Log(x + Sqrt(1 + x * x));

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sin(this ComplexF x)
        {
            var a = Exp(new ComplexF(-x.Imag, x.Real)) - Exp(new ComplexF(x.Imag, -x.Real));
            return new ComplexF(a.Imag, -a.Real) * 0.5f;
        }

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sinh(this ComplexF x)
        {
            var sin = Sin(new ComplexF(-x.Imag, x.Real));
            return new ComplexF(sin.Imag, -sin.Real);
        }

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Atan(this ComplexF x)
        {
            if (x == ComplexF.I)
                return ComplexF.PositiveInfinityI;
            else if (x == -ComplexF.I)
                return ComplexF.NegativeInfinityI;
            else if (x == ComplexF.PositiveInfinity)
                return new ComplexF(ConstantF.PiHalf);
            else if (x == ComplexF.NegativeInfinity)
                return new ComplexF(-ConstantF.PiHalf);
            else
                return new ComplexF(0, 0.5f) * Log((ComplexF.I + x) / (ComplexF.I - x));
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Atanh(this ComplexF x)
        {
            if (x == ComplexF.Zero)
                return ComplexF.Zero;
            else if (x == ComplexF.One)
                return ComplexF.PositiveInfinity;
            else if (x == ComplexF.PositiveInfinity)
                return new ComplexF(0, -ConstantF.PiHalf);
            else if (x == ComplexF.I)
                return new ComplexF(0, ConstantF.PiQuarter);
            else
                return 0.5f * (Log(1 + x) - Log(1 - x));
        }

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tan(this ComplexF x)
        {
            if (x == ComplexF.PositiveInfinityI)
                return ComplexF.I;
            else if (x == ComplexF.NegativeInfinityI)
                return -ComplexF.I;
            else
                return Sin(x) / Cos(x);
        }

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tanh(this ComplexF x)
        {
            var tan = Tan(new ComplexF(-x.Imag, x.Real));
            return new ComplexF(tan.Imag, -tan.Real);
        }

        #endregion

        #region Exp, Log

        /// <summary>
        /// Returns e raised to the power of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Exp(this ComplexF x)
            => new ComplexF(Cos(x.Imag), Sin(x.Imag)) * Exp(x.Real);

        /// <summary>
        /// Returns the natural logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log(this ComplexF x)
            => new ComplexF(Log(x.Norm), x.Argument);

        /// <summary>
        /// Returns the logarithm of the complex number <paramref name="x"/> in the given basis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log(this ComplexF x, float basis)
            => x.Log() / basis.Log();

        /// <summary>
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log10(this ComplexF x)
            => Log(x, 10);

        /// <summary>
        /// Returns the base-2 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log2(this ComplexF x)
            => x.Log() * ConstantF.Ln2Inv;

        #endregion

        #region Roots

        /// <summary>
        /// Returns the principal square root of the complex number <paramref name="x"/>.
        /// </summary>
        // https://math.stackexchange.com/a/44500
        // TODO: Check if this is actually better than the naive implementation
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sqrt(this ComplexF x)
        {
            if (x.Imag == 0)
            {
                if (x.Real < 0)
                    return new ComplexF(0, Sqrt(-x.Real));
                else
                    return new ComplexF(Sqrt(x.Real), 0);
            }
            else
            {
                var a = x.Norm;
                var b = x + a;
                return a.Sqrt() * (b / b.Norm);
            }
        }

        /// <summary>
        /// Returns the principal cubic root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cbrt(this ComplexF x)
            => ComplexF.CreateRadial(Cbrt(x.Norm), x.Argument / 3);

        /// <summary>
        /// Returns the square root of the given real number and returns a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Csqrt(this float number)
        {
            if (number >= 0)
            {
                return new ComplexF(Sqrt(number), 0);
            }
            else
            {
                return new ComplexF(0, Sqrt(-number));
            }
        }

        /// <summary>
        /// Calculates both square roots of a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF[] Csqrt(this ComplexF number)
        {
            ComplexF res0 = ComplexF.CreateRadial(Sqrt(number.Norm), number.Argument / 2);
            ComplexF res1 = ComplexF.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + ConstantF.Pi);

            return new ComplexF[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th root of a complex number and returns n solutions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF[] Root(this ComplexF number, int n)
        {
            ComplexF[] values = new ComplexF[n];

            float invN = 1 / (float)n;
            float phi = number.Argument / n;
            float dphi = ConstantF.PiTimesTwo * invN;
            float r = Pow(number.Norm, invN);

            for (int i = 0; i < n; i++)
            {
                values[i] = ComplexF.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }

        #endregion

        #region ApproximateEquals

        /// <summary>
        /// Returns whether the given complex numbers are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ComplexF a, ComplexF b, float tolerance)
        {
            return ApproximateEquals(a.Real, b.Real, tolerance) && ApproximateEquals(a.Imag, b.Imag, tolerance);
        }

        /// <summary>
        /// Returns whether the given complex numbers are equal within
        /// Constant&lt;float&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ComplexF a, ComplexF b)
        {
            return ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
        }

        #endregion

        #region Special Floating Point Value Checks

        /// <summary>
        /// Returns whether the given <see cref="ComplexF"/> is NaN.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(ComplexF v)
            => v.IsNaN;

        /// <summary>
        /// Returns whether the given <see cref="ComplexF"/> is infinity (positive or negative).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(ComplexF v)
            => v.IsInfinity;

        /// <summary>
        /// Returns whether the given <see cref="ComplexF"/> is positive infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(ComplexF v)
            => v.IsPositiveInfinity;

        /// <summary>
        /// Returns whether the given <see cref="ComplexF"/> is negative infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(ComplexF v)
            => v.IsNegativeInfinity;

        /// <summary>
        /// Returns whether the given <see cref="ComplexF"/> is finite (i.e. not NaN and not infinity).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(ComplexF v)
            => v.IsFinite;

        #endregion
    }

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ComplexD : IEquatable<ComplexD>
    {
        [DataMember]
        public double Real;
        [DataMember]
        public double Imag;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="ComplexD"/> from a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComplexD(double real)
        {
            Real = real;
            Imag = 0;
        }

        /// <summary>
        /// Constructs a <see cref="ComplexD"/> from a real and an imaginary part.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComplexD(double real, double imag)
        {
            Real = real;
            Imag = imag;
        }

        /// <summary>
        /// Constructs a <see cref="ComplexD"/> from a <see cref="ComplexF"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComplexD(ComplexF complex)
        {
            Real = (double)complex.Real;
            Imag = (double)complex.Imag;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Returns 0 + 0i.
        /// </summary>
        public static ComplexD Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexD(0, 0);
        }

        /// <summary>
        /// Returns 1 + 0i.
        /// </summary>
        public static ComplexD One
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexD(1, 0);
        }

        /// <summary>
        /// Returns 0 + 1i.
        /// </summary>
        public static ComplexD I
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexD(0, 1);
        }

        /// <summary>
        /// Returns ∞ + 0i.
        /// </summary>
        public static ComplexD PositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexD(double.PositiveInfinity);
        }

        /// <summary>
        /// Returns -∞ + 0i.
        /// </summary>
        public static ComplexD NegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexD(double.NegativeInfinity);
        }

        /// <summary>
        /// Returns 0 + ∞i.
        /// </summary>
        public static ComplexD PositiveInfinityI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexD(0, double.PositiveInfinity);
        }

        /// <summary>
        /// Returns 0 - ∞i.
        /// </summary>
        public static ComplexD NegativeInfinityI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ComplexD(0, double.NegativeInfinity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the conjugated of the complex number.
        /// </summary>
        public readonly ComplexD Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ComplexD(Real, -Imag); }
        }

        /// <summary>
        /// Returns the reciprocal of the complex number.
        /// </summary>
        public readonly ComplexD Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                double t = 1 / NormSquared;
                return new ComplexD(Real * t, -Imag * t);
            }
        }

        /// <summary>
        /// Returns the squared Gaussian Norm (modulus) of the complex number.
        /// </summary>
        public readonly double NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Returns the Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [XmlIgnore]
        public double Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get { return Fun.Sqrt(Real * Real + Imag * Imag); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                double r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Retruns the argument of the complex number.
        /// </summary>
        [XmlIgnore]
        public double Argument
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get { return Fun.Atan2(Imag, Real); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                double r = Norm;

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
            get { return (double.IsNaN(Real) || double.IsNaN(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite (positive or negative).
        /// </summary>
        public readonly bool IsInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (double.IsInfinity(Real) || double.IsInfinity(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite and positive.
        /// </summary>
        public readonly bool IsPositiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (double.IsPositiveInfinity(Real) || double.IsPositiveInfinity(Imag)); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is infinite and negative.
        /// </summary>
        public readonly bool IsNegativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (double.IsNegativeInfinity(Real) || double.IsNegativeInfinity(Imag)); }
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
        public static ComplexD CreateRadial(double r, double phi)
            => new ComplexD(r * Fun.Cos(phi), r * Fun.Sin(phi));

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD CreateOrthogonal(double real, double imag)
            => new ComplexD(real, imag);

        #endregion

        #region Static methods for F# core and Aardvark library support

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Acos(ComplexD x)
            => x.Acos();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Acoshb(ComplexD x)
            => x.Acosh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cos(ComplexD x)
            => x.Cos();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cosh(ComplexD x)
            => x.Cosh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Asin(ComplexD x)
            => x.Asin();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Asinhb(ComplexD x)
            => x.Asinh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sin(ComplexD x)
            => x.Sin();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sinh(ComplexD x)
            => x.Sinh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Atan(ComplexD x)
            => x.Atan();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Atanhb(ComplexD x)
            => x.Atanh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tan(ComplexD x)
            => x.Tan();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tanh(ComplexD x)
            => x.Tanh();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sqrt(ComplexD x)
            => x.Sqrt();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD CubeRoot(ComplexD x)
            => x.Cbrt();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Exp(ComplexD x)
            => x.Exp();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log(ComplexD x)
            => x.Log();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD LogBinary(ComplexD x)
            => x.Log2();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log10(ComplexD x)
            => x.Log10();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD DivideByInt(ComplexD x, int y)
            => x / y;

        #endregion

        #region Operators

        /// <summary>
        /// Conversion from a <see cref="ComplexD"/> to a <see cref="ComplexF"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ComplexF(ComplexD c)
            => new ComplexF(c);

        /// <summary>
        /// Implicit conversion from a <see cref="double"/> to a <see cref="ComplexD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ComplexD(double a)
            => new ComplexD(a);

        /// <summary>
        /// Adds two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator +(ComplexD a, ComplexD b)
            => new ComplexD(a.Real + b.Real, a.Imag + b.Imag);

        /// <summary>
        /// Adds a complex number and a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator +(ComplexD a, double b)
            => new ComplexD(a.Real + b, a.Imag);

        /// <summary>
        /// Adds a real number and a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator +(double a, ComplexD b)
            => new ComplexD(a + b.Real, b.Imag);

        /// <summary>
        /// Subtracts two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(ComplexD a, ComplexD b)
            => new ComplexD(a.Real - b.Real, a.Imag - b.Imag);

        /// <summary>
        /// Subtracts a real number from a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(ComplexD a, double b)
            => new ComplexD(a.Real - b, a.Imag);

        /// <summary>
        /// Subtracts a complex number from a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(double a, ComplexD b)
            => new ComplexD(a - b.Real, -b.Imag);

        /// <summary>
        /// Multiplies two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator *(ComplexD a, ComplexD b)
            => new ComplexD(
                a.Real * b.Real - a.Imag * b.Imag,
                a.Real * b.Imag + a.Imag * b.Real);

        /// <summary>
        /// Multiplies a complex number and a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator *(ComplexD a, double b)
            => new ComplexD(a.Real * b, a.Imag * b);

        /// <summary>
        /// Multiplies a real number and a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator *(double a, ComplexD b)
            => new ComplexD(a * b.Real, a * b.Imag);

        /// <summary>
        /// Divides two complex numbers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator /(ComplexD a, ComplexD b)
        {
            double t = 1 / b.NormSquared;
            return new ComplexD(
                t * (a.Real * b.Real + a.Imag * b.Imag),
                t * (a.Imag * b.Real - a.Real * b.Imag));
        }

        /// <summary>
        /// Divides a complex number by a real number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator /(ComplexD a, double b)
            => new ComplexD(a.Real / b, a.Imag / b);

        /// <summary>
        /// Divides a real number by a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator /(double a, ComplexD b)
        {
            double t = 1 / b.NormSquared;
            return new ComplexD(
                t * (a * b.Real),
                t * (-a * b.Imag));
        }

        /// <summary>
        /// Negates a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(ComplexD a)
            => new ComplexD(-a.Real, -a.Imag);

        /// <summary>
        /// Returns the conjugate of a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator !(ComplexD a)
            => a.Conjugated;

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Returns whether two <see cref="ComplexD"/> are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ComplexD a, ComplexD b)
            => a.Real == b.Real && a.Imag == b.Imag;

        /// <summary>
        /// Returns whether two <see cref="ComplexD"/> are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ComplexD a, ComplexD b)
            => !(a == b);

        #endregion

        #region Overrides

        public override readonly int GetHashCode()
            => HashCode.GetCombined(Real, Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(ComplexD other)
            => Real.Equals(other.Real) && Imag.Equals(other.Imag);

        public override readonly bool Equals(object other)
        {
            if (other is ComplexD obj)
                return Equals(obj);
            else
                return false;
        }

        public override readonly string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Real, Imag);
        }

        public static ComplexD Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray(2);
            return new ComplexD(
                double.Parse(x[0], CultureInfo.InvariantCulture),
                double.Parse(x[1], CultureInfo.InvariantCulture)
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
        public static ComplexD Conjugated(ComplexD c)
            => c.Conjugated;

        #endregion

        #region Norm

        /// <summary>
        /// Returns the squared Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NormSquared(ComplexD c)
            => c.NormSquared;

        /// <summary>
        /// Returns the Gaussian Norm (modulus) of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Norm(ComplexD c)
            => c.Norm;

        #endregion

        #region Argument

        /// <summary>
        /// Retruns the argument of the complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Argument(ComplexD c)
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
        public static ComplexD Pow(this ComplexD number, ComplexD exponent)
        {
            if (number.IsZero)
                return ComplexD.Zero;
            else if (exponent.IsZero)
                return ComplexD.One;
            else
            {
                double r = number.Norm;
                double phi = number.Argument;

                double a = exponent.Real;
                double b = exponent.Imag;

                return ComplexD.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
            }
        }

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Pow(this ComplexD number, double exponent)
        {
            if (number.IsZero)
                return ComplexD.Zero;
            else
            {
                double r = number.Norm;
                double phi = number.Argument;
                return ComplexD.CreateRadial(Pow(r, exponent), exponent * phi);
            }
        }

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Pow(this double number, ComplexD exponent)
        {
            if (number == 0)
                return ComplexD.Zero;
            else
            {
                double a = exponent.Real;
                double b = exponent.Imag;

                if (number < 0)
                {
                    var phi = Constant.Pi;
                    return ComplexD.CreateRadial(Exp(Log(-number) * a - b * phi), a * phi + b * Log(-number));
                }
                else
                    return ComplexD.CreateRadial(Pow(number, a), b * Log(number));
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Power(this ComplexD number, ComplexD exponent)
            => Pow(number, exponent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Power(this ComplexD number, double exponent)
            => Pow(number, exponent);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Power(this double number, ComplexD exponent)
            => Pow(number, exponent);

        #endregion

        #region Trigonometry

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Acos(this ComplexD x)
        {
            var t = Log(new ComplexD(-x.Imag, x.Real) + Sqrt(1 - x * x));
            return new ComplexD(-t.Imag + Constant.PiHalf, t.Real);
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Acosh(this ComplexD x)
            => Log(x + Sqrt(x * x - 1));

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cos(this ComplexD x)
            => (
                Exp(new ComplexD(-x.Imag, x.Real)) +
                Exp(new ComplexD(x.Imag, -x.Real))
            ) * 0.5;

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cosh(this ComplexD x)
            => Cos(new ComplexD(-x.Imag, x.Real));

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Asin(this ComplexD x)
        {
            var t = Log(new ComplexD(-x.Imag, x.Real) + Sqrt(1 - x * x));
            return new ComplexD(t.Imag, -t.Real);
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Asinh(this ComplexD x)
            => Log(x + Sqrt(1 + x * x));

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sin(this ComplexD x)
        {
            var a = Exp(new ComplexD(-x.Imag, x.Real)) - Exp(new ComplexD(x.Imag, -x.Real));
            return new ComplexD(a.Imag, -a.Real) * 0.5;
        }

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sinh(this ComplexD x)
        {
            var sin = Sin(new ComplexD(-x.Imag, x.Real));
            return new ComplexD(sin.Imag, -sin.Real);
        }

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Atan(this ComplexD x)
        {
            if (x == ComplexD.I)
                return ComplexD.PositiveInfinityI;
            else if (x == -ComplexD.I)
                return ComplexD.NegativeInfinityI;
            else if (x == ComplexD.PositiveInfinity)
                return new ComplexD(Constant.PiHalf);
            else if (x == ComplexD.NegativeInfinity)
                return new ComplexD(-Constant.PiHalf);
            else
                return new ComplexD(0, 0.5) * Log((ComplexD.I + x) / (ComplexD.I - x));
        }

        /// <summary>
        /// Returns the angle that is the hyperbolic arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Atanh(this ComplexD x)
        {
            if (x == ComplexD.Zero)
                return ComplexD.Zero;
            else if (x == ComplexD.One)
                return ComplexD.PositiveInfinity;
            else if (x == ComplexD.PositiveInfinity)
                return new ComplexD(0, -Constant.PiHalf);
            else if (x == ComplexD.I)
                return new ComplexD(0, Constant.PiQuarter);
            else
                return 0.5 * (Log(1 + x) - Log(1 - x));
        }

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tan(this ComplexD x)
        {
            if (x == ComplexD.PositiveInfinityI)
                return ComplexD.I;
            else if (x == ComplexD.NegativeInfinityI)
                return -ComplexD.I;
            else
                return Sin(x) / Cos(x);
        }

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tanh(this ComplexD x)
        {
            var tan = Tan(new ComplexD(-x.Imag, x.Real));
            return new ComplexD(tan.Imag, -tan.Real);
        }

        #endregion

        #region Exp, Log

        /// <summary>
        /// Returns e raised to the power of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Exp(this ComplexD x)
            => new ComplexD(Cos(x.Imag), Sin(x.Imag)) * Exp(x.Real);

        /// <summary>
        /// Returns the natural logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log(this ComplexD x)
            => new ComplexD(Log(x.Norm), x.Argument);

        /// <summary>
        /// Returns the logarithm of the complex number <paramref name="x"/> in the given basis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log(this ComplexD x, double basis)
            => x.Log() / basis.Log();

        /// <summary>
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log10(this ComplexD x)
            => Log(x, 10);

        /// <summary>
        /// Returns the base-2 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log2(this ComplexD x)
            => x.Log() * Constant.Ln2Inv;

        #endregion

        #region Roots

        /// <summary>
        /// Returns the principal square root of the complex number <paramref name="x"/>.
        /// </summary>
        // https://math.stackexchange.com/a/44500
        // TODO: Check if this is actually better than the naive implementation
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sqrt(this ComplexD x)
        {
            if (x.Imag == 0)
            {
                if (x.Real < 0)
                    return new ComplexD(0, Sqrt(-x.Real));
                else
                    return new ComplexD(Sqrt(x.Real), 0);
            }
            else
            {
                var a = x.Norm;
                var b = x + a;
                return a.Sqrt() * (b / b.Norm);
            }
        }

        /// <summary>
        /// Returns the principal cubic root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cbrt(this ComplexD x)
            => ComplexD.CreateRadial(Cbrt(x.Norm), x.Argument / 3);

        /// <summary>
        /// Returns the square root of the given real number and returns a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Csqrt(this sbyte number)
        {
            if (number >= 0)
            {
                return new ComplexD(Sqrt(number), 0);
            }
            else
            {
                return new ComplexD(0, Sqrt(-number));
            }
        }
        /// <summary>
        /// Returns the square root of the given real number and returns a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Csqrt(this short number)
        {
            if (number >= 0)
            {
                return new ComplexD(Sqrt(number), 0);
            }
            else
            {
                return new ComplexD(0, Sqrt(-number));
            }
        }
        /// <summary>
        /// Returns the square root of the given real number and returns a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Csqrt(this int number)
        {
            if (number >= 0)
            {
                return new ComplexD(Sqrt(number), 0);
            }
            else
            {
                return new ComplexD(0, Sqrt(-number));
            }
        }
        /// <summary>
        /// Returns the square root of the given real number and returns a complex number.
        /// Note: This function uses a double representation internally, but not all long values can be represented exactly as double.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Csqrt(this long number)
        {
            if (number >= 0)
            {
                return new ComplexD(Sqrt(number), 0);
            }
            else
            {
                return new ComplexD(0, Sqrt(-number));
            }
        }
        /// <summary>
        /// Returns the square root of the given real number and returns a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Csqrt(this double number)
        {
            if (number >= 0)
            {
                return new ComplexD(Sqrt(number), 0);
            }
            else
            {
                return new ComplexD(0, Sqrt(-number));
            }
        }

        /// <summary>
        /// Calculates both square roots of a complex number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD[] Csqrt(this ComplexD number)
        {
            ComplexD res0 = ComplexD.CreateRadial(Sqrt(number.Norm), number.Argument / 2);
            ComplexD res1 = ComplexD.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + Constant.Pi);

            return new ComplexD[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th root of a complex number and returns n solutions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD[] Root(this ComplexD number, int n)
        {
            ComplexD[] values = new ComplexD[n];

            double invN = 1 / (double)n;
            double phi = number.Argument / n;
            double dphi = Constant.PiTimesTwo * invN;
            double r = Pow(number.Norm, invN);

            for (int i = 0; i < n; i++)
            {
                values[i] = ComplexD.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }

        #endregion

        #region ApproximateEquals

        /// <summary>
        /// Returns whether the given complex numbers are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ComplexD a, ComplexD b, double tolerance)
        {
            return ApproximateEquals(a.Real, b.Real, tolerance) && ApproximateEquals(a.Imag, b.Imag, tolerance);
        }

        /// <summary>
        /// Returns whether the given complex numbers are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this ComplexD a, ComplexD b)
        {
            return ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
        }

        #endregion

        #region Special Floating Point Value Checks

        /// <summary>
        /// Returns whether the given <see cref="ComplexD"/> is NaN.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(ComplexD v)
            => v.IsNaN;

        /// <summary>
        /// Returns whether the given <see cref="ComplexD"/> is infinity (positive or negative).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(ComplexD v)
            => v.IsInfinity;

        /// <summary>
        /// Returns whether the given <see cref="ComplexD"/> is positive infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(ComplexD v)
            => v.IsPositiveInfinity;

        /// <summary>
        /// Returns whether the given <see cref="ComplexD"/> is negative infinity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(ComplexD v)
            => v.IsNegativeInfinity;

        /// <summary>
        /// Returns whether the given <see cref="ComplexD"/> is finite (i.e. not NaN and not infinity).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(ComplexD v)
            => v.IsFinite;

        #endregion
    }

}
