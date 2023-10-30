namespace Aardvark.Base
#nowarn "9"
#nowarn "51"
#nowarn "44"

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

[<AutoOpen>]
module CSharpCollectionExtensions =

    open System.Runtime.CompilerServices
    open System.Collections.Generic
    open System.Runtime.InteropServices 

    type public DictionaryExtensions = 

        [<Extension>] 
        static member TryRemove(x : Dictionary<'a,'b>, k,[<Out>] r: byref<'b>) =
            match x.TryGetValue k with
             | (true,v) -> r <- v; true
             | _ -> false

        [<Extension>] 
        static member GetOrAdd(x : Dictionary<'a,'b>, k : 'a, creator : 'a -> 'b) =
            match x.TryGetValue k with
             | (true,v) -> v
             | _ ->
                let v = creator k
                x.Add(k,v) |> ignore
                v

[<AutoOpen>]
module Threading =
    open System.Threading

    /// Please note that Aardvark.Base.FSharp's MVar implementation is different from Haskell's MVar introduced in
    ///  "Concurrent Haskell" by Simon Peyton Jones, Andrew Gordon and Sigbjorn Finne. 
    /// see also: http://hackage.haskell.org/package/base-4.11.1.0/docs/Control-Concurrent-MVar.html
    /// In our 'wrong' implementation put does not block but overrides the old value.
    /// We use it typically for synchronized sampling use cases.
    type MVar<'a>() =
        let l = obj()

        let mutable hasValue = false
        let mutable content = Unchecked.defaultof<'a>

        member x.Put v = 
            lock l (fun () ->
                content <- v
                if not hasValue then
                    hasValue <- true
                    Monitor.PulseAll l 
            )

        member x.Take () =
            lock l (fun () ->
                while not hasValue do
                    Monitor.Wait l |> ignore
                let v = content
                content <- Unchecked.defaultof<_>
                hasValue <- false
                v
            )

        [<Obsolete>]
        member x.TakeAsync () =
            async {
                let! ct = Async.CancellationToken
                do! Async.SwitchToThreadPool()
                return x.Take()
            }
            
     
    let startThread (f : unit -> unit) =
        let t = new Thread(ThreadStart f)
        t.IsBackground <- true
        t.Start()
        t
        
                 
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module MVar =
        let empty () = MVar<'a>()
        let create a =
            let v = empty()
            v.Put a
            v
        let put (m : MVar<'a>) v = m.Put v
        let take (m : MVar<'a>) = m.Take()
        [<Obsolete>]
        let takeAsync (m : MVar<'a>) = m.TakeAsync ()

    type Interlocked with
        static member Change(location : byref<'a>, f : 'a -> 'a) =
            let mutable initial = location
            let mutable computed = f initial

            while Interlocked.CompareExchange(&location, computed, initial) != initial do
                initial <- location
                computed <- f initial

            computed

        static member Change(location : byref<'a>, f : 'a -> 'a * 'b) =
            let mutable initial = location
            let (n,r) = f initial
            let mutable computed = n
            let mutable result = r

            while Interlocked.CompareExchange(&location, computed, initial) != initial do
                initial <- location
                let (n,r) = f initial
                computed <- n
                result <- r

            result


        static member Change(location : byref<int>, f : int -> int) =
            let mutable initial = location
            let mutable computed = f initial

            while Interlocked.CompareExchange(&location, computed, initial) <> initial do
                initial <- location
                computed <- f initial

            computed

        static member Change(location : byref<int>, f : int -> int * 'b) =
            let mutable initial = location
            let (n,r) = f initial
            let mutable computed = n
            let mutable result = r

            while Interlocked.CompareExchange(&location, computed, initial) <> initial do
                initial <- location
                let (n,r) = f initial
                computed <- n
                result <- r

            result

        static member Change(location : byref<int64>, f : int64 -> int64) =
            let mutable initial = location
            let mutable computed = f initial

            while Interlocked.CompareExchange(&location, computed, initial) <> initial do
                initial <- location
                computed <- f initial

            computed

        static member Change(location : byref<int64>, f : int64 -> int64 * 'b) =
            let mutable initial = location
            let (n,r) = f initial
            let mutable computed = n
            let mutable result = r

            while Interlocked.CompareExchange(&location, computed, initial) <> initial do
                initial <- location
                let (n,r) = f initial
                computed <- n
                result <- r

            result

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


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module IO =
    open System.IO

    let alterFileName str f = Path.Combine (Path.GetDirectoryName str, f (Path.GetFileName str))

    let createFileStream path = 
        if File.Exists path 
        then File.Delete path
        new FileStream(path, FileMode.CreateNew)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Path =
    open System.IO

    let combine (paths : seq<string>) = Path.Combine(paths |> Seq.toArray)

    let andPath first second = combine [| first; second |]

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module File =
    open System.IO

    /// <summary>
    /// Creates the parent directory of the given file path, if it does not exist.
    /// </summary>
    /// <param name="path">The path of the file, whose parent directory is to be created.</param>
    let createParentDirectory (path : string) =
        let info = FileInfo(path)
        if not info.Directory.Exists then
            info.Directory.Create()

    /// <summary>
    /// Creates a new file, writes the specified string array to the file, and then closes the file.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="lines">The lines to write to the file.</param>
    let writeAllLines (path : string) (lines : string[]) =
        File.WriteAllLines(path, lines)

    /// <summary>
    /// Creates a new file, writes the specified string array to the file, and then closes the file.
    /// If the parent directory does not exist, it is created first.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="lines">The lines to write to the file.</param>
    let writeAllLinesSafe (path : string) (lines : string[]) =
        createParentDirectory path
        File.WriteAllLines(path, lines)

    /// <summary>
    /// Creates a new file, writes the specified string to the file, and then closes the file.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="text">The string to write to the file.</param>
    let writeAllText (path : string) (text : string) =
        File.WriteAllText(path, text)

    /// <summary>
    /// Creates a new file, writes the specified string to the file, and then closes the file.
    /// If the parent directory does not exist, it is created first.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="text">The string to write to the file.</param>
    let writeAllTextSafe (path : string) (text : string) =
        createParentDirectory path
        File.WriteAllText(path, text)

    /// <summary>
    /// Creates a new file, writes the specified byte array to the file, and then closes the file.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    let writeAllBytes (path : string) (bytes : uint8[]) =
        File.WriteAllBytes(path, bytes)

    /// <summary>
    /// Creates a new file, writes the specified byte array to the file, and then closes the file.
    /// If the parent directory does not exist, it is created first.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    let writeAllBytesSafe (path : string) (bytes : uint8[]) =
        createParentDirectory path
        File.WriteAllBytes(path, bytes)

    /// <summary>
    /// Opens a text file, reads all lines of the file into a string array, and then closes the file.
    /// </summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A string array containing all lines of the file.</returns>
    let readAllLines (path : string) =
        File.ReadAllLines path

    /// <summary>
    /// Opens a text file, reads all the text in the file into a string, and then closes the file.
    /// </summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A string containing all the text in the file.</returns>
    let readAllText (path : string) =
        File.ReadAllText path

    /// <summary>
    /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
    /// </summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A byte array containing the contents of the file.</returns>
    let readAllBytes (path : string) =
        File.ReadAllBytes path

[<AutoOpen>]
module NativeUtilities =
    open System.Runtime.InteropServices
    open Microsoft.FSharp.NativeInterop

    let private os = System.Environment.OSVersion
    let private notimp() = raise <| NotImplementedException()


    /// <summary>
    /// MSVCRT wraps memory-functions provided by msvcrt.dll on windows systems.
    /// </summary>
    module internal MSVCRT =
        open System
        open System.Runtime.InteropServices

        [<DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memcpy_internal(nativeint dest, nativeint src, UIntPtr size);

        [<DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern int private memcmp_internal(nativeint ptr1, nativeint ptr2, UIntPtr size);

        [<DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memset_internal(nativeint ptr, int value, UIntPtr size);

        [<DllImport("msvcrt.dll", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memmove_internal(nativeint dest, nativeint src, UIntPtr size);


        let memcpy(target : nativeint, source : nativeint, size : unativeint) =
            memcpy_internal(target, source, size) |> ignore

        let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : unativeint) =
            memcmp_internal(ptr1, ptr2, size)

        let memset(ptr : nativeint, value : int, size : unativeint) =
            memset_internal(ptr, value, size) |> ignore

        let memmove(target : nativeint, source : nativeint, size : unativeint) =
            memmove_internal(target, source, size) |> ignore

    /// <summary>
    /// LibC wraps memory-functions provided by libc on linux systems.
    /// </summary>
    module internal LibC =
        open System
        open System.Runtime.InteropServices


        [<DllImport("libc", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memcpy_internal(nativeint dest, nativeint src, UIntPtr size);

        [<DllImport("libc", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern int private memcmp_internal(nativeint ptr1, nativeint ptr2, UIntPtr size);

        [<DllImport("libc", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memset_internal(nativeint ptr, int value, UIntPtr size);

        [<DllImport("libc", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memmove_internal(nativeint dest, nativeint src, UIntPtr size);

        [<DllImport("libc", EntryPoint = "uname", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern int private uname_intern(nativeint buf);


        let mutable osname = null
        let uname() =
            if isNull osname then
                let ptr : nativeptr<byte> = NativePtr.stackalloc 8192
                if uname_intern(NativePtr.toNativeInt ptr) = 0 then
                    osname <- Marshal.PtrToStringAnsi(NativePtr.toNativeInt ptr)
                else
                    failwith "could not get os-name"
            osname
                


        let memcpy(target : nativeint, source : nativeint, size : unativeint) =
            memcpy_internal(target, source, size) |> ignore

        let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : unativeint) =
            memcmp_internal(ptr1, ptr2, size)

        let memset(ptr : nativeint, value : int, size : unativeint) =
            memset_internal(ptr, value, size) |> ignore

        let memmove(target : nativeint, source : nativeint, size : unativeint) =
            memmove_internal(target, source, size) |> ignore

    [<AutoOpen>]
    module PlatformStuff =


        let (|Windows|Linux|Mac|) (p : System.OperatingSystem) =
            match p.Platform with
                | System.PlatformID.Unix ->
                    if LibC.uname() = "Darwin" then Mac
                    else Linux
                | System.PlatformID.MacOSX -> Mac
                | _ -> Windows

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module NativeInt =
        let memcpy (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memcpy(dst, src, unativeint size)
                | _ -> LibC.memcpy(dst, src, unativeint size)

        let memmove (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memmove(dst, src, unativeint size)
                | _ -> LibC.memmove(dst, src, unativeint size)

        let memset (dst : nativeint) (value : int) (size : int) =
            match os with
                | Windows -> MSVCRT.memset(dst, value, unativeint size)
                | _ -> LibC.memset(dst, value, unativeint size)

        let memcmp (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memcmp(dst, src, unativeint size)
                | _ -> LibC.memcmp(dst, src, unativeint size)

        let inline read<'a when 'a : unmanaged> (ptr : nativeint) =
            NativePtr.read (NativePtr.ofNativeInt<'a> ptr) 

        let inline write<'a when 'a : unmanaged> (ptr : nativeint) (value : 'a) =
            NativePtr.write (NativePtr.ofNativeInt<'a> ptr)  value

        let inline get<'a when 'a : unmanaged> (ptr : nativeint) (index : int) =
            NativePtr.get (NativePtr.ofNativeInt<'a> ptr) index
  
        let inline set<'a when 'a : unmanaged> (ptr : nativeint) (index : int) (value : 'a)=
            NativePtr.set (NativePtr.ofNativeInt<'a> ptr) index value

    type Marshal with
        static member Copy(source : nativeint, destination : nativeint, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memcpy(destination, source, length)
                | _ -> LibC.memcpy(destination, source, length)

        static member Move(source : nativeint, destination : nativeint, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memmove(destination, source, length)
                | _ -> LibC.memmove(destination, source, length)

        static member Set(memory : nativeint, value : int, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memset(memory, value, length)
                | _ -> LibC.memset(memory, value, length)

        static member Compare(source : nativeint, destination : nativeint, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memcmp(destination, source, length)
                | _ -> LibC.memcmp(destination, source, length)

                

        static member Copy(source : nativeint, destination : nativeint, length : int) =
            Marshal.Copy(source, destination, unativeint length)

        static member Move(source : nativeint, destination : nativeint, length : int) =
            Marshal.Move(source, destination, unativeint length)

        static member Set(memory : nativeint, value : int, length : int) =
            Marshal.Set(memory, value, unativeint length)

        static member Compare(source : nativeint, destination : nativeint, length : int) =
            Marshal.Compare(source, destination, unativeint length)




        static member inline Copy(source : nativeint, destination : nativeint, length : 'a) =
            Marshal.Copy(source, destination, unativeint length)

        static member inline Move(source : nativeint, destination : nativeint, length : 'a) =
            Marshal.Move(source, destination, unativeint length)

        static member inline Set(memory : nativeint, value : int, length : 'a) =
            Marshal.Set(memory, value, unativeint length)

        static member inline Compare(source : nativeint, destination : nativeint, length : 'a) =
            Marshal.Compare(source, destination, unativeint length)





    let pinned (a : obj) f = 
        let gc = GCHandle.Alloc(a, GCHandleType.Pinned)
        try
            f ( gc.AddrOfPinnedObject() )
        finally
            gc.Free()

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