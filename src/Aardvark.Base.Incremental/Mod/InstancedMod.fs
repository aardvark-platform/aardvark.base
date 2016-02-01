namespace Aardvark.Base.Incremental

open System
open System.Runtime.CompilerServices
open System.Collections.Generic
open System.Collections.Concurrent
open Aardvark.Base

type ModContext = Map<Symbol,IMod>

type InstancedMod<'a> =
    abstract Instantiate : ModContext -> IMod<'a>


type Hole<'a>(name : Symbol) =
    member x.Instantiate ctx =
        match Map.tryFind name ctx with 
         | Some v -> v |> unbox<IMod<'a>>
         | _ -> failwith ""

    interface InstancedMod<'a> with
        member x.Instantiate ctx = x.Instantiate ctx

type LazyInstancedMod<'a>(f : ModContext -> IMod<'a>) =
    member x.Instantiate ctx =
        f ctx
    interface InstancedMod<'a> with
        member x.Instantiate ctx = x.Instantiate ctx
    
module InstancedMod =

    let instantiate ctx (m : InstancedMod<'a>) = m.Instantiate ctx

    let map (f : 'a -> 'b) (x : InstancedMod<'a>) =
        LazyInstancedMod(fun ctx -> x.Instantiate ctx |> Mod.map f) :> InstancedMod<_>

    let bind (f : 'a -> InstancedMod<'b>) (x : InstancedMod<'a>) =
        LazyInstancedMod(fun ctx -> 
            let m = x.Instantiate ctx
            m |> Mod.bind (fun a -> f a |> instantiate ctx)
        ) :> InstancedMod<_>

    let map2 (f : 'a -> 'b -> 'c) (a : InstancedMod<'a>) (b: InstancedMod<'b>) =
        LazyInstancedMod(fun ctx -> Mod.map2 f (a.Instantiate ctx) (b.Instantiate ctx)) :> InstancedMod<_>

    let custom (f : ModContext -> IMod<'a> -> 'a) :  InstancedMod<'a> =
        LazyInstancedMod (fun ctx -> Mod.custom (f ctx)) :> InstancedMod<_>

    let ofMod (m : IMod<'a>) : InstancedMod<'a> =
        LazyInstancedMod (fun ctx -> m) :> InstancedMod<_>