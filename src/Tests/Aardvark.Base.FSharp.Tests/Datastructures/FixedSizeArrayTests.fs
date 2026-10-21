namespace Aardvark.Base.FSharp.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open NUnit.Framework

module FixedSizeArrayTests =

    // One-shot input with independently observable acquisition, advancement,
    // element access and cleanup. Failures retain their exact exception identity.
    type private Source<'a>(values : 'a[]) =
        let events = ResizeArray<string>()
        let failure = InvalidOperationException("source failure")
        let mutable acquired = false
        member val ThrowAt = "" with get, set
        member _.Events = events.ToArray()
        member _.Failure = failure
        member private x.Record(name) =
            events.Add name
            if x.ThrowAt = name then raise failure
        interface IEnumerable<'a> with
            member x.GetEnumerator() =
                x.Record "Acquire"
                if acquired then failwith "enumerated twice"
                acquired <- true
                let mutable index = -1
                { new IEnumerator<'a> with
                    member _.Current =
                        x.Record $"Current:{index}"
                        values.[index]
                  interface IEnumerator with
                    member _.Current = failwith "non-generic Current"
                    member _.MoveNext() =
                        x.Record $"MoveNext:{index + 1}"
                        index <- index + 1
                        index < values.Length
                    member _.Reset() = raise (NotSupportedException())
                  interface IDisposable with
                    member _.Dispose() = x.Record "Dispose" }
        interface IEnumerable with
            member x.GetEnumerator() = (x :> IEnumerable<'a>).GetEnumerator() :> IEnumerator

    let private create capacity (source : seq<'a>) =
        match capacity with
        | 0 -> Arr<N<0>, 'a>(source).Data
        | 1 -> Arr<N<1>, 'a>(source).Data
        | 4 -> Arr<N<4>, 'a>(source).Data
        | 32 -> Arr<N<32>, 'a>(source).Data
        | _ -> invalidArg "capacity" "unexpected capacity"

    [<Test>]
    let ``Prefix truncation and default padding across source kinds and capacities`` () =
        for capacity in [0; 1; 4; 32] do
            for count in [0; 1; 2; 4; 17; 32; 65] do
                let values = Array.init count (fun i -> i + 17)
                let expected = Array.init capacity (fun i -> if i < count then values.[i] else 0)
                let inputs = [values :> seq<_>; Array.toList values :> seq<_>; seq { yield! values }]
                for input in inputs do
                    Assert.That(create capacity input, Is.EqualTo(expected), $"capacity={capacity}, count={count}")
                let source = Source values
                Assert.That(create capacity source, Is.EqualTo(expected))

    [<Test>]
    let ``Reference elements retain identity and pad with null`` () =
        for capacity in [0; 1; 4; 32] do
            for count in [0; 1; 4; 32; 65] do
                let values = Array.init count (fun _ -> obj())
                for input in [values :> seq<_>; Array.toList values :> seq<_>; Source values :> seq<_>] do
                    let data = create capacity input
                    Assert.That(data.Length, Is.EqualTo(capacity))
                    for i in 0 .. capacity - 1 do
                        Assert.That(data.[i], Is.SameAs(if i < count then values.[i] else null))

    [<Test>]
    let ``Backing storage is independent of caller arrays`` () =
        let check (values : 'a[]) replacement =
            for capacity in [1; 4; 32] do
                let input = Array.copy values
                let original = Array.copy input
                let data = create capacity input
                Assert.That(data, Is.Not.SameAs(input))
                input.[0] <- replacement
                Assert.That(data.[0], Is.EqualTo(original.[0]))
                data.[0] <- replacement
                Assert.That(original.[0], Is.Not.EqualTo(replacement))
                if data.Length > 1 then
                    data.[1] <- replacement
                    Assert.That(input.[1], Is.EqualTo(original.[1]))
        check [| 13; 27; 39; 41; 53 |] -1
        check [| "a"; "b"; "c"; "d"; "e" |] "replacement"

    [<Test>]
    let ``Enumeration stops before the first discarded MoveNext`` () =
        for capacity in [0; 1; 4; 32] do
            for count in [0; 1; 4; 32; 65] do
                let source = Source(Array.init count id)
                create capacity source |> ignore
                let expected =
                    [| if capacity > 0 then
                           yield "Acquire"
                           for i in 0 .. min capacity count - 1 do
                               yield $"MoveNext:{i}"
                               yield $"Current:{i}"
                           if count < capacity then yield $"MoveNext:{count}"
                           yield "Dispose" |]
                Assert.That(source.Events, Is.EqualTo(expected), $"capacity={capacity}, count={count}")

    [<Test>]
    let ``Zero capacity does not acquire an enumerator`` () =
        let source = Source([| 1 |], ThrowAt = "Acquire")
        Assert.That(create 0 source, Is.Empty)
        Assert.That(source.Events, Is.Empty)

    [<Test>]
    let ``Guarded source only supplies the required prefix`` () =
        for capacity in [1; 4; 32] do
            let source = Source(Array.init (capacity + 1) (fun i -> i + 10), ThrowAt = $"MoveNext:{capacity}")
            Assert.That(create capacity source, Is.EqualTo(Array.init capacity (fun i -> i + 10)))
            Assert.That(source.Events.[source.Events.Length - 1], Is.EqualTo("Dispose"))

    [<TestCase("Acquire")>]
    [<TestCase("MoveNext:0")>]
    [<TestCase("MoveNext:2")>]
    [<TestCase("Current:0")>]
    [<TestCase("Current:2")>]
    [<TestCase("Dispose")>]
    let ``Source exceptions propagate and acquired enumerators are disposed`` (failureAt : string) =
        let source = Source([| 1; 2; 3; 4; 5 |], ThrowAt = failureAt)
        let actual = Assert.Throws<InvalidOperationException>(fun () -> create 4 source |> ignore)
        Assert.That(actual, Is.SameAs(source.Failure))
        Assert.That(source.Events |> Array.filter ((=) "Dispose") |> Array.length,
                    Is.EqualTo(if failureAt = "Acquire" then 0 else 1))
        if failureAt <> "Acquire" then Assert.That(Array.last source.Events, Is.EqualTo("Dispose"))

    [<TestCase(0)>]
    [<TestCase(1)>]
    [<TestCase(4)>]
    [<TestCase(32)>]
    let ``Null input preserves ArgumentNullException and parameter name`` capacity =
        let expected = Assert.Throws<ArgumentNullException>(fun () -> Seq.toArray (null : seq<int>) |> ignore)
        let actual = Assert.Throws<ArgumentNullException>(fun () -> create capacity (null : seq<int>) |> ignore)
        Assert.That(actual.ParamName, Is.EqualTo(expected.ParamName))

    [<Test>]
    let ``Convenience entry points share bounded copying and padding`` () =
        for count in [0; 2; 4; 9] do
            let values = Array.init count (fun i -> i + 1)
            let expected = Array.init 4 (fun i -> if i < count then values.[i] else 0)
            let viaList : Arr<N<4>, int> = Arr.ofList (Array.toList values)
            let viaListExtension = List.toFixed<N<4>, int> (Array.toList values)
            Assert.That(viaList.Data, Is.EqualTo(expected))
            Assert.That(viaListExtension.Data, Is.EqualTo(expected))
            let viaArray = Array.toFixed<N<4>, int> values
            Assert.That(viaArray.Data, Is.EqualTo(expected))
            Assert.That(viaArray.Data, Is.Not.SameAs(values))
            for construct in [ (fun s -> (Arr.ofSeq s : Arr<N<4>, int>));
                               Seq.toFixed<N<4>, int>; Array.toFixed<N<4>, int> ] do
                let source = Source(values, ThrowAt = "MoveNext:4")
                Assert.That((construct source).Data, Is.EqualTo(expected))
                Assert.That(Array.last source.Events, Is.EqualTo("Dispose"))
        for construct in [ (fun s -> (Arr.ofSeq s : Arr<N<0>, int>));
                           Seq.toFixed<N<0>, int>; Array.toFixed<N<0>, int> ] do
            let source = Source([| 1 |], ThrowAt = "Acquire")
            Assert.That((construct source).Length, Is.Zero)
            Assert.That(source.Events, Is.Empty)

    [<Test>]
    let ``Public constructor properties and reflection patterns retain their shape`` () =
        let t = typeof<Arr<N<4>, int>>
        Assert.That(t.GetConstructor([| typeof<seq<int>> |]), Is.Not.Null)
        Assert.That(t.GetConstructor(Type.EmptyTypes), Is.Not.Null)
        Assert.That(t.GetProperty("Data").PropertyType, Is.EqualTo(typeof<int[]>))
        Assert.That(t.GetProperty("Length").PropertyType, Is.EqualTo(typeof<int>))
        let value = Arr<N<4>, int>()
        Assert.That(value.Data, Is.EqualTo([| 0; 0; 0; 0 |]))
        value.[1] <- 42
        Assert.That(value.[1], Is.EqualTo(42))
        Assert.That(value :> seq<int> |> Seq.toArray, Is.EqualTo([| 0; 42; 0; 0 |]))
        match t with
        | FixedArrayType(4, element) -> Assert.That(element, Is.EqualTo(typeof<int>))
        | _ -> Assert.Fail("FixedArrayType no longer recognizes Arr")
        match box value with
        | FixedArray(element, data) ->
            Assert.That(element, Is.EqualTo(typeof<int>))
            Assert.That(data, Is.EqualTo([| box 0; box 42; box 0; box 0 |]))
        | _ -> Assert.Fail("FixedArray no longer recognizes Arr")

    [<Test>]
    let ``Allocation is independent of discarded source length`` () =
        let measure (source : seq<int>) =
            for _ in 1 .. 256 do GC.KeepAlive(Arr<N<4>, int>(source))
            let start = GC.GetAllocatedBytesForCurrentThread()
            for _ in 1 .. 64 do GC.KeepAlive(Arr<N<4>, int>(source))
            GC.GetAllocatedBytesForCurrentThread() - start
        for wrap in [ (fun (a : int[]) -> a :> seq<int>);
                      (fun a -> Array.toList a :> seq<int>);
                      (fun a -> seq { yield! a }) ] do
            let short = Array.init 4 id |> wrap
            let oversized = Array.init 4096 id |> wrap
            Assert.That(measure oversized, Is.EqualTo(measure short))
