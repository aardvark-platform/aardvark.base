namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module AList =


    module Comparer =
        let ofFunction (cmp : 'a -> 'a -> int) =
            let cmp = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(cmp)
            { new IComparer<'a> with
                member x.Compare(l,r) = cmp.Invoke(l,r)
            }

    
    [<AutoOpen>]
    module Implementation = 
        type EmptyReader<'a> private() =
            inherit ConstantObject()

            static let instance = new EmptyReader<'a>() :> IOpReader<_,_>
            static member Instance = instance

            interface IOpReader<pdeltalist<'a>> with
                member x.Dispose() =
                    ()

                member x.GetOperations caller =
                    PDeltaList.empty

            interface IOpReader<plist<'a>, pdeltalist<'a>> with
                member x.State = PList.empty

        type EmptyList<'a> private() =
            let content = Mod.constant PList.empty

            static let instance = EmptyList<'a>() :> alist<_>

            static member Instance = instance

            interface alist<'a> with
                member x.IsConstant = true
                member x.Content = content
                member x.GetReader() = EmptyReader.Instance

        type ConstantList<'a>(content : Lazy<plist<'a>>) =
            let deltas = lazy ( plist.ComputeDeltas(PList.empty, content.Value) )
            let mcontent = ConstantMod<plist<'a>>(content) :> IMod<_>

            interface alist<'a> with
                member x.IsConstant = true
                member x.GetReader() = new History.Readers.ConstantReader<_,_>(PList.trace, deltas, content) :> IListReader<_>
                member x.Content = mcontent
        
            new(content : plist<'a>) = ConstantList<'a>(Lazy.CreateFromValue content)

        type AdaptiveList<'a>(newReader : unit -> IOpReader<pdeltalist<'a>>) =
            let h = History.ofReader PList.trace newReader
            interface alist<'a> with
                member x.IsConstant = false
                member x.Content = h :> IMod<_>
                member x.GetReader() = h.NewReader()
                
        let inline alist (f : Ag.Scope -> #IOpReader<pdeltalist<'a>>) =
            let scope = Ag.getContext()
            AdaptiveList<'a>(fun () -> f scope :> IOpReader<_>) :> alist<_>
            
        let inline constant (l : Lazy<plist<'a>>) =
            ConstantList<'a>(l) :> alist<_>

    [<AutoOpen>]
    module Readers =
        open System.Collections.Generic

        [<AutoOpen>]
        module Helpers = 
            type IndexMapping<'k>(comparer : IComparer<'k>) =
                let comparer =
                    { new IComparer<'k * Index> with
                        member x.Compare((l,_),(r,_)) =
                            comparer.Compare(l,r)
                    }

                let store = SortedSetExt<'k * Index>(comparer)

                member x.Invoke(k : 'k) =
                    let (l,s,r) = store.FindNeighbours((k, Unchecked.defaultof<_>))
                    let s = if s.HasValue then Some s.Value else None

                    match s with
                        | Some(_,i) -> 
                            i

                        | None ->
                            let l = if l.HasValue then Some l.Value else None
                            let r = if r.HasValue then Some r.Value else None

                            let l =
                                match s with
                                    | Some s -> Some s
                                    | None -> l

                            let result = 
                                match l, r with
                                    | None, None                -> Index.after Index.zero
                                    | Some(_,l), None           -> Index.after l
                                    | None, Some(_,r)           -> Index.before r
                                    | Some (_,l), Some(_,r)     -> Index.between l r

                            store.Add((k, result)) |> ignore

                            result

                member x.Revoke(k : 'k) =
                    let (l,s,r) = store.FindNeighbours((k, Unchecked.defaultof<_>))

                    if s.HasValue then
                        store.Remove s.Value |> ignore
                        let (_,s) = s.Value
                        s
                    else
                        failwith "[AList] removing unknown index"

                member x.Clear() =
                    store.Clear()

            type IndexCache<'a, 'b>(f : Index -> 'a -> 'b, release : 'b -> unit) =
                let store = Dict<Index, 'a * 'b>()
//
//                member x.Invoke(i : Index, a : 'a) =
//                    match store.TryGetValue(i) with
//                        | (true, (oa, res)) ->
//                            if Unchecked.equals oa a then
//                                res
//                            else
//                                release res
//                                let res = f i a
//                                store.[i] <- (a, res)
//                                res
//                        | _ ->
//                            let res = f i a
//                            store.[i] <- (a, res)
//                            res

                member x.InvokeAndGetOld(i : Index, a : 'a) =
                    match store.TryGetValue(i) with
                        | (true, (oa, old)) ->
                            if Unchecked.equals oa a then
                                None, old
                            else
                                let res = f i a
                                store.[i] <- (a, res)
                                Some old, res
                        | _ ->
                            let res = f i a
                            store.[i] <- (a, res)
                            None, res       
                                        
                member x.Revoke(i : Index) =
                    match store.TryRemove i with
                        | (true, (oa,ob)) -> 
                            release ob
                            ob
                        | _ -> 
                            failwithf "[AList] cannot revoke unknown index %A" i

                member x.Clear() =
                    store.Values |> Seq.iter (snd >> release)
                    store.Clear()

                new(f : Index -> 'a -> 'b) = IndexCache(f, ignore)

            type Unique<'b when 'b : comparison>(value : 'b) =
                static let mutable currentId = 0
                static let newId() = System.Threading.Interlocked.Increment(&currentId)

                let id = newId()

                member x.Value = value
                member private x.Id = id

                override x.ToString() = value.ToString()

                override x.GetHashCode() = HashCode.Combine(Unchecked.hash value, id)
                override x.Equals o =
                    match o with
                        | :? Unique<'b> as o -> Unchecked.equals value o.Value && id = o.Id
                        | _ -> false

                interface IComparable with
                    member x.CompareTo o =
                        match o with
                            | :? Unique<'b> as o ->
                                let c = compare value o.Value
                                if c = 0 then compare id o.Id
                                else c
                            | _ ->
                                failwith "uncomparable"



        type MapReader<'a, 'b>(scope : Ag.Scope, input : alist<'a>, mapping : Index -> 'a -> 'b) =
            inherit AbstractReader<pdeltalist<'b>>(scope, PDeltaList.monoid)

            let r = input.GetReader()

            override x.Release() =
                r.Dispose()

            override x.Compute(token) =
                r.GetOperations token |> PDeltaList.map (fun i op ->
                    match op with
                        | Remove -> Remove
                        | Set v -> Set (mapping i v)
                )

        type ChooseReader<'a, 'b>(scope : Ag.Scope, input : alist<'a>, mapping : Index -> 'a -> Option<'b>) =
            inherit AbstractReader<pdeltalist<'b>>(scope, PDeltaList.monoid)

            let r = input.GetReader()
            let mapping = IndexCache mapping

            override x.Release() =
                mapping.Clear()
                r.Dispose()

            override x.Compute(token) =
                r.GetOperations token |> PDeltaList.choose (fun i op ->
                    match op with
                        | Remove -> 
                            match mapping.Revoke(i) with
                                | Some _ -> Some Remove
                                | _ -> None
                        | Set v -> 
                            let o, n = mapping.InvokeAndGetOld(i, v)
                            match n with
                                | Some res -> Some (Set res)
                                | None -> 
                                    match o with
                                        | Some (Some o) -> Some Remove
                                        | _ -> None
                )

        type FilterReader<'a>(scope : Ag.Scope, input : alist<'a>, mapping : Index -> 'a -> bool) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)

            let r = input.GetReader()
            let mapping = IndexCache mapping

            override x.Release() =
                mapping.Clear()
                r.Dispose()

            override x.Compute(token) =
                r.GetOperations token |> PDeltaList.choose (fun i op ->
                    match op with
                        | Remove -> 
                            match mapping.Revoke(i) with
                                | true -> Some Remove
                                | _ -> None
                        | Set v -> 
                            let o, n = mapping.InvokeAndGetOld(i, v)
                            match n with
                                | true -> 
                                    Some (Set v)
                                | false -> 
                                    match o with
                                        | Some true -> Some Remove
                                        | _ -> None
                )

        type MultiReader<'a>(scope : Ag.Scope, mapping : IndexMapping<Index * Index>, list : alist<'a>, release : alist<'a> -> unit) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)
            
            let targets = HashSet<Index>()

            let mutable reader = None

            let getReader() =
                match reader with
                    | Some r -> r
                    | None ->
                        let r = list.GetReader()
                        reader <- Some r
                        r

            member x.AddTarget(oi : Index) =
                if targets.Add oi then
                    getReader().State.Content
                        |> MapExt.mapMonotonic (fun ii v -> mapping.Invoke(oi, ii), Set v)
                        |> PDeltaList.ofMap
                else
                    PDeltaList.empty

            member x.RemoveTarget(dirty : HashSet<MultiReader<'a>>, oi : Index) =
                if targets.Remove oi then
                    match reader with
                        | Some r ->
                            let result = 
                                r.State.Content 
                                    |> MapExt.mapMonotonic (fun ii v -> mapping.Revoke(oi,ii), Remove)
                                    |> PDeltaList.ofMap

                            if targets.Count = 0 then 
                                dirty.Remove x |> ignore
                                x.Dispose()

                            result

                        | None ->
                            failwith "[AList] invalid reader state"
                else
                    PDeltaList.empty

            override x.Release() =
                match reader with
                    | Some r ->
                        release(list)
                        r.Dispose()
                        reader <- None
                    | None -> ()

            override x.Compute(token) =
                match reader with
                    | Some r -> 
                        let ops = r.GetOperations token

                        ops |> PDeltaList.collect (fun ii op ->
                            match op with
                                | Remove -> 
                                    targets
                                        |> Seq.map (fun oi -> mapping.Revoke(oi, ii), Remove)
                                        |> PDeltaList.ofSeq

                                | Set v ->
                                    targets
                                        |> Seq.map (fun oi -> mapping.Invoke(oi, ii), Set v)
                                        |> PDeltaList.ofSeq

                        )

                    | None ->
                        PDeltaList.empty

        type CollectReader<'a, 'b>(scope : Ag.Scope, input : alist<'a>, f : Index -> 'a -> alist<'b>) =
            inherit AbstractDirtyReader<MultiReader<'b>, pdeltalist<'b>>(scope, PDeltaList.monoid)
            
            let mapping = IndexMapping<Index * Index>(LanguagePrimitives.FastGenericComparer)
            let cache = Dictionary<Index, 'a * alist<'b>>()
            let readers = Dictionary<alist<'b>, MultiReader<'b>>()
            let input = input.GetReader()

            let removeReader (l : alist<'b>) =
                readers.Remove l |> ignore

            let getReader (l : alist<'b>) =
                match readers.TryGetValue l with
                    | (true, r) -> r
                    | _ ->
                        let r = new MultiReader<'b>(scope, mapping, l, removeReader)
                        readers.Add(l, r) 
                        r

            member x.Invoke (dirty : HashSet<MultiReader<'b>>, i : Index, v : 'a) =
                match cache.TryGetValue(i) with
                    | (true, (oldValue, oldList)) ->
                        if Unchecked.equals oldValue v then
                            dirty.Add (getReader(oldList)) |> ignore
                            PDeltaList.empty
                        else
                            let newList = f i v
                            cache.[i] <- (v, newList)
                            let newReader = getReader(newList)

                            let add = newReader.AddTarget i
                            let rem = getReader(oldList).RemoveTarget(dirty, i)
                            dirty.Add newReader |> ignore
                            PDeltaList.combine add rem

                    | _ ->
                        let newList = f i v
                        cache.[i] <- (v, newList)
                        let newReader = getReader(newList)
                        let add = newReader.AddTarget i
                        dirty.Add newReader |> ignore
                        add

            member x.Revoke (dirty : HashSet<MultiReader<'b>>, i : Index) =
                match cache.TryGetValue i with
                    | (true, (v,l)) ->
                        let r = getReader l
                        cache.Remove i |> ignore
                        r.RemoveTarget(dirty, i)
                    | _ ->
                        failwithf "[AList] cannot remove %A from outer list" i

            override x.Release() =
                mapping.Clear()
                cache.Clear()
                readers.Values |> Seq.iter (fun r -> r.Dispose())
                readers.Clear()
                input.Dispose()

            override x.Compute(token, dirty) =
                let mutable result = 
                    input.GetOperations token |> PDeltaList.collect (fun i op ->
                        match op with
                            | Remove -> x.Revoke(dirty, i)
                            | Set v -> x.Invoke(dirty, i, v)
                    )

                for d in dirty do
                    result <- PDeltaList.combine result (d.GetOperations token)

                result

        type ConcatReader<'a>(scope : Ag.Scope, input : alist<alist<'a>>) =
            inherit AbstractDirtyReader<MultiReader<'a>, pdeltalist<'a>>(scope, PDeltaList.monoid)
            
            let cache = Dictionary<Index, alist<'a>>()
            let mapping = IndexMapping<Index * Index>(LanguagePrimitives.FastGenericComparer)
            let readers = Dictionary<alist<'a>, MultiReader<'a>>()
            let input = input.GetReader()

            let removeReader (l : alist<'a>) =
                readers.Remove l |> ignore

            let getReader (l : alist<'a>) =
                match readers.TryGetValue l with
                    | (true, r) -> r
                    | _ ->
                        let r = new MultiReader<'a>(scope, mapping, l, removeReader)
                        readers.Add(l, r) 
                        r

            member x.Invoke (dirty : HashSet<MultiReader<'a>>, i : Index, newList : alist<'a>) =
                match cache.TryGetValue(i) with
                    | (true, oldList) ->
                        if oldList = newList then
                            dirty.Add (getReader(oldList)) |> ignore
                            PDeltaList.empty
                        else
                            cache.[i] <- newList
                            let newReader = getReader(newList)
                            let add = newReader.AddTarget i
                            let rem = getReader(oldList).RemoveTarget(dirty, i)
                            dirty.Add newReader |> ignore
                            PDeltaList.combine add rem

                    | _ ->
                        cache.[i] <- newList
                        let newReader = getReader(newList)
                        let add = newReader.AddTarget i
                        dirty.Add newReader |> ignore
                        add

            member x.Revoke (dirty : HashSet<MultiReader<'a>>, i : Index) =
                match cache.TryGetValue i with
                    | (true, l) ->
                        let r = getReader l
                        cache.Remove i |> ignore
                        r.RemoveTarget(dirty, i)
                    | _ ->
                        failwithf "[AList] cannot remove %A from outer list" i

            override x.Release() =
                mapping.Clear()
                cache.Clear()
                readers.Values |> Seq.iter (fun r -> r.Dispose())
                readers.Clear()
                input.Dispose()

            override x.Compute(token, dirty) =
                let mutable result = 
                    input.GetOperations token |> PDeltaList.collect (fun i op ->
                        match op with
                            | Remove -> x.Revoke(dirty,i)
                            | Set v -> x.Invoke(dirty, i, v)
                    )

                for d in dirty do
                    result <- PDeltaList.combine result (d.GetOperations token)

                result

        type ConcatListReader<'a>(scope : Ag.Scope, inputs : plist<alist<'a>>) =
            inherit AbstractDirtyReader<MultiReader<'a>, pdeltalist<'a>>(scope, PDeltaList.monoid)
            
            let mapping = IndexMapping<Index * Index>(LanguagePrimitives.FastGenericComparer)
            let readers = Dictionary<alist<'a>, MultiReader<'a>>()

            let removeReader (l : alist<'a>) =
                readers.Remove l |> ignore

            let getReader (l : alist<'a>) =
                match readers.TryGetValue l with
                    | (true, r) -> r
                    | _ ->
                        let r = new MultiReader<'a>(scope, mapping, l, removeReader)
                        readers.Add(l, r) 
                        r            

            do inputs.Content |> MapExt.iter (fun oi l -> getReader(l).AddTarget oi |> ignore)
            let mutable initial = true



            override x.Release() =
                mapping.Clear()
                readers.Values |> Seq.iter (fun r -> r.Dispose())
                readers.Clear()
                readers.Clear()

            override x.Compute(token, dirty) =
                if initial then
                    initial <- false
                    readers.Values |> Seq.fold (fun d r -> PDeltaList.combine d (r.GetOperations token)) PDeltaList.empty

                else    
                    dirty |> Seq.fold (fun d r -> PDeltaList.combine d (r.GetOperations token)) PDeltaList.empty

        type SetSortByReader<'a, 'b when 'b : comparison>(scope : Ag.Scope, input : aset<'a>, mapping : 'a -> 'b) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)

            let reader = input.GetReader()
            let indices = IndexMapping<Unique<'b>>(LanguagePrimitives.FastGenericComparer<Unique<'b>>)
            let mapping = Cache (mapping >> Unique)

            override x.Release() =
                indices.Clear()
                mapping.Clear(ignore)
                reader.Dispose()

            override x.Compute(token) =
                reader.GetOperations token 
                    |> HDeltaSet.toSeq
                    |> Seq.map (fun d ->
                        match d with
                            | Add(1,v) -> 
                                let k = mapping.Invoke v
                                let i = indices.Invoke k
                                i, Set v
                            | Rem(1,v) ->
                                let k = mapping.Revoke v
                                let i = indices.Revoke k
                                i, Remove
                            | _ ->
                                failwith ""
                    )
                    |> PDeltaList.ofSeq
        
        type SetSortWithReader<'a>(scope : Ag.Scope, input : aset<'a>, comparer : IComparer<'a>) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)

            let reader = input.GetReader()
            let indices = IndexMapping<'a>(comparer)
            
            override x.Release() =
                indices.Clear()
                reader.Dispose()

            override x.Compute(token) =
                reader.GetOperations token
                    |> HDeltaSet.toSeq
                    |> Seq.map (fun d ->
                        match d with
                            | Add(1,v) -> 
                                let i = indices.Invoke v
                                i, Set v
                            | Rem(1,v) ->
                                let i = indices.Revoke v
                                i, Remove
                            | _ ->
                                failwith ""
                    )
                    |> PDeltaList.ofSeq

        type ToListReader<'a>(scope : Ag.Scope, input : aset<'a>) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)

            let reader = input.GetReader()
            let mutable last = Index.zero
            let newIndex (v : 'a) =
                let i = Index.after last
                last <- i
                i

            let newIndex = Cache newIndex

            override x.Release() =
                newIndex.Clear(ignore)
                reader.Dispose()
                last <- Index.zero

            override x.Compute(token) =
                reader.GetOperations token
                    |> HDeltaSet.toSeq
                    |> Seq.map (fun d ->
                    match d with
                        | Add(1,v) -> 
                            let i = newIndex.Invoke v
                            i, Set v
                        | Rem(1,v) ->
                            let i = newIndex.Revoke v
                            i, Remove
                        | _ ->
                            failwith ""
                    )
                    |> PDeltaList.ofSeq

        type ToSetReader<'a>(scope : Ag.Scope, input : alist<'a>) =
            inherit AbstractReader<hdeltaset<'a>>(scope, HDeltaSet.monoid)
            
            let reader = input.GetReader()
            override x.Release() =
                reader.Dispose()

            override x.Compute(token) =
                let oldContent = reader.State.Content
                reader.GetOperations token
                    |> PDeltaList.toSeq
                    |> Seq.collect (fun (i,op) ->
                        match op with
                            | Set v ->
                                match MapExt.tryFind i oldContent with
                                    | Some o ->
                                        if Unchecked.equals o v then
                                            Seq.empty
                                        else
                                            Seq.ofList [Add v; Rem o]
                                    | _ -> 
                                        Seq.singleton (Add v)
                            | Remove ->
                                match MapExt.tryFind i oldContent with
                                    | Some v -> Seq.singleton (Rem v)
                                    | _ -> failwith ""
                    )
                    |> HDeltaSet.ofSeq

        type ListSortByReader<'a, 'b when 'b : comparison>(scope : Ag.Scope, input : alist<'a>, mapping : Index -> 'a -> 'b) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)

            let reader = input.GetReader()
            let indices = IndexMapping<'b * Index>(LanguagePrimitives.FastGenericComparer<_>)
            let mapping = IndexCache (fun i v -> mapping i v, i)

            override x.Release() =
                mapping.Clear()
                reader.Dispose()

            override x.Compute(token) =
                let oldContent = reader.State.Content
                reader.GetOperations token
                    |> PDeltaList.collect (fun ii op ->
                        match op with
                            | Set v ->
                                let oldB, b = mapping.InvokeAndGetOld(ii,v)
                                let i = indices.Invoke(b)

                                match oldB with
                                    | Some oldB ->
                                        let oi = indices.Revoke oldB
                                        PDeltaList.ofList [oi, Remove; i, Set v]
                                    | None ->
                                        PDeltaList.single i (Set v)



                            | Remove ->
                                let b = mapping.Revoke(ii)
                                let i = indices.Revoke b
                                PDeltaList.single i Remove
                    )
        
        type ListSortWithReader<'a>(scope : Ag.Scope, input : alist<'a>, comparer : IComparer<'a>) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)

            let comparer =
                { new IComparer<'a * Index> with
                    member x.Compare((lv,li), (rv, ri)) =
                        let c = comparer.Compare(lv, rv)
                        if c = 0 then compare li ri
                        else c
                }

            let reader = input.GetReader()
            let indices = IndexMapping<'a * Index>(comparer)
  
            override x.Release() =
                indices.Clear()
                reader.Dispose()

            override x.Compute(token) =
                let oldContent = reader.State.Content
                reader.GetOperations token
                    |> PDeltaList.collect (fun i op ->
                        match op with
                            | Set v -> 
                                match MapExt.tryFind i oldContent with
                                    | Some o -> 
                                        if Unchecked.equals o v then
                                            PDeltaList.empty
                                        else
                                            let oi = indices.Revoke(o, i)
                                            let i = indices.Invoke(v, i)
                                            PDeltaList.ofList [oi, Remove; i, Set v]
                                    | None ->
                                        let i = indices.Invoke(v, i)
                                        PDeltaList.single i (Set v)

                            | Remove ->
                                let v = oldContent |> MapExt.find i
                                let i = indices.Revoke(v, i)
                                PDeltaList.single i Remove
                    )

        type BindReader<'a, 'b>(scope : Ag.Scope, input : IMod<'a>, f : 'a -> alist<'b>) =
            inherit AbstractReader<pdeltalist<'b>>(scope, PDeltaList.monoid)

            let mutable inputChanged = 1
            let mutable reader : Option<'a * IListReader<'b>> = None

            override x.InputChanged(t : obj, o : IAdaptiveObject) =
                if Object.ReferenceEquals(input, o) then
                    inputChanged <- 1

            override x.Release() =
                match reader with
                    | Some(_,r) -> 
                        r.Dispose()
                        reader <- None
                    | None -> 
                        ()

            override x.Compute(token) =
                let v = input.GetValue token
                let inputChanged = System.Threading.Interlocked.CompareExchange(&inputChanged, 0, 1)
                match reader with
                    | Some (oldA, oldReader) when inputChanged = 0 || Unchecked.equals v oldA ->
                        oldReader.GetOperations token

                    | _ -> 
                        let r = f(v).GetReader()
                        let deltas = 
                            let addNew = r.GetOperations token
                            match reader with
                                | Some(_,old) ->
                                    let remOld = plist.ComputeDeltas(old.State, PList.empty)
                                    old.Dispose()
                                    PDeltaList.combine remOld addNew
                                | None ->
                                    addNew
                        reader <- Some (v,r)
                        deltas

        type IndexedMod<'a>(index : Index, m : IMod<'a>) =
            inherit Mod.AbstractMod<Index * 'a>()
            let mutable index = index
            let mutable lastValue = None

            member x.Index = index

            member x.LastValue = lastValue

            member x.Dispose() =
                let mutable foo = 0
                x.Outputs.Consume(&foo) |> ignore

            override x.Compute(token) =
                let v = m.GetValue token
                lastValue <- Some v
                (index, v)

        type ChooseMReader<'a, 'b>(scope : Ag.Scope, input : alist<'a>, f : Index -> 'a -> IMod<Option<'b>>) =
            inherit AbstractDirtyReader<IndexedMod<Option<'b>>, pdeltalist<'b>>(scope, PDeltaList.monoid)

            let input = input.GetReader()
            let cache = IndexCache((fun i v -> IndexedMod(i, f i v)), fun m -> m.Dispose())

            member private x.invoke (dirty : HashSet<_>) (i : Index) (v : 'a) =
                let o, n = cache.InvokeAndGetOld(i, v)
                dirty.Add n |> ignore
                match o with
                    | Some o ->
                        dirty.Remove o |> ignore
                        o.Outputs.Remove x |> ignore
                        match o.LastValue with
                            | Some l -> PDeltaList.single i Remove
                            | None -> PDeltaList.empty
                    | None ->
                        PDeltaList.empty

            member private x.revoke (dirty : HashSet<_>) (i : Index) =
                let o = cache.Revoke(i)
                dirty.Remove o |> ignore
                o.Outputs.Remove x |> ignore
                match o.LastValue with
                    | Some l -> PDeltaList.single o.Index Remove
                    | None -> PDeltaList.empty

            override x.Release() =
                input.Dispose()
                cache.Clear()

            override x.Compute(token, dirty) =
                let deltas = input.GetOperations token

                let mutable res = PDeltaList.empty
                for (i,d) in deltas |> PDeltaList.toSeq do
                    match d with
                        | Set v -> res <- PDeltaList.combine res (x.invoke dirty i v)
                        | Remove -> res <- PDeltaList.combine res (x.revoke dirty i)

                for d in dirty do
                    let i, v = d.GetValue token
                    match v with
                        | Some v -> 
                            res <- PDeltaList.combine res (PDeltaList.single i (Set v))
                        | None ->
                            ()

                res

        type OfModSingleReader<'a>(scope : Ag.Scope, input : IMod<'a>) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)
            let index = Index.after Index.zero
            let mutable old = None

            override x.Release() =
                input.Outputs.Remove x |> ignore
                old <- None

            override x.Compute(token) =
                let v = input.GetValue token
                match old with
                    | Some o when Unchecked.equals o v -> 
                        PDeltaList.empty

                    | _ -> 
                        old <- Some v
                        PDeltaList.single index (Set v)

        type OfModReader<'a>(scope : Ag.Scope, input : IMod<plist<'a>>) =
            inherit AbstractReader<pdeltalist<'a>>(scope, PDeltaList.monoid)

            let mutable state = PList.empty

            override x.Release() =
                input.Outputs.Remove x |> ignore

            override x.Compute(token) =
                let v = input.GetValue token
                let delta = plist.ComputeDeltas(state, v)
                state <- v
                delta

            interface IOpReader<plist<'a>, pdeltalist<'a>> with
                member x.State = state


    /// the empty alist
    let empty<'a> = EmptyList<'a>.Instance
    
    /// creates a new alist containing only the given element
    let single (v : 'a) =
        ConstantList(PList.single v) :> alist<_>
        
    /// creates a new alist using the given list
    let ofPList (l : plist<'a>) =
        ConstantList(l) :> alist<_>

    /// creates a new alist using the given sequence
    let ofSeq (s : seq<'a>) =
        s |> PList.ofSeq |> ofPList

    /// creates a new alist using the given sequence
    let ofList (l : list<'a>) =
        l |> PList.ofList |> ofPList

    /// creates a new alist using the given sequence
    let ofArray (l : 'a[]) =
        l |> PList.ofArray |> ofPList

        
    let toPList (list : alist<'a>) =
        list.Content |> Mod.force

    let toSeq (list : alist<'a>) =
        list.Content |> Mod.force :> seq<_>

    let toList (list : alist<'a>) =
        list.Content |> Mod.force |> PList.toList

    let toArray (list : alist<'a>) =
        list.Content |> Mod.force |> PList.toArray

    let concat' (lists : plist<alist<'a>>) =
        // TODO: when all constant -> result constant
        alist <| fun scope -> new ConcatListReader<'a>(scope, lists)

    let concat (lists : alist<alist<'a>>) =
        if lists.IsConstant then
            concat' (Mod.force lists.Content)
        else
            alist <| fun scope -> new ConcatReader<'a>(scope, lists)

    let mapi (mapping : Index -> 'a -> 'b) (list : alist<'a>) =
        if list.IsConstant then
            constant <| lazy (list.Content |> Mod.force |> PList.mapi mapping)
        else
            alist <| fun scope -> new MapReader<'a, 'b>(scope, list, mapping)

    let map (mapping : 'a -> 'b) (list : alist<'a>) =
        mapi (fun _ v -> mapping v) list

    let choosei (mapping : Index -> 'a -> Option<'b>) (list : alist<'a>) =
        if list.IsConstant then
            constant <| lazy (list.Content |> Mod.force |> PList.choosei mapping)
        else
            alist <| fun scope -> new ChooseReader<'a, 'b>(scope, list, mapping)

    let choose (mapping : 'a -> Option<'b>) (list : alist<'a>) =
        choosei (fun _ v -> mapping v) list

    let filteri (predicate : Index -> 'a -> bool) (list : alist<'a>) =
        if list.IsConstant then
            constant <| lazy (list.Content |> Mod.force |> PList.filteri predicate)
        else
            alist <| fun scope -> new FilterReader<'a>(scope, list, predicate)

    let filter (predicate : 'a -> bool) (list : alist<'a>) =
        filteri (fun _ v -> predicate v) list

    let collecti (mapping : Index -> 'a -> alist<'b>) (list : alist<'a>) =
        if list.IsConstant then
            concat' (list.Content |> Mod.force |> PList.mapi mapping)
        else
            alist <| fun scope -> new CollectReader<'a, 'b>(scope, list, mapping)

    let collect (mapping : 'a -> alist<'b>) (list : alist<'a>) =
        collecti (fun _ v -> mapping v) list


    let toASet (l : alist<'a>) =
        ASet.Implementation.aset <| fun scope -> new ToSetReader<'a>(scope, l)

    let ofASet (set : aset<'a>) =
        alist <| fun scope -> new ToListReader<'a>(scope, set)


    let sortBy (f : 'a -> 'b) (list : alist<'a>) =
        alist <| fun scope -> new ListSortByReader<'a, 'b>(scope, list, fun _ v -> f v)

    let sortWith (cmp : 'a -> 'a -> int) (list : alist<'a>) =
        let cmp = Comparer.ofFunction cmp
        alist <| fun scope -> new ListSortWithReader<'a>(scope, list, cmp)

    let sort<'a when 'a : comparison> (list : alist<'a>) =
        let cmp = LanguagePrimitives.FastGenericComparer<'a>
        alist <| fun scope -> new ListSortWithReader<'a>(scope, list, cmp)
            

    let toMod (l : alist<'a>) =
        l.Content

    let append (l : alist<'a>) (r : alist<'a>) =
        // TODO: better impl
        [l;r] |> PList.ofList |> concat'

    let bind (mapping : 'a -> alist<'b>) (m : IMod<'a>) : alist<'b> =
        if m.IsConstant then
            m |> Mod.force |> mapping
        else
            alist <| fun scope -> new BindReader<_,_>(scope, m, mapping)

    let bind2 (mapping : 'a -> 'b -> alist<'c>) (a : IMod<'a>) (b : IMod<'b>) : alist<'c> =
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

    let chooseiM (mapping : Index -> 'a -> IMod<Option<'b>>) (list : alist<'a>) : alist<'b> =
        alist <| fun scope -> new ChooseMReader<'a, 'b>(scope, list, mapping)
        
    let chooseM (mapping : 'a -> IMod<Option<'b>>) (list : alist<'a>) : alist<'b> =
        chooseiM (fun _ v -> mapping v) list

    let filteriM (f : Index -> 'a -> IMod<bool>) (list : alist<'a>) : alist<'a> =
        list |> chooseiM (fun i v -> f i v |> Mod.map (fun c -> if c then Some v else None))

    let filterM (f : 'a -> IMod<bool>) (list : alist<'a>) : alist<'a> =
        filteriM (fun _ v -> f v) list

    let ofModSingle (m : IMod<'a>) : alist<'a> =
        if m.IsConstant then
            constant <| lazy (m |> Mod.force |> PList.single)
        else
            alist <| fun scope -> new OfModSingleReader<'a>(scope, m)

    let ofMod (m : IMod<plist<'a>>) : alist<'a> =
        if m.IsConstant then
            constant <| lazy (m |> Mod.force)
        else
            alist <| fun scope -> new OfModReader<_>(scope, m)


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

    let unsafeRegisterCallbackNoGcRoot (f : list<Index * ElementOperation<'a>> -> unit) (list : alist<'a>) =
        let m = list.GetReader()

        let result =
            m.AddEvaluationCallback(fun self ->
                m.GetOperations(self) |> PDeltaList.toList |> f
            )


        let callbackSet = callbackTable.GetOrCreateValue(list)
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
    let unsafeRegisterCallbackKeepDisposable (f : list<Index * ElementOperation<'a>> -> unit) (list : alist<'a>) =
        let d = unsafeRegisterCallbackNoGcRoot f list
        undyingCallbacks.Add d |> ignore
        { new IDisposable with
            member x.Dispose() =
                d.Dispose()
                undyingCallbacks.Remove d |> ignore
        }


[<AutoOpen>]
module ``ASet -> AList interop`` =
    open AList.Implementation
    open AList.Readers

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    [<RequireQualifiedAccess>]
    module ASet =
        
        let sortBy (f : 'a -> 'b) (set : aset<'a>) =
            alist <| fun scope -> new SetSortByReader<'a, 'b>(scope, set, f)

        let sortWith (cmp : 'a -> 'a -> int) (set : aset<'a>) =
            let cmp = AList.Comparer.ofFunction cmp
            alist <| fun scope -> new SetSortWithReader<'a>(scope, set, cmp)

        let sort<'a when 'a : comparison> (set : aset<'a>) =
            let cmp = LanguagePrimitives.FastGenericComparer<'a>
            alist <| fun scope -> new SetSortWithReader<'a>(scope, set, cmp)
            
        let toAList (set : aset<'a>) =
            AList.ofASet set

        let ofAList (l : alist<'a>) =
            AList.toASet l