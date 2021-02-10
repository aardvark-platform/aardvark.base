

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
open Fake.Core.TargetOperators

do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultSetup.install ["src/Aardvark.sln"]

#if DEBUG
do System.Diagnostics.Debugger.Launch() |> ignore
#endif

Target.create "PushGithub" (fun _ -> 

    let tag = getVersion()

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
        myPackages |> Seq.map (fun id -> sprintf "bin/%s.%s.nupkg" id tag)

    Fake.DotNet.Paket.pushFiles (fun s -> 
        { s with
            PublishUrl = "https://nuget.pkg.github.com/aardvark-platform"
            WorkingDir = "."
        }
    ) nupkgs
)

Target.create "PushDev" (fun _ -> 

    DefaultSetup.push ["https://vrvis.myget.org/F/aardvark_public/api/v2" ,"public.key"]
    
)

"CreatePackage" ==> "PushGithub"
"CreatePackage" ==> "PushDev"

entry()