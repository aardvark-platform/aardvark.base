namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Aardvark.Base

[<CompiledName("ChangeableMap")>]
type cmap<'k, 'v>(initial : seq<'k * 'v>) =
    let history = History<hmap<'k, 'v>, hdeltamap<'k, 'v>>(HMap.trace)
    do initial |> Seq.map (fun (k,v) -> k, Set v) |> HMap.ofSeq |> history.Perform |> ignore

    member x.Count =
        history.State.Count

    member x.Item
        with get (key : 'k) = 
            history.State.Find(key)
        and set (key : 'k) (value : 'v) =
            lock x (fun () ->
                history.Perform(HMap.single key (Set value)) |> ignore
            )

    member x.TryFind(k : 'k) =
        history.State.TryFind k

    member x.Remove(key : 'k) =
        lock x (fun () ->
            history.Perform(HMap.single key Remove)
        )

    member x.Add(key : 'k, value : 'v) =
        lock x (fun () ->
            if history.State.ContainsKey key then
                raise <| System.Collections.Generic.KeyNotFoundException()

            history.Perform(HMap.single key (Set value)) |> ignore
        )

    member x.Clear() =
        lock x (fun () ->
            history.Perform(history.State |> HMap.map (fun k v -> Remove)) |> ignore
        )

    member x.TryGetValue(key : 'k, [<Out>] value : byref<'v>) =
        match HMap.tryFind key history.State with
            | Some v ->
                value <- v
                true
            | None ->
                false
           
    member x.ContainsKey(key : 'k) =
        history.State.ContainsKey key

    new() = cmap(Seq.empty)

    interface ICollection<KeyValuePair<'k, 'v>> with
        member x.Contains(kvp : KeyValuePair<'k, 'v>) =
            match x.TryGetValue kvp.Key with
                | (true, v) -> Unchecked.equals v kvp.Value
                | _ -> false
        member x.Count = x.Count
        member x.IsReadOnly = false
        member x.Add(kvp : KeyValuePair<'k, 'v>) = x.Add(kvp.Key, kvp.Value)
        member x.Remove(kvp : KeyValuePair<'k, 'v>) = x.Remove(kvp.Key)
        member x.Clear() = x.Clear()        
        member x.CopyTo(arr : KeyValuePair<'k, 'v>[], index : int) =
            let mutable index = index
            for (k,v) in history.State do
                arr.[index] <- KeyValuePair(k,v)
                index <- index + 1

    interface IDictionary<'k, 'v> with
        member x.Keys = history.State |> Seq.map fst |> Seq.toArray :> _
        member x.Values = history.State |> Seq.map snd |> Seq.toArray :> _
        member x.Item
            with get k = x.[k]
            and set k v = x.[k] <- v
        member x.ContainsKey k = x.ContainsKey k
        member x.Add(k,v) = x.Add(k,v)
        member x.Remove(k) = x.Remove(k)
        member x.TryGetValue(k,v) = x.TryGetValue(k, &v)

    interface IEnumerable with
        member x.GetEnumerator() = (history.State |> Seq.map KeyValuePair).GetEnumerator() :> _

    interface IEnumerable<KeyValuePair<'k, 'v>> with
        member x.GetEnumerator() = (history.State |> Seq.map KeyValuePair).GetEnumerator()

    interface amap<'k, 'v> with
        member x.IsConstant = false
        member x.GetReader() = history.NewReader()
        member x.Content = history :> IMod<_>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module CMap =
    
    let empty<'a,'b> = cmap<'a,'b>()
    
    let find    (k : 'k) (m : cmap<'k,'v>) = m.[k]
    let tryFind (k : 'k) (m : cmap<'k,'v>) = m.TryFind(k)
    let remove  (k : 'k) (m : cmap<'k,'v>) = m.Remove k

    let add (k : 'k) (v : 'v) (m : cmap<'k,'v>) = m.Add(k,v)

    let clear (m : cmap<'k,'v>) = m.Clear()
    let count (m : cmap<'k,'v>) = m.Count

