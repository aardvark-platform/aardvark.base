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
        let additionalSize =
            if maxArgs < 5 then 8uy
            else 8 * maxArgs - 24 |> byte


        let prolog = Fragment(manager, [| 0x48uy; 0x83uy; 0xECuy; 0x20uy + additionalSize |])
        let epilog = Fragment(manager, [| 0x48uy; 0x83uy; 0xC4uy; 0x20uy + additionalSize; 0xC3uy|])
        
        let f = Fragment(manager)

        f.Write [|
            myfunPtr, [|1 :> obj; 2 :> obj; 3 :> obj; 4 :> obj; 5 :> obj|]
            myfunPtr, [|4 :> obj; 3 :> obj; 2 :> obj; 1 :> obj; 0 :> obj|]
        |]

            
        prolog.Next <- f
        f.Prev <- prolog

        epilog.Prev <- f
        f.Next <- epilog


        f.NextPointer |> should equal epilog.EntryPointer
        prolog.NextPointer |> should equal f.EntryPointer

        Console.WriteLine("Code:")
        let instructions = f.Instructions
        for i in instructions do
            Console.WriteLine("  {0}", sprintf "%A" i)

        let run : unit -> unit = UnmanagedFunctions.wrap (manager.Pointer + prolog.EntryPointer)

        run()

        calls |> Seq.toList 
              |> should equal [
                [|1 :> obj; 2 :> obj; 3 :> obj; 4 :> obj; 5 :> obj|]
                [|4 :> obj; 3 :> obj; 2 :> obj; 1 :> obj; 0 :> obj|]
              ]

        ()
