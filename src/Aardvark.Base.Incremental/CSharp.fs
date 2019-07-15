﻿namespace Aardvark.Base.Incremental.CSharp

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

    /// <summary>
    /// Mod.delay
    /// </summary>
    static member LazyConstant (f : Func<'a>) : IMod<'a> =
        Mod.delay f.Invoke

    static member Custom (f : Func<AdaptiveToken, 'a>) : IMod<'a> =
        Mod.custom f.Invoke
        
    static member Async(t : System.Threading.Tasks.Task<'a>, defaultValue : 'a) : IMod<'a> =
        Mod.asyncWithDefault defaultValue (Async.AwaitTask t)

    static member Async(f: Func<'a>, defaultValue : 'a) : IMod<'a> =
        let a = async { return f.Invoke() }
        Mod.asyncWithDefault defaultValue a

    static member LazyAsync(f : Func<'a>, defaultValue : 'a) : IMod<'a> =
        let a = async { return f.Invoke() }
        Mod.asyncWithDefault defaultValue a

[<Extension; AbstractClass; Sealed>]
type ASet private() =
    static member Empty<'a> () = ASet.empty<'a>

    static member Single (v : 'a) = ASet.single v

    static member OfSeq (s : seq<'a>) = ASet.ofSeq s

    static member OfArray (a : 'a[]) = ASet.ofArray a

    static member OfList (l : List<'a>) = ASet.ofSeq l
    
    static member Create (s : seq<'a>) = ASet.ofSeq s

    static member Create (a : 'a[]) = ASet.ofArray a

    static member Custom (f : Func<AdaptiveToken, Func<hrefset<'a>, hdeltaset<'a>>>) : aset<'a> =
        ASet.custom (fun t -> f.Invoke(t).Invoke)

/// <summary> 
/// Represents a constant aset (using ASet.ofSeq) that allows to use the c# initializer syntax for construction
/// </summary>
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


[<Extension; AbstractClass; Sealed>]
type AList private() =
    static member Empty<'a> () = AList.empty<'a>

    static member Single (v : 'a) = AList.single v

    static member OfSeq (s : seq<'a>) = AList.ofSeq s

    static member OfArray (a : 'a[]) = AList.ofArray a

    static member OfList (l : List<'a>) = AList.ofSeq l

    static member Create (s : seq<'a>) = AList.ofSeq s

    static member Create (a : 'a[]) = AList.ofArray a
    

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
open Aardvark.Base.Geometry

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
    static member Compose (a : IMod<'a>, b : IMod<'b>, c : IMod<'c>, f : Func<'a, 'b, 'c, 'd>) =
        let tup = Mod.map2 (fun a b -> (a,b)) a b
        Mod.map2 (fun (a,b) c -> f.Invoke(a, b, c)) tup c

    [<Extension>]
    static member Compose (a : IMod<'a>, b : IMod<'b>, c : IMod<'c>, d : IMod<'d>, f : Func<'a, 'b, 'c, 'd, 'e>) =
        let tup1 = Mod.map2 (fun a b -> (a,b)) a b
        let tup2 = Mod.map2 (fun c d -> (c,d)) c d
        Mod.map2 (fun (a,b) (c,d) -> f.Invoke(a, b, c, d)) tup1 tup2

    [<Extension>]
    static member Compose (a : IMod<'a>, b : IMod<'b>, c : IMod<'c>, d : IMod<'d>, e : IMod<'e>, f : Func<'a, 'b, 'c, 'd, 'e, 'f>) =
        let tup1 = Mod.map2 (fun a b -> (a,b)) a b
        let tup2 = Mod.map2 (fun c d -> (c,d)) c d
        let tup3 = Mod.map2 (fun (c, d) e -> (c,d,e)) tup2 e
        Mod.map2 (fun (a,b) (c,d,e) -> f.Invoke(a, b, c, d, e)) tup1 tup3

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
    static member Bind(this : IMod<'a>, f : Func<'a, amap<'b, 'c>>) =
        AMap.bind f.Invoke this

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

    [<Extension>]
    static member And(a : IMod<bool>, b : IMod<bool>) =
        Mod.map2 (&&) a b
        //Mod.bind (fun a -> if a then b else Mod.Constant false) a // better ?

    [<Extension>]
    static member Or(a : IMod<bool>, b : IMod<bool>) =
        Mod.map2 (||) a b
        //Mod.bind (fun a -> if a then Mod.Constant true else b) a // better ?

    [<Extension>]
    static member Not(a : IMod<bool>) =
        Mod.map (not) a

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

    /// <summary>
    /// ASet.map
    /// </summary>
    [<Extension>]
    static member Select (this : aset<'a>, f : Func<'a, 'b>) =
        ASet.map f.Invoke this

    /// <summary>
    /// ASet.mapM
    /// </summary>
    [<Extension>]
    static member Select (this : aset<'a>, f : Func<'a, IMod<'b>>) =
        ASet.mapM f.Invoke this

    /// <summary>
    /// ASet.collect
    /// </summary>
    [<Extension>]
    static member SelectMany (this : aset<'a>, f : Func<'a, aset<'b>>) =
        ASet.collect f.Invoke this

    /// <summary>
    /// ASet.collect'
    /// </summary>
    [<Extension>]
    static member SelectMany (this : aset<'a>, f : Func<'a, seq<'b>>) =
        ASet.collect' f.Invoke this

    /// <summary>
    /// ASet.choose
    /// </summary>
    [<Extension>]
    static member Choose (this : aset<'a>, f : Func<'a, Option<'b>>) =
        ASet.choose f.Invoke this

    /// <summary>
    /// ASet.chooseM
    /// </summary>
    [<Extension>]
    static member Choose (this : aset<'a>, f : Func<'a, IMod<Option<'b>>>) =
        ASet.chooseM f.Invoke this

    /// <summary>
    /// ASet.mapUse
    /// </summary>
    [<Extension>]
    static member MapUse (this : aset<'a>, f : Func<'a, 'b>) =
        ASet.mapUse f.Invoke this

    /// <summary>
    /// ASet.union
    /// </summary>
    [<Extension>]
    static member Flatten (this : seq<aset<'a>>) =
        ASet.union (ASet.ofSeq this)

    /// <summary>
    /// ASet.unionMany
    /// </summary>
    [<Extension>]
    static member Flatten (this : aset<aset<'a>>) =
        ASet.unionMany this

    /// <summary>
    /// ASet.collect
    /// </summary>
    [<Extension>]
    static member Flatten (this : aset<seq<'a>>) =
        ASet.collect ASet.ofSeq this

    /// <summary>
    /// ASet.flattenM
    /// </summary>
    [<Extension>]
    static member Flatten (this : aset<IMod<'a>>) =
        ASet.flattenM this

    /// <summary>
    /// ASet.filter
    /// </summary>
    [<Extension>]
    static member Where (this : aset<'a>, f : Func<'a, bool>) =
        ASet.filter f.Invoke this

    /// <summary>
    /// ASet.filterM
    /// </summary>
    [<Extension>]
    static member Where (this : aset<'a>, f : Func<'a, IMod<bool>>) =
        ASet.filterM f.Invoke this

    /// <summary>
    /// ASet.union
    /// </summary>
    [<Extension>]
    static member Union (this : aset<'a>, other : aset<'a>) =
        ASet.union this other

    [<Extension>]
    static member ToMod (this : aset<'a>) =
        ASet.toMod this

    [<Extension>]
    static member ToSeq (this : aset<'a>) =
        ASet.toSeq this

    [<Extension>]
    static member ToArray (this : aset<'a>) =
        ASet.toArray this

    [<Extension>]
    static member MapSet (this : aset<'a>, valueSelector : Func<'a, 'v>) =
        this |> AMap.mapSet valueSelector.Invoke 

    /// <summary>
    /// Statically test if an items is contained in the AdaptiveSet
    /// </summary>
    [<Extension>]
    static member Contains (this : aset<'a>, item : 'a) =
        this.Content |> Mod.force |> HRefSet.contains item

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

    /// <summary> Provides direct access to IAdaptiveSet.GetReader as interface implementation is explicit and would requires cast. </summary>
    [<Extension>]
    static member GetReader (this : aset<'a>) =
        this.GetReader()

    [<Extension>]
    static member Count (this : aset<'a>) =
        ASet.count this

    [<Extension>]
    static member IsEmpty (this : aset<'a>) =
        this.Content |> Mod.map (fun x -> x.Count = 0)

    [<Extension>]
    static member Any (this : aset<'a>) =
        this.Content |> Mod.map (fun x -> x.Count > 0)

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

    /// <summary>
    /// Returns ChangeableSet cast to <cref=IAdaptiveSet{T}/>
    /// </summary>
    [<Extension>]
    static member AsAdaptiveSet(this : cset<'a>) =
        this :> aset<'a>

    /// <summary>
    /// Copies the ChangeableSet to an array.
    /// This extension avoid errors of resolving the ambigious extensions IAdaptiveSet<T>.ToArray and IEnumerable<T>.ToArray.
    /// </summary>
    [<Extension>]
    static member ToArray (this : cset<'a>) =
        Seq.toArray this

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
    static member ToSeq (this : alist<'a>) =
        AList.toSeq this

    [<Extension>]
    static member ToArray (this : alist<'a>) =
        AList.toArray this

    /// <summary>
    /// AList.map
    /// <summary>
    [<Extension>]
    static member Select (this : alist<'a>, f : Func<'a, 'b>) =
        AList.map f.Invoke this
    
    /// <summary>
    /// AList.collect
    /// <summary>
    [<Extension>]
    static member SelectMany (this : alist<'a>, f : Func<'a, alist<'b>>) =
        AList.collect f.Invoke this

    /// <summary>
    /// AList.collect
    /// <summary>
    [<Extension>]
    static member SelectMany (this : seq<'a>, f : Func<'a, alist<'b>>) =
        AList.collect f.Invoke (AList.ofSeq this)

    /// <summary>
    /// AList.concat
    /// <summary>
    [<Extension>]
    static member Concat (this : seq<alist<'a>>) =
        AList.concat (AList.ofSeq this)

    /// <summary>
    /// AList.concat
    /// <summary>
    [<Extension>]
    static member Concat (this : alist<alist<'a>>) =
        AList.concat this

    /// <summary>
    /// AList.choose
    /// <summary>
    [<Extension>]
    static member Choose (this : alist<'a>, f : Func<'a, Option<'b>>) =
        AList.choose f.Invoke this

    /// <summary>
    /// AList.chooseM
    /// <summary>
    [<Extension>]
    static member Choose (this : alist<'a>, f : Func<'a, IMod<Option<'b>>>) =
        AList.chooseM f.Invoke this

    /// <summary>
    /// AList.mapUse
    /// </summary>
    [<Extension>]
    static member MapUse (this : alist<'a>, f : Func<'a, 'b>) =
        AList.mapUse f.Invoke this

    /// <summary>
    /// AList.filter
    /// <summary>
    [<Extension>]
    static member Where (this : alist<'a>, f : Func<'a, bool>) =
        AList.filter f.Invoke this

    /// <summary>
    /// AList.filterM
    /// <summary>
    [<Extension>]
    static member Where (this : alist<'a>, f : Func<'a, IMod<bool>>) =
        AList.filterM f.Invoke this

    /// <summary>
    /// AList.append
    /// <summary>
    [<Extension>]
    static member Concat (this : alist<'a>, other : alist<'a>) =
        AList.append this other

    /// <summary>
    /// Converts the adaptive list to a set.
    /// Extension to manually fix conflicts of ambigious extensions of IAdaptiveList and IEnumerable
    /// </summary>
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

    /// <summary>
    /// Returns ChangeableList cast to <cref=IAdaptiveSet{T}/>
    /// </summary>
    [<Extension>]
    static member AsAdaptiveList(this : clist<'a>) =
        this :> alist<'a>

    /// <summary>
    /// Copies the ChangeableList to an array.
    /// This extension avoid errors of resolving the ambigious extensions IAdaptiveList<T>.ToArray and IEnumerable<T>.ToArray.
    /// </summary>
    [<Extension>]
    static member ToArray(this : clist<'a>) =
        Seq.toArray this
                

[<Extension; AbstractClass; Sealed>]
type COrderedSetExtensions private() =

    /// <summary>
    /// Returns ChangeableOrderedSet cast to <cref=IAdaptiveSet{T}/>
    /// Extension to manually fix conflicts of ambigious extensions of IAdaptiveSet and IEnumerable
    /// </summary>
    [<Extension>]
    static member AsAdaptiveSet(this : corderedset<'a>) =
        this :> aset<'a>

    /// <summary>
    /// Returns ChangeableOrderedSet cast to <cref=IAdaptiveList{T}/>
    /// Extension to manually fix conflicts of ambigious extensions of IAdaptiveSet and IEnumerable
    /// </summary>
    [<Extension>]
    static member AsAdaptiveList(this : corderedset<'a>) =
        this :> alist<'a>

    /// <summary>
    /// Copies the ChangeableOrderedSet to an array.
    /// This extension avoid errors of resolving the ambigious extensions IAdaptiveSet<T>.ToArray and IEnumerable<T>.ToArray.
    /// </summary>
    [<Extension>]
    static member ToArray(this : corderedset<'a>) =
        Seq.toArray this
        

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
