#if INTERACTIVE
#I @"../packages/FAKE/tools/"
#I @"../packages/Paket.Core/lib/net45"
#r @"Paket.Core.dll"
#r @"FakeLib.dll"
//do System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
namespace AdditionalSources
#else
#endif

open System.IO
open System
open System.Diagnostics
open Paket
open Fake
open System.Text.RegularExpressions
open System.IO.Compression
open System.Security.Cryptography
open System.Text

module Array =
    let skip (n : int) (a : 'a[]) =
        let res = Array.zeroCreate (a.Length - n)
        for i in n..a.Length-1 do
            res.[i - n] <- a.[i]
        res
 
[<AutoOpen>]
module PathHelpers =

    let tryDeleteDir d =
        if Directory.Exists d then
            try 
                Directory.Delete(d, true)
                true
            with e -> 
                traceError (sprintf "could not delete directory %A" d)
                false
        else
            true

    let deleteDir d =
        if Directory.Exists d then
            try Directory.Delete(d, true)
            with e -> traceError (sprintf "could not delete directory %A" d)

    let createDir d =
        if not <| Directory.Exists d then
            Directory.CreateDirectory d |> ignore

    let deleteFile f =
        if File.Exists f then
            try File.Delete f
            with e -> traceError (sprintf "could not delete %A" f)

module AdditionalSources =

    // register the logger
    Logging.event.Publish.Subscribe (fun a -> 
        match a.Level with
            | TraceLevel.Error -> traceError a.Text
            | TraceLevel.Info ->  trace a.Text
            | TraceLevel.Warning -> traceImportant a.Text
            | TraceLevel.Verbose -> traceVerbose a.Text
            | _ -> ()
    ) |> ignore

    let private packageNameRx = Regex @"(?<name>[a-zA-Z_0-9\.]+?)\.(?<version>([0-9]+\.)*[0-9]+)\.nupkg"
    let private templateIdRx = Regex @"^id[ \t]+(?<id>.*)$"
    let private templateVersionRx = Regex @"^version[ \t]+(?<version>([0-9]+\.)*[0-9]+)$"
    let private versionRx = Regex @"(?<version>([0-9]+\.)*[0-9]+).*"
    let private sourcesFileName = "sources.lock"

    // compute a base64 encoded hash for the given string
    let getHash (str : string) =
        let bytes = MD5.Create().ComputeHash(UnicodeEncoding.Unicode.GetBytes(str))
        Convert.ToBase64String(bytes).Replace('/', '_').Replace('\\', '_')

    // a hash based on the current path
    let private cacheFile = Path.Combine(Path.GetTempPath(), getHash Environment.CurrentDirectory)

    // since autorestore might overwrite our source-dependencies we simply turn it off
    let paketDependencies = Paket.Dependencies.Locate()
    do paketDependencies.TurnOffAutoRestore()

    // read a package id and version from a paket.template file
    let tryReadPackageIdAndVersion (file : string) =
        let lines = file |> File.ReadAllLines
        
        let id = 
            lines |> Array.tryPick (fun l ->
                let m = templateIdRx.Match l
                if m.Success then Some m.Groups.["id"].Value
                else None
            )

        match id with
            | Some id ->
                let version =
                    lines |> Array.tryPick (fun l ->
                        let m = templateVersionRx.Match l
                        if m.Success then Some m.Groups.["version"].Value
                        else None
                    )
                Some(id, version)
            | None -> None

    // find all created packages
    let findCreatedPackages (folder : string) =
        let files = !!Path.Combine(folder, "**", "*paket.template") |> Seq.toList
        let ids = files |> List.choose tryReadPackageIdAndVersion
        let tag = 
            try Git.Information.describe folder
            with _ -> ""

        let m = versionRx.Match tag
        if m.Success then
            let version = m.Groups.["version"].Value
            ids |> List.map (fun (id,_) -> (id,version))
        else
            ids |> List.map (fun (id,v) -> 
                    match v with
                        | Some v -> (id,v)
                        | None -> (id,"")
                   )

    // unzip a specific package to the local packages folder
    let installPackage (pkgFile : string) =
        let m = pkgFile |> Path.GetFileName |> packageNameRx.Match
        if m.Success then
            let id = m.Groups.["name"].Value
            let outputFolder = Path.Combine("packages", id) |> Path.GetFullPath
            
            createDir outputFolder

            Unzip outputFolder pkgFile
            File.Copy(pkgFile, Path.Combine(outputFolder, Path.GetFileName pkgFile), true)
            true
        else
            false

    // get the latest modification date for an entire folder
    let latestModificationDate (folder : string) =
        let file = DirectoryInfo(folder).GetFileSystemInfos("*", SearchOption.AllDirectories) |> Seq.maxBy (fun fi -> fi.LastWriteTime)
        file.LastWriteTime

    // install all packages from source-dependencies
    let installSources () =
        let sourceLines =
            if File.Exists sourcesFileName then 
                File.ReadAllLines sourcesFileName |> Array.toList
            else 
                []

        let cacheTimes = 
            if File.Exists cacheFile then 
                cacheFile |> File.ReadAllLines |> Array.choose (fun str -> match str.Split [|';'|] with [|a;b|] -> Some (a,DateTime(b |> int64)) | _ -> None) |> Map.ofArray |> ref
            else
                Map.empty |> ref

        let buildSourceFolder (folder : string) : Map<string, Version> =
            let cacheTime =
                match Map.tryFind folder !cacheTimes with
                    | Some t -> t
                    | None -> DateTime.MinValue

            let modTime = latestModificationDate folder

            let code = 
                if modTime > cacheTime then
                    shellExec { CommandLine = "/C build.cmd CreatePackage"; Program = "cmd.exe"; WorkingDirectory = folder; Args = [] }
                else
                    0

            if code <> 0 then
                failwith "failed to build: %A" folder
            else
                cacheTimes := Map.add folder DateTime.Now !cacheTimes
                let binPath = Path.Combine(folder, "bin", "*.nupkg")
                !!binPath 
                    |> Seq.choose (fun str ->
                        let m = packageNameRx.Match str
                        if m.Success then
                            Some (m.Groups.["name"].Value, Version.Parse m.Groups.["version"].Value)
                        else
                            None
                        )
                    |> Seq.groupBy fst
                    |> Seq.map (fun (id,versions) -> (id, versions |> Seq.map snd |> Seq.max))
                    |> Map.ofSeq

        let sourcePackages = sourceLines |> List.map (fun f -> f, buildSourceFolder f) |> Map.ofList
        let installedPackages = paketDependencies.GetInstalledPackages() |> List.map fst |> Set.ofList


        for (source, packages) in Map.toSeq sourcePackages do
            for (id, version) in Map.toSeq packages do
                let fileName = sprintf "%s.%s.nupkg" id (string version)
                let path = Path.Combine(source, "bin", fileName)
                let installPath = Path.Combine("packages", id)

                if tryDeleteDir installPath && installPackage path then
                    tracefn "reinstalled %A" id
                else
                    traceError <| sprintf "failed to reinstall: %A" id

        File.WriteAllLines(cacheFile, !cacheTimes |> Map.toSeq |> Seq.map (fun (a, time) -> sprintf "%s;%d" a time.Ticks))

    // add source-dependencies
    let addSources (folders : list<string>) = 
        
        let folders = folders |> List.filter Directory.Exists
        match folders with
            | [] -> 
                traceImportant "no sources found"
            | folders ->
                let taskName = sprintf "adding sources: %A" folders
                traceStartTask taskName taskName

                traceVerbose "reading sources.lock"
                let sourceFolders =
                    if File.Exists sourcesFileName then 
                        File.ReadAllLines sourcesFileName |> Set.ofArray
                    else 
                        Set.empty

                let newSourceFolders = Set.union (Set.ofList folders) sourceFolders

                traceVerbose "writing to sources.lock"
                File.WriteAllLines(sourcesFileName, newSourceFolders)

                trace "restoring missing packages"
                try paketDependencies.Restore()
                with e -> traceError (sprintf "failed to restore packages: %A" e.Message)

                installSources()

                traceEndTask taskName taskName

    // remove source dependencies
    let removeSources (folders : list<string>) =
        let sourceFolders =
            if File.Exists sourcesFileName then 
                File.ReadAllLines sourcesFileName |> Set.ofArray
            else 
                Set.empty

        let newSourceFolders = Set.difference sourceFolders (Set.ofList folders)

        if Set.isEmpty newSourceFolders then
            deleteFile sourcesFileName
        else
            File.WriteAllLines(sourcesFileName, newSourceFolders)

        for f in folders do
            let packages = findCreatedPackages f
            for (id,v) in packages do
                let path = Path.Combine("packages", id)
                deleteDir path


        paketDependencies.Restore()
        installSources()


        