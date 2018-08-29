namespace Aardvark.Base

open System
open System.Threading
open System.Collections.Generic

[<AbstractClass>]
type Index() =
    abstract member CompareTo : obj -> int
    abstract member After : unit -> Index
    abstract member Before : unit -> Index
    abstract member Between : Index -> Index

    [<CompilerMessage("Next is considered harmful", 4321, IsError=false, IsHidden=true)>]
    abstract member Next : Index

    default x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x)
    default x.Equals o = System.Object.ReferenceEquals(x,o)

    interface IComparable with
        member x.CompareTo(o : obj) = x.CompareTo o
        
    interface IComparable<Index> with
        member x.CompareTo(o : Index) = x.CompareTo o

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
                        try 
                            compare x.Key o.Key
                        finally
                            Monitor.Exit x
                            Monitor.Exit o

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
            startThread (fun () -> 
                while true do
                    let e = queue.Take()
                    e.Delete()
            )

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
