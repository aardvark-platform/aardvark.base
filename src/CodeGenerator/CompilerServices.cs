using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGenerator;

#pragma warning disable IDE0060 // Remove unused parameter

public static class CompilerServices
{
    //public static string CompilerExecutablePath = @".\csc.exe";

    /// <summary>
    /// Returns compiled assembly from given source code lines.
    /// Specify all dlls that need to be referenced when compiling (/r).
    /// </summary>
    public static Assembly CompileAssembly(
        IEnumerable<string> sourceLines,
        IEnumerable<string> dllsToReference,
        string tmpDirectoryName,
        out string[] errors
        )
    {
        var sb = new StringBuilder();
        foreach(var line in sourceLines) sb.AppendLine(line);
        var syntaxTree = CSharpSyntaxTree.ParseText(sb.ToString());

        var dir = Path.GetDirectoryName(typeof(CompilerServices).Assembly.Location);

        var references =
            AppDomain.CurrentDomain.GetAssemblies()
                .Concat([typeof(System.Xml.Linq.XName).Assembly])
                .Distinct()
                .Select(a => {
                    if(String.IsNullOrWhiteSpace(a.Location)) return null;
                    try { return MetadataReference.CreateFromFile(a.Location); }
                    catch(Exception) { return null; }
                })
                .Where(r => r != null).ToArray();
        //
        // var references =
        //     dllsToReference
        //         .Select(dll => {
        //             if(File.Exists(Path.Combine(dir, dll))) return MetadataReference.CreateFromFile(dll);
        //             else {
        //                 try 
        //                 {
        //                     var aa = Assembly.Load(dll);
        //                     return MetadataReference.CreateFromFile(aa.Location);
        //                 }
        //                 catch(Exception) {
        //                     return null;
        //                 }   
        //             }
        //         })
        //         .Where(a => a != null)
        //         .ToArray();

        CSharpCompilation compilation = CSharpCompilation.Create(
            Guid.NewGuid().ToString(),
            [syntaxTree],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var dllStream = new MemoryStream();
        using var pdbStream = new MemoryStream();
        var emitResult = compilation.Emit(dllStream, pdbStream);
        if (emitResult.Success)
        {
            errors = [];
            return Assembly.Load(dllStream.ToArray());
        }
        else
        {
            errors = [.. emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Select(m => m.GetMessage())];
            return null;
        }
    }


}
