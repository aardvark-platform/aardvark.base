#load @"paket-files/build/aardvark-platform/aardvark.fake/DefaultSetup.fsx"

open Fake
open System
open System.IO
open System.Diagnostics
open Aardvark.Fake
open Fake.Testing

do MSBuildDefaults <- { MSBuildDefaults with Verbosity = Some Minimal }
do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultSetup.install ["src/Aardvark.sln"]

Target "Tests" (fun () ->
    NUnit3 (fun p -> { p with OutputDir = "tests.out" })  (!! @"bin\Release\*.Tests.dll" ++ @"bin\Release\*.Tests.exe")
)

"Compile" ==> "Tests"

#if DEBUG
do System.Diagnostics.Debugger.Launch() |> ignore
#endif


entry()