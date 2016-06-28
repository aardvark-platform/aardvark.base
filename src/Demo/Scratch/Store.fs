module StoreTest

open System
open System.Text
open System.Diagnostics
open System.Security.Cryptography
open Aardvark.Base
open Aardvark.Base.Native
open System.IO
open Microsoft.FSharp.NativeInterop

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

    let inline (++) (ptr : nativeptr<'a>) (v : 'a) = NativePtr.add ptr (int v)
    let inline (!!) (ptr : nativeptr<'a>) = NativePtr.read ptr
    let inline (<--) (ptr : nativeptr<'a>) c = NativePtr.write ptr c

    module Sizes = 
        [<Literal>] 
        let BlockSize = 32
        let EntrySize = 56

    [<Literal>]
    let Red = 0
    [<Literal>]
    let Black = 1

    [<StructLayout(LayoutKind.Explicit, Size = Sizes.BlockSize)>]
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
        
    [<StructLayout(LayoutKind.Explicit, Size = 48)>]
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
            val mutable public Parent : int
            [<FieldOffset(32)>]
            val mutable public Color : int
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
            member inline x.Parent      = NativePtr.ofNativeInt<int> (28n + x.ptr)
            member inline x.Color       = NativePtr.ofNativeInt<int> (32n + x.ptr)

            
            member inline x.Value = NativePtr.read (NativePtr.ofNativeInt<Node> x.ptr)

            static member inline (+) (ptr : nodeptr, index : int) = nodeptr(ptr.ptr + nativeint (sizeof<Node> * index))
            static member Null = nodeptr(0n)

            new(p) = { ptr = p }
        end

    let inline (!) (r : 'a) = (^a : (member Value : 'b) (r))
    let inline (:=) (r : 'a) (v : 'b) = (^a : (member set_Value : 'b -> unit) (r, v))


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
        

    type Store =
        class
            
            new() =
                {
                    MFirst = -1
                    MLast = -1
                    MCapacity = 0L

                    NRoot = -1
                    NCount = 0
                    NFreeList = -1
                    NFreeCount = 0

                    ECount = 0
                    EFreeList = -1
                    EFreeCount = 0

                    DCapacity = 0
                    DCapacityId = -1

                    Pointer = 0n
                    Nodes = nodeptr(0n)
                    Entries = entryptr(0n)
                    Buckets = NativePtr.zero
                    Memory = 0n
                }

            val mutable public MFirst       : int
            val mutable public MLast        : int
            val mutable public MCapacity    : int64

            val mutable public NRoot        : int
            val mutable public NCount       : int
            val mutable public NFreeList    : int
            val mutable public NFreeCount   : int
            member x.NCapacity = if x.DCapacity > 0 then x.DCapacity + 1 else 0

            val mutable public ECount       : int
            val mutable public EFreeList    : int
            val mutable public EFreeCount   : int
            member x.ECapacity = if x.DCapacity > 0 then x.DCapacity * 2 + 1 else 0
          
            val mutable public DCapacity    : int
            val mutable public DCapacityId  : int

            val mutable public Pointer      : nativeint
            val mutable public Nodes        : nodeptr
            val mutable public Entries      : entryptr
            val mutable public Buckets      : nativeptr<int>
            val mutable public Memory       : nativeint

            member x.CheckLinkedList() =
                let mutable success = true

                let mutable last = -1
                let mutable pLast = x.Entries + last
                let mutable current = x.MFirst
                let mutable pCurrent = x.Entries + current

                let off = !!pCurrent.Block.Offset
                if off <> 0L then
                    Log.warn "invalid first offset %A" off
                    success <- false

                while current >= 0 do
                    let prev = !!pCurrent.Block.Prev
                    let next = !!pCurrent.Block.Next

                    if prev <> last then
                        Log.warn "invalid prev: %A" prev
                        success <- false

                    if last >= 0 then
                        let lastOff = !!pLast.Block.Offset
                        let lastSize = !!pLast.Block.Size
                        let lastEnd = lastOff + lastSize
                        let off = !!pCurrent.Block.Offset
                        if off <> lastEnd then
                            Log.warn "invalid offset: %A (should be %A)" off lastEnd
                            success <- false

                    current <- !!pCurrent.Block.Next







            member x.GrowData(needed : int64) =
                let newCapacity = Fun.NextPowerOfTwo (max 1024L (x.MCapacity + needed))

                let total = 
                    int64 x.NCapacity * int64 sizeof<Node> + 
                    int64 x.ECapacity * int64 sizeof<Entry> + 
                    int64 x.DCapacity * int64 sizeof<int> + 
                    newCapacity

                let offset, size = 
                    if x.Pointer <> 0n then
                        x.Pointer <- Marshal.ReAllocHGlobal(x.Pointer, nativeint total)
                        let offset = x.MCapacity
                        let newSize = newCapacity - x.MCapacity
                        x.MCapacity <- newCapacity
                        offset, newSize
                    else
                        x.Pointer <- Marshal.AllocHGlobal(nativeint total)
                        x.MCapacity <- newCapacity
                        0L, newCapacity

                let newBlock = 
                    Block(
                        Offset = offset, 
                        Size = size, 
                        IsFree = 0, 
                        Prev = x.MLast, 
                        Next = -1
                    )

                let ptr = x.Pointer
                x.Nodes <- ptr |> nodeptr

                let ptr = ptr + nativeint (sizeof<Node> * x.NCapacity)
                x.Entries <- ptr |> entryptr

                let ptr = ptr + nativeint (sizeof<Entry> * x.ECapacity)
                x.Buckets <- ptr |> NativePtr.ofNativeInt

                let ptr = ptr + nativeint (sizeof<int> * x.DCapacity)
                x.Memory <- ptr

                let e,_ = x.NewEntry(newBlock)
                if x.MLast < 0 then 
                    x.MFirst <- e
                    x.MLast <- e
                else 
                    (x.Entries + x.MLast).Block.Next <-- e

                x.Free(e)

            member x.GrowTable() =
                let newCapacity = if x.DCapacity = 0 then 1024 else 2 * x.DCapacity

                let nCapacity = if newCapacity > 0 then newCapacity + 1 else 0
                let eCapacity = if newCapacity > 0 then 2 * newCapacity + 1 else 0
                let dCapcaity = newCapacity

                let oldMOffset =
                    int64 x.NCapacity * int64 sizeof<Node> + 
                    int64 x.ECapacity * int64 sizeof<Entry> + 
                    int64 x.DCapacity * int64 sizeof<int>

                let newMOffset = 
                    int64 nCapacity * int64 sizeof<Node> + 
                    int64 eCapacity * int64 sizeof<Entry> + 
                    int64 dCapcaity * int64 sizeof<int>

                let total = newMOffset + x.MCapacity

                if x.Pointer <> 0n then
                    x.Pointer <- Marshal.ReAllocHGlobal(x.Pointer, nativeint total)
                    
                    let is = x.Pointer + nativeint oldMOffset
                    let should = x.Pointer + nativeint newMOffset
                    Marshal.Move(is, should, x.MCapacity)
                    x.Memory <- should

                    let oldSize     = nativeint (sizeof<int> * x.DCapacity)
                    let newSize     = nativeint (sizeof<int> * dCapcaity)
                    let is          = is - oldSize
                    let should      = should - newSize
                    // set all to 0
                    Marshal.Move(is, should, oldSize)
                    Marshal.Set(should + oldSize, 0, newSize - oldSize)
                    x.Buckets <- NativePtr.ofNativeInt should

                    let oldSize     = nativeint (sizeof<Entry> * x.ECapacity)
                    let newSize     = nativeint (sizeof<Entry> * eCapacity)
                    let is          = is - oldSize
                    let should      = should - newSize
                    Marshal.Move(is, should, oldSize)
                    Marshal.Set(should + oldSize, 0, newSize - oldSize)
                    x.Entries <- entryptr should

                    let oldSize     = nativeint (sizeof<Node> * x.NCapacity)
                    let newSize     = nativeint (sizeof<Node> * nCapacity)
                    Marshal.Set(x.Pointer + oldSize, 0, newSize - oldSize)
                    x.Nodes <- nodeptr x.Pointer

                    x.DCapacity <- newCapacity
                    

                else
                    x.Pointer <- Marshal.AllocHGlobal(nativeint total)

                    x.DCapacity <- newCapacity
                    let ptr = x.Pointer
                    x.Nodes <- ptr |> nodeptr

                    let ptr = ptr + nativeint (sizeof<Node> * x.NCapacity)
                    x.Entries <- ptr |> entryptr

                    let ptr = ptr + nativeint (sizeof<Entry> * x.ECapacity)
                    x.Buckets <- ptr |> NativePtr.ofNativeInt

                    let ptr = ptr + nativeint (sizeof<int> * x.DCapacity)
                    x.Memory <- ptr

                // repair buckets

            member x.NewEntry(b : Block) : int * bool =
                let mutable resized = false
                let id =
                    if x.EFreeCount > 0 then
                        let id = x.EFreeList
                        let pEntry = x.Entries + id

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

                let pEntry = x.Entries + id
                pEntry.Index <-- id
                pEntry.NextEntry <-- -1
                pEntry.HashCode <-- -1
                pEntry.SetBlock b
                id, resized

            member x.KillEntry(b : entryptr) : unit =
                b.NextEntry <-- x.EFreeList
                x.EFreeList <- !!b.Index
                x.EFreeCount <- x.EFreeCount + 1

            member x.NewNode() : int * bool =
                let mutable resized = false
                let id =
                    if x.NFreeCount > 0 then
                        let id = x.NFreeList
                        let pNode = x.Nodes + id

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

                let pNode = x.Nodes + id
                pNode.Left <-- -1
                id, resized

            member x.KillNode(b : int) =
                let n = x.Nodes + b
                n.Left <-- x.NFreeList
                x.NFreeList <- b
                x.NFreeCount <- x.NFreeCount + 1

            member x.WithTree (f : ref<VirtualNode> -> 'a) =
                let root = 
                    if x.NRoot < 0 then null
                    else VirtualNode(x.Nodes + x.NRoot, x.NRoot)

                let r = ref root
                let res = f r
                let root = !r

                if isNull root then x.NRoot <- -1
                else x.NRoot <- root.Index

                res


            member x.AllBlocks =
                [|
                    let mutable current = x.MFirst
                    while current >= 0 do
                        let pBlock = (x.Entries + current).Block
                        yield current, pBlock.Value
                        current <- !!pBlock.Next
                |]
//
//            member x.RemoveNode(id : int, res : nodeptr) =
//                let worked =
//                    x.WithTree(fun root ->
//                        let n = VirtualNode(res, id)
//                        Tree.remove n &root.contents
//                    )
//
//                if worked then
//                    x.KillNode(id)


            member x.InsertFree(pEntry : entryptr) : unit =
                let iEntry = !!pEntry.Index
                let size = !!pEntry.Block.Size
                let offset = !!pEntry.Block.Offset
                let n,_ = x.NewNode()
                let pEntry = ()

                let pNode = x.Nodes + n
                pNode.Offset <-- offset
                pNode.Size <-- size
                pNode.Left <-- -1
                pNode.Right <-- -1
                pNode.Parent <-- -1
                pNode.Entry <-- iEntry
                pNode.Color <-- Red

                x.WithTree (fun root ->
                    let nn = VirtualNode(pNode, n)
                    let worked = Tree.insert nn &root.contents
                    if not worked then
                        Log.warn "could not add to freelist: %A" (offset, size)
                        x.KillNode n
                    
                )

            member x.RemoveFree(pEntry : entryptr)  =
                let size = !!pEntry.Block.Size
                let offset = !!pEntry.Block.Offset

                let killed =
                    x.WithTree(fun root ->
                        Tree.removeKey offset size &root.contents
                    )

                if not (isNull killed) then
                    x.KillNode(killed.Index)
                    true
                else
                    Log.warn "could not remove from freelist: %A" (offset, size)
                    false
//                let mutable res = nodeptr(0n)
//                let mutable ptr = NativePtr.zero
//                let mutable current = x.NRoot
//
//                while current >= 0 && res.ptr = 0n do
//                    let pNode = x.Nodes + current
//
//                    let cmp =
//                        let c = compare size !!pNode.Size
//                        if c = 0 then compare offset !!pNode.Offset
//                        else c
//
//                    if cmp > 0 then
//                        ptr <- pNode.Right
//                        current <- !!pNode.Right
//                    elif cmp < 0 then
//                        ptr <- pNode.Left
//                        current <- !!pNode.Left
//                    else
//                        res <- pNode
//
//                if res.ptr <> 0n then
//                    x.RemoveNode(current, res)
//                    true
//                else
//                    false
                
            member x.GetFree(size : int64) =
                let node =
                    x.WithTree (fun tree ->
                        Tree.removeSmallestGreater size &tree.contents
                    )

                if isNull node then 
                    x.GrowData size
                    x.GetFree size
                else
                    let e = node.Entry
                    let pEntry = x.Entries + e
                    x.KillNode node.Index
                    pEntry
//                let mutable best = nodeptr(0n)
//                let mutable bestIndex = -1
//                let mutable current = x.NRoot
//                // search for the smallest node >= size
//                while current >= 0 do
//                    let pNode = x.Nodes + current
//
//                    let c = compare size !!pNode.Size
//
//                    if c > 0 then
//                        current <- !!pNode.Right
//
//                    elif c < 0 then
//                        best <- pNode
//                        bestIndex <- current
//                        current <- !!pNode.Left
//
//                    else
//                        best <- pNode
//                        bestIndex <- current
//                        current <- -1
//
//                // if we found a best-fit use it (removing it from the tree)
//                if best.ptr <> 0n then
//                    let e = !!best.Entry
//                    let pEntry = x.Entries + e
//
//                    let n = !!pEntry.Next
//                    if n >= 0 then
//                        best.Entry <-- n
//                    else
//                        x.RemoveNode(bestIndex, best)
//
//                    pEntry
//                
//                // otherwise increase memory-size and retry
//                else
//                    x.GrowData size
//                    x.GetFree size

            member x.Alloc(size : int64) =
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
                        pEntry <- x.Entries + iEntry
                        pBlock <- pEntry.Block

                    let pNewEntry = x.Entries + eid
                    let pNewBlock = pNewEntry.Block

                    let nn = !!pNewBlock.Next
                    if nn < 0 then x.MLast <- eid
                    else (x.Entries + nn).Block.Prev <-- eid

                    pBlock.Size <-- size
                    pBlock.Next <-- eid 
                    x.InsertFree pNewEntry







                pBlock.IsFree <-- 0
                !!pEntry.Index

            member x.Free(eid : int) =
                let pEntry = x.Entries + eid
                let pBlock = pEntry.Block

                if !!pBlock.IsFree = 0 then
                    let prev = !!pBlock.Prev
                    let next = !!pBlock.Next
                    let pPrev = x.Entries + prev
                    let pNext = x.Entries + next
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
                        else (x.Entries + prev).Block.Next <-- eid

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
                        else (x.Entries + next).Block.Prev <-- eid
                    
                        x.KillEntry(pNext)


                    pEntry.Key <-- Guid.Empty
                    pEntry.HashCode <-- -1
                    pBlock.IsFree <-- 1
                    x.InsertFree(pEntry)

            member x.Write(eid : int, data : byte[]) =
                let pEntry = x.Entries + eid
                let offset = nativeint !!pEntry.Block.Offset
                let size = int64 !!pEntry.Block.Size

                let gc = GCHandle.Alloc(data, GCHandleType.Pinned)
                try Marshal.Copy(gc.AddrOfPinnedObject(), x.Memory + offset, min size data.LongLength)
                finally gc.Free()

            member x.Read(eid : int, data : byte[]) =
                let pEntry = x.Entries + eid
                let offset = nativeint !!pEntry.Block.Offset
                let size = int64 !!pEntry.Block.Size

                let gc = GCHandle.Alloc(data, GCHandleType.Pinned)
                try Marshal.Copy(x.Memory + offset, gc.AddrOfPinnedObject(), min size data.LongLength)
                finally gc.Free()

            member x.Read(eid : int) =
                let pEntry = x.Entries + eid
                let size = int64 !!pEntry.Block.Size

                let arr = Array.zeroCreate (int size)
                x.Read(eid, arr)
                arr
//            member x.NewFile(key : Guid, size : int64) =
//                let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
//                let pEntry = x.Alloc size
//                pEntry.Key <-- key
//                pEntry.HashCode <-- hashCode
//
//                let index = (pEntry.ptr - x.Entries.ptr) / nativeint sizeof<Entry> |> int
//                let pBucket = x.Buckets ++ hashCode % x.Capacity
//                let ei = !!pBucket - 1
//
//                if ei < 0 then
//                    pBucket <-- index + 1
//                else
//                    pEntry.Next <-- ei
//                    pBucket <-- index + 1
//
//            member x.FindEntry(key : Guid, hashCode : int) =
//                let pBucket = x.Buckets ++ hashCode % x.Capacity
//                let ei = !!pBucket - 1
//
//                if ei < 0 then 
//                    -1
//                else
//                    let pEntry = x.Entries + ei
//                    if !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
//                        ei
//                    else
//                        let rec search (id : int) (pEntry : entryptr) =
//                            if id < 0 then
//                                -1
//                            elif !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
//                                id
//                            else
//                                let n = !!pEntry.Next
//                                search n (x.Entries + n)
//
//                        let n = !!pEntry.Next
//                        search n (x.Entries + n)
//
//            member x.Delete(key : Guid) =
//                let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
//                let id = x.FindEntry(key, hashCode)
//                if id >= 0 then
//                    
//
//
//                    true
//                else
//                    false

        end


    let test() =
        let s = Store()
        
        let data = Array.init 200 byte
        let e = s.Alloc(data.LongLength)

        s.Write(e, data)
        let r = s.Read(e)
        printfn "%A" r

        s.Free(e)


    let moreTest () =

        let s = Store()
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
            if i%100 = 0 then printfn "it: %d (%d/%A)" i s.ECapacity (Mem s.MCapacity)
            run ()