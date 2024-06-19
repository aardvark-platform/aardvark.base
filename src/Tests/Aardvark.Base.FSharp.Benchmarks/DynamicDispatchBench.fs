namespace Aardvark.Base.FSharp.Benchmarks

open System
open System.Runtime.CompilerServices
open Aardvark.Base
open BenchmarkDotNet.Attributes

// Benchmarks for dynamic dispatch pattern (e.g. creating a PixImage<T> from a given System.Type)
//
// Takeaways about dynamic invocation and delegates:
//      - MakeGenericMethod() is expensive
//      - Creating a delegate is also pretty expensive
//      - Dynamic invocation (MethodInfo.Invoke()) is expensive
//      - Calling a delegate is less expensive than dynamic invocation
//
// Takeaways about thread-safe caching:
//      - ConcurrentDictionary seems to incur less overhead than ThreadLocal or a Dictionary with a simple lock for some reason
//      - ConcurrentDictionary.TryGetValue() and manual insert is faster than ConcurrentDictionary.GetOrAdd()
//      - try-finally pattern with Monitor.Enter() and Monitor.Exit() is faster than lock()
//
// BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4529/22H2/2022Update)
// AMD Ryzen 5 5600X, 1 CPU, 12 logical and 6 physical cores
// .NET SDK 8.0.300
//   [Host] : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2 DEBUG

// Job=ShortRun  Toolchain=InProcessEmitToolchain  IterationCount=3
// LaunchCount=1  WarmupCount=3

// | Method                                    | Mean       | Error      | StdDev    | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
// |------------------------------------------ |-----------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
// | Direct                                    |   3.864 ns |  0.2973 ns | 0.0163 ns |   1.00 |    0.00 | 0.0014 |      24 B |        1.00 |
// | Dispatch (uncached)                       | 245.382 ns |  4.7115 ns | 0.2583 ns |  63.51 |    0.23 | 0.0153 |     256 B |       10.67 |
// | Dispatch (cached)                         |  43.992 ns |  2.6162 ns | 0.1434 ns |  11.39 |    0.08 | 0.0033 |      56 B |        2.33 |
// | Dispatch (uncached delegate)              | 550.627 ns |  7.2393 ns | 0.3968 ns | 142.51 |    0.70 | 0.0172 |     288 B |       12.00 |
// | Dispatch (cached delegate)                |  16.621 ns |  1.2594 ns | 0.0690 ns |   4.30 |    0.02 | 0.0014 |      24 B |        1.00 |
// | Dispatch (cached thread-local delegate)   |  29.383 ns |  1.0146 ns | 0.0556 ns |   7.60 |    0.02 | 0.0052 |      88 B |        3.67 |
// | Dispatch (cached locked delegate)         |  25.263 ns |  3.4076 ns | 0.1868 ns |   6.54 |    0.07 | 0.0052 |      88 B |        3.67 |
// | Dispatch (predefined delegate)            |  11.809 ns | 52.8603 ns | 2.8975 ns |   3.06 |    0.76 | 0.0014 |      24 B |        1.00 |


type IArray =
    abstract member Data: Array

type MyArray<'T>(data: 'T[]) =
    member x.Data = data
    interface IArray with
        member x.Data = data :> Array

module MyArray =
    open System.Threading
    open System.Reflection
    open System.Collections.Concurrent
    open System.Collections.Generic

    [<AbstractClass; Sealed>]
    type Dispatcher() =
        static member Create<'T>(data: Array) : IArray = MyArray<'T>(unbox<'T[]> data)

    type CreateDelegate = delegate of Array -> IArray

    let private createMethod = typeof<Dispatcher>.GetMethod(nameof Dispatcher.Create, BindingFlags.Public ||| BindingFlags.Static)
    let private createMethods = ConcurrentDictionary<Type, MethodInfo>()
    let private createDelegates = ConcurrentDictionary<Type, CreateDelegate>()
    let private createDelegatesThreadLocal = new ThreadLocal<Dictionary<Type, CreateDelegate>>((fun _ -> Dictionary()), false)
    let private createDelegatesLocked = Dictionary<Type, CreateDelegate>()

    let private makeDelegate t =
        let mi = createMethod.MakeGenericMethod [| t |]
        unbox<CreateDelegate> <| Delegate.CreateDelegate(typeof<CreateDelegate>, null, mi)

    let private createFloatDelegate =
        makeDelegate typeof<float>

    [<NoCompilerInlining; MethodImpl(MethodImplOptions.NoInlining)>]
    let create (data: 'T[]) : MyArray<'T> =
        MyArray<'T>(data)

    let createUntypedUncached (data: Array) : IArray =
        let mi = createMethod.MakeGenericMethod(data.GetType().GetElementType())
        mi.Invoke(null, [| data |]) |> unbox<IArray>

    let createUntypedCached (data: Array) : IArray =
        let t = data.GetType().GetElementType()

        let mi =
            match createMethods.TryGetValue t with
            | (true, mi) -> mi
            | _ ->
                let mi = createMethod.MakeGenericMethod t
                createMethods.[t] <- mi
                mi

        mi.Invoke(null, [| data |]) |> unbox<IArray>

    let createUntypedUncachedDelegate (data: Array) : IArray =
        let del = makeDelegate <| data.GetType().GetElementType()
        del.Invoke(data)

    let createUntypedCachedDelegate (data: Array) : IArray =
        let t = data.GetType().GetElementType()

        let del =
            match createDelegates.TryGetValue t with
            | (true, del) -> del
            | _ ->
                let del = makeDelegate t
                createDelegates.[t] <- del
                del

        del.Invoke(data)

    let createUntypedCachedThreadLocalDelegate (data: Array) : IArray =
        let del = createDelegatesThreadLocal.Value.GetCreate(data.GetType().GetElementType(), makeDelegate)
        del.Invoke(data)

    let createUntypedCachedLockedDelegate (data: Array) : IArray =
        let t = data.GetType().GetElementType()

        let del =
            let mutable taken = false
            try
                Monitor.Enter(createDelegatesLocked, &taken)

                match createDelegatesLocked.TryGetValue t with
                | (true, del) -> del
                | _ ->
                    let del = makeDelegate t
                    createDelegatesLocked.[t] <- del
                    del
            finally
                if taken then Monitor.Exit createDelegatesLocked

        del.Invoke(data)

    let createUntypedPredefinedDelegate (data: Array) : IArray =
        createFloatDelegate.Invoke(data)

[<MemoryDiagnoser>]
type Dispatch() =

    [<DefaultValue>]
    val mutable Data : float[]

    [<GlobalSetup>]
    member x.Setup() =
        let rnd = RandomSystem 0
        let arr = MyArray.create (Array.init 512 (ignore >> rnd.UniformDouble))
        x.Data <- arr.Data

    [<Benchmark(Description = "Direct", Baseline = true)>]
    member x.Direct() : IArray =
        MyArray.create x.Data

    [<Benchmark(Description = "Dispatch (uncached)")>]
    member x.DispatchUncached() : IArray =
        MyArray.createUntypedUncached x.Data

    [<Benchmark(Description = "Dispatch (cached)")>]
    member x.DispatchCached() : IArray =
        MyArray.createUntypedCached x.Data

    [<Benchmark(Description = "Dispatch (uncached delegate)")>]
    member x.DispatchUncachedDelegate() : IArray =
        MyArray.createUntypedUncachedDelegate x.Data

    [<Benchmark(Description = "Dispatch (cached delegate)")>]
    member x.DispatchCachedDelegate() : IArray =
        MyArray.createUntypedCachedDelegate x.Data

    [<Benchmark(Description = "Dispatch (cached thread-local delegate)")>]
    member x.DispatchCachedThreadLocalDelegate() : IArray =
        MyArray.createUntypedCachedThreadLocalDelegate x.Data

    [<Benchmark(Description = "Dispatch (cached locked delegate)")>]
    member x.DispatchCachedLockedDelegate() : IArray =
        MyArray.createUntypedCachedLockedDelegate x.Data

    [<Benchmark(Description = "Dispatch (predefined delegate)")>]
    member x.DispatchPredefinedDelegate() : IArray =
        MyArray.createUntypedPredefinedDelegate x.Data