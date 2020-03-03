namespace Aardvark.Base.FSharp.Tests
#nowarn "44"

open System
open System.Reflection
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

    [<Rule>]
    type FSem() =
        member x.SimpleInh(r : Root<F>, scope : Scope) = 
            r.Child?SimpleInh <- 1

        member x.SimpleSyn(a : FA, scope : Scope) : int = 
            scope?SimpleInh
        member x.Syn(f : FA, scope : Scope) : int = 
            scope?Inh
        member x.Inh(r : Root<F>, scope : Scope) =     
            r.Child?Inh <- 1

    [<Rule>]
    type GSem() =
        member x.Syn(f : GA, scope : Scope) : int  = 
            scope?Inh

        member x.Inh(r : Root<G>, scope : Scope) = 
            r.Child?Inh <- 0

    [<Test>]
    let ``[Ag] simple semantic root test``() =
        Introspection.RegisterAssembly(typeof<F>.Assembly)
        Aardvark.Init()

        let f = FA()

        let resultF = f?SimpleSyn(Scope.Root)

        resultF |> should equal 1

    [<Test>]
    let ``[Ag] simple multiple syntactic families``() =
        Introspection.RegisterAssembly(typeof<F>.Assembly)
        Aardvark.Init()

        let f = FA()
        let g = GA()

        let resultF = f?Syn(Scope.Root)
        let resultG = g?Syn(Scope.Root)

        resultF |> should equal 1
        resultG |> should equal 0


    type I = interface end
    type Trafo(t : ref<int>, child : I) =
        member x.T = t
        member x.Child = child
        interface I

    type Leaf() =
        interface I

    [<Rule>]
    type SomeSem() =

        member x.Ts(y : Root<I>, scope : Scope) =
            y.Child?Ts <- List.empty<ref<int>>

        member x.Ts(t : Trafo, scope : Scope) =
            t.Child?Ts <- t.T :: scope?Ts

        member x.T(y : I, scope : Scope) : list<ref<int>> =
            scope?Ts

        member x.T(t : Trafo, scope : Scope) =
            obj()

//        member x.T(leaf : Leaf) =
//            ()

        member x.Scope(l : Leaf, scope : Scope) : Ag.Scope  = 
            scope

        member x.Scope(t : Trafo, scope : Scope) : Ag.Scope =
            t.Child?Scope(scope)
        
    [<Test>]
    let ``[Ag] same attrib inh and syn``() =
        Introspection.RegisterAssembly(typeof<F>.Assembly)
        Aardvark.Init()

        let t = Trafo(ref 1, Trafo (ref 2, Trafo (ref 3, Leaf ())))

        let s : Ag.Scope = t?Scope(Scope.Root)

        let ob : obj = s.Node.GetSynthesized("T", s)
        let theT : list<ref<int>> = unbox ob

        assert (theT :> obj <> null)
        ()



