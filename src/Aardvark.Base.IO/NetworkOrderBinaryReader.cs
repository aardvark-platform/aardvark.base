using Aardvark.Base;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace Aardvark.Base.Coder
{
    /// <summary>
    /// Reads 16-, 32-, and 64-bit integer and IEEE 754 floating-point values in
    /// network byte order (big-endian). Scalar numeric reads are allocation-free;
    /// truncated values throw <see cref="EndOfStreamException"/>.
    /// Additional methods read vectors and colors component by component in declaration order.
    /// </summary>
    public class NetworkOrderBinaryReader : BinaryReader
    {

        #region Constructors

        public NetworkOrderBinaryReader(Stream input)
            : base(input, Encoding.UTF8)
        {
        }

        public NetworkOrderBinaryReader(Stream input, Encoding encoding)
            : base(input, encoding)
        {
            if (encoding == Encoding.Unicode ||
                encoding == Encoding.UTF32)
            {
                throw new Exception(
                    "Encoding '" + encoding.EncodingName +
                    "' uses little-endian byte order, which " +
                    "makes no sense for a network (big-endian) " +
                    "binary reader."
                    );
            }
        }

        #endregion

        #region Read Overrides

        public override short ReadInt16()
        {
            return BinaryPrimitives.ReverseEndianness(base.ReadInt16());
        }

        public override ushort ReadUInt16()
        {
            return BinaryPrimitives.ReverseEndianness(base.ReadUInt16());
        }

        public override int ReadInt32()
        {
            return BinaryPrimitives.ReverseEndianness(base.ReadInt32());
        }

        public override uint ReadUInt32()
        {
            return BinaryPrimitives.ReverseEndianness(base.ReadUInt32());
        }

        public override long ReadInt64()
        {
            return BinaryPrimitives.ReverseEndianness(base.ReadInt64());
        }

        public override ulong ReadUInt64()
        {
            return BinaryPrimitives.ReverseEndianness(base.ReadUInt64());
        }

        public override float ReadSingle()
        {
            return Fun.FloatFromBits(BinaryPrimitives.ReverseEndianness(base.ReadInt32()));
        }

        public override double ReadDouble()
        {
            return Fun.FloatFromBits(BinaryPrimitives.ReverseEndianness(base.ReadInt64()));
        }

        #endregion

        #region Read V2f

        public V2f ReadV2f()
        {
            return new V2f(ReadSingle(), ReadSingle());
        }

        public V2f ReadV2fFrom2SignedInt8()
        {
            return new V2f(ReadSByte(), ReadSByte());
        }

        public V2f ReadV2fFrom2Int16()
        {
            return new V2f(ReadInt16(), ReadInt16());
        }

        public V2f ReadV2fFrom2Int32()
        {
            return new V2f(ReadInt32(), ReadInt32());
        }

        #endregion

        #region Read V3f

        public V3f ReadV3f()
        {
            return new V3f(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public V3f ReadV3fFrom3SignedInt8()
        {
            return new V3f(ReadSByte(), ReadSByte(), ReadSByte());
        }

        public V3f ReadV3fFrom3Int16()
        {
            return new V3f(ReadInt16(), ReadInt16(), ReadInt16());
        }

        public V3f ReadV3fFrom3Int32()
        {
            return new V3f(ReadInt32(), ReadInt32(), ReadInt32());
        }

        #endregion

        #region Read C3f

        public C3f ReadC3f()
        {
            return new C3f(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public C3f ReadC3fFrom3SignedInt8()
        {
            return new C3f(ReadSByte(), ReadSByte(), ReadSByte());
        }

        public C3f ReadC3fFrom3Int16()
        {
            return new C3f(ReadInt16(), ReadInt16(), ReadInt16());
        }

        public C3f ReadC3fFrom3Int32()
        {
            return new C3f(ReadInt32(), ReadInt32(), ReadInt32());
        }

        #endregion

        #region Read C4f

        public C4f ReadC4f()
        {
            return new C4f(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public C4f ReadC4fFrom4SignedInt8()
        {
            return new C4f(ReadSByte(), ReadSByte(), ReadSByte(), ReadSByte());
        }

        public C4f ReadC4fFrom4UInt8()
        {
            return new C4f(ReadByte(), ReadByte(), ReadByte(), ReadByte());
        }

        public C4f ReadC4fFrom4Int16()
        {
            return new C4f(ReadInt16(), ReadInt16(), ReadInt16(), ReadInt16());
        }

        public C4f ReadC4fFrom4UInt16()
        {
            return new C4f(ReadUInt16(), ReadUInt16(), ReadUInt16(), ReadUInt16());
        }

        public C4f ReadC4fFrom4Int32()
        {
            return new C4f(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
        }

        public C4f ReadC4fFrom4UInt32()
        {
            return new C4f(ReadUInt32(), ReadUInt32(), ReadUInt32(), ReadUInt32());
        }

        #endregion

    }
}
