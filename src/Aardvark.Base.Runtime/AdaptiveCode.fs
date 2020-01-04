namespace Aardvark.Base.Runtime

open System
open System.Threading
open System.Threading.Tasks
open System.Runtime.InteropServices
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open FSharp.Data.Adaptive

module internal Unchecked =
    let inline isNull<'a when 'a : not struct>(value : 'a) = isNull (value :> obj)


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
                "AdaptiveProgramStatistics {"
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
    
    /// updates the program representation if needed and returns some statistics
    abstract member Update : AdaptiveToken -> AdaptiveProgramStatistics

    /// runs the program with the given input argument
    /// NOTE: that no updates are performed here
    abstract member Run : 'i -> unit
    
    /// disassembles the underlying program and returns
    /// an implementation specific representation
    abstract member Disassemble : unit -> obj

    /// gets or sets a flag indicating whether or not the program should automatically
    /// be defragmented whenever the jump-distance is non-zero
    abstract member AutoDefragmentation : bool with get, set

    /// explicitly starts a defragmentation task if needed
    abstract member StartDefragmentation : unit -> Task<TimeSpan>

    /// gets the number of native calls currently performed by the program
    abstract member NativeCallCount : int

    /// gets the number of elements currently contained in the program
    abstract member FragmentCount : int

    /// gets a total size for the underlying program data
    abstract member ProgramSizeInBytes : int64

    /// gets a total jump distance for the underlying program (if applicable)
    abstract member TotalJumpDistanceInBytes : int64

[<AllowNullLiteral>]
type IAdaptiveCode<'instruction> =
    inherit IDisposable
    abstract member Content : list<aval<list<'instruction>>>

type AdaptiveCode<'instruction>(content : list<aval<list<'instruction>>>) =
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
        disassemble : 'fragment -> list<'instruction>
        dispose : unit -> unit
    }


module internal GenericProgram =

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
            val mutable public IsDisposed : bool

            interface IFragment<'fragment> with
                member x.Next = x.Next :> IFragment<_>
                member x.Storage = x.Storage.Value

                member x.JumpDistance
                    with get() = x.JumpDistance
                    and set d = x.JumpDistance <- d

            member x.Recompile (token : AdaptiveToken) =
                x.EvaluateAlways token (fun token ->
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

            member x.WriteContent (token : AdaptiveToken) =
                x.EvaluateAlways token (fun token ->
                    if not (isNull x.Code) then
                        let code =
                            x.Code.Content
                                |> List.collect (fun c -> c.GetValue token)
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

            member x.LinkNext(token : AdaptiveToken) =
                x.EvaluateAlways token (fun token ->
                    if Unchecked.isNull x.Next && Unchecked.isNull x.Prev then
                        Log.warn "[AdaptiveCode] tried to link detached fragment { prev = %A; next = %A }" x.Prev x.Next
                    elif x.IsDisposed then
                        Log.warn "[AdaptiveCode] tried to link disposed fragment"
                    elif x.Next.IsDisposed then
                        failwith "[AdaptiveCode] tried to link fragment with disposed next"
                    elif Option.isNone x.Storage then
                        failwith "[AdaptiveCode] tried to link uninitialized fragment"
                    elif Option.isNone x.Next.Storage then
                        failwith "[AdaptiveCode] tried to link fragment with uninitialized next"
                    else
                        let myFragment = x.Storage.Value
                        let nextFragment = x.Next.Storage.Value

                        if not (x.Context.isNext myFragment nextFragment) then
                            let distance = x.Context.writeNext myFragment nextFragment
                            Interlocked.Add(x.Context.jumpDistance, distance - x.JumpDistance) |> ignore
                            x.JumpDistance <- distance
                )

            member x.Dispose() =
                x.Prev <- Unchecked.defaultof<_>
                x.Next <- Unchecked.defaultof<_>
                x.IsDisposed <- true

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
                  Storage = None; Next = Unchecked.defaultof<_>; 
                  Prev = Unchecked.defaultof<_>; Tag = Some tag; 
                  Code = null; CodePrevTag = None
                  CallCount = 0; JumpDistance = 0; IsDisposed = false }

            new(context, storage) = 
                { Context = context
                  Storage = Some storage; Next = Unchecked.defaultof<_>; 
                  Prev = Unchecked.defaultof<_>; Tag = None; 
                  Code = null; CodePrevTag = None
                  CallCount = 0; JumpDistance = 0; IsDisposed = false }
        end

    type Program<'i, 'k, 'instruction, 'fragment, 'a>
        (input : aset<'k * 'a>,
         keyComparer : IComparer<'k>,
         newHandler : unit -> FragmentHandler<'i, 'a, 'instruction, 'fragment>) =
        inherit AdaptiveObject()

        let reader = input.GetReader()
        let version = ref -1

        let cache = Dictionary<obj, Fragment<'i, 'a, 'instruction, 'fragment>>()
        let fragments = SortedDictionaryExt<'k, StableSet<Fragment<'i, 'a, 'instruction, 'fragment>>>(keyComparer)

        let handler = newHandler()

        let prolog = new Fragment<_,_,_,_>(handler, handler.prolog)
        let epilog = new Fragment<_,_,_,_>(handler, handler.epilog)

        do prolog.Next <- epilog
           epilog.Prev <- prolog
           prolog.WriteContent(AdaptiveToken.Top) |> ignore
           epilog.WriteContent(AdaptiveToken.Top) |> ignore
           handler.writeNext prolog.Storage.Value epilog.Storage.Value |> ignore


        let deltaProcessWatch = System.Diagnostics.Stopwatch()
        let compileWatch = System.Diagnostics.Stopwatch()
        let writeWatch = System.Diagnostics.Stopwatch()

        let dirtyLock = obj()
        let mutable dirtySet = System.Collections.Generic.HashSet<Fragment<'i, 'a, 'instruction, 'fragment>>()

        let mutable autoDefragmentation = 1

        #if DEBUG && false
        let validateCurrentState(deltas : list<Delta<'k * 'v>>) =
            let mutable hasErrors = false
            let mutable cnt = 0
            let mutable last = null
            let mutable current = prolog
            let all = HashSet()
            while not (isNull current) do
                all.Add current |> ignore
                if current.IsDisposed then
                    hasErrors <- true
                    Log.warn "found disposed fragment in list: %A" current.Id

                if current.Prev <> last then
                    hasErrors <- true
                    let lastId = if isNull last then "(null)" else string last.Id
                    Log.warn "found invalid prev-pointer from %A to %s" current.Id lastId

                last <- current
                current <- current.Next

                cnt <- cnt + 1

            if cnt - 2 <> cache.Count then
                hasErrors <- true
                Log.warn "unexpected fragment count: { real = %A; cache = %A }" (cnt - 2) cache.Count

            for (KeyValue(o, c)) in cache do
                let k,t = unbox<'k * 'a> o
                if not (cache.ContainsKey o) then
                    Log.warn "fun fact: enumerated values does not exist in dict"

                if c.IsDisposed then
                    hasErrors <- true
                    Log.warn "found disposed fragment in cache: { Id = %A; Key = %A; Tag = %A }" c.Id k t

                if not (all.Contains c) then
                    hasErrors <- true
                    Log.warn "found cached fragment which is not reachable: { Id = %A; Key = %A; Tag = %A }" c.Id k t

            if hasErrors then
                for d in deltas do
                    match d with
                        | Add (k,v) -> Log.warn "Add %A" k
                        | Rem (k,v) -> Log.warn "Rem %A" k
                failwith "[AdaptiveCode] validation failed"
        #else
        let validateCurrentState(deltas : HashSetDelta<'k * 'v>) = ()
        #endif
        override x.InputChangedObject(t, o : IAdaptiveObject) =
            match o with
                | :? Fragment<'i, 'a, 'instruction, 'fragment> as o ->
                    if not o.IsDisposed then
                        lock dirtyLock (fun () -> dirtySet.Add o |> ignore)
                | _ ->
                    ()

        
        member x.Run (v : 'i) =
            lock x (fun () ->        
                handler.run v
            )

        member x.Disassemble() =
            lock x (fun () ->        
                let code = List()
                let mutable current = prolog
                while current <> epilog do
                    if current <> prolog then
                        match current.Storage with
                            | None -> code.Add [||]
                            | Some s -> code.Add(handler.disassemble s |> List.toArray)

                    match current.Storage, current.Next.Storage with
                        | Some l, Some r -> 
                            if not (handler.isNext l r) then
                                failwith "bad pointers"
                        | _ -> ()

                    current <- current.Next
                code.ToArray()
            )

        member x.Update token = 
            // update basically runs in 4 stages:
            //  1) process input deltas adding/removing fragments
            //  2) create code for fragments which need to be recompiled.
            //     recompile can be triggered either because they're new or their 
            //     predecessor changed and compileDelta depends on the it.
            //  3) write the code for all changed fragments to their underlying 
            //     (possibly native) representation. This is obviously needed for new
            //     fragments but also for the ones where some inner-instruction changed.
            //  4) link fragments appropriately where needed. Fragments must obviously be
            //     linked whenever Next changes but also when WriteContent() returns true
            //     (which is the case when the underlying representation has been moved)
            // known issues:
            //   - whenver an instruction changes the entire fragment is re-assembled and written
            //     as a whole. The write itself may not be too bad but the assemble-step 
            //     (at least in the native implementation) might be expensive
            //   - core-implementations (like CodeFragment) provide Append/Update/Clear function which
            //     are no longer optimal for this new implementation. 
            //     a possibly better API would be (where bool indicates if the fragment was moved)
            //       Write : list<list<Instruction>> -> bool * list<Handle>
            //       Update : Handle -> list<Instruction> -> bool
            x.EvaluateAlways token (fun token ->
                if x.OutOfDate then
                    Interlocked.Increment(version) |> ignore

                    // start by getting the deltas from our input-set
                    let deltas = reader.GetChanges token

                    // get all fragments whose inner code-representation changed. 
                    let dirtySet = 
                        lock dirtyLock (fun () ->
                            let set = dirtySet
                            dirtySet <- System.Collections.Generic.HashSet()
                            set
                        )

                    // TODO: move to class-fields
                    let deadSet = System.Collections.Generic.HashSet()
                    let recompileSet = System.Collections.Generic.HashSet()
                    let relinkSet = System.Collections.Generic.HashSet()

                    let mutable added = 0
                    let mutable removed = 0
            

                    // utility function for creating a new fragment
                    // between two given ones.
                    // prev = None encodes for prolog
                    // next = None encodes for epilog
                    let createBetween (prev : Option<Fragment<'i, 'a, 'instruction, 'fragment>>) (v : 'a) (next : Option<Fragment<'i, 'a, 'instruction, 'fragment>>)  =
                        // create the new fragment
                        let fragment = new Fragment<'i, 'a, 'instruction, 'fragment>(handler, v)

                        // get the predecessor
                        let prev = 
                            match prev with 
                                | Some l -> l 
                                | None -> prolog

                        // get the successor and add it to the
                        // recompile-set whenever compileDelta depends on
                        // predecessors (since next's prev will be changed)
                        let mutable next = 
                            match next with 
                                | Some r -> 
                                    if handler.compileNeedsPrev then recompileSet.Add r |> ignore
                                    r 
                                | None -> epilog
                    
                        if prev.Next <> next then
                            Log.warn "[AdaptiveCode] bad fragment creation: prev.Next <> next"
                            Log.warn "               using prev as anchor (next = prev.Next)"
                            next <- prev.Next



                        // prev's successor was changed so we need to relink it
                        // NOTE that the new fragment must not be added here
                        //      since it will be processed by stages 1-4 anyway.
                        relinkSet.Add prev |> ignore

                        // insert the new fragment in the linked list.
                        fragment.Prev <- prev
                        fragment.Next <- next
                        next.Prev <- fragment
                        prev.Next <- fragment

                        // finally return the new fragment
                        fragment

                    // stage 1: process all the deltas 
                    deltaProcessWatch.Restart()
                    for d in deltas do
                        match d with
                            | Add (_,(k,v)) ->
                                if cache.ContainsKey((k,v)) then
                                    Log.warn "[AdaptiveCode] duplicate addition of element %A" k
                                else
                                    added <- added + 1

                                    // for new fragments we need to find the neighbouring ones
                                    // when possible which is achieved by searching for buckets
                                    // in our top-level trie.
                                    let left,self,right = SortedDictionary.neighbourhood k fragments

                                    // when the right bucket is existing (and therefore non-empty)
                                    // the new fragment's next will be the first element from that bucket
                                    let next =
                                        match right with
                                            | Some(_,r) -> r.First
                                            | None -> None

                                    // when the left bucket is existing (and therefore non-empty)
                                    // the new fragment's prev will be the last element from that bucket
                                    let prev =
                                        match left with
                                            | Some(_,l) -> l.Last
                                            | None -> None

                                    // create and insert the new fragment at the appropriate
                                    // position in the linked list.
                                    let fragment = 
                                        match self with
                                            | Some self ->
                                                // if a bucket exists for the exact same key we add the new fragment
                                                // at the end of the bucket. The fragment's next will be the first one
                                                // in the right bucket (given by next)
                                                let prev =
                                                    match self.Last with    
                                                        | Some last -> Some last
                                                        | None -> prev

                                                let fragment = createBetween prev v next
                                                self.Add fragment |> ignore

                                                // the creation cannot fail here since we always create
                                                // a new fragment here (which cannot have been in the bucket)
                                                //created.Value
                                                fragment

                                            | None ->
                                                // if no bucket was created for the fragment's key yet
                                                // the fragments neighbours are prev and next (as defined above).
                                                let frag = createBetween prev v next


                                                // we obviously need to create a new bucket which will
                                                // simply contain the created fragment and add it to the 
                                                // top-level trie.
                                                let set = StableSet()
                                                set.Add frag |> ignore
                                                fragments.[k] <- set
                                                frag

                                    // store the fragment in the cache (which is needed for removal)
                                    cache.[(k,v)] <- fragment

                                    // since the fragment was just created it must obviously be
                                    // recompiled.
                                    recompileSet.Add fragment |> ignore

                                    validateCurrentState deltas

                            | Rem (_,(k,v)) ->
                                let mutable set = Unchecked.defaultof<_>
                                let mutable fragment = Unchecked.defaultof<_>
                            
                                // when an element is removed we first need
                                // to find its associated bucket in the top-level
                                // trie using its key.
                                if not (fragments.TryGetValue(k, &set)) then
                                    failwithf "[AdaptiveProgram] failed to get containing bucket for %A" k

                                // furthermore we need to find the associated
                                // fragment for the given element and remove it
                                // from the cache.
                                if not (cache.TryGetValue((k,v), &fragment)) then
                                    failwithf "[AdaptiveProgram] failed to get fragment from cache: %A" k

                                if not (cache.Remove (k,v)) then
                                    failwithf "[AdaptiveProgram] failed to remove fragment from cache: %A" k

                                if not (set.Remove fragment) then
                                    failwithf "[AdaptiveProgram] failed to remove fragment from containing bucket: %A %A" fragment set

                                // finally we can remove the fragment from the
                                // associated bucket and the linked list.
                                removed <- removed + 1 

                                // remove the fragment from the linked list
                                fragment.Next.Prev <- fragment.Prev
                                fragment.Prev.Next <- fragment.Next

                                // the fragment's prev needs to be relinked since
                                // its successor was just changed
                                relinkSet.Add fragment.Prev |> ignore


                                // the fragment's next needs to be recompiled whenever
                                // compileDelta depends on predecessors.
                                if handler.compileNeedsPrev && fragment.Next <> epilog then
                                    recompileSet.Add fragment.Next |> ignore

                                // release all resources associated with the fragment
                                fragment.Dispose()

                                // since the fragment may be added to the recompileSet
                                // by a subsequent change it is not sufficient to remove it
                                // from recompile/relink/dirtySet but instead we need to
                                // maintain a "persistent" deadSet.
                                deadSet.Add fragment |> ignore


                                // if the bucket just got empty remove it
                                // from the top-level trie.
                                if set.Count = 0 then
                                    if not (fragments.Remove k) then
                                        failwithf "[AdaptiveProgram] failed to remove bucket: %A" k

                                validateCurrentState deltas

                    deltaProcessWatch.Stop()

                    validateCurrentState deltas

                    // remove all dead fragments from the
                    // update-sets
                    dirtySet.ExceptWith deadSet
                    recompileSet.ExceptWith deadSet
                    relinkSet.ExceptWith deadSet
                    recompileSet.ExceptWith [prolog; epilog]

                    // stage 2: recompile
                    compileWatch.Restart()
                    for r in recompileSet do
                        // create new code and if the code actually changed
                        // add the fragment to dirty/relinkSet.
                        if r.Recompile token then
                            relinkSet.Add r |> ignore
                            dirtySet.Add r |> ignore
                    compileWatch.Stop()


                    // stage 3: write
                    writeWatch.Restart()
                    for d in dirtySet do
                        // each dirty fragment needs to be written
                        // to its underlying representation.
                        if d.WriteContent token then
                            // whenever the fragment's location changed
                            // it needs to be relinked.
                            relinkSet.Add d.Prev |> ignore

                    // stage 4: relink
                    for d in relinkSet do
                        try
                            d.LinkNext token
                        with _ ->
                            let mutable index = -1
                            let mutable current = prolog
                            while not (Unchecked.isNull current) && current <> d do
                                current <- current.Next
                                index <- index + 1
                            let reachable = current = d
                            if not reachable then
                                failwith "[AdaptiveCode] unreachable fragment not disposed"
                            else
                                Log.error "fragment reachable at index: %A" index
                                reraise ()

                    writeWatch.Stop()


                    // if AutoDefragmentation is enabled and the total jumpDistance is non-zero
                    // start a new defragmentation-task.
                    if autoDefragmentation = 1 && relinkSet.Count > 0 && !handler.jumpDistance > 0 then
                        handler.startDefragmentation (x :> obj) version |> ignore

                    // finally return some update-statistics
                    let stats = 
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
                    stats
                else
                    AdaptiveProgramStatistics.Zero
            ) 

        member x.Dispose() =
            for f in fragments do
                for f in f.Value do
                    f.Dispose()

            fragments.Clear()
            dirtySet.Clear()
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

            disassemble = fun f ->
                f.Calls |> Array.toList
        }

    let nativeSimple (maxArgs : int) (compile : 'value -> IAdaptiveCode<NativeCall>) ()  =
        let desc = nativeDifferential maxArgs (fun _ v -> compile v) ()
        { desc with compileNeedsPrev = false }

    let native (maxArgs : int) () =
        nativeDifferential maxArgs (fun _ _ -> failwith "no compileDelta given") ()

    let warpDifferential (mapping : 'a -> 'b) (backward : 'b -> 'a) (compile : Option<'v> -> 'v -> IAdaptiveCode<'a>) (newInner : unit -> FragmentHandler<'i, 'v, 'b, 'frag>) =
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
                disassemble = fun f -> f |> inner.disassemble |> List.map backward
            }
    
    let wrapSimple (mapping : 'a -> 'b) (backward : 'b -> 'a) (compile : 'v -> IAdaptiveCode<'a>) (newInner : unit -> FragmentHandler<'i, 'v, 'b, 'frag>) =
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
                disassemble = fun f -> f |> inner.disassemble |> List.map backward
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
            disassemble = fun f -> f.Values |> Array.toList
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
        p.Update(AdaptiveToken.Top)


    let inline nativeCallCount (p : IAdaptiveProgram<'i>) = p.NativeCallCount
    let inline fragmentCount (p : IAdaptiveProgram<'i>) = p.FragmentCount
    let inline programSizeInBytes (p : IAdaptiveProgram<'i>) = p.ProgramSizeInBytes
    let inline totalJumpDistanceInBytes (p : IAdaptiveProgram<'i>) = p.TotalJumpDistanceInBytes
