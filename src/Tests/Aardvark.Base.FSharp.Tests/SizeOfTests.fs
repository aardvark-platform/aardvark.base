namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open System.Runtime.InteropServices

open FsUnit
open NUnit.Framework

module SizeOfTests =

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type MyStruct =
        val A : bool
        val B : bool
        val C : char

    [<Test>]
    let ``[GetCLRSize] Primitive types``() =
        typeof<int64>.GetCLRSize() |> should equal 8
        typeof<bool>.GetCLRSize() |> should equal 1
        typeof<char>.GetCLRSize() |> should equal 2

    [<Test>]
    let ``[GetCLRSize] Struct``() =
        typeof<MyStruct>.GetCLRSize() |> should equal 4
