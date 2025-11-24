module Aardvark.Base.Fonts.Tests.Loading

open Aardvark.Base
open Aardvark.Base.Fonts
open System.Runtime.InteropServices
open Expecto

[<Tests>]
let tests =
    testList "Fonts.Loading" [
        test "Embedded Symbola" {
            let font = Font.Symbola
            Expect.equal font.Family "Symbola" "Unexpected family name"
        }

        test "Resolve" {
            let os = Aardvark.GetOSPlatform()
            if os <> OSPlatform.Windows && os <> OSPlatform.OSX then
                skiptest "Font resolver only works for Windows and macOS"

            let font = Font("Arial") // Finds font with family name most similar to Arial
            Expect.isNotNull font.Family "Family name is null"
            Expect.notEqual font.Family "" "Family name is empty"
        }
    ]