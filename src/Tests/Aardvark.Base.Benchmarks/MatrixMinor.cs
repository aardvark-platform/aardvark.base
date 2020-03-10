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
        const int count = 10000000;
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

        private static M33d OldMinorImpl(M44d m, int rowToDelete, int columnToDelete)
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

        [Benchmark]
        public M33d MinorOld()
        {
            M33d sum = M33d.Zero;

            for (int i = 0; i < count; i++)
            {
                sum += OldMinorImpl(A[i], rows[i], columns[i]);
            }

            return sum;
        }


        [Benchmark]
        public M33d MinorNew()
        {
            M33d sum = M33d.Zero;

            for (int i = 0; i < count; i++)
            {
                sum += Mat.Minor(A[i], rows[i], columns[i]);
            }

            return sum;
        }

        [Test]
        public void MatrixMinorTest()
        {
            for (int i = 0; i < count; i++)
            {
                var x = Mat.Minor(A[i], rows[i], columns[i]);
                var y = Mat.Minor(A[i], rows[i], columns[i]);

                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.00001), "{0} != {1}", x, y);
            }
        }
    }
}