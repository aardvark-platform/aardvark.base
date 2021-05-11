using System;

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
                ParseableMinValue = (T)(object)byte.MinValue;
                ParseableMinValue = (T)(object)byte.MinValue;
                ParseableMaxValue = (T)(object)byte.MaxValue;
                MachineEpsilon = (T)(object)(byte)1;
                PositiveTinyValue = (T)(object)(byte)4;
            }
            else if (typeof(T) == typeof(ushort))
            {
                ParseableMinValue = (T)(object)ushort.MinValue;
                ParseableMaxValue = (T)(object)ushort.MaxValue;
                MachineEpsilon = (T)(object)(ushort)1;
                PositiveTinyValue = (T)(object)(ushort)4;
            }
            else if (typeof(T) == typeof(uint))
            {
                ParseableMinValue = (T)(object)uint.MinValue;
                ParseableMaxValue = (T)(object)uint.MaxValue;
                MachineEpsilon = (T)(object)(uint)1;
                PositiveTinyValue = (T)(object)(uint)4;
            }
            else if (typeof(T) == typeof(ulong))
            {
                ParseableMinValue = (T)(object)ulong.MinValue;
                ParseableMaxValue = (T)(object)ulong.MaxValue;
                MachineEpsilon = (T)(object)(ulong)1;
                PositiveTinyValue = (T)(object)(ulong)4;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                ParseableMinValue = (T)(object)sbyte.MinValue;
                ParseableMaxValue = (T)(object)sbyte.MaxValue;
                MachineEpsilon = (T)(object)(sbyte)1;
                PositiveTinyValue = (T)(object)(sbyte)4;
                NegativeTinyValue = (T)(object)(sbyte)-4;
            }
            else if (typeof(T) == typeof(short))
            {
                ParseableMinValue = (T)(object)short.MinValue;
                ParseableMaxValue = (T)(object)short.MaxValue;
                MachineEpsilon = (T)(object)(short)1;
                PositiveTinyValue = (T)(object)(short)4;
                NegativeTinyValue = (T)(object)(short)-4;
            }
            else if (typeof(T) == typeof(int))
            {
                ParseableMinValue = (T)(object)int.MinValue;
                ParseableMaxValue = (T)(object)int.MaxValue;
                MachineEpsilon = (T)(object)1;
                PositiveTinyValue = (T)(object)4;
                NegativeTinyValue = (T)(object)-4;
            }
            else if (typeof(T) == typeof(long))
            {
                ParseableMinValue = (T)(object)long.MinValue;
                ParseableMaxValue = (T)(object)long.MaxValue;
                MachineEpsilon = (T)(object)1L;
                PositiveTinyValue = (T)(object)4L;
                NegativeTinyValue = (T)(object)-4L;
            }
            else if (typeof(T) == typeof(float))
            {
                ParseableMinValue = (T)(object)(float.MinValue * 0.999999f);
                ParseableMaxValue = (T)(object)(float.MaxValue * 0.999999f);
                floatStore = 2.0f;
                float floatEps = 1.0f;
                while (floatStore > 1.0f)
                {
                    floatEps /= 2;
                    floatStore = floatEps + 1.0f;
                }
                MachineEpsilon = (T)(object)(2 * floatEps);
                PositiveTinyValue = (T)(object)(8 * floatEps);
                NegativeTinyValue = (T)(object)(-8 * floatEps);
            }
            else if (typeof(T) == typeof(double))
            {
                ParseableMinValue = (T)(object)(double.MinValue * 0.999999995);
                ParseableMaxValue = (T)(object)(double.MaxValue * 0.999999995);
                double doubleStore = 2.0;
                double doubleEps = 1.0;
                while (doubleStore > 1.0)
                {
                    doubleEps /= 2;
                    doubleStore = doubleEps + 1.0;
                }
                MachineEpsilon = (T)(object)(2 * doubleEps);
                PositiveTinyValue = (T)(object)(8 * doubleEps);
                NegativeTinyValue = (T)(object)(-8 * doubleEps);
            }
        }
    }

    static class Constants
    {
        [OnAardvarkInit]
        public static void Init()
        {
            var ignore1 = Constant<double>.MachineEpsilon;
            var ignore2 = Constant<float>.MachineEpsilon;
            var ignore3 = Constant<byte>.MachineEpsilon;
            var ignore4 = Constant<sbyte>.MachineEpsilon;
            var ignore5 = Constant<short>.MachineEpsilon;
            var ignore6 = Constant<ushort>.MachineEpsilon;
            var ignore7 = Constant<uint>.MachineEpsilon;
            var ignore8 = Constant<int>.MachineEpsilon;
            var ignore9 = Constant<long>.MachineEpsilon;
            var ignore0 = Constant<ulong>.MachineEpsilon;
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

        /// <summary>
        /// One divided by Pi (1 / Pi).
        /// </summary>
        public const double PiInv = 0.318309886183790671537767526745028724068919291480912897495;

        /// <summary>
        /// Ratio of a circle's circumference to its diameter as float
        /// in Euclidean geometry. Also known as Archimedes' constant.
        /// </summary>
        [Obsolete("Use ConstantF.Pi instead")]
        public const float PiF = (float)Pi;

        /// <summary>
        /// Two times PI: the circumference of the unit circle.
        /// </summary>
        public const double PiTimesTwo = 6.283185307179586476925286766559;

        /// <summary>
        /// Three times PI.
        /// </summary>
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
        public const double E = Math.E;

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
        [Obsolete("Use ConstantF.Sqrt2Half instead")]
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
        [Obsolete("Use ConstantF.Ln2 instead")]
        public const float Ln2F = (float)Ln2;

        /// <summary>
        /// 1 divided by logarithm of 2 (base e).
        /// </summary>
        public const double Ln2Inv = 1.4426950408889634073599246810023;

        /// <summary>
        /// 1 divided by logarithm of 2 (base e) as float.
        /// </summary>
        [Obsolete("Use ConstantF.Ln2Inv instead")]
        public const float Ln2InvF = (float)Ln2Inv;

        /// <summary>
        /// 1 divided by 3.
        /// </summary>
        public const double OneThird = 0.33333333333333333333333333333333;

        /// <summary>
        /// 1 divided by 3.
        /// </summary>
        [Obsolete("Use ConstantF.OneThird instead")]
        public const double OneThirdF = (float)OneThird;

        /// <summary>
        /// Used to convert degrees to radians.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const double RadiansPerDegree = Pi / 180.0;

        /// <summary>
        /// Used to convert radians to degrees.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const double DegreesPerRadian = 180.0 / Pi;

        /// <summary>
        /// Used to convert gons to radians.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const double RadiansPerGon = Pi / 200.0;

        /// <summary>
        /// Used to convert radians to gons.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const double GonsPerRadian = 200.0 / Pi;

        /// <summary>
        /// Used to convert gons to degrees.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const double DegreesPerGon = 180.0 / 200.0;

        /// <summary>
        /// Used to convert degrees to gons.
        /// See also <see cref="Conversion"/> class.
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

    /// <summary>
    /// Mathematical constants.
    /// </summary>
    public static class ConstantF
    {
        /// <summary>
        /// Ratio of a circle's circumference to its diameter
        /// in Euclidean geometry. Also known as Archimedes' constant.
        /// </summary>
        public const float Pi = (float)Constant.Pi;

        /// <summary>
        /// One divided by Pi (1 / Pi).
        /// </summary>
        public const float PiInv = (float)Constant.PiInv;

        /// <summary>
        /// Two times PI: the circumference of the unit circle.
        /// </summary>
        public const float PiTimesTwo = (float)Constant.PiTimesTwo;

        /// <summary>
        /// Twree times PI.
        /// </summary>
        public const float PiTimesThree = (float)Constant.PiTimesThree;

        /// <summary>
        /// Four times PI: the surface area of the unit sphere.
        /// </summary>
        public const float PiTimesFour = (float)Constant.PiTimesFour;

        /// <summary>
        /// Half of PI.
        /// </summary>
        public const float PiHalf = (float)Constant.PiHalf;

        /// <summary>
        /// Quarter of PI.
        /// </summary>
        public const float PiQuarter = (float)Constant.PiQuarter;

        /// <summary>
        /// Square of PI.
        /// </summary>
        public const float PiSquared = (float)Constant.PiSquared;

        /// <summary>
        /// Sqrt(2 * PI).
        /// </summary>
        public const float SqrtPiTimesTwo = (float)Constant.SqrtPiTimesTwo;

        /// <summary>
        /// Base of the natural logarithm.
        /// Also known as Euler's number.
        /// </summary>
        public const float E = (float)Constant.E;

        /// <summary>
        /// Golden Ratio.
        /// The golden ratio expresses the relationship that
        /// the sum of two quantities is to the larger quantity
        /// as the larger is to the smaller.
        /// It is defined as (1 + sqrt(5)) / 2).
        /// </summary>
        public const float Phi = (float)Constant.Phi;

        /// <summary>
        /// Square root of 2.
        /// </summary>
        public const float Sqrt2 = (float)Constant.Sqrt2;

        /// <summary>
        /// Square root of 0.5.
        /// </summary>
        public const float Sqrt2Half = (float)Constant.Sqrt2Half;

        /// <summary>
        /// Square root of 3.
        /// </summary>
        public const float Sqrt3 = (float)Constant.Sqrt3;

        /// <summary>
        /// Square root of 5.
        /// </summary>
        public const float Sqrt5 = (float)Constant.Sqrt5;

        /// <summary>
        /// Natural logarithm (base e) of 2.
        /// </summary>
        public const float Ln2 = (float)Constant.Ln2;

        /// <summary>
        /// 1 divided by logarithm of 2 (base e).
        /// </summary>
        public const float Ln2Inv = (float)Constant.Ln2Inv;

        /// <summary>
        /// 1 divided by 3.
        /// </summary>
        public const float OneThird = (float)Constant.OneThird;

        /// <summary>
        /// Used to convert degrees to radians.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const float RadiansPerDegree = (float)Constant.RadiansPerDegree;

        /// <summary>
        /// Used to convert radians to degrees.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const float DegreesPerRadian = (float)Constant.DegreesPerRadian;
        /// <summary>
        /// Used to convert gons to radians.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const float RadiansPerGon = (float)Constant.RadiansPerGon;
        /// <summary>
        /// Used to convert radians to gons.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const float GonsPerRadian = (float)Constant.GonsPerRadian;
        /// <summary>
        /// Used to convert gons to degrees.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const float DegreesPerGon = (float)Constant.DegreesPerGon;

        /// <summary>
        /// Used to convert degrees to gons.
        /// See also <see cref="Conversion"/> class.
        /// </summary>
        public const float GonsPerDegree = (float)Constant.GonsPerDegree;

        /// <summary>
        /// One sixtieth (1/60) of one degree (in radians).
        /// </summary>
        public const float ArcMinute = (float)Constant.ArcMinute;

        /// <summary>
        /// One sixtieth (1/60) of one arc minute (in radians).
        /// </summary>
        public const float ArcSecond = (float)Constant.ArcSecond;

        /// <summary>
        /// The speed of light in meters per second.
        /// </summary>
        public const float SpeedOfLight = (float)Constant.SpeedOfLight;
    }
}
