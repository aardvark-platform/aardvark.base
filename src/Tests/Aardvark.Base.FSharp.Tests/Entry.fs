namespace Aardvark.Base.FSharp.Tests

module Main =

    [<EntryPoint>]
    let main args = 
        Aardvark.Base.FSharp.Tests.``Control tests``.``[Control] stateful step var cancellation test``()
        1