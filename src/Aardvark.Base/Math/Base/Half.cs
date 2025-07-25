/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aardvark.Base;

// Code by Ladislav Lang: https://sourceforge.net/projects/csharp-half/
// Based on: http://www.fox-toolkit.org/ftp/fasthalffloatconversion.pdf

#region Helpers

/// <summary>
/// Helper class for Half conversions and some low level operations.
/// This class is internally used in the Half class.
/// </summary>
/// <remarks>
/// References:
///     - Fast Half Float Conversions, Jeroen van der Zijp, link: http://www.fox-toolkit.org/ftp/fasthalffloatconversion.pdf
/// </remarks>
[ComVisible(false)]
internal static class HalfHelper
{
    private static readonly uint[] mantissaTable = GenerateMantissaTable();
    private static readonly uint[] exponentTable = GenerateExponentTable();
    private static readonly ushort[] offsetTable = GenerateOffsetTable();
    private static readonly ushort[] baseTable = GenerateBaseTable();
    private static readonly sbyte[] shiftTable = GenerateShiftTable();

    // Transforms the subnormal representation to a normalized one.
    private static uint ConvertMantissa(int i)
    {
        uint m = (uint)(i << 13); // Zero pad mantissa bits
        uint e = 0; // Zero exponent

        // While not normalized
        while ((m & 0x00800000) == 0)
        {
            e -= 0x00800000; // Decrement exponent (1<<23)
            m <<= 1; // Shift mantissa
        }
        m &= unchecked((uint)~0x00800000); // Clear leading 1 bit
        e += 0x38800000; // Adjust bias ((127-14)<<23)
        return m | e; // Return combined number
    }

    private static uint[] GenerateMantissaTable()
    {
        uint[] mantissaTable = new uint[2048];
        mantissaTable[0] = 0;
        for (int i = 1; i < 1024; i++)
        {
            mantissaTable[i] = ConvertMantissa(i);
        }
        for (int i = 1024; i < 2048; i++)
        {
            mantissaTable[i] = (uint)(0x38000000 + ((i - 1024) << 13));
        }

        return mantissaTable;
    }

    private static uint[] GenerateExponentTable()
    {
        uint[] exponentTable = new uint[64];
        exponentTable[0] = 0;
        for (int i = 1; i < 31; i++)
        {
            exponentTable[i] = (uint)(i << 23);
        }
        exponentTable[31] = 0x47800000;
        exponentTable[32] = 0x80000000;
        for (int i = 33; i < 63; i++)
        {
            exponentTable[i] = (uint)(0x80000000 + ((i - 32) << 23));
        }
        exponentTable[63] = 0xc7800000;

        return exponentTable;
    }

    private static ushort[] GenerateOffsetTable()
    {
        ushort[] offsetTable = new ushort[64];
        offsetTable[0] = 0;
        for (int i = 1; i < 32; i++)
        {
            offsetTable[i] = 1024;
        }
        offsetTable[32] = 0;
        for (int i = 33; i < 64; i++)
        {
            offsetTable[i] = 1024;
        }

        return offsetTable;
    }

    private static ushort[] GenerateBaseTable()
    {
        ushort[] baseTable = new ushort[512];

        for (int i = 0; i < 256; ++i)
        {
            int e = i - 127;
            if (e < -24)
            { // Very small numbers map to zero
                baseTable[i | 0x000] = 0x0000;
                baseTable[i | 0x100] = 0x8000;
            }
            else if (e < -14)
            { // Small numbers map to denorms
                baseTable[i | 0x000] = (ushort)(0x0400 >> (-e - 14));
                baseTable[i | 0x100] = (ushort)((0x0400 >> (-e - 14)) | 0x8000);
            }
            else if (e <= 15)
            { // Normal numbers just lose precision
                baseTable[i | 0x000] = (ushort)((e + 15) << 10);
                baseTable[i | 0x100] = (ushort)(((e + 15) << 10) | 0x8000);
            }
            else if (e < 128)
            { // Large numbers map to Infinity
                baseTable[i | 0x000] = 0x7C00;
                baseTable[i | 0x100] = 0xFC00;
            }
            else
            { // Infinity and NaN's stay Infinity and NaN's
                baseTable[i | 0x000] = 0x7C00;
                baseTable[i | 0x100] = 0xFC00;
            }
        }

        return baseTable;

        //ushort[] baseTable = new ushort[512];
        //for (int i = 0; i < 256; ++i)
        //{
        //    sbyte e = (sbyte)(127 - i);
        //    if (e > 24)
        //    { // Very small numbers map to zero
        //        baseTable[i | 0x000] = 0x0000;
        //        baseTable[i | 0x100] = 0x8000;
        //    }
        //    else if (e > 14)
        //    { // Small numbers map to denorms
        //        baseTable[i | 0x000] = (ushort)(0x0400 >> (18 + e));
        //        baseTable[i | 0x100] = (ushort)((0x0400 >> (18 + e)) | 0x8000);
        //    }
        //    else if (e >= -15)
        //    { // Normal numbers just lose precision
        //        baseTable[i | 0x000] = (ushort)((15 - e) << 10);
        //        baseTable[i | 0x100] = (ushort)(((15 - e) << 10) | 0x8000);
        //    }
        //    else if (e > -128)
        //    { // Large numbers map to Infinity
        //        baseTable[i | 0x000] = 0x7c00;
        //        baseTable[i | 0x100] = 0xfc00;
        //    }
        //    else
        //    { // Infinity and NaN's stay Infinity and NaN's
        //        baseTable[i | 0x000] = 0x7c00;
        //        baseTable[i | 0x100] = 0xfc00;
        //    }
        //}

        //return baseTable;
    }

    private static sbyte[] GenerateShiftTable()
    {
        sbyte[] shiftTable = new sbyte[512];

        for (int i = 0; i < 256; ++i)
        {
            int e = i - 127;
            if (e < -24)
            { // Very small numbers map to zero
                shiftTable[i | 0x000] = 24;
                shiftTable[i | 0x100] = 24;
            }
            else if (e < -14)
            { // Small numbers map to denorms
                shiftTable[i | 0x000] = (sbyte)(-e - 1);
                shiftTable[i | 0x100] = (sbyte)(-e - 1);
            }
            else if (e <= 15)
            { // Normal numbers just lose precision
                shiftTable[i | 0x000] = 13;
                shiftTable[i | 0x100] = 13;
            }
            else if (e < 128)
            { // Large numbers map to Infinity
                shiftTable[i | 0x000] = 24;
                shiftTable[i | 0x100] = 24;
            }
            else
            { // Infinity and NaN's stay Infinity and NaN's
                shiftTable[i | 0x000] = 13;
                shiftTable[i | 0x100] = 13;
            }
        }

        return shiftTable;

        //sbyte[] shiftTable = new sbyte[512];
        //for (int i = 0; i < 256; ++i)
        //{
        //    sbyte e = (sbyte)(127 - i);
        //    if (e > 24)
        //    { // Very small numbers map to zero
        //        shiftTable[i | 0x000] = 24;
        //        shiftTable[i | 0x100] = 24;
        //    }
        //    else if (e > 14)
        //    { // Small numbers map to denorms
        //        shiftTable[i | 0x000] = (sbyte)(e - 1);
        //        shiftTable[i | 0x100] = (sbyte)(e - 1);
        //    }
        //    else if (e >= -15)
        //    { // Normal numbers just lose precision
        //        shiftTable[i | 0x000] = 13;
        //        shiftTable[i | 0x100] = 13;
        //    }
        //    else if (e > -128)
        //    { // Large numbers map to Infinity
        //        shiftTable[i | 0x000] = 24;
        //        shiftTable[i | 0x100] = 24;
        //    }
        //    else
        //    { // Infinity and NaN's stay Infinity and NaN's
        //        shiftTable[i | 0x000] = 13;
        //        shiftTable[i | 0x100] = 13;
        //    }
        //}

        //return shiftTable;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe float HalfToSingle(Half half)
    {
        uint result = mantissaTable[offsetTable[half.value >> 10] + (half.value & 0x3ff)] + exponentTable[half.value >> 10];
        return *((float*)&result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Half SingleToHalf(float single)
    {
        uint value = *((uint*)&single);

        ushort result = (ushort)(baseTable[(value >> 23) & 0x1ff] + ((value & 0x007fffff) >> shiftTable[value >> 23]));
        return Half.ToHalf(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Negate(Half half)
        => Half.ToHalf((ushort)(half.value ^ 0x8000));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Abs(Half half)
        => Half.ToHalf((ushort)(half.value & 0x7fff));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(Half half)
        => ((half.value & 0x7fff) > 0x7c00);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInfinity(Half half)
        => ((half.value & 0x7fff) == 0x7c00);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPositiveInfinity(Half half)
        => (half.value == 0x7c00);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegativeInfinity(Half half)
       => (half.value == 0xfc00);
}

#endregion Helper

/// <summary>
/// Represents a half-precision floating point number.
/// </summary>
[Serializable]
public struct Half : IComparable, IFormattable, IConvertible, IComparable<Half>, IEquatable<Half>
{
    /// <summary>
    /// Internal representation of the half-precision floating-point number.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal ushort value;

    #region Constants

    /// <summary>
    /// Represents a half value of zero.
    /// </summary>
    public static Half Zero
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0x0000);
    }

    /// <summary>
    /// Represents a half value of one.
    /// </summary>
    public static Half One
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0x3c00);
    }

    /// <summary>
    /// Represents the smallest positive Half value greater than zero.
    /// </summary>
    public static Half Epsilon
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0x0001);
    }

    /// <summary>
    /// Represents the largest possible value of Half.
    /// </summary>
    public static Half MaxValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0x7bff);
    }

    /// <summary>
    /// Represents the smallest possible value of Half.
    /// </summary>
    public static Half MinValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0xfbff);
    }

    /// <summary>
    /// Represents not a number (NaN).
    /// </summary>
    public static Half NaN
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0xfe00);
    }

    /// <summary>
    /// Represents negative infinity.
    /// </summary>
    public static Half NegativeInfinity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0xfc00);
    }

    /// <summary>
    /// Represents positive infinity.
    /// </summary>
    public static Half PositiveInfinity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Half.ToHalf(0x7c00);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of Half to the value of the specified single-precision floating-point number.
    /// </summary>
    /// <param name="value">The value to represent as a Half.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Half(float value) { this = HalfHelper.SingleToHalf(value); }

    /// <summary>
    /// Initializes a new instance of Half to the value of the specified 32-bit signed integer.
    /// </summary>
    /// <param name="value">The value to represent as a Half.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Half(int value) : this((float)value) { }

    /// <summary>
    /// Initializes a new instance of Half to the value of the specified 64-bit signed integer.
    /// </summary>
    /// <param name="value">The value to represent as a Half.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Half(long value) : this((float)value) { }

    /// <summary>
    /// Initializes a new instance of Half to the value of the specified double-precision floating-point number.
    /// </summary>
    /// <param name="value">The value to represent as a Half.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Half(double value) : this((float)value) { }

    /// <summary>
    /// Initializes a new instance of Half to the value of the specified decimal number.
    /// </summary>
    /// <param name="value">The value to represent as a Half.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Half(decimal value) : this((float)value) { }

    /// <summary>
    /// Initializes a new instance of Half to the value of the specified 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to represent as a Half.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Half(uint value) : this((float)value) { }

    /// <summary>
    /// Initializes a new instance of Half to the value of the specified 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">The value to represent as a Half.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Half(ulong value) : this((float)value) { }

    #endregion

    #region Numeric operators

    /// <summary>
    /// Returns the result of multiplying the specified Half value by negative one.
    /// </summary>
    /// <param name="half">A Half.</param>
    /// <returns>A Half with the value of half, but the opposite sign. -or- Zero, if half is zero.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Negate(Half half) { return -half; }

    /// <summary>
    /// Adds two specified Half values.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>A Half value that is the sum of half1 and half2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Add(Half half1, Half half2) { return half1 + half2; }

    /// <summary>
    /// Subtracts one specified Half value from another.
    /// </summary>
    /// <param name="half1">A Half (the minuend).</param>
    /// <param name="half2">A Half (the subtrahend).</param>
    /// <returns>The Half result of subtracting half2 from half1.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Subtract(Half half1, Half half2) { return half1 - half2; }

    /// <summary>
    /// Multiplies two specified Half values.
    /// </summary>
    /// <param name="half1">A Half (the multiplicand).</param>
    /// <param name="half2">A Half (the multiplier).</param>
    /// <returns>A Half that is the result of multiplying half1 and half2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Multiply(Half half1, Half half2) { return half1 * half2; }

    /// <summary>
    /// Divides two specified Half values.
    /// </summary>
    /// <param name="half1">A Half (the dividend).</param>
    /// <param name="half2">A Half (the divisor).</param>
    /// <returns>The Half that is the result of dividing half1 by half2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Divide(Half half1, Half half2) { return half1 / half2; }

    /// <summary>
    /// Returns the value of the Half operand (the sign of the operand is unchanged).
    /// </summary>
    /// <param name="half">The Half operand.</param>
    /// <returns>The value of the operand, half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator +(Half half) { return half; }

    /// <summary>
    /// Negates the value of the specified Half operand.
    /// </summary>
    /// <param name="half">The Half operand.</param>
    /// <returns>The result of half multiplied by negative one (-1).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator -(Half half) { return HalfHelper.Negate(half); }

    /// <summary>
    /// Increments the Half operand by 1.
    /// </summary>
    /// <param name="half">The Half operand.</param>
    /// <returns>The value of half incremented by 1.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator ++(Half half) { return (Half)(half + 1f); }

    /// <summary>
    /// Decrements the Half operand by one.
    /// </summary>
    /// <param name="half">The Half operand.</param>
    /// <returns>The value of half decremented by 1.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator --(Half half) { return (Half)(half - 1f); }

    /// <summary>
    /// Adds two specified Half values.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>The Half result of adding half1 and half2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator +(Half half1, Half half2) { return (Half)((float)half1 + (float)half2); }

    /// <summary>
    /// Subtracts two specified Half values.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>The Half result of subtracting half1 and half2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator -(Half half1, Half half2) { return (Half)((float)half1 - (float)half2); }

    /// <summary>
    /// Multiplies two specified Half values.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>The Half result of multiplying half1 by half2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator *(Half half1, Half half2) { return (Half)((float)half1 * (float)half2); }

    /// <summary>
    /// Divides two specified Half values.
    /// </summary>
    /// <param name="half1">A Half (the dividend).</param>
    /// <param name="half2">A Half (the divisor).</param>
    /// <returns>The Half result of half1 by half2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half operator /(Half half1, Half half2) { return (Half)((float)half1 / (float)half2); }

    /// <summary>
    /// Returns a value indicating whether two instances of Half are equal.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>true if half1 and half2 are equal; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Half half1, Half half2) { return (!IsNaN(half1) && (half1.value == half2.value)); }

    /// <summary>
    /// Returns a value indicating whether two instances of Half are not equal.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>true if half1 and half2 are not equal; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Half half1, Half half2) { return !(half1.value == half2.value); }

    /// <summary>
    /// Returns a value indicating whether a specified Half is less than another specified Half.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>true if half1 is less than half1; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Half half1, Half half2) { return (float)half1 < (float)half2; }

    /// <summary>
    /// Returns a value indicating whether a specified Half is greater than another specified Half.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>true if half1 is greater than half2; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Half half1, Half half2) { return (float)half1 > (float)half2; }

    /// <summary>
    /// Returns a value indicating whether a specified Half is less than or equal to another specified Half.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>true if half1 is less than or equal to half2; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Half half1, Half half2) { return (half1 == half2) || (half1 < half2); }

    /// <summary>
    /// Returns a value indicating whether a specified Half is greater than or equal to another specified Half.
    /// </summary>
    /// <param name="half1">A Half.</param>
    /// <param name="half2">A Half.</param>
    /// <returns>true if half1 is greater than or equal to half2; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Half half1, Half half2) { return (half1 == half2) || (half1 > half2); }

    #endregion

    #region Type casting operators

    /// <summary>
    /// Converts an 8-bit unsigned integer to a Half.
    /// </summary>
    /// <param name="value">An 8-bit unsigned integer.</param>
    /// <returns>A Half that represents the converted 8-bit unsigned integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(byte value) { return new Half((float)value); }

    /// <summary>
    /// Converts a 16-bit signed integer to a Half.
    /// </summary>
    /// <param name="value">A 16-bit signed integer.</param>
    /// <returns>A Half that represents the converted 16-bit signed integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(short value) { return new Half((float)value); }

    /// <summary>
    /// Converts a Unicode character to a Half.
    /// </summary>
    /// <param name="value">A Unicode character.</param>
    /// <returns>A Half that represents the converted Unicode character.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(char value) { return new Half((float)value); }

    /// <summary>
    /// Converts a 32-bit signed integer to a Half.
    /// </summary>
    /// <param name="value">A 32-bit signed integer.</param>
    /// <returns>A Half that represents the converted 32-bit signed integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(int value) { return new Half((float)value); }

    /// <summary>
    /// Converts a 64-bit signed integer to a Half.
    /// </summary>
    /// <param name="value">A 64-bit signed integer.</param>
    /// <returns>A Half that represents the converted 64-bit signed integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(long value) { return new Half((float)value); }

    /// <summary>
    /// Converts a single-precision floating-point number to a Half.
    /// </summary>
    /// <param name="value">A single-precision floating-point number.</param>
    /// <returns>A Half that represents the converted single-precision floating point number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Half(float value) { return new Half((float)value); }

    /// <summary>
    /// Converts a double-precision floating-point number to a Half.
    /// </summary>
    /// <param name="value">A double-precision floating-point number.</param>
    /// <returns>A Half that represents the converted double-precision floating point number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Half(double value) { return new Half((float)value); }

    /// <summary>
    /// Converts a decimal number to a Half.
    /// </summary>
    /// <param name="value">decimal number</param>
    /// <returns>A Half that represents the converted decimal number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Half(decimal value) { return new Half((float)value); }

    /// <summary>
    /// Converts a Half to an 8-bit unsigned integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>An 8-bit unsigned integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator byte(Half value) { return (byte)(float)value; }

    /// <summary>
    /// Converts a Half to a Unicode character.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A Unicode character that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator char(Half value) { return (char)(float)value; }

    /// <summary>
    /// Converts a Half to a 16-bit signed integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A 16-bit signed integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator short(Half value) { return (short)(float)value; }

    /// <summary>
    /// Converts a Half to a 32-bit signed integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A 32-bit signed integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int(Half value) { return (int)(float)value; }

    /// <summary>
    /// Converts a Half to a 64-bit signed integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A 64-bit signed integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator long(Half value) { return (long)(float)value; }

    /// <summary>
    /// Converts a Half to a single-precision floating-point number.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A single-precision floating-point number that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(Half value) { return (float)HalfHelper.HalfToSingle(value); }

    /// <summary>
    /// Converts a Half to a double-precision floating-point number.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A double-precision floating-point number that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator double(Half value) { return (double)(float)value; }

    /// <summary>
    /// Converts a Half to a decimal number.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A decimal number that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator decimal(Half value) { return (decimal)(float)value; }

    /// <summary>
    /// Converts an 8-bit signed integer to a Half.
    /// </summary>
    /// <param name="value">An 8-bit signed integer.</param>
    /// <returns>A Half that represents the converted 8-bit signed integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(sbyte value) { return new Half((float)value); }

    /// <summary>
    /// Converts a 16-bit unsigned integer to a Half.
    /// </summary>
    /// <param name="value">A 16-bit unsigned integer.</param>
    /// <returns>A Half that represents the converted 16-bit unsigned integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(ushort value) { return new Half((float)value); }

    /// <summary>
    /// Converts a 32-bit unsigned integer to a Half.
    /// </summary>
    /// <param name="value">A 32-bit unsigned integer.</param>
    /// <returns>A Half that represents the converted 32-bit unsigned integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(uint value) { return new Half((float)value); }

    /// <summary>
    /// Converts a 64-bit unsigned integer to a Half.
    /// </summary>
    /// <param name="value">A 64-bit unsigned integer.</param>
    /// <returns>A Half that represents the converted 64-bit unsigned integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Half(ulong value) { return new Half((float)value); }

    /// <summary>
    /// Converts a Half to an 8-bit signed integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>An 8-bit signed integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator sbyte(Half value) { return (sbyte)(float)value; }

    /// <summary>
    /// Converts a Half to a 16-bit unsigned integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A 16-bit unsigned integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator ushort(Half value) { return (ushort)(float)value; }

    /// <summary>
    /// Converts a Half to a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A 32-bit unsigned integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator uint(Half value) { return (uint)(float)value; }

    /// <summary>
    /// Converts a Half to a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">A Half to convert.</param>
    /// <returns>A 64-bit unsigned integer that represents the converted Half.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator ulong(Half value) { return (ulong)(float)value; }

    #endregion

    #region Comparison

    /// <summary>
    /// Compares this instance to a specified Half object.
    /// </summary>
    /// <param name="other">A Half object.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and value.
    /// Return Value Meaning Less than zero This instance is less than value. Zero
    /// This instance is equal to value. Greater than zero This instance is greater than value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(Half other)
    {
        int result = 0;
        if (this < other)
        {
            result = -1;
        }
        else if (this > other)
        {
            result = 1;
        }
        else if (this != other)
        {
            if (!IsNaN(this))
            {
                result = 1;
            }
            else if (!IsNaN(other))
            {
                result = -1;
            }
        }

        return result;
    }

    /// <summary>
    /// Compares this instance to a specified System.Object.
    /// </summary>
    /// <param name="obj">An System.Object or null.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and value.
    /// Return Value Meaning Less than zero This instance is less than value. Zero
    /// This instance is equal to value. Greater than zero This instance is greater
    /// than value. -or- value is null.
    /// </returns>
    /// <exception cref="System.ArgumentException">value is not a Half</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(object obj)
    {
        int result;
        if (obj == null)
        {
            result = 1;
        }
        else
        {
            if (obj is Half half)
            {
                result = CompareTo(half);
            }
            else
            {
                throw new ArgumentException("Object must be of type Half.");
            }
        }

        return result;
    }

    /// <summary>
    /// Returns a value indicating whether this instance and a specified Half object represent the same value.
    /// </summary>
    /// <param name="other">A Half object to compare to this instance.</param>
    /// <returns>true if value is equal to this instance; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Half other)
    {
        return ((other == this) || (IsNaN(other) && IsNaN(this)));
    }

    /// <summary>
    /// Returns a value indicating whether this instance and a specified System.Object
    /// represent the same type and value.
    /// </summary>
    /// <param name="obj">An System.Object.</param>
    /// <returns>true if value is a Half and equal to this instance; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object obj)
    {
        bool result = false;
        if (obj is Half half)
        {
            if ((half == this) || (IsNaN(half) && IsNaN(this)))
            {
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode()
        => value.GetHashCode();

    /// <summary>
    /// Returns the System.TypeCode for value type Half.
    /// </summary>
    /// <returns>The enumerated constant (TypeCode)255.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeCode GetTypeCode() => (TypeCode)255;

    #endregion

    #region BitConverter & Math methods for Half

    /// <summary>
    /// Returns the specified half-precision floating point value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 2.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(Half value)
        => BitConverter.GetBytes(value.value);

    /// <summary>
    /// Converts the value of a specified instance of Half to its equivalent binary representation.
    /// </summary>
    /// <param name="value">A Half value.</param>
    /// <returns>A 16-bit unsigned integer that contain the binary representation of value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetBits(Half value)
        => value.value;

    /// <summary>
    /// Returns a half-precision floating point number converted from two bytes
    /// at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes.</param>
    /// <param name="startIndex">The starting position within value.</param>
    /// <returns>A half-precision floating point number formed by two bytes beginning at startIndex.</returns>
    /// <exception cref="System.ArgumentException">
    /// startIndex is greater than or equal to the length of value minus 1, and is
    /// less than or equal to the length of value minus 1.
    /// </exception>
    /// <exception cref="System.ArgumentNullException">value is null.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">startIndex is less than zero or greater than the length of value minus 1.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(byte[] value, int startIndex)
        => Half.ToHalf((ushort)BitConverter.ToInt16(value, startIndex));

    /// <summary>
    /// Returns a half-precision floating point number converted from its binary representation.
    /// </summary>
    /// <param name="bits">Binary representation of Half value</param>
    /// <returns>A half-precision floating point number formed by its binary representation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(ushort bits)
        => new() { value = bits };

    /// <summary>
    /// Returns a value indicating the sign of a half-precision floating-point number.
    /// </summary>
    /// <param name="value">A signed number.</param>
    /// <returns>
    /// A number indicating the sign of value. Number Description -1 value is less
    /// than zero. 0 value is equal to zero. 1 value is greater than zero.
    /// </returns>
    /// <exception cref="System.ArithmeticException">value is equal to Half.NaN.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(Half value)
    {
        if (value < 0)
        {
            return -1;
        }
        else if (value > 0)
        {
            return 1;
        }
        else
        {
            if (value != 0)
            {
                throw new ArithmeticException("Function does not accept floating point Not-a-Number values.");
            }
        }

        return 0;
    }
    /// <summary>
    /// Returns the absolute value of a half-precision floating-point number.
    /// </summary>
    /// <param name="value">A number in the range Half.MinValue ≤ value ≤ Half.MaxValue.</param>
    /// <returns>A half-precision floating-point number, x, such that 0 ≤ x ≤Half.MaxValue.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Abs(Half value)
        => HalfHelper.Abs(value);

    /// <summary>
    /// Returns the larger of two half-precision floating-point numbers.
    /// </summary>
    /// <param name="value1">The first of two half-precision floating-point numbers to compare.</param>
    /// <param name="value2">The second of two half-precision floating-point numbers to compare.</param>
    /// <returns>
    /// Parameter value1 or value2, whichever is larger. If value1, or value2, or both val1
    /// and value2 are equal to Half.NaN, Half.NaN is returned.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Max(Half value1, Half value2)
        => (value1 < value2) ? value2 : value1;

    /// <summary>
    /// Returns the smaller of two half-precision floating-point numbers.
    /// </summary>
    /// <param name="value1">The first of two half-precision floating-point numbers to compare.</param>
    /// <param name="value2">The second of two half-precision floating-point numbers to compare.</param>
    /// <returns>
    /// Parameter value1 or value2, whichever is smaller. If value1, or value2, or both val1
    /// and value2 are equal to Half.NaN, Half.NaN is returned.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Min(Half value1, Half value2)
        => (value1 < value2) ? value1 : value2;

    #endregion

    #region Special value checks

    /// <summary>
    /// Returns a value indicating whether the specified number evaluates to not a number (Half.NaN).
    /// </summary>
    /// <param name="half">A half-precision floating-point number.</param>
    /// <returns>true if value evaluates to not a number (Half.NaN); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(Half half)
        => HalfHelper.IsNaN(half);

    /// <summary>
    /// Returns a value indicating whether the specified number evaluates to negative or positive infinity.
    /// </summary>
    /// <param name="half">A half-precision floating-point number.</param>
    /// <returns>true if half evaluates to Half.PositiveInfinity or Half.NegativeInfinity; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInfinity(Half half)
        => HalfHelper.IsInfinity(half);

    /// <summary>
    /// Returns a value indicating whether the specified number evaluates to negative infinity.
    /// </summary>
    /// <param name="half">A half-precision floating-point number.</param>
    /// <returns>true if half evaluates to Half.NegativeInfinity; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegativeInfinity(Half half)
        => HalfHelper.IsNegativeInfinity(half);

    /// <summary>
    /// Returns a value indicating whether the specified number evaluates to positive infinity.
    /// </summary>
    /// <param name="half">A half-precision floating-point number.</param>
    /// <returns>true if half evaluates to Half.PositiveInfinity; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPositiveInfinity(Half half)
        => HalfHelper.IsPositiveInfinity(half);

    #endregion

    #region String operations (Parse and ToString)

    /// <summary>
    /// Converts the string representation of a number to its Half equivalent.
    /// </summary>
    /// <param name="value">The string representation of the number to convert.</param>
    /// <returns>The Half number equivalent to the number contained in value.</returns>
    /// <exception cref="System.ArgumentNullException">value is null.</exception>
    /// <exception cref="System.FormatException">value is not in the correct format.</exception>
    /// <exception cref="System.OverflowException">value represents a number less than Half.MinValue or greater than Half.MaxValue.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Parse(string value)
        => (Half)float.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the string representation of a number to its Half equivalent
    /// using the specified culture-specific format information.
    /// </summary>
    /// <param name="value">The string representation of the number to convert.</param>
    /// <param name="provider">An System.IFormatProvider that supplies culture-specific parsing information about value.</param>
    /// <returns>The Half number equivalent to the number contained in s as specified by provider.</returns>
    /// <exception cref="System.ArgumentNullException">value is null.</exception>
    /// <exception cref="System.FormatException">value is not in the correct format.</exception>
    /// <exception cref="System.OverflowException">value represents a number less than Half.MinValue or greater than Half.MaxValue.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Parse(string value, IFormatProvider provider)
        =>(Half)float.Parse(value, provider);

    /// <summary>
    /// Converts the string representation of a number in a specified style to its Half equivalent.
    /// </summary>
    /// <param name="value">The string representation of the number to convert.</param>
    /// <param name="style">
    /// A bitwise combination of System.Globalization.NumberStyles values that indicates
    /// the style elements that can be present in value. A typical value to specify is
    /// System.Globalization.NumberStyles.Number.
    /// </param>
    /// <returns>The Half number equivalent to the number contained in s as specified by style.</returns>
    /// <exception cref="System.ArgumentNullException">value is null.</exception>
    /// <exception cref="System.ArgumentException">
    /// style is not a System.Globalization.NumberStyles value. -or- style is the
    /// System.Globalization.NumberStyles.AllowHexSpecifier value.
    /// </exception>
    /// <exception cref="System.FormatException">value is not in the correct format.</exception>
    /// <exception cref="System.OverflowException">value represents a number less than Half.MinValue or greater than Half.MaxValue.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Parse(string value, NumberStyles style)
        => (Half)float.Parse(value, style, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the string representation of a number to its Half equivalent
    /// using the specified style and culture-specific format.
    /// </summary>
    /// <param name="value">The string representation of the number to convert.</param>
    /// <param name="style">
    /// A bitwise combination of System.Globalization.NumberStyles values that indicates
    /// the style elements that can be present in value. A typical value to specify is
    /// System.Globalization.NumberStyles.Number.
    /// </param>
    /// <param name="provider">An System.IFormatProvider object that supplies culture-specific information about the format of value.</param>
    /// <returns>The Half number equivalent to the number contained in s as specified by style and provider.</returns>
    /// <exception cref="System.ArgumentNullException">value is null.</exception>
    /// <exception cref="System.ArgumentException">
    /// style is not a System.Globalization.NumberStyles value. -or- style is the
    /// System.Globalization.NumberStyles.AllowHexSpecifier value.
    /// </exception>
    /// <exception cref="System.FormatException">value is not in the correct format.</exception>
    /// <exception cref="System.OverflowException">value represents a number less than Half.MinValue or greater than Half.MaxValue.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Half Parse(string value, NumberStyles style, IFormatProvider provider)
        => (Half)float.Parse(value, style, provider);

    /// <summary>
    /// Converts the string representation of a number to its Half equivalent.
    /// A return value indicates whether the conversion succeeded or failed.
    /// </summary>
    /// <param name="value">The string representation of the number to convert.</param>
    /// <param name="result">
    /// When this method returns, contains the Half number that is equivalent
    /// to the numeric value contained in value, if the conversion succeeded, or is zero
    /// if the conversion failed. The conversion fails if the s parameter is null,
    /// is not a number in a valid format, or represents a number less than Half.MinValue
    /// or greater than Half.MaxValue. This parameter is passed uninitialized.
    /// </param>
    /// <returns>true if s was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string value, out Half result)
    {
        if (float.TryParse(value, out float f))
        {
            result = (Half)f;
            return true;
        }

        result = new Half();
        return false;
    }

    /// <summary>
    /// Converts the string representation of a number to its Half equivalent
    /// using the specified style and culture-specific format. A return value indicates
    /// whether the conversion succeeded or failed.
    /// </summary>
    /// <param name="value">The string representation of the number to convert.</param>
    /// <param name="style">
    /// A bitwise combination of System.Globalization.NumberStyles values that indicates
    /// the permitted format of value. A typical value to specify is System.Globalization.NumberStyles.Number.
    /// </param>
    /// <param name="provider">An System.IFormatProvider object that supplies culture-specific parsing information about value.</param>
    /// <param name="result">
    /// When this method returns, contains the Half number that is equivalent
    /// to the numeric value contained in value, if the conversion succeeded, or is zero
    /// if the conversion failed. The conversion fails if the s parameter is null,
    /// is not in a format compliant with style, or represents a number less than
    /// Half.MinValue or greater than Half.MaxValue. This parameter is passed uninitialized.
    /// </param>
    /// <returns>true if s was converted successfully; otherwise, false.</returns>
    /// <exception cref="System.ArgumentException">
    /// style is not a System.Globalization.NumberStyles value. -or- style
    /// is the System.Globalization.NumberStyles.AllowHexSpecifier value.
    /// </exception>
    public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out Half result)
    {
        bool parseResult = false;
        if (float.TryParse(value, style, provider, out float f))
        {
            result = (Half)f;
            parseResult = true;
        }
        else
        {
            result = new Half();
        }

        return parseResult;
    }

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation.
    /// </summary>
    /// <returns>A string that represents the value of this instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
        => ((float)this).ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation
    /// using the specified culture-specific format information.
    /// </summary>
    /// <param name="formatProvider">An System.IFormatProvider that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by provider.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string ToString(IFormatProvider formatProvider)
        => ((float)this).ToString(formatProvider);

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation, using the specified format.
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <returns>The string representation of the value of this instance as specified by format.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string ToString(string format)
        => ((float)this).ToString(format, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the numeric value of this instance to its equivalent string representation
    /// using the specified format and culture-specific format information.
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An System.IFormatProvider that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by format and provider.</returns>
    /// <exception cref="System.FormatException">format is invalid.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string ToString(string format, IFormatProvider formatProvider)
        => ((float)this).ToString(format, formatProvider);

    #endregion

    #region IConvertible Members

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly float IConvertible.ToSingle(IFormatProvider provider)
        => (float)this;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly TypeCode IConvertible.GetTypeCode()
        => Half.GetTypeCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly bool IConvertible.ToBoolean(IFormatProvider provider)
        => Convert.ToBoolean((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly byte IConvertible.ToByte(IFormatProvider provider)
        => Convert.ToByte((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly char IConvertible.ToChar(IFormatProvider provider)
        => throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "Char"));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly DateTime IConvertible.ToDateTime(IFormatProvider provider)
        => throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "DateTime"));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly decimal IConvertible.ToDecimal(IFormatProvider provider)
        => Convert.ToDecimal((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly double IConvertible.ToDouble(IFormatProvider provider)
        => Convert.ToDouble((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly short IConvertible.ToInt16(IFormatProvider provider)
        => Convert.ToInt16((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly int IConvertible.ToInt32(IFormatProvider provider)
        => Convert.ToInt32((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly long IConvertible.ToInt64(IFormatProvider provider)
        => Convert.ToInt64((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly sbyte IConvertible.ToSByte(IFormatProvider provider)
        => Convert.ToSByte((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly string IConvertible.ToString(IFormatProvider provider)
        => Convert.ToString((float)this, CultureInfo.InvariantCulture);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        => (((float)this) as IConvertible).ToType(conversionType, provider);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly ushort IConvertible.ToUInt16(IFormatProvider provider)
        => Convert.ToUInt16((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly uint IConvertible.ToUInt32(IFormatProvider provider)
        => Convert.ToUInt32((float)this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly ulong IConvertible.ToUInt64(IFormatProvider provider)
        => Convert.ToUInt64((float)this);

    #endregion
}