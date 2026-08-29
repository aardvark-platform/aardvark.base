using Aardvark.Base;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace Aardvark.Base.Coder
{
    /// <summary>
    /// Writes 16-, 32-, and 64-bit integer and IEEE 754 floating-point values in
    /// network byte order (big-endian). Scalar, vector, and color numeric writes are
    /// allocation-free and preserve floating-point bit patterns exactly. Vector and color
    /// components are written in declaration order. Use <see cref="Encoding.BigEndianUnicode"/>
    /// for big-endian UTF-16 strings and characters.
    /// </summary>
    public class NetworkOrderBinaryWriter : BinaryWriter
    {
        public NetworkOrderBinaryWriter(Stream output)
            : base(output, Encoding.UTF8)
        {
        }

        public NetworkOrderBinaryWriter(Stream output, Encoding encoding)
            : base(output, encoding)
        {
            if (encoding == Encoding.Unicode ||
                encoding == Encoding.UTF32)
            {
                throw new Exception(
                    "Encoding '" + encoding.EncodingName +
                    "' uses little-endian byte order, which " +
                    "makes no sense for a network (big-endian) " +
                    "binary writer."
                    );
            }
        }

        public override void Write(short value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(ushort value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(int value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(uint value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(long value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(ulong value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(float value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(Fun.FloatToBits(value)));
        }

        public override void Write(double value)
        {
            base.Write(BinaryPrimitives.ReverseEndianness(Fun.FloatToBits(value)));
        }

        public void Write(V2f value)
        {
            Write(value.X);
            Write(value.Y);
        }

        public void Write(V2d value)
        {
            Write(value.X);
            Write(value.Y);
        }

        public void Write(V3f value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }

        public void Write(V3d value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }

        public void Write(C3f value)
        {
            Write(value.R);
            Write(value.G);
            Write(value.B);
        }

        public void Write(C4f value)
        {
            Write(value.R);
            Write(value.G);
            Write(value.B);
            Write(value.A);
        }
    }
}
