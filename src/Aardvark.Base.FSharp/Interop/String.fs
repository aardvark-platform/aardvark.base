namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Strings =

    /// checks whether pattern is contained in str
    let contains pattern (str : string) = str.Contains pattern

    let toLower (str : string) = str.ToLower()