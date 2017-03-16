namespace Aardvark.Base.Incremental

open Aardvark.Base


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Bvh =

    type private ASetBvh<'a>(objects : aset<'a>, getBounds : 'a -> IMod<Box3d>) =
        inherit Mod.AbstractMod<Bvh<'a>>()
    
        let mutable isDisposed = false
        let reader = objects.GetReader()
        let boundsCache = Dict()

        override x.Compute(token) =
            if isDisposed then raise <| System.ObjectDisposedException("ASetBvh")

            let ops = reader.GetOperations token

            for op in HDeltaSet.toSeq ops do
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

            Bvh.ofArray data

        member x.Dispose() =
            if not isDisposed then
                isDisposed <- true
                reader.Dispose()
                for m in boundsCache.Values do
                    m.Outputs.Remove x |> ignore
                boundsCache.Clear()

        interface IDisposableMod<Bvh<'a>> with
            member x.Dispose() = x.Dispose()
  
    type private AMapBvh<'a>(objects : amap<'a, IMod<Box3d>>) =
        inherit Mod.AbstractMod<Bvh<'a>>()
    
        let mutable isDisposed = false
        let reader = objects.GetReader()
        let boundsCache = Dict<'a, IMod<Box3d>>()

        override x.Compute(token) =
            if isDisposed then raise <| System.ObjectDisposedException("AMapBvh")
            let ops = reader.GetOperations token

            for key, op in HMap.toSeq ops do
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

            Bvh.ofArray data

        member x.Dispose() =
            if not isDisposed then
                isDisposed <- true
                reader.Dispose()
                for m in boundsCache.Values do
                    m.Outputs.Remove x |> ignore
                boundsCache.Clear()

        interface IDisposableMod<Bvh<'a>> with
            member x.Dispose() = x.Dispose()

    let ofASet (getBounds : 'a -> IMod<Box3d>) (objects : aset<'a>) =
        new ASetBvh<'a>(objects, getBounds) :> IDisposableMod<_>
   
    let ofAMap (objects : amap<'a, IMod<Box3d>>) =
        new AMapBvh<'a>(objects) :> IDisposableMod<_>





