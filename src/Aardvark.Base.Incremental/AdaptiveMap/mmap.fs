namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base

[<StructuredFormatDisplay("{AsString}")>]
[<CompiledName("MutableMap")>]
type mmap<'k, 'v>(initial : hmap<'k, 'v>) =
    let history = History HMap.trace
    do 
        let delta = HMap.computeDelta HMap.empty initial
        history.Perform delta |> ignore

    let mutable current = initial

    member x.Update(values : hmap<'k, 'v>) =
        if not (Object.ReferenceEquals(values, current)) then
            let delta = HMap.computeDelta current values
            current <- values

            if not (HMap.isEmpty delta) then
                history.Perform delta |> ignore

    member private x.AsString = x.ToString()
    override x.ToString() = current.ToString()
        
    member x.Content = history :> IMod<_>

    member x.Value
        with get() = current
        and set v = x.Update v

    interface IEnumerable with
        member x.GetEnumerator() = (current :> IEnumerable).GetEnumerator()

    interface IEnumerable<'k * 'v> with
        member x.GetEnumerator() = (current :> seq<_>).GetEnumerator()
        
    interface amap<'k, 'v> with
        member x.IsConstant = false
        member x.Content = history :> IMod<_>
        member x.GetReader() = history.NewReader()
        
module MMap =
    let empty<'k, 'v> : mmap<'k, 'v> = mmap(HMap.empty)
    
    let inline ofHMap (s : hmap<'k, 'v>) = mmap(s)
    let inline ofSeq (s : seq<'k * 'v>) = mmap(HMap.ofSeq s)
    let inline ofList (s : list<'k * 'v>) = mmap(HMap.ofList s)
    let inline ofArray (s : ('k * 'v)[]) = mmap(HMap.ofArray s)

    let inline toSeq (s : mmap<'k, 'v>) = s :> seq<_>
    let inline toList (s : mmap<'k, 'v>) = s.Value |> HMap.toList
    let inline toArray (s : mmap<'k, 'v>) = s.Value |> HMap.toArray
    let inline toHMap (s : mmap<'k, 'v>) = s.Value

    let inline value (m : mmap<'k, 'v>) = m.Value
    let inline toMod (m : mmap<'k, 'v>) = m.Content

    let inline change (m : mmap<'k, 'v>) (value : hmap<'k, 'v>) = m.Update value