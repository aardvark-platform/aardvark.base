#if COMPILED
namespace Aardvark.Base
#endif

type Lens<'s, 'a>() =
    abstract member Get : 's -> 'a
    abstract member Set : 's * 'a -> 's 
    abstract member Update : 's * ('a -> 'a) -> 's

    default x.Get s =
        let mutable res = Unchecked.defaultof<'a>
        x.Update(s, fun v -> res <- v; v) |> ignore
        res

    default x.Set(s,v) =
        x.Update(s, fun _ -> v)

    default x.Update(s, f) =
        x.Set(s, x.Get(s) |> f)

    static member Compose(l : Lens<'s, 'a>, r : Lens<'a, 'b>) =
        { new Lens<'s, 'b>() with
            member x.Get s = l.Get s |> r.Get
            member x.Set(s,v) = l.Update(s, fun s -> r.Set(s, v))
            member x.Update(s,f) = l.Update(s, fun s -> r.Update(s,f))
        }

[<AutoOpen>]
module ``Lens Operators`` =

    let inline (|.) (l : ^a) (r : ^b) : ^c =
        ( (^a or ^b) : (static member Compose : ^a * ^b -> ^c) (l, r))

    let inline (|?) (l : Lens<'s, Option<'a>>) (r : 'a) : Lens<'s, 'a> =
        let def (o : Option<'a>) =
            match o with
                | Some o -> o
                | _ -> r
        { new Lens<'s, 'a>() with
            member x.Get s = l.Get s |> def
            member x.Set(s,v) = l.Set(s, Some v)
            member x.Update(s,f) = l.Update(s, def >> f >> Some)
        }

    let inline (|??) (l : Lens<'s, Option<'a>>) (empty : 'a, isEmpty : 'a -> bool) : Lens<'s, 'a> =
        let def (o : Option<'a>) =
            match o with
                | Some o -> o
                | _ -> empty

        { new Lens<'s, 'a>() with
            member x.Get s = l.Get s |> def
            member x.Set(s,v) = 
                if isEmpty v then l.Set(s, None)
                else l.Set(s, Some v)
        }

[<AutoOpen>]
module ``Predefined Lenses`` =
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Set = 
        module Lens =
            let contains (v : 'a) : Lens<Set<'a>, bool> =
                { new Lens<_, _>() with
                    member x.Get s = Set.contains v s
                    member x.Set(s,r) =
                        if r then Set.add v s
                        else Set.remove v s
                }  
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]        
    module Map =
        module Lens =
            let item (key : 'k) : Lens<Map<'k, 'v>, Option<'v>> =
                { new Lens<_, _>() with
                    member x.Get s = 
                        Map.tryFind key s

                    member x.Set(s,r) =
                        match r with
                            | Some r -> Map.add key r s
                            | None -> Map.remove key s
                }  

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module List =
        module Lens =
            let item (i : int) =
                { new Lens<list<'a>, Option<'a>>() with
                    member x.Get l = 
                        List.tryItem i l

                    member x.Set(l,v) = 
                        match v with
                            | Some v -> 
                                let mutable found = false
                                let mutable cnt = 0
                                let res = l |> List.mapi (fun ii o -> cnt <- cnt + 1; if ii = i then found <- true; v else o)
                            
                                if found then
                                    res
                                elif i = cnt - 1 then
                                    res @ [v]
                                else
                                    raise <| System.IndexOutOfRangeException()
                            | None ->
                                let mutable found = false
                                let mutable cnt = 0
                                let res = l |> List.mapi (fun ii o -> cnt <- cnt + 1; if ii = i then found <- true; None else Some o) |> List.choose id                          
                                res
                }

