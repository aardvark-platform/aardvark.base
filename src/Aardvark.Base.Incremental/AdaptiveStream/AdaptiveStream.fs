namespace Aardvark.Base.Incremental.Experimental

open System
open Aardvark.Base
open Aardvark.Base.Incremental

module AStream =
    open AStreamReaders

    type private ConstantStream<'a>(values : list<'a>) =
        interface astream<'a> with
            member x.GetReader() =
                let t = DateTime.Now
                new OneShotReader<'a>(History (values |> List.map (fun v -> (t,v)))) :> IStreamReader<_>

    type private AdaptiveStream<'a>(newReader : unit -> IStreamReader<'a>) =
        let l = obj()
        let readers = WeakSet<CopyReader<'a>>()

        let mutable inputReader = None
        let getReader() =
            match inputReader with
                | Some r -> r
                | None ->
                    let r = newReader()
                    inputReader <- Some r
                    r

        member x.GetReader() =
            lock l (fun () ->
                let r = getReader()

                let remove ri =
                    r.RemoveOutput ri
                    readers.Remove ri |> ignore

                    if readers.IsEmpty then
                        r.Dispose()
                        inputReader <- None

                let reader = new CopyReader<'a>(r, remove)
                readers.Add reader |> ignore

                reader :> IStreamReader<_>
            )

        interface astream<'a> with
            member x.GetReader() = x.GetReader()

    type private EmptyStreamImpl<'a>() =
        static let emptyStream = ConstantStream [] :> astream<'a>

        static member Instance = emptyStream


    let empty<'a> : astream<'a> = EmptyStreamImpl.Instance

    let single (v : 'a) = ConstantStream [v] :> astream<_>

    let ofSeq (v : seq<'a>) = ConstantStream (Seq.toList v) :> astream<_>

    let ofList (v : list<'a>) = ConstantStream v :> astream<_>

    let ofArray (v : 'a[]) = ConstantStream (Array.toList v) :> astream<_>

    let ofMod (m : IMod<'a>) =
        if m.IsConstant then
            ConstantStream [m.GetValue(null)] :> astream<_>
        else
            AdaptiveStream(fun () -> ofMod m) :> astream<_>

    let map (f : 'a -> 'b) (input : astream<'a>) = 
        let scope = Ag.getContext()
        AdaptiveStream(fun () -> input.GetReader() |> map scope f) :> astream<'b>

    let collect (f : 'a -> astream<'b>) (input : aset<'a>) = 
        let scope = Ag.getContext()
        AdaptiveStream(fun () -> input.GetReader() |> collect scope (fun v -> (f v).GetReader())) :> astream<'b>

    let bind (f : 'a -> astream<'b>) (input : IMod<'a>) =
        let scope = Ag.getContext()
        AdaptiveStream(fun () -> bind scope (fun v -> (f v).GetReader()) input) :> astream<_>
