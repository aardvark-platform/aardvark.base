namespace Aardvark.Base.Runtime

open System
open System.Threading
open System.Threading.Tasks
open System.Runtime.InteropServices
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental


type NativeCalls = list<NativeCall>

type AdaptiveProgramStatistics =
    struct
        val mutable public DeltaProcessTime : TimeSpan
        val mutable public CompileTime : TimeSpan
        val mutable public WriteTime : TimeSpan
        
        val mutable public AddedFragmentCount : int
        val mutable public RemovedFragmentCount : int
        val mutable public CompiledFragmentCount : int
        val mutable public UpdatedFragmentCount : int
        val mutable public UpdatedJumpCount : int

        static member Zero =
            AdaptiveProgramStatistics(
                DeltaProcessTime = TimeSpan.Zero,
                CompileTime = TimeSpan.Zero,
                WriteTime = TimeSpan.Zero,
                AddedFragmentCount = 0,
                RemovedFragmentCount = 0,
                CompiledFragmentCount = 0,
                UpdatedFragmentCount = 0,
                UpdatedJumpCount = 0
            )

        static member (+) (l : AdaptiveProgramStatistics, r : AdaptiveProgramStatistics) =
            AdaptiveProgramStatistics(
                DeltaProcessTime = l.DeltaProcessTime + r.DeltaProcessTime,
                CompileTime = l.CompileTime + r.CompileTime,
                WriteTime = l.WriteTime + r.WriteTime,
                AddedFragmentCount = l.AddedFragmentCount + r.AddedFragmentCount,
                RemovedFragmentCount = l.RemovedFragmentCount + r.RemovedFragmentCount,
                CompiledFragmentCount = l.CompiledFragmentCount + r.CompiledFragmentCount,
                UpdatedFragmentCount = l.UpdatedFragmentCount + r.UpdatedFragmentCount,
                UpdatedJumpCount = l.UpdatedJumpCount + r.UpdatedJumpCount
            )

        static member (-) (l : AdaptiveProgramStatistics, r : AdaptiveProgramStatistics) =
            AdaptiveProgramStatistics(
                DeltaProcessTime = l.DeltaProcessTime - r.DeltaProcessTime,
                CompileTime = l.CompileTime - r.CompileTime,
                WriteTime = l.WriteTime - r.WriteTime,
                AddedFragmentCount = l.AddedFragmentCount - r.AddedFragmentCount,
                RemovedFragmentCount = l.RemovedFragmentCount - r.RemovedFragmentCount,
                CompiledFragmentCount = l.CompiledFragmentCount - r.CompiledFragmentCount,
                UpdatedFragmentCount = l.UpdatedFragmentCount - r.UpdatedFragmentCount,
                UpdatedJumpCount = l.UpdatedJumpCount - r.UpdatedJumpCount
            )


        override x.ToString() =
            String.concat "\r\n" [
                "DynamicProgramStatistics {"
                sprintf "   DeltaProcessTime = %A" x.DeltaProcessTime
                sprintf "   CompileTime = %A" x.CompileTime
                sprintf "   WriteTime = %A" x.WriteTime
                sprintf "   AddedFragmentCount = %A" x.AddedFragmentCount
                sprintf "   RemovedFragmentCount = %A" x.RemovedFragmentCount
                sprintf "   CompiledFragmentCount = %A" x.CompiledFragmentCount
                sprintf "   UpdatedFragmentCount = %A" x.UpdatedFragmentCount
                sprintf "   UpdatedJumpCount = %A" x.UpdatedJumpCount
                "}"
            ]

    end

type IAdaptiveProgram<'i> =
    inherit IAdaptiveObject
    inherit IDisposable
    
    abstract member Update : IAdaptiveObject -> AdaptiveProgramStatistics
    abstract member Run : 'i -> unit
    
    abstract member Disassemble : unit -> obj

    abstract member AutoDefragmentation : bool with get, set
    abstract member StartDefragmentation : unit -> Task<TimeSpan>

    abstract member NativeCallCount : int
    abstract member FragmentCount : int
    abstract member ProgramSizeInBytes : int64
    abstract member TotalJumpDistanceInBytes : int64

[<AllowNullLiteral>]
type IAdaptiveCode<'instruction> =
    inherit IDisposable
    abstract member Content : list<IMod<list<'instruction>>>

type AdaptiveCode<'instruction>(content : list<IMod<list<'instruction>>>) =
    interface IDisposable with
        member x.Dispose() = ()

    member x.Content = content
    interface IAdaptiveCode<'instruction> with
        member x.Content = content

[<AllowNullLiteral>]
type IFragment<'store> =
    abstract member Storage : 'store
    abstract member Next : IFragment<'store>
    abstract member JumpDistance : int with get, set


type FragmentHandler<'i, 'value, 'instruction, 'fragment> =
    {
        compileNeedsPrev : bool
        nativeCallCount : ref<int>
        jumpDistance : ref<int>
        run : 'i -> unit

        memorySize : unit -> int64

        compileDelta : Option<'value> -> 'value -> IAdaptiveCode<'instruction>
        prolog : 'fragment
        epilog : 'fragment
        startDefragmentation : obj -> ref<int> -> IFragment<'fragment> -> Task<TimeSpan>
        alloc : array<'instruction> -> 'fragment
        free : 'fragment -> unit
        write : 'fragment -> array<'instruction> -> bool
        writeNext : 'fragment -> 'fragment -> int
        isNext : 'fragment -> 'fragment -> bool
        dispose : unit -> unit
    }


module internal GenericProgram =

    [<AllowNullLiteral>]
    type Fragment<'i, 'a, 'instruction, 'fragment> =
        class
            inherit AdaptiveObject

            val mutable public Context : FragmentHandler<'i, 'a, 'instruction, 'fragment>
            val mutable public Storage : Option<'fragment>
            val mutable public Tag : Option<'a>
            val mutable public Prev : Fragment<'i, 'a, 'instruction, 'fragment>
            val mutable public Next : Fragment<'i, 'a, 'instruction, 'fragment>
            val mutable public Code : IAdaptiveCode<'instruction>
            val mutable public CodePrevTag : Option<'a>
            val mutable public CallCount : int
            val mutable public JumpDistance : int

            interface IFragment<'fragment> with
                member x.Next = x.Next :> IFragment<_>
                member x.Storage = x.Storage.Value

                member x.JumpDistance
                    with get() = x.JumpDistance
                    and set d = x.JumpDistance <- d

            member x.Recompile (caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let hasCode = not (isNull x.Code)

                    if x.Context.compileNeedsPrev then
                        let upToDate = hasCode && Object.Equals(x.CodePrevTag, x.Prev.Tag)

                        if not upToDate then
                            if hasCode then x.Code.Dispose()
                            x.Code <- x.Context.compileDelta x.Prev.Tag x.Tag.Value
                            x.CodePrevTag <- x.Prev.Tag
                            true
                        else
                            false
                    else
                        if not hasCode then
                            x.Code <- x.Context.compileDelta None x.Tag.Value
                            true
                        else
                            false

                )

            member x.WriteContent (caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    if not (isNull x.Code) then
                        let code =
                            x.Code.Content
                                |> List.collect (fun c -> c.GetValue x)
                                |> List.toArray


                        Interlocked.Add(x.Context.nativeCallCount, code.Length - x.CallCount) |> ignore
                        x.CallCount <- code.Length

                        if Option.isNone x.Storage then
                            x.Storage <- Some <| x.Context.alloc code
                            true
                        else
                            x.Context.write x.Storage.Value code
                    else
                        false
                )

            member x.LinkPrev(caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let prevFragment = x.Prev.Storage.Value
                    let myFragment = x.Storage.Value

                    if not (x.Context.isNext prevFragment myFragment) then
                        let distance = x.Context.writeNext prevFragment myFragment
                        Interlocked.Add(x.Context.jumpDistance, distance - x.JumpDistance) |> ignore
                        x.JumpDistance <- distance
                )

            member x.LinkNext(caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let nextFragment = x.Next.Storage.Value
                    let myFragment = x.Storage.Value

                    if not (x.Context.isNext myFragment nextFragment) then
                        let distance = x.Context.writeNext myFragment nextFragment
                        Interlocked.Add(x.Context.jumpDistance, distance - x.JumpDistance) |> ignore
                        x.JumpDistance <- distance
                )

            member x.Dispose() =
                x.Prev <- null
                x.Next <- null

                Interlocked.Add(x.Context.jumpDistance, -x.JumpDistance) |> ignore
                x.JumpDistance <- 0

                Interlocked.Add(x.Context.nativeCallCount, -x.CallCount) |> ignore
                x.CallCount <- 0

                if Option.isSome x.Storage then
                    x.Context.free x.Storage.Value
                    x.Storage <- None
            
                if not (isNull x.Code) then
                    x.Code.Dispose()
                    x.CodePrevTag <- None
                    x.Code <- null

            interface IDisposable with
                member x.Dispose() = x.Dispose()


            new(context, tag) = 
                { Context = context
                  Storage = None; Next = null; 
                  Prev = null; Tag = Some tag; 
                  Code = null; CodePrevTag = None
                  CallCount = 0; JumpDistance = 0 }

            new(context, storage) = 
                { Context = context
                  Storage = Some storage; Next = null; 
                  Prev = null; Tag = None; 
                  Code = null; CodePrevTag = None
                  CallCount = 0; JumpDistance = 0 }
        end

    type Program<'i, 'k, 'instruction, 'fragment, 'a>
        (input : aset<'k * 'a>,
         keyComparer : IComparer<'k>,
         newHandler : unit -> FragmentHandler<'i, 'a, 'instruction, 'fragment>) =
        inherit AdaptiveObject()

        let reader = input.GetReader()
        let version = ref -1

        let cache = Dict<'k * 'a, Fragment<'i, 'a, 'instruction, 'fragment>>()
        let fragments = SortedDictionaryExt<'k, StableSet<Fragment<'i, 'a, 'instruction, 'fragment>>>(keyComparer)

        let handler = newHandler()

        let prolog = new Fragment<_,_,_,_>(handler, handler.prolog)
        let epilog = new Fragment<_,_,_,_>(handler, handler.epilog)

        do prolog.Next <- epilog
           epilog.Prev <- prolog

        let deltaProcessWatch = System.Diagnostics.Stopwatch()
        let compileWatch = System.Diagnostics.Stopwatch()
        let writeWatch = System.Diagnostics.Stopwatch()

        let dirtyLock = obj()
        let mutable dirtySet = HashSet<Fragment<'i, 'a, 'instruction, 'fragment>>()

        let mutable autoDefragmentation = 1

        override x.InputChanged(o : IAdaptiveObject) =
            match o with
                | :? Fragment<'i, 'a, 'instruction, 'fragment> as o ->
                    lock dirtyLock (fun () -> dirtySet.Add o |> ignore)
                | _ ->
                    ()

        member x.Run (v : 'i) =
            lock x (fun () ->        
                handler.run v
            )

        member x.Disassemble() = null

        member x.Update caller = 
            x.EvaluateIfNeeded caller AdaptiveProgramStatistics.Zero (fun v ->
                Interlocked.Increment(version) |> ignore

                let deltas = reader.GetDelta x

                let dirtySet = 
                    lock dirtyLock (fun () ->
                        let set = dirtySet
                        dirtySet <- HashSet()
                        set
                    )

                let deadSet = HashSet()
                let recompileSet = HashSet()
                let relinkSet = HashSet()

                let mutable added = 0
                let mutable removed = 0
            
                let prologNext = 
                    prolog.Next.Storage

                let epilogPrev = 
                    epilog.Prev.Storage


                let createBetween (prev : Option<Fragment<'i, 'a, 'instruction, 'fragment>>) (v : 'a) (next : Option<Fragment<'i, 'a, 'instruction, 'fragment>>)  =

                    if handler.compileNeedsPrev then
                        match next with
                            | Some n -> recompileSet.Add n |> ignore
                            | _ -> ()
                    else
                        match next with
                            | Some n -> relinkSet.Add n |> ignore
                            | _ -> ()

                    //let code = desc.compileDelta (Option.map DynamicFragment.tag prev) v (Option.map DynamicFragment.tag next)
                    let fragment = new Fragment<'i, 'a, 'instruction, 'fragment>(handler, v)

                    let l = match prev with | Some l -> l | None -> prolog
                    let r = match next with | Some r -> r | None -> epilog

                    r.Prev <- fragment
                    fragment.Prev <- l
                    fragment.Next <- r
                    l.Next <- fragment

                    fragment

                deltaProcessWatch.Restart()
                for d in deltas do
                    match d with
                        | Add (k,v) ->
                            added <- added + 1

                            let l,s,r = SortedDictionary.neighbourhood k fragments

                            let next =
                                match r with
                                    | Some(_,r) -> r.First
                                    | None -> None

                            let prev =
                                match l with
                                    | Some(_,l) -> l.Last
                                    | None -> None

                            let fragment = 
                                match s with
                                    | Some self ->
                                        let created =
                                            self.AddWithPrev (fun p -> createBetween p v next) 

                                        match created with
                                            | Some f -> f
                                            | None -> failwithf "duplicated key: %A" k

                                    | None ->
                                        let frag = createBetween prev v next
                                        let set = StableSet()
                                        set.Add frag |> ignore
                                        fragments.[k] <- set
                                        frag

                            cache.[(k,v)] <- fragment
                            recompileSet.Add fragment |> ignore

                        | Rem (k,v) ->
                        
                            match fragments.TryGetValue k with
                                | (true, set) ->
                                    match cache.TryRemove ((k,v)) with
                                        | (true, fragment) ->

                                            if set.Remove fragment then
                                                removed <- removed + 1 
                                                fragment.Next.Prev <- fragment.Prev
                                                fragment.Prev.Next <- fragment.Next


                                                if handler.compileNeedsPrev && fragment.Next <> epilog then
                                                    recompileSet.Add fragment.Next |> ignore
                                                else
                                                    relinkSet.Add fragment.Next |> ignore

                                                fragment.Dispose()
                                                deadSet.Add fragment |> ignore

                                                if set.Count = 0 then
                                                    fragments.Remove k |> ignore

                                            ()
                                        | _ ->
                                            failwithf "could not find Fragment for: %A" k
                                | _ -> 
                                    failwithf "could not find container for: %A" k
                deltaProcessWatch.Stop()

                dirtySet.ExceptWith deadSet
                recompileSet.ExceptWith deadSet
                relinkSet.ExceptWith deadSet
                relinkSet.Remove prolog |> ignore
                relinkSet.Remove epilog |> ignore

                compileWatch.Restart()
                for r in recompileSet do
                    if r.Recompile x then
                        relinkSet.Add r |> ignore
                        dirtySet.Add r |> ignore

                compileWatch.Stop()



                writeWatch.Restart()
                for d in dirtySet do
                    if d.WriteContent x then
                        relinkSet.Add d |> ignore

                for d in relinkSet do
                    d.LinkPrev x

                prolog.LinkNext x
                epilog.LinkPrev x
                writeWatch.Stop()

                if autoDefragmentation = 1 && relinkSet.Count > 0 && !handler.jumpDistance > 0 then
                    handler.startDefragmentation (x :> obj) version |> ignore

                AdaptiveProgramStatistics (
                    DeltaProcessTime = deltaProcessWatch.Elapsed,
                    CompileTime = compileWatch.Elapsed,
                    WriteTime = writeWatch.Elapsed,

                    AddedFragmentCount = added,
                    RemovedFragmentCount = removed,
                    CompiledFragmentCount = recompileSet.Count,
                    UpdatedFragmentCount = dirtySet.Count,
                    UpdatedJumpCount = relinkSet.Count
                )
            ) 

        member x.Dispose() =
            for f in fragments do
                for f in f.Value do
                    f.Dispose()

            fragments.Clear()
            dirtySet.Clear()
            reader.Dispose()
            handler.dispose()
            cache.Clear()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IAdaptiveProgram<'i> with
            member x.AutoDefragmentation
                with get() = autoDefragmentation = 1
                and set d =
                    autoDefragmentation <- if d then 1 else 0
                    if d && !handler.jumpDistance > 0 then
                        handler.startDefragmentation (x :> obj) version |> ignore

            member x.StartDefragmentation() = 
                if !handler.jumpDistance > 0 then handler.startDefragmentation (x :> obj) version (prolog :> IFragment<_>)
                else Task.FromResult TimeSpan.Zero

            member x.Update(caller) = x.Update(caller)
            member x.Run v = x.Run v

            member x.FragmentCount = cache.Count
            member x.NativeCallCount = !handler.nativeCallCount
            member x.ProgramSizeInBytes = handler.memorySize() //int64 (memory.AllocatedBytes - prolog.Storage.Memory.Size - epilog.Storage.Memory.Size)
            member x.TotalJumpDistanceInBytes = int64 (!handler.jumpDistance - prolog.JumpDistance - epilog.JumpDistance)
            member x.Disassemble() = x.Disassemble() :> obj

module FragmentHandler =
    

    module private Defragmentation =

        let rec private evacuateKernel (startVersion : int) (version : byref<int>) (mem : MemoryManager) (results : List<IFragment<CodeFragment> * managedptr>) (last : CodeFragment) (current : IFragment<CodeFragment>) =
            if isNull current then 
                true
            else
                let currentMem = current.Storage.Memory
                let ptr = mem.Alloc(currentMem.Size)
                currentMem.CopyTo(ptr)

                let frag = CodeFragment(ptr, current.Storage.ContainsJmp)

                if not (isNull last) then
                    last.WriteNextPointer(ptr.Offset) |> ignore

                results.Add(current, ptr)

                if version <> startVersion then false
                else evacuateKernel startVersion &version mem results frag current.Next

        let evacuate (lockObj : obj) (version : byref<int>) (jumpDistance : ref<int>) (memory : byref<MemoryManager>) (prolog : IFragment<_>) =
            let startVersion = version
            let results = List()
            let mem = new MemoryManager(memory.Capacity, ExecutableMemory.alloc, ExecutableMemory.free)

            let worked =
                try evacuateKernel startVersion &version mem results null prolog
                with _ -> false

            if worked then
                Monitor.Enter lockObj
                try
                    if version = startVersion then
                        for (frag,code) in results do
                            frag.Storage.Memory <- code
                            jumpDistance := !jumpDistance - frag.JumpDistance
                            frag.JumpDistance <- 0

                        let old = memory
                        memory <- mem
                        old.Dispose()

                        true
                    else
                        mem.Dispose()
                        false
                finally
                    Monitor.Exit lockObj
            else
                mem.Dispose()
                false



    let nativeDifferential (maxArgs : int) (compileDelta : Option<'value> -> 'value -> IAdaptiveCode<NativeCall>) () : FragmentHandler<'i, 'value, NativeCall, CodeFragment>=
        
        let dynamicArgs =
            if typeof<'i> = typeof<unit> then 0
            else 1

        let memory = ref <| MemoryManager.createExecutable ()
        
        let prolog = CodeFragment(!memory, ASM.functionProlog dynamicArgs maxArgs)
        let epilog = CodeFragment(!memory, ASM.functionEpilog dynamicArgs maxArgs)


        let jumpDistance = ref 0
        let nativeCallCount = ref 0
        let defragmentWatch = System.Diagnostics.Stopwatch()
        let mutable currentDefragTask = Task.FromResult TimeSpan.Zero

        let mutable defragRunning = 0
        let startDefragmentation (parent : obj) (version : ref<int>) (first : IFragment<_>) =
            let startTask (f : unit -> 'x) =
                Task.Factory.StartNew(f, TaskCreationOptions.LongRunning)

            let tryDefragment () =
                Defragmentation.evacuate parent &version.contents jumpDistance &memory.contents first

            let start = Interlocked.Exchange(&defragRunning, 1)
            if start = 0 then
                let task = 
                    startTask (fun () ->
                        Log.startTimed "defragmentation"
                        defragmentWatch.Restart()
                        while not (tryDefragment()) do
                            Log.line "retry"
                        defragmentWatch.Stop()
                        Log.stop()

                        Interlocked.Exchange(&defragRunning, 0) |> ignore
                        defragmentWatch.Elapsed
                    )
                currentDefragTask <- task
                task
            else
                currentDefragTask

        let mutable currentPtr = 0n
        let mutable wrapped = ignore
        let run arg =
            ReaderWriterLock.read memory.Value.PointerLock (fun () ->
                let entry = memory.Value.Pointer + prolog.Offset
                if entry <> currentPtr then
                    currentPtr <- entry
                    wrapped <- UnmanagedFunctions.wrap entry

                wrapped arg

            )

        {
            compileNeedsPrev = true
            nativeCallCount = nativeCallCount
            jumpDistance = jumpDistance
            prolog = prolog
            epilog = epilog

            compileDelta = compileDelta
            startDefragmentation = startDefragmentation
            run = run

            memorySize = fun () -> 
                memory.Value.AllocatedBytes |> int64

            alloc = fun code -> 
                let code = ASM.assembleCalls dynamicArgs code
                CodeFragment(!memory, code)

            free = fun frag -> 
                frag.Dispose()

            write = fun frag code -> 
                let code = ASM.assembleCalls dynamicArgs code
                let ptr = frag.Offset
                frag.Write code
                ptr <> frag.Offset

            writeNext = fun prev next -> 
                prev.WriteNextPointer next.Offset

            isNext = fun prev frag ->
                prev.ReadNextPointer() = frag.Offset

            dispose = fun () ->
                memory.Value.Dispose()
        }

    let nativeSimple (maxArgs : int) (compile : 'value -> IAdaptiveCode<NativeCall>) ()  =
        let desc = nativeDifferential maxArgs (fun _ v -> compile v) ()
        { desc with compileNeedsPrev = false }

    let native (maxArgs : int) () =
        nativeDifferential maxArgs (fun _ _ -> failwith "no compileDelta given") ()

    let warpDifferential (mapping : 'a -> 'b) (compile : Option<'v> -> 'v -> IAdaptiveCode<'a>) (newInner : unit -> FragmentHandler<'i, 'v, 'b, 'frag>) =
        fun () ->
            let inner = newInner()
            {
                compileNeedsPrev = true
                nativeCallCount = inner.nativeCallCount
                jumpDistance = inner.jumpDistance
                prolog = inner.prolog
                epilog = inner.epilog
                compileDelta = compile
                startDefragmentation = inner.startDefragmentation
                run = inner.run
                memorySize = inner.memorySize
                alloc = fun code -> inner.alloc(code |> Array.map mapping)
                free = ignore
                write = fun frag code -> inner.write frag (code |> Array.map mapping)
                writeNext = fun prev next -> inner.writeNext prev next
                isNext = fun prev frag -> inner.isNext prev frag
                dispose = inner.dispose
            }
    
    let wrapSimple (mapping : 'a -> 'b) (compile : 'v -> IAdaptiveCode<'a>) (newInner : unit -> FragmentHandler<'i, 'v, 'b, 'frag>) =
        fun () ->
            let inner = newInner()
            {
                compileNeedsPrev = false
                nativeCallCount = inner.nativeCallCount
                jumpDistance = inner.jumpDistance
                prolog = inner.prolog
                epilog = inner.epilog
                compileDelta = fun _ v -> compile v
                startDefragmentation = inner.startDefragmentation
                run = inner.run
                memorySize = inner.memorySize
                alloc = fun code -> inner.alloc(code |> Array.map mapping)
                free = ignore
                write = fun frag code -> inner.write frag (code |> Array.map mapping)
                writeNext = fun prev next -> inner.writeNext prev next
                isNext = fun prev frag -> inner.isNext prev frag
                dispose = inner.dispose
            }


    [<AllowNullLiteral>]
    type ManagedFragment<'a>(values : array<'a>) =
        let mutable next = null
        let mutable values = values

        member x.Values
            with get() = values
            and set v = values <- v

        member x.Next
            with get() : ManagedFragment<'a> = next
            and set (n : ManagedFragment<'a>) = next <- n

    let test (translate : float32 -> NativeCall) (compile : int -> float32) =
        native 6 |> wrapSimple translate (fun l -> new AdaptiveCode<_>([Mod.constant [compile l]]) :> IAdaptiveCode<_>)

    let managedDifferential (compileDelta : Option<'v> -> 'v -> IAdaptiveCode<'i -> unit>) () : FragmentHandler<'i, 'v, 'i -> unit, ManagedFragment<'i -> unit>> =
        let prolog = ManagedFragment [||]
        let epilog = ManagedFragment [||]

        let rec run (l : ManagedFragment<'i -> unit>) (v : 'i) =
            for f in l.Values do f v
            if not (isNull l.Next) then
                run l.Next v

        {
            compileNeedsPrev = true
            nativeCallCount = ref 0
            jumpDistance = ref 0
            prolog = prolog
            epilog = epilog
            compileDelta = compileDelta
            startDefragmentation = fun _ _ _ -> Task.FromResult TimeSpan.Zero
            run = run prolog
            memorySize = fun () -> 0L
            alloc = fun code -> ManagedFragment<'i -> unit>(code)
            free = ignore
            write = fun frag code -> frag.Values <- code; false
            writeNext = fun prev next -> prev.Next <- next; 0
            isNext = fun prev frag -> prev.Next = frag
            dispose = fun () -> ()
        }

    let managedSimple (compile : 'v -> IAdaptiveCode<'i -> unit>) () =
        { managedDifferential (fun _ v -> compile v) () with compileNeedsPrev = false }

module AdaptiveProgram =
    
    let nativeDifferential (maxArgs : int) (comparer : IComparer<'k>) (compileDelta : Option<'v> -> 'v -> IAdaptiveCode<NativeCall>) (input : aset<'k * 'v>) =
        new GenericProgram.Program<_,_,_,_,_>(input, comparer, FragmentHandler.nativeDifferential maxArgs compileDelta) :> IAdaptiveProgram<'i>

    let nativeSimple (maxArgs : int) (comparer : IComparer<'k>) (compile : 'v -> IAdaptiveCode<NativeCall>) (input : aset<'k * 'v>) =
        new GenericProgram.Program<_,_,_,_,_>(input, comparer, FragmentHandler.nativeSimple maxArgs compile) :> IAdaptiveProgram<'i>

    let managedDifferential (comparer : IComparer<'k>) (compileDelta : Option<'v> -> 'v -> IAdaptiveCode<_>) (input : aset<'k * 'v>) =
        new GenericProgram.Program<_,_,_,_,_>(input, comparer, FragmentHandler.managedDifferential compileDelta) :> IAdaptiveProgram<'i>

    let managedSimple (comparer : IComparer<'k>) (compile : 'v -> IAdaptiveCode<_>) (input : aset<'k * 'v>) =
        new GenericProgram.Program<_,_,_,_,_>(input, comparer, FragmentHandler.managedSimple compile) :> IAdaptiveProgram<'i>

    let custom (comparer : IComparer<'k>) (createHandler : unit -> FragmentHandler<'i, 'value, 'instruction, 'fragment>) (input : aset<'k * 'value>) =
        new GenericProgram.Program<_,_,_,_,_>(input, comparer, createHandler) :> IAdaptiveProgram<'i>


    let inline run (p : IAdaptiveProgram<unit>) =
        p.Run()

    let inline update (p : IAdaptiveProgram<'a>) =
        p.Update(null)


    let inline nativeCallCount (p : IAdaptiveProgram<'i>) = p.NativeCallCount
    let inline fragmentCount (p : IAdaptiveProgram<'i>) = p.FragmentCount
    let inline programSizeInBytes (p : IAdaptiveProgram<'i>) = p.ProgramSizeInBytes
    let inline totalJumpDistanceInBytes (p : IAdaptiveProgram<'i>) = p.TotalJumpDistanceInBytes


module internal Tests =
    
    let calls = List<int * int>()

    let read() =
        let arr = calls |> Seq.toArray
        calls.Clear()
        arr

    let printFunction (last : int) (v : int) =
        calls.Add(last, v)

    type Print = delegate of int * int -> unit
    let dPrint = Print printFunction
    let pPrint = Marshal.GetFunctionPointerForDelegate dPrint

    let run() =
        let calls  = 
            CSet.ofList [
                0, Mod.constant 0
                3, Mod.constant 3
                2, Mod.constant 6
                1, Mod.constant 9
            ]


        let compile =
            let compileDelta prev self =
                let prev =
                    match prev with
                        | Some p -> p
                        | None -> Mod.constant 0

                let call = 
                    if prev = self then 
                        self |> Mod.map (fun s -> [pPrint, [|s :> obj; s :> obj|]])
                    else 
                        Mod.map2 (fun s p -> [pPrint, [|p :> obj; s :> obj|]]) self prev

                new AdaptiveCode<_>([call]) :> IAdaptiveCode<_>

            AdaptiveProgram.nativeDifferential 1 Comparer.Default compileDelta

        let prog = calls |> compile
        prog.AutoDefragmentation <- false

        prog.Update(null) |> printfn "update: %A"
        prog.Run();
        read() |> printfn "%A"

        transact (fun () ->
            calls |> CSet.remove (0,Mod.constant 0) |> ignore
        )

        prog.Update(null) |> printfn "update: %A"
        prog.Run();
        read() |> printfn "%A"


        transact (fun () ->
            calls |> CSet.add (1,Mod.constant 10) |> ignore
        )
        prog.Update(null) |> printfn "update: %A"
        prog.Run();
        read() |> printfn "%A"


        transact (fun () ->
            calls |> CSet.unionWith (List.init 10000 (fun i -> (-1, Mod.constant i)))
        )


        prog.Update(null) |> printfn "update: %A"
        prog.Run(); 
        read() |> printfn "%A"


        printfn "fragments:         %A" prog.FragmentCount
        printfn "calls:             %A" prog.NativeCallCount
        printfn "jump distance:     %A" prog.TotalJumpDistanceInBytes
        printfn "program size:      %A" prog.ProgramSizeInBytes



        transact (fun () ->
            calls |> CSet.exceptWith (List.init 10000 (fun i -> (-1, Mod.constant i)))
        )


        prog.Update(null) |> printfn "update: %A"
        prog.Run(); 
        read() |> printfn "%A"

        printfn "fragments:         %A" prog.FragmentCount
        printfn "calls:             %A" prog.NativeCallCount
        printfn "jump distance:     %A" prog.TotalJumpDistanceInBytes
        printfn "program size:      %A" prog.ProgramSizeInBytes

        let last = Mod.init 1000
        transact (fun () ->
            calls |> CSet.unionWith [(10000, last :> IMod<_>); (10001, last :> IMod<_>)] |> ignore
        )

        prog.Update(null) |> printfn "%A"
        prog.Run(); 
        read() |> printfn "%A"

        prog.StartDefragmentation().Result.TotalMilliseconds |> printfn "defrag took: %A"

        transact (fun () ->
            Mod.change last 1001
        )

        prog.Update(null) |> printfn "%A"
        prog.Run();
        read() |> printfn "%A"

        prog.StartDefragmentation().Result.TotalMilliseconds |> printfn "defrag took: %A"
        prog.Run();
        read() |> printfn "%A"
        printfn "fragments:         %A" prog.FragmentCount
        printfn "calls:             %A" prog.NativeCallCount
        printfn "jump distance:     %A" prog.TotalJumpDistanceInBytes
        printfn "program size:      %A" prog.ProgramSizeInBytes
