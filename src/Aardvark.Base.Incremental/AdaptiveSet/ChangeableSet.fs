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

    let emit (deltas : Option<Change<'a>>) =
        lock readers (fun () ->
            for r in readers do 
                r.Emit(content, deltas)
        )

    interface aset<'a> with
        member x.ReaderCount = lock readers (fun () -> readers.Count)
        member x.IsConstant = false
        member x.GetReader() =
            lock readers (fun () ->
                let r = new EmitReader<'a>(fun r -> readers.Remove r |> ignore)
                r.Emit(content, None)
                readers.Add r |> ignore
                r :> _
            )

    member x.Readers = lock readers (fun () -> readers |> Seq.toList :> seq<_>)

    /// Gets the number of elements contained in the cset.
    member x.Count = lock content (fun () ->  content.Count)

    /// Determines whether a cset contains a specified element by using the default equality comparer.
    member x.Contains v = lock content (fun () -> content.Contains v)


    /// Modifies the current set so that it contains all elements that are present in either the current set or the specified collection.
    member x.UnionWith(s : seq<'a>) =
        let res =  lock content (fun () ->  s |> Seq.filter content.Add |> Seq.map Add |> Seq.toList)
        emit (Some res)

    /// Removes all elements in the specified collection from the current set.
    member x.ExceptWith(s : seq<'a>) =
        let res =  lock content (fun () -> s |> Seq.filter content.Remove |> Seq.map Rem |> Seq.toList)
        emit (Some res)

    /// Modifies the current set so that it contains only elements that are also in a specified collection.
    member x.IntersectWith(s : seq<'a>) =
        let s = HashSet(s)
        let res = 
            lock content (fun () -> 
                content 
                    |> Seq.toList
                    |> List.filter (not << s.Contains) 
                    |> List.map (fun a -> content.Remove a |> ignore; Rem a) 
            )

        emit (Some res)

    /// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
    member x.SymmetricExceptWith(s : seq<'a>) =
        let s = HashSet(s)
        let res = 
            lock content (fun () -> 
                let removed = 
                    content 
                        |> Seq.toList
                        |> List.choose (fun a -> 
                                if s.Contains a then 
                                    content.Remove a |> ignore
                                    s.Remove a |> ignore
                                    Some (Rem a)
                                else None
                            ) 
                let added = s |> Seq.filter content.Add |> Seq.map Add |> Seq.toList
                List.append removed added
            )

        emit (Some res)

    /// Removes all items from the set.
    member x.Clear() =
        let res = lock content (fun () -> 
            let res = content |> Seq.map Rem |> Seq.toList
            content.Clear()
            res
        )
        emit (Some res)

    /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
    member x.Add v =
        if lock content (fun () -> content.Add v) then 
            emit (Some [Add v])
            true
        else
            false

    /// Removes an element from the current set and returns a value to indicate if the element was successfully removed.
    member x.Remove v =
        if lock content (fun () -> content.Remove v) then 
            emit (Some [Rem v])
            true
        else
            false

    member x.ApplyDeltas (deltas : Change<'a>) =
        let deltas = 
            lock content (fun () -> 
                deltas |> List.choose (fun d ->
                    match d with
                        | Add v when content.Add v -> Some (Add v)
                        | Rem v when content.Remove v -> Some (Rem v)
                        | _ -> None
                )
            )
        emit (Some deltas)

    /// Creates a new empty set.
    new() = cset Seq.empty

    interface ISet<'a> with
        member x.Add(item: 'a): bool = x.Add item
        member x.Add(item: 'a): unit = x.Add item |> ignore
        member x.Clear(): unit = x.Clear()
        member x.Contains(item: 'a): bool = x.Contains item
        member x.CopyTo(array: 'a [], arrayIndex: int): unit = lock content (fun () -> content.CopyTo(array, arrayIndex))
        member x.Count: int = x.Count
        member x.ExceptWith(other: IEnumerable<'a>): unit = x.ExceptWith other
        member x.GetEnumerator(): IEnumerator = content.GetEnumerator() :> IEnumerator
        member x.GetEnumerator(): IEnumerator<'a> = content.GetEnumerator() :> IEnumerator<'a>
        member x.IntersectWith(other: IEnumerable<'a>): unit = x.IntersectWith other
        member x.IsProperSubsetOf(other: IEnumerable<'a>): bool = lock content (fun () -> content.IsProperSubsetOf other)
        member x.IsProperSupersetOf(other: IEnumerable<'a>): bool = lock content (fun () -> content.IsProperSupersetOf other)
        member x.IsReadOnly: bool = false
        member x.IsSubsetOf(other: IEnumerable<'a>): bool = lock content (fun () -> content.IsSubsetOf other)
        member x.IsSupersetOf(other: IEnumerable<'a>): bool = lock content (fun () -> content.IsSupersetOf other)
        member x.Overlaps(other: IEnumerable<'a>): bool = lock content (fun () -> content.Overlaps other)
        member x.Remove(item: 'a): bool = x.Remove item
        member x.SetEquals(other: IEnumerable<'a>): bool = lock content (fun () -> content.SetEquals other)
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
        lock (fun () -> set |> Seq.toList)

    /// Gets an array containing all elements from the cset.
    let toArray (set : cset<'a>) =
        lock (fun () -> set |> Seq.toArray)

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
        xs.ApplyDeltas deltas