namespace Aardvark.Base
#nowarn "9"
#nowarn "51"

open System

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

    module Array =
        let rec private forany' (f : 'a -> bool) (index : int) (a : 'a[]) =
            if index >= a.Length then false
            else
                if f a.[index] then true
                else forany' f (index + 1) a

        let forany (f : 'a -> bool) (a : 'a[]) =
            forany' f 0 a
    
    module Disposable =

        let inline dispose v = (^a : (member Dispose : unit -> unit) v)

    module Option =
        
        let inline defaultValue (fallback : 'a) (option : Option<'a>) = 
            match option with
             | Some value ->  value
             | None -> fallback
    
    type Async with
        static member AwaitTask(t : System.Threading.Tasks.Task) =
            t |> Async.AwaitIAsyncResult |> Async.Ignore

    module NativePtr =
        open Microsoft.FSharp.NativeInterop
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

    type MVar<'a>() =
        let mutable content = Unchecked.defaultof<ref<'a>>
        // feel free to replace atomic + sem by appropriate .net synchronization data type
        let sem = new SemaphoreSlim(0)
        let mutable cnt = 0
        member x.Put v = 
            content <- ref v
            if Interlocked.Exchange(&cnt, 1) = 0 then
                sem.Release() |> ignore
        member x.Take () =
            sem.Wait()
            let res = !Interlocked.Exchange(&content,Unchecked.defaultof<_>)
            cnt <- 0
            res
        member x.TakeAsync () =
            async {
                let! ct = Async.CancellationToken
                do! Async.AwaitTask(sem.WaitAsync(ct))
                let res = !Interlocked.Exchange(&content,Unchecked.defaultof<_>)
                cnt <- 0
                return res
            }
            
            
        
                 
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module MVar =
        let empty () = MVar<'a>()
        let create a =
            let v = empty()
            v.Put a
        let put (m : MVar<'a>) v = m.Put v
        let take (m : MVar<'a>) = m.Take()
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

    let writeAllLines p lines = File.WriteAllLines (p,lines)
    let writeAllText  p text  = File.WriteAllText  (p,text)
    let writeAllBytes p bytes = File.WriteAllBytes (p,bytes)

    let readAllLines p = File.ReadAllLines p
    let readAllText  p = File.ReadAllText  p
    let readAllBytes p = File.ReadAllBytes p

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


        let memcpy(target : nativeint, source : nativeint, size : int) =
            memcpy_internal(target, source, UIntPtr (uint32 size)) |> ignore

        let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : int) =
            memcmp_internal(ptr1, ptr2, UIntPtr (uint32 size))

        let memset(ptr : nativeint, value : int, size : int) =
            memset_internal(ptr, value, UIntPtr (uint32 size)) |> ignore

        let memmove(target : nativeint, source : nativeint, size : int) =
            memmove_internal(target, source, UIntPtr (uint32 size)) |> ignore

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


        let memcpy(target : nativeint, source : nativeint, size : int) =
            memcpy_internal(target, source, UIntPtr (uint32 size)) |> ignore

        let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : int) =
            memcmp_internal(ptr1, ptr2, UIntPtr (uint32 size))

        let memset(ptr : nativeint, value : int, size : int) =
            memset_internal(ptr, value, UIntPtr (uint32 size)) |> ignore

        let memmove(target : nativeint, source : nativeint, size : int) =
            memmove_internal(target, source, UIntPtr (uint32 size)) |> ignore

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module NativeInt =
        let memcpy (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memcpy(dst, src, size)
                | Linux -> LibC.memcpy(dst, src, size)
                | Mac -> notimp()

        let memmove (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memmove(dst, src, size)
                | Linux -> LibC.memmove(dst, src, size)
                | Mac -> notimp()

        let memset (dst : nativeint) (value : int) (size : int) =
            match os with
                | Windows -> MSVCRT.memset(dst, value, size)
                | Linux -> LibC.memset(dst, value, size)
                | Mac -> notimp()

        let memcmp (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memcmp(dst, src, size)
                | Linux -> LibC.memcmp(dst, src, size)
                | Mac -> notimp()

        let inline read<'a when 'a : unmanaged> (ptr : nativeint) =
            NativePtr.read (NativePtr.ofNativeInt<'a> ptr) 

        let inline write<'a when 'a : unmanaged> (ptr : nativeint) (value : 'a) =
            NativePtr.write (NativePtr.ofNativeInt<'a> ptr)  value

        let inline get<'a when 'a : unmanaged> (ptr : nativeint) (index : int) =
            NativePtr.get (NativePtr.ofNativeInt<'a> ptr) index
  
        let inline set<'a when 'a : unmanaged> (ptr : nativeint) (index : int) (value : 'a)=
            NativePtr.set (NativePtr.ofNativeInt<'a> ptr) index      

    type Marshal with
        static member Copy(source : nativeint, destination : nativeint, length : int) =
            match os with
                | Windows -> MSVCRT.memcpy(destination, source, length)
                | Linux -> LibC.memcpy(destination, source, length)
                | Mac -> notimp()

        static member Move(source : nativeint, destination : nativeint, length : int) =
            match os with
                | Windows -> MSVCRT.memmove(destination, source, length)
                | Linux -> LibC.memmove(destination, source, length)
                | Mac -> notimp()

        static member Set(memory : nativeint, value : int, length : int) =
            match os with
                | Windows -> MSVCRT.memset(memory, value, length)
                | Linux -> LibC.memset(memory, value, length)
                | Mac -> notimp()

        static member Compare(source : nativeint, destination : nativeint, length : int) =
            match os with
                | Windows -> MSVCRT.memcmp(destination, source, length)
                | Linux -> LibC.memcmp(destination, source, length)
                | Mac -> notimp()

    let pinned (a : obj) f = 
        let gc = GCHandle.Alloc(a, GCHandleType.Pinned)
        try
            f ( gc.AddrOfPinnedObject() )
        finally
            gc.Free()
        