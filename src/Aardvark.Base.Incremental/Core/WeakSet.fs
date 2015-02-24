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
type WeakSet<'a when 'a : not struct> private(inner : ConcurrentDictionary<Weak<'a>, int>) =
        
    // we use ConcurrentDictionary here for our implementation
    // since it can safely be modified while being enumerated
    // Note that we don't actually need support for concurrency
    // here but it makes the implementation easier.
    // Also note that the WeakSet is only pruned during enumeration
    // which means that all contained Weak objects may leak until the
    // set is enumerated (this of course doesn't apply to the weak's content)

    member internal x.Inner = inner

    interface IEnumerable with
        member x.GetEnumerator() = new WeakSetEnumerator<'a>(x) :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = new WeakSetEnumerator<'a>(x) :> _

    override x.ToString() =
        x |> Seq.toList |> sprintf "weakSet %A"

    member x.Add (value : 'a) =
        inner.TryAdd (Weak value, 0)

    member x.Remove (value : 'a) =
        let mutable foo = 0
        inner.TryRemove (Weak value, &foo)

    member x.Clear() =
        inner.Clear()

    member x.UnionWith (elements : #seq<'a>) =
        let add = elements |> Seq.map (fun e -> Weak e)
        for r in add do
            inner.TryAdd(r, 0) |> ignore
            
    member x.ExceptWith (elements : #seq<'a>) =
        let rem = elements |> Seq.map (fun e -> Weak e)
        let mutable foo = 0
        for r in rem do
            inner.TryRemove (r, &foo) |> ignore

    member x.IsEmpty =
        inner.Count = 0

    new() = WeakSet(ConcurrentDictionary(1,0))

// defines an enumerator cleaning the WeakSet during its enumeration
and private WeakSetEnumerator<'a when 'a : not struct> =
    struct
        val mutable public inputSet : ConcurrentDictionary<Weak<'a>, int>
        val mutable public e : IEnumerator<KeyValuePair<Weak<'a>, int>>
        val mutable public current : 'a

        member x.MoveNext() =
            if x.e.MoveNext() then
                match x.e.Current.Key with
                    | Strong v -> 
                        x.current <- v
                        true
                    | _ ->
                        x.inputSet.TryRemove x.e.Current.Key |> ignore
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
            { inputSet = inner; e = inner.GetEnumerator(); current = Unchecked.defaultof<'a> }
    end
     
