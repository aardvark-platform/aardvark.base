namespace Aardvark.Base.Incremental

open Aardvark.Base
open System.Collections.Generic

type PriorityQueue<'a>(cmp : 'a -> 'a -> int) =
    let store = List<'a>()
    let cmpFun = System.Func<'a,'a, int>(cmp)

    member x.Enqueue (v : 'a) =
        store.HeapEnqueue(cmpFun, v)

    member x.Dequeue() =
        store.HeapDequeue(cmpFun)

    member x.Count =
        store.Count

    member x.Min = store.[0]

type DuplicatePriorityQueue<'a, 'k when 'k : comparison>(extract : 'a -> 'k) =
    let q = PriorityQueue<'k> compare
    let values = Dictionary<'k, Queue<'a>>()
    let mutable count = 0

    member x.Enqueue(v : 'a) =
        let k = extract v
        count <- count + 1

        match values.TryGetValue k with
            | (true, q) -> q.Enqueue v
            | _ -> 
                let inner = Queue<'a>()
                inner.Enqueue v
                values.[k] <- inner
                q.Enqueue k
                
    member x.Dequeue() =
        let k = q.Min

        match values.TryGetValue k with
            | (true, inner) ->
                let res = inner.Dequeue()
                count <- count - 1
                if inner.Count = 0 then
                    q.Dequeue() |> ignore
                    values.Remove k |> ignore
                k, res
            | _ ->
                failwith "inconsistent state in DuplicatePriorityQueue"

    member x.Count =
        count

