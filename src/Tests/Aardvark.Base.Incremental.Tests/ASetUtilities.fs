namespace Aardvark.Base.Incremental.Tests


open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental


type aset_check<'a> =
    abstract member real : Lazy<aset<'a>>
    abstract member sim : Lazy<ASetReferenceImpl.aset<'a>>

type cset_check<'a>(initial : seq<'a>) =
    let real = cset<'a>(initial)
    let sim = ASetReferenceImpl.cset<'a>(initial)

    interface aset_check<'a> with
        member x.real = lazy ( real :> aset<_> )
        member x.sim = lazy( sim :> ASetReferenceImpl.aset<_> )

    member x.Add v =
        let r = real.Add v
        let s = sim.Add v
        if r = s then s
        else failwith "inconsistent add-result (is %A but should be %A)" r s

    member x.Remove v =
        let r = real.Remove v
        let s = sim.Remove v
        if r = s then s
        else failwith "inconsistent remove-result (is %A but should be %A)" r s

    member x.Clear() =
        sim.Clear()
        real.Clear()

    new() = cset_check Seq.empty

type aset_check_reader<'a> = { realReader : IReader<'a>; simReader : ASetReferenceImpl.IReader<'a> } with
    member x.GetDelta() =
        let r = HashSet(x.realReader.GetDelta())
        let s = HashSet(x.simReader.GetDelta())

        if not <| x.realReader.Content.SetEquals x.simReader.Content then
            failwith "inconsistent set content (is %A but should be %A)" x.realReader.Content x.simReader.Content

        if not <| r.SetEquals s then
            failwith "inconsistent set delta (is %A but should be %A)" r s

        r |> Seq.toList

    member x.Content =
        if not <| x.realReader.Content.SetEquals x.simReader.Content then
            failwith "inconsistent set content (is %A but should be %A)" x.realReader.Content x.simReader.Content

        x.realReader.Content

[<AutoOpen>]
module CheckExtensions =
    type aset_check<'a> with
        member x.GetReader() =
            { realReader = x.real.Value.GetReader(); simReader = x.sim.Value.GetReader() }


module CSetCheck =
    let empty<'a> = cset_check<'a>()

    let ofSeq (s : seq<'a>) = cset_check s
    let ofList (l : list<'a>) = cset_check l

module ASetCheck =
    type aset_check_impl<'a> = { real : Lazy<aset<'a>>; sim : Lazy<ASetReferenceImpl.aset<'a>> } with
        interface aset_check<'a> with
            member x.real = x.real
            member x.sim = x.sim

    let map (f : 'a -> 'b) (s : aset_check<'a>) =
        { real = lazy( ASet.map f (s.real.Value) )
          sim = lazy( ASetReferenceImpl.map f (s.sim.Value) )
        } :> aset_check<_>

    let collect (f : 'a -> aset_check<'b>) (s : aset_check<'a>) =
        { real = lazy( ASet.collect (fun a -> (f a).real.Value) (s.real.Value) )
          sim = lazy( ASetReferenceImpl.collect (fun a -> (f a).sim.Value) (s.sim.Value) )
        } :> aset_check<_>

[<AutoOpen>]
module FsUnitExtensions =
    open NUnit.Framework

    type SetEqualityConstraint<'a>(ref : seq<'a>) =
        inherit Constraints.EqualConstraint(ref)

        override x.Matches (o : obj) =
            base.Matches(o) |> ignore
            match o with
                | :? seq<'a> as o ->
                    let s = HashSet(o)
                    s.SetEquals ref
                | _ ->
                    false

    type DeltaListEqual<'a>(ref : list<list<'a>>) =
        inherit Constraints.EqualConstraint(ref)

        override x.Matches (o : obj) =
            base.Matches(o) |> ignore
            match o with
                | :? list<list<'a>> as o ->
                    if List.length ref = List.length o then
                        let zip = List.zip ref o

                        zip |> List.forall (fun (r,o) -> 
                            let s = HashSet(o)
                            s.SetEquals r
                        )
                    else
                        false

                | _ ->
                    false


    let setEqual<'a> a = SetEqualityConstraint<'a> a
    let deltaListEqual<'a> a = DeltaListEqual<'a> a


