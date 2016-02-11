namespace Aardvark.Base.Incremental

open System
open System.Threading
open System.Collections.Generic
open System.Collections.Concurrent
open Aardvark.Base

[<ReferenceEquality; NoComparison>]
type private Entry<'a> = { value : 'a; count : int }

type ConcurrentDeltaQueue<'a>() =
    let mutable isDisposed = 0
    let mutable queue = ConcurrentQueue<'a>()
    let mutable entries = ConcurrentDictionary<'a, Entry<'a>>()
    let sem = new SemaphoreSlim(0)
    let mutable subscription = { new IDisposable with member x.Dispose() = () }


    member internal x.Subscription
        with get() = subscription
        and set s = subscription <- s

    member x.Add(v : 'a) =
        if isDisposed = 0 then
            let isNew = ref false
            let newEntry (v : 'a) =
                isNew := true
                { value = v; count = 1 }

            let update (v : 'a) (e : Entry<'a>) =
                { e with count = e.count + 1}

            entries.AddOrUpdate(v, newEntry, update) |> ignore
            if !isNew then 
                queue.Enqueue v
                sem.Release() |> ignore

    member x.Remove(v : 'a) =
        if isDisposed = 0 then
            let isNew = ref false
            let newEntry (v : 'a) =
                isNew := true
                { value = v; count = -1 }

            let update (v : 'a) (e : Entry<'a>) =
                { e with count = e.count - 1}

            entries.AddOrUpdate(v, newEntry, update) |> ignore
            if !isNew then 
                queue.Enqueue v
                sem.Release() |> ignore

    member x.Enqueue(d : Delta<'a>) =
        match d with
            | Add v -> x.Add v
            | Rem v -> x.Remove v

    member x.Dequeue() =
        if isDisposed = 1 then
            raise <| System.ObjectDisposedException("ConcurrentDeltaQueue")

        sem.Wait()
        match queue.TryDequeue() with
            | (true, value) ->
                match entries.TryRemove(value) with
                    | (true, entry) ->
                        if entry.count > 0 then Add entry.value
                        elif entry.count < 0 then Rem entry.value
                        else x.Dequeue()
                    | _ ->
                        x.Dequeue()
            | _ ->
                failwith "queue empty but semaphore claims to have values"

    member x.DequeueAsync() =
        async {
            if isDisposed = 1 then
                raise <| System.ObjectDisposedException("ConcurrentDeltaQueue")

            let! _ = Async.AwaitIAsyncResult(sem.WaitAsync())
            match queue.TryDequeue() with
                | (true, value) ->
                    match entries.TryRemove(value) with
                        | (true, entry) ->
                            if entry.count > 0 then return Add entry.value
                            elif entry.count < 0 then return Rem entry.value
                            else return! x.DequeueAsync()
                        | _ ->
                            return! x.DequeueAsync()
                | _ ->
                    return failwith "queue empty but semaphore claims to have values"
        }

    member private x.Dispose(disposing : bool) =
        if Interlocked.Exchange(&isDisposed, 1) = 0 then
            subscription.Dispose()
            entries.Clear()
            sem.Dispose()
            queue <- null
            if disposing then System.GC.SuppressFinalize x

    member x.Dispose() = x.Dispose true

    interface IDisposable with
        member x.Dispose() = x.Dispose(true)

    override x.Finalize() =
        try x.Dispose false
        with _ -> ()


module ConcurrentDeltaQueue =
    
    let ofASet (s : aset<'a>) =
        let queue = new ConcurrentDeltaQueue<'a>()
         
        let subscription = 
            s |> ASet.unsafeRegisterCallbackKeepDisposable(fun deltas ->
                for d in deltas do queue.Enqueue d
            )

        queue.Subscription <- subscription
        queue



