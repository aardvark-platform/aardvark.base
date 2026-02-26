internal readonly record struct PatternRule(string File, string Pattern, string Reason);

internal readonly record struct SourceAnchorRule(string File, string Snippet);

internal sealed class DocsRules
{
    public required IReadOnlyList<string> RequiredFiles { get; init; }
    public required IReadOnlyList<PatternRule> ForbiddenPatterns { get; init; }
    public required IReadOnlyList<PatternRule> RequiredPatterns { get; init; }
    public required IReadOnlyList<SourceAnchorRule> SourceAnchors { get; init; }
    public required IReadOnlySet<string> LinkValidationIgnoreFiles { get; init; }
    public required IReadOnlyList<string> MojibakeTokens { get; init; }

    public static DocsRules CreateDefault()
        => new()
        {
            RequiredFiles =
            [
                "AGENTS.md",
                "ai/README.md",
                "ai/SYMBOL_INDEX.md",
                "ai/SEMANTICS_LINEAR_ALGEBRA.md",
                "ai/SEMANTICS_GEOMETRY_CORE.md",
                "ai/PRIMITIVE_TYPES.md",
                "ai/UTILITIES.md",
                "ai/COLLECTIONS.md",
                "ai/ALGORITHMS.md",
                "ai/PIXIMAGE.md",
                "ai/TENSORS.md",
                "ai/FSHARP_INTEROP.md",
                "ai/INCREMENTAL.md",
                "ai/SERIALIZATION.md",
                "docs/CONTRIBUTING.md",
                "docs/INTEROP.md",
                "docs/TROUBLESHOOTING.md",
            ],
            ForbiddenPatterns =
            [
                new("docs/CONTRIBUTING.md", "--config", "build.sh/build.cmd do not support --config."),
                new("docs/CONTRIBUTING.md", "./build.sh --project", "build.sh/build.cmd do not support --project."),
                new("docs/CONTRIBUTING.md", ".\\build.cmd --project", "build.sh/build.cmd do not support --project."),
                new("docs/CONTRIBUTING.md", "./test.sh --filter", "Use dotnet test ... --filter for filtered runs."),
                new("docs/CONTRIBUTING.md", ".\\test.cmd --filter", "Use dotnet test ... --filter for filtered runs."),
                new("docs/INTEROP.md", "F# Projects Reference C#, Never the Reverse", "Repository intentionally has mixed C#/F# reference directions."),
                new("ai/UTILITIES.md", "Telemetry.Reset()", "Actual API is Telemetry.ResetTelemetrySystem()."),
                new("ai/UTILITIES.md", "Constant<float>.Pi", "Pi/E are on Constant / ConstantF, not Constant<T>."),
                new("ai/UTILITIES.md", "Constant<double>.Pi", "Pi/E are on Constant / ConstantF, not Constant<T>."),
                new("ai/PIXIMAGE.md", "PixImage.Load<byte>", "Static generic PixImage.Load<T> is not available."),
                new("ai/PIXIMAGE.md", "PixImage.Load<float>", "Static generic PixImage.Load<T> is not available."),
                new("ai/COLLECTIONS.md", "TryRemove(item)", "ConcurrentHashSet<T> uses Remove, not TryRemove."),
                new("ai/TENSORS.md", "class Matrix<", "Generated tensor containers are structs."),
                new("ai/TENSORS.md", "class Volume<", "Generated tensor containers are structs."),
            ],
            RequiredPatterns =
            [
                new("AGENTS.md", "check-docs.sh", "AGENTS should expose docs checker command."),
                new("AGENTS.md", "check-docs.cmd", "AGENTS should expose docs checker command."),
                new("ai/README.md", "SYMBOL_INDEX.md", "README should expose symbol index."),
                new("ai/README.md", "SEMANTICS_LINEAR_ALGEBRA.md", "README should expose semantic deep-dive docs."),
                new("ai/README.md", "SEMANTICS_GEOMETRY_CORE.md", "README should expose semantic deep-dive docs."),
                new("ai/SEMANTICS_LINEAR_ALGEBRA.md", "row-major", "Linear algebra semantics should state row-major storage."),
                new("ai/SEMANTICS_LINEAR_ALGEBRA.md", "FromCols", "Linear algebra semantics should include conversion guidance."),
                new("ai/SEMANTICS_GEOMETRY_CORE.md", "TransformPos", "Geometry semantics should explicitly reference TransformPos/TransformDir."),
                new("ai/PIXIMAGE.md", "No static `PixImage.Load<T>(...)` API exists.", "PixImage docs should guard against stale generic load examples."),
                new("ai/UTILITIES.md", "ResetTelemetrySystem", "Utilities docs should use correct telemetry reset API."),
                new("ai/TENSORS.md", "`struct` types", "Tensor docs should call out struct container reality."),
                new("docs/CONTRIBUTING.md", "dotnet test src/Aardvark.sln --filter", "Contributing guide should show valid filtered test command."),
                new("docs/INTEROP.md", "mixed C#/F# project references", "Interop guide must reflect actual dependency reality."),
            ],
            SourceAnchors =
            [
                new("src/Aardvark.Base/Math/Trafos/Matrix_auto.cs", "public partial struct M44d"),
                new("src/Aardvark.Base/Math/Trafos/Matrix_auto.cs", "public static M44d FromCols(V4d col0, V4d col1, V4d col2, V4d col3)"),
                new("src/Aardvark.Base/Math/Trafos/Matrix_auto.cs", "public static V4d operator *(M44d m, V4d v)"),
                new("src/Aardvark.Base/Math/Trafos/Matrix_auto.cs", "public static V3d TransformPos(this M44d m, V3d p)"),
                new("src/Aardvark.Base/Math/Trafos/Matrix_auto.cs", "public static V3d TransposedTransformPos(this M44d m, V3d p)"),
                new("src/Aardvark.Base/Math/Trafos/Trafo_auto.cs", "public readonly partial struct Trafo3d : IEquatable<Trafo3d>"),
                new("src/Aardvark.Base/Math/Trafos/Trafo_auto.cs", "public static Trafo3d operator *(Trafo3d t0, Trafo3d t1)"),
                new("src/Aardvark.Base/Math/Colors/Color.cs", "public readonly struct PixFormat : IEquatable<PixFormat>"),
                new("src/Aardvark.Base.Tensors.CSharp/Tensor_auto.cs", "public partial struct Matrix<Td> : IValidity, IMatrix<Td>, IArrayMatrix"),
                new("src/Aardvark.Base.Tensors.CSharp/Tensor_auto.cs", "public partial struct Volume<Td> : IValidity, IVolume<Td>, IArrayVolume"),
                new("src/Aardvark.Base.Tensors.CSharp/Tensor_auto.cs", "public partial struct Tensor4<Td> : IValidity, ITensor4<Td>, IArrayTensor4"),
                new("src/Aardvark.Base.Tensors.CSharp/PixImage/PixImage.cs", "public static PixImage Load(string filename, IPixLoader loader = null)"),
                new("src/Aardvark.Base.Tensors.CSharp/PixImage/PixImage.cs", "public void SaveAsJpeg(string filename, int quality = PixJpegSaveParams.DefaultQuality,"),
                new("src/Aardvark.Base.Tensors.CSharp/PixImage/PixImage.cs", "public static PixImageInfo GetInfoFromFile(string filename, IPixLoader loader = null)"),
                new("src/Aardvark.Base.Tensors.CSharp/Tensors/ImageTensors.cs", "public static bool HasImageLayout<T>(this Volume<T> volume)"),
                new("src/Aardvark.Base.Telemetry/IProbe.cs", "public static void ResetTelemetrySystem()"),
                new("src/Aardvark.Base/Reporting/Report.cs", "public static void Trace([Localizable(true)] string line, params object[] args)"),
                new("src/Aardvark.Base/Random/RandomSample.cs", "public static V3d Lambertian(V3d normal, IRandomSeries rnds, int seriesIndex)"),
                new("src/Aardvark.Base/Random/HaltonRandomSeries.cs", "public class HaltonRandomSeries : IRandomSeries"),
                new("src/Aardvark.Base/Geodesy/GeoConversion.cs", "public static V3d XyzFromLonLatHeight(V3d lonLatHeight, GeoEllipsoid ellipsoid)"),
                new("src/Aardvark.Base/Geodesy/GeoConsts.cs", "public static readonly GeoEllipsoid Wgs84 = FromAF(6378137.0, 1.0 / 298.257223563);"),
                new("src/Aardvark.Base/Symbol/Symbol.cs", "public readonly struct Symbol : IEquatable<Symbol>, IComparable<Symbol>, IComparable"),
                new("src/Aardvark.Base/AlgoDat/LruCache.cs", "public class LruCache<TKey, TValue>"),
                new("src/Aardvark.Base/AlgoDat/ConcurrentHashSet.cs", "public bool Remove(T item) => m_entries.TryRemove(item, out int dummy);"),
                new("src/Aardvark.Base/AlgoDat/ShortestPath.cs", "public class ShortestPath<T> : IShortestPath<T>"),
                new("src/Aardvark.Base/Geometry/BbTree.cs", "public class BbTree"),
                new("src/Aardvark.Base/Math/Base/DistributionFunction.cs", "public class DistributionFunction"),
                new("src/Aardvark.Base/Math/Numerics/Polynomial.cs", "public static class Polynomial"),
            ],
            LinkValidationIgnoreFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ai/RECIPE_AI_FRIENDLINESS.md",
            },
            MojibakeTokens = ["â", "Ã", "Â", "�"],
        };
}
