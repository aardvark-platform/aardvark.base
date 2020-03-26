using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixMinor
    {
        private static class Implementations
        {
            #region Version1

            public static M33d Version1(M44d m, int rowToDelete, int columnToDelete)
            {
                M33d result = new M33d();
                int checked_row = 0;
                for (int actual_row = 0; actual_row < 4; actual_row++)
                {
                    int checked_column = 0;

                    if (actual_row != rowToDelete)
                    {
                        for (int actual_column = 0; actual_column < 4; actual_column++)
                        {
                            if (actual_column != columnToDelete)
                            {
                                result[checked_row, checked_column] = m[actual_row, actual_column];
                                checked_column++;
                            }
                        }
                        checked_row++;
                    }
                }
                return result;
            }

            #endregion

            #region Version2

            public static M33d Version2(M44d m, int row, int column)
            {
                M33d rs = new M33d();
                var idx = 0;

                for (int k = 0; k < 16; k++)
                {
                    var i = k / 4;
                    var j = k % 4;

                    if (i != row && j != column)
                    {
                        rs[idx++] = m[k];
                    }
                }

                return rs;
            }

            #endregion

            #region Version3

            public static M33d Version3(M44d m, int row, int column)
            {
                M33d rs = new M33d();

                for (int k = 0; k < 9; k++)
                {
                    var i = k / 3;
                    var j = k % 3;
                    var ii = (i < row) ? i : i + 1;
                    var jj = (j < column) ? j : j + 1;

                    rs[k] = m[ii * 4 + jj];
                }

                return rs;
            }

            #endregion
        }

        const int count = 100000;
        readonly M44d[] A = new M44d[count];
        readonly int[] rows = new int[count];
        readonly int[] columns = new int[count];

        public MatrixMinor()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => new M44d(rnd.CreateUniformDoubleArray(16)));
            rows.SetByIndex(i => rnd.UniformInt(4));
            columns.SetByIndex(i => rnd.UniformInt(4));
        }

        private M33d Benchmark(Func<M44d, int, int, M33d> method)
        {
            M33d sum = M33d.Zero;

            for (int i = 0; i < count; i++)
            {
                sum += method(A[i], rows[i], columns[i]);
            }

            return sum;
        }

        [Benchmark]
        public M33d MinorVersion1()
            => Benchmark(Implementations.Version1);

        [Benchmark]
        public M33d MinorVersion2()
            => Benchmark(Implementations.Version2);

        [Benchmark]
        public M33d MinorVersion3()
            => Benchmark(Implementations.Version3);

        [Test]
        public void MatrixMinorTest()
        {
            for (int i = 0; i < count; i++)
            {
                var x = Implementations.Version1(A[i], rows[i], columns[i]);
                var y = Implementations.Version2(A[i], rows[i], columns[i]);
                var z = Implementations.Version3(A[i], rows[i], columns[i]);

                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.00001), "Minor({0}, {1}, {2}) \n{3} \n{4}", A[i], rows[i], columns[i], x, y);
                Assert.IsTrue(Fun.ApproximateEquals(x, z, 0.00001), "Minor({0}, {1}, {2}) \n{3} \n{4}", A[i], rows[i], columns[i], x, z);
            }
        }
    }
}