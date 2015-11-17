#if INTERACTIVE
#r "..\\..\\..\\Bin\\Debug\\Aardvark.Base.dll"
#else
namespace Aardvark.Base
#endif
open System
open System.Reflection
open System.Linq.Expressions
open System.Threading
open System.Runtime.InteropServices
open System.Collections.Generic

module DependentHandle =
    module Api = 
        let flags = BindingFlags.NonPublic ||| BindingFlags.Static
        let depType = Type.GetType("System.Runtime.CompilerServices.DependentHandle") 

        //nInitialize
        let nInitializeMeth = depType.GetMethod("nInitialize", flags)
        type NInitDel = delegate of obj * obj * byref<nativeint> -> unit
        let nInitializeDel =
            let p = Expression.Parameter(typeof<obj>, "primary")
            let s = Expression.Parameter(typeof<obj>, "secondary")
            let h = Expression.Parameter(typeof<nativeint>.MakeByRefType(), "handle")
 
            let ex = Expression.Lambda<NInitDel>(Expression.Call(nInitializeMeth, p, s, h), [p;s;h])
            ex.Compile()

        let nInitialize (primary : obj, secondary : obj, handle : byref<nativeint>) =
            nInitializeDel.Invoke(primary, secondary, &handle)

        //nFree
        let nFreeMeth = depType.GetMethod("nFree", flags)
        type NFreeDel = delegate of nativeint -> unit
        let nFreeDel =
            let h = Expression.Parameter(typeof<nativeint>)

            let ex = Expression.Lambda<NFreeDel>(Expression.Call(nFreeMeth, h), [h])
            ex.Compile()

        let nFree (handle : nativeint) =
            nFreeDel.Invoke(handle)


        //nGetPrimary
        let nGetPrimaryMeth = depType.GetMethod("nGetPrimary", flags)
        type NGetPrimaryDel = delegate of nativeint * byref<obj> -> unit
        let nGetPrimaryDel =
            let h = Expression.Parameter(typeof<nativeint>)
            let p = Expression.Parameter(typeof<obj>.MakeByRefType())
            let ex = Expression.Lambda<NGetPrimaryDel>(Expression.Call(nGetPrimaryMeth, h, p), [h;p])
            ex.Compile()

        let nGetPrimary (handle : nativeint, primary : byref<obj>) =
            nGetPrimaryDel.Invoke(handle, &primary)

        //nGetPrimaryAndSecondary
        let nGetPrimaryAndSecondaryMeth = depType.GetMethod("nGetPrimaryAndSecondary", flags)
        type NGetPrimaryAndSecondaryDel = delegate of nativeint * byref<obj> * byref<obj> -> unit
        let nGetPrimaryAndSecondaryDel =
            let h = Expression.Parameter(typeof<nativeint>)
            let p = Expression.Parameter(typeof<obj>.MakeByRefType())
            let s = Expression.Parameter(typeof<obj>.MakeByRefType())
            let ex = Expression.Lambda<NGetPrimaryAndSecondaryDel>(Expression.Call(nGetPrimaryAndSecondaryMeth, h, p, s), [h;p;s])
            ex.Compile()

        let nGetPrimaryAndSecondary (handle : nativeint, primary : byref<obj>, secondary : byref<obj>) =
            nGetPrimaryAndSecondaryDel.Invoke(handle, &primary, &secondary)

    type DependentHandle<'k, 'v when 'k : not struct and 'v : not struct>(primary : 'k, secondary : 'v) =
        let mutable handle = 0n
        do Api.nInitialize(primary :> obj, secondary :> obj, &handle)

        member x.IsAllocated =
            handle <> 0n

        member x.Primary =
            if handle <> 0n then
                let mutable prim = null
                Api.nGetPrimary(handle, &prim)
                if isNull prim then 
                    None
                else
                    Some (prim |> unbox<'k>)
            else
                None

        member x.PrimaryAndSecondary =
            if handle <> 0n then
                let mutable prim = null
                let mutable sec = null
                Api.nGetPrimaryAndSecondary(handle, &prim, &sec)

                if not (isNull prim) && not (isNull sec) then
                    Some (prim |> unbox<'k>, sec |> unbox<'v>)
                else
                    None

            else
                None

        member x.Secondary =
            match x.PrimaryAndSecondary with
                | Some (_,s) -> Some s
                | None -> None

        member x.Dispose() =
            let old = Interlocked.Exchange(&handle, 0n)
            if old <> 0n then
                Api.nFree(old)

        interface IDisposable with
            member x.Dispose() = x.Dispose()


    let makeDependent (a : 'a) (b : 'b) =
        new DependentHandle<'a, 'b>(a, b)


    let test() =
        let mutable a = obj()
        let h = makeDependent a "some other string"

        printfn "%A" h.PrimaryAndSecondary
        a <- null
        System.GC.Collect()

        printfn "%A" h.PrimaryAndSecondary

        h.Dispose()

module WeakTable =
    open DependentHandle

    type private Weak<'a when 'a : not struct>(value : 'a) =
        do if isNull (value :> obj) then failwith "created null weak"
        let wr = System.WeakReference<'a>(value)
        let hash = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(value)

        member x.IsLife =
            match wr.TryGetTarget() with
                | (true, o) when not (isNull (o :> obj)) -> true
                | _ -> false

        member x.TargetOption =
            match wr.TryGetTarget() with
                | (true, t) -> Some t
                | _ -> None

        override x.GetHashCode() =
            hash

        override x.Equals o =
            match o with
                | :? Weak<'a> as o -> 
                    match x.TargetOption, o.TargetOption with
                        | Some l, Some r -> System.Object.ReferenceEquals(l,r)
                        | None, None -> true
                        | _ -> false
                | _ -> false

//    [<StructuredFormatDisplay("{AsString}")>]
//    type WeakTable<'k, 'v when 'k : not struct and 'v : not struct>() =
//        let store = Dictionary<Weak<'k>, DependentHandle<'k, 'v>>()
//        let l = obj()
//
//        let prune() =
//            printfn "prune: %d" store.Count
//            lock l (fun () ->
//                let dead = store.Keys |> Seq.filter (fun w -> not w.IsLife) |> Seq.toArray
//                for d in dead do
//                    let dep = store.[d]
//                    dep.Dispose()
//                    store.Remove d |> ignore
//            )
//
//        let mutable lastPrune = 0
//
//        let pruneIfLeakLarge() =
//            let lowerPowerOfTwo = Fun.NextPowerOfTwo(store.Count) / 2
//            if lowerPowerOfTwo > lastPrune then
//                prune()
//                lastPrune <- lowerPowerOfTwo
//
//        member x.TryGetValue(key : 'k, [<Out>] result : byref<'v>) =
//            let r =
//                lock l (fun () ->
//                    let key = Weak key
//                    match store.TryGetValue key with
//                        | (true, dep) -> 
//                            match dep.Secondary with
//                                | Some v ->
//                                    Some v
//                                | None ->
//                                    dep.Dispose()
//                                    store.Remove key |> ignore
//                                    None
//                        | _ -> None
//                )
//            match r with
//                | Some r -> 
//                    result <- r
//                    true
//                | None ->
//                    false
//
//        member x.Item 
//            with get key = match x.TryGetValue(key) with | (true,v) -> v | _ -> failwith "sdsadsa"
//
//        member x.Add(key : 'k, value : 'v) =
//            lock l (fun () ->
//                let dep = makeDependent key value
//                store.Add(Weak key, dep)
//                pruneIfLeakLarge()
//            )
//            
//
//        member x.Remove(key : 'k) =
//            lock l (fun () ->
//                let key = Weak key
//                match store.TryGetValue key with
//                    | (true, dep) -> 
//                        dep.Dispose()
//                        store.Remove(key) |> ignore
//                        true
//                    | _ ->  
//                        false
//            )     
//
//        member x.Clear() =
//            lock l (fun () ->
//                store.Values |> Seq.iter(fun d -> d.Dispose())
//                store.Clear()
//                lastPrune <- 0
//            )
//
//        member x.AsString =
//            store.Values |> Seq.choose (fun v -> v.PrimaryAndSecondary) |> Seq.toList |> sprintf "WeakTable %A"
//
//    let empty<'k, 'v when 'k : not struct and 'v : not struct> = WeakTable<'k, 'v>()
//
//    let add (key : 'k) (value : 'v) (table : WeakTable<'k, 'v>) =
//        table.Add(key, value)
//
//    let remove (key : 'k) (table : WeakTable<'k, 'v>) =
//        table.Remove(key)
//
//    let clear (table : WeakTable<'k, 'v>) =
//        table.Clear()
//
//    let find (key : 'k) (t : WeakTable<'k, 'v>) =
//        t.[key]
//
//    let tryFind (key : 'k) (t : WeakTable<'k, 'v>) =
//        match t.TryGetValue key with
//            | (true, v) -> Some v
//            | _ -> None
//
//    let test() =
//        let table = WeakTable<obj, string>()
//
//        let mutable a = obj()
//        let mutable b = obj()
//
//        table.Add(a, "a")
//        table.Add(b, "b")
//
//        printfn "%A" table
//
//        a <- null
//        System.GC.Collect()
//        printfn "%A" table
//
//        b <- if not (isNull a) then b else null
//        System.GC.Collect()
//        printfn "%A" table
//
//        let str = sprintf "%A %A" a b
//        ()
//
//    let testPruning() =
//        let t = empty
//
//        for i in 0..1000 do
//            add (obj()) (sprintf "%d" i) t
//
//        printfn "%A" t
//

