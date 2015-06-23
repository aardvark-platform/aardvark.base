#I @"packages/FAKE/tools/"
#I @"packages/Aardvark.Build/lib/net45"
#I @"packages/Mono.Cecil/lib/net45"
#I @"packages/Paket.Core/lib/net45"
#r @"System.Xml.Linq"
#r @"FakeLib.dll"
#r @"Aardvark.Build.dll"
#r @"Mono.Cecil.dll"
#r @"Paket.Core.dll"

open Fake
open System
open System.IO
open Aardvark.Build
open System.Text.RegularExpressions
open System.Diagnostics

let net40 = []
let net45 = []
let core = !!"src/**/*.fsproj" ++ "src/**/*.csproj" -- "src/**/CodeGenerator.csproj";
let paketDependencies = Paket.Dependencies.Locate(__SOURCE_DIRECTORY__)
Paket.Logging.verbose <- true

Paket.Logging.event.Publish.Subscribe (fun a -> 
    match a.Level with
        | TraceLevel.Error -> traceError a.Text
        | TraceLevel.Info -> trace a.Text
        | TraceLevel.Warning -> traceImportant a.Text
        | TraceLevel.Verbose -> trace a.Text
        | _ -> ()
) |> ignore

Target "Restore" (fun () ->
    paketDependencies.Restore()
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

Target "CreatePackage" (fun () ->
    let releaseNotes = try Fake.Git.Information.getCurrentHash() |> Some with _ -> None
    if releaseNotes.IsNone then 
        //traceError "could not grab git status. Possible source: no git, not a git working copy"
        failwith "could not grab git status. Possible source: no git, not a git working copy"
    else 
        trace "git appears to work fine."
    
    let releaseNotes = releaseNotes.Value
    let branch = try Fake.Git.Information.getBranchName "." with e -> "master"

    let tag = Fake.Git.Information.getLastTag()
    paketDependencies.Pack("bin", version = tag, releaseNotes = releaseNotes)
)

Target "Deploy" (fun () ->

    let packages = !!"bin/*.nupkg"
    let packageNameRx = Regex @"(?<name>[a-zA-Z_0-9\.]+?)\.(?<version>([0-9]+\.)*[0-9]+)\.nupkg"

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
                    for id in myPackages do
                        Paket.Dependencies.Push(sprintf "bin/%s.%s.nupkg" id tag, apiKey = accessKey)
                with e ->
                    traceError (string e)
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

