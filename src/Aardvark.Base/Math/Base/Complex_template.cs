using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var ct = isDouble ? "ComplexD" : "ComplexF";
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __ct__
    {
        [DataMember]
        public __ft__ Real;
        [DataMember]
        public __ft__ Imag;

        #region Constructors

        public __ct__(__ft__ real)
        {
            Real = real;
            Imag = 0;
        }

        public __ct__(__ft__ real, __ft__ imag)
        {
            Real = real;
            Imag = imag;
        }

        #endregion

        #region Constants

        public static readonly __ct__ Zero = new __ct__(0, 0);
        public static readonly __ct__ One = new __ct__(1, 0);
        public static readonly __ct__ I = new __ct__(0, 1);

        #endregion

        #region Properties

        public __ct__ Conjugated
        {
            get { return new __ct__(Real, -Imag); }
        }

        /// <summary>
        /// Squared gaussian Norm (modulus) of the complex number
        /// </summary>
        public __ft__ NormSquared
        {
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Gaussian Norm (modulus) of the complex number
        /// </summary>
        public __ft__ Norm
        {
            get { return (__ft__)System.Math.Sqrt(Real * Real + Imag * Imag); }
            set
            {
                __ft__ r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Argument of the complex number
        /// </summary>
        public __ft__ Argument
        {
            get { return (__ft__)System.Math.Atan2(Imag, Real); }
            set
            {
                __ft__ r = Norm;

                Real = r * (__ft__)System.Math.Cos(value);
                Imag = r * (__ft__)System.Math.Sin(value);
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
            get { return (__ft__.IsNaN(Real) || __ft__.IsNaN(Imag)); }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Adds a complex number to this
        /// </summary>
        /// <param name="number"></param>
        public void Add(__ct__ number)
        {
            Real += number.Real;
            Imag += number.Imag;
        }

        /// <summary>
        /// Adds a real number to this
        /// </summary>
        /// <param name="scalar"></param>
        public void Add(__ft__ scalar)
        {
            Real += scalar;
        }

        /// <summary>
        /// Subtracts a complex number from this
        /// </summary>
        /// <param name="number"></param>
        public void Subtract(__ct__ number)
        {
            Real -= number.Real;
            Imag -= number.Imag;
        }

        /// <summary>
        /// Subtracts a real number from this
        /// </summary>
        /// <param name="scalar"></param>
        public void Subtract(__ft__ scalar)
        {
            Real -= scalar;
        }

        /// <summary>
        /// Multiplies a complex number with this
        /// </summary>
        /// <param name="number"></param>
        public void Multiply(__ct__ number)
        {
            __ft__ real = Real;
            __ft__ imag = Imag;

            Real = real * number.Real - imag * number.Imag;
            Imag = real * number.Imag + imag * number.Real;
        }

        /// <summary>
        /// Multiplies a real number with this
        /// </summary>
        /// <param name="scalar"></param>
        public void Multiply(__ft__ scalar)
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
        public void Divide(__ct__ number)
        {
            __ft__ t = 1 / number.NormSquared;

            __ft__ real = Real;
            __ft__ imag = Imag;

            Real = t * (real * number.Real + imag * number.Imag);
            Imag = t * (imag * number.Real - real * number.Imag);
        }

        /// <summary>
        /// Divides this by a real number
        /// </summary>
        /// <param name="scalar"></param>
        public void Divide(__ft__ scalar)
        {
            Real /= scalar;
            Imag /= scalar;
        }

        /// <summary>
        /// Exponentiates this by a real number
        /// </summary>
        /// <param name="scalar"></param>
        public void Pow(__ft__ scalar)
        {
            __ft__ r = NormSquared;
            __ft__ phi = (__ft__)System.Math.Atan2(Imag, Real);

            r = (__ft__)System.Math.Pow(r, scalar);
            phi *= scalar;

            Real = r * (__ft__)System.Math.Cos(phi);
            Imag = r * (__ft__)System.Math.Sin(phi);
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Creates a Radial Complex
        /// </summary>
        /// <param name="r">Norm of the complex number</param>
        /// <param name="phi">Argument of the complex number</param>
        /// <returns></returns>
        public static __ct__ CreateRadial(__ft__ r, __ft__ phi)
        {
            return new __ct__(r * (__ft__)System.Math.Cos(phi), r * (__ft__)System.Math.Sin(phi));
        }

        /// <summary>
        /// Creates a Orthogonal Complex
        /// </summary>
        /// <param name="real">Real-Part</param>
        /// <param name="imag">Imaginary-Part</param>
        /// <returns></returns>
        public static __ct__ CreateOrthogonal(__ft__ real, __ft__ imag)
        {
            return new __ct__(real, imag);
        }

        /// <summary>
        /// Exponentiates a complex by a real number
        /// </summary>
        /// <returns></returns>
        public static __ct__ Pow(__ct__ number, __ft__ scalar)
        {
            number.Pow(scalar);
            return number;
        }

        /// <summary>
        /// Exponentiates a complex by another
        /// </summary>
        /// <returns></returns>
        public static __ct__ Pow(__ct__ number, __ct__ exponent)
        {
            __ft__ r = number.Norm;
            __ft__ phi = number.Argument;

            __ft__ a = exponent.Real;
            __ft__ b = exponent.Imag;

            return __ct__.CreateRadial((__ft__)System.Math.Exp(System.Math.Log(r) * a - b * phi), a * phi + b * (__ft__)System.Math.Log(r));
        }

        /// <summary>
        /// Natural Logartihm for complex numbers
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static __ct__ Log(__ct__ number)
        {
            return __ct__.CreateOrthogonal((__ft__)System.Math.Log(number.Norm), number.Argument);
        }

        /// <summary>
        /// Calculates the Square-Root of a real number and returns a Complex
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static __ct__ Sqrt(__ft__ number)
        {
            if (number >= 0)
            {
                return new __ct__((__ft__)System.Math.Sqrt(number), 0.0f);
            }
            else
            {
                return new __ct__(0.0f, (__ft__)System.Math.Sqrt(-1.0f * number));
            }
        }

        /// <summary>
        /// Calculates both Square-Roots of a complex number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public __ct__[] Sqrt(__ct__ number)
        {
            __ct__ res0 = __ct__.CreateRadial((__ft__)System.Math.Sqrt(number.Norm), number.Argument / 2.0f);
            __ct__ res1 = __ct__.CreateRadial((__ft__)System.Math.Sqrt(number.Norm), number.Argument / 2.0f + (__ft__)Constant.Pi);

            return new __ct__[2] { res0, res1 };
        }

        /// <summary>
        /// Calculates the n-th Root of a Complex number and returns n Solutions
        /// </summary>
        /// <param name="number"></param>
        /// <param name="order">n</param>
        /// <returns></returns>
        public __ct__[] Root(__ct__ number, int order)
        {
            __ct__[] values = new __ct__[order];

            __ft__ phi = number.Argument / 2.0f;
            __ft__ dphi = (__ft__)Constant.PiTimesTwo / (__ft__)order;
            __ft__ r = (__ft__)System.Math.Pow(number.Norm, 1.0f / order);

            for (int i = 1; i < order; i++)
            {
                values[i] = __ct__.CreateRadial(r, phi + dphi * i);
            }

            return values;
        }

        /// <summary>
        /// Calculates e^Complex
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static __ct__ Exp(__ct__ number)
        {
            __ct__ c = __ct__.Zero;

            __ft__ factor = (__ft__)System.Math.Pow(Constant.E, number.Real);

            c.Real = factor * (__ft__)System.Math.Cos(number.Imag);
            c.Imag = factor * (__ft__)System.Math.Sin(number.Imag);

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

        public static implicit operator __ct__(__ft__ a)
        {
            return new __ct__(a);
        }

        public static __ct__ operator +(__ct__ a, __ct__ b)
        {
            __ct__ c = a;
            c.Add(b);

            return c;
        }

        public static __ct__ operator +(__ct__ a, __ft__ b)
        {
            __ct__ c = a;
            c.Add(b);

            return c;
        }

        public static __ct__ operator +(__ft__ b, __ct__ a)
        {
            return a + b;
        }

        public static __ct__ operator -(__ct__ a, __ct__ b)
        {
            __ct__ c = a;
            c.Subtract(b);

            return c;
        }

        public static __ct__ operator -(__ct__ a, __ft__ b)
        {
            __ct__ c = a;
            c.Subtract(b);

            return c;
        }

        public static __ct__ operator -(__ft__ b, __ct__ a)
        {
            __ct__ c = -1.0f * a;
            c.Add(b);

            return c;
        }

        public static __ct__ operator *(__ct__ a, __ct__ b)
        {
            __ct__ c = a;
            c.Multiply(b);

            return c;
        }

        public static __ct__ operator *(__ct__ a, __ft__ b)
        {
            __ct__ c = a;
            c.Multiply(b);

            return c;
        }

        public static __ct__ operator *(__ft__ b, __ct__ a)
        {
            __ct__ c = a;
            c.Multiply(b);

            return c;
        }

        public static __ct__ operator /(__ct__ a, __ct__ b)
        {
            __ct__ c = a;
            c.Divide(b);

            return c;
        }

        public static __ct__ operator /(__ct__ a, __ft__ b)
        {
            __ct__ c = a;
            c.Divide(b);

            return c;
        }

        public static __ct__ operator /(__ft__ a, __ct__ b)
        {
            __ct__ c = a;
            c.Divide(b);

            return c;
        }

        public static __ct__ operator -(__ct__ a)
        {
            return new __ct__(-a.Real, -a.Imag);
        }

        public static __ct__ operator !(__ct__ a)
        {
            __ct__ c = a;
            c.Conjugate();

            return c;
        }

        #endregion
    }

    //# } // isDouble
}
