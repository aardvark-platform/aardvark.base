namespace Aardvark.Base.Runtime.Tests


open System
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Runtime
open NUnit.Framework
open FsUnit


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

        member x.DefragmentationStarted =
            program.DefragmentationStarted

        member x.DefragmentationDone =
            program.DefragmentationDone

        member x.StartDefragmentation() =
            program.StartDefragmentation()

        member x.Update(caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                program.Update x
            )

        member x.Run v =
            x.EvaluateIfNeeded null () (fun () ->
                program.Update(x) |> ignore
            )
            program.Run v
            getCalls()

        member x.Dispose() =
            getCalls() |> ignore
            program.Dispose()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

    type TestStruct =
        struct
            val mutable public Handle : int64

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

                new AdaptiveCode([Mod.constant [pAppend, [|l :> obj; r :> obj|]]])

            let program =
                input |> AMap.ofASet |> AdaptiveProgram.differential 6 Comparer.Default compileDelta

            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCalls)

        let createMod (input : aset<'k * IMod<int>>) =
            let compileDelta (l : Option<IMod<int>>) (r : IMod<int>) =
                let l = match l with | Some l -> l | None -> Mod.constant 0
                let call = Mod.map2 (fun l r -> [pAppend, [|l :> obj; r :> obj|]]) l r
                new AdaptiveCode([call])

            let program =
                input |> AMap.ofASet |> AdaptiveProgram.differential 6 Comparer.Default compileDelta
            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCalls)

        let createSimple (input : aset<int>) =
            let compile (r : int) =
                new AdaptiveCode([Mod.constant [pAppend, [|r :> obj; r :> obj|]]])

            let program =
                input 
                    |> ASet.map (fun i -> i,i) 
                    |> AMap.ofASet 
                    |> AdaptiveProgram.simple 6 Comparer.Default compile

            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCallsSelf)

        let createSimpleFloat (input : aset<float32>) =
            let compileDelta (l : Option<float32>) (r : float32) =
                let l = match l with | Some l -> l | None -> 0.0f

                new AdaptiveCode([Mod.constant [pAppendF, [|l :> obj; r :> obj|]]])

            let program =
                input |> ASet.map (fun i -> i,i) |> AMap.ofASet |> AdaptiveProgram.differential 6 Comparer.Default compileDelta

            program.AutoDefragmentation <- false

            new TestProgram<_,_>(program, getCallsSelfF)



        let createDynamic (input : aset<int>) =
            let compile (r : int) =
                new AdaptiveCode([Mod.constant [pAppend, [|r :> obj|]]])

            let program =
                input |> ASet.map (fun i -> i,i) |> AMap.ofASet |> AdaptiveProgram.simple 6 Comparer.Default compile

            program.AutoDefragmentation <- false

            new TestProgram<TestStruct,_>(program, getCalls)




    [<Test>]
    let ``[DynamicCode] add/remove/clear``() =

        let calls = CSet.ofList [1,1; 2,2]

        use prog = TestProgram.create calls

        // test initial execution
        prog.Update(null) |> ignore
        prog.NativeCallCount |> should equal 2
        prog.FragmentCount |> should equal 2

        prog.Run() |> should equal [0,1; 1,2]


        // test addition at end
        transact (fun () ->
            CSet.add (3,3) calls |> ignore
        )

        prog.Update(null) |> ignore
        prog.NativeCallCount |> should equal 3
        prog.FragmentCount |> should equal 3

        prog.Run() |> should equal [0,1; 1,2; 2,3]



        // test removal at end
        transact (fun () ->
            CSet.exceptWith [(2,2); (3,3)] calls |> ignore
        )
        prog.Update(null) |> ignore
        prog.NativeCallCount |> should equal 1 
        prog.FragmentCount |> should equal 1

        prog.Run() |> should equal [0,1]



        // test duplicate key stability
        transact (fun () ->
            CSet.add (1,2) calls |> ignore
        )
        prog.Update(null) |> ignore
        prog.NativeCallCount |> should equal 2 
        prog.FragmentCount |> should equal 2

        prog.Run() |> should equal [0,1; 1,2]


        // test removal at front
        transact (fun () ->
            CSet.remove (1,1) calls |> ignore
        )
        prog.Update(null) |> ignore
        prog.NativeCallCount |> should equal 1
        prog.FragmentCount |> should equal 1

        prog.Run() |> should equal [0,2]

        // test addition at front
        transact (fun () ->
            CSet.add (0,1) calls |> ignore
        )
        prog.Update(null) |> ignore
        prog.NativeCallCount |> should equal 2
        prog.FragmentCount |> should equal 2

        prog.Run() |> should equal [0,1;1,2]

        // test clear
        transact (fun () ->
            CSet.clear calls
        )
        prog.Update(null) |> ignore
        prog.NativeCallCount |> should equal 0
        prog.FragmentCount |> should equal 0

        prog.Run() |> should equal []
        ()

    [<Test>]
    let ``[DynamicCode] changes``() =

        let v1 = Mod.init 1
        let v2 = Mod.init 2
        let v3 = Mod.init 3

        let input = CSet.ofList [1, v1 :> IMod<_>; 2, v2 :> IMod<_>; 3, v3 :> IMod<_>]
        use prog = TestProgram.createMod input


        prog.Run() |> should equal [0,1; 1,2; 2,3]

        transact (fun () -> Mod.change v1 4)
        prog.Run() |> should equal [0,4; 4,2; 2,3]


        transact (fun () -> Mod.change v2 3)
        prog.Run() |> should equal [0,4; 4,3; 3,3]

        transact (fun () -> Mod.change v3 2)
        prog.Run() |> should equal [0,4; 4,3; 3,2]


        transact (fun () -> input |> CSet.remove (1,v1 :> IMod<_>)) |> should be True
        prog.Run() |> should equal [0,3; 3,2] 

        prog.StartDefragmentation().Wait()
        prog.Disassemble() |> Array.mapi (fun i a -> sprintf "%d: %A" i a) |> String.concat "\r\n" |> Console.WriteLine
        

    [<Test>]
    let ``[DynamicCode] defragmentation``() =
        
        let calls = [1..1000] |> CSet.ofList
        use prog = TestProgram.createSimple calls

        // create some fragmentation
        prog.Update(null) |> ignore
        transact (fun () ->
            calls |> CSet.exceptWith [100..200] 
        )
        prog.Update(null) |> ignore
        prog.TotalJumpDistanceInBytes |> should not' (equal 0L)


        //defragment and check
        prog.StartDefragmentation().Wait()
        prog.TotalJumpDistanceInBytes |> should equal 0L
        prog.Run() |> should equal ([1..99] @ [201..1000])


        // re-add the removed calls and check defragment/run
        transact (fun () ->
            calls |> CSet.unionWith [100..200] 
        )
        prog.Update(null) |> ignore
        prog.TotalJumpDistanceInBytes |> should not' (equal 0L)
        prog.StartDefragmentation().Wait()
        prog.TotalJumpDistanceInBytes |> should equal 0L
        prog.Run() |> should equal ([1..1000])


        ()


    [<Test>]
    let ``[DynamicCode] dynamic arguments``() =
        
        let calls = [1] |> CSet.ofList
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
        let calls = [1.0f; 2.0f] |> CSet.ofList
        use prog = TestProgram.createSimpleFloat calls

        prog.Update(null) |> ignore
        let res = prog.Run()
        res |> should equal [1.0f; 2.0f]


    
