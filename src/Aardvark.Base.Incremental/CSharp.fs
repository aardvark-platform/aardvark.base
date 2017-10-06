namespace Aardvark.Base.Incremental.CSharp

open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base
open System.Collections
open System.Collections.Generic

[<Extension; AbstractClass; Sealed>]
type Mod private() =
    static member Constant (v : 'a) = Mod.constant v

    static member Init(v : 'a) = Mod.init v

    static member LazyConstant (f : Func<'a>) : IMod<'a> =
        Mod.delay f.Invoke

    static member Custom (f : Func<AdaptiveToken, 'a>) : IMod<'a> =
        Mod.custom f.Invoke

    static member MapCustom (f : Func<AdaptiveToken, 'a>,  [<ParamArray>] inputs : IAdaptiveObject[] ) : IMod<'a> =
        Mod.mapCustom f.Invoke (List.ofArray(inputs))

    static member Async(t : System.Threading.Tasks.Task<'a>, defaultValue : 'a) : IMod<'a> =
        Mod.asyncWithDefault defaultValue (Async.AwaitTask t)

    static member Async(f: Func<'a>, defaultValue : 'a) : IMod<'a> =
        let a = async { return f.Invoke() }
        Mod.asyncWithDefault defaultValue a

    static member LazyAsync(f : Func<'a>, defaultValue : 'a) : IMod<'a> =
        let a = async { return f.Invoke() }
        Mod.asyncWithDefault defaultValue a
        
type AdaptiveSet<'a>(content : seq<'a>) =
    let content = HashSet content
    let s = lazy (ASet.ofSeq content)

    member x.Count = content.Count
    
    member x.Add v = 
        if s.IsValueCreated then
            failwith ("cannot add to AdaptiveSet once it has been used by the mod-system.\r\n" +
                      "add shall only be used in C#'s initializer syntax")
        content.Add v

    override x.ToString() =
        sprintf "aset %A" (content |> Seq.toList)

    interface aset<'a> with
        member x.Content = s.Value.Content
        member x.IsConstant = s.Value.IsConstant
        member x.GetReader() = s.Value.GetReader()

    interface IEnumerable with
        member x.GetEnumerator() = (content :> IEnumerable).GetEnumerator()

    interface IEnumerable<'a> with
        member x.GetEnumerator() = content.GetEnumerator() :> IEnumerator<_>

    new([<ParamArray>] values : 'a[]) = AdaptiveSet(values :> seq<_>)
    new() = AdaptiveSet Seq.empty

//[<Extension; AbstractClass; Sealed>]
//type AdaptiveSet private() =
//
//    static member Empty<'a>() : aset<'a> = ASet.empty
//
//    static member Single (v : 'a) = ASet.single v


[<AbstractClass; Sealed>]
type Adaptive private() =

    static member TryGetCurrentTransaction([<Out>] transaction : byref<Transaction>) : bool =
        match getCurrentTransaction() with
            | Some t -> 
                transaction <- t
                true
            | _ ->
                false

    static member SetCurrentTransaction(t : Transaction) =
        setCurrentTransaction (Some t)

    static member ReleaseCurrentTransaction() =
        setCurrentTransaction None

    static member Transaction =
        let t = new Transaction()
        let old = Transaction.Current // directly use current here, getCurrentTransaction will also return a currently running (finalizing) transaction
        setCurrentTransaction (Some t)
      
        { new IDisposable with
            member x.Dispose() =
                setCurrentTransaction old
                t.Commit()
                t.Dispose()
        }

module private ImplicitConversionHate =
    let inline addOutput (t:IAdaptiveObject) (o:IAdaptiveObject) = 
        t.AddOutput o

    let inline removeOutput (t:IAdaptiveObject) (o:IAdaptiveObject) = 
        t.RemoveOutput o

    let inline addMarkingCallback (t:IAdaptiveObject) (mc: unit -> unit ) = 
        t.AddMarkingCallback mc

    let inline markOutdated (t:IAdaptiveObject) = 
        t.MarkOutdated()


open CallbackExtensions
[<Extension; AbstractClass; Sealed>]
type AdaptiveObjectExtensions private() =

    [<Extension>]
    static member AddOutput (this : IAdaptiveObject, o : IAdaptiveObject) : unit =
        ImplicitConversionHate.addOutput this o

    [<Extension>]
    static member RemoveOutput (this : IAdaptiveObject, o : IAdaptiveObject) : unit =
        ImplicitConversionHate.removeOutput this o

    [<Extension>]
    static member AddMarkingCallback (this : IAdaptiveObject, o : Action) :IDisposable =
        ImplicitConversionHate.addMarkingCallback this o.Invoke
        
    [<Extension>]
    static member MarkOutdated (this : IAdaptiveObject) : unit =
        ImplicitConversionHate.markOutdated this

[<Extension; AbstractClass; Sealed>]
type ModExtensions private() =
  
    [<Extension>]
    static member Map (this : IMod<'a>, f : Func<'a, 'b>) =
        Mod.map f.Invoke this

    [<Extension>]
    static member Map (this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, 'c>) =
        Mod.map2 (curry f.Invoke) this other
        
    [<Extension>]
    static member MapFast (this : IMod<'a>, f : Func<'a, 'b>) =
        Mod.mapFast f.Invoke this
        
    [<Extension>]
    static member MapFast (this : IMod, f : Func<obj, 'b>) =
        Mod.mapFastObj f.Invoke this

    [<Extension>]
    static member Cast (this : IMod) =
        Mod.cast this

    [<Extension>]
    static member Compose (this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, 'c>) =
        Mod.map2 (curry f.Invoke) this other

    [<Extension>]
    static member Compose (this : seq<IMod<'a>>, f : Func<seq<'a>, 'b>) =
        Mod.mapN (fun a -> f.Invoke(a)) this

    [<Extension>]
    static member Bind(this : IMod<'a>, f : Func<'a, IMod<'b>>) =
        Mod.bind f.Invoke this

    [<Extension>]
    static member Bind(this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, IMod<'c>>) =
        Mod.bind2 (fun l r -> f.Invoke(l,r)) this other

    [<Extension>]
    static member Bind(this : IMod<'a>, f : Func<'a, aset<'b>>) =
        ASet.bind f.Invoke this

    [<Extension>]
    static member Bind(this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, aset<'c>>) =
        ASet.bind2 (fun l r -> f.Invoke(l,r)) this other

    [<Extension>]
    static member Bind(this : IMod<'a>, f : Func<'a, alist<'b>>) =
        AList.bind f.Invoke this

    [<Extension>]
    static member Bind(this : IMod<'a>, other : IMod<'b>, f : Func<'a, 'b, alist<'c>>) =
        AList.bind2 (fun l r -> f.Invoke(l,r)) this other

    [<Extension>]
    static member Eager(this : IMod<'a>) =
        Mod.onPush this

    [<Extension>]
    static member Eager(this : IMod<'a>, eq : Func<'a, 'a, bool>) =
        Mod.onPushCustomEq (fun a b -> eq.Invoke(a,b)) this

    [<Extension>]
    static member Lazy(this : IMod<'a>) =
        Mod.onPull this

    [<Extension>]
    static member ToAdaptiveSet(this : IMod<'a>) =
        Mod.toASet this

    [<Extension>]
    static member ToAdaptiveList(this : IMod<'a>) =
        Mod.toAList this

    /// <summary> see Mod/ModModule.unsafeRegisterCallbackNoGcRoot </summary>
    [<Extension>]
    [<Obsolete("use UnsafeRegisterCallbackNoGcRoot or UnsafeRegisterCallbackKeepDisposable instead")>]
    static member RegisterCallback(this : IMod<'a>, callback : Action<'a>) =
        this |> Mod.unsafeRegisterCallbackNoGcRoot callback.Invoke

    /// <summary> see Mod/ModModule.unsafeRegisterCallbackKeepDisposable </summary>
    [<Extension>]
    static member UnsafeRegisterCallbackKeepDisposable(this : IMod<'a>, callback : Action<'a>) =
        this |> Mod.unsafeRegisterCallbackKeepDisposable callback.Invoke

    /// <summary> see Mod/ModModule.unsafeRegisterCallbackNoGcRoot </summary>
    [<Extension>]
    static member UnsafeRegisterCallbackNoGcRoot(this : IMod<'a>, callback : Action<'a>) =
        this |> Mod.unsafeRegisterCallbackNoGcRoot callback.Invoke

[<Extension; AbstractClass; Sealed>]
type AdaptiveSetExtensions private() =

    [<Extension>]
    static member Select (this : aset<'a>, f : Func<'a, 'b>) =
        ASet.map f.Invoke this

    [<Extension>]
    static member Select (this : aset<'a>, f : Func<'a, IMod<'b>>) =
        ASet.mapM f.Invoke this

    [<Extension>]
    static member SelectMany (this : aset<'a>, f : Func<'a, aset<'b>>) =
        ASet.collect f.Invoke this

    [<Extension>]
    static member SelectMany (this : aset<'a>, f : Func<'a, seq<'b>>) =
        ASet.collect' f.Invoke this

    [<Extension>]
    static member Choose (this : aset<'a>, f : Func<'a, Option<'b>>) =
        ASet.choose f.Invoke this

    [<Extension>]
    static member Choose (this : aset<'a>, f : Func<'a, IMod<Option<'b>>>) =
        ASet.chooseM f.Invoke this

    [<Extension>]
    static member MapUse (this : aset<'a>, f : Func<'a, 'b>) =
        ASet.mapUse f.Invoke this

    [<Extension>]
    static member Flatten (this : seq<aset<'a>>) =
        ASet.union (ASet.ofSeq this)

    [<Extension>]
    static member Flatten (this : aset<aset<'a>>) =
        ASet.unionMany this

    [<Extension>]
    static member Flatten (this : aset<seq<'a>>) =
        ASet.collect ASet.ofSeq this

    [<Extension>]
    static member Flatten (this : aset<IMod<'a>>) =
        ASet.flattenM this

    [<Extension>]
    static member Where (this : aset<'a>, f : Func<'a, bool>) =
        ASet.filter f.Invoke this

    [<Extension>]
    static member Where (this : aset<'a>, f : Func<'a, IMod<bool>>) =
        ASet.filterM f.Invoke this

    [<Extension>]
    static member Union (this : aset<'a>, other : aset<'a>) =
        ASet.union this other

    [<Extension>]
    static member ToMod (this : aset<'a>) =
        ASet.toMod this

    [<Extension>]
    static member MapSet (this : aset<'a>, valueSelector : Func<'a, 'v>) =
        this |> AMap.mapSet valueSelector.Invoke 

    [<Extension>]
    static member ContainsMod (this : aset<'a>, [<ParamArray>] item : 'a[]) =
        this |> ASet.containsAll item

    [<Extension>]
    static member ContainsAny (this : aset<'a>, [<ParamArray>] item : 'a[]) =
        this |> ASet.containsAny item

    [<Extension>]
    static member ContainsAll (this : aset<'a>, [<ParamArray>] item : 'a[]) =
        this |> ASet.containsAll item


    [<Extension>]
    static member ContainsMod (this : aset<'a>, item : seq<'a>) =
        this |> ASet.containsAll item

    [<Extension>]
    static member ContainsAny (this : aset<'a>, item : seq<'a>) =
        this |> ASet.containsAny item

    [<Extension>]
    static member ContainsAll (this : aset<'a>, item : seq<'a>) =
        this |> ASet.containsAll item


    [<Extension>]
    static member GetReader (this : aset<'a>) =
        this.GetReader()

    [<Extension>]
    static member GetOperations (this : ISetReader<'a>) =
        this.GetOperations(AdaptiveToken.Top) |> HDeltaSet.toArray

    /// <summary> see ASet/ASetModule.unsafeRegisterCallbackNoGcRoot </summary>
    [<Extension>]
    static member UnsafeRegisterCallbackNoGcRoot(this : aset<'a>, callback : Action<SetOperation<'a>[]>) =
        this |> ASet.unsafeRegisterCallbackNoGcRoot (fun deltas ->
            deltas |> List.toArray |> callback.Invoke
        )

    /// <summary> see ASet/ASetModule.unsafeRegisterCallbackNoGcRoot </summary>
    [<Extension>]
    [<Obsolete("use UnsafeRegisterCallbackNoGcRoot or UnsafeRegisterCallbackKeepDisposable instead")>]
    static member RegisterCallback(this : aset<'a>, callback : Action<SetOperation<'a>[]>) =
        this |> ASet.unsafeRegisterCallbackNoGcRoot (fun deltas ->
            deltas |> List.toArray |> callback.Invoke
        )

    /// <summary> see ASet/ASetModule.unsafeRegisterCallbackKeepDisposable </summary>
    [<Extension>]
    static member UnsafeRegisterCallbackKeepDisposable(this : aset<'a>, callback : Action<SetOperation<'a>[]>) =
        this |> ASet.unsafeRegisterCallbackKeepDisposable (fun deltas ->
            deltas |> List.toArray |> callback.Invoke
        )

    [<Extension>]
    static member OrderBy (this : aset<'a>, f : Func<'a, 'b>) =
        this |> ASet.sortBy f.Invoke

    [<Extension>]
    static member OrderWith (this : aset<'a>, cmp : IComparer<'a>) =
        this |> ASet.sortWith (curry cmp.Compare)

    [<Extension>]
    static member ToAdaptiveList (this : aset<'a>) =
        this |> ASet.toAList

    [<Extension>]
    static member GroupBy(this : aset<'a>, f : Func<'a, 'b>) =
        this |> ASet.groupBy f.Invoke

    [<Extension>]
    static member ToAMap(this : aset<'k*'v>) =
        this |> AMap.ofASet

    [<Extension>]
    static member Fold(this : aset<'a>, add : Func<'s, 'a, 's>, zero : 's) : IMod<'s> =
        let addFun = fun s -> fun a -> add.Invoke(s, a)
        this |> ASet.fold addFun zero

    [<Extension>]
    static member FoldGroup(this : aset<'a>, add : Func<'s, 'a, 's>, sub : Func<'s, 'a, 's>, zero : 's) : IMod<'s> =
        let addFun = fun s -> fun a -> add.Invoke(s, a)
        let subFun = fun s -> fun a -> sub.Invoke(s, a)
        this |> ASet.foldGroup addFun subFun zero

    [<Extension>]
    static member Sum(this : aset<int>) : IMod<int> =
        this |> ASet.sum

    [<Extension>]
    static member Sum(this : aset<float>) : IMod<float> =
        this |> ASet.sum

    [<Extension>]
    static member SumM(this : aset<IMod<int>>) : IMod<int> =
        this |> ASet.flattenM |> ASet.sum

    [<Extension>]
    static member SumM(this : aset<IMod<float>>) : IMod<float> =
        this |> ASet.flattenM |> ASet.sum

[<Extension; AbstractClass; Sealed>]
type ChangeableSetExtensions private() =

    [<Extension>]
    static member Select (this : cset<'a>, f : Func<'a, 'b>) =
        ASet.map f.Invoke this

    [<Extension>]
    static member SelectMany (this : cset<'a>, f : Func<'a, aset<'b>>) =
        ASet.collect f.Invoke this

    [<Extension>]
    static member Flatten (this : cset<aset<'a>>) =
        ASet.union this

    [<Extension>]
    static member Flatten (this : cset<seq<'a>>) =
        ASet.collect ASet.ofSeq this

    [<Extension>]
    static member Where (this : cset<'a>, f : Func<'a, bool>) =
        ASet.filter f.Invoke this

    [<Extension>]
    static member Where (this : cset<'a>, f : Func<'a, IMod<bool>>) =
        ASet.filterM f.Invoke this

    [<Extension>]
    static member ContainsMod (this : cset<'a>, [<ParamArray>] item : 'a[]) =
        this |> ASet.containsAll item

    [<Extension>]
    static member ContainsAny (this : cset<'a>, [<ParamArray>] item : 'a[]) =
        this |> ASet.containsAny item

    [<Extension>]
    static member ContainsAll (this : cset<'a>, [<ParamArray>] item : 'a[]) =
        this |> ASet.containsAll item


    [<Extension>]
    static member ContainsAny (this : cset<'a>, item : seq<'a>) =
        this |> ASet.containsAny item

    [<Extension>]
    static member ContainsAll (this : cset<'a>, item : seq<'a>) =
        this |> ASet.containsAll item

    [<Extension>]
    static member OrderBy (this : cset<'a>, f : Func<'a, 'b>) =
        this |> ASet.sortBy f.Invoke

    [<Extension>]
    static member OrderWith (this : cset<'a>, cmp : IComparer<'a>) =
        this |> ASet.sortWith (curry cmp.Compare)

    [<Extension>]
    static member ToAdaptiveList (this : cset<'a>) =
        this |> ASet.toAList


[<Extension; AbstractClass; Sealed>]
type AdaptiveListExtensions private() =

    [<Extension>]
    static member ToMod (this : alist<'a>) =
        AList.toMod this

    [<Extension>]
    static member Select (this : alist<'a>, f : Func<'a, 'b>) =
        AList.map f.Invoke this

    [<Extension>]
    static member SelectMany (this : alist<'a>, f : Func<'a, alist<'b>>) =
        AList.collect f.Invoke this

    [<Extension>]
    static member SelectMany (this : seq<'a>, f : Func<'a, alist<'b>>) =
        AList.collect f.Invoke (AList.ofSeq this)

    [<Extension>]
    static member Concat (this : seq<alist<'a>>) =
        AList.concat (AList.ofSeq this)

    [<Extension>]
    static member Concat (this : alist<alist<'a>>) =
        AList.concat this

    [<Extension>]
    static member Where (this : alist<'a>, f : Func<'a, bool>) =
        AList.filter f.Invoke this

    [<Extension>]
    static member Where (this : alist<'a>, f : Func<'a, IMod<bool>>) =
        AList.filterM f.Invoke this

    [<Extension>]
    static member Concat (this : alist<'a>, other : alist<'a>) =
        AList.append this other

    [<Extension>]
    static member ToAdaptiveSet (this : alist<'a>) =
        this |> AList.toASet

    /// <summary> see AList/AListModule.unsafeRegisterCallbackNoGcRoot </summary>
    [<Extension>]
    static member UnsafeRegisterCallbackNoGcRoot(this : alist<'a>, callback : Action<array<Index * ElementOperation<'a>>>) =
        this |> AList.unsafeRegisterCallbackNoGcRoot (fun deltas ->
            deltas |> List.toArray |> callback.Invoke
        )
        
    /// <summary> see AList/AListModule.unsafeRegisterCallbackKeepDisposable </summary>
    [<Extension>]
    static member UnsafeRegisterCallbackKeepDisposable(this : alist<'a>, callback : Action<array<Index * ElementOperation<'a>>>) =
        this |> AList.unsafeRegisterCallbackKeepDisposable (fun deltas ->
            deltas |> List.toArray |> callback.Invoke
        )


[<Extension; AbstractClass; Sealed>]
type CListExtensions private() =

    [<Extension>]
    static member Select (this : clist<'a>, f : Func<'a, 'b>) =
        AList.map f.Invoke this

    [<Extension>]
    static member SelectMany (this : clist<'a>, f : Func<'a, alist<'b>>) =
        AList.collect f.Invoke this

    [<Extension>]
    static member Where (this : clist<'a>, f : Func<'a, bool>) =
        AList.filter f.Invoke this

    [<Extension>]
    static member Where (this : clist<'a>, f : Func<'a, IMod<bool>>) =
        AList.filterM f.Invoke this

    [<Extension>]
    static member Concat (this : clist<'a>, other : alist<'a>) =
        AList.append this other


[<Extension; AbstractClass; Sealed>]
type AdaptiveMapExtensions private() =

    [<Extension>]
    static member ToMod (this : amap<'k, 'v>) =
        AMap.toMod this

    [<Extension>]
    static member ToAdaptiveSet (this : amap<'k, 'v>) =
        this |> AMap.toASet
 
//[<Extension; AbstractClass; Sealed>]
//type COrderedSetExtensions private() =       
//    
//    // directly provide set view as callback, otherwise a cast to either aset or alist is necessary
//
//    /// <summary> see AList/AListModule.unsafeRegisterCallbackNoGcRoot </summary>
//    [<Extension>]
//    static member UnsafeRegisterCallbackNoGcRoot(this : corderedset<'a>, callback : Action<Delta<'a>[]>) =
//        this |> ASet.unsafeRegisterCallbackNoGcRoot (fun deltas ->
//            deltas |> List.toArray |> callback.Invoke
//        )
//
//    /// <summary> see AList/AListModule.unsafeRegisterCallbackKeepDisposable </summary>
//    [<Extension>]
//    static member UnsafeRegisterCallbackKeepDisposable(this : corderedset<'a>, callback : Action<Delta<'a>[]>) =
//        this |> ASet.unsafeRegisterCallbackKeepDisposable (fun deltas ->
//            deltas |> List.toArray |> callback.Invoke
//        )
//

[<Extension; AbstractClass; Sealed>]
type EvaluationExtensions private() =

    [<Extension>]
    static member EvaluateTopLevel (this : Func<'a>) =
        EvaluationUtilities.evaluateTopLevel this.Invoke
