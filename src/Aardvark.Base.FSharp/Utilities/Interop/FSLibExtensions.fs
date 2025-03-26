namespace Aardvark.Base

#nowarn "9"

open System
open FSharp.NativeInterop

[<AutoOpen>]
module Prelude =

    [<AutoOpen>]
    module ``Assignment Operators`` =

        // Arithmetic
        let inline ( += ) (x: byref<'T1>) (y: 'T2) = x <- x + y
        let inline ( -= ) (x: byref<'T1>) (y: 'T2) = x <- x - y
        let inline ( *= ) (x: byref<'T1>) (y: 'T2) = x <- x * y
        let inline ( /= ) (x: byref<'T1>) (y: 'T2) = x <- x / y
        let inline ( %= ) (x: byref<'T1>) (y: 'T2) = x <- x % y

        // Boolean
        let inline ( ||= ) (x: byref<bool>) (y: bool) = x <- x || y
        let inline ( &&= ) (x: byref<bool>) (y: bool) = x <- x && y

        // Bitwise
        let inline ( |||= ) (x: byref<'T>) (y: 'T) = x <- x ||| y
        let inline ( &&&= ) (x: byref<'T>) (y: 'T) = x <- x &&& y
        let inline ( ^^^= ) (x: byref<'T>) (y: 'T) = x <- x ^^^ y
        let inline ( <<<= ) (value: byref<'T>) (shift: int) = value <- value <<< shift
        let inline ( >>>= ) (value: byref<'T>) (shift: int) = value <- value >>> shift

    let inc (a:byref<int>) = a <- a + 1
    let dec (a:byref<int>) = a <- a - 1

    let inline isNull (a : 'a) =
        match a with
        | null -> true
        | _ -> false

    module Map =
        let union (l : Map<'k, 'v>) (r : Map<'k, 'v>) =
            let mutable result = l
            for KeyValue(k,v) in r do
                result <- Map.add k v result
            result

        let unionMany (input : seq<Map<'k, 'v>>) =
            (Map.empty, input) ||> Seq.fold union

        let inline tryFindV (key: 'Key) (map: Map<'Key, 'Value>) =
            match map.TryGetValue key with
            | true, value -> ValueSome value
            | _ -> ValueNone

        let ofSeqWithDuplicates (input: seq<'Key * 'Value>) =
            let mutable result = Map.empty

            for key, value in input do
                let set =
                    match result.TryGetValue key with
                    | true, set -> set |> Set.add value
                    | _ -> Set.singleton value

                result <- result |> Map.add key set

            result

    module Seq =
        let inline iter' ([<InlineIfLambda>] f : 'a -> 'b) (s : seq<'a>) =
            for i in s do
                f i |> ignore

        let inline repeat (n : int) ([<InlineIfLambda>] f : unit -> unit) =
            for i in 1..n do
                f()

        let inline repeat' (n : int) ([<InlineIfLambda>] f : unit -> 'a) =
            for i in 1..n do
                f() |> ignore

        let inline partition ([<InlineIfLambda>] f : 'a -> bool) (xs : seq<'a>) =
            let xs = xs |> Seq.map (fun a -> f a, a) |> Seq.cache
            (xs |> Seq.filter (fun (r,v) -> not r) |> Seq.map snd, xs |> Seq.filter (fun (r,v) -> r) |> Seq.map snd)

        [<Obsolete("Is anybody actually using this?")>]
        let inline chooseOption ([<InlineIfLambda>] f : 'a -> Option<'b>) (xs : seq<Option<'a>>) : seq<Option<'b>> =
            seq {
                for x in xs do
                    match x with
                     | None -> ()
                     | Some x -> yield f x
            }

        let inline choosei ([<InlineIfLambda>] chooser: int -> 'T -> 'U option) (source: 'T seq) : 'U seq =
            let mutable i = 0
            let result = ResizeArray<'U>()

            use e = source.GetEnumerator()
            while e.MoveNext() do
                match chooser i e.Current with
                | Some v -> result.Add v
                | _ -> ()

                i <- i + 1

            result :> 'U seq

        let inline collecti ([<InlineIfLambda>] mapping: int -> 'T -> 'U seq) (source: 'T seq) : 'U seq =
            let mutable i = 0
            let result = ResizeArray<'U>()

            use e = source.GetEnumerator()
            while e.MoveNext() do
                result.AddRange(mapping i e.Current)
                i <- i + 1

            result :> 'U seq

        let inline foldi (folder : int -> 'State -> 'T -> 'State) (state : 'State) (source : 'T seq) =
            use e = source.GetEnumerator()
            let f = OptimizedClosures.FSharpFunc<_, _, _, _>.Adapt folder
            let mutable state = state
            let mutable i = 0

            while e.MoveNext() do
                state <- f.Invoke(0, state, e.Current)
                i <- i + 1

            state

        let inline tryPickV ([<InlineIfLambda>] chooser) (source : seq<'T>) =
            use e = source.GetEnumerator()
            let mutable res = ValueNone

            while (ValueOption.isNone res && e.MoveNext()) do
                res <- chooser e.Current

            res

        let inline tryFindV ([<InlineIfLambda>] predicate) (source: seq<'T>) =
            use e = source.GetEnumerator()
            let mutable res = ValueNone

            while (ValueOption.isNone res && e.MoveNext()) do
                let c = e.Current

                if predicate c then
                    res <- ValueSome c

            res

        /// Computes the sum of the given sequence using the Kahan summation algorithm.
        let inline stableSumBy (projection: 'T -> float) (source: 'T seq) =
            let mutable sum = KahanSum.Zero
            for x in source do sum <- sum + projection x
            sum.Value

        /// Computes the sum of the given sequence using the Kahan summation algorithm.
        let inline stableSum (source: float seq) =
            stableSumBy id source

        open System.Collections
        open System.Collections.Generic

        let atMost (n : int) (s : seq<'a>) : seq<'a> =
            let newEnumerator() =
                let input = s.GetEnumerator()
                let mutable remaining = n
                { new IEnumerator<'a> with
                    member x.MoveNext() =
                        remaining <- remaining - 1
                        remaining >= 0 && input.MoveNext()
                    member x.Current : obj = input.Current :> obj
                    member x.Dispose() = input.Dispose()
                    member x.Reset() = input.Reset(); remaining <- n
                    member x.Current : 'a = input.Current
                }

            { new IEnumerable<'a> with
                member x.GetEnumerator() : IEnumerator = newEnumerator() :> IEnumerator
                member x.GetEnumerator() : IEnumerator<'a> = newEnumerator()
            }


    module List =

        //Experimental Results show that this implementation is faster than all other ones maintaining the list's order
        let rec private partitionAcc (f : 'a -> bool) (source : list<'a>) (l : System.Collections.Generic.List<'a>) (r : System.Collections.Generic.List<'a>) =
             match source with
                | [] ->
                    let mutable ll = []
                    let mutable rr = []

                    for i in 1..l.Count do
                        let i = l.Count - i
                        ll <- l.[i]::ll

                    for i in 1..r.Count do
                        let i = r.Count - i
                        rr <- r.[i]::rr

                    (ll,rr)

                | x::xs ->
                    if f x then
                        l.Add(x)
                        partitionAcc f xs l r
                    else
                        r.Add(x)
                        partitionAcc f xs l r

        let partition (f : 'a -> bool) (source : list<'a>) =
            partitionAcc f source (System.Collections.Generic.List()) (System.Collections.Generic.List())

        let inline choosei ([<InlineIfLambda>] chooser: int -> 'T -> 'U option) (list: 'T list) =
            let i = ref -1

            let chooser value =
                i.Value <- i.Value + 1
                chooser i.Value value

            List.choose chooser list

        let inline collecti ([<InlineIfLambda>] mapping: int -> 'T -> 'U list) (list: 'T list) =
            let i = ref -1

            let mapping value =
                i.Value <- i.Value + 1
                mapping i.Value value

            List.collect mapping list

        let inline foldi (folder : int -> 'State -> 'T -> 'State) (state : 'State) (list : 'T list) =
            match list with
            | [] -> state
            | _ ->
                let f = OptimizedClosures.FSharpFunc<_, _, _, _>.Adapt(folder)
                let mutable acc = state
                let mutable i = 0

                for x in list do
                    acc <- f.Invoke(i, acc, x)
                    i <- i + 1

                acc

        /// Inserts a separator in between the elements of the given list.
        let inline intersperse (separator: 'T) (list: 'T list) =
            (list, []) ||> List.foldBack (fun x -> function
                | [] -> [x]
                | xs -> x::separator::xs
            )

        /// Computes the sum of the given list using the Kahan summation algorithm.
        let inline stableSumBy (projection: 'T -> float) (list: 'T list) =
            let mutable sum = KahanSum.Zero
            for x in list do sum <- sum + projection x
            sum.Value

        /// Computes the sum of the given list using the Kahan summation algorithm.
        let inline stableSum (list: float list) =
            stableSumBy id list

    module Array =

        let inline choosei ([<InlineIfLambda>] chooser: int -> 'T -> 'U option) (array: 'T[]) =
            let i = ref -1

            let chooser value =
                i.Value <- i.Value + 1
                chooser i.Value value

            Array.choose chooser array

        let inline collecti ([<InlineIfLambda>] mapping: int -> 'T -> 'U[]) (array: 'T[]) =
            let i = ref -1

            let mapping value =
                i.Value <- i.Value + 1
                mapping i.Value value

            Array.collect mapping array

        let inline foldi (folder : int -> 'State -> 'T -> 'State) (state : 'State) (array : 'T[]) =
            let f = OptimizedClosures.FSharpFunc<_, _, _, _>.Adapt(folder)
            let mutable state = state

            for i = 0 to array.Length - 1 do
                state <- f.Invoke(i, state, array.[i])

            state

        let inline binarySearch ([<InlineIfLambda>] compare : 'T -> int) (array : 'T[]) =
            let rec search (a : int) (b : int) =
                if a <= b then
                    let i = (a + b) / 2
                    let cmp = compare array.[i]

                    if cmp = 0 then ValueSome i
                    elif cmp < 0 then search a (i - 1)
                    else search (i + 1) b
                else
                    ValueNone

            search 0 (array.Length - 1)

        let inline tryPickV ([<InlineIfLambda>] chooser) (array : _[]) =
            let rec loop i =
                if i >= array.Length then
                    ValueNone
                else
                    match chooser array.[i] with
                    | ValueNone -> loop (i + 1)
                    | res -> res

            loop 0

        /// Computes the sum of the given array using the Kahan summation algorithm.
        let inline stableSumBy (projection: 'T -> float) (array: 'T[]) =
            let mutable sum = KahanSum.Zero
            for x in array do sum <- sum + projection x
            sum.Value

        /// Computes the sum of the given array using the Kahan summation algorithm.
        let inline stableSum (array: float[]) =
            stableSumBy id array

    module Disposable =

        let empty = { new IDisposable with member x.Dispose() = () }

        let inline dispose v = (^a : (member Dispose : unit -> unit) v)

    module Option =
        let inline toValueOption (value : 'T option) =
            match value with Some x -> ValueSome x | _ -> ValueNone

    module ValueOption =
        let inline toOption (value : 'T voption) =
            match value with ValueSome x -> Some x | _ -> None

    [<AutoOpen>]
    module ValueOptionOperators =
        let inline fstv (struct (x, _)) = x
        let inline sndv (struct (_, y)) = y

    type nativeptr<'T when 'T : unmanaged> with
        member inline ptr.Address = NativePtr.toNativeInt ptr

        member ptr.Value
            with inline get () = NativePtr.read ptr
            and  inline set (value : 'T) = NativePtr.write ptr value

        member ptr.Item
            with inline get (index : int) = NativePtr.get ptr index
            and  inline set (index : int) (value : 'T) = NativePtr.set ptr index value

    module NativePtr =
        open System.Runtime.InteropServices
        open System.IO

        [<GeneralizableValue>]
        let inline zero<'a when 'a : unmanaged> : nativeptr<'a> = NativePtr.ofNativeInt 0n

        let inline cast (ptr : nativeptr<'a>) : nativeptr<'b> =
            ptr |> NativePtr.toNativeInt |> NativePtr.ofNativeInt

        let inline step (count : int) (ptr : nativeptr<'a>) =
            NativePtr.add ptr count

        let inline toArray (cnt : int) (ptr : nativeptr<'a>) =
            Array.init cnt (NativePtr.get ptr)

        let inline toSeq (cnt : int) (ptr : nativeptr<'a>) =
            Seq.init cnt (NativePtr.get ptr)

        let inline toList (cnt : int) (ptr : nativeptr<'a>) =
            List.init cnt (NativePtr.get ptr)

        let inline isNull (ptr : nativeptr<'a>) =
            ptr |> NativePtr.toNativeInt = 0n

        let alloc<'a when 'a: unmanaged> (size : int) =
            size * sizeof<'a> |> Marshal.AllocHGlobal |> NativePtr.ofNativeInt<'a>

        let free (ptr : nativeptr<'a>) =
            ptr |> NativePtr.toNativeInt |> Marshal.FreeHGlobal

        let inline stackUse (elements : seq<'a>) =
            let arr = elements |> Seq.toArray
            let ptr = NativePtr.stackalloc arr.Length
            for i in 0..arr.Length-1 do
                NativePtr.set ptr i arr.[i]
            ptr

        /// Allocates and initializes native memory on the stack from the given array and mapping function.
        let inline stackUseArr ([<InlineIfLambda>] mapping: 'T -> 'U) (data: 'T[]) =
            let ptr = NativePtr.stackalloc<'U> data.Length
            for i = 0 to data.Length - 1 do ptr.[i] <- mapping data.[i]
            ptr

        let toStream (count : int) (ptr : nativeptr<'a>) : Stream =
            let l = int64 <| sizeof<'a> * count
            new System.IO.UnmanagedMemoryStream(cast ptr, l,l, FileAccess.ReadWrite) :> _

        /// Pins the given value and invokes the action with the native pointer.
        /// Note: Use a fixed expression with a byref if writing to the original location is required.
        let inline pin<'T, 'U when 'T : unmanaged> ([<InlineIfLambda>] action: nativeptr<'T> -> 'U) (value: 'T) =
            use ptr = fixed &value
            action ptr

        /// Pins the given array and invokes the action with the native pointer.
        let inline pinArr<'T, 'U when 'T : unmanaged> ([<InlineIfLambda>] action: nativeptr<'T> -> 'U) (array: 'T[])  =
            use ptr = fixed array
            action ptr

        /// Pins the given array at the given index and invokes the action with the native pointer.
        let inline pinArri<'T, 'U when 'T : unmanaged> ([<InlineIfLambda>] action: nativeptr<'T> -> 'U) (index: int) (array: 'T[])  =
            use ptr = fixed &array.[index]
            action ptr

        /// Allocates a temporary native pointer and invokes the action.
        let inline temp<'T, 'U when 'T : unmanaged> ([<InlineIfLambda>] action: nativeptr<'T> -> 'U) =
            pin action Unchecked.defaultof<'T>

        module Operators =

            let ( &+ ) (ptr : nativeptr<'a>) (count : int) =
                ptr |> step count

            let ( &- ) (ptr : nativeptr<'a>) (count : int) =
                ptr |> step (-count)

            let ( !* ) (ptr : nativeptr<'a>) =
                NativePtr.read ptr

    type Buffer with

        static member inline MemoryCopy(source : nativeint, destination : nativeint, destinationSizeInBytes : uint64, sourceBytesToCopy : uint64) =
            Buffer.MemoryCopy(source.ToPointer(), destination.ToPointer(), destinationSizeInBytes, sourceBytesToCopy)

        static member inline MemoryCopy(source : nativeint, destination : nativeint, destinationSizeInBytes : int64, sourceBytesToCopy : int64) =
            Buffer.MemoryCopy(source.ToPointer(), destination.ToPointer(), destinationSizeInBytes, sourceBytesToCopy)


    (* Error datastructure *)
    [<Struct>]
    type Error<'T> =
        | Success of value: 'T
        | Error of error: string

    (* Either left or right *)
    [<Struct>]
    type Either<'L, 'R> =
        | Left of left: 'L
        | Right of right: 'R

    let toFunc (f : 'a -> 'b) : Func<'a, 'b> =
        Func<'a, 'b>(f)

    let fromFunc (f : Func<'a, 'b>) : 'a -> 'b =
        fun x -> f.Invoke(x)

    let dowhile (f : unit -> bool) =
        while f() do ()

    //VERY IMPORTANT CODE
    let uncurry (f : 'a -> 'b -> 'c) = fun (a,b) -> f a b
    let curry (f : 'a * 'b -> 'c) = fun a b -> f (a,b)

    [<AutoOpen>]
    module ReferenceEquality =
        let inline private refequals<'a when 'a : not struct> (a : 'a) (b : 'a) = Object.ReferenceEquals(a,b)

        /// Reference equality
        let inline (==) (a : 'a) (b : 'a) = refequals a b

        /// Reference inequality
        let inline (!=) (a : 'a) (b : 'a) = refequals a b |> not

    let inline flip f a b = f b a

    let inline constF v = fun _ -> v

[<AutoOpen>]
module CSharpInterop =

    open System.Runtime.CompilerServices

    type public FSharpFuncUtil =

        [<Extension>]
        static member ToFSharpFunc<'a,'b> (func:System.Converter<'a,'b>) = fun x -> func.Invoke(x)

        [<Extension>]
        static member ToFSharpFunc<'a,'b> (func:System.Func<'a,'b>) = fun x -> func.Invoke(x)

        [<Extension>]
        static member ToFSharpFunc<'a,'b,'c> (func:System.Func<'a,'b,'c>) = fun x y -> func.Invoke(x,y)

        [<Extension>]
        static member ToFSharpFunc<'a,'b,'c,'d> (func:System.Func<'a,'b,'c,'d>) = fun x y z -> func.Invoke(x,y,z)

        static member Create<'a,'b> (func:System.Func<'a,'b>) = FSharpFuncUtil.ToFSharpFunc func

        static member Create<'a,'b,'c> (func:System.Func<'a,'b,'c>) = FSharpFuncUtil.ToFSharpFunc func

        static member Create<'a,'b,'c,'d> (func:System.Func<'a,'b,'c,'d>) = FSharpFuncUtil.ToFSharpFunc func

module GenericValues =
    open System.Reflection

    [<Obsolete("Use LanguagePrimitives.GenericZero instead.")>]
    let inline zero< ^a when ^a : (static member (-) : ^a -> ^a -> ^a)> : ^a =
        let t = typeof< ^a>
        let pi = t.GetProperty("Zero", BindingFlags.Static ||| BindingFlags.Public)
        let fi = t.GetField("Zero", BindingFlags.Static ||| BindingFlags.Public)

        if not (isNull pi) then pi.GetValue(null) |> unbox
        elif not (isNull fi) then fi.GetValue(null) |> unbox
        else Unchecked.defaultof< ^a>

[<AutoOpen>]
module NiceUtilities =

    module LookupTable =
        open System.Collections.Generic

        let private build (entries: list<'Key * 'Value>) =
            let d = Dictionary()

            for k, v in entries do
                match d.TryGetValue k with
                | (true, vo) -> raise <| ArgumentException($"Duplicate lookup table entry {k}: {v} and {vo}.")
                | _ -> ()

                d.[k] <- v

            d

        /// Builds a lookup table from the given entries and returns a function that retrieves the value associated with a key.
        /// Fails if a key occurs more than once in the input collection.
        let lookup (entries: list<'Key * 'Value>) =
            let d = build entries

            fun (key : 'Key) ->
                match d.TryGetValue key with
                | (true, v) -> v
                | _ -> raise <| KeyNotFoundException($"Key {key} of type {typeof<'Key>} not found in lookup table.")

        /// Builds a lookup table from the given entries and returns a function that retrieves the value associated with a key if it exists.
        /// Fails if a key occurs more than once in the input collection.
        let tryLookup (entries: list<'Key * 'Value>) =
            let d = build entries

            fun (key : 'Key) ->
                match d.TryGetValue key with
                | (true, v) -> Some v
                | _ -> None

        /// Builds a lookup table from the given entries and returns a function that retrieves the value associated with a key if it exists.
        /// Fails if a key occurs more than once in the input collection.
        let tryLookupV (entries: list<'Key * 'Value>) =
            let d = build entries

            fun (key : 'Key) ->
                match d.TryGetValue key with
                | (true, v) -> ValueSome v
                | _ -> ValueNone

        [<Obsolete("Use LookupTable.lookup instead.")>]
        let lookupTable (l : list<'a * 'b>) =
            lookup l

        [<Obsolete("Use LookupTable.tryLookup instead.")>]
        let lookupTable' (l : list<'a * 'b>) =
            tryLookup l

[<AutoOpen>]
module EnumExtensions =

    module Enum =

        /// Converts the given value to an enumeration type.
        /// The input value must be an enumeration type or convertible to the underlying target enumeration type.
        /// If the value is not defined for the target enumeration, an InvalidCastException is thrown.
        let inline convert< ^Value, ^Enum when ^Enum :> Enum
                                           and ^Enum : struct
                                           and ^Enum : (new : unit -> ^Enum)> (value: ^Value) : ^Enum =
            let tSrc = typeof< ^Value>
            let tDst = typeof< ^Enum>
            let tDstBase = Enum.GetUnderlyingType tDst

            let cast: ^Enum =
                if tSrc = tDst || tSrc = tDstBase || (tSrc.IsEnum && Enum.GetUnderlyingType tSrc = tDstBase) then
                    unbox< ^Enum> value
                else
                    if tDstBase = typeof<int8> then unbox< ^Enum> <| Convert.ToSByte value
                    elif tDstBase = typeof<uint8> then unbox< ^Enum> <| Convert.ToByte value
                    elif tDstBase = typeof<int16> then unbox< ^Enum> <| Convert.ToInt16 value
                    elif tDstBase = typeof<uint16> then unbox< ^Enum> <| Convert.ToUInt16 value
                    elif tDstBase = typeof<int32> then unbox< ^Enum> <| Convert.ToInt32 value
                    elif tDstBase = typeof<uint32> then unbox< ^Enum> <| Convert.ToUInt32 value
                    elif tDstBase = typeof<int64> then unbox< ^Enum> <| Convert.ToInt64 value
                    elif tDstBase = typeof<uint64> then unbox< ^Enum> <| Convert.ToUInt64 value
                    else
                        failwithf "Unknown underlying type %A for enumeration" tDstBase

#if NET6_0_OR_GREATER
            if not <| Enum.IsDefined cast then
#else
            if not <| Enum.IsDefined(typeof< ^Enum>, cast) then
#endif
                raise <| InvalidCastException($"Value {value} is invalid for enumeration type {typeof< ^Enum>}.")

            cast

module ConversionHelpers =

    [<Obsolete("Use Enum.convert instead.")>]
    let inline convertEnum< ^a, ^b when ^a : (static member op_Explicit : ^a -> int)> (fmt : ^a) : ^b =
        let v = int fmt
        if Enum.IsDefined(typeof< ^b >, v) then
            unbox< ^b > v
        else
            failwithf "cannot convert %s %A to %s" typeof< ^a >.Name fmt typeof< ^b >.Name

type float16 = Aardvark.Base.Half