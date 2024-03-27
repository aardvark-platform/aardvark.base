namespace FSharp.Data.Adaptive

open System.Threading
open System.Threading.Tasks
open System.Runtime.CompilerServices

open Aardvark.Base

[<AutoOpen>]
module private Utilities =

    [<return: Struct>]
    let inline (|CancelExn|_|) (e : exn) =
        match e with
        | :? System.OperationCanceledException
        | :? TaskCanceledException ->
            ValueSome()
        | _ ->
            ValueNone

    [<return: Struct>]
    let inline (|AggregateExn|_|) (e : exn) =
        match e with
        | :? System.AggregateException as e ->
            let l = e.InnerExceptions |> Seq.toList
            ValueSome l
        | _ ->
            ValueNone

type ProcState =
    {
        ct : CancellationToken
    }

type ProcResult<'a> =
    | Value of 'a
    | Cancelled
    | Faulted of exn

and Proc<'a, 'r>(runWhenCancelled : bool, cont : ProcState -> (ProcResult<'a> -> 'r) -> 'r) =
    member x.RunWhenCancelled = runWhenCancelled
    member x.ContinueWith s c = 
        if not runWhenCancelled && s.ct.IsCancellationRequested then
            c Cancelled
        else
            cont s c 

    new(cont : ProcState -> (ProcResult<'a> -> 'r) -> 'r) = Proc(false, cont)

[<AutoOpen>]
module ``ProcResult Smart Constructors`` =
    let internal cancelexn = System.OperationCanceledException()
    let rec Faulted (e : exn) =
        match e with
            | AggregateExn [inner] -> Faulted inner
            | CancelExn -> Cancelled
            | e -> ProcResult.Faulted e

    type ProcResult<'a> with
        static member Faulted e =
            Faulted e

[<Extension>]
type Proc =
    static member Create (v : 'a) =
        Proc<'a, 'r>(fun state cont ->
            cont (Value v)
        )
            
    [<Extension>]
    static member Await (t : Task<'a>) =
        Proc<'a, unit>(fun state cont ->
            if state.ct.IsCancellationRequested then
                cont Cancelled
            else
                let s = state.ct.Register(fun () -> cont Cancelled)
                let c (t : Task<'a>) =
                    s.Dispose()
                    if state.ct.IsCancellationRequested || t.Status = TaskStatus.Canceled then
                        cont Cancelled
                    elif t.Status = TaskStatus.Faulted then
                        cont (Faulted t.Exception)
                    else
                        cont (Value t.Result)

                t.ContinueWith(c, state.ct) |> ignore
        )
            
    [<Extension>]
    static member Await (t : Task) =
        Proc<unit, unit>(fun state cont ->
            if state.ct.IsCancellationRequested then
                cont Cancelled
            else
                let s = state.ct.Register(fun () -> cont Cancelled)
                let c (t : Task) =
                    s.Dispose()
                    if state.ct.IsCancellationRequested || t.Status = TaskStatus.Canceled then
                        cont Cancelled
                    elif t.Status = TaskStatus.Faulted then
                        cont (Faulted t.Exception)
                    else
                        cont (Value ())

                t.ContinueWith(c, state.ct) |> ignore
        )
        
    [<Extension>]
    static member Await (a : Async<'a>) =
        Proc<'a, unit>(fun state cont ->
            if state.ct.IsCancellationRequested then
                cont Cancelled
            else
                Async.StartWithContinuations(
                    a,
                    (fun v -> cont (Value v)),
                    (fun e -> cont (Faulted e)),
                    (fun _ -> cont (Cancelled)),
                    state.ct
                )
        )

    [<Extension>]
    static member Await(sem : SemaphoreSlim) =
        sem.WaitAsync() |> Proc.Await

    static member Sleep (ms : int) =
        Task.Delay(ms) |> Proc.Await

    static member Bind (m : Proc<'a, 'r>, mapping : 'a -> Proc<'b, 'r>) =
        Proc (fun state cont ->
            m.ContinueWith state (fun res ->
                match res with
                    | Value v -> 
                        try mapping(v).ContinueWith state cont
                        with e -> cont (Faulted e)
                    | Cancelled -> cont Cancelled
                    | Faulted e -> cont (Faulted e)
            )
        )

    static member Map (m : Proc<'a, 'r>, f : 'a -> 'b) =
        Proc<'b, 'r>(fun state cont ->
            m.ContinueWith state (fun res ->
                match res with
                    | Value v -> 
                        try cont (Value (f v))
                        with e -> cont (Faulted e)
                    | Cancelled -> cont Cancelled
                    | Faulted e -> cont (Faulted e)
            )
        )
    
    static member TryFinally (m : Proc<'a, 'r>, fin : unit -> unit) =
        Proc(fun state cont ->
            m.ContinueWith state (fun res ->
                match res with
                    | Value v -> fin(); cont (Value v)
                    | Cancelled -> fin(); cont Cancelled
                    | Faulted e -> fin(); cont (Faulted e)
            )
        )

    static member TryWith (m : Proc<'a, 'r>, comp : exn -> unit) =
        Proc<_,_>(fun state cont ->
            m.ContinueWith state (fun res ->
                match res with
                    | Faulted e -> comp(e); cont(Faulted e) //.ContinueWith state cont
                    | Cancelled -> comp(cancelexn); cont(Cancelled) //.ContinueWith state cont
                    | Value c -> cont(Value c)
            )  
        ) 

    static member Append (l : Proc<unit, 'r>, r : Proc<'a, 'r>) =
        Proc (fun state cont ->
            l.ContinueWith state (fun res ->
                match res with
                    | Value () -> r.ContinueWith state cont
                    | Cancelled -> cont Cancelled
                    | Faulted e -> cont (Faulted e)
            )
        )

    static member SwitchToThreadPool() =
        Proc(fun state cont ->
            Task.Factory.StartNew(fun () ->
                cont (Value ())
            ) |> ignore
        )

    static member SwitchToNewThread() =
        Proc(fun state cont ->
            let run () = cont (Value())
            let thread = new Thread(ThreadStart(run), IsBackground = true)
            thread.Start()
        )

    static member RunSynchronously(m : Proc<'a, unit>, ?cancellationToken : CancellationToken) =
        let ct = defaultArg cancellationToken Unchecked.defaultof<_>
        use v = new ManualResetEventSlim(false)
        let mutable res = Unchecked.defaultof<_>
        m.ContinueWith { ct = ct } (fun r ->
            res <- r
            v.Set()
        )
        v.Wait()
        res
            
    static member StartAsTask(m : Proc<'a, unit>, ?cancellationToken : CancellationToken) =
        let ct = defaultArg cancellationToken Unchecked.defaultof<_>
        let tcs = TaskCompletionSource<ProcResult<'a>>()
        m.ContinueWith { ct = ct } (fun r ->
            tcs.SetResult(r)
        )
        tcs.Task

    static member Start(m : Proc<unit, unit>, ?cancellationToken : CancellationToken) =
        let ct = defaultArg cancellationToken Unchecked.defaultof<_>
        m.ContinueWith { ct = ct } ignore

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Proc =
    let inline create v = Proc.Create v
    let inline map (mapping : 'a -> 'b) (m : Proc<'a, 'r>) = Proc.Map(m, mapping)
    let inline bind (mapping : 'a -> Proc<'b, 'r>) (m : Proc<'a, 'r>) = Proc.Bind(m, mapping)
    let inline append (l : Proc<unit, 'r>) (r : Proc<'a, 'r>) = Proc.Append(l, r)

type ProcBuilder() =
    member x.Zero() = Proc.Create ()
    member x.Return v = Proc.Create v
    member x.ReturnFrom (p : Proc<_,_>) = p
    member x.Bind(m,f) = Proc.Bind(m,f)
    member x.Bind(m : Task<'a>,f) = Proc.Bind(Proc.Await m,f)
    member x.Bind(m : Task,f) = Proc.Bind(Proc.Await m,f)
    member x.Bind(m : Async<'a>,f) = Proc.Bind(Proc.Await m,f)
    member x.Combine(l,r) = Proc.Append(l,r)
    member x.Delay(f : unit -> Proc<'a, 'r>) = Proc<_,_>(fun state cont -> f().ContinueWith state cont)
    member x.TryFinally(m, fin) = Proc.TryFinally(m, fin)
    member x.TryWith(m, handler : exn -> Proc<unit, unit>) = 
        Proc.TryWith(m, fun e ->
            handler e |> Proc.RunSynchronously |> ignore
        )
    member x.For(s : seq<'a>, f : 'a -> Proc<unit, 'r>) =
        let rec build (l : list<'a>) =
            match l with
                | [] -> Proc.Create ()
                | h :: rest ->
                    Proc.Append(f h, build rest)

        s |> Seq.toList |> build

    member x.Using<'a, 'b, 'r when 'a :> System.IDisposable> (v : 'a, f : 'a -> Proc<'b, 'r>) =
        Proc(true, fun state cont ->
            if state.ct.IsCancellationRequested then
                v.Dispose()
                cont Cancelled
            else
                try
                    f(v).ContinueWith state (fun res ->
                        match res with
                            | Cancelled -> v.Dispose(); cont (Cancelled)
                            | Faulted e -> v.Dispose(); cont (Faulted e)
                            | Value res -> v.Dispose(); cont (Value res)
                    )
                with e ->
                    v.Dispose()
                    cont (Faulted e)
                        
        )

[<AutoOpen>]
module ``Proc Builder`` =
    let proc = ProcBuilder()
