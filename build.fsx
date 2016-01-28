#load @"paket-files\build\vrvis\Aardvark.Fake\DefaultSetup.fsx"

open Fake
open System
open System.IO
open System.Diagnostics
open Aardvark.Fake


do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultSetup.install ["src/Aardvark.sln"]

Target "Tests" (fun () ->
    Fake.NUnitSequential.NUnit (fun p -> { p with ToolPath = @"packages\NUnit.Runners\tools"
                                                  ToolName = "nunit-console.exe" }) [@"bin\Release\Aardvark.Base.Incremental.Tests.exe"]
)

#if DEBUG
do System.Diagnostics.Debugger.Launch() |> ignore
#endif


entry()