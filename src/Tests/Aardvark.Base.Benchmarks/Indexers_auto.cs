using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aardvark.Base.Benchmarks
{
    // Testing variants for indexer methods for matrix and vector types.
    // There are three variants:
    // (*) Flow control with switch (original method)
    // (*) Flow control flow with if
    // (*) Unsafe with pointers
    // Using "switch" is problematic because it is never actually inlined (despite AggressiveInlining).
    // Flow control with "if" seems strictly better for this reason, however there are noticable differences
    // between different dimensions, i.e. the number of branches. In the case of element indexers, the "if" variant actually isn't always better.
    // The final method to be tested uses unsafe and direct access with pointers. As expect this method is much more efficient than the other
    // two in most cases. An exception to this are the row and column indexers for 4x4 matrices. Here, the flow control variants actually perform
    // slightly better.

    #region Indexers2

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class Indexers2
    {
        #region Vector

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Vector
        {
            #region Fields

            public readonly double X, Y;

            #endregion

            #region Constructor

            public Vector(double[] elems)
            {
                X = elems[0];
                Y = elems[1];
            }

            #endregion

            #region Indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int index)
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int index)
            {
                if (index == 0) return X;
                else if (index == 1) return Y;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int index)
            {
                fixed (double* ptr = &X) { return ptr[index]; }
            }

            #endregion
        }

        #endregion

        #region Matrix

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Matrix
        {
            #region Fields

            public readonly double M00, M01;
            public readonly double M10, M11;

            #endregion

            #region Rows

            public V2d R0
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V2d( M00,  M01); }
            }

            public V2d R1
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V2d( M10,  M11); }
            }

            #endregion

            #region Columns

            public V2d C0
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V2d( M00,  M10); }
            }

            public V2d C1
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V2d( M01,  M11); }
            }

            #endregion

            #region Constructor

            public Matrix(double[] elems)
            {
                M00 = elems[0]; M01 = elems[1]; 
                M10 = elems[2]; M11 = elems[3]; 
            }

            #endregion

            #region Element indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int row, int column)
            {
                switch (row)
                {
                    case 0:
                        switch (column)
                        {
                            case 0: return M00;
                            case 1: return M01;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 1:
                        switch (column)
                        {
                            case 0: return M10;
                            case 1: return M11;
                            default: throw new IndexOutOfRangeException();
                        }
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int row, int column)
            {
                if (row == 0)
                {
                    if (column == 0) return M00;
                    else if (column == 1) return M01;
                    else throw new IndexOutOfRangeException();
                }
                else if (row == 1)
                {
                    if (column == 0) return M10;
                    else if (column == 1) return M11;
                    else throw new IndexOutOfRangeException();
                }
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int row, int column)
            {
                fixed (double* ptr = &M00)
                {
                    return ptr[row * 2 + column];
                }
            }

            #endregion

            #region Row indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V2d RowSwitch(int index)
            {
                switch (index)
                {
                    case 0: return R0;
                    case 1: return R1;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V2d RowIf(int index)
            {
                if (index == 0) return R0;
                else if (index == 1) return R1;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe V2d RowUnsafe(int index)
            {
                fixed (double* ptr = &M00)
                {
                    return new V2d(ptr[index * 2], ptr[index * 2 + 1]);
                }
            }

            #endregion

            #region Column indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V2d ColSwitch(int index)
            {
                switch (index)
                {
                    case 0: return C0;
                    case 1: return C1;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V2d ColIf(int index)
            {
                if (index == 0) return C0;
                else if (index == 1) return C1;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe V2d ColUnsafe(int index)
            {
                fixed (double* ptr = &M00)
                {
                    return new V2d(ptr[index], ptr[index + 2]);
                }
            }

            #endregion
        }

        #endregion

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly Vector[] vectors = new Vector[count];
        readonly Matrix[] matrices = new Matrix[count];

        public Indexers2()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(2));
            vectors.SetByIndex(i => new Vector(rnd.CreateUniformDoubleArray(2)));
            matrices.SetByIndex(i => new Matrix(rnd.CreateUniformDoubleArray(4)));
        }

        #region Tests

        [Test]
        public void VectorElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var vec = vectors[i];

                var mif = vec.ElemIf(idx.X);
                var msw = vec.ElemSwitch(idx.X);
                var mus = vec.ElemUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ElemIf(idx.X, idx.Y);
                var msw = mat.ElemSwitch(idx.X, idx.Y);
                var mus = mat.ElemUnsafe(idx.X, idx.Y);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixRowIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.RowIf(idx.X);
                var msw = mat.RowSwitch(idx.X);
                var mus = mat.RowUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixColumnIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ColIf(idx.X);
                var msw = mat.ColSwitch(idx.X);
                var mus = mat.ColUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        #endregion

        #region Benchmarks for Vector Element Indexers

        [Benchmark]
        public double VectorElementIndexerSwitch()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemSwitch(idx);
            }

            return accum;
        }

        [Benchmark]
        public double VectorElementIndexerIf()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemIf(idx);
            }

            return accum;
        }

        [Benchmark]
        public double VectorElementIndexerUnsafe()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemUnsafe(idx);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Element Indexers

        [Benchmark]
        public double MatrixElementIndexerSwitch()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemSwitch(idx.X, idx.Y);
            }

            return accum;
        }

        [Benchmark]
        public double MatrixElementIndexerIf()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemIf(idx.X, idx.Y);
            }

            return accum;
        }

        [Benchmark]
        public double MatrixElementIndexerUnsafe()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemUnsafe(idx.X, idx.Y);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Row Indexers

        [Benchmark]
        public V2d MatrixRowIndexerSwitch()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowSwitch(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V2d MatrixRowIndexerIf()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowIf(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V2d MatrixRowIndexerUnsafe()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowUnsafe(indices[i].X);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Column Indexers

        [Benchmark]
        public V2d MatrixColumnIndexerSwitch()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColSwitch(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V2d MatrixColumnIndexerIf()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColIf(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V2d MatrixColumnIndexerUnsafe()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColUnsafe(indices[i].X);
            }

            return accum;
        }

        #endregion
    }

    #endregion

    #region Indexers3

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class Indexers3
    {
        #region Vector

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Vector
        {
            #region Fields

            public readonly double X, Y, Z;

            #endregion

            #region Constructor

            public Vector(double[] elems)
            {
                X = elems[0];
                Y = elems[1];
                Z = elems[2];
            }

            #endregion

            #region Indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int index)
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int index)
            {
                if (index == 0) return X;
                else if (index == 1) return Y;
                else if (index == 2) return Z;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int index)
            {
                fixed (double* ptr = &X) { return ptr[index]; }
            }

            #endregion
        }

        #endregion

        #region Matrix

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Matrix
        {
            #region Fields

            public readonly double M00, M01, M02;
            public readonly double M10, M11, M12;
            public readonly double M20, M21, M22;

            #endregion

            #region Rows

            public V3d R0
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V3d( M00,  M01,  M02); }
            }

            public V3d R1
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V3d( M10,  M11,  M12); }
            }

            public V3d R2
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V3d( M20,  M21,  M22); }
            }

            #endregion

            #region Columns

            public V3d C0
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V3d( M00,  M10,  M20); }
            }

            public V3d C1
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V3d( M01,  M11,  M21); }
            }

            public V3d C2
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V3d( M02,  M12,  M22); }
            }

            #endregion

            #region Constructor

            public Matrix(double[] elems)
            {
                M00 = elems[0]; M01 = elems[1]; M02 = elems[2]; 
                M10 = elems[3]; M11 = elems[4]; M12 = elems[5]; 
                M20 = elems[6]; M21 = elems[7]; M22 = elems[8]; 
            }

            #endregion

            #region Element indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int row, int column)
            {
                switch (row)
                {
                    case 0:
                        switch (column)
                        {
                            case 0: return M00;
                            case 1: return M01;
                            case 2: return M02;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 1:
                        switch (column)
                        {
                            case 0: return M10;
                            case 1: return M11;
                            case 2: return M12;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 2:
                        switch (column)
                        {
                            case 0: return M20;
                            case 1: return M21;
                            case 2: return M22;
                            default: throw new IndexOutOfRangeException();
                        }
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int row, int column)
            {
                if (row == 0)
                {
                    if (column == 0) return M00;
                    else if (column == 1) return M01;
                    else if (column == 2) return M02;
                    else throw new IndexOutOfRangeException();
                }
                else if (row == 1)
                {
                    if (column == 0) return M10;
                    else if (column == 1) return M11;
                    else if (column == 2) return M12;
                    else throw new IndexOutOfRangeException();
                }
                else if (row == 2)
                {
                    if (column == 0) return M20;
                    else if (column == 1) return M21;
                    else if (column == 2) return M22;
                    else throw new IndexOutOfRangeException();
                }
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int row, int column)
            {
                fixed (double* ptr = &M00)
                {
                    return ptr[row * 3 + column];
                }
            }

            #endregion

            #region Row indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V3d RowSwitch(int index)
            {
                switch (index)
                {
                    case 0: return R0;
                    case 1: return R1;
                    case 2: return R2;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V3d RowIf(int index)
            {
                if (index == 0) return R0;
                else if (index == 1) return R1;
                else if (index == 2) return R2;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe V3d RowUnsafe(int index)
            {
                fixed (double* ptr = &M00)
                {
                    return new V3d(ptr[index * 3], ptr[index * 3 + 1], ptr[index * 3 + 2]);
                }
            }

            #endregion

            #region Column indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V3d ColSwitch(int index)
            {
                switch (index)
                {
                    case 0: return C0;
                    case 1: return C1;
                    case 2: return C2;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V3d ColIf(int index)
            {
                if (index == 0) return C0;
                else if (index == 1) return C1;
                else if (index == 2) return C2;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe V3d ColUnsafe(int index)
            {
                fixed (double* ptr = &M00)
                {
                    return new V3d(ptr[index], ptr[index + 3], ptr[index + 6]);
                }
            }

            #endregion
        }

        #endregion

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly Vector[] vectors = new Vector[count];
        readonly Matrix[] matrices = new Matrix[count];

        public Indexers3()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(3));
            vectors.SetByIndex(i => new Vector(rnd.CreateUniformDoubleArray(3)));
            matrices.SetByIndex(i => new Matrix(rnd.CreateUniformDoubleArray(9)));
        }

        #region Tests

        [Test]
        public void VectorElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var vec = vectors[i];

                var mif = vec.ElemIf(idx.X);
                var msw = vec.ElemSwitch(idx.X);
                var mus = vec.ElemUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ElemIf(idx.X, idx.Y);
                var msw = mat.ElemSwitch(idx.X, idx.Y);
                var mus = mat.ElemUnsafe(idx.X, idx.Y);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixRowIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.RowIf(idx.X);
                var msw = mat.RowSwitch(idx.X);
                var mus = mat.RowUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixColumnIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ColIf(idx.X);
                var msw = mat.ColSwitch(idx.X);
                var mus = mat.ColUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        #endregion

        #region Benchmarks for Vector Element Indexers

        [Benchmark]
        public double VectorElementIndexerSwitch()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemSwitch(idx);
            }

            return accum;
        }

        [Benchmark]
        public double VectorElementIndexerIf()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemIf(idx);
            }

            return accum;
        }

        [Benchmark]
        public double VectorElementIndexerUnsafe()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemUnsafe(idx);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Element Indexers

        [Benchmark]
        public double MatrixElementIndexerSwitch()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemSwitch(idx.X, idx.Y);
            }

            return accum;
        }

        [Benchmark]
        public double MatrixElementIndexerIf()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemIf(idx.X, idx.Y);
            }

            return accum;
        }

        [Benchmark]
        public double MatrixElementIndexerUnsafe()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemUnsafe(idx.X, idx.Y);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Row Indexers

        [Benchmark]
        public V3d MatrixRowIndexerSwitch()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowSwitch(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V3d MatrixRowIndexerIf()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowIf(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V3d MatrixRowIndexerUnsafe()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowUnsafe(indices[i].X);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Column Indexers

        [Benchmark]
        public V3d MatrixColumnIndexerSwitch()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColSwitch(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V3d MatrixColumnIndexerIf()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColIf(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V3d MatrixColumnIndexerUnsafe()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColUnsafe(indices[i].X);
            }

            return accum;
        }

        #endregion
    }

    #endregion

    #region Indexers4

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class Indexers4
    {
        #region Vector

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Vector
        {
            #region Fields

            public readonly double X, Y, Z, W;

            #endregion

            #region Constructor

            public Vector(double[] elems)
            {
                X = elems[0];
                Y = elems[1];
                Z = elems[2];
                W = elems[3];
            }

            #endregion

            #region Indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int index)
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int index)
            {
                if (index == 0) return X;
                else if (index == 1) return Y;
                else if (index == 2) return Z;
                else if (index == 3) return W;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int index)
            {
                fixed (double* ptr = &X) { return ptr[index]; }
            }

            #endregion
        }

        #endregion

        #region Matrix

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Matrix
        {
            #region Fields

            public readonly double M00, M01, M02, M03;
            public readonly double M10, M11, M12, M13;
            public readonly double M20, M21, M22, M23;
            public readonly double M30, M31, M32, M33;

            #endregion

            #region Rows

            public V4d R0
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M00,  M01,  M02,  M03); }
            }

            public V4d R1
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M10,  M11,  M12,  M13); }
            }

            public V4d R2
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M20,  M21,  M22,  M23); }
            }

            public V4d R3
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M30,  M31,  M32,  M33); }
            }

            #endregion

            #region Columns

            public V4d C0
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M00,  M10,  M20,  M30); }
            }

            public V4d C1
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M01,  M11,  M21,  M31); }
            }

            public V4d C2
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M02,  M12,  M22,  M32); }
            }

            public V4d C3
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new V4d( M03,  M13,  M23,  M33); }
            }

            #endregion

            #region Constructor

            public Matrix(double[] elems)
            {
                M00 = elems[0]; M01 = elems[1]; M02 = elems[2]; M03 = elems[3]; 
                M10 = elems[4]; M11 = elems[5]; M12 = elems[6]; M13 = elems[7]; 
                M20 = elems[8]; M21 = elems[9]; M22 = elems[10]; M23 = elems[11]; 
                M30 = elems[12]; M31 = elems[13]; M32 = elems[14]; M33 = elems[15]; 
            }

            #endregion

            #region Element indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemSwitch(int row, int column)
            {
                switch (row)
                {
                    case 0:
                        switch (column)
                        {
                            case 0: return M00;
                            case 1: return M01;
                            case 2: return M02;
                            case 3: return M03;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 1:
                        switch (column)
                        {
                            case 0: return M10;
                            case 1: return M11;
                            case 2: return M12;
                            case 3: return M13;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 2:
                        switch (column)
                        {
                            case 0: return M20;
                            case 1: return M21;
                            case 2: return M22;
                            case 3: return M23;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 3:
                        switch (column)
                        {
                            case 0: return M30;
                            case 1: return M31;
                            case 2: return M32;
                            case 3: return M33;
                            default: throw new IndexOutOfRangeException();
                        }
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ElemIf(int row, int column)
            {
                if (row == 0)
                {
                    if (column == 0) return M00;
                    else if (column == 1) return M01;
                    else if (column == 2) return M02;
                    else if (column == 3) return M03;
                    else throw new IndexOutOfRangeException();
                }
                else if (row == 1)
                {
                    if (column == 0) return M10;
                    else if (column == 1) return M11;
                    else if (column == 2) return M12;
                    else if (column == 3) return M13;
                    else throw new IndexOutOfRangeException();
                }
                else if (row == 2)
                {
                    if (column == 0) return M20;
                    else if (column == 1) return M21;
                    else if (column == 2) return M22;
                    else if (column == 3) return M23;
                    else throw new IndexOutOfRangeException();
                }
                else if (row == 3)
                {
                    if (column == 0) return M30;
                    else if (column == 1) return M31;
                    else if (column == 2) return M32;
                    else if (column == 3) return M33;
                    else throw new IndexOutOfRangeException();
                }
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe double ElemUnsafe(int row, int column)
            {
                fixed (double* ptr = &M00)
                {
                    return ptr[row * 4 + column];
                }
            }

            #endregion

            #region Row indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V4d RowSwitch(int index)
            {
                switch (index)
                {
                    case 0: return R0;
                    case 1: return R1;
                    case 2: return R2;
                    case 3: return R3;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V4d RowIf(int index)
            {
                if (index == 0) return R0;
                else if (index == 1) return R1;
                else if (index == 2) return R2;
                else if (index == 3) return R3;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe V4d RowUnsafe(int index)
            {
                fixed (double* ptr = &M00)
                {
                    return new V4d(ptr[index * 4], ptr[index * 4 + 1], ptr[index * 4 + 2], ptr[index * 4 + 3]);
                }
            }

            #endregion

            #region Column indexers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V4d ColSwitch(int index)
            {
                switch (index)
                {
                    case 0: return C0;
                    case 1: return C1;
                    case 2: return C2;
                    case 3: return C3;
                    default: throw new IndexOutOfRangeException();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public V4d ColIf(int index)
            {
                if (index == 0) return C0;
                else if (index == 1) return C1;
                else if (index == 2) return C2;
                else if (index == 3) return C3;
                else throw new IndexOutOfRangeException();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public unsafe V4d ColUnsafe(int index)
            {
                fixed (double* ptr = &M00)
                {
                    return new V4d(ptr[index], ptr[index + 4], ptr[index + 8], ptr[index + 12]);
                }
            }

            #endregion
        }

        #endregion

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly Vector[] vectors = new Vector[count];
        readonly Matrix[] matrices = new Matrix[count];

        public Indexers4()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(4));
            vectors.SetByIndex(i => new Vector(rnd.CreateUniformDoubleArray(4)));
            matrices.SetByIndex(i => new Matrix(rnd.CreateUniformDoubleArray(16)));
        }

        #region Tests

        [Test]
        public void VectorElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var vec = vectors[i];

                var mif = vec.ElemIf(idx.X);
                var msw = vec.ElemSwitch(idx.X);
                var mus = vec.ElemUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixElementIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ElemIf(idx.X, idx.Y);
                var msw = mat.ElemSwitch(idx.X, idx.Y);
                var mus = mat.ElemUnsafe(idx.X, idx.Y);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixRowIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.RowIf(idx.X);
                var msw = mat.RowSwitch(idx.X);
                var mus = mat.RowUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        [Test]
        public void MatrixColumnIndexerCorrectness()
        {
            for (int i = 0; i < count / 10; i++)
            {
                var idx = indices[i];
                var mat = matrices[i];

                var mif = mat.ColIf(idx.X);
                var msw = mat.ColSwitch(idx.X);
                var mus = mat.ColUnsafe(idx.X);
                Assert.AreEqual(mif, msw);
                Assert.AreEqual(mif, mus);
            }
        }

        #endregion

        #region Benchmarks for Vector Element Indexers

        [Benchmark]
        public double VectorElementIndexerSwitch()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemSwitch(idx);
            }

            return accum;
        }

        [Benchmark]
        public double VectorElementIndexerIf()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemIf(idx);
            }

            return accum;
        }

        [Benchmark]
        public double VectorElementIndexerUnsafe()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i].X;
                accum += vectors[i].ElemUnsafe(idx);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Element Indexers

        [Benchmark]
        public double MatrixElementIndexerSwitch()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemSwitch(idx.X, idx.Y);
            }

            return accum;
        }

        [Benchmark]
        public double MatrixElementIndexerIf()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemIf(idx.X, idx.Y);
            }

            return accum;
        }

        [Benchmark]
        public double MatrixElementIndexerUnsafe()
        {
            double accum = 0;

            for (int i = 0; i < count; i++)
            {
                var idx = indices[i];
                accum += matrices[i].ElemUnsafe(idx.X, idx.Y);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Row Indexers

        [Benchmark]
        public V4d MatrixRowIndexerSwitch()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowSwitch(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V4d MatrixRowIndexerIf()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowIf(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V4d MatrixRowIndexerUnsafe()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].RowUnsafe(indices[i].X);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for Matrix Column Indexers

        [Benchmark]
        public V4d MatrixColumnIndexerSwitch()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColSwitch(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V4d MatrixColumnIndexerIf()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColIf(indices[i].X);
            }

            return accum;
        }


        [Benchmark]
        public V4d MatrixColumnIndexerUnsafe()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColUnsafe(indices[i].X);
            }

            return accum;
        }

        #endregion
    }

    #endregion

}