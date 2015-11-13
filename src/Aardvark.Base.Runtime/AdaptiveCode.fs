namespace Aardvark.Base.Runtime

open System
open System.Threading
open System.Threading.Tasks
open System.Runtime.InteropServices
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental


type NativeCalls = list<NativeCall>

[<AllowNullLiteral>]
type AdaptiveCode(content : list<IMod<NativeCalls>>) =
    member x.Content = content

    abstract member Dispose : unit -> unit
    default x.Dispose() = ()

    interface IDisposable with
        member x.Dispose() = x.Dispose()


type DynamicProgramStatistics =
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
            DynamicProgramStatistics(
                DeltaProcessTime = TimeSpan.Zero,
                CompileTime = TimeSpan.Zero,
                WriteTime = TimeSpan.Zero,
                AddedFragmentCount = 0,
                RemovedFragmentCount = 0,
                CompiledFragmentCount = 0,
                UpdatedFragmentCount = 0,
                UpdatedJumpCount = 0
            )

        static member (+) (l : DynamicProgramStatistics, r : DynamicProgramStatistics) =
            DynamicProgramStatistics(
                DeltaProcessTime = l.DeltaProcessTime + r.DeltaProcessTime,
                CompileTime = l.CompileTime + r.CompileTime,
                WriteTime = l.WriteTime + r.WriteTime,
                AddedFragmentCount = l.AddedFragmentCount + r.AddedFragmentCount,
                RemovedFragmentCount = l.RemovedFragmentCount + r.RemovedFragmentCount,
                CompiledFragmentCount = l.CompiledFragmentCount + r.CompiledFragmentCount,
                UpdatedFragmentCount = l.UpdatedFragmentCount + r.UpdatedFragmentCount,
                UpdatedJumpCount = l.UpdatedJumpCount + r.UpdatedJumpCount
            )

        static member (-) (l : DynamicProgramStatistics, r : DynamicProgramStatistics) =
            DynamicProgramStatistics(
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
    
    abstract member Update : IAdaptiveObject -> DynamicProgramStatistics
    abstract member Run : 'i -> unit
    
    abstract member Disassemble : unit -> obj
    abstract member AutoDefragmentation : bool with get, set
    abstract member StartDefragmentation : unit -> Task<TimeSpan>
    abstract member DefragmentationStarted : IEvent<unit>
    abstract member DefragmentationDone : IEvent<TimeSpan>
    abstract member NativeCallCount : int
    abstract member FragmentCount : int
    abstract member ProgramSizeInBytes : int64
    abstract member TotalJumpDistanceInBytes : int64



module private DifferentialProgram =
    type FragmentContext<'a> =
        {
            dynamicArguments : int
            compileDelta : Option<'a> -> 'a -> AdaptiveCode
            mutable memory : MemoryManager
            nativeCallCount : ref<int>
            jumpDistance : ref<int>
        }

    [<AllowNullLiteral>]
    type Fragment<'a> =
        class
            inherit AdaptiveObject

            val mutable public Context : FragmentContext<'a>
            val mutable public Storage : CodeFragment
            val mutable public Tag : Option<'a>
            val mutable public Prev : Fragment<'a>
            val mutable public Next : Fragment<'a>
            val mutable public Code : AdaptiveCode
            val mutable public CodePrevTag : Option<'a>
            val mutable public CallCount : int
            val mutable public JumpDistance : int

            member x.Recompile (caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let hasCode = not (isNull x.Code)
                    let upToDate = hasCode && Object.Equals(x.CodePrevTag, x.Prev.Tag)

                    if not upToDate then
                        if hasCode then x.Code.Dispose()
                        x.Code <- x.Context.compileDelta x.Prev.Tag x.Tag.Value
                        x.CodePrevTag <- x.Prev.Tag
                        true
                    else
                        false
                )

            member x.WriteContent (caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let code =
                        x.Code.Content
                            |> List.collect (fun c -> c.GetValue x)
                            |> List.toArray

                    let asm =
                        code |> ASM.assembleCalls x.Context.dynamicArguments

                    Interlocked.Add(x.Context.nativeCallCount, code.Length - x.CallCount) |> ignore
                    x.CallCount <- code.Length

                    if isNull x.Storage then
                        x.Storage <- CodeFragment(x.Context.memory, asm)
                        true
                    else
                        let ptr = x.Storage.Offset

                        // TODO: maybe partial updates here
                        x.Storage.Write(asm)
                        ptr <> x.Storage.Offset
                )

            member x.LinkPrev(caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let prevFragment = x.Prev.Storage
                    let myFragment = x.Storage

                    if prevFragment.ReadNextPointer() <> myFragment.Offset then
                        let distance = prevFragment.WriteNextPointer(myFragment.Offset)
                        Interlocked.Add(x.Context.jumpDistance, distance - x.JumpDistance) |> ignore
                        x.JumpDistance <- distance
                )

            member x.LinkNext(caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let nextFragment = x.Next.Storage
                    let myFragment = x.Storage

                    if myFragment.ReadNextPointer() <> nextFragment.Offset then
                        let distance = myFragment.WriteNextPointer(nextFragment.Offset)
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

                if not (isNull x.Storage) then
                    x.Storage.Dispose()
                    x.Storage <- null
            
                if not (isNull x.Code) then
                    x.Code.Dispose()
                    x.CodePrevTag <- None
                    x.Code <- null

            interface IDisposable with
                member x.Dispose() = x.Dispose()


            new(context, tag) = 
                { Context = context
                  Storage = null; Next = null; 
                  Prev = null; Tag = Some tag; 
                  Code = null; CodePrevTag = None
                  CallCount = 0; JumpDistance = 0 }

            new(context, storage) = 
                { Context = context
                  Storage = storage; Next = null; 
                  Prev = null; Tag = None; 
                  Code = null; CodePrevTag = None
                  CallCount = 0; JumpDistance = 0 }
        end



    module Defragmentation =
        let rec private evacuateKernel (startVersion : int) (version : byref<int>) (mem : MemoryManager) (results : List<Fragment<'a> * CodeFragment>) (last : CodeFragment) (current : Fragment<'a>) =
            if isNull current then 
                true
            else
                let currentMem = current.Storage.Memory
                let ptr = mem.Alloc(currentMem.Size)
                currentMem.CopyTo(ptr)

                let frag = CodeFragment(ptr, current.Storage.ContainsJmp)

                if not (isNull last) then
                    last.WriteNextPointer(ptr.Offset) |> ignore

                results.Add(current, frag)

                if version <> startVersion then false
                else evacuateKernel startVersion &version mem results frag current.Next

        let evacuate (lockObj : obj) (version : byref<int>) (context : FragmentContext<'a>) (memory : byref<MemoryManager>) (prolog : Fragment<'a>) =
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
                            frag.Storage <- code
                            context.jumpDistance := !context.jumpDistance - frag.JumpDistance
                            frag.JumpDistance <- 0

                        let old = memory
                        memory <- mem
                        context.memory <- mem
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


    type Program<'i, 'k, 'a when 'k : equality>(input : amap<'k, 'a>, maxArgumentCount : int,
                                                keyComparer : IComparer<'k>, 
                                                compileDelta : Option<'a> -> 'a -> AdaptiveCode ) =
        inherit AdaptiveObject()

        let reader = input.ASet.GetReader()
        let mutable memory = MemoryManager.createExecutable()
        let mutable version = -1

        let dynamicArguments = 
            if typeof<'i> = typeof<unit> then 0
            elif typeof<'i>.IsValueType then 1
            else failwith "not implemented"

        let cache = Dict<'k * 'a, Fragment<'a>>()
        let fragments = SortedDictionaryExt<'k, StableSet<Fragment<'a>>>(keyComparer)

        let nativeCallCount = ref 0
        let jumpDistance = ref 0
        let context = { dynamicArguments = dynamicArguments; memory = memory; compileDelta = compileDelta; nativeCallCount = nativeCallCount; jumpDistance = jumpDistance }
        let prolog = new Fragment<_>(context, CodeFragment(memory, ASM.functionProlog dynamicArguments maxArgumentCount))
        let epilog = new Fragment<_>(context, CodeFragment(memory, ASM.functionEpilog dynamicArguments maxArgumentCount))

        do prolog.Next <- epilog
           epilog.Prev <- prolog


        let deltaProcessWatch = System.Diagnostics.Stopwatch()
        let compileWatch = System.Diagnostics.Stopwatch()
        let writeWatch = System.Diagnostics.Stopwatch()
        let defragmentWatch = System.Diagnostics.Stopwatch()

        let dirtyLock = obj()
        let mutable dirtySet = HashSet<Fragment<'a>>()


        let defragStartEvent = EventSource()
        let defragDoneEvent = EventSource()
        let mutable autoDefragmentation = 1
        let mutable defragRunning = 0
        let mutable currentDefragTask = null

        let issueDefragmentation (this : Program<'i, 'k, 'a>) =
            let startTask (f : unit -> 'x) =
                Task.Factory.StartNew(f, TaskCreationOptions.LongRunning)

            let tryDefragment () =
                Defragmentation.evacuate (this :> obj) &version context &memory prolog

            let start = Interlocked.Exchange(&defragRunning, 1)
            if start = 0 then
                let task = 
                    startTask (fun () ->
                        Log.startTimed "defragmentation"
                        defragStartEvent.Emit()
                        defragmentWatch.Restart()
                        while not (tryDefragment()) do
                            Log.line "retry"
                        defragmentWatch.Stop()
                        Log.stop()

                        defragDoneEvent.Emit defragmentWatch.Elapsed
                        Interlocked.Exchange(&defragRunning, 0) |> ignore
                        defragmentWatch.Elapsed
                    )
                currentDefragTask <- task
                task
            else
                currentDefragTask

        let run = 
            let mutable currentPtr = 0n
            let mutable currentRun = ignore

            fun v ->
                ReaderWriterLock.read memory.PointerLock (fun () ->
                    let ptr = memory.Pointer + prolog.Storage.Offset

                    if currentPtr <> ptr then
                        currentPtr <- ptr
                        currentRun <- UnmanagedFunctions.wrap ptr


                    currentRun v
                )


        override x.InputChanged(o : IAdaptiveObject) =
            match o with
                | :? Fragment<'a> as o ->
                    lock dirtyLock (fun () -> dirtySet.Add o |> ignore)
                | _ ->
                    ()

        member x.Run (v : 'i) =
            lock x (fun () ->        
                run v
            )

        member x.Disassemble() =
            lock x (fun () ->
                let result = List()
                let mutable current = prolog.Next
                while current <> epilog do
                    let mem = current.Storage.Memory

                    let data = mem.UInt8Array
                    let instructions = AMD64.Disassembler.disassemble data
                    result.AddRange instructions
                    current <- current.Next

                result.ToArray()
            )

        member x.Update caller = 
            x.EvaluateIfNeeded caller DynamicProgramStatistics.Zero (fun v ->
                Interlocked.Increment(&version) |> ignore

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
            
                let prologNextPtr = 
                    let store = prolog.Next.Storage
                    if isNull store then -1n
                    else store.Offset

                let epilogPrevPtr = 
                    let store = epilog.Prev.Storage
                    if isNull store then -1n
                    else store.Offset


                let createBetween (prev : Option<Fragment<'a>>) (v : 'a) (next : Option<Fragment<'a>>)  =

                    match next with
                        | Some n -> recompileSet.Add n |> ignore
                        | _ -> ()

                    //let code = desc.compileDelta (Option.map DynamicFragment.tag prev) v (Option.map DynamicFragment.tag next)
                    let fragment = new Fragment<_>(context, v)

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

                                                if fragment.Next <> epilog then
                                                    recompileSet.Add fragment.Next |> ignore

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

                if autoDefragmentation = 1 && relinkSet.Count > 0 && !jumpDistance > 0 then
                    issueDefragmentation x |> ignore

                DynamicProgramStatistics (
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
            dirtySet.Clear()
            reader.Dispose()
            memory.Dispose()
            cache.Clear()
            fragments.Clear()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IAdaptiveProgram<'i> with
            member x.DefragmentationStarted = defragStartEvent :> IEvent<_>
            member x.DefragmentationDone = defragDoneEvent :> IEvent<_>

            member x.AutoDefragmentation
                with get() = autoDefragmentation = 1
                and set d =
                    autoDefragmentation <- if d then 1 else 0
                    if d && !jumpDistance > 0 then
                        issueDefragmentation x |> ignore

            member x.StartDefragmentation() = 
                if !jumpDistance > 0 then issueDefragmentation x
                else Task.FromResult TimeSpan.Zero

            member x.Update(caller) = x.Update(caller)
            member x.Run v = x.Run v

            member x.FragmentCount = cache.Count
            member x.NativeCallCount = !nativeCallCount
            member x.ProgramSizeInBytes = int64 (memory.AllocatedBytes - prolog.Storage.Memory.Size - epilog.Storage.Memory.Size)
            member x.TotalJumpDistanceInBytes = int64 (!jumpDistance - prolog.JumpDistance - epilog.JumpDistance)
            member x.Disassemble() = x.Disassemble() :> obj

module private SimpleProgram =

    type FragmentContext<'a> =
        {
            dynamicArguments : int
            compile : 'a -> AdaptiveCode
            mutable memory : MemoryManager
            nativeCallCount : ref<int>
            jumpDistance : ref<int>
        }

    [<AllowNullLiteral>]
    type Fragment<'a> =
        class
            inherit AdaptiveObject

            val mutable public Context : FragmentContext<'a>
            val mutable public Storage : CodeFragment
            val mutable public Tag : Option<'a>
            val mutable public Prev : Fragment<'a>
            val mutable public Next : Fragment<'a>
            val mutable public Code : AdaptiveCode
            val mutable public CallCount : int
            val mutable public JumpDistance : int


            member x.Recompile (caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let hasCode = not (isNull x.Code)

                    if not hasCode then
                        if hasCode then x.Code.Dispose()
                        x.Code <- x.Context.compile x.Tag.Value
                        true
                    else
                        false
                )

            member x.WriteContent (caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let code =
                        x.Code.Content
                            |> List.collect (fun c -> c.GetValue x)
                            |> List.toArray

                    let asm =
                        code |> ASM.assembleCalls x.Context.dynamicArguments

                    Interlocked.Add(x.Context.nativeCallCount, code.Length - x.CallCount) |> ignore
                    x.CallCount <- code.Length

                    if isNull x.Storage then
                        x.Storage <- CodeFragment(x.Context.memory, asm)
                        true
                    else
                        let ptr = x.Storage.Offset

                        // TODO: maybe partial updates here
                        x.Storage.Write(asm)
                        ptr <> x.Storage.Offset
                )

            member x.LinkPrev(caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let prevFragment = x.Prev.Storage
                    let myFragment = x.Storage

                    if prevFragment.ReadNextPointer() <> myFragment.Offset then
                        let distance = prevFragment.WriteNextPointer(myFragment.Offset)
                        Interlocked.Add(x.Context.jumpDistance, distance - x.JumpDistance) |> ignore
                        x.JumpDistance <- distance
                )

            member x.LinkNext(caller : IAdaptiveObject) =
                x.EvaluateAlways caller (fun () ->
                    let nextFragment = x.Next.Storage
                    let myFragment = x.Storage

                    if myFragment.ReadNextPointer() <> nextFragment.Offset then
                        let distance = myFragment.WriteNextPointer(nextFragment.Offset)
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

                if not (isNull x.Storage) then
                    x.Storage.Dispose()
                    x.Storage <- null
            
                if not (isNull x.Code) then
                    x.Code.Dispose()
                    x.Code <- null

            interface IDisposable with
                member x.Dispose() = x.Dispose()


            new(context, tag) = 
                { Context = context
                  Storage = null; Next = null; 
                  Prev = null; Tag = Some tag; 
                  Code = null; 
                  CallCount = 0; JumpDistance = 0 }

            new(context, storage) = 
                { Context = context
                  Storage = storage; Next = null; 
                  Prev = null; Tag = None; 
                  Code = null; 
                  CallCount = 0; JumpDistance = 0 }
        end 

    module Defragmentation =
        let rec private evacuateKernel (startVersion : int) (version : byref<int>) (mem : MemoryManager) (results : List<Fragment<'a> * CodeFragment>) (last : CodeFragment) (current : Fragment<'a>) =
            if isNull current then 
                true
            else
                let currentMem = current.Storage.Memory
                let ptr = mem.Alloc(currentMem.Size)
                currentMem.CopyTo(ptr)

                let frag = CodeFragment(ptr, current.Storage.ContainsJmp)

                if not (isNull last) then
                    last.WriteNextPointer(ptr.Offset) |> ignore

                results.Add(current, frag)

                if version <> startVersion then false
                else evacuateKernel startVersion &version mem results frag current.Next

        let evacuate (lockObj : obj) (version : byref<int>) (context : FragmentContext<'a>) (memory : byref<MemoryManager>) (prolog : Fragment<'a>) =
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
                            frag.Storage <- code
                            context.jumpDistance := !context.jumpDistance - frag.JumpDistance
                            frag.JumpDistance <- 0

                        let old = memory
                        memory <- mem
                        context.memory <- mem
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


    type Program<'i, 'k, 'a when 'k : equality>(input : amap<'k, 'a>, maxArgumentCount : int,
                                                keyComparer : IComparer<'k>, 
                                                compile : 'a -> AdaptiveCode ) =
        inherit AdaptiveObject()

        let reader = input.ASet.GetReader()
        let mutable memory = MemoryManager.createExecutable()
        let mutable version = -1

        let dynamicArguments = 
            if typeof<'i> = typeof<unit> then 0
            elif typeof<'i>.IsValueType then 1
            else failwith "not implemented"

        let cache = Dict<'k * 'a, Fragment<'a>>()
        let fragments = SortedDictionaryExt<'k, StableSet<Fragment<'a>>>(keyComparer)

        let nativeCallCount = ref 0
        let jumpDistance = ref 0
        let context = { dynamicArguments = dynamicArguments; memory = memory; compile = compile; nativeCallCount = nativeCallCount; jumpDistance = jumpDistance }
        let prolog = new Fragment<_>(context, CodeFragment(memory, ASM.functionProlog dynamicArguments maxArgumentCount))
        let epilog = new Fragment<_>(context, CodeFragment(memory, ASM.functionEpilog dynamicArguments maxArgumentCount))

        do prolog.Next <- epilog
           epilog.Prev <- prolog


        let deltaProcessWatch = System.Diagnostics.Stopwatch()
        let compileWatch = System.Diagnostics.Stopwatch()
        let writeWatch = System.Diagnostics.Stopwatch()
        let defragmentWatch = System.Diagnostics.Stopwatch()

        let dirtyLock = obj()
        let mutable dirtySet = HashSet<Fragment<'a>>()


        let defragStartEvent = EventSource()
        let defragDoneEvent = EventSource()
        let mutable autoDefragmentation = 1
        let mutable defragRunning = 0
        let mutable currentDefragTask = null

        let issueDefragmentation (this : Program<'i, 'k, 'a>) =
            let startTask (f : unit -> 'x) =
                Task.Factory.StartNew(f, TaskCreationOptions.LongRunning)

            let tryDefragment () =
                Defragmentation.evacuate (this :> obj) &version context &memory prolog

            let start = Interlocked.Exchange(&defragRunning, 1)
            if start = 0 then
                let task = 
                    startTask (fun () ->
                        Log.startTimed "defragmentation"
                        defragStartEvent.Emit()
                        defragmentWatch.Restart()
                        while not (tryDefragment()) do
                            Log.line "retry"
                        defragmentWatch.Stop()
                        Log.stop()

                        defragDoneEvent.Emit defragmentWatch.Elapsed
                        Interlocked.Exchange(&defragRunning, 0) |> ignore
                        defragmentWatch.Elapsed
                    )
                currentDefragTask <- task
                task
            else
                currentDefragTask

        let run = 
            let mutable currentPtr = 0n
            let mutable currentRun = ignore

            fun v ->
                ReaderWriterLock.read memory.PointerLock (fun () ->
                    let ptr = memory.Pointer + prolog.Storage.Offset

                    if currentPtr <> ptr then
                        currentPtr <- ptr
                        currentRun <- UnmanagedFunctions.wrap ptr


                    currentRun v
                )


        override x.InputChanged(o : IAdaptiveObject) =
            match o with
                | :? Fragment<'a> as o ->
                    lock dirtyLock (fun () -> dirtySet.Add o |> ignore)
                | _ ->
                    ()

        member x.Run (v : 'i) =
            lock x (fun () ->        
                run v
            )

        member x.Disassemble() =
            lock x (fun () ->
                let result = List()
                let mutable current = prolog.Next
                while current <> epilog do
                    let mem = current.Storage.Memory

                    let data = mem.UInt8Array
                    let instructions = AMD64.Disassembler.disassemble data
                    result.AddRange instructions
                    current <- current.Next

                result.ToArray()
            )

        member x.Update caller = 
            x.EvaluateIfNeeded caller DynamicProgramStatistics.Zero (fun v ->
                Interlocked.Increment(&version) |> ignore

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
            
                let prologNextPtr = 
                    let store = prolog.Next.Storage
                    if isNull store then -1n
                    else store.Offset

                let epilogPrevPtr = 
                    let store = epilog.Prev.Storage
                    if isNull store then -1n
                    else store.Offset


                let createBetween (prev : Option<Fragment<'a>>) (v : 'a) (next : Option<Fragment<'a>>)  =
                    let fragment = new Fragment<_>(context, v)

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

                if autoDefragmentation = 1 && relinkSet.Count > 0 && !jumpDistance > 0 then
                    issueDefragmentation x |> ignore

                DynamicProgramStatistics (
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
            dirtySet.Clear()
            reader.Dispose()
            memory.Dispose()
            cache.Clear()
            fragments.Clear()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IAdaptiveProgram<'i> with
            member x.DefragmentationStarted = defragStartEvent :> IEvent<_>
            member x.DefragmentationDone = defragDoneEvent :> IEvent<_>

            member x.AutoDefragmentation
                with get() = autoDefragmentation = 1
                and set d =
                    autoDefragmentation <- if d then 1 else 0
                    if d && !jumpDistance > 0 then
                        issueDefragmentation x |> ignore

            member x.StartDefragmentation() = 
                if !jumpDistance > 0 then issueDefragmentation x
                else Task.FromResult TimeSpan.Zero

            member x.Update(caller) = x.Update(caller)
            member x.Run v = x.Run v

            member x.FragmentCount = cache.Count
            member x.NativeCallCount = !nativeCallCount
            member x.ProgramSizeInBytes = int64 (memory.AllocatedBytes - prolog.Storage.Memory.Size - epilog.Storage.Memory.Size)
            member x.TotalJumpDistanceInBytes = int64 (!jumpDistance - prolog.JumpDistance - epilog.JumpDistance)
            member x.Disassemble() = x.Disassemble() :> obj


module AdaptiveProgram =
    
    let differential (maxArgs : int) (comparer : IComparer<'k>) (compileDelta : Option<'v> -> 'v -> AdaptiveCode) (input : amap<'k, 'v>) =
        new DifferentialProgram.Program<_,_,_>(input, maxArgs, comparer, compileDelta) :> IAdaptiveProgram<'i>

    let simple (maxArgs : int) (comparer : IComparer<'k>) (compile : 'v -> AdaptiveCode) (input : amap<'k, 'v>) =
        new SimpleProgram.Program<_,_,_>(input, maxArgs, comparer, compile) :> IAdaptiveProgram<'i>


    let inline run (p : IAdaptiveProgram<unit>) =
        p.Run()

    let inline update (p : IAdaptiveProgram<'a>) =
        p.Update(null)


    let inline nativeCallCount (p : IAdaptiveProgram<'i>) = p.NativeCallCount
    let inline fragmentCount (p : IAdaptiveProgram<'i>) = p.FragmentCount
    let inline programSizeInBytes (p : IAdaptiveProgram<'i>) = p.ProgramSizeInBytes
    let inline totalJumpDistanceInBytes (p : IAdaptiveProgram<'i>) = p.TotalJumpDistanceInBytes


module Tests =
    
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

                new AdaptiveCode([call])

            AdaptiveProgram.differential 1 Comparer.Default compileDelta

        let prog = calls |> AMap.ofASet |> compile
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
