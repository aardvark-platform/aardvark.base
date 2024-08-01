module internal Aardvark.Rendering.Text.BvhImpl


open Aardvark.Base
open Aardvark.Base.Sorting
open FSharp.Data.Adaptive

type internal BvhNode3d<'K, 'V> =
    | Leaf of overflowCount : int * bounds : Box3d * values : HashMap<'K, struct(Box3d * 'V)>
    | Node of bestCost : float * count : int * bounds : Box3d * left : BvhNode3d<'K, 'V> * right : BvhNode3d<'K, 'V>

module internal BvhNode3d =

    module HashMap =
        let partition (predicate : 'K -> 'V -> bool) (map : HashMap<'K, 'V>) =
            HashMap.ApplyDelta(map, map, fun k _ v ->
                if predicate k v then struct(ValueSome v, ValueNone)
                else struct(ValueNone, ValueSome v)
            )

    [<GeneralizableValue>]
    let empty<'K, 'V> : option<BvhNode3d<'K, 'V>> = None

    let inline getBounds (n : BvhNode3d<'K, 'V>) =
        match n with
        | Leaf(_,b,_)
        | Node(_,_,b,_,_) -> b 
        
    let inline count (n : BvhNode3d<'K, 'V>) =
        match n with
        | Leaf(_,_,v) -> v.Count
        | Node(_,c,_,_,_) -> c

    let cost (invVolume : float) (lBox : Box3d) (lCnt : int) (rBox : Box3d) (rCnt : int) =
        let i = Box.Intersection(lBox, rBox)
        let iVol = if i.IsValid then i.Volume * invVolume else 0.0
        let lVol = lBox.Volume * invVolume
        let rVol = rBox.Volume * invVolume
        let cnt = lCnt + rCnt
        let l = float lCnt / float cnt
        let r = float rCnt / float cnt
        (1.0 / float cnt + (l * lVol) + (r * rVol) + iVol) / 2.0

    let split (invVolume : float) (elements : HashMap<'K, struct(Box3d * 'V)>) =
        if elements.Count <= 1 then
            None
        else
            let arr = elements.ToArrayV()
            let bounds = arr |> Array.map (fun struct(k, struct(b,v)) -> b)

            let mutable bestCost = System.Double.PositiveInfinity
            let mutable bestPerm = null
            let mutable bestSplit = -1
            let mutable bestlBox = Unchecked.defaultof<Box3d>
            let mutable bestrBox = Unchecked.defaultof<Box3d>


            for dim in 0 .. 2 do
                let getter = 
                    match dim with
                    | 0 -> fun (v : V3d) -> v.X
                    | 1 -> fun (v : V3d) -> v.Y
                    | _ -> fun (v : V3d) -> v.Z
                let mutable lBoxes = Array.zeroCreate elements.Count
                let mutable rBoxes = Array.zeroCreate elements.Count

                let perm = bounds.CreatePermutationQuickSortAscending(fun (b : Box3d) -> getter b.Center)

                let mutable last = bounds.[perm.[0]]
                lBoxes.[0] <- last
                for i in 1 .. perm.Length - 1 do
                    let b = bounds.[perm.[i]].Union(last)
                    lBoxes.[i] <- b //Box3d.Union(last, b)
                    last <- b

                let mutable i = perm.Length-1
                let mutable last = Box3d.Invalid
                while i >= 0 do
                    let b = bounds.[perm.[i]].Union(last)
                    rBoxes.[i] <- b
                    last <- b
                    i <- i - 1


                for lCnt in 1 .. perm.Length - 1 do
                    let rCnt = perm.Length - lCnt
                    let lBox = lBoxes.[lCnt-1]
                    let rBox = rBoxes.[lCnt]

                    let c = cost invVolume lBox lCnt rBox rCnt
                    if c < bestCost then
                        bestCost <- c
                        bestPerm <- perm
                        bestSplit <- lCnt
                        bestlBox <- lBox
                        bestrBox <- rBox

            if bestCost < 1.0 then
                let lCnt = bestSplit
                let rCnt = arr.Length - lCnt

                let left = Array.zeroCreate lCnt
                let right = Array.zeroCreate rCnt

                let mutable i = 0
                for li in 0 .. lCnt - 1 do
                    left.[li] <- arr.[bestPerm.[i]]
                    i <- i + 1
                
                for ri in 0 .. rCnt - 1 do
                    right.[ri] <- arr.[bestPerm.[i]]
                    i <- i + 1

                Some (bestCost, bestlBox, HashMap.ofArrayV left, bestrBox, HashMap.ofArrayV right)
            else    
                None

    let rec build (limit : int) (bounds : Box3d) (elements : HashMap<'K, struct(Box3d * 'V)>) =
        if elements.Count <= 0 then
            failwith "empty"

        elif elements.Count <= limit then
            Leaf(0, bounds, elements)

        else   
            let bv = 1.0 / bounds.Volume
            match split bv elements with
            | Some (cost, lBox, lElements, rBox, rElements) ->
                let l = build limit lBox lElements
                let r = build limit rBox rElements
                Node(cost, elements.Count, bounds, l, r)
            | None ->
                //Log.warn "bad split %d" elements.Count
                Leaf(elements.Count, bounds, elements)

    let rec getIntersecting (query : Box3d) (node : BvhNode3d<'K, 'V>) =
        match node with
        | Leaf(_, bounds, values) ->
            if bounds.Intersects query then
                values |> HashMap.filter (fun _ struct(b,_) -> b.Intersects query)
            else
                HashMap.empty
        | Node(_bestCost, _cnt, bounds, l, r) ->
            if bounds.Intersects query then
                let l = getIntersecting query l
                let r = getIntersecting query r
                HashMap.union l r
            else
                HashMap.empty
                
    let rec getIntersectingFilter (query : Box3d) (filter : OptimizedClosures.FSharpFunc<'K, Box3d, 'V, bool>) (node : BvhNode3d<'K, 'V>) =
        match node with
        | Leaf(_, bounds, values) ->
            if bounds.Intersects query then
                values |> HashMap.filter (fun k struct(b,v) -> b.Intersects query && filter.Invoke(k, b, v))
            else
                HashMap.empty
        | Node(_bestCost, _cnt, bounds, l, r) ->
            if bounds.Intersects query then
                let l = getIntersectingFilter query filter l
                let r = getIntersectingFilter query filter r
                HashMap.union l r
            else
                HashMap.empty

    let rec toSeq (n : BvhNode3d<'K, 'V>) =
        match n with
        | Leaf(_,_,vs) -> vs.ToSeqV()
        | Node(_,_,_,l,r) -> Seq.append (toSeq l) (toSeq r)

    let rec add (limit : int) (key : 'K) (bounds : Box3d) (value : 'V) (node : BvhNode3d<'K, 'V>) =
        match node with
        | Leaf(overflowCount, b, values) ->
            match HashMap.tryFindV key values with
            | ValueSome (struct(ob, _ov)) ->
                // replace
                if bounds.Contains ob then  
                    Leaf(overflowCount, b, HashMap.add key (struct(bounds, value)) values)
                else
                    let mutable bb = bounds
                    for struct(_,struct(b,_)) in values.ToSeqV() do
                        bb.ExtendBy b
                    Leaf(overflowCount, bb, HashMap.add key (struct(bounds, value)) values)
            | _ -> 
                // add
                let b = b.ExtendedBy bounds
                let vs = HashMap.add key (struct(bounds, value)) values

                if vs.Count >= 2 * overflowCount && vs.Count > limit then 
                    build limit b vs
                else 
                    Leaf(overflowCount, b, vs)

        | Node(bestCost, _, b, l, r) ->
            let nb = Box.Union(b, bounds)

            let lb = getBounds l
            let rb = getBounds r
            let lc = count l
            let rc = count r
            let invVol = 1.0 / nb.Volume
            let lCost = cost invVol (Box.Union(lb, bounds)) (1 + lc) rb rc
            let rCost = cost invVol lb lc (Box.Union(rb, bounds)) (1 + rc)

            if lCost < rCost then
                // add left
                if lCost > 2.0 * bestCost then
                    toSeq node 
                    |> HashMap.ofSeqV 
                    |> HashMap.add key (struct(bounds, value))
                    |> build limit nb
                else
                    let l = add limit key bounds value l
                    let lb = getBounds l
                    let lc = count l
                    let cc = cost invVol lb lc rb rc
                    let nb = Box.Union(lb, rb)
                    Node(min bestCost cc, lc + rc, nb, l, r)
            else
                if rCost > 2.0 * bestCost then  
                    toSeq node 
                    |> HashMap.ofSeqV 
                    |> HashMap.add key (struct(bounds, value))
                    |> build limit nb
                else
                    let r = add limit key bounds value r
                    let rb = getBounds r
                    let rc = count r
                    let cc = cost invVol lb lc rb rc
                    let nb = Box.Union(lb, rb)
                    Node(min bestCost cc, lc + rc, nb, l, r)
            
    let rec tryRemove (limit : int) (key : 'K) (bounds : Box3d) (node : BvhNode3d<'K, 'V>) =
        match node with
        | Leaf(overflowCount, b, values) ->
            if b.Intersects bounds then
                match HashMap.tryRemove key values with
                | Some (struct(_,v), n) -> 
                    if n.Count > 0 then
                        let mutable bb = Box3d.Invalid
                        for struct(_,struct(b,_)) in n.ToSeqV() do
                            bb.ExtendBy b
                        Some(v, Some (Leaf(max 0 (overflowCount - 1), bb, n)))
                    else
                        Some (v, None)
                | None ->
                    None
            else
                None

        | Node(bestCost, _, b, l, r) ->
            if b.Intersects bounds then
                match tryRemove limit key bounds l with
                | Some (v, l) ->
                    match l with
                    | Some l -> 
                        let lc = count l
                        let rc = count r
                        let lb = getBounds l
                        let rb = getBounds r
                        let o = Box.Union(lb, rb)
                        let cnt = lc + rc
                        if cnt <= limit then
                            let values = Seq.append (toSeq l) (toSeq r) |> HashMap.ofSeqV
                            Some (v, Some (Leaf(0, o, values)))
                        else
                            let cost = cost (1.0 / o.Volume) lb lc rb rc
                            Some (v, Some (Node(min cost bestCost, cnt, o, l, r)))
                    | None ->
                        Some (v, Some r)
                | None ->
                    match tryRemove limit key bounds r with
                    | Some(v,r) ->
                        match r with
                        | Some r -> 
                            let lc = count l
                            let rc = count r
                            let lb = getBounds l
                            let rb = getBounds r
                            let o = Box.Union(lb, rb)
                            let cnt = lc + rc
                            if cnt <= limit then
                                let values = Seq.append (toSeq l) (toSeq r) |> HashMap.ofSeqV
                                Some (v, Some (Leaf(0, o, values)))
                            else
                                let cost = cost (1.0 / o.Volume) lb lc rb rc
                                Some (v, Some (Node(min cost bestCost, cnt, o, l, r)))
                        | None ->
                            Some (v, Some l)
                    | None ->
                        None
            else
                None
           
    let remove (limit : int) (key : 'K) (bounds : Box3d) (node : BvhNode3d<'K, 'V>) =
        match tryRemove limit key bounds node with
        | Some (_, rest) -> rest
        | None -> Some node

type BvhTree3d<'K, 'V> private(limit : int, root : option<BvhNode3d<'K, 'V>>, keyBounds : HashMap<'K, Box3d>) =
    member internal x.Root = root

    member x.Count =
        match root with
        | Some r -> BvhNode3d.count r
        | None -> 0

    member x.Add(key : 'K, bounds : Box3d, value : 'V) =
        if bounds.IsValid then
            let keyBounds = HashMap.add key bounds keyBounds
            let newRoot = 
                match root with
                | Some r -> BvhNode3d.add limit key bounds value r
                | None -> BvhNode3d.Leaf(0, bounds, HashMap.single key (struct(bounds, value)))
            BvhTree3d(limit, Some newRoot, keyBounds)
        else    
            x
        
    member x.Remove(key : 'K) =
        match HashMap.tryRemove key keyBounds with
        | Some (bounds, keyBounds) ->
            let newRoot = 
                match root with
                | Some r -> BvhNode3d.remove limit key bounds r
                | None -> None
            BvhTree3d(limit, newRoot, keyBounds)
        | None ->
            x

    member x.TryRemove(key : 'K) =
        match HashMap.tryRemove key keyBounds with
        | Some (bounds, keyBounds) ->
            let newRoot = 
                match root with
                | Some r -> BvhNode3d.tryRemove limit key bounds r
                | None -> None
            match newRoot with
            | Some (v, newRoot) ->
                Some (v, BvhTree3d(limit, newRoot, keyBounds))
            | None ->
                None
        | None ->
            None
         
    member x.GetIntersecting(query : Box3d) =
        match root with
        | Some r ->
            BvhNode3d.getIntersecting query r
        | None ->
            HashMap.empty
            
    member x.GetIntersecting(query : Box3d, filter : 'K -> Box3d -> 'V -> bool) =
        match root with
        | Some r ->
            let opt = OptimizedClosures.FSharpFunc<_,_,_,_>.Adapt filter
            BvhNode3d.getIntersectingFilter query opt r
        | None ->
            HashMap.empty

    member x.ToSeq() =
        match root with
        | Some root -> BvhNode3d.toSeq root |> Seq.map (fun struct(k,struct(b,v)) -> k,b,v)
        | None -> Seq.empty
        
    member x.ToList() = 
        x.ToSeq() |> Seq.toList

    member x.ToArray() =
        match root with
        | Some root ->
            let arr = Array.zeroCreate x.Count
            let mutable i = 0
            for struct(k,struct(b,v)) in BvhNode3d.toSeq root do
                arr.[i] <- (k,b,v)
            arr
        | None ->
            [||]

    static member Build(limit : int, elements : seq<'K * Box3d * 'V>) =
        let mutable bb = Box3d.Invalid
        let mutable all = HashMap.empty
        let mutable keyBounds = HashMap.empty
        for (k, b, v) in elements do
            bb.ExtendBy b
            all <- HashMap.add k (struct(b, v)) all
            keyBounds <- HashMap.add k b keyBounds

        if all.Count > 0 then
            let root = BvhNode3d.build limit bb all
            BvhTree3d(limit, Some root, keyBounds)
        else
            BvhTree3d(limit, None, HashMap.empty)

    static member Empty(limit : int) = BvhTree3d<'K, 'V>(limit, None, HashMap.empty)

module BvhTree3d =

    let splitLimit = 24

    [<GeneralizableValue>]
    let empty<'K, 'V> : BvhTree3d<'K, 'V> = BvhTree3d.Empty(splitLimit)

    let inline add (key : 'K) (bounds : Box3d) (value : 'V) (t : BvhTree3d<'K, 'V>) = t.Add(key, bounds, value)
    let inline remove (key : 'K) (t : BvhTree3d<'K, 'V>) = t.Remove(key)
    let inline tryRemove (key : 'K) (t : BvhTree3d<'K, 'V>) = t.TryRemove(key)

    let inline getIntersecting (query : Box3d) (tree : BvhTree3d<'K, 'V>) = tree.GetIntersecting query

    let inline ofSeq (elements : seq<'K * Box3d * 'V>)= BvhTree3d.Build(splitLimit, elements)
    let inline ofList (elements : list<'K * Box3d * 'V>)= BvhTree3d.Build(splitLimit, elements)
    let inline ofArray (elements : array<'K * Box3d * 'V>)= BvhTree3d.Build(splitLimit, elements)
    
    let inline toSeq (tree : BvhTree3d<'K, 'V>)= tree.ToSeq()
    let inline toList (tree : BvhTree3d<'K, 'V>)= tree.ToList()
    let inline toArray (tree : BvhTree3d<'K, 'V>)= tree.ToArray()

    let union (l : BvhTree3d<'K, 'V>) (r : BvhTree3d<'K, 'V>) =
        let mutable l = l
        for (k,b,v) in toSeq r do
            l <- l.Add(k,b,v)
        l


type internal BvhNode2d<'K, 'V> =
    | Leaf of overflowCount : int * bounds : Box2d * values : HashMap<'K, struct(Box2d * 'V)>
    | Node of bestCost : float * count : int * bounds : Box2d * left : BvhNode2d<'K, 'V> * right : BvhNode2d<'K, 'V>

module internal BvhNode2d =

    module HashMap =
        let partition (predicate : 'K -> 'V -> bool) (map : HashMap<'K, 'V>) =
            HashMap.ApplyDelta(map, map, fun k _ v ->
                if predicate k v then struct(ValueSome v, ValueNone)
                else struct(ValueNone, ValueSome v)
            )

    [<GeneralizableValue>]
    let empty<'K, 'V> : option<BvhNode2d<'K, 'V>> = None

    let inline getBounds (n : BvhNode2d<'K, 'V>) =
        match n with
        | Leaf(_,b,_)
        | Node(_,_,b,_,_) -> b 
        
    let inline count (n : BvhNode2d<'K, 'V>) =
        match n with
        | Leaf(_,_,v) -> v.Count
        | Node(_,c,_,_,_) -> c

    let cost (invArea : float) (lBox : Box2d) (lCnt : int) (rBox : Box2d) (rCnt : int) =
        let i = Box.Intersection(lBox, rBox)
        let iVol = if i.IsValid then i.Area * invArea else 0.0
        let lVol = lBox.Area * invArea
        let rVol = rBox.Area * invArea
        let cnt = lCnt + rCnt
        let l = float lCnt / float cnt
        let r = float rCnt / float cnt
        (1.0 / float cnt + (l * lVol) + (r * rVol) + iVol) / 2.0

    let split (invArea : float) (elements : HashMap<'K, struct(Box2d * 'V)>) =
        if elements.Count <= 1 then
            None
        else
            let arr = elements.ToArrayV()
            let bounds = arr |> Array.map (fun struct(k, struct(b,v)) -> b)

            let mutable bestCost = System.Double.PositiveInfinity
            let mutable bestPerm = null
            let mutable bestSplit = -1
            let mutable bestlBox = Unchecked.defaultof<Box2d>
            let mutable bestrBox = Unchecked.defaultof<Box2d>


            for dim in 0 .. 1 do
                let getter = 
                    match dim with
                    | 0 -> fun (v : V2d) -> v.X
                    | _ -> fun (v : V2d) -> v.Y
                let mutable lBoxes = Array.zeroCreate elements.Count
                let mutable rBoxes = Array.zeroCreate elements.Count

                let perm = bounds.CreatePermutationQuickSortAscending(fun (b : Box2d) -> getter b.Center)

                let mutable last = bounds.[perm.[0]]
                lBoxes.[0] <- last
                for i in 1 .. perm.Length - 1 do
                    let b = bounds.[perm.[i]].Union(last)
                    lBoxes.[i] <- b //Box3d.Union(last, b)
                    last <- b

                let mutable i = perm.Length-1
                let mutable last = Box2d.Invalid
                while i >= 0 do
                    let b = bounds.[perm.[i]].Union(last)
                    rBoxes.[i] <- b
                    last <- b
                    i <- i - 1


                for lCnt in 1 .. perm.Length - 1 do
                    let rCnt = perm.Length - lCnt
                    let lBox = lBoxes.[lCnt-1]
                    let rBox = rBoxes.[lCnt]

                    let c = cost invArea lBox lCnt rBox rCnt
                    if c < bestCost then
                        bestCost <- c
                        bestPerm <- perm
                        bestSplit <- lCnt
                        bestlBox <- lBox
                        bestrBox <- rBox

            if bestCost < 1.0 then
                let lCnt = bestSplit
                let rCnt = arr.Length - lCnt

                let left = Array.zeroCreate lCnt
                let right = Array.zeroCreate rCnt

                let mutable i = 0
                for li in 0 .. lCnt - 1 do
                    left.[li] <- arr.[bestPerm.[i]]
                    i <- i + 1
                
                for ri in 0 .. rCnt - 1 do
                    right.[ri] <- arr.[bestPerm.[i]]
                    i <- i + 1

                Some (bestCost, bestlBox, HashMap.ofArrayV left, bestrBox, HashMap.ofArrayV right)
            else    
                None

    let rec build (limit : int) (bounds : Box2d) (elements : HashMap<'K, struct(Box2d * 'V)>) =
        if elements.Count <= 0 then
            failwith "empty"

        elif elements.Count <= limit then
            Leaf(0, bounds, elements)

        else   
            let bv = 1.0 / bounds.Area
            match split bv elements with
            | Some (cost, lBox, lElements, rBox, rElements) ->
                let l = build limit lBox lElements
                let r = build limit rBox rElements
                Node(cost, elements.Count, bounds, l, r)
            | None ->
                //Log.warn "bad split %d" elements.Count
                Leaf(elements.Count, bounds, elements)

    let rec getIntersecting (tests : ref<int>) (query : Box2d) (node : BvhNode2d<'K, 'V>) =
        match node with
        | Leaf(_, bounds, values) ->
            tests := !tests + 1
            if bounds.Intersects(query, 1E-20) then
                tests := !tests + values.Count
                values |> HashMap.filter (fun _ struct(b,_) -> b.Intersects query)
            else
                HashMap.empty
        | Node(_bestCost, _cnt, bounds, l, r) ->
            tests := !tests + 1
            if bounds.Intersects(query, 1E-20) then
                let l = getIntersecting tests query l
                let r = getIntersecting tests query r
                HashMap.union l r
            else
                HashMap.empty
        
    let rec getIntersectingFilter (query : Box2d) (filter : OptimizedClosures.FSharpFunc<'K, Box2d, 'V, option<'T>>) (node : BvhNode2d<'K, 'V>) =
        match node with
        | Leaf(_, bounds, values) ->
            if bounds.Intersects(query, 1E-20) then
                values |> HashMap.choose (fun k struct(b,v) -> if b.Intersects query then filter.Invoke(k, b, v) else None)
            else
                HashMap.empty
        | Node(_bestCost, _cnt, bounds, l, r) ->
            if bounds.Intersects(query, 1E-20) then
                let l = getIntersectingFilter query filter l
                let r = getIntersectingFilter query filter r
                HashMap.union l r
            else
                HashMap.empty

    let rec toSeq (n : BvhNode2d<'K, 'V>) =
        match n with
        | Leaf(_,_,vs) -> vs.ToSeqV()
        | Node(_,_,_,l,r) -> Seq.append (toSeq l) (toSeq r)

    let rec add (limit : int) (key : 'K) (bounds : Box2d) (value : 'V) (node : BvhNode2d<'K, 'V>) =
        match node with
        | Leaf(overflowCount, b, values) ->
            match HashMap.tryFindV key values with
            | ValueSome (struct(ob, _ov)) ->
                // replace
                if bounds.Contains ob then  
                    Leaf(overflowCount, b, HashMap.add key (struct(bounds, value)) values)
                else
                    let mutable bb = bounds
                    for struct(_,struct(b,_)) in values.ToSeqV() do
                        bb.ExtendBy b
                    Leaf(overflowCount, bb, HashMap.add key (struct(bounds, value)) values)
            | _ -> 
                // add
                let b = b.ExtendedBy bounds
                let vs = HashMap.add key (struct(bounds, value)) values

                if vs.Count >= 2 * overflowCount && vs.Count > limit then 
                    build limit b vs
                else 
                    Leaf(overflowCount, b, vs)

        | Node(bestCost, _, b, l, r) ->
            let nb = Box.Union(b, bounds)

            let lb = getBounds l
            let rb = getBounds r
            let lc = count l
            let rc = count r
            let invVol = 1.0 / nb.Area
            let lCost = cost invVol (Box.Union(lb, bounds)) (1 + lc) rb rc
            let rCost = cost invVol lb lc (Box.Union(rb, bounds)) (1 + rc)

            if lCost < rCost then
                // add left
                if lCost > 2.0 * bestCost then
                    toSeq node 
                    |> HashMap.ofSeqV 
                    |> HashMap.add key (struct(bounds, value))
                    |> build limit nb
                else
                    let l = add limit key bounds value l
                    let lb = getBounds l
                    let lc = count l
                    let cc = cost invVol lb lc rb rc
                    let nb = Box.Union(lb, rb)
                    Node(min bestCost cc, lc + rc, nb, l, r)
            else
                if rCost > 2.0 * bestCost then  
                    toSeq node 
                    |> HashMap.ofSeqV 
                    |> HashMap.add key (struct(bounds, value))
                    |> build limit nb
                else
                    let r = add limit key bounds value r
                    let rb = getBounds r
                    let rc = count r
                    let cc = cost invVol lb lc rb rc
                    let nb = Box.Union(lb, rb)
                    Node(min bestCost cc, lc + rc, nb, l, r)
            
    let rec tryRemove (limit : int) (key : 'K) (bounds : Box2d) (node : BvhNode2d<'K, 'V>) =
        match node with
        | Leaf(overflowCount, b, values) ->
            if b.Intersects bounds then
                match HashMap.tryRemove key values with
                | Some (struct(_,v), n) -> 
                    if n.Count > 0 then
                        let mutable bb = Box2d.Invalid
                        for struct(_,struct(b,_)) in n.ToSeqV() do
                            bb.ExtendBy b
                        Some(v, Some (Leaf(max 0 (overflowCount - 1), bb, n)))
                    else
                        Some (v, None)
                | None ->
                    None
            else
                None

        | Node(bestCost, _, b, l, r) ->
            if b.Intersects bounds then
                match tryRemove limit key bounds l with
                | Some (v, l) ->
                    match l with
                    | Some l -> 
                        let lc = count l
                        let rc = count r
                        let lb = getBounds l
                        let rb = getBounds r
                        let o = Box.Union(lb, rb)
                        let cnt = lc + rc
                        if cnt <= limit then
                            let values = Seq.append (toSeq l) (toSeq r) |> HashMap.ofSeqV
                            Some (v, Some (Leaf(0, o, values)))
                        else
                            let cost = cost (1.0 / o.Area) lb lc rb rc
                            Some (v, Some (Node(min cost bestCost, cnt, o, l, r)))
                    | None ->
                        Some (v, Some r)
                | None ->
                    match tryRemove limit key bounds r with
                    | Some(v,r) ->
                        match r with
                        | Some r -> 
                            let lc = count l
                            let rc = count r
                            let lb = getBounds l
                            let rb = getBounds r
                            let o = Box.Union(lb, rb)
                            let cnt = lc + rc
                            if cnt <= limit then
                                let values = Seq.append (toSeq l) (toSeq r) |> HashMap.ofSeqV
                                Some (v, Some (Leaf(0, o, values)))
                            else
                                let cost = cost (1.0 / o.Area) lb lc rb rc
                                Some (v, Some (Node(min cost bestCost, cnt, o, l, r)))
                        | None ->
                            Some (v, Some l)
                    | None ->
                        None
            else
                None
           
    let remove (limit : int) (key : 'K) (bounds : Box2d) (node : BvhNode2d<'K, 'V>) =
        match tryRemove limit key bounds node with
        | Some (_, rest) -> rest
        | None -> Some node

type BvhTree2d<'K, 'V> private(limit : int, root : option<BvhNode2d<'K, 'V>>, keyBounds : HashMap<'K, Box2d>) =
    member internal x.Root = root

    member x.Count =
        match root with
        | Some r -> BvhNode2d.count r
        | None -> 0

    member x.Add(key : 'K, bounds : Box2d, value : 'V) =
        if bounds.IsValid then
            let keyBounds = HashMap.add key bounds keyBounds
            let newRoot = 
                match root with
                | Some r -> BvhNode2d.add limit key bounds value r
                | None -> BvhNode2d.Leaf(0, bounds, HashMap.single key (struct(bounds, value)))
            BvhTree2d(limit, Some newRoot, keyBounds)
        else    
            x
        
    member x.Remove(key : 'K) =
        match HashMap.tryRemove key keyBounds with
        | Some (bounds, keyBounds) ->
            let newRoot = 
                match root with
                | Some r -> BvhNode2d.remove limit key bounds r
                | None -> None
            BvhTree2d(limit, newRoot, keyBounds)
        | None ->
            x

    member x.TryRemove(key : 'K) =
        match HashMap.tryRemove key keyBounds with
        | Some (bounds, keyBounds) ->
            let newRoot = 
                match root with
                | Some r -> BvhNode2d.tryRemove limit key bounds r
                | None -> None
            match newRoot with
            | Some (v, newRoot) ->
                Some (v, BvhTree2d(limit, newRoot, keyBounds))
            | None ->
                None
        | None ->
            None
         
    member x.GetIntersecting(query : Box2d) =
        match root with
        | Some r ->
            BvhNode2d.getIntersecting (ref 0) query r
        | None ->
            HashMap.empty
            
    member x.GetIntersecting(query : Box2d, filter : 'K -> Box2d -> 'V -> option<'T>) =
        match root with
        | Some r ->
            let opt = OptimizedClosures.FSharpFunc<_,_,_,_>.Adapt filter
            BvhNode2d.getIntersectingFilter query opt r
        | None ->
            HashMap.empty

    member x.ToSeq() =
        match root with
        | Some root -> BvhNode2d.toSeq root |> Seq.map (fun struct(k,struct(b,v)) -> k,b,v)
        | None -> Seq.empty
        
    member x.ToList() = 
        x.ToSeq() |> Seq.toList

    member x.ToArray() =
        match root with
        | Some root ->
            let arr = Array.zeroCreate x.Count
            let mutable i = 0
            for struct(k,struct(b,v)) in BvhNode2d.toSeq root do
                arr.[i] <- (k,b,v)
            arr
        | None ->
            [||]

    static member Build(limit : int, elements : seq<'K * Box2d * 'V>) =
        let mutable bb = Box2d.Invalid
        let mutable all = HashMap.empty
        let mutable keyBounds = HashMap.empty
        for (k, b, v) in elements do
            bb.ExtendBy b
            all <- HashMap.add k (struct(b, v)) all
            keyBounds <- HashMap.add k b keyBounds

        if all.Count > 0 then
            let root = BvhNode2d.build limit bb all
            BvhTree2d(limit, Some root, keyBounds)
        else
            BvhTree2d(limit, None, HashMap.empty)

    static member Empty(limit : int) = BvhTree2d<'K, 'V>(limit, None, HashMap.empty)



module BvhTree2d =

    let splitLimit = 24

    [<GeneralizableValue>]
    let empty<'K, 'V> : BvhTree2d<'K, 'V> = BvhTree2d.Empty(splitLimit)

    let inline add (key : 'K) (bounds : Box2d) (value : 'V) (t : BvhTree2d<'K, 'V>) = t.Add(key, bounds, value)
    let inline remove (key : 'K) (t : BvhTree2d<'K, 'V>) = t.Remove(key)
    let inline tryRemove (key : 'K) (t : BvhTree2d<'K, 'V>) = t.TryRemove(key)

    let inline getIntersecting (query : Box2d) (tree : BvhTree2d<'K, 'V>) = tree.GetIntersecting query

    let inline ofSeq (elements : seq<'K * Box2d * 'V>)= BvhTree2d.Build(splitLimit, elements)
    let inline ofList (elements : list<'K * Box2d * 'V>)= BvhTree2d.Build(splitLimit, elements)
    let inline ofArray (elements : array<'K * Box2d * 'V>)= BvhTree2d.Build(splitLimit, elements)
    
    let inline toSeq (tree : BvhTree2d<'K, 'V>)= tree.ToSeq()
    let inline toList (tree : BvhTree2d<'K, 'V>)= tree.ToList()
    let inline toArray (tree : BvhTree2d<'K, 'V>)= tree.ToArray()

    let union (l : BvhTree2d<'K, 'V>) (r : BvhTree2d<'K, 'V>) =
        let mutable l = l
        for (k,b,v) in toSeq r do
            l <- l.Add(k,b,v)
        l
            

module BvhTest =
    
    let ofSeq (limit : int) (bounds : seq<Box2d>) =
        BvhTree2d.Build(limit, bounds |> Seq.mapi (fun i b -> i,b,b))
        
    let ofSeqStupid (limit : int) (bounds : seq<Box2d>) =
        let mutable t = BvhTree2d.Empty limit
        for (i, b) in Seq.indexed bounds do
            t <- t.Add(i, b, b)
        t

    let run() =
        let randBounds = Box2d(-V2d.II, V2d.II)
        let size = 0.02
        let rand = RandomSystem()

        let randomBox() =
            Box2d.FromCenterAndSize(
                rand.UniformV2d(randBounds),
                V2d.II * rand.UniformDouble() * size
            )

        let limit = 24
        for cnt in [1024; 65536] do
            Log.start "%d boxes" cnt
            let boxes = 
                Array.init cnt (fun _ -> randomBox())

            let bruteForce (q : Box2d) =    
                let mutable res = HashSet.empty
                for i in 0 .. boxes.Length - 1 do
                    let b = boxes.[i]
                    if b.Intersects q then
                        res <- HashSet.add i res
                res

            let sw = System.Diagnostics.Stopwatch.StartNew()
            let bvh = ofSeq limit boxes
            sw.Stop()
            Log.line "build took: %A" sw.MicroTime

            sw.Restart()
            let bvhs = ofSeqStupid limit boxes
            sw.Stop()
            Log.line "incr took:  %A" sw.MicroTime
            
            sw.Restart()
            let mutable r = bvhs
            for i in 0 .. bvhs.Count - 1 do
                r <- r.Remove i
            sw.Stop()
            Log.line "clear took:  %A" sw.MicroTime

            let swb = System.Diagnostics.Stopwatch()
            let swi = System.Diagnostics.Stopwatch()
            let swf = System.Diagnostics.Stopwatch()

            let iter = 100000
            for i in 1 .. iter do
                let q = randomBox()

                swf.Start()
                let reference = bruteForce q
                swf.Stop()

                
                swb.Start()
                let build = bvh.GetIntersecting q
                swb.Stop()

                swi.Start()
                let incr = bvhs.GetIntersecting q
                swi.Stop()
                let build = HashMap.keys build
                let incr = HashMap.keys incr

                let delta = HashSet.computeDelta build reference
                if not (HashSetDelta.isEmpty delta) then
                    Log.start "build error"
                    Log.line "result: [%s]" (build |> Seq.sort |> Seq.map string |> String.concat "; ")
                    Log.line "ref:    [%s]" (reference |> Seq.sort |> Seq.map string |> String.concat "; ")
                    for op in delta do
                        match op with
                        | Add(_,i) -> Report.WarnNoPrefix("missing {0}", i)
                        | Rem(_,i) -> Report.WarnNoPrefix("wrong {0}", i)
                    Log.stop()
                    
                let delta = HashSet.computeDelta incr reference
                if not (HashSetDelta.isEmpty delta) then
                    Log.start "incr error"
                    Log.line "result: [%s]" (build |> Seq.sort |> Seq.map string |> String.concat "; ")
                    Log.line "ref:    [%s]" (reference |> Seq.sort |> Seq.map string |> String.concat "; ")
                    for op in delta do
                        match op with
                        | Add(_,i) -> Report.WarnNoPrefix("missing {0}", i)
                        | Rem(_,i) -> Report.WarnNoPrefix("wrong {0}", i)
                    Log.stop()
                    
            Log.line "force: %A" (swf.MicroTime / iter)
            Log.line "build: %A" (swb.MicroTime / iter)
            Log.line "incr:  %A" (swi.MicroTime / iter)

            Log.stop()




