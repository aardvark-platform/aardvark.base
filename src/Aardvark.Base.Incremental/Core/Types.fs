namespace Aardvark.Base.Incremental

open System.Threading
open System.Collections.Generic

type Delta<'a> = 
    | Add of 'a 
    | Rem of 'a


module Delta =
    let clean (l : list<Delta<'a>>) =
        if List.isEmpty l then l
        else
            let store = Dictionary<obj, 'a * ref<int>>()

            let inc a =
                match store.TryGetValue (a :> obj) with
                    | (true, (_,v)) -> v := !v + 1
                    | _ -> store.[a] <- (a, ref 1)

            let dec a =
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
            ]

[<AutoOpen>]
module UniqeIds =
    let mutable private currentId = 0
    let newId() = Interlocked.Increment &currentId
