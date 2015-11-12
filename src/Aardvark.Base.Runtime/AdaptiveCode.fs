namespace Aardvark.Base.Runtime

open System
open System.Threading
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

type DynamicProgramDescription<'k, 'a when 'k : equality> =
    {
        input : amap<'k, 'a>
        maxArgumentCount : int
        keyComparer : IComparer<'k>
        compileDelta : Option<'a> -> 'a -> Option<'a> -> AdaptiveCode
    }

type DynamicFragmentContext<'a> =
    {
        compileDelta : Option<'a> -> 'a -> Option<'a> -> AdaptiveCode
        memory : MemoryManager
    }


[<AllowNullLiteral>]
type DynamicFragment<'a> =
    class
        inherit AdaptiveObject

        val mutable public Context : DynamicFragmentContext<'a>
        val mutable public Storage : CodeFragment
        val mutable public Tag : Option<'a>
        val mutable public Prev : DynamicFragment<'a>
        val mutable public Next : DynamicFragment<'a>
        val mutable public Code : AdaptiveCode
        val mutable public CodePrevTag : Option<'a>

        member x.Recompile (caller : IAdaptiveObject) =
            let hasCode = not (isNull x.Code)
            let upToDate = hasCode && Object.Equals(x.CodePrevTag, x.Prev.Tag)

            if not upToDate then
                if hasCode then x.Code.Dispose()
                x.Code <- x.Context.compileDelta x.Prev.Tag x.Tag.Value x.Next.Tag
                x.CodePrevTag <- x.Prev.Tag
                true
            else
                false

        member x.WriteContent (caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                let code =
                    x.Code.Content
                        |> List.collect (fun c -> c.GetValue x)
                        |> List.toArray

                if isNull x.Storage then
                    x.Storage <- CodeFragment(x.Context.memory, code)
                    true
                else
                    let ptr = x.Storage.Offset

                    // TODO: maybe partial updates here
                    x.Storage.Write(code)
                    ptr <> x.Storage.Offset
            )

        member x.Link(caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                let prevFragment = x.Prev.Storage
                let myFragment = x.Storage

                if prevFragment.NextPointer <> myFragment.Offset then
                    prevFragment.NextPointer <- myFragment.Offset
            )

        member x.Dispose() =
            x.Prev <- null
            x.Next <- null

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
              Code = null; CodePrevTag = None }

        new(context, storage) = 
            { Context = context
              Storage = storage; Next = null; 
              Prev = null; Tag = None; 
              Code = null; CodePrevTag = None }
    end

type ProgramUpdateStatistics =
    {
        recompileTime : TimeSpan
        addRemoveTime : TimeSpan
        writeTime : TimeSpan


        recompiled : int
        jumpAdjusted : int
        updated : int
        added : int
        removed : int
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ProgramUpdateStatistics =
    let zero =
        {
            recompileTime = TimeSpan.Zero
            addRemoveTime = TimeSpan.Zero
            writeTime = TimeSpan.Zero
            recompiled = 0
            jumpAdjusted = 0
            updated = 0
            added = 0
            removed = 0
        }

   

module DynamicFragment =

    let create (context : DynamicFragmentContext<'a>) (tag : 'a) : DynamicFragment<'a> =
       new DynamicFragment<_>(context, tag)

    let inline tag (f : DynamicFragment<'a>) =
        f.Tag




type DynamicProgram<'k, 'a when 'k : equality>(desc : DynamicProgramDescription<'k, 'a>) =
    inherit AdaptiveObject()

    let reader = desc.input.ASet.GetReader()
    let memory = MemoryManager.createExecutable()

    let cache = Dict<'k * 'a, DynamicFragment<'a>>()
    let fragments = SortedDictionaryExt<'k, StableSet<DynamicFragment<'a>>>(desc.keyComparer)

    let context = { memory = memory; compileDelta = desc.compileDelta }
    let prolog : DynamicFragment<'a> = new DynamicFragment<_>(context, CodeFragment(memory, Assembler.functionProlog desc.maxArgumentCount))
    let epilog : DynamicFragment<'a> = new DynamicFragment<_>(context, CodeFragment(memory, Assembler.functionEpilog desc.maxArgumentCount))

    do prolog.Next <- epilog
       epilog.Prev <- prolog

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
        lock x (fun () ->        
            run()
        )

    member x.Update caller = 
        x.EvaluateIfNeeded caller ProgramUpdateStatistics.zero (fun v ->
            let deltas = reader.GetDelta x

            let dirtySet = 
                lock dirtyLock (fun () ->
                    let set = dirtySet
                    dirtySet <- HashSet()
                    set
                )

            let recompileSet = HashSet()
            let relinkSet = HashSet()

            let mutable added = 0
            let mutable removed = 0
            let mutable changed = 0
            let mutable moveCount = 0
            
            let prologNextPtr = 
                let store = prolog.Next.Storage
                if isNull store then -1n
                else store.Offset

            let epilogPrevPtr = 
                let store = epilog.Prev.Storage
                if isNull store then -1n
                else store.Offset


            let createBetween (prev : Option<DynamicFragment<'a>>) (v : 'a) (next : Option<DynamicFragment<'a>>)  =

                match next with
                    | Some n -> recompileSet.Add n |> ignore
                    | _ -> ()

                //let code = desc.compileDelta (Option.map DynamicFragment.tag prev) v (Option.map DynamicFragment.tag next)
                let fragment = DynamicFragment.create context v

                let l = match prev with | Some l -> l | None -> prolog
                let r = match next with | Some r -> r | None -> epilog

                r.Prev <- fragment
                fragment.Prev <- l
                fragment.Next <- r
                l.Next <- fragment

                fragment

            sw.Restart()
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

                                            recompileSet.Add fragment.Next |> ignore

                                            fragment.Dispose()
                                            dirtySet.Remove fragment |> ignore
                                            recompileSet.Remove fragment |> ignore

                                            if set.Count = 0 then
                                                fragments.Remove k |> ignore

                                        ()
                                    | _ ->
                                        failwithf "could not find Fragment for: %A" k
                            | _ -> 
                                failwithf "could not find container for: %A" k
            sw.Stop()
            let addRemoveTime = sw.Elapsed




            sw.Restart()
            for r in recompileSet do
                if r.Recompile x then
                    relinkSet.Add r |> ignore
                    dirtySet.Add r |> ignore

            sw.Stop()
            let recompileTime = sw.Elapsed


            sw.Restart()
            
            for d in dirtySet do
                changed <- changed + 1
                if d.WriteContent x then
                    relinkSet.Add d |> ignore

            for d in relinkSet do
                moveCount <- moveCount + 1
                d.Link x


            if prolog.Next.Storage.Offset <> prologNextPtr then
                moveCount <- moveCount + 1
                prolog.Storage.NextPointer <- prolog.Next.Storage.Offset

            if epilog.Prev.Storage.Offset <> epilogPrevPtr then
                moveCount <- moveCount + 1
                epilog.Prev.Storage.NextPointer <- epilog.Storage.Offset


            sw.Stop()
            let writeTime = sw.Elapsed


            {
                recompileTime = recompileTime
                addRemoveTime = addRemoveTime
                writeTime = writeTime

                recompiled = recompileSet.Count
                jumpAdjusted = relinkSet.Count
                updated = changed
                added = added
                removed = removed
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
    
    let printFunction (v : int) =
        printf "%A " v

    type Print = delegate of int -> unit
    let dPrint = Print printFunction
    let pPrint = Marshal.GetFunctionPointerForDelegate dPrint

    let run() =
        let calls  = 
            CSet.ofList [
                0,0
                3,3
                2,6
                1,9
            ]

        let desc =
            {
                input = AMap.ofASet calls
                maxArgumentCount = 6
                keyComparer = Comparer.Default
                compileDelta = fun p s _ -> 
                    match p with
                        | Some p -> new AdaptiveCode([Mod.constant [pPrint, [|s - p :> obj|]]])
                        | None -> new AdaptiveCode([Mod.constant [pPrint, [|s :> obj|]]])
            }

        let prog = new DynamicProgram<_,_>(desc)

        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""

        transact (fun () ->
            calls |> CSet.remove (0,0) |> ignore
        )

        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""


        transact (fun () ->
            calls |> CSet.add (1,10) |> ignore
        )
        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""


        transact (fun () ->
            calls |> CSet.unionWith (List.init 1000 (fun i -> (-1, i)))
        )


        prog.Update(null) |> printfn "update: %A"
        prog.Run(); printfn ""




