namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base

open NUnit.Framework
open FsUnit

module EnumTests =

    type EnumUI8 =
        | Zero = 0uy
        | Two = 2uy

    type EnumI32 =
        | Two = 2
        | Three = 3

    type UnexpectedPassException() =
        inherit Exception()

    let inline shouldFailWith<'TExn> (f: unit -> unit) =
        try
            f() |> ignore
            raise <| UnexpectedPassException()
        with
        | :? UnexpectedPassException -> failwith $"Expected exception of type {typeof<'TExn>} but got none."
        | exn when exn.GetType() <> typeof<'TExn> -> failwith $"Expected exception of type {typeof<'TExn>} but got: {exn}"
        | _ -> ()

    [<Test>]
    let ``[Enum] convert`` () =
        Enum.convert<_, EnumI32> 3 |> should equal EnumI32.Three
        Enum.convert<_, EnumI32> 3uy |> should equal EnumI32.Three
        Enum.convert<_, EnumI32> 3L |> should equal EnumI32.Three
        Enum.convert<_, EnumI32> EnumI32.Three |> should equal EnumI32.Three
        Enum.convert<_, EnumI32> EnumUI8.Two |> should equal EnumI32.Two
        Enum.convert<_, EnumI32> 3.14 |> should equal EnumI32.Three
        Enum.convert<_, EnumI32> "3" |> should equal EnumI32.Three

        shouldFailWith<InvalidCastException> (fun _ -> Enum.convert<_, EnumI32> EnumUI8.Zero |> ignore)
        shouldFailWith<FormatException> (fun _ -> Enum.convert<_, EnumI32> "Foo" |> ignore)