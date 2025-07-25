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
using System.Xml.Linq;

namespace Aardvark.Base.Coder;

// AUTO GENERATED CODE - DO NOT CHANGE!

public partial class NewXmlWritingCoder
{
    #region Vectors

    public void CodeV2i(ref V2i v) { throw new NotImplementedException(); }
    public void CodeV2ui(ref V2ui v) { throw new NotImplementedException(); }
    public void CodeV2l(ref V2l v) { throw new NotImplementedException(); }
    public void CodeV2f(ref V2f v) { throw new NotImplementedException(); }
    public void CodeV2d(ref V2d v) { throw new NotImplementedException(); }
    public void CodeV3i(ref V3i v) { throw new NotImplementedException(); }
    public void CodeV3ui(ref V3ui v) { throw new NotImplementedException(); }
    public void CodeV3l(ref V3l v) { throw new NotImplementedException(); }
    public void CodeV3f(ref V3f v) { throw new NotImplementedException(); }
    public void CodeV3d(ref V3d v) { throw new NotImplementedException(); }
    public void CodeV4i(ref V4i v) { throw new NotImplementedException(); }
    public void CodeV4ui(ref V4ui v) { throw new NotImplementedException(); }
    public void CodeV4l(ref V4l v) { throw new NotImplementedException(); }
    public void CodeV4f(ref V4f v) { throw new NotImplementedException(); }
    public void CodeV4d(ref V4d v) { throw new NotImplementedException(); }

    #endregion

    #region Matrices

    public void CodeM22i(ref M22i v) { throw new NotImplementedException(); }
    public void CodeM22l(ref M22l v) { throw new NotImplementedException(); }
    public void CodeM22f(ref M22f v) { throw new NotImplementedException(); }
    public void CodeM22d(ref M22d v) { throw new NotImplementedException(); }
    public void CodeM23i(ref M23i v) { throw new NotImplementedException(); }
    public void CodeM23l(ref M23l v) { throw new NotImplementedException(); }
    public void CodeM23f(ref M23f v) { throw new NotImplementedException(); }
    public void CodeM23d(ref M23d v) { throw new NotImplementedException(); }
    public void CodeM33i(ref M33i v) { throw new NotImplementedException(); }
    public void CodeM33l(ref M33l v) { throw new NotImplementedException(); }
    public void CodeM33f(ref M33f v) { throw new NotImplementedException(); }
    public void CodeM33d(ref M33d v) { throw new NotImplementedException(); }
    public void CodeM34i(ref M34i v) { throw new NotImplementedException(); }
    public void CodeM34l(ref M34l v) { throw new NotImplementedException(); }
    public void CodeM34f(ref M34f v) { throw new NotImplementedException(); }
    public void CodeM34d(ref M34d v) { throw new NotImplementedException(); }
    public void CodeM44i(ref M44i v) { throw new NotImplementedException(); }
    public void CodeM44l(ref M44l v) { throw new NotImplementedException(); }
    public void CodeM44f(ref M44f v) { throw new NotImplementedException(); }
    public void CodeM44d(ref M44d v) { throw new NotImplementedException(); }

    #endregion

    #region Ranges and Boxes

    public void CodeRange1b(ref Range1b v) { throw new NotImplementedException(); }
    public void CodeRange1sb(ref Range1sb v) { throw new NotImplementedException(); }
    public void CodeRange1s(ref Range1s v) { throw new NotImplementedException(); }
    public void CodeRange1us(ref Range1us v) { throw new NotImplementedException(); }
    public void CodeRange1i(ref Range1i v) { throw new NotImplementedException(); }
    public void CodeRange1ui(ref Range1ui v) { throw new NotImplementedException(); }
    public void CodeRange1l(ref Range1l v) { throw new NotImplementedException(); }
    public void CodeRange1ul(ref Range1ul v) { throw new NotImplementedException(); }
    public void CodeRange1f(ref Range1f v) { throw new NotImplementedException(); }
    public void CodeRange1d(ref Range1d v) { throw new NotImplementedException(); }
    public void CodeBox2i(ref Box2i v) { throw new NotImplementedException(); }
    public void CodeBox2l(ref Box2l v) { throw new NotImplementedException(); }
    public void CodeBox2f(ref Box2f v) { throw new NotImplementedException(); }
    public void CodeBox2d(ref Box2d v) { throw new NotImplementedException(); }
    public void CodeBox3i(ref Box3i v) { throw new NotImplementedException(); }
    public void CodeBox3l(ref Box3l v) { throw new NotImplementedException(); }
    public void CodeBox3f(ref Box3f v) { throw new NotImplementedException(); }
    public void CodeBox3d(ref Box3d v) { throw new NotImplementedException(); }

    #endregion

    #region Geometry Types

    public void CodeCircle2f(ref Circle2f v) { throw new NotImplementedException(); }
    public void CodeLine2f(ref Line2f v) { throw new NotImplementedException(); }
    public void CodeLine3f(ref Line3f v) { throw new NotImplementedException(); }
    public void CodePlane2f(ref Plane2f v) { throw new NotImplementedException(); }
    public void CodePlane3f(ref Plane3f v) { throw new NotImplementedException(); }
    public void CodePlaneWithPoint3f(ref PlaneWithPoint3f v) { throw new NotImplementedException(); }
    public void CodeQuad2f(ref Quad2f v) { throw new NotImplementedException(); }
    public void CodeQuad3f(ref Quad3f v) { throw new NotImplementedException(); }
    public void CodeRay2f(ref Ray2f v) { throw new NotImplementedException(); }
    public void CodeRay3f(ref Ray3f v) { throw new NotImplementedException(); }
    public void CodeSphere3f(ref Sphere3f v) { throw new NotImplementedException(); }
    public void CodeTriangle2f(ref Triangle2f v) { throw new NotImplementedException(); }
    public void CodeTriangle3f(ref Triangle3f v) { throw new NotImplementedException(); }

    public void CodeCircle2d(ref Circle2d v) { throw new NotImplementedException(); }
    public void CodeLine2d(ref Line2d v) { throw new NotImplementedException(); }
    public void CodeLine3d(ref Line3d v) { throw new NotImplementedException(); }
    public void CodePlane2d(ref Plane2d v) { throw new NotImplementedException(); }
    public void CodePlane3d(ref Plane3d v) { throw new NotImplementedException(); }
    public void CodePlaneWithPoint3d(ref PlaneWithPoint3d v) { throw new NotImplementedException(); }
    public void CodeQuad2d(ref Quad2d v) { throw new NotImplementedException(); }
    public void CodeQuad3d(ref Quad3d v) { throw new NotImplementedException(); }
    public void CodeRay2d(ref Ray2d v) { throw new NotImplementedException(); }
    public void CodeRay3d(ref Ray3d v) { throw new NotImplementedException(); }
    public void CodeSphere3d(ref Sphere3d v) { throw new NotImplementedException(); }
    public void CodeTriangle2d(ref Triangle2d v) { throw new NotImplementedException(); }
    public void CodeTriangle3d(ref Triangle3d v) { throw new NotImplementedException(); }

    #endregion

    #region Colors

    public void CodeC3b(ref C3b v) { throw new NotImplementedException(); }
    public void CodeC3us(ref C3us v) { throw new NotImplementedException(); }
    public void CodeC3ui(ref C3ui v) { throw new NotImplementedException(); }
    public void CodeC3f(ref C3f v) { throw new NotImplementedException(); }
    public void CodeC3d(ref C3d v) { throw new NotImplementedException(); }
    public void CodeC4b(ref C4b v) { throw new NotImplementedException(); }
    public void CodeC4us(ref C4us v) { throw new NotImplementedException(); }
    public void CodeC4ui(ref C4ui v) { throw new NotImplementedException(); }
    public void CodeC4f(ref C4f v) { throw new NotImplementedException(); }
    public void CodeC4d(ref C4d v) { throw new NotImplementedException(); }

    #endregion

    #region Trafos

    public void CodeEuclidean3f(ref Euclidean3f v) { throw new NotImplementedException(); }
    public void CodeEuclidean3d(ref Euclidean3d v) { throw new NotImplementedException(); }
    public void CodeRot2f(ref Rot2f v) { throw new NotImplementedException(); }
    public void CodeRot2d(ref Rot2d v) { throw new NotImplementedException(); }
    public void CodeRot3f(ref Rot3f v) { throw new NotImplementedException(); }
    public void CodeRot3d(ref Rot3d v) { throw new NotImplementedException(); }
    public void CodeScale3f(ref Scale3f v) { throw new NotImplementedException(); }
    public void CodeScale3d(ref Scale3d v) { throw new NotImplementedException(); }
    public void CodeShift3f(ref Shift3f v) { throw new NotImplementedException(); }
    public void CodeShift3d(ref Shift3d v) { throw new NotImplementedException(); }
    public void CodeTrafo2f(ref Trafo2f v) { throw new NotImplementedException(); }
    public void CodeTrafo2d(ref Trafo2d v) { throw new NotImplementedException(); }
    public void CodeTrafo3f(ref Trafo3f v) { throw new NotImplementedException(); }
    public void CodeTrafo3d(ref Trafo3d v) { throw new NotImplementedException(); }

    #endregion

    #region Tensors

    public void CodeVector_of_Byte_(ref Vector<byte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_SByte_(ref Vector<sbyte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Short_(ref Vector<short> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_UShort_(ref Vector<ushort> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Int_(ref Vector<int> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_UInt_(ref Vector<uint> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Long_(ref Vector<long> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_ULong_(ref Vector<ulong> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Float_(ref Vector<float> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Double_(ref Vector<double> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Fraction_(ref Vector<Fraction> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V2i_(ref Vector<V2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V2l_(ref Vector<V2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V2f_(ref Vector<V2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V2d_(ref Vector<V2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V3i_(ref Vector<V3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V3l_(ref Vector<V3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V3f_(ref Vector<V3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V3d_(ref Vector<V3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V4i_(ref Vector<V4i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V4l_(ref Vector<V4l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V4f_(ref Vector<V4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_V4d_(ref Vector<V4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M22i_(ref Vector<M22i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M22l_(ref Vector<M22l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M22f_(ref Vector<M22f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M22d_(ref Vector<M22d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M23i_(ref Vector<M23i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M23l_(ref Vector<M23l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M23f_(ref Vector<M23f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M23d_(ref Vector<M23d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M33i_(ref Vector<M33i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M33l_(ref Vector<M33l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M33f_(ref Vector<M33f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M33d_(ref Vector<M33d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M34i_(ref Vector<M34i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M34l_(ref Vector<M34l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M34f_(ref Vector<M34f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M34d_(ref Vector<M34d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M44i_(ref Vector<M44i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M44l_(ref Vector<M44l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M44f_(ref Vector<M44f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_M44d_(ref Vector<M44d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C3b_(ref Vector<C3b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C3us_(ref Vector<C3us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C3ui_(ref Vector<C3ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C3f_(ref Vector<C3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C3d_(ref Vector<C3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C4b_(ref Vector<C4b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C4us_(ref Vector<C4us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C4ui_(ref Vector<C4ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C4f_(ref Vector<C4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_C4d_(ref Vector<C4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1b_(ref Vector<Range1b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1sb_(ref Vector<Range1sb> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1s_(ref Vector<Range1s> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1us_(ref Vector<Range1us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1i_(ref Vector<Range1i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1ui_(ref Vector<Range1ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1l_(ref Vector<Range1l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1ul_(ref Vector<Range1ul> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1f_(ref Vector<Range1f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Range1d_(ref Vector<Range1d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box2i_(ref Vector<Box2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box2l_(ref Vector<Box2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box2f_(ref Vector<Box2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box2d_(ref Vector<Box2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box3i_(ref Vector<Box3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box3l_(ref Vector<Box3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box3f_(ref Vector<Box3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Box3d_(ref Vector<Box3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Euclidean3f_(ref Vector<Euclidean3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Euclidean3d_(ref Vector<Euclidean3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Rot2f_(ref Vector<Rot2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Rot2d_(ref Vector<Rot2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Rot3f_(ref Vector<Rot3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Rot3d_(ref Vector<Rot3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Scale3f_(ref Vector<Scale3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Scale3d_(ref Vector<Scale3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Shift3f_(ref Vector<Shift3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Shift3d_(ref Vector<Shift3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Trafo2f_(ref Vector<Trafo2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Trafo2d_(ref Vector<Trafo2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Trafo3f_(ref Vector<Trafo3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Trafo3d_(ref Vector<Trafo3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Bool_(ref Vector<bool> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Char_(ref Vector<char> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_String_(ref Vector<string> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Type_(ref Vector<Type> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Guid_(ref Vector<Guid> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Symbol_(ref Vector<Symbol> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Circle2d_(ref Vector<Circle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Line2d_(ref Vector<Line2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Line3d_(ref Vector<Line3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Plane2d_(ref Vector<Plane2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Plane3d_(ref Vector<Plane3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_PlaneWithPoint3d_(ref Vector<PlaneWithPoint3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Quad2d_(ref Vector<Quad2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Quad3d_(ref Vector<Quad3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Ray2d_(ref Vector<Ray2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Ray3d_(ref Vector<Ray3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Sphere3d_(ref Vector<Sphere3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Triangle2d_(ref Vector<Triangle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Triangle3d_(ref Vector<Triangle3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Circle2f_(ref Vector<Circle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Line2f_(ref Vector<Line2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Line3f_(ref Vector<Line3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Plane2f_(ref Vector<Plane2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Plane3f_(ref Vector<Plane3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_PlaneWithPoint3f_(ref Vector<PlaneWithPoint3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Quad2f_(ref Vector<Quad2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Quad3f_(ref Vector<Quad3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Ray2f_(ref Vector<Ray2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Ray3f_(ref Vector<Ray3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Sphere3f_(ref Vector<Sphere3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Triangle2f_(ref Vector<Triangle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVector_of_Triangle3f_(ref Vector<Triangle3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Byte_(ref Matrix<byte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_SByte_(ref Matrix<sbyte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Short_(ref Matrix<short> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_UShort_(ref Matrix<ushort> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Int_(ref Matrix<int> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_UInt_(ref Matrix<uint> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Long_(ref Matrix<long> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_ULong_(ref Matrix<ulong> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Float_(ref Matrix<float> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Double_(ref Matrix<double> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Fraction_(ref Matrix<Fraction> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V2i_(ref Matrix<V2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V2l_(ref Matrix<V2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V2f_(ref Matrix<V2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V2d_(ref Matrix<V2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V3i_(ref Matrix<V3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V3l_(ref Matrix<V3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V3f_(ref Matrix<V3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V3d_(ref Matrix<V3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V4i_(ref Matrix<V4i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V4l_(ref Matrix<V4l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V4f_(ref Matrix<V4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_V4d_(ref Matrix<V4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M22i_(ref Matrix<M22i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M22l_(ref Matrix<M22l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M22f_(ref Matrix<M22f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M22d_(ref Matrix<M22d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M23i_(ref Matrix<M23i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M23l_(ref Matrix<M23l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M23f_(ref Matrix<M23f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M23d_(ref Matrix<M23d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M33i_(ref Matrix<M33i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M33l_(ref Matrix<M33l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M33f_(ref Matrix<M33f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M33d_(ref Matrix<M33d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M34i_(ref Matrix<M34i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M34l_(ref Matrix<M34l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M34f_(ref Matrix<M34f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M34d_(ref Matrix<M34d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M44i_(ref Matrix<M44i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M44l_(ref Matrix<M44l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M44f_(ref Matrix<M44f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_M44d_(ref Matrix<M44d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C3b_(ref Matrix<C3b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C3us_(ref Matrix<C3us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C3ui_(ref Matrix<C3ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C3f_(ref Matrix<C3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C3d_(ref Matrix<C3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C4b_(ref Matrix<C4b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C4us_(ref Matrix<C4us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C4ui_(ref Matrix<C4ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C4f_(ref Matrix<C4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_C4d_(ref Matrix<C4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1b_(ref Matrix<Range1b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1sb_(ref Matrix<Range1sb> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1s_(ref Matrix<Range1s> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1us_(ref Matrix<Range1us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1i_(ref Matrix<Range1i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1ui_(ref Matrix<Range1ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1l_(ref Matrix<Range1l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1ul_(ref Matrix<Range1ul> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1f_(ref Matrix<Range1f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Range1d_(ref Matrix<Range1d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box2i_(ref Matrix<Box2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box2l_(ref Matrix<Box2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box2f_(ref Matrix<Box2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box2d_(ref Matrix<Box2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box3i_(ref Matrix<Box3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box3l_(ref Matrix<Box3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box3f_(ref Matrix<Box3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Box3d_(ref Matrix<Box3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Euclidean3f_(ref Matrix<Euclidean3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Euclidean3d_(ref Matrix<Euclidean3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Rot2f_(ref Matrix<Rot2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Rot2d_(ref Matrix<Rot2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Rot3f_(ref Matrix<Rot3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Rot3d_(ref Matrix<Rot3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Scale3f_(ref Matrix<Scale3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Scale3d_(ref Matrix<Scale3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Shift3f_(ref Matrix<Shift3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Shift3d_(ref Matrix<Shift3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Trafo2f_(ref Matrix<Trafo2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Trafo2d_(ref Matrix<Trafo2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Trafo3f_(ref Matrix<Trafo3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Trafo3d_(ref Matrix<Trafo3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Bool_(ref Matrix<bool> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Char_(ref Matrix<char> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_String_(ref Matrix<string> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Type_(ref Matrix<Type> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Guid_(ref Matrix<Guid> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Symbol_(ref Matrix<Symbol> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Circle2d_(ref Matrix<Circle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Line2d_(ref Matrix<Line2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Line3d_(ref Matrix<Line3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Plane2d_(ref Matrix<Plane2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Plane3d_(ref Matrix<Plane3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_PlaneWithPoint3d_(ref Matrix<PlaneWithPoint3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Quad2d_(ref Matrix<Quad2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Quad3d_(ref Matrix<Quad3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Ray2d_(ref Matrix<Ray2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Ray3d_(ref Matrix<Ray3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Sphere3d_(ref Matrix<Sphere3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Triangle2d_(ref Matrix<Triangle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Triangle3d_(ref Matrix<Triangle3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Circle2f_(ref Matrix<Circle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Line2f_(ref Matrix<Line2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Line3f_(ref Matrix<Line3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Plane2f_(ref Matrix<Plane2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Plane3f_(ref Matrix<Plane3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_PlaneWithPoint3f_(ref Matrix<PlaneWithPoint3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Quad2f_(ref Matrix<Quad2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Quad3f_(ref Matrix<Quad3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Ray2f_(ref Matrix<Ray2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Ray3f_(ref Matrix<Ray3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Sphere3f_(ref Matrix<Sphere3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Triangle2f_(ref Matrix<Triangle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeMatrix_of_Triangle3f_(ref Matrix<Triangle3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Byte_(ref Volume<byte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_SByte_(ref Volume<sbyte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Short_(ref Volume<short> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_UShort_(ref Volume<ushort> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Int_(ref Volume<int> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_UInt_(ref Volume<uint> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Long_(ref Volume<long> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_ULong_(ref Volume<ulong> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Float_(ref Volume<float> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Double_(ref Volume<double> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Fraction_(ref Volume<Fraction> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V2i_(ref Volume<V2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V2l_(ref Volume<V2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V2f_(ref Volume<V2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V2d_(ref Volume<V2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V3i_(ref Volume<V3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V3l_(ref Volume<V3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V3f_(ref Volume<V3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V3d_(ref Volume<V3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V4i_(ref Volume<V4i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V4l_(ref Volume<V4l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V4f_(ref Volume<V4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_V4d_(ref Volume<V4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M22i_(ref Volume<M22i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M22l_(ref Volume<M22l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M22f_(ref Volume<M22f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M22d_(ref Volume<M22d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M23i_(ref Volume<M23i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M23l_(ref Volume<M23l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M23f_(ref Volume<M23f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M23d_(ref Volume<M23d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M33i_(ref Volume<M33i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M33l_(ref Volume<M33l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M33f_(ref Volume<M33f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M33d_(ref Volume<M33d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M34i_(ref Volume<M34i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M34l_(ref Volume<M34l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M34f_(ref Volume<M34f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M34d_(ref Volume<M34d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M44i_(ref Volume<M44i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M44l_(ref Volume<M44l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M44f_(ref Volume<M44f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_M44d_(ref Volume<M44d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C3b_(ref Volume<C3b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C3us_(ref Volume<C3us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C3ui_(ref Volume<C3ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C3f_(ref Volume<C3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C3d_(ref Volume<C3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C4b_(ref Volume<C4b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C4us_(ref Volume<C4us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C4ui_(ref Volume<C4ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C4f_(ref Volume<C4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_C4d_(ref Volume<C4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1b_(ref Volume<Range1b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1sb_(ref Volume<Range1sb> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1s_(ref Volume<Range1s> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1us_(ref Volume<Range1us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1i_(ref Volume<Range1i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1ui_(ref Volume<Range1ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1l_(ref Volume<Range1l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1ul_(ref Volume<Range1ul> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1f_(ref Volume<Range1f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Range1d_(ref Volume<Range1d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box2i_(ref Volume<Box2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box2l_(ref Volume<Box2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box2f_(ref Volume<Box2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box2d_(ref Volume<Box2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box3i_(ref Volume<Box3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box3l_(ref Volume<Box3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box3f_(ref Volume<Box3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Box3d_(ref Volume<Box3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Euclidean3f_(ref Volume<Euclidean3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Euclidean3d_(ref Volume<Euclidean3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Rot2f_(ref Volume<Rot2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Rot2d_(ref Volume<Rot2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Rot3f_(ref Volume<Rot3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Rot3d_(ref Volume<Rot3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Scale3f_(ref Volume<Scale3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Scale3d_(ref Volume<Scale3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Shift3f_(ref Volume<Shift3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Shift3d_(ref Volume<Shift3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Trafo2f_(ref Volume<Trafo2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Trafo2d_(ref Volume<Trafo2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Trafo3f_(ref Volume<Trafo3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Trafo3d_(ref Volume<Trafo3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Bool_(ref Volume<bool> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Char_(ref Volume<char> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_String_(ref Volume<string> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Type_(ref Volume<Type> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Guid_(ref Volume<Guid> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Symbol_(ref Volume<Symbol> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Circle2d_(ref Volume<Circle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Line2d_(ref Volume<Line2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Line3d_(ref Volume<Line3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Plane2d_(ref Volume<Plane2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Plane3d_(ref Volume<Plane3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_PlaneWithPoint3d_(ref Volume<PlaneWithPoint3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Quad2d_(ref Volume<Quad2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Quad3d_(ref Volume<Quad3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Ray2d_(ref Volume<Ray2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Ray3d_(ref Volume<Ray3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Sphere3d_(ref Volume<Sphere3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Triangle2d_(ref Volume<Triangle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Triangle3d_(ref Volume<Triangle3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Circle2f_(ref Volume<Circle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Line2f_(ref Volume<Line2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Line3f_(ref Volume<Line3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Plane2f_(ref Volume<Plane2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Plane3f_(ref Volume<Plane3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_PlaneWithPoint3f_(ref Volume<PlaneWithPoint3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Quad2f_(ref Volume<Quad2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Quad3f_(ref Volume<Quad3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Ray2f_(ref Volume<Ray2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Ray3f_(ref Volume<Ray3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Sphere3f_(ref Volume<Sphere3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Triangle2f_(ref Volume<Triangle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeVolume_of_Triangle3f_(ref Volume<Triangle3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Byte_(ref Tensor<byte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_SByte_(ref Tensor<sbyte> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Short_(ref Tensor<short> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_UShort_(ref Tensor<ushort> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Int_(ref Tensor<int> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_UInt_(ref Tensor<uint> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Long_(ref Tensor<long> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_ULong_(ref Tensor<ulong> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Float_(ref Tensor<float> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Double_(ref Tensor<double> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Fraction_(ref Tensor<Fraction> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V2i_(ref Tensor<V2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V2l_(ref Tensor<V2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V2f_(ref Tensor<V2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V2d_(ref Tensor<V2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V3i_(ref Tensor<V3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V3l_(ref Tensor<V3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V3f_(ref Tensor<V3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V3d_(ref Tensor<V3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V4i_(ref Tensor<V4i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V4l_(ref Tensor<V4l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V4f_(ref Tensor<V4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_V4d_(ref Tensor<V4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M22i_(ref Tensor<M22i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M22l_(ref Tensor<M22l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M22f_(ref Tensor<M22f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M22d_(ref Tensor<M22d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M23i_(ref Tensor<M23i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M23l_(ref Tensor<M23l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M23f_(ref Tensor<M23f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M23d_(ref Tensor<M23d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M33i_(ref Tensor<M33i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M33l_(ref Tensor<M33l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M33f_(ref Tensor<M33f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M33d_(ref Tensor<M33d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M34i_(ref Tensor<M34i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M34l_(ref Tensor<M34l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M34f_(ref Tensor<M34f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M34d_(ref Tensor<M34d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M44i_(ref Tensor<M44i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M44l_(ref Tensor<M44l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M44f_(ref Tensor<M44f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_M44d_(ref Tensor<M44d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C3b_(ref Tensor<C3b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C3us_(ref Tensor<C3us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C3ui_(ref Tensor<C3ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C3f_(ref Tensor<C3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C3d_(ref Tensor<C3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C4b_(ref Tensor<C4b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C4us_(ref Tensor<C4us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C4ui_(ref Tensor<C4ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C4f_(ref Tensor<C4f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_C4d_(ref Tensor<C4d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1b_(ref Tensor<Range1b> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1sb_(ref Tensor<Range1sb> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1s_(ref Tensor<Range1s> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1us_(ref Tensor<Range1us> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1i_(ref Tensor<Range1i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1ui_(ref Tensor<Range1ui> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1l_(ref Tensor<Range1l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1ul_(ref Tensor<Range1ul> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1f_(ref Tensor<Range1f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Range1d_(ref Tensor<Range1d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box2i_(ref Tensor<Box2i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box2l_(ref Tensor<Box2l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box2f_(ref Tensor<Box2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box2d_(ref Tensor<Box2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box3i_(ref Tensor<Box3i> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box3l_(ref Tensor<Box3l> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box3f_(ref Tensor<Box3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Box3d_(ref Tensor<Box3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Euclidean3f_(ref Tensor<Euclidean3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Euclidean3d_(ref Tensor<Euclidean3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Rot2f_(ref Tensor<Rot2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Rot2d_(ref Tensor<Rot2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Rot3f_(ref Tensor<Rot3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Rot3d_(ref Tensor<Rot3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Scale3f_(ref Tensor<Scale3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Scale3d_(ref Tensor<Scale3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Shift3f_(ref Tensor<Shift3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Shift3d_(ref Tensor<Shift3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Trafo2f_(ref Tensor<Trafo2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Trafo2d_(ref Tensor<Trafo2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Trafo3f_(ref Tensor<Trafo3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Trafo3d_(ref Tensor<Trafo3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Bool_(ref Tensor<bool> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Char_(ref Tensor<char> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_String_(ref Tensor<string> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Type_(ref Tensor<Type> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Guid_(ref Tensor<Guid> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Symbol_(ref Tensor<Symbol> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Circle2d_(ref Tensor<Circle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Line2d_(ref Tensor<Line2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Line3d_(ref Tensor<Line3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Plane2d_(ref Tensor<Plane2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Plane3d_(ref Tensor<Plane3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_PlaneWithPoint3d_(ref Tensor<PlaneWithPoint3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Quad2d_(ref Tensor<Quad2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Quad3d_(ref Tensor<Quad3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Ray2d_(ref Tensor<Ray2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Ray3d_(ref Tensor<Ray3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Sphere3d_(ref Tensor<Sphere3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Triangle2d_(ref Tensor<Triangle2d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Triangle3d_(ref Tensor<Triangle3d> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Circle2f_(ref Tensor<Circle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Line2f_(ref Tensor<Line2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Line3f_(ref Tensor<Line3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Plane2f_(ref Tensor<Plane2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Plane3f_(ref Tensor<Plane3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_PlaneWithPoint3f_(ref Tensor<PlaneWithPoint3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Quad2f_(ref Tensor<Quad2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Quad3f_(ref Tensor<Quad3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Ray2f_(ref Tensor<Ray2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Ray3f_(ref Tensor<Ray3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Sphere3f_(ref Tensor<Sphere3f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Triangle2f_(ref Tensor<Triangle2f> value)
    {
        throw new NotImplementedException();
    }

    public void CodeTensor_of_Triangle3f_(ref Tensor<Triangle3f> value)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Arrays

    public void CodeV2iArray(ref V2i[] v) { throw new NotImplementedException(); }
    public void CodeV2uiArray(ref V2ui[] v) { throw new NotImplementedException(); }
    public void CodeV2lArray(ref V2l[] v) { throw new NotImplementedException(); }
    public void CodeV2fArray(ref V2f[] v) { throw new NotImplementedException(); }
    public void CodeV2dArray(ref V2d[] v) { throw new NotImplementedException(); }
    public void CodeV3iArray(ref V3i[] v) { throw new NotImplementedException(); }
    public void CodeV3uiArray(ref V3ui[] v) { throw new NotImplementedException(); }
    public void CodeV3lArray(ref V3l[] v) { throw new NotImplementedException(); }
    public void CodeV3fArray(ref V3f[] v) { throw new NotImplementedException(); }
    public void CodeV3dArray(ref V3d[] v) { throw new NotImplementedException(); }
    public void CodeV4iArray(ref V4i[] v) { throw new NotImplementedException(); }
    public void CodeV4uiArray(ref V4ui[] v) { throw new NotImplementedException(); }
    public void CodeV4lArray(ref V4l[] v) { throw new NotImplementedException(); }
    public void CodeV4fArray(ref V4f[] v) { throw new NotImplementedException(); }
    public void CodeV4dArray(ref V4d[] v) { throw new NotImplementedException(); }
    public void CodeM22iArray(ref M22i[] v) { throw new NotImplementedException(); }
    public void CodeM22lArray(ref M22l[] v) { throw new NotImplementedException(); }
    public void CodeM22fArray(ref M22f[] v) { throw new NotImplementedException(); }
    public void CodeM22dArray(ref M22d[] v) { throw new NotImplementedException(); }
    public void CodeM23iArray(ref M23i[] v) { throw new NotImplementedException(); }
    public void CodeM23lArray(ref M23l[] v) { throw new NotImplementedException(); }
    public void CodeM23fArray(ref M23f[] v) { throw new NotImplementedException(); }
    public void CodeM23dArray(ref M23d[] v) { throw new NotImplementedException(); }
    public void CodeM33iArray(ref M33i[] v) { throw new NotImplementedException(); }
    public void CodeM33lArray(ref M33l[] v) { throw new NotImplementedException(); }
    public void CodeM33fArray(ref M33f[] v) { throw new NotImplementedException(); }
    public void CodeM33dArray(ref M33d[] v) { throw new NotImplementedException(); }
    public void CodeM34iArray(ref M34i[] v) { throw new NotImplementedException(); }
    public void CodeM34lArray(ref M34l[] v) { throw new NotImplementedException(); }
    public void CodeM34fArray(ref M34f[] v) { throw new NotImplementedException(); }
    public void CodeM34dArray(ref M34d[] v) { throw new NotImplementedException(); }
    public void CodeM44iArray(ref M44i[] v) { throw new NotImplementedException(); }
    public void CodeM44lArray(ref M44l[] v) { throw new NotImplementedException(); }
    public void CodeM44fArray(ref M44f[] v) { throw new NotImplementedException(); }
    public void CodeM44dArray(ref M44d[] v) { throw new NotImplementedException(); }
    public void CodeRange1bArray(ref Range1b[] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArray(ref Range1sb[] v) { throw new NotImplementedException(); }
    public void CodeRange1sArray(ref Range1s[] v) { throw new NotImplementedException(); }
    public void CodeRange1usArray(ref Range1us[] v) { throw new NotImplementedException(); }
    public void CodeRange1iArray(ref Range1i[] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArray(ref Range1ui[] v) { throw new NotImplementedException(); }
    public void CodeRange1lArray(ref Range1l[] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArray(ref Range1ul[] v) { throw new NotImplementedException(); }
    public void CodeRange1fArray(ref Range1f[] v) { throw new NotImplementedException(); }
    public void CodeRange1dArray(ref Range1d[] v) { throw new NotImplementedException(); }
    public void CodeBox2iArray(ref Box2i[] v) { throw new NotImplementedException(); }
    public void CodeBox2lArray(ref Box2l[] v) { throw new NotImplementedException(); }
    public void CodeBox2fArray(ref Box2f[] v) { throw new NotImplementedException(); }
    public void CodeBox2dArray(ref Box2d[] v) { throw new NotImplementedException(); }
    public void CodeBox3iArray(ref Box3i[] v) { throw new NotImplementedException(); }
    public void CodeBox3lArray(ref Box3l[] v) { throw new NotImplementedException(); }
    public void CodeBox3fArray(ref Box3f[] v) { throw new NotImplementedException(); }
    public void CodeBox3dArray(ref Box3d[] v) { throw new NotImplementedException(); }
    public void CodeC3bArray(ref C3b[] v) { throw new NotImplementedException(); }
    public void CodeC3usArray(ref C3us[] v) { throw new NotImplementedException(); }
    public void CodeC3uiArray(ref C3ui[] v) { throw new NotImplementedException(); }
    public void CodeC3fArray(ref C3f[] v) { throw new NotImplementedException(); }
    public void CodeC3dArray(ref C3d[] v) { throw new NotImplementedException(); }
    public void CodeC4bArray(ref C4b[] v) { throw new NotImplementedException(); }
    public void CodeC4usArray(ref C4us[] v) { throw new NotImplementedException(); }
    public void CodeC4uiArray(ref C4ui[] v) { throw new NotImplementedException(); }
    public void CodeC4fArray(ref C4f[] v) { throw new NotImplementedException(); }
    public void CodeC4dArray(ref C4d[] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArray(ref Euclidean3f[] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArray(ref Euclidean3d[] v) { throw new NotImplementedException(); }
    public void CodeRot2fArray(ref Rot2f[] v) { throw new NotImplementedException(); }
    public void CodeRot2dArray(ref Rot2d[] v) { throw new NotImplementedException(); }
    public void CodeRot3fArray(ref Rot3f[] v) { throw new NotImplementedException(); }
    public void CodeRot3dArray(ref Rot3d[] v) { throw new NotImplementedException(); }
    public void CodeScale3fArray(ref Scale3f[] v) { throw new NotImplementedException(); }
    public void CodeScale3dArray(ref Scale3d[] v) { throw new NotImplementedException(); }
    public void CodeShift3fArray(ref Shift3f[] v) { throw new NotImplementedException(); }
    public void CodeShift3dArray(ref Shift3d[] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArray(ref Trafo2f[] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArray(ref Trafo2d[] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArray(ref Trafo3f[] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArray(ref Trafo3d[] v) { throw new NotImplementedException(); }
    public void CodeCircle2dArray(ref Circle2d[] v) { throw new NotImplementedException(); }
    public void CodeLine2dArray(ref Line2d[] v) { throw new NotImplementedException(); }
    public void CodeLine3dArray(ref Line3d[] v) { throw new NotImplementedException(); }
    public void CodePlane2dArray(ref Plane2d[] v) { throw new NotImplementedException(); }
    public void CodePlane3dArray(ref Plane3d[] v) { throw new NotImplementedException(); }
    public void CodePlaneWithPoint3dArray(ref PlaneWithPoint3d[] v) { throw new NotImplementedException(); }
    public void CodeQuad2dArray(ref Quad2d[] v) { throw new NotImplementedException(); }
    public void CodeQuad3dArray(ref Quad3d[] v) { throw new NotImplementedException(); }
    public void CodeRay2dArray(ref Ray2d[] v) { throw new NotImplementedException(); }
    public void CodeRay3dArray(ref Ray3d[] v) { throw new NotImplementedException(); }
    public void CodeSphere3dArray(ref Sphere3d[] v) { throw new NotImplementedException(); }
    public void CodeTriangle2dArray(ref Triangle2d[] v) { throw new NotImplementedException(); }
    public void CodeTriangle3dArray(ref Triangle3d[] v) { throw new NotImplementedException(); }
    public void CodeCircle2fArray(ref Circle2f[] v) { throw new NotImplementedException(); }
    public void CodeLine2fArray(ref Line2f[] v) { throw new NotImplementedException(); }
    public void CodeLine3fArray(ref Line3f[] v) { throw new NotImplementedException(); }
    public void CodePlane2fArray(ref Plane2f[] v) { throw new NotImplementedException(); }
    public void CodePlane3fArray(ref Plane3f[] v) { throw new NotImplementedException(); }
    public void CodePlaneWithPoint3fArray(ref PlaneWithPoint3f[] v) { throw new NotImplementedException(); }
    public void CodeQuad2fArray(ref Quad2f[] v) { throw new NotImplementedException(); }
    public void CodeQuad3fArray(ref Quad3f[] v) { throw new NotImplementedException(); }
    public void CodeRay2fArray(ref Ray2f[] v) { throw new NotImplementedException(); }
    public void CodeRay3fArray(ref Ray3f[] v) { throw new NotImplementedException(); }
    public void CodeSphere3fArray(ref Sphere3f[] v) { throw new NotImplementedException(); }
    public void CodeTriangle2fArray(ref Triangle2f[] v) { throw new NotImplementedException(); }
    public void CodeTriangle3fArray(ref Triangle3f[] v) { throw new NotImplementedException(); }

    #endregion

    #region Multi-Dimensional Arrays

    public void CodeByteArray2d(ref byte[,] v) { throw new NotImplementedException(); }
    public void CodeByteArray3d(ref byte[, ,] v) { throw new NotImplementedException(); }
    public void CodeSByteArray2d(ref sbyte[,] v) { throw new NotImplementedException(); }
    public void CodeSByteArray3d(ref sbyte[, ,] v) { throw new NotImplementedException(); }
    public void CodeShortArray2d(ref short[,] v) { throw new NotImplementedException(); }
    public void CodeShortArray3d(ref short[, ,] v) { throw new NotImplementedException(); }
    public void CodeUShortArray2d(ref ushort[,] v) { throw new NotImplementedException(); }
    public void CodeUShortArray3d(ref ushort[, ,] v) { throw new NotImplementedException(); }
    public void CodeIntArray2d(ref int[,] v) { throw new NotImplementedException(); }
    public void CodeIntArray3d(ref int[, ,] v) { throw new NotImplementedException(); }
    public void CodeUIntArray2d(ref uint[,] v) { throw new NotImplementedException(); }
    public void CodeUIntArray3d(ref uint[, ,] v) { throw new NotImplementedException(); }
    public void CodeLongArray2d(ref long[,] v) { throw new NotImplementedException(); }
    public void CodeLongArray3d(ref long[, ,] v) { throw new NotImplementedException(); }
    public void CodeULongArray2d(ref ulong[,] v) { throw new NotImplementedException(); }
    public void CodeULongArray3d(ref ulong[, ,] v) { throw new NotImplementedException(); }
    public void CodeFloatArray2d(ref float[,] v) { throw new NotImplementedException(); }
    public void CodeFloatArray3d(ref float[, ,] v) { throw new NotImplementedException(); }
    public void CodeDoubleArray2d(ref double[,] v) { throw new NotImplementedException(); }
    public void CodeDoubleArray3d(ref double[, ,] v) { throw new NotImplementedException(); }
    public void CodeFractionArray2d(ref Fraction[,] v) { throw new NotImplementedException(); }
    public void CodeFractionArray3d(ref Fraction[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2iArray2d(ref V2i[,] v) { throw new NotImplementedException(); }
    public void CodeV2iArray3d(ref V2i[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2lArray2d(ref V2l[,] v) { throw new NotImplementedException(); }
    public void CodeV2lArray3d(ref V2l[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2fArray2d(ref V2f[,] v) { throw new NotImplementedException(); }
    public void CodeV2fArray3d(ref V2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2dArray2d(ref V2d[,] v) { throw new NotImplementedException(); }
    public void CodeV2dArray3d(ref V2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3iArray2d(ref V3i[,] v) { throw new NotImplementedException(); }
    public void CodeV3iArray3d(ref V3i[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3lArray2d(ref V3l[,] v) { throw new NotImplementedException(); }
    public void CodeV3lArray3d(ref V3l[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3fArray2d(ref V3f[,] v) { throw new NotImplementedException(); }
    public void CodeV3fArray3d(ref V3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3dArray2d(ref V3d[,] v) { throw new NotImplementedException(); }
    public void CodeV3dArray3d(ref V3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4iArray2d(ref V4i[,] v) { throw new NotImplementedException(); }
    public void CodeV4iArray3d(ref V4i[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4lArray2d(ref V4l[,] v) { throw new NotImplementedException(); }
    public void CodeV4lArray3d(ref V4l[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4fArray2d(ref V4f[,] v) { throw new NotImplementedException(); }
    public void CodeV4fArray3d(ref V4f[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4dArray2d(ref V4d[,] v) { throw new NotImplementedException(); }
    public void CodeV4dArray3d(ref V4d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22iArray2d(ref M22i[,] v) { throw new NotImplementedException(); }
    public void CodeM22iArray3d(ref M22i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22lArray2d(ref M22l[,] v) { throw new NotImplementedException(); }
    public void CodeM22lArray3d(ref M22l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22fArray2d(ref M22f[,] v) { throw new NotImplementedException(); }
    public void CodeM22fArray3d(ref M22f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22dArray2d(ref M22d[,] v) { throw new NotImplementedException(); }
    public void CodeM22dArray3d(ref M22d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23iArray2d(ref M23i[,] v) { throw new NotImplementedException(); }
    public void CodeM23iArray3d(ref M23i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23lArray2d(ref M23l[,] v) { throw new NotImplementedException(); }
    public void CodeM23lArray3d(ref M23l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23fArray2d(ref M23f[,] v) { throw new NotImplementedException(); }
    public void CodeM23fArray3d(ref M23f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23dArray2d(ref M23d[,] v) { throw new NotImplementedException(); }
    public void CodeM23dArray3d(ref M23d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33iArray2d(ref M33i[,] v) { throw new NotImplementedException(); }
    public void CodeM33iArray3d(ref M33i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33lArray2d(ref M33l[,] v) { throw new NotImplementedException(); }
    public void CodeM33lArray3d(ref M33l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33fArray2d(ref M33f[,] v) { throw new NotImplementedException(); }
    public void CodeM33fArray3d(ref M33f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33dArray2d(ref M33d[,] v) { throw new NotImplementedException(); }
    public void CodeM33dArray3d(ref M33d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34iArray2d(ref M34i[,] v) { throw new NotImplementedException(); }
    public void CodeM34iArray3d(ref M34i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34lArray2d(ref M34l[,] v) { throw new NotImplementedException(); }
    public void CodeM34lArray3d(ref M34l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34fArray2d(ref M34f[,] v) { throw new NotImplementedException(); }
    public void CodeM34fArray3d(ref M34f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34dArray2d(ref M34d[,] v) { throw new NotImplementedException(); }
    public void CodeM34dArray3d(ref M34d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44iArray2d(ref M44i[,] v) { throw new NotImplementedException(); }
    public void CodeM44iArray3d(ref M44i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44lArray2d(ref M44l[,] v) { throw new NotImplementedException(); }
    public void CodeM44lArray3d(ref M44l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44fArray2d(ref M44f[,] v) { throw new NotImplementedException(); }
    public void CodeM44fArray3d(ref M44f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44dArray2d(ref M44d[,] v) { throw new NotImplementedException(); }
    public void CodeM44dArray3d(ref M44d[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3bArray2d(ref C3b[,] v) { throw new NotImplementedException(); }
    public void CodeC3bArray3d(ref C3b[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3usArray2d(ref C3us[,] v) { throw new NotImplementedException(); }
    public void CodeC3usArray3d(ref C3us[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3uiArray2d(ref C3ui[,] v) { throw new NotImplementedException(); }
    public void CodeC3uiArray3d(ref C3ui[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3fArray2d(ref C3f[,] v) { throw new NotImplementedException(); }
    public void CodeC3fArray3d(ref C3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3dArray2d(ref C3d[,] v) { throw new NotImplementedException(); }
    public void CodeC3dArray3d(ref C3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4bArray2d(ref C4b[,] v) { throw new NotImplementedException(); }
    public void CodeC4bArray3d(ref C4b[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4usArray2d(ref C4us[,] v) { throw new NotImplementedException(); }
    public void CodeC4usArray3d(ref C4us[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4uiArray2d(ref C4ui[,] v) { throw new NotImplementedException(); }
    public void CodeC4uiArray3d(ref C4ui[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4fArray2d(ref C4f[,] v) { throw new NotImplementedException(); }
    public void CodeC4fArray3d(ref C4f[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4dArray2d(ref C4d[,] v) { throw new NotImplementedException(); }
    public void CodeC4dArray3d(ref C4d[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1bArray2d(ref Range1b[,] v) { throw new NotImplementedException(); }
    public void CodeRange1bArray3d(ref Range1b[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArray2d(ref Range1sb[,] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArray3d(ref Range1sb[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1sArray2d(ref Range1s[,] v) { throw new NotImplementedException(); }
    public void CodeRange1sArray3d(ref Range1s[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1usArray2d(ref Range1us[,] v) { throw new NotImplementedException(); }
    public void CodeRange1usArray3d(ref Range1us[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1iArray2d(ref Range1i[,] v) { throw new NotImplementedException(); }
    public void CodeRange1iArray3d(ref Range1i[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArray2d(ref Range1ui[,] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArray3d(ref Range1ui[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1lArray2d(ref Range1l[,] v) { throw new NotImplementedException(); }
    public void CodeRange1lArray3d(ref Range1l[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArray2d(ref Range1ul[,] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArray3d(ref Range1ul[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1fArray2d(ref Range1f[,] v) { throw new NotImplementedException(); }
    public void CodeRange1fArray3d(ref Range1f[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1dArray2d(ref Range1d[,] v) { throw new NotImplementedException(); }
    public void CodeRange1dArray3d(ref Range1d[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2iArray2d(ref Box2i[,] v) { throw new NotImplementedException(); }
    public void CodeBox2iArray3d(ref Box2i[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2lArray2d(ref Box2l[,] v) { throw new NotImplementedException(); }
    public void CodeBox2lArray3d(ref Box2l[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2fArray2d(ref Box2f[,] v) { throw new NotImplementedException(); }
    public void CodeBox2fArray3d(ref Box2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2dArray2d(ref Box2d[,] v) { throw new NotImplementedException(); }
    public void CodeBox2dArray3d(ref Box2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3iArray2d(ref Box3i[,] v) { throw new NotImplementedException(); }
    public void CodeBox3iArray3d(ref Box3i[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3lArray2d(ref Box3l[,] v) { throw new NotImplementedException(); }
    public void CodeBox3lArray3d(ref Box3l[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3fArray2d(ref Box3f[,] v) { throw new NotImplementedException(); }
    public void CodeBox3fArray3d(ref Box3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3dArray2d(ref Box3d[,] v) { throw new NotImplementedException(); }
    public void CodeBox3dArray3d(ref Box3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArray2d(ref Euclidean3f[,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArray3d(ref Euclidean3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArray2d(ref Euclidean3d[,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArray3d(ref Euclidean3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot2fArray2d(ref Rot2f[,] v) { throw new NotImplementedException(); }
    public void CodeRot2fArray3d(ref Rot2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot2dArray2d(ref Rot2d[,] v) { throw new NotImplementedException(); }
    public void CodeRot2dArray3d(ref Rot2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot3fArray2d(ref Rot3f[,] v) { throw new NotImplementedException(); }
    public void CodeRot3fArray3d(ref Rot3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot3dArray2d(ref Rot3d[,] v) { throw new NotImplementedException(); }
    public void CodeRot3dArray3d(ref Rot3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeScale3fArray2d(ref Scale3f[,] v) { throw new NotImplementedException(); }
    public void CodeScale3fArray3d(ref Scale3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeScale3dArray2d(ref Scale3d[,] v) { throw new NotImplementedException(); }
    public void CodeScale3dArray3d(ref Scale3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeShift3fArray2d(ref Shift3f[,] v) { throw new NotImplementedException(); }
    public void CodeShift3fArray3d(ref Shift3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeShift3dArray2d(ref Shift3d[,] v) { throw new NotImplementedException(); }
    public void CodeShift3dArray3d(ref Shift3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArray2d(ref Trafo2f[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArray3d(ref Trafo2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArray2d(ref Trafo2d[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArray3d(ref Trafo2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArray2d(ref Trafo3f[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArray3d(ref Trafo3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArray2d(ref Trafo3d[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArray3d(ref Trafo3d[, ,] v) { throw new NotImplementedException(); }

    #endregion

    #region Jagged Multi-Dimensional Arrays

    public void CodeByteArrayArray(ref byte[][] v) { throw new NotImplementedException(); }
    public void CodeByteArrayArrayArray(ref byte[][][] v) { throw new NotImplementedException(); }
    public void CodeSByteArrayArray(ref sbyte[][] v) { throw new NotImplementedException(); }
    public void CodeSByteArrayArrayArray(ref sbyte[][][] v) { throw new NotImplementedException(); }
    public void CodeShortArrayArray(ref short[][] v) { throw new NotImplementedException(); }
    public void CodeShortArrayArrayArray(ref short[][][] v) { throw new NotImplementedException(); }
    public void CodeUShortArrayArray(ref ushort[][] v) { throw new NotImplementedException(); }
    public void CodeUShortArrayArrayArray(ref ushort[][][] v) { throw new NotImplementedException(); }
    public void CodeIntArrayArray(ref int[][] v) { throw new NotImplementedException(); }
    public void CodeIntArrayArrayArray(ref int[][][] v) { throw new NotImplementedException(); }
    public void CodeUIntArrayArray(ref uint[][] v) { throw new NotImplementedException(); }
    public void CodeUIntArrayArrayArray(ref uint[][][] v) { throw new NotImplementedException(); }
    public void CodeLongArrayArray(ref long[][] v) { throw new NotImplementedException(); }
    public void CodeLongArrayArrayArray(ref long[][][] v) { throw new NotImplementedException(); }
    public void CodeULongArrayArray(ref ulong[][] v) { throw new NotImplementedException(); }
    public void CodeULongArrayArrayArray(ref ulong[][][] v) { throw new NotImplementedException(); }
    public void CodeFloatArrayArray(ref float[][] v) { throw new NotImplementedException(); }
    public void CodeFloatArrayArrayArray(ref float[][][] v) { throw new NotImplementedException(); }
    public void CodeDoubleArrayArray(ref double[][] v) { throw new NotImplementedException(); }
    public void CodeDoubleArrayArrayArray(ref double[][][] v) { throw new NotImplementedException(); }
    public void CodeFractionArrayArray(ref Fraction[][] v) { throw new NotImplementedException(); }
    public void CodeFractionArrayArrayArray(ref Fraction[][][] v) { throw new NotImplementedException(); }
    public void CodeV2iArrayArray(ref V2i[][] v) { throw new NotImplementedException(); }
    public void CodeV2iArrayArrayArray(ref V2i[][][] v) { throw new NotImplementedException(); }
    public void CodeV2lArrayArray(ref V2l[][] v) { throw new NotImplementedException(); }
    public void CodeV2lArrayArrayArray(ref V2l[][][] v) { throw new NotImplementedException(); }
    public void CodeV2fArrayArray(ref V2f[][] v) { throw new NotImplementedException(); }
    public void CodeV2fArrayArrayArray(ref V2f[][][] v) { throw new NotImplementedException(); }
    public void CodeV2dArrayArray(ref V2d[][] v) { throw new NotImplementedException(); }
    public void CodeV2dArrayArrayArray(ref V2d[][][] v) { throw new NotImplementedException(); }
    public void CodeV3iArrayArray(ref V3i[][] v) { throw new NotImplementedException(); }
    public void CodeV3iArrayArrayArray(ref V3i[][][] v) { throw new NotImplementedException(); }
    public void CodeV3lArrayArray(ref V3l[][] v) { throw new NotImplementedException(); }
    public void CodeV3lArrayArrayArray(ref V3l[][][] v) { throw new NotImplementedException(); }
    public void CodeV3fArrayArray(ref V3f[][] v) { throw new NotImplementedException(); }
    public void CodeV3fArrayArrayArray(ref V3f[][][] v) { throw new NotImplementedException(); }
    public void CodeV3dArrayArray(ref V3d[][] v) { throw new NotImplementedException(); }
    public void CodeV3dArrayArrayArray(ref V3d[][][] v) { throw new NotImplementedException(); }
    public void CodeV4iArrayArray(ref V4i[][] v) { throw new NotImplementedException(); }
    public void CodeV4iArrayArrayArray(ref V4i[][][] v) { throw new NotImplementedException(); }
    public void CodeV4lArrayArray(ref V4l[][] v) { throw new NotImplementedException(); }
    public void CodeV4lArrayArrayArray(ref V4l[][][] v) { throw new NotImplementedException(); }
    public void CodeV4fArrayArray(ref V4f[][] v) { throw new NotImplementedException(); }
    public void CodeV4fArrayArrayArray(ref V4f[][][] v) { throw new NotImplementedException(); }
    public void CodeV4dArrayArray(ref V4d[][] v) { throw new NotImplementedException(); }
    public void CodeV4dArrayArrayArray(ref V4d[][][] v) { throw new NotImplementedException(); }
    public void CodeM22iArrayArray(ref M22i[][] v) { throw new NotImplementedException(); }
    public void CodeM22iArrayArrayArray(ref M22i[][][] v) { throw new NotImplementedException(); }
    public void CodeM22lArrayArray(ref M22l[][] v) { throw new NotImplementedException(); }
    public void CodeM22lArrayArrayArray(ref M22l[][][] v) { throw new NotImplementedException(); }
    public void CodeM22fArrayArray(ref M22f[][] v) { throw new NotImplementedException(); }
    public void CodeM22fArrayArrayArray(ref M22f[][][] v) { throw new NotImplementedException(); }
    public void CodeM22dArrayArray(ref M22d[][] v) { throw new NotImplementedException(); }
    public void CodeM22dArrayArrayArray(ref M22d[][][] v) { throw new NotImplementedException(); }
    public void CodeM23iArrayArray(ref M23i[][] v) { throw new NotImplementedException(); }
    public void CodeM23iArrayArrayArray(ref M23i[][][] v) { throw new NotImplementedException(); }
    public void CodeM23lArrayArray(ref M23l[][] v) { throw new NotImplementedException(); }
    public void CodeM23lArrayArrayArray(ref M23l[][][] v) { throw new NotImplementedException(); }
    public void CodeM23fArrayArray(ref M23f[][] v) { throw new NotImplementedException(); }
    public void CodeM23fArrayArrayArray(ref M23f[][][] v) { throw new NotImplementedException(); }
    public void CodeM23dArrayArray(ref M23d[][] v) { throw new NotImplementedException(); }
    public void CodeM23dArrayArrayArray(ref M23d[][][] v) { throw new NotImplementedException(); }
    public void CodeM33iArrayArray(ref M33i[][] v) { throw new NotImplementedException(); }
    public void CodeM33iArrayArrayArray(ref M33i[][][] v) { throw new NotImplementedException(); }
    public void CodeM33lArrayArray(ref M33l[][] v) { throw new NotImplementedException(); }
    public void CodeM33lArrayArrayArray(ref M33l[][][] v) { throw new NotImplementedException(); }
    public void CodeM33fArrayArray(ref M33f[][] v) { throw new NotImplementedException(); }
    public void CodeM33fArrayArrayArray(ref M33f[][][] v) { throw new NotImplementedException(); }
    public void CodeM33dArrayArray(ref M33d[][] v) { throw new NotImplementedException(); }
    public void CodeM33dArrayArrayArray(ref M33d[][][] v) { throw new NotImplementedException(); }
    public void CodeM34iArrayArray(ref M34i[][] v) { throw new NotImplementedException(); }
    public void CodeM34iArrayArrayArray(ref M34i[][][] v) { throw new NotImplementedException(); }
    public void CodeM34lArrayArray(ref M34l[][] v) { throw new NotImplementedException(); }
    public void CodeM34lArrayArrayArray(ref M34l[][][] v) { throw new NotImplementedException(); }
    public void CodeM34fArrayArray(ref M34f[][] v) { throw new NotImplementedException(); }
    public void CodeM34fArrayArrayArray(ref M34f[][][] v) { throw new NotImplementedException(); }
    public void CodeM34dArrayArray(ref M34d[][] v) { throw new NotImplementedException(); }
    public void CodeM34dArrayArrayArray(ref M34d[][][] v) { throw new NotImplementedException(); }
    public void CodeM44iArrayArray(ref M44i[][] v) { throw new NotImplementedException(); }
    public void CodeM44iArrayArrayArray(ref M44i[][][] v) { throw new NotImplementedException(); }
    public void CodeM44lArrayArray(ref M44l[][] v) { throw new NotImplementedException(); }
    public void CodeM44lArrayArrayArray(ref M44l[][][] v) { throw new NotImplementedException(); }
    public void CodeM44fArrayArray(ref M44f[][] v) { throw new NotImplementedException(); }
    public void CodeM44fArrayArrayArray(ref M44f[][][] v) { throw new NotImplementedException(); }
    public void CodeM44dArrayArray(ref M44d[][] v) { throw new NotImplementedException(); }
    public void CodeM44dArrayArrayArray(ref M44d[][][] v) { throw new NotImplementedException(); }
    public void CodeC3bArrayArray(ref C3b[][] v) { throw new NotImplementedException(); }
    public void CodeC3bArrayArrayArray(ref C3b[][][] v) { throw new NotImplementedException(); }
    public void CodeC3usArrayArray(ref C3us[][] v) { throw new NotImplementedException(); }
    public void CodeC3usArrayArrayArray(ref C3us[][][] v) { throw new NotImplementedException(); }
    public void CodeC3uiArrayArray(ref C3ui[][] v) { throw new NotImplementedException(); }
    public void CodeC3uiArrayArrayArray(ref C3ui[][][] v) { throw new NotImplementedException(); }
    public void CodeC3fArrayArray(ref C3f[][] v) { throw new NotImplementedException(); }
    public void CodeC3fArrayArrayArray(ref C3f[][][] v) { throw new NotImplementedException(); }
    public void CodeC3dArrayArray(ref C3d[][] v) { throw new NotImplementedException(); }
    public void CodeC3dArrayArrayArray(ref C3d[][][] v) { throw new NotImplementedException(); }
    public void CodeC4bArrayArray(ref C4b[][] v) { throw new NotImplementedException(); }
    public void CodeC4bArrayArrayArray(ref C4b[][][] v) { throw new NotImplementedException(); }
    public void CodeC4usArrayArray(ref C4us[][] v) { throw new NotImplementedException(); }
    public void CodeC4usArrayArrayArray(ref C4us[][][] v) { throw new NotImplementedException(); }
    public void CodeC4uiArrayArray(ref C4ui[][] v) { throw new NotImplementedException(); }
    public void CodeC4uiArrayArrayArray(ref C4ui[][][] v) { throw new NotImplementedException(); }
    public void CodeC4fArrayArray(ref C4f[][] v) { throw new NotImplementedException(); }
    public void CodeC4fArrayArrayArray(ref C4f[][][] v) { throw new NotImplementedException(); }
    public void CodeC4dArrayArray(ref C4d[][] v) { throw new NotImplementedException(); }
    public void CodeC4dArrayArrayArray(ref C4d[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1bArrayArray(ref Range1b[][] v) { throw new NotImplementedException(); }
    public void CodeRange1bArrayArrayArray(ref Range1b[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArrayArray(ref Range1sb[][] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArrayArrayArray(ref Range1sb[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1sArrayArray(ref Range1s[][] v) { throw new NotImplementedException(); }
    public void CodeRange1sArrayArrayArray(ref Range1s[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1usArrayArray(ref Range1us[][] v) { throw new NotImplementedException(); }
    public void CodeRange1usArrayArrayArray(ref Range1us[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1iArrayArray(ref Range1i[][] v) { throw new NotImplementedException(); }
    public void CodeRange1iArrayArrayArray(ref Range1i[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArrayArray(ref Range1ui[][] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArrayArrayArray(ref Range1ui[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1lArrayArray(ref Range1l[][] v) { throw new NotImplementedException(); }
    public void CodeRange1lArrayArrayArray(ref Range1l[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArrayArray(ref Range1ul[][] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArrayArrayArray(ref Range1ul[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1fArrayArray(ref Range1f[][] v) { throw new NotImplementedException(); }
    public void CodeRange1fArrayArrayArray(ref Range1f[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1dArrayArray(ref Range1d[][] v) { throw new NotImplementedException(); }
    public void CodeRange1dArrayArrayArray(ref Range1d[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2iArrayArray(ref Box2i[][] v) { throw new NotImplementedException(); }
    public void CodeBox2iArrayArrayArray(ref Box2i[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2lArrayArray(ref Box2l[][] v) { throw new NotImplementedException(); }
    public void CodeBox2lArrayArrayArray(ref Box2l[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2fArrayArray(ref Box2f[][] v) { throw new NotImplementedException(); }
    public void CodeBox2fArrayArrayArray(ref Box2f[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2dArrayArray(ref Box2d[][] v) { throw new NotImplementedException(); }
    public void CodeBox2dArrayArrayArray(ref Box2d[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3iArrayArray(ref Box3i[][] v) { throw new NotImplementedException(); }
    public void CodeBox3iArrayArrayArray(ref Box3i[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3lArrayArray(ref Box3l[][] v) { throw new NotImplementedException(); }
    public void CodeBox3lArrayArrayArray(ref Box3l[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3fArrayArray(ref Box3f[][] v) { throw new NotImplementedException(); }
    public void CodeBox3fArrayArrayArray(ref Box3f[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3dArrayArray(ref Box3d[][] v) { throw new NotImplementedException(); }
    public void CodeBox3dArrayArrayArray(ref Box3d[][][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArrayArray(ref Euclidean3f[][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArrayArrayArray(ref Euclidean3f[][][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArrayArray(ref Euclidean3d[][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArrayArrayArray(ref Euclidean3d[][][] v) { throw new NotImplementedException(); }
    public void CodeRot2fArrayArray(ref Rot2f[][] v) { throw new NotImplementedException(); }
    public void CodeRot2fArrayArrayArray(ref Rot2f[][][] v) { throw new NotImplementedException(); }
    public void CodeRot2dArrayArray(ref Rot2d[][] v) { throw new NotImplementedException(); }
    public void CodeRot2dArrayArrayArray(ref Rot2d[][][] v) { throw new NotImplementedException(); }
    public void CodeRot3fArrayArray(ref Rot3f[][] v) { throw new NotImplementedException(); }
    public void CodeRot3fArrayArrayArray(ref Rot3f[][][] v) { throw new NotImplementedException(); }
    public void CodeRot3dArrayArray(ref Rot3d[][] v) { throw new NotImplementedException(); }
    public void CodeRot3dArrayArrayArray(ref Rot3d[][][] v) { throw new NotImplementedException(); }
    public void CodeScale3fArrayArray(ref Scale3f[][] v) { throw new NotImplementedException(); }
    public void CodeScale3fArrayArrayArray(ref Scale3f[][][] v) { throw new NotImplementedException(); }
    public void CodeScale3dArrayArray(ref Scale3d[][] v) { throw new NotImplementedException(); }
    public void CodeScale3dArrayArrayArray(ref Scale3d[][][] v) { throw new NotImplementedException(); }
    public void CodeShift3fArrayArray(ref Shift3f[][] v) { throw new NotImplementedException(); }
    public void CodeShift3fArrayArrayArray(ref Shift3f[][][] v) { throw new NotImplementedException(); }
    public void CodeShift3dArrayArray(ref Shift3d[][] v) { throw new NotImplementedException(); }
    public void CodeShift3dArrayArrayArray(ref Shift3d[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArrayArray(ref Trafo2f[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArrayArrayArray(ref Trafo2f[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArrayArray(ref Trafo2d[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArrayArrayArray(ref Trafo2d[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArrayArray(ref Trafo3f[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArrayArrayArray(ref Trafo3f[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArrayArray(ref Trafo3d[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArrayArrayArray(ref Trafo3d[][][] v) { throw new NotImplementedException(); }

    #endregion

    #region Lists

    public void CodeList_of_V2i_(ref List<V2i> v) { throw new NotImplementedException(); }
    public void CodeList_of_V2ui_(ref List<V2ui> v) { throw new NotImplementedException(); }
    public void CodeList_of_V2l_(ref List<V2l> v) { throw new NotImplementedException(); }
    public void CodeList_of_V2f_(ref List<V2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_V2d_(ref List<V2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_V3i_(ref List<V3i> v) { throw new NotImplementedException(); }
    public void CodeList_of_V3ui_(ref List<V3ui> v) { throw new NotImplementedException(); }
    public void CodeList_of_V3l_(ref List<V3l> v) { throw new NotImplementedException(); }
    public void CodeList_of_V3f_(ref List<V3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_V3d_(ref List<V3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_V4i_(ref List<V4i> v) { throw new NotImplementedException(); }
    public void CodeList_of_V4ui_(ref List<V4ui> v) { throw new NotImplementedException(); }
    public void CodeList_of_V4l_(ref List<V4l> v) { throw new NotImplementedException(); }
    public void CodeList_of_V4f_(ref List<V4f> v) { throw new NotImplementedException(); }
    public void CodeList_of_V4d_(ref List<V4d> v) { throw new NotImplementedException(); }
    public void CodeList_of_M22i_(ref List<M22i> v) { throw new NotImplementedException(); }
    public void CodeList_of_M22l_(ref List<M22l> v) { throw new NotImplementedException(); }
    public void CodeList_of_M22f_(ref List<M22f> v) { throw new NotImplementedException(); }
    public void CodeList_of_M22d_(ref List<M22d> v) { throw new NotImplementedException(); }
    public void CodeList_of_M23i_(ref List<M23i> v) { throw new NotImplementedException(); }
    public void CodeList_of_M23l_(ref List<M23l> v) { throw new NotImplementedException(); }
    public void CodeList_of_M23f_(ref List<M23f> v) { throw new NotImplementedException(); }
    public void CodeList_of_M23d_(ref List<M23d> v) { throw new NotImplementedException(); }
    public void CodeList_of_M33i_(ref List<M33i> v) { throw new NotImplementedException(); }
    public void CodeList_of_M33l_(ref List<M33l> v) { throw new NotImplementedException(); }
    public void CodeList_of_M33f_(ref List<M33f> v) { throw new NotImplementedException(); }
    public void CodeList_of_M33d_(ref List<M33d> v) { throw new NotImplementedException(); }
    public void CodeList_of_M34i_(ref List<M34i> v) { throw new NotImplementedException(); }
    public void CodeList_of_M34l_(ref List<M34l> v) { throw new NotImplementedException(); }
    public void CodeList_of_M34f_(ref List<M34f> v) { throw new NotImplementedException(); }
    public void CodeList_of_M34d_(ref List<M34d> v) { throw new NotImplementedException(); }
    public void CodeList_of_M44i_(ref List<M44i> v) { throw new NotImplementedException(); }
    public void CodeList_of_M44l_(ref List<M44l> v) { throw new NotImplementedException(); }
    public void CodeList_of_M44f_(ref List<M44f> v) { throw new NotImplementedException(); }
    public void CodeList_of_M44d_(ref List<M44d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1b_(ref List<Range1b> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1sb_(ref List<Range1sb> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1s_(ref List<Range1s> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1us_(ref List<Range1us> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1i_(ref List<Range1i> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1ui_(ref List<Range1ui> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1l_(ref List<Range1l> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1ul_(ref List<Range1ul> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1f_(ref List<Range1f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Range1d_(ref List<Range1d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box2i_(ref List<Box2i> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box2l_(ref List<Box2l> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box2f_(ref List<Box2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box2d_(ref List<Box2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box3i_(ref List<Box3i> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box3l_(ref List<Box3l> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box3f_(ref List<Box3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Box3d_(ref List<Box3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_C3b_(ref List<C3b> v) { throw new NotImplementedException(); }
    public void CodeList_of_C3us_(ref List<C3us> v) { throw new NotImplementedException(); }
    public void CodeList_of_C3ui_(ref List<C3ui> v) { throw new NotImplementedException(); }
    public void CodeList_of_C3f_(ref List<C3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_C3d_(ref List<C3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_C4b_(ref List<C4b> v) { throw new NotImplementedException(); }
    public void CodeList_of_C4us_(ref List<C4us> v) { throw new NotImplementedException(); }
    public void CodeList_of_C4ui_(ref List<C4ui> v) { throw new NotImplementedException(); }
    public void CodeList_of_C4f_(ref List<C4f> v) { throw new NotImplementedException(); }
    public void CodeList_of_C4d_(ref List<C4d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Euclidean3f_(ref List<Euclidean3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Euclidean3d_(ref List<Euclidean3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Rot2f_(ref List<Rot2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Rot2d_(ref List<Rot2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Rot3f_(ref List<Rot3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Rot3d_(ref List<Rot3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Scale3f_(ref List<Scale3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Scale3d_(ref List<Scale3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Shift3f_(ref List<Shift3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Shift3d_(ref List<Shift3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Trafo2f_(ref List<Trafo2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Trafo2d_(ref List<Trafo2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Trafo3f_(ref List<Trafo3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Trafo3d_(ref List<Trafo3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Circle2d_(ref List<Circle2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Line2d_(ref List<Line2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Line3d_(ref List<Line3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Plane2d_(ref List<Plane2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Plane3d_(ref List<Plane3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_PlaneWithPoint3d_(ref List<PlaneWithPoint3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Quad2d_(ref List<Quad2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Quad3d_(ref List<Quad3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Ray2d_(ref List<Ray2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Ray3d_(ref List<Ray3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Sphere3d_(ref List<Sphere3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Triangle2d_(ref List<Triangle2d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Triangle3d_(ref List<Triangle3d> v) { throw new NotImplementedException(); }
    public void CodeList_of_Circle2f_(ref List<Circle2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Line2f_(ref List<Line2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Line3f_(ref List<Line3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Plane2f_(ref List<Plane2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Plane3f_(ref List<Plane3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_PlaneWithPoint3f_(ref List<PlaneWithPoint3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Quad2f_(ref List<Quad2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Quad3f_(ref List<Quad3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Ray2f_(ref List<Ray2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Ray3f_(ref List<Ray3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Sphere3f_(ref List<Sphere3f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Triangle2f_(ref List<Triangle2f> v) { throw new NotImplementedException(); }
    public void CodeList_of_Triangle3f_(ref List<Triangle3f> v) { throw new NotImplementedException(); }

    #endregion

    #region Arrays of Tensors

    public void CodeVector_of_Byte_Array(ref Vector<byte>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_SByte_Array(ref Vector<sbyte>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Short_Array(ref Vector<short>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_UShort_Array(ref Vector<ushort>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Int_Array(ref Vector<int>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_UInt_Array(ref Vector<uint>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Long_Array(ref Vector<long>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_ULong_Array(ref Vector<ulong>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Float_Array(ref Vector<float>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Double_Array(ref Vector<double>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Fraction_Array(ref Vector<Fraction>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V2i_Array(ref Vector<V2i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V2l_Array(ref Vector<V2l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V2f_Array(ref Vector<V2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V2d_Array(ref Vector<V2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V3i_Array(ref Vector<V3i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V3l_Array(ref Vector<V3l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V3f_Array(ref Vector<V3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V3d_Array(ref Vector<V3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V4i_Array(ref Vector<V4i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V4l_Array(ref Vector<V4l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V4f_Array(ref Vector<V4f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_V4d_Array(ref Vector<V4d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M22i_Array(ref Vector<M22i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M22l_Array(ref Vector<M22l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M22f_Array(ref Vector<M22f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M22d_Array(ref Vector<M22d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M23i_Array(ref Vector<M23i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M23l_Array(ref Vector<M23l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M23f_Array(ref Vector<M23f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M23d_Array(ref Vector<M23d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M33i_Array(ref Vector<M33i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M33l_Array(ref Vector<M33l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M33f_Array(ref Vector<M33f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M33d_Array(ref Vector<M33d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M34i_Array(ref Vector<M34i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M34l_Array(ref Vector<M34l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M34f_Array(ref Vector<M34f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M34d_Array(ref Vector<M34d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M44i_Array(ref Vector<M44i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M44l_Array(ref Vector<M44l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M44f_Array(ref Vector<M44f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_M44d_Array(ref Vector<M44d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C3b_Array(ref Vector<C3b>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C3us_Array(ref Vector<C3us>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C3ui_Array(ref Vector<C3ui>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C3f_Array(ref Vector<C3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C3d_Array(ref Vector<C3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C4b_Array(ref Vector<C4b>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C4us_Array(ref Vector<C4us>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C4ui_Array(ref Vector<C4ui>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C4f_Array(ref Vector<C4f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_C4d_Array(ref Vector<C4d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1b_Array(ref Vector<Range1b>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1sb_Array(ref Vector<Range1sb>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1s_Array(ref Vector<Range1s>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1us_Array(ref Vector<Range1us>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1i_Array(ref Vector<Range1i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1ui_Array(ref Vector<Range1ui>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1l_Array(ref Vector<Range1l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1ul_Array(ref Vector<Range1ul>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1f_Array(ref Vector<Range1f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Range1d_Array(ref Vector<Range1d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box2i_Array(ref Vector<Box2i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box2l_Array(ref Vector<Box2l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box2f_Array(ref Vector<Box2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box2d_Array(ref Vector<Box2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box3i_Array(ref Vector<Box3i>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box3l_Array(ref Vector<Box3l>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box3f_Array(ref Vector<Box3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Box3d_Array(ref Vector<Box3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Euclidean3f_Array(ref Vector<Euclidean3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Euclidean3d_Array(ref Vector<Euclidean3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Rot2f_Array(ref Vector<Rot2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Rot2d_Array(ref Vector<Rot2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Rot3f_Array(ref Vector<Rot3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Rot3d_Array(ref Vector<Rot3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Scale3f_Array(ref Vector<Scale3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Scale3d_Array(ref Vector<Scale3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Shift3f_Array(ref Vector<Shift3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Shift3d_Array(ref Vector<Shift3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Trafo2f_Array(ref Vector<Trafo2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Trafo2d_Array(ref Vector<Trafo2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Trafo3f_Array(ref Vector<Trafo3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Trafo3d_Array(ref Vector<Trafo3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Bool_Array(ref Vector<bool>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Char_Array(ref Vector<char>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_String_Array(ref Vector<string>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Type_Array(ref Vector<Type>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Guid_Array(ref Vector<Guid>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Symbol_Array(ref Vector<Symbol>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Circle2d_Array(ref Vector<Circle2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Line2d_Array(ref Vector<Line2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Line3d_Array(ref Vector<Line3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Plane2d_Array(ref Vector<Plane2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Plane3d_Array(ref Vector<Plane3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_PlaneWithPoint3d_Array(ref Vector<PlaneWithPoint3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Quad2d_Array(ref Vector<Quad2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Quad3d_Array(ref Vector<Quad3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Ray2d_Array(ref Vector<Ray2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Ray3d_Array(ref Vector<Ray3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Sphere3d_Array(ref Vector<Sphere3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Triangle2d_Array(ref Vector<Triangle2d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Triangle3d_Array(ref Vector<Triangle3d>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Circle2f_Array(ref Vector<Circle2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Line2f_Array(ref Vector<Line2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Line3f_Array(ref Vector<Line3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Plane2f_Array(ref Vector<Plane2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Plane3f_Array(ref Vector<Plane3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_PlaneWithPoint3f_Array(ref Vector<PlaneWithPoint3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Quad2f_Array(ref Vector<Quad2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Quad3f_Array(ref Vector<Quad3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Ray2f_Array(ref Vector<Ray2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Ray3f_Array(ref Vector<Ray3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Sphere3f_Array(ref Vector<Sphere3f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Triangle2f_Array(ref Vector<Triangle2f>[] v) { throw new NotImplementedException(); }
    public void CodeVector_of_Triangle3f_Array(ref Vector<Triangle3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Byte_Array(ref Matrix<byte>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_SByte_Array(ref Matrix<sbyte>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Short_Array(ref Matrix<short>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_UShort_Array(ref Matrix<ushort>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Int_Array(ref Matrix<int>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_UInt_Array(ref Matrix<uint>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Long_Array(ref Matrix<long>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_ULong_Array(ref Matrix<ulong>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Float_Array(ref Matrix<float>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Double_Array(ref Matrix<double>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Fraction_Array(ref Matrix<Fraction>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V2i_Array(ref Matrix<V2i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V2l_Array(ref Matrix<V2l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V2f_Array(ref Matrix<V2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V2d_Array(ref Matrix<V2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V3i_Array(ref Matrix<V3i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V3l_Array(ref Matrix<V3l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V3f_Array(ref Matrix<V3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V3d_Array(ref Matrix<V3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V4i_Array(ref Matrix<V4i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V4l_Array(ref Matrix<V4l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V4f_Array(ref Matrix<V4f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_V4d_Array(ref Matrix<V4d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M22i_Array(ref Matrix<M22i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M22l_Array(ref Matrix<M22l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M22f_Array(ref Matrix<M22f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M22d_Array(ref Matrix<M22d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M23i_Array(ref Matrix<M23i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M23l_Array(ref Matrix<M23l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M23f_Array(ref Matrix<M23f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M23d_Array(ref Matrix<M23d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M33i_Array(ref Matrix<M33i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M33l_Array(ref Matrix<M33l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M33f_Array(ref Matrix<M33f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M33d_Array(ref Matrix<M33d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M34i_Array(ref Matrix<M34i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M34l_Array(ref Matrix<M34l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M34f_Array(ref Matrix<M34f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M34d_Array(ref Matrix<M34d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M44i_Array(ref Matrix<M44i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M44l_Array(ref Matrix<M44l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M44f_Array(ref Matrix<M44f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_M44d_Array(ref Matrix<M44d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C3b_Array(ref Matrix<C3b>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C3us_Array(ref Matrix<C3us>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C3ui_Array(ref Matrix<C3ui>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C3f_Array(ref Matrix<C3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C3d_Array(ref Matrix<C3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C4b_Array(ref Matrix<C4b>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C4us_Array(ref Matrix<C4us>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C4ui_Array(ref Matrix<C4ui>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C4f_Array(ref Matrix<C4f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_C4d_Array(ref Matrix<C4d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1b_Array(ref Matrix<Range1b>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1sb_Array(ref Matrix<Range1sb>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1s_Array(ref Matrix<Range1s>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1us_Array(ref Matrix<Range1us>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1i_Array(ref Matrix<Range1i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1ui_Array(ref Matrix<Range1ui>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1l_Array(ref Matrix<Range1l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1ul_Array(ref Matrix<Range1ul>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1f_Array(ref Matrix<Range1f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Range1d_Array(ref Matrix<Range1d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box2i_Array(ref Matrix<Box2i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box2l_Array(ref Matrix<Box2l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box2f_Array(ref Matrix<Box2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box2d_Array(ref Matrix<Box2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box3i_Array(ref Matrix<Box3i>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box3l_Array(ref Matrix<Box3l>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box3f_Array(ref Matrix<Box3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Box3d_Array(ref Matrix<Box3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Euclidean3f_Array(ref Matrix<Euclidean3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Euclidean3d_Array(ref Matrix<Euclidean3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Rot2f_Array(ref Matrix<Rot2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Rot2d_Array(ref Matrix<Rot2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Rot3f_Array(ref Matrix<Rot3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Rot3d_Array(ref Matrix<Rot3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Scale3f_Array(ref Matrix<Scale3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Scale3d_Array(ref Matrix<Scale3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Shift3f_Array(ref Matrix<Shift3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Shift3d_Array(ref Matrix<Shift3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Trafo2f_Array(ref Matrix<Trafo2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Trafo2d_Array(ref Matrix<Trafo2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Trafo3f_Array(ref Matrix<Trafo3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Trafo3d_Array(ref Matrix<Trafo3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Bool_Array(ref Matrix<bool>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Char_Array(ref Matrix<char>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_String_Array(ref Matrix<string>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Type_Array(ref Matrix<Type>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Guid_Array(ref Matrix<Guid>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Symbol_Array(ref Matrix<Symbol>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Circle2d_Array(ref Matrix<Circle2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Line2d_Array(ref Matrix<Line2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Line3d_Array(ref Matrix<Line3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Plane2d_Array(ref Matrix<Plane2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Plane3d_Array(ref Matrix<Plane3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_PlaneWithPoint3d_Array(ref Matrix<PlaneWithPoint3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Quad2d_Array(ref Matrix<Quad2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Quad3d_Array(ref Matrix<Quad3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Ray2d_Array(ref Matrix<Ray2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Ray3d_Array(ref Matrix<Ray3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Sphere3d_Array(ref Matrix<Sphere3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Triangle2d_Array(ref Matrix<Triangle2d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Triangle3d_Array(ref Matrix<Triangle3d>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Circle2f_Array(ref Matrix<Circle2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Line2f_Array(ref Matrix<Line2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Line3f_Array(ref Matrix<Line3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Plane2f_Array(ref Matrix<Plane2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Plane3f_Array(ref Matrix<Plane3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_PlaneWithPoint3f_Array(ref Matrix<PlaneWithPoint3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Quad2f_Array(ref Matrix<Quad2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Quad3f_Array(ref Matrix<Quad3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Ray2f_Array(ref Matrix<Ray2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Ray3f_Array(ref Matrix<Ray3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Sphere3f_Array(ref Matrix<Sphere3f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Triangle2f_Array(ref Matrix<Triangle2f>[] v) { throw new NotImplementedException(); }
    public void CodeMatrix_of_Triangle3f_Array(ref Matrix<Triangle3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Byte_Array(ref Volume<byte>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_SByte_Array(ref Volume<sbyte>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Short_Array(ref Volume<short>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_UShort_Array(ref Volume<ushort>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Int_Array(ref Volume<int>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_UInt_Array(ref Volume<uint>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Long_Array(ref Volume<long>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_ULong_Array(ref Volume<ulong>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Float_Array(ref Volume<float>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Double_Array(ref Volume<double>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Fraction_Array(ref Volume<Fraction>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V2i_Array(ref Volume<V2i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V2l_Array(ref Volume<V2l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V2f_Array(ref Volume<V2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V2d_Array(ref Volume<V2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V3i_Array(ref Volume<V3i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V3l_Array(ref Volume<V3l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V3f_Array(ref Volume<V3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V3d_Array(ref Volume<V3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V4i_Array(ref Volume<V4i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V4l_Array(ref Volume<V4l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V4f_Array(ref Volume<V4f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_V4d_Array(ref Volume<V4d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M22i_Array(ref Volume<M22i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M22l_Array(ref Volume<M22l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M22f_Array(ref Volume<M22f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M22d_Array(ref Volume<M22d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M23i_Array(ref Volume<M23i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M23l_Array(ref Volume<M23l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M23f_Array(ref Volume<M23f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M23d_Array(ref Volume<M23d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M33i_Array(ref Volume<M33i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M33l_Array(ref Volume<M33l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M33f_Array(ref Volume<M33f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M33d_Array(ref Volume<M33d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M34i_Array(ref Volume<M34i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M34l_Array(ref Volume<M34l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M34f_Array(ref Volume<M34f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M34d_Array(ref Volume<M34d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M44i_Array(ref Volume<M44i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M44l_Array(ref Volume<M44l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M44f_Array(ref Volume<M44f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_M44d_Array(ref Volume<M44d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C3b_Array(ref Volume<C3b>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C3us_Array(ref Volume<C3us>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C3ui_Array(ref Volume<C3ui>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C3f_Array(ref Volume<C3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C3d_Array(ref Volume<C3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C4b_Array(ref Volume<C4b>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C4us_Array(ref Volume<C4us>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C4ui_Array(ref Volume<C4ui>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C4f_Array(ref Volume<C4f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_C4d_Array(ref Volume<C4d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1b_Array(ref Volume<Range1b>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1sb_Array(ref Volume<Range1sb>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1s_Array(ref Volume<Range1s>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1us_Array(ref Volume<Range1us>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1i_Array(ref Volume<Range1i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1ui_Array(ref Volume<Range1ui>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1l_Array(ref Volume<Range1l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1ul_Array(ref Volume<Range1ul>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1f_Array(ref Volume<Range1f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Range1d_Array(ref Volume<Range1d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box2i_Array(ref Volume<Box2i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box2l_Array(ref Volume<Box2l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box2f_Array(ref Volume<Box2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box2d_Array(ref Volume<Box2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box3i_Array(ref Volume<Box3i>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box3l_Array(ref Volume<Box3l>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box3f_Array(ref Volume<Box3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Box3d_Array(ref Volume<Box3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Euclidean3f_Array(ref Volume<Euclidean3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Euclidean3d_Array(ref Volume<Euclidean3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Rot2f_Array(ref Volume<Rot2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Rot2d_Array(ref Volume<Rot2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Rot3f_Array(ref Volume<Rot3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Rot3d_Array(ref Volume<Rot3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Scale3f_Array(ref Volume<Scale3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Scale3d_Array(ref Volume<Scale3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Shift3f_Array(ref Volume<Shift3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Shift3d_Array(ref Volume<Shift3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Trafo2f_Array(ref Volume<Trafo2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Trafo2d_Array(ref Volume<Trafo2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Trafo3f_Array(ref Volume<Trafo3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Trafo3d_Array(ref Volume<Trafo3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Bool_Array(ref Volume<bool>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Char_Array(ref Volume<char>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_String_Array(ref Volume<string>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Type_Array(ref Volume<Type>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Guid_Array(ref Volume<Guid>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Symbol_Array(ref Volume<Symbol>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Circle2d_Array(ref Volume<Circle2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Line2d_Array(ref Volume<Line2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Line3d_Array(ref Volume<Line3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Plane2d_Array(ref Volume<Plane2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Plane3d_Array(ref Volume<Plane3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_PlaneWithPoint3d_Array(ref Volume<PlaneWithPoint3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Quad2d_Array(ref Volume<Quad2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Quad3d_Array(ref Volume<Quad3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Ray2d_Array(ref Volume<Ray2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Ray3d_Array(ref Volume<Ray3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Sphere3d_Array(ref Volume<Sphere3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Triangle2d_Array(ref Volume<Triangle2d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Triangle3d_Array(ref Volume<Triangle3d>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Circle2f_Array(ref Volume<Circle2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Line2f_Array(ref Volume<Line2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Line3f_Array(ref Volume<Line3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Plane2f_Array(ref Volume<Plane2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Plane3f_Array(ref Volume<Plane3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_PlaneWithPoint3f_Array(ref Volume<PlaneWithPoint3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Quad2f_Array(ref Volume<Quad2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Quad3f_Array(ref Volume<Quad3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Ray2f_Array(ref Volume<Ray2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Ray3f_Array(ref Volume<Ray3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Sphere3f_Array(ref Volume<Sphere3f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Triangle2f_Array(ref Volume<Triangle2f>[] v) { throw new NotImplementedException(); }
    public void CodeVolume_of_Triangle3f_Array(ref Volume<Triangle3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Byte_Array(ref Tensor<byte>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_SByte_Array(ref Tensor<sbyte>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Short_Array(ref Tensor<short>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_UShort_Array(ref Tensor<ushort>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Int_Array(ref Tensor<int>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_UInt_Array(ref Tensor<uint>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Long_Array(ref Tensor<long>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_ULong_Array(ref Tensor<ulong>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Float_Array(ref Tensor<float>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Double_Array(ref Tensor<double>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Fraction_Array(ref Tensor<Fraction>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V2i_Array(ref Tensor<V2i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V2l_Array(ref Tensor<V2l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V2f_Array(ref Tensor<V2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V2d_Array(ref Tensor<V2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V3i_Array(ref Tensor<V3i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V3l_Array(ref Tensor<V3l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V3f_Array(ref Tensor<V3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V3d_Array(ref Tensor<V3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V4i_Array(ref Tensor<V4i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V4l_Array(ref Tensor<V4l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V4f_Array(ref Tensor<V4f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_V4d_Array(ref Tensor<V4d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M22i_Array(ref Tensor<M22i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M22l_Array(ref Tensor<M22l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M22f_Array(ref Tensor<M22f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M22d_Array(ref Tensor<M22d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M23i_Array(ref Tensor<M23i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M23l_Array(ref Tensor<M23l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M23f_Array(ref Tensor<M23f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M23d_Array(ref Tensor<M23d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M33i_Array(ref Tensor<M33i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M33l_Array(ref Tensor<M33l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M33f_Array(ref Tensor<M33f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M33d_Array(ref Tensor<M33d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M34i_Array(ref Tensor<M34i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M34l_Array(ref Tensor<M34l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M34f_Array(ref Tensor<M34f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M34d_Array(ref Tensor<M34d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M44i_Array(ref Tensor<M44i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M44l_Array(ref Tensor<M44l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M44f_Array(ref Tensor<M44f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_M44d_Array(ref Tensor<M44d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C3b_Array(ref Tensor<C3b>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C3us_Array(ref Tensor<C3us>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C3ui_Array(ref Tensor<C3ui>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C3f_Array(ref Tensor<C3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C3d_Array(ref Tensor<C3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C4b_Array(ref Tensor<C4b>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C4us_Array(ref Tensor<C4us>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C4ui_Array(ref Tensor<C4ui>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C4f_Array(ref Tensor<C4f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_C4d_Array(ref Tensor<C4d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1b_Array(ref Tensor<Range1b>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1sb_Array(ref Tensor<Range1sb>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1s_Array(ref Tensor<Range1s>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1us_Array(ref Tensor<Range1us>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1i_Array(ref Tensor<Range1i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1ui_Array(ref Tensor<Range1ui>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1l_Array(ref Tensor<Range1l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1ul_Array(ref Tensor<Range1ul>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1f_Array(ref Tensor<Range1f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Range1d_Array(ref Tensor<Range1d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box2i_Array(ref Tensor<Box2i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box2l_Array(ref Tensor<Box2l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box2f_Array(ref Tensor<Box2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box2d_Array(ref Tensor<Box2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box3i_Array(ref Tensor<Box3i>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box3l_Array(ref Tensor<Box3l>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box3f_Array(ref Tensor<Box3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Box3d_Array(ref Tensor<Box3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Euclidean3f_Array(ref Tensor<Euclidean3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Euclidean3d_Array(ref Tensor<Euclidean3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Rot2f_Array(ref Tensor<Rot2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Rot2d_Array(ref Tensor<Rot2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Rot3f_Array(ref Tensor<Rot3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Rot3d_Array(ref Tensor<Rot3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Scale3f_Array(ref Tensor<Scale3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Scale3d_Array(ref Tensor<Scale3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Shift3f_Array(ref Tensor<Shift3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Shift3d_Array(ref Tensor<Shift3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Trafo2f_Array(ref Tensor<Trafo2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Trafo2d_Array(ref Tensor<Trafo2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Trafo3f_Array(ref Tensor<Trafo3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Trafo3d_Array(ref Tensor<Trafo3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Bool_Array(ref Tensor<bool>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Char_Array(ref Tensor<char>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_String_Array(ref Tensor<string>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Type_Array(ref Tensor<Type>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Guid_Array(ref Tensor<Guid>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Symbol_Array(ref Tensor<Symbol>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Circle2d_Array(ref Tensor<Circle2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Line2d_Array(ref Tensor<Line2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Line3d_Array(ref Tensor<Line3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Plane2d_Array(ref Tensor<Plane2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Plane3d_Array(ref Tensor<Plane3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_PlaneWithPoint3d_Array(ref Tensor<PlaneWithPoint3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Quad2d_Array(ref Tensor<Quad2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Quad3d_Array(ref Tensor<Quad3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Ray2d_Array(ref Tensor<Ray2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Ray3d_Array(ref Tensor<Ray3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Sphere3d_Array(ref Tensor<Sphere3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Triangle2d_Array(ref Tensor<Triangle2d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Triangle3d_Array(ref Tensor<Triangle3d>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Circle2f_Array(ref Tensor<Circle2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Line2f_Array(ref Tensor<Line2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Line3f_Array(ref Tensor<Line3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Plane2f_Array(ref Tensor<Plane2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Plane3f_Array(ref Tensor<Plane3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_PlaneWithPoint3f_Array(ref Tensor<PlaneWithPoint3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Quad2f_Array(ref Tensor<Quad2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Quad3f_Array(ref Tensor<Quad3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Ray2f_Array(ref Tensor<Ray2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Ray3f_Array(ref Tensor<Ray3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Sphere3f_Array(ref Tensor<Sphere3f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Triangle2f_Array(ref Tensor<Triangle2f>[] v) { throw new NotImplementedException(); }
    public void CodeTensor_of_Triangle3f_Array(ref Tensor<Triangle3f>[] v) { throw new NotImplementedException(); }

    #endregion

    #region Lists of Tensors

    public void CodeList_of_Vector_of_Byte__(ref List<Vector<byte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_SByte__(ref List<Vector<sbyte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Short__(ref List<Vector<short>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_UShort__(ref List<Vector<ushort>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Int__(ref List<Vector<int>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_UInt__(ref List<Vector<uint>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Long__(ref List<Vector<long>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_ULong__(ref List<Vector<ulong>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Float__(ref List<Vector<float>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Double__(ref List<Vector<double>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Fraction__(ref List<Vector<Fraction>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V2i__(ref List<Vector<V2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V2l__(ref List<Vector<V2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V2f__(ref List<Vector<V2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V2d__(ref List<Vector<V2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V3i__(ref List<Vector<V3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V3l__(ref List<Vector<V3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V3f__(ref List<Vector<V3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V3d__(ref List<Vector<V3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V4i__(ref List<Vector<V4i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V4l__(ref List<Vector<V4l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V4f__(ref List<Vector<V4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_V4d__(ref List<Vector<V4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M22i__(ref List<Vector<M22i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M22l__(ref List<Vector<M22l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M22f__(ref List<Vector<M22f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M22d__(ref List<Vector<M22d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M23i__(ref List<Vector<M23i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M23l__(ref List<Vector<M23l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M23f__(ref List<Vector<M23f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M23d__(ref List<Vector<M23d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M33i__(ref List<Vector<M33i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M33l__(ref List<Vector<M33l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M33f__(ref List<Vector<M33f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M33d__(ref List<Vector<M33d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M34i__(ref List<Vector<M34i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M34l__(ref List<Vector<M34l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M34f__(ref List<Vector<M34f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M34d__(ref List<Vector<M34d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M44i__(ref List<Vector<M44i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M44l__(ref List<Vector<M44l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M44f__(ref List<Vector<M44f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_M44d__(ref List<Vector<M44d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C3b__(ref List<Vector<C3b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C3us__(ref List<Vector<C3us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C3ui__(ref List<Vector<C3ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C3f__(ref List<Vector<C3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C3d__(ref List<Vector<C3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C4b__(ref List<Vector<C4b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C4us__(ref List<Vector<C4us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C4ui__(ref List<Vector<C4ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C4f__(ref List<Vector<C4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_C4d__(ref List<Vector<C4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1b__(ref List<Vector<Range1b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1sb__(ref List<Vector<Range1sb>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1s__(ref List<Vector<Range1s>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1us__(ref List<Vector<Range1us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1i__(ref List<Vector<Range1i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1ui__(ref List<Vector<Range1ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1l__(ref List<Vector<Range1l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1ul__(ref List<Vector<Range1ul>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1f__(ref List<Vector<Range1f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Range1d__(ref List<Vector<Range1d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box2i__(ref List<Vector<Box2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box2l__(ref List<Vector<Box2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box2f__(ref List<Vector<Box2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box2d__(ref List<Vector<Box2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box3i__(ref List<Vector<Box3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box3l__(ref List<Vector<Box3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box3f__(ref List<Vector<Box3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Box3d__(ref List<Vector<Box3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Euclidean3f__(ref List<Vector<Euclidean3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Euclidean3d__(ref List<Vector<Euclidean3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Rot2f__(ref List<Vector<Rot2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Rot2d__(ref List<Vector<Rot2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Rot3f__(ref List<Vector<Rot3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Rot3d__(ref List<Vector<Rot3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Scale3f__(ref List<Vector<Scale3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Scale3d__(ref List<Vector<Scale3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Shift3f__(ref List<Vector<Shift3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Shift3d__(ref List<Vector<Shift3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Trafo2f__(ref List<Vector<Trafo2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Trafo2d__(ref List<Vector<Trafo2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Trafo3f__(ref List<Vector<Trafo3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Trafo3d__(ref List<Vector<Trafo3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Bool__(ref List<Vector<bool>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Char__(ref List<Vector<char>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_String__(ref List<Vector<string>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Type__(ref List<Vector<Type>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Guid__(ref List<Vector<Guid>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Symbol__(ref List<Vector<Symbol>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Circle2d__(ref List<Vector<Circle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Line2d__(ref List<Vector<Line2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Line3d__(ref List<Vector<Line3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Plane2d__(ref List<Vector<Plane2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Plane3d__(ref List<Vector<Plane3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_PlaneWithPoint3d__(ref List<Vector<PlaneWithPoint3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Quad2d__(ref List<Vector<Quad2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Quad3d__(ref List<Vector<Quad3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Ray2d__(ref List<Vector<Ray2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Ray3d__(ref List<Vector<Ray3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Sphere3d__(ref List<Vector<Sphere3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Triangle2d__(ref List<Vector<Triangle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Triangle3d__(ref List<Vector<Triangle3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Circle2f__(ref List<Vector<Circle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Line2f__(ref List<Vector<Line2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Line3f__(ref List<Vector<Line3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Plane2f__(ref List<Vector<Plane2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Plane3f__(ref List<Vector<Plane3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_PlaneWithPoint3f__(ref List<Vector<PlaneWithPoint3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Quad2f__(ref List<Vector<Quad2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Quad3f__(ref List<Vector<Quad3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Ray2f__(ref List<Vector<Ray2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Ray3f__(ref List<Vector<Ray3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Sphere3f__(ref List<Vector<Sphere3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Triangle2f__(ref List<Vector<Triangle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Vector_of_Triangle3f__(ref List<Vector<Triangle3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Byte__(ref List<Matrix<byte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_SByte__(ref List<Matrix<sbyte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Short__(ref List<Matrix<short>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_UShort__(ref List<Matrix<ushort>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Int__(ref List<Matrix<int>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_UInt__(ref List<Matrix<uint>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Long__(ref List<Matrix<long>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_ULong__(ref List<Matrix<ulong>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Float__(ref List<Matrix<float>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Double__(ref List<Matrix<double>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Fraction__(ref List<Matrix<Fraction>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V2i__(ref List<Matrix<V2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V2l__(ref List<Matrix<V2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V2f__(ref List<Matrix<V2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V2d__(ref List<Matrix<V2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V3i__(ref List<Matrix<V3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V3l__(ref List<Matrix<V3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V3f__(ref List<Matrix<V3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V3d__(ref List<Matrix<V3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V4i__(ref List<Matrix<V4i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V4l__(ref List<Matrix<V4l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V4f__(ref List<Matrix<V4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_V4d__(ref List<Matrix<V4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M22i__(ref List<Matrix<M22i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M22l__(ref List<Matrix<M22l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M22f__(ref List<Matrix<M22f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M22d__(ref List<Matrix<M22d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M23i__(ref List<Matrix<M23i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M23l__(ref List<Matrix<M23l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M23f__(ref List<Matrix<M23f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M23d__(ref List<Matrix<M23d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M33i__(ref List<Matrix<M33i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M33l__(ref List<Matrix<M33l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M33f__(ref List<Matrix<M33f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M33d__(ref List<Matrix<M33d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M34i__(ref List<Matrix<M34i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M34l__(ref List<Matrix<M34l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M34f__(ref List<Matrix<M34f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M34d__(ref List<Matrix<M34d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M44i__(ref List<Matrix<M44i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M44l__(ref List<Matrix<M44l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M44f__(ref List<Matrix<M44f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_M44d__(ref List<Matrix<M44d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C3b__(ref List<Matrix<C3b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C3us__(ref List<Matrix<C3us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C3ui__(ref List<Matrix<C3ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C3f__(ref List<Matrix<C3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C3d__(ref List<Matrix<C3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C4b__(ref List<Matrix<C4b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C4us__(ref List<Matrix<C4us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C4ui__(ref List<Matrix<C4ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C4f__(ref List<Matrix<C4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_C4d__(ref List<Matrix<C4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1b__(ref List<Matrix<Range1b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1sb__(ref List<Matrix<Range1sb>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1s__(ref List<Matrix<Range1s>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1us__(ref List<Matrix<Range1us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1i__(ref List<Matrix<Range1i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1ui__(ref List<Matrix<Range1ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1l__(ref List<Matrix<Range1l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1ul__(ref List<Matrix<Range1ul>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1f__(ref List<Matrix<Range1f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Range1d__(ref List<Matrix<Range1d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box2i__(ref List<Matrix<Box2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box2l__(ref List<Matrix<Box2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box2f__(ref List<Matrix<Box2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box2d__(ref List<Matrix<Box2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box3i__(ref List<Matrix<Box3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box3l__(ref List<Matrix<Box3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box3f__(ref List<Matrix<Box3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Box3d__(ref List<Matrix<Box3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Euclidean3f__(ref List<Matrix<Euclidean3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Euclidean3d__(ref List<Matrix<Euclidean3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Rot2f__(ref List<Matrix<Rot2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Rot2d__(ref List<Matrix<Rot2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Rot3f__(ref List<Matrix<Rot3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Rot3d__(ref List<Matrix<Rot3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Scale3f__(ref List<Matrix<Scale3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Scale3d__(ref List<Matrix<Scale3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Shift3f__(ref List<Matrix<Shift3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Shift3d__(ref List<Matrix<Shift3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Trafo2f__(ref List<Matrix<Trafo2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Trafo2d__(ref List<Matrix<Trafo2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Trafo3f__(ref List<Matrix<Trafo3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Trafo3d__(ref List<Matrix<Trafo3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Bool__(ref List<Matrix<bool>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Char__(ref List<Matrix<char>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_String__(ref List<Matrix<string>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Type__(ref List<Matrix<Type>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Guid__(ref List<Matrix<Guid>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Symbol__(ref List<Matrix<Symbol>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Circle2d__(ref List<Matrix<Circle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Line2d__(ref List<Matrix<Line2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Line3d__(ref List<Matrix<Line3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Plane2d__(ref List<Matrix<Plane2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Plane3d__(ref List<Matrix<Plane3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_PlaneWithPoint3d__(ref List<Matrix<PlaneWithPoint3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Quad2d__(ref List<Matrix<Quad2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Quad3d__(ref List<Matrix<Quad3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Ray2d__(ref List<Matrix<Ray2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Ray3d__(ref List<Matrix<Ray3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Sphere3d__(ref List<Matrix<Sphere3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Triangle2d__(ref List<Matrix<Triangle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Triangle3d__(ref List<Matrix<Triangle3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Circle2f__(ref List<Matrix<Circle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Line2f__(ref List<Matrix<Line2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Line3f__(ref List<Matrix<Line3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Plane2f__(ref List<Matrix<Plane2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Plane3f__(ref List<Matrix<Plane3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_PlaneWithPoint3f__(ref List<Matrix<PlaneWithPoint3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Quad2f__(ref List<Matrix<Quad2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Quad3f__(ref List<Matrix<Quad3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Ray2f__(ref List<Matrix<Ray2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Ray3f__(ref List<Matrix<Ray3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Sphere3f__(ref List<Matrix<Sphere3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Triangle2f__(ref List<Matrix<Triangle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Matrix_of_Triangle3f__(ref List<Matrix<Triangle3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Byte__(ref List<Volume<byte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_SByte__(ref List<Volume<sbyte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Short__(ref List<Volume<short>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_UShort__(ref List<Volume<ushort>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Int__(ref List<Volume<int>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_UInt__(ref List<Volume<uint>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Long__(ref List<Volume<long>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_ULong__(ref List<Volume<ulong>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Float__(ref List<Volume<float>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Double__(ref List<Volume<double>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Fraction__(ref List<Volume<Fraction>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V2i__(ref List<Volume<V2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V2l__(ref List<Volume<V2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V2f__(ref List<Volume<V2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V2d__(ref List<Volume<V2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V3i__(ref List<Volume<V3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V3l__(ref List<Volume<V3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V3f__(ref List<Volume<V3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V3d__(ref List<Volume<V3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V4i__(ref List<Volume<V4i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V4l__(ref List<Volume<V4l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V4f__(ref List<Volume<V4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_V4d__(ref List<Volume<V4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M22i__(ref List<Volume<M22i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M22l__(ref List<Volume<M22l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M22f__(ref List<Volume<M22f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M22d__(ref List<Volume<M22d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M23i__(ref List<Volume<M23i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M23l__(ref List<Volume<M23l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M23f__(ref List<Volume<M23f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M23d__(ref List<Volume<M23d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M33i__(ref List<Volume<M33i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M33l__(ref List<Volume<M33l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M33f__(ref List<Volume<M33f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M33d__(ref List<Volume<M33d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M34i__(ref List<Volume<M34i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M34l__(ref List<Volume<M34l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M34f__(ref List<Volume<M34f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M34d__(ref List<Volume<M34d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M44i__(ref List<Volume<M44i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M44l__(ref List<Volume<M44l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M44f__(ref List<Volume<M44f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_M44d__(ref List<Volume<M44d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C3b__(ref List<Volume<C3b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C3us__(ref List<Volume<C3us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C3ui__(ref List<Volume<C3ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C3f__(ref List<Volume<C3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C3d__(ref List<Volume<C3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C4b__(ref List<Volume<C4b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C4us__(ref List<Volume<C4us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C4ui__(ref List<Volume<C4ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C4f__(ref List<Volume<C4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_C4d__(ref List<Volume<C4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1b__(ref List<Volume<Range1b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1sb__(ref List<Volume<Range1sb>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1s__(ref List<Volume<Range1s>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1us__(ref List<Volume<Range1us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1i__(ref List<Volume<Range1i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1ui__(ref List<Volume<Range1ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1l__(ref List<Volume<Range1l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1ul__(ref List<Volume<Range1ul>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1f__(ref List<Volume<Range1f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Range1d__(ref List<Volume<Range1d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box2i__(ref List<Volume<Box2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box2l__(ref List<Volume<Box2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box2f__(ref List<Volume<Box2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box2d__(ref List<Volume<Box2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box3i__(ref List<Volume<Box3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box3l__(ref List<Volume<Box3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box3f__(ref List<Volume<Box3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Box3d__(ref List<Volume<Box3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Euclidean3f__(ref List<Volume<Euclidean3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Euclidean3d__(ref List<Volume<Euclidean3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Rot2f__(ref List<Volume<Rot2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Rot2d__(ref List<Volume<Rot2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Rot3f__(ref List<Volume<Rot3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Rot3d__(ref List<Volume<Rot3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Scale3f__(ref List<Volume<Scale3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Scale3d__(ref List<Volume<Scale3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Shift3f__(ref List<Volume<Shift3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Shift3d__(ref List<Volume<Shift3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Trafo2f__(ref List<Volume<Trafo2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Trafo2d__(ref List<Volume<Trafo2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Trafo3f__(ref List<Volume<Trafo3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Trafo3d__(ref List<Volume<Trafo3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Bool__(ref List<Volume<bool>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Char__(ref List<Volume<char>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_String__(ref List<Volume<string>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Type__(ref List<Volume<Type>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Guid__(ref List<Volume<Guid>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Symbol__(ref List<Volume<Symbol>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Circle2d__(ref List<Volume<Circle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Line2d__(ref List<Volume<Line2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Line3d__(ref List<Volume<Line3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Plane2d__(ref List<Volume<Plane2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Plane3d__(ref List<Volume<Plane3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_PlaneWithPoint3d__(ref List<Volume<PlaneWithPoint3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Quad2d__(ref List<Volume<Quad2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Quad3d__(ref List<Volume<Quad3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Ray2d__(ref List<Volume<Ray2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Ray3d__(ref List<Volume<Ray3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Sphere3d__(ref List<Volume<Sphere3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Triangle2d__(ref List<Volume<Triangle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Triangle3d__(ref List<Volume<Triangle3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Circle2f__(ref List<Volume<Circle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Line2f__(ref List<Volume<Line2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Line3f__(ref List<Volume<Line3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Plane2f__(ref List<Volume<Plane2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Plane3f__(ref List<Volume<Plane3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_PlaneWithPoint3f__(ref List<Volume<PlaneWithPoint3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Quad2f__(ref List<Volume<Quad2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Quad3f__(ref List<Volume<Quad3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Ray2f__(ref List<Volume<Ray2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Ray3f__(ref List<Volume<Ray3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Sphere3f__(ref List<Volume<Sphere3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Triangle2f__(ref List<Volume<Triangle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Volume_of_Triangle3f__(ref List<Volume<Triangle3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Byte__(ref List<Tensor<byte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_SByte__(ref List<Tensor<sbyte>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Short__(ref List<Tensor<short>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_UShort__(ref List<Tensor<ushort>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Int__(ref List<Tensor<int>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_UInt__(ref List<Tensor<uint>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Long__(ref List<Tensor<long>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_ULong__(ref List<Tensor<ulong>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Float__(ref List<Tensor<float>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Double__(ref List<Tensor<double>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Fraction__(ref List<Tensor<Fraction>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V2i__(ref List<Tensor<V2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V2l__(ref List<Tensor<V2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V2f__(ref List<Tensor<V2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V2d__(ref List<Tensor<V2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V3i__(ref List<Tensor<V3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V3l__(ref List<Tensor<V3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V3f__(ref List<Tensor<V3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V3d__(ref List<Tensor<V3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V4i__(ref List<Tensor<V4i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V4l__(ref List<Tensor<V4l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V4f__(ref List<Tensor<V4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_V4d__(ref List<Tensor<V4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M22i__(ref List<Tensor<M22i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M22l__(ref List<Tensor<M22l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M22f__(ref List<Tensor<M22f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M22d__(ref List<Tensor<M22d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M23i__(ref List<Tensor<M23i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M23l__(ref List<Tensor<M23l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M23f__(ref List<Tensor<M23f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M23d__(ref List<Tensor<M23d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M33i__(ref List<Tensor<M33i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M33l__(ref List<Tensor<M33l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M33f__(ref List<Tensor<M33f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M33d__(ref List<Tensor<M33d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M34i__(ref List<Tensor<M34i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M34l__(ref List<Tensor<M34l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M34f__(ref List<Tensor<M34f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M34d__(ref List<Tensor<M34d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M44i__(ref List<Tensor<M44i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M44l__(ref List<Tensor<M44l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M44f__(ref List<Tensor<M44f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_M44d__(ref List<Tensor<M44d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C3b__(ref List<Tensor<C3b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C3us__(ref List<Tensor<C3us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C3ui__(ref List<Tensor<C3ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C3f__(ref List<Tensor<C3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C3d__(ref List<Tensor<C3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C4b__(ref List<Tensor<C4b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C4us__(ref List<Tensor<C4us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C4ui__(ref List<Tensor<C4ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C4f__(ref List<Tensor<C4f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_C4d__(ref List<Tensor<C4d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1b__(ref List<Tensor<Range1b>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1sb__(ref List<Tensor<Range1sb>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1s__(ref List<Tensor<Range1s>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1us__(ref List<Tensor<Range1us>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1i__(ref List<Tensor<Range1i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1ui__(ref List<Tensor<Range1ui>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1l__(ref List<Tensor<Range1l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1ul__(ref List<Tensor<Range1ul>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1f__(ref List<Tensor<Range1f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Range1d__(ref List<Tensor<Range1d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box2i__(ref List<Tensor<Box2i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box2l__(ref List<Tensor<Box2l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box2f__(ref List<Tensor<Box2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box2d__(ref List<Tensor<Box2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box3i__(ref List<Tensor<Box3i>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box3l__(ref List<Tensor<Box3l>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box3f__(ref List<Tensor<Box3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Box3d__(ref List<Tensor<Box3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Euclidean3f__(ref List<Tensor<Euclidean3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Euclidean3d__(ref List<Tensor<Euclidean3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Rot2f__(ref List<Tensor<Rot2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Rot2d__(ref List<Tensor<Rot2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Rot3f__(ref List<Tensor<Rot3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Rot3d__(ref List<Tensor<Rot3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Scale3f__(ref List<Tensor<Scale3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Scale3d__(ref List<Tensor<Scale3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Shift3f__(ref List<Tensor<Shift3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Shift3d__(ref List<Tensor<Shift3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Trafo2f__(ref List<Tensor<Trafo2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Trafo2d__(ref List<Tensor<Trafo2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Trafo3f__(ref List<Tensor<Trafo3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Trafo3d__(ref List<Tensor<Trafo3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Bool__(ref List<Tensor<bool>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Char__(ref List<Tensor<char>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_String__(ref List<Tensor<string>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Type__(ref List<Tensor<Type>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Guid__(ref List<Tensor<Guid>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Symbol__(ref List<Tensor<Symbol>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Circle2d__(ref List<Tensor<Circle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Line2d__(ref List<Tensor<Line2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Line3d__(ref List<Tensor<Line3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Plane2d__(ref List<Tensor<Plane2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Plane3d__(ref List<Tensor<Plane3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_PlaneWithPoint3d__(ref List<Tensor<PlaneWithPoint3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Quad2d__(ref List<Tensor<Quad2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Quad3d__(ref List<Tensor<Quad3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Ray2d__(ref List<Tensor<Ray2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Ray3d__(ref List<Tensor<Ray3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Sphere3d__(ref List<Tensor<Sphere3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Triangle2d__(ref List<Tensor<Triangle2d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Triangle3d__(ref List<Tensor<Triangle3d>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Circle2f__(ref List<Tensor<Circle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Line2f__(ref List<Tensor<Line2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Line3f__(ref List<Tensor<Line3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Plane2f__(ref List<Tensor<Plane2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Plane3f__(ref List<Tensor<Plane3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_PlaneWithPoint3f__(ref List<Tensor<PlaneWithPoint3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Quad2f__(ref List<Tensor<Quad2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Quad3f__(ref List<Tensor<Quad3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Ray2f__(ref List<Tensor<Ray2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Ray3f__(ref List<Tensor<Ray3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Sphere3f__(ref List<Tensor<Sphere3f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Triangle2f__(ref List<Tensor<Triangle2f>> v) { throw new NotImplementedException(); }
    public void CodeList_of_Tensor_of_Triangle3f__(ref List<Tensor<Triangle3f>> v) { throw new NotImplementedException(); }

    #endregion
}

public partial class XmlWritingCoder
{
    #region Vectors

    public void CodeV2i(ref V2i v) { AddValue(v.ToString()); }
    public void CodeV2ui(ref V2ui v) { AddValue(v.ToString()); }
    public void CodeV2l(ref V2l v) { AddValue(v.ToString()); }
    public void CodeV2f(ref V2f v) { AddValue(v.ToString()); }
    public void CodeV2d(ref V2d v) { AddValue(v.ToString()); }
    public void CodeV3i(ref V3i v) { AddValue(v.ToString()); }
    public void CodeV3ui(ref V3ui v) { AddValue(v.ToString()); }
    public void CodeV3l(ref V3l v) { AddValue(v.ToString()); }
    public void CodeV3f(ref V3f v) { AddValue(v.ToString()); }
    public void CodeV3d(ref V3d v) { AddValue(v.ToString()); }
    public void CodeV4i(ref V4i v) { AddValue(v.ToString()); }
    public void CodeV4ui(ref V4ui v) { AddValue(v.ToString()); }
    public void CodeV4l(ref V4l v) { AddValue(v.ToString()); }
    public void CodeV4f(ref V4f v) { AddValue(v.ToString()); }
    public void CodeV4d(ref V4d v) { AddValue(v.ToString()); }

    #endregion

    #region Matrices

    public void CodeM22i(ref M22i v) { AddValue(v.ToString()); }
    public void CodeM22l(ref M22l v) { AddValue(v.ToString()); }
    public void CodeM22f(ref M22f v) { AddValue(v.ToString()); }
    public void CodeM22d(ref M22d v) { AddValue(v.ToString()); }
    public void CodeM23i(ref M23i v) { AddValue(v.ToString()); }
    public void CodeM23l(ref M23l v) { AddValue(v.ToString()); }
    public void CodeM23f(ref M23f v) { AddValue(v.ToString()); }
    public void CodeM23d(ref M23d v) { AddValue(v.ToString()); }
    public void CodeM33i(ref M33i v) { AddValue(v.ToString()); }
    public void CodeM33l(ref M33l v) { AddValue(v.ToString()); }
    public void CodeM33f(ref M33f v) { AddValue(v.ToString()); }
    public void CodeM33d(ref M33d v) { AddValue(v.ToString()); }
    public void CodeM34i(ref M34i v) { AddValue(v.ToString()); }
    public void CodeM34l(ref M34l v) { AddValue(v.ToString()); }
    public void CodeM34f(ref M34f v) { AddValue(v.ToString()); }
    public void CodeM34d(ref M34d v) { AddValue(v.ToString()); }
    public void CodeM44i(ref M44i v) { AddValue(v.ToString()); }
    public void CodeM44l(ref M44l v) { AddValue(v.ToString()); }
    public void CodeM44f(ref M44f v) { AddValue(v.ToString()); }
    public void CodeM44d(ref M44d v) { AddValue(v.ToString()); }

    #endregion

    #region Ranges and Boxes

    public void CodeRange1b(ref Range1b v) { AddValue(v.ToString()); }
    public void CodeRange1sb(ref Range1sb v) { AddValue(v.ToString()); }
    public void CodeRange1s(ref Range1s v) { AddValue(v.ToString()); }
    public void CodeRange1us(ref Range1us v) { AddValue(v.ToString()); }
    public void CodeRange1i(ref Range1i v) { AddValue(v.ToString()); }
    public void CodeRange1ui(ref Range1ui v) { AddValue(v.ToString()); }
    public void CodeRange1l(ref Range1l v) { AddValue(v.ToString()); }
    public void CodeRange1ul(ref Range1ul v) { AddValue(v.ToString()); }
    public void CodeRange1f(ref Range1f v) { AddValue(v.ToString()); }
    public void CodeRange1d(ref Range1d v) { AddValue(v.ToString()); }
    public void CodeBox2i(ref Box2i v) { AddValue(v.ToString()); }
    public void CodeBox2l(ref Box2l v) { AddValue(v.ToString()); }
    public void CodeBox2f(ref Box2f v) { AddValue(v.ToString()); }
    public void CodeBox2d(ref Box2d v) { AddValue(v.ToString()); }
    public void CodeBox3i(ref Box3i v) { AddValue(v.ToString()); }
    public void CodeBox3l(ref Box3l v) { AddValue(v.ToString()); }
    public void CodeBox3f(ref Box3f v) { AddValue(v.ToString()); }
    public void CodeBox3d(ref Box3d v) { AddValue(v.ToString()); }

    #endregion

    #region Geometry Types

    public void CodeCircle2f(ref Circle2f v) { AddValue(v.ToString()); }
    public void CodeLine2f(ref Line2f v) { AddValue(v.ToString()); }
    public void CodeLine3f(ref Line3f v) { AddValue(v.ToString()); }
    public void CodePlane2f(ref Plane2f v) { AddValue(v.ToString()); }
    public void CodePlane3f(ref Plane3f v) { AddValue(v.ToString()); }
    public void CodePlaneWithPoint3f(ref PlaneWithPoint3f v) { AddValue(v.ToString()); }
    public void CodeQuad2f(ref Quad2f v) { AddValue(v.ToString()); }
    public void CodeQuad3f(ref Quad3f v) { AddValue(v.ToString()); }
    public void CodeRay2f(ref Ray2f v) { AddValue(v.ToString()); }
    public void CodeRay3f(ref Ray3f v) { AddValue(v.ToString()); }
    public void CodeSphere3f(ref Sphere3f v) { AddValue(v.ToString()); }
    public void CodeTriangle2f(ref Triangle2f v) { AddValue(v.ToString()); }
    public void CodeTriangle3f(ref Triangle3f v) { AddValue(v.ToString()); }

    public void CodeCircle2d(ref Circle2d v) { AddValue(v.ToString()); }
    public void CodeLine2d(ref Line2d v) { AddValue(v.ToString()); }
    public void CodeLine3d(ref Line3d v) { AddValue(v.ToString()); }
    public void CodePlane2d(ref Plane2d v) { AddValue(v.ToString()); }
    public void CodePlane3d(ref Plane3d v) { AddValue(v.ToString()); }
    public void CodePlaneWithPoint3d(ref PlaneWithPoint3d v) { AddValue(v.ToString()); }
    public void CodeQuad2d(ref Quad2d v) { AddValue(v.ToString()); }
    public void CodeQuad3d(ref Quad3d v) { AddValue(v.ToString()); }
    public void CodeRay2d(ref Ray2d v) { AddValue(v.ToString()); }
    public void CodeRay3d(ref Ray3d v) { AddValue(v.ToString()); }
    public void CodeSphere3d(ref Sphere3d v) { AddValue(v.ToString()); }
    public void CodeTriangle2d(ref Triangle2d v) { AddValue(v.ToString()); }
    public void CodeTriangle3d(ref Triangle3d v) { AddValue(v.ToString()); }

    #endregion

    #region Colors

    public void CodeC3b(ref C3b v) { AddValue(v.ToString()); }
    public void CodeC3us(ref C3us v) { AddValue(v.ToString()); }
    public void CodeC3ui(ref C3ui v) { AddValue(v.ToString()); }
    public void CodeC3f(ref C3f v) { AddValue(v.ToString()); }
    public void CodeC3d(ref C3d v) { AddValue(v.ToString()); }
    public void CodeC4b(ref C4b v) { AddValue(v.ToString()); }
    public void CodeC4us(ref C4us v) { AddValue(v.ToString()); }
    public void CodeC4ui(ref C4ui v) { AddValue(v.ToString()); }
    public void CodeC4f(ref C4f v) { AddValue(v.ToString()); }
    public void CodeC4d(ref C4d v) { AddValue(v.ToString()); }

    #endregion

    #region Trafos

    public void CodeEuclidean3f(ref Euclidean3f v) { AddValue(v.ToString()); }
    public void CodeEuclidean3d(ref Euclidean3d v) { AddValue(v.ToString()); }
    public void CodeRot2f(ref Rot2f v) { AddValue(v.ToString()); }
    public void CodeRot2d(ref Rot2d v) { AddValue(v.ToString()); }
    public void CodeRot3f(ref Rot3f v) { AddValue(v.ToString()); }
    public void CodeRot3d(ref Rot3d v) { AddValue(v.ToString()); }
    public void CodeScale3f(ref Scale3f v) { AddValue(v.ToString()); }
    public void CodeScale3d(ref Scale3d v) { AddValue(v.ToString()); }
    public void CodeShift3f(ref Shift3f v) { AddValue(v.ToString()); }
    public void CodeShift3d(ref Shift3d v) { AddValue(v.ToString()); }
    public void CodeTrafo2f(ref Trafo2f v) { AddValue(v.ToString()); }
    public void CodeTrafo2d(ref Trafo2d v) { AddValue(v.ToString()); }
    public void CodeTrafo3f(ref Trafo3f v) { AddValue(v.ToString()); }
    public void CodeTrafo3d(ref Trafo3d v) { AddValue(v.ToString()); }

    #endregion

    #region Tensors

    public void CodeVector_of_Byte_(ref Vector<byte> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_SByte_(ref Vector<sbyte> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Short_(ref Vector<short> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_UShort_(ref Vector<ushort> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Int_(ref Vector<int> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_UInt_(ref Vector<uint> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Long_(ref Vector<long> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_ULong_(ref Vector<ulong> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeULongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Float_(ref Vector<float> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFloatArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Double_(ref Vector<double> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeDoubleArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Fraction_(ref Vector<Fraction> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFractionArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V2i_(ref Vector<V2i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V2l_(ref Vector<V2l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V2f_(ref Vector<V2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V2d_(ref Vector<V2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V3i_(ref Vector<V3i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V3l_(ref Vector<V3l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V3f_(ref Vector<V3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V3d_(ref Vector<V3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V4i_(ref Vector<V4i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V4l_(ref Vector<V4l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V4f_(ref Vector<V4f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_V4d_(ref Vector<V4d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M22i_(ref Vector<M22i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M22l_(ref Vector<M22l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M22f_(ref Vector<M22f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M22d_(ref Vector<M22d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M23i_(ref Vector<M23i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M23l_(ref Vector<M23l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M23f_(ref Vector<M23f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M23d_(ref Vector<M23d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M33i_(ref Vector<M33i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M33l_(ref Vector<M33l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M33f_(ref Vector<M33f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M33d_(ref Vector<M33d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M34i_(ref Vector<M34i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M34l_(ref Vector<M34l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M34f_(ref Vector<M34f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M34d_(ref Vector<M34d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M44i_(ref Vector<M44i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M44l_(ref Vector<M44l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M44f_(ref Vector<M44f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_M44d_(ref Vector<M44d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C3b_(ref Vector<C3b> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C3us_(ref Vector<C3us> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C3ui_(ref Vector<C3ui> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C3f_(ref Vector<C3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C3d_(ref Vector<C3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C4b_(ref Vector<C4b> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C4us_(ref Vector<C4us> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C4ui_(ref Vector<C4ui> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C4f_(ref Vector<C4f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_C4d_(ref Vector<C4d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1b_(ref Vector<Range1b> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1sb_(ref Vector<Range1sb> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sbArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1s_(ref Vector<Range1s> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1us_(ref Vector<Range1us> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1i_(ref Vector<Range1i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1ui_(ref Vector<Range1ui> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1l_(ref Vector<Range1l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1ul_(ref Vector<Range1ul> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1ulArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1f_(ref Vector<Range1f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Range1d_(ref Vector<Range1d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box2i_(ref Vector<Box2i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box2l_(ref Vector<Box2l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box2f_(ref Vector<Box2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box2d_(ref Vector<Box2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box3i_(ref Vector<Box3i> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box3l_(ref Vector<Box3l> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box3f_(ref Vector<Box3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Box3d_(ref Vector<Box3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Euclidean3f_(ref Vector<Euclidean3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Euclidean3d_(ref Vector<Euclidean3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Rot2f_(ref Vector<Rot2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Rot2d_(ref Vector<Rot2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Rot3f_(ref Vector<Rot3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Rot3d_(ref Vector<Rot3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Scale3f_(ref Vector<Scale3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Scale3d_(ref Vector<Scale3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Shift3f_(ref Vector<Shift3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Shift3d_(ref Vector<Shift3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Trafo2f_(ref Vector<Trafo2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Trafo2d_(ref Vector<Trafo2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Trafo3f_(ref Vector<Trafo3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Trafo3d_(ref Vector<Trafo3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Bool_(ref Vector<bool> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBoolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Char_(ref Vector<char> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCharArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_String_(ref Vector<string> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeStringArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Type_(ref Vector<Type> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTypeArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Guid_(ref Vector<Guid> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeGuidArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Symbol_(ref Vector<Symbol> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSymbolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Circle2d_(ref Vector<Circle2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Line2d_(ref Vector<Line2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Line3d_(ref Vector<Line3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Plane2d_(ref Vector<Plane2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Plane3d_(ref Vector<Plane3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_PlaneWithPoint3d_(ref Vector<PlaneWithPoint3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Quad2d_(ref Vector<Quad2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Quad3d_(ref Vector<Quad3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Ray2d_(ref Vector<Ray2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Ray3d_(ref Vector<Ray3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Sphere3d_(ref Vector<Sphere3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Triangle2d_(ref Vector<Triangle2d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Triangle3d_(ref Vector<Triangle3d> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Circle2f_(ref Vector<Circle2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Line2f_(ref Vector<Line2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Line3f_(ref Vector<Line3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Plane2f_(ref Vector<Plane2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Plane3f_(ref Vector<Plane3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_PlaneWithPoint3f_(ref Vector<PlaneWithPoint3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Quad2f_(ref Vector<Quad2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Quad3f_(ref Vector<Quad3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Ray2f_(ref Vector<Ray2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Ray3f_(ref Vector<Ray3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Sphere3f_(ref Vector<Sphere3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Triangle2f_(ref Vector<Triangle2f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVector_of_Triangle3f_(ref Vector<Triangle3f> value)
    {
        var item = new XElement("Vector");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLong(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLong(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Byte_(ref Matrix<byte> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_SByte_(ref Matrix<sbyte> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Short_(ref Matrix<short> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_UShort_(ref Matrix<ushort> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Int_(ref Matrix<int> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_UInt_(ref Matrix<uint> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Long_(ref Matrix<long> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_ULong_(ref Matrix<ulong> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeULongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Float_(ref Matrix<float> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFloatArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Double_(ref Matrix<double> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeDoubleArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Fraction_(ref Matrix<Fraction> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFractionArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V2i_(ref Matrix<V2i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V2l_(ref Matrix<V2l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V2f_(ref Matrix<V2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V2d_(ref Matrix<V2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V3i_(ref Matrix<V3i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V3l_(ref Matrix<V3l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V3f_(ref Matrix<V3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V3d_(ref Matrix<V3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V4i_(ref Matrix<V4i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V4l_(ref Matrix<V4l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V4f_(ref Matrix<V4f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_V4d_(ref Matrix<V4d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M22i_(ref Matrix<M22i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M22l_(ref Matrix<M22l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M22f_(ref Matrix<M22f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M22d_(ref Matrix<M22d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M23i_(ref Matrix<M23i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M23l_(ref Matrix<M23l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M23f_(ref Matrix<M23f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M23d_(ref Matrix<M23d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M33i_(ref Matrix<M33i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M33l_(ref Matrix<M33l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M33f_(ref Matrix<M33f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M33d_(ref Matrix<M33d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M34i_(ref Matrix<M34i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M34l_(ref Matrix<M34l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M34f_(ref Matrix<M34f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M34d_(ref Matrix<M34d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M44i_(ref Matrix<M44i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M44l_(ref Matrix<M44l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M44f_(ref Matrix<M44f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_M44d_(ref Matrix<M44d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C3b_(ref Matrix<C3b> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C3us_(ref Matrix<C3us> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C3ui_(ref Matrix<C3ui> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C3f_(ref Matrix<C3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C3d_(ref Matrix<C3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C4b_(ref Matrix<C4b> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C4us_(ref Matrix<C4us> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C4ui_(ref Matrix<C4ui> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C4f_(ref Matrix<C4f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_C4d_(ref Matrix<C4d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1b_(ref Matrix<Range1b> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1sb_(ref Matrix<Range1sb> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sbArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1s_(ref Matrix<Range1s> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1us_(ref Matrix<Range1us> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1i_(ref Matrix<Range1i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1ui_(ref Matrix<Range1ui> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1l_(ref Matrix<Range1l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1ul_(ref Matrix<Range1ul> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1ulArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1f_(ref Matrix<Range1f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Range1d_(ref Matrix<Range1d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box2i_(ref Matrix<Box2i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box2l_(ref Matrix<Box2l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box2f_(ref Matrix<Box2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box2d_(ref Matrix<Box2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box3i_(ref Matrix<Box3i> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box3l_(ref Matrix<Box3l> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box3f_(ref Matrix<Box3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Box3d_(ref Matrix<Box3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Euclidean3f_(ref Matrix<Euclidean3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Euclidean3d_(ref Matrix<Euclidean3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Rot2f_(ref Matrix<Rot2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Rot2d_(ref Matrix<Rot2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Rot3f_(ref Matrix<Rot3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Rot3d_(ref Matrix<Rot3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Scale3f_(ref Matrix<Scale3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Scale3d_(ref Matrix<Scale3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Shift3f_(ref Matrix<Shift3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Shift3d_(ref Matrix<Shift3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Trafo2f_(ref Matrix<Trafo2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Trafo2d_(ref Matrix<Trafo2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Trafo3f_(ref Matrix<Trafo3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Trafo3d_(ref Matrix<Trafo3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Bool_(ref Matrix<bool> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBoolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Char_(ref Matrix<char> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCharArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_String_(ref Matrix<string> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeStringArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Type_(ref Matrix<Type> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTypeArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Guid_(ref Matrix<Guid> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeGuidArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Symbol_(ref Matrix<Symbol> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSymbolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Circle2d_(ref Matrix<Circle2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Line2d_(ref Matrix<Line2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Line3d_(ref Matrix<Line3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Plane2d_(ref Matrix<Plane2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Plane3d_(ref Matrix<Plane3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_PlaneWithPoint3d_(ref Matrix<PlaneWithPoint3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Quad2d_(ref Matrix<Quad2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Quad3d_(ref Matrix<Quad3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Ray2d_(ref Matrix<Ray2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Ray3d_(ref Matrix<Ray3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Sphere3d_(ref Matrix<Sphere3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Triangle2d_(ref Matrix<Triangle2d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Triangle3d_(ref Matrix<Triangle3d> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Circle2f_(ref Matrix<Circle2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Line2f_(ref Matrix<Line2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Line3f_(ref Matrix<Line3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Plane2f_(ref Matrix<Plane2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Plane3f_(ref Matrix<Plane3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_PlaneWithPoint3f_(ref Matrix<PlaneWithPoint3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Quad2f_(ref Matrix<Quad2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Quad3f_(ref Matrix<Quad3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Ray2f_(ref Matrix<Ray2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Ray3f_(ref Matrix<Ray3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Sphere3f_(ref Matrix<Sphere3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Triangle2f_(ref Matrix<Triangle2f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeMatrix_of_Triangle3f_(ref Matrix<Triangle3f> value)
    {
        var item = new XElement("Matrix");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV2l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV2l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Byte_(ref Volume<byte> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_SByte_(ref Volume<sbyte> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Short_(ref Volume<short> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_UShort_(ref Volume<ushort> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Int_(ref Volume<int> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_UInt_(ref Volume<uint> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Long_(ref Volume<long> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_ULong_(ref Volume<ulong> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeULongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Float_(ref Volume<float> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFloatArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Double_(ref Volume<double> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeDoubleArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Fraction_(ref Volume<Fraction> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFractionArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V2i_(ref Volume<V2i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V2l_(ref Volume<V2l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V2f_(ref Volume<V2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V2d_(ref Volume<V2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V3i_(ref Volume<V3i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V3l_(ref Volume<V3l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V3f_(ref Volume<V3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V3d_(ref Volume<V3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V4i_(ref Volume<V4i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V4l_(ref Volume<V4l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V4f_(ref Volume<V4f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_V4d_(ref Volume<V4d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M22i_(ref Volume<M22i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M22l_(ref Volume<M22l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M22f_(ref Volume<M22f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M22d_(ref Volume<M22d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M23i_(ref Volume<M23i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M23l_(ref Volume<M23l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M23f_(ref Volume<M23f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M23d_(ref Volume<M23d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M33i_(ref Volume<M33i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M33l_(ref Volume<M33l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M33f_(ref Volume<M33f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M33d_(ref Volume<M33d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M34i_(ref Volume<M34i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M34l_(ref Volume<M34l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M34f_(ref Volume<M34f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M34d_(ref Volume<M34d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M44i_(ref Volume<M44i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M44l_(ref Volume<M44l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M44f_(ref Volume<M44f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_M44d_(ref Volume<M44d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C3b_(ref Volume<C3b> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C3us_(ref Volume<C3us> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C3ui_(ref Volume<C3ui> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C3f_(ref Volume<C3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C3d_(ref Volume<C3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C4b_(ref Volume<C4b> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C4us_(ref Volume<C4us> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C4ui_(ref Volume<C4ui> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C4f_(ref Volume<C4f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_C4d_(ref Volume<C4d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1b_(ref Volume<Range1b> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1sb_(ref Volume<Range1sb> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sbArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1s_(ref Volume<Range1s> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1us_(ref Volume<Range1us> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1i_(ref Volume<Range1i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1ui_(ref Volume<Range1ui> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1l_(ref Volume<Range1l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1ul_(ref Volume<Range1ul> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1ulArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1f_(ref Volume<Range1f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Range1d_(ref Volume<Range1d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box2i_(ref Volume<Box2i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box2l_(ref Volume<Box2l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box2f_(ref Volume<Box2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box2d_(ref Volume<Box2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box3i_(ref Volume<Box3i> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box3l_(ref Volume<Box3l> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box3f_(ref Volume<Box3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Box3d_(ref Volume<Box3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Euclidean3f_(ref Volume<Euclidean3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Euclidean3d_(ref Volume<Euclidean3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Rot2f_(ref Volume<Rot2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Rot2d_(ref Volume<Rot2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Rot3f_(ref Volume<Rot3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Rot3d_(ref Volume<Rot3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Scale3f_(ref Volume<Scale3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Scale3d_(ref Volume<Scale3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Shift3f_(ref Volume<Shift3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Shift3d_(ref Volume<Shift3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Trafo2f_(ref Volume<Trafo2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Trafo2d_(ref Volume<Trafo2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Trafo3f_(ref Volume<Trafo3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Trafo3d_(ref Volume<Trafo3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Bool_(ref Volume<bool> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBoolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Char_(ref Volume<char> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCharArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_String_(ref Volume<string> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeStringArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Type_(ref Volume<Type> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTypeArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Guid_(ref Volume<Guid> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeGuidArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Symbol_(ref Volume<Symbol> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSymbolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Circle2d_(ref Volume<Circle2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Line2d_(ref Volume<Line2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Line3d_(ref Volume<Line3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Plane2d_(ref Volume<Plane2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Plane3d_(ref Volume<Plane3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_PlaneWithPoint3d_(ref Volume<PlaneWithPoint3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Quad2d_(ref Volume<Quad2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Quad3d_(ref Volume<Quad3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Ray2d_(ref Volume<Ray2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Ray3d_(ref Volume<Ray3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Sphere3d_(ref Volume<Sphere3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Triangle2d_(ref Volume<Triangle2d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Triangle3d_(ref Volume<Triangle3d> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Circle2f_(ref Volume<Circle2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Line2f_(ref Volume<Line2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Line3f_(ref Volume<Line3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Plane2f_(ref Volume<Plane2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Plane3f_(ref Volume<Plane3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_PlaneWithPoint3f_(ref Volume<PlaneWithPoint3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Quad2f_(ref Volume<Quad2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Quad3f_(ref Volume<Quad3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Ray2f_(ref Volume<Ray2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Ray3f_(ref Volume<Ray3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Sphere3f_(ref Volume<Sphere3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Triangle2f_(ref Volume<Triangle2f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeVolume_of_Triangle3f_(ref Volume<Triangle3f> value)
    {
        var item = new XElement("Volume");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeV3l(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeV3l(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Byte_(ref Tensor<byte> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_SByte_(ref Tensor<sbyte> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSByteArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Short_(ref Tensor<short> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_UShort_(ref Tensor<ushort> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUShortArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Int_(ref Tensor<int> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_UInt_(ref Tensor<uint> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeUIntArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Long_(ref Tensor<long> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_ULong_(ref Tensor<ulong> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeULongArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Float_(ref Tensor<float> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFloatArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Double_(ref Tensor<double> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeDoubleArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Fraction_(ref Tensor<Fraction> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeFractionArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V2i_(ref Tensor<V2i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V2l_(ref Tensor<V2l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V2f_(ref Tensor<V2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V2d_(ref Tensor<V2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V3i_(ref Tensor<V3i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V3l_(ref Tensor<V3l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V3f_(ref Tensor<V3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V3d_(ref Tensor<V3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V4i_(ref Tensor<V4i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V4l_(ref Tensor<V4l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V4f_(ref Tensor<V4f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_V4d_(ref Tensor<V4d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeV4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M22i_(ref Tensor<M22i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M22l_(ref Tensor<M22l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M22f_(ref Tensor<M22f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M22d_(ref Tensor<M22d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM22dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M23i_(ref Tensor<M23i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M23l_(ref Tensor<M23l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M23f_(ref Tensor<M23f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M23d_(ref Tensor<M23d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM23dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M33i_(ref Tensor<M33i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M33l_(ref Tensor<M33l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M33f_(ref Tensor<M33f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M33d_(ref Tensor<M33d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM33dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M34i_(ref Tensor<M34i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M34l_(ref Tensor<M34l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M34f_(ref Tensor<M34f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M34d_(ref Tensor<M34d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM34dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M44i_(ref Tensor<M44i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M44l_(ref Tensor<M44l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M44f_(ref Tensor<M44f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_M44d_(ref Tensor<M44d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeM44dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C3b_(ref Tensor<C3b> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C3us_(ref Tensor<C3us> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C3ui_(ref Tensor<C3ui> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C3f_(ref Tensor<C3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C3d_(ref Tensor<C3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C4b_(ref Tensor<C4b> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C4us_(ref Tensor<C4us> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C4ui_(ref Tensor<C4ui> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C4f_(ref Tensor<C4f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_C4d_(ref Tensor<C4d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeC4dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1b_(ref Tensor<Range1b> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1bArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1sb_(ref Tensor<Range1sb> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sbArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1s_(ref Tensor<Range1s> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1sArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1us_(ref Tensor<Range1us> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1usArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1i_(ref Tensor<Range1i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1ui_(ref Tensor<Range1ui> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1uiArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1l_(ref Tensor<Range1l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1ul_(ref Tensor<Range1ul> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1ulArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1f_(ref Tensor<Range1f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Range1d_(ref Tensor<Range1d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRange1dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box2i_(ref Tensor<Box2i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box2l_(ref Tensor<Box2l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box2f_(ref Tensor<Box2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box2d_(ref Tensor<Box2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box3i_(ref Tensor<Box3i> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3iArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box3l_(ref Tensor<Box3l> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3lArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box3f_(ref Tensor<Box3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Box3d_(ref Tensor<Box3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBox3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Euclidean3f_(ref Tensor<Euclidean3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Euclidean3d_(ref Tensor<Euclidean3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeEuclidean3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Rot2f_(ref Tensor<Rot2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Rot2d_(ref Tensor<Rot2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Rot3f_(ref Tensor<Rot3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Rot3d_(ref Tensor<Rot3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRot3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Scale3f_(ref Tensor<Scale3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Scale3d_(ref Tensor<Scale3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeScale3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Shift3f_(ref Tensor<Shift3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Shift3d_(ref Tensor<Shift3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeShift3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Trafo2f_(ref Tensor<Trafo2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Trafo2d_(ref Tensor<Trafo2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Trafo3f_(ref Tensor<Trafo3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Trafo3d_(ref Tensor<Trafo3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTrafo3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Bool_(ref Tensor<bool> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeBoolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Char_(ref Tensor<char> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCharArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_String_(ref Tensor<string> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeStringArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Type_(ref Tensor<Type> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTypeArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Guid_(ref Tensor<Guid> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeGuidArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Symbol_(ref Tensor<Symbol> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSymbolArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Circle2d_(ref Tensor<Circle2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Line2d_(ref Tensor<Line2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Line3d_(ref Tensor<Line3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Plane2d_(ref Tensor<Plane2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Plane3d_(ref Tensor<Plane3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_PlaneWithPoint3d_(ref Tensor<PlaneWithPoint3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Quad2d_(ref Tensor<Quad2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Quad3d_(ref Tensor<Quad3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Ray2d_(ref Tensor<Ray2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Ray3d_(ref Tensor<Ray3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Sphere3d_(ref Tensor<Sphere3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Triangle2d_(ref Tensor<Triangle2d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Triangle3d_(ref Tensor<Triangle3d> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3dArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Circle2f_(ref Tensor<Circle2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeCircle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Line2f_(ref Tensor<Line2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Line3f_(ref Tensor<Line3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeLine3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Plane2f_(ref Tensor<Plane2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Plane3f_(ref Tensor<Plane3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlane3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_PlaneWithPoint3f_(ref Tensor<PlaneWithPoint3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodePlaneWithPoint3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Quad2f_(ref Tensor<Quad2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Quad3f_(ref Tensor<Quad3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeQuad3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Ray2f_(ref Tensor<Ray2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Ray3f_(ref Tensor<Ray3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeRay3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Sphere3f_(ref Tensor<Sphere3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeSphere3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Triangle2f_(ref Tensor<Triangle2f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle2fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    public void CodeTensor_of_Triangle3f_(ref Tensor<Triangle3f> value)
    {
        var item = new XElement("Tensor");
        m_elementStack.Push(m_element);
        m_element = item;

        var element = new XElement("Data");
        m_elementStack.Push(m_element);
        m_element = element;

        var data = value.Data; CodeTriangle3fArray(ref data);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Origin");
        m_elementStack.Push(m_element);
        m_element = element;

        var origin = value.Origin; CodeLong(ref origin);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Length");
        m_elementStack.Push(m_element);
        m_element = element;

        var size = value.Size; CodeLongArray(ref size);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        element = new XElement("Delta");
        m_elementStack.Push(m_element);
        m_element = element;

        var delta = value.Delta; CodeLongArray(ref delta);

        m_element = m_elementStack.Pop();
        m_element.Add(element);

        m_element = m_elementStack.Pop();
        AddValue(item);
    }

    #endregion

    #region Arrays

    public void CodeV2iArray(ref V2i[] v) { CodeArrayOfStruct(v); }
    public void CodeV2uiArray(ref V2ui[] v) { CodeArrayOfStruct(v); }
    public void CodeV2lArray(ref V2l[] v) { CodeArrayOfStruct(v); }
    public void CodeV2fArray(ref V2f[] v) { CodeArrayOfStruct(v); }
    public void CodeV2dArray(ref V2d[] v) { CodeArrayOfStruct(v); }
    public void CodeV3iArray(ref V3i[] v) { CodeArrayOfStruct(v); }
    public void CodeV3uiArray(ref V3ui[] v) { CodeArrayOfStruct(v); }
    public void CodeV3lArray(ref V3l[] v) { CodeArrayOfStruct(v); }
    public void CodeV3fArray(ref V3f[] v) { CodeArrayOfStruct(v); }
    public void CodeV3dArray(ref V3d[] v) { CodeArrayOfStruct(v); }
    public void CodeV4iArray(ref V4i[] v) { CodeArrayOfStruct(v); }
    public void CodeV4uiArray(ref V4ui[] v) { CodeArrayOfStruct(v); }
    public void CodeV4lArray(ref V4l[] v) { CodeArrayOfStruct(v); }
    public void CodeV4fArray(ref V4f[] v) { CodeArrayOfStruct(v); }
    public void CodeV4dArray(ref V4d[] v) { CodeArrayOfStruct(v); }
    public void CodeM22iArray(ref M22i[] v) { CodeArrayOfStruct(v); }
    public void CodeM22lArray(ref M22l[] v) { CodeArrayOfStruct(v); }
    public void CodeM22fArray(ref M22f[] v) { CodeArrayOfStruct(v); }
    public void CodeM22dArray(ref M22d[] v) { CodeArrayOfStruct(v); }
    public void CodeM23iArray(ref M23i[] v) { CodeArrayOfStruct(v); }
    public void CodeM23lArray(ref M23l[] v) { CodeArrayOfStruct(v); }
    public void CodeM23fArray(ref M23f[] v) { CodeArrayOfStruct(v); }
    public void CodeM23dArray(ref M23d[] v) { CodeArrayOfStruct(v); }
    public void CodeM33iArray(ref M33i[] v) { CodeArrayOfStruct(v); }
    public void CodeM33lArray(ref M33l[] v) { CodeArrayOfStruct(v); }
    public void CodeM33fArray(ref M33f[] v) { CodeArrayOfStruct(v); }
    public void CodeM33dArray(ref M33d[] v) { CodeArrayOfStruct(v); }
    public void CodeM34iArray(ref M34i[] v) { CodeArrayOfStruct(v); }
    public void CodeM34lArray(ref M34l[] v) { CodeArrayOfStruct(v); }
    public void CodeM34fArray(ref M34f[] v) { CodeArrayOfStruct(v); }
    public void CodeM34dArray(ref M34d[] v) { CodeArrayOfStruct(v); }
    public void CodeM44iArray(ref M44i[] v) { CodeArrayOfStruct(v); }
    public void CodeM44lArray(ref M44l[] v) { CodeArrayOfStruct(v); }
    public void CodeM44fArray(ref M44f[] v) { CodeArrayOfStruct(v); }
    public void CodeM44dArray(ref M44d[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1bArray(ref Range1b[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1sbArray(ref Range1sb[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1sArray(ref Range1s[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1usArray(ref Range1us[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1iArray(ref Range1i[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1uiArray(ref Range1ui[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1lArray(ref Range1l[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1ulArray(ref Range1ul[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1fArray(ref Range1f[] v) { CodeArrayOfStruct(v); }
    public void CodeRange1dArray(ref Range1d[] v) { CodeArrayOfStruct(v); }
    public void CodeBox2iArray(ref Box2i[] v) { CodeArrayOfStruct(v); }
    public void CodeBox2lArray(ref Box2l[] v) { CodeArrayOfStruct(v); }
    public void CodeBox2fArray(ref Box2f[] v) { CodeArrayOfStruct(v); }
    public void CodeBox2dArray(ref Box2d[] v) { CodeArrayOfStruct(v); }
    public void CodeBox3iArray(ref Box3i[] v) { CodeArrayOfStruct(v); }
    public void CodeBox3lArray(ref Box3l[] v) { CodeArrayOfStruct(v); }
    public void CodeBox3fArray(ref Box3f[] v) { CodeArrayOfStruct(v); }
    public void CodeBox3dArray(ref Box3d[] v) { CodeArrayOfStruct(v); }
    public void CodeC3bArray(ref C3b[] v) { CodeArrayOfStruct(v); }
    public void CodeC3usArray(ref C3us[] v) { CodeArrayOfStruct(v); }
    public void CodeC3uiArray(ref C3ui[] v) { CodeArrayOfStruct(v); }
    public void CodeC3fArray(ref C3f[] v) { CodeArrayOfStruct(v); }
    public void CodeC3dArray(ref C3d[] v) { CodeArrayOfStruct(v); }
    public void CodeC4bArray(ref C4b[] v) { CodeArrayOfStruct(v); }
    public void CodeC4usArray(ref C4us[] v) { CodeArrayOfStruct(v); }
    public void CodeC4uiArray(ref C4ui[] v) { CodeArrayOfStruct(v); }
    public void CodeC4fArray(ref C4f[] v) { CodeArrayOfStruct(v); }
    public void CodeC4dArray(ref C4d[] v) { CodeArrayOfStruct(v); }
    public void CodeEuclidean3fArray(ref Euclidean3f[] v) { CodeArrayOfStruct(v); }
    public void CodeEuclidean3dArray(ref Euclidean3d[] v) { CodeArrayOfStruct(v); }
    public void CodeRot2fArray(ref Rot2f[] v) { CodeArrayOfStruct(v); }
    public void CodeRot2dArray(ref Rot2d[] v) { CodeArrayOfStruct(v); }
    public void CodeRot3fArray(ref Rot3f[] v) { CodeArrayOfStruct(v); }
    public void CodeRot3dArray(ref Rot3d[] v) { CodeArrayOfStruct(v); }
    public void CodeScale3fArray(ref Scale3f[] v) { CodeArrayOfStruct(v); }
    public void CodeScale3dArray(ref Scale3d[] v) { CodeArrayOfStruct(v); }
    public void CodeShift3fArray(ref Shift3f[] v) { CodeArrayOfStruct(v); }
    public void CodeShift3dArray(ref Shift3d[] v) { CodeArrayOfStruct(v); }
    public void CodeTrafo2fArray(ref Trafo2f[] v) { CodeArrayOfStruct(v); }
    public void CodeTrafo2dArray(ref Trafo2d[] v) { CodeArrayOfStruct(v); }
    public void CodeTrafo3fArray(ref Trafo3f[] v) { CodeArrayOfStruct(v); }
    public void CodeTrafo3dArray(ref Trafo3d[] v) { CodeArrayOfStruct(v); }
    public void CodeCircle2dArray(ref Circle2d[] v) { CodeArrayOfStruct(v); }
    public void CodeLine2dArray(ref Line2d[] v) { CodeArrayOfStruct(v); }
    public void CodeLine3dArray(ref Line3d[] v) { CodeArrayOfStruct(v); }
    public void CodePlane2dArray(ref Plane2d[] v) { CodeArrayOfStruct(v); }
    public void CodePlane3dArray(ref Plane3d[] v) { CodeArrayOfStruct(v); }
    public void CodePlaneWithPoint3dArray(ref PlaneWithPoint3d[] v) { CodeArrayOfStruct(v); }
    public void CodeQuad2dArray(ref Quad2d[] v) { CodeArrayOfStruct(v); }
    public void CodeQuad3dArray(ref Quad3d[] v) { CodeArrayOfStruct(v); }
    public void CodeRay2dArray(ref Ray2d[] v) { CodeArrayOfStruct(v); }
    public void CodeRay3dArray(ref Ray3d[] v) { CodeArrayOfStruct(v); }
    public void CodeSphere3dArray(ref Sphere3d[] v) { CodeArrayOfStruct(v); }
    public void CodeTriangle2dArray(ref Triangle2d[] v) { CodeArrayOfStruct(v); }
    public void CodeTriangle3dArray(ref Triangle3d[] v) { CodeArrayOfStruct(v); }
    public void CodeCircle2fArray(ref Circle2f[] v) { CodeArrayOfStruct(v); }
    public void CodeLine2fArray(ref Line2f[] v) { CodeArrayOfStruct(v); }
    public void CodeLine3fArray(ref Line3f[] v) { CodeArrayOfStruct(v); }
    public void CodePlane2fArray(ref Plane2f[] v) { CodeArrayOfStruct(v); }
    public void CodePlane3fArray(ref Plane3f[] v) { CodeArrayOfStruct(v); }
    public void CodePlaneWithPoint3fArray(ref PlaneWithPoint3f[] v) { CodeArrayOfStruct(v); }
    public void CodeQuad2fArray(ref Quad2f[] v) { CodeArrayOfStruct(v); }
    public void CodeQuad3fArray(ref Quad3f[] v) { CodeArrayOfStruct(v); }
    public void CodeRay2fArray(ref Ray2f[] v) { CodeArrayOfStruct(v); }
    public void CodeRay3fArray(ref Ray3f[] v) { CodeArrayOfStruct(v); }
    public void CodeSphere3fArray(ref Sphere3f[] v) { CodeArrayOfStruct(v); }
    public void CodeTriangle2fArray(ref Triangle2f[] v) { CodeArrayOfStruct(v); }
    public void CodeTriangle3fArray(ref Triangle3f[] v) { CodeArrayOfStruct(v); }

    #endregion

    #region Multi-Dimensional Arrays

    public void CodeByteArray2d(ref byte[,] v) { throw new NotImplementedException(); }
    public void CodeByteArray3d(ref byte[, ,] v) { throw new NotImplementedException(); }
    public void CodeSByteArray2d(ref sbyte[,] v) { throw new NotImplementedException(); }
    public void CodeSByteArray3d(ref sbyte[, ,] v) { throw new NotImplementedException(); }
    public void CodeShortArray2d(ref short[,] v) { throw new NotImplementedException(); }
    public void CodeShortArray3d(ref short[, ,] v) { throw new NotImplementedException(); }
    public void CodeUShortArray2d(ref ushort[,] v) { throw new NotImplementedException(); }
    public void CodeUShortArray3d(ref ushort[, ,] v) { throw new NotImplementedException(); }
    public void CodeIntArray2d(ref int[,] v) { throw new NotImplementedException(); }
    public void CodeIntArray3d(ref int[, ,] v) { throw new NotImplementedException(); }
    public void CodeUIntArray2d(ref uint[,] v) { throw new NotImplementedException(); }
    public void CodeUIntArray3d(ref uint[, ,] v) { throw new NotImplementedException(); }
    public void CodeLongArray2d(ref long[,] v) { throw new NotImplementedException(); }
    public void CodeLongArray3d(ref long[, ,] v) { throw new NotImplementedException(); }
    public void CodeULongArray2d(ref ulong[,] v) { throw new NotImplementedException(); }
    public void CodeULongArray3d(ref ulong[, ,] v) { throw new NotImplementedException(); }
    public void CodeFloatArray2d(ref float[,] v) { throw new NotImplementedException(); }
    public void CodeFloatArray3d(ref float[, ,] v) { throw new NotImplementedException(); }
    public void CodeDoubleArray2d(ref double[,] v) { throw new NotImplementedException(); }
    public void CodeDoubleArray3d(ref double[, ,] v) { throw new NotImplementedException(); }
    public void CodeFractionArray2d(ref Fraction[,] v) { throw new NotImplementedException(); }
    public void CodeFractionArray3d(ref Fraction[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2iArray2d(ref V2i[,] v) { throw new NotImplementedException(); }
    public void CodeV2iArray3d(ref V2i[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2lArray2d(ref V2l[,] v) { throw new NotImplementedException(); }
    public void CodeV2lArray3d(ref V2l[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2fArray2d(ref V2f[,] v) { throw new NotImplementedException(); }
    public void CodeV2fArray3d(ref V2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeV2dArray2d(ref V2d[,] v) { throw new NotImplementedException(); }
    public void CodeV2dArray3d(ref V2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3iArray2d(ref V3i[,] v) { throw new NotImplementedException(); }
    public void CodeV3iArray3d(ref V3i[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3lArray2d(ref V3l[,] v) { throw new NotImplementedException(); }
    public void CodeV3lArray3d(ref V3l[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3fArray2d(ref V3f[,] v) { throw new NotImplementedException(); }
    public void CodeV3fArray3d(ref V3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeV3dArray2d(ref V3d[,] v) { throw new NotImplementedException(); }
    public void CodeV3dArray3d(ref V3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4iArray2d(ref V4i[,] v) { throw new NotImplementedException(); }
    public void CodeV4iArray3d(ref V4i[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4lArray2d(ref V4l[,] v) { throw new NotImplementedException(); }
    public void CodeV4lArray3d(ref V4l[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4fArray2d(ref V4f[,] v) { throw new NotImplementedException(); }
    public void CodeV4fArray3d(ref V4f[, ,] v) { throw new NotImplementedException(); }
    public void CodeV4dArray2d(ref V4d[,] v) { throw new NotImplementedException(); }
    public void CodeV4dArray3d(ref V4d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22iArray2d(ref M22i[,] v) { throw new NotImplementedException(); }
    public void CodeM22iArray3d(ref M22i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22lArray2d(ref M22l[,] v) { throw new NotImplementedException(); }
    public void CodeM22lArray3d(ref M22l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22fArray2d(ref M22f[,] v) { throw new NotImplementedException(); }
    public void CodeM22fArray3d(ref M22f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM22dArray2d(ref M22d[,] v) { throw new NotImplementedException(); }
    public void CodeM22dArray3d(ref M22d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23iArray2d(ref M23i[,] v) { throw new NotImplementedException(); }
    public void CodeM23iArray3d(ref M23i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23lArray2d(ref M23l[,] v) { throw new NotImplementedException(); }
    public void CodeM23lArray3d(ref M23l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23fArray2d(ref M23f[,] v) { throw new NotImplementedException(); }
    public void CodeM23fArray3d(ref M23f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM23dArray2d(ref M23d[,] v) { throw new NotImplementedException(); }
    public void CodeM23dArray3d(ref M23d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33iArray2d(ref M33i[,] v) { throw new NotImplementedException(); }
    public void CodeM33iArray3d(ref M33i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33lArray2d(ref M33l[,] v) { throw new NotImplementedException(); }
    public void CodeM33lArray3d(ref M33l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33fArray2d(ref M33f[,] v) { throw new NotImplementedException(); }
    public void CodeM33fArray3d(ref M33f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM33dArray2d(ref M33d[,] v) { throw new NotImplementedException(); }
    public void CodeM33dArray3d(ref M33d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34iArray2d(ref M34i[,] v) { throw new NotImplementedException(); }
    public void CodeM34iArray3d(ref M34i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34lArray2d(ref M34l[,] v) { throw new NotImplementedException(); }
    public void CodeM34lArray3d(ref M34l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34fArray2d(ref M34f[,] v) { throw new NotImplementedException(); }
    public void CodeM34fArray3d(ref M34f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM34dArray2d(ref M34d[,] v) { throw new NotImplementedException(); }
    public void CodeM34dArray3d(ref M34d[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44iArray2d(ref M44i[,] v) { throw new NotImplementedException(); }
    public void CodeM44iArray3d(ref M44i[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44lArray2d(ref M44l[,] v) { throw new NotImplementedException(); }
    public void CodeM44lArray3d(ref M44l[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44fArray2d(ref M44f[,] v) { throw new NotImplementedException(); }
    public void CodeM44fArray3d(ref M44f[, ,] v) { throw new NotImplementedException(); }
    public void CodeM44dArray2d(ref M44d[,] v) { throw new NotImplementedException(); }
    public void CodeM44dArray3d(ref M44d[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3bArray2d(ref C3b[,] v) { throw new NotImplementedException(); }
    public void CodeC3bArray3d(ref C3b[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3usArray2d(ref C3us[,] v) { throw new NotImplementedException(); }
    public void CodeC3usArray3d(ref C3us[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3uiArray2d(ref C3ui[,] v) { throw new NotImplementedException(); }
    public void CodeC3uiArray3d(ref C3ui[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3fArray2d(ref C3f[,] v) { throw new NotImplementedException(); }
    public void CodeC3fArray3d(ref C3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeC3dArray2d(ref C3d[,] v) { throw new NotImplementedException(); }
    public void CodeC3dArray3d(ref C3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4bArray2d(ref C4b[,] v) { throw new NotImplementedException(); }
    public void CodeC4bArray3d(ref C4b[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4usArray2d(ref C4us[,] v) { throw new NotImplementedException(); }
    public void CodeC4usArray3d(ref C4us[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4uiArray2d(ref C4ui[,] v) { throw new NotImplementedException(); }
    public void CodeC4uiArray3d(ref C4ui[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4fArray2d(ref C4f[,] v) { throw new NotImplementedException(); }
    public void CodeC4fArray3d(ref C4f[, ,] v) { throw new NotImplementedException(); }
    public void CodeC4dArray2d(ref C4d[,] v) { throw new NotImplementedException(); }
    public void CodeC4dArray3d(ref C4d[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1bArray2d(ref Range1b[,] v) { throw new NotImplementedException(); }
    public void CodeRange1bArray3d(ref Range1b[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArray2d(ref Range1sb[,] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArray3d(ref Range1sb[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1sArray2d(ref Range1s[,] v) { throw new NotImplementedException(); }
    public void CodeRange1sArray3d(ref Range1s[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1usArray2d(ref Range1us[,] v) { throw new NotImplementedException(); }
    public void CodeRange1usArray3d(ref Range1us[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1iArray2d(ref Range1i[,] v) { throw new NotImplementedException(); }
    public void CodeRange1iArray3d(ref Range1i[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArray2d(ref Range1ui[,] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArray3d(ref Range1ui[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1lArray2d(ref Range1l[,] v) { throw new NotImplementedException(); }
    public void CodeRange1lArray3d(ref Range1l[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArray2d(ref Range1ul[,] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArray3d(ref Range1ul[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1fArray2d(ref Range1f[,] v) { throw new NotImplementedException(); }
    public void CodeRange1fArray3d(ref Range1f[, ,] v) { throw new NotImplementedException(); }
    public void CodeRange1dArray2d(ref Range1d[,] v) { throw new NotImplementedException(); }
    public void CodeRange1dArray3d(ref Range1d[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2iArray2d(ref Box2i[,] v) { throw new NotImplementedException(); }
    public void CodeBox2iArray3d(ref Box2i[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2lArray2d(ref Box2l[,] v) { throw new NotImplementedException(); }
    public void CodeBox2lArray3d(ref Box2l[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2fArray2d(ref Box2f[,] v) { throw new NotImplementedException(); }
    public void CodeBox2fArray3d(ref Box2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox2dArray2d(ref Box2d[,] v) { throw new NotImplementedException(); }
    public void CodeBox2dArray3d(ref Box2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3iArray2d(ref Box3i[,] v) { throw new NotImplementedException(); }
    public void CodeBox3iArray3d(ref Box3i[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3lArray2d(ref Box3l[,] v) { throw new NotImplementedException(); }
    public void CodeBox3lArray3d(ref Box3l[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3fArray2d(ref Box3f[,] v) { throw new NotImplementedException(); }
    public void CodeBox3fArray3d(ref Box3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeBox3dArray2d(ref Box3d[,] v) { throw new NotImplementedException(); }
    public void CodeBox3dArray3d(ref Box3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArray2d(ref Euclidean3f[,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArray3d(ref Euclidean3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArray2d(ref Euclidean3d[,] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArray3d(ref Euclidean3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot2fArray2d(ref Rot2f[,] v) { throw new NotImplementedException(); }
    public void CodeRot2fArray3d(ref Rot2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot2dArray2d(ref Rot2d[,] v) { throw new NotImplementedException(); }
    public void CodeRot2dArray3d(ref Rot2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot3fArray2d(ref Rot3f[,] v) { throw new NotImplementedException(); }
    public void CodeRot3fArray3d(ref Rot3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeRot3dArray2d(ref Rot3d[,] v) { throw new NotImplementedException(); }
    public void CodeRot3dArray3d(ref Rot3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeScale3fArray2d(ref Scale3f[,] v) { throw new NotImplementedException(); }
    public void CodeScale3fArray3d(ref Scale3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeScale3dArray2d(ref Scale3d[,] v) { throw new NotImplementedException(); }
    public void CodeScale3dArray3d(ref Scale3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeShift3fArray2d(ref Shift3f[,] v) { throw new NotImplementedException(); }
    public void CodeShift3fArray3d(ref Shift3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeShift3dArray2d(ref Shift3d[,] v) { throw new NotImplementedException(); }
    public void CodeShift3dArray3d(ref Shift3d[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArray2d(ref Trafo2f[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArray3d(ref Trafo2f[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArray2d(ref Trafo2d[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArray3d(ref Trafo2d[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArray2d(ref Trafo3f[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArray3d(ref Trafo3f[, ,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArray2d(ref Trafo3d[,] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArray3d(ref Trafo3d[, ,] v) { throw new NotImplementedException(); }

    #endregion

    #region Multi-Dimensional Arrays

    public void CodeByteArrayArray(ref byte[][] v) { throw new NotImplementedException(); }
    public void CodeByteArrayArrayArray(ref byte[][][] v) { throw new NotImplementedException(); }
    public void CodeSByteArrayArray(ref sbyte[][] v) { throw new NotImplementedException(); }
    public void CodeSByteArrayArrayArray(ref sbyte[][][] v) { throw new NotImplementedException(); }
    public void CodeShortArrayArray(ref short[][] v) { throw new NotImplementedException(); }
    public void CodeShortArrayArrayArray(ref short[][][] v) { throw new NotImplementedException(); }
    public void CodeUShortArrayArray(ref ushort[][] v) { throw new NotImplementedException(); }
    public void CodeUShortArrayArrayArray(ref ushort[][][] v) { throw new NotImplementedException(); }
    public void CodeIntArrayArray(ref int[][] v) { throw new NotImplementedException(); }
    public void CodeIntArrayArrayArray(ref int[][][] v) { throw new NotImplementedException(); }
    public void CodeUIntArrayArray(ref uint[][] v) { throw new NotImplementedException(); }
    public void CodeUIntArrayArrayArray(ref uint[][][] v) { throw new NotImplementedException(); }
    public void CodeLongArrayArray(ref long[][] v) { throw new NotImplementedException(); }
    public void CodeLongArrayArrayArray(ref long[][][] v) { throw new NotImplementedException(); }
    public void CodeULongArrayArray(ref ulong[][] v) { throw new NotImplementedException(); }
    public void CodeULongArrayArrayArray(ref ulong[][][] v) { throw new NotImplementedException(); }
    public void CodeFloatArrayArray(ref float[][] v) { throw new NotImplementedException(); }
    public void CodeFloatArrayArrayArray(ref float[][][] v) { throw new NotImplementedException(); }
    public void CodeDoubleArrayArray(ref double[][] v) { throw new NotImplementedException(); }
    public void CodeDoubleArrayArrayArray(ref double[][][] v) { throw new NotImplementedException(); }
    public void CodeFractionArrayArray(ref Fraction[][] v) { throw new NotImplementedException(); }
    public void CodeFractionArrayArrayArray(ref Fraction[][][] v) { throw new NotImplementedException(); }
    public void CodeV2iArrayArray(ref V2i[][] v) { throw new NotImplementedException(); }
    public void CodeV2iArrayArrayArray(ref V2i[][][] v) { throw new NotImplementedException(); }
    public void CodeV2lArrayArray(ref V2l[][] v) { throw new NotImplementedException(); }
    public void CodeV2lArrayArrayArray(ref V2l[][][] v) { throw new NotImplementedException(); }
    public void CodeV2fArrayArray(ref V2f[][] v) { throw new NotImplementedException(); }
    public void CodeV2fArrayArrayArray(ref V2f[][][] v) { throw new NotImplementedException(); }
    public void CodeV2dArrayArray(ref V2d[][] v) { throw new NotImplementedException(); }
    public void CodeV2dArrayArrayArray(ref V2d[][][] v) { throw new NotImplementedException(); }
    public void CodeV3iArrayArray(ref V3i[][] v) { throw new NotImplementedException(); }
    public void CodeV3iArrayArrayArray(ref V3i[][][] v) { throw new NotImplementedException(); }
    public void CodeV3lArrayArray(ref V3l[][] v) { throw new NotImplementedException(); }
    public void CodeV3lArrayArrayArray(ref V3l[][][] v) { throw new NotImplementedException(); }
    public void CodeV3fArrayArray(ref V3f[][] v) { throw new NotImplementedException(); }
    public void CodeV3fArrayArrayArray(ref V3f[][][] v) { throw new NotImplementedException(); }
    public void CodeV3dArrayArray(ref V3d[][] v) { throw new NotImplementedException(); }
    public void CodeV3dArrayArrayArray(ref V3d[][][] v) { throw new NotImplementedException(); }
    public void CodeV4iArrayArray(ref V4i[][] v) { throw new NotImplementedException(); }
    public void CodeV4iArrayArrayArray(ref V4i[][][] v) { throw new NotImplementedException(); }
    public void CodeV4lArrayArray(ref V4l[][] v) { throw new NotImplementedException(); }
    public void CodeV4lArrayArrayArray(ref V4l[][][] v) { throw new NotImplementedException(); }
    public void CodeV4fArrayArray(ref V4f[][] v) { throw new NotImplementedException(); }
    public void CodeV4fArrayArrayArray(ref V4f[][][] v) { throw new NotImplementedException(); }
    public void CodeV4dArrayArray(ref V4d[][] v) { throw new NotImplementedException(); }
    public void CodeV4dArrayArrayArray(ref V4d[][][] v) { throw new NotImplementedException(); }
    public void CodeM22iArrayArray(ref M22i[][] v) { throw new NotImplementedException(); }
    public void CodeM22iArrayArrayArray(ref M22i[][][] v) { throw new NotImplementedException(); }
    public void CodeM22lArrayArray(ref M22l[][] v) { throw new NotImplementedException(); }
    public void CodeM22lArrayArrayArray(ref M22l[][][] v) { throw new NotImplementedException(); }
    public void CodeM22fArrayArray(ref M22f[][] v) { throw new NotImplementedException(); }
    public void CodeM22fArrayArrayArray(ref M22f[][][] v) { throw new NotImplementedException(); }
    public void CodeM22dArrayArray(ref M22d[][] v) { throw new NotImplementedException(); }
    public void CodeM22dArrayArrayArray(ref M22d[][][] v) { throw new NotImplementedException(); }
    public void CodeM23iArrayArray(ref M23i[][] v) { throw new NotImplementedException(); }
    public void CodeM23iArrayArrayArray(ref M23i[][][] v) { throw new NotImplementedException(); }
    public void CodeM23lArrayArray(ref M23l[][] v) { throw new NotImplementedException(); }
    public void CodeM23lArrayArrayArray(ref M23l[][][] v) { throw new NotImplementedException(); }
    public void CodeM23fArrayArray(ref M23f[][] v) { throw new NotImplementedException(); }
    public void CodeM23fArrayArrayArray(ref M23f[][][] v) { throw new NotImplementedException(); }
    public void CodeM23dArrayArray(ref M23d[][] v) { throw new NotImplementedException(); }
    public void CodeM23dArrayArrayArray(ref M23d[][][] v) { throw new NotImplementedException(); }
    public void CodeM33iArrayArray(ref M33i[][] v) { throw new NotImplementedException(); }
    public void CodeM33iArrayArrayArray(ref M33i[][][] v) { throw new NotImplementedException(); }
    public void CodeM33lArrayArray(ref M33l[][] v) { throw new NotImplementedException(); }
    public void CodeM33lArrayArrayArray(ref M33l[][][] v) { throw new NotImplementedException(); }
    public void CodeM33fArrayArray(ref M33f[][] v) { throw new NotImplementedException(); }
    public void CodeM33fArrayArrayArray(ref M33f[][][] v) { throw new NotImplementedException(); }
    public void CodeM33dArrayArray(ref M33d[][] v) { throw new NotImplementedException(); }
    public void CodeM33dArrayArrayArray(ref M33d[][][] v) { throw new NotImplementedException(); }
    public void CodeM34iArrayArray(ref M34i[][] v) { throw new NotImplementedException(); }
    public void CodeM34iArrayArrayArray(ref M34i[][][] v) { throw new NotImplementedException(); }
    public void CodeM34lArrayArray(ref M34l[][] v) { throw new NotImplementedException(); }
    public void CodeM34lArrayArrayArray(ref M34l[][][] v) { throw new NotImplementedException(); }
    public void CodeM34fArrayArray(ref M34f[][] v) { throw new NotImplementedException(); }
    public void CodeM34fArrayArrayArray(ref M34f[][][] v) { throw new NotImplementedException(); }
    public void CodeM34dArrayArray(ref M34d[][] v) { throw new NotImplementedException(); }
    public void CodeM34dArrayArrayArray(ref M34d[][][] v) { throw new NotImplementedException(); }
    public void CodeM44iArrayArray(ref M44i[][] v) { throw new NotImplementedException(); }
    public void CodeM44iArrayArrayArray(ref M44i[][][] v) { throw new NotImplementedException(); }
    public void CodeM44lArrayArray(ref M44l[][] v) { throw new NotImplementedException(); }
    public void CodeM44lArrayArrayArray(ref M44l[][][] v) { throw new NotImplementedException(); }
    public void CodeM44fArrayArray(ref M44f[][] v) { throw new NotImplementedException(); }
    public void CodeM44fArrayArrayArray(ref M44f[][][] v) { throw new NotImplementedException(); }
    public void CodeM44dArrayArray(ref M44d[][] v) { throw new NotImplementedException(); }
    public void CodeM44dArrayArrayArray(ref M44d[][][] v) { throw new NotImplementedException(); }
    public void CodeC3bArrayArray(ref C3b[][] v) { throw new NotImplementedException(); }
    public void CodeC3bArrayArrayArray(ref C3b[][][] v) { throw new NotImplementedException(); }
    public void CodeC3usArrayArray(ref C3us[][] v) { throw new NotImplementedException(); }
    public void CodeC3usArrayArrayArray(ref C3us[][][] v) { throw new NotImplementedException(); }
    public void CodeC3uiArrayArray(ref C3ui[][] v) { throw new NotImplementedException(); }
    public void CodeC3uiArrayArrayArray(ref C3ui[][][] v) { throw new NotImplementedException(); }
    public void CodeC3fArrayArray(ref C3f[][] v) { throw new NotImplementedException(); }
    public void CodeC3fArrayArrayArray(ref C3f[][][] v) { throw new NotImplementedException(); }
    public void CodeC3dArrayArray(ref C3d[][] v) { throw new NotImplementedException(); }
    public void CodeC3dArrayArrayArray(ref C3d[][][] v) { throw new NotImplementedException(); }
    public void CodeC4bArrayArray(ref C4b[][] v) { throw new NotImplementedException(); }
    public void CodeC4bArrayArrayArray(ref C4b[][][] v) { throw new NotImplementedException(); }
    public void CodeC4usArrayArray(ref C4us[][] v) { throw new NotImplementedException(); }
    public void CodeC4usArrayArrayArray(ref C4us[][][] v) { throw new NotImplementedException(); }
    public void CodeC4uiArrayArray(ref C4ui[][] v) { throw new NotImplementedException(); }
    public void CodeC4uiArrayArrayArray(ref C4ui[][][] v) { throw new NotImplementedException(); }
    public void CodeC4fArrayArray(ref C4f[][] v) { throw new NotImplementedException(); }
    public void CodeC4fArrayArrayArray(ref C4f[][][] v) { throw new NotImplementedException(); }
    public void CodeC4dArrayArray(ref C4d[][] v) { throw new NotImplementedException(); }
    public void CodeC4dArrayArrayArray(ref C4d[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1bArrayArray(ref Range1b[][] v) { throw new NotImplementedException(); }
    public void CodeRange1bArrayArrayArray(ref Range1b[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArrayArray(ref Range1sb[][] v) { throw new NotImplementedException(); }
    public void CodeRange1sbArrayArrayArray(ref Range1sb[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1sArrayArray(ref Range1s[][] v) { throw new NotImplementedException(); }
    public void CodeRange1sArrayArrayArray(ref Range1s[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1usArrayArray(ref Range1us[][] v) { throw new NotImplementedException(); }
    public void CodeRange1usArrayArrayArray(ref Range1us[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1iArrayArray(ref Range1i[][] v) { throw new NotImplementedException(); }
    public void CodeRange1iArrayArrayArray(ref Range1i[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArrayArray(ref Range1ui[][] v) { throw new NotImplementedException(); }
    public void CodeRange1uiArrayArrayArray(ref Range1ui[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1lArrayArray(ref Range1l[][] v) { throw new NotImplementedException(); }
    public void CodeRange1lArrayArrayArray(ref Range1l[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArrayArray(ref Range1ul[][] v) { throw new NotImplementedException(); }
    public void CodeRange1ulArrayArrayArray(ref Range1ul[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1fArrayArray(ref Range1f[][] v) { throw new NotImplementedException(); }
    public void CodeRange1fArrayArrayArray(ref Range1f[][][] v) { throw new NotImplementedException(); }
    public void CodeRange1dArrayArray(ref Range1d[][] v) { throw new NotImplementedException(); }
    public void CodeRange1dArrayArrayArray(ref Range1d[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2iArrayArray(ref Box2i[][] v) { throw new NotImplementedException(); }
    public void CodeBox2iArrayArrayArray(ref Box2i[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2lArrayArray(ref Box2l[][] v) { throw new NotImplementedException(); }
    public void CodeBox2lArrayArrayArray(ref Box2l[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2fArrayArray(ref Box2f[][] v) { throw new NotImplementedException(); }
    public void CodeBox2fArrayArrayArray(ref Box2f[][][] v) { throw new NotImplementedException(); }
    public void CodeBox2dArrayArray(ref Box2d[][] v) { throw new NotImplementedException(); }
    public void CodeBox2dArrayArrayArray(ref Box2d[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3iArrayArray(ref Box3i[][] v) { throw new NotImplementedException(); }
    public void CodeBox3iArrayArrayArray(ref Box3i[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3lArrayArray(ref Box3l[][] v) { throw new NotImplementedException(); }
    public void CodeBox3lArrayArrayArray(ref Box3l[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3fArrayArray(ref Box3f[][] v) { throw new NotImplementedException(); }
    public void CodeBox3fArrayArrayArray(ref Box3f[][][] v) { throw new NotImplementedException(); }
    public void CodeBox3dArrayArray(ref Box3d[][] v) { throw new NotImplementedException(); }
    public void CodeBox3dArrayArrayArray(ref Box3d[][][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArrayArray(ref Euclidean3f[][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3fArrayArrayArray(ref Euclidean3f[][][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArrayArray(ref Euclidean3d[][] v) { throw new NotImplementedException(); }
    public void CodeEuclidean3dArrayArrayArray(ref Euclidean3d[][][] v) { throw new NotImplementedException(); }
    public void CodeRot2fArrayArray(ref Rot2f[][] v) { throw new NotImplementedException(); }
    public void CodeRot2fArrayArrayArray(ref Rot2f[][][] v) { throw new NotImplementedException(); }
    public void CodeRot2dArrayArray(ref Rot2d[][] v) { throw new NotImplementedException(); }
    public void CodeRot2dArrayArrayArray(ref Rot2d[][][] v) { throw new NotImplementedException(); }
    public void CodeRot3fArrayArray(ref Rot3f[][] v) { throw new NotImplementedException(); }
    public void CodeRot3fArrayArrayArray(ref Rot3f[][][] v) { throw new NotImplementedException(); }
    public void CodeRot3dArrayArray(ref Rot3d[][] v) { throw new NotImplementedException(); }
    public void CodeRot3dArrayArrayArray(ref Rot3d[][][] v) { throw new NotImplementedException(); }
    public void CodeScale3fArrayArray(ref Scale3f[][] v) { throw new NotImplementedException(); }
    public void CodeScale3fArrayArrayArray(ref Scale3f[][][] v) { throw new NotImplementedException(); }
    public void CodeScale3dArrayArray(ref Scale3d[][] v) { throw new NotImplementedException(); }
    public void CodeScale3dArrayArrayArray(ref Scale3d[][][] v) { throw new NotImplementedException(); }
    public void CodeShift3fArrayArray(ref Shift3f[][] v) { throw new NotImplementedException(); }
    public void CodeShift3fArrayArrayArray(ref Shift3f[][][] v) { throw new NotImplementedException(); }
    public void CodeShift3dArrayArray(ref Shift3d[][] v) { throw new NotImplementedException(); }
    public void CodeShift3dArrayArrayArray(ref Shift3d[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArrayArray(ref Trafo2f[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2fArrayArrayArray(ref Trafo2f[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArrayArray(ref Trafo2d[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo2dArrayArrayArray(ref Trafo2d[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArrayArray(ref Trafo3f[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3fArrayArrayArray(ref Trafo3f[][][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArrayArray(ref Trafo3d[][] v) { throw new NotImplementedException(); }
    public void CodeTrafo3dArrayArrayArray(ref Trafo3d[][][] v) { throw new NotImplementedException(); }

    #endregion

    #region Lists

    public void CodeList_of_V2i_(ref List<V2i> v) { CodeListOfStruct(v); }
    public void CodeList_of_V2ui_(ref List<V2ui> v) { CodeListOfStruct(v); }
    public void CodeList_of_V2l_(ref List<V2l> v) { CodeListOfStruct(v); }
    public void CodeList_of_V2f_(ref List<V2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_V2d_(ref List<V2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_V3i_(ref List<V3i> v) { CodeListOfStruct(v); }
    public void CodeList_of_V3ui_(ref List<V3ui> v) { CodeListOfStruct(v); }
    public void CodeList_of_V3l_(ref List<V3l> v) { CodeListOfStruct(v); }
    public void CodeList_of_V3f_(ref List<V3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_V3d_(ref List<V3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_V4i_(ref List<V4i> v) { CodeListOfStruct(v); }
    public void CodeList_of_V4ui_(ref List<V4ui> v) { CodeListOfStruct(v); }
    public void CodeList_of_V4l_(ref List<V4l> v) { CodeListOfStruct(v); }
    public void CodeList_of_V4f_(ref List<V4f> v) { CodeListOfStruct(v); }
    public void CodeList_of_V4d_(ref List<V4d> v) { CodeListOfStruct(v); }
    public void CodeList_of_M22i_(ref List<M22i> v) { CodeListOfStruct(v); }
    public void CodeList_of_M22l_(ref List<M22l> v) { CodeListOfStruct(v); }
    public void CodeList_of_M22f_(ref List<M22f> v) { CodeListOfStruct(v); }
    public void CodeList_of_M22d_(ref List<M22d> v) { CodeListOfStruct(v); }
    public void CodeList_of_M23i_(ref List<M23i> v) { CodeListOfStruct(v); }
    public void CodeList_of_M23l_(ref List<M23l> v) { CodeListOfStruct(v); }
    public void CodeList_of_M23f_(ref List<M23f> v) { CodeListOfStruct(v); }
    public void CodeList_of_M23d_(ref List<M23d> v) { CodeListOfStruct(v); }
    public void CodeList_of_M33i_(ref List<M33i> v) { CodeListOfStruct(v); }
    public void CodeList_of_M33l_(ref List<M33l> v) { CodeListOfStruct(v); }
    public void CodeList_of_M33f_(ref List<M33f> v) { CodeListOfStruct(v); }
    public void CodeList_of_M33d_(ref List<M33d> v) { CodeListOfStruct(v); }
    public void CodeList_of_M34i_(ref List<M34i> v) { CodeListOfStruct(v); }
    public void CodeList_of_M34l_(ref List<M34l> v) { CodeListOfStruct(v); }
    public void CodeList_of_M34f_(ref List<M34f> v) { CodeListOfStruct(v); }
    public void CodeList_of_M34d_(ref List<M34d> v) { CodeListOfStruct(v); }
    public void CodeList_of_M44i_(ref List<M44i> v) { CodeListOfStruct(v); }
    public void CodeList_of_M44l_(ref List<M44l> v) { CodeListOfStruct(v); }
    public void CodeList_of_M44f_(ref List<M44f> v) { CodeListOfStruct(v); }
    public void CodeList_of_M44d_(ref List<M44d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1b_(ref List<Range1b> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1sb_(ref List<Range1sb> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1s_(ref List<Range1s> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1us_(ref List<Range1us> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1i_(ref List<Range1i> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1ui_(ref List<Range1ui> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1l_(ref List<Range1l> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1ul_(ref List<Range1ul> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1f_(ref List<Range1f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Range1d_(ref List<Range1d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box2i_(ref List<Box2i> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box2l_(ref List<Box2l> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box2f_(ref List<Box2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box2d_(ref List<Box2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box3i_(ref List<Box3i> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box3l_(ref List<Box3l> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box3f_(ref List<Box3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Box3d_(ref List<Box3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_C3b_(ref List<C3b> v) { CodeListOfStruct(v); }
    public void CodeList_of_C3us_(ref List<C3us> v) { CodeListOfStruct(v); }
    public void CodeList_of_C3ui_(ref List<C3ui> v) { CodeListOfStruct(v); }
    public void CodeList_of_C3f_(ref List<C3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_C3d_(ref List<C3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_C4b_(ref List<C4b> v) { CodeListOfStruct(v); }
    public void CodeList_of_C4us_(ref List<C4us> v) { CodeListOfStruct(v); }
    public void CodeList_of_C4ui_(ref List<C4ui> v) { CodeListOfStruct(v); }
    public void CodeList_of_C4f_(ref List<C4f> v) { CodeListOfStruct(v); }
    public void CodeList_of_C4d_(ref List<C4d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Euclidean3f_(ref List<Euclidean3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Euclidean3d_(ref List<Euclidean3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Rot2f_(ref List<Rot2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Rot2d_(ref List<Rot2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Rot3f_(ref List<Rot3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Rot3d_(ref List<Rot3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Scale3f_(ref List<Scale3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Scale3d_(ref List<Scale3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Shift3f_(ref List<Shift3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Shift3d_(ref List<Shift3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Trafo2f_(ref List<Trafo2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Trafo2d_(ref List<Trafo2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Trafo3f_(ref List<Trafo3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Trafo3d_(ref List<Trafo3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Circle2d_(ref List<Circle2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Line2d_(ref List<Line2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Line3d_(ref List<Line3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Plane2d_(ref List<Plane2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Plane3d_(ref List<Plane3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_PlaneWithPoint3d_(ref List<PlaneWithPoint3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Quad2d_(ref List<Quad2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Quad3d_(ref List<Quad3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Ray2d_(ref List<Ray2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Ray3d_(ref List<Ray3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Sphere3d_(ref List<Sphere3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Triangle2d_(ref List<Triangle2d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Triangle3d_(ref List<Triangle3d> v) { CodeListOfStruct(v); }
    public void CodeList_of_Circle2f_(ref List<Circle2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Line2f_(ref List<Line2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Line3f_(ref List<Line3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Plane2f_(ref List<Plane2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Plane3f_(ref List<Plane3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_PlaneWithPoint3f_(ref List<PlaneWithPoint3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Quad2f_(ref List<Quad2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Quad3f_(ref List<Quad3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Ray2f_(ref List<Ray2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Ray3f_(ref List<Ray3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Sphere3f_(ref List<Sphere3f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Triangle2f_(ref List<Triangle2f> v) { CodeListOfStruct(v); }
    public void CodeList_of_Triangle3f_(ref List<Triangle3f> v) { CodeListOfStruct(v); }

    #endregion

    #region Arrays of Tensors

    public void CodeVector_of_Byte_Array(ref Vector<byte>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_SByte_Array(ref Vector<sbyte>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Short_Array(ref Vector<short>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_UShort_Array(ref Vector<ushort>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Int_Array(ref Vector<int>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_UInt_Array(ref Vector<uint>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Long_Array(ref Vector<long>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_ULong_Array(ref Vector<ulong>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Float_Array(ref Vector<float>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Double_Array(ref Vector<double>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Fraction_Array(ref Vector<Fraction>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V2i_Array(ref Vector<V2i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V2l_Array(ref Vector<V2l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V2f_Array(ref Vector<V2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V2d_Array(ref Vector<V2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V3i_Array(ref Vector<V3i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V3l_Array(ref Vector<V3l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V3f_Array(ref Vector<V3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V3d_Array(ref Vector<V3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V4i_Array(ref Vector<V4i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V4l_Array(ref Vector<V4l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V4f_Array(ref Vector<V4f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_V4d_Array(ref Vector<V4d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M22i_Array(ref Vector<M22i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M22l_Array(ref Vector<M22l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M22f_Array(ref Vector<M22f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M22d_Array(ref Vector<M22d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M23i_Array(ref Vector<M23i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M23l_Array(ref Vector<M23l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M23f_Array(ref Vector<M23f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M23d_Array(ref Vector<M23d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M33i_Array(ref Vector<M33i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M33l_Array(ref Vector<M33l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M33f_Array(ref Vector<M33f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M33d_Array(ref Vector<M33d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M34i_Array(ref Vector<M34i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M34l_Array(ref Vector<M34l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M34f_Array(ref Vector<M34f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M34d_Array(ref Vector<M34d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M44i_Array(ref Vector<M44i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M44l_Array(ref Vector<M44l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M44f_Array(ref Vector<M44f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_M44d_Array(ref Vector<M44d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C3b_Array(ref Vector<C3b>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C3us_Array(ref Vector<C3us>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C3ui_Array(ref Vector<C3ui>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C3f_Array(ref Vector<C3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C3d_Array(ref Vector<C3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C4b_Array(ref Vector<C4b>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C4us_Array(ref Vector<C4us>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C4ui_Array(ref Vector<C4ui>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C4f_Array(ref Vector<C4f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_C4d_Array(ref Vector<C4d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1b_Array(ref Vector<Range1b>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1sb_Array(ref Vector<Range1sb>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1s_Array(ref Vector<Range1s>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1us_Array(ref Vector<Range1us>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1i_Array(ref Vector<Range1i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1ui_Array(ref Vector<Range1ui>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1l_Array(ref Vector<Range1l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1ul_Array(ref Vector<Range1ul>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1f_Array(ref Vector<Range1f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Range1d_Array(ref Vector<Range1d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box2i_Array(ref Vector<Box2i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box2l_Array(ref Vector<Box2l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box2f_Array(ref Vector<Box2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box2d_Array(ref Vector<Box2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box3i_Array(ref Vector<Box3i>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box3l_Array(ref Vector<Box3l>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box3f_Array(ref Vector<Box3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Box3d_Array(ref Vector<Box3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Euclidean3f_Array(ref Vector<Euclidean3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Euclidean3d_Array(ref Vector<Euclidean3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Rot2f_Array(ref Vector<Rot2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Rot2d_Array(ref Vector<Rot2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Rot3f_Array(ref Vector<Rot3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Rot3d_Array(ref Vector<Rot3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Scale3f_Array(ref Vector<Scale3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Scale3d_Array(ref Vector<Scale3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Shift3f_Array(ref Vector<Shift3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Shift3d_Array(ref Vector<Shift3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Trafo2f_Array(ref Vector<Trafo2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Trafo2d_Array(ref Vector<Trafo2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Trafo3f_Array(ref Vector<Trafo3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Trafo3d_Array(ref Vector<Trafo3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Bool_Array(ref Vector<bool>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Char_Array(ref Vector<char>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_String_Array(ref Vector<string>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Type_Array(ref Vector<Type>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Guid_Array(ref Vector<Guid>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Symbol_Array(ref Vector<Symbol>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Circle2d_Array(ref Vector<Circle2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Line2d_Array(ref Vector<Line2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Line3d_Array(ref Vector<Line3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Plane2d_Array(ref Vector<Plane2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Plane3d_Array(ref Vector<Plane3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_PlaneWithPoint3d_Array(ref Vector<PlaneWithPoint3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Quad2d_Array(ref Vector<Quad2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Quad3d_Array(ref Vector<Quad3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Ray2d_Array(ref Vector<Ray2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Ray3d_Array(ref Vector<Ray3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Sphere3d_Array(ref Vector<Sphere3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Triangle2d_Array(ref Vector<Triangle2d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Triangle3d_Array(ref Vector<Triangle3d>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Circle2f_Array(ref Vector<Circle2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Line2f_Array(ref Vector<Line2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Line3f_Array(ref Vector<Line3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Plane2f_Array(ref Vector<Plane2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Plane3f_Array(ref Vector<Plane3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_PlaneWithPoint3f_Array(ref Vector<PlaneWithPoint3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Quad2f_Array(ref Vector<Quad2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Quad3f_Array(ref Vector<Quad3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Ray2f_Array(ref Vector<Ray2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Ray3f_Array(ref Vector<Ray3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Sphere3f_Array(ref Vector<Sphere3f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Triangle2f_Array(ref Vector<Triangle2f>[] v) { CodeArrayOf(v); }
    public void CodeVector_of_Triangle3f_Array(ref Vector<Triangle3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Byte_Array(ref Matrix<byte>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_SByte_Array(ref Matrix<sbyte>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Short_Array(ref Matrix<short>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_UShort_Array(ref Matrix<ushort>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Int_Array(ref Matrix<int>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_UInt_Array(ref Matrix<uint>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Long_Array(ref Matrix<long>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_ULong_Array(ref Matrix<ulong>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Float_Array(ref Matrix<float>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Double_Array(ref Matrix<double>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Fraction_Array(ref Matrix<Fraction>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V2i_Array(ref Matrix<V2i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V2l_Array(ref Matrix<V2l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V2f_Array(ref Matrix<V2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V2d_Array(ref Matrix<V2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V3i_Array(ref Matrix<V3i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V3l_Array(ref Matrix<V3l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V3f_Array(ref Matrix<V3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V3d_Array(ref Matrix<V3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V4i_Array(ref Matrix<V4i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V4l_Array(ref Matrix<V4l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V4f_Array(ref Matrix<V4f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_V4d_Array(ref Matrix<V4d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M22i_Array(ref Matrix<M22i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M22l_Array(ref Matrix<M22l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M22f_Array(ref Matrix<M22f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M22d_Array(ref Matrix<M22d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M23i_Array(ref Matrix<M23i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M23l_Array(ref Matrix<M23l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M23f_Array(ref Matrix<M23f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M23d_Array(ref Matrix<M23d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M33i_Array(ref Matrix<M33i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M33l_Array(ref Matrix<M33l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M33f_Array(ref Matrix<M33f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M33d_Array(ref Matrix<M33d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M34i_Array(ref Matrix<M34i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M34l_Array(ref Matrix<M34l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M34f_Array(ref Matrix<M34f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M34d_Array(ref Matrix<M34d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M44i_Array(ref Matrix<M44i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M44l_Array(ref Matrix<M44l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M44f_Array(ref Matrix<M44f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_M44d_Array(ref Matrix<M44d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C3b_Array(ref Matrix<C3b>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C3us_Array(ref Matrix<C3us>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C3ui_Array(ref Matrix<C3ui>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C3f_Array(ref Matrix<C3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C3d_Array(ref Matrix<C3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C4b_Array(ref Matrix<C4b>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C4us_Array(ref Matrix<C4us>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C4ui_Array(ref Matrix<C4ui>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C4f_Array(ref Matrix<C4f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_C4d_Array(ref Matrix<C4d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1b_Array(ref Matrix<Range1b>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1sb_Array(ref Matrix<Range1sb>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1s_Array(ref Matrix<Range1s>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1us_Array(ref Matrix<Range1us>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1i_Array(ref Matrix<Range1i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1ui_Array(ref Matrix<Range1ui>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1l_Array(ref Matrix<Range1l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1ul_Array(ref Matrix<Range1ul>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1f_Array(ref Matrix<Range1f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Range1d_Array(ref Matrix<Range1d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box2i_Array(ref Matrix<Box2i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box2l_Array(ref Matrix<Box2l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box2f_Array(ref Matrix<Box2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box2d_Array(ref Matrix<Box2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box3i_Array(ref Matrix<Box3i>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box3l_Array(ref Matrix<Box3l>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box3f_Array(ref Matrix<Box3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Box3d_Array(ref Matrix<Box3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Euclidean3f_Array(ref Matrix<Euclidean3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Euclidean3d_Array(ref Matrix<Euclidean3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Rot2f_Array(ref Matrix<Rot2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Rot2d_Array(ref Matrix<Rot2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Rot3f_Array(ref Matrix<Rot3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Rot3d_Array(ref Matrix<Rot3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Scale3f_Array(ref Matrix<Scale3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Scale3d_Array(ref Matrix<Scale3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Shift3f_Array(ref Matrix<Shift3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Shift3d_Array(ref Matrix<Shift3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Trafo2f_Array(ref Matrix<Trafo2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Trafo2d_Array(ref Matrix<Trafo2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Trafo3f_Array(ref Matrix<Trafo3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Trafo3d_Array(ref Matrix<Trafo3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Bool_Array(ref Matrix<bool>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Char_Array(ref Matrix<char>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_String_Array(ref Matrix<string>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Type_Array(ref Matrix<Type>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Guid_Array(ref Matrix<Guid>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Symbol_Array(ref Matrix<Symbol>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Circle2d_Array(ref Matrix<Circle2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Line2d_Array(ref Matrix<Line2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Line3d_Array(ref Matrix<Line3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Plane2d_Array(ref Matrix<Plane2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Plane3d_Array(ref Matrix<Plane3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_PlaneWithPoint3d_Array(ref Matrix<PlaneWithPoint3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Quad2d_Array(ref Matrix<Quad2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Quad3d_Array(ref Matrix<Quad3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Ray2d_Array(ref Matrix<Ray2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Ray3d_Array(ref Matrix<Ray3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Sphere3d_Array(ref Matrix<Sphere3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Triangle2d_Array(ref Matrix<Triangle2d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Triangle3d_Array(ref Matrix<Triangle3d>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Circle2f_Array(ref Matrix<Circle2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Line2f_Array(ref Matrix<Line2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Line3f_Array(ref Matrix<Line3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Plane2f_Array(ref Matrix<Plane2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Plane3f_Array(ref Matrix<Plane3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_PlaneWithPoint3f_Array(ref Matrix<PlaneWithPoint3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Quad2f_Array(ref Matrix<Quad2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Quad3f_Array(ref Matrix<Quad3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Ray2f_Array(ref Matrix<Ray2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Ray3f_Array(ref Matrix<Ray3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Sphere3f_Array(ref Matrix<Sphere3f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Triangle2f_Array(ref Matrix<Triangle2f>[] v) { CodeArrayOf(v); }
    public void CodeMatrix_of_Triangle3f_Array(ref Matrix<Triangle3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Byte_Array(ref Volume<byte>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_SByte_Array(ref Volume<sbyte>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Short_Array(ref Volume<short>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_UShort_Array(ref Volume<ushort>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Int_Array(ref Volume<int>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_UInt_Array(ref Volume<uint>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Long_Array(ref Volume<long>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_ULong_Array(ref Volume<ulong>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Float_Array(ref Volume<float>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Double_Array(ref Volume<double>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Fraction_Array(ref Volume<Fraction>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V2i_Array(ref Volume<V2i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V2l_Array(ref Volume<V2l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V2f_Array(ref Volume<V2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V2d_Array(ref Volume<V2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V3i_Array(ref Volume<V3i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V3l_Array(ref Volume<V3l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V3f_Array(ref Volume<V3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V3d_Array(ref Volume<V3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V4i_Array(ref Volume<V4i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V4l_Array(ref Volume<V4l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V4f_Array(ref Volume<V4f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_V4d_Array(ref Volume<V4d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M22i_Array(ref Volume<M22i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M22l_Array(ref Volume<M22l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M22f_Array(ref Volume<M22f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M22d_Array(ref Volume<M22d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M23i_Array(ref Volume<M23i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M23l_Array(ref Volume<M23l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M23f_Array(ref Volume<M23f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M23d_Array(ref Volume<M23d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M33i_Array(ref Volume<M33i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M33l_Array(ref Volume<M33l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M33f_Array(ref Volume<M33f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M33d_Array(ref Volume<M33d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M34i_Array(ref Volume<M34i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M34l_Array(ref Volume<M34l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M34f_Array(ref Volume<M34f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M34d_Array(ref Volume<M34d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M44i_Array(ref Volume<M44i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M44l_Array(ref Volume<M44l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M44f_Array(ref Volume<M44f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_M44d_Array(ref Volume<M44d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C3b_Array(ref Volume<C3b>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C3us_Array(ref Volume<C3us>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C3ui_Array(ref Volume<C3ui>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C3f_Array(ref Volume<C3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C3d_Array(ref Volume<C3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C4b_Array(ref Volume<C4b>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C4us_Array(ref Volume<C4us>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C4ui_Array(ref Volume<C4ui>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C4f_Array(ref Volume<C4f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_C4d_Array(ref Volume<C4d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1b_Array(ref Volume<Range1b>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1sb_Array(ref Volume<Range1sb>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1s_Array(ref Volume<Range1s>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1us_Array(ref Volume<Range1us>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1i_Array(ref Volume<Range1i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1ui_Array(ref Volume<Range1ui>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1l_Array(ref Volume<Range1l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1ul_Array(ref Volume<Range1ul>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1f_Array(ref Volume<Range1f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Range1d_Array(ref Volume<Range1d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box2i_Array(ref Volume<Box2i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box2l_Array(ref Volume<Box2l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box2f_Array(ref Volume<Box2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box2d_Array(ref Volume<Box2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box3i_Array(ref Volume<Box3i>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box3l_Array(ref Volume<Box3l>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box3f_Array(ref Volume<Box3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Box3d_Array(ref Volume<Box3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Euclidean3f_Array(ref Volume<Euclidean3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Euclidean3d_Array(ref Volume<Euclidean3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Rot2f_Array(ref Volume<Rot2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Rot2d_Array(ref Volume<Rot2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Rot3f_Array(ref Volume<Rot3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Rot3d_Array(ref Volume<Rot3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Scale3f_Array(ref Volume<Scale3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Scale3d_Array(ref Volume<Scale3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Shift3f_Array(ref Volume<Shift3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Shift3d_Array(ref Volume<Shift3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Trafo2f_Array(ref Volume<Trafo2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Trafo2d_Array(ref Volume<Trafo2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Trafo3f_Array(ref Volume<Trafo3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Trafo3d_Array(ref Volume<Trafo3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Bool_Array(ref Volume<bool>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Char_Array(ref Volume<char>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_String_Array(ref Volume<string>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Type_Array(ref Volume<Type>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Guid_Array(ref Volume<Guid>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Symbol_Array(ref Volume<Symbol>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Circle2d_Array(ref Volume<Circle2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Line2d_Array(ref Volume<Line2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Line3d_Array(ref Volume<Line3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Plane2d_Array(ref Volume<Plane2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Plane3d_Array(ref Volume<Plane3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_PlaneWithPoint3d_Array(ref Volume<PlaneWithPoint3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Quad2d_Array(ref Volume<Quad2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Quad3d_Array(ref Volume<Quad3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Ray2d_Array(ref Volume<Ray2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Ray3d_Array(ref Volume<Ray3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Sphere3d_Array(ref Volume<Sphere3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Triangle2d_Array(ref Volume<Triangle2d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Triangle3d_Array(ref Volume<Triangle3d>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Circle2f_Array(ref Volume<Circle2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Line2f_Array(ref Volume<Line2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Line3f_Array(ref Volume<Line3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Plane2f_Array(ref Volume<Plane2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Plane3f_Array(ref Volume<Plane3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_PlaneWithPoint3f_Array(ref Volume<PlaneWithPoint3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Quad2f_Array(ref Volume<Quad2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Quad3f_Array(ref Volume<Quad3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Ray2f_Array(ref Volume<Ray2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Ray3f_Array(ref Volume<Ray3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Sphere3f_Array(ref Volume<Sphere3f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Triangle2f_Array(ref Volume<Triangle2f>[] v) { CodeArrayOf(v); }
    public void CodeVolume_of_Triangle3f_Array(ref Volume<Triangle3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Byte_Array(ref Tensor<byte>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_SByte_Array(ref Tensor<sbyte>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Short_Array(ref Tensor<short>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_UShort_Array(ref Tensor<ushort>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Int_Array(ref Tensor<int>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_UInt_Array(ref Tensor<uint>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Long_Array(ref Tensor<long>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_ULong_Array(ref Tensor<ulong>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Float_Array(ref Tensor<float>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Double_Array(ref Tensor<double>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Fraction_Array(ref Tensor<Fraction>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V2i_Array(ref Tensor<V2i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V2l_Array(ref Tensor<V2l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V2f_Array(ref Tensor<V2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V2d_Array(ref Tensor<V2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V3i_Array(ref Tensor<V3i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V3l_Array(ref Tensor<V3l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V3f_Array(ref Tensor<V3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V3d_Array(ref Tensor<V3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V4i_Array(ref Tensor<V4i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V4l_Array(ref Tensor<V4l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V4f_Array(ref Tensor<V4f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_V4d_Array(ref Tensor<V4d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M22i_Array(ref Tensor<M22i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M22l_Array(ref Tensor<M22l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M22f_Array(ref Tensor<M22f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M22d_Array(ref Tensor<M22d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M23i_Array(ref Tensor<M23i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M23l_Array(ref Tensor<M23l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M23f_Array(ref Tensor<M23f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M23d_Array(ref Tensor<M23d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M33i_Array(ref Tensor<M33i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M33l_Array(ref Tensor<M33l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M33f_Array(ref Tensor<M33f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M33d_Array(ref Tensor<M33d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M34i_Array(ref Tensor<M34i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M34l_Array(ref Tensor<M34l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M34f_Array(ref Tensor<M34f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M34d_Array(ref Tensor<M34d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M44i_Array(ref Tensor<M44i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M44l_Array(ref Tensor<M44l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M44f_Array(ref Tensor<M44f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_M44d_Array(ref Tensor<M44d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C3b_Array(ref Tensor<C3b>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C3us_Array(ref Tensor<C3us>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C3ui_Array(ref Tensor<C3ui>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C3f_Array(ref Tensor<C3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C3d_Array(ref Tensor<C3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C4b_Array(ref Tensor<C4b>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C4us_Array(ref Tensor<C4us>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C4ui_Array(ref Tensor<C4ui>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C4f_Array(ref Tensor<C4f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_C4d_Array(ref Tensor<C4d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1b_Array(ref Tensor<Range1b>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1sb_Array(ref Tensor<Range1sb>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1s_Array(ref Tensor<Range1s>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1us_Array(ref Tensor<Range1us>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1i_Array(ref Tensor<Range1i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1ui_Array(ref Tensor<Range1ui>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1l_Array(ref Tensor<Range1l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1ul_Array(ref Tensor<Range1ul>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1f_Array(ref Tensor<Range1f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Range1d_Array(ref Tensor<Range1d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box2i_Array(ref Tensor<Box2i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box2l_Array(ref Tensor<Box2l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box2f_Array(ref Tensor<Box2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box2d_Array(ref Tensor<Box2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box3i_Array(ref Tensor<Box3i>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box3l_Array(ref Tensor<Box3l>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box3f_Array(ref Tensor<Box3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Box3d_Array(ref Tensor<Box3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Euclidean3f_Array(ref Tensor<Euclidean3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Euclidean3d_Array(ref Tensor<Euclidean3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Rot2f_Array(ref Tensor<Rot2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Rot2d_Array(ref Tensor<Rot2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Rot3f_Array(ref Tensor<Rot3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Rot3d_Array(ref Tensor<Rot3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Scale3f_Array(ref Tensor<Scale3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Scale3d_Array(ref Tensor<Scale3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Shift3f_Array(ref Tensor<Shift3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Shift3d_Array(ref Tensor<Shift3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Trafo2f_Array(ref Tensor<Trafo2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Trafo2d_Array(ref Tensor<Trafo2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Trafo3f_Array(ref Tensor<Trafo3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Trafo3d_Array(ref Tensor<Trafo3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Bool_Array(ref Tensor<bool>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Char_Array(ref Tensor<char>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_String_Array(ref Tensor<string>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Type_Array(ref Tensor<Type>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Guid_Array(ref Tensor<Guid>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Symbol_Array(ref Tensor<Symbol>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Circle2d_Array(ref Tensor<Circle2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Line2d_Array(ref Tensor<Line2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Line3d_Array(ref Tensor<Line3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Plane2d_Array(ref Tensor<Plane2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Plane3d_Array(ref Tensor<Plane3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_PlaneWithPoint3d_Array(ref Tensor<PlaneWithPoint3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Quad2d_Array(ref Tensor<Quad2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Quad3d_Array(ref Tensor<Quad3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Ray2d_Array(ref Tensor<Ray2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Ray3d_Array(ref Tensor<Ray3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Sphere3d_Array(ref Tensor<Sphere3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Triangle2d_Array(ref Tensor<Triangle2d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Triangle3d_Array(ref Tensor<Triangle3d>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Circle2f_Array(ref Tensor<Circle2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Line2f_Array(ref Tensor<Line2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Line3f_Array(ref Tensor<Line3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Plane2f_Array(ref Tensor<Plane2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Plane3f_Array(ref Tensor<Plane3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_PlaneWithPoint3f_Array(ref Tensor<PlaneWithPoint3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Quad2f_Array(ref Tensor<Quad2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Quad3f_Array(ref Tensor<Quad3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Ray2f_Array(ref Tensor<Ray2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Ray3f_Array(ref Tensor<Ray3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Sphere3f_Array(ref Tensor<Sphere3f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Triangle2f_Array(ref Tensor<Triangle2f>[] v) { CodeArrayOf(v); }
    public void CodeTensor_of_Triangle3f_Array(ref Tensor<Triangle3f>[] v) { CodeArrayOf(v); }

    #endregion

    #region Lists of Tensors

    public void CodeList_of_Vector_of_Byte__(ref List<Vector<byte>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_SByte__(ref List<Vector<sbyte>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Short__(ref List<Vector<short>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_UShort__(ref List<Vector<ushort>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Int__(ref List<Vector<int>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_UInt__(ref List<Vector<uint>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Long__(ref List<Vector<long>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_ULong__(ref List<Vector<ulong>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Float__(ref List<Vector<float>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Double__(ref List<Vector<double>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Fraction__(ref List<Vector<Fraction>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V2i__(ref List<Vector<V2i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V2l__(ref List<Vector<V2l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V2f__(ref List<Vector<V2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V2d__(ref List<Vector<V2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V3i__(ref List<Vector<V3i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V3l__(ref List<Vector<V3l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V3f__(ref List<Vector<V3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V3d__(ref List<Vector<V3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V4i__(ref List<Vector<V4i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V4l__(ref List<Vector<V4l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V4f__(ref List<Vector<V4f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_V4d__(ref List<Vector<V4d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M22i__(ref List<Vector<M22i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M22l__(ref List<Vector<M22l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M22f__(ref List<Vector<M22f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M22d__(ref List<Vector<M22d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M23i__(ref List<Vector<M23i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M23l__(ref List<Vector<M23l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M23f__(ref List<Vector<M23f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M23d__(ref List<Vector<M23d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M33i__(ref List<Vector<M33i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M33l__(ref List<Vector<M33l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M33f__(ref List<Vector<M33f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M33d__(ref List<Vector<M33d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M34i__(ref List<Vector<M34i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M34l__(ref List<Vector<M34l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M34f__(ref List<Vector<M34f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M34d__(ref List<Vector<M34d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M44i__(ref List<Vector<M44i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M44l__(ref List<Vector<M44l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M44f__(ref List<Vector<M44f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_M44d__(ref List<Vector<M44d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C3b__(ref List<Vector<C3b>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C3us__(ref List<Vector<C3us>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C3ui__(ref List<Vector<C3ui>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C3f__(ref List<Vector<C3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C3d__(ref List<Vector<C3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C4b__(ref List<Vector<C4b>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C4us__(ref List<Vector<C4us>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C4ui__(ref List<Vector<C4ui>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C4f__(ref List<Vector<C4f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_C4d__(ref List<Vector<C4d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1b__(ref List<Vector<Range1b>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1sb__(ref List<Vector<Range1sb>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1s__(ref List<Vector<Range1s>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1us__(ref List<Vector<Range1us>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1i__(ref List<Vector<Range1i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1ui__(ref List<Vector<Range1ui>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1l__(ref List<Vector<Range1l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1ul__(ref List<Vector<Range1ul>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1f__(ref List<Vector<Range1f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Range1d__(ref List<Vector<Range1d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box2i__(ref List<Vector<Box2i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box2l__(ref List<Vector<Box2l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box2f__(ref List<Vector<Box2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box2d__(ref List<Vector<Box2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box3i__(ref List<Vector<Box3i>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box3l__(ref List<Vector<Box3l>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box3f__(ref List<Vector<Box3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Box3d__(ref List<Vector<Box3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Euclidean3f__(ref List<Vector<Euclidean3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Euclidean3d__(ref List<Vector<Euclidean3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Rot2f__(ref List<Vector<Rot2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Rot2d__(ref List<Vector<Rot2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Rot3f__(ref List<Vector<Rot3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Rot3d__(ref List<Vector<Rot3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Scale3f__(ref List<Vector<Scale3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Scale3d__(ref List<Vector<Scale3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Shift3f__(ref List<Vector<Shift3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Shift3d__(ref List<Vector<Shift3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Trafo2f__(ref List<Vector<Trafo2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Trafo2d__(ref List<Vector<Trafo2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Trafo3f__(ref List<Vector<Trafo3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Trafo3d__(ref List<Vector<Trafo3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Bool__(ref List<Vector<bool>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Char__(ref List<Vector<char>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_String__(ref List<Vector<string>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Type__(ref List<Vector<Type>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Guid__(ref List<Vector<Guid>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Symbol__(ref List<Vector<Symbol>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Circle2d__(ref List<Vector<Circle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Line2d__(ref List<Vector<Line2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Line3d__(ref List<Vector<Line3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Plane2d__(ref List<Vector<Plane2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Plane3d__(ref List<Vector<Plane3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_PlaneWithPoint3d__(ref List<Vector<PlaneWithPoint3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Quad2d__(ref List<Vector<Quad2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Quad3d__(ref List<Vector<Quad3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Ray2d__(ref List<Vector<Ray2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Ray3d__(ref List<Vector<Ray3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Sphere3d__(ref List<Vector<Sphere3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Triangle2d__(ref List<Vector<Triangle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Triangle3d__(ref List<Vector<Triangle3d>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Circle2f__(ref List<Vector<Circle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Line2f__(ref List<Vector<Line2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Line3f__(ref List<Vector<Line3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Plane2f__(ref List<Vector<Plane2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Plane3f__(ref List<Vector<Plane3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_PlaneWithPoint3f__(ref List<Vector<PlaneWithPoint3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Quad2f__(ref List<Vector<Quad2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Quad3f__(ref List<Vector<Quad3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Ray2f__(ref List<Vector<Ray2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Ray3f__(ref List<Vector<Ray3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Sphere3f__(ref List<Vector<Sphere3f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Triangle2f__(ref List<Vector<Triangle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Vector_of_Triangle3f__(ref List<Vector<Triangle3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Byte__(ref List<Matrix<byte>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_SByte__(ref List<Matrix<sbyte>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Short__(ref List<Matrix<short>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_UShort__(ref List<Matrix<ushort>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Int__(ref List<Matrix<int>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_UInt__(ref List<Matrix<uint>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Long__(ref List<Matrix<long>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_ULong__(ref List<Matrix<ulong>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Float__(ref List<Matrix<float>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Double__(ref List<Matrix<double>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Fraction__(ref List<Matrix<Fraction>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V2i__(ref List<Matrix<V2i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V2l__(ref List<Matrix<V2l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V2f__(ref List<Matrix<V2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V2d__(ref List<Matrix<V2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V3i__(ref List<Matrix<V3i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V3l__(ref List<Matrix<V3l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V3f__(ref List<Matrix<V3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V3d__(ref List<Matrix<V3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V4i__(ref List<Matrix<V4i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V4l__(ref List<Matrix<V4l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V4f__(ref List<Matrix<V4f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_V4d__(ref List<Matrix<V4d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M22i__(ref List<Matrix<M22i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M22l__(ref List<Matrix<M22l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M22f__(ref List<Matrix<M22f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M22d__(ref List<Matrix<M22d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M23i__(ref List<Matrix<M23i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M23l__(ref List<Matrix<M23l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M23f__(ref List<Matrix<M23f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M23d__(ref List<Matrix<M23d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M33i__(ref List<Matrix<M33i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M33l__(ref List<Matrix<M33l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M33f__(ref List<Matrix<M33f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M33d__(ref List<Matrix<M33d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M34i__(ref List<Matrix<M34i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M34l__(ref List<Matrix<M34l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M34f__(ref List<Matrix<M34f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M34d__(ref List<Matrix<M34d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M44i__(ref List<Matrix<M44i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M44l__(ref List<Matrix<M44l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M44f__(ref List<Matrix<M44f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_M44d__(ref List<Matrix<M44d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C3b__(ref List<Matrix<C3b>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C3us__(ref List<Matrix<C3us>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C3ui__(ref List<Matrix<C3ui>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C3f__(ref List<Matrix<C3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C3d__(ref List<Matrix<C3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C4b__(ref List<Matrix<C4b>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C4us__(ref List<Matrix<C4us>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C4ui__(ref List<Matrix<C4ui>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C4f__(ref List<Matrix<C4f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_C4d__(ref List<Matrix<C4d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1b__(ref List<Matrix<Range1b>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1sb__(ref List<Matrix<Range1sb>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1s__(ref List<Matrix<Range1s>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1us__(ref List<Matrix<Range1us>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1i__(ref List<Matrix<Range1i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1ui__(ref List<Matrix<Range1ui>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1l__(ref List<Matrix<Range1l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1ul__(ref List<Matrix<Range1ul>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1f__(ref List<Matrix<Range1f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Range1d__(ref List<Matrix<Range1d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box2i__(ref List<Matrix<Box2i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box2l__(ref List<Matrix<Box2l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box2f__(ref List<Matrix<Box2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box2d__(ref List<Matrix<Box2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box3i__(ref List<Matrix<Box3i>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box3l__(ref List<Matrix<Box3l>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box3f__(ref List<Matrix<Box3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Box3d__(ref List<Matrix<Box3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Euclidean3f__(ref List<Matrix<Euclidean3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Euclidean3d__(ref List<Matrix<Euclidean3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Rot2f__(ref List<Matrix<Rot2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Rot2d__(ref List<Matrix<Rot2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Rot3f__(ref List<Matrix<Rot3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Rot3d__(ref List<Matrix<Rot3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Scale3f__(ref List<Matrix<Scale3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Scale3d__(ref List<Matrix<Scale3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Shift3f__(ref List<Matrix<Shift3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Shift3d__(ref List<Matrix<Shift3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Trafo2f__(ref List<Matrix<Trafo2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Trafo2d__(ref List<Matrix<Trafo2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Trafo3f__(ref List<Matrix<Trafo3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Trafo3d__(ref List<Matrix<Trafo3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Bool__(ref List<Matrix<bool>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Char__(ref List<Matrix<char>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_String__(ref List<Matrix<string>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Type__(ref List<Matrix<Type>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Guid__(ref List<Matrix<Guid>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Symbol__(ref List<Matrix<Symbol>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Circle2d__(ref List<Matrix<Circle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Line2d__(ref List<Matrix<Line2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Line3d__(ref List<Matrix<Line3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Plane2d__(ref List<Matrix<Plane2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Plane3d__(ref List<Matrix<Plane3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_PlaneWithPoint3d__(ref List<Matrix<PlaneWithPoint3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Quad2d__(ref List<Matrix<Quad2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Quad3d__(ref List<Matrix<Quad3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Ray2d__(ref List<Matrix<Ray2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Ray3d__(ref List<Matrix<Ray3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Sphere3d__(ref List<Matrix<Sphere3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Triangle2d__(ref List<Matrix<Triangle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Triangle3d__(ref List<Matrix<Triangle3d>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Circle2f__(ref List<Matrix<Circle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Line2f__(ref List<Matrix<Line2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Line3f__(ref List<Matrix<Line3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Plane2f__(ref List<Matrix<Plane2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Plane3f__(ref List<Matrix<Plane3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_PlaneWithPoint3f__(ref List<Matrix<PlaneWithPoint3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Quad2f__(ref List<Matrix<Quad2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Quad3f__(ref List<Matrix<Quad3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Ray2f__(ref List<Matrix<Ray2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Ray3f__(ref List<Matrix<Ray3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Sphere3f__(ref List<Matrix<Sphere3f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Triangle2f__(ref List<Matrix<Triangle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Matrix_of_Triangle3f__(ref List<Matrix<Triangle3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Byte__(ref List<Volume<byte>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_SByte__(ref List<Volume<sbyte>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Short__(ref List<Volume<short>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_UShort__(ref List<Volume<ushort>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Int__(ref List<Volume<int>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_UInt__(ref List<Volume<uint>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Long__(ref List<Volume<long>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_ULong__(ref List<Volume<ulong>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Float__(ref List<Volume<float>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Double__(ref List<Volume<double>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Fraction__(ref List<Volume<Fraction>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V2i__(ref List<Volume<V2i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V2l__(ref List<Volume<V2l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V2f__(ref List<Volume<V2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V2d__(ref List<Volume<V2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V3i__(ref List<Volume<V3i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V3l__(ref List<Volume<V3l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V3f__(ref List<Volume<V3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V3d__(ref List<Volume<V3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V4i__(ref List<Volume<V4i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V4l__(ref List<Volume<V4l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V4f__(ref List<Volume<V4f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_V4d__(ref List<Volume<V4d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M22i__(ref List<Volume<M22i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M22l__(ref List<Volume<M22l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M22f__(ref List<Volume<M22f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M22d__(ref List<Volume<M22d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M23i__(ref List<Volume<M23i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M23l__(ref List<Volume<M23l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M23f__(ref List<Volume<M23f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M23d__(ref List<Volume<M23d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M33i__(ref List<Volume<M33i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M33l__(ref List<Volume<M33l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M33f__(ref List<Volume<M33f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M33d__(ref List<Volume<M33d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M34i__(ref List<Volume<M34i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M34l__(ref List<Volume<M34l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M34f__(ref List<Volume<M34f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M34d__(ref List<Volume<M34d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M44i__(ref List<Volume<M44i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M44l__(ref List<Volume<M44l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M44f__(ref List<Volume<M44f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_M44d__(ref List<Volume<M44d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C3b__(ref List<Volume<C3b>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C3us__(ref List<Volume<C3us>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C3ui__(ref List<Volume<C3ui>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C3f__(ref List<Volume<C3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C3d__(ref List<Volume<C3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C4b__(ref List<Volume<C4b>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C4us__(ref List<Volume<C4us>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C4ui__(ref List<Volume<C4ui>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C4f__(ref List<Volume<C4f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_C4d__(ref List<Volume<C4d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1b__(ref List<Volume<Range1b>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1sb__(ref List<Volume<Range1sb>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1s__(ref List<Volume<Range1s>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1us__(ref List<Volume<Range1us>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1i__(ref List<Volume<Range1i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1ui__(ref List<Volume<Range1ui>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1l__(ref List<Volume<Range1l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1ul__(ref List<Volume<Range1ul>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1f__(ref List<Volume<Range1f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Range1d__(ref List<Volume<Range1d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box2i__(ref List<Volume<Box2i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box2l__(ref List<Volume<Box2l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box2f__(ref List<Volume<Box2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box2d__(ref List<Volume<Box2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box3i__(ref List<Volume<Box3i>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box3l__(ref List<Volume<Box3l>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box3f__(ref List<Volume<Box3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Box3d__(ref List<Volume<Box3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Euclidean3f__(ref List<Volume<Euclidean3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Euclidean3d__(ref List<Volume<Euclidean3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Rot2f__(ref List<Volume<Rot2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Rot2d__(ref List<Volume<Rot2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Rot3f__(ref List<Volume<Rot3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Rot3d__(ref List<Volume<Rot3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Scale3f__(ref List<Volume<Scale3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Scale3d__(ref List<Volume<Scale3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Shift3f__(ref List<Volume<Shift3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Shift3d__(ref List<Volume<Shift3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Trafo2f__(ref List<Volume<Trafo2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Trafo2d__(ref List<Volume<Trafo2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Trafo3f__(ref List<Volume<Trafo3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Trafo3d__(ref List<Volume<Trafo3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Bool__(ref List<Volume<bool>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Char__(ref List<Volume<char>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_String__(ref List<Volume<string>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Type__(ref List<Volume<Type>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Guid__(ref List<Volume<Guid>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Symbol__(ref List<Volume<Symbol>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Circle2d__(ref List<Volume<Circle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Line2d__(ref List<Volume<Line2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Line3d__(ref List<Volume<Line3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Plane2d__(ref List<Volume<Plane2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Plane3d__(ref List<Volume<Plane3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_PlaneWithPoint3d__(ref List<Volume<PlaneWithPoint3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Quad2d__(ref List<Volume<Quad2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Quad3d__(ref List<Volume<Quad3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Ray2d__(ref List<Volume<Ray2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Ray3d__(ref List<Volume<Ray3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Sphere3d__(ref List<Volume<Sphere3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Triangle2d__(ref List<Volume<Triangle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Triangle3d__(ref List<Volume<Triangle3d>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Circle2f__(ref List<Volume<Circle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Line2f__(ref List<Volume<Line2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Line3f__(ref List<Volume<Line3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Plane2f__(ref List<Volume<Plane2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Plane3f__(ref List<Volume<Plane3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_PlaneWithPoint3f__(ref List<Volume<PlaneWithPoint3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Quad2f__(ref List<Volume<Quad2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Quad3f__(ref List<Volume<Quad3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Ray2f__(ref List<Volume<Ray2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Ray3f__(ref List<Volume<Ray3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Sphere3f__(ref List<Volume<Sphere3f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Triangle2f__(ref List<Volume<Triangle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Volume_of_Triangle3f__(ref List<Volume<Triangle3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Byte__(ref List<Tensor<byte>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_SByte__(ref List<Tensor<sbyte>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Short__(ref List<Tensor<short>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_UShort__(ref List<Tensor<ushort>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Int__(ref List<Tensor<int>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_UInt__(ref List<Tensor<uint>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Long__(ref List<Tensor<long>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_ULong__(ref List<Tensor<ulong>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Float__(ref List<Tensor<float>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Double__(ref List<Tensor<double>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Fraction__(ref List<Tensor<Fraction>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V2i__(ref List<Tensor<V2i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V2l__(ref List<Tensor<V2l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V2f__(ref List<Tensor<V2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V2d__(ref List<Tensor<V2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V3i__(ref List<Tensor<V3i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V3l__(ref List<Tensor<V3l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V3f__(ref List<Tensor<V3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V3d__(ref List<Tensor<V3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V4i__(ref List<Tensor<V4i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V4l__(ref List<Tensor<V4l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V4f__(ref List<Tensor<V4f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_V4d__(ref List<Tensor<V4d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M22i__(ref List<Tensor<M22i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M22l__(ref List<Tensor<M22l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M22f__(ref List<Tensor<M22f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M22d__(ref List<Tensor<M22d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M23i__(ref List<Tensor<M23i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M23l__(ref List<Tensor<M23l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M23f__(ref List<Tensor<M23f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M23d__(ref List<Tensor<M23d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M33i__(ref List<Tensor<M33i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M33l__(ref List<Tensor<M33l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M33f__(ref List<Tensor<M33f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M33d__(ref List<Tensor<M33d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M34i__(ref List<Tensor<M34i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M34l__(ref List<Tensor<M34l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M34f__(ref List<Tensor<M34f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M34d__(ref List<Tensor<M34d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M44i__(ref List<Tensor<M44i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M44l__(ref List<Tensor<M44l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M44f__(ref List<Tensor<M44f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_M44d__(ref List<Tensor<M44d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C3b__(ref List<Tensor<C3b>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C3us__(ref List<Tensor<C3us>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C3ui__(ref List<Tensor<C3ui>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C3f__(ref List<Tensor<C3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C3d__(ref List<Tensor<C3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C4b__(ref List<Tensor<C4b>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C4us__(ref List<Tensor<C4us>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C4ui__(ref List<Tensor<C4ui>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C4f__(ref List<Tensor<C4f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_C4d__(ref List<Tensor<C4d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1b__(ref List<Tensor<Range1b>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1sb__(ref List<Tensor<Range1sb>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1s__(ref List<Tensor<Range1s>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1us__(ref List<Tensor<Range1us>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1i__(ref List<Tensor<Range1i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1ui__(ref List<Tensor<Range1ui>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1l__(ref List<Tensor<Range1l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1ul__(ref List<Tensor<Range1ul>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1f__(ref List<Tensor<Range1f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Range1d__(ref List<Tensor<Range1d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box2i__(ref List<Tensor<Box2i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box2l__(ref List<Tensor<Box2l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box2f__(ref List<Tensor<Box2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box2d__(ref List<Tensor<Box2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box3i__(ref List<Tensor<Box3i>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box3l__(ref List<Tensor<Box3l>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box3f__(ref List<Tensor<Box3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Box3d__(ref List<Tensor<Box3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Euclidean3f__(ref List<Tensor<Euclidean3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Euclidean3d__(ref List<Tensor<Euclidean3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Rot2f__(ref List<Tensor<Rot2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Rot2d__(ref List<Tensor<Rot2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Rot3f__(ref List<Tensor<Rot3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Rot3d__(ref List<Tensor<Rot3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Scale3f__(ref List<Tensor<Scale3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Scale3d__(ref List<Tensor<Scale3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Shift3f__(ref List<Tensor<Shift3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Shift3d__(ref List<Tensor<Shift3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Trafo2f__(ref List<Tensor<Trafo2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Trafo2d__(ref List<Tensor<Trafo2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Trafo3f__(ref List<Tensor<Trafo3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Trafo3d__(ref List<Tensor<Trafo3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Bool__(ref List<Tensor<bool>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Char__(ref List<Tensor<char>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_String__(ref List<Tensor<string>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Type__(ref List<Tensor<Type>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Guid__(ref List<Tensor<Guid>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Symbol__(ref List<Tensor<Symbol>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Circle2d__(ref List<Tensor<Circle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Line2d__(ref List<Tensor<Line2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Line3d__(ref List<Tensor<Line3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Plane2d__(ref List<Tensor<Plane2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Plane3d__(ref List<Tensor<Plane3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_PlaneWithPoint3d__(ref List<Tensor<PlaneWithPoint3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Quad2d__(ref List<Tensor<Quad2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Quad3d__(ref List<Tensor<Quad3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Ray2d__(ref List<Tensor<Ray2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Ray3d__(ref List<Tensor<Ray3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Sphere3d__(ref List<Tensor<Sphere3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Triangle2d__(ref List<Tensor<Triangle2d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Triangle3d__(ref List<Tensor<Triangle3d>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Circle2f__(ref List<Tensor<Circle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Line2f__(ref List<Tensor<Line2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Line3f__(ref List<Tensor<Line3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Plane2f__(ref List<Tensor<Plane2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Plane3f__(ref List<Tensor<Plane3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_PlaneWithPoint3f__(ref List<Tensor<PlaneWithPoint3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Quad2f__(ref List<Tensor<Quad2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Quad3f__(ref List<Tensor<Quad3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Ray2f__(ref List<Tensor<Ray2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Ray3f__(ref List<Tensor<Ray3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Sphere3f__(ref List<Tensor<Sphere3f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Triangle2f__(ref List<Tensor<Triangle2f>> v) { CodeListOf(v); }
    public void CodeList_of_Tensor_of_Triangle3f__(ref List<Tensor<Triangle3f>> v) { CodeListOf(v); }

    #endregion
}
