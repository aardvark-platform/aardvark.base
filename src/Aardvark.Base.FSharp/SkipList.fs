namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic

[<AllowNullLiteral>]
type private Node<'a>(value : 'a, height : int) =
    let mutable height = height
    let mutable next : Link<'a>[] = Array.zeroCreate height

    member x.Height = height
    member x.Next = next
    member x.Value = value

    member x.Resize(h : int) =
        if h < height then
            failwith "cannot shrink node"
        else
            let newNext = Array.zeroCreate h
            for i in 0..height-1 do
                newNext.[i] <- next.[i]
            next <- newNext

and private Link<'a> =
    struct 
        val mutable public Target : Node<'a>
        val mutable public Width : int

        new(target, w) = { Target = target; Width = w }
    end

type SkipList<'a>(cmp : 'a -> 'a -> int) =
    let mutable root : Link<'a>[] = Array.zeroCreate 0
    let mutable count = 0
    static let random = Random()

    static let resize (height : int) (root : byref<Link<'a>[]>) =
        if height > root.Length then
            let newRoot = Array.zeroCreate height
            for i in 0..root.Length-1 do
                newRoot.[i] <- root.[i]
            root <- newRoot

    static let randomHeight() =
        let mutable height = 1
        while random.NextDouble() < 0.5 do
            height <- height + 1
        height

    static let newNode (v : 'a) =
        Node(v, randomHeight())

    static let rec findPrev (acc : list<_>) (cmp : 'a -> 'a -> int) (index : int) (level : int) (v : 'a) (n : Node<'a>) (links : Link<'a>[]) =
        if level < 0 then 
            (index, acc)
        else
            let link = links.[level]

            let vCmp = 
                if isNull link.Target then -1
                else cmp v link.Target.Value

            if vCmp <= 0 then
                findPrev ((index, n)::acc) cmp index (level - 1) v n links
            else
                let t = links.[level].Target

                findPrev acc cmp (index + link.Width) level v t t.Next

    static let rec findPrevIndex (acc : list<_>) (currentIndex : int) (level : int) (id : int) (n : Node<'a>) (links : Link<'a>[]) =
        if level < 0 then 
            (currentIndex, acc)
        else
            let link = links.[level]

            let vCmp = 
                if isNull link.Target then -1
                else compare id (currentIndex + link.Width)//link.Target.Value

            if vCmp <= 0 then
                findPrevIndex ((currentIndex, n)::acc) currentIndex (level - 1) id n links
            else
                let t = links.[level].Target
                findPrevIndex acc (currentIndex + link.Width) level id t t.Next

    let print (l : SkipList<'a>) =
        let rec cnt (n : Node<'a>) =
            if isNull n then 1
            else 1 + cnt n.Next.[0].Target

        let rec dist (l : Node<'a>) (r : Node<'a>) =
            if l = r then 0
            else 1 + dist (l.Next.[0].Target) r

        let h = root.Length
        let w = cnt root.[0].Target
        let arr = Array2D.create (w + 2) (h + 2) " "

        for y in 0..h-1 do
            let mutable current = root.[y].Target
            let mutable x = root.[y].Width
            while not (isNull current) do

                arr.[x,y] <- sprintf "%A" current.Value

                let next = current.Next.[y].Target
                x <- x + dist current next
                current <- next

        for y in 1..h do
            for x in 0..w-1 do
                printf "%s " arr.[x,h - y]
            printfn ""

    let removePtr (prev : list<int * Node<'a>>) (n : Node<'a>)  =
        let mutable level = 0
        for (id,p) in prev do
            let prevNext = if not (isNull p) then p.Next else root
            let leftLink = prevNext.[level]
            if level < n.Height then
                let rightLink = n.Next.[level]
                prevNext.[level] <- Link(rightLink.Target, leftLink.Width + rightLink.Width - 1)
            else
                prevNext.[level] <- Link(leftLink.Target, leftLink.Width - 1)

            level <- level + 1

        count <- count - 1

    let toSeq() =
        let rec toSeq (n : Node<'a>) =
            if isNull n then 
                Seq.empty
            else
                seq {
                    yield n.Value
                    yield! toSeq n.Next.[0].Target
                }
        if root.Length > 0 then
            toSeq root.[0].Target
        else
            Seq.empty


    member x.Add (v : 'a) =
        if root.Length <> 0 then
            // create a new node with random height
            let n = newNode v

            // resize the root if necessary
            resize n.Height &root

            let height = root.Length
            let (index, prev) = findPrev [] cmp 0 (height - 1) v null root

            let (_,p) = prev |> List.head
            let on = 
                if isNull p then root.[0].Target 
                else p.Next.[0].Target

            if not (isNull on) && cmp on.Value v = 0 then
                false
            else
                let fi = index + 1

                let mutable level = 0
                for (id,p) in prev do
                    let prevNext = if not (isNull p) then p.Next else root
                    let l = prevNext.[level]

                    if level < n.Height then
                        let ti = id + l.Width
                        let si = id
                        //ti - si
                        prevNext.[level] <- Link(n, fi - si)
                        n.Next.[level] <- Link(l.Target, 1 + ti - fi)
                    else
                        prevNext.[level] <- Link(l.Target, l.Width + 1)

                    level <- level + 1

                count <- count + 1
                true
        else
            // create a new node with random height
            let n = newNode v

            // resize the root if necessary
            resize n.Height &root

            for i in 0..n.Height-1 do
                root.[i] <- Link(n, 1)

            count <- count + 1
            true

    member x.Remove (v : 'a) =
        if root.Length <> 0 then
            let height = root.Length
            let (index, prev) = findPrev [] cmp 0 (height - 1) v null root

            let (_,p) = prev |> List.head
            let n = if isNull p then root.[0].Target else p.Next.[0].Target

            if not (isNull n) && cmp n.Value v = 0 then
                removePtr prev n
                true
            else
                false
        else
            false

    member x.Clear() =
        count <- 0
        root <- Array.zeroCreate 0

    member x.RemoveAt (index : int) =
        if root.Length <> 0 then
            let height = root.Length
            let (_, prev) = findPrevIndex [] 0 (height - 1) (index + 1) null root

            let (_,p) = prev |> List.head
            let n = if isNull p then root.[0].Target else p.Next.[0].Target

            if not (isNull n) then
                removePtr prev n
                true
            else
                false
        else
            false

    member x.TryAt(index : int) =
        if root.Length = 0 then
            None
        else
            let height = root.Length
            let (_,ptr) = findPrevIndex [] 0 (height - 1) (index + 1) null root
            let (id,n) = ptr |> List.head

            let link = 
                if isNull n then root.[0] 
                else n.Next.[0]

            if not (isNull link.Target) && link.Width + id = index + 1 then
                Some link.Target.Value
            else
                None

    member x.Contains (v : 'a) (l : SkipList<'a>) =
        if root.Length <> 0 then
            let height = root.Length
            let (index, prev) = findPrev [] cmp 0 (height - 1) v null root

            let (_,p) = prev |> List.head
            let n = if isNull p then root.[0].Target else p.Next.[0].Target

            if not (isNull n) && cmp n.Value v = 0 then
                true
            else
                false
        else
            false

    member x.Item
        with get (index : int) = 
            match x.TryAt index with
                | Some v -> v
                | None -> raise <| IndexOutOfRangeException()

    member x.Count = count

    interface IEnumerable with
        member x.GetEnumerator() = toSeq().GetEnumerator() :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() = toSeq().GetEnumerator()

module SkipList =

    let empty<'a when 'a : comparison> : SkipList<'a> = SkipList<'a>(compare)

    let custom (cmp : 'a -> 'a -> int) = SkipList<'a>(cmp)

    let count (s : SkipList<'a>) = s.Count

    let add (v : 'a) (l : SkipList<'a>) = l.Add v

    let remove (v : 'a) (l : SkipList<'a>) = l.Remove v

    let removeAt (index : int) (l : SkipList<'a>) = l.RemoveAt index

    let clear (l : SkipList<'a>) =
        l.Clear()

    let at (index : int) (l : SkipList<'a>) = l.TryAt index

    let contains (v : 'a) (l : SkipList<'a>) = l.Contains v

    let ofSeq (s : seq<'a>) =
        let l = empty
        for e in s do
            add e l |> ignore
        l

    let ofList (l : list<'a>) =
        ofSeq l

    let ofArray (a : 'a[]) =
        ofSeq a

    let toSeq (s : SkipList<'a>) =
        s :> seq<_>

    let toList (s : SkipList<'a>) =
        s |> Seq.toList

    let toArray (s : SkipList<'a>) =
        s |> Seq.toArray