namespace Aardvark.Base.Runtime

open System
open System.Threading
open System.Runtime.InteropServices
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental


type NativeCalls = list<NativeCall>

type AdaptiveCode(content : list<IMod<NativeCalls>>) =
    member x.Content = content

    abstract member Dispose : unit -> unit
    default x.Dispose() = ()

    interface IDisposable with
        member x.Dispose() = x.Dispose()

type CompileDelta<'a> = Option<'a> -> 'a -> Option<'a> -> AdaptiveCode

[<AllowNullLiteral>]
type DynamicFragment<'a> =
    class
        inherit AdaptiveObject

        val mutable public Storage : CodeFragment
        val mutable public Tag : 'a
        val mutable public Code : AdaptiveCode
        val mutable public CompileDelta : CompileDelta<'a>
        val mutable public Next : DynamicFragment<'a>
        val mutable public Prev : DynamicFragment<'a>
        
        val mutable private lastPrevTag : 'a


        member x.WriteContent (caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                let nextFragment = x.Next.Storage
                let myFragment = x.Storage
                let ptr = myFragment.Offset

                if not <| System.Object.ReferenceEquals(x.lastPrevTag, x.Prev.Tag) then
                    x.Code <- x.CompileDelta (Some x.Prev.Tag) x.Tag (Some x.Next.Tag)

                let code =
                    x.Code.Content
                        |> List.collect (fun c -> c.GetValue x)
                        |> List.toArray

                // TODO: maybe partial updates here
                myFragment.Write(code)
                ptr <> myFragment.Offset
            )

        member x.Link(caller : IAdaptiveObject) =
            x.EvaluateAlways caller  (fun () ->
                let prevFragment = x.Prev.Storage
                let myFragment = x.Storage

                if prevFragment.Next <> myFragment then
                    prevFragment.Next <- myFragment
                    myFragment.Prev <- prevFragment
            )

        member x.Dispose() =
            x.Code.Dispose()
            x.Storage.Dispose()

        interface IDisposable with
            member x.Dispose() = x.Dispose()


        new(storage, tag, compileDelta) = 
            { Storage = storage; Next = null; 
              Prev = null; lastPrevTag = Unchecked.defaultof<_>;
              Tag = tag; 
              Code = Unchecked.defaultof<_>;
              CompileDelta = compileDelta }

    end

type ProgramUpdateStatistics =
    {
        structureChangeTime : TimeSpan
        writeTime : TimeSpan
        added : int
        removed : int
        changed : int
        moved : int
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ProgramUpdateStatistics =
    let zero =
        {
            structureChangeTime = TimeSpan.Zero
            writeTime = TimeSpan.Zero
            added = 0
            removed = 0
            changed = 0
            moved = 0
        }

   

module DynamicFragment =

    let create (memory : MemoryManager) (tag : 'a) (compileDelta : CompileDelta<'a>) : DynamicFragment<'a> =
       let store = CodeFragment(memory)
       new DynamicFragment<_>(store, tag, compileDelta)

    let inline tag (f : DynamicFragment<'a>) =
        f.Tag


type DynamicProgramDescription<'k, 'a when 'k : equality> =
    {
        maxArgs : int
        compileDelta : CompileDelta<'a>
        input : amap<'k, 'a>
        comparer : IComparer<'k>
    }

type DynamicProgram<'k, 'a when 'k : equality>(desc : DynamicProgramDescription<'k, 'a>) =
    inherit AdaptiveObject()

    let reader = desc.input.ASet.GetReader()
    let memory = MemoryManager.createExecutable()

    let cache = Dict<'k * 'a, DynamicFragment<'a>>()
    let fragments = SortedDictionaryExt<'k, StableSet<DynamicFragment<'a>>> (curry desc.comparer.Compare)

    let prolog : DynamicFragment<'a> = new DynamicFragment<_>(CodeFragment(memory, Assembler.functionProlog desc.maxArgs), Unchecked.defaultof<_>, desc.compileDelta)
    let epilog : DynamicFragment<'a> = new DynamicFragment<_>(CodeFragment(memory, Assembler.functionEpilog desc.maxArgs), Unchecked.defaultof<_>, desc.compileDelta)

    let dirtyLock = obj()
    let mutable dirtySet = HashSet<DynamicFragment<'a>>()

    let run = CodeFragment.wrap prolog.Storage

    let sw = System.Diagnostics.Stopwatch()

    override x.InputChanged(o : IAdaptiveObject) =
        match o with
            | :? DynamicFragment<'a> as o ->
                lock dirtyLock (fun () -> dirtySet.Add o |> ignore)
            | _ ->
                ()

    member x.Run() =
        run()

    member x.Update caller = 
        x.EvaluateIfNeeded caller ProgramUpdateStatistics.zero (fun v ->
            let deltas = reader.GetDelta x

            let dirtySet = 
                lock dirtyLock (fun () ->
                    let set = dirtySet
                    dirtySet <- HashSet()
                    set
                )

            let mutable added = 0
            let mutable removed = 0
            let mutable changed = 0
            let mutable moveCount = 0
            
            sw.Restart()
            for d in deltas do
                match d with
                    | Add (k,v) ->
                        added <- added + 1
                        let createBetween (prev : Option<DynamicFragment<'a>>) (next : Option<DynamicFragment<'a>>)  =

                            //let code = desc.compileDelta (Option.map DynamicFragment.tag prev) v (Option.map DynamicFragment.tag next)
                            let fragment = DynamicFragment.create memory v desc.compileDelta

                            let l = match prev with | Some l -> l | None -> prolog
                            let r = match next with | Some r -> r | None -> epilog

                            r.Prev <- fragment
                            fragment.Prev <- l
                            fragment.Next <- r
                            l.Next <- fragment

                            fragment

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
                                        self.AddWithPrev (fun p -> createBetween p next) 

                                    match created with
                                        | Some f -> f
                                        | None -> failwithf "duplicated key: %A" k

                                | None ->
                                    let frag = createBetween prev next
                                    let set = StableSet()
                                    set.Add frag |> ignore
                                    fragments.[k] <- set
                                    frag

                        cache.[(k,v)] <- fragment
                        dirtySet.Add fragment |> ignore

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

                                            // next needs to be recompiled
                                            dirtySet.Add fragment.Next |> ignore
                                            dirtySet.Remove fragment |> ignore

                                            if set.Count = 0 then
                                                fragments.Remove k |> ignore

                                        else failwith  "could remove fragment from stable set: %A" k
                                    | _ ->
                                        failwithf "could not find Fragment for: %A" k
                            | _ -> 
                                failwithf "could not find container for: %A" k
            sw.Stop()
            let structuralTime = sw.Elapsed

            sw.Restart()
            let moved = HashSet()
            for d in dirtySet do
                changed <- changed + 1
                if d.WriteContent x then
                    moved.Add d |> ignore

            for d in moved do
                moveCount <- moveCount + 1
                d.Link x
            epilog.Link x

            sw.Stop()

            {
                structureChangeTime = structuralTime
                writeTime = sw.Elapsed
                added = added
                removed = removed
                changed = changed
                moved = moveCount
            }
        ) 

    member x.Dispose() =
        dirtySet.Clear()
        reader.Dispose()
        memory.Dispose()
        cache.Clear()
        fragments.Clear()

    interface IDisposable with
        member x.Dispose() = x.Dispose()


module Tests =
    
    let mutable deltaResults = List<int>()

    let check (l : list<int>) =
        let c = Seq.toList deltaResults
        if c <> l then failwithf "should be: %A, was %A" l c
        deltaResults.Clear()

    let printFunction (v : int) =
        deltaResults.Add v
        printfn "%A" v

    type Print = delegate of int -> unit
    let dPrint = Print printFunction
    let pPrint = Marshal.GetFunctionPointerForDelegate dPrint

    let run() =
        let calls  = 
            CMap.ofList [
                0,0
                3,3
                2,6
                1,9
            ]

        let desc =
            {
                maxArgs = 6
                compileDelta = fun p s _ -> 
                    match p with
                        | Some p -> new AdaptiveCode([Mod.constant [pPrint, [|s - p :> obj|]]])
                        | None -> new AdaptiveCode([Mod.constant [pPrint, [|s :> obj|]]])
                input = calls
                comparer = Comparer.Default
            }

        let prog = new DynamicProgram<_,_>(desc)

        prog.Update(null) |> printfn "update: %A"

        prog.Run()

        check [0; 9; -3; -3] 

        ()





