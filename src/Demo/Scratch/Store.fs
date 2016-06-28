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
            val mutable public Next : int
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
            member inline x.Next        = NativePtr.ofNativeInt<int>  (56n + x.ptr)

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

    [<AllowNullLiteral>]
    type VirtualNode =
        class
            val mutable public Root     : VirtualNodeRef
            val mutable public BasePtr  : nodeptr
            val mutable public Ptr      : nodeptr
            val mutable public Index    : int

            override x.GetHashCode() =
                x.Index

            override x.Equals o =
                match o with
                    | :? VirtualNode as o -> x.Index = o.Index
                    | _ -> false

            member x.Color
                with get() = !!x.Ptr.Color
                and set v = x.Ptr.Color <-- v

            member x.Offset
                with get() = !!x.Ptr.Offset
                and set v = x.Ptr.Offset <-- v

            member x.Size
                with get() = !!x.Ptr.Size
                and set v = x.Ptr.Size <-- v

            member x.Entry
                with get() = !!x.Ptr.Entry
                and set v = x.Ptr.Entry <-- v
 
            member x.Left = VirtualNodeRef(x.Root, x.BasePtr, x.Ptr.Left)
            member x.Right = VirtualNodeRef(x.Root, x.BasePtr, x.Ptr.Right)

            member x.SetParent(n : VirtualNode) =
                if isNull n then x.Ptr.Parent <-- -1
                else x.Ptr.Parent <-- n.Index

            member x.Parent = 
                let p = !!x.Ptr.Parent
                if p < 0 then
                    VirtualNodeRef.Null
                else
                    let g = !!(x.BasePtr + p).Parent
                    if g < 0 then
                        x.Root
                    else
                        let pParent = x.BasePtr + g
                        if !!pParent.Left = p then VirtualNodeRef(x.Root, x.BasePtr, pParent.Left)   
                        else VirtualNodeRef(x.Root, x.BasePtr, pParent.Right)  
                    
                          
            new(root, basePtr, ptr, index) = { Root = root; BasePtr = basePtr; Ptr = ptr; Index = index }

        end

    and VirtualNodeRef =
        class
            val mutable public Root     : VirtualNodeRef
            val mutable public BasePtr  : nodeptr
            val mutable public Index    : nativeptr<int>

            static member Null = VirtualNodeRef(Unchecked.defaultof<_>, nodeptr.Null, NativePtr.zero)

            member x.Value
                with get() = 
                    if NativePtr.toNativeInt x.Index = 0n then
                        null
                    else
                        let index = !!x.Index
                        if index < 0 then null
                        else VirtualNode(x.Root, x.BasePtr, x.BasePtr + index, index)

                and set (v : VirtualNode) =
                    if isNull v then NativePtr.write x.Index -1
                    else NativePtr.write x.Index v.Index
            
            new (root, basePtr, index) = { Root = root; BasePtr = basePtr; Index = index }
        end

    let inline (!) (r : 'a) = (^a : (member Value : 'b) (r))
    let inline (:=) (r : 'a) (v : 'b) = (^a : (member set_Value : 'b -> unit) (r, v))

    module Tree =
        
        let grandParent (n : VirtualNode) =
            if isNull n then VirtualNodeRef.Null
            else
                let p = !n.Parent
                if isNull p then VirtualNodeRef.Null
                else p.Parent
                
        let uncle (n : VirtualNode) =
            let gpr = grandParent n
            let gp = !gpr
            if isNull gp then 
                VirtualNodeRef.Null, VirtualNodeRef.Null
            elif !gp.Left = !n.Parent then
                gp.Right, gpr
            else 
                gp.Left, gpr
    
        let sibling (n : VirtualNode) =
            if isNull n then VirtualNodeRef.Null
            else
                let p = !n.Parent
                if isNull p then VirtualNodeRef.Null
                else 
                    if !p.Left = n then p.Right
                    else p.Left

        let rotateLeft (n : VirtualNodeRef) =
            let g = !n
            let p = !g.Right
            let parent = !g.Parent

            p.Left.Value.SetParent g
            g.Right := !p.Left

            g.SetParent p
            p.Left := g

            p.SetParent parent
            n := p

        let rotateRight (n : VirtualNodeRef) =
            let g = !n
            let p = !g.Left
            let parent = !g.Parent

            p.Right.Value.SetParent g
            g.Left := !p.Right
                    
            g.SetParent p
            p.Right := g
                    
            p.SetParent parent
            n := p

        module Insert = 
            let rec case1 (nr : VirtualNodeRef) =
                let n = !nr
                if isNull !n.Parent then
                    n.Color <- Black
                else 
                    case2 nr

            and case2 (nr : VirtualNodeRef) = 
                let n = !nr
                if n.Parent.Value.Color <> Black then
                    case3 nr

            and case3 (nr : VirtualNodeRef) =
                let n = !nr
                let ur, gr = uncle n
                let u = !ur
                let g = !gr

                if not (isNull u) && u.Color = Red then
                    n.Parent.Value.Color <- Black
                    u.Color <- Black
                    g.Color <- Red
                    case1 gr
                else
                    case4 nr

            and case4 (nr : VirtualNodeRef) =
                let n = !nr
                let g = !(grandParent n)
                let p = !n.Parent

                let nr = 
                    if n = !p.Right && p = !g.Left then
                        rotateLeft n.Parent
                        n.Left
                    elif n = !p.Left && p = !g.Right then
                        rotateRight n.Parent
                        n.Right
                    else
                        nr
                            
                case5 nr

            and case5 (nr : VirtualNodeRef) =
                let n = !nr
                let gr = grandParent n
                let g = !gr
                let p = !n.Parent

                p.Color <- Black
                g.Color <- Red
                if n = !p.Left then rotateRight gr
                else rotateLeft gr

        module Delete =
            let rec case1 (nr : VirtualNodeRef) =
                let n = !nr
                if not (isNull !n.Parent) then
                    case2 nr

            and case2 (nr : VirtualNodeRef) =
                let n = !nr
                let p = !n.Parent
                let s = !(sibling n)
                if s.Color = Red then
                    p.Color <- Red
                    s.Color <- Black
                    if n = !p.Left then rotateLeft n.Parent
                    else rotateRight n.Parent
                case3 nr

            and case3 (nr : VirtualNodeRef) =
                let n = !nr
                let p = !n.Parent
                let s = !(sibling n)

                if p.Color = Black &&
                   s.Color = Black &&
                   s.Left.Value.Color = Black &&
                   s.Right.Value.Color = Black then
                   s.Color <- Red
                   case1 n.Parent
                else
                    case4 nr

            and case4 (nr : VirtualNodeRef) =
                let n = !nr
                let p = !n.Parent
                let s = !(sibling n)

                if p.Color = Red &&
                   s.Color = Black &&
                   s.Left.Value.Color = Black &&
                   s.Right.Value.Color = Black then
                   s.Color <- Red
                   p.Color <- Black
                else
                    case5 nr

            and case5 (nr : VirtualNodeRef) =
                let n = !nr
                let p = !n.Parent
                let sr = sibling n
                let s = !sr

                if s.Color = Black then
                    if n = !p.Left && s.Right.Value.Color = Black && s.Left.Value.Color = Red then
                        s.Color <- Red
                        s.Left.Value.Color <- Black
                        rotateRight sr
                    elif n = !p.Right && s.Left.Value.Color = Black && s.Right.Value.Color = Red then
                        s.Color <- Red
                        s.Right.Value.Color <- Black
                        rotateLeft sr
                case6 nr
                
            and case6 (nr : VirtualNodeRef) =
                let n = !nr
                let p = !n.Parent
                let sr = sibling n
                let s = !sr
                
                s.Color <- p.Color
                p.Color <- Black
                if n = !p.Left then
                    s.Right.Value.Color <- Black
                    rotateLeft n.Parent
                else
                    s.Left.Value.Color <- Black
                    rotateRight n.Parent


        let rec insert (n : VirtualNode) (tree : VirtualNodeRef) =
            assert (n.Color = Red)

            let t = !tree
            if isNull t then
                n.SetParent t
                tree := n
                Insert.case1 tree
            else
                let cmp = 
                    let c = compare n.Size t.Size 
                    if c = 0 then compare n.Offset t.Offset
                    else c

                if cmp < 0 then
                    insert n t.Left
                elif cmp > 0 then
                    insert n t.Right
                else
                    failwith "duplicated entry"

        let rec findMin (n : VirtualNode) =
            let l = !n.Left
            if isNull l then n
            else findMin l

        let rec remove (n : VirtualNode) (tree : VirtualNodeRef) =
            let t = !tree
            if isNull t then
                false
            else    
                let cmp = 
                    let c = compare n.Size t.Size 
                    if c = 0 then compare n.Offset t.Offset
                    else c


                if cmp < 0 then
                    remove n t.Left
                elif cmp > 0 then
                    remove n t.Right
                else
                    if isNull !t.Left && isNull !t.Right then
                        tree := null
                        true
                    elif isNull !t.Left then
                        let n = t
                        let child = !t.Right
                        tree := child

                        if n.Color = Black then
                            if child.Color = Red then
                                child.Color <- Black
                            else
                                Delete.case1 tree
                        
                        true

                    elif isNull !t.Right then

                        let n = t
                        let child = !t.Left
                        tree := child

                        if n.Color = Black then
                            if child.Color = Red then
                                child.Color <- Black
                            else
                                Delete.case1 tree
                        
                        true
                    else
                        let min = findMin !t.Right
                        t.Offset <- min.Offset
                        t.Size <- min.Size
                        t.Entry <- min.Entry
                        remove min t.Right
     



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

                let e = x.NewEntry(newBlock)
                if x.MLast < 0 then 
                    x.MFirst <- e
                    x.MLast <- e
                else (x.Entries + x.MLast).Block.Next <-- e

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

            member x.NewEntry(b : Block) : int =
                let id =
                    if x.EFreeCount > 0 then
                        let id = x.EFreeList
                        let pEntry = x.Entries + id

                        x.EFreeList <- !!pEntry.Next
                        x.EFreeCount <- x.EFreeCount - 1

                        id
                    else
                        if x.ECount >= x.ECapacity then
                            x.GrowTable()

                        let id = x.ECount
                        x.ECount <- x.ECount + 1
                        id

                let pEntry = x.Entries + id
                pEntry.Index <-- id
                pEntry.Next <-- -1
                pEntry.SetBlock b
                id

            member x.KillEntry(b : entryptr) : unit =
                b.Next <-- x.EFreeList
                x.EFreeList <- !!b.Index
                x.EFreeCount <- x.EFreeCount + 1

            member x.NewNode() : int =
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

                        let id = x.NCount
                        x.NCount <- x.NCount + 1
                        id

                let pNode = x.Nodes + id
                pNode.Left <-- -1
                id

            member x.KillNode(b : int) =
                let n = x.Nodes + b
                n.Left <-- x.NFreeList
                x.NFreeList <- b
                x.NFreeCount <- x.NFreeCount + 1

            member x.WithTree (f : VirtualNodeRef -> 'a) =
                let ptr = NativePtr.stackalloc 1
                NativePtr.write ptr x.NRoot
                let root = VirtualNodeRef(Unchecked.defaultof<_>, x.Nodes, ptr)
                root.Root <- root
                let res = f root
                x.NRoot <- NativePtr.read ptr
                res

            member x.RemoveNode(id : int, res : nodeptr) =
                let worked =
                    x.WithTree(fun root ->
                        let n = VirtualNode(root, x.Nodes, res, id)
                        Tree.remove n root
                    )
                if worked then
                    x.KillNode(id)
//
//                let l = !!res.Left
//                let r = !!res.Right
//                let pi = !!res.Parent
//
//                if pi >= 0 then
//                    let pParent = x.Nodes + !!res.Parent
//                    let ptr = if !!pParent.Left = id then pParent.Left else pParent.Right
//
//                    if l < 0 then
//                        ptr <-- r
//                        x.KillNode(id)
//                    elif r < 0 then
//                        ptr <-- l
//                        x.KillNode(id)
//                    else    
//                        let mutable last = res
//                        let mutable lastIndex = -1
//                        let mutable current = r
//                        while current >= 0 do
//                            let n =  x.Nodes + current
//                            last <- n
//                            lastIndex <- current
//                            current <- !!n.Left
//                        (x.Nodes + l).Parent <-- lastIndex
//                        last.Left <-- l
//                else
//                    if l < 0 then
//                        x.NRoot <- r
//                        x.KillNode(id)
//                    elif r < 0 then
//                        x.NRoot <- l
//                        x.KillNode(id)
//                    else
//                        let mutable last = res
//                        let mutable lastIndex = -1
//                        let mutable current = r
//                        while current >= 0 do
//                            let n =  x.Nodes + current
//                            last <- n
//                            lastIndex <- current
//                            current <- !!n.Left
//                        last.Left <-- l
//                        (x.Nodes + l).Parent <-- lastIndex
//                        (x.Nodes + r).Parent <-- -1
//                        x.NRoot <- r


            member x.InsertFree(pEntry : entryptr) : unit =
                let size = !!pEntry.Block.Size
                let offset = !!pEntry.Block.Offset
                let n = x.NewNode()
                let pNode = x.Nodes + n
                pNode.Offset <-- !!pEntry.Block.Offset
                pNode.Size <-- !!pEntry.Block.Size
                pNode.Left <-- -1
                pNode.Right <-- -1
                pNode.Parent <-- -1
                pNode.Entry <-- !!pEntry.Index
                pNode.Color <-- Red

                x.WithTree (fun root ->
                    let n = VirtualNode(root, x.Nodes, pNode, n)
                    Tree.insert n root
                )
//
//                let mutable parent = NativePtr.zero
//                let mutable last = -1
//                let mutable current = x.NRoot
//
//                while current >= 0 do
//                    let pNode = x.Nodes + current
//
//                    let cmp =
//                        let c = compare size !!pNode.Size
//                        if c = 0 then compare offset !!pNode.Offset
//                        else c
//
//                    if cmp > 0 then
//                        parent <- pNode.Right
//                        last <- current
//                        current <- !!pNode.Right
//                    elif cmp < 0 then
//                        parent <- pNode.Left
//                        last <- current
//                        current <- !!pNode.Left
//                    else
//                        failwith "duplicate entry"
//
//                let n = x.NewNode()
//                let pNode = x.Nodes + n
//                pNode.Offset <-- !!pEntry.Block.Offset
//                pNode.Size <-- !!pEntry.Block.Size
//                pNode.Left <-- -1
//                pNode.Right <-- -1
//                pNode.Parent <-- last
//                pNode.Entry <-- !!pEntry.Index
//
//                let grandParent (n : VirtualNode) =
//                    if isNull n then VirtualNodeRef.Null
//                    else
//                        let p = !n.Parent
//                        if isNull p then VirtualNodeRef.Null
//                        else p.Parent
//                
//                let uncle (n : VirtualNode) =
//                    let gpr = grandParent n
//                    let gp = !gpr
//                    if isNull gp then 
//                        VirtualNodeRef.Null, VirtualNodeRef.Null
//                    elif !gp.Left = !n.Parent then
//                        gp.Right, gpr
//                    else 
//                        gp.Left, gpr
//
//                let rotateLeft (n : VirtualNodeRef) =
//                    let g = !n
//                    let p = !g.Right
//                    let parent = !g.Parent
//
//                    p.Left.Value.Parent := g
//                    g.Right := !p.Left
//
//                    g.Parent := p
//                    p.Left := g
//
//                    p.Parent := parent
//                    n := p
//
//                let rotateRight (n : VirtualNodeRef) =
//                    let g = !n
//                    let p = !g.Left
//                    let parent = !g.Parent
//
//                    p.Right.Value.Parent := g
//                    g.Left := !p.Right
//                    
//                    g.Parent := p
//                    p.Right := g
//                    
//                    p.Parent := parent
//                    n := p
//
//                let rec case1 (nr : VirtualNodeRef) =
//                    let n = !nr
//                    if isNull !n.Parent then
//                        n.Color <- Black
//                    else 
//                        case2 nr
//
//                and case2 (nr : VirtualNodeRef) = 
//                    let n = !nr
//                    if n.Parent.Value.Color <> Black then
//                        case3 nr
//
//                and case3 (nr : VirtualNodeRef) =
//                    let n = !nr
//                    let ur, gr = uncle n
//                    let u = !ur
//                    let g = !gr
//
//                    if not (isNull u) && u.Color = Red then
//                        n.Parent.Value.Color <- Black
//                        u.Color <- Black
//                        g.Color <- Red
//                        case1 gr
//                    else
//                        case4 nr
//
//                and case4 (nr : VirtualNodeRef) =
//                    let n = !nr
//                    let g = !(grandParent n)
//                    let p = !n.Parent
//
//                    let nr = 
//                        if n = !p.Right && p = !g.Left then
//                            rotateLeft n.Parent
//                            n.Left
//                        elif n = !p.Left && p = !g.Right then
//                            rotateRight n.Parent
//                            n.Right
//                        else
//                            nr
//                            
//                    case5 nr
//
//                and case5 (nr : VirtualNodeRef) =
//                    let n = !nr
//                    let gr = grandParent n
//                    let g = !gr
//                    let p = !n.Parent
//
//                    p.Color <- Black
//                    g.Color <- Red
//                    if n = !p.Left then rotateRight gr
//                    else rotateLeft gr
//
//                if parent = NativePtr.zero then
//                    x.NRoot <- n
//                    let ptr = NativePtr.stackalloc 1
//                    NativePtr.write ptr x.NRoot
//                    let root = VirtualNodeRef(Unchecked.defaultof<_>, x.Nodes, ptr)
//                    root.Root <- root
//                    case1 root
//                    x.NRoot <- NativePtr.read ptr
//                else
//                    parent <-- n
//
//                    let ptr = NativePtr.stackalloc 1
//                    NativePtr.write ptr x.NRoot
//                    let root = VirtualNodeRef(Unchecked.defaultof<_>, x.Nodes, ptr)
//                    root.Root <- root
//      
//                    case1 (VirtualNodeRef(root, x.Nodes, parent))
//
//                    x.NRoot <- NativePtr.read ptr


            member x.RemoveFree(pEntry : entryptr)  =
                let size = !!pEntry.Block.Size
                let offset = !!pEntry.Block.Offset

                let mutable res = nodeptr(0n)
                let mutable ptr = NativePtr.zero
                let mutable current = x.NRoot

                while current >= 0 && res.ptr = 0n do
                    let pNode = x.Nodes + current

                    let cmp =
                        let c = compare size !!pNode.Size
                        if c = 0 then compare offset !!pNode.Offset
                        else c

                    if cmp > 0 then
                        ptr <- pNode.Right
                        current <- !!pNode.Right
                    elif cmp < 0 then
                        ptr <- pNode.Left
                        current <- !!pNode.Left
                    else
                        res <- pNode

                if res.ptr <> 0n then
                    x.RemoveNode(current, res)
                    true
                else
                    false
                
            member x.GetFree(size : int64) =
                let mutable best = nodeptr(0n)
                let mutable bestIndex = -1
                let mutable current = x.NRoot
                // search for the smallest node >= size
                while current >= 0 do
                    let pNode = x.Nodes + current

                    let c = compare size !!pNode.Size

                    if c > 0 then
                        current <- !!pNode.Right

                    elif c < 0 then
                        best <- pNode
                        bestIndex <- current
                        current <- !!pNode.Left

                    else
                        best <- pNode
                        bestIndex <- current
                        current <- -1

                // if we found a best-fit use it (removing it from the tree)
                if best.ptr <> 0n then
                    let e = !!best.Entry
                    let pEntry = x.Entries + e

                    let n = !!pEntry.Next
                    if n >= 0 then
                        best.Entry <-- n
                    else
                        x.RemoveNode(bestIndex, best)

                    pEntry
                
                // otherwise increase memory-size and retry
                else
                    x.GrowData size
                    x.GetFree size

            member x.Alloc(size : int64) =
                let pEntry = x.GetFree(size)
                let pBlock = pEntry.Block
                let blockSize = !!pBlock.Size
                if blockSize > size then
                    let newBlock = 
                        Block(
                            Offset = !!pBlock.Offset + size, 
                            Size = blockSize - size, 
                            IsFree = 1, 
                            Prev = !!pEntry.Index, 
                            Next = !!pEntry.Next
                        )
                    
                    let eid = x.NewEntry(newBlock)

                    let nn = !!pBlock.Next
                    if nn < 0 then x.MLast <- eid
                    else (x.Entries + nn).Block.Prev <-- eid

                    pBlock.Next <-- eid 
                    pBlock.Size <-- size

                    x.InsertFree (x.Entries + eid)





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
                        x.MFirst <- !!pEntry.Index

                    elif !!pPrevBlock.IsFree <> 0 then
                        // merge with prev
                        x.RemoveFree pPrev |> ignore

                        pBlock.Offset <-- !!pPrevBlock.Offset
                        pBlock.Size <-- !!pPrevBlock.Size + !!pBlock.Size


                        let prev = !!pPrevBlock.Prev
                        pBlock.Prev <-- prev
                        if prev < 0 then x.MFirst <- !!pEntry.Index
                        else (x.Entries + prev).Block.Next <-- !!pEntry.Index

                        x.KillEntry(pPrev)

                    if next < 0 then
                        x.MLast <- !!pEntry.Index

                    elif !!pNextBlock.IsFree <> 0 then
                        // merge with next
                        x.RemoveFree pNext |> ignore

                        pBlock.Size <-- !!pBlock.Size + !!pNextBlock.Size

                        let next = !!pNextBlock.Next
                        pBlock.Next <-- next
                        if next < 0 then x.MLast <- !!pEntry.Index
                        else (x.Entries + next).Block.Prev <-- !!pEntry.Index
                    
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
            if op < 0.5 || allocations.Count <= 0 then
                let ssize = 1+r.Next(255)
                let e = s.Alloc(int64 ssize)
                allocations.Add(e)
                s.Write(e,Array.create ssize (byte e))
            else 
                let index = r.Next(0,allocations.Count-1)
                let a = allocations.[index]
                let r = s.Read a |> Array.forall (fun b -> b = byte a)
                if not r then printfn "hate"
                s.Free(a)
                allocations.RemoveAt index

        for i in 0 .. 10000 do
            if i%100 = 0 then printfn "it: %d" i
            run ()