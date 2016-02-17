namespace Aardvark.Base.Incremental


open Aardvark.Base
open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Runtime.InteropServices


type Op =
    abstract member Target : obj
    abstract member Run : unit -> Op
    abstract member TryMerge : Op -> Option<Op>


type Change =
    | NoChange
    | Single of list<Op>
    | Union of list<Change>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Change =
    
    let empty = NoChange

    let single (o : Op) =
        Single [o]


    let ofSeq (s : seq<Op>) =
        Single (Seq.toList s)

    let ofList (l : list<Op>) =
        Single l

    let ofArray (a : Op[]) =
        Single (Array.toList a)

    let rec toSeq (c : Change) =
        seq {
            match c with
                | NoChange -> ()
                | Single ops -> 
                    yield! ops
                | Union changes ->
                    for c in changes do
                        yield! toSeq c
        }

    let rec toList (c : Change) =
        match c with
            | NoChange -> []
            | Single ops -> ops
            | Union changes -> changes |> List.collect toList

    let toArray (c : Change) =
        c |> toSeq |> Seq.toArray


    let isEmpty (c : Change) =
        match c with
            | NoChange -> true
            | _ -> false

    let union (c : #seq<Change>) =
        match Seq.toList c with
            | [] -> empty
            | [s] -> s
            | changes -> Union changes

    let rec iter (f : Op -> unit) (c : Change) =
        match c with
            | NoChange -> ()
            | Single ops -> ops |> List.iter f
            | Union changes -> changes |> List.iter (iter f)

    let append (l : Change) (r : Change) =
        match l, r with
            | NoChange, r -> r
            | l, NoChange -> l
            | l, r -> Union [l;r]


type pmod<'a>(name : string, value : 'a) =
    let r = ModRef<'a>(value)

    member x.Value = value
    member x.Name = name

    interface IWeakable<IAdaptiveObject> with
        member x.Weak = r.Weak

    interface IAdaptiveObject with
        member x.Id = r.Id
        member x.OutOfDate
            with get() = r.OutOfDate
            and set v = r.OutOfDate <- v

        member x.Outputs = r.Outputs
        member x.Inputs = r.Inputs
        member x.Level 
            with get() = r.Level
            and set l = r.Level <- l

        member x.Mark () =
            r.Mark ()

        member x.InputChanged ip = r.InputChanged ip


    interface IMod with
        member x.IsConstant = false
        member x.GetValue(caller) = r.GetValue(caller) :> obj

    interface IMod<'a> with
        member x.GetValue(caller) = r.GetValue(caller)

    override x.ToString() =
        sprintf "pmod { %s = %A }" name r.Value

    override x.GetHashCode() =
        r.GetHashCode()

    override x.Equals o =
        r.Equals o

    member internal x.Ref = r

module PMod =
    
    type ModChange<'a>(m : pmod<'a>, value : 'a) =
        interface Op with
            member x.Target = m :> obj
            member x.Run() =
                let old = m.Value
                m.Ref.Value <- value

                ModChange(m, old) :> Op

            member x.TryMerge o =
                match o with
                    | :? ModChange<'a> as o ->
                        None
                    | _ -> 
                        failwith "impossible"

        override x.ToString() =
            sprintf "%s = %A" m.Name value

    let change (m : pmod<'a>) (value : 'a) =
        Single [ModChange(m, value) :> Op]



type pset<'a>(name : string, value : PersistentHashSet<'a>) =
    let r = cset<'a>()
    let a = r :> aset<_>
    member x.Value = value

    interface aset<'a> with
        member x.ReaderCount = a.ReaderCount
        member x.IsConstant = false
        member x.Copy = a
        member x.GetReader() = a.GetReader()


type Commit =
    { 
        hash : Guid
        ops : list<Op>
        message : string
        author : string
        date : DateTime
        mergeSource : Option<string * History>
    }

and History =
    | Empty
    | Commit of History * Commit 

type MergeResult =
    | Success
    | Conflicts of list<Op * Op>


type WorkingCopy(remote : string) =
    let refs = Dictionary<string, IMod>()
    let sets = Dictionary<string, obj>()

    let mutable history = Empty
    let mutable localModifications = NoChange

    let mutable branch = "master"

    let branches = Dictionary<string, History>()
    do branches.Add("master", history) |> ignore

    let mutable undo = Dict<Op, Op>()
    
    member x.Undo = undo


    member x.Remote = remote
    member x.Branches = branches

    member x.Branch
        with get() = branch
        and set b = branch <- b

    member x.History
        with get() = history
        and set h = history <- h

    member x.LocalModifications
        with get() = localModifications
        and set l = localModifications <- l

    member x.NewRef(name : string, value : 'a) =
        match refs.TryGetValue name with
            | (true, (:? pmod<'a> as m)) -> m
            | _ ->
                let m = pmod(name, value)
                refs.[name] <- m
                m

    member x.NewSet(name : string, value : PersistentHashSet<'a>) =
        match sets.TryGetValue name with
            | (true, (:? pset<'a> as s)) -> s
            | _ ->
                let s = pset(name, value)
                sets.[name] <- s
                s
    
    member x.NewSet(name : string) =
        x.NewSet(name, PersistentHashSet.empty)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Git =
    
    [<AutoOpen>]
    module private Helpers =
        
        let inline (==) (a : 'a) (b : 'a) = Object.ReferenceEquals(a,b)
        let inline (!=) (a : 'a) (b : 'a) = Object.ReferenceEquals(a,b) |> not



        let findCommonAncestor (l : History) (r : History) =
            let visited = HashSet<Guid>()
            let rec run (l : History) (r : History) =
                match l with
                    | Empty -> l
                    | Commit(b,{ hash = h }) ->
                        if visited.Contains h then
                            l
                        else
                            visited.Add h |> ignore
                            run r b
                
            run l r


        let rec backward (w : WorkingCopy) (current : History) (target : History) =
            if current != target then
                match current with
                    | Commit(b,{ ops = ops; message = msg; mergeSource = m }) ->
                        
                        ops |> List.iter (fun o ->
                            match w.Undo.TryRemove o with
                                | (true, u) -> u.Run() |> ignore
                                | _ -> failwithf "cannot undo operation in commit %A" msg
                        )

                        backward w b target

                    | Empty ->
                        ()
        
        let private undo (w : WorkingCopy) (o : Op) =
            match w.Undo.TryRemove o with
                | (true, u) -> u.Run() |> ignore
                | _ ->  ()

        let rec forward (w : WorkingCopy) (current : History) (target : History) =
            if current != target then
                match target with
                    | Commit(b,{ ops = ops }) ->
                        forward w current b
                        ops |> List.iter (fun o -> w.Undo.[o] <- o.Run())
                    | _ -> ()

        let rec forwardMerge (w : WorkingCopy) (conflictTable : Dictionary<obj, Op>) (current : History) (target : History) =
            if current != target then
                match target with
                    | Commit(b,{ ops = ops }) ->
                        let res, inner = forwardMerge w conflictTable current b
                        let rec tryMerge (ops : list<Op>) =
                            match ops with
                                | [] -> [], []
                                | o::ops ->
                                    let working, conflicts = tryMerge ops

                                    let op = 
                                        match conflictTable.TryGetValue o.Target with
                                            | (true, other) ->
                                                match o.TryMerge(other) with
                                                    | Some res ->
                                                        Left res
                                                    | None -> Right (o, other)
                                            | _ -> Left o

                                    match op with
                                        | Left o ->
                                            o::working, conflicts
                                        | Right c ->
                                            working, c::conflicts
     
                        let working, conflicts = tryMerge ops

                        match res with
                            | Success ->
                                if List.isEmpty conflicts then 
                                    working |> List.iter (fun o -> w.Undo.[o] <- o.Run())
                                    let result = Change.append inner (Change.ofList working)

                                    Success, result
                                else
                                    backward w b current
                                    Conflicts conflicts, NoChange


                            | Conflicts inner ->
                                Conflicts (inner @ conflicts), NoChange

                    | _ -> Success, NoChange
            else
                Success,NoChange

        let rec changeFromTo (current : History) (target : History) =
            if current != target then
                match target with
                    | Commit(b,{ ops = ops }) ->
                        let inner = changeFromTo current b
                        Change.append inner (Change.ofList ops)
                    | _ -> NoChange
            else
                NoChange

    let init (folder : string) = WorkingCopy(folder)

    let pmod (name : string) (value : 'a) (w : WorkingCopy) =
        w.NewRef(name, value)

    let pset (name : string) (w : WorkingCopy) =
        w.NewSet(name)

    let commit (message : string) (w : WorkingCopy) =
        let ops = w.LocalModifications |> Change.toList

        match ops with
            | [] -> 
                Log.warn "nothing to commit"
            | ops ->

                let commit =
                    {
                        hash = Guid.NewGuid()
                        ops = ops
                        message = message
                        author = Environment.UserName
                        date = DateTime.UtcNow
                        mergeSource = None
                    }

                let newHistory = Commit(w.History, commit)

                w.History <- newHistory
                w.LocalModifications <- NoChange
                w.Branches.[w.Branch] <- newHistory


    let checkout (branch : string) (w : WorkingCopy) =
        if branch <> w.Branch then
            if Change.isEmpty w.LocalModifications then
                match w.Branches.TryGetValue branch with
                    | (true, target) ->
                        let current = w.History

                        let anc = findCommonAncestor current target

                        transact (fun () ->
                            backward w current anc
                            forward w anc target
                        )

                        w.Branch <- branch
                        w.History <- target

                        ()
                    | _ ->
                        w.Branches.[branch] <- w.History
                        w.Branch <- branch

            
            else
                Log.warn "cannot checkout branch %s because you have local changes" branch

    let apply (change : Change) (w : WorkingCopy) =
        transact (fun () ->
            change |> Change.iter (fun o -> w.Undo.[o] <- o.Run())
        )
        w.LocalModifications <- Change.append w.LocalModifications change

    let merge (source : string) (w : WorkingCopy) =
        if source <> w.Branch then
            if Change.isEmpty w.LocalModifications then
                match w.Branches.TryGetValue source with
                    | (true, s) ->

                        let anc = findCommonAncestor s w.History


                        let changesOnMyBranch = changeFromTo anc w.History

                        let conflictTable = Dictionary<obj, Op>()
                        changesOnMyBranch |> Change.iter (fun o ->
                            conflictTable.[o.Target] <- o
                        )

                        // TODO: check for conflicts
                        let mergeRes, change = 
                            transact (fun () ->
                                forwardMerge w conflictTable anc s
                            )

                        match mergeRes with
                            | Success ->

                                let commit =
                                    {
                                        hash = Guid.NewGuid()
                                        ops = Change.toList change
                                        message = sprintf "Merge branch %s into %s" source w.Branch
                                        author = Environment.UserName
                                        date = DateTime.UtcNow
                                        mergeSource = Some(source, s)
                                    }

                                let newHistory = Commit(w.History, commit)

                                w.History <- newHistory
                                w.LocalModifications <- NoChange
                                w.Branches.[w.Branch] <- newHistory
                            | Conflicts l ->
                                Log.warn "merge conflict"
                                for (l,r) in l do
                                    Log.warn "    conflicting assignments: {%A} vs {%A}" l r

                    | _ ->
                        Log.warn "could not find branch %A" source
            else
                Log.warn "cannot merge when having local modifications"

    let log (w : WorkingCopy) =
        let rec log (h : History) =
            seq {
                match h with
                    | Empty -> ()
                    | Commit(b, commit) ->
                        yield commit
                        yield! log b
            }

        log w.History

[<AutoOpen>]
module WorkingCopyExtensions =
    

    type WorkingCopy with
        member x.pmod (name : string) (value : 'a) =
            Git.pmod name value x

        
        member x.pset (name : string) =
            Git.pset name x

        member x.commit (message : string) =
            Git.commit message x

        member x.checkout (branch : string) =
            Git.checkout branch x

        member x.apply change =
            Git.apply change x

        member x.merge source =
            Git.merge source x

        member x.log =
            Git.log x

        member x.getlog (length : int) =
            let s = x.log

            let e = s.GetEnumerator()
            let cnt = ref length
            [
                while !cnt > 0 && e.MoveNext() do
                    yield e.Current
                    cnt := !cnt - 1
                e.Dispose()
            ]

        member x.printLog() =
            let l = x.getlog(10)
            for c in l do
                Log.start "%A" c.hash
                Log.line "message: %s" c.message
                Log.line "author:  %s" c.author
                Log.line "date:    %A" c.date
                Log.start "changes:"
                for o in c.ops do
                    Log.line "%A" o
                Log.stop()
                Log.stop()

                    
        

module GitTest =
    
    let run() =
        let git = Git.init "temp"

        let a = git.pmod "a" 1
        let b = git.pmod "b" 1

        let c = Mod.map2 (+) a b
        
        let print() =
            Log.line "%s: %A" git.Branch (Mod.force c)

        print()

        // create a new branch a10, set a to 10 and commit it
        git.checkout "a10"
        git.apply (PMod.change a 10)
        git.commit "a = 10"
        print()


        // go back to master, create a new branch b5, set b to 5 and commit it
        git.checkout "master"
        git.checkout "b5"
        git.apply (PMod.change b 5)
        git.commit "b = 5"
        print()
        

        // switch back to a10
        git.checkout "a10"
        print()

        // and again back to b5
        git.checkout "b5"
        print()

        // merge the branch a10 into b5
        git.checkout "merge"
        git.merge "a10"
        print()


        git.checkout "a10"
        git.checkout "conflict"
        git.apply (PMod.change b 10)
        git.commit "sync"
        git.merge "b5"
        print()

        git.printLog()

        // and back to master
        git.checkout "master"
        print()

        git.checkout "merge"
        print()
        git.printLog()










        





