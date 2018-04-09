namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open System.Collections.Generic

type SimListTree =
    | Changeable of IChangeable
    | Constant of obj
    | Map of obj * SimListTree
    | Choose of obj * SimListTree
    | Filter of obj * SimListTree
    | Collect of obj * SimListTree
    | OfMod of IDependent
    | Bind of obj * IDependent

and simlist<'a> =
    inherit IDependent
    abstract member Content : list<'a>
    abstract member AList : alist<'a>
    abstract member Expression : SimListTree

and csimlist<'a>(initial : seq<'a>) =
    static let mutable currentId = 0
    static let newId() =
        System.Threading.Interlocked.Increment(&currentId)

    let id = newId()
    let clist = clist initial
    let mutable content = List.ofSeq initial

    member x.Name = "clist" + string id

    member x.Count = content.Length

    member x.Add (value : 'a) =
        clist.Append(value) |> ignore
        content <- content @ [value]
        
    member x.RemoveAt (index : int) =
        clist.RemoveAt index |> ignore
        content <- content |> List.indexed |> List.choose (fun (i,v) -> if i = index then None else Some v)

    member x.Clear() =
        clist.Clear()
        content <- []

    member x.Content = content

    interface IDependent with
        member x.AsString = x.ToString()
        member x.Inputs = HSet.ofList [x :> IChangeable]
        
    interface IChangeable with
        member x.RandomChange(rand : RandomSystem, addprob : float) =
            gen {
                if rand.UniformDouble() > 0.95 then
                    x.Clear()
                    return sprintf "clear"
                elif x.Count > 0 && rand.UniformDouble() > addprob then
                    let v = rand.UniformInt(x.Count)
                    x.RemoveAt v |> ignore
                    return sprintf "removeAt(%A)" v
                else
                    let! v = Arb.generate<'a>
                    x.Add v |> ignore
                    return sprintf "add(%A)" v
            }
                

    override x.ToString() =
        let suffix =
            if clist.Count > 8 then "; ..."
            else ""

        let content = 
            clist |> Seq.truncate 8 |> Seq.map (sprintf "%A") |> String.concat "; "
        
        "clist" + string id + " [" + content + suffix + "]"

    interface simlist<'a> with
        member x.Expression = Changeable x
        member x.Content = content
        member x.AList = clist :> _

module SimList =
    
    type SimList<'a> = { expression : SimListTree; inputs : unit -> hset<IChangeable>; alist : alist<'a>; content : unit -> list<'a> } with
        interface simlist<'a> with
            member x.AsString =
                let rec toString (t : SimListTree) =
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
            member x.AList = x.alist
            

    let inline private simlist a =
        a :> simlist<_>


    let inline private alist (v : simlist<'a>) = v.AList
    let inline private content (v : simlist<'a>) = v.Content

    let empty<'a> =
        simlist { 
            expression = Constant HRefSet.empty 
            alist = AList.empty
            inputs = fun () -> HSet.empty
            content = fun () -> []
        }

    let ofSeq (seq : seq<'a>) =
        let content = List.ofSeq seq
        simlist {
            expression = Constant content
            alist = AList.ofSeq seq
            inputs = fun () -> HSet.empty
            content = fun () -> content 
        }

    let ofList (list : list<'a>) =
        ofSeq list

    let ofArray (arr : 'a[]) =
        ofSeq arr

    let map (f : 'a -> 'b) (list : simlist<'a>) =
        let f = Cache f
        simlist { 
            expression = Map(f, list.Expression)
            alist = AList.map f.Invoke list.AList
            inputs = fun () -> list.Inputs
            content = fun () -> List.map f.Invoke list.Content 
        }

    let choose (f : 'a -> Option<'b>) (list : simlist<'a>) =
        let f = Cache f
        simlist { 
            expression = Choose(f, list.Expression)
            alist = AList.choose f.Invoke list.AList
            inputs = fun () -> list.Inputs
            content = fun () -> List.choose f.Invoke list.Content 
        }

    let filter (f : 'a -> bool) (list : simlist<'a>) =
        let f = Cache f
        simlist { 
            expression = Filter(f, list.Expression)
            alist = AList.filter f.Invoke list.AList
            inputs = fun () -> list.Inputs
            content = fun () -> List.filter f.Invoke list.Content 
        }

    let collect (f : 'a -> simlist<'b>) (list : simlist<'a>) =
        let f = Cache f

        simlist { 
            expression = Collect(f, list.Expression)
            alist = AList.collect (f.Invoke >> alist) list.AList
            inputs = fun () -> HSet.unionMany (list.Inputs :: (list.Content |> Seq.map (fun v -> f.Invoke(v).Inputs) |> Seq.toList))
            content = fun () -> list.Content |> List.collect (f.Invoke >> content) 
        }

    let ofModSingle (m : simmod<'a>) =
        simlist {
            expression = OfMod m
            alist = AList.ofModSingle m.Mod
            inputs = fun () -> m.Inputs
            content = fun () -> [m.Value]
        }
        

    let bind (f : 'a -> simlist<'b>) (m : simmod<'a>) =
        let f = Cache f 
        simlist { 
            expression = Bind(f, m)
            alist = AList.bind (f.Invoke >> alist) m.Mod
            inputs = fun () -> HSet.union (m.Inputs) (f.Invoke(m.Value).Inputs)
            content = fun () -> (f.Invoke (m.Value)).Content
        }


    let private checkEqual (is : plist<'a>) (should : list<'a>) =
        if is.Count <> should.Length then
            Log.error "is: %A" (PList.toList is)
            Log.error "should: %A" should
            failwithf "[AList] count does not match: %d should be %d" is.Count should.Length

        let arr1 = is |> PList.toArray
        let arr2 = should |> List.toArray
        for i in 0..arr1.Length-1 do
            let a = arr1.[i]
            let b = arr2.[i]
            if a <> b then
                failwithf "[AList] elements at %d not equalt (%A <> %A)" i a b

        ()
    
    let private checkEqualDelta (is : pdeltalist<'a>) (should : pdeltalist<'a>) =
        //let his = HashSet is
        //let hshould = HashSet should
        //if not (his.SetEquals hshould) then
        //    failwithf "[ASet] got deltas %A but effective are %A" is should
        ()

    let validator (list : simlist<'a>) =
        let reader = list.AList.GetReader()

        { new IValidator with
            member x.Validate(log) = 
                try
                    let alistOldState = reader.State
                    let alistOps = reader.GetOperations(AdaptiveToken.Top)
                    let alistState = reader.State
                    let simState = list.Content

                    checkEqual alistState simState

                    //let asetOldState = HRefSet.ofSeq (HSet.ofSeq asetOldState)
                    //let asetStateClean, asetOpsClean = HRefSet.applyDelta asetOldState asetOps
                    //checkEqual asetStateClean simState

                    //checkEqualDelta asetOps asetOpsClean
                with e ->
                    Log.start "latest operations"
                    for (t,op) in log do
                        Log.line "%s.%s" t.AsString op
                    Log.stop()
                    reraise()

            member x.Release() =
                reader.Dispose()
        }

type SimListGenerator() =
    static let rand = RandomSystem(2)

    static member SimList() =
        { new Arbitrary<simlist<'a>>() with
            override x.Generator =
                gen {
                    let case = rand.UniformInt(10)

                    

                    match case with
                        | 0 | 1  -> 
                            let! content = Arb.generate<list<'a>>
                            return csimlist content :> simlist<_>

                        | 2 | 3 ->
                            let! content = Arb.generate<list<'a>>
                            return SimList.ofList content

                        | 4 ->
                            let! input = Arb.generate<simlist<'a>>
                            let! f = Arb.generate<'a -> 'a>
 
                            return SimList.map f input

                        | 5 ->
                            let! input = Arb.generate<simlist<'a>>
                            let! f = Arb.generate<'a -> Option<'a>>

                            return SimList.choose f input
                            
                        | 6 ->
                            let! input = Arb.generate<simlist<'a>>
                            let! f = Arb.generate<'a -> bool>

                            return SimList.filter f input
                        | 7 ->
                            let! input = Arb.generate<simlist<'a>>
                            let! f = Arb.generate<'a -> simlist<'a>>

                            let cache = Cache f
                            let f a = cache.Invoke a

                            return SimList.collect f input


                        | 8 ->
                            let! ref = Arb.generate<simmod<'a>>
                            return SimList.ofModSingle ref


                        | 9 ->
                            let! ref = Arb.generate<simmod<'a>>
                            let! f = Arb.generate<'a -> simlist<'a>>
                            let cache = Cache f
                            let f a = cache.Invoke a

                            return SimList.bind f ref

                        | _ ->
                            return failwith ""
                }
        }


module SimListTest =
    let private rand = RandomSystem()

    [<Test>]
    let ``[AList] validation``() =
        Arb.register<SimModGenerator>() |> ignore
        Arb.register<SimListGenerator>() |> ignore
        Dependent.validate<simlist<int>> 400 100 SimList.validator
            |> Gen.eval 5 (Random.StdGen(rand.UniformInt(), rand.UniformInt()))




