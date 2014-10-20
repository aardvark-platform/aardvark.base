#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif
open System
open System.Collections.Generic
open System.Threading


module GenericValues =
    open System.Reflection

    let inline zero< ^a when ^a : (static member (-) : ^a -> ^a -> ^a)> : ^a =
        let t = typeof< ^a>
        let pi = t.GetProperty("Zero", BindingFlags.Static ||| BindingFlags.Public)
        let fi = t.GetField("Zero", BindingFlags.Static ||| BindingFlags.Public)

        if pi <> null then pi.GetValue(null) |> unbox
        elif fi <> null then fi.GetValue(null) |> unbox
        else Unchecked.defaultof< ^a>

module Log =
    
    let liftPrintf (f : string * obj[] -> 'a) =
         Printf.kprintf (fun str -> f("{0}", [|str :> obj|]) |> ignore)

    let line fmt = liftPrintf Report.Line fmt
    let warn fmt = liftPrintf (fun (fmt,args) -> Report.Warn(fmt, args)) fmt
    let error fmt = liftPrintf (fun (fmt,args) -> Report.Error(fmt, args)) fmt

    let start fmt = liftPrintf Report.Begin fmt
    let close() = Report.End() |> ignore
    let closeP fmt = liftPrintf Report.End fmt

    let check c fmt =
        if not c then
            warn fmt
        else
            Printf.kprintf ignore fmt

[<AutoOpen>]
module Prelude =

    type Delta<'a> = Add of 'a | Remove of 'a

    module Map =
        let union (l : Map<'k, 'v>) (r : Map<'k, 'v>) =
            let mutable result = l
            for KeyValue(k,v) in r do
                result <- Map.add k v result
            result

    module Seq =
        let iter' (f : 'a -> 'b) (s : seq<'a>) = 
            for i in s do
                f i |> ignore 

        let repeat (n : int) (f : unit -> unit) =
            for i in 1..n do
                f()

        let repeat' (n : int) (f : unit -> 'a) =
            for i in 1..n do
                f() |> ignore

        let partition (f : 'a -> bool) (xs : seq<'a>) = Seq.filter f xs, Seq.filter (not << f) xs

        let chooseOption (f : 'a -> Option<'b>) (xs : seq<Option<'a>>) : seq<Option<'b>> =
            seq {
                for x in xs do
                    match x with
                     | None -> ()
                     | Some x -> yield f x
            }

    module List = 
        let rec any (f : 'a -> bool) (s : list<'a>) =
            match s with
                | x::xs ->
                    if f x then true
                    else any f xs
                | [] -> false

        let rec all (f : 'a -> bool) (s : list<'a>) =
            match s with
                | x::xs ->
                    if not <| f x then false
                    else any f xs
                | [] -> true

    let dowhile (f : unit -> bool) =
        while f() do ()

    (* Error datastructure *)
    type Error<'a> = Success of 'a
                   | Error of string

    (* Either left or right *)
    type Either<'a,'b> = Left of 'a 
                       | Right of 'b

    let toFunc (f : 'a -> 'b) : Func<'a, 'b> =
        Func<'a, 'b>(f)

    let fromFunc (f : Func<'a, 'b>) : 'a -> 'b =
        fun x -> f.Invoke(x)


    //VERY IMPORTANT CODE
    let uncurry (f : 'a -> 'b -> 'c) = fun (a,b) -> f a b
    let curry (f : 'a * 'b -> 'c) = fun a b -> f (a,b)

    let schönfinkel = curry
    let deschönfinkel = uncurry

    let frege = curry
    let unfrege = uncurry

    let ಠ_ಠ str = failwith str

    let inline flip f a b = f b a

    let private BinaryDynamicImpl mn : ('T -> 'U -> 'V) =
        let aty = typeof<'T>
        let bty = typeof<'U>
        let minfo = aty.GetMethod(mn,[| aty;bty |])
        (fun x y -> unbox<_>(minfo.Invoke(null,[| box x; box y|])))

    type private PowDynamicImplTable<'T>() = 
        static let result : ('T -> 'T -> 'T) = 
            let aty = typeof<'T>
            if   aty.Equals(typeof<sbyte>)      then unbox<_>(fun (x:sbyte) (y:sbyte)           -> Fun.Pow(int x, float y) |> sbyte)
            elif aty.Equals(typeof<int16>)      then unbox<_>(fun (x:int16) (y:int16)           -> Fun.Pow(int x, float y) |> int16)
            elif aty.Equals(typeof<int32>)      then unbox<_>(fun (x:int32) (y:int32)           -> Fun.Pow(x, float y) |> int)
            elif aty.Equals(typeof<int64>)      then unbox<_>(fun (x:int64) (y:int64)           -> Fun.Pow(x, float y) |> int64)

            elif aty.Equals(typeof<byte>)        then unbox<_>(fun (x:byte) (y:byte)             -> Fun.Pow(int x, float y) |> byte)
            elif aty.Equals(typeof<uint16>)      then unbox<_>(fun (x:uint16) (y:uint16)         -> Fun.Pow(int x, float y) |> uint16)
            elif aty.Equals(typeof<uint32>)      then unbox<_>(fun (x:uint32) (y:uint32)         -> Fun.Pow(int x, float y) |> uint32)
            elif aty.Equals(typeof<uint64>)      then unbox<_>(fun (x:uint64) (y:uint64)         -> Fun.Pow(int64 x, float y) |> uint64)

            elif aty.Equals(typeof<nativeint>)  then unbox<_>(fun (x:nativeint) (y:nativeint)   -> Fun.Pow(int64 x, float y) |> nativeint)
            elif aty.Equals(typeof<float>)      then unbox<_>(fun (x:float) (y:float)           -> Fun.Pow(x, y) )
            elif aty.Equals(typeof<float32>)    then unbox<_>(fun (x:float32) (y:float32)       -> Fun.Pow(x, y))
            elif aty.Equals(typeof<decimal>)    then unbox<_>(fun (x:decimal) (y:decimal)       -> Fun.Pow(float x, float y) |> decimal)
            else BinaryDynamicImpl "Pow" 
        static member Result : ('T -> 'T -> 'T) = result

    let PowDynamic (x : 'T) (y : 'T) = PowDynamicImplTable<'T>.Result x y

    let inline pow< ^T when ^T : (static member (*) : ^T -> ^T -> ^T)> (x: ^T) (y : ^T) : ^T = 
        PowDynamic x y

    let inline clamp (min : ^a) (max : ^a) (x : ^a) =
        if x < min then min
        elif x > max then max
        else x


    let (|Windows|Linux|Mac|) (p : System.OperatingSystem) =
        match p.Platform with
            | System.PlatformID.Unix -> Linux
            | System.PlatformID.MacOSX -> Mac
            | _ -> Windows

//
//
//    module Dictionary =
//        let inline add< ^d, ^k, ^s, ^v when ^d : (member TryGetValue : ^k * byref< ^s> -> bool) and ^d : (member Add : ^k * ^s -> unit) and ^s : (new : unit -> ^s) and ^s : (member Add : ^v -> bool)> (d : ^d, key : ^k,value : ^v) : unit =
//            let mutable set = Unchecked.defaultof< ^s>
//            if (^d : (member TryGetValue : ^k * byref< ^s> -> bool) (d, key, &set)) then
//                (^s : (member Add : ^v -> bool) (set, value)) |> ignore
//            else
//                let set = new ^s()
//                (^d : (member Add : ^k * ^s -> unit) (d, key, set))
//                (^s : (member Add : ^v -> bool) (set, value)) |> ignore


module Caching =

    let memoTable = System.Collections.Concurrent.ConcurrentDictionary<obj,obj>()
    let cacheFunction (f : 'a -> 'b) (a : 'a) :  'b =
        memoTable.GetOrAdd((a,f) :> obj, fun (o:obj) -> f a :> obj)  |> unbox<'b>
            

module IO =
    open System.IO

    let alterFileName str f = Path.Combine (Path.GetDirectoryName str, f (Path.GetFileName str))

    let openStreamStrong path = 
        if File.Exists path 
        then File.Delete path
        new FileStream(path, FileMode.CreateNew)


[<AutoOpen>]
module Arrays =

    open System
    open System.Collections.Generic

    [<StructuredFormatDisplay("{AsString}")>]
    type Arr<'d, 'a when 'd :> INatural>(elements : seq<'a>) =
        static let size = size<'d>
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

    let z<'a> = Arr<Z, 'a>([||])

    let (|&) (a : Arr<'d, 'a>) (v : 'a) : Arr<S<'d>, 'a> =
        let data = a.Data
        let inner = Array.concat [data; [|v|]]
        Arr<S<'d>, 'a>(inner)

    let (~~) (v : 'a) : Arr<S<Z>, 'a> =
        Arr [v]

    [<ReflectedDefinition>]
    module Arr =
        let map (f : 'a -> 'b) (a : Arr<'d, 'a>) : Arr<'d, 'b> =
            let result = Array.zeroCreate a.Length
            for i in 0..a.Length-1 do
                result.[i] <- f a.[i]
            Arr<'d,'b>(result)

        let fold (f : 's -> 'a -> 's) (seed : 's) (a : Arr<'d, 'a>) : 's =
            let mutable result = seed
            for i in 0..a.Length-1 do
                result <- f result a.[i]
            result

        let foldBack (f : 'a -> 's -> 's) (seed : 's) (a : Arr<'d, 'a>) : 's =
            let mutable result = seed
            for i in 1..a.Length do
                let i = a.Length-i
                result <- f a.[i] result
            result

        let inline sumBy (f : 'a -> 'b) (arr : Arr<'d, 'a>)=
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

        let map (f : 'a -> 'b) (l : FixedList<'d, 'a>) : FixedList<'d, 'b> =
            let result = Arr<'d, 'b>()
            for i in 0..l.Count-1 do
                result.[i] <- f l.storage.[i]
            { storage =result; Count = l.Count } 

        let choose (f : 'a -> Option<'b>) (l : FixedList<'d, 'a>) : FixedList<'d, 'b> =
            let result = Arr<'d, 'b>()
            let mutable count = 0

            for i in 0..l.Count-1 do
                match f l.[i] with
                    | Some v ->  
                        result.[count] <- v
                        count <- count + 1
                    | None -> ()

            { storage = result; Count = count } 

        let fold (acc : 'a -> 'b -> 'a) (seed : 'a) (l : FixedList<'d, 'b>) : 'a =
            let mutable result = seed
            for i in 0..l.Count-1 do
                result <- acc result l.storage.[i]
            result

        let foldBack (acc : 'b -> 'a -> 'a) (seed : 'a) (l : FixedList<'d, 'b>) : 'a =
            let mutable result = seed
            for i in 1..l.Count do
                result <- acc l.storage.[i - l.Count] result
            result

        let inline sumBy (f : 'a -> 'b) (l : FixedList<'d, 'a>) : 'b =
            let mutable result = GenericValues.zero
            for i in 0..l.Count-1 do
                result <- result + f l.storage.[i]
            result

        let inline sum (l : FixedList<'d, 'a>) : 'a =
            let mutable result = GenericValues.zero
            for i in 0..l.Count-1 do
                result <- result + l.storage.[i]
            result

        let filter (l : FixedList<'d, 'a>) (condition : 'a -> bool) : FixedList<'d, 'a> =
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

    let (|FixedArrayType|_|) (t : Type) =
        if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Arr<Z, int>> then
            let targs = t.GetGenericArguments()
            FixedArrayType(getSize targs.[0], targs.[1]) |> Some
        else
            None

    let (|FixedArray|_|) (o : obj) =
        let t = o.GetType()
        match t with
            | FixedArrayType(_,content) ->
                let store = t.GetProperty("Store").GetValue(o) |> unbox<Array>
                let result = Array.create store.Length null
                for i in 0..store.Length-1 do
                    result.[i] <- store.GetValue(i)

                FixedArray(content, result) |> Some

            | _ -> None

