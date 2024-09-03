module StoreTest

open System
open System.Text
open System.Diagnostics
open System.Security.Cryptography
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Aardvark.Base
open Aardvark.Base.Native
open Aardvark.Base.Native.FileManager
open System.IO
open Microsoft.FSharp.NativeInterop
open System.Threading

#nowarn "9"

module FileManagerTypes =
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
            [<FieldOffset(28)>]
            val mutable public Chunk    : int

            static member Invalid = Block(Offset = -1L, Size = -1L, IsFree = -1, Prev = -1, Next = -1, Chunk = -1)


            override x.ToString() =
                sprintf "{ Offset = %d; Size = %d; IsFree = %A; Prev = %d; Next = %d; Chunk = %d }" x.Offset x.Size (x.IsFree <> 0) x.Prev x.Next x.Chunk

        end

    [<StructLayout(LayoutKind.Explicit, Size = 32)>]
    type DictEntry =
        struct
            [<FieldOffset(0)>]
            val mutable public Key : Guid       // 16
            [<FieldOffset(16)>]
            val mutable public HashCode : int   // 20
            [<FieldOffset(20)>]
            val mutable public Next : int       // 24
            [<FieldOffset(24)>]
            val mutable public Block : int      // 28


            static member Invalid = DictEntry(Key = Guid.Empty, HashCode = -1, Next = -1, Block = -1)



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


            static member Invalid = Node(Offset = -1L, Size = -1L, Entry = -1, Left = -1, Right = -1, Color = -1)


        end


    [<StructLayout(LayoutKind.Sequential, Size = 128)>]
    type Header =
        struct
            val mutable public Magic        : Guid      // 16
            val mutable public MFirst       : int       // 20
            val mutable public MLast        : int       // 24
            val mutable public MBlubb       : int64     // 32
            val mutable public NRoot        : int       // 36
            val mutable public NCount       : int       // 40
            val mutable public NFreeList    : int       // 44
            val mutable public NFreeCount   : int       // 48
            val mutable public ECount       : int       // 52
            val mutable public EFreeList    : int       // 56
            val mutable public EFreeCount   : int       // 60
            val mutable public DCapacity    : int       // 64
            val mutable public DCapacityId  : int       // 68

            val mutable public BCount       : int       // 72
            val mutable public BFreeList    : int       // 76
            val mutable public BFreeCount   : int       // 80
            val mutable public BCapacity    : int       // 84
            val mutable public Chunks       : int       // 88

        end

    type blockptr =
        struct
            val mutable public ptr : nativeint

            member inline x.Offset      = NativePtr.ofNativeInt<int64>  x.ptr
            member inline x.Size        = NativePtr.ofNativeInt<int64>  (8n + x.ptr)
            member inline x.IsFree      = NativePtr.ofNativeInt<int>    (16n + x.ptr)
            member inline x.Prev        = NativePtr.ofNativeInt<int>    (20n + x.ptr)
            member inline x.Next        = NativePtr.ofNativeInt<int>    (24n + x.ptr)
            member inline x.Chunk       = NativePtr.ofNativeInt<int>    (28n + x.ptr)
 
 
            member x.Value  
                with inline get() = NativePtr.read (NativePtr.ofNativeInt<Block> x.ptr)
                and inline set v = NativePtr.write (NativePtr.ofNativeInt x.ptr) v

            static member inline (+) (ptr : blockptr, index : int) = blockptr(ptr.ptr + nativeint (sizeof<Block> * index))
            static member Null = blockptr(0n)

            new(p) = { ptr = p }

        end

    type dentryptr =
        struct
            val mutable public ptr : nativeint
            member inline x.Key         = (x.ptr) |> NativePtr.ofNativeInt<Guid>
            member inline x.HashCode    = (x.ptr + 16n) |> NativePtr.ofNativeInt<int>
            member inline x.Next        = (x.ptr + 20n) |> NativePtr.ofNativeInt<int>
            member inline x.Block       = (x.ptr + 24n) |> NativePtr.ofNativeInt<int>


            member x.Value  
                with inline get() = NativePtr.read (NativePtr.ofNativeInt<DictEntry> x.ptr)
                and inline set v = NativePtr.write (NativePtr.ofNativeInt x.ptr) v


            static member inline (+) (ptr : dentryptr, index : int) = dentryptr(ptr.ptr + nativeint (sizeof<DictEntry> * index))
            static member Null = dentryptr(0n)

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

            
            member x.Value
                with inline get() = NativePtr.read (NativePtr.ofNativeInt<Node> x.ptr)
                and inline set n = NativePtr.write (NativePtr.ofNativeInt x.ptr) n

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

            member inline x.NRoot           = NativePtr.ofNativeInt<int>    (32n + x.ptr)
            member inline x.NCount          = NativePtr.ofNativeInt<int>    (36n + x.ptr)
            member inline x.NFreeList       = NativePtr.ofNativeInt<int>    (40n + x.ptr)
            member inline x.NFreeCount      = NativePtr.ofNativeInt<int>    (44n + x.ptr)

            member inline x.ECount          = NativePtr.ofNativeInt<int>    (48n + x.ptr)
            member inline x.EFreeList       = NativePtr.ofNativeInt<int>    (52n + x.ptr)
            member inline x.EFreeCount      = NativePtr.ofNativeInt<int>    (56n + x.ptr)
            member inline x.DCapacity       = NativePtr.ofNativeInt<int>    (60n + x.ptr)
            member inline x.DCapacityId     = NativePtr.ofNativeInt<int>    (64n + x.ptr)

            member inline x.BCount          = NativePtr.ofNativeInt<int>    (68n + x.ptr)
            member inline x.BFreeList       = NativePtr.ofNativeInt<int>    (72n + x.ptr)
            member inline x.BFreeCount      = NativePtr.ofNativeInt<int>    (76n + x.ptr)
            member inline x.BCapacity       = NativePtr.ofNativeInt<int>    (80n + x.ptr)
            member inline x.Chunks          = NativePtr.ofNativeInt<int>    (84n + x.ptr)
 

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
                        if c = 0 then 
                            let c = compare x.Offset o.Offset
                            if c = 0 then compare x.Entry o.Entry
                            else c
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


    [<AllowNullLiteral>]
    type FileHandle =
        class
            val mutable internal EntryId : int
            val mutable internal Length : int64
            val public Id : Guid

            member x.Size = x.Length
            member x.Exists = x.EntryId >= 0

            internal new(id : Guid, eid : int, length : int64) = { Id = id; EntryId = eid; Length = length }
        end

    type BlockHandle =
        struct
            val mutable internal BlockId : int
            internal new(bid : int) = { BlockId = bid }
        end

    type FileManagerStatistics =
        {
            growManagerTime     : MicroTime
            growDictTime        : MicroTime
            growDataTime        : MicroTime
            rehashTime          : MicroTime
            reallocTime         : MicroTime
            allocCount          : int64
            freeCount           : int64
            fileCount           : int
            blockCount          : int
        }

open FileManagerTypes

type FileManager(mem : Memory, getChunk : int -> Memory) =
    static let initialCapacityId = 7
    static let initialMemoryCapacity = 1L <<< 20
    static let chunkSize = 2L <<< 30
    static let isStore (m : Memory) =
        if m.Size >= int64 sizeof<Header> then
            let ptr = m.Pointer |> headerptr
            magic.Equals !!ptr.Magic
        else
            false

    static let headerSize = int64 sizeof<Header>

    static let managerSize (bCapacity : int) =
        let nCapacity = if bCapacity > 0 then bCapacity + 1 else 0

        int64 sizeof<Node> * int64 nCapacity +
        int64 sizeof<Block> * int64 bCapacity

    static let dictSize (dCapacity : int) =
        int64 (sizeof<int> + sizeof<DictEntry>) * int64 dCapacity


    let mutable mem = mem

    let chunks = System.Collections.Generic.List<Memory>()

    let mutable header = headerptr.Null
    let mutable nodes = nodeptr.Null
    let mutable blocks = blockptr.Null
    let mutable entries = dentryptr.Null
    let mutable buckets = NativePtr.zero<int>

    let growManagerTime = Stopwatch()
    let growDictTime = Stopwatch()
    let growDataTime = Stopwatch()
    let rehashTime = Stopwatch()
    let reallocTime = Stopwatch()
    let mutable allocCount = 0L
    let mutable freeCount = 0L
        

    let handleCache = Dict<Guid, FileHandle>()

    let nCapacity() =
        let cap = !!header.BCapacity
        if cap > 0 then cap + 1 else 0

    let bCapacity() = !!header.BCapacity
    let dCapacity() = !!header.DCapacity

    let setPointers() =
        let ptr = mem.Pointer

        header <- ptr |> headerptr
        let ptr = ptr + nativeint headerSize

        let nCapacity = nCapacity()
        let bCapacity = bCapacity()
        let dCapacity = dCapacity()

        nodes <- ptr |> nodeptr
        let ptr = ptr + (nativeint sizeof<Node> * nativeint nCapacity)

        blocks <- ptr |> blockptr
        let ptr = ptr + (nativeint sizeof<Block> * nativeint bCapacity)

        entries <- ptr |> dentryptr
        let ptr = ptr + (nativeint sizeof<DictEntry> * nativeint dCapacity)

        buckets <- NativePtr.ofNativeInt ptr
        let ptr = ptr + (nativeint sizeof<int> * nativeint dCapacity)

        ()


    let rec growManager() =
        growManagerTime.Start()

        let oldBCapacity = !!header.BCapacity
        let newBCapacity = 2 * oldBCapacity
        let oldNCapacity = if oldBCapacity = 0 then 0 else oldBCapacity + 1
        let newNCapacity = if newBCapacity = 0 then 0 else newBCapacity + 1

        let dictSize = dictSize !!header.DCapacity

        let newTotalSize =
            headerSize +
            managerSize newBCapacity +
            dictSize

        let oldTotalSize =
            headerSize +
            managerSize oldBCapacity +
            dictSize

        header.BCapacity <-- newBCapacity
        mem.Realloc(newTotalSize)
            
        let newPtr = mem.Pointer + nativeint newTotalSize
        let oldPtr = mem.Pointer + nativeint oldTotalSize

        // move the dict
        let newPtr = newPtr - nativeint dictSize
        let oldPtr = oldPtr - nativeint dictSize
        Marshal.Move(oldPtr, newPtr, dictSize)

        // move the Blocks
        let oldSize = int64 sizeof<Block> * int64 oldBCapacity
        let newSize = int64 sizeof<Block> * int64 newBCapacity
        let newPtr = newPtr - nativeint newSize
        let oldPtr = oldPtr - nativeint oldSize
        Marshal.Move(oldPtr, newPtr, oldSize)
        Marshal.Set(newPtr + nativeint oldSize, -1, newSize - oldSize)

        // move the Nodes
        let oldSize = int64 sizeof<Node> * int64 oldNCapacity
        let newSize = int64 sizeof<Node> * int64 newNCapacity
        let newPtr = newPtr - nativeint newSize
        let oldPtr = oldPtr - nativeint oldSize
        Marshal.Move(oldPtr, newPtr, oldSize)
        Marshal.Set(newPtr + nativeint oldSize, -1, newSize - oldSize)

            
        setPointers()
        growManagerTime.Stop()

    and growDict() =
        growDictTime.Start()
        let oldDCapacityId = !!header.DCapacityId
        let oldDCapacity = !!header.DCapacity
        let newDCapacityId = 1 + oldDCapacityId
        let newDCapacity = primeSizes.[newDCapacityId]

        let oldDictSize = dictSize oldDCapacity
        let newDictSize = dictSize newDCapacity

        let managerSize = managerSize !!header.BCapacity

        let newTotalSize =
            headerSize +
            managerSize +
            newDictSize

        let oldTotalSize =
            headerSize +
            managerSize +
            oldDictSize
                
        header.DCapacityId <-- newDCapacityId
        header.DCapacity <-- newDCapacity
        reallocTime.Start()
        mem.Realloc(newTotalSize)
        reallocTime.Stop()

        // move the dict
        let newPtr = mem.Pointer + nativeint newTotalSize
        let oldPtr = mem.Pointer + nativeint oldTotalSize


        // set the buckets to 0
        let newSize = int64 sizeof<int> * int64 newDCapacity
        let oldSize = int64 sizeof<int> * int64 oldDCapacity
        let newPtr = newPtr - nativeint newSize
        let oldPtr = oldPtr - nativeint oldSize
        Marshal.Set(newPtr, 0, newSize)

        // move the DictEntries
        let newSize = int64 sizeof<DictEntry> * int64 newDCapacity
        let oldSize = int64 sizeof<DictEntry> * int64 oldDCapacity
        let newPtr = newPtr - nativeint newSize
        let oldPtr = oldPtr - nativeint oldSize
        Marshal.Move(oldPtr, newPtr, oldSize)

        setPointers()

        rehashTime.Start()
        rehash()
        rehashTime.Stop()

        growDictTime.Stop()

    and growData(needed : int64) =
        growDataTime.Start()

        let chunkIndex = chunks.Count - 1

        let oldLastId = !!header.MLast
        let _, freeBlockId = newBlock()
        let pFreeBlock = blocks + freeBlockId
        pFreeBlock.Prev <-- oldLastId
        pFreeBlock.Next <-- -1
        pFreeBlock.IsFree <-- 0

        if chunkIndex >= 0 && chunks.[chunkIndex].Size < chunkSize then
            let lastChunk = chunks.[chunkIndex]
            let oldChunkCapacity = lastChunk.Size
            let newChunkCapacity = 2L * oldChunkCapacity
            lastChunk.Realloc newChunkCapacity

            let offset = chunkSize * int64 chunkIndex + oldChunkCapacity

            // free the new block
            pFreeBlock.Offset <-- offset
            pFreeBlock.Size <-- newChunkCapacity - oldChunkCapacity
            pFreeBlock.Chunk <-- chunkIndex

        else
            let chunkIndex = 1 + chunkIndex
            let newChunk = getChunk chunkIndex
            let newChunkCapacity = max (1L <<< 20) (Fun.NextPowerOfTwo needed)
            newChunk.Clear newChunkCapacity
            chunks.Add newChunk

            let offset = chunkSize * int64 chunkIndex
            pFreeBlock.Offset <-- offset
            pFreeBlock.Size <-- newChunkCapacity
            pFreeBlock.Chunk <-- chunkIndex

            header.Chunks <-- !!header.Chunks + 1

        if oldLastId < 0 then header.MFirst <-- freeBlockId
        else (blocks + oldLastId).Next <-- freeBlockId

        free freeBlockId
        growDataTime.Stop()


    and newBlock() : bool * int =
        if !!header.BFreeCount > 0 then
            let id = !!header.BFreeList
            assert (id >= 0)

            let b = blocks + id
            header.BFreeList <-- !!b.Next
            header.BFreeCount <-- !!header.BFreeCount - 1
            b.Next <-- -1


            false, id
        else
            let mutable resized = false
            let cnt = !!header.BCount
            if cnt >= bCapacity() then
                resized <- true
                growManager()

            let id = cnt
            header.BCount <-- cnt + 1
            resized, id

    and deleteBlock(bid : int) =
        assert (bid >= 0)
        let n = !!header.BFreeList
        let mutable b = blocks + bid
        b.Value <- Block.Invalid
        b.Next <-- n
        header.BFreeList <-- bid
        header.BFreeCount <-- !!header.BFreeCount + 1

    and newNode() : bool * int =
        if !!header.NFreeCount > 0 then
            let id = !!header.NFreeList
            assert (id >= 0)

            let n = nodes + id
            header.NFreeList <-- !!n.Left
            header.NFreeCount <-- !!header.NFreeCount - 1
            n.Left <-- -1


            false, id
        else
            let mutable resized = false
            let cnt = !!header.NCount
            if cnt >= nCapacity() then
                resized <- true
                growManager()

            let id = cnt
            header.NCount <-- cnt + 1
            resized, id
            
    and deleteNode(nid : int) =
        assert (nid >= 0)
        let next = !!header.NFreeList
        let mutable n = nodes + nid
        n.Value <- Node.Invalid
        n.Left <-- next
        header.NFreeList <-- nid
        header.NFreeCount <-- !!header.NFreeCount + 1   

    and newEntry() : bool * int =
        if !!header.EFreeCount > 0 then
            let id = !!header.EFreeList
            assert (id >= 0)

            let e = entries + id
            header.EFreeList <-- !!e.Next
            header.EFreeCount <-- !!header.EFreeCount - 1
            e.Next <-- -1

            false, id
        else
            let mutable resized = false
            let cnt = !!header.ECount
            if cnt >= dCapacity() then
                resized <- true
                growDict()

            let id = cnt
            header.ECount <-- cnt + 1
            resized, id

    and deleteEntry(eid : int) =
        assert (eid >= 0)

        let next = !!header.EFreeList
        let mutable e = entries + eid
        e.Value <- DictEntry.Invalid
        e.Next <-- next
        header.EFreeList <-- eid
        header.EFreeCount <-- !!header.EFreeCount + 1   



    and withTree (f : ref<VirtualNode> -> 'a) =
        let root = 
            let rid = !!header.NRoot
            if rid < 0 then null
            else VirtualNode(nodes + rid, rid)

        let r = ref root
        let res = f r
        let root = !r

        if isNull root then header.NRoot <-- -1
        else header.NRoot <-- root.Index

        res

    and insertFreeBlock (bid : int) =
        assert (bid >= 0)

        let pBlock = blocks + bid
        let size = !!pBlock.Size
        let offset = !!pBlock.Offset
        let _, nid = newNode()
        let pEntry = ()

        let pNode = nodes + nid
        pNode.Offset <-- offset
        pNode.Size <-- size
        pNode.Left <-- -1
        pNode.Right <-- -1
        pNode.Entry <-- bid
        pNode.Color <-- Red

        withTree (fun root ->
            let nn = VirtualNode(pNode, nid)
            let worked = Tree.insert nn &root.contents
            if not worked then
                Log.warn "could not add to freelist: %A" (offset, size)
                deleteNode nid
                    
        )

    and removeFreeBlock (bid : int) =
        assert (bid >= 0)

        let pBlock = blocks + bid
        let offset = !!pBlock.Offset
        let size = !!pBlock.Size

        let toKill =
            withTree(fun root ->
                Tree.removeKey offset size &root.contents
            )

        if not (isNull toKill) then
            deleteNode toKill.Index
            true
        else
            Log.warn "could not remove from freelist: %A" (offset, size)
            false

    and getFreeBlock (size : int64) : bool * int =
        let node =
            withTree (fun tree ->
                Tree.removeSmallestGreater size &tree.contents
            )

        if isNull node then 
            growData size
            let _, bid = getFreeBlock size
            true, bid
        else
            let bid = node.Entry
            assert (bid >= 0)
            assert (!!(blocks + bid).Size >= size)
            deleteNode node.Index
            false, bid

    and alloc (size : int64) =
        if size <= 0L then
            false, -1
        else
            allocCount <- allocCount + 1L
            let mutable resized, bid = getFreeBlock size
            let mutable pBlock = blocks + bid
            let blockSize = !!pBlock.Size

            if blockSize > size then
                let r, rid = newBlock()
                if r then
                    pBlock <- blocks + bid
                    resized <- true


                let restNext = !!pBlock.Next

                let mutable pRestBlock = blocks + rid
                pRestBlock.Offset <-- !!pBlock.Offset + size
                pRestBlock.Size <-- blockSize - size
                pRestBlock.IsFree <-- 1
                pRestBlock.Prev <-- bid
                pRestBlock.Next <-- restNext
                pRestBlock.Chunk <-- !!pBlock.Chunk

                if restNext < 0 then header.MLast <-- rid
                else (blocks + restNext).Prev <-- rid

                pBlock.Size <-- size
                pBlock.Next <-- rid 
                insertFreeBlock rid


            pBlock.IsFree <-- 0
            assert (!!pBlock.Size = size)
            resized, bid

    and free (bid : int) =
        if bid >= 0 then
            freeCount <- freeCount + 1L
            let pBlock = blocks + bid
            if !!pBlock.IsFree = 0 then
                let chunk = !!pBlock.Chunk
                let prev = !!pBlock.Prev
                let next = !!pBlock.Next
                let pPrevBlock = blocks + prev
                let pNextBlock = blocks + next

                if prev < 0 then
                    header.MFirst <-- bid

                elif !!pPrevBlock.IsFree <> 0 && !!pPrevBlock.Chunk = chunk then
                    // merge with prev
                    removeFreeBlock prev |> ignore

                    pBlock.Offset <-- !!pPrevBlock.Offset
                    pBlock.Size <-- !!pPrevBlock.Size + !!pBlock.Size


                    let pp = !!pPrevBlock.Prev
                    pBlock.Prev <-- pp
                    if pp < 0 then header.MFirst <-- bid
                    else (blocks + pp).Next <-- bid

                    deleteBlock prev


                if next < 0 then
                    header.MLast <-- bid

                elif !!pNextBlock.IsFree <> 0 && !!pNextBlock.Chunk = chunk then
                    // merge with next
                    removeFreeBlock next |> ignore

                    pBlock.Size <-- !!pBlock.Size + !!pNextBlock.Size

                    let nn = !!pNextBlock.Next
                    pBlock.Next <-- nn
                    if nn < 0 then header.MLast <-- bid
                    else (blocks + nn).Prev <-- bid
                    
                    deleteBlock next

                pBlock.IsFree <-- 1
                insertFreeBlock bid

        

    and findEntry (key : Guid) (hashCode : int) =
        let bid = hashCode % !!header.DCapacity
        let pBucket = NativePtr.add buckets bid

        let b = !!pBucket - 1
        if b < 0 then
            -1
        else
            let pEntry = entries + b
            if !!pEntry.HashCode = hashCode && key.Equals !!pEntry.Key then
                b
            else
                let rec search (id : int) (pEntry : dentryptr) = 
                    if id < 0 then
                        -1
                    elif !!pEntry.HashCode = hashCode && key.Equals !!pEntry.Key then
                        id
                    else
                        let next = !!pEntry.Next
                        search next (entries + next)

                let next = !!pEntry.Next
                search next (entries + next)

//        and removeEntry (key : Guid) (hashCode : int) =
//            let bid = hashCode % !!header.DCapacity
//            let pBucket = NativePtr.add buckets bid
//
//            let b = !!pBucket - 1
//            if b < 0 then
//                -1
//            else
//                let pEntry = entries + b
//                if !!pEntry.HashCode = hashCode && key.Equals !!pEntry.Key then
//                    let block = !!pEntry.Block
//                    deleteEntry b
//                    pBucket <-- 0
//                    block
//                else
//                    let rec search (last : dentryptr) (id : int) (pEntry : dentryptr) = 
//                        if id < 0 then
//                            -1
//                        elif !!pEntry.HashCode = hashCode && key.Equals !!pEntry.Key then
//                            let next = !!pEntry.Next
//                            last.Next <-- next
//                            
//                            let block = !!pEntry.Block
//                            deleteEntry id
//                            block
//                        else
//                            let next = !!pEntry.Next
//                            search pEntry next (entries + next)
//
//                    let next = !!pEntry.Next
//                    search pEntry next (entries + next) 

    and removeEntryPtr (eid : int) =
        if eid >= 0 then
            let pRemEntry = entries + eid
            let hashCode = !!pRemEntry.HashCode
            let bid = hashCode % !!header.DCapacity
            let pBucket = NativePtr.add buckets bid

            let b = !!pBucket - 1
            if b < 0 then
                false
            else
                let pEntry = entries + b
                if b = eid then
                    let next = !!pEntry.Next
                    deleteEntry b
                    pBucket <-- 1 + next
                    true
                else
                    let rec search (last : dentryptr) (id : int) (pEntry : dentryptr) = 
                        if id < 0 then
                            false
                        elif id = eid then
                            let next = !!pEntry.Next
                            last.Next <-- next
                            
                            let block = !!pEntry.Block
                            deleteEntry id
                            true
                        else
                            let next = !!pEntry.Next
                            search pEntry next (entries + next)

                    let next = !!pEntry.Next
                    search pEntry next (entries + next) 
        else
            true

    and setEntry (key : Guid) (hashCode : int) (value : int) =
        let eid = findEntry key hashCode
        if eid >= 0 then
            let pEntry = entries + eid
            pEntry.Block <-- value
            eid
        else
            let _, eid = newEntry()
               
            let pEntry = entries + eid
            pEntry.Key <-- key
            pEntry.HashCode <-- hashCode
            pEntry.Block <-- value
            pEntry.Next <-- -1

            let bid = hashCode % !!header.DCapacity
            let pBucket = NativePtr.add buckets bid

            let b = !!pBucket - 1
            pEntry.Next <-- b
            pBucket <-- eid + 1
            eid

    and rehash() =
        let mutable pEntry = entries
        let eCount = !!header.ECount
        let dCapacity = !!header.DCapacity
        for i in 0..eCount-1 do
            let hash = !!pEntry.HashCode
            if hash >= 0 then
                let bid = hash % dCapacity
                let pBucket = NativePtr.add buckets bid
                let b = !!pBucket - 1
                pEntry.Next <-- b
                pBucket <-- i + 1

            pEntry <- pEntry + 1



    do  if isStore mem then
            setPointers()

            let numChunks = !!header.Chunks
            for i in 0..numChunks-1 do
                chunks.Add(getChunk i)

        else
            let dCapacityId = initialCapacityId
            let dCapacity = primeSizes.[dCapacityId]
            let bCapacity = Fun.NextPowerOfTwo dCapacity

            let total =
                headerSize +
                managerSize bCapacity +
                dictSize dCapacity

            mem.Clear(total)

            let c0 = getChunk 0
            chunks.Add c0
            c0.Clear(initialMemoryCapacity)

            let initialHeader =
                Header(
                    Magic        = magic,
                    MFirst       = -1,
                    MLast        = -1,
                    NRoot        = -1,
                    NCount       = 0,
                    NFreeList    = -1,
                    NFreeCount   = 0,
                    ECount       = 0,
                    EFreeList    = -1,
                    EFreeCount   = 0,
                    DCapacity    = dCapacity,
                    DCapacityId  = dCapacityId,
                        
                    BCount       = 0,
                    BFreeList    = -1,
                    BFreeCount   = 0,
                    BCapacity    = bCapacity,
                    Chunks       = 1  
                )     
                    
            NativePtr.write (NativePtr.ofNativeInt mem.Pointer) initialHeader      
            setPointers()

            // create the new free-block
            let _,bid = newBlock()
            let pBlock = blocks + bid
            pBlock.Offset <-- 0L
            pBlock.Size <-- initialMemoryCapacity
            pBlock.IsFree <-- 0
            pBlock.Prev <-- -1
            pBlock.Next <-- -1
            pBlock.Chunk <-- 0

            // store the free-block in a new entry
            // and ensure its correct linkage
            header.MFirst <-- bid
            header.MLast <-- bid

            // free the new block
            free bid
            
    // ===================================================================================================
    // Block API
    // ===================================================================================================
    member x.Alloc (size : int64) =
        let _,bid = alloc size
        BlockHandle bid

    member x.Free (block : BlockHandle) =
        free block.BlockId

    member x.SizeOf(block : BlockHandle) =
        let bid = block.BlockId
        if bid < 0 then 0L
        else
            let pBlock = blocks + bid
            !!pBlock.Size

    member x.Copy(src : byte[], srcOffset : int64, dst : BlockHandle, dstOffset : int64, length : int64) =
        if length <= 0L then
            ()
        else
            let bid = dst.BlockId
            if bid < 0 then failwithf "[FileManager] cannot write %d bytes to block of size 0" length
            if isNull src then failwith "[FileManager] cannot read from null array"

            let pBlock = blocks + bid
            assert (!!pBlock.IsFree <> 1)

            let blockSize = !!pBlock.Size
            if length > blockSize then failwithf "[FileManager] cannot write %d bytes to block of size %d" length blockSize

            let chunk = !!pBlock.Chunk
            let memory = chunks.[chunk].Pointer - nativeint chunkSize * nativeint chunk

            let gc = GCHandle.Alloc(src, GCHandleType.Pinned)
            try
                let dst = memory + nativeint !!pBlock.Offset + nativeint dstOffset
                let src = gc.AddrOfPinnedObject() + nativeint srcOffset
                Marshal.Copy(src, dst, length)
            finally
                gc.Free()

    member x.Copy(src : BlockHandle, srcOffset : int64, dst : byte[], dstOffset : int64, length : int64) =
        if length <= 0L then
            ()
        else
            let bid = src.BlockId
            if bid < 0 then failwithf "[FileManager] cannot read %d bytes from block of size 0" length
            if isNull dst then failwith "[FileManager] cannot write to null array"

            let pBlock = blocks + bid
            assert (!!pBlock.IsFree <> 1)
            let blockSize = !!pBlock.Size
            if length > blockSize then failwithf "[FileManager] cannot read %d bytes from block of size %d" length blockSize
                
            let chunk = !!pBlock.Chunk
            let memory = chunks.[chunk].Pointer - nativeint chunkSize * nativeint chunk


            let gc = GCHandle.Alloc(dst, GCHandleType.Pinned)
            try
                let dst = gc.AddrOfPinnedObject() + nativeint dstOffset
                let src = memory + nativeint !!pBlock.Offset + nativeint srcOffset
                Marshal.Copy(src, dst, length)
            finally
                gc.Free()

    member x.Copy(src : BlockHandle, srcOffset : int64, dst : BlockHandle, dstOffset : int64, length : int64) =
        if length <= 0L then
            ()
        else
            let sid = src.BlockId
            let did = src.BlockId

            if sid < 0 then failwithf "[FileManager] cannot copy %d bytes from block of size 0" length
            if did < 0 then failwithf "[FileManager] cannot copy %d bytes to block of size 0" length

            let pSrc = blocks + sid
            let pDst = blocks + did

            assert (!!pSrc.IsFree <> 1)
            assert (!!pDst.IsFree <> 1)

            let srcSize = !!pSrc.Size
            let dstSize = !!pDst.Size



            if length > srcSize then failwithf "[FileManager] cannot copy %d bytes from block of size %d" length srcSize
            if length > dstSize then failwithf "[FileManager] cannot copy %d bytes to block of size %d" length dstSize

            let dstChunk = !!pDst.Chunk
            let dstMemory = chunks.[dstChunk].Pointer - nativeint chunkSize * nativeint dstChunk

            let srcChunk = !!pSrc.Chunk
            let srcMemory = chunks.[srcChunk].Pointer - nativeint chunkSize * nativeint srcChunk


            Marshal.Copy(srcMemory + nativeint !!pSrc.Offset + nativeint srcOffset, dstMemory + nativeint !!pDst.Offset + nativeint dstOffset, length)

    member x.Copy(src : BlockHandle, dst : byte[], dstOffset : int64, length : int64) = x.Copy(src, 0L, dst, dstOffset, length)
    member x.Copy(src : BlockHandle, srcOffset : int64, dst : byte[], length : int64) = x.Copy(src, srcOffset, dst, 0L, length)
    member x.Copy(src : BlockHandle, dst : byte[], length : int64) = x.Copy(src, 0L, dst, 0L, length)
    member x.Copy(src : BlockHandle, dst : byte[]) = x.Copy(src, 0L, dst, 0L, dst.LongLength)
    member x.Copy(src : byte[], dst : BlockHandle, dstOffset : int64, length : int64) = x.Copy(src, 0L, dst, dstOffset, length)
    member x.Copy(src : byte[], srcOffset : int64, dst : BlockHandle, length : int64) = x.Copy(src, srcOffset, dst, 0L, length)
    member x.Copy(src : byte[], dst : BlockHandle, length : int64) = x.Copy(src, 0L, dst, 0L, length)
    member x.Copy(src : byte[], dst : BlockHandle) = x.Copy(src, 0L, dst, 0L, src.LongLength)
    member x.Copy(src : BlockHandle, dst : BlockHandle, dstOffset : int64, length : int64) = x.Copy(src, 0L, dst, dstOffset, length)
    member x.Copy(src : BlockHandle, srcOffset : int64, dst : BlockHandle, length : int64) = x.Copy(src, srcOffset, dst, 0L, length)
    member x.Copy(src : BlockHandle, dst : BlockHandle, length : int64) = x.Copy(src, 0L, dst, 0L, length)
    member x.Copy(src : BlockHandle, dst : BlockHandle) = x.Copy(src, 0L, dst, 0L, x.SizeOf src)

    member x.Read(src : BlockHandle) =
        if src.BlockId >= 0 then
            let size = x.SizeOf src
            let arr = Array.zeroCreate (int size)
            x.Copy(src, 0L, arr, 0L, arr.LongLength)
            arr
        else
            [||]


    // ===================================================================================================
    // File API
    // ===================================================================================================

    member x.Exists(file : FileHandle) =
        file.EntryId >= 0

    member x.GetCurrentBlock(file : FileHandle) =
        if file.EntryId < 0 then
            BlockHandle(-1)
        else
            let pEntry = entries + file.EntryId
            BlockHandle(!!pEntry.Block)


    member x.GetFile (key : Guid) =
        handleCache.GetOrCreate(key, fun key ->
            let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
            let eid = findEntry key hashCode
            if eid < 0 then
                FileHandle(key, -1, 0L)
            else
                let bid = !!(entries + eid).Block |> BlockHandle
                let size = x.SizeOf bid
                FileHandle(key, eid, size)
        )

    member x.TryGetFile (key : Guid, [<Out>] file : byref<FileHandle>) =
        if handleCache.TryGetValue(key, &file) then
            true
        else
            let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
            let eid = findEntry key hashCode
            if eid < 0 then
                false
            else
                let bid = !!(entries + eid).Block |> BlockHandle
                let size = x.SizeOf bid
                let res = FileHandle(key, eid, size)
                handleCache.[key] <- res
                file <- res
                true

    member x.Resize (file : FileHandle, newSize : int64) =
        let eid = file.EntryId
        if eid < 0 then
            if newSize > 0L then
                let _, bid = alloc newSize
                let hashCode = file.Id.GetHashCode() &&& 0x7FFFFFFF
                let eid = setEntry file.Id hashCode bid
                file.EntryId <- eid
                file.Length <- newSize

            else
                file.Length <- 0L

        else

            if newSize > 0L then
                let mutable pEntry = entries + eid
                let oldBlockId = !!pEntry.Block

                if oldBlockId >= 0 then
                    let pBlock = blocks + oldBlockId
                    let oldSize = !!pBlock.Size

                    if oldSize <> newSize then
                        free oldBlockId
                        let resized, newBlockId = alloc newSize
                        if resized then 
                            pEntry <- entries + eid

                        pEntry.Block <-- newBlockId

                else
                    let resized, bid = alloc newSize
                    if resized then
                        pEntry <- entries + eid

                    pEntry.Block <-- bid

                file.Length <- newSize

            else
                let pEntry = entries + eid
                let bid = !!pEntry.Block
                if bid >= 0 then
                    pEntry.Block <-- -1
                    free bid

                file.Length <- 0L

    member x.Delete(file : FileHandle) =
        let eid = file.EntryId
        if eid >= 0 then
            file.EntryId <- -1

            handleCache.Remove file.Id |> ignore
                
            let bid = !!(entries + eid).Block
            let removed = removeEntryPtr eid
            assert removed


            if bid >= 0 then free bid
         

    member x.Copy(src : byte[], srcOffset : int64, dst : FileHandle, dstOffset : int64, length : int64) = x.Copy(src, srcOffset, x.GetCurrentBlock dst, dstOffset, length)
    member x.Copy(src : byte[], dst : FileHandle, dstOffset : int64, length : int64) = x.Copy(src, 0L, x.GetCurrentBlock dst, dstOffset, length)
    member x.Copy(src : byte[], srcOffset : int64, dst : FileHandle, length : int64) = x.Copy(src, srcOffset, x.GetCurrentBlock dst, 0L, length)
    member x.Copy(src : byte[], dst : FileHandle, length : int64) = x.Copy(src, 0L, x.GetCurrentBlock dst, 0L, length)
    member x.Copy(src : byte[], dst : FileHandle) = x.Copy(src, 0L, x.GetCurrentBlock dst, 0L, src.LongLength)

    member x.Copy(src : FileHandle, srcOffset : int64, dst : byte[], dstOffset : int64, length : int64) = x.Copy(x.GetCurrentBlock src, srcOffset, dst, dstOffset, length) 
    member x.Copy(src : FileHandle, dst : byte[], dstOffset : int64, length : int64) = x.Copy(x.GetCurrentBlock src, 0L, dst, dstOffset, length)
    member x.Copy(src : FileHandle, srcOffset : int64, dst : byte[], length : int64) = x.Copy(x.GetCurrentBlock src, srcOffset, dst, 0L, length)
    member x.Copy(src : FileHandle, dst : byte[], length : int64) = x.Copy(x.GetCurrentBlock src, 0L, dst, 0L, length)
    member x.Copy(src : FileHandle, dst : byte[]) = x.Copy(x.GetCurrentBlock src, 0L, dst, 0L, dst.LongLength)

    member x.Read(src : FileHandle) =
        if src.EntryId >= 0 then
            let arr = Array.zeroCreate (int src.Length)
            x.Copy(x.GetCurrentBlock src, 0L, arr, 0L, arr.LongLength)
            arr
        else
            [||]



    member x.TotalMemory = Mem mem.Size + Mem (chunks |> Seq.sumBy (fun m -> m.Size))
    member x.Entries = !!header.BCount - !!header.BFreeCount
    member x.Files = !!header.ECount - !!header.EFreeCount

    member x.Statistics =
        {
            growManagerTime     = growManagerTime.MicroTime
            growDictTime        = growDictTime.MicroTime
            growDataTime        = growDataTime.MicroTime
            rehashTime          = rehashTime.MicroTime
            reallocTime         = reallocTime.MicroTime
            allocCount          = allocCount
            freeCount           = freeCount
            fileCount           = !!header.ECount - !!header.EFreeCount
            blockCount          = !!header.BCount - !!header.BFreeCount
        }


    member private x.Dispose(disposing : bool) =
        if header.ptr <> 0n then
            if disposing then GC.SuppressFinalize x
            mem.Dispose()
            chunks |> Seq.iter (fun c -> c.Dispose())
            chunks.Clear()
            header <- headerptr.Null
            nodes <- nodeptr.Null
            blocks <- blockptr.Null
            entries <- dentryptr.Null
            buckets <- NativePtr.zero<int>

    member x.Dispose() = x.Dispose(true)
    override x.Finalize() = x.Dispose(false)

    interface IDisposable with
        member x.Dispose() = x.Dispose(true)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FileManager =
    
    let directory (dir : string) =
        let exists = Directory.Exists dir
        if not exists then Directory.CreateDirectory dir |> ignore

        let main = Path.Combine(dir, "index.bin") |> Memory.mapped
        let get (i : int) =
            Path.Combine(dir, sprintf "store%d.bin" i) |> Memory.mapped

        new FileManager(main, get)
    
    let hglobal () =
        let main = Memory.hglobal 0L
        let get (i : int) =
            Memory.hglobal 0L

        new FileManager(main, get)
            

module FS = 
    module MemoryHelpers =
        let mapped (f : Memory -> 'a) =
            let path = Path.GetTempFileName()
            if System.IO.File.Exists path then System.IO.File.Delete path

            let mem = Memory.mapped path
            Log.start "File"
            try f mem
            finally 
                Log.stop()
                mem.Dispose()
                System.IO.File.Delete path

        let hglobal (f : Memory -> 'a) =
            let mem = Memory.hglobal 0L
            Log.start "HGlobal"
            try f mem
            finally 
                Log.stop()
                mem.Dispose()

    

    let test() =
        use s = FileManager.hglobal()
        
        let data = Array.init 200 byte
        let e = s.Alloc(data.LongLength)

        s.Copy(data, e)
        let r = s.Read(e)
        printfn "%A" r

        s.Free(e)

    let moreTest () =
        //if File.Exists @"C:\Users\Schorsch\Desktop\test.bin" then File.Delete @"C:\Users\Schorsch\Desktop\test.bin"

        use s = FileManager.directory  @"C:\Users\Schorsch\Desktop\moreTest"//(Memory.mapped @"C:\Users\Schorsch\Desktop\test.bin")

        printfn "(%d/%A)" s.Entries s.TotalMemory

        let r = System.Random()
        let allocations = System.Collections.Generic.List()

        let run () =
            let op = r.NextDouble() 
            if op < 0.8 || allocations.Count <= 0 then
                let ssize = 1+r.Next(1 <<< 20)
                let e = s.Alloc(int64 ssize)
                s.Copy(Array.create ssize (byte e.BlockId), e)
                allocations.Add(e)
            else 
                let index = r.Next(0,allocations.Count-1)
                let a = allocations.[index]
                let arr = s.Read a
                let r = arr |> Array.forall (fun b -> b = byte a.BlockId)
                if not r then printfn "furious anger"
                s.Free(a)
                allocations.RemoveAt index

        for i in 0 .. 100000 do
            if i%100 = 0 then printfn "it: %d (%d/%A)" i s.Entries s.TotalMemory
            run ()

        printfn "living: %A" allocations.Count
        printfn "%A" s.Statistics

        for a in allocations do
            let arr = s.Read a
            let r = arr |> Array.forall (fun b -> b = byte a.BlockId)
            if not r then printfn "furious anger"
            s.Free(a)

        printfn "%A" s.Statistics

    let moreFileTest () =
        //if File.Exists @"C:\Users\Schorsch\Desktop\test.bin" then File.Delete @"C:\Users\Schorsch\Desktop\test.bin"

        use s = FileManager.directory @"C:\Users\Schorsch\Desktop\blabla"

        printfn "(%d/%A)" s.Files s.TotalMemory

        let allocations = System.Collections.Generic.List()
        let r = Random()

        let run () =
            let op = r.NextDouble() 
            if op < 0.6 || allocations.Count <= 0 then
                let id = Guid.NewGuid()
                let e = s.GetFile(id)
                s.Resize(e, 16L)
                s.Copy(id.ToByteArray(), e)
                allocations.Add(e)
            else 
                let index = r.Next(0,allocations.Count-1)
                let a = allocations.[index]
                allocations.RemoveAt index
                let arr = s.Read a
                let r = a.Id.ToByteArray() = arr
                if not r then printfn "furious anger"
                s.Delete(a)


        for i in 0 .. 1000000 do
            if i%100 = 0 then printfn "it: %d (%d/%A)" i s.Files s.TotalMemory
            run ()

        printfn "%A" s.Statistics
        for a in allocations do
            let arr = s.Read a
            let r = a.Id.ToByteArray() = arr
            if not r then printfn "furious anger"
            s.Delete(a)

        
        printfn "%A" s.Statistics
        printfn "done"

//    let allocPerf() =
//
//        let log = @"C:\Users\schorsch\Desktop\store.csv"
//
//        let test (kind : string) (cnt : int) (mem : Memory) =
//            mem.Clear(1024L)
//            use s = new FileManager.FileManager(mem)
//            let r = System.Random()
//
//            Log.line "size: %d" cnt
//
//            let mutable blocks = Array.zeroCreate cnt
//            let sizes = Array.init cnt (fun _ -> int64 (1 + r.Next 255))
//            let data = Array.zeroCreate 1024
//
//            let sw = Stopwatch()
//            sw.Restart()
//            for i in 0 .. cnt-1 do
//                blocks.[i] <- s.Alloc sizes.[i]
//            sw.Stop()
//            let talloc = sw.MicroTime / float cnt
//            Log.line "alloc: %A" talloc
//
//
//            sw.Restart()
//            for i in 0 .. cnt-1 do
//                s.Copy(data, 0L, blocks.[i], 0L, sizes.[i])
//            sw.Stop()
//            let twrite = sw.MicroTime / float cnt
//            Log.line "write: %A" twrite
//
//            blocks <- blocks.RandomOrder() |> Seq.toArray
//            sw.Restart()
//            for i in 0 .. cnt-1 do
//                s.Free(blocks.[i])
//            sw.Stop()
//            let tfreer = sw.MicroTime / float cnt
//            Log.line "free:  %A (random order)" tfreer
//
//            mem.Clear(1024L)
//            use s = new FileManager.FileManager(mem)
//            for i in 0 .. cnt-1 do
//                blocks.[i] <- s.Alloc sizes.[i]
//            sw.Restart()
//            for i in 0 .. cnt-1 do
//                s.Free(blocks.[i])
//            sw.Stop()
//            let tfrees = sw.MicroTime / float cnt
//            Log.line "free:  %A (sequential)" tfrees
//
//
//
//            File.AppendAllLines(log, [sprintf "%s;%d;%d;%d;%d;%d" kind cnt talloc.TotalNanoseconds twrite.TotalNanoseconds tfreer.TotalNanoseconds tfrees.TotalNanoseconds])
//
//
//        File.WriteAllLines(log, ["mem;size;alloc [ns];write [ns];free (random) [ns]; free (sequential) [ns]"])
//        for i in 10 .. 20 do
//            let cnt = 1 <<< 20
//            MemoryHelpers.hglobal (test "hglobal" cnt)
//            MemoryHelpers.mapped (test "mapped" cnt)

    let fileTest() =
        use s = BlobStore.directory @"" //new BlobStore(Memory.mapped @"C:\Users\Schorsch\Desktop\test.bin")

        let a = s.Get (Guid "d5993e65-6e27-4596-85c1-2a59491477c0")
        let b = s.Get (Guid "d5993e65-6e27-4596-85c1-2a59491477c0")

        
        match a.Exists with
            | true -> 
                let arr = a.Read() 
                Log.line "a = %A" arr
                let arr = Array.append arr [|123uy|]
                a.Write arr

                

            | _ ->
                a.Write(Array.init 10 byte)
                a.Read() |> Log.line "a = %A (created)"
