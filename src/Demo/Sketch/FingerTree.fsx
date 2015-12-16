#I @"..\..\..\bin\Release"
#I @"..\..\..\Packages\Rx-Core\lib\net45"
#I @"..\..\..\Packages\Rx-Interfaces\lib\net45"
#I @"..\..\..\Packages\Rx-Linq\lib\net45"
#r "Aardvark.Base.dll"
#r "Aardvark.Base.Essentials.dll"
#r "Aardvark.Base.TypeProviders.dll"
#r "Aardvark.Base.FSharp.dll"
#r "Aardvark.Base.Incremental.dll"
#r "System.Reactive.Core.dll"
#r "System.Reactive.Interfaces.dll"
#r "System.Reactive.Linq.dll"



open System
open System.Runtime.CompilerServices
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental

type Measure<'a, 'm> = { f : 'a -> 'm; add : 'm -> 'm -> 'm; zero : 'm}


[<AutoOpen>]
module private FingerTreeNode =


    type Affix<'a, 'm> =
        | One of 'm * 'a
        | Two of 'm * 'a * 'a
        | Three of 'm * 'a * 'a * 'a
        | Four of 'm * 'a * 'a * 'a * 'a

    type Node<'a, 'm> =
        | Node2 of 'm * 'a * 'a
        | Node3 of 'm * 'a * 'a * 'a

    type Measure<'a, 'm> with
        member x.one (v : 'a) = One(x.f v, v)
        member x.two (a : 'a) (b : 'a) = Two(x.add (x.f a) (x.f b), a,b)
        member x.three (a : 'a) (b : 'a) (c : 'a) = Three(x.add (x.add (x.f a) (x.f b)) (x.f c), a,b,c)
        member x.four (a : 'a) (b : 'a) (c : 'a) (d : 'a) = Four(x.add (x.add (x.f a) (x.f b)) (x.add (x.f c) (x.f d)), a,b,c,d)

        member x.node2 (a : 'a) (b : 'a) = Node2(x.add (x.f a) (x.f b), a,b)
        member x.node3 (a : 'a) (b : 'a) (c : 'a) = Node3(x.add (x.add (x.f a) (x.f b)) (x.f c), a,b,c)

        member x.compute ([<ParamArray>] values : 'a[]) =
            values |> Array.fold (fun s e -> x.add s (x.f e)) x.zero

        member x.NodeMeasure =
            let measureNode (n : Node<'a, 'm>) =
                match n with
                    | Node2(m,_,_) -> m
                    | Node3(m,_,_,_) -> m

            { f = measureNode; add = x.add; zero = x.zero }

    module Affix =
        let single (ctx : Measure<'a, 'm>) (v : 'a) =
            One(ctx.f v ,v)

        let ofList (ctx : Measure<'a, 'm>) (l : list<'a>) =
            match l with
                | [a] -> One(ctx.compute(a), a)
                | [a;b] -> Two(ctx.compute(a,b), a, b)
                | [a;b;c] -> Three(ctx.compute(a,b,c), a, b, c)
                | [a;b;c;d] -> Four(ctx.compute(a,b,c,d), a, b, c, d)
                | _ -> failwith "affix must have length 1 to 4"


        let prepend (ctx : Measure<'a, 'm>) (v : 'a) (a : Affix<'a, 'm>) =
            match a with
                | One(m, x) -> Two(ctx.add (ctx.f v) m,v,x)
                | Two(m,x,y) -> Three(ctx.add (ctx.f v) m,v,x,y)
                | Three(m,x,y,z) -> Four(ctx.add (ctx.f v) m,v,x,y,z)
                | _ -> failwith "affix must have length 1 to 4"

        let append (ctx : Measure<'a, 'm>) (v : 'a) (a : Affix<'a, 'm>) =
            match a with
                | One(m, x) -> Two(ctx.add (ctx.f v) m,x,v)
                | Two(m,x,y) -> Three(ctx.add (ctx.f v) m,x,y,v)
                | Three(m,x,y,z) -> Four(ctx.add (ctx.f v) m,x,y,z,v)
                | _ -> failwith "affix must have length 1 to 4"

        let toNode(a : Affix<'a,'m>) =
            match a with
                | Two(m,a,b) -> Node2(m, a,b)
                | Three(m, a,b,c) -> Node3(m, a,b,c)
                | _ -> failwith "nodes must have length 2 or 3"

        let ofNode(n : Node<'a, 'm>) =
            match n with
                | Node2(m,a,b) -> Two(m,a,b)
                | Node3(m,a,b,c) -> Three(m,a,b,c)

        let viewl (a : Affix<'a, 'm>) =
            match a with
                | One(_,a) -> [a]
                | Two(_,a,b) -> [a;b]
                | Three(_,a,b,c) -> [a;b;c]
                | Four(_,a,b,c,d) -> [a;b;c;d]

        let viewr (a : Affix<'a, 'm>) =
            match a with
                | One(_,a) -> [a]
                | Two(_,a,b) -> [b;a]
                | Three(_,a,b,c) -> [c;b;a]
                | Four(_,a,b,c,d) -> [d;c;b;a]

        let size (a : Affix<'a, 'm>) =
            match a with
                | One(m,_) -> m
                | Two(m,_,_) -> m
                | Three(m,_,_,_) -> m
                | Four(m,_,_,_,_) -> m

        let first (a : Affix<'a, 'm>) =
            match a with
                | One(_,a) -> a
                | Two(_,a,_) -> a
                | Three(_,a,_,_) -> a
                | Four(_,a,_,_,_) -> a
  
        let last (a : Affix<'a, 'm>) =
            match a with
                | One(_,a) -> a
                | Two(_,_,a) -> a
                | Three(_,_,_,a) -> a
                | Four(_,_,_,_,a) -> a
  
    module Node =
        let toAffix(n : Node<'a,'m>) = Affix.ofNode n

        let fromAffix(a : Affix<'a, 'm>) = Affix.toNode a

        let ofList (ctx : Measure<'a, 'm>) (l : list<'a>) =
            match l with
                | [a;b] -> Node2(ctx.compute(a,b), a, b)
                | [a;b;c] -> Node3(ctx.compute(a,b,c), a, b, c)
                | _ -> failwith "nodes must have 2-3 elements"

        let size (n : Node<'a, 'm>) =
            match n with
                | Node2(m,_,_) -> m
                | Node3(m,_,_,_) -> m

        let viewl (a : Node<'a, 'm>) =
            match a with
                | Node2(_,a,b) -> [a;b]
                | Node3(_,a,b,c) -> [a;b;c]

        let viewr (a : Node<'a, 'm>) =
            match a with
                | Node2(_,a,b) -> [b;a]
                | Node3(_,a,b,c) -> [c;b;a]

    type Deep<'a, 'm> = { annotation : 'm; prefix : Affix<'a, 'm>; deeper : FingerTreeNode<Node<'a, 'm>, 'm>; suffix : Affix<'a, 'm> }

    and FingerTreeNode<'a, 'm> = 
        | Empty 
        | Single of 'a 
        | Deep of Deep<'a, 'm> with

            member x.ViewLeft : seq<'a> =
                seq {
                    match x with
                        | Empty -> ()
                        | Single (a) -> yield a
                        | Deep { prefix = prefix; deeper = deeper; suffix = suffix } ->
                            yield! Affix.viewl prefix
                            for b in deeper.ViewLeft do
                                yield! Node.viewl b
                            yield! Affix.viewl suffix
                }

            member x.ViewRight : seq<'a> =
                seq {
                    match x with
                        | Empty -> ()
                        | Single (a) -> yield a
                        | Deep { prefix = prefix; deeper = deeper; suffix = suffix } ->
                            yield! Affix.viewr suffix
                            for b in deeper.ViewRight do
                                yield! Node.viewr b
                            yield! Affix.viewr prefix
                }




    let rec prepend<'a, 'm> (ctx : Measure<'a, 'm>) (value : 'a) (node : FingerTreeNode<'a, 'm>) : FingerTreeNode<'a, 'm> =
        match node with
            | Empty -> 
                Single value

            | Single y -> 
                Deep { 
                    annotation = ctx.compute(value,y)
                    prefix = ctx.one value
                    deeper = Empty
                    suffix = ctx.one y
                }

            | Deep { annotation = annotation; prefix = Four(_,a,b,c,d); deeper = deeper; suffix = suffix } ->
                Deep { 
                    annotation = ctx.add annotation (ctx.f value) 
                    prefix = ctx.two value a
                    deeper = prepend ctx.NodeMeasure (ctx.node3 b c d) deeper
                    suffix = suffix 
                }

            | Deep deep ->

                Deep { 
                    annotation = ctx.add (ctx.f value) deep.annotation
                    prefix = Affix.prepend ctx value deep.prefix
                    deeper = deep.deeper 
                    suffix = deep.suffix 
                }

    let rec append<'a, 'm> (ctx : Measure<'a, 'm>) (value : 'a) (node : FingerTreeNode<'a, 'm>) : FingerTreeNode<'a, 'm> =
        match node with
            | Empty -> 
                Single value

            | Single y -> 
                Deep { 
                    annotation = ctx.compute(value,y)
                    prefix = ctx.one y
                    deeper = Empty
                    suffix = ctx.one value
                }

            | Deep { annotation = annotation; prefix = prefix; deeper = deeper; suffix = Four(_,a,b,c,d) } ->
                Deep { 
                    annotation = ctx.add annotation (ctx.f value) 
                    prefix = prefix
                    deeper = append ctx.NodeMeasure (ctx.node3 a b c) deeper
                    suffix = ctx.two d value 
                }

            | Deep deep ->
                Deep { 
                    annotation = ctx.add (ctx.f value) deep.annotation
                    prefix = deep.prefix
                    deeper = deep.deeper 
                    suffix = Affix.append ctx value deep.suffix
                }


    type View<'a, 'm> =
        | Nil
        | Cons of 'a * FingerTreeNode<'a, 'm>

    let private affixToTree (ctx : Measure<'a, 'm>) (a : Affix<'a, 'm>) =
        match a with
            | One(_,x) -> 
                Single x
            | Two(m,x,y) -> 
                Deep { annotation = m; prefix = One(ctx.compute(x), x); deeper = Empty; suffix = One(ctx.compute(y), y) }
            | Three(m,x,y,z) ->
                Deep { annotation = m; prefix = One(ctx.compute(x), x); deeper = Empty; suffix = Two(ctx.compute(y,z), y,z) }
            | Four(m,x,y,z,w) ->
                Deep { annotation = m; prefix = Two(ctx.compute(x,y), x,y); deeper = Empty; suffix = Two(ctx.compute(z,w), z,w) }

    let rec viewl<'a, 'm> (ctx : Measure<'a,'m>) (node : FingerTreeNode<'a, 'm>) : View<'a, 'm> =
        match node with
            | Empty -> Nil
            | Single x -> Cons(x, Empty)
            | Deep ({ prefix = One(_,x) } as deep) ->
                match viewl ctx.NodeMeasure deep.deeper with
                    | Cons(node, rest') ->
                        let restMeasure =
                            match rest' with
                                | Empty -> ctx.zero
                                | Single v -> ctx.NodeMeasure.compute(v)
                                | Deep d -> d.annotation

                        let pref = Affix.ofNode node
                        let annot = ctx.add (ctx.add (Affix.size pref) restMeasure) (Affix.size deep.suffix)

                        let rest = Deep { annotation = annot; prefix = pref; deeper = rest'; suffix = deep.suffix }
                        Cons(x, rest)
                    | _ ->
                        Cons(x, affixToTree ctx deep.suffix)
            | Deep deep ->
                match Affix.viewl deep.prefix with
                    | (x::xs) ->
                        let newPrefix = Affix.ofList ctx xs

                        let prefixMeasure = Affix.size newPrefix
                        let suffixMeasure = Affix.size deep.suffix
                        let deeperMeasure =
                            match deep.deeper with
                                | Empty -> ctx.zero
                                | Single v -> ctx.NodeMeasure.compute(v)
                                | Deep d -> d.annotation

                        let sum = ctx.add (ctx.add prefixMeasure deeperMeasure) suffixMeasure

                        Cons(x, Deep { annotation = sum; prefix = newPrefix; deeper = deep.deeper; suffix = deep.suffix })
                    | [] ->
                        failwith "affixes cannot be empty"

    let rec viewr<'a, 'm> (ctx : Measure<'a,'m>) (node : FingerTreeNode<'a, 'm>) : View<'a, 'm> =
        match node with
            | Empty -> Nil
            | Single v -> Cons(v, Empty)
            | Deep ({ suffix = One(_,x) } as deep) ->
                match viewr ctx.NodeMeasure deep.deeper with
                    | Cons(n, rest') ->
                        let restMeasure =
                            match rest' with
                                | Empty -> ctx.zero
                                | Single v -> ctx.NodeMeasure.compute(v)
                                | Deep d -> d.annotation

                        let suff = Affix.ofNode n
                        let annot = ctx.add (ctx.add (Affix.size deep.prefix) restMeasure) (Affix.size suff)

                        let rest = Deep { annotation = annot; prefix = deep.prefix; deeper = rest'; suffix = suff }
                        Cons(x, rest)

                    | Nil -> 
                        Cons(x, affixToTree ctx deep.prefix)

            | Deep deep -> 
                match Affix.viewr deep.suffix with
                    | (x::xs) ->
                        let newSuffix = Affix.ofList ctx (List.rev xs)

                        let prefixMeasure = Affix.size deep.prefix
                        let suffixMeasure = Affix.size newSuffix
                        let deeperMeasure =
                            match deep.deeper with
                                | Empty -> ctx.zero
                                | Single v -> ctx.NodeMeasure.compute(v)
                                | Deep d -> d.annotation

                        let sum = ctx.add (ctx.add prefixMeasure deeperMeasure) suffixMeasure

                        Cons(x, Deep { annotation = sum; prefix = deep.prefix; deeper = deep.deeper; suffix = newSuffix })
                    | [] ->
                        failwith "affixes cannot be empty"



    type Split<'a, 'm> = 
        | Split of FingerTreeNode<'a, 'm> * 'a * FingerTreeNode<'a, 'm>
        | NoSplit

    let rec private splitList (ctx : Measure<'a, 'm>) (cond : 'm -> bool) (start : 'm) (list : list<'a>) =
        match list with
            | [] -> [], []
            | x::xs ->
                let startNew = ctx.add start (ctx.f x)
                if cond startNew then
                    [], list
                else
                    let before, after = splitList ctx cond startNew xs
                    (x::before, after)


    let rec private deep<'a, 'm> (ctx : Measure<'a, 'm>) prefix (deeper : FingerTreeNode<Node<'a, 'm>, 'm>) suffix =
        match prefix, suffix with
            | [], [] -> 
                match viewl ctx.NodeMeasure deeper with
                    | Nil -> Empty
                    | Cons(x,rest) -> deep ctx (Node.viewl x) rest []

            | [], _ ->
                match viewr ctx.NodeMeasure deeper with
                    | Nil -> suffix |> Affix.ofList ctx |> affixToTree ctx
                    | Cons(node, deeper') -> deep ctx (Node.viewl node) deeper' suffix

            | _, [] ->
                match viewr ctx.NodeMeasure deeper with
                    | Nil -> prefix |> Affix.ofList ctx |> affixToTree ctx
                    | Cons(node, deeper') -> deep ctx prefix deeper' (Node.viewl node)
            | _ ->
                let pref = Affix.ofList ctx prefix
                let suff = Affix.ofList ctx suffix

                let deeperMeasure =
                    match deeper with
                        | Empty -> ctx.zero
                        | Single v -> Node.size v
                        | Deep d -> d.annotation

                let sum = ctx.add (ctx.add (Affix.size pref) (Affix.size suff)) deeperMeasure

                Deep { annotation = sum; prefix = pref; deeper = deeper; suffix = suff }

    let rec split<'a, 'm> (ctx : Measure<'a, 'm>) (cond : 'm -> bool) (start : 'm) (node : FingerTreeNode<'a, 'm>) : Split<'a, 'm> =
        match node with
            | Empty -> NoSplit
            | Single x ->
                if cond (ctx.add start (ctx.compute(x))) then Split(Empty, x, Empty)
                else NoSplit
            | Deep { annotation = total; prefix = pref; deeper = deeper; suffix = suff } ->
               
                let inside = cond (ctx.add start total)
                if not inside then
                    NoSplit
                else
                    let prefix = Affix.viewl pref
                    let suffix = Affix.viewl suff
                    let startPref = ctx.add start (Affix.size pref)
                    
                    let chunkToTree l =
                        match l with
                            | [] -> Empty
                            | xs -> xs |> Affix.ofList ctx |> affixToTree ctx

                    if cond startPref then
                        match splitList ctx cond start (Affix.viewl pref) with
                            | (before, x::after) ->
                                Split (chunkToTree before, x, deep ctx after deeper suffix)
                            | _ ->
                                failwith "not possible"

                    else
                        let deeperMeasure =
                            match deeper with
                                | Empty -> ctx.zero
                                | Single v -> ctx.NodeMeasure.compute v
                                | Deep d -> d.annotation

                        let startSuff = ctx.add startPref deeperMeasure

                        if cond startSuff then
                            // inside deeper
                            match split ctx.NodeMeasure cond startPref deeper with
                                | Split(before, node, after) ->
                                    
                                    let beforeMeasure =
                                        match before with
                                            | Empty -> ctx.zero
                                            | Single v -> ctx.NodeMeasure.compute v
                                            | Deep d -> d.annotation

                                    let start' = ctx.add (ctx.add start (Affix.size pref)) beforeMeasure

                                    match splitList ctx cond start' (Node.viewl node) with
                                        | (beforeNode, x::afterNode) ->
                                            Split(deep ctx prefix before beforeNode, x, deep ctx afterNode after suffix)
                                        | _ ->
                                            failwith "not possible"

                                | NoSplit ->
                                    NoSplit

                        else
                            // in suffix
                            match splitList ctx cond startSuff suffix with
                                | (before, x::after) ->
                                    Split(deep ctx prefix deeper before, x, chunkToTree after)
                                | _ ->
                                    failwith "not possible" 

    let rec concatWithMiddle<'a, 'm> (ctx : Measure<'a, 'm>) (l : FingerTreeNode<'a, 'm>) (middle : list<'a>) (r : FingerTreeNode<'a, 'm>) : FingerTreeNode<'a, 'm> =
        
        let rec allButLast (l : list<'a>) =
            match l with
                | [_] -> []
                | x::xs -> x::allButLast xs
                | [] -> failwith "empty list"

        let rec nodes (l : list<'a>) : list<Node<'a, 'm>> =
            match l with
                | [] | [_] -> failwith "not enough elements for nodes"
                | [x;y] -> [Node2(ctx.compute(x,y), x, y)]
                | [x;y;z] -> [Node3(ctx.compute(x,y,z), x, y, z)]
                | x::y::rest -> (ctx.node2 x y)::nodes rest

        match l, middle, r with
            | Empty,        [],         r           -> r
            | Empty,        (x::xs),    r           -> prepend ctx x (concatWithMiddle ctx Empty xs r)
            | Single y,     xs,         r           -> prepend ctx y (concatWithMiddle ctx Empty xs r)

            | l,            [],         Empty       -> l
            | l,            xs,         Empty       -> append ctx (List.last xs) (concatWithMiddle ctx l (allButLast xs) Empty)
            | l,            xs,         Single y    -> append ctx y (concatWithMiddle ctx l xs Empty)

            | Deep l,       m,          Deep r      ->
                let mid' = List.concat [l.suffix |> Affix.viewl; m; r.prefix |> Affix.viewl] |> nodes
                let deeper' = concatWithMiddle ctx.NodeMeasure l.deeper mid' r.deeper


                let prefixMeasure = Affix.size l.prefix
                let suffixMeasure = Affix.size r.suffix
                let deeperMeasure =
                    match deeper' with
                        | Empty -> ctx.zero
                        | Single n -> ctx.NodeMeasure.compute n
                        | Deep d -> d.annotation

                let sum = ctx.add (ctx.add prefixMeasure deeperMeasure) suffixMeasure

                Deep { annotation = sum; prefix = l.prefix; deeper = deeper'; suffix = r.suffix }
                
[<StructuredFormatDisplay("{AsString}")>]
type FingerTree<'a, 'm> = private { ctx : Measure<'a, 'm>; root : FingerTreeNode<'a, 'm> } with

    member x.ViewLeft = x.root.ViewLeft
    member x.ViewRight = x.root.ViewRight

    member private x.AsString =
        x.ViewLeft |> Seq.toList |> sprintf "F %A"

    interface IEnumerable<'a> with
        member x.GetEnumerator() = x.ViewLeft.GetEnumerator()

    interface IEnumerable with
        member x.GetEnumerator() = (x.ViewLeft :> IEnumerable).GetEnumerator()

type Split<'a, 'm> =
    | Split of FingerTree<'a, 'm> * 'a * FingerTree<'a, 'm>
    | NoSplit

module FingerTree =
    
    let custom<'a, 'm> (m : Measure<'a, 'm>) = 
        { ctx = m; root = Empty }


    let total (t : FingerTree<'a, 'm>) : 'm =
        match t.root with
            | Empty -> t.ctx.zero
            | Single v -> t.ctx.compute v
            | Deep d -> d.annotation

    let prepend (value : 'a) (t : FingerTree<'a, 'm>) =
        { ctx = t.ctx; root = FingerTreeNode.prepend t.ctx value t.root }

    let append (value : 'a) (t : FingerTree<'a, 'm>) =
        { ctx = t.ctx; root = FingerTreeNode.append t.ctx value t.root }


    let toSeq (t : FingerTree<'a, 'm>) =
        t.root.ViewLeft

    let toList (t : FingerTree<'a, 'm>) =
        t.root.ViewLeft |> Seq.toList

    let toArray (t : FingerTree<'a, 'm>) =
        t.root.ViewLeft |> Seq.toArray

    let ofSeq (m : Measure<'a, 'm>) (s : seq<'a>) =
        let mutable r = Empty
        for e in s do
            r <- FingerTreeNode.append m e r

        { ctx = m; root = r }

    let ofList (m : Measure<'a, 'm>) (l : list<'a>) =
        ofSeq m l

    let ofArray (m : Measure<'a, 'm>) (l : 'a[]) =
        ofSeq m l




    let split (cond : 'm -> bool) (t : FingerTree<'a, 'm>) =
        match FingerTreeNode.split t.ctx cond t.ctx.zero t.root with
            | FingerTreeNode.Split(l,v,r) ->
                Split({ t with root = l}, v, { t with root = r })
            | _ ->
                NoSplit

    let concatWithMiddle (l : FingerTree<'a, 'm>) (m : list<'a>) (r : FingerTree<'a, 'm>) =
        let newRoot = FingerTreeNode.concatWithMiddle l.ctx l.root m r.root
        { l with root = newRoot }

    let concat l r =
        concatWithMiddle l [] r


    let viewl (t : FingerTree<'a, 'm>) =
        match FingerTreeNode.viewl t.ctx t.root with
            | Cons(v,rest) -> Some(v, { t with root = rest })
            | _ -> None

    let viewr (t : FingerTree<'a, 'm>) =
        match FingerTreeNode.viewr t.ctx t.root with
            | Cons(v,rest) -> Some(v, { t with root = rest })
            | _ -> None


// immutable array built upon FingerTree
[<StructuredFormatDisplay("{AsString}")>]
type Arr<'a> = private { tree : FingerTree<'a, int> } with
    member private x.AsString =
        x.tree |> FingerTree.toList |> sprintf "%A"

module Arr =
    module Patterns =
        let (|ArrCons|ArrNil|) (a : Arr<'a>) =
            match FingerTree.viewl a.tree with
                | Some(v,r) -> ArrCons(v, { tree = r })
                | None -> ArrNil

            
    let private m<'a> : Measure<'a, int> =
        {
            zero = 0
            add = (+)
            f = fun _ -> 1
        }

    let empty<'a> : Arr<'a> =
        { tree = FingerTree.custom m }

    let ofSeq (l : seq<'a>) =
        { tree = l |> FingerTree.ofSeq m }

    let ofList (l : list<'a>) =
        { tree = l |> FingerTree.ofList m }

    let ofArray (l : 'a[]) =
        { tree = l |> FingerTree.ofArray m }

    let toSeq (a : Arr<'a>) =
        a.tree |> FingerTree.toSeq

    let toList (a : Arr<'a>) =
        a.tree |> FingerTree.toList

    let toArray (a : Arr<'a>) =
        a.tree |> FingerTree.toArray


    let length (a : Arr<'a>) =
        a.tree |> FingerTree.total

    let get (i : int) (a : Arr<'a>) =
        if i < 0 then failwith "index out of range"

        match a.tree |> FingerTree.split (fun id -> id > i) with
            | Split(_,v,_) -> v
            | NoSplit -> failwith "index out of range"

    let set (i : int) (v : 'a) (a : Arr<'a>) =
        if i < 0 then failwith "index out of range"

        match a.tree |> FingerTree.split (fun id -> id > i) with
            | Split(l,_,r) ->
                { tree = FingerTree.concatWithMiddle l [v] r }
            | NoSplit -> 
                failwith "index out of range"

    let splitAt (i : int) (a : Arr<'a>) =
        if i < 0 then 
            failwith "index out of range"
        elif i = 0 then
            empty, a
        else
            match a.tree |> FingerTree.split (fun id -> id > i) with
                | Split(l,v,r) ->
                    { tree = l }, { tree = FingerTree.prepend v r }
                | _ ->
                    failwith "index out of range"

    let concatWithMiddle (l : Arr<'a>) (m : list<'a>) (r : Arr<'a>) =
        { tree = FingerTree.concatWithMiddle l.tree m r.tree }

    let replaceRange (first : int) (size : int) (newElements : list<'a>) (a : Arr<'a>) =
        if first < 0 then failwith "index out of range"
        if size < 0 then failwith "replace does not take negative sizes"

        let (l,rest) = splitAt first a
        let (_,r) = splitAt size rest
        concatWithMiddle l newElements r

    let viewl (a : Arr<'a>) =
        match FingerTree.viewl a.tree with
            | Some(v,r) -> Some(v, { tree = r })
            | None -> None

    let viewr (a : Arr<'a>) =
        match FingerTree.viewr a.tree with
            | Some(v,r) -> Some(v, { tree = r })
            | None -> None


// sorted list built upon FingerTree
[<StructuredFormatDisplay("{AsString}")>]
type SortedList<'a when 'a : comparison> = private { tree : FingerTree<'a, Option<'a>> } with
    member private x.AsString =
        x.tree |> FingerTree.toList |> sprintf "%A"

module SortedList =
    
    let private m<'a when 'a : comparison> : Measure<'a, Option<'a>> =
        {
            zero = None
            add = max
            f = Some
        }

    let empty<'a when 'a : comparison> : SortedList<'a> =
        { tree = FingerTree.custom m }

    let add (v : 'a) (s : SortedList<'a>) =
        match FingerTree.split (fun x -> x >= Some v) s.tree with
            | Split(l,x,r) ->
                let c = compare x v
                if c = 0 then s
                elif c > 0 then { tree = FingerTree.concatWithMiddle l [v;x] r }
                else failwith ""
            | NoSplit ->
                { tree = FingerTree.append v s.tree }
    
    let remove (v : 'a) (s : SortedList<'a>) =
        match FingerTree.split (fun x -> x >= Some v) s.tree with
            | Split(l,x,r) ->
                let c = compare x v
                if c = 0 then { tree = FingerTree.concatWithMiddle l [] r }
                elif c > 0 then s
                else failwith ""
            | NoSplit ->
                s
     
    let ofSeq (vs : seq<'a>) =
        let mutable current = empty
        for v in vs do current <- add v current
        current

    let ofList (vs : list<'a>) =
        vs |> ofSeq

    let ofArray (vs : 'a[]) =
        vs |> ofSeq

    let toSeq (s : SortedList<'a>) =
        s.tree.ViewLeft

    let toList (s : SortedList<'a>) =
        s.tree.ViewLeft |> Seq.toList
    
    let toArray (s : SortedList<'a>) =
        s.tree.ViewLeft |> Seq.toArray

    let splitLessOrEqual (v : 'a) (s : SortedList<'a>) =
        match FingerTree.split (fun vi -> vi >= Some v) s.tree with
            | Split(l,x,r) ->
                let c = compare x v
                if c > 0 then ({ tree = l }, { tree = FingerTree.prepend x r})
                else ({ tree = FingerTree.append x l }, { tree = r })

            | NoSplit ->
                (s, empty)

    let splitLess (v : 'a) (s : SortedList<'a>) =
        match FingerTree.split (fun vi -> vi >= Some v) s.tree with
            | Split(l,x,r) ->
                let c = compare x v
                if c < 0 then ({ tree = FingerTree.append x l }, { tree = r })
                else ({ tree = l }, { tree = FingerTree.prepend x r})
            | NoSplit ->
                (s, empty)

    let inline split (v : 'a) (s : SortedList<'a>) =
        splitLessOrEqual v s

    let minOpt (s : SortedList<'a>) =
        match s.tree.root with
            | Empty -> None
            | Single x -> Some(x)
            | Deep { prefix = p } ->
                Some (Affix.first p)

    let maxOpt (s : SortedList<'a>) =
        match s.tree.root with
            | Empty -> None
            | Single x -> Some(x)
            | Deep { suffix = s } ->
                Some (Affix.last s)

    let rangeOpt (s : SortedList<'a>) =
        match s.tree.root with
            | Empty -> None
            | Single x -> Some(x,x)
            | Deep { prefix = p; suffix = s } ->
                Some (Affix.first p, Affix.last s)

    let inline max (s : SortedList<'a>) =
        s |> maxOpt |> Option.get

    let inline min (s : SortedList<'a>) =
        s |> minOpt |> Option.get

    let inline range (s : SortedList<'a>) =
        s |> rangeOpt |> Option.get

    let isEmpty (s : SortedList<'a>) =
        match s.tree.root with
            | Empty -> true
            | _ -> false


    let union (l : SortedList<'a>) (r : SortedList<'a>) =
        let lt = FingerTree.total l.tree
        let rt = FingerTree.total r.tree

        match lt, rt with
            | None, None -> empty
            | Some _, None -> l
            | None, Some _ -> r
            | Some lv, Some rv ->
                let rightMin = min r
                let leftMin = min l
                if lv < rightMin then
                    { tree = FingerTree.concat l.tree r.tree }
                elif rv < leftMin then
                    { tree = FingerTree.concat r.tree l.tree }
                else
                    let mutable res = l
                    for e in r.tree.ViewLeft do
                        res <- add e res
                    res

    let unsafeConcatWithMiddle (l : SortedList<'a>) (m : list<'a>) (r : SortedList<'a>) =
        { tree = FingerTree.concatWithMiddle l.tree m r.tree }
   
    let unsafeAppendRange (l : SortedList<'a>) (r : list<'a>) =
        { tree = FingerTree.concatWithMiddle l.tree r (FingerTree.custom m) }
    
    let unsafePrependRange (l : list<'a>) (r : SortedList<'a>) =
        { tree = FingerTree.concatWithMiddle (FingerTree.custom m) l r.tree }


    let viewl (l : SortedList<'a>) =
        match FingerTree.viewl l.tree with
            | Some(v,rest) -> Some(v, { tree = rest })
            | None -> None

    let viewr (l : SortedList<'a>) =
        match FingerTree.viewr l.tree with
            | Some(v,rest) -> Some(v, { tree = rest })
            | None -> None

    let replaceRangeInclusive (firstInclusive : 'a) (lastInclusive : 'a) (newElements : list<'a>) (s : SortedList<'a>) =
        let inRange = newElements |> List.forall (fun v -> v >= firstInclusive && v <= lastInclusive)
        let (l, rest) = splitLess firstInclusive s
        let (_,r) = splitLessOrEqual lastInclusive rest

        if inRange then
            { tree = FingerTree.concatWithMiddle l.tree newElements r.tree }
        else
            let maxE = newElements |> List.max
            let minE = newElements |> List.min

            if maxE < min l then
                let l' = List.foldBack FingerTree.prepend newElements l.tree
                { tree = FingerTree.concat l' r.tree}
            elif minE > max r then
                let r' = List.fold (fun s e -> FingerTree.append e s) r.tree newElements
                { tree = FingerTree.concat l.tree r' }
            else
                let mutable l = l
                for e in newElements do l <- add e l
                union l r

    let replaceRangeExclusive (firstExclusive : 'a) (lastExclusive : 'a) (newElements : list<'a>) (s : SortedList<'a>) =
        let inRange = newElements |> List.forall (fun v -> v > firstExclusive && v < lastExclusive)
        let (l, rest) = splitLessOrEqual firstExclusive s
        let (_,r) = splitLess lastExclusive rest

        if inRange then
            { tree = FingerTree.concatWithMiddle l.tree newElements r.tree }
        else
            let maxE = newElements |> List.max
            let minE = newElements |> List.min

            if maxE < min l then
                let l' = List.foldBack FingerTree.prepend newElements l.tree
                { tree = FingerTree.concat l' r.tree}
            elif minE > max r then
                let r' = List.fold (fun s e -> FingerTree.append e s) r.tree newElements
                { tree = FingerTree.concat l.tree r' }
            else
                let mutable l = l
                for e in newElements do l <- add e l
                union l r



[<CustomComparison; CustomEquality>]
type private HalfRange = 
    struct
        val mutable public Value : int
        val mutable public IsMax : bool

        new(v,m) = { Value = v; IsMax = m }

        interface IComparable with
            member x.CompareTo(o) =
                match o with
                    | :? HalfRange as o -> 
                        let c = compare x.Value o.Value
                        if c <> 0 then c
                        else compare (if x.IsMax then 1 else 0) (if o.IsMax then 1 else 0)

                    | _ -> failwith "uncomparable"

        override x.GetHashCode() =
            HashCode.Combine(x.Value.GetHashCode(), x.IsMax.GetHashCode())

        override x.Equals o =
            match o with
                | :? HalfRange as o -> o.Value = x.Value && o.IsMax = x.IsMax
                | _ -> false

        override x.ToString() =
            if x.IsMax then sprintf "Leq %d" x.Value
            else sprintf "Geq %d" x.Value

    end

[<StructuredFormatDisplay("{AsString}")>]
type IntervalTree = private { list : SortedList<HalfRange> } with

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = 
            new IntervalEnumerator(x.list |> SortedList.toSeq) :> System.Collections.IEnumerator

    interface IEnumerable<Range1i> with
        member x.GetEnumerator() = 
            new IntervalEnumerator(x.list |> SortedList.toSeq) :> IEnumerator<_>

    member private x.AsString =
        x |> Seq.toList |> sprintf "%A"

and private IntervalEnumerator(input : seq<HalfRange>) =
    let mutable e = input.GetEnumerator()
    let mutable last = Unchecked.defaultof<HalfRange>
    let mutable current = Unchecked.defaultof<HalfRange>

    member x.Current =
        Range1i(last.Value, current.Value - 1)

    interface System.Collections.IEnumerator with
        member x.MoveNext() =
            if e.MoveNext() then
                last <- e.Current
                if e.MoveNext() then
                    current <- e.Current
                    true
                else
                    false
            else
                false
        
        member x.Current = x.Current :> obj
        member x.Reset() =
            e.Reset()
            last <- Unchecked.defaultof<_>
            current <- Unchecked.defaultof<_>

    interface IEnumerator<Range1i> with
        member x.Dispose() =
            e.Dispose()

        member x.Current = x.Current


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module IntervalTree = 

    [<AutoOpen>]
    module private HalfRanges =
        let Leq v =
            HalfRange(v, true)

        let Geq v =
            HalfRange(v, false)

        let (|Leq|Geq|) (r : HalfRange) =
            if r.IsMax then Leq r.Value
            else Geq r.Value
        

        let inline value (r : HalfRange) = r.Value

    let empty = { list = SortedList.empty }

    let min (t : IntervalTree) =
        t.list |> SortedList.min |> value

    let max (t : IntervalTree) =
        t.list |> SortedList.max |> value

    let insert (r : Range1i) (t : IntervalTree) =
        let r = Range1i(r.Min, r.Max + 1)
        if SortedList.isEmpty t.list then
            { list = SortedList.ofList [Geq r.Min; Leq r.Max] }
        else
            let (left,rest) = t.list |> SortedList.splitLess (Leq r.Min)
            let (inner,right) = rest |> SortedList.splitLessOrEqual (Geq r.Max)

            let smaller = SortedList.viewr left |> Option.map fst
            let greater = SortedList.viewl right  |> Option.map fst


            match smaller, greater with
                | None, None ->
                    { list = SortedList.ofList [Geq r.Min; Leq r.Max] }
                | Some s, None ->
                    match s with
                        | Leq v ->
                            { list = SortedList.unsafeAppendRange left [Geq r.Min; Leq r.Max] }
                        | Geq v ->
                            { list = SortedList.unsafeAppendRange left [Leq r.Max] }

                | None, Some g ->
                    match g with
                        | Leq v ->
                            { list = SortedList.unsafePrependRange [Geq r.Min] right }
                        | Geq v ->
                            { list = SortedList.unsafePrependRange [Geq r.Min; Leq r.Max] right }

                | Some s, Some g ->
                    match s, g with
                        | Leq lmax, Leq rmax ->
                            { list = SortedList.unsafeConcatWithMiddle left [Geq r.Min] right }

                        | Leq lmax, Geq rmin ->
                            { list = SortedList.unsafeConcatWithMiddle left [Geq r.Min; Leq r.Max] right }

                        | Geq lmin, Leq rmax ->
                            { list = SortedList.unsafeConcatWithMiddle left [] right }

                        | Geq lmin, Geq rmin ->
                            { list = SortedList.unsafeConcatWithMiddle left [Leq r.Max] right }

    let remove (r : Range1i) (t : IntervalTree) =
        let r = Range1i(r.Min, r.Max + 1)

        if SortedList.isEmpty t.list then
            { list = t.list }
        else
            let (left,rest) = t.list |> SortedList.splitLess (Leq r.Min)
            let (inner,right) = rest |> SortedList.splitLessOrEqual (Geq r.Max)

            let smaller = SortedList.viewr left |> Option.map fst
            let greater = SortedList.viewl right  |> Option.map fst


            match smaller, greater with
                | None, None ->
                    { list = SortedList.ofList [] }
                | Some s, None ->
                    match s with
                        | Leq v ->
                            { list = left }
                        | Geq v ->
                            { list = SortedList.unsafeAppendRange left [Leq r.Min] }

                | None, Some g ->
                    match g with
                        | Leq v ->
                            { list = SortedList.unsafePrependRange [Geq r.Max] right }
                        | Geq v ->
                            { list = right }

                | Some s, Some g ->
                    match s, g with
                        | Leq lmax, Leq rmax ->
                            { list = SortedList.unsafeConcatWithMiddle left [Geq r.Max] right }

                        | Leq lmax, Geq rmin ->
                            { list = SortedList.unsafeConcatWithMiddle left [] right }

                        | Geq lmin, Leq rmax ->
                            { list = SortedList.unsafeConcatWithMiddle left [Leq r.Min; Geq r.Max] right }

                        | Geq lmin, Geq rmin ->
                            { list = SortedList.unsafeConcatWithMiddle left [Leq r.Min] right }


    let toSeq (t : IntervalTree) = t :> seq<_>
    let toList (t : IntervalTree) = t |> Seq.toList
    let toArray (t : IntervalTree) = t |> Seq.toArray

    let ofSeq (s : seq<Range1i>) =
        let mutable res = empty
        for r in s do res <- insert r res
        res

    let ofList (l : list<Range1i>) =
        l |> ofSeq

    let ofArray (a : Range1i[]) =
        a |> ofSeq

          

// some tests
module Test =
    open Arr.Patterns

    let run() =
        let arr = Arr.ofList [0..9]


        printfn "length = %A" (Arr.length arr)

        printfn "arr = %A" (Arr.toList arr)
        let e4 = Arr.get 4 arr
        printfn "arr[4] = %A" e4


        printfn "arr[4] <- -1"
        let arr = Arr.set 4 -1 arr
        printfn "arr = %A" (Arr.toList arr)

        printfn "Arr.replaceRange 1 2 [1000] "
        let arr = arr |> Arr.replaceRange 1 2 [1000] 
        printfn "arr = %A" (Arr.toList arr)

        let (l,r) = arr |> Arr.splitAt 4
        printfn "arr.[0..3] = %A" l
        printfn "arr.[4..] = %A" r

        match arr with
            | ArrCons(v,rest) ->
                printfn "head = %A" v
                printfn "tail = %A" rest
            | ArrNil ->
                printfn "empty"


    let run2() =
        let l = SortedList.ofList [10;2;3;1;9;4;12]
        printfn "%A" l

        let sp = 4
        let (left, right) = SortedList.split sp l
        printfn "l[<=%A] = %A" sp left
        printfn "l[>%A] = %A" sp right


        let low = SortedList.ofList [1;2;3]
        let high = SortedList.ofList [4;5;6]
        printfn "low = %A" low
        printfn "high = %A" high

        let merged = SortedList.union low high
        printfn "merged = %A" merged


        let (low', high') = SortedList.split 3 merged
        printfn "low' = %A" low'
        printfn "high' = %A" high'


        let a = SortedList.ofList [1;10;3]
        let b = SortedList.ofList [4;5;6]
        printfn "a = %A" a
        printfn "b = %A" b
        let merged = SortedList.union a b
        printfn "merged = %A" merged

        printfn "range(a) = %A" (SortedList.range a)
        printfn "range(b) = %A" (SortedList.range b)


        let test = l |> SortedList.replaceRangeInclusive 4 10 [8]
        printfn "l = %A" l
        printfn "test = %A" test


    let run3() =
        let res = IntervalTree.ofList [Range1i(0,4); Range1i(6,10); Range1i(12,20)]

        printfn "%A" res
