namespace Aardvark.Base.Runtime.Tests

#nowarn "9"
#nowarn "51"

open System
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Runtime
open Aardvark.Base.Runtime
open NUnit.Framework
open FsUnit
open System.IO

module CoderTests =
    
    [<AutoOpen>]
    module Utilities = 
        type private ArrayCoerce<'a, 'b when 'a : unmanaged and 'b : unmanaged>() =
            
            static let sa = sizeof<'a>
            static let sb = sizeof<'b>

//            static let lengthOffset = sizeof<nativeint> |> nativeint
//            static let typeOffset = 2n * lengthOffset

            static let idb : nativeint =
                let gc = GCHandle.Alloc(Array.zeroCreate<'b> 1, GCHandleType.Pinned)
                try 
                    let ptr = gc.AddrOfPinnedObject() |> NativePtr.ofNativeInt
                    NativePtr.get ptr -2
                finally
                    gc.Free()


            static member Coerce (a : 'a[]) : 'b[] =
                let newLength = (a.Length * sa) / sb |> nativeint
                let gc = GCHandle.Alloc(a, GCHandleType.Pinned)
                try
                    let ptr = gc.AddrOfPinnedObject() |> NativePtr.ofNativeInt<nativeint>

                    NativePtr.set ptr -1 newLength
                    NativePtr.set ptr -2 idb

                    a |> unbox<'b[]>

                finally
                    gc.Free()

            static member CoercedApply (f : 'b[] -> 'r) (a : 'a[]) : 'r =
                let newLength = (a.Length * sa) / sb |> nativeint
                let gc = GCHandle.Alloc(a, GCHandleType.Pinned)
                try
                    let ptr = gc.AddrOfPinnedObject() |> NativePtr.ofNativeInt<nativeint>

                    let oldLength = NativePtr.get ptr -1
                    let oldType = NativePtr.get ptr -2

                    NativePtr.set ptr -1 newLength
                    NativePtr.set ptr -2 idb

                    try
                        a |> unbox<'b[]> |> f
                    finally
                        NativePtr.set ptr -1 oldLength
                        NativePtr.set ptr -2 oldType

                finally
                    gc.Free()


        type StreamWriter(stream : Stream) =
            let bin = new BinaryWriter(stream)

            member x.WritePrimitive (v : 'a) =
                let mutable v = v
                let ptr : byte[] = &&v |> NativePtr.cast |> NativePtr.toArray sizeof<'a>
                code { return bin.Write(ptr) }

            interface IWriter with
                member x.WritePrimitive (v : 'a) = x.WritePrimitive v
                member x.WritePrimitiveArray(data : 'a[]) =
                    code {
                        if isNull data then
                            do! x.WritePrimitive -1
                        else
                            do! x.WritePrimitive data.Length
                            data |> ArrayCoerce<'a, byte>.CoercedApply bin.Write
                    }

                member x.WriteBool(v : bool) =
                    code { return bin.Write(v) }

                member x.WriteString (str : string) =
                    code { return bin.Write(str) }

            interface IDisposable with
                member x.Dispose() = bin.Dispose()

        type StreamReader(stream : Stream) =
            let bin = new BinaryReader(stream)

            member x.ReadPrimitive () : Code<'a> =
                let read() : 'a =
                    let arr = bin.ReadBytes sizeof<'a>
                    let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)
                    let res = gc.AddrOfPinnedObject() |> NativePtr.ofNativeInt |> NativePtr.read
                    gc.Free()
                    res

                code { return read() }
            interface IReader with
                member x.ReadPrimitive () = x.ReadPrimitive()

                member x.ReadPrimitiveArray() : Code<'a[]> =
                    code {
                        let! length = x.ReadPrimitive()
                        if length < 0 then
                            return null
                        else
                            let data = bin.ReadBytes(sizeof<'a> * length)
                            return ArrayCoerce<byte, 'a>.Coerce data
                    }

                member x.ReadBool() =
                    code { return bin.ReadBoolean() }

                member x.ReadString () =
                    code { return bin.ReadString() }

            interface IDisposable with
                member x.Dispose() = bin.Dispose()

        let emptyState =
            {
                Version         = Version(0,0,1)
                MemberStack     = []
                References      = RefMap.empty
                Values          = Map.empty
                Database        = Unchecked.defaultof<_>
            }

        let toArray (v : 'a) =
            use ms = new MemoryStream()
            use w = new StreamWriter(ms)
            w.Write(v)
            ms.ToArray()

        let ofArray<'a> (arr : byte[]) : 'a =
            use r = new StreamReader(new MemoryStream(arr))
            r.Read()

        let roundtrip (value : 'a) : 'a =
            let arr = toArray value
            ofArray arr

    type Class =
        class
            val mutable public ClassA : int
            val mutable public ClassB : obj

            new() = { ClassA = 0; ClassB = null }
        end

    type Struct =
        struct
            val mutable public StructA : int
            val mutable public StructB : obj

        end

    type Rec =
        {
            RecA : int
            RecB : obj
        }

    type BlittableStruct =
        struct
            val mutable public BlitA : int
            val mutable public BlitB : float

        end

    type Union =
        | A of int
        | B of obj

    let inline simpletest a =
        let res = roundtrip a
        res |> should equal a


    [<Test>]
    let ``[Coder] int``() = simpletest 1

    [<Test>]
    let ``[Coder] bool``() = simpletest true

    [<Test>]
    let ``[Coder] string``() = simpletest "hi there äü³²#"

    [<Test>]
    let ``[Coder] Type``() = simpletest typeof<int * obj * string>


    [<Test>]
    let ``[Coder] Class``() =
        let a = Class()
        a.ClassB <- a
        a.ClassA <- 10

        let arr = toArray a
        Log.line "data: %A" arr

        let tmp = ofArray<Class> arr

        tmp.ClassA |> should equal a.ClassA
        tmp.ClassB == (tmp :> obj) |> should be True


        ()

    [<Test>]
    let ``[Coder] Struct (non blittable)``() =
        let mutable input = Struct()
        input.StructA <- 100
        input.StructB <- "hi there"
        let test = roundtrip input

        test.StructA |> should equal input.StructA
        test.StructB |> should equal input.StructB

    [<Test>]
    let ``[Coder] Struct (blittable)``() =
        let mutable input = BlittableStruct()
        input.BlitA <- 100
        input.BlitB <- 10.0123456
        let test = roundtrip input

        test.BlitA |> should equal input.BlitA
        test.BlitB |> should equal input.BlitB

    [<Test>]
    let ``[Coder] Record``() =
        let input = { RecA = 10; RecB = "sadsad" }
        let test = roundtrip input
        test |> should equal input

    [<Test>]
    let ``[Coder] Union``() =
        simpletest (A 10)
        simpletest (B "asdsad")

    [<Test>]
    let ``[Coder] Option<int>``() =
        simpletest (Some 10)
        simpletest Option<int>.None


    [<Test>]
    let ``[Coder] Option<obj>``() =
        simpletest (Some (10 :> obj))
        simpletest Option<obj>.None

    [<Test>]
    let ``[Coder] int * float * string``() =
        simpletest (10, 10.012345, "hi there")

    [<Test>]
    let ``[Coder] Choice<int, string>``() =
        let input : Choice<int, string> = Choice1Of2 1
        simpletest input
        let input : Choice<int, string> = Choice2Of2 "bla"
        simpletest input

    [<Test>]
    let ``[Coder] list<int>``() =
        simpletest [1;2;3;4;5]

    [<Test>]
    let ``[Coder] int[]``() =
        simpletest [|1;2;3;4;5|]

    [<Test>]
    let ``[Coder] int[,]``() =
        let input = Array2D.init 10 10 (fun x y -> x + y)
        simpletest input

    [<Test>]
    let ``[Coder] int[] (null)``() =
        let input : int[] = null
        simpletest input

    [<Test>]
    let ``[Coder] int[,] (null)``() =
        let input : int[,] = null
        simpletest input

    [<Test>]
    let ``[Coder] int[,,]``() =
        let input = Array3D.init 10 10 10 (fun x y z -> x + y + z)
        simpletest input

    [<Test>]
    let ``[Coder] obj[]``() =
        simpletest [|"asdas" :> obj; 2 :> obj; null; [|1;2;3|] :> obj|]

    [<Test>]
    let ``[Coder] top-level types``() =
        let arr = toArray 1
        let o : obj = ofArray arr

        o |> should equal 1

        ()

