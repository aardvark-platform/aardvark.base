namespace Aardvark.Base


open System.Threading
open System.Runtime.InteropServices

[<AllowNullLiteral>]
type private HashQueueNode<'a> =
    class
        val mutable public Value : 'a
        val mutable public Next : HashQueueNode<'a>
        val mutable public Prev : HashQueueNode<'a>

        new(v,p,n) = { Value = v; Prev = p; Next = n }
    end

type ConcurrentHashQueue<'a when 'a : equality>() =
    let lockObj = obj()
    let nodes = Dict<'a, HashQueueNode<'a>>()
    let mutable first = null
    let mutable last = null

    let detach (node : HashQueueNode<'a>) =
        if isNull node.Prev then first <- node.Next
        else node.Prev.Next <- node.Next

        if isNull node.Next then last <- node.Prev
        else node.Next.Prev <- node.Prev
                                 

    member x.Count = lock lockObj (fun () -> nodes.Count)

    member x.Enqueue(value : 'a) =
        lock lockObj (fun () ->
            let node = 
                match nodes.TryGetValue value with
                    | (true, node) -> 
                        detach node
                        node.Prev <- last
                        node.Next <- null
                        node    
                    | _ ->
                        let node = HashQueueNode(value, last, null)
                        nodes.[value] <- node
                        node

            if isNull last then first <- node
            else last.Next <- node
            last <- node
        )

    member x.Dequeue() =
        lock lockObj (fun () ->
            if isNull first then 
                failwith "HashQueue empty"
            else
                let value = first.Value
                first <- first.Next
                first.Prev <- null
                nodes.Remove value |> ignore
                value
        )

    member x.TryDequeue([<Out>] result : byref<'a>) =
        try
            Monitor.Enter lockObj
            if isNull first then 
                false
            else
                let value = first.Value
                detach first
                nodes.Remove value |> ignore
                result <- value
                true
        finally
            Monitor.Exit lockObj

    member x.Remove(value : 'a) =
        lock lockObj (fun () ->
            match nodes.TryRemove value with
                | (true, node) ->
                    detach node
                    node.Value <- Unchecked.defaultof<_>
                    node.Prev <- null
                    node.Next <- null
                    true
                | _ ->
                    false
        )

    member x.Clear() =
        lock lockObj (fun () ->
            nodes.Clear()
            first <- null
            last <- null
        )

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ConcurrentHashQueue =
    
    let inline empty<'a when 'a : equality> = ConcurrentHashQueue<'a>()

    let inline enqueue (v : 'a) (q : ConcurrentHashQueue<'a>) =
        q.Enqueue v

    let inline tryDequeue (q : ConcurrentHashQueue<'a>) =
        match q.TryDequeue() with
            | (true, v) -> Some v
            | _ -> None

    let inline dequeue (q : ConcurrentHashQueue<'a>) =
        q.Dequeue()

    let inline count (q : ConcurrentHashQueue<'a>) =
        q.Count

    let inline clear (q : ConcurrentHashQueue<'a>) =
        q.Clear()