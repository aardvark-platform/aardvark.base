namespace Aardvark.Base.Incremental.Tests

open System.Collections.Generic
open Aardvark.Base.Incremental
           
module ASetReferenceImpl =

    type IReader<'a> = 
        abstract member GetDelta : unit -> Change<'a>
        abstract member Content : ReferenceCountingSet<'a>

    type aset<'a> = 
        abstract member traverse : unit -> list<'a> 
        abstract member GetReader : unit -> IReader<'a>

    type ASetReader<'a>(xs : aset<'a>) =
        let mutable oldValues = ReferenceCountingSet()
        interface IReader<'a> with
            member x.GetDelta() =
                let newValues = ReferenceCountingSet( xs.traverse () )
                let adds     = newValues |> Seq.filter (not << oldValues.Contains) |> Seq.map Add |> Seq.toList
                let removals = oldValues |> Seq.filter (not << newValues.Contains) |> Seq.map Rem |> Seq.toList
                oldValues <- newValues
                List.concat [removals; adds]

            member x.Content = oldValues


    type cset<'a>(xs : seq<'a>) =
        let values = HashSet(xs)
        member x.Add y = values.Add y
        member x.Remove y = values.Remove y

        member x.Clear() = values.Clear()

        interface aset<'a> with
            member x.traverse () = values |> Seq.toList
            member x.GetReader () = ASetReader x :> _


    let traverse (xs : aset<'a>) = xs.traverse ()
   
    let map (f : 'a -> 'b) (xs : aset<'a>) : aset<'b> =
        { new aset<'b> with 
            member x.traverse () = xs.traverse () |> List.map f
            member x.GetReader () = ASetReader x :> _
        }

    let collect (f : 'a -> aset<'b>) (xs : aset<'a>) : aset<'b> =
        { new aset<'b> with
            member x.traverse () = xs.traverse () |> List.collect (traverse << f) 
            member x.GetReader () = ASetReader x :> _
        }
