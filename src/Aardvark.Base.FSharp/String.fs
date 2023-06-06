#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices

#if !NET6_0_OR_GREATER
[<Sealed; Extension>]
type StringExtensions private() =

    /// Replaces all newline sequences in the current string with replacementText.
    [<Extension>]
    static member inline ReplaceLineEndings(str : string, replacementText : string) =
        Regex.Replace(str, @"\r\n?|\n", replacementText)

    /// Replaces all newline sequences in the current string with Environment.NewLine.
    [<Extension>]
    static member inline ReplaceLineEndings(str : string) =
        str.ReplaceLineEndings(Environment.NewLine)
#endif

[<AutoOpen>]
module ``String Extensions`` =
    module String =

        /// Splits the string into individual lines, recognizing various line ending sequences.
        let inline getLines (str : string) =
            str.ReplaceLineEndings().Split([| Environment.NewLine |], StringSplitOptions.None)

        /// Replaces all newline sequences in the given string with Environment.NewLine.
        let inline normalizeLineEndings (str : string) =
            str.ReplaceLineEndings()

        /// Adds line number prefixes to the given string.
        /// Line endings are normalized.
        let withLineNumbers (str : string) =
            let lines = getLines str
            let lineColumns = 1 + int (Fun.Log10 lines.Length)

            lines |> Array.mapi (fun i str ->
                let n = (string (i + 1)).PadLeft lineColumns
                $"{n}: {str}"
            )
            |> String.concat Environment.NewLine

        /// Indents each line of string by step * 4 spaces.
        /// Line endings are normalized.
        let indent (step : int) (s : string) =
            let lines = getLines s
            let indent = System.String(' ', step * 4)

            lines
            |> Array.map (fun l -> indent + l)
            |> String.concat Environment.NewLine

        /// Gets the number of lines in the given string, recognizing various line ending sequences.
        let lineCount (s : string) =
            let lines = getLines s
            lines.Length