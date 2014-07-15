using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    public partial class StreamCodeReader
    {

        #region Vectors

        public V2i ReadV2i()
        {
            return new V2i(ReadInt32(), ReadInt32());
        }

        public V2l ReadV2l()
        {
            return new V2l(ReadInt64(), ReadInt64());
        }

        public V2f ReadV2f()
        {
            return new V2f(ReadSingle(), ReadSingle());
        }

        public V2d ReadV2d()
        {
            return new V2d(ReadDouble(), ReadDouble());
        }

        public V3i ReadV3i()
        {
            return new V3i(ReadInt32(), ReadInt32(), ReadInt32());
        }

        public V3l ReadV3l()
        {
            return new V3l(ReadInt64(), ReadInt64(), ReadInt64());
        }

        public V3f ReadV3f()
        {
            return new V3f(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public V3d ReadV3d()
        {
            return new V3d(ReadDouble(), ReadDouble(), ReadDouble());
        }

        public V4i ReadV4i()
        {
            return new V4i(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
        }

        public V4l ReadV4l()
        {
            return new V4l(ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64());
        }

        public V4f ReadV4f()
        {
            return new V4f(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public V4d ReadV4d()
        {
            return new V4d(ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble());
        }

        #endregion

        #region Matrices

        public M22i ReadM22i()
        {
            return new M22i(
                ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32()
                );
        }

        public M22l ReadM22l()
        {
            return new M22l(
                ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64()
                );
        }

        public M22f ReadM22f()
        {
            return new M22f(
                ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle()
                );
        }

        public M22d ReadM22d()
        {
            return new M22d(
                ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble()
                );
        }

        public M23i ReadM23i()
        {
            return new M23i(
                ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32()
                );
        }

        public M23l ReadM23l()
        {
            return new M23l(
                ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64()
                );
        }

        public M23f ReadM23f()
        {
            return new M23f(
                ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle()
                );
        }

        public M23d ReadM23d()
        {
            return new M23d(
                ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble()
                );
        }

        public M33i ReadM33i()
        {
            return new M33i(
                ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32()
                );
        }

        public M33l ReadM33l()
        {
            return new M33l(
                ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64()
                );
        }

        public M33f ReadM33f()
        {
            return new M33f(
                ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle()
                );
        }

        public M33d ReadM33d()
        {
            return new M33d(
                ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble()
                );
        }

        public M34i ReadM34i()
        {
            return new M34i(
                ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32()
                );
        }

        public M34l ReadM34l()
        {
            return new M34l(
                ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64()
                );
        }

        public M34f ReadM34f()
        {
            return new M34f(
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle()
                );
        }

        public M34d ReadM34d()
        {
            return new M34d(
                ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble()
                );
        }

        public M44i ReadM44i()
        {
            return new M44i(
                ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
                ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32()
                );
        }

        public M44l ReadM44l()
        {
            return new M44l(
                ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
                ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64()
                );
        }

        public M44f ReadM44f()
        {
            return new M44f(
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle()
                );
        }

        public M44d ReadM44d()
        {
            return new M44d(
                ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
                ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble()
                );
        }

        #endregion

        #region Ranges and Boxes

        public Range1b ReadRange1b()
        {
            return new Range1b(ReadByte(), ReadByte());
        }

        public Range1sb ReadRange1sb()
        {
            return new Range1sb(ReadSByte(), ReadSByte());
        }

        public Range1s ReadRange1s()
        {
            return new Range1s(ReadInt16(), ReadInt16());
        }

        public Range1us ReadRange1us()
        {
            return new Range1us(ReadUInt16(), ReadUInt16());
        }

        public Range1i ReadRange1i()
        {
            return new Range1i(ReadInt32(), ReadInt32());
        }

        public Range1ui ReadRange1ui()
        {
            return new Range1ui(ReadUInt32(), ReadUInt32());
        }

        public Range1l ReadRange1l()
        {
            return new Range1l(ReadInt64(), ReadInt64());
        }

        public Range1ul ReadRange1ul()
        {
            return new Range1ul(ReadUInt64(), ReadUInt64());
        }

        public Range1f ReadRange1f()
        {
            return new Range1f(ReadSingle(), ReadSingle());
        }

        public Range1d ReadRange1d()
        {
            return new Range1d(ReadDouble(), ReadDouble());
        }

        public Box2i ReadBox2i()
        {
            return new Box2i(ReadV2i(), ReadV2i());
        }

        public Box2l ReadBox2l()
        {
            return new Box2l(ReadV2l(), ReadV2l());
        }

        public Box2f ReadBox2f()
        {
            return new Box2f(ReadV2f(), ReadV2f());
        }

        public Box2d ReadBox2d()
        {
            return new Box2d(ReadV2d(), ReadV2d());
        }

        public Box3i ReadBox3i()
        {
            return new Box3i(ReadV3i(), ReadV3i());
        }

        public Box3l ReadBox3l()
        {
            return new Box3l(ReadV3l(), ReadV3l());
        }

        public Box3f ReadBox3f()
        {
            return new Box3f(ReadV3f(), ReadV3f());
        }

        public Box3d ReadBox3d()
        {
            return new Box3d(ReadV3d(), ReadV3d());
        }

        #endregion

        #region Colors

        public C3b ReadC3b()
        {
            return new C3b(ReadByte(), ReadByte(), ReadByte());
        }

        public C3us ReadC3us()
        {
            return new C3us(ReadUInt16(), ReadUInt16(), ReadUInt16());
        }

        public C3ui ReadC3ui()
        {
            return new C3ui(ReadUInt32(), ReadUInt32(), ReadUInt32());
        }

        public C3f ReadC3f()
        {
            return new C3f(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public C3d ReadC3d()
        {
            return new C3d(ReadDouble(), ReadDouble(), ReadDouble());
        }

        public C4b ReadC4b()
        {
            return new C4b(ReadByte(), ReadByte(), ReadByte(), ReadByte());
        }

        public C4us ReadC4us()
        {
            return new C4us(ReadUInt16(), ReadUInt16(), ReadUInt16(), ReadUInt16());
        }

        public C4ui ReadC4ui()
        {
            return new C4ui(ReadUInt32(), ReadUInt32(), ReadUInt32(), ReadUInt32());
        }

        public C4f ReadC4f()
        {
            return new C4f(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public C4d ReadC4d()
        {
            return new C4d(ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble());
        }

        #endregion

    }
}
