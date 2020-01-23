using BenchmarkDotNet.Running;
using System;

namespace Aardvark.Base.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<V4fLength>();
            BenchmarkRunner.Run<V3fLength>();
            BenchmarkRunner.Run<V2fLength>();
            //BenchmarkRunner.Run<CodeGenTests>();
        }
    }
}
