#if COMPILED
namespace Aardvark.Base
#else
#I @"..\..\bin\Release"
#r "Aardvark.Base.dll"
open Aardvark.Base
#endif

open System
open System.Collections
open System.Collections.Generic


/// Measure represents a monoidal measure for the given type 'a 
/// where 'm has to fulfill monoid laws:
///     1) identity element:    a <*> mempty = mempty <*> a = a
///     2) associativity:       (a <*> b) <> c = a <*> (b <*> c)
type Measure<'a, 'm> = 
    { 
        quantify : 'a -> 'm
        mappend : 'm -> 'm -> 'm
        mempty : 'm
    }

[<AutoOpen>]
module private MeasureHelpers =
    
    let inline ( +. ) (a : Measure<'a, 'm> -> 'm) (b : Measure<'a, 'm> -> 'm) : Measure<'a, 'm> -> 'm =
        fun m -> m.mappend (a m) (b m)

    let inline q (a : 'a) =
        fun (m : Measure<'a, 'm>) -> m.quantify a

    let inline cq (m : 'm) =
        fun (_ : Measure<'a, 'm>) -> m

module FingerTreeImplementation =
    
    type Affix<'a> =
        | One of 'a
        | Two of 'a * 'a
        | Three of 'a * 'a * 'a
        | Four of 'a * 'a * 'a * 'a

    type Node<'a, 'm> =
        | Node2 of 'm * 'a * 'a
        | Node3 of 'm * 'a * 'a * 'a with

        member inline x.Measure =
            match x with
                | Node2(m,_,_) -> m
                | Node3(m,_,_,_) -> m

    type FingerTreeNode<'a, 'm> =
        | Empty
        | Single of 'a
        | Deep of m : 'm * prefix : Affix<'a> * deeper : FingerTreeNode<Node<'a, 'm>, 'm> * suffix : Affix<'a>
       
    type FingerTreeView<'a, 'm> =
        | Nil
        | Cons of 'a * FingerTreeNode<'a, 'm>   
    
    type FingerTreeSplit<'a, 'm> = { left : FingerTreeNode<'a, 'm>; value : 'a; right : FingerTreeNode<'a, 'm> }

    type OptionalFingerTreeSplit<'a, 'm> =
        | Left
        | Right
        | Inside of FingerTreeSplit<'a, 'm>

    module Affix =
        let inline one a = One(a)
        let inline two a b = Two(a,b)
        let inline three a b c = Three(a,b,c)
        let inline four a b c d = Four(a,b,c,d)

        let quanitfy (m : Measure<'a, 'm>) (a : Affix<'a>) =
            match a with
                | One(a) -> m |> (q a)
                | Two(a,b) -> m |> (q a) +. (q b)
                | Three(a,b,c) -> m |> (q a) +. (q b) +. (q c)
                | Four(a,b,c,d) -> m |> (q a) +. (q b) +. (q c) +. (q d)

        let prepend (value : 'a) (a : Affix<'a>) =
            match a with
                | One(a) -> Two(value, a)
                | Two(a,b) -> Three(value, a, b)
                | Three(a,b,c) -> Four(value, a, b, c)
                | _ -> failwith "affix can only hold 1 to 4 elements"

        let append (value : 'a) (a : Affix<'a>) =
            match a with
                | One(a) -> Two(a, value)
                | Two(a,b) -> Three(a,b,value)
                | Three(a,b,c) -> Four(a,b,c,value)
                | _ -> failwith "affix can only hold 1 to 4 elements"

        let toNode (m : Measure<'a, 'm>) (a : Affix<'a>) =
            match a with
                | Two(a,b) -> Node2(m |> (q a) +. (q b), a, b)
                | Three(a,b,c) -> Node3(m |> (q a) +. (q b) +. (q c), a, b, c)
                | _ -> failwith "node can only hold 2 or 3 elements"

        let ofNode (n : Node<'a, 'm>) =
            match n with
                | Node2(_,a,b) -> Two(a,b)
                | Node3(_,a,b,c) -> Three(a,b,c)

        let toListFw (a : Affix<'a>) =
            match a with
                | One(a) -> [a]
                | Two(a,b) -> [a;b]
                | Three(a,b,c) -> [a;b;c]
                | Four(a,b,c,d) -> [a;b;c;d]

        let toListBw (a : Affix<'a>) =
            match a with
                | One(a) -> [a]
                | Two(a,b) -> [b;a]
                | Three(a,b,c) -> [c;b;a]
                | Four(a,b,c,d) -> [d;c;b;a]

        let ofList (l : list<'a>) =
            match l with
                | [a] -> One(a)
                | [a;b] -> Two(a,b)
                | [a;b;c] -> Three(a,b,c)
                | [a;b;c;d] -> Four(a,b,c,d)
                | _ -> failwith "affix can only hold 1 to 4 elements"

        let first (a : Affix<'a>) =
            match a with
                | One(a) -> a
                | Two(a,_) -> a
                | Three(a,_,_) -> a
                | Four(a,_,_,_) -> a 

        let last (a : Affix<'a>) =
            match a with
                | One(a) -> a
                | Two(_,a) -> a
                | Three(_,_,a) -> a
                | Four(_,_,_,a) -> a 

        let takeFirst (a : Affix<'a>) =
            match a with
                | Two(a,b) -> a, One(b)
                | Three(a,b,c) -> a, Two(b,c)
                | Four(a,b,c,d) -> a, Three(b,c,d)
                | _ -> failwith "affix can only hold 1 to 4 elements"

        let takeLast(a : Affix<'a>) =
            match a with
                | Two(a,b) -> b, One(a)
                | Three(a,b,c) -> c, Two(a,b)
                | Four(a,b,c,d) -> d, Three(a,b,c)
                | _ -> failwith "affix can only hold 1 to 4 elements"

        let split (m : Measure<'a, 'm>) (pred : 'm -> bool) (start : 'm) (affix : Affix<'a>) =
            match affix with
                | One(a) ->
                    let s = m.mappend start (m.quantify a)
                    if pred s then None, Some a, None
                    else Some affix, None, None

                | Two(a,b) ->
                    let s = m.mappend start (m.quantify a)
                    if pred s then None, Some a, Some (One b)
                    else
                        let s = m.mappend s (m.quantify b)
                        if pred s then Some (One a), Some b, None
                        else Some affix, None, None
                
                | Three(a,b,c) ->
                    let s = m.mappend start (m.quantify a)
                    if pred s then None, Some a, Some (Two(b,c))
                    else
                        let s = m.mappend s (m.quantify b)
                        if pred s then Some (One a), Some b, Some (One c)
                        else
                            let s = m.mappend s (m.quantify c)
                            if pred s then Some (Two(a,b)), Some c, None
                            else Some affix, None, None
                
                | Four(a,b,c,d) ->
                    let s = m.mappend start (m.quantify a)
                    if pred s then None, Some a, Some (Three(b,c,d))
                    else
                        let s = m.mappend s (m.quantify b)
                        if pred s then Some (One a), Some b, Some (Two(c,d))
                        else
                            let s = m.mappend s (m.quantify c)
                            if pred s then Some (Two(a,b)), Some c, Some (One d)
                            else
                                let s = m.mappend s (m.quantify d)
                                if pred s then Some (Three(a,b,c)), Some d, None
                                else Some affix, None, None

    module Node = 
        let inline two (m : Measure<'a, 'm>) (a : 'a) (b : 'a) =
            Node2(m |> (q a) +. (q b), a, b)

        let inline three (m : Measure<'a, 'm>) (a : 'a) (b : 'a) (c : 'a) =
            Node3(m |> (q a) +. (q b) +. (q c), a, b, c)

        let inline quantify (n : Node<'a, 'm>) =
            match n with
                | Node2(m,_,_) -> m
                | Node3(m,_,_,_) -> m 

        let inline toAffix (n : Node<'a, 'm>) =
            Affix.ofNode n

        let inline ofAffix (m : Measure<'a, 'm>) (a : Affix<'a>) =
            Affix.toNode m a

        let toListFw (n : Node<'a, 'm>) =
            match n with
                | Node2(_,a,b) -> [a;b]
                | Node3(_,a,b,c) -> [a;b;c]

        let toListBw (n : Node<'a, 'm>) =
            match n with
                | Node2(_,a,b) -> [b;a]
                | Node3(_,a,b,c) -> [c;b;a]

        let ofList (m : Measure<'a, 'm>) (l : list<'a>) =
            match l with
                | [a;b] -> Node2(m |> (q a) +. (q b), a, b)
                | [a;b;c] -> Node3(m |> (q a) +. (q b) +. (q c), a, b, c)
                | _ -> failwith "node can only hold 2 or 3 elements"

        let rec manyOfList (m : Measure<'a, 'm>) (l : list<'a>) =
            match l with
                | [] | [_] -> failwith "node can only hold 2 or 3 elements"
                | [a;b] -> [two m a b]
                | [a;b;c] -> [three m a b c]
                | a::b::rest -> (two m a b) :: manyOfList m rest

        let nodesOf (m : Measure<'a, 'm>) (l : Affix<'a>) (r : Affix<'a>) =
            match l, r with
                | One(a), One(b) -> 
                    [ two m a b ]

                | Two(a,b), One(c) 
                | One(a), Two(b,c) -> 
                    [ three m a b c ]

                | One(a), Three(b,c,d)
                | Two(a,b), Two(c,d)
                | Three(a,b,c), One(d) ->
                    [ two m a b; two m c d ]

                | One(a), Four(b,c,d,e) 
                | Two(a,b), Three(c,d,e)
                | Three(a,b,c), Two(d,e)
                | Four(a,b,c,d), One(e) ->
                    [ two m a b; three m c d e ]

                | Two(a,b), Four(c,d,e,f)
                | Three(a,b,c), Three(d,e,f)
                | Four(a,b,c,d), Two(e,f) ->
                    [ three m a b c; three m d e f ]

                | Three(a,b,c), Four(d,e,f,g)
                | Four(a,b,c,d), Three(e,f,g) ->
                    [ two m a b; three m c d e; two m f g ]

                | Four(a,b,c,d), Four(e,f,g,h) ->
                    [ two m a b; three m c d e; three m f g h ]

    module Measure =
        type private NodeMeasureFun<'a, 'm>() =
            static let measureFun = fun (n : Node<'a, 'm>) -> n.Measure
            static member MeasureFun = measureFun

        let node (m : Measure<'a, 'm>) =
            {
                quantify = NodeMeasureFun<'a, 'm>.MeasureFun
                mempty = m.mempty
                mappend = m.mappend
            }

    module FingerTreeNode =
        
        [<AutoOpen>]
        module private ListExtensions =
            
            let rec extractLast (l : list<'a>) =
                match l with
                    | [a] -> a, []
                    | a::rest -> 
                        let (last, l) = extractLast rest
                        last, a :: l
                    | [] -> failwith "empty list"

            let (|Snoc|) (l : list<'a>) =
                Snoc(l |> extractLast)


        let quanitfy (m : Measure<'a, 'm>) (node : FingerTreeNode<'a, 'm>) =
            match node with
                | Empty -> m.mempty
                | Single v -> m.quantify v
                | Deep(m,_,_,_) -> m

        let rec prepend<'a, 'm> (m : Measure<'a, 'm>) (value : 'a) (node : FingerTreeNode<'a, 'm>) : FingerTreeNode<'a, 'm> =
            match node with
                | Empty -> 
                    Single value

                | Single y ->
                    Deep(
                        m |> (q y) +. (q value),
                        Affix.one value,
                        Empty,
                        Affix.one y
                    )

                | Deep(ann, Four(a,b,c,d), deeper, suffix) ->
                    Deep(
                        m |> (cq ann) +. (q value),
                        Affix.two value a,
                        prepend (Measure.node m) (Node.three m b c d) deeper,
                        suffix
                    )

                | Deep(ann, prefix, deeper, suffix) ->
                    Deep(
                        m |> (cq ann) +. (q value),
                        Affix.prepend value prefix,
                        deeper,
                        suffix    
                    )
                            
        let rec append<'a, 'm> (m : Measure<'a, 'm>) (value : 'a) (node : FingerTreeNode<'a, 'm>) : FingerTreeNode<'a, 'm> =
            match node with
                | Empty -> 
                    Single value

                | Single y ->
                    Deep(
                        m |> (q y) +. (q value),
                        Affix.one y,
                        Empty,
                        Affix.one value
                    )

                | Deep(ann, prefix, deeper, Four(a,b,c,d)) ->
                    Deep(
                        m |> (cq ann) +. (q value),
                        prefix,
                        append (Measure.node m) (Node.three m a b c) deeper,
                        Affix.two d value
                    )

                | Deep(ann, prefix, deeper, suffix) ->
                    Deep(
                        m |> (cq ann) +. (q value),
                        prefix,
                        deeper,
                        Affix.append value suffix    
                    )
                            
        let ofAffix (m : Measure<'a, 'm>) (a : Affix<'a>) =
            match a with
                | One(a) -> 
                    Single a

                | Two(a,b) ->
                    Deep(
                        m |> (q a) +. (q b),
                        Affix.one a,
                        Empty,
                        Affix.one b    
                    )

                | Three(a,b,c) ->
                    Deep(
                        m |> (q a) +. (q b) +. (q c),
                        Affix.one a,
                        Empty,
                        Affix.two b c
                    )

                | Four(a,b,c,d) ->
                    Deep(
                        m |> (q a) +. (q b) +. (q c) +. (q d),
                        Affix.two a b,
                        Empty,
                        Affix.two c d
                    )

        let inline ofOptAffix (m : Measure<'a, 'm>) (o : Option<Affix<'a>>) =
            match o with
                | None -> Empty
                | Some a -> ofAffix m a

        let rec viewl<'a, 'm> (m : Measure<'a, 'm>) (node : FingerTreeNode<'a, 'm>) : FingerTreeView<'a, 'm> =
            match node with
                | Empty ->
                    Nil

                | Single v ->
                    Cons(v, Empty)

                | Deep(_, One(a), deeper, suffix) ->
                    match viewl (Measure.node m) deeper with
                        | Nil ->
                            Cons(a, ofAffix m suffix)

                        | Cons(n, rest) ->
                            
                            let pref = Affix.ofNode n
                            let mPref = Affix.quanitfy m pref
                            let mSuff = Affix.quanitfy m suffix
                            let mDeeper = quanitfy (Measure.node m) rest

                            let tail =
                                Deep(
                                    m.mappend (m.mappend mPref mDeeper) mSuff,
                                    pref, rest, suffix
                                )
                            Cons(a, tail)

                | Deep(_,prefix, deeper, suffix) ->
                    let (a, pref) = Affix.takeFirst prefix

                    let mPref = Affix.quanitfy m pref
                    let mSuff = Affix.quanitfy m suffix
                    let mDeeper = quanitfy (Measure.node m) deeper

                    let tail =
                        Deep(
                            m.mappend (m.mappend mPref mDeeper) mSuff,
                            pref, deeper, suffix
                        )

                    Cons(a, tail)

        let rec viewr<'a, 'm> (m : Measure<'a, 'm>) (node : FingerTreeNode<'a, 'm>) : FingerTreeView<'a, 'm> =
            match node with
                | Empty ->
                    Nil

                | Single v ->
                    Cons(v, Empty)

                | Deep(_, prefix, deeper, One(a)) ->
                    match viewr (Measure.node m) deeper with
                        | Nil ->
                            Cons(a, ofAffix m prefix)

                        | Cons(n, rest) ->
                            
                            let suff = Affix.ofNode n
                            let mPref = Affix.quanitfy m prefix
                            let mSuff = Affix.quanitfy m suff
                            let mDeeper = quanitfy (Measure.node m) rest

                            let tail =
                                Deep(
                                    m.mappend (m.mappend mPref mDeeper) mSuff,
                                    prefix, rest, suff
                                )
                            Cons(a, tail)

                | Deep(_,prefix, deeper, suffix) ->
                    let (a, suff) = Affix.takeLast suffix

                    let mPref = Affix.quanitfy m prefix
                    let mSuff = Affix.quanitfy m suff
                    let mDeeper = quanitfy (Measure.node m) deeper

                    let tail =
                        Deep(
                            m.mappend (m.mappend mPref mDeeper) mSuff,
                            prefix, deeper, suff
                        )

                    Cons(a, tail)


        let tail (m : Measure<'a,'m>) (node : FingerTreeNode<'a, 'm>) =
            match viewl m node with
                | Cons(_,rest) -> rest
                | _ -> failwith "empty sequence"

        let init (m : Measure<'a,'m>) (node : FingerTreeNode<'a, 'm>) =
            match viewr m node with
                | Cons(_,rest) -> rest
                | _ -> failwith "empty sequence"

        let firstOpt (n : FingerTreeNode<'a, 'm>) =
            match n with
                | Empty -> None
                | Single v -> Some v
                | Deep(_,p,_,_) -> Affix.first p |> Some

        let lastOpt (n : FingerTreeNode<'a, 'm>) =
            match n with
                | Empty -> None
                | Single v -> Some v
                | Deep(_,_,_,s) -> Affix.last s |> Some

        let total (m : Measure<'a, 'm>) (n : FingerTreeNode<'a, 'm>) =
            match n with
                | Empty -> m.mempty
                | Single v -> m.quantify v
                | Deep(m,_,_,_) -> m

        let rec deep<'a, 'm> (m : Measure<'a, 'm>) (prefix : Option<Affix<'a>>) (deeper : FingerTreeNode<Node<'a, 'm>, 'm>) (suffix : Option<Affix<'a>>) =
            match prefix, suffix with
                | None, None ->
                    match viewl (Measure.node m) deeper with
                        | Cons(n, rest) ->
                            deep m (Some (Affix.ofNode n)) rest None
                        | Nil ->
                            Empty

                | None, Some s ->
                    match viewl (Measure.node m) deeper with
                        | Cons(n, rest) ->
                            deep m (Some (Affix.ofNode n)) rest suffix
                        | Nil ->
                            ofAffix m s

                | Some p, None ->
                    match viewr (Measure.node m) deeper with
                        | Cons(n, rest) ->
                            deep m prefix rest (Some (Affix.ofNode n))
                        | Nil ->
                            ofAffix m p

                | Some p, Some s ->
                    let mPref = Affix.quanitfy m p
                    let mSuff = Affix.quanitfy m s
                    let mDeeper = quanitfy (Measure.node m) deeper

                    Deep(
                        m.mappend (m.mappend mPref mDeeper) mSuff,
                        p, deeper, s
                    )

        let rec split<'a, 'm> (m : Measure<'a, 'm>) (pred : 'm -> bool) (start : 'm) (node : FingerTreeNode<'a, 'm>) : FingerTreeSplit<'a, 'm> =
            match node with
                | Empty -> 
                    failwith "inconsistent measures in FingerTree"

                | Single v ->
                    let s = m.mappend start (m.quantify v)
                    if pred s then { left = Empty; value = v; right = Empty}
                    else failwith "inconsistent measures in FingerTree"

                | Deep(total, prefix, deeper, suffix) ->
                    let current = start
                    let s = m.mappend current (Affix.quanitfy m prefix)
                    if pred s then
                        // inside prefix
                        match Affix.split m pred current prefix with
                            | l, Some s, r ->
                                { left = ofOptAffix m l; value = s; right = deep m r deeper (Some suffix) }
                            | _ ->
                                failwith "inconsistent measures in FingerTree"

                    else
                        let current = s
                        let s = m.mappend current (quanitfy (Measure.node m) deeper)

                        if pred s then
                            // inside deeper
                            let inner = split (Measure.node m) pred current deeper
                            let start = m.mappend current (quanitfy (Measure.node m) inner.left)

                            match Affix.split m pred start (Affix.ofNode inner.value) with
                                | ls, Some s, rp ->
                                    {
                                        left = deep m (Some prefix) inner.left ls
                                        value = s
                                        right = deep m rp inner.right (Some suffix)
                                    }
                                | _ ->
                                    failwith "inconsistent measures in FingerTree"


                        else
                            let current = s
                            let s = m.mappend current (Affix.quanitfy m suffix)
                            if pred s then
                                match Affix.split m pred current suffix with
                                    | l, Some s, r ->
                                        { left = deep m (Some prefix) deeper l; value = s; right = ofOptAffix m r }
                                    | _ ->
                                        failwith "inconsistent measures in FingerTree"
                            else
                                failwith "inconsistent measures in FingerTree"
   
        let splitFirstRight<'a, 'm> (m : Measure<'a, 'm>) (pred : 'm -> bool) (start : 'm) (node : FingerTreeNode<'a, 'm>) : FingerTreeNode<'a, 'm> * FingerTreeNode<'a, 'm> =
            match node with
                | Empty -> 
                    (Empty, Empty)
                | Single v ->
                    let s = m.mappend start (m.quantify v)
                    if pred s then (Empty, Single v)
                    else (Single v, Empty)

                | Deep(_,prefix,deeper,suffix) ->
                    let current = start
                    let s = m.mappend current (Affix.quanitfy m prefix)
                    if pred s then
                        // inside prefix
                        match Affix.split m pred current prefix with
                            | l, Some s, r ->
                                let r =
                                    match r with
                                        | Some r -> Affix.prepend s r
                                        | None -> One(s)

                                (ofOptAffix m l, deep m (Some r) deeper (Some suffix))
                            | _ ->
                                failwith "inconsistent measures in FingerTree"

                    else
                        let current = s
                        let s = m.mappend current (quanitfy (Measure.node m) deeper)

                        if pred s then
                            // inside deeper
                            let inner = split (Measure.node m) pred current deeper
                            let start = m.mappend current (quanitfy (Measure.node m) inner.left)

                            match Affix.split m pred start (Affix.ofNode inner.value) with
                                | ls, Some s, rp ->
                                    let rp =
                                        match rp with
                                            | Some r -> Affix.prepend s r
                                            | None -> One(s)
                         
                                    (deep m (Some prefix) inner.left ls, deep m (Some rp) inner.right (Some suffix))
                   
                                | _ ->
                                    failwith "inconsistent measures in FingerTree"


                        else
                            let current = s
                            let s = m.mappend current (Affix.quanitfy m suffix)
                            if pred s then
                                match Affix.split m pred current suffix with
                                    | l, Some s, r ->
                                        let r =
                                            match r with
                                                | Some r -> Affix.prepend s r
                                                | None -> One(s)
                                        (deep m (Some prefix) deeper l, ofAffix m r)
                                    | _ ->
                                        failwith "inconsistent measures in FingerTree"
                            else
                                (node, Empty)
   
          
        let trySplit<'a, 'm> (m : Measure<'a, 'm>) (pred : 'm -> bool) (node : FingerTreeNode<'a, 'm>) : OptionalFingerTreeSplit<'a, 'm> =
            if pred m.mempty then Left
            elif not (pred (quanitfy m node)) then Right
            else split m pred m.mempty node |> Inside

        let rec concatWithMiddle<'a, 'm> (m : Measure<'a, 'm>) (l : FingerTreeNode<'a, 'm>) (mid : list<'a>) (r : FingerTreeNode<'a, 'm>) : FingerTreeNode<'a, 'm> =
            match l, mid, r with
                | Empty, [], r -> r
                | Empty, (x::xs), r -> prepend m x (concatWithMiddle m Empty xs r)
                | Single y, xs, r -> prepend m y (concatWithMiddle m Empty xs r)

                | l, [], Empty -> l
                | l, Snoc(x,xs), Empty -> append m x (concatWithMiddle m l xs Empty)
                | l, xs, Single y -> append m y (concatWithMiddle m l xs Empty)


                | Deep(lm, lp, ld, ls), mid, Deep(rm,rp,rd,rs) ->
                    
                    let mid' = Node.manyOfList m (Affix.toListFw ls @ mid @ Affix.toListFw rp)
                    let deeper' = concatWithMiddle (Measure.node m) ld mid' rd

                    let mPref = Affix.quanitfy m lp
                    let mSuff = Affix.quanitfy m rs
                    let mDeeper = quanitfy (Measure.node m) deeper'

                    Deep(
                        m.mappend (m.mappend mPref mDeeper) mSuff,
                        lp, deeper', rs
                    )

            
        let rec toSeqFw<'a, 'm> (n : FingerTreeNode<'a, 'm>) : seq<'a> =
            match n with
                | Empty -> Seq.empty
                | Single v -> Seq.singleton v
                | Deep(_,prefix, deeper, suffix) ->
                    seq {
                        yield! Affix.toListFw prefix
                        for n in toSeqFw deeper do
                            yield! Node.toListFw n
                        yield! Affix.toListFw suffix
                    }

        let rec toSeqBw<'a, 'm> (n : FingerTreeNode<'a, 'm>) : seq<'a> =
            match n with
                | Empty -> Seq.empty
                | Single v -> Seq.singleton v
                | Deep(_,prefix, deeper, suffix) ->
                    seq {
                        yield! Affix.toListBw suffix
                        for n in toSeqBw deeper do
                            yield! Node.toListBw n
                        yield! Affix.toListBw suffix
                    }

        let getEnumeratorFw<'a, 'm> (n : FingerTreeNode<'a, 'm>) =
            let s = toSeqFw n
            s.GetEnumerator()

        let getEnumeratorBw<'a, 'm> (n : FingerTreeNode<'a, 'm>) =
            let s = toSeqBw n
            s.GetEnumerator()



open FingerTreeImplementation

type private ArrMeasureImpl<'a>() =
    static let instance =
        {
            quantify = fun (a : 'a) -> 1
            mempty = 0
            mappend = (+)
        }
    static member Instance = instance

[<StructuredFormatDisplay("{AsString}")>]
type arr<'a> = private { root : FingerTreeNode<'a, int> } with
    
    member private x.AsString =
        if x.Length > 20 then
            x.root 
                |> FingerTreeNode.toSeqFw 
                |> Seq.take 20
                |> Seq.map (sprintf "%A")
                |> String.concat "; "
                |> sprintf "arr [%s; ...]"
        else
            x.root 
                |> FingerTreeNode.toSeqFw 
                |> Seq.map (sprintf "%A")
                |> String.concat "; "
                |> sprintf "arr [%s]"

    member x.Length =
        x.root |> FingerTreeNode.quanitfy ArrMeasureImpl<'a>.Instance

    member x.Item
        with get (i : int) =
            if i < 0 || i >= x.Length then raise <| IndexOutOfRangeException()
            let split = x.root |> FingerTreeNode.split ArrMeasureImpl<'a>.Instance (fun id -> id > i) 0 
            split.value

    interface IEnumerable with
        member x.GetEnumerator() = FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() = FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Arr =

    let private mm<'a> = ArrMeasureImpl<'a>.Instance

    let private outOfRange() =
        raise <| IndexOutOfRangeException()


    let empty<'a> : arr<'a> =
        { root = Empty }

    let ofSeq (s : seq<'a>) =
        let mutable res = Empty
        for e in s do res <- FingerTreeNode.append mm e res
        { root = res }

    let inline ofList (l : list<'a>) = ofSeq l
    let inline ofArray (l : 'a[]) = ofSeq l


    let toSeq (a : arr<'a>) = a.root |> FingerTreeNode.toSeqFw
    let inline toList (a : arr<'a>) = a |> toSeq |> Seq.toList
    let inline toArray (a : arr<'a>) = a |> toSeq |> Seq.toArray


    let length (a : arr<'a>) =
        a.root |> FingerTreeNode.quanitfy mm

    let get (i : int) (a : arr<'a>) =
        match FingerTreeNode.trySplit mm (fun id -> id > i) a.root with
            | Inside res -> res.value
            | _ -> outOfRange()

    let set (i : int) (value : 'a) (a : arr<'a>) =
        match FingerTreeNode.trySplit mm (fun id -> id > i) a.root with
            | Inside res ->
                { root = FingerTreeNode.concatWithMiddle mm res.left [value] res.right }
            | _ -> 
                outOfRange()


    let splitAt (i : int) (a : arr<'a>) =
        if i = 0 then 
            empty, a
        else
            match FingerTreeNode.trySplit mm (fun id -> id > i) a.root with
                | Inside res ->
                    { root = res.left }, { root = FingerTreeNode.prepend mm res.value res.right }
                | _ -> 
                    outOfRange()

    let concatWithMiddle (l : arr<'a>) (m : list<'a>) (r : arr<'a>) =
        { root = FingerTreeNode.concatWithMiddle mm l.root m r.root }

    let viewl (a : arr<'a>) =
        match FingerTreeNode.viewl mm a.root with
            | Cons(a, rest) ->
                Some(a, { root = rest })
            | Nil ->
                None

    let viewr (a : arr<'a>) =
        match FingerTreeNode.viewr mm a.root with
            | Cons(a, rest) ->
                Some(a, { root = rest })
            | Nil ->
                None

    let prepend (value : 'a) (a : arr<'a>) =
        { root = FingerTreeNode.prepend mm value a.root }

    let append (value : 'a) (a : arr<'a>) =
        { root = FingerTreeNode.append mm value a.root }





type private SortedMeasureImpl<'a when 'a : comparison>() =

    static let cmp = Comparer<Option<'a>>.Default
    static let vcmp = Comparer<'a>.Default

    static let instance =
        {
            quantify = Some
            mempty = None
            mappend = fun a b -> if cmp.Compare(a,b) > 0 then a else b
        }

    static member Instance = instance
    static member ValueComparer = vcmp

[<StructuredFormatDisplay("{AsString}")>]
type SortedList<'a> = private { root : FingerTreeNode<'a, Option<'a>> } with
    
    member private x.AsString =
        x.root 
            |> FingerTreeNode.toSeqFw 
            |> Seq.map (sprintf "%A")
            |> String.concat "; "
            |> sprintf "sorted [%s]"

    interface IEnumerable with
        member x.GetEnumerator() = FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() = FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root

module SortedList =
    let private mm<'a when 'a : comparison> = SortedMeasureImpl<'a>.Instance
    let private cmp<'a when 'a : comparison> = SortedMeasureImpl<'a>.ValueComparer

    let empty<'a> : SortedList<'a> =
        { root = Empty }

    let add (v : 'a) (l : SortedList<'a>) =
        let ov = Some v
        match FingerTreeNode.trySplit mm (fun i -> i >= ov) l.root with
            | Left -> { root = FingerTreeNode.prepend mm v l.root }
            | Right -> { root = FingerTreeNode.append mm v l.root }
            | Inside split ->
                if cmp.Compare(v, split.value) = 0 then 
                    l
                else
                    { root = FingerTreeNode.concatWithMiddle mm split.left [v; split.value] split.right}
                
    let remove (v : 'a) (l : SortedList<'a>) =
        let ov = Some v
        match FingerTreeNode.trySplit mm (fun i -> i >= ov) l.root with
            | Left -> { root = FingerTreeNode.prepend mm v l.root }
            | Right -> { root = FingerTreeNode.append mm v l.root }
            | Inside split ->
                if cmp.Compare(v, split.value) = 0 then 
                    { root = FingerTreeNode.concatWithMiddle mm split.left [] split.right}
                else
                    l
                
    let ofSeq (s : seq<'a>) =
        let mutable res = empty
        for e in s do res <- add e res
        res
    let inline ofList (l : list<'a>) = ofSeq l
    let inline ofArray (l : 'a[]) = ofSeq l

    let toSeq (a : SortedList<'a>) = a.root |> FingerTreeNode.toSeqFw
    let inline toList (a : SortedList<'a>) = a |> toSeq |> Seq.toList
    let inline toArray (a : SortedList<'a>) = a |> toSeq |> Seq.toArray

    let maxOpt (l : SortedList<'a>) =
        l.root |> FingerTreeNode.lastOpt

    let minOpt (l : SortedList<'a>) =
        l.root |> FingerTreeNode.firstOpt

    let inline max l = (maxOpt l).Value
    let inline min l = (minOpt l).Value

    let viewl (a : SortedList<'a>) =
        match FingerTreeNode.viewl mm a.root with
            | Cons(a, rest) ->
                Some(a, { root = rest })
            | Nil ->
                None

    let viewr (a : SortedList<'a>) =
        match FingerTreeNode.viewr mm a.root with
            | Cons(a, rest) ->
                Some(a, { root = rest })
            | Nil ->
                None





[<CustomEquality;CustomComparison>]
type private HalfRange =
    struct
        val mutable public IsMax : bool
        val mutable public Value : int

        new(m,v) = { IsMax = m; Value = v }

        override x.GetHashCode() =
            if x.IsMax then 0
            else x.Value.GetHashCode()

        override x.Equals o =
            match o with
                | :? HalfRange as o -> 
                    x.IsMax = o.IsMax && x.Value = o.Value
                | _ ->
                    false

        member x.CompareTo (o : HalfRange) =
            let c = x.Value.CompareTo o.Value
            if c = 0 then 
                if x.IsMax = o.IsMax then 0
                else (if x.IsMax then 1 else -1)
            else
                c

        interface IComparable with
            member x.CompareTo o =
                match o with
                    | :? HalfRange as o -> 
                        let c = x.Value.CompareTo o.Value
                        if c = 0 then 
                            if x.IsMax = o.IsMax then 0
                            else (if x.IsMax then 1 else -1)
                        else
                            c
                    | _ -> failwith "uncomparable"
    end

[<StructuredFormatDisplay("{AsString}")>]
type RangeSet = private { root : FingerTreeNode<HalfRange, HalfRange> } with
    
    member private x.AsString =
        x |> Seq.map (sprintf "%A")
          |> String.concat "; "
          |> sprintf "set [%s]"

    member x.Min =
        match x.root |> FingerTreeNode.firstOpt with
            | Some f -> f.Value
            | _ -> Int32.MaxValue

    member x.Max =
        match x.root |> FingerTreeNode.lastOpt with
            | Some f -> f.Value
            | _ -> Int32.MinValue

    member x.Range =
        match FingerTreeNode.firstOpt x.root, FingerTreeNode.lastOpt x.root with
            | Some min, Some max -> Range1i(min.Value, max.Value)
            | _ -> Range1i.Invalid


    interface IEnumerable with
        member x.GetEnumerator() = new RangeSetEnumerator(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> IEnumerator

    interface IEnumerable<Range1i> with
        member x.GetEnumerator() = new RangeSetEnumerator(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> _

and private RangeSetEnumerator(i : IEnumerator<HalfRange>) =
    
    let mutable last = HalfRange()
    let mutable current = HalfRange()

    member x.Current = Range1i(last.Value, current.Value-1)

    interface IEnumerator with
        member x.MoveNext() =
            if i.MoveNext() then
                last <- i.Current
                if i.MoveNext() then
                    current <- i.Current
                    true
                else
                    false
            else
                false

        member x.Current = x.Current :> obj

        member x.Reset() =
            i.Reset()

    interface IEnumerator<Range1i> with
        member x.Current = x.Current
        member x.Dispose() = i.Dispose()




[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RangeSet =
    let private mm =
        {
            quantify = fun (r : HalfRange) -> r
            mempty = HalfRange(false, Int32.MinValue)
            mappend = fun l r -> if l.CompareTo r > 0 then l else r
        }

    let private minRange = HalfRange(false, Int32.MinValue)

    let inline private leq v = HalfRange(true, v)
    let inline private geq v = HalfRange(false, v)

    let inline private (|Leq|Geq|) (r : HalfRange) =
        if r.IsMax then Leq r.Value
        else Geq r.Value

    
    let empty = { root = Empty }

    let insert (range : Range1i) (t : RangeSet) =
        let rangeMax = range.Max + 1

        let (l,rest) = t.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo range.Min >= 0) minRange
        let (_,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax > 0) minRange

        let max = leq rangeMax
        let min = geq range.Min

        match FingerTreeNode.lastOpt l, FingerTreeNode.firstOpt r with
            | None, None -> 
                { root = Deep(max, One(min), Empty, One(max)) }

            | Some lmax, None ->
                match lmax with
                    | Leq _ -> { root = l |> FingerTreeNode.append mm min |> FingerTreeNode.append mm max }
                    | Geq _ -> { root = l |> FingerTreeNode.append mm max }

            | None, Some rmin ->
                match rmin with
                    | Leq _ -> { root = r |> FingerTreeNode.prepend mm min }
                    | Geq _ -> { root = r |> FingerTreeNode.prepend mm max |> FingerTreeNode.prepend mm min }

            | Some lmax, Some rmin ->
                match lmax, rmin with
                    | Leq _, Geq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [min;max] r }

                    | Geq _, Leq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [] r }

                    | Leq _, Leq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [min] r }

                    | Geq _, Geq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [max] r }

    let remove (range : Range1i) (t : RangeSet) =
        let rangeMax = range.Max + 1

        let (l,rest) = t.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo range.Min >= 0) minRange
        let (_,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax > 0) minRange

        let max = geq rangeMax
        let min = leq range.Min

        match FingerTreeNode.lastOpt l, FingerTreeNode.firstOpt r with
            | None, None -> 
                { root = Empty }

            | Some lmax, None ->
                match lmax with
                    | Leq _ -> { root = l }
                    | Geq _ -> { root = l |> FingerTreeNode.append mm min }

            | None, Some rmin ->
                match rmin with
                    | Leq _ -> { root = r |> FingerTreeNode.prepend mm max }
                    | Geq _ -> { root = r }

            | Some lmax, Some rmin ->
                match lmax, rmin with
                    | Leq _, Geq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [] r }

                    | Geq _, Leq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [min; max] r }

                    | Leq _, Leq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [max] r }

                    | Geq _, Geq _ ->
                        { root = FingerTreeNode.concatWithMiddle mm l [min] r }

    let ofSeq (s : seq<Range1i>) =
        let mutable res = empty
        for e in s do res <- insert e res
        res

    let inline ofList (l : list<Range1i>) = ofSeq l
    let inline ofArray (l : Range1i[]) = ofSeq l

    let toSeq (r : RangeSet) = r :> seq<_>
    let toList (r : RangeSet) = r |> Seq.toList
    let toArray (r : RangeSet) = r |> Seq.toArray


    let inline min (t : RangeSet) = t.Min
    let inline max (t : RangeSet) = t.Max
    let inline range (t : RangeSet) = t.Range


    let window (window : Range1i) (set : RangeSet) =
        let rangeMax = window.Max + 1

        let (l,rest) = set.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo window.Min > 0) minRange
        let (inner,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax >= 0) minRange

        let inner =
            match FingerTreeNode.lastOpt l with
                | Some (Geq _) -> FingerTreeNode.prepend mm (geq window.Min) inner
                | _ -> inner

        let inner =
            match FingerTreeNode.firstOpt r with
                | Some (Leq _) -> FingerTreeNode.append mm (leq rangeMax) inner
                | _ -> inner

        { root = inner }

    



module FingerTreeBenchmarks =
    open System.Diagnostics
    open System.IO

    let testPerf (f : unit -> 'a) =
        let sw = Stopwatch()

        // warmup
        for i in 0..100 do f() |> ignore

        // determine an appropriate iteration count
        let mutable iterations = 0UL
        let mutable results = List<'a>(2000)
        sw.Restart()
        while sw.Elapsed.TotalMilliseconds < 500.0 do
            f() |> results.Add
            iterations <- iterations + 1UL
            if iterations % 1000UL = 0UL then
                sw.Stop()
                results <- List<'a>(2000)

                System.GC.Collect(3)
                System.GC.WaitForFullGCComplete() |> ignore
                System.GC.Collect(3)
                System.GC.WaitForFullGCApproach() |> ignore

                sw.Start()

        // may have produced lots of garbage
        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore
        System.GC.Collect(3)
        System.GC.WaitForFullGCApproach() |> ignore


        let sw = Stopwatch()


        // measure real performance
        let mutable results = List<'a>(2000)
        sw.Start()
        for i in 1UL..iterations do
            let r = f()
            results.Add r

            if i % 1000UL = 0UL then
                sw.Stop()
                results <- List<'a>(2000)

                System.GC.Collect(3)
                System.GC.WaitForFullGCComplete() |> ignore
                System.GC.Collect(3)
                System.GC.WaitForFullGCApproach() |> ignore

                sw.Start()
        sw.Stop()

        let timePerF = sw.Elapsed.TotalMilliseconds / float iterations
        
        1000.0 * timePerF

    let runTest (filename : string) (create : int -> 'a) (op : int -> 'a -> 'b) (sizes : list<int>) =
        File.WriteAllLines(filename, ["size;time"])
        
        printfn "determinig nop-overhead"
        let nopInstance = create (List.head sizes)
        testPerf(fun () -> nopInstance) |> ignore // warmup
        let tNop = testPerf(fun () -> nopInstance)
        printfn "nop: %fµs" tNop
        
        for size in sizes do
            System.GC.Collect()
            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore
            let instance = create size
            let t = testPerf(fun () -> op size instance)
            let t = t - tNop

            printfn "%d: %fµs" size t
            File.AppendAllLines(filename, [sprintf "%d;%f" size t])

    let run() =

//        runTest @"C:\Users\schorsch\desktop\SortedListAdd.csv" 
//                (fun s -> SortedList.ofList ([1..s]) )
//                (fun s a -> SortedList.add 1 a)
//                [0..10..2000]

        runTest @"C:\Users\schorsch\desktop\SortedListBuild.csv" 
                (fun s -> [1..s] )
                (fun s a -> a |> List.fold (fun s a -> SortedList.add a s) SortedList.empty)
                [0..10..2000]

        runTest @"C:\Users\schorsch\desktop\FSharpSetBuild.csv" 
                (fun s -> [1..s] )
                (fun s a -> a |> List.fold (fun s a -> Set.add a s) Set.empty)
                [0..10..2000]