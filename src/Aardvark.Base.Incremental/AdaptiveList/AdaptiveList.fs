namespace Aardvark.Base.Incremental

open System
open Aardvark.Base
open Aardvark.Base.Incremental.AListReaders


module AList =
    type AdaptiveList<'a>(newReader : unit -> IListReader<'a>) =
        let state = TimeList<'a>()
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
            let delta = r.GetDelta ()
            if not <| List.isEmpty delta then
                delta |> apply state |> ignore
                readers  |> Seq.iter (fun ri ->
                    if ri.IsIncremental then
                        ri.Emit delta
                    else ri.Reset state
                )

        interface alist<'a> with
            member x.GetReader () =
                bringUpToDate()
                let r = getReader()

                let remove ri =
                    r.RemoveOutput ri
                    readers.Remove ri |> ignore

                    if readers.IsEmpty then
                        r.Dispose()
                        inputReader <- None

                let reader = new BufferedReader<'a>(r.RootTime, bringUpToDate, remove)
                reader.Emit (state |> Seq.map Add |> Seq.toList)
                r.AddOutput reader
                readers.Add reader |> ignore

                reader :> _

    type ConstantList<'a>(rootTime : Time, content : seq<Time * 'a>) =
        let content = List.ofSeq content
        interface alist<'a> with
            member x.GetReader () =
                let r = new BufferedReader<'a>(rootTime)
                r.Emit(content |> List.map Add)
                r :> IListReader<_>

    let private emptyTime = Time.newRoot()
    type private EmptyListImpl<'a> private() =
        static let emptySet = ConstantList(emptyTime, []) :> alist<'a>
        static member Instance = emptySet

    let empty<'a> : alist<'a> =
        EmptyListImpl<'a>.Instance

    let single (v : 'a) =
        let r = Time.newRoot()
        ConstantList(r, [Time.after r, v]) :> alist<_>

    let ofSeq (s : seq<'a>) =
        let r = Time.newRoot()
        let current = ref r
        let elements =
            s |> Seq.toList
              |> List.map (fun e ->
                    let t = Time.after !current
                    current := t
                    t,e
                 )

        ConstantList(r, elements) :> alist<_>

    let ofList (l : list<'a>) =
        ofSeq l

    let ofArray (a : 'a[]) =
        ofSeq a

    let toSeq (set : alist<'a>) =
        use r = set.GetReader()
        r.GetDelta() |> ignore
        r.Content.Values

    let toList (set : alist<'a>) =
        set |> toSeq |> Seq.toList

    let toArray (set : alist<'a>) =
        set |> toSeq |> Seq.toArray

    let ofMod (m : IMod<'a>) =
        AdaptiveList(fun () -> ofMod m) :> alist<_>

    let toMod (s : alist<'a>) =
        let r = s.GetReader()
        let m = Mod.custom (fun () ->
            r.GetDelta() |> ignore
            r.Content
        )
        r.AddOutput m
        m

    let map (f : 'a -> 'b) (set : alist<'a>) = 
        let scope = Ag.getContext()
        AdaptiveList(fun () -> set.GetReader() |> map scope f) :> alist<'b>

    let collect (f : 'a -> alist<'b>) (set : alist<'a>) = 
        let scope = Ag.getContext()
        AdaptiveList(fun () -> set.GetReader() |> collect scope (fun v -> (f v).GetReader())) :> alist<'b>

    let choose (f : 'a -> Option<'b>) (set : alist<'a>) =
        let scope = Ag.getContext()
        AdaptiveList(fun () -> set.GetReader() |> choose scope f) :> alist<'b>

    let filter (f : 'a -> bool) (set : alist<'a>) =
        choose (fun v -> if f v then Some v else None) set

    let concat (set : alist<alist<'a>>) =
        collect id set

    let concat' (set : seq<alist<'a>>) =
        concat (ofSeq set)

    let collect' (f : 'a -> alist<'b>) (set : seq<'a>) =
        set |> Seq.map f |> concat'
    
    let bind (f : 'a -> alist<'b>) (m : IMod<'a>) =
        let scope = Ag.getContext()
        AdaptiveList(fun () -> m |> bind scope (fun a -> (f a).GetReader())) :> alist<'b>

    let bind2 (f : 'a -> 'b -> alist<'c>) (ma : IMod<'a>) (mb : IMod<'b>) =
        let scope = Ag.getContext()
        AdaptiveList(fun () -> bind2 scope (fun a b -> (f a b).GetReader()) ma mb) :> alist<'c>


    let filterM (f : 'a -> IMod<bool>) (s : alist<'a>) =
        s |> collect (fun v ->
            v |> f |> bind (fun b -> if b then single v else empty)
        )

    let registerCallback (f : list<Delta<Time * 'a>> -> unit) (set : alist<'a>) =
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
        
    
