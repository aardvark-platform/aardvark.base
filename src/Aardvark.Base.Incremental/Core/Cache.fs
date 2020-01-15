﻿namespace Aardvark.Base.Incremental

open System.Collections.Generic
open Aardvark.Base

/// <summary>
/// Cache represents a cached function which can be 
/// invoked and revoked. invoke increments the reference
/// count for a specific argument (possibly causing the 
/// function to be executed) whereas revoke decreases the
/// reference count and removes the cache entry whenever
/// the reference count is 0.
/// </summary>
type Cache<'a, 'b>(f : 'a -> 'b) =  

    static let isNull : 'a -> bool =
        if typeof<'a>.IsValueType then fun _ -> false
        else fun v -> isNull (v :> obj)

    static let comparer =
        {
            new IEqualityComparer<'a> with
                member x.GetHashCode (a : 'a) = Unchecked.hash a
                member x.Equals(a : 'a, b : 'a) = Unchecked.equals a b
        }

    let cache = Dictionary<'a, 'b * ref<int>>(comparer)
    let mutable nullCache = None

    /// <summary>
    /// Clear removes all entries from the Cache and
    /// executes a function for all removed cache entries.
    /// this function is helpful if the contained values
    /// are (for example) disposable resources.
    /// </summary>
    member x.Clear(remove : 'b -> unit) =
        for (KeyValue(_,(v,_))) in cache do 
            remove v
        cache.Clear()
        match nullCache with
            | Some (v,_) -> 
                remove v
                nullCache <- None
            | None -> ()

    /// <summary>
    /// invoke returns the function value associated
    /// with the given argument (possibly executing the function)
    /// and increases the associated reference count.
    /// </summary>
    member x.Invoke (v : 'a) =
        if isNull v then
            match nullCache with
                | Some (r, ref) -> 
                    ref := !ref + 1
                    r
                | None ->
                    let r = f v
                    nullCache <- Some(r, ref 1)
                    r
        else
            match cache.TryGetValue v with
                | (true, (r, ref)) -> 
                    ref := !ref + 1
                    r
                | _ ->
                    let r = f v
                    cache.[v] <- (r, ref 1)
                    r
    /// <summary>
    /// revoke returns the function value associated
    /// with the given argument and decreases its reference count.
    /// </summary>
    member x.RevokeAndGetDeleted (v : 'a) =
        if isNull v then
            match nullCache with
                | Some (r, ref) -> 
                    ref := !ref - 1
                    if !ref = 0 then
                        nullCache <- None
                        (true, r)
                    else
                        (false, r)
                | None -> failwithf "cannot revoke null"
        else
            match cache.TryGetValue v with
                | (true, (r, ref)) -> 
                    ref := !ref - 1
                    if !ref = 0 then
                        cache.Remove v |> ignore
                        (true, r)
                    else
                        (false, r)
                | _ -> failwithf "cannot revoke unknown value: %A" v

    member x.RevokeAndGetDeletedTotal (v : 'a) =
        if isNull v then
            match nullCache with
                | Some (r, ref) -> 
                    ref := !ref - 1
                    if !ref = 0 then
                        nullCache <- None
                        Some (true, r)
                    else
                        Some(false, r)
                | None -> Log.warn "cannot revoke null"; None
        else
            match cache.TryGetValue v with
                | (true, (r, ref)) -> 
                    ref := !ref - 1
                    if !ref = 0 then
                        cache.Remove v |> ignore
                        Some(true, r)
                    else
                        Some(false, r)
                | _ -> Log.warn "cannot revoke unknown value: %A" v;None

    member x.Revoke (v : 'a) =
        x.RevokeAndGetDeleted v |> snd
        
    member x.Keys = cache.Keys :> seq<_>
    member x.Values = cache.Values |> Seq.map fst

    new(scope : Ag.Scope, f : 'a -> 'b) = Cache(fun a -> Ag.useScope scope (fun () -> f a))


/// <summary>
/// Cache represents a cached function which can be 
/// invoked and revoked. invoke increments the reference
/// count for a specific argument (possibly causing the 
/// function to be executed) whereas revoke decreases the
/// reference count and removes the cache entry whenever
/// the reference count is 0.
/// </summary>
type Cache2<'a, 'b, 'c>(scope : Ag.Scope, f : 'a -> 'b -> 'c) =  
    let cache = Dictionary<obj, 'c * ref<int>>()
    let mutable nullCache = None

    /// <summary>
    /// Clear removes all entries from the Cache and
    /// executes a function for all removed cache entries.
    /// this function is helpful if the contained values
    /// are (for example) disposable resources.
    /// </summary>
    member x.Clear(remove : 'c -> unit) =
        for (KeyValue(_,(v,_))) in cache do remove v
        cache.Clear()
        match nullCache with
            | Some (v,_) -> 
                remove v
                nullCache <- None
            | None -> ()

    /// <summary>
    /// invoke returns the function value associated
    /// with the given argument (possibly executing the function)
    /// and increases the associated reference count.
    /// </summary>
    member x.Invoke (a : 'a, b : 'b) =
        let ao = a :> obj
        let bo = b :> obj
        if isNull ao && isNull bo then
            match nullCache with
                | Some (r, ref) -> 
                    ref := !ref + 1
                    r
                | None ->
                    let r = Ag.useScope scope (fun () -> f a b)
                    nullCache <- Some(r, ref 1)
                    r
        else
            let key =
                if isNull ao then bo
                elif isNull bo then ao
                else (ao,bo) :> obj

            match cache.TryGetValue key with
                | (true, (r, ref)) -> 
                    ref := !ref + 1
                    r
                | _ ->
                    let r = Ag.useScope scope (fun () -> f a b)
                    cache.[key] <- (r, ref 1)
                    r

    /// <summary>
    /// revoke returns the function value associated
    /// with the given argument and decreases its reference count.
    /// </summary>
    member x.Revoke (a : 'a, b : 'b) =
        let ao = a :> obj
        let bo = b :> obj
        if isNull ao && isNull bo then
            match nullCache with
                | Some (r, ref) -> 
                    ref := !ref - 1
                    if !ref = 0 then
                        nullCache <- None
                    r
                | None -> failwithf "cannot revoke null"
        else
            let key =
                if isNull ao then bo
                elif isNull bo then ao
                else (ao,bo) :> obj

            match cache.TryGetValue key with
                | (true, (r, ref)) -> 
                    ref := !ref - 1
                    if !ref = 0 then
                        cache.Remove key |> ignore
                    r
                | _ -> failwithf "cannot revoke unknown value: %A" key


