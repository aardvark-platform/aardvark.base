#load @"paket-files/build/vrvis/Aardvark.Fake/DefaultSetup.fsx"

open Fake
open System
open System.IO
open System.Diagnostics
open Aardvark.Fake
open Fake.Testing

do MSBuildDefaults <- { MSBuildDefaults with Verbosity = Some Minimal }
do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultSetup.install ["src/Aardvark.sln"]

Target "Tests" (fun () ->
    NUnit3 (fun p -> { p with OutputDir = "tests.out" })  (!! @"bin\Release\*.Tests.dll" ++ @"bin\Release\*.Tests.exe")
)

"Compile" ==> "Tests"


let projInfo n =
    [ "page-description", "Aardvark.Base"
      "page-author",  "The Aardvark Platform Team" 
      "project-author",  "The Aardvark Platform Team" 
      "github-link", sprintf "https://github.com/vrvis/aardvark.base/tree/docs/src/%s" n
      "project-github", sprintf "https://github.com/vrvis/aardvark.base/tree/docs/src/%s" n
      "project-nuget", sprintf "https://www.nuget.org/packages/%s" n
      "root", "https://rawgit.com/vrvis/aardvark.base/docs/docs/api" 
      "project-name", n
    ]

let libDirs = ["--libDirs", "bin/Release"]


module MyFake =
    let CreateDocsForDlls outputDir templatesDirs projectParameters sourceRepo dllFiles = 
        let layoutRoots = String.concat " " (templatesDirs |> Seq.map (sprintf "\"%s\""))
        for file in dllFiles do
            projectParameters
            |> Seq.map (fun (k, v) -> [ k; v ])
            |> Seq.concat
            |> Seq.append 
                    ([ "metadataformat"; "--generate"; "--outdir"; outputDir; "--layoutroots"; layoutRoots;
                        "--sourceRepo"; sourceRepo; "--sourceFolder"; currentDirectory; "--parameters" ])
            |> Seq.map (fun s -> 
                    if s.StartsWith "\"" then s
                    else sprintf "\"%s\"" s)
            |> separated " "
            |> fun prefix -> sprintf "%s --dllfiles \"%s\"" prefix file
            |> Fake.FSharpFormatting.run

            printfn "Successfully generated docs for DLL %s" file

Target "API" (fun () -> 
    let projects = ["Aardvark.Base"; "Aardvark.Base.FSharp"; "Aardvark.Base.Incremental"]
    for p in projects do
        let target = sprintf "docs/api/%s" p
        if Directory.Exists target then ()
        else Directory.CreateDirectory target |> ignore
        MyFake.CreateDocsForDlls target ["docs/templates/"; "docs/templates/reference/"] (projInfo p @ libDirs) "https://github.com/vrvis/aardvark.base" [sprintf "bin/Release/%s.dll" p]
        let logo = Path.Combine(target, "logo.png")
        if File.Exists logo |> not then File.Copy("docs/logo.png", logo) |> ignore

)

Target "Docs" (fun () -> 
    Fake.FSharpFormatting.CreateDocs "docs" "docs" "docs/template.html" (projInfo "Aardvark.Base")
    Fake.FileHelper.CopyRecursive "packages/build/FSharp.Formatting.CommandTool/styles" "docs/content" true |> printfn "copied: %A"
)


"Compile" ==> "Docs"
"Compile" ==> "Docs" ==> "API"

#if DEBUG
do System.Diagnostics.Debugger.Launch() |> ignore
#endif


entry()