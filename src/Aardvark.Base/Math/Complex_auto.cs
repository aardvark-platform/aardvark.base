using System.Runtime.InteropServices;
using System.Runtime.Serialization;

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

        public static readonly ComplexF Zero = new ComplexF(0, 0);
        public static readonly ComplexF One = new ComplexF(1, 0);
        public static readonly ComplexF I = new ComplexF(0, 1);

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
            get { return (float)System.Math.Sqrt(Real * Real + Imag * Imag); }
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
            get { return (float)System.Math.Atan2(Imag, Real); }
            set
            {
                float r = Norm;

                Real = r * (float)System.Math.Cos(value);
                Imag = r * (float)System.Math.Sin(value);
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

        /// <summary>
        /// Exponentiates this by a real number
        /// </summary>
        /// <param name="scalar"></param>
        public void Pow(float scalar)
        {
            float r = NormSquared;
            float phi = (float)System.Math.Atan2(Imag, Real);

            r = (float)System.Math.Pow(r, scalar);
            phi *= scalar;

            Real = r * (float)System.Math.Cos(phi);
            Imag = r * (float)System.Math.Sin(phi);
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Creates a Radial Complex
        /// </summary>
        /// <param name="r">Norm of the complex number</param>
        /// <param name="phi">Argument of the complex number</param>
        /// <returns></returns>
        public static ComplexF CreateRadial(float r, float phi)
        {
            return new ComplexF(r * (float)System.Math.Cos(phi), r * (float)System.Math.Sin(phi));
        }

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        public static ComplexF CreateOrthogonal(float real, float imag)
        {
            return new ComplexF(real, imag);
        }

        /// <summary>
        /// Exponentiates a complex by a real number
        /// </summary>
        /// <returns></returns>
        public static ComplexF Pow(ComplexF number, float scalar)
        {
            number.Pow(scalar);
            return number;
        }

        /// <summary>
        /// Exponentiates a complex by another
        /// </summary>
        /// <returns></returns>
        public static ComplexF Pow(ComplexF number, ComplexF exponent)
        {
            float r = number.Norm;
            float phi = number.Argument;

            float a = exponent.Real;
            float b = exponent.Imag;

            return ComplexF.CreateRadial((float)System.Math.Exp(System.Math.Log(r) * a - b * phi), a * phi + b * (float)System.Math.Log(r));
        }

        /// <summary>
        /// Natural Logartihm for complex numbers
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static ComplexF Log(ComplexF number)
        {
            return ComplexF.CreateOrthogonal((float)System.Math.Log(number.Norm), number.Argument);
        }

        /// <summary>
        /// Calculates the Square-Root of a real number and returns a Complex
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static ComplexF Sqrt(float number)
        {
            if (number >= 0)
            {
                return new ComplexF((float)System.Math.Sqrt(number), 0.0f);
            }
            else
            {
                return new ComplexF(0.0f, (float)System.Math.Sqrt(-1.0f * number));
            }
        }

        /// <summary>
        /// Calculates both Square-Roots of a complex number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ComplexF[] Sqrt(ComplexF number)
        {
            ComplexF res0 = ComplexF.CreateRadial((float)System.Math.Sqrt(number.Norm), number.Argument / 2.0f);
            ComplexF res1 = ComplexF.CreateRadial((float)System.Math.Sqrt(number.Norm), number.Argument / 2.0f + (float)Constant.Pi);

            return new ComplexF[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th Root of a Complex number and returns n Solutions
        /// </summary>
        /// <param name="number"></param>
        /// <param name="order">n</param>
        /// <returns></returns>
        public ComplexF[] Root(ComplexF number, int order)
        {
            ComplexF[] values = new ComplexF[order];

            float phi = number.Argument / 2.0f;
            float dphi = (float)Constant.PiTimesTwo / (float)order;
            float r = (float)System.Math.Pow(number.Norm, 1.0f / order);

            for (int i = 1; i < order; i++)
            {
                values[i] = ComplexF.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }

        /// <summary>
        /// Calculates e^Complex
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static ComplexF Exp(ComplexF number)
        {
            ComplexF c = ComplexF.Zero;

            float factor = (float)System.Math.Pow(Constant.E, number.Real);

            c.Real = factor * (float)System.Math.Cos(number.Imag);
            c.Imag = factor * (float)System.Math.Sin(number.Imag);

            return c;
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
                    return Real.ToString(format) + " - i" + System.Math.Abs(Imag).ToString(format);
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

        public static readonly ComplexD Zero = new ComplexD(0, 0);
        public static readonly ComplexD One = new ComplexD(1, 0);
        public static readonly ComplexD I = new ComplexD(0, 1);

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
            get { return (double)System.Math.Sqrt(Real * Real + Imag * Imag); }
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
            get { return (double)System.Math.Atan2(Imag, Real); }
            set
            {
                double r = Norm;

                Real = r * (double)System.Math.Cos(value);
                Imag = r * (double)System.Math.Sin(value);
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

        /// <summary>
        /// Exponentiates this by a real number
        /// </summary>
        /// <param name="scalar"></param>
        public void Pow(double scalar)
        {
            double r = NormSquared;
            double phi = (double)System.Math.Atan2(Imag, Real);

            r = (double)System.Math.Pow(r, scalar);
            phi *= scalar;

            Real = r * (double)System.Math.Cos(phi);
            Imag = r * (double)System.Math.Sin(phi);
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Creates a Radial Complex
        /// </summary>
        /// <param name="r">Norm of the complex number</param>
        /// <param name="phi">Argument of the complex number</param>
        /// <returns></returns>
        public static ComplexD CreateRadial(double r, double phi)
        {
            return new ComplexD(r * (double)System.Math.Cos(phi), r * (double)System.Math.Sin(phi));
        }

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        public static ComplexD CreateOrthogonal(double real, double imag)
        {
            return new ComplexD(real, imag);
        }

        /// <summary>
        /// Exponentiates a complex by a real number
        /// </summary>
        /// <returns></returns>
        public static ComplexD Pow(ComplexD number, double scalar)
        {
            number.Pow(scalar);
            return number;
        }

        /// <summary>
        /// Exponentiates a complex by another
        /// </summary>
        /// <returns></returns>
        public static ComplexD Pow(ComplexD number, ComplexD exponent)
        {
            double r = number.Norm;
            double phi = number.Argument;

            double a = exponent.Real;
            double b = exponent.Imag;

            return ComplexD.CreateRadial((double)System.Math.Exp(System.Math.Log(r) * a - b * phi), a * phi + b * (double)System.Math.Log(r));
        }

        /// <summary>
        /// Natural Logartihm for complex numbers
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static ComplexD Log(ComplexD number)
        {
            return ComplexD.CreateOrthogonal((double)System.Math.Log(number.Norm), number.Argument);
        }

        /// <summary>
        /// Calculates the Square-Root of a real number and returns a Complex
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static ComplexD Sqrt(double number)
        {
            if (number >= 0)
            {
                return new ComplexD((double)System.Math.Sqrt(number), 0.0f);
            }
            else
            {
                return new ComplexD(0.0f, (double)System.Math.Sqrt(-1.0f * number));
            }
        }

        /// <summary>
        /// Calculates both Square-Roots of a complex number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ComplexD[] Sqrt(ComplexD number)
        {
            ComplexD res0 = ComplexD.CreateRadial((double)System.Math.Sqrt(number.Norm), number.Argument / 2.0f);
            ComplexD res1 = ComplexD.CreateRadial((double)System.Math.Sqrt(number.Norm), number.Argument / 2.0f + (double)Constant.Pi);

            return new ComplexD[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th Root of a Complex number and returns n Solutions
        /// </summary>
        /// <param name="number"></param>
        /// <param name="order">n</param>
        /// <returns></returns>
        public ComplexD[] Root(ComplexD number, int order)
        {
            ComplexD[] values = new ComplexD[order];

            double phi = number.Argument / 2.0f;
            double dphi = (double)Constant.PiTimesTwo / (double)order;
            double r = (double)System.Math.Pow(number.Norm, 1.0f / order);

            for (int i = 1; i < order; i++)
            {
                values[i] = ComplexD.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }

        /// <summary>
        /// Calculates e^Complex
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static ComplexD Exp(ComplexD number)
        {
            ComplexD c = ComplexD.Zero;

            double factor = (double)System.Math.Pow(Constant.E, number.Real);

            c.Real = factor * (double)System.Math.Cos(number.Imag);
            c.Imag = factor * (double)System.Math.Sin(number.Imag);

            return c;
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
                    return Real.ToString(format) + " - i" + System.Math.Abs(Imag).ToString(format);
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

}
