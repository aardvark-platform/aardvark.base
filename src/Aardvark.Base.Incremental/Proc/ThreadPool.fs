namespace FSharp.Data.Adaptive

open System
open System.Threading
open FSharp.Data.Adaptive

[<AbstractClass>]
type Command<'a>() =
    abstract member Start : ('a -> unit) -> unit
    abstract member Stop : unit -> unit

type LeafCommand<'a>(p : ProcList<'a, unit>) =
    inherit Command<'a>()

    let name = Guid.NewGuid()
    let cancel = new CancellationTokenSource()

    let rec run (ct : CancellationToken) (f : 'a -> unit) (p : ProcList<'a, unit>) =
        proc {
            let! res = p.run
            match res with
                | ProcListValue.Nil -> 
                    ()
                | ProcListValue.Cons(msg, cont) ->
                    if not ct.IsCancellationRequested then 
                        f msg
                        do! run ct f cont
        }

    override x.Start(f : 'a -> unit) =
        let runner = run cancel.Token f p
        Proc.Start(runner, cancel.Token)

    override x.Stop() =
        cancel.Cancel()


type ThreadPool<'a>(_store : HashMap<string, Command<'a>>) =
    static let empty = ThreadPool<'a>(HashMap.empty)

    static member Empty = empty
    
    member x.store = _store


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ThreadPool =
    
    let empty<'msg> = ThreadPool<'msg>.Empty

    let map (mapping : 'a -> 'b) (pool : ThreadPool<'a>) =
        ThreadPool(
            pool.store |> HashMap.map (fun _ v -> 
                { new Command<'b>() with
                    member x.Start(f) = v.Start(mapping >> f)
                    member x.Stop() = v.Stop()
                }
            )
        )

    let add (id : string) (proc : ProcList<'a, unit>) (p : ThreadPool<'a>) =
        ThreadPool(HashMap.add id (LeafCommand(proc) :> Command<_>) p.store)

    let remove (id : string) (p : ThreadPool<'a>) =
        ThreadPool(HashMap.remove id p.store)

    let start (proc : ProcList<'a, unit>) (p : ThreadPool<'a>) =
        let id = Guid.NewGuid() |> string
        ThreadPool(HashMap.add id (LeafCommand(proc) :> Command<_>) p.store)

    let union (l : ThreadPool<'a>) (r : ThreadPool<'a>) =
        ThreadPool(HashMap.union l.store r.store)