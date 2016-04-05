namespace Aardvark.Base.FSharp.Tests
#nowarn "44"

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework
open System.Runtime.InteropServices
open System.Diagnostics
open System.Collections.Generic

module FragmentTests =

    type MyDelegate = delegate of int * int * int * int * int -> unit

    let calls = List<obj[]>()

    let func(a : int) (b : int) (c : int) (d : int) (e : int) =
        calls.Add([|a :> obj; b; c; d; e|])
        sprintf "{ a = %d; b = %d; c = %d; d = %d; e = %d }" a b c d e |> Console.WriteLine

    let myfun = MyDelegate func
    let myfunPtr = Marshal.GetFunctionPointerForDelegate(myfun)

    [<Test>]
    let ``[Fragment] execution working``() =
        let manager = MemoryManager.createExecutable()

        let maxArgs = 6
        let prolog = manager |> CodeFragment.prolog maxArgs 
        let epilog = manager |> CodeFragment.epilog maxArgs 

        let frag = 
            manager |> CodeFragment.ofCalls [
                myfunPtr, [|1 :> obj; 2 :> obj; 3 :> obj; 4 :> obj; 5 :> obj|]
                myfunPtr, [|4 :> obj; 3 :> obj; 2 :> obj; 1 :> obj; 0 :> obj|]
            ]

            
        prolog.WriteNextPointer frag.Offset |> ignore
        frag.WriteNextPointer epilog.Offset |> ignore


        frag.ReadNextPointer() |> should equal epilog.Offset
        prolog.ReadNextPointer() |> should equal frag.Offset

        Console.WriteLine("Code:")
        let instructions = frag.Calls
        for (ptr, args) in instructions do
            Console.WriteLine("  {0}({1})", sprintf "%A" ptr, sprintf "%A" args)

 
        let run = CodeFragment.wrap prolog
        run()

        calls |> Seq.toList 
              |> should equal [
                [|1 :> obj; 2 :> obj; 3 :> obj; 4 :> obj; 5 :> obj|]
                [|4 :> obj; 3 :> obj; 2 :> obj; 1 :> obj; 0 :> obj|]
              ]

        ()


    type PrintDelegate = delegate of int * int64 -> unit
    let printer (i : int) (l : int64) =
        printfn "i=%d l=%d" i l
        calls.Add([|i :> obj; l :> obj|])

    let pdel = PrintDelegate printer
    let pptr = Marshal.GetFunctionPointerForDelegate(pdel)

    [<Test>]
    let ``[Fragment] pointer arguments working``() =
        
        let l0 = Marshal.AllocHGlobal sizeof<int>
        let l1 = Marshal.AllocHGlobal sizeof<int64>

        let code =
            Array.concat [
                ASM.functionProlog 0 6
                ASM.assembleCalls 0 [pptr, [|Ptr32 l0 :> obj; Ptr64 l1 :> obj|]]
                ASM.functionEpilog 0 6
            ]

        let ptr = ExecutableMemory.alloc code.Length
        Marshal.Copy(code, 0, ptr, code.Length)
        let f : unit -> unit = UnmanagedFunctions.wrap ptr

        let run() = 
            f()
            let arr = calls |> CSharpList.toArray
            calls.Clear()
            arr.[0]


        calls.Clear()
        Marshal.WriteInt32(l0, 1)
        Marshal.WriteInt64(l1, 10L)
        run() |> should equal [|1 :> obj; 10L :> obj|]


        Marshal.WriteInt32(l0, 5)
        Marshal.WriteInt64(l1, 6L)
        run() |> should equal [|5 :> obj; 6L :> obj|]
