namespace Aardvark.Base.Incremental

open Aardvark.Base
open System
open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent

type WeakSet<'a when 'a : not struct> private(inner : ConcurrentDictionary<Weak<'a>, int>) =
        
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
     
