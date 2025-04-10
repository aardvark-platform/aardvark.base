namespace Aardvark.Base.TestLib

open Aardvark.Base

module TestLib =

    module Native =
        open System.Runtime.InteropServices

        [<DllImport("testliba")>]
        extern int foo()

    let mutable value = -1

    [<OnAardvarkInit>]
    let init() =
        value <- Native.foo()
        Report.Verbosity <- value

    let isInitialized() =
        value = 42 * 2