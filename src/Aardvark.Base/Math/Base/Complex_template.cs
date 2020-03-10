using System;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# var signedtypes = Meta.SignedTypes;
    //# var numtypes = Meta.StandardNumericTypes;
    //# var dreptypes = Meta.DoubleRepresentableTypes;
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? Meta.DoubleType : Meta.FloatType;
    //#   var ft = ftype.Name;
    //#   var ct = isDouble ? "ComplexD" : "ComplexF";
    //#   var cast = isDouble ? "" : "(" + ft + ")";
    //#   var half = isDouble ? "0.5" : "0.5f";
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __ct__
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
        public __ct__ Conjugated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __ct__(Real, -Imag); }
        }

        /// <summary>
        /// Returns the reciprocal of the complex number.
        /// </summary>
        public __ct__ Reciprocal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get 
            {
                __ft__ t = 1 / NormSquared;
                return new __ct__(Real * t, -Imag * t);
            }
        }

        /// <summary>
        /// Returns the squared Gaussian Norm (modulus) of the complex number.
        /// </summary>
        public __ft__ NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Real * Real + Imag * Imag; }
        }

        /// <summary>
        /// Returns the Gaussian Norm (modulus) of the complex number.
        /// </summary>
        public __ft__ Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Sqrt(Real * Real + Imag * Imag); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                __ft__ r = Norm;
                Real = value * Real / r;
                Imag = value * Imag / r;
            }
        }

        /// <summary>
        /// Retruns the argument of the complex number.
        /// </summary>
        public __ft__ Argument
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Fun.Atan2(Imag, Real); }
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
        public bool IsNaN
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (__ft__.IsNaN(Real) || __ft__.IsNaN(Imag)); }
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

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Acos(__ct__ x)
            => x.Acos();

        /// <summary>
        /// Returns the cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Cos(__ct__ x)
            => x.Cos();

        /// <summary>
        /// Returns the hyperbolic cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Cosh(__ct__ x)
            => x.Cosh();

        /// <summary>
        /// Returns the angle that is the arc sine of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Asin(__ct__ x)
            => x.Asin();

        /// <summary>
        /// Returns the sine of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sin(__ct__ x)
            => x.Sin();

        /// <summary>
        /// Returns the hyperbolic sine of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sinh(__ct__ x)
            => x.Sinh();

        /// <summary>
        /// Returns the angle that is the arc tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Atan(__ct__ x)
            => x.Atan();

        /// <summary>
        /// Returns the tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Tan(__ct__ x)
            => x.Tan();

        /// <summary>
        /// Returns the hyperbolic tangent of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Tanh(__ct__ x)
            => x.Tanh();

        /// <summary>
        /// Returns the square root of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Sqrt(__ct__ x)
            => x.Sqrt();

        /// <summary>
        /// Returns e raised to the power of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Exp(__ct__ x)
            => x.Exp();

        /// <summary>
        /// Returns the natural logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log(__ct__ x)
            => x.Log();

        /// <summary>
        /// Returns the base-10 logarithm of the complex number <paramref name="x"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Log10(__ct__ x)
            => x.Log10();

        #endregion

        #region Operators

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
            __ft__ t = 1 / b.NormSquared;
            return new __ct__(
                t * (a.Real * b.Real + a.Imag * b.Imag),
                t * (a.Imag * b.Real - a.Real * b.Imag));
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
            __ft__ t = 1 / b.NormSquared;
            return new __ct__(
                t * (a * b.Real),
                t * (-a * b.Imag));
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

        public override int GetHashCode()
            => HashCode.GetCombined(Real, Imag);

        public override bool Equals(object other)
        {
            if (other is __ct__ obj)
                return Real.Equals(obj.Real) && Imag.Equals(obj.Imag);
            else
                return false;
        }

        /*public override string ToString()
        {
            return ToString("");
        }*/

        public override string ToString(/*string format*/)
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", Real, Imag);
            /*if (!Fun.IsTiny(Real))
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
            }*/
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Power(this __ct__ number, __ct__ exponent)
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
        public static __ct__ Pow(this __ct__ number, __ct__ exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Power(this __ct__ number, __ft__ exponent)
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
        /// Returns the complex number <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Pow(this __ct__ number, __ft__ exponent)
            => Power(number, exponent);

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Power(this __ft__ number, __ct__ exponent)
        {
            if (number == 0)
                return __ct__.Zero;
            else
            {
                __ft__ a = exponent.Real;
                __ft__ b = exponent.Imag;

                if (number < 0)
                {
                    var phi = __cast__Constant.Pi;
                    return __ct__.CreateRadial(Exp(Log(-number) * a - b * phi), a * phi + b * Log(-number));
                }
                else
                    return __ct__.CreateRadial(Pow(number, a), b * Log(number));
            }
        }

        /// <summary>
        /// Returns <paramref name="number"/> raised to the power of <paramref name="exponent"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Pow(this __ft__ number, __ct__ exponent)
            => Power(number, exponent);

        #endregion

        #region Trigonometry

        /// <summary>
        /// Returns the angle that is the arc cosine of the complex number <paramref name="x"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ct__ Acos(this __ct__ x)
        {
            var t = Log(new __ct__(-x.Imag, x.Real) + Sqrt(1 - x * x));
            return new __ct__(-t.Imag + __cast__Constant.PiHalf, t.Real);
        }

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
                return new __ct__(__cast__Constant.PiHalf);
            else if (x == __ct__.NegativeInfinity)
                return new __ct__(-__cast__Constant.PiHalf);
            else
                return new __ct__(0, __half__) * Log((__ct__.I + x) / (__ct__.I - x));
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
            => x.Log() * __cast__Constant.Ln2Inv;

        #endregion

        #region Roots

        /// <summary>
        /// Returns the principal square root of the complex number <paramref name="x"/>.
        /// </summary>
        // https://math.stackexchange.com/a/44500
        // TODO: Check if this is actually better than the naive implementation
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
            __ct__ res1 = __ct__.CreateRadial(Sqrt(number.Norm), number.Argument / 2 + (__ft__)Constant.Pi);

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
            __ft__ dphi = __cast__Constant.PiTimesTwo * invN;
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
        /// Constant{__ft__}.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __ct__ a, __ct__ b)
        {
            return ApproximateEquals(a, b, Constant<__ft__>.PositiveTinyValue);
        }

        #endregion
    }

    //# } // isDouble
}
