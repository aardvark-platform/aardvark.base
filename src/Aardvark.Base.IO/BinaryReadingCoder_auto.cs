using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Coder
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    public partial class BinaryReadingCoder
    {
        #region Vectors

        public void CodeV2i(ref V2i value) 
        {
            value.X = m_reader.ReadInt32();
            value.Y = m_reader.ReadInt32();
        }

        public void CodeV2ui(ref V2ui value) 
        {
            value.X = m_reader.ReadUInt32();
            value.Y = m_reader.ReadUInt32();
        }

        public void CodeV2l(ref V2l value) 
        {
            value.X = m_reader.ReadInt64();
            value.Y = m_reader.ReadInt64();
        }

        public void CodeV2f(ref V2f value) 
        {
            value.X = m_reader.ReadSingle();
            value.Y = m_reader.ReadSingle();
        }

        public void CodeV2d(ref V2d value) 
        {
            value.X = m_reader.ReadDouble();
            value.Y = m_reader.ReadDouble();
        }

        public void CodeV3i(ref V3i value) 
        {
            value.X = m_reader.ReadInt32();
            value.Y = m_reader.ReadInt32();
            value.Z = m_reader.ReadInt32();
        }

        public void CodeV3ui(ref V3ui value) 
        {
            value.X = m_reader.ReadUInt32();
            value.Y = m_reader.ReadUInt32();
            value.Z = m_reader.ReadUInt32();
        }

        public void CodeV3l(ref V3l value) 
        {
            value.X = m_reader.ReadInt64();
            value.Y = m_reader.ReadInt64();
            value.Z = m_reader.ReadInt64();
        }

        public void CodeV3f(ref V3f value) 
        {
            value.X = m_reader.ReadSingle();
            value.Y = m_reader.ReadSingle();
            value.Z = m_reader.ReadSingle();
        }

        public void CodeV3d(ref V3d value) 
        {
            value.X = m_reader.ReadDouble();
            value.Y = m_reader.ReadDouble();
            value.Z = m_reader.ReadDouble();
        }

        public void CodeV4i(ref V4i value) 
        {
            value.X = m_reader.ReadInt32();
            value.Y = m_reader.ReadInt32();
            value.Z = m_reader.ReadInt32();
            value.W = m_reader.ReadInt32();
        }

        public void CodeV4ui(ref V4ui value) 
        {
            value.X = m_reader.ReadUInt32();
            value.Y = m_reader.ReadUInt32();
            value.Z = m_reader.ReadUInt32();
            value.W = m_reader.ReadUInt32();
        }

        public void CodeV4l(ref V4l value) 
        {
            value.X = m_reader.ReadInt64();
            value.Y = m_reader.ReadInt64();
            value.Z = m_reader.ReadInt64();
            value.W = m_reader.ReadInt64();
        }

        public void CodeV4f(ref V4f value) 
        {
            value.X = m_reader.ReadSingle();
            value.Y = m_reader.ReadSingle();
            value.Z = m_reader.ReadSingle();
            value.W = m_reader.ReadSingle();
        }

        public void CodeV4d(ref V4d value) 
        {
            value.X = m_reader.ReadDouble();
            value.Y = m_reader.ReadDouble();
            value.Z = m_reader.ReadDouble();
            value.W = m_reader.ReadDouble();
        }

        #endregion

        #region Matrices

        public void CodeM22i(ref M22i value) { value = m_reader.ReadM22i(); }
        public void CodeM22l(ref M22l value) { value = m_reader.ReadM22l(); }
        public void CodeM22f(ref M22f value) { value = m_reader.ReadM22f(); }
        public void CodeM22d(ref M22d value) { value = m_reader.ReadM22d(); }
        public void CodeM23i(ref M23i value) { value = m_reader.ReadM23i(); }
        public void CodeM23l(ref M23l value) { value = m_reader.ReadM23l(); }
        public void CodeM23f(ref M23f value) { value = m_reader.ReadM23f(); }
        public void CodeM23d(ref M23d value) { value = m_reader.ReadM23d(); }
        public void CodeM33i(ref M33i value) { value = m_reader.ReadM33i(); }
        public void CodeM33l(ref M33l value) { value = m_reader.ReadM33l(); }
        public void CodeM33f(ref M33f value) { value = m_reader.ReadM33f(); }
        public void CodeM33d(ref M33d value) { value = m_reader.ReadM33d(); }
        public void CodeM34i(ref M34i value) { value = m_reader.ReadM34i(); }
        public void CodeM34l(ref M34l value) { value = m_reader.ReadM34l(); }
        public void CodeM34f(ref M34f value) { value = m_reader.ReadM34f(); }
        public void CodeM34d(ref M34d value) { value = m_reader.ReadM34d(); }
        public void CodeM44i(ref M44i value) { value = m_reader.ReadM44i(); }
        public void CodeM44l(ref M44l value) { value = m_reader.ReadM44l(); }
        public void CodeM44f(ref M44f value) { value = m_reader.ReadM44f(); }
        public void CodeM44d(ref M44d value) { value = m_reader.ReadM44d(); }

        #endregion

        #region Ranges and Boxes

        public void CodeRange1b(ref Range1b value)
        {
            value.Min = m_reader.ReadByte();
            value.Max = m_reader.ReadByte();
        }

        public void CodeRange1sb(ref Range1sb value)
        {
            value.Min = m_reader.ReadSByte();
            value.Max = m_reader.ReadSByte();
        }

        public void CodeRange1s(ref Range1s value)
        {
            value.Min = m_reader.ReadInt16();
            value.Max = m_reader.ReadInt16();
        }

        public void CodeRange1us(ref Range1us value)
        {
            value.Min = m_reader.ReadUInt16();
            value.Max = m_reader.ReadUInt16();
        }

        public void CodeRange1i(ref Range1i value)
        {
            value.Min = m_reader.ReadInt32();
            value.Max = m_reader.ReadInt32();
        }

        public void CodeRange1ui(ref Range1ui value)
        {
            value.Min = m_reader.ReadUInt32();
            value.Max = m_reader.ReadUInt32();
        }

        public void CodeRange1l(ref Range1l value)
        {
            value.Min = m_reader.ReadInt64();
            value.Max = m_reader.ReadInt64();
        }

        public void CodeRange1ul(ref Range1ul value)
        {
            value.Min = m_reader.ReadUInt64();
            value.Max = m_reader.ReadUInt64();
        }

        public void CodeRange1f(ref Range1f value)
        {
            value.Min = m_reader.ReadSingle();
            value.Max = m_reader.ReadSingle();
        }

        public void CodeRange1d(ref Range1d value)
        {
            value.Min = m_reader.ReadDouble();
            value.Max = m_reader.ReadDouble();
        }

        public void CodeBox2i(ref Box2i value)
        {
            value.Min = m_reader.ReadV2i();
            value.Max = m_reader.ReadV2i();
        }

        public void CodeBox2l(ref Box2l value)
        {
            value.Min = m_reader.ReadV2l();
            value.Max = m_reader.ReadV2l();
        }

        public void CodeBox2f(ref Box2f value)
        {
            value.Min = m_reader.ReadV2f();
            value.Max = m_reader.ReadV2f();
        }

        public void CodeBox2d(ref Box2d value)
        {
            value.Min = m_reader.ReadV2d();
            value.Max = m_reader.ReadV2d();
        }

        public void CodeBox3i(ref Box3i value)
        {
            value.Min = m_reader.ReadV3i();
            value.Max = m_reader.ReadV3i();
        }

        public void CodeBox3l(ref Box3l value)
        {
            value.Min = m_reader.ReadV3l();
            value.Max = m_reader.ReadV3l();
        }

        public void CodeBox3f(ref Box3f value)
        {
            value.Min = m_reader.ReadV3f();
            value.Max = m_reader.ReadV3f();
        }

        public void CodeBox3d(ref Box3d value)
        {
            value.Min = m_reader.ReadV3d();
            value.Max = m_reader.ReadV3d();
        }

        #endregion

        #region Geometry Types

        public void CodeCircle2f(ref Circle2f v)
        {
            CodeV2f(ref v.Center); CodeFloat(ref v.Radius);
        }

        public void CodeLine2f(ref Line2f v)
        {
            CodeV2f(ref v.P0); CodeV2f(ref v.P1);
        }

        public void CodeLine3f(ref Line3f v)
        {
            CodeV3f(ref v.P0); CodeV3f(ref v.P1);
        }

        public void CodePlane2f(ref Plane2f v)
        {
            CodeV2f(ref v.Normal); CodeFloat(ref v.Distance);
        }

        public void CodePlane3f(ref Plane3f v)
        {
            CodeV3f(ref v.Normal); CodeFloat(ref v.Distance);
        }

        public void CodePlaneWithPoint3f(ref PlaneWithPoint3f v)
        {
            CodeV3f(ref v.Normal); CodeV3f(ref v.Point);
        }

        public void CodeQuad2f(ref Quad2f v)
        {
            CodeV2f(ref v.P0); CodeV2f(ref v.P1); CodeV2f(ref v.P2); CodeV2f(ref v.P3);
        }

        public void CodeQuad3f(ref Quad3f v)
        {
            CodeV3f(ref v.P0); CodeV3f(ref v.P1); CodeV3f(ref v.P2); CodeV3f(ref v.P3);
        }

        public void CodeRay2f(ref Ray2f v)
        {
            CodeV2f(ref v.Origin); CodeV2f(ref v.Direction);
        }

        public void CodeRay3f(ref Ray3f v)
        {
            CodeV3f(ref v.Origin); CodeV3f(ref v.Direction);
        }

        public void CodeSphere3f(ref Sphere3f v)
        {
            CodeV3f(ref v.Center); CodeFloat(ref v.Radius);
        }

        public void CodeTriangle2f(ref Triangle2f v)
        {
            CodeV2f(ref v.P0); CodeV2f(ref v.P1); CodeV2f(ref v.P2);
        }

        public void CodeTriangle3f(ref Triangle3f v)
        {
            CodeV3f(ref v.P0); CodeV3f(ref v.P1); CodeV3f(ref v.P2);
        }

        public void CodeCircle2d(ref Circle2d v)
        {
            CodeV2d(ref v.Center); CodeDouble(ref v.Radius);
        }

        public void CodeLine2d(ref Line2d v)
        {
            CodeV2d(ref v.P0); CodeV2d(ref v.P1);
        }

        public void CodeLine3d(ref Line3d v)
        {
            CodeV3d(ref v.P0); CodeV3d(ref v.P1);
        }

        public void CodePlane2d(ref Plane2d v)
        {
            CodeV2d(ref v.Normal); CodeDouble(ref v.Distance);
        }

        public void CodePlane3d(ref Plane3d v)
        {
            CodeV3d(ref v.Normal); CodeDouble(ref v.Distance);
        }

        public void CodePlaneWithPoint3d(ref PlaneWithPoint3d v)
        {
            CodeV3d(ref v.Normal); CodeV3d(ref v.Point);
        }

        public void CodeQuad2d(ref Quad2d v)
        {
            CodeV2d(ref v.P0); CodeV2d(ref v.P1); CodeV2d(ref v.P2); CodeV2d(ref v.P3);
        }

        public void CodeQuad3d(ref Quad3d v)
        {
            CodeV3d(ref v.P0); CodeV3d(ref v.P1); CodeV3d(ref v.P2); CodeV3d(ref v.P3);
        }

        public void CodeRay2d(ref Ray2d v)
        {
            CodeV2d(ref v.Origin); CodeV2d(ref v.Direction);
        }

        public void CodeRay3d(ref Ray3d v)
        {
            CodeV3d(ref v.Origin); CodeV3d(ref v.Direction);
        }

        public void CodeSphere3d(ref Sphere3d v)
        {
            CodeV3d(ref v.Center); CodeDouble(ref v.Radius);
        }

        public void CodeTriangle2d(ref Triangle2d v)
        {
            CodeV2d(ref v.P0); CodeV2d(ref v.P1); CodeV2d(ref v.P2);
        }

        public void CodeTriangle3d(ref Triangle3d v)
        {
            CodeV3d(ref v.P0); CodeV3d(ref v.P1); CodeV3d(ref v.P2);
        }

        #endregion

        #region Colors

        public void CodeC3b(ref C3b value)
        {
            value.R = m_reader.ReadByte();
            value.G = m_reader.ReadByte();
            value.B = m_reader.ReadByte();
        }

        public void CodeC3us(ref C3us value)
        {
            value.R = m_reader.ReadUInt16();
            value.G = m_reader.ReadUInt16();
            value.B = m_reader.ReadUInt16();
        }

        public void CodeC3ui(ref C3ui value)
        {
            value.R = m_reader.ReadUInt32();
            value.G = m_reader.ReadUInt32();
            value.B = m_reader.ReadUInt32();
        }

        public void CodeC3f(ref C3f value)
        {
            value.R = m_reader.ReadSingle();
            value.G = m_reader.ReadSingle();
            value.B = m_reader.ReadSingle();
        }

        public void CodeC3d(ref C3d value)
        {
            value.R = m_reader.ReadDouble();
            value.G = m_reader.ReadDouble();
            value.B = m_reader.ReadDouble();
        }

        public void CodeC4b(ref C4b value)
        {
            value.R = m_reader.ReadByte();
            value.G = m_reader.ReadByte();
            value.B = m_reader.ReadByte();
            value.A = m_reader.ReadByte();
        }

        public void CodeC4us(ref C4us value)
        {
            value.R = m_reader.ReadUInt16();
            value.G = m_reader.ReadUInt16();
            value.B = m_reader.ReadUInt16();
            value.A = m_reader.ReadUInt16();
        }

        public void CodeC4ui(ref C4ui value)
        {
            value.R = m_reader.ReadUInt32();
            value.G = m_reader.ReadUInt32();
            value.B = m_reader.ReadUInt32();
            value.A = m_reader.ReadUInt32();
        }

        public void CodeC4f(ref C4f value)
        {
            value.R = m_reader.ReadSingle();
            value.G = m_reader.ReadSingle();
            value.B = m_reader.ReadSingle();
            value.A = m_reader.ReadSingle();
        }

        public void CodeC4d(ref C4d value)
        {
            value.R = m_reader.ReadDouble();
            value.G = m_reader.ReadDouble();
            value.B = m_reader.ReadDouble();
            value.A = m_reader.ReadDouble();
        }

        #endregion

        #region Trafos

        public void CodeEuclidean3f(ref Euclidean3f value) { value = m_reader.ReadEuclidean3f(); }
        public void CodeEuclidean3d(ref Euclidean3d value) { value = m_reader.ReadEuclidean3d(); }
        public void CodeRot2f(ref Rot2f value) { value = m_reader.ReadRot2f(); }
        public void CodeRot2d(ref Rot2d value) { value = m_reader.ReadRot2d(); }
        public void CodeRot3f(ref Rot3f value) { value = m_reader.ReadRot3f(); }
        public void CodeRot3d(ref Rot3d value) { value = m_reader.ReadRot3d(); }
        public void CodeScale3f(ref Scale3f value) { value = m_reader.ReadScale3f(); }
        public void CodeScale3d(ref Scale3d value) { value = m_reader.ReadScale3d(); }
        public void CodeShift3f(ref Shift3f value) { value = m_reader.ReadShift3f(); }
        public void CodeShift3d(ref Shift3d value) { value = m_reader.ReadShift3d(); }
        public void CodeTrafo2f(ref Trafo2f value) { value = m_reader.ReadTrafo2f(); }
        public void CodeTrafo2d(ref Trafo2d value) { value = m_reader.ReadTrafo2d(); }
        public void CodeTrafo3f(ref Trafo3f value) { value = m_reader.ReadTrafo3f(); }
        public void CodeTrafo3d(ref Trafo3d value) { value = m_reader.ReadTrafo3d(); }

        #endregion

        #region Tensors

        public void CodeVector_of_Byte_(ref Vector<byte> value)
        {
            byte[] data = null; CodeByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<byte>(data, origin, size, delta);
        }

        public void CodeVector_of_SByte_(ref Vector<sbyte> value)
        {
            sbyte[] data = null; CodeSByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<sbyte>(data, origin, size, delta);
        }

        public void CodeVector_of_Short_(ref Vector<short> value)
        {
            short[] data = null; CodeShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<short>(data, origin, size, delta);
        }

        public void CodeVector_of_UShort_(ref Vector<ushort> value)
        {
            ushort[] data = null; CodeUShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<ushort>(data, origin, size, delta);
        }

        public void CodeVector_of_Int_(ref Vector<int> value)
        {
            int[] data = null; CodeIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<int>(data, origin, size, delta);
        }

        public void CodeVector_of_UInt_(ref Vector<uint> value)
        {
            uint[] data = null; CodeUIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<uint>(data, origin, size, delta);
        }

        public void CodeVector_of_Long_(ref Vector<long> value)
        {
            long[] data = null; CodeLongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<long>(data, origin, size, delta);
        }

        public void CodeVector_of_ULong_(ref Vector<ulong> value)
        {
            ulong[] data = null; CodeULongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<ulong>(data, origin, size, delta);
        }

        public void CodeVector_of_Float_(ref Vector<float> value)
        {
            float[] data = null; CodeFloatArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<float>(data, origin, size, delta);
        }

        public void CodeVector_of_Double_(ref Vector<double> value)
        {
            double[] data = null; CodeDoubleArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<double>(data, origin, size, delta);
        }

        public void CodeVector_of_Fraction_(ref Vector<Fraction> value)
        {
            Fraction[] data = null; CodeFractionArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Fraction>(data, origin, size, delta);
        }

        public void CodeVector_of_V2i_(ref Vector<V2i> value)
        {
            V2i[] data = null; CodeV2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V2i>(data, origin, size, delta);
        }

        public void CodeVector_of_V2l_(ref Vector<V2l> value)
        {
            V2l[] data = null; CodeV2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V2l>(data, origin, size, delta);
        }

        public void CodeVector_of_V2f_(ref Vector<V2f> value)
        {
            V2f[] data = null; CodeV2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V2f>(data, origin, size, delta);
        }

        public void CodeVector_of_V2d_(ref Vector<V2d> value)
        {
            V2d[] data = null; CodeV2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V2d>(data, origin, size, delta);
        }

        public void CodeVector_of_V3i_(ref Vector<V3i> value)
        {
            V3i[] data = null; CodeV3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V3i>(data, origin, size, delta);
        }

        public void CodeVector_of_V3l_(ref Vector<V3l> value)
        {
            V3l[] data = null; CodeV3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V3l>(data, origin, size, delta);
        }

        public void CodeVector_of_V3f_(ref Vector<V3f> value)
        {
            V3f[] data = null; CodeV3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V3f>(data, origin, size, delta);
        }

        public void CodeVector_of_V3d_(ref Vector<V3d> value)
        {
            V3d[] data = null; CodeV3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V3d>(data, origin, size, delta);
        }

        public void CodeVector_of_V4i_(ref Vector<V4i> value)
        {
            V4i[] data = null; CodeV4iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V4i>(data, origin, size, delta);
        }

        public void CodeVector_of_V4l_(ref Vector<V4l> value)
        {
            V4l[] data = null; CodeV4lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V4l>(data, origin, size, delta);
        }

        public void CodeVector_of_V4f_(ref Vector<V4f> value)
        {
            V4f[] data = null; CodeV4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V4f>(data, origin, size, delta);
        }

        public void CodeVector_of_V4d_(ref Vector<V4d> value)
        {
            V4d[] data = null; CodeV4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<V4d>(data, origin, size, delta);
        }

        public void CodeVector_of_M22i_(ref Vector<M22i> value)
        {
            M22i[] data = null; CodeM22iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M22i>(data, origin, size, delta);
        }

        public void CodeVector_of_M22l_(ref Vector<M22l> value)
        {
            M22l[] data = null; CodeM22lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M22l>(data, origin, size, delta);
        }

        public void CodeVector_of_M22f_(ref Vector<M22f> value)
        {
            M22f[] data = null; CodeM22fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M22f>(data, origin, size, delta);
        }

        public void CodeVector_of_M22d_(ref Vector<M22d> value)
        {
            M22d[] data = null; CodeM22dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M22d>(data, origin, size, delta);
        }

        public void CodeVector_of_M23i_(ref Vector<M23i> value)
        {
            M23i[] data = null; CodeM23iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M23i>(data, origin, size, delta);
        }

        public void CodeVector_of_M23l_(ref Vector<M23l> value)
        {
            M23l[] data = null; CodeM23lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M23l>(data, origin, size, delta);
        }

        public void CodeVector_of_M23f_(ref Vector<M23f> value)
        {
            M23f[] data = null; CodeM23fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M23f>(data, origin, size, delta);
        }

        public void CodeVector_of_M23d_(ref Vector<M23d> value)
        {
            M23d[] data = null; CodeM23dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M23d>(data, origin, size, delta);
        }

        public void CodeVector_of_M33i_(ref Vector<M33i> value)
        {
            M33i[] data = null; CodeM33iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M33i>(data, origin, size, delta);
        }

        public void CodeVector_of_M33l_(ref Vector<M33l> value)
        {
            M33l[] data = null; CodeM33lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M33l>(data, origin, size, delta);
        }

        public void CodeVector_of_M33f_(ref Vector<M33f> value)
        {
            M33f[] data = null; CodeM33fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M33f>(data, origin, size, delta);
        }

        public void CodeVector_of_M33d_(ref Vector<M33d> value)
        {
            M33d[] data = null; CodeM33dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M33d>(data, origin, size, delta);
        }

        public void CodeVector_of_M34i_(ref Vector<M34i> value)
        {
            M34i[] data = null; CodeM34iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M34i>(data, origin, size, delta);
        }

        public void CodeVector_of_M34l_(ref Vector<M34l> value)
        {
            M34l[] data = null; CodeM34lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M34l>(data, origin, size, delta);
        }

        public void CodeVector_of_M34f_(ref Vector<M34f> value)
        {
            M34f[] data = null; CodeM34fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M34f>(data, origin, size, delta);
        }

        public void CodeVector_of_M34d_(ref Vector<M34d> value)
        {
            M34d[] data = null; CodeM34dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M34d>(data, origin, size, delta);
        }

        public void CodeVector_of_M44i_(ref Vector<M44i> value)
        {
            M44i[] data = null; CodeM44iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M44i>(data, origin, size, delta);
        }

        public void CodeVector_of_M44l_(ref Vector<M44l> value)
        {
            M44l[] data = null; CodeM44lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M44l>(data, origin, size, delta);
        }

        public void CodeVector_of_M44f_(ref Vector<M44f> value)
        {
            M44f[] data = null; CodeM44fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M44f>(data, origin, size, delta);
        }

        public void CodeVector_of_M44d_(ref Vector<M44d> value)
        {
            M44d[] data = null; CodeM44dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<M44d>(data, origin, size, delta);
        }

        public void CodeVector_of_C3b_(ref Vector<C3b> value)
        {
            C3b[] data = null; CodeC3bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C3b>(data, origin, size, delta);
        }

        public void CodeVector_of_C3us_(ref Vector<C3us> value)
        {
            C3us[] data = null; CodeC3usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C3us>(data, origin, size, delta);
        }

        public void CodeVector_of_C3ui_(ref Vector<C3ui> value)
        {
            C3ui[] data = null; CodeC3uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C3ui>(data, origin, size, delta);
        }

        public void CodeVector_of_C3f_(ref Vector<C3f> value)
        {
            C3f[] data = null; CodeC3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C3f>(data, origin, size, delta);
        }

        public void CodeVector_of_C3d_(ref Vector<C3d> value)
        {
            C3d[] data = null; CodeC3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C3d>(data, origin, size, delta);
        }

        public void CodeVector_of_C4b_(ref Vector<C4b> value)
        {
            C4b[] data = null; CodeC4bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C4b>(data, origin, size, delta);
        }

        public void CodeVector_of_C4us_(ref Vector<C4us> value)
        {
            C4us[] data = null; CodeC4usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C4us>(data, origin, size, delta);
        }

        public void CodeVector_of_C4ui_(ref Vector<C4ui> value)
        {
            C4ui[] data = null; CodeC4uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C4ui>(data, origin, size, delta);
        }

        public void CodeVector_of_C4f_(ref Vector<C4f> value)
        {
            C4f[] data = null; CodeC4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C4f>(data, origin, size, delta);
        }

        public void CodeVector_of_C4d_(ref Vector<C4d> value)
        {
            C4d[] data = null; CodeC4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<C4d>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1b_(ref Vector<Range1b> value)
        {
            Range1b[] data = null; CodeRange1bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1b>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1sb_(ref Vector<Range1sb> value)
        {
            Range1sb[] data = null; CodeRange1sbArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1sb>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1s_(ref Vector<Range1s> value)
        {
            Range1s[] data = null; CodeRange1sArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1s>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1us_(ref Vector<Range1us> value)
        {
            Range1us[] data = null; CodeRange1usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1us>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1i_(ref Vector<Range1i> value)
        {
            Range1i[] data = null; CodeRange1iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1i>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1ui_(ref Vector<Range1ui> value)
        {
            Range1ui[] data = null; CodeRange1uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1ui>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1l_(ref Vector<Range1l> value)
        {
            Range1l[] data = null; CodeRange1lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1l>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1ul_(ref Vector<Range1ul> value)
        {
            Range1ul[] data = null; CodeRange1ulArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1ul>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1f_(ref Vector<Range1f> value)
        {
            Range1f[] data = null; CodeRange1fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1f>(data, origin, size, delta);
        }

        public void CodeVector_of_Range1d_(ref Vector<Range1d> value)
        {
            Range1d[] data = null; CodeRange1dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Range1d>(data, origin, size, delta);
        }

        public void CodeVector_of_Box2i_(ref Vector<Box2i> value)
        {
            Box2i[] data = null; CodeBox2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box2i>(data, origin, size, delta);
        }

        public void CodeVector_of_Box2l_(ref Vector<Box2l> value)
        {
            Box2l[] data = null; CodeBox2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box2l>(data, origin, size, delta);
        }

        public void CodeVector_of_Box2f_(ref Vector<Box2f> value)
        {
            Box2f[] data = null; CodeBox2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Box2d_(ref Vector<Box2d> value)
        {
            Box2d[] data = null; CodeBox2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Box3i_(ref Vector<Box3i> value)
        {
            Box3i[] data = null; CodeBox3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box3i>(data, origin, size, delta);
        }

        public void CodeVector_of_Box3l_(ref Vector<Box3l> value)
        {
            Box3l[] data = null; CodeBox3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box3l>(data, origin, size, delta);
        }

        public void CodeVector_of_Box3f_(ref Vector<Box3f> value)
        {
            Box3f[] data = null; CodeBox3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Box3d_(ref Vector<Box3d> value)
        {
            Box3d[] data = null; CodeBox3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Box3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Euclidean3f_(ref Vector<Euclidean3f> value)
        {
            Euclidean3f[] data = null; CodeEuclidean3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Euclidean3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Euclidean3d_(ref Vector<Euclidean3d> value)
        {
            Euclidean3d[] data = null; CodeEuclidean3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Euclidean3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Rot2f_(ref Vector<Rot2f> value)
        {
            Rot2f[] data = null; CodeRot2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Rot2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Rot2d_(ref Vector<Rot2d> value)
        {
            Rot2d[] data = null; CodeRot2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Rot2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Rot3f_(ref Vector<Rot3f> value)
        {
            Rot3f[] data = null; CodeRot3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Rot3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Rot3d_(ref Vector<Rot3d> value)
        {
            Rot3d[] data = null; CodeRot3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Rot3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Scale3f_(ref Vector<Scale3f> value)
        {
            Scale3f[] data = null; CodeScale3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Scale3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Scale3d_(ref Vector<Scale3d> value)
        {
            Scale3d[] data = null; CodeScale3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Scale3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Shift3f_(ref Vector<Shift3f> value)
        {
            Shift3f[] data = null; CodeShift3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Shift3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Shift3d_(ref Vector<Shift3d> value)
        {
            Shift3d[] data = null; CodeShift3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Shift3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Trafo2f_(ref Vector<Trafo2f> value)
        {
            Trafo2f[] data = null; CodeTrafo2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Trafo2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Trafo2d_(ref Vector<Trafo2d> value)
        {
            Trafo2d[] data = null; CodeTrafo2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Trafo2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Trafo3f_(ref Vector<Trafo3f> value)
        {
            Trafo3f[] data = null; CodeTrafo3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Trafo3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Trafo3d_(ref Vector<Trafo3d> value)
        {
            Trafo3d[] data = null; CodeTrafo3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Trafo3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Bool_(ref Vector<bool> value)
        {
            bool[] data = null; CodeBoolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<bool>(data, origin, size, delta);
        }

        public void CodeVector_of_Char_(ref Vector<char> value)
        {
            char[] data = null; CodeCharArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<char>(data, origin, size, delta);
        }

        public void CodeVector_of_String_(ref Vector<string> value)
        {
            string[] data = null; CodeStringArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<string>(data, origin, size, delta);
        }

        public void CodeVector_of_Type_(ref Vector<Type> value)
        {
            Type[] data = null; CodeTypeArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Type>(data, origin, size, delta);
        }

        public void CodeVector_of_Guid_(ref Vector<Guid> value)
        {
            Guid[] data = null; CodeGuidArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Guid>(data, origin, size, delta);
        }

        public void CodeVector_of_Symbol_(ref Vector<Symbol> value)
        {
            Symbol[] data = null; CodeSymbolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Symbol>(data, origin, size, delta);
        }

        public void CodeVector_of_Circle2d_(ref Vector<Circle2d> value)
        {
            Circle2d[] data = null; CodeCircle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Circle2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Line2d_(ref Vector<Line2d> value)
        {
            Line2d[] data = null; CodeLine2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Line2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Line3d_(ref Vector<Line3d> value)
        {
            Line3d[] data = null; CodeLine3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Line3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Plane2d_(ref Vector<Plane2d> value)
        {
            Plane2d[] data = null; CodePlane2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Plane2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Plane3d_(ref Vector<Plane3d> value)
        {
            Plane3d[] data = null; CodePlane3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Plane3d>(data, origin, size, delta);
        }

        public void CodeVector_of_PlaneWithPoint3d_(ref Vector<PlaneWithPoint3d> value)
        {
            PlaneWithPoint3d[] data = null; CodePlaneWithPoint3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<PlaneWithPoint3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Quad2d_(ref Vector<Quad2d> value)
        {
            Quad2d[] data = null; CodeQuad2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Quad2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Quad3d_(ref Vector<Quad3d> value)
        {
            Quad3d[] data = null; CodeQuad3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Quad3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Ray2d_(ref Vector<Ray2d> value)
        {
            Ray2d[] data = null; CodeRay2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Ray2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Ray3d_(ref Vector<Ray3d> value)
        {
            Ray3d[] data = null; CodeRay3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Ray3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Sphere3d_(ref Vector<Sphere3d> value)
        {
            Sphere3d[] data = null; CodeSphere3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Sphere3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Triangle2d_(ref Vector<Triangle2d> value)
        {
            Triangle2d[] data = null; CodeTriangle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Triangle2d>(data, origin, size, delta);
        }

        public void CodeVector_of_Triangle3d_(ref Vector<Triangle3d> value)
        {
            Triangle3d[] data = null; CodeTriangle3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Triangle3d>(data, origin, size, delta);
        }

        public void CodeVector_of_Circle2f_(ref Vector<Circle2f> value)
        {
            Circle2f[] data = null; CodeCircle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Circle2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Line2f_(ref Vector<Line2f> value)
        {
            Line2f[] data = null; CodeLine2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Line2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Line3f_(ref Vector<Line3f> value)
        {
            Line3f[] data = null; CodeLine3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Line3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Plane2f_(ref Vector<Plane2f> value)
        {
            Plane2f[] data = null; CodePlane2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Plane2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Plane3f_(ref Vector<Plane3f> value)
        {
            Plane3f[] data = null; CodePlane3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Plane3f>(data, origin, size, delta);
        }

        public void CodeVector_of_PlaneWithPoint3f_(ref Vector<PlaneWithPoint3f> value)
        {
            PlaneWithPoint3f[] data = null; CodePlaneWithPoint3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<PlaneWithPoint3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Quad2f_(ref Vector<Quad2f> value)
        {
            Quad2f[] data = null; CodeQuad2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Quad2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Quad3f_(ref Vector<Quad3f> value)
        {
            Quad3f[] data = null; CodeQuad3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Quad3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Ray2f_(ref Vector<Ray2f> value)
        {
            Ray2f[] data = null; CodeRay2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Ray2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Ray3f_(ref Vector<Ray3f> value)
        {
            Ray3f[] data = null; CodeRay3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Ray3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Sphere3f_(ref Vector<Sphere3f> value)
        {
            Sphere3f[] data = null; CodeSphere3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Sphere3f>(data, origin, size, delta);
        }

        public void CodeVector_of_Triangle2f_(ref Vector<Triangle2f> value)
        {
            Triangle2f[] data = null; CodeTriangle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Triangle2f>(data, origin, size, delta);
        }

        public void CodeVector_of_Triangle3f_(ref Vector<Triangle3f> value)
        {
            Triangle3f[] data = null; CodeTriangle3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long size = default(long); CodeLong(ref size);
            long delta = default(long); CodeLong(ref delta);
            value = new Vector<Triangle3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Byte_(ref Matrix<byte> value)
        {
            byte[] data = null; CodeByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<byte>(data, origin, size, delta);
        }

        public void CodeMatrix_of_SByte_(ref Matrix<sbyte> value)
        {
            sbyte[] data = null; CodeSByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<sbyte>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Short_(ref Matrix<short> value)
        {
            short[] data = null; CodeShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<short>(data, origin, size, delta);
        }

        public void CodeMatrix_of_UShort_(ref Matrix<ushort> value)
        {
            ushort[] data = null; CodeUShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<ushort>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Int_(ref Matrix<int> value)
        {
            int[] data = null; CodeIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<int>(data, origin, size, delta);
        }

        public void CodeMatrix_of_UInt_(ref Matrix<uint> value)
        {
            uint[] data = null; CodeUIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<uint>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Long_(ref Matrix<long> value)
        {
            long[] data = null; CodeLongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<long>(data, origin, size, delta);
        }

        public void CodeMatrix_of_ULong_(ref Matrix<ulong> value)
        {
            ulong[] data = null; CodeULongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<ulong>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Float_(ref Matrix<float> value)
        {
            float[] data = null; CodeFloatArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<float>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Double_(ref Matrix<double> value)
        {
            double[] data = null; CodeDoubleArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<double>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Fraction_(ref Matrix<Fraction> value)
        {
            Fraction[] data = null; CodeFractionArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Fraction>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V2i_(ref Matrix<V2i> value)
        {
            V2i[] data = null; CodeV2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V2i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V2l_(ref Matrix<V2l> value)
        {
            V2l[] data = null; CodeV2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V2l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V2f_(ref Matrix<V2f> value)
        {
            V2f[] data = null; CodeV2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V2d_(ref Matrix<V2d> value)
        {
            V2d[] data = null; CodeV2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V3i_(ref Matrix<V3i> value)
        {
            V3i[] data = null; CodeV3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V3i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V3l_(ref Matrix<V3l> value)
        {
            V3l[] data = null; CodeV3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V3l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V3f_(ref Matrix<V3f> value)
        {
            V3f[] data = null; CodeV3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V3d_(ref Matrix<V3d> value)
        {
            V3d[] data = null; CodeV3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V4i_(ref Matrix<V4i> value)
        {
            V4i[] data = null; CodeV4iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V4i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V4l_(ref Matrix<V4l> value)
        {
            V4l[] data = null; CodeV4lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V4l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V4f_(ref Matrix<V4f> value)
        {
            V4f[] data = null; CodeV4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V4f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_V4d_(ref Matrix<V4d> value)
        {
            V4d[] data = null; CodeV4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<V4d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M22i_(ref Matrix<M22i> value)
        {
            M22i[] data = null; CodeM22iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M22i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M22l_(ref Matrix<M22l> value)
        {
            M22l[] data = null; CodeM22lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M22l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M22f_(ref Matrix<M22f> value)
        {
            M22f[] data = null; CodeM22fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M22f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M22d_(ref Matrix<M22d> value)
        {
            M22d[] data = null; CodeM22dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M22d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M23i_(ref Matrix<M23i> value)
        {
            M23i[] data = null; CodeM23iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M23i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M23l_(ref Matrix<M23l> value)
        {
            M23l[] data = null; CodeM23lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M23l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M23f_(ref Matrix<M23f> value)
        {
            M23f[] data = null; CodeM23fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M23f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M23d_(ref Matrix<M23d> value)
        {
            M23d[] data = null; CodeM23dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M23d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M33i_(ref Matrix<M33i> value)
        {
            M33i[] data = null; CodeM33iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M33i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M33l_(ref Matrix<M33l> value)
        {
            M33l[] data = null; CodeM33lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M33l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M33f_(ref Matrix<M33f> value)
        {
            M33f[] data = null; CodeM33fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M33f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M33d_(ref Matrix<M33d> value)
        {
            M33d[] data = null; CodeM33dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M33d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M34i_(ref Matrix<M34i> value)
        {
            M34i[] data = null; CodeM34iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M34i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M34l_(ref Matrix<M34l> value)
        {
            M34l[] data = null; CodeM34lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M34l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M34f_(ref Matrix<M34f> value)
        {
            M34f[] data = null; CodeM34fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M34f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M34d_(ref Matrix<M34d> value)
        {
            M34d[] data = null; CodeM34dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M34d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M44i_(ref Matrix<M44i> value)
        {
            M44i[] data = null; CodeM44iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M44i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M44l_(ref Matrix<M44l> value)
        {
            M44l[] data = null; CodeM44lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M44l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M44f_(ref Matrix<M44f> value)
        {
            M44f[] data = null; CodeM44fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M44f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_M44d_(ref Matrix<M44d> value)
        {
            M44d[] data = null; CodeM44dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<M44d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C3b_(ref Matrix<C3b> value)
        {
            C3b[] data = null; CodeC3bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C3b>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C3us_(ref Matrix<C3us> value)
        {
            C3us[] data = null; CodeC3usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C3us>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C3ui_(ref Matrix<C3ui> value)
        {
            C3ui[] data = null; CodeC3uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C3ui>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C3f_(ref Matrix<C3f> value)
        {
            C3f[] data = null; CodeC3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C3d_(ref Matrix<C3d> value)
        {
            C3d[] data = null; CodeC3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C4b_(ref Matrix<C4b> value)
        {
            C4b[] data = null; CodeC4bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C4b>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C4us_(ref Matrix<C4us> value)
        {
            C4us[] data = null; CodeC4usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C4us>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C4ui_(ref Matrix<C4ui> value)
        {
            C4ui[] data = null; CodeC4uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C4ui>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C4f_(ref Matrix<C4f> value)
        {
            C4f[] data = null; CodeC4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C4f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_C4d_(ref Matrix<C4d> value)
        {
            C4d[] data = null; CodeC4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<C4d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1b_(ref Matrix<Range1b> value)
        {
            Range1b[] data = null; CodeRange1bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1b>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1sb_(ref Matrix<Range1sb> value)
        {
            Range1sb[] data = null; CodeRange1sbArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1sb>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1s_(ref Matrix<Range1s> value)
        {
            Range1s[] data = null; CodeRange1sArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1s>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1us_(ref Matrix<Range1us> value)
        {
            Range1us[] data = null; CodeRange1usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1us>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1i_(ref Matrix<Range1i> value)
        {
            Range1i[] data = null; CodeRange1iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1ui_(ref Matrix<Range1ui> value)
        {
            Range1ui[] data = null; CodeRange1uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1ui>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1l_(ref Matrix<Range1l> value)
        {
            Range1l[] data = null; CodeRange1lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1ul_(ref Matrix<Range1ul> value)
        {
            Range1ul[] data = null; CodeRange1ulArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1ul>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1f_(ref Matrix<Range1f> value)
        {
            Range1f[] data = null; CodeRange1fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Range1d_(ref Matrix<Range1d> value)
        {
            Range1d[] data = null; CodeRange1dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Range1d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box2i_(ref Matrix<Box2i> value)
        {
            Box2i[] data = null; CodeBox2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box2i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box2l_(ref Matrix<Box2l> value)
        {
            Box2l[] data = null; CodeBox2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box2l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box2f_(ref Matrix<Box2f> value)
        {
            Box2f[] data = null; CodeBox2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box2d_(ref Matrix<Box2d> value)
        {
            Box2d[] data = null; CodeBox2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box3i_(ref Matrix<Box3i> value)
        {
            Box3i[] data = null; CodeBox3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box3i>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box3l_(ref Matrix<Box3l> value)
        {
            Box3l[] data = null; CodeBox3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box3l>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box3f_(ref Matrix<Box3f> value)
        {
            Box3f[] data = null; CodeBox3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Box3d_(ref Matrix<Box3d> value)
        {
            Box3d[] data = null; CodeBox3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Box3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Euclidean3f_(ref Matrix<Euclidean3f> value)
        {
            Euclidean3f[] data = null; CodeEuclidean3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Euclidean3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Euclidean3d_(ref Matrix<Euclidean3d> value)
        {
            Euclidean3d[] data = null; CodeEuclidean3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Euclidean3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Rot2f_(ref Matrix<Rot2f> value)
        {
            Rot2f[] data = null; CodeRot2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Rot2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Rot2d_(ref Matrix<Rot2d> value)
        {
            Rot2d[] data = null; CodeRot2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Rot2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Rot3f_(ref Matrix<Rot3f> value)
        {
            Rot3f[] data = null; CodeRot3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Rot3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Rot3d_(ref Matrix<Rot3d> value)
        {
            Rot3d[] data = null; CodeRot3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Rot3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Scale3f_(ref Matrix<Scale3f> value)
        {
            Scale3f[] data = null; CodeScale3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Scale3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Scale3d_(ref Matrix<Scale3d> value)
        {
            Scale3d[] data = null; CodeScale3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Scale3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Shift3f_(ref Matrix<Shift3f> value)
        {
            Shift3f[] data = null; CodeShift3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Shift3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Shift3d_(ref Matrix<Shift3d> value)
        {
            Shift3d[] data = null; CodeShift3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Shift3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Trafo2f_(ref Matrix<Trafo2f> value)
        {
            Trafo2f[] data = null; CodeTrafo2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Trafo2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Trafo2d_(ref Matrix<Trafo2d> value)
        {
            Trafo2d[] data = null; CodeTrafo2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Trafo2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Trafo3f_(ref Matrix<Trafo3f> value)
        {
            Trafo3f[] data = null; CodeTrafo3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Trafo3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Trafo3d_(ref Matrix<Trafo3d> value)
        {
            Trafo3d[] data = null; CodeTrafo3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Trafo3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Bool_(ref Matrix<bool> value)
        {
            bool[] data = null; CodeBoolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<bool>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Char_(ref Matrix<char> value)
        {
            char[] data = null; CodeCharArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<char>(data, origin, size, delta);
        }

        public void CodeMatrix_of_String_(ref Matrix<string> value)
        {
            string[] data = null; CodeStringArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<string>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Type_(ref Matrix<Type> value)
        {
            Type[] data = null; CodeTypeArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Type>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Guid_(ref Matrix<Guid> value)
        {
            Guid[] data = null; CodeGuidArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Guid>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Symbol_(ref Matrix<Symbol> value)
        {
            Symbol[] data = null; CodeSymbolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Symbol>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Circle2d_(ref Matrix<Circle2d> value)
        {
            Circle2d[] data = null; CodeCircle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Circle2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Line2d_(ref Matrix<Line2d> value)
        {
            Line2d[] data = null; CodeLine2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Line2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Line3d_(ref Matrix<Line3d> value)
        {
            Line3d[] data = null; CodeLine3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Line3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Plane2d_(ref Matrix<Plane2d> value)
        {
            Plane2d[] data = null; CodePlane2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Plane2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Plane3d_(ref Matrix<Plane3d> value)
        {
            Plane3d[] data = null; CodePlane3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Plane3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_PlaneWithPoint3d_(ref Matrix<PlaneWithPoint3d> value)
        {
            PlaneWithPoint3d[] data = null; CodePlaneWithPoint3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<PlaneWithPoint3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Quad2d_(ref Matrix<Quad2d> value)
        {
            Quad2d[] data = null; CodeQuad2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Quad2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Quad3d_(ref Matrix<Quad3d> value)
        {
            Quad3d[] data = null; CodeQuad3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Quad3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Ray2d_(ref Matrix<Ray2d> value)
        {
            Ray2d[] data = null; CodeRay2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Ray2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Ray3d_(ref Matrix<Ray3d> value)
        {
            Ray3d[] data = null; CodeRay3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Ray3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Sphere3d_(ref Matrix<Sphere3d> value)
        {
            Sphere3d[] data = null; CodeSphere3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Sphere3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Triangle2d_(ref Matrix<Triangle2d> value)
        {
            Triangle2d[] data = null; CodeTriangle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Triangle2d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Triangle3d_(ref Matrix<Triangle3d> value)
        {
            Triangle3d[] data = null; CodeTriangle3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Triangle3d>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Circle2f_(ref Matrix<Circle2f> value)
        {
            Circle2f[] data = null; CodeCircle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Circle2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Line2f_(ref Matrix<Line2f> value)
        {
            Line2f[] data = null; CodeLine2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Line2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Line3f_(ref Matrix<Line3f> value)
        {
            Line3f[] data = null; CodeLine3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Line3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Plane2f_(ref Matrix<Plane2f> value)
        {
            Plane2f[] data = null; CodePlane2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Plane2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Plane3f_(ref Matrix<Plane3f> value)
        {
            Plane3f[] data = null; CodePlane3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Plane3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_PlaneWithPoint3f_(ref Matrix<PlaneWithPoint3f> value)
        {
            PlaneWithPoint3f[] data = null; CodePlaneWithPoint3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<PlaneWithPoint3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Quad2f_(ref Matrix<Quad2f> value)
        {
            Quad2f[] data = null; CodeQuad2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Quad2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Quad3f_(ref Matrix<Quad3f> value)
        {
            Quad3f[] data = null; CodeQuad3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Quad3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Ray2f_(ref Matrix<Ray2f> value)
        {
            Ray2f[] data = null; CodeRay2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Ray2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Ray3f_(ref Matrix<Ray3f> value)
        {
            Ray3f[] data = null; CodeRay3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Ray3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Sphere3f_(ref Matrix<Sphere3f> value)
        {
            Sphere3f[] data = null; CodeSphere3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Sphere3f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Triangle2f_(ref Matrix<Triangle2f> value)
        {
            Triangle2f[] data = null; CodeTriangle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Triangle2f>(data, origin, size, delta);
        }

        public void CodeMatrix_of_Triangle3f_(ref Matrix<Triangle3f> value)
        {
            Triangle3f[] data = null; CodeTriangle3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V2l size = default(V2l); CodeV2l(ref size);
            V2l delta = default(V2l); CodeV2l(ref delta);
            value = new Matrix<Triangle3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Byte_(ref Volume<byte> value)
        {
            byte[] data = null; CodeByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<byte>(data, origin, size, delta);
        }

        public void CodeVolume_of_SByte_(ref Volume<sbyte> value)
        {
            sbyte[] data = null; CodeSByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<sbyte>(data, origin, size, delta);
        }

        public void CodeVolume_of_Short_(ref Volume<short> value)
        {
            short[] data = null; CodeShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<short>(data, origin, size, delta);
        }

        public void CodeVolume_of_UShort_(ref Volume<ushort> value)
        {
            ushort[] data = null; CodeUShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<ushort>(data, origin, size, delta);
        }

        public void CodeVolume_of_Int_(ref Volume<int> value)
        {
            int[] data = null; CodeIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<int>(data, origin, size, delta);
        }

        public void CodeVolume_of_UInt_(ref Volume<uint> value)
        {
            uint[] data = null; CodeUIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<uint>(data, origin, size, delta);
        }

        public void CodeVolume_of_Long_(ref Volume<long> value)
        {
            long[] data = null; CodeLongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<long>(data, origin, size, delta);
        }

        public void CodeVolume_of_ULong_(ref Volume<ulong> value)
        {
            ulong[] data = null; CodeULongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<ulong>(data, origin, size, delta);
        }

        public void CodeVolume_of_Float_(ref Volume<float> value)
        {
            float[] data = null; CodeFloatArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<float>(data, origin, size, delta);
        }

        public void CodeVolume_of_Double_(ref Volume<double> value)
        {
            double[] data = null; CodeDoubleArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<double>(data, origin, size, delta);
        }

        public void CodeVolume_of_Fraction_(ref Volume<Fraction> value)
        {
            Fraction[] data = null; CodeFractionArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Fraction>(data, origin, size, delta);
        }

        public void CodeVolume_of_V2i_(ref Volume<V2i> value)
        {
            V2i[] data = null; CodeV2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V2i>(data, origin, size, delta);
        }

        public void CodeVolume_of_V2l_(ref Volume<V2l> value)
        {
            V2l[] data = null; CodeV2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V2l>(data, origin, size, delta);
        }

        public void CodeVolume_of_V2f_(ref Volume<V2f> value)
        {
            V2f[] data = null; CodeV2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_V2d_(ref Volume<V2d> value)
        {
            V2d[] data = null; CodeV2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_V3i_(ref Volume<V3i> value)
        {
            V3i[] data = null; CodeV3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V3i>(data, origin, size, delta);
        }

        public void CodeVolume_of_V3l_(ref Volume<V3l> value)
        {
            V3l[] data = null; CodeV3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V3l>(data, origin, size, delta);
        }

        public void CodeVolume_of_V3f_(ref Volume<V3f> value)
        {
            V3f[] data = null; CodeV3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_V3d_(ref Volume<V3d> value)
        {
            V3d[] data = null; CodeV3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_V4i_(ref Volume<V4i> value)
        {
            V4i[] data = null; CodeV4iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V4i>(data, origin, size, delta);
        }

        public void CodeVolume_of_V4l_(ref Volume<V4l> value)
        {
            V4l[] data = null; CodeV4lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V4l>(data, origin, size, delta);
        }

        public void CodeVolume_of_V4f_(ref Volume<V4f> value)
        {
            V4f[] data = null; CodeV4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V4f>(data, origin, size, delta);
        }

        public void CodeVolume_of_V4d_(ref Volume<V4d> value)
        {
            V4d[] data = null; CodeV4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<V4d>(data, origin, size, delta);
        }

        public void CodeVolume_of_M22i_(ref Volume<M22i> value)
        {
            M22i[] data = null; CodeM22iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M22i>(data, origin, size, delta);
        }

        public void CodeVolume_of_M22l_(ref Volume<M22l> value)
        {
            M22l[] data = null; CodeM22lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M22l>(data, origin, size, delta);
        }

        public void CodeVolume_of_M22f_(ref Volume<M22f> value)
        {
            M22f[] data = null; CodeM22fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M22f>(data, origin, size, delta);
        }

        public void CodeVolume_of_M22d_(ref Volume<M22d> value)
        {
            M22d[] data = null; CodeM22dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M22d>(data, origin, size, delta);
        }

        public void CodeVolume_of_M23i_(ref Volume<M23i> value)
        {
            M23i[] data = null; CodeM23iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M23i>(data, origin, size, delta);
        }

        public void CodeVolume_of_M23l_(ref Volume<M23l> value)
        {
            M23l[] data = null; CodeM23lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M23l>(data, origin, size, delta);
        }

        public void CodeVolume_of_M23f_(ref Volume<M23f> value)
        {
            M23f[] data = null; CodeM23fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M23f>(data, origin, size, delta);
        }

        public void CodeVolume_of_M23d_(ref Volume<M23d> value)
        {
            M23d[] data = null; CodeM23dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M23d>(data, origin, size, delta);
        }

        public void CodeVolume_of_M33i_(ref Volume<M33i> value)
        {
            M33i[] data = null; CodeM33iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M33i>(data, origin, size, delta);
        }

        public void CodeVolume_of_M33l_(ref Volume<M33l> value)
        {
            M33l[] data = null; CodeM33lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M33l>(data, origin, size, delta);
        }

        public void CodeVolume_of_M33f_(ref Volume<M33f> value)
        {
            M33f[] data = null; CodeM33fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M33f>(data, origin, size, delta);
        }

        public void CodeVolume_of_M33d_(ref Volume<M33d> value)
        {
            M33d[] data = null; CodeM33dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M33d>(data, origin, size, delta);
        }

        public void CodeVolume_of_M34i_(ref Volume<M34i> value)
        {
            M34i[] data = null; CodeM34iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M34i>(data, origin, size, delta);
        }

        public void CodeVolume_of_M34l_(ref Volume<M34l> value)
        {
            M34l[] data = null; CodeM34lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M34l>(data, origin, size, delta);
        }

        public void CodeVolume_of_M34f_(ref Volume<M34f> value)
        {
            M34f[] data = null; CodeM34fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M34f>(data, origin, size, delta);
        }

        public void CodeVolume_of_M34d_(ref Volume<M34d> value)
        {
            M34d[] data = null; CodeM34dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M34d>(data, origin, size, delta);
        }

        public void CodeVolume_of_M44i_(ref Volume<M44i> value)
        {
            M44i[] data = null; CodeM44iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M44i>(data, origin, size, delta);
        }

        public void CodeVolume_of_M44l_(ref Volume<M44l> value)
        {
            M44l[] data = null; CodeM44lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M44l>(data, origin, size, delta);
        }

        public void CodeVolume_of_M44f_(ref Volume<M44f> value)
        {
            M44f[] data = null; CodeM44fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M44f>(data, origin, size, delta);
        }

        public void CodeVolume_of_M44d_(ref Volume<M44d> value)
        {
            M44d[] data = null; CodeM44dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<M44d>(data, origin, size, delta);
        }

        public void CodeVolume_of_C3b_(ref Volume<C3b> value)
        {
            C3b[] data = null; CodeC3bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C3b>(data, origin, size, delta);
        }

        public void CodeVolume_of_C3us_(ref Volume<C3us> value)
        {
            C3us[] data = null; CodeC3usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C3us>(data, origin, size, delta);
        }

        public void CodeVolume_of_C3ui_(ref Volume<C3ui> value)
        {
            C3ui[] data = null; CodeC3uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C3ui>(data, origin, size, delta);
        }

        public void CodeVolume_of_C3f_(ref Volume<C3f> value)
        {
            C3f[] data = null; CodeC3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_C3d_(ref Volume<C3d> value)
        {
            C3d[] data = null; CodeC3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_C4b_(ref Volume<C4b> value)
        {
            C4b[] data = null; CodeC4bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C4b>(data, origin, size, delta);
        }

        public void CodeVolume_of_C4us_(ref Volume<C4us> value)
        {
            C4us[] data = null; CodeC4usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C4us>(data, origin, size, delta);
        }

        public void CodeVolume_of_C4ui_(ref Volume<C4ui> value)
        {
            C4ui[] data = null; CodeC4uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C4ui>(data, origin, size, delta);
        }

        public void CodeVolume_of_C4f_(ref Volume<C4f> value)
        {
            C4f[] data = null; CodeC4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C4f>(data, origin, size, delta);
        }

        public void CodeVolume_of_C4d_(ref Volume<C4d> value)
        {
            C4d[] data = null; CodeC4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<C4d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1b_(ref Volume<Range1b> value)
        {
            Range1b[] data = null; CodeRange1bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1b>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1sb_(ref Volume<Range1sb> value)
        {
            Range1sb[] data = null; CodeRange1sbArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1sb>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1s_(ref Volume<Range1s> value)
        {
            Range1s[] data = null; CodeRange1sArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1s>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1us_(ref Volume<Range1us> value)
        {
            Range1us[] data = null; CodeRange1usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1us>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1i_(ref Volume<Range1i> value)
        {
            Range1i[] data = null; CodeRange1iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1i>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1ui_(ref Volume<Range1ui> value)
        {
            Range1ui[] data = null; CodeRange1uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1ui>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1l_(ref Volume<Range1l> value)
        {
            Range1l[] data = null; CodeRange1lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1l>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1ul_(ref Volume<Range1ul> value)
        {
            Range1ul[] data = null; CodeRange1ulArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1ul>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1f_(ref Volume<Range1f> value)
        {
            Range1f[] data = null; CodeRange1fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Range1d_(ref Volume<Range1d> value)
        {
            Range1d[] data = null; CodeRange1dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Range1d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box2i_(ref Volume<Box2i> value)
        {
            Box2i[] data = null; CodeBox2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box2i>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box2l_(ref Volume<Box2l> value)
        {
            Box2l[] data = null; CodeBox2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box2l>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box2f_(ref Volume<Box2f> value)
        {
            Box2f[] data = null; CodeBox2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box2d_(ref Volume<Box2d> value)
        {
            Box2d[] data = null; CodeBox2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box3i_(ref Volume<Box3i> value)
        {
            Box3i[] data = null; CodeBox3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box3i>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box3l_(ref Volume<Box3l> value)
        {
            Box3l[] data = null; CodeBox3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box3l>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box3f_(ref Volume<Box3f> value)
        {
            Box3f[] data = null; CodeBox3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Box3d_(ref Volume<Box3d> value)
        {
            Box3d[] data = null; CodeBox3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Box3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Euclidean3f_(ref Volume<Euclidean3f> value)
        {
            Euclidean3f[] data = null; CodeEuclidean3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Euclidean3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Euclidean3d_(ref Volume<Euclidean3d> value)
        {
            Euclidean3d[] data = null; CodeEuclidean3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Euclidean3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Rot2f_(ref Volume<Rot2f> value)
        {
            Rot2f[] data = null; CodeRot2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Rot2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Rot2d_(ref Volume<Rot2d> value)
        {
            Rot2d[] data = null; CodeRot2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Rot2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Rot3f_(ref Volume<Rot3f> value)
        {
            Rot3f[] data = null; CodeRot3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Rot3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Rot3d_(ref Volume<Rot3d> value)
        {
            Rot3d[] data = null; CodeRot3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Rot3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Scale3f_(ref Volume<Scale3f> value)
        {
            Scale3f[] data = null; CodeScale3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Scale3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Scale3d_(ref Volume<Scale3d> value)
        {
            Scale3d[] data = null; CodeScale3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Scale3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Shift3f_(ref Volume<Shift3f> value)
        {
            Shift3f[] data = null; CodeShift3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Shift3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Shift3d_(ref Volume<Shift3d> value)
        {
            Shift3d[] data = null; CodeShift3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Shift3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Trafo2f_(ref Volume<Trafo2f> value)
        {
            Trafo2f[] data = null; CodeTrafo2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Trafo2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Trafo2d_(ref Volume<Trafo2d> value)
        {
            Trafo2d[] data = null; CodeTrafo2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Trafo2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Trafo3f_(ref Volume<Trafo3f> value)
        {
            Trafo3f[] data = null; CodeTrafo3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Trafo3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Trafo3d_(ref Volume<Trafo3d> value)
        {
            Trafo3d[] data = null; CodeTrafo3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Trafo3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Bool_(ref Volume<bool> value)
        {
            bool[] data = null; CodeBoolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<bool>(data, origin, size, delta);
        }

        public void CodeVolume_of_Char_(ref Volume<char> value)
        {
            char[] data = null; CodeCharArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<char>(data, origin, size, delta);
        }

        public void CodeVolume_of_String_(ref Volume<string> value)
        {
            string[] data = null; CodeStringArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<string>(data, origin, size, delta);
        }

        public void CodeVolume_of_Type_(ref Volume<Type> value)
        {
            Type[] data = null; CodeTypeArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Type>(data, origin, size, delta);
        }

        public void CodeVolume_of_Guid_(ref Volume<Guid> value)
        {
            Guid[] data = null; CodeGuidArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Guid>(data, origin, size, delta);
        }

        public void CodeVolume_of_Symbol_(ref Volume<Symbol> value)
        {
            Symbol[] data = null; CodeSymbolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Symbol>(data, origin, size, delta);
        }

        public void CodeVolume_of_Circle2d_(ref Volume<Circle2d> value)
        {
            Circle2d[] data = null; CodeCircle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Circle2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Line2d_(ref Volume<Line2d> value)
        {
            Line2d[] data = null; CodeLine2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Line2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Line3d_(ref Volume<Line3d> value)
        {
            Line3d[] data = null; CodeLine3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Line3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Plane2d_(ref Volume<Plane2d> value)
        {
            Plane2d[] data = null; CodePlane2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Plane2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Plane3d_(ref Volume<Plane3d> value)
        {
            Plane3d[] data = null; CodePlane3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Plane3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_PlaneWithPoint3d_(ref Volume<PlaneWithPoint3d> value)
        {
            PlaneWithPoint3d[] data = null; CodePlaneWithPoint3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<PlaneWithPoint3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Quad2d_(ref Volume<Quad2d> value)
        {
            Quad2d[] data = null; CodeQuad2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Quad2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Quad3d_(ref Volume<Quad3d> value)
        {
            Quad3d[] data = null; CodeQuad3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Quad3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Ray2d_(ref Volume<Ray2d> value)
        {
            Ray2d[] data = null; CodeRay2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Ray2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Ray3d_(ref Volume<Ray3d> value)
        {
            Ray3d[] data = null; CodeRay3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Ray3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Sphere3d_(ref Volume<Sphere3d> value)
        {
            Sphere3d[] data = null; CodeSphere3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Sphere3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Triangle2d_(ref Volume<Triangle2d> value)
        {
            Triangle2d[] data = null; CodeTriangle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Triangle2d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Triangle3d_(ref Volume<Triangle3d> value)
        {
            Triangle3d[] data = null; CodeTriangle3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Triangle3d>(data, origin, size, delta);
        }

        public void CodeVolume_of_Circle2f_(ref Volume<Circle2f> value)
        {
            Circle2f[] data = null; CodeCircle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Circle2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Line2f_(ref Volume<Line2f> value)
        {
            Line2f[] data = null; CodeLine2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Line2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Line3f_(ref Volume<Line3f> value)
        {
            Line3f[] data = null; CodeLine3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Line3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Plane2f_(ref Volume<Plane2f> value)
        {
            Plane2f[] data = null; CodePlane2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Plane2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Plane3f_(ref Volume<Plane3f> value)
        {
            Plane3f[] data = null; CodePlane3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Plane3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_PlaneWithPoint3f_(ref Volume<PlaneWithPoint3f> value)
        {
            PlaneWithPoint3f[] data = null; CodePlaneWithPoint3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<PlaneWithPoint3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Quad2f_(ref Volume<Quad2f> value)
        {
            Quad2f[] data = null; CodeQuad2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Quad2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Quad3f_(ref Volume<Quad3f> value)
        {
            Quad3f[] data = null; CodeQuad3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Quad3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Ray2f_(ref Volume<Ray2f> value)
        {
            Ray2f[] data = null; CodeRay2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Ray2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Ray3f_(ref Volume<Ray3f> value)
        {
            Ray3f[] data = null; CodeRay3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Ray3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Sphere3f_(ref Volume<Sphere3f> value)
        {
            Sphere3f[] data = null; CodeSphere3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Sphere3f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Triangle2f_(ref Volume<Triangle2f> value)
        {
            Triangle2f[] data = null; CodeTriangle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Triangle2f>(data, origin, size, delta);
        }

        public void CodeVolume_of_Triangle3f_(ref Volume<Triangle3f> value)
        {
            Triangle3f[] data = null; CodeTriangle3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            V3l size = default(V3l); CodeV3l(ref size);
            V3l delta = default(V3l); CodeV3l(ref delta);
            value = new Volume<Triangle3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Byte_(ref Tensor<byte> value)
        {
            byte[] data = null; CodeByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<byte>(data, origin, size, delta);
        }

        public void CodeTensor_of_SByte_(ref Tensor<sbyte> value)
        {
            sbyte[] data = null; CodeSByteArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<sbyte>(data, origin, size, delta);
        }

        public void CodeTensor_of_Short_(ref Tensor<short> value)
        {
            short[] data = null; CodeShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<short>(data, origin, size, delta);
        }

        public void CodeTensor_of_UShort_(ref Tensor<ushort> value)
        {
            ushort[] data = null; CodeUShortArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<ushort>(data, origin, size, delta);
        }

        public void CodeTensor_of_Int_(ref Tensor<int> value)
        {
            int[] data = null; CodeIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<int>(data, origin, size, delta);
        }

        public void CodeTensor_of_UInt_(ref Tensor<uint> value)
        {
            uint[] data = null; CodeUIntArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<uint>(data, origin, size, delta);
        }

        public void CodeTensor_of_Long_(ref Tensor<long> value)
        {
            long[] data = null; CodeLongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<long>(data, origin, size, delta);
        }

        public void CodeTensor_of_ULong_(ref Tensor<ulong> value)
        {
            ulong[] data = null; CodeULongArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<ulong>(data, origin, size, delta);
        }

        public void CodeTensor_of_Float_(ref Tensor<float> value)
        {
            float[] data = null; CodeFloatArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<float>(data, origin, size, delta);
        }

        public void CodeTensor_of_Double_(ref Tensor<double> value)
        {
            double[] data = null; CodeDoubleArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<double>(data, origin, size, delta);
        }

        public void CodeTensor_of_Fraction_(ref Tensor<Fraction> value)
        {
            Fraction[] data = null; CodeFractionArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Fraction>(data, origin, size, delta);
        }

        public void CodeTensor_of_V2i_(ref Tensor<V2i> value)
        {
            V2i[] data = null; CodeV2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V2i>(data, origin, size, delta);
        }

        public void CodeTensor_of_V2l_(ref Tensor<V2l> value)
        {
            V2l[] data = null; CodeV2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V2l>(data, origin, size, delta);
        }

        public void CodeTensor_of_V2f_(ref Tensor<V2f> value)
        {
            V2f[] data = null; CodeV2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_V2d_(ref Tensor<V2d> value)
        {
            V2d[] data = null; CodeV2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_V3i_(ref Tensor<V3i> value)
        {
            V3i[] data = null; CodeV3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V3i>(data, origin, size, delta);
        }

        public void CodeTensor_of_V3l_(ref Tensor<V3l> value)
        {
            V3l[] data = null; CodeV3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V3l>(data, origin, size, delta);
        }

        public void CodeTensor_of_V3f_(ref Tensor<V3f> value)
        {
            V3f[] data = null; CodeV3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_V3d_(ref Tensor<V3d> value)
        {
            V3d[] data = null; CodeV3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_V4i_(ref Tensor<V4i> value)
        {
            V4i[] data = null; CodeV4iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V4i>(data, origin, size, delta);
        }

        public void CodeTensor_of_V4l_(ref Tensor<V4l> value)
        {
            V4l[] data = null; CodeV4lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V4l>(data, origin, size, delta);
        }

        public void CodeTensor_of_V4f_(ref Tensor<V4f> value)
        {
            V4f[] data = null; CodeV4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V4f>(data, origin, size, delta);
        }

        public void CodeTensor_of_V4d_(ref Tensor<V4d> value)
        {
            V4d[] data = null; CodeV4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<V4d>(data, origin, size, delta);
        }

        public void CodeTensor_of_M22i_(ref Tensor<M22i> value)
        {
            M22i[] data = null; CodeM22iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M22i>(data, origin, size, delta);
        }

        public void CodeTensor_of_M22l_(ref Tensor<M22l> value)
        {
            M22l[] data = null; CodeM22lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M22l>(data, origin, size, delta);
        }

        public void CodeTensor_of_M22f_(ref Tensor<M22f> value)
        {
            M22f[] data = null; CodeM22fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M22f>(data, origin, size, delta);
        }

        public void CodeTensor_of_M22d_(ref Tensor<M22d> value)
        {
            M22d[] data = null; CodeM22dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M22d>(data, origin, size, delta);
        }

        public void CodeTensor_of_M23i_(ref Tensor<M23i> value)
        {
            M23i[] data = null; CodeM23iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M23i>(data, origin, size, delta);
        }

        public void CodeTensor_of_M23l_(ref Tensor<M23l> value)
        {
            M23l[] data = null; CodeM23lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M23l>(data, origin, size, delta);
        }

        public void CodeTensor_of_M23f_(ref Tensor<M23f> value)
        {
            M23f[] data = null; CodeM23fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M23f>(data, origin, size, delta);
        }

        public void CodeTensor_of_M23d_(ref Tensor<M23d> value)
        {
            M23d[] data = null; CodeM23dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M23d>(data, origin, size, delta);
        }

        public void CodeTensor_of_M33i_(ref Tensor<M33i> value)
        {
            M33i[] data = null; CodeM33iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M33i>(data, origin, size, delta);
        }

        public void CodeTensor_of_M33l_(ref Tensor<M33l> value)
        {
            M33l[] data = null; CodeM33lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M33l>(data, origin, size, delta);
        }

        public void CodeTensor_of_M33f_(ref Tensor<M33f> value)
        {
            M33f[] data = null; CodeM33fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M33f>(data, origin, size, delta);
        }

        public void CodeTensor_of_M33d_(ref Tensor<M33d> value)
        {
            M33d[] data = null; CodeM33dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M33d>(data, origin, size, delta);
        }

        public void CodeTensor_of_M34i_(ref Tensor<M34i> value)
        {
            M34i[] data = null; CodeM34iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M34i>(data, origin, size, delta);
        }

        public void CodeTensor_of_M34l_(ref Tensor<M34l> value)
        {
            M34l[] data = null; CodeM34lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M34l>(data, origin, size, delta);
        }

        public void CodeTensor_of_M34f_(ref Tensor<M34f> value)
        {
            M34f[] data = null; CodeM34fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M34f>(data, origin, size, delta);
        }

        public void CodeTensor_of_M34d_(ref Tensor<M34d> value)
        {
            M34d[] data = null; CodeM34dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M34d>(data, origin, size, delta);
        }

        public void CodeTensor_of_M44i_(ref Tensor<M44i> value)
        {
            M44i[] data = null; CodeM44iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M44i>(data, origin, size, delta);
        }

        public void CodeTensor_of_M44l_(ref Tensor<M44l> value)
        {
            M44l[] data = null; CodeM44lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M44l>(data, origin, size, delta);
        }

        public void CodeTensor_of_M44f_(ref Tensor<M44f> value)
        {
            M44f[] data = null; CodeM44fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M44f>(data, origin, size, delta);
        }

        public void CodeTensor_of_M44d_(ref Tensor<M44d> value)
        {
            M44d[] data = null; CodeM44dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<M44d>(data, origin, size, delta);
        }

        public void CodeTensor_of_C3b_(ref Tensor<C3b> value)
        {
            C3b[] data = null; CodeC3bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C3b>(data, origin, size, delta);
        }

        public void CodeTensor_of_C3us_(ref Tensor<C3us> value)
        {
            C3us[] data = null; CodeC3usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C3us>(data, origin, size, delta);
        }

        public void CodeTensor_of_C3ui_(ref Tensor<C3ui> value)
        {
            C3ui[] data = null; CodeC3uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C3ui>(data, origin, size, delta);
        }

        public void CodeTensor_of_C3f_(ref Tensor<C3f> value)
        {
            C3f[] data = null; CodeC3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_C3d_(ref Tensor<C3d> value)
        {
            C3d[] data = null; CodeC3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_C4b_(ref Tensor<C4b> value)
        {
            C4b[] data = null; CodeC4bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C4b>(data, origin, size, delta);
        }

        public void CodeTensor_of_C4us_(ref Tensor<C4us> value)
        {
            C4us[] data = null; CodeC4usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C4us>(data, origin, size, delta);
        }

        public void CodeTensor_of_C4ui_(ref Tensor<C4ui> value)
        {
            C4ui[] data = null; CodeC4uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C4ui>(data, origin, size, delta);
        }

        public void CodeTensor_of_C4f_(ref Tensor<C4f> value)
        {
            C4f[] data = null; CodeC4fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C4f>(data, origin, size, delta);
        }

        public void CodeTensor_of_C4d_(ref Tensor<C4d> value)
        {
            C4d[] data = null; CodeC4dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<C4d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1b_(ref Tensor<Range1b> value)
        {
            Range1b[] data = null; CodeRange1bArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1b>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1sb_(ref Tensor<Range1sb> value)
        {
            Range1sb[] data = null; CodeRange1sbArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1sb>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1s_(ref Tensor<Range1s> value)
        {
            Range1s[] data = null; CodeRange1sArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1s>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1us_(ref Tensor<Range1us> value)
        {
            Range1us[] data = null; CodeRange1usArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1us>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1i_(ref Tensor<Range1i> value)
        {
            Range1i[] data = null; CodeRange1iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1i>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1ui_(ref Tensor<Range1ui> value)
        {
            Range1ui[] data = null; CodeRange1uiArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1ui>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1l_(ref Tensor<Range1l> value)
        {
            Range1l[] data = null; CodeRange1lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1l>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1ul_(ref Tensor<Range1ul> value)
        {
            Range1ul[] data = null; CodeRange1ulArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1ul>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1f_(ref Tensor<Range1f> value)
        {
            Range1f[] data = null; CodeRange1fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Range1d_(ref Tensor<Range1d> value)
        {
            Range1d[] data = null; CodeRange1dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Range1d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box2i_(ref Tensor<Box2i> value)
        {
            Box2i[] data = null; CodeBox2iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box2i>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box2l_(ref Tensor<Box2l> value)
        {
            Box2l[] data = null; CodeBox2lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box2l>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box2f_(ref Tensor<Box2f> value)
        {
            Box2f[] data = null; CodeBox2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box2d_(ref Tensor<Box2d> value)
        {
            Box2d[] data = null; CodeBox2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box3i_(ref Tensor<Box3i> value)
        {
            Box3i[] data = null; CodeBox3iArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box3i>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box3l_(ref Tensor<Box3l> value)
        {
            Box3l[] data = null; CodeBox3lArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box3l>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box3f_(ref Tensor<Box3f> value)
        {
            Box3f[] data = null; CodeBox3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Box3d_(ref Tensor<Box3d> value)
        {
            Box3d[] data = null; CodeBox3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Box3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Euclidean3f_(ref Tensor<Euclidean3f> value)
        {
            Euclidean3f[] data = null; CodeEuclidean3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Euclidean3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Euclidean3d_(ref Tensor<Euclidean3d> value)
        {
            Euclidean3d[] data = null; CodeEuclidean3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Euclidean3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Rot2f_(ref Tensor<Rot2f> value)
        {
            Rot2f[] data = null; CodeRot2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Rot2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Rot2d_(ref Tensor<Rot2d> value)
        {
            Rot2d[] data = null; CodeRot2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Rot2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Rot3f_(ref Tensor<Rot3f> value)
        {
            Rot3f[] data = null; CodeRot3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Rot3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Rot3d_(ref Tensor<Rot3d> value)
        {
            Rot3d[] data = null; CodeRot3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Rot3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Scale3f_(ref Tensor<Scale3f> value)
        {
            Scale3f[] data = null; CodeScale3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Scale3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Scale3d_(ref Tensor<Scale3d> value)
        {
            Scale3d[] data = null; CodeScale3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Scale3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Shift3f_(ref Tensor<Shift3f> value)
        {
            Shift3f[] data = null; CodeShift3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Shift3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Shift3d_(ref Tensor<Shift3d> value)
        {
            Shift3d[] data = null; CodeShift3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Shift3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Trafo2f_(ref Tensor<Trafo2f> value)
        {
            Trafo2f[] data = null; CodeTrafo2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Trafo2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Trafo2d_(ref Tensor<Trafo2d> value)
        {
            Trafo2d[] data = null; CodeTrafo2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Trafo2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Trafo3f_(ref Tensor<Trafo3f> value)
        {
            Trafo3f[] data = null; CodeTrafo3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Trafo3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Trafo3d_(ref Tensor<Trafo3d> value)
        {
            Trafo3d[] data = null; CodeTrafo3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Trafo3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Bool_(ref Tensor<bool> value)
        {
            bool[] data = null; CodeBoolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<bool>(data, origin, size, delta);
        }

        public void CodeTensor_of_Char_(ref Tensor<char> value)
        {
            char[] data = null; CodeCharArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<char>(data, origin, size, delta);
        }

        public void CodeTensor_of_String_(ref Tensor<string> value)
        {
            string[] data = null; CodeStringArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<string>(data, origin, size, delta);
        }

        public void CodeTensor_of_Type_(ref Tensor<Type> value)
        {
            Type[] data = null; CodeTypeArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Type>(data, origin, size, delta);
        }

        public void CodeTensor_of_Guid_(ref Tensor<Guid> value)
        {
            Guid[] data = null; CodeGuidArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Guid>(data, origin, size, delta);
        }

        public void CodeTensor_of_Symbol_(ref Tensor<Symbol> value)
        {
            Symbol[] data = null; CodeSymbolArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Symbol>(data, origin, size, delta);
        }

        public void CodeTensor_of_Circle2d_(ref Tensor<Circle2d> value)
        {
            Circle2d[] data = null; CodeCircle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Circle2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Line2d_(ref Tensor<Line2d> value)
        {
            Line2d[] data = null; CodeLine2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Line2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Line3d_(ref Tensor<Line3d> value)
        {
            Line3d[] data = null; CodeLine3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Line3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Plane2d_(ref Tensor<Plane2d> value)
        {
            Plane2d[] data = null; CodePlane2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Plane2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Plane3d_(ref Tensor<Plane3d> value)
        {
            Plane3d[] data = null; CodePlane3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Plane3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_PlaneWithPoint3d_(ref Tensor<PlaneWithPoint3d> value)
        {
            PlaneWithPoint3d[] data = null; CodePlaneWithPoint3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<PlaneWithPoint3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Quad2d_(ref Tensor<Quad2d> value)
        {
            Quad2d[] data = null; CodeQuad2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Quad2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Quad3d_(ref Tensor<Quad3d> value)
        {
            Quad3d[] data = null; CodeQuad3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Quad3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Ray2d_(ref Tensor<Ray2d> value)
        {
            Ray2d[] data = null; CodeRay2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Ray2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Ray3d_(ref Tensor<Ray3d> value)
        {
            Ray3d[] data = null; CodeRay3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Ray3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Sphere3d_(ref Tensor<Sphere3d> value)
        {
            Sphere3d[] data = null; CodeSphere3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Sphere3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Triangle2d_(ref Tensor<Triangle2d> value)
        {
            Triangle2d[] data = null; CodeTriangle2dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Triangle2d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Triangle3d_(ref Tensor<Triangle3d> value)
        {
            Triangle3d[] data = null; CodeTriangle3dArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Triangle3d>(data, origin, size, delta);
        }

        public void CodeTensor_of_Circle2f_(ref Tensor<Circle2f> value)
        {
            Circle2f[] data = null; CodeCircle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Circle2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Line2f_(ref Tensor<Line2f> value)
        {
            Line2f[] data = null; CodeLine2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Line2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Line3f_(ref Tensor<Line3f> value)
        {
            Line3f[] data = null; CodeLine3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Line3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Plane2f_(ref Tensor<Plane2f> value)
        {
            Plane2f[] data = null; CodePlane2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Plane2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Plane3f_(ref Tensor<Plane3f> value)
        {
            Plane3f[] data = null; CodePlane3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Plane3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_PlaneWithPoint3f_(ref Tensor<PlaneWithPoint3f> value)
        {
            PlaneWithPoint3f[] data = null; CodePlaneWithPoint3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<PlaneWithPoint3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Quad2f_(ref Tensor<Quad2f> value)
        {
            Quad2f[] data = null; CodeQuad2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Quad2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Quad3f_(ref Tensor<Quad3f> value)
        {
            Quad3f[] data = null; CodeQuad3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Quad3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Ray2f_(ref Tensor<Ray2f> value)
        {
            Ray2f[] data = null; CodeRay2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Ray2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Ray3f_(ref Tensor<Ray3f> value)
        {
            Ray3f[] data = null; CodeRay3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Ray3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Sphere3f_(ref Tensor<Sphere3f> value)
        {
            Sphere3f[] data = null; CodeSphere3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Sphere3f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Triangle2f_(ref Tensor<Triangle2f> value)
        {
            Triangle2f[] data = null; CodeTriangle2fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Triangle2f>(data, origin, size, delta);
        }

        public void CodeTensor_of_Triangle3f_(ref Tensor<Triangle3f> value)
        {
            Triangle3f[] data = null; CodeTriangle3fArray(ref data);
            long origin = 0L; CodeLong(ref origin);
            long[] size = default(long[]); CodeLongArray(ref size);
            long[] delta = default(long[]); CodeLongArray(ref delta);
            value = new Tensor<Triangle3f>(data, origin, size, delta);
        }

        #endregion

        #region Arrays

        public void CodeV2iArray(ref V2i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV2uiArray(ref V2ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV2lArray(ref V2l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV2fArray(ref V2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV2dArray(ref V2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV3iArray(ref V3i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV3uiArray(ref V3ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV3lArray(ref V3l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV3fArray(ref V3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV3dArray(ref V3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV4iArray(ref V4i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV4uiArray(ref V4ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV4lArray(ref V4l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV4fArray(ref V4f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeV4dArray(ref V4d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM22iArray(ref M22i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM22lArray(ref M22l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM22fArray(ref M22f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM22dArray(ref M22d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM23iArray(ref M23i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM23lArray(ref M23l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM23fArray(ref M23f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM23dArray(ref M23d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM33iArray(ref M33i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM33lArray(ref M33l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM33fArray(ref M33f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM33dArray(ref M33d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM34iArray(ref M34i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM34lArray(ref M34l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM34fArray(ref M34f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM34dArray(ref M34d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM44iArray(ref M44i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM44lArray(ref M44l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM44fArray(ref M44f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeM44dArray(ref M44d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1bArray(ref Range1b[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1sbArray(ref Range1sb[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1sArray(ref Range1s[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1usArray(ref Range1us[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1iArray(ref Range1i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1uiArray(ref Range1ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1lArray(ref Range1l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1ulArray(ref Range1ul[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1fArray(ref Range1f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeRange1dArray(ref Range1d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox2iArray(ref Box2i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox2lArray(ref Box2l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox2fArray(ref Box2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox2dArray(ref Box2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox3iArray(ref Box3i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox3lArray(ref Box3l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox3fArray(ref Box3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeBox3dArray(ref Box3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC3bArray(ref C3b[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC3usArray(ref C3us[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC3uiArray(ref C3ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC3fArray(ref C3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC3dArray(ref C3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC4bArray(ref C4b[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC4usArray(ref C4us[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC4uiArray(ref C4ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC4fArray(ref C4f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeC4dArray(ref C4d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_reader.ReadArray(value, 0, count);
        }

        public void CodeEuclidean3fArray(ref Euclidean3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeEuclidean3f(ref value[i]);
        }

        public void CodeEuclidean3dArray(ref Euclidean3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeEuclidean3d(ref value[i]);
        }

        public void CodeRot2fArray(ref Rot2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot2f(ref value[i]);
        }

        public void CodeRot2dArray(ref Rot2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot2d(ref value[i]);
        }

        public void CodeRot3fArray(ref Rot3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot3f(ref value[i]);
        }

        public void CodeRot3dArray(ref Rot3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot3d(ref value[i]);
        }

        public void CodeScale3fArray(ref Scale3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeScale3f(ref value[i]);
        }

        public void CodeScale3dArray(ref Scale3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeScale3d(ref value[i]);
        }

        public void CodeShift3fArray(ref Shift3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShift3f(ref value[i]);
        }

        public void CodeShift3dArray(ref Shift3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShift3d(ref value[i]);
        }

        public void CodeTrafo2fArray(ref Trafo2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo2f(ref value[i]);
        }

        public void CodeTrafo2dArray(ref Trafo2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo2d(ref value[i]);
        }

        public void CodeTrafo3fArray(ref Trafo3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo3f(ref value[i]);
        }

        public void CodeTrafo3dArray(ref Trafo3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo3d(ref value[i]);
        }

        public void CodeCircle2dArray(ref Circle2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeCircle2d(ref value[i]);
        }

        public void CodeLine2dArray(ref Line2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeLine2d(ref value[i]);
        }

        public void CodeLine3dArray(ref Line3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeLine3d(ref value[i]);
        }

        public void CodePlane2dArray(ref Plane2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodePlane2d(ref value[i]);
        }

        public void CodePlane3dArray(ref Plane3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodePlane3d(ref value[i]);
        }

        public void CodePlaneWithPoint3dArray(ref PlaneWithPoint3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodePlaneWithPoint3d(ref value[i]);
        }

        public void CodeQuad2dArray(ref Quad2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeQuad2d(ref value[i]);
        }

        public void CodeQuad3dArray(ref Quad3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeQuad3d(ref value[i]);
        }

        public void CodeRay2dArray(ref Ray2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRay2d(ref value[i]);
        }

        public void CodeRay3dArray(ref Ray3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRay3d(ref value[i]);
        }

        public void CodeSphere3dArray(ref Sphere3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeSphere3d(ref value[i]);
        }

        public void CodeTriangle2dArray(ref Triangle2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTriangle2d(ref value[i]);
        }

        public void CodeTriangle3dArray(ref Triangle3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTriangle3d(ref value[i]);
        }

        public void CodeCircle2fArray(ref Circle2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeCircle2f(ref value[i]);
        }

        public void CodeLine2fArray(ref Line2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeLine2f(ref value[i]);
        }

        public void CodeLine3fArray(ref Line3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeLine3f(ref value[i]);
        }

        public void CodePlane2fArray(ref Plane2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodePlane2f(ref value[i]);
        }

        public void CodePlane3fArray(ref Plane3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodePlane3f(ref value[i]);
        }

        public void CodePlaneWithPoint3fArray(ref PlaneWithPoint3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodePlaneWithPoint3f(ref value[i]);
        }

        public void CodeQuad2fArray(ref Quad2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeQuad2f(ref value[i]);
        }

        public void CodeQuad3fArray(ref Quad3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeQuad3f(ref value[i]);
        }

        public void CodeRay2fArray(ref Ray2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRay2f(ref value[i]);
        }

        public void CodeRay3fArray(ref Ray3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRay3f(ref value[i]);
        }

        public void CodeSphere3fArray(ref Sphere3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeSphere3f(ref value[i]);
        }

        public void CodeTriangle2fArray(ref Triangle2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTriangle2f(ref value[i]);
        }

        public void CodeTriangle3fArray(ref Triangle3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTriangle3f(ref value[i]);
        }

        public void CodeVector_of_Byte_Array(ref Vector<byte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Byte_(ref value[i]);
        }

        public void CodeVector_of_SByte_Array(ref Vector<sbyte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_SByte_(ref value[i]);
        }

        public void CodeVector_of_Short_Array(ref Vector<short>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Short_(ref value[i]);
        }

        public void CodeVector_of_UShort_Array(ref Vector<ushort>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_UShort_(ref value[i]);
        }

        public void CodeVector_of_Int_Array(ref Vector<int>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Int_(ref value[i]);
        }

        public void CodeVector_of_UInt_Array(ref Vector<uint>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_UInt_(ref value[i]);
        }

        public void CodeVector_of_Long_Array(ref Vector<long>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Long_(ref value[i]);
        }

        public void CodeVector_of_ULong_Array(ref Vector<ulong>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_ULong_(ref value[i]);
        }

        public void CodeVector_of_Float_Array(ref Vector<float>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Float_(ref value[i]);
        }

        public void CodeVector_of_Double_Array(ref Vector<double>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Double_(ref value[i]);
        }

        public void CodeVector_of_Fraction_Array(ref Vector<Fraction>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Fraction_(ref value[i]);
        }

        public void CodeVector_of_V2i_Array(ref Vector<V2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V2i_(ref value[i]);
        }

        public void CodeVector_of_V2l_Array(ref Vector<V2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V2l_(ref value[i]);
        }

        public void CodeVector_of_V2f_Array(ref Vector<V2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V2f_(ref value[i]);
        }

        public void CodeVector_of_V2d_Array(ref Vector<V2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V2d_(ref value[i]);
        }

        public void CodeVector_of_V3i_Array(ref Vector<V3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V3i_(ref value[i]);
        }

        public void CodeVector_of_V3l_Array(ref Vector<V3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V3l_(ref value[i]);
        }

        public void CodeVector_of_V3f_Array(ref Vector<V3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V3f_(ref value[i]);
        }

        public void CodeVector_of_V3d_Array(ref Vector<V3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V3d_(ref value[i]);
        }

        public void CodeVector_of_V4i_Array(ref Vector<V4i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V4i_(ref value[i]);
        }

        public void CodeVector_of_V4l_Array(ref Vector<V4l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V4l_(ref value[i]);
        }

        public void CodeVector_of_V4f_Array(ref Vector<V4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V4f_(ref value[i]);
        }

        public void CodeVector_of_V4d_Array(ref Vector<V4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_V4d_(ref value[i]);
        }

        public void CodeVector_of_M22i_Array(ref Vector<M22i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M22i_(ref value[i]);
        }

        public void CodeVector_of_M22l_Array(ref Vector<M22l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M22l_(ref value[i]);
        }

        public void CodeVector_of_M22f_Array(ref Vector<M22f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M22f_(ref value[i]);
        }

        public void CodeVector_of_M22d_Array(ref Vector<M22d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M22d_(ref value[i]);
        }

        public void CodeVector_of_M23i_Array(ref Vector<M23i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M23i_(ref value[i]);
        }

        public void CodeVector_of_M23l_Array(ref Vector<M23l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M23l_(ref value[i]);
        }

        public void CodeVector_of_M23f_Array(ref Vector<M23f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M23f_(ref value[i]);
        }

        public void CodeVector_of_M23d_Array(ref Vector<M23d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M23d_(ref value[i]);
        }

        public void CodeVector_of_M33i_Array(ref Vector<M33i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M33i_(ref value[i]);
        }

        public void CodeVector_of_M33l_Array(ref Vector<M33l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M33l_(ref value[i]);
        }

        public void CodeVector_of_M33f_Array(ref Vector<M33f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M33f_(ref value[i]);
        }

        public void CodeVector_of_M33d_Array(ref Vector<M33d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M33d_(ref value[i]);
        }

        public void CodeVector_of_M34i_Array(ref Vector<M34i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M34i_(ref value[i]);
        }

        public void CodeVector_of_M34l_Array(ref Vector<M34l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M34l_(ref value[i]);
        }

        public void CodeVector_of_M34f_Array(ref Vector<M34f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M34f_(ref value[i]);
        }

        public void CodeVector_of_M34d_Array(ref Vector<M34d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M34d_(ref value[i]);
        }

        public void CodeVector_of_M44i_Array(ref Vector<M44i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M44i_(ref value[i]);
        }

        public void CodeVector_of_M44l_Array(ref Vector<M44l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M44l_(ref value[i]);
        }

        public void CodeVector_of_M44f_Array(ref Vector<M44f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M44f_(ref value[i]);
        }

        public void CodeVector_of_M44d_Array(ref Vector<M44d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_M44d_(ref value[i]);
        }

        public void CodeVector_of_C3b_Array(ref Vector<C3b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C3b_(ref value[i]);
        }

        public void CodeVector_of_C3us_Array(ref Vector<C3us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C3us_(ref value[i]);
        }

        public void CodeVector_of_C3ui_Array(ref Vector<C3ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C3ui_(ref value[i]);
        }

        public void CodeVector_of_C3f_Array(ref Vector<C3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C3f_(ref value[i]);
        }

        public void CodeVector_of_C3d_Array(ref Vector<C3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C3d_(ref value[i]);
        }

        public void CodeVector_of_C4b_Array(ref Vector<C4b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C4b_(ref value[i]);
        }

        public void CodeVector_of_C4us_Array(ref Vector<C4us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C4us_(ref value[i]);
        }

        public void CodeVector_of_C4ui_Array(ref Vector<C4ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C4ui_(ref value[i]);
        }

        public void CodeVector_of_C4f_Array(ref Vector<C4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C4f_(ref value[i]);
        }

        public void CodeVector_of_C4d_Array(ref Vector<C4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_C4d_(ref value[i]);
        }

        public void CodeVector_of_Range1b_Array(ref Vector<Range1b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1b_(ref value[i]);
        }

        public void CodeVector_of_Range1sb_Array(ref Vector<Range1sb>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1sb_(ref value[i]);
        }

        public void CodeVector_of_Range1s_Array(ref Vector<Range1s>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1s_(ref value[i]);
        }

        public void CodeVector_of_Range1us_Array(ref Vector<Range1us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1us_(ref value[i]);
        }

        public void CodeVector_of_Range1i_Array(ref Vector<Range1i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1i_(ref value[i]);
        }

        public void CodeVector_of_Range1ui_Array(ref Vector<Range1ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1ui_(ref value[i]);
        }

        public void CodeVector_of_Range1l_Array(ref Vector<Range1l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1l_(ref value[i]);
        }

        public void CodeVector_of_Range1ul_Array(ref Vector<Range1ul>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1ul_(ref value[i]);
        }

        public void CodeVector_of_Range1f_Array(ref Vector<Range1f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1f_(ref value[i]);
        }

        public void CodeVector_of_Range1d_Array(ref Vector<Range1d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Range1d_(ref value[i]);
        }

        public void CodeVector_of_Box2i_Array(ref Vector<Box2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box2i_(ref value[i]);
        }

        public void CodeVector_of_Box2l_Array(ref Vector<Box2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box2l_(ref value[i]);
        }

        public void CodeVector_of_Box2f_Array(ref Vector<Box2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box2f_(ref value[i]);
        }

        public void CodeVector_of_Box2d_Array(ref Vector<Box2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box2d_(ref value[i]);
        }

        public void CodeVector_of_Box3i_Array(ref Vector<Box3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box3i_(ref value[i]);
        }

        public void CodeVector_of_Box3l_Array(ref Vector<Box3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box3l_(ref value[i]);
        }

        public void CodeVector_of_Box3f_Array(ref Vector<Box3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box3f_(ref value[i]);
        }

        public void CodeVector_of_Box3d_Array(ref Vector<Box3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Box3d_(ref value[i]);
        }

        public void CodeVector_of_Euclidean3f_Array(ref Vector<Euclidean3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Euclidean3f_(ref value[i]);
        }

        public void CodeVector_of_Euclidean3d_Array(ref Vector<Euclidean3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Euclidean3d_(ref value[i]);
        }

        public void CodeVector_of_Rot2f_Array(ref Vector<Rot2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Rot2f_(ref value[i]);
        }

        public void CodeVector_of_Rot2d_Array(ref Vector<Rot2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Rot2d_(ref value[i]);
        }

        public void CodeVector_of_Rot3f_Array(ref Vector<Rot3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Rot3f_(ref value[i]);
        }

        public void CodeVector_of_Rot3d_Array(ref Vector<Rot3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Rot3d_(ref value[i]);
        }

        public void CodeVector_of_Scale3f_Array(ref Vector<Scale3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Scale3f_(ref value[i]);
        }

        public void CodeVector_of_Scale3d_Array(ref Vector<Scale3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Scale3d_(ref value[i]);
        }

        public void CodeVector_of_Shift3f_Array(ref Vector<Shift3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Shift3f_(ref value[i]);
        }

        public void CodeVector_of_Shift3d_Array(ref Vector<Shift3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Shift3d_(ref value[i]);
        }

        public void CodeVector_of_Trafo2f_Array(ref Vector<Trafo2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Trafo2f_(ref value[i]);
        }

        public void CodeVector_of_Trafo2d_Array(ref Vector<Trafo2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Trafo2d_(ref value[i]);
        }

        public void CodeVector_of_Trafo3f_Array(ref Vector<Trafo3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Trafo3f_(ref value[i]);
        }

        public void CodeVector_of_Trafo3d_Array(ref Vector<Trafo3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Trafo3d_(ref value[i]);
        }

        public void CodeVector_of_Bool_Array(ref Vector<bool>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Bool_(ref value[i]);
        }

        public void CodeVector_of_Char_Array(ref Vector<char>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Char_(ref value[i]);
        }

        public void CodeVector_of_String_Array(ref Vector<string>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_String_(ref value[i]);
        }

        public void CodeVector_of_Type_Array(ref Vector<Type>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Type_(ref value[i]);
        }

        public void CodeVector_of_Guid_Array(ref Vector<Guid>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Guid_(ref value[i]);
        }

        public void CodeVector_of_Symbol_Array(ref Vector<Symbol>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Symbol_(ref value[i]);
        }

        public void CodeVector_of_Circle2d_Array(ref Vector<Circle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Circle2d_(ref value[i]);
        }

        public void CodeVector_of_Line2d_Array(ref Vector<Line2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Line2d_(ref value[i]);
        }

        public void CodeVector_of_Line3d_Array(ref Vector<Line3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Line3d_(ref value[i]);
        }

        public void CodeVector_of_Plane2d_Array(ref Vector<Plane2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Plane2d_(ref value[i]);
        }

        public void CodeVector_of_Plane3d_Array(ref Vector<Plane3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Plane3d_(ref value[i]);
        }

        public void CodeVector_of_PlaneWithPoint3d_Array(ref Vector<PlaneWithPoint3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_PlaneWithPoint3d_(ref value[i]);
        }

        public void CodeVector_of_Quad2d_Array(ref Vector<Quad2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Quad2d_(ref value[i]);
        }

        public void CodeVector_of_Quad3d_Array(ref Vector<Quad3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Quad3d_(ref value[i]);
        }

        public void CodeVector_of_Ray2d_Array(ref Vector<Ray2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Ray2d_(ref value[i]);
        }

        public void CodeVector_of_Ray3d_Array(ref Vector<Ray3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Ray3d_(ref value[i]);
        }

        public void CodeVector_of_Sphere3d_Array(ref Vector<Sphere3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Sphere3d_(ref value[i]);
        }

        public void CodeVector_of_Triangle2d_Array(ref Vector<Triangle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Triangle2d_(ref value[i]);
        }

        public void CodeVector_of_Triangle3d_Array(ref Vector<Triangle3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Triangle3d_(ref value[i]);
        }

        public void CodeVector_of_Circle2f_Array(ref Vector<Circle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Circle2f_(ref value[i]);
        }

        public void CodeVector_of_Line2f_Array(ref Vector<Line2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Line2f_(ref value[i]);
        }

        public void CodeVector_of_Line3f_Array(ref Vector<Line3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Line3f_(ref value[i]);
        }

        public void CodeVector_of_Plane2f_Array(ref Vector<Plane2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Plane2f_(ref value[i]);
        }

        public void CodeVector_of_Plane3f_Array(ref Vector<Plane3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Plane3f_(ref value[i]);
        }

        public void CodeVector_of_PlaneWithPoint3f_Array(ref Vector<PlaneWithPoint3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_PlaneWithPoint3f_(ref value[i]);
        }

        public void CodeVector_of_Quad2f_Array(ref Vector<Quad2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Quad2f_(ref value[i]);
        }

        public void CodeVector_of_Quad3f_Array(ref Vector<Quad3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Quad3f_(ref value[i]);
        }

        public void CodeVector_of_Ray2f_Array(ref Vector<Ray2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Ray2f_(ref value[i]);
        }

        public void CodeVector_of_Ray3f_Array(ref Vector<Ray3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Ray3f_(ref value[i]);
        }

        public void CodeVector_of_Sphere3f_Array(ref Vector<Sphere3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Sphere3f_(ref value[i]);
        }

        public void CodeVector_of_Triangle2f_Array(ref Vector<Triangle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Triangle2f_(ref value[i]);
        }

        public void CodeVector_of_Triangle3f_Array(ref Vector<Triangle3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVector_of_Triangle3f_(ref value[i]);
        }

        public void CodeMatrix_of_Byte_Array(ref Matrix<byte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Byte_(ref value[i]);
        }

        public void CodeMatrix_of_SByte_Array(ref Matrix<sbyte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_SByte_(ref value[i]);
        }

        public void CodeMatrix_of_Short_Array(ref Matrix<short>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Short_(ref value[i]);
        }

        public void CodeMatrix_of_UShort_Array(ref Matrix<ushort>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_UShort_(ref value[i]);
        }

        public void CodeMatrix_of_Int_Array(ref Matrix<int>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Int_(ref value[i]);
        }

        public void CodeMatrix_of_UInt_Array(ref Matrix<uint>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_UInt_(ref value[i]);
        }

        public void CodeMatrix_of_Long_Array(ref Matrix<long>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Long_(ref value[i]);
        }

        public void CodeMatrix_of_ULong_Array(ref Matrix<ulong>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_ULong_(ref value[i]);
        }

        public void CodeMatrix_of_Float_Array(ref Matrix<float>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Float_(ref value[i]);
        }

        public void CodeMatrix_of_Double_Array(ref Matrix<double>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Double_(ref value[i]);
        }

        public void CodeMatrix_of_Fraction_Array(ref Matrix<Fraction>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Fraction_(ref value[i]);
        }

        public void CodeMatrix_of_V2i_Array(ref Matrix<V2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V2i_(ref value[i]);
        }

        public void CodeMatrix_of_V2l_Array(ref Matrix<V2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V2l_(ref value[i]);
        }

        public void CodeMatrix_of_V2f_Array(ref Matrix<V2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V2f_(ref value[i]);
        }

        public void CodeMatrix_of_V2d_Array(ref Matrix<V2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V2d_(ref value[i]);
        }

        public void CodeMatrix_of_V3i_Array(ref Matrix<V3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V3i_(ref value[i]);
        }

        public void CodeMatrix_of_V3l_Array(ref Matrix<V3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V3l_(ref value[i]);
        }

        public void CodeMatrix_of_V3f_Array(ref Matrix<V3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V3f_(ref value[i]);
        }

        public void CodeMatrix_of_V3d_Array(ref Matrix<V3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V3d_(ref value[i]);
        }

        public void CodeMatrix_of_V4i_Array(ref Matrix<V4i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V4i_(ref value[i]);
        }

        public void CodeMatrix_of_V4l_Array(ref Matrix<V4l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V4l_(ref value[i]);
        }

        public void CodeMatrix_of_V4f_Array(ref Matrix<V4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V4f_(ref value[i]);
        }

        public void CodeMatrix_of_V4d_Array(ref Matrix<V4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_V4d_(ref value[i]);
        }

        public void CodeMatrix_of_M22i_Array(ref Matrix<M22i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M22i_(ref value[i]);
        }

        public void CodeMatrix_of_M22l_Array(ref Matrix<M22l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M22l_(ref value[i]);
        }

        public void CodeMatrix_of_M22f_Array(ref Matrix<M22f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M22f_(ref value[i]);
        }

        public void CodeMatrix_of_M22d_Array(ref Matrix<M22d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M22d_(ref value[i]);
        }

        public void CodeMatrix_of_M23i_Array(ref Matrix<M23i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M23i_(ref value[i]);
        }

        public void CodeMatrix_of_M23l_Array(ref Matrix<M23l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M23l_(ref value[i]);
        }

        public void CodeMatrix_of_M23f_Array(ref Matrix<M23f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M23f_(ref value[i]);
        }

        public void CodeMatrix_of_M23d_Array(ref Matrix<M23d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M23d_(ref value[i]);
        }

        public void CodeMatrix_of_M33i_Array(ref Matrix<M33i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M33i_(ref value[i]);
        }

        public void CodeMatrix_of_M33l_Array(ref Matrix<M33l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M33l_(ref value[i]);
        }

        public void CodeMatrix_of_M33f_Array(ref Matrix<M33f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M33f_(ref value[i]);
        }

        public void CodeMatrix_of_M33d_Array(ref Matrix<M33d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M33d_(ref value[i]);
        }

        public void CodeMatrix_of_M34i_Array(ref Matrix<M34i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M34i_(ref value[i]);
        }

        public void CodeMatrix_of_M34l_Array(ref Matrix<M34l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M34l_(ref value[i]);
        }

        public void CodeMatrix_of_M34f_Array(ref Matrix<M34f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M34f_(ref value[i]);
        }

        public void CodeMatrix_of_M34d_Array(ref Matrix<M34d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M34d_(ref value[i]);
        }

        public void CodeMatrix_of_M44i_Array(ref Matrix<M44i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M44i_(ref value[i]);
        }

        public void CodeMatrix_of_M44l_Array(ref Matrix<M44l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M44l_(ref value[i]);
        }

        public void CodeMatrix_of_M44f_Array(ref Matrix<M44f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M44f_(ref value[i]);
        }

        public void CodeMatrix_of_M44d_Array(ref Matrix<M44d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_M44d_(ref value[i]);
        }

        public void CodeMatrix_of_C3b_Array(ref Matrix<C3b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C3b_(ref value[i]);
        }

        public void CodeMatrix_of_C3us_Array(ref Matrix<C3us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C3us_(ref value[i]);
        }

        public void CodeMatrix_of_C3ui_Array(ref Matrix<C3ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C3ui_(ref value[i]);
        }

        public void CodeMatrix_of_C3f_Array(ref Matrix<C3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C3f_(ref value[i]);
        }

        public void CodeMatrix_of_C3d_Array(ref Matrix<C3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C3d_(ref value[i]);
        }

        public void CodeMatrix_of_C4b_Array(ref Matrix<C4b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C4b_(ref value[i]);
        }

        public void CodeMatrix_of_C4us_Array(ref Matrix<C4us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C4us_(ref value[i]);
        }

        public void CodeMatrix_of_C4ui_Array(ref Matrix<C4ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C4ui_(ref value[i]);
        }

        public void CodeMatrix_of_C4f_Array(ref Matrix<C4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C4f_(ref value[i]);
        }

        public void CodeMatrix_of_C4d_Array(ref Matrix<C4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_C4d_(ref value[i]);
        }

        public void CodeMatrix_of_Range1b_Array(ref Matrix<Range1b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1b_(ref value[i]);
        }

        public void CodeMatrix_of_Range1sb_Array(ref Matrix<Range1sb>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1sb_(ref value[i]);
        }

        public void CodeMatrix_of_Range1s_Array(ref Matrix<Range1s>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1s_(ref value[i]);
        }

        public void CodeMatrix_of_Range1us_Array(ref Matrix<Range1us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1us_(ref value[i]);
        }

        public void CodeMatrix_of_Range1i_Array(ref Matrix<Range1i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1i_(ref value[i]);
        }

        public void CodeMatrix_of_Range1ui_Array(ref Matrix<Range1ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1ui_(ref value[i]);
        }

        public void CodeMatrix_of_Range1l_Array(ref Matrix<Range1l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1l_(ref value[i]);
        }

        public void CodeMatrix_of_Range1ul_Array(ref Matrix<Range1ul>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1ul_(ref value[i]);
        }

        public void CodeMatrix_of_Range1f_Array(ref Matrix<Range1f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1f_(ref value[i]);
        }

        public void CodeMatrix_of_Range1d_Array(ref Matrix<Range1d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Range1d_(ref value[i]);
        }

        public void CodeMatrix_of_Box2i_Array(ref Matrix<Box2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box2i_(ref value[i]);
        }

        public void CodeMatrix_of_Box2l_Array(ref Matrix<Box2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box2l_(ref value[i]);
        }

        public void CodeMatrix_of_Box2f_Array(ref Matrix<Box2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box2f_(ref value[i]);
        }

        public void CodeMatrix_of_Box2d_Array(ref Matrix<Box2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box2d_(ref value[i]);
        }

        public void CodeMatrix_of_Box3i_Array(ref Matrix<Box3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box3i_(ref value[i]);
        }

        public void CodeMatrix_of_Box3l_Array(ref Matrix<Box3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box3l_(ref value[i]);
        }

        public void CodeMatrix_of_Box3f_Array(ref Matrix<Box3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box3f_(ref value[i]);
        }

        public void CodeMatrix_of_Box3d_Array(ref Matrix<Box3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Box3d_(ref value[i]);
        }

        public void CodeMatrix_of_Euclidean3f_Array(ref Matrix<Euclidean3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Euclidean3f_(ref value[i]);
        }

        public void CodeMatrix_of_Euclidean3d_Array(ref Matrix<Euclidean3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Euclidean3d_(ref value[i]);
        }

        public void CodeMatrix_of_Rot2f_Array(ref Matrix<Rot2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Rot2f_(ref value[i]);
        }

        public void CodeMatrix_of_Rot2d_Array(ref Matrix<Rot2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Rot2d_(ref value[i]);
        }

        public void CodeMatrix_of_Rot3f_Array(ref Matrix<Rot3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Rot3f_(ref value[i]);
        }

        public void CodeMatrix_of_Rot3d_Array(ref Matrix<Rot3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Rot3d_(ref value[i]);
        }

        public void CodeMatrix_of_Scale3f_Array(ref Matrix<Scale3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Scale3f_(ref value[i]);
        }

        public void CodeMatrix_of_Scale3d_Array(ref Matrix<Scale3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Scale3d_(ref value[i]);
        }

        public void CodeMatrix_of_Shift3f_Array(ref Matrix<Shift3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Shift3f_(ref value[i]);
        }

        public void CodeMatrix_of_Shift3d_Array(ref Matrix<Shift3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Shift3d_(ref value[i]);
        }

        public void CodeMatrix_of_Trafo2f_Array(ref Matrix<Trafo2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Trafo2f_(ref value[i]);
        }

        public void CodeMatrix_of_Trafo2d_Array(ref Matrix<Trafo2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Trafo2d_(ref value[i]);
        }

        public void CodeMatrix_of_Trafo3f_Array(ref Matrix<Trafo3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Trafo3f_(ref value[i]);
        }

        public void CodeMatrix_of_Trafo3d_Array(ref Matrix<Trafo3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Trafo3d_(ref value[i]);
        }

        public void CodeMatrix_of_Bool_Array(ref Matrix<bool>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Bool_(ref value[i]);
        }

        public void CodeMatrix_of_Char_Array(ref Matrix<char>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Char_(ref value[i]);
        }

        public void CodeMatrix_of_String_Array(ref Matrix<string>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_String_(ref value[i]);
        }

        public void CodeMatrix_of_Type_Array(ref Matrix<Type>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Type_(ref value[i]);
        }

        public void CodeMatrix_of_Guid_Array(ref Matrix<Guid>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Guid_(ref value[i]);
        }

        public void CodeMatrix_of_Symbol_Array(ref Matrix<Symbol>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Symbol_(ref value[i]);
        }

        public void CodeMatrix_of_Circle2d_Array(ref Matrix<Circle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Circle2d_(ref value[i]);
        }

        public void CodeMatrix_of_Line2d_Array(ref Matrix<Line2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Line2d_(ref value[i]);
        }

        public void CodeMatrix_of_Line3d_Array(ref Matrix<Line3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Line3d_(ref value[i]);
        }

        public void CodeMatrix_of_Plane2d_Array(ref Matrix<Plane2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Plane2d_(ref value[i]);
        }

        public void CodeMatrix_of_Plane3d_Array(ref Matrix<Plane3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Plane3d_(ref value[i]);
        }

        public void CodeMatrix_of_PlaneWithPoint3d_Array(ref Matrix<PlaneWithPoint3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_PlaneWithPoint3d_(ref value[i]);
        }

        public void CodeMatrix_of_Quad2d_Array(ref Matrix<Quad2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Quad2d_(ref value[i]);
        }

        public void CodeMatrix_of_Quad3d_Array(ref Matrix<Quad3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Quad3d_(ref value[i]);
        }

        public void CodeMatrix_of_Ray2d_Array(ref Matrix<Ray2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Ray2d_(ref value[i]);
        }

        public void CodeMatrix_of_Ray3d_Array(ref Matrix<Ray3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Ray3d_(ref value[i]);
        }

        public void CodeMatrix_of_Sphere3d_Array(ref Matrix<Sphere3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Sphere3d_(ref value[i]);
        }

        public void CodeMatrix_of_Triangle2d_Array(ref Matrix<Triangle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Triangle2d_(ref value[i]);
        }

        public void CodeMatrix_of_Triangle3d_Array(ref Matrix<Triangle3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Triangle3d_(ref value[i]);
        }

        public void CodeMatrix_of_Circle2f_Array(ref Matrix<Circle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Circle2f_(ref value[i]);
        }

        public void CodeMatrix_of_Line2f_Array(ref Matrix<Line2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Line2f_(ref value[i]);
        }

        public void CodeMatrix_of_Line3f_Array(ref Matrix<Line3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Line3f_(ref value[i]);
        }

        public void CodeMatrix_of_Plane2f_Array(ref Matrix<Plane2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Plane2f_(ref value[i]);
        }

        public void CodeMatrix_of_Plane3f_Array(ref Matrix<Plane3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Plane3f_(ref value[i]);
        }

        public void CodeMatrix_of_PlaneWithPoint3f_Array(ref Matrix<PlaneWithPoint3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_PlaneWithPoint3f_(ref value[i]);
        }

        public void CodeMatrix_of_Quad2f_Array(ref Matrix<Quad2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Quad2f_(ref value[i]);
        }

        public void CodeMatrix_of_Quad3f_Array(ref Matrix<Quad3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Quad3f_(ref value[i]);
        }

        public void CodeMatrix_of_Ray2f_Array(ref Matrix<Ray2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Ray2f_(ref value[i]);
        }

        public void CodeMatrix_of_Ray3f_Array(ref Matrix<Ray3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Ray3f_(ref value[i]);
        }

        public void CodeMatrix_of_Sphere3f_Array(ref Matrix<Sphere3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Sphere3f_(ref value[i]);
        }

        public void CodeMatrix_of_Triangle2f_Array(ref Matrix<Triangle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Triangle2f_(ref value[i]);
        }

        public void CodeMatrix_of_Triangle3f_Array(ref Matrix<Triangle3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeMatrix_of_Triangle3f_(ref value[i]);
        }

        public void CodeVolume_of_Byte_Array(ref Volume<byte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Byte_(ref value[i]);
        }

        public void CodeVolume_of_SByte_Array(ref Volume<sbyte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_SByte_(ref value[i]);
        }

        public void CodeVolume_of_Short_Array(ref Volume<short>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Short_(ref value[i]);
        }

        public void CodeVolume_of_UShort_Array(ref Volume<ushort>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_UShort_(ref value[i]);
        }

        public void CodeVolume_of_Int_Array(ref Volume<int>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Int_(ref value[i]);
        }

        public void CodeVolume_of_UInt_Array(ref Volume<uint>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_UInt_(ref value[i]);
        }

        public void CodeVolume_of_Long_Array(ref Volume<long>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Long_(ref value[i]);
        }

        public void CodeVolume_of_ULong_Array(ref Volume<ulong>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_ULong_(ref value[i]);
        }

        public void CodeVolume_of_Float_Array(ref Volume<float>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Float_(ref value[i]);
        }

        public void CodeVolume_of_Double_Array(ref Volume<double>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Double_(ref value[i]);
        }

        public void CodeVolume_of_Fraction_Array(ref Volume<Fraction>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Fraction_(ref value[i]);
        }

        public void CodeVolume_of_V2i_Array(ref Volume<V2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V2i_(ref value[i]);
        }

        public void CodeVolume_of_V2l_Array(ref Volume<V2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V2l_(ref value[i]);
        }

        public void CodeVolume_of_V2f_Array(ref Volume<V2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V2f_(ref value[i]);
        }

        public void CodeVolume_of_V2d_Array(ref Volume<V2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V2d_(ref value[i]);
        }

        public void CodeVolume_of_V3i_Array(ref Volume<V3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V3i_(ref value[i]);
        }

        public void CodeVolume_of_V3l_Array(ref Volume<V3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V3l_(ref value[i]);
        }

        public void CodeVolume_of_V3f_Array(ref Volume<V3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V3f_(ref value[i]);
        }

        public void CodeVolume_of_V3d_Array(ref Volume<V3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V3d_(ref value[i]);
        }

        public void CodeVolume_of_V4i_Array(ref Volume<V4i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V4i_(ref value[i]);
        }

        public void CodeVolume_of_V4l_Array(ref Volume<V4l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V4l_(ref value[i]);
        }

        public void CodeVolume_of_V4f_Array(ref Volume<V4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V4f_(ref value[i]);
        }

        public void CodeVolume_of_V4d_Array(ref Volume<V4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_V4d_(ref value[i]);
        }

        public void CodeVolume_of_M22i_Array(ref Volume<M22i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M22i_(ref value[i]);
        }

        public void CodeVolume_of_M22l_Array(ref Volume<M22l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M22l_(ref value[i]);
        }

        public void CodeVolume_of_M22f_Array(ref Volume<M22f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M22f_(ref value[i]);
        }

        public void CodeVolume_of_M22d_Array(ref Volume<M22d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M22d_(ref value[i]);
        }

        public void CodeVolume_of_M23i_Array(ref Volume<M23i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M23i_(ref value[i]);
        }

        public void CodeVolume_of_M23l_Array(ref Volume<M23l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M23l_(ref value[i]);
        }

        public void CodeVolume_of_M23f_Array(ref Volume<M23f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M23f_(ref value[i]);
        }

        public void CodeVolume_of_M23d_Array(ref Volume<M23d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M23d_(ref value[i]);
        }

        public void CodeVolume_of_M33i_Array(ref Volume<M33i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M33i_(ref value[i]);
        }

        public void CodeVolume_of_M33l_Array(ref Volume<M33l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M33l_(ref value[i]);
        }

        public void CodeVolume_of_M33f_Array(ref Volume<M33f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M33f_(ref value[i]);
        }

        public void CodeVolume_of_M33d_Array(ref Volume<M33d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M33d_(ref value[i]);
        }

        public void CodeVolume_of_M34i_Array(ref Volume<M34i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M34i_(ref value[i]);
        }

        public void CodeVolume_of_M34l_Array(ref Volume<M34l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M34l_(ref value[i]);
        }

        public void CodeVolume_of_M34f_Array(ref Volume<M34f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M34f_(ref value[i]);
        }

        public void CodeVolume_of_M34d_Array(ref Volume<M34d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M34d_(ref value[i]);
        }

        public void CodeVolume_of_M44i_Array(ref Volume<M44i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M44i_(ref value[i]);
        }

        public void CodeVolume_of_M44l_Array(ref Volume<M44l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M44l_(ref value[i]);
        }

        public void CodeVolume_of_M44f_Array(ref Volume<M44f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M44f_(ref value[i]);
        }

        public void CodeVolume_of_M44d_Array(ref Volume<M44d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_M44d_(ref value[i]);
        }

        public void CodeVolume_of_C3b_Array(ref Volume<C3b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C3b_(ref value[i]);
        }

        public void CodeVolume_of_C3us_Array(ref Volume<C3us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C3us_(ref value[i]);
        }

        public void CodeVolume_of_C3ui_Array(ref Volume<C3ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C3ui_(ref value[i]);
        }

        public void CodeVolume_of_C3f_Array(ref Volume<C3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C3f_(ref value[i]);
        }

        public void CodeVolume_of_C3d_Array(ref Volume<C3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C3d_(ref value[i]);
        }

        public void CodeVolume_of_C4b_Array(ref Volume<C4b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C4b_(ref value[i]);
        }

        public void CodeVolume_of_C4us_Array(ref Volume<C4us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C4us_(ref value[i]);
        }

        public void CodeVolume_of_C4ui_Array(ref Volume<C4ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C4ui_(ref value[i]);
        }

        public void CodeVolume_of_C4f_Array(ref Volume<C4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C4f_(ref value[i]);
        }

        public void CodeVolume_of_C4d_Array(ref Volume<C4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_C4d_(ref value[i]);
        }

        public void CodeVolume_of_Range1b_Array(ref Volume<Range1b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1b_(ref value[i]);
        }

        public void CodeVolume_of_Range1sb_Array(ref Volume<Range1sb>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1sb_(ref value[i]);
        }

        public void CodeVolume_of_Range1s_Array(ref Volume<Range1s>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1s_(ref value[i]);
        }

        public void CodeVolume_of_Range1us_Array(ref Volume<Range1us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1us_(ref value[i]);
        }

        public void CodeVolume_of_Range1i_Array(ref Volume<Range1i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1i_(ref value[i]);
        }

        public void CodeVolume_of_Range1ui_Array(ref Volume<Range1ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1ui_(ref value[i]);
        }

        public void CodeVolume_of_Range1l_Array(ref Volume<Range1l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1l_(ref value[i]);
        }

        public void CodeVolume_of_Range1ul_Array(ref Volume<Range1ul>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1ul_(ref value[i]);
        }

        public void CodeVolume_of_Range1f_Array(ref Volume<Range1f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1f_(ref value[i]);
        }

        public void CodeVolume_of_Range1d_Array(ref Volume<Range1d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Range1d_(ref value[i]);
        }

        public void CodeVolume_of_Box2i_Array(ref Volume<Box2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box2i_(ref value[i]);
        }

        public void CodeVolume_of_Box2l_Array(ref Volume<Box2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box2l_(ref value[i]);
        }

        public void CodeVolume_of_Box2f_Array(ref Volume<Box2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box2f_(ref value[i]);
        }

        public void CodeVolume_of_Box2d_Array(ref Volume<Box2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box2d_(ref value[i]);
        }

        public void CodeVolume_of_Box3i_Array(ref Volume<Box3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box3i_(ref value[i]);
        }

        public void CodeVolume_of_Box3l_Array(ref Volume<Box3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box3l_(ref value[i]);
        }

        public void CodeVolume_of_Box3f_Array(ref Volume<Box3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box3f_(ref value[i]);
        }

        public void CodeVolume_of_Box3d_Array(ref Volume<Box3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Box3d_(ref value[i]);
        }

        public void CodeVolume_of_Euclidean3f_Array(ref Volume<Euclidean3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Euclidean3f_(ref value[i]);
        }

        public void CodeVolume_of_Euclidean3d_Array(ref Volume<Euclidean3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Euclidean3d_(ref value[i]);
        }

        public void CodeVolume_of_Rot2f_Array(ref Volume<Rot2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Rot2f_(ref value[i]);
        }

        public void CodeVolume_of_Rot2d_Array(ref Volume<Rot2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Rot2d_(ref value[i]);
        }

        public void CodeVolume_of_Rot3f_Array(ref Volume<Rot3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Rot3f_(ref value[i]);
        }

        public void CodeVolume_of_Rot3d_Array(ref Volume<Rot3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Rot3d_(ref value[i]);
        }

        public void CodeVolume_of_Scale3f_Array(ref Volume<Scale3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Scale3f_(ref value[i]);
        }

        public void CodeVolume_of_Scale3d_Array(ref Volume<Scale3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Scale3d_(ref value[i]);
        }

        public void CodeVolume_of_Shift3f_Array(ref Volume<Shift3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Shift3f_(ref value[i]);
        }

        public void CodeVolume_of_Shift3d_Array(ref Volume<Shift3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Shift3d_(ref value[i]);
        }

        public void CodeVolume_of_Trafo2f_Array(ref Volume<Trafo2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Trafo2f_(ref value[i]);
        }

        public void CodeVolume_of_Trafo2d_Array(ref Volume<Trafo2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Trafo2d_(ref value[i]);
        }

        public void CodeVolume_of_Trafo3f_Array(ref Volume<Trafo3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Trafo3f_(ref value[i]);
        }

        public void CodeVolume_of_Trafo3d_Array(ref Volume<Trafo3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Trafo3d_(ref value[i]);
        }

        public void CodeVolume_of_Bool_Array(ref Volume<bool>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Bool_(ref value[i]);
        }

        public void CodeVolume_of_Char_Array(ref Volume<char>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Char_(ref value[i]);
        }

        public void CodeVolume_of_String_Array(ref Volume<string>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_String_(ref value[i]);
        }

        public void CodeVolume_of_Type_Array(ref Volume<Type>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Type_(ref value[i]);
        }

        public void CodeVolume_of_Guid_Array(ref Volume<Guid>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Guid_(ref value[i]);
        }

        public void CodeVolume_of_Symbol_Array(ref Volume<Symbol>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Symbol_(ref value[i]);
        }

        public void CodeVolume_of_Circle2d_Array(ref Volume<Circle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Circle2d_(ref value[i]);
        }

        public void CodeVolume_of_Line2d_Array(ref Volume<Line2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Line2d_(ref value[i]);
        }

        public void CodeVolume_of_Line3d_Array(ref Volume<Line3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Line3d_(ref value[i]);
        }

        public void CodeVolume_of_Plane2d_Array(ref Volume<Plane2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Plane2d_(ref value[i]);
        }

        public void CodeVolume_of_Plane3d_Array(ref Volume<Plane3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Plane3d_(ref value[i]);
        }

        public void CodeVolume_of_PlaneWithPoint3d_Array(ref Volume<PlaneWithPoint3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_PlaneWithPoint3d_(ref value[i]);
        }

        public void CodeVolume_of_Quad2d_Array(ref Volume<Quad2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Quad2d_(ref value[i]);
        }

        public void CodeVolume_of_Quad3d_Array(ref Volume<Quad3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Quad3d_(ref value[i]);
        }

        public void CodeVolume_of_Ray2d_Array(ref Volume<Ray2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Ray2d_(ref value[i]);
        }

        public void CodeVolume_of_Ray3d_Array(ref Volume<Ray3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Ray3d_(ref value[i]);
        }

        public void CodeVolume_of_Sphere3d_Array(ref Volume<Sphere3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Sphere3d_(ref value[i]);
        }

        public void CodeVolume_of_Triangle2d_Array(ref Volume<Triangle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Triangle2d_(ref value[i]);
        }

        public void CodeVolume_of_Triangle3d_Array(ref Volume<Triangle3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Triangle3d_(ref value[i]);
        }

        public void CodeVolume_of_Circle2f_Array(ref Volume<Circle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Circle2f_(ref value[i]);
        }

        public void CodeVolume_of_Line2f_Array(ref Volume<Line2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Line2f_(ref value[i]);
        }

        public void CodeVolume_of_Line3f_Array(ref Volume<Line3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Line3f_(ref value[i]);
        }

        public void CodeVolume_of_Plane2f_Array(ref Volume<Plane2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Plane2f_(ref value[i]);
        }

        public void CodeVolume_of_Plane3f_Array(ref Volume<Plane3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Plane3f_(ref value[i]);
        }

        public void CodeVolume_of_PlaneWithPoint3f_Array(ref Volume<PlaneWithPoint3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_PlaneWithPoint3f_(ref value[i]);
        }

        public void CodeVolume_of_Quad2f_Array(ref Volume<Quad2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Quad2f_(ref value[i]);
        }

        public void CodeVolume_of_Quad3f_Array(ref Volume<Quad3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Quad3f_(ref value[i]);
        }

        public void CodeVolume_of_Ray2f_Array(ref Volume<Ray2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Ray2f_(ref value[i]);
        }

        public void CodeVolume_of_Ray3f_Array(ref Volume<Ray3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Ray3f_(ref value[i]);
        }

        public void CodeVolume_of_Sphere3f_Array(ref Volume<Sphere3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Sphere3f_(ref value[i]);
        }

        public void CodeVolume_of_Triangle2f_Array(ref Volume<Triangle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Triangle2f_(ref value[i]);
        }

        public void CodeVolume_of_Triangle3f_Array(ref Volume<Triangle3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeVolume_of_Triangle3f_(ref value[i]);
        }

        public void CodeTensor_of_Byte_Array(ref Tensor<byte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Byte_(ref value[i]);
        }

        public void CodeTensor_of_SByte_Array(ref Tensor<sbyte>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_SByte_(ref value[i]);
        }

        public void CodeTensor_of_Short_Array(ref Tensor<short>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Short_(ref value[i]);
        }

        public void CodeTensor_of_UShort_Array(ref Tensor<ushort>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_UShort_(ref value[i]);
        }

        public void CodeTensor_of_Int_Array(ref Tensor<int>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Int_(ref value[i]);
        }

        public void CodeTensor_of_UInt_Array(ref Tensor<uint>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_UInt_(ref value[i]);
        }

        public void CodeTensor_of_Long_Array(ref Tensor<long>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Long_(ref value[i]);
        }

        public void CodeTensor_of_ULong_Array(ref Tensor<ulong>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_ULong_(ref value[i]);
        }

        public void CodeTensor_of_Float_Array(ref Tensor<float>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Float_(ref value[i]);
        }

        public void CodeTensor_of_Double_Array(ref Tensor<double>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Double_(ref value[i]);
        }

        public void CodeTensor_of_Fraction_Array(ref Tensor<Fraction>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Fraction_(ref value[i]);
        }

        public void CodeTensor_of_V2i_Array(ref Tensor<V2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V2i_(ref value[i]);
        }

        public void CodeTensor_of_V2l_Array(ref Tensor<V2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V2l_(ref value[i]);
        }

        public void CodeTensor_of_V2f_Array(ref Tensor<V2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V2f_(ref value[i]);
        }

        public void CodeTensor_of_V2d_Array(ref Tensor<V2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V2d_(ref value[i]);
        }

        public void CodeTensor_of_V3i_Array(ref Tensor<V3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V3i_(ref value[i]);
        }

        public void CodeTensor_of_V3l_Array(ref Tensor<V3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V3l_(ref value[i]);
        }

        public void CodeTensor_of_V3f_Array(ref Tensor<V3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V3f_(ref value[i]);
        }

        public void CodeTensor_of_V3d_Array(ref Tensor<V3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V3d_(ref value[i]);
        }

        public void CodeTensor_of_V4i_Array(ref Tensor<V4i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V4i_(ref value[i]);
        }

        public void CodeTensor_of_V4l_Array(ref Tensor<V4l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V4l_(ref value[i]);
        }

        public void CodeTensor_of_V4f_Array(ref Tensor<V4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V4f_(ref value[i]);
        }

        public void CodeTensor_of_V4d_Array(ref Tensor<V4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_V4d_(ref value[i]);
        }

        public void CodeTensor_of_M22i_Array(ref Tensor<M22i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M22i_(ref value[i]);
        }

        public void CodeTensor_of_M22l_Array(ref Tensor<M22l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M22l_(ref value[i]);
        }

        public void CodeTensor_of_M22f_Array(ref Tensor<M22f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M22f_(ref value[i]);
        }

        public void CodeTensor_of_M22d_Array(ref Tensor<M22d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M22d_(ref value[i]);
        }

        public void CodeTensor_of_M23i_Array(ref Tensor<M23i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M23i_(ref value[i]);
        }

        public void CodeTensor_of_M23l_Array(ref Tensor<M23l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M23l_(ref value[i]);
        }

        public void CodeTensor_of_M23f_Array(ref Tensor<M23f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M23f_(ref value[i]);
        }

        public void CodeTensor_of_M23d_Array(ref Tensor<M23d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M23d_(ref value[i]);
        }

        public void CodeTensor_of_M33i_Array(ref Tensor<M33i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M33i_(ref value[i]);
        }

        public void CodeTensor_of_M33l_Array(ref Tensor<M33l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M33l_(ref value[i]);
        }

        public void CodeTensor_of_M33f_Array(ref Tensor<M33f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M33f_(ref value[i]);
        }

        public void CodeTensor_of_M33d_Array(ref Tensor<M33d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M33d_(ref value[i]);
        }

        public void CodeTensor_of_M34i_Array(ref Tensor<M34i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M34i_(ref value[i]);
        }

        public void CodeTensor_of_M34l_Array(ref Tensor<M34l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M34l_(ref value[i]);
        }

        public void CodeTensor_of_M34f_Array(ref Tensor<M34f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M34f_(ref value[i]);
        }

        public void CodeTensor_of_M34d_Array(ref Tensor<M34d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M34d_(ref value[i]);
        }

        public void CodeTensor_of_M44i_Array(ref Tensor<M44i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M44i_(ref value[i]);
        }

        public void CodeTensor_of_M44l_Array(ref Tensor<M44l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M44l_(ref value[i]);
        }

        public void CodeTensor_of_M44f_Array(ref Tensor<M44f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M44f_(ref value[i]);
        }

        public void CodeTensor_of_M44d_Array(ref Tensor<M44d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_M44d_(ref value[i]);
        }

        public void CodeTensor_of_C3b_Array(ref Tensor<C3b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C3b_(ref value[i]);
        }

        public void CodeTensor_of_C3us_Array(ref Tensor<C3us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C3us_(ref value[i]);
        }

        public void CodeTensor_of_C3ui_Array(ref Tensor<C3ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C3ui_(ref value[i]);
        }

        public void CodeTensor_of_C3f_Array(ref Tensor<C3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C3f_(ref value[i]);
        }

        public void CodeTensor_of_C3d_Array(ref Tensor<C3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C3d_(ref value[i]);
        }

        public void CodeTensor_of_C4b_Array(ref Tensor<C4b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C4b_(ref value[i]);
        }

        public void CodeTensor_of_C4us_Array(ref Tensor<C4us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C4us_(ref value[i]);
        }

        public void CodeTensor_of_C4ui_Array(ref Tensor<C4ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C4ui_(ref value[i]);
        }

        public void CodeTensor_of_C4f_Array(ref Tensor<C4f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C4f_(ref value[i]);
        }

        public void CodeTensor_of_C4d_Array(ref Tensor<C4d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_C4d_(ref value[i]);
        }

        public void CodeTensor_of_Range1b_Array(ref Tensor<Range1b>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1b_(ref value[i]);
        }

        public void CodeTensor_of_Range1sb_Array(ref Tensor<Range1sb>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1sb_(ref value[i]);
        }

        public void CodeTensor_of_Range1s_Array(ref Tensor<Range1s>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1s_(ref value[i]);
        }

        public void CodeTensor_of_Range1us_Array(ref Tensor<Range1us>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1us_(ref value[i]);
        }

        public void CodeTensor_of_Range1i_Array(ref Tensor<Range1i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1i_(ref value[i]);
        }

        public void CodeTensor_of_Range1ui_Array(ref Tensor<Range1ui>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1ui_(ref value[i]);
        }

        public void CodeTensor_of_Range1l_Array(ref Tensor<Range1l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1l_(ref value[i]);
        }

        public void CodeTensor_of_Range1ul_Array(ref Tensor<Range1ul>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1ul_(ref value[i]);
        }

        public void CodeTensor_of_Range1f_Array(ref Tensor<Range1f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1f_(ref value[i]);
        }

        public void CodeTensor_of_Range1d_Array(ref Tensor<Range1d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Range1d_(ref value[i]);
        }

        public void CodeTensor_of_Box2i_Array(ref Tensor<Box2i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box2i_(ref value[i]);
        }

        public void CodeTensor_of_Box2l_Array(ref Tensor<Box2l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box2l_(ref value[i]);
        }

        public void CodeTensor_of_Box2f_Array(ref Tensor<Box2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box2f_(ref value[i]);
        }

        public void CodeTensor_of_Box2d_Array(ref Tensor<Box2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box2d_(ref value[i]);
        }

        public void CodeTensor_of_Box3i_Array(ref Tensor<Box3i>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box3i_(ref value[i]);
        }

        public void CodeTensor_of_Box3l_Array(ref Tensor<Box3l>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box3l_(ref value[i]);
        }

        public void CodeTensor_of_Box3f_Array(ref Tensor<Box3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box3f_(ref value[i]);
        }

        public void CodeTensor_of_Box3d_Array(ref Tensor<Box3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Box3d_(ref value[i]);
        }

        public void CodeTensor_of_Euclidean3f_Array(ref Tensor<Euclidean3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Euclidean3f_(ref value[i]);
        }

        public void CodeTensor_of_Euclidean3d_Array(ref Tensor<Euclidean3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Euclidean3d_(ref value[i]);
        }

        public void CodeTensor_of_Rot2f_Array(ref Tensor<Rot2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Rot2f_(ref value[i]);
        }

        public void CodeTensor_of_Rot2d_Array(ref Tensor<Rot2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Rot2d_(ref value[i]);
        }

        public void CodeTensor_of_Rot3f_Array(ref Tensor<Rot3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Rot3f_(ref value[i]);
        }

        public void CodeTensor_of_Rot3d_Array(ref Tensor<Rot3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Rot3d_(ref value[i]);
        }

        public void CodeTensor_of_Scale3f_Array(ref Tensor<Scale3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Scale3f_(ref value[i]);
        }

        public void CodeTensor_of_Scale3d_Array(ref Tensor<Scale3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Scale3d_(ref value[i]);
        }

        public void CodeTensor_of_Shift3f_Array(ref Tensor<Shift3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Shift3f_(ref value[i]);
        }

        public void CodeTensor_of_Shift3d_Array(ref Tensor<Shift3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Shift3d_(ref value[i]);
        }

        public void CodeTensor_of_Trafo2f_Array(ref Tensor<Trafo2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Trafo2f_(ref value[i]);
        }

        public void CodeTensor_of_Trafo2d_Array(ref Tensor<Trafo2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Trafo2d_(ref value[i]);
        }

        public void CodeTensor_of_Trafo3f_Array(ref Tensor<Trafo3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Trafo3f_(ref value[i]);
        }

        public void CodeTensor_of_Trafo3d_Array(ref Tensor<Trafo3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Trafo3d_(ref value[i]);
        }

        public void CodeTensor_of_Bool_Array(ref Tensor<bool>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Bool_(ref value[i]);
        }

        public void CodeTensor_of_Char_Array(ref Tensor<char>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Char_(ref value[i]);
        }

        public void CodeTensor_of_String_Array(ref Tensor<string>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_String_(ref value[i]);
        }

        public void CodeTensor_of_Type_Array(ref Tensor<Type>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Type_(ref value[i]);
        }

        public void CodeTensor_of_Guid_Array(ref Tensor<Guid>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Guid_(ref value[i]);
        }

        public void CodeTensor_of_Symbol_Array(ref Tensor<Symbol>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Symbol_(ref value[i]);
        }

        public void CodeTensor_of_Circle2d_Array(ref Tensor<Circle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Circle2d_(ref value[i]);
        }

        public void CodeTensor_of_Line2d_Array(ref Tensor<Line2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Line2d_(ref value[i]);
        }

        public void CodeTensor_of_Line3d_Array(ref Tensor<Line3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Line3d_(ref value[i]);
        }

        public void CodeTensor_of_Plane2d_Array(ref Tensor<Plane2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Plane2d_(ref value[i]);
        }

        public void CodeTensor_of_Plane3d_Array(ref Tensor<Plane3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Plane3d_(ref value[i]);
        }

        public void CodeTensor_of_PlaneWithPoint3d_Array(ref Tensor<PlaneWithPoint3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_PlaneWithPoint3d_(ref value[i]);
        }

        public void CodeTensor_of_Quad2d_Array(ref Tensor<Quad2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Quad2d_(ref value[i]);
        }

        public void CodeTensor_of_Quad3d_Array(ref Tensor<Quad3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Quad3d_(ref value[i]);
        }

        public void CodeTensor_of_Ray2d_Array(ref Tensor<Ray2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Ray2d_(ref value[i]);
        }

        public void CodeTensor_of_Ray3d_Array(ref Tensor<Ray3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Ray3d_(ref value[i]);
        }

        public void CodeTensor_of_Sphere3d_Array(ref Tensor<Sphere3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Sphere3d_(ref value[i]);
        }

        public void CodeTensor_of_Triangle2d_Array(ref Tensor<Triangle2d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Triangle2d_(ref value[i]);
        }

        public void CodeTensor_of_Triangle3d_Array(ref Tensor<Triangle3d>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Triangle3d_(ref value[i]);
        }

        public void CodeTensor_of_Circle2f_Array(ref Tensor<Circle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Circle2f_(ref value[i]);
        }

        public void CodeTensor_of_Line2f_Array(ref Tensor<Line2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Line2f_(ref value[i]);
        }

        public void CodeTensor_of_Line3f_Array(ref Tensor<Line3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Line3f_(ref value[i]);
        }

        public void CodeTensor_of_Plane2f_Array(ref Tensor<Plane2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Plane2f_(ref value[i]);
        }

        public void CodeTensor_of_Plane3f_Array(ref Tensor<Plane3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Plane3f_(ref value[i]);
        }

        public void CodeTensor_of_PlaneWithPoint3f_Array(ref Tensor<PlaneWithPoint3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_PlaneWithPoint3f_(ref value[i]);
        }

        public void CodeTensor_of_Quad2f_Array(ref Tensor<Quad2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Quad2f_(ref value[i]);
        }

        public void CodeTensor_of_Quad3f_Array(ref Tensor<Quad3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Quad3f_(ref value[i]);
        }

        public void CodeTensor_of_Ray2f_Array(ref Tensor<Ray2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Ray2f_(ref value[i]);
        }

        public void CodeTensor_of_Ray3f_Array(ref Tensor<Ray3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Ray3f_(ref value[i]);
        }

        public void CodeTensor_of_Sphere3f_Array(ref Tensor<Sphere3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Sphere3f_(ref value[i]);
        }

        public void CodeTensor_of_Triangle2f_Array(ref Tensor<Triangle2f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Triangle2f_(ref value[i]);
        }

        public void CodeTensor_of_Triangle3f_Array(ref Tensor<Triangle3f>[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTensor_of_Triangle3f_(ref value[i]);
        }

        #endregion

        #region Multi-Dimensional Arrays

        public void CodeByteArray2d(ref byte[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeByteArray3d(ref byte[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeSByteArray2d(ref sbyte[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeSByteArray3d(ref sbyte[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeShortArray2d(ref short[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeShortArray3d(ref short[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeUShortArray2d(ref ushort[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeUShortArray3d(ref ushort[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeIntArray2d(ref int[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeIntArray3d(ref int[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeUIntArray2d(ref uint[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeUIntArray3d(ref uint[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeLongArray2d(ref long[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeLongArray3d(ref long[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeULongArray2d(ref ulong[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeULongArray3d(ref ulong[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeFloatArray2d(ref float[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeFloatArray3d(ref float[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeDoubleArray2d(ref double[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeDoubleArray3d(ref double[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeFractionArray2d(ref Fraction[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeFractionArray3d(ref Fraction[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV2iArray2d(ref V2i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV2iArray3d(ref V2i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV2lArray2d(ref V2l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV2lArray3d(ref V2l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV2fArray2d(ref V2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV2fArray3d(ref V2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV2dArray2d(ref V2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV2dArray3d(ref V2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV3iArray2d(ref V3i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV3iArray3d(ref V3i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV3lArray2d(ref V3l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV3lArray3d(ref V3l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV3fArray2d(ref V3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV3fArray3d(ref V3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV3dArray2d(ref V3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV3dArray3d(ref V3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV4iArray2d(ref V4i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV4iArray3d(ref V4i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV4lArray2d(ref V4l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV4lArray3d(ref V4l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV4fArray2d(ref V4f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV4fArray3d(ref V4f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeV4dArray2d(ref V4d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeV4dArray3d(ref V4d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM22iArray2d(ref M22i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM22iArray3d(ref M22i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM22lArray2d(ref M22l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM22lArray3d(ref M22l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM22fArray2d(ref M22f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM22fArray3d(ref M22f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM22dArray2d(ref M22d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM22dArray3d(ref M22d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM23iArray2d(ref M23i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM23iArray3d(ref M23i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM23lArray2d(ref M23l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM23lArray3d(ref M23l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM23fArray2d(ref M23f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM23fArray3d(ref M23f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM23dArray2d(ref M23d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM23dArray3d(ref M23d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM33iArray2d(ref M33i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM33iArray3d(ref M33i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM33lArray2d(ref M33l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM33lArray3d(ref M33l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM33fArray2d(ref M33f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM33fArray3d(ref M33f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM33dArray2d(ref M33d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM33dArray3d(ref M33d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM34iArray2d(ref M34i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM34iArray3d(ref M34i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM34lArray2d(ref M34l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM34lArray3d(ref M34l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM34fArray2d(ref M34f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM34fArray3d(ref M34f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM34dArray2d(ref M34d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM34dArray3d(ref M34d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM44iArray2d(ref M44i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM44iArray3d(ref M44i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM44lArray2d(ref M44l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM44lArray3d(ref M44l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM44fArray2d(ref M44f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM44fArray3d(ref M44f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeM44dArray2d(ref M44d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeM44dArray3d(ref M44d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC3bArray2d(ref C3b[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC3bArray3d(ref C3b[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC3usArray2d(ref C3us[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC3usArray3d(ref C3us[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC3uiArray2d(ref C3ui[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC3uiArray3d(ref C3ui[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC3fArray2d(ref C3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC3fArray3d(ref C3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC3dArray2d(ref C3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC3dArray3d(ref C3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC4bArray2d(ref C4b[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC4bArray3d(ref C4b[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC4usArray2d(ref C4us[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC4usArray3d(ref C4us[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC4uiArray2d(ref C4ui[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC4uiArray3d(ref C4ui[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC4fArray2d(ref C4f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC4fArray3d(ref C4f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeC4dArray2d(ref C4d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeC4dArray3d(ref C4d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1bArray2d(ref Range1b[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1bArray3d(ref Range1b[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1sbArray2d(ref Range1sb[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1sbArray3d(ref Range1sb[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1sArray2d(ref Range1s[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1sArray3d(ref Range1s[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1usArray2d(ref Range1us[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1usArray3d(ref Range1us[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1iArray2d(ref Range1i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1iArray3d(ref Range1i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1uiArray2d(ref Range1ui[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1uiArray3d(ref Range1ui[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1lArray2d(ref Range1l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1lArray3d(ref Range1l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1ulArray2d(ref Range1ul[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1ulArray3d(ref Range1ul[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1fArray2d(ref Range1f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1fArray3d(ref Range1f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRange1dArray2d(ref Range1d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRange1dArray3d(ref Range1d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox2iArray2d(ref Box2i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox2iArray3d(ref Box2i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox2lArray2d(ref Box2l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox2lArray3d(ref Box2l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox2fArray2d(ref Box2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox2fArray3d(ref Box2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox2dArray2d(ref Box2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox2dArray3d(ref Box2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox3iArray2d(ref Box3i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox3iArray3d(ref Box3i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox3lArray2d(ref Box3l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox3lArray3d(ref Box3l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox3fArray2d(ref Box3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox3fArray3d(ref Box3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeBox3dArray2d(ref Box3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeBox3dArray3d(ref Box3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeEuclidean3fArray2d(ref Euclidean3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeEuclidean3fArray3d(ref Euclidean3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeEuclidean3dArray2d(ref Euclidean3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeEuclidean3dArray3d(ref Euclidean3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRot2fArray2d(ref Rot2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRot2fArray3d(ref Rot2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRot2dArray2d(ref Rot2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRot2dArray3d(ref Rot2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRot3fArray2d(ref Rot3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRot3fArray3d(ref Rot3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeRot3dArray2d(ref Rot3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeRot3dArray3d(ref Rot3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeScale3fArray2d(ref Scale3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeScale3fArray3d(ref Scale3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeScale3dArray2d(ref Scale3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeScale3dArray3d(ref Scale3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeShift3fArray2d(ref Shift3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeShift3fArray3d(ref Shift3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeShift3dArray2d(ref Shift3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeShift3dArray3d(ref Shift3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo2fArray2d(ref Trafo2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeTrafo2fArray3d(ref Trafo2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo2dArray2d(ref Trafo2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeTrafo2dArray3d(ref Trafo2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo3fArray2d(ref Trafo3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeTrafo3fArray3d(ref Trafo3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo3dArray2d(ref Trafo3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_reader.ReadArray(value, c0 * c1);
        }

        public void CodeTrafo3dArray3d(ref Trafo3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_reader.ReadArray(value, c0 * c1 * c2);
        }

        #endregion

        #region Jagged Multi-Dimensional Arrays

        public void CodeByteArrayArray(ref byte[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeByteArray(ref value[i]);
        }

        public void CodeByteArrayArrayArray(ref byte[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeByteArrayArray(ref value[i]);
        }

        public void CodeSByteArrayArray(ref sbyte[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeSByteArray(ref value[i]);
        }

        public void CodeSByteArrayArrayArray(ref sbyte[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeSByteArrayArray(ref value[i]);
        }

        public void CodeShortArrayArray(ref short[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShortArray(ref value[i]);
        }

        public void CodeShortArrayArrayArray(ref short[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShortArrayArray(ref value[i]);
        }

        public void CodeUShortArrayArray(ref ushort[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeUShortArray(ref value[i]);
        }

        public void CodeUShortArrayArrayArray(ref ushort[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeUShortArrayArray(ref value[i]);
        }

        public void CodeIntArrayArray(ref int[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeIntArray(ref value[i]);
        }

        public void CodeIntArrayArrayArray(ref int[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeIntArrayArray(ref value[i]);
        }

        public void CodeUIntArrayArray(ref uint[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeUIntArray(ref value[i]);
        }

        public void CodeUIntArrayArrayArray(ref uint[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeUIntArrayArray(ref value[i]);
        }

        public void CodeLongArrayArray(ref long[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeLongArray(ref value[i]);
        }

        public void CodeLongArrayArrayArray(ref long[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeLongArrayArray(ref value[i]);
        }

        public void CodeULongArrayArray(ref ulong[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeULongArray(ref value[i]);
        }

        public void CodeULongArrayArrayArray(ref ulong[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeULongArrayArray(ref value[i]);
        }

        public void CodeFloatArrayArray(ref float[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeFloatArray(ref value[i]);
        }

        public void CodeFloatArrayArrayArray(ref float[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeFloatArrayArray(ref value[i]);
        }

        public void CodeDoubleArrayArray(ref double[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeDoubleArray(ref value[i]);
        }

        public void CodeDoubleArrayArrayArray(ref double[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeDoubleArrayArray(ref value[i]);
        }

        public void CodeFractionArrayArray(ref Fraction[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeFractionArray(ref value[i]);
        }

        public void CodeFractionArrayArrayArray(ref Fraction[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeFractionArrayArray(ref value[i]);
        }

        public void CodeV2iArrayArray(ref V2i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2iArray(ref value[i]);
        }

        public void CodeV2iArrayArrayArray(ref V2i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2iArrayArray(ref value[i]);
        }

        public void CodeV2lArrayArray(ref V2l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2lArray(ref value[i]);
        }

        public void CodeV2lArrayArrayArray(ref V2l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2lArrayArray(ref value[i]);
        }

        public void CodeV2fArrayArray(ref V2f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2fArray(ref value[i]);
        }

        public void CodeV2fArrayArrayArray(ref V2f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2fArrayArray(ref value[i]);
        }

        public void CodeV2dArrayArray(ref V2d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2dArray(ref value[i]);
        }

        public void CodeV2dArrayArrayArray(ref V2d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV2dArrayArray(ref value[i]);
        }

        public void CodeV3iArrayArray(ref V3i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3iArray(ref value[i]);
        }

        public void CodeV3iArrayArrayArray(ref V3i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3iArrayArray(ref value[i]);
        }

        public void CodeV3lArrayArray(ref V3l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3lArray(ref value[i]);
        }

        public void CodeV3lArrayArrayArray(ref V3l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3lArrayArray(ref value[i]);
        }

        public void CodeV3fArrayArray(ref V3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3fArray(ref value[i]);
        }

        public void CodeV3fArrayArrayArray(ref V3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3fArrayArray(ref value[i]);
        }

        public void CodeV3dArrayArray(ref V3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3dArray(ref value[i]);
        }

        public void CodeV3dArrayArrayArray(ref V3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV3dArrayArray(ref value[i]);
        }

        public void CodeV4iArrayArray(ref V4i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4iArray(ref value[i]);
        }

        public void CodeV4iArrayArrayArray(ref V4i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4iArrayArray(ref value[i]);
        }

        public void CodeV4lArrayArray(ref V4l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4lArray(ref value[i]);
        }

        public void CodeV4lArrayArrayArray(ref V4l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4lArrayArray(ref value[i]);
        }

        public void CodeV4fArrayArray(ref V4f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4fArray(ref value[i]);
        }

        public void CodeV4fArrayArrayArray(ref V4f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4fArrayArray(ref value[i]);
        }

        public void CodeV4dArrayArray(ref V4d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4dArray(ref value[i]);
        }

        public void CodeV4dArrayArrayArray(ref V4d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeV4dArrayArray(ref value[i]);
        }

        public void CodeM22iArrayArray(ref M22i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22iArray(ref value[i]);
        }

        public void CodeM22iArrayArrayArray(ref M22i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22iArrayArray(ref value[i]);
        }

        public void CodeM22lArrayArray(ref M22l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22lArray(ref value[i]);
        }

        public void CodeM22lArrayArrayArray(ref M22l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22lArrayArray(ref value[i]);
        }

        public void CodeM22fArrayArray(ref M22f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22fArray(ref value[i]);
        }

        public void CodeM22fArrayArrayArray(ref M22f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22fArrayArray(ref value[i]);
        }

        public void CodeM22dArrayArray(ref M22d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22dArray(ref value[i]);
        }

        public void CodeM22dArrayArrayArray(ref M22d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM22dArrayArray(ref value[i]);
        }

        public void CodeM23iArrayArray(ref M23i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23iArray(ref value[i]);
        }

        public void CodeM23iArrayArrayArray(ref M23i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23iArrayArray(ref value[i]);
        }

        public void CodeM23lArrayArray(ref M23l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23lArray(ref value[i]);
        }

        public void CodeM23lArrayArrayArray(ref M23l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23lArrayArray(ref value[i]);
        }

        public void CodeM23fArrayArray(ref M23f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23fArray(ref value[i]);
        }

        public void CodeM23fArrayArrayArray(ref M23f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23fArrayArray(ref value[i]);
        }

        public void CodeM23dArrayArray(ref M23d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23dArray(ref value[i]);
        }

        public void CodeM23dArrayArrayArray(ref M23d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM23dArrayArray(ref value[i]);
        }

        public void CodeM33iArrayArray(ref M33i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33iArray(ref value[i]);
        }

        public void CodeM33iArrayArrayArray(ref M33i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33iArrayArray(ref value[i]);
        }

        public void CodeM33lArrayArray(ref M33l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33lArray(ref value[i]);
        }

        public void CodeM33lArrayArrayArray(ref M33l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33lArrayArray(ref value[i]);
        }

        public void CodeM33fArrayArray(ref M33f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33fArray(ref value[i]);
        }

        public void CodeM33fArrayArrayArray(ref M33f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33fArrayArray(ref value[i]);
        }

        public void CodeM33dArrayArray(ref M33d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33dArray(ref value[i]);
        }

        public void CodeM33dArrayArrayArray(ref M33d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM33dArrayArray(ref value[i]);
        }

        public void CodeM34iArrayArray(ref M34i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34iArray(ref value[i]);
        }

        public void CodeM34iArrayArrayArray(ref M34i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34iArrayArray(ref value[i]);
        }

        public void CodeM34lArrayArray(ref M34l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34lArray(ref value[i]);
        }

        public void CodeM34lArrayArrayArray(ref M34l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34lArrayArray(ref value[i]);
        }

        public void CodeM34fArrayArray(ref M34f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34fArray(ref value[i]);
        }

        public void CodeM34fArrayArrayArray(ref M34f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34fArrayArray(ref value[i]);
        }

        public void CodeM34dArrayArray(ref M34d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34dArray(ref value[i]);
        }

        public void CodeM34dArrayArrayArray(ref M34d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM34dArrayArray(ref value[i]);
        }

        public void CodeM44iArrayArray(ref M44i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44iArray(ref value[i]);
        }

        public void CodeM44iArrayArrayArray(ref M44i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44iArrayArray(ref value[i]);
        }

        public void CodeM44lArrayArray(ref M44l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44lArray(ref value[i]);
        }

        public void CodeM44lArrayArrayArray(ref M44l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44lArrayArray(ref value[i]);
        }

        public void CodeM44fArrayArray(ref M44f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44fArray(ref value[i]);
        }

        public void CodeM44fArrayArrayArray(ref M44f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44fArrayArray(ref value[i]);
        }

        public void CodeM44dArrayArray(ref M44d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44dArray(ref value[i]);
        }

        public void CodeM44dArrayArrayArray(ref M44d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeM44dArrayArray(ref value[i]);
        }

        public void CodeC3bArrayArray(ref C3b[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3bArray(ref value[i]);
        }

        public void CodeC3bArrayArrayArray(ref C3b[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3bArrayArray(ref value[i]);
        }

        public void CodeC3usArrayArray(ref C3us[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3usArray(ref value[i]);
        }

        public void CodeC3usArrayArrayArray(ref C3us[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3usArrayArray(ref value[i]);
        }

        public void CodeC3uiArrayArray(ref C3ui[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3uiArray(ref value[i]);
        }

        public void CodeC3uiArrayArrayArray(ref C3ui[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3uiArrayArray(ref value[i]);
        }

        public void CodeC3fArrayArray(ref C3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3fArray(ref value[i]);
        }

        public void CodeC3fArrayArrayArray(ref C3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3fArrayArray(ref value[i]);
        }

        public void CodeC3dArrayArray(ref C3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3dArray(ref value[i]);
        }

        public void CodeC3dArrayArrayArray(ref C3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC3dArrayArray(ref value[i]);
        }

        public void CodeC4bArrayArray(ref C4b[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4bArray(ref value[i]);
        }

        public void CodeC4bArrayArrayArray(ref C4b[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4bArrayArray(ref value[i]);
        }

        public void CodeC4usArrayArray(ref C4us[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4usArray(ref value[i]);
        }

        public void CodeC4usArrayArrayArray(ref C4us[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4usArrayArray(ref value[i]);
        }

        public void CodeC4uiArrayArray(ref C4ui[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4uiArray(ref value[i]);
        }

        public void CodeC4uiArrayArrayArray(ref C4ui[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4uiArrayArray(ref value[i]);
        }

        public void CodeC4fArrayArray(ref C4f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4fArray(ref value[i]);
        }

        public void CodeC4fArrayArrayArray(ref C4f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4fArrayArray(ref value[i]);
        }

        public void CodeC4dArrayArray(ref C4d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4dArray(ref value[i]);
        }

        public void CodeC4dArrayArrayArray(ref C4d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeC4dArrayArray(ref value[i]);
        }

        public void CodeRange1bArrayArray(ref Range1b[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1bArray(ref value[i]);
        }

        public void CodeRange1bArrayArrayArray(ref Range1b[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1bArrayArray(ref value[i]);
        }

        public void CodeRange1sbArrayArray(ref Range1sb[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1sbArray(ref value[i]);
        }

        public void CodeRange1sbArrayArrayArray(ref Range1sb[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1sbArrayArray(ref value[i]);
        }

        public void CodeRange1sArrayArray(ref Range1s[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1sArray(ref value[i]);
        }

        public void CodeRange1sArrayArrayArray(ref Range1s[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1sArrayArray(ref value[i]);
        }

        public void CodeRange1usArrayArray(ref Range1us[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1usArray(ref value[i]);
        }

        public void CodeRange1usArrayArrayArray(ref Range1us[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1usArrayArray(ref value[i]);
        }

        public void CodeRange1iArrayArray(ref Range1i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1iArray(ref value[i]);
        }

        public void CodeRange1iArrayArrayArray(ref Range1i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1iArrayArray(ref value[i]);
        }

        public void CodeRange1uiArrayArray(ref Range1ui[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1uiArray(ref value[i]);
        }

        public void CodeRange1uiArrayArrayArray(ref Range1ui[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1uiArrayArray(ref value[i]);
        }

        public void CodeRange1lArrayArray(ref Range1l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1lArray(ref value[i]);
        }

        public void CodeRange1lArrayArrayArray(ref Range1l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1lArrayArray(ref value[i]);
        }

        public void CodeRange1ulArrayArray(ref Range1ul[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1ulArray(ref value[i]);
        }

        public void CodeRange1ulArrayArrayArray(ref Range1ul[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1ulArrayArray(ref value[i]);
        }

        public void CodeRange1fArrayArray(ref Range1f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1fArray(ref value[i]);
        }

        public void CodeRange1fArrayArrayArray(ref Range1f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1fArrayArray(ref value[i]);
        }

        public void CodeRange1dArrayArray(ref Range1d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1dArray(ref value[i]);
        }

        public void CodeRange1dArrayArrayArray(ref Range1d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRange1dArrayArray(ref value[i]);
        }

        public void CodeBox2iArrayArray(ref Box2i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2iArray(ref value[i]);
        }

        public void CodeBox2iArrayArrayArray(ref Box2i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2iArrayArray(ref value[i]);
        }

        public void CodeBox2lArrayArray(ref Box2l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2lArray(ref value[i]);
        }

        public void CodeBox2lArrayArrayArray(ref Box2l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2lArrayArray(ref value[i]);
        }

        public void CodeBox2fArrayArray(ref Box2f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2fArray(ref value[i]);
        }

        public void CodeBox2fArrayArrayArray(ref Box2f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2fArrayArray(ref value[i]);
        }

        public void CodeBox2dArrayArray(ref Box2d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2dArray(ref value[i]);
        }

        public void CodeBox2dArrayArrayArray(ref Box2d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox2dArrayArray(ref value[i]);
        }

        public void CodeBox3iArrayArray(ref Box3i[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3iArray(ref value[i]);
        }

        public void CodeBox3iArrayArrayArray(ref Box3i[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3iArrayArray(ref value[i]);
        }

        public void CodeBox3lArrayArray(ref Box3l[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3lArray(ref value[i]);
        }

        public void CodeBox3lArrayArrayArray(ref Box3l[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3lArrayArray(ref value[i]);
        }

        public void CodeBox3fArrayArray(ref Box3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3fArray(ref value[i]);
        }

        public void CodeBox3fArrayArrayArray(ref Box3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3fArrayArray(ref value[i]);
        }

        public void CodeBox3dArrayArray(ref Box3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3dArray(ref value[i]);
        }

        public void CodeBox3dArrayArrayArray(ref Box3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeBox3dArrayArray(ref value[i]);
        }

        public void CodeEuclidean3fArrayArray(ref Euclidean3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeEuclidean3fArray(ref value[i]);
        }

        public void CodeEuclidean3fArrayArrayArray(ref Euclidean3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeEuclidean3fArrayArray(ref value[i]);
        }

        public void CodeEuclidean3dArrayArray(ref Euclidean3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeEuclidean3dArray(ref value[i]);
        }

        public void CodeEuclidean3dArrayArrayArray(ref Euclidean3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeEuclidean3dArrayArray(ref value[i]);
        }

        public void CodeRot2fArrayArray(ref Rot2f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot2fArray(ref value[i]);
        }

        public void CodeRot2fArrayArrayArray(ref Rot2f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot2fArrayArray(ref value[i]);
        }

        public void CodeRot2dArrayArray(ref Rot2d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot2dArray(ref value[i]);
        }

        public void CodeRot2dArrayArrayArray(ref Rot2d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot2dArrayArray(ref value[i]);
        }

        public void CodeRot3fArrayArray(ref Rot3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot3fArray(ref value[i]);
        }

        public void CodeRot3fArrayArrayArray(ref Rot3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot3fArrayArray(ref value[i]);
        }

        public void CodeRot3dArrayArray(ref Rot3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot3dArray(ref value[i]);
        }

        public void CodeRot3dArrayArrayArray(ref Rot3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeRot3dArrayArray(ref value[i]);
        }

        public void CodeScale3fArrayArray(ref Scale3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeScale3fArray(ref value[i]);
        }

        public void CodeScale3fArrayArrayArray(ref Scale3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeScale3fArrayArray(ref value[i]);
        }

        public void CodeScale3dArrayArray(ref Scale3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeScale3dArray(ref value[i]);
        }

        public void CodeScale3dArrayArrayArray(ref Scale3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeScale3dArrayArray(ref value[i]);
        }

        public void CodeShift3fArrayArray(ref Shift3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShift3fArray(ref value[i]);
        }

        public void CodeShift3fArrayArrayArray(ref Shift3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShift3fArrayArray(ref value[i]);
        }

        public void CodeShift3dArrayArray(ref Shift3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShift3dArray(ref value[i]);
        }

        public void CodeShift3dArrayArrayArray(ref Shift3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeShift3dArrayArray(ref value[i]);
        }

        public void CodeTrafo2fArrayArray(ref Trafo2f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo2fArray(ref value[i]);
        }

        public void CodeTrafo2fArrayArrayArray(ref Trafo2f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo2fArrayArray(ref value[i]);
        }

        public void CodeTrafo2dArrayArray(ref Trafo2d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo2dArray(ref value[i]);
        }

        public void CodeTrafo2dArrayArrayArray(ref Trafo2d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo2dArrayArray(ref value[i]);
        }

        public void CodeTrafo3fArrayArray(ref Trafo3f[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo3fArray(ref value[i]);
        }

        public void CodeTrafo3fArrayArrayArray(ref Trafo3f[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo3fArrayArray(ref value[i]);
        }

        public void CodeTrafo3dArrayArray(ref Trafo3d[][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo3dArray(ref value[i]);
        }

        public void CodeTrafo3dArrayArrayArray(ref Trafo3d[][][] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            for (long i = 0; i < count; i++) CodeTrafo3dArrayArray(ref value[i]);
        }

        #endregion

        #region Lists

        public void CodeList_of_V2i_(ref List<V2i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V2ui_(ref List<V2ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V2l_(ref List<V2l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V2f_(ref List<V2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V2d_(ref List<V2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V3i_(ref List<V3i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V3ui_(ref List<V3ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V3l_(ref List<V3l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V3f_(ref List<V3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V3d_(ref List<V3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V4i_(ref List<V4i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V4ui_(ref List<V4ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V4l_(ref List<V4l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V4f_(ref List<V4f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_V4d_(ref List<V4d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M22i_(ref List<M22i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M22l_(ref List<M22l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M22f_(ref List<M22f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M22d_(ref List<M22d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M23i_(ref List<M23i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M23l_(ref List<M23l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M23f_(ref List<M23f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M23d_(ref List<M23d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M33i_(ref List<M33i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M33l_(ref List<M33l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M33f_(ref List<M33f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M33d_(ref List<M33d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M34i_(ref List<M34i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M34l_(ref List<M34l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M34f_(ref List<M34f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M34d_(ref List<M34d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M44i_(ref List<M44i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M44l_(ref List<M44l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M44f_(ref List<M44f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_M44d_(ref List<M44d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1b_(ref List<Range1b> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1sb_(ref List<Range1sb> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1s_(ref List<Range1s> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1us_(ref List<Range1us> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1i_(ref List<Range1i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1ui_(ref List<Range1ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1l_(ref List<Range1l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1ul_(ref List<Range1ul> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1f_(ref List<Range1f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Range1d_(ref List<Range1d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box2i_(ref List<Box2i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box2l_(ref List<Box2l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box2f_(ref List<Box2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box2d_(ref List<Box2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box3i_(ref List<Box3i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box3l_(ref List<Box3l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box3f_(ref List<Box3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Box3d_(ref List<Box3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C3b_(ref List<C3b> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C3us_(ref List<C3us> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C3ui_(ref List<C3ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C3f_(ref List<C3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C3d_(ref List<C3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C4b_(ref List<C4b> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C4us_(ref List<C4us> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C4ui_(ref List<C4ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C4f_(ref List<C4f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_C4d_(ref List<C4d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_reader.ReadList(value, 0, count);
        }

        public void CodeList_of_Euclidean3f_(ref List<Euclidean3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Euclidean3f); CodeEuclidean3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Euclidean3d_(ref List<Euclidean3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Euclidean3d); CodeEuclidean3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Rot2f_(ref List<Rot2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Rot2f); CodeRot2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Rot2d_(ref List<Rot2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Rot2d); CodeRot2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Rot3f_(ref List<Rot3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Rot3f); CodeRot3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Rot3d_(ref List<Rot3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Rot3d); CodeRot3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Scale3f_(ref List<Scale3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Scale3f); CodeScale3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Scale3d_(ref List<Scale3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Scale3d); CodeScale3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Shift3f_(ref List<Shift3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Shift3f); CodeShift3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Shift3d_(ref List<Shift3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Shift3d); CodeShift3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Trafo2f_(ref List<Trafo2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Trafo2f); CodeTrafo2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Trafo2d_(ref List<Trafo2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Trafo2d); CodeTrafo2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Trafo3f_(ref List<Trafo3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Trafo3f); CodeTrafo3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Trafo3d_(ref List<Trafo3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Trafo3d); CodeTrafo3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Circle2d_(ref List<Circle2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Circle2d); CodeCircle2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Line2d_(ref List<Line2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Line2d); CodeLine2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Line3d_(ref List<Line3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Line3d); CodeLine3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Plane2d_(ref List<Plane2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Plane2d); CodePlane2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Plane3d_(ref List<Plane3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Plane3d); CodePlane3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_PlaneWithPoint3d_(ref List<PlaneWithPoint3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(PlaneWithPoint3d); CodePlaneWithPoint3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Quad2d_(ref List<Quad2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Quad2d); CodeQuad2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Quad3d_(ref List<Quad3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Quad3d); CodeQuad3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Ray2d_(ref List<Ray2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Ray2d); CodeRay2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Ray3d_(ref List<Ray3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Ray3d); CodeRay3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Sphere3d_(ref List<Sphere3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Sphere3d); CodeSphere3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Triangle2d_(ref List<Triangle2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Triangle2d); CodeTriangle2d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Triangle3d_(ref List<Triangle3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Triangle3d); CodeTriangle3d(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Circle2f_(ref List<Circle2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Circle2f); CodeCircle2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Line2f_(ref List<Line2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Line2f); CodeLine2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Line3f_(ref List<Line3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Line3f); CodeLine3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Plane2f_(ref List<Plane2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Plane2f); CodePlane2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Plane3f_(ref List<Plane3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Plane3f); CodePlane3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_PlaneWithPoint3f_(ref List<PlaneWithPoint3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(PlaneWithPoint3f); CodePlaneWithPoint3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Quad2f_(ref List<Quad2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Quad2f); CodeQuad2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Quad3f_(ref List<Quad3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Quad3f); CodeQuad3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Ray2f_(ref List<Ray2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Ray2f); CodeRay2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Ray3f_(ref List<Ray3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Ray3f); CodeRay3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Sphere3f_(ref List<Sphere3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Sphere3f); CodeSphere3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Triangle2f_(ref List<Triangle2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Triangle2f); CodeTriangle2f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Triangle3f_(ref List<Triangle3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Triangle3f); CodeTriangle3f(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Byte__(ref List<Vector<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<byte>); CodeVector_of_Byte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_SByte__(ref List<Vector<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<sbyte>); CodeVector_of_SByte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Short__(ref List<Vector<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<short>); CodeVector_of_Short_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_UShort__(ref List<Vector<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<ushort>); CodeVector_of_UShort_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Int__(ref List<Vector<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<int>); CodeVector_of_Int_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_UInt__(ref List<Vector<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<uint>); CodeVector_of_UInt_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Long__(ref List<Vector<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<long>); CodeVector_of_Long_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_ULong__(ref List<Vector<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<ulong>); CodeVector_of_ULong_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Float__(ref List<Vector<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<float>); CodeVector_of_Float_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Double__(ref List<Vector<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<double>); CodeVector_of_Double_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Fraction__(ref List<Vector<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Fraction>); CodeVector_of_Fraction_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V2i__(ref List<Vector<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V2i>); CodeVector_of_V2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V2l__(ref List<Vector<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V2l>); CodeVector_of_V2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V2f__(ref List<Vector<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V2f>); CodeVector_of_V2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V2d__(ref List<Vector<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V2d>); CodeVector_of_V2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V3i__(ref List<Vector<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V3i>); CodeVector_of_V3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V3l__(ref List<Vector<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V3l>); CodeVector_of_V3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V3f__(ref List<Vector<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V3f>); CodeVector_of_V3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V3d__(ref List<Vector<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V3d>); CodeVector_of_V3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V4i__(ref List<Vector<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V4i>); CodeVector_of_V4i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V4l__(ref List<Vector<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V4l>); CodeVector_of_V4l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V4f__(ref List<Vector<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V4f>); CodeVector_of_V4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_V4d__(ref List<Vector<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<V4d>); CodeVector_of_V4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M22i__(ref List<Vector<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M22i>); CodeVector_of_M22i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M22l__(ref List<Vector<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M22l>); CodeVector_of_M22l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M22f__(ref List<Vector<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M22f>); CodeVector_of_M22f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M22d__(ref List<Vector<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M22d>); CodeVector_of_M22d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M23i__(ref List<Vector<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M23i>); CodeVector_of_M23i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M23l__(ref List<Vector<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M23l>); CodeVector_of_M23l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M23f__(ref List<Vector<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M23f>); CodeVector_of_M23f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M23d__(ref List<Vector<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M23d>); CodeVector_of_M23d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M33i__(ref List<Vector<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M33i>); CodeVector_of_M33i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M33l__(ref List<Vector<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M33l>); CodeVector_of_M33l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M33f__(ref List<Vector<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M33f>); CodeVector_of_M33f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M33d__(ref List<Vector<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M33d>); CodeVector_of_M33d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M34i__(ref List<Vector<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M34i>); CodeVector_of_M34i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M34l__(ref List<Vector<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M34l>); CodeVector_of_M34l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M34f__(ref List<Vector<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M34f>); CodeVector_of_M34f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M34d__(ref List<Vector<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M34d>); CodeVector_of_M34d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M44i__(ref List<Vector<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M44i>); CodeVector_of_M44i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M44l__(ref List<Vector<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M44l>); CodeVector_of_M44l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M44f__(ref List<Vector<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M44f>); CodeVector_of_M44f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_M44d__(ref List<Vector<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<M44d>); CodeVector_of_M44d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C3b__(ref List<Vector<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C3b>); CodeVector_of_C3b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C3us__(ref List<Vector<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C3us>); CodeVector_of_C3us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C3ui__(ref List<Vector<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C3ui>); CodeVector_of_C3ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C3f__(ref List<Vector<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C3f>); CodeVector_of_C3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C3d__(ref List<Vector<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C3d>); CodeVector_of_C3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C4b__(ref List<Vector<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C4b>); CodeVector_of_C4b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C4us__(ref List<Vector<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C4us>); CodeVector_of_C4us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C4ui__(ref List<Vector<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C4ui>); CodeVector_of_C4ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C4f__(ref List<Vector<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C4f>); CodeVector_of_C4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_C4d__(ref List<Vector<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<C4d>); CodeVector_of_C4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1b__(ref List<Vector<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1b>); CodeVector_of_Range1b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1sb__(ref List<Vector<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1sb>); CodeVector_of_Range1sb_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1s__(ref List<Vector<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1s>); CodeVector_of_Range1s_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1us__(ref List<Vector<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1us>); CodeVector_of_Range1us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1i__(ref List<Vector<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1i>); CodeVector_of_Range1i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1ui__(ref List<Vector<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1ui>); CodeVector_of_Range1ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1l__(ref List<Vector<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1l>); CodeVector_of_Range1l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1ul__(ref List<Vector<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1ul>); CodeVector_of_Range1ul_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1f__(ref List<Vector<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1f>); CodeVector_of_Range1f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Range1d__(ref List<Vector<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Range1d>); CodeVector_of_Range1d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box2i__(ref List<Vector<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box2i>); CodeVector_of_Box2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box2l__(ref List<Vector<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box2l>); CodeVector_of_Box2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box2f__(ref List<Vector<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box2f>); CodeVector_of_Box2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box2d__(ref List<Vector<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box2d>); CodeVector_of_Box2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box3i__(ref List<Vector<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box3i>); CodeVector_of_Box3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box3l__(ref List<Vector<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box3l>); CodeVector_of_Box3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box3f__(ref List<Vector<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box3f>); CodeVector_of_Box3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Box3d__(ref List<Vector<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Box3d>); CodeVector_of_Box3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Euclidean3f__(ref List<Vector<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Euclidean3f>); CodeVector_of_Euclidean3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Euclidean3d__(ref List<Vector<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Euclidean3d>); CodeVector_of_Euclidean3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Rot2f__(ref List<Vector<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Rot2f>); CodeVector_of_Rot2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Rot2d__(ref List<Vector<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Rot2d>); CodeVector_of_Rot2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Rot3f__(ref List<Vector<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Rot3f>); CodeVector_of_Rot3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Rot3d__(ref List<Vector<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Rot3d>); CodeVector_of_Rot3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Scale3f__(ref List<Vector<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Scale3f>); CodeVector_of_Scale3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Scale3d__(ref List<Vector<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Scale3d>); CodeVector_of_Scale3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Shift3f__(ref List<Vector<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Shift3f>); CodeVector_of_Shift3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Shift3d__(ref List<Vector<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Shift3d>); CodeVector_of_Shift3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Trafo2f__(ref List<Vector<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Trafo2f>); CodeVector_of_Trafo2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Trafo2d__(ref List<Vector<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Trafo2d>); CodeVector_of_Trafo2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Trafo3f__(ref List<Vector<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Trafo3f>); CodeVector_of_Trafo3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Trafo3d__(ref List<Vector<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Trafo3d>); CodeVector_of_Trafo3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Bool__(ref List<Vector<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<bool>); CodeVector_of_Bool_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Char__(ref List<Vector<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<char>); CodeVector_of_Char_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_String__(ref List<Vector<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<string>); CodeVector_of_String_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Type__(ref List<Vector<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Type>); CodeVector_of_Type_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Guid__(ref List<Vector<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Guid>); CodeVector_of_Guid_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Symbol__(ref List<Vector<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Symbol>); CodeVector_of_Symbol_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Circle2d__(ref List<Vector<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Circle2d>); CodeVector_of_Circle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Line2d__(ref List<Vector<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Line2d>); CodeVector_of_Line2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Line3d__(ref List<Vector<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Line3d>); CodeVector_of_Line3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Plane2d__(ref List<Vector<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Plane2d>); CodeVector_of_Plane2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Plane3d__(ref List<Vector<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Plane3d>); CodeVector_of_Plane3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_PlaneWithPoint3d__(ref List<Vector<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<PlaneWithPoint3d>); CodeVector_of_PlaneWithPoint3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Quad2d__(ref List<Vector<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Quad2d>); CodeVector_of_Quad2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Quad3d__(ref List<Vector<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Quad3d>); CodeVector_of_Quad3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Ray2d__(ref List<Vector<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Ray2d>); CodeVector_of_Ray2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Ray3d__(ref List<Vector<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Ray3d>); CodeVector_of_Ray3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Sphere3d__(ref List<Vector<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Sphere3d>); CodeVector_of_Sphere3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Triangle2d__(ref List<Vector<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Triangle2d>); CodeVector_of_Triangle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Triangle3d__(ref List<Vector<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Triangle3d>); CodeVector_of_Triangle3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Circle2f__(ref List<Vector<Circle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Circle2f>); CodeVector_of_Circle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Line2f__(ref List<Vector<Line2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Line2f>); CodeVector_of_Line2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Line3f__(ref List<Vector<Line3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Line3f>); CodeVector_of_Line3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Plane2f__(ref List<Vector<Plane2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Plane2f>); CodeVector_of_Plane2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Plane3f__(ref List<Vector<Plane3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Plane3f>); CodeVector_of_Plane3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_PlaneWithPoint3f__(ref List<Vector<PlaneWithPoint3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<PlaneWithPoint3f>); CodeVector_of_PlaneWithPoint3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Quad2f__(ref List<Vector<Quad2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Quad2f>); CodeVector_of_Quad2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Quad3f__(ref List<Vector<Quad3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Quad3f>); CodeVector_of_Quad3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Ray2f__(ref List<Vector<Ray2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Ray2f>); CodeVector_of_Ray2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Ray3f__(ref List<Vector<Ray3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Ray3f>); CodeVector_of_Ray3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Sphere3f__(ref List<Vector<Sphere3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Sphere3f>); CodeVector_of_Sphere3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Triangle2f__(ref List<Vector<Triangle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Triangle2f>); CodeVector_of_Triangle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Vector_of_Triangle3f__(ref List<Vector<Triangle3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Vector<Triangle3f>); CodeVector_of_Triangle3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Byte__(ref List<Matrix<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<byte>); CodeMatrix_of_Byte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_SByte__(ref List<Matrix<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<sbyte>); CodeMatrix_of_SByte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Short__(ref List<Matrix<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<short>); CodeMatrix_of_Short_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_UShort__(ref List<Matrix<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<ushort>); CodeMatrix_of_UShort_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Int__(ref List<Matrix<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<int>); CodeMatrix_of_Int_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_UInt__(ref List<Matrix<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<uint>); CodeMatrix_of_UInt_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Long__(ref List<Matrix<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<long>); CodeMatrix_of_Long_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_ULong__(ref List<Matrix<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<ulong>); CodeMatrix_of_ULong_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Float__(ref List<Matrix<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<float>); CodeMatrix_of_Float_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Double__(ref List<Matrix<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<double>); CodeMatrix_of_Double_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Fraction__(ref List<Matrix<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Fraction>); CodeMatrix_of_Fraction_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V2i__(ref List<Matrix<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V2i>); CodeMatrix_of_V2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V2l__(ref List<Matrix<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V2l>); CodeMatrix_of_V2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V2f__(ref List<Matrix<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V2f>); CodeMatrix_of_V2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V2d__(ref List<Matrix<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V2d>); CodeMatrix_of_V2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V3i__(ref List<Matrix<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V3i>); CodeMatrix_of_V3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V3l__(ref List<Matrix<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V3l>); CodeMatrix_of_V3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V3f__(ref List<Matrix<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V3f>); CodeMatrix_of_V3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V3d__(ref List<Matrix<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V3d>); CodeMatrix_of_V3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V4i__(ref List<Matrix<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V4i>); CodeMatrix_of_V4i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V4l__(ref List<Matrix<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V4l>); CodeMatrix_of_V4l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V4f__(ref List<Matrix<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V4f>); CodeMatrix_of_V4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_V4d__(ref List<Matrix<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<V4d>); CodeMatrix_of_V4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M22i__(ref List<Matrix<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M22i>); CodeMatrix_of_M22i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M22l__(ref List<Matrix<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M22l>); CodeMatrix_of_M22l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M22f__(ref List<Matrix<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M22f>); CodeMatrix_of_M22f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M22d__(ref List<Matrix<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M22d>); CodeMatrix_of_M22d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M23i__(ref List<Matrix<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M23i>); CodeMatrix_of_M23i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M23l__(ref List<Matrix<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M23l>); CodeMatrix_of_M23l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M23f__(ref List<Matrix<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M23f>); CodeMatrix_of_M23f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M23d__(ref List<Matrix<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M23d>); CodeMatrix_of_M23d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M33i__(ref List<Matrix<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M33i>); CodeMatrix_of_M33i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M33l__(ref List<Matrix<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M33l>); CodeMatrix_of_M33l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M33f__(ref List<Matrix<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M33f>); CodeMatrix_of_M33f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M33d__(ref List<Matrix<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M33d>); CodeMatrix_of_M33d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M34i__(ref List<Matrix<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M34i>); CodeMatrix_of_M34i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M34l__(ref List<Matrix<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M34l>); CodeMatrix_of_M34l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M34f__(ref List<Matrix<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M34f>); CodeMatrix_of_M34f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M34d__(ref List<Matrix<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M34d>); CodeMatrix_of_M34d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M44i__(ref List<Matrix<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M44i>); CodeMatrix_of_M44i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M44l__(ref List<Matrix<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M44l>); CodeMatrix_of_M44l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M44f__(ref List<Matrix<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M44f>); CodeMatrix_of_M44f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_M44d__(ref List<Matrix<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<M44d>); CodeMatrix_of_M44d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C3b__(ref List<Matrix<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C3b>); CodeMatrix_of_C3b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C3us__(ref List<Matrix<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C3us>); CodeMatrix_of_C3us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C3ui__(ref List<Matrix<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C3ui>); CodeMatrix_of_C3ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C3f__(ref List<Matrix<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C3f>); CodeMatrix_of_C3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C3d__(ref List<Matrix<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C3d>); CodeMatrix_of_C3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C4b__(ref List<Matrix<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C4b>); CodeMatrix_of_C4b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C4us__(ref List<Matrix<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C4us>); CodeMatrix_of_C4us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C4ui__(ref List<Matrix<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C4ui>); CodeMatrix_of_C4ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C4f__(ref List<Matrix<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C4f>); CodeMatrix_of_C4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_C4d__(ref List<Matrix<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<C4d>); CodeMatrix_of_C4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1b__(ref List<Matrix<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1b>); CodeMatrix_of_Range1b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1sb__(ref List<Matrix<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1sb>); CodeMatrix_of_Range1sb_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1s__(ref List<Matrix<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1s>); CodeMatrix_of_Range1s_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1us__(ref List<Matrix<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1us>); CodeMatrix_of_Range1us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1i__(ref List<Matrix<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1i>); CodeMatrix_of_Range1i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1ui__(ref List<Matrix<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1ui>); CodeMatrix_of_Range1ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1l__(ref List<Matrix<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1l>); CodeMatrix_of_Range1l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1ul__(ref List<Matrix<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1ul>); CodeMatrix_of_Range1ul_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1f__(ref List<Matrix<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1f>); CodeMatrix_of_Range1f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Range1d__(ref List<Matrix<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Range1d>); CodeMatrix_of_Range1d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box2i__(ref List<Matrix<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box2i>); CodeMatrix_of_Box2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box2l__(ref List<Matrix<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box2l>); CodeMatrix_of_Box2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box2f__(ref List<Matrix<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box2f>); CodeMatrix_of_Box2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box2d__(ref List<Matrix<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box2d>); CodeMatrix_of_Box2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box3i__(ref List<Matrix<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box3i>); CodeMatrix_of_Box3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box3l__(ref List<Matrix<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box3l>); CodeMatrix_of_Box3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box3f__(ref List<Matrix<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box3f>); CodeMatrix_of_Box3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Box3d__(ref List<Matrix<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Box3d>); CodeMatrix_of_Box3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Euclidean3f__(ref List<Matrix<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Euclidean3f>); CodeMatrix_of_Euclidean3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Euclidean3d__(ref List<Matrix<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Euclidean3d>); CodeMatrix_of_Euclidean3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Rot2f__(ref List<Matrix<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Rot2f>); CodeMatrix_of_Rot2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Rot2d__(ref List<Matrix<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Rot2d>); CodeMatrix_of_Rot2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Rot3f__(ref List<Matrix<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Rot3f>); CodeMatrix_of_Rot3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Rot3d__(ref List<Matrix<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Rot3d>); CodeMatrix_of_Rot3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Scale3f__(ref List<Matrix<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Scale3f>); CodeMatrix_of_Scale3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Scale3d__(ref List<Matrix<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Scale3d>); CodeMatrix_of_Scale3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Shift3f__(ref List<Matrix<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Shift3f>); CodeMatrix_of_Shift3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Shift3d__(ref List<Matrix<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Shift3d>); CodeMatrix_of_Shift3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Trafo2f__(ref List<Matrix<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Trafo2f>); CodeMatrix_of_Trafo2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Trafo2d__(ref List<Matrix<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Trafo2d>); CodeMatrix_of_Trafo2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Trafo3f__(ref List<Matrix<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Trafo3f>); CodeMatrix_of_Trafo3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Trafo3d__(ref List<Matrix<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Trafo3d>); CodeMatrix_of_Trafo3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Bool__(ref List<Matrix<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<bool>); CodeMatrix_of_Bool_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Char__(ref List<Matrix<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<char>); CodeMatrix_of_Char_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_String__(ref List<Matrix<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<string>); CodeMatrix_of_String_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Type__(ref List<Matrix<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Type>); CodeMatrix_of_Type_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Guid__(ref List<Matrix<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Guid>); CodeMatrix_of_Guid_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Symbol__(ref List<Matrix<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Symbol>); CodeMatrix_of_Symbol_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Circle2d__(ref List<Matrix<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Circle2d>); CodeMatrix_of_Circle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Line2d__(ref List<Matrix<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Line2d>); CodeMatrix_of_Line2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Line3d__(ref List<Matrix<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Line3d>); CodeMatrix_of_Line3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Plane2d__(ref List<Matrix<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Plane2d>); CodeMatrix_of_Plane2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Plane3d__(ref List<Matrix<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Plane3d>); CodeMatrix_of_Plane3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_PlaneWithPoint3d__(ref List<Matrix<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<PlaneWithPoint3d>); CodeMatrix_of_PlaneWithPoint3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Quad2d__(ref List<Matrix<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Quad2d>); CodeMatrix_of_Quad2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Quad3d__(ref List<Matrix<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Quad3d>); CodeMatrix_of_Quad3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Ray2d__(ref List<Matrix<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Ray2d>); CodeMatrix_of_Ray2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Ray3d__(ref List<Matrix<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Ray3d>); CodeMatrix_of_Ray3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Sphere3d__(ref List<Matrix<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Sphere3d>); CodeMatrix_of_Sphere3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Triangle2d__(ref List<Matrix<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Triangle2d>); CodeMatrix_of_Triangle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Triangle3d__(ref List<Matrix<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Triangle3d>); CodeMatrix_of_Triangle3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Circle2f__(ref List<Matrix<Circle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Circle2f>); CodeMatrix_of_Circle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Line2f__(ref List<Matrix<Line2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Line2f>); CodeMatrix_of_Line2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Line3f__(ref List<Matrix<Line3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Line3f>); CodeMatrix_of_Line3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Plane2f__(ref List<Matrix<Plane2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Plane2f>); CodeMatrix_of_Plane2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Plane3f__(ref List<Matrix<Plane3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Plane3f>); CodeMatrix_of_Plane3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_PlaneWithPoint3f__(ref List<Matrix<PlaneWithPoint3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<PlaneWithPoint3f>); CodeMatrix_of_PlaneWithPoint3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Quad2f__(ref List<Matrix<Quad2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Quad2f>); CodeMatrix_of_Quad2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Quad3f__(ref List<Matrix<Quad3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Quad3f>); CodeMatrix_of_Quad3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Ray2f__(ref List<Matrix<Ray2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Ray2f>); CodeMatrix_of_Ray2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Ray3f__(ref List<Matrix<Ray3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Ray3f>); CodeMatrix_of_Ray3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Sphere3f__(ref List<Matrix<Sphere3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Sphere3f>); CodeMatrix_of_Sphere3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Triangle2f__(ref List<Matrix<Triangle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Triangle2f>); CodeMatrix_of_Triangle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Matrix_of_Triangle3f__(ref List<Matrix<Triangle3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Matrix<Triangle3f>); CodeMatrix_of_Triangle3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Byte__(ref List<Volume<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<byte>); CodeVolume_of_Byte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_SByte__(ref List<Volume<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<sbyte>); CodeVolume_of_SByte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Short__(ref List<Volume<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<short>); CodeVolume_of_Short_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_UShort__(ref List<Volume<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<ushort>); CodeVolume_of_UShort_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Int__(ref List<Volume<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<int>); CodeVolume_of_Int_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_UInt__(ref List<Volume<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<uint>); CodeVolume_of_UInt_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Long__(ref List<Volume<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<long>); CodeVolume_of_Long_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_ULong__(ref List<Volume<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<ulong>); CodeVolume_of_ULong_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Float__(ref List<Volume<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<float>); CodeVolume_of_Float_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Double__(ref List<Volume<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<double>); CodeVolume_of_Double_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Fraction__(ref List<Volume<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Fraction>); CodeVolume_of_Fraction_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V2i__(ref List<Volume<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V2i>); CodeVolume_of_V2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V2l__(ref List<Volume<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V2l>); CodeVolume_of_V2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V2f__(ref List<Volume<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V2f>); CodeVolume_of_V2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V2d__(ref List<Volume<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V2d>); CodeVolume_of_V2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V3i__(ref List<Volume<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V3i>); CodeVolume_of_V3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V3l__(ref List<Volume<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V3l>); CodeVolume_of_V3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V3f__(ref List<Volume<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V3f>); CodeVolume_of_V3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V3d__(ref List<Volume<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V3d>); CodeVolume_of_V3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V4i__(ref List<Volume<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V4i>); CodeVolume_of_V4i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V4l__(ref List<Volume<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V4l>); CodeVolume_of_V4l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V4f__(ref List<Volume<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V4f>); CodeVolume_of_V4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_V4d__(ref List<Volume<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<V4d>); CodeVolume_of_V4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M22i__(ref List<Volume<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M22i>); CodeVolume_of_M22i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M22l__(ref List<Volume<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M22l>); CodeVolume_of_M22l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M22f__(ref List<Volume<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M22f>); CodeVolume_of_M22f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M22d__(ref List<Volume<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M22d>); CodeVolume_of_M22d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M23i__(ref List<Volume<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M23i>); CodeVolume_of_M23i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M23l__(ref List<Volume<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M23l>); CodeVolume_of_M23l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M23f__(ref List<Volume<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M23f>); CodeVolume_of_M23f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M23d__(ref List<Volume<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M23d>); CodeVolume_of_M23d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M33i__(ref List<Volume<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M33i>); CodeVolume_of_M33i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M33l__(ref List<Volume<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M33l>); CodeVolume_of_M33l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M33f__(ref List<Volume<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M33f>); CodeVolume_of_M33f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M33d__(ref List<Volume<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M33d>); CodeVolume_of_M33d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M34i__(ref List<Volume<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M34i>); CodeVolume_of_M34i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M34l__(ref List<Volume<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M34l>); CodeVolume_of_M34l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M34f__(ref List<Volume<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M34f>); CodeVolume_of_M34f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M34d__(ref List<Volume<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M34d>); CodeVolume_of_M34d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M44i__(ref List<Volume<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M44i>); CodeVolume_of_M44i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M44l__(ref List<Volume<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M44l>); CodeVolume_of_M44l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M44f__(ref List<Volume<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M44f>); CodeVolume_of_M44f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_M44d__(ref List<Volume<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<M44d>); CodeVolume_of_M44d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C3b__(ref List<Volume<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C3b>); CodeVolume_of_C3b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C3us__(ref List<Volume<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C3us>); CodeVolume_of_C3us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C3ui__(ref List<Volume<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C3ui>); CodeVolume_of_C3ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C3f__(ref List<Volume<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C3f>); CodeVolume_of_C3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C3d__(ref List<Volume<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C3d>); CodeVolume_of_C3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C4b__(ref List<Volume<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C4b>); CodeVolume_of_C4b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C4us__(ref List<Volume<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C4us>); CodeVolume_of_C4us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C4ui__(ref List<Volume<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C4ui>); CodeVolume_of_C4ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C4f__(ref List<Volume<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C4f>); CodeVolume_of_C4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_C4d__(ref List<Volume<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<C4d>); CodeVolume_of_C4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1b__(ref List<Volume<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1b>); CodeVolume_of_Range1b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1sb__(ref List<Volume<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1sb>); CodeVolume_of_Range1sb_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1s__(ref List<Volume<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1s>); CodeVolume_of_Range1s_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1us__(ref List<Volume<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1us>); CodeVolume_of_Range1us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1i__(ref List<Volume<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1i>); CodeVolume_of_Range1i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1ui__(ref List<Volume<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1ui>); CodeVolume_of_Range1ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1l__(ref List<Volume<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1l>); CodeVolume_of_Range1l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1ul__(ref List<Volume<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1ul>); CodeVolume_of_Range1ul_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1f__(ref List<Volume<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1f>); CodeVolume_of_Range1f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Range1d__(ref List<Volume<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Range1d>); CodeVolume_of_Range1d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box2i__(ref List<Volume<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box2i>); CodeVolume_of_Box2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box2l__(ref List<Volume<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box2l>); CodeVolume_of_Box2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box2f__(ref List<Volume<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box2f>); CodeVolume_of_Box2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box2d__(ref List<Volume<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box2d>); CodeVolume_of_Box2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box3i__(ref List<Volume<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box3i>); CodeVolume_of_Box3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box3l__(ref List<Volume<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box3l>); CodeVolume_of_Box3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box3f__(ref List<Volume<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box3f>); CodeVolume_of_Box3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Box3d__(ref List<Volume<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Box3d>); CodeVolume_of_Box3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Euclidean3f__(ref List<Volume<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Euclidean3f>); CodeVolume_of_Euclidean3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Euclidean3d__(ref List<Volume<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Euclidean3d>); CodeVolume_of_Euclidean3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Rot2f__(ref List<Volume<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Rot2f>); CodeVolume_of_Rot2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Rot2d__(ref List<Volume<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Rot2d>); CodeVolume_of_Rot2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Rot3f__(ref List<Volume<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Rot3f>); CodeVolume_of_Rot3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Rot3d__(ref List<Volume<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Rot3d>); CodeVolume_of_Rot3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Scale3f__(ref List<Volume<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Scale3f>); CodeVolume_of_Scale3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Scale3d__(ref List<Volume<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Scale3d>); CodeVolume_of_Scale3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Shift3f__(ref List<Volume<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Shift3f>); CodeVolume_of_Shift3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Shift3d__(ref List<Volume<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Shift3d>); CodeVolume_of_Shift3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Trafo2f__(ref List<Volume<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Trafo2f>); CodeVolume_of_Trafo2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Trafo2d__(ref List<Volume<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Trafo2d>); CodeVolume_of_Trafo2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Trafo3f__(ref List<Volume<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Trafo3f>); CodeVolume_of_Trafo3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Trafo3d__(ref List<Volume<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Trafo3d>); CodeVolume_of_Trafo3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Bool__(ref List<Volume<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<bool>); CodeVolume_of_Bool_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Char__(ref List<Volume<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<char>); CodeVolume_of_Char_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_String__(ref List<Volume<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<string>); CodeVolume_of_String_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Type__(ref List<Volume<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Type>); CodeVolume_of_Type_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Guid__(ref List<Volume<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Guid>); CodeVolume_of_Guid_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Symbol__(ref List<Volume<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Symbol>); CodeVolume_of_Symbol_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Circle2d__(ref List<Volume<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Circle2d>); CodeVolume_of_Circle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Line2d__(ref List<Volume<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Line2d>); CodeVolume_of_Line2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Line3d__(ref List<Volume<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Line3d>); CodeVolume_of_Line3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Plane2d__(ref List<Volume<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Plane2d>); CodeVolume_of_Plane2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Plane3d__(ref List<Volume<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Plane3d>); CodeVolume_of_Plane3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_PlaneWithPoint3d__(ref List<Volume<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<PlaneWithPoint3d>); CodeVolume_of_PlaneWithPoint3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Quad2d__(ref List<Volume<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Quad2d>); CodeVolume_of_Quad2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Quad3d__(ref List<Volume<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Quad3d>); CodeVolume_of_Quad3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Ray2d__(ref List<Volume<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Ray2d>); CodeVolume_of_Ray2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Ray3d__(ref List<Volume<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Ray3d>); CodeVolume_of_Ray3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Sphere3d__(ref List<Volume<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Sphere3d>); CodeVolume_of_Sphere3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Triangle2d__(ref List<Volume<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Triangle2d>); CodeVolume_of_Triangle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Triangle3d__(ref List<Volume<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Triangle3d>); CodeVolume_of_Triangle3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Circle2f__(ref List<Volume<Circle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Circle2f>); CodeVolume_of_Circle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Line2f__(ref List<Volume<Line2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Line2f>); CodeVolume_of_Line2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Line3f__(ref List<Volume<Line3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Line3f>); CodeVolume_of_Line3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Plane2f__(ref List<Volume<Plane2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Plane2f>); CodeVolume_of_Plane2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Plane3f__(ref List<Volume<Plane3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Plane3f>); CodeVolume_of_Plane3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_PlaneWithPoint3f__(ref List<Volume<PlaneWithPoint3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<PlaneWithPoint3f>); CodeVolume_of_PlaneWithPoint3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Quad2f__(ref List<Volume<Quad2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Quad2f>); CodeVolume_of_Quad2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Quad3f__(ref List<Volume<Quad3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Quad3f>); CodeVolume_of_Quad3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Ray2f__(ref List<Volume<Ray2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Ray2f>); CodeVolume_of_Ray2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Ray3f__(ref List<Volume<Ray3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Ray3f>); CodeVolume_of_Ray3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Sphere3f__(ref List<Volume<Sphere3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Sphere3f>); CodeVolume_of_Sphere3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Triangle2f__(ref List<Volume<Triangle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Triangle2f>); CodeVolume_of_Triangle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Volume_of_Triangle3f__(ref List<Volume<Triangle3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Volume<Triangle3f>); CodeVolume_of_Triangle3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Byte__(ref List<Tensor<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<byte>); CodeTensor_of_Byte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_SByte__(ref List<Tensor<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<sbyte>); CodeTensor_of_SByte_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Short__(ref List<Tensor<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<short>); CodeTensor_of_Short_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_UShort__(ref List<Tensor<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<ushort>); CodeTensor_of_UShort_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Int__(ref List<Tensor<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<int>); CodeTensor_of_Int_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_UInt__(ref List<Tensor<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<uint>); CodeTensor_of_UInt_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Long__(ref List<Tensor<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<long>); CodeTensor_of_Long_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_ULong__(ref List<Tensor<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<ulong>); CodeTensor_of_ULong_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Float__(ref List<Tensor<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<float>); CodeTensor_of_Float_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Double__(ref List<Tensor<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<double>); CodeTensor_of_Double_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Fraction__(ref List<Tensor<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Fraction>); CodeTensor_of_Fraction_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V2i__(ref List<Tensor<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V2i>); CodeTensor_of_V2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V2l__(ref List<Tensor<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V2l>); CodeTensor_of_V2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V2f__(ref List<Tensor<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V2f>); CodeTensor_of_V2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V2d__(ref List<Tensor<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V2d>); CodeTensor_of_V2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V3i__(ref List<Tensor<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V3i>); CodeTensor_of_V3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V3l__(ref List<Tensor<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V3l>); CodeTensor_of_V3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V3f__(ref List<Tensor<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V3f>); CodeTensor_of_V3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V3d__(ref List<Tensor<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V3d>); CodeTensor_of_V3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V4i__(ref List<Tensor<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V4i>); CodeTensor_of_V4i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V4l__(ref List<Tensor<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V4l>); CodeTensor_of_V4l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V4f__(ref List<Tensor<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V4f>); CodeTensor_of_V4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_V4d__(ref List<Tensor<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<V4d>); CodeTensor_of_V4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M22i__(ref List<Tensor<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M22i>); CodeTensor_of_M22i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M22l__(ref List<Tensor<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M22l>); CodeTensor_of_M22l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M22f__(ref List<Tensor<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M22f>); CodeTensor_of_M22f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M22d__(ref List<Tensor<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M22d>); CodeTensor_of_M22d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M23i__(ref List<Tensor<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M23i>); CodeTensor_of_M23i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M23l__(ref List<Tensor<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M23l>); CodeTensor_of_M23l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M23f__(ref List<Tensor<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M23f>); CodeTensor_of_M23f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M23d__(ref List<Tensor<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M23d>); CodeTensor_of_M23d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M33i__(ref List<Tensor<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M33i>); CodeTensor_of_M33i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M33l__(ref List<Tensor<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M33l>); CodeTensor_of_M33l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M33f__(ref List<Tensor<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M33f>); CodeTensor_of_M33f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M33d__(ref List<Tensor<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M33d>); CodeTensor_of_M33d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M34i__(ref List<Tensor<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M34i>); CodeTensor_of_M34i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M34l__(ref List<Tensor<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M34l>); CodeTensor_of_M34l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M34f__(ref List<Tensor<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M34f>); CodeTensor_of_M34f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M34d__(ref List<Tensor<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M34d>); CodeTensor_of_M34d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M44i__(ref List<Tensor<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M44i>); CodeTensor_of_M44i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M44l__(ref List<Tensor<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M44l>); CodeTensor_of_M44l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M44f__(ref List<Tensor<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M44f>); CodeTensor_of_M44f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_M44d__(ref List<Tensor<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<M44d>); CodeTensor_of_M44d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C3b__(ref List<Tensor<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C3b>); CodeTensor_of_C3b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C3us__(ref List<Tensor<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C3us>); CodeTensor_of_C3us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C3ui__(ref List<Tensor<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C3ui>); CodeTensor_of_C3ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C3f__(ref List<Tensor<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C3f>); CodeTensor_of_C3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C3d__(ref List<Tensor<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C3d>); CodeTensor_of_C3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C4b__(ref List<Tensor<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C4b>); CodeTensor_of_C4b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C4us__(ref List<Tensor<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C4us>); CodeTensor_of_C4us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C4ui__(ref List<Tensor<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C4ui>); CodeTensor_of_C4ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C4f__(ref List<Tensor<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C4f>); CodeTensor_of_C4f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_C4d__(ref List<Tensor<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<C4d>); CodeTensor_of_C4d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1b__(ref List<Tensor<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1b>); CodeTensor_of_Range1b_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1sb__(ref List<Tensor<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1sb>); CodeTensor_of_Range1sb_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1s__(ref List<Tensor<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1s>); CodeTensor_of_Range1s_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1us__(ref List<Tensor<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1us>); CodeTensor_of_Range1us_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1i__(ref List<Tensor<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1i>); CodeTensor_of_Range1i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1ui__(ref List<Tensor<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1ui>); CodeTensor_of_Range1ui_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1l__(ref List<Tensor<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1l>); CodeTensor_of_Range1l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1ul__(ref List<Tensor<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1ul>); CodeTensor_of_Range1ul_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1f__(ref List<Tensor<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1f>); CodeTensor_of_Range1f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Range1d__(ref List<Tensor<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Range1d>); CodeTensor_of_Range1d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box2i__(ref List<Tensor<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box2i>); CodeTensor_of_Box2i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box2l__(ref List<Tensor<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box2l>); CodeTensor_of_Box2l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box2f__(ref List<Tensor<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box2f>); CodeTensor_of_Box2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box2d__(ref List<Tensor<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box2d>); CodeTensor_of_Box2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box3i__(ref List<Tensor<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box3i>); CodeTensor_of_Box3i_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box3l__(ref List<Tensor<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box3l>); CodeTensor_of_Box3l_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box3f__(ref List<Tensor<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box3f>); CodeTensor_of_Box3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Box3d__(ref List<Tensor<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Box3d>); CodeTensor_of_Box3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Euclidean3f__(ref List<Tensor<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Euclidean3f>); CodeTensor_of_Euclidean3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Euclidean3d__(ref List<Tensor<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Euclidean3d>); CodeTensor_of_Euclidean3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Rot2f__(ref List<Tensor<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Rot2f>); CodeTensor_of_Rot2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Rot2d__(ref List<Tensor<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Rot2d>); CodeTensor_of_Rot2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Rot3f__(ref List<Tensor<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Rot3f>); CodeTensor_of_Rot3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Rot3d__(ref List<Tensor<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Rot3d>); CodeTensor_of_Rot3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Scale3f__(ref List<Tensor<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Scale3f>); CodeTensor_of_Scale3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Scale3d__(ref List<Tensor<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Scale3d>); CodeTensor_of_Scale3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Shift3f__(ref List<Tensor<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Shift3f>); CodeTensor_of_Shift3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Shift3d__(ref List<Tensor<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Shift3d>); CodeTensor_of_Shift3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Trafo2f__(ref List<Tensor<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Trafo2f>); CodeTensor_of_Trafo2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Trafo2d__(ref List<Tensor<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Trafo2d>); CodeTensor_of_Trafo2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Trafo3f__(ref List<Tensor<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Trafo3f>); CodeTensor_of_Trafo3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Trafo3d__(ref List<Tensor<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Trafo3d>); CodeTensor_of_Trafo3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Bool__(ref List<Tensor<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<bool>); CodeTensor_of_Bool_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Char__(ref List<Tensor<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<char>); CodeTensor_of_Char_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_String__(ref List<Tensor<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<string>); CodeTensor_of_String_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Type__(ref List<Tensor<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Type>); CodeTensor_of_Type_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Guid__(ref List<Tensor<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Guid>); CodeTensor_of_Guid_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Symbol__(ref List<Tensor<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Symbol>); CodeTensor_of_Symbol_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Circle2d__(ref List<Tensor<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Circle2d>); CodeTensor_of_Circle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Line2d__(ref List<Tensor<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Line2d>); CodeTensor_of_Line2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Line3d__(ref List<Tensor<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Line3d>); CodeTensor_of_Line3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Plane2d__(ref List<Tensor<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Plane2d>); CodeTensor_of_Plane2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Plane3d__(ref List<Tensor<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Plane3d>); CodeTensor_of_Plane3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_PlaneWithPoint3d__(ref List<Tensor<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<PlaneWithPoint3d>); CodeTensor_of_PlaneWithPoint3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Quad2d__(ref List<Tensor<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Quad2d>); CodeTensor_of_Quad2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Quad3d__(ref List<Tensor<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Quad3d>); CodeTensor_of_Quad3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Ray2d__(ref List<Tensor<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Ray2d>); CodeTensor_of_Ray2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Ray3d__(ref List<Tensor<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Ray3d>); CodeTensor_of_Ray3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Sphere3d__(ref List<Tensor<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Sphere3d>); CodeTensor_of_Sphere3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Triangle2d__(ref List<Tensor<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Triangle2d>); CodeTensor_of_Triangle2d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Triangle3d__(ref List<Tensor<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Triangle3d>); CodeTensor_of_Triangle3d_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Circle2f__(ref List<Tensor<Circle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Circle2f>); CodeTensor_of_Circle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Line2f__(ref List<Tensor<Line2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Line2f>); CodeTensor_of_Line2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Line3f__(ref List<Tensor<Line3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Line3f>); CodeTensor_of_Line3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Plane2f__(ref List<Tensor<Plane2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Plane2f>); CodeTensor_of_Plane2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Plane3f__(ref List<Tensor<Plane3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Plane3f>); CodeTensor_of_Plane3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_PlaneWithPoint3f__(ref List<Tensor<PlaneWithPoint3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<PlaneWithPoint3f>); CodeTensor_of_PlaneWithPoint3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Quad2f__(ref List<Tensor<Quad2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Quad2f>); CodeTensor_of_Quad2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Quad3f__(ref List<Tensor<Quad3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Quad3f>); CodeTensor_of_Quad3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Ray2f__(ref List<Tensor<Ray2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Ray2f>); CodeTensor_of_Ray2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Ray3f__(ref List<Tensor<Ray3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Ray3f>); CodeTensor_of_Ray3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Sphere3f__(ref List<Tensor<Sphere3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Sphere3f>); CodeTensor_of_Sphere3f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Triangle2f__(ref List<Tensor<Triangle2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Triangle2f>); CodeTensor_of_Triangle2f_(ref m); value.Add(m);
            }
        }

        public void CodeList_of_Tensor_of_Triangle3f__(ref List<Tensor<Triangle3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                var m = default(Tensor<Triangle3f>); CodeTensor_of_Triangle3f_(ref m); value.Add(m);
            }
        }

        #endregion
    }
}
