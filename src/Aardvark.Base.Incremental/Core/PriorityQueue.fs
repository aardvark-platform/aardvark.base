namespace Aardvark.Base.Incremental

open Aardvark.Base
open System.Collections.Generic

/// <summary>
/// represents a simple priority queue using user-given compare function
/// </summary>
type PriorityQueue<'a>(cmp : 'a -> 'a -> int) =

    // we simply use the Aardvark.Base implementation here.
    // to ensure that all compare-functions used are identical
    // we wrap the base implementation in PriorityQueue.
    let store = List<'a>()
    let cmpFun = System.Func<'a,'a, int>(cmp)

    /// <summary>
    /// enqueues a new element
    /// </summary>
    member x.Enqueue (v : 'a) =
        store.HeapEnqueue(cmpFun, v)

    /// <summary>
    /// dequeues the min element from the queue and
    /// fails if the queue is empty
    /// </summary>
    member x.Dequeue() =
        store.HeapDequeue(cmpFun)

    /// <summary>
    /// returns the number of elements currently contained in the queue
    /// </summary>
    member x.Count =
        store.Count

    /// <summary>
    /// returns the current minimal value (according to cmp) contained
    /// and fails if the queue is empty.
    /// </summary>
    member x.Min = store.[0]

/// <summary>
/// implements a queue with "uncomparable" duplicates. 
/// This is helpful since regular heap implementation cannot
/// deal with a large number of duplicated keys efficiently.
/// Note: the duplicated values will be returned in the order they were enqueued
/// </summary>
type DuplicatePriorityQueue<'a, 'k when 'k : comparison>(extract : 'a -> 'k) =
    let q = PriorityQueue<'k> compare
    let values = Dictionary<'k, Queue<'a>>()
    let mutable count = 0

    /// <summary>
    /// enqueues a new element
    /// </summary>
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
             
    /// <summary>
    /// dequeues the current minimal value (and its key)
    /// </summary>   
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

    /// <summary>
    /// returns the number of elements currently contained in the queue
    /// </summary>
    member x.Count =
        count

