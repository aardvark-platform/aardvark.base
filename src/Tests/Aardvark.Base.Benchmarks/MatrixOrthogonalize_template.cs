using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# Action comma = () => Out(", ");
    //# Action add = () => Out(" + ");
    //# Action xor = () => Out(" ^ ");
    //# Action andLit = () => Out(" and ");
    //# Action andand = () => Out(" && ");
    //# Action oror = () => Out(" || ");
    //# Action addqcomma = () => Out(" + \",\" ");
    //# Action addbetweenM = () => Out(" + betweenM ");
    //# var fields = new[] {"X", "Y", "Z", "W"};
    //# foreach (var ft in Meta.RealTypes) {
    //# for (int n = 2; n <= 4; n++) {
    //#     var isDouble = ft == Meta.DoubleType;
    //#     var ftype = ft.Name;
    //#     var Ftype = ft.Caps;
    //#     var fc = ft.Char;
    //#     var mnnt = "M" + n + n + fc;
    //#     var mnnd = "M" + n + n + "d";
    //#     var epsgs = isDouble ? "1e-10" : "1e-2f";
    //#     var epsgsr = isDouble ? "1e-14" : "1e-6f";
    #region __mnnt__

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class MatrixOrthogonalize__mnnt__
    {
        private struct Implementation
        {
            public Func<__mnnt__, __mnnt__> Run;
            public __ftype__ Epsilon;
        }

        private static class Implementations
        {
            #region Modified Gram-Schmidt

            private static __mnnt__ Mgs(__mnnt__ m)
            {
                //# for (int j = 1; j < n; j++) {
                //# for (int k = 0; k < j; k++) { 
                m.C__j__ -= (Vec.Dot(m.C__k__, m.C__j__) / Vec.Dot(m.C__k__, m.C__k__)) * m.C__k__;
                //# }

                //# }
                return m;
            }

            private static __mnnt__ MgsNorm(__mnnt__ m)
            {
                m.C0 = m.C0.Normalized;

                //# for (int j = 1; j < n; j++) {
                //# for (int k = 0; k < j; k++) { 
                m.C__j__ -= (Vec.Dot(m.C__k__, m.C__j__) / Vec.Dot(m.C__k__, m.C__k__)) * m.C__k__;
                //# }
                m.C__j__ = m.C__j__.Normalized;

                //# }
                return m;
            }

            public static Implementation GramSchmidt
            {
                get => new Implementation
                {
                    Run = Mgs,  
                    Epsilon = __epsgs__
                };
            }

            public static Implementation GramSchmidtNorm
            {
                get => new Implementation
                {
                    Run = MgsNorm,
                    Epsilon = __epsgs__
                };
            }

            #endregion

            #region Modified Gram-Schmidt (with Reorthogonalization)
            
            private static __mnnt__ Mgsr(__mnnt__ m)
            {
                //# for (int j = 1; j < n; j++) {
                //# for (int i = 0; i < 2; i++) {
                //# for (int k = 0; k < j; k++) { 
                m.C__j__ -= (Vec.Dot(m.C__k__, m.C__j__) / Vec.Dot(m.C__k__, m.C__k__)) * m.C__k__;
                //# }
                //# }

                //# }
                return m;
            }

            private static __mnnt__ MgsrNorm(__mnnt__ m)
            {
                m.C0 = m.C0.Normalized;

                //# for (int j = 1; j < n; j++) {
                //# for (int i = 0; i < 2; i++) {
                //# for (int k = 0; k < j; k++) { 
                m.C__j__ -= (Vec.Dot(m.C__k__, m.C__j__) / Vec.Dot(m.C__k__, m.C__k__)) * m.C__k__;
                //# }
                //# }
                m.C__j__ = m.C__j__.Normalized;

                //# }
                return m;
            }

            public static Implementation GramSchmidtReortho
            {
                get => new Implementation
                {
                    Run = Mgsr,
                    Epsilon = __epsgsr__
                };
            }

            public static Implementation GramSchmidtReorthoNorm
            {
                get => new Implementation
                {
                    Run = MgsrNorm,
                    Epsilon = __epsgsr__
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
        readonly __mnnt__[] Matrices = new __mnnt__[count];

        public MatrixOrthogonalize__mnnt__()
        {
            var rnd = new RandomSystem(1);
            Matrices.SetByIndex(i => new __mnnt__(rnd.CreateUniform__Ftype__Array(16)) * 10 );
        }

        // This is probably not optimal...
        private double ErrorMeasure(__mnnt__ matrix)
        {
            var m = (__mnnd__)matrix;
            double error = 0;

            //# for (int j = 0; j < n; j++) {
            //# for (int k = j + 1; k < n; k++) {
            error += Vec.Dot(m.C__j__, m.C__k__).Square();
            //# }
            //# }

            return error;
        }

        private bool AreOrtho(__mnnt__ m, __ftype__ epsilon)
        {
            return ErrorMeasure(m) < epsilon;
        }

        private double[] ComputeAbsoluteError(Func<__mnnt__, __mnnt__> f)
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
            BenchmarkNumericalStability("__mnnt__.Orthogonalized()", Implementations.Orthogonalize);
            BenchmarkNumericalStability("__mnnt__.Orthonormalized()", Implementations.Orthonormalize);
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
        public __mnnt__ GramSchmidt()
        {
            __mnnt__ accum = __mnnt__.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidt.Run(Matrices[i]);
                accum += Implementations.GramSchmidtNorm.Run(Matrices[i]);
            }

            return accum;
        }

        [Benchmark]
        public __mnnt__ GramSchmidtWithReortho()
        {
            __mnnt__ accum = __mnnt__.Zero;

            for (int i = 0; i < count; i++)
            {
                accum += Implementations.GramSchmidtReortho.Run(Matrices[i]);
                accum += Implementations.GramSchmidtReorthoNorm.Run(Matrices[i]);
            }

            return accum;
        }
    }

    #endregion

    //# } }
}