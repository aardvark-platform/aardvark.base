module Caches

open System
open NUnit
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open Aardvark.Base
open System.Diagnostics
open System.Threading
open System.Runtime.CompilerServices

type RuntimeObject(name : string) =

    static let mutable objCount = 0

    let data = Array.zeroCreate 100000

    let number = Interlocked.Increment &objCount

    do
        Log.line "Create %s %d" name number

    static member ObjCount = objCount

    member x.Name 
        with get() = name

    override x.Finalize() = 
        Interlocked.Decrement &objCount |> ignore
        Log.line "Collect %s %d" name number

    override x.ToString() = name

 type ResultObject<'a, 'b>(a : 'a, b : 'b) =

    static let mutable resCount = 0

    do    
        let resCnt = Interlocked.Increment &resCount
        Log.line "Create Result %d" resCnt

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
                                        Log.line "result created"
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
        
        Log.line "created: %A" result

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
                                        Log.line "result created: %s" res
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
                            
        Log.line "created: %A" result

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

        Log.line "created: %A" result

        i <- i + 1 

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

    System.GC.Collect(3)
    System.GC.WaitForFullGCComplete() |> ignore

    Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

    if RuntimeObject.ObjCount > 100 then
        failwith "leak"

[<Test>]
[<Ignore("not fully understood issue")>]
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
                    
        Log.line "created: %A" result

        i <- i + 1 

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

    System.GC.Collect(3)
    System.GC.WaitForFullGCComplete() |> ignore

    Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

    if RuntimeObject.ObjCount > 100 then
        failwith "leak"
        
[<Test>]
let ``[Caches] ConditionalWeakTable: input holding output``() =

    let sw = Stopwatch.StartNew()
    let mutable i = 0
    while sw.Elapsed.TotalSeconds < 10.0 do
        
        let temp = RuntimeObject(sprintf "run-%i" i)

        let result = UnaryCache<_, _>(fun a -> a).Invoke temp
                
        Log.line "created: %A" result

        i <- i + 1 

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

    System.GC.Collect(3)
    System.GC.WaitForFullGCComplete() |> ignore

    Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

    if RuntimeObject.ObjCount > 100 then
        failwith "leak"

[<Test>]
let ``[Caches] BinaryCache2 forward``() =
    
    let a = "hugo"

    let mutable compCount = 0
    let cache = BinaryCache2<_, _, _>((fun a b -> 
                                        let res = a.ToString() + b.ToString()
                                        compCount <- compCount + 1
                                        Log.line "result created"
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
        
        Log.line "created: %A" result

        i <- i + 1 

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

    System.GC.Collect(3)
    System.GC.WaitForFullGCComplete() |> ignore

    Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

    if RuntimeObject.ObjCount > 100 then
        failwith "leak"

[<Test>]
let ``[Caches] BinaryCache2 backward``() =
    
    let a = "hugo"

    let mutable compCount = 0
    let cache = BinaryCache2<_, _, _>((fun a b -> 
                                        let res = a.ToString() + b.ToString() 
                                        compCount <- compCount + 1
                                        Log.line "result created: %s" res
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
                            
        Log.line "created: %A" result

        i <- i + 1 

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

    System.GC.Collect(3)
    System.GC.WaitForFullGCComplete() |> ignore

    Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

    if RuntimeObject.ObjCount > 100 then
        failwith "leak"

[<Test>]
[<Ignore("issue by design -> see comment in BinaryChache2")>]
let ``[Caches] BinaryCache2 forward with ResultObject``() =
    
    let a = "hugo"

    let cache = BinaryCache2<_, _, _>((fun a b -> ResultObject(a, b)))

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

        Log.line "created: %A" result

        i <- i + 1 

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

    System.GC.Collect(3)
    System.GC.WaitForFullGCComplete() |> ignore

    Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

    if RuntimeObject.ObjCount > 100 then
        failwith "leak"

[<Test>]
[<Ignore("issue by design -> see comment in BinaryChache2")>]
let ``[Caches] BinaryCache2 backward with ResultObject``() =
    
    let a = "hugo"

    let cache = BinaryCache2<_, _, _>((fun a b -> ResultObject(a, b)))

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
                    
        Log.line "created: %A" result

        i <- i + 1 

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

    System.GC.Collect(3)
    System.GC.WaitForFullGCComplete() |> ignore

    Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

    if RuntimeObject.ObjCount > 100 then
        failwith "leak"


