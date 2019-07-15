namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open Aardvark.Base

[<StructuredFormatDisplay("{AsString}")>]
[<CompiledName("MutableSet")>]
type mset<'a>(initial : hset<'a>) =
    let history = History HRefSet.traceNoRefCount
    do initial |> Seq.map Add |> HDeltaSet.ofSeq |> history.Perform |> ignore

    let mutable current = initial
    
    member x.Update(values : hset<'a>) =
        if not (Object.Equals(values, current)) then
            let ops = HSet.computeDelta current values
            current <- values
            if not (HDeltaSet.isEmpty ops) then
                history.Perform ops |> ignore

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

    interface aset<'a> with
        member x.IsConstant = false
        member x.GetReader() = history.NewReader()
        member x.Content = history :> IMod<_>

module MSet =
    let empty<'a> : mset<'a> = mset(HSet.empty)
    
    let inline ofHSet (s : hset<'a>) = mset(s)
    let inline ofSeq (s : seq<'a>) = mset(HSet.ofSeq s)
    let inline ofList (s : list<'a>) = mset(HSet.ofList s)
    let inline ofArray (s : 'a[]) = mset(HSet.ofArray s)

    let inline toSeq (s : mset<'a>) = s :> seq<_>
    let inline toList (s : mset<'a>) = s.Value |> HSet.toList
    let inline toArray (s : mset<'a>) = s.Value |> HSet.toArray
    let inline toHSet (s : mset<'a>) = s.Value

    let inline value (m : mset<'a>) = m.Value
    let inline toMod (m : mset<'a>) = m.Content

    let inline change (m : mset<'a>) (value : hset<'a>) = m.Update value