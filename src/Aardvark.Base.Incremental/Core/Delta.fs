namespace Aardvark.Base.Incremental

open System.Threading
open System.Collections.Generic
open Aardvark.Base

/// <summary>
/// a simple module for creating unique ids
/// </summary>
[<AutoOpen>]
module UniqeIds =
    let mutable private currentId = 0
    let newId() = Interlocked.Increment &currentId
