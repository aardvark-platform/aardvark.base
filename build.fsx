#I @"packages/FAKE/tools/"
#I @"packages/Aardvark.Build/lib/net45"
#I @"packages/Mono.Cecil/lib/net45"
#I @"packages/Paket.Core/lib/net45"
#r @"System.Xml.Linq"
#r @"FakeLib.dll"
#r @"Aardvark.Build.dll"
#r @"Mono.Cecil.dll"
#r @"Paket.Core.dll"
#load "bin/addSources.fsx"

open Fake
open System
open System.IO
open Aardvark.Build
open System.Text.RegularExpressions
open System.Diagnostics
open AdditionalSources

let net40 = []
let net45 = []
let core = !!"src/**/*.fsproj" ++ "src/**/*.csproj" -- "src/**/CodeGenerator.csproj";
let paketDependencies = Paket.Dependencies.Locate(__SOURCE_DIRECTORY__)
Paket.Logging.verbose <- true


Target "Install" (fun () ->
    AdditionalSources.paketDependencies.Install(false, false, false, true)
    AdditionalSources.installSources ()
)

Target "Restore" (fun () ->
    AdditionalSources.paketDependencies.Restore()
    AdditionalSources.installSources ()
)

Target "Update" (fun () ->
    AdditionalSources.paketDependencies.Update(false, false)
    AdditionalSources.installSources ()
)

Target "AddSource" (fun () ->
    let args = Environment.GetCommandLineArgs()
    let folders =
        if args.Length > 3 then
            Array.skip 3 args
        else
            failwith "no source folder given"

    AdditionalSources.addSources (Array.toList folders)
)

Target "RemoveSource" (fun () ->
    let args = Environment.GetCommandLineArgs()
    let folders =
        if args.Length > 3 then
            Array.skip 3 args
        else
            failwith "no source folder given"

    AdditionalSources.removeSources (Array.toList folders)
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


Target "RunTests" (fun () ->
    NUnit (fun p -> { p with Framework = "net-4.5"; WorkingDir = Path.GetFullPath "bin\\Release" }) ["Aardvark.Base.Incremental.Tests.dll"]
)

Target "GenerateDocs" (fun _ ->
    let source = "./help"
    let template = "./help/literate/templates/template-project.html"
    let templatesDir = "./help/templates/reference/" 
    let projInfo =
      [ "page-description", "Aardvark.Base"
        "page-author", "VRVis"
        "project-author", "VRVis"
        "github-link", "http://github.com/vrvis"
        "project-github", "http://github.com/vrvis"
        "project-nuget", "https://www.nuget.org/packages/FAKE"
        "root", "http://fsharp.github.io/FAKE"
        "project-name", "Aardvark.Base" ]


    FSharpFormatting.CreateDocs source "docs" template projInfo

    WriteStringToFile false "./docs/.nojekyll" ""

    CopyDir ("docs" @@ "content") "help/content" allFiles
    CopyDir ("docs" @@ "pics") "help/pics" allFiles
)

Target "Default" (fun () -> ())

"Restore" ==> "Compile"

"Restore" ==> 
    "CodeGenerator" ==>
    "Compile" ==>
    "Default"

//"Compile" ==> "RunTests"

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

let push url =
    let packages = !!"bin/*.nupkg"
    let packageNameRx = Regex @"(?<name>[a-zA-Z_0-9\.]+?)\.(?<version>([0-9]+\.)*[0-9]+)\.nupkg"
    let tag = Fake.Git.Information.getLastTag()

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

    try
        for id in myPackages do
            let source = sprintf "bin/%s.%s.nupkg" id tag
            let target = sprintf @"%s%s.%s.nupkg" url id tag
            File.Copy(source, target, true)
    with e ->
        traceError (string e)

Target "Push" (fun () -> push @"\\hobel.ra1.vrvis.lan\NuGet\")

let deploy (url : string) (keyName : Option<string>) =

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

   
    let accessKey =
        match keyName with
         | Some keyName -> 
            let accessKeyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh", keyName)
            if File.Exists accessKeyPath then 
                let r = Some (File.ReadAllText accessKeyPath)
                tracefn "key: %A" r.Value
                r
            else None
         | None -> None


    let branch = Fake.Git.Information.getBranchName "."
    let releaseNotes = Fake.Git.Information.getCurrentHash()
    if branch <> "master" then
        tracefn "are you really sure you want do deploy a non-master branch? (Y/N)"
        let l = Console.ReadLine().Trim().ToLower()
        if l <> "y" then failwithf "could not deploy branch: %A" branch

    let tag = Fake.Git.Information.getLastTag()

    for id in myPackages do
        let packageName = sprintf "bin/%s.%s.nupkg" id tag
        tracefn "pushing: %s" packageName
        match accessKey with
            | Some accessKey -> 
                Paket.Dependencies.Push(packageName, apiKey = accessKey, url = url)
            | None -> Paket.Dependencies.Push(packageName, url = url)



Target "Deploy" (fun () -> 
    deploy "https://www.nuget.org/api/v2/" (Some "nuget.key") 
)
Target "MyGetDeploy" (fun () -> 
    deploy "https://vrvis.myget.org/F/aardvark/api/v2" (Some "myget.key") 
)

Target "InternalDeploy" id

"MyGetDeploy" ==> "Push" ==> "InternalDeploy"

"Compile" ==> "CreatePackage"
"Compile40" ==> "CreatePackage"
"Compile45" ==> "CreatePackage"
"CreatePackage" ==> "Deploy"
"CreatePackage" ==> "MyGetDeploy"

"CreatePackage" ==> "Push"

// start build
RunTargetOrDefault "Default"

