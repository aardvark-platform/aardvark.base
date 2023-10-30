namespace Aardvark.Base

#nowarn "9"

open System
open FSharp.NativeInterop

[<AutoOpen>]
module Prelude =

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

        let partition (f : 'a -> bool) (xs : seq<'a>) = 
            let xs = xs |> Seq.map (fun a -> f a, a) |> Seq.cache

            (xs |> Seq.filter (fun (r,v) -> not r) |> Seq.map snd, xs |> Seq.filter (fun (r,v) -> r) |> Seq.map snd)

        let chooseOption (f : 'a -> Option<'b>) (xs : seq<Option<'a>>) : seq<Option<'b>> =
            seq {
                for x in xs do
                    match x with
                     | None -> ()
                     | Some x -> yield f x
            }

        let forany (f : 'a -> bool) (s : seq<'a>) =
            let e = s.GetEnumerator()
            let mutable r = false
            while not r && e.MoveNext() do
                if f e.Current then
                    r <- true
            e.Dispose()
            r

        let foldi (folder : int -> 'State -> 'T -> 'State) (state : 'State) (source : 'T seq) =
            ((0, state), source) ||> Seq.fold (fun (i, state) x ->
                (i + 1), folder i state x
            ) |> snd

        let inline tryPickV chooser (source : seq<'T>) =
            use e = source.GetEnumerator()
            let mutable res = ValueNone

            while (ValueOption.isNone res && e.MoveNext()) do
                res <- chooser e.Current

            res

        open System.Collections
        open System.Collections.Generic

        let atMost (n : int) (s : seq<'a>) : seq<'a> =
            let newEnumerator() =
                let input = s.GetEnumerator()
                let remaining = ref n
                { new IEnumerator<'a> with
                    member x.MoveNext() = 
                        remaining := !remaining - 1
                        !remaining >= 0 && input.MoveNext()
                    member x.Current : obj = input.Current :> obj
                    member x.Dispose() = input.Dispose()
                    member x.Reset() = input.Reset(); remaining := n
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

        let foldi (folder : int -> 'State -> 'T -> 'State) (state : 'State) (list : 'T list) =
            ((0, state), list) ||> List.fold (fun (i, state) x ->
                (i + 1), folder i state x
            ) |> snd

    module Array =
        let rec private forany' (f : 'a -> bool) (index : int) (a : 'a[]) =
            if index >= a.Length then false
            else
                if f a.[index] then true
                else forany' f (index + 1) a

        let forany (f : 'a -> bool) (a : 'a[]) =
            forany' f 0 a

        let foldi (folder : int -> 'State -> 'T -> 'State) (state : 'State) (array : 'T[]) =
            ((0, state), array) ||> Array.fold (fun (i, state) x ->
                (i + 1), folder i state x
            ) |> snd

        let binarySearch (compare : 'T -> int) (array : 'T[]) =
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

        let inline tryPickV chooser (array : _[]) =
            let rec loop i =
                if i >= array.Length then
                    ValueNone
                else
                    match chooser array.[i] with
                    | ValueNone -> loop (i + 1)
                    | res -> res

            loop 0
    
    module Disposable =

        let empty = { new IDisposable with member x.Dispose() = () }

        let inline dispose v = (^a : (member Dispose : unit -> unit) v)

    module Option =
        let inline defaultValue (fallback : 'a) (option : Option<'a>) = 
            match option with
             | Some value ->  value
             | None -> fallback

        let inline toValueOption (value : 'T option) =
            match value with Some x -> ValueSome x | _ -> ValueNone

    module ValueOption =
        let inline toOption (value : 'T voption) =
            match value with ValueSome x -> Some x | _ -> None

    [<AutoOpen>]
    module ValueOptionOperators =
        let inline fstv (struct (x, _)) = x
        let inline sndv (struct (_, y)) = y
    
    type Async with
        static member AwaitTask(t : System.Threading.Tasks.Task) =
            t |> Async.AwaitIAsyncResult |> Async.Ignore

    type nativeptr<'T when 'T : unmanaged> with
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

        let toStream (count : int) (ptr : nativeptr<'a>) : Stream =
            let l = int64 <| sizeof<'a> * count
            new System.IO.UnmanagedMemoryStream(cast ptr, l,l, FileAccess.ReadWrite) :> _

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
    type Error<'a> = Success of 'a
                   | Error of string

    (* Either left or right *)
    type Either<'a,'b> = Left of 'a 
                       | Right of 'b


    let toFunc (f : 'a -> 'b) : Func<'a, 'b> =
        Func<'a, 'b>(f)

    let fromFunc (f : Func<'a, 'b>) : 'a -> 'b =
        fun x -> f.Invoke(x)

    let dowhile (f : unit -> bool) =
        while f() do ()

    //VERY IMPORTANT CODE
    let uncurry (f : 'a -> 'b -> 'c) = fun (a,b) -> f a b
    let curry (f : 'a * 'b -> 'c) = fun a b -> f (a,b)

    let schönfinkel = curry
    let deschönfinkel = uncurry

    let frege = curry
    let unfrege = uncurry

    let ಠ_ಠ str = failwith str

    let inline private refequals<'a when 'a : not struct> (a : 'a) (b : 'a) = Object.ReferenceEquals(a,b)

    let inline (==) (a : 'a) (b : 'a) = refequals a b
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

    let inline zero< ^a when ^a : (static member (-) : ^a -> ^a -> ^a)> : ^a =
        let t = typeof< ^a>
        let pi = t.GetProperty("Zero", BindingFlags.Static ||| BindingFlags.Public)
        let fi = t.GetField("Zero", BindingFlags.Static ||| BindingFlags.Public)

        if not (isNull pi) then pi.GetValue(null) |> unbox
        elif not (isNull fi) then fi.GetValue(null) |> unbox
        else Unchecked.defaultof< ^a>

module Caching =

    let memoTable = System.Collections.Concurrent.ConcurrentDictionary<obj,obj>()
    let cacheFunction (f : 'a -> 'b) (a : 'a) :  'b =
        memoTable.GetOrAdd((a,f) :> obj, fun (o:obj) -> f a :> obj)  |> unbox<'b>

[<AutoOpen>]
module NiceUtilities =

    module LookupTable =
        open System.Collections.Generic

        let lookupTable (l : list<'a * 'b>) =
            let d = Dictionary()
            for (k,v) in l do

                match d.TryGetValue k with
                    | (true, vo) -> failwithf "duplicated lookup-entry: %A (%A vs %A)" k vo v
                    | _ -> ()

                d.[k] <- v

            fun (key : 'a) ->
                match d.TryGetValue key with
                    | (true, v) -> v
                    | _ -> failwithf "unsupported %A: %A" typeof<'a> key


        let lookupTable' (l : list<'a * 'b>) =
            let d = Dictionary()
            for (k,v) in l do
                match d.TryGetValue k with
                    | (true, vo) -> failwithf "duplicated lookup-entry: %A (%A vs %A)" k vo v
                    | _ -> ()

                d.[k] <- v

            fun (key : 'a) ->
                match d.TryGetValue key with
                    | (true, v) -> Some v
                    | _ -> None

module ConversionHelpers =

    [<Obsolete("Use LookupTable.lookupTable' instead.")>]
    let lookupTableOption (l : list<'a * 'b>) : ('a -> 'b option) =
        LookupTable.lookupTable' l

    [<Obsolete("Use LookupTable.lookupTable instead.")>]
    let lookupTable (l : list<'a * 'b>) : ('a -> 'b) =
        LookupTable.lookupTable l

    let inline convertEnum< ^a, ^b when ^a : (static member op_Explicit : ^a -> int)> (fmt : ^a) : ^b =
        let v = int fmt
        if Enum.IsDefined(typeof< ^b >, v) then
            unbox< ^b > v
        else
            failwithf "cannot convert %s %A to %s" typeof< ^a >.Name fmt typeof< ^b >.Name

type float16 = Aardvark.Base.Half