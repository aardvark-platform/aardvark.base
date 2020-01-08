using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Aardvark.Base.Coder
{

    // AUTO GENERATED CODE - DO NOT CHANGE!


    public static partial class TypeCoder
    {
        internal static Dictionary<Type, Action<IWritingCoder, object>> TypeWriterMap =
                                    new Dictionary<Type, Action<IWritingCoder, object>>
        {
            #region byte

            { typeof(byte), (c,o) => { var v = (byte)o; c.CodeByte(ref v); } },
            { typeof(byte[]), (c,o) => { var v = (byte[])o; c.CodeByteArray(ref v); } },
            { typeof(List<byte>), (c,o) => { var v = (List<byte>)o; c.CodeList_of_Byte_(ref v); } },

            { typeof(Vector<byte>), (c,o) => { var v = (Vector<byte>)o; c.CodeVector_of_Byte_(ref v); } },
            { typeof(Vector<byte>[]), (c,o) => { var v = (Vector<byte>[])o; c.CodeVector_of_Byte_Array(ref v); } },
            { typeof(List<Vector<byte>>), (c,o) => { var v = (List<Vector<byte>>)o; c.CodeList_of_Vector_of_Byte__(ref v); } },

            { typeof(Matrix<byte>), (c,o) => { var v = (Matrix<byte>)o; c.CodeMatrix_of_Byte_(ref v); } },
            { typeof(Matrix<byte>[]), (c,o) => { var v = (Matrix<byte>[])o; c.CodeMatrix_of_Byte_Array(ref v); } },
            { typeof(List<Matrix<byte>>), (c,o) => { var v = (List<Matrix<byte>>)o; c.CodeList_of_Matrix_of_Byte__(ref v); } },

            { typeof(Volume<byte>), (c,o) => { var v = (Volume<byte>)o; c.CodeVolume_of_Byte_(ref v); } },
            { typeof(Volume<byte>[]), (c,o) => { var v = (Volume<byte>[])o; c.CodeVolume_of_Byte_Array(ref v); } },
            { typeof(List<Volume<byte>>), (c,o) => { var v = (List<Volume<byte>>)o; c.CodeList_of_Volume_of_Byte__(ref v); } },

            { typeof(Tensor<byte>), (c,o) => { var v = (Tensor<byte>)o; c.CodeTensor_of_Byte_(ref v); } },
            { typeof(Tensor<byte>[]), (c,o) => { var v = (Tensor<byte>[])o; c.CodeTensor_of_Byte_Array(ref v); } },
            { typeof(List<Tensor<byte>>), (c,o) => { var v = (List<Tensor<byte>>)o; c.CodeList_of_Tensor_of_Byte__(ref v); } },

            #endregion

            #region sbyte

            { typeof(sbyte), (c,o) => { var v = (sbyte)o; c.CodeSByte(ref v); } },
            { typeof(sbyte[]), (c,o) => { var v = (sbyte[])o; c.CodeSByteArray(ref v); } },
            { typeof(List<sbyte>), (c,o) => { var v = (List<sbyte>)o; c.CodeList_of_SByte_(ref v); } },

            { typeof(Vector<sbyte>), (c,o) => { var v = (Vector<sbyte>)o; c.CodeVector_of_SByte_(ref v); } },
            { typeof(Vector<sbyte>[]), (c,o) => { var v = (Vector<sbyte>[])o; c.CodeVector_of_SByte_Array(ref v); } },
            { typeof(List<Vector<sbyte>>), (c,o) => { var v = (List<Vector<sbyte>>)o; c.CodeList_of_Vector_of_SByte__(ref v); } },

            { typeof(Matrix<sbyte>), (c,o) => { var v = (Matrix<sbyte>)o; c.CodeMatrix_of_SByte_(ref v); } },
            { typeof(Matrix<sbyte>[]), (c,o) => { var v = (Matrix<sbyte>[])o; c.CodeMatrix_of_SByte_Array(ref v); } },
            { typeof(List<Matrix<sbyte>>), (c,o) => { var v = (List<Matrix<sbyte>>)o; c.CodeList_of_Matrix_of_SByte__(ref v); } },

            { typeof(Volume<sbyte>), (c,o) => { var v = (Volume<sbyte>)o; c.CodeVolume_of_SByte_(ref v); } },
            { typeof(Volume<sbyte>[]), (c,o) => { var v = (Volume<sbyte>[])o; c.CodeVolume_of_SByte_Array(ref v); } },
            { typeof(List<Volume<sbyte>>), (c,o) => { var v = (List<Volume<sbyte>>)o; c.CodeList_of_Volume_of_SByte__(ref v); } },

            { typeof(Tensor<sbyte>), (c,o) => { var v = (Tensor<sbyte>)o; c.CodeTensor_of_SByte_(ref v); } },
            { typeof(Tensor<sbyte>[]), (c,o) => { var v = (Tensor<sbyte>[])o; c.CodeTensor_of_SByte_Array(ref v); } },
            { typeof(List<Tensor<sbyte>>), (c,o) => { var v = (List<Tensor<sbyte>>)o; c.CodeList_of_Tensor_of_SByte__(ref v); } },

            #endregion

            #region short

            { typeof(short), (c,o) => { var v = (short)o; c.CodeShort(ref v); } },
            { typeof(short[]), (c,o) => { var v = (short[])o; c.CodeShortArray(ref v); } },
            { typeof(List<short>), (c,o) => { var v = (List<short>)o; c.CodeList_of_Short_(ref v); } },

            { typeof(Vector<short>), (c,o) => { var v = (Vector<short>)o; c.CodeVector_of_Short_(ref v); } },
            { typeof(Vector<short>[]), (c,o) => { var v = (Vector<short>[])o; c.CodeVector_of_Short_Array(ref v); } },
            { typeof(List<Vector<short>>), (c,o) => { var v = (List<Vector<short>>)o; c.CodeList_of_Vector_of_Short__(ref v); } },

            { typeof(Matrix<short>), (c,o) => { var v = (Matrix<short>)o; c.CodeMatrix_of_Short_(ref v); } },
            { typeof(Matrix<short>[]), (c,o) => { var v = (Matrix<short>[])o; c.CodeMatrix_of_Short_Array(ref v); } },
            { typeof(List<Matrix<short>>), (c,o) => { var v = (List<Matrix<short>>)o; c.CodeList_of_Matrix_of_Short__(ref v); } },

            { typeof(Volume<short>), (c,o) => { var v = (Volume<short>)o; c.CodeVolume_of_Short_(ref v); } },
            { typeof(Volume<short>[]), (c,o) => { var v = (Volume<short>[])o; c.CodeVolume_of_Short_Array(ref v); } },
            { typeof(List<Volume<short>>), (c,o) => { var v = (List<Volume<short>>)o; c.CodeList_of_Volume_of_Short__(ref v); } },

            { typeof(Tensor<short>), (c,o) => { var v = (Tensor<short>)o; c.CodeTensor_of_Short_(ref v); } },
            { typeof(Tensor<short>[]), (c,o) => { var v = (Tensor<short>[])o; c.CodeTensor_of_Short_Array(ref v); } },
            { typeof(List<Tensor<short>>), (c,o) => { var v = (List<Tensor<short>>)o; c.CodeList_of_Tensor_of_Short__(ref v); } },

            #endregion

            #region ushort

            { typeof(ushort), (c,o) => { var v = (ushort)o; c.CodeUShort(ref v); } },
            { typeof(ushort[]), (c,o) => { var v = (ushort[])o; c.CodeUShortArray(ref v); } },
            { typeof(List<ushort>), (c,o) => { var v = (List<ushort>)o; c.CodeList_of_UShort_(ref v); } },

            { typeof(Vector<ushort>), (c,o) => { var v = (Vector<ushort>)o; c.CodeVector_of_UShort_(ref v); } },
            { typeof(Vector<ushort>[]), (c,o) => { var v = (Vector<ushort>[])o; c.CodeVector_of_UShort_Array(ref v); } },
            { typeof(List<Vector<ushort>>), (c,o) => { var v = (List<Vector<ushort>>)o; c.CodeList_of_Vector_of_UShort__(ref v); } },

            { typeof(Matrix<ushort>), (c,o) => { var v = (Matrix<ushort>)o; c.CodeMatrix_of_UShort_(ref v); } },
            { typeof(Matrix<ushort>[]), (c,o) => { var v = (Matrix<ushort>[])o; c.CodeMatrix_of_UShort_Array(ref v); } },
            { typeof(List<Matrix<ushort>>), (c,o) => { var v = (List<Matrix<ushort>>)o; c.CodeList_of_Matrix_of_UShort__(ref v); } },

            { typeof(Volume<ushort>), (c,o) => { var v = (Volume<ushort>)o; c.CodeVolume_of_UShort_(ref v); } },
            { typeof(Volume<ushort>[]), (c,o) => { var v = (Volume<ushort>[])o; c.CodeVolume_of_UShort_Array(ref v); } },
            { typeof(List<Volume<ushort>>), (c,o) => { var v = (List<Volume<ushort>>)o; c.CodeList_of_Volume_of_UShort__(ref v); } },

            { typeof(Tensor<ushort>), (c,o) => { var v = (Tensor<ushort>)o; c.CodeTensor_of_UShort_(ref v); } },
            { typeof(Tensor<ushort>[]), (c,o) => { var v = (Tensor<ushort>[])o; c.CodeTensor_of_UShort_Array(ref v); } },
            { typeof(List<Tensor<ushort>>), (c,o) => { var v = (List<Tensor<ushort>>)o; c.CodeList_of_Tensor_of_UShort__(ref v); } },

            #endregion

            #region int

            { typeof(int), (c,o) => { var v = (int)o; c.CodeInt(ref v); } },
            { typeof(int[]), (c,o) => { var v = (int[])o; c.CodeIntArray(ref v); } },
            { typeof(List<int>), (c,o) => { var v = (List<int>)o; c.CodeList_of_Int_(ref v); } },

            { typeof(Vector<int>), (c,o) => { var v = (Vector<int>)o; c.CodeVector_of_Int_(ref v); } },
            { typeof(Vector<int>[]), (c,o) => { var v = (Vector<int>[])o; c.CodeVector_of_Int_Array(ref v); } },
            { typeof(List<Vector<int>>), (c,o) => { var v = (List<Vector<int>>)o; c.CodeList_of_Vector_of_Int__(ref v); } },

            { typeof(Matrix<int>), (c,o) => { var v = (Matrix<int>)o; c.CodeMatrix_of_Int_(ref v); } },
            { typeof(Matrix<int>[]), (c,o) => { var v = (Matrix<int>[])o; c.CodeMatrix_of_Int_Array(ref v); } },
            { typeof(List<Matrix<int>>), (c,o) => { var v = (List<Matrix<int>>)o; c.CodeList_of_Matrix_of_Int__(ref v); } },

            { typeof(Volume<int>), (c,o) => { var v = (Volume<int>)o; c.CodeVolume_of_Int_(ref v); } },
            { typeof(Volume<int>[]), (c,o) => { var v = (Volume<int>[])o; c.CodeVolume_of_Int_Array(ref v); } },
            { typeof(List<Volume<int>>), (c,o) => { var v = (List<Volume<int>>)o; c.CodeList_of_Volume_of_Int__(ref v); } },

            { typeof(Tensor<int>), (c,o) => { var v = (Tensor<int>)o; c.CodeTensor_of_Int_(ref v); } },
            { typeof(Tensor<int>[]), (c,o) => { var v = (Tensor<int>[])o; c.CodeTensor_of_Int_Array(ref v); } },
            { typeof(List<Tensor<int>>), (c,o) => { var v = (List<Tensor<int>>)o; c.CodeList_of_Tensor_of_Int__(ref v); } },

            #endregion

            #region uint

            { typeof(uint), (c,o) => { var v = (uint)o; c.CodeUInt(ref v); } },
            { typeof(uint[]), (c,o) => { var v = (uint[])o; c.CodeUIntArray(ref v); } },
            { typeof(List<uint>), (c,o) => { var v = (List<uint>)o; c.CodeList_of_UInt_(ref v); } },

            { typeof(Vector<uint>), (c,o) => { var v = (Vector<uint>)o; c.CodeVector_of_UInt_(ref v); } },
            { typeof(Vector<uint>[]), (c,o) => { var v = (Vector<uint>[])o; c.CodeVector_of_UInt_Array(ref v); } },
            { typeof(List<Vector<uint>>), (c,o) => { var v = (List<Vector<uint>>)o; c.CodeList_of_Vector_of_UInt__(ref v); } },

            { typeof(Matrix<uint>), (c,o) => { var v = (Matrix<uint>)o; c.CodeMatrix_of_UInt_(ref v); } },
            { typeof(Matrix<uint>[]), (c,o) => { var v = (Matrix<uint>[])o; c.CodeMatrix_of_UInt_Array(ref v); } },
            { typeof(List<Matrix<uint>>), (c,o) => { var v = (List<Matrix<uint>>)o; c.CodeList_of_Matrix_of_UInt__(ref v); } },

            { typeof(Volume<uint>), (c,o) => { var v = (Volume<uint>)o; c.CodeVolume_of_UInt_(ref v); } },
            { typeof(Volume<uint>[]), (c,o) => { var v = (Volume<uint>[])o; c.CodeVolume_of_UInt_Array(ref v); } },
            { typeof(List<Volume<uint>>), (c,o) => { var v = (List<Volume<uint>>)o; c.CodeList_of_Volume_of_UInt__(ref v); } },

            { typeof(Tensor<uint>), (c,o) => { var v = (Tensor<uint>)o; c.CodeTensor_of_UInt_(ref v); } },
            { typeof(Tensor<uint>[]), (c,o) => { var v = (Tensor<uint>[])o; c.CodeTensor_of_UInt_Array(ref v); } },
            { typeof(List<Tensor<uint>>), (c,o) => { var v = (List<Tensor<uint>>)o; c.CodeList_of_Tensor_of_UInt__(ref v); } },

            #endregion

            #region long

            { typeof(long), (c,o) => { var v = (long)o; c.CodeLong(ref v); } },
            { typeof(long[]), (c,o) => { var v = (long[])o; c.CodeLongArray(ref v); } },
            { typeof(List<long>), (c,o) => { var v = (List<long>)o; c.CodeList_of_Long_(ref v); } },

            { typeof(Vector<long>), (c,o) => { var v = (Vector<long>)o; c.CodeVector_of_Long_(ref v); } },
            { typeof(Vector<long>[]), (c,o) => { var v = (Vector<long>[])o; c.CodeVector_of_Long_Array(ref v); } },
            { typeof(List<Vector<long>>), (c,o) => { var v = (List<Vector<long>>)o; c.CodeList_of_Vector_of_Long__(ref v); } },

            { typeof(Matrix<long>), (c,o) => { var v = (Matrix<long>)o; c.CodeMatrix_of_Long_(ref v); } },
            { typeof(Matrix<long>[]), (c,o) => { var v = (Matrix<long>[])o; c.CodeMatrix_of_Long_Array(ref v); } },
            { typeof(List<Matrix<long>>), (c,o) => { var v = (List<Matrix<long>>)o; c.CodeList_of_Matrix_of_Long__(ref v); } },

            { typeof(Volume<long>), (c,o) => { var v = (Volume<long>)o; c.CodeVolume_of_Long_(ref v); } },
            { typeof(Volume<long>[]), (c,o) => { var v = (Volume<long>[])o; c.CodeVolume_of_Long_Array(ref v); } },
            { typeof(List<Volume<long>>), (c,o) => { var v = (List<Volume<long>>)o; c.CodeList_of_Volume_of_Long__(ref v); } },

            { typeof(Tensor<long>), (c,o) => { var v = (Tensor<long>)o; c.CodeTensor_of_Long_(ref v); } },
            { typeof(Tensor<long>[]), (c,o) => { var v = (Tensor<long>[])o; c.CodeTensor_of_Long_Array(ref v); } },
            { typeof(List<Tensor<long>>), (c,o) => { var v = (List<Tensor<long>>)o; c.CodeList_of_Tensor_of_Long__(ref v); } },

            #endregion

            #region ulong

            { typeof(ulong), (c,o) => { var v = (ulong)o; c.CodeULong(ref v); } },
            { typeof(ulong[]), (c,o) => { var v = (ulong[])o; c.CodeULongArray(ref v); } },
            { typeof(List<ulong>), (c,o) => { var v = (List<ulong>)o; c.CodeList_of_ULong_(ref v); } },

            { typeof(Vector<ulong>), (c,o) => { var v = (Vector<ulong>)o; c.CodeVector_of_ULong_(ref v); } },
            { typeof(Vector<ulong>[]), (c,o) => { var v = (Vector<ulong>[])o; c.CodeVector_of_ULong_Array(ref v); } },
            { typeof(List<Vector<ulong>>), (c,o) => { var v = (List<Vector<ulong>>)o; c.CodeList_of_Vector_of_ULong__(ref v); } },

            { typeof(Matrix<ulong>), (c,o) => { var v = (Matrix<ulong>)o; c.CodeMatrix_of_ULong_(ref v); } },
            { typeof(Matrix<ulong>[]), (c,o) => { var v = (Matrix<ulong>[])o; c.CodeMatrix_of_ULong_Array(ref v); } },
            { typeof(List<Matrix<ulong>>), (c,o) => { var v = (List<Matrix<ulong>>)o; c.CodeList_of_Matrix_of_ULong__(ref v); } },

            { typeof(Volume<ulong>), (c,o) => { var v = (Volume<ulong>)o; c.CodeVolume_of_ULong_(ref v); } },
            { typeof(Volume<ulong>[]), (c,o) => { var v = (Volume<ulong>[])o; c.CodeVolume_of_ULong_Array(ref v); } },
            { typeof(List<Volume<ulong>>), (c,o) => { var v = (List<Volume<ulong>>)o; c.CodeList_of_Volume_of_ULong__(ref v); } },

            { typeof(Tensor<ulong>), (c,o) => { var v = (Tensor<ulong>)o; c.CodeTensor_of_ULong_(ref v); } },
            { typeof(Tensor<ulong>[]), (c,o) => { var v = (Tensor<ulong>[])o; c.CodeTensor_of_ULong_Array(ref v); } },
            { typeof(List<Tensor<ulong>>), (c,o) => { var v = (List<Tensor<ulong>>)o; c.CodeList_of_Tensor_of_ULong__(ref v); } },

            #endregion

            #region float

            { typeof(float), (c,o) => { var v = (float)o; c.CodeFloat(ref v); } },
            { typeof(float[]), (c,o) => { var v = (float[])o; c.CodeFloatArray(ref v); } },
            { typeof(List<float>), (c,o) => { var v = (List<float>)o; c.CodeList_of_Float_(ref v); } },

            { typeof(Vector<float>), (c,o) => { var v = (Vector<float>)o; c.CodeVector_of_Float_(ref v); } },
            { typeof(Vector<float>[]), (c,o) => { var v = (Vector<float>[])o; c.CodeVector_of_Float_Array(ref v); } },
            { typeof(List<Vector<float>>), (c,o) => { var v = (List<Vector<float>>)o; c.CodeList_of_Vector_of_Float__(ref v); } },

            { typeof(Matrix<float>), (c,o) => { var v = (Matrix<float>)o; c.CodeMatrix_of_Float_(ref v); } },
            { typeof(Matrix<float>[]), (c,o) => { var v = (Matrix<float>[])o; c.CodeMatrix_of_Float_Array(ref v); } },
            { typeof(List<Matrix<float>>), (c,o) => { var v = (List<Matrix<float>>)o; c.CodeList_of_Matrix_of_Float__(ref v); } },

            { typeof(Volume<float>), (c,o) => { var v = (Volume<float>)o; c.CodeVolume_of_Float_(ref v); } },
            { typeof(Volume<float>[]), (c,o) => { var v = (Volume<float>[])o; c.CodeVolume_of_Float_Array(ref v); } },
            { typeof(List<Volume<float>>), (c,o) => { var v = (List<Volume<float>>)o; c.CodeList_of_Volume_of_Float__(ref v); } },

            { typeof(Tensor<float>), (c,o) => { var v = (Tensor<float>)o; c.CodeTensor_of_Float_(ref v); } },
            { typeof(Tensor<float>[]), (c,o) => { var v = (Tensor<float>[])o; c.CodeTensor_of_Float_Array(ref v); } },
            { typeof(List<Tensor<float>>), (c,o) => { var v = (List<Tensor<float>>)o; c.CodeList_of_Tensor_of_Float__(ref v); } },

            #endregion

            #region double

            { typeof(double), (c,o) => { var v = (double)o; c.CodeDouble(ref v); } },
            { typeof(double[]), (c,o) => { var v = (double[])o; c.CodeDoubleArray(ref v); } },
            { typeof(List<double>), (c,o) => { var v = (List<double>)o; c.CodeList_of_Double_(ref v); } },

            { typeof(Vector<double>), (c,o) => { var v = (Vector<double>)o; c.CodeVector_of_Double_(ref v); } },
            { typeof(Vector<double>[]), (c,o) => { var v = (Vector<double>[])o; c.CodeVector_of_Double_Array(ref v); } },
            { typeof(List<Vector<double>>), (c,o) => { var v = (List<Vector<double>>)o; c.CodeList_of_Vector_of_Double__(ref v); } },

            { typeof(Matrix<double>), (c,o) => { var v = (Matrix<double>)o; c.CodeMatrix_of_Double_(ref v); } },
            { typeof(Matrix<double>[]), (c,o) => { var v = (Matrix<double>[])o; c.CodeMatrix_of_Double_Array(ref v); } },
            { typeof(List<Matrix<double>>), (c,o) => { var v = (List<Matrix<double>>)o; c.CodeList_of_Matrix_of_Double__(ref v); } },

            { typeof(Volume<double>), (c,o) => { var v = (Volume<double>)o; c.CodeVolume_of_Double_(ref v); } },
            { typeof(Volume<double>[]), (c,o) => { var v = (Volume<double>[])o; c.CodeVolume_of_Double_Array(ref v); } },
            { typeof(List<Volume<double>>), (c,o) => { var v = (List<Volume<double>>)o; c.CodeList_of_Volume_of_Double__(ref v); } },

            { typeof(Tensor<double>), (c,o) => { var v = (Tensor<double>)o; c.CodeTensor_of_Double_(ref v); } },
            { typeof(Tensor<double>[]), (c,o) => { var v = (Tensor<double>[])o; c.CodeTensor_of_Double_Array(ref v); } },
            { typeof(List<Tensor<double>>), (c,o) => { var v = (List<Tensor<double>>)o; c.CodeList_of_Tensor_of_Double__(ref v); } },

            #endregion

            #region Fraction

            { typeof(Fraction), (c,o) => { var v = (Fraction)o; c.CodeFraction(ref v); } },
            { typeof(Fraction[]), (c,o) => { var v = (Fraction[])o; c.CodeFractionArray(ref v); } },
            { typeof(List<Fraction>), (c,o) => { var v = (List<Fraction>)o; c.CodeList_of_Fraction_(ref v); } },

            { typeof(Vector<Fraction>), (c,o) => { var v = (Vector<Fraction>)o; c.CodeVector_of_Fraction_(ref v); } },
            { typeof(Vector<Fraction>[]), (c,o) => { var v = (Vector<Fraction>[])o; c.CodeVector_of_Fraction_Array(ref v); } },
            { typeof(List<Vector<Fraction>>), (c,o) => { var v = (List<Vector<Fraction>>)o; c.CodeList_of_Vector_of_Fraction__(ref v); } },

            { typeof(Matrix<Fraction>), (c,o) => { var v = (Matrix<Fraction>)o; c.CodeMatrix_of_Fraction_(ref v); } },
            { typeof(Matrix<Fraction>[]), (c,o) => { var v = (Matrix<Fraction>[])o; c.CodeMatrix_of_Fraction_Array(ref v); } },
            { typeof(List<Matrix<Fraction>>), (c,o) => { var v = (List<Matrix<Fraction>>)o; c.CodeList_of_Matrix_of_Fraction__(ref v); } },

            { typeof(Volume<Fraction>), (c,o) => { var v = (Volume<Fraction>)o; c.CodeVolume_of_Fraction_(ref v); } },
            { typeof(Volume<Fraction>[]), (c,o) => { var v = (Volume<Fraction>[])o; c.CodeVolume_of_Fraction_Array(ref v); } },
            { typeof(List<Volume<Fraction>>), (c,o) => { var v = (List<Volume<Fraction>>)o; c.CodeList_of_Volume_of_Fraction__(ref v); } },

            { typeof(Tensor<Fraction>), (c,o) => { var v = (Tensor<Fraction>)o; c.CodeTensor_of_Fraction_(ref v); } },
            { typeof(Tensor<Fraction>[]), (c,o) => { var v = (Tensor<Fraction>[])o; c.CodeTensor_of_Fraction_Array(ref v); } },
            { typeof(List<Tensor<Fraction>>), (c,o) => { var v = (List<Tensor<Fraction>>)o; c.CodeList_of_Tensor_of_Fraction__(ref v); } },

            #endregion

            #region V2i

            { typeof(V2i), (c,o) => { var v = (V2i)o; c.CodeV2i(ref v); } },
            { typeof(V2i[]), (c,o) => { var v = (V2i[])o; c.CodeV2iArray(ref v); } },
            { typeof(List<V2i>), (c,o) => { var v = (List<V2i>)o; c.CodeList_of_V2i_(ref v); } },

            { typeof(Vector<V2i>), (c,o) => { var v = (Vector<V2i>)o; c.CodeVector_of_V2i_(ref v); } },
            { typeof(Vector<V2i>[]), (c,o) => { var v = (Vector<V2i>[])o; c.CodeVector_of_V2i_Array(ref v); } },
            { typeof(List<Vector<V2i>>), (c,o) => { var v = (List<Vector<V2i>>)o; c.CodeList_of_Vector_of_V2i__(ref v); } },

            { typeof(Matrix<V2i>), (c,o) => { var v = (Matrix<V2i>)o; c.CodeMatrix_of_V2i_(ref v); } },
            { typeof(Matrix<V2i>[]), (c,o) => { var v = (Matrix<V2i>[])o; c.CodeMatrix_of_V2i_Array(ref v); } },
            { typeof(List<Matrix<V2i>>), (c,o) => { var v = (List<Matrix<V2i>>)o; c.CodeList_of_Matrix_of_V2i__(ref v); } },

            { typeof(Volume<V2i>), (c,o) => { var v = (Volume<V2i>)o; c.CodeVolume_of_V2i_(ref v); } },
            { typeof(Volume<V2i>[]), (c,o) => { var v = (Volume<V2i>[])o; c.CodeVolume_of_V2i_Array(ref v); } },
            { typeof(List<Volume<V2i>>), (c,o) => { var v = (List<Volume<V2i>>)o; c.CodeList_of_Volume_of_V2i__(ref v); } },

            { typeof(Tensor<V2i>), (c,o) => { var v = (Tensor<V2i>)o; c.CodeTensor_of_V2i_(ref v); } },
            { typeof(Tensor<V2i>[]), (c,o) => { var v = (Tensor<V2i>[])o; c.CodeTensor_of_V2i_Array(ref v); } },
            { typeof(List<Tensor<V2i>>), (c,o) => { var v = (List<Tensor<V2i>>)o; c.CodeList_of_Tensor_of_V2i__(ref v); } },

            #endregion

            #region V2l

            { typeof(V2l), (c,o) => { var v = (V2l)o; c.CodeV2l(ref v); } },
            { typeof(V2l[]), (c,o) => { var v = (V2l[])o; c.CodeV2lArray(ref v); } },
            { typeof(List<V2l>), (c,o) => { var v = (List<V2l>)o; c.CodeList_of_V2l_(ref v); } },

            { typeof(Vector<V2l>), (c,o) => { var v = (Vector<V2l>)o; c.CodeVector_of_V2l_(ref v); } },
            { typeof(Vector<V2l>[]), (c,o) => { var v = (Vector<V2l>[])o; c.CodeVector_of_V2l_Array(ref v); } },
            { typeof(List<Vector<V2l>>), (c,o) => { var v = (List<Vector<V2l>>)o; c.CodeList_of_Vector_of_V2l__(ref v); } },

            { typeof(Matrix<V2l>), (c,o) => { var v = (Matrix<V2l>)o; c.CodeMatrix_of_V2l_(ref v); } },
            { typeof(Matrix<V2l>[]), (c,o) => { var v = (Matrix<V2l>[])o; c.CodeMatrix_of_V2l_Array(ref v); } },
            { typeof(List<Matrix<V2l>>), (c,o) => { var v = (List<Matrix<V2l>>)o; c.CodeList_of_Matrix_of_V2l__(ref v); } },

            { typeof(Volume<V2l>), (c,o) => { var v = (Volume<V2l>)o; c.CodeVolume_of_V2l_(ref v); } },
            { typeof(Volume<V2l>[]), (c,o) => { var v = (Volume<V2l>[])o; c.CodeVolume_of_V2l_Array(ref v); } },
            { typeof(List<Volume<V2l>>), (c,o) => { var v = (List<Volume<V2l>>)o; c.CodeList_of_Volume_of_V2l__(ref v); } },

            { typeof(Tensor<V2l>), (c,o) => { var v = (Tensor<V2l>)o; c.CodeTensor_of_V2l_(ref v); } },
            { typeof(Tensor<V2l>[]), (c,o) => { var v = (Tensor<V2l>[])o; c.CodeTensor_of_V2l_Array(ref v); } },
            { typeof(List<Tensor<V2l>>), (c,o) => { var v = (List<Tensor<V2l>>)o; c.CodeList_of_Tensor_of_V2l__(ref v); } },

            #endregion

            #region V2f

            { typeof(V2f), (c,o) => { var v = (V2f)o; c.CodeV2f(ref v); } },
            { typeof(V2f[]), (c,o) => { var v = (V2f[])o; c.CodeV2fArray(ref v); } },
            { typeof(List<V2f>), (c,o) => { var v = (List<V2f>)o; c.CodeList_of_V2f_(ref v); } },

            { typeof(Vector<V2f>), (c,o) => { var v = (Vector<V2f>)o; c.CodeVector_of_V2f_(ref v); } },
            { typeof(Vector<V2f>[]), (c,o) => { var v = (Vector<V2f>[])o; c.CodeVector_of_V2f_Array(ref v); } },
            { typeof(List<Vector<V2f>>), (c,o) => { var v = (List<Vector<V2f>>)o; c.CodeList_of_Vector_of_V2f__(ref v); } },

            { typeof(Matrix<V2f>), (c,o) => { var v = (Matrix<V2f>)o; c.CodeMatrix_of_V2f_(ref v); } },
            { typeof(Matrix<V2f>[]), (c,o) => { var v = (Matrix<V2f>[])o; c.CodeMatrix_of_V2f_Array(ref v); } },
            { typeof(List<Matrix<V2f>>), (c,o) => { var v = (List<Matrix<V2f>>)o; c.CodeList_of_Matrix_of_V2f__(ref v); } },

            { typeof(Volume<V2f>), (c,o) => { var v = (Volume<V2f>)o; c.CodeVolume_of_V2f_(ref v); } },
            { typeof(Volume<V2f>[]), (c,o) => { var v = (Volume<V2f>[])o; c.CodeVolume_of_V2f_Array(ref v); } },
            { typeof(List<Volume<V2f>>), (c,o) => { var v = (List<Volume<V2f>>)o; c.CodeList_of_Volume_of_V2f__(ref v); } },

            { typeof(Tensor<V2f>), (c,o) => { var v = (Tensor<V2f>)o; c.CodeTensor_of_V2f_(ref v); } },
            { typeof(Tensor<V2f>[]), (c,o) => { var v = (Tensor<V2f>[])o; c.CodeTensor_of_V2f_Array(ref v); } },
            { typeof(List<Tensor<V2f>>), (c,o) => { var v = (List<Tensor<V2f>>)o; c.CodeList_of_Tensor_of_V2f__(ref v); } },

            #endregion

            #region V2d

            { typeof(V2d), (c,o) => { var v = (V2d)o; c.CodeV2d(ref v); } },
            { typeof(V2d[]), (c,o) => { var v = (V2d[])o; c.CodeV2dArray(ref v); } },
            { typeof(List<V2d>), (c,o) => { var v = (List<V2d>)o; c.CodeList_of_V2d_(ref v); } },

            { typeof(Vector<V2d>), (c,o) => { var v = (Vector<V2d>)o; c.CodeVector_of_V2d_(ref v); } },
            { typeof(Vector<V2d>[]), (c,o) => { var v = (Vector<V2d>[])o; c.CodeVector_of_V2d_Array(ref v); } },
            { typeof(List<Vector<V2d>>), (c,o) => { var v = (List<Vector<V2d>>)o; c.CodeList_of_Vector_of_V2d__(ref v); } },

            { typeof(Matrix<V2d>), (c,o) => { var v = (Matrix<V2d>)o; c.CodeMatrix_of_V2d_(ref v); } },
            { typeof(Matrix<V2d>[]), (c,o) => { var v = (Matrix<V2d>[])o; c.CodeMatrix_of_V2d_Array(ref v); } },
            { typeof(List<Matrix<V2d>>), (c,o) => { var v = (List<Matrix<V2d>>)o; c.CodeList_of_Matrix_of_V2d__(ref v); } },

            { typeof(Volume<V2d>), (c,o) => { var v = (Volume<V2d>)o; c.CodeVolume_of_V2d_(ref v); } },
            { typeof(Volume<V2d>[]), (c,o) => { var v = (Volume<V2d>[])o; c.CodeVolume_of_V2d_Array(ref v); } },
            { typeof(List<Volume<V2d>>), (c,o) => { var v = (List<Volume<V2d>>)o; c.CodeList_of_Volume_of_V2d__(ref v); } },

            { typeof(Tensor<V2d>), (c,o) => { var v = (Tensor<V2d>)o; c.CodeTensor_of_V2d_(ref v); } },
            { typeof(Tensor<V2d>[]), (c,o) => { var v = (Tensor<V2d>[])o; c.CodeTensor_of_V2d_Array(ref v); } },
            { typeof(List<Tensor<V2d>>), (c,o) => { var v = (List<Tensor<V2d>>)o; c.CodeList_of_Tensor_of_V2d__(ref v); } },

            #endregion

            #region V3i

            { typeof(V3i), (c,o) => { var v = (V3i)o; c.CodeV3i(ref v); } },
            { typeof(V3i[]), (c,o) => { var v = (V3i[])o; c.CodeV3iArray(ref v); } },
            { typeof(List<V3i>), (c,o) => { var v = (List<V3i>)o; c.CodeList_of_V3i_(ref v); } },

            { typeof(Vector<V3i>), (c,o) => { var v = (Vector<V3i>)o; c.CodeVector_of_V3i_(ref v); } },
            { typeof(Vector<V3i>[]), (c,o) => { var v = (Vector<V3i>[])o; c.CodeVector_of_V3i_Array(ref v); } },
            { typeof(List<Vector<V3i>>), (c,o) => { var v = (List<Vector<V3i>>)o; c.CodeList_of_Vector_of_V3i__(ref v); } },

            { typeof(Matrix<V3i>), (c,o) => { var v = (Matrix<V3i>)o; c.CodeMatrix_of_V3i_(ref v); } },
            { typeof(Matrix<V3i>[]), (c,o) => { var v = (Matrix<V3i>[])o; c.CodeMatrix_of_V3i_Array(ref v); } },
            { typeof(List<Matrix<V3i>>), (c,o) => { var v = (List<Matrix<V3i>>)o; c.CodeList_of_Matrix_of_V3i__(ref v); } },

            { typeof(Volume<V3i>), (c,o) => { var v = (Volume<V3i>)o; c.CodeVolume_of_V3i_(ref v); } },
            { typeof(Volume<V3i>[]), (c,o) => { var v = (Volume<V3i>[])o; c.CodeVolume_of_V3i_Array(ref v); } },
            { typeof(List<Volume<V3i>>), (c,o) => { var v = (List<Volume<V3i>>)o; c.CodeList_of_Volume_of_V3i__(ref v); } },

            { typeof(Tensor<V3i>), (c,o) => { var v = (Tensor<V3i>)o; c.CodeTensor_of_V3i_(ref v); } },
            { typeof(Tensor<V3i>[]), (c,o) => { var v = (Tensor<V3i>[])o; c.CodeTensor_of_V3i_Array(ref v); } },
            { typeof(List<Tensor<V3i>>), (c,o) => { var v = (List<Tensor<V3i>>)o; c.CodeList_of_Tensor_of_V3i__(ref v); } },

            #endregion

            #region V3l

            { typeof(V3l), (c,o) => { var v = (V3l)o; c.CodeV3l(ref v); } },
            { typeof(V3l[]), (c,o) => { var v = (V3l[])o; c.CodeV3lArray(ref v); } },
            { typeof(List<V3l>), (c,o) => { var v = (List<V3l>)o; c.CodeList_of_V3l_(ref v); } },

            { typeof(Vector<V3l>), (c,o) => { var v = (Vector<V3l>)o; c.CodeVector_of_V3l_(ref v); } },
            { typeof(Vector<V3l>[]), (c,o) => { var v = (Vector<V3l>[])o; c.CodeVector_of_V3l_Array(ref v); } },
            { typeof(List<Vector<V3l>>), (c,o) => { var v = (List<Vector<V3l>>)o; c.CodeList_of_Vector_of_V3l__(ref v); } },

            { typeof(Matrix<V3l>), (c,o) => { var v = (Matrix<V3l>)o; c.CodeMatrix_of_V3l_(ref v); } },
            { typeof(Matrix<V3l>[]), (c,o) => { var v = (Matrix<V3l>[])o; c.CodeMatrix_of_V3l_Array(ref v); } },
            { typeof(List<Matrix<V3l>>), (c,o) => { var v = (List<Matrix<V3l>>)o; c.CodeList_of_Matrix_of_V3l__(ref v); } },

            { typeof(Volume<V3l>), (c,o) => { var v = (Volume<V3l>)o; c.CodeVolume_of_V3l_(ref v); } },
            { typeof(Volume<V3l>[]), (c,o) => { var v = (Volume<V3l>[])o; c.CodeVolume_of_V3l_Array(ref v); } },
            { typeof(List<Volume<V3l>>), (c,o) => { var v = (List<Volume<V3l>>)o; c.CodeList_of_Volume_of_V3l__(ref v); } },

            { typeof(Tensor<V3l>), (c,o) => { var v = (Tensor<V3l>)o; c.CodeTensor_of_V3l_(ref v); } },
            { typeof(Tensor<V3l>[]), (c,o) => { var v = (Tensor<V3l>[])o; c.CodeTensor_of_V3l_Array(ref v); } },
            { typeof(List<Tensor<V3l>>), (c,o) => { var v = (List<Tensor<V3l>>)o; c.CodeList_of_Tensor_of_V3l__(ref v); } },

            #endregion

            #region V3f

            { typeof(V3f), (c,o) => { var v = (V3f)o; c.CodeV3f(ref v); } },
            { typeof(V3f[]), (c,o) => { var v = (V3f[])o; c.CodeV3fArray(ref v); } },
            { typeof(List<V3f>), (c,o) => { var v = (List<V3f>)o; c.CodeList_of_V3f_(ref v); } },

            { typeof(Vector<V3f>), (c,o) => { var v = (Vector<V3f>)o; c.CodeVector_of_V3f_(ref v); } },
            { typeof(Vector<V3f>[]), (c,o) => { var v = (Vector<V3f>[])o; c.CodeVector_of_V3f_Array(ref v); } },
            { typeof(List<Vector<V3f>>), (c,o) => { var v = (List<Vector<V3f>>)o; c.CodeList_of_Vector_of_V3f__(ref v); } },

            { typeof(Matrix<V3f>), (c,o) => { var v = (Matrix<V3f>)o; c.CodeMatrix_of_V3f_(ref v); } },
            { typeof(Matrix<V3f>[]), (c,o) => { var v = (Matrix<V3f>[])o; c.CodeMatrix_of_V3f_Array(ref v); } },
            { typeof(List<Matrix<V3f>>), (c,o) => { var v = (List<Matrix<V3f>>)o; c.CodeList_of_Matrix_of_V3f__(ref v); } },

            { typeof(Volume<V3f>), (c,o) => { var v = (Volume<V3f>)o; c.CodeVolume_of_V3f_(ref v); } },
            { typeof(Volume<V3f>[]), (c,o) => { var v = (Volume<V3f>[])o; c.CodeVolume_of_V3f_Array(ref v); } },
            { typeof(List<Volume<V3f>>), (c,o) => { var v = (List<Volume<V3f>>)o; c.CodeList_of_Volume_of_V3f__(ref v); } },

            { typeof(Tensor<V3f>), (c,o) => { var v = (Tensor<V3f>)o; c.CodeTensor_of_V3f_(ref v); } },
            { typeof(Tensor<V3f>[]), (c,o) => { var v = (Tensor<V3f>[])o; c.CodeTensor_of_V3f_Array(ref v); } },
            { typeof(List<Tensor<V3f>>), (c,o) => { var v = (List<Tensor<V3f>>)o; c.CodeList_of_Tensor_of_V3f__(ref v); } },

            #endregion

            #region V3d

            { typeof(V3d), (c,o) => { var v = (V3d)o; c.CodeV3d(ref v); } },
            { typeof(V3d[]), (c,o) => { var v = (V3d[])o; c.CodeV3dArray(ref v); } },
            { typeof(List<V3d>), (c,o) => { var v = (List<V3d>)o; c.CodeList_of_V3d_(ref v); } },

            { typeof(Vector<V3d>), (c,o) => { var v = (Vector<V3d>)o; c.CodeVector_of_V3d_(ref v); } },
            { typeof(Vector<V3d>[]), (c,o) => { var v = (Vector<V3d>[])o; c.CodeVector_of_V3d_Array(ref v); } },
            { typeof(List<Vector<V3d>>), (c,o) => { var v = (List<Vector<V3d>>)o; c.CodeList_of_Vector_of_V3d__(ref v); } },

            { typeof(Matrix<V3d>), (c,o) => { var v = (Matrix<V3d>)o; c.CodeMatrix_of_V3d_(ref v); } },
            { typeof(Matrix<V3d>[]), (c,o) => { var v = (Matrix<V3d>[])o; c.CodeMatrix_of_V3d_Array(ref v); } },
            { typeof(List<Matrix<V3d>>), (c,o) => { var v = (List<Matrix<V3d>>)o; c.CodeList_of_Matrix_of_V3d__(ref v); } },

            { typeof(Volume<V3d>), (c,o) => { var v = (Volume<V3d>)o; c.CodeVolume_of_V3d_(ref v); } },
            { typeof(Volume<V3d>[]), (c,o) => { var v = (Volume<V3d>[])o; c.CodeVolume_of_V3d_Array(ref v); } },
            { typeof(List<Volume<V3d>>), (c,o) => { var v = (List<Volume<V3d>>)o; c.CodeList_of_Volume_of_V3d__(ref v); } },

            { typeof(Tensor<V3d>), (c,o) => { var v = (Tensor<V3d>)o; c.CodeTensor_of_V3d_(ref v); } },
            { typeof(Tensor<V3d>[]), (c,o) => { var v = (Tensor<V3d>[])o; c.CodeTensor_of_V3d_Array(ref v); } },
            { typeof(List<Tensor<V3d>>), (c,o) => { var v = (List<Tensor<V3d>>)o; c.CodeList_of_Tensor_of_V3d__(ref v); } },

            #endregion

            #region V4i

            { typeof(V4i), (c,o) => { var v = (V4i)o; c.CodeV4i(ref v); } },
            { typeof(V4i[]), (c,o) => { var v = (V4i[])o; c.CodeV4iArray(ref v); } },
            { typeof(List<V4i>), (c,o) => { var v = (List<V4i>)o; c.CodeList_of_V4i_(ref v); } },

            { typeof(Vector<V4i>), (c,o) => { var v = (Vector<V4i>)o; c.CodeVector_of_V4i_(ref v); } },
            { typeof(Vector<V4i>[]), (c,o) => { var v = (Vector<V4i>[])o; c.CodeVector_of_V4i_Array(ref v); } },
            { typeof(List<Vector<V4i>>), (c,o) => { var v = (List<Vector<V4i>>)o; c.CodeList_of_Vector_of_V4i__(ref v); } },

            { typeof(Matrix<V4i>), (c,o) => { var v = (Matrix<V4i>)o; c.CodeMatrix_of_V4i_(ref v); } },
            { typeof(Matrix<V4i>[]), (c,o) => { var v = (Matrix<V4i>[])o; c.CodeMatrix_of_V4i_Array(ref v); } },
            { typeof(List<Matrix<V4i>>), (c,o) => { var v = (List<Matrix<V4i>>)o; c.CodeList_of_Matrix_of_V4i__(ref v); } },

            { typeof(Volume<V4i>), (c,o) => { var v = (Volume<V4i>)o; c.CodeVolume_of_V4i_(ref v); } },
            { typeof(Volume<V4i>[]), (c,o) => { var v = (Volume<V4i>[])o; c.CodeVolume_of_V4i_Array(ref v); } },
            { typeof(List<Volume<V4i>>), (c,o) => { var v = (List<Volume<V4i>>)o; c.CodeList_of_Volume_of_V4i__(ref v); } },

            { typeof(Tensor<V4i>), (c,o) => { var v = (Tensor<V4i>)o; c.CodeTensor_of_V4i_(ref v); } },
            { typeof(Tensor<V4i>[]), (c,o) => { var v = (Tensor<V4i>[])o; c.CodeTensor_of_V4i_Array(ref v); } },
            { typeof(List<Tensor<V4i>>), (c,o) => { var v = (List<Tensor<V4i>>)o; c.CodeList_of_Tensor_of_V4i__(ref v); } },

            #endregion

            #region V4l

            { typeof(V4l), (c,o) => { var v = (V4l)o; c.CodeV4l(ref v); } },
            { typeof(V4l[]), (c,o) => { var v = (V4l[])o; c.CodeV4lArray(ref v); } },
            { typeof(List<V4l>), (c,o) => { var v = (List<V4l>)o; c.CodeList_of_V4l_(ref v); } },

            { typeof(Vector<V4l>), (c,o) => { var v = (Vector<V4l>)o; c.CodeVector_of_V4l_(ref v); } },
            { typeof(Vector<V4l>[]), (c,o) => { var v = (Vector<V4l>[])o; c.CodeVector_of_V4l_Array(ref v); } },
            { typeof(List<Vector<V4l>>), (c,o) => { var v = (List<Vector<V4l>>)o; c.CodeList_of_Vector_of_V4l__(ref v); } },

            { typeof(Matrix<V4l>), (c,o) => { var v = (Matrix<V4l>)o; c.CodeMatrix_of_V4l_(ref v); } },
            { typeof(Matrix<V4l>[]), (c,o) => { var v = (Matrix<V4l>[])o; c.CodeMatrix_of_V4l_Array(ref v); } },
            { typeof(List<Matrix<V4l>>), (c,o) => { var v = (List<Matrix<V4l>>)o; c.CodeList_of_Matrix_of_V4l__(ref v); } },

            { typeof(Volume<V4l>), (c,o) => { var v = (Volume<V4l>)o; c.CodeVolume_of_V4l_(ref v); } },
            { typeof(Volume<V4l>[]), (c,o) => { var v = (Volume<V4l>[])o; c.CodeVolume_of_V4l_Array(ref v); } },
            { typeof(List<Volume<V4l>>), (c,o) => { var v = (List<Volume<V4l>>)o; c.CodeList_of_Volume_of_V4l__(ref v); } },

            { typeof(Tensor<V4l>), (c,o) => { var v = (Tensor<V4l>)o; c.CodeTensor_of_V4l_(ref v); } },
            { typeof(Tensor<V4l>[]), (c,o) => { var v = (Tensor<V4l>[])o; c.CodeTensor_of_V4l_Array(ref v); } },
            { typeof(List<Tensor<V4l>>), (c,o) => { var v = (List<Tensor<V4l>>)o; c.CodeList_of_Tensor_of_V4l__(ref v); } },

            #endregion

            #region V4f

            { typeof(V4f), (c,o) => { var v = (V4f)o; c.CodeV4f(ref v); } },
            { typeof(V4f[]), (c,o) => { var v = (V4f[])o; c.CodeV4fArray(ref v); } },
            { typeof(List<V4f>), (c,o) => { var v = (List<V4f>)o; c.CodeList_of_V4f_(ref v); } },

            { typeof(Vector<V4f>), (c,o) => { var v = (Vector<V4f>)o; c.CodeVector_of_V4f_(ref v); } },
            { typeof(Vector<V4f>[]), (c,o) => { var v = (Vector<V4f>[])o; c.CodeVector_of_V4f_Array(ref v); } },
            { typeof(List<Vector<V4f>>), (c,o) => { var v = (List<Vector<V4f>>)o; c.CodeList_of_Vector_of_V4f__(ref v); } },

            { typeof(Matrix<V4f>), (c,o) => { var v = (Matrix<V4f>)o; c.CodeMatrix_of_V4f_(ref v); } },
            { typeof(Matrix<V4f>[]), (c,o) => { var v = (Matrix<V4f>[])o; c.CodeMatrix_of_V4f_Array(ref v); } },
            { typeof(List<Matrix<V4f>>), (c,o) => { var v = (List<Matrix<V4f>>)o; c.CodeList_of_Matrix_of_V4f__(ref v); } },

            { typeof(Volume<V4f>), (c,o) => { var v = (Volume<V4f>)o; c.CodeVolume_of_V4f_(ref v); } },
            { typeof(Volume<V4f>[]), (c,o) => { var v = (Volume<V4f>[])o; c.CodeVolume_of_V4f_Array(ref v); } },
            { typeof(List<Volume<V4f>>), (c,o) => { var v = (List<Volume<V4f>>)o; c.CodeList_of_Volume_of_V4f__(ref v); } },

            { typeof(Tensor<V4f>), (c,o) => { var v = (Tensor<V4f>)o; c.CodeTensor_of_V4f_(ref v); } },
            { typeof(Tensor<V4f>[]), (c,o) => { var v = (Tensor<V4f>[])o; c.CodeTensor_of_V4f_Array(ref v); } },
            { typeof(List<Tensor<V4f>>), (c,o) => { var v = (List<Tensor<V4f>>)o; c.CodeList_of_Tensor_of_V4f__(ref v); } },

            #endregion

            #region V4d

            { typeof(V4d), (c,o) => { var v = (V4d)o; c.CodeV4d(ref v); } },
            { typeof(V4d[]), (c,o) => { var v = (V4d[])o; c.CodeV4dArray(ref v); } },
            { typeof(List<V4d>), (c,o) => { var v = (List<V4d>)o; c.CodeList_of_V4d_(ref v); } },

            { typeof(Vector<V4d>), (c,o) => { var v = (Vector<V4d>)o; c.CodeVector_of_V4d_(ref v); } },
            { typeof(Vector<V4d>[]), (c,o) => { var v = (Vector<V4d>[])o; c.CodeVector_of_V4d_Array(ref v); } },
            { typeof(List<Vector<V4d>>), (c,o) => { var v = (List<Vector<V4d>>)o; c.CodeList_of_Vector_of_V4d__(ref v); } },

            { typeof(Matrix<V4d>), (c,o) => { var v = (Matrix<V4d>)o; c.CodeMatrix_of_V4d_(ref v); } },
            { typeof(Matrix<V4d>[]), (c,o) => { var v = (Matrix<V4d>[])o; c.CodeMatrix_of_V4d_Array(ref v); } },
            { typeof(List<Matrix<V4d>>), (c,o) => { var v = (List<Matrix<V4d>>)o; c.CodeList_of_Matrix_of_V4d__(ref v); } },

            { typeof(Volume<V4d>), (c,o) => { var v = (Volume<V4d>)o; c.CodeVolume_of_V4d_(ref v); } },
            { typeof(Volume<V4d>[]), (c,o) => { var v = (Volume<V4d>[])o; c.CodeVolume_of_V4d_Array(ref v); } },
            { typeof(List<Volume<V4d>>), (c,o) => { var v = (List<Volume<V4d>>)o; c.CodeList_of_Volume_of_V4d__(ref v); } },

            { typeof(Tensor<V4d>), (c,o) => { var v = (Tensor<V4d>)o; c.CodeTensor_of_V4d_(ref v); } },
            { typeof(Tensor<V4d>[]), (c,o) => { var v = (Tensor<V4d>[])o; c.CodeTensor_of_V4d_Array(ref v); } },
            { typeof(List<Tensor<V4d>>), (c,o) => { var v = (List<Tensor<V4d>>)o; c.CodeList_of_Tensor_of_V4d__(ref v); } },

            #endregion

            #region M22i

            { typeof(M22i), (c,o) => { var v = (M22i)o; c.CodeM22i(ref v); } },
            { typeof(M22i[]), (c,o) => { var v = (M22i[])o; c.CodeM22iArray(ref v); } },
            { typeof(List<M22i>), (c,o) => { var v = (List<M22i>)o; c.CodeList_of_M22i_(ref v); } },

            { typeof(Vector<M22i>), (c,o) => { var v = (Vector<M22i>)o; c.CodeVector_of_M22i_(ref v); } },
            { typeof(Vector<M22i>[]), (c,o) => { var v = (Vector<M22i>[])o; c.CodeVector_of_M22i_Array(ref v); } },
            { typeof(List<Vector<M22i>>), (c,o) => { var v = (List<Vector<M22i>>)o; c.CodeList_of_Vector_of_M22i__(ref v); } },

            { typeof(Matrix<M22i>), (c,o) => { var v = (Matrix<M22i>)o; c.CodeMatrix_of_M22i_(ref v); } },
            { typeof(Matrix<M22i>[]), (c,o) => { var v = (Matrix<M22i>[])o; c.CodeMatrix_of_M22i_Array(ref v); } },
            { typeof(List<Matrix<M22i>>), (c,o) => { var v = (List<Matrix<M22i>>)o; c.CodeList_of_Matrix_of_M22i__(ref v); } },

            { typeof(Volume<M22i>), (c,o) => { var v = (Volume<M22i>)o; c.CodeVolume_of_M22i_(ref v); } },
            { typeof(Volume<M22i>[]), (c,o) => { var v = (Volume<M22i>[])o; c.CodeVolume_of_M22i_Array(ref v); } },
            { typeof(List<Volume<M22i>>), (c,o) => { var v = (List<Volume<M22i>>)o; c.CodeList_of_Volume_of_M22i__(ref v); } },

            { typeof(Tensor<M22i>), (c,o) => { var v = (Tensor<M22i>)o; c.CodeTensor_of_M22i_(ref v); } },
            { typeof(Tensor<M22i>[]), (c,o) => { var v = (Tensor<M22i>[])o; c.CodeTensor_of_M22i_Array(ref v); } },
            { typeof(List<Tensor<M22i>>), (c,o) => { var v = (List<Tensor<M22i>>)o; c.CodeList_of_Tensor_of_M22i__(ref v); } },

            #endregion

            #region M22l

            { typeof(M22l), (c,o) => { var v = (M22l)o; c.CodeM22l(ref v); } },
            { typeof(M22l[]), (c,o) => { var v = (M22l[])o; c.CodeM22lArray(ref v); } },
            { typeof(List<M22l>), (c,o) => { var v = (List<M22l>)o; c.CodeList_of_M22l_(ref v); } },

            { typeof(Vector<M22l>), (c,o) => { var v = (Vector<M22l>)o; c.CodeVector_of_M22l_(ref v); } },
            { typeof(Vector<M22l>[]), (c,o) => { var v = (Vector<M22l>[])o; c.CodeVector_of_M22l_Array(ref v); } },
            { typeof(List<Vector<M22l>>), (c,o) => { var v = (List<Vector<M22l>>)o; c.CodeList_of_Vector_of_M22l__(ref v); } },

            { typeof(Matrix<M22l>), (c,o) => { var v = (Matrix<M22l>)o; c.CodeMatrix_of_M22l_(ref v); } },
            { typeof(Matrix<M22l>[]), (c,o) => { var v = (Matrix<M22l>[])o; c.CodeMatrix_of_M22l_Array(ref v); } },
            { typeof(List<Matrix<M22l>>), (c,o) => { var v = (List<Matrix<M22l>>)o; c.CodeList_of_Matrix_of_M22l__(ref v); } },

            { typeof(Volume<M22l>), (c,o) => { var v = (Volume<M22l>)o; c.CodeVolume_of_M22l_(ref v); } },
            { typeof(Volume<M22l>[]), (c,o) => { var v = (Volume<M22l>[])o; c.CodeVolume_of_M22l_Array(ref v); } },
            { typeof(List<Volume<M22l>>), (c,o) => { var v = (List<Volume<M22l>>)o; c.CodeList_of_Volume_of_M22l__(ref v); } },

            { typeof(Tensor<M22l>), (c,o) => { var v = (Tensor<M22l>)o; c.CodeTensor_of_M22l_(ref v); } },
            { typeof(Tensor<M22l>[]), (c,o) => { var v = (Tensor<M22l>[])o; c.CodeTensor_of_M22l_Array(ref v); } },
            { typeof(List<Tensor<M22l>>), (c,o) => { var v = (List<Tensor<M22l>>)o; c.CodeList_of_Tensor_of_M22l__(ref v); } },

            #endregion

            #region M22f

            { typeof(M22f), (c,o) => { var v = (M22f)o; c.CodeM22f(ref v); } },
            { typeof(M22f[]), (c,o) => { var v = (M22f[])o; c.CodeM22fArray(ref v); } },
            { typeof(List<M22f>), (c,o) => { var v = (List<M22f>)o; c.CodeList_of_M22f_(ref v); } },

            { typeof(Vector<M22f>), (c,o) => { var v = (Vector<M22f>)o; c.CodeVector_of_M22f_(ref v); } },
            { typeof(Vector<M22f>[]), (c,o) => { var v = (Vector<M22f>[])o; c.CodeVector_of_M22f_Array(ref v); } },
            { typeof(List<Vector<M22f>>), (c,o) => { var v = (List<Vector<M22f>>)o; c.CodeList_of_Vector_of_M22f__(ref v); } },

            { typeof(Matrix<M22f>), (c,o) => { var v = (Matrix<M22f>)o; c.CodeMatrix_of_M22f_(ref v); } },
            { typeof(Matrix<M22f>[]), (c,o) => { var v = (Matrix<M22f>[])o; c.CodeMatrix_of_M22f_Array(ref v); } },
            { typeof(List<Matrix<M22f>>), (c,o) => { var v = (List<Matrix<M22f>>)o; c.CodeList_of_Matrix_of_M22f__(ref v); } },

            { typeof(Volume<M22f>), (c,o) => { var v = (Volume<M22f>)o; c.CodeVolume_of_M22f_(ref v); } },
            { typeof(Volume<M22f>[]), (c,o) => { var v = (Volume<M22f>[])o; c.CodeVolume_of_M22f_Array(ref v); } },
            { typeof(List<Volume<M22f>>), (c,o) => { var v = (List<Volume<M22f>>)o; c.CodeList_of_Volume_of_M22f__(ref v); } },

            { typeof(Tensor<M22f>), (c,o) => { var v = (Tensor<M22f>)o; c.CodeTensor_of_M22f_(ref v); } },
            { typeof(Tensor<M22f>[]), (c,o) => { var v = (Tensor<M22f>[])o; c.CodeTensor_of_M22f_Array(ref v); } },
            { typeof(List<Tensor<M22f>>), (c,o) => { var v = (List<Tensor<M22f>>)o; c.CodeList_of_Tensor_of_M22f__(ref v); } },

            #endregion

            #region M22d

            { typeof(M22d), (c,o) => { var v = (M22d)o; c.CodeM22d(ref v); } },
            { typeof(M22d[]), (c,o) => { var v = (M22d[])o; c.CodeM22dArray(ref v); } },
            { typeof(List<M22d>), (c,o) => { var v = (List<M22d>)o; c.CodeList_of_M22d_(ref v); } },

            { typeof(Vector<M22d>), (c,o) => { var v = (Vector<M22d>)o; c.CodeVector_of_M22d_(ref v); } },
            { typeof(Vector<M22d>[]), (c,o) => { var v = (Vector<M22d>[])o; c.CodeVector_of_M22d_Array(ref v); } },
            { typeof(List<Vector<M22d>>), (c,o) => { var v = (List<Vector<M22d>>)o; c.CodeList_of_Vector_of_M22d__(ref v); } },

            { typeof(Matrix<M22d>), (c,o) => { var v = (Matrix<M22d>)o; c.CodeMatrix_of_M22d_(ref v); } },
            { typeof(Matrix<M22d>[]), (c,o) => { var v = (Matrix<M22d>[])o; c.CodeMatrix_of_M22d_Array(ref v); } },
            { typeof(List<Matrix<M22d>>), (c,o) => { var v = (List<Matrix<M22d>>)o; c.CodeList_of_Matrix_of_M22d__(ref v); } },

            { typeof(Volume<M22d>), (c,o) => { var v = (Volume<M22d>)o; c.CodeVolume_of_M22d_(ref v); } },
            { typeof(Volume<M22d>[]), (c,o) => { var v = (Volume<M22d>[])o; c.CodeVolume_of_M22d_Array(ref v); } },
            { typeof(List<Volume<M22d>>), (c,o) => { var v = (List<Volume<M22d>>)o; c.CodeList_of_Volume_of_M22d__(ref v); } },

            { typeof(Tensor<M22d>), (c,o) => { var v = (Tensor<M22d>)o; c.CodeTensor_of_M22d_(ref v); } },
            { typeof(Tensor<M22d>[]), (c,o) => { var v = (Tensor<M22d>[])o; c.CodeTensor_of_M22d_Array(ref v); } },
            { typeof(List<Tensor<M22d>>), (c,o) => { var v = (List<Tensor<M22d>>)o; c.CodeList_of_Tensor_of_M22d__(ref v); } },

            #endregion

            #region M23i

            { typeof(M23i), (c,o) => { var v = (M23i)o; c.CodeM23i(ref v); } },
            { typeof(M23i[]), (c,o) => { var v = (M23i[])o; c.CodeM23iArray(ref v); } },
            { typeof(List<M23i>), (c,o) => { var v = (List<M23i>)o; c.CodeList_of_M23i_(ref v); } },

            { typeof(Vector<M23i>), (c,o) => { var v = (Vector<M23i>)o; c.CodeVector_of_M23i_(ref v); } },
            { typeof(Vector<M23i>[]), (c,o) => { var v = (Vector<M23i>[])o; c.CodeVector_of_M23i_Array(ref v); } },
            { typeof(List<Vector<M23i>>), (c,o) => { var v = (List<Vector<M23i>>)o; c.CodeList_of_Vector_of_M23i__(ref v); } },

            { typeof(Matrix<M23i>), (c,o) => { var v = (Matrix<M23i>)o; c.CodeMatrix_of_M23i_(ref v); } },
            { typeof(Matrix<M23i>[]), (c,o) => { var v = (Matrix<M23i>[])o; c.CodeMatrix_of_M23i_Array(ref v); } },
            { typeof(List<Matrix<M23i>>), (c,o) => { var v = (List<Matrix<M23i>>)o; c.CodeList_of_Matrix_of_M23i__(ref v); } },

            { typeof(Volume<M23i>), (c,o) => { var v = (Volume<M23i>)o; c.CodeVolume_of_M23i_(ref v); } },
            { typeof(Volume<M23i>[]), (c,o) => { var v = (Volume<M23i>[])o; c.CodeVolume_of_M23i_Array(ref v); } },
            { typeof(List<Volume<M23i>>), (c,o) => { var v = (List<Volume<M23i>>)o; c.CodeList_of_Volume_of_M23i__(ref v); } },

            { typeof(Tensor<M23i>), (c,o) => { var v = (Tensor<M23i>)o; c.CodeTensor_of_M23i_(ref v); } },
            { typeof(Tensor<M23i>[]), (c,o) => { var v = (Tensor<M23i>[])o; c.CodeTensor_of_M23i_Array(ref v); } },
            { typeof(List<Tensor<M23i>>), (c,o) => { var v = (List<Tensor<M23i>>)o; c.CodeList_of_Tensor_of_M23i__(ref v); } },

            #endregion

            #region M23l

            { typeof(M23l), (c,o) => { var v = (M23l)o; c.CodeM23l(ref v); } },
            { typeof(M23l[]), (c,o) => { var v = (M23l[])o; c.CodeM23lArray(ref v); } },
            { typeof(List<M23l>), (c,o) => { var v = (List<M23l>)o; c.CodeList_of_M23l_(ref v); } },

            { typeof(Vector<M23l>), (c,o) => { var v = (Vector<M23l>)o; c.CodeVector_of_M23l_(ref v); } },
            { typeof(Vector<M23l>[]), (c,o) => { var v = (Vector<M23l>[])o; c.CodeVector_of_M23l_Array(ref v); } },
            { typeof(List<Vector<M23l>>), (c,o) => { var v = (List<Vector<M23l>>)o; c.CodeList_of_Vector_of_M23l__(ref v); } },

            { typeof(Matrix<M23l>), (c,o) => { var v = (Matrix<M23l>)o; c.CodeMatrix_of_M23l_(ref v); } },
            { typeof(Matrix<M23l>[]), (c,o) => { var v = (Matrix<M23l>[])o; c.CodeMatrix_of_M23l_Array(ref v); } },
            { typeof(List<Matrix<M23l>>), (c,o) => { var v = (List<Matrix<M23l>>)o; c.CodeList_of_Matrix_of_M23l__(ref v); } },

            { typeof(Volume<M23l>), (c,o) => { var v = (Volume<M23l>)o; c.CodeVolume_of_M23l_(ref v); } },
            { typeof(Volume<M23l>[]), (c,o) => { var v = (Volume<M23l>[])o; c.CodeVolume_of_M23l_Array(ref v); } },
            { typeof(List<Volume<M23l>>), (c,o) => { var v = (List<Volume<M23l>>)o; c.CodeList_of_Volume_of_M23l__(ref v); } },

            { typeof(Tensor<M23l>), (c,o) => { var v = (Tensor<M23l>)o; c.CodeTensor_of_M23l_(ref v); } },
            { typeof(Tensor<M23l>[]), (c,o) => { var v = (Tensor<M23l>[])o; c.CodeTensor_of_M23l_Array(ref v); } },
            { typeof(List<Tensor<M23l>>), (c,o) => { var v = (List<Tensor<M23l>>)o; c.CodeList_of_Tensor_of_M23l__(ref v); } },

            #endregion

            #region M23f

            { typeof(M23f), (c,o) => { var v = (M23f)o; c.CodeM23f(ref v); } },
            { typeof(M23f[]), (c,o) => { var v = (M23f[])o; c.CodeM23fArray(ref v); } },
            { typeof(List<M23f>), (c,o) => { var v = (List<M23f>)o; c.CodeList_of_M23f_(ref v); } },

            { typeof(Vector<M23f>), (c,o) => { var v = (Vector<M23f>)o; c.CodeVector_of_M23f_(ref v); } },
            { typeof(Vector<M23f>[]), (c,o) => { var v = (Vector<M23f>[])o; c.CodeVector_of_M23f_Array(ref v); } },
            { typeof(List<Vector<M23f>>), (c,o) => { var v = (List<Vector<M23f>>)o; c.CodeList_of_Vector_of_M23f__(ref v); } },

            { typeof(Matrix<M23f>), (c,o) => { var v = (Matrix<M23f>)o; c.CodeMatrix_of_M23f_(ref v); } },
            { typeof(Matrix<M23f>[]), (c,o) => { var v = (Matrix<M23f>[])o; c.CodeMatrix_of_M23f_Array(ref v); } },
            { typeof(List<Matrix<M23f>>), (c,o) => { var v = (List<Matrix<M23f>>)o; c.CodeList_of_Matrix_of_M23f__(ref v); } },

            { typeof(Volume<M23f>), (c,o) => { var v = (Volume<M23f>)o; c.CodeVolume_of_M23f_(ref v); } },
            { typeof(Volume<M23f>[]), (c,o) => { var v = (Volume<M23f>[])o; c.CodeVolume_of_M23f_Array(ref v); } },
            { typeof(List<Volume<M23f>>), (c,o) => { var v = (List<Volume<M23f>>)o; c.CodeList_of_Volume_of_M23f__(ref v); } },

            { typeof(Tensor<M23f>), (c,o) => { var v = (Tensor<M23f>)o; c.CodeTensor_of_M23f_(ref v); } },
            { typeof(Tensor<M23f>[]), (c,o) => { var v = (Tensor<M23f>[])o; c.CodeTensor_of_M23f_Array(ref v); } },
            { typeof(List<Tensor<M23f>>), (c,o) => { var v = (List<Tensor<M23f>>)o; c.CodeList_of_Tensor_of_M23f__(ref v); } },

            #endregion

            #region M23d

            { typeof(M23d), (c,o) => { var v = (M23d)o; c.CodeM23d(ref v); } },
            { typeof(M23d[]), (c,o) => { var v = (M23d[])o; c.CodeM23dArray(ref v); } },
            { typeof(List<M23d>), (c,o) => { var v = (List<M23d>)o; c.CodeList_of_M23d_(ref v); } },

            { typeof(Vector<M23d>), (c,o) => { var v = (Vector<M23d>)o; c.CodeVector_of_M23d_(ref v); } },
            { typeof(Vector<M23d>[]), (c,o) => { var v = (Vector<M23d>[])o; c.CodeVector_of_M23d_Array(ref v); } },
            { typeof(List<Vector<M23d>>), (c,o) => { var v = (List<Vector<M23d>>)o; c.CodeList_of_Vector_of_M23d__(ref v); } },

            { typeof(Matrix<M23d>), (c,o) => { var v = (Matrix<M23d>)o; c.CodeMatrix_of_M23d_(ref v); } },
            { typeof(Matrix<M23d>[]), (c,o) => { var v = (Matrix<M23d>[])o; c.CodeMatrix_of_M23d_Array(ref v); } },
            { typeof(List<Matrix<M23d>>), (c,o) => { var v = (List<Matrix<M23d>>)o; c.CodeList_of_Matrix_of_M23d__(ref v); } },

            { typeof(Volume<M23d>), (c,o) => { var v = (Volume<M23d>)o; c.CodeVolume_of_M23d_(ref v); } },
            { typeof(Volume<M23d>[]), (c,o) => { var v = (Volume<M23d>[])o; c.CodeVolume_of_M23d_Array(ref v); } },
            { typeof(List<Volume<M23d>>), (c,o) => { var v = (List<Volume<M23d>>)o; c.CodeList_of_Volume_of_M23d__(ref v); } },

            { typeof(Tensor<M23d>), (c,o) => { var v = (Tensor<M23d>)o; c.CodeTensor_of_M23d_(ref v); } },
            { typeof(Tensor<M23d>[]), (c,o) => { var v = (Tensor<M23d>[])o; c.CodeTensor_of_M23d_Array(ref v); } },
            { typeof(List<Tensor<M23d>>), (c,o) => { var v = (List<Tensor<M23d>>)o; c.CodeList_of_Tensor_of_M23d__(ref v); } },

            #endregion

            #region M33i

            { typeof(M33i), (c,o) => { var v = (M33i)o; c.CodeM33i(ref v); } },
            { typeof(M33i[]), (c,o) => { var v = (M33i[])o; c.CodeM33iArray(ref v); } },
            { typeof(List<M33i>), (c,o) => { var v = (List<M33i>)o; c.CodeList_of_M33i_(ref v); } },

            { typeof(Vector<M33i>), (c,o) => { var v = (Vector<M33i>)o; c.CodeVector_of_M33i_(ref v); } },
            { typeof(Vector<M33i>[]), (c,o) => { var v = (Vector<M33i>[])o; c.CodeVector_of_M33i_Array(ref v); } },
            { typeof(List<Vector<M33i>>), (c,o) => { var v = (List<Vector<M33i>>)o; c.CodeList_of_Vector_of_M33i__(ref v); } },

            { typeof(Matrix<M33i>), (c,o) => { var v = (Matrix<M33i>)o; c.CodeMatrix_of_M33i_(ref v); } },
            { typeof(Matrix<M33i>[]), (c,o) => { var v = (Matrix<M33i>[])o; c.CodeMatrix_of_M33i_Array(ref v); } },
            { typeof(List<Matrix<M33i>>), (c,o) => { var v = (List<Matrix<M33i>>)o; c.CodeList_of_Matrix_of_M33i__(ref v); } },

            { typeof(Volume<M33i>), (c,o) => { var v = (Volume<M33i>)o; c.CodeVolume_of_M33i_(ref v); } },
            { typeof(Volume<M33i>[]), (c,o) => { var v = (Volume<M33i>[])o; c.CodeVolume_of_M33i_Array(ref v); } },
            { typeof(List<Volume<M33i>>), (c,o) => { var v = (List<Volume<M33i>>)o; c.CodeList_of_Volume_of_M33i__(ref v); } },

            { typeof(Tensor<M33i>), (c,o) => { var v = (Tensor<M33i>)o; c.CodeTensor_of_M33i_(ref v); } },
            { typeof(Tensor<M33i>[]), (c,o) => { var v = (Tensor<M33i>[])o; c.CodeTensor_of_M33i_Array(ref v); } },
            { typeof(List<Tensor<M33i>>), (c,o) => { var v = (List<Tensor<M33i>>)o; c.CodeList_of_Tensor_of_M33i__(ref v); } },

            #endregion

            #region M33l

            { typeof(M33l), (c,o) => { var v = (M33l)o; c.CodeM33l(ref v); } },
            { typeof(M33l[]), (c,o) => { var v = (M33l[])o; c.CodeM33lArray(ref v); } },
            { typeof(List<M33l>), (c,o) => { var v = (List<M33l>)o; c.CodeList_of_M33l_(ref v); } },

            { typeof(Vector<M33l>), (c,o) => { var v = (Vector<M33l>)o; c.CodeVector_of_M33l_(ref v); } },
            { typeof(Vector<M33l>[]), (c,o) => { var v = (Vector<M33l>[])o; c.CodeVector_of_M33l_Array(ref v); } },
            { typeof(List<Vector<M33l>>), (c,o) => { var v = (List<Vector<M33l>>)o; c.CodeList_of_Vector_of_M33l__(ref v); } },

            { typeof(Matrix<M33l>), (c,o) => { var v = (Matrix<M33l>)o; c.CodeMatrix_of_M33l_(ref v); } },
            { typeof(Matrix<M33l>[]), (c,o) => { var v = (Matrix<M33l>[])o; c.CodeMatrix_of_M33l_Array(ref v); } },
            { typeof(List<Matrix<M33l>>), (c,o) => { var v = (List<Matrix<M33l>>)o; c.CodeList_of_Matrix_of_M33l__(ref v); } },

            { typeof(Volume<M33l>), (c,o) => { var v = (Volume<M33l>)o; c.CodeVolume_of_M33l_(ref v); } },
            { typeof(Volume<M33l>[]), (c,o) => { var v = (Volume<M33l>[])o; c.CodeVolume_of_M33l_Array(ref v); } },
            { typeof(List<Volume<M33l>>), (c,o) => { var v = (List<Volume<M33l>>)o; c.CodeList_of_Volume_of_M33l__(ref v); } },

            { typeof(Tensor<M33l>), (c,o) => { var v = (Tensor<M33l>)o; c.CodeTensor_of_M33l_(ref v); } },
            { typeof(Tensor<M33l>[]), (c,o) => { var v = (Tensor<M33l>[])o; c.CodeTensor_of_M33l_Array(ref v); } },
            { typeof(List<Tensor<M33l>>), (c,o) => { var v = (List<Tensor<M33l>>)o; c.CodeList_of_Tensor_of_M33l__(ref v); } },

            #endregion

            #region M33f

            { typeof(M33f), (c,o) => { var v = (M33f)o; c.CodeM33f(ref v); } },
            { typeof(M33f[]), (c,o) => { var v = (M33f[])o; c.CodeM33fArray(ref v); } },
            { typeof(List<M33f>), (c,o) => { var v = (List<M33f>)o; c.CodeList_of_M33f_(ref v); } },

            { typeof(Vector<M33f>), (c,o) => { var v = (Vector<M33f>)o; c.CodeVector_of_M33f_(ref v); } },
            { typeof(Vector<M33f>[]), (c,o) => { var v = (Vector<M33f>[])o; c.CodeVector_of_M33f_Array(ref v); } },
            { typeof(List<Vector<M33f>>), (c,o) => { var v = (List<Vector<M33f>>)o; c.CodeList_of_Vector_of_M33f__(ref v); } },

            { typeof(Matrix<M33f>), (c,o) => { var v = (Matrix<M33f>)o; c.CodeMatrix_of_M33f_(ref v); } },
            { typeof(Matrix<M33f>[]), (c,o) => { var v = (Matrix<M33f>[])o; c.CodeMatrix_of_M33f_Array(ref v); } },
            { typeof(List<Matrix<M33f>>), (c,o) => { var v = (List<Matrix<M33f>>)o; c.CodeList_of_Matrix_of_M33f__(ref v); } },

            { typeof(Volume<M33f>), (c,o) => { var v = (Volume<M33f>)o; c.CodeVolume_of_M33f_(ref v); } },
            { typeof(Volume<M33f>[]), (c,o) => { var v = (Volume<M33f>[])o; c.CodeVolume_of_M33f_Array(ref v); } },
            { typeof(List<Volume<M33f>>), (c,o) => { var v = (List<Volume<M33f>>)o; c.CodeList_of_Volume_of_M33f__(ref v); } },

            { typeof(Tensor<M33f>), (c,o) => { var v = (Tensor<M33f>)o; c.CodeTensor_of_M33f_(ref v); } },
            { typeof(Tensor<M33f>[]), (c,o) => { var v = (Tensor<M33f>[])o; c.CodeTensor_of_M33f_Array(ref v); } },
            { typeof(List<Tensor<M33f>>), (c,o) => { var v = (List<Tensor<M33f>>)o; c.CodeList_of_Tensor_of_M33f__(ref v); } },

            #endregion

            #region M33d

            { typeof(M33d), (c,o) => { var v = (M33d)o; c.CodeM33d(ref v); } },
            { typeof(M33d[]), (c,o) => { var v = (M33d[])o; c.CodeM33dArray(ref v); } },
            { typeof(List<M33d>), (c,o) => { var v = (List<M33d>)o; c.CodeList_of_M33d_(ref v); } },

            { typeof(Vector<M33d>), (c,o) => { var v = (Vector<M33d>)o; c.CodeVector_of_M33d_(ref v); } },
            { typeof(Vector<M33d>[]), (c,o) => { var v = (Vector<M33d>[])o; c.CodeVector_of_M33d_Array(ref v); } },
            { typeof(List<Vector<M33d>>), (c,o) => { var v = (List<Vector<M33d>>)o; c.CodeList_of_Vector_of_M33d__(ref v); } },

            { typeof(Matrix<M33d>), (c,o) => { var v = (Matrix<M33d>)o; c.CodeMatrix_of_M33d_(ref v); } },
            { typeof(Matrix<M33d>[]), (c,o) => { var v = (Matrix<M33d>[])o; c.CodeMatrix_of_M33d_Array(ref v); } },
            { typeof(List<Matrix<M33d>>), (c,o) => { var v = (List<Matrix<M33d>>)o; c.CodeList_of_Matrix_of_M33d__(ref v); } },

            { typeof(Volume<M33d>), (c,o) => { var v = (Volume<M33d>)o; c.CodeVolume_of_M33d_(ref v); } },
            { typeof(Volume<M33d>[]), (c,o) => { var v = (Volume<M33d>[])o; c.CodeVolume_of_M33d_Array(ref v); } },
            { typeof(List<Volume<M33d>>), (c,o) => { var v = (List<Volume<M33d>>)o; c.CodeList_of_Volume_of_M33d__(ref v); } },

            { typeof(Tensor<M33d>), (c,o) => { var v = (Tensor<M33d>)o; c.CodeTensor_of_M33d_(ref v); } },
            { typeof(Tensor<M33d>[]), (c,o) => { var v = (Tensor<M33d>[])o; c.CodeTensor_of_M33d_Array(ref v); } },
            { typeof(List<Tensor<M33d>>), (c,o) => { var v = (List<Tensor<M33d>>)o; c.CodeList_of_Tensor_of_M33d__(ref v); } },

            #endregion

            #region M34i

            { typeof(M34i), (c,o) => { var v = (M34i)o; c.CodeM34i(ref v); } },
            { typeof(M34i[]), (c,o) => { var v = (M34i[])o; c.CodeM34iArray(ref v); } },
            { typeof(List<M34i>), (c,o) => { var v = (List<M34i>)o; c.CodeList_of_M34i_(ref v); } },

            { typeof(Vector<M34i>), (c,o) => { var v = (Vector<M34i>)o; c.CodeVector_of_M34i_(ref v); } },
            { typeof(Vector<M34i>[]), (c,o) => { var v = (Vector<M34i>[])o; c.CodeVector_of_M34i_Array(ref v); } },
            { typeof(List<Vector<M34i>>), (c,o) => { var v = (List<Vector<M34i>>)o; c.CodeList_of_Vector_of_M34i__(ref v); } },

            { typeof(Matrix<M34i>), (c,o) => { var v = (Matrix<M34i>)o; c.CodeMatrix_of_M34i_(ref v); } },
            { typeof(Matrix<M34i>[]), (c,o) => { var v = (Matrix<M34i>[])o; c.CodeMatrix_of_M34i_Array(ref v); } },
            { typeof(List<Matrix<M34i>>), (c,o) => { var v = (List<Matrix<M34i>>)o; c.CodeList_of_Matrix_of_M34i__(ref v); } },

            { typeof(Volume<M34i>), (c,o) => { var v = (Volume<M34i>)o; c.CodeVolume_of_M34i_(ref v); } },
            { typeof(Volume<M34i>[]), (c,o) => { var v = (Volume<M34i>[])o; c.CodeVolume_of_M34i_Array(ref v); } },
            { typeof(List<Volume<M34i>>), (c,o) => { var v = (List<Volume<M34i>>)o; c.CodeList_of_Volume_of_M34i__(ref v); } },

            { typeof(Tensor<M34i>), (c,o) => { var v = (Tensor<M34i>)o; c.CodeTensor_of_M34i_(ref v); } },
            { typeof(Tensor<M34i>[]), (c,o) => { var v = (Tensor<M34i>[])o; c.CodeTensor_of_M34i_Array(ref v); } },
            { typeof(List<Tensor<M34i>>), (c,o) => { var v = (List<Tensor<M34i>>)o; c.CodeList_of_Tensor_of_M34i__(ref v); } },

            #endregion

            #region M34l

            { typeof(M34l), (c,o) => { var v = (M34l)o; c.CodeM34l(ref v); } },
            { typeof(M34l[]), (c,o) => { var v = (M34l[])o; c.CodeM34lArray(ref v); } },
            { typeof(List<M34l>), (c,o) => { var v = (List<M34l>)o; c.CodeList_of_M34l_(ref v); } },

            { typeof(Vector<M34l>), (c,o) => { var v = (Vector<M34l>)o; c.CodeVector_of_M34l_(ref v); } },
            { typeof(Vector<M34l>[]), (c,o) => { var v = (Vector<M34l>[])o; c.CodeVector_of_M34l_Array(ref v); } },
            { typeof(List<Vector<M34l>>), (c,o) => { var v = (List<Vector<M34l>>)o; c.CodeList_of_Vector_of_M34l__(ref v); } },

            { typeof(Matrix<M34l>), (c,o) => { var v = (Matrix<M34l>)o; c.CodeMatrix_of_M34l_(ref v); } },
            { typeof(Matrix<M34l>[]), (c,o) => { var v = (Matrix<M34l>[])o; c.CodeMatrix_of_M34l_Array(ref v); } },
            { typeof(List<Matrix<M34l>>), (c,o) => { var v = (List<Matrix<M34l>>)o; c.CodeList_of_Matrix_of_M34l__(ref v); } },

            { typeof(Volume<M34l>), (c,o) => { var v = (Volume<M34l>)o; c.CodeVolume_of_M34l_(ref v); } },
            { typeof(Volume<M34l>[]), (c,o) => { var v = (Volume<M34l>[])o; c.CodeVolume_of_M34l_Array(ref v); } },
            { typeof(List<Volume<M34l>>), (c,o) => { var v = (List<Volume<M34l>>)o; c.CodeList_of_Volume_of_M34l__(ref v); } },

            { typeof(Tensor<M34l>), (c,o) => { var v = (Tensor<M34l>)o; c.CodeTensor_of_M34l_(ref v); } },
            { typeof(Tensor<M34l>[]), (c,o) => { var v = (Tensor<M34l>[])o; c.CodeTensor_of_M34l_Array(ref v); } },
            { typeof(List<Tensor<M34l>>), (c,o) => { var v = (List<Tensor<M34l>>)o; c.CodeList_of_Tensor_of_M34l__(ref v); } },

            #endregion

            #region M34f

            { typeof(M34f), (c,o) => { var v = (M34f)o; c.CodeM34f(ref v); } },
            { typeof(M34f[]), (c,o) => { var v = (M34f[])o; c.CodeM34fArray(ref v); } },
            { typeof(List<M34f>), (c,o) => { var v = (List<M34f>)o; c.CodeList_of_M34f_(ref v); } },

            { typeof(Vector<M34f>), (c,o) => { var v = (Vector<M34f>)o; c.CodeVector_of_M34f_(ref v); } },
            { typeof(Vector<M34f>[]), (c,o) => { var v = (Vector<M34f>[])o; c.CodeVector_of_M34f_Array(ref v); } },
            { typeof(List<Vector<M34f>>), (c,o) => { var v = (List<Vector<M34f>>)o; c.CodeList_of_Vector_of_M34f__(ref v); } },

            { typeof(Matrix<M34f>), (c,o) => { var v = (Matrix<M34f>)o; c.CodeMatrix_of_M34f_(ref v); } },
            { typeof(Matrix<M34f>[]), (c,o) => { var v = (Matrix<M34f>[])o; c.CodeMatrix_of_M34f_Array(ref v); } },
            { typeof(List<Matrix<M34f>>), (c,o) => { var v = (List<Matrix<M34f>>)o; c.CodeList_of_Matrix_of_M34f__(ref v); } },

            { typeof(Volume<M34f>), (c,o) => { var v = (Volume<M34f>)o; c.CodeVolume_of_M34f_(ref v); } },
            { typeof(Volume<M34f>[]), (c,o) => { var v = (Volume<M34f>[])o; c.CodeVolume_of_M34f_Array(ref v); } },
            { typeof(List<Volume<M34f>>), (c,o) => { var v = (List<Volume<M34f>>)o; c.CodeList_of_Volume_of_M34f__(ref v); } },

            { typeof(Tensor<M34f>), (c,o) => { var v = (Tensor<M34f>)o; c.CodeTensor_of_M34f_(ref v); } },
            { typeof(Tensor<M34f>[]), (c,o) => { var v = (Tensor<M34f>[])o; c.CodeTensor_of_M34f_Array(ref v); } },
            { typeof(List<Tensor<M34f>>), (c,o) => { var v = (List<Tensor<M34f>>)o; c.CodeList_of_Tensor_of_M34f__(ref v); } },

            #endregion

            #region M34d

            { typeof(M34d), (c,o) => { var v = (M34d)o; c.CodeM34d(ref v); } },
            { typeof(M34d[]), (c,o) => { var v = (M34d[])o; c.CodeM34dArray(ref v); } },
            { typeof(List<M34d>), (c,o) => { var v = (List<M34d>)o; c.CodeList_of_M34d_(ref v); } },

            { typeof(Vector<M34d>), (c,o) => { var v = (Vector<M34d>)o; c.CodeVector_of_M34d_(ref v); } },
            { typeof(Vector<M34d>[]), (c,o) => { var v = (Vector<M34d>[])o; c.CodeVector_of_M34d_Array(ref v); } },
            { typeof(List<Vector<M34d>>), (c,o) => { var v = (List<Vector<M34d>>)o; c.CodeList_of_Vector_of_M34d__(ref v); } },

            { typeof(Matrix<M34d>), (c,o) => { var v = (Matrix<M34d>)o; c.CodeMatrix_of_M34d_(ref v); } },
            { typeof(Matrix<M34d>[]), (c,o) => { var v = (Matrix<M34d>[])o; c.CodeMatrix_of_M34d_Array(ref v); } },
            { typeof(List<Matrix<M34d>>), (c,o) => { var v = (List<Matrix<M34d>>)o; c.CodeList_of_Matrix_of_M34d__(ref v); } },

            { typeof(Volume<M34d>), (c,o) => { var v = (Volume<M34d>)o; c.CodeVolume_of_M34d_(ref v); } },
            { typeof(Volume<M34d>[]), (c,o) => { var v = (Volume<M34d>[])o; c.CodeVolume_of_M34d_Array(ref v); } },
            { typeof(List<Volume<M34d>>), (c,o) => { var v = (List<Volume<M34d>>)o; c.CodeList_of_Volume_of_M34d__(ref v); } },

            { typeof(Tensor<M34d>), (c,o) => { var v = (Tensor<M34d>)o; c.CodeTensor_of_M34d_(ref v); } },
            { typeof(Tensor<M34d>[]), (c,o) => { var v = (Tensor<M34d>[])o; c.CodeTensor_of_M34d_Array(ref v); } },
            { typeof(List<Tensor<M34d>>), (c,o) => { var v = (List<Tensor<M34d>>)o; c.CodeList_of_Tensor_of_M34d__(ref v); } },

            #endregion

            #region M44i

            { typeof(M44i), (c,o) => { var v = (M44i)o; c.CodeM44i(ref v); } },
            { typeof(M44i[]), (c,o) => { var v = (M44i[])o; c.CodeM44iArray(ref v); } },
            { typeof(List<M44i>), (c,o) => { var v = (List<M44i>)o; c.CodeList_of_M44i_(ref v); } },

            { typeof(Vector<M44i>), (c,o) => { var v = (Vector<M44i>)o; c.CodeVector_of_M44i_(ref v); } },
            { typeof(Vector<M44i>[]), (c,o) => { var v = (Vector<M44i>[])o; c.CodeVector_of_M44i_Array(ref v); } },
            { typeof(List<Vector<M44i>>), (c,o) => { var v = (List<Vector<M44i>>)o; c.CodeList_of_Vector_of_M44i__(ref v); } },

            { typeof(Matrix<M44i>), (c,o) => { var v = (Matrix<M44i>)o; c.CodeMatrix_of_M44i_(ref v); } },
            { typeof(Matrix<M44i>[]), (c,o) => { var v = (Matrix<M44i>[])o; c.CodeMatrix_of_M44i_Array(ref v); } },
            { typeof(List<Matrix<M44i>>), (c,o) => { var v = (List<Matrix<M44i>>)o; c.CodeList_of_Matrix_of_M44i__(ref v); } },

            { typeof(Volume<M44i>), (c,o) => { var v = (Volume<M44i>)o; c.CodeVolume_of_M44i_(ref v); } },
            { typeof(Volume<M44i>[]), (c,o) => { var v = (Volume<M44i>[])o; c.CodeVolume_of_M44i_Array(ref v); } },
            { typeof(List<Volume<M44i>>), (c,o) => { var v = (List<Volume<M44i>>)o; c.CodeList_of_Volume_of_M44i__(ref v); } },

            { typeof(Tensor<M44i>), (c,o) => { var v = (Tensor<M44i>)o; c.CodeTensor_of_M44i_(ref v); } },
            { typeof(Tensor<M44i>[]), (c,o) => { var v = (Tensor<M44i>[])o; c.CodeTensor_of_M44i_Array(ref v); } },
            { typeof(List<Tensor<M44i>>), (c,o) => { var v = (List<Tensor<M44i>>)o; c.CodeList_of_Tensor_of_M44i__(ref v); } },

            #endregion

            #region M44l

            { typeof(M44l), (c,o) => { var v = (M44l)o; c.CodeM44l(ref v); } },
            { typeof(M44l[]), (c,o) => { var v = (M44l[])o; c.CodeM44lArray(ref v); } },
            { typeof(List<M44l>), (c,o) => { var v = (List<M44l>)o; c.CodeList_of_M44l_(ref v); } },

            { typeof(Vector<M44l>), (c,o) => { var v = (Vector<M44l>)o; c.CodeVector_of_M44l_(ref v); } },
            { typeof(Vector<M44l>[]), (c,o) => { var v = (Vector<M44l>[])o; c.CodeVector_of_M44l_Array(ref v); } },
            { typeof(List<Vector<M44l>>), (c,o) => { var v = (List<Vector<M44l>>)o; c.CodeList_of_Vector_of_M44l__(ref v); } },

            { typeof(Matrix<M44l>), (c,o) => { var v = (Matrix<M44l>)o; c.CodeMatrix_of_M44l_(ref v); } },
            { typeof(Matrix<M44l>[]), (c,o) => { var v = (Matrix<M44l>[])o; c.CodeMatrix_of_M44l_Array(ref v); } },
            { typeof(List<Matrix<M44l>>), (c,o) => { var v = (List<Matrix<M44l>>)o; c.CodeList_of_Matrix_of_M44l__(ref v); } },

            { typeof(Volume<M44l>), (c,o) => { var v = (Volume<M44l>)o; c.CodeVolume_of_M44l_(ref v); } },
            { typeof(Volume<M44l>[]), (c,o) => { var v = (Volume<M44l>[])o; c.CodeVolume_of_M44l_Array(ref v); } },
            { typeof(List<Volume<M44l>>), (c,o) => { var v = (List<Volume<M44l>>)o; c.CodeList_of_Volume_of_M44l__(ref v); } },

            { typeof(Tensor<M44l>), (c,o) => { var v = (Tensor<M44l>)o; c.CodeTensor_of_M44l_(ref v); } },
            { typeof(Tensor<M44l>[]), (c,o) => { var v = (Tensor<M44l>[])o; c.CodeTensor_of_M44l_Array(ref v); } },
            { typeof(List<Tensor<M44l>>), (c,o) => { var v = (List<Tensor<M44l>>)o; c.CodeList_of_Tensor_of_M44l__(ref v); } },

            #endregion

            #region M44f

            { typeof(M44f), (c,o) => { var v = (M44f)o; c.CodeM44f(ref v); } },
            { typeof(M44f[]), (c,o) => { var v = (M44f[])o; c.CodeM44fArray(ref v); } },
            { typeof(List<M44f>), (c,o) => { var v = (List<M44f>)o; c.CodeList_of_M44f_(ref v); } },

            { typeof(Vector<M44f>), (c,o) => { var v = (Vector<M44f>)o; c.CodeVector_of_M44f_(ref v); } },
            { typeof(Vector<M44f>[]), (c,o) => { var v = (Vector<M44f>[])o; c.CodeVector_of_M44f_Array(ref v); } },
            { typeof(List<Vector<M44f>>), (c,o) => { var v = (List<Vector<M44f>>)o; c.CodeList_of_Vector_of_M44f__(ref v); } },

            { typeof(Matrix<M44f>), (c,o) => { var v = (Matrix<M44f>)o; c.CodeMatrix_of_M44f_(ref v); } },
            { typeof(Matrix<M44f>[]), (c,o) => { var v = (Matrix<M44f>[])o; c.CodeMatrix_of_M44f_Array(ref v); } },
            { typeof(List<Matrix<M44f>>), (c,o) => { var v = (List<Matrix<M44f>>)o; c.CodeList_of_Matrix_of_M44f__(ref v); } },

            { typeof(Volume<M44f>), (c,o) => { var v = (Volume<M44f>)o; c.CodeVolume_of_M44f_(ref v); } },
            { typeof(Volume<M44f>[]), (c,o) => { var v = (Volume<M44f>[])o; c.CodeVolume_of_M44f_Array(ref v); } },
            { typeof(List<Volume<M44f>>), (c,o) => { var v = (List<Volume<M44f>>)o; c.CodeList_of_Volume_of_M44f__(ref v); } },

            { typeof(Tensor<M44f>), (c,o) => { var v = (Tensor<M44f>)o; c.CodeTensor_of_M44f_(ref v); } },
            { typeof(Tensor<M44f>[]), (c,o) => { var v = (Tensor<M44f>[])o; c.CodeTensor_of_M44f_Array(ref v); } },
            { typeof(List<Tensor<M44f>>), (c,o) => { var v = (List<Tensor<M44f>>)o; c.CodeList_of_Tensor_of_M44f__(ref v); } },

            #endregion

            #region M44d

            { typeof(M44d), (c,o) => { var v = (M44d)o; c.CodeM44d(ref v); } },
            { typeof(M44d[]), (c,o) => { var v = (M44d[])o; c.CodeM44dArray(ref v); } },
            { typeof(List<M44d>), (c,o) => { var v = (List<M44d>)o; c.CodeList_of_M44d_(ref v); } },

            { typeof(Vector<M44d>), (c,o) => { var v = (Vector<M44d>)o; c.CodeVector_of_M44d_(ref v); } },
            { typeof(Vector<M44d>[]), (c,o) => { var v = (Vector<M44d>[])o; c.CodeVector_of_M44d_Array(ref v); } },
            { typeof(List<Vector<M44d>>), (c,o) => { var v = (List<Vector<M44d>>)o; c.CodeList_of_Vector_of_M44d__(ref v); } },

            { typeof(Matrix<M44d>), (c,o) => { var v = (Matrix<M44d>)o; c.CodeMatrix_of_M44d_(ref v); } },
            { typeof(Matrix<M44d>[]), (c,o) => { var v = (Matrix<M44d>[])o; c.CodeMatrix_of_M44d_Array(ref v); } },
            { typeof(List<Matrix<M44d>>), (c,o) => { var v = (List<Matrix<M44d>>)o; c.CodeList_of_Matrix_of_M44d__(ref v); } },

            { typeof(Volume<M44d>), (c,o) => { var v = (Volume<M44d>)o; c.CodeVolume_of_M44d_(ref v); } },
            { typeof(Volume<M44d>[]), (c,o) => { var v = (Volume<M44d>[])o; c.CodeVolume_of_M44d_Array(ref v); } },
            { typeof(List<Volume<M44d>>), (c,o) => { var v = (List<Volume<M44d>>)o; c.CodeList_of_Volume_of_M44d__(ref v); } },

            { typeof(Tensor<M44d>), (c,o) => { var v = (Tensor<M44d>)o; c.CodeTensor_of_M44d_(ref v); } },
            { typeof(Tensor<M44d>[]), (c,o) => { var v = (Tensor<M44d>[])o; c.CodeTensor_of_M44d_Array(ref v); } },
            { typeof(List<Tensor<M44d>>), (c,o) => { var v = (List<Tensor<M44d>>)o; c.CodeList_of_Tensor_of_M44d__(ref v); } },

            #endregion

            #region C3b

            { typeof(C3b), (c,o) => { var v = (C3b)o; c.CodeC3b(ref v); } },
            { typeof(C3b[]), (c,o) => { var v = (C3b[])o; c.CodeC3bArray(ref v); } },
            { typeof(List<C3b>), (c,o) => { var v = (List<C3b>)o; c.CodeList_of_C3b_(ref v); } },

            { typeof(Vector<C3b>), (c,o) => { var v = (Vector<C3b>)o; c.CodeVector_of_C3b_(ref v); } },
            { typeof(Vector<C3b>[]), (c,o) => { var v = (Vector<C3b>[])o; c.CodeVector_of_C3b_Array(ref v); } },
            { typeof(List<Vector<C3b>>), (c,o) => { var v = (List<Vector<C3b>>)o; c.CodeList_of_Vector_of_C3b__(ref v); } },

            { typeof(Matrix<C3b>), (c,o) => { var v = (Matrix<C3b>)o; c.CodeMatrix_of_C3b_(ref v); } },
            { typeof(Matrix<C3b>[]), (c,o) => { var v = (Matrix<C3b>[])o; c.CodeMatrix_of_C3b_Array(ref v); } },
            { typeof(List<Matrix<C3b>>), (c,o) => { var v = (List<Matrix<C3b>>)o; c.CodeList_of_Matrix_of_C3b__(ref v); } },

            { typeof(Volume<C3b>), (c,o) => { var v = (Volume<C3b>)o; c.CodeVolume_of_C3b_(ref v); } },
            { typeof(Volume<C3b>[]), (c,o) => { var v = (Volume<C3b>[])o; c.CodeVolume_of_C3b_Array(ref v); } },
            { typeof(List<Volume<C3b>>), (c,o) => { var v = (List<Volume<C3b>>)o; c.CodeList_of_Volume_of_C3b__(ref v); } },

            { typeof(Tensor<C3b>), (c,o) => { var v = (Tensor<C3b>)o; c.CodeTensor_of_C3b_(ref v); } },
            { typeof(Tensor<C3b>[]), (c,o) => { var v = (Tensor<C3b>[])o; c.CodeTensor_of_C3b_Array(ref v); } },
            { typeof(List<Tensor<C3b>>), (c,o) => { var v = (List<Tensor<C3b>>)o; c.CodeList_of_Tensor_of_C3b__(ref v); } },

            #endregion

            #region C3us

            { typeof(C3us), (c,o) => { var v = (C3us)o; c.CodeC3us(ref v); } },
            { typeof(C3us[]), (c,o) => { var v = (C3us[])o; c.CodeC3usArray(ref v); } },
            { typeof(List<C3us>), (c,o) => { var v = (List<C3us>)o; c.CodeList_of_C3us_(ref v); } },

            { typeof(Vector<C3us>), (c,o) => { var v = (Vector<C3us>)o; c.CodeVector_of_C3us_(ref v); } },
            { typeof(Vector<C3us>[]), (c,o) => { var v = (Vector<C3us>[])o; c.CodeVector_of_C3us_Array(ref v); } },
            { typeof(List<Vector<C3us>>), (c,o) => { var v = (List<Vector<C3us>>)o; c.CodeList_of_Vector_of_C3us__(ref v); } },

            { typeof(Matrix<C3us>), (c,o) => { var v = (Matrix<C3us>)o; c.CodeMatrix_of_C3us_(ref v); } },
            { typeof(Matrix<C3us>[]), (c,o) => { var v = (Matrix<C3us>[])o; c.CodeMatrix_of_C3us_Array(ref v); } },
            { typeof(List<Matrix<C3us>>), (c,o) => { var v = (List<Matrix<C3us>>)o; c.CodeList_of_Matrix_of_C3us__(ref v); } },

            { typeof(Volume<C3us>), (c,o) => { var v = (Volume<C3us>)o; c.CodeVolume_of_C3us_(ref v); } },
            { typeof(Volume<C3us>[]), (c,o) => { var v = (Volume<C3us>[])o; c.CodeVolume_of_C3us_Array(ref v); } },
            { typeof(List<Volume<C3us>>), (c,o) => { var v = (List<Volume<C3us>>)o; c.CodeList_of_Volume_of_C3us__(ref v); } },

            { typeof(Tensor<C3us>), (c,o) => { var v = (Tensor<C3us>)o; c.CodeTensor_of_C3us_(ref v); } },
            { typeof(Tensor<C3us>[]), (c,o) => { var v = (Tensor<C3us>[])o; c.CodeTensor_of_C3us_Array(ref v); } },
            { typeof(List<Tensor<C3us>>), (c,o) => { var v = (List<Tensor<C3us>>)o; c.CodeList_of_Tensor_of_C3us__(ref v); } },

            #endregion

            #region C3ui

            { typeof(C3ui), (c,o) => { var v = (C3ui)o; c.CodeC3ui(ref v); } },
            { typeof(C3ui[]), (c,o) => { var v = (C3ui[])o; c.CodeC3uiArray(ref v); } },
            { typeof(List<C3ui>), (c,o) => { var v = (List<C3ui>)o; c.CodeList_of_C3ui_(ref v); } },

            { typeof(Vector<C3ui>), (c,o) => { var v = (Vector<C3ui>)o; c.CodeVector_of_C3ui_(ref v); } },
            { typeof(Vector<C3ui>[]), (c,o) => { var v = (Vector<C3ui>[])o; c.CodeVector_of_C3ui_Array(ref v); } },
            { typeof(List<Vector<C3ui>>), (c,o) => { var v = (List<Vector<C3ui>>)o; c.CodeList_of_Vector_of_C3ui__(ref v); } },

            { typeof(Matrix<C3ui>), (c,o) => { var v = (Matrix<C3ui>)o; c.CodeMatrix_of_C3ui_(ref v); } },
            { typeof(Matrix<C3ui>[]), (c,o) => { var v = (Matrix<C3ui>[])o; c.CodeMatrix_of_C3ui_Array(ref v); } },
            { typeof(List<Matrix<C3ui>>), (c,o) => { var v = (List<Matrix<C3ui>>)o; c.CodeList_of_Matrix_of_C3ui__(ref v); } },

            { typeof(Volume<C3ui>), (c,o) => { var v = (Volume<C3ui>)o; c.CodeVolume_of_C3ui_(ref v); } },
            { typeof(Volume<C3ui>[]), (c,o) => { var v = (Volume<C3ui>[])o; c.CodeVolume_of_C3ui_Array(ref v); } },
            { typeof(List<Volume<C3ui>>), (c,o) => { var v = (List<Volume<C3ui>>)o; c.CodeList_of_Volume_of_C3ui__(ref v); } },

            { typeof(Tensor<C3ui>), (c,o) => { var v = (Tensor<C3ui>)o; c.CodeTensor_of_C3ui_(ref v); } },
            { typeof(Tensor<C3ui>[]), (c,o) => { var v = (Tensor<C3ui>[])o; c.CodeTensor_of_C3ui_Array(ref v); } },
            { typeof(List<Tensor<C3ui>>), (c,o) => { var v = (List<Tensor<C3ui>>)o; c.CodeList_of_Tensor_of_C3ui__(ref v); } },

            #endregion

            #region C3f

            { typeof(C3f), (c,o) => { var v = (C3f)o; c.CodeC3f(ref v); } },
            { typeof(C3f[]), (c,o) => { var v = (C3f[])o; c.CodeC3fArray(ref v); } },
            { typeof(List<C3f>), (c,o) => { var v = (List<C3f>)o; c.CodeList_of_C3f_(ref v); } },

            { typeof(Vector<C3f>), (c,o) => { var v = (Vector<C3f>)o; c.CodeVector_of_C3f_(ref v); } },
            { typeof(Vector<C3f>[]), (c,o) => { var v = (Vector<C3f>[])o; c.CodeVector_of_C3f_Array(ref v); } },
            { typeof(List<Vector<C3f>>), (c,o) => { var v = (List<Vector<C3f>>)o; c.CodeList_of_Vector_of_C3f__(ref v); } },

            { typeof(Matrix<C3f>), (c,o) => { var v = (Matrix<C3f>)o; c.CodeMatrix_of_C3f_(ref v); } },
            { typeof(Matrix<C3f>[]), (c,o) => { var v = (Matrix<C3f>[])o; c.CodeMatrix_of_C3f_Array(ref v); } },
            { typeof(List<Matrix<C3f>>), (c,o) => { var v = (List<Matrix<C3f>>)o; c.CodeList_of_Matrix_of_C3f__(ref v); } },

            { typeof(Volume<C3f>), (c,o) => { var v = (Volume<C3f>)o; c.CodeVolume_of_C3f_(ref v); } },
            { typeof(Volume<C3f>[]), (c,o) => { var v = (Volume<C3f>[])o; c.CodeVolume_of_C3f_Array(ref v); } },
            { typeof(List<Volume<C3f>>), (c,o) => { var v = (List<Volume<C3f>>)o; c.CodeList_of_Volume_of_C3f__(ref v); } },

            { typeof(Tensor<C3f>), (c,o) => { var v = (Tensor<C3f>)o; c.CodeTensor_of_C3f_(ref v); } },
            { typeof(Tensor<C3f>[]), (c,o) => { var v = (Tensor<C3f>[])o; c.CodeTensor_of_C3f_Array(ref v); } },
            { typeof(List<Tensor<C3f>>), (c,o) => { var v = (List<Tensor<C3f>>)o; c.CodeList_of_Tensor_of_C3f__(ref v); } },

            #endregion

            #region C3d

            { typeof(C3d), (c,o) => { var v = (C3d)o; c.CodeC3d(ref v); } },
            { typeof(C3d[]), (c,o) => { var v = (C3d[])o; c.CodeC3dArray(ref v); } },
            { typeof(List<C3d>), (c,o) => { var v = (List<C3d>)o; c.CodeList_of_C3d_(ref v); } },

            { typeof(Vector<C3d>), (c,o) => { var v = (Vector<C3d>)o; c.CodeVector_of_C3d_(ref v); } },
            { typeof(Vector<C3d>[]), (c,o) => { var v = (Vector<C3d>[])o; c.CodeVector_of_C3d_Array(ref v); } },
            { typeof(List<Vector<C3d>>), (c,o) => { var v = (List<Vector<C3d>>)o; c.CodeList_of_Vector_of_C3d__(ref v); } },

            { typeof(Matrix<C3d>), (c,o) => { var v = (Matrix<C3d>)o; c.CodeMatrix_of_C3d_(ref v); } },
            { typeof(Matrix<C3d>[]), (c,o) => { var v = (Matrix<C3d>[])o; c.CodeMatrix_of_C3d_Array(ref v); } },
            { typeof(List<Matrix<C3d>>), (c,o) => { var v = (List<Matrix<C3d>>)o; c.CodeList_of_Matrix_of_C3d__(ref v); } },

            { typeof(Volume<C3d>), (c,o) => { var v = (Volume<C3d>)o; c.CodeVolume_of_C3d_(ref v); } },
            { typeof(Volume<C3d>[]), (c,o) => { var v = (Volume<C3d>[])o; c.CodeVolume_of_C3d_Array(ref v); } },
            { typeof(List<Volume<C3d>>), (c,o) => { var v = (List<Volume<C3d>>)o; c.CodeList_of_Volume_of_C3d__(ref v); } },

            { typeof(Tensor<C3d>), (c,o) => { var v = (Tensor<C3d>)o; c.CodeTensor_of_C3d_(ref v); } },
            { typeof(Tensor<C3d>[]), (c,o) => { var v = (Tensor<C3d>[])o; c.CodeTensor_of_C3d_Array(ref v); } },
            { typeof(List<Tensor<C3d>>), (c,o) => { var v = (List<Tensor<C3d>>)o; c.CodeList_of_Tensor_of_C3d__(ref v); } },

            #endregion

            #region C4b

            { typeof(C4b), (c,o) => { var v = (C4b)o; c.CodeC4b(ref v); } },
            { typeof(C4b[]), (c,o) => { var v = (C4b[])o; c.CodeC4bArray(ref v); } },
            { typeof(List<C4b>), (c,o) => { var v = (List<C4b>)o; c.CodeList_of_C4b_(ref v); } },

            { typeof(Vector<C4b>), (c,o) => { var v = (Vector<C4b>)o; c.CodeVector_of_C4b_(ref v); } },
            { typeof(Vector<C4b>[]), (c,o) => { var v = (Vector<C4b>[])o; c.CodeVector_of_C4b_Array(ref v); } },
            { typeof(List<Vector<C4b>>), (c,o) => { var v = (List<Vector<C4b>>)o; c.CodeList_of_Vector_of_C4b__(ref v); } },

            { typeof(Matrix<C4b>), (c,o) => { var v = (Matrix<C4b>)o; c.CodeMatrix_of_C4b_(ref v); } },
            { typeof(Matrix<C4b>[]), (c,o) => { var v = (Matrix<C4b>[])o; c.CodeMatrix_of_C4b_Array(ref v); } },
            { typeof(List<Matrix<C4b>>), (c,o) => { var v = (List<Matrix<C4b>>)o; c.CodeList_of_Matrix_of_C4b__(ref v); } },

            { typeof(Volume<C4b>), (c,o) => { var v = (Volume<C4b>)o; c.CodeVolume_of_C4b_(ref v); } },
            { typeof(Volume<C4b>[]), (c,o) => { var v = (Volume<C4b>[])o; c.CodeVolume_of_C4b_Array(ref v); } },
            { typeof(List<Volume<C4b>>), (c,o) => { var v = (List<Volume<C4b>>)o; c.CodeList_of_Volume_of_C4b__(ref v); } },

            { typeof(Tensor<C4b>), (c,o) => { var v = (Tensor<C4b>)o; c.CodeTensor_of_C4b_(ref v); } },
            { typeof(Tensor<C4b>[]), (c,o) => { var v = (Tensor<C4b>[])o; c.CodeTensor_of_C4b_Array(ref v); } },
            { typeof(List<Tensor<C4b>>), (c,o) => { var v = (List<Tensor<C4b>>)o; c.CodeList_of_Tensor_of_C4b__(ref v); } },

            #endregion

            #region C4us

            { typeof(C4us), (c,o) => { var v = (C4us)o; c.CodeC4us(ref v); } },
            { typeof(C4us[]), (c,o) => { var v = (C4us[])o; c.CodeC4usArray(ref v); } },
            { typeof(List<C4us>), (c,o) => { var v = (List<C4us>)o; c.CodeList_of_C4us_(ref v); } },

            { typeof(Vector<C4us>), (c,o) => { var v = (Vector<C4us>)o; c.CodeVector_of_C4us_(ref v); } },
            { typeof(Vector<C4us>[]), (c,o) => { var v = (Vector<C4us>[])o; c.CodeVector_of_C4us_Array(ref v); } },
            { typeof(List<Vector<C4us>>), (c,o) => { var v = (List<Vector<C4us>>)o; c.CodeList_of_Vector_of_C4us__(ref v); } },

            { typeof(Matrix<C4us>), (c,o) => { var v = (Matrix<C4us>)o; c.CodeMatrix_of_C4us_(ref v); } },
            { typeof(Matrix<C4us>[]), (c,o) => { var v = (Matrix<C4us>[])o; c.CodeMatrix_of_C4us_Array(ref v); } },
            { typeof(List<Matrix<C4us>>), (c,o) => { var v = (List<Matrix<C4us>>)o; c.CodeList_of_Matrix_of_C4us__(ref v); } },

            { typeof(Volume<C4us>), (c,o) => { var v = (Volume<C4us>)o; c.CodeVolume_of_C4us_(ref v); } },
            { typeof(Volume<C4us>[]), (c,o) => { var v = (Volume<C4us>[])o; c.CodeVolume_of_C4us_Array(ref v); } },
            { typeof(List<Volume<C4us>>), (c,o) => { var v = (List<Volume<C4us>>)o; c.CodeList_of_Volume_of_C4us__(ref v); } },

            { typeof(Tensor<C4us>), (c,o) => { var v = (Tensor<C4us>)o; c.CodeTensor_of_C4us_(ref v); } },
            { typeof(Tensor<C4us>[]), (c,o) => { var v = (Tensor<C4us>[])o; c.CodeTensor_of_C4us_Array(ref v); } },
            { typeof(List<Tensor<C4us>>), (c,o) => { var v = (List<Tensor<C4us>>)o; c.CodeList_of_Tensor_of_C4us__(ref v); } },

            #endregion

            #region C4ui

            { typeof(C4ui), (c,o) => { var v = (C4ui)o; c.CodeC4ui(ref v); } },
            { typeof(C4ui[]), (c,o) => { var v = (C4ui[])o; c.CodeC4uiArray(ref v); } },
            { typeof(List<C4ui>), (c,o) => { var v = (List<C4ui>)o; c.CodeList_of_C4ui_(ref v); } },

            { typeof(Vector<C4ui>), (c,o) => { var v = (Vector<C4ui>)o; c.CodeVector_of_C4ui_(ref v); } },
            { typeof(Vector<C4ui>[]), (c,o) => { var v = (Vector<C4ui>[])o; c.CodeVector_of_C4ui_Array(ref v); } },
            { typeof(List<Vector<C4ui>>), (c,o) => { var v = (List<Vector<C4ui>>)o; c.CodeList_of_Vector_of_C4ui__(ref v); } },

            { typeof(Matrix<C4ui>), (c,o) => { var v = (Matrix<C4ui>)o; c.CodeMatrix_of_C4ui_(ref v); } },
            { typeof(Matrix<C4ui>[]), (c,o) => { var v = (Matrix<C4ui>[])o; c.CodeMatrix_of_C4ui_Array(ref v); } },
            { typeof(List<Matrix<C4ui>>), (c,o) => { var v = (List<Matrix<C4ui>>)o; c.CodeList_of_Matrix_of_C4ui__(ref v); } },

            { typeof(Volume<C4ui>), (c,o) => { var v = (Volume<C4ui>)o; c.CodeVolume_of_C4ui_(ref v); } },
            { typeof(Volume<C4ui>[]), (c,o) => { var v = (Volume<C4ui>[])o; c.CodeVolume_of_C4ui_Array(ref v); } },
            { typeof(List<Volume<C4ui>>), (c,o) => { var v = (List<Volume<C4ui>>)o; c.CodeList_of_Volume_of_C4ui__(ref v); } },

            { typeof(Tensor<C4ui>), (c,o) => { var v = (Tensor<C4ui>)o; c.CodeTensor_of_C4ui_(ref v); } },
            { typeof(Tensor<C4ui>[]), (c,o) => { var v = (Tensor<C4ui>[])o; c.CodeTensor_of_C4ui_Array(ref v); } },
            { typeof(List<Tensor<C4ui>>), (c,o) => { var v = (List<Tensor<C4ui>>)o; c.CodeList_of_Tensor_of_C4ui__(ref v); } },

            #endregion

            #region C4f

            { typeof(C4f), (c,o) => { var v = (C4f)o; c.CodeC4f(ref v); } },
            { typeof(C4f[]), (c,o) => { var v = (C4f[])o; c.CodeC4fArray(ref v); } },
            { typeof(List<C4f>), (c,o) => { var v = (List<C4f>)o; c.CodeList_of_C4f_(ref v); } },

            { typeof(Vector<C4f>), (c,o) => { var v = (Vector<C4f>)o; c.CodeVector_of_C4f_(ref v); } },
            { typeof(Vector<C4f>[]), (c,o) => { var v = (Vector<C4f>[])o; c.CodeVector_of_C4f_Array(ref v); } },
            { typeof(List<Vector<C4f>>), (c,o) => { var v = (List<Vector<C4f>>)o; c.CodeList_of_Vector_of_C4f__(ref v); } },

            { typeof(Matrix<C4f>), (c,o) => { var v = (Matrix<C4f>)o; c.CodeMatrix_of_C4f_(ref v); } },
            { typeof(Matrix<C4f>[]), (c,o) => { var v = (Matrix<C4f>[])o; c.CodeMatrix_of_C4f_Array(ref v); } },
            { typeof(List<Matrix<C4f>>), (c,o) => { var v = (List<Matrix<C4f>>)o; c.CodeList_of_Matrix_of_C4f__(ref v); } },

            { typeof(Volume<C4f>), (c,o) => { var v = (Volume<C4f>)o; c.CodeVolume_of_C4f_(ref v); } },
            { typeof(Volume<C4f>[]), (c,o) => { var v = (Volume<C4f>[])o; c.CodeVolume_of_C4f_Array(ref v); } },
            { typeof(List<Volume<C4f>>), (c,o) => { var v = (List<Volume<C4f>>)o; c.CodeList_of_Volume_of_C4f__(ref v); } },

            { typeof(Tensor<C4f>), (c,o) => { var v = (Tensor<C4f>)o; c.CodeTensor_of_C4f_(ref v); } },
            { typeof(Tensor<C4f>[]), (c,o) => { var v = (Tensor<C4f>[])o; c.CodeTensor_of_C4f_Array(ref v); } },
            { typeof(List<Tensor<C4f>>), (c,o) => { var v = (List<Tensor<C4f>>)o; c.CodeList_of_Tensor_of_C4f__(ref v); } },

            #endregion

            #region C4d

            { typeof(C4d), (c,o) => { var v = (C4d)o; c.CodeC4d(ref v); } },
            { typeof(C4d[]), (c,o) => { var v = (C4d[])o; c.CodeC4dArray(ref v); } },
            { typeof(List<C4d>), (c,o) => { var v = (List<C4d>)o; c.CodeList_of_C4d_(ref v); } },

            { typeof(Vector<C4d>), (c,o) => { var v = (Vector<C4d>)o; c.CodeVector_of_C4d_(ref v); } },
            { typeof(Vector<C4d>[]), (c,o) => { var v = (Vector<C4d>[])o; c.CodeVector_of_C4d_Array(ref v); } },
            { typeof(List<Vector<C4d>>), (c,o) => { var v = (List<Vector<C4d>>)o; c.CodeList_of_Vector_of_C4d__(ref v); } },

            { typeof(Matrix<C4d>), (c,o) => { var v = (Matrix<C4d>)o; c.CodeMatrix_of_C4d_(ref v); } },
            { typeof(Matrix<C4d>[]), (c,o) => { var v = (Matrix<C4d>[])o; c.CodeMatrix_of_C4d_Array(ref v); } },
            { typeof(List<Matrix<C4d>>), (c,o) => { var v = (List<Matrix<C4d>>)o; c.CodeList_of_Matrix_of_C4d__(ref v); } },

            { typeof(Volume<C4d>), (c,o) => { var v = (Volume<C4d>)o; c.CodeVolume_of_C4d_(ref v); } },
            { typeof(Volume<C4d>[]), (c,o) => { var v = (Volume<C4d>[])o; c.CodeVolume_of_C4d_Array(ref v); } },
            { typeof(List<Volume<C4d>>), (c,o) => { var v = (List<Volume<C4d>>)o; c.CodeList_of_Volume_of_C4d__(ref v); } },

            { typeof(Tensor<C4d>), (c,o) => { var v = (Tensor<C4d>)o; c.CodeTensor_of_C4d_(ref v); } },
            { typeof(Tensor<C4d>[]), (c,o) => { var v = (Tensor<C4d>[])o; c.CodeTensor_of_C4d_Array(ref v); } },
            { typeof(List<Tensor<C4d>>), (c,o) => { var v = (List<Tensor<C4d>>)o; c.CodeList_of_Tensor_of_C4d__(ref v); } },

            #endregion

            #region Range1b

            { typeof(Range1b), (c,o) => { var v = (Range1b)o; c.CodeRange1b(ref v); } },
            { typeof(Range1b[]), (c,o) => { var v = (Range1b[])o; c.CodeRange1bArray(ref v); } },
            { typeof(List<Range1b>), (c,o) => { var v = (List<Range1b>)o; c.CodeList_of_Range1b_(ref v); } },

            { typeof(Vector<Range1b>), (c,o) => { var v = (Vector<Range1b>)o; c.CodeVector_of_Range1b_(ref v); } },
            { typeof(Vector<Range1b>[]), (c,o) => { var v = (Vector<Range1b>[])o; c.CodeVector_of_Range1b_Array(ref v); } },
            { typeof(List<Vector<Range1b>>), (c,o) => { var v = (List<Vector<Range1b>>)o; c.CodeList_of_Vector_of_Range1b__(ref v); } },

            { typeof(Matrix<Range1b>), (c,o) => { var v = (Matrix<Range1b>)o; c.CodeMatrix_of_Range1b_(ref v); } },
            { typeof(Matrix<Range1b>[]), (c,o) => { var v = (Matrix<Range1b>[])o; c.CodeMatrix_of_Range1b_Array(ref v); } },
            { typeof(List<Matrix<Range1b>>), (c,o) => { var v = (List<Matrix<Range1b>>)o; c.CodeList_of_Matrix_of_Range1b__(ref v); } },

            { typeof(Volume<Range1b>), (c,o) => { var v = (Volume<Range1b>)o; c.CodeVolume_of_Range1b_(ref v); } },
            { typeof(Volume<Range1b>[]), (c,o) => { var v = (Volume<Range1b>[])o; c.CodeVolume_of_Range1b_Array(ref v); } },
            { typeof(List<Volume<Range1b>>), (c,o) => { var v = (List<Volume<Range1b>>)o; c.CodeList_of_Volume_of_Range1b__(ref v); } },

            { typeof(Tensor<Range1b>), (c,o) => { var v = (Tensor<Range1b>)o; c.CodeTensor_of_Range1b_(ref v); } },
            { typeof(Tensor<Range1b>[]), (c,o) => { var v = (Tensor<Range1b>[])o; c.CodeTensor_of_Range1b_Array(ref v); } },
            { typeof(List<Tensor<Range1b>>), (c,o) => { var v = (List<Tensor<Range1b>>)o; c.CodeList_of_Tensor_of_Range1b__(ref v); } },

            #endregion

            #region Range1sb

            { typeof(Range1sb), (c,o) => { var v = (Range1sb)o; c.CodeRange1sb(ref v); } },
            { typeof(Range1sb[]), (c,o) => { var v = (Range1sb[])o; c.CodeRange1sbArray(ref v); } },
            { typeof(List<Range1sb>), (c,o) => { var v = (List<Range1sb>)o; c.CodeList_of_Range1sb_(ref v); } },

            { typeof(Vector<Range1sb>), (c,o) => { var v = (Vector<Range1sb>)o; c.CodeVector_of_Range1sb_(ref v); } },
            { typeof(Vector<Range1sb>[]), (c,o) => { var v = (Vector<Range1sb>[])o; c.CodeVector_of_Range1sb_Array(ref v); } },
            { typeof(List<Vector<Range1sb>>), (c,o) => { var v = (List<Vector<Range1sb>>)o; c.CodeList_of_Vector_of_Range1sb__(ref v); } },

            { typeof(Matrix<Range1sb>), (c,o) => { var v = (Matrix<Range1sb>)o; c.CodeMatrix_of_Range1sb_(ref v); } },
            { typeof(Matrix<Range1sb>[]), (c,o) => { var v = (Matrix<Range1sb>[])o; c.CodeMatrix_of_Range1sb_Array(ref v); } },
            { typeof(List<Matrix<Range1sb>>), (c,o) => { var v = (List<Matrix<Range1sb>>)o; c.CodeList_of_Matrix_of_Range1sb__(ref v); } },

            { typeof(Volume<Range1sb>), (c,o) => { var v = (Volume<Range1sb>)o; c.CodeVolume_of_Range1sb_(ref v); } },
            { typeof(Volume<Range1sb>[]), (c,o) => { var v = (Volume<Range1sb>[])o; c.CodeVolume_of_Range1sb_Array(ref v); } },
            { typeof(List<Volume<Range1sb>>), (c,o) => { var v = (List<Volume<Range1sb>>)o; c.CodeList_of_Volume_of_Range1sb__(ref v); } },

            { typeof(Tensor<Range1sb>), (c,o) => { var v = (Tensor<Range1sb>)o; c.CodeTensor_of_Range1sb_(ref v); } },
            { typeof(Tensor<Range1sb>[]), (c,o) => { var v = (Tensor<Range1sb>[])o; c.CodeTensor_of_Range1sb_Array(ref v); } },
            { typeof(List<Tensor<Range1sb>>), (c,o) => { var v = (List<Tensor<Range1sb>>)o; c.CodeList_of_Tensor_of_Range1sb__(ref v); } },

            #endregion

            #region Range1s

            { typeof(Range1s), (c,o) => { var v = (Range1s)o; c.CodeRange1s(ref v); } },
            { typeof(Range1s[]), (c,o) => { var v = (Range1s[])o; c.CodeRange1sArray(ref v); } },
            { typeof(List<Range1s>), (c,o) => { var v = (List<Range1s>)o; c.CodeList_of_Range1s_(ref v); } },

            { typeof(Vector<Range1s>), (c,o) => { var v = (Vector<Range1s>)o; c.CodeVector_of_Range1s_(ref v); } },
            { typeof(Vector<Range1s>[]), (c,o) => { var v = (Vector<Range1s>[])o; c.CodeVector_of_Range1s_Array(ref v); } },
            { typeof(List<Vector<Range1s>>), (c,o) => { var v = (List<Vector<Range1s>>)o; c.CodeList_of_Vector_of_Range1s__(ref v); } },

            { typeof(Matrix<Range1s>), (c,o) => { var v = (Matrix<Range1s>)o; c.CodeMatrix_of_Range1s_(ref v); } },
            { typeof(Matrix<Range1s>[]), (c,o) => { var v = (Matrix<Range1s>[])o; c.CodeMatrix_of_Range1s_Array(ref v); } },
            { typeof(List<Matrix<Range1s>>), (c,o) => { var v = (List<Matrix<Range1s>>)o; c.CodeList_of_Matrix_of_Range1s__(ref v); } },

            { typeof(Volume<Range1s>), (c,o) => { var v = (Volume<Range1s>)o; c.CodeVolume_of_Range1s_(ref v); } },
            { typeof(Volume<Range1s>[]), (c,o) => { var v = (Volume<Range1s>[])o; c.CodeVolume_of_Range1s_Array(ref v); } },
            { typeof(List<Volume<Range1s>>), (c,o) => { var v = (List<Volume<Range1s>>)o; c.CodeList_of_Volume_of_Range1s__(ref v); } },

            { typeof(Tensor<Range1s>), (c,o) => { var v = (Tensor<Range1s>)o; c.CodeTensor_of_Range1s_(ref v); } },
            { typeof(Tensor<Range1s>[]), (c,o) => { var v = (Tensor<Range1s>[])o; c.CodeTensor_of_Range1s_Array(ref v); } },
            { typeof(List<Tensor<Range1s>>), (c,o) => { var v = (List<Tensor<Range1s>>)o; c.CodeList_of_Tensor_of_Range1s__(ref v); } },

            #endregion

            #region Range1us

            { typeof(Range1us), (c,o) => { var v = (Range1us)o; c.CodeRange1us(ref v); } },
            { typeof(Range1us[]), (c,o) => { var v = (Range1us[])o; c.CodeRange1usArray(ref v); } },
            { typeof(List<Range1us>), (c,o) => { var v = (List<Range1us>)o; c.CodeList_of_Range1us_(ref v); } },

            { typeof(Vector<Range1us>), (c,o) => { var v = (Vector<Range1us>)o; c.CodeVector_of_Range1us_(ref v); } },
            { typeof(Vector<Range1us>[]), (c,o) => { var v = (Vector<Range1us>[])o; c.CodeVector_of_Range1us_Array(ref v); } },
            { typeof(List<Vector<Range1us>>), (c,o) => { var v = (List<Vector<Range1us>>)o; c.CodeList_of_Vector_of_Range1us__(ref v); } },

            { typeof(Matrix<Range1us>), (c,o) => { var v = (Matrix<Range1us>)o; c.CodeMatrix_of_Range1us_(ref v); } },
            { typeof(Matrix<Range1us>[]), (c,o) => { var v = (Matrix<Range1us>[])o; c.CodeMatrix_of_Range1us_Array(ref v); } },
            { typeof(List<Matrix<Range1us>>), (c,o) => { var v = (List<Matrix<Range1us>>)o; c.CodeList_of_Matrix_of_Range1us__(ref v); } },

            { typeof(Volume<Range1us>), (c,o) => { var v = (Volume<Range1us>)o; c.CodeVolume_of_Range1us_(ref v); } },
            { typeof(Volume<Range1us>[]), (c,o) => { var v = (Volume<Range1us>[])o; c.CodeVolume_of_Range1us_Array(ref v); } },
            { typeof(List<Volume<Range1us>>), (c,o) => { var v = (List<Volume<Range1us>>)o; c.CodeList_of_Volume_of_Range1us__(ref v); } },

            { typeof(Tensor<Range1us>), (c,o) => { var v = (Tensor<Range1us>)o; c.CodeTensor_of_Range1us_(ref v); } },
            { typeof(Tensor<Range1us>[]), (c,o) => { var v = (Tensor<Range1us>[])o; c.CodeTensor_of_Range1us_Array(ref v); } },
            { typeof(List<Tensor<Range1us>>), (c,o) => { var v = (List<Tensor<Range1us>>)o; c.CodeList_of_Tensor_of_Range1us__(ref v); } },

            #endregion

            #region Range1i

            { typeof(Range1i), (c,o) => { var v = (Range1i)o; c.CodeRange1i(ref v); } },
            { typeof(Range1i[]), (c,o) => { var v = (Range1i[])o; c.CodeRange1iArray(ref v); } },
            { typeof(List<Range1i>), (c,o) => { var v = (List<Range1i>)o; c.CodeList_of_Range1i_(ref v); } },

            { typeof(Vector<Range1i>), (c,o) => { var v = (Vector<Range1i>)o; c.CodeVector_of_Range1i_(ref v); } },
            { typeof(Vector<Range1i>[]), (c,o) => { var v = (Vector<Range1i>[])o; c.CodeVector_of_Range1i_Array(ref v); } },
            { typeof(List<Vector<Range1i>>), (c,o) => { var v = (List<Vector<Range1i>>)o; c.CodeList_of_Vector_of_Range1i__(ref v); } },

            { typeof(Matrix<Range1i>), (c,o) => { var v = (Matrix<Range1i>)o; c.CodeMatrix_of_Range1i_(ref v); } },
            { typeof(Matrix<Range1i>[]), (c,o) => { var v = (Matrix<Range1i>[])o; c.CodeMatrix_of_Range1i_Array(ref v); } },
            { typeof(List<Matrix<Range1i>>), (c,o) => { var v = (List<Matrix<Range1i>>)o; c.CodeList_of_Matrix_of_Range1i__(ref v); } },

            { typeof(Volume<Range1i>), (c,o) => { var v = (Volume<Range1i>)o; c.CodeVolume_of_Range1i_(ref v); } },
            { typeof(Volume<Range1i>[]), (c,o) => { var v = (Volume<Range1i>[])o; c.CodeVolume_of_Range1i_Array(ref v); } },
            { typeof(List<Volume<Range1i>>), (c,o) => { var v = (List<Volume<Range1i>>)o; c.CodeList_of_Volume_of_Range1i__(ref v); } },

            { typeof(Tensor<Range1i>), (c,o) => { var v = (Tensor<Range1i>)o; c.CodeTensor_of_Range1i_(ref v); } },
            { typeof(Tensor<Range1i>[]), (c,o) => { var v = (Tensor<Range1i>[])o; c.CodeTensor_of_Range1i_Array(ref v); } },
            { typeof(List<Tensor<Range1i>>), (c,o) => { var v = (List<Tensor<Range1i>>)o; c.CodeList_of_Tensor_of_Range1i__(ref v); } },

            #endregion

            #region Range1ui

            { typeof(Range1ui), (c,o) => { var v = (Range1ui)o; c.CodeRange1ui(ref v); } },
            { typeof(Range1ui[]), (c,o) => { var v = (Range1ui[])o; c.CodeRange1uiArray(ref v); } },
            { typeof(List<Range1ui>), (c,o) => { var v = (List<Range1ui>)o; c.CodeList_of_Range1ui_(ref v); } },

            { typeof(Vector<Range1ui>), (c,o) => { var v = (Vector<Range1ui>)o; c.CodeVector_of_Range1ui_(ref v); } },
            { typeof(Vector<Range1ui>[]), (c,o) => { var v = (Vector<Range1ui>[])o; c.CodeVector_of_Range1ui_Array(ref v); } },
            { typeof(List<Vector<Range1ui>>), (c,o) => { var v = (List<Vector<Range1ui>>)o; c.CodeList_of_Vector_of_Range1ui__(ref v); } },

            { typeof(Matrix<Range1ui>), (c,o) => { var v = (Matrix<Range1ui>)o; c.CodeMatrix_of_Range1ui_(ref v); } },
            { typeof(Matrix<Range1ui>[]), (c,o) => { var v = (Matrix<Range1ui>[])o; c.CodeMatrix_of_Range1ui_Array(ref v); } },
            { typeof(List<Matrix<Range1ui>>), (c,o) => { var v = (List<Matrix<Range1ui>>)o; c.CodeList_of_Matrix_of_Range1ui__(ref v); } },

            { typeof(Volume<Range1ui>), (c,o) => { var v = (Volume<Range1ui>)o; c.CodeVolume_of_Range1ui_(ref v); } },
            { typeof(Volume<Range1ui>[]), (c,o) => { var v = (Volume<Range1ui>[])o; c.CodeVolume_of_Range1ui_Array(ref v); } },
            { typeof(List<Volume<Range1ui>>), (c,o) => { var v = (List<Volume<Range1ui>>)o; c.CodeList_of_Volume_of_Range1ui__(ref v); } },

            { typeof(Tensor<Range1ui>), (c,o) => { var v = (Tensor<Range1ui>)o; c.CodeTensor_of_Range1ui_(ref v); } },
            { typeof(Tensor<Range1ui>[]), (c,o) => { var v = (Tensor<Range1ui>[])o; c.CodeTensor_of_Range1ui_Array(ref v); } },
            { typeof(List<Tensor<Range1ui>>), (c,o) => { var v = (List<Tensor<Range1ui>>)o; c.CodeList_of_Tensor_of_Range1ui__(ref v); } },

            #endregion

            #region Range1l

            { typeof(Range1l), (c,o) => { var v = (Range1l)o; c.CodeRange1l(ref v); } },
            { typeof(Range1l[]), (c,o) => { var v = (Range1l[])o; c.CodeRange1lArray(ref v); } },
            { typeof(List<Range1l>), (c,o) => { var v = (List<Range1l>)o; c.CodeList_of_Range1l_(ref v); } },

            { typeof(Vector<Range1l>), (c,o) => { var v = (Vector<Range1l>)o; c.CodeVector_of_Range1l_(ref v); } },
            { typeof(Vector<Range1l>[]), (c,o) => { var v = (Vector<Range1l>[])o; c.CodeVector_of_Range1l_Array(ref v); } },
            { typeof(List<Vector<Range1l>>), (c,o) => { var v = (List<Vector<Range1l>>)o; c.CodeList_of_Vector_of_Range1l__(ref v); } },

            { typeof(Matrix<Range1l>), (c,o) => { var v = (Matrix<Range1l>)o; c.CodeMatrix_of_Range1l_(ref v); } },
            { typeof(Matrix<Range1l>[]), (c,o) => { var v = (Matrix<Range1l>[])o; c.CodeMatrix_of_Range1l_Array(ref v); } },
            { typeof(List<Matrix<Range1l>>), (c,o) => { var v = (List<Matrix<Range1l>>)o; c.CodeList_of_Matrix_of_Range1l__(ref v); } },

            { typeof(Volume<Range1l>), (c,o) => { var v = (Volume<Range1l>)o; c.CodeVolume_of_Range1l_(ref v); } },
            { typeof(Volume<Range1l>[]), (c,o) => { var v = (Volume<Range1l>[])o; c.CodeVolume_of_Range1l_Array(ref v); } },
            { typeof(List<Volume<Range1l>>), (c,o) => { var v = (List<Volume<Range1l>>)o; c.CodeList_of_Volume_of_Range1l__(ref v); } },

            { typeof(Tensor<Range1l>), (c,o) => { var v = (Tensor<Range1l>)o; c.CodeTensor_of_Range1l_(ref v); } },
            { typeof(Tensor<Range1l>[]), (c,o) => { var v = (Tensor<Range1l>[])o; c.CodeTensor_of_Range1l_Array(ref v); } },
            { typeof(List<Tensor<Range1l>>), (c,o) => { var v = (List<Tensor<Range1l>>)o; c.CodeList_of_Tensor_of_Range1l__(ref v); } },

            #endregion

            #region Range1ul

            { typeof(Range1ul), (c,o) => { var v = (Range1ul)o; c.CodeRange1ul(ref v); } },
            { typeof(Range1ul[]), (c,o) => { var v = (Range1ul[])o; c.CodeRange1ulArray(ref v); } },
            { typeof(List<Range1ul>), (c,o) => { var v = (List<Range1ul>)o; c.CodeList_of_Range1ul_(ref v); } },

            { typeof(Vector<Range1ul>), (c,o) => { var v = (Vector<Range1ul>)o; c.CodeVector_of_Range1ul_(ref v); } },
            { typeof(Vector<Range1ul>[]), (c,o) => { var v = (Vector<Range1ul>[])o; c.CodeVector_of_Range1ul_Array(ref v); } },
            { typeof(List<Vector<Range1ul>>), (c,o) => { var v = (List<Vector<Range1ul>>)o; c.CodeList_of_Vector_of_Range1ul__(ref v); } },

            { typeof(Matrix<Range1ul>), (c,o) => { var v = (Matrix<Range1ul>)o; c.CodeMatrix_of_Range1ul_(ref v); } },
            { typeof(Matrix<Range1ul>[]), (c,o) => { var v = (Matrix<Range1ul>[])o; c.CodeMatrix_of_Range1ul_Array(ref v); } },
            { typeof(List<Matrix<Range1ul>>), (c,o) => { var v = (List<Matrix<Range1ul>>)o; c.CodeList_of_Matrix_of_Range1ul__(ref v); } },

            { typeof(Volume<Range1ul>), (c,o) => { var v = (Volume<Range1ul>)o; c.CodeVolume_of_Range1ul_(ref v); } },
            { typeof(Volume<Range1ul>[]), (c,o) => { var v = (Volume<Range1ul>[])o; c.CodeVolume_of_Range1ul_Array(ref v); } },
            { typeof(List<Volume<Range1ul>>), (c,o) => { var v = (List<Volume<Range1ul>>)o; c.CodeList_of_Volume_of_Range1ul__(ref v); } },

            { typeof(Tensor<Range1ul>), (c,o) => { var v = (Tensor<Range1ul>)o; c.CodeTensor_of_Range1ul_(ref v); } },
            { typeof(Tensor<Range1ul>[]), (c,o) => { var v = (Tensor<Range1ul>[])o; c.CodeTensor_of_Range1ul_Array(ref v); } },
            { typeof(List<Tensor<Range1ul>>), (c,o) => { var v = (List<Tensor<Range1ul>>)o; c.CodeList_of_Tensor_of_Range1ul__(ref v); } },

            #endregion

            #region Range1f

            { typeof(Range1f), (c,o) => { var v = (Range1f)o; c.CodeRange1f(ref v); } },
            { typeof(Range1f[]), (c,o) => { var v = (Range1f[])o; c.CodeRange1fArray(ref v); } },
            { typeof(List<Range1f>), (c,o) => { var v = (List<Range1f>)o; c.CodeList_of_Range1f_(ref v); } },

            { typeof(Vector<Range1f>), (c,o) => { var v = (Vector<Range1f>)o; c.CodeVector_of_Range1f_(ref v); } },
            { typeof(Vector<Range1f>[]), (c,o) => { var v = (Vector<Range1f>[])o; c.CodeVector_of_Range1f_Array(ref v); } },
            { typeof(List<Vector<Range1f>>), (c,o) => { var v = (List<Vector<Range1f>>)o; c.CodeList_of_Vector_of_Range1f__(ref v); } },

            { typeof(Matrix<Range1f>), (c,o) => { var v = (Matrix<Range1f>)o; c.CodeMatrix_of_Range1f_(ref v); } },
            { typeof(Matrix<Range1f>[]), (c,o) => { var v = (Matrix<Range1f>[])o; c.CodeMatrix_of_Range1f_Array(ref v); } },
            { typeof(List<Matrix<Range1f>>), (c,o) => { var v = (List<Matrix<Range1f>>)o; c.CodeList_of_Matrix_of_Range1f__(ref v); } },

            { typeof(Volume<Range1f>), (c,o) => { var v = (Volume<Range1f>)o; c.CodeVolume_of_Range1f_(ref v); } },
            { typeof(Volume<Range1f>[]), (c,o) => { var v = (Volume<Range1f>[])o; c.CodeVolume_of_Range1f_Array(ref v); } },
            { typeof(List<Volume<Range1f>>), (c,o) => { var v = (List<Volume<Range1f>>)o; c.CodeList_of_Volume_of_Range1f__(ref v); } },

            { typeof(Tensor<Range1f>), (c,o) => { var v = (Tensor<Range1f>)o; c.CodeTensor_of_Range1f_(ref v); } },
            { typeof(Tensor<Range1f>[]), (c,o) => { var v = (Tensor<Range1f>[])o; c.CodeTensor_of_Range1f_Array(ref v); } },
            { typeof(List<Tensor<Range1f>>), (c,o) => { var v = (List<Tensor<Range1f>>)o; c.CodeList_of_Tensor_of_Range1f__(ref v); } },

            #endregion

            #region Range1d

            { typeof(Range1d), (c,o) => { var v = (Range1d)o; c.CodeRange1d(ref v); } },
            { typeof(Range1d[]), (c,o) => { var v = (Range1d[])o; c.CodeRange1dArray(ref v); } },
            { typeof(List<Range1d>), (c,o) => { var v = (List<Range1d>)o; c.CodeList_of_Range1d_(ref v); } },

            { typeof(Vector<Range1d>), (c,o) => { var v = (Vector<Range1d>)o; c.CodeVector_of_Range1d_(ref v); } },
            { typeof(Vector<Range1d>[]), (c,o) => { var v = (Vector<Range1d>[])o; c.CodeVector_of_Range1d_Array(ref v); } },
            { typeof(List<Vector<Range1d>>), (c,o) => { var v = (List<Vector<Range1d>>)o; c.CodeList_of_Vector_of_Range1d__(ref v); } },

            { typeof(Matrix<Range1d>), (c,o) => { var v = (Matrix<Range1d>)o; c.CodeMatrix_of_Range1d_(ref v); } },
            { typeof(Matrix<Range1d>[]), (c,o) => { var v = (Matrix<Range1d>[])o; c.CodeMatrix_of_Range1d_Array(ref v); } },
            { typeof(List<Matrix<Range1d>>), (c,o) => { var v = (List<Matrix<Range1d>>)o; c.CodeList_of_Matrix_of_Range1d__(ref v); } },

            { typeof(Volume<Range1d>), (c,o) => { var v = (Volume<Range1d>)o; c.CodeVolume_of_Range1d_(ref v); } },
            { typeof(Volume<Range1d>[]), (c,o) => { var v = (Volume<Range1d>[])o; c.CodeVolume_of_Range1d_Array(ref v); } },
            { typeof(List<Volume<Range1d>>), (c,o) => { var v = (List<Volume<Range1d>>)o; c.CodeList_of_Volume_of_Range1d__(ref v); } },

            { typeof(Tensor<Range1d>), (c,o) => { var v = (Tensor<Range1d>)o; c.CodeTensor_of_Range1d_(ref v); } },
            { typeof(Tensor<Range1d>[]), (c,o) => { var v = (Tensor<Range1d>[])o; c.CodeTensor_of_Range1d_Array(ref v); } },
            { typeof(List<Tensor<Range1d>>), (c,o) => { var v = (List<Tensor<Range1d>>)o; c.CodeList_of_Tensor_of_Range1d__(ref v); } },

            #endregion

            #region Box2i

            { typeof(Box2i), (c,o) => { var v = (Box2i)o; c.CodeBox2i(ref v); } },
            { typeof(Box2i[]), (c,o) => { var v = (Box2i[])o; c.CodeBox2iArray(ref v); } },
            { typeof(List<Box2i>), (c,o) => { var v = (List<Box2i>)o; c.CodeList_of_Box2i_(ref v); } },

            { typeof(Vector<Box2i>), (c,o) => { var v = (Vector<Box2i>)o; c.CodeVector_of_Box2i_(ref v); } },
            { typeof(Vector<Box2i>[]), (c,o) => { var v = (Vector<Box2i>[])o; c.CodeVector_of_Box2i_Array(ref v); } },
            { typeof(List<Vector<Box2i>>), (c,o) => { var v = (List<Vector<Box2i>>)o; c.CodeList_of_Vector_of_Box2i__(ref v); } },

            { typeof(Matrix<Box2i>), (c,o) => { var v = (Matrix<Box2i>)o; c.CodeMatrix_of_Box2i_(ref v); } },
            { typeof(Matrix<Box2i>[]), (c,o) => { var v = (Matrix<Box2i>[])o; c.CodeMatrix_of_Box2i_Array(ref v); } },
            { typeof(List<Matrix<Box2i>>), (c,o) => { var v = (List<Matrix<Box2i>>)o; c.CodeList_of_Matrix_of_Box2i__(ref v); } },

            { typeof(Volume<Box2i>), (c,o) => { var v = (Volume<Box2i>)o; c.CodeVolume_of_Box2i_(ref v); } },
            { typeof(Volume<Box2i>[]), (c,o) => { var v = (Volume<Box2i>[])o; c.CodeVolume_of_Box2i_Array(ref v); } },
            { typeof(List<Volume<Box2i>>), (c,o) => { var v = (List<Volume<Box2i>>)o; c.CodeList_of_Volume_of_Box2i__(ref v); } },

            { typeof(Tensor<Box2i>), (c,o) => { var v = (Tensor<Box2i>)o; c.CodeTensor_of_Box2i_(ref v); } },
            { typeof(Tensor<Box2i>[]), (c,o) => { var v = (Tensor<Box2i>[])o; c.CodeTensor_of_Box2i_Array(ref v); } },
            { typeof(List<Tensor<Box2i>>), (c,o) => { var v = (List<Tensor<Box2i>>)o; c.CodeList_of_Tensor_of_Box2i__(ref v); } },

            #endregion

            #region Box2l

            { typeof(Box2l), (c,o) => { var v = (Box2l)o; c.CodeBox2l(ref v); } },
            { typeof(Box2l[]), (c,o) => { var v = (Box2l[])o; c.CodeBox2lArray(ref v); } },
            { typeof(List<Box2l>), (c,o) => { var v = (List<Box2l>)o; c.CodeList_of_Box2l_(ref v); } },

            { typeof(Vector<Box2l>), (c,o) => { var v = (Vector<Box2l>)o; c.CodeVector_of_Box2l_(ref v); } },
            { typeof(Vector<Box2l>[]), (c,o) => { var v = (Vector<Box2l>[])o; c.CodeVector_of_Box2l_Array(ref v); } },
            { typeof(List<Vector<Box2l>>), (c,o) => { var v = (List<Vector<Box2l>>)o; c.CodeList_of_Vector_of_Box2l__(ref v); } },

            { typeof(Matrix<Box2l>), (c,o) => { var v = (Matrix<Box2l>)o; c.CodeMatrix_of_Box2l_(ref v); } },
            { typeof(Matrix<Box2l>[]), (c,o) => { var v = (Matrix<Box2l>[])o; c.CodeMatrix_of_Box2l_Array(ref v); } },
            { typeof(List<Matrix<Box2l>>), (c,o) => { var v = (List<Matrix<Box2l>>)o; c.CodeList_of_Matrix_of_Box2l__(ref v); } },

            { typeof(Volume<Box2l>), (c,o) => { var v = (Volume<Box2l>)o; c.CodeVolume_of_Box2l_(ref v); } },
            { typeof(Volume<Box2l>[]), (c,o) => { var v = (Volume<Box2l>[])o; c.CodeVolume_of_Box2l_Array(ref v); } },
            { typeof(List<Volume<Box2l>>), (c,o) => { var v = (List<Volume<Box2l>>)o; c.CodeList_of_Volume_of_Box2l__(ref v); } },

            { typeof(Tensor<Box2l>), (c,o) => { var v = (Tensor<Box2l>)o; c.CodeTensor_of_Box2l_(ref v); } },
            { typeof(Tensor<Box2l>[]), (c,o) => { var v = (Tensor<Box2l>[])o; c.CodeTensor_of_Box2l_Array(ref v); } },
            { typeof(List<Tensor<Box2l>>), (c,o) => { var v = (List<Tensor<Box2l>>)o; c.CodeList_of_Tensor_of_Box2l__(ref v); } },

            #endregion

            #region Box2f

            { typeof(Box2f), (c,o) => { var v = (Box2f)o; c.CodeBox2f(ref v); } },
            { typeof(Box2f[]), (c,o) => { var v = (Box2f[])o; c.CodeBox2fArray(ref v); } },
            { typeof(List<Box2f>), (c,o) => { var v = (List<Box2f>)o; c.CodeList_of_Box2f_(ref v); } },

            { typeof(Vector<Box2f>), (c,o) => { var v = (Vector<Box2f>)o; c.CodeVector_of_Box2f_(ref v); } },
            { typeof(Vector<Box2f>[]), (c,o) => { var v = (Vector<Box2f>[])o; c.CodeVector_of_Box2f_Array(ref v); } },
            { typeof(List<Vector<Box2f>>), (c,o) => { var v = (List<Vector<Box2f>>)o; c.CodeList_of_Vector_of_Box2f__(ref v); } },

            { typeof(Matrix<Box2f>), (c,o) => { var v = (Matrix<Box2f>)o; c.CodeMatrix_of_Box2f_(ref v); } },
            { typeof(Matrix<Box2f>[]), (c,o) => { var v = (Matrix<Box2f>[])o; c.CodeMatrix_of_Box2f_Array(ref v); } },
            { typeof(List<Matrix<Box2f>>), (c,o) => { var v = (List<Matrix<Box2f>>)o; c.CodeList_of_Matrix_of_Box2f__(ref v); } },

            { typeof(Volume<Box2f>), (c,o) => { var v = (Volume<Box2f>)o; c.CodeVolume_of_Box2f_(ref v); } },
            { typeof(Volume<Box2f>[]), (c,o) => { var v = (Volume<Box2f>[])o; c.CodeVolume_of_Box2f_Array(ref v); } },
            { typeof(List<Volume<Box2f>>), (c,o) => { var v = (List<Volume<Box2f>>)o; c.CodeList_of_Volume_of_Box2f__(ref v); } },

            { typeof(Tensor<Box2f>), (c,o) => { var v = (Tensor<Box2f>)o; c.CodeTensor_of_Box2f_(ref v); } },
            { typeof(Tensor<Box2f>[]), (c,o) => { var v = (Tensor<Box2f>[])o; c.CodeTensor_of_Box2f_Array(ref v); } },
            { typeof(List<Tensor<Box2f>>), (c,o) => { var v = (List<Tensor<Box2f>>)o; c.CodeList_of_Tensor_of_Box2f__(ref v); } },

            #endregion

            #region Box2d

            { typeof(Box2d), (c,o) => { var v = (Box2d)o; c.CodeBox2d(ref v); } },
            { typeof(Box2d[]), (c,o) => { var v = (Box2d[])o; c.CodeBox2dArray(ref v); } },
            { typeof(List<Box2d>), (c,o) => { var v = (List<Box2d>)o; c.CodeList_of_Box2d_(ref v); } },

            { typeof(Vector<Box2d>), (c,o) => { var v = (Vector<Box2d>)o; c.CodeVector_of_Box2d_(ref v); } },
            { typeof(Vector<Box2d>[]), (c,o) => { var v = (Vector<Box2d>[])o; c.CodeVector_of_Box2d_Array(ref v); } },
            { typeof(List<Vector<Box2d>>), (c,o) => { var v = (List<Vector<Box2d>>)o; c.CodeList_of_Vector_of_Box2d__(ref v); } },

            { typeof(Matrix<Box2d>), (c,o) => { var v = (Matrix<Box2d>)o; c.CodeMatrix_of_Box2d_(ref v); } },
            { typeof(Matrix<Box2d>[]), (c,o) => { var v = (Matrix<Box2d>[])o; c.CodeMatrix_of_Box2d_Array(ref v); } },
            { typeof(List<Matrix<Box2d>>), (c,o) => { var v = (List<Matrix<Box2d>>)o; c.CodeList_of_Matrix_of_Box2d__(ref v); } },

            { typeof(Volume<Box2d>), (c,o) => { var v = (Volume<Box2d>)o; c.CodeVolume_of_Box2d_(ref v); } },
            { typeof(Volume<Box2d>[]), (c,o) => { var v = (Volume<Box2d>[])o; c.CodeVolume_of_Box2d_Array(ref v); } },
            { typeof(List<Volume<Box2d>>), (c,o) => { var v = (List<Volume<Box2d>>)o; c.CodeList_of_Volume_of_Box2d__(ref v); } },

            { typeof(Tensor<Box2d>), (c,o) => { var v = (Tensor<Box2d>)o; c.CodeTensor_of_Box2d_(ref v); } },
            { typeof(Tensor<Box2d>[]), (c,o) => { var v = (Tensor<Box2d>[])o; c.CodeTensor_of_Box2d_Array(ref v); } },
            { typeof(List<Tensor<Box2d>>), (c,o) => { var v = (List<Tensor<Box2d>>)o; c.CodeList_of_Tensor_of_Box2d__(ref v); } },

            #endregion

            #region Box3i

            { typeof(Box3i), (c,o) => { var v = (Box3i)o; c.CodeBox3i(ref v); } },
            { typeof(Box3i[]), (c,o) => { var v = (Box3i[])o; c.CodeBox3iArray(ref v); } },
            { typeof(List<Box3i>), (c,o) => { var v = (List<Box3i>)o; c.CodeList_of_Box3i_(ref v); } },

            { typeof(Vector<Box3i>), (c,o) => { var v = (Vector<Box3i>)o; c.CodeVector_of_Box3i_(ref v); } },
            { typeof(Vector<Box3i>[]), (c,o) => { var v = (Vector<Box3i>[])o; c.CodeVector_of_Box3i_Array(ref v); } },
            { typeof(List<Vector<Box3i>>), (c,o) => { var v = (List<Vector<Box3i>>)o; c.CodeList_of_Vector_of_Box3i__(ref v); } },

            { typeof(Matrix<Box3i>), (c,o) => { var v = (Matrix<Box3i>)o; c.CodeMatrix_of_Box3i_(ref v); } },
            { typeof(Matrix<Box3i>[]), (c,o) => { var v = (Matrix<Box3i>[])o; c.CodeMatrix_of_Box3i_Array(ref v); } },
            { typeof(List<Matrix<Box3i>>), (c,o) => { var v = (List<Matrix<Box3i>>)o; c.CodeList_of_Matrix_of_Box3i__(ref v); } },

            { typeof(Volume<Box3i>), (c,o) => { var v = (Volume<Box3i>)o; c.CodeVolume_of_Box3i_(ref v); } },
            { typeof(Volume<Box3i>[]), (c,o) => { var v = (Volume<Box3i>[])o; c.CodeVolume_of_Box3i_Array(ref v); } },
            { typeof(List<Volume<Box3i>>), (c,o) => { var v = (List<Volume<Box3i>>)o; c.CodeList_of_Volume_of_Box3i__(ref v); } },

            { typeof(Tensor<Box3i>), (c,o) => { var v = (Tensor<Box3i>)o; c.CodeTensor_of_Box3i_(ref v); } },
            { typeof(Tensor<Box3i>[]), (c,o) => { var v = (Tensor<Box3i>[])o; c.CodeTensor_of_Box3i_Array(ref v); } },
            { typeof(List<Tensor<Box3i>>), (c,o) => { var v = (List<Tensor<Box3i>>)o; c.CodeList_of_Tensor_of_Box3i__(ref v); } },

            #endregion

            #region Box3l

            { typeof(Box3l), (c,o) => { var v = (Box3l)o; c.CodeBox3l(ref v); } },
            { typeof(Box3l[]), (c,o) => { var v = (Box3l[])o; c.CodeBox3lArray(ref v); } },
            { typeof(List<Box3l>), (c,o) => { var v = (List<Box3l>)o; c.CodeList_of_Box3l_(ref v); } },

            { typeof(Vector<Box3l>), (c,o) => { var v = (Vector<Box3l>)o; c.CodeVector_of_Box3l_(ref v); } },
            { typeof(Vector<Box3l>[]), (c,o) => { var v = (Vector<Box3l>[])o; c.CodeVector_of_Box3l_Array(ref v); } },
            { typeof(List<Vector<Box3l>>), (c,o) => { var v = (List<Vector<Box3l>>)o; c.CodeList_of_Vector_of_Box3l__(ref v); } },

            { typeof(Matrix<Box3l>), (c,o) => { var v = (Matrix<Box3l>)o; c.CodeMatrix_of_Box3l_(ref v); } },
            { typeof(Matrix<Box3l>[]), (c,o) => { var v = (Matrix<Box3l>[])o; c.CodeMatrix_of_Box3l_Array(ref v); } },
            { typeof(List<Matrix<Box3l>>), (c,o) => { var v = (List<Matrix<Box3l>>)o; c.CodeList_of_Matrix_of_Box3l__(ref v); } },

            { typeof(Volume<Box3l>), (c,o) => { var v = (Volume<Box3l>)o; c.CodeVolume_of_Box3l_(ref v); } },
            { typeof(Volume<Box3l>[]), (c,o) => { var v = (Volume<Box3l>[])o; c.CodeVolume_of_Box3l_Array(ref v); } },
            { typeof(List<Volume<Box3l>>), (c,o) => { var v = (List<Volume<Box3l>>)o; c.CodeList_of_Volume_of_Box3l__(ref v); } },

            { typeof(Tensor<Box3l>), (c,o) => { var v = (Tensor<Box3l>)o; c.CodeTensor_of_Box3l_(ref v); } },
            { typeof(Tensor<Box3l>[]), (c,o) => { var v = (Tensor<Box3l>[])o; c.CodeTensor_of_Box3l_Array(ref v); } },
            { typeof(List<Tensor<Box3l>>), (c,o) => { var v = (List<Tensor<Box3l>>)o; c.CodeList_of_Tensor_of_Box3l__(ref v); } },

            #endregion

            #region Box3f

            { typeof(Box3f), (c,o) => { var v = (Box3f)o; c.CodeBox3f(ref v); } },
            { typeof(Box3f[]), (c,o) => { var v = (Box3f[])o; c.CodeBox3fArray(ref v); } },
            { typeof(List<Box3f>), (c,o) => { var v = (List<Box3f>)o; c.CodeList_of_Box3f_(ref v); } },

            { typeof(Vector<Box3f>), (c,o) => { var v = (Vector<Box3f>)o; c.CodeVector_of_Box3f_(ref v); } },
            { typeof(Vector<Box3f>[]), (c,o) => { var v = (Vector<Box3f>[])o; c.CodeVector_of_Box3f_Array(ref v); } },
            { typeof(List<Vector<Box3f>>), (c,o) => { var v = (List<Vector<Box3f>>)o; c.CodeList_of_Vector_of_Box3f__(ref v); } },

            { typeof(Matrix<Box3f>), (c,o) => { var v = (Matrix<Box3f>)o; c.CodeMatrix_of_Box3f_(ref v); } },
            { typeof(Matrix<Box3f>[]), (c,o) => { var v = (Matrix<Box3f>[])o; c.CodeMatrix_of_Box3f_Array(ref v); } },
            { typeof(List<Matrix<Box3f>>), (c,o) => { var v = (List<Matrix<Box3f>>)o; c.CodeList_of_Matrix_of_Box3f__(ref v); } },

            { typeof(Volume<Box3f>), (c,o) => { var v = (Volume<Box3f>)o; c.CodeVolume_of_Box3f_(ref v); } },
            { typeof(Volume<Box3f>[]), (c,o) => { var v = (Volume<Box3f>[])o; c.CodeVolume_of_Box3f_Array(ref v); } },
            { typeof(List<Volume<Box3f>>), (c,o) => { var v = (List<Volume<Box3f>>)o; c.CodeList_of_Volume_of_Box3f__(ref v); } },

            { typeof(Tensor<Box3f>), (c,o) => { var v = (Tensor<Box3f>)o; c.CodeTensor_of_Box3f_(ref v); } },
            { typeof(Tensor<Box3f>[]), (c,o) => { var v = (Tensor<Box3f>[])o; c.CodeTensor_of_Box3f_Array(ref v); } },
            { typeof(List<Tensor<Box3f>>), (c,o) => { var v = (List<Tensor<Box3f>>)o; c.CodeList_of_Tensor_of_Box3f__(ref v); } },

            #endregion

            #region Box3d

            { typeof(Box3d), (c,o) => { var v = (Box3d)o; c.CodeBox3d(ref v); } },
            { typeof(Box3d[]), (c,o) => { var v = (Box3d[])o; c.CodeBox3dArray(ref v); } },
            { typeof(List<Box3d>), (c,o) => { var v = (List<Box3d>)o; c.CodeList_of_Box3d_(ref v); } },

            { typeof(Vector<Box3d>), (c,o) => { var v = (Vector<Box3d>)o; c.CodeVector_of_Box3d_(ref v); } },
            { typeof(Vector<Box3d>[]), (c,o) => { var v = (Vector<Box3d>[])o; c.CodeVector_of_Box3d_Array(ref v); } },
            { typeof(List<Vector<Box3d>>), (c,o) => { var v = (List<Vector<Box3d>>)o; c.CodeList_of_Vector_of_Box3d__(ref v); } },

            { typeof(Matrix<Box3d>), (c,o) => { var v = (Matrix<Box3d>)o; c.CodeMatrix_of_Box3d_(ref v); } },
            { typeof(Matrix<Box3d>[]), (c,o) => { var v = (Matrix<Box3d>[])o; c.CodeMatrix_of_Box3d_Array(ref v); } },
            { typeof(List<Matrix<Box3d>>), (c,o) => { var v = (List<Matrix<Box3d>>)o; c.CodeList_of_Matrix_of_Box3d__(ref v); } },

            { typeof(Volume<Box3d>), (c,o) => { var v = (Volume<Box3d>)o; c.CodeVolume_of_Box3d_(ref v); } },
            { typeof(Volume<Box3d>[]), (c,o) => { var v = (Volume<Box3d>[])o; c.CodeVolume_of_Box3d_Array(ref v); } },
            { typeof(List<Volume<Box3d>>), (c,o) => { var v = (List<Volume<Box3d>>)o; c.CodeList_of_Volume_of_Box3d__(ref v); } },

            { typeof(Tensor<Box3d>), (c,o) => { var v = (Tensor<Box3d>)o; c.CodeTensor_of_Box3d_(ref v); } },
            { typeof(Tensor<Box3d>[]), (c,o) => { var v = (Tensor<Box3d>[])o; c.CodeTensor_of_Box3d_Array(ref v); } },
            { typeof(List<Tensor<Box3d>>), (c,o) => { var v = (List<Tensor<Box3d>>)o; c.CodeList_of_Tensor_of_Box3d__(ref v); } },

            #endregion

            #region Euclidean3f

            { typeof(Euclidean3f), (c,o) => { var v = (Euclidean3f)o; c.CodeEuclidean3f(ref v); } },
            { typeof(Euclidean3f[]), (c,o) => { var v = (Euclidean3f[])o; c.CodeEuclidean3fArray(ref v); } },
            { typeof(List<Euclidean3f>), (c,o) => { var v = (List<Euclidean3f>)o; c.CodeList_of_Euclidean3f_(ref v); } },

            { typeof(Vector<Euclidean3f>), (c,o) => { var v = (Vector<Euclidean3f>)o; c.CodeVector_of_Euclidean3f_(ref v); } },
            { typeof(Vector<Euclidean3f>[]), (c,o) => { var v = (Vector<Euclidean3f>[])o; c.CodeVector_of_Euclidean3f_Array(ref v); } },
            { typeof(List<Vector<Euclidean3f>>), (c,o) => { var v = (List<Vector<Euclidean3f>>)o; c.CodeList_of_Vector_of_Euclidean3f__(ref v); } },

            { typeof(Matrix<Euclidean3f>), (c,o) => { var v = (Matrix<Euclidean3f>)o; c.CodeMatrix_of_Euclidean3f_(ref v); } },
            { typeof(Matrix<Euclidean3f>[]), (c,o) => { var v = (Matrix<Euclidean3f>[])o; c.CodeMatrix_of_Euclidean3f_Array(ref v); } },
            { typeof(List<Matrix<Euclidean3f>>), (c,o) => { var v = (List<Matrix<Euclidean3f>>)o; c.CodeList_of_Matrix_of_Euclidean3f__(ref v); } },

            { typeof(Volume<Euclidean3f>), (c,o) => { var v = (Volume<Euclidean3f>)o; c.CodeVolume_of_Euclidean3f_(ref v); } },
            { typeof(Volume<Euclidean3f>[]), (c,o) => { var v = (Volume<Euclidean3f>[])o; c.CodeVolume_of_Euclidean3f_Array(ref v); } },
            { typeof(List<Volume<Euclidean3f>>), (c,o) => { var v = (List<Volume<Euclidean3f>>)o; c.CodeList_of_Volume_of_Euclidean3f__(ref v); } },

            { typeof(Tensor<Euclidean3f>), (c,o) => { var v = (Tensor<Euclidean3f>)o; c.CodeTensor_of_Euclidean3f_(ref v); } },
            { typeof(Tensor<Euclidean3f>[]), (c,o) => { var v = (Tensor<Euclidean3f>[])o; c.CodeTensor_of_Euclidean3f_Array(ref v); } },
            { typeof(List<Tensor<Euclidean3f>>), (c,o) => { var v = (List<Tensor<Euclidean3f>>)o; c.CodeList_of_Tensor_of_Euclidean3f__(ref v); } },

            #endregion

            #region Euclidean3d

            { typeof(Euclidean3d), (c,o) => { var v = (Euclidean3d)o; c.CodeEuclidean3d(ref v); } },
            { typeof(Euclidean3d[]), (c,o) => { var v = (Euclidean3d[])o; c.CodeEuclidean3dArray(ref v); } },
            { typeof(List<Euclidean3d>), (c,o) => { var v = (List<Euclidean3d>)o; c.CodeList_of_Euclidean3d_(ref v); } },

            { typeof(Vector<Euclidean3d>), (c,o) => { var v = (Vector<Euclidean3d>)o; c.CodeVector_of_Euclidean3d_(ref v); } },
            { typeof(Vector<Euclidean3d>[]), (c,o) => { var v = (Vector<Euclidean3d>[])o; c.CodeVector_of_Euclidean3d_Array(ref v); } },
            { typeof(List<Vector<Euclidean3d>>), (c,o) => { var v = (List<Vector<Euclidean3d>>)o; c.CodeList_of_Vector_of_Euclidean3d__(ref v); } },

            { typeof(Matrix<Euclidean3d>), (c,o) => { var v = (Matrix<Euclidean3d>)o; c.CodeMatrix_of_Euclidean3d_(ref v); } },
            { typeof(Matrix<Euclidean3d>[]), (c,o) => { var v = (Matrix<Euclidean3d>[])o; c.CodeMatrix_of_Euclidean3d_Array(ref v); } },
            { typeof(List<Matrix<Euclidean3d>>), (c,o) => { var v = (List<Matrix<Euclidean3d>>)o; c.CodeList_of_Matrix_of_Euclidean3d__(ref v); } },

            { typeof(Volume<Euclidean3d>), (c,o) => { var v = (Volume<Euclidean3d>)o; c.CodeVolume_of_Euclidean3d_(ref v); } },
            { typeof(Volume<Euclidean3d>[]), (c,o) => { var v = (Volume<Euclidean3d>[])o; c.CodeVolume_of_Euclidean3d_Array(ref v); } },
            { typeof(List<Volume<Euclidean3d>>), (c,o) => { var v = (List<Volume<Euclidean3d>>)o; c.CodeList_of_Volume_of_Euclidean3d__(ref v); } },

            { typeof(Tensor<Euclidean3d>), (c,o) => { var v = (Tensor<Euclidean3d>)o; c.CodeTensor_of_Euclidean3d_(ref v); } },
            { typeof(Tensor<Euclidean3d>[]), (c,o) => { var v = (Tensor<Euclidean3d>[])o; c.CodeTensor_of_Euclidean3d_Array(ref v); } },
            { typeof(List<Tensor<Euclidean3d>>), (c,o) => { var v = (List<Tensor<Euclidean3d>>)o; c.CodeList_of_Tensor_of_Euclidean3d__(ref v); } },

            #endregion

            #region Rot2f

            { typeof(Rot2f), (c,o) => { var v = (Rot2f)o; c.CodeRot2f(ref v); } },
            { typeof(Rot2f[]), (c,o) => { var v = (Rot2f[])o; c.CodeRot2fArray(ref v); } },
            { typeof(List<Rot2f>), (c,o) => { var v = (List<Rot2f>)o; c.CodeList_of_Rot2f_(ref v); } },

            { typeof(Vector<Rot2f>), (c,o) => { var v = (Vector<Rot2f>)o; c.CodeVector_of_Rot2f_(ref v); } },
            { typeof(Vector<Rot2f>[]), (c,o) => { var v = (Vector<Rot2f>[])o; c.CodeVector_of_Rot2f_Array(ref v); } },
            { typeof(List<Vector<Rot2f>>), (c,o) => { var v = (List<Vector<Rot2f>>)o; c.CodeList_of_Vector_of_Rot2f__(ref v); } },

            { typeof(Matrix<Rot2f>), (c,o) => { var v = (Matrix<Rot2f>)o; c.CodeMatrix_of_Rot2f_(ref v); } },
            { typeof(Matrix<Rot2f>[]), (c,o) => { var v = (Matrix<Rot2f>[])o; c.CodeMatrix_of_Rot2f_Array(ref v); } },
            { typeof(List<Matrix<Rot2f>>), (c,o) => { var v = (List<Matrix<Rot2f>>)o; c.CodeList_of_Matrix_of_Rot2f__(ref v); } },

            { typeof(Volume<Rot2f>), (c,o) => { var v = (Volume<Rot2f>)o; c.CodeVolume_of_Rot2f_(ref v); } },
            { typeof(Volume<Rot2f>[]), (c,o) => { var v = (Volume<Rot2f>[])o; c.CodeVolume_of_Rot2f_Array(ref v); } },
            { typeof(List<Volume<Rot2f>>), (c,o) => { var v = (List<Volume<Rot2f>>)o; c.CodeList_of_Volume_of_Rot2f__(ref v); } },

            { typeof(Tensor<Rot2f>), (c,o) => { var v = (Tensor<Rot2f>)o; c.CodeTensor_of_Rot2f_(ref v); } },
            { typeof(Tensor<Rot2f>[]), (c,o) => { var v = (Tensor<Rot2f>[])o; c.CodeTensor_of_Rot2f_Array(ref v); } },
            { typeof(List<Tensor<Rot2f>>), (c,o) => { var v = (List<Tensor<Rot2f>>)o; c.CodeList_of_Tensor_of_Rot2f__(ref v); } },

            #endregion

            #region Rot2d

            { typeof(Rot2d), (c,o) => { var v = (Rot2d)o; c.CodeRot2d(ref v); } },
            { typeof(Rot2d[]), (c,o) => { var v = (Rot2d[])o; c.CodeRot2dArray(ref v); } },
            { typeof(List<Rot2d>), (c,o) => { var v = (List<Rot2d>)o; c.CodeList_of_Rot2d_(ref v); } },

            { typeof(Vector<Rot2d>), (c,o) => { var v = (Vector<Rot2d>)o; c.CodeVector_of_Rot2d_(ref v); } },
            { typeof(Vector<Rot2d>[]), (c,o) => { var v = (Vector<Rot2d>[])o; c.CodeVector_of_Rot2d_Array(ref v); } },
            { typeof(List<Vector<Rot2d>>), (c,o) => { var v = (List<Vector<Rot2d>>)o; c.CodeList_of_Vector_of_Rot2d__(ref v); } },

            { typeof(Matrix<Rot2d>), (c,o) => { var v = (Matrix<Rot2d>)o; c.CodeMatrix_of_Rot2d_(ref v); } },
            { typeof(Matrix<Rot2d>[]), (c,o) => { var v = (Matrix<Rot2d>[])o; c.CodeMatrix_of_Rot2d_Array(ref v); } },
            { typeof(List<Matrix<Rot2d>>), (c,o) => { var v = (List<Matrix<Rot2d>>)o; c.CodeList_of_Matrix_of_Rot2d__(ref v); } },

            { typeof(Volume<Rot2d>), (c,o) => { var v = (Volume<Rot2d>)o; c.CodeVolume_of_Rot2d_(ref v); } },
            { typeof(Volume<Rot2d>[]), (c,o) => { var v = (Volume<Rot2d>[])o; c.CodeVolume_of_Rot2d_Array(ref v); } },
            { typeof(List<Volume<Rot2d>>), (c,o) => { var v = (List<Volume<Rot2d>>)o; c.CodeList_of_Volume_of_Rot2d__(ref v); } },

            { typeof(Tensor<Rot2d>), (c,o) => { var v = (Tensor<Rot2d>)o; c.CodeTensor_of_Rot2d_(ref v); } },
            { typeof(Tensor<Rot2d>[]), (c,o) => { var v = (Tensor<Rot2d>[])o; c.CodeTensor_of_Rot2d_Array(ref v); } },
            { typeof(List<Tensor<Rot2d>>), (c,o) => { var v = (List<Tensor<Rot2d>>)o; c.CodeList_of_Tensor_of_Rot2d__(ref v); } },

            #endregion

            #region Rot3f

            { typeof(Rot3f), (c,o) => { var v = (Rot3f)o; c.CodeRot3f(ref v); } },
            { typeof(Rot3f[]), (c,o) => { var v = (Rot3f[])o; c.CodeRot3fArray(ref v); } },
            { typeof(List<Rot3f>), (c,o) => { var v = (List<Rot3f>)o; c.CodeList_of_Rot3f_(ref v); } },

            { typeof(Vector<Rot3f>), (c,o) => { var v = (Vector<Rot3f>)o; c.CodeVector_of_Rot3f_(ref v); } },
            { typeof(Vector<Rot3f>[]), (c,o) => { var v = (Vector<Rot3f>[])o; c.CodeVector_of_Rot3f_Array(ref v); } },
            { typeof(List<Vector<Rot3f>>), (c,o) => { var v = (List<Vector<Rot3f>>)o; c.CodeList_of_Vector_of_Rot3f__(ref v); } },

            { typeof(Matrix<Rot3f>), (c,o) => { var v = (Matrix<Rot3f>)o; c.CodeMatrix_of_Rot3f_(ref v); } },
            { typeof(Matrix<Rot3f>[]), (c,o) => { var v = (Matrix<Rot3f>[])o; c.CodeMatrix_of_Rot3f_Array(ref v); } },
            { typeof(List<Matrix<Rot3f>>), (c,o) => { var v = (List<Matrix<Rot3f>>)o; c.CodeList_of_Matrix_of_Rot3f__(ref v); } },

            { typeof(Volume<Rot3f>), (c,o) => { var v = (Volume<Rot3f>)o; c.CodeVolume_of_Rot3f_(ref v); } },
            { typeof(Volume<Rot3f>[]), (c,o) => { var v = (Volume<Rot3f>[])o; c.CodeVolume_of_Rot3f_Array(ref v); } },
            { typeof(List<Volume<Rot3f>>), (c,o) => { var v = (List<Volume<Rot3f>>)o; c.CodeList_of_Volume_of_Rot3f__(ref v); } },

            { typeof(Tensor<Rot3f>), (c,o) => { var v = (Tensor<Rot3f>)o; c.CodeTensor_of_Rot3f_(ref v); } },
            { typeof(Tensor<Rot3f>[]), (c,o) => { var v = (Tensor<Rot3f>[])o; c.CodeTensor_of_Rot3f_Array(ref v); } },
            { typeof(List<Tensor<Rot3f>>), (c,o) => { var v = (List<Tensor<Rot3f>>)o; c.CodeList_of_Tensor_of_Rot3f__(ref v); } },

            #endregion

            #region Rot3d

            { typeof(Rot3d), (c,o) => { var v = (Rot3d)o; c.CodeRot3d(ref v); } },
            { typeof(Rot3d[]), (c,o) => { var v = (Rot3d[])o; c.CodeRot3dArray(ref v); } },
            { typeof(List<Rot3d>), (c,o) => { var v = (List<Rot3d>)o; c.CodeList_of_Rot3d_(ref v); } },

            { typeof(Vector<Rot3d>), (c,o) => { var v = (Vector<Rot3d>)o; c.CodeVector_of_Rot3d_(ref v); } },
            { typeof(Vector<Rot3d>[]), (c,o) => { var v = (Vector<Rot3d>[])o; c.CodeVector_of_Rot3d_Array(ref v); } },
            { typeof(List<Vector<Rot3d>>), (c,o) => { var v = (List<Vector<Rot3d>>)o; c.CodeList_of_Vector_of_Rot3d__(ref v); } },

            { typeof(Matrix<Rot3d>), (c,o) => { var v = (Matrix<Rot3d>)o; c.CodeMatrix_of_Rot3d_(ref v); } },
            { typeof(Matrix<Rot3d>[]), (c,o) => { var v = (Matrix<Rot3d>[])o; c.CodeMatrix_of_Rot3d_Array(ref v); } },
            { typeof(List<Matrix<Rot3d>>), (c,o) => { var v = (List<Matrix<Rot3d>>)o; c.CodeList_of_Matrix_of_Rot3d__(ref v); } },

            { typeof(Volume<Rot3d>), (c,o) => { var v = (Volume<Rot3d>)o; c.CodeVolume_of_Rot3d_(ref v); } },
            { typeof(Volume<Rot3d>[]), (c,o) => { var v = (Volume<Rot3d>[])o; c.CodeVolume_of_Rot3d_Array(ref v); } },
            { typeof(List<Volume<Rot3d>>), (c,o) => { var v = (List<Volume<Rot3d>>)o; c.CodeList_of_Volume_of_Rot3d__(ref v); } },

            { typeof(Tensor<Rot3d>), (c,o) => { var v = (Tensor<Rot3d>)o; c.CodeTensor_of_Rot3d_(ref v); } },
            { typeof(Tensor<Rot3d>[]), (c,o) => { var v = (Tensor<Rot3d>[])o; c.CodeTensor_of_Rot3d_Array(ref v); } },
            { typeof(List<Tensor<Rot3d>>), (c,o) => { var v = (List<Tensor<Rot3d>>)o; c.CodeList_of_Tensor_of_Rot3d__(ref v); } },

            #endregion

            #region Scale3f

            { typeof(Scale3f), (c,o) => { var v = (Scale3f)o; c.CodeScale3f(ref v); } },
            { typeof(Scale3f[]), (c,o) => { var v = (Scale3f[])o; c.CodeScale3fArray(ref v); } },
            { typeof(List<Scale3f>), (c,o) => { var v = (List<Scale3f>)o; c.CodeList_of_Scale3f_(ref v); } },

            { typeof(Vector<Scale3f>), (c,o) => { var v = (Vector<Scale3f>)o; c.CodeVector_of_Scale3f_(ref v); } },
            { typeof(Vector<Scale3f>[]), (c,o) => { var v = (Vector<Scale3f>[])o; c.CodeVector_of_Scale3f_Array(ref v); } },
            { typeof(List<Vector<Scale3f>>), (c,o) => { var v = (List<Vector<Scale3f>>)o; c.CodeList_of_Vector_of_Scale3f__(ref v); } },

            { typeof(Matrix<Scale3f>), (c,o) => { var v = (Matrix<Scale3f>)o; c.CodeMatrix_of_Scale3f_(ref v); } },
            { typeof(Matrix<Scale3f>[]), (c,o) => { var v = (Matrix<Scale3f>[])o; c.CodeMatrix_of_Scale3f_Array(ref v); } },
            { typeof(List<Matrix<Scale3f>>), (c,o) => { var v = (List<Matrix<Scale3f>>)o; c.CodeList_of_Matrix_of_Scale3f__(ref v); } },

            { typeof(Volume<Scale3f>), (c,o) => { var v = (Volume<Scale3f>)o; c.CodeVolume_of_Scale3f_(ref v); } },
            { typeof(Volume<Scale3f>[]), (c,o) => { var v = (Volume<Scale3f>[])o; c.CodeVolume_of_Scale3f_Array(ref v); } },
            { typeof(List<Volume<Scale3f>>), (c,o) => { var v = (List<Volume<Scale3f>>)o; c.CodeList_of_Volume_of_Scale3f__(ref v); } },

            { typeof(Tensor<Scale3f>), (c,o) => { var v = (Tensor<Scale3f>)o; c.CodeTensor_of_Scale3f_(ref v); } },
            { typeof(Tensor<Scale3f>[]), (c,o) => { var v = (Tensor<Scale3f>[])o; c.CodeTensor_of_Scale3f_Array(ref v); } },
            { typeof(List<Tensor<Scale3f>>), (c,o) => { var v = (List<Tensor<Scale3f>>)o; c.CodeList_of_Tensor_of_Scale3f__(ref v); } },

            #endregion

            #region Scale3d

            { typeof(Scale3d), (c,o) => { var v = (Scale3d)o; c.CodeScale3d(ref v); } },
            { typeof(Scale3d[]), (c,o) => { var v = (Scale3d[])o; c.CodeScale3dArray(ref v); } },
            { typeof(List<Scale3d>), (c,o) => { var v = (List<Scale3d>)o; c.CodeList_of_Scale3d_(ref v); } },

            { typeof(Vector<Scale3d>), (c,o) => { var v = (Vector<Scale3d>)o; c.CodeVector_of_Scale3d_(ref v); } },
            { typeof(Vector<Scale3d>[]), (c,o) => { var v = (Vector<Scale3d>[])o; c.CodeVector_of_Scale3d_Array(ref v); } },
            { typeof(List<Vector<Scale3d>>), (c,o) => { var v = (List<Vector<Scale3d>>)o; c.CodeList_of_Vector_of_Scale3d__(ref v); } },

            { typeof(Matrix<Scale3d>), (c,o) => { var v = (Matrix<Scale3d>)o; c.CodeMatrix_of_Scale3d_(ref v); } },
            { typeof(Matrix<Scale3d>[]), (c,o) => { var v = (Matrix<Scale3d>[])o; c.CodeMatrix_of_Scale3d_Array(ref v); } },
            { typeof(List<Matrix<Scale3d>>), (c,o) => { var v = (List<Matrix<Scale3d>>)o; c.CodeList_of_Matrix_of_Scale3d__(ref v); } },

            { typeof(Volume<Scale3d>), (c,o) => { var v = (Volume<Scale3d>)o; c.CodeVolume_of_Scale3d_(ref v); } },
            { typeof(Volume<Scale3d>[]), (c,o) => { var v = (Volume<Scale3d>[])o; c.CodeVolume_of_Scale3d_Array(ref v); } },
            { typeof(List<Volume<Scale3d>>), (c,o) => { var v = (List<Volume<Scale3d>>)o; c.CodeList_of_Volume_of_Scale3d__(ref v); } },

            { typeof(Tensor<Scale3d>), (c,o) => { var v = (Tensor<Scale3d>)o; c.CodeTensor_of_Scale3d_(ref v); } },
            { typeof(Tensor<Scale3d>[]), (c,o) => { var v = (Tensor<Scale3d>[])o; c.CodeTensor_of_Scale3d_Array(ref v); } },
            { typeof(List<Tensor<Scale3d>>), (c,o) => { var v = (List<Tensor<Scale3d>>)o; c.CodeList_of_Tensor_of_Scale3d__(ref v); } },

            #endregion

            #region Shift3f

            { typeof(Shift3f), (c,o) => { var v = (Shift3f)o; c.CodeShift3f(ref v); } },
            { typeof(Shift3f[]), (c,o) => { var v = (Shift3f[])o; c.CodeShift3fArray(ref v); } },
            { typeof(List<Shift3f>), (c,o) => { var v = (List<Shift3f>)o; c.CodeList_of_Shift3f_(ref v); } },

            { typeof(Vector<Shift3f>), (c,o) => { var v = (Vector<Shift3f>)o; c.CodeVector_of_Shift3f_(ref v); } },
            { typeof(Vector<Shift3f>[]), (c,o) => { var v = (Vector<Shift3f>[])o; c.CodeVector_of_Shift3f_Array(ref v); } },
            { typeof(List<Vector<Shift3f>>), (c,o) => { var v = (List<Vector<Shift3f>>)o; c.CodeList_of_Vector_of_Shift3f__(ref v); } },

            { typeof(Matrix<Shift3f>), (c,o) => { var v = (Matrix<Shift3f>)o; c.CodeMatrix_of_Shift3f_(ref v); } },
            { typeof(Matrix<Shift3f>[]), (c,o) => { var v = (Matrix<Shift3f>[])o; c.CodeMatrix_of_Shift3f_Array(ref v); } },
            { typeof(List<Matrix<Shift3f>>), (c,o) => { var v = (List<Matrix<Shift3f>>)o; c.CodeList_of_Matrix_of_Shift3f__(ref v); } },

            { typeof(Volume<Shift3f>), (c,o) => { var v = (Volume<Shift3f>)o; c.CodeVolume_of_Shift3f_(ref v); } },
            { typeof(Volume<Shift3f>[]), (c,o) => { var v = (Volume<Shift3f>[])o; c.CodeVolume_of_Shift3f_Array(ref v); } },
            { typeof(List<Volume<Shift3f>>), (c,o) => { var v = (List<Volume<Shift3f>>)o; c.CodeList_of_Volume_of_Shift3f__(ref v); } },

            { typeof(Tensor<Shift3f>), (c,o) => { var v = (Tensor<Shift3f>)o; c.CodeTensor_of_Shift3f_(ref v); } },
            { typeof(Tensor<Shift3f>[]), (c,o) => { var v = (Tensor<Shift3f>[])o; c.CodeTensor_of_Shift3f_Array(ref v); } },
            { typeof(List<Tensor<Shift3f>>), (c,o) => { var v = (List<Tensor<Shift3f>>)o; c.CodeList_of_Tensor_of_Shift3f__(ref v); } },

            #endregion

            #region Shift3d

            { typeof(Shift3d), (c,o) => { var v = (Shift3d)o; c.CodeShift3d(ref v); } },
            { typeof(Shift3d[]), (c,o) => { var v = (Shift3d[])o; c.CodeShift3dArray(ref v); } },
            { typeof(List<Shift3d>), (c,o) => { var v = (List<Shift3d>)o; c.CodeList_of_Shift3d_(ref v); } },

            { typeof(Vector<Shift3d>), (c,o) => { var v = (Vector<Shift3d>)o; c.CodeVector_of_Shift3d_(ref v); } },
            { typeof(Vector<Shift3d>[]), (c,o) => { var v = (Vector<Shift3d>[])o; c.CodeVector_of_Shift3d_Array(ref v); } },
            { typeof(List<Vector<Shift3d>>), (c,o) => { var v = (List<Vector<Shift3d>>)o; c.CodeList_of_Vector_of_Shift3d__(ref v); } },

            { typeof(Matrix<Shift3d>), (c,o) => { var v = (Matrix<Shift3d>)o; c.CodeMatrix_of_Shift3d_(ref v); } },
            { typeof(Matrix<Shift3d>[]), (c,o) => { var v = (Matrix<Shift3d>[])o; c.CodeMatrix_of_Shift3d_Array(ref v); } },
            { typeof(List<Matrix<Shift3d>>), (c,o) => { var v = (List<Matrix<Shift3d>>)o; c.CodeList_of_Matrix_of_Shift3d__(ref v); } },

            { typeof(Volume<Shift3d>), (c,o) => { var v = (Volume<Shift3d>)o; c.CodeVolume_of_Shift3d_(ref v); } },
            { typeof(Volume<Shift3d>[]), (c,o) => { var v = (Volume<Shift3d>[])o; c.CodeVolume_of_Shift3d_Array(ref v); } },
            { typeof(List<Volume<Shift3d>>), (c,o) => { var v = (List<Volume<Shift3d>>)o; c.CodeList_of_Volume_of_Shift3d__(ref v); } },

            { typeof(Tensor<Shift3d>), (c,o) => { var v = (Tensor<Shift3d>)o; c.CodeTensor_of_Shift3d_(ref v); } },
            { typeof(Tensor<Shift3d>[]), (c,o) => { var v = (Tensor<Shift3d>[])o; c.CodeTensor_of_Shift3d_Array(ref v); } },
            { typeof(List<Tensor<Shift3d>>), (c,o) => { var v = (List<Tensor<Shift3d>>)o; c.CodeList_of_Tensor_of_Shift3d__(ref v); } },

            #endregion

            #region Trafo2f

            { typeof(Trafo2f), (c,o) => { var v = (Trafo2f)o; c.CodeTrafo2f(ref v); } },
            { typeof(Trafo2f[]), (c,o) => { var v = (Trafo2f[])o; c.CodeTrafo2fArray(ref v); } },
            { typeof(List<Trafo2f>), (c,o) => { var v = (List<Trafo2f>)o; c.CodeList_of_Trafo2f_(ref v); } },

            { typeof(Vector<Trafo2f>), (c,o) => { var v = (Vector<Trafo2f>)o; c.CodeVector_of_Trafo2f_(ref v); } },
            { typeof(Vector<Trafo2f>[]), (c,o) => { var v = (Vector<Trafo2f>[])o; c.CodeVector_of_Trafo2f_Array(ref v); } },
            { typeof(List<Vector<Trafo2f>>), (c,o) => { var v = (List<Vector<Trafo2f>>)o; c.CodeList_of_Vector_of_Trafo2f__(ref v); } },

            { typeof(Matrix<Trafo2f>), (c,o) => { var v = (Matrix<Trafo2f>)o; c.CodeMatrix_of_Trafo2f_(ref v); } },
            { typeof(Matrix<Trafo2f>[]), (c,o) => { var v = (Matrix<Trafo2f>[])o; c.CodeMatrix_of_Trafo2f_Array(ref v); } },
            { typeof(List<Matrix<Trafo2f>>), (c,o) => { var v = (List<Matrix<Trafo2f>>)o; c.CodeList_of_Matrix_of_Trafo2f__(ref v); } },

            { typeof(Volume<Trafo2f>), (c,o) => { var v = (Volume<Trafo2f>)o; c.CodeVolume_of_Trafo2f_(ref v); } },
            { typeof(Volume<Trafo2f>[]), (c,o) => { var v = (Volume<Trafo2f>[])o; c.CodeVolume_of_Trafo2f_Array(ref v); } },
            { typeof(List<Volume<Trafo2f>>), (c,o) => { var v = (List<Volume<Trafo2f>>)o; c.CodeList_of_Volume_of_Trafo2f__(ref v); } },

            { typeof(Tensor<Trafo2f>), (c,o) => { var v = (Tensor<Trafo2f>)o; c.CodeTensor_of_Trafo2f_(ref v); } },
            { typeof(Tensor<Trafo2f>[]), (c,o) => { var v = (Tensor<Trafo2f>[])o; c.CodeTensor_of_Trafo2f_Array(ref v); } },
            { typeof(List<Tensor<Trafo2f>>), (c,o) => { var v = (List<Tensor<Trafo2f>>)o; c.CodeList_of_Tensor_of_Trafo2f__(ref v); } },

            #endregion

            #region Trafo2d

            { typeof(Trafo2d), (c,o) => { var v = (Trafo2d)o; c.CodeTrafo2d(ref v); } },
            { typeof(Trafo2d[]), (c,o) => { var v = (Trafo2d[])o; c.CodeTrafo2dArray(ref v); } },
            { typeof(List<Trafo2d>), (c,o) => { var v = (List<Trafo2d>)o; c.CodeList_of_Trafo2d_(ref v); } },

            { typeof(Vector<Trafo2d>), (c,o) => { var v = (Vector<Trafo2d>)o; c.CodeVector_of_Trafo2d_(ref v); } },
            { typeof(Vector<Trafo2d>[]), (c,o) => { var v = (Vector<Trafo2d>[])o; c.CodeVector_of_Trafo2d_Array(ref v); } },
            { typeof(List<Vector<Trafo2d>>), (c,o) => { var v = (List<Vector<Trafo2d>>)o; c.CodeList_of_Vector_of_Trafo2d__(ref v); } },

            { typeof(Matrix<Trafo2d>), (c,o) => { var v = (Matrix<Trafo2d>)o; c.CodeMatrix_of_Trafo2d_(ref v); } },
            { typeof(Matrix<Trafo2d>[]), (c,o) => { var v = (Matrix<Trafo2d>[])o; c.CodeMatrix_of_Trafo2d_Array(ref v); } },
            { typeof(List<Matrix<Trafo2d>>), (c,o) => { var v = (List<Matrix<Trafo2d>>)o; c.CodeList_of_Matrix_of_Trafo2d__(ref v); } },

            { typeof(Volume<Trafo2d>), (c,o) => { var v = (Volume<Trafo2d>)o; c.CodeVolume_of_Trafo2d_(ref v); } },
            { typeof(Volume<Trafo2d>[]), (c,o) => { var v = (Volume<Trafo2d>[])o; c.CodeVolume_of_Trafo2d_Array(ref v); } },
            { typeof(List<Volume<Trafo2d>>), (c,o) => { var v = (List<Volume<Trafo2d>>)o; c.CodeList_of_Volume_of_Trafo2d__(ref v); } },

            { typeof(Tensor<Trafo2d>), (c,o) => { var v = (Tensor<Trafo2d>)o; c.CodeTensor_of_Trafo2d_(ref v); } },
            { typeof(Tensor<Trafo2d>[]), (c,o) => { var v = (Tensor<Trafo2d>[])o; c.CodeTensor_of_Trafo2d_Array(ref v); } },
            { typeof(List<Tensor<Trafo2d>>), (c,o) => { var v = (List<Tensor<Trafo2d>>)o; c.CodeList_of_Tensor_of_Trafo2d__(ref v); } },

            #endregion

            #region Trafo3f

            { typeof(Trafo3f), (c,o) => { var v = (Trafo3f)o; c.CodeTrafo3f(ref v); } },
            { typeof(Trafo3f[]), (c,o) => { var v = (Trafo3f[])o; c.CodeTrafo3fArray(ref v); } },
            { typeof(List<Trafo3f>), (c,o) => { var v = (List<Trafo3f>)o; c.CodeList_of_Trafo3f_(ref v); } },

            { typeof(Vector<Trafo3f>), (c,o) => { var v = (Vector<Trafo3f>)o; c.CodeVector_of_Trafo3f_(ref v); } },
            { typeof(Vector<Trafo3f>[]), (c,o) => { var v = (Vector<Trafo3f>[])o; c.CodeVector_of_Trafo3f_Array(ref v); } },
            { typeof(List<Vector<Trafo3f>>), (c,o) => { var v = (List<Vector<Trafo3f>>)o; c.CodeList_of_Vector_of_Trafo3f__(ref v); } },

            { typeof(Matrix<Trafo3f>), (c,o) => { var v = (Matrix<Trafo3f>)o; c.CodeMatrix_of_Trafo3f_(ref v); } },
            { typeof(Matrix<Trafo3f>[]), (c,o) => { var v = (Matrix<Trafo3f>[])o; c.CodeMatrix_of_Trafo3f_Array(ref v); } },
            { typeof(List<Matrix<Trafo3f>>), (c,o) => { var v = (List<Matrix<Trafo3f>>)o; c.CodeList_of_Matrix_of_Trafo3f__(ref v); } },

            { typeof(Volume<Trafo3f>), (c,o) => { var v = (Volume<Trafo3f>)o; c.CodeVolume_of_Trafo3f_(ref v); } },
            { typeof(Volume<Trafo3f>[]), (c,o) => { var v = (Volume<Trafo3f>[])o; c.CodeVolume_of_Trafo3f_Array(ref v); } },
            { typeof(List<Volume<Trafo3f>>), (c,o) => { var v = (List<Volume<Trafo3f>>)o; c.CodeList_of_Volume_of_Trafo3f__(ref v); } },

            { typeof(Tensor<Trafo3f>), (c,o) => { var v = (Tensor<Trafo3f>)o; c.CodeTensor_of_Trafo3f_(ref v); } },
            { typeof(Tensor<Trafo3f>[]), (c,o) => { var v = (Tensor<Trafo3f>[])o; c.CodeTensor_of_Trafo3f_Array(ref v); } },
            { typeof(List<Tensor<Trafo3f>>), (c,o) => { var v = (List<Tensor<Trafo3f>>)o; c.CodeList_of_Tensor_of_Trafo3f__(ref v); } },

            #endregion

            #region Trafo3d

            { typeof(Trafo3d), (c,o) => { var v = (Trafo3d)o; c.CodeTrafo3d(ref v); } },
            { typeof(Trafo3d[]), (c,o) => { var v = (Trafo3d[])o; c.CodeTrafo3dArray(ref v); } },
            { typeof(List<Trafo3d>), (c,o) => { var v = (List<Trafo3d>)o; c.CodeList_of_Trafo3d_(ref v); } },

            { typeof(Vector<Trafo3d>), (c,o) => { var v = (Vector<Trafo3d>)o; c.CodeVector_of_Trafo3d_(ref v); } },
            { typeof(Vector<Trafo3d>[]), (c,o) => { var v = (Vector<Trafo3d>[])o; c.CodeVector_of_Trafo3d_Array(ref v); } },
            { typeof(List<Vector<Trafo3d>>), (c,o) => { var v = (List<Vector<Trafo3d>>)o; c.CodeList_of_Vector_of_Trafo3d__(ref v); } },

            { typeof(Matrix<Trafo3d>), (c,o) => { var v = (Matrix<Trafo3d>)o; c.CodeMatrix_of_Trafo3d_(ref v); } },
            { typeof(Matrix<Trafo3d>[]), (c,o) => { var v = (Matrix<Trafo3d>[])o; c.CodeMatrix_of_Trafo3d_Array(ref v); } },
            { typeof(List<Matrix<Trafo3d>>), (c,o) => { var v = (List<Matrix<Trafo3d>>)o; c.CodeList_of_Matrix_of_Trafo3d__(ref v); } },

            { typeof(Volume<Trafo3d>), (c,o) => { var v = (Volume<Trafo3d>)o; c.CodeVolume_of_Trafo3d_(ref v); } },
            { typeof(Volume<Trafo3d>[]), (c,o) => { var v = (Volume<Trafo3d>[])o; c.CodeVolume_of_Trafo3d_Array(ref v); } },
            { typeof(List<Volume<Trafo3d>>), (c,o) => { var v = (List<Volume<Trafo3d>>)o; c.CodeList_of_Volume_of_Trafo3d__(ref v); } },

            { typeof(Tensor<Trafo3d>), (c,o) => { var v = (Tensor<Trafo3d>)o; c.CodeTensor_of_Trafo3d_(ref v); } },
            { typeof(Tensor<Trafo3d>[]), (c,o) => { var v = (Tensor<Trafo3d>[])o; c.CodeTensor_of_Trafo3d_Array(ref v); } },
            { typeof(List<Tensor<Trafo3d>>), (c,o) => { var v = (List<Tensor<Trafo3d>>)o; c.CodeList_of_Tensor_of_Trafo3d__(ref v); } },

            #endregion

            #region bool

            { typeof(bool), (c,o) => { var v = (bool)o; c.CodeBool(ref v); } },
            { typeof(bool[]), (c,o) => { var v = (bool[])o; c.CodeBoolArray(ref v); } },
            { typeof(List<bool>), (c,o) => { var v = (List<bool>)o; c.CodeList_of_Bool_(ref v); } },

            { typeof(Vector<bool>), (c,o) => { var v = (Vector<bool>)o; c.CodeVector_of_Bool_(ref v); } },
            { typeof(Vector<bool>[]), (c,o) => { var v = (Vector<bool>[])o; c.CodeVector_of_Bool_Array(ref v); } },
            { typeof(List<Vector<bool>>), (c,o) => { var v = (List<Vector<bool>>)o; c.CodeList_of_Vector_of_Bool__(ref v); } },

            { typeof(Matrix<bool>), (c,o) => { var v = (Matrix<bool>)o; c.CodeMatrix_of_Bool_(ref v); } },
            { typeof(Matrix<bool>[]), (c,o) => { var v = (Matrix<bool>[])o; c.CodeMatrix_of_Bool_Array(ref v); } },
            { typeof(List<Matrix<bool>>), (c,o) => { var v = (List<Matrix<bool>>)o; c.CodeList_of_Matrix_of_Bool__(ref v); } },

            { typeof(Volume<bool>), (c,o) => { var v = (Volume<bool>)o; c.CodeVolume_of_Bool_(ref v); } },
            { typeof(Volume<bool>[]), (c,o) => { var v = (Volume<bool>[])o; c.CodeVolume_of_Bool_Array(ref v); } },
            { typeof(List<Volume<bool>>), (c,o) => { var v = (List<Volume<bool>>)o; c.CodeList_of_Volume_of_Bool__(ref v); } },

            { typeof(Tensor<bool>), (c,o) => { var v = (Tensor<bool>)o; c.CodeTensor_of_Bool_(ref v); } },
            { typeof(Tensor<bool>[]), (c,o) => { var v = (Tensor<bool>[])o; c.CodeTensor_of_Bool_Array(ref v); } },
            { typeof(List<Tensor<bool>>), (c,o) => { var v = (List<Tensor<bool>>)o; c.CodeList_of_Tensor_of_Bool__(ref v); } },

            #endregion

            #region char

            { typeof(char), (c,o) => { var v = (char)o; c.CodeChar(ref v); } },
            { typeof(char[]), (c,o) => { var v = (char[])o; c.CodeCharArray(ref v); } },
            { typeof(List<char>), (c,o) => { var v = (List<char>)o; c.CodeList_of_Char_(ref v); } },

            { typeof(Vector<char>), (c,o) => { var v = (Vector<char>)o; c.CodeVector_of_Char_(ref v); } },
            { typeof(Vector<char>[]), (c,o) => { var v = (Vector<char>[])o; c.CodeVector_of_Char_Array(ref v); } },
            { typeof(List<Vector<char>>), (c,o) => { var v = (List<Vector<char>>)o; c.CodeList_of_Vector_of_Char__(ref v); } },

            { typeof(Matrix<char>), (c,o) => { var v = (Matrix<char>)o; c.CodeMatrix_of_Char_(ref v); } },
            { typeof(Matrix<char>[]), (c,o) => { var v = (Matrix<char>[])o; c.CodeMatrix_of_Char_Array(ref v); } },
            { typeof(List<Matrix<char>>), (c,o) => { var v = (List<Matrix<char>>)o; c.CodeList_of_Matrix_of_Char__(ref v); } },

            { typeof(Volume<char>), (c,o) => { var v = (Volume<char>)o; c.CodeVolume_of_Char_(ref v); } },
            { typeof(Volume<char>[]), (c,o) => { var v = (Volume<char>[])o; c.CodeVolume_of_Char_Array(ref v); } },
            { typeof(List<Volume<char>>), (c,o) => { var v = (List<Volume<char>>)o; c.CodeList_of_Volume_of_Char__(ref v); } },

            { typeof(Tensor<char>), (c,o) => { var v = (Tensor<char>)o; c.CodeTensor_of_Char_(ref v); } },
            { typeof(Tensor<char>[]), (c,o) => { var v = (Tensor<char>[])o; c.CodeTensor_of_Char_Array(ref v); } },
            { typeof(List<Tensor<char>>), (c,o) => { var v = (List<Tensor<char>>)o; c.CodeList_of_Tensor_of_Char__(ref v); } },

            #endregion

            #region string

            { typeof(string), (c,o) => { var v = (string)o; c.CodeString(ref v); } },
            { typeof(string[]), (c,o) => { var v = (string[])o; c.CodeStringArray(ref v); } },
            { typeof(List<string>), (c,o) => { var v = (List<string>)o; c.CodeList_of_String_(ref v); } },

            { typeof(Vector<string>), (c,o) => { var v = (Vector<string>)o; c.CodeVector_of_String_(ref v); } },
            { typeof(Vector<string>[]), (c,o) => { var v = (Vector<string>[])o; c.CodeVector_of_String_Array(ref v); } },
            { typeof(List<Vector<string>>), (c,o) => { var v = (List<Vector<string>>)o; c.CodeList_of_Vector_of_String__(ref v); } },

            { typeof(Matrix<string>), (c,o) => { var v = (Matrix<string>)o; c.CodeMatrix_of_String_(ref v); } },
            { typeof(Matrix<string>[]), (c,o) => { var v = (Matrix<string>[])o; c.CodeMatrix_of_String_Array(ref v); } },
            { typeof(List<Matrix<string>>), (c,o) => { var v = (List<Matrix<string>>)o; c.CodeList_of_Matrix_of_String__(ref v); } },

            { typeof(Volume<string>), (c,o) => { var v = (Volume<string>)o; c.CodeVolume_of_String_(ref v); } },
            { typeof(Volume<string>[]), (c,o) => { var v = (Volume<string>[])o; c.CodeVolume_of_String_Array(ref v); } },
            { typeof(List<Volume<string>>), (c,o) => { var v = (List<Volume<string>>)o; c.CodeList_of_Volume_of_String__(ref v); } },

            { typeof(Tensor<string>), (c,o) => { var v = (Tensor<string>)o; c.CodeTensor_of_String_(ref v); } },
            { typeof(Tensor<string>[]), (c,o) => { var v = (Tensor<string>[])o; c.CodeTensor_of_String_Array(ref v); } },
            { typeof(List<Tensor<string>>), (c,o) => { var v = (List<Tensor<string>>)o; c.CodeList_of_Tensor_of_String__(ref v); } },

            #endregion

            #region Type

            { typeof(Type), (c,o) => { var v = (Type)o; c.CodeType(ref v); } },
            { typeof(Type[]), (c,o) => { var v = (Type[])o; c.CodeTypeArray(ref v); } },
            { typeof(List<Type>), (c,o) => { var v = (List<Type>)o; c.CodeList_of_Type_(ref v); } },

            { typeof(Vector<Type>), (c,o) => { var v = (Vector<Type>)o; c.CodeVector_of_Type_(ref v); } },
            { typeof(Vector<Type>[]), (c,o) => { var v = (Vector<Type>[])o; c.CodeVector_of_Type_Array(ref v); } },
            { typeof(List<Vector<Type>>), (c,o) => { var v = (List<Vector<Type>>)o; c.CodeList_of_Vector_of_Type__(ref v); } },

            { typeof(Matrix<Type>), (c,o) => { var v = (Matrix<Type>)o; c.CodeMatrix_of_Type_(ref v); } },
            { typeof(Matrix<Type>[]), (c,o) => { var v = (Matrix<Type>[])o; c.CodeMatrix_of_Type_Array(ref v); } },
            { typeof(List<Matrix<Type>>), (c,o) => { var v = (List<Matrix<Type>>)o; c.CodeList_of_Matrix_of_Type__(ref v); } },

            { typeof(Volume<Type>), (c,o) => { var v = (Volume<Type>)o; c.CodeVolume_of_Type_(ref v); } },
            { typeof(Volume<Type>[]), (c,o) => { var v = (Volume<Type>[])o; c.CodeVolume_of_Type_Array(ref v); } },
            { typeof(List<Volume<Type>>), (c,o) => { var v = (List<Volume<Type>>)o; c.CodeList_of_Volume_of_Type__(ref v); } },

            { typeof(Tensor<Type>), (c,o) => { var v = (Tensor<Type>)o; c.CodeTensor_of_Type_(ref v); } },
            { typeof(Tensor<Type>[]), (c,o) => { var v = (Tensor<Type>[])o; c.CodeTensor_of_Type_Array(ref v); } },
            { typeof(List<Tensor<Type>>), (c,o) => { var v = (List<Tensor<Type>>)o; c.CodeList_of_Tensor_of_Type__(ref v); } },

            #endregion

            #region Guid

            { typeof(Guid), (c,o) => { var v = (Guid)o; c.CodeGuid(ref v); } },
            { typeof(Guid[]), (c,o) => { var v = (Guid[])o; c.CodeGuidArray(ref v); } },
            { typeof(List<Guid>), (c,o) => { var v = (List<Guid>)o; c.CodeList_of_Guid_(ref v); } },

            { typeof(Vector<Guid>), (c,o) => { var v = (Vector<Guid>)o; c.CodeVector_of_Guid_(ref v); } },
            { typeof(Vector<Guid>[]), (c,o) => { var v = (Vector<Guid>[])o; c.CodeVector_of_Guid_Array(ref v); } },
            { typeof(List<Vector<Guid>>), (c,o) => { var v = (List<Vector<Guid>>)o; c.CodeList_of_Vector_of_Guid__(ref v); } },

            { typeof(Matrix<Guid>), (c,o) => { var v = (Matrix<Guid>)o; c.CodeMatrix_of_Guid_(ref v); } },
            { typeof(Matrix<Guid>[]), (c,o) => { var v = (Matrix<Guid>[])o; c.CodeMatrix_of_Guid_Array(ref v); } },
            { typeof(List<Matrix<Guid>>), (c,o) => { var v = (List<Matrix<Guid>>)o; c.CodeList_of_Matrix_of_Guid__(ref v); } },

            { typeof(Volume<Guid>), (c,o) => { var v = (Volume<Guid>)o; c.CodeVolume_of_Guid_(ref v); } },
            { typeof(Volume<Guid>[]), (c,o) => { var v = (Volume<Guid>[])o; c.CodeVolume_of_Guid_Array(ref v); } },
            { typeof(List<Volume<Guid>>), (c,o) => { var v = (List<Volume<Guid>>)o; c.CodeList_of_Volume_of_Guid__(ref v); } },

            { typeof(Tensor<Guid>), (c,o) => { var v = (Tensor<Guid>)o; c.CodeTensor_of_Guid_(ref v); } },
            { typeof(Tensor<Guid>[]), (c,o) => { var v = (Tensor<Guid>[])o; c.CodeTensor_of_Guid_Array(ref v); } },
            { typeof(List<Tensor<Guid>>), (c,o) => { var v = (List<Tensor<Guid>>)o; c.CodeList_of_Tensor_of_Guid__(ref v); } },

            #endregion

            #region Symbol

            { typeof(Symbol), (c,o) => { var v = (Symbol)o; c.CodeSymbol(ref v); } },
            { typeof(Symbol[]), (c,o) => { var v = (Symbol[])o; c.CodeSymbolArray(ref v); } },
            { typeof(List<Symbol>), (c,o) => { var v = (List<Symbol>)o; c.CodeList_of_Symbol_(ref v); } },

            { typeof(Vector<Symbol>), (c,o) => { var v = (Vector<Symbol>)o; c.CodeVector_of_Symbol_(ref v); } },
            { typeof(Vector<Symbol>[]), (c,o) => { var v = (Vector<Symbol>[])o; c.CodeVector_of_Symbol_Array(ref v); } },
            { typeof(List<Vector<Symbol>>), (c,o) => { var v = (List<Vector<Symbol>>)o; c.CodeList_of_Vector_of_Symbol__(ref v); } },

            { typeof(Matrix<Symbol>), (c,o) => { var v = (Matrix<Symbol>)o; c.CodeMatrix_of_Symbol_(ref v); } },
            { typeof(Matrix<Symbol>[]), (c,o) => { var v = (Matrix<Symbol>[])o; c.CodeMatrix_of_Symbol_Array(ref v); } },
            { typeof(List<Matrix<Symbol>>), (c,o) => { var v = (List<Matrix<Symbol>>)o; c.CodeList_of_Matrix_of_Symbol__(ref v); } },

            { typeof(Volume<Symbol>), (c,o) => { var v = (Volume<Symbol>)o; c.CodeVolume_of_Symbol_(ref v); } },
            { typeof(Volume<Symbol>[]), (c,o) => { var v = (Volume<Symbol>[])o; c.CodeVolume_of_Symbol_Array(ref v); } },
            { typeof(List<Volume<Symbol>>), (c,o) => { var v = (List<Volume<Symbol>>)o; c.CodeList_of_Volume_of_Symbol__(ref v); } },

            { typeof(Tensor<Symbol>), (c,o) => { var v = (Tensor<Symbol>)o; c.CodeTensor_of_Symbol_(ref v); } },
            { typeof(Tensor<Symbol>[]), (c,o) => { var v = (Tensor<Symbol>[])o; c.CodeTensor_of_Symbol_Array(ref v); } },
            { typeof(List<Tensor<Symbol>>), (c,o) => { var v = (List<Tensor<Symbol>>)o; c.CodeList_of_Tensor_of_Symbol__(ref v); } },

            #endregion

            #region Circle2d

            { typeof(Circle2d), (c,o) => { var v = (Circle2d)o; c.CodeCircle2d(ref v); } },
            { typeof(Circle2d[]), (c,o) => { var v = (Circle2d[])o; c.CodeCircle2dArray(ref v); } },
            { typeof(List<Circle2d>), (c,o) => { var v = (List<Circle2d>)o; c.CodeList_of_Circle2d_(ref v); } },

            { typeof(Vector<Circle2d>), (c,o) => { var v = (Vector<Circle2d>)o; c.CodeVector_of_Circle2d_(ref v); } },
            { typeof(Vector<Circle2d>[]), (c,o) => { var v = (Vector<Circle2d>[])o; c.CodeVector_of_Circle2d_Array(ref v); } },
            { typeof(List<Vector<Circle2d>>), (c,o) => { var v = (List<Vector<Circle2d>>)o; c.CodeList_of_Vector_of_Circle2d__(ref v); } },

            { typeof(Matrix<Circle2d>), (c,o) => { var v = (Matrix<Circle2d>)o; c.CodeMatrix_of_Circle2d_(ref v); } },
            { typeof(Matrix<Circle2d>[]), (c,o) => { var v = (Matrix<Circle2d>[])o; c.CodeMatrix_of_Circle2d_Array(ref v); } },
            { typeof(List<Matrix<Circle2d>>), (c,o) => { var v = (List<Matrix<Circle2d>>)o; c.CodeList_of_Matrix_of_Circle2d__(ref v); } },

            { typeof(Volume<Circle2d>), (c,o) => { var v = (Volume<Circle2d>)o; c.CodeVolume_of_Circle2d_(ref v); } },
            { typeof(Volume<Circle2d>[]), (c,o) => { var v = (Volume<Circle2d>[])o; c.CodeVolume_of_Circle2d_Array(ref v); } },
            { typeof(List<Volume<Circle2d>>), (c,o) => { var v = (List<Volume<Circle2d>>)o; c.CodeList_of_Volume_of_Circle2d__(ref v); } },

            { typeof(Tensor<Circle2d>), (c,o) => { var v = (Tensor<Circle2d>)o; c.CodeTensor_of_Circle2d_(ref v); } },
            { typeof(Tensor<Circle2d>[]), (c,o) => { var v = (Tensor<Circle2d>[])o; c.CodeTensor_of_Circle2d_Array(ref v); } },
            { typeof(List<Tensor<Circle2d>>), (c,o) => { var v = (List<Tensor<Circle2d>>)o; c.CodeList_of_Tensor_of_Circle2d__(ref v); } },

            #endregion

            #region Line2d

            { typeof(Line2d), (c,o) => { var v = (Line2d)o; c.CodeLine2d(ref v); } },
            { typeof(Line2d[]), (c,o) => { var v = (Line2d[])o; c.CodeLine2dArray(ref v); } },
            { typeof(List<Line2d>), (c,o) => { var v = (List<Line2d>)o; c.CodeList_of_Line2d_(ref v); } },

            { typeof(Vector<Line2d>), (c,o) => { var v = (Vector<Line2d>)o; c.CodeVector_of_Line2d_(ref v); } },
            { typeof(Vector<Line2d>[]), (c,o) => { var v = (Vector<Line2d>[])o; c.CodeVector_of_Line2d_Array(ref v); } },
            { typeof(List<Vector<Line2d>>), (c,o) => { var v = (List<Vector<Line2d>>)o; c.CodeList_of_Vector_of_Line2d__(ref v); } },

            { typeof(Matrix<Line2d>), (c,o) => { var v = (Matrix<Line2d>)o; c.CodeMatrix_of_Line2d_(ref v); } },
            { typeof(Matrix<Line2d>[]), (c,o) => { var v = (Matrix<Line2d>[])o; c.CodeMatrix_of_Line2d_Array(ref v); } },
            { typeof(List<Matrix<Line2d>>), (c,o) => { var v = (List<Matrix<Line2d>>)o; c.CodeList_of_Matrix_of_Line2d__(ref v); } },

            { typeof(Volume<Line2d>), (c,o) => { var v = (Volume<Line2d>)o; c.CodeVolume_of_Line2d_(ref v); } },
            { typeof(Volume<Line2d>[]), (c,o) => { var v = (Volume<Line2d>[])o; c.CodeVolume_of_Line2d_Array(ref v); } },
            { typeof(List<Volume<Line2d>>), (c,o) => { var v = (List<Volume<Line2d>>)o; c.CodeList_of_Volume_of_Line2d__(ref v); } },

            { typeof(Tensor<Line2d>), (c,o) => { var v = (Tensor<Line2d>)o; c.CodeTensor_of_Line2d_(ref v); } },
            { typeof(Tensor<Line2d>[]), (c,o) => { var v = (Tensor<Line2d>[])o; c.CodeTensor_of_Line2d_Array(ref v); } },
            { typeof(List<Tensor<Line2d>>), (c,o) => { var v = (List<Tensor<Line2d>>)o; c.CodeList_of_Tensor_of_Line2d__(ref v); } },

            #endregion

            #region Line3d

            { typeof(Line3d), (c,o) => { var v = (Line3d)o; c.CodeLine3d(ref v); } },
            { typeof(Line3d[]), (c,o) => { var v = (Line3d[])o; c.CodeLine3dArray(ref v); } },
            { typeof(List<Line3d>), (c,o) => { var v = (List<Line3d>)o; c.CodeList_of_Line3d_(ref v); } },

            { typeof(Vector<Line3d>), (c,o) => { var v = (Vector<Line3d>)o; c.CodeVector_of_Line3d_(ref v); } },
            { typeof(Vector<Line3d>[]), (c,o) => { var v = (Vector<Line3d>[])o; c.CodeVector_of_Line3d_Array(ref v); } },
            { typeof(List<Vector<Line3d>>), (c,o) => { var v = (List<Vector<Line3d>>)o; c.CodeList_of_Vector_of_Line3d__(ref v); } },

            { typeof(Matrix<Line3d>), (c,o) => { var v = (Matrix<Line3d>)o; c.CodeMatrix_of_Line3d_(ref v); } },
            { typeof(Matrix<Line3d>[]), (c,o) => { var v = (Matrix<Line3d>[])o; c.CodeMatrix_of_Line3d_Array(ref v); } },
            { typeof(List<Matrix<Line3d>>), (c,o) => { var v = (List<Matrix<Line3d>>)o; c.CodeList_of_Matrix_of_Line3d__(ref v); } },

            { typeof(Volume<Line3d>), (c,o) => { var v = (Volume<Line3d>)o; c.CodeVolume_of_Line3d_(ref v); } },
            { typeof(Volume<Line3d>[]), (c,o) => { var v = (Volume<Line3d>[])o; c.CodeVolume_of_Line3d_Array(ref v); } },
            { typeof(List<Volume<Line3d>>), (c,o) => { var v = (List<Volume<Line3d>>)o; c.CodeList_of_Volume_of_Line3d__(ref v); } },

            { typeof(Tensor<Line3d>), (c,o) => { var v = (Tensor<Line3d>)o; c.CodeTensor_of_Line3d_(ref v); } },
            { typeof(Tensor<Line3d>[]), (c,o) => { var v = (Tensor<Line3d>[])o; c.CodeTensor_of_Line3d_Array(ref v); } },
            { typeof(List<Tensor<Line3d>>), (c,o) => { var v = (List<Tensor<Line3d>>)o; c.CodeList_of_Tensor_of_Line3d__(ref v); } },

            #endregion

            #region Plane2d

            { typeof(Plane2d), (c,o) => { var v = (Plane2d)o; c.CodePlane2d(ref v); } },
            { typeof(Plane2d[]), (c,o) => { var v = (Plane2d[])o; c.CodePlane2dArray(ref v); } },
            { typeof(List<Plane2d>), (c,o) => { var v = (List<Plane2d>)o; c.CodeList_of_Plane2d_(ref v); } },

            { typeof(Vector<Plane2d>), (c,o) => { var v = (Vector<Plane2d>)o; c.CodeVector_of_Plane2d_(ref v); } },
            { typeof(Vector<Plane2d>[]), (c,o) => { var v = (Vector<Plane2d>[])o; c.CodeVector_of_Plane2d_Array(ref v); } },
            { typeof(List<Vector<Plane2d>>), (c,o) => { var v = (List<Vector<Plane2d>>)o; c.CodeList_of_Vector_of_Plane2d__(ref v); } },

            { typeof(Matrix<Plane2d>), (c,o) => { var v = (Matrix<Plane2d>)o; c.CodeMatrix_of_Plane2d_(ref v); } },
            { typeof(Matrix<Plane2d>[]), (c,o) => { var v = (Matrix<Plane2d>[])o; c.CodeMatrix_of_Plane2d_Array(ref v); } },
            { typeof(List<Matrix<Plane2d>>), (c,o) => { var v = (List<Matrix<Plane2d>>)o; c.CodeList_of_Matrix_of_Plane2d__(ref v); } },

            { typeof(Volume<Plane2d>), (c,o) => { var v = (Volume<Plane2d>)o; c.CodeVolume_of_Plane2d_(ref v); } },
            { typeof(Volume<Plane2d>[]), (c,o) => { var v = (Volume<Plane2d>[])o; c.CodeVolume_of_Plane2d_Array(ref v); } },
            { typeof(List<Volume<Plane2d>>), (c,o) => { var v = (List<Volume<Plane2d>>)o; c.CodeList_of_Volume_of_Plane2d__(ref v); } },

            { typeof(Tensor<Plane2d>), (c,o) => { var v = (Tensor<Plane2d>)o; c.CodeTensor_of_Plane2d_(ref v); } },
            { typeof(Tensor<Plane2d>[]), (c,o) => { var v = (Tensor<Plane2d>[])o; c.CodeTensor_of_Plane2d_Array(ref v); } },
            { typeof(List<Tensor<Plane2d>>), (c,o) => { var v = (List<Tensor<Plane2d>>)o; c.CodeList_of_Tensor_of_Plane2d__(ref v); } },

            #endregion

            #region Plane3d

            { typeof(Plane3d), (c,o) => { var v = (Plane3d)o; c.CodePlane3d(ref v); } },
            { typeof(Plane3d[]), (c,o) => { var v = (Plane3d[])o; c.CodePlane3dArray(ref v); } },
            { typeof(List<Plane3d>), (c,o) => { var v = (List<Plane3d>)o; c.CodeList_of_Plane3d_(ref v); } },

            { typeof(Vector<Plane3d>), (c,o) => { var v = (Vector<Plane3d>)o; c.CodeVector_of_Plane3d_(ref v); } },
            { typeof(Vector<Plane3d>[]), (c,o) => { var v = (Vector<Plane3d>[])o; c.CodeVector_of_Plane3d_Array(ref v); } },
            { typeof(List<Vector<Plane3d>>), (c,o) => { var v = (List<Vector<Plane3d>>)o; c.CodeList_of_Vector_of_Plane3d__(ref v); } },

            { typeof(Matrix<Plane3d>), (c,o) => { var v = (Matrix<Plane3d>)o; c.CodeMatrix_of_Plane3d_(ref v); } },
            { typeof(Matrix<Plane3d>[]), (c,o) => { var v = (Matrix<Plane3d>[])o; c.CodeMatrix_of_Plane3d_Array(ref v); } },
            { typeof(List<Matrix<Plane3d>>), (c,o) => { var v = (List<Matrix<Plane3d>>)o; c.CodeList_of_Matrix_of_Plane3d__(ref v); } },

            { typeof(Volume<Plane3d>), (c,o) => { var v = (Volume<Plane3d>)o; c.CodeVolume_of_Plane3d_(ref v); } },
            { typeof(Volume<Plane3d>[]), (c,o) => { var v = (Volume<Plane3d>[])o; c.CodeVolume_of_Plane3d_Array(ref v); } },
            { typeof(List<Volume<Plane3d>>), (c,o) => { var v = (List<Volume<Plane3d>>)o; c.CodeList_of_Volume_of_Plane3d__(ref v); } },

            { typeof(Tensor<Plane3d>), (c,o) => { var v = (Tensor<Plane3d>)o; c.CodeTensor_of_Plane3d_(ref v); } },
            { typeof(Tensor<Plane3d>[]), (c,o) => { var v = (Tensor<Plane3d>[])o; c.CodeTensor_of_Plane3d_Array(ref v); } },
            { typeof(List<Tensor<Plane3d>>), (c,o) => { var v = (List<Tensor<Plane3d>>)o; c.CodeList_of_Tensor_of_Plane3d__(ref v); } },

            #endregion

            #region PlaneWithPoint3d

            { typeof(PlaneWithPoint3d), (c,o) => { var v = (PlaneWithPoint3d)o; c.CodePlaneWithPoint3d(ref v); } },
            { typeof(PlaneWithPoint3d[]), (c,o) => { var v = (PlaneWithPoint3d[])o; c.CodePlaneWithPoint3dArray(ref v); } },
            { typeof(List<PlaneWithPoint3d>), (c,o) => { var v = (List<PlaneWithPoint3d>)o; c.CodeList_of_PlaneWithPoint3d_(ref v); } },

            { typeof(Vector<PlaneWithPoint3d>), (c,o) => { var v = (Vector<PlaneWithPoint3d>)o; c.CodeVector_of_PlaneWithPoint3d_(ref v); } },
            { typeof(Vector<PlaneWithPoint3d>[]), (c,o) => { var v = (Vector<PlaneWithPoint3d>[])o; c.CodeVector_of_PlaneWithPoint3d_Array(ref v); } },
            { typeof(List<Vector<PlaneWithPoint3d>>), (c,o) => { var v = (List<Vector<PlaneWithPoint3d>>)o; c.CodeList_of_Vector_of_PlaneWithPoint3d__(ref v); } },

            { typeof(Matrix<PlaneWithPoint3d>), (c,o) => { var v = (Matrix<PlaneWithPoint3d>)o; c.CodeMatrix_of_PlaneWithPoint3d_(ref v); } },
            { typeof(Matrix<PlaneWithPoint3d>[]), (c,o) => { var v = (Matrix<PlaneWithPoint3d>[])o; c.CodeMatrix_of_PlaneWithPoint3d_Array(ref v); } },
            { typeof(List<Matrix<PlaneWithPoint3d>>), (c,o) => { var v = (List<Matrix<PlaneWithPoint3d>>)o; c.CodeList_of_Matrix_of_PlaneWithPoint3d__(ref v); } },

            { typeof(Volume<PlaneWithPoint3d>), (c,o) => { var v = (Volume<PlaneWithPoint3d>)o; c.CodeVolume_of_PlaneWithPoint3d_(ref v); } },
            { typeof(Volume<PlaneWithPoint3d>[]), (c,o) => { var v = (Volume<PlaneWithPoint3d>[])o; c.CodeVolume_of_PlaneWithPoint3d_Array(ref v); } },
            { typeof(List<Volume<PlaneWithPoint3d>>), (c,o) => { var v = (List<Volume<PlaneWithPoint3d>>)o; c.CodeList_of_Volume_of_PlaneWithPoint3d__(ref v); } },

            { typeof(Tensor<PlaneWithPoint3d>), (c,o) => { var v = (Tensor<PlaneWithPoint3d>)o; c.CodeTensor_of_PlaneWithPoint3d_(ref v); } },
            { typeof(Tensor<PlaneWithPoint3d>[]), (c,o) => { var v = (Tensor<PlaneWithPoint3d>[])o; c.CodeTensor_of_PlaneWithPoint3d_Array(ref v); } },
            { typeof(List<Tensor<PlaneWithPoint3d>>), (c,o) => { var v = (List<Tensor<PlaneWithPoint3d>>)o; c.CodeList_of_Tensor_of_PlaneWithPoint3d__(ref v); } },

            #endregion

            #region Quad2d

            { typeof(Quad2d), (c,o) => { var v = (Quad2d)o; c.CodeQuad2d(ref v); } },
            { typeof(Quad2d[]), (c,o) => { var v = (Quad2d[])o; c.CodeQuad2dArray(ref v); } },
            { typeof(List<Quad2d>), (c,o) => { var v = (List<Quad2d>)o; c.CodeList_of_Quad2d_(ref v); } },

            { typeof(Vector<Quad2d>), (c,o) => { var v = (Vector<Quad2d>)o; c.CodeVector_of_Quad2d_(ref v); } },
            { typeof(Vector<Quad2d>[]), (c,o) => { var v = (Vector<Quad2d>[])o; c.CodeVector_of_Quad2d_Array(ref v); } },
            { typeof(List<Vector<Quad2d>>), (c,o) => { var v = (List<Vector<Quad2d>>)o; c.CodeList_of_Vector_of_Quad2d__(ref v); } },

            { typeof(Matrix<Quad2d>), (c,o) => { var v = (Matrix<Quad2d>)o; c.CodeMatrix_of_Quad2d_(ref v); } },
            { typeof(Matrix<Quad2d>[]), (c,o) => { var v = (Matrix<Quad2d>[])o; c.CodeMatrix_of_Quad2d_Array(ref v); } },
            { typeof(List<Matrix<Quad2d>>), (c,o) => { var v = (List<Matrix<Quad2d>>)o; c.CodeList_of_Matrix_of_Quad2d__(ref v); } },

            { typeof(Volume<Quad2d>), (c,o) => { var v = (Volume<Quad2d>)o; c.CodeVolume_of_Quad2d_(ref v); } },
            { typeof(Volume<Quad2d>[]), (c,o) => { var v = (Volume<Quad2d>[])o; c.CodeVolume_of_Quad2d_Array(ref v); } },
            { typeof(List<Volume<Quad2d>>), (c,o) => { var v = (List<Volume<Quad2d>>)o; c.CodeList_of_Volume_of_Quad2d__(ref v); } },

            { typeof(Tensor<Quad2d>), (c,o) => { var v = (Tensor<Quad2d>)o; c.CodeTensor_of_Quad2d_(ref v); } },
            { typeof(Tensor<Quad2d>[]), (c,o) => { var v = (Tensor<Quad2d>[])o; c.CodeTensor_of_Quad2d_Array(ref v); } },
            { typeof(List<Tensor<Quad2d>>), (c,o) => { var v = (List<Tensor<Quad2d>>)o; c.CodeList_of_Tensor_of_Quad2d__(ref v); } },

            #endregion

            #region Quad3d

            { typeof(Quad3d), (c,o) => { var v = (Quad3d)o; c.CodeQuad3d(ref v); } },
            { typeof(Quad3d[]), (c,o) => { var v = (Quad3d[])o; c.CodeQuad3dArray(ref v); } },
            { typeof(List<Quad3d>), (c,o) => { var v = (List<Quad3d>)o; c.CodeList_of_Quad3d_(ref v); } },

            { typeof(Vector<Quad3d>), (c,o) => { var v = (Vector<Quad3d>)o; c.CodeVector_of_Quad3d_(ref v); } },
            { typeof(Vector<Quad3d>[]), (c,o) => { var v = (Vector<Quad3d>[])o; c.CodeVector_of_Quad3d_Array(ref v); } },
            { typeof(List<Vector<Quad3d>>), (c,o) => { var v = (List<Vector<Quad3d>>)o; c.CodeList_of_Vector_of_Quad3d__(ref v); } },

            { typeof(Matrix<Quad3d>), (c,o) => { var v = (Matrix<Quad3d>)o; c.CodeMatrix_of_Quad3d_(ref v); } },
            { typeof(Matrix<Quad3d>[]), (c,o) => { var v = (Matrix<Quad3d>[])o; c.CodeMatrix_of_Quad3d_Array(ref v); } },
            { typeof(List<Matrix<Quad3d>>), (c,o) => { var v = (List<Matrix<Quad3d>>)o; c.CodeList_of_Matrix_of_Quad3d__(ref v); } },

            { typeof(Volume<Quad3d>), (c,o) => { var v = (Volume<Quad3d>)o; c.CodeVolume_of_Quad3d_(ref v); } },
            { typeof(Volume<Quad3d>[]), (c,o) => { var v = (Volume<Quad3d>[])o; c.CodeVolume_of_Quad3d_Array(ref v); } },
            { typeof(List<Volume<Quad3d>>), (c,o) => { var v = (List<Volume<Quad3d>>)o; c.CodeList_of_Volume_of_Quad3d__(ref v); } },

            { typeof(Tensor<Quad3d>), (c,o) => { var v = (Tensor<Quad3d>)o; c.CodeTensor_of_Quad3d_(ref v); } },
            { typeof(Tensor<Quad3d>[]), (c,o) => { var v = (Tensor<Quad3d>[])o; c.CodeTensor_of_Quad3d_Array(ref v); } },
            { typeof(List<Tensor<Quad3d>>), (c,o) => { var v = (List<Tensor<Quad3d>>)o; c.CodeList_of_Tensor_of_Quad3d__(ref v); } },

            #endregion

            #region Ray2d

            { typeof(Ray2d), (c,o) => { var v = (Ray2d)o; c.CodeRay2d(ref v); } },
            { typeof(Ray2d[]), (c,o) => { var v = (Ray2d[])o; c.CodeRay2dArray(ref v); } },
            { typeof(List<Ray2d>), (c,o) => { var v = (List<Ray2d>)o; c.CodeList_of_Ray2d_(ref v); } },

            { typeof(Vector<Ray2d>), (c,o) => { var v = (Vector<Ray2d>)o; c.CodeVector_of_Ray2d_(ref v); } },
            { typeof(Vector<Ray2d>[]), (c,o) => { var v = (Vector<Ray2d>[])o; c.CodeVector_of_Ray2d_Array(ref v); } },
            { typeof(List<Vector<Ray2d>>), (c,o) => { var v = (List<Vector<Ray2d>>)o; c.CodeList_of_Vector_of_Ray2d__(ref v); } },

            { typeof(Matrix<Ray2d>), (c,o) => { var v = (Matrix<Ray2d>)o; c.CodeMatrix_of_Ray2d_(ref v); } },
            { typeof(Matrix<Ray2d>[]), (c,o) => { var v = (Matrix<Ray2d>[])o; c.CodeMatrix_of_Ray2d_Array(ref v); } },
            { typeof(List<Matrix<Ray2d>>), (c,o) => { var v = (List<Matrix<Ray2d>>)o; c.CodeList_of_Matrix_of_Ray2d__(ref v); } },

            { typeof(Volume<Ray2d>), (c,o) => { var v = (Volume<Ray2d>)o; c.CodeVolume_of_Ray2d_(ref v); } },
            { typeof(Volume<Ray2d>[]), (c,o) => { var v = (Volume<Ray2d>[])o; c.CodeVolume_of_Ray2d_Array(ref v); } },
            { typeof(List<Volume<Ray2d>>), (c,o) => { var v = (List<Volume<Ray2d>>)o; c.CodeList_of_Volume_of_Ray2d__(ref v); } },

            { typeof(Tensor<Ray2d>), (c,o) => { var v = (Tensor<Ray2d>)o; c.CodeTensor_of_Ray2d_(ref v); } },
            { typeof(Tensor<Ray2d>[]), (c,o) => { var v = (Tensor<Ray2d>[])o; c.CodeTensor_of_Ray2d_Array(ref v); } },
            { typeof(List<Tensor<Ray2d>>), (c,o) => { var v = (List<Tensor<Ray2d>>)o; c.CodeList_of_Tensor_of_Ray2d__(ref v); } },

            #endregion

            #region Ray3d

            { typeof(Ray3d), (c,o) => { var v = (Ray3d)o; c.CodeRay3d(ref v); } },
            { typeof(Ray3d[]), (c,o) => { var v = (Ray3d[])o; c.CodeRay3dArray(ref v); } },
            { typeof(List<Ray3d>), (c,o) => { var v = (List<Ray3d>)o; c.CodeList_of_Ray3d_(ref v); } },

            { typeof(Vector<Ray3d>), (c,o) => { var v = (Vector<Ray3d>)o; c.CodeVector_of_Ray3d_(ref v); } },
            { typeof(Vector<Ray3d>[]), (c,o) => { var v = (Vector<Ray3d>[])o; c.CodeVector_of_Ray3d_Array(ref v); } },
            { typeof(List<Vector<Ray3d>>), (c,o) => { var v = (List<Vector<Ray3d>>)o; c.CodeList_of_Vector_of_Ray3d__(ref v); } },

            { typeof(Matrix<Ray3d>), (c,o) => { var v = (Matrix<Ray3d>)o; c.CodeMatrix_of_Ray3d_(ref v); } },
            { typeof(Matrix<Ray3d>[]), (c,o) => { var v = (Matrix<Ray3d>[])o; c.CodeMatrix_of_Ray3d_Array(ref v); } },
            { typeof(List<Matrix<Ray3d>>), (c,o) => { var v = (List<Matrix<Ray3d>>)o; c.CodeList_of_Matrix_of_Ray3d__(ref v); } },

            { typeof(Volume<Ray3d>), (c,o) => { var v = (Volume<Ray3d>)o; c.CodeVolume_of_Ray3d_(ref v); } },
            { typeof(Volume<Ray3d>[]), (c,o) => { var v = (Volume<Ray3d>[])o; c.CodeVolume_of_Ray3d_Array(ref v); } },
            { typeof(List<Volume<Ray3d>>), (c,o) => { var v = (List<Volume<Ray3d>>)o; c.CodeList_of_Volume_of_Ray3d__(ref v); } },

            { typeof(Tensor<Ray3d>), (c,o) => { var v = (Tensor<Ray3d>)o; c.CodeTensor_of_Ray3d_(ref v); } },
            { typeof(Tensor<Ray3d>[]), (c,o) => { var v = (Tensor<Ray3d>[])o; c.CodeTensor_of_Ray3d_Array(ref v); } },
            { typeof(List<Tensor<Ray3d>>), (c,o) => { var v = (List<Tensor<Ray3d>>)o; c.CodeList_of_Tensor_of_Ray3d__(ref v); } },

            #endregion

            #region Sphere3d

            { typeof(Sphere3d), (c,o) => { var v = (Sphere3d)o; c.CodeSphere3d(ref v); } },
            { typeof(Sphere3d[]), (c,o) => { var v = (Sphere3d[])o; c.CodeSphere3dArray(ref v); } },
            { typeof(List<Sphere3d>), (c,o) => { var v = (List<Sphere3d>)o; c.CodeList_of_Sphere3d_(ref v); } },

            { typeof(Vector<Sphere3d>), (c,o) => { var v = (Vector<Sphere3d>)o; c.CodeVector_of_Sphere3d_(ref v); } },
            { typeof(Vector<Sphere3d>[]), (c,o) => { var v = (Vector<Sphere3d>[])o; c.CodeVector_of_Sphere3d_Array(ref v); } },
            { typeof(List<Vector<Sphere3d>>), (c,o) => { var v = (List<Vector<Sphere3d>>)o; c.CodeList_of_Vector_of_Sphere3d__(ref v); } },

            { typeof(Matrix<Sphere3d>), (c,o) => { var v = (Matrix<Sphere3d>)o; c.CodeMatrix_of_Sphere3d_(ref v); } },
            { typeof(Matrix<Sphere3d>[]), (c,o) => { var v = (Matrix<Sphere3d>[])o; c.CodeMatrix_of_Sphere3d_Array(ref v); } },
            { typeof(List<Matrix<Sphere3d>>), (c,o) => { var v = (List<Matrix<Sphere3d>>)o; c.CodeList_of_Matrix_of_Sphere3d__(ref v); } },

            { typeof(Volume<Sphere3d>), (c,o) => { var v = (Volume<Sphere3d>)o; c.CodeVolume_of_Sphere3d_(ref v); } },
            { typeof(Volume<Sphere3d>[]), (c,o) => { var v = (Volume<Sphere3d>[])o; c.CodeVolume_of_Sphere3d_Array(ref v); } },
            { typeof(List<Volume<Sphere3d>>), (c,o) => { var v = (List<Volume<Sphere3d>>)o; c.CodeList_of_Volume_of_Sphere3d__(ref v); } },

            { typeof(Tensor<Sphere3d>), (c,o) => { var v = (Tensor<Sphere3d>)o; c.CodeTensor_of_Sphere3d_(ref v); } },
            { typeof(Tensor<Sphere3d>[]), (c,o) => { var v = (Tensor<Sphere3d>[])o; c.CodeTensor_of_Sphere3d_Array(ref v); } },
            { typeof(List<Tensor<Sphere3d>>), (c,o) => { var v = (List<Tensor<Sphere3d>>)o; c.CodeList_of_Tensor_of_Sphere3d__(ref v); } },

            #endregion

            #region Triangle2d

            { typeof(Triangle2d), (c,o) => { var v = (Triangle2d)o; c.CodeTriangle2d(ref v); } },
            { typeof(Triangle2d[]), (c,o) => { var v = (Triangle2d[])o; c.CodeTriangle2dArray(ref v); } },
            { typeof(List<Triangle2d>), (c,o) => { var v = (List<Triangle2d>)o; c.CodeList_of_Triangle2d_(ref v); } },

            { typeof(Vector<Triangle2d>), (c,o) => { var v = (Vector<Triangle2d>)o; c.CodeVector_of_Triangle2d_(ref v); } },
            { typeof(Vector<Triangle2d>[]), (c,o) => { var v = (Vector<Triangle2d>[])o; c.CodeVector_of_Triangle2d_Array(ref v); } },
            { typeof(List<Vector<Triangle2d>>), (c,o) => { var v = (List<Vector<Triangle2d>>)o; c.CodeList_of_Vector_of_Triangle2d__(ref v); } },

            { typeof(Matrix<Triangle2d>), (c,o) => { var v = (Matrix<Triangle2d>)o; c.CodeMatrix_of_Triangle2d_(ref v); } },
            { typeof(Matrix<Triangle2d>[]), (c,o) => { var v = (Matrix<Triangle2d>[])o; c.CodeMatrix_of_Triangle2d_Array(ref v); } },
            { typeof(List<Matrix<Triangle2d>>), (c,o) => { var v = (List<Matrix<Triangle2d>>)o; c.CodeList_of_Matrix_of_Triangle2d__(ref v); } },

            { typeof(Volume<Triangle2d>), (c,o) => { var v = (Volume<Triangle2d>)o; c.CodeVolume_of_Triangle2d_(ref v); } },
            { typeof(Volume<Triangle2d>[]), (c,o) => { var v = (Volume<Triangle2d>[])o; c.CodeVolume_of_Triangle2d_Array(ref v); } },
            { typeof(List<Volume<Triangle2d>>), (c,o) => { var v = (List<Volume<Triangle2d>>)o; c.CodeList_of_Volume_of_Triangle2d__(ref v); } },

            { typeof(Tensor<Triangle2d>), (c,o) => { var v = (Tensor<Triangle2d>)o; c.CodeTensor_of_Triangle2d_(ref v); } },
            { typeof(Tensor<Triangle2d>[]), (c,o) => { var v = (Tensor<Triangle2d>[])o; c.CodeTensor_of_Triangle2d_Array(ref v); } },
            { typeof(List<Tensor<Triangle2d>>), (c,o) => { var v = (List<Tensor<Triangle2d>>)o; c.CodeList_of_Tensor_of_Triangle2d__(ref v); } },

            #endregion

            #region Triangle3d

            { typeof(Triangle3d), (c,o) => { var v = (Triangle3d)o; c.CodeTriangle3d(ref v); } },
            { typeof(Triangle3d[]), (c,o) => { var v = (Triangle3d[])o; c.CodeTriangle3dArray(ref v); } },
            { typeof(List<Triangle3d>), (c,o) => { var v = (List<Triangle3d>)o; c.CodeList_of_Triangle3d_(ref v); } },

            { typeof(Vector<Triangle3d>), (c,o) => { var v = (Vector<Triangle3d>)o; c.CodeVector_of_Triangle3d_(ref v); } },
            { typeof(Vector<Triangle3d>[]), (c,o) => { var v = (Vector<Triangle3d>[])o; c.CodeVector_of_Triangle3d_Array(ref v); } },
            { typeof(List<Vector<Triangle3d>>), (c,o) => { var v = (List<Vector<Triangle3d>>)o; c.CodeList_of_Vector_of_Triangle3d__(ref v); } },

            { typeof(Matrix<Triangle3d>), (c,o) => { var v = (Matrix<Triangle3d>)o; c.CodeMatrix_of_Triangle3d_(ref v); } },
            { typeof(Matrix<Triangle3d>[]), (c,o) => { var v = (Matrix<Triangle3d>[])o; c.CodeMatrix_of_Triangle3d_Array(ref v); } },
            { typeof(List<Matrix<Triangle3d>>), (c,o) => { var v = (List<Matrix<Triangle3d>>)o; c.CodeList_of_Matrix_of_Triangle3d__(ref v); } },

            { typeof(Volume<Triangle3d>), (c,o) => { var v = (Volume<Triangle3d>)o; c.CodeVolume_of_Triangle3d_(ref v); } },
            { typeof(Volume<Triangle3d>[]), (c,o) => { var v = (Volume<Triangle3d>[])o; c.CodeVolume_of_Triangle3d_Array(ref v); } },
            { typeof(List<Volume<Triangle3d>>), (c,o) => { var v = (List<Volume<Triangle3d>>)o; c.CodeList_of_Volume_of_Triangle3d__(ref v); } },

            { typeof(Tensor<Triangle3d>), (c,o) => { var v = (Tensor<Triangle3d>)o; c.CodeTensor_of_Triangle3d_(ref v); } },
            { typeof(Tensor<Triangle3d>[]), (c,o) => { var v = (Tensor<Triangle3d>[])o; c.CodeTensor_of_Triangle3d_Array(ref v); } },
            { typeof(List<Tensor<Triangle3d>>), (c,o) => { var v = (List<Tensor<Triangle3d>>)o; c.CodeList_of_Tensor_of_Triangle3d__(ref v); } },

            #endregion

            #region Multi-Dimensional Arrays

            { typeof(byte[,]), (c,o) => { var v = (byte[,])o; c.CodeByteArray2d(ref v); } },
            { typeof(byte[, ,]), (c,o) => { var v = (byte[, ,])o; c.CodeByteArray3d(ref v); } },
            { typeof(sbyte[,]), (c,o) => { var v = (sbyte[,])o; c.CodeSByteArray2d(ref v); } },
            { typeof(sbyte[, ,]), (c,o) => { var v = (sbyte[, ,])o; c.CodeSByteArray3d(ref v); } },
            { typeof(short[,]), (c,o) => { var v = (short[,])o; c.CodeShortArray2d(ref v); } },
            { typeof(short[, ,]), (c,o) => { var v = (short[, ,])o; c.CodeShortArray3d(ref v); } },
            { typeof(ushort[,]), (c,o) => { var v = (ushort[,])o; c.CodeUShortArray2d(ref v); } },
            { typeof(ushort[, ,]), (c,o) => { var v = (ushort[, ,])o; c.CodeUShortArray3d(ref v); } },
            { typeof(int[,]), (c,o) => { var v = (int[,])o; c.CodeIntArray2d(ref v); } },
            { typeof(int[, ,]), (c,o) => { var v = (int[, ,])o; c.CodeIntArray3d(ref v); } },
            { typeof(uint[,]), (c,o) => { var v = (uint[,])o; c.CodeUIntArray2d(ref v); } },
            { typeof(uint[, ,]), (c,o) => { var v = (uint[, ,])o; c.CodeUIntArray3d(ref v); } },
            { typeof(long[,]), (c,o) => { var v = (long[,])o; c.CodeLongArray2d(ref v); } },
            { typeof(long[, ,]), (c,o) => { var v = (long[, ,])o; c.CodeLongArray3d(ref v); } },
            { typeof(ulong[,]), (c,o) => { var v = (ulong[,])o; c.CodeULongArray2d(ref v); } },
            { typeof(ulong[, ,]), (c,o) => { var v = (ulong[, ,])o; c.CodeULongArray3d(ref v); } },
            { typeof(float[,]), (c,o) => { var v = (float[,])o; c.CodeFloatArray2d(ref v); } },
            { typeof(float[, ,]), (c,o) => { var v = (float[, ,])o; c.CodeFloatArray3d(ref v); } },
            { typeof(double[,]), (c,o) => { var v = (double[,])o; c.CodeDoubleArray2d(ref v); } },
            { typeof(double[, ,]), (c,o) => { var v = (double[, ,])o; c.CodeDoubleArray3d(ref v); } },
            { typeof(Fraction[,]), (c,o) => { var v = (Fraction[,])o; c.CodeFractionArray2d(ref v); } },
            { typeof(Fraction[, ,]), (c,o) => { var v = (Fraction[, ,])o; c.CodeFractionArray3d(ref v); } },
            { typeof(V2i[,]), (c,o) => { var v = (V2i[,])o; c.CodeV2iArray2d(ref v); } },
            { typeof(V2i[, ,]), (c,o) => { var v = (V2i[, ,])o; c.CodeV2iArray3d(ref v); } },
            { typeof(V2l[,]), (c,o) => { var v = (V2l[,])o; c.CodeV2lArray2d(ref v); } },
            { typeof(V2l[, ,]), (c,o) => { var v = (V2l[, ,])o; c.CodeV2lArray3d(ref v); } },
            { typeof(V2f[,]), (c,o) => { var v = (V2f[,])o; c.CodeV2fArray2d(ref v); } },
            { typeof(V2f[, ,]), (c,o) => { var v = (V2f[, ,])o; c.CodeV2fArray3d(ref v); } },
            { typeof(V2d[,]), (c,o) => { var v = (V2d[,])o; c.CodeV2dArray2d(ref v); } },
            { typeof(V2d[, ,]), (c,o) => { var v = (V2d[, ,])o; c.CodeV2dArray3d(ref v); } },
            { typeof(V3i[,]), (c,o) => { var v = (V3i[,])o; c.CodeV3iArray2d(ref v); } },
            { typeof(V3i[, ,]), (c,o) => { var v = (V3i[, ,])o; c.CodeV3iArray3d(ref v); } },
            { typeof(V3l[,]), (c,o) => { var v = (V3l[,])o; c.CodeV3lArray2d(ref v); } },
            { typeof(V3l[, ,]), (c,o) => { var v = (V3l[, ,])o; c.CodeV3lArray3d(ref v); } },
            { typeof(V3f[,]), (c,o) => { var v = (V3f[,])o; c.CodeV3fArray2d(ref v); } },
            { typeof(V3f[, ,]), (c,o) => { var v = (V3f[, ,])o; c.CodeV3fArray3d(ref v); } },
            { typeof(V3d[,]), (c,o) => { var v = (V3d[,])o; c.CodeV3dArray2d(ref v); } },
            { typeof(V3d[, ,]), (c,o) => { var v = (V3d[, ,])o; c.CodeV3dArray3d(ref v); } },
            { typeof(V4i[,]), (c,o) => { var v = (V4i[,])o; c.CodeV4iArray2d(ref v); } },
            { typeof(V4i[, ,]), (c,o) => { var v = (V4i[, ,])o; c.CodeV4iArray3d(ref v); } },
            { typeof(V4l[,]), (c,o) => { var v = (V4l[,])o; c.CodeV4lArray2d(ref v); } },
            { typeof(V4l[, ,]), (c,o) => { var v = (V4l[, ,])o; c.CodeV4lArray3d(ref v); } },
            { typeof(V4f[,]), (c,o) => { var v = (V4f[,])o; c.CodeV4fArray2d(ref v); } },
            { typeof(V4f[, ,]), (c,o) => { var v = (V4f[, ,])o; c.CodeV4fArray3d(ref v); } },
            { typeof(V4d[,]), (c,o) => { var v = (V4d[,])o; c.CodeV4dArray2d(ref v); } },
            { typeof(V4d[, ,]), (c,o) => { var v = (V4d[, ,])o; c.CodeV4dArray3d(ref v); } },
            { typeof(M22i[,]), (c,o) => { var v = (M22i[,])o; c.CodeM22iArray2d(ref v); } },
            { typeof(M22i[, ,]), (c,o) => { var v = (M22i[, ,])o; c.CodeM22iArray3d(ref v); } },
            { typeof(M22l[,]), (c,o) => { var v = (M22l[,])o; c.CodeM22lArray2d(ref v); } },
            { typeof(M22l[, ,]), (c,o) => { var v = (M22l[, ,])o; c.CodeM22lArray3d(ref v); } },
            { typeof(M22f[,]), (c,o) => { var v = (M22f[,])o; c.CodeM22fArray2d(ref v); } },
            { typeof(M22f[, ,]), (c,o) => { var v = (M22f[, ,])o; c.CodeM22fArray3d(ref v); } },
            { typeof(M22d[,]), (c,o) => { var v = (M22d[,])o; c.CodeM22dArray2d(ref v); } },
            { typeof(M22d[, ,]), (c,o) => { var v = (M22d[, ,])o; c.CodeM22dArray3d(ref v); } },
            { typeof(M23i[,]), (c,o) => { var v = (M23i[,])o; c.CodeM23iArray2d(ref v); } },
            { typeof(M23i[, ,]), (c,o) => { var v = (M23i[, ,])o; c.CodeM23iArray3d(ref v); } },
            { typeof(M23l[,]), (c,o) => { var v = (M23l[,])o; c.CodeM23lArray2d(ref v); } },
            { typeof(M23l[, ,]), (c,o) => { var v = (M23l[, ,])o; c.CodeM23lArray3d(ref v); } },
            { typeof(M23f[,]), (c,o) => { var v = (M23f[,])o; c.CodeM23fArray2d(ref v); } },
            { typeof(M23f[, ,]), (c,o) => { var v = (M23f[, ,])o; c.CodeM23fArray3d(ref v); } },
            { typeof(M23d[,]), (c,o) => { var v = (M23d[,])o; c.CodeM23dArray2d(ref v); } },
            { typeof(M23d[, ,]), (c,o) => { var v = (M23d[, ,])o; c.CodeM23dArray3d(ref v); } },
            { typeof(M33i[,]), (c,o) => { var v = (M33i[,])o; c.CodeM33iArray2d(ref v); } },
            { typeof(M33i[, ,]), (c,o) => { var v = (M33i[, ,])o; c.CodeM33iArray3d(ref v); } },
            { typeof(M33l[,]), (c,o) => { var v = (M33l[,])o; c.CodeM33lArray2d(ref v); } },
            { typeof(M33l[, ,]), (c,o) => { var v = (M33l[, ,])o; c.CodeM33lArray3d(ref v); } },
            { typeof(M33f[,]), (c,o) => { var v = (M33f[,])o; c.CodeM33fArray2d(ref v); } },
            { typeof(M33f[, ,]), (c,o) => { var v = (M33f[, ,])o; c.CodeM33fArray3d(ref v); } },
            { typeof(M33d[,]), (c,o) => { var v = (M33d[,])o; c.CodeM33dArray2d(ref v); } },
            { typeof(M33d[, ,]), (c,o) => { var v = (M33d[, ,])o; c.CodeM33dArray3d(ref v); } },
            { typeof(M34i[,]), (c,o) => { var v = (M34i[,])o; c.CodeM34iArray2d(ref v); } },
            { typeof(M34i[, ,]), (c,o) => { var v = (M34i[, ,])o; c.CodeM34iArray3d(ref v); } },
            { typeof(M34l[,]), (c,o) => { var v = (M34l[,])o; c.CodeM34lArray2d(ref v); } },
            { typeof(M34l[, ,]), (c,o) => { var v = (M34l[, ,])o; c.CodeM34lArray3d(ref v); } },
            { typeof(M34f[,]), (c,o) => { var v = (M34f[,])o; c.CodeM34fArray2d(ref v); } },
            { typeof(M34f[, ,]), (c,o) => { var v = (M34f[, ,])o; c.CodeM34fArray3d(ref v); } },
            { typeof(M34d[,]), (c,o) => { var v = (M34d[,])o; c.CodeM34dArray2d(ref v); } },
            { typeof(M34d[, ,]), (c,o) => { var v = (M34d[, ,])o; c.CodeM34dArray3d(ref v); } },
            { typeof(M44i[,]), (c,o) => { var v = (M44i[,])o; c.CodeM44iArray2d(ref v); } },
            { typeof(M44i[, ,]), (c,o) => { var v = (M44i[, ,])o; c.CodeM44iArray3d(ref v); } },
            { typeof(M44l[,]), (c,o) => { var v = (M44l[,])o; c.CodeM44lArray2d(ref v); } },
            { typeof(M44l[, ,]), (c,o) => { var v = (M44l[, ,])o; c.CodeM44lArray3d(ref v); } },
            { typeof(M44f[,]), (c,o) => { var v = (M44f[,])o; c.CodeM44fArray2d(ref v); } },
            { typeof(M44f[, ,]), (c,o) => { var v = (M44f[, ,])o; c.CodeM44fArray3d(ref v); } },
            { typeof(M44d[,]), (c,o) => { var v = (M44d[,])o; c.CodeM44dArray2d(ref v); } },
            { typeof(M44d[, ,]), (c,o) => { var v = (M44d[, ,])o; c.CodeM44dArray3d(ref v); } },
            { typeof(C3b[,]), (c,o) => { var v = (C3b[,])o; c.CodeC3bArray2d(ref v); } },
            { typeof(C3b[, ,]), (c,o) => { var v = (C3b[, ,])o; c.CodeC3bArray3d(ref v); } },
            { typeof(C3us[,]), (c,o) => { var v = (C3us[,])o; c.CodeC3usArray2d(ref v); } },
            { typeof(C3us[, ,]), (c,o) => { var v = (C3us[, ,])o; c.CodeC3usArray3d(ref v); } },
            { typeof(C3ui[,]), (c,o) => { var v = (C3ui[,])o; c.CodeC3uiArray2d(ref v); } },
            { typeof(C3ui[, ,]), (c,o) => { var v = (C3ui[, ,])o; c.CodeC3uiArray3d(ref v); } },
            { typeof(C3f[,]), (c,o) => { var v = (C3f[,])o; c.CodeC3fArray2d(ref v); } },
            { typeof(C3f[, ,]), (c,o) => { var v = (C3f[, ,])o; c.CodeC3fArray3d(ref v); } },
            { typeof(C3d[,]), (c,o) => { var v = (C3d[,])o; c.CodeC3dArray2d(ref v); } },
            { typeof(C3d[, ,]), (c,o) => { var v = (C3d[, ,])o; c.CodeC3dArray3d(ref v); } },
            { typeof(C4b[,]), (c,o) => { var v = (C4b[,])o; c.CodeC4bArray2d(ref v); } },
            { typeof(C4b[, ,]), (c,o) => { var v = (C4b[, ,])o; c.CodeC4bArray3d(ref v); } },
            { typeof(C4us[,]), (c,o) => { var v = (C4us[,])o; c.CodeC4usArray2d(ref v); } },
            { typeof(C4us[, ,]), (c,o) => { var v = (C4us[, ,])o; c.CodeC4usArray3d(ref v); } },
            { typeof(C4ui[,]), (c,o) => { var v = (C4ui[,])o; c.CodeC4uiArray2d(ref v); } },
            { typeof(C4ui[, ,]), (c,o) => { var v = (C4ui[, ,])o; c.CodeC4uiArray3d(ref v); } },
            { typeof(C4f[,]), (c,o) => { var v = (C4f[,])o; c.CodeC4fArray2d(ref v); } },
            { typeof(C4f[, ,]), (c,o) => { var v = (C4f[, ,])o; c.CodeC4fArray3d(ref v); } },
            { typeof(C4d[,]), (c,o) => { var v = (C4d[,])o; c.CodeC4dArray2d(ref v); } },
            { typeof(C4d[, ,]), (c,o) => { var v = (C4d[, ,])o; c.CodeC4dArray3d(ref v); } },
            { typeof(Range1b[,]), (c,o) => { var v = (Range1b[,])o; c.CodeRange1bArray2d(ref v); } },
            { typeof(Range1b[, ,]), (c,o) => { var v = (Range1b[, ,])o; c.CodeRange1bArray3d(ref v); } },
            { typeof(Range1sb[,]), (c,o) => { var v = (Range1sb[,])o; c.CodeRange1sbArray2d(ref v); } },
            { typeof(Range1sb[, ,]), (c,o) => { var v = (Range1sb[, ,])o; c.CodeRange1sbArray3d(ref v); } },
            { typeof(Range1s[,]), (c,o) => { var v = (Range1s[,])o; c.CodeRange1sArray2d(ref v); } },
            { typeof(Range1s[, ,]), (c,o) => { var v = (Range1s[, ,])o; c.CodeRange1sArray3d(ref v); } },
            { typeof(Range1us[,]), (c,o) => { var v = (Range1us[,])o; c.CodeRange1usArray2d(ref v); } },
            { typeof(Range1us[, ,]), (c,o) => { var v = (Range1us[, ,])o; c.CodeRange1usArray3d(ref v); } },
            { typeof(Range1i[,]), (c,o) => { var v = (Range1i[,])o; c.CodeRange1iArray2d(ref v); } },
            { typeof(Range1i[, ,]), (c,o) => { var v = (Range1i[, ,])o; c.CodeRange1iArray3d(ref v); } },
            { typeof(Range1ui[,]), (c,o) => { var v = (Range1ui[,])o; c.CodeRange1uiArray2d(ref v); } },
            { typeof(Range1ui[, ,]), (c,o) => { var v = (Range1ui[, ,])o; c.CodeRange1uiArray3d(ref v); } },
            { typeof(Range1l[,]), (c,o) => { var v = (Range1l[,])o; c.CodeRange1lArray2d(ref v); } },
            { typeof(Range1l[, ,]), (c,o) => { var v = (Range1l[, ,])o; c.CodeRange1lArray3d(ref v); } },
            { typeof(Range1ul[,]), (c,o) => { var v = (Range1ul[,])o; c.CodeRange1ulArray2d(ref v); } },
            { typeof(Range1ul[, ,]), (c,o) => { var v = (Range1ul[, ,])o; c.CodeRange1ulArray3d(ref v); } },
            { typeof(Range1f[,]), (c,o) => { var v = (Range1f[,])o; c.CodeRange1fArray2d(ref v); } },
            { typeof(Range1f[, ,]), (c,o) => { var v = (Range1f[, ,])o; c.CodeRange1fArray3d(ref v); } },
            { typeof(Range1d[,]), (c,o) => { var v = (Range1d[,])o; c.CodeRange1dArray2d(ref v); } },
            { typeof(Range1d[, ,]), (c,o) => { var v = (Range1d[, ,])o; c.CodeRange1dArray3d(ref v); } },
            { typeof(Box2i[,]), (c,o) => { var v = (Box2i[,])o; c.CodeBox2iArray2d(ref v); } },
            { typeof(Box2i[, ,]), (c,o) => { var v = (Box2i[, ,])o; c.CodeBox2iArray3d(ref v); } },
            { typeof(Box2l[,]), (c,o) => { var v = (Box2l[,])o; c.CodeBox2lArray2d(ref v); } },
            { typeof(Box2l[, ,]), (c,o) => { var v = (Box2l[, ,])o; c.CodeBox2lArray3d(ref v); } },
            { typeof(Box2f[,]), (c,o) => { var v = (Box2f[,])o; c.CodeBox2fArray2d(ref v); } },
            { typeof(Box2f[, ,]), (c,o) => { var v = (Box2f[, ,])o; c.CodeBox2fArray3d(ref v); } },
            { typeof(Box2d[,]), (c,o) => { var v = (Box2d[,])o; c.CodeBox2dArray2d(ref v); } },
            { typeof(Box2d[, ,]), (c,o) => { var v = (Box2d[, ,])o; c.CodeBox2dArray3d(ref v); } },
            { typeof(Box3i[,]), (c,o) => { var v = (Box3i[,])o; c.CodeBox3iArray2d(ref v); } },
            { typeof(Box3i[, ,]), (c,o) => { var v = (Box3i[, ,])o; c.CodeBox3iArray3d(ref v); } },
            { typeof(Box3l[,]), (c,o) => { var v = (Box3l[,])o; c.CodeBox3lArray2d(ref v); } },
            { typeof(Box3l[, ,]), (c,o) => { var v = (Box3l[, ,])o; c.CodeBox3lArray3d(ref v); } },
            { typeof(Box3f[,]), (c,o) => { var v = (Box3f[,])o; c.CodeBox3fArray2d(ref v); } },
            { typeof(Box3f[, ,]), (c,o) => { var v = (Box3f[, ,])o; c.CodeBox3fArray3d(ref v); } },
            { typeof(Box3d[,]), (c,o) => { var v = (Box3d[,])o; c.CodeBox3dArray2d(ref v); } },
            { typeof(Box3d[, ,]), (c,o) => { var v = (Box3d[, ,])o; c.CodeBox3dArray3d(ref v); } },
            { typeof(Euclidean3f[,]), (c,o) => { var v = (Euclidean3f[,])o; c.CodeEuclidean3fArray2d(ref v); } },
            { typeof(Euclidean3f[, ,]), (c,o) => { var v = (Euclidean3f[, ,])o; c.CodeEuclidean3fArray3d(ref v); } },
            { typeof(Euclidean3d[,]), (c,o) => { var v = (Euclidean3d[,])o; c.CodeEuclidean3dArray2d(ref v); } },
            { typeof(Euclidean3d[, ,]), (c,o) => { var v = (Euclidean3d[, ,])o; c.CodeEuclidean3dArray3d(ref v); } },
            { typeof(Rot2f[,]), (c,o) => { var v = (Rot2f[,])o; c.CodeRot2fArray2d(ref v); } },
            { typeof(Rot2f[, ,]), (c,o) => { var v = (Rot2f[, ,])o; c.CodeRot2fArray3d(ref v); } },
            { typeof(Rot2d[,]), (c,o) => { var v = (Rot2d[,])o; c.CodeRot2dArray2d(ref v); } },
            { typeof(Rot2d[, ,]), (c,o) => { var v = (Rot2d[, ,])o; c.CodeRot2dArray3d(ref v); } },
            { typeof(Rot3f[,]), (c,o) => { var v = (Rot3f[,])o; c.CodeRot3fArray2d(ref v); } },
            { typeof(Rot3f[, ,]), (c,o) => { var v = (Rot3f[, ,])o; c.CodeRot3fArray3d(ref v); } },
            { typeof(Rot3d[,]), (c,o) => { var v = (Rot3d[,])o; c.CodeRot3dArray2d(ref v); } },
            { typeof(Rot3d[, ,]), (c,o) => { var v = (Rot3d[, ,])o; c.CodeRot3dArray3d(ref v); } },
            { typeof(Scale3f[,]), (c,o) => { var v = (Scale3f[,])o; c.CodeScale3fArray2d(ref v); } },
            { typeof(Scale3f[, ,]), (c,o) => { var v = (Scale3f[, ,])o; c.CodeScale3fArray3d(ref v); } },
            { typeof(Scale3d[,]), (c,o) => { var v = (Scale3d[,])o; c.CodeScale3dArray2d(ref v); } },
            { typeof(Scale3d[, ,]), (c,o) => { var v = (Scale3d[, ,])o; c.CodeScale3dArray3d(ref v); } },
            { typeof(Shift3f[,]), (c,o) => { var v = (Shift3f[,])o; c.CodeShift3fArray2d(ref v); } },
            { typeof(Shift3f[, ,]), (c,o) => { var v = (Shift3f[, ,])o; c.CodeShift3fArray3d(ref v); } },
            { typeof(Shift3d[,]), (c,o) => { var v = (Shift3d[,])o; c.CodeShift3dArray2d(ref v); } },
            { typeof(Shift3d[, ,]), (c,o) => { var v = (Shift3d[, ,])o; c.CodeShift3dArray3d(ref v); } },
            { typeof(Trafo2f[,]), (c,o) => { var v = (Trafo2f[,])o; c.CodeTrafo2fArray2d(ref v); } },
            { typeof(Trafo2f[, ,]), (c,o) => { var v = (Trafo2f[, ,])o; c.CodeTrafo2fArray3d(ref v); } },
            { typeof(Trafo2d[,]), (c,o) => { var v = (Trafo2d[,])o; c.CodeTrafo2dArray2d(ref v); } },
            { typeof(Trafo2d[, ,]), (c,o) => { var v = (Trafo2d[, ,])o; c.CodeTrafo2dArray3d(ref v); } },
            { typeof(Trafo3f[,]), (c,o) => { var v = (Trafo3f[,])o; c.CodeTrafo3fArray2d(ref v); } },
            { typeof(Trafo3f[, ,]), (c,o) => { var v = (Trafo3f[, ,])o; c.CodeTrafo3fArray3d(ref v); } },
            { typeof(Trafo3d[,]), (c,o) => { var v = (Trafo3d[,])o; c.CodeTrafo3dArray2d(ref v); } },
            { typeof(Trafo3d[, ,]), (c,o) => { var v = (Trafo3d[, ,])o; c.CodeTrafo3dArray3d(ref v); } },

            #endregion

            #region Jagged Multi-Dimensional Arrays

            { typeof(byte[][]), (c,o) => { var v = (byte[][])o; c.CodeByteArrayArray(ref v); } },
            { typeof(byte[][][]), (c,o) => { var v = (byte[][][])o; c.CodeByteArrayArrayArray(ref v); } },
            { typeof(sbyte[][]), (c,o) => { var v = (sbyte[][])o; c.CodeSByteArrayArray(ref v); } },
            { typeof(sbyte[][][]), (c,o) => { var v = (sbyte[][][])o; c.CodeSByteArrayArrayArray(ref v); } },
            { typeof(short[][]), (c,o) => { var v = (short[][])o; c.CodeShortArrayArray(ref v); } },
            { typeof(short[][][]), (c,o) => { var v = (short[][][])o; c.CodeShortArrayArrayArray(ref v); } },
            { typeof(ushort[][]), (c,o) => { var v = (ushort[][])o; c.CodeUShortArrayArray(ref v); } },
            { typeof(ushort[][][]), (c,o) => { var v = (ushort[][][])o; c.CodeUShortArrayArrayArray(ref v); } },
            { typeof(int[][]), (c,o) => { var v = (int[][])o; c.CodeIntArrayArray(ref v); } },
            { typeof(int[][][]), (c,o) => { var v = (int[][][])o; c.CodeIntArrayArrayArray(ref v); } },
            { typeof(uint[][]), (c,o) => { var v = (uint[][])o; c.CodeUIntArrayArray(ref v); } },
            { typeof(uint[][][]), (c,o) => { var v = (uint[][][])o; c.CodeUIntArrayArrayArray(ref v); } },
            { typeof(long[][]), (c,o) => { var v = (long[][])o; c.CodeLongArrayArray(ref v); } },
            { typeof(long[][][]), (c,o) => { var v = (long[][][])o; c.CodeLongArrayArrayArray(ref v); } },
            { typeof(ulong[][]), (c,o) => { var v = (ulong[][])o; c.CodeULongArrayArray(ref v); } },
            { typeof(ulong[][][]), (c,o) => { var v = (ulong[][][])o; c.CodeULongArrayArrayArray(ref v); } },
            { typeof(float[][]), (c,o) => { var v = (float[][])o; c.CodeFloatArrayArray(ref v); } },
            { typeof(float[][][]), (c,o) => { var v = (float[][][])o; c.CodeFloatArrayArrayArray(ref v); } },
            { typeof(double[][]), (c,o) => { var v = (double[][])o; c.CodeDoubleArrayArray(ref v); } },
            { typeof(double[][][]), (c,o) => { var v = (double[][][])o; c.CodeDoubleArrayArrayArray(ref v); } },
            { typeof(Fraction[][]), (c,o) => { var v = (Fraction[][])o; c.CodeFractionArrayArray(ref v); } },
            { typeof(Fraction[][][]), (c,o) => { var v = (Fraction[][][])o; c.CodeFractionArrayArrayArray(ref v); } },
            { typeof(V2i[][]), (c,o) => { var v = (V2i[][])o; c.CodeV2iArrayArray(ref v); } },
            { typeof(V2i[][][]), (c,o) => { var v = (V2i[][][])o; c.CodeV2iArrayArrayArray(ref v); } },
            { typeof(V2l[][]), (c,o) => { var v = (V2l[][])o; c.CodeV2lArrayArray(ref v); } },
            { typeof(V2l[][][]), (c,o) => { var v = (V2l[][][])o; c.CodeV2lArrayArrayArray(ref v); } },
            { typeof(V2f[][]), (c,o) => { var v = (V2f[][])o; c.CodeV2fArrayArray(ref v); } },
            { typeof(V2f[][][]), (c,o) => { var v = (V2f[][][])o; c.CodeV2fArrayArrayArray(ref v); } },
            { typeof(V2d[][]), (c,o) => { var v = (V2d[][])o; c.CodeV2dArrayArray(ref v); } },
            { typeof(V2d[][][]), (c,o) => { var v = (V2d[][][])o; c.CodeV2dArrayArrayArray(ref v); } },
            { typeof(V3i[][]), (c,o) => { var v = (V3i[][])o; c.CodeV3iArrayArray(ref v); } },
            { typeof(V3i[][][]), (c,o) => { var v = (V3i[][][])o; c.CodeV3iArrayArrayArray(ref v); } },
            { typeof(V3l[][]), (c,o) => { var v = (V3l[][])o; c.CodeV3lArrayArray(ref v); } },
            { typeof(V3l[][][]), (c,o) => { var v = (V3l[][][])o; c.CodeV3lArrayArrayArray(ref v); } },
            { typeof(V3f[][]), (c,o) => { var v = (V3f[][])o; c.CodeV3fArrayArray(ref v); } },
            { typeof(V3f[][][]), (c,o) => { var v = (V3f[][][])o; c.CodeV3fArrayArrayArray(ref v); } },
            { typeof(V3d[][]), (c,o) => { var v = (V3d[][])o; c.CodeV3dArrayArray(ref v); } },
            { typeof(V3d[][][]), (c,o) => { var v = (V3d[][][])o; c.CodeV3dArrayArrayArray(ref v); } },
            { typeof(V4i[][]), (c,o) => { var v = (V4i[][])o; c.CodeV4iArrayArray(ref v); } },
            { typeof(V4i[][][]), (c,o) => { var v = (V4i[][][])o; c.CodeV4iArrayArrayArray(ref v); } },
            { typeof(V4l[][]), (c,o) => { var v = (V4l[][])o; c.CodeV4lArrayArray(ref v); } },
            { typeof(V4l[][][]), (c,o) => { var v = (V4l[][][])o; c.CodeV4lArrayArrayArray(ref v); } },
            { typeof(V4f[][]), (c,o) => { var v = (V4f[][])o; c.CodeV4fArrayArray(ref v); } },
            { typeof(V4f[][][]), (c,o) => { var v = (V4f[][][])o; c.CodeV4fArrayArrayArray(ref v); } },
            { typeof(V4d[][]), (c,o) => { var v = (V4d[][])o; c.CodeV4dArrayArray(ref v); } },
            { typeof(V4d[][][]), (c,o) => { var v = (V4d[][][])o; c.CodeV4dArrayArrayArray(ref v); } },
            { typeof(M22i[][]), (c,o) => { var v = (M22i[][])o; c.CodeM22iArrayArray(ref v); } },
            { typeof(M22i[][][]), (c,o) => { var v = (M22i[][][])o; c.CodeM22iArrayArrayArray(ref v); } },
            { typeof(M22l[][]), (c,o) => { var v = (M22l[][])o; c.CodeM22lArrayArray(ref v); } },
            { typeof(M22l[][][]), (c,o) => { var v = (M22l[][][])o; c.CodeM22lArrayArrayArray(ref v); } },
            { typeof(M22f[][]), (c,o) => { var v = (M22f[][])o; c.CodeM22fArrayArray(ref v); } },
            { typeof(M22f[][][]), (c,o) => { var v = (M22f[][][])o; c.CodeM22fArrayArrayArray(ref v); } },
            { typeof(M22d[][]), (c,o) => { var v = (M22d[][])o; c.CodeM22dArrayArray(ref v); } },
            { typeof(M22d[][][]), (c,o) => { var v = (M22d[][][])o; c.CodeM22dArrayArrayArray(ref v); } },
            { typeof(M23i[][]), (c,o) => { var v = (M23i[][])o; c.CodeM23iArrayArray(ref v); } },
            { typeof(M23i[][][]), (c,o) => { var v = (M23i[][][])o; c.CodeM23iArrayArrayArray(ref v); } },
            { typeof(M23l[][]), (c,o) => { var v = (M23l[][])o; c.CodeM23lArrayArray(ref v); } },
            { typeof(M23l[][][]), (c,o) => { var v = (M23l[][][])o; c.CodeM23lArrayArrayArray(ref v); } },
            { typeof(M23f[][]), (c,o) => { var v = (M23f[][])o; c.CodeM23fArrayArray(ref v); } },
            { typeof(M23f[][][]), (c,o) => { var v = (M23f[][][])o; c.CodeM23fArrayArrayArray(ref v); } },
            { typeof(M23d[][]), (c,o) => { var v = (M23d[][])o; c.CodeM23dArrayArray(ref v); } },
            { typeof(M23d[][][]), (c,o) => { var v = (M23d[][][])o; c.CodeM23dArrayArrayArray(ref v); } },
            { typeof(M33i[][]), (c,o) => { var v = (M33i[][])o; c.CodeM33iArrayArray(ref v); } },
            { typeof(M33i[][][]), (c,o) => { var v = (M33i[][][])o; c.CodeM33iArrayArrayArray(ref v); } },
            { typeof(M33l[][]), (c,o) => { var v = (M33l[][])o; c.CodeM33lArrayArray(ref v); } },
            { typeof(M33l[][][]), (c,o) => { var v = (M33l[][][])o; c.CodeM33lArrayArrayArray(ref v); } },
            { typeof(M33f[][]), (c,o) => { var v = (M33f[][])o; c.CodeM33fArrayArray(ref v); } },
            { typeof(M33f[][][]), (c,o) => { var v = (M33f[][][])o; c.CodeM33fArrayArrayArray(ref v); } },
            { typeof(M33d[][]), (c,o) => { var v = (M33d[][])o; c.CodeM33dArrayArray(ref v); } },
            { typeof(M33d[][][]), (c,o) => { var v = (M33d[][][])o; c.CodeM33dArrayArrayArray(ref v); } },
            { typeof(M34i[][]), (c,o) => { var v = (M34i[][])o; c.CodeM34iArrayArray(ref v); } },
            { typeof(M34i[][][]), (c,o) => { var v = (M34i[][][])o; c.CodeM34iArrayArrayArray(ref v); } },
            { typeof(M34l[][]), (c,o) => { var v = (M34l[][])o; c.CodeM34lArrayArray(ref v); } },
            { typeof(M34l[][][]), (c,o) => { var v = (M34l[][][])o; c.CodeM34lArrayArrayArray(ref v); } },
            { typeof(M34f[][]), (c,o) => { var v = (M34f[][])o; c.CodeM34fArrayArray(ref v); } },
            { typeof(M34f[][][]), (c,o) => { var v = (M34f[][][])o; c.CodeM34fArrayArrayArray(ref v); } },
            { typeof(M34d[][]), (c,o) => { var v = (M34d[][])o; c.CodeM34dArrayArray(ref v); } },
            { typeof(M34d[][][]), (c,o) => { var v = (M34d[][][])o; c.CodeM34dArrayArrayArray(ref v); } },
            { typeof(M44i[][]), (c,o) => { var v = (M44i[][])o; c.CodeM44iArrayArray(ref v); } },
            { typeof(M44i[][][]), (c,o) => { var v = (M44i[][][])o; c.CodeM44iArrayArrayArray(ref v); } },
            { typeof(M44l[][]), (c,o) => { var v = (M44l[][])o; c.CodeM44lArrayArray(ref v); } },
            { typeof(M44l[][][]), (c,o) => { var v = (M44l[][][])o; c.CodeM44lArrayArrayArray(ref v); } },
            { typeof(M44f[][]), (c,o) => { var v = (M44f[][])o; c.CodeM44fArrayArray(ref v); } },
            { typeof(M44f[][][]), (c,o) => { var v = (M44f[][][])o; c.CodeM44fArrayArrayArray(ref v); } },
            { typeof(M44d[][]), (c,o) => { var v = (M44d[][])o; c.CodeM44dArrayArray(ref v); } },
            { typeof(M44d[][][]), (c,o) => { var v = (M44d[][][])o; c.CodeM44dArrayArrayArray(ref v); } },
            { typeof(C3b[][]), (c,o) => { var v = (C3b[][])o; c.CodeC3bArrayArray(ref v); } },
            { typeof(C3b[][][]), (c,o) => { var v = (C3b[][][])o; c.CodeC3bArrayArrayArray(ref v); } },
            { typeof(C3us[][]), (c,o) => { var v = (C3us[][])o; c.CodeC3usArrayArray(ref v); } },
            { typeof(C3us[][][]), (c,o) => { var v = (C3us[][][])o; c.CodeC3usArrayArrayArray(ref v); } },
            { typeof(C3ui[][]), (c,o) => { var v = (C3ui[][])o; c.CodeC3uiArrayArray(ref v); } },
            { typeof(C3ui[][][]), (c,o) => { var v = (C3ui[][][])o; c.CodeC3uiArrayArrayArray(ref v); } },
            { typeof(C3f[][]), (c,o) => { var v = (C3f[][])o; c.CodeC3fArrayArray(ref v); } },
            { typeof(C3f[][][]), (c,o) => { var v = (C3f[][][])o; c.CodeC3fArrayArrayArray(ref v); } },
            { typeof(C3d[][]), (c,o) => { var v = (C3d[][])o; c.CodeC3dArrayArray(ref v); } },
            { typeof(C3d[][][]), (c,o) => { var v = (C3d[][][])o; c.CodeC3dArrayArrayArray(ref v); } },
            { typeof(C4b[][]), (c,o) => { var v = (C4b[][])o; c.CodeC4bArrayArray(ref v); } },
            { typeof(C4b[][][]), (c,o) => { var v = (C4b[][][])o; c.CodeC4bArrayArrayArray(ref v); } },
            { typeof(C4us[][]), (c,o) => { var v = (C4us[][])o; c.CodeC4usArrayArray(ref v); } },
            { typeof(C4us[][][]), (c,o) => { var v = (C4us[][][])o; c.CodeC4usArrayArrayArray(ref v); } },
            { typeof(C4ui[][]), (c,o) => { var v = (C4ui[][])o; c.CodeC4uiArrayArray(ref v); } },
            { typeof(C4ui[][][]), (c,o) => { var v = (C4ui[][][])o; c.CodeC4uiArrayArrayArray(ref v); } },
            { typeof(C4f[][]), (c,o) => { var v = (C4f[][])o; c.CodeC4fArrayArray(ref v); } },
            { typeof(C4f[][][]), (c,o) => { var v = (C4f[][][])o; c.CodeC4fArrayArrayArray(ref v); } },
            { typeof(C4d[][]), (c,o) => { var v = (C4d[][])o; c.CodeC4dArrayArray(ref v); } },
            { typeof(C4d[][][]), (c,o) => { var v = (C4d[][][])o; c.CodeC4dArrayArrayArray(ref v); } },
            { typeof(Range1b[][]), (c,o) => { var v = (Range1b[][])o; c.CodeRange1bArrayArray(ref v); } },
            { typeof(Range1b[][][]), (c,o) => { var v = (Range1b[][][])o; c.CodeRange1bArrayArrayArray(ref v); } },
            { typeof(Range1sb[][]), (c,o) => { var v = (Range1sb[][])o; c.CodeRange1sbArrayArray(ref v); } },
            { typeof(Range1sb[][][]), (c,o) => { var v = (Range1sb[][][])o; c.CodeRange1sbArrayArrayArray(ref v); } },
            { typeof(Range1s[][]), (c,o) => { var v = (Range1s[][])o; c.CodeRange1sArrayArray(ref v); } },
            { typeof(Range1s[][][]), (c,o) => { var v = (Range1s[][][])o; c.CodeRange1sArrayArrayArray(ref v); } },
            { typeof(Range1us[][]), (c,o) => { var v = (Range1us[][])o; c.CodeRange1usArrayArray(ref v); } },
            { typeof(Range1us[][][]), (c,o) => { var v = (Range1us[][][])o; c.CodeRange1usArrayArrayArray(ref v); } },
            { typeof(Range1i[][]), (c,o) => { var v = (Range1i[][])o; c.CodeRange1iArrayArray(ref v); } },
            { typeof(Range1i[][][]), (c,o) => { var v = (Range1i[][][])o; c.CodeRange1iArrayArrayArray(ref v); } },
            { typeof(Range1ui[][]), (c,o) => { var v = (Range1ui[][])o; c.CodeRange1uiArrayArray(ref v); } },
            { typeof(Range1ui[][][]), (c,o) => { var v = (Range1ui[][][])o; c.CodeRange1uiArrayArrayArray(ref v); } },
            { typeof(Range1l[][]), (c,o) => { var v = (Range1l[][])o; c.CodeRange1lArrayArray(ref v); } },
            { typeof(Range1l[][][]), (c,o) => { var v = (Range1l[][][])o; c.CodeRange1lArrayArrayArray(ref v); } },
            { typeof(Range1ul[][]), (c,o) => { var v = (Range1ul[][])o; c.CodeRange1ulArrayArray(ref v); } },
            { typeof(Range1ul[][][]), (c,o) => { var v = (Range1ul[][][])o; c.CodeRange1ulArrayArrayArray(ref v); } },
            { typeof(Range1f[][]), (c,o) => { var v = (Range1f[][])o; c.CodeRange1fArrayArray(ref v); } },
            { typeof(Range1f[][][]), (c,o) => { var v = (Range1f[][][])o; c.CodeRange1fArrayArrayArray(ref v); } },
            { typeof(Range1d[][]), (c,o) => { var v = (Range1d[][])o; c.CodeRange1dArrayArray(ref v); } },
            { typeof(Range1d[][][]), (c,o) => { var v = (Range1d[][][])o; c.CodeRange1dArrayArrayArray(ref v); } },
            { typeof(Box2i[][]), (c,o) => { var v = (Box2i[][])o; c.CodeBox2iArrayArray(ref v); } },
            { typeof(Box2i[][][]), (c,o) => { var v = (Box2i[][][])o; c.CodeBox2iArrayArrayArray(ref v); } },
            { typeof(Box2l[][]), (c,o) => { var v = (Box2l[][])o; c.CodeBox2lArrayArray(ref v); } },
            { typeof(Box2l[][][]), (c,o) => { var v = (Box2l[][][])o; c.CodeBox2lArrayArrayArray(ref v); } },
            { typeof(Box2f[][]), (c,o) => { var v = (Box2f[][])o; c.CodeBox2fArrayArray(ref v); } },
            { typeof(Box2f[][][]), (c,o) => { var v = (Box2f[][][])o; c.CodeBox2fArrayArrayArray(ref v); } },
            { typeof(Box2d[][]), (c,o) => { var v = (Box2d[][])o; c.CodeBox2dArrayArray(ref v); } },
            { typeof(Box2d[][][]), (c,o) => { var v = (Box2d[][][])o; c.CodeBox2dArrayArrayArray(ref v); } },
            { typeof(Box3i[][]), (c,o) => { var v = (Box3i[][])o; c.CodeBox3iArrayArray(ref v); } },
            { typeof(Box3i[][][]), (c,o) => { var v = (Box3i[][][])o; c.CodeBox3iArrayArrayArray(ref v); } },
            { typeof(Box3l[][]), (c,o) => { var v = (Box3l[][])o; c.CodeBox3lArrayArray(ref v); } },
            { typeof(Box3l[][][]), (c,o) => { var v = (Box3l[][][])o; c.CodeBox3lArrayArrayArray(ref v); } },
            { typeof(Box3f[][]), (c,o) => { var v = (Box3f[][])o; c.CodeBox3fArrayArray(ref v); } },
            { typeof(Box3f[][][]), (c,o) => { var v = (Box3f[][][])o; c.CodeBox3fArrayArrayArray(ref v); } },
            { typeof(Box3d[][]), (c,o) => { var v = (Box3d[][])o; c.CodeBox3dArrayArray(ref v); } },
            { typeof(Box3d[][][]), (c,o) => { var v = (Box3d[][][])o; c.CodeBox3dArrayArrayArray(ref v); } },
            { typeof(Euclidean3f[][]), (c,o) => { var v = (Euclidean3f[][])o; c.CodeEuclidean3fArrayArray(ref v); } },
            { typeof(Euclidean3f[][][]), (c,o) => { var v = (Euclidean3f[][][])o; c.CodeEuclidean3fArrayArrayArray(ref v); } },
            { typeof(Euclidean3d[][]), (c,o) => { var v = (Euclidean3d[][])o; c.CodeEuclidean3dArrayArray(ref v); } },
            { typeof(Euclidean3d[][][]), (c,o) => { var v = (Euclidean3d[][][])o; c.CodeEuclidean3dArrayArrayArray(ref v); } },
            { typeof(Rot2f[][]), (c,o) => { var v = (Rot2f[][])o; c.CodeRot2fArrayArray(ref v); } },
            { typeof(Rot2f[][][]), (c,o) => { var v = (Rot2f[][][])o; c.CodeRot2fArrayArrayArray(ref v); } },
            { typeof(Rot2d[][]), (c,o) => { var v = (Rot2d[][])o; c.CodeRot2dArrayArray(ref v); } },
            { typeof(Rot2d[][][]), (c,o) => { var v = (Rot2d[][][])o; c.CodeRot2dArrayArrayArray(ref v); } },
            { typeof(Rot3f[][]), (c,o) => { var v = (Rot3f[][])o; c.CodeRot3fArrayArray(ref v); } },
            { typeof(Rot3f[][][]), (c,o) => { var v = (Rot3f[][][])o; c.CodeRot3fArrayArrayArray(ref v); } },
            { typeof(Rot3d[][]), (c,o) => { var v = (Rot3d[][])o; c.CodeRot3dArrayArray(ref v); } },
            { typeof(Rot3d[][][]), (c,o) => { var v = (Rot3d[][][])o; c.CodeRot3dArrayArrayArray(ref v); } },
            { typeof(Scale3f[][]), (c,o) => { var v = (Scale3f[][])o; c.CodeScale3fArrayArray(ref v); } },
            { typeof(Scale3f[][][]), (c,o) => { var v = (Scale3f[][][])o; c.CodeScale3fArrayArrayArray(ref v); } },
            { typeof(Scale3d[][]), (c,o) => { var v = (Scale3d[][])o; c.CodeScale3dArrayArray(ref v); } },
            { typeof(Scale3d[][][]), (c,o) => { var v = (Scale3d[][][])o; c.CodeScale3dArrayArrayArray(ref v); } },
            { typeof(Shift3f[][]), (c,o) => { var v = (Shift3f[][])o; c.CodeShift3fArrayArray(ref v); } },
            { typeof(Shift3f[][][]), (c,o) => { var v = (Shift3f[][][])o; c.CodeShift3fArrayArrayArray(ref v); } },
            { typeof(Shift3d[][]), (c,o) => { var v = (Shift3d[][])o; c.CodeShift3dArrayArray(ref v); } },
            { typeof(Shift3d[][][]), (c,o) => { var v = (Shift3d[][][])o; c.CodeShift3dArrayArrayArray(ref v); } },
            { typeof(Trafo2f[][]), (c,o) => { var v = (Trafo2f[][])o; c.CodeTrafo2fArrayArray(ref v); } },
            { typeof(Trafo2f[][][]), (c,o) => { var v = (Trafo2f[][][])o; c.CodeTrafo2fArrayArrayArray(ref v); } },
            { typeof(Trafo2d[][]), (c,o) => { var v = (Trafo2d[][])o; c.CodeTrafo2dArrayArray(ref v); } },
            { typeof(Trafo2d[][][]), (c,o) => { var v = (Trafo2d[][][])o; c.CodeTrafo2dArrayArrayArray(ref v); } },
            { typeof(Trafo3f[][]), (c,o) => { var v = (Trafo3f[][])o; c.CodeTrafo3fArrayArray(ref v); } },
            { typeof(Trafo3f[][][]), (c,o) => { var v = (Trafo3f[][][])o; c.CodeTrafo3fArrayArrayArray(ref v); } },
            { typeof(Trafo3d[][]), (c,o) => { var v = (Trafo3d[][])o; c.CodeTrafo3dArrayArray(ref v); } },
            { typeof(Trafo3d[][][]), (c,o) => { var v = (Trafo3d[][][])o; c.CodeTrafo3dArrayArrayArray(ref v); } },

            #endregion

            #region Other Types

            { typeof(IntSet), (c,o) => { var v = (IntSet)o; c.CodeIntSet(ref v); } },
            { typeof(SymbolSet), (c,o) => { var v = (SymbolSet)o; c.CodeSymbolSet(ref v); } },


            { typeof(HashSet<string>), (c, o) => { var v = (HashSet<string>)o; c.CodeHashSet_of_T_(ref v); } },

            #endregion
        };

        internal static Dictionary<Type, Func<IReadingCoder, object>> TypeReaderMap =
                                new Dictionary<Type, Func<IReadingCoder, object>>
        {
            #region byte

            { typeof(byte), c => { var v = default(byte); c.CodeByte(ref v); return v; } },
            { typeof(byte[]), c => { var v = default(byte[]); c.CodeByteArray(ref v); return v; } },
            { typeof(List<byte>), c => { var v = default(List<byte>); c.CodeList_of_Byte_(ref v); return v; } },

            { typeof(Vector<byte>), c => { var v = default(Vector<byte>); c.CodeVector_of_Byte_(ref v); return v; } },
            { typeof(Vector<byte>[]), c => { var v = default(Vector<byte>[]); c.CodeVector_of_Byte_Array(ref v); return v; } },
            { typeof(List<Vector<byte>>), c => { var v = default(List<Vector<byte>>); c.CodeList_of_Vector_of_Byte__(ref v); return v; } },

            { typeof(Matrix<byte>), c => { var v = default(Matrix<byte>); c.CodeMatrix_of_Byte_(ref v); return v; } },
            { typeof(Matrix<byte>[]), c => { var v = default(Matrix<byte>[]); c.CodeMatrix_of_Byte_Array(ref v); return v; } },
            { typeof(List<Matrix<byte>>), c => { var v = default(List<Matrix<byte>>); c.CodeList_of_Matrix_of_Byte__(ref v); return v; } },

            { typeof(Volume<byte>), c => { var v = default(Volume<byte>); c.CodeVolume_of_Byte_(ref v); return v; } },
            { typeof(Volume<byte>[]), c => { var v = default(Volume<byte>[]); c.CodeVolume_of_Byte_Array(ref v); return v; } },
            { typeof(List<Volume<byte>>), c => { var v = default(List<Volume<byte>>); c.CodeList_of_Volume_of_Byte__(ref v); return v; } },

            { typeof(Tensor<byte>), c => { var v = default(Tensor<byte>); c.CodeTensor_of_Byte_(ref v); return v; } },
            { typeof(Tensor<byte>[]), c => { var v = default(Tensor<byte>[]); c.CodeTensor_of_Byte_Array(ref v); return v; } },
            { typeof(List<Tensor<byte>>), c => { var v = default(List<Tensor<byte>>); c.CodeList_of_Tensor_of_Byte__(ref v); return v; } },

            #endregion

            #region sbyte

            { typeof(sbyte), c => { var v = default(sbyte); c.CodeSByte(ref v); return v; } },
            { typeof(sbyte[]), c => { var v = default(sbyte[]); c.CodeSByteArray(ref v); return v; } },
            { typeof(List<sbyte>), c => { var v = default(List<sbyte>); c.CodeList_of_SByte_(ref v); return v; } },

            { typeof(Vector<sbyte>), c => { var v = default(Vector<sbyte>); c.CodeVector_of_SByte_(ref v); return v; } },
            { typeof(Vector<sbyte>[]), c => { var v = default(Vector<sbyte>[]); c.CodeVector_of_SByte_Array(ref v); return v; } },
            { typeof(List<Vector<sbyte>>), c => { var v = default(List<Vector<sbyte>>); c.CodeList_of_Vector_of_SByte__(ref v); return v; } },

            { typeof(Matrix<sbyte>), c => { var v = default(Matrix<sbyte>); c.CodeMatrix_of_SByte_(ref v); return v; } },
            { typeof(Matrix<sbyte>[]), c => { var v = default(Matrix<sbyte>[]); c.CodeMatrix_of_SByte_Array(ref v); return v; } },
            { typeof(List<Matrix<sbyte>>), c => { var v = default(List<Matrix<sbyte>>); c.CodeList_of_Matrix_of_SByte__(ref v); return v; } },

            { typeof(Volume<sbyte>), c => { var v = default(Volume<sbyte>); c.CodeVolume_of_SByte_(ref v); return v; } },
            { typeof(Volume<sbyte>[]), c => { var v = default(Volume<sbyte>[]); c.CodeVolume_of_SByte_Array(ref v); return v; } },
            { typeof(List<Volume<sbyte>>), c => { var v = default(List<Volume<sbyte>>); c.CodeList_of_Volume_of_SByte__(ref v); return v; } },

            { typeof(Tensor<sbyte>), c => { var v = default(Tensor<sbyte>); c.CodeTensor_of_SByte_(ref v); return v; } },
            { typeof(Tensor<sbyte>[]), c => { var v = default(Tensor<sbyte>[]); c.CodeTensor_of_SByte_Array(ref v); return v; } },
            { typeof(List<Tensor<sbyte>>), c => { var v = default(List<Tensor<sbyte>>); c.CodeList_of_Tensor_of_SByte__(ref v); return v; } },

            #endregion

            #region short

            { typeof(short), c => { var v = default(short); c.CodeShort(ref v); return v; } },
            { typeof(short[]), c => { var v = default(short[]); c.CodeShortArray(ref v); return v; } },
            { typeof(List<short>), c => { var v = default(List<short>); c.CodeList_of_Short_(ref v); return v; } },

            { typeof(Vector<short>), c => { var v = default(Vector<short>); c.CodeVector_of_Short_(ref v); return v; } },
            { typeof(Vector<short>[]), c => { var v = default(Vector<short>[]); c.CodeVector_of_Short_Array(ref v); return v; } },
            { typeof(List<Vector<short>>), c => { var v = default(List<Vector<short>>); c.CodeList_of_Vector_of_Short__(ref v); return v; } },

            { typeof(Matrix<short>), c => { var v = default(Matrix<short>); c.CodeMatrix_of_Short_(ref v); return v; } },
            { typeof(Matrix<short>[]), c => { var v = default(Matrix<short>[]); c.CodeMatrix_of_Short_Array(ref v); return v; } },
            { typeof(List<Matrix<short>>), c => { var v = default(List<Matrix<short>>); c.CodeList_of_Matrix_of_Short__(ref v); return v; } },

            { typeof(Volume<short>), c => { var v = default(Volume<short>); c.CodeVolume_of_Short_(ref v); return v; } },
            { typeof(Volume<short>[]), c => { var v = default(Volume<short>[]); c.CodeVolume_of_Short_Array(ref v); return v; } },
            { typeof(List<Volume<short>>), c => { var v = default(List<Volume<short>>); c.CodeList_of_Volume_of_Short__(ref v); return v; } },

            { typeof(Tensor<short>), c => { var v = default(Tensor<short>); c.CodeTensor_of_Short_(ref v); return v; } },
            { typeof(Tensor<short>[]), c => { var v = default(Tensor<short>[]); c.CodeTensor_of_Short_Array(ref v); return v; } },
            { typeof(List<Tensor<short>>), c => { var v = default(List<Tensor<short>>); c.CodeList_of_Tensor_of_Short__(ref v); return v; } },

            #endregion

            #region ushort

            { typeof(ushort), c => { var v = default(ushort); c.CodeUShort(ref v); return v; } },
            { typeof(ushort[]), c => { var v = default(ushort[]); c.CodeUShortArray(ref v); return v; } },
            { typeof(List<ushort>), c => { var v = default(List<ushort>); c.CodeList_of_UShort_(ref v); return v; } },

            { typeof(Vector<ushort>), c => { var v = default(Vector<ushort>); c.CodeVector_of_UShort_(ref v); return v; } },
            { typeof(Vector<ushort>[]), c => { var v = default(Vector<ushort>[]); c.CodeVector_of_UShort_Array(ref v); return v; } },
            { typeof(List<Vector<ushort>>), c => { var v = default(List<Vector<ushort>>); c.CodeList_of_Vector_of_UShort__(ref v); return v; } },

            { typeof(Matrix<ushort>), c => { var v = default(Matrix<ushort>); c.CodeMatrix_of_UShort_(ref v); return v; } },
            { typeof(Matrix<ushort>[]), c => { var v = default(Matrix<ushort>[]); c.CodeMatrix_of_UShort_Array(ref v); return v; } },
            { typeof(List<Matrix<ushort>>), c => { var v = default(List<Matrix<ushort>>); c.CodeList_of_Matrix_of_UShort__(ref v); return v; } },

            { typeof(Volume<ushort>), c => { var v = default(Volume<ushort>); c.CodeVolume_of_UShort_(ref v); return v; } },
            { typeof(Volume<ushort>[]), c => { var v = default(Volume<ushort>[]); c.CodeVolume_of_UShort_Array(ref v); return v; } },
            { typeof(List<Volume<ushort>>), c => { var v = default(List<Volume<ushort>>); c.CodeList_of_Volume_of_UShort__(ref v); return v; } },

            { typeof(Tensor<ushort>), c => { var v = default(Tensor<ushort>); c.CodeTensor_of_UShort_(ref v); return v; } },
            { typeof(Tensor<ushort>[]), c => { var v = default(Tensor<ushort>[]); c.CodeTensor_of_UShort_Array(ref v); return v; } },
            { typeof(List<Tensor<ushort>>), c => { var v = default(List<Tensor<ushort>>); c.CodeList_of_Tensor_of_UShort__(ref v); return v; } },

            #endregion

            #region int

            { typeof(int), c => { var v = default(int); c.CodeInt(ref v); return v; } },
            { typeof(int[]), c => { var v = default(int[]); c.CodeIntArray(ref v); return v; } },
            { typeof(List<int>), c => { var v = default(List<int>); c.CodeList_of_Int_(ref v); return v; } },

            { typeof(Vector<int>), c => { var v = default(Vector<int>); c.CodeVector_of_Int_(ref v); return v; } },
            { typeof(Vector<int>[]), c => { var v = default(Vector<int>[]); c.CodeVector_of_Int_Array(ref v); return v; } },
            { typeof(List<Vector<int>>), c => { var v = default(List<Vector<int>>); c.CodeList_of_Vector_of_Int__(ref v); return v; } },

            { typeof(Matrix<int>), c => { var v = default(Matrix<int>); c.CodeMatrix_of_Int_(ref v); return v; } },
            { typeof(Matrix<int>[]), c => { var v = default(Matrix<int>[]); c.CodeMatrix_of_Int_Array(ref v); return v; } },
            { typeof(List<Matrix<int>>), c => { var v = default(List<Matrix<int>>); c.CodeList_of_Matrix_of_Int__(ref v); return v; } },

            { typeof(Volume<int>), c => { var v = default(Volume<int>); c.CodeVolume_of_Int_(ref v); return v; } },
            { typeof(Volume<int>[]), c => { var v = default(Volume<int>[]); c.CodeVolume_of_Int_Array(ref v); return v; } },
            { typeof(List<Volume<int>>), c => { var v = default(List<Volume<int>>); c.CodeList_of_Volume_of_Int__(ref v); return v; } },

            { typeof(Tensor<int>), c => { var v = default(Tensor<int>); c.CodeTensor_of_Int_(ref v); return v; } },
            { typeof(Tensor<int>[]), c => { var v = default(Tensor<int>[]); c.CodeTensor_of_Int_Array(ref v); return v; } },
            { typeof(List<Tensor<int>>), c => { var v = default(List<Tensor<int>>); c.CodeList_of_Tensor_of_Int__(ref v); return v; } },

            #endregion

            #region uint

            { typeof(uint), c => { var v = default(uint); c.CodeUInt(ref v); return v; } },
            { typeof(uint[]), c => { var v = default(uint[]); c.CodeUIntArray(ref v); return v; } },
            { typeof(List<uint>), c => { var v = default(List<uint>); c.CodeList_of_UInt_(ref v); return v; } },

            { typeof(Vector<uint>), c => { var v = default(Vector<uint>); c.CodeVector_of_UInt_(ref v); return v; } },
            { typeof(Vector<uint>[]), c => { var v = default(Vector<uint>[]); c.CodeVector_of_UInt_Array(ref v); return v; } },
            { typeof(List<Vector<uint>>), c => { var v = default(List<Vector<uint>>); c.CodeList_of_Vector_of_UInt__(ref v); return v; } },

            { typeof(Matrix<uint>), c => { var v = default(Matrix<uint>); c.CodeMatrix_of_UInt_(ref v); return v; } },
            { typeof(Matrix<uint>[]), c => { var v = default(Matrix<uint>[]); c.CodeMatrix_of_UInt_Array(ref v); return v; } },
            { typeof(List<Matrix<uint>>), c => { var v = default(List<Matrix<uint>>); c.CodeList_of_Matrix_of_UInt__(ref v); return v; } },

            { typeof(Volume<uint>), c => { var v = default(Volume<uint>); c.CodeVolume_of_UInt_(ref v); return v; } },
            { typeof(Volume<uint>[]), c => { var v = default(Volume<uint>[]); c.CodeVolume_of_UInt_Array(ref v); return v; } },
            { typeof(List<Volume<uint>>), c => { var v = default(List<Volume<uint>>); c.CodeList_of_Volume_of_UInt__(ref v); return v; } },

            { typeof(Tensor<uint>), c => { var v = default(Tensor<uint>); c.CodeTensor_of_UInt_(ref v); return v; } },
            { typeof(Tensor<uint>[]), c => { var v = default(Tensor<uint>[]); c.CodeTensor_of_UInt_Array(ref v); return v; } },
            { typeof(List<Tensor<uint>>), c => { var v = default(List<Tensor<uint>>); c.CodeList_of_Tensor_of_UInt__(ref v); return v; } },

            #endregion

            #region long

            { typeof(long), c => { var v = default(long); c.CodeLong(ref v); return v; } },
            { typeof(long[]), c => { var v = default(long[]); c.CodeLongArray(ref v); return v; } },
            { typeof(List<long>), c => { var v = default(List<long>); c.CodeList_of_Long_(ref v); return v; } },

            { typeof(Vector<long>), c => { var v = default(Vector<long>); c.CodeVector_of_Long_(ref v); return v; } },
            { typeof(Vector<long>[]), c => { var v = default(Vector<long>[]); c.CodeVector_of_Long_Array(ref v); return v; } },
            { typeof(List<Vector<long>>), c => { var v = default(List<Vector<long>>); c.CodeList_of_Vector_of_Long__(ref v); return v; } },

            { typeof(Matrix<long>), c => { var v = default(Matrix<long>); c.CodeMatrix_of_Long_(ref v); return v; } },
            { typeof(Matrix<long>[]), c => { var v = default(Matrix<long>[]); c.CodeMatrix_of_Long_Array(ref v); return v; } },
            { typeof(List<Matrix<long>>), c => { var v = default(List<Matrix<long>>); c.CodeList_of_Matrix_of_Long__(ref v); return v; } },

            { typeof(Volume<long>), c => { var v = default(Volume<long>); c.CodeVolume_of_Long_(ref v); return v; } },
            { typeof(Volume<long>[]), c => { var v = default(Volume<long>[]); c.CodeVolume_of_Long_Array(ref v); return v; } },
            { typeof(List<Volume<long>>), c => { var v = default(List<Volume<long>>); c.CodeList_of_Volume_of_Long__(ref v); return v; } },

            { typeof(Tensor<long>), c => { var v = default(Tensor<long>); c.CodeTensor_of_Long_(ref v); return v; } },
            { typeof(Tensor<long>[]), c => { var v = default(Tensor<long>[]); c.CodeTensor_of_Long_Array(ref v); return v; } },
            { typeof(List<Tensor<long>>), c => { var v = default(List<Tensor<long>>); c.CodeList_of_Tensor_of_Long__(ref v); return v; } },

            #endregion

            #region ulong

            { typeof(ulong), c => { var v = default(ulong); c.CodeULong(ref v); return v; } },
            { typeof(ulong[]), c => { var v = default(ulong[]); c.CodeULongArray(ref v); return v; } },
            { typeof(List<ulong>), c => { var v = default(List<ulong>); c.CodeList_of_ULong_(ref v); return v; } },

            { typeof(Vector<ulong>), c => { var v = default(Vector<ulong>); c.CodeVector_of_ULong_(ref v); return v; } },
            { typeof(Vector<ulong>[]), c => { var v = default(Vector<ulong>[]); c.CodeVector_of_ULong_Array(ref v); return v; } },
            { typeof(List<Vector<ulong>>), c => { var v = default(List<Vector<ulong>>); c.CodeList_of_Vector_of_ULong__(ref v); return v; } },

            { typeof(Matrix<ulong>), c => { var v = default(Matrix<ulong>); c.CodeMatrix_of_ULong_(ref v); return v; } },
            { typeof(Matrix<ulong>[]), c => { var v = default(Matrix<ulong>[]); c.CodeMatrix_of_ULong_Array(ref v); return v; } },
            { typeof(List<Matrix<ulong>>), c => { var v = default(List<Matrix<ulong>>); c.CodeList_of_Matrix_of_ULong__(ref v); return v; } },

            { typeof(Volume<ulong>), c => { var v = default(Volume<ulong>); c.CodeVolume_of_ULong_(ref v); return v; } },
            { typeof(Volume<ulong>[]), c => { var v = default(Volume<ulong>[]); c.CodeVolume_of_ULong_Array(ref v); return v; } },
            { typeof(List<Volume<ulong>>), c => { var v = default(List<Volume<ulong>>); c.CodeList_of_Volume_of_ULong__(ref v); return v; } },

            { typeof(Tensor<ulong>), c => { var v = default(Tensor<ulong>); c.CodeTensor_of_ULong_(ref v); return v; } },
            { typeof(Tensor<ulong>[]), c => { var v = default(Tensor<ulong>[]); c.CodeTensor_of_ULong_Array(ref v); return v; } },
            { typeof(List<Tensor<ulong>>), c => { var v = default(List<Tensor<ulong>>); c.CodeList_of_Tensor_of_ULong__(ref v); return v; } },

            #endregion

            #region float

            { typeof(float), c => { var v = default(float); c.CodeFloat(ref v); return v; } },
            { typeof(float[]), c => { var v = default(float[]); c.CodeFloatArray(ref v); return v; } },
            { typeof(List<float>), c => { var v = default(List<float>); c.CodeList_of_Float_(ref v); return v; } },

            { typeof(Vector<float>), c => { var v = default(Vector<float>); c.CodeVector_of_Float_(ref v); return v; } },
            { typeof(Vector<float>[]), c => { var v = default(Vector<float>[]); c.CodeVector_of_Float_Array(ref v); return v; } },
            { typeof(List<Vector<float>>), c => { var v = default(List<Vector<float>>); c.CodeList_of_Vector_of_Float__(ref v); return v; } },

            { typeof(Matrix<float>), c => { var v = default(Matrix<float>); c.CodeMatrix_of_Float_(ref v); return v; } },
            { typeof(Matrix<float>[]), c => { var v = default(Matrix<float>[]); c.CodeMatrix_of_Float_Array(ref v); return v; } },
            { typeof(List<Matrix<float>>), c => { var v = default(List<Matrix<float>>); c.CodeList_of_Matrix_of_Float__(ref v); return v; } },

            { typeof(Volume<float>), c => { var v = default(Volume<float>); c.CodeVolume_of_Float_(ref v); return v; } },
            { typeof(Volume<float>[]), c => { var v = default(Volume<float>[]); c.CodeVolume_of_Float_Array(ref v); return v; } },
            { typeof(List<Volume<float>>), c => { var v = default(List<Volume<float>>); c.CodeList_of_Volume_of_Float__(ref v); return v; } },

            { typeof(Tensor<float>), c => { var v = default(Tensor<float>); c.CodeTensor_of_Float_(ref v); return v; } },
            { typeof(Tensor<float>[]), c => { var v = default(Tensor<float>[]); c.CodeTensor_of_Float_Array(ref v); return v; } },
            { typeof(List<Tensor<float>>), c => { var v = default(List<Tensor<float>>); c.CodeList_of_Tensor_of_Float__(ref v); return v; } },

            #endregion

            #region double

            { typeof(double), c => { var v = default(double); c.CodeDouble(ref v); return v; } },
            { typeof(double[]), c => { var v = default(double[]); c.CodeDoubleArray(ref v); return v; } },
            { typeof(List<double>), c => { var v = default(List<double>); c.CodeList_of_Double_(ref v); return v; } },

            { typeof(Vector<double>), c => { var v = default(Vector<double>); c.CodeVector_of_Double_(ref v); return v; } },
            { typeof(Vector<double>[]), c => { var v = default(Vector<double>[]); c.CodeVector_of_Double_Array(ref v); return v; } },
            { typeof(List<Vector<double>>), c => { var v = default(List<Vector<double>>); c.CodeList_of_Vector_of_Double__(ref v); return v; } },

            { typeof(Matrix<double>), c => { var v = default(Matrix<double>); c.CodeMatrix_of_Double_(ref v); return v; } },
            { typeof(Matrix<double>[]), c => { var v = default(Matrix<double>[]); c.CodeMatrix_of_Double_Array(ref v); return v; } },
            { typeof(List<Matrix<double>>), c => { var v = default(List<Matrix<double>>); c.CodeList_of_Matrix_of_Double__(ref v); return v; } },

            { typeof(Volume<double>), c => { var v = default(Volume<double>); c.CodeVolume_of_Double_(ref v); return v; } },
            { typeof(Volume<double>[]), c => { var v = default(Volume<double>[]); c.CodeVolume_of_Double_Array(ref v); return v; } },
            { typeof(List<Volume<double>>), c => { var v = default(List<Volume<double>>); c.CodeList_of_Volume_of_Double__(ref v); return v; } },

            { typeof(Tensor<double>), c => { var v = default(Tensor<double>); c.CodeTensor_of_Double_(ref v); return v; } },
            { typeof(Tensor<double>[]), c => { var v = default(Tensor<double>[]); c.CodeTensor_of_Double_Array(ref v); return v; } },
            { typeof(List<Tensor<double>>), c => { var v = default(List<Tensor<double>>); c.CodeList_of_Tensor_of_Double__(ref v); return v; } },

            #endregion

            #region Fraction

            { typeof(Fraction), c => { var v = default(Fraction); c.CodeFraction(ref v); return v; } },
            { typeof(Fraction[]), c => { var v = default(Fraction[]); c.CodeFractionArray(ref v); return v; } },
            { typeof(List<Fraction>), c => { var v = default(List<Fraction>); c.CodeList_of_Fraction_(ref v); return v; } },

            { typeof(Vector<Fraction>), c => { var v = default(Vector<Fraction>); c.CodeVector_of_Fraction_(ref v); return v; } },
            { typeof(Vector<Fraction>[]), c => { var v = default(Vector<Fraction>[]); c.CodeVector_of_Fraction_Array(ref v); return v; } },
            { typeof(List<Vector<Fraction>>), c => { var v = default(List<Vector<Fraction>>); c.CodeList_of_Vector_of_Fraction__(ref v); return v; } },

            { typeof(Matrix<Fraction>), c => { var v = default(Matrix<Fraction>); c.CodeMatrix_of_Fraction_(ref v); return v; } },
            { typeof(Matrix<Fraction>[]), c => { var v = default(Matrix<Fraction>[]); c.CodeMatrix_of_Fraction_Array(ref v); return v; } },
            { typeof(List<Matrix<Fraction>>), c => { var v = default(List<Matrix<Fraction>>); c.CodeList_of_Matrix_of_Fraction__(ref v); return v; } },

            { typeof(Volume<Fraction>), c => { var v = default(Volume<Fraction>); c.CodeVolume_of_Fraction_(ref v); return v; } },
            { typeof(Volume<Fraction>[]), c => { var v = default(Volume<Fraction>[]); c.CodeVolume_of_Fraction_Array(ref v); return v; } },
            { typeof(List<Volume<Fraction>>), c => { var v = default(List<Volume<Fraction>>); c.CodeList_of_Volume_of_Fraction__(ref v); return v; } },

            { typeof(Tensor<Fraction>), c => { var v = default(Tensor<Fraction>); c.CodeTensor_of_Fraction_(ref v); return v; } },
            { typeof(Tensor<Fraction>[]), c => { var v = default(Tensor<Fraction>[]); c.CodeTensor_of_Fraction_Array(ref v); return v; } },
            { typeof(List<Tensor<Fraction>>), c => { var v = default(List<Tensor<Fraction>>); c.CodeList_of_Tensor_of_Fraction__(ref v); return v; } },

            #endregion

            #region V2i

            { typeof(V2i), c => { var v = default(V2i); c.CodeV2i(ref v); return v; } },
            { typeof(V2i[]), c => { var v = default(V2i[]); c.CodeV2iArray(ref v); return v; } },
            { typeof(List<V2i>), c => { var v = default(List<V2i>); c.CodeList_of_V2i_(ref v); return v; } },

            { typeof(Vector<V2i>), c => { var v = default(Vector<V2i>); c.CodeVector_of_V2i_(ref v); return v; } },
            { typeof(Vector<V2i>[]), c => { var v = default(Vector<V2i>[]); c.CodeVector_of_V2i_Array(ref v); return v; } },
            { typeof(List<Vector<V2i>>), c => { var v = default(List<Vector<V2i>>); c.CodeList_of_Vector_of_V2i__(ref v); return v; } },

            { typeof(Matrix<V2i>), c => { var v = default(Matrix<V2i>); c.CodeMatrix_of_V2i_(ref v); return v; } },
            { typeof(Matrix<V2i>[]), c => { var v = default(Matrix<V2i>[]); c.CodeMatrix_of_V2i_Array(ref v); return v; } },
            { typeof(List<Matrix<V2i>>), c => { var v = default(List<Matrix<V2i>>); c.CodeList_of_Matrix_of_V2i__(ref v); return v; } },

            { typeof(Volume<V2i>), c => { var v = default(Volume<V2i>); c.CodeVolume_of_V2i_(ref v); return v; } },
            { typeof(Volume<V2i>[]), c => { var v = default(Volume<V2i>[]); c.CodeVolume_of_V2i_Array(ref v); return v; } },
            { typeof(List<Volume<V2i>>), c => { var v = default(List<Volume<V2i>>); c.CodeList_of_Volume_of_V2i__(ref v); return v; } },

            { typeof(Tensor<V2i>), c => { var v = default(Tensor<V2i>); c.CodeTensor_of_V2i_(ref v); return v; } },
            { typeof(Tensor<V2i>[]), c => { var v = default(Tensor<V2i>[]); c.CodeTensor_of_V2i_Array(ref v); return v; } },
            { typeof(List<Tensor<V2i>>), c => { var v = default(List<Tensor<V2i>>); c.CodeList_of_Tensor_of_V2i__(ref v); return v; } },

            #endregion

            #region V2l

            { typeof(V2l), c => { var v = default(V2l); c.CodeV2l(ref v); return v; } },
            { typeof(V2l[]), c => { var v = default(V2l[]); c.CodeV2lArray(ref v); return v; } },
            { typeof(List<V2l>), c => { var v = default(List<V2l>); c.CodeList_of_V2l_(ref v); return v; } },

            { typeof(Vector<V2l>), c => { var v = default(Vector<V2l>); c.CodeVector_of_V2l_(ref v); return v; } },
            { typeof(Vector<V2l>[]), c => { var v = default(Vector<V2l>[]); c.CodeVector_of_V2l_Array(ref v); return v; } },
            { typeof(List<Vector<V2l>>), c => { var v = default(List<Vector<V2l>>); c.CodeList_of_Vector_of_V2l__(ref v); return v; } },

            { typeof(Matrix<V2l>), c => { var v = default(Matrix<V2l>); c.CodeMatrix_of_V2l_(ref v); return v; } },
            { typeof(Matrix<V2l>[]), c => { var v = default(Matrix<V2l>[]); c.CodeMatrix_of_V2l_Array(ref v); return v; } },
            { typeof(List<Matrix<V2l>>), c => { var v = default(List<Matrix<V2l>>); c.CodeList_of_Matrix_of_V2l__(ref v); return v; } },

            { typeof(Volume<V2l>), c => { var v = default(Volume<V2l>); c.CodeVolume_of_V2l_(ref v); return v; } },
            { typeof(Volume<V2l>[]), c => { var v = default(Volume<V2l>[]); c.CodeVolume_of_V2l_Array(ref v); return v; } },
            { typeof(List<Volume<V2l>>), c => { var v = default(List<Volume<V2l>>); c.CodeList_of_Volume_of_V2l__(ref v); return v; } },

            { typeof(Tensor<V2l>), c => { var v = default(Tensor<V2l>); c.CodeTensor_of_V2l_(ref v); return v; } },
            { typeof(Tensor<V2l>[]), c => { var v = default(Tensor<V2l>[]); c.CodeTensor_of_V2l_Array(ref v); return v; } },
            { typeof(List<Tensor<V2l>>), c => { var v = default(List<Tensor<V2l>>); c.CodeList_of_Tensor_of_V2l__(ref v); return v; } },

            #endregion

            #region V2f

            { typeof(V2f), c => { var v = default(V2f); c.CodeV2f(ref v); return v; } },
            { typeof(V2f[]), c => { var v = default(V2f[]); c.CodeV2fArray(ref v); return v; } },
            { typeof(List<V2f>), c => { var v = default(List<V2f>); c.CodeList_of_V2f_(ref v); return v; } },

            { typeof(Vector<V2f>), c => { var v = default(Vector<V2f>); c.CodeVector_of_V2f_(ref v); return v; } },
            { typeof(Vector<V2f>[]), c => { var v = default(Vector<V2f>[]); c.CodeVector_of_V2f_Array(ref v); return v; } },
            { typeof(List<Vector<V2f>>), c => { var v = default(List<Vector<V2f>>); c.CodeList_of_Vector_of_V2f__(ref v); return v; } },

            { typeof(Matrix<V2f>), c => { var v = default(Matrix<V2f>); c.CodeMatrix_of_V2f_(ref v); return v; } },
            { typeof(Matrix<V2f>[]), c => { var v = default(Matrix<V2f>[]); c.CodeMatrix_of_V2f_Array(ref v); return v; } },
            { typeof(List<Matrix<V2f>>), c => { var v = default(List<Matrix<V2f>>); c.CodeList_of_Matrix_of_V2f__(ref v); return v; } },

            { typeof(Volume<V2f>), c => { var v = default(Volume<V2f>); c.CodeVolume_of_V2f_(ref v); return v; } },
            { typeof(Volume<V2f>[]), c => { var v = default(Volume<V2f>[]); c.CodeVolume_of_V2f_Array(ref v); return v; } },
            { typeof(List<Volume<V2f>>), c => { var v = default(List<Volume<V2f>>); c.CodeList_of_Volume_of_V2f__(ref v); return v; } },

            { typeof(Tensor<V2f>), c => { var v = default(Tensor<V2f>); c.CodeTensor_of_V2f_(ref v); return v; } },
            { typeof(Tensor<V2f>[]), c => { var v = default(Tensor<V2f>[]); c.CodeTensor_of_V2f_Array(ref v); return v; } },
            { typeof(List<Tensor<V2f>>), c => { var v = default(List<Tensor<V2f>>); c.CodeList_of_Tensor_of_V2f__(ref v); return v; } },

            #endregion

            #region V2d

            { typeof(V2d), c => { var v = default(V2d); c.CodeV2d(ref v); return v; } },
            { typeof(V2d[]), c => { var v = default(V2d[]); c.CodeV2dArray(ref v); return v; } },
            { typeof(List<V2d>), c => { var v = default(List<V2d>); c.CodeList_of_V2d_(ref v); return v; } },

            { typeof(Vector<V2d>), c => { var v = default(Vector<V2d>); c.CodeVector_of_V2d_(ref v); return v; } },
            { typeof(Vector<V2d>[]), c => { var v = default(Vector<V2d>[]); c.CodeVector_of_V2d_Array(ref v); return v; } },
            { typeof(List<Vector<V2d>>), c => { var v = default(List<Vector<V2d>>); c.CodeList_of_Vector_of_V2d__(ref v); return v; } },

            { typeof(Matrix<V2d>), c => { var v = default(Matrix<V2d>); c.CodeMatrix_of_V2d_(ref v); return v; } },
            { typeof(Matrix<V2d>[]), c => { var v = default(Matrix<V2d>[]); c.CodeMatrix_of_V2d_Array(ref v); return v; } },
            { typeof(List<Matrix<V2d>>), c => { var v = default(List<Matrix<V2d>>); c.CodeList_of_Matrix_of_V2d__(ref v); return v; } },

            { typeof(Volume<V2d>), c => { var v = default(Volume<V2d>); c.CodeVolume_of_V2d_(ref v); return v; } },
            { typeof(Volume<V2d>[]), c => { var v = default(Volume<V2d>[]); c.CodeVolume_of_V2d_Array(ref v); return v; } },
            { typeof(List<Volume<V2d>>), c => { var v = default(List<Volume<V2d>>); c.CodeList_of_Volume_of_V2d__(ref v); return v; } },

            { typeof(Tensor<V2d>), c => { var v = default(Tensor<V2d>); c.CodeTensor_of_V2d_(ref v); return v; } },
            { typeof(Tensor<V2d>[]), c => { var v = default(Tensor<V2d>[]); c.CodeTensor_of_V2d_Array(ref v); return v; } },
            { typeof(List<Tensor<V2d>>), c => { var v = default(List<Tensor<V2d>>); c.CodeList_of_Tensor_of_V2d__(ref v); return v; } },

            #endregion

            #region V3i

            { typeof(V3i), c => { var v = default(V3i); c.CodeV3i(ref v); return v; } },
            { typeof(V3i[]), c => { var v = default(V3i[]); c.CodeV3iArray(ref v); return v; } },
            { typeof(List<V3i>), c => { var v = default(List<V3i>); c.CodeList_of_V3i_(ref v); return v; } },

            { typeof(Vector<V3i>), c => { var v = default(Vector<V3i>); c.CodeVector_of_V3i_(ref v); return v; } },
            { typeof(Vector<V3i>[]), c => { var v = default(Vector<V3i>[]); c.CodeVector_of_V3i_Array(ref v); return v; } },
            { typeof(List<Vector<V3i>>), c => { var v = default(List<Vector<V3i>>); c.CodeList_of_Vector_of_V3i__(ref v); return v; } },

            { typeof(Matrix<V3i>), c => { var v = default(Matrix<V3i>); c.CodeMatrix_of_V3i_(ref v); return v; } },
            { typeof(Matrix<V3i>[]), c => { var v = default(Matrix<V3i>[]); c.CodeMatrix_of_V3i_Array(ref v); return v; } },
            { typeof(List<Matrix<V3i>>), c => { var v = default(List<Matrix<V3i>>); c.CodeList_of_Matrix_of_V3i__(ref v); return v; } },

            { typeof(Volume<V3i>), c => { var v = default(Volume<V3i>); c.CodeVolume_of_V3i_(ref v); return v; } },
            { typeof(Volume<V3i>[]), c => { var v = default(Volume<V3i>[]); c.CodeVolume_of_V3i_Array(ref v); return v; } },
            { typeof(List<Volume<V3i>>), c => { var v = default(List<Volume<V3i>>); c.CodeList_of_Volume_of_V3i__(ref v); return v; } },

            { typeof(Tensor<V3i>), c => { var v = default(Tensor<V3i>); c.CodeTensor_of_V3i_(ref v); return v; } },
            { typeof(Tensor<V3i>[]), c => { var v = default(Tensor<V3i>[]); c.CodeTensor_of_V3i_Array(ref v); return v; } },
            { typeof(List<Tensor<V3i>>), c => { var v = default(List<Tensor<V3i>>); c.CodeList_of_Tensor_of_V3i__(ref v); return v; } },

            #endregion

            #region V3l

            { typeof(V3l), c => { var v = default(V3l); c.CodeV3l(ref v); return v; } },
            { typeof(V3l[]), c => { var v = default(V3l[]); c.CodeV3lArray(ref v); return v; } },
            { typeof(List<V3l>), c => { var v = default(List<V3l>); c.CodeList_of_V3l_(ref v); return v; } },

            { typeof(Vector<V3l>), c => { var v = default(Vector<V3l>); c.CodeVector_of_V3l_(ref v); return v; } },
            { typeof(Vector<V3l>[]), c => { var v = default(Vector<V3l>[]); c.CodeVector_of_V3l_Array(ref v); return v; } },
            { typeof(List<Vector<V3l>>), c => { var v = default(List<Vector<V3l>>); c.CodeList_of_Vector_of_V3l__(ref v); return v; } },

            { typeof(Matrix<V3l>), c => { var v = default(Matrix<V3l>); c.CodeMatrix_of_V3l_(ref v); return v; } },
            { typeof(Matrix<V3l>[]), c => { var v = default(Matrix<V3l>[]); c.CodeMatrix_of_V3l_Array(ref v); return v; } },
            { typeof(List<Matrix<V3l>>), c => { var v = default(List<Matrix<V3l>>); c.CodeList_of_Matrix_of_V3l__(ref v); return v; } },

            { typeof(Volume<V3l>), c => { var v = default(Volume<V3l>); c.CodeVolume_of_V3l_(ref v); return v; } },
            { typeof(Volume<V3l>[]), c => { var v = default(Volume<V3l>[]); c.CodeVolume_of_V3l_Array(ref v); return v; } },
            { typeof(List<Volume<V3l>>), c => { var v = default(List<Volume<V3l>>); c.CodeList_of_Volume_of_V3l__(ref v); return v; } },

            { typeof(Tensor<V3l>), c => { var v = default(Tensor<V3l>); c.CodeTensor_of_V3l_(ref v); return v; } },
            { typeof(Tensor<V3l>[]), c => { var v = default(Tensor<V3l>[]); c.CodeTensor_of_V3l_Array(ref v); return v; } },
            { typeof(List<Tensor<V3l>>), c => { var v = default(List<Tensor<V3l>>); c.CodeList_of_Tensor_of_V3l__(ref v); return v; } },

            #endregion

            #region V3f

            { typeof(V3f), c => { var v = default(V3f); c.CodeV3f(ref v); return v; } },
            { typeof(V3f[]), c => { var v = default(V3f[]); c.CodeV3fArray(ref v); return v; } },
            { typeof(List<V3f>), c => { var v = default(List<V3f>); c.CodeList_of_V3f_(ref v); return v; } },

            { typeof(Vector<V3f>), c => { var v = default(Vector<V3f>); c.CodeVector_of_V3f_(ref v); return v; } },
            { typeof(Vector<V3f>[]), c => { var v = default(Vector<V3f>[]); c.CodeVector_of_V3f_Array(ref v); return v; } },
            { typeof(List<Vector<V3f>>), c => { var v = default(List<Vector<V3f>>); c.CodeList_of_Vector_of_V3f__(ref v); return v; } },

            { typeof(Matrix<V3f>), c => { var v = default(Matrix<V3f>); c.CodeMatrix_of_V3f_(ref v); return v; } },
            { typeof(Matrix<V3f>[]), c => { var v = default(Matrix<V3f>[]); c.CodeMatrix_of_V3f_Array(ref v); return v; } },
            { typeof(List<Matrix<V3f>>), c => { var v = default(List<Matrix<V3f>>); c.CodeList_of_Matrix_of_V3f__(ref v); return v; } },

            { typeof(Volume<V3f>), c => { var v = default(Volume<V3f>); c.CodeVolume_of_V3f_(ref v); return v; } },
            { typeof(Volume<V3f>[]), c => { var v = default(Volume<V3f>[]); c.CodeVolume_of_V3f_Array(ref v); return v; } },
            { typeof(List<Volume<V3f>>), c => { var v = default(List<Volume<V3f>>); c.CodeList_of_Volume_of_V3f__(ref v); return v; } },

            { typeof(Tensor<V3f>), c => { var v = default(Tensor<V3f>); c.CodeTensor_of_V3f_(ref v); return v; } },
            { typeof(Tensor<V3f>[]), c => { var v = default(Tensor<V3f>[]); c.CodeTensor_of_V3f_Array(ref v); return v; } },
            { typeof(List<Tensor<V3f>>), c => { var v = default(List<Tensor<V3f>>); c.CodeList_of_Tensor_of_V3f__(ref v); return v; } },

            #endregion

            #region V3d

            { typeof(V3d), c => { var v = default(V3d); c.CodeV3d(ref v); return v; } },
            { typeof(V3d[]), c => { var v = default(V3d[]); c.CodeV3dArray(ref v); return v; } },
            { typeof(List<V3d>), c => { var v = default(List<V3d>); c.CodeList_of_V3d_(ref v); return v; } },

            { typeof(Vector<V3d>), c => { var v = default(Vector<V3d>); c.CodeVector_of_V3d_(ref v); return v; } },
            { typeof(Vector<V3d>[]), c => { var v = default(Vector<V3d>[]); c.CodeVector_of_V3d_Array(ref v); return v; } },
            { typeof(List<Vector<V3d>>), c => { var v = default(List<Vector<V3d>>); c.CodeList_of_Vector_of_V3d__(ref v); return v; } },

            { typeof(Matrix<V3d>), c => { var v = default(Matrix<V3d>); c.CodeMatrix_of_V3d_(ref v); return v; } },
            { typeof(Matrix<V3d>[]), c => { var v = default(Matrix<V3d>[]); c.CodeMatrix_of_V3d_Array(ref v); return v; } },
            { typeof(List<Matrix<V3d>>), c => { var v = default(List<Matrix<V3d>>); c.CodeList_of_Matrix_of_V3d__(ref v); return v; } },

            { typeof(Volume<V3d>), c => { var v = default(Volume<V3d>); c.CodeVolume_of_V3d_(ref v); return v; } },
            { typeof(Volume<V3d>[]), c => { var v = default(Volume<V3d>[]); c.CodeVolume_of_V3d_Array(ref v); return v; } },
            { typeof(List<Volume<V3d>>), c => { var v = default(List<Volume<V3d>>); c.CodeList_of_Volume_of_V3d__(ref v); return v; } },

            { typeof(Tensor<V3d>), c => { var v = default(Tensor<V3d>); c.CodeTensor_of_V3d_(ref v); return v; } },
            { typeof(Tensor<V3d>[]), c => { var v = default(Tensor<V3d>[]); c.CodeTensor_of_V3d_Array(ref v); return v; } },
            { typeof(List<Tensor<V3d>>), c => { var v = default(List<Tensor<V3d>>); c.CodeList_of_Tensor_of_V3d__(ref v); return v; } },

            #endregion

            #region V4i

            { typeof(V4i), c => { var v = default(V4i); c.CodeV4i(ref v); return v; } },
            { typeof(V4i[]), c => { var v = default(V4i[]); c.CodeV4iArray(ref v); return v; } },
            { typeof(List<V4i>), c => { var v = default(List<V4i>); c.CodeList_of_V4i_(ref v); return v; } },

            { typeof(Vector<V4i>), c => { var v = default(Vector<V4i>); c.CodeVector_of_V4i_(ref v); return v; } },
            { typeof(Vector<V4i>[]), c => { var v = default(Vector<V4i>[]); c.CodeVector_of_V4i_Array(ref v); return v; } },
            { typeof(List<Vector<V4i>>), c => { var v = default(List<Vector<V4i>>); c.CodeList_of_Vector_of_V4i__(ref v); return v; } },

            { typeof(Matrix<V4i>), c => { var v = default(Matrix<V4i>); c.CodeMatrix_of_V4i_(ref v); return v; } },
            { typeof(Matrix<V4i>[]), c => { var v = default(Matrix<V4i>[]); c.CodeMatrix_of_V4i_Array(ref v); return v; } },
            { typeof(List<Matrix<V4i>>), c => { var v = default(List<Matrix<V4i>>); c.CodeList_of_Matrix_of_V4i__(ref v); return v; } },

            { typeof(Volume<V4i>), c => { var v = default(Volume<V4i>); c.CodeVolume_of_V4i_(ref v); return v; } },
            { typeof(Volume<V4i>[]), c => { var v = default(Volume<V4i>[]); c.CodeVolume_of_V4i_Array(ref v); return v; } },
            { typeof(List<Volume<V4i>>), c => { var v = default(List<Volume<V4i>>); c.CodeList_of_Volume_of_V4i__(ref v); return v; } },

            { typeof(Tensor<V4i>), c => { var v = default(Tensor<V4i>); c.CodeTensor_of_V4i_(ref v); return v; } },
            { typeof(Tensor<V4i>[]), c => { var v = default(Tensor<V4i>[]); c.CodeTensor_of_V4i_Array(ref v); return v; } },
            { typeof(List<Tensor<V4i>>), c => { var v = default(List<Tensor<V4i>>); c.CodeList_of_Tensor_of_V4i__(ref v); return v; } },

            #endregion

            #region V4l

            { typeof(V4l), c => { var v = default(V4l); c.CodeV4l(ref v); return v; } },
            { typeof(V4l[]), c => { var v = default(V4l[]); c.CodeV4lArray(ref v); return v; } },
            { typeof(List<V4l>), c => { var v = default(List<V4l>); c.CodeList_of_V4l_(ref v); return v; } },

            { typeof(Vector<V4l>), c => { var v = default(Vector<V4l>); c.CodeVector_of_V4l_(ref v); return v; } },
            { typeof(Vector<V4l>[]), c => { var v = default(Vector<V4l>[]); c.CodeVector_of_V4l_Array(ref v); return v; } },
            { typeof(List<Vector<V4l>>), c => { var v = default(List<Vector<V4l>>); c.CodeList_of_Vector_of_V4l__(ref v); return v; } },

            { typeof(Matrix<V4l>), c => { var v = default(Matrix<V4l>); c.CodeMatrix_of_V4l_(ref v); return v; } },
            { typeof(Matrix<V4l>[]), c => { var v = default(Matrix<V4l>[]); c.CodeMatrix_of_V4l_Array(ref v); return v; } },
            { typeof(List<Matrix<V4l>>), c => { var v = default(List<Matrix<V4l>>); c.CodeList_of_Matrix_of_V4l__(ref v); return v; } },

            { typeof(Volume<V4l>), c => { var v = default(Volume<V4l>); c.CodeVolume_of_V4l_(ref v); return v; } },
            { typeof(Volume<V4l>[]), c => { var v = default(Volume<V4l>[]); c.CodeVolume_of_V4l_Array(ref v); return v; } },
            { typeof(List<Volume<V4l>>), c => { var v = default(List<Volume<V4l>>); c.CodeList_of_Volume_of_V4l__(ref v); return v; } },

            { typeof(Tensor<V4l>), c => { var v = default(Tensor<V4l>); c.CodeTensor_of_V4l_(ref v); return v; } },
            { typeof(Tensor<V4l>[]), c => { var v = default(Tensor<V4l>[]); c.CodeTensor_of_V4l_Array(ref v); return v; } },
            { typeof(List<Tensor<V4l>>), c => { var v = default(List<Tensor<V4l>>); c.CodeList_of_Tensor_of_V4l__(ref v); return v; } },

            #endregion

            #region V4f

            { typeof(V4f), c => { var v = default(V4f); c.CodeV4f(ref v); return v; } },
            { typeof(V4f[]), c => { var v = default(V4f[]); c.CodeV4fArray(ref v); return v; } },
            { typeof(List<V4f>), c => { var v = default(List<V4f>); c.CodeList_of_V4f_(ref v); return v; } },

            { typeof(Vector<V4f>), c => { var v = default(Vector<V4f>); c.CodeVector_of_V4f_(ref v); return v; } },
            { typeof(Vector<V4f>[]), c => { var v = default(Vector<V4f>[]); c.CodeVector_of_V4f_Array(ref v); return v; } },
            { typeof(List<Vector<V4f>>), c => { var v = default(List<Vector<V4f>>); c.CodeList_of_Vector_of_V4f__(ref v); return v; } },

            { typeof(Matrix<V4f>), c => { var v = default(Matrix<V4f>); c.CodeMatrix_of_V4f_(ref v); return v; } },
            { typeof(Matrix<V4f>[]), c => { var v = default(Matrix<V4f>[]); c.CodeMatrix_of_V4f_Array(ref v); return v; } },
            { typeof(List<Matrix<V4f>>), c => { var v = default(List<Matrix<V4f>>); c.CodeList_of_Matrix_of_V4f__(ref v); return v; } },

            { typeof(Volume<V4f>), c => { var v = default(Volume<V4f>); c.CodeVolume_of_V4f_(ref v); return v; } },
            { typeof(Volume<V4f>[]), c => { var v = default(Volume<V4f>[]); c.CodeVolume_of_V4f_Array(ref v); return v; } },
            { typeof(List<Volume<V4f>>), c => { var v = default(List<Volume<V4f>>); c.CodeList_of_Volume_of_V4f__(ref v); return v; } },

            { typeof(Tensor<V4f>), c => { var v = default(Tensor<V4f>); c.CodeTensor_of_V4f_(ref v); return v; } },
            { typeof(Tensor<V4f>[]), c => { var v = default(Tensor<V4f>[]); c.CodeTensor_of_V4f_Array(ref v); return v; } },
            { typeof(List<Tensor<V4f>>), c => { var v = default(List<Tensor<V4f>>); c.CodeList_of_Tensor_of_V4f__(ref v); return v; } },

            #endregion

            #region V4d

            { typeof(V4d), c => { var v = default(V4d); c.CodeV4d(ref v); return v; } },
            { typeof(V4d[]), c => { var v = default(V4d[]); c.CodeV4dArray(ref v); return v; } },
            { typeof(List<V4d>), c => { var v = default(List<V4d>); c.CodeList_of_V4d_(ref v); return v; } },

            { typeof(Vector<V4d>), c => { var v = default(Vector<V4d>); c.CodeVector_of_V4d_(ref v); return v; } },
            { typeof(Vector<V4d>[]), c => { var v = default(Vector<V4d>[]); c.CodeVector_of_V4d_Array(ref v); return v; } },
            { typeof(List<Vector<V4d>>), c => { var v = default(List<Vector<V4d>>); c.CodeList_of_Vector_of_V4d__(ref v); return v; } },

            { typeof(Matrix<V4d>), c => { var v = default(Matrix<V4d>); c.CodeMatrix_of_V4d_(ref v); return v; } },
            { typeof(Matrix<V4d>[]), c => { var v = default(Matrix<V4d>[]); c.CodeMatrix_of_V4d_Array(ref v); return v; } },
            { typeof(List<Matrix<V4d>>), c => { var v = default(List<Matrix<V4d>>); c.CodeList_of_Matrix_of_V4d__(ref v); return v; } },

            { typeof(Volume<V4d>), c => { var v = default(Volume<V4d>); c.CodeVolume_of_V4d_(ref v); return v; } },
            { typeof(Volume<V4d>[]), c => { var v = default(Volume<V4d>[]); c.CodeVolume_of_V4d_Array(ref v); return v; } },
            { typeof(List<Volume<V4d>>), c => { var v = default(List<Volume<V4d>>); c.CodeList_of_Volume_of_V4d__(ref v); return v; } },

            { typeof(Tensor<V4d>), c => { var v = default(Tensor<V4d>); c.CodeTensor_of_V4d_(ref v); return v; } },
            { typeof(Tensor<V4d>[]), c => { var v = default(Tensor<V4d>[]); c.CodeTensor_of_V4d_Array(ref v); return v; } },
            { typeof(List<Tensor<V4d>>), c => { var v = default(List<Tensor<V4d>>); c.CodeList_of_Tensor_of_V4d__(ref v); return v; } },

            #endregion

            #region M22i

            { typeof(M22i), c => { var v = default(M22i); c.CodeM22i(ref v); return v; } },
            { typeof(M22i[]), c => { var v = default(M22i[]); c.CodeM22iArray(ref v); return v; } },
            { typeof(List<M22i>), c => { var v = default(List<M22i>); c.CodeList_of_M22i_(ref v); return v; } },

            { typeof(Vector<M22i>), c => { var v = default(Vector<M22i>); c.CodeVector_of_M22i_(ref v); return v; } },
            { typeof(Vector<M22i>[]), c => { var v = default(Vector<M22i>[]); c.CodeVector_of_M22i_Array(ref v); return v; } },
            { typeof(List<Vector<M22i>>), c => { var v = default(List<Vector<M22i>>); c.CodeList_of_Vector_of_M22i__(ref v); return v; } },

            { typeof(Matrix<M22i>), c => { var v = default(Matrix<M22i>); c.CodeMatrix_of_M22i_(ref v); return v; } },
            { typeof(Matrix<M22i>[]), c => { var v = default(Matrix<M22i>[]); c.CodeMatrix_of_M22i_Array(ref v); return v; } },
            { typeof(List<Matrix<M22i>>), c => { var v = default(List<Matrix<M22i>>); c.CodeList_of_Matrix_of_M22i__(ref v); return v; } },

            { typeof(Volume<M22i>), c => { var v = default(Volume<M22i>); c.CodeVolume_of_M22i_(ref v); return v; } },
            { typeof(Volume<M22i>[]), c => { var v = default(Volume<M22i>[]); c.CodeVolume_of_M22i_Array(ref v); return v; } },
            { typeof(List<Volume<M22i>>), c => { var v = default(List<Volume<M22i>>); c.CodeList_of_Volume_of_M22i__(ref v); return v; } },

            { typeof(Tensor<M22i>), c => { var v = default(Tensor<M22i>); c.CodeTensor_of_M22i_(ref v); return v; } },
            { typeof(Tensor<M22i>[]), c => { var v = default(Tensor<M22i>[]); c.CodeTensor_of_M22i_Array(ref v); return v; } },
            { typeof(List<Tensor<M22i>>), c => { var v = default(List<Tensor<M22i>>); c.CodeList_of_Tensor_of_M22i__(ref v); return v; } },

            #endregion

            #region M22l

            { typeof(M22l), c => { var v = default(M22l); c.CodeM22l(ref v); return v; } },
            { typeof(M22l[]), c => { var v = default(M22l[]); c.CodeM22lArray(ref v); return v; } },
            { typeof(List<M22l>), c => { var v = default(List<M22l>); c.CodeList_of_M22l_(ref v); return v; } },

            { typeof(Vector<M22l>), c => { var v = default(Vector<M22l>); c.CodeVector_of_M22l_(ref v); return v; } },
            { typeof(Vector<M22l>[]), c => { var v = default(Vector<M22l>[]); c.CodeVector_of_M22l_Array(ref v); return v; } },
            { typeof(List<Vector<M22l>>), c => { var v = default(List<Vector<M22l>>); c.CodeList_of_Vector_of_M22l__(ref v); return v; } },

            { typeof(Matrix<M22l>), c => { var v = default(Matrix<M22l>); c.CodeMatrix_of_M22l_(ref v); return v; } },
            { typeof(Matrix<M22l>[]), c => { var v = default(Matrix<M22l>[]); c.CodeMatrix_of_M22l_Array(ref v); return v; } },
            { typeof(List<Matrix<M22l>>), c => { var v = default(List<Matrix<M22l>>); c.CodeList_of_Matrix_of_M22l__(ref v); return v; } },

            { typeof(Volume<M22l>), c => { var v = default(Volume<M22l>); c.CodeVolume_of_M22l_(ref v); return v; } },
            { typeof(Volume<M22l>[]), c => { var v = default(Volume<M22l>[]); c.CodeVolume_of_M22l_Array(ref v); return v; } },
            { typeof(List<Volume<M22l>>), c => { var v = default(List<Volume<M22l>>); c.CodeList_of_Volume_of_M22l__(ref v); return v; } },

            { typeof(Tensor<M22l>), c => { var v = default(Tensor<M22l>); c.CodeTensor_of_M22l_(ref v); return v; } },
            { typeof(Tensor<M22l>[]), c => { var v = default(Tensor<M22l>[]); c.CodeTensor_of_M22l_Array(ref v); return v; } },
            { typeof(List<Tensor<M22l>>), c => { var v = default(List<Tensor<M22l>>); c.CodeList_of_Tensor_of_M22l__(ref v); return v; } },

            #endregion

            #region M22f

            { typeof(M22f), c => { var v = default(M22f); c.CodeM22f(ref v); return v; } },
            { typeof(M22f[]), c => { var v = default(M22f[]); c.CodeM22fArray(ref v); return v; } },
            { typeof(List<M22f>), c => { var v = default(List<M22f>); c.CodeList_of_M22f_(ref v); return v; } },

            { typeof(Vector<M22f>), c => { var v = default(Vector<M22f>); c.CodeVector_of_M22f_(ref v); return v; } },
            { typeof(Vector<M22f>[]), c => { var v = default(Vector<M22f>[]); c.CodeVector_of_M22f_Array(ref v); return v; } },
            { typeof(List<Vector<M22f>>), c => { var v = default(List<Vector<M22f>>); c.CodeList_of_Vector_of_M22f__(ref v); return v; } },

            { typeof(Matrix<M22f>), c => { var v = default(Matrix<M22f>); c.CodeMatrix_of_M22f_(ref v); return v; } },
            { typeof(Matrix<M22f>[]), c => { var v = default(Matrix<M22f>[]); c.CodeMatrix_of_M22f_Array(ref v); return v; } },
            { typeof(List<Matrix<M22f>>), c => { var v = default(List<Matrix<M22f>>); c.CodeList_of_Matrix_of_M22f__(ref v); return v; } },

            { typeof(Volume<M22f>), c => { var v = default(Volume<M22f>); c.CodeVolume_of_M22f_(ref v); return v; } },
            { typeof(Volume<M22f>[]), c => { var v = default(Volume<M22f>[]); c.CodeVolume_of_M22f_Array(ref v); return v; } },
            { typeof(List<Volume<M22f>>), c => { var v = default(List<Volume<M22f>>); c.CodeList_of_Volume_of_M22f__(ref v); return v; } },

            { typeof(Tensor<M22f>), c => { var v = default(Tensor<M22f>); c.CodeTensor_of_M22f_(ref v); return v; } },
            { typeof(Tensor<M22f>[]), c => { var v = default(Tensor<M22f>[]); c.CodeTensor_of_M22f_Array(ref v); return v; } },
            { typeof(List<Tensor<M22f>>), c => { var v = default(List<Tensor<M22f>>); c.CodeList_of_Tensor_of_M22f__(ref v); return v; } },

            #endregion

            #region M22d

            { typeof(M22d), c => { var v = default(M22d); c.CodeM22d(ref v); return v; } },
            { typeof(M22d[]), c => { var v = default(M22d[]); c.CodeM22dArray(ref v); return v; } },
            { typeof(List<M22d>), c => { var v = default(List<M22d>); c.CodeList_of_M22d_(ref v); return v; } },

            { typeof(Vector<M22d>), c => { var v = default(Vector<M22d>); c.CodeVector_of_M22d_(ref v); return v; } },
            { typeof(Vector<M22d>[]), c => { var v = default(Vector<M22d>[]); c.CodeVector_of_M22d_Array(ref v); return v; } },
            { typeof(List<Vector<M22d>>), c => { var v = default(List<Vector<M22d>>); c.CodeList_of_Vector_of_M22d__(ref v); return v; } },

            { typeof(Matrix<M22d>), c => { var v = default(Matrix<M22d>); c.CodeMatrix_of_M22d_(ref v); return v; } },
            { typeof(Matrix<M22d>[]), c => { var v = default(Matrix<M22d>[]); c.CodeMatrix_of_M22d_Array(ref v); return v; } },
            { typeof(List<Matrix<M22d>>), c => { var v = default(List<Matrix<M22d>>); c.CodeList_of_Matrix_of_M22d__(ref v); return v; } },

            { typeof(Volume<M22d>), c => { var v = default(Volume<M22d>); c.CodeVolume_of_M22d_(ref v); return v; } },
            { typeof(Volume<M22d>[]), c => { var v = default(Volume<M22d>[]); c.CodeVolume_of_M22d_Array(ref v); return v; } },
            { typeof(List<Volume<M22d>>), c => { var v = default(List<Volume<M22d>>); c.CodeList_of_Volume_of_M22d__(ref v); return v; } },

            { typeof(Tensor<M22d>), c => { var v = default(Tensor<M22d>); c.CodeTensor_of_M22d_(ref v); return v; } },
            { typeof(Tensor<M22d>[]), c => { var v = default(Tensor<M22d>[]); c.CodeTensor_of_M22d_Array(ref v); return v; } },
            { typeof(List<Tensor<M22d>>), c => { var v = default(List<Tensor<M22d>>); c.CodeList_of_Tensor_of_M22d__(ref v); return v; } },

            #endregion

            #region M23i

            { typeof(M23i), c => { var v = default(M23i); c.CodeM23i(ref v); return v; } },
            { typeof(M23i[]), c => { var v = default(M23i[]); c.CodeM23iArray(ref v); return v; } },
            { typeof(List<M23i>), c => { var v = default(List<M23i>); c.CodeList_of_M23i_(ref v); return v; } },

            { typeof(Vector<M23i>), c => { var v = default(Vector<M23i>); c.CodeVector_of_M23i_(ref v); return v; } },
            { typeof(Vector<M23i>[]), c => { var v = default(Vector<M23i>[]); c.CodeVector_of_M23i_Array(ref v); return v; } },
            { typeof(List<Vector<M23i>>), c => { var v = default(List<Vector<M23i>>); c.CodeList_of_Vector_of_M23i__(ref v); return v; } },

            { typeof(Matrix<M23i>), c => { var v = default(Matrix<M23i>); c.CodeMatrix_of_M23i_(ref v); return v; } },
            { typeof(Matrix<M23i>[]), c => { var v = default(Matrix<M23i>[]); c.CodeMatrix_of_M23i_Array(ref v); return v; } },
            { typeof(List<Matrix<M23i>>), c => { var v = default(List<Matrix<M23i>>); c.CodeList_of_Matrix_of_M23i__(ref v); return v; } },

            { typeof(Volume<M23i>), c => { var v = default(Volume<M23i>); c.CodeVolume_of_M23i_(ref v); return v; } },
            { typeof(Volume<M23i>[]), c => { var v = default(Volume<M23i>[]); c.CodeVolume_of_M23i_Array(ref v); return v; } },
            { typeof(List<Volume<M23i>>), c => { var v = default(List<Volume<M23i>>); c.CodeList_of_Volume_of_M23i__(ref v); return v; } },

            { typeof(Tensor<M23i>), c => { var v = default(Tensor<M23i>); c.CodeTensor_of_M23i_(ref v); return v; } },
            { typeof(Tensor<M23i>[]), c => { var v = default(Tensor<M23i>[]); c.CodeTensor_of_M23i_Array(ref v); return v; } },
            { typeof(List<Tensor<M23i>>), c => { var v = default(List<Tensor<M23i>>); c.CodeList_of_Tensor_of_M23i__(ref v); return v; } },

            #endregion

            #region M23l

            { typeof(M23l), c => { var v = default(M23l); c.CodeM23l(ref v); return v; } },
            { typeof(M23l[]), c => { var v = default(M23l[]); c.CodeM23lArray(ref v); return v; } },
            { typeof(List<M23l>), c => { var v = default(List<M23l>); c.CodeList_of_M23l_(ref v); return v; } },

            { typeof(Vector<M23l>), c => { var v = default(Vector<M23l>); c.CodeVector_of_M23l_(ref v); return v; } },
            { typeof(Vector<M23l>[]), c => { var v = default(Vector<M23l>[]); c.CodeVector_of_M23l_Array(ref v); return v; } },
            { typeof(List<Vector<M23l>>), c => { var v = default(List<Vector<M23l>>); c.CodeList_of_Vector_of_M23l__(ref v); return v; } },

            { typeof(Matrix<M23l>), c => { var v = default(Matrix<M23l>); c.CodeMatrix_of_M23l_(ref v); return v; } },
            { typeof(Matrix<M23l>[]), c => { var v = default(Matrix<M23l>[]); c.CodeMatrix_of_M23l_Array(ref v); return v; } },
            { typeof(List<Matrix<M23l>>), c => { var v = default(List<Matrix<M23l>>); c.CodeList_of_Matrix_of_M23l__(ref v); return v; } },

            { typeof(Volume<M23l>), c => { var v = default(Volume<M23l>); c.CodeVolume_of_M23l_(ref v); return v; } },
            { typeof(Volume<M23l>[]), c => { var v = default(Volume<M23l>[]); c.CodeVolume_of_M23l_Array(ref v); return v; } },
            { typeof(List<Volume<M23l>>), c => { var v = default(List<Volume<M23l>>); c.CodeList_of_Volume_of_M23l__(ref v); return v; } },

            { typeof(Tensor<M23l>), c => { var v = default(Tensor<M23l>); c.CodeTensor_of_M23l_(ref v); return v; } },
            { typeof(Tensor<M23l>[]), c => { var v = default(Tensor<M23l>[]); c.CodeTensor_of_M23l_Array(ref v); return v; } },
            { typeof(List<Tensor<M23l>>), c => { var v = default(List<Tensor<M23l>>); c.CodeList_of_Tensor_of_M23l__(ref v); return v; } },

            #endregion

            #region M23f

            { typeof(M23f), c => { var v = default(M23f); c.CodeM23f(ref v); return v; } },
            { typeof(M23f[]), c => { var v = default(M23f[]); c.CodeM23fArray(ref v); return v; } },
            { typeof(List<M23f>), c => { var v = default(List<M23f>); c.CodeList_of_M23f_(ref v); return v; } },

            { typeof(Vector<M23f>), c => { var v = default(Vector<M23f>); c.CodeVector_of_M23f_(ref v); return v; } },
            { typeof(Vector<M23f>[]), c => { var v = default(Vector<M23f>[]); c.CodeVector_of_M23f_Array(ref v); return v; } },
            { typeof(List<Vector<M23f>>), c => { var v = default(List<Vector<M23f>>); c.CodeList_of_Vector_of_M23f__(ref v); return v; } },

            { typeof(Matrix<M23f>), c => { var v = default(Matrix<M23f>); c.CodeMatrix_of_M23f_(ref v); return v; } },
            { typeof(Matrix<M23f>[]), c => { var v = default(Matrix<M23f>[]); c.CodeMatrix_of_M23f_Array(ref v); return v; } },
            { typeof(List<Matrix<M23f>>), c => { var v = default(List<Matrix<M23f>>); c.CodeList_of_Matrix_of_M23f__(ref v); return v; } },

            { typeof(Volume<M23f>), c => { var v = default(Volume<M23f>); c.CodeVolume_of_M23f_(ref v); return v; } },
            { typeof(Volume<M23f>[]), c => { var v = default(Volume<M23f>[]); c.CodeVolume_of_M23f_Array(ref v); return v; } },
            { typeof(List<Volume<M23f>>), c => { var v = default(List<Volume<M23f>>); c.CodeList_of_Volume_of_M23f__(ref v); return v; } },

            { typeof(Tensor<M23f>), c => { var v = default(Tensor<M23f>); c.CodeTensor_of_M23f_(ref v); return v; } },
            { typeof(Tensor<M23f>[]), c => { var v = default(Tensor<M23f>[]); c.CodeTensor_of_M23f_Array(ref v); return v; } },
            { typeof(List<Tensor<M23f>>), c => { var v = default(List<Tensor<M23f>>); c.CodeList_of_Tensor_of_M23f__(ref v); return v; } },

            #endregion

            #region M23d

            { typeof(M23d), c => { var v = default(M23d); c.CodeM23d(ref v); return v; } },
            { typeof(M23d[]), c => { var v = default(M23d[]); c.CodeM23dArray(ref v); return v; } },
            { typeof(List<M23d>), c => { var v = default(List<M23d>); c.CodeList_of_M23d_(ref v); return v; } },

            { typeof(Vector<M23d>), c => { var v = default(Vector<M23d>); c.CodeVector_of_M23d_(ref v); return v; } },
            { typeof(Vector<M23d>[]), c => { var v = default(Vector<M23d>[]); c.CodeVector_of_M23d_Array(ref v); return v; } },
            { typeof(List<Vector<M23d>>), c => { var v = default(List<Vector<M23d>>); c.CodeList_of_Vector_of_M23d__(ref v); return v; } },

            { typeof(Matrix<M23d>), c => { var v = default(Matrix<M23d>); c.CodeMatrix_of_M23d_(ref v); return v; } },
            { typeof(Matrix<M23d>[]), c => { var v = default(Matrix<M23d>[]); c.CodeMatrix_of_M23d_Array(ref v); return v; } },
            { typeof(List<Matrix<M23d>>), c => { var v = default(List<Matrix<M23d>>); c.CodeList_of_Matrix_of_M23d__(ref v); return v; } },

            { typeof(Volume<M23d>), c => { var v = default(Volume<M23d>); c.CodeVolume_of_M23d_(ref v); return v; } },
            { typeof(Volume<M23d>[]), c => { var v = default(Volume<M23d>[]); c.CodeVolume_of_M23d_Array(ref v); return v; } },
            { typeof(List<Volume<M23d>>), c => { var v = default(List<Volume<M23d>>); c.CodeList_of_Volume_of_M23d__(ref v); return v; } },

            { typeof(Tensor<M23d>), c => { var v = default(Tensor<M23d>); c.CodeTensor_of_M23d_(ref v); return v; } },
            { typeof(Tensor<M23d>[]), c => { var v = default(Tensor<M23d>[]); c.CodeTensor_of_M23d_Array(ref v); return v; } },
            { typeof(List<Tensor<M23d>>), c => { var v = default(List<Tensor<M23d>>); c.CodeList_of_Tensor_of_M23d__(ref v); return v; } },

            #endregion

            #region M33i

            { typeof(M33i), c => { var v = default(M33i); c.CodeM33i(ref v); return v; } },
            { typeof(M33i[]), c => { var v = default(M33i[]); c.CodeM33iArray(ref v); return v; } },
            { typeof(List<M33i>), c => { var v = default(List<M33i>); c.CodeList_of_M33i_(ref v); return v; } },

            { typeof(Vector<M33i>), c => { var v = default(Vector<M33i>); c.CodeVector_of_M33i_(ref v); return v; } },
            { typeof(Vector<M33i>[]), c => { var v = default(Vector<M33i>[]); c.CodeVector_of_M33i_Array(ref v); return v; } },
            { typeof(List<Vector<M33i>>), c => { var v = default(List<Vector<M33i>>); c.CodeList_of_Vector_of_M33i__(ref v); return v; } },

            { typeof(Matrix<M33i>), c => { var v = default(Matrix<M33i>); c.CodeMatrix_of_M33i_(ref v); return v; } },
            { typeof(Matrix<M33i>[]), c => { var v = default(Matrix<M33i>[]); c.CodeMatrix_of_M33i_Array(ref v); return v; } },
            { typeof(List<Matrix<M33i>>), c => { var v = default(List<Matrix<M33i>>); c.CodeList_of_Matrix_of_M33i__(ref v); return v; } },

            { typeof(Volume<M33i>), c => { var v = default(Volume<M33i>); c.CodeVolume_of_M33i_(ref v); return v; } },
            { typeof(Volume<M33i>[]), c => { var v = default(Volume<M33i>[]); c.CodeVolume_of_M33i_Array(ref v); return v; } },
            { typeof(List<Volume<M33i>>), c => { var v = default(List<Volume<M33i>>); c.CodeList_of_Volume_of_M33i__(ref v); return v; } },

            { typeof(Tensor<M33i>), c => { var v = default(Tensor<M33i>); c.CodeTensor_of_M33i_(ref v); return v; } },
            { typeof(Tensor<M33i>[]), c => { var v = default(Tensor<M33i>[]); c.CodeTensor_of_M33i_Array(ref v); return v; } },
            { typeof(List<Tensor<M33i>>), c => { var v = default(List<Tensor<M33i>>); c.CodeList_of_Tensor_of_M33i__(ref v); return v; } },

            #endregion

            #region M33l

            { typeof(M33l), c => { var v = default(M33l); c.CodeM33l(ref v); return v; } },
            { typeof(M33l[]), c => { var v = default(M33l[]); c.CodeM33lArray(ref v); return v; } },
            { typeof(List<M33l>), c => { var v = default(List<M33l>); c.CodeList_of_M33l_(ref v); return v; } },

            { typeof(Vector<M33l>), c => { var v = default(Vector<M33l>); c.CodeVector_of_M33l_(ref v); return v; } },
            { typeof(Vector<M33l>[]), c => { var v = default(Vector<M33l>[]); c.CodeVector_of_M33l_Array(ref v); return v; } },
            { typeof(List<Vector<M33l>>), c => { var v = default(List<Vector<M33l>>); c.CodeList_of_Vector_of_M33l__(ref v); return v; } },

            { typeof(Matrix<M33l>), c => { var v = default(Matrix<M33l>); c.CodeMatrix_of_M33l_(ref v); return v; } },
            { typeof(Matrix<M33l>[]), c => { var v = default(Matrix<M33l>[]); c.CodeMatrix_of_M33l_Array(ref v); return v; } },
            { typeof(List<Matrix<M33l>>), c => { var v = default(List<Matrix<M33l>>); c.CodeList_of_Matrix_of_M33l__(ref v); return v; } },

            { typeof(Volume<M33l>), c => { var v = default(Volume<M33l>); c.CodeVolume_of_M33l_(ref v); return v; } },
            { typeof(Volume<M33l>[]), c => { var v = default(Volume<M33l>[]); c.CodeVolume_of_M33l_Array(ref v); return v; } },
            { typeof(List<Volume<M33l>>), c => { var v = default(List<Volume<M33l>>); c.CodeList_of_Volume_of_M33l__(ref v); return v; } },

            { typeof(Tensor<M33l>), c => { var v = default(Tensor<M33l>); c.CodeTensor_of_M33l_(ref v); return v; } },
            { typeof(Tensor<M33l>[]), c => { var v = default(Tensor<M33l>[]); c.CodeTensor_of_M33l_Array(ref v); return v; } },
            { typeof(List<Tensor<M33l>>), c => { var v = default(List<Tensor<M33l>>); c.CodeList_of_Tensor_of_M33l__(ref v); return v; } },

            #endregion

            #region M33f

            { typeof(M33f), c => { var v = default(M33f); c.CodeM33f(ref v); return v; } },
            { typeof(M33f[]), c => { var v = default(M33f[]); c.CodeM33fArray(ref v); return v; } },
            { typeof(List<M33f>), c => { var v = default(List<M33f>); c.CodeList_of_M33f_(ref v); return v; } },

            { typeof(Vector<M33f>), c => { var v = default(Vector<M33f>); c.CodeVector_of_M33f_(ref v); return v; } },
            { typeof(Vector<M33f>[]), c => { var v = default(Vector<M33f>[]); c.CodeVector_of_M33f_Array(ref v); return v; } },
            { typeof(List<Vector<M33f>>), c => { var v = default(List<Vector<M33f>>); c.CodeList_of_Vector_of_M33f__(ref v); return v; } },

            { typeof(Matrix<M33f>), c => { var v = default(Matrix<M33f>); c.CodeMatrix_of_M33f_(ref v); return v; } },
            { typeof(Matrix<M33f>[]), c => { var v = default(Matrix<M33f>[]); c.CodeMatrix_of_M33f_Array(ref v); return v; } },
            { typeof(List<Matrix<M33f>>), c => { var v = default(List<Matrix<M33f>>); c.CodeList_of_Matrix_of_M33f__(ref v); return v; } },

            { typeof(Volume<M33f>), c => { var v = default(Volume<M33f>); c.CodeVolume_of_M33f_(ref v); return v; } },
            { typeof(Volume<M33f>[]), c => { var v = default(Volume<M33f>[]); c.CodeVolume_of_M33f_Array(ref v); return v; } },
            { typeof(List<Volume<M33f>>), c => { var v = default(List<Volume<M33f>>); c.CodeList_of_Volume_of_M33f__(ref v); return v; } },

            { typeof(Tensor<M33f>), c => { var v = default(Tensor<M33f>); c.CodeTensor_of_M33f_(ref v); return v; } },
            { typeof(Tensor<M33f>[]), c => { var v = default(Tensor<M33f>[]); c.CodeTensor_of_M33f_Array(ref v); return v; } },
            { typeof(List<Tensor<M33f>>), c => { var v = default(List<Tensor<M33f>>); c.CodeList_of_Tensor_of_M33f__(ref v); return v; } },

            #endregion

            #region M33d

            { typeof(M33d), c => { var v = default(M33d); c.CodeM33d(ref v); return v; } },
            { typeof(M33d[]), c => { var v = default(M33d[]); c.CodeM33dArray(ref v); return v; } },
            { typeof(List<M33d>), c => { var v = default(List<M33d>); c.CodeList_of_M33d_(ref v); return v; } },

            { typeof(Vector<M33d>), c => { var v = default(Vector<M33d>); c.CodeVector_of_M33d_(ref v); return v; } },
            { typeof(Vector<M33d>[]), c => { var v = default(Vector<M33d>[]); c.CodeVector_of_M33d_Array(ref v); return v; } },
            { typeof(List<Vector<M33d>>), c => { var v = default(List<Vector<M33d>>); c.CodeList_of_Vector_of_M33d__(ref v); return v; } },

            { typeof(Matrix<M33d>), c => { var v = default(Matrix<M33d>); c.CodeMatrix_of_M33d_(ref v); return v; } },
            { typeof(Matrix<M33d>[]), c => { var v = default(Matrix<M33d>[]); c.CodeMatrix_of_M33d_Array(ref v); return v; } },
            { typeof(List<Matrix<M33d>>), c => { var v = default(List<Matrix<M33d>>); c.CodeList_of_Matrix_of_M33d__(ref v); return v; } },

            { typeof(Volume<M33d>), c => { var v = default(Volume<M33d>); c.CodeVolume_of_M33d_(ref v); return v; } },
            { typeof(Volume<M33d>[]), c => { var v = default(Volume<M33d>[]); c.CodeVolume_of_M33d_Array(ref v); return v; } },
            { typeof(List<Volume<M33d>>), c => { var v = default(List<Volume<M33d>>); c.CodeList_of_Volume_of_M33d__(ref v); return v; } },

            { typeof(Tensor<M33d>), c => { var v = default(Tensor<M33d>); c.CodeTensor_of_M33d_(ref v); return v; } },
            { typeof(Tensor<M33d>[]), c => { var v = default(Tensor<M33d>[]); c.CodeTensor_of_M33d_Array(ref v); return v; } },
            { typeof(List<Tensor<M33d>>), c => { var v = default(List<Tensor<M33d>>); c.CodeList_of_Tensor_of_M33d__(ref v); return v; } },

            #endregion

            #region M34i

            { typeof(M34i), c => { var v = default(M34i); c.CodeM34i(ref v); return v; } },
            { typeof(M34i[]), c => { var v = default(M34i[]); c.CodeM34iArray(ref v); return v; } },
            { typeof(List<M34i>), c => { var v = default(List<M34i>); c.CodeList_of_M34i_(ref v); return v; } },

            { typeof(Vector<M34i>), c => { var v = default(Vector<M34i>); c.CodeVector_of_M34i_(ref v); return v; } },
            { typeof(Vector<M34i>[]), c => { var v = default(Vector<M34i>[]); c.CodeVector_of_M34i_Array(ref v); return v; } },
            { typeof(List<Vector<M34i>>), c => { var v = default(List<Vector<M34i>>); c.CodeList_of_Vector_of_M34i__(ref v); return v; } },

            { typeof(Matrix<M34i>), c => { var v = default(Matrix<M34i>); c.CodeMatrix_of_M34i_(ref v); return v; } },
            { typeof(Matrix<M34i>[]), c => { var v = default(Matrix<M34i>[]); c.CodeMatrix_of_M34i_Array(ref v); return v; } },
            { typeof(List<Matrix<M34i>>), c => { var v = default(List<Matrix<M34i>>); c.CodeList_of_Matrix_of_M34i__(ref v); return v; } },

            { typeof(Volume<M34i>), c => { var v = default(Volume<M34i>); c.CodeVolume_of_M34i_(ref v); return v; } },
            { typeof(Volume<M34i>[]), c => { var v = default(Volume<M34i>[]); c.CodeVolume_of_M34i_Array(ref v); return v; } },
            { typeof(List<Volume<M34i>>), c => { var v = default(List<Volume<M34i>>); c.CodeList_of_Volume_of_M34i__(ref v); return v; } },

            { typeof(Tensor<M34i>), c => { var v = default(Tensor<M34i>); c.CodeTensor_of_M34i_(ref v); return v; } },
            { typeof(Tensor<M34i>[]), c => { var v = default(Tensor<M34i>[]); c.CodeTensor_of_M34i_Array(ref v); return v; } },
            { typeof(List<Tensor<M34i>>), c => { var v = default(List<Tensor<M34i>>); c.CodeList_of_Tensor_of_M34i__(ref v); return v; } },

            #endregion

            #region M34l

            { typeof(M34l), c => { var v = default(M34l); c.CodeM34l(ref v); return v; } },
            { typeof(M34l[]), c => { var v = default(M34l[]); c.CodeM34lArray(ref v); return v; } },
            { typeof(List<M34l>), c => { var v = default(List<M34l>); c.CodeList_of_M34l_(ref v); return v; } },

            { typeof(Vector<M34l>), c => { var v = default(Vector<M34l>); c.CodeVector_of_M34l_(ref v); return v; } },
            { typeof(Vector<M34l>[]), c => { var v = default(Vector<M34l>[]); c.CodeVector_of_M34l_Array(ref v); return v; } },
            { typeof(List<Vector<M34l>>), c => { var v = default(List<Vector<M34l>>); c.CodeList_of_Vector_of_M34l__(ref v); return v; } },

            { typeof(Matrix<M34l>), c => { var v = default(Matrix<M34l>); c.CodeMatrix_of_M34l_(ref v); return v; } },
            { typeof(Matrix<M34l>[]), c => { var v = default(Matrix<M34l>[]); c.CodeMatrix_of_M34l_Array(ref v); return v; } },
            { typeof(List<Matrix<M34l>>), c => { var v = default(List<Matrix<M34l>>); c.CodeList_of_Matrix_of_M34l__(ref v); return v; } },

            { typeof(Volume<M34l>), c => { var v = default(Volume<M34l>); c.CodeVolume_of_M34l_(ref v); return v; } },
            { typeof(Volume<M34l>[]), c => { var v = default(Volume<M34l>[]); c.CodeVolume_of_M34l_Array(ref v); return v; } },
            { typeof(List<Volume<M34l>>), c => { var v = default(List<Volume<M34l>>); c.CodeList_of_Volume_of_M34l__(ref v); return v; } },

            { typeof(Tensor<M34l>), c => { var v = default(Tensor<M34l>); c.CodeTensor_of_M34l_(ref v); return v; } },
            { typeof(Tensor<M34l>[]), c => { var v = default(Tensor<M34l>[]); c.CodeTensor_of_M34l_Array(ref v); return v; } },
            { typeof(List<Tensor<M34l>>), c => { var v = default(List<Tensor<M34l>>); c.CodeList_of_Tensor_of_M34l__(ref v); return v; } },

            #endregion

            #region M34f

            { typeof(M34f), c => { var v = default(M34f); c.CodeM34f(ref v); return v; } },
            { typeof(M34f[]), c => { var v = default(M34f[]); c.CodeM34fArray(ref v); return v; } },
            { typeof(List<M34f>), c => { var v = default(List<M34f>); c.CodeList_of_M34f_(ref v); return v; } },

            { typeof(Vector<M34f>), c => { var v = default(Vector<M34f>); c.CodeVector_of_M34f_(ref v); return v; } },
            { typeof(Vector<M34f>[]), c => { var v = default(Vector<M34f>[]); c.CodeVector_of_M34f_Array(ref v); return v; } },
            { typeof(List<Vector<M34f>>), c => { var v = default(List<Vector<M34f>>); c.CodeList_of_Vector_of_M34f__(ref v); return v; } },

            { typeof(Matrix<M34f>), c => { var v = default(Matrix<M34f>); c.CodeMatrix_of_M34f_(ref v); return v; } },
            { typeof(Matrix<M34f>[]), c => { var v = default(Matrix<M34f>[]); c.CodeMatrix_of_M34f_Array(ref v); return v; } },
            { typeof(List<Matrix<M34f>>), c => { var v = default(List<Matrix<M34f>>); c.CodeList_of_Matrix_of_M34f__(ref v); return v; } },

            { typeof(Volume<M34f>), c => { var v = default(Volume<M34f>); c.CodeVolume_of_M34f_(ref v); return v; } },
            { typeof(Volume<M34f>[]), c => { var v = default(Volume<M34f>[]); c.CodeVolume_of_M34f_Array(ref v); return v; } },
            { typeof(List<Volume<M34f>>), c => { var v = default(List<Volume<M34f>>); c.CodeList_of_Volume_of_M34f__(ref v); return v; } },

            { typeof(Tensor<M34f>), c => { var v = default(Tensor<M34f>); c.CodeTensor_of_M34f_(ref v); return v; } },
            { typeof(Tensor<M34f>[]), c => { var v = default(Tensor<M34f>[]); c.CodeTensor_of_M34f_Array(ref v); return v; } },
            { typeof(List<Tensor<M34f>>), c => { var v = default(List<Tensor<M34f>>); c.CodeList_of_Tensor_of_M34f__(ref v); return v; } },

            #endregion

            #region M34d

            { typeof(M34d), c => { var v = default(M34d); c.CodeM34d(ref v); return v; } },
            { typeof(M34d[]), c => { var v = default(M34d[]); c.CodeM34dArray(ref v); return v; } },
            { typeof(List<M34d>), c => { var v = default(List<M34d>); c.CodeList_of_M34d_(ref v); return v; } },

            { typeof(Vector<M34d>), c => { var v = default(Vector<M34d>); c.CodeVector_of_M34d_(ref v); return v; } },
            { typeof(Vector<M34d>[]), c => { var v = default(Vector<M34d>[]); c.CodeVector_of_M34d_Array(ref v); return v; } },
            { typeof(List<Vector<M34d>>), c => { var v = default(List<Vector<M34d>>); c.CodeList_of_Vector_of_M34d__(ref v); return v; } },

            { typeof(Matrix<M34d>), c => { var v = default(Matrix<M34d>); c.CodeMatrix_of_M34d_(ref v); return v; } },
            { typeof(Matrix<M34d>[]), c => { var v = default(Matrix<M34d>[]); c.CodeMatrix_of_M34d_Array(ref v); return v; } },
            { typeof(List<Matrix<M34d>>), c => { var v = default(List<Matrix<M34d>>); c.CodeList_of_Matrix_of_M34d__(ref v); return v; } },

            { typeof(Volume<M34d>), c => { var v = default(Volume<M34d>); c.CodeVolume_of_M34d_(ref v); return v; } },
            { typeof(Volume<M34d>[]), c => { var v = default(Volume<M34d>[]); c.CodeVolume_of_M34d_Array(ref v); return v; } },
            { typeof(List<Volume<M34d>>), c => { var v = default(List<Volume<M34d>>); c.CodeList_of_Volume_of_M34d__(ref v); return v; } },

            { typeof(Tensor<M34d>), c => { var v = default(Tensor<M34d>); c.CodeTensor_of_M34d_(ref v); return v; } },
            { typeof(Tensor<M34d>[]), c => { var v = default(Tensor<M34d>[]); c.CodeTensor_of_M34d_Array(ref v); return v; } },
            { typeof(List<Tensor<M34d>>), c => { var v = default(List<Tensor<M34d>>); c.CodeList_of_Tensor_of_M34d__(ref v); return v; } },

            #endregion

            #region M44i

            { typeof(M44i), c => { var v = default(M44i); c.CodeM44i(ref v); return v; } },
            { typeof(M44i[]), c => { var v = default(M44i[]); c.CodeM44iArray(ref v); return v; } },
            { typeof(List<M44i>), c => { var v = default(List<M44i>); c.CodeList_of_M44i_(ref v); return v; } },

            { typeof(Vector<M44i>), c => { var v = default(Vector<M44i>); c.CodeVector_of_M44i_(ref v); return v; } },
            { typeof(Vector<M44i>[]), c => { var v = default(Vector<M44i>[]); c.CodeVector_of_M44i_Array(ref v); return v; } },
            { typeof(List<Vector<M44i>>), c => { var v = default(List<Vector<M44i>>); c.CodeList_of_Vector_of_M44i__(ref v); return v; } },

            { typeof(Matrix<M44i>), c => { var v = default(Matrix<M44i>); c.CodeMatrix_of_M44i_(ref v); return v; } },
            { typeof(Matrix<M44i>[]), c => { var v = default(Matrix<M44i>[]); c.CodeMatrix_of_M44i_Array(ref v); return v; } },
            { typeof(List<Matrix<M44i>>), c => { var v = default(List<Matrix<M44i>>); c.CodeList_of_Matrix_of_M44i__(ref v); return v; } },

            { typeof(Volume<M44i>), c => { var v = default(Volume<M44i>); c.CodeVolume_of_M44i_(ref v); return v; } },
            { typeof(Volume<M44i>[]), c => { var v = default(Volume<M44i>[]); c.CodeVolume_of_M44i_Array(ref v); return v; } },
            { typeof(List<Volume<M44i>>), c => { var v = default(List<Volume<M44i>>); c.CodeList_of_Volume_of_M44i__(ref v); return v; } },

            { typeof(Tensor<M44i>), c => { var v = default(Tensor<M44i>); c.CodeTensor_of_M44i_(ref v); return v; } },
            { typeof(Tensor<M44i>[]), c => { var v = default(Tensor<M44i>[]); c.CodeTensor_of_M44i_Array(ref v); return v; } },
            { typeof(List<Tensor<M44i>>), c => { var v = default(List<Tensor<M44i>>); c.CodeList_of_Tensor_of_M44i__(ref v); return v; } },

            #endregion

            #region M44l

            { typeof(M44l), c => { var v = default(M44l); c.CodeM44l(ref v); return v; } },
            { typeof(M44l[]), c => { var v = default(M44l[]); c.CodeM44lArray(ref v); return v; } },
            { typeof(List<M44l>), c => { var v = default(List<M44l>); c.CodeList_of_M44l_(ref v); return v; } },

            { typeof(Vector<M44l>), c => { var v = default(Vector<M44l>); c.CodeVector_of_M44l_(ref v); return v; } },
            { typeof(Vector<M44l>[]), c => { var v = default(Vector<M44l>[]); c.CodeVector_of_M44l_Array(ref v); return v; } },
            { typeof(List<Vector<M44l>>), c => { var v = default(List<Vector<M44l>>); c.CodeList_of_Vector_of_M44l__(ref v); return v; } },

            { typeof(Matrix<M44l>), c => { var v = default(Matrix<M44l>); c.CodeMatrix_of_M44l_(ref v); return v; } },
            { typeof(Matrix<M44l>[]), c => { var v = default(Matrix<M44l>[]); c.CodeMatrix_of_M44l_Array(ref v); return v; } },
            { typeof(List<Matrix<M44l>>), c => { var v = default(List<Matrix<M44l>>); c.CodeList_of_Matrix_of_M44l__(ref v); return v; } },

            { typeof(Volume<M44l>), c => { var v = default(Volume<M44l>); c.CodeVolume_of_M44l_(ref v); return v; } },
            { typeof(Volume<M44l>[]), c => { var v = default(Volume<M44l>[]); c.CodeVolume_of_M44l_Array(ref v); return v; } },
            { typeof(List<Volume<M44l>>), c => { var v = default(List<Volume<M44l>>); c.CodeList_of_Volume_of_M44l__(ref v); return v; } },

            { typeof(Tensor<M44l>), c => { var v = default(Tensor<M44l>); c.CodeTensor_of_M44l_(ref v); return v; } },
            { typeof(Tensor<M44l>[]), c => { var v = default(Tensor<M44l>[]); c.CodeTensor_of_M44l_Array(ref v); return v; } },
            { typeof(List<Tensor<M44l>>), c => { var v = default(List<Tensor<M44l>>); c.CodeList_of_Tensor_of_M44l__(ref v); return v; } },

            #endregion

            #region M44f

            { typeof(M44f), c => { var v = default(M44f); c.CodeM44f(ref v); return v; } },
            { typeof(M44f[]), c => { var v = default(M44f[]); c.CodeM44fArray(ref v); return v; } },
            { typeof(List<M44f>), c => { var v = default(List<M44f>); c.CodeList_of_M44f_(ref v); return v; } },

            { typeof(Vector<M44f>), c => { var v = default(Vector<M44f>); c.CodeVector_of_M44f_(ref v); return v; } },
            { typeof(Vector<M44f>[]), c => { var v = default(Vector<M44f>[]); c.CodeVector_of_M44f_Array(ref v); return v; } },
            { typeof(List<Vector<M44f>>), c => { var v = default(List<Vector<M44f>>); c.CodeList_of_Vector_of_M44f__(ref v); return v; } },

            { typeof(Matrix<M44f>), c => { var v = default(Matrix<M44f>); c.CodeMatrix_of_M44f_(ref v); return v; } },
            { typeof(Matrix<M44f>[]), c => { var v = default(Matrix<M44f>[]); c.CodeMatrix_of_M44f_Array(ref v); return v; } },
            { typeof(List<Matrix<M44f>>), c => { var v = default(List<Matrix<M44f>>); c.CodeList_of_Matrix_of_M44f__(ref v); return v; } },

            { typeof(Volume<M44f>), c => { var v = default(Volume<M44f>); c.CodeVolume_of_M44f_(ref v); return v; } },
            { typeof(Volume<M44f>[]), c => { var v = default(Volume<M44f>[]); c.CodeVolume_of_M44f_Array(ref v); return v; } },
            { typeof(List<Volume<M44f>>), c => { var v = default(List<Volume<M44f>>); c.CodeList_of_Volume_of_M44f__(ref v); return v; } },

            { typeof(Tensor<M44f>), c => { var v = default(Tensor<M44f>); c.CodeTensor_of_M44f_(ref v); return v; } },
            { typeof(Tensor<M44f>[]), c => { var v = default(Tensor<M44f>[]); c.CodeTensor_of_M44f_Array(ref v); return v; } },
            { typeof(List<Tensor<M44f>>), c => { var v = default(List<Tensor<M44f>>); c.CodeList_of_Tensor_of_M44f__(ref v); return v; } },

            #endregion

            #region M44d

            { typeof(M44d), c => { var v = default(M44d); c.CodeM44d(ref v); return v; } },
            { typeof(M44d[]), c => { var v = default(M44d[]); c.CodeM44dArray(ref v); return v; } },
            { typeof(List<M44d>), c => { var v = default(List<M44d>); c.CodeList_of_M44d_(ref v); return v; } },

            { typeof(Vector<M44d>), c => { var v = default(Vector<M44d>); c.CodeVector_of_M44d_(ref v); return v; } },
            { typeof(Vector<M44d>[]), c => { var v = default(Vector<M44d>[]); c.CodeVector_of_M44d_Array(ref v); return v; } },
            { typeof(List<Vector<M44d>>), c => { var v = default(List<Vector<M44d>>); c.CodeList_of_Vector_of_M44d__(ref v); return v; } },

            { typeof(Matrix<M44d>), c => { var v = default(Matrix<M44d>); c.CodeMatrix_of_M44d_(ref v); return v; } },
            { typeof(Matrix<M44d>[]), c => { var v = default(Matrix<M44d>[]); c.CodeMatrix_of_M44d_Array(ref v); return v; } },
            { typeof(List<Matrix<M44d>>), c => { var v = default(List<Matrix<M44d>>); c.CodeList_of_Matrix_of_M44d__(ref v); return v; } },

            { typeof(Volume<M44d>), c => { var v = default(Volume<M44d>); c.CodeVolume_of_M44d_(ref v); return v; } },
            { typeof(Volume<M44d>[]), c => { var v = default(Volume<M44d>[]); c.CodeVolume_of_M44d_Array(ref v); return v; } },
            { typeof(List<Volume<M44d>>), c => { var v = default(List<Volume<M44d>>); c.CodeList_of_Volume_of_M44d__(ref v); return v; } },

            { typeof(Tensor<M44d>), c => { var v = default(Tensor<M44d>); c.CodeTensor_of_M44d_(ref v); return v; } },
            { typeof(Tensor<M44d>[]), c => { var v = default(Tensor<M44d>[]); c.CodeTensor_of_M44d_Array(ref v); return v; } },
            { typeof(List<Tensor<M44d>>), c => { var v = default(List<Tensor<M44d>>); c.CodeList_of_Tensor_of_M44d__(ref v); return v; } },

            #endregion

            #region C3b

            { typeof(C3b), c => { var v = default(C3b); c.CodeC3b(ref v); return v; } },
            { typeof(C3b[]), c => { var v = default(C3b[]); c.CodeC3bArray(ref v); return v; } },
            { typeof(List<C3b>), c => { var v = default(List<C3b>); c.CodeList_of_C3b_(ref v); return v; } },

            { typeof(Vector<C3b>), c => { var v = default(Vector<C3b>); c.CodeVector_of_C3b_(ref v); return v; } },
            { typeof(Vector<C3b>[]), c => { var v = default(Vector<C3b>[]); c.CodeVector_of_C3b_Array(ref v); return v; } },
            { typeof(List<Vector<C3b>>), c => { var v = default(List<Vector<C3b>>); c.CodeList_of_Vector_of_C3b__(ref v); return v; } },

            { typeof(Matrix<C3b>), c => { var v = default(Matrix<C3b>); c.CodeMatrix_of_C3b_(ref v); return v; } },
            { typeof(Matrix<C3b>[]), c => { var v = default(Matrix<C3b>[]); c.CodeMatrix_of_C3b_Array(ref v); return v; } },
            { typeof(List<Matrix<C3b>>), c => { var v = default(List<Matrix<C3b>>); c.CodeList_of_Matrix_of_C3b__(ref v); return v; } },

            { typeof(Volume<C3b>), c => { var v = default(Volume<C3b>); c.CodeVolume_of_C3b_(ref v); return v; } },
            { typeof(Volume<C3b>[]), c => { var v = default(Volume<C3b>[]); c.CodeVolume_of_C3b_Array(ref v); return v; } },
            { typeof(List<Volume<C3b>>), c => { var v = default(List<Volume<C3b>>); c.CodeList_of_Volume_of_C3b__(ref v); return v; } },

            { typeof(Tensor<C3b>), c => { var v = default(Tensor<C3b>); c.CodeTensor_of_C3b_(ref v); return v; } },
            { typeof(Tensor<C3b>[]), c => { var v = default(Tensor<C3b>[]); c.CodeTensor_of_C3b_Array(ref v); return v; } },
            { typeof(List<Tensor<C3b>>), c => { var v = default(List<Tensor<C3b>>); c.CodeList_of_Tensor_of_C3b__(ref v); return v; } },

            #endregion

            #region C3us

            { typeof(C3us), c => { var v = default(C3us); c.CodeC3us(ref v); return v; } },
            { typeof(C3us[]), c => { var v = default(C3us[]); c.CodeC3usArray(ref v); return v; } },
            { typeof(List<C3us>), c => { var v = default(List<C3us>); c.CodeList_of_C3us_(ref v); return v; } },

            { typeof(Vector<C3us>), c => { var v = default(Vector<C3us>); c.CodeVector_of_C3us_(ref v); return v; } },
            { typeof(Vector<C3us>[]), c => { var v = default(Vector<C3us>[]); c.CodeVector_of_C3us_Array(ref v); return v; } },
            { typeof(List<Vector<C3us>>), c => { var v = default(List<Vector<C3us>>); c.CodeList_of_Vector_of_C3us__(ref v); return v; } },

            { typeof(Matrix<C3us>), c => { var v = default(Matrix<C3us>); c.CodeMatrix_of_C3us_(ref v); return v; } },
            { typeof(Matrix<C3us>[]), c => { var v = default(Matrix<C3us>[]); c.CodeMatrix_of_C3us_Array(ref v); return v; } },
            { typeof(List<Matrix<C3us>>), c => { var v = default(List<Matrix<C3us>>); c.CodeList_of_Matrix_of_C3us__(ref v); return v; } },

            { typeof(Volume<C3us>), c => { var v = default(Volume<C3us>); c.CodeVolume_of_C3us_(ref v); return v; } },
            { typeof(Volume<C3us>[]), c => { var v = default(Volume<C3us>[]); c.CodeVolume_of_C3us_Array(ref v); return v; } },
            { typeof(List<Volume<C3us>>), c => { var v = default(List<Volume<C3us>>); c.CodeList_of_Volume_of_C3us__(ref v); return v; } },

            { typeof(Tensor<C3us>), c => { var v = default(Tensor<C3us>); c.CodeTensor_of_C3us_(ref v); return v; } },
            { typeof(Tensor<C3us>[]), c => { var v = default(Tensor<C3us>[]); c.CodeTensor_of_C3us_Array(ref v); return v; } },
            { typeof(List<Tensor<C3us>>), c => { var v = default(List<Tensor<C3us>>); c.CodeList_of_Tensor_of_C3us__(ref v); return v; } },

            #endregion

            #region C3ui

            { typeof(C3ui), c => { var v = default(C3ui); c.CodeC3ui(ref v); return v; } },
            { typeof(C3ui[]), c => { var v = default(C3ui[]); c.CodeC3uiArray(ref v); return v; } },
            { typeof(List<C3ui>), c => { var v = default(List<C3ui>); c.CodeList_of_C3ui_(ref v); return v; } },

            { typeof(Vector<C3ui>), c => { var v = default(Vector<C3ui>); c.CodeVector_of_C3ui_(ref v); return v; } },
            { typeof(Vector<C3ui>[]), c => { var v = default(Vector<C3ui>[]); c.CodeVector_of_C3ui_Array(ref v); return v; } },
            { typeof(List<Vector<C3ui>>), c => { var v = default(List<Vector<C3ui>>); c.CodeList_of_Vector_of_C3ui__(ref v); return v; } },

            { typeof(Matrix<C3ui>), c => { var v = default(Matrix<C3ui>); c.CodeMatrix_of_C3ui_(ref v); return v; } },
            { typeof(Matrix<C3ui>[]), c => { var v = default(Matrix<C3ui>[]); c.CodeMatrix_of_C3ui_Array(ref v); return v; } },
            { typeof(List<Matrix<C3ui>>), c => { var v = default(List<Matrix<C3ui>>); c.CodeList_of_Matrix_of_C3ui__(ref v); return v; } },

            { typeof(Volume<C3ui>), c => { var v = default(Volume<C3ui>); c.CodeVolume_of_C3ui_(ref v); return v; } },
            { typeof(Volume<C3ui>[]), c => { var v = default(Volume<C3ui>[]); c.CodeVolume_of_C3ui_Array(ref v); return v; } },
            { typeof(List<Volume<C3ui>>), c => { var v = default(List<Volume<C3ui>>); c.CodeList_of_Volume_of_C3ui__(ref v); return v; } },

            { typeof(Tensor<C3ui>), c => { var v = default(Tensor<C3ui>); c.CodeTensor_of_C3ui_(ref v); return v; } },
            { typeof(Tensor<C3ui>[]), c => { var v = default(Tensor<C3ui>[]); c.CodeTensor_of_C3ui_Array(ref v); return v; } },
            { typeof(List<Tensor<C3ui>>), c => { var v = default(List<Tensor<C3ui>>); c.CodeList_of_Tensor_of_C3ui__(ref v); return v; } },

            #endregion

            #region C3f

            { typeof(C3f), c => { var v = default(C3f); c.CodeC3f(ref v); return v; } },
            { typeof(C3f[]), c => { var v = default(C3f[]); c.CodeC3fArray(ref v); return v; } },
            { typeof(List<C3f>), c => { var v = default(List<C3f>); c.CodeList_of_C3f_(ref v); return v; } },

            { typeof(Vector<C3f>), c => { var v = default(Vector<C3f>); c.CodeVector_of_C3f_(ref v); return v; } },
            { typeof(Vector<C3f>[]), c => { var v = default(Vector<C3f>[]); c.CodeVector_of_C3f_Array(ref v); return v; } },
            { typeof(List<Vector<C3f>>), c => { var v = default(List<Vector<C3f>>); c.CodeList_of_Vector_of_C3f__(ref v); return v; } },

            { typeof(Matrix<C3f>), c => { var v = default(Matrix<C3f>); c.CodeMatrix_of_C3f_(ref v); return v; } },
            { typeof(Matrix<C3f>[]), c => { var v = default(Matrix<C3f>[]); c.CodeMatrix_of_C3f_Array(ref v); return v; } },
            { typeof(List<Matrix<C3f>>), c => { var v = default(List<Matrix<C3f>>); c.CodeList_of_Matrix_of_C3f__(ref v); return v; } },

            { typeof(Volume<C3f>), c => { var v = default(Volume<C3f>); c.CodeVolume_of_C3f_(ref v); return v; } },
            { typeof(Volume<C3f>[]), c => { var v = default(Volume<C3f>[]); c.CodeVolume_of_C3f_Array(ref v); return v; } },
            { typeof(List<Volume<C3f>>), c => { var v = default(List<Volume<C3f>>); c.CodeList_of_Volume_of_C3f__(ref v); return v; } },

            { typeof(Tensor<C3f>), c => { var v = default(Tensor<C3f>); c.CodeTensor_of_C3f_(ref v); return v; } },
            { typeof(Tensor<C3f>[]), c => { var v = default(Tensor<C3f>[]); c.CodeTensor_of_C3f_Array(ref v); return v; } },
            { typeof(List<Tensor<C3f>>), c => { var v = default(List<Tensor<C3f>>); c.CodeList_of_Tensor_of_C3f__(ref v); return v; } },

            #endregion

            #region C3d

            { typeof(C3d), c => { var v = default(C3d); c.CodeC3d(ref v); return v; } },
            { typeof(C3d[]), c => { var v = default(C3d[]); c.CodeC3dArray(ref v); return v; } },
            { typeof(List<C3d>), c => { var v = default(List<C3d>); c.CodeList_of_C3d_(ref v); return v; } },

            { typeof(Vector<C3d>), c => { var v = default(Vector<C3d>); c.CodeVector_of_C3d_(ref v); return v; } },
            { typeof(Vector<C3d>[]), c => { var v = default(Vector<C3d>[]); c.CodeVector_of_C3d_Array(ref v); return v; } },
            { typeof(List<Vector<C3d>>), c => { var v = default(List<Vector<C3d>>); c.CodeList_of_Vector_of_C3d__(ref v); return v; } },

            { typeof(Matrix<C3d>), c => { var v = default(Matrix<C3d>); c.CodeMatrix_of_C3d_(ref v); return v; } },
            { typeof(Matrix<C3d>[]), c => { var v = default(Matrix<C3d>[]); c.CodeMatrix_of_C3d_Array(ref v); return v; } },
            { typeof(List<Matrix<C3d>>), c => { var v = default(List<Matrix<C3d>>); c.CodeList_of_Matrix_of_C3d__(ref v); return v; } },

            { typeof(Volume<C3d>), c => { var v = default(Volume<C3d>); c.CodeVolume_of_C3d_(ref v); return v; } },
            { typeof(Volume<C3d>[]), c => { var v = default(Volume<C3d>[]); c.CodeVolume_of_C3d_Array(ref v); return v; } },
            { typeof(List<Volume<C3d>>), c => { var v = default(List<Volume<C3d>>); c.CodeList_of_Volume_of_C3d__(ref v); return v; } },

            { typeof(Tensor<C3d>), c => { var v = default(Tensor<C3d>); c.CodeTensor_of_C3d_(ref v); return v; } },
            { typeof(Tensor<C3d>[]), c => { var v = default(Tensor<C3d>[]); c.CodeTensor_of_C3d_Array(ref v); return v; } },
            { typeof(List<Tensor<C3d>>), c => { var v = default(List<Tensor<C3d>>); c.CodeList_of_Tensor_of_C3d__(ref v); return v; } },

            #endregion

            #region C4b

            { typeof(C4b), c => { var v = default(C4b); c.CodeC4b(ref v); return v; } },
            { typeof(C4b[]), c => { var v = default(C4b[]); c.CodeC4bArray(ref v); return v; } },
            { typeof(List<C4b>), c => { var v = default(List<C4b>); c.CodeList_of_C4b_(ref v); return v; } },

            { typeof(Vector<C4b>), c => { var v = default(Vector<C4b>); c.CodeVector_of_C4b_(ref v); return v; } },
            { typeof(Vector<C4b>[]), c => { var v = default(Vector<C4b>[]); c.CodeVector_of_C4b_Array(ref v); return v; } },
            { typeof(List<Vector<C4b>>), c => { var v = default(List<Vector<C4b>>); c.CodeList_of_Vector_of_C4b__(ref v); return v; } },

            { typeof(Matrix<C4b>), c => { var v = default(Matrix<C4b>); c.CodeMatrix_of_C4b_(ref v); return v; } },
            { typeof(Matrix<C4b>[]), c => { var v = default(Matrix<C4b>[]); c.CodeMatrix_of_C4b_Array(ref v); return v; } },
            { typeof(List<Matrix<C4b>>), c => { var v = default(List<Matrix<C4b>>); c.CodeList_of_Matrix_of_C4b__(ref v); return v; } },

            { typeof(Volume<C4b>), c => { var v = default(Volume<C4b>); c.CodeVolume_of_C4b_(ref v); return v; } },
            { typeof(Volume<C4b>[]), c => { var v = default(Volume<C4b>[]); c.CodeVolume_of_C4b_Array(ref v); return v; } },
            { typeof(List<Volume<C4b>>), c => { var v = default(List<Volume<C4b>>); c.CodeList_of_Volume_of_C4b__(ref v); return v; } },

            { typeof(Tensor<C4b>), c => { var v = default(Tensor<C4b>); c.CodeTensor_of_C4b_(ref v); return v; } },
            { typeof(Tensor<C4b>[]), c => { var v = default(Tensor<C4b>[]); c.CodeTensor_of_C4b_Array(ref v); return v; } },
            { typeof(List<Tensor<C4b>>), c => { var v = default(List<Tensor<C4b>>); c.CodeList_of_Tensor_of_C4b__(ref v); return v; } },

            #endregion

            #region C4us

            { typeof(C4us), c => { var v = default(C4us); c.CodeC4us(ref v); return v; } },
            { typeof(C4us[]), c => { var v = default(C4us[]); c.CodeC4usArray(ref v); return v; } },
            { typeof(List<C4us>), c => { var v = default(List<C4us>); c.CodeList_of_C4us_(ref v); return v; } },

            { typeof(Vector<C4us>), c => { var v = default(Vector<C4us>); c.CodeVector_of_C4us_(ref v); return v; } },
            { typeof(Vector<C4us>[]), c => { var v = default(Vector<C4us>[]); c.CodeVector_of_C4us_Array(ref v); return v; } },
            { typeof(List<Vector<C4us>>), c => { var v = default(List<Vector<C4us>>); c.CodeList_of_Vector_of_C4us__(ref v); return v; } },

            { typeof(Matrix<C4us>), c => { var v = default(Matrix<C4us>); c.CodeMatrix_of_C4us_(ref v); return v; } },
            { typeof(Matrix<C4us>[]), c => { var v = default(Matrix<C4us>[]); c.CodeMatrix_of_C4us_Array(ref v); return v; } },
            { typeof(List<Matrix<C4us>>), c => { var v = default(List<Matrix<C4us>>); c.CodeList_of_Matrix_of_C4us__(ref v); return v; } },

            { typeof(Volume<C4us>), c => { var v = default(Volume<C4us>); c.CodeVolume_of_C4us_(ref v); return v; } },
            { typeof(Volume<C4us>[]), c => { var v = default(Volume<C4us>[]); c.CodeVolume_of_C4us_Array(ref v); return v; } },
            { typeof(List<Volume<C4us>>), c => { var v = default(List<Volume<C4us>>); c.CodeList_of_Volume_of_C4us__(ref v); return v; } },

            { typeof(Tensor<C4us>), c => { var v = default(Tensor<C4us>); c.CodeTensor_of_C4us_(ref v); return v; } },
            { typeof(Tensor<C4us>[]), c => { var v = default(Tensor<C4us>[]); c.CodeTensor_of_C4us_Array(ref v); return v; } },
            { typeof(List<Tensor<C4us>>), c => { var v = default(List<Tensor<C4us>>); c.CodeList_of_Tensor_of_C4us__(ref v); return v; } },

            #endregion

            #region C4ui

            { typeof(C4ui), c => { var v = default(C4ui); c.CodeC4ui(ref v); return v; } },
            { typeof(C4ui[]), c => { var v = default(C4ui[]); c.CodeC4uiArray(ref v); return v; } },
            { typeof(List<C4ui>), c => { var v = default(List<C4ui>); c.CodeList_of_C4ui_(ref v); return v; } },

            { typeof(Vector<C4ui>), c => { var v = default(Vector<C4ui>); c.CodeVector_of_C4ui_(ref v); return v; } },
            { typeof(Vector<C4ui>[]), c => { var v = default(Vector<C4ui>[]); c.CodeVector_of_C4ui_Array(ref v); return v; } },
            { typeof(List<Vector<C4ui>>), c => { var v = default(List<Vector<C4ui>>); c.CodeList_of_Vector_of_C4ui__(ref v); return v; } },

            { typeof(Matrix<C4ui>), c => { var v = default(Matrix<C4ui>); c.CodeMatrix_of_C4ui_(ref v); return v; } },
            { typeof(Matrix<C4ui>[]), c => { var v = default(Matrix<C4ui>[]); c.CodeMatrix_of_C4ui_Array(ref v); return v; } },
            { typeof(List<Matrix<C4ui>>), c => { var v = default(List<Matrix<C4ui>>); c.CodeList_of_Matrix_of_C4ui__(ref v); return v; } },

            { typeof(Volume<C4ui>), c => { var v = default(Volume<C4ui>); c.CodeVolume_of_C4ui_(ref v); return v; } },
            { typeof(Volume<C4ui>[]), c => { var v = default(Volume<C4ui>[]); c.CodeVolume_of_C4ui_Array(ref v); return v; } },
            { typeof(List<Volume<C4ui>>), c => { var v = default(List<Volume<C4ui>>); c.CodeList_of_Volume_of_C4ui__(ref v); return v; } },

            { typeof(Tensor<C4ui>), c => { var v = default(Tensor<C4ui>); c.CodeTensor_of_C4ui_(ref v); return v; } },
            { typeof(Tensor<C4ui>[]), c => { var v = default(Tensor<C4ui>[]); c.CodeTensor_of_C4ui_Array(ref v); return v; } },
            { typeof(List<Tensor<C4ui>>), c => { var v = default(List<Tensor<C4ui>>); c.CodeList_of_Tensor_of_C4ui__(ref v); return v; } },

            #endregion

            #region C4f

            { typeof(C4f), c => { var v = default(C4f); c.CodeC4f(ref v); return v; } },
            { typeof(C4f[]), c => { var v = default(C4f[]); c.CodeC4fArray(ref v); return v; } },
            { typeof(List<C4f>), c => { var v = default(List<C4f>); c.CodeList_of_C4f_(ref v); return v; } },

            { typeof(Vector<C4f>), c => { var v = default(Vector<C4f>); c.CodeVector_of_C4f_(ref v); return v; } },
            { typeof(Vector<C4f>[]), c => { var v = default(Vector<C4f>[]); c.CodeVector_of_C4f_Array(ref v); return v; } },
            { typeof(List<Vector<C4f>>), c => { var v = default(List<Vector<C4f>>); c.CodeList_of_Vector_of_C4f__(ref v); return v; } },

            { typeof(Matrix<C4f>), c => { var v = default(Matrix<C4f>); c.CodeMatrix_of_C4f_(ref v); return v; } },
            { typeof(Matrix<C4f>[]), c => { var v = default(Matrix<C4f>[]); c.CodeMatrix_of_C4f_Array(ref v); return v; } },
            { typeof(List<Matrix<C4f>>), c => { var v = default(List<Matrix<C4f>>); c.CodeList_of_Matrix_of_C4f__(ref v); return v; } },

            { typeof(Volume<C4f>), c => { var v = default(Volume<C4f>); c.CodeVolume_of_C4f_(ref v); return v; } },
            { typeof(Volume<C4f>[]), c => { var v = default(Volume<C4f>[]); c.CodeVolume_of_C4f_Array(ref v); return v; } },
            { typeof(List<Volume<C4f>>), c => { var v = default(List<Volume<C4f>>); c.CodeList_of_Volume_of_C4f__(ref v); return v; } },

            { typeof(Tensor<C4f>), c => { var v = default(Tensor<C4f>); c.CodeTensor_of_C4f_(ref v); return v; } },
            { typeof(Tensor<C4f>[]), c => { var v = default(Tensor<C4f>[]); c.CodeTensor_of_C4f_Array(ref v); return v; } },
            { typeof(List<Tensor<C4f>>), c => { var v = default(List<Tensor<C4f>>); c.CodeList_of_Tensor_of_C4f__(ref v); return v; } },

            #endregion

            #region C4d

            { typeof(C4d), c => { var v = default(C4d); c.CodeC4d(ref v); return v; } },
            { typeof(C4d[]), c => { var v = default(C4d[]); c.CodeC4dArray(ref v); return v; } },
            { typeof(List<C4d>), c => { var v = default(List<C4d>); c.CodeList_of_C4d_(ref v); return v; } },

            { typeof(Vector<C4d>), c => { var v = default(Vector<C4d>); c.CodeVector_of_C4d_(ref v); return v; } },
            { typeof(Vector<C4d>[]), c => { var v = default(Vector<C4d>[]); c.CodeVector_of_C4d_Array(ref v); return v; } },
            { typeof(List<Vector<C4d>>), c => { var v = default(List<Vector<C4d>>); c.CodeList_of_Vector_of_C4d__(ref v); return v; } },

            { typeof(Matrix<C4d>), c => { var v = default(Matrix<C4d>); c.CodeMatrix_of_C4d_(ref v); return v; } },
            { typeof(Matrix<C4d>[]), c => { var v = default(Matrix<C4d>[]); c.CodeMatrix_of_C4d_Array(ref v); return v; } },
            { typeof(List<Matrix<C4d>>), c => { var v = default(List<Matrix<C4d>>); c.CodeList_of_Matrix_of_C4d__(ref v); return v; } },

            { typeof(Volume<C4d>), c => { var v = default(Volume<C4d>); c.CodeVolume_of_C4d_(ref v); return v; } },
            { typeof(Volume<C4d>[]), c => { var v = default(Volume<C4d>[]); c.CodeVolume_of_C4d_Array(ref v); return v; } },
            { typeof(List<Volume<C4d>>), c => { var v = default(List<Volume<C4d>>); c.CodeList_of_Volume_of_C4d__(ref v); return v; } },

            { typeof(Tensor<C4d>), c => { var v = default(Tensor<C4d>); c.CodeTensor_of_C4d_(ref v); return v; } },
            { typeof(Tensor<C4d>[]), c => { var v = default(Tensor<C4d>[]); c.CodeTensor_of_C4d_Array(ref v); return v; } },
            { typeof(List<Tensor<C4d>>), c => { var v = default(List<Tensor<C4d>>); c.CodeList_of_Tensor_of_C4d__(ref v); return v; } },

            #endregion

            #region Range1b

            { typeof(Range1b), c => { var v = default(Range1b); c.CodeRange1b(ref v); return v; } },
            { typeof(Range1b[]), c => { var v = default(Range1b[]); c.CodeRange1bArray(ref v); return v; } },
            { typeof(List<Range1b>), c => { var v = default(List<Range1b>); c.CodeList_of_Range1b_(ref v); return v; } },

            { typeof(Vector<Range1b>), c => { var v = default(Vector<Range1b>); c.CodeVector_of_Range1b_(ref v); return v; } },
            { typeof(Vector<Range1b>[]), c => { var v = default(Vector<Range1b>[]); c.CodeVector_of_Range1b_Array(ref v); return v; } },
            { typeof(List<Vector<Range1b>>), c => { var v = default(List<Vector<Range1b>>); c.CodeList_of_Vector_of_Range1b__(ref v); return v; } },

            { typeof(Matrix<Range1b>), c => { var v = default(Matrix<Range1b>); c.CodeMatrix_of_Range1b_(ref v); return v; } },
            { typeof(Matrix<Range1b>[]), c => { var v = default(Matrix<Range1b>[]); c.CodeMatrix_of_Range1b_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1b>>), c => { var v = default(List<Matrix<Range1b>>); c.CodeList_of_Matrix_of_Range1b__(ref v); return v; } },

            { typeof(Volume<Range1b>), c => { var v = default(Volume<Range1b>); c.CodeVolume_of_Range1b_(ref v); return v; } },
            { typeof(Volume<Range1b>[]), c => { var v = default(Volume<Range1b>[]); c.CodeVolume_of_Range1b_Array(ref v); return v; } },
            { typeof(List<Volume<Range1b>>), c => { var v = default(List<Volume<Range1b>>); c.CodeList_of_Volume_of_Range1b__(ref v); return v; } },

            { typeof(Tensor<Range1b>), c => { var v = default(Tensor<Range1b>); c.CodeTensor_of_Range1b_(ref v); return v; } },
            { typeof(Tensor<Range1b>[]), c => { var v = default(Tensor<Range1b>[]); c.CodeTensor_of_Range1b_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1b>>), c => { var v = default(List<Tensor<Range1b>>); c.CodeList_of_Tensor_of_Range1b__(ref v); return v; } },

            #endregion

            #region Range1sb

            { typeof(Range1sb), c => { var v = default(Range1sb); c.CodeRange1sb(ref v); return v; } },
            { typeof(Range1sb[]), c => { var v = default(Range1sb[]); c.CodeRange1sbArray(ref v); return v; } },
            { typeof(List<Range1sb>), c => { var v = default(List<Range1sb>); c.CodeList_of_Range1sb_(ref v); return v; } },

            { typeof(Vector<Range1sb>), c => { var v = default(Vector<Range1sb>); c.CodeVector_of_Range1sb_(ref v); return v; } },
            { typeof(Vector<Range1sb>[]), c => { var v = default(Vector<Range1sb>[]); c.CodeVector_of_Range1sb_Array(ref v); return v; } },
            { typeof(List<Vector<Range1sb>>), c => { var v = default(List<Vector<Range1sb>>); c.CodeList_of_Vector_of_Range1sb__(ref v); return v; } },

            { typeof(Matrix<Range1sb>), c => { var v = default(Matrix<Range1sb>); c.CodeMatrix_of_Range1sb_(ref v); return v; } },
            { typeof(Matrix<Range1sb>[]), c => { var v = default(Matrix<Range1sb>[]); c.CodeMatrix_of_Range1sb_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1sb>>), c => { var v = default(List<Matrix<Range1sb>>); c.CodeList_of_Matrix_of_Range1sb__(ref v); return v; } },

            { typeof(Volume<Range1sb>), c => { var v = default(Volume<Range1sb>); c.CodeVolume_of_Range1sb_(ref v); return v; } },
            { typeof(Volume<Range1sb>[]), c => { var v = default(Volume<Range1sb>[]); c.CodeVolume_of_Range1sb_Array(ref v); return v; } },
            { typeof(List<Volume<Range1sb>>), c => { var v = default(List<Volume<Range1sb>>); c.CodeList_of_Volume_of_Range1sb__(ref v); return v; } },

            { typeof(Tensor<Range1sb>), c => { var v = default(Tensor<Range1sb>); c.CodeTensor_of_Range1sb_(ref v); return v; } },
            { typeof(Tensor<Range1sb>[]), c => { var v = default(Tensor<Range1sb>[]); c.CodeTensor_of_Range1sb_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1sb>>), c => { var v = default(List<Tensor<Range1sb>>); c.CodeList_of_Tensor_of_Range1sb__(ref v); return v; } },

            #endregion

            #region Range1s

            { typeof(Range1s), c => { var v = default(Range1s); c.CodeRange1s(ref v); return v; } },
            { typeof(Range1s[]), c => { var v = default(Range1s[]); c.CodeRange1sArray(ref v); return v; } },
            { typeof(List<Range1s>), c => { var v = default(List<Range1s>); c.CodeList_of_Range1s_(ref v); return v; } },

            { typeof(Vector<Range1s>), c => { var v = default(Vector<Range1s>); c.CodeVector_of_Range1s_(ref v); return v; } },
            { typeof(Vector<Range1s>[]), c => { var v = default(Vector<Range1s>[]); c.CodeVector_of_Range1s_Array(ref v); return v; } },
            { typeof(List<Vector<Range1s>>), c => { var v = default(List<Vector<Range1s>>); c.CodeList_of_Vector_of_Range1s__(ref v); return v; } },

            { typeof(Matrix<Range1s>), c => { var v = default(Matrix<Range1s>); c.CodeMatrix_of_Range1s_(ref v); return v; } },
            { typeof(Matrix<Range1s>[]), c => { var v = default(Matrix<Range1s>[]); c.CodeMatrix_of_Range1s_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1s>>), c => { var v = default(List<Matrix<Range1s>>); c.CodeList_of_Matrix_of_Range1s__(ref v); return v; } },

            { typeof(Volume<Range1s>), c => { var v = default(Volume<Range1s>); c.CodeVolume_of_Range1s_(ref v); return v; } },
            { typeof(Volume<Range1s>[]), c => { var v = default(Volume<Range1s>[]); c.CodeVolume_of_Range1s_Array(ref v); return v; } },
            { typeof(List<Volume<Range1s>>), c => { var v = default(List<Volume<Range1s>>); c.CodeList_of_Volume_of_Range1s__(ref v); return v; } },

            { typeof(Tensor<Range1s>), c => { var v = default(Tensor<Range1s>); c.CodeTensor_of_Range1s_(ref v); return v; } },
            { typeof(Tensor<Range1s>[]), c => { var v = default(Tensor<Range1s>[]); c.CodeTensor_of_Range1s_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1s>>), c => { var v = default(List<Tensor<Range1s>>); c.CodeList_of_Tensor_of_Range1s__(ref v); return v; } },

            #endregion

            #region Range1us

            { typeof(Range1us), c => { var v = default(Range1us); c.CodeRange1us(ref v); return v; } },
            { typeof(Range1us[]), c => { var v = default(Range1us[]); c.CodeRange1usArray(ref v); return v; } },
            { typeof(List<Range1us>), c => { var v = default(List<Range1us>); c.CodeList_of_Range1us_(ref v); return v; } },

            { typeof(Vector<Range1us>), c => { var v = default(Vector<Range1us>); c.CodeVector_of_Range1us_(ref v); return v; } },
            { typeof(Vector<Range1us>[]), c => { var v = default(Vector<Range1us>[]); c.CodeVector_of_Range1us_Array(ref v); return v; } },
            { typeof(List<Vector<Range1us>>), c => { var v = default(List<Vector<Range1us>>); c.CodeList_of_Vector_of_Range1us__(ref v); return v; } },

            { typeof(Matrix<Range1us>), c => { var v = default(Matrix<Range1us>); c.CodeMatrix_of_Range1us_(ref v); return v; } },
            { typeof(Matrix<Range1us>[]), c => { var v = default(Matrix<Range1us>[]); c.CodeMatrix_of_Range1us_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1us>>), c => { var v = default(List<Matrix<Range1us>>); c.CodeList_of_Matrix_of_Range1us__(ref v); return v; } },

            { typeof(Volume<Range1us>), c => { var v = default(Volume<Range1us>); c.CodeVolume_of_Range1us_(ref v); return v; } },
            { typeof(Volume<Range1us>[]), c => { var v = default(Volume<Range1us>[]); c.CodeVolume_of_Range1us_Array(ref v); return v; } },
            { typeof(List<Volume<Range1us>>), c => { var v = default(List<Volume<Range1us>>); c.CodeList_of_Volume_of_Range1us__(ref v); return v; } },

            { typeof(Tensor<Range1us>), c => { var v = default(Tensor<Range1us>); c.CodeTensor_of_Range1us_(ref v); return v; } },
            { typeof(Tensor<Range1us>[]), c => { var v = default(Tensor<Range1us>[]); c.CodeTensor_of_Range1us_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1us>>), c => { var v = default(List<Tensor<Range1us>>); c.CodeList_of_Tensor_of_Range1us__(ref v); return v; } },

            #endregion

            #region Range1i

            { typeof(Range1i), c => { var v = default(Range1i); c.CodeRange1i(ref v); return v; } },
            { typeof(Range1i[]), c => { var v = default(Range1i[]); c.CodeRange1iArray(ref v); return v; } },
            { typeof(List<Range1i>), c => { var v = default(List<Range1i>); c.CodeList_of_Range1i_(ref v); return v; } },

            { typeof(Vector<Range1i>), c => { var v = default(Vector<Range1i>); c.CodeVector_of_Range1i_(ref v); return v; } },
            { typeof(Vector<Range1i>[]), c => { var v = default(Vector<Range1i>[]); c.CodeVector_of_Range1i_Array(ref v); return v; } },
            { typeof(List<Vector<Range1i>>), c => { var v = default(List<Vector<Range1i>>); c.CodeList_of_Vector_of_Range1i__(ref v); return v; } },

            { typeof(Matrix<Range1i>), c => { var v = default(Matrix<Range1i>); c.CodeMatrix_of_Range1i_(ref v); return v; } },
            { typeof(Matrix<Range1i>[]), c => { var v = default(Matrix<Range1i>[]); c.CodeMatrix_of_Range1i_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1i>>), c => { var v = default(List<Matrix<Range1i>>); c.CodeList_of_Matrix_of_Range1i__(ref v); return v; } },

            { typeof(Volume<Range1i>), c => { var v = default(Volume<Range1i>); c.CodeVolume_of_Range1i_(ref v); return v; } },
            { typeof(Volume<Range1i>[]), c => { var v = default(Volume<Range1i>[]); c.CodeVolume_of_Range1i_Array(ref v); return v; } },
            { typeof(List<Volume<Range1i>>), c => { var v = default(List<Volume<Range1i>>); c.CodeList_of_Volume_of_Range1i__(ref v); return v; } },

            { typeof(Tensor<Range1i>), c => { var v = default(Tensor<Range1i>); c.CodeTensor_of_Range1i_(ref v); return v; } },
            { typeof(Tensor<Range1i>[]), c => { var v = default(Tensor<Range1i>[]); c.CodeTensor_of_Range1i_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1i>>), c => { var v = default(List<Tensor<Range1i>>); c.CodeList_of_Tensor_of_Range1i__(ref v); return v; } },

            #endregion

            #region Range1ui

            { typeof(Range1ui), c => { var v = default(Range1ui); c.CodeRange1ui(ref v); return v; } },
            { typeof(Range1ui[]), c => { var v = default(Range1ui[]); c.CodeRange1uiArray(ref v); return v; } },
            { typeof(List<Range1ui>), c => { var v = default(List<Range1ui>); c.CodeList_of_Range1ui_(ref v); return v; } },

            { typeof(Vector<Range1ui>), c => { var v = default(Vector<Range1ui>); c.CodeVector_of_Range1ui_(ref v); return v; } },
            { typeof(Vector<Range1ui>[]), c => { var v = default(Vector<Range1ui>[]); c.CodeVector_of_Range1ui_Array(ref v); return v; } },
            { typeof(List<Vector<Range1ui>>), c => { var v = default(List<Vector<Range1ui>>); c.CodeList_of_Vector_of_Range1ui__(ref v); return v; } },

            { typeof(Matrix<Range1ui>), c => { var v = default(Matrix<Range1ui>); c.CodeMatrix_of_Range1ui_(ref v); return v; } },
            { typeof(Matrix<Range1ui>[]), c => { var v = default(Matrix<Range1ui>[]); c.CodeMatrix_of_Range1ui_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1ui>>), c => { var v = default(List<Matrix<Range1ui>>); c.CodeList_of_Matrix_of_Range1ui__(ref v); return v; } },

            { typeof(Volume<Range1ui>), c => { var v = default(Volume<Range1ui>); c.CodeVolume_of_Range1ui_(ref v); return v; } },
            { typeof(Volume<Range1ui>[]), c => { var v = default(Volume<Range1ui>[]); c.CodeVolume_of_Range1ui_Array(ref v); return v; } },
            { typeof(List<Volume<Range1ui>>), c => { var v = default(List<Volume<Range1ui>>); c.CodeList_of_Volume_of_Range1ui__(ref v); return v; } },

            { typeof(Tensor<Range1ui>), c => { var v = default(Tensor<Range1ui>); c.CodeTensor_of_Range1ui_(ref v); return v; } },
            { typeof(Tensor<Range1ui>[]), c => { var v = default(Tensor<Range1ui>[]); c.CodeTensor_of_Range1ui_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1ui>>), c => { var v = default(List<Tensor<Range1ui>>); c.CodeList_of_Tensor_of_Range1ui__(ref v); return v; } },

            #endregion

            #region Range1l

            { typeof(Range1l), c => { var v = default(Range1l); c.CodeRange1l(ref v); return v; } },
            { typeof(Range1l[]), c => { var v = default(Range1l[]); c.CodeRange1lArray(ref v); return v; } },
            { typeof(List<Range1l>), c => { var v = default(List<Range1l>); c.CodeList_of_Range1l_(ref v); return v; } },

            { typeof(Vector<Range1l>), c => { var v = default(Vector<Range1l>); c.CodeVector_of_Range1l_(ref v); return v; } },
            { typeof(Vector<Range1l>[]), c => { var v = default(Vector<Range1l>[]); c.CodeVector_of_Range1l_Array(ref v); return v; } },
            { typeof(List<Vector<Range1l>>), c => { var v = default(List<Vector<Range1l>>); c.CodeList_of_Vector_of_Range1l__(ref v); return v; } },

            { typeof(Matrix<Range1l>), c => { var v = default(Matrix<Range1l>); c.CodeMatrix_of_Range1l_(ref v); return v; } },
            { typeof(Matrix<Range1l>[]), c => { var v = default(Matrix<Range1l>[]); c.CodeMatrix_of_Range1l_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1l>>), c => { var v = default(List<Matrix<Range1l>>); c.CodeList_of_Matrix_of_Range1l__(ref v); return v; } },

            { typeof(Volume<Range1l>), c => { var v = default(Volume<Range1l>); c.CodeVolume_of_Range1l_(ref v); return v; } },
            { typeof(Volume<Range1l>[]), c => { var v = default(Volume<Range1l>[]); c.CodeVolume_of_Range1l_Array(ref v); return v; } },
            { typeof(List<Volume<Range1l>>), c => { var v = default(List<Volume<Range1l>>); c.CodeList_of_Volume_of_Range1l__(ref v); return v; } },

            { typeof(Tensor<Range1l>), c => { var v = default(Tensor<Range1l>); c.CodeTensor_of_Range1l_(ref v); return v; } },
            { typeof(Tensor<Range1l>[]), c => { var v = default(Tensor<Range1l>[]); c.CodeTensor_of_Range1l_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1l>>), c => { var v = default(List<Tensor<Range1l>>); c.CodeList_of_Tensor_of_Range1l__(ref v); return v; } },

            #endregion

            #region Range1ul

            { typeof(Range1ul), c => { var v = default(Range1ul); c.CodeRange1ul(ref v); return v; } },
            { typeof(Range1ul[]), c => { var v = default(Range1ul[]); c.CodeRange1ulArray(ref v); return v; } },
            { typeof(List<Range1ul>), c => { var v = default(List<Range1ul>); c.CodeList_of_Range1ul_(ref v); return v; } },

            { typeof(Vector<Range1ul>), c => { var v = default(Vector<Range1ul>); c.CodeVector_of_Range1ul_(ref v); return v; } },
            { typeof(Vector<Range1ul>[]), c => { var v = default(Vector<Range1ul>[]); c.CodeVector_of_Range1ul_Array(ref v); return v; } },
            { typeof(List<Vector<Range1ul>>), c => { var v = default(List<Vector<Range1ul>>); c.CodeList_of_Vector_of_Range1ul__(ref v); return v; } },

            { typeof(Matrix<Range1ul>), c => { var v = default(Matrix<Range1ul>); c.CodeMatrix_of_Range1ul_(ref v); return v; } },
            { typeof(Matrix<Range1ul>[]), c => { var v = default(Matrix<Range1ul>[]); c.CodeMatrix_of_Range1ul_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1ul>>), c => { var v = default(List<Matrix<Range1ul>>); c.CodeList_of_Matrix_of_Range1ul__(ref v); return v; } },

            { typeof(Volume<Range1ul>), c => { var v = default(Volume<Range1ul>); c.CodeVolume_of_Range1ul_(ref v); return v; } },
            { typeof(Volume<Range1ul>[]), c => { var v = default(Volume<Range1ul>[]); c.CodeVolume_of_Range1ul_Array(ref v); return v; } },
            { typeof(List<Volume<Range1ul>>), c => { var v = default(List<Volume<Range1ul>>); c.CodeList_of_Volume_of_Range1ul__(ref v); return v; } },

            { typeof(Tensor<Range1ul>), c => { var v = default(Tensor<Range1ul>); c.CodeTensor_of_Range1ul_(ref v); return v; } },
            { typeof(Tensor<Range1ul>[]), c => { var v = default(Tensor<Range1ul>[]); c.CodeTensor_of_Range1ul_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1ul>>), c => { var v = default(List<Tensor<Range1ul>>); c.CodeList_of_Tensor_of_Range1ul__(ref v); return v; } },

            #endregion

            #region Range1f

            { typeof(Range1f), c => { var v = default(Range1f); c.CodeRange1f(ref v); return v; } },
            { typeof(Range1f[]), c => { var v = default(Range1f[]); c.CodeRange1fArray(ref v); return v; } },
            { typeof(List<Range1f>), c => { var v = default(List<Range1f>); c.CodeList_of_Range1f_(ref v); return v; } },

            { typeof(Vector<Range1f>), c => { var v = default(Vector<Range1f>); c.CodeVector_of_Range1f_(ref v); return v; } },
            { typeof(Vector<Range1f>[]), c => { var v = default(Vector<Range1f>[]); c.CodeVector_of_Range1f_Array(ref v); return v; } },
            { typeof(List<Vector<Range1f>>), c => { var v = default(List<Vector<Range1f>>); c.CodeList_of_Vector_of_Range1f__(ref v); return v; } },

            { typeof(Matrix<Range1f>), c => { var v = default(Matrix<Range1f>); c.CodeMatrix_of_Range1f_(ref v); return v; } },
            { typeof(Matrix<Range1f>[]), c => { var v = default(Matrix<Range1f>[]); c.CodeMatrix_of_Range1f_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1f>>), c => { var v = default(List<Matrix<Range1f>>); c.CodeList_of_Matrix_of_Range1f__(ref v); return v; } },

            { typeof(Volume<Range1f>), c => { var v = default(Volume<Range1f>); c.CodeVolume_of_Range1f_(ref v); return v; } },
            { typeof(Volume<Range1f>[]), c => { var v = default(Volume<Range1f>[]); c.CodeVolume_of_Range1f_Array(ref v); return v; } },
            { typeof(List<Volume<Range1f>>), c => { var v = default(List<Volume<Range1f>>); c.CodeList_of_Volume_of_Range1f__(ref v); return v; } },

            { typeof(Tensor<Range1f>), c => { var v = default(Tensor<Range1f>); c.CodeTensor_of_Range1f_(ref v); return v; } },
            { typeof(Tensor<Range1f>[]), c => { var v = default(Tensor<Range1f>[]); c.CodeTensor_of_Range1f_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1f>>), c => { var v = default(List<Tensor<Range1f>>); c.CodeList_of_Tensor_of_Range1f__(ref v); return v; } },

            #endregion

            #region Range1d

            { typeof(Range1d), c => { var v = default(Range1d); c.CodeRange1d(ref v); return v; } },
            { typeof(Range1d[]), c => { var v = default(Range1d[]); c.CodeRange1dArray(ref v); return v; } },
            { typeof(List<Range1d>), c => { var v = default(List<Range1d>); c.CodeList_of_Range1d_(ref v); return v; } },

            { typeof(Vector<Range1d>), c => { var v = default(Vector<Range1d>); c.CodeVector_of_Range1d_(ref v); return v; } },
            { typeof(Vector<Range1d>[]), c => { var v = default(Vector<Range1d>[]); c.CodeVector_of_Range1d_Array(ref v); return v; } },
            { typeof(List<Vector<Range1d>>), c => { var v = default(List<Vector<Range1d>>); c.CodeList_of_Vector_of_Range1d__(ref v); return v; } },

            { typeof(Matrix<Range1d>), c => { var v = default(Matrix<Range1d>); c.CodeMatrix_of_Range1d_(ref v); return v; } },
            { typeof(Matrix<Range1d>[]), c => { var v = default(Matrix<Range1d>[]); c.CodeMatrix_of_Range1d_Array(ref v); return v; } },
            { typeof(List<Matrix<Range1d>>), c => { var v = default(List<Matrix<Range1d>>); c.CodeList_of_Matrix_of_Range1d__(ref v); return v; } },

            { typeof(Volume<Range1d>), c => { var v = default(Volume<Range1d>); c.CodeVolume_of_Range1d_(ref v); return v; } },
            { typeof(Volume<Range1d>[]), c => { var v = default(Volume<Range1d>[]); c.CodeVolume_of_Range1d_Array(ref v); return v; } },
            { typeof(List<Volume<Range1d>>), c => { var v = default(List<Volume<Range1d>>); c.CodeList_of_Volume_of_Range1d__(ref v); return v; } },

            { typeof(Tensor<Range1d>), c => { var v = default(Tensor<Range1d>); c.CodeTensor_of_Range1d_(ref v); return v; } },
            { typeof(Tensor<Range1d>[]), c => { var v = default(Tensor<Range1d>[]); c.CodeTensor_of_Range1d_Array(ref v); return v; } },
            { typeof(List<Tensor<Range1d>>), c => { var v = default(List<Tensor<Range1d>>); c.CodeList_of_Tensor_of_Range1d__(ref v); return v; } },

            #endregion

            #region Box2i

            { typeof(Box2i), c => { var v = default(Box2i); c.CodeBox2i(ref v); return v; } },
            { typeof(Box2i[]), c => { var v = default(Box2i[]); c.CodeBox2iArray(ref v); return v; } },
            { typeof(List<Box2i>), c => { var v = default(List<Box2i>); c.CodeList_of_Box2i_(ref v); return v; } },

            { typeof(Vector<Box2i>), c => { var v = default(Vector<Box2i>); c.CodeVector_of_Box2i_(ref v); return v; } },
            { typeof(Vector<Box2i>[]), c => { var v = default(Vector<Box2i>[]); c.CodeVector_of_Box2i_Array(ref v); return v; } },
            { typeof(List<Vector<Box2i>>), c => { var v = default(List<Vector<Box2i>>); c.CodeList_of_Vector_of_Box2i__(ref v); return v; } },

            { typeof(Matrix<Box2i>), c => { var v = default(Matrix<Box2i>); c.CodeMatrix_of_Box2i_(ref v); return v; } },
            { typeof(Matrix<Box2i>[]), c => { var v = default(Matrix<Box2i>[]); c.CodeMatrix_of_Box2i_Array(ref v); return v; } },
            { typeof(List<Matrix<Box2i>>), c => { var v = default(List<Matrix<Box2i>>); c.CodeList_of_Matrix_of_Box2i__(ref v); return v; } },

            { typeof(Volume<Box2i>), c => { var v = default(Volume<Box2i>); c.CodeVolume_of_Box2i_(ref v); return v; } },
            { typeof(Volume<Box2i>[]), c => { var v = default(Volume<Box2i>[]); c.CodeVolume_of_Box2i_Array(ref v); return v; } },
            { typeof(List<Volume<Box2i>>), c => { var v = default(List<Volume<Box2i>>); c.CodeList_of_Volume_of_Box2i__(ref v); return v; } },

            { typeof(Tensor<Box2i>), c => { var v = default(Tensor<Box2i>); c.CodeTensor_of_Box2i_(ref v); return v; } },
            { typeof(Tensor<Box2i>[]), c => { var v = default(Tensor<Box2i>[]); c.CodeTensor_of_Box2i_Array(ref v); return v; } },
            { typeof(List<Tensor<Box2i>>), c => { var v = default(List<Tensor<Box2i>>); c.CodeList_of_Tensor_of_Box2i__(ref v); return v; } },

            #endregion

            #region Box2l

            { typeof(Box2l), c => { var v = default(Box2l); c.CodeBox2l(ref v); return v; } },
            { typeof(Box2l[]), c => { var v = default(Box2l[]); c.CodeBox2lArray(ref v); return v; } },
            { typeof(List<Box2l>), c => { var v = default(List<Box2l>); c.CodeList_of_Box2l_(ref v); return v; } },

            { typeof(Vector<Box2l>), c => { var v = default(Vector<Box2l>); c.CodeVector_of_Box2l_(ref v); return v; } },
            { typeof(Vector<Box2l>[]), c => { var v = default(Vector<Box2l>[]); c.CodeVector_of_Box2l_Array(ref v); return v; } },
            { typeof(List<Vector<Box2l>>), c => { var v = default(List<Vector<Box2l>>); c.CodeList_of_Vector_of_Box2l__(ref v); return v; } },

            { typeof(Matrix<Box2l>), c => { var v = default(Matrix<Box2l>); c.CodeMatrix_of_Box2l_(ref v); return v; } },
            { typeof(Matrix<Box2l>[]), c => { var v = default(Matrix<Box2l>[]); c.CodeMatrix_of_Box2l_Array(ref v); return v; } },
            { typeof(List<Matrix<Box2l>>), c => { var v = default(List<Matrix<Box2l>>); c.CodeList_of_Matrix_of_Box2l__(ref v); return v; } },

            { typeof(Volume<Box2l>), c => { var v = default(Volume<Box2l>); c.CodeVolume_of_Box2l_(ref v); return v; } },
            { typeof(Volume<Box2l>[]), c => { var v = default(Volume<Box2l>[]); c.CodeVolume_of_Box2l_Array(ref v); return v; } },
            { typeof(List<Volume<Box2l>>), c => { var v = default(List<Volume<Box2l>>); c.CodeList_of_Volume_of_Box2l__(ref v); return v; } },

            { typeof(Tensor<Box2l>), c => { var v = default(Tensor<Box2l>); c.CodeTensor_of_Box2l_(ref v); return v; } },
            { typeof(Tensor<Box2l>[]), c => { var v = default(Tensor<Box2l>[]); c.CodeTensor_of_Box2l_Array(ref v); return v; } },
            { typeof(List<Tensor<Box2l>>), c => { var v = default(List<Tensor<Box2l>>); c.CodeList_of_Tensor_of_Box2l__(ref v); return v; } },

            #endregion

            #region Box2f

            { typeof(Box2f), c => { var v = default(Box2f); c.CodeBox2f(ref v); return v; } },
            { typeof(Box2f[]), c => { var v = default(Box2f[]); c.CodeBox2fArray(ref v); return v; } },
            { typeof(List<Box2f>), c => { var v = default(List<Box2f>); c.CodeList_of_Box2f_(ref v); return v; } },

            { typeof(Vector<Box2f>), c => { var v = default(Vector<Box2f>); c.CodeVector_of_Box2f_(ref v); return v; } },
            { typeof(Vector<Box2f>[]), c => { var v = default(Vector<Box2f>[]); c.CodeVector_of_Box2f_Array(ref v); return v; } },
            { typeof(List<Vector<Box2f>>), c => { var v = default(List<Vector<Box2f>>); c.CodeList_of_Vector_of_Box2f__(ref v); return v; } },

            { typeof(Matrix<Box2f>), c => { var v = default(Matrix<Box2f>); c.CodeMatrix_of_Box2f_(ref v); return v; } },
            { typeof(Matrix<Box2f>[]), c => { var v = default(Matrix<Box2f>[]); c.CodeMatrix_of_Box2f_Array(ref v); return v; } },
            { typeof(List<Matrix<Box2f>>), c => { var v = default(List<Matrix<Box2f>>); c.CodeList_of_Matrix_of_Box2f__(ref v); return v; } },

            { typeof(Volume<Box2f>), c => { var v = default(Volume<Box2f>); c.CodeVolume_of_Box2f_(ref v); return v; } },
            { typeof(Volume<Box2f>[]), c => { var v = default(Volume<Box2f>[]); c.CodeVolume_of_Box2f_Array(ref v); return v; } },
            { typeof(List<Volume<Box2f>>), c => { var v = default(List<Volume<Box2f>>); c.CodeList_of_Volume_of_Box2f__(ref v); return v; } },

            { typeof(Tensor<Box2f>), c => { var v = default(Tensor<Box2f>); c.CodeTensor_of_Box2f_(ref v); return v; } },
            { typeof(Tensor<Box2f>[]), c => { var v = default(Tensor<Box2f>[]); c.CodeTensor_of_Box2f_Array(ref v); return v; } },
            { typeof(List<Tensor<Box2f>>), c => { var v = default(List<Tensor<Box2f>>); c.CodeList_of_Tensor_of_Box2f__(ref v); return v; } },

            #endregion

            #region Box2d

            { typeof(Box2d), c => { var v = default(Box2d); c.CodeBox2d(ref v); return v; } },
            { typeof(Box2d[]), c => { var v = default(Box2d[]); c.CodeBox2dArray(ref v); return v; } },
            { typeof(List<Box2d>), c => { var v = default(List<Box2d>); c.CodeList_of_Box2d_(ref v); return v; } },

            { typeof(Vector<Box2d>), c => { var v = default(Vector<Box2d>); c.CodeVector_of_Box2d_(ref v); return v; } },
            { typeof(Vector<Box2d>[]), c => { var v = default(Vector<Box2d>[]); c.CodeVector_of_Box2d_Array(ref v); return v; } },
            { typeof(List<Vector<Box2d>>), c => { var v = default(List<Vector<Box2d>>); c.CodeList_of_Vector_of_Box2d__(ref v); return v; } },

            { typeof(Matrix<Box2d>), c => { var v = default(Matrix<Box2d>); c.CodeMatrix_of_Box2d_(ref v); return v; } },
            { typeof(Matrix<Box2d>[]), c => { var v = default(Matrix<Box2d>[]); c.CodeMatrix_of_Box2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Box2d>>), c => { var v = default(List<Matrix<Box2d>>); c.CodeList_of_Matrix_of_Box2d__(ref v); return v; } },

            { typeof(Volume<Box2d>), c => { var v = default(Volume<Box2d>); c.CodeVolume_of_Box2d_(ref v); return v; } },
            { typeof(Volume<Box2d>[]), c => { var v = default(Volume<Box2d>[]); c.CodeVolume_of_Box2d_Array(ref v); return v; } },
            { typeof(List<Volume<Box2d>>), c => { var v = default(List<Volume<Box2d>>); c.CodeList_of_Volume_of_Box2d__(ref v); return v; } },

            { typeof(Tensor<Box2d>), c => { var v = default(Tensor<Box2d>); c.CodeTensor_of_Box2d_(ref v); return v; } },
            { typeof(Tensor<Box2d>[]), c => { var v = default(Tensor<Box2d>[]); c.CodeTensor_of_Box2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Box2d>>), c => { var v = default(List<Tensor<Box2d>>); c.CodeList_of_Tensor_of_Box2d__(ref v); return v; } },

            #endregion

            #region Box3i

            { typeof(Box3i), c => { var v = default(Box3i); c.CodeBox3i(ref v); return v; } },
            { typeof(Box3i[]), c => { var v = default(Box3i[]); c.CodeBox3iArray(ref v); return v; } },
            { typeof(List<Box3i>), c => { var v = default(List<Box3i>); c.CodeList_of_Box3i_(ref v); return v; } },

            { typeof(Vector<Box3i>), c => { var v = default(Vector<Box3i>); c.CodeVector_of_Box3i_(ref v); return v; } },
            { typeof(Vector<Box3i>[]), c => { var v = default(Vector<Box3i>[]); c.CodeVector_of_Box3i_Array(ref v); return v; } },
            { typeof(List<Vector<Box3i>>), c => { var v = default(List<Vector<Box3i>>); c.CodeList_of_Vector_of_Box3i__(ref v); return v; } },

            { typeof(Matrix<Box3i>), c => { var v = default(Matrix<Box3i>); c.CodeMatrix_of_Box3i_(ref v); return v; } },
            { typeof(Matrix<Box3i>[]), c => { var v = default(Matrix<Box3i>[]); c.CodeMatrix_of_Box3i_Array(ref v); return v; } },
            { typeof(List<Matrix<Box3i>>), c => { var v = default(List<Matrix<Box3i>>); c.CodeList_of_Matrix_of_Box3i__(ref v); return v; } },

            { typeof(Volume<Box3i>), c => { var v = default(Volume<Box3i>); c.CodeVolume_of_Box3i_(ref v); return v; } },
            { typeof(Volume<Box3i>[]), c => { var v = default(Volume<Box3i>[]); c.CodeVolume_of_Box3i_Array(ref v); return v; } },
            { typeof(List<Volume<Box3i>>), c => { var v = default(List<Volume<Box3i>>); c.CodeList_of_Volume_of_Box3i__(ref v); return v; } },

            { typeof(Tensor<Box3i>), c => { var v = default(Tensor<Box3i>); c.CodeTensor_of_Box3i_(ref v); return v; } },
            { typeof(Tensor<Box3i>[]), c => { var v = default(Tensor<Box3i>[]); c.CodeTensor_of_Box3i_Array(ref v); return v; } },
            { typeof(List<Tensor<Box3i>>), c => { var v = default(List<Tensor<Box3i>>); c.CodeList_of_Tensor_of_Box3i__(ref v); return v; } },

            #endregion

            #region Box3l

            { typeof(Box3l), c => { var v = default(Box3l); c.CodeBox3l(ref v); return v; } },
            { typeof(Box3l[]), c => { var v = default(Box3l[]); c.CodeBox3lArray(ref v); return v; } },
            { typeof(List<Box3l>), c => { var v = default(List<Box3l>); c.CodeList_of_Box3l_(ref v); return v; } },

            { typeof(Vector<Box3l>), c => { var v = default(Vector<Box3l>); c.CodeVector_of_Box3l_(ref v); return v; } },
            { typeof(Vector<Box3l>[]), c => { var v = default(Vector<Box3l>[]); c.CodeVector_of_Box3l_Array(ref v); return v; } },
            { typeof(List<Vector<Box3l>>), c => { var v = default(List<Vector<Box3l>>); c.CodeList_of_Vector_of_Box3l__(ref v); return v; } },

            { typeof(Matrix<Box3l>), c => { var v = default(Matrix<Box3l>); c.CodeMatrix_of_Box3l_(ref v); return v; } },
            { typeof(Matrix<Box3l>[]), c => { var v = default(Matrix<Box3l>[]); c.CodeMatrix_of_Box3l_Array(ref v); return v; } },
            { typeof(List<Matrix<Box3l>>), c => { var v = default(List<Matrix<Box3l>>); c.CodeList_of_Matrix_of_Box3l__(ref v); return v; } },

            { typeof(Volume<Box3l>), c => { var v = default(Volume<Box3l>); c.CodeVolume_of_Box3l_(ref v); return v; } },
            { typeof(Volume<Box3l>[]), c => { var v = default(Volume<Box3l>[]); c.CodeVolume_of_Box3l_Array(ref v); return v; } },
            { typeof(List<Volume<Box3l>>), c => { var v = default(List<Volume<Box3l>>); c.CodeList_of_Volume_of_Box3l__(ref v); return v; } },

            { typeof(Tensor<Box3l>), c => { var v = default(Tensor<Box3l>); c.CodeTensor_of_Box3l_(ref v); return v; } },
            { typeof(Tensor<Box3l>[]), c => { var v = default(Tensor<Box3l>[]); c.CodeTensor_of_Box3l_Array(ref v); return v; } },
            { typeof(List<Tensor<Box3l>>), c => { var v = default(List<Tensor<Box3l>>); c.CodeList_of_Tensor_of_Box3l__(ref v); return v; } },

            #endregion

            #region Box3f

            { typeof(Box3f), c => { var v = default(Box3f); c.CodeBox3f(ref v); return v; } },
            { typeof(Box3f[]), c => { var v = default(Box3f[]); c.CodeBox3fArray(ref v); return v; } },
            { typeof(List<Box3f>), c => { var v = default(List<Box3f>); c.CodeList_of_Box3f_(ref v); return v; } },

            { typeof(Vector<Box3f>), c => { var v = default(Vector<Box3f>); c.CodeVector_of_Box3f_(ref v); return v; } },
            { typeof(Vector<Box3f>[]), c => { var v = default(Vector<Box3f>[]); c.CodeVector_of_Box3f_Array(ref v); return v; } },
            { typeof(List<Vector<Box3f>>), c => { var v = default(List<Vector<Box3f>>); c.CodeList_of_Vector_of_Box3f__(ref v); return v; } },

            { typeof(Matrix<Box3f>), c => { var v = default(Matrix<Box3f>); c.CodeMatrix_of_Box3f_(ref v); return v; } },
            { typeof(Matrix<Box3f>[]), c => { var v = default(Matrix<Box3f>[]); c.CodeMatrix_of_Box3f_Array(ref v); return v; } },
            { typeof(List<Matrix<Box3f>>), c => { var v = default(List<Matrix<Box3f>>); c.CodeList_of_Matrix_of_Box3f__(ref v); return v; } },

            { typeof(Volume<Box3f>), c => { var v = default(Volume<Box3f>); c.CodeVolume_of_Box3f_(ref v); return v; } },
            { typeof(Volume<Box3f>[]), c => { var v = default(Volume<Box3f>[]); c.CodeVolume_of_Box3f_Array(ref v); return v; } },
            { typeof(List<Volume<Box3f>>), c => { var v = default(List<Volume<Box3f>>); c.CodeList_of_Volume_of_Box3f__(ref v); return v; } },

            { typeof(Tensor<Box3f>), c => { var v = default(Tensor<Box3f>); c.CodeTensor_of_Box3f_(ref v); return v; } },
            { typeof(Tensor<Box3f>[]), c => { var v = default(Tensor<Box3f>[]); c.CodeTensor_of_Box3f_Array(ref v); return v; } },
            { typeof(List<Tensor<Box3f>>), c => { var v = default(List<Tensor<Box3f>>); c.CodeList_of_Tensor_of_Box3f__(ref v); return v; } },

            #endregion

            #region Box3d

            { typeof(Box3d), c => { var v = default(Box3d); c.CodeBox3d(ref v); return v; } },
            { typeof(Box3d[]), c => { var v = default(Box3d[]); c.CodeBox3dArray(ref v); return v; } },
            { typeof(List<Box3d>), c => { var v = default(List<Box3d>); c.CodeList_of_Box3d_(ref v); return v; } },

            { typeof(Vector<Box3d>), c => { var v = default(Vector<Box3d>); c.CodeVector_of_Box3d_(ref v); return v; } },
            { typeof(Vector<Box3d>[]), c => { var v = default(Vector<Box3d>[]); c.CodeVector_of_Box3d_Array(ref v); return v; } },
            { typeof(List<Vector<Box3d>>), c => { var v = default(List<Vector<Box3d>>); c.CodeList_of_Vector_of_Box3d__(ref v); return v; } },

            { typeof(Matrix<Box3d>), c => { var v = default(Matrix<Box3d>); c.CodeMatrix_of_Box3d_(ref v); return v; } },
            { typeof(Matrix<Box3d>[]), c => { var v = default(Matrix<Box3d>[]); c.CodeMatrix_of_Box3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Box3d>>), c => { var v = default(List<Matrix<Box3d>>); c.CodeList_of_Matrix_of_Box3d__(ref v); return v; } },

            { typeof(Volume<Box3d>), c => { var v = default(Volume<Box3d>); c.CodeVolume_of_Box3d_(ref v); return v; } },
            { typeof(Volume<Box3d>[]), c => { var v = default(Volume<Box3d>[]); c.CodeVolume_of_Box3d_Array(ref v); return v; } },
            { typeof(List<Volume<Box3d>>), c => { var v = default(List<Volume<Box3d>>); c.CodeList_of_Volume_of_Box3d__(ref v); return v; } },

            { typeof(Tensor<Box3d>), c => { var v = default(Tensor<Box3d>); c.CodeTensor_of_Box3d_(ref v); return v; } },
            { typeof(Tensor<Box3d>[]), c => { var v = default(Tensor<Box3d>[]); c.CodeTensor_of_Box3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Box3d>>), c => { var v = default(List<Tensor<Box3d>>); c.CodeList_of_Tensor_of_Box3d__(ref v); return v; } },

            #endregion

            #region Euclidean3f

            { typeof(Euclidean3f), c => { var v = default(Euclidean3f); c.CodeEuclidean3f(ref v); return v; } },
            { typeof(Euclidean3f[]), c => { var v = default(Euclidean3f[]); c.CodeEuclidean3fArray(ref v); return v; } },
            { typeof(List<Euclidean3f>), c => { var v = default(List<Euclidean3f>); c.CodeList_of_Euclidean3f_(ref v); return v; } },

            { typeof(Vector<Euclidean3f>), c => { var v = default(Vector<Euclidean3f>); c.CodeVector_of_Euclidean3f_(ref v); return v; } },
            { typeof(Vector<Euclidean3f>[]), c => { var v = default(Vector<Euclidean3f>[]); c.CodeVector_of_Euclidean3f_Array(ref v); return v; } },
            { typeof(List<Vector<Euclidean3f>>), c => { var v = default(List<Vector<Euclidean3f>>); c.CodeList_of_Vector_of_Euclidean3f__(ref v); return v; } },

            { typeof(Matrix<Euclidean3f>), c => { var v = default(Matrix<Euclidean3f>); c.CodeMatrix_of_Euclidean3f_(ref v); return v; } },
            { typeof(Matrix<Euclidean3f>[]), c => { var v = default(Matrix<Euclidean3f>[]); c.CodeMatrix_of_Euclidean3f_Array(ref v); return v; } },
            { typeof(List<Matrix<Euclidean3f>>), c => { var v = default(List<Matrix<Euclidean3f>>); c.CodeList_of_Matrix_of_Euclidean3f__(ref v); return v; } },

            { typeof(Volume<Euclidean3f>), c => { var v = default(Volume<Euclidean3f>); c.CodeVolume_of_Euclidean3f_(ref v); return v; } },
            { typeof(Volume<Euclidean3f>[]), c => { var v = default(Volume<Euclidean3f>[]); c.CodeVolume_of_Euclidean3f_Array(ref v); return v; } },
            { typeof(List<Volume<Euclidean3f>>), c => { var v = default(List<Volume<Euclidean3f>>); c.CodeList_of_Volume_of_Euclidean3f__(ref v); return v; } },

            { typeof(Tensor<Euclidean3f>), c => { var v = default(Tensor<Euclidean3f>); c.CodeTensor_of_Euclidean3f_(ref v); return v; } },
            { typeof(Tensor<Euclidean3f>[]), c => { var v = default(Tensor<Euclidean3f>[]); c.CodeTensor_of_Euclidean3f_Array(ref v); return v; } },
            { typeof(List<Tensor<Euclidean3f>>), c => { var v = default(List<Tensor<Euclidean3f>>); c.CodeList_of_Tensor_of_Euclidean3f__(ref v); return v; } },

            #endregion

            #region Euclidean3d

            { typeof(Euclidean3d), c => { var v = default(Euclidean3d); c.CodeEuclidean3d(ref v); return v; } },
            { typeof(Euclidean3d[]), c => { var v = default(Euclidean3d[]); c.CodeEuclidean3dArray(ref v); return v; } },
            { typeof(List<Euclidean3d>), c => { var v = default(List<Euclidean3d>); c.CodeList_of_Euclidean3d_(ref v); return v; } },

            { typeof(Vector<Euclidean3d>), c => { var v = default(Vector<Euclidean3d>); c.CodeVector_of_Euclidean3d_(ref v); return v; } },
            { typeof(Vector<Euclidean3d>[]), c => { var v = default(Vector<Euclidean3d>[]); c.CodeVector_of_Euclidean3d_Array(ref v); return v; } },
            { typeof(List<Vector<Euclidean3d>>), c => { var v = default(List<Vector<Euclidean3d>>); c.CodeList_of_Vector_of_Euclidean3d__(ref v); return v; } },

            { typeof(Matrix<Euclidean3d>), c => { var v = default(Matrix<Euclidean3d>); c.CodeMatrix_of_Euclidean3d_(ref v); return v; } },
            { typeof(Matrix<Euclidean3d>[]), c => { var v = default(Matrix<Euclidean3d>[]); c.CodeMatrix_of_Euclidean3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Euclidean3d>>), c => { var v = default(List<Matrix<Euclidean3d>>); c.CodeList_of_Matrix_of_Euclidean3d__(ref v); return v; } },

            { typeof(Volume<Euclidean3d>), c => { var v = default(Volume<Euclidean3d>); c.CodeVolume_of_Euclidean3d_(ref v); return v; } },
            { typeof(Volume<Euclidean3d>[]), c => { var v = default(Volume<Euclidean3d>[]); c.CodeVolume_of_Euclidean3d_Array(ref v); return v; } },
            { typeof(List<Volume<Euclidean3d>>), c => { var v = default(List<Volume<Euclidean3d>>); c.CodeList_of_Volume_of_Euclidean3d__(ref v); return v; } },

            { typeof(Tensor<Euclidean3d>), c => { var v = default(Tensor<Euclidean3d>); c.CodeTensor_of_Euclidean3d_(ref v); return v; } },
            { typeof(Tensor<Euclidean3d>[]), c => { var v = default(Tensor<Euclidean3d>[]); c.CodeTensor_of_Euclidean3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Euclidean3d>>), c => { var v = default(List<Tensor<Euclidean3d>>); c.CodeList_of_Tensor_of_Euclidean3d__(ref v); return v; } },

            #endregion

            #region Rot2f

            { typeof(Rot2f), c => { var v = default(Rot2f); c.CodeRot2f(ref v); return v; } },
            { typeof(Rot2f[]), c => { var v = default(Rot2f[]); c.CodeRot2fArray(ref v); return v; } },
            { typeof(List<Rot2f>), c => { var v = default(List<Rot2f>); c.CodeList_of_Rot2f_(ref v); return v; } },

            { typeof(Vector<Rot2f>), c => { var v = default(Vector<Rot2f>); c.CodeVector_of_Rot2f_(ref v); return v; } },
            { typeof(Vector<Rot2f>[]), c => { var v = default(Vector<Rot2f>[]); c.CodeVector_of_Rot2f_Array(ref v); return v; } },
            { typeof(List<Vector<Rot2f>>), c => { var v = default(List<Vector<Rot2f>>); c.CodeList_of_Vector_of_Rot2f__(ref v); return v; } },

            { typeof(Matrix<Rot2f>), c => { var v = default(Matrix<Rot2f>); c.CodeMatrix_of_Rot2f_(ref v); return v; } },
            { typeof(Matrix<Rot2f>[]), c => { var v = default(Matrix<Rot2f>[]); c.CodeMatrix_of_Rot2f_Array(ref v); return v; } },
            { typeof(List<Matrix<Rot2f>>), c => { var v = default(List<Matrix<Rot2f>>); c.CodeList_of_Matrix_of_Rot2f__(ref v); return v; } },

            { typeof(Volume<Rot2f>), c => { var v = default(Volume<Rot2f>); c.CodeVolume_of_Rot2f_(ref v); return v; } },
            { typeof(Volume<Rot2f>[]), c => { var v = default(Volume<Rot2f>[]); c.CodeVolume_of_Rot2f_Array(ref v); return v; } },
            { typeof(List<Volume<Rot2f>>), c => { var v = default(List<Volume<Rot2f>>); c.CodeList_of_Volume_of_Rot2f__(ref v); return v; } },

            { typeof(Tensor<Rot2f>), c => { var v = default(Tensor<Rot2f>); c.CodeTensor_of_Rot2f_(ref v); return v; } },
            { typeof(Tensor<Rot2f>[]), c => { var v = default(Tensor<Rot2f>[]); c.CodeTensor_of_Rot2f_Array(ref v); return v; } },
            { typeof(List<Tensor<Rot2f>>), c => { var v = default(List<Tensor<Rot2f>>); c.CodeList_of_Tensor_of_Rot2f__(ref v); return v; } },

            #endregion

            #region Rot2d

            { typeof(Rot2d), c => { var v = default(Rot2d); c.CodeRot2d(ref v); return v; } },
            { typeof(Rot2d[]), c => { var v = default(Rot2d[]); c.CodeRot2dArray(ref v); return v; } },
            { typeof(List<Rot2d>), c => { var v = default(List<Rot2d>); c.CodeList_of_Rot2d_(ref v); return v; } },

            { typeof(Vector<Rot2d>), c => { var v = default(Vector<Rot2d>); c.CodeVector_of_Rot2d_(ref v); return v; } },
            { typeof(Vector<Rot2d>[]), c => { var v = default(Vector<Rot2d>[]); c.CodeVector_of_Rot2d_Array(ref v); return v; } },
            { typeof(List<Vector<Rot2d>>), c => { var v = default(List<Vector<Rot2d>>); c.CodeList_of_Vector_of_Rot2d__(ref v); return v; } },

            { typeof(Matrix<Rot2d>), c => { var v = default(Matrix<Rot2d>); c.CodeMatrix_of_Rot2d_(ref v); return v; } },
            { typeof(Matrix<Rot2d>[]), c => { var v = default(Matrix<Rot2d>[]); c.CodeMatrix_of_Rot2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Rot2d>>), c => { var v = default(List<Matrix<Rot2d>>); c.CodeList_of_Matrix_of_Rot2d__(ref v); return v; } },

            { typeof(Volume<Rot2d>), c => { var v = default(Volume<Rot2d>); c.CodeVolume_of_Rot2d_(ref v); return v; } },
            { typeof(Volume<Rot2d>[]), c => { var v = default(Volume<Rot2d>[]); c.CodeVolume_of_Rot2d_Array(ref v); return v; } },
            { typeof(List<Volume<Rot2d>>), c => { var v = default(List<Volume<Rot2d>>); c.CodeList_of_Volume_of_Rot2d__(ref v); return v; } },

            { typeof(Tensor<Rot2d>), c => { var v = default(Tensor<Rot2d>); c.CodeTensor_of_Rot2d_(ref v); return v; } },
            { typeof(Tensor<Rot2d>[]), c => { var v = default(Tensor<Rot2d>[]); c.CodeTensor_of_Rot2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Rot2d>>), c => { var v = default(List<Tensor<Rot2d>>); c.CodeList_of_Tensor_of_Rot2d__(ref v); return v; } },

            #endregion

            #region Rot3f

            { typeof(Rot3f), c => { var v = default(Rot3f); c.CodeRot3f(ref v); return v; } },
            { typeof(Rot3f[]), c => { var v = default(Rot3f[]); c.CodeRot3fArray(ref v); return v; } },
            { typeof(List<Rot3f>), c => { var v = default(List<Rot3f>); c.CodeList_of_Rot3f_(ref v); return v; } },

            { typeof(Vector<Rot3f>), c => { var v = default(Vector<Rot3f>); c.CodeVector_of_Rot3f_(ref v); return v; } },
            { typeof(Vector<Rot3f>[]), c => { var v = default(Vector<Rot3f>[]); c.CodeVector_of_Rot3f_Array(ref v); return v; } },
            { typeof(List<Vector<Rot3f>>), c => { var v = default(List<Vector<Rot3f>>); c.CodeList_of_Vector_of_Rot3f__(ref v); return v; } },

            { typeof(Matrix<Rot3f>), c => { var v = default(Matrix<Rot3f>); c.CodeMatrix_of_Rot3f_(ref v); return v; } },
            { typeof(Matrix<Rot3f>[]), c => { var v = default(Matrix<Rot3f>[]); c.CodeMatrix_of_Rot3f_Array(ref v); return v; } },
            { typeof(List<Matrix<Rot3f>>), c => { var v = default(List<Matrix<Rot3f>>); c.CodeList_of_Matrix_of_Rot3f__(ref v); return v; } },

            { typeof(Volume<Rot3f>), c => { var v = default(Volume<Rot3f>); c.CodeVolume_of_Rot3f_(ref v); return v; } },
            { typeof(Volume<Rot3f>[]), c => { var v = default(Volume<Rot3f>[]); c.CodeVolume_of_Rot3f_Array(ref v); return v; } },
            { typeof(List<Volume<Rot3f>>), c => { var v = default(List<Volume<Rot3f>>); c.CodeList_of_Volume_of_Rot3f__(ref v); return v; } },

            { typeof(Tensor<Rot3f>), c => { var v = default(Tensor<Rot3f>); c.CodeTensor_of_Rot3f_(ref v); return v; } },
            { typeof(Tensor<Rot3f>[]), c => { var v = default(Tensor<Rot3f>[]); c.CodeTensor_of_Rot3f_Array(ref v); return v; } },
            { typeof(List<Tensor<Rot3f>>), c => { var v = default(List<Tensor<Rot3f>>); c.CodeList_of_Tensor_of_Rot3f__(ref v); return v; } },

            #endregion

            #region Rot3d

            { typeof(Rot3d), c => { var v = default(Rot3d); c.CodeRot3d(ref v); return v; } },
            { typeof(Rot3d[]), c => { var v = default(Rot3d[]); c.CodeRot3dArray(ref v); return v; } },
            { typeof(List<Rot3d>), c => { var v = default(List<Rot3d>); c.CodeList_of_Rot3d_(ref v); return v; } },

            { typeof(Vector<Rot3d>), c => { var v = default(Vector<Rot3d>); c.CodeVector_of_Rot3d_(ref v); return v; } },
            { typeof(Vector<Rot3d>[]), c => { var v = default(Vector<Rot3d>[]); c.CodeVector_of_Rot3d_Array(ref v); return v; } },
            { typeof(List<Vector<Rot3d>>), c => { var v = default(List<Vector<Rot3d>>); c.CodeList_of_Vector_of_Rot3d__(ref v); return v; } },

            { typeof(Matrix<Rot3d>), c => { var v = default(Matrix<Rot3d>); c.CodeMatrix_of_Rot3d_(ref v); return v; } },
            { typeof(Matrix<Rot3d>[]), c => { var v = default(Matrix<Rot3d>[]); c.CodeMatrix_of_Rot3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Rot3d>>), c => { var v = default(List<Matrix<Rot3d>>); c.CodeList_of_Matrix_of_Rot3d__(ref v); return v; } },

            { typeof(Volume<Rot3d>), c => { var v = default(Volume<Rot3d>); c.CodeVolume_of_Rot3d_(ref v); return v; } },
            { typeof(Volume<Rot3d>[]), c => { var v = default(Volume<Rot3d>[]); c.CodeVolume_of_Rot3d_Array(ref v); return v; } },
            { typeof(List<Volume<Rot3d>>), c => { var v = default(List<Volume<Rot3d>>); c.CodeList_of_Volume_of_Rot3d__(ref v); return v; } },

            { typeof(Tensor<Rot3d>), c => { var v = default(Tensor<Rot3d>); c.CodeTensor_of_Rot3d_(ref v); return v; } },
            { typeof(Tensor<Rot3d>[]), c => { var v = default(Tensor<Rot3d>[]); c.CodeTensor_of_Rot3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Rot3d>>), c => { var v = default(List<Tensor<Rot3d>>); c.CodeList_of_Tensor_of_Rot3d__(ref v); return v; } },

            #endregion

            #region Scale3f

            { typeof(Scale3f), c => { var v = default(Scale3f); c.CodeScale3f(ref v); return v; } },
            { typeof(Scale3f[]), c => { var v = default(Scale3f[]); c.CodeScale3fArray(ref v); return v; } },
            { typeof(List<Scale3f>), c => { var v = default(List<Scale3f>); c.CodeList_of_Scale3f_(ref v); return v; } },

            { typeof(Vector<Scale3f>), c => { var v = default(Vector<Scale3f>); c.CodeVector_of_Scale3f_(ref v); return v; } },
            { typeof(Vector<Scale3f>[]), c => { var v = default(Vector<Scale3f>[]); c.CodeVector_of_Scale3f_Array(ref v); return v; } },
            { typeof(List<Vector<Scale3f>>), c => { var v = default(List<Vector<Scale3f>>); c.CodeList_of_Vector_of_Scale3f__(ref v); return v; } },

            { typeof(Matrix<Scale3f>), c => { var v = default(Matrix<Scale3f>); c.CodeMatrix_of_Scale3f_(ref v); return v; } },
            { typeof(Matrix<Scale3f>[]), c => { var v = default(Matrix<Scale3f>[]); c.CodeMatrix_of_Scale3f_Array(ref v); return v; } },
            { typeof(List<Matrix<Scale3f>>), c => { var v = default(List<Matrix<Scale3f>>); c.CodeList_of_Matrix_of_Scale3f__(ref v); return v; } },

            { typeof(Volume<Scale3f>), c => { var v = default(Volume<Scale3f>); c.CodeVolume_of_Scale3f_(ref v); return v; } },
            { typeof(Volume<Scale3f>[]), c => { var v = default(Volume<Scale3f>[]); c.CodeVolume_of_Scale3f_Array(ref v); return v; } },
            { typeof(List<Volume<Scale3f>>), c => { var v = default(List<Volume<Scale3f>>); c.CodeList_of_Volume_of_Scale3f__(ref v); return v; } },

            { typeof(Tensor<Scale3f>), c => { var v = default(Tensor<Scale3f>); c.CodeTensor_of_Scale3f_(ref v); return v; } },
            { typeof(Tensor<Scale3f>[]), c => { var v = default(Tensor<Scale3f>[]); c.CodeTensor_of_Scale3f_Array(ref v); return v; } },
            { typeof(List<Tensor<Scale3f>>), c => { var v = default(List<Tensor<Scale3f>>); c.CodeList_of_Tensor_of_Scale3f__(ref v); return v; } },

            #endregion

            #region Scale3d

            { typeof(Scale3d), c => { var v = default(Scale3d); c.CodeScale3d(ref v); return v; } },
            { typeof(Scale3d[]), c => { var v = default(Scale3d[]); c.CodeScale3dArray(ref v); return v; } },
            { typeof(List<Scale3d>), c => { var v = default(List<Scale3d>); c.CodeList_of_Scale3d_(ref v); return v; } },

            { typeof(Vector<Scale3d>), c => { var v = default(Vector<Scale3d>); c.CodeVector_of_Scale3d_(ref v); return v; } },
            { typeof(Vector<Scale3d>[]), c => { var v = default(Vector<Scale3d>[]); c.CodeVector_of_Scale3d_Array(ref v); return v; } },
            { typeof(List<Vector<Scale3d>>), c => { var v = default(List<Vector<Scale3d>>); c.CodeList_of_Vector_of_Scale3d__(ref v); return v; } },

            { typeof(Matrix<Scale3d>), c => { var v = default(Matrix<Scale3d>); c.CodeMatrix_of_Scale3d_(ref v); return v; } },
            { typeof(Matrix<Scale3d>[]), c => { var v = default(Matrix<Scale3d>[]); c.CodeMatrix_of_Scale3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Scale3d>>), c => { var v = default(List<Matrix<Scale3d>>); c.CodeList_of_Matrix_of_Scale3d__(ref v); return v; } },

            { typeof(Volume<Scale3d>), c => { var v = default(Volume<Scale3d>); c.CodeVolume_of_Scale3d_(ref v); return v; } },
            { typeof(Volume<Scale3d>[]), c => { var v = default(Volume<Scale3d>[]); c.CodeVolume_of_Scale3d_Array(ref v); return v; } },
            { typeof(List<Volume<Scale3d>>), c => { var v = default(List<Volume<Scale3d>>); c.CodeList_of_Volume_of_Scale3d__(ref v); return v; } },

            { typeof(Tensor<Scale3d>), c => { var v = default(Tensor<Scale3d>); c.CodeTensor_of_Scale3d_(ref v); return v; } },
            { typeof(Tensor<Scale3d>[]), c => { var v = default(Tensor<Scale3d>[]); c.CodeTensor_of_Scale3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Scale3d>>), c => { var v = default(List<Tensor<Scale3d>>); c.CodeList_of_Tensor_of_Scale3d__(ref v); return v; } },

            #endregion

            #region Shift3f

            { typeof(Shift3f), c => { var v = default(Shift3f); c.CodeShift3f(ref v); return v; } },
            { typeof(Shift3f[]), c => { var v = default(Shift3f[]); c.CodeShift3fArray(ref v); return v; } },
            { typeof(List<Shift3f>), c => { var v = default(List<Shift3f>); c.CodeList_of_Shift3f_(ref v); return v; } },

            { typeof(Vector<Shift3f>), c => { var v = default(Vector<Shift3f>); c.CodeVector_of_Shift3f_(ref v); return v; } },
            { typeof(Vector<Shift3f>[]), c => { var v = default(Vector<Shift3f>[]); c.CodeVector_of_Shift3f_Array(ref v); return v; } },
            { typeof(List<Vector<Shift3f>>), c => { var v = default(List<Vector<Shift3f>>); c.CodeList_of_Vector_of_Shift3f__(ref v); return v; } },

            { typeof(Matrix<Shift3f>), c => { var v = default(Matrix<Shift3f>); c.CodeMatrix_of_Shift3f_(ref v); return v; } },
            { typeof(Matrix<Shift3f>[]), c => { var v = default(Matrix<Shift3f>[]); c.CodeMatrix_of_Shift3f_Array(ref v); return v; } },
            { typeof(List<Matrix<Shift3f>>), c => { var v = default(List<Matrix<Shift3f>>); c.CodeList_of_Matrix_of_Shift3f__(ref v); return v; } },

            { typeof(Volume<Shift3f>), c => { var v = default(Volume<Shift3f>); c.CodeVolume_of_Shift3f_(ref v); return v; } },
            { typeof(Volume<Shift3f>[]), c => { var v = default(Volume<Shift3f>[]); c.CodeVolume_of_Shift3f_Array(ref v); return v; } },
            { typeof(List<Volume<Shift3f>>), c => { var v = default(List<Volume<Shift3f>>); c.CodeList_of_Volume_of_Shift3f__(ref v); return v; } },

            { typeof(Tensor<Shift3f>), c => { var v = default(Tensor<Shift3f>); c.CodeTensor_of_Shift3f_(ref v); return v; } },
            { typeof(Tensor<Shift3f>[]), c => { var v = default(Tensor<Shift3f>[]); c.CodeTensor_of_Shift3f_Array(ref v); return v; } },
            { typeof(List<Tensor<Shift3f>>), c => { var v = default(List<Tensor<Shift3f>>); c.CodeList_of_Tensor_of_Shift3f__(ref v); return v; } },

            #endregion

            #region Shift3d

            { typeof(Shift3d), c => { var v = default(Shift3d); c.CodeShift3d(ref v); return v; } },
            { typeof(Shift3d[]), c => { var v = default(Shift3d[]); c.CodeShift3dArray(ref v); return v; } },
            { typeof(List<Shift3d>), c => { var v = default(List<Shift3d>); c.CodeList_of_Shift3d_(ref v); return v; } },

            { typeof(Vector<Shift3d>), c => { var v = default(Vector<Shift3d>); c.CodeVector_of_Shift3d_(ref v); return v; } },
            { typeof(Vector<Shift3d>[]), c => { var v = default(Vector<Shift3d>[]); c.CodeVector_of_Shift3d_Array(ref v); return v; } },
            { typeof(List<Vector<Shift3d>>), c => { var v = default(List<Vector<Shift3d>>); c.CodeList_of_Vector_of_Shift3d__(ref v); return v; } },

            { typeof(Matrix<Shift3d>), c => { var v = default(Matrix<Shift3d>); c.CodeMatrix_of_Shift3d_(ref v); return v; } },
            { typeof(Matrix<Shift3d>[]), c => { var v = default(Matrix<Shift3d>[]); c.CodeMatrix_of_Shift3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Shift3d>>), c => { var v = default(List<Matrix<Shift3d>>); c.CodeList_of_Matrix_of_Shift3d__(ref v); return v; } },

            { typeof(Volume<Shift3d>), c => { var v = default(Volume<Shift3d>); c.CodeVolume_of_Shift3d_(ref v); return v; } },
            { typeof(Volume<Shift3d>[]), c => { var v = default(Volume<Shift3d>[]); c.CodeVolume_of_Shift3d_Array(ref v); return v; } },
            { typeof(List<Volume<Shift3d>>), c => { var v = default(List<Volume<Shift3d>>); c.CodeList_of_Volume_of_Shift3d__(ref v); return v; } },

            { typeof(Tensor<Shift3d>), c => { var v = default(Tensor<Shift3d>); c.CodeTensor_of_Shift3d_(ref v); return v; } },
            { typeof(Tensor<Shift3d>[]), c => { var v = default(Tensor<Shift3d>[]); c.CodeTensor_of_Shift3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Shift3d>>), c => { var v = default(List<Tensor<Shift3d>>); c.CodeList_of_Tensor_of_Shift3d__(ref v); return v; } },

            #endregion

            #region Trafo2f

            { typeof(Trafo2f), c => { var v = default(Trafo2f); c.CodeTrafo2f(ref v); return v; } },
            { typeof(Trafo2f[]), c => { var v = default(Trafo2f[]); c.CodeTrafo2fArray(ref v); return v; } },
            { typeof(List<Trafo2f>), c => { var v = default(List<Trafo2f>); c.CodeList_of_Trafo2f_(ref v); return v; } },

            { typeof(Vector<Trafo2f>), c => { var v = default(Vector<Trafo2f>); c.CodeVector_of_Trafo2f_(ref v); return v; } },
            { typeof(Vector<Trafo2f>[]), c => { var v = default(Vector<Trafo2f>[]); c.CodeVector_of_Trafo2f_Array(ref v); return v; } },
            { typeof(List<Vector<Trafo2f>>), c => { var v = default(List<Vector<Trafo2f>>); c.CodeList_of_Vector_of_Trafo2f__(ref v); return v; } },

            { typeof(Matrix<Trafo2f>), c => { var v = default(Matrix<Trafo2f>); c.CodeMatrix_of_Trafo2f_(ref v); return v; } },
            { typeof(Matrix<Trafo2f>[]), c => { var v = default(Matrix<Trafo2f>[]); c.CodeMatrix_of_Trafo2f_Array(ref v); return v; } },
            { typeof(List<Matrix<Trafo2f>>), c => { var v = default(List<Matrix<Trafo2f>>); c.CodeList_of_Matrix_of_Trafo2f__(ref v); return v; } },

            { typeof(Volume<Trafo2f>), c => { var v = default(Volume<Trafo2f>); c.CodeVolume_of_Trafo2f_(ref v); return v; } },
            { typeof(Volume<Trafo2f>[]), c => { var v = default(Volume<Trafo2f>[]); c.CodeVolume_of_Trafo2f_Array(ref v); return v; } },
            { typeof(List<Volume<Trafo2f>>), c => { var v = default(List<Volume<Trafo2f>>); c.CodeList_of_Volume_of_Trafo2f__(ref v); return v; } },

            { typeof(Tensor<Trafo2f>), c => { var v = default(Tensor<Trafo2f>); c.CodeTensor_of_Trafo2f_(ref v); return v; } },
            { typeof(Tensor<Trafo2f>[]), c => { var v = default(Tensor<Trafo2f>[]); c.CodeTensor_of_Trafo2f_Array(ref v); return v; } },
            { typeof(List<Tensor<Trafo2f>>), c => { var v = default(List<Tensor<Trafo2f>>); c.CodeList_of_Tensor_of_Trafo2f__(ref v); return v; } },

            #endregion

            #region Trafo2d

            { typeof(Trafo2d), c => { var v = default(Trafo2d); c.CodeTrafo2d(ref v); return v; } },
            { typeof(Trafo2d[]), c => { var v = default(Trafo2d[]); c.CodeTrafo2dArray(ref v); return v; } },
            { typeof(List<Trafo2d>), c => { var v = default(List<Trafo2d>); c.CodeList_of_Trafo2d_(ref v); return v; } },

            { typeof(Vector<Trafo2d>), c => { var v = default(Vector<Trafo2d>); c.CodeVector_of_Trafo2d_(ref v); return v; } },
            { typeof(Vector<Trafo2d>[]), c => { var v = default(Vector<Trafo2d>[]); c.CodeVector_of_Trafo2d_Array(ref v); return v; } },
            { typeof(List<Vector<Trafo2d>>), c => { var v = default(List<Vector<Trafo2d>>); c.CodeList_of_Vector_of_Trafo2d__(ref v); return v; } },

            { typeof(Matrix<Trafo2d>), c => { var v = default(Matrix<Trafo2d>); c.CodeMatrix_of_Trafo2d_(ref v); return v; } },
            { typeof(Matrix<Trafo2d>[]), c => { var v = default(Matrix<Trafo2d>[]); c.CodeMatrix_of_Trafo2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Trafo2d>>), c => { var v = default(List<Matrix<Trafo2d>>); c.CodeList_of_Matrix_of_Trafo2d__(ref v); return v; } },

            { typeof(Volume<Trafo2d>), c => { var v = default(Volume<Trafo2d>); c.CodeVolume_of_Trafo2d_(ref v); return v; } },
            { typeof(Volume<Trafo2d>[]), c => { var v = default(Volume<Trafo2d>[]); c.CodeVolume_of_Trafo2d_Array(ref v); return v; } },
            { typeof(List<Volume<Trafo2d>>), c => { var v = default(List<Volume<Trafo2d>>); c.CodeList_of_Volume_of_Trafo2d__(ref v); return v; } },

            { typeof(Tensor<Trafo2d>), c => { var v = default(Tensor<Trafo2d>); c.CodeTensor_of_Trafo2d_(ref v); return v; } },
            { typeof(Tensor<Trafo2d>[]), c => { var v = default(Tensor<Trafo2d>[]); c.CodeTensor_of_Trafo2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Trafo2d>>), c => { var v = default(List<Tensor<Trafo2d>>); c.CodeList_of_Tensor_of_Trafo2d__(ref v); return v; } },

            #endregion

            #region Trafo3f

            { typeof(Trafo3f), c => { var v = default(Trafo3f); c.CodeTrafo3f(ref v); return v; } },
            { typeof(Trafo3f[]), c => { var v = default(Trafo3f[]); c.CodeTrafo3fArray(ref v); return v; } },
            { typeof(List<Trafo3f>), c => { var v = default(List<Trafo3f>); c.CodeList_of_Trafo3f_(ref v); return v; } },

            { typeof(Vector<Trafo3f>), c => { var v = default(Vector<Trafo3f>); c.CodeVector_of_Trafo3f_(ref v); return v; } },
            { typeof(Vector<Trafo3f>[]), c => { var v = default(Vector<Trafo3f>[]); c.CodeVector_of_Trafo3f_Array(ref v); return v; } },
            { typeof(List<Vector<Trafo3f>>), c => { var v = default(List<Vector<Trafo3f>>); c.CodeList_of_Vector_of_Trafo3f__(ref v); return v; } },

            { typeof(Matrix<Trafo3f>), c => { var v = default(Matrix<Trafo3f>); c.CodeMatrix_of_Trafo3f_(ref v); return v; } },
            { typeof(Matrix<Trafo3f>[]), c => { var v = default(Matrix<Trafo3f>[]); c.CodeMatrix_of_Trafo3f_Array(ref v); return v; } },
            { typeof(List<Matrix<Trafo3f>>), c => { var v = default(List<Matrix<Trafo3f>>); c.CodeList_of_Matrix_of_Trafo3f__(ref v); return v; } },

            { typeof(Volume<Trafo3f>), c => { var v = default(Volume<Trafo3f>); c.CodeVolume_of_Trafo3f_(ref v); return v; } },
            { typeof(Volume<Trafo3f>[]), c => { var v = default(Volume<Trafo3f>[]); c.CodeVolume_of_Trafo3f_Array(ref v); return v; } },
            { typeof(List<Volume<Trafo3f>>), c => { var v = default(List<Volume<Trafo3f>>); c.CodeList_of_Volume_of_Trafo3f__(ref v); return v; } },

            { typeof(Tensor<Trafo3f>), c => { var v = default(Tensor<Trafo3f>); c.CodeTensor_of_Trafo3f_(ref v); return v; } },
            { typeof(Tensor<Trafo3f>[]), c => { var v = default(Tensor<Trafo3f>[]); c.CodeTensor_of_Trafo3f_Array(ref v); return v; } },
            { typeof(List<Tensor<Trafo3f>>), c => { var v = default(List<Tensor<Trafo3f>>); c.CodeList_of_Tensor_of_Trafo3f__(ref v); return v; } },

            #endregion

            #region Trafo3d

            { typeof(Trafo3d), c => { var v = default(Trafo3d); c.CodeTrafo3d(ref v); return v; } },
            { typeof(Trafo3d[]), c => { var v = default(Trafo3d[]); c.CodeTrafo3dArray(ref v); return v; } },
            { typeof(List<Trafo3d>), c => { var v = default(List<Trafo3d>); c.CodeList_of_Trafo3d_(ref v); return v; } },

            { typeof(Vector<Trafo3d>), c => { var v = default(Vector<Trafo3d>); c.CodeVector_of_Trafo3d_(ref v); return v; } },
            { typeof(Vector<Trafo3d>[]), c => { var v = default(Vector<Trafo3d>[]); c.CodeVector_of_Trafo3d_Array(ref v); return v; } },
            { typeof(List<Vector<Trafo3d>>), c => { var v = default(List<Vector<Trafo3d>>); c.CodeList_of_Vector_of_Trafo3d__(ref v); return v; } },

            { typeof(Matrix<Trafo3d>), c => { var v = default(Matrix<Trafo3d>); c.CodeMatrix_of_Trafo3d_(ref v); return v; } },
            { typeof(Matrix<Trafo3d>[]), c => { var v = default(Matrix<Trafo3d>[]); c.CodeMatrix_of_Trafo3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Trafo3d>>), c => { var v = default(List<Matrix<Trafo3d>>); c.CodeList_of_Matrix_of_Trafo3d__(ref v); return v; } },

            { typeof(Volume<Trafo3d>), c => { var v = default(Volume<Trafo3d>); c.CodeVolume_of_Trafo3d_(ref v); return v; } },
            { typeof(Volume<Trafo3d>[]), c => { var v = default(Volume<Trafo3d>[]); c.CodeVolume_of_Trafo3d_Array(ref v); return v; } },
            { typeof(List<Volume<Trafo3d>>), c => { var v = default(List<Volume<Trafo3d>>); c.CodeList_of_Volume_of_Trafo3d__(ref v); return v; } },

            { typeof(Tensor<Trafo3d>), c => { var v = default(Tensor<Trafo3d>); c.CodeTensor_of_Trafo3d_(ref v); return v; } },
            { typeof(Tensor<Trafo3d>[]), c => { var v = default(Tensor<Trafo3d>[]); c.CodeTensor_of_Trafo3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Trafo3d>>), c => { var v = default(List<Tensor<Trafo3d>>); c.CodeList_of_Tensor_of_Trafo3d__(ref v); return v; } },

            #endregion

            #region bool

            { typeof(bool), c => { var v = default(bool); c.CodeBool(ref v); return v; } },
            { typeof(bool[]), c => { var v = default(bool[]); c.CodeBoolArray(ref v); return v; } },
            { typeof(List<bool>), c => { var v = default(List<bool>); c.CodeList_of_Bool_(ref v); return v; } },

            { typeof(Vector<bool>), c => { var v = default(Vector<bool>); c.CodeVector_of_Bool_(ref v); return v; } },
            { typeof(Vector<bool>[]), c => { var v = default(Vector<bool>[]); c.CodeVector_of_Bool_Array(ref v); return v; } },
            { typeof(List<Vector<bool>>), c => { var v = default(List<Vector<bool>>); c.CodeList_of_Vector_of_Bool__(ref v); return v; } },

            { typeof(Matrix<bool>), c => { var v = default(Matrix<bool>); c.CodeMatrix_of_Bool_(ref v); return v; } },
            { typeof(Matrix<bool>[]), c => { var v = default(Matrix<bool>[]); c.CodeMatrix_of_Bool_Array(ref v); return v; } },
            { typeof(List<Matrix<bool>>), c => { var v = default(List<Matrix<bool>>); c.CodeList_of_Matrix_of_Bool__(ref v); return v; } },

            { typeof(Volume<bool>), c => { var v = default(Volume<bool>); c.CodeVolume_of_Bool_(ref v); return v; } },
            { typeof(Volume<bool>[]), c => { var v = default(Volume<bool>[]); c.CodeVolume_of_Bool_Array(ref v); return v; } },
            { typeof(List<Volume<bool>>), c => { var v = default(List<Volume<bool>>); c.CodeList_of_Volume_of_Bool__(ref v); return v; } },

            { typeof(Tensor<bool>), c => { var v = default(Tensor<bool>); c.CodeTensor_of_Bool_(ref v); return v; } },
            { typeof(Tensor<bool>[]), c => { var v = default(Tensor<bool>[]); c.CodeTensor_of_Bool_Array(ref v); return v; } },
            { typeof(List<Tensor<bool>>), c => { var v = default(List<Tensor<bool>>); c.CodeList_of_Tensor_of_Bool__(ref v); return v; } },

            #endregion

            #region char

            { typeof(char), c => { var v = default(char); c.CodeChar(ref v); return v; } },
            { typeof(char[]), c => { var v = default(char[]); c.CodeCharArray(ref v); return v; } },
            { typeof(List<char>), c => { var v = default(List<char>); c.CodeList_of_Char_(ref v); return v; } },

            { typeof(Vector<char>), c => { var v = default(Vector<char>); c.CodeVector_of_Char_(ref v); return v; } },
            { typeof(Vector<char>[]), c => { var v = default(Vector<char>[]); c.CodeVector_of_Char_Array(ref v); return v; } },
            { typeof(List<Vector<char>>), c => { var v = default(List<Vector<char>>); c.CodeList_of_Vector_of_Char__(ref v); return v; } },

            { typeof(Matrix<char>), c => { var v = default(Matrix<char>); c.CodeMatrix_of_Char_(ref v); return v; } },
            { typeof(Matrix<char>[]), c => { var v = default(Matrix<char>[]); c.CodeMatrix_of_Char_Array(ref v); return v; } },
            { typeof(List<Matrix<char>>), c => { var v = default(List<Matrix<char>>); c.CodeList_of_Matrix_of_Char__(ref v); return v; } },

            { typeof(Volume<char>), c => { var v = default(Volume<char>); c.CodeVolume_of_Char_(ref v); return v; } },
            { typeof(Volume<char>[]), c => { var v = default(Volume<char>[]); c.CodeVolume_of_Char_Array(ref v); return v; } },
            { typeof(List<Volume<char>>), c => { var v = default(List<Volume<char>>); c.CodeList_of_Volume_of_Char__(ref v); return v; } },

            { typeof(Tensor<char>), c => { var v = default(Tensor<char>); c.CodeTensor_of_Char_(ref v); return v; } },
            { typeof(Tensor<char>[]), c => { var v = default(Tensor<char>[]); c.CodeTensor_of_Char_Array(ref v); return v; } },
            { typeof(List<Tensor<char>>), c => { var v = default(List<Tensor<char>>); c.CodeList_of_Tensor_of_Char__(ref v); return v; } },

            #endregion

            #region string

            { typeof(string), c => { var v = default(string); c.CodeString(ref v); return v; } },
            { typeof(string[]), c => { var v = default(string[]); c.CodeStringArray(ref v); return v; } },
            { typeof(List<string>), c => { var v = default(List<string>); c.CodeList_of_String_(ref v); return v; } },

            { typeof(Vector<string>), c => { var v = default(Vector<string>); c.CodeVector_of_String_(ref v); return v; } },
            { typeof(Vector<string>[]), c => { var v = default(Vector<string>[]); c.CodeVector_of_String_Array(ref v); return v; } },
            { typeof(List<Vector<string>>), c => { var v = default(List<Vector<string>>); c.CodeList_of_Vector_of_String__(ref v); return v; } },

            { typeof(Matrix<string>), c => { var v = default(Matrix<string>); c.CodeMatrix_of_String_(ref v); return v; } },
            { typeof(Matrix<string>[]), c => { var v = default(Matrix<string>[]); c.CodeMatrix_of_String_Array(ref v); return v; } },
            { typeof(List<Matrix<string>>), c => { var v = default(List<Matrix<string>>); c.CodeList_of_Matrix_of_String__(ref v); return v; } },

            { typeof(Volume<string>), c => { var v = default(Volume<string>); c.CodeVolume_of_String_(ref v); return v; } },
            { typeof(Volume<string>[]), c => { var v = default(Volume<string>[]); c.CodeVolume_of_String_Array(ref v); return v; } },
            { typeof(List<Volume<string>>), c => { var v = default(List<Volume<string>>); c.CodeList_of_Volume_of_String__(ref v); return v; } },

            { typeof(Tensor<string>), c => { var v = default(Tensor<string>); c.CodeTensor_of_String_(ref v); return v; } },
            { typeof(Tensor<string>[]), c => { var v = default(Tensor<string>[]); c.CodeTensor_of_String_Array(ref v); return v; } },
            { typeof(List<Tensor<string>>), c => { var v = default(List<Tensor<string>>); c.CodeList_of_Tensor_of_String__(ref v); return v; } },

            #endregion

            #region Type

            { typeof(Type), c => { var v = default(Type); c.CodeType(ref v); return v; } },
            { typeof(Type[]), c => { var v = default(Type[]); c.CodeTypeArray(ref v); return v; } },
            { typeof(List<Type>), c => { var v = default(List<Type>); c.CodeList_of_Type_(ref v); return v; } },

            { typeof(Vector<Type>), c => { var v = default(Vector<Type>); c.CodeVector_of_Type_(ref v); return v; } },
            { typeof(Vector<Type>[]), c => { var v = default(Vector<Type>[]); c.CodeVector_of_Type_Array(ref v); return v; } },
            { typeof(List<Vector<Type>>), c => { var v = default(List<Vector<Type>>); c.CodeList_of_Vector_of_Type__(ref v); return v; } },

            { typeof(Matrix<Type>), c => { var v = default(Matrix<Type>); c.CodeMatrix_of_Type_(ref v); return v; } },
            { typeof(Matrix<Type>[]), c => { var v = default(Matrix<Type>[]); c.CodeMatrix_of_Type_Array(ref v); return v; } },
            { typeof(List<Matrix<Type>>), c => { var v = default(List<Matrix<Type>>); c.CodeList_of_Matrix_of_Type__(ref v); return v; } },

            { typeof(Volume<Type>), c => { var v = default(Volume<Type>); c.CodeVolume_of_Type_(ref v); return v; } },
            { typeof(Volume<Type>[]), c => { var v = default(Volume<Type>[]); c.CodeVolume_of_Type_Array(ref v); return v; } },
            { typeof(List<Volume<Type>>), c => { var v = default(List<Volume<Type>>); c.CodeList_of_Volume_of_Type__(ref v); return v; } },

            { typeof(Tensor<Type>), c => { var v = default(Tensor<Type>); c.CodeTensor_of_Type_(ref v); return v; } },
            { typeof(Tensor<Type>[]), c => { var v = default(Tensor<Type>[]); c.CodeTensor_of_Type_Array(ref v); return v; } },
            { typeof(List<Tensor<Type>>), c => { var v = default(List<Tensor<Type>>); c.CodeList_of_Tensor_of_Type__(ref v); return v; } },

            #endregion

            #region Guid

            { typeof(Guid), c => { var v = default(Guid); c.CodeGuid(ref v); return v; } },
            { typeof(Guid[]), c => { var v = default(Guid[]); c.CodeGuidArray(ref v); return v; } },
            { typeof(List<Guid>), c => { var v = default(List<Guid>); c.CodeList_of_Guid_(ref v); return v; } },

            { typeof(Vector<Guid>), c => { var v = default(Vector<Guid>); c.CodeVector_of_Guid_(ref v); return v; } },
            { typeof(Vector<Guid>[]), c => { var v = default(Vector<Guid>[]); c.CodeVector_of_Guid_Array(ref v); return v; } },
            { typeof(List<Vector<Guid>>), c => { var v = default(List<Vector<Guid>>); c.CodeList_of_Vector_of_Guid__(ref v); return v; } },

            { typeof(Matrix<Guid>), c => { var v = default(Matrix<Guid>); c.CodeMatrix_of_Guid_(ref v); return v; } },
            { typeof(Matrix<Guid>[]), c => { var v = default(Matrix<Guid>[]); c.CodeMatrix_of_Guid_Array(ref v); return v; } },
            { typeof(List<Matrix<Guid>>), c => { var v = default(List<Matrix<Guid>>); c.CodeList_of_Matrix_of_Guid__(ref v); return v; } },

            { typeof(Volume<Guid>), c => { var v = default(Volume<Guid>); c.CodeVolume_of_Guid_(ref v); return v; } },
            { typeof(Volume<Guid>[]), c => { var v = default(Volume<Guid>[]); c.CodeVolume_of_Guid_Array(ref v); return v; } },
            { typeof(List<Volume<Guid>>), c => { var v = default(List<Volume<Guid>>); c.CodeList_of_Volume_of_Guid__(ref v); return v; } },

            { typeof(Tensor<Guid>), c => { var v = default(Tensor<Guid>); c.CodeTensor_of_Guid_(ref v); return v; } },
            { typeof(Tensor<Guid>[]), c => { var v = default(Tensor<Guid>[]); c.CodeTensor_of_Guid_Array(ref v); return v; } },
            { typeof(List<Tensor<Guid>>), c => { var v = default(List<Tensor<Guid>>); c.CodeList_of_Tensor_of_Guid__(ref v); return v; } },

            #endregion

            #region Symbol

            { typeof(Symbol), c => { var v = default(Symbol); c.CodeSymbol(ref v); return v; } },
            { typeof(Symbol[]), c => { var v = default(Symbol[]); c.CodeSymbolArray(ref v); return v; } },
            { typeof(List<Symbol>), c => { var v = default(List<Symbol>); c.CodeList_of_Symbol_(ref v); return v; } },

            { typeof(Vector<Symbol>), c => { var v = default(Vector<Symbol>); c.CodeVector_of_Symbol_(ref v); return v; } },
            { typeof(Vector<Symbol>[]), c => { var v = default(Vector<Symbol>[]); c.CodeVector_of_Symbol_Array(ref v); return v; } },
            { typeof(List<Vector<Symbol>>), c => { var v = default(List<Vector<Symbol>>); c.CodeList_of_Vector_of_Symbol__(ref v); return v; } },

            { typeof(Matrix<Symbol>), c => { var v = default(Matrix<Symbol>); c.CodeMatrix_of_Symbol_(ref v); return v; } },
            { typeof(Matrix<Symbol>[]), c => { var v = default(Matrix<Symbol>[]); c.CodeMatrix_of_Symbol_Array(ref v); return v; } },
            { typeof(List<Matrix<Symbol>>), c => { var v = default(List<Matrix<Symbol>>); c.CodeList_of_Matrix_of_Symbol__(ref v); return v; } },

            { typeof(Volume<Symbol>), c => { var v = default(Volume<Symbol>); c.CodeVolume_of_Symbol_(ref v); return v; } },
            { typeof(Volume<Symbol>[]), c => { var v = default(Volume<Symbol>[]); c.CodeVolume_of_Symbol_Array(ref v); return v; } },
            { typeof(List<Volume<Symbol>>), c => { var v = default(List<Volume<Symbol>>); c.CodeList_of_Volume_of_Symbol__(ref v); return v; } },

            { typeof(Tensor<Symbol>), c => { var v = default(Tensor<Symbol>); c.CodeTensor_of_Symbol_(ref v); return v; } },
            { typeof(Tensor<Symbol>[]), c => { var v = default(Tensor<Symbol>[]); c.CodeTensor_of_Symbol_Array(ref v); return v; } },
            { typeof(List<Tensor<Symbol>>), c => { var v = default(List<Tensor<Symbol>>); c.CodeList_of_Tensor_of_Symbol__(ref v); return v; } },

            #endregion

            #region Circle2d

            { typeof(Circle2d), c => { var v = default(Circle2d); c.CodeCircle2d(ref v); return v; } },
            { typeof(Circle2d[]), c => { var v = default(Circle2d[]); c.CodeCircle2dArray(ref v); return v; } },
            { typeof(List<Circle2d>), c => { var v = default(List<Circle2d>); c.CodeList_of_Circle2d_(ref v); return v; } },

            { typeof(Vector<Circle2d>), c => { var v = default(Vector<Circle2d>); c.CodeVector_of_Circle2d_(ref v); return v; } },
            { typeof(Vector<Circle2d>[]), c => { var v = default(Vector<Circle2d>[]); c.CodeVector_of_Circle2d_Array(ref v); return v; } },
            { typeof(List<Vector<Circle2d>>), c => { var v = default(List<Vector<Circle2d>>); c.CodeList_of_Vector_of_Circle2d__(ref v); return v; } },

            { typeof(Matrix<Circle2d>), c => { var v = default(Matrix<Circle2d>); c.CodeMatrix_of_Circle2d_(ref v); return v; } },
            { typeof(Matrix<Circle2d>[]), c => { var v = default(Matrix<Circle2d>[]); c.CodeMatrix_of_Circle2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Circle2d>>), c => { var v = default(List<Matrix<Circle2d>>); c.CodeList_of_Matrix_of_Circle2d__(ref v); return v; } },

            { typeof(Volume<Circle2d>), c => { var v = default(Volume<Circle2d>); c.CodeVolume_of_Circle2d_(ref v); return v; } },
            { typeof(Volume<Circle2d>[]), c => { var v = default(Volume<Circle2d>[]); c.CodeVolume_of_Circle2d_Array(ref v); return v; } },
            { typeof(List<Volume<Circle2d>>), c => { var v = default(List<Volume<Circle2d>>); c.CodeList_of_Volume_of_Circle2d__(ref v); return v; } },

            { typeof(Tensor<Circle2d>), c => { var v = default(Tensor<Circle2d>); c.CodeTensor_of_Circle2d_(ref v); return v; } },
            { typeof(Tensor<Circle2d>[]), c => { var v = default(Tensor<Circle2d>[]); c.CodeTensor_of_Circle2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Circle2d>>), c => { var v = default(List<Tensor<Circle2d>>); c.CodeList_of_Tensor_of_Circle2d__(ref v); return v; } },

            #endregion

            #region Line2d

            { typeof(Line2d), c => { var v = default(Line2d); c.CodeLine2d(ref v); return v; } },
            { typeof(Line2d[]), c => { var v = default(Line2d[]); c.CodeLine2dArray(ref v); return v; } },
            { typeof(List<Line2d>), c => { var v = default(List<Line2d>); c.CodeList_of_Line2d_(ref v); return v; } },

            { typeof(Vector<Line2d>), c => { var v = default(Vector<Line2d>); c.CodeVector_of_Line2d_(ref v); return v; } },
            { typeof(Vector<Line2d>[]), c => { var v = default(Vector<Line2d>[]); c.CodeVector_of_Line2d_Array(ref v); return v; } },
            { typeof(List<Vector<Line2d>>), c => { var v = default(List<Vector<Line2d>>); c.CodeList_of_Vector_of_Line2d__(ref v); return v; } },

            { typeof(Matrix<Line2d>), c => { var v = default(Matrix<Line2d>); c.CodeMatrix_of_Line2d_(ref v); return v; } },
            { typeof(Matrix<Line2d>[]), c => { var v = default(Matrix<Line2d>[]); c.CodeMatrix_of_Line2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Line2d>>), c => { var v = default(List<Matrix<Line2d>>); c.CodeList_of_Matrix_of_Line2d__(ref v); return v; } },

            { typeof(Volume<Line2d>), c => { var v = default(Volume<Line2d>); c.CodeVolume_of_Line2d_(ref v); return v; } },
            { typeof(Volume<Line2d>[]), c => { var v = default(Volume<Line2d>[]); c.CodeVolume_of_Line2d_Array(ref v); return v; } },
            { typeof(List<Volume<Line2d>>), c => { var v = default(List<Volume<Line2d>>); c.CodeList_of_Volume_of_Line2d__(ref v); return v; } },

            { typeof(Tensor<Line2d>), c => { var v = default(Tensor<Line2d>); c.CodeTensor_of_Line2d_(ref v); return v; } },
            { typeof(Tensor<Line2d>[]), c => { var v = default(Tensor<Line2d>[]); c.CodeTensor_of_Line2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Line2d>>), c => { var v = default(List<Tensor<Line2d>>); c.CodeList_of_Tensor_of_Line2d__(ref v); return v; } },

            #endregion

            #region Line3d

            { typeof(Line3d), c => { var v = default(Line3d); c.CodeLine3d(ref v); return v; } },
            { typeof(Line3d[]), c => { var v = default(Line3d[]); c.CodeLine3dArray(ref v); return v; } },
            { typeof(List<Line3d>), c => { var v = default(List<Line3d>); c.CodeList_of_Line3d_(ref v); return v; } },

            { typeof(Vector<Line3d>), c => { var v = default(Vector<Line3d>); c.CodeVector_of_Line3d_(ref v); return v; } },
            { typeof(Vector<Line3d>[]), c => { var v = default(Vector<Line3d>[]); c.CodeVector_of_Line3d_Array(ref v); return v; } },
            { typeof(List<Vector<Line3d>>), c => { var v = default(List<Vector<Line3d>>); c.CodeList_of_Vector_of_Line3d__(ref v); return v; } },

            { typeof(Matrix<Line3d>), c => { var v = default(Matrix<Line3d>); c.CodeMatrix_of_Line3d_(ref v); return v; } },
            { typeof(Matrix<Line3d>[]), c => { var v = default(Matrix<Line3d>[]); c.CodeMatrix_of_Line3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Line3d>>), c => { var v = default(List<Matrix<Line3d>>); c.CodeList_of_Matrix_of_Line3d__(ref v); return v; } },

            { typeof(Volume<Line3d>), c => { var v = default(Volume<Line3d>); c.CodeVolume_of_Line3d_(ref v); return v; } },
            { typeof(Volume<Line3d>[]), c => { var v = default(Volume<Line3d>[]); c.CodeVolume_of_Line3d_Array(ref v); return v; } },
            { typeof(List<Volume<Line3d>>), c => { var v = default(List<Volume<Line3d>>); c.CodeList_of_Volume_of_Line3d__(ref v); return v; } },

            { typeof(Tensor<Line3d>), c => { var v = default(Tensor<Line3d>); c.CodeTensor_of_Line3d_(ref v); return v; } },
            { typeof(Tensor<Line3d>[]), c => { var v = default(Tensor<Line3d>[]); c.CodeTensor_of_Line3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Line3d>>), c => { var v = default(List<Tensor<Line3d>>); c.CodeList_of_Tensor_of_Line3d__(ref v); return v; } },

            #endregion

            #region Plane2d

            { typeof(Plane2d), c => { var v = default(Plane2d); c.CodePlane2d(ref v); return v; } },
            { typeof(Plane2d[]), c => { var v = default(Plane2d[]); c.CodePlane2dArray(ref v); return v; } },
            { typeof(List<Plane2d>), c => { var v = default(List<Plane2d>); c.CodeList_of_Plane2d_(ref v); return v; } },

            { typeof(Vector<Plane2d>), c => { var v = default(Vector<Plane2d>); c.CodeVector_of_Plane2d_(ref v); return v; } },
            { typeof(Vector<Plane2d>[]), c => { var v = default(Vector<Plane2d>[]); c.CodeVector_of_Plane2d_Array(ref v); return v; } },
            { typeof(List<Vector<Plane2d>>), c => { var v = default(List<Vector<Plane2d>>); c.CodeList_of_Vector_of_Plane2d__(ref v); return v; } },

            { typeof(Matrix<Plane2d>), c => { var v = default(Matrix<Plane2d>); c.CodeMatrix_of_Plane2d_(ref v); return v; } },
            { typeof(Matrix<Plane2d>[]), c => { var v = default(Matrix<Plane2d>[]); c.CodeMatrix_of_Plane2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Plane2d>>), c => { var v = default(List<Matrix<Plane2d>>); c.CodeList_of_Matrix_of_Plane2d__(ref v); return v; } },

            { typeof(Volume<Plane2d>), c => { var v = default(Volume<Plane2d>); c.CodeVolume_of_Plane2d_(ref v); return v; } },
            { typeof(Volume<Plane2d>[]), c => { var v = default(Volume<Plane2d>[]); c.CodeVolume_of_Plane2d_Array(ref v); return v; } },
            { typeof(List<Volume<Plane2d>>), c => { var v = default(List<Volume<Plane2d>>); c.CodeList_of_Volume_of_Plane2d__(ref v); return v; } },

            { typeof(Tensor<Plane2d>), c => { var v = default(Tensor<Plane2d>); c.CodeTensor_of_Plane2d_(ref v); return v; } },
            { typeof(Tensor<Plane2d>[]), c => { var v = default(Tensor<Plane2d>[]); c.CodeTensor_of_Plane2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Plane2d>>), c => { var v = default(List<Tensor<Plane2d>>); c.CodeList_of_Tensor_of_Plane2d__(ref v); return v; } },

            #endregion

            #region Plane3d

            { typeof(Plane3d), c => { var v = default(Plane3d); c.CodePlane3d(ref v); return v; } },
            { typeof(Plane3d[]), c => { var v = default(Plane3d[]); c.CodePlane3dArray(ref v); return v; } },
            { typeof(List<Plane3d>), c => { var v = default(List<Plane3d>); c.CodeList_of_Plane3d_(ref v); return v; } },

            { typeof(Vector<Plane3d>), c => { var v = default(Vector<Plane3d>); c.CodeVector_of_Plane3d_(ref v); return v; } },
            { typeof(Vector<Plane3d>[]), c => { var v = default(Vector<Plane3d>[]); c.CodeVector_of_Plane3d_Array(ref v); return v; } },
            { typeof(List<Vector<Plane3d>>), c => { var v = default(List<Vector<Plane3d>>); c.CodeList_of_Vector_of_Plane3d__(ref v); return v; } },

            { typeof(Matrix<Plane3d>), c => { var v = default(Matrix<Plane3d>); c.CodeMatrix_of_Plane3d_(ref v); return v; } },
            { typeof(Matrix<Plane3d>[]), c => { var v = default(Matrix<Plane3d>[]); c.CodeMatrix_of_Plane3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Plane3d>>), c => { var v = default(List<Matrix<Plane3d>>); c.CodeList_of_Matrix_of_Plane3d__(ref v); return v; } },

            { typeof(Volume<Plane3d>), c => { var v = default(Volume<Plane3d>); c.CodeVolume_of_Plane3d_(ref v); return v; } },
            { typeof(Volume<Plane3d>[]), c => { var v = default(Volume<Plane3d>[]); c.CodeVolume_of_Plane3d_Array(ref v); return v; } },
            { typeof(List<Volume<Plane3d>>), c => { var v = default(List<Volume<Plane3d>>); c.CodeList_of_Volume_of_Plane3d__(ref v); return v; } },

            { typeof(Tensor<Plane3d>), c => { var v = default(Tensor<Plane3d>); c.CodeTensor_of_Plane3d_(ref v); return v; } },
            { typeof(Tensor<Plane3d>[]), c => { var v = default(Tensor<Plane3d>[]); c.CodeTensor_of_Plane3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Plane3d>>), c => { var v = default(List<Tensor<Plane3d>>); c.CodeList_of_Tensor_of_Plane3d__(ref v); return v; } },

            #endregion

            #region PlaneWithPoint3d

            { typeof(PlaneWithPoint3d), c => { var v = default(PlaneWithPoint3d); c.CodePlaneWithPoint3d(ref v); return v; } },
            { typeof(PlaneWithPoint3d[]), c => { var v = default(PlaneWithPoint3d[]); c.CodePlaneWithPoint3dArray(ref v); return v; } },
            { typeof(List<PlaneWithPoint3d>), c => { var v = default(List<PlaneWithPoint3d>); c.CodeList_of_PlaneWithPoint3d_(ref v); return v; } },

            { typeof(Vector<PlaneWithPoint3d>), c => { var v = default(Vector<PlaneWithPoint3d>); c.CodeVector_of_PlaneWithPoint3d_(ref v); return v; } },
            { typeof(Vector<PlaneWithPoint3d>[]), c => { var v = default(Vector<PlaneWithPoint3d>[]); c.CodeVector_of_PlaneWithPoint3d_Array(ref v); return v; } },
            { typeof(List<Vector<PlaneWithPoint3d>>), c => { var v = default(List<Vector<PlaneWithPoint3d>>); c.CodeList_of_Vector_of_PlaneWithPoint3d__(ref v); return v; } },

            { typeof(Matrix<PlaneWithPoint3d>), c => { var v = default(Matrix<PlaneWithPoint3d>); c.CodeMatrix_of_PlaneWithPoint3d_(ref v); return v; } },
            { typeof(Matrix<PlaneWithPoint3d>[]), c => { var v = default(Matrix<PlaneWithPoint3d>[]); c.CodeMatrix_of_PlaneWithPoint3d_Array(ref v); return v; } },
            { typeof(List<Matrix<PlaneWithPoint3d>>), c => { var v = default(List<Matrix<PlaneWithPoint3d>>); c.CodeList_of_Matrix_of_PlaneWithPoint3d__(ref v); return v; } },

            { typeof(Volume<PlaneWithPoint3d>), c => { var v = default(Volume<PlaneWithPoint3d>); c.CodeVolume_of_PlaneWithPoint3d_(ref v); return v; } },
            { typeof(Volume<PlaneWithPoint3d>[]), c => { var v = default(Volume<PlaneWithPoint3d>[]); c.CodeVolume_of_PlaneWithPoint3d_Array(ref v); return v; } },
            { typeof(List<Volume<PlaneWithPoint3d>>), c => { var v = default(List<Volume<PlaneWithPoint3d>>); c.CodeList_of_Volume_of_PlaneWithPoint3d__(ref v); return v; } },

            { typeof(Tensor<PlaneWithPoint3d>), c => { var v = default(Tensor<PlaneWithPoint3d>); c.CodeTensor_of_PlaneWithPoint3d_(ref v); return v; } },
            { typeof(Tensor<PlaneWithPoint3d>[]), c => { var v = default(Tensor<PlaneWithPoint3d>[]); c.CodeTensor_of_PlaneWithPoint3d_Array(ref v); return v; } },
            { typeof(List<Tensor<PlaneWithPoint3d>>), c => { var v = default(List<Tensor<PlaneWithPoint3d>>); c.CodeList_of_Tensor_of_PlaneWithPoint3d__(ref v); return v; } },

            #endregion

            #region Quad2d

            { typeof(Quad2d), c => { var v = default(Quad2d); c.CodeQuad2d(ref v); return v; } },
            { typeof(Quad2d[]), c => { var v = default(Quad2d[]); c.CodeQuad2dArray(ref v); return v; } },
            { typeof(List<Quad2d>), c => { var v = default(List<Quad2d>); c.CodeList_of_Quad2d_(ref v); return v; } },

            { typeof(Vector<Quad2d>), c => { var v = default(Vector<Quad2d>); c.CodeVector_of_Quad2d_(ref v); return v; } },
            { typeof(Vector<Quad2d>[]), c => { var v = default(Vector<Quad2d>[]); c.CodeVector_of_Quad2d_Array(ref v); return v; } },
            { typeof(List<Vector<Quad2d>>), c => { var v = default(List<Vector<Quad2d>>); c.CodeList_of_Vector_of_Quad2d__(ref v); return v; } },

            { typeof(Matrix<Quad2d>), c => { var v = default(Matrix<Quad2d>); c.CodeMatrix_of_Quad2d_(ref v); return v; } },
            { typeof(Matrix<Quad2d>[]), c => { var v = default(Matrix<Quad2d>[]); c.CodeMatrix_of_Quad2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Quad2d>>), c => { var v = default(List<Matrix<Quad2d>>); c.CodeList_of_Matrix_of_Quad2d__(ref v); return v; } },

            { typeof(Volume<Quad2d>), c => { var v = default(Volume<Quad2d>); c.CodeVolume_of_Quad2d_(ref v); return v; } },
            { typeof(Volume<Quad2d>[]), c => { var v = default(Volume<Quad2d>[]); c.CodeVolume_of_Quad2d_Array(ref v); return v; } },
            { typeof(List<Volume<Quad2d>>), c => { var v = default(List<Volume<Quad2d>>); c.CodeList_of_Volume_of_Quad2d__(ref v); return v; } },

            { typeof(Tensor<Quad2d>), c => { var v = default(Tensor<Quad2d>); c.CodeTensor_of_Quad2d_(ref v); return v; } },
            { typeof(Tensor<Quad2d>[]), c => { var v = default(Tensor<Quad2d>[]); c.CodeTensor_of_Quad2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Quad2d>>), c => { var v = default(List<Tensor<Quad2d>>); c.CodeList_of_Tensor_of_Quad2d__(ref v); return v; } },

            #endregion

            #region Quad3d

            { typeof(Quad3d), c => { var v = default(Quad3d); c.CodeQuad3d(ref v); return v; } },
            { typeof(Quad3d[]), c => { var v = default(Quad3d[]); c.CodeQuad3dArray(ref v); return v; } },
            { typeof(List<Quad3d>), c => { var v = default(List<Quad3d>); c.CodeList_of_Quad3d_(ref v); return v; } },

            { typeof(Vector<Quad3d>), c => { var v = default(Vector<Quad3d>); c.CodeVector_of_Quad3d_(ref v); return v; } },
            { typeof(Vector<Quad3d>[]), c => { var v = default(Vector<Quad3d>[]); c.CodeVector_of_Quad3d_Array(ref v); return v; } },
            { typeof(List<Vector<Quad3d>>), c => { var v = default(List<Vector<Quad3d>>); c.CodeList_of_Vector_of_Quad3d__(ref v); return v; } },

            { typeof(Matrix<Quad3d>), c => { var v = default(Matrix<Quad3d>); c.CodeMatrix_of_Quad3d_(ref v); return v; } },
            { typeof(Matrix<Quad3d>[]), c => { var v = default(Matrix<Quad3d>[]); c.CodeMatrix_of_Quad3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Quad3d>>), c => { var v = default(List<Matrix<Quad3d>>); c.CodeList_of_Matrix_of_Quad3d__(ref v); return v; } },

            { typeof(Volume<Quad3d>), c => { var v = default(Volume<Quad3d>); c.CodeVolume_of_Quad3d_(ref v); return v; } },
            { typeof(Volume<Quad3d>[]), c => { var v = default(Volume<Quad3d>[]); c.CodeVolume_of_Quad3d_Array(ref v); return v; } },
            { typeof(List<Volume<Quad3d>>), c => { var v = default(List<Volume<Quad3d>>); c.CodeList_of_Volume_of_Quad3d__(ref v); return v; } },

            { typeof(Tensor<Quad3d>), c => { var v = default(Tensor<Quad3d>); c.CodeTensor_of_Quad3d_(ref v); return v; } },
            { typeof(Tensor<Quad3d>[]), c => { var v = default(Tensor<Quad3d>[]); c.CodeTensor_of_Quad3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Quad3d>>), c => { var v = default(List<Tensor<Quad3d>>); c.CodeList_of_Tensor_of_Quad3d__(ref v); return v; } },

            #endregion

            #region Ray2d

            { typeof(Ray2d), c => { var v = default(Ray2d); c.CodeRay2d(ref v); return v; } },
            { typeof(Ray2d[]), c => { var v = default(Ray2d[]); c.CodeRay2dArray(ref v); return v; } },
            { typeof(List<Ray2d>), c => { var v = default(List<Ray2d>); c.CodeList_of_Ray2d_(ref v); return v; } },

            { typeof(Vector<Ray2d>), c => { var v = default(Vector<Ray2d>); c.CodeVector_of_Ray2d_(ref v); return v; } },
            { typeof(Vector<Ray2d>[]), c => { var v = default(Vector<Ray2d>[]); c.CodeVector_of_Ray2d_Array(ref v); return v; } },
            { typeof(List<Vector<Ray2d>>), c => { var v = default(List<Vector<Ray2d>>); c.CodeList_of_Vector_of_Ray2d__(ref v); return v; } },

            { typeof(Matrix<Ray2d>), c => { var v = default(Matrix<Ray2d>); c.CodeMatrix_of_Ray2d_(ref v); return v; } },
            { typeof(Matrix<Ray2d>[]), c => { var v = default(Matrix<Ray2d>[]); c.CodeMatrix_of_Ray2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Ray2d>>), c => { var v = default(List<Matrix<Ray2d>>); c.CodeList_of_Matrix_of_Ray2d__(ref v); return v; } },

            { typeof(Volume<Ray2d>), c => { var v = default(Volume<Ray2d>); c.CodeVolume_of_Ray2d_(ref v); return v; } },
            { typeof(Volume<Ray2d>[]), c => { var v = default(Volume<Ray2d>[]); c.CodeVolume_of_Ray2d_Array(ref v); return v; } },
            { typeof(List<Volume<Ray2d>>), c => { var v = default(List<Volume<Ray2d>>); c.CodeList_of_Volume_of_Ray2d__(ref v); return v; } },

            { typeof(Tensor<Ray2d>), c => { var v = default(Tensor<Ray2d>); c.CodeTensor_of_Ray2d_(ref v); return v; } },
            { typeof(Tensor<Ray2d>[]), c => { var v = default(Tensor<Ray2d>[]); c.CodeTensor_of_Ray2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Ray2d>>), c => { var v = default(List<Tensor<Ray2d>>); c.CodeList_of_Tensor_of_Ray2d__(ref v); return v; } },

            #endregion

            #region Ray3d

            { typeof(Ray3d), c => { var v = default(Ray3d); c.CodeRay3d(ref v); return v; } },
            { typeof(Ray3d[]), c => { var v = default(Ray3d[]); c.CodeRay3dArray(ref v); return v; } },
            { typeof(List<Ray3d>), c => { var v = default(List<Ray3d>); c.CodeList_of_Ray3d_(ref v); return v; } },

            { typeof(Vector<Ray3d>), c => { var v = default(Vector<Ray3d>); c.CodeVector_of_Ray3d_(ref v); return v; } },
            { typeof(Vector<Ray3d>[]), c => { var v = default(Vector<Ray3d>[]); c.CodeVector_of_Ray3d_Array(ref v); return v; } },
            { typeof(List<Vector<Ray3d>>), c => { var v = default(List<Vector<Ray3d>>); c.CodeList_of_Vector_of_Ray3d__(ref v); return v; } },

            { typeof(Matrix<Ray3d>), c => { var v = default(Matrix<Ray3d>); c.CodeMatrix_of_Ray3d_(ref v); return v; } },
            { typeof(Matrix<Ray3d>[]), c => { var v = default(Matrix<Ray3d>[]); c.CodeMatrix_of_Ray3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Ray3d>>), c => { var v = default(List<Matrix<Ray3d>>); c.CodeList_of_Matrix_of_Ray3d__(ref v); return v; } },

            { typeof(Volume<Ray3d>), c => { var v = default(Volume<Ray3d>); c.CodeVolume_of_Ray3d_(ref v); return v; } },
            { typeof(Volume<Ray3d>[]), c => { var v = default(Volume<Ray3d>[]); c.CodeVolume_of_Ray3d_Array(ref v); return v; } },
            { typeof(List<Volume<Ray3d>>), c => { var v = default(List<Volume<Ray3d>>); c.CodeList_of_Volume_of_Ray3d__(ref v); return v; } },

            { typeof(Tensor<Ray3d>), c => { var v = default(Tensor<Ray3d>); c.CodeTensor_of_Ray3d_(ref v); return v; } },
            { typeof(Tensor<Ray3d>[]), c => { var v = default(Tensor<Ray3d>[]); c.CodeTensor_of_Ray3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Ray3d>>), c => { var v = default(List<Tensor<Ray3d>>); c.CodeList_of_Tensor_of_Ray3d__(ref v); return v; } },

            #endregion

            #region Sphere3d

            { typeof(Sphere3d), c => { var v = default(Sphere3d); c.CodeSphere3d(ref v); return v; } },
            { typeof(Sphere3d[]), c => { var v = default(Sphere3d[]); c.CodeSphere3dArray(ref v); return v; } },
            { typeof(List<Sphere3d>), c => { var v = default(List<Sphere3d>); c.CodeList_of_Sphere3d_(ref v); return v; } },

            { typeof(Vector<Sphere3d>), c => { var v = default(Vector<Sphere3d>); c.CodeVector_of_Sphere3d_(ref v); return v; } },
            { typeof(Vector<Sphere3d>[]), c => { var v = default(Vector<Sphere3d>[]); c.CodeVector_of_Sphere3d_Array(ref v); return v; } },
            { typeof(List<Vector<Sphere3d>>), c => { var v = default(List<Vector<Sphere3d>>); c.CodeList_of_Vector_of_Sphere3d__(ref v); return v; } },

            { typeof(Matrix<Sphere3d>), c => { var v = default(Matrix<Sphere3d>); c.CodeMatrix_of_Sphere3d_(ref v); return v; } },
            { typeof(Matrix<Sphere3d>[]), c => { var v = default(Matrix<Sphere3d>[]); c.CodeMatrix_of_Sphere3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Sphere3d>>), c => { var v = default(List<Matrix<Sphere3d>>); c.CodeList_of_Matrix_of_Sphere3d__(ref v); return v; } },

            { typeof(Volume<Sphere3d>), c => { var v = default(Volume<Sphere3d>); c.CodeVolume_of_Sphere3d_(ref v); return v; } },
            { typeof(Volume<Sphere3d>[]), c => { var v = default(Volume<Sphere3d>[]); c.CodeVolume_of_Sphere3d_Array(ref v); return v; } },
            { typeof(List<Volume<Sphere3d>>), c => { var v = default(List<Volume<Sphere3d>>); c.CodeList_of_Volume_of_Sphere3d__(ref v); return v; } },

            { typeof(Tensor<Sphere3d>), c => { var v = default(Tensor<Sphere3d>); c.CodeTensor_of_Sphere3d_(ref v); return v; } },
            { typeof(Tensor<Sphere3d>[]), c => { var v = default(Tensor<Sphere3d>[]); c.CodeTensor_of_Sphere3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Sphere3d>>), c => { var v = default(List<Tensor<Sphere3d>>); c.CodeList_of_Tensor_of_Sphere3d__(ref v); return v; } },

            #endregion

            #region Triangle2d

            { typeof(Triangle2d), c => { var v = default(Triangle2d); c.CodeTriangle2d(ref v); return v; } },
            { typeof(Triangle2d[]), c => { var v = default(Triangle2d[]); c.CodeTriangle2dArray(ref v); return v; } },
            { typeof(List<Triangle2d>), c => { var v = default(List<Triangle2d>); c.CodeList_of_Triangle2d_(ref v); return v; } },

            { typeof(Vector<Triangle2d>), c => { var v = default(Vector<Triangle2d>); c.CodeVector_of_Triangle2d_(ref v); return v; } },
            { typeof(Vector<Triangle2d>[]), c => { var v = default(Vector<Triangle2d>[]); c.CodeVector_of_Triangle2d_Array(ref v); return v; } },
            { typeof(List<Vector<Triangle2d>>), c => { var v = default(List<Vector<Triangle2d>>); c.CodeList_of_Vector_of_Triangle2d__(ref v); return v; } },

            { typeof(Matrix<Triangle2d>), c => { var v = default(Matrix<Triangle2d>); c.CodeMatrix_of_Triangle2d_(ref v); return v; } },
            { typeof(Matrix<Triangle2d>[]), c => { var v = default(Matrix<Triangle2d>[]); c.CodeMatrix_of_Triangle2d_Array(ref v); return v; } },
            { typeof(List<Matrix<Triangle2d>>), c => { var v = default(List<Matrix<Triangle2d>>); c.CodeList_of_Matrix_of_Triangle2d__(ref v); return v; } },

            { typeof(Volume<Triangle2d>), c => { var v = default(Volume<Triangle2d>); c.CodeVolume_of_Triangle2d_(ref v); return v; } },
            { typeof(Volume<Triangle2d>[]), c => { var v = default(Volume<Triangle2d>[]); c.CodeVolume_of_Triangle2d_Array(ref v); return v; } },
            { typeof(List<Volume<Triangle2d>>), c => { var v = default(List<Volume<Triangle2d>>); c.CodeList_of_Volume_of_Triangle2d__(ref v); return v; } },

            { typeof(Tensor<Triangle2d>), c => { var v = default(Tensor<Triangle2d>); c.CodeTensor_of_Triangle2d_(ref v); return v; } },
            { typeof(Tensor<Triangle2d>[]), c => { var v = default(Tensor<Triangle2d>[]); c.CodeTensor_of_Triangle2d_Array(ref v); return v; } },
            { typeof(List<Tensor<Triangle2d>>), c => { var v = default(List<Tensor<Triangle2d>>); c.CodeList_of_Tensor_of_Triangle2d__(ref v); return v; } },

            #endregion

            #region Triangle3d

            { typeof(Triangle3d), c => { var v = default(Triangle3d); c.CodeTriangle3d(ref v); return v; } },
            { typeof(Triangle3d[]), c => { var v = default(Triangle3d[]); c.CodeTriangle3dArray(ref v); return v; } },
            { typeof(List<Triangle3d>), c => { var v = default(List<Triangle3d>); c.CodeList_of_Triangle3d_(ref v); return v; } },

            { typeof(Vector<Triangle3d>), c => { var v = default(Vector<Triangle3d>); c.CodeVector_of_Triangle3d_(ref v); return v; } },
            { typeof(Vector<Triangle3d>[]), c => { var v = default(Vector<Triangle3d>[]); c.CodeVector_of_Triangle3d_Array(ref v); return v; } },
            { typeof(List<Vector<Triangle3d>>), c => { var v = default(List<Vector<Triangle3d>>); c.CodeList_of_Vector_of_Triangle3d__(ref v); return v; } },

            { typeof(Matrix<Triangle3d>), c => { var v = default(Matrix<Triangle3d>); c.CodeMatrix_of_Triangle3d_(ref v); return v; } },
            { typeof(Matrix<Triangle3d>[]), c => { var v = default(Matrix<Triangle3d>[]); c.CodeMatrix_of_Triangle3d_Array(ref v); return v; } },
            { typeof(List<Matrix<Triangle3d>>), c => { var v = default(List<Matrix<Triangle3d>>); c.CodeList_of_Matrix_of_Triangle3d__(ref v); return v; } },

            { typeof(Volume<Triangle3d>), c => { var v = default(Volume<Triangle3d>); c.CodeVolume_of_Triangle3d_(ref v); return v; } },
            { typeof(Volume<Triangle3d>[]), c => { var v = default(Volume<Triangle3d>[]); c.CodeVolume_of_Triangle3d_Array(ref v); return v; } },
            { typeof(List<Volume<Triangle3d>>), c => { var v = default(List<Volume<Triangle3d>>); c.CodeList_of_Volume_of_Triangle3d__(ref v); return v; } },

            { typeof(Tensor<Triangle3d>), c => { var v = default(Tensor<Triangle3d>); c.CodeTensor_of_Triangle3d_(ref v); return v; } },
            { typeof(Tensor<Triangle3d>[]), c => { var v = default(Tensor<Triangle3d>[]); c.CodeTensor_of_Triangle3d_Array(ref v); return v; } },
            { typeof(List<Tensor<Triangle3d>>), c => { var v = default(List<Tensor<Triangle3d>>); c.CodeList_of_Tensor_of_Triangle3d__(ref v); return v; } },

            #endregion

            #region Multi-Dimensional Arrays

            { typeof(byte[,]), c => { var v = default(byte[,]); c.CodeByteArray2d(ref v); return v; } },
            { typeof(byte[, ,]), c => { var v = default(byte[, ,]); c.CodeByteArray3d(ref v); return v; } },
            { typeof(sbyte[,]), c => { var v = default(sbyte[,]); c.CodeSByteArray2d(ref v); return v; } },
            { typeof(sbyte[, ,]), c => { var v = default(sbyte[, ,]); c.CodeSByteArray3d(ref v); return v; } },
            { typeof(short[,]), c => { var v = default(short[,]); c.CodeShortArray2d(ref v); return v; } },
            { typeof(short[, ,]), c => { var v = default(short[, ,]); c.CodeShortArray3d(ref v); return v; } },
            { typeof(ushort[,]), c => { var v = default(ushort[,]); c.CodeUShortArray2d(ref v); return v; } },
            { typeof(ushort[, ,]), c => { var v = default(ushort[, ,]); c.CodeUShortArray3d(ref v); return v; } },
            { typeof(int[,]), c => { var v = default(int[,]); c.CodeIntArray2d(ref v); return v; } },
            { typeof(int[, ,]), c => { var v = default(int[, ,]); c.CodeIntArray3d(ref v); return v; } },
            { typeof(uint[,]), c => { var v = default(uint[,]); c.CodeUIntArray2d(ref v); return v; } },
            { typeof(uint[, ,]), c => { var v = default(uint[, ,]); c.CodeUIntArray3d(ref v); return v; } },
            { typeof(long[,]), c => { var v = default(long[,]); c.CodeLongArray2d(ref v); return v; } },
            { typeof(long[, ,]), c => { var v = default(long[, ,]); c.CodeLongArray3d(ref v); return v; } },
            { typeof(ulong[,]), c => { var v = default(ulong[,]); c.CodeULongArray2d(ref v); return v; } },
            { typeof(ulong[, ,]), c => { var v = default(ulong[, ,]); c.CodeULongArray3d(ref v); return v; } },
            { typeof(float[,]), c => { var v = default(float[,]); c.CodeFloatArray2d(ref v); return v; } },
            { typeof(float[, ,]), c => { var v = default(float[, ,]); c.CodeFloatArray3d(ref v); return v; } },
            { typeof(double[,]), c => { var v = default(double[,]); c.CodeDoubleArray2d(ref v); return v; } },
            { typeof(double[, ,]), c => { var v = default(double[, ,]); c.CodeDoubleArray3d(ref v); return v; } },
            { typeof(Fraction[,]), c => { var v = default(Fraction[,]); c.CodeFractionArray2d(ref v); return v; } },
            { typeof(Fraction[, ,]), c => { var v = default(Fraction[, ,]); c.CodeFractionArray3d(ref v); return v; } },
            { typeof(V2i[,]), c => { var v = default(V2i[,]); c.CodeV2iArray2d(ref v); return v; } },
            { typeof(V2i[, ,]), c => { var v = default(V2i[, ,]); c.CodeV2iArray3d(ref v); return v; } },
            { typeof(V2l[,]), c => { var v = default(V2l[,]); c.CodeV2lArray2d(ref v); return v; } },
            { typeof(V2l[, ,]), c => { var v = default(V2l[, ,]); c.CodeV2lArray3d(ref v); return v; } },
            { typeof(V2f[,]), c => { var v = default(V2f[,]); c.CodeV2fArray2d(ref v); return v; } },
            { typeof(V2f[, ,]), c => { var v = default(V2f[, ,]); c.CodeV2fArray3d(ref v); return v; } },
            { typeof(V2d[,]), c => { var v = default(V2d[,]); c.CodeV2dArray2d(ref v); return v; } },
            { typeof(V2d[, ,]), c => { var v = default(V2d[, ,]); c.CodeV2dArray3d(ref v); return v; } },
            { typeof(V3i[,]), c => { var v = default(V3i[,]); c.CodeV3iArray2d(ref v); return v; } },
            { typeof(V3i[, ,]), c => { var v = default(V3i[, ,]); c.CodeV3iArray3d(ref v); return v; } },
            { typeof(V3l[,]), c => { var v = default(V3l[,]); c.CodeV3lArray2d(ref v); return v; } },
            { typeof(V3l[, ,]), c => { var v = default(V3l[, ,]); c.CodeV3lArray3d(ref v); return v; } },
            { typeof(V3f[,]), c => { var v = default(V3f[,]); c.CodeV3fArray2d(ref v); return v; } },
            { typeof(V3f[, ,]), c => { var v = default(V3f[, ,]); c.CodeV3fArray3d(ref v); return v; } },
            { typeof(V3d[,]), c => { var v = default(V3d[,]); c.CodeV3dArray2d(ref v); return v; } },
            { typeof(V3d[, ,]), c => { var v = default(V3d[, ,]); c.CodeV3dArray3d(ref v); return v; } },
            { typeof(V4i[,]), c => { var v = default(V4i[,]); c.CodeV4iArray2d(ref v); return v; } },
            { typeof(V4i[, ,]), c => { var v = default(V4i[, ,]); c.CodeV4iArray3d(ref v); return v; } },
            { typeof(V4l[,]), c => { var v = default(V4l[,]); c.CodeV4lArray2d(ref v); return v; } },
            { typeof(V4l[, ,]), c => { var v = default(V4l[, ,]); c.CodeV4lArray3d(ref v); return v; } },
            { typeof(V4f[,]), c => { var v = default(V4f[,]); c.CodeV4fArray2d(ref v); return v; } },
            { typeof(V4f[, ,]), c => { var v = default(V4f[, ,]); c.CodeV4fArray3d(ref v); return v; } },
            { typeof(V4d[,]), c => { var v = default(V4d[,]); c.CodeV4dArray2d(ref v); return v; } },
            { typeof(V4d[, ,]), c => { var v = default(V4d[, ,]); c.CodeV4dArray3d(ref v); return v; } },
            { typeof(M22i[,]), c => { var v = default(M22i[,]); c.CodeM22iArray2d(ref v); return v; } },
            { typeof(M22i[, ,]), c => { var v = default(M22i[, ,]); c.CodeM22iArray3d(ref v); return v; } },
            { typeof(M22l[,]), c => { var v = default(M22l[,]); c.CodeM22lArray2d(ref v); return v; } },
            { typeof(M22l[, ,]), c => { var v = default(M22l[, ,]); c.CodeM22lArray3d(ref v); return v; } },
            { typeof(M22f[,]), c => { var v = default(M22f[,]); c.CodeM22fArray2d(ref v); return v; } },
            { typeof(M22f[, ,]), c => { var v = default(M22f[, ,]); c.CodeM22fArray3d(ref v); return v; } },
            { typeof(M22d[,]), c => { var v = default(M22d[,]); c.CodeM22dArray2d(ref v); return v; } },
            { typeof(M22d[, ,]), c => { var v = default(M22d[, ,]); c.CodeM22dArray3d(ref v); return v; } },
            { typeof(M23i[,]), c => { var v = default(M23i[,]); c.CodeM23iArray2d(ref v); return v; } },
            { typeof(M23i[, ,]), c => { var v = default(M23i[, ,]); c.CodeM23iArray3d(ref v); return v; } },
            { typeof(M23l[,]), c => { var v = default(M23l[,]); c.CodeM23lArray2d(ref v); return v; } },
            { typeof(M23l[, ,]), c => { var v = default(M23l[, ,]); c.CodeM23lArray3d(ref v); return v; } },
            { typeof(M23f[,]), c => { var v = default(M23f[,]); c.CodeM23fArray2d(ref v); return v; } },
            { typeof(M23f[, ,]), c => { var v = default(M23f[, ,]); c.CodeM23fArray3d(ref v); return v; } },
            { typeof(M23d[,]), c => { var v = default(M23d[,]); c.CodeM23dArray2d(ref v); return v; } },
            { typeof(M23d[, ,]), c => { var v = default(M23d[, ,]); c.CodeM23dArray3d(ref v); return v; } },
            { typeof(M33i[,]), c => { var v = default(M33i[,]); c.CodeM33iArray2d(ref v); return v; } },
            { typeof(M33i[, ,]), c => { var v = default(M33i[, ,]); c.CodeM33iArray3d(ref v); return v; } },
            { typeof(M33l[,]), c => { var v = default(M33l[,]); c.CodeM33lArray2d(ref v); return v; } },
            { typeof(M33l[, ,]), c => { var v = default(M33l[, ,]); c.CodeM33lArray3d(ref v); return v; } },
            { typeof(M33f[,]), c => { var v = default(M33f[,]); c.CodeM33fArray2d(ref v); return v; } },
            { typeof(M33f[, ,]), c => { var v = default(M33f[, ,]); c.CodeM33fArray3d(ref v); return v; } },
            { typeof(M33d[,]), c => { var v = default(M33d[,]); c.CodeM33dArray2d(ref v); return v; } },
            { typeof(M33d[, ,]), c => { var v = default(M33d[, ,]); c.CodeM33dArray3d(ref v); return v; } },
            { typeof(M34i[,]), c => { var v = default(M34i[,]); c.CodeM34iArray2d(ref v); return v; } },
            { typeof(M34i[, ,]), c => { var v = default(M34i[, ,]); c.CodeM34iArray3d(ref v); return v; } },
            { typeof(M34l[,]), c => { var v = default(M34l[,]); c.CodeM34lArray2d(ref v); return v; } },
            { typeof(M34l[, ,]), c => { var v = default(M34l[, ,]); c.CodeM34lArray3d(ref v); return v; } },
            { typeof(M34f[,]), c => { var v = default(M34f[,]); c.CodeM34fArray2d(ref v); return v; } },
            { typeof(M34f[, ,]), c => { var v = default(M34f[, ,]); c.CodeM34fArray3d(ref v); return v; } },
            { typeof(M34d[,]), c => { var v = default(M34d[,]); c.CodeM34dArray2d(ref v); return v; } },
            { typeof(M34d[, ,]), c => { var v = default(M34d[, ,]); c.CodeM34dArray3d(ref v); return v; } },
            { typeof(M44i[,]), c => { var v = default(M44i[,]); c.CodeM44iArray2d(ref v); return v; } },
            { typeof(M44i[, ,]), c => { var v = default(M44i[, ,]); c.CodeM44iArray3d(ref v); return v; } },
            { typeof(M44l[,]), c => { var v = default(M44l[,]); c.CodeM44lArray2d(ref v); return v; } },
            { typeof(M44l[, ,]), c => { var v = default(M44l[, ,]); c.CodeM44lArray3d(ref v); return v; } },
            { typeof(M44f[,]), c => { var v = default(M44f[,]); c.CodeM44fArray2d(ref v); return v; } },
            { typeof(M44f[, ,]), c => { var v = default(M44f[, ,]); c.CodeM44fArray3d(ref v); return v; } },
            { typeof(M44d[,]), c => { var v = default(M44d[,]); c.CodeM44dArray2d(ref v); return v; } },
            { typeof(M44d[, ,]), c => { var v = default(M44d[, ,]); c.CodeM44dArray3d(ref v); return v; } },
            { typeof(C3b[,]), c => { var v = default(C3b[,]); c.CodeC3bArray2d(ref v); return v; } },
            { typeof(C3b[, ,]), c => { var v = default(C3b[, ,]); c.CodeC3bArray3d(ref v); return v; } },
            { typeof(C3us[,]), c => { var v = default(C3us[,]); c.CodeC3usArray2d(ref v); return v; } },
            { typeof(C3us[, ,]), c => { var v = default(C3us[, ,]); c.CodeC3usArray3d(ref v); return v; } },
            { typeof(C3ui[,]), c => { var v = default(C3ui[,]); c.CodeC3uiArray2d(ref v); return v; } },
            { typeof(C3ui[, ,]), c => { var v = default(C3ui[, ,]); c.CodeC3uiArray3d(ref v); return v; } },
            { typeof(C3f[,]), c => { var v = default(C3f[,]); c.CodeC3fArray2d(ref v); return v; } },
            { typeof(C3f[, ,]), c => { var v = default(C3f[, ,]); c.CodeC3fArray3d(ref v); return v; } },
            { typeof(C3d[,]), c => { var v = default(C3d[,]); c.CodeC3dArray2d(ref v); return v; } },
            { typeof(C3d[, ,]), c => { var v = default(C3d[, ,]); c.CodeC3dArray3d(ref v); return v; } },
            { typeof(C4b[,]), c => { var v = default(C4b[,]); c.CodeC4bArray2d(ref v); return v; } },
            { typeof(C4b[, ,]), c => { var v = default(C4b[, ,]); c.CodeC4bArray3d(ref v); return v; } },
            { typeof(C4us[,]), c => { var v = default(C4us[,]); c.CodeC4usArray2d(ref v); return v; } },
            { typeof(C4us[, ,]), c => { var v = default(C4us[, ,]); c.CodeC4usArray3d(ref v); return v; } },
            { typeof(C4ui[,]), c => { var v = default(C4ui[,]); c.CodeC4uiArray2d(ref v); return v; } },
            { typeof(C4ui[, ,]), c => { var v = default(C4ui[, ,]); c.CodeC4uiArray3d(ref v); return v; } },
            { typeof(C4f[,]), c => { var v = default(C4f[,]); c.CodeC4fArray2d(ref v); return v; } },
            { typeof(C4f[, ,]), c => { var v = default(C4f[, ,]); c.CodeC4fArray3d(ref v); return v; } },
            { typeof(C4d[,]), c => { var v = default(C4d[,]); c.CodeC4dArray2d(ref v); return v; } },
            { typeof(C4d[, ,]), c => { var v = default(C4d[, ,]); c.CodeC4dArray3d(ref v); return v; } },
            { typeof(Range1b[,]), c => { var v = default(Range1b[,]); c.CodeRange1bArray2d(ref v); return v; } },
            { typeof(Range1b[, ,]), c => { var v = default(Range1b[, ,]); c.CodeRange1bArray3d(ref v); return v; } },
            { typeof(Range1sb[,]), c => { var v = default(Range1sb[,]); c.CodeRange1sbArray2d(ref v); return v; } },
            { typeof(Range1sb[, ,]), c => { var v = default(Range1sb[, ,]); c.CodeRange1sbArray3d(ref v); return v; } },
            { typeof(Range1s[,]), c => { var v = default(Range1s[,]); c.CodeRange1sArray2d(ref v); return v; } },
            { typeof(Range1s[, ,]), c => { var v = default(Range1s[, ,]); c.CodeRange1sArray3d(ref v); return v; } },
            { typeof(Range1us[,]), c => { var v = default(Range1us[,]); c.CodeRange1usArray2d(ref v); return v; } },
            { typeof(Range1us[, ,]), c => { var v = default(Range1us[, ,]); c.CodeRange1usArray3d(ref v); return v; } },
            { typeof(Range1i[,]), c => { var v = default(Range1i[,]); c.CodeRange1iArray2d(ref v); return v; } },
            { typeof(Range1i[, ,]), c => { var v = default(Range1i[, ,]); c.CodeRange1iArray3d(ref v); return v; } },
            { typeof(Range1ui[,]), c => { var v = default(Range1ui[,]); c.CodeRange1uiArray2d(ref v); return v; } },
            { typeof(Range1ui[, ,]), c => { var v = default(Range1ui[, ,]); c.CodeRange1uiArray3d(ref v); return v; } },
            { typeof(Range1l[,]), c => { var v = default(Range1l[,]); c.CodeRange1lArray2d(ref v); return v; } },
            { typeof(Range1l[, ,]), c => { var v = default(Range1l[, ,]); c.CodeRange1lArray3d(ref v); return v; } },
            { typeof(Range1ul[,]), c => { var v = default(Range1ul[,]); c.CodeRange1ulArray2d(ref v); return v; } },
            { typeof(Range1ul[, ,]), c => { var v = default(Range1ul[, ,]); c.CodeRange1ulArray3d(ref v); return v; } },
            { typeof(Range1f[,]), c => { var v = default(Range1f[,]); c.CodeRange1fArray2d(ref v); return v; } },
            { typeof(Range1f[, ,]), c => { var v = default(Range1f[, ,]); c.CodeRange1fArray3d(ref v); return v; } },
            { typeof(Range1d[,]), c => { var v = default(Range1d[,]); c.CodeRange1dArray2d(ref v); return v; } },
            { typeof(Range1d[, ,]), c => { var v = default(Range1d[, ,]); c.CodeRange1dArray3d(ref v); return v; } },
            { typeof(Box2i[,]), c => { var v = default(Box2i[,]); c.CodeBox2iArray2d(ref v); return v; } },
            { typeof(Box2i[, ,]), c => { var v = default(Box2i[, ,]); c.CodeBox2iArray3d(ref v); return v; } },
            { typeof(Box2l[,]), c => { var v = default(Box2l[,]); c.CodeBox2lArray2d(ref v); return v; } },
            { typeof(Box2l[, ,]), c => { var v = default(Box2l[, ,]); c.CodeBox2lArray3d(ref v); return v; } },
            { typeof(Box2f[,]), c => { var v = default(Box2f[,]); c.CodeBox2fArray2d(ref v); return v; } },
            { typeof(Box2f[, ,]), c => { var v = default(Box2f[, ,]); c.CodeBox2fArray3d(ref v); return v; } },
            { typeof(Box2d[,]), c => { var v = default(Box2d[,]); c.CodeBox2dArray2d(ref v); return v; } },
            { typeof(Box2d[, ,]), c => { var v = default(Box2d[, ,]); c.CodeBox2dArray3d(ref v); return v; } },
            { typeof(Box3i[,]), c => { var v = default(Box3i[,]); c.CodeBox3iArray2d(ref v); return v; } },
            { typeof(Box3i[, ,]), c => { var v = default(Box3i[, ,]); c.CodeBox3iArray3d(ref v); return v; } },
            { typeof(Box3l[,]), c => { var v = default(Box3l[,]); c.CodeBox3lArray2d(ref v); return v; } },
            { typeof(Box3l[, ,]), c => { var v = default(Box3l[, ,]); c.CodeBox3lArray3d(ref v); return v; } },
            { typeof(Box3f[,]), c => { var v = default(Box3f[,]); c.CodeBox3fArray2d(ref v); return v; } },
            { typeof(Box3f[, ,]), c => { var v = default(Box3f[, ,]); c.CodeBox3fArray3d(ref v); return v; } },
            { typeof(Box3d[,]), c => { var v = default(Box3d[,]); c.CodeBox3dArray2d(ref v); return v; } },
            { typeof(Box3d[, ,]), c => { var v = default(Box3d[, ,]); c.CodeBox3dArray3d(ref v); return v; } },
            { typeof(Euclidean3f[,]), c => { var v = default(Euclidean3f[,]); c.CodeEuclidean3fArray2d(ref v); return v; } },
            { typeof(Euclidean3f[, ,]), c => { var v = default(Euclidean3f[, ,]); c.CodeEuclidean3fArray3d(ref v); return v; } },
            { typeof(Euclidean3d[,]), c => { var v = default(Euclidean3d[,]); c.CodeEuclidean3dArray2d(ref v); return v; } },
            { typeof(Euclidean3d[, ,]), c => { var v = default(Euclidean3d[, ,]); c.CodeEuclidean3dArray3d(ref v); return v; } },
            { typeof(Rot2f[,]), c => { var v = default(Rot2f[,]); c.CodeRot2fArray2d(ref v); return v; } },
            { typeof(Rot2f[, ,]), c => { var v = default(Rot2f[, ,]); c.CodeRot2fArray3d(ref v); return v; } },
            { typeof(Rot2d[,]), c => { var v = default(Rot2d[,]); c.CodeRot2dArray2d(ref v); return v; } },
            { typeof(Rot2d[, ,]), c => { var v = default(Rot2d[, ,]); c.CodeRot2dArray3d(ref v); return v; } },
            { typeof(Rot3f[,]), c => { var v = default(Rot3f[,]); c.CodeRot3fArray2d(ref v); return v; } },
            { typeof(Rot3f[, ,]), c => { var v = default(Rot3f[, ,]); c.CodeRot3fArray3d(ref v); return v; } },
            { typeof(Rot3d[,]), c => { var v = default(Rot3d[,]); c.CodeRot3dArray2d(ref v); return v; } },
            { typeof(Rot3d[, ,]), c => { var v = default(Rot3d[, ,]); c.CodeRot3dArray3d(ref v); return v; } },
            { typeof(Scale3f[,]), c => { var v = default(Scale3f[,]); c.CodeScale3fArray2d(ref v); return v; } },
            { typeof(Scale3f[, ,]), c => { var v = default(Scale3f[, ,]); c.CodeScale3fArray3d(ref v); return v; } },
            { typeof(Scale3d[,]), c => { var v = default(Scale3d[,]); c.CodeScale3dArray2d(ref v); return v; } },
            { typeof(Scale3d[, ,]), c => { var v = default(Scale3d[, ,]); c.CodeScale3dArray3d(ref v); return v; } },
            { typeof(Shift3f[,]), c => { var v = default(Shift3f[,]); c.CodeShift3fArray2d(ref v); return v; } },
            { typeof(Shift3f[, ,]), c => { var v = default(Shift3f[, ,]); c.CodeShift3fArray3d(ref v); return v; } },
            { typeof(Shift3d[,]), c => { var v = default(Shift3d[,]); c.CodeShift3dArray2d(ref v); return v; } },
            { typeof(Shift3d[, ,]), c => { var v = default(Shift3d[, ,]); c.CodeShift3dArray3d(ref v); return v; } },
            { typeof(Trafo2f[,]), c => { var v = default(Trafo2f[,]); c.CodeTrafo2fArray2d(ref v); return v; } },
            { typeof(Trafo2f[, ,]), c => { var v = default(Trafo2f[, ,]); c.CodeTrafo2fArray3d(ref v); return v; } },
            { typeof(Trafo2d[,]), c => { var v = default(Trafo2d[,]); c.CodeTrafo2dArray2d(ref v); return v; } },
            { typeof(Trafo2d[, ,]), c => { var v = default(Trafo2d[, ,]); c.CodeTrafo2dArray3d(ref v); return v; } },
            { typeof(Trafo3f[,]), c => { var v = default(Trafo3f[,]); c.CodeTrafo3fArray2d(ref v); return v; } },
            { typeof(Trafo3f[, ,]), c => { var v = default(Trafo3f[, ,]); c.CodeTrafo3fArray3d(ref v); return v; } },
            { typeof(Trafo3d[,]), c => { var v = default(Trafo3d[,]); c.CodeTrafo3dArray2d(ref v); return v; } },
            { typeof(Trafo3d[, ,]), c => { var v = default(Trafo3d[, ,]); c.CodeTrafo3dArray3d(ref v); return v; } },

            #endregion

            #region Jagged Multi-Dimensional Arrays

            { typeof(byte[][]), c => { var v = default(byte[][]); c.CodeByteArrayArray(ref v); return v; } },
            { typeof(byte[][][]), c => { var v = default(byte[][][]); c.CodeByteArrayArrayArray(ref v); return v; } },
            { typeof(sbyte[][]), c => { var v = default(sbyte[][]); c.CodeSByteArrayArray(ref v); return v; } },
            { typeof(sbyte[][][]), c => { var v = default(sbyte[][][]); c.CodeSByteArrayArrayArray(ref v); return v; } },
            { typeof(short[][]), c => { var v = default(short[][]); c.CodeShortArrayArray(ref v); return v; } },
            { typeof(short[][][]), c => { var v = default(short[][][]); c.CodeShortArrayArrayArray(ref v); return v; } },
            { typeof(ushort[][]), c => { var v = default(ushort[][]); c.CodeUShortArrayArray(ref v); return v; } },
            { typeof(ushort[][][]), c => { var v = default(ushort[][][]); c.CodeUShortArrayArrayArray(ref v); return v; } },
            { typeof(int[][]), c => { var v = default(int[][]); c.CodeIntArrayArray(ref v); return v; } },
            { typeof(int[][][]), c => { var v = default(int[][][]); c.CodeIntArrayArrayArray(ref v); return v; } },
            { typeof(uint[][]), c => { var v = default(uint[][]); c.CodeUIntArrayArray(ref v); return v; } },
            { typeof(uint[][][]), c => { var v = default(uint[][][]); c.CodeUIntArrayArrayArray(ref v); return v; } },
            { typeof(long[][]), c => { var v = default(long[][]); c.CodeLongArrayArray(ref v); return v; } },
            { typeof(long[][][]), c => { var v = default(long[][][]); c.CodeLongArrayArrayArray(ref v); return v; } },
            { typeof(ulong[][]), c => { var v = default(ulong[][]); c.CodeULongArrayArray(ref v); return v; } },
            { typeof(ulong[][][]), c => { var v = default(ulong[][][]); c.CodeULongArrayArrayArray(ref v); return v; } },
            { typeof(float[][]), c => { var v = default(float[][]); c.CodeFloatArrayArray(ref v); return v; } },
            { typeof(float[][][]), c => { var v = default(float[][][]); c.CodeFloatArrayArrayArray(ref v); return v; } },
            { typeof(double[][]), c => { var v = default(double[][]); c.CodeDoubleArrayArray(ref v); return v; } },
            { typeof(double[][][]), c => { var v = default(double[][][]); c.CodeDoubleArrayArrayArray(ref v); return v; } },
            { typeof(Fraction[][]), c => { var v = default(Fraction[][]); c.CodeFractionArrayArray(ref v); return v; } },
            { typeof(Fraction[][][]), c => { var v = default(Fraction[][][]); c.CodeFractionArrayArrayArray(ref v); return v; } },
            { typeof(V2i[][]), c => { var v = default(V2i[][]); c.CodeV2iArrayArray(ref v); return v; } },
            { typeof(V2i[][][]), c => { var v = default(V2i[][][]); c.CodeV2iArrayArrayArray(ref v); return v; } },
            { typeof(V2l[][]), c => { var v = default(V2l[][]); c.CodeV2lArrayArray(ref v); return v; } },
            { typeof(V2l[][][]), c => { var v = default(V2l[][][]); c.CodeV2lArrayArrayArray(ref v); return v; } },
            { typeof(V2f[][]), c => { var v = default(V2f[][]); c.CodeV2fArrayArray(ref v); return v; } },
            { typeof(V2f[][][]), c => { var v = default(V2f[][][]); c.CodeV2fArrayArrayArray(ref v); return v; } },
            { typeof(V2d[][]), c => { var v = default(V2d[][]); c.CodeV2dArrayArray(ref v); return v; } },
            { typeof(V2d[][][]), c => { var v = default(V2d[][][]); c.CodeV2dArrayArrayArray(ref v); return v; } },
            { typeof(V3i[][]), c => { var v = default(V3i[][]); c.CodeV3iArrayArray(ref v); return v; } },
            { typeof(V3i[][][]), c => { var v = default(V3i[][][]); c.CodeV3iArrayArrayArray(ref v); return v; } },
            { typeof(V3l[][]), c => { var v = default(V3l[][]); c.CodeV3lArrayArray(ref v); return v; } },
            { typeof(V3l[][][]), c => { var v = default(V3l[][][]); c.CodeV3lArrayArrayArray(ref v); return v; } },
            { typeof(V3f[][]), c => { var v = default(V3f[][]); c.CodeV3fArrayArray(ref v); return v; } },
            { typeof(V3f[][][]), c => { var v = default(V3f[][][]); c.CodeV3fArrayArrayArray(ref v); return v; } },
            { typeof(V3d[][]), c => { var v = default(V3d[][]); c.CodeV3dArrayArray(ref v); return v; } },
            { typeof(V3d[][][]), c => { var v = default(V3d[][][]); c.CodeV3dArrayArrayArray(ref v); return v; } },
            { typeof(V4i[][]), c => { var v = default(V4i[][]); c.CodeV4iArrayArray(ref v); return v; } },
            { typeof(V4i[][][]), c => { var v = default(V4i[][][]); c.CodeV4iArrayArrayArray(ref v); return v; } },
            { typeof(V4l[][]), c => { var v = default(V4l[][]); c.CodeV4lArrayArray(ref v); return v; } },
            { typeof(V4l[][][]), c => { var v = default(V4l[][][]); c.CodeV4lArrayArrayArray(ref v); return v; } },
            { typeof(V4f[][]), c => { var v = default(V4f[][]); c.CodeV4fArrayArray(ref v); return v; } },
            { typeof(V4f[][][]), c => { var v = default(V4f[][][]); c.CodeV4fArrayArrayArray(ref v); return v; } },
            { typeof(V4d[][]), c => { var v = default(V4d[][]); c.CodeV4dArrayArray(ref v); return v; } },
            { typeof(V4d[][][]), c => { var v = default(V4d[][][]); c.CodeV4dArrayArrayArray(ref v); return v; } },
            { typeof(M22i[][]), c => { var v = default(M22i[][]); c.CodeM22iArrayArray(ref v); return v; } },
            { typeof(M22i[][][]), c => { var v = default(M22i[][][]); c.CodeM22iArrayArrayArray(ref v); return v; } },
            { typeof(M22l[][]), c => { var v = default(M22l[][]); c.CodeM22lArrayArray(ref v); return v; } },
            { typeof(M22l[][][]), c => { var v = default(M22l[][][]); c.CodeM22lArrayArrayArray(ref v); return v; } },
            { typeof(M22f[][]), c => { var v = default(M22f[][]); c.CodeM22fArrayArray(ref v); return v; } },
            { typeof(M22f[][][]), c => { var v = default(M22f[][][]); c.CodeM22fArrayArrayArray(ref v); return v; } },
            { typeof(M22d[][]), c => { var v = default(M22d[][]); c.CodeM22dArrayArray(ref v); return v; } },
            { typeof(M22d[][][]), c => { var v = default(M22d[][][]); c.CodeM22dArrayArrayArray(ref v); return v; } },
            { typeof(M23i[][]), c => { var v = default(M23i[][]); c.CodeM23iArrayArray(ref v); return v; } },
            { typeof(M23i[][][]), c => { var v = default(M23i[][][]); c.CodeM23iArrayArrayArray(ref v); return v; } },
            { typeof(M23l[][]), c => { var v = default(M23l[][]); c.CodeM23lArrayArray(ref v); return v; } },
            { typeof(M23l[][][]), c => { var v = default(M23l[][][]); c.CodeM23lArrayArrayArray(ref v); return v; } },
            { typeof(M23f[][]), c => { var v = default(M23f[][]); c.CodeM23fArrayArray(ref v); return v; } },
            { typeof(M23f[][][]), c => { var v = default(M23f[][][]); c.CodeM23fArrayArrayArray(ref v); return v; } },
            { typeof(M23d[][]), c => { var v = default(M23d[][]); c.CodeM23dArrayArray(ref v); return v; } },
            { typeof(M23d[][][]), c => { var v = default(M23d[][][]); c.CodeM23dArrayArrayArray(ref v); return v; } },
            { typeof(M33i[][]), c => { var v = default(M33i[][]); c.CodeM33iArrayArray(ref v); return v; } },
            { typeof(M33i[][][]), c => { var v = default(M33i[][][]); c.CodeM33iArrayArrayArray(ref v); return v; } },
            { typeof(M33l[][]), c => { var v = default(M33l[][]); c.CodeM33lArrayArray(ref v); return v; } },
            { typeof(M33l[][][]), c => { var v = default(M33l[][][]); c.CodeM33lArrayArrayArray(ref v); return v; } },
            { typeof(M33f[][]), c => { var v = default(M33f[][]); c.CodeM33fArrayArray(ref v); return v; } },
            { typeof(M33f[][][]), c => { var v = default(M33f[][][]); c.CodeM33fArrayArrayArray(ref v); return v; } },
            { typeof(M33d[][]), c => { var v = default(M33d[][]); c.CodeM33dArrayArray(ref v); return v; } },
            { typeof(M33d[][][]), c => { var v = default(M33d[][][]); c.CodeM33dArrayArrayArray(ref v); return v; } },
            { typeof(M34i[][]), c => { var v = default(M34i[][]); c.CodeM34iArrayArray(ref v); return v; } },
            { typeof(M34i[][][]), c => { var v = default(M34i[][][]); c.CodeM34iArrayArrayArray(ref v); return v; } },
            { typeof(M34l[][]), c => { var v = default(M34l[][]); c.CodeM34lArrayArray(ref v); return v; } },
            { typeof(M34l[][][]), c => { var v = default(M34l[][][]); c.CodeM34lArrayArrayArray(ref v); return v; } },
            { typeof(M34f[][]), c => { var v = default(M34f[][]); c.CodeM34fArrayArray(ref v); return v; } },
            { typeof(M34f[][][]), c => { var v = default(M34f[][][]); c.CodeM34fArrayArrayArray(ref v); return v; } },
            { typeof(M34d[][]), c => { var v = default(M34d[][]); c.CodeM34dArrayArray(ref v); return v; } },
            { typeof(M34d[][][]), c => { var v = default(M34d[][][]); c.CodeM34dArrayArrayArray(ref v); return v; } },
            { typeof(M44i[][]), c => { var v = default(M44i[][]); c.CodeM44iArrayArray(ref v); return v; } },
            { typeof(M44i[][][]), c => { var v = default(M44i[][][]); c.CodeM44iArrayArrayArray(ref v); return v; } },
            { typeof(M44l[][]), c => { var v = default(M44l[][]); c.CodeM44lArrayArray(ref v); return v; } },
            { typeof(M44l[][][]), c => { var v = default(M44l[][][]); c.CodeM44lArrayArrayArray(ref v); return v; } },
            { typeof(M44f[][]), c => { var v = default(M44f[][]); c.CodeM44fArrayArray(ref v); return v; } },
            { typeof(M44f[][][]), c => { var v = default(M44f[][][]); c.CodeM44fArrayArrayArray(ref v); return v; } },
            { typeof(M44d[][]), c => { var v = default(M44d[][]); c.CodeM44dArrayArray(ref v); return v; } },
            { typeof(M44d[][][]), c => { var v = default(M44d[][][]); c.CodeM44dArrayArrayArray(ref v); return v; } },
            { typeof(C3b[][]), c => { var v = default(C3b[][]); c.CodeC3bArrayArray(ref v); return v; } },
            { typeof(C3b[][][]), c => { var v = default(C3b[][][]); c.CodeC3bArrayArrayArray(ref v); return v; } },
            { typeof(C3us[][]), c => { var v = default(C3us[][]); c.CodeC3usArrayArray(ref v); return v; } },
            { typeof(C3us[][][]), c => { var v = default(C3us[][][]); c.CodeC3usArrayArrayArray(ref v); return v; } },
            { typeof(C3ui[][]), c => { var v = default(C3ui[][]); c.CodeC3uiArrayArray(ref v); return v; } },
            { typeof(C3ui[][][]), c => { var v = default(C3ui[][][]); c.CodeC3uiArrayArrayArray(ref v); return v; } },
            { typeof(C3f[][]), c => { var v = default(C3f[][]); c.CodeC3fArrayArray(ref v); return v; } },
            { typeof(C3f[][][]), c => { var v = default(C3f[][][]); c.CodeC3fArrayArrayArray(ref v); return v; } },
            { typeof(C3d[][]), c => { var v = default(C3d[][]); c.CodeC3dArrayArray(ref v); return v; } },
            { typeof(C3d[][][]), c => { var v = default(C3d[][][]); c.CodeC3dArrayArrayArray(ref v); return v; } },
            { typeof(C4b[][]), c => { var v = default(C4b[][]); c.CodeC4bArrayArray(ref v); return v; } },
            { typeof(C4b[][][]), c => { var v = default(C4b[][][]); c.CodeC4bArrayArrayArray(ref v); return v; } },
            { typeof(C4us[][]), c => { var v = default(C4us[][]); c.CodeC4usArrayArray(ref v); return v; } },
            { typeof(C4us[][][]), c => { var v = default(C4us[][][]); c.CodeC4usArrayArrayArray(ref v); return v; } },
            { typeof(C4ui[][]), c => { var v = default(C4ui[][]); c.CodeC4uiArrayArray(ref v); return v; } },
            { typeof(C4ui[][][]), c => { var v = default(C4ui[][][]); c.CodeC4uiArrayArrayArray(ref v); return v; } },
            { typeof(C4f[][]), c => { var v = default(C4f[][]); c.CodeC4fArrayArray(ref v); return v; } },
            { typeof(C4f[][][]), c => { var v = default(C4f[][][]); c.CodeC4fArrayArrayArray(ref v); return v; } },
            { typeof(C4d[][]), c => { var v = default(C4d[][]); c.CodeC4dArrayArray(ref v); return v; } },
            { typeof(C4d[][][]), c => { var v = default(C4d[][][]); c.CodeC4dArrayArrayArray(ref v); return v; } },
            { typeof(Range1b[][]), c => { var v = default(Range1b[][]); c.CodeRange1bArrayArray(ref v); return v; } },
            { typeof(Range1b[][][]), c => { var v = default(Range1b[][][]); c.CodeRange1bArrayArrayArray(ref v); return v; } },
            { typeof(Range1sb[][]), c => { var v = default(Range1sb[][]); c.CodeRange1sbArrayArray(ref v); return v; } },
            { typeof(Range1sb[][][]), c => { var v = default(Range1sb[][][]); c.CodeRange1sbArrayArrayArray(ref v); return v; } },
            { typeof(Range1s[][]), c => { var v = default(Range1s[][]); c.CodeRange1sArrayArray(ref v); return v; } },
            { typeof(Range1s[][][]), c => { var v = default(Range1s[][][]); c.CodeRange1sArrayArrayArray(ref v); return v; } },
            { typeof(Range1us[][]), c => { var v = default(Range1us[][]); c.CodeRange1usArrayArray(ref v); return v; } },
            { typeof(Range1us[][][]), c => { var v = default(Range1us[][][]); c.CodeRange1usArrayArrayArray(ref v); return v; } },
            { typeof(Range1i[][]), c => { var v = default(Range1i[][]); c.CodeRange1iArrayArray(ref v); return v; } },
            { typeof(Range1i[][][]), c => { var v = default(Range1i[][][]); c.CodeRange1iArrayArrayArray(ref v); return v; } },
            { typeof(Range1ui[][]), c => { var v = default(Range1ui[][]); c.CodeRange1uiArrayArray(ref v); return v; } },
            { typeof(Range1ui[][][]), c => { var v = default(Range1ui[][][]); c.CodeRange1uiArrayArrayArray(ref v); return v; } },
            { typeof(Range1l[][]), c => { var v = default(Range1l[][]); c.CodeRange1lArrayArray(ref v); return v; } },
            { typeof(Range1l[][][]), c => { var v = default(Range1l[][][]); c.CodeRange1lArrayArrayArray(ref v); return v; } },
            { typeof(Range1ul[][]), c => { var v = default(Range1ul[][]); c.CodeRange1ulArrayArray(ref v); return v; } },
            { typeof(Range1ul[][][]), c => { var v = default(Range1ul[][][]); c.CodeRange1ulArrayArrayArray(ref v); return v; } },
            { typeof(Range1f[][]), c => { var v = default(Range1f[][]); c.CodeRange1fArrayArray(ref v); return v; } },
            { typeof(Range1f[][][]), c => { var v = default(Range1f[][][]); c.CodeRange1fArrayArrayArray(ref v); return v; } },
            { typeof(Range1d[][]), c => { var v = default(Range1d[][]); c.CodeRange1dArrayArray(ref v); return v; } },
            { typeof(Range1d[][][]), c => { var v = default(Range1d[][][]); c.CodeRange1dArrayArrayArray(ref v); return v; } },
            { typeof(Box2i[][]), c => { var v = default(Box2i[][]); c.CodeBox2iArrayArray(ref v); return v; } },
            { typeof(Box2i[][][]), c => { var v = default(Box2i[][][]); c.CodeBox2iArrayArrayArray(ref v); return v; } },
            { typeof(Box2l[][]), c => { var v = default(Box2l[][]); c.CodeBox2lArrayArray(ref v); return v; } },
            { typeof(Box2l[][][]), c => { var v = default(Box2l[][][]); c.CodeBox2lArrayArrayArray(ref v); return v; } },
            { typeof(Box2f[][]), c => { var v = default(Box2f[][]); c.CodeBox2fArrayArray(ref v); return v; } },
            { typeof(Box2f[][][]), c => { var v = default(Box2f[][][]); c.CodeBox2fArrayArrayArray(ref v); return v; } },
            { typeof(Box2d[][]), c => { var v = default(Box2d[][]); c.CodeBox2dArrayArray(ref v); return v; } },
            { typeof(Box2d[][][]), c => { var v = default(Box2d[][][]); c.CodeBox2dArrayArrayArray(ref v); return v; } },
            { typeof(Box3i[][]), c => { var v = default(Box3i[][]); c.CodeBox3iArrayArray(ref v); return v; } },
            { typeof(Box3i[][][]), c => { var v = default(Box3i[][][]); c.CodeBox3iArrayArrayArray(ref v); return v; } },
            { typeof(Box3l[][]), c => { var v = default(Box3l[][]); c.CodeBox3lArrayArray(ref v); return v; } },
            { typeof(Box3l[][][]), c => { var v = default(Box3l[][][]); c.CodeBox3lArrayArrayArray(ref v); return v; } },
            { typeof(Box3f[][]), c => { var v = default(Box3f[][]); c.CodeBox3fArrayArray(ref v); return v; } },
            { typeof(Box3f[][][]), c => { var v = default(Box3f[][][]); c.CodeBox3fArrayArrayArray(ref v); return v; } },
            { typeof(Box3d[][]), c => { var v = default(Box3d[][]); c.CodeBox3dArrayArray(ref v); return v; } },
            { typeof(Box3d[][][]), c => { var v = default(Box3d[][][]); c.CodeBox3dArrayArrayArray(ref v); return v; } },
            { typeof(Euclidean3f[][]), c => { var v = default(Euclidean3f[][]); c.CodeEuclidean3fArrayArray(ref v); return v; } },
            { typeof(Euclidean3f[][][]), c => { var v = default(Euclidean3f[][][]); c.CodeEuclidean3fArrayArrayArray(ref v); return v; } },
            { typeof(Euclidean3d[][]), c => { var v = default(Euclidean3d[][]); c.CodeEuclidean3dArrayArray(ref v); return v; } },
            { typeof(Euclidean3d[][][]), c => { var v = default(Euclidean3d[][][]); c.CodeEuclidean3dArrayArrayArray(ref v); return v; } },
            { typeof(Rot2f[][]), c => { var v = default(Rot2f[][]); c.CodeRot2fArrayArray(ref v); return v; } },
            { typeof(Rot2f[][][]), c => { var v = default(Rot2f[][][]); c.CodeRot2fArrayArrayArray(ref v); return v; } },
            { typeof(Rot2d[][]), c => { var v = default(Rot2d[][]); c.CodeRot2dArrayArray(ref v); return v; } },
            { typeof(Rot2d[][][]), c => { var v = default(Rot2d[][][]); c.CodeRot2dArrayArrayArray(ref v); return v; } },
            { typeof(Rot3f[][]), c => { var v = default(Rot3f[][]); c.CodeRot3fArrayArray(ref v); return v; } },
            { typeof(Rot3f[][][]), c => { var v = default(Rot3f[][][]); c.CodeRot3fArrayArrayArray(ref v); return v; } },
            { typeof(Rot3d[][]), c => { var v = default(Rot3d[][]); c.CodeRot3dArrayArray(ref v); return v; } },
            { typeof(Rot3d[][][]), c => { var v = default(Rot3d[][][]); c.CodeRot3dArrayArrayArray(ref v); return v; } },
            { typeof(Scale3f[][]), c => { var v = default(Scale3f[][]); c.CodeScale3fArrayArray(ref v); return v; } },
            { typeof(Scale3f[][][]), c => { var v = default(Scale3f[][][]); c.CodeScale3fArrayArrayArray(ref v); return v; } },
            { typeof(Scale3d[][]), c => { var v = default(Scale3d[][]); c.CodeScale3dArrayArray(ref v); return v; } },
            { typeof(Scale3d[][][]), c => { var v = default(Scale3d[][][]); c.CodeScale3dArrayArrayArray(ref v); return v; } },
            { typeof(Shift3f[][]), c => { var v = default(Shift3f[][]); c.CodeShift3fArrayArray(ref v); return v; } },
            { typeof(Shift3f[][][]), c => { var v = default(Shift3f[][][]); c.CodeShift3fArrayArrayArray(ref v); return v; } },
            { typeof(Shift3d[][]), c => { var v = default(Shift3d[][]); c.CodeShift3dArrayArray(ref v); return v; } },
            { typeof(Shift3d[][][]), c => { var v = default(Shift3d[][][]); c.CodeShift3dArrayArrayArray(ref v); return v; } },
            { typeof(Trafo2f[][]), c => { var v = default(Trafo2f[][]); c.CodeTrafo2fArrayArray(ref v); return v; } },
            { typeof(Trafo2f[][][]), c => { var v = default(Trafo2f[][][]); c.CodeTrafo2fArrayArrayArray(ref v); return v; } },
            { typeof(Trafo2d[][]), c => { var v = default(Trafo2d[][]); c.CodeTrafo2dArrayArray(ref v); return v; } },
            { typeof(Trafo2d[][][]), c => { var v = default(Trafo2d[][][]); c.CodeTrafo2dArrayArrayArray(ref v); return v; } },
            { typeof(Trafo3f[][]), c => { var v = default(Trafo3f[][]); c.CodeTrafo3fArrayArray(ref v); return v; } },
            { typeof(Trafo3f[][][]), c => { var v = default(Trafo3f[][][]); c.CodeTrafo3fArrayArrayArray(ref v); return v; } },
            { typeof(Trafo3d[][]), c => { var v = default(Trafo3d[][]); c.CodeTrafo3dArrayArray(ref v); return v; } },
            { typeof(Trafo3d[][][]), c => { var v = default(Trafo3d[][][]); c.CodeTrafo3dArrayArrayArray(ref v); return v; } },

            #endregion

            #region Other Types

            { typeof(IntSet), c => { var v = default(IntSet); c.CodeIntSet(ref v); return v; } },
            { typeof(SymbolSet), c => { var v = default(SymbolSet); c.CodeSymbolSet(ref v); return v; } },

            { typeof(HashSet<string>), c => { var v = default(HashSet<string>); c.CodeHashSet_of_T_(ref v); return v; } },

            #endregion
        };

        public static partial class Default
        {
            /// <summary>
            /// The default way of encoding various basic types.
            /// </summary>
            public static TypeInfo[] Basic = new[]
            {
                #region byte

                new TypeInfo("byte", typeof(byte), TypeInfo.Option.None),
                new TypeInfo(typeof(byte[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<byte>), TypeInfo.Option.None),

                new TypeInfo("Vector<byte>", "Vector_of_" + "byte", typeof(Vector<byte>), TypeInfo.Option.None),
                new TypeInfo("Vector<byte>[]", "Array_of_Vector_of_" + "byte", typeof(Vector<byte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<byte>>", "List_of_Vector_of_" + "byte", typeof(List<Vector<byte>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<byte>", "Matrix_of_" + "byte", typeof(Matrix<byte>), TypeInfo.Option.None),
                new TypeInfo("Matrix<byte>[]", "Array_of_Matrix_of_" + "byte", typeof(Matrix<byte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<byte>>", "List_of_Matrix_of_" + "byte", typeof(List<Matrix<byte>>), TypeInfo.Option.None),

                new TypeInfo("Volume<byte>", "Volume_of_" + "byte", typeof(Volume<byte>), TypeInfo.Option.None),
                new TypeInfo("Volume<byte>[]", "Array_of_Volume_of_" + "byte", typeof(Volume<byte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<byte>>", "List_of_Volume_of_" + "byte", typeof(List<Volume<byte>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<byte>", "Tensor_of_" + "byte", typeof(Tensor<byte>), TypeInfo.Option.None),
                new TypeInfo("Tensor<byte>[]", "Array_of_Tensor_of_" + "byte", typeof(Tensor<byte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<byte>>", "List_of_Tensor_of_" + "byte", typeof(List<Tensor<byte>>), TypeInfo.Option.None),

                #endregion

                #region sbyte

                new TypeInfo("sbyte", typeof(sbyte), TypeInfo.Option.None),
                new TypeInfo(typeof(sbyte[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<sbyte>), TypeInfo.Option.None),

                new TypeInfo("Vector<sbyte>", "Vector_of_" + "sbyte", typeof(Vector<sbyte>), TypeInfo.Option.None),
                new TypeInfo("Vector<sbyte>[]", "Array_of_Vector_of_" + "sbyte", typeof(Vector<sbyte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<sbyte>>", "List_of_Vector_of_" + "sbyte", typeof(List<Vector<sbyte>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<sbyte>", "Matrix_of_" + "sbyte", typeof(Matrix<sbyte>), TypeInfo.Option.None),
                new TypeInfo("Matrix<sbyte>[]", "Array_of_Matrix_of_" + "sbyte", typeof(Matrix<sbyte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<sbyte>>", "List_of_Matrix_of_" + "sbyte", typeof(List<Matrix<sbyte>>), TypeInfo.Option.None),

                new TypeInfo("Volume<sbyte>", "Volume_of_" + "sbyte", typeof(Volume<sbyte>), TypeInfo.Option.None),
                new TypeInfo("Volume<sbyte>[]", "Array_of_Volume_of_" + "sbyte", typeof(Volume<sbyte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<sbyte>>", "List_of_Volume_of_" + "sbyte", typeof(List<Volume<sbyte>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<sbyte>", "Tensor_of_" + "sbyte", typeof(Tensor<sbyte>), TypeInfo.Option.None),
                new TypeInfo("Tensor<sbyte>[]", "Array_of_Tensor_of_" + "sbyte", typeof(Tensor<sbyte>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<sbyte>>", "List_of_Tensor_of_" + "sbyte", typeof(List<Tensor<sbyte>>), TypeInfo.Option.None),

                #endregion

                #region short

                new TypeInfo("short", typeof(short), TypeInfo.Option.None),
                new TypeInfo(typeof(short[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<short>), TypeInfo.Option.None),

                new TypeInfo("Vector<short>", "Vector_of_" + "short", typeof(Vector<short>), TypeInfo.Option.None),
                new TypeInfo("Vector<short>[]", "Array_of_Vector_of_" + "short", typeof(Vector<short>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<short>>", "List_of_Vector_of_" + "short", typeof(List<Vector<short>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<short>", "Matrix_of_" + "short", typeof(Matrix<short>), TypeInfo.Option.None),
                new TypeInfo("Matrix<short>[]", "Array_of_Matrix_of_" + "short", typeof(Matrix<short>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<short>>", "List_of_Matrix_of_" + "short", typeof(List<Matrix<short>>), TypeInfo.Option.None),

                new TypeInfo("Volume<short>", "Volume_of_" + "short", typeof(Volume<short>), TypeInfo.Option.None),
                new TypeInfo("Volume<short>[]", "Array_of_Volume_of_" + "short", typeof(Volume<short>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<short>>", "List_of_Volume_of_" + "short", typeof(List<Volume<short>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<short>", "Tensor_of_" + "short", typeof(Tensor<short>), TypeInfo.Option.None),
                new TypeInfo("Tensor<short>[]", "Array_of_Tensor_of_" + "short", typeof(Tensor<short>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<short>>", "List_of_Tensor_of_" + "short", typeof(List<Tensor<short>>), TypeInfo.Option.None),

                #endregion

                #region ushort

                new TypeInfo("ushort", typeof(ushort), TypeInfo.Option.None),
                new TypeInfo(typeof(ushort[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<ushort>), TypeInfo.Option.None),

                new TypeInfo("Vector<ushort>", "Vector_of_" + "ushort", typeof(Vector<ushort>), TypeInfo.Option.None),
                new TypeInfo("Vector<ushort>[]", "Array_of_Vector_of_" + "ushort", typeof(Vector<ushort>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<ushort>>", "List_of_Vector_of_" + "ushort", typeof(List<Vector<ushort>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<ushort>", "Matrix_of_" + "ushort", typeof(Matrix<ushort>), TypeInfo.Option.None),
                new TypeInfo("Matrix<ushort>[]", "Array_of_Matrix_of_" + "ushort", typeof(Matrix<ushort>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<ushort>>", "List_of_Matrix_of_" + "ushort", typeof(List<Matrix<ushort>>), TypeInfo.Option.None),

                new TypeInfo("Volume<ushort>", "Volume_of_" + "ushort", typeof(Volume<ushort>), TypeInfo.Option.None),
                new TypeInfo("Volume<ushort>[]", "Array_of_Volume_of_" + "ushort", typeof(Volume<ushort>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<ushort>>", "List_of_Volume_of_" + "ushort", typeof(List<Volume<ushort>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<ushort>", "Tensor_of_" + "ushort", typeof(Tensor<ushort>), TypeInfo.Option.None),
                new TypeInfo("Tensor<ushort>[]", "Array_of_Tensor_of_" + "ushort", typeof(Tensor<ushort>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<ushort>>", "List_of_Tensor_of_" + "ushort", typeof(List<Tensor<ushort>>), TypeInfo.Option.None),

                #endregion

                #region int

                new TypeInfo("int", typeof(int), TypeInfo.Option.None),
                new TypeInfo(typeof(int[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<int>), TypeInfo.Option.None),

                new TypeInfo("Vector<int>", "Vector_of_" + "int", typeof(Vector<int>), TypeInfo.Option.None),
                new TypeInfo("Vector<int>[]", "Array_of_Vector_of_" + "int", typeof(Vector<int>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<int>>", "List_of_Vector_of_" + "int", typeof(List<Vector<int>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<int>", "Matrix_of_" + "int", typeof(Matrix<int>), TypeInfo.Option.None),
                new TypeInfo("Matrix<int>[]", "Array_of_Matrix_of_" + "int", typeof(Matrix<int>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<int>>", "List_of_Matrix_of_" + "int", typeof(List<Matrix<int>>), TypeInfo.Option.None),

                new TypeInfo("Volume<int>", "Volume_of_" + "int", typeof(Volume<int>), TypeInfo.Option.None),
                new TypeInfo("Volume<int>[]", "Array_of_Volume_of_" + "int", typeof(Volume<int>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<int>>", "List_of_Volume_of_" + "int", typeof(List<Volume<int>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<int>", "Tensor_of_" + "int", typeof(Tensor<int>), TypeInfo.Option.None),
                new TypeInfo("Tensor<int>[]", "Array_of_Tensor_of_" + "int", typeof(Tensor<int>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<int>>", "List_of_Tensor_of_" + "int", typeof(List<Tensor<int>>), TypeInfo.Option.None),

                #endregion

                #region uint

                new TypeInfo("uint", typeof(uint), TypeInfo.Option.None),
                new TypeInfo(typeof(uint[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<uint>), TypeInfo.Option.None),

                new TypeInfo("Vector<uint>", "Vector_of_" + "uint", typeof(Vector<uint>), TypeInfo.Option.None),
                new TypeInfo("Vector<uint>[]", "Array_of_Vector_of_" + "uint", typeof(Vector<uint>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<uint>>", "List_of_Vector_of_" + "uint", typeof(List<Vector<uint>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<uint>", "Matrix_of_" + "uint", typeof(Matrix<uint>), TypeInfo.Option.None),
                new TypeInfo("Matrix<uint>[]", "Array_of_Matrix_of_" + "uint", typeof(Matrix<uint>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<uint>>", "List_of_Matrix_of_" + "uint", typeof(List<Matrix<uint>>), TypeInfo.Option.None),

                new TypeInfo("Volume<uint>", "Volume_of_" + "uint", typeof(Volume<uint>), TypeInfo.Option.None),
                new TypeInfo("Volume<uint>[]", "Array_of_Volume_of_" + "uint", typeof(Volume<uint>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<uint>>", "List_of_Volume_of_" + "uint", typeof(List<Volume<uint>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<uint>", "Tensor_of_" + "uint", typeof(Tensor<uint>), TypeInfo.Option.None),
                new TypeInfo("Tensor<uint>[]", "Array_of_Tensor_of_" + "uint", typeof(Tensor<uint>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<uint>>", "List_of_Tensor_of_" + "uint", typeof(List<Tensor<uint>>), TypeInfo.Option.None),

                #endregion

                #region long

                new TypeInfo("long", typeof(long), TypeInfo.Option.None),
                new TypeInfo(typeof(long[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<long>), TypeInfo.Option.None),

                new TypeInfo("Vector<long>", "Vector_of_" + "long", typeof(Vector<long>), TypeInfo.Option.None),
                new TypeInfo("Vector<long>[]", "Array_of_Vector_of_" + "long", typeof(Vector<long>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<long>>", "List_of_Vector_of_" + "long", typeof(List<Vector<long>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<long>", "Matrix_of_" + "long", typeof(Matrix<long>), TypeInfo.Option.None),
                new TypeInfo("Matrix<long>[]", "Array_of_Matrix_of_" + "long", typeof(Matrix<long>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<long>>", "List_of_Matrix_of_" + "long", typeof(List<Matrix<long>>), TypeInfo.Option.None),

                new TypeInfo("Volume<long>", "Volume_of_" + "long", typeof(Volume<long>), TypeInfo.Option.None),
                new TypeInfo("Volume<long>[]", "Array_of_Volume_of_" + "long", typeof(Volume<long>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<long>>", "List_of_Volume_of_" + "long", typeof(List<Volume<long>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<long>", "Tensor_of_" + "long", typeof(Tensor<long>), TypeInfo.Option.None),
                new TypeInfo("Tensor<long>[]", "Array_of_Tensor_of_" + "long", typeof(Tensor<long>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<long>>", "List_of_Tensor_of_" + "long", typeof(List<Tensor<long>>), TypeInfo.Option.None),

                #endregion

                #region ulong

                new TypeInfo("ulong", typeof(ulong), TypeInfo.Option.None),
                new TypeInfo(typeof(ulong[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<ulong>), TypeInfo.Option.None),

                new TypeInfo("Vector<ulong>", "Vector_of_" + "ulong", typeof(Vector<ulong>), TypeInfo.Option.None),
                new TypeInfo("Vector<ulong>[]", "Array_of_Vector_of_" + "ulong", typeof(Vector<ulong>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<ulong>>", "List_of_Vector_of_" + "ulong", typeof(List<Vector<ulong>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<ulong>", "Matrix_of_" + "ulong", typeof(Matrix<ulong>), TypeInfo.Option.None),
                new TypeInfo("Matrix<ulong>[]", "Array_of_Matrix_of_" + "ulong", typeof(Matrix<ulong>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<ulong>>", "List_of_Matrix_of_" + "ulong", typeof(List<Matrix<ulong>>), TypeInfo.Option.None),

                new TypeInfo("Volume<ulong>", "Volume_of_" + "ulong", typeof(Volume<ulong>), TypeInfo.Option.None),
                new TypeInfo("Volume<ulong>[]", "Array_of_Volume_of_" + "ulong", typeof(Volume<ulong>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<ulong>>", "List_of_Volume_of_" + "ulong", typeof(List<Volume<ulong>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<ulong>", "Tensor_of_" + "ulong", typeof(Tensor<ulong>), TypeInfo.Option.None),
                new TypeInfo("Tensor<ulong>[]", "Array_of_Tensor_of_" + "ulong", typeof(Tensor<ulong>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<ulong>>", "List_of_Tensor_of_" + "ulong", typeof(List<Tensor<ulong>>), TypeInfo.Option.None),

                #endregion

                #region float

                new TypeInfo("float", typeof(float), TypeInfo.Option.None),
                new TypeInfo(typeof(float[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<float>), TypeInfo.Option.None),

                new TypeInfo("Vector<float>", "Vector_of_" + "float", typeof(Vector<float>), TypeInfo.Option.None),
                new TypeInfo("Vector<float>[]", "Array_of_Vector_of_" + "float", typeof(Vector<float>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<float>>", "List_of_Vector_of_" + "float", typeof(List<Vector<float>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<float>", "Matrix_of_" + "float", typeof(Matrix<float>), TypeInfo.Option.None),
                new TypeInfo("Matrix<float>[]", "Array_of_Matrix_of_" + "float", typeof(Matrix<float>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<float>>", "List_of_Matrix_of_" + "float", typeof(List<Matrix<float>>), TypeInfo.Option.None),

                new TypeInfo("Volume<float>", "Volume_of_" + "float", typeof(Volume<float>), TypeInfo.Option.None),
                new TypeInfo("Volume<float>[]", "Array_of_Volume_of_" + "float", typeof(Volume<float>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<float>>", "List_of_Volume_of_" + "float", typeof(List<Volume<float>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<float>", "Tensor_of_" + "float", typeof(Tensor<float>), TypeInfo.Option.None),
                new TypeInfo("Tensor<float>[]", "Array_of_Tensor_of_" + "float", typeof(Tensor<float>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<float>>", "List_of_Tensor_of_" + "float", typeof(List<Tensor<float>>), TypeInfo.Option.None),

                #endregion

                #region double

                new TypeInfo("double", typeof(double), TypeInfo.Option.None),
                new TypeInfo(typeof(double[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<double>), TypeInfo.Option.None),

                new TypeInfo("Vector<double>", "Vector_of_" + "double", typeof(Vector<double>), TypeInfo.Option.None),
                new TypeInfo("Vector<double>[]", "Array_of_Vector_of_" + "double", typeof(Vector<double>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<double>>", "List_of_Vector_of_" + "double", typeof(List<Vector<double>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<double>", "Matrix_of_" + "double", typeof(Matrix<double>), TypeInfo.Option.None),
                new TypeInfo("Matrix<double>[]", "Array_of_Matrix_of_" + "double", typeof(Matrix<double>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<double>>", "List_of_Matrix_of_" + "double", typeof(List<Matrix<double>>), TypeInfo.Option.None),

                new TypeInfo("Volume<double>", "Volume_of_" + "double", typeof(Volume<double>), TypeInfo.Option.None),
                new TypeInfo("Volume<double>[]", "Array_of_Volume_of_" + "double", typeof(Volume<double>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<double>>", "List_of_Volume_of_" + "double", typeof(List<Volume<double>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<double>", "Tensor_of_" + "double", typeof(Tensor<double>), TypeInfo.Option.None),
                new TypeInfo("Tensor<double>[]", "Array_of_Tensor_of_" + "double", typeof(Tensor<double>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<double>>", "List_of_Tensor_of_" + "double", typeof(List<Tensor<double>>), TypeInfo.Option.None),

                #endregion

                #region Fraction

                new TypeInfo("Fraction", typeof(Fraction), TypeInfo.Option.None),
                new TypeInfo(typeof(Fraction[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Fraction>), TypeInfo.Option.None),

                new TypeInfo("Vector<Fraction>", "Vector_of_" + "Fraction", typeof(Vector<Fraction>), TypeInfo.Option.None),
                new TypeInfo("Vector<Fraction>[]", "Array_of_Vector_of_" + "Fraction", typeof(Vector<Fraction>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Fraction>>", "List_of_Vector_of_" + "Fraction", typeof(List<Vector<Fraction>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Fraction>", "Matrix_of_" + "Fraction", typeof(Matrix<Fraction>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Fraction>[]", "Array_of_Matrix_of_" + "Fraction", typeof(Matrix<Fraction>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Fraction>>", "List_of_Matrix_of_" + "Fraction", typeof(List<Matrix<Fraction>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Fraction>", "Volume_of_" + "Fraction", typeof(Volume<Fraction>), TypeInfo.Option.None),
                new TypeInfo("Volume<Fraction>[]", "Array_of_Volume_of_" + "Fraction", typeof(Volume<Fraction>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Fraction>>", "List_of_Volume_of_" + "Fraction", typeof(List<Volume<Fraction>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Fraction>", "Tensor_of_" + "Fraction", typeof(Tensor<Fraction>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Fraction>[]", "Array_of_Tensor_of_" + "Fraction", typeof(Tensor<Fraction>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Fraction>>", "List_of_Tensor_of_" + "Fraction", typeof(List<Tensor<Fraction>>), TypeInfo.Option.None),

                #endregion

                #region V2i

                new TypeInfo("V2i", typeof(V2i), TypeInfo.Option.None),
                new TypeInfo(typeof(V2i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V2i>), TypeInfo.Option.None),

                new TypeInfo("Vector<V2i>", "Vector_of_" + "V2i", typeof(Vector<V2i>), TypeInfo.Option.None),
                new TypeInfo("Vector<V2i>[]", "Array_of_Vector_of_" + "V2i", typeof(Vector<V2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V2i>>", "List_of_Vector_of_" + "V2i", typeof(List<Vector<V2i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V2i>", "Matrix_of_" + "V2i", typeof(Matrix<V2i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V2i>[]", "Array_of_Matrix_of_" + "V2i", typeof(Matrix<V2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V2i>>", "List_of_Matrix_of_" + "V2i", typeof(List<Matrix<V2i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V2i>", "Volume_of_" + "V2i", typeof(Volume<V2i>), TypeInfo.Option.None),
                new TypeInfo("Volume<V2i>[]", "Array_of_Volume_of_" + "V2i", typeof(Volume<V2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V2i>>", "List_of_Volume_of_" + "V2i", typeof(List<Volume<V2i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V2i>", "Tensor_of_" + "V2i", typeof(Tensor<V2i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V2i>[]", "Array_of_Tensor_of_" + "V2i", typeof(Tensor<V2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V2i>>", "List_of_Tensor_of_" + "V2i", typeof(List<Tensor<V2i>>), TypeInfo.Option.None),

                #endregion

                #region V2l

                new TypeInfo("V2l", typeof(V2l), TypeInfo.Option.None),
                new TypeInfo(typeof(V2l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V2l>), TypeInfo.Option.None),

                new TypeInfo("Vector<V2l>", "Vector_of_" + "V2l", typeof(Vector<V2l>), TypeInfo.Option.None),
                new TypeInfo("Vector<V2l>[]", "Array_of_Vector_of_" + "V2l", typeof(Vector<V2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V2l>>", "List_of_Vector_of_" + "V2l", typeof(List<Vector<V2l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V2l>", "Matrix_of_" + "V2l", typeof(Matrix<V2l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V2l>[]", "Array_of_Matrix_of_" + "V2l", typeof(Matrix<V2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V2l>>", "List_of_Matrix_of_" + "V2l", typeof(List<Matrix<V2l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V2l>", "Volume_of_" + "V2l", typeof(Volume<V2l>), TypeInfo.Option.None),
                new TypeInfo("Volume<V2l>[]", "Array_of_Volume_of_" + "V2l", typeof(Volume<V2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V2l>>", "List_of_Volume_of_" + "V2l", typeof(List<Volume<V2l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V2l>", "Tensor_of_" + "V2l", typeof(Tensor<V2l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V2l>[]", "Array_of_Tensor_of_" + "V2l", typeof(Tensor<V2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V2l>>", "List_of_Tensor_of_" + "V2l", typeof(List<Tensor<V2l>>), TypeInfo.Option.None),

                #endregion

                #region V2f

                new TypeInfo("V2f", typeof(V2f), TypeInfo.Option.None),
                new TypeInfo(typeof(V2f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V2f>), TypeInfo.Option.None),

                new TypeInfo("Vector<V2f>", "Vector_of_" + "V2f", typeof(Vector<V2f>), TypeInfo.Option.None),
                new TypeInfo("Vector<V2f>[]", "Array_of_Vector_of_" + "V2f", typeof(Vector<V2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V2f>>", "List_of_Vector_of_" + "V2f", typeof(List<Vector<V2f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V2f>", "Matrix_of_" + "V2f", typeof(Matrix<V2f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V2f>[]", "Array_of_Matrix_of_" + "V2f", typeof(Matrix<V2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V2f>>", "List_of_Matrix_of_" + "V2f", typeof(List<Matrix<V2f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V2f>", "Volume_of_" + "V2f", typeof(Volume<V2f>), TypeInfo.Option.None),
                new TypeInfo("Volume<V2f>[]", "Array_of_Volume_of_" + "V2f", typeof(Volume<V2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V2f>>", "List_of_Volume_of_" + "V2f", typeof(List<Volume<V2f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V2f>", "Tensor_of_" + "V2f", typeof(Tensor<V2f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V2f>[]", "Array_of_Tensor_of_" + "V2f", typeof(Tensor<V2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V2f>>", "List_of_Tensor_of_" + "V2f", typeof(List<Tensor<V2f>>), TypeInfo.Option.None),

                #endregion

                #region V2d

                new TypeInfo("V2d", typeof(V2d), TypeInfo.Option.None),
                new TypeInfo(typeof(V2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<V2d>", "Vector_of_" + "V2d", typeof(Vector<V2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<V2d>[]", "Array_of_Vector_of_" + "V2d", typeof(Vector<V2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V2d>>", "List_of_Vector_of_" + "V2d", typeof(List<Vector<V2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V2d>", "Matrix_of_" + "V2d", typeof(Matrix<V2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V2d>[]", "Array_of_Matrix_of_" + "V2d", typeof(Matrix<V2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V2d>>", "List_of_Matrix_of_" + "V2d", typeof(List<Matrix<V2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V2d>", "Volume_of_" + "V2d", typeof(Volume<V2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<V2d>[]", "Array_of_Volume_of_" + "V2d", typeof(Volume<V2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V2d>>", "List_of_Volume_of_" + "V2d", typeof(List<Volume<V2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V2d>", "Tensor_of_" + "V2d", typeof(Tensor<V2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V2d>[]", "Array_of_Tensor_of_" + "V2d", typeof(Tensor<V2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V2d>>", "List_of_Tensor_of_" + "V2d", typeof(List<Tensor<V2d>>), TypeInfo.Option.None),

                #endregion

                #region V3i

                new TypeInfo("V3i", typeof(V3i), TypeInfo.Option.None),
                new TypeInfo(typeof(V3i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V3i>), TypeInfo.Option.None),

                new TypeInfo("Vector<V3i>", "Vector_of_" + "V3i", typeof(Vector<V3i>), TypeInfo.Option.None),
                new TypeInfo("Vector<V3i>[]", "Array_of_Vector_of_" + "V3i", typeof(Vector<V3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V3i>>", "List_of_Vector_of_" + "V3i", typeof(List<Vector<V3i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V3i>", "Matrix_of_" + "V3i", typeof(Matrix<V3i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V3i>[]", "Array_of_Matrix_of_" + "V3i", typeof(Matrix<V3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V3i>>", "List_of_Matrix_of_" + "V3i", typeof(List<Matrix<V3i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V3i>", "Volume_of_" + "V3i", typeof(Volume<V3i>), TypeInfo.Option.None),
                new TypeInfo("Volume<V3i>[]", "Array_of_Volume_of_" + "V3i", typeof(Volume<V3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V3i>>", "List_of_Volume_of_" + "V3i", typeof(List<Volume<V3i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V3i>", "Tensor_of_" + "V3i", typeof(Tensor<V3i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V3i>[]", "Array_of_Tensor_of_" + "V3i", typeof(Tensor<V3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V3i>>", "List_of_Tensor_of_" + "V3i", typeof(List<Tensor<V3i>>), TypeInfo.Option.None),

                #endregion

                #region V3l

                new TypeInfo("V3l", typeof(V3l), TypeInfo.Option.None),
                new TypeInfo(typeof(V3l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V3l>), TypeInfo.Option.None),

                new TypeInfo("Vector<V3l>", "Vector_of_" + "V3l", typeof(Vector<V3l>), TypeInfo.Option.None),
                new TypeInfo("Vector<V3l>[]", "Array_of_Vector_of_" + "V3l", typeof(Vector<V3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V3l>>", "List_of_Vector_of_" + "V3l", typeof(List<Vector<V3l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V3l>", "Matrix_of_" + "V3l", typeof(Matrix<V3l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V3l>[]", "Array_of_Matrix_of_" + "V3l", typeof(Matrix<V3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V3l>>", "List_of_Matrix_of_" + "V3l", typeof(List<Matrix<V3l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V3l>", "Volume_of_" + "V3l", typeof(Volume<V3l>), TypeInfo.Option.None),
                new TypeInfo("Volume<V3l>[]", "Array_of_Volume_of_" + "V3l", typeof(Volume<V3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V3l>>", "List_of_Volume_of_" + "V3l", typeof(List<Volume<V3l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V3l>", "Tensor_of_" + "V3l", typeof(Tensor<V3l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V3l>[]", "Array_of_Tensor_of_" + "V3l", typeof(Tensor<V3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V3l>>", "List_of_Tensor_of_" + "V3l", typeof(List<Tensor<V3l>>), TypeInfo.Option.None),

                #endregion

                #region V3f

                new TypeInfo("V3f", typeof(V3f), TypeInfo.Option.None),
                new TypeInfo(typeof(V3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<V3f>", "Vector_of_" + "V3f", typeof(Vector<V3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<V3f>[]", "Array_of_Vector_of_" + "V3f", typeof(Vector<V3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V3f>>", "List_of_Vector_of_" + "V3f", typeof(List<Vector<V3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V3f>", "Matrix_of_" + "V3f", typeof(Matrix<V3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V3f>[]", "Array_of_Matrix_of_" + "V3f", typeof(Matrix<V3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V3f>>", "List_of_Matrix_of_" + "V3f", typeof(List<Matrix<V3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V3f>", "Volume_of_" + "V3f", typeof(Volume<V3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<V3f>[]", "Array_of_Volume_of_" + "V3f", typeof(Volume<V3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V3f>>", "List_of_Volume_of_" + "V3f", typeof(List<Volume<V3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V3f>", "Tensor_of_" + "V3f", typeof(Tensor<V3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V3f>[]", "Array_of_Tensor_of_" + "V3f", typeof(Tensor<V3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V3f>>", "List_of_Tensor_of_" + "V3f", typeof(List<Tensor<V3f>>), TypeInfo.Option.None),

                #endregion

                #region V3d

                new TypeInfo("V3d", typeof(V3d), TypeInfo.Option.None),
                new TypeInfo(typeof(V3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<V3d>", "Vector_of_" + "V3d", typeof(Vector<V3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<V3d>[]", "Array_of_Vector_of_" + "V3d", typeof(Vector<V3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V3d>>", "List_of_Vector_of_" + "V3d", typeof(List<Vector<V3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V3d>", "Matrix_of_" + "V3d", typeof(Matrix<V3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V3d>[]", "Array_of_Matrix_of_" + "V3d", typeof(Matrix<V3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V3d>>", "List_of_Matrix_of_" + "V3d", typeof(List<Matrix<V3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V3d>", "Volume_of_" + "V3d", typeof(Volume<V3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<V3d>[]", "Array_of_Volume_of_" + "V3d", typeof(Volume<V3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V3d>>", "List_of_Volume_of_" + "V3d", typeof(List<Volume<V3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V3d>", "Tensor_of_" + "V3d", typeof(Tensor<V3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V3d>[]", "Array_of_Tensor_of_" + "V3d", typeof(Tensor<V3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V3d>>", "List_of_Tensor_of_" + "V3d", typeof(List<Tensor<V3d>>), TypeInfo.Option.None),

                #endregion

                #region V4i

                new TypeInfo("V4i", typeof(V4i), TypeInfo.Option.None),
                new TypeInfo(typeof(V4i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V4i>), TypeInfo.Option.None),

                new TypeInfo("Vector<V4i>", "Vector_of_" + "V4i", typeof(Vector<V4i>), TypeInfo.Option.None),
                new TypeInfo("Vector<V4i>[]", "Array_of_Vector_of_" + "V4i", typeof(Vector<V4i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V4i>>", "List_of_Vector_of_" + "V4i", typeof(List<Vector<V4i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V4i>", "Matrix_of_" + "V4i", typeof(Matrix<V4i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V4i>[]", "Array_of_Matrix_of_" + "V4i", typeof(Matrix<V4i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V4i>>", "List_of_Matrix_of_" + "V4i", typeof(List<Matrix<V4i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V4i>", "Volume_of_" + "V4i", typeof(Volume<V4i>), TypeInfo.Option.None),
                new TypeInfo("Volume<V4i>[]", "Array_of_Volume_of_" + "V4i", typeof(Volume<V4i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V4i>>", "List_of_Volume_of_" + "V4i", typeof(List<Volume<V4i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V4i>", "Tensor_of_" + "V4i", typeof(Tensor<V4i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V4i>[]", "Array_of_Tensor_of_" + "V4i", typeof(Tensor<V4i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V4i>>", "List_of_Tensor_of_" + "V4i", typeof(List<Tensor<V4i>>), TypeInfo.Option.None),

                #endregion

                #region V4l

                new TypeInfo("V4l", typeof(V4l), TypeInfo.Option.None),
                new TypeInfo(typeof(V4l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V4l>), TypeInfo.Option.None),

                new TypeInfo("Vector<V4l>", "Vector_of_" + "V4l", typeof(Vector<V4l>), TypeInfo.Option.None),
                new TypeInfo("Vector<V4l>[]", "Array_of_Vector_of_" + "V4l", typeof(Vector<V4l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V4l>>", "List_of_Vector_of_" + "V4l", typeof(List<Vector<V4l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V4l>", "Matrix_of_" + "V4l", typeof(Matrix<V4l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V4l>[]", "Array_of_Matrix_of_" + "V4l", typeof(Matrix<V4l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V4l>>", "List_of_Matrix_of_" + "V4l", typeof(List<Matrix<V4l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V4l>", "Volume_of_" + "V4l", typeof(Volume<V4l>), TypeInfo.Option.None),
                new TypeInfo("Volume<V4l>[]", "Array_of_Volume_of_" + "V4l", typeof(Volume<V4l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V4l>>", "List_of_Volume_of_" + "V4l", typeof(List<Volume<V4l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V4l>", "Tensor_of_" + "V4l", typeof(Tensor<V4l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V4l>[]", "Array_of_Tensor_of_" + "V4l", typeof(Tensor<V4l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V4l>>", "List_of_Tensor_of_" + "V4l", typeof(List<Tensor<V4l>>), TypeInfo.Option.None),

                #endregion

                #region V4f

                new TypeInfo("V4f", typeof(V4f), TypeInfo.Option.None),
                new TypeInfo(typeof(V4f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V4f>), TypeInfo.Option.None),

                new TypeInfo("Vector<V4f>", "Vector_of_" + "V4f", typeof(Vector<V4f>), TypeInfo.Option.None),
                new TypeInfo("Vector<V4f>[]", "Array_of_Vector_of_" + "V4f", typeof(Vector<V4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V4f>>", "List_of_Vector_of_" + "V4f", typeof(List<Vector<V4f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V4f>", "Matrix_of_" + "V4f", typeof(Matrix<V4f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V4f>[]", "Array_of_Matrix_of_" + "V4f", typeof(Matrix<V4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V4f>>", "List_of_Matrix_of_" + "V4f", typeof(List<Matrix<V4f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V4f>", "Volume_of_" + "V4f", typeof(Volume<V4f>), TypeInfo.Option.None),
                new TypeInfo("Volume<V4f>[]", "Array_of_Volume_of_" + "V4f", typeof(Volume<V4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V4f>>", "List_of_Volume_of_" + "V4f", typeof(List<Volume<V4f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V4f>", "Tensor_of_" + "V4f", typeof(Tensor<V4f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V4f>[]", "Array_of_Tensor_of_" + "V4f", typeof(Tensor<V4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V4f>>", "List_of_Tensor_of_" + "V4f", typeof(List<Tensor<V4f>>), TypeInfo.Option.None),

                #endregion

                #region V4d

                new TypeInfo("V4d", typeof(V4d), TypeInfo.Option.None),
                new TypeInfo(typeof(V4d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<V4d>), TypeInfo.Option.None),

                new TypeInfo("Vector<V4d>", "Vector_of_" + "V4d", typeof(Vector<V4d>), TypeInfo.Option.None),
                new TypeInfo("Vector<V4d>[]", "Array_of_Vector_of_" + "V4d", typeof(Vector<V4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<V4d>>", "List_of_Vector_of_" + "V4d", typeof(List<Vector<V4d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<V4d>", "Matrix_of_" + "V4d", typeof(Matrix<V4d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<V4d>[]", "Array_of_Matrix_of_" + "V4d", typeof(Matrix<V4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<V4d>>", "List_of_Matrix_of_" + "V4d", typeof(List<Matrix<V4d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<V4d>", "Volume_of_" + "V4d", typeof(Volume<V4d>), TypeInfo.Option.None),
                new TypeInfo("Volume<V4d>[]", "Array_of_Volume_of_" + "V4d", typeof(Volume<V4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<V4d>>", "List_of_Volume_of_" + "V4d", typeof(List<Volume<V4d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<V4d>", "Tensor_of_" + "V4d", typeof(Tensor<V4d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<V4d>[]", "Array_of_Tensor_of_" + "V4d", typeof(Tensor<V4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<V4d>>", "List_of_Tensor_of_" + "V4d", typeof(List<Tensor<V4d>>), TypeInfo.Option.None),

                #endregion

                #region M22i

                new TypeInfo("M22i", typeof(M22i), TypeInfo.Option.None),
                new TypeInfo(typeof(M22i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M22i>), TypeInfo.Option.None),

                new TypeInfo("Vector<M22i>", "Vector_of_" + "M22i", typeof(Vector<M22i>), TypeInfo.Option.None),
                new TypeInfo("Vector<M22i>[]", "Array_of_Vector_of_" + "M22i", typeof(Vector<M22i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M22i>>", "List_of_Vector_of_" + "M22i", typeof(List<Vector<M22i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M22i>", "Matrix_of_" + "M22i", typeof(Matrix<M22i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M22i>[]", "Array_of_Matrix_of_" + "M22i", typeof(Matrix<M22i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M22i>>", "List_of_Matrix_of_" + "M22i", typeof(List<Matrix<M22i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M22i>", "Volume_of_" + "M22i", typeof(Volume<M22i>), TypeInfo.Option.None),
                new TypeInfo("Volume<M22i>[]", "Array_of_Volume_of_" + "M22i", typeof(Volume<M22i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M22i>>", "List_of_Volume_of_" + "M22i", typeof(List<Volume<M22i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M22i>", "Tensor_of_" + "M22i", typeof(Tensor<M22i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M22i>[]", "Array_of_Tensor_of_" + "M22i", typeof(Tensor<M22i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M22i>>", "List_of_Tensor_of_" + "M22i", typeof(List<Tensor<M22i>>), TypeInfo.Option.None),

                #endregion

                #region M22l

                new TypeInfo("M22l", typeof(M22l), TypeInfo.Option.None),
                new TypeInfo(typeof(M22l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M22l>), TypeInfo.Option.None),

                new TypeInfo("Vector<M22l>", "Vector_of_" + "M22l", typeof(Vector<M22l>), TypeInfo.Option.None),
                new TypeInfo("Vector<M22l>[]", "Array_of_Vector_of_" + "M22l", typeof(Vector<M22l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M22l>>", "List_of_Vector_of_" + "M22l", typeof(List<Vector<M22l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M22l>", "Matrix_of_" + "M22l", typeof(Matrix<M22l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M22l>[]", "Array_of_Matrix_of_" + "M22l", typeof(Matrix<M22l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M22l>>", "List_of_Matrix_of_" + "M22l", typeof(List<Matrix<M22l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M22l>", "Volume_of_" + "M22l", typeof(Volume<M22l>), TypeInfo.Option.None),
                new TypeInfo("Volume<M22l>[]", "Array_of_Volume_of_" + "M22l", typeof(Volume<M22l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M22l>>", "List_of_Volume_of_" + "M22l", typeof(List<Volume<M22l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M22l>", "Tensor_of_" + "M22l", typeof(Tensor<M22l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M22l>[]", "Array_of_Tensor_of_" + "M22l", typeof(Tensor<M22l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M22l>>", "List_of_Tensor_of_" + "M22l", typeof(List<Tensor<M22l>>), TypeInfo.Option.None),

                #endregion

                #region M22f

                new TypeInfo("M22f", typeof(M22f), TypeInfo.Option.None),
                new TypeInfo(typeof(M22f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M22f>), TypeInfo.Option.None),

                new TypeInfo("Vector<M22f>", "Vector_of_" + "M22f", typeof(Vector<M22f>), TypeInfo.Option.None),
                new TypeInfo("Vector<M22f>[]", "Array_of_Vector_of_" + "M22f", typeof(Vector<M22f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M22f>>", "List_of_Vector_of_" + "M22f", typeof(List<Vector<M22f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M22f>", "Matrix_of_" + "M22f", typeof(Matrix<M22f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M22f>[]", "Array_of_Matrix_of_" + "M22f", typeof(Matrix<M22f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M22f>>", "List_of_Matrix_of_" + "M22f", typeof(List<Matrix<M22f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M22f>", "Volume_of_" + "M22f", typeof(Volume<M22f>), TypeInfo.Option.None),
                new TypeInfo("Volume<M22f>[]", "Array_of_Volume_of_" + "M22f", typeof(Volume<M22f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M22f>>", "List_of_Volume_of_" + "M22f", typeof(List<Volume<M22f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M22f>", "Tensor_of_" + "M22f", typeof(Tensor<M22f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M22f>[]", "Array_of_Tensor_of_" + "M22f", typeof(Tensor<M22f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M22f>>", "List_of_Tensor_of_" + "M22f", typeof(List<Tensor<M22f>>), TypeInfo.Option.None),

                #endregion

                #region M22d

                new TypeInfo("M22d", typeof(M22d), TypeInfo.Option.None),
                new TypeInfo(typeof(M22d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M22d>), TypeInfo.Option.None),

                new TypeInfo("Vector<M22d>", "Vector_of_" + "M22d", typeof(Vector<M22d>), TypeInfo.Option.None),
                new TypeInfo("Vector<M22d>[]", "Array_of_Vector_of_" + "M22d", typeof(Vector<M22d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M22d>>", "List_of_Vector_of_" + "M22d", typeof(List<Vector<M22d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M22d>", "Matrix_of_" + "M22d", typeof(Matrix<M22d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M22d>[]", "Array_of_Matrix_of_" + "M22d", typeof(Matrix<M22d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M22d>>", "List_of_Matrix_of_" + "M22d", typeof(List<Matrix<M22d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M22d>", "Volume_of_" + "M22d", typeof(Volume<M22d>), TypeInfo.Option.None),
                new TypeInfo("Volume<M22d>[]", "Array_of_Volume_of_" + "M22d", typeof(Volume<M22d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M22d>>", "List_of_Volume_of_" + "M22d", typeof(List<Volume<M22d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M22d>", "Tensor_of_" + "M22d", typeof(Tensor<M22d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M22d>[]", "Array_of_Tensor_of_" + "M22d", typeof(Tensor<M22d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M22d>>", "List_of_Tensor_of_" + "M22d", typeof(List<Tensor<M22d>>), TypeInfo.Option.None),

                #endregion

                #region M23i

                new TypeInfo("M23i", typeof(M23i), TypeInfo.Option.None),
                new TypeInfo(typeof(M23i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M23i>), TypeInfo.Option.None),

                new TypeInfo("Vector<M23i>", "Vector_of_" + "M23i", typeof(Vector<M23i>), TypeInfo.Option.None),
                new TypeInfo("Vector<M23i>[]", "Array_of_Vector_of_" + "M23i", typeof(Vector<M23i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M23i>>", "List_of_Vector_of_" + "M23i", typeof(List<Vector<M23i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M23i>", "Matrix_of_" + "M23i", typeof(Matrix<M23i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M23i>[]", "Array_of_Matrix_of_" + "M23i", typeof(Matrix<M23i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M23i>>", "List_of_Matrix_of_" + "M23i", typeof(List<Matrix<M23i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M23i>", "Volume_of_" + "M23i", typeof(Volume<M23i>), TypeInfo.Option.None),
                new TypeInfo("Volume<M23i>[]", "Array_of_Volume_of_" + "M23i", typeof(Volume<M23i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M23i>>", "List_of_Volume_of_" + "M23i", typeof(List<Volume<M23i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M23i>", "Tensor_of_" + "M23i", typeof(Tensor<M23i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M23i>[]", "Array_of_Tensor_of_" + "M23i", typeof(Tensor<M23i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M23i>>", "List_of_Tensor_of_" + "M23i", typeof(List<Tensor<M23i>>), TypeInfo.Option.None),

                #endregion

                #region M23l

                new TypeInfo("M23l", typeof(M23l), TypeInfo.Option.None),
                new TypeInfo(typeof(M23l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M23l>), TypeInfo.Option.None),

                new TypeInfo("Vector<M23l>", "Vector_of_" + "M23l", typeof(Vector<M23l>), TypeInfo.Option.None),
                new TypeInfo("Vector<M23l>[]", "Array_of_Vector_of_" + "M23l", typeof(Vector<M23l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M23l>>", "List_of_Vector_of_" + "M23l", typeof(List<Vector<M23l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M23l>", "Matrix_of_" + "M23l", typeof(Matrix<M23l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M23l>[]", "Array_of_Matrix_of_" + "M23l", typeof(Matrix<M23l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M23l>>", "List_of_Matrix_of_" + "M23l", typeof(List<Matrix<M23l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M23l>", "Volume_of_" + "M23l", typeof(Volume<M23l>), TypeInfo.Option.None),
                new TypeInfo("Volume<M23l>[]", "Array_of_Volume_of_" + "M23l", typeof(Volume<M23l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M23l>>", "List_of_Volume_of_" + "M23l", typeof(List<Volume<M23l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M23l>", "Tensor_of_" + "M23l", typeof(Tensor<M23l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M23l>[]", "Array_of_Tensor_of_" + "M23l", typeof(Tensor<M23l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M23l>>", "List_of_Tensor_of_" + "M23l", typeof(List<Tensor<M23l>>), TypeInfo.Option.None),

                #endregion

                #region M23f

                new TypeInfo("M23f", typeof(M23f), TypeInfo.Option.None),
                new TypeInfo(typeof(M23f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M23f>), TypeInfo.Option.None),

                new TypeInfo("Vector<M23f>", "Vector_of_" + "M23f", typeof(Vector<M23f>), TypeInfo.Option.None),
                new TypeInfo("Vector<M23f>[]", "Array_of_Vector_of_" + "M23f", typeof(Vector<M23f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M23f>>", "List_of_Vector_of_" + "M23f", typeof(List<Vector<M23f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M23f>", "Matrix_of_" + "M23f", typeof(Matrix<M23f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M23f>[]", "Array_of_Matrix_of_" + "M23f", typeof(Matrix<M23f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M23f>>", "List_of_Matrix_of_" + "M23f", typeof(List<Matrix<M23f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M23f>", "Volume_of_" + "M23f", typeof(Volume<M23f>), TypeInfo.Option.None),
                new TypeInfo("Volume<M23f>[]", "Array_of_Volume_of_" + "M23f", typeof(Volume<M23f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M23f>>", "List_of_Volume_of_" + "M23f", typeof(List<Volume<M23f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M23f>", "Tensor_of_" + "M23f", typeof(Tensor<M23f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M23f>[]", "Array_of_Tensor_of_" + "M23f", typeof(Tensor<M23f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M23f>>", "List_of_Tensor_of_" + "M23f", typeof(List<Tensor<M23f>>), TypeInfo.Option.None),

                #endregion

                #region M23d

                new TypeInfo("M23d", typeof(M23d), TypeInfo.Option.None),
                new TypeInfo(typeof(M23d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M23d>), TypeInfo.Option.None),

                new TypeInfo("Vector<M23d>", "Vector_of_" + "M23d", typeof(Vector<M23d>), TypeInfo.Option.None),
                new TypeInfo("Vector<M23d>[]", "Array_of_Vector_of_" + "M23d", typeof(Vector<M23d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M23d>>", "List_of_Vector_of_" + "M23d", typeof(List<Vector<M23d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M23d>", "Matrix_of_" + "M23d", typeof(Matrix<M23d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M23d>[]", "Array_of_Matrix_of_" + "M23d", typeof(Matrix<M23d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M23d>>", "List_of_Matrix_of_" + "M23d", typeof(List<Matrix<M23d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M23d>", "Volume_of_" + "M23d", typeof(Volume<M23d>), TypeInfo.Option.None),
                new TypeInfo("Volume<M23d>[]", "Array_of_Volume_of_" + "M23d", typeof(Volume<M23d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M23d>>", "List_of_Volume_of_" + "M23d", typeof(List<Volume<M23d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M23d>", "Tensor_of_" + "M23d", typeof(Tensor<M23d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M23d>[]", "Array_of_Tensor_of_" + "M23d", typeof(Tensor<M23d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M23d>>", "List_of_Tensor_of_" + "M23d", typeof(List<Tensor<M23d>>), TypeInfo.Option.None),

                #endregion

                #region M33i

                new TypeInfo("M33i", typeof(M33i), TypeInfo.Option.None),
                new TypeInfo(typeof(M33i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M33i>), TypeInfo.Option.None),

                new TypeInfo("Vector<M33i>", "Vector_of_" + "M33i", typeof(Vector<M33i>), TypeInfo.Option.None),
                new TypeInfo("Vector<M33i>[]", "Array_of_Vector_of_" + "M33i", typeof(Vector<M33i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M33i>>", "List_of_Vector_of_" + "M33i", typeof(List<Vector<M33i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M33i>", "Matrix_of_" + "M33i", typeof(Matrix<M33i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M33i>[]", "Array_of_Matrix_of_" + "M33i", typeof(Matrix<M33i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M33i>>", "List_of_Matrix_of_" + "M33i", typeof(List<Matrix<M33i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M33i>", "Volume_of_" + "M33i", typeof(Volume<M33i>), TypeInfo.Option.None),
                new TypeInfo("Volume<M33i>[]", "Array_of_Volume_of_" + "M33i", typeof(Volume<M33i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M33i>>", "List_of_Volume_of_" + "M33i", typeof(List<Volume<M33i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M33i>", "Tensor_of_" + "M33i", typeof(Tensor<M33i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M33i>[]", "Array_of_Tensor_of_" + "M33i", typeof(Tensor<M33i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M33i>>", "List_of_Tensor_of_" + "M33i", typeof(List<Tensor<M33i>>), TypeInfo.Option.None),

                #endregion

                #region M33l

                new TypeInfo("M33l", typeof(M33l), TypeInfo.Option.None),
                new TypeInfo(typeof(M33l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M33l>), TypeInfo.Option.None),

                new TypeInfo("Vector<M33l>", "Vector_of_" + "M33l", typeof(Vector<M33l>), TypeInfo.Option.None),
                new TypeInfo("Vector<M33l>[]", "Array_of_Vector_of_" + "M33l", typeof(Vector<M33l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M33l>>", "List_of_Vector_of_" + "M33l", typeof(List<Vector<M33l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M33l>", "Matrix_of_" + "M33l", typeof(Matrix<M33l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M33l>[]", "Array_of_Matrix_of_" + "M33l", typeof(Matrix<M33l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M33l>>", "List_of_Matrix_of_" + "M33l", typeof(List<Matrix<M33l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M33l>", "Volume_of_" + "M33l", typeof(Volume<M33l>), TypeInfo.Option.None),
                new TypeInfo("Volume<M33l>[]", "Array_of_Volume_of_" + "M33l", typeof(Volume<M33l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M33l>>", "List_of_Volume_of_" + "M33l", typeof(List<Volume<M33l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M33l>", "Tensor_of_" + "M33l", typeof(Tensor<M33l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M33l>[]", "Array_of_Tensor_of_" + "M33l", typeof(Tensor<M33l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M33l>>", "List_of_Tensor_of_" + "M33l", typeof(List<Tensor<M33l>>), TypeInfo.Option.None),

                #endregion

                #region M33f

                new TypeInfo("M33f", typeof(M33f), TypeInfo.Option.None),
                new TypeInfo(typeof(M33f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M33f>), TypeInfo.Option.None),

                new TypeInfo("Vector<M33f>", "Vector_of_" + "M33f", typeof(Vector<M33f>), TypeInfo.Option.None),
                new TypeInfo("Vector<M33f>[]", "Array_of_Vector_of_" + "M33f", typeof(Vector<M33f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M33f>>", "List_of_Vector_of_" + "M33f", typeof(List<Vector<M33f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M33f>", "Matrix_of_" + "M33f", typeof(Matrix<M33f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M33f>[]", "Array_of_Matrix_of_" + "M33f", typeof(Matrix<M33f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M33f>>", "List_of_Matrix_of_" + "M33f", typeof(List<Matrix<M33f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M33f>", "Volume_of_" + "M33f", typeof(Volume<M33f>), TypeInfo.Option.None),
                new TypeInfo("Volume<M33f>[]", "Array_of_Volume_of_" + "M33f", typeof(Volume<M33f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M33f>>", "List_of_Volume_of_" + "M33f", typeof(List<Volume<M33f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M33f>", "Tensor_of_" + "M33f", typeof(Tensor<M33f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M33f>[]", "Array_of_Tensor_of_" + "M33f", typeof(Tensor<M33f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M33f>>", "List_of_Tensor_of_" + "M33f", typeof(List<Tensor<M33f>>), TypeInfo.Option.None),

                #endregion

                #region M33d

                new TypeInfo("M33d", typeof(M33d), TypeInfo.Option.None),
                new TypeInfo(typeof(M33d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M33d>), TypeInfo.Option.None),

                new TypeInfo("Vector<M33d>", "Vector_of_" + "M33d", typeof(Vector<M33d>), TypeInfo.Option.None),
                new TypeInfo("Vector<M33d>[]", "Array_of_Vector_of_" + "M33d", typeof(Vector<M33d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M33d>>", "List_of_Vector_of_" + "M33d", typeof(List<Vector<M33d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M33d>", "Matrix_of_" + "M33d", typeof(Matrix<M33d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M33d>[]", "Array_of_Matrix_of_" + "M33d", typeof(Matrix<M33d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M33d>>", "List_of_Matrix_of_" + "M33d", typeof(List<Matrix<M33d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M33d>", "Volume_of_" + "M33d", typeof(Volume<M33d>), TypeInfo.Option.None),
                new TypeInfo("Volume<M33d>[]", "Array_of_Volume_of_" + "M33d", typeof(Volume<M33d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M33d>>", "List_of_Volume_of_" + "M33d", typeof(List<Volume<M33d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M33d>", "Tensor_of_" + "M33d", typeof(Tensor<M33d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M33d>[]", "Array_of_Tensor_of_" + "M33d", typeof(Tensor<M33d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M33d>>", "List_of_Tensor_of_" + "M33d", typeof(List<Tensor<M33d>>), TypeInfo.Option.None),

                #endregion

                #region M34i

                new TypeInfo("M34i", typeof(M34i), TypeInfo.Option.None),
                new TypeInfo(typeof(M34i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M34i>), TypeInfo.Option.None),

                new TypeInfo("Vector<M34i>", "Vector_of_" + "M34i", typeof(Vector<M34i>), TypeInfo.Option.None),
                new TypeInfo("Vector<M34i>[]", "Array_of_Vector_of_" + "M34i", typeof(Vector<M34i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M34i>>", "List_of_Vector_of_" + "M34i", typeof(List<Vector<M34i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M34i>", "Matrix_of_" + "M34i", typeof(Matrix<M34i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M34i>[]", "Array_of_Matrix_of_" + "M34i", typeof(Matrix<M34i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M34i>>", "List_of_Matrix_of_" + "M34i", typeof(List<Matrix<M34i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M34i>", "Volume_of_" + "M34i", typeof(Volume<M34i>), TypeInfo.Option.None),
                new TypeInfo("Volume<M34i>[]", "Array_of_Volume_of_" + "M34i", typeof(Volume<M34i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M34i>>", "List_of_Volume_of_" + "M34i", typeof(List<Volume<M34i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M34i>", "Tensor_of_" + "M34i", typeof(Tensor<M34i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M34i>[]", "Array_of_Tensor_of_" + "M34i", typeof(Tensor<M34i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M34i>>", "List_of_Tensor_of_" + "M34i", typeof(List<Tensor<M34i>>), TypeInfo.Option.None),

                #endregion

                #region M34l

                new TypeInfo("M34l", typeof(M34l), TypeInfo.Option.None),
                new TypeInfo(typeof(M34l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M34l>), TypeInfo.Option.None),

                new TypeInfo("Vector<M34l>", "Vector_of_" + "M34l", typeof(Vector<M34l>), TypeInfo.Option.None),
                new TypeInfo("Vector<M34l>[]", "Array_of_Vector_of_" + "M34l", typeof(Vector<M34l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M34l>>", "List_of_Vector_of_" + "M34l", typeof(List<Vector<M34l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M34l>", "Matrix_of_" + "M34l", typeof(Matrix<M34l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M34l>[]", "Array_of_Matrix_of_" + "M34l", typeof(Matrix<M34l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M34l>>", "List_of_Matrix_of_" + "M34l", typeof(List<Matrix<M34l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M34l>", "Volume_of_" + "M34l", typeof(Volume<M34l>), TypeInfo.Option.None),
                new TypeInfo("Volume<M34l>[]", "Array_of_Volume_of_" + "M34l", typeof(Volume<M34l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M34l>>", "List_of_Volume_of_" + "M34l", typeof(List<Volume<M34l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M34l>", "Tensor_of_" + "M34l", typeof(Tensor<M34l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M34l>[]", "Array_of_Tensor_of_" + "M34l", typeof(Tensor<M34l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M34l>>", "List_of_Tensor_of_" + "M34l", typeof(List<Tensor<M34l>>), TypeInfo.Option.None),

                #endregion

                #region M34f

                new TypeInfo("M34f", typeof(M34f), TypeInfo.Option.None),
                new TypeInfo(typeof(M34f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M34f>), TypeInfo.Option.None),

                new TypeInfo("Vector<M34f>", "Vector_of_" + "M34f", typeof(Vector<M34f>), TypeInfo.Option.None),
                new TypeInfo("Vector<M34f>[]", "Array_of_Vector_of_" + "M34f", typeof(Vector<M34f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M34f>>", "List_of_Vector_of_" + "M34f", typeof(List<Vector<M34f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M34f>", "Matrix_of_" + "M34f", typeof(Matrix<M34f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M34f>[]", "Array_of_Matrix_of_" + "M34f", typeof(Matrix<M34f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M34f>>", "List_of_Matrix_of_" + "M34f", typeof(List<Matrix<M34f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M34f>", "Volume_of_" + "M34f", typeof(Volume<M34f>), TypeInfo.Option.None),
                new TypeInfo("Volume<M34f>[]", "Array_of_Volume_of_" + "M34f", typeof(Volume<M34f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M34f>>", "List_of_Volume_of_" + "M34f", typeof(List<Volume<M34f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M34f>", "Tensor_of_" + "M34f", typeof(Tensor<M34f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M34f>[]", "Array_of_Tensor_of_" + "M34f", typeof(Tensor<M34f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M34f>>", "List_of_Tensor_of_" + "M34f", typeof(List<Tensor<M34f>>), TypeInfo.Option.None),

                #endregion

                #region M34d

                new TypeInfo("M34d", typeof(M34d), TypeInfo.Option.None),
                new TypeInfo(typeof(M34d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M34d>), TypeInfo.Option.None),

                new TypeInfo("Vector<M34d>", "Vector_of_" + "M34d", typeof(Vector<M34d>), TypeInfo.Option.None),
                new TypeInfo("Vector<M34d>[]", "Array_of_Vector_of_" + "M34d", typeof(Vector<M34d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M34d>>", "List_of_Vector_of_" + "M34d", typeof(List<Vector<M34d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M34d>", "Matrix_of_" + "M34d", typeof(Matrix<M34d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M34d>[]", "Array_of_Matrix_of_" + "M34d", typeof(Matrix<M34d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M34d>>", "List_of_Matrix_of_" + "M34d", typeof(List<Matrix<M34d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M34d>", "Volume_of_" + "M34d", typeof(Volume<M34d>), TypeInfo.Option.None),
                new TypeInfo("Volume<M34d>[]", "Array_of_Volume_of_" + "M34d", typeof(Volume<M34d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M34d>>", "List_of_Volume_of_" + "M34d", typeof(List<Volume<M34d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M34d>", "Tensor_of_" + "M34d", typeof(Tensor<M34d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M34d>[]", "Array_of_Tensor_of_" + "M34d", typeof(Tensor<M34d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M34d>>", "List_of_Tensor_of_" + "M34d", typeof(List<Tensor<M34d>>), TypeInfo.Option.None),

                #endregion

                #region M44i

                new TypeInfo("M44i", typeof(M44i), TypeInfo.Option.None),
                new TypeInfo(typeof(M44i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M44i>), TypeInfo.Option.None),

                new TypeInfo("Vector<M44i>", "Vector_of_" + "M44i", typeof(Vector<M44i>), TypeInfo.Option.None),
                new TypeInfo("Vector<M44i>[]", "Array_of_Vector_of_" + "M44i", typeof(Vector<M44i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M44i>>", "List_of_Vector_of_" + "M44i", typeof(List<Vector<M44i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M44i>", "Matrix_of_" + "M44i", typeof(Matrix<M44i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M44i>[]", "Array_of_Matrix_of_" + "M44i", typeof(Matrix<M44i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M44i>>", "List_of_Matrix_of_" + "M44i", typeof(List<Matrix<M44i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M44i>", "Volume_of_" + "M44i", typeof(Volume<M44i>), TypeInfo.Option.None),
                new TypeInfo("Volume<M44i>[]", "Array_of_Volume_of_" + "M44i", typeof(Volume<M44i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M44i>>", "List_of_Volume_of_" + "M44i", typeof(List<Volume<M44i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M44i>", "Tensor_of_" + "M44i", typeof(Tensor<M44i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M44i>[]", "Array_of_Tensor_of_" + "M44i", typeof(Tensor<M44i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M44i>>", "List_of_Tensor_of_" + "M44i", typeof(List<Tensor<M44i>>), TypeInfo.Option.None),

                #endregion

                #region M44l

                new TypeInfo("M44l", typeof(M44l), TypeInfo.Option.None),
                new TypeInfo(typeof(M44l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M44l>), TypeInfo.Option.None),

                new TypeInfo("Vector<M44l>", "Vector_of_" + "M44l", typeof(Vector<M44l>), TypeInfo.Option.None),
                new TypeInfo("Vector<M44l>[]", "Array_of_Vector_of_" + "M44l", typeof(Vector<M44l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M44l>>", "List_of_Vector_of_" + "M44l", typeof(List<Vector<M44l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M44l>", "Matrix_of_" + "M44l", typeof(Matrix<M44l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M44l>[]", "Array_of_Matrix_of_" + "M44l", typeof(Matrix<M44l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M44l>>", "List_of_Matrix_of_" + "M44l", typeof(List<Matrix<M44l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M44l>", "Volume_of_" + "M44l", typeof(Volume<M44l>), TypeInfo.Option.None),
                new TypeInfo("Volume<M44l>[]", "Array_of_Volume_of_" + "M44l", typeof(Volume<M44l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M44l>>", "List_of_Volume_of_" + "M44l", typeof(List<Volume<M44l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M44l>", "Tensor_of_" + "M44l", typeof(Tensor<M44l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M44l>[]", "Array_of_Tensor_of_" + "M44l", typeof(Tensor<M44l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M44l>>", "List_of_Tensor_of_" + "M44l", typeof(List<Tensor<M44l>>), TypeInfo.Option.None),

                #endregion

                #region M44f

                new TypeInfo("M44f", typeof(M44f), TypeInfo.Option.None),
                new TypeInfo(typeof(M44f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M44f>), TypeInfo.Option.None),

                new TypeInfo("Vector<M44f>", "Vector_of_" + "M44f", typeof(Vector<M44f>), TypeInfo.Option.None),
                new TypeInfo("Vector<M44f>[]", "Array_of_Vector_of_" + "M44f", typeof(Vector<M44f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M44f>>", "List_of_Vector_of_" + "M44f", typeof(List<Vector<M44f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M44f>", "Matrix_of_" + "M44f", typeof(Matrix<M44f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M44f>[]", "Array_of_Matrix_of_" + "M44f", typeof(Matrix<M44f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M44f>>", "List_of_Matrix_of_" + "M44f", typeof(List<Matrix<M44f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M44f>", "Volume_of_" + "M44f", typeof(Volume<M44f>), TypeInfo.Option.None),
                new TypeInfo("Volume<M44f>[]", "Array_of_Volume_of_" + "M44f", typeof(Volume<M44f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M44f>>", "List_of_Volume_of_" + "M44f", typeof(List<Volume<M44f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M44f>", "Tensor_of_" + "M44f", typeof(Tensor<M44f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M44f>[]", "Array_of_Tensor_of_" + "M44f", typeof(Tensor<M44f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M44f>>", "List_of_Tensor_of_" + "M44f", typeof(List<Tensor<M44f>>), TypeInfo.Option.None),

                #endregion

                #region M44d

                new TypeInfo("M44d", typeof(M44d), TypeInfo.Option.None),
                new TypeInfo(typeof(M44d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<M44d>), TypeInfo.Option.None),

                new TypeInfo("Vector<M44d>", "Vector_of_" + "M44d", typeof(Vector<M44d>), TypeInfo.Option.None),
                new TypeInfo("Vector<M44d>[]", "Array_of_Vector_of_" + "M44d", typeof(Vector<M44d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<M44d>>", "List_of_Vector_of_" + "M44d", typeof(List<Vector<M44d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<M44d>", "Matrix_of_" + "M44d", typeof(Matrix<M44d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<M44d>[]", "Array_of_Matrix_of_" + "M44d", typeof(Matrix<M44d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<M44d>>", "List_of_Matrix_of_" + "M44d", typeof(List<Matrix<M44d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<M44d>", "Volume_of_" + "M44d", typeof(Volume<M44d>), TypeInfo.Option.None),
                new TypeInfo("Volume<M44d>[]", "Array_of_Volume_of_" + "M44d", typeof(Volume<M44d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<M44d>>", "List_of_Volume_of_" + "M44d", typeof(List<Volume<M44d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<M44d>", "Tensor_of_" + "M44d", typeof(Tensor<M44d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<M44d>[]", "Array_of_Tensor_of_" + "M44d", typeof(Tensor<M44d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<M44d>>", "List_of_Tensor_of_" + "M44d", typeof(List<Tensor<M44d>>), TypeInfo.Option.None),

                #endregion

                #region C3b

                new TypeInfo("C3b", typeof(C3b), TypeInfo.Option.None),
                new TypeInfo(typeof(C3b[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C3b>), TypeInfo.Option.None),

                new TypeInfo("Vector<C3b>", "Vector_of_" + "C3b", typeof(Vector<C3b>), TypeInfo.Option.None),
                new TypeInfo("Vector<C3b>[]", "Array_of_Vector_of_" + "C3b", typeof(Vector<C3b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C3b>>", "List_of_Vector_of_" + "C3b", typeof(List<Vector<C3b>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C3b>", "Matrix_of_" + "C3b", typeof(Matrix<C3b>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C3b>[]", "Array_of_Matrix_of_" + "C3b", typeof(Matrix<C3b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C3b>>", "List_of_Matrix_of_" + "C3b", typeof(List<Matrix<C3b>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C3b>", "Volume_of_" + "C3b", typeof(Volume<C3b>), TypeInfo.Option.None),
                new TypeInfo("Volume<C3b>[]", "Array_of_Volume_of_" + "C3b", typeof(Volume<C3b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C3b>>", "List_of_Volume_of_" + "C3b", typeof(List<Volume<C3b>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C3b>", "Tensor_of_" + "C3b", typeof(Tensor<C3b>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C3b>[]", "Array_of_Tensor_of_" + "C3b", typeof(Tensor<C3b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C3b>>", "List_of_Tensor_of_" + "C3b", typeof(List<Tensor<C3b>>), TypeInfo.Option.None),

                #endregion

                #region C3us

                new TypeInfo("C3us", typeof(C3us), TypeInfo.Option.None),
                new TypeInfo(typeof(C3us[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C3us>), TypeInfo.Option.None),

                new TypeInfo("Vector<C3us>", "Vector_of_" + "C3us", typeof(Vector<C3us>), TypeInfo.Option.None),
                new TypeInfo("Vector<C3us>[]", "Array_of_Vector_of_" + "C3us", typeof(Vector<C3us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C3us>>", "List_of_Vector_of_" + "C3us", typeof(List<Vector<C3us>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C3us>", "Matrix_of_" + "C3us", typeof(Matrix<C3us>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C3us>[]", "Array_of_Matrix_of_" + "C3us", typeof(Matrix<C3us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C3us>>", "List_of_Matrix_of_" + "C3us", typeof(List<Matrix<C3us>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C3us>", "Volume_of_" + "C3us", typeof(Volume<C3us>), TypeInfo.Option.None),
                new TypeInfo("Volume<C3us>[]", "Array_of_Volume_of_" + "C3us", typeof(Volume<C3us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C3us>>", "List_of_Volume_of_" + "C3us", typeof(List<Volume<C3us>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C3us>", "Tensor_of_" + "C3us", typeof(Tensor<C3us>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C3us>[]", "Array_of_Tensor_of_" + "C3us", typeof(Tensor<C3us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C3us>>", "List_of_Tensor_of_" + "C3us", typeof(List<Tensor<C3us>>), TypeInfo.Option.None),

                #endregion

                #region C3ui

                new TypeInfo("C3ui", typeof(C3ui), TypeInfo.Option.None),
                new TypeInfo(typeof(C3ui[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C3ui>), TypeInfo.Option.None),

                new TypeInfo("Vector<C3ui>", "Vector_of_" + "C3ui", typeof(Vector<C3ui>), TypeInfo.Option.None),
                new TypeInfo("Vector<C3ui>[]", "Array_of_Vector_of_" + "C3ui", typeof(Vector<C3ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C3ui>>", "List_of_Vector_of_" + "C3ui", typeof(List<Vector<C3ui>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C3ui>", "Matrix_of_" + "C3ui", typeof(Matrix<C3ui>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C3ui>[]", "Array_of_Matrix_of_" + "C3ui", typeof(Matrix<C3ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C3ui>>", "List_of_Matrix_of_" + "C3ui", typeof(List<Matrix<C3ui>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C3ui>", "Volume_of_" + "C3ui", typeof(Volume<C3ui>), TypeInfo.Option.None),
                new TypeInfo("Volume<C3ui>[]", "Array_of_Volume_of_" + "C3ui", typeof(Volume<C3ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C3ui>>", "List_of_Volume_of_" + "C3ui", typeof(List<Volume<C3ui>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C3ui>", "Tensor_of_" + "C3ui", typeof(Tensor<C3ui>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C3ui>[]", "Array_of_Tensor_of_" + "C3ui", typeof(Tensor<C3ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C3ui>>", "List_of_Tensor_of_" + "C3ui", typeof(List<Tensor<C3ui>>), TypeInfo.Option.None),

                #endregion

                #region C3f

                new TypeInfo("C3f", typeof(C3f), TypeInfo.Option.None),
                new TypeInfo(typeof(C3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<C3f>", "Vector_of_" + "C3f", typeof(Vector<C3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<C3f>[]", "Array_of_Vector_of_" + "C3f", typeof(Vector<C3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C3f>>", "List_of_Vector_of_" + "C3f", typeof(List<Vector<C3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C3f>", "Matrix_of_" + "C3f", typeof(Matrix<C3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C3f>[]", "Array_of_Matrix_of_" + "C3f", typeof(Matrix<C3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C3f>>", "List_of_Matrix_of_" + "C3f", typeof(List<Matrix<C3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C3f>", "Volume_of_" + "C3f", typeof(Volume<C3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<C3f>[]", "Array_of_Volume_of_" + "C3f", typeof(Volume<C3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C3f>>", "List_of_Volume_of_" + "C3f", typeof(List<Volume<C3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C3f>", "Tensor_of_" + "C3f", typeof(Tensor<C3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C3f>[]", "Array_of_Tensor_of_" + "C3f", typeof(Tensor<C3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C3f>>", "List_of_Tensor_of_" + "C3f", typeof(List<Tensor<C3f>>), TypeInfo.Option.None),

                #endregion

                #region C3d

                new TypeInfo("C3d", typeof(C3d), TypeInfo.Option.None),
                new TypeInfo(typeof(C3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<C3d>", "Vector_of_" + "C3d", typeof(Vector<C3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<C3d>[]", "Array_of_Vector_of_" + "C3d", typeof(Vector<C3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C3d>>", "List_of_Vector_of_" + "C3d", typeof(List<Vector<C3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C3d>", "Matrix_of_" + "C3d", typeof(Matrix<C3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C3d>[]", "Array_of_Matrix_of_" + "C3d", typeof(Matrix<C3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C3d>>", "List_of_Matrix_of_" + "C3d", typeof(List<Matrix<C3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C3d>", "Volume_of_" + "C3d", typeof(Volume<C3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<C3d>[]", "Array_of_Volume_of_" + "C3d", typeof(Volume<C3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C3d>>", "List_of_Volume_of_" + "C3d", typeof(List<Volume<C3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C3d>", "Tensor_of_" + "C3d", typeof(Tensor<C3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C3d>[]", "Array_of_Tensor_of_" + "C3d", typeof(Tensor<C3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C3d>>", "List_of_Tensor_of_" + "C3d", typeof(List<Tensor<C3d>>), TypeInfo.Option.None),

                #endregion

                #region C4b

                new TypeInfo("C4b", typeof(C4b), TypeInfo.Option.None),
                new TypeInfo(typeof(C4b[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C4b>), TypeInfo.Option.None),

                new TypeInfo("Vector<C4b>", "Vector_of_" + "C4b", typeof(Vector<C4b>), TypeInfo.Option.None),
                new TypeInfo("Vector<C4b>[]", "Array_of_Vector_of_" + "C4b", typeof(Vector<C4b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C4b>>", "List_of_Vector_of_" + "C4b", typeof(List<Vector<C4b>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C4b>", "Matrix_of_" + "C4b", typeof(Matrix<C4b>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C4b>[]", "Array_of_Matrix_of_" + "C4b", typeof(Matrix<C4b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C4b>>", "List_of_Matrix_of_" + "C4b", typeof(List<Matrix<C4b>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C4b>", "Volume_of_" + "C4b", typeof(Volume<C4b>), TypeInfo.Option.None),
                new TypeInfo("Volume<C4b>[]", "Array_of_Volume_of_" + "C4b", typeof(Volume<C4b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C4b>>", "List_of_Volume_of_" + "C4b", typeof(List<Volume<C4b>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C4b>", "Tensor_of_" + "C4b", typeof(Tensor<C4b>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C4b>[]", "Array_of_Tensor_of_" + "C4b", typeof(Tensor<C4b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C4b>>", "List_of_Tensor_of_" + "C4b", typeof(List<Tensor<C4b>>), TypeInfo.Option.None),

                #endregion

                #region C4us

                new TypeInfo("C4us", typeof(C4us), TypeInfo.Option.None),
                new TypeInfo(typeof(C4us[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C4us>), TypeInfo.Option.None),

                new TypeInfo("Vector<C4us>", "Vector_of_" + "C4us", typeof(Vector<C4us>), TypeInfo.Option.None),
                new TypeInfo("Vector<C4us>[]", "Array_of_Vector_of_" + "C4us", typeof(Vector<C4us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C4us>>", "List_of_Vector_of_" + "C4us", typeof(List<Vector<C4us>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C4us>", "Matrix_of_" + "C4us", typeof(Matrix<C4us>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C4us>[]", "Array_of_Matrix_of_" + "C4us", typeof(Matrix<C4us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C4us>>", "List_of_Matrix_of_" + "C4us", typeof(List<Matrix<C4us>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C4us>", "Volume_of_" + "C4us", typeof(Volume<C4us>), TypeInfo.Option.None),
                new TypeInfo("Volume<C4us>[]", "Array_of_Volume_of_" + "C4us", typeof(Volume<C4us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C4us>>", "List_of_Volume_of_" + "C4us", typeof(List<Volume<C4us>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C4us>", "Tensor_of_" + "C4us", typeof(Tensor<C4us>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C4us>[]", "Array_of_Tensor_of_" + "C4us", typeof(Tensor<C4us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C4us>>", "List_of_Tensor_of_" + "C4us", typeof(List<Tensor<C4us>>), TypeInfo.Option.None),

                #endregion

                #region C4ui

                new TypeInfo("C4ui", typeof(C4ui), TypeInfo.Option.None),
                new TypeInfo(typeof(C4ui[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C4ui>), TypeInfo.Option.None),

                new TypeInfo("Vector<C4ui>", "Vector_of_" + "C4ui", typeof(Vector<C4ui>), TypeInfo.Option.None),
                new TypeInfo("Vector<C4ui>[]", "Array_of_Vector_of_" + "C4ui", typeof(Vector<C4ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C4ui>>", "List_of_Vector_of_" + "C4ui", typeof(List<Vector<C4ui>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C4ui>", "Matrix_of_" + "C4ui", typeof(Matrix<C4ui>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C4ui>[]", "Array_of_Matrix_of_" + "C4ui", typeof(Matrix<C4ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C4ui>>", "List_of_Matrix_of_" + "C4ui", typeof(List<Matrix<C4ui>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C4ui>", "Volume_of_" + "C4ui", typeof(Volume<C4ui>), TypeInfo.Option.None),
                new TypeInfo("Volume<C4ui>[]", "Array_of_Volume_of_" + "C4ui", typeof(Volume<C4ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C4ui>>", "List_of_Volume_of_" + "C4ui", typeof(List<Volume<C4ui>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C4ui>", "Tensor_of_" + "C4ui", typeof(Tensor<C4ui>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C4ui>[]", "Array_of_Tensor_of_" + "C4ui", typeof(Tensor<C4ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C4ui>>", "List_of_Tensor_of_" + "C4ui", typeof(List<Tensor<C4ui>>), TypeInfo.Option.None),

                #endregion

                #region C4f

                new TypeInfo("C4f", typeof(C4f), TypeInfo.Option.None),
                new TypeInfo(typeof(C4f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C4f>), TypeInfo.Option.None),

                new TypeInfo("Vector<C4f>", "Vector_of_" + "C4f", typeof(Vector<C4f>), TypeInfo.Option.None),
                new TypeInfo("Vector<C4f>[]", "Array_of_Vector_of_" + "C4f", typeof(Vector<C4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C4f>>", "List_of_Vector_of_" + "C4f", typeof(List<Vector<C4f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C4f>", "Matrix_of_" + "C4f", typeof(Matrix<C4f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C4f>[]", "Array_of_Matrix_of_" + "C4f", typeof(Matrix<C4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C4f>>", "List_of_Matrix_of_" + "C4f", typeof(List<Matrix<C4f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C4f>", "Volume_of_" + "C4f", typeof(Volume<C4f>), TypeInfo.Option.None),
                new TypeInfo("Volume<C4f>[]", "Array_of_Volume_of_" + "C4f", typeof(Volume<C4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C4f>>", "List_of_Volume_of_" + "C4f", typeof(List<Volume<C4f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C4f>", "Tensor_of_" + "C4f", typeof(Tensor<C4f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C4f>[]", "Array_of_Tensor_of_" + "C4f", typeof(Tensor<C4f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C4f>>", "List_of_Tensor_of_" + "C4f", typeof(List<Tensor<C4f>>), TypeInfo.Option.None),

                #endregion

                #region C4d

                new TypeInfo("C4d", typeof(C4d), TypeInfo.Option.None),
                new TypeInfo(typeof(C4d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<C4d>), TypeInfo.Option.None),

                new TypeInfo("Vector<C4d>", "Vector_of_" + "C4d", typeof(Vector<C4d>), TypeInfo.Option.None),
                new TypeInfo("Vector<C4d>[]", "Array_of_Vector_of_" + "C4d", typeof(Vector<C4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<C4d>>", "List_of_Vector_of_" + "C4d", typeof(List<Vector<C4d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<C4d>", "Matrix_of_" + "C4d", typeof(Matrix<C4d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<C4d>[]", "Array_of_Matrix_of_" + "C4d", typeof(Matrix<C4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<C4d>>", "List_of_Matrix_of_" + "C4d", typeof(List<Matrix<C4d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<C4d>", "Volume_of_" + "C4d", typeof(Volume<C4d>), TypeInfo.Option.None),
                new TypeInfo("Volume<C4d>[]", "Array_of_Volume_of_" + "C4d", typeof(Volume<C4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<C4d>>", "List_of_Volume_of_" + "C4d", typeof(List<Volume<C4d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<C4d>", "Tensor_of_" + "C4d", typeof(Tensor<C4d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<C4d>[]", "Array_of_Tensor_of_" + "C4d", typeof(Tensor<C4d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<C4d>>", "List_of_Tensor_of_" + "C4d", typeof(List<Tensor<C4d>>), TypeInfo.Option.None),

                #endregion

                #region Range1b

                new TypeInfo("Range1b", typeof(Range1b), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1b[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1b>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1b>", "Vector_of_" + "Range1b", typeof(Vector<Range1b>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1b>[]", "Array_of_Vector_of_" + "Range1b", typeof(Vector<Range1b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1b>>", "List_of_Vector_of_" + "Range1b", typeof(List<Vector<Range1b>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1b>", "Matrix_of_" + "Range1b", typeof(Matrix<Range1b>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1b>[]", "Array_of_Matrix_of_" + "Range1b", typeof(Matrix<Range1b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1b>>", "List_of_Matrix_of_" + "Range1b", typeof(List<Matrix<Range1b>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1b>", "Volume_of_" + "Range1b", typeof(Volume<Range1b>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1b>[]", "Array_of_Volume_of_" + "Range1b", typeof(Volume<Range1b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1b>>", "List_of_Volume_of_" + "Range1b", typeof(List<Volume<Range1b>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1b>", "Tensor_of_" + "Range1b", typeof(Tensor<Range1b>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1b>[]", "Array_of_Tensor_of_" + "Range1b", typeof(Tensor<Range1b>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1b>>", "List_of_Tensor_of_" + "Range1b", typeof(List<Tensor<Range1b>>), TypeInfo.Option.None),

                #endregion

                #region Range1sb

                new TypeInfo("Range1sb", typeof(Range1sb), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1sb[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1sb>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1sb>", "Vector_of_" + "Range1sb", typeof(Vector<Range1sb>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1sb>[]", "Array_of_Vector_of_" + "Range1sb", typeof(Vector<Range1sb>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1sb>>", "List_of_Vector_of_" + "Range1sb", typeof(List<Vector<Range1sb>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1sb>", "Matrix_of_" + "Range1sb", typeof(Matrix<Range1sb>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1sb>[]", "Array_of_Matrix_of_" + "Range1sb", typeof(Matrix<Range1sb>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1sb>>", "List_of_Matrix_of_" + "Range1sb", typeof(List<Matrix<Range1sb>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1sb>", "Volume_of_" + "Range1sb", typeof(Volume<Range1sb>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1sb>[]", "Array_of_Volume_of_" + "Range1sb", typeof(Volume<Range1sb>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1sb>>", "List_of_Volume_of_" + "Range1sb", typeof(List<Volume<Range1sb>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1sb>", "Tensor_of_" + "Range1sb", typeof(Tensor<Range1sb>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1sb>[]", "Array_of_Tensor_of_" + "Range1sb", typeof(Tensor<Range1sb>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1sb>>", "List_of_Tensor_of_" + "Range1sb", typeof(List<Tensor<Range1sb>>), TypeInfo.Option.None),

                #endregion

                #region Range1s

                new TypeInfo("Range1s", typeof(Range1s), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1s[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1s>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1s>", "Vector_of_" + "Range1s", typeof(Vector<Range1s>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1s>[]", "Array_of_Vector_of_" + "Range1s", typeof(Vector<Range1s>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1s>>", "List_of_Vector_of_" + "Range1s", typeof(List<Vector<Range1s>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1s>", "Matrix_of_" + "Range1s", typeof(Matrix<Range1s>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1s>[]", "Array_of_Matrix_of_" + "Range1s", typeof(Matrix<Range1s>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1s>>", "List_of_Matrix_of_" + "Range1s", typeof(List<Matrix<Range1s>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1s>", "Volume_of_" + "Range1s", typeof(Volume<Range1s>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1s>[]", "Array_of_Volume_of_" + "Range1s", typeof(Volume<Range1s>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1s>>", "List_of_Volume_of_" + "Range1s", typeof(List<Volume<Range1s>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1s>", "Tensor_of_" + "Range1s", typeof(Tensor<Range1s>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1s>[]", "Array_of_Tensor_of_" + "Range1s", typeof(Tensor<Range1s>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1s>>", "List_of_Tensor_of_" + "Range1s", typeof(List<Tensor<Range1s>>), TypeInfo.Option.None),

                #endregion

                #region Range1us

                new TypeInfo("Range1us", typeof(Range1us), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1us[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1us>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1us>", "Vector_of_" + "Range1us", typeof(Vector<Range1us>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1us>[]", "Array_of_Vector_of_" + "Range1us", typeof(Vector<Range1us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1us>>", "List_of_Vector_of_" + "Range1us", typeof(List<Vector<Range1us>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1us>", "Matrix_of_" + "Range1us", typeof(Matrix<Range1us>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1us>[]", "Array_of_Matrix_of_" + "Range1us", typeof(Matrix<Range1us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1us>>", "List_of_Matrix_of_" + "Range1us", typeof(List<Matrix<Range1us>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1us>", "Volume_of_" + "Range1us", typeof(Volume<Range1us>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1us>[]", "Array_of_Volume_of_" + "Range1us", typeof(Volume<Range1us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1us>>", "List_of_Volume_of_" + "Range1us", typeof(List<Volume<Range1us>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1us>", "Tensor_of_" + "Range1us", typeof(Tensor<Range1us>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1us>[]", "Array_of_Tensor_of_" + "Range1us", typeof(Tensor<Range1us>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1us>>", "List_of_Tensor_of_" + "Range1us", typeof(List<Tensor<Range1us>>), TypeInfo.Option.None),

                #endregion

                #region Range1i

                new TypeInfo("Range1i", typeof(Range1i), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1i>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1i>", "Vector_of_" + "Range1i", typeof(Vector<Range1i>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1i>[]", "Array_of_Vector_of_" + "Range1i", typeof(Vector<Range1i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1i>>", "List_of_Vector_of_" + "Range1i", typeof(List<Vector<Range1i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1i>", "Matrix_of_" + "Range1i", typeof(Matrix<Range1i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1i>[]", "Array_of_Matrix_of_" + "Range1i", typeof(Matrix<Range1i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1i>>", "List_of_Matrix_of_" + "Range1i", typeof(List<Matrix<Range1i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1i>", "Volume_of_" + "Range1i", typeof(Volume<Range1i>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1i>[]", "Array_of_Volume_of_" + "Range1i", typeof(Volume<Range1i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1i>>", "List_of_Volume_of_" + "Range1i", typeof(List<Volume<Range1i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1i>", "Tensor_of_" + "Range1i", typeof(Tensor<Range1i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1i>[]", "Array_of_Tensor_of_" + "Range1i", typeof(Tensor<Range1i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1i>>", "List_of_Tensor_of_" + "Range1i", typeof(List<Tensor<Range1i>>), TypeInfo.Option.None),

                #endregion

                #region Range1ui

                new TypeInfo("Range1ui", typeof(Range1ui), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1ui[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1ui>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1ui>", "Vector_of_" + "Range1ui", typeof(Vector<Range1ui>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1ui>[]", "Array_of_Vector_of_" + "Range1ui", typeof(Vector<Range1ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1ui>>", "List_of_Vector_of_" + "Range1ui", typeof(List<Vector<Range1ui>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1ui>", "Matrix_of_" + "Range1ui", typeof(Matrix<Range1ui>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1ui>[]", "Array_of_Matrix_of_" + "Range1ui", typeof(Matrix<Range1ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1ui>>", "List_of_Matrix_of_" + "Range1ui", typeof(List<Matrix<Range1ui>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1ui>", "Volume_of_" + "Range1ui", typeof(Volume<Range1ui>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1ui>[]", "Array_of_Volume_of_" + "Range1ui", typeof(Volume<Range1ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1ui>>", "List_of_Volume_of_" + "Range1ui", typeof(List<Volume<Range1ui>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1ui>", "Tensor_of_" + "Range1ui", typeof(Tensor<Range1ui>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1ui>[]", "Array_of_Tensor_of_" + "Range1ui", typeof(Tensor<Range1ui>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1ui>>", "List_of_Tensor_of_" + "Range1ui", typeof(List<Tensor<Range1ui>>), TypeInfo.Option.None),

                #endregion

                #region Range1l

                new TypeInfo("Range1l", typeof(Range1l), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1l>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1l>", "Vector_of_" + "Range1l", typeof(Vector<Range1l>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1l>[]", "Array_of_Vector_of_" + "Range1l", typeof(Vector<Range1l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1l>>", "List_of_Vector_of_" + "Range1l", typeof(List<Vector<Range1l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1l>", "Matrix_of_" + "Range1l", typeof(Matrix<Range1l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1l>[]", "Array_of_Matrix_of_" + "Range1l", typeof(Matrix<Range1l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1l>>", "List_of_Matrix_of_" + "Range1l", typeof(List<Matrix<Range1l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1l>", "Volume_of_" + "Range1l", typeof(Volume<Range1l>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1l>[]", "Array_of_Volume_of_" + "Range1l", typeof(Volume<Range1l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1l>>", "List_of_Volume_of_" + "Range1l", typeof(List<Volume<Range1l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1l>", "Tensor_of_" + "Range1l", typeof(Tensor<Range1l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1l>[]", "Array_of_Tensor_of_" + "Range1l", typeof(Tensor<Range1l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1l>>", "List_of_Tensor_of_" + "Range1l", typeof(List<Tensor<Range1l>>), TypeInfo.Option.None),

                #endregion

                #region Range1ul

                new TypeInfo("Range1ul", typeof(Range1ul), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1ul[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1ul>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1ul>", "Vector_of_" + "Range1ul", typeof(Vector<Range1ul>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1ul>[]", "Array_of_Vector_of_" + "Range1ul", typeof(Vector<Range1ul>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1ul>>", "List_of_Vector_of_" + "Range1ul", typeof(List<Vector<Range1ul>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1ul>", "Matrix_of_" + "Range1ul", typeof(Matrix<Range1ul>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1ul>[]", "Array_of_Matrix_of_" + "Range1ul", typeof(Matrix<Range1ul>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1ul>>", "List_of_Matrix_of_" + "Range1ul", typeof(List<Matrix<Range1ul>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1ul>", "Volume_of_" + "Range1ul", typeof(Volume<Range1ul>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1ul>[]", "Array_of_Volume_of_" + "Range1ul", typeof(Volume<Range1ul>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1ul>>", "List_of_Volume_of_" + "Range1ul", typeof(List<Volume<Range1ul>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1ul>", "Tensor_of_" + "Range1ul", typeof(Tensor<Range1ul>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1ul>[]", "Array_of_Tensor_of_" + "Range1ul", typeof(Tensor<Range1ul>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1ul>>", "List_of_Tensor_of_" + "Range1ul", typeof(List<Tensor<Range1ul>>), TypeInfo.Option.None),

                #endregion

                #region Range1f

                new TypeInfo("Range1f", typeof(Range1f), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1f>", "Vector_of_" + "Range1f", typeof(Vector<Range1f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1f>[]", "Array_of_Vector_of_" + "Range1f", typeof(Vector<Range1f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1f>>", "List_of_Vector_of_" + "Range1f", typeof(List<Vector<Range1f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1f>", "Matrix_of_" + "Range1f", typeof(Matrix<Range1f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1f>[]", "Array_of_Matrix_of_" + "Range1f", typeof(Matrix<Range1f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1f>>", "List_of_Matrix_of_" + "Range1f", typeof(List<Matrix<Range1f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1f>", "Volume_of_" + "Range1f", typeof(Volume<Range1f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1f>[]", "Array_of_Volume_of_" + "Range1f", typeof(Volume<Range1f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1f>>", "List_of_Volume_of_" + "Range1f", typeof(List<Volume<Range1f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1f>", "Tensor_of_" + "Range1f", typeof(Tensor<Range1f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1f>[]", "Array_of_Tensor_of_" + "Range1f", typeof(Tensor<Range1f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1f>>", "List_of_Tensor_of_" + "Range1f", typeof(List<Tensor<Range1f>>), TypeInfo.Option.None),

                #endregion

                #region Range1d

                new TypeInfo("Range1d", typeof(Range1d), TypeInfo.Option.None),
                new TypeInfo(typeof(Range1d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Range1d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Range1d>", "Vector_of_" + "Range1d", typeof(Vector<Range1d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Range1d>[]", "Array_of_Vector_of_" + "Range1d", typeof(Vector<Range1d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Range1d>>", "List_of_Vector_of_" + "Range1d", typeof(List<Vector<Range1d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Range1d>", "Matrix_of_" + "Range1d", typeof(Matrix<Range1d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Range1d>[]", "Array_of_Matrix_of_" + "Range1d", typeof(Matrix<Range1d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Range1d>>", "List_of_Matrix_of_" + "Range1d", typeof(List<Matrix<Range1d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Range1d>", "Volume_of_" + "Range1d", typeof(Volume<Range1d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Range1d>[]", "Array_of_Volume_of_" + "Range1d", typeof(Volume<Range1d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Range1d>>", "List_of_Volume_of_" + "Range1d", typeof(List<Volume<Range1d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Range1d>", "Tensor_of_" + "Range1d", typeof(Tensor<Range1d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Range1d>[]", "Array_of_Tensor_of_" + "Range1d", typeof(Tensor<Range1d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Range1d>>", "List_of_Tensor_of_" + "Range1d", typeof(List<Tensor<Range1d>>), TypeInfo.Option.None),

                #endregion

                #region Box2i

                new TypeInfo("Box2i", typeof(Box2i), TypeInfo.Option.None),
                new TypeInfo(typeof(Box2i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box2i>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box2i>", "Vector_of_" + "Box2i", typeof(Vector<Box2i>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box2i>[]", "Array_of_Vector_of_" + "Box2i", typeof(Vector<Box2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box2i>>", "List_of_Vector_of_" + "Box2i", typeof(List<Vector<Box2i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box2i>", "Matrix_of_" + "Box2i", typeof(Matrix<Box2i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box2i>[]", "Array_of_Matrix_of_" + "Box2i", typeof(Matrix<Box2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box2i>>", "List_of_Matrix_of_" + "Box2i", typeof(List<Matrix<Box2i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box2i>", "Volume_of_" + "Box2i", typeof(Volume<Box2i>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box2i>[]", "Array_of_Volume_of_" + "Box2i", typeof(Volume<Box2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box2i>>", "List_of_Volume_of_" + "Box2i", typeof(List<Volume<Box2i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box2i>", "Tensor_of_" + "Box2i", typeof(Tensor<Box2i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box2i>[]", "Array_of_Tensor_of_" + "Box2i", typeof(Tensor<Box2i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box2i>>", "List_of_Tensor_of_" + "Box2i", typeof(List<Tensor<Box2i>>), TypeInfo.Option.None),

                #endregion

                #region Box2l

                new TypeInfo("Box2l", typeof(Box2l), TypeInfo.Option.None),
                new TypeInfo(typeof(Box2l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box2l>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box2l>", "Vector_of_" + "Box2l", typeof(Vector<Box2l>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box2l>[]", "Array_of_Vector_of_" + "Box2l", typeof(Vector<Box2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box2l>>", "List_of_Vector_of_" + "Box2l", typeof(List<Vector<Box2l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box2l>", "Matrix_of_" + "Box2l", typeof(Matrix<Box2l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box2l>[]", "Array_of_Matrix_of_" + "Box2l", typeof(Matrix<Box2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box2l>>", "List_of_Matrix_of_" + "Box2l", typeof(List<Matrix<Box2l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box2l>", "Volume_of_" + "Box2l", typeof(Volume<Box2l>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box2l>[]", "Array_of_Volume_of_" + "Box2l", typeof(Volume<Box2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box2l>>", "List_of_Volume_of_" + "Box2l", typeof(List<Volume<Box2l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box2l>", "Tensor_of_" + "Box2l", typeof(Tensor<Box2l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box2l>[]", "Array_of_Tensor_of_" + "Box2l", typeof(Tensor<Box2l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box2l>>", "List_of_Tensor_of_" + "Box2l", typeof(List<Tensor<Box2l>>), TypeInfo.Option.None),

                #endregion

                #region Box2f

                new TypeInfo("Box2f", typeof(Box2f), TypeInfo.Option.None),
                new TypeInfo(typeof(Box2f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box2f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box2f>", "Vector_of_" + "Box2f", typeof(Vector<Box2f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box2f>[]", "Array_of_Vector_of_" + "Box2f", typeof(Vector<Box2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box2f>>", "List_of_Vector_of_" + "Box2f", typeof(List<Vector<Box2f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box2f>", "Matrix_of_" + "Box2f", typeof(Matrix<Box2f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box2f>[]", "Array_of_Matrix_of_" + "Box2f", typeof(Matrix<Box2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box2f>>", "List_of_Matrix_of_" + "Box2f", typeof(List<Matrix<Box2f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box2f>", "Volume_of_" + "Box2f", typeof(Volume<Box2f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box2f>[]", "Array_of_Volume_of_" + "Box2f", typeof(Volume<Box2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box2f>>", "List_of_Volume_of_" + "Box2f", typeof(List<Volume<Box2f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box2f>", "Tensor_of_" + "Box2f", typeof(Tensor<Box2f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box2f>[]", "Array_of_Tensor_of_" + "Box2f", typeof(Tensor<Box2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box2f>>", "List_of_Tensor_of_" + "Box2f", typeof(List<Tensor<Box2f>>), TypeInfo.Option.None),

                #endregion

                #region Box2d

                new TypeInfo("Box2d", typeof(Box2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Box2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box2d>", "Vector_of_" + "Box2d", typeof(Vector<Box2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box2d>[]", "Array_of_Vector_of_" + "Box2d", typeof(Vector<Box2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box2d>>", "List_of_Vector_of_" + "Box2d", typeof(List<Vector<Box2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box2d>", "Matrix_of_" + "Box2d", typeof(Matrix<Box2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box2d>[]", "Array_of_Matrix_of_" + "Box2d", typeof(Matrix<Box2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box2d>>", "List_of_Matrix_of_" + "Box2d", typeof(List<Matrix<Box2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box2d>", "Volume_of_" + "Box2d", typeof(Volume<Box2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box2d>[]", "Array_of_Volume_of_" + "Box2d", typeof(Volume<Box2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box2d>>", "List_of_Volume_of_" + "Box2d", typeof(List<Volume<Box2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box2d>", "Tensor_of_" + "Box2d", typeof(Tensor<Box2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box2d>[]", "Array_of_Tensor_of_" + "Box2d", typeof(Tensor<Box2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box2d>>", "List_of_Tensor_of_" + "Box2d", typeof(List<Tensor<Box2d>>), TypeInfo.Option.None),

                #endregion

                #region Box3i

                new TypeInfo("Box3i", typeof(Box3i), TypeInfo.Option.None),
                new TypeInfo(typeof(Box3i[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box3i>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box3i>", "Vector_of_" + "Box3i", typeof(Vector<Box3i>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box3i>[]", "Array_of_Vector_of_" + "Box3i", typeof(Vector<Box3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box3i>>", "List_of_Vector_of_" + "Box3i", typeof(List<Vector<Box3i>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box3i>", "Matrix_of_" + "Box3i", typeof(Matrix<Box3i>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box3i>[]", "Array_of_Matrix_of_" + "Box3i", typeof(Matrix<Box3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box3i>>", "List_of_Matrix_of_" + "Box3i", typeof(List<Matrix<Box3i>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box3i>", "Volume_of_" + "Box3i", typeof(Volume<Box3i>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box3i>[]", "Array_of_Volume_of_" + "Box3i", typeof(Volume<Box3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box3i>>", "List_of_Volume_of_" + "Box3i", typeof(List<Volume<Box3i>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box3i>", "Tensor_of_" + "Box3i", typeof(Tensor<Box3i>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box3i>[]", "Array_of_Tensor_of_" + "Box3i", typeof(Tensor<Box3i>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box3i>>", "List_of_Tensor_of_" + "Box3i", typeof(List<Tensor<Box3i>>), TypeInfo.Option.None),

                #endregion

                #region Box3l

                new TypeInfo("Box3l", typeof(Box3l), TypeInfo.Option.None),
                new TypeInfo(typeof(Box3l[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box3l>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box3l>", "Vector_of_" + "Box3l", typeof(Vector<Box3l>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box3l>[]", "Array_of_Vector_of_" + "Box3l", typeof(Vector<Box3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box3l>>", "List_of_Vector_of_" + "Box3l", typeof(List<Vector<Box3l>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box3l>", "Matrix_of_" + "Box3l", typeof(Matrix<Box3l>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box3l>[]", "Array_of_Matrix_of_" + "Box3l", typeof(Matrix<Box3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box3l>>", "List_of_Matrix_of_" + "Box3l", typeof(List<Matrix<Box3l>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box3l>", "Volume_of_" + "Box3l", typeof(Volume<Box3l>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box3l>[]", "Array_of_Volume_of_" + "Box3l", typeof(Volume<Box3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box3l>>", "List_of_Volume_of_" + "Box3l", typeof(List<Volume<Box3l>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box3l>", "Tensor_of_" + "Box3l", typeof(Tensor<Box3l>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box3l>[]", "Array_of_Tensor_of_" + "Box3l", typeof(Tensor<Box3l>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box3l>>", "List_of_Tensor_of_" + "Box3l", typeof(List<Tensor<Box3l>>), TypeInfo.Option.None),

                #endregion

                #region Box3f

                new TypeInfo("Box3f", typeof(Box3f), TypeInfo.Option.None),
                new TypeInfo(typeof(Box3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box3f>", "Vector_of_" + "Box3f", typeof(Vector<Box3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box3f>[]", "Array_of_Vector_of_" + "Box3f", typeof(Vector<Box3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box3f>>", "List_of_Vector_of_" + "Box3f", typeof(List<Vector<Box3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box3f>", "Matrix_of_" + "Box3f", typeof(Matrix<Box3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box3f>[]", "Array_of_Matrix_of_" + "Box3f", typeof(Matrix<Box3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box3f>>", "List_of_Matrix_of_" + "Box3f", typeof(List<Matrix<Box3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box3f>", "Volume_of_" + "Box3f", typeof(Volume<Box3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box3f>[]", "Array_of_Volume_of_" + "Box3f", typeof(Volume<Box3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box3f>>", "List_of_Volume_of_" + "Box3f", typeof(List<Volume<Box3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box3f>", "Tensor_of_" + "Box3f", typeof(Tensor<Box3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box3f>[]", "Array_of_Tensor_of_" + "Box3f", typeof(Tensor<Box3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box3f>>", "List_of_Tensor_of_" + "Box3f", typeof(List<Tensor<Box3f>>), TypeInfo.Option.None),

                #endregion

                #region Box3d

                new TypeInfo("Box3d", typeof(Box3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Box3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Box3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Box3d>", "Vector_of_" + "Box3d", typeof(Vector<Box3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Box3d>[]", "Array_of_Vector_of_" + "Box3d", typeof(Vector<Box3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Box3d>>", "List_of_Vector_of_" + "Box3d", typeof(List<Vector<Box3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Box3d>", "Matrix_of_" + "Box3d", typeof(Matrix<Box3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Box3d>[]", "Array_of_Matrix_of_" + "Box3d", typeof(Matrix<Box3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Box3d>>", "List_of_Matrix_of_" + "Box3d", typeof(List<Matrix<Box3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Box3d>", "Volume_of_" + "Box3d", typeof(Volume<Box3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Box3d>[]", "Array_of_Volume_of_" + "Box3d", typeof(Volume<Box3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Box3d>>", "List_of_Volume_of_" + "Box3d", typeof(List<Volume<Box3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Box3d>", "Tensor_of_" + "Box3d", typeof(Tensor<Box3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Box3d>[]", "Array_of_Tensor_of_" + "Box3d", typeof(Tensor<Box3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Box3d>>", "List_of_Tensor_of_" + "Box3d", typeof(List<Tensor<Box3d>>), TypeInfo.Option.None),

                #endregion

                #region Euclidean3f

                new TypeInfo("Euclidean3f", typeof(Euclidean3f), TypeInfo.Option.None),
                new TypeInfo(typeof(Euclidean3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Euclidean3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Euclidean3f>", "Vector_of_" + "Euclidean3f", typeof(Vector<Euclidean3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Euclidean3f>[]", "Array_of_Vector_of_" + "Euclidean3f", typeof(Vector<Euclidean3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Euclidean3f>>", "List_of_Vector_of_" + "Euclidean3f", typeof(List<Vector<Euclidean3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Euclidean3f>", "Matrix_of_" + "Euclidean3f", typeof(Matrix<Euclidean3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Euclidean3f>[]", "Array_of_Matrix_of_" + "Euclidean3f", typeof(Matrix<Euclidean3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Euclidean3f>>", "List_of_Matrix_of_" + "Euclidean3f", typeof(List<Matrix<Euclidean3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Euclidean3f>", "Volume_of_" + "Euclidean3f", typeof(Volume<Euclidean3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Euclidean3f>[]", "Array_of_Volume_of_" + "Euclidean3f", typeof(Volume<Euclidean3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Euclidean3f>>", "List_of_Volume_of_" + "Euclidean3f", typeof(List<Volume<Euclidean3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Euclidean3f>", "Tensor_of_" + "Euclidean3f", typeof(Tensor<Euclidean3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Euclidean3f>[]", "Array_of_Tensor_of_" + "Euclidean3f", typeof(Tensor<Euclidean3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Euclidean3f>>", "List_of_Tensor_of_" + "Euclidean3f", typeof(List<Tensor<Euclidean3f>>), TypeInfo.Option.None),

                #endregion

                #region Euclidean3d

                new TypeInfo("Euclidean3d", typeof(Euclidean3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Euclidean3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Euclidean3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Euclidean3d>", "Vector_of_" + "Euclidean3d", typeof(Vector<Euclidean3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Euclidean3d>[]", "Array_of_Vector_of_" + "Euclidean3d", typeof(Vector<Euclidean3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Euclidean3d>>", "List_of_Vector_of_" + "Euclidean3d", typeof(List<Vector<Euclidean3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Euclidean3d>", "Matrix_of_" + "Euclidean3d", typeof(Matrix<Euclidean3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Euclidean3d>[]", "Array_of_Matrix_of_" + "Euclidean3d", typeof(Matrix<Euclidean3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Euclidean3d>>", "List_of_Matrix_of_" + "Euclidean3d", typeof(List<Matrix<Euclidean3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Euclidean3d>", "Volume_of_" + "Euclidean3d", typeof(Volume<Euclidean3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Euclidean3d>[]", "Array_of_Volume_of_" + "Euclidean3d", typeof(Volume<Euclidean3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Euclidean3d>>", "List_of_Volume_of_" + "Euclidean3d", typeof(List<Volume<Euclidean3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Euclidean3d>", "Tensor_of_" + "Euclidean3d", typeof(Tensor<Euclidean3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Euclidean3d>[]", "Array_of_Tensor_of_" + "Euclidean3d", typeof(Tensor<Euclidean3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Euclidean3d>>", "List_of_Tensor_of_" + "Euclidean3d", typeof(List<Tensor<Euclidean3d>>), TypeInfo.Option.None),

                #endregion

                #region Rot2f

                new TypeInfo("Rot2f", typeof(Rot2f), TypeInfo.Option.None),
                new TypeInfo(typeof(Rot2f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Rot2f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Rot2f>", "Vector_of_" + "Rot2f", typeof(Vector<Rot2f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Rot2f>[]", "Array_of_Vector_of_" + "Rot2f", typeof(Vector<Rot2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Rot2f>>", "List_of_Vector_of_" + "Rot2f", typeof(List<Vector<Rot2f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Rot2f>", "Matrix_of_" + "Rot2f", typeof(Matrix<Rot2f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Rot2f>[]", "Array_of_Matrix_of_" + "Rot2f", typeof(Matrix<Rot2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Rot2f>>", "List_of_Matrix_of_" + "Rot2f", typeof(List<Matrix<Rot2f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Rot2f>", "Volume_of_" + "Rot2f", typeof(Volume<Rot2f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Rot2f>[]", "Array_of_Volume_of_" + "Rot2f", typeof(Volume<Rot2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Rot2f>>", "List_of_Volume_of_" + "Rot2f", typeof(List<Volume<Rot2f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Rot2f>", "Tensor_of_" + "Rot2f", typeof(Tensor<Rot2f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Rot2f>[]", "Array_of_Tensor_of_" + "Rot2f", typeof(Tensor<Rot2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Rot2f>>", "List_of_Tensor_of_" + "Rot2f", typeof(List<Tensor<Rot2f>>), TypeInfo.Option.None),

                #endregion

                #region Rot2d

                new TypeInfo("Rot2d", typeof(Rot2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Rot2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Rot2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Rot2d>", "Vector_of_" + "Rot2d", typeof(Vector<Rot2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Rot2d>[]", "Array_of_Vector_of_" + "Rot2d", typeof(Vector<Rot2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Rot2d>>", "List_of_Vector_of_" + "Rot2d", typeof(List<Vector<Rot2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Rot2d>", "Matrix_of_" + "Rot2d", typeof(Matrix<Rot2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Rot2d>[]", "Array_of_Matrix_of_" + "Rot2d", typeof(Matrix<Rot2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Rot2d>>", "List_of_Matrix_of_" + "Rot2d", typeof(List<Matrix<Rot2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Rot2d>", "Volume_of_" + "Rot2d", typeof(Volume<Rot2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Rot2d>[]", "Array_of_Volume_of_" + "Rot2d", typeof(Volume<Rot2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Rot2d>>", "List_of_Volume_of_" + "Rot2d", typeof(List<Volume<Rot2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Rot2d>", "Tensor_of_" + "Rot2d", typeof(Tensor<Rot2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Rot2d>[]", "Array_of_Tensor_of_" + "Rot2d", typeof(Tensor<Rot2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Rot2d>>", "List_of_Tensor_of_" + "Rot2d", typeof(List<Tensor<Rot2d>>), TypeInfo.Option.None),

                #endregion

                #region Rot3f

                new TypeInfo("Rot3f", typeof(Rot3f), TypeInfo.Option.None),
                new TypeInfo(typeof(Rot3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Rot3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Rot3f>", "Vector_of_" + "Rot3f", typeof(Vector<Rot3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Rot3f>[]", "Array_of_Vector_of_" + "Rot3f", typeof(Vector<Rot3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Rot3f>>", "List_of_Vector_of_" + "Rot3f", typeof(List<Vector<Rot3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Rot3f>", "Matrix_of_" + "Rot3f", typeof(Matrix<Rot3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Rot3f>[]", "Array_of_Matrix_of_" + "Rot3f", typeof(Matrix<Rot3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Rot3f>>", "List_of_Matrix_of_" + "Rot3f", typeof(List<Matrix<Rot3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Rot3f>", "Volume_of_" + "Rot3f", typeof(Volume<Rot3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Rot3f>[]", "Array_of_Volume_of_" + "Rot3f", typeof(Volume<Rot3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Rot3f>>", "List_of_Volume_of_" + "Rot3f", typeof(List<Volume<Rot3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Rot3f>", "Tensor_of_" + "Rot3f", typeof(Tensor<Rot3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Rot3f>[]", "Array_of_Tensor_of_" + "Rot3f", typeof(Tensor<Rot3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Rot3f>>", "List_of_Tensor_of_" + "Rot3f", typeof(List<Tensor<Rot3f>>), TypeInfo.Option.None),

                #endregion

                #region Rot3d

                new TypeInfo("Rot3d", typeof(Rot3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Rot3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Rot3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Rot3d>", "Vector_of_" + "Rot3d", typeof(Vector<Rot3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Rot3d>[]", "Array_of_Vector_of_" + "Rot3d", typeof(Vector<Rot3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Rot3d>>", "List_of_Vector_of_" + "Rot3d", typeof(List<Vector<Rot3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Rot3d>", "Matrix_of_" + "Rot3d", typeof(Matrix<Rot3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Rot3d>[]", "Array_of_Matrix_of_" + "Rot3d", typeof(Matrix<Rot3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Rot3d>>", "List_of_Matrix_of_" + "Rot3d", typeof(List<Matrix<Rot3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Rot3d>", "Volume_of_" + "Rot3d", typeof(Volume<Rot3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Rot3d>[]", "Array_of_Volume_of_" + "Rot3d", typeof(Volume<Rot3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Rot3d>>", "List_of_Volume_of_" + "Rot3d", typeof(List<Volume<Rot3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Rot3d>", "Tensor_of_" + "Rot3d", typeof(Tensor<Rot3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Rot3d>[]", "Array_of_Tensor_of_" + "Rot3d", typeof(Tensor<Rot3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Rot3d>>", "List_of_Tensor_of_" + "Rot3d", typeof(List<Tensor<Rot3d>>), TypeInfo.Option.None),

                #endregion

                #region Scale3f

                new TypeInfo("Scale3f", typeof(Scale3f), TypeInfo.Option.None),
                new TypeInfo(typeof(Scale3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Scale3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Scale3f>", "Vector_of_" + "Scale3f", typeof(Vector<Scale3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Scale3f>[]", "Array_of_Vector_of_" + "Scale3f", typeof(Vector<Scale3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Scale3f>>", "List_of_Vector_of_" + "Scale3f", typeof(List<Vector<Scale3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Scale3f>", "Matrix_of_" + "Scale3f", typeof(Matrix<Scale3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Scale3f>[]", "Array_of_Matrix_of_" + "Scale3f", typeof(Matrix<Scale3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Scale3f>>", "List_of_Matrix_of_" + "Scale3f", typeof(List<Matrix<Scale3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Scale3f>", "Volume_of_" + "Scale3f", typeof(Volume<Scale3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Scale3f>[]", "Array_of_Volume_of_" + "Scale3f", typeof(Volume<Scale3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Scale3f>>", "List_of_Volume_of_" + "Scale3f", typeof(List<Volume<Scale3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Scale3f>", "Tensor_of_" + "Scale3f", typeof(Tensor<Scale3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Scale3f>[]", "Array_of_Tensor_of_" + "Scale3f", typeof(Tensor<Scale3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Scale3f>>", "List_of_Tensor_of_" + "Scale3f", typeof(List<Tensor<Scale3f>>), TypeInfo.Option.None),

                #endregion

                #region Scale3d

                new TypeInfo("Scale3d", typeof(Scale3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Scale3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Scale3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Scale3d>", "Vector_of_" + "Scale3d", typeof(Vector<Scale3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Scale3d>[]", "Array_of_Vector_of_" + "Scale3d", typeof(Vector<Scale3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Scale3d>>", "List_of_Vector_of_" + "Scale3d", typeof(List<Vector<Scale3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Scale3d>", "Matrix_of_" + "Scale3d", typeof(Matrix<Scale3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Scale3d>[]", "Array_of_Matrix_of_" + "Scale3d", typeof(Matrix<Scale3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Scale3d>>", "List_of_Matrix_of_" + "Scale3d", typeof(List<Matrix<Scale3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Scale3d>", "Volume_of_" + "Scale3d", typeof(Volume<Scale3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Scale3d>[]", "Array_of_Volume_of_" + "Scale3d", typeof(Volume<Scale3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Scale3d>>", "List_of_Volume_of_" + "Scale3d", typeof(List<Volume<Scale3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Scale3d>", "Tensor_of_" + "Scale3d", typeof(Tensor<Scale3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Scale3d>[]", "Array_of_Tensor_of_" + "Scale3d", typeof(Tensor<Scale3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Scale3d>>", "List_of_Tensor_of_" + "Scale3d", typeof(List<Tensor<Scale3d>>), TypeInfo.Option.None),

                #endregion

                #region Shift3f

                new TypeInfo("Shift3f", typeof(Shift3f), TypeInfo.Option.None),
                new TypeInfo(typeof(Shift3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Shift3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Shift3f>", "Vector_of_" + "Shift3f", typeof(Vector<Shift3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Shift3f>[]", "Array_of_Vector_of_" + "Shift3f", typeof(Vector<Shift3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Shift3f>>", "List_of_Vector_of_" + "Shift3f", typeof(List<Vector<Shift3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Shift3f>", "Matrix_of_" + "Shift3f", typeof(Matrix<Shift3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Shift3f>[]", "Array_of_Matrix_of_" + "Shift3f", typeof(Matrix<Shift3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Shift3f>>", "List_of_Matrix_of_" + "Shift3f", typeof(List<Matrix<Shift3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Shift3f>", "Volume_of_" + "Shift3f", typeof(Volume<Shift3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Shift3f>[]", "Array_of_Volume_of_" + "Shift3f", typeof(Volume<Shift3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Shift3f>>", "List_of_Volume_of_" + "Shift3f", typeof(List<Volume<Shift3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Shift3f>", "Tensor_of_" + "Shift3f", typeof(Tensor<Shift3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Shift3f>[]", "Array_of_Tensor_of_" + "Shift3f", typeof(Tensor<Shift3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Shift3f>>", "List_of_Tensor_of_" + "Shift3f", typeof(List<Tensor<Shift3f>>), TypeInfo.Option.None),

                #endregion

                #region Shift3d

                new TypeInfo("Shift3d", typeof(Shift3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Shift3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Shift3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Shift3d>", "Vector_of_" + "Shift3d", typeof(Vector<Shift3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Shift3d>[]", "Array_of_Vector_of_" + "Shift3d", typeof(Vector<Shift3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Shift3d>>", "List_of_Vector_of_" + "Shift3d", typeof(List<Vector<Shift3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Shift3d>", "Matrix_of_" + "Shift3d", typeof(Matrix<Shift3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Shift3d>[]", "Array_of_Matrix_of_" + "Shift3d", typeof(Matrix<Shift3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Shift3d>>", "List_of_Matrix_of_" + "Shift3d", typeof(List<Matrix<Shift3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Shift3d>", "Volume_of_" + "Shift3d", typeof(Volume<Shift3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Shift3d>[]", "Array_of_Volume_of_" + "Shift3d", typeof(Volume<Shift3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Shift3d>>", "List_of_Volume_of_" + "Shift3d", typeof(List<Volume<Shift3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Shift3d>", "Tensor_of_" + "Shift3d", typeof(Tensor<Shift3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Shift3d>[]", "Array_of_Tensor_of_" + "Shift3d", typeof(Tensor<Shift3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Shift3d>>", "List_of_Tensor_of_" + "Shift3d", typeof(List<Tensor<Shift3d>>), TypeInfo.Option.None),

                #endregion

                #region Trafo2f

                new TypeInfo("Trafo2f", typeof(Trafo2f), TypeInfo.Option.None),
                new TypeInfo(typeof(Trafo2f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Trafo2f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Trafo2f>", "Vector_of_" + "Trafo2f", typeof(Vector<Trafo2f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Trafo2f>[]", "Array_of_Vector_of_" + "Trafo2f", typeof(Vector<Trafo2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Trafo2f>>", "List_of_Vector_of_" + "Trafo2f", typeof(List<Vector<Trafo2f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Trafo2f>", "Matrix_of_" + "Trafo2f", typeof(Matrix<Trafo2f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Trafo2f>[]", "Array_of_Matrix_of_" + "Trafo2f", typeof(Matrix<Trafo2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Trafo2f>>", "List_of_Matrix_of_" + "Trafo2f", typeof(List<Matrix<Trafo2f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Trafo2f>", "Volume_of_" + "Trafo2f", typeof(Volume<Trafo2f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Trafo2f>[]", "Array_of_Volume_of_" + "Trafo2f", typeof(Volume<Trafo2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Trafo2f>>", "List_of_Volume_of_" + "Trafo2f", typeof(List<Volume<Trafo2f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Trafo2f>", "Tensor_of_" + "Trafo2f", typeof(Tensor<Trafo2f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Trafo2f>[]", "Array_of_Tensor_of_" + "Trafo2f", typeof(Tensor<Trafo2f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Trafo2f>>", "List_of_Tensor_of_" + "Trafo2f", typeof(List<Tensor<Trafo2f>>), TypeInfo.Option.None),

                #endregion

                #region Trafo2d

                new TypeInfo("Trafo2d", typeof(Trafo2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Trafo2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Trafo2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Trafo2d>", "Vector_of_" + "Trafo2d", typeof(Vector<Trafo2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Trafo2d>[]", "Array_of_Vector_of_" + "Trafo2d", typeof(Vector<Trafo2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Trafo2d>>", "List_of_Vector_of_" + "Trafo2d", typeof(List<Vector<Trafo2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Trafo2d>", "Matrix_of_" + "Trafo2d", typeof(Matrix<Trafo2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Trafo2d>[]", "Array_of_Matrix_of_" + "Trafo2d", typeof(Matrix<Trafo2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Trafo2d>>", "List_of_Matrix_of_" + "Trafo2d", typeof(List<Matrix<Trafo2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Trafo2d>", "Volume_of_" + "Trafo2d", typeof(Volume<Trafo2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Trafo2d>[]", "Array_of_Volume_of_" + "Trafo2d", typeof(Volume<Trafo2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Trafo2d>>", "List_of_Volume_of_" + "Trafo2d", typeof(List<Volume<Trafo2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Trafo2d>", "Tensor_of_" + "Trafo2d", typeof(Tensor<Trafo2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Trafo2d>[]", "Array_of_Tensor_of_" + "Trafo2d", typeof(Tensor<Trafo2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Trafo2d>>", "List_of_Tensor_of_" + "Trafo2d", typeof(List<Tensor<Trafo2d>>), TypeInfo.Option.None),

                #endregion

                #region Trafo3f

                new TypeInfo("Trafo3f", typeof(Trafo3f), TypeInfo.Option.None),
                new TypeInfo(typeof(Trafo3f[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Trafo3f>), TypeInfo.Option.None),

                new TypeInfo("Vector<Trafo3f>", "Vector_of_" + "Trafo3f", typeof(Vector<Trafo3f>), TypeInfo.Option.None),
                new TypeInfo("Vector<Trafo3f>[]", "Array_of_Vector_of_" + "Trafo3f", typeof(Vector<Trafo3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Trafo3f>>", "List_of_Vector_of_" + "Trafo3f", typeof(List<Vector<Trafo3f>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Trafo3f>", "Matrix_of_" + "Trafo3f", typeof(Matrix<Trafo3f>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Trafo3f>[]", "Array_of_Matrix_of_" + "Trafo3f", typeof(Matrix<Trafo3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Trafo3f>>", "List_of_Matrix_of_" + "Trafo3f", typeof(List<Matrix<Trafo3f>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Trafo3f>", "Volume_of_" + "Trafo3f", typeof(Volume<Trafo3f>), TypeInfo.Option.None),
                new TypeInfo("Volume<Trafo3f>[]", "Array_of_Volume_of_" + "Trafo3f", typeof(Volume<Trafo3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Trafo3f>>", "List_of_Volume_of_" + "Trafo3f", typeof(List<Volume<Trafo3f>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Trafo3f>", "Tensor_of_" + "Trafo3f", typeof(Tensor<Trafo3f>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Trafo3f>[]", "Array_of_Tensor_of_" + "Trafo3f", typeof(Tensor<Trafo3f>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Trafo3f>>", "List_of_Tensor_of_" + "Trafo3f", typeof(List<Tensor<Trafo3f>>), TypeInfo.Option.None),

                #endregion

                #region Trafo3d

                new TypeInfo("Trafo3d", typeof(Trafo3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Trafo3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Trafo3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Trafo3d>", "Vector_of_" + "Trafo3d", typeof(Vector<Trafo3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Trafo3d>[]", "Array_of_Vector_of_" + "Trafo3d", typeof(Vector<Trafo3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Trafo3d>>", "List_of_Vector_of_" + "Trafo3d", typeof(List<Vector<Trafo3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Trafo3d>", "Matrix_of_" + "Trafo3d", typeof(Matrix<Trafo3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Trafo3d>[]", "Array_of_Matrix_of_" + "Trafo3d", typeof(Matrix<Trafo3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Trafo3d>>", "List_of_Matrix_of_" + "Trafo3d", typeof(List<Matrix<Trafo3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Trafo3d>", "Volume_of_" + "Trafo3d", typeof(Volume<Trafo3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Trafo3d>[]", "Array_of_Volume_of_" + "Trafo3d", typeof(Volume<Trafo3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Trafo3d>>", "List_of_Volume_of_" + "Trafo3d", typeof(List<Volume<Trafo3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Trafo3d>", "Tensor_of_" + "Trafo3d", typeof(Tensor<Trafo3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Trafo3d>[]", "Array_of_Tensor_of_" + "Trafo3d", typeof(Tensor<Trafo3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Trafo3d>>", "List_of_Tensor_of_" + "Trafo3d", typeof(List<Tensor<Trafo3d>>), TypeInfo.Option.None),

                #endregion

                #region bool

                new TypeInfo("bool", typeof(bool), TypeInfo.Option.None),
                new TypeInfo(typeof(bool[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<bool>), TypeInfo.Option.None),

                new TypeInfo("Vector<bool>", "Vector_of_" + "bool", typeof(Vector<bool>), TypeInfo.Option.None),
                new TypeInfo("Vector<bool>[]", "Array_of_Vector_of_" + "bool", typeof(Vector<bool>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<bool>>", "List_of_Vector_of_" + "bool", typeof(List<Vector<bool>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<bool>", "Matrix_of_" + "bool", typeof(Matrix<bool>), TypeInfo.Option.None),
                new TypeInfo("Matrix<bool>[]", "Array_of_Matrix_of_" + "bool", typeof(Matrix<bool>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<bool>>", "List_of_Matrix_of_" + "bool", typeof(List<Matrix<bool>>), TypeInfo.Option.None),

                new TypeInfo("Volume<bool>", "Volume_of_" + "bool", typeof(Volume<bool>), TypeInfo.Option.None),
                new TypeInfo("Volume<bool>[]", "Array_of_Volume_of_" + "bool", typeof(Volume<bool>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<bool>>", "List_of_Volume_of_" + "bool", typeof(List<Volume<bool>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<bool>", "Tensor_of_" + "bool", typeof(Tensor<bool>), TypeInfo.Option.None),
                new TypeInfo("Tensor<bool>[]", "Array_of_Tensor_of_" + "bool", typeof(Tensor<bool>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<bool>>", "List_of_Tensor_of_" + "bool", typeof(List<Tensor<bool>>), TypeInfo.Option.None),

                #endregion

                #region char

                new TypeInfo("char", typeof(char), TypeInfo.Option.None),
                new TypeInfo(typeof(char[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<char>), TypeInfo.Option.None),

                new TypeInfo("Vector<char>", "Vector_of_" + "char", typeof(Vector<char>), TypeInfo.Option.None),
                new TypeInfo("Vector<char>[]", "Array_of_Vector_of_" + "char", typeof(Vector<char>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<char>>", "List_of_Vector_of_" + "char", typeof(List<Vector<char>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<char>", "Matrix_of_" + "char", typeof(Matrix<char>), TypeInfo.Option.None),
                new TypeInfo("Matrix<char>[]", "Array_of_Matrix_of_" + "char", typeof(Matrix<char>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<char>>", "List_of_Matrix_of_" + "char", typeof(List<Matrix<char>>), TypeInfo.Option.None),

                new TypeInfo("Volume<char>", "Volume_of_" + "char", typeof(Volume<char>), TypeInfo.Option.None),
                new TypeInfo("Volume<char>[]", "Array_of_Volume_of_" + "char", typeof(Volume<char>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<char>>", "List_of_Volume_of_" + "char", typeof(List<Volume<char>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<char>", "Tensor_of_" + "char", typeof(Tensor<char>), TypeInfo.Option.None),
                new TypeInfo("Tensor<char>[]", "Array_of_Tensor_of_" + "char", typeof(Tensor<char>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<char>>", "List_of_Tensor_of_" + "char", typeof(List<Tensor<char>>), TypeInfo.Option.None),

                #endregion

                #region string

                new TypeInfo("string", typeof(string), TypeInfo.Option.None),
                new TypeInfo(typeof(string[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<string>), TypeInfo.Option.None),

                new TypeInfo("Vector<string>", "Vector_of_" + "string", typeof(Vector<string>), TypeInfo.Option.None),
                new TypeInfo("Vector<string>[]", "Array_of_Vector_of_" + "string", typeof(Vector<string>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<string>>", "List_of_Vector_of_" + "string", typeof(List<Vector<string>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<string>", "Matrix_of_" + "string", typeof(Matrix<string>), TypeInfo.Option.None),
                new TypeInfo("Matrix<string>[]", "Array_of_Matrix_of_" + "string", typeof(Matrix<string>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<string>>", "List_of_Matrix_of_" + "string", typeof(List<Matrix<string>>), TypeInfo.Option.None),

                new TypeInfo("Volume<string>", "Volume_of_" + "string", typeof(Volume<string>), TypeInfo.Option.None),
                new TypeInfo("Volume<string>[]", "Array_of_Volume_of_" + "string", typeof(Volume<string>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<string>>", "List_of_Volume_of_" + "string", typeof(List<Volume<string>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<string>", "Tensor_of_" + "string", typeof(Tensor<string>), TypeInfo.Option.None),
                new TypeInfo("Tensor<string>[]", "Array_of_Tensor_of_" + "string", typeof(Tensor<string>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<string>>", "List_of_Tensor_of_" + "string", typeof(List<Tensor<string>>), TypeInfo.Option.None),

                #endregion

                #region Type

                new TypeInfo("Type", typeof(Type), TypeInfo.Option.None),
                new TypeInfo(typeof(Type[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Type>), TypeInfo.Option.None),

                new TypeInfo("Vector<Type>", "Vector_of_" + "Type", typeof(Vector<Type>), TypeInfo.Option.None),
                new TypeInfo("Vector<Type>[]", "Array_of_Vector_of_" + "Type", typeof(Vector<Type>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Type>>", "List_of_Vector_of_" + "Type", typeof(List<Vector<Type>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Type>", "Matrix_of_" + "Type", typeof(Matrix<Type>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Type>[]", "Array_of_Matrix_of_" + "Type", typeof(Matrix<Type>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Type>>", "List_of_Matrix_of_" + "Type", typeof(List<Matrix<Type>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Type>", "Volume_of_" + "Type", typeof(Volume<Type>), TypeInfo.Option.None),
                new TypeInfo("Volume<Type>[]", "Array_of_Volume_of_" + "Type", typeof(Volume<Type>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Type>>", "List_of_Volume_of_" + "Type", typeof(List<Volume<Type>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Type>", "Tensor_of_" + "Type", typeof(Tensor<Type>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Type>[]", "Array_of_Tensor_of_" + "Type", typeof(Tensor<Type>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Type>>", "List_of_Tensor_of_" + "Type", typeof(List<Tensor<Type>>), TypeInfo.Option.None),

                #endregion

                #region Guid

                new TypeInfo("Guid", typeof(Guid), TypeInfo.Option.None),
                new TypeInfo(typeof(Guid[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Guid>), TypeInfo.Option.None),

                new TypeInfo("Vector<Guid>", "Vector_of_" + "Guid", typeof(Vector<Guid>), TypeInfo.Option.None),
                new TypeInfo("Vector<Guid>[]", "Array_of_Vector_of_" + "Guid", typeof(Vector<Guid>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Guid>>", "List_of_Vector_of_" + "Guid", typeof(List<Vector<Guid>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Guid>", "Matrix_of_" + "Guid", typeof(Matrix<Guid>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Guid>[]", "Array_of_Matrix_of_" + "Guid", typeof(Matrix<Guid>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Guid>>", "List_of_Matrix_of_" + "Guid", typeof(List<Matrix<Guid>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Guid>", "Volume_of_" + "Guid", typeof(Volume<Guid>), TypeInfo.Option.None),
                new TypeInfo("Volume<Guid>[]", "Array_of_Volume_of_" + "Guid", typeof(Volume<Guid>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Guid>>", "List_of_Volume_of_" + "Guid", typeof(List<Volume<Guid>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Guid>", "Tensor_of_" + "Guid", typeof(Tensor<Guid>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Guid>[]", "Array_of_Tensor_of_" + "Guid", typeof(Tensor<Guid>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Guid>>", "List_of_Tensor_of_" + "Guid", typeof(List<Tensor<Guid>>), TypeInfo.Option.None),

                #endregion

                #region Symbol

                new TypeInfo("Symbol", typeof(Symbol), TypeInfo.Option.None),
                new TypeInfo(typeof(Symbol[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Symbol>), TypeInfo.Option.None),

                new TypeInfo("Vector<Symbol>", "Vector_of_" + "Symbol", typeof(Vector<Symbol>), TypeInfo.Option.None),
                new TypeInfo("Vector<Symbol>[]", "Array_of_Vector_of_" + "Symbol", typeof(Vector<Symbol>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Symbol>>", "List_of_Vector_of_" + "Symbol", typeof(List<Vector<Symbol>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Symbol>", "Matrix_of_" + "Symbol", typeof(Matrix<Symbol>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Symbol>[]", "Array_of_Matrix_of_" + "Symbol", typeof(Matrix<Symbol>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Symbol>>", "List_of_Matrix_of_" + "Symbol", typeof(List<Matrix<Symbol>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Symbol>", "Volume_of_" + "Symbol", typeof(Volume<Symbol>), TypeInfo.Option.None),
                new TypeInfo("Volume<Symbol>[]", "Array_of_Volume_of_" + "Symbol", typeof(Volume<Symbol>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Symbol>>", "List_of_Volume_of_" + "Symbol", typeof(List<Volume<Symbol>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Symbol>", "Tensor_of_" + "Symbol", typeof(Tensor<Symbol>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Symbol>[]", "Array_of_Tensor_of_" + "Symbol", typeof(Tensor<Symbol>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Symbol>>", "List_of_Tensor_of_" + "Symbol", typeof(List<Tensor<Symbol>>), TypeInfo.Option.None),

                #endregion

                #region Circle2d

                new TypeInfo("Circle2d", typeof(Circle2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Circle2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Circle2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Circle2d>", "Vector_of_" + "Circle2d", typeof(Vector<Circle2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Circle2d>[]", "Array_of_Vector_of_" + "Circle2d", typeof(Vector<Circle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Circle2d>>", "List_of_Vector_of_" + "Circle2d", typeof(List<Vector<Circle2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Circle2d>", "Matrix_of_" + "Circle2d", typeof(Matrix<Circle2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Circle2d>[]", "Array_of_Matrix_of_" + "Circle2d", typeof(Matrix<Circle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Circle2d>>", "List_of_Matrix_of_" + "Circle2d", typeof(List<Matrix<Circle2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Circle2d>", "Volume_of_" + "Circle2d", typeof(Volume<Circle2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Circle2d>[]", "Array_of_Volume_of_" + "Circle2d", typeof(Volume<Circle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Circle2d>>", "List_of_Volume_of_" + "Circle2d", typeof(List<Volume<Circle2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Circle2d>", "Tensor_of_" + "Circle2d", typeof(Tensor<Circle2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Circle2d>[]", "Array_of_Tensor_of_" + "Circle2d", typeof(Tensor<Circle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Circle2d>>", "List_of_Tensor_of_" + "Circle2d", typeof(List<Tensor<Circle2d>>), TypeInfo.Option.None),

                #endregion

                #region Line2d

                new TypeInfo("Line2d", typeof(Line2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Line2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Line2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Line2d>", "Vector_of_" + "Line2d", typeof(Vector<Line2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Line2d>[]", "Array_of_Vector_of_" + "Line2d", typeof(Vector<Line2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Line2d>>", "List_of_Vector_of_" + "Line2d", typeof(List<Vector<Line2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Line2d>", "Matrix_of_" + "Line2d", typeof(Matrix<Line2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Line2d>[]", "Array_of_Matrix_of_" + "Line2d", typeof(Matrix<Line2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Line2d>>", "List_of_Matrix_of_" + "Line2d", typeof(List<Matrix<Line2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Line2d>", "Volume_of_" + "Line2d", typeof(Volume<Line2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Line2d>[]", "Array_of_Volume_of_" + "Line2d", typeof(Volume<Line2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Line2d>>", "List_of_Volume_of_" + "Line2d", typeof(List<Volume<Line2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Line2d>", "Tensor_of_" + "Line2d", typeof(Tensor<Line2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Line2d>[]", "Array_of_Tensor_of_" + "Line2d", typeof(Tensor<Line2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Line2d>>", "List_of_Tensor_of_" + "Line2d", typeof(List<Tensor<Line2d>>), TypeInfo.Option.None),

                #endregion

                #region Line3d

                new TypeInfo("Line3d", typeof(Line3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Line3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Line3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Line3d>", "Vector_of_" + "Line3d", typeof(Vector<Line3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Line3d>[]", "Array_of_Vector_of_" + "Line3d", typeof(Vector<Line3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Line3d>>", "List_of_Vector_of_" + "Line3d", typeof(List<Vector<Line3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Line3d>", "Matrix_of_" + "Line3d", typeof(Matrix<Line3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Line3d>[]", "Array_of_Matrix_of_" + "Line3d", typeof(Matrix<Line3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Line3d>>", "List_of_Matrix_of_" + "Line3d", typeof(List<Matrix<Line3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Line3d>", "Volume_of_" + "Line3d", typeof(Volume<Line3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Line3d>[]", "Array_of_Volume_of_" + "Line3d", typeof(Volume<Line3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Line3d>>", "List_of_Volume_of_" + "Line3d", typeof(List<Volume<Line3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Line3d>", "Tensor_of_" + "Line3d", typeof(Tensor<Line3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Line3d>[]", "Array_of_Tensor_of_" + "Line3d", typeof(Tensor<Line3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Line3d>>", "List_of_Tensor_of_" + "Line3d", typeof(List<Tensor<Line3d>>), TypeInfo.Option.None),

                #endregion

                #region Plane2d

                new TypeInfo("Plane2d", typeof(Plane2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Plane2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Plane2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Plane2d>", "Vector_of_" + "Plane2d", typeof(Vector<Plane2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Plane2d>[]", "Array_of_Vector_of_" + "Plane2d", typeof(Vector<Plane2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Plane2d>>", "List_of_Vector_of_" + "Plane2d", typeof(List<Vector<Plane2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Plane2d>", "Matrix_of_" + "Plane2d", typeof(Matrix<Plane2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Plane2d>[]", "Array_of_Matrix_of_" + "Plane2d", typeof(Matrix<Plane2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Plane2d>>", "List_of_Matrix_of_" + "Plane2d", typeof(List<Matrix<Plane2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Plane2d>", "Volume_of_" + "Plane2d", typeof(Volume<Plane2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Plane2d>[]", "Array_of_Volume_of_" + "Plane2d", typeof(Volume<Plane2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Plane2d>>", "List_of_Volume_of_" + "Plane2d", typeof(List<Volume<Plane2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Plane2d>", "Tensor_of_" + "Plane2d", typeof(Tensor<Plane2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Plane2d>[]", "Array_of_Tensor_of_" + "Plane2d", typeof(Tensor<Plane2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Plane2d>>", "List_of_Tensor_of_" + "Plane2d", typeof(List<Tensor<Plane2d>>), TypeInfo.Option.None),

                #endregion

                #region Plane3d

                new TypeInfo("Plane3d", typeof(Plane3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Plane3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Plane3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Plane3d>", "Vector_of_" + "Plane3d", typeof(Vector<Plane3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Plane3d>[]", "Array_of_Vector_of_" + "Plane3d", typeof(Vector<Plane3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Plane3d>>", "List_of_Vector_of_" + "Plane3d", typeof(List<Vector<Plane3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Plane3d>", "Matrix_of_" + "Plane3d", typeof(Matrix<Plane3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Plane3d>[]", "Array_of_Matrix_of_" + "Plane3d", typeof(Matrix<Plane3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Plane3d>>", "List_of_Matrix_of_" + "Plane3d", typeof(List<Matrix<Plane3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Plane3d>", "Volume_of_" + "Plane3d", typeof(Volume<Plane3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Plane3d>[]", "Array_of_Volume_of_" + "Plane3d", typeof(Volume<Plane3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Plane3d>>", "List_of_Volume_of_" + "Plane3d", typeof(List<Volume<Plane3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Plane3d>", "Tensor_of_" + "Plane3d", typeof(Tensor<Plane3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Plane3d>[]", "Array_of_Tensor_of_" + "Plane3d", typeof(Tensor<Plane3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Plane3d>>", "List_of_Tensor_of_" + "Plane3d", typeof(List<Tensor<Plane3d>>), TypeInfo.Option.None),

                #endregion

                #region PlaneWithPoint3d

                new TypeInfo("PlaneWithPoint3d", typeof(PlaneWithPoint3d), TypeInfo.Option.None),
                new TypeInfo(typeof(PlaneWithPoint3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<PlaneWithPoint3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<PlaneWithPoint3d>", "Vector_of_" + "PlaneWithPoint3d", typeof(Vector<PlaneWithPoint3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<PlaneWithPoint3d>[]", "Array_of_Vector_of_" + "PlaneWithPoint3d", typeof(Vector<PlaneWithPoint3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<PlaneWithPoint3d>>", "List_of_Vector_of_" + "PlaneWithPoint3d", typeof(List<Vector<PlaneWithPoint3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<PlaneWithPoint3d>", "Matrix_of_" + "PlaneWithPoint3d", typeof(Matrix<PlaneWithPoint3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<PlaneWithPoint3d>[]", "Array_of_Matrix_of_" + "PlaneWithPoint3d", typeof(Matrix<PlaneWithPoint3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<PlaneWithPoint3d>>", "List_of_Matrix_of_" + "PlaneWithPoint3d", typeof(List<Matrix<PlaneWithPoint3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<PlaneWithPoint3d>", "Volume_of_" + "PlaneWithPoint3d", typeof(Volume<PlaneWithPoint3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<PlaneWithPoint3d>[]", "Array_of_Volume_of_" + "PlaneWithPoint3d", typeof(Volume<PlaneWithPoint3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<PlaneWithPoint3d>>", "List_of_Volume_of_" + "PlaneWithPoint3d", typeof(List<Volume<PlaneWithPoint3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<PlaneWithPoint3d>", "Tensor_of_" + "PlaneWithPoint3d", typeof(Tensor<PlaneWithPoint3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<PlaneWithPoint3d>[]", "Array_of_Tensor_of_" + "PlaneWithPoint3d", typeof(Tensor<PlaneWithPoint3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<PlaneWithPoint3d>>", "List_of_Tensor_of_" + "PlaneWithPoint3d", typeof(List<Tensor<PlaneWithPoint3d>>), TypeInfo.Option.None),

                #endregion

                #region Quad2d

                new TypeInfo("Quad2d", typeof(Quad2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Quad2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Quad2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Quad2d>", "Vector_of_" + "Quad2d", typeof(Vector<Quad2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Quad2d>[]", "Array_of_Vector_of_" + "Quad2d", typeof(Vector<Quad2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Quad2d>>", "List_of_Vector_of_" + "Quad2d", typeof(List<Vector<Quad2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Quad2d>", "Matrix_of_" + "Quad2d", typeof(Matrix<Quad2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Quad2d>[]", "Array_of_Matrix_of_" + "Quad2d", typeof(Matrix<Quad2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Quad2d>>", "List_of_Matrix_of_" + "Quad2d", typeof(List<Matrix<Quad2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Quad2d>", "Volume_of_" + "Quad2d", typeof(Volume<Quad2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Quad2d>[]", "Array_of_Volume_of_" + "Quad2d", typeof(Volume<Quad2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Quad2d>>", "List_of_Volume_of_" + "Quad2d", typeof(List<Volume<Quad2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Quad2d>", "Tensor_of_" + "Quad2d", typeof(Tensor<Quad2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Quad2d>[]", "Array_of_Tensor_of_" + "Quad2d", typeof(Tensor<Quad2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Quad2d>>", "List_of_Tensor_of_" + "Quad2d", typeof(List<Tensor<Quad2d>>), TypeInfo.Option.None),

                #endregion

                #region Quad3d

                new TypeInfo("Quad3d", typeof(Quad3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Quad3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Quad3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Quad3d>", "Vector_of_" + "Quad3d", typeof(Vector<Quad3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Quad3d>[]", "Array_of_Vector_of_" + "Quad3d", typeof(Vector<Quad3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Quad3d>>", "List_of_Vector_of_" + "Quad3d", typeof(List<Vector<Quad3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Quad3d>", "Matrix_of_" + "Quad3d", typeof(Matrix<Quad3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Quad3d>[]", "Array_of_Matrix_of_" + "Quad3d", typeof(Matrix<Quad3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Quad3d>>", "List_of_Matrix_of_" + "Quad3d", typeof(List<Matrix<Quad3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Quad3d>", "Volume_of_" + "Quad3d", typeof(Volume<Quad3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Quad3d>[]", "Array_of_Volume_of_" + "Quad3d", typeof(Volume<Quad3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Quad3d>>", "List_of_Volume_of_" + "Quad3d", typeof(List<Volume<Quad3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Quad3d>", "Tensor_of_" + "Quad3d", typeof(Tensor<Quad3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Quad3d>[]", "Array_of_Tensor_of_" + "Quad3d", typeof(Tensor<Quad3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Quad3d>>", "List_of_Tensor_of_" + "Quad3d", typeof(List<Tensor<Quad3d>>), TypeInfo.Option.None),

                #endregion

                #region Ray2d

                new TypeInfo("Ray2d", typeof(Ray2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Ray2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Ray2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Ray2d>", "Vector_of_" + "Ray2d", typeof(Vector<Ray2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Ray2d>[]", "Array_of_Vector_of_" + "Ray2d", typeof(Vector<Ray2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Ray2d>>", "List_of_Vector_of_" + "Ray2d", typeof(List<Vector<Ray2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Ray2d>", "Matrix_of_" + "Ray2d", typeof(Matrix<Ray2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Ray2d>[]", "Array_of_Matrix_of_" + "Ray2d", typeof(Matrix<Ray2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Ray2d>>", "List_of_Matrix_of_" + "Ray2d", typeof(List<Matrix<Ray2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Ray2d>", "Volume_of_" + "Ray2d", typeof(Volume<Ray2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Ray2d>[]", "Array_of_Volume_of_" + "Ray2d", typeof(Volume<Ray2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Ray2d>>", "List_of_Volume_of_" + "Ray2d", typeof(List<Volume<Ray2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Ray2d>", "Tensor_of_" + "Ray2d", typeof(Tensor<Ray2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Ray2d>[]", "Array_of_Tensor_of_" + "Ray2d", typeof(Tensor<Ray2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Ray2d>>", "List_of_Tensor_of_" + "Ray2d", typeof(List<Tensor<Ray2d>>), TypeInfo.Option.None),

                #endregion

                #region Ray3d

                new TypeInfo("Ray3d", typeof(Ray3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Ray3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Ray3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Ray3d>", "Vector_of_" + "Ray3d", typeof(Vector<Ray3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Ray3d>[]", "Array_of_Vector_of_" + "Ray3d", typeof(Vector<Ray3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Ray3d>>", "List_of_Vector_of_" + "Ray3d", typeof(List<Vector<Ray3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Ray3d>", "Matrix_of_" + "Ray3d", typeof(Matrix<Ray3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Ray3d>[]", "Array_of_Matrix_of_" + "Ray3d", typeof(Matrix<Ray3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Ray3d>>", "List_of_Matrix_of_" + "Ray3d", typeof(List<Matrix<Ray3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Ray3d>", "Volume_of_" + "Ray3d", typeof(Volume<Ray3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Ray3d>[]", "Array_of_Volume_of_" + "Ray3d", typeof(Volume<Ray3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Ray3d>>", "List_of_Volume_of_" + "Ray3d", typeof(List<Volume<Ray3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Ray3d>", "Tensor_of_" + "Ray3d", typeof(Tensor<Ray3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Ray3d>[]", "Array_of_Tensor_of_" + "Ray3d", typeof(Tensor<Ray3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Ray3d>>", "List_of_Tensor_of_" + "Ray3d", typeof(List<Tensor<Ray3d>>), TypeInfo.Option.None),

                #endregion

                #region Sphere3d

                new TypeInfo("Sphere3d", typeof(Sphere3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Sphere3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Sphere3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Sphere3d>", "Vector_of_" + "Sphere3d", typeof(Vector<Sphere3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Sphere3d>[]", "Array_of_Vector_of_" + "Sphere3d", typeof(Vector<Sphere3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Sphere3d>>", "List_of_Vector_of_" + "Sphere3d", typeof(List<Vector<Sphere3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Sphere3d>", "Matrix_of_" + "Sphere3d", typeof(Matrix<Sphere3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Sphere3d>[]", "Array_of_Matrix_of_" + "Sphere3d", typeof(Matrix<Sphere3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Sphere3d>>", "List_of_Matrix_of_" + "Sphere3d", typeof(List<Matrix<Sphere3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Sphere3d>", "Volume_of_" + "Sphere3d", typeof(Volume<Sphere3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Sphere3d>[]", "Array_of_Volume_of_" + "Sphere3d", typeof(Volume<Sphere3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Sphere3d>>", "List_of_Volume_of_" + "Sphere3d", typeof(List<Volume<Sphere3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Sphere3d>", "Tensor_of_" + "Sphere3d", typeof(Tensor<Sphere3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Sphere3d>[]", "Array_of_Tensor_of_" + "Sphere3d", typeof(Tensor<Sphere3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Sphere3d>>", "List_of_Tensor_of_" + "Sphere3d", typeof(List<Tensor<Sphere3d>>), TypeInfo.Option.None),

                #endregion

                #region Triangle2d

                new TypeInfo("Triangle2d", typeof(Triangle2d), TypeInfo.Option.None),
                new TypeInfo(typeof(Triangle2d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Triangle2d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Triangle2d>", "Vector_of_" + "Triangle2d", typeof(Vector<Triangle2d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Triangle2d>[]", "Array_of_Vector_of_" + "Triangle2d", typeof(Vector<Triangle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Triangle2d>>", "List_of_Vector_of_" + "Triangle2d", typeof(List<Vector<Triangle2d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Triangle2d>", "Matrix_of_" + "Triangle2d", typeof(Matrix<Triangle2d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Triangle2d>[]", "Array_of_Matrix_of_" + "Triangle2d", typeof(Matrix<Triangle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Triangle2d>>", "List_of_Matrix_of_" + "Triangle2d", typeof(List<Matrix<Triangle2d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Triangle2d>", "Volume_of_" + "Triangle2d", typeof(Volume<Triangle2d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Triangle2d>[]", "Array_of_Volume_of_" + "Triangle2d", typeof(Volume<Triangle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Triangle2d>>", "List_of_Volume_of_" + "Triangle2d", typeof(List<Volume<Triangle2d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Triangle2d>", "Tensor_of_" + "Triangle2d", typeof(Tensor<Triangle2d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Triangle2d>[]", "Array_of_Tensor_of_" + "Triangle2d", typeof(Tensor<Triangle2d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Triangle2d>>", "List_of_Tensor_of_" + "Triangle2d", typeof(List<Tensor<Triangle2d>>), TypeInfo.Option.None),

                #endregion

                #region Triangle3d

                new TypeInfo("Triangle3d", typeof(Triangle3d), TypeInfo.Option.None),
                new TypeInfo(typeof(Triangle3d[]), TypeInfo.Option.None),
                new TypeInfo(typeof(List<Triangle3d>), TypeInfo.Option.None),

                new TypeInfo("Vector<Triangle3d>", "Vector_of_" + "Triangle3d", typeof(Vector<Triangle3d>), TypeInfo.Option.None),
                new TypeInfo("Vector<Triangle3d>[]", "Array_of_Vector_of_" + "Triangle3d", typeof(Vector<Triangle3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Vector<Triangle3d>>", "List_of_Vector_of_" + "Triangle3d", typeof(List<Vector<Triangle3d>>), TypeInfo.Option.None),

                new TypeInfo("Matrix<Triangle3d>", "Matrix_of_" + "Triangle3d", typeof(Matrix<Triangle3d>), TypeInfo.Option.None),
                new TypeInfo("Matrix<Triangle3d>[]", "Array_of_Matrix_of_" + "Triangle3d", typeof(Matrix<Triangle3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Matrix<Triangle3d>>", "List_of_Matrix_of_" + "Triangle3d", typeof(List<Matrix<Triangle3d>>), TypeInfo.Option.None),

                new TypeInfo("Volume<Triangle3d>", "Volume_of_" + "Triangle3d", typeof(Volume<Triangle3d>), TypeInfo.Option.None),
                new TypeInfo("Volume<Triangle3d>[]", "Array_of_Volume_of_" + "Triangle3d", typeof(Volume<Triangle3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Volume<Triangle3d>>", "List_of_Volume_of_" + "Triangle3d", typeof(List<Volume<Triangle3d>>), TypeInfo.Option.None),

                new TypeInfo("Tensor<Triangle3d>", "Tensor_of_" + "Triangle3d", typeof(Tensor<Triangle3d>), TypeInfo.Option.None),
                new TypeInfo("Tensor<Triangle3d>[]", "Array_of_Tensor_of_" + "Triangle3d", typeof(Tensor<Triangle3d>[]), TypeInfo.Option.None),
                new TypeInfo("List<Tensor<Triangle3d>>", "List_of_Tensor_of_" + "Triangle3d", typeof(List<Tensor<Triangle3d>>), TypeInfo.Option.None),

                #endregion


                new TypeInfo("HashSet<string>", "HashSet_of_string", typeof(HashSet<string>), TypeInfo.Option.None),
            };
        }
    }
}
