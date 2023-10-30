#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices

[<Obsolete("Use String module instead.")>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Strings =
    let partRx = Regex @"([A-Z][a-z0-9]*)[_]*"

    /// checks whether pattern is contained in str
    let contains pattern (str : string) = str.Contains pattern

    let toLower (str : string) = str.ToLower()
    let toUpper (str : string) = str.ToUpper()

    let inline split (sep : string) (str : string) = str.Split([| sep |], StringSplitOptions.None)
    let inline startsWith (s : string) (str : string) = str.StartsWith s
    let inline endsWith (s : string) (str : string) = str.EndsWith s
    let inline trim (str : string) = str.Trim()

[<Sealed; Extension>]
type StringExtensions private() =

    /// Replaces all newline sequences in the current string with replacementText.
    [<Extension>]
    static member inline NormalizeLineEndings(str : string, replacementText : string) =
        #if NET6_0_OR_GREATER
        str.ReplaceLineEndings(replacementText)
        #else
        Regex.Replace(str, @"\r\n?|\n", replacementText)
        #endif

    /// Replaces all newline sequences in the current string with Environment.NewLine.
    [<Extension>]
    static member inline NormalizeLineEndings(str : string) =
        #if NET6_0_OR_GREATER
        str.ReplaceLineEndings()
        #else
        str.NormalizeLineEndings(Environment.NewLine)
        #endif

[<AutoOpen>]
module ``String Extensions`` =
    module String =

        /// Splits the string into individual lines, recognizing various line ending sequences.
        let inline getLines (str : string) =
            str.NormalizeLineEndings().Split([| Environment.NewLine |], StringSplitOptions.None)

        /// Replaces all newline sequences in the given string with Environment.NewLine.
        let inline normalizeLineEndings (str : string) =
            str.NormalizeLineEndings()

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

        /// Returns whether the string contains the given pattern.
        let inline contains (pattern : string) (str : string) =
            str.Contains pattern

        /// Returns a copy of the string converted to lowercase.
        let inline toLower (str : string) =
            str.ToLower()

        /// Returns a copy of the string converted to uppercase.
        let inline toUpper (str : string) =
            str.ToUpper()

        /// Splits the string into substring based on the separator.
        let inline split (separator : string) (str : string) =
            str.Split([| separator |], StringSplitOptions.None)

        /// Returns whether the string starts with the given pattern.
        let inline startsWith (pattern : string) (str : string) =
            str.StartsWith pattern

        /// Returns whether the string ends with the given pattern.
        let inline endsWith (pattern : string) (str : string) =
            str.EndsWith pattern

        /// Removes all leading and trailing white-space characters from the string.
        let inline trim (str : string) =
            str.Trim()

        /// Replaces all occurrences of the pattern with the replacement string.
        let inline replace (pattern : string) (replacement : string) (str : string) =
            str.Replace(pattern, replacement)