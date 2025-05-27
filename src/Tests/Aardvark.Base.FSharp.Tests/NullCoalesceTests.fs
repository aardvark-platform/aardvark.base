namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open System

open FsUnit
open NUnit.Framework

module CoalesceTests =

    [<AllowNullLiteral>]
    type Foo() = class end

    type Container<'T>(value: 'T) =
        let mutable getterInvoked = false

        member x.Value =
            getterInvoked <- true
            value

        member x.GetterInvoked =
            getterInvoked

    [<Test>]
    let ``[Null Coalescing] Object``() =
        let a: Foo = null
        let b: Foo = Foo()
        let c: Foo = Foo()
        let cnt = Container c
        let lzy = lazy c

        (b ||? cnt.get_Value) |> should equal b
        cnt.GetterInvoked |> should be False
        (a ||? cnt.get_Value) |> should equal c

        (b ||? lzy) |> should equal b
        lzy.IsValueCreated |> should be False
        (a ||? lzy.get_Value) |> should equal c

        (a ||? b) |> should equal b
        (b ||? a) |> should equal b

    [<Test>]
    let ``[Null Coalescing] Nullable``() =
        let a: Nullable<int> = Nullable()
        let b: int = 1
        let c: int = 2
        let cnt = Container c
        let lzy = lazy c

        (Nullable b ||? cnt.get_Value) |> should equal b
        cnt.GetterInvoked |> should be False
        (a ||? cnt.get_Value) |> should equal c

        (Nullable b ||? lzy) |> should equal b
        lzy.IsValueCreated |> should be False
        (a ||? lzy.get_Value) |> should equal c

        (a ||? b) |> should equal b
        (Nullable b ||? c) |> should equal b

    [<Test>]
    let ``[Null Coalescing] Option``() =
        let a: int option = None
        let b: int = 1
        let c: int = 2
        let cnt = Container c
        let lzy = lazy c

        (Some b ||? cnt.get_Value) |> should equal b
        cnt.GetterInvoked |> should be False
        (a ||? cnt.get_Value) |> should equal c

        (Some b ||? lzy) |> should equal b
        lzy.IsValueCreated |> should be False
        (a ||? lzy.get_Value) |> should equal c

        (a ||? b) |> should equal b
        (Some b ||? c) |> should equal b

    [<Test>]
    let ``[Null Coalescing] Value Option``() =
        let a: int voption = ValueNone
        let b: int = 1
        let c: int = 2
        let cnt = Container c
        let lzy = lazy c

        (ValueSome b ||? cnt.get_Value) |> should equal b
        cnt.GetterInvoked |> should be False
        (a ||? cnt.get_Value) |> should equal c

        (ValueSome b ||? lzy) |> should equal b
        lzy.IsValueCreated |> should be False
        (a ||? lzy.get_Value) |> should equal c

        (a ||? b) |> should equal b
        (ValueSome b ||? c) |> should equal b