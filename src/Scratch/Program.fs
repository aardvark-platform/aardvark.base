// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Aardvark.Base

[<EntryPoint>]
let main argv = 
    
    let test = DynamicLinker.tryUnpackNativeLibrary "Assimp"

    

    printfn "%A" test
    0 // return an integer exit code
