namespace Aardvark.Base

open System
open System.Threading
open System.Collections.Generic

type Monoid<'ops> =
    {
        misEmpty    : 'ops -> bool
        mempty      : 'ops
        mappend     : 'ops -> 'ops -> 'ops
    }

type Traceable<'s, 'ops> =
    {
        ops         : Monoid<'ops>
        empty       : 's
        apply       : 's -> 'ops -> 's * 'ops
        compute     : 's -> 's -> 'ops
        collapse    : 's -> int -> bool
    }


[<AbstractClass>]
type Index() =
    abstract member CompareTo : obj -> int
    abstract member After : unit -> Index
    abstract member Before : unit -> Index
    abstract member Between : Index -> Index

    [<CompilerMessage("Next is considered harmful", 4321, IsError=false, IsHidden=true)>]
    abstract member Next : Index

    default x.GetHashCode() = failwith ""
    default x.Equals o = failwith ""

    interface IComparable with
        member x.CompareTo(o : obj) = x.CompareTo o

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Index =
    
    [<StructuredFormatDisplay("{AsString}")>]
    type private Value =
        class
            val mutable public Root : Value
            val mutable public Prev : Value
            val mutable public Next : Value
            val mutable public Tag : uint64
            val mutable public RefCount : int

            static member private Relabel(start : Value) =
                let all = List<Value>()

                let distance (l : Value) (r : Value) =
                    if l = r then UInt64.MaxValue
                    else r.Tag - l.Tag


                // distance start start.Next == 1

                let mutable current = start.Next
                all.Add start.Next
                Monitor.Enter start.Next

                let mutable cnt = 1UL
                while distance start current < 1UL + cnt * cnt do
                    current <- current.Next
                    cnt <- cnt + 1UL
                    all.Add current
                    Monitor.Enter current

                let space = distance start current

                // the last node does not get relabeled
                current <- current.Prev
                all.RemoveAt (all.Count - 1)
                Monitor.Exit current.Next
                cnt <- cnt - 1UL

                let step = space / (1UL + cnt)
                let mutable current = start.Tag + step
                for n in all do
                    n.Tag <- current
                    current <- current + step
                    Monitor.Exit n
                    
                step
//
//
//
//
//                let available  = distance start current
//
//
//
//                
//                let distance (n : Value) =
//                    if n = start then UInt64.MaxValue
//                    else n.Tag - start.Tag
//
//                // find a range s.t. distance(range) >= 1 + |range|^2 
//                let mutable current = start.Next
//                all.Add current
//                Monitor.Enter current
//                let mutable j = 1UL
//                while distance current < 1UL + j * j do
//                    all.Add current.Next
//                    Monitor.Enter current.Next
//                    current <- current.Next
//                    j <- j + 1UL
//
//                // distribute all times in the range equally spaced
//                let step = (distance current) / j
//                current <- start.Next
//                let mutable currentTime = start.Tag + step
//                for k in 1UL..(j-1UL) do
//                    current.Tag <- currentTime
//                    current <- current.Next
//                    currentTime <- currentTime + step
//                    Monitor.Exit current
//                Monitor.Exit current.Next
//                // store the distance to the next time
//                step

//                let mutable n = 1UL
//
//                while distance current < 1UL + n * n do
//                    Monitor.Enter current
//                    current <- current.Next
//                    all.Add current
//                    n <- n + 1UL
//
//                let step = (distance current) / n
//
//                let mutable key = start.Tag + step
//                for a in all do
//                    a.Tag <- key
//                    key <- key + step
//                    Monitor.Exit a
//
//                step

            member x.Key = x.Tag - x.Root.Tag

            member x.InsertAfter() =
                lock x (fun () ->
                    let next = x.Next
                    
                    let mutable distance = 
                        if next = x then UInt64.MaxValue
                        else next.Tag - x.Tag

                    if distance = 1UL then
                        distance <- Value.Relabel x
                        
                    let key = x.Tag + (distance / 2UL)
                    let res = Value(x.Root, Prev = x, Next = x.Next, Tag = key)

                    next.Prev <- res
                    x.Next <- res

                    res
                )

            member x.Delete() =
                let prev = x.Prev
                Monitor.Enter prev
                if prev.Next <> x then
                    Monitor.Exit prev
                    x.Delete()
                else
                    Monitor.Enter x
                    try
                        if x.RefCount = 1 then
                            prev.Next <- x.Next
                            x.Next.Prev <- prev
                            x.RefCount <- 0
                        else
                            x.RefCount <- x.RefCount - 1

                    finally
                        Monitor.Exit x
                        Monitor.Exit prev

            member x.AddRef() =
                lock x (fun () ->
                    x.RefCount <- x.RefCount + 1
                )

            member x.CompareTo(o : Value) =
                match Monitor.TryEnter x, Monitor.TryEnter o with
                    | true, true ->
                        compare x.Key o.Key

                    | true, false ->
                        Monitor.Exit x
                        x.CompareTo o

                    | false, true ->
                        Monitor.Exit o
                        x.CompareTo o

                    | false, false ->
                        x.CompareTo o

            interface IComparable with
                member x.CompareTo (o : obj) =
                    match o with
                        | :? Value as o -> x.CompareTo o
                        | _ -> failwithf "[Real] cannot compare real to %A" o

            override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x)
            override x.Equals o = System.Object.ReferenceEquals(x,o)
            override x.ToString() = sprintf "%f" (float x.Key / float UInt64.MaxValue)
            member private x.AsString = x.ToString()

            new(root : Value) = { Root = root; Prev = Unchecked.defaultof<_>; Next = Unchecked.defaultof<_>; Tag = 0UL; RefCount = 0 }
        end
        
    [<StructuredFormatDisplay("{AsString}")>]
    type private GCReal(real : Value) =
        inherit Index()
        do real.AddRef()

        static let queue = new System.Collections.Concurrent.BlockingCollection<Value>()
        static let runner =
            async {
                do! Async.SwitchToNewThread()
                while true do
                    let e = queue.Take()
                    e.Delete()
            }
        static do Async.Start runner

        member private x.Value = real

        override x.After() =
            lock real (fun () ->
                if real.Next <> real.Root then GCReal real.Next :> Index
                else GCReal (real.InsertAfter()) :> Index
            )   

        override x.Before() =
            let prev = real.Prev
            Monitor.Enter prev
            if prev.Next <> real then
                Monitor.Exit prev
                x.Before()
            else
                try
                    if prev = real.Root then 
                        prev.InsertAfter() |> GCReal :> Index
                    else
                        prev |> GCReal :> Index
                finally
                    Monitor.Exit prev

        override l.Between(r : Index) =
            let l = l.Value
            let r = (unbox<GCReal> r).Value
            Monitor.Enter l
            try
                if l.Next = r then l.InsertAfter() |> GCReal :> Index
                else l.Next |> GCReal :> Index
            finally
                Monitor.Exit l

        override x.Finalize() =
            queue.Add real

        override x.CompareTo (o : obj) =
            match o with
                | :? GCReal as o -> real.CompareTo(o.Value)
                | _ -> failwithf "[Real] cannot compare real to %A" o

        override x.GetHashCode() = real.GetHashCode()
        override x.Equals o =
            match o with
                | :? GCReal as o -> real.Equals o.Value
                | _ -> false
                
        override x.ToString() = real.ToString()
        member private x.AsString = x.ToString()

        override x.Next = 
            lock real (fun () ->
                GCReal real.Next :> Index
            )

        new() = 
            let r = Value(Unchecked.defaultof<_>)
            r.Root <- r
            r.Next <- r
            r.Prev <- r
            GCReal(r)

    let zero = GCReal() :> Index
    let after (r : Index) = r.After()
    let before (r : Index) = r.Before()
    let between (l : Index) (r : Index) = l.Between r

module Map =
    let mapMonotonic (mapping : 'k -> 'v -> 'k2 * 'v2) (l : Map<'k, 'v>) =
        l |> Map.toSeq |> Seq.map (fun (i,v) -> mapping i v) |> Map.ofSeq

