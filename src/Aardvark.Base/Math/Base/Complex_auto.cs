using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ComplexF
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

        #endregion

        #region Properties

        /// <summary>
        /// Returns the conjugated of the complex number.
        /// </summary>
        public ComplexF Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ComplexF(Real, -Imag); }
        }

        /// <summary>
        /// Returns the reciprocal of the complex number.
        /// </summary>
        public ComplexF Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get 
            {
                float t = 1 / NormSquared;
                return new ComplexF(Real * t, -Imag * t);
            }
        }

        /// <summary>
        /// Squared gaussian Norm (modulus) of the complex number.
        /// </summary>
        public float NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Gaussian Norm (modulus) of the complex number.
        /// </summary>
        public float Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(Real * Real + Imag * Imag); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                float r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Argument of the complex number.
        /// </summary>
        public float Argument
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Atan2(Imag, Real); }
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
        public bool IsReal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number has no real part.
        /// </summary>
        public bool IsImaginary
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is 1 + 0i.
        /// </summary>
        public bool IsOne
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.ApproximateEquals(1) && Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is zero.
        /// </summary>
        public bool IsZero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny() && Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is 0 + 1i.
        /// </summary>
        public bool IsI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny(Real) && Imag.ApproximateEquals(1); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is NaN.
        /// </summary>
        public bool IsNan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (float.IsNaN(Real) || float.IsNaN(Imag)); }
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

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Acos(ComplexF x)
            => x.Acos();

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cos(ComplexF x)
            => x.Cos();

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cosh(ComplexF x)
            => x.Cosh();

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Asin(ComplexF x)
            => x.Asin();

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sin(ComplexF x)
            => x.Sin();

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sinh(ComplexF x)
            => x.Sinh();

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Atan(ComplexF x)
            => x.Atan();

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tan(ComplexF x)
            => x.Tan();

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tanh(ComplexF x)
            => x.Tanh();

        /// <summary>
        /// Returns the square root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sqrt(ComplexF x)
            => x.Sqrt();

        /// <summary>
        /// Returns e raised to the power of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Exp(ComplexF x)
            => x.Exp();

        /// <summary>
        /// Returns the natural logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log(ComplexF x)
            => x.Log();

        /// <summary>
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log10(ComplexF x)
            => x.Log10();

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ComplexF(float a)
            => new ComplexF(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator +(ComplexF a, ComplexF b)
            => new ComplexF(a.Real + b.Real, a.Imag + b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator +(ComplexF a, float b)
            => new ComplexF(a.Real + b, a.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator +(float a, ComplexF b)
            => new ComplexF(a + b.Real, b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(ComplexF a, ComplexF b)
            => new ComplexF(a.Real - b.Real, a.Imag - b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(ComplexF a, float b)
            => new ComplexF(a.Real - b, a.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(float a, ComplexF b)
            => new ComplexF(a - b.Real, -b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator *(ComplexF a, ComplexF b)
            => new ComplexF(
                a.Real * b.Real - a.Imag * b.Imag,
                a.Real * b.Imag + a.Imag * b.Real);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator *(ComplexF a, float b)
            => new ComplexF(a.Real * b, a.Imag * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator *(float a, ComplexF b)
            => new ComplexF(a * b.Real, a * b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator /(ComplexF a, ComplexF b)
        {
            float t = 1 / b.NormSquared;
            return new ComplexF(
                t * (a.Real * b.Real + a.Imag * b.Imag),
                t * (a.Imag * b.Real - a.Real * b.Imag));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator /(ComplexF a, float b)
            => new ComplexF(a.Real / b, a.Imag / b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator /(float a, ComplexF b)
        {
            float t = 1 / b.NormSquared;
            return new ComplexF(
                t * (a * b.Real),
                t * (-a * b.Imag));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator -(ComplexF a)
            => new ComplexF(-a.Real, -a.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF operator !(ComplexF a)
            => a.Conjugated;

        #endregion

        #region Overrides

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string format)
        {
            if (!Fun.IsTiny(Real))
            {
                if (Imag > 0.0f)
                {
                    return Real.ToString(format) + " + i" + Imag.ToString(format);
                }
                else if (Fun.IsTiny(Imag))
                {
                    return Real.ToString(format);
                }
                else
                {
                    return Real.ToString(format) + " - i" + Fun.Abs(Imag).ToString(format);
                }
            }
            else
            {
                if (Fun.IsTiny(Imag - 1.00)) return "i";
                else if (Fun.IsTiny(Imag + 1.00)) return "-i";
                if (!Fun.IsTiny(Imag))
                {
                    return Imag.ToString(format) + "i";
                }
                else return "0";
            }

        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Power(this ComplexF number, ComplexF exponent)
        {
            float r = number.Norm;
            float phi = number.Argument;

            float a = exponent.Real;
            float b = exponent.Imag;

            return ComplexF.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
        }

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Pow(this ComplexF number, ComplexF exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Power(this ComplexF number, float exponent)
        {
            float r = number.NormSquared;
            float phi = Atan2(number.Imag, number.Real);

            r = Pow(r, exponent);
            phi *= exponent;

            return new ComplexF(r * Cos(phi), r * Sin(phi));
        }

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Pow(this ComplexF number, float exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Power(this float number, ComplexF exponent)
        {
            float r = number;
            float phi = (number < 0) ? (float)Constant.Pi : 0;

            float a = exponent.Real;
            float b = exponent.Imag;

            return ComplexF.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
        }

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Pow(this float number, ComplexF exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Acos(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cos(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cosh(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Asin(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sin(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sinh(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Atan(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tan(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Tanh(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the square root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Sqrt(this ComplexF x)
            => throw new NotImplementedException();

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
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log10(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the base-2 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log2(this ComplexF x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the cubic root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Cbrt(this ComplexF x)
            => throw new NotImplementedException();

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
        /// Calculates both square roots of a complex number-
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF[] Csqrt(this ComplexF number)
        {
            ComplexF res0 = ComplexF.CreateRadial(Sqrt(number.Norm), number.Argument / 2);
            ComplexF res1 = ComplexF.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + (float)Constant.Pi);

            return new ComplexF[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th root of a complex number and returns n solutions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF[] Root(this ComplexF number, int n)
        {
            ComplexF[] values = new ComplexF[n];

            float phi = number.Argument / 2;
            float dphi = (float)Constant.PiTimesTwo / (float)n;
            float r = Pow(number.Norm, 1 / n);

            for (int i = 1; i < n; i++)
            {
                values[i] = ComplexF.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }
    }

    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ComplexD
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

        #endregion

        #region Properties

        /// <summary>
        /// Returns the conjugated of the complex number.
        /// </summary>
        public ComplexD Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ComplexD(Real, -Imag); }
        }

        /// <summary>
        /// Returns the reciprocal of the complex number.
        /// </summary>
        public ComplexD Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get 
            {
                double t = 1 / NormSquared;
                return new ComplexD(Real * t, -Imag * t);
            }
        }

        /// <summary>
        /// Squared gaussian Norm (modulus) of the complex number.
        /// </summary>
        public double NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Gaussian Norm (modulus) of the complex number.
        /// </summary>
        public double Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(Real * Real + Imag * Imag); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                double r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Argument of the complex number.
        /// </summary>
        public double Argument
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Atan2(Imag, Real); }
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
        public bool IsReal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number has no real part.
        /// </summary>
        public bool IsImaginary
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is 1 + 0i.
        /// </summary>
        public bool IsOne
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.ApproximateEquals(1) && Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is zero.
        /// </summary>
        public bool IsZero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny() && Imag.IsTiny(); }
        }

        /// <summary>
        /// Returns whether the complex number is 0 + 1i.
        /// </summary>
        public bool IsI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real.IsTiny(Real) && Imag.ApproximateEquals(1); }
        }

        /// <summary>
        /// Returns whether the complex number has a part that is NaN.
        /// </summary>
        public bool IsNan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (double.IsNaN(Real) || double.IsNaN(Imag)); }
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

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Acos(ComplexD x)
            => x.Acos();

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cos(ComplexD x)
            => x.Cos();

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cosh(ComplexD x)
            => x.Cosh();

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Asin(ComplexD x)
            => x.Asin();

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sin(ComplexD x)
            => x.Sin();

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sinh(ComplexD x)
            => x.Sinh();

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Atan(ComplexD x)
            => x.Atan();

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tan(ComplexD x)
            => x.Tan();

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tanh(ComplexD x)
            => x.Tanh();

        /// <summary>
        /// Returns the square root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sqrt(ComplexD x)
            => x.Sqrt();

        /// <summary>
        /// Returns e raised to the power of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Exp(ComplexD x)
            => x.Exp();

        /// <summary>
        /// Returns the natural logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log(ComplexD x)
            => x.Log();

        /// <summary>
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log10(ComplexD x)
            => x.Log10();

        #endregion

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ComplexD(double a)
            => new ComplexD(a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator +(ComplexD a, ComplexD b)
            => new ComplexD(a.Real + b.Real, a.Imag + b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator +(ComplexD a, double b)
            => new ComplexD(a.Real + b, a.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator +(double a, ComplexD b)
            => new ComplexD(a + b.Real, b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(ComplexD a, ComplexD b)
            => new ComplexD(a.Real - b.Real, a.Imag - b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(ComplexD a, double b)
            => new ComplexD(a.Real - b, a.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(double a, ComplexD b)
            => new ComplexD(a - b.Real, -b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator *(ComplexD a, ComplexD b)
            => new ComplexD(
                a.Real * b.Real - a.Imag * b.Imag,
                a.Real * b.Imag + a.Imag * b.Real);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator *(ComplexD a, double b)
            => new ComplexD(a.Real * b, a.Imag * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator *(double a, ComplexD b)
            => new ComplexD(a * b.Real, a * b.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator /(ComplexD a, ComplexD b)
        {
            double t = 1 / b.NormSquared;
            return new ComplexD(
                t * (a.Real * b.Real + a.Imag * b.Imag),
                t * (a.Imag * b.Real - a.Real * b.Imag));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator /(ComplexD a, double b)
            => new ComplexD(a.Real / b, a.Imag / b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator /(double a, ComplexD b)
        {
            double t = 1 / b.NormSquared;
            return new ComplexD(
                t * (a * b.Real),
                t * (-a * b.Imag));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator -(ComplexD a)
            => new ComplexD(-a.Real, -a.Imag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD operator !(ComplexD a)
            => a.Conjugated;

        #endregion

        #region Overrides

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string format)
        {
            if (!Fun.IsTiny(Real))
            {
                if (Imag > 0.0f)
                {
                    return Real.ToString(format) + " + i" + Imag.ToString(format);
                }
                else if (Fun.IsTiny(Imag))
                {
                    return Real.ToString(format);
                }
                else
                {
                    return Real.ToString(format) + " - i" + Fun.Abs(Imag).ToString(format);
                }
            }
            else
            {
                if (Fun.IsTiny(Imag - 1.00)) return "i";
                else if (Fun.IsTiny(Imag + 1.00)) return "-i";
                if (!Fun.IsTiny(Imag))
                {
                    return Imag.ToString(format) + "i";
                }
                else return "0";
            }

        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Power(this ComplexD number, ComplexD exponent)
        {
            double r = number.Norm;
            double phi = number.Argument;

            double a = exponent.Real;
            double b = exponent.Imag;

            return ComplexD.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
        }

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Pow(this ComplexD number, ComplexD exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Power(this ComplexD number, double exponent)
        {
            double r = number.NormSquared;
            double phi = Atan2(number.Imag, number.Real);

            r = Pow(r, exponent);
            phi *= exponent;

            return new ComplexD(r * Cos(phi), r * Sin(phi));
        }

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Pow(this ComplexD number, double exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Power(this double number, ComplexD exponent)
        {
            double r = number;
            double phi = (number < 0) ? Constant.Pi : 0;

            double a = exponent.Real;
            double b = exponent.Imag;

            return ComplexD.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
        }

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Pow(this double number, ComplexD exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Acos(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cos(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cosh(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Asin(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sin(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sinh(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Atan(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tan(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Tanh(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the square root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Sqrt(this ComplexD x)
            => throw new NotImplementedException();

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
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log10(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the base-2 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log2(this ComplexD x)
            => throw new NotImplementedException();

        /// <summary>
        /// Returns the cubic root of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Cbrt(this ComplexD x)
            => throw new NotImplementedException();

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
        /// Calculates both square roots of a complex number-
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD[] Csqrt(this ComplexD number)
        {
            ComplexD res0 = ComplexD.CreateRadial(Sqrt(number.Norm), number.Argument / 2);
            ComplexD res1 = ComplexD.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + (double)Constant.Pi);

            return new ComplexD[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th root of a complex number and returns n solutions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD[] Root(this ComplexD number, int n)
        {
            ComplexD[] values = new ComplexD[n];

            double phi = number.Argument / 2;
            double dphi = (double)Constant.PiTimesTwo / (double)n;
            double r = Pow(number.Norm, 1 / n);

            for (int i = 1; i < n; i++)
            {
                values[i] = ComplexD.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }
    }

}
