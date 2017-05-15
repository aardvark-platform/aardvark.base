namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open System.Collections.Generic

type SimSetTree =
    | Changeable of IChangeable
    | Constant of obj
    | Map of obj * SimSetTree
    | Choose of obj * SimSetTree
    | Filter of obj * SimSetTree
    | Collect of obj * SimSetTree
    | OfMod of IDependent
    | Bind of obj * IDependent

and simset<'a> =
    inherit IDependent
    abstract member Content : hset<'a>
    abstract member ASet : aset<'a>
    abstract member Expression : SimSetTree

and csimset<'a>(initial : seq<'a>) =
    static let mutable currentId = 0
    static let newId() =
        System.Threading.Interlocked.Increment(&currentId)

    let id = newId()
    let cset = cset initial
    let mutable content = HSet.ofSeq initial

    member x.Name = "cset" + string id

    member x.Count = content.Count

    member x.Add (value : 'a) =
        cset.Add(value) |> ignore
        content <- content |> HSet.add value
        
    member x.Remove (value : 'a) =
        cset.Remove value |> ignore
        content <- content |> HSet.remove value

    member x.Content = content

    interface IDependent with
        member x.AsString = x.ToString()
        member x.Inputs = HSet.ofList [x :> IChangeable]
        
    interface IChangeable with
        member x.RandomChange(rand : RandomSystem, addprob : float) =
            gen {
                if x.Count > 0 && rand.UniformDouble() > addprob then
                    let v = content |> Seq.item (rand.UniformInt(x.Count))
                    x.Remove v |> ignore
                    return sprintf "remove(%A)" v
                else
                    let! v = Arb.generate<'a>
                    x.Add v |> ignore
                    return sprintf "add(%A)" v
            }
                

    override x.ToString() =
        let suffix =
            if cset.Count > 8 then "; ..."
            else ""

        let content = 
            cset |> Seq.truncate 8 |> Seq.map (sprintf "%A") |> String.concat "; "
        
        "cset" + string id + " [" + content + suffix + "]"

    interface simset<'a> with
        member x.Expression = Changeable x
        member x.Content = content
        member x.ASet = cset :> _

module SimSet =
    
    type SimSet<'a> = { expression : SimSetTree; inputs : unit -> hset<IChangeable>; aset : aset<'a>; content : unit -> hset<'a> } with
        interface simset<'a> with
            member x.AsString =
                let rec toString (t : SimSetTree) =
                    match t with
                        | Changeable set -> set.AsString
                        | Constant set -> string set
                        | Map (_,t) -> sprintf "%s |> map f" (toString t)
                        | Choose (_,t) -> sprintf "%s |> choose f" (toString t)
                        | Filter (_,t) -> sprintf "%s |> filter f" (toString t)
                        | Collect (_,t) -> sprintf "%s |> collect f" (toString t)
                        | OfMod(m) -> sprintf "%s |> ofModSingle" m.AsString
                        | Bind(f, m) -> sprintf "%s |> bind f" m.AsString
                toString x.expression

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
            content = fun () -> HSet.empty 
        }

    let ofSeq (seq : seq<'a>) =
        let set = HSet.ofSeq seq
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
            content = fun () -> HSet.map f set.Content 
        }

    let choose (f : 'a -> Option<'b>) (set : simset<'a>) =
        simset { 
            expression = Choose(f, set.Expression)
            aset = ASet.choose f set.ASet
            inputs = fun () -> set.Inputs
            content = fun () -> HSet.choose f set.Content 
        }

    let filter (f : 'a -> bool) (set : simset<'a>) =
        simset { 
            expression = Filter(f, set.Expression)
            aset = ASet.filter f set.ASet
            inputs = fun () -> set.Inputs
            content = fun () -> HSet.filter f set.Content 
        }

    let collect (f : 'a -> simset<'b>) (set : simset<'a>) =
        let f = Cache f
        simset { 
            expression = Collect(f, set.Expression)
            aset = ASet.collect (f.Invoke >> aset) set.ASet
            inputs = fun () -> HSet.unionMany (set.Inputs :: (set.Content |> Seq.map (fun v -> f.Invoke(v).Inputs) |> Seq.toList))
            content = fun () -> HSet.collect (f.Invoke >> content) set.Content 
        }

    let ofModSingle (m : simmod<'a>) =
        simset {
            expression = OfMod m
            aset = ASet.ofModSingle m.Mod
            inputs = fun () -> m.Inputs
            content = fun () -> HSet.ofList [m.Value]
        }
        

    let bind (f : 'a -> simset<'b>) (m : simmod<'a>) =
        let f = Cache f
        simset { 
            expression = Bind(f, m)
            aset = ASet.bind (f.Invoke >> aset) m.Mod
            inputs = fun () -> HSet.union (m.Inputs) (f.Invoke(m.Value).Inputs)
            content = fun () -> HSet.collect (f.Invoke >> content) (HSet.ofList ([m.Value]))
        }


    let private checkEqual (is : hrefset<'a>) (should : hset<'a>) =
        let his = HashSet is
        let hshould = HashSet should
        if not (his.SetEquals hshould) then
            let isButNotShould = HSet.difference (HSet.ofSeq is) should
            let shouldButNotIs = HSet.difference should (HSet.ofSeq is)


            match HSet.isEmpty isButNotShould, HSet.isEmpty shouldButNotIs with
                | true, true ->
                    failwith "[ASet] PRefSet.difference produced empty sets"

                | false, true ->
                    failwithf "[ASet] values %A exist but should not." isButNotShould

                | true, false ->
                    failwithf "[ASet] values %A don't exist but should." shouldButNotIs

                | false, false ->
                    failwithf "[ASet] values %A exist but should not and %A don't but should" isButNotShould shouldButNotIs
                    
                    
            

            ()
    
    let private checkEqualDelta (is : hdeltaset<'a>) (should : hdeltaset<'a>) =
        let his = HashSet is
        let hshould = HashSet should
        if not (his.SetEquals hshould) then
            failwithf "[ASet] got deltas %A but effective are %A" is should

    let validator (set : simset<'a>) =
        let reader = set.ASet.GetReader()

        { new IValidator with
            member x.Validate(log) = 
                try
                    let asetOldState = reader.State
                    let asetOps = reader.GetOperations(AdaptiveToken.Top)
                    let asetState = reader.State
                    let simState = set.Content

                    checkEqual asetState simState

                    let asetOldState = HRefSet.ofSeq (HSet.ofSeq asetOldState)
                    let asetStateClean, asetOpsClean = HRefSet.applyDelta asetOldState asetOps
                    checkEqual asetStateClean simState

                    checkEqualDelta asetOps asetOpsClean
                with e ->
                    Log.start "latest operations"
                    for (t,op) in log do
                        Log.line "%s.%s" t.AsString op
                    Log.stop()
                    reraise()

            member x.Release() =
                reader.Dispose()
        }

type SimSetGenerator() =
    static let rand = RandomSystem()

    static member SimSet() =
        { new Arbitrary<simset<'a>>() with
            override x.Generator =
                gen {
                    let case = rand.UniformInt(10)

                    

                    match case with
                        | 0 | 1  -> 
                            let! content = Arb.generate<list<'a>>
                            return csimset content :> simset<_>

                        | 2 | 3 ->
                            let! content = Arb.generate<list<'a>>
                            return SimSet.ofList content

                        | 4 ->
                            let! input = Arb.generate<simset<'a>>
                            let! f = Arb.generate<'a -> 'a>
 
                            return SimSet.map f input

                        | 5 ->
                            let! input = Arb.generate<simset<'a>>
                            let! f = Arb.generate<'a -> Option<'a>>

                            return SimSet.choose f input
                            
                        | 6 ->
                            let! input = Arb.generate<simset<'a>>
                            let! f = Arb.generate<'a -> bool>

                            return SimSet.filter f input
                        | 7 ->
                            let! input = Arb.generate<simset<'a>>
                            let! f = Arb.generate<'a -> simset<'a>>

                            let cache = Cache f
                            let f a = cache.Invoke a

                            return SimSet.collect f input


                        | 8 ->
                            let! ref = Arb.generate<simmod<'a>>
                            return SimSet.ofModSingle ref


                        | 9 ->
                            let! ref = Arb.generate<simmod<'a>>
                            let! f = Arb.generate<'a -> simset<'a>>
                            let cache = Cache f
                            let f a = cache.Invoke a

                            return SimSet.bind f ref

                        | _ ->
                            return failwith ""
                }
        }


module SimSetTest =
    let private rand = RandomSystem()

    [<Test>]
    let ``[ASet] validation``() =
        Arb.register<SimModGenerator>() |> ignore
        Arb.register<SimSetGenerator>() |> ignore
        Dependent.validate<simset<int>> 400 100 SimSet.validator
            |> Gen.eval 5 (Random.StdGen(rand.UniformInt(), rand.UniformInt()))




