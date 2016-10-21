namespace Aardvark.Base.Incremental

open System.Collections.Generic
open Aardvark.Base


type DomainTypeAttribute() = inherit System.Attribute()

type IUnique =
    abstract member Id : int64 with get, set

module Unique = 
    open System.Threading
    let mutable private currentId = 1L

    let id (u : IUnique) =
        let old = u.Id
        if old = 0L then
            let i = Interlocked.Increment(&currentId)
            u.Id <- i
            i
        else
            old

type ResetSet<'k>(initial : pset<'k>) =
    inherit cset<'k>(initial)
    let mutable current = initial

    member x.Update(keys : pset<'k>) =
        let delta = PSet.computeDelta current keys
        current <- keys
        x.ApplyDeltas(delta)

type MapSet<'k, 'v when 'k : equality and 'k :> IUnique>(initial : pset<'k>, create : 'k -> 'v, update : 'v * 'k -> unit) =
    let store = Dictionary<int64, 'v>()
    let readers = HashSet<ASetReaders.EmitReader<'v>>()
    let content = VersionedSet (HashSet())
    let readers = WeakSet<ASetReaders.EmitReader<'v>>()
    do for e in initial do
        let id = Unique.id e
        match store.TryGetValue id with
            | (true, v) -> 
                update(v, e)
            | _ ->
                let v = create e
                store.Add(id, v)
                content.Add v |> ignore

    let emit (deltas : Option<Change<'v>>) =
        lock readers (fun () ->
            for r in readers do 
                r.Emit(content, deltas)
        )

    member x.Update(keys : pset<'k>) =
        let deltas =
            lock content (fun () -> 
                let removed = HashSet store.Values
                let added = HashSet()
                for k in PSet.toSeq keys do
                    let id = Unique.id k

                    match store.TryGetValue id with
                        | (true, v) -> 
                            update(v, k)
                            removed.Remove v |> ignore
                        | _ ->
                            let v = create k
                            store.Add(id, v)
                            added.Add v |> ignore

                [
                    yield! added |> Seq.filter content.Add |> Seq.map Add
                    yield! removed |> Seq.filter content.Remove |> Seq.map Rem
                ]
            )
        emit (Some deltas)

    member x.GetReader() =
        lock readers (fun () ->
            let r = new ASetReaders.EmitReader<'v>(content, fun r -> lock readers (fun () -> readers.Remove r |> ignore))
            r.Emit(content, None)
            readers.Add r |> ignore
            r :> IReader<_>
        )

    interface aset<'v> with
        member x.ReaderCount = lock readers (fun () -> readers.Count)
        member x.IsConstant = false

        member x.Copy = x :> aset<_>
        member x.GetReader() = x.GetReader()
