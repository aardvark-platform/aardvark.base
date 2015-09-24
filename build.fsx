#I @"packages/FAKE/tools/"
#I @"packages/Paket.Core/lib/net45"
#I @"bin"
#r @"System.Xml.Linq"
#r @"FakeLib.dll"
#r @"Paket.Core.dll"
#r @"Aardvark.Fake.dll"


open Fake
open System
open System.IO
open System.Diagnostics
open Aardvark.Fake

do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultTargets.install ["src/Aardvark.sln"]

// start build
RunTargetOrDefault "Default"