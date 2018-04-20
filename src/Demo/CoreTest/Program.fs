// Learn more about F# at http://fsharp.org

open System
open Aardvark.Base

[<EntryPoint>]
let main argv =
    Aardvark.Init()


    let pi = PixImage.Create @"image.jpg"
    pi.SaveAsImage @"sepp.png"
    0 
