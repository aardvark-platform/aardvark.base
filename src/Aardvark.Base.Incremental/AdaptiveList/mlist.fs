namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base

[<StructuredFormatDisplay("{AsString}")>]
[<CompiledName("MutableList")>]
type mlist<'a>(initial : plist<'a>) =
    let history = History PList.trace
    do 
        let delta = plist.ComputeDeltas(PList.empty, initial)
        history.Perform delta |> ignore

    let mutable current = initial

    member x.Update(values : plist<'a>) =
        if not (Object.ReferenceEquals(values, current)) then
            let delta = plist.ComputeDeltas(current, values)
            current <- values
            if not (PDeltaList.isEmpty delta) then
                history.Perform delta |> ignore

    member private x.AsString = x.ToString()
    override x.ToString() = current.ToString()
        
    member x.Content = history :> IMod<_>

    member x.Value
        with get() = current
        and set v = x.Update v

    interface IEnumerable with
        member x.GetEnumerator() = (current :> IEnumerable).GetEnumerator()

    interface IEnumerable<'a> with
        member x.GetEnumerator() = (current :> seq<_>).GetEnumerator()


    interface alist<'a> with
        member x.IsConstant = false
        member x.Content = history :> IMod<_>
        member x.GetReader() = history.NewReader()

        
module MList =
    let empty<'a> : mlist<'a> = mlist(PList.empty)
    
    let inline ofPList (s : plist<'a>) = mlist(s)
    let inline ofSeq (s : seq<'a>) = mlist(PList.ofSeq s)
    let inline ofList (s : list<'a>) = mlist(PList.ofList s)
    let inline ofArray (s : 'a[]) = mlist(PList.ofArray s)

    let inline toSeq (s : mlist<'a>) = s :> seq<_>
    let inline toList (s : mlist<'a>) = s.Value |> PList.toList
    let inline toArray (s : mlist<'a>) = s.Value |> PList.toArray
    let inline toPList (s : mlist<'a>) = s.Value

    let inline value (m : mlist<'a>) = m.Value
    let inline toMod (m : mlist<'a>) = m.Content

    let inline change (m : mlist<'a>) (value : plist<'a>) = m.Update value