namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base

[<CompiledName("ChangeableOrderedSet")>]
type corderedset<'a>(initial : seq<'a>) =
    let setHistory = History HRefSet.traceNoRefCount
    let history = History PList.trace
    let indices = Dict<'a, Index>()

    let addRange(values : seq<'a>) =
        let mutable last = history.State.MaxIndex
        let values =
            values 
                |> Seq.toList
                |> List.choose (fun v ->
                    match indices.TryGetValue v with
                        | (true, i) -> None
                        | _ ->
                            let i = Index.after last
                            last <- i
                            indices.Add(v, i)
                            Some (i, v)
                )

        match values with
            | [] -> 
                ()
            | l ->
                history.Perform (values |> Seq.map (fun (i,v) -> i, Set v) |> PDeltaList.ofSeq) |> ignore
                setHistory.Perform (values |> Seq.map (snd >> Add) |> HDeltaSet.ofSeq ) |> ignore
        

    do addRange initial

    member x.Count = setHistory.State.Count

    member x.Add(value : 'a) =
        lock x (fun () ->
            match indices.TryGetValue value with
                | (true, i) -> 
                    false

                | _ -> 
                    let i = Index.after history.State.MaxIndex
                    indices.[value] <- i
                    history.Perform (PDeltaList.single i (Set value)) |> ignore
                    setHistory.Perform (HDeltaSet.single (Add value))
        )

    member x.AddRange(values : seq<'a>) =
        lock x (fun () -> addRange values)

    member x.Remove(value : 'a) =
        lock x (fun () ->
            match indices.TryRemove value with
                | (true, i) -> 
                    history.Perform (PDeltaList.single i Remove) |> ignore
                    setHistory.Perform (HDeltaSet.single (Rem value))
                    
                | _ ->
                    false
        )

    member x.InsertAfter(anchor : 'a, value : 'a) =
        lock x (fun () ->
            match indices.TryGetValue anchor with
                | (true, ai) ->
                    match indices.TryGetValue value with
                        | (true, vi) ->  
                            false
                        | _ ->
                            // TODO: more efficient ways of doing that
                            let _, _, r = history.State.Content |> MapExt.split ai
                            let index = 
                                match MapExt.tryMin r with
                                    | Some ri -> Index.between ai ri
                                    | None -> Index.after ai
                                    
                            indices.[value] <- index

                            history.Perform (PDeltaList.single index (Set value)) |> ignore
                            setHistory.Perform (HDeltaSet.single (Add value))

                | _ ->
                    raise <| KeyNotFoundException()
        )

    member x.InsertBefore(anchor : 'a, value : 'a) =
        lock x (fun () ->
            match indices.TryGetValue anchor with
                | (true, ai) ->
                    match indices.TryGetValue value with
                        | (true, vi) ->  
                            false
                        | _ ->
                            // TODO: more efficient ways of doing that
                            let l, _, _ = history.State.Content |> MapExt.split ai
                            let index = 
                                match MapExt.tryMax l with
                                    | Some li -> Index.between li ai
                                    | None -> Index.before ai
                                    
                            indices.[value] <- index

                            history.Perform (PDeltaList.single index (Set value)) |> ignore
                            setHistory.Perform (HDeltaSet.single (Add value))

                | _ ->
                    raise <| KeyNotFoundException()
        )

    member x.InsertAfter(anchor : 'a, values : seq<'a>) =
        lock x (fun () ->
            let values = 
                values 
                    |> Seq.filter (fun v -> not (indices.ContainsKey v))
                    |> Seq.toList

            match values with
                | [] -> ()
                | values ->
                    match indices.TryGetValue anchor with
                        | (true, ai) ->
                            // TODO: more efficient ways of doing that
                            let _, _, r = history.State.Content |> MapExt.split ai

                            let index = 
                                let mutable last = ai
                                let ai = ()
                                match MapExt.tryMin r with
                                    | Some ri -> 
                                        fun () -> 
                                            let res = Index.between last ri
                                            last <- res
                                            res

                                    | None -> 
                                        fun () -> 
                                            let res = Index.after last
                                            last <- res
                                            res
                               
                            let deltaList = values |> List.map (fun v -> let i = index() in indices.[v] <- i; i, Set v) |> PDeltaList.ofList
                            let deltaSet = values |> List.map Add |> HDeltaSet.ofList
                            
                            history.Perform deltaList |> ignore
                            setHistory.Perform deltaSet |> ignore

                        | _ ->
                            raise <| KeyNotFoundException()
                )
 
    member x.InsertBefore(anchor : 'a, values : seq<'a>) =
        lock x (fun () ->
            let values = 
                values 
                    |> Seq.filter (fun v -> not (indices.ContainsKey v))
                    |> Seq.toList

            match values with
                | [] -> ()
                | values ->
                    match indices.TryGetValue anchor with
                        | (true, ai) ->
                            // TODO: more efficient ways of doing that
                            let l, _, _ = history.State.Content |> MapExt.split ai
                            let index = 
                                match MapExt.tryMax l with
                                    | Some li -> 
                                        let mutable last = li
                                        fun () -> 
                                            let res = Index.between last ai
                                            last <- res
                                            res

                                    | None -> 
                                        let mutable last = Index.zero
                                        fun () -> 
                                            let res = Index.between last ai
                                            last <- res
                                            res
                               
                            let deltaList = values |> List.map (fun v -> let i = index() in indices.[v] <- i; i, Set v) |> PDeltaList.ofList
                            let deltaSet = values |> List.map Add |> HDeltaSet.ofList
                            
                            history.Perform deltaList |> ignore
                            setHistory.Perform deltaSet |> ignore

                        | _ ->
                            raise <| KeyNotFoundException()
                )

    member x.TryGetNext(value : 'a) =
        lock x (fun () ->
            match indices.TryGetValue(value) with
                | (true, t) ->
                    if history.State.MaxIndex <> t then 
                        let l, s, r = MapExt.neighbours t history.State.Content
                        match r with 
                            | Some (ni,_) -> history.State.TryGet(ni)
                            | _ -> None
                    else
                        None
                | _ -> None
            )

    member x.TryGetPrev(value : 'a) =
        lock x (fun () ->
            match indices.TryGetValue(value) with
                | (true, t) ->
                    if history.State.MinIndex <> t then 
                        let l, s, r = MapExt.neighbours t history.State.Content
                        match l with 
                            | Some (pi,_) -> history.State.TryGet(pi)
                            | _ -> None
                    else
                        None
                | _ -> None
            )
       
    member x.Clear() =
        lock x (fun () ->
            indices.Clear()
            history.Perform (plist.ComputeDeltas(history.State, plist.Empty)) |> ignore
            setHistory.Perform (HRefSet.computeDelta setHistory.State HRefSet.empty) |> ignore
        )

    member x.Contains(item : 'a) =
        lock x (fun () ->
            indices.ContainsKey(item)
            )

    new() = corderedset(Seq.empty)

    interface aset<'a> with
        member x.IsConstant = false
        member x.GetReader() = setHistory.NewReader()
        member x.Content = setHistory :> IMod<_>

    interface alist<'a> with
        member x.IsConstant = false
        member x.GetReader() = history.NewReader()
        member x.Content = history :> IMod<_>
        
    interface IEnumerable with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator() :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator()


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module COrderedSet =
    
    let empty<'a> = corderedset<_>()

    let ofSeq (l : seq<'a>) : corderedset<'a> = corderedset(l)