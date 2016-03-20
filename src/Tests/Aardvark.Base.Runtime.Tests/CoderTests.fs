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
        type StreamWriter(stream : Stream) =
            let bin = new BinaryWriter(stream)

            interface IWriter with
                member x.WritePrimitive (v : 'a) =
                    let mutable v = v
                    let ptr : byte[] = &&v |> NativePtr.cast |> NativePtr.toArray sizeof<'a>
                    code { return bin.Write(ptr) }

                member x.WriteBool(v : bool) =
                    code { return bin.Write(v) }

                member x.WriteString (str : string) =
                    code { return bin.Write(str) }

            interface IDisposable with
                member x.Dispose() = bin.Dispose()

        type StreamReader(stream : Stream) =
            let bin = new BinaryReader(stream)

            interface IReader with
                member x.ReadPrimitive () : Code<'a> =
                    let read() : 'a =
                        let arr = bin.ReadBytes sizeof<'a>
                        let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)
                        let res = gc.AddrOfPinnedObject() |> NativePtr.ofNativeInt |> NativePtr.read
                        gc.Free()
                        res

                    code { return read() }

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

            let coder = ValueCoder.coder<'a>
            let mutable s = emptyState
            coder.Write(w, v).Run(&s)
            ms.ToArray()

        let ofArray<'a> (arr : byte[]) =
            use ms = new MemoryStream(arr)
            use w = new StreamReader(ms)

            let coder = ValueCoder.coder<'a>
            let mutable s = emptyState
            coder.Read(w).Run(&s)

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

    [<Test>]
    let ``Class``() =
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
    let ``Struct (non blittable)``() =
        let mutable input = Struct()
        input.StructA <- 100
        input.StructB <- "hi there"
        let test = roundtrip input

        test.StructA |> should equal input.StructA
        test.StructB |> should equal input.StructB

    [<Test>]
    let ``Struct (blittable)``() =
        let mutable input = BlittableStruct()
        input.BlitA <- 100
        input.BlitB <- 10.0123456
        let test = roundtrip input

        test.BlitA |> should equal input.BlitA
        test.BlitB |> should equal input.BlitB

    [<Test>]
    let ``Record``() =
        let input = { RecA = 10; RecB = "sadsad" }
        let test = roundtrip input
        test |> should equal input

    [<Test>]
    let ``Union``() =
        let input = A 10
        let test = roundtrip input
        test |> should equal input

        let input = B "asdsad"
        let test = roundtrip input
        test |> should equal input

    [<Test>]
    let ``Option``() =
        let input : Option<int> = Some 10
        let test = roundtrip input
        test |> should equal input

        let input : Option<int> = None
        let test = roundtrip input
        test |> should equal input

    [<Test>]
    let ``Tuple``() =
        let input = (10, 10.012345, "hi there")
        let test = roundtrip input
        test |> should equal input

    [<Test>]
    let ``Choice``() =
        let input : Choice<int, string> = Choice1Of2 1
        let test = roundtrip input
        test |> should equal input

        
        let input : Choice<int, string> = Choice2Of2 "bla"
        let test = roundtrip input
        test |> should equal input

    [<Test>]
    let ``List``() =
        let input = [1;2;3;4;5]
        let test = roundtrip input
        test |> should equal input
