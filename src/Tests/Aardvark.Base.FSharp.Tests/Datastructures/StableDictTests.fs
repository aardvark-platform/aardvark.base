namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base
open NUnit.Framework

module StableDictTests =

    type private Key(id : int, collide : bool) =
        member _.Id = id
        member _.Collide = collide
        override _.GetHashCode() = if collide then 0 else id
        override _.Equals(other) =
            match other with
            | :? Key as key -> id = key.Id && collide = key.Collide
            | _ -> false
        override _.ToString() = sprintf "Key(%d, collide=%b)" id collide

    let private assertState context (expected : (Key * int)[]) (dict : StableDict<Key, int>) =
        Assert.AreEqual(expected.Length, dict.Count, context + ": count")
        Assert.AreEqual(Array.tryHead expected, dict.First, context + ": first")
        Assert.AreEqual(Array.tryLast expected, dict.Last, context + ": last")
        use iterator = (dict :> seq<_>).GetEnumerator()
        for i in 0 .. expected.Length - 1 do
            let key, value = expected.[i]
            let message = sprintf "%s: index=%d, key=%O" context i key
            Assert.IsTrue(iterator.MoveNext(), message + ": missing entry")
            let storedKey, storedValue = iterator.Current
            Assert.AreSame(key, storedKey, message + ": stored key identity")
            Assert.AreEqual(value, storedValue, message + ": enumerated value")
            // Equal but distinct keys must find the same value and neighbours.
            let alias = Key(key.Id, key.Collide)
            let mutable found = -1
            Assert.IsTrue(dict.ContainsKey alias, message)
            Assert.IsTrue(dict.TryGetValue(alias, &found), message)
            Assert.AreEqual(value, found, message + ": lookup")
            let previous = if i = 0 then None else Some expected.[i - 1]
            let next = if i + 1 = expected.Length then None else Some expected.[i + 1]
            Assert.AreEqual(previous, dict.TryGetPrev alias, message + ": previous")
            Assert.AreEqual(next, dict.TryGetNext alias, message + ": next")
        // Never materialize the dictionary's sequence: the regression can create a cycle.
        Assert.IsFalse(iterator.MoveNext(), context + ": extra entry or cycle")
        let missing = Key(-999, false)
        let mutable missingValue = 0
        Assert.IsFalse(dict.ContainsKey missing, context + ": missing key")
        Assert.IsFalse(dict.TryGetValue(missing, &missingValue), context + ": missing lookup")
        Assert.AreEqual(None, dict.TryGetPrev missing, context + ": missing previous")
        Assert.AreEqual(None, dict.TryGetNext missing, context + ": missing next")

    [<Test>]
    let ``[StableDict] Same-key factories preserve the inserted value and order`` () =
        for nested in [false; true] do
            for collide in [false; true] do
                for prefixCount, beforeCount, afterCount in [(0, 0, 0); (2, 0, 0); (0, 2, 2); (3, 4, 5); (32, 16, 16)] do
                    let context = sprintf "nested=%b, collide=%b, prefix=%d, before=%d, after=%d" nested collide prefixCount beforeCount afterCount
                    let dict = StableDict<Key, int>()
                    let expected = ResizeArray<Key * int>()
                    let add id value =
                        let key = Key(id, collide)
                        Assert.IsTrue(dict.TryAdd(key, value), context)
                        expected.Add(key, value)
                    for i in 0 .. prefixCount - 1 do add i (100 + i)
                    let requested = Key(-1, collide)
                    let inserted = Key(-1, collide)
                    let mutable outerCalls = 0
                    let mutable innerCalls = 0
                    let result = dict.GetOrAdd(requested, fun key ->
                        outerCalls <- outerCalls + 1
                        Assert.AreSame(requested, key, context + ": factory key")
                        for i in 0 .. beforeCount - 1 do add (1000 + i) (200 + i)
                        if nested then
                            let value = dict.GetOrAdd(inserted, fun _ -> innerCalls <- innerCalls + 1; 17)
                            Assert.AreEqual(17, value, context)
                        else
                            Assert.IsTrue(dict.TryAdd(inserted, 17), context)
                        expected.Add(inserted, 17)
                        for i in 0 .. afterCount - 1 do add (2000 + i) (300 + i)
                        assertState (context + ": inside factory") (expected.ToArray()) dict
                        99
                    )
                    Assert.AreEqual(17, result, context)
                    Assert.AreEqual(1, outerCalls, context)
                    Assert.AreEqual((if nested then 1 else 0), innerCalls, context)
                    assertState context (expected.ToArray()) dict
                    Assert.AreEqual(17, dict.GetOrAdd(requested, fun _ -> Assert.Fail(context + ": hit invoked factory"); -1), context)
                    assertState (context + ": hit") (expected.ToArray()) dict

                    let mutable removed = 0
                    Assert.IsTrue(dict.TryRemove(requested, &removed), context)
                    Assert.AreEqual(17, removed, context)
                    let remaining = expected |> Seq.filter (fun (key, _) -> key.Id <> -1) |> Seq.toArray
                    assertState (context + ": removed") remaining dict
                    Assert.IsFalse(dict.Remove requested, context)
                    let replacement = Key(-1, collide)
                    Assert.AreEqual(23, dict.GetOrAdd(replacement, fun _ -> 23), context)
                    let readded = Array.append remaining [| replacement, 23 |]
                    assertState (context + ": re-added") readded dict
                    for key, _ in readded do Assert.IsTrue(dict.Remove key, context)
                    assertState (context + ": emptied") [||] dict

    [<Test>]
    let ``[StableDict] Ordinary hits and different-key factories retain insertion order`` () =
        for collide in [false; true] do
            for count in [0; 1; 32] do
                let context = sprintf "collide=%b, recursive insertions=%d" collide count
                let dict = StableDict<Key, int>()
                let key = Key(-1, collide)
                let expected = ResizeArray<Key * int>()
                let mutable calls = 0
                let value = dict.GetOrAdd(key, fun _ ->
                    calls <- calls + 1
                    for i in 0 .. count - 1 do
                        let other = Key(i, collide)
                        Assert.AreEqual(i + 10, dict.GetOrAdd(other, fun _ -> i + 10), context)
                        expected.Add(other, i + 10)
                    99
                )
                expected.Add(key, 99)
                Assert.AreEqual(99, value, context)
                Assert.AreEqual(1, calls, context)
                assertState context (expected.ToArray()) dict
                Assert.AreEqual(99, dict.GetOrAdd(Key(-1, collide), fun _ -> Assert.Fail(context + ": hit invoked factory"); 0), context)
                assertState (context + ": hit") (expected.ToArray()) dict

    [<Test>]
    let ``[StableDict] Insert-then-remove factories append only the surviving node`` () =
        for collide in [false; true] do
            for reinsert in [false; true] do
                let context = sprintf "collide=%b, reinsert=%b" collide reinsert
                let dict = StableDict<Key, int>()
                let prefix, middle, suffix = Key(0, collide), Key(1, collide), Key(2, collide)
                let requested, replacement = Key(-1, collide), Key(-1, collide)
                Assert.IsTrue(dict.TryAdd(prefix, 1), context)
                let result = dict.GetOrAdd(requested, fun key ->
                    Assert.IsTrue(dict.TryAdd(Key(key.Id, collide), 17), context)
                    Assert.IsTrue(dict.TryAdd(middle, 2), context)
                    let mutable removed = 0
                    Assert.IsTrue(dict.TryRemove(key, &removed), context)
                    Assert.AreEqual(17, removed, context)
                    if reinsert then Assert.IsTrue(dict.TryAdd(replacement, 18), context)
                    Assert.IsTrue(dict.TryAdd(suffix, 3), context)
                    99
                )
                let expected =
                    if reinsert then [| prefix, 1; middle, 2; replacement, 18; suffix, 3 |]
                    else [| prefix, 1; middle, 2; suffix, 3; requested, 99 |]
                Assert.AreEqual((if reinsert then 18 else 99), result, context)
                assertState context expected dict
                Assert.IsTrue(dict.Remove requested, context)
                assertState (context + ": removed") [| prefix, 1; middle, 2; suffix, 3 |] dict

    [<Test>]
    let ``[StableDict] Factory exceptions propagate without undoing callback effects`` () =
        for collide in [false; true] do
            for mutation in [0; 1; 2; 3] do
                let context = sprintf "collide=%b, mutation=%d" collide mutation
                let dict = StableDict<Key, int>()
                let prefix, requested, inserted, other = Key(0, collide), Key(-1, collide), Key(-1, collide), Key(1, collide)
                Assert.IsTrue(dict.TryAdd(prefix, 1), context)
                let expected = ResizeArray<Key * int>([| prefix, 1 |])
                let failure = InvalidOperationException("factory failure")
                let mutable calls = 0
                let error = Assert.Throws<InvalidOperationException>(fun () ->
                    dict.GetOrAdd(requested, fun _ ->
                        calls <- calls + 1
                        match mutation with
                        | 1 ->
                            Assert.IsTrue(dict.TryAdd(inserted, 17), context)
                            expected.Add(inserted, 17)
                        | 2 ->
                            Assert.AreEqual(23, dict.GetOrAdd(other, fun _ -> 23), context)
                            expected.Add(other, 23)
                        | 3 ->
                            Assert.IsTrue(dict.TryAdd(inserted, 17), context)
                            Assert.IsTrue(dict.Remove inserted, context)
                        | _ -> ()
                        raise failure
                    ) |> ignore
                )
                Assert.AreSame(failure, error, context)
                Assert.AreEqual(1, calls, context)
                assertState context (expected.ToArray()) dict
                let value = dict.GetOrAdd(requested, fun _ -> 99)
                if mutation = 1 then Assert.AreEqual(17, value, context)
                else
                    Assert.AreEqual(99, value, context)
                    expected.Add(requested, 99)
                assertState (context + ": recovery") (expected.ToArray()) dict
