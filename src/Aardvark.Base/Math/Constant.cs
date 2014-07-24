using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{

    public enum Metric
    {
        Manhattan = 1,
        Euclidean = 2,
        Maximum = int.MaxValue,

        L1 = Manhattan,
        L2 = Euclidean,
        LInfinity = Maximum,
    }

    public static class Constant<T>
    {
        /// <summary>
        /// A value that is or is close to the minimum value of the type, but
        /// can be parsed back without exception.
        /// </summary>
        public static readonly T ParseableMinValue;

        /// <summary>
        /// A value that is or is close to the maximum value of the type, but
        /// can be parsed back without exception.
        /// </summary>
        public static readonly T ParseableMaxValue;

        /// <summary>
        /// The smallest value that can be added to 1.0 for obtaining a result
        /// that is different from 1.0.
        /// </summary>
        public static readonly T MachineEpsilon;

        /// <summary>
        /// Four times the MachineEpsilon of the type.
        /// </summary>
        public static readonly T PositiveTinyValue;

        /// <summary>
        /// Minus four times the MachineEpsilon of the type.
        /// </summary>
        public static readonly T NegativeTinyValue;

        private static volatile float floatStore;

        static Constant()
        {
            // Console.WriteLine("initializing {0}", typeof(T).Name);

            if (typeof(T) == typeof(byte))
            {
                Constant<byte>.ParseableMinValue = byte.MinValue;
                Constant<byte>.ParseableMaxValue = byte.MaxValue;
                Constant<byte>.MachineEpsilon = (byte)1;
                Constant<byte>.PositiveTinyValue = (byte)4;
            }
            else if (typeof(T) == typeof(ushort))
            {
                Constant<ushort>.ParseableMinValue = ushort.MinValue;
                Constant<ushort>.ParseableMaxValue = ushort.MaxValue;
                Constant<ushort>.MachineEpsilon = (ushort)1;
                Constant<ushort>.PositiveTinyValue = (ushort)4;
            }
            else if (typeof(T) == typeof(uint))
            {
                Constant<uint>.ParseableMinValue = uint.MinValue;
                Constant<uint>.ParseableMaxValue = uint.MaxValue;
                Constant<uint>.MachineEpsilon = (uint)1;
                Constant<uint>.PositiveTinyValue = (uint)4;
            }
            else if (typeof(T) == typeof(ulong))
            {
                Constant<ulong>.ParseableMinValue = ulong.MinValue;
                Constant<ulong>.ParseableMaxValue = ulong.MaxValue;
                Constant<ulong>.MachineEpsilon = (ulong)1;
                Constant<ulong>.PositiveTinyValue = (ulong)4;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                Constant<sbyte>.ParseableMinValue = sbyte.MinValue;
                Constant<sbyte>.ParseableMaxValue = sbyte.MaxValue;
                Constant<sbyte>.MachineEpsilon = (sbyte)1;
                Constant<sbyte>.PositiveTinyValue = (sbyte)4;
                Constant<sbyte>.NegativeTinyValue = (sbyte)-4;
            }
            else if (typeof(T) == typeof(short))
            {
                Constant<short>.ParseableMinValue = short.MinValue;
                Constant<short>.ParseableMaxValue = short.MaxValue;
                Constant<short>.MachineEpsilon = (short)1;
                Constant<short>.PositiveTinyValue = (short)4;
                Constant<short>.NegativeTinyValue = (short)-4;
            }
            else if (typeof(T) == typeof(int))
            {
                Constant<int>.ParseableMinValue = int.MinValue;
                Constant<int>.ParseableMaxValue = int.MaxValue;
                Constant<int>.MachineEpsilon = 1;
                Constant<int>.PositiveTinyValue = 4;
                Constant<int>.NegativeTinyValue = -4;
            }
            else if (typeof(T) == typeof(long))
            {
                Constant<long>.ParseableMinValue = long.MinValue;
                Constant<long>.ParseableMaxValue = long.MaxValue;
                Constant<long>.MachineEpsilon = 1L;
                Constant<long>.PositiveTinyValue = 4L;
                Constant<long>.NegativeTinyValue = -4L;
            }
            else if (typeof(T) == typeof(float))
            {
                Constant<float>.ParseableMinValue = float.MinValue * 0.999999f;
                Constant<float>.ParseableMaxValue = float.MaxValue * 0.999999f;
                floatStore = 2.0f;
                float floatEps = 1.0f;
                while (floatStore > 1.0f)
                {
                    floatEps /= 2;
                    floatStore = floatEps + 1.0f;
                }
                Constant<float>.MachineEpsilon = 2 * floatEps;
                Constant<float>.PositiveTinyValue = 8 * floatEps;
                Constant<float>.NegativeTinyValue = -8 * floatEps;
            }
            else if (typeof(T) == typeof(double))
            {
                Constant<double>.ParseableMinValue = double.MinValue * 0.999999995;
                Constant<double>.ParseableMaxValue = double.MaxValue * 0.999999995;
                double doubleStore = 2.0;
                double doubleEps = 1.0;
                while (doubleStore > 1.0)
                {
                    doubleEps /= 2;
                    doubleStore = doubleEps + 1.0;
                }
                Constant<double>.MachineEpsilon = 2 * doubleEps;
                Constant<double>.PositiveTinyValue = 8 * doubleEps;
                Constant<double>.NegativeTinyValue = -8 * doubleEps;
            }
        }
    }

    /// <summary>
    /// Mathematical constants.
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// Ratio of a circle's circumference to its diameter
        /// in Euclidean geometry. Also known as Archimedes' constant.
        /// </summary>
        public const double Pi = 3.1415926535897932384626433832795;
            // System.Math.PI; System.Math.PI only defined for 5 digits!? (pm)

        /// <summary>
        /// Ratio of a circle's circumference to its diameter as float
        /// in Euclidean geometry. Also known as Archimedes' constant.
        /// </summary>
        public const float PiF = (float)Pi; // System.Math.PI;

        /// <summary>
        /// Two times PI: the circumference of the unit circle.
        /// </summary>
        public const double PiTimesTwo = 6.283185307179586476925286766559;
        public const double PiTimesThree = 9.424777960769379715387930149839;

        /// <summary>
        /// Four times PI: the surface area of the unit sphere.
        /// </summary>
        public const double PiTimesFour = 12.56637061435917295385057353312;

        /// <summary>
        /// Half of PI.
        /// </summary>
        public const double PiHalf = 1.570796326794896619231321691640;

        /// <summary>
        /// Quarter of PI.
        /// </summary>
        public const double PiQuarter = 0.78539816339744830961566084581988;

        /// <summary>
        /// Square of PI.
        /// </summary>
        public const double PiSquared = 9.869604401089358618834490999876;

        /// <summary>
        /// Sqrt(2 * PI).
        /// </summary>
        public const double SqrtPiTimesTwo = 2.5066282746310005024157652848110;

        /// <summary>
        /// Base of the natural logarithm.
        /// Also known as Euler's number.
        /// </summary>
        public const double E = System.Math.E;

        /// <summary>
        /// Golden Ratio.
        /// The golden ratio expresses the relationship that
        /// the sum of two quantities is to the larger quantity
        /// as the larger is to the smaller.
        /// It is defined as (1 + sqrt(5)) / 2).
        /// </summary>
        public const double Phi = 1.61803398874989484820458683;

        /// <summary>
        /// Square root of 2.
        /// </summary>
        public const double Sqrt2 = 1.414213562373095048801688724209;

        /// <summary>
        /// Square root of 0.5.
        /// </summary>
        public const double Sqrt2Half = 0.70710678118654752440084436210485;

        /// <summary>
        /// Square root of 0.5.
        /// </summary>
        public const float Sqrt2HalfF = (float)Sqrt2Half;

        /// <summary>
        /// Square root of 3.
        /// </summary>
        public const double Sqrt3 = 1.732050807568877293527446341505;

        /// <summary>
        /// Square root of 5.
        /// </summary>
        public const double Sqrt5 = 2.236067977499789696409173668731;

        /// <summary>
        /// Natural logarithm (base e) of 2.
        /// </summary>
        public const double Ln2 = 0.69314718055994530941723212145818;

        /// <summary>
        /// Natural logarithm (base e) of 2 as float.
        /// </summary>
        public const float Ln2F = (float)Ln2;

        /// <summary>
        /// 1 divided by logarithm of 2 (base e).
        /// </summary>
        public const double Ln2Inv = 1.4426950408889634073599246810023;

        /// <summary>
        /// 1 divided by logarithm of 2 (base e) as float.
        /// </summary>
        public const float Ln2InvF = (float)Ln2Inv;


        public const double OneThird = 0.33333333333333333333333333333333;

        public const double OneThirdF = (float)OneThird;

        /// <summary>
        /// Used to convert degrees to radians.
        /// See also <see cref="Aardvark.Base.Conversion"/> class.
        /// </summary>
        public const double RadiansPerDegree = Pi / 180.0;

        /// <summary>
        /// Used to convert radians to degrees.
        /// See also <see cref="Aardvark.Base.Conversion"/> class.
        /// </summary>
        public const double DegreesPerRadian = 180.0 / Pi;

        /// <summary>
        /// Used to convert gons to radians.
        /// See also <see cref="Aardvark.Base.Conversion"/> class.
        /// </summary>
        public const double RadiansPerGon = Pi / 200.0;

        /// <summary>
        /// Used to convert radians to gons.
        /// See also <see cref="Aardvark.Base.Conversion"/> class.
        /// </summary>
        public const double GonsPerRadian = 200.0 / Pi;

        /// <summary>
        /// Used to convert gons to degrees.
        /// See also <see cref="Aardvark.Base.Conversion"/> class.
        /// </summary>
        public const double DegreesPerGon = 180.0 / 200.0;

        /// <summary>
        /// Used to convert degrees to gons.
        /// See also <see cref="Aardvark.Base.Conversion"/> class.
        /// </summary>
        public const double GonsPerDegree = 200.0 / 180.0;

        /// <summary>
        /// One sixtieth (1/60) of one degree (in radians).
        /// </summary>
        public const double ArcMinute = Pi / (180 * 60);

        /// <summary>
        /// One sixtieth (1/60) of one arc minute (in radians).
        /// </summary>
        public const double ArcSecond = Pi / (180 * 60 * 60);

        /// <summary>
        /// The speed of light in meters per second.
        /// </summary>
        public const double SpeedOfLight = 299792458.0;
    }

}
