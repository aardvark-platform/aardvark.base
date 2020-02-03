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

        public ComplexF(float real)
        {
            Real = real;
            Imag = 0;
        }

        public ComplexF(float real, float imag)
        {
            Real = real;
            Imag = imag;
        }

        #endregion

        #region Constants

        public static ComplexF Zero => new ComplexF(0, 0);
        public static ComplexF One => new ComplexF(1, 0);
        public static ComplexF I => new ComplexF(0, 1);

        #endregion

        #region Properties

        public ComplexF Conjugated
        {
            get { return new ComplexF(Real, -Imag); }
        }

        /// <summary>
        /// Squared gaussian Norm (modulus) of the complex number
        /// </summary>
        public float NormSquared
        {
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Gaussian Norm (modulus) of the complex number
        /// </summary>
        public float Norm
        {
            get { return Fun.Sqrt(Real * Real + Imag * Imag); }
            set
            {
                float r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Argument of the complex number
        /// </summary>
        public float Argument
        {
            get { return Fun.Atan2(Imag, Real); }
            set
            {
                float r = Norm;

                Real = r * Fun.Cos(value);
                Imag = r * Fun.Sin(value);
            }
        }

        /// <summary>
        /// Number has no imaginary-part
        /// </summary>
        public bool IsReal
        {
            get { return Fun.IsTiny(Imag); }
        }

        /// <summary>
        /// Number has no real-part
        /// </summary>
        public bool IsImaginary
        {
            get { return Fun.IsTiny(Real); }
        }

        public bool IsOne
        {
            get { return Fun.IsTiny(Real - 1) && Fun.IsTiny(Imag); }
        }

        public bool IsZero
        {
            get { return Fun.IsTiny(Real) && Fun.IsTiny(Imag); }
        }

        public bool IsI
        {
            get { return Fun.IsTiny(Imag - 1) && Fun.IsTiny(Real); }
        }

        /// <summary>
        /// Returns 
        /// </summary>
        public bool IsNan
        {
            get { return (float.IsNaN(Real) || float.IsNaN(Imag)); }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Adds a complex number to this
        /// </summary>
        /// <param name="number"></param>
        public void Add(ComplexF number)
        {
            Real += number.Real;
            Imag += number.Imag;
        }

        /// <summary>
        /// Adds a real number to this
        /// </summary>
        /// <param name="scalar"></param>
        public void Add(float scalar)
        {
            Real += scalar;
        }

        /// <summary>
        /// Subtracts a complex number from this
        /// </summary>
        /// <param name="number"></param>
        public void Subtract(ComplexF number)
        {
            Real -= number.Real;
            Imag -= number.Imag;
        }

        /// <summary>
        /// Subtracts a real number from this
        /// </summary>
        /// <param name="scalar"></param>
        public void Subtract(float scalar)
        {
            Real -= scalar;
        }

        /// <summary>
        /// Multiplies a complex number with this
        /// </summary>
        /// <param name="number"></param>
        public void Multiply(ComplexF number)
        {
            float real = Real;
            float imag = Imag;

            Real = real * number.Real - imag * number.Imag;
            Imag = real * number.Imag + imag * number.Real;
        }

        /// <summary>
        /// Multiplies a real number with this
        /// </summary>
        /// <param name="scalar"></param>
        public void Multiply(float scalar)
        {
            Real *= scalar;
            Imag *= scalar;
        }

        /// <summary>
        /// Conjugates the complex number
        /// </summary>
        public void Conjugate()
        {
            Imag = -Imag;
        }

        /// <summary>
        /// Divides this by a complex number
        /// </summary>
        /// <param name="number"></param>
        public void Divide(ComplexF number)
        {
            float t = 1 / number.NormSquared;

            float real = Real;
            float imag = Imag;

            Real = t * (real * number.Real + imag * number.Imag);
            Imag = t * (imag * number.Real - real * number.Imag);
        }

        /// <summary>
        /// Divides this by a real number
        /// </summary>
        /// <param name="scalar"></param>
        public void Divide(float scalar)
        {
            Real /= scalar;
            Imag /= scalar;
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
        {
            return new ComplexF(r * Fun.Cos(phi), r * Fun.Sin(phi));
        }

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF CreateOrthogonal(float real, float imag)
        {
            return new ComplexF(real, imag);
        }

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

        #region Operators

        public static implicit operator ComplexF(float a)
        {
            return new ComplexF(a);
        }

        public static ComplexF operator +(ComplexF a, ComplexF b)
        {
            ComplexF c = a;
            c.Add(b);

            return c;
        }

        public static ComplexF operator +(ComplexF a, float b)
        {
            ComplexF c = a;
            c.Add(b);

            return c;
        }

        public static ComplexF operator +(float b, ComplexF a)
        {
            return a + b;
        }

        public static ComplexF operator -(ComplexF a, ComplexF b)
        {
            ComplexF c = a;
            c.Subtract(b);

            return c;
        }

        public static ComplexF operator -(ComplexF a, float b)
        {
            ComplexF c = a;
            c.Subtract(b);

            return c;
        }

        public static ComplexF operator -(float b, ComplexF a)
        {
            ComplexF c = -1.0f * a;
            c.Add(b);

            return c;
        }

        public static ComplexF operator *(ComplexF a, ComplexF b)
        {
            ComplexF c = a;
            c.Multiply(b);

            return c;
        }

        public static ComplexF operator *(ComplexF a, float b)
        {
            ComplexF c = a;
            c.Multiply(b);

            return c;
        }

        public static ComplexF operator *(float b, ComplexF a)
        {
            ComplexF c = a;
            c.Multiply(b);

            return c;
        }

        public static ComplexF operator /(ComplexF a, ComplexF b)
        {
            ComplexF c = a;
            c.Divide(b);

            return c;
        }

        public static ComplexF operator /(ComplexF a, float b)
        {
            ComplexF c = a;
            c.Divide(b);

            return c;
        }

        public static ComplexF operator /(float a, ComplexF b)
        {
            ComplexF c = a;
            c.Divide(b);

            return c;
        }

        public static ComplexF operator -(ComplexF a)
        {
            return new ComplexF(-a.Real, -a.Imag);
        }

        public static ComplexF operator !(ComplexF a)
        {
            ComplexF c = a;
            c.Conjugate();

            return c;
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Exponentiates the given complex number by a real number.
        /// </summary>
        public static ComplexF Pow(this ComplexF number, float exponent)
        {
            float r = number.NormSquared;
            float phi = Atan2(number.Imag, number.Real);

            r = Pow(r, exponent);
            phi *= exponent;

            return new ComplexF(r * Cos(phi), r * Sin(phi));
        }

        /// <summary>
        /// Exponentiates a complex number by another.
        /// </summary>
        public static ComplexF Pow(this ComplexF number, ComplexF exponent)
        {
            float r = number.Norm;
            float phi = number.Argument;

            float a = exponent.Real;
            float b = exponent.Imag;

            return ComplexF.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
        }

        /// <summary>
        /// Natural Logartihm for complex numbers
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexF Log(this ComplexF number)
        {
            return ComplexF.CreateOrthogonal(Log(number.Norm), number.Argument);
        }

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
        public static ComplexF[] Csqrt(this ComplexF number)
        {
            ComplexF res0 = ComplexF.CreateRadial(Sqrt(number.Norm), number.Argument / 2);
            ComplexF res1 = ComplexF.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + (float)Constant.Pi);

            return new ComplexF[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th root of a complex number and returns n solutions.
        /// </summary>
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

        /// <summary>
        /// Calculates e^Complex
        /// </summary>
        public static ComplexF Exp(this ComplexF number)
        {
            ComplexF c = ComplexF.Zero;

            float factor = Pow((float)Constant.E, number.Real);

            c.Real = factor * Cos(number.Imag);
            c.Imag = factor * Sin(number.Imag);

            return c;
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

        public ComplexD(double real)
        {
            Real = real;
            Imag = 0;
        }

        public ComplexD(double real, double imag)
        {
            Real = real;
            Imag = imag;
        }

        #endregion

        #region Constants

        public static ComplexD Zero => new ComplexD(0, 0);
        public static ComplexD One => new ComplexD(1, 0);
        public static ComplexD I => new ComplexD(0, 1);

        #endregion

        #region Properties

        public ComplexD Conjugated
        {
            get { return new ComplexD(Real, -Imag); }
        }

        /// <summary>
        /// Squared gaussian Norm (modulus) of the complex number
        /// </summary>
        public double NormSquared
        {
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Gaussian Norm (modulus) of the complex number
        /// </summary>
        public double Norm
        {
            get { return Fun.Sqrt(Real * Real + Imag * Imag); }
            set
            {
                double r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Argument of the complex number
        /// </summary>
        public double Argument
        {
            get { return Fun.Atan2(Imag, Real); }
            set
            {
                double r = Norm;

                Real = r * Fun.Cos(value);
                Imag = r * Fun.Sin(value);
            }
        }

        /// <summary>
        /// Number has no imaginary-part
        /// </summary>
        public bool IsReal
        {
            get { return Fun.IsTiny(Imag); }
        }

        /// <summary>
        /// Number has no real-part
        /// </summary>
        public bool IsImaginary
        {
            get { return Fun.IsTiny(Real); }
        }

        public bool IsOne
        {
            get { return Fun.IsTiny(Real - 1) && Fun.IsTiny(Imag); }
        }

        public bool IsZero
        {
            get { return Fun.IsTiny(Real) && Fun.IsTiny(Imag); }
        }

        public bool IsI
        {
            get { return Fun.IsTiny(Imag - 1) && Fun.IsTiny(Real); }
        }

        /// <summary>
        /// Returns 
        /// </summary>
        public bool IsNan
        {
            get { return (double.IsNaN(Real) || double.IsNaN(Imag)); }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Adds a complex number to this
        /// </summary>
        /// <param name="number"></param>
        public void Add(ComplexD number)
        {
            Real += number.Real;
            Imag += number.Imag;
        }

        /// <summary>
        /// Adds a real number to this
        /// </summary>
        /// <param name="scalar"></param>
        public void Add(double scalar)
        {
            Real += scalar;
        }

        /// <summary>
        /// Subtracts a complex number from this
        /// </summary>
        /// <param name="number"></param>
        public void Subtract(ComplexD number)
        {
            Real -= number.Real;
            Imag -= number.Imag;
        }

        /// <summary>
        /// Subtracts a real number from this
        /// </summary>
        /// <param name="scalar"></param>
        public void Subtract(double scalar)
        {
            Real -= scalar;
        }

        /// <summary>
        /// Multiplies a complex number with this
        /// </summary>
        /// <param name="number"></param>
        public void Multiply(ComplexD number)
        {
            double real = Real;
            double imag = Imag;

            Real = real * number.Real - imag * number.Imag;
            Imag = real * number.Imag + imag * number.Real;
        }

        /// <summary>
        /// Multiplies a real number with this
        /// </summary>
        /// <param name="scalar"></param>
        public void Multiply(double scalar)
        {
            Real *= scalar;
            Imag *= scalar;
        }

        /// <summary>
        /// Conjugates the complex number
        /// </summary>
        public void Conjugate()
        {
            Imag = -Imag;
        }

        /// <summary>
        /// Divides this by a complex number
        /// </summary>
        /// <param name="number"></param>
        public void Divide(ComplexD number)
        {
            double t = 1 / number.NormSquared;

            double real = Real;
            double imag = Imag;

            Real = t * (real * number.Real + imag * number.Imag);
            Imag = t * (imag * number.Real - real * number.Imag);
        }

        /// <summary>
        /// Divides this by a real number
        /// </summary>
        /// <param name="scalar"></param>
        public void Divide(double scalar)
        {
            Real /= scalar;
            Imag /= scalar;
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
        {
            return new ComplexD(r * Fun.Cos(phi), r * Fun.Sin(phi));
        }

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD CreateOrthogonal(double real, double imag)
        {
            return new ComplexD(real, imag);
        }

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

        #region Operators

        public static implicit operator ComplexD(double a)
        {
            return new ComplexD(a);
        }

        public static ComplexD operator +(ComplexD a, ComplexD b)
        {
            ComplexD c = a;
            c.Add(b);

            return c;
        }

        public static ComplexD operator +(ComplexD a, double b)
        {
            ComplexD c = a;
            c.Add(b);

            return c;
        }

        public static ComplexD operator +(double b, ComplexD a)
        {
            return a + b;
        }

        public static ComplexD operator -(ComplexD a, ComplexD b)
        {
            ComplexD c = a;
            c.Subtract(b);

            return c;
        }

        public static ComplexD operator -(ComplexD a, double b)
        {
            ComplexD c = a;
            c.Subtract(b);

            return c;
        }

        public static ComplexD operator -(double b, ComplexD a)
        {
            ComplexD c = -1.0f * a;
            c.Add(b);

            return c;
        }

        public static ComplexD operator *(ComplexD a, ComplexD b)
        {
            ComplexD c = a;
            c.Multiply(b);

            return c;
        }

        public static ComplexD operator *(ComplexD a, double b)
        {
            ComplexD c = a;
            c.Multiply(b);

            return c;
        }

        public static ComplexD operator *(double b, ComplexD a)
        {
            ComplexD c = a;
            c.Multiply(b);

            return c;
        }

        public static ComplexD operator /(ComplexD a, ComplexD b)
        {
            ComplexD c = a;
            c.Divide(b);

            return c;
        }

        public static ComplexD operator /(ComplexD a, double b)
        {
            ComplexD c = a;
            c.Divide(b);

            return c;
        }

        public static ComplexD operator /(double a, ComplexD b)
        {
            ComplexD c = a;
            c.Divide(b);

            return c;
        }

        public static ComplexD operator -(ComplexD a)
        {
            return new ComplexD(-a.Real, -a.Imag);
        }

        public static ComplexD operator !(ComplexD a)
        {
            ComplexD c = a;
            c.Conjugate();

            return c;
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Exponentiates the given complex number by a real number.
        /// </summary>
        public static ComplexD Pow(this ComplexD number, double exponent)
        {
            double r = number.NormSquared;
            double phi = Atan2(number.Imag, number.Real);

            r = Pow(r, exponent);
            phi *= exponent;

            return new ComplexD(r * Cos(phi), r * Sin(phi));
        }

        /// <summary>
        /// Exponentiates a complex number by another.
        /// </summary>
        public static ComplexD Pow(this ComplexD number, ComplexD exponent)
        {
            double r = number.Norm;
            double phi = number.Argument;

            double a = exponent.Real;
            double b = exponent.Imag;

            return ComplexD.CreateRadial(Exp(Log(r) * a - b * phi), a * phi + b * Log(r));
        }

        /// <summary>
        /// Natural Logartihm for complex numbers
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComplexD Log(this ComplexD number)
        {
            return ComplexD.CreateOrthogonal(Log(number.Norm), number.Argument);
        }

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
        public static ComplexD[] Csqrt(this ComplexD number)
        {
            ComplexD res0 = ComplexD.CreateRadial(Sqrt(number.Norm), number.Argument / 2);
            ComplexD res1 = ComplexD.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + (double)Constant.Pi);

            return new ComplexD[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th root of a complex number and returns n solutions.
        /// </summary>
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

        /// <summary>
        /// Calculates e^Complex
        /// </summary>
        public static ComplexD Exp(this ComplexD number)
        {
            ComplexD c = ComplexD.Zero;

            double factor = Pow((double)Constant.E, number.Real);

            c.Real = factor * Cos(number.Imag);
            c.Imag = factor * Sin(number.Imag);

            return c;
        }
    }

}
