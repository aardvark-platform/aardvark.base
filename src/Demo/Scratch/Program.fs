// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.


open System 
open System.Threading
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Runtime
open Aardvark.Base.Incremental
open Aardvark.Base.Monads.State


open System.Drawing
open System.Windows.Forms
open System.IO
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"
#nowarn "51"

type NativeStream() =
    inherit Stream()

    let mutable capacity = 128n
    let mutable store = Marshal.AllocHGlobal(capacity)
    let mutable offset = 0n
    let mutable length = 0n

    member x.Pointer = store
        
    override x.Dispose(disposing : bool) =
        base.Dispose(disposing)
        let s = Interlocked.Exchange(&store, 0n)
        if s <> 0n then
            Marshal.FreeHGlobal s
            capacity <- 0n
            offset <- 0n
            length <- 0n

    override x.CanRead = true
    override x.CanSeek = true
    override x.CanWrite = true

    override x.Length = int64 length
    override x.Position
        with get() = int64 offset
        and set o = offset <- nativeint o

    override x.Flush() = ()

    override x.Seek(o, origin) =
        match origin with
            | SeekOrigin.Begin -> offset <- nativeint o
            | SeekOrigin.Current -> offset <- offset + nativeint o
            | SeekOrigin.End -> offset <- length - nativeint o
            | _ -> ()
        int64 offset

    override x.SetLength(l : int64) =
        if l > int64 capacity then
            let newCap = Fun.NextPowerOfTwo l |> nativeint
            store <- Marshal.ReAllocHGlobal(store, newCap)
            capacity <- newCap

        length <- nativeint l

    override x.Read(buffer : byte[], o : int, count : int) =
        let count = min (length - nativeint offset) (nativeint count) |> int
        Marshal.Copy(store + offset, buffer, o, count)
        count

    override x.Write(buffer : byte[], o : int, count : int) =
        let l = offset + nativeint count
        if l > capacity then
            let newCap = Fun.NextPowerOfTwo (int64 l) |> nativeint
            store <- Marshal.ReAllocHGlobal(store, newCap)
            capacity <- newCap

        Marshal.Copy(buffer, o, store + offset, count)
        length <- length + nativeint count
        offset <- offset + nativeint count


module Benchmark =
    open System.Diagnostics

    type MyDelegate = delegate of float32 * float32 * int64 * float32 * int * float32 * int * float32 -> unit

    let callback =
        MyDelegate (fun a b c d e f g h ->
            printfn "a: %A" a
            printfn "b: %A" b
            printfn "c: %A" c
            printfn "d: %A" d
            printfn "e: %A" e
            printfn "f: %A" f
            printfn "g: %A" g
            printfn "h: %A" h
        )

    let pDel = Marshal.PinDelegate(callback)
    let ptr = pDel.Pointer

    let cnt = 1 <<< 12

    let args = [| 1.0f :> obj; 2.0f :> obj; 3L :> obj; 4.0f :> obj; 5 :> obj; 6.0f :> obj; 7 :> obj; 8.0f :> obj |]
    let calls = Array.init cnt (fun _ -> ptr, args)

    let runOld(iter : int) =
        // warmup
        for i in 1 .. 10 do
            Aardvark.Base.Assembler.compileCalls 0 calls |> ignore

        let sw = Stopwatch.StartNew()
        for i in 1 .. iter do
            Aardvark.Base.Assembler.compileCalls 0 calls |> ignore
        sw.Stop()

        sw.MicroTime / (float iter)

    let cc = AMD64.CallingConvention.windows
    let fillNew(cnt) =
        use s = new MemoryStream()
        use w = new AMD64.AssemblerStream(s)

        w.Begin()

        for i in 1 .. cnt do
            w.BeginCall(8)
            w.PushArg(cc, 7.0f)
            w.PushArg(cc, 6u)
            w.PushArg(cc, 5.0f)
            w.PushArg(cc, 4u)
            w.PushArg(cc, 3.0f)
            w.PushArg(cc, 2UL)
            w.PushArg(cc, 1.0f)
            w.PushArg(cc, 0.0f)
            w.Call(cc, ptr)

        w.End()
        w.Ret()
        //s.ToArray()

    let runNew(iter : int) =
        // warmup
        for i in 1 .. 10 do
            fillNew(cnt) |> ignore

        let sw = Stopwatch.StartNew()
        for i in 1 .. iter do
            fillNew(cnt) |> ignore
        sw.Stop()

        sw.MicroTime / (float iter)

    let run() =
//        let size = 1 <<< 14
//        while true do
//            fillNew size

        let ot = runOld 100
        let throughput = float cnt / ot.TotalSeconds
        Log.line "old: %A (%.0fc/s)" ot throughput

        let nt = runNew 100
        let throughput = float cnt / nt.TotalSeconds
        Log.line "new: %A (%.0fc/s)" nt throughput

        let speedup = ot / nt
        Log.line "factor: %A" speedup

module Test =

    type MyDelegate = delegate of float32 * float32 * int * float32 * int * float32 * int * float32 -> unit

    let callback =
        MyDelegate (fun a b c d e f g h ->
            Log.start "call"
            Log.line "a: %A" a
            Log.line "b: %A" b
            Log.line "c: %A" c
            Log.line "d: %A" d
            Log.line "e: %A" e
            Log.line "f: %A" f
            Log.line "g: %A" g
            Log.line "h: %A" h
            Log.stop()
        )

    let ptr = Marshal.PinDelegate(callback)

    let run () =
        let store = NativePtr.alloc 1
        NativePtr.write store 100.0f

        use stream = new NativeStream()
        use asm = AssemblerStream.ofStream stream

        asm.BeginFunction()

        asm.BeginCall(8)
        asm.PushFloatArg(NativePtr.toNativeInt store)
        asm.PushArg(6)
        asm.PushArg(5.0f)
        asm.PushArg(4)
        asm.PushArg(3.0f)
        asm.PushArg(2)
        asm.PushArg(1.0f)
        asm.PushArg(0.0f)
        asm.Call(ptr.Pointer)
 
        asm.BeginCall(8)
        asm.PushArg(17.0f)
        asm.PushArg(16)
        asm.PushArg(15.0f)
        asm.PushArg(14)
        asm.PushArg(13.0f)
        asm.PushArg(12)
        asm.PushArg(11.0f)
        asm.PushArg(10.0f)
        asm.Call(ptr.Pointer)

        asm.WriteOutput(1234)
        asm.EndFunction()
        asm.Ret()
        
        let size = Fun.NextPowerOfTwo stream.Length |> nativeint
        let mem = ExecutableMemory.alloc size
        Marshal.Copy(stream.Pointer, mem, stream.Length)

        let managed : int -> int = UnmanagedFunctions.wrap mem
        Log.start "run(3)"

        if sizeof<nativeint> = 4 then Log.warn "32 bit"
        else Log.warn "64 bit"


        let res = managed 3
        Log.line "ret: %A" res
        Log.stop()

        ExecutableMemory.free mem size

        Environment.Exit 0


type MyDelegate = delegate of int * int * int * int64 * int64 -> unit // * int64 * int64 * int64 * int64 -> unit

open AMD64

[<EntryPoint; STAThread>]
let main argv = 

    let sw = System.Diagnostics.Stopwatch.StartNew()
    for iter in 1..50 do
        let cnt = iter * 1000
        let arr = ASet.ofArray(Array.init(cnt) (fun i -> Mod.init(i) :> IMod<_>))

        let arrr = arr |> ASet.collect (fun x -> 
                            x |> ASet.bind (fun y -> ASet.single y))

        let r = arrr.GetReader()

        let sw = System.Diagnostics.Stopwatch.StartNew()
        r.GetOperations AdaptiveToken.Top |> ignore
        sw.Stop()
        Log.line "%d took: %A" cnt sw.MicroTime
    Environment.Exit 0

//    Program.test()

    
    let m = Mod.init 10
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    let set = CSet.ofList [1 .. 100000]
    sw.Stop()
    Log.line "build: %A" sw.MicroTime

    let op = 
        m |> ASet.bind (fun a ->
            printfn "bind"
            set 
                |> ASet.collect (fun b -> if a <> b then ASet.ofList [a] else ASet.empty)
                |> fun a -> printfn "collect rebuild"; a
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
        )

    let r = op.GetReader()

    let sw = System.Diagnostics.Stopwatch.StartNew()
    r.GetOperations AdaptiveToken.Top |> ignore
    sw.Stop()
    Log.line "took: %A" sw.MicroTime

    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    transact (fun () -> m.Value <- 1)
    sw.Stop()
    Log.line "transact: %A" sw.MicroTime
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    r.GetOperations AdaptiveToken.Top |> ignore
    sw.Stop()
    Log.line "took: %A" sw.MicroTime
    
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    transact (fun () -> set.Add 0 |> ignore)
    sw.Stop()
    Log.line "transact: %A" sw.MicroTime
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    r.GetOperations AdaptiveToken.Top |> ignore
    sw.Stop()


    Log.line "took: %A" sw.MicroTime
    Environment.Exit 0








    let callback =
        MyDelegate (fun a b c d e ->
            printfn "a: %A" a
            printfn "b: %A" b
            printfn "c: %A" c
            printfn "d: %A" d
            printfn "e: %A" e
        )

    let ptr = Marshal.PinDelegate(callback)

    let store = NativePtr.alloc 1

    use stream = new NativeStream()
    let asm = new AssemblerStream(stream, true)

    let data = NativePtr.alloc 1
    NativePtr.write data 12321.0

    let cc = CallingConvention.windows

    let l = asm.NewLabel()

    asm.Begin()
    
    asm.Load(Register.Rax, NativePtr.toNativeInt store, false)
    asm.Cmp(Register.Rax, 0u)
    asm.Jump(JumpCondition.Equal, l)

    asm.BeginCall(5)
    asm.PushArg(cc, 1234UL)
    asm.PushArg(cc, 4321UL)
    asm.PushArg(cc, 3u)
    asm.PushArg(cc, 2u)
    asm.PushArg(cc, 1u)
    asm.Call(cc, ptr.Pointer)

    asm.Mark(l)

    asm.BeginCall(5)
    asm.PushArg(cc, 81234UL)
    asm.PushArg(cc, 74321UL)
    asm.PushArg(cc, 6u)
    asm.PushArg(cc, 5u)
    asm.PushArg(cc, 4u)
    asm.Call(cc, ptr.Pointer)

    asm.End()
    asm.Ret()
    asm.Dispose()

    let size = Fun.NextPowerOfTwo stream.Length |> nativeint
    let mem = ExecutableMemory.alloc size
    Marshal.Copy(stream.Pointer, mem, stream.Length)

    let managed : int  -> float32 = UnmanagedFunctions.wrap mem
    
    NativePtr.write store 0
    let res = managed 1234
    printfn "res = %A" res
    ExecutableMemory.free mem size

    Environment.Exit 0

    let rand = RandomSystem()
    let g = UndirectedGraph.ofNodes (Set.ofList [0..127]) (fun li ri -> float (ri - li) |> Some)

    let tree = UndirectedGraph.maximumSpanningTree compare g
    printfn "%A" tree

    printfn "%A" (Tree.weight tree / float (Tree.count tree))



    //React.Test.run()
    Environment.Exit 0

    0 // return an integer exit code
