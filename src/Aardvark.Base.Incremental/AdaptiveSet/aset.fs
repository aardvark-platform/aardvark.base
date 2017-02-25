namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open Aardvark.Base

type ISetReader<'a> = IOpReader<prefset<'a>, deltaset<'a>>

type aset<'a> =
    abstract member IsConstant  : bool
    abstract member Content     : IMod<prefset<'a>>
    abstract member GetReader   : unit -> ISetReader<'a>

[<StructuredFormatDisplay("{AsString}")>]
type cset<'a>(initial : seq<'a>) =
    let history = History PRefSet.traceNoRefCount
    do initial |> Seq.map Add |> DeltaSet.ofSeq |> history.Perform |> ignore

    member x.Add(v : 'a) =
        lock x (fun () ->
            let op = DeltaSet.single (Add v)
            history.Perform op
        )
        
    member x.Remove(v : 'a) =
        lock x (fun () ->
            let op = DeltaSet.single (Rem v)
            history.Perform op
        )

    member x.Contains (v : 'a) =
        history.State |> PRefSet.contains v

    member x.Count =
        history.State.Count

    member x.UnionWith (other : seq<'a>) =
        lock x (fun () -> 
            let op = other |> Seq.map Add |> DeltaSet.ofSeq
            history.Perform op |> ignore
        )

    member x.ExceptWith (other : seq<'a>) =
        lock x (fun () -> 
            let op = other |> Seq.map Rem |> DeltaSet.ofSeq
            history.Perform op |> ignore
        )

    member x.IntersectWith (other : seq<'a>) =
        lock x (fun () -> 
            let other = PRefSet.ofSeq other
            let op = PRefSet.computeDeltas (PRefSet.difference history.State other) PRefSet.empty
            history.Perform op |> ignore
        )

    member x.SymmetricExceptWith (other : seq<'a>) = 
        let other = PRefSet.ofSeq other
        lock x (fun () -> 
            let add = PRefSet.computeDeltas PRefSet.empty (PRefSet.difference other history.State) 
            let rem = PRefSet.computeDeltas (PRefSet.intersect other history.State) PRefSet.empty
            let op = DeltaSet.combine add rem
            history.Perform op |> ignore
        )

    member x.Clear() =
        lock x (fun () -> 
            let op = PRefSet.computeDeltas history.State PRefSet.empty
            history.Perform op |> ignore
        )

    member x.CopyTo(arr : 'a[], index : int) =
        let mutable index = index
        for e in x do
            arr.[index] <- e
            index <- index + 1

    interface ICollection<'a> with 
        member x.Add(v) = x.Add v |> ignore
        member x.Clear() = x.Clear()
        member x.Remove(v) = x.Remove v
        member x.Contains(v) = x.Contains v
        member x.CopyTo(arr,i) = x.CopyTo(arr, i)
        member x.IsReadOnly = false
        member x.Count = x.Count

    interface ISet<'a> with
        member x.Add v = x.Add v
        member x.ExceptWith o = x.ExceptWith o
        member x.UnionWith o = x.UnionWith o
        member x.IntersectWith o = x.IntersectWith o
        member x.SymmetricExceptWith o = x.SymmetricExceptWith o
        member x.IsSubsetOf o = 
            let (l, b, r) = prefset.Compare(history.State, PRefSet.ofSeq o)
            l = 0

        member x.IsProperSubsetOf o = 
            let (l, b, r) = prefset.Compare(history.State, PRefSet.ofSeq o)
            l = 0 && r > 0
            
        member x.IsSupersetOf o = 
            let (l, b, r) = prefset.Compare(history.State, PRefSet.ofSeq o)
            r = 0

        member x.IsProperSupersetOf o = 
            let (l, b, r) = prefset.Compare(history.State, PRefSet.ofSeq o)
            r = 0 && l > 0

        member x.Overlaps o = 
            let (l, b, r) = prefset.Compare(history.State, PRefSet.ofSeq o)
            b > 0

        member x.SetEquals o = 
            let (l, b, r) = prefset.Compare(history.State, PRefSet.ofSeq o)
            l = 0 && r = 0

    interface aset<'a> with
        member x.IsConstant = false
        member x.Content = history :> IMod<_>
        member x.GetReader() = history.NewReader()

    interface IEnumerable with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator() :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator() :> _

    override x.ToString() =
        let suffix =
            if x.Count > 5 then "; ..."
            else ""

        let content =
            history.State |> Seq.truncate 5 |> Seq.map (sprintf "%A") |> String.concat "; "

        "cset [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    new() = cset<'a>(Seq.empty)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module CSet =
    let inline empty<'a> = cset<'a>()
    
    let inline ofSet (set : prefset<'a>) = cset(set)
    let inline ofSeq (seq : seq<'a>) = cset(seq)
    let inline ofList (list : list<'a>) = cset(list)
    let inline ofArray (list : array<'a>) = cset(list)

    let inline add (v : 'a) (set : cset<'a>) = set.Add v
    let inline remove (v : 'a) (set : cset<'a>) = set.Remove v
    let inline clear (set : cset<'a>) = set.Clear()

    let inline unionWith (other : seq<'a>) (set : cset<'a>) = set.UnionWith other
    let inline exceptWith (other : seq<'a>) (set : cset<'a>) = set.ExceptWith other
    let inline intersectWith (other : seq<'a>) (set : cset<'a>) = set.IntersectWith other
    let inline symmetricExceptWith (other : seq<'a>) (set : cset<'a>) = set.SymmetricExceptWith other

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module ASet =

    [<AutoOpen>]
    module Implementation = 
        type EmptyReader<'a> private() =
            inherit ConstantObject()

            static let instance = new EmptyReader<'a>() :> IOpReader<_,_>
            static member Instance = instance

            interface IOpReader<deltaset<'a>> with
                member x.Dispose() =
                    ()

                member x.GetOperations caller =
                    DeltaSet.empty

            interface IOpReader<prefset<'a>, deltaset<'a>> with
                member x.State = PRefSet.empty

        type EmptySet<'a> private() =
            let content = Mod.constant PRefSet.empty

            static let instance = EmptySet<'a>() :> aset<_>

            static member Instance = instance


            interface aset<'a> with
                member x.IsConstant = true
                member x.Content = content
                member x.GetReader() = EmptyReader.Instance

        type ConstantSet<'a>(content : Lazy<prefset<'a>>) =
            let deltas = lazy ( content.Value |> Seq.map Add |> DeltaSet.ofSeq)
            let mcontent = ConstantMod<prefset<'a>>(content) :> IMod<_>

            interface aset<'a> with
                member x.IsConstant = true
                member x.GetReader() = new History.Readers.ConstantReader<_,_>(PRefSet.trace, deltas, content) :> ISetReader<_>
                member x.Content = mcontent
        
            new(content : prefset<'a>) = ConstantSet<'a>(Lazy.CreateFromValue content)

        type AdaptiveSet<'a>(newReader : unit -> IOpReader<deltaset<'a>>) =
            let h = History.ofReader PRefSet.trace newReader

            interface aset<'a> with
                member x.IsConstant = false
                member x.Content = h :> IMod<_>
                member x.GetReader() = h.NewReader()

        let inline unexpected() =
            failwith "[ASet] deltas are expected to be unique"

        let inline aset (f : Ag.Scope -> #IOpReader<deltaset<'a>>) =
            let scope = Ag.getContext()
            AdaptiveSet<'a>(fun () -> f scope :> IOpReader<_>) :> aset<_>

        let inline constant (l : Lazy<prefset<'a>>) =
            ConstantSet<'a>(l) :> aset<_>

    [<AutoOpen>]
    module Readers =
        type MapReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> 'b) =
            inherit AbstractReader<deltaset<'b>>(scope, DeltaSet.monoid)
            
            let cache = Cache f
            let r = input.GetReader()

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute() =
                r.GetOperations x |> DeltaSet.map (fun d ->
                    match d with
                        | Add(1, v) -> Add(cache.Invoke v)
                        | Rem(1, v) -> Rem(cache.Revoke v)
                        | _ -> unexpected()
                )

        type ChooseReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> Option<'b>) =
            inherit AbstractReader<deltaset<'b>>(scope, DeltaSet.monoid)
            
            let cache = Cache f
            let r = input.GetReader()

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute() =
                r.GetOperations x |> DeltaSet.choose (fun d ->
                    match d with
                        | Add(1, v) -> 
                            match cache.Invoke v with
                                | Some v -> Some (Add v)
                                | None -> None
                        | Rem(1, v) ->
                            match cache.Revoke v with
                                | Some v -> Some (Rem v)
                                | None -> None
                        | _ -> 
                            unexpected()
                )

        type FilterReader<'a>(scope : Ag.Scope, input : aset<'a>, f : 'a -> bool) =
            inherit AbstractReader<deltaset<'a>>(scope, DeltaSet.monoid)
            
            let cache = Cache f
            let r = input.GetReader()

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute() =
                r.GetOperations x |> DeltaSet.choose (fun d ->
                    match d with
                        | Add(1, v) -> 
                            if cache.Invoke v then Some (Add v)
                            else None
                        | Rem(1, v) ->
                            if cache.Revoke v then Some (Rem v)
                            else None
                        | _ -> 
                            unexpected()
                )

        type UnionReader<'a>(scope : Ag.Scope, input : aset<aset<'a>>) =
            inherit AbstractDirtyReader<ISetReader<'a>, deltaset<'a>>(scope, DeltaSet.monoid)

            let r = input.GetReader()
            let cache = Cache(fun (a : aset<'a>) -> a.GetReader())

            override x.Release() =
                r.Dispose()
                cache.Clear (fun r -> r.Dispose())

            override x.Compute dirty =
                let mutable deltas = 
                    r.GetOperations x |> DeltaSet.collect (fun d ->
                        match d with
                            | Add(1, v) ->
                                let r = cache.Invoke v
                                dirty.Remove r |> ignore
                                r.GetOperations x

                            | Rem(1, v) -> 
                                let deleted, r = cache.RevokeAndGetDeleted v
                                dirty.Remove r |> ignore
                                if deleted then 
                                    let ops = PRefSet.computeDeltas r.State PRefSet.empty
                                    r.Dispose()
                                    ops
                                else
                                    r.GetOperations x
                                
                            | _ -> unexpected()
                    )

                for d in dirty do
                    deltas <- DeltaSet.combine deltas (d.GetOperations x)

                deltas

        type UnionFixedReader<'a>(scope : Ag.Scope, input : prefset<aset<'a>>) =
            inherit AbstractDirtyReader<ISetReader<'a>, deltaset<'a>>(scope, DeltaSet.monoid)

            let mutable initial = true
            let input = input |> PRefSet.map (fun s -> s.GetReader())

            override x.Release() =
                for i in input do
                    i.Dispose()

            override x.Compute(dirty) =
                if initial then
                    initial <- false
                    input |> PRefSet.fold (fun deltas r -> DeltaSet.combine deltas (r.GetOperations x)) DeltaSet.empty
                else
                    dirty |> Seq.fold (fun deltas r -> DeltaSet.combine deltas (r.GetOperations x)) DeltaSet.empty

        type DifferenceReader<'a>(scope : Ag.Scope, l : aset<'a>, r : aset<'a>) =
            inherit AbstractReader<deltaset<'a>>(scope, DeltaSet.monoid)
            
            let l = l.GetReader()
            let r = r.GetReader()

            override x.Release() =
                l.Dispose()
                r.Dispose()

            override x.Compute() =
                let lops = l.GetOperations x
                let rops = r.GetOperations x

                let rops = DeltaSet.map SetDelta.inverse rops

                DeltaSet.combine lops rops




        type CollectReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> aset<'b>) =
            inherit AbstractDirtyReader<ISetReader<'b>, deltaset<'b>>(scope, DeltaSet.monoid)

            let r = input.GetReader()
            let cache = Cache(fun a -> (f a).GetReader())

            override x.Release() =
                r.Dispose()
                cache.Clear (fun r -> r.Dispose())

            override x.Compute dirty =
                let mutable deltas = 
                    r.GetOperations x |> DeltaSet.collect (fun d ->
                        match d with
                            | Add(1, v) ->
                                let r = cache.Invoke v
                                dirty.Remove r |> ignore
                                r.GetOperations x

                            | Rem(1, v) -> 
                                let deleted, r = cache.RevokeAndGetDeleted v
                                dirty.Remove r |> ignore
                                if deleted then 
                                    let ops = PRefSet.computeDeltas r.State PRefSet.empty
                                    r.Dispose()
                                    ops
                                else
                                    r.GetOperations x
                                
                            | _ -> unexpected()
                    )

                for d in dirty do
                    deltas <- DeltaSet.combine deltas (d.GetOperations x)

                deltas

        type CollectSetReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> prefset<'b>) =
            inherit AbstractReader<deltaset<'b>>(scope, DeltaSet.monoid)
            
            let r = input.GetReader()
            let cache = Cache f

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute() =
                r.GetOperations x |> DeltaSet.collect (fun d ->
                    match d with
                        | Add(1,v) -> 
                            PRefSet.computeDeltas PRefSet.empty (cache.Invoke v)
                        | Rem(1,v) ->
                            PRefSet.computeDeltas (cache.Revoke v) PRefSet.empty
                        | _ ->
                            unexpected()
                )


        type ModSetReader<'a>(scope : Ag.Scope, input : IMod<prefset<'a>>) =
            inherit AbstractReader<deltaset<'a>>(scope, DeltaSet.monoid)

            let mutable old = PRefSet.empty

            override x.Release() =
                lock input (fun () -> input.Outputs.Remove x |> ignore)

            override x.Compute() =
                let n = input.GetValue x
                let deltas = PRefSet.computeDeltas old n
                old <- n
                deltas

        type ModValueReader<'a>(scope : Ag.Scope, input : IMod<'a>) =
            inherit AbstractReader<deltaset<'a>>(scope, DeltaSet.monoid)

            let mutable old = None

            override x.Release() =
                lock input (fun () -> input.Outputs.Remove x |> ignore)
                old <- None

            override x.Compute() =
                let n = input.GetValue x
                let delta = 
                    match old with
                        | None -> DeltaSet.ofList [Add n]
                        | Some o when Object.Equals(o, n) -> DeltaSet.empty
                        | Some o -> DeltaSet.ofList [Rem o; Add n]
                old <- Some n
                delta

        type BindReader<'a, 'b>(scope : Ag.Scope, input : IMod<'a>, f : 'a -> aset<'b>) =
            inherit AbstractReader<deltaset<'b>>(scope, DeltaSet.monoid)
            
            let mutable inputChanged = true
            let mutable old : Option<'a * ISetReader<'b>> = None
            
            override x.InputChanged(t : obj, i : IAdaptiveObject) =
                inputChanged <- inputChanged || Object.ReferenceEquals(i, input)

            override x.Release() =
                lock input (fun () -> input.Outputs.Remove x |> ignore)
                match old with
                    | Some (_,r) -> 
                        r.Dispose()
                        old <- None
                    | _ ->
                        ()

            override x.Compute() =
                let v = input.GetValue x
                match old with
                    | Some(_,ro) when inputChanged ->
                        ro.Dispose()
                        let r = (f v).GetReader()
                        old <- Some(v, r)
                        r.GetOperations x

                    | Some(vo, ro) ->
                        ro.GetOperations x

                    | None ->
                        let r = (f v).GetReader()
                        old <- Some(v, r)
                        r.GetOperations x

        type CustomReader<'a>(scope : Ag.Scope, compute : ISetReader<'a> -> deltaset<'a>) =
            inherit AbstractReader<prefset<'a>, deltaset<'a>>(scope, PRefSet.trace)
            
            override x.Release() =
                ()

            override x.Compute() =
                compute x
            
        type FlattenReader<'a>(scope : Ag.Scope, input : aset<IMod<'a>>) =
            inherit AbstractDirtyReader<IMod<'a>, deltaset<'a>>(scope, DeltaSet.monoid)
            
            let r = input.GetReader()

            let mutable initial = true
            let cache = Dict<IMod<'a>, 'a>()

            member x.Invoke(m : IMod<'a>) =
                let v = m.GetValue x
                cache.[m] <- v
                v

            member x.Invoke2(m : IMod<'a>) =
                let o = cache.[m]
                let v = m.GetValue x
                cache.[m] <- v
                o, v

            member x.Revoke(m : IMod<'a>) =
                match cache.TryRemove m with
                    | (true, v) -> 
                        lock m (fun () -> m.Outputs.Remove x |> ignore )
                        v
                    | _ -> 
                        failwith "[ASet] cannot remove unknown object"


            override x.Release() =
                for m in r.State do 
                    lock m (fun () -> m.Outputs.Remove x |> ignore)
                r.Dispose()

            override x.Compute(dirty) =
                let mutable deltas = 
                    r.GetOperations x |> DeltaSet.map (fun d ->
                        match d with
                            | Add(1,m) -> Add(x.Invoke m)
                            | Rem(1,m) -> Rem(x.Revoke m)
                            | _ -> unexpected()
                    )

                for d in dirty do
                    let o, n = x.Invoke2 d
                    if not <| Object.Equals(o,n) then
                        deltas <- DeltaSet.combine deltas (DeltaSet.ofList [Add n; Rem o])

                deltas
            
        type MapMReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> IMod<'b>) =
            inherit AbstractDirtyReader<IMod<'b>, deltaset<'b>>(scope, DeltaSet.monoid)
            
            let r = input.GetReader()

            let f = Cache f
            let mutable initial = true
            let cache = Dict<IMod<'b>, 'b>()

            member x.Invoke(v : 'a) =
                let m = f.Invoke v
                let v = m.GetValue x
                cache.[m] <- v
                v

            member x.Invoke2(m : IMod<'b>) =
                let o = cache.[m]
                let v = m.GetValue x
                cache.[m] <- v
                o, v

            member x.Revoke(v : 'a) =
                let m = f.Revoke v
                match cache.TryRemove m with
                    | (true, v) -> 
                        lock m (fun () -> m.Outputs.Remove x |> ignore )
                        v
                    | _ -> 
                        failwith "[ASet] cannot remove unknown object"


            override x.Release() =
                f.Clear ignore

                for (KeyValue(m,_)) in cache do 
                    lock m (fun () -> m.Outputs.Remove x |> ignore)
                cache.Clear()

                r.Dispose()

            override x.Compute(dirty) =
                let mutable deltas = 
                    r.GetOperations x |> DeltaSet.map (fun d ->
                        match d with
                            | Add(1,m) -> Add(x.Invoke m)
                            | Rem(1,m) -> Rem(x.Revoke m)
                            | _ -> unexpected()
                    )

                for d in dirty do
                    let o, n = x.Invoke2 d
                    if not <| Object.Equals(o,n) then
                        deltas <- DeltaSet.combine deltas (DeltaSet.ofList [Add n; Rem o])

                deltas

        type ChooseMReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> IMod<Option<'b>>) =
            inherit AbstractDirtyReader<IMod<Option<'b>>, deltaset<'b>>(scope, DeltaSet.monoid)
            
            let r = input.GetReader()

            let f = Cache f
            let mutable initial = true
            let cache = Dict<IMod<Option<'b>>, Option<'b>>()

            member x.Invoke(v : 'a) =
                let m = f.Invoke v
                let v = m.GetValue x
                cache.[m] <- v
                v

            member x.Invoke2(m : IMod<Option<'b>>) =
                let o = cache.[m]
                let v = m.GetValue x
                cache.[m] <- v
                o, v

            member x.Revoke(v : 'a) =
                let m = f.Revoke v
                match cache.TryRemove m with
                    | (true, v) -> 
                        lock m (fun () -> m.Outputs.Remove x |> ignore )
                        v
                    | _ -> 
                        failwith "[ASet] cannot remove unknown object"


            override x.Release() =
                f.Clear ignore

                for (KeyValue(m,_)) in cache do 
                    lock m (fun () -> m.Outputs.Remove x |> ignore)
                cache.Clear()

                r.Dispose()

            override x.Compute(dirty) =
                let mutable deltas = 
                    r.GetOperations x |> DeltaSet.choose (fun d ->
                        match d with
                            | Add(1,m) -> 
                                match x.Invoke m with
                                    | Some v -> Some (Add v)
                                    | None -> None

                            | Rem(1,m) ->
                                match x.Revoke m with
                                    | Some v -> Some (Rem v)
                                    | None -> None

                            | _ -> 
                                unexpected()
                    )

                for d in dirty do
                    let change = 
                        match x.Invoke2 d with
                            | None, None -> 
                                DeltaSet.empty

                            | None, Some v ->
                                DeltaSet.single (Add v)

                            | Some o, None ->
                                DeltaSet.single (Rem o)

                            | Some o, Some n ->
                                if Object.Equals(o, n) then
                                    DeltaSet.empty
                                else
                                    DeltaSet.ofList [Rem o; Add n]

                    deltas <- DeltaSet.combine deltas change

                deltas

    // =====================================================================================
    // CREATORS (of*)
    // =====================================================================================

    /// the empty aset
    let empty<'a> = EmptySet<'a>.Instance

    /// creates a new aset containing only the given element
    let single (v : 'a) =
        ConstantSet(PRefSet.single v) :> aset<_>

    /// creates a new aset using the given set content
    let ofSet (set : prefset<'a>) =
        ConstantSet(set) :> aset<_>

    /// create a new aset using all distinct entries from the sequence
    let ofSeq (seq : seq<'a>) =
        seq |> PRefSet.ofSeq |> ofSet
        
    /// create a new aset using all distinct entries from the list
    let ofList (list : list<'a>) =
        list |> PRefSet.ofList |> ofSet
        
    /// create a new aset using all distinct entries from the array
    let ofArray (arr : 'a[]) =
        arr |> PRefSet.ofArray |> ofSet

    /// creates set which will always contain the elements given by the mod-cell
    let ofMod (m : IMod<prefset<'a>>) =
        if m.IsConstant then
            constant <| lazy ( Mod.force m )
        else
            aset <| fun scope -> new ModSetReader<'a>(scope, m)
            
    /// creates a singleton set which will always contain the latest value of the given mod-cell
    let ofModSingle (m : IMod<'a>) =
        if m.IsConstant then
            constant <| lazy ( m |> Mod.force |> PRefSet.single )
        else
            aset <| fun scope -> new ModValueReader<'a>(scope, m)


    // =====================================================================================
    // VIEWS (to*)
    // =====================================================================================

    /// creates a set from the current state of the aset
    let toSet (set : aset<'a>) =
        set.Content |> Mod.force
        
    /// creates a seq from the current state of the aset
    let toSeq (set : aset<'a>) =
        set.Content |> Mod.force :> seq<_>
        
    /// creates a list from the current state of the aset
    let toList (set : aset<'a>) =
        set.Content |> Mod.force |> PRefSet.toList
        
    /// creates an array from the current state of the aset
    let toArray (set : aset<'a>) =
        set.Content |> Mod.force |> PRefSet.toArray

    /// creates a mod-cell containing the set's content as set
    let toMod (s : aset<'a>) =
        s.Content


    // =====================================================================================
    // OPERATIONS
    // =====================================================================================

    let union (l : aset<'a>) (r : aset<'a>) =
        if l.IsConstant && r.IsConstant then
            constant <| lazy ( PRefSet.union (Mod.force l.Content) (Mod.force r.Content) )
        else
            aset <| fun scope -> new UnionFixedReader<'a>(scope, PRefSet.ofList [l; r])

    let difference (l : aset<'a>) (r : aset<'a>) =
        if l.IsConstant && r.IsConstant then
            constant <| lazy ( PRefSet.difference (Mod.force l.Content) (Mod.force r.Content) )
        else
            aset <| fun scope -> new DifferenceReader<'a>(scope, l, r)

    let unionMany' (sets : seq<aset<'a>>) =
        let sets = PRefSet.ofSeq sets
        if sets |> Seq.forall (fun s -> s.IsConstant) then
            constant <| lazy ( sets |> PRefSet.collect (fun s -> s.Content |> Mod.force) )
        else
            aset <| fun scope -> new UnionFixedReader<'a>(scope, sets)

    let unionMany (sets : aset<aset<'a>>) =
        if sets.IsConstant then
            sets.Content |> Mod.force |> unionMany'
        else
            aset <| fun scope -> new UnionReader<'a>(scope, sets)

    // =====================================================================================
    // PROJECTIONS
    // =====================================================================================

    /// creates a new aset whose elements are the result of applying the given function to each of the elements of the given set
    let map (mapping : 'a -> 'b) (set : aset<'a>) =
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> PRefSet.map mapping )
        else
            aset <| fun scope -> new MapReader<'a, 'b>(scope, set, mapping)
        
    /// applies the given function to each element of the given aset. returns an aset comprised of the results x for each element
    /// where the function returns Some(x)
    let choose (chooser : 'a -> Option<'b>) (set : aset<'a>) =
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> PRefSet.choose chooser )
        else
            aset <| fun scope -> new ChooseReader<'a, 'b>(scope, set, chooser)

    /// creates a new aset containing only the elements of the given one for which the given predicate returns true
    let filter (predicate : 'a -> bool) (set : aset<'a>) =
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> PRefSet.filter predicate )
        else
            aset <| fun scope -> new FilterReader<'a>(scope, set, predicate)

    /// applies the given function to each element of the given aset. unions all the results and returns the combined aset
    let collect (mapping : 'a -> aset<'b>) (set : aset<'a>) =
        if set.IsConstant then
            set.Content |> Mod.force |> PRefSet.map mapping |> unionMany'
        else
            aset <| fun scope -> new CollectReader<'a, 'b>(scope, set, mapping)
        
    /// applies the given function to each element of the given aset. unions all the results and returns the combined aset
    let collect' (mapping : 'a -> #seq<'b>) (set : aset<'a>) =
        let mapping = mapping >> PRefSet.ofSeq
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> PRefSet.collect mapping )
        else
            aset <| fun scope -> new CollectSetReader<'a, 'b>(scope, set, mapping)
    
    
    // =====================================================================================
    // MOD INTEROP
    // =====================================================================================

    let flattenM (set : aset<IMod<'a>>) =
        aset <| fun scope -> new FlattenReader<'a>(scope, set)


    let mapM (mapping : 'a -> IMod<'b>) (set : aset<'a>) =
        aset <| fun scope -> new MapMReader<'a, 'b>(scope, set, mapping)

    let chooseM (mapping : 'a -> IMod<Option<'b>>) (set : aset<'a>) =
        aset <| fun scope -> new ChooseMReader<'a, 'b>(scope, set, mapping)

    let filterM (predicate : 'a -> IMod<bool>) (set : aset<'a>) =
        set |> chooseM (fun a ->
            a |> predicate |> Mod.map (fun v -> if v then Some a else None)
        )

    let bind (mapping : 'a -> aset<'b>) (m : IMod<'a>) =
        if m.IsConstant then
            mapping (Mod.force m)
        else
            aset <| fun scope -> new BindReader<'a, 'b>(scope, m, mapping)

    let bind2 (mapping : 'a -> 'b -> aset<'c>) (a : IMod<'a>) (b : IMod<'b>) =
        match a.IsConstant, b.IsConstant with
            | true,  true  -> 
                mapping (Mod.force a) (Mod.force b)
            | true,  false ->
                let mapping = mapping (Mod.force a)
                b |> bind mapping

            | false, true  ->
                let mapping = 
                    let b = Mod.force b
                    fun a -> mapping a b

                a |> bind mapping

            | false, false ->
                let tup = Mod.map2 (fun a b -> (a,b)) a b
                tup |> bind (fun (a,b) -> mapping a b)

    
    // =====================================================================================
    // FOLDS
    // =====================================================================================

    let foldHalfGroup (add : 's -> 'a -> 's) (trySub : 's -> 'a -> Option<'s>) (zero : 's) (s : aset<'a>) =
        let r = s.GetReader()
        let mutable res = zero

        let rec traverse (d : list<SetDelta<'a>>) =
            match d with
                | [] -> true
                | d :: rest ->
                    match d with
                        | Add(1,v) -> 
                            res <- add res v
                            traverse rest

                        | Rem(1,v) ->
                            match trySub res v with
                                | Some s ->
                                    res <- s
                                    traverse rest
                                | None ->
                                    false
                        | _ -> failwith "unexpected delta"
                                    

        Mod.custom (fun self ->
            let ops = r.GetOperations self

            let worked = traverse (DeltaSet.toList ops)

            if not worked then
                res <- r.State |> PRefSet.fold add zero
                
            res
        )

    let fold (f : 's -> 'a -> 's) (seed : 's) (s : aset<'a>) =
        foldHalfGroup f (fun _ _ -> None) seed s

    let foldGroup (add : 's -> 'a -> 's) (sub : 's -> 'a -> 's) (zero : 's) (s : aset<'a>) =
        foldHalfGroup add (fun a b -> Some (sub a b)) zero s
       
       
    let containsAll (seq : seq<'a>) (set : aset<'a>) =
        set.Content |> Mod.map (fun set ->
            seq |> Seq.forall (fun v -> PRefSet.contains v set)
        )   
        
    let containsAny (seq : seq<'a>) (set : aset<'a>) =
        set.Content |> Mod.map (fun set ->
            seq |> Seq.exists (fun v -> PRefSet.contains v set)
        )   

    /// Adaptively calculates the sum of all elements in the set
    let inline sum (s : aset<'a>) = foldGroup (+) (-) LanguagePrimitives.GenericZero s

    /// Adaptively calculates the product of all elements in the set
    let inline product (s : aset<'a>) = foldGroup (*) (/) LanguagePrimitives.GenericOne s


    let mapUse<'a, 'b when 'b :> IDisposable> (f : 'a -> 'b) (set : aset<'a>) : aset<'b> =
        failwith "not implemented"



    /// creates a new aset using the given reader-creator
    let create (f : Ag.Scope -> #IOpReader<deltaset<'a>>) =
        aset f

    let custom (f : ISetReader<'a> -> deltaset<'a>) =
        aset <| fun scope -> new CustomReader<'a>(scope, f)

    open System.Collections.Concurrent
    open System.Runtime.CompilerServices
    /// <summary>
    /// registers a callback for execution whenever the
    /// set's value might have changed and returns a disposable
    /// subscription in order to unregister the callback.
    /// Note that the callback will be executed immediately
    /// once here.
    /// Note that this function does not hold on to the created disposable, i.e.
    /// if the disposable as well as the source dies, the callback dies as well.
    /// If you use callbacks to propagate changed to other mods by using side-effects
    /// (which you should not do), use registerCallbackKeepDisposable in order to
    /// create a gc to the fresh disposable.
    /// registerCallbackKeepDisposable only destroys the callback, iff the associated
    /// disposable is disposed.
    /// </summary>
    let private callbackTable = ConditionalWeakTable<obj, ConcurrentHashSet<IDisposable>>()

    let unsafeRegisterCallbackNoGcRoot (f : list<SetDelta<'a>> -> unit) (set : aset<'a>) =
        let m = set.GetReader()

        let result =
            m.AddEvaluationCallback(fun self ->
                m.GetOperations(self) |> DeltaSet.toList |> f
            )


        let callbackSet = callbackTable.GetOrCreateValue(set)
        callbackSet.Add result |> ignore
        result

    [<Obsolete("use unsafeRegisterCallbackNoGcRoot or unsafeRegisterCallbackKeepDisposable instead")>]
    let registerCallback f set = unsafeRegisterCallbackNoGcRoot f set

    let private undyingCallbacks = ConcurrentHashSet<IDisposable>()

    /// <summary>
    /// registers a callback for execution whenever the
    /// set's value might have changed and returns a disposable
    /// subscription in order to unregister the callback.
    /// Note that the callback will be executed immediately
    /// once here.
    /// In contrast to registerCallbackNoGcRoot, this function holds on to the
    /// fresh disposable, i.e. even if the input set goes out of scope,
    /// the disposable still forces the complete computation to exist.
    /// When disposing the assosciated disposable, the gc root disappears and
    /// the computation can be collected.
    /// </summary>
    let unsafeRegisterCallbackKeepDisposable (f : list<SetDelta<'a>> -> unit) (set : aset<'a>) =
        let d = unsafeRegisterCallbackNoGcRoot f set
        undyingCallbacks.Add d |> ignore
        { new IDisposable with
            member x.Dispose() =
                d.Dispose()
                undyingCallbacks.Remove d |> ignore
        }
