namespace Aardvark.Base

#nowarn "44"

open System

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