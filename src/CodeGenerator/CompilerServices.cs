using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace CodeGenerator
{

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
            out CompilerResults compilerResults
            )
        {
            var newAssemblyName = "Aardvark." + s_uid + ".dll";
            s_uid++;

#if NEVERMORE
            string sourceFileName =
                Path.Combine(tmpDirectoryName, "Aardvark." + s_uid + ".cs");
            string assemblyFileName =
                Path.Combine(tmpDirectoryName, newAssemblyName);
            s_uid++;

            // create tmp directory if necessary
            if (!Directory.Exists(tmpDirectoryName))
            {
                Directory.CreateDirectory(tmpDirectoryName);
            }

            // create source code file
            File.WriteAllLines(sourceFileName, sourceLines.ToArray());

            // 
            if (File.Exists(assemblyFileName)) File.Delete(assemblyFileName);

            string assemblyReferences =
                @"/r:System.Core.dll " +
                @"/r:System.Data.Linq.dll " +
                Environment.NewLine
                ;
            if (dllsToReference != null)
            {
                foreach (string s in dllsToReference)
                {
                    assemblyReferences += @"/r:" + s + Environment.NewLine;
                }
            }

            if (!File.Exists(CompilerExecutablePath))
            {
                throw new FileNotFoundException(CompilerExecutablePath);
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = tmpDirectoryName;
            psi.FileName = CompilerExecutablePath;
            psi.Arguments =
                assemblyReferences +
                "/r:System.Windows.Forms.dll " +
                "/target:library " +
                "\"/out:" + assemblyFileName + "\" " +
                "\"" + sourceFileName + "\""
                ;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;

            Console.WriteLine("csc.exe " + psi.Arguments);

            Process p = Process.Start(psi);
            p.WaitForExit();

            File.Delete(sourceFileName);

            if (File.Exists(assemblyFileName))
            {
                return Assembly.LoadFile(assemblyFileName);
            }
            else
            {
                Console.WriteLine(p.StandardOutput.ReadToEnd());
                Console.WriteLine(p.StandardError.ReadToEnd());
                return null;
            }
#endif

            var providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v4.0");

            CodeDomProvider compiler = new CSharpCodeProvider(providerOptions);
            CompilerParameters cp = new CompilerParameters();

            if (dllsToReference != null)
            {
                cp = new CompilerParameters(dllsToReference.ToArray());
            }
            cp.GenerateInMemory = true;
            cp.WarningLevel = 3;
            cp.OutputAssembly = newAssemblyName;
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                bool skip = false;
                foreach (var s in cp.ReferencedAssemblies)
                {
                    if (Path.GetFileName(s) == Path.GetFileName(asm.Location))
                    {
                        skip = true;
                        continue;
                    }
                }
                if (skip) continue;
                cp.ReferencedAssemblies.Add(asm.Location);
            }

            try
            {
                CompilerResults cr = compiler.CompileAssemblyFromSource(
                    cp, sourceLines.ToArray()
                    );

                compilerResults = cr;
                if (compilerResults.Errors.Count > 0) return null;

                return cr.CompiledAssembly;
            }
            finally
            {
                if (File.Exists(newAssemblyName)) File.Delete(newAssemblyName);
            }
        }

        private static int s_uid = 0;

    }

}
