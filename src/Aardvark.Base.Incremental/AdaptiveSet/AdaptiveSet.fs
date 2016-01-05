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
    let GetReaderProbe = Symbol.Create "[ASet] GetReader"
    let GetConstantReaderProbe = Symbol.Create "[ASet] GetConstantReader"

    type NoRefASet<'a>(inputReader : ReferenceCountedReader<'a>) =
        let mutable inputReader = inputReader

        interface aset<'a> with
            member x.ReaderCount = inputReader.ReferenceCount
            member x.IsConstant = false

            member x.Copy = x :> aset<_>

            member x.GetReader () =
                Telemetry.timed GetReaderProbe (fun () ->
                    let reader = new CopyReader<'a>(inputReader)
                    if inputReader.ReferenceCount > 1 then reader.SetPassThru(false, false)

                    reader :> _
                )

    type AdaptiveSet<'a>(newReader : unit -> IReader<'a>) =
        let mutable inputReader = ReferenceCountedReader(newReader)

        override x.Finalize() =
            try inputReader.ContainingSetDied()
            with _ -> ()

        interface aset<'a> with
            member x.ReaderCount = inputReader.ReferenceCount
            member x.IsConstant = false

            member x.Copy = NoRefASet inputReader :> aset<_>

            member x.GetReader () =
                let reader = new CopyReader<'a>(inputReader)
                if inputReader.ReferenceCount > 1 then reader.SetPassThru(false, false)

                reader :> _

    type ConstantSet<'a>(content : Lazy<ReferenceCountingSet<'a>>) =
        interface aset<'a> with
            member x.ReaderCount = 0
            member x.IsConstant = true

            member x.Copy = x :> aset<_>

            member x.GetReader () =
                Telemetry.timed GetConstantReaderProbe (fun () ->
                    let r = new OneShotReader<'a>(content.Value)
                    r :> IReader<_>
                )

        new(content : Lazy<list<'a>>) = ConstantSet<'a>(lazy (ReferenceCountingSet content.Value))
        new(content : Lazy<seq<'a>>) = ConstantSet<'a>(lazy (ReferenceCountingSet content.Value))
        new(l : list<'a>) = ConstantSet<'a> (lazy (ReferenceCountingSet l))
        new(l : array<'a>) = ConstantSet<'a> (lazy (ReferenceCountingSet l))
        new(l : seq<'a>) = ConstantSet<'a> (lazy (ReferenceCountingSet l))

    type private EmptySetImpl<'a> private() =
        static let emptySet = ConstantSet (lazy ([])) :> aset<'a>
        static member Instance = emptySet

    let private scoped (f : 'a -> 'b) =
        let scope = Ag.getContext()
        fun v -> Ag.useScope scope (fun () -> f v)

    
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
        ConstantSet(lazy (ReferenceCountingSet s)) :> aset<_>
    
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

    let delay (f : unit -> list<'a>) =
        let scope = Ag.getContext()
        ConstantSet(lazy (Ag.useScope scope f)) :> aset<_>

    /// <summary>
    /// returns a list of all elements currently in the adaptive set. 
    /// NOTE: will force the evaluation of the set.
    /// </summary>
    let toList (set : aset<'a>) =
        use r = set.GetReader()
        r.GetDelta(null) |> ignore
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
        r.GetDelta(null) |> ignore
        r.Content |> Seq.toArray

    /// <summary>
    /// creates a singleton set which will always contain
    /// the latest value of the given mod-cell.
    /// </summary>
    let ofMod (m : IMod<'a>) =
        if m.IsConstant then
            ConstantSet [m.GetValue(null)] :> aset<_>
        else
            AdaptiveSet(fun () -> ofMod m) :> aset<_>


    /// <summary>
    /// creates an aset containing all element from the
    /// original one but disposing them as they are removed
    /// </summary>
    let using (s : aset<'a>) =
        let noRef = s.Copy
        AdaptiveSet(fun () -> using <| noRef.GetReader()) :> aset<_>

    /// <summary>
    /// creates a mod-cell containing the set's content
    /// as IVersionedSet. Since the IVersionedSet is mutating
    /// (without changing its identity) the mod system is 
    /// prepared to track changes using the version.
    /// </summary>
    let toMod (s : aset<'a>) =
        if s.IsConstant then
            Mod.delay (fun () ->
                let r = s.GetReader()
                r.GetDelta(null) |> ignore
                r.Content :> IVersionedSet<_>
            )
        else
            let r = s.GetReader()

            let m = 
                [r] |> Mod.mapCustom (fun s ->
                    r.GetDelta(s) |> ignore
                    r.Content :> IVersionedSet<_>
                )
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
                r.Update(null)
                r.Content |> Seq.map f |> Seq.toList
            )
        else
            let scope = Ag.getContext()
            let noRef = set.Copy
            AdaptiveSet(fun () -> noRef.GetReader() |> map scope f) :> aset<'b>


    let mapUse (f : 'a -> 'b) (set : aset<'a>) = 
        if set.IsConstant then
            delay (fun () ->
                use r = set.GetReader()
                r.Update(null)
                r.Content |> Seq.map f |> Seq.toList
            )
        else
            let scope = Ag.getContext()
            let noRef = set.Copy
            AdaptiveSet(fun () -> noRef.GetReader() |> mapUsing scope f) :> aset<'b>


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
    /// applies the given function to a task-result and adaptively
    /// returns the resulting set. (the resulting set is empty until
    /// the task completes)
    /// </summary>
    let bindTask (f : 'a -> aset<'b>) (t : System.Threading.Tasks.Task<'a>) =
        let scope = Ag.getContext()
        AdaptiveSet(fun () -> t |> bindTask scope (fun v -> (f v).GetReader())) :> aset<'b>

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
    /// adaptively unions the given sets
    /// </summary>
    let union' (set : seq<aset<'a>>) : aset<'a> =
        let set = set |> Seq.toList
        if set |> List.forall (fun s -> s.IsConstant) then
            let allElements =
                set |> Seq.collect (fun s ->
                    let r = s.GetReader()
                    r.Update(null)
                    r.Content
                )
            ConstantSet(lazy (ReferenceCountingSet(allElements))) :> aset<_>
        else
            AdaptiveSet(fun () -> union (set |> Seq.map (fun s -> s.GetReader()) |> Seq.toList)) :> aset<_>

    /// <summary>
    /// adaptively unions the given sets, similar to union, nice for composing |> style with paps
    /// </summary>
    let unionTwo (x : aset<'a>) (y : aset<'a>) = union' [x;y]

    /// <summary>
    /// applies the given function to all elements in the set
    /// and unions all output-sets.
    /// NOTE: duplicates are handled correctly here meaning that
    ///       the supplied function may return overlapping sets.
    /// </summary>
    let collect (f : 'a -> aset<'b>) (set : aset<'a>) = 
        if set.IsConstant then
            let r = set.GetReader()
            r.GetDelta(null) |> ignore
            
            let innerSets = r.Content |> Seq.map f |> Seq.toList

            if innerSets |> List.forall (fun s -> s.IsConstant) then
                delay (fun () ->
                    innerSets |> List.collect (fun s ->
                        let r = s.GetReader()
                        r.Update(null)
                        r.Content |> Seq.toList
                    )
                )
            else
                innerSets |> union'
        else
            let scope = Ag.getContext()
            let noRef = set.Copy
            AdaptiveSet(fun () -> noRef.GetReader() |> collect scope (fun v -> (f v).GetReader())) :> aset<'b>



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
                r.Update(null)
                r.Content |> Seq.toList |> List.choose f
            )
        else
            let scope = Ag.getContext()
            let noRef = set.Copy
            AdaptiveSet(fun () -> noRef.GetReader() |> choose scope f) :> aset<'b>

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

    let collect'' (f : 'a -> seq<'b>) (set : aset<'a>) =
        let scope = Ag.getContext()
        if set.IsConstant then
            let r = set.GetReader()
            r.Update(null)
            ConstantSet(lazy (ReferenceCountingSet(r.Content |> Seq.collect f))) :> aset<_>
        else
            AdaptiveSet(fun () -> set.GetReader() |> ASetReaders.collect' scope f) :> aset<_>
  
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
            [r] |> Mod.mapCustom (fun s ->
                let mutable rem = false
                let delta = r.GetDelta(s)

                if not <| processDeltas delta then
                    sum := r.Content |> Seq.fold add zero

                !sum
            )

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
    let foldGroup (add : 'b -> 'a -> 'b) (sub : 'b -> 'a -> 'b) (zero : 'b) (s : aset<'a>) : IMod<'b> =
        let r = s.GetReader()
        let sum = ref zero

        let res =
            [r] |> Mod.mapCustom (fun s ->
                let delta = r.GetDelta(s)
                for d in delta do
                    match d with
                        | Add v -> sum := add !sum v
                        | Rem v -> sum := sub !sum v
                !sum
            )

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
    let private callbackTable = ConditionalWeakTable<obj, ConcurrentHashSet<IDisposable>>()

    let unsafeRegisterCallbackNoGcRoot (f : list<Delta<'a>> -> unit) (set : aset<'a>) =
        let m = set.GetReader()

        let result =
            m.AddEvaluationCallback(fun () ->
                m.GetDelta(null) |> f
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
    let unsafeRegisterCallbackKeepDisposable (f : list<Delta<'a>> -> unit) (set : aset<'a>) =
        let d = unsafeRegisterCallbackNoGcRoot f set
        undyingCallbacks.Add d |> ignore
        { new IDisposable with
            member x.Dispose() =
                d.Dispose()
                undyingCallbacks.Remove d |> ignore
        }
