namespace Aardvark.Base

open System

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Strings =
    let partRx = System.Text.RegularExpressions.Regex @"([A-Z][a-z0-9]*)[_]*"
    
    /// checks whether pattern is contained in str
    let contains pattern (str : string) = str.Contains pattern

    let toLower (str : string) = str.ToLower()
    let toUpper (str : string) = str.ToUpper()

    let inline split (sep : string) (str : string) = str.Split([| sep |], StringSplitOptions.None)
    let inline startsWith (s : string) (str : string) = str.StartsWith s
    let inline endsWith (s : string) (str : string) = str.EndsWith s
    let inline trim (str : string) = str.Trim()



