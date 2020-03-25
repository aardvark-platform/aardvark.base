using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks
{
    // Testing two variants for the Column(int) and Row(int) and element indexer methods for matrix types.
    // The old implementation used a "switch" statement, whereas here we want to prove that using "if" is more efficient.
    // Using "switch" seems to be inferior for Column and Row because it is never actually inlined. However, there are noticable differences
    // between different dimensions, i.e. the number of branches.
    // In the case of element indexers, the "if" variant actually isn't always better. Note that here we may not want to put
    // AggressiveInlining because for 4x4 matrices there would be 16 branches in total.
    
    #region SwitchIf2

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class SwitchIf2
    {
        private struct M2x2
        {
            public double M00, M01;
            public double M10, M11;

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

            public M2x2(double[] elems)
            {
                M00 = elems[0]; M01 = elems[1]; 
                M10 = elems[2]; M11 = elems[3]; 
            }

            #region Elem

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

            #endregion

            #region Col

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

            #endregion
        }

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly M2x2[] matrices = new M2x2[count];

        public SwitchIf2()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(2));
            matrices.SetByIndex(i => new M2x2(rnd.CreateUniformDoubleArray(4)));
        }

        #region Benchmarks for Switch

        [Benchmark]
        public double ElemSwitch()
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
        public V2d ColSwitch()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColSwitch(indices[i].X);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for If

        [Benchmark]
        public double ElemIf()
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
        public V2d ColIf()
        {
            V2d accum = V2d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColIf(indices[i].X);
            }

            return accum;
        }

        #endregion

    }

    #endregion

    #region SwitchIf3

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class SwitchIf3
    {
        private struct M3x3
        {
            public double M00, M01, M02;
            public double M10, M11, M12;
            public double M20, M21, M22;

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

            public M3x3(double[] elems)
            {
                M00 = elems[0]; M01 = elems[1]; M02 = elems[2]; 
                M10 = elems[3]; M11 = elems[4]; M12 = elems[5]; 
                M20 = elems[6]; M21 = elems[7]; M22 = elems[8]; 
            }

            #region Elem

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

            #endregion

            #region Col

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

            #endregion
        }

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly M3x3[] matrices = new M3x3[count];

        public SwitchIf3()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(3));
            matrices.SetByIndex(i => new M3x3(rnd.CreateUniformDoubleArray(9)));
        }

        #region Benchmarks for Switch

        [Benchmark]
        public double ElemSwitch()
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
        public V3d ColSwitch()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColSwitch(indices[i].X);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for If

        [Benchmark]
        public double ElemIf()
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
        public V3d ColIf()
        {
            V3d accum = V3d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColIf(indices[i].X);
            }

            return accum;
        }

        #endregion

    }

    #endregion

    #region SwitchIf4

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
         //<DebugType>pdbonly</DebugType>
         //<DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class SwitchIf4
    {
        private struct M4x4
        {
            public double M00, M01, M02, M03;
            public double M10, M11, M12, M13;
            public double M20, M21, M22, M23;
            public double M30, M31, M32, M33;

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

            public M4x4(double[] elems)
            {
                M00 = elems[0]; M01 = elems[1]; M02 = elems[2]; M03 = elems[3]; 
                M10 = elems[4]; M11 = elems[5]; M12 = elems[6]; M13 = elems[7]; 
                M20 = elems[8]; M21 = elems[9]; M22 = elems[10]; M23 = elems[11]; 
                M30 = elems[12]; M31 = elems[13]; M32 = elems[14]; M33 = elems[15]; 
            }

            #region Elem

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

            #endregion

            #region Col

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

            #endregion
        }

        const int count = 1000000;
        readonly V2i[] indices = new V2i[count];
        readonly M4x4[] matrices = new M4x4[count];

        public SwitchIf4()
        {
            var rnd = new RandomSystem(1);
            indices.SetByIndex(i => rnd.UniformV2i(4));
            matrices.SetByIndex(i => new M4x4(rnd.CreateUniformDoubleArray(16)));
        }

        #region Benchmarks for Switch

        [Benchmark]
        public double ElemSwitch()
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
        public V4d ColSwitch()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColSwitch(indices[i].X);
            }

            return accum;
        }

        #endregion

        #region Benchmarks for If

        [Benchmark]
        public double ElemIf()
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
        public V4d ColIf()
        {
            V4d accum = V4d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += matrices[i].ColIf(indices[i].X);
            }

            return accum;
        }

        #endregion

    }

    #endregion

}