namespace Aardvark.Base.Incremental

open System
open Aardvark.Base
open Aardvark.Base.Incremental.ASetReaders


module ASet =
    type AdaptiveSet<'a>(newReader : unit -> IReader<'a>) =
        let state = ReferenceCountingSet<'a>()
        let readers = WeakSet<BufferedReader<'a>>()

        let mutable inputReader = None
        let getReader() =
            match inputReader with
                | Some r -> r
                | None ->
                    let r = newReader()
                    inputReader <- Some r
                    r

        let bringUpToDate () =
            let r = getReader()
            if r.OutOfDate then
                let delta = r.GetDelta ()
                delta |> apply state |> ignore
                readers  |> Seq.iter (fun ri ->
                    if ri.IsIncremental then
                        ri.Emit delta
                    else ri.Reset state
                )

        interface aset<'a> with
            member x.GetReader () =
                bringUpToDate()
                let r = getReader()

                let remove ri =
                    r.RemoveOutput ri
                    readers.Remove ri |> ignore

                    if readers.IsEmpty then
                        r.Dispose()
                        inputReader <- None

                let reader = new BufferedReader<'a>(bringUpToDate, remove)
                reader.Emit (state |> Seq.map Add |> Seq.toList)
                r.AddOutput reader
                readers.Add reader |> ignore

                reader :> _

    type ConstantSet<'a>(content : seq<'a>) =
        let content = List.ofSeq content

        interface aset<'a> with
            member x.GetReader () =
                let r = new BufferedReader<'a>()
                r.Emit(content |> List.map Add)
                r :> IReader<_>

    let empty<'a> : aset<'a> =
        ConstantSet [] :> aset<_>

    let single (v : 'a) =
        ConstantSet [v] :> aset<_>

    let ofSeq (s : seq<'a>) =
        ConstantSet(s) :> aset<_>

    let ofList (l : list<'a>) =
        ConstantSet(l) :> aset<_>

    let ofArray (a : 'a[]) =
        ConstantSet(a) :> aset<_>

    let toList (set : aset<'a>) =
        use r = set.GetReader()
        r.GetDelta() |> ignore
        r.Content |> Seq.toList

    let toSeq (set : aset<'a>) =
        let l = toList set
        l :> seq<_>

    let toArray (set : aset<'a>) =
        use r = set.GetReader()
        r.GetDelta() |> ignore
        r.Content |> Seq.toArray

    let ofMod (m : IMod<'a>) =
        AdaptiveSet(fun () -> ofMod m) :> aset<_>

    let map (f : 'a -> 'b) (set : aset<'a>) = 
        AdaptiveSet(fun () -> set.GetReader() |> map f) :> aset<'b>

    let bind (f : 'a -> aset<'b>) (m : IMod<'a>) =
        AdaptiveSet(fun () -> m |> bind (fun v -> (f v).GetReader())) :> aset<'b>

    let collect (f : 'a -> aset<'b>) (set : aset<'a>) = 
        AdaptiveSet(fun () -> set.GetReader() |> collect (fun v -> (f v).GetReader())) :> aset<'b>

    let choose (f : 'a -> Option<'b>) (set : aset<'a>) =
        AdaptiveSet(fun () -> set.GetReader() |> choose f) :> aset<'b>

    let filter (f : 'a -> bool) (set : aset<'a>) =
        choose (fun v -> if f v then Some v else None) set

    let concat (set : aset<aset<'a>>) =
        collect id set

    let concat' (set : seq<aset<'a>>) =
        concat (ConstantSet set)

    let collect' (f : 'a -> aset<'b>) (set : seq<'a>) =
        set |> Seq.map f |> concat'
    



    let registerCallback (f : list<Delta<'a>> -> unit) (set : aset<'a>) =
        let reader = set.GetReader()
        let self = ref id
        self := fun () ->
            reader.GetDelta() |> f
            reader.MarkingCallbacks.Add !self |> ignore

        !self ()

        { new IDisposable with
            member x.Dispose() = 
                reader.Dispose()
                reader.MarkingCallbacks.Remove !self |> ignore
                
        }    
        
    
