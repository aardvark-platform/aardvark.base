namespace Aardvark.Base.Incremental

open Aardvark.Base
open System
open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent


/// <summary>
/// represents a set of elements while not introducing
/// a "garbage-collector-edge". This is desirable in 
/// scenarios where one needs to enumerate a set of things
/// which shall be allowed to be garbage-collected.
/// </summary>
type WeakSet<'a when 'a : not struct> private(inner : HashSet<Weak<'a>>) =
    let lockObj = obj()

    let keys() = inner |> Seq.choose(function (Strong o) -> Some o | _ -> None)

    let contains (item : 'a) = inner.Contains (Weak item)
        

    let compareSeq (other : seq<'a>) =
        let distinctOther = 
            match other with
                | :? HashSet<'a> as o -> o
                | _ -> HashSet other

        let mutable both = 0
        let mutable onlyMe = 0

        for o in keys() do
            if distinctOther.Contains o then
                both <- both + 1
            else
                onlyMe <- onlyMe + 1

        let onlyOther = distinctOther.Count - both

        (both, onlyMe, onlyOther)

    // we use ConcurrentDictionary here for our implementation
    // since it can safely be modified while being enumerated
    // Note that we don't actually need support for concurrency
    // here but it makes the implementation easier.
    // Also note that the WeakSet is only pruned during enumeration
    // which means that all contained Weak objects may leak until the
    // set is enumerated (this of course doesn't apply to the weak's content)

    

    member internal x.Inner = inner

    interface ICollection<'a> with
        member x.Add v = x.Add v |> ignore
        member x.Remove v = x.Remove v
        member x.Contains v = x.Contains v
        member x.Clear() = x.Clear()
        member x.CopyTo(arr,idx) = x.CopyTo(arr,idx)
        member x.Count = x.Count
        member x.IsReadOnly = false

    interface ISet<'a> with
        member x.Add item = x.Add item
        member x.ExceptWith other = x.ExceptWith other
        member x.IntersectWith other = x.IntersectWith other
        member x.UnionWith other = x.UnionWith other
        member x.SymmetricExceptWith other = x.SymmetricExceptWith other

        member x.IsProperSubsetOf other = x.IsProperSubsetOf other
        member x.IsProperSupersetOf other = x.IsProperSupersetOf other
        member x.IsSubsetOf other = x.IsSubsetOf other
        member x.IsSupersetOf other = x.IsSupersetOf other
        member x.Overlaps other = x.Overlaps other
        member x.SetEquals other = x.SetEquals other

    interface IEnumerable with
        member x.GetEnumerator() = new WeakSetEnumerator<'a>(x) :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = new WeakSetEnumerator<'a>(x) :> _


    override x.ToString() =
        lock lockObj (fun () -> x |> Seq.toList |> sprintf "weakSet %A")

    member x.ToWeakArray() =
        lock lockObj (fun () -> 
            inner |> Seq.toArray
        )

    member x.CopyTo(arr : 'a[], index : int) =
        lock lockObj (fun () -> 
            let mutable index = index
            for e in x do
                arr.[index] <- e
                index <- index + 1
        )

    member x.Count =
        lock lockObj (fun () -> inner.Count)

    member x.Contains(value : 'a) =
        lock lockObj (fun () -> inner.Contains (Weak value))

    member x.Add (value : 'a) =
        lock lockObj (fun () -> inner.Add (Weak value))

    member x.Remove (value : 'a) =
        lock lockObj (fun () -> inner.Remove (Weak value))

    member x.Clear() =
        lock lockObj (fun () -> inner.Clear())

    member x.UnionWith (elements : #seq<'a>) =
        lock lockObj (fun () -> 
            let add = elements |> Seq.map (fun e -> Weak e)
            inner.UnionWith add
        )
            
    member x.ExceptWith (elements : #seq<'a>) =
        lock lockObj (fun () -> 
            let rem = elements |> Seq.map (fun e -> Weak e)
            inner.ExceptWith rem
        )
            
    member x.IntersectWith (other : seq<'a>) =
        lock lockObj (fun () -> 
            inner.IntersectWith(other |> Seq.map (fun e -> Weak e))
        )

    member x.SymmetricExceptWith (other : seq<'a>) =
        lock lockObj (fun () -> 
            inner.SymmetricExceptWith(other |> Seq.map (fun e -> Weak e))
        )

    member x.IsSubsetOf (other : seq<'a>) =
        lock lockObj (fun () -> 
            match compareSeq other with
                | (_, 0, _) -> true
                | _ -> false
        )

    member x.IsSupersetOf (other : seq<'a>) =
        lock lockObj (fun () -> 
            match compareSeq other with
                | (_, _, 0) -> true
                | _ -> false
        )

    member x.IsProperSubsetOf (other : seq<'a>) =
        lock lockObj (fun () -> 
            match compareSeq other with
                | (_, 0, o) -> o > 0
                | _ -> false
        )

    member x.IsProperSupersetOf (other : seq<'a>) =
        lock lockObj (fun () -> 
            match compareSeq other with
                | (_, m, 0) -> m > 0
                | _ -> false
        )

    member x.Overlaps (other : seq<'a>) =
        lock lockObj (fun () -> 
            let (b,_,_) = compareSeq other
            b > 0
        )

    member x.SetEquals (other : seq<'a>) =
        lock lockObj (fun () -> 
            match compareSeq other with
                | (_, 0, 0) -> true
                | _ -> false
        )

    member x.IsEmpty =
        lock lockObj (fun () -> inner.Count = 0)

    new() = WeakSet(HashSet())

// defines an enumerator cleaning the WeakSet during its enumeration
and private WeakSetEnumerator<'a when 'a : not struct> =
    struct
        val mutable public inputSet : HashSet<Weak<'a>>
        val mutable public e : IEnumerator<Weak<'a>>
        val mutable public current : 'a

        member x.MoveNext() =
            if x.e.MoveNext() then
                match x.e.Current with
                    | Strong v -> 
                        x.current <- v
                        true
                    | _ ->
                        x.inputSet.Remove x.e.Current |> ignore
                        x.MoveNext()
            else
                false

        member x.Current = x.current

        interface IEnumerator with
            member x.MoveNext() = x.MoveNext()
            member x.Current = x.Current :> _
            member x.Reset() = 
                x.e.Reset()
                x.current <- Unchecked.defaultof<'a>
                    
        interface IEnumerator<'a> with
            member x.Current = x.Current

        interface IDisposable with
            member x.Dispose() = ()


        new (w : WeakSet<'a>) =
            let inner = w.Inner
            { inputSet = inner; e = (w.ToWeakArray() :> seq<_>).GetEnumerator(); current = Unchecked.defaultof<'a> }
    end
     
