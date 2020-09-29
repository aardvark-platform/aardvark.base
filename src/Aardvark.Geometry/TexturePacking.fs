namespace Aardvark.Geometry

open System
open Aardvark.Base
open Aardvark.Base.Sorting
open Aardvark.Base.Geometry
open Aardvark.Geometry
open FSharp.Data.Adaptive

[<AutoOpen>]
module private TexturePackerHelpers =

    let inline getSize (b : Box2i) = V2i.II + b.Max - b.Min
    
    let inline split (a : Box2i) (b : Box2i) : list<Box2i> =
        List.filter (fun (a : Box2i) -> a.IsNonEmpty) [
            (
                let mutable a = a
                a.Min.X <- b.Max.X + 1
                a
            )
            (
                let mutable a = a
                a.Max.X <- b.Min.X - 1
                a
            )
            (
                let mutable a = a
                a.Min.Y <- b.Max.Y + 1
                a
            )
            (
                let mutable a = a
                a.Max.Y <- b.Min.Y - 1
                a
            )
        ]

    let inline merge (boxes : HashSet<Box2i>) : seq<Box2i> =
        if boxes.Count <= 1 then 
            boxes :> seq<_>
        else
            failwith "[TexturePacking] remove not implemented: max-rects merge"
        
    let inline addFree (b : Box2i) (free : BvhTree2d<_,_>) =
        if b.IsEmpty then free
        else 
            let c = BvhTree2d.getContaining (Box2d b) free
            if c.IsEmpty then 
                BvhTree2d.add b (Box2d b) b free
            else 
                free

    module Seq =
        let inline tryMinByV (mapping : 'a -> ValueOption<struct('b * 'c)>) (l : seq<'a>) =
            use e = l.GetEnumerator()

            let mutable best = ValueNone
            let mutable bestScore = Unchecked.defaultof<'b>
            while ValueOption.isNone best && e.MoveNext() do
                let c = e.Current
                match mapping c with
                | ValueSome(s, v) ->
                    bestScore <- s
                    best <- ValueSome(struct(c, v))
                | ValueNone ->
                    ()

            match best with
            | ValueSome best ->
                let mutable best = best

                while e.MoveNext() do
                    let c = e.Current
                    match mapping c with
                    | ValueSome(s, v) ->
                        if s < bestScore then
                            bestScore <- s
                            best <- struct(c, v)
                        
                    | ValueNone ->
                        ()

                ValueSome best

            | ValueNone ->
                ValueNone

/// TexturePacking represents an immutable 2D atlas.
type TexturePacking<'a when 'a : equality> private(atlasSize : V2i, free : BvhTree2d<Box2i, Box2i>, used : HashMap<'a, Box2i>) =
    /// All free MaxRects.
    member x.Free = free

    /// Used Rects.
    member x.Used = used

    /// Total Atlas Size.
    member x.Size = atlasSize

    /// The relative number of pixels occupied.
    member x.Occupancy =
        let area = 
            used |> Seq.sumBy (fun (_, b) ->
                let s = V2i.II + b.Max - b.Min
                float s.X * float s.Y
            )
        area / (float atlasSize.X * float atlasSize.Y)

    /// Creates an empty TexturePacking with the given size.
    static member Empty (size : V2i) : TexturePacking<'a> =
        if size.AnySmallerOrEqual 0 then
            TexturePacking<'a>(V2i.Zero, BvhTree2d.empty, HashMap.empty)
        else
            let bb = Box2i(V2i.Zero, size - V2i.II)
            TexturePacking<'a>(size, BvhTree2d.ofList [bb, Box2d bb, bb], HashMap.empty)

    /// Tries to add an element with the given size and (optionally) returns a new TexturePacking.
    member x.TryAdd(id : 'a, size : V2i) : option<TexturePacking<'a>> =
        if size.AnySmallerOrEqual 0 then
            Some x
        else
            match HashMap.tryFind id used with
            | Some b ->
                if getSize b = size then
                    Some x
                else
                    x.Remove(id).TryAdd(id, size)
                    
            | None ->   
                let error (b : Box2i) : voption<struct(float * bool)> =
                    let sh = getSize b

                    let f0 = sh.AllGreaterOrEqual size
                    let f1 = sh.AllGreaterOrEqual size.YX
                    let w0 = (sh.X - size.X) * (sh.Y - size.Y)
                    let w1 = (sh.X - size.Y) * (sh.Y - size.X)

                    let inline w (v : int) =
                        float v
                        //1.0 / (1.0 + float v)

                    if f0 && f1 then
                        if w0 < w1 then ValueSome struct(w w0, true)
                        else ValueSome struct(w w1, false)

                    elif f0 then
                        ValueSome struct(w w0, true)
                        
                    elif f1 then
                        ValueSome struct(w w1, false)
                        
                    else
                        ValueNone

                let fitting = 
                    free 
                    |> BvhTree2d.keys 
                    |> Seq.tryMinByV error

                match fitting with
                | ValueSome (struct(fitting, upright)) -> 
                    let size = 
                        if upright then size
                        else size.YX

                    let r0 = Box2i(fitting.Min + V2i(size.X, 0), fitting.Max)
                    let r1 = Box2i(fitting.Min + V2i(0, size.Y), fitting.Max)

                    let rect = Box2i(fitting.Min, fitting.Min + size - V2i.II)

                    let free =
                        free
                        |> BvhTree2d.remove fitting
                        |> addFree r0
                        |> addFree r1

                    let intersecting = 
                        BvhTree2d.getIntersecting (Box2d rect) free

                    let free = 
                        (free, intersecting) ||> HashMap.fold (fun free fid struct(_, other) ->
                            (BvhTree2d.remove fid free, split other rect) 
                            ||> List.fold (fun free p -> addFree p free)
                        )

                    let result =
                        TexturePacking(atlasSize, free, HashMap.add id rect used)

                    Some result
                | ValueNone ->
                    None

    /// Tries to add several elements with the given sizes and (optionally) returns a new TexturePacking.
    member x.TryAdd(many : seq<'a * V2i>) : option<TexturePacking<'a>> =
        use e = many.GetEnumerator()
        let mutable res = Some x
        while Option.isSome res && e.MoveNext() do
            let (id, size) = e.Current
            res <- res.Value.TryAdd(id, size)
        res

    /// Removes a given element from the TexturePacking.
    member x.Remove(id : 'a) : TexturePacking<'a> =
        match HashMap.tryRemoveV id used with
        | ValueSome(rect, used) ->
            let q = Box2i(rect.Min - V2i.II, rect.Max + V2i.II)
            let adjacent = 
                free 
                |> BvhTree2d.getIntersecting (Box2d q)
                |> HashMap.keys
                |> HashSet.filter (fun b -> 
                    b.Min.X = q.Max.X ||
                    b.Max.X = q.Min.X ||
                    b.Min.Y = q.Max.Y ||
                    b.Max.Y = q.Min.Y
                )

            let free = 
                (free, adjacent) 
                ||> HashSet.fold (fun free a -> BvhTree2d.remove a free)

            let free = 
                (free, merge (HashSet.add rect adjacent))
                ||> Seq.fold (fun free a -> addFree a free)
                
            TexturePacking(atlasSize, free, used)
        | ValueNone ->
            x

/// TexturePacking represents an immutable 2D atlas.
module TexturePacking =
    
    /// Creates an empty TexturePacking with the given size.
    let inline empty (size : V2i) = TexturePacking<'a>.Empty size
    
    /// Checks whether the TexturePacking is empty.
    let inline isEmpty (packing : TexturePacking<'a>) = packing.Used.Count = 0
    
    /// Returns the number of Rects in the TexturePacking.
    let inline count (packing : TexturePacking<'a>) = packing.Used.Count
    
    /// The relative number of pixels occupied.
    let inline occupancy (packing : TexturePacking<'a>) = packing.Occupancy

    /// Tries to add an element with the given size and (optionally) returns a new TexturePacking.
    let inline tryAdd (id : 'a) (size : V2i) (packing : TexturePacking<'a>) =
        packing.TryAdd(id, size)
            
    /// Tries to add several elements with the given sizes and (optionally) returns a new TexturePacking.
    let inline tryAddMany (elements : #seq<'a * V2i>) (packing : TexturePacking<'a>) =
        let sorted = elements |> Seq.sortByDescending (fun (_,v) -> max v.X v.Y)
        packing.TryAdd sorted

    /// Tries to create a TexturePacking with the given size for all elements.
    let inline tryOfSeq (size : V2i) (elements : #seq<'a * V2i>) =
        empty size
        |> tryAddMany elements
            
    /// Tries to create a TexturePacking with the given size for all elements.
    let inline tryOfList (size : V2i) (elements : list<'a * V2i>) =
        empty size
        |> tryAddMany elements
            
    /// Tries to create a TexturePacking with the given size for all elements.
    let inline tryOfArray (size : V2i) (elements : array<'a * V2i>) =
        empty size
        |> tryAddMany elements
        
    /// Tries to create an optimal square packing for the given elements.
    let square (elements : seq<'a * V2i>) =
        let elements = Seq.toArray elements
        elements.QuickSortDescending(fun (_,v) -> max v.X v.Y)
        if elements.Length <= 0 then
            empty V2i.Zero
        elif elements.Length = 1 then
            let (id, size) = elements.[0]
            let s = max size.X size.Y
            empty (V2i(s,s)) |> tryAdd id size |> Option.get
        else
            let area = elements |> Array.sumBy (fun (_,s) -> s.X * s.Y)
            let mutable l = floor (sqrt (float area)) |> int
            let mutable h = Fun.NextPowerOfTwo l

            let mutable best = tryOfArray (V2i(h,h)) elements
            while Option.isNone best do
                l <- h
                h <- h * 2
                best <- tryOfArray (V2i(h,h)) elements


            let mutable bestPacking = best.Value
            while h > l + 1 do
                let m = (h + l) / 2
                match tryOfArray (V2i(m, m)) elements with
                | Some packing ->
                    bestPacking <- packing
                    h <- m
                | None ->
                    l <- m

            bestPacking

    /// Creates a sequence of all Rects in the packing.
    let inline toSeq (packing : TexturePacking<'a>) =
        packing.Used :> seq<_>

    /// Creates a list of all Rects in the packing.
    let inline toList (packing : TexturePacking<'a>) =
        packing.Used |> HashMap.toList

    /// Creates an array of all Rects in the packing.
    let inline toArray (packing : TexturePacking<'a>) =
        packing.Used |> HashMap.toArray
