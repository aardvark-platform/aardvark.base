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
            let newcount = ref 0
            let newEntry (v : 'a) =
                isNew := true
                { value = v; count = 1 }

            let update (v : 'a) (e : Entry<'a>) =
                newcount := e.count + 1
                { e with count = e.count + 1}

            entries.AddOrUpdate(v, newEntry, update) |> ignore
            if !isNew then 
                queue.Enqueue v
                sem.Release() |> ignore
                true
            else 
                !newcount <> 0
        else false
                

    member x.Remove(v : 'a) =
        if isDisposed = 0 then
            let isNew = ref false
            let newcount = ref 0
            let newEntry (v : 'a) =
                isNew := true
                { value = v; count = -1 }

            let update (v : 'a) (e : Entry<'a>) =
                newcount := e.count - 1
                { e with count = e.count - 1}

            entries.AddOrUpdate(v, newEntry, update) |> ignore
            if !isNew then 
                queue.Enqueue v
                sem.Release() |> ignore
                true
            else 
                !newcount <> 0
        else false

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
    
    type AsyncReader<'a>(inner : IReader<'a>) =
        inherit ASetReaders.AbstractReader<'a>()

        let sem = new SemaphoreSlim(1)

        member x.GetDeltaAsync(caller : IAdaptiveObject) =
            async {
                do! Async.SwitchToThreadPool()
                let! _ = Async.AwaitIAsyncResult(sem.WaitAsync())
                let d = EvaluationUtilities.evaluateTopLevel (fun () -> x.GetDelta(caller))
                return d
            }

        override x.Mark() =
            sem.Release() |> ignore
            true

        override x.ComputeDelta() =
            inner.GetDelta(x)

        override x.Release() =
            sem.Dispose()
            inner.Dispose()

        override x.Inputs = Seq.singleton (inner :> IAdaptiveObject)

    let ofASet (s : aset<'a>) =
        let queue = new ConcurrentDeltaQueue<'a>()
         
        let reader = new AsyncReader<_>(s.GetReader())

        let pull =
            async {
                do! Async.SwitchToThreadPool()
                while true do
                    let! deltas = reader.GetDeltaAsync(null)
                    for d in deltas do queue.Enqueue d |> ignore
            }

        let cancel = new CancellationTokenSource()
        let task = Async.StartAsTask(pull, cancellationToken = cancel.Token)

        let disposable =
            { new IDisposable with
                member x.Dispose() =
                    cancel.Cancel()
                    reader.Dispose()
            }

        queue.Subscription <- disposable
        queue



