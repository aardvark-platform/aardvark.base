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


type IRef =
    abstract member Name : string


type Op =
    abstract member Target : IRef
    abstract member Run : unit -> Op
    abstract member Data : seq<obj>
    abstract member TryMergeParallel : Op -> Option<Op>
    abstract member MergeSequential : Op -> Op

    abstract member Serialize : (obj -> string) -> Map<string, string>



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

    let run (c : Change) =
        match c with
            | NoChange -> NoChange
            | Change map ->
                let mutable inverse = HashMap.empty

                for (k,op) in HashMap.toSeq map do
                    let inv = op.Run()
                    inverse <- HashMap.add k inv inverse

                Change inverse




type pmod<'a> private(name : string, defaultValue : Option<'a>) =
    let mutable defaultValue = defaultValue
    let r = lazy ( ModRef<'a>(defaultValue.Value) )

    interface IRef with
        member x.Name = name

    member x.DefaultValue
        with get() = defaultValue.Value
        and set d = 
            match defaultValue with
                | Some d -> ()
                | None -> defaultValue <- Some d


    member x.Value = r.Value.Value
    member x.Name = name

    interface IWeakable<IAdaptiveObject> with
        member x.Weak = r.Value.Weak

    interface IAdaptiveObject with
        member x.Id = r.Value.Id
        member x.OutOfDate
            with get() = r.Value.OutOfDate
            and set v = r.Value.OutOfDate <- v

        member x.Outputs = r.Value.Outputs
        member x.Inputs = r.Value.Inputs
        member x.Level 
            with get() = r.Value.Level
            and set l = r.Value.Level <- l

        member x.Mark () =
            r.Value.Mark ()

        member x.InputChanged ip = r.Value.InputChanged ip


    interface IMod with
        member x.IsConstant = false
        member x.GetValue(caller) = r.Value.GetValue(caller) :> obj

    interface IMod<'a> with
        member x.GetValue(caller) = r.Value.GetValue(caller)

    override x.ToString() =
        sprintf "pmod { %s = %A }" name r.Value.Value

    override x.GetHashCode() =
        r.Value.GetHashCode()

    override x.Equals o =
        r.Value.Equals o

    member internal x.Ref = r.Value

    new(name, value) = pmod<'a>(name, Some value)
    new(name) = pmod<'a>(name, None)

module PMod =
    
    type ModChange<'a>(m : pmod<'a>, value : 'a) =
        interface Op with
            member x.Data = Seq.singleton (value :> obj)
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

            member x.Serialize (store : obj -> string) =
                Map.ofList [
                    "Value",        store value
                ]

        static member Deserialize (target : pmod<'a>, data : Map<string, string>, load : string -> obj) =
            let value = data.["Value"] |> load |> unbox<'a>
            ModChange(target, value)

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
        forward : list<Op>
        backward : list<Op>
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

[<AutoOpen>]
module GuidExts =
    let private max = Guid(Int32.MaxValue, Int16.MaxValue, Int16.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)
    type Guid with
        static member MaxValue = max

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Commit =



    let root =
        {
            hash = Guid.Empty
            forward = []
            backward = []
            message = "root"
            author = "root"
            date = DateTime.MinValue
            parent = Unchecked.defaultof<_>
            mergeSource = None
        }

    let unrelated =
        {
            hash = Guid.MaxValue
            forward = []
            backward = []
            message = "unrelated"
            author = "unrelated"
            date = DateTime.MaxValue
            parent = Unchecked.defaultof<_>
            mergeSource = None
        }


    let inline isRoot (c : Commit) = c.hash = Guid.Empty

    let inline message (c : Commit) = c.message
    let inline author (c : Commit) = c.author
    let inline date (c : Commit) = c.date
    let inline parent (c : Commit) = c.parent
    let inline mergeSource (c : Commit) = c.mergeSource
    let inline forward (c : Commit) = c.forward
    let inline backward (c : Commit) = c.backward
    let inline hash (c : Commit) = c.hash


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
        mutable LocalModificationsInverse : Change
        mutable Branch : string
        mutable Branches : Dictionary<string, Commit>
        mutable Commits : Dictionary<Guid, Commit>
        mutable Data : Dictionary<string, obj>
        mutable Refs : Dict<string, IRef>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Git =

    open System.IO
    open System.Reflection
        
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
                    | Commit(b,{ backward = ops; message = msg; mergeSource = m }) ->
                        
                        ops |> List.iter (fun o ->
                            o.Run() |> ignore
                        )

                        backward w b target

                    | Root ->
                        ()
        

        let rec forward (w : WorkingCopy) (current : Commit) (target : Commit) =
            if current != target then
                match target with
                    | Commit(b,{ forward = ops }) ->
                        forward w current b
                        ops |> List.iter (fun o -> o.Run() |> ignore)
                    | _ -> ()

        let rec forwardMerge (w : WorkingCopy) (conflictTable : Dictionary<obj, Op>) (current : Commit) (target : Commit) =
            if current != target then
                match target with
                    | Commit(b,{ forward = ops }) ->
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
                    | Commit(b,{ forward = ops }) ->
                        let inner = changeFromTo current b
                        Change.append inner (Change.ofList ops)
                    | _ -> NoChange
            else
                NoChange

    [<AutoOpen>]
    module private Serialization = 
        open System.IO
        open Nessos.FsPickler
        open Nessos.FsPickler.Hashing
        open Nessos.FsPickler.Json

        let pickler = FsPickler.CreateJsonSerializer(true, true)
        let bson = FsPickler.CreateBsonSerializer()

        let objHash (o : obj) =
            let hash = FsPickler.ComputeHash o
            System.Convert.ToBase64String(hash.Hash).Replace('/', '_')

        let ensureExists (dir : string) =
            if not (Directory.Exists dir) then
                Directory.CreateDirectory dir |> ignore

        let exists (dir : string) =
            File.Exists dir

        let (++) (str : string) (name : string) =
            Path.Combine(str, name)

        let (!!) (str : string) =
            not (File.Exists str) && not (Directory.Exists str)

        type StorableCommit =
            {
                shash : Guid
                smessage : string
                sforward : list<Map<string, string>>
                sbackward : list<Map<string, string>>
                sauthor : string
                sdate : DateTime
                sparent : Guid
                smergeSource : Option<string * Guid>
            }


    let push (allBranches : bool) (force : bool) (target : string) (w : WorkingCopy) =
        ensureExists target


        let data = target ++ "data"
        ensureExists data

        let dataTable = Dict<obj, string>()

        let storeData (d : obj) =
            if d = null then "null"
            else
                dataTable.GetOrCreate(d, fun o ->
                    let hash = objHash o
                    let binary = bson.Pickle(o)
                

                    let target = data ++ hash
                    if !!target then File.WriteAllBytes(target, binary)

                    hash
                )

        let commits = target ++ "commits"
        ensureExists commits

        let rec storeCommits (c : Commit) =
            if not (Commit.isRoot c) then
                let file = commits ++ (string c.hash)
                if File.Exists file then
                    ()
                else
                    let serializeOp (o : Op) =
                        let m = o.Serialize storeData
                        m |> Map.add "Type" (o.GetType().FullName)
                          |> Map.add "Target" o.Target.Name

                    let storable =
                        {
                            shash = c.hash
                            smessage = c.message
                            sforward = c.forward |> List.map serializeOp
                            sbackward = c.backward |> List.map serializeOp
                            sauthor = c.author
                            sdate = c.date
                            sparent = c.parent.hash
                            smergeSource = c.mergeSource |> Option.map (fun (a,b) -> a, b.hash)
                        }

                    let data = pickler.Pickle storable
                    File.WriteAllBytes(file, data)

                    storeCommits c.parent

        let headsFile = target ++ "heads.json"
        let remoteHeads : Map<string, Guid> = 
            if !!headsFile then Map.empty
            else pickler.UnPickle (File.ReadAllBytes headsFile)

        let branches =
            if allBranches then w.Branches.Keys |> Seq.filter (fun k -> k.StartsWith "remote" |> not) |> Seq.toList
            else [w.Branch]

        let remoteState =
            branches 
                |> List.map (fun b ->
                    match Map.tryFind b remoteHeads with
                        | Some h -> 
                            match w.Commits.TryGetValue h with
                                | (true, c) -> b,c
                                | _ -> b,Commit.unrelated
                        | None -> b,Commit.root

                   )
                |> Map.ofList


        let bad = 
            if force then []
            else remoteState |> Map.filter (fun _ v -> v = Commit.unrelated) |> Map.toSeq |> Seq.map fst |> Seq.toList

        if List.isEmpty bad then
            
            let mutable heads = remoteHeads 

            for b in branches do
                let myCommit = w.Branches.[b]
                storeCommits myCommit
                heads <- Map.add b myCommit.hash heads

            pickler.Pickle heads |> File.writeAllBytes headsFile


        else
            Log.warn "remote contains changes not in your local copy (please integrate them)"

//        if allBranches then
//            let heads = 
//                w.Branches 
//                    |> Dictionary.toMap
//                    |> Map.filter (fun k _ -> k.StartsWith "remotes" |> not)
//                    |> Map.map (fun _ c -> c.hash)
//
//            pickler.Pickle heads |> File.writeAllBytes (target ++ "heads.json")
//
//            for (b,c) in Dictionary.toSeq w.Branches do
//                if not (b.StartsWith "remotes/") then
//                    storeCommits c
//        else
//
//            
//            let heads = 
//                w.Branches 
//                    |> Dictionary.toMap
//                    |> Map.map (fun _ c -> c.hash)
//
//            pickler.Pickle heads |> File.writeAllBytes (target ++ "heads.json")
//
//
//            let c = w.Branches.[w.Branch]
//            storeCommits c



    let private remoteBranchName (branch : string) =
        sprintf "remote/%s" branch

    let fetch (source : string) (w : WorkingCopy) =
        let commits = source ++ "commits"
        let data = source ++ "data"
        let heads = source ++ "heads.json"

        if !!source then
            failwithf "could not fetch from %A (does not exist)" source

        if !!commits || !!data || !!heads then
            failwithf "not a valid repo: %A" source

        let heads : Map<string, Guid> = pickler.UnPickle(File.ReadAllBytes heads)
        
        let loadData (str : string) =
            if str = "null" then Unchecked.defaultof<_>
            else
                match w.Data.TryGetValue str with
                    | (true, v) -> v
                    | _ ->
                        let path = data ++ str
                        if exists path then
                            let data = path |> File.ReadAllBytes |> bson.UnPickle
                            w.Data.[str] <- data
                            data
                        else
                            failwithf "could not get data for hash: %A" str

        let rec loadCommit (id : Guid) =
            match w.Commits.TryGetValue id with
                | (true, c) -> c
                | _ ->
                    let file = commits ++ string id

                    if exists file then
                        let sc : StorableCommit = pickler.UnPickle (File.ReadAllBytes file)

                        let parent = loadCommit sc.sparent

                        let mergeSource =
                            match sc.smergeSource with
                                | Some (name, c) ->
                                    Some (name, loadCommit c)
                                | _ ->
                                    None

                        let deserializeOp (m : Map<string, string>) =
                            let target = m.["Target"]

                            let opType = System.Type.GetType(m.["Type"])
                            let des = opType.GetMethod("Deserialize", BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Static)
                            let target =
                                match w.Refs.TryGetValue target with
                                    | (true, target) -> target
                                    | _ -> 
                                        let t = des.GetParameters().[0].ParameterType
                                        let ctor = t.GetConstructor [|typeof<string>|]
                                        let res = ctor.Invoke([|target|]) |> unbox<IRef>
                                        w.Refs.[target] <- res
                                        res


                            des.Invoke(null, [|target; m; loadData|]) |> unbox<Op>

                        let commit =
                            {
                                hash = sc.shash
                                message = sc.smessage
                                forward = sc.sforward |> List.map deserializeOp
                                backward = sc.sbackward |> List.map deserializeOp
                                author = sc.sauthor
                                date = sc.sdate
                                parent = parent
                                mergeSource = mergeSource
                            }

                        w.Commits.[sc.shash] <- commit

                        commit

                    else
                        failwithf "failed to load commit: %A" id

    
        for (name, h) in Map.toSeq heads do
            let c = loadCommit h
            w.Branches.[remoteBranchName name] <- c

        //forward w w.History w.Branches.[w.Branch]





    let init () =
        {
            History = Commit.root
            LocalModifications = Change.empty
            LocalModificationsInverse = Change.empty
            Branch = "master"
            Branches = Dictionary.ofList ["master", Commit.root]
            Commits = Dictionary.ofList [Guid.Empty, Commit.root]
            Data = Dictionary.empty
            Refs = Dict.empty
        }

    let pmod (name : string) (defaultValue : 'a) (w : WorkingCopy) =
        let result = 
            w.Refs.GetOrCreate(name, fun name ->
                pmod(name, defaultValue) :> IRef
            ) |> unbox<pmod<'a>>

        result.DefaultValue <- defaultValue
        result

    let pset (name : string) (w : WorkingCopy) =
        w.Refs.GetOrCreate(name, fun name ->
            pset(name, PersistentHashSet.empty) :> IRef
        ) |> unbox<pset<'a>>


    let commit (message : string) (w : WorkingCopy) =
        let ops = w.LocalModifications |> Change.toList
        let inverse = w.LocalModificationsInverse |> Change.toList

        match ops with
            | [] -> 
                Log.warn "nothing to commit"
            | ops ->

                ops 
                    |> Seq.collect (fun o -> o.Data) 
                    |> Seq.iter (fun d ->
                        let hash = objHash d
                        w.Data.[hash] <- d
                    )

                let newHistory =
                    {
                        hash = Guid.NewGuid()
                        forward = ops
                        backward = inverse
                        message = message
                        author = Environment.UserName
                        date = DateTime.UtcNow
                        mergeSource = None
                        parent = w.History
                    }

                w.Commits.[newHistory.hash] <- newHistory
                w.History <- newHistory
                w.LocalModifications <- NoChange
                w.LocalModificationsInverse <- NoChange
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
                        match w.Branches.TryGetValue (remoteBranchName branch) with
                            | (true, target) ->  
                                let current = w.History

                                let anc = findCommonAncestor current target

                                transact (fun () ->
                                    backward w current anc
                                    forward w anc target
                                )

                                w.Branch <- branch
                                w.History <- target
                            | _ ->
                                w.Branches.[branch] <- w.History
                                w.Branch <- branch

            
            else
                Log.warn "cannot checkout branch %s because you have local changes" branch

    let apply (change : Change) (w : WorkingCopy) =
        let inverse = 
            transact (fun () ->
                change |> Change.run
            )
        w.LocalModificationsInverse <- Change.append w.LocalModificationsInverse inverse
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
                        let mergeRes, change = forwardMerge w conflictTable anc s

                        match mergeRes with
                            | Merged ->
                                let inverse = transact (fun () -> change |> Change.run)

                                let newHistory =
                                    {
                                        hash = Guid.NewGuid()
                                        forward = Change.toList change
                                        backward = Change.toList inverse
                                        message = sprintf "Merge branch %s into %s" source w.Branch
                                        author = Environment.UserName
                                        date = DateTime.UtcNow
                                        mergeSource = Some(source, s)
                                        parent = w.History
                                    }

                                w.Commits.[newHistory.hash] <- newHistory
                                w.History <- newHistory
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


    let pull (remote : string) (w : WorkingCopy) =
        w |> fetch remote

        if Change.isEmpty w.LocalModifications then

            let remName = remoteBranchName w.Branch

            match w.Branches.TryGetValue remName with
                | (true, rem) ->
                
                    let anc = findCommonAncestor rem w.History

                    if anc == w.History then
                        transact (fun () -> forward w anc rem)
                        w.History <- rem
                        w.Branches.[w.Branch] <- rem
                    else
                        merge remName w


                | _ ->
                    ()

        else
            Log.warn "you have local changes (cannot pull)"



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

        member x.pull remote =
            Git.pull remote x

        member x.push remote =
            Git.push false false remote x

        member x.pushAll remote =
            Git.push true false remote x


        member x.forcePushAll remote =
            Git.push true true remote x

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
                for o in c.forward do
                    Log.line "%A" o
                Log.stop()
                Log.stop()

                    
module Git2Dgml =
    open System.Text
    open System.IO
    open System.Collections.Generic
        
    let private properties = """
        <Properties>
        <Property Id="Background" Label="Background" DataType="Brush" />
        <Property Id="Label" Label="Label" DataType="String" />
        <Property Id="Size" DataType="String" />
        <Property Id="Start" DataType="DateTime" />
        </Properties>"""

    let rec genDgml (nodes : StringBuilder) (links : StringBuilder) (v : HashSet<_>) (h : Commit) =
        if Commit.isRoot h then ()
        else
            if v.Contains h then ()
            else
                v.Add h |> ignore
                nodes.AppendLine(sprintf "\t\t<Node Id=\"%A\" Label=\"%s\"/>" h.hash h.message) |> ignore
                links.AppendLine(sprintf "\t\t<Link Source=\"%A\" Target=\"%A\"/>" h.hash h.parent.hash) |> ignore
                genDgml nodes links v h.parent

    let visualizeHistory (fileName : string) (branches : list<string*Commit>) =
        let nodes = StringBuilder()
        let links = StringBuilder()
        let visited = HashSet()
        for (name,b) in branches do
            nodes.AppendLine(sprintf "\t\t<Node Id=\"%s\" Label=\"%s\"/>" name name) |> ignore
            links.AppendLine(sprintf "\t\t<Link Source=\"%s\" Target=\"%A\"/>" name b.hash) |> ignore
            genDgml nodes links visited b
        use file = new StreamWriter( fileName )
        file.WriteLine("<?xml version='1.0' encoding='utf-8'?>")
        file.WriteLine("<DirectedGraph xmlns=\"http://schemas.microsoft.com/vs/2009/dgml\">")
        file.WriteLine("<Nodes>")
        file.Write(nodes.ToString())
        file.WriteLine(@"</Nodes>")
        file.WriteLine("<Links>")
        file.Write(links.ToString())
        file.WriteLine("</Links>")
        file.WriteLine(properties)
        file.WriteLine("</DirectedGraph>")
        fileName

module GitTest =
    open System
    open System.IO
    let repo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.rep")
    
    let build () =
        let git = Git.init()
        let a = git.pmod "a" 1
        let b = git.pmod "b" 1

        let c = Mod.map2 (+) a b

        let print() =
            Log.line "%s: %A" git.Branch (Mod.force c)

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

        if not (Directory.Exists repo) then
            git.forcePushAll repo

    let read() =
        let git = Git.init()
        git.pull repo

        let a = git.pmod "a" 1
        let b = git.pmod "b" 1

        let c = Mod.map2 (+) a b


        let print() =
            Log.line "%s: %A" git.Branch (Mod.force c)

        print()

        git.checkout "merge"
        git.pull repo
        print()

        git.checkout "conflict"
        git.pull repo
        print()

        
        git.checkout "b5"
        git.pull repo
        print()

        git.pushAll repo

    let repo2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test2.rep")

    let build2() =

        let git = Git.init()

        let pm = git.pmod "abc" None

        git.apply (PMod.change pm (Some 1))
        git.commit "abc"

        pm |> Mod.force |> printfn "%A"

        git.forcePushAll repo2

    let read2() =
     
        let git = Git.init()

        let none : Option<int> = None
        let a = git.pmod "abc" none
        git.pull repo2

        printfn "%A" (a |> Mod.force)

    let run() =
        build2()
        read2()
//
//        build()
//        read()








        





