// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Aardvark.Base

[<EntryPoint>]
let main argv = 

    let img = PixImage.Create(@"C:\Users\haaser\Desktop\SixteenBitRGB.tif");

    img.SaveAsImage(@"C:\Users\haaser\Desktop\SixteenBitRGB2.tif")

    printfn "%A" img

    0 // return an integer exit code
