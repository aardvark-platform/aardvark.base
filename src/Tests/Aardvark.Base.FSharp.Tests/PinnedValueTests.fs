namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open FSharp.NativeInterop

open FsUnit
open NUnit.Framework

#nowarn "9"

module PinnedValueTests =

    [<Test>]
    let ``[PinnedValue] Single value``() =
        use value = new PinnedValue<_>(42UL)

        value.Length |> should equal 1
        value.Pointer.[0] |> should equal 42UL

        NativePtr.write value.Pointer 43UL
        value.Pointer.[0] |> should equal 43UL

    [<Test>]
    let ``[PinnedValue] Array``() =
        let array = [| 42UL; 43UL; 44UL |]
        use value = new PinnedValue<_>(array)

        value.Length |> should equal 3
        value.Pointer |> NativePtr.toArray value.Length |> should equal array

        NativePtr.set value.Pointer 2 256UL
        array.[2] |> should equal 256UL

    [<Test>]
    let ``[PinnedValue] Array as obj``() =
        let array = [| 42UL; 43UL; 44UL |]
        use value = new PinnedValue(array :> obj)

        value.Length |> should equal 3
        value.Address |> NativePtr.ofNativeInt<uint64> |> NativePtr.toArray value.Length |> should equal array