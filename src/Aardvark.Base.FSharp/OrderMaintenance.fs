namespace Aardvark.Base

open System
open System.Collections.Generic

[<AllowNullLiteral>] 
type IDeletableComparable =
    inherit IComparable
    abstract member IsDeleted : bool

[<AllowNullLiteral>] 
type ISortKey =
    inherit IDeletableComparable
    abstract member Clock : IOrder

and IOrder =
    abstract member Count : int
    abstract member Root : ISortKey

module Order =
//    let toSeq (c : IOrder) =
//        let rec toSeq (t : ISortKey) =
//            seq {
//                yield t
//                if t.Next <> c.Root then
//                    yield! toSeq t
//            }
//
//        toSeq c.Root.Next
//        
//    let toArray (c : IOrder) =
//        let mutable current = c.Root.Next
//        let arr = Array.zeroCreate (c.Count-1)
//        for i in 0..c.Count-2 do
//            arr.[i] <- current
//            current <- current.Next
//        arr

//    let toList (c : IOrder) =
//        c |> toArray |> Array.toList

    let inline count (c : IOrder) =
        c.Count

    let inline root (c : IOrder) =
        c.Root


module SimpleOrder =
    
    [<AllowNullLiteral>]
    type SortKey =
        class
            val mutable public Clock : Order
            val mutable public Tag : uint64
            val mutable public Next : SortKey
            val mutable public Prev : SortKey

            member x.Time =
                x.Tag - x.Clock.Root.Tag

            member x.CompareTo (o : SortKey) =
                if isNull o.Next || isNull x.Next then
                    failwith "cannot compare deleted times"

                if o.Clock <> x.Clock then
                    failwith "cannot compare times from different clocks"

                compare x.Time o.Time

            interface IComparable with
                member x.CompareTo o =
                    match o with
                        | :? SortKey as o -> x.CompareTo(o)
                        | _ -> failwithf "cannot compare time with %A" o

            interface IComparable<ISortKey> with
                member x.CompareTo o =
                    match o with
                        | :? SortKey as o -> x.CompareTo o
                        | _ -> failwithf "cannot compare time with %A" o

            override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x)
            override x.Equals o = System.Object.ReferenceEquals(x,o)

            interface ISortKey with
                member x.Clock = x.Clock :> IOrder
                member x.IsDeleted = isNull x.Next
                //member x.Next = x.Next :> ISortKey

            new(c) = { Clock = c; Tag = 0UL; Next = null; Prev = null }
        end

    and Order =
        class
            val mutable public Root : SortKey
            val mutable public Count : int

            member x.After (t : SortKey) =
                if t.Clock <> x then
                    failwith "cannot insert after a different clock's time"

                let distance (a : SortKey) (b : SortKey) =
                    if a = b then System.UInt64.MaxValue
                    else b.Tag - a.Tag

                let mutable dn = distance t t.Next

                // if the distance to the next time is 1 (no room)
                // relabel all times s.t. the new one can be inserted
                if dn = 1UL then
                    // find a range s.t. distance(range) >= 1 + |range|^2 
                    let mutable current = t.Next
                    let mutable j = 1UL
                    while distance t current < 1UL + j * j do
                        current <- current.Next
                        j <- j + 1UL

                    // distribute all times in the range equally spaced
                    let step = (distance t current) / j
                    current <- t.Next
                    let mutable currentTime = t.Tag + step
                    for k in 1UL..(j-1UL) do
                        current.Tag <- currentTime
                        current <- current.Next
                        currentTime <- currentTime + step

                    // store the distance to the next time
                    dn <- step

                // insert the new time with distance (dn / 2) after
                // the given one (there has to be enough room now)
                let res = SortKey(x)
                res.Tag <- t.Tag + dn / 2UL

                res.Next <- t.Next
                res.Prev <- t
                t.Next.Prev <- res
                t.Next <- res

                res

            member x.Before (t : SortKey) =
                if t = x.Root then
                    failwith "cannot insert before root-time"
                x.After t.Prev

            member x.Delete (t : SortKey) =
                if not (isNull t.Next) then
                    if t.Clock <> x then
                        failwith "cannot delete time from different clock"

                    t.Prev.Next <- t.Next
                    t.Next.Prev <- t.Prev
                    t.Next <- null
                    t.Prev <- null
                    t.Tag <- 0UL
                    t.Clock <- Unchecked.defaultof<_>      

            member x.Clear() =
                let r = SortKey(x)
                x.Root <- r
                r.Next <- r
                r.Prev <- r
                x.Count <- 1

            interface IOrder with
                member x.Root = x.Root :> ISortKey
                member x.Count = x.Count

            static member New() =
                let c = Order()
                let r = SortKey(c)
                c.Root <- r
                r.Next <- r
                r.Prev <- r
                c

            private new() = { Root = null; Count = 1 }
        end

    let create() =
        Order.New()

module SkipOrder =
    let private random = Random()
    let randomHeight() =
        1 - random.NextDouble().Log2Int()

    [<AllowNullLiteral>]
    type SortKey =
        class
            val mutable public Clock : Order
            val mutable public Tag : uint64
            val mutable public NextArray : SortKeyLink[]
            val mutable public PrevArray : SortKeyLink[]
            val mutable public IsDeleted : bool

            member x.Time =
                x.Tag - x.Clock.Root.Tag

            member x.Height =
                x.NextArray.Length

            member x.Next = 
                if not (isNull x.NextArray) then x.NextArray.[0].Target
                else null

            member x.Prev =
                if not (isNull x.PrevArray) then x.PrevArray.[0].Target
                else null

            member x.CompareTo (o : SortKey) =
                if o.IsDeleted || x.IsDeleted then
                    failwith "cannot compare deleted times"

                if o.Clock <> x.Clock then
                    failwith "cannot compare times from different clocks"

                compare x.Time o.Time

            interface IComparable with
                member x.CompareTo o =
                    match o with
                        | :? SortKey as o -> x.CompareTo(o)
                        | _ -> failwithf "cannot compare time with %A" o

            interface IComparable<ISortKey> with
                member x.CompareTo o =
                    match o with
                        | :? SortKey as o -> x.CompareTo o
                        | _ -> failwithf "cannot compare time with %A" o

            override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x)
            override x.Equals o = System.Object.ReferenceEquals(x,o)

            interface ISortKey with
                member x.Clock = x.Clock :> IOrder
                member x.IsDeleted = x.IsDeleted
                //member x.Next = x.Next :> ISortKey

            new(c, h) = { Clock = c; Tag = 0UL; NextArray = Array.zeroCreate h; PrevArray = Array.zeroCreate h; IsDeleted = false }
        end

    and SortKeyLink =
        struct
            val mutable public Width : int
            val mutable public Target : SortKey

            new(w,t) = { Width = w; Target = t }
        end

    and Order =
        class
            val mutable public Root : SortKey
            val mutable public Count : int

            member x.After (t : SortKey) =
                if t.Clock <> x then
                    failwith "cannot insert after a different clock's time"

                let distance (a : SortKey) (b : SortKey) =
                    if a = b then System.UInt64.MaxValue
                    else b.Tag - a.Tag

                let mutable dn = distance t t.Next

                // if the distance to the next time is 1 (no room)
                // relabel all times s.t. the new one can be inserted
                if dn = 1UL then
                    // find a range s.t. distance(range) >= 1 + |range|^2 
                    let mutable current = t.NextArray.[0].Target
                    let mutable j = 1UL
                    while distance t current < 1UL + j * j do
                        current <- current.Next
                        j <- j + 1UL

                    // distribute all times in the range equally spaced
                    let step = (distance t current) / j
                    current <- t.NextArray.[0].Target
                    let mutable currentTime = t.Tag + step
                    for k in 1UL..(j-1UL) do
                        current.Tag <- currentTime
                        current <- current.NextArray.[0].Target
                        currentTime <- currentTime + step

                    // store the distance to the next time
                    dn <- step

                // insert the new time with distance (dn / 2) after
                // the given one (there has to be enough room now)
                let h = randomHeight()
                let res = SortKey(x, h)
                res.Tag <- t.Tag + dn / 2UL


                // since the predecessor might be "smaller" than 
                // the new node we need to search the remaining links
                // by going backward in the list
                let mutable current = t
                let mutable distance = 1

                let resize (h : int) =
                    let n = x.Root
                    if h > n.Height then
                        let additional = Array.create (h - n.Height) (SortKeyLink(x.Count,n))
                        n.NextArray <- Array.append n.NextArray additional
                        n.PrevArray <- Array.append n.PrevArray additional

                let back (n : SortKey) =
                    let link = n.PrevArray.[n.PrevArray.Length - 1]
                    (link.Width, link.Target)

                let tup (l : SortKeyLink) =
                    (l.Width, l.Target)

                for i in 0..h-1 do

                    // go backwards until a node with sufficient height is found
                    // or until we've reached the representant
                    while i >= current.Height && current <> x.Root do
                        let (d,l) = back current

                        current <- l
                        distance <- distance + d

                    // if the found node is not sufficiently high it must
                    // be the representant and therefore it has to be resized
                    if i >= current.Height then
                        resize h

                    // current must now be sufficiently high  
                    let (d,n) = tup current.NextArray.[i]

                    current.NextArray.[i] <- SortKeyLink(distance, res)
                    res.PrevArray.[i] <- SortKeyLink(distance, current)
                    res.NextArray.[i] <- SortKeyLink(1 + d - distance, n)
                    n.PrevArray.[i] <- SortKeyLink(1 + d - distance, res)

                // since the predecessor and the new node might be
                // smaller than the total height we need to increment
                // the width of all pointers "passing" the new node (above)
                let mutable current = t
                for i in h..x.Root.Height-1 do
                    while i >= current.Height && current <> x.Root do
                        let (d,l) = back current
                        current <- l

                    let (d,n) = tup current.NextArray.[i]
                    current.NextArray.[i] <- SortKeyLink(d + 1, n)
                    n.PrevArray.[i] <- SortKeyLink(d + 1, current)

                // finally increment the count and return the node
                x.Count <- x.Count + 1
                res

            member x.Before (t : SortKey) =
                if t = x.Root then
                    failwith "cannot insert before root-time"
                x.After t.Prev
              
            member x.Delete (t : SortKey) = 
                if not (isNull t.NextArray) then 
                    if t.Clock <> x then
                        failwith "cannot delete time from different clock"

                    let tup (l : SortKeyLink) = (l.Width, l.Target)
                    for l in 0..t.Height-1 do
                        let (dp, p) = tup t.PrevArray.[l]
                        let (dn, n) = tup t.NextArray.[l]

                        n.PrevArray.[l] <- SortKeyLink(dp + dn - 1, p)
                        p.NextArray.[l] <- SortKeyLink(dp + dn - 1, n)

                    let mutable current = t
                    let mutable distance = 1
                    for i in t.Height..x.Root.Height-1 do
                        let back (n : SortKey) =
                            let l = n.PrevArray.[n.PrevArray.Length - 1]
                            (l.Width, l.Target)
                        // go backwards until a node with sufficient height is found
                        // or until we've reached the representant
                        while i >= current.Height && current <> x.Root do
                            let (d,l) = back current
                            current <- l
                            distance <- distance + d

                        let (dn, n) = tup current.NextArray.[i]

                        current.NextArray.[i] <- SortKeyLink(dn - 1, n)
                        n.PrevArray.[i] <- SortKeyLink(dn - 1, current)

                    // every level (except for 0) on which rep.NextArray.[level] = rep 
                    // is useless and is therefore removed
                    let rep = x.Root
                    let mutable repHeight = rep.Height
                    while repHeight > 1 && rep.NextArray.[repHeight - 1].Target = rep do
                        repHeight <- repHeight - 1

                    if repHeight < rep.Height then
                        rep.NextArray <- Array.sub rep.NextArray 0 repHeight
                        rep.PrevArray <- Array.sub rep.PrevArray 0 repHeight
        
                    x.Count <- x.Count - 1
                    t.IsDeleted <- true
//                    t.NextArray <- null
//                    t.PrevArray <- null
//                    t.Tag <- 0UL
//                    t.Clock <- Unchecked.defaultof<_>      

            member x.Clear() =
                let r = SortKey(x, 1)
                x.Root <- r
                r.NextArray.[0] <- SortKeyLink(1,r)
                r.PrevArray.[0] <- SortKeyLink(1,r)
                x.Count <- 1

            /// gets the n-th time after this one
            /// NOTE that this only works on representant-nodes
            member x.TryAt (index : int) =

                let rec search (index : int) (level : int) (t : SortKey) : Option<SortKey> =
                    if level < 0 then
                        if index = 0 then Some t
                        else None
                    else
                        let link = t.NextArray.[level]
                        if index < link.Width then
                            search index (level - 1) t
                        else
                            search (index - link.Width) level link.Target

                if index >= 0 && index < x.Count then
                    search index (x.Root.Height - 1) x.Root
                else
                    None

            member x.TryGetIndex (t : SortKey) =
                if t.Clock <> x || t.IsDeleted then
                    -1
                else
                    let mutable index = 0
                    let mutable current = t
                    while current <> x.Root do
                        let link = t.PrevArray.[t.PrevArray.Length - 1]

                        index <- index + link.Width
                        current <- link.Target
                    index

            member x.Item
                with get (index : int) =
                    match x.TryAt index with
                        | Some t -> t 
                        | None -> raise <| IndexOutOfRangeException()

            interface IOrder with
                member x.Root = x.Root :> ISortKey
                member x.Count = x.Count

            static member New() =
                let c = Order()
                let r = SortKey(c, 1)
                c.Root <- r
                r.NextArray.[0] <- SortKeyLink(1,r)
                r.PrevArray.[0] <- SortKeyLink(1,r)
                c

            private new() = { Root = null; Count = 1 }
        end

    let create() =
        Order.New()

module DerivedOrder =
    let private random = Random()
    let randomHeight() =
        1 - random.NextDouble().Log2Int()


    type private DeletedImpl<'a>() =
        static let isDeleted =
            if typeof<IDeletableComparable>.IsAssignableFrom(typeof<'a>) then
                fun (a : 'a) -> 
                    let t = unbox<IDeletableComparable> a
                    if not (isNull t) then t.IsDeleted
                    else false
            else
                fun (a : 'a) -> false

        static member IsDeleted = isDeleted

    [<AllowNullLiteral>]
    type SortKey<'a> =
        class
            val mutable public Clock : Order<'a>
            val mutable public Tag : uint64
            val mutable public Item : 'a
            val mutable public NextArray : SortKeyLink<'a>[]
            val mutable public PrevArray : SortKeyLink<'a>[]
            val mutable public IsDeleted : bool

            member x.Time =
                x.Tag - x.Clock.Root.Tag

            member x.Height =
                x.NextArray.Length

            member x.Next = 
                if not (isNull x.NextArray) then x.NextArray.[0].Target
                else null

            member x.Prev =
                if not (isNull x.PrevArray) then x.PrevArray.[0].Target
                else null

            member x.CompareTo (o : SortKey<'a>) =
                if o.IsDeleted || x.IsDeleted then
                    failwith "cannot compare deleted times"

                if o.Clock <> x.Clock then
                    failwith "cannot compare times from different clocks"

                compare x.Time o.Time

            interface IComparable with
                member x.CompareTo o =
                    match o with
                        | :? SortKey<'a> as o -> x.CompareTo(o)
                        | _ -> failwithf "cannot compare time with %A" o

            interface IComparable<ISortKey> with
                member x.CompareTo o =
                    match o with
                        | :? SortKey<'a> as o -> x.CompareTo o
                        | _ -> failwithf "cannot compare time with %A" o

            override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x)
            override x.Equals o = System.Object.ReferenceEquals(x,o)

            interface ISortKey with
                member x.Clock = x.Clock :> IOrder
                member x.IsDeleted = x.IsDeleted
                //member x.Next = x.Next :> ISortKey

            new(c, h) = { Clock = c; Tag = 0UL; NextArray = Array.zeroCreate h; PrevArray = Array.zeroCreate h; Item = Unchecked.defaultof<_>; IsDeleted = false }
        end

    and SortKeyLink<'a> =
        struct
            val mutable public Width : int
            val mutable public Target : SortKey<'a>

            new(w,t) = { Width = w; Target = t }
        end

    and Order<'a> =
        class
            val mutable public Root : SortKey<'a>
            val mutable public Count : int
            val mutable public Comparer : IComparer<'a>

            member x.IsDeleted (item : 'a) =
                DeletedImpl<'a>.IsDeleted item

            member private x.After (t : SortKey<'a>) =
                if t.Clock <> x then
                    failwith "cannot insert after a different clock's time"

                let distance (a : SortKey<'a>) (b : SortKey<'a>) =
                    if a = b then System.UInt64.MaxValue
                    else b.Tag - a.Tag

                let mutable dn = distance t t.Next

                // if the distance to the next time is 1 (no room)
                // relabel all times s.t. the new one can be inserted
                if dn = 1UL then
                    // find a range s.t. distance(range) >= 1 + |range|^2 
                    let mutable current = t.NextArray.[0].Target
                    let mutable j = 1UL
                    while distance t current < 1UL + j * j do
                        current <- current.Next
                        j <- j + 1UL

                    // distribute all times in the range equally spaced
                    let step = (distance t current) / j
                    current <- t.NextArray.[0].Target
                    let mutable currentTime = t.Tag + step
                    for k in 1UL..(j-1UL) do
                        current.Tag <- currentTime
                        current <- current.NextArray.[0].Target
                        currentTime <- currentTime + step

                    // store the distance to the next time
                    dn <- step

                // insert the new time with distance (dn / 2) after
                // the given one (there has to be enough room now)
                let h = randomHeight()
                let res = SortKey(x, h)
                res.Tag <- t.Tag + dn / 2UL


                // since the predecessor might be "smaller" than 
                // the new node we need to search the remaining links
                // by going backward in the list
                let mutable current = t
                let mutable distance = 1

                let resize (h : int) =
                    let n = x.Root
                    if h > n.Height then
                        let additional = Array.create (h - n.Height) (SortKeyLink(x.Count,n))
                        n.NextArray <- Array.append n.NextArray additional
                        n.PrevArray <- Array.append n.PrevArray additional

                let back (n : SortKey<'a>) =
                    let link = n.PrevArray.[n.PrevArray.Length - 1]
                    (link.Width, link.Target)

                let tup (l : SortKeyLink<'a>) =
                    (l.Width, l.Target)

                for i in 0..h-1 do

                    // go backwards until a node with sufficient height is found
                    // or until we've reached the representant
                    while i >= current.Height && current <> x.Root do
                        let (d,l) = back current

                        current <- l
                        distance <- distance + d

                    // if the found node is not sufficiently high it must
                    // be the representant and therefore it has to be resized
                    if i >= current.Height then
                        resize h

                    // current must now be sufficiently high  
                    let (d,n) = tup current.NextArray.[i]

                    current.NextArray.[i] <- SortKeyLink(distance, res)
                    res.PrevArray.[i] <- SortKeyLink(distance, current)
                    res.NextArray.[i] <- SortKeyLink(1 + d - distance, n)
                    n.PrevArray.[i] <- SortKeyLink(1 + d - distance, res)

                // since the predecessor and the new node might be
                // smaller than the total height we need to increment
                // the width of all pointers "passing" the new node (above)
                let mutable current = t
                for i in h..x.Root.Height-1 do
                    while i >= current.Height && current <> x.Root do
                        let (d,l) = back current
                        current <- l

                    let (d,n) = tup current.NextArray.[i]
                    current.NextArray.[i] <- SortKeyLink(d + 1, n)
                    n.PrevArray.[i] <- SortKeyLink(d + 1, current)

                // finally increment the count and return the node
                x.Count <- x.Count + 1
                res

            member x.Get(value : 'a) : SortKey<'a> =
                if x.IsDeleted value then
                    failwith "cannot get time for deleted input-value"

                let rec findPrevAcc (acc : array<_>) (index : int) (level : int) (v : 'a) (n : SortKey<'a>) =
                    if level < 0 then 
                        (index, acc)
                    else
                        let link = n.NextArray.[level]
                        if link.Target = x.Root then
                            acc.[level] <- SortKeyLink(index, n)
                            findPrevAcc acc index (level - 1) v n 
                        else
                            if x.IsDeleted link.Target.Item then
                                x.Delete link.Target
                                findPrevAcc acc index (min (x.Root.Height-1) level) v n
                            else
                                if link.Target <> x.Root && x.Comparer.Compare(v, link.Target.Item) > 0 then
                                    let t = n.NextArray.[level].Target

                                    let level = min level (t.NextArray.Length-1)
                                    findPrevAcc acc (index + link.Width) level v t 
                                else
                                    acc.[level] <- SortKeyLink(index, n)
                                    findPrevAcc acc index (level - 1) v n 

                let ptr = Array.zeroCreate x.Root.Height
                let (index, prev) = findPrevAcc ptr 0 (x.Root.Height-1) value x.Root

                let next = prev.[0].Target
                if next <> x.Root && x.Comparer.Compare(next.Item, value) = 0 then
                    prev.[0].Target
                else
                    let tn = x.After prev.[0].Target
                    tn.Item <- value
                    tn
            
            member x.Delete (t : SortKey<'a>) = 
                if t = x.Root then failwith "tried to delete root"
                if t.IsDeleted |> not then 
                    if t.Clock <> x then
                        failwith "cannot delete time from different clock"

                    let tup (l : SortKeyLink<'a>) = (l.Width, l.Target)
                    for l in 0..t.Height-1 do
                        let (dp, p) = tup t.PrevArray.[l]
                        let (dn, n) = tup t.NextArray.[l]

                        n.PrevArray.[l] <- SortKeyLink(dp + dn - 1, p)
                        p.NextArray.[l] <- SortKeyLink(dp + dn - 1, n)

                    let mutable current = t
                    let mutable distance = 1
                    for i in t.Height..x.Root.Height-1 do
                        let back (n : SortKey<'a>) =
                            let l = n.PrevArray.[n.PrevArray.Length - 1]
                            (l.Width, l.Target)
                        // go backwards until a node with sufficient height is found
                        // or until we've reached the representant
                        while current.Height <= i && current <> x.Root do
                            let (d,l) = back current
                            current <- l
                            distance <- distance + d

                        let (dn, n) = tup current.NextArray.[i]

                        current.NextArray.[i] <- SortKeyLink(dn - 1, n)
                        n.PrevArray.[i] <- SortKeyLink(dn - 1, current)

                    // every level (except for 0) on which rep.NextArray.[level] = rep 
                    // is useless and is therefore removed
                    let rep = x.Root
                    let mutable repHeight = rep.Height
                    while repHeight > 1 && rep.NextArray.[repHeight - 1].Target = rep do
                        repHeight <- repHeight - 1

                    if repHeight < rep.Height then
                        rep.NextArray <- Array.sub rep.NextArray 0 repHeight
                        rep.PrevArray <- Array.sub rep.PrevArray 0 repHeight
        
                    x.Count <- x.Count - 1
                    t.IsDeleted <- true
//                    t.NextArray <- null
//                    t.PrevArray <- null
//                    t.Tag <- 0UL
//                    t.Clock <- Unchecked.defaultof<_>      

            member x.Clear() =
                let r = SortKey(x, 1)
                x.Root <- r
                r.NextArray.[0] <- SortKeyLink(1,r)
                r.PrevArray.[0] <- SortKeyLink(1,r)
                x.Count <- 1

            /// gets the n-th time after this one
            /// NOTE that this only works on representant-nodes
            member x.TryAt (index : int) =

                let rec search (index : int) (level : int) (t : SortKey<'a>) : Option<SortKey<'a>> =
                    if level < 0 then
                        if index = 0 then Some t
                        else None
                    else
                        let link = t.NextArray.[level]
                        if index < link.Width then
                            search index (level - 1) t
                        else
                            search (index - link.Width) level link.Target

                if index >= 0 && index < x.Count then
                    search index (x.Root.Height - 1) x.Root
                else
                    None

            member x.Item
                with get (index : int) =
                    match x.TryAt index with
                        | Some t -> t 
                        | None -> raise <| IndexOutOfRangeException()

            interface IOrder with
                member x.Root = x.Root :> ISortKey
                member x.Count = x.Count

            static member New(cmp) =
                let c = Order<'a>(cmp)
                let r = SortKey(c, 1)
                c.Root <- r
                r.NextArray.[0] <- SortKeyLink(1,r)
                r.PrevArray.[0] <- SortKeyLink(1,r)
                c

            private new(cmp) = { Root = null; Count = 1; Comparer = cmp }
        end

    let create(cmp) =
        Order.New(cmp)

type IReal =
    inherit IComparable
    abstract member InsertAfter : unit -> IReal

module RealNumber =
    open System.Threading

    [<AutoOpen>]
    module private Implementation = 
        type MonitorList() =
            let acquired = HashSet<obj>()

            member x.Add(o : obj) =
                if acquired.Add o then
                    Monitor.Enter o

            member x.Dispose() =
                for a in acquired do Monitor.Exit a

            interface IDisposable with
                member x.Dispose() = x.Dispose()

        [<AllowNullLiteral>]
        type SortKey =
            class
                val mutable public RefCount : int
                val mutable public Root : SortKey
                val mutable public Tag : uint64
                val mutable public Next : SortKey
                val mutable public Prev : SortKey

                member x.Time =
                    x.Tag - x.Root.Tag

                static member private CompareInternal(l : SortKey, r : SortKey) =
                    match Monitor.TryEnter(l), Monitor.TryEnter(r) with
                        | true, true -> 
                            let res = compare l.Time r.Time
                            Monitor.Exit l
                            Monitor.Exit r
                            res

                        | false, true -> 
                            Monitor.Exit r
                            SortKey.CompareInternal(l, r)

                        | true, false -> 
                            Monitor.Exit l
                            SortKey.CompareInternal(l, r)

                        | false, false -> 
                            SortKey.CompareInternal(l, r)

                member x.CompareTo (o : SortKey) =
                    if isNull o.Root || isNull x.Root then
                        failwith "cannot compare deleted times"

                    if o.Root != x.Root then
                        failwith "cannot compare times from different clocks"

                    SortKey.CompareInternal(x, o)

                member t.InsertAfter() =
                    use l = new MonitorList()
                    l.Add t
                    l.Add t.Next

                    let distance (a : SortKey) (b : SortKey) =
                        if a = b then System.UInt64.MaxValue
                        else b.Tag - a.Tag

                    let mutable dn = distance t t.Next

                    // if the distance to the next time is 1 (no room)
                    // relabel all times s.t. the new one can be inserted
                    if dn = 1UL then
                        // find a range s.t. distance(range) >= 1 + |range|^2 
                        let mutable current = t.Next
                        let mutable j = 1UL
                        while distance t current < 1UL + j * j do
                            l.Add current.Next
                            current <- current.Next
                            j <- j + 1UL

                        // distribute all times in the range equally spaced
                        let step = (distance t current) / j
                        current <- t.Next
                        let mutable currentTime = t.Tag + step
                        for k in 1UL..(j-1UL) do
                            current.Tag <- currentTime
                            current <- current.Next
                            currentTime <- currentTime + step

                        // store the distance to the next time
                        dn <- step

                    // insert the new time with distance (dn / 2) after
                    // the given one (there has to be enough room now)
                    let res = SortKey(t.Root, Prev = t, Next = t.Next, Tag = t.Tag + dn / 2UL)
                    t.Next.Prev <- res
                    t.Next <- res

                    res

                member x.AddRef() =
                    Interlocked.Increment(&x.RefCount) |> ignore

                member x.Delete() =
                    if Interlocked.Decrement(&x.RefCount) = 0 then
                        lock x (fun () ->
                            x.Prev.Next <- x.Next
                            x.Next.Prev <- x.Prev
                            // x.Next <- null
                            // x.Prev <- null
                            x.Root <- null
                        )

                interface IComparable<ISortKey> with
                    member x.CompareTo o =
                        match o with
                            | :? SortKey as o -> x.CompareTo o
                            | _ -> failwithf "cannot compare time with %A" o
                            

                interface IComparable with
                    member x.CompareTo o =
                        match o with
                            | :? SortKey as o -> x.CompareTo o
                            | _ -> failwithf "cannot compare time with %A" o

                override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x)
                override x.Equals o = System.Object.ReferenceEquals(x,o)


                new(c) = { RefCount = 0; Root = c; Tag = 0UL; Next = null; Prev = null }
            end

        type GCKey(k : SortKey) =
            do k.AddRef()

            override x.Finalize() =
                k.Delete()

            member x.Key = k
        
            member x.InsertAfter() = GCKey(k.InsertAfter()) :> IReal

            interface IReal with
                member x.InsertAfter() = GCKey(k.InsertAfter()) :> IReal
                member x.CompareTo o =
                    match o with
                        | :? GCKey as o -> compare k o.Key
                        | :? SortKey as o -> compare k o
                        | _ -> failwith "cannot compare"

            override x.ToString() =
               let value = float k.Time / float UInt64.MaxValue
               value.ToString(System.Globalization.CultureInfo.InvariantCulture) + "r"

            override x.GetHashCode() =
                k.GetHashCode()

            override x.Equals o =
                match o with
                    | :? GCKey as o -> k = o.Key
                    | _ -> false

    let zero = 
        let root = SortKey(null)
        root.Root <- root
        root.Next <- root
        root.Prev <- root

        GCKey(root) :> IReal

    let after (k : IReal) =
        k.InsertAfter()

    let between (l : IReal) (r : IReal) =
        let l = unbox<GCKey> l
        let r = unbox<GCKey> r

        if l >= r then
            failwith "[GCKey] negative range given"

        use locks = new MonitorList()
        locks.Add l
        locks.Add r

        if l.Key.Next == r.Key then
            l.InsertAfter()

        elif l.Key.Next < r.Key then
            GCKey(l.Key.Next) :> IReal

        else
            failwith "[GCKey] illformed range given"

[<CustomEquality; CustomComparison>]
type private SortKeyTuple =
    struct
        val mutable public K0 : ISortKey
        val mutable public K1 : ISortKey

        interface IComparable with
            member x.CompareTo o =
                match o with
                    | :? SortKeyTuple as o ->
                        let c = compare x.K0 o.K0
                        if c <> 0 then c
                        else compare x.K1 o.K1
                    | _ ->
                        failwith "uncomparable"

        override x.GetHashCode() =
            HashCode.Combine(x.K0.GetHashCode(), x.K1.GetHashCode())

        override x.Equals o =
            match o with
                | :? SortKeyTuple as o -> x.K0 = o.K0 && x.K1 = o.K1
                | _ -> false

        interface IDeletableComparable with
            member x.IsDeleted = 
                (not (isNull x.K0) && x.K0.IsDeleted) || (not (isNull x.K1) && x.K1.IsDeleted)

        new(k0, k1) = { K0 = k0; K1 = k1 }

    end

type OrderMaintenance<'a when 'a : equality>(comparer : IComparer<'a>) =
    let derived = DerivedOrder.create(comparer)
    let nodes = Dictionary<'a, DerivedOrder.SortKey<'a>>()

    member x.Count = derived.Count

    member x.Order = derived :> IOrder

    member x.Root = derived.Root :> ISortKey

    member x.Clear() =
        derived.Clear()
        nodes.Clear()

    member x.Invoke(t : 'a) =
        let res = derived.Get t
        nodes.[t] <- res
        res :> ISortKey

    member x.TryGet(t : 'a) =
        match nodes.TryGetValue t with
            | (true,v) -> Some (v :> ISortKey)
            | _ -> None

    member x.Revoke(t : 'a) =
        match nodes.TryGetValue t with
            | (true, d) ->
                derived.Delete d
                d :> ISortKey
            | _ ->
                failwith "cannot delete unknown time"

    new() = OrderMaintenance(Comparer<'a>.Default)
    new(cmp : 'a -> 'a -> int) = OrderMaintenance { new IComparer<'a> with member x.Compare(l,r) = cmp l r }

type OrderMapping() =
    inherit OrderMaintenance<ISortKey>()

type NestedOrderMapping() =
    let derived = DerivedOrder.create(Comparer<SortKeyTuple>.Default)
    let nodes = Dictionary<SortKeyTuple, DerivedOrder.SortKey<SortKeyTuple>>()

    member x.Order = derived :> IOrder

    member x.Count = derived.Count

    member x.Root = derived.Root :> ISortKey

    member x.Clear() =
        derived.Clear()
        nodes.Clear()

    member x.Invoke(outer : ISortKey, inner : ISortKey) =
        let tup = SortKeyTuple(outer,inner)
        let res = derived.Get tup
        nodes.[tup] <- res
        res :> ISortKey

    member inline private x.Revoke(tup : SortKeyTuple) =
        match nodes.TryGetValue tup with
            | (true, d) ->
                derived.Delete d
                d :> ISortKey
            | _ ->
                failwith "cannot delete unknown time"

    member x.Revoke(outer : ISortKey, inner : ISortKey) =
        let tup = SortKeyTuple(outer,inner)
        x.Revoke(tup)

    member x.RevokeAll(outer : ISortKey) =
        let all = nodes.Keys |> Seq.filter (fun tup -> tup.K0 = outer) |> Seq.toList
        all |> List.map x.Revoke


