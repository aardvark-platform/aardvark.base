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


    type I = interface end
    type Trafo(t : ref<int>, child : I) =
        member x.T = t
        member x.Child = child
        interface I

    type Leaf() =
        interface I

    [<Semantic>]
    type SomeSem() =

        member x.Ts(y : Root<I>) =
            y.Child?Ts <- List.empty<ref<int>>

        member x.Ts(t : Trafo) =
            t.Child?Ts <- t.T :: t?Ts

        member x.T(y : I) : list<ref<int>> =
            y?Ts

        member x.T(t : Trafo) =
            ()

//        member x.T(leaf : Leaf) =
//            ()

        member x.Scope(l : Leaf) : Ag.Scope  = 
            Ag.getContext()

        member x.Scope(t : Trafo) : Ag.Scope =
            t.Child?Scope()
        
    [<Test>]
    let ``[Ag] same attrib inh and syn``() =
        Aardvark.Init()

        let t = Trafo(ref 1, Trafo (ref 2, Trafo (ref 3, Leaf ())))

        let s : Ag.Scope = t?Scope()

        let ob : obj = s?T()
        let theT : list<ref<int>> = unbox ob

        assert (theT :> obj <> null)
        ()



