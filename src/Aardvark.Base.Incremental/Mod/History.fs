namespace Aardvark.Base.Incremental

open System
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base
open System.Threading
open System.Collections
open System.Collections.Generic


type IOpReader<'ops> =
    inherit IAdaptiveObject
    inherit IDisposable
    abstract member GetOperations : AdaptiveToken -> 'ops

type IOpReader<'s, 'ops> =
    inherit IOpReader<'ops>
    abstract member State : 's

[<AbstractClass>]
type AbstractReader<'ops>(scope : Ag.Scope, t : Monoid<'ops>) =
    inherit AdaptiveObject()

    abstract member Release : unit -> unit
    abstract member Compute : AdaptiveToken -> 'ops

    abstract member Apply : 'ops -> 'ops
    default x.Apply o = o

    member x.GetOperations(token : AdaptiveToken) =
        x.EvaluateAlways token (fun token ->
            if x.OutOfDate then
                Ag.useScope scope (fun () -> 
                    x.Compute token |> x.Apply
                )
            else
                t.mempty
        )   

    member x.Dispose() =
        x.Release()
        let mutable foo = 0
        x.Outputs.Consume(&foo) |> ignore

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface IOpReader<'ops> with
        member x.GetOperations c = x.GetOperations c

[<AbstractClass>]
type AbstractReader<'s, 'ops>(scope : Ag.Scope, t : Traceable<'s, 'ops>) =
    inherit AbstractReader<'ops>(scope, t.tops)

    let mutable state = t.tempty

    override x.Apply o =
        let (s, o) = t.tapply state o
        state <- s
        o
    member x.State = state

    interface IOpReader<'s, 'ops> with
        member x.State = state

[<AbstractClass>]
type AbstractDirtyReader<'t, 'ops when 't :> IAdaptiveObject>(scope : Ag.Scope, t : Monoid<'ops>) =
    inherit DirtyTrackingAdaptiveObject<'t>()

    abstract member Release : unit -> unit
    abstract member Compute : AdaptiveToken * HashSet<'t> -> 'ops

    abstract member Apply : 'ops -> 'ops
    default x.Apply o = o

    member x.GetOperations(token : AdaptiveToken) =
        x.EvaluateAlways' token (fun token dirty ->
            if x.OutOfDate then
                Ag.useScope scope (fun () -> 
                    x.Compute(token, dirty) |> x.Apply
                )
            else
                t.mempty
        )   

    member x.Dispose() =
        x.Release()
        let mutable foo = 0
        x.Outputs.Consume(&foo) |> ignore

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface IOpReader<'ops> with
        member x.GetOperations c = x.GetOperations c



type DisposeThread private() =
    static let queue = new Concurrent.BlockingCollection<IDisposable>()
    static let runner =
        async {
            do! Async.SwitchToNewThread()
            while true do
                let v = queue.Take()
                try v.Dispose()
                with e -> Log.warn "disposal of %A faulted: %A" v e
        }

    static do Async.Start runner

    static member Dispose(d : 'a) =
        queue.Add(d :> IDisposable)

type LazyWithFinalizer<'a when 'a :> IDisposable>(create : unit -> 'a) =
    let mutable value = Unchecked.defaultof<'a>
    let mutable created = false

    member x.Value =
        if created then 
            value
        else
            lock x (fun () ->
                if created then 
                    value
                else
                    value <- create()
                    created <- true
                    value
            )

    override x.Finalize() =
        if created then
            DisposeThread.Dispose(value)
            created <- false
            value <- Unchecked.defaultof<_>

[<AllowNullLiteral>]
type RelevantNode<'s, 'a> =
    class
        val mutable public Prev : RelevantNode<'s, 'a>
        val mutable public Next : RelevantNode<'s, 'a>
        val mutable public RefCount : int
        val mutable public BaseState : 's
        val mutable public Value : 'a
            
        new(p, s, v, n) = { Prev = p; Next = n; RefCount = 0; BaseState = s; Value = v }
    end

type History<'s, 'op> private(input : Option<LazyWithFinalizer<IOpReader<'op>>>, t : Traceable<'s, 'op>, finalize : 'op -> unit) =
    inherit AdaptiveObject()

    let mutable state   : 's = t.tempty
    let mutable first   : RelevantNode<'s, 'op> = null
    let mutable last    : RelevantNode<'s, 'op> = null
    let mutable count   : int = 0

    let rec tryCollapse (node : RelevantNode<'s, 'op>) =
        if t.tcollapse node.BaseState node.Value then
            let next = node.Next
            let prev = node.Prev
            finalize node.Value
            node.Value <- Unchecked.defaultof<_>
            node.Prev <- null
            node.Next <- null
            node.RefCount <- -1

            count <- count - 1
                
            if isNull next then last <- prev
            else next.Prev <- prev
            if isNull prev then first <- next
            else prev.Next <- next

            tryCollapse next

    let append (op : 'op) =
        // only append non-empty ops
        if not (t.tops.misEmpty op) then
            // apply the op to the state
            let s, op = t.tapply state op
            state <- s

            // if op got empty do not append it
            if not (t.tops.misEmpty op) then

                // it last is null no reader is interested in ops.
                // therefore we simply discard them here
                if not (isNull last) then

                    // last is non-null and no one pulled it yet
                    // so we can append our op to it
                    last.Value <- t.tops.mappend last.Value op

                    tryCollapse first
                true

            else
                false
        else
            false

    let addRefToLast() =
        if isNull last then
            // if there is no last (the history is empty) we append
            // a new empty last with no ops and set its refcount to 1
            let n = RelevantNode(null, state, t.tops.mempty, null)
            n.RefCount <- 1
            last <- n
            first <- n
            count <- count + 1
            n
        else
                
            if t.tops.misEmpty last.Value then
                // if last has no ops we can reuse it here
                last.RefCount <- last.RefCount + 1
                last
            else
                // if last contains ops we just consumed it and therefore
                // need a new empty last
                let n = RelevantNode(last, state, t.tops.mempty, null)
                last.Next <- n
                last <- n
                n.RefCount <- 1
                count <- count + 1
                n
              
    let mergeIntoLast (node : RelevantNode<'s, 'op>) =
        if node.RefCount = 1 then
            let res = node.Value
            let next = node.Next
            let prev = node.Prev
            
            finalize node.Value
            node.Value <- Unchecked.defaultof<_>
            node.Prev <- null
            node.Next <- null
            node.RefCount <- -1
            count <- count - 1

            if isNull next then last <- prev
            else next.Prev <- prev

            if isNull prev then 
                first <- next
                res, next
            else 
                prev.Next <- next
                prev.Value <- t.tops.mappend prev.Value res
                res, next

        else
            node.RefCount <- node.RefCount - 1
            node.Value, node.Next      


    let isInvalid (node : RelevantNode<'s, 'op>) =
        isNull node || node.RefCount < 0

    let isValid (node : RelevantNode<'s, 'op>) =
        not (isNull node) && node.RefCount >= 0

    member private x.Update (self : AdaptiveToken) =
        if x.OutOfDate then
            match input with
                | Some c -> 
                    let v = c.Value.GetOperations self
                    append v |> ignore
                | None ->
                    ()

    member x.State = state

    member x.Trace = t

    member x.Perform(op : 'op) =
        lock x (fun () ->
            let changed = append op
            if changed then x.MarkOutdated()
            changed
        )

    member x.Remove(token : RelevantNode<'s, 'op>) =
        lock x (fun () ->
            if isValid token then
                mergeIntoLast token |> ignore
        )

    member x.Read(token : AdaptiveToken, old : RelevantNode<'s, 'op>, oldState : 's) =
        x.EvaluateAlways token (fun token ->
            x.Update token

            if isInvalid old then
                let ops = t.tcompute oldState state
                let node = addRefToLast()

                node, ops
            else
                let mutable res, current = mergeIntoLast old

                while not (isNull current) do
                    res <- t.tops.mappend res current.Value
                    current <- current.Next

                let node = addRefToLast()
                node, res
        )
        
    member x.GetValue(token : AdaptiveToken) =
        x.EvaluateAlways token (fun token ->
            x.Update token
            state
        )

    member x.ReaderCount = count

    member x.NewReader() =
        new HistoryReader<'s, 'op>(x) :> IOpReader<'s, 'op>

    new (t : Traceable<'s, 'op>, finalize : 'op -> unit) = History<'s, 'op>(None, t, finalize)
    new (input : unit -> IOpReader<'op>, t : Traceable<'s, 'op>, finalize : 'op -> unit) = History<'s, 'op>(Some (LazyWithFinalizer input), t, finalize)
    new (t : Traceable<'s, 'op>) = History<'s, 'op>(None, t, ignore)
    new (input : unit -> IOpReader<'op>, t : Traceable<'s, 'op>) = History<'s, 'op>(Some (LazyWithFinalizer input), t, ignore)

    interface IMod with
        member x.IsConstant = false
        member x.GetValue c = x.GetValue c :> obj
            
    interface IMod<'s> with
        member x.GetValue c = x.GetValue c

and HistoryReader<'s, 'op>(h : History<'s, 'op>) =
    inherit AdaptiveObject()
    let trace = h.Trace
    let mutable node : RelevantNode<'s, 'op> = null
    let mutable state = trace.tempty

    member x.GetOperations(token : AdaptiveToken) =
        x.EvaluateAlways token (fun token ->
            if x.OutOfDate then
                let nt, ops = h.Read(token, node, state)
                node <- nt
                state <- h.State
                ops
            else
                trace.tops.mempty
        )

    member private x.Dispose(disposing : bool) =
        if disposing then GC.SuppressFinalize x
        lock h (fun () ->
            h.Outputs.Remove x |> ignore
            h.Remove node
        )
        node <- null
        state <- trace.tempty
        
    override x.Finalize() =
        DisposeThread.Dispose {
            new IDisposable with
                member __.Dispose() = x.Dispose false
        }

    member x.Dispose() = x.Dispose true

    interface IOpReader<'op> with
        member x.Dispose() = x.Dispose()
        member x.GetOperations c = x.GetOperations c

    interface IOpReader<'s, 'op> with
        member x.State = state

module History =

    module Readers =
        type EmptyReader<'s, 'ops>(t : Traceable<'s, 'ops>) =
            inherit ConstantObject()

            interface IOpReader<'ops> with
                member x.Dispose() = ()
                member x.GetOperations(caller) = t.tops.mempty
    
            interface IOpReader<'s, 'ops> with
                member x.State = t.tempty

        type ConstantReader<'s, 'ops>(t : Traceable<'s, 'ops>, ops : Lazy<'ops>, finalState : Lazy<'s>) =
            inherit ConstantObject()
            
            let mutable state = t.tempty
            let mutable initial = true

            interface IOpReader<'ops> with
                member x.Dispose() = ()
                member x.GetOperations(caller) =
                    lock x (fun () ->
                        if initial then
                            initial <- false
                            state <- finalState.Value
                            ops.Value
                        else
                            t.tops.mempty
                    )

            interface IOpReader<'s, 'ops> with
                member x.State = state
    
    let ofReader (t : Traceable<'s, 'ops>) (newReader : unit -> IOpReader<'ops>) =
        History<'s, 'ops>(newReader, t)

