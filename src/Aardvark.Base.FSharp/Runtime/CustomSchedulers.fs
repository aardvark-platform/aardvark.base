namespace Aardvark.Base

open System;
open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent
open System.Linq
open System.Threading
open System.Threading.Tasks

type CustomTaskScheduler(threadCount : int, priority : ThreadPriority) as this =
    inherit TaskScheduler()

    let workItems = new BlockingCollection<Task>()

    let threads = 
        [| for i in 0 .. threadCount - 1 do
            let t = System.Threading.Thread(ThreadStart(fun () ->
                while true do 
                    let t = workItems.Take()
                    this.TryExecuteTask t |> ignore
            ))
            t.Priority <- ThreadPriority.Lowest
            t.IsBackground <- true
            t.Start()
            yield t
        |]

    let mutable isDisposed = false

    member x.TryExecuteTask t = base.TryExecuteTask t 
    override x.QueueTask t = workItems.Add t
    override x.TryExecuteTaskInline (t,p) = x.TryExecuteTask t

    override x.MaximumConcurrencyLevel = threadCount
    override x.GetScheduledTasks() = workItems.ToArray() :> _

    interface IDisposable with
        member x.Dispose() =
            if not isDisposed then
                isDisposed <- true
                workItems.CompleteAdding()
                for t in threads do t.Join()
                workItems.Dispose()

    new(threadCount) = new CustomTaskScheduler(threadCount,ThreadPriority.Normal)
    new() = new CustomTaskScheduler(System.Environment.ProcessorCount,ThreadPriority.Normal)