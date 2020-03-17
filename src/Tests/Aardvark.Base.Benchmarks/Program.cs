using BenchmarkDotNet.Running;
using System;

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

            var tf = new RotateIntoFloat();
            tf.BenchmarkNumericalStability();

            var td = new RotateIntoDouble();
            td.BenchmarkNumericalStability();

            //BenchmarkRunner.Run<Rot3dTransform>();
            //BenchmarkRunner.Run<StaticConstants>();
            //BenchmarkRunner.Run<MatrixMultiply>();
            //BenchmarkRunner.Run<IntegerPowerFloat>();
            //BenchmarkRunner.Run<IntegerPowerDouble>();
            //BenchmarkRunner.Run<IntegerPowerInt>();
            //BenchmarkRunner.Run<IntegerPowerLong>();
            //BenchmarkRunner.Run<RotateIntoFloat>();
            //BenchmarkRunner.Run<RotateIntoDouble>();
        }
    }
}
