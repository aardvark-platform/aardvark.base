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
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Aardvark.Base;

public static partial class TensorAccessors
{
    #region Single channel accessors

    private static ITensorAccessors GetByteAsByte(long[] delta)
    {
        return new TensorAccessors<byte, byte>()
        {
            Getter = (da, i) => (da[i]),
            Setter = (da, i, v) => da[i] = (v),
        };
    }

    private static ITensorAccessors GetByteAsUShort(long[] delta)
    {
        return new TensorAccessors<byte, ushort>()
        {
            Getter = (da, i) => Col.ByteToUShort(da[i]),
            Setter = (da, i, v) => da[i] = Col.UShortToByte(v),
        };
    }

    private static ITensorAccessors GetByteAsUInt(long[] delta)
    {
        return new TensorAccessors<byte, uint>()
        {
            Getter = (da, i) => Col.ByteToUInt(da[i]),
            Setter = (da, i, v) => da[i] = Col.UIntToByte(v),
        };
    }

    private static ITensorAccessors GetByteAsHalf(long[] delta)
    {
        return new TensorAccessors<byte, Half>()
        {
            Getter = (da, i) => Col.ByteToHalf(da[i]),
            Setter = (da, i, v) => da[i] = Col.HalfToByte(v),
        };
    }

    private static ITensorAccessors GetByteAsFloat(long[] delta)
    {
        return new TensorAccessors<byte, float>()
        {
            Getter = (da, i) => Col.ByteToFloat(da[i]),
            Setter = (da, i, v) => da[i] = Col.FloatToByte(v),
        };
    }

    private static ITensorAccessors GetByteAsDouble(long[] delta)
    {
        return new TensorAccessors<byte, double>()
        {
            Getter = (da, i) => Col.ByteToDouble(da[i]),
            Setter = (da, i, v) => da[i] = Col.DoubleToByte(v),
        };
    }

    private static ITensorAccessors GetUShortAsByte(long[] delta)
    {
        return new TensorAccessors<ushort, byte>()
        {
            Getter = (da, i) => Col.UShortToByte(da[i]),
            Setter = (da, i, v) => da[i] = Col.ByteToUShort(v),
        };
    }

    private static ITensorAccessors GetUShortAsUShort(long[] delta)
    {
        return new TensorAccessors<ushort, ushort>()
        {
            Getter = (da, i) => (da[i]),
            Setter = (da, i, v) => da[i] = (v),
        };
    }

    private static ITensorAccessors GetUShortAsUInt(long[] delta)
    {
        return new TensorAccessors<ushort, uint>()
        {
            Getter = (da, i) => Col.UShortToUInt(da[i]),
            Setter = (da, i, v) => da[i] = Col.UIntToUShort(v),
        };
    }

    private static ITensorAccessors GetUShortAsHalf(long[] delta)
    {
        return new TensorAccessors<ushort, Half>()
        {
            Getter = (da, i) => Col.UShortToHalf(da[i]),
            Setter = (da, i, v) => da[i] = Col.HalfToUShort(v),
        };
    }

    private static ITensorAccessors GetUShortAsFloat(long[] delta)
    {
        return new TensorAccessors<ushort, float>()
        {
            Getter = (da, i) => Col.UShortToFloat(da[i]),
            Setter = (da, i, v) => da[i] = Col.FloatToUShort(v),
        };
    }

    private static ITensorAccessors GetUShortAsDouble(long[] delta)
    {
        return new TensorAccessors<ushort, double>()
        {
            Getter = (da, i) => Col.UShortToDouble(da[i]),
            Setter = (da, i, v) => da[i] = Col.DoubleToUShort(v),
        };
    }

    private static ITensorAccessors GetUIntAsByte(long[] delta)
    {
        return new TensorAccessors<uint, byte>()
        {
            Getter = (da, i) => Col.UIntToByte(da[i]),
            Setter = (da, i, v) => da[i] = Col.ByteToUInt(v),
        };
    }

    private static ITensorAccessors GetUIntAsUShort(long[] delta)
    {
        return new TensorAccessors<uint, ushort>()
        {
            Getter = (da, i) => Col.UIntToUShort(da[i]),
            Setter = (da, i, v) => da[i] = Col.UShortToUInt(v),
        };
    }

    private static ITensorAccessors GetUIntAsUInt(long[] delta)
    {
        return new TensorAccessors<uint, uint>()
        {
            Getter = (da, i) => (da[i]),
            Setter = (da, i, v) => da[i] = (v),
        };
    }

    private static ITensorAccessors GetUIntAsHalf(long[] delta)
    {
        return new TensorAccessors<uint, Half>()
        {
            Getter = (da, i) => Col.UIntToHalf(da[i]),
            Setter = (da, i, v) => da[i] = Col.HalfToUInt(v),
        };
    }

    private static ITensorAccessors GetUIntAsFloat(long[] delta)
    {
        return new TensorAccessors<uint, float>()
        {
            Getter = (da, i) => Col.UIntToFloat(da[i]),
            Setter = (da, i, v) => da[i] = Col.FloatToUInt(v),
        };
    }

    private static ITensorAccessors GetUIntAsDouble(long[] delta)
    {
        return new TensorAccessors<uint, double>()
        {
            Getter = (da, i) => Col.UIntToDouble(da[i]),
            Setter = (da, i, v) => da[i] = Col.DoubleToUInt(v),
        };
    }

    private static ITensorAccessors GetHalfAsByte(long[] delta)
    {
        return new TensorAccessors<Half, byte>()
        {
            Getter = (da, i) => Col.HalfToByte(da[i]),
            Setter = (da, i, v) => da[i] = Col.ByteToHalf(v),
        };
    }

    private static ITensorAccessors GetHalfAsUShort(long[] delta)
    {
        return new TensorAccessors<Half, ushort>()
        {
            Getter = (da, i) => Col.HalfToUShort(da[i]),
            Setter = (da, i, v) => da[i] = Col.UShortToHalf(v),
        };
    }

    private static ITensorAccessors GetHalfAsUInt(long[] delta)
    {
        return new TensorAccessors<Half, uint>()
        {
            Getter = (da, i) => Col.HalfToUInt(da[i]),
            Setter = (da, i, v) => da[i] = Col.UIntToHalf(v),
        };
    }

    private static ITensorAccessors GetHalfAsHalf(long[] delta)
    {
        return new TensorAccessors<Half, Half>()
        {
            Getter = (da, i) => (da[i]),
            Setter = (da, i, v) => da[i] = (v),
        };
    }

    private static ITensorAccessors GetHalfAsFloat(long[] delta)
    {
        return new TensorAccessors<Half, float>()
        {
            Getter = (da, i) => Col.HalfToFloat(da[i]),
            Setter = (da, i, v) => da[i] = Col.FloatToHalf(v),
        };
    }

    private static ITensorAccessors GetHalfAsDouble(long[] delta)
    {
        return new TensorAccessors<Half, double>()
        {
            Getter = (da, i) => Col.HalfToDouble(da[i]),
            Setter = (da, i, v) => da[i] = Col.DoubleToHalf(v),
        };
    }

    private static ITensorAccessors GetFloatAsByte(long[] delta)
    {
        return new TensorAccessors<float, byte>()
        {
            Getter = (da, i) => Col.FloatToByte(da[i]),
            Setter = (da, i, v) => da[i] = Col.ByteToFloat(v),
        };
    }

    private static ITensorAccessors GetFloatAsUShort(long[] delta)
    {
        return new TensorAccessors<float, ushort>()
        {
            Getter = (da, i) => Col.FloatToUShort(da[i]),
            Setter = (da, i, v) => da[i] = Col.UShortToFloat(v),
        };
    }

    private static ITensorAccessors GetFloatAsUInt(long[] delta)
    {
        return new TensorAccessors<float, uint>()
        {
            Getter = (da, i) => Col.FloatToUInt(da[i]),
            Setter = (da, i, v) => da[i] = Col.UIntToFloat(v),
        };
    }

    private static ITensorAccessors GetFloatAsHalf(long[] delta)
    {
        return new TensorAccessors<float, Half>()
        {
            Getter = (da, i) => Col.FloatToHalf(da[i]),
            Setter = (da, i, v) => da[i] = Col.HalfToFloat(v),
        };
    }

    private static ITensorAccessors GetFloatAsFloat(long[] delta)
    {
        return new TensorAccessors<float, float>()
        {
            Getter = (da, i) => (da[i]),
            Setter = (da, i, v) => da[i] = (v),
        };
    }

    private static ITensorAccessors GetFloatAsDouble(long[] delta)
    {
        return new TensorAccessors<float, double>()
        {
            Getter = (da, i) => Col.FloatToDouble(da[i]),
            Setter = (da, i, v) => da[i] = Col.DoubleToFloat(v),
        };
    }

    private static ITensorAccessors GetDoubleAsByte(long[] delta)
    {
        return new TensorAccessors<double, byte>()
        {
            Getter = (da, i) => Col.DoubleToByte(da[i]),
            Setter = (da, i, v) => da[i] = Col.ByteToDouble(v),
        };
    }

    private static ITensorAccessors GetDoubleAsUShort(long[] delta)
    {
        return new TensorAccessors<double, ushort>()
        {
            Getter = (da, i) => Col.DoubleToUShort(da[i]),
            Setter = (da, i, v) => da[i] = Col.UShortToDouble(v),
        };
    }

    private static ITensorAccessors GetDoubleAsUInt(long[] delta)
    {
        return new TensorAccessors<double, uint>()
        {
            Getter = (da, i) => Col.DoubleToUInt(da[i]),
            Setter = (da, i, v) => da[i] = Col.UIntToDouble(v),
        };
    }

    private static ITensorAccessors GetDoubleAsHalf(long[] delta)
    {
        return new TensorAccessors<double, Half>()
        {
            Getter = (da, i) => Col.DoubleToHalf(da[i]),
            Setter = (da, i, v) => da[i] = Col.HalfToDouble(v),
        };
    }

    private static ITensorAccessors GetDoubleAsFloat(long[] delta)
    {
        return new TensorAccessors<double, float>()
        {
            Getter = (da, i) => Col.DoubleToFloat(da[i]),
            Setter = (da, i, v) => da[i] = Col.FloatToDouble(v),
        };
    }

    private static ITensorAccessors GetDoubleAsDouble(long[] delta)
    {
        return new TensorAccessors<double, double>()
        {
            Getter = (da, i) => (da[i]),
            Setter = (da, i, v) => da[i] = (v),
        };
    }

    #endregion

    private static readonly Dictionary<(Type, Type, Symbol),
                                       Func<long[], ITensorAccessors>> s_creatorMap
        = new Dictionary<(Type, Type, Symbol),
                         Func<long[], ITensorAccessors>>()
        {
            #region Single channel byte as byte

            { (typeof(byte), typeof(byte), Intent.ColorChannel), GetByteAsByte },
            { (typeof(byte), typeof(byte), Intent.BW), GetByteAsByte },
            { (typeof(byte), typeof(byte), Intent.Gray), GetByteAsByte },
            { (typeof(byte), typeof(byte), Intent.Alpha), GetByteAsByte },

            #endregion

            #region Single channel byte as ushort

            { (typeof(byte), typeof(ushort), Intent.ColorChannel), GetByteAsUShort },
            { (typeof(byte), typeof(ushort), Intent.BW), GetByteAsUShort },
            { (typeof(byte), typeof(ushort), Intent.Gray), GetByteAsUShort },
            { (typeof(byte), typeof(ushort), Intent.Alpha), GetByteAsUShort },

            #endregion

            #region Single channel byte as uint

            { (typeof(byte), typeof(uint), Intent.ColorChannel), GetByteAsUInt },
            { (typeof(byte), typeof(uint), Intent.BW), GetByteAsUInt },
            { (typeof(byte), typeof(uint), Intent.Gray), GetByteAsUInt },
            { (typeof(byte), typeof(uint), Intent.Alpha), GetByteAsUInt },

            #endregion

            #region Single channel byte as Half

            { (typeof(byte), typeof(Half), Intent.ColorChannel), GetByteAsHalf },
            { (typeof(byte), typeof(Half), Intent.BW), GetByteAsHalf },
            { (typeof(byte), typeof(Half), Intent.Gray), GetByteAsHalf },
            { (typeof(byte), typeof(Half), Intent.Alpha), GetByteAsHalf },

            #endregion

            #region Single channel byte as float

            { (typeof(byte), typeof(float), Intent.ColorChannel), GetByteAsFloat },
            { (typeof(byte), typeof(float), Intent.BW), GetByteAsFloat },
            { (typeof(byte), typeof(float), Intent.Gray), GetByteAsFloat },
            { (typeof(byte), typeof(float), Intent.Alpha), GetByteAsFloat },

            #endregion

            #region Single channel byte as double

            { (typeof(byte), typeof(double), Intent.ColorChannel), GetByteAsDouble },
            { (typeof(byte), typeof(double), Intent.BW), GetByteAsDouble },
            { (typeof(byte), typeof(double), Intent.Gray), GetByteAsDouble },
            { (typeof(byte), typeof(double), Intent.Alpha), GetByteAsDouble },

            #endregion

            #region Single channel ushort as byte

            { (typeof(ushort), typeof(byte), Intent.ColorChannel), GetUShortAsByte },
            { (typeof(ushort), typeof(byte), Intent.BW), GetUShortAsByte },
            { (typeof(ushort), typeof(byte), Intent.Gray), GetUShortAsByte },
            { (typeof(ushort), typeof(byte), Intent.Alpha), GetUShortAsByte },

            #endregion

            #region Single channel ushort as ushort

            { (typeof(ushort), typeof(ushort), Intent.ColorChannel), GetUShortAsUShort },
            { (typeof(ushort), typeof(ushort), Intent.BW), GetUShortAsUShort },
            { (typeof(ushort), typeof(ushort), Intent.Gray), GetUShortAsUShort },
            { (typeof(ushort), typeof(ushort), Intent.Alpha), GetUShortAsUShort },

            #endregion

            #region Single channel ushort as uint

            { (typeof(ushort), typeof(uint), Intent.ColorChannel), GetUShortAsUInt },
            { (typeof(ushort), typeof(uint), Intent.BW), GetUShortAsUInt },
            { (typeof(ushort), typeof(uint), Intent.Gray), GetUShortAsUInt },
            { (typeof(ushort), typeof(uint), Intent.Alpha), GetUShortAsUInt },

            #endregion

            #region Single channel ushort as Half

            { (typeof(ushort), typeof(Half), Intent.ColorChannel), GetUShortAsHalf },
            { (typeof(ushort), typeof(Half), Intent.BW), GetUShortAsHalf },
            { (typeof(ushort), typeof(Half), Intent.Gray), GetUShortAsHalf },
            { (typeof(ushort), typeof(Half), Intent.Alpha), GetUShortAsHalf },

            #endregion

            #region Single channel ushort as float

            { (typeof(ushort), typeof(float), Intent.ColorChannel), GetUShortAsFloat },
            { (typeof(ushort), typeof(float), Intent.BW), GetUShortAsFloat },
            { (typeof(ushort), typeof(float), Intent.Gray), GetUShortAsFloat },
            { (typeof(ushort), typeof(float), Intent.Alpha), GetUShortAsFloat },

            #endregion

            #region Single channel ushort as double

            { (typeof(ushort), typeof(double), Intent.ColorChannel), GetUShortAsDouble },
            { (typeof(ushort), typeof(double), Intent.BW), GetUShortAsDouble },
            { (typeof(ushort), typeof(double), Intent.Gray), GetUShortAsDouble },
            { (typeof(ushort), typeof(double), Intent.Alpha), GetUShortAsDouble },

            #endregion

            #region Single channel uint as byte

            { (typeof(uint), typeof(byte), Intent.ColorChannel), GetUIntAsByte },
            { (typeof(uint), typeof(byte), Intent.BW), GetUIntAsByte },
            { (typeof(uint), typeof(byte), Intent.Gray), GetUIntAsByte },
            { (typeof(uint), typeof(byte), Intent.Alpha), GetUIntAsByte },

            #endregion

            #region Single channel uint as ushort

            { (typeof(uint), typeof(ushort), Intent.ColorChannel), GetUIntAsUShort },
            { (typeof(uint), typeof(ushort), Intent.BW), GetUIntAsUShort },
            { (typeof(uint), typeof(ushort), Intent.Gray), GetUIntAsUShort },
            { (typeof(uint), typeof(ushort), Intent.Alpha), GetUIntAsUShort },

            #endregion

            #region Single channel uint as uint

            { (typeof(uint), typeof(uint), Intent.ColorChannel), GetUIntAsUInt },
            { (typeof(uint), typeof(uint), Intent.BW), GetUIntAsUInt },
            { (typeof(uint), typeof(uint), Intent.Gray), GetUIntAsUInt },
            { (typeof(uint), typeof(uint), Intent.Alpha), GetUIntAsUInt },

            #endregion

            #region Single channel uint as Half

            { (typeof(uint), typeof(Half), Intent.ColorChannel), GetUIntAsHalf },
            { (typeof(uint), typeof(Half), Intent.BW), GetUIntAsHalf },
            { (typeof(uint), typeof(Half), Intent.Gray), GetUIntAsHalf },
            { (typeof(uint), typeof(Half), Intent.Alpha), GetUIntAsHalf },

            #endregion

            #region Single channel uint as float

            { (typeof(uint), typeof(float), Intent.ColorChannel), GetUIntAsFloat },
            { (typeof(uint), typeof(float), Intent.BW), GetUIntAsFloat },
            { (typeof(uint), typeof(float), Intent.Gray), GetUIntAsFloat },
            { (typeof(uint), typeof(float), Intent.Alpha), GetUIntAsFloat },

            #endregion

            #region Single channel uint as double

            { (typeof(uint), typeof(double), Intent.ColorChannel), GetUIntAsDouble },
            { (typeof(uint), typeof(double), Intent.BW), GetUIntAsDouble },
            { (typeof(uint), typeof(double), Intent.Gray), GetUIntAsDouble },
            { (typeof(uint), typeof(double), Intent.Alpha), GetUIntAsDouble },

            #endregion

            #region Single channel Half as byte

            { (typeof(Half), typeof(byte), Intent.ColorChannel), GetHalfAsByte },
            { (typeof(Half), typeof(byte), Intent.BW), GetHalfAsByte },
            { (typeof(Half), typeof(byte), Intent.Gray), GetHalfAsByte },
            { (typeof(Half), typeof(byte), Intent.Alpha), GetHalfAsByte },

            #endregion

            #region Single channel Half as ushort

            { (typeof(Half), typeof(ushort), Intent.ColorChannel), GetHalfAsUShort },
            { (typeof(Half), typeof(ushort), Intent.BW), GetHalfAsUShort },
            { (typeof(Half), typeof(ushort), Intent.Gray), GetHalfAsUShort },
            { (typeof(Half), typeof(ushort), Intent.Alpha), GetHalfAsUShort },

            #endregion

            #region Single channel Half as uint

            { (typeof(Half), typeof(uint), Intent.ColorChannel), GetHalfAsUInt },
            { (typeof(Half), typeof(uint), Intent.BW), GetHalfAsUInt },
            { (typeof(Half), typeof(uint), Intent.Gray), GetHalfAsUInt },
            { (typeof(Half), typeof(uint), Intent.Alpha), GetHalfAsUInt },

            #endregion

            #region Single channel Half as Half

            { (typeof(Half), typeof(Half), Intent.ColorChannel), GetHalfAsHalf },
            { (typeof(Half), typeof(Half), Intent.BW), GetHalfAsHalf },
            { (typeof(Half), typeof(Half), Intent.Gray), GetHalfAsHalf },
            { (typeof(Half), typeof(Half), Intent.Alpha), GetHalfAsHalf },

            #endregion

            #region Single channel Half as float

            { (typeof(Half), typeof(float), Intent.ColorChannel), GetHalfAsFloat },
            { (typeof(Half), typeof(float), Intent.BW), GetHalfAsFloat },
            { (typeof(Half), typeof(float), Intent.Gray), GetHalfAsFloat },
            { (typeof(Half), typeof(float), Intent.Alpha), GetHalfAsFloat },

            #endregion

            #region Single channel Half as double

            { (typeof(Half), typeof(double), Intent.ColorChannel), GetHalfAsDouble },
            { (typeof(Half), typeof(double), Intent.BW), GetHalfAsDouble },
            { (typeof(Half), typeof(double), Intent.Gray), GetHalfAsDouble },
            { (typeof(Half), typeof(double), Intent.Alpha), GetHalfAsDouble },

            #endregion

            #region Single channel float as byte

            { (typeof(float), typeof(byte), Intent.ColorChannel), GetFloatAsByte },
            { (typeof(float), typeof(byte), Intent.BW), GetFloatAsByte },
            { (typeof(float), typeof(byte), Intent.Gray), GetFloatAsByte },
            { (typeof(float), typeof(byte), Intent.Alpha), GetFloatAsByte },

            #endregion

            #region Single channel float as ushort

            { (typeof(float), typeof(ushort), Intent.ColorChannel), GetFloatAsUShort },
            { (typeof(float), typeof(ushort), Intent.BW), GetFloatAsUShort },
            { (typeof(float), typeof(ushort), Intent.Gray), GetFloatAsUShort },
            { (typeof(float), typeof(ushort), Intent.Alpha), GetFloatAsUShort },

            #endregion

            #region Single channel float as uint

            { (typeof(float), typeof(uint), Intent.ColorChannel), GetFloatAsUInt },
            { (typeof(float), typeof(uint), Intent.BW), GetFloatAsUInt },
            { (typeof(float), typeof(uint), Intent.Gray), GetFloatAsUInt },
            { (typeof(float), typeof(uint), Intent.Alpha), GetFloatAsUInt },

            #endregion

            #region Single channel float as Half

            { (typeof(float), typeof(Half), Intent.ColorChannel), GetFloatAsHalf },
            { (typeof(float), typeof(Half), Intent.BW), GetFloatAsHalf },
            { (typeof(float), typeof(Half), Intent.Gray), GetFloatAsHalf },
            { (typeof(float), typeof(Half), Intent.Alpha), GetFloatAsHalf },

            #endregion

            #region Single channel float as float

            { (typeof(float), typeof(float), Intent.ColorChannel), GetFloatAsFloat },
            { (typeof(float), typeof(float), Intent.BW), GetFloatAsFloat },
            { (typeof(float), typeof(float), Intent.Gray), GetFloatAsFloat },
            { (typeof(float), typeof(float), Intent.Alpha), GetFloatAsFloat },

            #endregion

            #region Single channel float as double

            { (typeof(float), typeof(double), Intent.ColorChannel), GetFloatAsDouble },
            { (typeof(float), typeof(double), Intent.BW), GetFloatAsDouble },
            { (typeof(float), typeof(double), Intent.Gray), GetFloatAsDouble },
            { (typeof(float), typeof(double), Intent.Alpha), GetFloatAsDouble },

            #endregion

            #region Single channel double as byte

            { (typeof(double), typeof(byte), Intent.ColorChannel), GetDoubleAsByte },
            { (typeof(double), typeof(byte), Intent.BW), GetDoubleAsByte },
            { (typeof(double), typeof(byte), Intent.Gray), GetDoubleAsByte },
            { (typeof(double), typeof(byte), Intent.Alpha), GetDoubleAsByte },

            #endregion

            #region Single channel double as ushort

            { (typeof(double), typeof(ushort), Intent.ColorChannel), GetDoubleAsUShort },
            { (typeof(double), typeof(ushort), Intent.BW), GetDoubleAsUShort },
            { (typeof(double), typeof(ushort), Intent.Gray), GetDoubleAsUShort },
            { (typeof(double), typeof(ushort), Intent.Alpha), GetDoubleAsUShort },

            #endregion

            #region Single channel double as uint

            { (typeof(double), typeof(uint), Intent.ColorChannel), GetDoubleAsUInt },
            { (typeof(double), typeof(uint), Intent.BW), GetDoubleAsUInt },
            { (typeof(double), typeof(uint), Intent.Gray), GetDoubleAsUInt },
            { (typeof(double), typeof(uint), Intent.Alpha), GetDoubleAsUInt },

            #endregion

            #region Single channel double as Half

            { (typeof(double), typeof(Half), Intent.ColorChannel), GetDoubleAsHalf },
            { (typeof(double), typeof(Half), Intent.BW), GetDoubleAsHalf },
            { (typeof(double), typeof(Half), Intent.Gray), GetDoubleAsHalf },
            { (typeof(double), typeof(Half), Intent.Alpha), GetDoubleAsHalf },

            #endregion

            #region Single channel double as float

            { (typeof(double), typeof(float), Intent.ColorChannel), GetDoubleAsFloat },
            { (typeof(double), typeof(float), Intent.BW), GetDoubleAsFloat },
            { (typeof(double), typeof(float), Intent.Gray), GetDoubleAsFloat },
            { (typeof(double), typeof(float), Intent.Alpha), GetDoubleAsFloat },

            #endregion

            #region Single channel double as double

            { (typeof(double), typeof(double), Intent.ColorChannel), GetDoubleAsDouble },
            { (typeof(double), typeof(double), Intent.BW), GetDoubleAsDouble },
            { (typeof(double), typeof(double), Intent.Gray), GetDoubleAsDouble },
            { (typeof(double), typeof(double), Intent.Alpha), GetDoubleAsDouble },

            #endregion

            #region RGB bytes as C3b

            {
                (typeof(byte), typeof(C3b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C3b

            {
                (typeof(byte), typeof(C3b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C3b

            {
                (typeof(byte), typeof(C3b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C3b

            {
                (typeof(byte), typeof(C3b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C3us

            {
                (typeof(byte), typeof(C3us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C3us

            {
                (typeof(byte), typeof(C3us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i+2]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i+d2]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C3us

            {
                (typeof(byte), typeof(C3us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.B);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.B);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C3us

            {
                (typeof(byte), typeof(C3us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i+2]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.R);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.ByteToUShort(da[i+d2]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.R);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C3ui

            {
                (typeof(byte), typeof(C3ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C3ui

            {
                (typeof(byte), typeof(C3ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i+2]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i+d2]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C3ui

            {
                (typeof(byte), typeof(C3ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.B);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.B);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C3ui

            {
                (typeof(byte), typeof(C3ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i+2]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.R);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.ByteToUInt(da[i+d2]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.R);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C3f

            {
                (typeof(byte), typeof(C3f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C3f

            {
                (typeof(byte), typeof(C3f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i+2]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i+d2]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C3f

            {
                (typeof(byte), typeof(C3f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.B);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.B);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C3f

            {
                (typeof(byte), typeof(C3f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i+2]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.R);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.ByteToFloat(da[i+d2]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.R);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C3d

            {
                (typeof(byte), typeof(C3d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C3d

            {
                (typeof(byte), typeof(C3d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i+2]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i+d2]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C3d

            {
                (typeof(byte), typeof(C3d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.B);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.B);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C3d

            {
                (typeof(byte), typeof(C3d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i+2]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.R);
                                da[i+3] = (byte)255;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.ByteToDouble(da[i+d2]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.R);
                                da[i+d3] = (byte)255;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C4b

            {
                (typeof(byte), typeof(C4b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C4b

            {
                (typeof(byte), typeof(C4b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C4b

            {
                (typeof(byte), typeof(C4b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C4b

            {
                (typeof(byte), typeof(C4b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C4us

            {
                (typeof(byte), typeof(C4us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i+2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i+d2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C4us

            {
                (typeof(byte), typeof(C4us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i+2]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i+d2]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C4us

            {
                (typeof(byte), typeof(C4us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i+2]), 
                                        Col.ByteToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.B);
                                da[i+3] = Col.UShortToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i+d2]), 
                                        Col.ByteToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.R);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.B);
                                da[i+d3] = Col.UShortToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C4us

            {
                (typeof(byte), typeof(C4us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i+2]), 
                                        Col.ByteToUShort(da[i+1]), 
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+1] = Col.UShortToByte(v.G);
                                da[i+2] = Col.UShortToByte(v.R);
                                da[i+3] = Col.UShortToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.ByteToUShort(da[i+d2]), 
                                        Col.ByteToUShort(da[i+d1]), 
                                        Col.ByteToUShort(da[i]), 
                                        Col.ByteToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToByte(v.B);
                                da[i+d1] = Col.UShortToByte(v.G);
                                da[i+d2] = Col.UShortToByte(v.R);
                                da[i+d3] = Col.UShortToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C4ui

            {
                (typeof(byte), typeof(C4ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i+2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i+d2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C4ui

            {
                (typeof(byte), typeof(C4ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i+2]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i+d2]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C4ui

            {
                (typeof(byte), typeof(C4ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i+2]), 
                                        Col.ByteToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.B);
                                da[i+3] = Col.UIntToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i+d2]), 
                                        Col.ByteToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.R);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.B);
                                da[i+d3] = Col.UIntToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C4ui

            {
                (typeof(byte), typeof(C4ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i+2]), 
                                        Col.ByteToUInt(da[i+1]), 
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+1] = Col.UIntToByte(v.G);
                                da[i+2] = Col.UIntToByte(v.R);
                                da[i+3] = Col.UIntToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.ByteToUInt(da[i+d2]), 
                                        Col.ByteToUInt(da[i+d1]), 
                                        Col.ByteToUInt(da[i]), 
                                        Col.ByteToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToByte(v.B);
                                da[i+d1] = Col.UIntToByte(v.G);
                                da[i+d2] = Col.UIntToByte(v.R);
                                da[i+d3] = Col.UIntToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C4f

            {
                (typeof(byte), typeof(C4f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i+2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i+d2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C4f

            {
                (typeof(byte), typeof(C4f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i+2]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i+d2]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C4f

            {
                (typeof(byte), typeof(C4f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i+2]), 
                                        Col.ByteToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.B);
                                da[i+3] = Col.FloatToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i+d2]), 
                                        Col.ByteToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.R);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.B);
                                da[i+d3] = Col.FloatToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C4f

            {
                (typeof(byte), typeof(C4f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i+2]), 
                                        Col.ByteToFloat(da[i+1]), 
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+1] = Col.FloatToByte(v.G);
                                da[i+2] = Col.FloatToByte(v.R);
                                da[i+3] = Col.FloatToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.ByteToFloat(da[i+d2]), 
                                        Col.ByteToFloat(da[i+d1]), 
                                        Col.ByteToFloat(da[i]), 
                                        Col.ByteToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToByte(v.B);
                                da[i+d1] = Col.FloatToByte(v.G);
                                da[i+d2] = Col.FloatToByte(v.R);
                                da[i+d3] = Col.FloatToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB bytes as C4d

            {
                (typeof(byte), typeof(C4d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i+2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i+d2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR bytes as C4d

            {
                (typeof(byte), typeof(C4d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i+2]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i+d2]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA bytes as C4d

            {
                (typeof(byte), typeof(C4d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i+2]), 
                                        Col.ByteToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.B);
                                da[i+3] = Col.DoubleToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i+d2]), 
                                        Col.ByteToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.R);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.B);
                                da[i+d3] = Col.DoubleToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA bytes as C4d

            {
                (typeof(byte), typeof(C4d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i+2]), 
                                        Col.ByteToDouble(da[i+1]), 
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+1] = Col.DoubleToByte(v.G);
                                da[i+2] = Col.DoubleToByte(v.R);
                                da[i+3] = Col.DoubleToByte(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<byte, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.ByteToDouble(da[i+d2]), 
                                        Col.ByteToDouble(da[i+d1]), 
                                        Col.ByteToDouble(da[i]), 
                                        Col.ByteToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToByte(v.B);
                                da[i+d1] = Col.DoubleToByte(v.G);
                                da[i+d2] = Col.DoubleToByte(v.R);
                                da[i+d3] = Col.DoubleToByte(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C3b

            {
                (typeof(ushort), typeof(C3b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C3b

            {
                (typeof(ushort), typeof(C3b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i+2]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i+d2]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C3b

            {
                (typeof(ushort), typeof(C3b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.B);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.B);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C3b

            {
                (typeof(ushort), typeof(C3b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i+2]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.R);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UShortToByte(da[i+d2]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.R);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C3us

            {
                (typeof(ushort), typeof(C3us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C3us

            {
                (typeof(ushort), typeof(C3us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C3us

            {
                (typeof(ushort), typeof(C3us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C3us

            {
                (typeof(ushort), typeof(C3us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C3ui

            {
                (typeof(ushort), typeof(C3ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C3ui

            {
                (typeof(ushort), typeof(C3ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i+2]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i+d2]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C3ui

            {
                (typeof(ushort), typeof(C3ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.B);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.B);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C3ui

            {
                (typeof(ushort), typeof(C3ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i+2]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.R);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.UShortToUInt(da[i+d2]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.R);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C3f

            {
                (typeof(ushort), typeof(C3f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C3f

            {
                (typeof(ushort), typeof(C3f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i+2]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i+d2]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C3f

            {
                (typeof(ushort), typeof(C3f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.B);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.B);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C3f

            {
                (typeof(ushort), typeof(C3f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i+2]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.R);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UShortToFloat(da[i+d2]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.R);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C3d

            {
                (typeof(ushort), typeof(C3d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C3d

            {
                (typeof(ushort), typeof(C3d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i+2]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i+d2]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C3d

            {
                (typeof(ushort), typeof(C3d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.B);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.B);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C3d

            {
                (typeof(ushort), typeof(C3d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i+2]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.R);
                                da[i+3] = (ushort)65535;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UShortToDouble(da[i+d2]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.R);
                                da[i+d3] = (ushort)65535;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C4b

            {
                (typeof(ushort), typeof(C4b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i+2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i+d2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C4b

            {
                (typeof(ushort), typeof(C4b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i+2]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i+d2]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C4b

            {
                (typeof(ushort), typeof(C4b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i+2]), 
                                        Col.UShortToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.B);
                                da[i+3] = Col.ByteToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i+d2]), 
                                        Col.UShortToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.R);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.B);
                                da[i+d3] = Col.ByteToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C4b

            {
                (typeof(ushort), typeof(C4b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i+2]), 
                                        Col.UShortToByte(da[i+1]), 
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+1] = Col.ByteToUShort(v.G);
                                da[i+2] = Col.ByteToUShort(v.R);
                                da[i+3] = Col.ByteToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UShortToByte(da[i+d2]), 
                                        Col.UShortToByte(da[i+d1]), 
                                        Col.UShortToByte(da[i]), 
                                        Col.UShortToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUShort(v.B);
                                da[i+d1] = Col.ByteToUShort(v.G);
                                da[i+d2] = Col.ByteToUShort(v.R);
                                da[i+d3] = Col.ByteToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C4us

            {
                (typeof(ushort), typeof(C4us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C4us

            {
                (typeof(ushort), typeof(C4us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C4us

            {
                (typeof(ushort), typeof(C4us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C4us

            {
                (typeof(ushort), typeof(C4us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C4ui

            {
                (typeof(ushort), typeof(C4ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i+2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i+d2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C4ui

            {
                (typeof(ushort), typeof(C4ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i+2]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i+d2]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C4ui

            {
                (typeof(ushort), typeof(C4ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i+2]), 
                                        Col.UShortToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.B);
                                da[i+3] = Col.UIntToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i+d2]), 
                                        Col.UShortToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.R);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.B);
                                da[i+d3] = Col.UIntToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C4ui

            {
                (typeof(ushort), typeof(C4ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i+2]), 
                                        Col.UShortToUInt(da[i+1]), 
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+1] = Col.UIntToUShort(v.G);
                                da[i+2] = Col.UIntToUShort(v.R);
                                da[i+3] = Col.UIntToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.UShortToUInt(da[i+d2]), 
                                        Col.UShortToUInt(da[i+d1]), 
                                        Col.UShortToUInt(da[i]), 
                                        Col.UShortToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToUShort(v.B);
                                da[i+d1] = Col.UIntToUShort(v.G);
                                da[i+d2] = Col.UIntToUShort(v.R);
                                da[i+d3] = Col.UIntToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C4f

            {
                (typeof(ushort), typeof(C4f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i+2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i+d2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C4f

            {
                (typeof(ushort), typeof(C4f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i+2]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i+d2]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C4f

            {
                (typeof(ushort), typeof(C4f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i+2]), 
                                        Col.UShortToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.B);
                                da[i+3] = Col.FloatToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i+d2]), 
                                        Col.UShortToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.R);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.B);
                                da[i+d3] = Col.FloatToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C4f

            {
                (typeof(ushort), typeof(C4f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i+2]), 
                                        Col.UShortToFloat(da[i+1]), 
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+1] = Col.FloatToUShort(v.G);
                                da[i+2] = Col.FloatToUShort(v.R);
                                da[i+3] = Col.FloatToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UShortToFloat(da[i+d2]), 
                                        Col.UShortToFloat(da[i+d1]), 
                                        Col.UShortToFloat(da[i]), 
                                        Col.UShortToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUShort(v.B);
                                da[i+d1] = Col.FloatToUShort(v.G);
                                da[i+d2] = Col.FloatToUShort(v.R);
                                da[i+d3] = Col.FloatToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB ushorts as C4d

            {
                (typeof(ushort), typeof(C4d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i+2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i+d2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR ushorts as C4d

            {
                (typeof(ushort), typeof(C4d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i+2]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i+d2]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA ushorts as C4d

            {
                (typeof(ushort), typeof(C4d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i+2]), 
                                        Col.UShortToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.B);
                                da[i+3] = Col.DoubleToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i+d2]), 
                                        Col.UShortToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.R);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.B);
                                da[i+d3] = Col.DoubleToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA ushorts as C4d

            {
                (typeof(ushort), typeof(C4d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i+2]), 
                                        Col.UShortToDouble(da[i+1]), 
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+1] = Col.DoubleToUShort(v.G);
                                da[i+2] = Col.DoubleToUShort(v.R);
                                da[i+3] = Col.DoubleToUShort(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<ushort, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UShortToDouble(da[i+d2]), 
                                        Col.UShortToDouble(da[i+d1]), 
                                        Col.UShortToDouble(da[i]), 
                                        Col.UShortToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUShort(v.B);
                                da[i+d1] = Col.DoubleToUShort(v.G);
                                da[i+d2] = Col.DoubleToUShort(v.R);
                                da[i+d3] = Col.DoubleToUShort(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C3b

            {
                (typeof(uint), typeof(C3b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C3b

            {
                (typeof(uint), typeof(C3b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i+2]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i+d2]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C3b

            {
                (typeof(uint), typeof(C3b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.B);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.B);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C3b

            {
                (typeof(uint), typeof(C3b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i+2]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.R);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.UIntToByte(da[i+d2]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.R);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C3us

            {
                (typeof(uint), typeof(C3us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C3us

            {
                (typeof(uint), typeof(C3us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i+2]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i+d2]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C3us

            {
                (typeof(uint), typeof(C3us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.B);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.B);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C3us

            {
                (typeof(uint), typeof(C3us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i+2]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.R);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.UIntToUShort(da[i+d2]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.R);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C3ui

            {
                (typeof(uint), typeof(C3ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C3ui

            {
                (typeof(uint), typeof(C3ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C3ui

            {
                (typeof(uint), typeof(C3ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C3ui

            {
                (typeof(uint), typeof(C3ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C3f

            {
                (typeof(uint), typeof(C3f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C3f

            {
                (typeof(uint), typeof(C3f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i+2]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i+d2]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C3f

            {
                (typeof(uint), typeof(C3f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.B);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.B);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C3f

            {
                (typeof(uint), typeof(C3f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i+2]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.R);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.UIntToFloat(da[i+d2]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.R);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C3d

            {
                (typeof(uint), typeof(C3d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C3d

            {
                (typeof(uint), typeof(C3d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i+2]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i+d2]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C3d

            {
                (typeof(uint), typeof(C3d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.B);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.B);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C3d

            {
                (typeof(uint), typeof(C3d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i+2]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.R);
                                da[i+3] = (uint)UInt32.MaxValue;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.UIntToDouble(da[i+d2]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.R);
                                da[i+d3] = (uint)UInt32.MaxValue;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C4b

            {
                (typeof(uint), typeof(C4b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i+2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i+d2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C4b

            {
                (typeof(uint), typeof(C4b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i+2]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i+d2]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C4b

            {
                (typeof(uint), typeof(C4b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i+2]), 
                                        Col.UIntToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.B);
                                da[i+3] = Col.ByteToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i+d2]), 
                                        Col.UIntToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.R);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.B);
                                da[i+d3] = Col.ByteToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C4b

            {
                (typeof(uint), typeof(C4b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i+2]), 
                                        Col.UIntToByte(da[i+1]), 
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+1] = Col.ByteToUInt(v.G);
                                da[i+2] = Col.ByteToUInt(v.R);
                                da[i+3] = Col.ByteToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.UIntToByte(da[i+d2]), 
                                        Col.UIntToByte(da[i+d1]), 
                                        Col.UIntToByte(da[i]), 
                                        Col.UIntToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToUInt(v.B);
                                da[i+d1] = Col.ByteToUInt(v.G);
                                da[i+d2] = Col.ByteToUInt(v.R);
                                da[i+d3] = Col.ByteToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C4us

            {
                (typeof(uint), typeof(C4us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i+2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i+d2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C4us

            {
                (typeof(uint), typeof(C4us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i+2]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i+d2]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C4us

            {
                (typeof(uint), typeof(C4us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i+2]), 
                                        Col.UIntToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.B);
                                da[i+3] = Col.UShortToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i+d2]), 
                                        Col.UIntToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.R);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.B);
                                da[i+d3] = Col.UShortToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C4us

            {
                (typeof(uint), typeof(C4us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i+2]), 
                                        Col.UIntToUShort(da[i+1]), 
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+1] = Col.UShortToUInt(v.G);
                                da[i+2] = Col.UShortToUInt(v.R);
                                da[i+3] = Col.UShortToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.UIntToUShort(da[i+d2]), 
                                        Col.UIntToUShort(da[i+d1]), 
                                        Col.UIntToUShort(da[i]), 
                                        Col.UIntToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToUInt(v.B);
                                da[i+d1] = Col.UShortToUInt(v.G);
                                da[i+d2] = Col.UShortToUInt(v.R);
                                da[i+d3] = Col.UShortToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C4ui

            {
                (typeof(uint), typeof(C4ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C4ui

            {
                (typeof(uint), typeof(C4ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C4ui

            {
                (typeof(uint), typeof(C4ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C4ui

            {
                (typeof(uint), typeof(C4ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C4f

            {
                (typeof(uint), typeof(C4f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i+2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i+d2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C4f

            {
                (typeof(uint), typeof(C4f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i+2]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i+d2]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C4f

            {
                (typeof(uint), typeof(C4f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i+2]), 
                                        Col.UIntToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.B);
                                da[i+3] = Col.FloatToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i+d2]), 
                                        Col.UIntToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.R);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.B);
                                da[i+d3] = Col.FloatToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C4f

            {
                (typeof(uint), typeof(C4f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i+2]), 
                                        Col.UIntToFloat(da[i+1]), 
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+1] = Col.FloatToUInt(v.G);
                                da[i+2] = Col.FloatToUInt(v.R);
                                da[i+3] = Col.FloatToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.UIntToFloat(da[i+d2]), 
                                        Col.UIntToFloat(da[i+d1]), 
                                        Col.UIntToFloat(da[i]), 
                                        Col.UIntToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToUInt(v.B);
                                da[i+d1] = Col.FloatToUInt(v.G);
                                da[i+d2] = Col.FloatToUInt(v.R);
                                da[i+d3] = Col.FloatToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB uints as C4d

            {
                (typeof(uint), typeof(C4d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i+2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i+d2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR uints as C4d

            {
                (typeof(uint), typeof(C4d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i+2]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i+d2]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA uints as C4d

            {
                (typeof(uint), typeof(C4d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i+2]), 
                                        Col.UIntToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.B);
                                da[i+3] = Col.DoubleToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i+d2]), 
                                        Col.UIntToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.R);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.B);
                                da[i+d3] = Col.DoubleToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA uints as C4d

            {
                (typeof(uint), typeof(C4d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i+2]), 
                                        Col.UIntToDouble(da[i+1]), 
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+1] = Col.DoubleToUInt(v.G);
                                da[i+2] = Col.DoubleToUInt(v.R);
                                da[i+3] = Col.DoubleToUInt(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<uint, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.UIntToDouble(da[i+d2]), 
                                        Col.UIntToDouble(da[i+d1]), 
                                        Col.UIntToDouble(da[i]), 
                                        Col.UIntToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToUInt(v.B);
                                da[i+d1] = Col.DoubleToUInt(v.G);
                                da[i+d2] = Col.DoubleToUInt(v.R);
                                da[i+d3] = Col.DoubleToUInt(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C3b

            {
                (typeof(Half), typeof(C3b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C3b

            {
                (typeof(Half), typeof(C3b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i+2]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i+d2]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C3b

            {
                (typeof(Half), typeof(C3b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.B);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.B);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C3b

            {
                (typeof(Half), typeof(C3b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i+2]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.R);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.HalfToByte(da[i+d2]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.R);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C3us

            {
                (typeof(Half), typeof(C3us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C3us

            {
                (typeof(Half), typeof(C3us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i+2]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i+d2]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C3us

            {
                (typeof(Half), typeof(C3us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.B);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.B);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C3us

            {
                (typeof(Half), typeof(C3us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i+2]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.R);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.HalfToUShort(da[i+d2]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.R);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C3ui

            {
                (typeof(Half), typeof(C3ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C3ui

            {
                (typeof(Half), typeof(C3ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i+2]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i+d2]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C3ui

            {
                (typeof(Half), typeof(C3ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.B);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.B);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C3ui

            {
                (typeof(Half), typeof(C3ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i+2]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.R);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.HalfToUInt(da[i+d2]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.R);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C3f

            {
                (typeof(Half), typeof(C3f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C3f

            {
                (typeof(Half), typeof(C3f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i+2]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i+d2]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C3f

            {
                (typeof(Half), typeof(C3f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.B);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.B);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C3f

            {
                (typeof(Half), typeof(C3f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i+2]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.R);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.HalfToFloat(da[i+d2]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.R);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C3d

            {
                (typeof(Half), typeof(C3d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C3d

            {
                (typeof(Half), typeof(C3d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i+2]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i+d2]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C3d

            {
                (typeof(Half), typeof(C3d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.B);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.B);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C3d

            {
                (typeof(Half), typeof(C3d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i+2]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.R);
                                da[i+3] = (Half)Half.One;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.HalfToDouble(da[i+d2]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.R);
                                da[i+d3] = (Half)Half.One;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C4b

            {
                (typeof(Half), typeof(C4b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i+2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i+d2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C4b

            {
                (typeof(Half), typeof(C4b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i+2]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i+d2]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C4b

            {
                (typeof(Half), typeof(C4b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i+2]), 
                                        Col.HalfToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.B);
                                da[i+3] = Col.ByteToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i+d2]), 
                                        Col.HalfToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.R);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.B);
                                da[i+d3] = Col.ByteToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C4b

            {
                (typeof(Half), typeof(C4b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i+2]), 
                                        Col.HalfToByte(da[i+1]), 
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+1] = Col.ByteToHalf(v.G);
                                da[i+2] = Col.ByteToHalf(v.R);
                                da[i+3] = Col.ByteToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.HalfToByte(da[i+d2]), 
                                        Col.HalfToByte(da[i+d1]), 
                                        Col.HalfToByte(da[i]), 
                                        Col.HalfToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToHalf(v.B);
                                da[i+d1] = Col.ByteToHalf(v.G);
                                da[i+d2] = Col.ByteToHalf(v.R);
                                da[i+d3] = Col.ByteToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C4us

            {
                (typeof(Half), typeof(C4us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i+2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i+d2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C4us

            {
                (typeof(Half), typeof(C4us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i+2]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i+d2]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C4us

            {
                (typeof(Half), typeof(C4us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i+2]), 
                                        Col.HalfToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.B);
                                da[i+3] = Col.UShortToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i+d2]), 
                                        Col.HalfToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.R);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.B);
                                da[i+d3] = Col.UShortToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C4us

            {
                (typeof(Half), typeof(C4us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i+2]), 
                                        Col.HalfToUShort(da[i+1]), 
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+1] = Col.UShortToHalf(v.G);
                                da[i+2] = Col.UShortToHalf(v.R);
                                da[i+3] = Col.UShortToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.HalfToUShort(da[i+d2]), 
                                        Col.HalfToUShort(da[i+d1]), 
                                        Col.HalfToUShort(da[i]), 
                                        Col.HalfToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToHalf(v.B);
                                da[i+d1] = Col.UShortToHalf(v.G);
                                da[i+d2] = Col.UShortToHalf(v.R);
                                da[i+d3] = Col.UShortToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C4ui

            {
                (typeof(Half), typeof(C4ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i+2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i+d2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C4ui

            {
                (typeof(Half), typeof(C4ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i+2]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i+d2]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C4ui

            {
                (typeof(Half), typeof(C4ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i+2]), 
                                        Col.HalfToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.B);
                                da[i+3] = Col.UIntToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i+d2]), 
                                        Col.HalfToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.R);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.B);
                                da[i+d3] = Col.UIntToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C4ui

            {
                (typeof(Half), typeof(C4ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i+2]), 
                                        Col.HalfToUInt(da[i+1]), 
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+1] = Col.UIntToHalf(v.G);
                                da[i+2] = Col.UIntToHalf(v.R);
                                da[i+3] = Col.UIntToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.HalfToUInt(da[i+d2]), 
                                        Col.HalfToUInt(da[i+d1]), 
                                        Col.HalfToUInt(da[i]), 
                                        Col.HalfToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToHalf(v.B);
                                da[i+d1] = Col.UIntToHalf(v.G);
                                da[i+d2] = Col.UIntToHalf(v.R);
                                da[i+d3] = Col.UIntToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C4f

            {
                (typeof(Half), typeof(C4f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i+2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i+d2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C4f

            {
                (typeof(Half), typeof(C4f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i+2]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i+d2]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C4f

            {
                (typeof(Half), typeof(C4f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i+2]), 
                                        Col.HalfToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.B);
                                da[i+3] = Col.FloatToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i+d2]), 
                                        Col.HalfToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.R);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.B);
                                da[i+d3] = Col.FloatToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C4f

            {
                (typeof(Half), typeof(C4f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i+2]), 
                                        Col.HalfToFloat(da[i+1]), 
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+1] = Col.FloatToHalf(v.G);
                                da[i+2] = Col.FloatToHalf(v.R);
                                da[i+3] = Col.FloatToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.HalfToFloat(da[i+d2]), 
                                        Col.HalfToFloat(da[i+d1]), 
                                        Col.HalfToFloat(da[i]), 
                                        Col.HalfToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToHalf(v.B);
                                da[i+d1] = Col.FloatToHalf(v.G);
                                da[i+d2] = Col.FloatToHalf(v.R);
                                da[i+d3] = Col.FloatToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB Halfs as C4d

            {
                (typeof(Half), typeof(C4d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i+2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i+d2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR Halfs as C4d

            {
                (typeof(Half), typeof(C4d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i+2]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i+d2]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA Halfs as C4d

            {
                (typeof(Half), typeof(C4d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i+2]), 
                                        Col.HalfToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.B);
                                da[i+3] = Col.DoubleToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i+d2]), 
                                        Col.HalfToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.R);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.B);
                                da[i+d3] = Col.DoubleToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA Halfs as C4d

            {
                (typeof(Half), typeof(C4d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i+2]), 
                                        Col.HalfToDouble(da[i+1]), 
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+1] = Col.DoubleToHalf(v.G);
                                da[i+2] = Col.DoubleToHalf(v.R);
                                da[i+3] = Col.DoubleToHalf(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<Half, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.HalfToDouble(da[i+d2]), 
                                        Col.HalfToDouble(da[i+d1]), 
                                        Col.HalfToDouble(da[i]), 
                                        Col.HalfToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToHalf(v.B);
                                da[i+d1] = Col.DoubleToHalf(v.G);
                                da[i+d2] = Col.DoubleToHalf(v.R);
                                da[i+d3] = Col.DoubleToHalf(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C3b

            {
                (typeof(float), typeof(C3b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C3b

            {
                (typeof(float), typeof(C3b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i+2]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i+d2]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C3b

            {
                (typeof(float), typeof(C3b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.B);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.B);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C3b

            {
                (typeof(float), typeof(C3b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i+2]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.R);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.FloatToByte(da[i+d2]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.R);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C3us

            {
                (typeof(float), typeof(C3us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C3us

            {
                (typeof(float), typeof(C3us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i+2]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i+d2]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C3us

            {
                (typeof(float), typeof(C3us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.B);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.B);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C3us

            {
                (typeof(float), typeof(C3us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i+2]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.R);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.FloatToUShort(da[i+d2]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.R);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C3ui

            {
                (typeof(float), typeof(C3ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C3ui

            {
                (typeof(float), typeof(C3ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i+2]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i+d2]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C3ui

            {
                (typeof(float), typeof(C3ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.B);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.B);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C3ui

            {
                (typeof(float), typeof(C3ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i+2]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.R);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.FloatToUInt(da[i+d2]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.R);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C3f

            {
                (typeof(float), typeof(C3f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C3f

            {
                (typeof(float), typeof(C3f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C3f

            {
                (typeof(float), typeof(C3f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C3f

            {
                (typeof(float), typeof(C3f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C3d

            {
                (typeof(float), typeof(C3d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C3d

            {
                (typeof(float), typeof(C3d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i+2]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i+d2]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C3d

            {
                (typeof(float), typeof(C3d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.B);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.B);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C3d

            {
                (typeof(float), typeof(C3d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i+2]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.R);
                                da[i+3] = (float)1.0f;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        Col.FloatToDouble(da[i+d2]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.R);
                                da[i+d3] = (float)1.0f;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C4b

            {
                (typeof(float), typeof(C4b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i+2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i+d2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C4b

            {
                (typeof(float), typeof(C4b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i+2]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i+d2]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C4b

            {
                (typeof(float), typeof(C4b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i+2]), 
                                        Col.FloatToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.B);
                                da[i+3] = Col.ByteToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i+d2]), 
                                        Col.FloatToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.R);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.B);
                                da[i+d3] = Col.ByteToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C4b

            {
                (typeof(float), typeof(C4b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i+2]), 
                                        Col.FloatToByte(da[i+1]), 
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+1] = Col.ByteToFloat(v.G);
                                da[i+2] = Col.ByteToFloat(v.R);
                                da[i+3] = Col.ByteToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.FloatToByte(da[i+d2]), 
                                        Col.FloatToByte(da[i+d1]), 
                                        Col.FloatToByte(da[i]), 
                                        Col.FloatToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToFloat(v.B);
                                da[i+d1] = Col.ByteToFloat(v.G);
                                da[i+d2] = Col.ByteToFloat(v.R);
                                da[i+d3] = Col.ByteToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C4us

            {
                (typeof(float), typeof(C4us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i+2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i+d2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C4us

            {
                (typeof(float), typeof(C4us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i+2]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i+d2]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C4us

            {
                (typeof(float), typeof(C4us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i+2]), 
                                        Col.FloatToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.B);
                                da[i+3] = Col.UShortToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i+d2]), 
                                        Col.FloatToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.R);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.B);
                                da[i+d3] = Col.UShortToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C4us

            {
                (typeof(float), typeof(C4us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i+2]), 
                                        Col.FloatToUShort(da[i+1]), 
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+1] = Col.UShortToFloat(v.G);
                                da[i+2] = Col.UShortToFloat(v.R);
                                da[i+3] = Col.UShortToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.FloatToUShort(da[i+d2]), 
                                        Col.FloatToUShort(da[i+d1]), 
                                        Col.FloatToUShort(da[i]), 
                                        Col.FloatToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToFloat(v.B);
                                da[i+d1] = Col.UShortToFloat(v.G);
                                da[i+d2] = Col.UShortToFloat(v.R);
                                da[i+d3] = Col.UShortToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C4ui

            {
                (typeof(float), typeof(C4ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i+2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i+d2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C4ui

            {
                (typeof(float), typeof(C4ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i+2]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i+d2]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C4ui

            {
                (typeof(float), typeof(C4ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i+2]), 
                                        Col.FloatToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.B);
                                da[i+3] = Col.UIntToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i+d2]), 
                                        Col.FloatToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.R);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.B);
                                da[i+d3] = Col.UIntToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C4ui

            {
                (typeof(float), typeof(C4ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i+2]), 
                                        Col.FloatToUInt(da[i+1]), 
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+1] = Col.UIntToFloat(v.G);
                                da[i+2] = Col.UIntToFloat(v.R);
                                da[i+3] = Col.UIntToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.FloatToUInt(da[i+d2]), 
                                        Col.FloatToUInt(da[i+d1]), 
                                        Col.FloatToUInt(da[i]), 
                                        Col.FloatToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToFloat(v.B);
                                da[i+d1] = Col.UIntToFloat(v.G);
                                da[i+d2] = Col.UIntToFloat(v.R);
                                da[i+d3] = Col.UIntToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C4f

            {
                (typeof(float), typeof(C4f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C4f

            {
                (typeof(float), typeof(C4f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C4f

            {
                (typeof(float), typeof(C4f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C4f

            {
                (typeof(float), typeof(C4f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB floats as C4d

            {
                (typeof(float), typeof(C4d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i+2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i+d2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR floats as C4d

            {
                (typeof(float), typeof(C4d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i+2]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i+d2]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA floats as C4d

            {
                (typeof(float), typeof(C4d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i+2]), 
                                        Col.FloatToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.B);
                                da[i+3] = Col.DoubleToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i+d2]), 
                                        Col.FloatToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.R);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.B);
                                da[i+d3] = Col.DoubleToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA floats as C4d

            {
                (typeof(float), typeof(C4d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i+2]), 
                                        Col.FloatToDouble(da[i+1]), 
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+1] = Col.DoubleToFloat(v.G);
                                da[i+2] = Col.DoubleToFloat(v.R);
                                da[i+3] = Col.DoubleToFloat(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<float, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        Col.FloatToDouble(da[i+d2]), 
                                        Col.FloatToDouble(da[i+d1]), 
                                        Col.FloatToDouble(da[i]), 
                                        Col.FloatToDouble(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.DoubleToFloat(v.B);
                                da[i+d1] = Col.DoubleToFloat(v.G);
                                da[i+d2] = Col.DoubleToFloat(v.R);
                                da[i+d3] = Col.DoubleToFloat(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C3b

            {
                (typeof(double), typeof(C3b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C3b

            {
                (typeof(double), typeof(C3b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i+2]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i+d2]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C3b

            {
                (typeof(double), typeof(C3b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.B);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.B);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C3b

            {
                (typeof(double), typeof(C3b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i+2]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.R);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3b>()
                        {
                            Getter = (da, i) =>
                                new C3b(
                                        Col.DoubleToByte(da[i+d2]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.R);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C3us

            {
                (typeof(double), typeof(C3us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C3us

            {
                (typeof(double), typeof(C3us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i+2]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i+d2]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C3us

            {
                (typeof(double), typeof(C3us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.B);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.B);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C3us

            {
                (typeof(double), typeof(C3us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i+2]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.R);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3us>()
                        {
                            Getter = (da, i) =>
                                new C3us(
                                        Col.DoubleToUShort(da[i+d2]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.R);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C3ui

            {
                (typeof(double), typeof(C3ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C3ui

            {
                (typeof(double), typeof(C3ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i+2]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i+d2]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C3ui

            {
                (typeof(double), typeof(C3ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.B);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.B);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C3ui

            {
                (typeof(double), typeof(C3ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i+2]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.R);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3ui>()
                        {
                            Getter = (da, i) =>
                                new C3ui(
                                        Col.DoubleToUInt(da[i+d2]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.R);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C3f

            {
                (typeof(double), typeof(C3f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C3f

            {
                (typeof(double), typeof(C3f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i+2]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i+d2]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C3f

            {
                (typeof(double), typeof(C3f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.B);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.B);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C3f

            {
                (typeof(double), typeof(C3f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i+2]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.R);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3f>()
                        {
                            Getter = (da, i) =>
                                new C3f(
                                        Col.DoubleToFloat(da[i+d2]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.R);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C3d

            {
                (typeof(double), typeof(C3d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C3d

            {
                (typeof(double), typeof(C3d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C3d

            {
                (typeof(double), typeof(C3d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C3d

            {
                (typeof(double), typeof(C3d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (double)1.0;
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C3d>()
                        {
                            Getter = (da, i) =>
                                new C3d(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (double)1.0;
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C4b

            {
                (typeof(double), typeof(C4b), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i+2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i+d2]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C4b

            {
                (typeof(double), typeof(C4b), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i+2]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i+d2]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i]), 
                                        (byte)255),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C4b

            {
                (typeof(double), typeof(C4b), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i+2]), 
                                        Col.DoubleToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.B);
                                da[i+3] = Col.ByteToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i+d2]), 
                                        Col.DoubleToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.R);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.B);
                                da[i+d3] = Col.ByteToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C4b

            {
                (typeof(double), typeof(C4b), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i+2]), 
                                        Col.DoubleToByte(da[i+1]), 
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+1] = Col.ByteToDouble(v.G);
                                da[i+2] = Col.ByteToDouble(v.R);
                                da[i+3] = Col.ByteToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4b>()
                        {
                            Getter = (da, i) =>
                                new C4b(
                                        Col.DoubleToByte(da[i+d2]), 
                                        Col.DoubleToByte(da[i+d1]), 
                                        Col.DoubleToByte(da[i]), 
                                        Col.DoubleToByte(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.ByteToDouble(v.B);
                                da[i+d1] = Col.ByteToDouble(v.G);
                                da[i+d2] = Col.ByteToDouble(v.R);
                                da[i+d3] = Col.ByteToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C4us

            {
                (typeof(double), typeof(C4us), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i+2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i+d2]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C4us

            {
                (typeof(double), typeof(C4us), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i+2]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i+d2]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i]), 
                                        (ushort)65535),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C4us

            {
                (typeof(double), typeof(C4us), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i+2]), 
                                        Col.DoubleToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.B);
                                da[i+3] = Col.UShortToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i+d2]), 
                                        Col.DoubleToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.R);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.B);
                                da[i+d3] = Col.UShortToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C4us

            {
                (typeof(double), typeof(C4us), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i+2]), 
                                        Col.DoubleToUShort(da[i+1]), 
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+1] = Col.UShortToDouble(v.G);
                                da[i+2] = Col.UShortToDouble(v.R);
                                da[i+3] = Col.UShortToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4us>()
                        {
                            Getter = (da, i) =>
                                new C4us(
                                        Col.DoubleToUShort(da[i+d2]), 
                                        Col.DoubleToUShort(da[i+d1]), 
                                        Col.DoubleToUShort(da[i]), 
                                        Col.DoubleToUShort(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UShortToDouble(v.B);
                                da[i+d1] = Col.UShortToDouble(v.G);
                                da[i+d2] = Col.UShortToDouble(v.R);
                                da[i+d3] = Col.UShortToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C4ui

            {
                (typeof(double), typeof(C4ui), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i+2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i+d2]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C4ui

            {
                (typeof(double), typeof(C4ui), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i+2]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i+d2]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i]), 
                                        (uint)UInt32.MaxValue),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C4ui

            {
                (typeof(double), typeof(C4ui), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i+2]), 
                                        Col.DoubleToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.B);
                                da[i+3] = Col.UIntToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i+d2]), 
                                        Col.DoubleToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.R);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.B);
                                da[i+d3] = Col.UIntToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C4ui

            {
                (typeof(double), typeof(C4ui), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i+2]), 
                                        Col.DoubleToUInt(da[i+1]), 
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+1] = Col.UIntToDouble(v.G);
                                da[i+2] = Col.UIntToDouble(v.R);
                                da[i+3] = Col.UIntToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4ui>()
                        {
                            Getter = (da, i) =>
                                new C4ui(
                                        Col.DoubleToUInt(da[i+d2]), 
                                        Col.DoubleToUInt(da[i+d1]), 
                                        Col.DoubleToUInt(da[i]), 
                                        Col.DoubleToUInt(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.UIntToDouble(v.B);
                                da[i+d1] = Col.UIntToDouble(v.G);
                                da[i+d2] = Col.UIntToDouble(v.R);
                                da[i+d3] = Col.UIntToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C4f

            {
                (typeof(double), typeof(C4f), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i+2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i+d2]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C4f

            {
                (typeof(double), typeof(C4f), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i+2]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i+d2]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i]), 
                                        (float)1.0f),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C4f

            {
                (typeof(double), typeof(C4f), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i+2]), 
                                        Col.DoubleToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.B);
                                da[i+3] = Col.FloatToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i+d2]), 
                                        Col.DoubleToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.R);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.B);
                                da[i+d3] = Col.FloatToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C4f

            {
                (typeof(double), typeof(C4f), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i+2]), 
                                        Col.DoubleToFloat(da[i+1]), 
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+1] = Col.FloatToDouble(v.G);
                                da[i+2] = Col.FloatToDouble(v.R);
                                da[i+3] = Col.FloatToDouble(v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4f>()
                        {
                            Getter = (da, i) =>
                                new C4f(
                                        Col.DoubleToFloat(da[i+d2]), 
                                        Col.DoubleToFloat(da[i+d1]), 
                                        Col.DoubleToFloat(da[i]), 
                                        Col.DoubleToFloat(da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = Col.FloatToDouble(v.B);
                                da[i+d1] = Col.FloatToDouble(v.G);
                                da[i+d2] = Col.FloatToDouble(v.R);
                                da[i+d3] = Col.FloatToDouble(v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGB doubles as C4d

            {
                (typeof(double), typeof(C4d), Intent.RGB),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGR doubles as C4d

            {
                (typeof(double), typeof(C4d), Intent.BGR),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1;
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (double)1.0),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                            }
                        };
                    }
                }
            },

            #endregion

            #region RGBA doubles as C4d

            {
                (typeof(double), typeof(C4d), Intent.RGBA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i]), 
                                        (da[i+1]), 
                                        (da[i+2]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+1] = (v.G);
                                da[i+2] = (v.B);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i]), 
                                        (da[i+d1]), 
                                        (da[i+d2]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.R);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.B);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

            #region BGRA doubles as C4d

            {
                (typeof(double), typeof(C4d), Intent.BGRA),
                delta =>
                {
                    if (delta.Length < 3)
                        throw new ArgumentException("to few dimensions in tensor");
                    long d1 = delta[delta.Length - 1];
                    if (d1 == 1)
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i+2]), 
                                        (da[i+1]), 
                                        (da[i]), 
                                        (da[i+3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+1] = (v.G);
                                da[i+2] = (v.R);
                                da[i+3] = (v.A);
                            }
                        };
                    else
                    {
                        long d2 = d1 + d1, d3 = d1 + d2;
                        return new TensorAccessors<double, C4d>()
                        {
                            Getter = (da, i) =>
                                new C4d(
                                        (da[i+d2]), 
                                        (da[i+d1]), 
                                        (da[i]), 
                                        (da[i+d3])),
                            Setter = (da, i, v) =>
                            {
                                da[i] = (v.B);
                                da[i+d1] = (v.G);
                                da[i+d2] = (v.R);
                                da[i+d3] = (v.A);
                            }
                        };
                    }
                }
            },

            #endregion

        };
}
