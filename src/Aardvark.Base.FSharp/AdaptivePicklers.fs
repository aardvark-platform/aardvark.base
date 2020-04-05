namespace Aardvark.Base.Picklers

open Aardvark.Base
open MBrace.FsPickler
open MBrace.FsPickler.Combinators
open FSharp.Data.Traceable
open FSharp.Data.Adaptive


[<CustomPicklerProvider>]
type AdaptivePicklers() =
    static member HashSet (r : IPicklerResolver) : Pickler<HashSet<'a>> =
        let pInt = r.Resolve<int>()
        let pValue = r.Resolve<'a>()
        let pArray = Pickler.array pValue
        let read (rs : ReadState) =
            let _cnt = pInt.Read rs "count"
            let elements = pArray.Read rs "elements"
            HashSet<'a>.OfArray elements

        let write (ws : WriteState) (s : HashSet<'a>) =
            pInt.Write ws "count" s.Count
            pArray.Write ws "elements" (s.ToArray())

        let clone (cs : CloneState) (m : HashSet<'a>) =
            m |> HashSet.map (pValue.Clone cs)

        let accept (vs : VisitState) (m : HashSet<'a>) =
            for v in m do pValue.Accept vs v
            
        Pickler.FromPrimitives(read, write, clone, accept)

    static member CountingHashSet (r : IPicklerResolver) : Pickler<CountingHashSet<'a>> =
        let pInt = r.Resolve<int>()
        let pValue = r.Resolve<'a>()
        let pArray = Pickler.array (Pickler.pair pValue pInt)

        let read (rs : ReadState) =
            let _cnt = pInt.Read rs "count"
            let elements = pArray.Read rs "elements"
            HashMap.ofArray elements 
            |> CountingHashSet.ofHashMap

        let write (ws : WriteState) (s : CountingHashSet<'a>) =
            pInt.Write ws "count" s.Count
            let elements = s |> CountingHashSet.toHashMap |> HashMap.toArray
            pArray.Write ws "elements" elements

        let clone (cs : CloneState) (m : CountingHashSet<'a>) =
            m |> CountingHashSet.map (pValue.Clone cs)

        let accept (vs : VisitState) (m : CountingHashSet<'a>) =
            for v in m do pValue.Accept vs v
            
        Pickler.FromPrimitives(read, write, clone, accept)

    static member HashSetDelta (r : IPicklerResolver) : Pickler<HashSetDelta<'a>> =
        let pInt = r.Resolve<int>()
        let pValue = r.Resolve<'a>()
        let pArray = Pickler.array (Pickler.pair pValue pInt)

        let read (rs : ReadState) =
            let _cnt = pInt.Read rs "count"
            let elements = pArray.Read rs "elements"
            HashMap.ofArray elements 
            |> HashSetDelta.ofHashMap

        let write (ws : WriteState) (s : HashSetDelta<'a>) =
            pInt.Write ws "count" s.Count
            let elements = s |> HashSetDelta.toHashMap |> HashMap.toArray
            pArray.Write ws "elements" elements

        let clone (cs : CloneState) (m : HashSetDelta<'a>) =
            m |> HashSetDelta.map (SetOperation.map (pValue.Clone cs))

        let accept (vs : VisitState) (m : HashSetDelta<'a>) =
            for v in m do 
                pValue.Accept vs v.Value
            
        Pickler.FromPrimitives(read, write, clone, accept)

    static member IndexList (r : IPicklerResolver) : Pickler<IndexList<'a>> =
        let pInt = r.Resolve<int>()
        let pValue = r.Resolve<'a>()
        let pArray = Pickler.array pValue

        let read (rs : ReadState) =
            let _cnt = pInt.Read rs "count"
            let elements = pArray.Read rs "elements"
            IndexList.ofArray elements

        let write (ws : WriteState) (s : IndexList<'a>) =
            pInt.Write ws "count" s.Count
            pArray.Write ws "elements" (IndexList.toArray s)

        let clone (cs : CloneState) (s : IndexList<'a>) =
            s.Map(fun _ v -> pValue.Clone cs v)

        let accept (vs : VisitState) (s : IndexList<'a>) =
            s |> Seq.iter (pValue.Accept vs)

        Pickler.FromPrimitives(read, write, clone, accept)

    static member IndexListDelta (r : IPicklerResolver) : Pickler<IndexListDelta<'a>> =
        failwith "cannot pickle IndexListDelta"

    static member HashMap (r : IPicklerResolver) : Pickler<HashMap<'k, 'v>> =
        let pInt = r.Resolve<int>()
        let pKey = r.Resolve<'k>()
        let pValue = r.Resolve<'v>()
        let pArray = Pickler.array (Pickler.pair pKey pValue)

        let read (rs : ReadState) =
            let _cnt = pInt.Read rs "count"
            let arr = pArray.Read rs "items"
            HashMap<'k, 'v>.OfArray arr

        let write (ws : WriteState) (m : HashMap<'k, 'v>) =
            pInt.Write ws "count" m.Count
            pArray.Write ws "items" (m.ToArray())

        let clone (cs : CloneState) (m : HashMap<'k, 'v>) =
            m.ToSeqV()
            |> Seq.map (fun struct(k,v) -> struct(pKey.Clone cs k, pValue.Clone cs v))
            |> HashMap.OfSeqV

        let accept (vs : VisitState) (m : HashMap<'k, 'v>) =
            for (k,v) in m do pKey.Accept vs k; pValue.Accept vs v

        Pickler.FromPrimitives(read, write, clone, accept)

    static member HashMapDelta (r : IPicklerResolver) : Pickler<HashMapDelta<'k, 'v>> =
        let pInt = r.Resolve<int>()
        let pKey = r.Resolve<'k>()
        let pValue = r.Resolve<ElementOperation<'v>>()
        let pArray = Pickler.array (Pickler.pair pKey pValue)

        let read (rs : ReadState) =
            let _cnt = pInt.Read rs "count"
            let arr = pArray.Read rs "items"
            HashMapDelta.ofArray arr

        let write (ws : WriteState) (m : HashMapDelta<'k, 'v>) =
            pInt.Write ws "count" m.Count
            pArray.Write ws "items" (HashMapDelta.toArray m)

        let clone (cs : CloneState) (m : HashMapDelta<'k, 'v>) =
            (HashMapDelta.toHashMap m).ToSeqV()
            |> Seq.map (fun struct(k,v) -> struct(pKey.Clone cs k, pValue.Clone cs v))
            |> HashMap.OfSeqV
            |> HashMapDelta.ofHashMap

        let accept (vs : VisitState) (m : HashMapDelta<'k, 'v>) =
            for (k,v) in m do pKey.Accept vs k; pValue.Accept vs v

        Pickler.FromPrimitives(read, write, clone, accept)
