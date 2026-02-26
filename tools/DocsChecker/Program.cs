internal static class Program
{
    private static int Main(string[] args)
    {
        var root = args.Length > 0
            ? Path.GetFullPath(args[0])
            : Directory.GetCurrentDirectory();

        var analyzer = new DocsAnalyzer();
        var failures = analyzer.Analyze(root);

        if (failures.Count > 0)
        {
            Console.Error.WriteLine("Docs check failed:");
            foreach (var failure in failures)
                Console.Error.WriteLine($"- {failure}");
            return 1;
        }

        Console.WriteLine("Docs check passed.");
        return 0;
    }
}
