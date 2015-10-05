namespace Aardvark.Base.FSharp.Tests
#nowarn "44"

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework
open System.Runtime.InteropServices


module MemoryManagerTests =
    
    let create() = MemoryManager(16, Marshal.AllocHGlobal, fun ptr _ -> Marshal.FreeHGlobal ptr)

    let rec validateBlocks (last : Block) (current : Block) =
        let mutable last = last
        let mutable current = current
        while current <> null do
            current.Prev |> should equal last

            current.Size |> should greaterThan 0

            if not (isNull last) then
                last.Offset + nativeint last.Size |> should equal current.Offset

            last <- current
            current <- current.Next


    [<Test>]
    let ``[Memory] simple alloc test``() =
        let m = create()

        let b0 = m.Alloc(10)
        let b1 = m.Alloc(6)

        validateBlocks null m.FirstBlock
        m.FirstBlock |> should equal b0
        m.LastBlock |> should equal b1

    [<Test>]
    let ``[Memory] simple free test``() =
        let m = create()

        let b0 = m.Alloc(10)
        m.Free b0


        let b1 = m.Alloc(16)
        
        validateBlocks null m.FirstBlock
        m.FirstBlock |> should equal b1
        m.LastBlock |> should equal b1

    [<Test>]
    let ``[Memory] free collapse left``() =
        let m = create()

        let b0 = m.Alloc(2)
        let b1 = m.Alloc(2)
        let b2 = m.Alloc(2)
        m.Free b0
        m.Free b1

        
        validateBlocks null m.FirstBlock
        m.FirstBlock.Size |> should equal 4
        m.FirstBlock.Next |> should equal b2
        m.LastBlock.Prev |> should equal b2

    [<Test>]
    let ``[Memory] free collapse right``() =
        let m = create()

        let b0 = m.Alloc(2)
        let b1 = m.Alloc(2)
        let b2 = m.Alloc(2)
        let r = m.Alloc(10)
        m.Free b2
        m.Free b1

        
        validateBlocks null m.FirstBlock
        m.FirstBlock |> should equal b0
        b0.Next.Size |> should equal 4
        m.LastBlock |> should equal b0.Next.Next

    [<Test>]
    let ``[Memory] free collapse both``() =
        let m = create()

        let b0 = m.Alloc(2)
        let b1 = m.Alloc(2)
        let b2 = m.Alloc(2)
        let r = m.Alloc(10)
        m.Free b2
        m.Free b0
        m.Free b1

        
        validateBlocks null m.FirstBlock
        m.FirstBlock.Size |> should equal 6
        m.FirstBlock.Offset |> should equal 0n
        m.FirstBlock.Next |> should equal r
        m.LastBlock |> should equal r


    [<Test>]
    let ``[Memory] realloc move``() =
        let m = create()

        let b0 = m.Alloc(2)
        let b1 = m.Alloc(2)
        
        m.Realloc(b0, 4) |> should be True
        validateBlocks null m.FirstBlock

        b0.Size |> should equal 4
        b0.Offset |> should equal 4n
        b0.Prev |> should equal b1

        b1.Prev.Size |> should equal 2
        b1.Prev.Offset |> should equal 0n

        m.FirstBlock.Next |> should equal b1
        m.LastBlock.Prev |> should equal b0

    [<Test>]
    let ``[Memory] realloc space left``() =
        let m = create()

        let b0 = m.Alloc(2)
        let b1 = m.Alloc(4)
        let b2 = m.Alloc(2)
        
        m.Free(b1)

        m.Realloc(b0, 6) |> should be False
        validateBlocks null m.FirstBlock

        b0.Next |> should equal b2
        m.FirstBlock |> should equal b0

    [<Test>]
    let ``[Memory] realloc shrink``() =
        let m = create()

        let b0 = m.Alloc(2)
        let b1 = m.Alloc(4)

        m.Realloc(b0, 1) |> should be False
        validateBlocks null m.FirstBlock

        b0.Next.Next |> should equal b1
        m.FirstBlock |> should equal b0

    [<Test>]
    let ``[Memory] realloc 0``() =
        let m = create()

        let b0 = m.Alloc(2)
        let b1 = m.Alloc(4)

        m.Realloc(b0, 0) |> should be False
        validateBlocks null m.FirstBlock

        b0.Size |> should equal 0
        m.FirstBlock |> should not' (equal b0)
        m.FirstBlock.Size |> should equal 2
        m.FirstBlock.Offset |> should equal 0n


    [<Test>]
    let ``[Memory] resize``() =
        let m = create()

        let b0 = m.Alloc(10)
        let b1 = m.Alloc(100)

        validateBlocks null m.FirstBlock
        m.FirstBlock |> should equal b0
        m.LastBlock.Prev |> should equal b1