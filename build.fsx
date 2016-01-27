#r @"System.Xml.Linq"
#r @"paket-files/build/vrvis/Aardvark.Fake/bin/Aardvark.Fake.dll"


open Fake
open System
open System.IO
open System.Diagnostics
open Aardvark.Fake

do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultTargets.install ["src/Aardvark.sln"]

Target "Tests" (fun () ->
    Fake.NUnitSequential.NUnit (fun p -> { p with ToolPath = @"packages\NUnit.Runners\tools"
                                                  ToolName = "nunit-console.exe" }) [@"bin\Release\Aardvark.Base.Incremental.Tests.exe"]
)

// start build
RunTargetOrDefault "Default"