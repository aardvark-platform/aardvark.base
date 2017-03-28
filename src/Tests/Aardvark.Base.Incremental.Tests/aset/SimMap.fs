namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsCheck
open FsCheck.NUnit

type SimMapTree =
    | Changeable of IChangeable
    | Constant of obj
    | Map of obj * SimMapTree
    | Choose of obj * SimMapTree
    | Filter of obj * SimMapTree
    | MapM of obj * SimMapTree
    | ChooseM of obj * SimMapTree
    | UnionWith of obj * SimMapTree * SimMapTree
    | Choose2 of obj * SimMapTree * SimMapTree

and simmap<'k, 'v> =
    inherit IDependent
    abstract member Content : hmap<'k, 'v>
    abstract member AMap : amap<'k, 'v>
    abstract member Expression : SimMapTree

and csimmap<'k, 'v>(initial : seq<'k * 'v>) =
    static let mutable currentId = 0
    static let newId() =
        System.Threading.Interlocked.Increment(&currentId)

    let id = newId()
    let cmap = cmap initial
    let mutable content = HMap.ofSeq initial

    interface IDependent with
        member x.AsString = x.ToString()
        member x.Inputs = HSet.ofList [x :> IChangeable]

    interface IChangeable with
        member x.RandomChange(rand, addprob) =
            gen {
                if x.Count > 0 && rand.UniformDouble() > addprob then
                    let key,_ = content |> HMap.toSeq |> Seq.item (rand.UniformInt(x.Count))
                    if rand.UniformDouble() < 0.5 then
                        x.Remove key
                        return sprintf "remove(%A)" key
                    else
                        let! v = Arb.generate<'v>
                        x.Add(key, v)
                        return sprintf "update(%A, %A)" key v
                else
                    let! k = Arb.generate<'k>
                    let! v = Arb.generate<'v>
                    x.Add(k, v)
                    return sprintf "add(%A, %A)" k v
            }

    member x.Name = "cmap" + string id

    member x.Count = content.Count

    member x.Add (key : 'k, value : 'v) =
        cmap.[key] <- value
        content <- content |> HMap.add key value
        
    member x.Remove (key : 'k) =
        cmap.Remove key |> ignore
        content <- content |> HMap.remove key

    member x.Content = content

    override x.ToString() =
        let suffix =
            if content.Count > 8 then "; ..."
            else ""

        let content = 
            content |> Seq.truncate 8 |> Seq.map (sprintf "%A") |> String.concat "; "
        
        "cmap" + string id + " [" + content + suffix + "]"

    interface simmap<'k, 'v> with
        member x.Expression = Changeable x
        member x.Content = content
        member x.AMap = cmap :> _

module SimMap =
    open System.Collections.Generic

    type SimMap<'k, 'v> = { expression : SimMapTree; inputs : unit -> hset<IChangeable>; amap : amap<'k, 'v>; content : unit -> hmap<'k, 'v> } with
        interface simmap<'k, 'v> with
            member x.AsString =
                let rec toString (e : SimMapTree) =
                    match e with
                        | Constant o -> string o
                        | Changeable o -> o.AsString
                        | Map(f,o) -> sprintf "%s |> map f" (toString o)
                        | Choose(f,o) -> sprintf "%s |> choose f" (toString o)
                        | Filter(f,o) -> sprintf "%s |> filter f" (toString o)
                        | MapM(f,o) -> sprintf "%s |> mapM f" (toString o)
                        | ChooseM(f,o) -> sprintf "%s |> chooseM f" (toString o)
                        | UnionWith(f,l,r) -> sprintf "union(%s, %s)" (toString l) (toString r)
                        | Choose2(f,l,r) -> sprintf "choose2(f, %s, %s)" (toString l) (toString r)


                toString x.expression
            member x.Expression = x.expression
            member x.Inputs = x.inputs()
            member x.Content = x.content()
            member x.AMap = x.amap


    let inline private simmap a =
        a :> simmap<_,_>


    let inline private amap (v : simmap<'k, 'v>) = v.AMap
    let inline private content (v : simmap<'k, 'v>) = v.Content


    let empty<'k, 'v> =
        simmap { 
            expression = Constant HMap.empty<'k, 'v> 
            amap = AMap.empty<'k, 'v> 
            inputs = fun () -> HSet.empty
            content = fun () -> HMap.empty<'k, 'v> 
        }

    let ofSeq (seq : seq<'k * 'v>) =
        let map = HMap.ofSeq seq
        simmap {
            expression = Constant map
            amap = AMap.ofSeq seq
            inputs = fun () -> HSet.empty
            content = fun () -> map 
        }

    let ofList (list : list<'k * 'v>) =
        ofSeq list

    let ofArray (arr : array<'k * 'v>) =
        ofSeq arr

    let map (f : 'k -> 'a -> 'b) (map : simmap<'k, 'a>) =
        simmap { 
            expression = Map(f, map.Expression)
            amap = AMap.map f map.AMap
            inputs = fun () -> map.Inputs
            content = fun () -> HMap.map f map.Content
        }

    let choose (f : 'k -> 'a -> Option<'b>) (set : simmap<'k, 'a>) =
        simmap { 
            expression = Choose(f, set.Expression)
            amap = AMap.choose f set.AMap
            inputs = fun () -> set.Inputs
            content = fun () -> HMap.choose f set.Content 
        }

    let filter (f : 'k -> 'a -> bool) (set : simmap<'k, 'a>) =
        simmap { 
            expression = Filter(f, set.Expression)
            amap = AMap.filter f set.AMap
            inputs = fun () -> set.Inputs
            content = fun () -> HMap.filter f set.Content 
        }

    let mapM (f : 'k -> 'a -> simmod<'b>) (m : simmap<'k, 'a>) =
        let cached = Cache(fun (k,v) -> f k v)
        simmap { 
            expression = ChooseM(f, m.Expression)
            amap = AMap.mapM (fun k v -> cached.Invoke(k,v).Mod :> IMod<_>) m.AMap
            inputs = fun () -> 
                let inner = cached.Values |> Seq.map (fun v -> v.Inputs) |> HSet.unionMany
                HSet.union inner m.Inputs
            content = fun () -> HMap.map (fun k v -> cached.Invoke(k,v).Value) m.Content
        }

    let chooseM (f : 'k -> 'a -> simmod<Option<'b>>) (m : simmap<'k, 'a>) =
        let f = Cache(fun (k,v) -> f k v)
        
        simmap { 
            expression = ChooseM(f, m.Expression)
            amap = AMap.chooseM (fun k v -> f.Invoke(k,v).Mod :> IMod<_>) m.AMap
            inputs = fun () -> 
                let inner = f.Values |> Seq.map (fun v -> v.Inputs) |> HSet.unionMany
                HSet.union inner m.Inputs
            content = fun () -> 
                HMap.choose (fun k v -> f.Invoke(k,v).Value) m.Content
        }

    let unionWith (f : 'k -> 'a -> 'a -> 'a) (l : simmap<'k, 'a>) (r : simmap<'k, 'a>) =
        simmap { 
            expression = UnionWith(f, l.Expression, r.Expression)
            amap = AMap.unionWith f l.AMap r.AMap
            inputs = fun () -> HSet.union l.Inputs r.Inputs
            content = fun () -> HMap.unionWith f l.Content r.Content 
        }
    let choose2 (f : 'k -> Option<'a> -> Option<'a> -> Option<'a>) (l : simmap<'k, 'a>) (r : simmap<'k, 'a>) =
        simmap { 
            expression = Choose2(f, l.Expression, r.Expression)
            amap = AMap.choose2 f l.AMap r.AMap
            inputs = fun () -> HSet.union l.Inputs r.Inputs
            content = fun () -> HMap.choose2 f l.Content r.Content 
        }

    let private equal (l : hmap<'k, 'v>) (r : hmap<'k, 'v>) =
        let mutable l = l
        let mutable r = r

        let mutable equal = true

        for (lk, lv) in l do
            match HMap.tryRemove lk r with
                | Some(rv, rm) ->
                    r <- rm
                    if not (Unchecked.equals lv rv) then
                        equal <- false
                | None ->
                    equal <- false

        if HMap.count r > 0 then false
        else equal

    let validator (instance : simmap<'k, 'v>) =
        let reader = instance.AMap.GetReader()

        { new IValidator with
            member x.Validate(log) =
                let printLog() =
                    printfn "latest operations"
                    for (t,op) in log do
                        printfn "    %s.%s" t.AsString op

                let aold = reader.State
                let aops = reader.GetOperations()
                let anew = reader.State

                let snew = instance.Content

                if not (equal anew snew) then
                    printLog()
                    failwithf "[SimMap] invalid state: { is: %A; should: %A }" anew snew

                let cops = HMap.computeDelta aold anew
                if not (equal aops cops) then
                    printLog()
                    failwithf "[SimMap] invalid ops: { is: %A; should: %A }" aops cops
                    
                let _, eops = HMap.applyDelta aold aops
                if not (equal aops eops) then
                    printLog()
                    failwithf "[SimMap] invalid effective-ops: { is: %A; should: %A }" aops eops
                  
            member x.Release() =
                reader.Dispose()
        }
        
type SimMapGenerator() =
    static let rand = RandomSystem()

    static member SimMap() =
        { new Arbitrary<simmap<'k, 'v>>() with
            override x.Generator =
                gen {
                    let case = rand.UniformInt(11)
                    match case with
                        | 0 | 1  -> 
                            let! content = Arb.generate<list<'k * 'v>>
                            return csimmap content :> simmap<_,_>

                
                        | 2 | 3 ->
                            let! content = Arb.generate<list<'k * 'v>>
                            return SimMap.ofList content

                        | 4 ->
                            let! input = Arb.generate<simmap<'k, 'v>>
                            let! f = Arb.generate<'k -> 'v -> 'v>
                            return SimMap.map f input

                        | 5 ->
                            let! input = Arb.generate<simmap<'k, 'v>>
                            let! f = Arb.generate<'k -> 'v -> Option<'v>>
                            return SimMap.choose f input

                        | 6 ->
                            let! input = Arb.generate<simmap<'k, 'v>>
                            let! f = Arb.generate<'k -> 'v -> bool>
                            return SimMap.filter f input

                        | 7 ->
                            let! input = Arb.generate<simmap<'k, 'v>>
                            let! f = Arb.generate<'k -> 'v -> simmod<'v>>
                            return SimMap.mapM f input
                            
                        | 8 ->
                            let! input = Arb.generate<simmap<'k, 'v>>
                            let! f = Arb.generate<'k -> 'v -> simmod<Option<'v>>>
                            return SimMap.chooseM f input

                        | 9 ->
                            let! l = Arb.generate<simmap<'k, 'v>>
                            let! r = Arb.generate<simmap<'k, 'v>>
                            let! f = Arb.generate<'k -> 'v -> 'v -> 'v>
                            return SimMap.unionWith f l r 

                        | 10 ->
                            let! l = Arb.generate<simmap<'k, 'v>>
                            let! r = Arb.generate<simmap<'k, 'v>>
                            let! f = Arb.generate<'k -> Option<'v> -> Option<'v> -> Option<'v>>
                            return SimMap.choose2 f l r 
                            
                        | _ ->
                            return failwith ""
                            

                }
        }



module ``AMap Validation`` =
    let rand = RandomSystem()

    [<Test>]
    let ``[AMap] validation``() =
        Arb.register<SimModGenerator>() |> ignore
        Arb.register<SimMapGenerator>() |> ignore

        let dep = Dependent.validate<simmap<int, int>> 400 100 SimMap.validator

        dep |> Gen.eval 5 (Random.StdGen(rand.UniformInt(), rand.UniformInt()))
        ()

