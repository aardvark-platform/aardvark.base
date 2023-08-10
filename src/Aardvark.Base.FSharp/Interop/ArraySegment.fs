namespace Aardvark.Base

open System

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ArraySegment =

    #if !NETSTANDARD2_1_OR_GREATER
    [<AutoOpen>]
    module private Extensions =
        type ArraySegment<'T> with
            member inline x.Item(index : int) = x.Array.[x.Offset + index]
    #endif

    let inline mapArray (mapping : 'T1 -> 'T2) (segment : ArraySegment<'T1>) =
        Array.init segment.Count (fun i -> mapping segment.[i])

    let inline map (mapping : 'T1 -> 'T2) (segment : ArraySegment<'T1>) =
        ArraySegment (segment |> mapArray mapping)

    /// Tests if the array segment contains the specified element.
    let inline contains (value : 'T) (segment : ArraySegment<'T>) =
        let mutable state = false
        let mutable i = 0

        while not state && i < segment.Count do
            state <- value = segment.[i]
            i <- i + 1

        state

    /// Forms a slice of the specified length out of the array segment starting at the specified index.
    let inline slice (start : int) (count : int) (segment : ArraySegment<'T>) =
        #if NETSTANDARD2_1_OR_GREATER
        segment.Slice(start, count)
        #else
        ArraySegment<'T>(segment.Array, segment.Offset + start, count)
        #endif

    /// Forms a slice out of the array segment starting at the specified index.
    let inline sliceFrom (start : int) (segment : ArraySegment<'T>) =
        #if NETSTANDARD2_1_OR_GREATER
        segment.Slice(start)
        #else
        ArraySegment<'T>(segment.Array, segment.Offset + start, segment.Count - start)
        #endif