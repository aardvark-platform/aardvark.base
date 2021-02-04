

#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"
#load @"paket-files/build/aardvark-platform/aardvark.fake/DefaultSetup.fsx"

open System
open System.IO
open System.Diagnostics
open System.Text.RegularExpressions

open Aardvark.Fake

open Fake
open Fake.Core
open Fake.IO.Globbing.Operators

let notes = ReleaseNotes.load "RELEASE_NOTES.md"

do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultSetup.install ["src/Aardvark.sln"]

#if DEBUG
do System.Diagnostics.Debugger.Launch() |> ignore
#endif

Target.create "pushGithub" (fun _ -> 

    let packages = !!"bin/*.nupkg"
    let packageNameRx = Regex @"^(?<name>[a-zA-Z_0-9\.-]+?)\.(?<version>([0-9]+\.)*[0-9]+)(.*?)\.nupkg$"

    let myPackages = 
        packages 
        |> Seq.choose (fun p ->
            let m = packageNameRx.Match (Path.GetFileName p)
            if m.Success then 
                Some(m.Groups.["name"].Value)
            else
                None
        )
        |> Set.ofSeq


    let nupkgs = 
        myPackages |> Seq.map (fun id -> sprintf "bin/%s.%s.nupkg" id notes.NugetVersion)


    printfn "%A" (nupkgs |> Seq.toArray)
    //Fake.DotNet.Paket.pushFiles (fun s -> 
    //    { s with
    //        PublishUrl = "https://nuget.pkg.github.com/aardvark-platform/index.json"
    //    }
    //) nupkgs
)

entry()