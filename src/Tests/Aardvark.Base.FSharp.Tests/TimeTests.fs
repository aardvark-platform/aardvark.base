namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open FsUnit
open NUnit.Framework

module ``Time tests`` =
    
    [<Test>]
    let ``indexing test``() =
        
        let r = Time.newRoot()
        r.[0] |> should equal r

        r |> Time.nth 1 |> should equal None
        r |> Time.nth -1 |> should equal None





