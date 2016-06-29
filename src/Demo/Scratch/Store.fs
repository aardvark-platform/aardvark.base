module StoreTest

open System
open System.Text
open System.Diagnostics
open System.Security.Cryptography
open Aardvark.Base
open Aardvark.Base.Native
open System.IO
open Microsoft.FSharp.NativeInterop
open System.Threading

#nowarn "9"

let hash = MD5.Create()
let md5 (str : string) =
    str |> Encoding.Unicode.GetBytes |> hash.ComputeHash |> Guid

let perf() =
    let path = @"C:\Users\schorsch\Desktop\test.bin"
    if File.Exists path then File.Delete path
    let sw = Stopwatch()
    let mem = Memory.mapped path

    let ids = Array.init (1 <<< 20) (fun i -> i |> string |> md5)

    Log.startTimed "import"
    sw.Restart()
    use store = new Store(mem)
    sw.Stop()

    Log.line "memory:       %A" store.DataSize
    Log.line "fileTable:    %A" store.FileTableSize
    if store.FileCount > 0 then
        Log.line "files:        %d" store.FileCount
        Log.line "per file:     %A" (sw.MicroTime / store.FileCount)
    Log.stop()

    Log.startTimed "get"
    sw.Restart()
    let files = ids |> Array.map store.Get
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()


    let data : byte[] = Array.zeroCreate 44

    Log.startTimed "write"
    sw.Restart()
    for fi in 0..files.Length-1 do
        files.[fi].Write data
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()




    ()




module FS = 
    open System.Runtime.InteropServices

    [<AutoOpen>]
    module StoreTypes =
        let inline (++) (ptr : nativeptr<'a>) (v : 'a) = NativePtr.add ptr (int v)
        let inline (!!) (ptr : nativeptr<'a>) = NativePtr.read ptr
        let inline (<--) (ptr : nativeptr<'a>) c = NativePtr.write ptr c


        let magic = Guid("CFD123CA-17BC-437A-9EFC-6EECE27188FF")

        [<Literal>]
        let Red = 0

        [<Literal>]
        let Black = 1

        [<StructLayout(LayoutKind.Explicit, Size = 32)>]
        type Block =
            struct
                [<FieldOffset(0)>]
                val mutable public Offset   : int64
                [<FieldOffset(8)>]
                val mutable public Size     : int64
                [<FieldOffset(16)>]
                val mutable public IsFree   : int
                [<FieldOffset(20)>]
                val mutable public Prev     : int
                [<FieldOffset(24)>]
                val mutable public Next     : int


                override x.ToString() =
                    sprintf "{ Offset = %d; Size = %d; IsFree = %A; Prev = %d; Next = %d }" x.Offset x.Size (x.IsFree <> 0) x.Prev x.Next

            end

        [<StructLayout(LayoutKind.Explicit, Size = 64)>]
        type Entry =
            struct
                [<FieldOffset(0)>]
                val mutable public Block : Block
            
                [<FieldOffset(32)>]
                val mutable public Index : int
            
                [<FieldOffset(36)>]
                val mutable public Key : Guid

                [<FieldOffset(52)>]
                val mutable public HashCode : int

                [<FieldOffset(56)>]
                val mutable public NextEntry : int
            end
        
        [<StructLayout(LayoutKind.Explicit, Size = 32)>]
        type Node =
            struct  
                [<FieldOffset(0)>]
                val mutable public Offset : int64
                [<FieldOffset(8)>]
                val mutable public Size : int64
                [<FieldOffset(16)>]
                val mutable public Entry : int
                [<FieldOffset(20)>]
                val mutable public Left : int
                [<FieldOffset(24)>]
                val mutable public Right : int
                [<FieldOffset(28)>]
                val mutable public Color : int
            end

        [<StructLayout(LayoutKind.Sequential, Size = 128)>]
        type Header =
            struct
                val mutable public Magic        : Guid      // 16
                val mutable public MFirst       : int       // 20
                val mutable public MLast        : int       // 24
                val mutable public MCapacity    : int64     // 32
                val mutable public NRoot        : int       // 36
                val mutable public NCount       : int       // 40
                val mutable public NFreeList    : int       // 44
                val mutable public NFreeCount   : int       // 48
                val mutable public ECount       : int       // 52
                val mutable public EFreeList    : int       // 56
                val mutable public EFreeCount   : int       // 60
                val mutable public DCapacity    : int       // 64
                val mutable public DCapacityId  : int       // 68
            end

        type blockptr =
            struct
                val mutable public ptr : nativeint

                static member inline (+) (ptr : blockptr, index : int) = blockptr(ptr.ptr + nativeint (sizeof<Block> * index))

                member inline x.Offset      = NativePtr.ofNativeInt<int64>  x.ptr
                member inline x.Size        = NativePtr.ofNativeInt<int64>  (8n + x.ptr)
                member inline x.IsFree      = NativePtr.ofNativeInt<int>    (16n + x.ptr)
                member inline x.Prev        = NativePtr.ofNativeInt<int>    (20n + x.ptr)
                member inline x.Next        = NativePtr.ofNativeInt<int>    (24n + x.ptr)
 
                member inline x.Value = NativePtr.read (NativePtr.ofNativeInt<Block> x.ptr)

                new(p) = { ptr = p }

            end

        type entryptr =
            struct
                val mutable public ptr : nativeint

                member inline x.Block       = blockptr x.ptr
                member inline x.Index       = NativePtr.ofNativeInt<int>  (32n + x.ptr)
                member inline x.Key         = NativePtr.ofNativeInt<Guid> (36n + x.ptr)
                member inline x.HashCode    = NativePtr.ofNativeInt<int>  (52n + x.ptr)
                member inline x.NextEntry   = NativePtr.ofNativeInt<int>  (56n + x.ptr)

                member inline x.Value = NativePtr.read (NativePtr.ofNativeInt<Entry> x.ptr)


                member x.SetBlock(b : Block) =
                    NativePtr.write (NativePtr.ofNativeInt x.ptr) b

                static member inline (+) (ptr : entryptr, index : int) = entryptr(ptr.ptr + nativeint (sizeof<Entry> * index))
                static member Null = entryptr(0n)

                new(p) = { ptr = p }
            end

        type nodeptr =
            struct
                val mutable public ptr : nativeint

                member inline x.Offset      = NativePtr.ofNativeInt<int64> (x.ptr)
                member inline x.Size        = NativePtr.ofNativeInt<int64> (8n + x.ptr)
                member inline x.Entry       = NativePtr.ofNativeInt<int> (16n + x.ptr)
                member inline x.Left        = NativePtr.ofNativeInt<int> (20n + x.ptr)
                member inline x.Right       = NativePtr.ofNativeInt<int> (24n + x.ptr)
                member inline x.Color       = NativePtr.ofNativeInt<int> (28n + x.ptr)

            
                member inline x.Value = NativePtr.read (NativePtr.ofNativeInt<Node> x.ptr)

                static member inline (+) (ptr : nodeptr, index : int) = nodeptr(ptr.ptr + nativeint (sizeof<Node> * index))
                static member Null = nodeptr(0n)

                new(p) = { ptr = p }
            end

        type headerptr =
            struct
                val mutable public ptr : nativeint
            
                member inline x.Magic           = NativePtr.ofNativeInt<Guid>   (0n + x.ptr)
                member inline x.MFirst          = NativePtr.ofNativeInt<int>    (16n + x.ptr)
                member inline x.MLast           = NativePtr.ofNativeInt<int>    (20n + x.ptr)
                member inline x.MCapacity       = NativePtr.ofNativeInt<int64>  (24n + x.ptr)
                member inline x.NRoot           = NativePtr.ofNativeInt<int>    (32n + x.ptr)
                member inline x.NCount          = NativePtr.ofNativeInt<int>    (36n + x.ptr)
                member inline x.NFreeList       = NativePtr.ofNativeInt<int>    (40n + x.ptr)
                member inline x.NFreeCount      = NativePtr.ofNativeInt<int>    (44n + x.ptr)
                member inline x.ECount          = NativePtr.ofNativeInt<int>    (48n + x.ptr)
                member inline x.EFreeList       = NativePtr.ofNativeInt<int>    (52n + x.ptr)
                member inline x.EFreeCount      = NativePtr.ofNativeInt<int>    (56n + x.ptr)
                member inline x.DCapacity       = NativePtr.ofNativeInt<int>    (60n + x.ptr)
                member inline x.DCapacityId     = NativePtr.ofNativeInt<int>    (64n + x.ptr)

                member inline x.Value = NativePtr.read (NativePtr.ofNativeInt<Header> x.ptr)

                static member inline (+) (ptr : headerptr, index : int) = nodeptr(ptr.ptr + nativeint (sizeof<Header> * index))
                static member Null = headerptr(0n)

                new(p) = { ptr = p }
            end

        [<AllowNullLiteral>]
        type VirtualNode(ptr : nodeptr, index : int) =

            member x.Index = index
            
            override x.GetHashCode() =
                index

            override x.Equals o =
                match o with
                    | :? VirtualNode as o -> index = o.Index
                    | _ -> false

            interface IComparable with
                member x.CompareTo o =
                    match o with
                        | :? VirtualNode as o ->
                            let c = compare x.Size o.Size
                            if c = 0 then compare x.Offset o.Offset
                            else c
                        | _ ->
                            failwith "uncomparable"

            member x.Equals (o : VirtualNode) =
                if isNull o then false
                else index = o.Index

            member x.Color
                with get() = !!ptr.Color
                and set v = ptr.Color <-- v

            member x.Offset
                with get() = !!ptr.Offset
                and set v = ptr.Offset <-- v

            member x.Size
                with get() = !!ptr.Size
                and set v = ptr.Size <-- v

            member x.Entry
                with get() = !!ptr.Entry
                and set v = ptr.Entry <-- v

            member x.Left
                with get() = 
                    let id = !!ptr.Left
                    if id < 0 then null
                    else VirtualNode(ptr + (id - x.Index), id)

                and set (l : VirtualNode) =
                    if isNull l then ptr.Left <-- -1
                    else ptr.Left <-- l.Index

            member x.Right
                with get() = 
                    let id = !!ptr.Right
                    if id < 0 then null
                    else VirtualNode(ptr + (id - x.Index), id)

                and set (l : VirtualNode) =
                    if isNull l then ptr.Right <-- -1
                    else ptr.Right <-- l.Index

        module Tree =

            [<AutoOpen>]
            module private Tools = 
        
                type TreeRotation =
                    | Left
                    | Right
                    | RightLeft
                    | LeftRight

                let inline isRed (n : VirtualNode) =
                    not (isNull n) && n.Color = Red

                let inline isBlack (n : VirtualNode) =
                    not (isNull n) && n.Color = Black
   
                let inline isNullOrBlack (n : VirtualNode) =
                    isNull n || n.Color = Black
            
                let inline is4Node (n : VirtualNode) =
                    isRed n.Left && isRed n.Right

                let inline is2Node (n : VirtualNode) =
                    assert (not (isNull n))
                    isBlack n && isNullOrBlack n.Left && isNullOrBlack n.Right

                let split4Node (n : VirtualNode) =
                    n.Color <- Red
                    n.Left.Color <- Black
                    n.Right.Color <- Black

                let merge2Nodes (parent : VirtualNode) (child1 : VirtualNode) (child2 : VirtualNode) =
                    assert (isRed parent)
                    parent.Color <- Black
                    child1.Color <- Red
                    child2.Color <- Red

                let rotateLeft (n : VirtualNode) =
                    let x = n.Right
                    n.Right <- x.Left
                    x.Left <- n
                    x

                let rotateRight (n : VirtualNode) : VirtualNode =
                    let x = n.Left
                    n.Left <- x.Right
                    x.Right <- n
                    x

                let rotateLeftRight (n : VirtualNode) =
                    let child = n.Left
                    let grandChild = child.Right

                    n.Left <- grandChild.Right
                    grandChild.Right <- n
                    child.Right <- grandChild.Left
                    grandChild.Left <- child
                    grandChild

                let rotateRightLeft (n : VirtualNode) =
                    let child = n.Right
                    let grandChild = child.Left

                    n.Right <- grandChild.Left
                    grandChild.Left <- n
                    child.Left <- grandChild.Right
                    grandChild.Right <- child
                    grandChild

                let replaceChildOfNodeOrRoot(root : byref<VirtualNode>, parent : VirtualNode, child : VirtualNode, newChild : VirtualNode) =
                    if isNull parent then
                        root <- newChild
            
                    else
                        if parent.Left = child then parent.Left <- newChild
                        else parent.Right <- newChild

                let replaceNode(root : byref<VirtualNode>, match_ : VirtualNode, parentOfMatch : VirtualNode, successor : VirtualNode, parentOfSuccessor : VirtualNode) =
                    let mutable successor = successor
                    if successor = match_ then
                        assert (isNull match_.Right)
                        successor <- match_.Left
                    else
                        assert ( not (isNull parentOfSuccessor) && isNull successor.Left)
                        assert ((isNull successor.Right && successor.Color = Red) || (successor.Right.Color = Red && successor.Color = Black))

                        if not (isNull successor.Right) then
                            successor.Right.Color <- Black

                        if parentOfSuccessor <> match_ then
                            parentOfSuccessor.Left <- successor.Right
                            successor.Right <- match_.Right

                        successor.Left <- match_.Left

                    if not (isNull successor) then
                        successor.Color <- match_.Color

                    replaceChildOfNodeOrRoot(&root, parentOfMatch, match_, successor)

                let sibling (n : VirtualNode) (p : VirtualNode) =
                    if n = p.Left then p.Right
                    else p.Left

                let insertionBalance(root : byref<VirtualNode>, current : VirtualNode, parent : byref<VirtualNode>, grandParent : VirtualNode, greatGrandParent : VirtualNode) =
                    assert (not (isNull grandParent))

                    let parentIsOnRight = grandParent.Right = parent
                    let currentIsOnRight = parent.Right = current

                    let mutable newChildOfGreatGrandParent = null
                    if parentIsOnRight = currentIsOnRight then
                        newChildOfGreatGrandParent <- if currentIsOnRight then rotateLeft grandParent else rotateRight grandParent
                    else
                        newChildOfGreatGrandParent <- if currentIsOnRight then rotateLeftRight grandParent else rotateRightLeft grandParent
                        parent <- greatGrandParent

                    grandParent.Color <- Red
                    newChildOfGreatGrandParent.Color <- Black

                    replaceChildOfNodeOrRoot(&root, greatGrandParent, grandParent, newChildOfGreatGrandParent)

                let rotationNeeded (parent : VirtualNode) (current : VirtualNode) (sibling : VirtualNode) =
                    assert (isRed sibling.Left || isRed sibling.Right)

                    if isRed sibling.Left then
                        if parent.Left = current then
                            TreeRotation.RightLeft
                        else
                            TreeRotation.Right
                    else
                        if parent.Left = current then
                            TreeRotation.Left
                        else
                            TreeRotation.LeftRight

            let insert (node : VirtualNode) (root : byref<VirtualNode>) =
                node.Color <- Red

                if isNull root then
                    node.Color <- Black
                    root <- node
                    true
                else
                    let mutable current : VirtualNode           = root
                    let mutable parent : VirtualNode            = null
                    let mutable grandParent : VirtualNode       = null
                    let mutable greatGrandParent : VirtualNode  = null
                    let mutable order = 0
                    let mutable found = false

                    while not (found || isNull current) do
                        order <- compare node current
                        if order = 0 then
                            root.Color <- Black
                            found <- true

                        else
                            if is4Node current then
                                split4Node current

                                if isRed parent then
                                    insertionBalance(&root, current, &parent, grandParent, greatGrandParent)

                            greatGrandParent <- grandParent
                            grandParent <- parent
                            parent <- current
                            current <- if order < 0 then current.Left else current.Right
                            ()

                    if found then 
                        false

                    else 
                        assert (not (isNull parent))

                        if order > 0 then parent.Right <- node
                        else parent.Left <- node

                        if isRed parent then
                            insertionBalance(&root, node, &parent, grandParent, greatGrandParent)

                        root.Color <- Black
                        true

            let remove (node : VirtualNode) (root : byref<VirtualNode>) =
                if isNull root then
                    false
                else
                    let mutable current : VirtualNode           = root
                    let mutable parent : VirtualNode            = null
                    let mutable grandParent : VirtualNode       = null
                    let mutable match_ : VirtualNode            = null
                    let mutable parentOfMatch : VirtualNode     = null
                    let mutable foundMatch = false

                    while not (isNull current) do
                        if is2Node current then
                            if isNull parent then
                                current.Color <- Red
                            else
                                let mutable sibling = sibling current parent
                                if isRed sibling then

                                    assert(parent.Color <> Red)

                                    if parent.Right = sibling then rotateLeft parent |> ignore
                                    else rotateRight parent |> ignore

                                    parent.Color <- Red
                                    sibling.Color <- Black

                                    replaceChildOfNodeOrRoot(&root, grandParent, parent, sibling)

                                    grandParent <- sibling
                                    if parent = match_ then
                                        parentOfMatch <- sibling

                                    sibling <- if parent.Left = current then parent.Right else parent.Left

                                assert (not (isNull sibling) || sibling.Color <> Red)

                                if is2Node sibling then
                                    merge2Nodes parent current sibling
                                else
                                    let rotation = rotationNeeded parent current sibling
                                    let mutable newGrandParent = null

                                    match rotation with
                                        | TreeRotation.Right ->
                                            assert (parent.Left = sibling && sibling.Left.Color = Red)
                                            sibling.Left.Color <- Black
                                            newGrandParent <- rotateRight parent

                                        | TreeRotation.Left ->
                                            assert (parent.Right = sibling && sibling.Right.Color = Red)
                                            sibling.Right.Color <- Black
                                            newGrandParent <- rotateLeft parent

                                        | TreeRotation.RightLeft ->
                                            assert (parent.Right = sibling && sibling.Left.Color = Red)
                                            newGrandParent <- rotateRightLeft parent

                                        | TreeRotation.LeftRight ->
                                            assert (parent.Left = sibling && sibling.Right.Color = Red)
                                            newGrandParent <- rotateLeftRight parent

                                    newGrandParent.Color <- parent.Color
                                    parent.Color <- Black
                                    current.Color <- Red
                                    replaceChildOfNodeOrRoot(&root, grandParent, parent, newGrandParent)
                                    if parent = match_ then
                                        parentOfMatch <- newGrandParent

                                    grandParent <- newGrandParent

                        let order = if foundMatch then -1 else compare node current
                        if order = 0 then
                            foundMatch <- true
                            match_ <- current
                            parentOfMatch <- parent

                        grandParent <- parent
                        parent <- current

                        current <- if order < 0 then current.Left else current.Right
                
                    if not (isNull match_) then
                        replaceNode(&root, match_, parentOfMatch, parent, grandParent)

                    if not (isNull root) then
                        root.Color <- Black

                    foundMatch

            let removeKey (offset : int64) (size : int64) (root : byref<VirtualNode>) =
                if isNull root then
                    null
                else
                    let mutable current : VirtualNode           = root
                    let mutable parent : VirtualNode            = null
                    let mutable grandParent : VirtualNode       = null
                    let mutable match_ : VirtualNode            = null
                    let mutable parentOfMatch : VirtualNode     = null
                    let mutable foundMatch = false

                    while not (isNull current) do
                        if is2Node current then
                            if isNull parent then
                                current.Color <- Red
                            else
                                let mutable sibling = sibling current parent
                                if isRed sibling then

                                    assert(parent.Color <> Red)

                                    if parent.Right = sibling then rotateLeft parent |> ignore
                                    else rotateRight parent |> ignore

                                    parent.Color <- Red
                                    sibling.Color <- Black

                                    replaceChildOfNodeOrRoot(&root, grandParent, parent, sibling)

                                    grandParent <- sibling
                                    if parent = match_ then
                                        parentOfMatch <- sibling

                                    sibling <- if parent.Left = current then parent.Right else parent.Left

                                assert (not (isNull sibling) || sibling.Color <> Red)

                                if is2Node sibling then
                                    merge2Nodes parent current sibling
                                else
                                    let rotation = rotationNeeded parent current sibling
                                    let mutable newGrandParent = null

                                    match rotation with
                                        | TreeRotation.Right ->
                                            assert (parent.Left = sibling && sibling.Left.Color = Red)
                                            sibling.Left.Color <- Black
                                            newGrandParent <- rotateRight parent

                                        | TreeRotation.Left ->
                                            assert (parent.Right = sibling && sibling.Right.Color = Red)
                                            sibling.Right.Color <- Black
                                            newGrandParent <- rotateLeft parent

                                        | TreeRotation.RightLeft ->
                                            assert (parent.Right = sibling && sibling.Left.Color = Red)
                                            newGrandParent <- rotateRightLeft parent

                                        | TreeRotation.LeftRight ->
                                            assert (parent.Left = sibling && sibling.Right.Color = Red)
                                            newGrandParent <- rotateLeftRight parent

                                    newGrandParent.Color <- parent.Color
                                    parent.Color <- Black
                                    current.Color <- Red
                                    replaceChildOfNodeOrRoot(&root, grandParent, parent, newGrandParent)
                                    if parent = match_ then
                                        parentOfMatch <- newGrandParent

                                    grandParent <- newGrandParent

                        let order = 
                            if foundMatch then 
                                -1 
                            else 
                                let c = compare size current.Size
                                if c = 0 then compare offset current.Offset
                                else c

                        if order = 0 then
                            foundMatch <- true
                            match_ <- current
                            parentOfMatch <- parent

                        grandParent <- parent
                        parent <- current

                        current <- if order < 0 then current.Left else current.Right
                
                    if not (isNull match_) then
                        replaceNode(&root, match_, parentOfMatch, parent, grandParent)

                    if not (isNull root) then
                        root.Color <- Black

                    if not foundMatch then null
                    else match_

            let findSmallestGreater (size : int64) (root : VirtualNode) =
                if isNull root then
                    null
                else
                    let mutable best = null
                    let mutable current = root
                    while not (isNull current) do
                        let cmp = compare size current.Size


                        if cmp = 0 then 
                            best <- current
                            current <- null

                        elif cmp > 0 then
                            current <- current.Right

                        else
                            best <- current
                            current <- current.Left

                    best

            let removeSmallestGreater (size : int64) (root : byref<VirtualNode>) =
                let n = findSmallestGreater size root

                if isNull n then
                    null
                else
                    let res = remove n &root
                    assert res
                    n
        
        let primeSizes =
            [|
                (*    prime no.           prime *)
                (*           2                3       +  1 = 2^2 *)
                (*           4 *) 7                // +  1 = 2^3, minimal size
                (*           6 *) 13               // +  3 = 2^4
                (*          11 *) 31               // +  1 = 2^5
                (*          18 *) 61               // +  3 = 2^6
                (*          31 *) 127              // +  1 = 2^7
                (*          54 *) 251              // +  5 = 2^8
                (*          97 *) 509              // +  3 = 2^9
                (*         172 *) 1021             // +  3 = 2^10
                (*         309 *) 2039             // +  9 = 2^11
                (*         564 *) 4093             // +  3 = 2^12
                (*        1028 *) 8191             // +  1 = 2^13
                (*        1900 *) 16381            // +  3 = 2^14
                (*        3512 *) 32749            // + 19 = 2^15
                (*        6542 *) 65521            // + 15 = 2^16
                (*       12251 *) 131071           // +  1 = 2^17
                (*       23000 *) 262139           // +  5 = 2^18
                (*       43390 *) 524287           // +  1 = 2^19
                (*       82025 *) 1048573          // +  3 = 2^20
                (*      155611 *) 2097143          // +  9 = 2^21
                (*      295947 *) 4194301          // +  3 = 2^22
                (*      564163 *) 8388593          // + 15 = 2^23
                (*     1077871 *) 16777213         // +  3 = 2^24
                (*     2063689 *) 33554393         // + 39 = 2^25
                (*     3957809 *) 67108859         // +  5 = 2^26
                (*     7603553 *) 134217689        // + 39 = 2^27
                (*    14630843 *) 268435399        // + 57 = 2^28
                (*    28192750 *) 536870909        // +  3 = 2^29
                (*    54400028 *) 1073741789       // + 35 = 2^30
                (*   105097565 *) 2147483647       // +  1 = 2^31
            |]

    type Store(pointer : Memory) as this =

        static let initialCapacityId = 7
        static let initialMemoryCapacity = 1L <<< 20

        static let isStore (m : Memory) =
            if m.Size >= int64 sizeof<Header> then
                let ptr = m.Pointer |> headerptr
                magic.Equals !!ptr.Magic
            else
                false

        let mutable pointer = pointer
        let mutable header = headerptr.Null
        let mutable nodes = nodeptr.Null
        let mutable entries = entryptr.Null
        let mutable buckets = NativePtr.zero<int>
        let mutable memory = 0n

        let nCapacity() = 
            let cap = !!header.DCapacity
            if cap > 0 then cap + 1 else 0

        let eCapacity() = 
            let cap = !!header.DCapacity
            if cap > 0 then 2 * cap + 1 else 0

        let dCapacity() = !!header.DCapacity

        let setPointers() =
            let ptr = pointer.Pointer
            header <- ptr |> headerptr

            let ptr = ptr + nativeint Store.HeaderSize
            nodes <- ptr |> nodeptr

            let ptr = ptr + nativeint (sizeof<Node> * nCapacity())
            entries <- ptr |> entryptr

            let ptr = ptr + nativeint (sizeof<Entry> * eCapacity())
            buckets <- ptr |> NativePtr.ofNativeInt

            let ptr = ptr + nativeint (sizeof<int> * dCapacity())
            memory <- ptr

        do  if not (isStore pointer) then
                
                let dCapacityId = initialCapacityId
                let dCapacity = primeSizes.[dCapacityId]
                let mCapacity = initialMemoryCapacity

                let total =
                    Store.HeaderSize +
                    Store.TableSize dCapacity +
                    mCapacity

                pointer.Clear(total)

                let header =
                    Header(
                        Magic        = magic,
                        MFirst       = -1,
                        MLast        = -1,
                        MCapacity    = mCapacity,
                        NRoot        = -1,
                        NCount       = 0,
                        NFreeList    = -1,
                        NFreeCount   = 0,
                        ECount       = 0,
                        EFreeList    = -1,
                        EFreeCount   = 0,
                        DCapacity    = dCapacity,
                        DCapacityId  = dCapacityId                        
                    )     
                    
                NativePtr.write (NativePtr.ofNativeInt pointer.Pointer) header      
                setPointers()

                // create the new free-block
                let newBlock = 
                    Block(
                        Offset = 0L, 
                        Size = mCapacity, 
                        IsFree = 0, 
                        Prev = -1, 
                        Next = -1
                    )

                // store the free-block in a new entry
                // and ensure its correct linkage
                let e,_ = this.NewEntry(newBlock)
                this.MFirst <- e
                this.MLast <- e

                // free the new block
                this.Free(e)
            else
                setPointers()


        static member private HeaderSize =
            int64 sizeof<Header>

        static member private TableSize(cap : int) =
            let nCapacity = if cap > 0 then cap + 1 else 0
            let eCapacity = if cap > 0 then 2 * cap + 1 else 0
            let dCapacity = cap
            int64 nCapacity * int64 sizeof<Node> + 
            int64 eCapacity * int64 sizeof<Entry> + 
            int64 dCapacity * int64 sizeof<int>

        member private x.Magic
            with inline get() = !!header.Magic
            and inline set v = header.Magic <-- v

        member private x.MFirst
            with get() = !!header.MFirst
            and set v = header.MFirst <-- v

        member private x.MLast
            with get() = !!header.MLast
            and set v = header.MLast <-- v

        member private x.MCapacity
            with inline get() = !!header.MCapacity
            and inline set v = header.MCapacity <-- v

        member private x.NRoot
            with inline get() = !!header.NRoot
            and inline set v = header.NRoot <-- v

        member private x.NCount
            with inline get() = !!header.NCount
            and inline set v = header.NCount <-- v

        member private x.NFreeList
            with inline get() = !!header.NFreeList
            and inline set v = header.NFreeList <-- v

        member private x.NFreeCount
            with inline get() = !!header.NFreeCount
            and inline set v = header.NFreeCount <-- v

        member private x.NCapacity = 
            let cap = x.DCapacity 
            if cap > 0 then cap + 1 else 0

        member private x.ECount
            with inline get() = !!header.ECount
            and inline set v = header.ECount <-- v

        member private x.EFreeList
            with inline get() = !!header.EFreeList
            and inline set v = header.EFreeList <-- v

        member private x.EFreeCount
            with inline get() = !!header.EFreeCount
            and inline set v = header.EFreeCount <-- v

        member private x.ECapacity = 
            let cap = x.DCapacity 
            if cap > 0 then cap * 2 + 1 else 0

        member private x.DCapacity
            with get() = !!header.DCapacity
            and inline set v = header.DCapacity <-- v

        member private x.DCapacityId
            with inline get() = !!header.DCapacityId
            and inline set v = header.DCapacityId <-- v






        // ===================================================================================================
        // Validation
        // ===================================================================================================
        member private x.CheckLinkedList() =
            let mutable success = true

            let mutable last = -1
            let mutable pLast = entries + last
            let mutable current = x.MFirst
            let mutable pCurrent = entries + current

            // first.Offset = 0L
            let off = !!pCurrent.Block.Offset
            if off <> 0L then
                Log.warn "invalid first offset %A" off
                success <- false

            while current >= 0 do
                let prev = !!pCurrent.Block.Prev
                let next = !!pCurrent.Block.Next

                // validate a.Prev.Next = a
                if prev <> last then
                    Log.warn "invalid prev: %A" prev
                    success <- false

                // validate a.Prev.Offset + a.Prev.Size = a.Offset
                if last >= 0 then
                    let lastOff = !!pLast.Block.Offset
                    let lastSize = !!pLast.Block.Size
                    let lastEnd = lastOff + lastSize
                    let off = !!pCurrent.Block.Offset
                    if off <> lastEnd then
                        Log.warn "invalid offset: %A (should be %A)" off lastEnd
                        success <- false

                last <- current
                pLast <- pCurrent
                current <- !!pCurrent.Block.Next
                pCurrent <- entries + current

            if last >= 0 then
                let total = !!pLast.Block.Offset + !!pLast.Block.Size
                if total <> x.MCapacity then
                    Log.warn "not filling memory %A" total
                    success <- false

            success


        // ===================================================================================================
        // Realloc
        // ===================================================================================================
        member private x.GrowData(needed : int64) =
            // find a minimal memory size being a power of 2
            let newCapacity = Fun.NextPowerOfTwo (max 1024L (x.MCapacity + needed))

            // calculate the new total capacity
            let total = 
                Store.HeaderSize +
                Store.TableSize(x.DCapacity) +
                newCapacity

                
            let offset, size = 
                if x.MCapacity > 0L then
                    // realloc the pointer and return the new memory's offset and size
                    pointer.Realloc(total) // <- Marshal.ReAllocHGlobal(pointer, nativeint total)
                    setPointers()
                    let offset = x.MCapacity
                    let newSize = newCapacity - x.MCapacity
                    x.MCapacity <- newCapacity
                    offset, newSize
                else
                    // allocate the pointer and return the memory's offset and size
                    pointer.Realloc total
                    setPointers()
                    x.MCapacity <- newCapacity
                    0L, newCapacity

            // recreate all pointers based on the new Pointer
            setPointers()

            // create the new free-block
            let newBlock = 
                Block(
                    Offset = offset, 
                    Size = size, 
                    IsFree = 0, 
                    Prev = x.MLast, 
                    Next = -1
                )

            // store the free-block in a new entry
            // and ensure its correct linkage
            let e,_ = x.NewEntry(newBlock)
            if x.MLast < 0 then 
                x.MFirst <- e
                x.MLast <- e
            else 
                (entries + x.MLast).Block.Next <-- e

            // free the new block
            x.Free(e)

        member private x.GrowTable() =
            let newCapacityId = if x.DCapacity = 0 then 7 else x.DCapacityId + 1
            let newCapacity = primeSizes.[newCapacityId]

            let nCapacity = if newCapacity > 0 then newCapacity + 1 else 0
            let eCapacity = if newCapacity > 0 then 2 * newCapacity + 1 else 0
            let dCapacity = newCapacity
  
            let odCapacity = x.DCapacity
            let oeCapacity = if odCapacity > 0 then 2 * odCapacity + 1 else 0
            let onCapacity = if odCapacity > 0 then odCapacity + 1 else 0
            let mCapacity = x.MCapacity

            let oldMOffset = Store.HeaderSize + Store.TableSize x.DCapacity
            let newMOffset = Store.HeaderSize + Store.TableSize newCapacity


            let total = newMOffset + x.MCapacity

            if odCapacity > 0 || mCapacity > 0L then
                pointer.Realloc total // <- Marshal.ReAllocHGlobal(pointer, nativeint total)
                    
                let ptr = pointer.Pointer

                // move the memory
                let oldPtr = ptr + nativeint oldMOffset
                let newPtr = ptr + nativeint newMOffset
                Marshal.Move(oldPtr, newPtr, mCapacity)
                memory <- newPtr

                // set the buckets to 0
                let oldSize     = nativeint (sizeof<int> * odCapacity)
                let newSize     = nativeint (sizeof<int> * dCapacity)
                let oldPtr      = oldPtr - oldSize
                let newPtr      = newPtr - newSize
                Marshal.Set(newPtr, 0, newSize)
                buckets <- NativePtr.ofNativeInt newPtr

                // move the entries and set the new memory to 0
                let oldSize     = nativeint (sizeof<Entry> * oeCapacity)
                let newSize     = nativeint (sizeof<Entry> * eCapacity)
                let oldPtr      = oldPtr - oldSize
                let newPtr      = newPtr - newSize
                Marshal.Move(oldPtr, newPtr, oldSize)
                Marshal.Set(newPtr + oldSize, 0, newSize - oldSize)
                entries <- entryptr newPtr

                    
                // move the nodes and set the new memory to 0
                let oldSize     = nativeint (sizeof<Node> * onCapacity)
                let newSize     = nativeint (sizeof<Node> * nCapacity)
                let oldPtr      = oldPtr - oldSize
                let newPtr      = newPtr - newSize
                Marshal.Move(oldPtr, newPtr, oldSize)
                Marshal.Set(newPtr + oldSize, 0, newSize - oldSize)
                nodes <- nodeptr newPtr

                // set the header pointer
                header <- headerptr ptr

                x.DCapacity <- newCapacity
                x.DCapacityId <- newCapacityId
                    
            else
                pointer.Realloc total
                setPointers()
                x.DCapacity <- newCapacity
                x.DCapacityId <- newCapacityId
              

            x.Rehash()
                    
        // ===================================================================================================
        // Entries
        // ===================================================================================================
        member private x.NewEntry(b : Block) : int * bool =
            let mutable resized = false
            let id =
                if x.EFreeCount > 0 then
                    let id = x.EFreeList
                    let pEntry = entries + id

                    x.EFreeList <- !!pEntry.NextEntry
                    x.EFreeCount <- x.EFreeCount - 1

                    id
                else
                    if x.ECount >= x.ECapacity then
                        x.GrowTable()
                        resized <- true

                    let id = x.ECount
                    x.ECount <- x.ECount + 1
                    id

            let pEntry = entries + id
            pEntry.Index <-- id
            pEntry.NextEntry <-- -1
            pEntry.HashCode <-- -1
            pEntry.SetBlock b
            id, resized

        member private x.KillEntry(b : entryptr) : unit =
            b.HashCode <-- -1
            b.Key <-- Guid.Empty

            b.NextEntry <-- x.EFreeList
            x.EFreeList <- !!b.Index
            x.EFreeCount <- x.EFreeCount + 1


        // ===================================================================================================
        // FreeList
        // ===================================================================================================
        member private x.NewNode() : int * bool =
            let mutable resized = false
            let id =
                if x.NFreeCount > 0 then
                    let id = x.NFreeList
                    let pNode = nodes + id

                    x.NFreeList <- !!pNode.Left
                    x.NFreeCount <- x.NFreeCount - 1

                    id
                else
                    if x.NCount >= x.NCapacity then
                        x.GrowTable()
                        resized <- true

                    let id = x.NCount
                    x.NCount <- x.NCount + 1
                    id

            let pNode = nodes + id
            pNode.Left <-- -1
            id, resized

        member private x.KillNode(b : int) =
            let n = nodes + b
            n.Left <-- x.NFreeList
            x.NFreeList <- b
            x.NFreeCount <- x.NFreeCount + 1

        member private x.WithTree (f : ref<VirtualNode> -> 'a) =
            let root = 
                if x.NRoot < 0 then null
                else VirtualNode(nodes + x.NRoot, x.NRoot)

            let r = ref root
            let res = f r
            let root = !r

            if isNull root then x.NRoot <- -1
            else x.NRoot <- root.Index

            res

        member private x.InsertFree(pEntry : entryptr) : unit =
            let iEntry = !!pEntry.Index
            let size = !!pEntry.Block.Size
            let offset = !!pEntry.Block.Offset
            let n,_ = x.NewNode()
            let pEntry = ()

            let pNode = nodes + n
            pNode.Offset <-- offset
            pNode.Size <-- size
            pNode.Left <-- -1
            pNode.Right <-- -1
            pNode.Entry <-- iEntry
            pNode.Color <-- Red

            x.WithTree (fun root ->
                let nn = VirtualNode(pNode, n)
                let worked = Tree.insert nn &root.contents
                if not worked then
                    Log.warn "could not add to freelist: %A" (offset, size)
                    x.KillNode n
                    
            )

        member private x.RemoveFree(pEntry : entryptr)  =
            let size = !!pEntry.Block.Size
            let offset = !!pEntry.Block.Offset

            let toKill =
                x.WithTree(fun root ->
                    Tree.removeKey offset size &root.contents
                )

            if not (isNull toKill) then
                x.KillNode toKill.Index
                true
            else
                Log.warn "could not remove from freelist: %A" (offset, size)
                false
                
        member private x.GetFree(size : int64) =
            let node =
                x.WithTree (fun tree ->
                    Tree.removeSmallestGreater size &tree.contents
                )

            if isNull node then 
                x.GrowData size
                x.GetFree size
            else
                let e = node.Entry
                let pEntry = entries + e
                x.KillNode node.Index
                pEntry


        // ===================================================================================================
        // HashTable
        // ===================================================================================================

        member private x.FindEntry(key : Guid, hashCode : int) =
            let pBucket = buckets ++ hashCode % x.DCapacity
            let ei = !!pBucket - 1

            if ei < 0 then 
                -1
            else
                let pEntry = entries + ei
                if !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                    ei
                else
                    let rec search (id : int) (pEntry : entryptr) =
                        if id < 0 then
                            -1
                        elif !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                            id
                        else
                            let n = !!pEntry.NextEntry
                            search n (entries + n)

                    let n = !!pEntry.NextEntry
                    search n (entries + n)
        
        member private x.Remove(key : Guid, hashCode : int) =
            let pBucket = buckets ++ hashCode % x.DCapacity
            let ei = !!pBucket - 1

            if ei < 0 then 
                -1
            else
                let pEntry = entries + ei
                if !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                    pBucket <-- !!pEntry.NextEntry + 1
                    ei
                else
                    let rec search (last : entryptr) (id : int) (pEntry : entryptr) =
                        if id < 0 then
                            -1
                        elif !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                            last.NextEntry <-- !!pEntry.NextEntry
                            id
                        else
                            let n = !!pEntry.NextEntry
                            search pEntry n (entries + n)

                    let n = !!pEntry.NextEntry
                    search pEntry n (entries + n)

        member private x.ReplaceEntryId(key : Guid, hashCode : int, newEntryId : int) =
            let pBucket = buckets ++ hashCode % x.DCapacity
            let ei = !!pBucket - 1
            let pNewEntry = entries + newEntryId

            if ei < 0 then 
                -1
            else
                let pEntry = entries + ei
                if !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                    pBucket <-- newEntryId + 1

                    pNewEntry.NextEntry <-- !!pEntry.NextEntry
                    pEntry.NextEntry <-- -1

                    ei
                else
                    let rec search (last : entryptr) (id : int) (pEntry : entryptr) =
                        if id < 0 then
                            -1

                        elif !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                            pNewEntry.NextEntry <-- !!pEntry.NextEntry
                            pEntry.NextEntry <-- -1
                            last.NextEntry <-- newEntryId
                            id
                        else
                            let n = !!pEntry.NextEntry
                            search pEntry n (entries + n)

                    let n = !!pEntry.NextEntry
                    search pEntry n (entries + n)


        member private x.Rehash() =
            let mutable e = entries
            let mutable i = 0

            while i < x.ECount do
                let hashCode = !!e.HashCode
                if hashCode >= 0 then
                    let bid = hashCode % x.DCapacity
                    let pBucket = NativePtr.add buckets bid

                    let old = !!pBucket - 1
                    e.NextEntry <-- old
                    pBucket <-- i + 1

                e <- e + 1
                i <- i + 1

        member x.TryGetFile(key : Guid, [<Out>] file : byref<int>) =
            let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
            let eid = x.FindEntry(key, hashCode)
            if eid >= 0 then
                file <- eid
                true
            else
                file <- -1
                false

        member x.GetOrCreateFile(key : Guid, size : int64) =
            let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
            let eid = x.FindEntry(key, hashCode)

            if eid >= 0 then
                eid
            else
                let eid = x.Alloc(size)
                let pEntry = entries + eid
                pEntry.Key <-- key
                pEntry.HashCode <-- hashCode

                let bid = hashCode % x.DCapacity
                let pBucket = NativePtr.add buckets bid

                let old = !!pBucket - 1
                pEntry.NextEntry <-- old
                pBucket <-- eid + 1


                eid

        member x.DeleteFile(key : Guid) =
            let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
            let eid = x.Remove(key, hashCode)
            if eid < 0 then
                false
            else
                x.Free eid
                true
            
        member x.Resize(key : Guid, size : int64) =
            let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
            let eid = x.FindEntry(key, hashCode)

            if eid >= 0 then
                if size >= 0L then
                    let pEntry = entries + eid
                    pEntry.HashCode <-- -1
                    pEntry.Key <-- Guid.Empty
                    let s = !!pEntry.Block.Size


                    if s <> size then
                        x.Free eid
                        let nid = x.Alloc size
                        let pNewEntry = entries + nid
                        pNewEntry.Key <-- key
                        pNewEntry.HashCode <-- hashCode

                        let oid = x.ReplaceEntryId(key, hashCode, nid)
                        assert (oid = eid)


                        nid
                    else
                        eid
                else
                    -1

            elif size >= 0L then
                let eid = x.Alloc(size)
                let pEntry = entries + eid
                pEntry.Key <-- key
                pEntry.HashCode <-- hashCode

                let bid = hashCode % x.DCapacity
                let pBucket = NativePtr.add buckets bid

                let old = !!pBucket - 1
                pEntry.NextEntry <-- old
                pBucket <-- eid + 1

                eid
            else
                -1

        // ===================================================================================================
        // Public
        // ===================================================================================================
        member x.Entries = x.ECount - x.EFreeCount
        member x.TotalMemory = Mem pointer.Size
        member x.FileTableMemory = Mem (pointer.Size - x.MCapacity)
        member x.DataMemory = Mem x.MCapacity

        member x.AllBlocks =
            [|
                let mutable current = x.MFirst
                while current >= 0 do
                    let pBlock = (entries + current).Block
                    yield current, pBlock.Value
                    current <- !!pBlock.Next
            |]

        member x.Alloc(size : int64) =
            if size <= 0L then
                -1
            else
                let mutable pEntry = x.GetFree(size)
                let mutable pBlock = pEntry.Block
                let iEntry = !!pEntry.Index
                let blockSize = !!pBlock.Size

                if blockSize > size then
                    let newBlock = 
                        Block(
                            Offset = !!pBlock.Offset + size, 
                            Size = blockSize - size, 
                            IsFree = 1, 
                            Prev = !!pEntry.Index, 
                            Next = !!pBlock.Next
                        )
                    

                    let eid, resized = x.NewEntry(newBlock)
                    if resized then
                        pEntry <- entries + iEntry
                        pBlock <- pEntry.Block

                    let pNewEntry = entries + eid
                    let pNewBlock = pNewEntry.Block

                    let nn = !!pNewBlock.Next
                    if nn < 0 then x.MLast <- eid
                    else (entries + nn).Block.Prev <-- eid

                    pBlock.Size <-- size
                    pBlock.Next <-- eid 
                    x.InsertFree pNewEntry


                pBlock.IsFree <-- 0
                !!pEntry.Index

        member x.Free(eid : int) =
            if eid >= 0 then
                let pEntry = entries + eid
                let pBlock = pEntry.Block

                if !!pBlock.IsFree = 0 then
                    let prev = !!pBlock.Prev
                    let next = !!pBlock.Next
                    let pPrev = entries + prev
                    let pNext = entries + next
                    let pPrevBlock = pPrev.Block
                    let pNextBlock = pNext.Block

                    if prev < 0 then
                        x.MFirst <- eid

                    elif !!pPrevBlock.IsFree <> 0 then
                        // merge with prev
                        x.RemoveFree pPrev |> ignore

                        pBlock.Offset <-- !!pPrevBlock.Offset
                        pBlock.Size <-- !!pPrevBlock.Size + !!pBlock.Size



                        let prev = !!pPrevBlock.Prev
                        pBlock.Prev <-- prev
                        if prev < 0 then x.MFirst <- eid
                        else (entries + prev).Block.Next <-- eid

                        x.KillEntry(pPrev)

                    if next < 0 then
                        x.MLast <- eid

                    elif !!pNextBlock.IsFree <> 0 then
                        // merge with next
                        x.RemoveFree pNext |> ignore

                        pBlock.Size <-- !!pBlock.Size + !!pNextBlock.Size

                        let next = !!pNextBlock.Next
                        pBlock.Next <-- next
                        if next < 0 then x.MLast <- eid
                        else (entries + next).Block.Prev <-- eid
                    
                        x.KillEntry(pNext)


                    pEntry.Key <-- Guid.Empty
                    pEntry.HashCode <-- -1
                    pBlock.IsFree <-- 1
                    x.InsertFree(pEntry)


        member x.Write(eid : int, src : byte[], srcOffset : int64, length : int64) =
            if eid < 0 then
                if length > 0L then failwithf "[Store] cannot write %d bytes to block with size 0" length
            else
                let pEntry = entries + eid
                let offset = nativeint !!pEntry.Block.Offset
                let size = !!pEntry.Block.Size

                if length > size then
                    failwithf "[Store] cannot write %d bytes to block with size %d" length size

                let gc = GCHandle.Alloc(src, GCHandleType.Pinned)
                try Marshal.Copy(gc.AddrOfPinnedObject() + nativeint srcOffset, memory + offset, length)
                finally gc.Free()

        member x.Write(eid : int, src : byte[], srcOffset : int64) =
            x.Write(eid, src, srcOffset, src.LongLength - srcOffset)

        member x.Write(eid : int, src : byte[]) =
            x.Write(eid, src, 0L, src.LongLength )


        member x.Read(eid : int, dst : byte[], dstOffset : int64, length : int64) =
            if eid < 0 then
                if length > 0L then failwithf "[Store] cannot read %d bytes from block with size 0" length
            else
                let pEntry = entries + eid
                let offset = nativeint !!pEntry.Block.Offset
                let size = int64 !!pEntry.Block.Size

                if length > size then
                    failwithf "[Store] cannot read %d bytes from block with size %d" length size


                let gc = GCHandle.Alloc(dst, GCHandleType.Pinned)
                try Marshal.Copy(memory + offset, gc.AddrOfPinnedObject() + nativeint dstOffset, length)
                finally gc.Free()

        member x.Read(eid : int, dst : byte[], dstOffset : int64) =
            x.Read(eid, dst, dstOffset, dst.LongLength - dstOffset)

        member x.Read(eid : int, dst : byte[]) =
            x.Read(eid, dst, 0L, dst.LongLength)

        member x.Read(eid : int) =
            if eid >= 0 then
                let pEntry = entries + eid
                let size = int64 !!pEntry.Block.Size

                let arr = Array.zeroCreate (int size)
                x.Read(eid, arr)
                arr
            else
                [||]

        member x.SizeOf(eid : int) =
            if eid >= 0 then
                let pEntry = entries + eid
                let size = !!pEntry.Block.Size
                size
            else
                0L
            

        


        member private x.Dispose(disposing : bool) =
            let o = Interlocked.Exchange(&memory, 0n)
            if o <> 0n then
                if disposing then GC.SuppressFinalize x
                pointer.Dispose()
                header <- headerptr.Null
                nodes <- nodeptr.Null
                entries <- entryptr.Null
                buckets <- NativePtr.zero<int>
                memory <- 0n

        member x.Dispose() = x.Dispose(true)
        override x.Finalize() = x.Dispose(false)

        interface IDisposable with
            member x.Dispose() = x.Dispose(true)


    type BlobStore(m : Memory) =
        let store = new Store(m)
        let cache = System.Collections.Concurrent.ConcurrentDictionary<Guid, File>()

        let rw = new ReaderWriterLockSlim()

        member x.Memory = 
            ReaderWriterLock.read rw (fun () -> store.TotalMemory)

        member x.Get (id : Guid) =
            cache.GetOrAdd(id, fun id ->
                ReaderWriterLock.read rw (fun () -> 
                    match store.TryGetFile id with
                        | (true, eid) -> File(rw, store, id, eid)
                        | _ -> File(rw, store, id, -1)
                )
            )

        member x.Create() =
            let id = Guid.NewGuid()
            let res = File(rw, store, id, -1)
            cache.[id] <- res
            res

        member x.Dispose() = 
            ReaderWriterLock.write rw (fun () -> 
                store.Dispose()
            )
        
        member x.Print() =
            ReaderWriterLock.read rw (fun () -> 
                Log.start "BlobStore"
                store.AllBlocks
                    |> Seq.map (fun (id, b) -> sprintf "%d: %A" id b)
                    |> Seq.iter (Log.line "%s")
                Log.stop()
            )

        interface IBlobStore with
            member x.Memory = x.Memory
            member x.Get id = x.Get(id) :> IBlobFile
            member x.Create() = x.Create() :> IBlobFile
            member x.Dispose() = x.Dispose()

    and File(rw : ReaderWriterLockSlim, store : Store, id : Guid, eid : int) =
        let mutable eid = eid
        let mutable size = store.SizeOf eid

        member x.Name = id

        member x.Exists =
            lock x (fun () -> eid >= 0)

        member x.Size =
            lock x (fun () -> size)

        member x.Read() =
            lock x (fun () ->
                ReaderWriterLock.read rw (fun () ->
                    store.Read eid
                )
            )

        member x.Write(data : byte[]) =
            lock x (fun () ->
                if size <> data.LongLength then
                    ReaderWriterLock.write rw (fun () ->
                        size <- data.LongLength
                        eid <- store.Resize(id, size)
                    )

                ReaderWriterLock.read rw (fun () ->
                    store.Write(eid, data)
                )
            )

        member x.Delete() =
            lock x (fun () ->
                if eid >= 0 then
                    ReaderWriterLock.write rw (fun () ->
                        let deleted = store.DeleteFile(id)
                        assert deleted
                        eid <- -1
                        size <- 0L
                    )
            )

        interface IBlobFile with
            member x.Name = x.Name
            member x.Exists = x.Exists
            member x.Size = x.Size
            member x.Read() = x.Read()
            member x.Write arr = x.Write arr
            member x.Delete() = x.Delete()
            member x.CopyTo o = o.Write (x.Read())

    let test() =
        use s = new Store(Memory.hglobal 0L)
        
        let data = Array.init 200 byte
        let e = s.Alloc(data.LongLength)

        s.Write(e, data)
        let r = s.Read(e)
        printfn "%A" r

        s.Free(e)

    let moreTest () =
        //if File.Exists @"C:\Users\Schorsch\Desktop\test.bin" then File.Delete @"C:\Users\Schorsch\Desktop\test.bin"

        use s = new Store(Memory.mapped @"C:\Users\Schorsch\Desktop\test.bin")

        printfn "(%d/%A)" s.Entries s.TotalMemory

        let r = System.Random()
        let allocations = System.Collections.Generic.List()

        let run () =
            let op = r.NextDouble() 
            if op < 0.8 || allocations.Count <= 0 then
                let ssize = 1+r.Next(255)
                let e = s.Alloc(int64 ssize)
                s.Write(e,Array.create ssize (byte e))
                allocations.Add(e)
            else 
                let index = r.Next(0,allocations.Count-1)
                let a = allocations.[index]
                let arr = s.Read a
                let r = arr |> Array.forall (fun b -> b = byte a)
                if not r then printfn "furious anger"
                s.Free(a)
                allocations.RemoveAt index

        for i in 0 .. 1000000 do
            if i%100 = 0 then printfn "it: %d (%d/%A)" i s.Entries s.TotalMemory
            run ()

    let allocPerf() =
        use s = new Store(Memory.hglobal 0L)
        let r = System.Random()

        let cnt = 1 <<< 20

        let mutable blocks = Array.zeroCreate cnt
        let sizes = Array.init cnt (fun _ -> int64 (1 + r.Next 255))
        let data = Array.zeroCreate 1024

        let sw = Stopwatch()
        sw.Restart()
        for i in 0 .. cnt-1 do
            blocks.[i] <- s.Alloc sizes.[i]
        sw.Stop()
        Log.line "alloc: %A" (sw.MicroTime / float cnt)


        sw.Restart()
        for i in 0 .. cnt-1 do
            s.Write(blocks.[i], data, 0L, sizes.[i])
        sw.Stop()
        Log.line "write: %A" (sw.MicroTime / float cnt)

        blocks <- blocks.RandomOrder() |> Seq.toArray
        sw.Restart()
        for i in 0 .. cnt-1 do
            s.Free(blocks.[i])
        sw.Stop()
        Log.line "free:  %A (random order)" (sw.MicroTime / float cnt)

        use s = new Store(Memory.hglobal 0L)
        for i in 0 .. cnt-1 do
            blocks.[i] <- s.Alloc sizes.[i]
        sw.Restart()
        for i in 0 .. cnt-1 do
            s.Free(blocks.[i])
        sw.Stop()
        Log.line "free:  %A (sequential)" (sw.MicroTime / float cnt)

    let fileTest() =
        use s = new BlobStore(Memory.mapped @"C:\Users\Schorsch\Desktop\test.bin")

        let a = s.Get (Guid "d5993e65-6e27-4596-85c1-2a59491477c0")
        let b = s.Get (Guid "d5993e65-6e27-4596-85c1-2a59491477c0")

        
        match a.Exists with
            | true -> 
                let arr = a.Read() 
                printfn "a = %A" arr
                let arr = Array.append arr [|123uy|]
                a.Write arr

                

            | _ ->
                a.Write(Array.init 10 byte)
                a.Read() |> printfn "a = %A (created)"

        s.Print()