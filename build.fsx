#I @"packages/FAKE/tools/"
#r @"FakeLib.dll"

open Fake
open System
open System.IO

let net40 = ["src/Aardvark.Base/Aardvark.Base.csproj"]
let net45 = ["src/Aardvark.Base/Aardvark.Base.csproj"]
let core = !!"src/**/*.fsproj" ++ "src/**/*.csproj" -- "src/**/CodeGenerator.csproj";

printfn "%A" (core |> Seq.toList)

Target "Restore" (fun () ->

    let packageConfigs = !!"src/**/packages.config" |> Seq.toList

    let defaultNuGetSources = RestorePackageHelper.RestorePackageDefaults.Sources
    for pc in packageConfigs do
        RestorePackage (fun p -> { p with OutputPath = "packages" }) pc


)

Target "Clean" (fun () ->
    DeleteDir (Path.Combine("bin", "Release"))
    DeleteDir (Path.Combine("bin", "Release 4.0"))
    DeleteDir (Path.Combine("bin", "Release 4.5"))
    DeleteDir (Path.Combine("bin", "Debug"))
)

Target "CodeGenerator" (fun () ->
    MSBuildRelease "bin/Release" "Build" (!!"src/**/CodeGenerator.csproj") |> ignore
)


Target "Compile40" (fun () ->
    MSBuild "bin/net40" "Build" ["Configuration", "Release 4.0"] net40 |> ignore
)

Target "Compile45" (fun () ->
    MSBuild "bin/net45" "Build" ["Configuration", "Release 4.5"] net45 |> ignore
)

Target "Compile" (fun () ->
    MSBuildRelease "bin/Release" "Build" core |> ignore
)



Target "Default" (fun () -> ())

"Restore" ==> "Compile"

"Restore" ==> 
    "CodeGenerator" ==>
    "Compile" ==>
    "Default"

"Restore" ==> 
    "CodeGenerator"
    "Compile40"

"Restore" ==> 
    "CodeGenerator"
    "Compile45"

let knownPackages = 
    Set.ofList [
        "Aardvark.Base"
        "Aardvark.Base.FSharp"
        "Aardvark.Base.Essentials"
        "Aardvark.Base.Incremental"
    ]


Target "CreatePackage" (fun () ->
    let branch = Fake.Git.Information.getBranchName "."
    let releaseNotes = Fake.Git.Information.getCurrentHash()

    if branch = "master" then
        let tag = Fake.Git.Information.getLastTag()

        for id in knownPackages do
            NuGetPack (fun p -> 
                { p with OutputPath = "bin"; 
                         Version = tag; 
                         ReleaseNotes = releaseNotes; 
                         WorkingDir = "bin"
                         Dependencies = p.Dependencies |> List.map (fun (id,version) -> if Set.contains id knownPackages then (id, tag) else (id,version)) 
                }) (sprintf "bin/%s.nuspec" id)
    
    else 
        traceError (sprintf "cannot create package for branch: %A" branch)
)

Target "Deploy" (fun () ->

    let accessKeyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh", "nuget.key")
    let accessKey =
        if File.Exists accessKeyPath then Some (File.ReadAllText accessKeyPath)
        else None

    let branch = Fake.Git.Information.getBranchName "."
    let releaseNotes = Fake.Git.Information.getCurrentHash()
    if branch = "master" then
        let tag = Fake.Git.Information.getLastTag()
        match accessKey with
            | Some accessKey ->
                try
                    for id in knownPackages do
                        NuGetPublish (fun p -> 
                            { p with 
                                Project = id
                                OutputPath = "bin"
                                Version = tag; 
                                ReleaseNotes = releaseNotes; 
                                WorkingDir = "bin"
                                Dependencies = p.Dependencies |> List.map (fun (id,version) -> if Set.contains id knownPackages then (id, tag) else (id,version)) 
                                AccessKey = accessKey
                                Publish = true
                            })
                with e ->
                    ()
            | None ->
                ()
     else 
        traceError (sprintf "cannot deploy branch: %A" branch)
)

"Compile" ==> "CreatePackage"
"Compile40" ==> "CreatePackage"
"Compile45" ==> "CreatePackage"
"CreatePackage" ==> "Deploy"

// start build
RunTargetOrDefault "Default"

