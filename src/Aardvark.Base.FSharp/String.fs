#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

open System.Linq.Expressions
open System.Runtime.InteropServices
open Microsoft.FSharp.Reflection
open System.Reflection
open System.Reflection.Emit
open System


[<AutoOpen>]
module ``String Extensions`` =
    module String =
        let private lineBreak = System.Text.RegularExpressions.Regex("\r\n")

        let indent (step : int) (s : string) =
            let parts = lineBreak.Split s
            let indent = System.String(' ', step * 4)
            let parts = parts |> Seq.map (fun l -> indent + l)
            System.String.Join("\r\n", parts)

        let lineCount (s : string) =
            let parts = lineBreak.Split s
            parts.Length