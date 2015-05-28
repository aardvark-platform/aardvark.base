#I @"packages/FAKE/tools/"
#I @"packages/Aardvark.Build/lib/net45"
#I @"packages/Mono.Cecil/lib/net45"
#r @"System.Xml.Linq"
#r @"FakeLib.dll"
#r @"Aardvark.Build.dll"
#r @"Mono.Cecil.dll"

open Fake
open System
open System.IO
open Aardvark.Build
open System.Text.RegularExpressions

let net40 = []
let net45 = []
let core = !!"src/**/*.fsproj" ++ "src/**/*.csproj" -- "src/**/CodeGenerator.csproj";


Target "Restore" (fun () ->

    let packageConfigs = !!"src/**/packages.config" |> Seq.toList

    let sources = NuGetUtils.sources 
    for pc in packageConfigs do
        RestorePackage (fun p -> { p with OutputPath = "packages"
                                          Sources = sources
                                 }) pc


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
        "Aardvark.Data.Vrml97"
    ]


Target "CreatePackage" (fun () ->
    let checkIfGitWorks = try Fake.Git.Information.showStatus "."; true with _ -> false
    if not checkIfGitWorks 
    then traceError "could not grab git status. Possible source: no git, not a git working copy" 
    else trace "git appears to work fine."
    
    let branch = try Fake.Git.Information.getBranchName "." with e -> "master"
    let releaseNotes = Fake.Git.Information.getCurrentHash()

    let tag = Fake.Git.Information.getLastTag()

    for id in knownPackages do
        NuGetPack (fun p -> 
            { p with OutputPath = "bin"; 
                     Version = tag; 
                     ReleaseNotes = releaseNotes; 
                     WorkingDir = "bin"
            }) (sprintf "bin/%s.nuspec" id)

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
                                AccessKey = accessKey
                                Publish = true
                            })
                with e ->
                    ()
            | None ->
                traceError (sprintf "Could not find nuget access key")
     else 
        traceError (sprintf "cannot deploy branch: %A" branch)
)

Target "InstallTo" (fun () ->
    let target = environVar "InstallPath"

    match target with
        | null | "" -> 
            traceError "no output path given"
        | target ->
            if not <| Directory.Exists target then
                failwithf "directory %A not found" target
            let target = Path.GetFullPath target
            if not <| Directory.Exists target then
                failwithf "directory %A not found" target

            Run "CreatePackage"
            

            let tag = Fake.Git.Information.getLastTag()
            
            let packageOutputPath = Path.GetFullPath "bin"
            let packages = !!"bin/*.nupkg" |> Seq.filter (fun str -> str.EndsWith(tag + ".nupkg")) |> Seq.map Path.GetFullPath

            let packageNameRx = Regex @"^(?<name>.*?)\.(?<version>([0-9]+\.)*[0-9]+)$"

            tracefn "found packages: %A" packages
                      
            for p in packages do
                try
                    
                    let fileName = Path.GetFileNameWithoutExtension p
                    let packageName = packageNameRx.Match fileName

                    if packageName.Success then
                        let name = packageName.Groups.["name"].Value
                        let version = packageName.Groups.["version"].Value

                        tracefn "installing package %A (%A)" name version

                        let replace = 
                            Directory.GetDirectories(target) 
                                |> Array.map Path.GetFileName
                                |> Array.filter (fun d -> d.StartsWith name)
                                |> Array.choose (fun d -> 
                                    let m = packageNameRx.Match d
                                    if m.Success then Some (m.Groups.["version"].Value)
                                    else None
                                   )

                        for (version) in replace do
                            let installed = Path.Combine(target, name + "." + version)
                            if Directory.Exists installed then
                                Directory.Delete(installed, true)

                        RestorePackageId (fun p -> { p with Sources = [packageOutputPath; "\\\\hobel\\NuGet"]; OutputPath = target; }) name

                        let restoredPackageFolder = DirectoryInfo (Path.Combine(target, name + "." + version))

                        for (version) in replace do
                            let installed = Path.Combine(target, name + "." + version)
                            if not <| Directory.Exists installed then
                                tracefn "overriding version %A" version
                                copyRecursive restoredPackageFolder (Directory.CreateDirectory installed) true |> ignore

                        trace (sprintf "successfully reinstalled %A" p)
                    else
                        traceError (sprintf "could not get package name for: %A" fileName)

                with :? UnauthorizedAccessException as e ->
                    traceImportant (sprintf "could not reinstall %A" p  )


)


"Compile" ==> "CreatePackage"
"Compile40" ==> "CreatePackage"
"Compile45" ==> "CreatePackage"
"CreatePackage" ==> "Deploy"

// start build
RunTargetOrDefault "Default"

