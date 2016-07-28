module Program

open Aardvark.Base.Runtime.Tests

[<EntryPoint>]
let main args =
    DynamicCodeTests.``[DynamicCode] huge changeset``()
    0