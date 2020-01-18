// Learn more about F# at http://fsharp.org

open Aardvark.Base
open FSharp.Data.Traceable
open FSharp.Data.Adaptive
open MBrace.FsPickler
open MBrace.FsPickler.Json

let pickler = FsPickler.CreateJsonSerializer(true, true)

let picklerTest (input : 'a) =
    Log.start "%s" typeof<'a>.Name

    let str = pickler.PickleToString input
    Log.line "%s" str

    let output = str |> pickler.UnPickleOfString
    if input = output then Log.line "success %A" output
    else Log.warn "error: %A vs %A" input output
    Log.stop()

[<EntryPoint>]
let main argv =
    Aardvark.Init()
    picklerTest (HashSet.ofList [1;2;3;2])
    picklerTest (CountingHashSet.ofList [1;2;3;2])
    picklerTest (HashSetDelta.ofList [Add 1; Rem 2; Add 3; Rem 2])
    picklerTest (Map.ofList [1,2; 3,4; 5,6])
    picklerTest (HashMap.ofList [1,2; 3,4; 5,6])
    picklerTest (HashMapDelta.ofList [1, Set 2; 2, Remove; 3, Set 4])
    picklerTest (IndexList.ofList [1;2;3;2])
    picklerTest (MapExt.ofList [1,2; 3,4; 5,6])
    
    0 
