using System.Text.RegularExpressions;

internal sealed class DocsAnalyzer
{
    private static readonly Regex s_markdownLinkPattern = new(@"\[[^\]]+\]\(([^)]+)\)", RegexOptions.Compiled);

    private readonly DocsRules m_rules;

    public DocsAnalyzer(DocsRules? rules = null)
    {
        m_rules = rules ?? DocsRules.CreateDefault();
    }

    public IReadOnlyList<string> Analyze(string root)
    {
        var fullRoot = Path.GetFullPath(root);
        var failures = new List<string>();

        CheckRequiredFiles(fullRoot, failures);
        CheckMarkdownFiles(fullRoot, failures);
        CheckForbiddenPatterns(fullRoot, failures);
        CheckRequiredPatterns(fullRoot, failures);
        CheckSourceAnchors(fullRoot, failures);

        return failures;
    }

    private void CheckRequiredFiles(string root, List<string> failures)
    {
        foreach (var rel in m_rules.RequiredFiles)
        {
            var full = FullPath(root, rel);
            if (!File.Exists(full))
            {
                failures.Add($"Missing required file: {rel}");
                continue;
            }

            if (new FileInfo(full).Length == 0)
                failures.Add($"Empty required file: {rel}");
        }
    }

    private void CheckMarkdownFiles(string root, List<string> failures)
    {
        foreach (var markdownFile in EnumerateMarkdownFiles(root))
        {
            var relPath = Relative(root, markdownFile);
            var text = File.ReadAllText(markdownFile);

            foreach (var token in m_rules.MojibakeTokens)
            {
                if (!text.Contains(token, StringComparison.Ordinal)) continue;
                failures.Add($"Potential mojibake token '{token}' found in {relPath}");
                break;
            }

            ValidateMarkdownLinks(root, markdownFile, relPath, text, failures);
        }
    }

    private void CheckForbiddenPatterns(string root, List<string> failures)
    {
        foreach (var rule in m_rules.ForbiddenPatterns)
        {
            var full = FullPath(root, rule.File);
            if (!File.Exists(full)) continue;

            var text = File.ReadAllText(full);
            if (text.Contains(rule.Pattern, StringComparison.Ordinal))
                failures.Add($"Forbidden pattern in {rule.File}: '{rule.Pattern}'. {rule.Reason}");
        }
    }

    private void CheckRequiredPatterns(string root, List<string> failures)
    {
        foreach (var rule in m_rules.RequiredPatterns)
        {
            var full = FullPath(root, rule.File);
            if (!File.Exists(full))
            {
                failures.Add($"Missing required file for pattern check: {rule.File}");
                continue;
            }

            var text = File.ReadAllText(full);
            if (!text.Contains(rule.Pattern, StringComparison.Ordinal))
                failures.Add($"Missing required pattern in {rule.File}: '{rule.Pattern}'. {rule.Reason}");
        }
    }

    private void CheckSourceAnchors(string root, List<string> failures)
    {
        foreach (var rule in m_rules.SourceAnchors)
        {
            var full = FullPath(root, rule.File);
            if (!File.Exists(full))
            {
                failures.Add($"Missing source anchor file: {rule.File}");
                continue;
            }

            var text = File.ReadAllText(full);
            if (!text.Contains(rule.Snippet, StringComparison.Ordinal))
                failures.Add($"Source anchor not found in {rule.File}: '{rule.Snippet}'");
        }
    }

    private static IEnumerable<string> EnumerateMarkdownFiles(string root)
    {
        var readme = FullPath(root, "README.md");
        if (File.Exists(readme))
            yield return readme;

        var ai = FullPath(root, "ai");
        if (Directory.Exists(ai))
        {
            foreach (var file in Directory.EnumerateFiles(ai, "*.md", SearchOption.TopDirectoryOnly))
                yield return file;
        }

        var docs = FullPath(root, "docs");
        if (Directory.Exists(docs))
        {
            foreach (var file in Directory.EnumerateFiles(docs, "*.md", SearchOption.TopDirectoryOnly))
                yield return file;
        }

        var agents = FullPath(root, "AGENTS.md");
        if (File.Exists(agents))
            yield return agents;
    }

    private void ValidateMarkdownLinks(
        string root,
        string markdownFile,
        string relPath,
        string text,
        List<string> failures)
    {
        if (m_rules.LinkValidationIgnoreFiles.Contains(relPath))
            return;

        foreach (Match match in s_markdownLinkPattern.Matches(text))
        {
            if (match.Groups.Count < 2) continue;
            var rawLink = match.Groups[1].Value.Trim();
            if (string.IsNullOrEmpty(rawLink)) continue;

            var linkPath = NormalizeMarkdownDestination(rawLink);
            if (string.IsNullOrWhiteSpace(linkPath)) continue;

            if (linkPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                linkPath.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                linkPath.StartsWith("#", StringComparison.Ordinal))
            {
                continue;
            }

            if (linkPath.Contains("://", StringComparison.Ordinal)) continue;

            var relativeBase = Path.GetDirectoryName(markdownFile) ?? root;
            var normalized = linkPath.Replace('/', Path.DirectorySeparatorChar);
            var target = Path.GetFullPath(Path.Combine(relativeBase, normalized));

            if (!target.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            {
                failures.Add($"Suspicious out-of-repo link in {relPath}: {rawLink}");
                continue;
            }

            if (!File.Exists(target) && !Directory.Exists(target))
                failures.Add($"Broken local link in {relPath}: {rawLink}");
        }
    }

    private static string NormalizeMarkdownDestination(string rawLink)
    {
        var trimmed = rawLink.Trim();
        if (trimmed.Length == 0) return string.Empty;

        string destination;

        if (trimmed[0] == '<')
        {
            var closing = trimmed.IndexOf('>');
            destination = closing > 0 ? trimmed[1..closing] : trimmed;
        }
        else
        {
            destination = trimmed;

            for (var i = 0; i < trimmed.Length - 1; i++)
            {
                if (!char.IsWhiteSpace(trimmed[i])) continue;

                var next = trimmed[i + 1];
                if (next == '"' || next == '\'')
                {
                    destination = trimmed[..i];
                    break;
                }
            }
        }

        destination = destination.Trim();
        if (destination.Length >= 2 && destination[0] == '<' && destination[^1] == '>')
            destination = destination[1..^1].Trim();

        var fragmentIndex = destination.IndexOf('#');
        if (fragmentIndex >= 0)
            destination = destination[..fragmentIndex];

        return destination.Trim();
    }

    internal static string FullPath(string root, string rel)
        => Path.GetFullPath(Path.Combine(root, rel.Replace('/', Path.DirectorySeparatorChar)));

    internal static string Relative(string root, string full)
        => Path.GetRelativePath(root, full).Replace('\\', '/');
}
