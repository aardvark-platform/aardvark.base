namespace Aardvark.Base.Incremental

open Aardvark.Base
open System.Collections
open System.Collections.Generic
open System.Threading

type IVersioned =
    abstract member Version : int

type IVersionedSet<'a> =
    inherit ISet<'a>
    inherit IVersioned


type VersionedSet<'a>(s : ISet<'a>) =
    let mutable version = 0
    let hasChanged() = Interlocked.Increment &version |> ignore
        
    let trackCount (f : unit -> 'x) =
        let old = s.Count
        let res = f()
        if s.Count <> old then
            hasChanged()
        res

    member x.Remove v = if s.Remove v then hasChanged(); true else false
    member x.Clear() = if s.Count <> 0 then hasChanged(); s.Clear()
    member x.Count = s.Count
    member x.CopyTo(arr, index) = s.CopyTo(arr, index)
    member x.IsReadOnly = s.IsReadOnly
    member x.Contains v = s.Contains v
    member x.Add item = if s.Add item then hasChanged(); true else false
    member x.ExceptWith other = trackCount (fun () -> s.ExceptWith other)
    member x.IntersectWith other = trackCount (fun () -> s.IntersectWith other)
    member x.UnionWith other = trackCount (fun () -> s.UnionWith other)
    member x.SymmetricExceptWith other = trackCount (fun () -> s.SymmetricExceptWith other)
    member x.IsProperSubsetOf other = s.IsProperSubsetOf other
    member x.IsProperSupersetOf other = s.IsProperSupersetOf other
    member x.IsSubsetOf other = s.IsSubsetOf other
    member x.IsSupersetOf other = s.IsSupersetOf other
    member x.Overlaps other = s.Overlaps other
    member x.SetEquals other = s.SetEquals other

    member x.GetEnumerator() = s.GetEnumerator()

    interface IVersionedSet<'a> with
        member x.Version = version

    interface ICollection<'a> with
        member x.Add v = x.Add v |> ignore
        member x.Remove v = x.Remove v
        member x.Clear() = x.Clear()
        member x.Count = x.Count
        member x.CopyTo(arr, index) = x.CopyTo(arr, index)

        member x.IsReadOnly = x.IsReadOnly
        member x.Contains v = x.Contains v

    interface ISet<'a> with
        member x.Add item = x.Add item
        member x.ExceptWith other = x.ExceptWith other
        member x.IntersectWith other = x.IntersectWith other
        member x.UnionWith other = x.UnionWith other
        member x.SymmetricExceptWith other = x.SymmetricExceptWith other

        member x.IsProperSubsetOf other = x.IsProperSubsetOf other
        member x.IsProperSupersetOf other = x.IsProperSupersetOf other
        member x.IsSubsetOf other = x.IsSubsetOf other
        member x.IsSupersetOf other = x.IsSupersetOf other
        member x.Overlaps other = x.Overlaps other
        member x.SetEquals other = x.SetEquals other


    interface IEnumerable with
        member x.GetEnumerator() = s.GetEnumerator() :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() = x.GetEnumerator()

module ChangeTracker =
    type private ChangeTracker<'a>() =
        static let getVersion (a : 'a) =
            (a |> unbox<IVersioned>).Version

        static let createSimpleTracker : (Option<'a -> 'a -> bool>) -> 'a -> bool =
            fun eq ->
                let eq = defaultArg eq (fun a b -> System.Object.Equals(a,b))
                let old = ref None
                fun n ->
                    match !old with
                        | None -> 
                            old := Some n
                            true
                        | Some o ->
                            if eq o n then 
                                false
                            else 
                                old := Some n
                                true

        static let createTracker : (Option<'a -> 'a -> bool>) -> 'a -> bool =
            
            if typeof<IVersioned>.IsAssignableFrom typeof<'a> then
                fun eq ->
                    let eq = defaultArg eq (fun a b -> System.Object.Equals(a,b))
                    let old = ref None

                    fun n ->
                        let v = getVersion n
                        match !old with
                            | None -> 
                                old := Some (n, v)
                                true
                            | Some (o,ov) ->
                                if ov = v && eq o n then
                                    false
                                else
                                    old := Some (n, v)
                                    true

            else createSimpleTracker

        static let createVersionOnlyTracker : unit -> 'a -> bool =
            
            if typeof<IVersioned>.IsAssignableFrom typeof<'a> then
                fun () ->
                    let old = ref None

                    fun n ->
                        let v = getVersion n
                        match !old with
                            | None -> 
                                old := Some v
                                true
                            | Some ov ->
                                if ov = v then
                                    false
                                else
                                    old := Some v
                                    true
            else
                fun () n -> false


              

        static member CreateTracker eq = createTracker eq
        static member CreateDefaultTracker() = createTracker None
        static member CreateCustomTracker eq = createTracker (Some eq)
        static member CreateVersionOnlyTracker() = createVersionOnlyTracker ()

    let track<'a> : 'a -> bool =
        ChangeTracker<'a>.CreateDefaultTracker()

    let trackCustom<'a> (eq : Option<'a -> 'a -> bool>) : 'a -> bool =
        ChangeTracker<'a>.CreateTracker eq

    let trackVersion<'a> : 'a -> bool =
        ChangeTracker<'a>.CreateVersionOnlyTracker()