namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsCheck
open FsCheck.NUnit

type SimSetTree =
    | Changeable of obj
    | Constant of obj
    | Map of obj * SimSetTree
    | Choose of obj * SimSetTree
    | Filter of obj * SimSetTree
    | Collect of obj * SimSetTree

and simset<'a> =
    abstract member Content : hrefset<'a>
    abstract member ASet : aset<'a>
    abstract member Inputs : hset<obj>
    abstract member Expression : SimSetTree

and csimset<'a>(initial : seq<'a>) =
    static let mutable currentId = 0
    static let newId() =
        System.Threading.Interlocked.Increment(&currentId)

    let id = newId()
    let cset = cset initial
    let mutable content = HRefSet.ofSeq (HSet.ofSeq initial)

    member x.Name = "cset" + string id

    member x.Count = content.Count

    member x.Add (value : 'a) =
        cset.Add(value) |> ignore
        content <- content |> HRefSet.alter value (fun o -> max o 1)
        
    member x.Remove (value : 'a) =
        cset.Remove value |> ignore
        content <- content |> HRefSet.alter value (fun o -> 0)

    member x.Content = content

    override x.ToString() =
        let suffix =
            if cset.Count > 8 then "; ..."
            else ""

        let content = 
            cset |> Seq.truncate 8 |> Seq.map (sprintf "%A") |> String.concat "; "
        
        "cset" + string id + " [" + content + suffix + "]"

    interface simset<'a> with
        member x.Expression = Changeable x
        member x.Inputs = HSet.ofList [x :> obj]
        member x.Content = content
        member x.ASet = cset :> _

module SimSet =
    
    type SimSet<'a> = { expression : SimSetTree; inputs : unit -> hset<obj>; aset : aset<'a>; content : unit -> hrefset<'a> } with
        interface simset<'a> with
            member x.Expression = x.expression
            member x.Inputs = x.inputs()
            member x.Content = x.content()
            member x.ASet = x.aset
            

    let inline private simset a =
        a :> simset<_>


    let inline private aset (v : simset<'a>) = v.ASet
    let inline private content (v : simset<'a>) = v.Content

    let empty<'a> =
        simset { 
            expression = Constant HRefSet.empty 
            aset = ASet.empty
            inputs = fun () -> HSet.empty
            content = fun () -> HRefSet.empty 
        }

    let ofSeq (seq : seq<'a>) =
        let set = HRefSet.ofSeq (HSet.ofSeq seq)
        simset { 
            expression = Constant set
            aset = ASet.ofSeq seq
            inputs = fun () -> HSet.empty
            content = fun () -> set 
        }

    let ofList (list : list<'a>) =
        ofSeq list

    let ofArray (arr : 'a[]) =
        ofSeq arr

    let map (f : 'a -> 'b) (set : simset<'a>) =
        simset { 
            expression = Map(f, set.Expression)
            aset = ASet.map f set.ASet
            inputs = fun () -> set.Inputs
            content = fun () -> HRefSet.map f set.Content 
        }

    let choose (f : 'a -> Option<'b>) (set : simset<'a>) =
        simset { 
            expression = Choose(f, set.Expression)
            aset = ASet.choose f set.ASet
            inputs = fun () -> set.Inputs
            content = fun () -> HRefSet.choose f set.Content 
        }

    let filter (f : 'a -> bool) (set : simset<'a>) =
        simset { 
            expression = Filter(f, set.Expression)
            aset = ASet.filter f set.ASet
            inputs = fun () -> set.Inputs
            content = fun () -> HRefSet.filter f set.Content 
        }


    let collect (f : 'a -> simset<'b>) (set : simset<'a>) =
        simset { 
            expression = Collect(f, set.Expression)
            aset = ASet.collect (f >> aset) set.ASet
            inputs = fun () -> HSet.unionMany (set.Inputs :: (set.Content |> Seq.map (fun v -> f(v).Inputs) |> Seq.toList))
            content = fun () -> HRefSet.collect (f >> content) set.Content 
        }


type SimSetGenerator() =
    static let rand = RandomSystem()
    static member SimSet() =
        { new Arbitrary<simset<'a>>() with
            override x.Generator =
                gen {
                    let case = rand.UniformInt(9)

                    match case with
                        | 0 | 1 | 2 -> 
                            let! content = Arb.generate<list<'a>>
                            return csimset content :> simset<_>

                        | 3 | 4 | 5 ->
                            let! content = Arb.generate<list<'a>>
                            return SimSet.ofList content

                        | 6 ->
                            let! input = Arb.generate<simset<'a>>
                            let! f = Arb.generate<'a -> 'a>

                            return SimSet.map f input

                        | 7 ->
                            let! input = Arb.generate<simset<'a>>
                            let! f = Arb.generate<'a -> Option<'a>>

                            return SimSet.choose f input
                            
                        | 8 ->
                            let! input = Arb.generate<simset<'a>>
                            let! f = Arb.generate<'a -> simset<'a>>

                            let cache = Cache f
                            let f a = cache.Invoke a

                            return SimSet.collect f input


                        | _ ->
                            return failwith ""
                }
        }


module SimSetTest =
    open System.Collections.Generic
    let private rand = RandomSystem()

    type IExistential =
        abstract member Run : csimset<'a> -> unit

    let private runMeth = typeof<IExistential>.GetMethod "Run"

    let private existential (v : obj) (e : IExistential) : unit =
        let t = v.GetType()
        if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<csimset<_>> then
            let t = t.GetGenericArguments().[0]
            let meth = runMeth.MakeGenericMethod [|t|]
            meth.Invoke(e, [| v |]) |> ignore
        else
            failwith "bad type"

    let arbitraryChange (addprob : float) (setProb : float) (sim : simset<'a>) =
        let inputs = sim.Inputs

        let log = List<string>()

        transact (fun () ->
            for i in inputs do
                if rand.UniformDouble() < setProb then
                    existential i {
                        new IExistential with
                            member x.Run(set : csimset<'b>) =
                                if rand.UniformDouble() > addprob && set.Count > 0 then
                                    // remove
                                    let arr = set.Content |> Seq.toArray
                                    let element = arr.[rand.UniformInt(arr.Length)]

                                    log.Add(sprintf "%s.remove(%A)" set.Name element)

                                    set.Remove element
                                else
                                    // add
                                    let element = Arb.generate<'b> |> Gen.eval 10 (Random.StdGen(rand.UniformInt(), rand.UniformInt()))
                                    
                                    
                                    log.Add(sprintf "%s.add(%A)" set.Name element)
                                    set.Add element
                    }
            if log.Count > 0 then
                Log.start "change"
                for l in log do Log.line "%s" l
                Log.stop()
        )

    let checkEqual (is : hrefset<'a>) (should : hrefset<'a>) =
        let his = HashSet is
        let hshould = HashSet should
        if not (his.SetEquals hshould) then
            let isButNotShould = HRefSet.difference is should
            let shouldButNotIs = HRefSet.difference should is


            match HRefSet.isEmpty isButNotShould, HRefSet.isEmpty shouldButNotIs with
                | true, true ->
                    failwith "[ASet] PRefSet.difference produced empty sets"

                | false, true ->
                    failwithf "[ASet] values %A exist but should not." isButNotShould

                | true, false ->
                    failwithf "[ASet] values %A don't exist but should." shouldButNotIs

                | false, false ->
                    failwithf "[ASet] values %A exist but should not and %A don't but should" isButNotShould shouldButNotIs
                    
                    
            

            ()
    
    let checkEqualDelta (is : hdeltaset<'a>) (should : hdeltaset<'a>) =
        let his = HashSet is
        let hshould = HashSet should
        if not (his.SetEquals hshould) then
            failwithf "[ASet] got deltas %A but effective are %A" is should

    let check (reader : ISetReader<'a>) (set : simset<'a>) =
        let asetOldState = reader.State
        let asetOps = reader.GetOperations null
        let asetState = reader.State
        let simState = set.Content

        checkEqual asetState simState


        let asetStateClean, asetOpsClean = HRefSet.applyDelta asetOldState asetOps
        checkEqual asetStateClean simState

        checkEqualDelta asetOps asetOpsClean




        ()

    let validate (set : simset<'a>) =
        use reader = set.ASet.GetReader()

        check reader set

        for i in 1 .. 100 do
            arbitraryChange 0.5 0.3 set
            check reader set

    [<Test>]
    let ``[ASet] validation``() =
        Arb.register<SimSetGenerator>() |> ignore

        let generated = 
            Arb.generate<simset<int>> 
                |> Gen.arrayOfLength 100 
                |> Gen.eval 5 (Random.StdGen(rand.UniformInt(), rand.UniformInt()))

        let rec toString (e : SimSetTree) =
            match e with
                | Constant o -> string o
                | Changeable o -> string o
                | Map(f,o) -> sprintf "%s |> map f" (toString o)
                | Choose(f,o) -> sprintf "%s |> choose f" (toString o)
                | Filter(f,o) -> sprintf "%s |> filter f" (toString o)
                | Collect(f,o) -> sprintf "%s |> collect f" (toString o)

        for g in generated do
            let name = g.Expression |> toString
            Log.start "%s" name
            validate g
            Log.stop()
        ()
