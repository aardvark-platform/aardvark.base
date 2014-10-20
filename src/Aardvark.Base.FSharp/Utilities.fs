#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif
open System
open System.Collections.Generic
open System.Collections.Concurrent

[<AutoOpen>]
module Memo = 
    open Aardvark.Base
    open System.Threading

    [<AbstractClass>]
    type AbstractMemoCache<'t>(useAttributeScop : bool) =
        let entries = ConcurrentDictionary<string * list<Weak<obj>>, 't>()
        let lock = obj()
        let dummyStack = obj()

        let getStack () =
            if useAttributeScop then Ag.getContext() :> obj
            else dummyStack

        new() = AbstractMemoCache(true)

        member private x.registerFinalize (key : string * list<Weak<obj>>) (strong : list<obj>) =
            let (_,weakList) = key
            strong.RegisterAnyFinalizer(fun () -> x.remove key)

        member private x.add k v =
            Monitor.Enter(lock)
            entries.TryAdd(k,x.Pack(v)) |> ignore
            Monitor.Exit(lock)

        member private x.remove k =
            Monitor.Enter(lock)
            entries.TryRemove(k) |> ignore
            Monitor.Exit(lock)

        

        abstract member Run : (unit -> 'a) -> 'a
        abstract member Pack : obj -> 't
        abstract member TryUnpack : 't -> Option<obj>

        member private x.TryGetCacheEntry(key) =
            match entries.TryGetValue(key) with
                | (true,v) -> x.TryUnpack(v)
                | _ -> None
                                

        member x.Memoized1 (f : 'a -> 'b) (arg0 : 'a) : 'b=
            let stack = getStack ()
            let key = (f.ToString(), [Weak<obj>(stack); Weak(arg0 :> obj)])
            
            match x.TryGetCacheEntry(key) with
                | Some(v) -> v |> unbox
                | _ -> let r = x.Run (fun () -> (f arg0))
                       x.add key r
                       let strong = [arg0 :> obj]
                       x.registerFinalize key strong

                       r

        member x.Memoized2 (f : 'a -> 'b -> 'c) (arg0 : 'a) (arg1 : 'b) : 'c =
            let stack = getStack ()
            let key = (f.ToString(), [Weak<obj>(stack); Weak(arg0 :> obj); Weak(arg1 :> obj)])
            
            match x.TryGetCacheEntry(key) with
                | Some(v) -> v |> unbox
                | _    -> let r = x.Run (fun () -> (f arg0 arg1))
                          x.add key r
                          let strong = [arg0 :> obj; arg1 :> obj]
                          x.registerFinalize key strong

                          r

        member x.Memoized3 (f : 'a -> 'b -> 'c -> 'd) (arg0 : 'a) (arg1 : 'b) (arg2 : 'c) : 'd =
            let stack = getStack ()
            let key = (f.ToString(), [Weak<obj>(stack); Weak(arg0 :> obj); Weak(arg1 :> obj); Weak(arg2 :> obj)])
            
            match x.TryGetCacheEntry(key) with
                | Some(v) -> v |> unbox
                | _    -> let r = x.Run (fun () -> (f arg0 arg1 arg2))
                          x.add key r
                          let strong = [arg0 :> obj; arg1 :> obj; arg2 :> obj]
                          x.registerFinalize key strong

                          r

    type MemoCache(useAgScope : bool) =
        inherit AbstractMemoCache<Weak<obj>>(useAgScope) with
            override x.Run(f) = f()
            override x.Pack(o) = if o = null then null else Weak o
            override x.TryUnpack(o) = if o = null then Some null else o.TargetOption
            new() = MemoCache(true)
