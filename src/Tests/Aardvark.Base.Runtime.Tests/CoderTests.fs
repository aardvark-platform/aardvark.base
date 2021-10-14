namespace Aardvark.Base.Runtime.Tests

#nowarn "9"
#nowarn "51"

open System
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Base
open Aardvark.Base.Runtime
open NUnit.Framework
open FsUnit
open System.IO

[<AbstractClass; Sealed; Extension>]
type CoderExtensions private() =
    
    [<Extension>]
    static member Code(c : ICoder, value : ref<Symbol>) =
        code {
            if c.IsReading then 
                let! str = c.ReadString()
                value := Symbol.Create str
            else
                do! c.WriteString (value.Value.ToString())
        }

    [<Extension>]
    static member Code(c : ICoder, l : ref<list<'a>>) =
        code {
            if c.IsReading then
                let! arr = c.ReadState()
                l := Array.toList arr
            else
                let arr = !l |> List.toArray
                do! c.WriteState arr
        }



module CoderTests =
    open Aardvark.Base.Runtime.FieldCoders

    do  let ass = typeof<CoderExtensions>.Assembly
        System.Environment.CurrentDirectory <- System.IO.Path.GetDirectoryName(ass.Location)
        IntrospectionProperties.CustomEntryAssembly <- ass
        
        Aardvark.Init()

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


            static member Copy (a : 'a[]) : 'b[] =
                let bytes = (a.Length * sa) |> nativeint
                let gc = GCHandle.Alloc(a, GCHandleType.Pinned)
                let res : 'b[] = Array.zeroCreate (int (bytes / nativeint sb))
                let gcDst = GCHandle.Alloc(res, GCHandleType.Pinned)

                try
                    Marshal.Copy(gc.AddrOfPinnedObject(), gcDst.AddrOfPinnedObject(), bytes)
                    res

                finally
                    gc.Free()
                    gcDst.Free()

            //static member CoercedApply (f : 'b[] -> 'r) (a : 'a[]) : 'r =
                
            //    let newLength = (a.Length * sa) / sb |> nativeint
            //    let gc = GCHandle.Alloc(a, GCHandleType.Pinned)
            //    try
            //        let ptr = gc.AddrOfPinnedObject() |> NativePtr.ofNativeInt<nativeint>

            //        let oldLength = NativePtr.get ptr -1
            //        let oldType = NativePtr.get ptr -2

            //        NativePtr.set ptr -1 newLength
            //        NativePtr.set ptr -2 idb

            //        try
            //            a |> unbox<'b[]> |> f
            //        finally
            //            NativePtr.set ptr -1 oldLength
            //            NativePtr.set ptr -2 oldType

            //    finally
            //        gc.Free()


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
                            let bytes = data |> ArrayCoerce<'a, byte>.Copy 
                            bin.Write bytes
                    }

                member x.WriteBool(v : bool) =
                    code { return bin.Write(v) }

                member x.WriteString (str : string) =
                    code { return bin.Write(str) }

            interface IReader with
                member x.ReadPrimitive() = failwith "[StreamWriter] cannot read"
                member x.ReadPrimitiveArray() = failwith "[StreamWriter] cannot read"
                member x.ReadBool() = failwith "[StreamWriter] cannot read"
                member x.ReadString() = failwith "[StreamWriter] cannot read"

            interface ICoder with
                member x.IsReading = false

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
                            return ArrayCoerce<byte, 'a>.Copy data
                    }

                member x.ReadBool() =
                    code { return bin.ReadBoolean() }

                member x.ReadString () =
                    code { return bin.ReadString() }

            interface IWriter with
                member x.WritePrimitive _ = failwith "[StreamReader] cannot write"
                member x.WritePrimitiveArray _ = failwith "[StreamReader] cannot write"
                member x.WriteBool _ = failwith "[StreamReader] cannot write"
                member x.WriteString _ = failwith "[StreamReader] cannot write"

            interface ICoder with
                member x.IsReading = true

            interface IDisposable with
                member x.Dispose() = bin.Dispose()

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

        let simpletest a =
            let res = roundtrip a
            res |> should equal a



    [<Version(1)>]
    type FieldCodedClass (a : int, b : float32) =

        member x.A = a
        member x.B = b

        override x.ToString() =
            sprintf "{ A = %A; B = %A }" a b

        override x.GetHashCode() = HashCode.Combine(a.GetHashCode(), b.GetHashCode())

        override x.Equals o =
            match o with
                | :? FieldCodedClass as o -> a = o.A && b = o.B
                | _ -> false

        member x.Coders(v : int) =
            coders {
                do! a <-> ValueCoder.coder

                if v < 1 then
                    do! b <-> (ValueCoder.coder<float> |>> float32)
                else
                    do! b <-> ValueCoder.coder
            }

    type ExtensionFieldCodedClass (a : int, b : float32) =
        let mutable a = a
        let mutable b = b
        member x.A
            with get() = a
            and private set v = a <- v

        member x.B
            with get() = b
            and private set v = b <- v

        override x.ToString() =
            sprintf "{ A = %A; B = %A }" a b

        override x.GetHashCode() = HashCode.Combine(a.GetHashCode(), b.GetHashCode())

        override x.Equals o =
            match o with
                | :? ExtensionFieldCodedClass as o -> a = o.A && b = o.B
                | _ -> false


    let mutable extensionInvoked = false

    [<Extension>]
    type FieldCodeableExtension private() =

//        [<Extension; Version(1)>]
//        static member Coders(x : ExtensionFieldCodedClass, v : int) =
//            extensionInvoked <- true
//            coders {
//                do! x.A <-> ValueCoder.coder
//
//                if v < 1 then
//                    do! x.B <-> (ValueCoder.coder<float> |>> float32)
//                else
//                    do! x.B <-> ValueCoder.coder
//            }

        [<Extension; Version(1)>]
        static member Coders(x : ExtensionFieldCodedClass, v : int) =
            extensionInvoked <- true
            let a = typeof<ExtensionFieldCodedClass>.GetProperty("A")
            let b = typeof<ExtensionFieldCodedClass>.GetProperty("B")
            [
                yield Property(a, ValueCoder.coder<int>)
                if v < 1 then
                    yield Skip(ValueCoder.coder<float>)
                    yield Property(b, ValueCoder.coder<float> |>> float32)
                else
                    yield Property(b, ValueCoder.coder<float32>)
                    
            ]

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

    type Enum32 =
        | A = 1
        | B = 2
        | C = 3

    type Enum8 =
        | A = 1uy
        | B = 2uy
        | C = 3uy


    [<Test>]
    let ``[Coder] int``() = simpletest 1

    [<Test>]
    let ``[Coder] decimal``() = simpletest (1.0m)

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] Symbol``() = simpletest (Symbol.Create "blabla")


    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] DateTime``() = simpletest DateTime.Now


    [<Test>]
    let ``[Coder] bool``() = simpletest true

    [<Test>]
    let ``[Coder] string``() = simpletest "hi there äü³²#"

    [<Test>]
    let ``[Coder] Type``() = simpletest typeof<int * obj * string>


    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
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

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
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

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] Record``() =
        let input = { RecA = 10; RecB = "sadsad" }
        let test = roundtrip input
        test |> should equal input

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] Union``() =
        simpletest (A 10)
        simpletest (B "asdsad")

    [<Test>]
    let ``[Coder] Enum32``() = simpletest Enum32.A

    [<Test>]
    let ``[Coder] Enum8``() = simpletest Enum8.A

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] Option<int>``() =
        simpletest (Some 10)
        simpletest Option<int>.None


    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] Option<obj>``() =
        simpletest (Some (10 :> obj))
        simpletest Option<obj>.None

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] int * float * string``() =
        simpletest (10, 10.012345, "hi there")

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] Choice<int, string>``() =
        let input : Choice<int, string> = Choice1Of2 1
        simpletest input
        let input : Choice<int, string> = Choice2Of2 "bla"
        simpletest input

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
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
    let ``[Coder] write int read obj``() =
        let arr = toArray 1
        let o : obj = ofArray arr
        o |> should equal 1

    [<Test>]
    let ``[Coder] write obj(int) read int``() =
        let arr = toArray (1 :> obj)
        let o : int = ofArray arr
        o |> should equal 1

    [<Test>]
    let ``[Coder] write obj(float) read int``() =
        let arr = toArray (1.0 :> obj)
        try
            let o : int = ofArray arr
            failwith "should not be able to read int when double was written"
        with _ ->
            ()

    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] FieldCodedClass``() =

        let v0Stub = 
            [|
                0x8Auy; 0x01uy; 0x41uy; 0x61uy; 0x72uy; 0x64uy; 0x76uy; 0x61uy; 0x72uy; 0x6Buy; 0x2Euy; 0x42uy; 0x61uy; 0x73uy; 0x65uy; 0x2Euy; 0x52uy; 0x75uy; 0x6Euy; 0x74uy; 0x69uy; 0x6Duy; 0x65uy; 0x2Euy; 0x54uy; 0x65uy; 0x73uy; 0x74uy; 0x73uy; 0x2Euy; 0x43uy; 0x6Fuy; 0x64uy; 0x65uy; 0x72uy; 0x54uy; 0x65uy; 0x73uy; 0x74uy; 0x73uy; 0x2Buy; 0x46uy; 0x69uy; 0x65uy; 0x6Cuy; 0x64uy; 0x43uy; 0x6Fuy; 0x64uy; 0x65uy; 0x64uy; 0x43uy; 0x6Cuy; 0x61uy; 0x73uy; 0x73uy; 0x2Cuy; 0x20uy; 0x41uy; 0x61uy; 0x72uy; 0x64uy; 0x76uy; 0x61uy; 0x72uy; 0x6Buy; 0x2Euy; 0x42uy; 0x61uy; 0x73uy; 0x65uy; 0x2Euy; 0x52uy; 0x75uy; 0x6Euy; 0x74uy; 0x69uy; 0x6Duy; 0x65uy; 0x2Euy; 0x54uy; 0x65uy; 0x73uy; 0x74uy; 0x73uy; 0x2Cuy; 0x20uy; 0x56uy; 0x65uy; 0x72uy; 0x73uy; 0x69uy; 0x6Fuy; 0x6Euy; 0x3Duy; 0x30uy; 0x2Euy; 0x30uy; 0x2Euy; 0x30uy; 0x2Euy; 0x30uy; 0x2Cuy; 0x20uy; 0x43uy; 0x75uy; 0x6Cuy; 0x74uy; 0x75uy; 0x72uy; 0x65uy; 0x3Duy; 0x6Euy; 0x65uy; 0x75uy; 0x74uy; 0x72uy; 0x61uy; 0x6Cuy; 0x2Cuy; 0x20uy; 0x50uy; 0x75uy; 0x62uy; 0x6Cuy; 0x69uy; 0x63uy; 0x4Buy; 0x65uy; 0x79uy; 0x54uy; 0x6Fuy; 0x6Buy; 0x65uy; 0x6Euy; 0x3Duy; 0x6Euy; 0x75uy; 0x6Cuy; 0x6Cuy; 0x4Cuy; 0xADuy; 0x59uy; 0x1Euy; 0x16uy; 0xE4uy; 0x5Auy; 0x47uy; 0xB7uy; 0xEAuy; 0xBBuy; 0x25uy; 0xBFuy; 0x8Auy; 0xC0uy; 0x44uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 
                // int : 0x01uy; 0x00uy; 0x00uy; 0x00uy; 
                // float : 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0xF0uy; 0x3Fuy
            |]

        let v0Data (a : int) (b : float) =
            Array.concat [
                v0Stub
                BitConverter.GetBytes(a)
                BitConverter.GetBytes(b)
            ]

        v0Data 1 1.0 |> ofArray<FieldCodedClass> |> should equal (FieldCodedClass(1, 1.0f))
        v0Data 2 5.0 |> ofArray<FieldCodedClass> |> should equal (FieldCodedClass(2, 5.0f))

        simpletest (FieldCodedClass(1, 1.0f))



    [<Test>][<Ignore("Is this coder fully implemented, or just a sketch?")>]
    let ``[Coder] ExtensionFieldCodedClass``() =
        simpletest (ExtensionFieldCodedClass(1, 1.0f))
        extensionInvoked |> should be True

