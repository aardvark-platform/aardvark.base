namespace Aardvark.Base.FSharp.Tests
#nowarn "44"

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework
open Aardvark.Base.Ag

module ``Ag Tests`` =

    type F = interface end
    type FA() =
        interface F

    type G = interface end
    type GA() =
        interface G

    [<Semantic>]
    type FSem() =
        member x.SimpleInh(r : Root<F>) = r.Child?SimpleInh <- 1
        member x.SimpleSyn(a : FA) : int = a?SimpleInh
        member x.Syn(f : FA) : int = f?Inh
        member x.Inh(r : Root<F>) = r.Child?Inh <- 1

    [<Semantic>]
    type GSem() =
        member x.Syn(f : GA) : int  = f?Inh
        member x.Inh(r : Root<G>) = r.Child?Inh <- 0

    [<Test>]
    let ``[Ag] simple semantic root test``() =
        Aardvark.Init()

        let f = FA()

        let resultF = f?SimpleSyn()

        resultF |> should equal 1

    [<Test>]
    let ``[Ag] simple multiple syntactic families``() =
        Aardvark.Init()

        let f = FA()
        let g = GA()

        let resultF = f?Syn()
        let resultG = g?Syn()

        resultF |> should equal 1
        resultG |> should equal 0

