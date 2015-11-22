namespace Aardvark.Base


open System.Collections
open System.Collections.Generic

[<AllowNullLiteral>]
type private Linked<'a> =
    class
        val mutable public Value : 'a
        val mutable public Prev : Linked<'a>
        val mutable public Next : Linked<'a>

        new (v, p, n) = { Value = v; Prev = p; Next = n }
        new (v) = { Value = v; Prev = null; Next = null }
    end


type private LinkedEnumerator<'a>(l : Linked<'a>) =
    let mutable current = null
    interface IEnumerator with
        member x.MoveNext() =
            if isNull current then
                current <- l
            else
                current <- current.Next
            not (isNull current)

        member x.Current =
            current.Value :> obj

        member x.Reset() =
            current <- null

    interface IEnumerator<'a> with
        member x.Current = current.Value
        member x.Dispose() = current <- null
                
type StableSet<'a>() =
    let content = Dict<'a, Linked<'a>>()
    let mutable first : Linked<'a> = null
    let mutable last : Linked<'a> = null

    member x.Count = content.Count

    member x.First = 
        if isNull first then None
        else Some first.Value

    member x.Last = 
        if isNull last then None
        else Some last.Value

    member x.Contains(v : 'a) =
        content.ContainsKey v

    member x.Clear() =
        content.Clear()
        first <- null
        last <- null

    member x.Add(v : 'a) =
        let n = Linked(v, last, null)
        if not (content.ContainsKey v) then
            content.[v] <- n

            if isNull last then first <- n
            else last.Next <- n

            last <- n
            true
        else
            false

    member x.Remove(v : 'a) =
        match content.TryRemove v with
            | (true, n) ->
                
                if isNull n.Prev then first <- n.Next
                else n.Prev.Next <- n.Next

                if isNull n.Next then last <- n.Prev
                else n.Next.Prev <- n.Prev

                n.Prev <- null
                n.Next <- null

                true
            | _ ->
                false

    member x.UnionWith (s : seq<'a>) =
        s |> Seq.iter (x.Add >> ignore)

    member x.ExeptWith (s : seq<'a>) =
        s |> Seq.iter (x.Remove >> ignore)

    member x.TryGetPrev(v : 'a) =
        match content.TryGetValue v with
            | (true, n) ->
                if isNull n.Prev then None
                else Some n.Prev.Value
            | _ ->
                None

    member x.TryGetNext(v : 'a) =
        match content.TryGetValue v with
            | (true, n) ->
                if isNull n.Next then None
                else Some n.Next.Value
            | _ ->
                None

    member x.AddWithPrev (f : Option<'a> -> 'a) =
        let left =
            if isNull last then None
            else Some last.Value

        let v = f left
        if x.Add v then
            Some v
        else
            None

    interface IEnumerable with
        member x.GetEnumerator() = 
            new LinkedEnumerator<'a>(first) :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() = 
            new LinkedEnumerator<'a>(first) :> IEnumerator<'a>


