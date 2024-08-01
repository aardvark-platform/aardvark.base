open Expecto
open Expecto.Impl

[<EntryPoint>]
let main args =
    let cfg : ExpectoConfig = { ExpectoConfig.defaultConfig with runInParallel = false }
    //runTests cfg Aardvark.Base.Fonts.Tests.PathSegment.splitMerge |> ignore
    //runTestsWithCLIArgs [] args Aardvark.Rendering.Text.Tests.PathSegment.splitMerge |> ignore

    0
