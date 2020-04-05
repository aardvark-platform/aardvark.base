namespace FSharp.Data.Adaptive

open Aardvark.Base
open Aardvark.Base.Geometry
open FSharp.Data.Adaptive

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module BvhTree =

    type private ASetBvh<'a>(objects : aset<'a>, getBounds : 'a -> aval<Box3d>) =
        inherit AVal.AbstractVal<BvhTree<'a>>()
    
        let reader = objects.GetReader()
        let boundsCache = Dict()

        override x.Compute(token) =
            let ops = reader.GetChanges token

            for op in HashSetDelta.toSeq ops do
                match op with
                | Add(_,key) ->
                    boundsCache.GetOrCreate(key, fun key -> getBounds key) |> ignore

                | Rem(_,key) ->
                    match boundsCache.TryRemove key with
                        | (true, bounds) ->
                            bounds.Outputs.Remove x |> ignore
                        | _ ->
                            failwith "[Bvh] unexpected removal"

            let data = boundsCache |> Dict.toArray |> Array.map (fun (k,v) -> k, v.GetValue(token))
            BvhTree(data)

    type private AMapBvh<'a>(objects : amap<'a, aval<Box3d>>) =
        inherit AVal.AbstractVal<BvhTree<'a>>()
    
        let reader = objects.GetReader()
        let boundsCache = Dict<'a, aval<Box3d>>()

        override x.Compute(token) =
            let ops = reader.GetChanges token
            for key, op in HashMapDelta.toSeq ops do
                match op with
                    | Set bounds ->
                        match boundsCache.TryRemove key with
                            | (true, old) -> old.Outputs.Remove x |> ignore
                            | _ -> ()

                        boundsCache.[key] <- bounds

                    | Remove ->
                        match boundsCache.TryRemove key with
                            | (true, bounds) ->
                                bounds.Outputs.Remove x |> ignore
                            | _ ->
                                failwith "[Bvh] unexpected removal"

            let data = boundsCache |> Dict.toArray |> Array.map (fun (k,v) -> k, v.GetValue(token))

            BvhTree(data)


    let ofASet (getBounds : 'a -> aval<Box3d>) (objects : aset<'a>) =
        new ASetBvh<'a>(objects, getBounds) :> aval<_>
   
    let ofAMap (objects : amap<'a, aval<Box3d>>) =
        new AMapBvh<'a>(objects) :> aval<_>





