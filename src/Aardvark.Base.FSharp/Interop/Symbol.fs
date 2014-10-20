namespace Aardvark.Base

module Sym =

    let empty = Symbol.Empty

    let newGuid() = Symbol.CreateNewGuid()

    let ofString (str : string) = Symbol.Create str

    let toString (sym : Symbol) = sym.ToString()

    let toGuid (sym : Symbol) = sym.ToGuid()

    let id (sym : Symbol) = sym.Id

    let isEmpty (sym : Symbol) = sym.IsEmpty

    let isNotEmpty (sym : Symbol) = sym.IsNotEmpty

    let isPositive (sym : Symbol) = sym.IsPositive

    let isNegative (sym : Symbol) = sym.IsNegative