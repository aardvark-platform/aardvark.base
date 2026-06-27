namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open FsUnit
open NUnit.Framework

module ConcurrentHashQueueTests =

    [<Test>]
    let ``[ConcurrentHashQueue] Dequeue single item`` () =
        let q = ConcurrentHashQueue<int>()

        q.Enqueue 1 |> should equal true

        q.Dequeue() |> should equal 1
        q.Count |> should equal 0

    [<Test>]
    let ``[ConcurrentHashQueue] Dequeue preserves FIFO order`` () =
        let q = ConcurrentHashQueue<int>()

        q.Enqueue 1 |> should equal true
        q.Enqueue 2 |> should equal true
        q.Enqueue 3 |> should equal true

        q.Dequeue() |> should equal 1
        q.Dequeue() |> should equal 2
        q.Dequeue() |> should equal 3
        q.Count |> should equal 0

    [<Test>]
    let ``[ConcurrentHashQueue] TryDequeue single item`` () =
        let q = ConcurrentHashQueue<int>()
        let mutable value = 0

        q.Enqueue 42 |> should equal true

        q.TryDequeue(&value) |> should equal true
        value |> should equal 42
        q.Count |> should equal 0

    [<Test>]
    let ``[ConcurrentHashQueue] Re-enqueue existing item moves it to back`` () =
        let q = ConcurrentHashQueue<int>()

        q.Enqueue 1 |> should equal true
        q.Enqueue 2 |> should equal true
        q.Enqueue 1 |> should equal false

        q.Count |> should equal 2
        q.Dequeue() |> should equal 2
        q.Dequeue() |> should equal 1
        q.Count |> should equal 0
