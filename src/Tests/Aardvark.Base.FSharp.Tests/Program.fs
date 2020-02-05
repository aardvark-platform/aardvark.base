

module Program

    open Aardvark.Base.FSharp.Tests

    [<EntryPoint>]
    let main argv = 

        //Caches.``[Caches] BinaryCache partial backward with ResultObject``()
        //Caches.``[Caches] BinaryCache forward``()
        //Caches.``[Caches] BinaryCache forward with ResultObject``()
        //SVDTests.``[SVD] DecomposeWithArrayWeirdTranspose``()
        try
            SVDTests.``[SVD] DecomposeWithNative``()
        with 
            | :? System.Exception as e -> 
                sprintf "%s" (e.ToString())
                ()

        0