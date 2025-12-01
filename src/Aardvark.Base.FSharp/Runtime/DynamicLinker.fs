#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

open System.Linq.Expressions
open System.Runtime.InteropServices
open Microsoft.FSharp.Reflection
open System.Reflection
open System.Reflection.Emit
open System

/// <summary>
/// Declares the basic interface for a dynamic linker provided by the OS.
/// </summary>
module DynamicLinkerTypes =
    type IDynamicLinker =
        abstract member LoadLibrary : string -> nativeint
        abstract member FreeLibrary : nativeint -> unit
        abstract member GetProcAddress : nativeint -> string -> nativeint

    type Function internal(ptr : nativeint) =
        member x.Handle = ptr

    type Library internal(ptr : nativeint, linker : IDynamicLinker) =
        let mutable ptr = ptr

        interface IDisposable with
            member x.Dispose() = x.Dispose()


        member x.Dispose() =
            if ptr <> 0n then
                linker.FreeLibrary ptr |> ignore
                ptr <- 0n

        member x.Handle = ptr

        member x.TryFindFunction (name : string) =
            if ptr = 0n then
                None
            else
                let ptr = linker.GetProcAddress ptr name
                if ptr <> 0n then
                    Function(ptr) |> Some
                else
                    None

        member x.GetFunction (name : string) =
            if ptr <> 0n then
                Function (linker.GetProcAddress ptr name)
            else
                raise <| ObjectDisposedException("Library")

/// <summary>
/// DynamicLinker provides platform independent functions for loading libraries and
/// resolving function-pointers within those libraries.
/// Note that MacOSX is currently not supported
/// </summary>
module DynamicLinker =
    open DynamicLinkerTypes

    let private os = System.Environment.OSVersion

    let notimp() = raise <| NotImplementedException()

    
    let private linker =
        { new IDynamicLinker with
            member x.LoadLibrary(name : string) = Aardvark.LoadLibrary(name, Assembly.GetCallingAssembly())
            member x.FreeLibrary(address : nativeint) = ()
            member x.GetProcAddress (handle : nativeint) (name : string) = Aardvark.GetProcAddress(handle, name)
        }

    let tryLoadLibrary (name : string) =
        let ptr = Aardvark.LoadLibrary(name, Assembly.GetCallingAssembly())
        if ptr <> 0n then new Library(ptr, linker) |> Some
        else None

    let loadLibrary (name : string) =
        let ptr = Aardvark.LoadLibrary(name, Assembly.GetCallingAssembly())
        new Library(ptr, linker)

    let tryLoadFunction (name : string) (lib : Library) =
        lib.TryFindFunction name

    let tryUnpackNativeLibrary (name : string) =
        let resname = name + ".zip"
        let resourceInfo =
            Introspection.AllAssemblies |> Seq.tryPick (fun a ->
                let r = a.GetManifestResourceInfo(resname)
                if not (isNull r) then Some (a,r)
                else None
            )

        match resourceInfo with
            | Some (ass,res) ->
                use stream = ass.GetManifestResourceStream(resname)
                use archive = new System.IO.Compression.ZipArchive(stream)

                let osName =
                    match os with
                        | Windows -> "Windows"
                        | Linux -> "Linux"
                        | Mac -> "Mac"

                let archName =
                    if System.IntPtr.Size = 8 then "AMD64"
                    else "x86"

                let resName = osName + "_" + archName + "/"

                let entries = archive.Entries |> Seq.filter (fun e -> e.FullName.StartsWith resName) |> Seq.toList

                match entries with
                    | [] -> false
                    | entries ->
                        for entry in entries do
                            use stream = entry.Open()
                            let data = Array.zeroCreate (int entry.Length)
                            let mutable read = 0
                            while read < data.Length do
                                let r = stream.Read(data, read, data.Length - read)
                                read <- read + r

                            let name = entry.Name
                            try
                                System.IO.File.WriteAllBytes(name, data)
                            with | :? System.IO.IOException as e -> 
                                Log.line "could not unpack native dependency %s because: %s" name e.Message
                        true


            | None ->
                false