using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aardvark.Base.Benchmarks.Geometry
{
    public static class TransformOverloadTargetedPerf
    {
        private const int WarmupRounds = 2;
        private const int MeasurementRounds = 7;
        private const double TargetMillisecondsPerRound = 150.0;

        private static readonly ITransformOverloadPerfCase[] Cases =
        {
            Create("Box2dForwardEuclidean", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Euclidean2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2dForwardSimilarity", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Similarity2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2dForwardAffine", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Affine2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Affine2d)),
            Create("Box2dForwardShift", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Shift2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2dForwardRot", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Rot2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2dForwardScale", () => TransformOverloadBenchData.Box2d.Transformed((M33d)TransformOverloadBenchData.Scale2d), () => TransformOverloadBenchData.Box2d.Transformed(TransformOverloadBenchData.Scale2d)),

            Create("Box2dInverseEuclidean", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Euclidean2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Box2dInverseSimilarity", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Similarity2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Similarity2d)),
            Create("Box2dInverseShift", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Shift2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Shift2d)),
            Create("Box2dInverseRot", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Rot2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Rot2d)),
            Create("Box2dInverseScale", () => TransformOverloadBenchData.Box2d.Transformed(((M33d)TransformOverloadBenchData.Scale2d).Inverse), () => TransformOverloadBenchData.Box2d.InvTransformed(TransformOverloadBenchData.Scale2d)),

            Create("Box3dForwardEuclidean", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Euclidean3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3dForwardSimilarity", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Similarity3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3dForwardAffine", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Affine3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Box3dForwardShift", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Shift3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3dForwardRot", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Rot3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3dForwardScale", () => TransformOverloadBenchData.Box3d.Transformed((M44d)TransformOverloadBenchData.Scale3d), () => TransformOverloadBenchData.Box3d.Transformed(TransformOverloadBenchData.Scale3d)),

            Create("Box3dInverseEuclidean", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Euclidean3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Box3dInverseSimilarity", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Similarity3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Box3dInverseShift", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Shift3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Box3dInverseRot", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Rot3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Box3dInverseScale", () => TransformOverloadBenchData.Box3d.Transformed(((M44d)TransformOverloadBenchData.Scale3d).Inverse), () => TransformOverloadBenchData.Box3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            Create("Hull2dForwardEuclidean", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Euclidean2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Hull2dForwardSimilarity", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Similarity2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Similarity2d)),
            Create("Hull2dForwardAffine", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Affine2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Affine2d)),
            Create("Hull2dForwardShift", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Shift2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Shift2d)),
            Create("Hull2dForwardRot", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Rot2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Rot2d)),
            Create("Hull2dForwardScale", () => TransformOverloadBenchData.Hull2d.Transformed(new Trafo2d(TransformOverloadBenchData.Scale2d)), () => TransformOverloadBenchData.Hull2d.Transformed(TransformOverloadBenchData.Scale2d)),

            Create("Hull2dInverseEuclidean", () => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Euclidean2d)), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Euclidean2d)),
            Create("Hull2dInverseSimilarity", () => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Similarity2d)), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Similarity2d)),
            Create("Hull2dInverseShift", () => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Shift2d)), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Shift2d)),
            Create("Hull2dInverseRot", () => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Rot2d)), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Rot2d)),
            Create("Hull2dInverseScale", () => TransformOverloadBenchData.Hull2d.InvTransformed(new Trafo2d(TransformOverloadBenchData.Scale2d)), () => TransformOverloadBenchData.Hull2d.InvTransformed(TransformOverloadBenchData.Scale2d)),

            Create("Hull3dForwardEuclidean", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Hull3dForwardSimilarity", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Hull3dForwardAffine", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Hull3dForwardShift", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Hull3dForwardRot", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Hull3dForwardScale", () => TransformOverloadBenchData.Hull3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d)), () => TransformOverloadBenchData.Hull3d.Transformed(TransformOverloadBenchData.Scale3d)),

            Create("Hull3dInverseEuclidean", () => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d)), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Hull3dInverseSimilarity", () => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Hull3dInverseShift", () => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Shift3d)), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Hull3dInverseRot", () => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Rot3d)), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Hull3dInverseScale", () => TransformOverloadBenchData.Hull3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Scale3d)), () => TransformOverloadBenchData.Hull3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            Create("FastHull3dForwardEuclidean", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Euclidean3d)),
            Create("FastHull3dForwardSimilarity", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("FastHull3dForwardAffine", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("FastHull3dForwardShift", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("FastHull3dForwardRot", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("FastHull3dForwardScale", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d))), () => TransformOverloadBenchData.FastHull3d.Transformed(TransformOverloadBenchData.Scale3d)),

            Create("FastHull3dInverseEuclidean", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d))), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("FastHull3dInverseSimilarity", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Similarity3d))), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("FastHull3dInverseShift", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Shift3d))), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("FastHull3dInverseRot", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Rot3d))), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("FastHull3dInverseScale", () => new FastHull3d(TransformOverloadBenchData.FastHull3d.Hull.InvTransformed(new Trafo3d(TransformOverloadBenchData.Scale3d))), () => TransformOverloadBenchData.FastHull3d.InvTransformed(TransformOverloadBenchData.Scale3d)),

            Create("Plane3dForwardSimilarity", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Similarity3d)),
            Create("Plane3dForwardAffine", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Affine3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Affine3d)),
            Create("Plane3dForwardShift", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Shift3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Shift3d)),
            Create("Plane3dForwardRot", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Rot3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Rot3d)),
            Create("Plane3dForwardScale", () => TransformOverloadBenchData.Plane3d.Transformed(new Trafo3d(TransformOverloadBenchData.Scale3d)), () => TransformOverloadBenchData.Plane3d.Transformed(TransformOverloadBenchData.Scale3d)),

            Create("Plane3dInverseEuclidean", () => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Euclidean3d)), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Euclidean3d)),
            Create("Plane3dInverseSimilarity", () => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Similarity3d)), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Similarity3d)),
            Create("Plane3dInverseShift", () => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Shift3d)), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Shift3d)),
            Create("Plane3dInverseRot", () => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Rot3d)), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Rot3d)),
            Create("Plane3dInverseScale", () => TransformOverloadBenchData.Plane3d.InvTransformed(new Trafo3d(TransformOverloadBenchData.Scale3d)), () => TransformOverloadBenchData.Plane3d.InvTransformed(TransformOverloadBenchData.Scale3d)),
        };

        public static bool TryHandle(string[] args)
        {
            if (args.Contains("--list-transform-perf-cases", StringComparer.OrdinalIgnoreCase))
            {
                foreach (var @case in Cases.OrderBy(static c => c.Name, StringComparer.OrdinalIgnoreCase))
                    Console.WriteLine(@case.Name);
                return true;
            }

            if (!args.Contains("--targeted-transform-perf", StringComparer.OrdinalIgnoreCase))
                return false;

            var filter = GetOption(args, "--case");
            var outputDir = GetOption(args, "--output-dir");
            if (string.IsNullOrWhiteSpace(outputDir))
                outputDir = Path.Combine("BenchmarkDotNet.Artifacts", "results");
            var selected = string.IsNullOrWhiteSpace(filter)
                ? Cases
                : Cases.Where(c => c.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToArray();

            if (selected.Length == 0)
                throw new InvalidOperationException($"No targeted transform perf cases matched '{filter}'.");

            Directory.CreateDirectory(outputDir);

            var results = new List<TransformOverloadPerfResult>(selected.Length);
            foreach (var @case in selected)
            {
                var result = @case.Run();
                results.Add(result);
                Console.WriteLine(
                    $"{result.Name,-28} ops={result.Operations,8} baseline={result.BaselineNanosecondsPerOperation,10:F3} ns/op " +
                    $"specialized={result.SpecializedNanosecondsPerOperation,10:F3} ns/op ratio={result.Ratio,6:F3} " +
                    $"alloc={result.BaselineAllocatedBytesPerOperation,8:F3}/{result.SpecializedAllocatedBytesPerOperation,8:F3} B/op");
            }

            WriteReports(outputDir, results);
            return true;
        }

        private static void WriteReports(string outputDir, IReadOnlyList<TransformOverloadPerfResult> results)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            var csvPath = Path.Combine(outputDir, $"TransformOverloadTargetedPerf-{timestamp}.csv");
            var markdownPath = Path.Combine(outputDir, $"TransformOverloadTargetedPerf-{timestamp}.md");

            var csv = new StringBuilder();
            csv.AppendLine("Name,Operations,BaselineNsPerOp,SpecializedNsPerOp,Ratio,BaselineAllocatedBytesPerOp,SpecializedAllocatedBytesPerOp");
            foreach (var result in results)
            {
                csv.Append(result.Name).Append(',')
                   .Append(result.Operations.ToString(CultureInfo.InvariantCulture)).Append(',')
                   .Append(result.BaselineNanosecondsPerOperation.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(result.SpecializedNanosecondsPerOperation.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(result.Ratio.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(result.BaselineAllocatedBytesPerOperation.ToString("F6", CultureInfo.InvariantCulture)).Append(',')
                   .Append(result.SpecializedAllocatedBytesPerOperation.ToString("F6", CultureInfo.InvariantCulture)).AppendLine();
            }
            File.WriteAllText(csvPath, csv.ToString());

            var markdown = new StringBuilder();
            markdown.AppendLine("# Transform Overload Targeted Perf");
            markdown.AppendLine();
            markdown.AppendLine("| Case | Ops | Baseline ns/op | Specialized ns/op | Ratio | Baseline B/op | Specialized B/op |");
            markdown.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: | ---: |");
            foreach (var result in results)
            {
                markdown.Append("| ").Append(result.Name)
                        .Append(" | ").Append(result.Operations.ToString(CultureInfo.InvariantCulture))
                        .Append(" | ").Append(result.BaselineNanosecondsPerOperation.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(result.SpecializedNanosecondsPerOperation.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(result.Ratio.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(result.BaselineAllocatedBytesPerOperation.ToString("F3", CultureInfo.InvariantCulture))
                        .Append(" | ").Append(result.SpecializedAllocatedBytesPerOperation.ToString("F3", CultureInfo.InvariantCulture))
                        .AppendLine(" |");
            }
            File.WriteAllText(markdownPath, markdown.ToString());

            Console.WriteLine($"Wrote targeted perf reports to {csvPath} and {markdownPath}");
        }

        private static string GetOption(string[] args, string name)
        {
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
                    return args[i + 1];
            }

            return string.Empty;
        }

        private static ITransformOverloadPerfCase Create<T>(string name, Func<T> baseline, Func<T> specialized)
            => new TransformOverloadPerfCase<T>(name, baseline, specialized, WarmupRounds, MeasurementRounds, TargetMillisecondsPerRound);
    }

    public readonly struct TransformOverloadPerfResult
    {
        public TransformOverloadPerfResult(string name, int operations, double baselineNanosecondsPerOperation, double specializedNanosecondsPerOperation, double baselineAllocatedBytesPerOperation, double specializedAllocatedBytesPerOperation)
        {
            Name = name;
            Operations = operations;
            BaselineNanosecondsPerOperation = baselineNanosecondsPerOperation;
            SpecializedNanosecondsPerOperation = specializedNanosecondsPerOperation;
            BaselineAllocatedBytesPerOperation = baselineAllocatedBytesPerOperation;
            SpecializedAllocatedBytesPerOperation = specializedAllocatedBytesPerOperation;
        }

        public string Name { get; }
        public int Operations { get; }
        public double BaselineNanosecondsPerOperation { get; }
        public double SpecializedNanosecondsPerOperation { get; }
        public double BaselineAllocatedBytesPerOperation { get; }
        public double SpecializedAllocatedBytesPerOperation { get; }
        public double Ratio => SpecializedNanosecondsPerOperation / BaselineNanosecondsPerOperation;
    }

    internal interface ITransformOverloadPerfCase
    {
        string Name { get; }
        TransformOverloadPerfResult Run();
    }

    internal sealed class TransformOverloadPerfCase<T> : ITransformOverloadPerfCase
    {
        private readonly Func<T> _baseline;
        private readonly Func<T> _specialized;
        private readonly int _warmupRounds;
        private readonly int _measurementRounds;
        private readonly double _targetMillisecondsPerRound;

        public TransformOverloadPerfCase(string name, Func<T> baseline, Func<T> specialized, int warmupRounds, int measurementRounds, double targetMillisecondsPerRound)
        {
            Name = name;
            _baseline = baseline;
            _specialized = specialized;
            _warmupRounds = warmupRounds;
            _measurementRounds = measurementRounds;
            _targetMillisecondsPerRound = targetMillisecondsPerRound;
        }

        public string Name { get; }

        public TransformOverloadPerfResult Run()
        {
            Consume(_baseline());
            Consume(_specialized());

            var operations = CalibrateOperations();

            for (var i = 0; i < _warmupRounds; i++)
            {
                Measure(_baseline, operations, collectAllocations: false);
                Measure(_specialized, operations, collectAllocations: false);
            }

            var baselineTimes = new double[_measurementRounds];
            var specializedTimes = new double[_measurementRounds];
            var baselineAllocations = new double[_measurementRounds];
            var specializedAllocations = new double[_measurementRounds];

            for (var i = 0; i < _measurementRounds; i++)
            {
                var baseline = Measure(_baseline, operations, collectAllocations: true);
                var specialized = Measure(_specialized, operations, collectAllocations: true);

                baselineTimes[i] = baseline.Elapsed.TotalMilliseconds * 1_000_000.0 / operations;
                specializedTimes[i] = specialized.Elapsed.TotalMilliseconds * 1_000_000.0 / operations;
                baselineAllocations[i] = baseline.AllocatedBytes / (double)operations;
                specializedAllocations[i] = specialized.AllocatedBytes / (double)operations;
            }

            Array.Sort(baselineTimes);
            Array.Sort(specializedTimes);
            Array.Sort(baselineAllocations);
            Array.Sort(specializedAllocations);

            return new TransformOverloadPerfResult(
                Name,
                operations,
                baselineTimes[_measurementRounds / 2],
                specializedTimes[_measurementRounds / 2],
                baselineAllocations[_measurementRounds / 2],
                specializedAllocations[_measurementRounds / 2]);
        }

        private int CalibrateOperations()
        {
            var operations = 1;
            while (operations < 1 << 26)
            {
                var measurement = Measure(_baseline, operations, collectAllocations: false);
                if (measurement.Elapsed.TotalMilliseconds >= _targetMillisecondsPerRound)
                    return operations;
                operations <<= 1;
            }

            return operations;
        }

        private static PerfMeasurement Measure(Func<T> func, int operations, bool collectAllocations)
        {
            if (collectAllocations)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            var allocatedBefore = collectAllocations ? GC.GetAllocatedBytesForCurrentThread() : 0L;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < operations; i++)
                Consume(func());
            stopwatch.Stop();
            var allocatedAfter = collectAllocations ? GC.GetAllocatedBytesForCurrentThread() : allocatedBefore;

            return new PerfMeasurement(stopwatch.Elapsed, allocatedAfter - allocatedBefore);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Consume(T value) => PerfSink<T>.Value = value;
    }

    internal readonly struct PerfMeasurement
    {
        public PerfMeasurement(TimeSpan elapsed, long allocatedBytes)
        {
            Elapsed = elapsed;
            AllocatedBytes = allocatedBytes;
        }

        public TimeSpan Elapsed { get; }
        public long AllocatedBytes { get; }
    }

    internal static class PerfSink<T>
    {
        public static T Value = default!;
    }
}
