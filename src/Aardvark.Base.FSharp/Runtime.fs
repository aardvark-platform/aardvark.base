#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif


open System
open System.Threading

[<AutoOpen>]
module Weak =
    open System
    open System.Collections.Generic
    open System.Linq
    open Aardvark.Base
    open System.Threading
    open System.Runtime.CompilerServices

    (*============================ Statistics =================================*)
    let private lastValues = Dictionary<obj, int>() 
    let mutable private lastNone = 0

    let private statistics = Dictionary<obj, int>() 
    let mutable private statisticsNone = 0

    let convertArray (f : ('a -> 'b)) (arr : Array) =
        if arr = null || arr.GetType().GetElementType() = typeof<'b> then
            arr
        else
            let result = Array.CreateInstance(typeof<'b>, arr.Length)
            for i in 0..(arr.Length-1) do
                result.SetValue(f(arr.GetValue(i) |> unbox<'a>), i)
            result

    let private registerInStatistics(data : Option<obj>) =
        match data with
            | Some(key) -> if key <> null then
                                 Monitor.Enter(statistics)
                                 match statistics.TryGetValue(key) with
                                     | (true,v) -> statistics.[key] <- v + 1
                                     | _ -> statistics.Add(key, 1)
                                 Monitor.Exit(statistics)
                            else
                                 Report.Warn("Some(null)")
            | _ -> statisticsNone <- statisticsNone + 1

    let printStatistics() =
        let mem = System.GC.GetTotalMemory(true)
        System.GC.WaitForPendingFinalizers()

        Monitor.Enter(statistics)
        Log.line "============================= Finalizer Statistics =========================="
        Log.line "    Total:"
        for kvp in statistics do
            Log.line "        %A: %A" kvp.Key kvp.Value
        Log.line "        \"NONE\": %A" statisticsNone


        Log.line "    Relative:"
        for kvp in statistics do
            let old = match lastValues.TryGetValue(kvp.Key) with
                        | (true,v) -> v
                        | _ -> 0
            Log.line "        %A: +%A" kvp.Key (kvp.Value - old)
        Log.line "        \"NONE\": +%A" (statisticsNone - lastNone)

        Log.line "    Total used Memory: %dMB" (mem >>> 20)

        Log.line "============================================================================="

        statistics |> Seq.iter (fun kvp -> lastValues.[kvp.Key] <- kvp.Value)
        lastNone <- statisticsNone

        Monitor.Exit(statistics)



    (*============================ Registrations ==============================*)

    type private Finalizable() =
        let mutable finalizers = List<(obj -> unit) * Option<obj>>()

        let finalizableFinalize(f,d) =
            registerInStatistics(d)
            match d with
                | None -> f(null)
                | Some(v) -> f(v)

        member x.Add(f : obj -> unit, data : Option<obj>) =
            finalizers.Add((f,data))

        override x.Finalize() =
            try
                finalizers |> Seq.iter finalizableFinalize
                finalizers.Clear()
                finalizers <- null
            with e -> Log.warn "Finalizable threw an exception: %A" e

    let private registrations = ConditionalWeakTable<obj, Finalizable>()

    let registerFinalizer<'a> (obj : 'a, finalizer : obj -> unit, data : Option<obj>) =
        match registrations.TryGetValue(obj) with
            | (true, v) -> v.Add(finalizer, data)
            | _ -> let v = Finalizable()
                   registrations.Add(obj,v)
                   v.Add(finalizer, data)


    (*============================ Weak Types =================================*)
    [<AllowNullLiteral>]
    type Weak<'a when 'a : not struct>(obj : 'a) =
        do if obj :> obj = null then failwith "created null weak"
        let m_weak = System.WeakReference<'a>(obj)
        let m_hashCode = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj)

        member private x.WeakReference = m_weak

        member x.IsLife = match m_weak.TryGetTarget() with
                            | (true,o) when o :> obj <> null -> true
                            | _ -> false

        member x.Target = match m_weak.TryGetTarget() with
                            | (true,v) when v :> obj <> null -> v
                            | _ -> failwith "Weak is no longer accessible"

        member x.TryGetTarget([<System.Runtime.InteropServices.OutAttribute>] a : byref<'a>) = 
            m_weak.TryGetTarget(&a) && a :> obj <> null

        member x.TargetOption = 
            match m_weak.TryGetTarget() with
                | (true, v) -> Some v
                | _ -> None


        member x.RegisterFinalizer(f : obj -> unit, ?data) =
            match m_weak.TryGetTarget() with
                | (true, v) -> registerFinalizer(v, f, data)
                | _ -> f()

        member x.RegisterFinalizer(f : unit -> unit) =
            match m_weak.TryGetTarget() with
                | (true, v) -> registerFinalizer(v, (fun _ -> f()), None)
                | _ -> f()


        override x.GetHashCode() = m_hashCode
        override x.Equals(o : obj) =
            if System.Object.ReferenceEquals(o, x) then
                true
            else
                match o with
                    | :? Weak<'a> as other -> match (m_weak.TryGetTarget(), other.WeakReference.TryGetTarget()) with
                                                | ((true, l), (true, r)) -> System.Object.ReferenceEquals(l, r)
                                                | ((false,_), (false,_)) -> true
                                                | _ -> false 
                    | _ -> false
 
    let (|Strong|_|) (weak : Weak<'a>) = 
        match weak.TryGetTarget() with
            | (true,v) -> Some(v)
            | _ -> None
        

    
    let registerWeakFinalizer (w : Weak<obj>, finalizer : obj -> unit, data : Option<obj>) =
        match w with
            | Strong(v) -> registerFinalizer(v, finalizer, data)
            | _ -> match data with
                    | Some(d) -> finalizer(d)
                    | _ -> finalizer(null)


    type System.Object with
        member x.RegisterFinalizer(f : obj -> unit, ?data) =
            registerFinalizer(x, f, data)

        member x.RegisterFinalizer(f : unit -> unit) =
            registerFinalizer(x, (fun _ -> f()), None)

    type System.Collections.Generic.IEnumerable<'a> with
        member x.RegisterAnyFinalizer(f : obj -> unit, ?data) =
            let fRef = ref <| Some(f)
            let finalize(_) =
                match !fRef with
                    | Some(f) -> match data with
                                    | Some(v) -> f(v)
                                    | _ -> f(null)

                                 registerInStatistics(data)
                                 fRef := None
                    | None -> ()

            x |> Seq.iter (fun o -> registerFinalizer(o, finalize, None))

        member x.RegisterAllFinalizer(f : obj -> unit, ?data) =
            let countRef = ref (x.Count())
            let finalize(_) =
                if !countRef = 1 then
                    match data with
                        | Some(v) -> f(v)
                        | _ -> f(null)
                    registerInStatistics(data)
                else 
                    countRef := !countRef - 1

            x |> Seq.iter (fun o -> registerFinalizer(o, finalize, None))

        member x.RegisterAnyFinalizer(f : unit -> unit) =
            x.RegisterAnyFinalizer(fun (o:obj) -> f())

        member x.RegisterAllFinalizer(f : unit -> unit, ?data) =
            x.RegisterAllFinalizer(fun (o:obj) -> f())


    type WeakList<'a when 'a : not struct and 'a : equality>(l : list<Weak<'a>>) =
        let m_elements = l
        let m_isLife = ref true
        let m_hashcode = l |> List.fold (fun c e -> c ^^^ e.GetHashCode()) 0


        new(l : list<'a>) = WeakList<'a>(l |> List.map (fun e -> Weak(e)))

        member private x.Target = m_elements
        
        member x.IsLife = m_elements |> List.fold (fun l e -> l && e.IsLife) true

        member x.Elements = m_elements |> List.map (fun w -> w.Target)
        

        member x.RegisterAnyFinalizer(f : obj -> unit, ?data) =
            let fRef = ref (Some(f))
            let finalizer(_) =
                match !fRef with
                    | Some(f) -> match data with
                                    | Some(v) -> f(v)
                                    | _ -> f(null)

                                 registerInStatistics(data)
                                 fRef := None
                    | _ -> ()

            m_elements |> List.iter (fun e -> match e.TryGetTarget() with
                                                    | (true,v) -> if (!fRef).IsSome then 
                                                                    registerFinalizer(v, finalizer, None)            
                                                    | _ -> finalizer())

        member x.RegisterAllFinalizer(f : obj -> unit, ?data) =
            let countRef = ref (m_elements.Length)
            let finalizer(_) =
                if !countRef = 1 then
                    registerInStatistics(data)
                    match data with
                        | Some(v) -> f(v)
                        | _ -> f(null)
                else
                    countRef := !countRef - 1

            m_elements |> List.iter (fun e -> match e.TryGetTarget() with
                                                    | (true,v) -> registerFinalizer(v, finalizer, None)
                                                    | _ -> finalizer())

        member x.RegisterAnyFinalizer(f : unit -> unit) =
            x.RegisterAnyFinalizer(fun (o:obj) -> f())

        member x.RegisterAllFinalizer(f : unit -> unit) =
            x.RegisterAllFinalizer(fun (o:obj) -> f())




        override x.GetHashCode() = m_hashcode
        override x.Equals(o) =
            if System.Object.ReferenceEquals(x,o) then
                true
            else
                match o with
                    | :? WeakList<'a> as other -> if other.Target.Length <> m_elements.Length then
                                                        false
                                                   else
                                                        List.fold2 (fun b l r -> b && (l.Equals(r))) true m_elements other.Target
                    | _ -> false 

[<System.Runtime.CompilerServices.Extension>]
module ReflectionHelpers =
    open Microsoft.FSharp.Reflection

    let private lockObj = obj()

    let private prettyNames =
        Dict.ofList [
            typeof<sbyte>, "sbyte"
            typeof<byte>, "byte"
            typeof<int16>, "int16"
            typeof<uint16>, "uint16"
            typeof<int>, "int"
            typeof<uint32>, "uint32"
            typeof<int64>, "int64"
            typeof<uint64>, "uint64"
            typeof<nativeint>, "nativeint"
            typeof<unativeint>, "unativeint"

            typeof<char>, "char"
            typeof<string>, "string"


            typeof<float32>, "float32"
            typeof<float>, "float"
            typeof<decimal>, "decimal"

            typeof<obj>, "obj"
            typeof<unit>, "unit"
            typeof<System.Void>, "void"

        ]

    let private genericPrettyNames =
        Dict.ofList [
            typedefof<list<_>>, "list"
            typedefof<Option<_>>, "Option"
            typedefof<Set<_>>, "Set"
            typedefof<Map<_,_>>, "Map"
            typedefof<seq<_>>, "seq"

        ]

    let private idRx = System.Text.RegularExpressions.Regex @"[a-zA-Z_][a-zA-Z_0-9]*"

    let rec private getPrettyNameInternal (t : Type) =
        let res = 
            match prettyNames.TryGetValue t with
                | (true, n) -> n
                | _ ->
                    if t.IsArray then
                        t.GetElementType() |> getPrettyNameInternal |> sprintf "%s[]"

                    elif FSharpType.IsTuple t then
                        FSharpType.GetTupleElements t |> Seq.map getPrettyNameInternal |> String.concat " * "

                    elif FSharpType.IsFunction t then
                        let (arg, res) = FSharpType.GetFunctionElements t

                        sprintf "%s -> %s" (getPrettyNameInternal arg) (getPrettyNameInternal res)

                    elif typeof<Aardvark.Base.INatural>.IsAssignableFrom t then
                        let s = Aardvark.Base.Peano.getSize t
                        sprintf "N%d" s

                    elif t.IsGenericType then
                        let args = t.GetGenericArguments() |> Seq.map getPrettyNameInternal |> String.concat ", "
                        let bt = t.GetGenericTypeDefinition()
                        match genericPrettyNames.TryGetValue bt with
                            | (true, gen) ->
                                sprintf "%s<%s>" gen args
                            | _ ->
                                let gen = idRx.Match bt.Name
                                sprintf "%s<%s>" gen.Value args


                    else
                        t.Name

        prettyNames.[t] <- res
        res

    [<System.Runtime.CompilerServices.Extension; CompiledName("GetPrettyName")>]
    let getPrettyName(t : Type) =
        lock lockObj (fun () ->
            getPrettyNameInternal t
        )

    type Type with
        member x.PrettyName =
            lock lockObj (fun () ->
                getPrettyNameInternal x
            )


[<AutoOpen>]
module Unique =
    type private Id = Id of int
    type Unique<'a when 'a : not struct>(value : 'a) =
        static let mutable currentId = 0
        static let table = System.Runtime.CompilerServices.ConditionalWeakTable<'a, Id>()
        static let getId (value : 'a) =
            match table.TryGetValue value with
                | (true,Id v) -> v
                | _ -> let v = Interlocked.Increment(&currentId)
                       table.Add(value, Id v)
                       v

        let id = getId value
        member x.Id = id
        member x.Value = value

        override x.Equals o =
            match o with
                | :? Unique<'a> as o -> x.Id = o.Id
                | _ -> false

        override x.GetHashCode() =
            x.Id.GetHashCode()

        interface IComparable with
            member x.CompareTo o =
                match o with
                    | :? Unique<'a> as o -> x.Id.CompareTo(o.Id)
                    | _ -> failwith ""

    let unique (x : 'a) : Unique<'a> = Unique(x)

//something with stackalloc
#nowarn "9"
module RuntimeExtensions =
    open Microsoft.FSharp.NativeInterop

    let getStackAddress() = 
        let ptr : nativeptr<byte> = NativePtr.stackalloc(1) 
        ptr |> NativePtr.toNativeInt |> int64

    let stackBasePtr = getStackAddress()
    let getStackSize () = 
       Aardvark.Base.Fun.Abs(stackBasePtr - getStackAddress())


    open System
    open System.Threading
    open System.Threading.Tasks

    let concurrentAcc = ref 0
    let locks = new System.Runtime.CompilerServices.ConditionalWeakTable<obj,ref<int>>()    
    let sl = new SpinLock()

    let inline spinLocked (obj : SpinLock) (f : unit -> 'b) : 'b =
            let locked = ref false
            let mutable result = Unchecked.defaultof<'b>
            try
                obj.Enter locked
                result <- f ()
            finally
                if !locked then
                    obj.Exit()
            result

    let checkConcurrency lockObj : IDisposable =
        let mkDisposable r = { new IDisposable with member x.Dispose() = spinLocked sl (fun () -> r := !r - 1) }
        spinLocked sl (fun () ->
            match locks.TryGetValue lockObj with
                | (true,r) -> r := !r + 1
                              concurrentAcc := System.Math.Max(!r,!concurrentAcc)
                              mkDisposable r
                | _ -> let r = ref 0
                       locks.Add (lockObj, ref 0)
                       mkDisposable r
        )


    let inline getSourceLocation() = __SOURCE_FILE__ + __LINE__

module Arr =
    type ArrayWriter<'a> = int * ('a[] -> int -> unit)
    type ArrayBuilder() =
        member x.YieldFrom(e : 'a[]) : ArrayWriter<'a> =
            (e.Length, fun arr i -> e.CopyTo(arr, i))

        member x.Yield(e : 'a) : ArrayWriter<'a> =
            (1, fun arr i -> arr.[i] <- e)

        member x.Combine((lc,l) : ArrayWriter<'a>, (rc,r) : ArrayWriter<'a>) =
            (lc + rc, fun arr i -> l arr i
                                   r arr (i + lc))

        member x.Zero() = (0, fun arr i -> ())

        member x.Delay(f : unit -> ArrayWriter<'a>) = f()

        member x.For(s : seq<'a>, f : 'a -> ArrayWriter<'b>) : ArrayWriter<'b> =
            let mapped = s |> Seq.map f |> Seq.toArray
            let count = mapped |> Array.sumBy fst

            (count, fun arr i -> 
                let mutable c = i
                for (ci,ei) in mapped do
                    ei arr c
                    c <- c + ci)

    let arr = ArrayBuilder()

    let padToLength (l : int) (f : int -> ArrayWriter<'a>) (b : ArrayWriter<'a>) : ArrayWriter<'a> =
        let (bc,b) = b
        (l, fun arr i -> 
            b arr i; 
            let (fc,f) = f (l - bc)
            f arr (i + bc)
            )

    let padToLengthValue (l : int) (v : 'a) (b : ArrayWriter<'a>) : ArrayWriter<'a> =
        padToLength l (fun m -> arr { yield! Array.create m v }) b

    let padToLengthZero (l : int) (b : ArrayWriter<'a>) : ArrayWriter<'a> =
        padToLength l (fun m -> arr { yield! Array.create m (Unchecked.defaultof<'a>) }) b

    
    let create ((count,writer) : ArrayWriter<'a>) =
        let arr = Array.zeroCreate count
        writer arr 0
        arr

    let update (data : 'a[]) (start : int) ((count, writer) : ArrayWriter<'a>) =
        if start + count > data.Length then
            raise <| IndexOutOfRangeException()
        else
            writer data start 

    let updatePtr (data : IntPtr) (m : ArrayWriter<'a>) =
        let arr = create m
        arr.UnsafeCoercedApply<byte>(fun byteArr ->
            System.Runtime.InteropServices.Marshal.Copy(byteArr, 0, data, byteArr.Length)
        )

[<AutoOpen>]
module MarshalDelegateExtensions =
    open System.Runtime.InteropServices
    open System.Collections.Concurrent

    let private pinnedDelegates = ConcurrentDictionary<Delegate, nativeint>()
    type PinnedDelegate internal(d : Delegate, ptr : nativeint) =
        member x.Pointer = ptr
        member x.Dispose() = pinnedDelegates.TryRemove d |> ignore

        interface IDisposable with
            member x.Dispose() = x.Dispose()

    type Marshal with
        static member PinDelegate(d : Delegate) =
            let ptr = pinnedDelegates.GetOrAdd(d, fun _ -> Marshal.GetFunctionPointerForDelegate d)
            new PinnedDelegate(d, ptr)

        static member PinFunction(f : 'a -> 'b) =
            Marshal.PinDelegate(Func<'a, 'b>(f))

        static member PinFunction(f : 'a -> 'b -> 'c) =
            Marshal.PinDelegate(Func<'a, 'b, 'c>(f))

        static member PinFunction(f : 'a -> 'b -> 'c -> 'd) =
            Marshal.PinDelegate(Func<'a, 'b, 'c, 'd>(f))
