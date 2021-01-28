#if INTERACTIVE
#r "..\\..\\..\\Bin\\Release\\Aardvark.Base.dll"
#r "..\\..\\..\\Bin\\Release\\Aardvark.Base.FSharp.dll"
#else
namespace Aardvark.Base
#endif

open Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module AVL =

    [<AllowNullLiteral>]
    [<StructuredFormatDisplay("AsString")>]
    type Node<'a> =
        class
        val mutable public Value : 'a
        val mutable public Left : Node<'a>
        val mutable public Right : Node<'a>
        val mutable public height : int

        new(value : 'a) = { Value = value;  Left = null; Right = null; height = -1 }
        new(value : 'a, l : Node<'a>, r : Node<'a>) = { Value = value;  Left = l; Right = r; height = -1 }

        member x.Height =
            if x.height < 0 then
                let l = if not (isNull x.Left) then x.Left.Height else 0
                let r = if not (isNull x.Right) then x.Right.Height else 0 
                x.height <- 1 + Fun.Max(l,r)
            x.height

        member x.Balance =
            let l = if not (isNull x.Left) then x.Left.Height else 0
            let r = if not (isNull x.Right) then x.Right.Height else 0 
            l - r

        member x.AsString =
            if not (isNull x.Left) || not (isNull x.Right) then
                let l = if not (isNull x.Left) then x.Left.AsString else "Nil"
                let r = if not (isNull x.Right) then x.Right.AsString else "Nil"
                sprintf "Node(%A, %s, %s)" x.Value l r
            else
                sprintf "Leaf(%A)" x.Value

        end
        
    [<StructuredFormatDisplay("AsString")>]
    type Tree<'a> =
        class
            val mutable public Root : Node<'a>
            val public cmp : 'a -> 'a -> int

            new(cmp) = { cmp = cmp; Root = null }


            member x.AsString = if not (isNull x.Root) then x.Root.AsString else "Nil"

        end

    let private (|Nil|Node|) (n : Node<'a>) =
        if isNull n then Nil
        else Node(n.Value)

    let private rebalance (n : byref<Node<'a>>) =
        let b = if not (isNull n) then n.Balance else 0
        if b = 2 then
            let l = n.Left
            if l.Balance >= 0 then
                //left-left
                let ll = l.Left
                n <- Node(l.Value, ll, Node(n.Value, l.Right, n.Right))
            elif l.Balance = -1 then
                //left-right
                let lr = l.Right
                n <- Node(lr.Value, Node(l.Value, l.Left, lr.Left), Node(n.Value, lr.Right, n.Right))

        elif b = -2 then
            let r = n.Right
            if r.Balance <= 0 then
                //right-right
                let rr = r.Right
                n <- Node(r.Value, Node(n.Value, n.Left, r.Left), rr)
            elif r.Balance = 1 then
                //right-left
                let rl = r.Left
                n <- Node(rl.Value, Node(n.Value, n.Left, rl.Left), Node(r.Value, rl.Right, r.Right))

    let rec private ninsert' (cmp : 'a -> 'a -> int) (n : byref<Node<'a>>) (value : 'a) =
        match n with
            | Node(v) ->
                let c = cmp v value
                if c = 0 then false
                elif c > 0 then 
                    if ninsert' cmp &n.Left value then
                        n.height <- -1
                        rebalance &n
                        true
                    else false
                else 
                    if ninsert' cmp &n.Right value then
                        n.height <- -1
                        rebalance &n
                        true
                    else false
            | Nil ->
                n <- Node(value)
                true

    let rec private ninsertNeighbourhood' (prev : Option<'a>) (next : Option<'a>) (cmp : 'a -> 'a -> int) (n : byref<Node<'a>>) (value : 'a) (f : Option<'a> -> Option<'a> -> unit) =
        match n with
            | Node(v) ->
                let c = cmp v value
                if c = 0 then false
                elif c > 0 then 
                    if ninsertNeighbourhood' prev (Some v) cmp &n.Left value f then
                        n.height <- -1
                        rebalance &n
                        true
                    else false
                else 
                    if ninsertNeighbourhood' (Some v) next cmp &n.Right value f then
                        n.height <- -1
                        rebalance &n
                        true
                    else false
            | Nil ->
                n <- Node(value)
                f prev next
                true

    let rec private ninsertOrUpdateNeighbourhood' (prev : Option<'a>) (next : Option<'a>) (cmp : 'a -> 'a -> int) (n : byref<Node<'a>>) (value : 'a) (f : Option<'a> -> Option<'a> -> Option<'a> -> unit) =
        match n with
            | Node(v) ->
                let c = cmp v value
                if c = 0 then 
                    f prev (Some v) next
                    false
                elif c > 0 then 
                    if ninsertOrUpdateNeighbourhood' prev (Some v) cmp &n.Left value f then
                        n.height <- -1
                        rebalance &n
                        true
                    else false
                else
                    if ninsertOrUpdateNeighbourhood' (Some v) next cmp &n.Right value f then
                        n.height <- -1
                        rebalance &n
                        true
                    else false

            | Nil ->
                f prev None next
                n <- Node(value)
                true


    let rec private nextractLeftmost' (n : byref<Node<'a>>) =
        match n with
            | Nil -> None
            | _ -> 
                if isNull n.Left then
                    let v = n.Value
                    n <- n.Right
                    Some v
                else
                    let r = nextractLeftmost' &n.Left
                    if r.IsSome then
                        n.height <- -1
                        rebalance &n
                    r

    let rec private nextractRightmost' (n : byref<Node<'a>>) =
        match n with
            | Nil -> None
            | _ -> 
                if isNull n.Right then
                    let v = n.Value
                    n <- n.Left
                    Some v
                else
                    let r = nextractRightmost' &n.Right
                    if r.IsSome then
                        n.height <- -1
                        rebalance &n
                    r


    let rec private nremove' (n : byref<Node<'a>>) (valueCmp : 'a -> int) =
        match n with
            | Nil -> false
            | Node(v) ->
                let c = valueCmp v
                if c = 0 then
                    match n.Left, n.Right with
                        | Nil,r -> 
                            n <- r
                            true
                        | l,Nil -> 
                            n <- l
                            true
                        | _ -> 
                            match nextractLeftmost' &n.Right with
                            | Some v -> 
                                n <- Node(v, n.Left, n.Right)
                                rebalance &n
                                true
                            | None ->
                                match nextractRightmost' &n.Left with
                                    | Some v -> 
                                        n <- Node(v, n.Left, n.Right)
                                        rebalance &n
                                        true
                                    | None -> failwith "not possible"

                elif c < 0 then
                    if nremove' &n.Left valueCmp then
                        n.height <- -1
                        rebalance &n
                        true
                    else
                        false
                else
                    if nremove' &n.Right valueCmp then
                        n.height <- -1
                        rebalance &n
                        true
                    else
                        false

    let rec private ntoSeq' (n : Node<'a>) : seq<'a> =
        seq {
            match n with
                | Node(v) -> 
                    yield! n.Left |> ntoSeq'
                    yield v
                    yield! n.Right |> ntoSeq'
                | _ -> ()
        }

    let rec private nextractMinimalWhere' (condition : 'a -> bool) (n : byref<Node<'a>>) =
        match n with
            | Nil -> None
            | Node(v) ->
                let c = condition v
                if c then
                    if isNull n.Left then 
                        let v = n.Value
                        n <- n.Right
                        Some v
                    else
                        let r = nextractMinimalWhere' condition &n.Left
                        if r.IsSome then
                            n.height <- -1
                            rebalance &n
                            r
                        else
                            let v = n.Value
                                
                            if isNull n.Right then
                                n <- n.Left
                            else
                                match nextractLeftmost' &n.Right with
                                | Some v -> 
                                    n <- Node(v, n.Left, n.Right)
                                    rebalance &n
                                | None ->
                                    match nextractRightmost' &n.Left with
                                        | Some v -> 
                                            n <- Node(v, n.Left, n.Right)
                                            rebalance &n
                                        | None -> failwith "not possible"

                            Some v
                else
                    if isNull n.Right then 
                        None
                    else
                        let r = nextractMinimalWhere' condition &n.Right
                        if r.IsSome then
                            n.height <- -1
                            rebalance &n
                        r

    let rec private nfindMinimalWhere' (condition : 'a -> bool) (n : Node<'a>) =
        match n with
            | Nil -> None
            | Node(v) ->
                let c = condition v
                if c then
                    if isNull n.Left then 
                        Some n.Value
                    else
                        let r = nfindMinimalWhere' condition n.Left
                        if r.IsSome then
                            r
                        else
                            Some n.Value
                else
                    if isNull n.Right then 
                        None
                    else
                        nfindMinimalWhere' condition n.Right

    let rec private nfindMaximalWhere' (condition : 'a -> bool) (n : Node<'a>) =
        match n with
            | Nil -> None
            | Node(v) ->
                let c = condition v
                if c then
                    if isNull n.Right then 
                        Some n.Value
                    else
                        let r = nfindMaximalWhere' condition n.Right
                        if r.IsSome then
                            r
                        else
                            Some n.Value
                else
                    if isNull n.Left then 
                        None
                    else
                        nfindMaximalWhere' condition n.Left


    let rec private nextractMaximalWhere' (condition : 'a -> bool) (n : byref<Node<'a>>) =
        match n with
            | Nil -> None
            | Node(v) ->
                let c = condition v
                if c then
                    if isNull n.Right then 
                        let v = n.Value
                        n <- n.Left
                        Some v
                    else
                        let r = nextractMaximalWhere' condition &n.Right
                        if r.IsSome then
                            n.height <- -1
                            rebalance &n
                            r
                        else
                            let v = n.Value
                                
                            if isNull n.Left then
                                n <- n.Right
                            else
                                match nextractLeftmost' &n.Right with
                                | Some v -> 
                                    n <- Node(v, n.Left, n.Right)
                                    rebalance &n
                                | None ->
                                    match nextractRightmost' &n.Left with
                                        | Some v -> 
                                            n <- Node(v, n.Left, n.Right)
                                            rebalance &n
                                        | None -> failwith "not possible"

                            Some v
                else
                    if isNull n.Left then 
                        None
                    else
                        let r = nextractMaximalWhere' condition &n.Left
                        if r.IsSome then
                            n.height <- -1
                            rebalance &n
                        r

    let rec private nfind' (cmp : 'a -> int) (n : Node<'a>) =
        match n with
            | Nil -> None
            | Node(v) ->
                let c = cmp v
                if c = 0 then Some v
                elif c < 0 then nfind' cmp n.Left
                else nfind' cmp n.Right

    let private nprint' (n : Node<'a>) =
        if isNull n then printfn "Nil"
        else
            let h = n.Height + 1
            let data = System.Collections.Generic.Dictionary<int, Option<'a>[]>()
            let set (x : int) (y : int) (v : 'a) =
                match data.TryGetValue x with
                    | (true, arr) -> arr.[y] <- Some v
                    | _ -> let arr = Array.create h None
                           arr.[y] <- Some v
                           data.[x] <- arr

            let rec dump (l : int) (x : int) (n : Node<'a>) =
                match n with
                    | Node _ ->
                        if not (isNull n.Left) || not (isNull n.Right) then
                            set x l n.Value
                            dump (l + 1) (x - (1 <<< (1 + h - l))) n.Left
                            dump (l + 1) (x + (1 <<< (1 + h - l))) n.Right
                        else 
                            set x l n.Value
                    | Nil -> ()

            dump 0 0 n

            let nonEmpty = data |> Seq.sortBy (fun (KeyValue(k,v)) -> k) |> Seq.map (fun (KeyValue(_,v)) -> v) |> Seq.toArray

            let maxLength = ref 0
            let strings = nonEmpty |> Array.map (fun a -> a |> Array.map (fun o ->
                            match o with
                                | None -> None
                                | Some v -> 
                                    let str = sprintf " %A " v
                                    if str.Length > !maxLength then maxLength := str.Length
                                    Some str
                            ))

            let padToLength (s : string) =
                if s.Length < !maxLength then
                    s + System.String(' ', !maxLength - s.Length)
                else
                    s

            let strings = strings |> Array.map (fun a -> a |> Array.map (fun o ->
                            match o with
                                | Some s -> padToLength s
                                | None -> System.String(' ', !maxLength)
                            ))

            for y in 0..h-1 do
                for x in 0..strings.Length-1 do
                    printf  "%s" strings.[x].[y]
                printf "\r\n"


    /// <summary>
    /// creates a new empty AVL-Tree using the given comparison function
    /// note that the given function must define a total order and may only 
    /// return {-1,0,1}. Futhermore cmp(a,b) = 0 must be equivalent to a = b.
    /// </summary>
    let custom cmp = Tree(cmp)

    /// <summary>
    /// creates a new empty AVL-Tree using the default comparison function
    /// </summary>
    let empty<'a when 'a : comparison> =
        Tree(compare<'a>)

    /// <summary>
    /// inserts a new value into the tree and returns true if the value was
    /// not already present in the tree. 
    /// Runtime: O(log(N))
    /// </summary>
    let insert (t : Tree<'a>) (value : 'a) =
        ninsert' t.cmp &t.Root value

    let insertNeighbourhood (t : Tree<'a>) (value : 'a) (f : Option<'a> -> Option<'a> -> unit) =
        ninsertNeighbourhood' None None t.cmp &t.Root value f

    let insertOrUpdateNeighbourhood (t : Tree<'a>) (value : 'a) (f : Option<'a> -> Option<'a> -> Option<'a> -> unit) =
        ninsertOrUpdateNeighbourhood' None None t.cmp &t.Root value f


    /// <summary>
    /// inserts a new value into the tree if not already present.
    /// Runtime: O(log(N))
    /// </summary>
    let insert' (t : Tree<'a>) (value : 'a) =
        ninsert' t.cmp &t.Root value |> ignore

    /// <summary>
    /// removes a value from the tree and returns true if the value was found.
    /// Runtime: O(log(N))
    /// </summary>
    let remove (t : Tree<'a>) (value : 'a) =
        nremove' &t.Root (fun v -> t.cmp value v)

    /// <summary>
    /// removes a value from the tree if present.
    /// Runtime: O(log(N))
    /// </summary>
    let remove' (t : Tree<'a>) (value : 'a) =
        nremove' &t.Root (fun v -> t.cmp value v) |> ignore


    /// <summary>
    /// removes a value using a partially applied comparison function.
    /// the given function must perform comparison like: compare(myValue, treeValue).
    /// returns true if the element was found and false otherwise
    /// Runtime: O(log(N))
    /// </summary>
    let removeCmp (t : Tree<'a>) (cmpValue : 'a -> int) =
        nremove' &t.Root cmpValue

    /// <summary>
    /// creates a sequence containing all the elements in the tree.
    /// the sequence is ascendingly sorted with respect to the given comparison function.
    /// Runtime: O(1) [Note that traversing the entire sequence is of course in O(N)]
    /// </summary>
    let toSeq (t : Tree<'a>) =
        ntoSeq' t.Root

    /// <summary>
    /// creates a list containing all the elements in the tree.
    /// the list is ascendingly sorted with respect to the given comparison function.
    /// Runtime: O(N)
    /// </summary>
    let toList (t : Tree<'a>) =
        t.Root |> ntoSeq' |> Seq.toList

    /// <summary>
    /// finds and removes the minimal value from the tree for which the given condition holds. 
    /// it is assumed that this condition is transitive with respect to the given comparison function.
    /// returns the (optional) value found in the tree.
    /// Example: extractMinimalWhere (fun v -> v > 10) gives the minimal value in the tree being greater than 10.
    /// Runtime: O(log(N))
    ///</summary>
    let extractMinimalWhere (condition : 'a -> bool) (t : Tree<'a>) =
        nextractMinimalWhere' condition &t.Root

    /// <summary>
    /// finds and removes the maximum value from the tree for which the given condition holds. 
    /// it is assumed that this condition is transitive with respect to the given comparison function.
    /// returns the (optional) value found in the tree.
    /// Example: extractMaximalWhere (fun v -> v > 10) gives the maximal value in the tree being greater than 10.
    /// Runtime: O(log(N))
    ///</summary>
    let extractMaximalWhere (condition : 'a -> bool) (t : Tree<'a>) =
        nextractMaximalWhere' condition &t.Root


    let findMinimalWhere (condition : 'a -> bool) (t : Tree<'a>) =
        nfindMinimalWhere' condition t.Root

    let findMaximalWhere (condition : 'a -> bool) (t : Tree<'a>) =
        nfindMaximalWhere' condition t.Root



    /// <summary>
    /// finds a value using a partially applied comparison function.
    /// the given function must perform comparison like: compare(myValue, treeValue).
    /// returns the (optional) value found
    /// Runtime: O(log(N))
    /// </summary>
    let find (cmp : 'a -> int) (t : Tree<'a>) =
        nfind' cmp t.Root

    let get (element : 'a) (tree : Tree<'a>) =
        nfind' (fun e -> tree.cmp element e) tree.Root

    /// <summary>
    /// prints the tree to the console for debugging purposes.
    /// </summary>
    let print (t : Tree<'a>) =
        if isNull t.Root then printfn "Nil"
        else nprint' t.Root

    /// <summary>
    /// runs a randomized series of additions / removals on a tree and 
    /// validates its correctness, completeness, order and internal cache-values.
    /// </summary>
    let runTests() =
        let elements = Array.init 10000 (fun i -> i)
        let elements = System.Collections.Generic.List(elements)
        let used = System.Collections.Generic.List<int>()
        let tree = custom (fun (a : int) (b : int) -> a.CompareTo b)

        let checkCorrectness() =
            let l = toList tree
            let correct = l |> Seq.fold (fun a b -> a && used.Contains b) true
            if not correct then
                printfn "ERROR: removed elements are still in the tree"

        let checkCompleteness() =
            let l = System.Collections.Generic.HashSet(tree |> toList)
            let correct = used |> Seq.fold (fun a b -> a && l.Contains b) true
            if not correct then
                printfn "ERROR: elements are missing in the tree"

        let checkSorting() =
            let list = System.Collections.Generic.List(tree |> toList)
                
            let mutable error = false
            for i in 1..list.Count-1 do
                let l = list.[i-1]
                let r = list.[i]
                if l > r then
                    error <- true

                     
            if error then
                printfn "ERROR: elements are not ascending"

        let checkBalanceAndHeightCaches() =
            let rec h (n : Node<int>) =
                match n with
                    | Nil -> 0
                    | Node _ ->
                        let l = if not (isNull n.Left) then h n.Left else 0
                        let r = if not (isNull n.Right) then h n.Right else 0
                        1 + Fun.Max(l,r)

            let rec balanceAndHeightValid(n : Node<int>) =
                if not (isNull n) then
                    let mine = h n
                    let cache = n.Height
                    if mine <> cache then false
                    else
                        let b = (h n.Left) - (h n.Right)
                        let cache = n.Balance
                        if b <> cache || b >= 2 then false
                        else true
                else
                    true

            if not <| balanceAndHeightValid tree.Root then
                printfn "ERROR: invalid balance or height cache"

        let checkFind() =
            for u in used do
                match tree |> find (fun b -> compare u b) with
                    | Some v -> ()
                    | None -> printfn "ERROR: find could not find element: %d" u

            for e in elements do
                match tree |> find (fun b -> compare e b) with
                    | Some v -> printfn "ERROR: found element not in tree: %d" e
                    | None -> ()
 

        let mutable iter = 0
        let mutable maxCount = 0
        let r = System.Random()

        let mutable removeCount = 0
        let mutable extractMinimalWhereCount = 0
        let mutable extractMaximalWhereCount = 0
        let mutable insertCount = 0

        for i in 0..10000 do
            let u = r.NextDouble()

            if u < 0.5 && used.Count > 0 && iter > 500 then
                let id = r.Next(used.Count)
                let element = used.[id]
                used.RemoveAt id
                elements.Add element

                let r = r.NextDouble()
                if r < 0.3333 then
                    if (extractMinimalWhere (fun e -> e >= element) tree).IsNone then printfn "ERROR: extractMinimalWhere returned false although the element was present"
                    extractMinimalWhereCount <- extractMinimalWhereCount + 1
                elif r < 0.6666 then
                    if (extractMaximalWhere (fun e -> e <= element) tree).IsNone then printfn "ERROR: extractMaximalWhere returned false although the element was present"
                    extractMaximalWhereCount <- extractMaximalWhereCount + 1
                else
                    if not <| remove tree element then printfn "ERROR: remove returned false although the element was present"
                    removeCount <- removeCount + 1
            else
                let id = r.Next(elements.Count)
                let element = elements.[id]
                elements.RemoveAt id
                used.Add element
                if not <| insert tree element then printfn "ERROR: insert returned false although the element was not present"
                maxCount <- Fun.Max(maxCount, used.Count)
                insertCount <- insertCount + 1

            iter <- iter + 1
            checkCorrectness()
            checkCompleteness()
            checkSorting()
            checkBalanceAndHeightCaches()
            checkFind()

        
        printfn "tree contained up to %d elements" maxCount
        printfn "    insert:              %d" insertCount
        printfn "    remove:              %d" removeCount
        printfn "    extractMinimalWhere: %d" extractMinimalWhereCount 
        printfn "    extractMaximalWhere: %d" extractMaximalWhereCount 
        printfn "    current-count: %d (expected %d)" (tree |> toList).Length (insertCount - removeCount - extractMaximalWhereCount - extractMinimalWhereCount)
        printfn "test finished"

    let test() =
        let t = empty
            
        insert t 5 |> ignore
        insert t 6 |> ignore
        insert t 7 |> ignore
        insert t 8 |> ignore
        insert t 9 |> ignore
        insert t 10 |> ignore

        printfn "%s" t.AsString
        print t

        let test = extractMinimalWhere (fun v -> v > 5) t
        let test2 = extractMaximalWhere (fun v -> v < 10) t

        printfn "minimal > 5 = %A" test
        printfn "maximal < 10 = %A" test2

        remove t 10 |> ignore

        printfn "%s" t.Root.AsString


module BucketAVL =
    open System.Collections.Generic

    [<AllowNullLiteral>]
    type private LinkedListNode<'a> =
        class
            val mutable public Value : 'a
            val mutable public Next : LinkedListNode<'a>
            val mutable public Prev : LinkedListNode<'a>
            
            new(value : 'a) = { Value = value; Next = null; Prev = null }
        end

    type private Bucket<'a when 'a : equality>() =
        let mutable representative : Option<'a> = None
        let mutable root : Option<LinkedListNode<'a>> = None
        let references = Dictionary<'a, LinkedListNode<'a>>()
        let mutable next : Option<Bucket<'a>> = None
        let mutable prev : Option<Bucket<'a>> = None

        member x.AsSeq =
            match root with
                | Some r ->
                    seq {
                        yield r.Value
                        let current = ref r.Next
                            
                        while !current <> r do
                            yield current.Value.Value
                            current := current.Value.Next
                    }
                | None ->
                    Seq.empty

        member x.Next 
            with get() = next
            and set v = next <- v

        member x.Prev 
            with get() = prev
            and set v = prev <- v

        member x.Value = representative.Value

        member x.Add(value : 'a) =
            if references.ContainsKey value then
                false
            else
                representative <- Some value
                let node = LinkedListNode(value)
                references.Add(value, node)

                match root with
                    | Some r -> 
                        let last = r.Prev
                        node.Prev <- last
                        node.Next <- r
                        last.Next <- node
                        r.Prev <- node
                    | None ->
                        node.Prev <- node
                        node.Next <- node
                        root <- Some node
                true

        member x.Remove(value : 'a) =
            match references.TryGetValue value with
                | (true, r) ->
                    if r = root.Value then
                        if r.Next = r then
                            root <- None
                        else
                            root <- Some r.Next

                        

                    r.Prev.Next <- r.Next
                    r.Next.Prev <- r.Prev
                    references.Remove value |> ignore

                    //if the representative is removed change it to some other one
                    //if the bucket gets empty maintain the representative since
                    //it is needed for the tree-removal. (the bucket will be removed anyways)
                    if value = representative.Value && references.Count > 0 then
                        representative <- Some root.Value.Value
                        
                    true

                | _ -> false

        member x.FirstElement = root.Value.Value
        member x.LastElement = 
            let last : LinkedListNode<'a> = root.Value.Prev
            last.Value
        member x.Count = references.Count

    type BucketTree<'a when 'a : equality> = private { cmp : 'a -> 'a -> int; tree : AVL.Tree<Bucket<'a>> }

    let custom cmp = { cmp = cmp; tree = AVL.custom (fun (l : Bucket<'a>) (r : Bucket<'a>) -> cmp l.Value r.Value) }
                              
    let insert (t : BucketTree<'a>) (value : 'a) =
        match AVL.find (fun (o : Bucket<'a>) -> t.cmp value o.Value) t.tree with
            | Some b -> b.Add value
            | None ->
                let b = Bucket<'a>()
                b.Add value |> ignore
                AVL.insertNeighbourhood t.tree b (fun prev next -> 
                    b.Prev <- prev
                    b.Next <- next
                    match next with Some n -> n.Prev <- Some b | _ -> ()
                    match prev with Some p -> p.Next <- Some b | _ -> ()
                )

    let insertNeighbourhood (t : BucketTree<'a>) (value : 'a) (f : Option<'a> -> Option<'a> -> unit) =
        match t.tree |> AVL.find (fun o -> t.cmp value o.Value) with
            | Some b ->
                let last = Some b.LastElement
                let next = match b.Next with | Some b -> Some b.FirstElement | _ -> None
                if b.Add value then
                    f last next
                    true
                else
                    false
            | _ ->
                let b = Bucket<'a>()
                b.Add value |> ignore
                AVL.insertNeighbourhood t.tree b (fun prev next ->
                    b.Prev <- prev
                    b.Next <- next
                    match next with Some n -> n.Prev <- Some b | _ -> ()
                    match prev with Some p -> p.Next <- Some b | _ -> ()
                        
                        
                    let prev = match prev with | Some p -> Some p.LastElement | None -> None
                    let next = match next with | Some n -> Some n.FirstElement | None -> None

                        

                    f prev next
                )

    let remove (t : BucketTree<'a>) (value : 'a) =
        match AVL.find (fun (o : Bucket<'a>) -> t.cmp value o.Value) t.tree with
            | Some b ->
                if b.Remove value then
                    if b.Count = 0 then
                        if AVL.remove t.tree b then
                            match b.Prev with
                                | Some p -> p.Next <- b.Next
                                | _ -> ()
                            match b.Next with
                                | Some n -> n.Prev <- b.Prev
                                | _ -> ()
                            true
                        else
                            false
                    else
                        true
                else
                    false
            | None -> false
//
//        let extractMinimalWhere (condition : 'a -> bool) (t : BucketTree<'a>) =
//            match t.tree |> AVL.findMinimalWhere (fun b -> condition b.Value)  with
//                | Some b -> 
//                    let some = b.Value
//                    b.Remove some |> ignore
//                    if b.Count = 0 then
//                        AVL.remove t.tree b |> ignore
//                    Some some
//                | None -> None
//
//        let extractMaximalWhere (condition : 'a -> bool) (t : BucketTree<'a>) =
//            match t.tree |> AVL.findMaximalWhere (fun b -> condition b.Value)  with
//                | Some b -> 
//                    let some = b.Value
//                    b.Remove some |> ignore
//                    if b.Count = 0 then
//                        AVL.remove t.tree b |> ignore
//                    Some some
//                | None -> None

    let toSeq (t : BucketTree<'a>) =
        t.tree |> AVL.toSeq |> Seq.collect (fun b -> b.AsSeq)

    let toList (t : BucketTree<'a>) =
        t |> toSeq |> Seq.toList

    let toArray (t : BucketTree<'a>) =
        t |> toSeq |> Seq.toArray


    let runTests() =
        let t = custom (fun l r -> compare (l % 10) (r % 10))

        let numbers = Array.init 1000 id
            

        for n in numbers do
            if not <| insertNeighbourhood t n (fun l r -> ()) then
                printfn "ERROR: insert of %A failed" n

        let content = toArray t
        if content.Length <> numbers.Length then
            printfn "elements missing"
