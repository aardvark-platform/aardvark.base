namespace Aardvark.Base.Incremental

open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Aardvark.Base

type hdeltamap<'k, 'v> = hmap<'k, ElementOperation<'v>>

module HDeltaMap =
    open Aardvark.Base.MultimethodTest

    let combine (l : hdeltamap<'k, 'v>) (r : hdeltamap<'k, 'v>) : hdeltamap<'k, 'v> =
        // O(1)
        if l.Store == r.Store then
            l
        // O(1)
        elif l.Count = 0 then r
        
        // O(1)
        elif r.Count = 0 then l

        // O(N * log M)
        elif l.Count * 5 < r.Count then
            let mutable res = r
            for (k,v) in l do
                res <- HMap.alter k (function None -> Some v | Some r -> Some r) res
            res
            
        // O(M * log N)
        elif r.Count * 5 < l.Count then
            let mutable res = l
            for (k,v) in r do
                res <- HMap.add k v res
            res

        // O(M + N)
        else 
            let merge (key : 'k) (l : ElementOperation<'v>) (r : ElementOperation<'v>) =
                r
            HMap.unionWith merge l r

    let empty : hdeltamap<'k, 'v> = HMap.empty

    let monoid<'k, 'v> : Monoid<hdeltamap<'k, 'v>> =
        {
            mempty = empty
            misEmpty = HMap.isEmpty
            mappend = combine
        }

module HMap =

    let computeDelta (l : hmap<'k, 'v>) (r : hmap<'k, 'v>) : hdeltamap<'k, 'v> =
        if l.Store == r.Store then
            HMap.empty
        elif l.Count = 0 && r.Count = 0 then
            HMap.empty
        elif l.Count = 0 then
            r |> HMap.map (fun _ v -> Set v)
        elif r.Count = 0 then
            l |> HMap.map (fun _ _ -> Remove)
        else
            // TODO: one small???
            let merge (key : 'k) (l : Option<'v>) (r : Option<'v>) =
                match l, r with
                    | None, None -> None
                    | Some l, None -> Some Remove
                    | None, Some r -> Some (Set r)
                    | Some l, Some r ->
                        if Unchecked.equals l r then None
                        else Some (Set r)
            HMap.choose2 merge l r

    let applyDelta (m : hmap<'k, 'v>) (delta : hdeltamap<'k, 'v>) =
        if delta.Count = 0 then
            m, delta
        elif m.Count = 0 then
            delta.ChooseTup(fun _ op ->
                match op with
                | Set v -> Some (v, Set v)
                | _ -> None
            )
        else
            let mutable effective = HMap.empty
            let mutable m = m
            for (k,v) in delta do
                m <- m.Alter(k, fun o ->
                    match o, v with
                        | Some o, Remove ->
                            effective <- HMap.add k Remove effective
                            None
                        | None, Remove ->
                            None

                        | None, Set n ->
                            effective <- HMap.add k (Set n) effective
                            Some n

                        | Some o, Set n ->
                            if not (Unchecked.equals o n) then
                                effective <- HMap.add k (Set n) effective

                            Some n
                )

            m, effective

    let trace<'k, 'v> : Traceable<hmap<'k, 'v>, hdeltamap<'k, 'v>> =
        {
            tempty = HMap.empty
            tops = HDeltaMap.monoid
            tapply = applyDelta
            tcompute = computeDelta
            tcollapse = fun _ _ -> false
        }


type IMapReader<'k, 'v> = IOpReader<hmap<'k, 'v>, hdeltamap<'k, 'v>>

[<CompiledName("IAdaptiveMap")>]
type amap<'k, 'v> =
    abstract member IsConstant : bool
    abstract member GetReader : unit -> IMapReader<'k, 'v>
    abstract member Content : IMod<hmap<'k, 'v>>