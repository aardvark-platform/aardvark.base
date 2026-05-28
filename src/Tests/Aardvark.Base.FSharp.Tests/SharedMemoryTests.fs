namespace Aardvark.Base.FSharp.Tests

open System.IO
open System.Reflection
open System.Runtime.InteropServices
open System.Text
open Aardvark.Base
open System
open System.Diagnostics

open NUnit.Framework

#nowarn "9"

module SharedMemoryTests =

    type Span<'T> with
        member inline this.AsReadOnlySpan() : ReadOnlySpan<'T> = Span.op_Implicit this

    let run (argv: string[]) =
        try
            if argv.Length > 0 then
                match argv.[0] with
                | "--read-shared-mem" ->
                    let name = argv.[1]
                    let size = int64 argv.[2]
                    let access = enum<SharedMemoryAccess> <| int argv.[3]
                    let bytesToRead = int argv.[4]

                    use shm = SharedMemory.Open(name, size, access)
                    let srcSpan = ReadOnlySpan<byte>(shm.Pointer.ToPointer(), bytesToRead)

                    let charCount = Encoding.UTF8.GetCharCount srcSpan
                    let dst = Array.zeroCreate<char> charCount
                    let dstSpan = Span<char>(dst)

                    let count = Encoding.UTF8.GetChars(srcSpan, dstSpan)
                    let data = String(dstSpan.Slice(0, count).AsReadOnlySpan())

                    printfn $"{data}"

                | "--write-shared-mem" ->
                    let name = argv.[1]
                    let size = int64 argv.[2]
                    let access = enum<SharedMemoryAccess> <| int argv.[3]
                    let data = argv.[4]

                    use shm = SharedMemory.Open(name, size, access)

                    let srcSpan = data.AsSpan()
                    let dstSpan = Span<byte>(shm.Pointer.ToPointer(), int shm.Size)

                    let count = Encoding.UTF8.GetBytes(srcSpan, dstSpan)
                    printfn $"{count}"

                | _ ->
                    Environment.Exit 1

                Environment.Exit 0
        with exn ->
            eprintfn $"{exn}"
            reraise()

    module private Process =

        let private start (output: bool -> string -> unit) (args: string) =
            let path =
                let assembly = Assembly.GetExecutingAssembly()

                if RuntimeInformation.IsOSPlatform OSPlatform.Windows then
                    Path.ChangeExtension(assembly.Location, "exe")
                else
                    Path.ChangeExtension(assembly.Location, null)

            if not <| File.Exists path then
                raise <| FileNotFoundException($"Executable not found: {path}", path)

            let p = new Process()
            p.StartInfo.FileName <- path
            p.StartInfo.Arguments <- args
            p.StartInfo.RedirectStandardOutput <- true
            p.StartInfo.RedirectStandardError <- true
            p.StartInfo.UseShellExecute <- false
            p.StartInfo.CreateNoWindow <- true

            p.OutputDataReceived.Add (fun args ->
                if not <| String.IsNullOrWhiteSpace args.Data then
                    output false args.Data
            )
            p.ErrorDataReceived.Add (fun args ->
                if not <| String.IsNullOrWhiteSpace args.Data then
                    output true args.Data
            )

            p.Start() |> ignore
            p.BeginOutputReadLine()
            p.BeginErrorReadLine()

            p

        let readSharedMemory (access: SharedMemoryAccess) (bytesToRead: int) (shm: ISharedMemory) =
            let result = MVar.empty()
            let timeout = TimeSpan.FromSeconds 10.0

            let errors = ResizeArray<string>()

            let output isError (str: string) =
                if isError then
                    errors.Add str
                else
                    str.Trim() |> MVar.put result

            use p = start output $"--read-shared-mem {shm.Name} {shm.Size} {int access} {bytesToRead}"
            p.WaitForExit()

            if errors.Count > 0 then
                let errors = String.concat Environment.NewLine errors
                failwith $"Read subprocess failed: {errors}"

            Assert.AreEqual(0, p.ExitCode)

            match result.TryTake timeout with
            | Some r -> r
            | _ -> raise <| TimeoutException("Failed to wait for read subprocess")

        let writeSharedMemory (access: SharedMemoryAccess) (data: string) (shm: ISharedMemory)  =
            let bytesWritten = MVar.empty()
            let timeout = TimeSpan.FromSeconds 10.0

            let errors = ResizeArray<string>()

            let output isError (str: string) =
                if isError then
                    errors.Add str
                else
                    str.Trim() |> int |> MVar.put bytesWritten

            use p = start output $"--write-shared-mem {shm.Name} {shm.Size} {int access} \"{data}\""
            p.WaitForExit()

            if errors.Count > 0 then
                let errors = String.concat Environment.NewLine errors
                failwith $"Write subprocess failed: {errors}"

            Assert.AreEqual(0, p.ExitCode)

            match bytesWritten.TryTake timeout with
            | Some r -> r
            | _ -> raise <| TimeoutException("Failed to wait for write subprocess")

    [<Test>]
    let ``[SharedMemory] Read and write``() =
        use shm = SharedMemory.Create(4096L)

        let data = "Hello my dudes!"
        let written = shm |> Process.writeSharedMemory SharedMemoryAccess.Write data
        Assert.AreEqual(Encoding.UTF8.GetByteCount(data), written)

        let result = shm |> Process.readSharedMemory SharedMemoryAccess.Read written
        Assert.AreEqual(data, result)

    [<Test>]
    let ``[SharedMemory] Read-only``() =
        use shm = SharedMemory.Create(512L, SharedMemoryAccess.Read)

        let result = shm |> Process.readSharedMemory SharedMemoryAccess.Read 512
        Assert.AreEqual(512, result.Length)

    [<Test>]
    let ``[SharedMemory] Create existing``() =
        use shm1 = SharedMemory.Create(512L)

        let createFail() =
            use shm2 = SharedMemory.Create(shm1.Name, 128L)
            ()

        Assert.That(createFail, Throws.Exception)