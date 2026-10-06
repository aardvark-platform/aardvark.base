namespace Aardvark.Base.FSharp.Tests

open System
open System.Collections.Generic
open Aardvark.Base
open NUnit.Framework

module SkipListTests =

    let private removeAt viaModule index (list : SkipList<int>) =
        if viaModule then SkipList.removeAt index list else list.RemoveAt index

    let private assertContents context (expected : int[]) (list : SkipList<int>) =
        Assert.AreEqual(expected.Length, list.Count, context + ": Count")
        Assert.AreEqual(expected.Length, SkipList.count list, context + ": module Count")
        CollectionAssert.AreEqual(expected, Seq.toArray list, context + ": enumeration")
        for index in 0 .. expected.Length - 1 do
            Assert.AreEqual(expected.[index], list.[index], sprintf "%s: Item[%d]" context index)
            Assert.AreEqual(Some expected.[index], list.TryAt index, sprintf "%s: TryAt[%d]" context index)
            Assert.AreEqual(Some expected.[index], SkipList.at index list, sprintf "%s: module at[%d]" context index)

    let private populated cmp size =
        let list = SkipList<int>(cmp)
        for i in size - 1 .. -1 .. 0 do
            Assert.IsTrue(list.Add (3 * i - size), sprintf "populate size=%d, i=%d" size i)
        list

    [<Test>]
    let ``[SkipList] Rejected indices preserve populated and emptied lists`` () =
        for descending in [false; true] do
            let cmp a b = if descending then compare b a else compare a b
            for viaModule in [false; true] do
                for size in [0; 1; 2; 17; 64] do
                    for state in ["populated"; "removed"; "cleared"; "reused after removal"; "reused after clear"] do
                        let list = populated cmp size
                        if state.Contains "removal" || state = "removed" then
                            while list.Count > 0 do Assert.IsTrue(removeAt viaModule (list.Count / 2) list)
                        elif state.Contains "clear" then
                            SkipList.clear list
                        if state.StartsWith "reused" then Assert.IsTrue(list.Add 123)
                        let expected = Seq.toArray list
                        for index in [Int32.MinValue; -7; -1; list.Count; list.Count + 1; list.Count + 97; Int32.MaxValue] do
                            let context = sprintf "descending=%b, module=%b, size=%d, state=%s, index=%d" descending viaModule size state index
                            Assert.IsFalse(removeAt viaModule index list, context)
                            assertContents context expected list

    [<Test>]
    let ``[SkipList] Valid removals retain ordering and indexed contents`` () =
        for descending in [false; true] do
            let cmp a b = if descending then compare b a else compare a b
            for viaModule in [false; true] do
                for size in [1; 2; 17; 257] do
                    for first in [0; size / 2; size - 1] do
                        let list = populated cmp size
                        let model = ResizeArray<int>([| for i in 0 .. size - 1 -> 3 * i - size |] |> Array.sortWith cmp)
                        let mutable index = first
                        let mutable step = 0
                        while model.Count > 0 do
                            let context = sprintf "descending=%b, module=%b, size=%d, first=%d, step=%d, index=%d" descending viaModule size first step index
                            Assert.IsTrue(removeAt viaModule index list, context)
                            model.RemoveAt index
                            assertContents context (model.ToArray()) list
                            step <- step + 1
                            index <- if model.Count = 0 then 0 elif step % 3 = 0 then 0 elif step % 3 = 1 then model.Count / 2 else model.Count - 1
                        Assert.IsFalse(removeAt viaModule 0 list)
                        Assert.IsFalse(removeAt viaModule -1 list)
                        assertContents "removed to empty" [||] list

    [<Test>]
    let ``[SkipList] Interleaved operations agree with a sorted list model`` () =
        for descending in [false; true] do
            let cmp a b = if descending then compare b a else compare a b
            let comparer = Comparer<int>.Create(Comparison<int>(cmp))
            for viaModule in [false; true] do
                for seed in [52719; 8609] do
                    let random = Random(seed)
                    let list = SkipList<int>(cmp)
                    let model = ResizeArray<int>()
                    for step in 0 .. 1999 do
                        let context = sprintf "descending=%b, module=%b, seed=%d, step=%d, count=%d" descending viaModule seed step model.Count
                        match random.Next 10 with
                        | 0 ->
                            list.Clear()
                            model.Clear()
                        | 1 | 2 | 3 | 4 ->
                            let value = random.Next(-128, 129)
                            let index = model.BinarySearch(value, comparer)
                            let expected = index < 0
                            Assert.AreEqual(expected, list.Add value, sprintf "%s, add=%d" context value)
                            if expected then model.Insert(~~~index, value)
                        | 5 | 6 when model.Count > 0 ->
                            let index = random.Next(model.Count)
                            Assert.IsTrue(removeAt viaModule index list, sprintf "%s, valid index=%d" context index)
                            model.RemoveAt index
                        | _ ->
                            let indices = [| Int32.MinValue; -1; -17; model.Count; model.Count + 13; Int32.MaxValue |]
                            let index = indices.[random.Next indices.Length]
                            Assert.IsFalse(removeAt viaModule index list, sprintf "%s, invalid index=%d" context index)
                        assertContents context (model.ToArray()) list
