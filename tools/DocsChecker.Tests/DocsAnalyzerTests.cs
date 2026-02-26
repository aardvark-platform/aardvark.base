using System.Text;
using Xunit;

public sealed class DocsAnalyzerTests
{
    [Fact]
    public void ValidFixture_PassesWithoutFailures()
    {
        using var repo = TempRepo.CreateValid();
        var analyzer = new DocsAnalyzer();

        var failures = analyzer.Analyze(repo.Root);

        Assert.Empty(failures);
    }

    [Fact]
    public void BrokenLink_IsReported()
    {
        using var repo = TempRepo.CreateValid();
        repo.Append("ai/README.md", "\n[broken](missing.md)\n");
        var analyzer = new DocsAnalyzer();

        var failures = analyzer.Analyze(repo.Root);

        Assert.Contains("Broken local link in ai/README.md: missing.md", failures);
    }

    [Fact]
    public void ForbiddenPattern_IsReported()
    {
        using var repo = TempRepo.CreateValid();
        repo.Append("docs/CONTRIBUTING.md", "\n./build.sh --project src/Aardvark.Base/Aardvark.Base.csproj\n");
        var analyzer = new DocsAnalyzer();

        var failures = analyzer.Analyze(repo.Root);

        Assert.Contains(failures, f => f.Contains("Forbidden pattern in docs/CONTRIBUTING.md", StringComparison.Ordinal));
    }

    [Fact]
    public void MissingRequiredPattern_IsReported()
    {
        using var repo = TempRepo.CreateValid();
        repo.Write("ai/SEMANTICS_LINEAR_ALGEBRA.md", "layout notes without required token");
        var analyzer = new DocsAnalyzer();

        var failures = analyzer.Analyze(repo.Root);

        Assert.Contains(
            "Missing required pattern in ai/SEMANTICS_LINEAR_ALGEBRA.md: 'row-major'. Linear algebra semantics should state row-major storage.",
            failures);
    }

    [Fact]
    public void MissingSourceAnchor_IsReported()
    {
        using var repo = TempRepo.CreateValid();
        repo.Write("src/Aardvark.Base/Symbol/Symbol.cs", "public sealed class NotASymbol {}");
        var analyzer = new DocsAnalyzer();

        var failures = analyzer.Analyze(repo.Root);

        Assert.Contains(
            "Source anchor not found in src/Aardvark.Base/Symbol/Symbol.cs: 'public readonly struct Symbol : IEquatable<Symbol>, IComparable<Symbol>, IComparable'",
            failures);
    }

    [Fact]
    public void MojibakeToken_IsReported()
    {
        using var repo = TempRepo.CreateValid();
        repo.Append("ai/UTILITIES.md", "\nPotential mojibake: Ã\n");
        var analyzer = new DocsAnalyzer();

        var failures = analyzer.Analyze(repo.Root);

        Assert.Contains("Potential mojibake token 'Ã' found in ai/UTILITIES.md", failures);
    }

    [Fact]
    public void IntegrationFixture_ReportsExactFailureMessages()
    {
        using var repo = TempRepo.CreateValid();
        repo.Append("ai/README.md", "\n[broken](missing.md)\n");
        repo.Write("src/Aardvark.Base/Math/Numerics/Polynomial.cs", "public static class NotPolynomial {}");
        var analyzer = new DocsAnalyzer();

        var failures = analyzer.Analyze(repo.Root);

        var expected = new[]
        {
            "Broken local link in ai/README.md: missing.md",
            "Source anchor not found in src/Aardvark.Base/Math/Numerics/Polynomial.cs: 'public static class Polynomial'",
        };

        foreach (var e in expected)
            Assert.Contains(e, failures);
    }

    private sealed class TempRepo : IDisposable
    {
        private readonly string m_root;
        private bool m_disposed;

        public string Root => m_root;

        private TempRepo(string root)
        {
            m_root = root;
        }

        public static TempRepo CreateValid()
        {
            var root = Path.Combine(Path.GetTempPath(), "docs-checker-tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(root);

            var repo = new TempRepo(root);
            repo.InitializeValidFixture();
            return repo;
        }

        public void Write(string relPath, string content)
        {
            var full = Path.Combine(m_root, relPath.Replace('/', Path.DirectorySeparatorChar));
            var dir = Path.GetDirectoryName(full);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(full, content, new UTF8Encoding(false));
        }

        public void Append(string relPath, string content)
        {
            var full = Path.Combine(m_root, relPath.Replace('/', Path.DirectorySeparatorChar));
            File.AppendAllText(full, content, new UTF8Encoding(false));
        }

        private void InitializeValidFixture()
        {
            var rules = DocsRules.CreateDefault();

            foreach (var rel in rules.RequiredFiles)
                Write(rel, "# placeholder");

            Write("AGENTS.md", "check-docs.sh\ncheck-docs.cmd\n");
            Write("ai/README.md", "SYMBOL_INDEX.md\nSEMANTICS_LINEAR_ALGEBRA.md\nSEMANTICS_GEOMETRY_CORE.md\n");
            Write("ai/SEMANTICS_LINEAR_ALGEBRA.md", "row-major\nFromCols\n");
            Write("ai/SEMANTICS_GEOMETRY_CORE.md", "TransformPos and TransformDir semantics\n");
            Write("ai/PIXIMAGE.md", "No static `PixImage.Load<T>(...)` API exists.\n");
            Write("ai/UTILITIES.md", "Use ResetTelemetrySystem for reset.\n");
            Write("ai/TENSORS.md", "These are `struct` types in generated code.\n");
            Write("docs/CONTRIBUTING.md", "dotnet test src/Aardvark.sln --filter \"FullyQualifiedName~Vector\"\n");
            Write("docs/INTEROP.md", "This repo has mixed C#/F# project references.\n");

            foreach (var group in rules.SourceAnchors.GroupBy(x => x.File))
            {
                var content = string.Join("\n", group.Select(x => x.Snippet)) + "\n";
                Write(group.Key, content);
            }
        }

        public void Dispose()
        {
            if (m_disposed) return;
            m_disposed = true;

            if (Directory.Exists(m_root))
            {
                try
                {
                    Directory.Delete(m_root, recursive: true);
                }
                catch
                {
                    // Best effort cleanup for temp fixtures.
                }
            }
        }
    }
}
