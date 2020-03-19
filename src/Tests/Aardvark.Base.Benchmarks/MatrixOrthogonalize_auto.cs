using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region M22f

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixOrthogonalizeM22f
    {
        private struct Implementation
        {
            public Func<M22f, M22f> Run;
            public float Epsilon;
        }

        private static class Implementations
        {
            #region Modified Gram-Schmidt

            private static M22f Mgs(M22f m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                return m;
            }

            private static M22f MgsNorm(M22f m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                return m;
            }

            public static Implementation GramSchmidt
            {
                get => new Implementation
                {
                    Run = Mgs,  
                    Epsilon = 1e-2f
                };
            }

            public static Implementation GramSchmidtNorm
            {
                get => new Implementation
                {
                    Run = MgsNorm,
                    Epsilon = 1e-2f
                };
            }

            #endregion

            #region Modified Gram-Schmidt (with Reorthogonalization)
            
            private static M22f Mgsr(M22f m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                return m;
            }

            private static M22f MgsrNorm(M22f m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                return m;
            }

            public static Implementation GramSchmidtReortho
            {
                get => new Implementation
                {
                    Run = Mgsr,
                    Epsilon = 1e-6f
                };
            }

            public static Implementation GramSchmidtReorthoNorm
            {
                get => new Implementation
                {
                    Run = MgsrNorm,
                    Epsilon = 1e-6f
                };
            }

            #endregion

            public static Dictionary<string, Implementation> Orthogonalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidt },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReortho }
                };
            }

            public static Dictionary<string, Implementation> Orthonormalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidtNorm },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReorthoNorm }
                };
            }

            public static Dictionary<string, Implementation> All
            {
                get => Orthogonalize.Combine(Orthogonalize);
            }
        }

        const int count = 100000;
        readonly M22f[] Matrices = new M22f[count];

        public MatrixOrthogonalizeM22f()
        {
            var rnd = new RandomSystem(1);
            Matrices.SetByIndex(i => new M22f(rnd.CreateUniformFloatArray(16)) * 10 );
        }

        // This is probably not optimal...
        private double ErrorMeasure(M22f matrix)
        {
            var m = (M22d)matrix;
            double error = 0;

            error += Vec.Dot(m.C0, m.C1).Square();

            return error;
        }

        private bool AreOrtho(M22f m, float epsilon)
        {
            return ErrorMeasure(m) < epsilon;
        }

        private double[] ComputeAbsoluteError(Func<M22f, M22f> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var m = f(Matrices[i]);
                error[i] = ErrorMeasure(m);
            }

            return error;
        }

        private void BenchmarkNumericalStability(string name, Dictionary<string, Implementation> methods)
        {
            Console.WriteLine("Benchmarking numerical stability for {0} variants", name);
            Console.WriteLine();

            foreach (var m in methods)
            {
                var errors = ComputeAbsoluteError(m.Value.Run);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        public void BenchmarkNumericalStability()
        {
            BenchmarkNumericalStability("M22f.Orthogonalized()", Implementations.Orthogonalize);
            BenchmarkNumericalStability("M22f.Orthonormalized()", Implementations.Orthonormalize);
        }

        [Test]
        public void OrthogonalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthogonalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        AreOrtho(y, f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Test]
        public void OrthonormalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthonormalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        y.IsOrthonormal(f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Benchmark]
        public M22f GramSchmidt()
        {
            M22f accum = M22f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidt.Run(Matrices[i]);
                accum += Implementations.GramSchmidtNorm.Run(Matrices[i]);
            }

            return accum;
        }

        [Benchmark]
        public M22f GramSchmidtWithReortho()
        {
            M22f accum = M22f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidtReortho.Run(Matrices[i]);
                accum += Implementations.GramSchmidtReorthoNorm.Run(Matrices[i]);
            }

            return accum;
        }
    }

    #endregion

    #region M33f

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixOrthogonalizeM33f
    {
        private struct Implementation
        {
            public Func<M33f, M33f> Run;
            public float Epsilon;
        }

        private static class Implementations
        {
            #region Modified Gram-Schmidt

            private static M33f Mgs(M33f m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                return m;
            }

            private static M33f MgsNorm(M33f m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                return m;
            }

            public static Implementation GramSchmidt
            {
                get => new Implementation
                {
                    Run = Mgs,  
                    Epsilon = 1e-2f
                };
            }

            public static Implementation GramSchmidtNorm
            {
                get => new Implementation
                {
                    Run = MgsNorm,
                    Epsilon = 1e-2f
                };
            }

            #endregion

            #region Modified Gram-Schmidt (with Reorthogonalization)
            
            private static M33f Mgsr(M33f m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                return m;
            }

            private static M33f MgsrNorm(M33f m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                return m;
            }

            public static Implementation GramSchmidtReortho
            {
                get => new Implementation
                {
                    Run = Mgsr,
                    Epsilon = 1e-6f
                };
            }

            public static Implementation GramSchmidtReorthoNorm
            {
                get => new Implementation
                {
                    Run = MgsrNorm,
                    Epsilon = 1e-6f
                };
            }

            #endregion

            public static Dictionary<string, Implementation> Orthogonalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidt },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReortho }
                };
            }

            public static Dictionary<string, Implementation> Orthonormalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidtNorm },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReorthoNorm }
                };
            }

            public static Dictionary<string, Implementation> All
            {
                get => Orthogonalize.Combine(Orthogonalize);
            }
        }

        const int count = 100000;
        readonly M33f[] Matrices = new M33f[count];

        public MatrixOrthogonalizeM33f()
        {
            var rnd = new RandomSystem(1);
            Matrices.SetByIndex(i => new M33f(rnd.CreateUniformFloatArray(16)) * 10 );
        }

        // This is probably not optimal...
        private double ErrorMeasure(M33f matrix)
        {
            var m = (M33d)matrix;
            double error = 0;

            error += Vec.Dot(m.C0, m.C1).Square();
            error += Vec.Dot(m.C0, m.C2).Square();
            error += Vec.Dot(m.C1, m.C2).Square();

            return error;
        }

        private bool AreOrtho(M33f m, float epsilon)
        {
            return ErrorMeasure(m) < epsilon;
        }

        private double[] ComputeAbsoluteError(Func<M33f, M33f> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var m = f(Matrices[i]);
                error[i] = ErrorMeasure(m);
            }

            return error;
        }

        private void BenchmarkNumericalStability(string name, Dictionary<string, Implementation> methods)
        {
            Console.WriteLine("Benchmarking numerical stability for {0} variants", name);
            Console.WriteLine();

            foreach (var m in methods)
            {
                var errors = ComputeAbsoluteError(m.Value.Run);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        public void BenchmarkNumericalStability()
        {
            BenchmarkNumericalStability("M33f.Orthogonalized()", Implementations.Orthogonalize);
            BenchmarkNumericalStability("M33f.Orthonormalized()", Implementations.Orthonormalize);
        }

        [Test]
        public void OrthogonalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthogonalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        AreOrtho(y, f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Test]
        public void OrthonormalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthonormalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        y.IsOrthonormal(f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Benchmark]
        public M33f GramSchmidt()
        {
            M33f accum = M33f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidt.Run(Matrices[i]);
                accum += Implementations.GramSchmidtNorm.Run(Matrices[i]);
            }

            return accum;
        }

        [Benchmark]
        public M33f GramSchmidtWithReortho()
        {
            M33f accum = M33f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidtReortho.Run(Matrices[i]);
                accum += Implementations.GramSchmidtReorthoNorm.Run(Matrices[i]);
            }

            return accum;
        }
    }

    #endregion

    #region M44f

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixOrthogonalizeM44f
    {
        private struct Implementation
        {
            public Func<M44f, M44f> Run;
            public float Epsilon;
        }

        private static class Implementations
        {
            #region Modified Gram-Schmidt

            private static M44f Mgs(M44f m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;

                return m;
            }

            private static M44f MgsNorm(M44f m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 = m.C3.Normalized;

                return m;
            }

            public static Implementation GramSchmidt
            {
                get => new Implementation
                {
                    Run = Mgs,  
                    Epsilon = 1e-2f
                };
            }

            public static Implementation GramSchmidtNorm
            {
                get => new Implementation
                {
                    Run = MgsNorm,
                    Epsilon = 1e-2f
                };
            }

            #endregion

            #region Modified Gram-Schmidt (with Reorthogonalization)
            
            private static M44f Mgsr(M44f m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;

                return m;
            }

            private static M44f MgsrNorm(M44f m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 = m.C3.Normalized;

                return m;
            }

            public static Implementation GramSchmidtReortho
            {
                get => new Implementation
                {
                    Run = Mgsr,
                    Epsilon = 1e-6f
                };
            }

            public static Implementation GramSchmidtReorthoNorm
            {
                get => new Implementation
                {
                    Run = MgsrNorm,
                    Epsilon = 1e-6f
                };
            }

            #endregion

            public static Dictionary<string, Implementation> Orthogonalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidt },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReortho }
                };
            }

            public static Dictionary<string, Implementation> Orthonormalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidtNorm },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReorthoNorm }
                };
            }

            public static Dictionary<string, Implementation> All
            {
                get => Orthogonalize.Combine(Orthogonalize);
            }
        }

        const int count = 100000;
        readonly M44f[] Matrices = new M44f[count];

        public MatrixOrthogonalizeM44f()
        {
            var rnd = new RandomSystem(1);
            Matrices.SetByIndex(i => new M44f(rnd.CreateUniformFloatArray(16)) * 10 );
        }

        // This is probably not optimal...
        private double ErrorMeasure(M44f matrix)
        {
            var m = (M44d)matrix;
            double error = 0;

            error += Vec.Dot(m.C0, m.C1).Square();
            error += Vec.Dot(m.C0, m.C2).Square();
            error += Vec.Dot(m.C0, m.C3).Square();
            error += Vec.Dot(m.C1, m.C2).Square();
            error += Vec.Dot(m.C1, m.C3).Square();
            error += Vec.Dot(m.C2, m.C3).Square();

            return error;
        }

        private bool AreOrtho(M44f m, float epsilon)
        {
            return ErrorMeasure(m) < epsilon;
        }

        private double[] ComputeAbsoluteError(Func<M44f, M44f> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var m = f(Matrices[i]);
                error[i] = ErrorMeasure(m);
            }

            return error;
        }

        private void BenchmarkNumericalStability(string name, Dictionary<string, Implementation> methods)
        {
            Console.WriteLine("Benchmarking numerical stability for {0} variants", name);
            Console.WriteLine();

            foreach (var m in methods)
            {
                var errors = ComputeAbsoluteError(m.Value.Run);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        public void BenchmarkNumericalStability()
        {
            BenchmarkNumericalStability("M44f.Orthogonalized()", Implementations.Orthogonalize);
            BenchmarkNumericalStability("M44f.Orthonormalized()", Implementations.Orthonormalize);
        }

        [Test]
        public void OrthogonalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthogonalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        AreOrtho(y, f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Test]
        public void OrthonormalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthonormalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        y.IsOrthonormal(f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Benchmark]
        public M44f GramSchmidt()
        {
            M44f accum = M44f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidt.Run(Matrices[i]);
                accum += Implementations.GramSchmidtNorm.Run(Matrices[i]);
            }

            return accum;
        }

        [Benchmark]
        public M44f GramSchmidtWithReortho()
        {
            M44f accum = M44f.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidtReortho.Run(Matrices[i]);
                accum += Implementations.GramSchmidtReorthoNorm.Run(Matrices[i]);
            }

            return accum;
        }
    }

    #endregion

    #region M22d

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixOrthogonalizeM22d
    {
        private struct Implementation
        {
            public Func<M22d, M22d> Run;
            public double Epsilon;
        }

        private static class Implementations
        {
            #region Modified Gram-Schmidt

            private static M22d Mgs(M22d m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                return m;
            }

            private static M22d MgsNorm(M22d m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                return m;
            }

            public static Implementation GramSchmidt
            {
                get => new Implementation
                {
                    Run = Mgs,  
                    Epsilon = 1e-10
                };
            }

            public static Implementation GramSchmidtNorm
            {
                get => new Implementation
                {
                    Run = MgsNorm,
                    Epsilon = 1e-10
                };
            }

            #endregion

            #region Modified Gram-Schmidt (with Reorthogonalization)
            
            private static M22d Mgsr(M22d m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                return m;
            }

            private static M22d MgsrNorm(M22d m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                return m;
            }

            public static Implementation GramSchmidtReortho
            {
                get => new Implementation
                {
                    Run = Mgsr,
                    Epsilon = 1e-14
                };
            }

            public static Implementation GramSchmidtReorthoNorm
            {
                get => new Implementation
                {
                    Run = MgsrNorm,
                    Epsilon = 1e-14
                };
            }

            #endregion

            public static Dictionary<string, Implementation> Orthogonalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidt },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReortho }
                };
            }

            public static Dictionary<string, Implementation> Orthonormalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidtNorm },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReorthoNorm }
                };
            }

            public static Dictionary<string, Implementation> All
            {
                get => Orthogonalize.Combine(Orthogonalize);
            }
        }

        const int count = 100000;
        readonly M22d[] Matrices = new M22d[count];

        public MatrixOrthogonalizeM22d()
        {
            var rnd = new RandomSystem(1);
            Matrices.SetByIndex(i => new M22d(rnd.CreateUniformDoubleArray(16)) * 10 );
        }

        // This is probably not optimal...
        private double ErrorMeasure(M22d matrix)
        {
            var m = (M22d)matrix;
            double error = 0;

            error += Vec.Dot(m.C0, m.C1).Square();

            return error;
        }

        private bool AreOrtho(M22d m, double epsilon)
        {
            return ErrorMeasure(m) < epsilon;
        }

        private double[] ComputeAbsoluteError(Func<M22d, M22d> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var m = f(Matrices[i]);
                error[i] = ErrorMeasure(m);
            }

            return error;
        }

        private void BenchmarkNumericalStability(string name, Dictionary<string, Implementation> methods)
        {
            Console.WriteLine("Benchmarking numerical stability for {0} variants", name);
            Console.WriteLine();

            foreach (var m in methods)
            {
                var errors = ComputeAbsoluteError(m.Value.Run);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        public void BenchmarkNumericalStability()
        {
            BenchmarkNumericalStability("M22d.Orthogonalized()", Implementations.Orthogonalize);
            BenchmarkNumericalStability("M22d.Orthonormalized()", Implementations.Orthonormalize);
        }

        [Test]
        public void OrthogonalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthogonalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        AreOrtho(y, f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Test]
        public void OrthonormalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthonormalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        y.IsOrthonormal(f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Benchmark]
        public M22d GramSchmidt()
        {
            M22d accum = M22d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidt.Run(Matrices[i]);
                accum += Implementations.GramSchmidtNorm.Run(Matrices[i]);
            }

            return accum;
        }

        [Benchmark]
        public M22d GramSchmidtWithReortho()
        {
            M22d accum = M22d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidtReortho.Run(Matrices[i]);
                accum += Implementations.GramSchmidtReorthoNorm.Run(Matrices[i]);
            }

            return accum;
        }
    }

    #endregion

    #region M33d

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixOrthogonalizeM33d
    {
        private struct Implementation
        {
            public Func<M33d, M33d> Run;
            public double Epsilon;
        }

        private static class Implementations
        {
            #region Modified Gram-Schmidt

            private static M33d Mgs(M33d m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                return m;
            }

            private static M33d MgsNorm(M33d m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                return m;
            }

            public static Implementation GramSchmidt
            {
                get => new Implementation
                {
                    Run = Mgs,  
                    Epsilon = 1e-10
                };
            }

            public static Implementation GramSchmidtNorm
            {
                get => new Implementation
                {
                    Run = MgsNorm,
                    Epsilon = 1e-10
                };
            }

            #endregion

            #region Modified Gram-Schmidt (with Reorthogonalization)
            
            private static M33d Mgsr(M33d m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                return m;
            }

            private static M33d MgsrNorm(M33d m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                return m;
            }

            public static Implementation GramSchmidtReortho
            {
                get => new Implementation
                {
                    Run = Mgsr,
                    Epsilon = 1e-14
                };
            }

            public static Implementation GramSchmidtReorthoNorm
            {
                get => new Implementation
                {
                    Run = MgsrNorm,
                    Epsilon = 1e-14
                };
            }

            #endregion

            public static Dictionary<string, Implementation> Orthogonalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidt },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReortho }
                };
            }

            public static Dictionary<string, Implementation> Orthonormalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidtNorm },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReorthoNorm }
                };
            }

            public static Dictionary<string, Implementation> All
            {
                get => Orthogonalize.Combine(Orthogonalize);
            }
        }

        const int count = 100000;
        readonly M33d[] Matrices = new M33d[count];

        public MatrixOrthogonalizeM33d()
        {
            var rnd = new RandomSystem(1);
            Matrices.SetByIndex(i => new M33d(rnd.CreateUniformDoubleArray(16)) * 10 );
        }

        // This is probably not optimal...
        private double ErrorMeasure(M33d matrix)
        {
            var m = (M33d)matrix;
            double error = 0;

            error += Vec.Dot(m.C0, m.C1).Square();
            error += Vec.Dot(m.C0, m.C2).Square();
            error += Vec.Dot(m.C1, m.C2).Square();

            return error;
        }

        private bool AreOrtho(M33d m, double epsilon)
        {
            return ErrorMeasure(m) < epsilon;
        }

        private double[] ComputeAbsoluteError(Func<M33d, M33d> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var m = f(Matrices[i]);
                error[i] = ErrorMeasure(m);
            }

            return error;
        }

        private void BenchmarkNumericalStability(string name, Dictionary<string, Implementation> methods)
        {
            Console.WriteLine("Benchmarking numerical stability for {0} variants", name);
            Console.WriteLine();

            foreach (var m in methods)
            {
                var errors = ComputeAbsoluteError(m.Value.Run);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        public void BenchmarkNumericalStability()
        {
            BenchmarkNumericalStability("M33d.Orthogonalized()", Implementations.Orthogonalize);
            BenchmarkNumericalStability("M33d.Orthonormalized()", Implementations.Orthonormalize);
        }

        [Test]
        public void OrthogonalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthogonalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        AreOrtho(y, f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Test]
        public void OrthonormalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthonormalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        y.IsOrthonormal(f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Benchmark]
        public M33d GramSchmidt()
        {
            M33d accum = M33d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidt.Run(Matrices[i]);
                accum += Implementations.GramSchmidtNorm.Run(Matrices[i]);
            }

            return accum;
        }

        [Benchmark]
        public M33d GramSchmidtWithReortho()
        {
            M33d accum = M33d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidtReortho.Run(Matrices[i]);
                accum += Implementations.GramSchmidtReorthoNorm.Run(Matrices[i]);
            }

            return accum;
        }
    }

    #endregion

    #region M44d

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixOrthogonalizeM44d
    {
        private struct Implementation
        {
            public Func<M44d, M44d> Run;
            public double Epsilon;
        }

        private static class Implementations
        {
            #region Modified Gram-Schmidt

            private static M44d Mgs(M44d m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;

                return m;
            }

            private static M44d MgsNorm(M44d m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 = m.C3.Normalized;

                return m;
            }

            public static Implementation GramSchmidt
            {
                get => new Implementation
                {
                    Run = Mgs,  
                    Epsilon = 1e-10
                };
            }

            public static Implementation GramSchmidtNorm
            {
                get => new Implementation
                {
                    Run = MgsNorm,
                    Epsilon = 1e-10
                };
            }

            #endregion

            #region Modified Gram-Schmidt (with Reorthogonalization)
            
            private static M44d Mgsr(M44d m)
            {
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;

                return m;
            }

            private static M44d MgsrNorm(M44d m)
            {
                m.C0 = m.C0.Normalized;

                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 -= (Vec.Dot(m.C0, m.C1) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C1 = m.C1.Normalized;

                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 -= (Vec.Dot(m.C0, m.C2) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C2 -= (Vec.Dot(m.C1, m.C2) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C2 = m.C2.Normalized;

                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 -= (Vec.Dot(m.C0, m.C3) / Vec.Dot(m.C0, m.C0)) * m.C0;
                m.C3 -= (Vec.Dot(m.C1, m.C3) / Vec.Dot(m.C1, m.C1)) * m.C1;
                m.C3 -= (Vec.Dot(m.C2, m.C3) / Vec.Dot(m.C2, m.C2)) * m.C2;
                m.C3 = m.C3.Normalized;

                return m;
            }

            public static Implementation GramSchmidtReortho
            {
                get => new Implementation
                {
                    Run = Mgsr,
                    Epsilon = 1e-14
                };
            }

            public static Implementation GramSchmidtReorthoNorm
            {
                get => new Implementation
                {
                    Run = MgsrNorm,
                    Epsilon = 1e-14
                };
            }

            #endregion

            public static Dictionary<string, Implementation> Orthogonalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidt },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReortho }
                };
            }

            public static Dictionary<string, Implementation> Orthonormalize
            {
                get => new Dictionary<string, Implementation>()
                {
                    { "Gram-Schmidt", GramSchmidtNorm },
                    { "Gram-Schmidt with Reorthogonalization", GramSchmidtReorthoNorm }
                };
            }

            public static Dictionary<string, Implementation> All
            {
                get => Orthogonalize.Combine(Orthogonalize);
            }
        }

        const int count = 100000;
        readonly M44d[] Matrices = new M44d[count];

        public MatrixOrthogonalizeM44d()
        {
            var rnd = new RandomSystem(1);
            Matrices.SetByIndex(i => new M44d(rnd.CreateUniformDoubleArray(16)) * 10 );
        }

        // This is probably not optimal...
        private double ErrorMeasure(M44d matrix)
        {
            var m = (M44d)matrix;
            double error = 0;

            error += Vec.Dot(m.C0, m.C1).Square();
            error += Vec.Dot(m.C0, m.C2).Square();
            error += Vec.Dot(m.C0, m.C3).Square();
            error += Vec.Dot(m.C1, m.C2).Square();
            error += Vec.Dot(m.C1, m.C3).Square();
            error += Vec.Dot(m.C2, m.C3).Square();

            return error;
        }

        private bool AreOrtho(M44d m, double epsilon)
        {
            return ErrorMeasure(m) < epsilon;
        }

        private double[] ComputeAbsoluteError(Func<M44d, M44d> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var m = f(Matrices[i]);
                error[i] = ErrorMeasure(m);
            }

            return error;
        }

        private void BenchmarkNumericalStability(string name, Dictionary<string, Implementation> methods)
        {
            Console.WriteLine("Benchmarking numerical stability for {0} variants", name);
            Console.WriteLine();

            foreach (var m in methods)
            {
                var errors = ComputeAbsoluteError(m.Value.Run);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Absolute error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        public void BenchmarkNumericalStability()
        {
            BenchmarkNumericalStability("M44d.Orthogonalized()", Implementations.Orthogonalize);
            BenchmarkNumericalStability("M44d.Orthonormalized()", Implementations.Orthonormalize);
        }

        [Test]
        public void OrthogonalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthogonalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        AreOrtho(y, f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Test]
        public void OrthonormalizeCorrectnessTest()
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var f in Implementations.Orthonormalize)
                {
                    var x = Matrices[i];
                    var y = f.Value.Run(x);

                    Assert.IsTrue(
                        y.IsOrthonormal(f.Value.Epsilon),
                        string.Format("{0} failed.\n" +
                        "Input = {1}\n" +
                        "Output = {2}",
                        f.Key, x, y)
                    );
                }
            }
        }

        [Benchmark]
        public M44d GramSchmidt()
        {
            M44d accum = M44d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidt.Run(Matrices[i]);
                accum += Implementations.GramSchmidtNorm.Run(Matrices[i]);
            }

            return accum;
        }

        [Benchmark]
        public M44d GramSchmidtWithReortho()
        {
            M44d accum = M44d.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidtReortho.Run(Matrices[i]);
                accum += Implementations.GramSchmidtReorthoNorm.Run(Matrices[i]);
            }

            return accum;
        }
    }

    #endregion

}