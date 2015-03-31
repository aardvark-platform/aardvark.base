namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental.AListReaders

[<CompiledName("ChangeableListKey")>]
type clistkey internal (t : Time) =
    member internal x.Time = t

[<CompiledName("ChangeableList")>]
type clist<'a>(initial : seq<'a>) =
    let content = TimeList<'a>()
    let rootTime = Time.newRoot()
    let readers = WeakSet<BufferedReader<'a>>()

    interface alist<'a> with
        member x.GetReader() =
            let r = new BufferedReader<'a>(rootTime, fun r -> readers.Remove r |> ignore)
            r.Emit (content |> Seq.map Add |> Seq.toList)
            readers.Add r |> ignore
            r :> _

    member x.Count = content.Count

    member x.Remove(key : clistkey) =
        let c = content.[key.Time]
        content.Remove key.Time |> ignore

        Time.delete key.Time
        for r in readers do 
            if r.IsIncremental then r.Emit [Rem (key.Time, c)]
            else r.Reset content

    member x.InsertAfter(key : clistkey, value : 'a) =
        let newTime = Time.after key.Time
        content.Add(newTime, value)

        for r in readers do 
            if r.IsIncremental then r.Emit [Add (newTime, value)]
            else r.Reset content

        clistkey newTime

    member x.InsertBefore(key : clistkey, value : 'a) =
        x.InsertAfter(clistkey key.Time.prev, value)

    member x.Add(value : 'a) =
        x.InsertAfter(clistkey rootTime.prev, value)

    member x.Clear() =
        let deltas = content |> Seq.map Rem |> Seq.toList
        content.Clear()
        rootTime.next <- rootTime
        rootTime.prev <- rootTime
        
        for r in readers do 
            if r.IsIncremental then r.Emit deltas
            else r.Reset content

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (content.Values :> System.Collections.IEnumerable).GetEnumerator()

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() =
            (content.Values :> seq<'a>).GetEnumerator()