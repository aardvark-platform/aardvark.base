namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Dictionary =
    open System.Collections.Generic

    let empty<'k, 'v when 'k : equality> = Dictionary<'k, 'v>()
    let emptyNoEquality<'k,'v> =
        Dictionary<'k,'v>(
            { new IEqualityComparer<'k> with
                    member x.Equals(a,b)    = Unchecked.equals a b
                    member x.GetHashCode(t) = Unchecked.hash t
            })

    let inline add (key : 'k) (value : 'v) (d : Dictionary<'k, 'v>) =
        d.Add(key,value)

    let inline set (key : 'k) (value : 'v) (d : Dictionary<'k, 'v>) =
        d.[key] <- value

    let inline remove (key : 'k) (d : Dictionary<'k, 'v>) =
        d.Remove key

    let inline clear (d : Dictionary<'k, 'v>) =
        d.Clear()

    let inline map (f : 'k -> 'a -> 'b) (d : Dictionary<'k, 'a>) =
        let result = Dictionary()
        for (KeyValue(k,v)) in d do
            result.[k] <- f k v
        result

    let inline mapKeys (f : 'k -> 'a -> 'b) (d : Dictionary<'k, 'a>) =
        let result = Dictionary()
        for (KeyValue(k,v)) in d do
            result.[f k v] <- v
        result

    let inline union (dicts : #seq<Dictionary<'k, 'v>>) =
        let result = Dictionary()
        for d in dicts do
            for v in d do
                result.[v.Key] <- v.Value
        result

    let inline contains (key : 'k) (d : Dictionary<'k, 'v>) =
        d.ContainsKey key

    let inline tryFind (key : 'k) (d : Dictionary<'k, 'v>) =
        match d.TryGetValue key with
        | (true, v) -> Some v
        | _ -> None

    let inline tryFindV (key : 'k) (d : Dictionary<'k, 'v>) =
        let mutable value = Unchecked.defaultof<_>
        if d.TryGetValue(key, &value) then ValueSome value
        else ValueNone

    let inline ofSeq (elements : seq<'k * 'v>) =
        let result = Dictionary()
        for (k,v) in elements do
            result.[k] <- v
        result

    let inline ofSeqV (elements : seq<struct('k * 'v)>) =
        let result = Dictionary()
        for (k,v) in elements do
            result.[k] <- v
        result

    let inline ofList (elements : list<'k * 'v>) =
        ofSeq elements

    let inline ofListV (elements : list<struct('k * 'v)>) =
        ofSeqV elements

    let inline ofArray (elements : ('k * 'v)[]) =
        ofSeq elements

    let inline ofArrayV (elements : (struct('k * 'v))[]) =
        ofSeqV elements

    let inline ofMap (elements : Map<'k, 'v>) =
        elements |> Map.toSeq |> ofSeq

    let inline toSeq (d : Dictionary<'k, 'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> k,v)

    let inline toSeqV (d : Dictionary<'k, 'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> struct(k,v))

    let inline toList (d : Dictionary<'k, 'v>) =
        d |> toSeq |> Seq.toList

    let inline toListV (d : Dictionary<'k, 'v>) =
        d |> toSeqV |> Seq.toList

    let inline toArray (d : Dictionary<'k, 'v>) =
        d |> toSeq |> Seq.toArray

    let inline toArrayV (d : Dictionary<'k, 'v>) =
        d |> toSeqV |> Seq.toArray

    let inline toMap (d : Dictionary<'k, 'v>) =
        d |> toSeq |> Map.ofSeq

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Dict =

    #if __SLIM__
    let empty<'k, 'v> = Dictionary.emptyNoEquality<'k,'v>
    #else
    let empty<'k, 'v> = Dict<'k,'v>()
    #endif

    let inline add (key : 'k) (value : 'v) (d : Dict<'k, 'v>) =
        d.Add(key,value)

    let inline set (key : 'k) (value : 'v) (d : Dict<'k, 'v>) =
        d.[key] <- value

    let inline remove (key : 'k) (d : Dict<'k, 'v>) =
        d.Remove key

    let inline clear (d : Dict<'k, 'v>) =
        d.Clear()

    let inline keys (k : Dict<'k,'v>) =
        #if __SLIM__
        k.Keys |> Seq.map id
        #else
        k.Keys
        #endif
    let inline values (k : Dict<'k,'v>) =
        #if __SLIM__
        k.Values |> Seq.map id
        #else
        k.Values
        #endif
    let inline keyValues (k : Dict<'k,'v>) =
        #if __SLIM__
        k |> Seq.map id
        #else
        k.KeyValuePairs
        #endif

    let inline map (f : 'k -> 'a -> 'b) (d : Dict<'k, 'a>) =
        let result = Dict()
        for (KeyValue(k,v)) in d do
            result.[k] <- f k v
        result

    let inline mapKeys (f : 'k -> 'a -> 'b) (d : Dict<'k, 'a>) =
        let result = Dict()
        for (KeyValue(k,v)) in d do
            result.[f k v] <- v
        result

    let inline union (dicts : #seq<Dict<'k, 'v>>) =
        let result = Dict()
        for d in dicts do
            for (KeyValue(k,v)) in d do
                result.[k] <- v
        result

    let inline contains (key : 'k) (d : Dict<'k, 'v>) =
        d.ContainsKey key

    let inline tryFind (key : 'k) (d : Dict<'k, 'v>) =
        match d.TryGetValue key with
        | (true, v) -> Some v
        | _ -> None

    let inline tryFindV (key : 'k) (d : Dict<'k, 'v>) =
        let mutable value = Unchecked.defaultof<_>
        if d.TryGetValue(key, &value) then ValueSome value
        else ValueNone

    let inline ofSeq (elements : seq<'k * 'v>) =
        let result = Dict()
        for (k,v) in elements do
            result.[k] <- v
        result

    let inline ofSeqV (elements : seq<struct('k * 'v)>) =
        let result = Dict()
        for (k,v) in elements do
            result.[k] <- v
        result

    let inline ofList (elements : list<'k * 'v>) =
        ofSeq elements

    let inline ofListV (elements : list<struct('k * 'v)>) =
        ofSeqV elements

    let inline ofArray (elements : ('k * 'v)[]) =
        ofSeq elements

    let inline ofArrayV (elements : struct('k * 'v)[]) =
        ofSeqV elements

    let inline ofMap (elements : Map<'k, 'v>) =
        elements |> Map.toSeq |> ofSeq

    let inline toSeq (d : Dict<'k, 'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> k,v)

    let inline toSeqV (d : Dict<'k, 'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> struct(k,v))

    let inline toList (d : Dict<'k, 'v>) =
        d |> toSeq |> Seq.toList

    let inline toListV (d : Dict<'k, 'v>) =
        d |> toSeqV |> Seq.toList

    let inline toArray (d : Dict<'k, 'v>) =
        d |> toSeq |> Seq.toArray

    let inline toArrayV (d : Dict<'k, 'v>) =
        d |> toSeqV |> Seq.toArray

    let inline toMap (d : Dict<'k, 'v>) =
        d |> toSeq |> Map.ofSeq

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SymDict =

    let empty<'v> = SymbolDict<'v>()

    let inline add (key : Symbol) (value : 'v) (d : SymbolDict<'v>) =
        d.Add(key,value)

    let inline set (key : Symbol) (value : 'v) (d : SymbolDict<'v>) =
        d.[key] <- value

    let inline remove (key : Symbol) (d : SymbolDict<'v>) =
        d.Remove key

    let inline clear (d : SymbolDict<'v>) =
        d.Clear()

    let inline map (f : Symbol -> 'a -> 'b) (d : SymbolDict<'a>) =
        let result = SymbolDict()
        for (KeyValue(k,v)) in d do
            result.[k] <-  f k v
        result

    let inline mapKeys (f : Symbol -> 'a -> Symbol) (d : SymbolDict<'a>) =
        let result = SymbolDict()
        for (KeyValue(k,v)) in d do
            result.[f k v] <- v
        result

    let inline union (dicts : #seq<SymbolDict<'v>>) =
        let result = SymbolDict()
        for d in dicts do
            for v in d do
                result.[v.Key] <- v.Value
        result

    let inline contains (key : Symbol) (d : SymbolDict<'v>) =
        d.ContainsKey key

    let inline tryFind (key : Symbol) (d : SymbolDict<'v>) =
        match d.TryGetValue key with
        | (true, v) -> Some v
        | _ -> None

    let inline tryFindV (key : Symbol) (d : SymbolDict<'v>) =
        let mutable value = Unchecked.defaultof<_>
        if d.TryGetValue(key, &value) then ValueSome value
        else ValueNone

    let inline ofSeq (elements : seq<Symbol * 'v>) =
        let result = SymbolDict()
        for (k,v) in elements do
            result.Add(k,v)
        result

    let inline ofSeqV (elements : seq<struct(Symbol * 'v)>) =
        let result = SymbolDict()
        for (k,v) in elements do
            result.Add(k,v)
        result

    let inline ofList (elements : list<Symbol * 'v>) =
        ofSeq elements

    let inline ofListV (elements : list<struct(Symbol * 'v)>) =
        ofSeqV elements

    let inline ofArray (elements : (Symbol * 'v)[]) =
        ofSeq elements

    let inline ofArrayV (elements : (struct(Symbol * 'v))[]) =
        ofSeqV elements

    let inline ofMap (elements : Map<Symbol, 'v>) =
        elements |> Map.toSeq |> ofSeq

    let inline toSeq (d : SymbolDict<'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> k,v)

    let inline toSeqV (d : SymbolDict<'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> struct (k,v))

    let inline toList (d : SymbolDict<'v>) =
        d |> toSeq |> Seq.toList

    let inline toListV (d : SymbolDict<'v>) =
        d |> toSeqV |> Seq.toList

    let inline toArray (d : SymbolDict<'v>) =
        d |> toSeq |> Seq.toArray

    let inline toArrayV (d : SymbolDict<'v>) =
        d |> toSeqV |> Seq.toArray

    let inline toMap (elements : SymbolDict<'v>) =
        elements |> toSeq |> Map.ofSeq


[<AutoOpen>]
module CSharpCollectionExtensions =
    open System
    open System.Runtime.CompilerServices
    open System.Collections.Generic
    open System.Runtime.InteropServices

    type Dictionary<'Key, 'Value> with
        member inline x.TryFind(key : 'Key) : 'Value option = Dictionary.tryFind key x
        member inline x.TryFindV(key : 'Key) : 'Value voption = Dictionary.tryFindV key x

    type Dict<'Key, 'Value> with
        member inline x.TryFind(key : 'Key) : 'Value option = Dict.tryFind key x
        member inline x.TryFindV(key : 'Key) : 'Value voption = Dict.tryFindV key x

    type SymbolDict<'Value> with
        member inline x.TryFind(key : Symbol) : 'Value option = SymDict.tryFind key x
        member inline x.TryFindV(key : Symbol) : 'Value voption = SymDict.tryFindV key x

    type public DictionaryExtensions =

        [<Obsolete("Broken. Use IDictionary.TryPop instead.")>]
        [<Extension>]
        static member TryRemove(x : Dictionary<'a,'b>, k,[<Out>] r: byref<'b>) =
            match x.TryGetValue k with
            | (true,v) -> r <- v; true
            | _ -> false

        [<Obsolete("Use IDictionary.GetCreate instead.")>]
        [<Extension>]
        static member GetOrAdd(x : Dictionary<'a,'b>, k : 'a, creator : 'a -> 'b) =
            match x.TryGetValue k with
            | (true,v) -> v
            | _ ->
                let v = creator k
                x.Add(k,v) |> ignore
                v