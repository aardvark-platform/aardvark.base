using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using Aardvark.Base;

namespace Aardvark.Base.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run<V4fLength>();
            //BenchmarkRunner.Run<V3fLeng
            //BenchmarkRunner.Run<V2fLength>();
            //BenchmarkRunner.Run<CodeGenTests>();
            //BenchmarkRunner.Run<AngleBetweenFloat>();
            //BenchmarkRunner.Run<AngleBetweenDouble>();

            //var tf = new AngleBetweenFloat();
            //tf.BenchmarkNumericalStability();

            //var td = new AngleBetweenDouble();
            //td.BenchmarkNumericalStability();

            //var tf = new RotateIntoFloat();
            //tf.BenchmarkNumericalStability();

            //var td = new RotateIntoDouble();
            //td.BenchmarkNumericalStability();

            //var tf = new Rot3fGetEuler();
            //tf.BenchmarkNumericalStability();

            //var td = new Rot3dGetEuler();
            //td.BenchmarkNumericalStability();

            //var tf2 = new MatrixOrthogonalizeM22f();
            //tf2.BenchmarkNumericalStability();

            //var td2 = new MatrixOrthogonalizeM22d();
            //td2.BenchmarkNumericalStability();

            //var tf3 = new MatrixOrthogonalizeM33f();
            //tf3.BenchmarkNumericalStability();

            //var td3 = new MatrixOrthogonalizeM33d();
            //td3.BenchmarkNumericalStability();

            //var tf4 = new MatrixOrthogonalizeM44f();
            //tf4.BenchmarkNumericalStability();

            //var td4 = new MatrixOrthogonalizeM44d();
            //td4.BenchmarkNumericalStability();

            //BenchmarkRunner.Run<Rot3dTransform>();
            //BenchmarkRunner.Run<StaticConstants>();
            //BenchmarkRunner.Run<MatrixMultiply>();
            //BenchmarkRunner.Run<IntegerPowerFloat>();
            //BenchmarkRunner.Run<IntegerPowerDouble>();
            //BenchmarkRunner.Run<IntegerPowerInt>();
            //BenchmarkRunner.Run<IntegerPowerLong>();
            //BenchmarkRunner.Run<RotateIntoFloat>();
            //BenchmarkRunner.Run<RotateIntoDouble>();
            //BenchmarkRunner.Run<Rot3fGetEuler>();
            //BenchmarkRunner.Run<Rot3dGetEuler>();
            //BenchmarkRunner.Run<MatrixOrthogonalizeM22f>();
            //BenchmarkRunner.Run<MatrixOrthogonalizeM22d>();
            //BenchmarkRunner.Run<Indexers2>();
            //BenchmarkRunner.Run<Indexers3>();
            //BenchmarkRunner.Run<Indexers4>();
            //BenchmarkRunner.Run<MatrixMinor>();
            //BenchmarkRunner.Run<TransformV3d>();
            //BenchmarkRunner.Run<Log2Int>();
            //BenchmarkRunner.Run<Log2>();
            //BenchmarkRunner.Run<Enumerators>();
            //BenchmarkRunner.Run<IntSetCreator>();
            //BenchmarkRunner.Run<ArrayInit>();
            //BenchmarkRunner.Run<ArraySum>();
            //BenchmarkRunner.Run<SomeApp.HashCodeCombine>();
            BenchmarkRunner.Run<TelemetryProbesBenchmark>();

            //new Enumerators().Test();
        }
    }
}
