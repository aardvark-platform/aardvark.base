namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open Aardvark.Base

[<StructuredFormatDisplay("{AsString}")>]
[<CompiledName("ChangeableSet")>]
type cset<'a>(initial : seq<'a>) =
    let history = History HRefSet.traceNoRefCount
    do initial |> Seq.map Add |> HDeltaSet.ofSeq |> history.Perform |> ignore

    member x.Add(v : 'a) =
        lock x (fun () ->
            let op = HDeltaSet.single (Add v)
            history.Perform op
        )
    
    member x.AddRange (items : seq<'a>) =
        lock x (fun () ->
            let ops = HDeltaSet.ofSeq (items |> Seq.map (fun v -> Add v))
            history.Perform ops
        )

    member x.Remove(v : 'a) =
        lock x (fun () ->
            let op = HDeltaSet.single (Rem v)
            history.Perform op
        )

    member x.Contains (v : 'a) =
        history.State |> HRefSet.contains v

    member x.Count =
        history.State.Count

    member x.UnionWith (other : seq<'a>) =
        lock x (fun () -> 
            let op = other |> Seq.map Add |> HDeltaSet.ofSeq
            history.Perform op |> ignore
        )

    member x.ExceptWith (other : seq<'a>) =
        lock x (fun () -> 
            let op = other |> Seq.map Rem |> HDeltaSet.ofSeq
            history.Perform op |> ignore
        )

    member x.IntersectWith (other : seq<'a>) =
        lock x (fun () -> 
            let other = HRefSet.ofSeq other
            let op = HRefSet.computeDelta (HRefSet.difference history.State other) HRefSet.empty
            history.Perform op |> ignore
        )

    member x.SymmetricExceptWith (other : seq<'a>) = 
        let other = HRefSet.ofSeq other
        lock x (fun () -> 
            let add = HRefSet.computeDelta HRefSet.empty (HRefSet.difference other history.State) 
            let rem = HRefSet.computeDelta (HRefSet.intersect other history.State) HRefSet.empty
            let op = HDeltaSet.combine add rem
            history.Perform op |> ignore
        )

    member x.Clear() =
        lock x (fun () -> 
            let op = HRefSet.computeDelta history.State HRefSet.empty
            history.Perform op |> ignore
        )

    member x.CopyTo(arr : 'a[], index : int) =
        let mutable index = index
        for e in x do
            arr.[index] <- e
            index <- index + 1

    interface ICollection<'a> with 
        member x.Add(v) = x.Add v |> ignore
        member x.Clear() = x.Clear()
        member x.Remove(v) = x.Remove v
        member x.Contains(v) = x.Contains v
        member x.CopyTo(arr,i) = x.CopyTo(arr, i)
        member x.IsReadOnly = false
        member x.Count = x.Count

    interface ISet<'a> with
        member x.Add v = x.Add v
        member x.ExceptWith o = x.ExceptWith o
        member x.UnionWith o = x.UnionWith o
        member x.IntersectWith o = x.IntersectWith o
        member x.SymmetricExceptWith o = x.SymmetricExceptWith o
        member x.IsSubsetOf o = 
            match HRefSet.compare history.State (HRefSet.ofSeq o) with 
                | SetCmp.ProperSubset | SetCmp.Equal -> true
                | _ -> false

        member x.IsProperSubsetOf o = 
            match HRefSet.compare history.State (HRefSet.ofSeq o) with 
                | SetCmp.ProperSubset -> true
                | _ -> false
            
        member x.IsSupersetOf o = 
            match HRefSet.compare history.State (HRefSet.ofSeq o) with 
                | SetCmp.ProperSuperset | SetCmp.Equal -> true
                | _ -> false

        member x.IsProperSupersetOf o = 
            match HRefSet.compare history.State (HRefSet.ofSeq o) with 
                | SetCmp.ProperSuperset -> true
                | _ -> false

        member x.Overlaps o = 
            match HRefSet.compare history.State (HRefSet.ofSeq o) with 
                | SetCmp.Distinct -> false
                | _ -> true

        member x.SetEquals o = 
            match HRefSet.compare history.State (HRefSet.ofSeq o) with 
                | SetCmp.Equal -> true
                | _ -> false

    interface aset<'a> with
        member x.IsConstant = false
        member x.Content = history :> IMod<_>
        member x.GetReader() = history.NewReader()

    interface IEnumerable with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator() :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = (history.State :> seq<_>).GetEnumerator() :> _

    override x.ToString() =
        let suffix =
            if x.Count > 5 then "; ..."
            else ""

        let content =
            history.State |> Seq.truncate 5 |> Seq.map (sprintf "%A") |> String.concat "; "

        "cset [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    new() = cset<'a>(Seq.empty)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module CSet =
    let inline empty<'a> = cset<'a>()
    
    let inline ofSet (set : hrefset<'a>) = cset(set)
    let inline ofSeq (seq : seq<'a>) = cset(seq)
    let inline ofList (list : list<'a>) = cset(list)
    let inline ofArray (list : array<'a>) = cset(list)

    let inline add (v : 'a) (set : cset<'a>) = set.Add v
    let inline remove (v : 'a) (set : cset<'a>) = set.Remove v
    let inline clear (set : cset<'a>) = set.Clear()

    let inline unionWith (other : seq<'a>) (set : cset<'a>) = set.UnionWith other
    let inline exceptWith (other : seq<'a>) (set : cset<'a>) = set.ExceptWith other
    let inline intersectWith (other : seq<'a>) (set : cset<'a>) = set.IntersectWith other
    let inline symmetricExceptWith (other : seq<'a>) (set : cset<'a>) = set.SymmetricExceptWith other