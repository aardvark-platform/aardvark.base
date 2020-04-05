using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aardvark.Base
{

    /// <summary>
    /// Conversion routines.
    /// </summary>
    public static partial class Conversion
    {
        #region Spherical <-> Cartesian

        /// <summary>
        /// Returns unit vector in direction (phi, theta).
        /// Phi is rotation in xy-plane (x-axis is 0, y-axis is pi/2, ...).
        /// Positive theta rotates towards positive z-axis.
        /// </summary>
        public static V3d CartesianFromSpherical(double phi, double theta)
        {
            var s = theta.Cos();
            return new V3d(phi.Cos() * s, phi.Sin() * s, theta.Sin());
        }

        /// <summary>
        /// Returns unit vector in direction (phi, theta).
        /// Phi is rotation in xy-plane (x-axis is 0, y-axis is pi/2, ...).
        /// Positive theta rotates towards positive z-axis.
        /// </summary>
        public static V3d CartesianFromSpherical(this V2d phiAndTheta)
        {
            return CartesianFromSpherical(phiAndTheta.X, phiAndTheta.Y);
        }

        /// <summary>
        /// Returns unit vector in direction phi.
        /// Phi is rotation in xy-plane (x-axis is 0, y-axis is pi/2, ...).
        /// </summary>
        public static V2d CartesianFromSpherical(this double phi)
        {
            return new V2d(phi.Cos(), phi.Sin());
        }

        /// <summary>
        /// Returns spherical direction (phi, theta) from Cartesian direction.
        /// </summary>
        public static V2d SphericalFromCartesian(this V3d v)
        {
            return new V2d(
                Fun.Atan2(v.Y, v.X), // phi
                Fun.Atan2(v.Z, v.XY.Length) // theta
                );
        }

        /// <summary>
        /// Returns spherical direction (phi) from Cartesian direction.
        /// </summary>
        public static double SphericalFromCartesian(this V2d v)
        {
            return Fun.Atan2(v.Y, v.X);
        }

        #endregion

        #region Angles (Radians, Degrees, Gons)

        /// <summary>
        /// Converts an angle given in radians to degrees.
        /// </summary>
        public static float DegreesFromRadians(this float radians)
        {
            return radians * (float)Constant.DegreesPerRadian;
        }

        /// <summary>
        /// Converts an angle given in radians to degrees.
        /// </summary>
        public static double DegreesFromRadians(this double radians)
        {
            return radians * Constant.DegreesPerRadian;
        }

        /// <summary>
        /// Converts an angle given in degrees to radians.
        /// </summary>
        public static float RadiansFromDegrees(this float degrees)
        {
            return degrees * (float)Constant.RadiansPerDegree;
        }

        /// <summary>
        /// Converts an angle given in degrees to radians.
        /// </summary>
        public static double RadiansFromDegrees(this double degrees)
        {
            return degrees * Constant.RadiansPerDegree;
        }

        /// <summary>
        /// Converts an angle given in gons to degrees.
        /// </summary>
        public static float DegreesFromGons(this float gons)
        {
            return gons * (float)Constant.DegreesPerGon;
        }

        /// <summary>
        /// Converts an angle given in gons to degrees.
        /// </summary>
        public static double DegreesFromGons(this double gons)
        {
            return gons * Constant.DegreesPerGon;
        }

        /// <summary>
        /// Converts an angle given in gons to radians.
        /// </summary>
        public static float RadiansFromGons(this float gons)
        {
            return gons * (float)Constant.RadiansPerGon;
        }

        /// <summary>
        /// Converts an angle given in gons to radians.
        /// </summary>
        public static double RadiansFromGons(this double gons)
        {
            return gons * Constant.RadiansPerGon;
        }

        /// <summary>
        /// Converts an angle given in degrees to gons.
        /// </summary>
        public static float GonsFromDegrees(this float degrees)
        {
            return degrees * (float)Constant.GonsPerDegree;
        }

        /// <summary>
        /// Converts an angle given in degrees to gons.
        /// </summary>
        public static double GonsFromDegrees(this double degrees)
        {
            return degrees * Constant.GonsPerDegree;
        }

        /// <summary>
        /// Converts an angle given in radians to gons.
        /// </summary>
        public static float GonsFromRadians(this float radians)
        {
            return radians * (float)Constant.GonsPerRadian;
        }

        /// <summary>
        /// Converts an angle given in radians to gons.
        /// </summary>
        public static double GonsFromRadians(this double radians)
        {
            return radians * Constant.GonsPerRadian;
        }

        #endregion

        #region Temperature

        /// <summary>
        /// Converts a temperature given in Fahrenheit to Celsius.
        /// </summary>
        public static float CelsiusFromFahrenheit(this float fahrenheit)
        {
            return (fahrenheit - 32.0f) / 1.8f;
        }

        /// <summary>
        /// Converts a temperature given in Fahrenheit to Celsius.
        /// </summary>
        public static double CelsiusFromFahrenheit(this double fahrenheit)
        {
            return (fahrenheit - 32.0) / 1.8;
        }

        /// <summary>
        /// Converts a temperature given in Celsius to Fahrenheit.
        /// </summary>
        public static float FahrenheitFromCelsius(this float celsius)
        {
            return (celsius * 1.8f) + 32.0f;
        }

        /// <summary>
        /// Converts a temperature given in Celsius to Fahrenheit.
        /// </summary>
        public static double FahrenheitFromCelsius(this double celsius)
        {
            return (celsius * 1.8) + 32.0;
        }

        /// <summary>
        /// Converts a temperature given in Fahrenheit to Kelvin.
        /// </summary>
        public static float KelvinFromFahrenheit(this float fahrenheit)
        {
            return (fahrenheit + 459.67f) / 1.8f;
        }

        /// <summary>
        /// Converts a temperature given in Fahrenheit to Kelvin.
        /// </summary>
        public static double KelvinFromFahrenheit(this double fahrenheit)
        {
            return (fahrenheit + 459.67) / 1.8;
        }

        /// <summary>
        /// Converts a temperature given in Kelvin to Fahrenheit.
        /// </summary>
        public static float FahrenheitFromKelvin(this float kelvin)
        {
            return (kelvin * 1.8f) - 459.67f;
        }

        /// <summary>
        /// Converts a temperature given in Kelvin to Fahrenheit.
        /// </summary>
        public static double FahrenheitFromKelvin(this double kelvin)
        {
            return (kelvin * 1.8) - 459.67;
        }

        /// <summary>
        /// Converts a temperature given in Kelvin to Celsius.
        /// </summary>
        public static float CelsiusFromKelvin(this float kelvin)
        {
            return kelvin - 273.15f;
        }

        /// <summary>
        /// Converts a temperature given in Kelvin to Celsius.
        /// </summary>
        public static double CelsiusFromKelvin(this double kelvin)
        {
            return kelvin - 273.15;
        }

        /// <summary>
        /// Converts a temperature given in Celsius to Kelvin.
        /// </summary>
        public static float KelvinFromCelsius(this float celsius)
        {
            return celsius + 273.15f;
        }

        /// <summary>
        /// Converts a temperature given in Celsius to Kelvin.
        /// </summary>
        public static double KelvinFromCelsius(this double celsius)
        {
            return celsius + 273.15;
        }

        #endregion

        #region Byte order (little endian / big endian)

        #region HostToNetworkOrder

        public static byte[] HostToNetworkOrder(short x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        public static byte[] HostToNetworkOrder(ushort x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        public static byte[] HostToNetworkOrder(int x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        public static byte[] HostToNetworkOrder(uint x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        public static byte[] HostToNetworkOrder(long x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        public static byte[] HostToNetworkOrder(ulong x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        public static byte[] HostToNetworkOrder(float x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        public static byte[] HostToNetworkOrder(double x)
        {
            byte[] data = BitConverter.GetBytes(x);
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return data;
        }

        #endregion

        #region NetworkToHostOrder

        public static short NetworkToHostOrderInt16(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToInt16(copy, 0);
        }

        public static short NetworkToHostOrderInt16InPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToInt16(data, 0);
        }

        public static ushort NetworkToHostOrderUInt16(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToUInt16(copy, 0);
        }

        public static ushort NetworkToHostOrderUInt16InPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToUInt16(data, 0);
        }

        public static int NetworkToHostOrderInt32(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToInt32(copy, 0);
        }

        public static int NetworkToHostOrderInt32InPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToInt32(data, 0);
        }

        public static uint NetworkToHostOrderUInt32(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToUInt32(copy, 0);
        }

        public static uint NetworkToHostOrderUInt32InPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public static long NetworkToHostOrderInt64(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToInt64(copy, 0);
        }

        public static long NetworkToHostOrderInt64InPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToInt64(data, 0);
        }

        public static ulong NetworkToHostOrderUInt64(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToUInt64(copy, 0);
        }

        public static ulong NetworkToHostOrderUInt64InPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToUInt64(data, 0);
        }

        public static float NetworkToHostOrderSingle(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToSingle(copy, 0);
        }

        public static float NetworkToHostOrderSingleInPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToSingle(data, 0);
        }

        public static double NetworkToHostOrderDouble(byte[] data)
        {
            byte[] copy = (byte[])data.Clone();
            if (BitConverter.IsLittleEndian) ReverseBytes(copy);
            return BitConverter.ToDouble(copy, 0);
        }

        public static double NetworkToHostOrderDoubleInPlace(byte[] data)
        {
            if (BitConverter.IsLittleEndian) ReverseBytes(data);
            return BitConverter.ToDouble(data, 0);
        }

        #endregion

        /// <summary>
        /// Reverses elements of given byte array.
        /// </summary>
        public static void ReverseBytes(byte[] data)
        {
            int length = data.Length;
            int maxIndex = length - 1;
            for (int i = 0; i < length / 2; i++)
            {
                int j = maxIndex - i;
                byte tmp = data[i]; data[i] = data[j]; data[j] = tmp;
            }
        }

        #endregion

        #region Color

        /// <summary>
        /// Converts XYZ-color to Lab-color
        /// </summary>
        /// <param name="xyz"></param>
        /// <returns></returns>
        public static C3f XYZToLab(this C3f xyz)
        {
            var X = xyz.R / XREF;
            var Y = xyz.G / YREF;
            var Z = xyz.B / ZREF;

            var fx = f(X);
            var fy = f(Y);
            var fz = f(Z);

            var L = (116 * fy) - 16;
            var a = 500 * (fx - fy);
            var b = 200 * (fy - fz);
            return new C3f(L, a, b);
        }

        /// <summary>
        /// converts Lab-color to XYZ-color
        /// </summary>
        /// <param name="lab">L=[0,100], a=[-150,150], b=[-150,150]</param>
        /// <returns></returns>
        public static C3f LabToXYZ(this C3f lab)
        {
            var L = lab.R;
            var a = lab.G;
            var b = lab.B;

            var d = (1.0f / 116.0f) * (L + 16);
            var Y = YREF * fInv(d);
            var X = XREF * fInv(d + a / 500.0f);
            var Z = ZREF * fInv(d - b / 200.0f);
            return new C3f(X, Y, Z);
        }

        private static float f(float t)
        {
            if (t > 0.008856)
                return t.Pow(1.0f / 3.0f);
            return (903.3f * t + 16f) / 116f;   //(1.0f / 3.0f) * (float)(29.0f / 6.0f).Square() * t + (4.0f / 29.0f);
        }
        private static float fInv(float t)
        {
            if (t > 0.20689) 
                return t.Pow(3);
            return (t * 116f - 16) / 903.3f;  //3 * (float)(6.0f / 29.0f).Square() * (t - 4.0f / 29.0f);
        }
        private const float XREF = 0.95f; // Illuminant= D65
        private const float YREF = 1f;
        private const float ZREF = 1.09f;

        #endregion
    }

    public static class ColorConversion
    {
        public static C3f FromYuvToYxy(this C3f Yuv)
        {
            var Y = Yuv.R;
            var u = Yuv.G;
            var v = Yuv.B;

            return new C3f(Y,
                           3 * u / (2 * u - 8 * v + 4),
                           2 * v / (2 * u - 8 * v + 4));
        }

        public static C3f FromYxyToXYZ(this C3f Yxy)
        {
            double Y = Yxy.R;
            double x = Yxy.G;
            double y = Yxy.B;

            //AZ: check for y == 0  needed - szabo: DONE
            if (y == 0) y = double.Epsilon;

            return new C3f((x * (Y / y)), Y, ((1 - x - y) * (Y / y)));
        }

        public static C3f FromXYZToYxy(this C3f XYZ)
        {
            double Y = XYZ.G;
            double x = XYZ.R / (XYZ.R + XYZ.G + XYZ.B);
            double y = XYZ.G / (XYZ.R + XYZ.G + XYZ.B);

            return new C3f(Y, x, y);
        }

        /// <summary>
        /// Returns the GamutMap represantion of the supplied C3f.
        /// </summary>
        public static C3f ToGamutMap(this C3f self)
        {
            return self.Clamped(0.0f, 1.0f);
        }

        /// <summary>
        /// Returns the int representation of the supplied C3f color.
        /// </summary>
        public static int FromRgbToInt(this C3f self)
        {
            return Col.ByteFromDouble(self.R) << 16 |
                     Col.ByteFromDouble(self.G) << 8 |
                     Col.ByteFromDouble(self.B);


        }
    }

}
