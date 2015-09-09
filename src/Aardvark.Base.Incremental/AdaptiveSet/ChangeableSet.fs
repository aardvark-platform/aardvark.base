namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental.ASetReaders


/// <summary>
/// ChangeableSet is a user-changeable set implementing aset.
/// NOTE that changes need to be pushed using a transaction.
/// </summary>
[<CompiledName("ChangeableSet")>]
type cset<'a>(initial : seq<'a>) =
    let content = VersionedSet (HashSet initial)
    let readers = WeakSet<EmitReader<'a>>()

    interface aset<'a> with
        member x.IsConstant = false
        member x.GetReader() =
            let r = new EmitReader<'a>(fun r -> readers.Remove r |> ignore)
            r.Emit(content, None)
            readers.Add r |> ignore
            r :> _

    /// Gets the number of elements contained in the cset.
    member x.Count = content.Count

    /// Determines whether a cset contains a specified element by using the default equality comparer.
    member x.Contains v = content.Contains v

    /// Modifies the current set so that it contains all elements that are present in either the current set or the specified collection.
    member x.UnionWith(s : seq<'a>) =
        let res = s |> Seq.filter content.Add |> Seq.map Add |> Seq.toList
        for r in readers do 
            r.Emit(content, Some res)

    /// Removes all elements in the specified collection from the current set.
    member x.ExceptWith(s : seq<'a>) =
        let res = s |> Seq.filter content.Remove |> Seq.map Rem |> Seq.toList
        for r in readers do 
            r.Emit(content, Some res)

    /// Modifies the current set so that it contains only elements that are also in a specified collection.
    member x.IntersectWith(s : seq<'a>) =
        let s = HashSet(s)
        let res = 
            content |> Seq.toList
                    |> List.filter (not << s.Contains) 
                    |> List.map (fun a -> content.Remove a |> ignore; Rem a) 

        for r in readers do 
            r.Emit(content, Some res)

    /// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
    member x.SymmetricExceptWith(s : seq<'a>) =
        let s = HashSet(s)
        let removed = 
            content |> Seq.toList
                    |> List.choose (fun a -> 
                            if s.Contains a then 
                                content.Remove a |> ignore
                                s.Remove a |> ignore
                                Some (Rem a)
                            else None
                        ) 

        let added = s |> Seq.filter content.Add |> Seq.map Add |> Seq.toList
        let res = List.append removed added

        for r in readers do 
            r.Emit(content, Some res)

    /// Removes all items from the set.
    member x.Clear() =
        content.Clear()
        for r in readers do 
            r.Emit(content, None)

    /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
    member x.Add v =
        if content.Add v then 
            for r in readers do 
                r.Emit(content, Some [Add v])

            true
        else
            false

    /// Removes an element from the current set and returns a value to indicate if the element was successfully removed.
    member x.Remove v =
        if content.Remove v then 
            for r in readers do
                r.Emit(content, Some [Rem v])

            true
        else
            false

    /// Creates a new empty set.
    new() = cset Seq.empty

    interface ISet<'a> with
        member x.Add(item: 'a): bool = x.Add item
        member x.Add(item: 'a): unit = x.Add item |> ignore
        member x.Clear(): unit = x.Clear()
        member x.Contains(item: 'a): bool = x.Contains item
        member x.CopyTo(array: 'a [], arrayIndex: int): unit = content.CopyTo(array, arrayIndex)
        member x.Count: int = x.Count
        member x.ExceptWith(other: IEnumerable<'a>): unit = x.ExceptWith other
        member x.GetEnumerator(): IEnumerator = content.GetEnumerator() :> IEnumerator
        member x.GetEnumerator(): IEnumerator<'a> = content.GetEnumerator() :> IEnumerator<'a>
        member x.IntersectWith(other: IEnumerable<'a>): unit = x.IntersectWith other
        member x.IsProperSubsetOf(other: IEnumerable<'a>): bool = content.IsProperSubsetOf other
        member x.IsProperSupersetOf(other: IEnumerable<'a>): bool = content.IsProperSupersetOf other
        member x.IsReadOnly: bool = false
        member x.IsSubsetOf(other: IEnumerable<'a>): bool = content.IsSubsetOf other
        member x.IsSupersetOf(other: IEnumerable<'a>): bool = content.IsSubsetOf other
        member x.Overlaps(other: IEnumerable<'a>): bool = content.Overlaps other
        member x.Remove(item: 'a): bool = x.Remove item
        member x.SetEquals(other: IEnumerable<'a>): bool = content.SetEquals other
        member x.SymmetricExceptWith(other: IEnumerable<'a>): unit = x.SymmetricExceptWith other
        member x.UnionWith(other: IEnumerable<'a>): unit = x.UnionWith other

module CSet =

    /// Creates a new empty cset.
    let empty<'a> = cset<'a>()

    /// Creates a new cset containing all distinct elements from the given sequence.
    let ofSeq (s : seq<'a>) = cset<'a> s

    /// Creates a new cset containing all distinct elements from the given list.
    let ofList (s : list<'a>) = cset<'a> s

    /// Creates a new cset containing all distinct elements from the given array.
    let ofArray (s : 'a[]) = cset<'a> s

    /// Adds an element to the set and returns a value to indicate if the element was successfully added.
    let add (a : 'a) (set : cset<'a>) = set.Add a
    
    /// Removes an element from the set and returns a value to indicate if the element was successfully removed.
    let remove (a : 'a) (set : cset<'a>) = set.Remove a

    /// Removes all items from the set.
    let clear (set : cset<'a>) =
        set.Clear()

    /// Determines whether a cset contains a specified element.
    let contains (value : 'a) (set : cset<'a>) =
        set.Contains value

    /// Gets the number of elements contained in the cset.
    let count (set : cset<'a>) =
        set.Count

    /// Gets a sequence containing all elements from the cset.
    let toSeq (set : cset<'a>) =
        set :> seq<_>

    /// Gets a list containing all elements from the cset.
    let toList (set : cset<'a>) =
        set |> Seq.toList

    /// Gets an array containing all elements from the cset.
    let toArray (set : cset<'a>) =
        set |> Seq.toArray

    /// Modifies the set so that it contains all elements that are present in either the current set or the specified collection.
    let unionWith (elems : seq<'a>) (set : cset<'a>) =
        set.UnionWith elems

    /// Removes all elements in the specified collection from the set.
    let exceptWith (elems : seq<'a>) (set : cset<'a>) =
        set.ExceptWith elems

    /// Modifies the set so that it contains only elements that are also in a specified collection.
    let intersectWith (elems : seq<'a>) (set : cset<'a>) =
        set.IntersectWith elems

    /// Determines whether the set and the specified collection contain the same elements.
    let equals (elems : seq<'a>) (set : cset<'a>) =
        set.SetEquals(elems)

    /// Modifies the set by performing all delta operations given.
    let applyDeltas (deltas : list<Delta<'a>>) (xs : cset<'a>) =
        for d in deltas do
            match d with 
              | Add x -> xs.Add x |> ignore
              | Rem x -> xs.Remove x |> ignore
