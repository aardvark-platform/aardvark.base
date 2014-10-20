namespace Aardvark.Base

module Dictionary =
    open System.Collections.Generic

    let empty<'k, 'v when 'k : equality> = Dictionary<'k, 'v>()

    let add (key : 'k) (value : 'v) (d : Dictionary<'k, 'v>) =
        d.Add(key,value)

    let set (key : 'k) (value : 'v) (d : Dictionary<'k, 'v>) =
        d.[key] <- value

    let remove (key : 'k) (d : Dictionary<'k, 'v>) =
        d.Remove key

    let clear (d : Dictionary<'k, 'v>) =
        d.Clear()


    let map (f : 'k -> 'a -> 'b) (d : Dictionary<'k, 'a>) =
        let result = Dictionary()
        for (KeyValue(k,v)) in d do
            result.Add(k, f k v)
        result

    let mapKeys (f : 'k -> 'a -> 'b) (d : Dictionary<'k, 'a>) =
        let result = Dictionary()
        for (KeyValue(k,v)) in d do
            result.Add(f k v, v)
        result

    let union (dicts : #seq<Dictionary<'k, 'v>>) =
        let result = Dictionary()
        for d in dicts do
            result.AddRange(d) |> ignore
        result

    let contains (key : 'k) (d : Dictionary<'k, 'v>) =
        d.ContainsKey key

    let tryFind (key : 'k) (d : Dictionary<'k, 'v>) =
        match d.TryGetValue key with
            | (true, v) -> Some v
            | _ -> None

    let ofSeq (elements : seq<'k * 'v>) =
        let result = Dictionary()
        for (k,v) in elements do
            result.Add(k,v)
        result

    let ofList (elements : list<'k * 'v>) =
        ofSeq elements

    let ofArray (elements : ('k * 'v)[]) =
        ofSeq elements

    let ofMap (elements : Map<'k, 'v>) =
        elements |> Map.toSeq |> ofSeq

    let toSeq (d : Dictionary<'k, 'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> k,v)

    let toList (d : Dictionary<'k, 'v>) =
        d |> toSeq |> Seq.toList

    let toArray (d : Dictionary<'k, 'v>) =
        d |> toSeq |> Seq.toArray

    let toMap (d : Dictionary<'k, 'v>) =
        d |> toSeq |> Map.ofSeq

module Dict =

    let empty<'k, 'v when 'k : equality> = Dict<'k, 'v>()

    let add (key : 'k) (value : 'v) (d : Dict<'k, 'v>) =
        d.Add(key,value)

    let set (key : 'k) (value : 'v) (d : Dict<'k, 'v>) =
        d.[key] <- value

    let remove (key : 'k) (d : Dict<'k, 'v>) =
        d.Remove key

    let clear (d : Dict<'k, 'v>) =
        d.Clear()


    let map (f : 'k -> 'a -> 'b) (d : Dict<'k, 'a>) =
        let result = Dict()
        for (KeyValue(k,v)) in d do
            result.Add(k, f k v)
        result

    let mapKeys (f : 'k -> 'a -> 'b) (d : Dict<'k, 'a>) =
        let result = Dict()
        for (KeyValue(k,v)) in d do
            result.Add(f k v, v)
        result

    let union (dicts : #seq<Dict<'k, 'v>>) =
        let result = Dict()
        for d in dicts do
            result.AddRange(d) |> ignore
        result

    let contains (key : 'k) (d : Dict<'k, 'v>) =
        d.ContainsKey key

    let tryFind (key : 'k) (d : Dict<'k, 'v>) =
        match d.TryGetValue key with
            | (true, v) -> Some v
            | _ -> None

    let ofSeq (elements : seq<'k * 'v>) =
        let result = Dict()
        for (k,v) in elements do
            result.Add(k,v)
        result

    let ofList (elements : list<'k * 'v>) =
        ofSeq elements

    let ofArray (elements : ('k * 'v)[]) =
        ofSeq elements

    let ofMap (elements : Map<'k, 'v>) =
        elements |> Map.toSeq |> ofSeq

    let toSeq (d : Dict<'k, 'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> k,v)

    let toList (d : Dict<'k, 'v>) =
        d |> toSeq |> Seq.toList

    let toArray (d : Dict<'k, 'v>) =
        d |> toSeq |> Seq.toArray  

    let toMap (d : Dict<'k, 'v>) =
        d |> toSeq |> Map.ofSeq

module SymDict =

    let empty<'v> = SymbolDict<'v>()

    let add (key : Symbol) (value : 'v) (d : SymbolDict<'v>) =
        d.Add(key,value)

    let set (key : Symbol) (value : 'v) (d : SymbolDict<'v>) =
        d.[key] <- value

    let remove (key : Symbol) (d : SymbolDict<'v>) =
        d.Remove key

    let clear (d : SymbolDict<'v>) =
        d.Clear()


    let map (f : Symbol -> 'a -> 'b) (d : SymbolDict<'a>) =
        let result = SymbolDict()
        for (KeyValue(k,v)) in d do
            result.Add(k, f k v)
        result

    let mapKeys (f : Symbol -> 'a -> Symbol) (d : SymbolDict<'a>) =
        let result = SymbolDict()
        for (KeyValue(k,v)) in d do
            result.Add(f k v, v)
        result

    let union (dicts : #seq<SymbolDict<'v>>) =
        let result = SymbolDict()
        for d in dicts do
            result.AddRange(d) |> ignore
        result

    let contains (key : Symbol) (d : SymbolDict<'v>) =
        d.ContainsKey key

    let tryFind (key : Symbol) (d : SymbolDict<'v>) =
        match d.TryGetValue key with
            | (true, v) -> Some v
            | _ -> None

    let ofSeq (elements : seq<Symbol * 'v>) =
        let result = SymbolDict()
        for (k,v) in elements do
            result.Add(k,v)
        result

    let ofList (elements : list<Symbol * 'v>) =
        ofSeq elements

    let ofArray (elements : (Symbol * 'v)[]) =
        ofSeq elements

    let ofMap (elements : Map<Symbol, 'v>) =
        elements |> Map.toSeq |> ofSeq

    let toSeq (d : SymbolDict<'v>) =
        d |> Seq.map (fun (KeyValue(k,v)) -> k,v)

    let toList (d : SymbolDict<'v>) =
        d |> toSeq |> Seq.toList

    let toArray (d : SymbolDict<'v>) =
        d |> toSeq |> Seq.toArray  

    let toMap (elements : SymbolDict<'v>) =
        elements |> toSeq |> Map.ofSeq
