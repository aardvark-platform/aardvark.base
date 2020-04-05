using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Coder
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    public partial class BinaryWritingCoder
    {
        #region Vectors

        public void CodeV2i(ref V2i value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
        }

        public void CodeV2l(ref V2l value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
        }

        public void CodeV2f(ref V2f value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
        }

        public void CodeV2d(ref V2d value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
        }

        public void CodeV3i(ref V3i value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
        }

        public void CodeV3l(ref V3l value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
        }

        public void CodeV3f(ref V3f value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
        }

        public void CodeV3d(ref V3d value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
        }

        public void CodeV4i(ref V4i value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
            m_writer.Write(value.W);
        }

        public void CodeV4l(ref V4l value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
            m_writer.Write(value.W);
        }

        public void CodeV4f(ref V4f value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
            m_writer.Write(value.W);
        }

        public void CodeV4d(ref V4d value)
        {
            m_writer.Write(value.X);
            m_writer.Write(value.Y);
            m_writer.Write(value.Z);
            m_writer.Write(value.W);
        }

        #endregion

        #region Matrices

        public void CodeM22i(ref M22i value) { m_writer.Write(value); }
        public void CodeM22l(ref M22l value) { m_writer.Write(value); }
        public void CodeM22f(ref M22f value) { m_writer.Write(value); }
        public void CodeM22d(ref M22d value) { m_writer.Write(value); }
        public void CodeM23i(ref M23i value) { m_writer.Write(value); }
        public void CodeM23l(ref M23l value) { m_writer.Write(value); }
        public void CodeM23f(ref M23f value) { m_writer.Write(value); }
        public void CodeM23d(ref M23d value) { m_writer.Write(value); }
        public void CodeM33i(ref M33i value) { m_writer.Write(value); }
        public void CodeM33l(ref M33l value) { m_writer.Write(value); }
        public void CodeM33f(ref M33f value) { m_writer.Write(value); }
        public void CodeM33d(ref M33d value) { m_writer.Write(value); }
        public void CodeM34i(ref M34i value) { m_writer.Write(value); }
        public void CodeM34l(ref M34l value) { m_writer.Write(value); }
        public void CodeM34f(ref M34f value) { m_writer.Write(value); }
        public void CodeM34d(ref M34d value) { m_writer.Write(value); }
        public void CodeM44i(ref M44i value) { m_writer.Write(value); }
        public void CodeM44l(ref M44l value) { m_writer.Write(value); }
        public void CodeM44f(ref M44f value) { m_writer.Write(value); }
        public void CodeM44d(ref M44d value) { m_writer.Write(value); }

        #endregion

        #region Ranges and Boxes

        public void CodeRange1b(ref Range1b value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1sb(ref Range1sb value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1s(ref Range1s value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1us(ref Range1us value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1i(ref Range1i value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1ui(ref Range1ui value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1l(ref Range1l value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1ul(ref Range1ul value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1f(ref Range1f value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeRange1d(ref Range1d value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox2i(ref Box2i value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox2l(ref Box2l value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox2f(ref Box2f value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox2d(ref Box2d value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox3i(ref Box3i value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox3l(ref Box3l value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox3f(ref Box3f value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        public void CodeBox3d(ref Box3d value)
        {
            m_writer.Write(value.Min); m_writer.Write(value.Max);
        }

        #endregion

        #region Colors

        public void CodeC3b(ref C3b value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
        }

        public void CodeC3us(ref C3us value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
        }

        public void CodeC3ui(ref C3ui value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
        }

        public void CodeC3f(ref C3f value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
        }

        public void CodeC3d(ref C3d value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
        }

        public void CodeC4b(ref C4b value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
            m_writer.Write(value.A);
        }

        public void CodeC4us(ref C4us value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
            m_writer.Write(value.A);
        }

        public void CodeC4ui(ref C4ui value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
            m_writer.Write(value.A);
        }

        public void CodeC4f(ref C4f value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
            m_writer.Write(value.A);
        }

        public void CodeC4d(ref C4d value)
        {
            m_writer.Write(value.R);
            m_writer.Write(value.G);
            m_writer.Write(value.B);
            m_writer.Write(value.A);
        }

        #endregion

        #region Trafos

        public void CodeEuclidean3f(ref Euclidean3f value) { m_writer.Write(value); }
        public void CodeEuclidean3d(ref Euclidean3d value) { m_writer.Write(value); }
        public void CodeRot2f(ref Rot2f value) { m_writer.Write(value); }
        public void CodeRot2d(ref Rot2d value) { m_writer.Write(value); }
        public void CodeRot3f(ref Rot3f value) { m_writer.Write(value); }
        public void CodeRot3d(ref Rot3d value) { m_writer.Write(value); }
        public void CodeScale3f(ref Scale3f value) { m_writer.Write(value); }
        public void CodeScale3d(ref Scale3d value) { m_writer.Write(value); }
        public void CodeShift3f(ref Shift3f value) { m_writer.Write(value); }
        public void CodeShift3d(ref Shift3d value) { m_writer.Write(value); }
        public void CodeTrafo2f(ref Trafo2f value) { m_writer.Write(value); }
        public void CodeTrafo2d(ref Trafo2d value) { m_writer.Write(value); }
        public void CodeTrafo3f(ref Trafo3f value) { m_writer.Write(value); }
        public void CodeTrafo3d(ref Trafo3d value) { m_writer.Write(value); }

        #endregion

        #region Tensors

        public void CodeVector_of_Byte_(ref Vector<byte> value)
        {
            var data = value.Data; CodeByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_SByte_(ref Vector<sbyte> value)
        {
            var data = value.Data; CodeSByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Short_(ref Vector<short> value)
        {
            var data = value.Data; CodeShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_UShort_(ref Vector<ushort> value)
        {
            var data = value.Data; CodeUShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Int_(ref Vector<int> value)
        {
            var data = value.Data; CodeIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_UInt_(ref Vector<uint> value)
        {
            var data = value.Data; CodeUIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Long_(ref Vector<long> value)
        {
            var data = value.Data; CodeLongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_ULong_(ref Vector<ulong> value)
        {
            var data = value.Data; CodeULongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Float_(ref Vector<float> value)
        {
            var data = value.Data; CodeFloatArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Double_(ref Vector<double> value)
        {
            var data = value.Data; CodeDoubleArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Fraction_(ref Vector<Fraction> value)
        {
            var data = value.Data; CodeFractionArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V2i_(ref Vector<V2i> value)
        {
            var data = value.Data; CodeV2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V2l_(ref Vector<V2l> value)
        {
            var data = value.Data; CodeV2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V2f_(ref Vector<V2f> value)
        {
            var data = value.Data; CodeV2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V2d_(ref Vector<V2d> value)
        {
            var data = value.Data; CodeV2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V3i_(ref Vector<V3i> value)
        {
            var data = value.Data; CodeV3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V3l_(ref Vector<V3l> value)
        {
            var data = value.Data; CodeV3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V3f_(ref Vector<V3f> value)
        {
            var data = value.Data; CodeV3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V3d_(ref Vector<V3d> value)
        {
            var data = value.Data; CodeV3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V4i_(ref Vector<V4i> value)
        {
            var data = value.Data; CodeV4iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V4l_(ref Vector<V4l> value)
        {
            var data = value.Data; CodeV4lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V4f_(ref Vector<V4f> value)
        {
            var data = value.Data; CodeV4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_V4d_(ref Vector<V4d> value)
        {
            var data = value.Data; CodeV4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M22i_(ref Vector<M22i> value)
        {
            var data = value.Data; CodeM22iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M22l_(ref Vector<M22l> value)
        {
            var data = value.Data; CodeM22lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M22f_(ref Vector<M22f> value)
        {
            var data = value.Data; CodeM22fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M22d_(ref Vector<M22d> value)
        {
            var data = value.Data; CodeM22dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M23i_(ref Vector<M23i> value)
        {
            var data = value.Data; CodeM23iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M23l_(ref Vector<M23l> value)
        {
            var data = value.Data; CodeM23lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M23f_(ref Vector<M23f> value)
        {
            var data = value.Data; CodeM23fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M23d_(ref Vector<M23d> value)
        {
            var data = value.Data; CodeM23dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M33i_(ref Vector<M33i> value)
        {
            var data = value.Data; CodeM33iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M33l_(ref Vector<M33l> value)
        {
            var data = value.Data; CodeM33lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M33f_(ref Vector<M33f> value)
        {
            var data = value.Data; CodeM33fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M33d_(ref Vector<M33d> value)
        {
            var data = value.Data; CodeM33dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M34i_(ref Vector<M34i> value)
        {
            var data = value.Data; CodeM34iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M34l_(ref Vector<M34l> value)
        {
            var data = value.Data; CodeM34lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M34f_(ref Vector<M34f> value)
        {
            var data = value.Data; CodeM34fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M34d_(ref Vector<M34d> value)
        {
            var data = value.Data; CodeM34dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M44i_(ref Vector<M44i> value)
        {
            var data = value.Data; CodeM44iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M44l_(ref Vector<M44l> value)
        {
            var data = value.Data; CodeM44lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M44f_(ref Vector<M44f> value)
        {
            var data = value.Data; CodeM44fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_M44d_(ref Vector<M44d> value)
        {
            var data = value.Data; CodeM44dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C3b_(ref Vector<C3b> value)
        {
            var data = value.Data; CodeC3bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C3us_(ref Vector<C3us> value)
        {
            var data = value.Data; CodeC3usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C3ui_(ref Vector<C3ui> value)
        {
            var data = value.Data; CodeC3uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C3f_(ref Vector<C3f> value)
        {
            var data = value.Data; CodeC3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C3d_(ref Vector<C3d> value)
        {
            var data = value.Data; CodeC3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C4b_(ref Vector<C4b> value)
        {
            var data = value.Data; CodeC4bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C4us_(ref Vector<C4us> value)
        {
            var data = value.Data; CodeC4usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C4ui_(ref Vector<C4ui> value)
        {
            var data = value.Data; CodeC4uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C4f_(ref Vector<C4f> value)
        {
            var data = value.Data; CodeC4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_C4d_(ref Vector<C4d> value)
        {
            var data = value.Data; CodeC4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1b_(ref Vector<Range1b> value)
        {
            var data = value.Data; CodeRange1bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1sb_(ref Vector<Range1sb> value)
        {
            var data = value.Data; CodeRange1sbArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1s_(ref Vector<Range1s> value)
        {
            var data = value.Data; CodeRange1sArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1us_(ref Vector<Range1us> value)
        {
            var data = value.Data; CodeRange1usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1i_(ref Vector<Range1i> value)
        {
            var data = value.Data; CodeRange1iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1ui_(ref Vector<Range1ui> value)
        {
            var data = value.Data; CodeRange1uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1l_(ref Vector<Range1l> value)
        {
            var data = value.Data; CodeRange1lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1ul_(ref Vector<Range1ul> value)
        {
            var data = value.Data; CodeRange1ulArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1f_(ref Vector<Range1f> value)
        {
            var data = value.Data; CodeRange1fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Range1d_(ref Vector<Range1d> value)
        {
            var data = value.Data; CodeRange1dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box2i_(ref Vector<Box2i> value)
        {
            var data = value.Data; CodeBox2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box2l_(ref Vector<Box2l> value)
        {
            var data = value.Data; CodeBox2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box2f_(ref Vector<Box2f> value)
        {
            var data = value.Data; CodeBox2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box2d_(ref Vector<Box2d> value)
        {
            var data = value.Data; CodeBox2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box3i_(ref Vector<Box3i> value)
        {
            var data = value.Data; CodeBox3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box3l_(ref Vector<Box3l> value)
        {
            var data = value.Data; CodeBox3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box3f_(ref Vector<Box3f> value)
        {
            var data = value.Data; CodeBox3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Box3d_(ref Vector<Box3d> value)
        {
            var data = value.Data; CodeBox3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Euclidean3f_(ref Vector<Euclidean3f> value)
        {
            var data = value.Data; CodeEuclidean3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Euclidean3d_(ref Vector<Euclidean3d> value)
        {
            var data = value.Data; CodeEuclidean3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Rot2f_(ref Vector<Rot2f> value)
        {
            var data = value.Data; CodeRot2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Rot2d_(ref Vector<Rot2d> value)
        {
            var data = value.Data; CodeRot2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Rot3f_(ref Vector<Rot3f> value)
        {
            var data = value.Data; CodeRot3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Rot3d_(ref Vector<Rot3d> value)
        {
            var data = value.Data; CodeRot3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Scale3f_(ref Vector<Scale3f> value)
        {
            var data = value.Data; CodeScale3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Scale3d_(ref Vector<Scale3d> value)
        {
            var data = value.Data; CodeScale3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Shift3f_(ref Vector<Shift3f> value)
        {
            var data = value.Data; CodeShift3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Shift3d_(ref Vector<Shift3d> value)
        {
            var data = value.Data; CodeShift3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Trafo2f_(ref Vector<Trafo2f> value)
        {
            var data = value.Data; CodeTrafo2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Trafo2d_(ref Vector<Trafo2d> value)
        {
            var data = value.Data; CodeTrafo2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Trafo3f_(ref Vector<Trafo3f> value)
        {
            var data = value.Data; CodeTrafo3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Trafo3d_(ref Vector<Trafo3d> value)
        {
            var data = value.Data; CodeTrafo3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Bool_(ref Vector<bool> value)
        {
            var data = value.Data; CodeBoolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Char_(ref Vector<char> value)
        {
            var data = value.Data; CodeCharArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_String_(ref Vector<string> value)
        {
            var data = value.Data; CodeStringArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Type_(ref Vector<Type> value)
        {
            var data = value.Data; CodeTypeArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Guid_(ref Vector<Guid> value)
        {
            var data = value.Data; CodeGuidArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Symbol_(ref Vector<Symbol> value)
        {
            var data = value.Data; CodeSymbolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Circle2d_(ref Vector<Circle2d> value)
        {
            var data = value.Data; CodeCircle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Line2d_(ref Vector<Line2d> value)
        {
            var data = value.Data; CodeLine2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Line3d_(ref Vector<Line3d> value)
        {
            var data = value.Data; CodeLine3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Plane2d_(ref Vector<Plane2d> value)
        {
            var data = value.Data; CodePlane2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Plane3d_(ref Vector<Plane3d> value)
        {
            var data = value.Data; CodePlane3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_PlaneWithPoint3d_(ref Vector<PlaneWithPoint3d> value)
        {
            var data = value.Data; CodePlaneWithPoint3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Quad2d_(ref Vector<Quad2d> value)
        {
            var data = value.Data; CodeQuad2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Quad3d_(ref Vector<Quad3d> value)
        {
            var data = value.Data; CodeQuad3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Ray2d_(ref Vector<Ray2d> value)
        {
            var data = value.Data; CodeRay2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Ray3d_(ref Vector<Ray3d> value)
        {
            var data = value.Data; CodeRay3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Sphere3d_(ref Vector<Sphere3d> value)
        {
            var data = value.Data; CodeSphere3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Triangle2d_(ref Vector<Triangle2d> value)
        {
            var data = value.Data; CodeTriangle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeVector_of_Triangle3d_(ref Vector<Triangle3d> value)
        {
            var data = value.Data; CodeTriangle3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLong(ref size);
            var delta = value.Delta; CodeLong(ref delta);
        }

        public void CodeMatrix_of_Byte_(ref Matrix<byte> value)
        {
            var data = value.Data; CodeByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_SByte_(ref Matrix<sbyte> value)
        {
            var data = value.Data; CodeSByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Short_(ref Matrix<short> value)
        {
            var data = value.Data; CodeShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_UShort_(ref Matrix<ushort> value)
        {
            var data = value.Data; CodeUShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Int_(ref Matrix<int> value)
        {
            var data = value.Data; CodeIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_UInt_(ref Matrix<uint> value)
        {
            var data = value.Data; CodeUIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Long_(ref Matrix<long> value)
        {
            var data = value.Data; CodeLongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_ULong_(ref Matrix<ulong> value)
        {
            var data = value.Data; CodeULongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Float_(ref Matrix<float> value)
        {
            var data = value.Data; CodeFloatArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Double_(ref Matrix<double> value)
        {
            var data = value.Data; CodeDoubleArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Fraction_(ref Matrix<Fraction> value)
        {
            var data = value.Data; CodeFractionArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V2i_(ref Matrix<V2i> value)
        {
            var data = value.Data; CodeV2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V2l_(ref Matrix<V2l> value)
        {
            var data = value.Data; CodeV2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V2f_(ref Matrix<V2f> value)
        {
            var data = value.Data; CodeV2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V2d_(ref Matrix<V2d> value)
        {
            var data = value.Data; CodeV2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V3i_(ref Matrix<V3i> value)
        {
            var data = value.Data; CodeV3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V3l_(ref Matrix<V3l> value)
        {
            var data = value.Data; CodeV3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V3f_(ref Matrix<V3f> value)
        {
            var data = value.Data; CodeV3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V3d_(ref Matrix<V3d> value)
        {
            var data = value.Data; CodeV3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V4i_(ref Matrix<V4i> value)
        {
            var data = value.Data; CodeV4iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V4l_(ref Matrix<V4l> value)
        {
            var data = value.Data; CodeV4lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V4f_(ref Matrix<V4f> value)
        {
            var data = value.Data; CodeV4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_V4d_(ref Matrix<V4d> value)
        {
            var data = value.Data; CodeV4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M22i_(ref Matrix<M22i> value)
        {
            var data = value.Data; CodeM22iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M22l_(ref Matrix<M22l> value)
        {
            var data = value.Data; CodeM22lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M22f_(ref Matrix<M22f> value)
        {
            var data = value.Data; CodeM22fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M22d_(ref Matrix<M22d> value)
        {
            var data = value.Data; CodeM22dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M23i_(ref Matrix<M23i> value)
        {
            var data = value.Data; CodeM23iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M23l_(ref Matrix<M23l> value)
        {
            var data = value.Data; CodeM23lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M23f_(ref Matrix<M23f> value)
        {
            var data = value.Data; CodeM23fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M23d_(ref Matrix<M23d> value)
        {
            var data = value.Data; CodeM23dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M33i_(ref Matrix<M33i> value)
        {
            var data = value.Data; CodeM33iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M33l_(ref Matrix<M33l> value)
        {
            var data = value.Data; CodeM33lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M33f_(ref Matrix<M33f> value)
        {
            var data = value.Data; CodeM33fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M33d_(ref Matrix<M33d> value)
        {
            var data = value.Data; CodeM33dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M34i_(ref Matrix<M34i> value)
        {
            var data = value.Data; CodeM34iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M34l_(ref Matrix<M34l> value)
        {
            var data = value.Data; CodeM34lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M34f_(ref Matrix<M34f> value)
        {
            var data = value.Data; CodeM34fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M34d_(ref Matrix<M34d> value)
        {
            var data = value.Data; CodeM34dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M44i_(ref Matrix<M44i> value)
        {
            var data = value.Data; CodeM44iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M44l_(ref Matrix<M44l> value)
        {
            var data = value.Data; CodeM44lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M44f_(ref Matrix<M44f> value)
        {
            var data = value.Data; CodeM44fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_M44d_(ref Matrix<M44d> value)
        {
            var data = value.Data; CodeM44dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C3b_(ref Matrix<C3b> value)
        {
            var data = value.Data; CodeC3bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C3us_(ref Matrix<C3us> value)
        {
            var data = value.Data; CodeC3usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C3ui_(ref Matrix<C3ui> value)
        {
            var data = value.Data; CodeC3uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C3f_(ref Matrix<C3f> value)
        {
            var data = value.Data; CodeC3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C3d_(ref Matrix<C3d> value)
        {
            var data = value.Data; CodeC3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C4b_(ref Matrix<C4b> value)
        {
            var data = value.Data; CodeC4bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C4us_(ref Matrix<C4us> value)
        {
            var data = value.Data; CodeC4usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C4ui_(ref Matrix<C4ui> value)
        {
            var data = value.Data; CodeC4uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C4f_(ref Matrix<C4f> value)
        {
            var data = value.Data; CodeC4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_C4d_(ref Matrix<C4d> value)
        {
            var data = value.Data; CodeC4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1b_(ref Matrix<Range1b> value)
        {
            var data = value.Data; CodeRange1bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1sb_(ref Matrix<Range1sb> value)
        {
            var data = value.Data; CodeRange1sbArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1s_(ref Matrix<Range1s> value)
        {
            var data = value.Data; CodeRange1sArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1us_(ref Matrix<Range1us> value)
        {
            var data = value.Data; CodeRange1usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1i_(ref Matrix<Range1i> value)
        {
            var data = value.Data; CodeRange1iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1ui_(ref Matrix<Range1ui> value)
        {
            var data = value.Data; CodeRange1uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1l_(ref Matrix<Range1l> value)
        {
            var data = value.Data; CodeRange1lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1ul_(ref Matrix<Range1ul> value)
        {
            var data = value.Data; CodeRange1ulArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1f_(ref Matrix<Range1f> value)
        {
            var data = value.Data; CodeRange1fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Range1d_(ref Matrix<Range1d> value)
        {
            var data = value.Data; CodeRange1dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box2i_(ref Matrix<Box2i> value)
        {
            var data = value.Data; CodeBox2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box2l_(ref Matrix<Box2l> value)
        {
            var data = value.Data; CodeBox2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box2f_(ref Matrix<Box2f> value)
        {
            var data = value.Data; CodeBox2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box2d_(ref Matrix<Box2d> value)
        {
            var data = value.Data; CodeBox2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box3i_(ref Matrix<Box3i> value)
        {
            var data = value.Data; CodeBox3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box3l_(ref Matrix<Box3l> value)
        {
            var data = value.Data; CodeBox3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box3f_(ref Matrix<Box3f> value)
        {
            var data = value.Data; CodeBox3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Box3d_(ref Matrix<Box3d> value)
        {
            var data = value.Data; CodeBox3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Euclidean3f_(ref Matrix<Euclidean3f> value)
        {
            var data = value.Data; CodeEuclidean3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Euclidean3d_(ref Matrix<Euclidean3d> value)
        {
            var data = value.Data; CodeEuclidean3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Rot2f_(ref Matrix<Rot2f> value)
        {
            var data = value.Data; CodeRot2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Rot2d_(ref Matrix<Rot2d> value)
        {
            var data = value.Data; CodeRot2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Rot3f_(ref Matrix<Rot3f> value)
        {
            var data = value.Data; CodeRot3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Rot3d_(ref Matrix<Rot3d> value)
        {
            var data = value.Data; CodeRot3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Scale3f_(ref Matrix<Scale3f> value)
        {
            var data = value.Data; CodeScale3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Scale3d_(ref Matrix<Scale3d> value)
        {
            var data = value.Data; CodeScale3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Shift3f_(ref Matrix<Shift3f> value)
        {
            var data = value.Data; CodeShift3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Shift3d_(ref Matrix<Shift3d> value)
        {
            var data = value.Data; CodeShift3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Trafo2f_(ref Matrix<Trafo2f> value)
        {
            var data = value.Data; CodeTrafo2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Trafo2d_(ref Matrix<Trafo2d> value)
        {
            var data = value.Data; CodeTrafo2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Trafo3f_(ref Matrix<Trafo3f> value)
        {
            var data = value.Data; CodeTrafo3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Trafo3d_(ref Matrix<Trafo3d> value)
        {
            var data = value.Data; CodeTrafo3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Bool_(ref Matrix<bool> value)
        {
            var data = value.Data; CodeBoolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Char_(ref Matrix<char> value)
        {
            var data = value.Data; CodeCharArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_String_(ref Matrix<string> value)
        {
            var data = value.Data; CodeStringArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Type_(ref Matrix<Type> value)
        {
            var data = value.Data; CodeTypeArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Guid_(ref Matrix<Guid> value)
        {
            var data = value.Data; CodeGuidArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Symbol_(ref Matrix<Symbol> value)
        {
            var data = value.Data; CodeSymbolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Circle2d_(ref Matrix<Circle2d> value)
        {
            var data = value.Data; CodeCircle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Line2d_(ref Matrix<Line2d> value)
        {
            var data = value.Data; CodeLine2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Line3d_(ref Matrix<Line3d> value)
        {
            var data = value.Data; CodeLine3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Plane2d_(ref Matrix<Plane2d> value)
        {
            var data = value.Data; CodePlane2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Plane3d_(ref Matrix<Plane3d> value)
        {
            var data = value.Data; CodePlane3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_PlaneWithPoint3d_(ref Matrix<PlaneWithPoint3d> value)
        {
            var data = value.Data; CodePlaneWithPoint3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Quad2d_(ref Matrix<Quad2d> value)
        {
            var data = value.Data; CodeQuad2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Quad3d_(ref Matrix<Quad3d> value)
        {
            var data = value.Data; CodeQuad3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Ray2d_(ref Matrix<Ray2d> value)
        {
            var data = value.Data; CodeRay2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Ray3d_(ref Matrix<Ray3d> value)
        {
            var data = value.Data; CodeRay3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Sphere3d_(ref Matrix<Sphere3d> value)
        {
            var data = value.Data; CodeSphere3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Triangle2d_(ref Matrix<Triangle2d> value)
        {
            var data = value.Data; CodeTriangle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeMatrix_of_Triangle3d_(ref Matrix<Triangle3d> value)
        {
            var data = value.Data; CodeTriangle3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV2l(ref size);
            var delta = value.Delta; CodeV2l(ref delta);
        }

        public void CodeVolume_of_Byte_(ref Volume<byte> value)
        {
            var data = value.Data; CodeByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_SByte_(ref Volume<sbyte> value)
        {
            var data = value.Data; CodeSByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Short_(ref Volume<short> value)
        {
            var data = value.Data; CodeShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_UShort_(ref Volume<ushort> value)
        {
            var data = value.Data; CodeUShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Int_(ref Volume<int> value)
        {
            var data = value.Data; CodeIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_UInt_(ref Volume<uint> value)
        {
            var data = value.Data; CodeUIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Long_(ref Volume<long> value)
        {
            var data = value.Data; CodeLongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_ULong_(ref Volume<ulong> value)
        {
            var data = value.Data; CodeULongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Float_(ref Volume<float> value)
        {
            var data = value.Data; CodeFloatArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Double_(ref Volume<double> value)
        {
            var data = value.Data; CodeDoubleArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Fraction_(ref Volume<Fraction> value)
        {
            var data = value.Data; CodeFractionArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V2i_(ref Volume<V2i> value)
        {
            var data = value.Data; CodeV2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V2l_(ref Volume<V2l> value)
        {
            var data = value.Data; CodeV2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V2f_(ref Volume<V2f> value)
        {
            var data = value.Data; CodeV2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V2d_(ref Volume<V2d> value)
        {
            var data = value.Data; CodeV2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V3i_(ref Volume<V3i> value)
        {
            var data = value.Data; CodeV3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V3l_(ref Volume<V3l> value)
        {
            var data = value.Data; CodeV3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V3f_(ref Volume<V3f> value)
        {
            var data = value.Data; CodeV3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V3d_(ref Volume<V3d> value)
        {
            var data = value.Data; CodeV3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V4i_(ref Volume<V4i> value)
        {
            var data = value.Data; CodeV4iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V4l_(ref Volume<V4l> value)
        {
            var data = value.Data; CodeV4lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V4f_(ref Volume<V4f> value)
        {
            var data = value.Data; CodeV4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_V4d_(ref Volume<V4d> value)
        {
            var data = value.Data; CodeV4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M22i_(ref Volume<M22i> value)
        {
            var data = value.Data; CodeM22iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M22l_(ref Volume<M22l> value)
        {
            var data = value.Data; CodeM22lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M22f_(ref Volume<M22f> value)
        {
            var data = value.Data; CodeM22fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M22d_(ref Volume<M22d> value)
        {
            var data = value.Data; CodeM22dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M23i_(ref Volume<M23i> value)
        {
            var data = value.Data; CodeM23iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M23l_(ref Volume<M23l> value)
        {
            var data = value.Data; CodeM23lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M23f_(ref Volume<M23f> value)
        {
            var data = value.Data; CodeM23fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M23d_(ref Volume<M23d> value)
        {
            var data = value.Data; CodeM23dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M33i_(ref Volume<M33i> value)
        {
            var data = value.Data; CodeM33iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M33l_(ref Volume<M33l> value)
        {
            var data = value.Data; CodeM33lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M33f_(ref Volume<M33f> value)
        {
            var data = value.Data; CodeM33fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M33d_(ref Volume<M33d> value)
        {
            var data = value.Data; CodeM33dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M34i_(ref Volume<M34i> value)
        {
            var data = value.Data; CodeM34iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M34l_(ref Volume<M34l> value)
        {
            var data = value.Data; CodeM34lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M34f_(ref Volume<M34f> value)
        {
            var data = value.Data; CodeM34fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M34d_(ref Volume<M34d> value)
        {
            var data = value.Data; CodeM34dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M44i_(ref Volume<M44i> value)
        {
            var data = value.Data; CodeM44iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M44l_(ref Volume<M44l> value)
        {
            var data = value.Data; CodeM44lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M44f_(ref Volume<M44f> value)
        {
            var data = value.Data; CodeM44fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_M44d_(ref Volume<M44d> value)
        {
            var data = value.Data; CodeM44dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C3b_(ref Volume<C3b> value)
        {
            var data = value.Data; CodeC3bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C3us_(ref Volume<C3us> value)
        {
            var data = value.Data; CodeC3usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C3ui_(ref Volume<C3ui> value)
        {
            var data = value.Data; CodeC3uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C3f_(ref Volume<C3f> value)
        {
            var data = value.Data; CodeC3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C3d_(ref Volume<C3d> value)
        {
            var data = value.Data; CodeC3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C4b_(ref Volume<C4b> value)
        {
            var data = value.Data; CodeC4bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C4us_(ref Volume<C4us> value)
        {
            var data = value.Data; CodeC4usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C4ui_(ref Volume<C4ui> value)
        {
            var data = value.Data; CodeC4uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C4f_(ref Volume<C4f> value)
        {
            var data = value.Data; CodeC4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_C4d_(ref Volume<C4d> value)
        {
            var data = value.Data; CodeC4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1b_(ref Volume<Range1b> value)
        {
            var data = value.Data; CodeRange1bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1sb_(ref Volume<Range1sb> value)
        {
            var data = value.Data; CodeRange1sbArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1s_(ref Volume<Range1s> value)
        {
            var data = value.Data; CodeRange1sArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1us_(ref Volume<Range1us> value)
        {
            var data = value.Data; CodeRange1usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1i_(ref Volume<Range1i> value)
        {
            var data = value.Data; CodeRange1iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1ui_(ref Volume<Range1ui> value)
        {
            var data = value.Data; CodeRange1uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1l_(ref Volume<Range1l> value)
        {
            var data = value.Data; CodeRange1lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1ul_(ref Volume<Range1ul> value)
        {
            var data = value.Data; CodeRange1ulArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1f_(ref Volume<Range1f> value)
        {
            var data = value.Data; CodeRange1fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Range1d_(ref Volume<Range1d> value)
        {
            var data = value.Data; CodeRange1dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box2i_(ref Volume<Box2i> value)
        {
            var data = value.Data; CodeBox2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box2l_(ref Volume<Box2l> value)
        {
            var data = value.Data; CodeBox2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box2f_(ref Volume<Box2f> value)
        {
            var data = value.Data; CodeBox2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box2d_(ref Volume<Box2d> value)
        {
            var data = value.Data; CodeBox2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box3i_(ref Volume<Box3i> value)
        {
            var data = value.Data; CodeBox3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box3l_(ref Volume<Box3l> value)
        {
            var data = value.Data; CodeBox3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box3f_(ref Volume<Box3f> value)
        {
            var data = value.Data; CodeBox3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Box3d_(ref Volume<Box3d> value)
        {
            var data = value.Data; CodeBox3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Euclidean3f_(ref Volume<Euclidean3f> value)
        {
            var data = value.Data; CodeEuclidean3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Euclidean3d_(ref Volume<Euclidean3d> value)
        {
            var data = value.Data; CodeEuclidean3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Rot2f_(ref Volume<Rot2f> value)
        {
            var data = value.Data; CodeRot2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Rot2d_(ref Volume<Rot2d> value)
        {
            var data = value.Data; CodeRot2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Rot3f_(ref Volume<Rot3f> value)
        {
            var data = value.Data; CodeRot3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Rot3d_(ref Volume<Rot3d> value)
        {
            var data = value.Data; CodeRot3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Scale3f_(ref Volume<Scale3f> value)
        {
            var data = value.Data; CodeScale3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Scale3d_(ref Volume<Scale3d> value)
        {
            var data = value.Data; CodeScale3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Shift3f_(ref Volume<Shift3f> value)
        {
            var data = value.Data; CodeShift3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Shift3d_(ref Volume<Shift3d> value)
        {
            var data = value.Data; CodeShift3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Trafo2f_(ref Volume<Trafo2f> value)
        {
            var data = value.Data; CodeTrafo2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Trafo2d_(ref Volume<Trafo2d> value)
        {
            var data = value.Data; CodeTrafo2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Trafo3f_(ref Volume<Trafo3f> value)
        {
            var data = value.Data; CodeTrafo3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Trafo3d_(ref Volume<Trafo3d> value)
        {
            var data = value.Data; CodeTrafo3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Bool_(ref Volume<bool> value)
        {
            var data = value.Data; CodeBoolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Char_(ref Volume<char> value)
        {
            var data = value.Data; CodeCharArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_String_(ref Volume<string> value)
        {
            var data = value.Data; CodeStringArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Type_(ref Volume<Type> value)
        {
            var data = value.Data; CodeTypeArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Guid_(ref Volume<Guid> value)
        {
            var data = value.Data; CodeGuidArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Symbol_(ref Volume<Symbol> value)
        {
            var data = value.Data; CodeSymbolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Circle2d_(ref Volume<Circle2d> value)
        {
            var data = value.Data; CodeCircle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Line2d_(ref Volume<Line2d> value)
        {
            var data = value.Data; CodeLine2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Line3d_(ref Volume<Line3d> value)
        {
            var data = value.Data; CodeLine3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Plane2d_(ref Volume<Plane2d> value)
        {
            var data = value.Data; CodePlane2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Plane3d_(ref Volume<Plane3d> value)
        {
            var data = value.Data; CodePlane3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_PlaneWithPoint3d_(ref Volume<PlaneWithPoint3d> value)
        {
            var data = value.Data; CodePlaneWithPoint3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Quad2d_(ref Volume<Quad2d> value)
        {
            var data = value.Data; CodeQuad2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Quad3d_(ref Volume<Quad3d> value)
        {
            var data = value.Data; CodeQuad3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Ray2d_(ref Volume<Ray2d> value)
        {
            var data = value.Data; CodeRay2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Ray3d_(ref Volume<Ray3d> value)
        {
            var data = value.Data; CodeRay3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Sphere3d_(ref Volume<Sphere3d> value)
        {
            var data = value.Data; CodeSphere3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Triangle2d_(ref Volume<Triangle2d> value)
        {
            var data = value.Data; CodeTriangle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeVolume_of_Triangle3d_(ref Volume<Triangle3d> value)
        {
            var data = value.Data; CodeTriangle3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeV3l(ref size);
            var delta = value.Delta; CodeV3l(ref delta);
        }

        public void CodeTensor_of_Byte_(ref Tensor<byte> value)
        {
            var data = value.Data; CodeByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_SByte_(ref Tensor<sbyte> value)
        {
            var data = value.Data; CodeSByteArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Short_(ref Tensor<short> value)
        {
            var data = value.Data; CodeShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_UShort_(ref Tensor<ushort> value)
        {
            var data = value.Data; CodeUShortArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Int_(ref Tensor<int> value)
        {
            var data = value.Data; CodeIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_UInt_(ref Tensor<uint> value)
        {
            var data = value.Data; CodeUIntArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Long_(ref Tensor<long> value)
        {
            var data = value.Data; CodeLongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_ULong_(ref Tensor<ulong> value)
        {
            var data = value.Data; CodeULongArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Float_(ref Tensor<float> value)
        {
            var data = value.Data; CodeFloatArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Double_(ref Tensor<double> value)
        {
            var data = value.Data; CodeDoubleArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Fraction_(ref Tensor<Fraction> value)
        {
            var data = value.Data; CodeFractionArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V2i_(ref Tensor<V2i> value)
        {
            var data = value.Data; CodeV2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V2l_(ref Tensor<V2l> value)
        {
            var data = value.Data; CodeV2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V2f_(ref Tensor<V2f> value)
        {
            var data = value.Data; CodeV2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V2d_(ref Tensor<V2d> value)
        {
            var data = value.Data; CodeV2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V3i_(ref Tensor<V3i> value)
        {
            var data = value.Data; CodeV3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V3l_(ref Tensor<V3l> value)
        {
            var data = value.Data; CodeV3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V3f_(ref Tensor<V3f> value)
        {
            var data = value.Data; CodeV3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V3d_(ref Tensor<V3d> value)
        {
            var data = value.Data; CodeV3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V4i_(ref Tensor<V4i> value)
        {
            var data = value.Data; CodeV4iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V4l_(ref Tensor<V4l> value)
        {
            var data = value.Data; CodeV4lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V4f_(ref Tensor<V4f> value)
        {
            var data = value.Data; CodeV4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_V4d_(ref Tensor<V4d> value)
        {
            var data = value.Data; CodeV4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M22i_(ref Tensor<M22i> value)
        {
            var data = value.Data; CodeM22iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M22l_(ref Tensor<M22l> value)
        {
            var data = value.Data; CodeM22lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M22f_(ref Tensor<M22f> value)
        {
            var data = value.Data; CodeM22fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M22d_(ref Tensor<M22d> value)
        {
            var data = value.Data; CodeM22dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M23i_(ref Tensor<M23i> value)
        {
            var data = value.Data; CodeM23iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M23l_(ref Tensor<M23l> value)
        {
            var data = value.Data; CodeM23lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M23f_(ref Tensor<M23f> value)
        {
            var data = value.Data; CodeM23fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M23d_(ref Tensor<M23d> value)
        {
            var data = value.Data; CodeM23dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M33i_(ref Tensor<M33i> value)
        {
            var data = value.Data; CodeM33iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M33l_(ref Tensor<M33l> value)
        {
            var data = value.Data; CodeM33lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M33f_(ref Tensor<M33f> value)
        {
            var data = value.Data; CodeM33fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M33d_(ref Tensor<M33d> value)
        {
            var data = value.Data; CodeM33dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M34i_(ref Tensor<M34i> value)
        {
            var data = value.Data; CodeM34iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M34l_(ref Tensor<M34l> value)
        {
            var data = value.Data; CodeM34lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M34f_(ref Tensor<M34f> value)
        {
            var data = value.Data; CodeM34fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M34d_(ref Tensor<M34d> value)
        {
            var data = value.Data; CodeM34dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M44i_(ref Tensor<M44i> value)
        {
            var data = value.Data; CodeM44iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M44l_(ref Tensor<M44l> value)
        {
            var data = value.Data; CodeM44lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M44f_(ref Tensor<M44f> value)
        {
            var data = value.Data; CodeM44fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_M44d_(ref Tensor<M44d> value)
        {
            var data = value.Data; CodeM44dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C3b_(ref Tensor<C3b> value)
        {
            var data = value.Data; CodeC3bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C3us_(ref Tensor<C3us> value)
        {
            var data = value.Data; CodeC3usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C3ui_(ref Tensor<C3ui> value)
        {
            var data = value.Data; CodeC3uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C3f_(ref Tensor<C3f> value)
        {
            var data = value.Data; CodeC3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C3d_(ref Tensor<C3d> value)
        {
            var data = value.Data; CodeC3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C4b_(ref Tensor<C4b> value)
        {
            var data = value.Data; CodeC4bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C4us_(ref Tensor<C4us> value)
        {
            var data = value.Data; CodeC4usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C4ui_(ref Tensor<C4ui> value)
        {
            var data = value.Data; CodeC4uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C4f_(ref Tensor<C4f> value)
        {
            var data = value.Data; CodeC4fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_C4d_(ref Tensor<C4d> value)
        {
            var data = value.Data; CodeC4dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1b_(ref Tensor<Range1b> value)
        {
            var data = value.Data; CodeRange1bArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1sb_(ref Tensor<Range1sb> value)
        {
            var data = value.Data; CodeRange1sbArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1s_(ref Tensor<Range1s> value)
        {
            var data = value.Data; CodeRange1sArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1us_(ref Tensor<Range1us> value)
        {
            var data = value.Data; CodeRange1usArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1i_(ref Tensor<Range1i> value)
        {
            var data = value.Data; CodeRange1iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1ui_(ref Tensor<Range1ui> value)
        {
            var data = value.Data; CodeRange1uiArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1l_(ref Tensor<Range1l> value)
        {
            var data = value.Data; CodeRange1lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1ul_(ref Tensor<Range1ul> value)
        {
            var data = value.Data; CodeRange1ulArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1f_(ref Tensor<Range1f> value)
        {
            var data = value.Data; CodeRange1fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Range1d_(ref Tensor<Range1d> value)
        {
            var data = value.Data; CodeRange1dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box2i_(ref Tensor<Box2i> value)
        {
            var data = value.Data; CodeBox2iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box2l_(ref Tensor<Box2l> value)
        {
            var data = value.Data; CodeBox2lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box2f_(ref Tensor<Box2f> value)
        {
            var data = value.Data; CodeBox2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box2d_(ref Tensor<Box2d> value)
        {
            var data = value.Data; CodeBox2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box3i_(ref Tensor<Box3i> value)
        {
            var data = value.Data; CodeBox3iArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box3l_(ref Tensor<Box3l> value)
        {
            var data = value.Data; CodeBox3lArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box3f_(ref Tensor<Box3f> value)
        {
            var data = value.Data; CodeBox3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Box3d_(ref Tensor<Box3d> value)
        {
            var data = value.Data; CodeBox3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Euclidean3f_(ref Tensor<Euclidean3f> value)
        {
            var data = value.Data; CodeEuclidean3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Euclidean3d_(ref Tensor<Euclidean3d> value)
        {
            var data = value.Data; CodeEuclidean3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Rot2f_(ref Tensor<Rot2f> value)
        {
            var data = value.Data; CodeRot2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Rot2d_(ref Tensor<Rot2d> value)
        {
            var data = value.Data; CodeRot2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Rot3f_(ref Tensor<Rot3f> value)
        {
            var data = value.Data; CodeRot3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Rot3d_(ref Tensor<Rot3d> value)
        {
            var data = value.Data; CodeRot3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Scale3f_(ref Tensor<Scale3f> value)
        {
            var data = value.Data; CodeScale3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Scale3d_(ref Tensor<Scale3d> value)
        {
            var data = value.Data; CodeScale3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Shift3f_(ref Tensor<Shift3f> value)
        {
            var data = value.Data; CodeShift3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Shift3d_(ref Tensor<Shift3d> value)
        {
            var data = value.Data; CodeShift3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Trafo2f_(ref Tensor<Trafo2f> value)
        {
            var data = value.Data; CodeTrafo2fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Trafo2d_(ref Tensor<Trafo2d> value)
        {
            var data = value.Data; CodeTrafo2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Trafo3f_(ref Tensor<Trafo3f> value)
        {
            var data = value.Data; CodeTrafo3fArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Trafo3d_(ref Tensor<Trafo3d> value)
        {
            var data = value.Data; CodeTrafo3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Bool_(ref Tensor<bool> value)
        {
            var data = value.Data; CodeBoolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Char_(ref Tensor<char> value)
        {
            var data = value.Data; CodeCharArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_String_(ref Tensor<string> value)
        {
            var data = value.Data; CodeStringArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Type_(ref Tensor<Type> value)
        {
            var data = value.Data; CodeTypeArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Guid_(ref Tensor<Guid> value)
        {
            var data = value.Data; CodeGuidArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Symbol_(ref Tensor<Symbol> value)
        {
            var data = value.Data; CodeSymbolArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Circle2d_(ref Tensor<Circle2d> value)
        {
            var data = value.Data; CodeCircle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Line2d_(ref Tensor<Line2d> value)
        {
            var data = value.Data; CodeLine2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Line3d_(ref Tensor<Line3d> value)
        {
            var data = value.Data; CodeLine3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Plane2d_(ref Tensor<Plane2d> value)
        {
            var data = value.Data; CodePlane2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Plane3d_(ref Tensor<Plane3d> value)
        {
            var data = value.Data; CodePlane3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_PlaneWithPoint3d_(ref Tensor<PlaneWithPoint3d> value)
        {
            var data = value.Data; CodePlaneWithPoint3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Quad2d_(ref Tensor<Quad2d> value)
        {
            var data = value.Data; CodeQuad2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Quad3d_(ref Tensor<Quad3d> value)
        {
            var data = value.Data; CodeQuad3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Ray2d_(ref Tensor<Ray2d> value)
        {
            var data = value.Data; CodeRay2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Ray3d_(ref Tensor<Ray3d> value)
        {
            var data = value.Data; CodeRay3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Sphere3d_(ref Tensor<Sphere3d> value)
        {
            var data = value.Data; CodeSphere3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Triangle2d_(ref Tensor<Triangle2d> value)
        {
            var data = value.Data; CodeTriangle2dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        public void CodeTensor_of_Triangle3d_(ref Tensor<Triangle3d> value)
        {
            var data = value.Data; CodeTriangle3dArray(ref data);
            var origin = value.Origin; CodeLong(ref origin);
            var size = value.Size; CodeLongArray(ref size);
            var delta = value.Delta; CodeLongArray(ref delta);
        }

        #endregion

        #region Arrays

        public void CodeV2iArray(ref V2i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV2lArray(ref V2l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV2fArray(ref V2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV2dArray(ref V2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV3iArray(ref V3i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV3lArray(ref V3l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV3fArray(ref V3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV3dArray(ref V3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV4iArray(ref V4i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV4lArray(ref V4l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV4fArray(ref V4f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeV4dArray(ref V4d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM22iArray(ref M22i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM22lArray(ref M22l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM22fArray(ref M22f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM22dArray(ref M22d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM23iArray(ref M23i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM23lArray(ref M23l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM23fArray(ref M23f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM23dArray(ref M23d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM33iArray(ref M33i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM33lArray(ref M33l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM33fArray(ref M33f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM33dArray(ref M33d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM34iArray(ref M34i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM34lArray(ref M34l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM34fArray(ref M34f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM34dArray(ref M34d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM44iArray(ref M44i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM44lArray(ref M44l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM44fArray(ref M44f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeM44dArray(ref M44d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1bArray(ref Range1b[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1sbArray(ref Range1sb[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1sArray(ref Range1s[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1usArray(ref Range1us[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1iArray(ref Range1i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1uiArray(ref Range1ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1lArray(ref Range1l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1ulArray(ref Range1ul[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1fArray(ref Range1f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeRange1dArray(ref Range1d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox2iArray(ref Box2i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox2lArray(ref Box2l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox2fArray(ref Box2f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox2dArray(ref Box2d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox3iArray(ref Box3i[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox3lArray(ref Box3l[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox3fArray(ref Box3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeBox3dArray(ref Box3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC3bArray(ref C3b[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC3usArray(ref C3us[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC3uiArray(ref C3ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC3fArray(ref C3f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC3dArray(ref C3d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC4bArray(ref C4b[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC4usArray(ref C4us[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC4uiArray(ref C4ui[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC4fArray(ref C4f[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
        }

        public void CodeC4dArray(ref C4d[] value)
        {
            long count = CodeCountLong(ref value);
            if (count < 1) return;
            m_writer.WriteArray(value, 0, count);
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

        #endregion

        #region Multi-Dimensional Arrays

        public void CodeByteArray2d(ref byte[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeByteArray3d(ref byte[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeSByteArray2d(ref sbyte[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeSByteArray3d(ref sbyte[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeShortArray2d(ref short[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeShortArray3d(ref short[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeUShortArray2d(ref ushort[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeUShortArray3d(ref ushort[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeIntArray2d(ref int[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeIntArray3d(ref int[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeUIntArray2d(ref uint[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeUIntArray3d(ref uint[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeLongArray2d(ref long[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeLongArray3d(ref long[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeULongArray2d(ref ulong[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeULongArray3d(ref ulong[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeFloatArray2d(ref float[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeFloatArray3d(ref float[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeDoubleArray2d(ref double[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeDoubleArray3d(ref double[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeFractionArray2d(ref Fraction[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeFractionArray3d(ref Fraction[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV2iArray2d(ref V2i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV2iArray3d(ref V2i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV2lArray2d(ref V2l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV2lArray3d(ref V2l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV2fArray2d(ref V2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV2fArray3d(ref V2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV2dArray2d(ref V2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV2dArray3d(ref V2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV3iArray2d(ref V3i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV3iArray3d(ref V3i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV3lArray2d(ref V3l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV3lArray3d(ref V3l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV3fArray2d(ref V3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV3fArray3d(ref V3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV3dArray2d(ref V3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV3dArray3d(ref V3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV4iArray2d(ref V4i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV4iArray3d(ref V4i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV4lArray2d(ref V4l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV4lArray3d(ref V4l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV4fArray2d(ref V4f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV4fArray3d(ref V4f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeV4dArray2d(ref V4d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeV4dArray3d(ref V4d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM22iArray2d(ref M22i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM22iArray3d(ref M22i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM22lArray2d(ref M22l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM22lArray3d(ref M22l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM22fArray2d(ref M22f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM22fArray3d(ref M22f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM22dArray2d(ref M22d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM22dArray3d(ref M22d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM23iArray2d(ref M23i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM23iArray3d(ref M23i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM23lArray2d(ref M23l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM23lArray3d(ref M23l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM23fArray2d(ref M23f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM23fArray3d(ref M23f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM23dArray2d(ref M23d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM23dArray3d(ref M23d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM33iArray2d(ref M33i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM33iArray3d(ref M33i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM33lArray2d(ref M33l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM33lArray3d(ref M33l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM33fArray2d(ref M33f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM33fArray3d(ref M33f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM33dArray2d(ref M33d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM33dArray3d(ref M33d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM34iArray2d(ref M34i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM34iArray3d(ref M34i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM34lArray2d(ref M34l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM34lArray3d(ref M34l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM34fArray2d(ref M34f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM34fArray3d(ref M34f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM34dArray2d(ref M34d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM34dArray3d(ref M34d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM44iArray2d(ref M44i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM44iArray3d(ref M44i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM44lArray2d(ref M44l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM44lArray3d(ref M44l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM44fArray2d(ref M44f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM44fArray3d(ref M44f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeM44dArray2d(ref M44d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeM44dArray3d(ref M44d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC3bArray2d(ref C3b[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC3bArray3d(ref C3b[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC3usArray2d(ref C3us[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC3usArray3d(ref C3us[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC3uiArray2d(ref C3ui[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC3uiArray3d(ref C3ui[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC3fArray2d(ref C3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC3fArray3d(ref C3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC3dArray2d(ref C3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC3dArray3d(ref C3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC4bArray2d(ref C4b[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC4bArray3d(ref C4b[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC4usArray2d(ref C4us[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC4usArray3d(ref C4us[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC4uiArray2d(ref C4ui[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC4uiArray3d(ref C4ui[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC4fArray2d(ref C4f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC4fArray3d(ref C4f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeC4dArray2d(ref C4d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeC4dArray3d(ref C4d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1bArray2d(ref Range1b[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1bArray3d(ref Range1b[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1sbArray2d(ref Range1sb[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1sbArray3d(ref Range1sb[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1sArray2d(ref Range1s[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1sArray3d(ref Range1s[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1usArray2d(ref Range1us[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1usArray3d(ref Range1us[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1iArray2d(ref Range1i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1iArray3d(ref Range1i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1uiArray2d(ref Range1ui[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1uiArray3d(ref Range1ui[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1lArray2d(ref Range1l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1lArray3d(ref Range1l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1ulArray2d(ref Range1ul[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1ulArray3d(ref Range1ul[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1fArray2d(ref Range1f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1fArray3d(ref Range1f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRange1dArray2d(ref Range1d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRange1dArray3d(ref Range1d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox2iArray2d(ref Box2i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox2iArray3d(ref Box2i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox2lArray2d(ref Box2l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox2lArray3d(ref Box2l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox2fArray2d(ref Box2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox2fArray3d(ref Box2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox2dArray2d(ref Box2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox2dArray3d(ref Box2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox3iArray2d(ref Box3i[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox3iArray3d(ref Box3i[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox3lArray2d(ref Box3l[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox3lArray3d(ref Box3l[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox3fArray2d(ref Box3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox3fArray3d(ref Box3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeBox3dArray2d(ref Box3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeBox3dArray3d(ref Box3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeEuclidean3fArray2d(ref Euclidean3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeEuclidean3fArray3d(ref Euclidean3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeEuclidean3dArray2d(ref Euclidean3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeEuclidean3dArray3d(ref Euclidean3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRot2fArray2d(ref Rot2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRot2fArray3d(ref Rot2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRot2dArray2d(ref Rot2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRot2dArray3d(ref Rot2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRot3fArray2d(ref Rot3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRot3fArray3d(ref Rot3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeRot3dArray2d(ref Rot3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeRot3dArray3d(ref Rot3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeScale3fArray2d(ref Scale3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeScale3fArray3d(ref Scale3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeScale3dArray2d(ref Scale3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeScale3dArray3d(ref Scale3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeShift3fArray2d(ref Shift3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeShift3fArray3d(ref Shift3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeShift3dArray2d(ref Shift3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeShift3dArray3d(ref Shift3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo2fArray2d(ref Trafo2f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeTrafo2fArray3d(ref Trafo2f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo2dArray2d(ref Trafo2d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeTrafo2dArray3d(ref Trafo2d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo3fArray2d(ref Trafo3f[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeTrafo3fArray3d(ref Trafo3f[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
        }

        public void CodeTrafo3dArray2d(ref Trafo3d[,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            m_writer.WriteArray(value, c0 * c1);
        }

        public void CodeTrafo3dArray3d(ref Trafo3d[, ,] value)
        {
            long[] countArray = CodeCountLong(ref value);
            long c0 = countArray[0]; if (c0 < 1) return;
            long c1 = countArray[1]; if (c1 < 1) return;
            long c2 = countArray[2]; if (c2 < 1) return;
            m_writer.WriteArray(value, c0 * c1 * c2);
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
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V2l_(ref List<V2l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V2f_(ref List<V2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V2d_(ref List<V2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V3i_(ref List<V3i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V3l_(ref List<V3l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V3f_(ref List<V3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V3d_(ref List<V3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V4i_(ref List<V4i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V4l_(ref List<V4l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V4f_(ref List<V4f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_V4d_(ref List<V4d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M22i_(ref List<M22i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M22l_(ref List<M22l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M22f_(ref List<M22f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M22d_(ref List<M22d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M23i_(ref List<M23i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M23l_(ref List<M23l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M23f_(ref List<M23f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M23d_(ref List<M23d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M33i_(ref List<M33i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M33l_(ref List<M33l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M33f_(ref List<M33f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M33d_(ref List<M33d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M34i_(ref List<M34i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M34l_(ref List<M34l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M34f_(ref List<M34f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M34d_(ref List<M34d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M44i_(ref List<M44i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M44l_(ref List<M44l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M44f_(ref List<M44f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_M44d_(ref List<M44d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1b_(ref List<Range1b> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1sb_(ref List<Range1sb> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1s_(ref List<Range1s> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1us_(ref List<Range1us> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1i_(ref List<Range1i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1ui_(ref List<Range1ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1l_(ref List<Range1l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1ul_(ref List<Range1ul> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1f_(ref List<Range1f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Range1d_(ref List<Range1d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box2i_(ref List<Box2i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box2l_(ref List<Box2l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box2f_(ref List<Box2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box2d_(ref List<Box2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box3i_(ref List<Box3i> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box3l_(ref List<Box3l> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box3f_(ref List<Box3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Box3d_(ref List<Box3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C3b_(ref List<C3b> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C3us_(ref List<C3us> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C3ui_(ref List<C3ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C3f_(ref List<C3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C3d_(ref List<C3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C4b_(ref List<C4b> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C4us_(ref List<C4us> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C4ui_(ref List<C4ui> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C4f_(ref List<C4f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_C4d_(ref List<C4d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            m_writer.WriteList(value, 0, count);
        }

        public void CodeList_of_Euclidean3f_(ref List<Euclidean3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeEuclidean3f(ref x); }
        }

        public void CodeList_of_Euclidean3d_(ref List<Euclidean3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeEuclidean3d(ref x); }
        }

        public void CodeList_of_Rot2f_(ref List<Rot2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeRot2f(ref x); }
        }

        public void CodeList_of_Rot2d_(ref List<Rot2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeRot2d(ref x); }
        }

        public void CodeList_of_Rot3f_(ref List<Rot3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeRot3f(ref x); }
        }

        public void CodeList_of_Rot3d_(ref List<Rot3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeRot3d(ref x); }
        }

        public void CodeList_of_Scale3f_(ref List<Scale3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeScale3f(ref x); }
        }

        public void CodeList_of_Scale3d_(ref List<Scale3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeScale3d(ref x); }
        }

        public void CodeList_of_Shift3f_(ref List<Shift3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeShift3f(ref x); }
        }

        public void CodeList_of_Shift3d_(ref List<Shift3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeShift3d(ref x); }
        }

        public void CodeList_of_Trafo2f_(ref List<Trafo2f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTrafo2f(ref x); }
        }

        public void CodeList_of_Trafo2d_(ref List<Trafo2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTrafo2d(ref x); }
        }

        public void CodeList_of_Trafo3f_(ref List<Trafo3f> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTrafo3f(ref x); }
        }

        public void CodeList_of_Trafo3d_(ref List<Trafo3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTrafo3d(ref x); }
        }

        public void CodeList_of_Circle2d_(ref List<Circle2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeCircle2d(ref x); }
        }

        public void CodeList_of_Line2d_(ref List<Line2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeLine2d(ref x); }
        }

        public void CodeList_of_Line3d_(ref List<Line3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeLine3d(ref x); }
        }

        public void CodeList_of_Plane2d_(ref List<Plane2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodePlane2d(ref x); }
        }

        public void CodeList_of_Plane3d_(ref List<Plane3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodePlane3d(ref x); }
        }

        public void CodeList_of_PlaneWithPoint3d_(ref List<PlaneWithPoint3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodePlaneWithPoint3d(ref x); }
        }

        public void CodeList_of_Quad2d_(ref List<Quad2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeQuad2d(ref x); }
        }

        public void CodeList_of_Quad3d_(ref List<Quad3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeQuad3d(ref x); }
        }

        public void CodeList_of_Ray2d_(ref List<Ray2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeRay2d(ref x); }
        }

        public void CodeList_of_Ray3d_(ref List<Ray3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeRay3d(ref x); }
        }

        public void CodeList_of_Sphere3d_(ref List<Sphere3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeSphere3d(ref x); }
        }

        public void CodeList_of_Triangle2d_(ref List<Triangle2d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTriangle2d(ref x); }
        }

        public void CodeList_of_Triangle3d_(ref List<Triangle3d> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTriangle3d(ref x); }
        }

        public void CodeList_of_Vector_of_Byte__(ref List<Vector<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Byte_(ref x); }
        }

        public void CodeList_of_Vector_of_SByte__(ref List<Vector<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_SByte_(ref x); }
        }

        public void CodeList_of_Vector_of_Short__(ref List<Vector<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Short_(ref x); }
        }

        public void CodeList_of_Vector_of_UShort__(ref List<Vector<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_UShort_(ref x); }
        }

        public void CodeList_of_Vector_of_Int__(ref List<Vector<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Int_(ref x); }
        }

        public void CodeList_of_Vector_of_UInt__(ref List<Vector<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_UInt_(ref x); }
        }

        public void CodeList_of_Vector_of_Long__(ref List<Vector<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Long_(ref x); }
        }

        public void CodeList_of_Vector_of_ULong__(ref List<Vector<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_ULong_(ref x); }
        }

        public void CodeList_of_Vector_of_Float__(ref List<Vector<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Float_(ref x); }
        }

        public void CodeList_of_Vector_of_Double__(ref List<Vector<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Double_(ref x); }
        }

        public void CodeList_of_Vector_of_Fraction__(ref List<Vector<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Fraction_(ref x); }
        }

        public void CodeList_of_Vector_of_V2i__(ref List<Vector<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V2i_(ref x); }
        }

        public void CodeList_of_Vector_of_V2l__(ref List<Vector<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V2l_(ref x); }
        }

        public void CodeList_of_Vector_of_V2f__(ref List<Vector<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V2f_(ref x); }
        }

        public void CodeList_of_Vector_of_V2d__(ref List<Vector<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V2d_(ref x); }
        }

        public void CodeList_of_Vector_of_V3i__(ref List<Vector<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V3i_(ref x); }
        }

        public void CodeList_of_Vector_of_V3l__(ref List<Vector<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V3l_(ref x); }
        }

        public void CodeList_of_Vector_of_V3f__(ref List<Vector<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V3f_(ref x); }
        }

        public void CodeList_of_Vector_of_V3d__(ref List<Vector<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V3d_(ref x); }
        }

        public void CodeList_of_Vector_of_V4i__(ref List<Vector<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V4i_(ref x); }
        }

        public void CodeList_of_Vector_of_V4l__(ref List<Vector<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V4l_(ref x); }
        }

        public void CodeList_of_Vector_of_V4f__(ref List<Vector<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V4f_(ref x); }
        }

        public void CodeList_of_Vector_of_V4d__(ref List<Vector<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_V4d_(ref x); }
        }

        public void CodeList_of_Vector_of_M22i__(ref List<Vector<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M22i_(ref x); }
        }

        public void CodeList_of_Vector_of_M22l__(ref List<Vector<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M22l_(ref x); }
        }

        public void CodeList_of_Vector_of_M22f__(ref List<Vector<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M22f_(ref x); }
        }

        public void CodeList_of_Vector_of_M22d__(ref List<Vector<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M22d_(ref x); }
        }

        public void CodeList_of_Vector_of_M23i__(ref List<Vector<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M23i_(ref x); }
        }

        public void CodeList_of_Vector_of_M23l__(ref List<Vector<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M23l_(ref x); }
        }

        public void CodeList_of_Vector_of_M23f__(ref List<Vector<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M23f_(ref x); }
        }

        public void CodeList_of_Vector_of_M23d__(ref List<Vector<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M23d_(ref x); }
        }

        public void CodeList_of_Vector_of_M33i__(ref List<Vector<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M33i_(ref x); }
        }

        public void CodeList_of_Vector_of_M33l__(ref List<Vector<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M33l_(ref x); }
        }

        public void CodeList_of_Vector_of_M33f__(ref List<Vector<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M33f_(ref x); }
        }

        public void CodeList_of_Vector_of_M33d__(ref List<Vector<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M33d_(ref x); }
        }

        public void CodeList_of_Vector_of_M34i__(ref List<Vector<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M34i_(ref x); }
        }

        public void CodeList_of_Vector_of_M34l__(ref List<Vector<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M34l_(ref x); }
        }

        public void CodeList_of_Vector_of_M34f__(ref List<Vector<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M34f_(ref x); }
        }

        public void CodeList_of_Vector_of_M34d__(ref List<Vector<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M34d_(ref x); }
        }

        public void CodeList_of_Vector_of_M44i__(ref List<Vector<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M44i_(ref x); }
        }

        public void CodeList_of_Vector_of_M44l__(ref List<Vector<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M44l_(ref x); }
        }

        public void CodeList_of_Vector_of_M44f__(ref List<Vector<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M44f_(ref x); }
        }

        public void CodeList_of_Vector_of_M44d__(ref List<Vector<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_M44d_(ref x); }
        }

        public void CodeList_of_Vector_of_C3b__(ref List<Vector<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C3b_(ref x); }
        }

        public void CodeList_of_Vector_of_C3us__(ref List<Vector<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C3us_(ref x); }
        }

        public void CodeList_of_Vector_of_C3ui__(ref List<Vector<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C3ui_(ref x); }
        }

        public void CodeList_of_Vector_of_C3f__(ref List<Vector<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C3f_(ref x); }
        }

        public void CodeList_of_Vector_of_C3d__(ref List<Vector<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C3d_(ref x); }
        }

        public void CodeList_of_Vector_of_C4b__(ref List<Vector<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C4b_(ref x); }
        }

        public void CodeList_of_Vector_of_C4us__(ref List<Vector<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C4us_(ref x); }
        }

        public void CodeList_of_Vector_of_C4ui__(ref List<Vector<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C4ui_(ref x); }
        }

        public void CodeList_of_Vector_of_C4f__(ref List<Vector<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C4f_(ref x); }
        }

        public void CodeList_of_Vector_of_C4d__(ref List<Vector<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_C4d_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1b__(ref List<Vector<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1b_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1sb__(ref List<Vector<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1sb_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1s__(ref List<Vector<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1s_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1us__(ref List<Vector<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1us_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1i__(ref List<Vector<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1i_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1ui__(ref List<Vector<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1ui_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1l__(ref List<Vector<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1l_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1ul__(ref List<Vector<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1ul_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1f__(ref List<Vector<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1f_(ref x); }
        }

        public void CodeList_of_Vector_of_Range1d__(ref List<Vector<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Range1d_(ref x); }
        }

        public void CodeList_of_Vector_of_Box2i__(ref List<Vector<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box2i_(ref x); }
        }

        public void CodeList_of_Vector_of_Box2l__(ref List<Vector<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box2l_(ref x); }
        }

        public void CodeList_of_Vector_of_Box2f__(ref List<Vector<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box2f_(ref x); }
        }

        public void CodeList_of_Vector_of_Box2d__(ref List<Vector<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Box3i__(ref List<Vector<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box3i_(ref x); }
        }

        public void CodeList_of_Vector_of_Box3l__(ref List<Vector<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box3l_(ref x); }
        }

        public void CodeList_of_Vector_of_Box3f__(ref List<Vector<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box3f_(ref x); }
        }

        public void CodeList_of_Vector_of_Box3d__(ref List<Vector<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Box3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Euclidean3f__(ref List<Vector<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Euclidean3f_(ref x); }
        }

        public void CodeList_of_Vector_of_Euclidean3d__(ref List<Vector<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Euclidean3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Rot2f__(ref List<Vector<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Rot2f_(ref x); }
        }

        public void CodeList_of_Vector_of_Rot2d__(ref List<Vector<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Rot2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Rot3f__(ref List<Vector<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Rot3f_(ref x); }
        }

        public void CodeList_of_Vector_of_Rot3d__(ref List<Vector<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Rot3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Scale3f__(ref List<Vector<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Scale3f_(ref x); }
        }

        public void CodeList_of_Vector_of_Scale3d__(ref List<Vector<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Scale3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Shift3f__(ref List<Vector<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Shift3f_(ref x); }
        }

        public void CodeList_of_Vector_of_Shift3d__(ref List<Vector<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Shift3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Trafo2f__(ref List<Vector<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Trafo2f_(ref x); }
        }

        public void CodeList_of_Vector_of_Trafo2d__(ref List<Vector<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Trafo2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Trafo3f__(ref List<Vector<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Trafo3f_(ref x); }
        }

        public void CodeList_of_Vector_of_Trafo3d__(ref List<Vector<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Trafo3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Bool__(ref List<Vector<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Bool_(ref x); }
        }

        public void CodeList_of_Vector_of_Char__(ref List<Vector<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Char_(ref x); }
        }

        public void CodeList_of_Vector_of_String__(ref List<Vector<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_String_(ref x); }
        }

        public void CodeList_of_Vector_of_Type__(ref List<Vector<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Type_(ref x); }
        }

        public void CodeList_of_Vector_of_Guid__(ref List<Vector<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Guid_(ref x); }
        }

        public void CodeList_of_Vector_of_Symbol__(ref List<Vector<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Symbol_(ref x); }
        }

        public void CodeList_of_Vector_of_Circle2d__(ref List<Vector<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Circle2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Line2d__(ref List<Vector<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Line2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Line3d__(ref List<Vector<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Line3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Plane2d__(ref List<Vector<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Plane2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Plane3d__(ref List<Vector<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Plane3d_(ref x); }
        }

        public void CodeList_of_Vector_of_PlaneWithPoint3d__(ref List<Vector<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_PlaneWithPoint3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Quad2d__(ref List<Vector<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Quad2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Quad3d__(ref List<Vector<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Quad3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Ray2d__(ref List<Vector<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Ray2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Ray3d__(ref List<Vector<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Ray3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Sphere3d__(ref List<Vector<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Sphere3d_(ref x); }
        }

        public void CodeList_of_Vector_of_Triangle2d__(ref List<Vector<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Triangle2d_(ref x); }
        }

        public void CodeList_of_Vector_of_Triangle3d__(ref List<Vector<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVector_of_Triangle3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Byte__(ref List<Matrix<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Byte_(ref x); }
        }

        public void CodeList_of_Matrix_of_SByte__(ref List<Matrix<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_SByte_(ref x); }
        }

        public void CodeList_of_Matrix_of_Short__(ref List<Matrix<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Short_(ref x); }
        }

        public void CodeList_of_Matrix_of_UShort__(ref List<Matrix<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_UShort_(ref x); }
        }

        public void CodeList_of_Matrix_of_Int__(ref List<Matrix<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Int_(ref x); }
        }

        public void CodeList_of_Matrix_of_UInt__(ref List<Matrix<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_UInt_(ref x); }
        }

        public void CodeList_of_Matrix_of_Long__(ref List<Matrix<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Long_(ref x); }
        }

        public void CodeList_of_Matrix_of_ULong__(ref List<Matrix<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_ULong_(ref x); }
        }

        public void CodeList_of_Matrix_of_Float__(ref List<Matrix<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Float_(ref x); }
        }

        public void CodeList_of_Matrix_of_Double__(ref List<Matrix<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Double_(ref x); }
        }

        public void CodeList_of_Matrix_of_Fraction__(ref List<Matrix<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Fraction_(ref x); }
        }

        public void CodeList_of_Matrix_of_V2i__(ref List<Matrix<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V2i_(ref x); }
        }

        public void CodeList_of_Matrix_of_V2l__(ref List<Matrix<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V2l_(ref x); }
        }

        public void CodeList_of_Matrix_of_V2f__(ref List<Matrix<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V2f_(ref x); }
        }

        public void CodeList_of_Matrix_of_V2d__(ref List<Matrix<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_V3i__(ref List<Matrix<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V3i_(ref x); }
        }

        public void CodeList_of_Matrix_of_V3l__(ref List<Matrix<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V3l_(ref x); }
        }

        public void CodeList_of_Matrix_of_V3f__(ref List<Matrix<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_V3d__(ref List<Matrix<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_V4i__(ref List<Matrix<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V4i_(ref x); }
        }

        public void CodeList_of_Matrix_of_V4l__(ref List<Matrix<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V4l_(ref x); }
        }

        public void CodeList_of_Matrix_of_V4f__(ref List<Matrix<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V4f_(ref x); }
        }

        public void CodeList_of_Matrix_of_V4d__(ref List<Matrix<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_V4d_(ref x); }
        }

        public void CodeList_of_Matrix_of_M22i__(ref List<Matrix<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M22i_(ref x); }
        }

        public void CodeList_of_Matrix_of_M22l__(ref List<Matrix<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M22l_(ref x); }
        }

        public void CodeList_of_Matrix_of_M22f__(ref List<Matrix<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M22f_(ref x); }
        }

        public void CodeList_of_Matrix_of_M22d__(ref List<Matrix<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M22d_(ref x); }
        }

        public void CodeList_of_Matrix_of_M23i__(ref List<Matrix<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M23i_(ref x); }
        }

        public void CodeList_of_Matrix_of_M23l__(ref List<Matrix<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M23l_(ref x); }
        }

        public void CodeList_of_Matrix_of_M23f__(ref List<Matrix<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M23f_(ref x); }
        }

        public void CodeList_of_Matrix_of_M23d__(ref List<Matrix<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M23d_(ref x); }
        }

        public void CodeList_of_Matrix_of_M33i__(ref List<Matrix<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M33i_(ref x); }
        }

        public void CodeList_of_Matrix_of_M33l__(ref List<Matrix<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M33l_(ref x); }
        }

        public void CodeList_of_Matrix_of_M33f__(ref List<Matrix<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M33f_(ref x); }
        }

        public void CodeList_of_Matrix_of_M33d__(ref List<Matrix<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M33d_(ref x); }
        }

        public void CodeList_of_Matrix_of_M34i__(ref List<Matrix<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M34i_(ref x); }
        }

        public void CodeList_of_Matrix_of_M34l__(ref List<Matrix<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M34l_(ref x); }
        }

        public void CodeList_of_Matrix_of_M34f__(ref List<Matrix<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M34f_(ref x); }
        }

        public void CodeList_of_Matrix_of_M34d__(ref List<Matrix<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M34d_(ref x); }
        }

        public void CodeList_of_Matrix_of_M44i__(ref List<Matrix<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M44i_(ref x); }
        }

        public void CodeList_of_Matrix_of_M44l__(ref List<Matrix<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M44l_(ref x); }
        }

        public void CodeList_of_Matrix_of_M44f__(ref List<Matrix<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M44f_(ref x); }
        }

        public void CodeList_of_Matrix_of_M44d__(ref List<Matrix<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_M44d_(ref x); }
        }

        public void CodeList_of_Matrix_of_C3b__(ref List<Matrix<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C3b_(ref x); }
        }

        public void CodeList_of_Matrix_of_C3us__(ref List<Matrix<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C3us_(ref x); }
        }

        public void CodeList_of_Matrix_of_C3ui__(ref List<Matrix<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C3ui_(ref x); }
        }

        public void CodeList_of_Matrix_of_C3f__(ref List<Matrix<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_C3d__(ref List<Matrix<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_C4b__(ref List<Matrix<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C4b_(ref x); }
        }

        public void CodeList_of_Matrix_of_C4us__(ref List<Matrix<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C4us_(ref x); }
        }

        public void CodeList_of_Matrix_of_C4ui__(ref List<Matrix<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C4ui_(ref x); }
        }

        public void CodeList_of_Matrix_of_C4f__(ref List<Matrix<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C4f_(ref x); }
        }

        public void CodeList_of_Matrix_of_C4d__(ref List<Matrix<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_C4d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1b__(ref List<Matrix<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1b_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1sb__(ref List<Matrix<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1sb_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1s__(ref List<Matrix<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1s_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1us__(ref List<Matrix<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1us_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1i__(ref List<Matrix<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1i_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1ui__(ref List<Matrix<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1ui_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1l__(ref List<Matrix<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1l_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1ul__(ref List<Matrix<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1ul_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1f__(ref List<Matrix<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Range1d__(ref List<Matrix<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Range1d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box2i__(ref List<Matrix<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box2i_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box2l__(ref List<Matrix<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box2l_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box2f__(ref List<Matrix<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box2f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box2d__(ref List<Matrix<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box3i__(ref List<Matrix<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box3i_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box3l__(ref List<Matrix<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box3l_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box3f__(ref List<Matrix<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Box3d__(ref List<Matrix<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Box3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Euclidean3f__(ref List<Matrix<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Euclidean3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Euclidean3d__(ref List<Matrix<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Euclidean3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Rot2f__(ref List<Matrix<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Rot2f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Rot2d__(ref List<Matrix<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Rot2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Rot3f__(ref List<Matrix<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Rot3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Rot3d__(ref List<Matrix<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Rot3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Scale3f__(ref List<Matrix<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Scale3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Scale3d__(ref List<Matrix<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Scale3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Shift3f__(ref List<Matrix<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Shift3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Shift3d__(ref List<Matrix<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Shift3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Trafo2f__(ref List<Matrix<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Trafo2f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Trafo2d__(ref List<Matrix<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Trafo2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Trafo3f__(ref List<Matrix<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Trafo3f_(ref x); }
        }

        public void CodeList_of_Matrix_of_Trafo3d__(ref List<Matrix<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Trafo3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Bool__(ref List<Matrix<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Bool_(ref x); }
        }

        public void CodeList_of_Matrix_of_Char__(ref List<Matrix<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Char_(ref x); }
        }

        public void CodeList_of_Matrix_of_String__(ref List<Matrix<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_String_(ref x); }
        }

        public void CodeList_of_Matrix_of_Type__(ref List<Matrix<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Type_(ref x); }
        }

        public void CodeList_of_Matrix_of_Guid__(ref List<Matrix<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Guid_(ref x); }
        }

        public void CodeList_of_Matrix_of_Symbol__(ref List<Matrix<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Symbol_(ref x); }
        }

        public void CodeList_of_Matrix_of_Circle2d__(ref List<Matrix<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Circle2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Line2d__(ref List<Matrix<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Line2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Line3d__(ref List<Matrix<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Line3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Plane2d__(ref List<Matrix<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Plane2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Plane3d__(ref List<Matrix<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Plane3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_PlaneWithPoint3d__(ref List<Matrix<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_PlaneWithPoint3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Quad2d__(ref List<Matrix<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Quad2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Quad3d__(ref List<Matrix<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Quad3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Ray2d__(ref List<Matrix<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Ray2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Ray3d__(ref List<Matrix<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Ray3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Sphere3d__(ref List<Matrix<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Sphere3d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Triangle2d__(ref List<Matrix<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Triangle2d_(ref x); }
        }

        public void CodeList_of_Matrix_of_Triangle3d__(ref List<Matrix<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeMatrix_of_Triangle3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Byte__(ref List<Volume<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Byte_(ref x); }
        }

        public void CodeList_of_Volume_of_SByte__(ref List<Volume<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_SByte_(ref x); }
        }

        public void CodeList_of_Volume_of_Short__(ref List<Volume<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Short_(ref x); }
        }

        public void CodeList_of_Volume_of_UShort__(ref List<Volume<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_UShort_(ref x); }
        }

        public void CodeList_of_Volume_of_Int__(ref List<Volume<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Int_(ref x); }
        }

        public void CodeList_of_Volume_of_UInt__(ref List<Volume<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_UInt_(ref x); }
        }

        public void CodeList_of_Volume_of_Long__(ref List<Volume<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Long_(ref x); }
        }

        public void CodeList_of_Volume_of_ULong__(ref List<Volume<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_ULong_(ref x); }
        }

        public void CodeList_of_Volume_of_Float__(ref List<Volume<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Float_(ref x); }
        }

        public void CodeList_of_Volume_of_Double__(ref List<Volume<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Double_(ref x); }
        }

        public void CodeList_of_Volume_of_Fraction__(ref List<Volume<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Fraction_(ref x); }
        }

        public void CodeList_of_Volume_of_V2i__(ref List<Volume<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V2i_(ref x); }
        }

        public void CodeList_of_Volume_of_V2l__(ref List<Volume<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V2l_(ref x); }
        }

        public void CodeList_of_Volume_of_V2f__(ref List<Volume<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V2f_(ref x); }
        }

        public void CodeList_of_Volume_of_V2d__(ref List<Volume<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V2d_(ref x); }
        }

        public void CodeList_of_Volume_of_V3i__(ref List<Volume<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V3i_(ref x); }
        }

        public void CodeList_of_Volume_of_V3l__(ref List<Volume<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V3l_(ref x); }
        }

        public void CodeList_of_Volume_of_V3f__(ref List<Volume<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V3f_(ref x); }
        }

        public void CodeList_of_Volume_of_V3d__(ref List<Volume<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V3d_(ref x); }
        }

        public void CodeList_of_Volume_of_V4i__(ref List<Volume<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V4i_(ref x); }
        }

        public void CodeList_of_Volume_of_V4l__(ref List<Volume<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V4l_(ref x); }
        }

        public void CodeList_of_Volume_of_V4f__(ref List<Volume<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V4f_(ref x); }
        }

        public void CodeList_of_Volume_of_V4d__(ref List<Volume<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_V4d_(ref x); }
        }

        public void CodeList_of_Volume_of_M22i__(ref List<Volume<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M22i_(ref x); }
        }

        public void CodeList_of_Volume_of_M22l__(ref List<Volume<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M22l_(ref x); }
        }

        public void CodeList_of_Volume_of_M22f__(ref List<Volume<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M22f_(ref x); }
        }

        public void CodeList_of_Volume_of_M22d__(ref List<Volume<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M22d_(ref x); }
        }

        public void CodeList_of_Volume_of_M23i__(ref List<Volume<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M23i_(ref x); }
        }

        public void CodeList_of_Volume_of_M23l__(ref List<Volume<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M23l_(ref x); }
        }

        public void CodeList_of_Volume_of_M23f__(ref List<Volume<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M23f_(ref x); }
        }

        public void CodeList_of_Volume_of_M23d__(ref List<Volume<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M23d_(ref x); }
        }

        public void CodeList_of_Volume_of_M33i__(ref List<Volume<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M33i_(ref x); }
        }

        public void CodeList_of_Volume_of_M33l__(ref List<Volume<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M33l_(ref x); }
        }

        public void CodeList_of_Volume_of_M33f__(ref List<Volume<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M33f_(ref x); }
        }

        public void CodeList_of_Volume_of_M33d__(ref List<Volume<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M33d_(ref x); }
        }

        public void CodeList_of_Volume_of_M34i__(ref List<Volume<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M34i_(ref x); }
        }

        public void CodeList_of_Volume_of_M34l__(ref List<Volume<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M34l_(ref x); }
        }

        public void CodeList_of_Volume_of_M34f__(ref List<Volume<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M34f_(ref x); }
        }

        public void CodeList_of_Volume_of_M34d__(ref List<Volume<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M34d_(ref x); }
        }

        public void CodeList_of_Volume_of_M44i__(ref List<Volume<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M44i_(ref x); }
        }

        public void CodeList_of_Volume_of_M44l__(ref List<Volume<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M44l_(ref x); }
        }

        public void CodeList_of_Volume_of_M44f__(ref List<Volume<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M44f_(ref x); }
        }

        public void CodeList_of_Volume_of_M44d__(ref List<Volume<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_M44d_(ref x); }
        }

        public void CodeList_of_Volume_of_C3b__(ref List<Volume<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C3b_(ref x); }
        }

        public void CodeList_of_Volume_of_C3us__(ref List<Volume<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C3us_(ref x); }
        }

        public void CodeList_of_Volume_of_C3ui__(ref List<Volume<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C3ui_(ref x); }
        }

        public void CodeList_of_Volume_of_C3f__(ref List<Volume<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C3f_(ref x); }
        }

        public void CodeList_of_Volume_of_C3d__(ref List<Volume<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C3d_(ref x); }
        }

        public void CodeList_of_Volume_of_C4b__(ref List<Volume<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C4b_(ref x); }
        }

        public void CodeList_of_Volume_of_C4us__(ref List<Volume<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C4us_(ref x); }
        }

        public void CodeList_of_Volume_of_C4ui__(ref List<Volume<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C4ui_(ref x); }
        }

        public void CodeList_of_Volume_of_C4f__(ref List<Volume<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C4f_(ref x); }
        }

        public void CodeList_of_Volume_of_C4d__(ref List<Volume<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_C4d_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1b__(ref List<Volume<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1b_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1sb__(ref List<Volume<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1sb_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1s__(ref List<Volume<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1s_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1us__(ref List<Volume<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1us_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1i__(ref List<Volume<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1i_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1ui__(ref List<Volume<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1ui_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1l__(ref List<Volume<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1l_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1ul__(ref List<Volume<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1ul_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1f__(ref List<Volume<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1f_(ref x); }
        }

        public void CodeList_of_Volume_of_Range1d__(ref List<Volume<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Range1d_(ref x); }
        }

        public void CodeList_of_Volume_of_Box2i__(ref List<Volume<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box2i_(ref x); }
        }

        public void CodeList_of_Volume_of_Box2l__(ref List<Volume<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box2l_(ref x); }
        }

        public void CodeList_of_Volume_of_Box2f__(ref List<Volume<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box2f_(ref x); }
        }

        public void CodeList_of_Volume_of_Box2d__(ref List<Volume<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Box3i__(ref List<Volume<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box3i_(ref x); }
        }

        public void CodeList_of_Volume_of_Box3l__(ref List<Volume<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box3l_(ref x); }
        }

        public void CodeList_of_Volume_of_Box3f__(ref List<Volume<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box3f_(ref x); }
        }

        public void CodeList_of_Volume_of_Box3d__(ref List<Volume<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Box3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Euclidean3f__(ref List<Volume<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Euclidean3f_(ref x); }
        }

        public void CodeList_of_Volume_of_Euclidean3d__(ref List<Volume<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Euclidean3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Rot2f__(ref List<Volume<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Rot2f_(ref x); }
        }

        public void CodeList_of_Volume_of_Rot2d__(ref List<Volume<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Rot2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Rot3f__(ref List<Volume<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Rot3f_(ref x); }
        }

        public void CodeList_of_Volume_of_Rot3d__(ref List<Volume<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Rot3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Scale3f__(ref List<Volume<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Scale3f_(ref x); }
        }

        public void CodeList_of_Volume_of_Scale3d__(ref List<Volume<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Scale3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Shift3f__(ref List<Volume<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Shift3f_(ref x); }
        }

        public void CodeList_of_Volume_of_Shift3d__(ref List<Volume<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Shift3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Trafo2f__(ref List<Volume<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Trafo2f_(ref x); }
        }

        public void CodeList_of_Volume_of_Trafo2d__(ref List<Volume<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Trafo2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Trafo3f__(ref List<Volume<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Trafo3f_(ref x); }
        }

        public void CodeList_of_Volume_of_Trafo3d__(ref List<Volume<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Trafo3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Bool__(ref List<Volume<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Bool_(ref x); }
        }

        public void CodeList_of_Volume_of_Char__(ref List<Volume<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Char_(ref x); }
        }

        public void CodeList_of_Volume_of_String__(ref List<Volume<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_String_(ref x); }
        }

        public void CodeList_of_Volume_of_Type__(ref List<Volume<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Type_(ref x); }
        }

        public void CodeList_of_Volume_of_Guid__(ref List<Volume<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Guid_(ref x); }
        }

        public void CodeList_of_Volume_of_Symbol__(ref List<Volume<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Symbol_(ref x); }
        }

        public void CodeList_of_Volume_of_Circle2d__(ref List<Volume<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Circle2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Line2d__(ref List<Volume<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Line2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Line3d__(ref List<Volume<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Line3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Plane2d__(ref List<Volume<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Plane2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Plane3d__(ref List<Volume<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Plane3d_(ref x); }
        }

        public void CodeList_of_Volume_of_PlaneWithPoint3d__(ref List<Volume<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_PlaneWithPoint3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Quad2d__(ref List<Volume<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Quad2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Quad3d__(ref List<Volume<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Quad3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Ray2d__(ref List<Volume<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Ray2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Ray3d__(ref List<Volume<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Ray3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Sphere3d__(ref List<Volume<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Sphere3d_(ref x); }
        }

        public void CodeList_of_Volume_of_Triangle2d__(ref List<Volume<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Triangle2d_(ref x); }
        }

        public void CodeList_of_Volume_of_Triangle3d__(ref List<Volume<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeVolume_of_Triangle3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Byte__(ref List<Tensor<byte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Byte_(ref x); }
        }

        public void CodeList_of_Tensor_of_SByte__(ref List<Tensor<sbyte>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_SByte_(ref x); }
        }

        public void CodeList_of_Tensor_of_Short__(ref List<Tensor<short>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Short_(ref x); }
        }

        public void CodeList_of_Tensor_of_UShort__(ref List<Tensor<ushort>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_UShort_(ref x); }
        }

        public void CodeList_of_Tensor_of_Int__(ref List<Tensor<int>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Int_(ref x); }
        }

        public void CodeList_of_Tensor_of_UInt__(ref List<Tensor<uint>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_UInt_(ref x); }
        }

        public void CodeList_of_Tensor_of_Long__(ref List<Tensor<long>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Long_(ref x); }
        }

        public void CodeList_of_Tensor_of_ULong__(ref List<Tensor<ulong>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_ULong_(ref x); }
        }

        public void CodeList_of_Tensor_of_Float__(ref List<Tensor<float>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Float_(ref x); }
        }

        public void CodeList_of_Tensor_of_Double__(ref List<Tensor<double>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Double_(ref x); }
        }

        public void CodeList_of_Tensor_of_Fraction__(ref List<Tensor<Fraction>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Fraction_(ref x); }
        }

        public void CodeList_of_Tensor_of_V2i__(ref List<Tensor<V2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V2i_(ref x); }
        }

        public void CodeList_of_Tensor_of_V2l__(ref List<Tensor<V2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V2l_(ref x); }
        }

        public void CodeList_of_Tensor_of_V2f__(ref List<Tensor<V2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V2f_(ref x); }
        }

        public void CodeList_of_Tensor_of_V2d__(ref List<Tensor<V2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_V3i__(ref List<Tensor<V3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V3i_(ref x); }
        }

        public void CodeList_of_Tensor_of_V3l__(ref List<Tensor<V3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V3l_(ref x); }
        }

        public void CodeList_of_Tensor_of_V3f__(ref List<Tensor<V3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_V3d__(ref List<Tensor<V3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_V4i__(ref List<Tensor<V4i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V4i_(ref x); }
        }

        public void CodeList_of_Tensor_of_V4l__(ref List<Tensor<V4l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V4l_(ref x); }
        }

        public void CodeList_of_Tensor_of_V4f__(ref List<Tensor<V4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V4f_(ref x); }
        }

        public void CodeList_of_Tensor_of_V4d__(ref List<Tensor<V4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_V4d_(ref x); }
        }

        public void CodeList_of_Tensor_of_M22i__(ref List<Tensor<M22i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M22i_(ref x); }
        }

        public void CodeList_of_Tensor_of_M22l__(ref List<Tensor<M22l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M22l_(ref x); }
        }

        public void CodeList_of_Tensor_of_M22f__(ref List<Tensor<M22f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M22f_(ref x); }
        }

        public void CodeList_of_Tensor_of_M22d__(ref List<Tensor<M22d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M22d_(ref x); }
        }

        public void CodeList_of_Tensor_of_M23i__(ref List<Tensor<M23i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M23i_(ref x); }
        }

        public void CodeList_of_Tensor_of_M23l__(ref List<Tensor<M23l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M23l_(ref x); }
        }

        public void CodeList_of_Tensor_of_M23f__(ref List<Tensor<M23f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M23f_(ref x); }
        }

        public void CodeList_of_Tensor_of_M23d__(ref List<Tensor<M23d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M23d_(ref x); }
        }

        public void CodeList_of_Tensor_of_M33i__(ref List<Tensor<M33i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M33i_(ref x); }
        }

        public void CodeList_of_Tensor_of_M33l__(ref List<Tensor<M33l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M33l_(ref x); }
        }

        public void CodeList_of_Tensor_of_M33f__(ref List<Tensor<M33f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M33f_(ref x); }
        }

        public void CodeList_of_Tensor_of_M33d__(ref List<Tensor<M33d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M33d_(ref x); }
        }

        public void CodeList_of_Tensor_of_M34i__(ref List<Tensor<M34i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M34i_(ref x); }
        }

        public void CodeList_of_Tensor_of_M34l__(ref List<Tensor<M34l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M34l_(ref x); }
        }

        public void CodeList_of_Tensor_of_M34f__(ref List<Tensor<M34f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M34f_(ref x); }
        }

        public void CodeList_of_Tensor_of_M34d__(ref List<Tensor<M34d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M34d_(ref x); }
        }

        public void CodeList_of_Tensor_of_M44i__(ref List<Tensor<M44i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M44i_(ref x); }
        }

        public void CodeList_of_Tensor_of_M44l__(ref List<Tensor<M44l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M44l_(ref x); }
        }

        public void CodeList_of_Tensor_of_M44f__(ref List<Tensor<M44f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M44f_(ref x); }
        }

        public void CodeList_of_Tensor_of_M44d__(ref List<Tensor<M44d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_M44d_(ref x); }
        }

        public void CodeList_of_Tensor_of_C3b__(ref List<Tensor<C3b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C3b_(ref x); }
        }

        public void CodeList_of_Tensor_of_C3us__(ref List<Tensor<C3us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C3us_(ref x); }
        }

        public void CodeList_of_Tensor_of_C3ui__(ref List<Tensor<C3ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C3ui_(ref x); }
        }

        public void CodeList_of_Tensor_of_C3f__(ref List<Tensor<C3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_C3d__(ref List<Tensor<C3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_C4b__(ref List<Tensor<C4b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C4b_(ref x); }
        }

        public void CodeList_of_Tensor_of_C4us__(ref List<Tensor<C4us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C4us_(ref x); }
        }

        public void CodeList_of_Tensor_of_C4ui__(ref List<Tensor<C4ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C4ui_(ref x); }
        }

        public void CodeList_of_Tensor_of_C4f__(ref List<Tensor<C4f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C4f_(ref x); }
        }

        public void CodeList_of_Tensor_of_C4d__(ref List<Tensor<C4d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_C4d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1b__(ref List<Tensor<Range1b>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1b_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1sb__(ref List<Tensor<Range1sb>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1sb_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1s__(ref List<Tensor<Range1s>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1s_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1us__(ref List<Tensor<Range1us>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1us_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1i__(ref List<Tensor<Range1i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1i_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1ui__(ref List<Tensor<Range1ui>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1ui_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1l__(ref List<Tensor<Range1l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1l_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1ul__(ref List<Tensor<Range1ul>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1ul_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1f__(ref List<Tensor<Range1f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Range1d__(ref List<Tensor<Range1d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Range1d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box2i__(ref List<Tensor<Box2i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box2i_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box2l__(ref List<Tensor<Box2l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box2l_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box2f__(ref List<Tensor<Box2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box2f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box2d__(ref List<Tensor<Box2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box3i__(ref List<Tensor<Box3i>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box3i_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box3l__(ref List<Tensor<Box3l>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box3l_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box3f__(ref List<Tensor<Box3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Box3d__(ref List<Tensor<Box3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Box3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Euclidean3f__(ref List<Tensor<Euclidean3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Euclidean3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Euclidean3d__(ref List<Tensor<Euclidean3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Euclidean3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Rot2f__(ref List<Tensor<Rot2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Rot2f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Rot2d__(ref List<Tensor<Rot2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Rot2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Rot3f__(ref List<Tensor<Rot3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Rot3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Rot3d__(ref List<Tensor<Rot3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Rot3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Scale3f__(ref List<Tensor<Scale3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Scale3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Scale3d__(ref List<Tensor<Scale3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Scale3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Shift3f__(ref List<Tensor<Shift3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Shift3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Shift3d__(ref List<Tensor<Shift3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Shift3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Trafo2f__(ref List<Tensor<Trafo2f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Trafo2f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Trafo2d__(ref List<Tensor<Trafo2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Trafo2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Trafo3f__(ref List<Tensor<Trafo3f>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Trafo3f_(ref x); }
        }

        public void CodeList_of_Tensor_of_Trafo3d__(ref List<Tensor<Trafo3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Trafo3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Bool__(ref List<Tensor<bool>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Bool_(ref x); }
        }

        public void CodeList_of_Tensor_of_Char__(ref List<Tensor<char>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Char_(ref x); }
        }

        public void CodeList_of_Tensor_of_String__(ref List<Tensor<string>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_String_(ref x); }
        }

        public void CodeList_of_Tensor_of_Type__(ref List<Tensor<Type>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Type_(ref x); }
        }

        public void CodeList_of_Tensor_of_Guid__(ref List<Tensor<Guid>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Guid_(ref x); }
        }

        public void CodeList_of_Tensor_of_Symbol__(ref List<Tensor<Symbol>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Symbol_(ref x); }
        }

        public void CodeList_of_Tensor_of_Circle2d__(ref List<Tensor<Circle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Circle2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Line2d__(ref List<Tensor<Line2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Line2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Line3d__(ref List<Tensor<Line3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Line3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Plane2d__(ref List<Tensor<Plane2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Plane2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Plane3d__(ref List<Tensor<Plane3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Plane3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_PlaneWithPoint3d__(ref List<Tensor<PlaneWithPoint3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_PlaneWithPoint3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Quad2d__(ref List<Tensor<Quad2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Quad2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Quad3d__(ref List<Tensor<Quad3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Quad3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Ray2d__(ref List<Tensor<Ray2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Ray2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Ray3d__(ref List<Tensor<Ray3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Ray3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Sphere3d__(ref List<Tensor<Sphere3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Sphere3d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Triangle2d__(ref List<Tensor<Triangle2d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Triangle2d_(ref x); }
        }

        public void CodeList_of_Tensor_of_Triangle3d__(ref List<Tensor<Triangle3d>> value)
        {
            int count = CodeCount(ref value);
            if (count < 1) return;
            for (int i = 0; i < count; i++) { var x = value[i]; CodeTensor_of_Triangle3d_(ref x); }
        }

        #endregion
    }
}
