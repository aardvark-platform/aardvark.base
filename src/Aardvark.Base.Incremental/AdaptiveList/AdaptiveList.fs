namespace Aardvark.Base.Incremental

open System
open System.Runtime.CompilerServices
open System.Collections.Concurrent
open Aardvark.Base
open Aardvark.Base.Incremental.AListReaders


module AList =
    type AdaptiveList<'a>(newReader : unit -> IListReader<'a>) =
//        let state = TimeList<'a>()
        let readers = WeakSet<CopyReader<'a>>()

        let mutable inputReader = None
        let getReader() =
            match inputReader with
                | Some r -> r
                | None ->
                    let r = newReader()
                    inputReader <- Some r
                    r
//
//        let bringUpToDate () =
//            let r = getReader()
//            let delta = r.GetDelta ()
//            if not <| List.isEmpty delta then
//                delta |> apply state |> ignore
//                readers  |> Seq.iter (fun ri ->
//                    if ri.IsIncremental then
//                        ri.Emit delta
//                    else ri.Reset state
//                )

        interface alist<'a> with
            member x.GetReader () =
                //bringUpToDate()
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

    type ConstantList<'a>(rootTime : IOrder, content : seq<ISortKey * 'a>) =
        let content = List.ofSeq content
        interface alist<'a> with
            member x.GetReader () =
                let r = new OneShotReader<'a>(rootTime, content |> List.map Add)
                r :> IListReader<_>

    let private emptyTime = SimpleOrder.create()
    type private EmptyListImpl<'a> private() =
        static let emptySet = ConstantList(emptyTime, []) :> alist<'a>
        static member Instance = emptySet

    let private scoped (f : 'a -> 'b) =
        let scope = Ag.getContext()
        fun v -> Ag.useScope scope (fun () -> f v)

    let private callbackTable = ConditionalWeakTable<obj, ConcurrentHashSet<IDisposable>>()
//    type private CallbackSubscription(m : IAdaptiveObject, cb : unit -> unit, live : ref<bool>, reader : IDisposable, set : ConcurrentHashSet<IDisposable>) =
//        
//        member x.Dispose() = 
//            if !live then
//                live := false
//                reader.Dispose()
//                m.MarkingCallbacks.Remove cb |> ignore
//                set.Remove x |> ignore
//
//        interface IDisposable with
//            member x.Dispose() = x.Dispose()
//
//        override x.Finalize() =
//            try x.Dispose()
//            with _ -> ()
//

    let empty<'a> : alist<'a> =
        EmptyListImpl<'a>.Instance

    let single (v : 'a) =
        let r = SimpleOrder.create()
        ConstantList(r, [r.After r.Root :> ISortKey, v]) :> alist<_>

    let ofSeq (s : seq<'a>) =
        let r = SimpleOrder.create()
        let current = ref r.Root
        let elements =
            s |> Seq.toList
              |> List.map (fun e ->
                    let t = r.After !current
                    current := t
                    t :> ISortKey,e
                 )

        ConstantList(r, elements) :> alist<_>

    let ofList (l : list<'a>) =
        ofSeq l

    let ofArray (a : 'a[]) =
        ofSeq a

    // TODO: fix this crazy implementation
    let ofASet (s : aset<'a>) =
        AdaptiveList(fun () -> s.GetReader() |> sortWith (fun a b -> a.GetHashCode().CompareTo(b.GetHashCode()))) :> alist<_>
        //AdaptiveList(fun () -> ofSet s) :> alist<_>

    let toASet (l : alist<'a>) =
        ASet.AdaptiveSet(fun () -> toSetReader l) :> aset<_>

    let toSeq (set : alist<'a>) =
        use r = set.GetReader()
        r.GetDelta(null) |> ignore
        r.Content.Values |> Seq.toArray :> seq<_>

    let toList (set : alist<'a>) =
        set |> toSeq |> Seq.toList

    let toArray (set : alist<'a>) =
        set |> toSeq |> Seq.toArray

    let ofMod (m : IMod<'a>) =
        AdaptiveList(fun () -> ofMod m) :> alist<_>

    let toMod (s : alist<'a>) =
        let r = s.GetReader()
        let m = 
            [r :> IAdaptiveObject] |> Mod.mapCustom (fun s ->
                r.GetDelta(s) |> ignore
                r.Content
            )
        r.AddOutput m
        m

    let map (f : 'a -> 'b) (set : alist<'a>) = 
        let scope = Ag.getContext()
        AdaptiveList(fun () -> set.GetReader() |> map scope f) :> alist<'b>

    let mapKey (f : ISortKey -> 'a -> 'b) (set : alist<'a>) = 
        let scope = Ag.getContext()
        AdaptiveList(fun () -> set.GetReader() |> mapKey scope f) :> alist<'b>

    let collect (f : 'a -> alist<'b>) (set : alist<'a>) = 
        let scope = Ag.getContext()
        AdaptiveList(fun () -> set.GetReader() |> collect scope (fun v -> (f v).GetReader())) :> alist<'b>

    let choose (f : 'a -> Option<'b>) (set : alist<'a>) =
        let scope = Ag.getContext()
        AdaptiveList(fun () -> set.GetReader() |> choose scope f) :> alist<'b>

    let filter (f : 'a -> bool) (set : alist<'a>) =
        choose (fun v -> if f v then Some v else None) set

    let concat (set : alist<alist<'a>>) =
        collect id set

    let concat' (set : seq<alist<'a>>) =
        concat (ofSeq set)

    let collect' (f : 'a -> alist<'b>) (set : seq<'a>) =
        set |> Seq.map f |> concat'
    
    let bind (f : 'a -> alist<'b>) (m : IMod<'a>) =
        let scope = Ag.getContext()
        AdaptiveList(fun () -> m |> bind scope (fun a -> (f a).GetReader())) :> alist<'b>

    let bind2 (f : 'a -> 'b -> alist<'c>) (ma : IMod<'a>) (mb : IMod<'b>) =
        let scope = Ag.getContext()
        AdaptiveList(fun () -> bind2 scope (fun a b -> (f a b).GetReader()) ma mb) :> alist<'c>


    let mapM (f : 'a -> IMod<'b>) (s : alist<'a>) =
        s |> collect (fun v ->
            v |> f |> ofMod
        )

    let filterM (f : 'a -> IMod<bool>) (s : alist<'a>) =
        s |> collect (fun v ->
            v |> f |> bind (fun b -> if b then single v else empty)
        )

    let chooseM (f : 'a -> IMod<Option<'b>>) (s : alist<'a>) =
        s |> collect (fun v ->
            v |> f |> bind (fun b -> match b with | Some v -> single v | _ -> empty)
        )

    let flattenM (s : alist<IMod<'a>>) =
        s |> collect ofMod

    

    /// <summary>
    /// registers a callback for execution whenever the
    /// list's value might have changed and returns a disposable
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
    let unsafeRegisterCallbackNoGcRoot (f : list<Delta<ISortKey * 'a>> -> unit) (list : alist<'a>) =
        let m = list.GetReader()

        let result = 
            m.AddEvaluationCallback(fun () ->
                m.GetDelta(null) |> f
            )


        let set = callbackTable.GetOrCreateValue(list)
        set.Add result |> ignore
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
    let unsafeRegisterCallbackKeepDisposable f list =
        let d = unsafeRegisterCallbackNoGcRoot f list
        undyingCallbacks.Add d |> ignore
        { new IDisposable with
            member x.Dispose() =
                d.Dispose()
                undyingCallbacks.Remove d |> ignore
        }

  
[<AutoOpen>]
module ``ASet sorting functions`` =      
    
    module ASet =
        let sortWith (cmp : 'a -> 'a -> int) (s : aset<'a>) =
            AList.AdaptiveList(fun () -> s.GetReader() |> sortWith cmp) :> alist<_>

        let sortBy (f : 'a -> 'b) (s : aset<'a>) =
            let cmp (a : 'a) (b : 'a) = compare (f a) (f b)
            s |> sortWith cmp

        let sort (s : aset<'a>) =
            sortWith compare s

        let toAList (s : aset<'a>) =
            AList.ofASet s

        let ofAList (l : alist<'a>) =
            AList.toASet l