namespace Aardvark.Base.Runtime.Tests


open System
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Runtime
open NUnit.Framework
open FsUnit
open FSharp.Data.Adaptive


module DynamicCodeTests =
    
    type TestProgram<'i, 'a>(program : IAdaptiveProgram<'i>, getCalls : unit -> list<'a>) =
        inherit AdaptiveObject()

        member x.NativeCallCount = program.NativeCallCount
        member x.FragmentCount = program.FragmentCount
        member x.TotalJumpDistanceInBytes = program.TotalJumpDistanceInBytes
        member x.ProgramSizeInBytes = program.ProgramSizeInBytes

        member x.Disassemble() = program.Disassemble() |> unbox<AMD64.Instruction[]>

        member x.AutoDefragmentation
            with get() = program.AutoDefragmentation
            and set d = program.AutoDefragmentation <- d


        member x.StartDefragmentation() =
            program.StartDefragmentation()

        member x.Update(token : AdaptiveToken) =
            x.EvaluateAlways token (fun token ->
                program.Update token
            )

        member x.Run i =
            let token = AdaptiveToken.Top
            x.EvaluateAlways token (fun token ->
                program.Update(token) |> ignore
            )
            program.Run i
            getCalls()

        member x.Dispose() =
            getCalls() |> ignore
            program.Dispose()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

    type TestStruct =
        struct
            val mutable public Handle : int64

            override x.ToString() = sprintf "S%d" x.Handle

            new(h : int) = { Handle = int64 h }
        end

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module TestProgram =
        open System.Runtime.InteropServices
        open System.Linq

        type private AppendDelegate = delegate of int * int -> unit
        type private AppendFDelegate = delegate of float32 * float32 -> unit
        let private calls = new ThreadLocal<List<int * int>>(fun () -> List<int * int>())
        let private callsF = new ThreadLocal<List<float32 * float32>>(fun () -> List<float32 * float32>())

        let private append (arg0 : int) (arg1 : int) =
            calls.Value.Add(arg0, arg1)

        let private appendF (arg0 : float32) (arg1 : float32) =
            callsF.Value.Add(arg0, arg1)

        let private dAppend = AppendDelegate append
        let private pAppend = Marshal.GetFunctionPointerForDelegate dAppend

        let private dAppendF = AppendFDelegate appendF
        let private pAppendF = Marshal.GetFunctionPointerForDelegate dAppendF
        
        let private getCalls() =
            let arr = calls.Value.ToArray()
            calls.Value.Clear()
            arr |> Array.toList

        let private getCallsF() =
            let arr = callsF.Value.ToArray()
            callsF.Value.Clear()
            arr |> Array.toList

        let private getCallsSelf() =
            let arr = calls.Value.ToArray()
            calls.Value.Clear()
            arr |> Array.toList |> List.map snd

        let private getCallsSelfF() =
            let arr = callsF.Value.ToArray()
            callsF.Value.Clear()
            arr |> Array.toList |> List.map snd

        let create (input : aset<'k * int>) =
            let compileDelta (l : Option<int>) (r : int) =
                let l = match l with | Some l -> l | None -> 0

                new AdaptiveCode<_>([AVal.constant [pAppend, [|l :> obj; r :> obj|]]]) :> IAdaptiveCode<_>

            let program =
                input |> AdaptiveProgram.nativeDifferential 6 Comparer.Default compileDelta

            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCalls)

        let createMod (input : aset<'k * aval<int>>) =
            let compileDelta (l : Option<aval<int>>) (r : aval<int>) =
                let l = match l with | Some l -> l | None -> AVal.constant 0
                let call = AVal.map2 (fun l r -> [pAppend, [|l :> obj; r :> obj|]]) l r
                new AdaptiveCode<_>([call]) :> IAdaptiveCode<_>

            let program =
                input |> AdaptiveProgram.nativeDifferential 6 Comparer.Default compileDelta
            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCalls)

        let createSimple (input : aset<int>) =
            let compile (r : int) =
                new AdaptiveCode<_>([AVal.constant [pAppend, [|r :> obj; r :> obj|]]]) :> IAdaptiveCode<_>

            let program =
                input 
                    |> ASet.map (fun i -> i,i) 
                    |> AdaptiveProgram.nativeSimple 6 Comparer.Default compile

            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCallsSelf)

        let createSimpleFloat (input : aset<float32>) =
            let compileDelta (l : Option<float32>) (r : float32) =
                let l = match l with | Some l -> l | None -> 0.0f

                new AdaptiveCode<_>([AVal.constant [pAppendF, [|l :> obj; r :> obj|]]]) :> IAdaptiveCode<_>

            let program =
                input |> ASet.map (fun i -> i,i) |> AdaptiveProgram.nativeDifferential 6 Comparer.Default compileDelta

            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCallsSelfF)



        let createDynamic (input : aset<int>) =
            let compile (r : int) =
                new AdaptiveCode<_>([AVal.constant [pAppend, [|r :> obj|]]]) :> IAdaptiveCode<_>

            let program =
                input |> ASet.map (fun i -> i,i) |> AdaptiveProgram.nativeSimple 6 Comparer.Default compile

            program.AutoDefragmentation <- false

            new TestProgram<TestStruct,_>(program, getCalls)

    let testF(a : int) (b : int) (c : int) (d : int) (e : int) =
        sprintf "(%A,%A,%A,%A,%A)" a b c d e |> Console.WriteLine

    type TestDel = delegate of int * int * int * int * int -> unit
    type SimpleDel = delegate of int -> unit
    let dTest = TestDel testF
    let pTest = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(dTest)

    let simpleOut = List<int>()
    let dSimple = SimpleDel (fun v -> printfn "%d" v; simpleOut.Add v)
    let pSimple = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(dSimple)



    [<Test>] 
    let ``[DynamicCode] imperative``() =
        use prog = new ChangeableNativeProgram<int, int>((fun i s -> s.BeginCall(1); s.PushArg(i); s.Call(pSimple); 0), 0, (fun a b -> a + b), (fun a b -> a - b))

        let run() =
            simpleOut.Clear()
            prog.Run()
            let res = simpleOut |> Seq.toList
            simpleOut.Clear()
            res

        run() |> should equal []

        prog.Add(1) |> ignore
        run() |> should equal [1]
        
        prog.Add(2) |> ignore
        run() |> should equal [1;2]
        
        prog.Remove(1) |> ignore
        run() |> should equal [2]

        prog.Remove(2) |> ignore
        run() |> should equal []

        prog.Clear()
        run() |> should equal []


        ()

    [<Test>]
    let ``[DynamicCode] lots of args``() =
        let input = cset [1,1]
        let prog = 
            AdaptiveProgram.nativeSimple 
                6 Comparer<int>.Default 
                (fun a -> new AdaptiveCode<_>([AVal.constant [pTest, [|2 :> obj; 3 :> obj; 4 :> obj; 5 :> obj|]]]) :> IAdaptiveCode<_>)
                input

        prog.Update(AdaptiveToken.Top) |> ignore
        prog.Run(1)

        ()

    [<Test>]
    let ``[DynamicCode] add/remove/clear``() =

        let calls = cset [1,1; 2,2]

        use prog = TestProgram.create calls

        // test initial execution
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.NativeCallCount |> should equal 2
        prog.FragmentCount |> should equal 2

        prog.Run() |> should equal [0,1; 1,2]


        // test addition at end
        transact (fun () ->
            calls.Add (3,3) |> ignore
        )

        prog.Update(AdaptiveToken.Top) |> ignore
        prog.NativeCallCount |> should equal 3
        prog.FragmentCount |> should equal 3

        prog.Run() |> should equal [0,1; 1,2; 2,3]



        // test removal at end
        transact (fun () ->
            calls.ExceptWith [(2,2); (3,3)]
        )
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.NativeCallCount |> should equal 1 
        prog.FragmentCount |> should equal 1

        prog.Run() |> should equal [0,1]



        // test duplicate key stability
        transact (fun () ->
            calls.Add (1,2) |> ignore
        )
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.NativeCallCount |> should equal 2 
        prog.FragmentCount |> should equal 2

        prog.Run() |> should equal [0,1; 1,2]


        // test removal at front
        transact (fun () ->
            calls.Remove (1,1) |> ignore
        )
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.NativeCallCount |> should equal 1
        prog.FragmentCount |> should equal 1

        prog.Run() |> should equal [0,2]

        // test addition at front
        transact (fun () ->
            calls.Add (0,1) |> ignore
        )
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.NativeCallCount |> should equal 2
        prog.FragmentCount |> should equal 2

        prog.Run() |> should equal [0,1;1,2]

        // test clear
        transact (fun () ->
            calls.Clear()
        )
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.NativeCallCount |> should equal 0
        prog.FragmentCount |> should equal 0

        prog.Run() |> should equal []
        ()

    [<Test>]
    let ``[DynamicCode] changes``() =

        let v1 = cval 1
        let v2 = cval 2
        let v3 = cval 3

        let input = cset [1, v1 :> aval<_>; 2, v2 :> aval<_>; 3, v3 :> aval<_>]
        use prog = TestProgram.createMod input


        prog.Run() |> should equal [0,1; 1,2; 2,3]

        transact (fun () -> v1.Value <- 4)
        prog.Run() |> should equal [0,4; 4,2; 2,3]


        transact (fun () -> v2.Value <- 3)
        prog.Run() |> should equal [0,4; 4,3; 3,3]

        transact (fun () -> v3.Value <- 2)
        prog.Run() |> should equal [0,4; 4,3; 3,2]


        transact (fun () -> input.Remove(1,v1 :> aval<_>)) |> should be True
        prog.Run() |> should equal [0,3; 3,2] 

        prog.StartDefragmentation().Wait()


    [<Test>]
    let ``[DynamicCode] defragmentation``() =
        
        let calls = cset [1..1000]
        use prog = TestProgram.createSimple calls

        // create some fragmentation
        prog.Update(AdaptiveToken.Top) |> ignore
        transact (fun () ->
            calls.ExceptWith [100..200] 
        )
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.TotalJumpDistanceInBytes |> should not' (equal 0L)


        //defragment and check
        prog.StartDefragmentation().Wait()
        prog.TotalJumpDistanceInBytes |> should equal 0L
        prog.Run() |> should equal ([1..99] @ [201..1000])


        // re-add the removed calls and check defragment/run
        transact (fun () ->
            calls.UnionWith [100..200] 
        )
        prog.Update(AdaptiveToken.Top) |> ignore
        prog.TotalJumpDistanceInBytes |> should not' (equal 0L)
        prog.StartDefragmentation().Wait()
        prog.TotalJumpDistanceInBytes |> should equal 0L
        prog.Run() |> should equal ([1..1000])


        ()


    [<Test>]
    let ``[DynamicCode] dynamic arguments``() =
        
        let calls = cset [1]
        use prog = TestProgram.createDynamic calls

        prog.Run (TestStruct 5) |> should equal [5,1]
        prog.Run (TestStruct 7) |> should equal [7,1]

        transact (fun () -> calls.Add 2 |> ignore)
        prog.Run (TestStruct 5) |> should equal [5,1; 5,2]


        transact (fun () -> calls.Add 3 |> ignore)
        prog.Run (TestStruct 20) |> should equal [20,1; 20,2; 20,3]


        transact (fun () -> calls.Clear())
        prog.Run (TestStruct 20) |> should equal []


        ()


    [<Test>]
    let ``[DynamicCode] float arguments``() =
        let calls = cset [1.0f; 2.0f]
        use prog = TestProgram.createSimpleFloat calls

        prog.Update(AdaptiveToken.Top) |> ignore
        let res = prog.Run()
        res |> should equal [1.0f; 2.0f]


    
    [<Test>]
    let ``[DynamicCode] huge changeset``() =

        let cnt = 250000
        let manyCalls =
            [
                for i in 1 .. cnt do yield i,i+1
            ]

        let calls = cset manyCalls

        use prog = TestProgram.create calls

        let sw = System.Diagnostics.Stopwatch()
        sw.Start()
        let stats = prog.Update(AdaptiveToken.Top)
        prog.NativeCallCount |> ignore
        prog.FragmentCount |> ignore
        sw.Stop()
        printfn "stats: %A" stats
        printfn "update took: %As" sw.Elapsed.TotalSeconds
        sw.Restart()

        prog.Run() |> ignore

        sw.Stop()
        printfn "run took: %As" sw.Elapsed.TotalSeconds


