#I @"..\..\..\bin\Release"

#r "Aardvark.Base.dll"
#r "Aardvark.Base.Essentials.dll"
#r "Aardvark.Base.TypeProviders.dll"
#r "Aardvark.Base.FSharp.dll"
#r "Aardvark.Base.Incremental.dll"

open System
open System.Threading
open System.Linq
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental

[<AutoOpen>]
module private FutureUtils =
    let noDisposable = { new IDisposable with member x.Dispose() = () }



/// defines a future-value
type Future<'a> =
    abstract member OnReady : readyCallback : ('a -> unit) -> IDisposable

type FutureSubject<'a>() =
    let mutable value = None
    let callbacks = HashSet<'a -> unit>()

    let add cb =
        lock callbacks (fun () -> callbacks.Add cb)

    let remove cb =
        lock callbacks (fun () -> callbacks.Remove cb)


    member x.Set(v : 'a) =
        value <- Some v
        let all = lock callbacks (fun () -> let arr = callbacks.ToArray() in callbacks.Clear(); arr)
        for a in all do a v

    member x.OnReady(cb : 'a -> unit) =
        match value with
            | Some v -> 
                cb v
                noDisposable
            | None -> 
                if add cb then
                    { new IDisposable with member x.Dispose() = remove cb |> ignore }
                else
                    noDisposable

    interface Future<'a> with
        member x.OnReady cb = x.OnReady cb

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Future =


    let subject() : FutureSubject<'a> = FutureSubject()

    let custom (create : ('a -> unit) -> IDisposable) =
        { new Future<'a> with
            member x.OnReady(cb) = create cb
        }  

    let ofValue (v : 'a) =
        custom (fun cb ->
            cb v
            noDisposable
        )

    let map (f : 'a -> 'b) (m : Future<'a>) =
        custom (fun cb ->
            m.OnReady(fun v ->
                v |> f |> cb
            )
        )

    let bind (f : 'a -> Future<'b>) (m : Future<'a>) =
        custom (fun cb ->
            let inner = ref noDisposable
            let outer =
                m.OnReady(fun a ->
                    inner := (f a).OnReady cb
                )

            { new IDisposable with member x.Dispose() = inner.Value.Dispose(); outer.Dispose() }
        )

    let delay (f : unit -> Future<'a>) =
        custom (fun cb ->
            f().OnReady cb
        )

    let never<'a> : Future<'a> =
        custom (fun _ -> noDisposable)

    type private FutureMod<'a>(m : IMod<'a>) =
        inherit Mod.AbstractMod<'a>()

        let mutable finished = 0
        let subject = FutureSubject()

        override x.Compute() =
            m.GetValue x

        override x.Mark() =
            let wasFinished = Interlocked.Exchange(&finished, 1)
            if wasFinished = 0 then
                try
                    let v = x.GetValue null 
                    subject.Set v
                with :? LevelChangedException ->
                    finished <- wasFinished

            false

        interface Future<'a> with
            member x.OnReady cb = subject.OnReady cb

    let next (m : IMod<'a>) =
        FutureMod(m) :> Future<'a>

    let either (l : Future<'a>) (r : Future<'b>) =
        custom (fun cb ->
            let emitted = ref 0
            let leftSub = ref noDisposable
            let rightSub = ref noDisposable

            leftSub :=
                l.OnReady(fun l -> 
                    if Interlocked.Increment &emitted.contents = 1 then
                        cb (Left l)
                        rightSub.Value.Dispose()
                    else
                        leftSub.Value.Dispose()
                )

            rightSub :=
                r.OnReady(fun r -> 
                    if Interlocked.Increment &emitted.contents = 1 then
                        cb (Right r)
                        leftSub.Value.Dispose()
                    else
                        rightSub.Value.Dispose()
                )


            { new IDisposable with member x.Dispose() = leftSub.Value.Dispose(); rightSub.Value.Dispose() }

        )

[<AutoOpen>]
module ``Future Builder`` =
    
    type FutureBuilder() =
        member x.Bind(m : Future<'a>, f : 'a -> Future<'b>) =
            m |> Future.bind f

        member x.Return(v) =
            Future.ofValue v

        member x.ReturnFrom (f : Future<'a>) =
            f

        member x.Delay (f : unit -> Future<'a>) =
            Future.delay f

    let future = FutureBuilder()



/// define a future-list
type FutureList<'s, 'a> = { run : 's -> Future<'s * FutureListType<'s, 'a>> }

and FutureListType<'s, 'a> = 
    | Nil
    | Error of Exception
    | Cons of 'a * FutureList<'s, 'a>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FutureList =
    
    type private EmptyImpl<'s, 'a>() =
        static let instance : FutureList<'s, 'a> = { run = fun s -> Future.ofValue(s, Nil) }
        static member Instance = instance

    let empty<'s, 'a> = EmptyImpl<'s, 'a>.Instance

    let single (value : 'a) : FutureList<'s, 'a> =
        { run = fun s -> Future.ofValue(s, Cons(value, empty)) }

    let rec ofList (values : list<'a>) : FutureList<'s, 'a> =
        match values with
            | [] -> empty
            | v::vs ->
                { run = fun s -> Future.ofValue(s, Cons(v, ofList vs)) }

    let inline ofSeq (values : seq<'a>) =
        values |> Seq.toList |> ofList

    let inline ofArray (arr : 'a[]) =
        arr |> Array.toList |> ofList


    let rec map (f : 'a -> 'b) (l : FutureList<'s, 'a>) =
        { run = fun s ->
            future {
                let! (s,l) = l.run s
                match l with
                    | Nil -> return s, Nil
                    | Error e -> return s, Error e
                    | Cons(v, rest) ->
                        return s, Cons(f v, map f rest)
            }
        }

    let rec append (l : FutureList<'s, 'a>) (r : FutureList<'s, 'a>) =
        { run = fun s ->
            future {
                let! (s, l) = l.run s
                match l with
                    | Error e -> 
                        return s, Error e
                    | Nil -> 
                        return! r.run s
                    | Cons(v, rest) ->
                        return s, Cons(v, append rest r)
            }
        }

    let rec collect (f : 'a -> FutureList<'s, 'b>) (m : FutureList<'s, 'a>) =
        { run = fun s ->
            future {
                let! (s,v) = m.run s
                match v with
                    | Error e -> return s, Error e
                    | Nil -> return s, Nil
                    | Cons(v, rest) ->
                        return! (append (f v) (collect f rest)).run s
            }
        }

    let inline concat (l : FutureList<'s, FutureList<'s, 'a>>) =
        l |> collect id

    let inline concat' (l : seq<FutureList<'s, 'a>>) =
        l |> Seq.fold append empty

    let rec choose (f : 'a -> Option<'b>) (m : FutureList<'s, 'a>) =
        { run = fun s ->
            future {
                let! (s,v) = m.run s
                match v with
                    | Error e -> return s, Error e
                    | Nil -> return s, Nil
                    | Cons(v, rest) ->
                        match f v with  
                            | Some v -> return s, Cons(v, choose f rest)
                            | None -> return! (choose f rest).run s
            }
        }

    let rec filter (f : 'a -> bool) (m : FutureList<'s, 'a>) =
        { run = fun s ->
            future {
                let! (s,v) = m.run s
                match v with
                    | Error e -> return s, Error e
                    | Nil -> return s, Nil
                    | Cons(v, rest) ->
                        if f v then  
                            return s, Cons(v, filter f rest)
                        else
                            return! (filter f rest).run s
            }
        }

    let rec skip (n : int) (m : FutureList<'s, 'a>) =
        if n = 0 then 
            m
        else
            { run = fun s ->
                future {
                    let! (s, c) = m.run s
                    match c with
                        | Nil -> return s, Nil
                        | Error e -> return s, Error e
                        | Cons(_,cont) -> return! (skip (n-1) cont).run s
                }
            }

    let ofObservable (o : IObservable<'a>) : FutureList<'s, 'a> =
        let first = Future.subject()
        let mutable current = first

        let subscription = ref noDisposable
        subscription :=
            o.Subscribe {
                new IObserver<'a> with
                    member x.OnNext(v) =
                        let next = Future.subject()
                        current.Set(Cons(v, { run = fun s -> next |> Future.map (fun v -> s, v) }))
                        current <- next
                    member x.OnError e =
                        subscription.Value.Dispose()
                        current.Set (Error e)
                    member x.OnCompleted() =
                        subscription.Value.Dispose()
                        current.Set Nil
            }

        { run = fun s -> first |> Future.map (fun v -> s, v)}

    let ofMod (m : IMod<'a>) : FutureList<'s, 'a> =
        m |> Mod.toObservable |> ofObservable |> skip 1

    let rec intersperse (l : FutureList<'s, 'a>) (r : FutureList<'s, 'b>) =
        { run = fun s ->
            future {
                let lf = l.run s
                let rf = r.run s

                let! v = Future.either lf rf 

                match v with
                    | Left (s,l) ->
                        match l with
                            | Nil -> return! (map Right r).run s
                            | Cons(v,rest) -> return s, Cons(Left v, intersperse rest r)
                            | Error e -> return s, Error e
                    | Right (s,r) ->
                        match r with
                            | Nil -> return! (map Left l).run s
                            | Cons(v,rest) -> return s, Cons(Right v, intersperse l rest)
                            | Error e -> return s, Error e

            }
//
//            Future.custom (fun cb ->
//                let doneCount = ref 0
//                let ls = ref noDisposable
//                let rs = ref noDisposable
//                let unsub() = ls.Value.Dispose(); rs.Value.Dispose()
//
//                ls := 
//                    lf.OnReady (fun (s, v) -> 
//                        match v with
//                            | Error e ->
//                                unsub()
//                                cb (s,Error e)
//                            | Nil ->
//                                ls.Value.Dispose()
//                                let dc = Interlocked.Increment(&doneCount.contents)
//                                if dc = 2 then cb(s,Nil)
//                            | Cons(v, rest) ->
//                                unsub()
//                                cb (s, Cons(Left v, intersperse rest r))
//                    )
//                
//                rs := 
//                    rf.OnReady (fun (s, v) -> 
//                        match v with
//                            | Error e ->
//                                unsub()
//                                cb (s,Error e)
//                            | Nil ->
//                                rs.Value.Dispose()
//                                let dc = Interlocked.Increment(&doneCount.contents)
//                                if dc = 2 then cb(s,Nil)
//                            | Cons(v, rest) ->
//                                unsub()
//                                cb (s, Cons(Right v, intersperse l rest))
//                    )
//                { new IDisposable with member x.Dispose() = unsub() }
//            )
        }

    let modifyState (f : 's -> 's) =
        { run = fun s ->
            Future.ofValue (f s, Cons((), empty))
        }

    let modifyState' (f : 's -> 's * 'a) =
        { run = fun s ->
            let s, r = f s
            Future.ofValue (s, Cons(r, empty))
        }


    let getState<'s> : FutureList<'s, 's> =
        { run = fun s ->
            Future.ofValue(s, Cons(s, empty))
        }

    let putState (newState : 's) : FutureList<'s, unit> =
        { run = fun _ ->
            Future.ofValue(newState, Cons((), empty))
        }


    let rec subscribe (initial : 's) (cb : 's * 'a -> unit) (l : FutureList<'s, 'a>) =
        let future = l.run initial
        let inner = ref noDisposable
        let outer = ref noDisposable

        outer := 
            future.OnReady(fun (s, v) ->
                match v with
                    | Nil -> outer.Value.Dispose()
                    | Error e -> outer.Value.Dispose()
                    | Cons(v, rest) ->
                        cb(s, v)
                        inner := subscribe s cb rest
            )

        { new IDisposable with member x.Dispose() = inner.Value.Dispose(); outer.Value.Dispose() }


    let rec private subscribeObs (initial : 's) (cb : IObserver<'a>) (l : FutureList<'s, 'a>) =
        let future = l.run initial
        let inner = ref noDisposable
        let outer = ref noDisposable

        outer := 
            future.OnReady(fun (s, v) ->
                match v with
                    | Nil -> 
                        cb.OnCompleted()
                        outer.Value.Dispose()
                    | Error e -> 
                        cb.OnError(e)
                        outer.Value.Dispose()
                    | Cons(v, rest) ->
                        cb.OnNext(v)
                        inner := subscribeObs s cb rest
            )

        { new IDisposable with member x.Dispose() = inner.Value.Dispose(); outer.Value.Dispose() }


    let toObservable (initial : 's) (l : FutureList<'s, 'a>) =
        let observers = HashSet<IObserver<'a>>()
        
        let all() = lock observers (fun () -> Seq.toArray observers)

        let myObserver =
            { new IObserver<'a> with
                member x.OnNext(v) = 
                    for o in all() do o.OnNext v

                member x.OnError e =
                    for o in all() do o.OnError e

                member x.OnCompleted() =
                    for o in all() do o.OnCompleted()
            }

        let disp = l |> subscribeObs initial myObserver

        { new IObservable<'a> with
            member x.Subscribe o =
                let added = lock observers (fun () -> observers.Add o)

                if added then { new IDisposable with member x.Dispose() = lock observers (fun () -> observers.Remove o |> ignore)}
                else noDisposable
        }



        

[<AutoOpen>]
module ``FutureList Builder`` =
    
    type FutureListBuilder() =
        
        member x.For(m : IObservable<'a>, f : 'a -> FutureList<'s, 'b>) =
            m |> FutureList.ofObservable |> FutureList.collect f

        member x.For(m : IMod<'a>, f : 'a -> FutureList<'s, 'b>) =
            m |> FutureList.ofMod |> FutureList.collect f

        member x.For(m : FutureList<'s, 'a>, f : 'a -> FutureList<'s, 'b>) =
            m |> FutureList.collect f

        member x.Yield (v : 'a) =
            FutureList.single v

        member x.Delay(f : unit -> FutureList<'s, 'a>) = f

        member x.Run(f : unit -> FutureList<'s, 'a>) = f()

        member x.Combine(l : FutureList<'s, 'a>, r : unit -> FutureList<'s, 'a>) =
            FutureList.append l { run = fun s -> r().run s }

        member x.Bind(m : FutureList<'s, 'a>, f : 'a -> FutureList<'s, 'b>) =
            { run = fun s ->
                future {
                    let! (s, v) = m.run s
                    match v with
                        | Nil -> return s, Nil
                        | Error e -> return s, Error e
                        | Cons(v,_) -> return! (f v).run s
                }
            }

        member x.Return(u : unit) =
            FutureList.empty

        member x.Zero() =
            FutureList.empty


        member x.While(guard : unit -> #IMod<bool>, body : unit -> FutureList<'s, 'a>) =
            let guard = guard() :> IMod<bool> |> FutureList.ofMod

            let rec kernel (both : FutureList<'s, Either<bool, 'a>>)=
                { run = fun s ->
                    future {
                        let! (s,res) = both.run s
                        match res with
                            | Nil -> return s, Nil
                            | Error e -> return s, Error e
                            | Cons(v, rest) ->
                                match v with
                                    | Left b ->
                                        if not b then return s, Nil
                                        else return! (kernel rest).run s

                                    | Right a -> 
                                        return s, Cons(a, kernel rest)
                    }
                }
             
            let rec wait(guard : FutureList<'s, bool>) =
                { run = fun s ->
                    future {
                        let! (s, g) = guard.run s
                        match g with
                            | Nil -> return s, Nil
                            | Error e -> return s, Error e
                            | Cons(v, rest) ->
                                if v then return! (kernel (FutureList.intersperse rest (body()))).run s
                                else return! (wait rest).run s
                    }
                }

            wait guard

        member x.While(guard : unit -> bool, body : unit -> FutureList<'s, 'a>) =
            { run = fun s ->
                if guard() then
                    let l = FutureList.append (body()) { run = fun s -> body().run s }
                    l.run s
                else
                    Future.ofValue(s, Nil)
            }


    let flist = FutureListBuilder()

    type ModRef<'a> with
        member x.Emit(value : 'a) =
            x.UnsafeCache <- value
            x.MarkOutdated()



/// define a worklfow 
type WorkflowState =
    {
        refValues : Map<int, obj>
        currentId : int
    } with

    static member Empty = { refValues = Map.empty; currentId = 0 }

type WorkflowResult<'i, 'f> =
    | Final of 'f
    | Update of 'i

type Workflow<'i, 'f> = { obs : IObservable<WorkflowResult<'i, 'f>> }

type StateWorkflow<'i, 'f> = { list : FutureList<WorkflowState, WorkflowResult<'i, 'f>> }
type WorkflowList<'a> = FutureList<WorkflowState, 'a>

module Workflow =
    
    let updates (l : Workflow<'i, 'f>) =
        l.obs |> Observable.choose (function Update v -> Some v | _ -> None)

    let finals (l : Workflow<'i, 'f>) =
        l.obs |> Observable.choose (function Final v -> Some v | _ -> None)

    let all (l : Workflow<'i, 'f>) =
        l.obs

    let latestUpdate (def : 'i) (l : Workflow<'i, 'f>) =
        let ref = Mod.init def

        l.obs |> Observable.add (fun v ->
            match v with
                | Update v -> transact (fun () -> Mod.change ref v)
                | Final v -> transact (fun () -> Mod.change ref def)
        )

        ref :> IMod<_>

    let allFinal(l : Workflow<'i, 'f>) =
        let ref = CSet.empty

        l.obs |> Observable.add (fun v ->
            match v with
                | Update v -> ()
                | Final v -> transact (fun () -> CSet.add v ref |> ignore)
        )

        ref :> aset<_>





[<AutoOpen>]
module ``Workflow Builder`` =
    
    type StateRef<'a> internal(id : int) =
        member x.Id = id

    let sref (value : 'a) =
        FutureList.modifyState' (fun (s : WorkflowState) ->
            let id = s.currentId
            let newState = { s with refValues = Map.add id (value :> obj) s.refValues; currentId = s.currentId + 1}
            newState, StateRef<'a>(id)
        )

    let read (r : StateRef<'a>) =
        flist {
            let! s = FutureList.getState
            yield Map.find r.Id s.refValues |> unbox<'a>
        }

    let write (r : StateRef<'a>) (value : 'a) =
        FutureList.modifyState (fun (s : WorkflowState) ->
            { s with refValues = Map.add r.Id (value :> obj) s.refValues }
        )

    let modify (f : 'a -> 'a) (r : StateRef<'a>) =
        FutureList.modifyState' (fun (s : WorkflowState) ->
            let v = s.refValues |> Map.find r.Id |> unbox<'a> |> f
            { s with refValues = Map.add r.Id (v :> obj) s.refValues }, ()
        )

    let modify' (f : 'a -> 'a) (r : StateRef<'a>) =
        FutureList.modifyState' (fun (s : WorkflowState) ->
            let v = s.refValues |> Map.find r.Id |> unbox<'a> |> f
            { s with refValues = Map.add r.Id (v :> obj) s.refValues }, v
        )

    let resetState =
        FutureList.putState WorkflowState.Empty

    type WorkflowBuilder() =
        
        member x.For(s : WorkflowList<'a>, f : 'a -> StateWorkflow<'i, 'f>) =
            { list = s |> FutureList.collect (fun v -> (f v).list) }

        member x.For(s : IMod<'a>, f : 'a -> StateWorkflow<'i, 'f>) =
            x.For(s |> FutureList.ofMod, f)

        member x.For(s : IObservable<'a>, f : 'a -> StateWorkflow<'i, 'f>) =
            x.For(s |> FutureList.ofObservable, f)

        member x.Bind(m : WorkflowList<'a>, f : 'a -> StateWorkflow<'i, 'f>) =
            { list = flist.Bind(m, fun v -> f(v).list) }


        member x.Bind(m : StateRef<'a>, f : 'a -> StateWorkflow<'i, 'f>) =
            { list = flist.Bind(read m, fun v -> f(v).list) }


        member x.Return(u : unit) =
            { list = flist.Return u }

        member x.Zero() =
            { list = flist.Zero() }

        member x.Yield v =
            { list = FutureList.single (Update v) }

        member x.YieldFrom (f : WorkflowList<'i>) =
            { list = f |> FutureList.map Update }

        member x.YieldFrom (f : StateRef<'i>) =
            { list = read f |> FutureList.map Update }


        member x.Return v =
            { list = FutureList.single (Final v) }

        member x.ReturnFrom (f : WorkflowList<'f>) =
            { list = f |> FutureList.map Final }

        member x.ReturnFrom (f : StateRef<'i>) =
            { list = read f |> FutureList.map Final }


        member x.Combine(l : StateWorkflow<'i, 'f>, r : unit -> StateWorkflow<'i, 'f>) =
            { list = flist.Combine(l.list, fun () -> r().list) }

        member x.Delay(f : unit -> StateWorkflow<'i, 'f>) = f

        member x.Run(f : unit -> StateWorkflow<'i, 'f>) = 
            { obs = f().list |> FutureList.toObservable { refValues = Map.empty; currentId = 0 } }

        member x.While(guard : unit -> #IMod<bool>, body : unit -> StateWorkflow<'i, 'f>) =
            { list = flist.While(guard, fun () -> body().list) }

        member x.While(guard : unit -> bool, body : unit -> StateWorkflow<'i, 'f>) =
            { list = flist.While(guard, fun () -> body().list) }



    let inline (!=) (r : StateRef<'a>) (value : 'a) = write r value
    let inline (!&) (r : StateRef<'a>) = read r

    let workflow = WorkflowBuilder()


module Test =
    
    let (<==) (m : ModRef<'a>) (value : 'a) =
        transact (fun () -> m.Emit value)

    let run() =
        let active = Mod.init false
        let positions = Mod.init 0


        let test =
            workflow {
                    do! resetState
                    let! r = sref []

                    while active do
                        for p in positions do
                            do! r |> modify (fun l -> p::l)
                            yield! r

                    let! current = r
                    return current |> List.rev |> List.toArray

            }

        test |> Workflow.latestUpdate [] |> Mod.unsafeRegisterCallbackKeepDisposable (fun v ->
            printfn "{ latest = %A }" v
        ) |> ignore

        test |> Workflow.allFinal |> ASet.unsafeRegisterCallbackKeepDisposable (fun v ->
            printfn "{ finals = %A }" v
        ) |> ignore



        active <== true
        positions <== 1
        positions <== 2
        positions <== 3
        positions <== 3
        active <== false
        positions <== 6
        positions <== 7

        active <== true
        positions <== 5
        positions <== 6
        positions <== 7
        positions <== 7
        active <== false





