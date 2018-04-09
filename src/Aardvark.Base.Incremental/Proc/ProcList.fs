namespace Aardvark.Base.Incremental

open System
open System.Threading
open System.Threading.Tasks

open Aardvark.Base

type ProcListValue<'a, 'r> =
    | Nil
    | Cons of 'a * ProcList<'a, 'r>

and ProcList<'a, 'r>(run : Proc<ProcListValue<'a, 'r>, 'r>) =
    static let empty = ProcList<'a, 'r>(Proc.Create Nil)

    static member Empty = empty

    member x.run = run

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ProcList =
    let empty<'a, 'r> = ProcList<'a, 'r>.Empty

    let single (v : 'a) : ProcList<'a, 'r> = 
        ProcList<'a, 'r>(Proc.create <| Cons(v, empty))

    let ofSeq (v : seq<'a>) : ProcList<'a, 'r> = 
        use e = v.GetEnumerator()
        let rec build() =
            if e.MoveNext() then
                let v = e.Current
                let rest = build()
                ProcList(Proc.create <| Cons(v, rest))
            else
                empty
        build()

    let rec ofList (l : list<'a>) : ProcList<'a, 'r> =
        match l with
            | [] -> 
                empty

            | h :: rest ->
                Cons(h, ofList rest)
                    |> Proc.Create
                    |> ProcList
    
    let ofArray (arr : 'a[]) : ProcList<'a, 'r> =
        let rec build (i : int) (arr : 'a[]) =
            if i >= arr.Length then
                empty
            else
                ProcList(Proc.create <| Cons(arr.[i], build (i+1) arr))
        build 0 arr


    let rec map (mapping : 'a -> 'b) (list : ProcList<'a, 'r>) : ProcList<'b, 'r> =
        ProcList(
            proc {
                let! c = list.run
                match c with
                    | Nil -> 
                        return Nil

                    | Cons(h,tail) ->
                        return Cons(mapping h, map mapping tail)
            }
        )

    let rec choose (mapping : 'a -> Option<'b>) (list : ProcList<'a, 'r>) : ProcList<'b, 'r> =
        ProcList(
            proc {
                let! c = list.run
                match c with
                    | Nil -> 
                        return Nil

                    | Cons(h,tail) ->
                        match mapping h with
                            | Some v ->
                                return Cons(v, choose mapping tail)
                            | None ->
                                return! (choose mapping tail).run
            }
        )

    let rec filter (predicate : 'a -> bool) (list : ProcList<'a, 'r>) : ProcList<'a, 'r> =
        ProcList (
            proc {
                let! l = list.run
                match l with
                    | Nil -> return Nil
                    | Cons(h,tail) ->
                        if predicate h then
                            return Cons(h, filter predicate tail)
                        else
                            return! (filter predicate tail).run
            }
        )

    let rec append (l : ProcList<'a, 'r>) (r : ProcList<'a, 'r>) =
        ProcList (
            proc {
                let! l = l.run
                match l with
                    | Nil -> return! r.run
                    | Cons(h,tail) -> return Cons(h, append tail r)
            }
        )

    let rec concat (l : ProcList<ProcList<'a, 'r>, 'r>) =
        ProcList(
            proc {
                let! l = l.run
                match l with
                    | Nil -> 
                        return Nil

                    | Cons(h,t) ->
                        return! (append h (concat t)).run
            }
        )

    let rec bind (mapping : 'a -> ProcList<'b, 'r>) (m : Proc<'a, 'r>) : ProcList<'b, 'r> =
        ProcList (
            proc {
                let! res = m
                return! (mapping res).run
            }
        )

    let rec collect (mapping : 'a -> ProcList<'b, 'r>) (list : ProcList<'a, 'r>) : ProcList<'b, 'r> =
        ProcList (
            proc {
                let! list = list.run
                match list with
                    | Nil -> 
                        return Nil

                    | Cons(h,tail) ->
                        let res = append (mapping h) (collect mapping tail)
                        return! res.run
            }
        )

    let delay (f : unit -> ProcList<'a, 'r>) =
        ProcList(
            proc {
                return! f().run
            }
        )

type ProcListBuilder() =
    member x.Bind(m,f) = ProcList.bind f m
    member x.Bind(m : Task<'a>,f) = ProcList.bind f (m.Await())
    member x.Bind(m : Task,f) = ProcList.bind f (m.Await())
    member x.Bind(m : Async<'a>,f) = ProcList.bind f (m.Await())
    member x.Zero() = ProcList.empty
    member x.Yield v = ProcList.single v
    member x.Combine(l,r) = ProcList.append l r
    member x.Delay(f) = ProcList.delay f
    member x.YieldFrom(l : Proc<'a, 'r>) = l |> ProcList.bind ProcList.single
    member x.YieldFrom(l : Task<'a>) = l.Await() |> ProcList.bind ProcList.single
    member x.YieldFrom(l : Task) = l.Await() |> ProcList.bind ProcList.single
    member x.YieldFrom(l : Async<'a>) = l.Await() |> ProcList.bind ProcList.single
    member x.YieldFrom(l : ProcList<'a, 'r>) = l
    member x.YieldFrom(s : seq<'a>) = ProcList.ofSeq s
    member x.YieldFrom(s : list<'a>) = ProcList.ofList s
    member x.YieldFrom(s : array<'a>) = ProcList.ofArray s
    member x.For(m : ProcList<'a, 'r>, f : 'a -> ProcList<'b, 'r>) = ProcList.collect f m
    member x.For(m : seq<'a>, f : 'a -> ProcList<'b, 'r>) = ProcList.collect f (ProcList.ofSeq m)

[<AutoOpen>]
module ``ProcList Builder`` =
    let proclist = ProcListBuilder()