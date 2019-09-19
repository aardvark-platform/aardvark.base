namespace Aardvark.Base

open System
open System.Collections.Generic
open System.Runtime.CompilerServices

[<AutoOpen>]
module Caches =

    type UnaryCache<'a, 'b when 'a : not struct and 'b : not struct> private(store : Option<ConditionalWeakTable<'a, WeakReference<'b>>>, f : 'a -> 'b) =
        let store = 
            match store with
                | Some s -> s
                | None -> ConditionalWeakTable<'a, WeakReference<'b>>()

        member x.Store = store

        member x.Invoke (a : 'a) =
            lock store (fun () ->
                match store.TryGetValue(a) with
                    | (true, b) -> 
                        match b.TryGetTarget() with
                        | (true, b) -> b
                        | _ -> 
                            store.Remove a |> ignore
                            let b = f a
                            store.Add(a, WeakReference<'b>(b))
                            b
                    | _ ->
                        let b = f a
                        store.Add(a, WeakReference<'b>(b))
                        b
            )

        new(f : 'a -> 'b) = UnaryCache<'a, 'b>(None, f)
        new(store : ConditionalWeakTable<'a, WeakReference<'b>>, f : 'a -> 'b) = UnaryCache<'a, 'b>(Some store, f)

    type BinaryCache<'a, 'b, 'c when 'a : not struct and 'b : not struct and 'c : not struct>(f : 'a -> 'b -> 'c) =
        inherit OptimizedClosures.FSharpFunc<'a, 'b, 'c>()

        // ISSUE: if the computation/cache value holds references to their inputs 'a' and 'b'
        //        and 'a' is no longer held by the application 
        //         -> 'a' and the computation will not be collected !!
        //        if 'b' runs out of scope -> 'b' and the computation is collected

        let store = ConditionalWeakTable<'a, ConditionalWeakTable<'b, WeakReference<'c>>>()

        override x.Invoke(a : 'a) =
            lock store (fun () ->
                match store.TryGetValue(a) with
                | (true, inner) ->
                    UnaryCache<'b, 'c>(inner, f a).Invoke
                | _ ->
                    let inner = ConditionalWeakTable()
                    store.Add(a, inner)
                    UnaryCache<'b, 'c>(inner, f a).Invoke
            )

        override x.Invoke(a : 'a, b : 'b) =
            lock store (fun () ->
                match store.TryGetValue(a) with
                | (true, inner) ->
                    match inner.TryGetValue(b) with
                    | (true, v) -> 
                        match v.TryGetTarget() with
                        | (true, v) -> v
                        | _ ->  
                            inner.Remove b |> ignore
                            let v = f a b
                            inner.Add(b, WeakReference<'c>(v))
                            v

                    | _ ->
                        let v = f a b
                        inner.Add(b, WeakReference<'c>(v))
                        v
                | _ ->
                    let v = f a b
                    let inner = ConditionalWeakTable<'b, WeakReference<'c>>()
                    inner.Add(b, WeakReference<'c>(v))
                    store.Add(a, inner)
                    v
            )
