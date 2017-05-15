namespace Aardvark.Base.Incremental

open System
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Aardvark.Base

type hdeltamap<'k, 'v> = hmap<'k, ElementOperation<'v>>

module HDeltaMap =
    let combine (l : hdeltamap<'k, 'v>) (r : hdeltamap<'k, 'v>) : hdeltamap<'k, 'v> =
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
        let mutable effective = HMap.empty
        let apply (key : 'k) (l : Option<'v>) (r : Option<ElementOperation<'v>>) =
            match r with
                | None -> 
                    l
                | Some op ->
                    match l, op with
                        | Some _, Remove ->
                            effective <- HMap.add key Remove effective
                            None
                        | None, Remove ->
                            None

                        | None, Set n ->
                            effective <- HMap.add key (Set n) effective
                            Some n
                            
                        | Some o, Set n ->
                            if not (Unchecked.equals o n) then
                                effective <- HMap.add key (Set n) effective

                            Some n

        let res = HMap.choose2 apply m delta
        res, effective

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