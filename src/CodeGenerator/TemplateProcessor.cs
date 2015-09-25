using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aardvark.Base;

namespace CodeGenerator
{

    public class TemplateProcessor : TextParser<TemplateProcessor>
    {
        public string Input;

        public StringBuilder Code;
        public StringBuilder Using;
        public StringBuilder Class;

        public StringBuilder Active;

        public string GeneratorSourceCode;
        public string Result;

        #region Constructor

        public TemplateProcessor()
        {
            Input = null;

            Code = new StringBuilder();
            Using = new StringBuilder();
            Class = new StringBuilder();
            Active = Code;

            GeneratorSourceCode = null;
            Result = null;
        }

        #endregion

        #region Constants

        public string StandardUsing = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using CodeGenerator;
using Aardvark.Base;
using Aardvark.Base.CSharp;
";

        public string Prologue = @"
public static class SourceGenerator
{
    public static StringBuilder ___sb = new StringBuilder();
    public static Func<string, string> Filter = null;
    public static Func<string, string> ___Filter = s => Filter == null ? s : Filter(s);
    public static Action<string> Out = s => { ___sb.Append(___Filter(s)); };
    public static string Generate()
    {
";

        public string StandardClass = @"
        return ___sb.ToString();
    }
";

        public string Epilogue = @"
}
";

        #endregion

        #region Operations

        public void Perform()
        {
            CreateGenerator();
            CompileAndRunGenerator();
        }

        public void CreateGenerator() { CreateGenerator(null); }

        public void CreateGenerator(string injectAfterPrologue)
        {
            if (injectAfterPrologue == null) injectAfterPrologue = "";

            Parse(new Text(Input), this, State, new Nd());

            GeneratorSourceCode = StandardUsing
                                    + Using.ToString()
                                    + Prologue
                                    + injectAfterPrologue
                                    + Code.ToString()
                                    + StandardClass
                                    + Class.ToString()
                                    + Epilogue;

            // ReportUsings();
        }

        public void CompileAndRunGenerator()
        {
            CompilerResults results = null;
            var generatorAssembly = CompilerServices.CompileAssembly(
                GeneratorSourceCode.IntoArray(),
                new string[] {
                    "System.Xml.dll",
                    "System.Xml.Linq.dll",
                    "Aardvark.Base.dll",
                },
                ".", out results);
            if (results.Errors.Count > 0)
            {
                Console.WriteLine("WARNING: build of generator failed!");
                foreach (var x in results.Errors) Console.WriteLine("{0}", x);
                Result = null;
                return;
            }
            var generatorFun = generatorAssembly.GetTypes().First().GetMethods().First();
            Result = (string)generatorFun.Invoke(null, null);
        }

        #endregion

        #region Private Methods

        private void ReportUsings()
        {
            var nonSystemUsings = FilterNonSystemUsingsIntoList(Using.ToString());
            foreach (var use in nonSystemUsings) Console.WriteLine("USING {0};", use);
        }

        private static List<string> FilterNonSystemUsingsIntoList(string usings)
        {
            var genUse = from use in
                             (from rawUse in usings.Split(
                                             new string[] { Environment.NewLine },
                                             StringSplitOptions.RemoveEmptyEntries)
                              let trimUse = rawUse.Trim()
                              where trimUse.StartsWith("using ")
                                      && trimUse.EndsWith(";")
                              select trimUse.Substring(6, trimUse.Length - 7).Trim())
                         where !use.StartsWith("System")
                         select use;
            return genUse.ToList();
        }

        #endregion

        #region Parser States

        private class Nd { }

        private static readonly State<TemplateProcessor, Nd> State
                                    = new State<TemplateProcessor, Nd>(
            new[] {
                new Case<TemplateProcessor, Nd>(@"/\*CLASS#",
                    (p, n) => { p.Skip(); p.Class.Append(p.GetToStartOf("*/"));
                                p.Skip(); return State; }),
                new Case<TemplateProcessor, Nd>(@"/\*USING#",
                    (p, n) => { p.Skip(); p.Using.Append(p.GetToStartOf("*/"));
                                p.Skip(); return State; }),
                new Case<TemplateProcessor, Nd>(@"/\*#",
                    (p, n) => { p.Skip(); p.Active.Append(p.GetToStartOf("*/"));
                                p.Skip(); return State; }),
                new Case<TemplateProcessor, Nd>(@"//BEGIN\sCLASS#",
                    (p, n) => { p.Skip(); p.SkipToEndOfOrEnd('\n');
                                p.Active = p.Class; return State; },
                    (p, n, t) => t.TrimmedAtEnd(CharFun.IsSpaceOrTab)),
                new Case<TemplateProcessor, Nd>(@"//END\sCLASS#",
                    (p, n) => { p.Skip(); p.SkipToEndOfOrEnd('\n');
                                p.Active = p.Code; return State; },
                    (p, n, t) => t.TrimmedAtEnd(CharFun.IsSpaceOrTab)),
                new Case<TemplateProcessor, Nd>(@"//#",
                    (p, n) => { p.Skip();
                                p.Active.Append(p.GetToEndOfOrEnd('\n'));
                                return State; },
                    // indented //# comments eat the preceeding indentation
                    // by trimming it from the preceeding text:
                    (p, n, t) => t.TrimmedAtEnd(CharFun.IsSpaceOrTab)),
                new Case<TemplateProcessor, Nd>(@"__",
                    (p, n) => { p.Skip();
                                p.Active.Append(string.Format(
                                    "___sb.Append(({0}).ToString());",
                                    p.GetToStartOf("__")));
                                p.Skip(); return State; }),
            },
            (p, n, t) =>
            {
                if (!t.IsEmpty)
                {
                    p.Active.Append(
                        string.Format("___sb.Append(___Filter(@\"{0}\"));",
                                      t.ToString().Replace("\"", "\"\"")));
                }
            }
        );

        #endregion
    }
}
