namespace Aardvark.Base

module private HSetList =
    let rec add (cnt : byref<int>) (value : 'a) (list : list<'a>) =
        match list with
            | [] -> 
                cnt <- cnt + 1
                [value]
            | h :: tail ->
                if Unchecked.equals h value then
                    list
                else
                    h :: add &cnt value tail

    let rec remove (cnt : byref<int>) (value : 'a) (list : list<'a>) =
        match list with
            | [] ->
                []
            | h :: tail ->
                if Unchecked.equals h value then
                    cnt <- cnt - 1
                    tail
                else
                    h :: remove &cnt value tail

    let rec union (dupl : byref<int>) (l : list<'a>) (r : list<'a>) =
        let mutable d = dupl
        let newR = 
            r |> List.choose (fun r ->
                if l |> List.exists (Unchecked.equals r) then
                    d <- d + 1
                    None
                else
                    Some r
            )

        dupl <- d
        l @ newR

type hset<'a>(cnt : int, store : intmap<list<'a>>) =
    static let empty = hset(0, IntMap.empty)

    static member Empty = empty
    
    member private x.Store = store

    member x.Count = cnt

    member x.IsEmpty = cnt = 0

    member x.Add (value : 'a) =
        let hash = Unchecked.hash value
        let mutable cnt = cnt

        let newStore = 
            store |> IntMap.alter (fun o ->
                match o with
                    | None -> Some [value]
                    | Some old -> HSetList.add &cnt value old |> Some
            ) hash

        hset(cnt, newStore)

    member x.Remove (value : 'a) =
        let hash = Unchecked.hash value
        let mutable cnt = cnt
        
        let newStore = 
            store |> IntMap.alter (fun o ->
                match o with
                    | None -> None
                    | Some old -> HSetList.remove &cnt value old |> Some
            ) hash

        hset(cnt, newStore)

    member x.Contains (value : 'a) =
        let hash = Unchecked.hash value
        match IntMap.tryFind hash store with
            | Some l -> l |> List.exists (Unchecked.equals value)
            | None -> false
        

    member x.Map (mapping : 'a -> 'b) =
        hset(cnt, store |> IntMap.map (List.map mapping))

    member x.Choose (mapping : 'a -> Option<'b>) =
        let mutable cnt = 0
        let mapping a =
            match mapping a with
                | Some v -> 
                    cnt <- cnt + 1
                    Some v
                | None ->
                    None

        let newStore =
            store |> IntMap.mapOption (fun l ->
                match List.choose mapping l with
                    | [] -> None
                    | l -> Some l
            )

        hset(cnt, newStore)

    member x.Filter (predicate : 'a -> bool) =
        let mutable cnt = 0
        let predicate v =
            if predicate v then
                cnt <- cnt + 1
                true
            else
                false

        let newStore =
            store |> IntMap.mapOption (fun l ->
                match List.filter predicate l with
                    | [] -> None
                    | l -> Some l
            )

        hset(cnt, newStore)

    member x.Iter (iter : 'a -> unit) =
        store |> IntMap.toSeq |> Seq.iter (fun (_,l) -> l |> List.iter iter)

    member x.Exists (predicate : 'a -> bool) =
        store |> IntMap.toSeq |> Seq.exists (fun (_,l) -> l |> List.exists predicate)

    member x.Forall (predicate : 'a -> bool) =
        store |> IntMap.toSeq |> Seq.forall (fun (_,l) -> l |> List.forall predicate)

    member x.Fold (seed : 's, folder : 's -> 'a -> 's) =
        store |> IntMap.toSeq |> Seq.fold (fun s (_,l) ->
            l |> List.fold folder s
        ) seed

    member x.Union (other : hset<'a>) =
        let mutable dupl = 0
        let newStore = 
            IntMap.appendWith (fun l r -> HSetList.union &dupl l r) store other.Store

        hset(cnt - dupl, newStore)
