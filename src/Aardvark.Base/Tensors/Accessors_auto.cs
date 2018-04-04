using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Aardvark.Base
{
    public static partial class TensorAccessors
    {
        private static Dictionary<Tup<Type, Type, Symbol>,
                                  Func<long[], ITensorAccessors>> s_creatorMap
            = new Dictionary<Tup<Type, Type, Symbol>,
                             Func<long[], ITensorAccessors>>()
            {
                #region ColorChannel byte as byte

                {
                    Tup.Create(typeof(byte), typeof(byte), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<byte, byte>()
                        {
                            Getter = (da, i) => (da[i]),
                            Setter = (da, i, v) => da[i] = (v),
                        };
                    }
                },

                #endregion

                #region ColorChannel byte as ushort

                {
                    Tup.Create(typeof(byte), typeof(ushort), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<byte, ushort>()
                        {
                            Getter = (da, i) => Col.UShortFromByte(da[i]),
                            Setter = (da, i, v) => da[i] = Col.ByteFromUShort(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel byte as uint

                {
                    Tup.Create(typeof(byte), typeof(uint), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<byte, uint>()
                        {
                            Getter = (da, i) => Col.UIntFromByte(da[i]),
                            Setter = (da, i, v) => da[i] = Col.ByteFromUInt(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel byte as float

                {
                    Tup.Create(typeof(byte), typeof(float), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<byte, float>()
                        {
                            Getter = (da, i) => Col.FloatFromByte(da[i]),
                            Setter = (da, i, v) => da[i] = Col.ByteFromFloat(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel byte as double

                {
                    Tup.Create(typeof(byte), typeof(double), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<byte, double>()
                        {
                            Getter = (da, i) => Col.DoubleFromByte(da[i]),
                            Setter = (da, i, v) => da[i] = Col.ByteFromDouble(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel ushort as byte

                {
                    Tup.Create(typeof(ushort), typeof(byte), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<ushort, byte>()
                        {
                            Getter = (da, i) => Col.ByteFromUShort(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UShortFromByte(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel ushort as ushort

                {
                    Tup.Create(typeof(ushort), typeof(ushort), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<ushort, ushort>()
                        {
                            Getter = (da, i) => (da[i]),
                            Setter = (da, i, v) => da[i] = (v),
                        };
                    }
                },

                #endregion

                #region ColorChannel ushort as uint

                {
                    Tup.Create(typeof(ushort), typeof(uint), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<ushort, uint>()
                        {
                            Getter = (da, i) => Col.UIntFromUShort(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UShortFromUInt(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel ushort as float

                {
                    Tup.Create(typeof(ushort), typeof(float), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<ushort, float>()
                        {
                            Getter = (da, i) => Col.FloatFromUShort(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UShortFromFloat(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel ushort as double

                {
                    Tup.Create(typeof(ushort), typeof(double), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<ushort, double>()
                        {
                            Getter = (da, i) => Col.DoubleFromUShort(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UShortFromDouble(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel uint as byte

                {
                    Tup.Create(typeof(uint), typeof(byte), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<uint, byte>()
                        {
                            Getter = (da, i) => Col.ByteFromUInt(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UIntFromByte(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel uint as ushort

                {
                    Tup.Create(typeof(uint), typeof(ushort), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<uint, ushort>()
                        {
                            Getter = (da, i) => Col.UShortFromUInt(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UIntFromUShort(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel uint as uint

                {
                    Tup.Create(typeof(uint), typeof(uint), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<uint, uint>()
                        {
                            Getter = (da, i) => (da[i]),
                            Setter = (da, i, v) => da[i] = (v),
                        };
                    }
                },

                #endregion

                #region ColorChannel uint as float

                {
                    Tup.Create(typeof(uint), typeof(float), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<uint, float>()
                        {
                            Getter = (da, i) => Col.FloatFromUInt(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UIntFromFloat(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel uint as double

                {
                    Tup.Create(typeof(uint), typeof(double), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<uint, double>()
                        {
                            Getter = (da, i) => Col.DoubleFromUInt(da[i]),
                            Setter = (da, i, v) => da[i] = Col.UIntFromDouble(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel float as byte

                {
                    Tup.Create(typeof(float), typeof(byte), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<float, byte>()
                        {
                            Getter = (da, i) => Col.ByteFromFloat(da[i]),
                            Setter = (da, i, v) => da[i] = Col.FloatFromByte(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel float as ushort

                {
                    Tup.Create(typeof(float), typeof(ushort), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<float, ushort>()
                        {
                            Getter = (da, i) => Col.UShortFromFloat(da[i]),
                            Setter = (da, i, v) => da[i] = Col.FloatFromUShort(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel float as uint

                {
                    Tup.Create(typeof(float), typeof(uint), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<float, uint>()
                        {
                            Getter = (da, i) => Col.UIntFromFloat(da[i]),
                            Setter = (da, i, v) => da[i] = Col.FloatFromUInt(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel float as float

                {
                    Tup.Create(typeof(float), typeof(float), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<float, float>()
                        {
                            Getter = (da, i) => (da[i]),
                            Setter = (da, i, v) => da[i] = (v),
                        };
                    }
                },

                #endregion

                #region ColorChannel float as double

                {
                    Tup.Create(typeof(float), typeof(double), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<float, double>()
                        {
                            Getter = (da, i) => Col.DoubleFromFloat(da[i]),
                            Setter = (da, i, v) => da[i] = Col.FloatFromDouble(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel double as byte

                {
                    Tup.Create(typeof(double), typeof(byte), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<double, byte>()
                        {
                            Getter = (da, i) => Col.ByteFromDouble(da[i]),
                            Setter = (da, i, v) => da[i] = Col.DoubleFromByte(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel double as ushort

                {
                    Tup.Create(typeof(double), typeof(ushort), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<double, ushort>()
                        {
                            Getter = (da, i) => Col.UShortFromDouble(da[i]),
                            Setter = (da, i, v) => da[i] = Col.DoubleFromUShort(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel double as uint

                {
                    Tup.Create(typeof(double), typeof(uint), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<double, uint>()
                        {
                            Getter = (da, i) => Col.UIntFromDouble(da[i]),
                            Setter = (da, i, v) => da[i] = Col.DoubleFromUInt(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel double as float

                {
                    Tup.Create(typeof(double), typeof(float), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<double, float>()
                        {
                            Getter = (da, i) => Col.FloatFromDouble(da[i]),
                            Setter = (da, i, v) => da[i] = Col.DoubleFromFloat(v),
                        };
                    }
                },

                #endregion

                #region ColorChannel double as double

                {
                    Tup.Create(typeof(double), typeof(double), Intent.ColorChannel),
                    delta =>
                    {
                        return new TensorAccessors<double, double>()
                        {
                            Getter = (da, i) => (da[i]),
                            Setter = (da, i, v) => da[i] = (v),
                        };
                    }
                },

                #endregion

                #region RGB bytes as C3b

                {
                    Tup.Create(typeof(byte), typeof(C3b), Intent.RGB),
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
                    Tup.Create(typeof(byte), typeof(C3b), Intent.BGR),
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
                    Tup.Create(typeof(byte), typeof(C3b), Intent.RGBA),
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
                    Tup.Create(typeof(byte), typeof(C3b), Intent.BGRA),
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
                    Tup.Create(typeof(byte), typeof(C3us), Intent.RGB),
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
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C3us

                {
                    Tup.Create(typeof(byte), typeof(C3us), Intent.BGR),
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
                                            Col.UShortFromByte(da[i+2]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromByte(da[i+d2]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C3us

                {
                    Tup.Create(typeof(byte), typeof(C3us), Intent.RGBA),
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
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.B);
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
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.B);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C3us

                {
                    Tup.Create(typeof(byte), typeof(C3us), Intent.BGRA),
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
                                            Col.UShortFromByte(da[i+2]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.R);
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
                                            Col.UShortFromByte(da[i+d2]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.R);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB bytes as C3ui

                {
                    Tup.Create(typeof(byte), typeof(C3ui), Intent.RGB),
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
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C3ui

                {
                    Tup.Create(typeof(byte), typeof(C3ui), Intent.BGR),
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
                                            Col.UIntFromByte(da[i+2]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromByte(da[i+d2]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C3ui

                {
                    Tup.Create(typeof(byte), typeof(C3ui), Intent.RGBA),
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
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.B);
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
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.B);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C3ui

                {
                    Tup.Create(typeof(byte), typeof(C3ui), Intent.BGRA),
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
                                            Col.UIntFromByte(da[i+2]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.R);
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
                                            Col.UIntFromByte(da[i+d2]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.R);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB bytes as C3f

                {
                    Tup.Create(typeof(byte), typeof(C3f), Intent.RGB),
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
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C3f

                {
                    Tup.Create(typeof(byte), typeof(C3f), Intent.BGR),
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
                                            Col.FloatFromByte(da[i+2]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromByte(da[i+d2]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C3f

                {
                    Tup.Create(typeof(byte), typeof(C3f), Intent.RGBA),
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
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.B);
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
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.B);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C3f

                {
                    Tup.Create(typeof(byte), typeof(C3f), Intent.BGRA),
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
                                            Col.FloatFromByte(da[i+2]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.R);
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
                                            Col.FloatFromByte(da[i+d2]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.R);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB bytes as C3d

                {
                    Tup.Create(typeof(byte), typeof(C3d), Intent.RGB),
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
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C3d

                {
                    Tup.Create(typeof(byte), typeof(C3d), Intent.BGR),
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
                                            Col.DoubleFromByte(da[i+2]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromByte(da[i+d2]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C3d

                {
                    Tup.Create(typeof(byte), typeof(C3d), Intent.RGBA),
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
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.B);
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
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.B);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C3d

                {
                    Tup.Create(typeof(byte), typeof(C3d), Intent.BGRA),
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
                                            Col.DoubleFromByte(da[i+2]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.R);
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
                                            Col.DoubleFromByte(da[i+d2]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.R);
                                    da[i+d3] = (byte)255;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB bytes as C4b

                {
                    Tup.Create(typeof(byte), typeof(C4b), Intent.RGB),
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
                    Tup.Create(typeof(byte), typeof(C4b), Intent.BGR),
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
                    Tup.Create(typeof(byte), typeof(C4b), Intent.RGBA),
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
                    Tup.Create(typeof(byte), typeof(C4b), Intent.BGRA),
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
                    Tup.Create(typeof(byte), typeof(C4us), Intent.RGB),
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
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i+2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i+d2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C4us

                {
                    Tup.Create(typeof(byte), typeof(C4us), Intent.BGR),
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
                                            Col.UShortFromByte(da[i+2]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromByte(da[i+d2]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C4us

                {
                    Tup.Create(typeof(byte), typeof(C4us), Intent.RGBA),
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
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i+2]), 
                                            Col.UShortFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.B);
                                    da[i+3] = Col.ByteFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i+d2]), 
                                            Col.UShortFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.R);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.B);
                                    da[i+d3] = Col.ByteFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C4us

                {
                    Tup.Create(typeof(byte), typeof(C4us), Intent.BGRA),
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
                                            Col.UShortFromByte(da[i+2]), 
                                            Col.UShortFromByte(da[i+1]), 
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+1] = Col.ByteFromUShort(v.G);
                                    da[i+2] = Col.ByteFromUShort(v.R);
                                    da[i+3] = Col.ByteFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromByte(da[i+d2]), 
                                            Col.UShortFromByte(da[i+d1]), 
                                            Col.UShortFromByte(da[i]), 
                                            Col.UShortFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUShort(v.B);
                                    da[i+d1] = Col.ByteFromUShort(v.G);
                                    da[i+d2] = Col.ByteFromUShort(v.R);
                                    da[i+d3] = Col.ByteFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB bytes as C4ui

                {
                    Tup.Create(typeof(byte), typeof(C4ui), Intent.RGB),
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
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i+2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i+d2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C4ui

                {
                    Tup.Create(typeof(byte), typeof(C4ui), Intent.BGR),
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
                                            Col.UIntFromByte(da[i+2]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromByte(da[i+d2]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C4ui

                {
                    Tup.Create(typeof(byte), typeof(C4ui), Intent.RGBA),
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
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i+2]), 
                                            Col.UIntFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.B);
                                    da[i+3] = Col.ByteFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i+d2]), 
                                            Col.UIntFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.R);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.B);
                                    da[i+d3] = Col.ByteFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C4ui

                {
                    Tup.Create(typeof(byte), typeof(C4ui), Intent.BGRA),
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
                                            Col.UIntFromByte(da[i+2]), 
                                            Col.UIntFromByte(da[i+1]), 
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+1] = Col.ByteFromUInt(v.G);
                                    da[i+2] = Col.ByteFromUInt(v.R);
                                    da[i+3] = Col.ByteFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromByte(da[i+d2]), 
                                            Col.UIntFromByte(da[i+d1]), 
                                            Col.UIntFromByte(da[i]), 
                                            Col.UIntFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromUInt(v.B);
                                    da[i+d1] = Col.ByteFromUInt(v.G);
                                    da[i+d2] = Col.ByteFromUInt(v.R);
                                    da[i+d3] = Col.ByteFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB bytes as C4f

                {
                    Tup.Create(typeof(byte), typeof(C4f), Intent.RGB),
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
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i+2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i+d2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C4f

                {
                    Tup.Create(typeof(byte), typeof(C4f), Intent.BGR),
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
                                            Col.FloatFromByte(da[i+2]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromByte(da[i+d2]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C4f

                {
                    Tup.Create(typeof(byte), typeof(C4f), Intent.RGBA),
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
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i+2]), 
                                            Col.FloatFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.B);
                                    da[i+3] = Col.ByteFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i+d2]), 
                                            Col.FloatFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.R);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.B);
                                    da[i+d3] = Col.ByteFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C4f

                {
                    Tup.Create(typeof(byte), typeof(C4f), Intent.BGRA),
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
                                            Col.FloatFromByte(da[i+2]), 
                                            Col.FloatFromByte(da[i+1]), 
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+1] = Col.ByteFromFloat(v.G);
                                    da[i+2] = Col.ByteFromFloat(v.R);
                                    da[i+3] = Col.ByteFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromByte(da[i+d2]), 
                                            Col.FloatFromByte(da[i+d1]), 
                                            Col.FloatFromByte(da[i]), 
                                            Col.FloatFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromFloat(v.B);
                                    da[i+d1] = Col.ByteFromFloat(v.G);
                                    da[i+d2] = Col.ByteFromFloat(v.R);
                                    da[i+d3] = Col.ByteFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB bytes as C4d

                {
                    Tup.Create(typeof(byte), typeof(C4d), Intent.RGB),
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
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i+2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i+d2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR bytes as C4d

                {
                    Tup.Create(typeof(byte), typeof(C4d), Intent.BGR),
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
                                            Col.DoubleFromByte(da[i+2]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<byte, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromByte(da[i+d2]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA bytes as C4d

                {
                    Tup.Create(typeof(byte), typeof(C4d), Intent.RGBA),
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
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i+2]), 
                                            Col.DoubleFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.B);
                                    da[i+3] = Col.ByteFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i+d2]), 
                                            Col.DoubleFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.R);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.B);
                                    da[i+d3] = Col.ByteFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA bytes as C4d

                {
                    Tup.Create(typeof(byte), typeof(C4d), Intent.BGRA),
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
                                            Col.DoubleFromByte(da[i+2]), 
                                            Col.DoubleFromByte(da[i+1]), 
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+1] = Col.ByteFromDouble(v.G);
                                    da[i+2] = Col.ByteFromDouble(v.R);
                                    da[i+3] = Col.ByteFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<byte, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromByte(da[i+d2]), 
                                            Col.DoubleFromByte(da[i+d1]), 
                                            Col.DoubleFromByte(da[i]), 
                                            Col.DoubleFromByte(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.ByteFromDouble(v.B);
                                    da[i+d1] = Col.ByteFromDouble(v.G);
                                    da[i+d2] = Col.ByteFromDouble(v.R);
                                    da[i+d3] = Col.ByteFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C3b

                {
                    Tup.Create(typeof(ushort), typeof(C3b), Intent.RGB),
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
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C3b

                {
                    Tup.Create(typeof(ushort), typeof(C3b), Intent.BGR),
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
                                            Col.ByteFromUShort(da[i+2]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromUShort(da[i+d2]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C3b

                {
                    Tup.Create(typeof(ushort), typeof(C3b), Intent.RGBA),
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
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.B);
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
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.B);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C3b

                {
                    Tup.Create(typeof(ushort), typeof(C3b), Intent.BGRA),
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
                                            Col.ByteFromUShort(da[i+2]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.R);
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
                                            Col.ByteFromUShort(da[i+d2]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.R);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C3us

                {
                    Tup.Create(typeof(ushort), typeof(C3us), Intent.RGB),
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
                    Tup.Create(typeof(ushort), typeof(C3us), Intent.BGR),
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
                    Tup.Create(typeof(ushort), typeof(C3us), Intent.RGBA),
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
                    Tup.Create(typeof(ushort), typeof(C3us), Intent.BGRA),
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
                    Tup.Create(typeof(ushort), typeof(C3ui), Intent.RGB),
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
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C3ui

                {
                    Tup.Create(typeof(ushort), typeof(C3ui), Intent.BGR),
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
                                            Col.UIntFromUShort(da[i+2]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromUShort(da[i+d2]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C3ui

                {
                    Tup.Create(typeof(ushort), typeof(C3ui), Intent.RGBA),
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
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.B);
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
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.B);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C3ui

                {
                    Tup.Create(typeof(ushort), typeof(C3ui), Intent.BGRA),
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
                                            Col.UIntFromUShort(da[i+2]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.R);
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
                                            Col.UIntFromUShort(da[i+d2]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.R);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C3f

                {
                    Tup.Create(typeof(ushort), typeof(C3f), Intent.RGB),
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
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C3f

                {
                    Tup.Create(typeof(ushort), typeof(C3f), Intent.BGR),
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
                                            Col.FloatFromUShort(da[i+2]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromUShort(da[i+d2]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C3f

                {
                    Tup.Create(typeof(ushort), typeof(C3f), Intent.RGBA),
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
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.B);
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
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.B);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C3f

                {
                    Tup.Create(typeof(ushort), typeof(C3f), Intent.BGRA),
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
                                            Col.FloatFromUShort(da[i+2]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.R);
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
                                            Col.FloatFromUShort(da[i+d2]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.R);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C3d

                {
                    Tup.Create(typeof(ushort), typeof(C3d), Intent.RGB),
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
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C3d

                {
                    Tup.Create(typeof(ushort), typeof(C3d), Intent.BGR),
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
                                            Col.DoubleFromUShort(da[i+2]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromUShort(da[i+d2]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C3d

                {
                    Tup.Create(typeof(ushort), typeof(C3d), Intent.RGBA),
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
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.B);
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
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.B);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C3d

                {
                    Tup.Create(typeof(ushort), typeof(C3d), Intent.BGRA),
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
                                            Col.DoubleFromUShort(da[i+2]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.R);
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
                                            Col.DoubleFromUShort(da[i+d2]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.R);
                                    da[i+d3] = (ushort)65535;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C4b

                {
                    Tup.Create(typeof(ushort), typeof(C4b), Intent.RGB),
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
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i+2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i+d2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C4b

                {
                    Tup.Create(typeof(ushort), typeof(C4b), Intent.BGR),
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
                                            Col.ByteFromUShort(da[i+2]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUShort(da[i+d2]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C4b

                {
                    Tup.Create(typeof(ushort), typeof(C4b), Intent.RGBA),
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
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i+2]), 
                                            Col.ByteFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.B);
                                    da[i+3] = Col.UShortFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i+d2]), 
                                            Col.ByteFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.R);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.B);
                                    da[i+d3] = Col.UShortFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C4b

                {
                    Tup.Create(typeof(ushort), typeof(C4b), Intent.BGRA),
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
                                            Col.ByteFromUShort(da[i+2]), 
                                            Col.ByteFromUShort(da[i+1]), 
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+1] = Col.UShortFromByte(v.G);
                                    da[i+2] = Col.UShortFromByte(v.R);
                                    da[i+3] = Col.UShortFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUShort(da[i+d2]), 
                                            Col.ByteFromUShort(da[i+d1]), 
                                            Col.ByteFromUShort(da[i]), 
                                            Col.ByteFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromByte(v.B);
                                    da[i+d1] = Col.UShortFromByte(v.G);
                                    da[i+d2] = Col.UShortFromByte(v.R);
                                    da[i+d3] = Col.UShortFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C4us

                {
                    Tup.Create(typeof(ushort), typeof(C4us), Intent.RGB),
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
                    Tup.Create(typeof(ushort), typeof(C4us), Intent.BGR),
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
                    Tup.Create(typeof(ushort), typeof(C4us), Intent.RGBA),
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
                    Tup.Create(typeof(ushort), typeof(C4us), Intent.BGRA),
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
                    Tup.Create(typeof(ushort), typeof(C4ui), Intent.RGB),
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
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i+2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i+d2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C4ui

                {
                    Tup.Create(typeof(ushort), typeof(C4ui), Intent.BGR),
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
                                            Col.UIntFromUShort(da[i+2]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromUShort(da[i+d2]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C4ui

                {
                    Tup.Create(typeof(ushort), typeof(C4ui), Intent.RGBA),
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
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i+2]), 
                                            Col.UIntFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.B);
                                    da[i+3] = Col.UShortFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i+d2]), 
                                            Col.UIntFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.R);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.B);
                                    da[i+d3] = Col.UShortFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C4ui

                {
                    Tup.Create(typeof(ushort), typeof(C4ui), Intent.BGRA),
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
                                            Col.UIntFromUShort(da[i+2]), 
                                            Col.UIntFromUShort(da[i+1]), 
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+1] = Col.UShortFromUInt(v.G);
                                    da[i+2] = Col.UShortFromUInt(v.R);
                                    da[i+3] = Col.UShortFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromUShort(da[i+d2]), 
                                            Col.UIntFromUShort(da[i+d1]), 
                                            Col.UIntFromUShort(da[i]), 
                                            Col.UIntFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromUInt(v.B);
                                    da[i+d1] = Col.UShortFromUInt(v.G);
                                    da[i+d2] = Col.UShortFromUInt(v.R);
                                    da[i+d3] = Col.UShortFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C4f

                {
                    Tup.Create(typeof(ushort), typeof(C4f), Intent.RGB),
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
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i+2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i+d2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C4f

                {
                    Tup.Create(typeof(ushort), typeof(C4f), Intent.BGR),
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
                                            Col.FloatFromUShort(da[i+2]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUShort(da[i+d2]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C4f

                {
                    Tup.Create(typeof(ushort), typeof(C4f), Intent.RGBA),
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
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i+2]), 
                                            Col.FloatFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.B);
                                    da[i+3] = Col.UShortFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i+d2]), 
                                            Col.FloatFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.R);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.B);
                                    da[i+d3] = Col.UShortFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C4f

                {
                    Tup.Create(typeof(ushort), typeof(C4f), Intent.BGRA),
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
                                            Col.FloatFromUShort(da[i+2]), 
                                            Col.FloatFromUShort(da[i+1]), 
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+1] = Col.UShortFromFloat(v.G);
                                    da[i+2] = Col.UShortFromFloat(v.R);
                                    da[i+3] = Col.UShortFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUShort(da[i+d2]), 
                                            Col.FloatFromUShort(da[i+d1]), 
                                            Col.FloatFromUShort(da[i]), 
                                            Col.FloatFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromFloat(v.B);
                                    da[i+d1] = Col.UShortFromFloat(v.G);
                                    da[i+d2] = Col.UShortFromFloat(v.R);
                                    da[i+d3] = Col.UShortFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB ushorts as C4d

                {
                    Tup.Create(typeof(ushort), typeof(C4d), Intent.RGB),
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
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i+2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i+d2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR ushorts as C4d

                {
                    Tup.Create(typeof(ushort), typeof(C4d), Intent.BGR),
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
                                            Col.DoubleFromUShort(da[i+2]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<ushort, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUShort(da[i+d2]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA ushorts as C4d

                {
                    Tup.Create(typeof(ushort), typeof(C4d), Intent.RGBA),
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
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i+2]), 
                                            Col.DoubleFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.B);
                                    da[i+3] = Col.UShortFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i+d2]), 
                                            Col.DoubleFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.R);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.B);
                                    da[i+d3] = Col.UShortFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA ushorts as C4d

                {
                    Tup.Create(typeof(ushort), typeof(C4d), Intent.BGRA),
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
                                            Col.DoubleFromUShort(da[i+2]), 
                                            Col.DoubleFromUShort(da[i+1]), 
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+1] = Col.UShortFromDouble(v.G);
                                    da[i+2] = Col.UShortFromDouble(v.R);
                                    da[i+3] = Col.UShortFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<ushort, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUShort(da[i+d2]), 
                                            Col.DoubleFromUShort(da[i+d1]), 
                                            Col.DoubleFromUShort(da[i]), 
                                            Col.DoubleFromUShort(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UShortFromDouble(v.B);
                                    da[i+d1] = Col.UShortFromDouble(v.G);
                                    da[i+d2] = Col.UShortFromDouble(v.R);
                                    da[i+d3] = Col.UShortFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C3b

                {
                    Tup.Create(typeof(uint), typeof(C3b), Intent.RGB),
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
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C3b

                {
                    Tup.Create(typeof(uint), typeof(C3b), Intent.BGR),
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
                                            Col.ByteFromUInt(da[i+2]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromUInt(da[i+d2]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C3b

                {
                    Tup.Create(typeof(uint), typeof(C3b), Intent.RGBA),
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
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.B);
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
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.B);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C3b

                {
                    Tup.Create(typeof(uint), typeof(C3b), Intent.BGRA),
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
                                            Col.ByteFromUInt(da[i+2]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.R);
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
                                            Col.ByteFromUInt(da[i+d2]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.R);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C3us

                {
                    Tup.Create(typeof(uint), typeof(C3us), Intent.RGB),
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
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C3us

                {
                    Tup.Create(typeof(uint), typeof(C3us), Intent.BGR),
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
                                            Col.UShortFromUInt(da[i+2]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromUInt(da[i+d2]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C3us

                {
                    Tup.Create(typeof(uint), typeof(C3us), Intent.RGBA),
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
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.B);
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
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.B);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C3us

                {
                    Tup.Create(typeof(uint), typeof(C3us), Intent.BGRA),
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
                                            Col.UShortFromUInt(da[i+2]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.R);
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
                                            Col.UShortFromUInt(da[i+d2]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.R);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C3ui

                {
                    Tup.Create(typeof(uint), typeof(C3ui), Intent.RGB),
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
                    Tup.Create(typeof(uint), typeof(C3ui), Intent.BGR),
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
                    Tup.Create(typeof(uint), typeof(C3ui), Intent.RGBA),
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
                    Tup.Create(typeof(uint), typeof(C3ui), Intent.BGRA),
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
                    Tup.Create(typeof(uint), typeof(C3f), Intent.RGB),
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
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C3f

                {
                    Tup.Create(typeof(uint), typeof(C3f), Intent.BGR),
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
                                            Col.FloatFromUInt(da[i+2]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromUInt(da[i+d2]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C3f

                {
                    Tup.Create(typeof(uint), typeof(C3f), Intent.RGBA),
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
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.B);
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
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.B);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C3f

                {
                    Tup.Create(typeof(uint), typeof(C3f), Intent.BGRA),
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
                                            Col.FloatFromUInt(da[i+2]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.R);
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
                                            Col.FloatFromUInt(da[i+d2]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.R);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C3d

                {
                    Tup.Create(typeof(uint), typeof(C3d), Intent.RGB),
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
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C3d

                {
                    Tup.Create(typeof(uint), typeof(C3d), Intent.BGR),
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
                                            Col.DoubleFromUInt(da[i+2]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromUInt(da[i+d2]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C3d

                {
                    Tup.Create(typeof(uint), typeof(C3d), Intent.RGBA),
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
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.B);
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
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.B);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C3d

                {
                    Tup.Create(typeof(uint), typeof(C3d), Intent.BGRA),
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
                                            Col.DoubleFromUInt(da[i+2]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.R);
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
                                            Col.DoubleFromUInt(da[i+d2]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.R);
                                    da[i+d3] = (uint)UInt32.MaxValue;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C4b

                {
                    Tup.Create(typeof(uint), typeof(C4b), Intent.RGB),
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
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i+2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i+d2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C4b

                {
                    Tup.Create(typeof(uint), typeof(C4b), Intent.BGR),
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
                                            Col.ByteFromUInt(da[i+2]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUInt(da[i+d2]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C4b

                {
                    Tup.Create(typeof(uint), typeof(C4b), Intent.RGBA),
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
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i+2]), 
                                            Col.ByteFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.B);
                                    da[i+3] = Col.UIntFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i+d2]), 
                                            Col.ByteFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.R);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.B);
                                    da[i+d3] = Col.UIntFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C4b

                {
                    Tup.Create(typeof(uint), typeof(C4b), Intent.BGRA),
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
                                            Col.ByteFromUInt(da[i+2]), 
                                            Col.ByteFromUInt(da[i+1]), 
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+1] = Col.UIntFromByte(v.G);
                                    da[i+2] = Col.UIntFromByte(v.R);
                                    da[i+3] = Col.UIntFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromUInt(da[i+d2]), 
                                            Col.ByteFromUInt(da[i+d1]), 
                                            Col.ByteFromUInt(da[i]), 
                                            Col.ByteFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromByte(v.B);
                                    da[i+d1] = Col.UIntFromByte(v.G);
                                    da[i+d2] = Col.UIntFromByte(v.R);
                                    da[i+d3] = Col.UIntFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C4us

                {
                    Tup.Create(typeof(uint), typeof(C4us), Intent.RGB),
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
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i+2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i+d2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C4us

                {
                    Tup.Create(typeof(uint), typeof(C4us), Intent.BGR),
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
                                            Col.UShortFromUInt(da[i+2]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromUInt(da[i+d2]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C4us

                {
                    Tup.Create(typeof(uint), typeof(C4us), Intent.RGBA),
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
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i+2]), 
                                            Col.UShortFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.B);
                                    da[i+3] = Col.UIntFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i+d2]), 
                                            Col.UShortFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.R);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.B);
                                    da[i+d3] = Col.UIntFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C4us

                {
                    Tup.Create(typeof(uint), typeof(C4us), Intent.BGRA),
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
                                            Col.UShortFromUInt(da[i+2]), 
                                            Col.UShortFromUInt(da[i+1]), 
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+1] = Col.UIntFromUShort(v.G);
                                    da[i+2] = Col.UIntFromUShort(v.R);
                                    da[i+3] = Col.UIntFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromUInt(da[i+d2]), 
                                            Col.UShortFromUInt(da[i+d1]), 
                                            Col.UShortFromUInt(da[i]), 
                                            Col.UShortFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromUShort(v.B);
                                    da[i+d1] = Col.UIntFromUShort(v.G);
                                    da[i+d2] = Col.UIntFromUShort(v.R);
                                    da[i+d3] = Col.UIntFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C4ui

                {
                    Tup.Create(typeof(uint), typeof(C4ui), Intent.RGB),
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
                    Tup.Create(typeof(uint), typeof(C4ui), Intent.BGR),
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
                    Tup.Create(typeof(uint), typeof(C4ui), Intent.RGBA),
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
                    Tup.Create(typeof(uint), typeof(C4ui), Intent.BGRA),
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
                    Tup.Create(typeof(uint), typeof(C4f), Intent.RGB),
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
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i+2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i+d2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C4f

                {
                    Tup.Create(typeof(uint), typeof(C4f), Intent.BGR),
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
                                            Col.FloatFromUInt(da[i+2]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUInt(da[i+d2]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C4f

                {
                    Tup.Create(typeof(uint), typeof(C4f), Intent.RGBA),
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
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i+2]), 
                                            Col.FloatFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.B);
                                    da[i+3] = Col.UIntFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i+d2]), 
                                            Col.FloatFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.R);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.B);
                                    da[i+d3] = Col.UIntFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C4f

                {
                    Tup.Create(typeof(uint), typeof(C4f), Intent.BGRA),
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
                                            Col.FloatFromUInt(da[i+2]), 
                                            Col.FloatFromUInt(da[i+1]), 
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+1] = Col.UIntFromFloat(v.G);
                                    da[i+2] = Col.UIntFromFloat(v.R);
                                    da[i+3] = Col.UIntFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromUInt(da[i+d2]), 
                                            Col.FloatFromUInt(da[i+d1]), 
                                            Col.FloatFromUInt(da[i]), 
                                            Col.FloatFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromFloat(v.B);
                                    da[i+d1] = Col.UIntFromFloat(v.G);
                                    da[i+d2] = Col.UIntFromFloat(v.R);
                                    da[i+d3] = Col.UIntFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB uints as C4d

                {
                    Tup.Create(typeof(uint), typeof(C4d), Intent.RGB),
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
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i+2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i+d2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR uints as C4d

                {
                    Tup.Create(typeof(uint), typeof(C4d), Intent.BGR),
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
                                            Col.DoubleFromUInt(da[i+2]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<uint, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUInt(da[i+d2]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA uints as C4d

                {
                    Tup.Create(typeof(uint), typeof(C4d), Intent.RGBA),
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
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i+2]), 
                                            Col.DoubleFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.B);
                                    da[i+3] = Col.UIntFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i+d2]), 
                                            Col.DoubleFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.R);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.B);
                                    da[i+d3] = Col.UIntFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA uints as C4d

                {
                    Tup.Create(typeof(uint), typeof(C4d), Intent.BGRA),
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
                                            Col.DoubleFromUInt(da[i+2]), 
                                            Col.DoubleFromUInt(da[i+1]), 
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+1] = Col.UIntFromDouble(v.G);
                                    da[i+2] = Col.UIntFromDouble(v.R);
                                    da[i+3] = Col.UIntFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<uint, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromUInt(da[i+d2]), 
                                            Col.DoubleFromUInt(da[i+d1]), 
                                            Col.DoubleFromUInt(da[i]), 
                                            Col.DoubleFromUInt(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.UIntFromDouble(v.B);
                                    da[i+d1] = Col.UIntFromDouble(v.G);
                                    da[i+d2] = Col.UIntFromDouble(v.R);
                                    da[i+d3] = Col.UIntFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C3b

                {
                    Tup.Create(typeof(float), typeof(C3b), Intent.RGB),
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
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C3b

                {
                    Tup.Create(typeof(float), typeof(C3b), Intent.BGR),
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
                                            Col.ByteFromFloat(da[i+2]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromFloat(da[i+d2]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C3b

                {
                    Tup.Create(typeof(float), typeof(C3b), Intent.RGBA),
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
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.B);
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
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.B);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C3b

                {
                    Tup.Create(typeof(float), typeof(C3b), Intent.BGRA),
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
                                            Col.ByteFromFloat(da[i+2]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.R);
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
                                            Col.ByteFromFloat(da[i+d2]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.R);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C3us

                {
                    Tup.Create(typeof(float), typeof(C3us), Intent.RGB),
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
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C3us

                {
                    Tup.Create(typeof(float), typeof(C3us), Intent.BGR),
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
                                            Col.UShortFromFloat(da[i+2]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromFloat(da[i+d2]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C3us

                {
                    Tup.Create(typeof(float), typeof(C3us), Intent.RGBA),
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
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.B);
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
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.B);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C3us

                {
                    Tup.Create(typeof(float), typeof(C3us), Intent.BGRA),
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
                                            Col.UShortFromFloat(da[i+2]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.R);
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
                                            Col.UShortFromFloat(da[i+d2]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.R);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C3ui

                {
                    Tup.Create(typeof(float), typeof(C3ui), Intent.RGB),
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
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C3ui

                {
                    Tup.Create(typeof(float), typeof(C3ui), Intent.BGR),
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
                                            Col.UIntFromFloat(da[i+2]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromFloat(da[i+d2]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C3ui

                {
                    Tup.Create(typeof(float), typeof(C3ui), Intent.RGBA),
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
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.B);
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
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.B);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C3ui

                {
                    Tup.Create(typeof(float), typeof(C3ui), Intent.BGRA),
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
                                            Col.UIntFromFloat(da[i+2]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.R);
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
                                            Col.UIntFromFloat(da[i+d2]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.R);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C3f

                {
                    Tup.Create(typeof(float), typeof(C3f), Intent.RGB),
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
                    Tup.Create(typeof(float), typeof(C3f), Intent.BGR),
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
                    Tup.Create(typeof(float), typeof(C3f), Intent.RGBA),
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
                    Tup.Create(typeof(float), typeof(C3f), Intent.BGRA),
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
                    Tup.Create(typeof(float), typeof(C3d), Intent.RGB),
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
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C3d

                {
                    Tup.Create(typeof(float), typeof(C3d), Intent.BGR),
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
                                            Col.DoubleFromFloat(da[i+2]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C3d>()
                            {
                                Getter = (da, i) =>
                                    new C3d(
                                            Col.DoubleFromFloat(da[i+d2]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C3d

                {
                    Tup.Create(typeof(float), typeof(C3d), Intent.RGBA),
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
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.B);
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
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.B);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C3d

                {
                    Tup.Create(typeof(float), typeof(C3d), Intent.BGRA),
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
                                            Col.DoubleFromFloat(da[i+2]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.R);
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
                                            Col.DoubleFromFloat(da[i+d2]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.R);
                                    da[i+d3] = (float)1.0f;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C4b

                {
                    Tup.Create(typeof(float), typeof(C4b), Intent.RGB),
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
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i+2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i+d2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C4b

                {
                    Tup.Create(typeof(float), typeof(C4b), Intent.BGR),
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
                                            Col.ByteFromFloat(da[i+2]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromFloat(da[i+d2]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C4b

                {
                    Tup.Create(typeof(float), typeof(C4b), Intent.RGBA),
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
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i+2]), 
                                            Col.ByteFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.B);
                                    da[i+3] = Col.FloatFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i+d2]), 
                                            Col.ByteFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.R);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.B);
                                    da[i+d3] = Col.FloatFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C4b

                {
                    Tup.Create(typeof(float), typeof(C4b), Intent.BGRA),
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
                                            Col.ByteFromFloat(da[i+2]), 
                                            Col.ByteFromFloat(da[i+1]), 
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+1] = Col.FloatFromByte(v.G);
                                    da[i+2] = Col.FloatFromByte(v.R);
                                    da[i+3] = Col.FloatFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromFloat(da[i+d2]), 
                                            Col.ByteFromFloat(da[i+d1]), 
                                            Col.ByteFromFloat(da[i]), 
                                            Col.ByteFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromByte(v.B);
                                    da[i+d1] = Col.FloatFromByte(v.G);
                                    da[i+d2] = Col.FloatFromByte(v.R);
                                    da[i+d3] = Col.FloatFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C4us

                {
                    Tup.Create(typeof(float), typeof(C4us), Intent.RGB),
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
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i+2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i+d2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C4us

                {
                    Tup.Create(typeof(float), typeof(C4us), Intent.BGR),
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
                                            Col.UShortFromFloat(da[i+2]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromFloat(da[i+d2]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C4us

                {
                    Tup.Create(typeof(float), typeof(C4us), Intent.RGBA),
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
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i+2]), 
                                            Col.UShortFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.B);
                                    da[i+3] = Col.FloatFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i+d2]), 
                                            Col.UShortFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.R);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.B);
                                    da[i+d3] = Col.FloatFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C4us

                {
                    Tup.Create(typeof(float), typeof(C4us), Intent.BGRA),
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
                                            Col.UShortFromFloat(da[i+2]), 
                                            Col.UShortFromFloat(da[i+1]), 
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+1] = Col.FloatFromUShort(v.G);
                                    da[i+2] = Col.FloatFromUShort(v.R);
                                    da[i+3] = Col.FloatFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromFloat(da[i+d2]), 
                                            Col.UShortFromFloat(da[i+d1]), 
                                            Col.UShortFromFloat(da[i]), 
                                            Col.UShortFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUShort(v.B);
                                    da[i+d1] = Col.FloatFromUShort(v.G);
                                    da[i+d2] = Col.FloatFromUShort(v.R);
                                    da[i+d3] = Col.FloatFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C4ui

                {
                    Tup.Create(typeof(float), typeof(C4ui), Intent.RGB),
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
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i+2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i+d2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C4ui

                {
                    Tup.Create(typeof(float), typeof(C4ui), Intent.BGR),
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
                                            Col.UIntFromFloat(da[i+2]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromFloat(da[i+d2]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C4ui

                {
                    Tup.Create(typeof(float), typeof(C4ui), Intent.RGBA),
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
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i+2]), 
                                            Col.UIntFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.B);
                                    da[i+3] = Col.FloatFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i+d2]), 
                                            Col.UIntFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.R);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.B);
                                    da[i+d3] = Col.FloatFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C4ui

                {
                    Tup.Create(typeof(float), typeof(C4ui), Intent.BGRA),
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
                                            Col.UIntFromFloat(da[i+2]), 
                                            Col.UIntFromFloat(da[i+1]), 
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+1] = Col.FloatFromUInt(v.G);
                                    da[i+2] = Col.FloatFromUInt(v.R);
                                    da[i+3] = Col.FloatFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromFloat(da[i+d2]), 
                                            Col.UIntFromFloat(da[i+d1]), 
                                            Col.UIntFromFloat(da[i]), 
                                            Col.UIntFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromUInt(v.B);
                                    da[i+d1] = Col.FloatFromUInt(v.G);
                                    da[i+d2] = Col.FloatFromUInt(v.R);
                                    da[i+d3] = Col.FloatFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB floats as C4f

                {
                    Tup.Create(typeof(float), typeof(C4f), Intent.RGB),
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
                    Tup.Create(typeof(float), typeof(C4f), Intent.BGR),
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
                    Tup.Create(typeof(float), typeof(C4f), Intent.RGBA),
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
                    Tup.Create(typeof(float), typeof(C4f), Intent.BGRA),
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
                    Tup.Create(typeof(float), typeof(C4d), Intent.RGB),
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
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i+2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i+d2]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR floats as C4d

                {
                    Tup.Create(typeof(float), typeof(C4d), Intent.BGR),
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
                                            Col.DoubleFromFloat(da[i+2]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<float, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromFloat(da[i+d2]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i]), 
                                            (double)1.0),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA floats as C4d

                {
                    Tup.Create(typeof(float), typeof(C4d), Intent.RGBA),
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
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i+2]), 
                                            Col.DoubleFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.B);
                                    da[i+3] = Col.FloatFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i+d2]), 
                                            Col.DoubleFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.R);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.B);
                                    da[i+d3] = Col.FloatFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA floats as C4d

                {
                    Tup.Create(typeof(float), typeof(C4d), Intent.BGRA),
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
                                            Col.DoubleFromFloat(da[i+2]), 
                                            Col.DoubleFromFloat(da[i+1]), 
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+1] = Col.FloatFromDouble(v.G);
                                    da[i+2] = Col.FloatFromDouble(v.R);
                                    da[i+3] = Col.FloatFromDouble(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<float, C4d>()
                            {
                                Getter = (da, i) =>
                                    new C4d(
                                            Col.DoubleFromFloat(da[i+d2]), 
                                            Col.DoubleFromFloat(da[i+d1]), 
                                            Col.DoubleFromFloat(da[i]), 
                                            Col.DoubleFromFloat(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.FloatFromDouble(v.B);
                                    da[i+d1] = Col.FloatFromDouble(v.G);
                                    da[i+d2] = Col.FloatFromDouble(v.R);
                                    da[i+d3] = Col.FloatFromDouble(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C3b

                {
                    Tup.Create(typeof(double), typeof(C3b), Intent.RGB),
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
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C3b

                {
                    Tup.Create(typeof(double), typeof(C3b), Intent.BGR),
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
                                            Col.ByteFromDouble(da[i+2]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3b>()
                            {
                                Getter = (da, i) =>
                                    new C3b(
                                            Col.ByteFromDouble(da[i+d2]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C3b

                {
                    Tup.Create(typeof(double), typeof(C3b), Intent.RGBA),
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
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.B);
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
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.B);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C3b

                {
                    Tup.Create(typeof(double), typeof(C3b), Intent.BGRA),
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
                                            Col.ByteFromDouble(da[i+2]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.R);
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
                                            Col.ByteFromDouble(da[i+d2]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.R);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C3us

                {
                    Tup.Create(typeof(double), typeof(C3us), Intent.RGB),
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
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C3us

                {
                    Tup.Create(typeof(double), typeof(C3us), Intent.BGR),
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
                                            Col.UShortFromDouble(da[i+2]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3us>()
                            {
                                Getter = (da, i) =>
                                    new C3us(
                                            Col.UShortFromDouble(da[i+d2]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C3us

                {
                    Tup.Create(typeof(double), typeof(C3us), Intent.RGBA),
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
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.B);
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
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.B);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C3us

                {
                    Tup.Create(typeof(double), typeof(C3us), Intent.BGRA),
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
                                            Col.UShortFromDouble(da[i+2]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.R);
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
                                            Col.UShortFromDouble(da[i+d2]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.R);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C3ui

                {
                    Tup.Create(typeof(double), typeof(C3ui), Intent.RGB),
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
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C3ui

                {
                    Tup.Create(typeof(double), typeof(C3ui), Intent.BGR),
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
                                            Col.UIntFromDouble(da[i+2]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3ui>()
                            {
                                Getter = (da, i) =>
                                    new C3ui(
                                            Col.UIntFromDouble(da[i+d2]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C3ui

                {
                    Tup.Create(typeof(double), typeof(C3ui), Intent.RGBA),
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
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.B);
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
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.B);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C3ui

                {
                    Tup.Create(typeof(double), typeof(C3ui), Intent.BGRA),
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
                                            Col.UIntFromDouble(da[i+2]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.R);
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
                                            Col.UIntFromDouble(da[i+d2]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.R);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C3f

                {
                    Tup.Create(typeof(double), typeof(C3f), Intent.RGB),
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
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C3f

                {
                    Tup.Create(typeof(double), typeof(C3f), Intent.BGR),
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
                                            Col.FloatFromDouble(da[i+2]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C3f>()
                            {
                                Getter = (da, i) =>
                                    new C3f(
                                            Col.FloatFromDouble(da[i+d2]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C3f

                {
                    Tup.Create(typeof(double), typeof(C3f), Intent.RGBA),
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
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i+2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.B);
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
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i+d2])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.B);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C3f

                {
                    Tup.Create(typeof(double), typeof(C3f), Intent.BGRA),
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
                                            Col.FloatFromDouble(da[i+2]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.R);
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
                                            Col.FloatFromDouble(da[i+d2]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.R);
                                    da[i+d3] = (double)1.0;
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C3d

                {
                    Tup.Create(typeof(double), typeof(C3d), Intent.RGB),
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
                    Tup.Create(typeof(double), typeof(C3d), Intent.BGR),
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
                    Tup.Create(typeof(double), typeof(C3d), Intent.RGBA),
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
                    Tup.Create(typeof(double), typeof(C3d), Intent.BGRA),
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
                    Tup.Create(typeof(double), typeof(C4b), Intent.RGB),
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
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i+2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i+d2]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C4b

                {
                    Tup.Create(typeof(double), typeof(C4b), Intent.BGR),
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
                                            Col.ByteFromDouble(da[i+2]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromDouble(da[i+d2]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i]), 
                                            (byte)255),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C4b

                {
                    Tup.Create(typeof(double), typeof(C4b), Intent.RGBA),
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
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i+2]), 
                                            Col.ByteFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.B);
                                    da[i+3] = Col.DoubleFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i+d2]), 
                                            Col.ByteFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.R);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.B);
                                    da[i+d3] = Col.DoubleFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C4b

                {
                    Tup.Create(typeof(double), typeof(C4b), Intent.BGRA),
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
                                            Col.ByteFromDouble(da[i+2]), 
                                            Col.ByteFromDouble(da[i+1]), 
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+1] = Col.DoubleFromByte(v.G);
                                    da[i+2] = Col.DoubleFromByte(v.R);
                                    da[i+3] = Col.DoubleFromByte(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4b>()
                            {
                                Getter = (da, i) =>
                                    new C4b(
                                            Col.ByteFromDouble(da[i+d2]), 
                                            Col.ByteFromDouble(da[i+d1]), 
                                            Col.ByteFromDouble(da[i]), 
                                            Col.ByteFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromByte(v.B);
                                    da[i+d1] = Col.DoubleFromByte(v.G);
                                    da[i+d2] = Col.DoubleFromByte(v.R);
                                    da[i+d3] = Col.DoubleFromByte(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C4us

                {
                    Tup.Create(typeof(double), typeof(C4us), Intent.RGB),
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
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i+2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i+d2]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C4us

                {
                    Tup.Create(typeof(double), typeof(C4us), Intent.BGR),
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
                                            Col.UShortFromDouble(da[i+2]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromDouble(da[i+d2]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i]), 
                                            (ushort)65535),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C4us

                {
                    Tup.Create(typeof(double), typeof(C4us), Intent.RGBA),
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
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i+2]), 
                                            Col.UShortFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.B);
                                    da[i+3] = Col.DoubleFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i+d2]), 
                                            Col.UShortFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.R);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.B);
                                    da[i+d3] = Col.DoubleFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C4us

                {
                    Tup.Create(typeof(double), typeof(C4us), Intent.BGRA),
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
                                            Col.UShortFromDouble(da[i+2]), 
                                            Col.UShortFromDouble(da[i+1]), 
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+1] = Col.DoubleFromUShort(v.G);
                                    da[i+2] = Col.DoubleFromUShort(v.R);
                                    da[i+3] = Col.DoubleFromUShort(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4us>()
                            {
                                Getter = (da, i) =>
                                    new C4us(
                                            Col.UShortFromDouble(da[i+d2]), 
                                            Col.UShortFromDouble(da[i+d1]), 
                                            Col.UShortFromDouble(da[i]), 
                                            Col.UShortFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUShort(v.B);
                                    da[i+d1] = Col.DoubleFromUShort(v.G);
                                    da[i+d2] = Col.DoubleFromUShort(v.R);
                                    da[i+d3] = Col.DoubleFromUShort(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C4ui

                {
                    Tup.Create(typeof(double), typeof(C4ui), Intent.RGB),
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
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i+2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i+d2]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C4ui

                {
                    Tup.Create(typeof(double), typeof(C4ui), Intent.BGR),
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
                                            Col.UIntFromDouble(da[i+2]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromDouble(da[i+d2]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i]), 
                                            (uint)UInt32.MaxValue),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C4ui

                {
                    Tup.Create(typeof(double), typeof(C4ui), Intent.RGBA),
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
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i+2]), 
                                            Col.UIntFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.B);
                                    da[i+3] = Col.DoubleFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i+d2]), 
                                            Col.UIntFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.R);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.B);
                                    da[i+d3] = Col.DoubleFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C4ui

                {
                    Tup.Create(typeof(double), typeof(C4ui), Intent.BGRA),
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
                                            Col.UIntFromDouble(da[i+2]), 
                                            Col.UIntFromDouble(da[i+1]), 
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+1] = Col.DoubleFromUInt(v.G);
                                    da[i+2] = Col.DoubleFromUInt(v.R);
                                    da[i+3] = Col.DoubleFromUInt(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4ui>()
                            {
                                Getter = (da, i) =>
                                    new C4ui(
                                            Col.UIntFromDouble(da[i+d2]), 
                                            Col.UIntFromDouble(da[i+d1]), 
                                            Col.UIntFromDouble(da[i]), 
                                            Col.UIntFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromUInt(v.B);
                                    da[i+d1] = Col.DoubleFromUInt(v.G);
                                    da[i+d2] = Col.DoubleFromUInt(v.R);
                                    da[i+d3] = Col.DoubleFromUInt(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C4f

                {
                    Tup.Create(typeof(double), typeof(C4f), Intent.RGB),
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
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i+2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.B);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i+d2]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.B);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGR doubles as C4f

                {
                    Tup.Create(typeof(double), typeof(C4f), Intent.BGR),
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
                                            Col.FloatFromDouble(da[i+2]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.R);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1;
                            return new TensorAccessors<double, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromDouble(da[i+d2]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i]), 
                                            (float)1.0f),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.R);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGBA doubles as C4f

                {
                    Tup.Create(typeof(double), typeof(C4f), Intent.RGBA),
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
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i+2]), 
                                            Col.FloatFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.B);
                                    da[i+3] = Col.DoubleFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i+d2]), 
                                            Col.FloatFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.R);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.B);
                                    da[i+d3] = Col.DoubleFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region BGRA doubles as C4f

                {
                    Tup.Create(typeof(double), typeof(C4f), Intent.BGRA),
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
                                            Col.FloatFromDouble(da[i+2]), 
                                            Col.FloatFromDouble(da[i+1]), 
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+1] = Col.DoubleFromFloat(v.G);
                                    da[i+2] = Col.DoubleFromFloat(v.R);
                                    da[i+3] = Col.DoubleFromFloat(v.A);
                                }
                            };
                        else
                        {
                            long d2 = d1 + d1, d3 = d1 + d2;
                            return new TensorAccessors<double, C4f>()
                            {
                                Getter = (da, i) =>
                                    new C4f(
                                            Col.FloatFromDouble(da[i+d2]), 
                                            Col.FloatFromDouble(da[i+d1]), 
                                            Col.FloatFromDouble(da[i]), 
                                            Col.FloatFromDouble(da[i+d3])),
                                Setter = (da, i, v) =>
                                {
                                    da[i] = Col.DoubleFromFloat(v.B);
                                    da[i+d1] = Col.DoubleFromFloat(v.G);
                                    da[i+d2] = Col.DoubleFromFloat(v.R);
                                    da[i+d3] = Col.DoubleFromFloat(v.A);
                                }
                            };
                        }
                    }
                },

                #endregion

                #region RGB doubles as C4d

                {
                    Tup.Create(typeof(double), typeof(C4d), Intent.RGB),
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
                    Tup.Create(typeof(double), typeof(C4d), Intent.BGR),
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
                    Tup.Create(typeof(double), typeof(C4d), Intent.RGBA),
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
                    Tup.Create(typeof(double), typeof(C4d), Intent.BGRA),
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
}
