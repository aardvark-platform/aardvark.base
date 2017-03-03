namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module ASet =

    [<AutoOpen>]
    module Implementation = 
        type EmptyReader<'a> private() =
            inherit ConstantObject()

            static let instance = new EmptyReader<'a>() :> IOpReader<_,_>
            static member Instance = instance

            interface IOpReader<hdeltaset<'a>> with
                member x.Dispose() =
                    ()

                member x.GetOperations caller =
                    HDeltaSet.empty

            interface IOpReader<hrefset<'a>, hdeltaset<'a>> with
                member x.State = HRefSet.empty

        type EmptySet<'a> private() =
            let content = Mod.constant HRefSet.empty

            static let instance = EmptySet<'a>() :> aset<_>

            static member Instance = instance


            interface aset<'a> with
                member x.IsConstant = true
                member x.Content = content
                member x.GetReader() = EmptyReader.Instance

        type ConstantSet<'a>(content : Lazy<hrefset<'a>>) =
            let deltas = lazy ( content.Value |> Seq.map Add |> HDeltaSet.ofSeq)
            let mcontent = ConstantMod<hrefset<'a>>(content) :> IMod<_>

            interface aset<'a> with
                member x.IsConstant = true
                member x.GetReader() = new History.Readers.ConstantReader<_,_>(HRefSet.trace, deltas, content) :> ISetReader<_>
                member x.Content = mcontent
        
            new(content : hrefset<'a>) = ConstantSet<'a>(Lazy.CreateFromValue content)

        type AdaptiveSet<'a>(newReader : unit -> IOpReader<hdeltaset<'a>>) =
            let h = History.ofReader HRefSet.trace newReader

            interface aset<'a> with
                member x.IsConstant = false
                member x.Content = h :> IMod<_>
                member x.GetReader() = h.NewReader()

        let inline unexpected() =
            failwith "[ASet] deltas are expected to be unique"

        let inline aset (f : Ag.Scope -> #IOpReader<hdeltaset<'a>>) =
            let scope = Ag.getContext()
            AdaptiveSet<'a>(fun () -> f scope :> IOpReader<_>) :> aset<_>

        let inline constant (l : Lazy<hrefset<'a>>) =
            ConstantSet<'a>(l) :> aset<_>

    [<AutoOpen>]
    module Readers =
        type MapReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> 'b) =
            inherit AbstractReader<hdeltaset<'b>>(scope, HDeltaSet.monoid)
            
            let cache = Cache f
            let r = input.GetReader()

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute(token) =
                r.GetOperations token |> HDeltaSet.map (fun d ->
                    match d with
                        | Add(1, v) -> Add(cache.Invoke v)
                        | Rem(1, v) -> Rem(cache.Revoke v)
                        | _ -> unexpected()
                )

        type ChooseReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> Option<'b>) =
            inherit AbstractReader<hdeltaset<'b>>(scope, HDeltaSet.monoid)
            
            let cache = Cache f
            let r = input.GetReader()

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute(token) =
                r.GetOperations token |> HDeltaSet.choose (fun d ->
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
            inherit AbstractReader<hdeltaset<'a>>(scope, HDeltaSet.monoid)
            
            let cache = Cache f
            let r = input.GetReader()

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute(token) =
                r.GetOperations token |> HDeltaSet.choose (fun d ->
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
            inherit AbstractDirtyReader<ISetReader<'a>, hdeltaset<'a>>(scope, HDeltaSet.monoid)

            let r = input.GetReader()
            let cache = Cache(fun (a : aset<'a>) -> a.GetReader())

            override x.Release() =
                r.Dispose()
                cache.Clear (fun r -> r.Dispose())

            override x.Compute(token,dirty) =
                let mutable deltas = 
                    r.GetOperations token |> HDeltaSet.collect (fun d ->
                        match d with
                            | Add(1, v) ->
                                let r = cache.Invoke v
                                dirty.Remove r |> ignore
                                r.GetOperations token

                            | Rem(1, v) -> 
                                let deleted, r = cache.RevokeAndGetDeleted v
                                dirty.Remove r |> ignore
                                if deleted then 
                                    let ops = HRefSet.computeDelta r.State HRefSet.empty
                                    r.Dispose()
                                    ops
                                else
                                    r.GetOperations token
                                
                            | _ -> unexpected()
                    )

                for d in dirty do
                    deltas <- HDeltaSet.combine deltas (d.GetOperations token)

                deltas

        type UnionFixedReader<'a>(scope : Ag.Scope, input : hrefset<aset<'a>>) =
            inherit AbstractDirtyReader<ISetReader<'a>, hdeltaset<'a>>(scope, HDeltaSet.monoid)

            let mutable initial = true
            let input = input |> HRefSet.map (fun s -> s.GetReader())

            override x.Release() =
                for i in input do
                    i.Dispose()

            override x.Compute(token,dirty) =
                if initial then
                    initial <- false
                    input |> HRefSet.fold (fun deltas r -> HDeltaSet.combine deltas (r.GetOperations token)) HDeltaSet.empty
                else
                    dirty |> Seq.fold (fun deltas r -> HDeltaSet.combine deltas (r.GetOperations token)) HDeltaSet.empty

        type DifferenceReader<'a>(scope : Ag.Scope, l : aset<'a>, r : aset<'a>) =
            inherit AbstractReader<hdeltaset<'a>>(scope, HDeltaSet.monoid)
            
            let l = l.GetReader()
            let r = r.GetReader()

            override x.Release() =
                l.Dispose()
                r.Dispose()

            override x.Compute(token) =
                let lops = l.GetOperations token
                let rops = r.GetOperations token

                let rops = HDeltaSet.map SetOperation.inverse rops

                HDeltaSet.combine lops rops




        type CollectReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> aset<'b>) =
            inherit AbstractDirtyReader<ISetReader<'b>, hdeltaset<'b>>(scope, HDeltaSet.monoid)

            let r = input.GetReader()
            let cache = Cache(fun a -> (f a).GetReader())

            override x.Release() =
                r.Dispose()
                cache.Clear (fun r -> r.Dispose())

            override x.Compute(token,dirty) =
                let mutable deltas = 
                    r.GetOperations token |> HDeltaSet.collect (fun d ->
                        match d with
                            | Add(1, v) ->
                                let r = cache.Invoke v
                                dirty.Remove r |> ignore
                                r.GetOperations token

                            | Rem(1, v) -> 
                                let deleted, r = cache.RevokeAndGetDeleted v
                                dirty.Remove r |> ignore
                                if deleted then 
                                    let ops = HRefSet.computeDelta r.State HRefSet.empty
                                    r.Dispose()
                                    ops
                                else
                                    r.GetOperations token
                                
                            | _ -> unexpected()
                    )

                for d in dirty do
                    deltas <- HDeltaSet.combine deltas (d.GetOperations token)

                deltas

        type CollectSetReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> hrefset<'b>) =
            inherit AbstractReader<hdeltaset<'b>>(scope, HDeltaSet.monoid)
            
            let r = input.GetReader()
            let cache = Cache f

            override x.Release() =
                r.Dispose()
                cache.Clear ignore

            override x.Compute(token) =
                r.GetOperations token |> HDeltaSet.collect (fun d ->
                    match d with
                        | Add(1,v) -> 
                            HRefSet.computeDelta HRefSet.empty (cache.Invoke v)
                        | Rem(1,v) ->
                            HRefSet.computeDelta (cache.Revoke v) HRefSet.empty
                        | _ ->
                            unexpected()
                )


        type ModSetReader<'a>(scope : Ag.Scope, input : IMod<hrefset<'a>>) =
            inherit AbstractReader<hdeltaset<'a>>(scope, HDeltaSet.monoid)

            let mutable old = HRefSet.empty

            override x.Release() =
                lock input (fun () -> input.Outputs.Remove x |> ignore)

            override x.Compute(token) =
                let n = input.GetValue token
                let deltas = HRefSet.computeDelta old n
                old <- n
                deltas

        type ModValueReader<'a>(scope : Ag.Scope, input : IMod<'a>) =
            inherit AbstractReader<hdeltaset<'a>>(scope, HDeltaSet.monoid)

            let mutable old = None

            override x.Release() =
                lock input (fun () -> input.Outputs.Remove x |> ignore)
                old <- None

            override x.Compute(token) =
                let n = input.GetValue token
                let delta = 
                    match old with
                        | None -> HDeltaSet.ofList [Add n]
                        | Some o when Object.Equals(o, n) -> HDeltaSet.empty
                        | Some o -> HDeltaSet.ofList [Rem o; Add n]
                old <- Some n
                delta

        type BindReader<'a, 'b>(scope : Ag.Scope, input : IMod<'a>, f : 'a -> aset<'b>) =
            inherit AbstractReader<hdeltaset<'b>>(scope, HDeltaSet.monoid)
            
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

            override x.Compute(token) =
                let v = input.GetValue token
                match old with
                    | Some(_,ro) when inputChanged ->
                        ro.Dispose()
                        let r = (f v).GetReader()
                        old <- Some(v, r)
                        r.GetOperations token

                    | Some(vo, ro) ->
                        ro.GetOperations token

                    | None ->
                        let r = (f v).GetReader()
                        old <- Some(v, r)
                        r.GetOperations token

        type CustomReader<'a>(scope : Ag.Scope, compute : AdaptiveToken -> hrefset<'a> -> hdeltaset<'a>) =
            inherit AbstractReader<hrefset<'a>, hdeltaset<'a>>(scope, HRefSet.trace)
            
            override x.Release() =
                ()

            override x.Compute(token) =
                compute token x.State
            
        type FlattenReader<'a>(scope : Ag.Scope, input : aset<IMod<'a>>) =
            inherit AbstractDirtyReader<IMod<'a>, hdeltaset<'a>>(scope, HDeltaSet.monoid)
            
            let r = input.GetReader()

            let mutable initial = true
            let cache = Dict<IMod<'a>, 'a>()

            member x.Invoke(token : AdaptiveToken, m : IMod<'a>) =
                let v = m.GetValue token
                cache.[m] <- v
                v

            member x.Invoke2(token : AdaptiveToken, m : IMod<'a>) =
                let o = cache.[m]
                let v = m.GetValue token
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

            override x.Compute(token, dirty) =
                let mutable deltas = 
                    r.GetOperations token |> HDeltaSet.map (fun d ->
                        match d with
                            | Add(1,m) -> Add(x.Invoke(token, m))
                            | Rem(1,m) -> Rem(x.Revoke(m))
                            | _ -> unexpected()
                    )

                for d in dirty do
                    let o, n = x.Invoke2(token, d)
                    if not <| Object.Equals(o,n) then
                        deltas <- HDeltaSet.combine deltas (HDeltaSet.ofList [Add n; Rem o])

                deltas
            
        type MapMReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> IMod<'b>) =
            inherit AbstractDirtyReader<IMod<'b>, hdeltaset<'b>>(scope, HDeltaSet.monoid)
            
            let r = input.GetReader()

            let f = Cache f
            let mutable initial = true
            let cache = Dict<IMod<'b>, 'b>()

            member x.Invoke(token : AdaptiveToken, v : 'a) =
                let m = f.Invoke v
                let v = m.GetValue token
                cache.[m] <- v
                v

            member x.Invoke2(token : AdaptiveToken, m : IMod<'b>) =
                let o = cache.[m]
                let v = m.GetValue token
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

            override x.Compute(token, dirty) =
                let mutable deltas = 
                    r.GetOperations token |> HDeltaSet.map (fun d ->
                        match d with
                            | Add(1,m) -> Add(x.Invoke(token,m))
                            | Rem(1,m) -> Rem(x.Revoke m)
                            | _ -> unexpected()
                    )

                for d in dirty do
                    let o, n = x.Invoke2(token, d)
                    if not <| Object.Equals(o,n) then
                        deltas <- HDeltaSet.combine deltas (HDeltaSet.ofList [Add n; Rem o])

                deltas

        type ChooseMReader<'a, 'b>(scope : Ag.Scope, input : aset<'a>, f : 'a -> IMod<Option<'b>>) =
            inherit AbstractDirtyReader<IMod<Option<'b>>, hdeltaset<'b>>(scope, HDeltaSet.monoid)
            
            let r = input.GetReader()

            let f = Cache f
            let mutable initial = true
            let cache = Dict<IMod<Option<'b>>, Option<'b>>()

            member x.Invoke(token : AdaptiveToken, v : 'a) =
                let m = f.Invoke v
                let v = m.GetValue token
                cache.[m] <- v
                v

            member x.Invoke2(token : AdaptiveToken, m : IMod<Option<'b>>) =
                let o = cache.[m]
                let v = m.GetValue token
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

            override x.Compute(token, dirty) =
                let mutable deltas = 
                    r.GetOperations token |> HDeltaSet.choose (fun d ->
                        match d with
                            | Add(1,m) -> 
                                match x.Invoke(token,m) with
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
                        match x.Invoke2(token, d) with
                            | None, None -> 
                                HDeltaSet.empty

                            | None, Some v ->
                                HDeltaSet.single (Add v)

                            | Some o, None ->
                                HDeltaSet.single (Rem o)

                            | Some o, Some n ->
                                if Object.Equals(o, n) then
                                    HDeltaSet.empty
                                else
                                    HDeltaSet.ofList [Rem o; Add n]

                    deltas <- HDeltaSet.combine deltas change

                deltas

    // =====================================================================================
    // CREATORS (of*)
    // =====================================================================================

    /// the empty aset
    let empty<'a> = EmptySet<'a>.Instance

    /// creates a new aset containing only the given element
    let single (v : 'a) =
        ConstantSet(HRefSet.single v) :> aset<_>

    /// creates a new aset using the given set content
    let ofSet (set : hrefset<'a>) =
        ConstantSet(set) :> aset<_>

    /// create a new aset using all distinct entries from the sequence
    let ofSeq (seq : seq<'a>) =
        seq |> HRefSet.ofSeq |> ofSet
        
    /// create a new aset using all distinct entries from the list
    let ofList (list : list<'a>) =
        list |> HRefSet.ofList |> ofSet
        
    /// create a new aset using all distinct entries from the array
    let ofArray (arr : 'a[]) =
        arr |> HRefSet.ofArray |> ofSet

    /// creates set which will always contain the elements given by the mod-cell
    let ofMod (m : IMod<hrefset<'a>>) =
        if m.IsConstant then
            constant <| lazy ( Mod.force m )
        else
            aset <| fun scope -> new ModSetReader<'a>(scope, m)
            
    /// creates a singleton set which will always contain the latest value of the given mod-cell
    let ofModSingle (m : IMod<'a>) =
        if m.IsConstant then
            constant <| lazy ( m |> Mod.force |> HRefSet.single )
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
        set.Content |> Mod.force |> HRefSet.toList
        
    /// creates an array from the current state of the aset
    let toArray (set : aset<'a>) =
        set.Content |> Mod.force |> HRefSet.toArray

    /// creates a mod-cell containing the set's content as set
    let toMod (s : aset<'a>) =
        s.Content


    // =====================================================================================
    // OPERATIONS
    // =====================================================================================

    let union (l : aset<'a>) (r : aset<'a>) =
        if l.IsConstant && r.IsConstant then
            constant <| lazy ( HRefSet.union (Mod.force l.Content) (Mod.force r.Content) )
        else
            aset <| fun scope -> new UnionFixedReader<'a>(scope, HRefSet.ofList [l; r])

    let difference (l : aset<'a>) (r : aset<'a>) =
        if l.IsConstant && r.IsConstant then
            constant <| lazy ( HRefSet.difference (Mod.force l.Content) (Mod.force r.Content) )
        else
            aset <| fun scope -> new DifferenceReader<'a>(scope, l, r)

    let unionMany' (sets : seq<aset<'a>>) =
        let sets = HRefSet.ofSeq sets
        if sets |> Seq.forall (fun s -> s.IsConstant) then
            constant <| lazy ( sets |> HRefSet.collect (fun s -> s.Content |> Mod.force) )
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
            constant <| lazy ( set.Content |> Mod.force |> HRefSet.map mapping )
        else
            aset <| fun scope -> new MapReader<'a, 'b>(scope, set, mapping)
        
    /// applies the given function to each element of the given aset. returns an aset comprised of the results x for each element
    /// where the function returns Some(x)
    let choose (chooser : 'a -> Option<'b>) (set : aset<'a>) =
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> HRefSet.choose chooser )
        else
            aset <| fun scope -> new ChooseReader<'a, 'b>(scope, set, chooser)

    /// creates a new aset containing only the elements of the given one for which the given predicate returns true
    let filter (predicate : 'a -> bool) (set : aset<'a>) =
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> HRefSet.filter predicate )
        else
            aset <| fun scope -> new FilterReader<'a>(scope, set, predicate)

    /// applies the given function to each element of the given aset. unions all the results and returns the combined aset
    let collect (mapping : 'a -> aset<'b>) (set : aset<'a>) =
        if set.IsConstant then
            set.Content |> Mod.force |> HRefSet.map mapping |> unionMany'
        else
            aset <| fun scope -> new CollectReader<'a, 'b>(scope, set, mapping)
        
    /// applies the given function to each element of the given aset. unions all the results and returns the combined aset
    let collect' (mapping : 'a -> #seq<'b>) (set : aset<'a>) =
        let mapping = mapping >> HRefSet.ofSeq
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> HRefSet.collect mapping )
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

        let rec traverse (d : list<SetOperation<'a>>) =
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

            let worked = traverse (HDeltaSet.toList ops)

            if not worked then
                res <- r.State |> HRefSet.fold add zero
                
            res
        )

    let fold (f : 's -> 'a -> 's) (seed : 's) (s : aset<'a>) =
        foldHalfGroup f (fun _ _ -> None) seed s

    let foldGroup (add : 's -> 'a -> 's) (sub : 's -> 'a -> 's) (zero : 's) (s : aset<'a>) =
        foldHalfGroup add (fun a b -> Some (sub a b)) zero s
       
       
    let containsAll (seq : seq<'a>) (set : aset<'a>) =
        set.Content |> Mod.map (fun set ->
            seq |> Seq.forall (fun v -> HRefSet.contains v set)
        )   
        
    let containsAny (seq : seq<'a>) (set : aset<'a>) =
        set.Content |> Mod.map (fun set ->
            seq |> Seq.exists (fun v -> HRefSet.contains v set)
        )   

    /// Adaptively calculates the sum of all elements in the set
    let inline sum (s : aset<'a>) = foldGroup (+) (-) LanguagePrimitives.GenericZero s

    /// Adaptively calculates the product of all elements in the set
    let inline product (s : aset<'a>) = foldGroup (*) (/) LanguagePrimitives.GenericOne s


    let mapUse<'a, 'b when 'b :> IDisposable> (f : 'a -> 'b) (set : aset<'a>) : aset<'b> =
        failwith "not implemented"



    /// creates a new aset using the given reader-creator
    let create (f : Ag.Scope -> #IOpReader<hdeltaset<'a>>) =
        aset f

    let custom (f : AdaptiveToken -> hrefset<'a> -> hdeltaset<'a>) =
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

    let unsafeRegisterCallbackNoGcRoot (f : list<SetOperation<'a>> -> unit) (set : aset<'a>) =
        let m = set.GetReader()

        let result =
            m.AddEvaluationCallback(fun self ->
                m.GetOperations(self) |> HDeltaSet.toList |> f
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
    let unsafeRegisterCallbackKeepDisposable (f : list<SetOperation<'a>> -> unit) (set : aset<'a>) =
        let d = unsafeRegisterCallbackNoGcRoot f set
        undyingCallbacks.Add d |> ignore
        { new IDisposable with
            member x.Dispose() =
                d.Dispose()
                undyingCallbacks.Remove d |> ignore
        }
