namespace Aardvark.Base.Incremental

open System
open System.Threading
open System.Runtime.CompilerServices
open System.Collections.Generic
open Aardvark.Base


type DomainTypeAttribute() = inherit System.Attribute()

[<AllowNullLiteral>]
type Id() =
    static let mutable current = 0
    let id = Interlocked.Increment(&current)
    static member New = Id()
    override x.ToString() = sprintf "Id %d" id



type IUnique =
    abstract member Id : Id with get, set

module Unique = 
    let id (u : IUnique) =
        let old = u.Id
        if isNull old then
            let i = Id()
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

type IReuseCache =
    interface end

type ReuseCache<'a>() =
    let store = ConditionalWeakTable<Id, ref<'a>>()
    
    interface IReuseCache

    member x.GetOrCreate(k : 'k, f : 'k -> 'a) =
        let i = Unique.id k
        lock store (fun () ->
            match store.TryGetValue i with
                | (true, v) -> !v
                | _ -> 
                    let v = f k
                    store.Add(i, ref v)
                    v
        )
        

type MapSet<'k, 'v when 'k : equality and 'k :> IUnique>(cache : ReuseCache<'v>, initial : pset<'k>, create : 'k -> 'v, update : 'v * 'k -> unit) =
    //let store = Dictionary<int64, 'v>()
    let readers = HashSet<ASetReaders.EmitReader<'v>>()
    let content = VersionedSet (HashSet())
    let readers = WeakSet<ASetReaders.EmitReader<'v>>()
    do for e in initial do
        let id = Unique.id e
        let mutable isNew = false
        let v = cache.GetOrCreate(e, fun k -> isNew <- true; create k)
        if not (content.Add v) then
            update(v,e)

    let emit (deltas : Option<Change<'v>>) =
        lock readers (fun () ->
            for r in readers do 
                r.Emit(content, deltas)
        )

    member x.Update(keys : pset<'k>) =
        let deltas =
            lock content (fun () -> 
                let removed = HashSet content
                let added = HashSet()
                for k in PSet.toSeq keys do
                    let id = Unique.id k
                    let v = cache.GetOrCreate(k, create)
                    if not (removed.Remove v) then 
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


type ReuseCache() =

    let version = ref 0L
    let store = Dict<Type, IReuseCache>()

    member x.GetCache<'a>() =
        let dict = store.GetOrCreate(typeof<'a>, fun t -> ReuseCache<'a>() :> IReuseCache)
        dict |> unbox<ReuseCache<'a>>
