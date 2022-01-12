module Program

open Aardvark.Geometry.Tests

[<EntryPoint>]
let main args = 
    FsCheck.Arb.register<ImageLoadingTests.Generators>() |> ignore
    let r = FsCheck.Check.Quick ImageLoadingTests.saveLoadTest
    
    //TexturePackingTests.run args
    0