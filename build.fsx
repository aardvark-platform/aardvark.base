

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
open Mono.Cecil

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

Target.create "CheckWitnesses" (fun _ ->

    let folder = 
        if config.debug then "Debug"
        else "Release"

    use ass = Mono.Cecil.AssemblyDefinition.ReadAssembly(Path.Combine(__SOURCE_DIRECTORY__, "bin", folder, "netstandard2.0", "Aardvark.Base.FSharp.dll"))
    
    let hasWitnesses = 
        ass.MainModule.Types |> Seq.exists (fun t ->
            if t.Name = "VecModule" then
                t.Methods |> Seq.exists (fun m ->
                    m.Name = "dot$W"
                )
            else 
                false    
        )

    if not hasWitnesses then
        failwith "Witnesses not found"
)

"Compile" ?=> "CheckWitnesses"
"CheckWitnesses" ==> "CreatePackage"


"CreatePackage" ==> "PushGithub"
"CreatePackage" ==> "PushDev"

entry()