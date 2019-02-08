namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module AMap =


    [<AutoOpen>]
    module Implementation = 
        type EmptyReader<'a, 'b> private() =
            inherit ConstantObject()

            static let instance = new EmptyReader<'a, 'b>() :> IOpReader<_,_>
            static member Instance = instance

            interface IOpReader<hdeltamap<'a, 'b>> with
                member x.Dispose() =
                    ()

                member x.GetOperations caller =
                    HDeltaMap.empty

            interface IOpReader<hmap<'a, 'b>, hdeltamap<'a, 'b>> with
                member x.State = HMap.empty

        type EmptyMap<'a, 'b> private() =
            let content = Mod.constant HMap.empty

            static let instance = EmptyMap<'a, 'b>() :> amap<_,_>

            static member Instance = instance
            
            override x.ToString() = HMap.empty.ToString()

            interface amap<'a, 'b> with
                member x.IsConstant = true
                member x.Content = content
                member x.GetReader() = EmptyReader.Instance

        type ConstantMap<'a, 'b>(content : Lazy<hmap<'a, 'b>>) =
            let deltas = lazy ( HMap.computeDelta HMap.empty content.Value )
            let mcontent = ConstantMod<hmap<'a, 'b>>(content) :> IMod<_>

            interface amap<'a, 'b> with
                member x.IsConstant = true
                member x.GetReader() = new History.Readers.ConstantReader<_,_>(HMap.trace, deltas, content) :> IMapReader<_,_>
                member x.Content = mcontent
        
            override x.ToString() = content.Value.ToString()

            new(content : hmap<'a, 'b>) = ConstantMap<'a, 'b>(System.Lazy<hmap<'a, 'b>>.CreateFromValue content)

        type AdaptiveMap<'a, 'b>(newReader : unit -> IOpReader<hdeltamap<'a, 'b>>) =
            let h = History.ofReader HMap.trace newReader
            interface amap<'a, 'b> with
                member x.IsConstant = false
                member x.Content = h :> IMod<_>
                member x.GetReader() = h.NewReader()
                
        let inline amap (f : Ag.Scope -> #IOpReader<hdeltamap<'a, 'b>>) =
            let scope = Ag.getContext()
            AdaptiveMap<'a, 'b>(fun () -> f scope :> IOpReader<_>) :> amap<_,_>
            
        let inline constant (l : Lazy<hmap<'a, 'b>>) =
            ConstantMap<'a, 'b>(l) :> amap<_,_>

    module Readers =
        
        type MapReader<'k, 'a, 'b>(scope : Ag.Scope, input : amap<'k, 'a>, f : 'k -> 'a -> 'b) =
            inherit AbstractReader<hdeltamap<'k, 'b>>(scope, HDeltaMap.monoid)

            let reader = input.GetReader()

            override x.Release() =
                reader.Dispose()

            override x.Compute(token) =
                let ops = reader.GetOperations token
                ops |> HMap.map (fun k op ->
                    match op with
                        | Set v -> Set (f k v)
                        | Remove -> Remove
                )

        type ChooseReader<'k, 'a, 'b>(scope : Ag.Scope, input : amap<'k, 'a>, f : 'k -> 'a -> Option<'b>) =
            inherit AbstractReader<hdeltamap<'k, 'b>>(scope, HDeltaMap.monoid)

            let reader = input.GetReader()
            let cache = Dict<'k, bool>()

            override x.Release() =
                reader.Dispose()

            override x.Compute(token) =
                let ops = reader.GetOperations token
                ops |> HMap.choose (fun k op ->
                    match op with
                        | Set v -> 
                            match f k v with
                                | Some n ->
                                    cache.[k] <- true
                                    Some (Set n)
                                | None ->
                                    let wasExisting =
                                        match cache.TryGetValue k with
                                            | (true, v) -> v
                                            | _ -> false

                                    cache.[k] <- false
                                    if wasExisting then
                                        Some Remove
                                    else
                                        None
                        | Remove -> 
                            match cache.TryRemove k with
                                | (true, wasExisting) ->
                                    if wasExisting then Some Remove
                                    else None
                                | _ ->
                                    None
                                    //failwithf "[AMap] could not remove entry %A" k
                )

        type KeyedMod<'k, 'a>(key : 'k, m : IMod<'a>) =
            inherit Mod.AbstractMod<'k * 'a>()

            let mutable last = None
            
            member x.Key = key
            
            member x.UnsafeLast =
                last

            override x.Compute(token) =
                let v = m.GetValue(token)
                last <- Some v
                key, v

        type ChooseMReader<'k, 'a, 'b>(scope : Ag.Scope, input : amap<'k, 'a>, f : 'k -> 'a -> IMod<Option<'b>>) =
            inherit AbstractDirtyReader<KeyedMod<'k, Option<'b>>, hdeltamap<'k, 'b>>(scope, HDeltaMap.monoid)
            
            let reader = input.GetReader()
            let cache = Dict<'k, KeyedMod<'k, Option<'b>>>()

            override x.Release() =
                reader.Dispose()
                cache.Clear()
            
            override x.Compute(token, dirty) =
                let ops = reader.GetOperations token

                let mutable ops =
                    ops |> HMap.choose (fun k op ->
                        match op with
                            | Set v ->
                                let mutable o = Unchecked.defaultof<_>
                                let hadOld = cache.TryGetValue(k, &o)
                                if hadOld then
                                    o.Outputs.Remove x |> ignore
                                    dirty.Remove o |> ignore

                                    
                                let n = KeyedMod(k, f k v)
                                cache.[k] <- n
                                let _,v = n.GetValue(token)
                                match v with
                                    | Some v -> Some (Set v)
                                    | None ->
                                        if hadOld then Some Remove
                                        else None
                            | Remove ->
                                match cache.TryRemove k with
                                    | (true, o) ->
                                        o.Outputs.Remove x |> ignore
                                        dirty.Remove o |> ignore
                                        match o.UnsafeLast with
                                            | None | Some None -> 
                                                None
                                            | Some _ ->
                                                Some Remove
                                                
                                    | _ ->
                                        None
                                        //failwith "[AMap] invalid state"
                    )

                for m in dirty do
                    let last = m.UnsafeLast
                    let k, v = m.GetValue(token)
                    match last, v with
                        | None, None -> ()
                        | None, Some v -> ops <- HMap.add k (Set v) ops


                        | Some None, None -> ()
                        | Some (Some _), None -> ops <- HMap.add k Remove ops
                        | Some None, Some v -> ops <- HMap.add k (Set v) ops
                        | Some (Some _), Some v -> ops <- HMap.add k (Set v) ops
                        
                ops


        type MapMReader<'k, 'x, 'a>(scope : Ag.Scope, input : amap<'k, 'x>, f : 'k -> 'x -> IMod<'a>) =
            inherit AbstractDirtyReader<IMod<'k * 'a>, hdeltamap<'k, 'a>>(scope, HDeltaMap.monoid)

            let reader = input.GetReader()
            let cache = Dict<'k, IMod<'k * 'a>>()

            override x.Compute(token, dirty) =
                let ops = reader.GetOperations token

                let mutable ops =
                    ops |> HMap.choose (fun k op ->
                        match op with
                            | Set v -> 
                                let mutable o = Unchecked.defaultof<_>
                                if cache.TryGetValue(k, &o) then
                                    o.Outputs.Remove x |> ignore
                                    dirty.Remove o |> ignore
                                
                                let n = f k v |> Mod.map (fun v -> k, v)
                                cache.[k] <- n
                                let _,v = n.GetValue(token)
                                Some (Set v)

                            | Remove ->
                                let (worked, o) = cache.TryRemove k
                                if not worked then
                                    None
                                else
                                    o.Outputs.Clear()
                                    dirty.Remove o |> ignore
                                    Some Remove
                    )
                        
                for m in dirty do
                    let k, v = m.GetValue(token)
                    ops <- HMap.add k (Set v) ops

                ops

            override x.Release() =
                reader.Dispose()
                cache.Clear()


        type UpdateReader<'k, 'a>(scope : Ag.Scope, input : amap<'k, 'a>, keys : hset<'k>, f : 'k -> Option<'a> -> 'a) =
            inherit AbstractReader<hdeltamap<'k, 'a>>(scope, HDeltaMap.monoid)
            let reader = input.GetReader()

            let mutable missing = keys

            override x.Compute(token) =
                let ops = reader.GetOperations(token)
                let state = reader.State

                let presentOps =
                    ops |> HMap.map (fun k op ->
                        match op with
                            | Set v ->
                                if HSet.contains k keys then
                                    missing <- HSet.remove k missing
                                    Set (f k (Some v))
                                else
                                    Set v

                            | Remove ->
                                if HSet.contains k keys then
                                    missing <- HSet.add k missing
                                Remove
                    )

                let additionalOps =
                    missing 
                        |> HSet.toSeq
                        |> Seq.map (fun k -> k, Set (f k None))
                        |> HMap.ofSeq

                missing <- HSet.empty
                HDeltaMap.combine presentOps additionalOps

            override x.Release() =
                missing <- keys
                reader.Dispose()

        type UnionWithReader<'k, 'a>(scope : Ag.Scope, l : amap<'k, 'a>, r : amap<'k, 'a>, f : 'k -> 'a -> 'a -> 'a) =
            inherit AbstractReader<hdeltamap<'k, 'a>>(scope, HDeltaMap.monoid)

            let l = l.GetReader()
            let r = r.GetReader()

            override x.Compute(token) =
                let lops = l.GetOperations token
                let rops = r.GetOperations token

                let merge (key : 'k) (lop : Option<ElementOperation<'a>>) (rop : Option<ElementOperation<'a>>) : ElementOperation<'a> =
                    let lv =
                        match lop with
                            | Some (Set lv) -> Some lv
                            | Some (Remove) -> None
                            | None -> HMap.tryFind key l.State
                            
                    let rv =
                        match rop with
                            | Some (Set rv) -> Some rv
                            | Some (Remove) -> None
                            | None -> HMap.tryFind key r.State


                    match lv, rv with
                        | None, None -> Remove
                        | Some l, None -> Set l
                        | None, Some r -> Set r
                        | Some l, Some r -> Set (f key l r)

                HMap.map2 merge lops rops

            override x.Release() =
                l.Dispose()
                r.Dispose()

        type Choose2Reader<'k, 'a, 'b, 'c>(scope : Ag.Scope, l : amap<'k, 'a>, r : amap<'k, 'b>, f : 'k -> Option<'a> -> Option<'b> -> Option<'c>) =
            inherit AbstractReader<hdeltamap<'k, 'c>>(scope, HDeltaMap.monoid)

            let l = l.GetReader()
            let r = r.GetReader()

            override x.Compute(token) =
                let lops = l.GetOperations token
                let rops = r.GetOperations token
                
                let merge (key : 'k) (lop : Option<ElementOperation<'a>>) (rop : Option<ElementOperation<'b>>) : ElementOperation<'c> =
                    let lv =
                        match lop with
                            | Some (Set lv) -> Some lv
                            | Some (Remove) -> None
                            | None -> HMap.tryFind key l.State
                            
                    let rv =
                        match rop with
                            | Some (Set rv) -> Some rv
                            | Some (Remove) -> None
                            | None -> HMap.tryFind key r.State

                    match lv, rv with
                        | None, None -> Remove
                        | _ ->
                            match f key lv rv with
                                | Some c -> Set c
                                | None -> Remove

                HMap.map2 merge lops rops


            override x.Release() =
                l.Dispose()
                r.Dispose()

        type MapSetReader<'a, 'b>(scope : Ag.Scope, set : aset<'a>, f : 'a -> 'b) =
            inherit AbstractReader<hdeltamap<'a, 'b>>(scope, HDeltaMap.monoid)

            let r = set.GetReader()

            override x.Compute(token) =
                r.GetOperations token
                    |> HDeltaSet.toHMap
                    |> HMap.choose (fun key v ->
                        if v > 0 then Some (Set (f key))
                        elif v < 0 then Some Remove
                        else None
                    )

            override x.Release() =
                r.Dispose()
     
        type OfASetReader<'a, 'b>(scope : Ag.Scope, set : aset<'a * 'b>) =
            inherit AbstractReader<hmap<'a, hset<'b>>, hdeltamap<'a, hset<'b>>>(scope, HMap.trace)

            let r = set.GetReader()

            override x.Compute(token) =
                let mutable state = x.State
                let mutable deltas = HMap.empty

                let ops = r.GetOperations token
                for op in ops do
                    match op with
                        | Add (_,(a,b)) ->
                            state <- 
                                state |> HMap.update a (fun ob ->
                                    let newState = 
                                        match ob with
                                            | None -> HSet.ofList [b]
                                            | Some ob -> HSet.add b ob
                                    deltas <- HMap.add a (Set newState) deltas
                                    newState
                                )
                        | Rem(_,(a,b)) ->
                            state <-    
                                state |> HMap.alter a (fun ob ->
                                    match ob with
                                        | None -> 
                                            Log.warn "[AMap] strange"
                                            None
                                        | Some ob ->
                                            let newSet = HSet.remove b ob
                                            if HSet.isEmpty newSet then 
                                                deltas <- HMap.add a Remove deltas
                                            else
                                                deltas <- HMap.add a (Set newSet) deltas
                                            Some newSet
                                )         

                deltas

            override x.Release() =
                r.Dispose()

        type GroupByReader<'a, 'b, 'c>(scope : Ag.Scope, set : aset<'a>, f : 'a -> 'b * 'c) =
            inherit AbstractReader<hmap<'b, hset<'c>>, hdeltamap<'b, hset<'c>>>(scope, HMap.trace)

            let r = set.GetReader()
            let f = Cache f

            override x.Compute(token) =
                let mutable state = x.State
                let mutable deltas = HMap.empty
                let ops = r.GetOperations token

                for op in ops do
                    match op with
                        | Add(_,a) ->
                            let (b,c) = f.Invoke a
                            state <- 
                                state |> HMap.update b (fun oc ->
                                    let newState = 
                                        match oc with
                                            | None -> HSet.ofList [c]
                                            | Some oc -> HSet.add c oc
                                    deltas <- HMap.add b (Set newState) deltas
                                    newState
                                )
                        | Rem(_,a) ->
                            let (b,c) = f.Revoke a
                            state <-    
                                state |> HMap.alter b (fun oc ->
                                    match oc with
                                        | None -> 
                                            Log.warn "[AMap] strange"
                                            None
                                        | Some oc ->
                                            let newSet = HSet.remove c oc
                                            if HSet.isEmpty newSet then 
                                                deltas <- HMap.add b Remove deltas
                                            else
                                                deltas <- HMap.add b (Set newSet) deltas
                                            Some newSet
                                )       
                            
                deltas

            override x.Release() =
                r.Dispose()
                f.Clear ignore
            





            

        type OfModReader<'a, 'b>(scope : Ag.Scope, input : IMod<hmap<'a, 'b>>) =
            inherit AbstractReader<hmap<'a, 'b>, hdeltamap<'a, 'b>>(scope, HMap.trace)

            override x.Compute(token) =
                input.GetValue token
                    |> HMap.computeDelta x.State

            override x.Release() =
                ()

        type BindReader<'a, 'k, 'v>(scope : Ag.Scope, input : IMod<'a>, f : 'a -> amap<'k, 'v>) =
            inherit AbstractReader<hdeltamap<'k, 'v>>(scope, HDeltaMap.monoid)

            let mutable oldValue : Option<'a * IMapReader<'k, 'v>> = None

            override x.Compute(token) =
                let v = input.GetValue token
            
                match oldValue with
                    | Some (ov, r) when Unchecked.equals ov v ->
                        r.GetOperations(token)
                    | _ ->
                        let rem =
                            match oldValue with
                                | Some (_, oldReader) ->
                                    let res = HMap.computeDelta oldReader.State HMap.empty
                                    oldReader.Dispose()
                                    oldReader.Outputs.Remove x |> ignore
                                    res
                                | _ ->
                                    HMap.empty
                                
                        let newMap = f v
                        let newReader = newMap.GetReader()
                        oldValue <- Some(v, newReader)
                        let add = newReader.GetOperations token

                        HDeltaMap.combine rem add

            override x.Release() =
                match oldValue with
                    | Some (_,r) ->
                        r.Dispose()
                        oldValue <- None
                    | None ->
                        ()

    let empty<'k, 'v> = EmptyMap<'k, 'v>.Instance

    let ofHMap (map : hmap<'k, 'v>) = ConstantMap(map) :> amap<_,_>
    let ofSeq (seq : seq<'k * 'v>) = ConstantMap(HMap.ofSeq seq) :> amap<_,_>
    let ofList (list : list<'k * 'v>) = ConstantMap(HMap.ofList list) :> amap<_,_>
    let ofArray (arr : array<'k * 'v>) = ConstantMap(HMap.ofArray arr) :> amap<_,_>


    let map (mapping : 'k -> 'a -> 'b) (map : amap<'k, 'a>) =
        if map.IsConstant then
            constant <| lazy ( map.Content |> Mod.force |> HMap.map mapping )
        else
            amap <| fun scope -> new Readers.MapReader<'k, 'a, 'b>(scope, map, mapping)

    let choose (mapping : 'k -> 'a -> Option<'b>) (map : amap<'k, 'a>) =
        if map.IsConstant then
            constant <| lazy ( map.Content |> Mod.force |> HMap.choose mapping )
        else
            amap <| fun scope -> new Readers.ChooseReader<'k, 'a, 'b>(scope, map, mapping)
        
    let filter (predicate : 'k -> 'a -> bool) (map : amap<'k, 'a>) =
        choose (fun k v -> if predicate k v then Some v else None) map

    let toMod (map : amap<'k, 'v>) = map.Content

    let toASet (m : amap<'k, 'v>) : aset<'k * 'v> =
        ASet.create (fun scope ->
            let r = m.GetReader()

            { new AbstractReader<hdeltaset<'k * 'v>>(scope, HDeltaSet.monoid) with
                
                member x.Compute(token) =
                    let oldState = r.State
                    let ops = r.GetOperations token

                    let mutable deltas = HDeltaSet.empty

                    for (k,op) in ops do
                        match op with
                            | Set v ->
                                match HMap.tryFind k oldState with
                                    | Some ov ->
                                        deltas <- HDeltaSet.add (Rem(k,ov)) deltas
                                    | None ->
                                        ()
                                deltas <- HDeltaSet.add (Add(k, v)) deltas

                            | Remove ->
                                // NOTE: As it is not clear at what point the toASet computation has been evaluated last, it is 
                                //       a valid case that something is removed that is not present in the current local state.
                                deltas <-
                                    match HMap.tryFind k oldState with
                                    | Some ov ->
                                        HDeltaSet.add (Rem (k, ov)) deltas
                                    | None ->
                                        deltas


                    deltas
                member x.Release() =
                    r.Dispose()
            }
        )

    let bind (mapping : 'a -> amap<'k, 'v>) (m : IMod<'a>) =
        if m.IsConstant then
            mapping (Mod.force m)
        else
            amap <| fun scope -> new Readers.BindReader<'a, 'k, 'v>(scope, m, mapping)

    let mapM (mapping : 'k -> 'a -> IMod<'b>) (map : amap<'k, 'a>) =
        amap <| fun scope -> new Readers.MapMReader<'k, 'a, 'b>(scope, map, mapping)

    let reduce (map : amap<'k, 'a>) =
       map

    let reduceM (map : amap<'k, IMod<'a>>) =
       mapM (fun k v -> v) map

    let chooseM (mapping : 'k -> 'a -> IMod<Option<'b>>) (map : amap<'k, 'a>) =
        amap <| fun scope -> new Readers.ChooseMReader<'k, 'a, 'b>(scope, map, mapping)

    let filterM (predicate : 'k -> 'a -> IMod<bool>) (map : amap<'k, 'a>) =
        chooseM (fun k v -> predicate k v |> Mod.map (function true -> Some v | false -> None)) map
              
    let flatten (map : amap<'k, Option<'a>>) : amap<'k, 'a> =
       choose (fun k v -> v) map

    let flattenM (map : amap<'k, IMod<Option<'a>>>) : amap<'k, 'a> =
       chooseM (fun k v -> v) map

    let mapSet (f : 'a -> 'b) (set : aset<'a>) : amap<'a, 'b> =
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> HRefSet.toHMap |> HMap.map (fun k _ -> f k) )
        else
            amap <| fun scope -> new Readers.MapSetReader<'a, 'b>(scope, set, f)

    let ofASet (set : aset<'a * 'b>) : amap<'a, hset<'b>> =
        if set.IsConstant then
            constant <| lazy ( set.Content |> Mod.force |> HRefSet.toList |> List.groupBy (fun (a,_) -> a :> obj) |> List.map (fun (k,kvs) -> unbox<'a> k, kvs |> List.map snd |> HSet.ofList ) |> HMap.ofList )
        else
            amap <| fun scope -> new Readers.OfASetReader<'a, 'b>(scope, set)

    let single (k : 'k) (v : 'v) = (k,v) |> ASet.single |> ofASet

    let chooseSet (f : 'a -> Option<'b>) (set : aset<'a>) : amap<'a, 'b> =
        set |> mapSet f |> flatten

    let mapSetM (f : 'a -> IMod<'b>) (set : aset<'a>) : amap<'a, 'b> =
        set |> mapSet f |> reduceM

    let chooseSetM (f : 'a -> IMod<Option<'b>>) (set : aset<'a>) : amap<'a, 'b> =
        set |> mapSet f |> flattenM

    let ofMod (m : IMod<hmap<'a, 'b>>) : amap<'a, 'b> =
        if m.IsConstant then
            constant <| lazy (Mod.force m)
        else
            amap <| fun scope -> new Readers.OfModReader<'a, 'b>(scope, m)


    let updateMany (keys : hset<'k>) (f : 'k -> Option<'v> -> 'v) (m : amap<'k, 'v>) =
        amap <| fun scope -> new Readers.UpdateReader<'k, 'v>(scope, m, keys, f)
        



    let unionWith (f : 'k -> 'a -> 'a -> 'a) (l : amap<'k, 'a>) (r : amap<'k, 'a>) =
        if l.IsConstant && r.IsConstant then
            constant <| lazy ( HMap.unionWith f (Mod.force l.Content) (Mod.force r.Content) )
        else
            amap <| fun scope -> new Readers.UnionWithReader<'k, 'a>(scope, l, r, f)

    let union (l : amap<'k, 'a>) (r : amap<'k, 'a>) =
        unionWith (fun _ _ a -> a) l r

    let unionSet (maps : aset<amap<'k, 'v>>) =
        maps |> ASet.collect toASet |> ofASet |> map (fun (k) (v) -> v |> Seq.collect (fun x -> x) |> HSet.ofSeq)
        
    let choose2 (mapping : 'k -> Option<'a> -> Option<'b> -> Option<'c>) (l : amap<'k, 'a>) (r : amap<'k, 'b>) =
        if l.IsConstant && r.IsConstant then
            constant <| lazy ( HMap.choose2 mapping (Mod.force l.Content) (Mod.force r.Content) )
        else
            amap <| fun scope -> new Readers.Choose2Reader<'k, 'a, 'b, 'c>(scope, l, r, mapping)
        
    let map2 (mapping : 'k -> Option<'a> -> Option<'b> -> 'c) (l : amap<'k, 'a>) (r : amap<'k, 'b>) =
        choose2 (fun k l r -> Some (mapping k l r)) l r



    let tryFind (key : 'k) (m : amap<'k, 'v>) =
        let reader = m.GetReader()
        let mutable last = None
        Mod.custom (fun token ->
            let ops = reader.GetOperations token
            match HMap.tryFind key ops with
                | Some Remove -> 
                    last <- None
                    None
                | Some (Set v) -> 
                    last <- Some v
                    Some v
                | None -> 
                    last
        )

    let find (key : 'k) (m : amap<'k, 'v>) =
        let reader = m.GetReader()
        let mutable last = None
        Mod.custom (fun token ->
            let ops = reader.GetOperations token
            match HMap.tryFind key ops with
                | Some Remove -> 
                    failwith "[AMap] key not found"
                | Some (Set v) -> 
                    last <- Some v
                    v
                | None -> 
                    match last with
                        | Some v -> v
                        | None -> failwith "[AMap] key not found"
        )

    let findWithDefault (key : 'k) (def : 'v) (m : amap<'k, 'v>) =
        let reader = m.GetReader()
        let mutable last = def
        Mod.custom (fun token ->
            let ops = reader.GetOperations token
            match HMap.tryFind key ops with
                | Some Remove -> 
                    last <- def
                    def
                | Some (Set v) -> 
                    last <- v
                    v
                | None -> 
                    last
        )
        
[<AutoOpen>]
module ``ASet -> AMap interop`` =
    open AMap.Implementation
    open AMap.Readers

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    [<RequireQualifiedAccess>]
    module ASet =

        let groupBy (f : 'a -> 'g) (s : aset<'a>) : amap<'g, hset<'a>> =
            if s.IsConstant then
                constant <| lazy ( s.Content |> Mod.force |> Seq.groupBy (fun v -> f v :> obj) |> Seq.map (fun (g,vs) -> unbox<'g> g, HSet.ofSeq vs) |> HMap.ofSeq )
            else
                amap <| function scope -> new GroupByReader<'a, 'g, 'a>(scope, s, fun v -> f v, v)

        let groupBy' (f : 'a -> 'g * 'b) (s : aset<'a>) : amap<'g, hset<'b>> =
            if s.IsConstant then
                constant <| lazy ( s.Content |> Mod.force |> Seq.map f |> Seq.groupBy (fun (g,_) -> g :> obj) |> Seq.map (fun (g,vs) -> unbox<'g> g, vs |> Seq.map snd |> HSet.ofSeq) |> HMap.ofSeq )
            else
                amap <| function scope -> new GroupByReader<'a, 'g, 'b>(scope, s, f)



[<AutoOpen>]
module AMapBuilderExperiments =

    type AMapBuilder() =

        member x.Yield(kvp : 'k * 'v) =
            AMap.ofList [kvp]

        member x.Combine(l : amap<'k, 'v>, r : amap<'k, 'v>) : amap<'k, 'v> =
            AMap.union l r

        member x.Bind(m : IMod<'a>, f : 'a -> amap<'k, 'v>) =
            m |> AMap.bind f

        member x.Zero() =
            AMap.empty

        member x.Delay(f : unit -> amap<'k, 'v>) =
            f()

        member x.YieldFrom(m : hmap<'k, 'v>) = 
            AMap.ofHMap m

        member x.YieldFrom(m : Map<'k, 'v>) = 
            AMap.ofHMap (HMap.ofSeq (Map.toSeq m))

        member x.YieldFrom(m : seq<'k * 'v>) = 
            AMap.ofHMap (HMap.ofSeq m)

        member x.YieldFrom(m : amap<'k, 'v>) = 
            m

    type RestrictedAMapBuilder<'k, 'v>() =

        abstract member Resolve : 'k * 'v * 'v -> 'v
        default x.Resolve(_,_,r) = r

        member x.Yield(kvp : 'k * 'v) =
            AMap.ofList [kvp]

        member x.Combine(l : amap<'k, 'v>, r : amap<'k, 'v>) : amap<'k, 'v> =
            AMap.union l r

        member x.Bind(m : IMod<'a>, f : 'a -> amap<'k, 'v>) =
            m |> AMap.bind f

        member x.Zero() =
            AMap.empty

        member x.Delay(f : unit -> amap<'k, 'v>) =
            f()

        member x.YieldFrom(m : hmap<'k, 'v>) = 
            AMap.ofHMap m

        member x.YieldFrom(m : seq<'k * 'v>) = 
            AMap.ofHMap (HMap.ofSeq m)

        member x.YieldFrom(m : amap<'k, 'v>) = 
            m

    let amap = AMapBuilder()