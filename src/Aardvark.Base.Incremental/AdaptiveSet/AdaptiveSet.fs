namespace Aardvark.Base.Incremental

open System
open System.Runtime.CompilerServices
open System.Collections.Generic
open System.Collections.Concurrent
open Aardvark.Base
open Aardvark.Base.Incremental.ASetReaders

/// <summary>
/// defines functions for composing asets and mods
/// </summary>
module ASet =
    type AdaptiveSet<'a>(newReader : unit -> IReader<'a>) =
        let l = obj()
        let readers = WeakSet<CopyReader<'a>>()

        let mutable inputReader = None
        let getReader() =
            match inputReader with
                | Some r -> r
                | None ->
                    let r = newReader()
                    inputReader <- Some r
                    r


        interface aset<'a> with
            member x.ReaderCount = readers.Count
            member x.IsConstant = false
            member x.GetReader () =
                lock l (fun () ->
                    let r = getReader()

                    let remove ri =
                        r.RemoveOutput ri
                        readers.Remove ri |> ignore

                        if readers.IsEmpty then
                            r.Dispose()
                            inputReader <- None

                    let reader = new CopyReader<'a>(r, remove)
                    readers.Add reader |> ignore

                    reader :> _
                )

    type ConstantSet<'a>(content : Lazy<HashSet<'a>>) =
        let content = lazy ( HashSet content.Value )

        interface aset<'a> with
            member x.ReaderCount = 0
            member x.IsConstant = true
            member x.GetReader () =
                let r = new OneShotReader<'a>(content.Value |> Seq.toList |> List.map Add)
                r :> IReader<_>

        new(l : seq<'a>) = ConstantSet<'a>(lazy (HashSet l))
        new(content : Lazy<list<'a>>) = ConstantSet<'a>(lazy (HashSet content.Value))

    type private EmptySetImpl<'a> private() =
        static let emptySet = ConstantSet [] :> aset<'a>
        static member Instance = emptySet

    let private scoped (f : 'a -> 'b) =
        let scope = Ag.getContext()
        fun v -> Ag.useScope scope (fun () -> f v)

    let private callbackTable = ConditionalWeakTable<obj, ConcurrentHashSet<IDisposable>>()
    type private CallbackSubscription(m : obj, cb : unit -> unit, live : ref<bool>, reader : IAdaptiveObject, set : ConcurrentHashSet<IDisposable>) =
        let disposable = reader |> unbox<IDisposable>

        member x.Dispose() = 
            if !live then
                live := false
                disposable.Dispose()
                reader.MarkingCallbacks.Remove cb |> ignore
                set.Remove x |> ignore
                if set.Count = 0 then
                    callbackTable.Remove(m) |> ignore

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        override x.Finalize() =
            try x.Dispose()
            with _ -> ()


    /// <summary>
    /// creates an empty set instance being reference
    /// equal to all other empty sets of the same type.
    /// </summary>
    let empty<'a> : aset<'a> =
        EmptySetImpl<'a>.Instance

    /// <summary>
    /// creates a set containing one element
    /// </summary>
    let single (v : 'a) =
        ConstantSet [v] :> aset<_>

    /// <summary>
    /// creates a set containing all distinct 
    /// elements in the given sequence
    /// </summary>
    let ofSeq (s : seq<'a>) =
        ConstantSet(s) :> aset<_>
    
    /// <summary>
    /// creates a set containing all distinct 
    /// elements in the given list
    /// </summary>
    let ofList (l : list<'a>) =
        ConstantSet(l) :> aset<_>

    /// <summary>
    /// creates a set containing all distinct 
    /// elements in the given array
    /// </summary>
    let ofArray (a : 'a[]) =
        ConstantSet(a) :> aset<_>

    let delay (f : unit -> seq<'a>) =
        let scope = Ag.getContext()
        ConstantSet(lazy (Ag.useScope scope (fun () -> f() |> Seq.toList))) :> aset<_>

    /// <summary>
    /// returns a list of all elements currently in the adaptive set. 
    /// NOTE: will force the evaluation of the set.
    /// </summary>
    let toList (set : aset<'a>) =
        use r = set.GetReader()
        r.GetDelta() |> ignore
        r.Content |> Seq.toList

    /// <summary>
    /// returns a sequence of all elements currently in the adaptive set. 
    /// NOTE: will force the evaluation of the set.
    /// </summary>
    let toSeq (set : aset<'a>) =
        let l = toList set
        l :> seq<_>

    /// <summary>
    /// returns an array of all elements currently in the adaptive set. 
    /// NOTE: will force the evaluation of the set.
    /// </summary>
    let toArray (set : aset<'a>) =
        use r = set.GetReader()
        r.GetDelta() |> ignore
        r.Content |> Seq.toArray

    /// <summary>
    /// creates a singleton set which will always contain
    /// the latest value of the given mod-cell.
    /// </summary>
    let ofMod (m : IMod<'a>) =
        if m.IsConstant then
            ConstantSet [m.GetValue()] :> aset<_>
        else
            AdaptiveSet(fun () -> ofMod m) :> aset<_>


    /// <summary>
    /// creates an aset containing all element from the
    /// original one but disposing them as they are removed
    /// </summary>
    let using (s : aset<'a>) =
        AdaptiveSet(fun () -> using <| s.GetReader()) :> aset<_>

    /// <summary>
    /// creates a mod-cell containing the set's content
    /// as IVersionedSet. Since the IVersionedSet is mutating
    /// (without changing its identity) the mod system is 
    /// prepared to track changes using the version.
    /// </summary>
    let toMod (s : aset<'a>) =
        let r = s.GetReader()
        let c = r.Content :> IVersionedSet<_>

        let m = Mod.custom (fun () ->
            r.GetDelta() |> ignore
            c
        )
        r.AddOutput m
        m

    /// <summary>
    /// adaptively computes whether the given aset is empty.
    /// </summary>
    let isEmpty (s : aset<'a>) =
        s |> toMod |> Mod.map (fun s -> s.Count = 0)

    /// <summary>
    /// adaptively checks if the set contains the given element
    /// </summary>
    let contains (elem : 'a) (set : aset<'a>) =
        set |> toMod |> Mod.map (fun s -> s.Contains elem)

    /// <summary>
    /// adaptively checks if the set contains all given elements
    /// </summary>
    let containsAll (elems : #seq<'a>) (set : aset<'a>) =
        set |> toMod |> Mod.map (fun s -> elems |> Seq.forall (fun e -> s.Contains e))

    /// <summary>
    /// adaptively checks if the set contains any of the given elements
    /// </summary>
    let containsAny (elems : #seq<'a>) (set : aset<'a>) =
        set |> toMod |> Mod.map (fun s -> elems |> Seq.exists (fun e -> s.Contains e))

    /// <summary>
    /// applies the given function to all elements in the set
    /// and returns a new set containing the respective results.
    /// NOTE: duplicates are handled correctly here which means
    ///       that the function may be non-injective
    /// </summary>
    let map (f : 'a -> 'b) (set : aset<'a>) = 
        if set.IsConstant then
            delay (fun () ->
                use r = set.GetReader()
                r.Update()
                r.Content |> Seq.map f
            )
        else
            let scope = Ag.getContext()
            AdaptiveSet(fun () -> set.GetReader() |> map scope f) :> aset<'b>

    /// <summary>
    /// applies the given function to a cell and adaptively
    /// returns the resulting set.
    /// </summary>
    let bind (f : 'a -> aset<'b>) (m : IMod<'a>) =
        if m.IsConstant then 
            m |> Mod.force |> f
        else
            let scope = Ag.getContext()
            AdaptiveSet(fun () -> m |> bind scope (fun v -> (f v).GetReader())) :> aset<'b>

    /// <summary>
    /// applies the given function to both cells and adaptively
    /// returns the resulting set.
    /// </summary>
    let bind2 (f : 'a -> 'b -> aset<'c>) (ma : IMod<'a>) (mb : IMod<'b>) =
        match ma.IsConstant, mb.IsConstant with
            | true, true -> f (Mod.force ma) (Mod.force mb)
            | true, false -> let va = Mod.force ma in bind (fun b -> f va b) mb
            | false, true -> let vb = Mod.force mb in bind (fun a -> f a vb) ma
            | false, false->
                let scope = Ag.getContext()
                AdaptiveSet(fun () -> bind2 scope (fun a b -> (f a b).GetReader()) ma mb) :> aset<'c>

    /// <summary>
    /// applies the given function to all elements in the set
    /// and unions all output-sets.
    /// NOTE: duplicates are handled correctly here meaning that
    ///       the supplied function may return overlapping sets.
    /// </summary>
    let collect (f : 'a -> aset<'b>) (set : aset<'a>) = 
        let scope = Ag.getContext()
        AdaptiveSet(fun () -> set.GetReader() |> collect scope (fun v -> (f v).GetReader())) :> aset<'b>

    /// <summary>
    /// applies the given function to all elements in the set
    /// and returns a set containing all the elements that were Some(v)
    /// NOTE: duplicates are handled correctly here which means
    ///       that the function may be non-injective
    /// </summary>
    let choose (f : 'a -> Option<'b>) (set : aset<'a>) =
        if set.IsConstant then
            delay (fun () ->
                use r = set.GetReader()
                r.Update()
                r.Content |> Seq.choose f
            )
        else
            let scope = Ag.getContext()
            AdaptiveSet(fun () -> set.GetReader() |> choose scope f) :> aset<'b>

    /// <summary>
    /// filters the elements in the set using the given predicate
    /// </summary>
    let filter (f : 'a -> bool) (set : aset<'a>) =
        choose (fun v -> if f v then Some v else None) set

    /// <summary>
    /// adaptively unions the given sets
    /// </summary>
    let union (set : aset<aset<'a>>) =
        collect id set

    /// <summary>
    /// adaptively unions the given sets
    /// </summary>
    let union' (set : seq<aset<'a>>) =
        union (ConstantSet set)

    /// deprecated in favor of union
    [<Obsolete>]
    let concat (set : aset<aset<'a>>) =
        collect id set

    /// deprecated in favor of union'
    [<Obsolete>]
    let concat' (set : seq<aset<'a>>) =
        union (ConstantSet set)

    /// <summary>
    /// applies the given function to all elements in the sequence
    /// and unions all output-sets.
    /// NOTE: duplicates are handled correctly here meaning that
    ///       the supplied function may return overlapping sets.
    /// </summary>
    let collect' (f : 'a -> aset<'b>) (set : seq<'a>) =
        set |> Seq.map f |> union'
  
    /// <summary>
    /// applies the given modifiable function to all elements in the set
    /// </summary>
    let mapM (f : 'a -> IMod<'b>) (s : aset<'a>) =
        s |> collect (fun v ->
            v |> f |> ofMod
        )

    /// <summary>
    /// Filters the set by applying the given modifiable function to all elements
    /// </summary>
    let filterM (f : 'a -> IMod<bool>) (s : aset<'a>) =
        s |> collect (fun v ->
            v |> f |> bind (fun b -> if b then single v else empty)
        )

    /// <summary>
    /// Applies the given modifiable function f to each element x of the set. 
    /// Returns the set comprised of the results for each element where the function returns Some(f(x)).
    /// </summary>
    let chooseM (f : 'a -> IMod<Option<'b>>) (s : aset<'a>) =
        s |> collect (fun v ->
            v |> f |> bind (fun b -> match b with | Some v -> single v | _ -> empty)
        )

    /// <summary>
    /// Creates a set consisting of all modifiable values in the original set
    /// </summary>
    let flattenM (s : aset<IMod<'a>>) =
        s |> collect ofMod

    /// <summary>
    /// Adaptively applies the given function to the entire set-content
    /// </summary>
    let reduce (f : seq<'a> -> 'b) (s : aset<'a>) : IMod<'b> =
        s |> toMod |> Mod.map f


    let fold (add : 'b -> 'a -> 'b) (zero : 'b) (s : aset<'a>) : IMod<'b> =
        let r = s.GetReader()
        let sum = ref zero

        let rec processDeltas (deltas : list<Delta<'a>>) =
            match deltas with
                | Add v :: deltas ->
                    sum := add !sum v
                    processDeltas deltas
                | Rem v :: deltas ->
                    false
                | [] ->
                    true

        let res =
            Mod.custom (fun () ->
                let mutable rem = false
                let delta = r.GetDelta()

                if not <| processDeltas delta then
                    sum := r.Content |> Seq.fold add zero

                !sum
            )

        r.AddOutput res
        res

    /// <summary>
    /// Adaptively projects the set to a value by using an associative add-operation (mappend) and a zero-element (mempty).
    /// NOTE that removals cause a complete re-evaluation whereas additions can be treated efficiently.
    /// </summary>
    let foldMonoid (add : 'a -> 'a -> 'a) (zero : 'a) (s : aset<'a>) : IMod<'a> =
        fold add zero s

    /// <summary>
    /// Adaptively projects the set to a value by using associative add-operation (+), a sub-operation (-) and a zero-element (0).
    /// </summary>
    let foldGroup (add : 'a -> 'a -> 'a) (sub : 'a -> 'a -> 'a) (zero : 'a) (s : aset<'a>) : IMod<'a> =
        let r = s.GetReader()
        let sum = ref zero

        let res =
            Mod.custom (fun () ->
                let delta = r.GetDelta()
                for d in delta do
                    match d with
                        | Add v -> sum := add !sum v
                        | Rem v -> sum := sub !sum v
                !sum
            )

        r.AddOutput res
        res

    /// <summary>
    /// Adaptively applies the given modifiable function to the entire set-content
    /// </summary>
    let reduceM (f : seq<'a> -> 'b) (s : aset<IMod<'a>>) : IMod<'b> =
        reduce f (collect ofMod s)

    /// <summary>
    /// Adaptively projects the set to a value by using an associative add-operation (mappend) and a zero-element (mempty).
    /// NOTE that removals/changes cause a complete re-evaluation whereas additions can be treated efficiently.
    /// </summary>
    let foldMonoidM (add : 'a -> 'a -> 'a) (zero : 'a) (s : aset<IMod<'a>>) : IMod<'a> =
        let s = s |> collect ofMod
        foldMonoid add zero s

    /// <summary>
    /// Adaptively projects the set to a value by using associative add-operation (+), a sub-operation (-) and a zero-element (0).
    /// </summary>
    let foldGroupM (add : 'a -> 'a -> 'a) (sub : 'a -> 'a -> 'a) (zero : 'a) (s : aset<IMod<'a>>) : IMod<'a> =
        let s = s |> collect ofMod
        foldGroup add sub zero s


    /// <summary>
    /// Adaptively calculates the sum of all elements in the set
    /// </summary>
    let inline sum (s : aset<'a>) = foldGroup (+) (-) LanguagePrimitives.GenericZero s

    /// <summary>
    /// Adaptively calculates the product of all elements in the set
    /// </summary>
    let inline product (s : aset<'a>) = foldGroup (*) (/) LanguagePrimitives.GenericOne s

    /// <summary>
    /// Adaptively calculates the sum of all elements in the set
    /// </summary>
    let inline sumM (s : aset<IMod<'a>>) = foldGroupM (+) (-) LanguagePrimitives.GenericZero s

    /// <summary>
    /// Adaptively calculates the product of all elements in the set
    /// </summary>
    let inline productM (s : aset<IMod<'a>>) = foldGroupM (*) (/) LanguagePrimitives.GenericOne s

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
    let unsafeRegisterCallbackNoGcRoot (f : list<Delta<'a>> -> unit) (set : aset<'a>) =
        let m = set.GetReader()
        let f = scoped f
        let self = ref id
        let live = ref true
        self := fun () ->
            if !live then
                try
                    m.GetDelta() |> f
                finally 
                    m.MarkingCallbacks.Add !self |> ignore
        
        lock m (fun () ->
            !self ()
        )

        let callbackSet = callbackTable.GetOrCreateValue(set)
        let s = new CallbackSubscription(set, !self, live, m, callbackSet)
        callbackSet.Add s |> ignore
        s :> IDisposable

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
    let unsafeRegisterCallbackKeepDisposable (f : list<Delta<'a>> -> unit) (set : aset<'a>) =
        let d = unsafeRegisterCallbackNoGcRoot f set
        undyingCallbacks.Add d |> ignore
        { new IDisposable with
            member x.Dispose() =
                d.Dispose()
                undyingCallbacks.Remove d |> ignore
        }
