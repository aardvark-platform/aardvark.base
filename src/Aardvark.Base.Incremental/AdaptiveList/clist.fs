namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base

[<StructuredFormatDisplay("{AsString}")>]
[<CompiledName("ChangeableList")>]
type clist<'a>(initial : seq<'a>) =
    let history = History PList.trace
    do 
        let mutable last = Index.zero
        let ops = 
            initial |> Seq.map (fun v ->
                let t = Index.after last
                last <- t
                t, Set v
            )
            |> PDeltaList.ofSeq

        history.Perform ops |> ignore

    member x.Count = 
        lock x (fun () -> history.State.Count)

    member x.Clear() =
        lock x (fun () ->
            if x.Count > 0 then
                let deltas = history.State.Content |> MapExt.map (fun k v -> Remove) |> PDeltaList.ofMap
                history.Perform deltas |> ignore
        )

    member x.Append(v : 'a) =
        lock x (fun () ->
            let t = Index.after history.State.MaxIndex
            history.Perform (PDeltaList.ofList [t, Set v]) |> ignore
            t
        )

    member x.Prepend(v : 'a) =
        lock x (fun () ->
            let t = Index.before history.State.MinIndex
            history.Perform (PDeltaList.ofList [t, Set v]) |> ignore
            t
        )

    member x.Remove(i : Index) =
        lock x (fun () ->
            history.Perform (PDeltaList.ofList [i, Remove])
        )

    member x.RemoveAt(i : int) =
        lock x (fun () ->
            let (id,_) = history.State.Content |> MapExt.item i
            x.Remove id |> ignore
        )

    member x.IndexOf(v : 'a) =
        lock x (fun () ->
            history.State 
                |> Seq.tryFindIndex (Unchecked.equals v) 
                |> Option.defaultValue -1
        )

    member x.Remove(v : 'a) =
        lock x (fun () ->
            match x.IndexOf v with
                | -1 -> false
                | i -> x.RemoveAt i; true
        )

    member x.Contains(v : 'a) =
        lock x (fun () -> history.State) |> Seq.exists (Unchecked.equals v)

    member x.Insert(i : int, value : 'a) =
        lock x (fun () ->
            if i < 0 || i > history.State.Content.Count then
                raise <| IndexOutOfRangeException()

            let l, s, r = MapExt.neighboursAt i history.State.Content
            let r = 
                match s with
                    | Some s -> Some s
                    | None -> r
            let index = 
                match l, r with
                    | Some (before,_), Some (after,_) -> Index.between before after
                    | None,            Some (after,_) -> Index.before after
                    | Some (before,_), None           -> Index.after before
                    | None,            None           -> Index.after Index.zero
            history.Perform (PDeltaList.ofList [index, Set value]) |> ignore
            
        )
        
    member x.CopyTo(arr : 'a[], i : int) = 
        let state = lock x (fun () -> history.State )
        state.CopyTo(arr, i)

    member x.Item
        with get (i : int) = 
            let state = lock x (fun () -> history.State)
            state.[i]

        and set (i : int) (v : 'a) =
            lock x (fun () ->
                let k = history.State.Content |> MapExt.tryItem i
                match k with
                    | Some (id,_) -> history.Perform(PDeltaList.ofList [id, Set v]) |> ignore
                    | None -> ()
            )

    member x.Item
        with get (i : Index) =
            let state = lock x (fun () -> history.State)
            state.[i]

        and set (i : Index) (v : 'a) =
            lock x (fun () ->
                history.Perform(PDeltaList.ofList [i, Set v]) |> ignore
            )

    member x.AppendMany(elements : seq<'a>) =
        lock x (fun () ->
            let mutable deltas = PDeltaList.empty
            let mutable l = history.State.MaxIndex
            for e in elements do
                let t = Index.after l
                deltas <- PDeltaList.add t (Set e) deltas
                l <- t

            history.Perform deltas |> ignore
        )

    member x.PrependMany(elements : seq<'a>) =
        lock x (fun () ->
            let mutable deltas = PDeltaList.empty
            let mutable l = Index.before history.State.MinIndex
            for e in elements do
                let t = Index.between l history.State.MinIndex
                deltas <- PDeltaList.add t (Set e) deltas
                l <- t

            history.Perform deltas |> ignore
        )

    override x.ToString() =
        let suffix =
            if x.Count > 5 then "; ..."
            else ""

        let content =
            history.State |> Seq.truncate 5 |> Seq.map (sprintf "%A") |> String.concat "; "

        "clist [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    new() = clist(Seq.empty)

    interface alist<'a> with
        member x.IsConstant = false
        member x.Content = history :> IMod<_>
        member x.GetReader() = history.NewReader()

    interface ICollection<'a> with 
        member x.Add(v) = x.Append v |> ignore
        member x.Clear() = x.Clear()
        member x.Remove(v) = x.Remove v
        member x.Contains(v) = x.Contains v
        member x.CopyTo(arr,i) = x.CopyTo(arr, i)
        member x.IsReadOnly = false
        member x.Count = x.Count

    interface IList<'a> with
        member x.RemoveAt(i) = x.RemoveAt i
        member x.IndexOf(item : 'a) = x.IndexOf item
        member x.Item
            with get(i : int) = x.[i]
            and set (i : int) (v : 'a) = x.[i] <- v
        member x.Insert(i,v) = x.Insert(i,v) |> ignore

    interface IEnumerable with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator() :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator() :> _


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module CList =
    
    let empty<'a> = clist<'a>()

    let clear (l : clist<'a>) = l.Clear()
    let append (v : 'a) (l : clist<'a>) = l.Append v
    let prepend (v : 'a) (l : clist<'a>) = l.Prepend v
    let remove (i : Index) (l : clist<'a>) = l.Remove i
    let removeAt (index : int) (l : clist<'a>) = l.RemoveAt index
    let indexOf (v : 'a) (l : clist<'a>) = l.IndexOf v
    let removeElement (v : 'a) (l : clist<'a>) = l.Remove v
    let contains (v : 'a) (l : clist<'a>) = l.Contains v
    let insert (index : int) (v : 'a) (l : clist<'a>) = l.Insert(index,v)
    
    let getIndex (index : int) (l : clist<'a>) = l.Item index
    let get      (i : Index) (l : clist<'a>) = l.Item i

    let setIndex (index : int) (v : 'a) (l : clist<'a>) = l.[index] <- v
    let set      (i : Index)   (v : 'a) (l : clist<'a>) = l.[i] <- v

    let ofSeq (l : seq<'a>) : clist<'a> = clist(l)