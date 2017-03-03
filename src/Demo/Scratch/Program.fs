// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.


open System 
open System.Threading
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Monads.State


open System.Drawing
open System.Windows.Forms


[<EntryPoint; STAThread>]
let main argv = 
    React.Test.run()
    Environment.Exit 0

    0 // return an integer exit code
