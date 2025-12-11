namespace Aardvark.Base.FSharp.Tests

open System
open NUnit.Framework
open Expecto
open Aardvark.Base
open System.IO
open System.Diagnostics
open System.Threading
open System.Reflection
open System.Collections

module Caches =
    type RuntimeObject(name : string) =

        static let mutable objCount = 0

        let data = Array.zeroCreate 100000

        let number = Interlocked.Increment &objCount

        static member ObjCount = objCount

        member x.Name
            with get() = name

        override x.Finalize() =
            Interlocked.Decrement &objCount |> ignore

        override x.ToString() = name

    type ResultObject<'a, 'b>(a : 'a, b : 'b) =

        static let mutable resCount = 0

        do Interlocked.Increment &resCount |> ignore

        static member ResCount = resCount

        member x.A
            with get() = a
        member x.B
            with get() = b

        override x.ToString() =
            a.ToString() + b.ToString()

    [<Test>]
    let ``[Caches] BinaryCache forward``() =

        let a = "hugo"

        let mutable compCount = 0
        let cache = BinaryCache<_, _, _>((fun a b ->
                                            let res = a.ToString() + b.ToString()
                                            compCount <- compCount + 1
                                            res))

        let sw = Stopwatch.StartNew()
        let mutable i = 0
        let random = Random(123)
        let mutable temp = RuntimeObject(sprintf "run-%i" i)

        while sw.Elapsed.TotalSeconds < 10.0 do

            let resCount =
                if random.NextDouble() < 0.1 || i = 0 then
                    temp <- RuntimeObject(sprintf "run-%i" i)
                    compCount + 1
                else
                    compCount

            let result = cache.Invoke(a, temp)

            if resCount <> compCount then
                failwith "caching failed"

            i <- i + 1

            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

        Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

        if RuntimeObject.ObjCount > 100 then
            failwith "leak"

    [<Test>]
    let ``[Caches] BinaryCache backward``() =

        let a = "hugo"

        let mutable compCount = 0
        let cache = BinaryCache<_, _, _>((fun a b ->
                                            let res = a.ToString() + b.ToString()
                                            compCount <- compCount + 1
                                            res))

        let sw = Stopwatch.StartNew()
        let mutable i = 0
        let random = Random(123)
        let mutable temp = RuntimeObject(sprintf "run-%i" i)

        while sw.Elapsed.TotalSeconds < 10.0 do

            let resCount =
                if random.NextDouble() < 0.1 || i = 0 then
                    temp <- RuntimeObject(sprintf "run-%i" i)
                    compCount + 1
                else
                    compCount

            let result = cache.Invoke(temp, a)

            if resCount <> compCount then
                failwith "caching failed"

            i <- i + 1

            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

        Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

        if RuntimeObject.ObjCount > 100 then
            failwith "leak"


    [<Test>]
    let ``[Caches] BinaryCache forward with ResultObject``() =

        let a = "hugo"

        let cache = BinaryCache<_, _, _>((fun a b -> ResultObject(a, b)))

        let sw = Stopwatch.StartNew()
        let mutable i = 0

        let rnd = Random(123)
        let mutable temp = Unchecked.defaultof<_>

        while sw.Elapsed.TotalSeconds < 10.0 do

            let resCount =
                if rnd.NextDouble() < 0.1 || i = 0 then
                    temp <- RuntimeObject(sprintf "run-%i" i)
                    ResultObject<string, RuntimeObject>.ResCount + 1
                else
                    ResultObject<string, RuntimeObject>.ResCount

            let result = cache.Invoke(a, temp)

            if resCount <> ResultObject<string, RuntimeObject>.ResCount then
                failwith "cache not working"

            i <- i + 1

            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

        Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

        if RuntimeObject.ObjCount > 100 then
            failwith "leak"

    [<Test>]
    let ``[Caches] BinaryCache backward with ResultObject``() =

        let a = "hugo"
        let cache = BinaryCache<_, _, _>((fun a b -> ResultObject(a, b)))

        let sw = Stopwatch.StartNew()
        let mutable i = 0

        let rnd = Random(123)
        let mutable temp = Unchecked.defaultof<_>

        while sw.Elapsed.TotalSeconds < 10.0 do

            let resCount =
                if rnd.NextDouble() < 0.1 || i = 0 then
                    temp <- RuntimeObject(sprintf "run-%i" i)
                    ResultObject<RuntimeObject, string>.ResCount + 1
                else
                    ResultObject<RuntimeObject, string>.ResCount

            let result = cache.Invoke(temp, a)

            if resCount <> ResultObject<RuntimeObject, string>.ResCount then
                failwith "cache not working"

            i <- i + 1

            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

        Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

        if RuntimeObject.ObjCount > 100 then
            failwith "leak"


        let x = cache.Invoke(temp, a)
        let y = cache.Invoke(temp, a)
        if not (System.Object.ReferenceEquals(x,y)) then failwith "not caching"

    [<Test>]
    let ``[Caches] ConditionalWeakTable: input holding output``() =

        let sw = Stopwatch.StartNew()
        let mutable i = 0
        while sw.Elapsed.TotalSeconds < 10.0 do

            let temp = RuntimeObject(sprintf "run-%i" i)

            let result = UnaryCache<_, _>(fun a -> a).Invoke temp

            i <- i + 1

            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

        Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

        if RuntimeObject.ObjCount > 100 then
            failwith "leak"

    [<Test>]
    let ``[Caches] Introspection plugin cache``() =

        // Don't want to make this type public just for a unit test, so we just use reflection?
        let plugins = typeof<Aardvark>.GetNestedType("Plugins", BindingFlags.NonPublic ||| BindingFlags.Public)
        let cacheType = plugins.GetNestedType("PluginCache", BindingFlags.NonPublic ||| BindingFlags.Public)

        let dataType = cacheType.GetNestedType("Data");
        let dataCtor = dataType.GetConstructor([| typeof<DateTime>; typeof<bool> |])

        let cacheCtor = cacheType.GetConstructor([||])
        let serialize = cacheType.GetMethod("Serialize", BindingFlags.NonPublic ||| BindingFlags.Instance, [| typeof<Stream> |])
        let deserialize = cacheType.GetMethod("Deserialize",  BindingFlags.NonPublic ||| BindingFlags.Static, [| typeof<Stream> |])
        let add = cacheType.GetMethod("Add", [| typeof<string>; dataType |])

        let addEntry (path : string) (lastModified : DateTime) (isPlugin : bool) (cache : IDictionary) =
            let data = dataCtor.Invoke([| lastModified; isPlugin |])
            add.Invoke(cache, [| path; data |]) |> ignore

        let cache = unbox<IDictionary> <| cacheCtor.Invoke([||])
        cache |> addEntry "foo.dll" DateTime.Now true
        cache |> addEntry "bar.dll" DateTime.Now false

        use stream = new MemoryStream()
        serialize.Invoke(cache, [| stream |]) |> ignore

        stream.Seek(0L, SeekOrigin.Begin) |> ignore
        let result = unbox<IDictionary> <| deserialize.Invoke(null, [| stream |])

        Expect.equal result.Count cache.Count "Unexpected entry count"

        for path in cache.Keys do
            Expect.isTrue (result.Contains path) "Result did not contain path"
            Expect.equal result.[path] cache.[path] "Cache entry mismatch"