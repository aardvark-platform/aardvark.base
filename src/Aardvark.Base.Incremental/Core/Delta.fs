namespace Aardvark.Base.Incremental

open System.Threading
open System.Collections.Generic

/// <summary>
/// Delta defines the two basic set-operations
/// add and remove with their respective value
/// </summary>
type Delta<'a> = 
    | Add of 'a 
    | Rem of 'a with

    member x.Value =
        match x with
            | Add v -> v
            | Rem v -> v


module Delta =
    /// <summary>
    /// cleans a given list of deltas by removing redundant
    /// occurances of deltas. For example [Add 1; Rem 1; Add 1]
    /// will be transformed to [Add 1]. 
    /// Note that this transformation will respect duplicates
    /// which means that there might still be multiple additions
    /// removals for the same value.
    /// </summary>
    let clean (l : list<Delta<'a>>) =
        if List.isEmpty l then 
            l
        else
            let store = Dictionary<obj, 'a * ref<int>>()
            let nullCount = ref 0

            let inc a =
                if a :> obj = null then nullCount := !nullCount + 1
                else
                    match store.TryGetValue (a :> obj) with
                        | (true, (_,v)) -> v := !v + 1
                        | _ -> store.[a] <- (a, ref 1)

            let dec a =
                if a :> obj = null then nullCount := !nullCount - 1
                else
                    match store.TryGetValue (a :> obj) with
                        | (true, (_,v)) -> v := !v - 1
                        | _ -> store.[a] <- (a, ref -1)

            for e in l do
                match e with
                    | Add v -> inc v
                    | Rem v -> dec v

            [ for (KeyValue(_,(k,v))) in store do
                for i in 1..!v do yield Add k
                for i in 1..-(!v) do yield Rem k 

              for i in 1..!nullCount do yield Add Unchecked.defaultof<_>
              for i in 1..-(!nullCount) do yield Rem Unchecked.defaultof<_>
            ]

/// <summary>
/// a simple module for creating unique ids
/// </summary>
[<AutoOpen>]
module UniqeIds =
    let mutable private currentId = 0
    let newId() = Interlocked.Increment &currentId
