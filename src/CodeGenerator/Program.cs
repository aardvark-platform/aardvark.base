using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace CodeGenerator
{
    class Program
    {
        private struct TaskPattern
        {
            public string TemplateEnding;
            public string AutoEnding;
        }

        private static readonly TaskPattern[] c_patterns = new[]
            {
                new TaskPattern
                {
                    TemplateEnding = "_template.cs",
                    AutoEnding = "_auto.cs",
                },
                //new TaskPattern
                //{
                //    TemplateEnding = "_templateF.cs",
                //    AutoEnding = "_autoF.cs"
                //},
                new TaskPattern
                {
                    TemplateEnding = "_template.cl",
                    AutoEnding = "_auto.cl",
                },
            };

        private const string c_generatorEnding = "_generator.cs";

        private struct Task
        {
            public string TemplateFileName;
            public string OutputFileName;
            public string Base;
            public string Report;
            // public bool OnlyPerformIfOutputFileDoesNotExist;
        }

        private static bool IsOlderThan(string fileName1, string fileName2)
        {
            return File.GetLastWriteTimeUtc(fileName1) < File.GetLastWriteTimeUtc(fileName2);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var tasks = new List<Task>();
            string dir = null;
            bool writeGenerator = true;

            if (args.Length > 0)
            {
                if (args[0].EndsWith(".conf"))
                {
                    foreach (var line in File.ReadAllLines(args[0]))
                    {
                        var parts = line.Split(new char[] { ' ', '\t' },
                                    StringSplitOptions.RemoveEmptyEntries);
                        tasks.Add(new Task
                        {
                            TemplateFileName = parts[0],
                            OutputFileName = parts[1]
                        });
                    }
                }
                else if (Directory.Exists(args[0]))
                    dir = args[0];
                else
                    tasks.Add(new Task { TemplateFileName = args[0], OutputFileName = args[1] });
            }
            else
                dir = @"..\..\..";

            if (dir != null)
            {
                foreach (var pattern in c_patterns)
                {
                    var templates = Directory.GetFiles(dir, "*" + pattern.TemplateEnding,
                                                       SearchOption.AllDirectories);
                    foreach (var name in templates)
                    {
                        var baseName = name.Substring(0, name.Length - pattern.TemplateEnding.Length);
                        tasks.Add(new Task
                        {
                            TemplateFileName = name,
                            Report = baseName.Substring(dir.Length),
                            Base = baseName,
                            OutputFileName = baseName + pattern.AutoEnding,
                        });
                    }
                }
            }

            foreach (var task in tasks)
            {
                string report = task.Report ?? task.OutputFileName;

                if (task.TemplateFileName != null &&
                    IsOlderThan(task.TemplateFileName, task.OutputFileName))
                {
                    Console.WriteLine("- {0}", report);
                    continue;
                }
                Console.WriteLine("# {0}", report);

                string input = File.ReadAllText(task.TemplateFileName);

                var engine = new TemplateProcessor() { Input = input };

                engine.CreateGenerator();

                if (writeGenerator)
                {
                    var baseName = task.Base ?? task.OutputFileName;
                    var genName = baseName + c_generatorEnding;
                    var genReport = report + c_generatorEnding;
                    File.WriteAllText(genName, engine.GeneratorSourceCode);
                    Console.WriteLine("GENERATOR {0}", genReport);
                }
                engine.CompileAndRunGenerator();
                if (engine.Result == null)
                {
                    //File.WriteAllText(task.OutputFileName + ".debug.cs", engine.GeneratorSourceCode);
                    Console.WriteLine("WARNING: processing template failed!");
                }
                else
                {
                    int lineCountInput = engine.Input.Split('\n').Count();
                    int lineCountResult = engine.Result.Split('\n').Count();
                    double ratio = lineCountResult / (double)lineCountInput;
                    Console.WriteLine("{0} lines out of {1} lines (factor {2:0.00})",
                        lineCountResult, lineCountInput, ratio
                        );

                    File.WriteAllText(task.OutputFileName, engine.Result);
                }
                Console.WriteLine("+");
            }
        }
    }
}
