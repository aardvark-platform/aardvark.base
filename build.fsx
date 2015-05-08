#I @"packages/FAKE/tools/"
#r @"FakeLib.dll"

open Fake
open System
open System.IO

let net40 = []
let net45 = []
let core = !!"src/**/*.fsproj" ++ "src/**/*.csproj" -- "src/**/CodeGenerator.csproj";

printfn "%A" (core |> Seq.toList)

let packageRx = System.Text.RegularExpressions.Regex @"(?<name>.*?)\.(?<version>([0-9]+\.)*[0-9]+)\.nupkg$"

let updatePackages (sources : list<string>) (projectFiles : #seq<string>) =
    let packages = 
        sources |> List.collect (fun source ->  
            Directory.GetFiles(source,"*.nupkg") 
                |> Array.choose (fun s -> 
                    let m = Path.GetFileName s |> packageRx.Match in if m.Success then m.Groups.["name"].Value |> Some else None
                ) 
                |> Array.toList
        ) |> Set.ofList

    if Set.count packages <> 0 then
        for project in projectFiles do
            project |> Fake.NuGet.Update.NugetUpdate (fun p ->  
                { p with 
                    Ids = packages |> Set.intersect (Set.ofList p.Ids) |> Set.toList
                    //ToolPath = @"E:\Development\aardvark-2015\tools\NuGet\nuget.exe"
                    RepositoryPath = "packages"
                    Sources = sources @ Fake.NuGet.Install.NugetInstallDefaults.Sources
                    //Prerelease = true
                } 
            )

Target "Restore" (fun () ->

    let packageConfigs = !!"src/**/packages.config" |> Seq.toList

    let addtionalSources = (environVarOrDefault "AdditionalNugetSources" "").Split([|";"|],StringSplitOptions.RemoveEmptyEntries) |> Array.toList
    let defaultNuGetSources = RestorePackageHelper.RestorePackageDefaults.Sources
    for pc in packageConfigs do
        RestorePackage (fun p -> { p with OutputPath = "packages"
                                          Sources = addtionalSources @ defaultNuGetSources  
                                 }) pc

    updatePackages addtionalSources  (!!"src/**/*.csproj" ++ "src/**/*.fsproj")


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

"Compile" ==> "CreatePackage"
"Compile40" ==> "CreatePackage"
"Compile45" ==> "CreatePackage"
"CreatePackage" ==> "Deploy"

// start build
RunTargetOrDefault "Default"

