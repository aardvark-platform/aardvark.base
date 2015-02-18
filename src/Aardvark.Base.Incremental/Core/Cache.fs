namespace Aardvark.Base.Incremental

open System.Collections.Generic


type Cache<'a, 'b>(scope : Aardvark.Base.Ag.Scope, f : 'a -> 'b) =  
    let cache = Dictionary<obj, 'b * ref<int>>()

    member x.Clear(remove : 'b -> unit) =
        for (KeyValue(_,(v,_))) in cache do remove v
        cache.Clear()

    member x.Invoke (v : 'a) =
        match cache.TryGetValue v with
            | (true, (r, ref)) -> 
                ref := !ref + 1
                r
            | _ ->
                let r = Aardvark.Base.Ag.useScope scope (fun () -> f v)
                cache.[v] <- (r, ref 1)
                r

    member x.Revoke (v : 'a) =
        match cache.TryGetValue v with
            | (true, (r, ref)) -> 
                ref := !ref - 1
                if !ref = 0 then
                    cache.Remove v |> ignore
                r
            | _ -> failwith ""