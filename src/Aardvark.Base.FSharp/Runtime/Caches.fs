namespace Aardvark.Base

open System
open System.Collections.Generic
open System.Runtime.CompilerServices

[<AutoOpen>]
module Caches =

    type UnaryCache<'a, 'b when 'a : not struct and 'b : not struct> private(store : Option<ConditionalWeakTable<'a, 'b>>, f : 'a -> 'b) =
        let store = 
            match store with
                | Some s -> s
                | None -> ConditionalWeakTable<'a, 'b>()

        member x.Store = store

        member x.Invoke (a : 'a) =
            lock store (fun () ->
                match store.TryGetValue(a) with
                    | (true, b) -> b
                    | _ ->
                        let b = f a
                        store.Add(a, b)
                        b
            )

        new(f : 'a -> 'b) = UnaryCache<'a, 'b>(None, f)
        new(store : ConditionalWeakTable<'a, 'b>, f : 'a -> 'b) = UnaryCache<'a, 'b>(Some store, f)

    type BinaryCache<'a, 'b, 'c when 'a : not struct and 'b : not struct and 'c : not struct>(f : 'a -> 'b -> 'c) =
        inherit OptimizedClosures.FSharpFunc<'a, 'b, 'c>()

        // ISSUE: if the computation/cache value holds references to their inputs 'a' and 'b'
        //        and 'a' is no longer held by the application 
        //         -> 'a' and the computation will not be collected !!
        //        if 'b' runs out of scope -> 'b' and the computation is collected

        let store = ConditionalWeakTable<'a, ConditionalWeakTable<'b, 'c>>()

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
                            | (true, v) -> v
                            | _ ->
                                let v = f a b
                                inner.Add(b, v)
                                v
                    | _ ->
                        let v = f a b
                        let inner = ConditionalWeakTable<'b, 'c>()
                        inner.Add(b, v)
                        store.Add(a, inner)
                        v
            )

    //type private Key = { a : WeakReference; b : WeakReference }

    type BinaryCache2<'a, 'b, 'c when 'a : not struct and 'b : not struct and 'c : not struct>(f : 'a -> 'b -> 'c) =

        // ISSUE: if c holds a or b then a or b will not be collected
        let refStore = ConditionalWeakTable<obj, (WeakReference*obj)>() // WeakReference of inputs + finalizer
        let entryMap = Dictionary<WeakReference, Dictionary<WeakReference, struct ((WeakReference*WeakReference)*(WeakReference*WeakReference))>>() // (ha, hb), (hb, ha)
        let valueStore = ConditionalWeakTable<(WeakReference*WeakReference), 'c>()

       
        let getHandle input =
            match refStore.TryGetValue(input) with
            | (true, handle) -> fst handle
            | _ -> 
                let handle = WeakReference(input)
                let finalizer = { new Object() with
                                    override x.Finalize() =
                                        // remove all computations sharing 'input' as key
                                        lock refStore (fun () ->
                                            match entryMap.TryGetValue handle with
                                            | (true, entries) ->
                                                      entryMap.Remove handle |> ignore
                                                      for struct(ka, kb) in entries.Values do // ka=(ha,hb); kb=(hb,ha)
                                                        let hb = 
                                                            if not (Object.ReferenceEquals(ka, null)) then
                                                                Some (snd ka) // remove ka from valueStore when cleaning up hb
                                                            elif not (Object.ReferenceEquals(kb, null)) then
                                                                Some (fst kb) // remove kb from valueStore when cleaning up hb
                                                            else 
                                                                None

                                                        match hb with
                                                        | Some hb -> 
                                                            match entryMap.TryGetValue hb with
                                                            | (true, compB) ->
                                                                match compB.TryGetValue handle with
                                                                | (true, struct(ka, kb)) ->
                                                                    if not (Object.ReferenceEquals(ka, null)) then
                                                                        valueStore.Remove(ka)  |> ignore
                                                                    elif not (Object.ReferenceEquals(kb, null)) then
                                                                        valueStore.Remove(kb) |> ignore
                                                                    compB.Remove(hb) |> ignore
                                                                    if compB.Count = 0 then
                                                                        entryMap.Remove(hb) |> ignore
                                                                | _ -> () // WARN
                                                            | _ -> () // WARN
                                                        | _ -> () // nop
                                                            
                                            | _ -> () // already no entries ? might be if cleaned up when secondary key got was finalized
                                            )
                                 }
                refStore.Add(input, (handle, finalizer))
                handle

        let createKey ha hb =
            // create key (a, b)
            let key = (ha, hb)
            // also register for handle b
            let secondMap = entryMap.GetCreate(hb, (fun _ -> Dictionary<_,_>()))
            // in the secondary map there should not be a key for ha with (ha, hb)
            //  -> if it is the case -> overwrite
            match secondMap.TryGetValue ha with
            | (true, struct(ka, kb)) ->
                    secondMap.[ha] <- struct(ka, key)
            | _ -> secondMap.Add(ha, struct(Unchecked.defaultof<_>, key))
            key
  
        member x.Invoke(a : 'a, b : 'b) =
            lock refStore (fun () ->
                let ha = getHandle a
                let hb = getHandle b                

                // try find value key
                let key = match entryMap.TryGetValue ha with
                          | (true, other) -> match other.TryGetValue hb with
                                             | (true, struct(key, kb)) -> // key=(ha, hb); may exist: kb=(hb, ha)
                                                               if Object.ReferenceEquals(key, null) then // only kb exists
                                                                  let key = createKey ha hb
                                                                  other.[hb] <- struct (key, kb)
                                                                  key
                                                               else
                                                                  key
                                             | _ -> // register key
                                                    let key = createKey ha hb
                                                    other.Add(hb, struct(key, Unchecked.defaultof<_>))
                                                    key

                          | _ -> let other = Dictionary<_,_>()
                                 entryMap.Add(ha, other)

                                 let key = createKey ha hb
                                 other.Add(hb, struct(key, Unchecked.defaultof<_>))
                                 key

                match valueStore.TryGetValue(key) with
                | (true, value) -> value
                | _ -> 
                        let value = f a b
                        valueStore.Add(key, value)
                        value
            )
