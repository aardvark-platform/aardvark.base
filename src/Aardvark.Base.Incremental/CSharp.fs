namespace Aardvark.Base.Incremental.CSharp

open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base
open System.Collections
open System.Collections.Generic
open FSharp.Data.Adaptive

[<Extension; AbstractClass; Sealed>]
type Mod private() =
    static member Constant (v : 'a) = AVal.constant v

    static member Init(v : 'a) = AVal.init v

    /// <summary>
    /// AVal.delay
    /// </summary>
    static member LazyConstant (f : Func<'a>) : aval<'a> =
        AVal.delay f.Invoke

    static member Custom (f : Func<AdaptiveToken, 'a>) : aval<'a> =
        AVal.custom f.Invoke
        
    //static member Async(t : System.Threading.Tasks.Task<'a>, defaultValue : 'a) : aval<'a> =
    //    AVal.asyncWithDefault defaultValue (Async.AwaitTask t)

    //static member Async(f: Func<'a>, defaultValue : 'a) : aval<'a> =
    //    let a = async { return f.Invoke() }
    //    AVal.asyncWithDefault defaultValue a

    //static member LazyAsync(f : Func<'a>, defaultValue : 'a) : aval<'a> =
    //    let a = async { return f.Invoke() }
    //    AVal.asyncWithDefault defaultValue a

[<Extension; AbstractClass; Sealed>]
type ASet private() =
    static member Empty<'a> () = ASet.empty<'a>

    static member Single (v : 'a) = ASet.single v

    static member OfSeq (s : seq<'a>) = ASet.ofSeq s

    static member OfArray (a : 'a[]) = ASet.ofArray a

    static member OfList (l : List<'a>) = ASet.ofSeq l
    
    static member Create (s : seq<'a>) = ASet.ofSeq s

    static member Create (a : 'a[]) = ASet.ofArray a

    //static member Custom (f : Func<AdaptiveToken, Func<HashSet<'a>, HashSetDelta<'a>>>) : aset<'a> =
    //    ASet.ofReader (fun t -> f.Invoke(t).Invoke)

/// <summary> 
/// Represents a constant aset (using ASet.ofSeq) that allows to use the c# initializer syntax for construction
/// </summary>
type AdaptiveSet<'a>(content : seq<'a>) =
    let content = HashSet.ofSeq content
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
        member x.History = None
        member x.Content = s.Value.Content
        member x.IsConstant = s.Value.IsConstant
        member x.GetReader() = s.Value.GetReader()

    interface IEnumerable with
        member x.GetEnumerator() = (content :> IEnumerable).GetEnumerator()

    interface IEnumerable<'a> with
        member x.GetEnumerator() = (content :> IEnumerable<_>).GetEnumerator()

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

    static member Transaction =
        let t = new Transaction()

        let d = Transaction.makeCurrent t
      
        { new IDisposable with
            member x.Dispose() =
                t.Commit()
                t.Dispose()
                d.Dispose()
        }

module private ImplicitConversionHate =
    let inline addOutput (t:IAdaptiveObject) (o:IAdaptiveObject) = 
        t.Outputs.Add o |> ignore

    let inline removeOutput (t:IAdaptiveObject) (o:IAdaptiveObject) = 
        t.Outputs.Remove o |> ignore

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
    static member Map (this : aval<'a>, f : Func<'a, 'b>) =
        AVal.map f.Invoke this

    [<Extension>]
    static member Map (this : aval<'a>, other : aval<'b>, f : Func<'a, 'b, 'c>) =
        AVal.map2 (curry f.Invoke) this other
       
    [<Extension>]
    static member Compose (this : aval<'a>, other : aval<'b>, f : Func<'a, 'b, 'c>) =
        AVal.map2 (curry f.Invoke) this other

    [<Extension>]
    static member Compose (a : aval<'a>, b : aval<'b>, c : aval<'c>, f : Func<'a, 'b, 'c, 'd>) =
        let tup = AVal.map2 (fun a b -> (a,b)) a b
        AVal.map2 (fun (a,b) c -> f.Invoke(a, b, c)) tup c

    [<Extension>]
    static member Compose (a : aval<'a>, b : aval<'b>, c : aval<'c>, d : aval<'d>, f : Func<'a, 'b, 'c, 'd, 'e>) =
        let tup1 = AVal.map2 (fun a b -> (a,b)) a b
        let tup2 = AVal.map2 (fun c d -> (c,d)) c d
        AVal.map2 (fun (a,b) (c,d) -> f.Invoke(a, b, c, d)) tup1 tup2

    [<Extension>]
    static member Compose (a : aval<'a>, b : aval<'b>, c : aval<'c>, d : aval<'d>, e : aval<'e>, f : Func<'a, 'b, 'c, 'd, 'e, 'f>) =
        let tup1 = AVal.map2 (fun a b -> (a,b)) a b
        let tup2 = AVal.map2 (fun c d -> (c,d)) c d
        let tup3 = AVal.map2 (fun (c, d) e -> (c,d,e)) tup2 e
        AVal.map2 (fun (a,b) (c,d,e) -> f.Invoke(a, b, c, d, e)) tup1 tup3

    [<Extension>]
    static member Compose (this : seq<aval<'a>>, f : Func<seq<'a>, 'b>) =
        AVal.custom (fun t -> 
            let all = this |> Seq.map (fun v -> v.GetValue t) |> Seq.toArray
            f.Invoke(all :> seq<_>)
        )

    [<Extension>]
    static member Bind(this : aval<'a>, f : Func<'a, aval<'b>>) =
        AVal.bind f.Invoke this

    [<Extension>]
    static member Bind(this : aval<'a>, other : aval<'b>, f : Func<'a, 'b, aval<'c>>) =
        AVal.bind2 (fun l r -> f.Invoke(l,r)) this other

    [<Extension>]
    static member Bind(this : aval<'a>, f : Func<'a, aset<'b>>) =
        ASet.bind f.Invoke this

    [<Extension>]
    static member Bind(this : aval<'a>, other : aval<'b>, f : Func<'a, 'b, aset<'c>>) =
        let tup = AVal.map2 (fun a b -> a,b) this other
        tup |> ASet.bind (fun (l, r) -> f.Invoke(l,r))

    [<Extension>]
    static member Bind(this : aval<'a>, f : Func<'a, alist<'b>>) =
        AList.bind f.Invoke this

    [<Extension>]
    static member Bind(this : aval<'a>, other : aval<'b>, f : Func<'a, 'b, alist<'c>>) =
        let tup = AVal.map2 (fun a b -> a,b) this other
        tup |> AList.bind (fun (l, r) -> f.Invoke(l,r))

    [<Extension>]
    static member Bind(this : aval<'a>, f : Func<'a, amap<'b, 'c>>) =
        AMap.bind f.Invoke this

    [<Extension>]
    static member ToAdaptiveSet(this : aval<'a>) =
        ASet.ofAVal (this |> AVal.map HashSet.single)

    [<Extension>]
    static member ToAdaptiveList(this : aval<'a>) =
        AList.ofAVal (this |> AVal.map IndexList.single)

    [<Extension>]
    static member And(a : aval<bool>, b : aval<bool>) =
        AVal.map2 (&&) a b
        //AVal.bind (fun a -> if a then b else AVal.Constant false) a // better ?

    [<Extension>]
    static member Or(a : aval<bool>, b : aval<bool>) =
        AVal.map2 (||) a b
        //AVal.bind (fun a -> if a then AVal.Constant true else b) a // better ?

    [<Extension>]
    static member Not(a : aval<bool>) =
        AVal.map (not) a

 
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
    static member Select (this : aset<'a>, f : Func<'a, aval<'b>>) =
        ASet.mapA f.Invoke this

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
        ASet.collect (f.Invoke >> ASet.ofSeq) this

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
    static member Choose (this : aset<'a>, f : Func<'a, aval<Option<'b>>>) =
        ASet.chooseA f.Invoke this

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
    static member Flatten (this : aset<aval<'a>>) =
        ASet.flattenA this

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
    static member Where (this : aset<'a>, f : Func<'a, aval<bool>>) =
        ASet.filterA f.Invoke this

    /// <summary>
    /// ASet.union
    /// </summary>
    [<Extension>]
    static member Union (this : aset<'a>, other : aset<'a>) =
        ASet.union this other

    [<Extension>]
    static member ToMod (this : aset<'a>) =
        ASet.toAVal this

    [<Extension>]
    static member ToSeq (this : aset<'a>) =
        ASet.force this :> seq<_>

    [<Extension>]
    static member ToArray (this : aset<'a>) =
        ASet.force this |> HashSet.toArray

    [<Extension>]
    static member MapSet (this : aset<'a>, valueSelector : Func<'a, 'v>) =
        this |> AMap.mapSet valueSelector.Invoke 

    /// <summary>
    /// Statically test if an items is contained in the AdaptiveSet
    /// </summary>
    [<Extension>]
    static member Contains (this : aset<'a>, item : 'a) =
        this.Content |> AVal.force |> HashSet.contains item

    //[<Extension>]
    //static member ContainsMod (this : aset<'a>, [<ParamArray>] item : 'a[]) =
    //    this |> ASet.containsAll item

    //[<Extension>]
    //static member ContainsAny (this : aset<'a>, [<ParamArray>] item : 'a[]) =
    //    this |> ASet.containsAny item

    //[<Extension>]
    //static member ContainsAll (this : aset<'a>, [<ParamArray>] item : 'a[]) =
    //    this |> ASet.containsAll item


    //[<Extension>]
    //static member ContainsMod (this : aset<'a>, item : seq<'a>) =
    //    this |> ASet.containsAll item

    //[<Extension>]
    //static member ContainsAny (this : aset<'a>, item : seq<'a>) =
    //    this |> ASet.containsAny item

    //[<Extension>]
    //static member ContainsAll (this : aset<'a>, item : seq<'a>) =
    //    this |> ASet.containsAll item

    /// <summary> Provides direct access to IAdaptiveSet.GetReader as interface implementation is explicit and would requires cast. </summary>
    [<Extension>]
    static member GetReader (this : aset<'a>) =
        this.GetReader()

    [<Extension>]
    static member Count (this : aset<'a>) =
        ASet.count this

    [<Extension>]
    static member IsEmpty (this : aset<'a>) =
        this.Content |> AVal.map (fun x -> x.Count = 0)

    [<Extension>]
    static member Any (this : aset<'a>) =
        this.Content |> AVal.map (fun x -> x.Count > 0)

    [<Extension>]
    static member GetChanges (this : IHashSetReader<'a>) =
        this.GetChanges(AdaptiveToken.Top) |> HashSetDelta.toArray

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
    static member ToAMap(this : aset<'k*'v>) =
        this |> AMap.ofASet

    [<Extension>]
    static member Fold(this : aset<'a>, add : Func<'s, 'a, 's>, zero : 's) : aval<'s> =
        let addFun = fun s -> fun a -> add.Invoke(s, a)
        this |> ASet.fold addFun zero

    [<Extension>]
    static member FoldGroup(this : aset<'a>, add : Func<'s, 'a, 's>, sub : Func<'s, 'a, 's>, zero : 's) : aval<'s> =
        let addFun = fun s -> fun a -> add.Invoke(s, a)
        let subFun = fun s -> fun a -> sub.Invoke(s, a)
        this |> ASet.foldGroup addFun subFun zero

    [<Extension>]
    static member Sum(this : aset<int>) : aval<int> =
        this |> ASet.sum

    [<Extension>]
    static member Sum(this : aset<float>) : aval<float> =
        this |> ASet.sum

    [<Extension>]
    static member SumM(this : aset<aval<int>>) : aval<int> =
        this |> ASet.flattenA |> ASet.sum

    [<Extension>]
    static member SumM(this : aset<aval<float>>) : aval<float> =
        this |> ASet.flattenA |> ASet.sum

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
        HashSet.toArray this.Value

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
    static member Where (this : cset<'a>, f : Func<'a, aval<bool>>) =
        ASet.filterA f.Invoke this

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
        AList.toAVal this

    [<Extension>]
    static member ToSeq (this : alist<'a>) =
        AList.force this :> seq<_>

    [<Extension>]
    static member ToArray (this : alist<'a>) =
        AList.force this |> IndexList.toArray

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
        AList.concat this

    /// <summary>
    /// AList.concat
    /// <summary>
    [<Extension>]
    static member Concat (this : alist<alist<'a>>) =
        AList.collect id this

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
    static member Choose (this : alist<'a>, f : Func<'a, aval<Option<'b>>>) =
        AList.chooseA f.Invoke this


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
    static member Where (this : alist<'a>, f : Func<'a, aval<bool>>) =
        AList.filterA f.Invoke this

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
        IndexList.toArray this.Value
                

[<Extension; AbstractClass; Sealed>]
type AdaptiveMapExtensions private() =

    [<Extension>]
    static member ToAVal (this : amap<'k, 'v>) =
        AMap.toAVal this

    [<Extension>]
    static member ToAdaptiveSet (this : amap<'k, 'v>) =
        this |> AMap.toASet
 
