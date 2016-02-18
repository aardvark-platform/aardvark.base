namespace Aardvark.Base.Incremental


open Aardvark.Base
open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Runtime.InteropServices


module DB =
    
    type Table = { content : IDict<Guid, obj> }

    type Database = { tables : IDict<string, Table> }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Database =
        let create() = { tables = Dict() }

        let getTable (name : string) (db : Database) =
            match db.tables.TryGetValue name with
                | (true, t) -> t
                | _ ->
                    let t = { content = Dict() }
                    db.tables.[name] <- t
                    t


//        let store (file : string) (db : Database) =
//            
//        

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Table =
        let read (id : Guid) (t : Table) =
            t.content.[id]

        let write (id : Guid) (value : obj) (t : Table) =
            t.content.[id] <- value

        let insert (value : obj) (t : Table) =
            let id = Guid.NewGuid()
            t.content.[id] <- value
            id

        let delete (id : Guid) (t : Table) =
            t.content.Remove(id) |> ignore


    
type Thunk<'a>(f : unit -> 'a) =
    
    member x.Value = f()


type IRef =
    abstract member Name : string


type Op =
    abstract member Target : IRef
    abstract member Run : unit -> Op
    abstract member TryMergeParallel : Op -> Option<Op>
    abstract member MergeSequential : Op -> Op

type Change =
    private 
    | NoChange
    | Change of HashMap<IRef, Op>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Change =
    
    module private HashMap =
        let update (key : 'k) (f : Option<'a> -> 'a) (m : HashMap<'k, 'a>) =
            let old = HashMap.tryFind key m
            HashMap.add key (f old) m

    let empty = NoChange

    let single (o : Op) =
        Change (HashMap.ofList [o.Target, o])


    let ofSeq (s : seq<Op>) =
        let mutable m = HashMap.empty
        for op in s do
            m <- m |> HashMap.update op.Target (fun o -> 
                match o with
                    | None -> op
                    | Some o -> o.MergeSequential op
            )
        Change m

    let ofList (l : list<Op>) =
        ofSeq l

    let ofArray (a : Op[]) =
        ofSeq a

    let toSeq (c : Change) =
        seq {
            match c with
                | NoChange -> ()
                | Change map ->
                    yield! map |> HashMap.toSeq |> Seq.map snd
        }

    let rec toList (c : Change) =
        match c with
            | NoChange -> []
            | Change map ->
                map |> HashMap.toList |> List.map snd

    let toArray (c : Change) =
        c |> toSeq |> Seq.toArray


    let isEmpty (c : Change) =
        match c with
            | NoChange -> true
            | Change m -> HashMap.isEmpty m

    let append (l : Change) (r : Change) =
        match l, r with
            | NoChange, r -> r
            | l, NoChange -> l
            | Change l, Change r ->
                let mutable res = l
                for (k,v) in HashMap.toSeq r do
                    res <- res |> HashMap.update k (fun ov ->
                        match ov with
                            | Some ov -> ov.MergeSequential v
                            | _ -> v
                    )
                Change res

    let concat (c : #seq<Change>) =
        match Seq.toList c with
            | [] -> empty
            | f::r -> 
                let mutable res = f
                for c in r do
                    res <- append res c
                res

    

    let iter (f : Op -> unit) (c : Change) =
        match c with
            | NoChange -> ()
            | Change map ->
                map |> HashMap.toSeq |> Seq.iter(fun (_,op) -> f op)



type pmod<'a>(name : string, value : 'a) =
    let r = ModRef<'a>(value)

    interface IRef with
        member x.Name = name

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
            member x.Target = m :> IRef
            member x.Run() =
                let old = m.Value
                m.Ref.Value <- value

                ModChange(m, old) :> Op

            member x.TryMergeParallel o =
                match o with
                    | :? ModChange<'a> as o ->
                        None
                    | _ -> 
                        failwith "impossible"

            member x.MergeSequential o =
                match o with
                    | :? ModChange<'a> as o -> o :> Op
                    | _ ->
                        failwith "impossible"

        override x.ToString() =
            sprintf "%s = %A" m.Name value

    let change (m : pmod<'a>) (value : 'a) =
        Change.single (ModChange(m, value) :> Op)



type pset<'a>(name : string, value : PersistentHashSet<'a>) =
    let r = cset<'a>()
    let a = r :> aset<_>
    member x.Value = value

    interface IRef with
        member x.Name = name

    interface aset<'a> with
        member x.ReaderCount = a.ReaderCount
        member x.IsConstant = false
        member x.Copy = a
        member x.GetReader() = a.GetReader()




[<CustomEquality; CustomComparison>]
type Commit =
    { 
        hash : Guid
        ops : list<Op>
        message : string
        author : string
        date : DateTime
        parent : Commit
        mergeSource : Option<string * Commit>
    }

    override x.GetHashCode() = x.hash.GetHashCode()
    override x.Equals o =
        match o with
            | :? Commit as o -> x.hash = o.hash
            | _ -> false

    interface IComparable with
        member x.CompareTo o =
            match o with
                | :? Commit as o -> compare x.hash o.hash
                | _ -> failwith "uncomparable"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Commit =
    let root =
        {
            hash = Guid.Empty
            ops = []
            message = "root"
            author = "root"
            date = DateTime.MinValue
            parent = Unchecked.defaultof<_>
            mergeSource = None
        }

[<AutoOpen>]
module CommitPatterns =
    let (|Root|Commit|) (c : Commit) =
        if c.hash = Guid.Empty then Root
        else Commit(c.parent, c)




type MergeResult =
    | Merged
    | Conflicts of list<Op * Op>

type WorkingCopy =
    {
        mutable History : Commit
        mutable LocalModifications : Change
        mutable Branch : string
        mutable Branches : Dictionary<string, Commit>
        mutable Undo : Dict<Op, Op>
        mutable Refs : Dict<string, IRef>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Git =
    
    [<AutoOpen>]
    module private Helpers =
        let findCommonAncestor (l : Commit) (r : Commit) =
            let visited = HashSet<Guid>()
            let rec run (l : Commit) (r : Commit) =
                match l with
                    | Root -> l
                    | Commit(b, { hash = h }) ->
                        if visited.Contains h then
                            l
                        else
                            visited.Add h |> ignore
                            run r b
                
            run l r


        let rec backward (w : WorkingCopy) (current : Commit) (target : Commit) =
            if current != target then
                match current with
                    | Commit(b,{ ops = ops; message = msg; mergeSource = m }) ->
                        
                        ops |> List.iter (fun o ->
                            match w.Undo.TryRemove o with
                                | (true, u) -> u.Run() |> ignore
                                | _ -> failwithf "cannot undo operation in commit %A" msg
                        )

                        backward w b target

                    | Root ->
                        ()
        
        let private undo (w : WorkingCopy) (o : Op) =
            match w.Undo.TryRemove o with
                | (true, u) -> u.Run() |> ignore
                | _ ->  ()

        let rec forward (w : WorkingCopy) (current : Commit) (target : Commit) =
            if current != target then
                match target with
                    | Commit(b,{ ops = ops }) ->
                        forward w current b
                        ops |> List.iter (fun o -> w.Undo.[o] <- o.Run())
                    | _ -> ()

        let rec forwardMerge (w : WorkingCopy) (conflictTable : Dictionary<obj, Op>) (current : Commit) (target : Commit) =
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
                                                match o.TryMergeParallel(other) with
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
                            | Merged ->
                                if List.isEmpty conflicts then 
                                    working |> List.iter (fun o -> w.Undo.[o] <- o.Run())
                                    let result = Change.append inner (Change.ofList working)

                                    Merged, result
                                else
                                    backward w b current
                                    Conflicts conflicts, NoChange


                            | Conflicts inner ->
                                Conflicts (inner @ conflicts), NoChange

                    | _ -> Merged, NoChange
            else
                Merged,NoChange

        let rec changeFromTo (current : Commit) (target : Commit) =
            if current != target then
                match target with
                    | Commit(b,{ ops = ops }) ->
                        let inner = changeFromTo current b
                        Change.append inner (Change.ofList ops)
                    | _ -> NoChange
            else
                NoChange

    let init () =
        {
            History = Commit.root
            LocalModifications = Change.empty
            Branch = "master"
            Branches = Dictionary.ofList ["master", Commit.root]
            Undo = Dict.empty
            Refs = Dict.empty
        }

    let pmod (name : string) (value : 'a) (w : WorkingCopy) =
        w.Refs.GetOrCreate(name, fun name ->
            pmod(name, value) :> IRef
        ) |> unbox<pmod<'a>>

    let pset (name : string) (w : WorkingCopy) =
        w.Refs.GetOrCreate(name, fun name ->
            pset(name, PersistentHashSet.empty) :> IRef
        ) |> unbox<pset<'a>>


    let commit (message : string) (w : WorkingCopy) =
        let ops = w.LocalModifications |> Change.toList

        match ops with
            | [] -> 
                Log.warn "nothing to commit"
            | ops ->

                let newHistory =
                    {
                        hash = Guid.NewGuid()
                        ops = ops
                        message = message
                        author = Environment.UserName
                        date = DateTime.UtcNow
                        mergeSource = None
                        parent = w.History
                    }


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
                            | Merged ->

                                let newHistory =
                                    {
                                        hash = Guid.NewGuid()
                                        ops = Change.toList change
                                        message = sprintf "Merge branch %s into %s" source w.Branch
                                        author = Environment.UserName
                                        date = DateTime.UtcNow
                                        mergeSource = Some(source, s)
                                        parent = w.History
                                    }


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
        let rec log (h : Commit) =
            seq {
                match h with
                    | Root -> ()
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
            x.log |> Seq.atMost length

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
        let git = Git.init()

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










        





