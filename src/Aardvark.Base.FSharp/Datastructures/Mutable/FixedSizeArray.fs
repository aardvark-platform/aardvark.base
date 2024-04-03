namespace Aardvark.Base

[<AutoOpen>]
module Arrays =

    open System
    open System.Collections.Generic

    [<StructuredFormatDisplay("{AsString}")>]
    type Arr<'d, 'a when 'd :> INatural>(elements : seq<'a>) =
        static let size = typeSize<'d>
        let data = Array.zeroCreate size
        do let elements = elements |> Seq.toArray
           let l = min elements.Length size
           for i in 0..l-1 do
           data.[i] <- elements.[i]

        member x.Data = data

        member x.Item
            with get i = data.[i]
            and set i v = data.[i] <- v

        member x.Length = size

        member x.AsString =
            sprintf "%A" data

        new() = Arr []

        interface IEnumerable<'a> with
            member x.GetEnumerator() : System.Collections.Generic.IEnumerator<'a> = (data |> Array.toSeq).GetEnumerator()
            member x.GetEnumerator() : System.Collections.IEnumerator = data.GetEnumerator()

    [<ReflectedDefinition>]
    type FixedList<'d, 'a when 'd :> INatural> = { storage : Arr<'d, 'a>; mutable Count : int } with
        member x.Item
            with get i = x.storage.[i]
            and set i v = x.storage.[i] <- v

        member x.Add(value : 'a) =
            x.storage.[x.Count] <- value
            x.Count <- x.Count + 1

        member x.RemoveAt(index : int) =
            for i in index+1..x.Count-1 do
                x.storage.[i-1] <- x.storage.[i]
            x.Count <- x.Count - 1
//
//    let z<'a> = Arr<Z, 'a>([||])
//
//    let (|&) (a : Arr<'d, 'a>) (v : 'a) : Arr<S<'d>, 'a> =
//        let data = a.Data
//        let inner = Array.concat [data; [|v|]]
//        Arr<S<'d>, 'a>(inner)
//
//    let (~~) (v : 'a) : Arr<S<Z>, 'a> =
//        Arr [v]

    [<ReflectedDefinition>]
    module Arr =
        let inline map ([<InlineIfLambda>] f : 'a -> 'b) (a : Arr<'d, 'a>) : Arr<'d, 'b> =
            let result = Array.zeroCreate a.Length
            for i in 0..a.Length-1 do
                result.[i] <- f a.[i]
            Arr<'d,'b>(result)

        let inline fold (f : 's -> 'a -> 's) (seed : 's) (a : Arr<'d, 'a>) : 's =
            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt f
            let mutable result = seed
            for i in 0..a.Length-1 do
                result <- f.Invoke(result, a.[i])
            result

        let inline foldBack (f : 'a -> 's -> 's) (seed : 's) (a : Arr<'d, 'a>) : 's =
            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt f
            let mutable result = seed
            for i in 1..a.Length do
                let i = a.Length-i
                result <- f.Invoke(a.[i], result)
            result

        let inline sumBy ([<InlineIfLambda>] f : 'a -> 'b) (arr : Arr<'d, 'a>)=
            fold (fun s v -> s + (f v)) GenericValues.zero arr

        let inline sum (arr : Arr<'d, 'a>)=
            fold (+) GenericValues.zero arr

        let ofList (l : list<'a>) : Arr<'d, 'a> =
            Arr(l)

        let ofSeq (l : seq<'a>) : Arr<'d, 'a> =
            Arr(l)

    [<ReflectedDefinition>]
    module ArrList =

        
        let empty<'d, 'a when 'd :> INatural> : FixedList<'d, 'a> = { storage = Arr<'d, 'a>(); Count = 0 } 

        let inline map ([<InlineIfLambda>] f : 'a -> 'b) (l : FixedList<'d, 'a>) : FixedList<'d, 'b> =
            let result = Arr<'d, 'b>()
            for i in 0..l.Count-1 do
                result.[i] <- f l.storage.[i]
            { storage =result; Count = l.Count } 

        let inline choose ([<InlineIfLambda>] f : 'a -> Option<'b>) (l : FixedList<'d, 'a>) : FixedList<'d, 'b> =
            let result = Arr<'d, 'b>()
            let mutable count = 0

            for i in 0..l.Count-1 do
                match f l.[i] with
                    | Some v ->  
                        result.[count] <- v
                        count <- count + 1
                    | None -> ()

            { storage = result; Count = count } 

        let inline fold (acc : 'a -> 'b -> 'a) (seed : 'a) (l : FixedList<'d, 'b>) : 'a =
            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt acc
            let mutable result = seed
            for i in 0..l.Count-1 do
                result <- f.Invoke(result, l.storage.[i])
            result

        let inline foldBack (acc : 'b -> 'a -> 'a) (seed : 'a) (l : FixedList<'d, 'b>) : 'a =
            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt acc
            let mutable result = seed
            for i in 1..l.Count do
                result <- f.Invoke(l.storage.[i - l.Count], result)
            result

        let inline sumBy ([<InlineIfLambda>] f : 'a -> 'b) (l : FixedList<'d, 'a>) : 'b =
            let mutable result = GenericValues.zero
            for i in 0..l.Count-1 do
                result <- result + f l.storage.[i]
            result

        let inline sum (l : FixedList<'d, 'a>) : 'a =
            let mutable result = GenericValues.zero
            for i in 0..l.Count-1 do
                result <- result + l.storage.[i]
            result

        let inline filter ([<InlineIfLambda>] condition : 'a -> bool) (l : FixedList<'d, 'a>) : FixedList<'d, 'a> =
            let result = Arr<'d, 'a>()
            let mutable count = 0

            for i in 0..l.Count-1 do
                if condition l.[i] then 
                    result.[count] <- l.storage.[i]
                    count <- count + 1

            { storage = result; Count = count } 

        let ofArr (a : Arr<'d, 'a>) =
            { storage = a; Count = a.Length } 

    module List =
        let toFixed<'d, 'a when 'd :> INatural> (l : list<'a>) =
            Arr<'d, 'a>(l)

    module Seq =
        let toFixed<'d, 'a when 'd :> INatural> (l : seq<'a>) =
            Arr<'d, 'a>(l)

    module Array =
        let toFixed<'d, 'a when 'd :> INatural> (l : seq<'a>) =
            Arr<'d, 'a>(l)

    [<return: Struct>]
    let (|FixedArrayType|_|) (t : Type) =
        if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Arr<Z, int>> then
            let targs = t.GetGenericArguments()
            ValueSome (getSize targs.[0], targs.[1])
        else
            ValueNone

    [<return: Struct>]
    let (|FixedArray|_|) (o : obj) =
        let t = o.GetType()
        match t with
        | FixedArrayType(_,content) ->
            let store = t.GetProperty("Data").GetValue(o) |> unbox<Array>
            let result = Array.create store.Length null
            for i in 0..store.Length-1 do
                result.[i] <- store.GetValue(i)

            ValueSome (content, result)

        | _ -> ValueNone

