namespace Aardvark.Base

/// <summary>Immutable sets based on binary trees, where comparison is 
/// using the element's hashcodes and duplicates are handled in linked
/// lists</summary>
///
/// <remarks>See the PersistentHashSet module for further operations on sets.
///
/// All members of this class are thread-safe and may be used concurrently from multiple threads.</remarks>
[<StructuredFormatDisplay("{AsString}")>]
type PersistentHashSet<'a>(count : int, content : Map<int, list<'a>>) =

    member x.Count = count

    member internal x.Content = content

    member x.AsString = 
        let l = content |> Map.toList |> List.collect (fun (_,v) -> v)
        sprintf "set %A" l

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = 
            new PersistentHashSetEnumerator<'a>(content) :> _

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() = 
            new PersistentHashSetEnumerator<'a>(content) :> _

       
and private PersistentHashSetEnumerator<'a>(set : Map<int, list<'a>>) =
    let e = (set :> seq<_>).GetEnumerator()
    let mutable current = []
    let mutable value = Unchecked.defaultof<'a>

    member x.MoveNext() =
        match current with
            | [] -> 
                if e.MoveNext() then
                    current <- e.Current.Value
                    match current with
                        | h :: rest ->
                            value <- h
                            current <- rest
                            true
                        | [] ->
                            Log.warn "[PersistentHashSet] empty bucket"
                            x.MoveNext()
                else
                    false
            | h :: rest ->
                value <- h 
                current <- rest
                true

    member x.Current = value

    member x.Reset() =
        e.Reset()
        current <- []
        value <- Unchecked.defaultof<'a>

    member x.Dispose() =
        e.Dispose()
        current <- []
        value <- Unchecked.defaultof<'a>


    interface System.Collections.IEnumerator with
        member x.MoveNext() = x.MoveNext()
        member x.Current = x.Current :> obj
        member x.Reset() = x.Reset()

    interface System.Collections.Generic.IEnumerator<'a> with
        member x.Current = x.Current
        member x.Dispose() = x.Dispose()


       
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
/// <summary>Functional programming operators related to the <c>PersistentHashSet&lt;_&gt;</c> type.</summary> 
module PersistentHashSet =
    let inline private (!) (h : PersistentHashSet<'a>) =
        h.Content

    type private EmptyImpl<'a>() =
        static let instance : PersistentHashSet<'a> = PersistentHashSet(0, Map.empty)
        static member Instance = instance

    /// <summary>The empty set for the type 'T.</summary>
    [<GeneralizableValue>]
    [<CompiledName("Empty")>]
    let empty<'a> : PersistentHashSet<'a> = EmptyImpl<'a>.Instance


    /// <summary>The set containing the given element.</summary>
    /// <param name="value">The value for the set to contain.</param>
    /// <returns>The set containing <c>value</c>.</returns>
    [<CompiledName("Singleton")>]
    let singleton (value : 'a) = 
        let hash = value.GetHashCode()
        PersistentHashSet(1, [hash, [value]] |> Map.ofList)

    /// <summary>Returns a new set with an element added to the set. No exception is raised if
    /// the set already contains the given element.</summary>
    /// <param name="value">The value to add.</param>
    /// <param name="set">The input set.</param>
    /// <returns>A new set containing <c>value</c>.</returns>
    [<CompiledName("Add")>]
    let add (value : 'a) (set : PersistentHashSet<'a>) =
        let map = !set
        let hash = value.GetHashCode()

        match Map.tryFind hash map with
            | Some l ->
                if l |> List.exists (fun e -> e = value) then
                    set
                else
                    PersistentHashSet(set.Count + 1, Map.add hash (value::l) map)
            | None ->
                PersistentHashSet(set.Count + 1, Map.add hash [value] map)


    /// <summary>Returns a new set with the given element removed. No exception is raised if 
    /// the set doesn't contain the given element.</summary>
    /// <param name="value">The element to remove.</param>
    /// <param name="set">The input set.</param>
    /// <returns>The input set with <c>value</c> removed.</returns>
    [<CompiledName("Remove")>]
    let remove (value : 'a) (set : PersistentHashSet<'a>) =
        let map = !set
        let hash = value.GetHashCode()

        match Map.tryFind hash map with
            | Some l ->
                let newL = l |> List.filter (fun e -> e <> value)
                if List.isEmpty newL then
                    PersistentHashSet(set.Count - 1, Map.remove hash map)
                else
                    PersistentHashSet(set.Count - 1, Map.add hash newL map)
            | None ->
                set


    /// <summary>Evaluates to "true" if the given element is in the given set.</summary>
    /// <param name="element">The element to test.</param>
    /// <param name="set">The input set.</param>
    /// <returns>True if <c>element</c> is in <c>set</c>.</returns>
    [<CompiledName("Contains")>]
    let contains (element : 'a) (set : PersistentHashSet<'a>) =
        let map = !set
        let hash = element.GetHashCode()

        match Map.tryFind hash map with
            | Some l -> l |> List.exists (fun e -> e = element)
            | None -> false

    /// <summary>Computes the union of the two sets.</summary>
    /// <param name="set1">The first input set.</param>
    /// <param name="set2">The second input set.</param>
    /// <returns>The union of <c>set1</c> and <c>set2</c>.</returns>
    [<CompiledName("Union")>]
    let union (set1 : PersistentHashSet<'a>) (set2 : PersistentHashSet<'a>) =
        let mutable l = !set1
        let r = !set2

        let mutable count = set1.Count
        for (h,vs) in r |> Map.toSeq do
            match Map.tryFind h l with
                | Some values ->
                    let mutable values = values
                    for v in vs do
                        if values |> List.forall (fun e -> e <> v) then
                            values <- v::values
                            count <- count + 1
                    l <- Map.add h values l

                | None -> 
                    l <- Map.add h vs l
                    count <- count + (List.length vs)

        PersistentHashSet(count, l)

    /// <summary>Computes the union of a sequence of sets.</summary>
    /// <param name="sets">The sequence of sets to untion.</param>
    /// <returns>The union of the input sets.</returns>
    [<CompiledName("UnionMany")>]
    let unionMany (sets : seq<PersistentHashSet<'a>>) =
        sets |> Seq.fold union empty


    /// <summary>Returns a new set with the elements of the second set removed from the first.</summary>
    /// <param name="set1">The first input set.</param>
    /// <param name="set2">The set whose elements will be removed from <c>set1</c>.</param>
    /// <returns>The set with the elements of <c>set2</c> removed from <c>set1</c>.</returns>
    [<CompiledName("Difference")>]
    let difference (set1 : PersistentHashSet<'a>) (set2 : PersistentHashSet<'a>) =
        let mutable l = !set1
        let r = !set2
        let mutable count = set1.Count

        for (h,vs) in r |> Map.toSeq do
            match Map.tryFind h l with
                | Some original ->
                    let mutable values = original
                    for v in vs do
                        values <- values |> List.filter (fun e -> e <> v)

                    count <- count + (List.length values - List.length original)

                    l <- Map.add h values l

                | None -> ()

        PersistentHashSet(count, l)

    /// <summary>Computes the intersection of the two sets.</summary>
    /// <param name="set1">The first input set.</param>
    /// <param name="set2">The second input set.</param>
    /// <returns>The intersection of <c>set1</c> and <c>set2</c>.</returns>
    [<CompiledName("Intersect")>]
    let intersect (set1 : PersistentHashSet<'a>) (set2 : PersistentHashSet<'a>) =
        let mutable l = !set1
        let r = !set2

        let mutable count = set1.Count
        for (h,vs) in r |> Map.toSeq do
            match Map.tryFind h l with
                | None -> ()
                | Some values ->
                    let newValues = values |> List.filter (fun v -> vs |> List.exists (fun vi -> vi = v))
                    l <- Map.add h newValues l
                    count <- count + (List.length newValues - List.length values)
        
        PersistentHashSet(count, l)
   
   
    /// <summary>Computes the intersection of a sequence of sets. The sequence must be non-empty.</summary>
    /// <param name="sets">The sequence of sets to intersect.</param>
    /// <returns>The intersection of the input sets.</returns>
    [<CompiledName("IntersectMany")>] 
    let intersectMany (sets : seq<PersistentHashSet<'a>>) =
        sets |> Seq.fold intersect empty


    /// <summary>Builds a new collection from the given enumerable object.</summary>
    /// <param name="elements">The input sequence.</param>
    /// <returns>The set containing <c>elements</c>.</returns>
    [<CompiledName("OfSeq")>]
    let ofSeq (elements : seq<'a>) =
        let mutable r = empty
        for e in elements do
            r <- add e r
        r


    /// <summary>Builds a set that contains the same elements as the given list.</summary>
    /// <param name="elements">The input list.</param>
    /// <returns>A set containing the elements form the input list.</returns>
    [<CompiledName("OfList")>]
    let ofList (elements : list<'a>) =
        ofSeq elements

    /// <summary>Builds a set that contains the same elements as the given array.</summary>
    /// <param name="array">The input array.</param>
    /// <returns>A set containing the elements of <c>array</c>.</returns>
    [<CompiledName("OfArray")>]
    let ofArray (array : 'a[]) =
        ofSeq array


    /// <summary>Builds a list that contains the elements of the set in order.</summary>
    /// <param name="set">The input set.</param>
    /// <returns>An ordered list of the elements of <c>set</c>.</returns>
    [<CompiledName("ToList")>]
    let toList (set : PersistentHashSet<'a>) =
        set |> Seq.toList
  
    /// <summary>Returns an ordered view of the collection as an enumerable object.</summary>
    /// <param name="set">The input set.</param>
    /// <returns>An ordered sequence of the elements of <c>set</c>.</returns>
    [<CompiledName("ToSeq")>]
    let toSeq (set : PersistentHashSet<'a>) =
        set :> seq<_>    

    /// <summary>Builds an array that contains the elements of the set in order.</summary>
    /// <param name="set">The input set.</param>
    /// <returns>An ordered array of the elements of <c>set</c>.</returns>
    [<CompiledName("ToArray")>]
    let toArray (set : PersistentHashSet<'a>) =
        toList set |> List.toArray


    /// <summary>Returns "true" if the set is empty.</summary>
    /// <param name="set">The input set.</param>
    /// <returns>True if <c>set</c> is empty.</returns>
    [<CompiledName("IsEmpty")>]
    let isEmpty (set : PersistentHashSet<'a>) =
        !set |> Map.isEmpty


    /// <summary>Returns the number of elements in the set. Same as <c>size</c>.</summary>
    /// <param name="set">The input set.</param>
    /// <returns>The number of elements in the set.</returns>
    [<CompiledName("Count")>]
    let count (set : PersistentHashSet<'a>) =
        set.Count

    /// <summary>Evaluates to "true" if all elements of the first set are in the second</summary>
    /// <param name="set1">The potential subset.</param>
    /// <param name="set2">The set to test against.</param>
    /// <returns>True if <c>set1</c> is a subset of <c>set2</c>.</returns>
    [<CompiledName("IsSubset")>]
    let isSubset (set1 : PersistentHashSet<'a>) (set2 : PersistentHashSet<'a>) =
        set1 |> toSeq |> Seq.forall(fun v -> contains v set2)

    /// <summary>Evaluates to "true" if all elements of the first set are in the second, and at least 
    /// one element of the second is not in the first.</summary>
    /// <param name="set1">The potential subset.</param>
    /// <param name="set2">The set to test against.</param>
    /// <returns>True if <c>set1</c> is a proper subset of <c>set2</c>.</returns>
    [<CompiledName("IsProperSubset")>]
    let isProperSubset (set1 : PersistentHashSet<'a>) (set2 : PersistentHashSet<'a>) =
        isSubset set1 set2 && 
        count set1 < count set2

    /// <summary>Evaluates to "true" if all elements of the second set are in the first.</summary>
    /// <param name="set1">The potential superset.</param>
    /// <param name="set2">The set to test against.</param>
    /// <returns>True if <c>set1</c> is a superset of <c>set2</c>.</returns>
    [<CompiledName("IsSuperset")>]
    let isSuperset (set1 : PersistentHashSet<'a>) (set2 : PersistentHashSet<'a>) =
        isSubset set2 set1

    /// <summary>Evaluates to "true" if all elements of the second set are in the first, and at least 
    /// one element of the first is not in the second.</summary>
    /// <param name="set1">The potential superset.</param>
    /// <param name="set2">The set to test against.</param>
    /// <returns>True if <c>set1</c> is a proper superset of <c>set2</c>.</returns>
    [<CompiledName("IsProperSuperset")>]
    let isProperSuperset (set1 : PersistentHashSet<'a>) (set2 : PersistentHashSet<'a>) =
        isProperSubset set2 set1

    /// <summary>Tests if any element of the collection satisfies the given predicate.
    /// If the input function is <c>predicate</c> and the elements are <c>i0...iN</c> 
    /// then computes <c>p i0 or ... or p iN</c>.</summary>
    /// <param name="predicate">The function to test set elements.</param>
    /// <param name="set">The input set.</param>
    /// <returns>True if any element of <c>set</c> satisfies <c>predicate</c>.</returns>
    [<CompiledName("Exists")>]
    let exists (predicate : 'a -> bool) (set : PersistentHashSet<'a>) =
        let m = toSeq set
        m |> Seq.exists predicate

    /// <summary>Tests if all elements of the collection satisfy the given predicate.
    /// If the input function is <c>f</c> and the elements are <c>i0...iN</c> and "j0...jN"
    /// then computes <c>p i0 &amp;&amp; ... &amp;&amp; p iN</c>.</summary>
    /// <param name="predicate">The function to test set elements.</param>
    /// <param name="set">The input set.</param>
    /// <returns>True if all elements of <c>set</c> satisfy <c>predicate</c>.</returns>
    [<CompiledName("ForAll")>]
    let forall (predicate : 'a -> bool) (set : PersistentHashSet<'a>) =
        let m = toSeq set
        m |> Seq.forall predicate
    
    /// <summary>Applies the given function to each element of the set, in order according
    /// to the comparison function.</summary>
    /// <param name="action">The function to apply to each element.</param>
    /// <param name="set">The input set.</param>
    [<CompiledName("Iterate")>]    
    let iter (action : 'a -> unit) (set : PersistentHashSet<'a>) =
        set |> toSeq |> Seq.iter action


    /// <summary>Returns a new collection containing the results of applying the
    /// given function to each element of the input set.</summary>
    /// <param name="mapping">The function to transform elements of the input set.</param>
    /// <param name="set">The input set.</param>
    /// <returns>A set containing the transformed elements.</returns>
    [<CompiledName("Map")>]
    let map (mapping : 'a -> 'b) (set : PersistentHashSet<'a>) =
        let mutable result = empty
        for e in set |> toSeq do
            result <- add (mapping e) result

        result

    let choose (mapping : 'a -> Option<'b>) (set : PersistentHashSet<'a>) =
        let mutable result = empty
        for e in set |> toSeq do
            match mapping e with
                | Some v -> result <- add v result
                | None -> ()

        result


    /// <summary>Returns a new collection containing only the elements of the collection
    /// for which the given predicate returns <c>true</c>.</summary>
    /// <param name="predicate">The function to test set elements.</param>
    /// <param name="set">The input set.</param>
    /// <returns>The set containing only the elements for which <c>predicate</c> returns true.</returns>
    [<CompiledName("Filter")>]
    let filter (predicate : 'a -> bool) (set : PersistentHashSet<'a>) =
        let mutable result = empty
        for e in set |> toSeq do
            if predicate e then
                result <- add e result

        result


    /// <summary>Applies the given accumulating function to all the elements of the set</summary>
    /// <param name="folder">The accumulating function.</param>
    /// <param name="state">The initial state.</param>
    /// <param name="set">The input set.</param>
    /// <returns>The final state.</returns>
    [<CompiledName("Fold")>]
    let fold (folder : 's -> 'a -> 's) (state : 's) (set : PersistentHashSet<'a>) =
        set |> toSeq |> Seq.fold folder state

    /// <summary>Applies the given accumulating function to all the elements of the set.</summary>
    /// <param name="folder">The accumulating function.</param>
    /// <param name="set">The input set.</param>
    /// <param name="state">The initial state.</param>
    /// <returns>The final state.</returns>
    [<CompiledName("FoldBack")>]
    let foldBack (folder : 'a -> 's -> 's) (state : 's) (set : PersistentHashSet<'a>) =
        List.foldBack folder (set |> toList) state


    /// <summary>Splits the set into two sets containing the elements for which the given predicate
    /// returns true and false respectively.</summary>
    /// <param name="predicate">The function to test set elements.</param>
    /// <param name="set">The input set.</param>
    /// <returns>A pair of sets with the first containing the elements for which <c>predicate</c> returns
    /// true and the second containing the elements for which <c>predicate</c> returns false.</returns>
    [<CompiledName("Partition")>]
    let partition (predicate : 'a -> bool) (set : PersistentHashSet<'a>) =
        let mutable l = empty
        let mutable r = empty

        for e in set |> toSeq do
            if predicate e then
                l <- add e l
            else
                r <- add e r

        l,r