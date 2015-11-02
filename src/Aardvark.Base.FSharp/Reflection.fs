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

module Amd64 =
    open System
    open Aardvark.Base
    open System.Runtime.InteropServices

    let mutable NativeLogging = false
    let mutable LogFunction : nativeint -> obj[] -> unit = fun _ _ -> ()

    module Disasm =
        open System.Diagnostics
        open System.IO

        let disasm = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\libs\\Release\\disasm.exe")

        [<StructuredFormatDisplay("{AsString}")>]
        type Instruction = { offset : uint64; size : uint32; code : string; bytes : byte[] } with
            member x.AsString =
                sprintf "0x%X: %s" x.offset x.code
            member x.WithOffset (o : int) =
                { offset = x.offset + uint64 o; size = x.size; code = x.code; bytes = x.bytes }



        let private rx = System.Text.RegularExpressions.Regex("(?<offset>[0-9a-fA-F]+)[ \t]+\((?<size>[0-9]+)\)[ \t]+(?<opcode>[0-9a-fA-F]+)[ \t]+(?<name>.*)")
        let private lineBreak = System.Text.RegularExpressions.Regex("\r\n")

        let private parseAsm (line : string) =
            //0000000000000000 (04) 4883ec28                 SUB RSP, 0x28
            let m = rx.Match line
            if m.Success then
                let bytes = m.Groups.["opcode"].Value
                let arr = Array.zeroCreate (bytes.Length/2)
                for i in 0..2..bytes.Length-1 do
                    let b = sprintf "%c%c" bytes.[i] bytes.[i+1]
                    let b = Convert.ToByte(b, 16)
                    arr.[i/2] <- b

                let offset = Convert.ToUInt64(m.Groups.["offset"].Value, 16)
                let size = Convert.ToUInt32(m.Groups.["size"].Value, 10)
                let code = m.Groups.["name"].Value

                Some { offset = offset; size = size; code = code; bytes = arr }
            else 
                None

        let decompile (arr : byte[]) =
            let file = Path.GetTempFileName()

            File.WriteAllBytes(file, arr)

            let pi = ProcessStartInfo(disasm, sprintf "-b64 \"%s\"" file)
            pi.UseShellExecute <- false
            pi.RedirectStandardOutput <- true
            pi.RedirectStandardError <- true

        
            let p = Process.Start(pi)
            let result = p.StandardOutput.ReadToEnd()
            if p.WaitForExit(500) then
                if p.ExitCode = 0 then
                    let lines = lineBreak.Split(result)
                    seq {
                        for line in lines do
                            match parseAsm line with
                                | Some i -> yield i
                                | _ -> ()
                    
                        File.Delete file
                        p.Dispose()
                    }
                else
                    Seq.empty
            else
                Log.error "%s" (p.StandardError.ReadToEnd())
                Seq.empty

    type FlatArray<'a when 'a : struct>(ptr : nativeint, dataSize : int) =
        let elementSize = Marshal.SizeOf(typeof<'a>)
        let length = dataSize / elementSize
        let indices = Array.create length (-1,0)
        let mutable currentOffset = 0
        let mutable currentId = 0

        do if dataSize = 0 then printfn "uh. what is that"

        let sizeof (data : 'a[]) =
            elementSize * data.Length

        let write (offset : int) (data : 'a[]) =
            data.UnsafeCoercedApply<byte>(fun b -> Marshal.Copy(b, 0, ptr + (nativeint offset), b.Length))

        let read (id : int) =
            let (o,s) = indices.[id]
            if s <> 0 then
                let length = s / elementSize
                let result : 'a[] = Array.zeroCreate length
                result.UnsafeCoercedApply<byte>(fun b -> Marshal.Copy(ptr + (nativeint o), b, 0, b.Length))
                result
            else
                [||]

        let getNextId() =
            let id = currentId
            currentId <- id + 1
            id

        let move (start : int) (delta : int) =
            
            MSVCRT.memmove(ptr + (nativeint (start + delta)), ptr + (nativeint start), currentOffset - start)
            

        member x.Add(values : 'a[]) =
            let id = getNextId()
            let size = sizeof values

            if currentOffset + size > dataSize then
                raise <| IndexOutOfRangeException()

            indices.[id] <- (currentOffset, size)
            write currentOffset values
            currentOffset <- currentOffset + size

            id

        member x.Update(id : int, values : 'a[]) =
            let (offset, oldSize) = indices.[id]
            let size = sizeof values

            if oldSize <> size then
                let delta = size - oldSize

                if currentOffset + delta > dataSize then
                    raise <| IndexOutOfRangeException()

                move (offset + oldSize) delta
                currentOffset <- currentOffset + delta

                for i in id+1..currentId-1 do
                    let o,s = indices.[i]
                    indices.[i] <- (o + delta, s)
                indices.[id] <- (offset,size)

            write offset values

        member x.Item
            with get id = read id
            and set id v = x.Update(id, v)

        member x.Count = currentId

        member x.Address = ptr

        member x.SizeInBytes = currentOffset

        member x.Data = 
            let result : 'a[] = Array.zeroCreate (currentOffset / elementSize)
            result.UnsafeCoercedApply<byte>(fun b ->
                Marshal.Copy(ptr, b, 0, currentOffset)
            )
            result

    type FlatByteArray(ptr : nativeint, dataSize : int) =
        let length = dataSize
        let indices = Array.create length (-1,0)
        let mutable currentOffset = 0
        let mutable currentId = 0
        let lock = new System.Threading.ReaderWriterLockSlim()

        let write (offset : int) (data : byte[]) =
            Marshal.Copy(data, 0, ptr + (nativeint offset), data.Length)

        let read (id : int) =
            
            let (o,s) = indices.[id]
            if s <> 0 then
                let length = s
                let result : byte[] = Array.zeroCreate length
                lock.EnterReadLock()
                Marshal.Copy(ptr + (nativeint o), result, 0, result.Length)
                lock.ExitReadLock()
                result
            else
                [||]

        let getNextId() =
            let id = currentId
            currentId <- id + 1
            id

        let move (start : int) (delta : int) =
            MSVCRT.memmove(ptr + (nativeint (start + delta)), ptr + (nativeint start), currentOffset - start)
            

        member x.Add(values : byte[]) =
            let id = getNextId()
            let size = values.Length

            lock.EnterWriteLock()
            if currentOffset + size > dataSize then
                raise <| IndexOutOfRangeException()

            indices.[id] <- (currentOffset, size)
            write currentOffset values
            currentOffset <- currentOffset + size
            lock.ExitWriteLock()

            id
            

        member x.Update(id : int, values : byte[]) =
            if id < indices.Length then
                let (offset, oldSize) = indices.[id]
                let size = values.Length

                if oldSize <> size then
                    let delta = size - oldSize

                    if currentOffset + delta > dataSize then
                        raise <| IndexOutOfRangeException()

                    lock.EnterWriteLock()
                    move (offset + oldSize) delta
                    currentOffset <- currentOffset + delta
                    lock.ExitWriteLock()

                    for i in id+1..currentId-1 do
                        let o,s = indices.[i]
                        indices.[i] <- (o + delta, s)
                    indices.[id] <- (offset,size)

                write offset values

        member x.Item
            with get id = read id
            and set id v = x.Update(id, v)

        member x.Count = currentId

        member x.Address = ptr

        member x.SizeInBytes = currentOffset

        member x.Data = 
            let result : byte[] = Array.zeroCreate currentOffset
            Marshal.Copy(ptr, result, 0, currentOffset)
            result


    let flatArrayTest() =
        let ptr = Marshal.AllocHGlobal 1024
        let arr = FlatArray<int>(ptr, 256)

        arr.Add [|1;2;3|] |> ignore
        arr.Add [|5|] |> ignore
        arr.Add [|7|] |> ignore
             
        printfn "%A" arr.Data

        arr.[0] <- [|1;2;3;4|]
        arr.[1] <- [|5;6|]

        printfn "%A" arr.Data

        for i in 0..2 do
            printfn "%d -> %A" i arr.[i]

        arr.[1] <- [||]

        printfn "%A" arr.Data

        for i in 0..2 do
            printfn "%d -> %A" i arr.[i]


    type Program = { padding : int; mutable ptr : FlatByteArray; mutable run : unit -> unit } with
        member x.Dispose() =
            let ptr = x.ptr.Address
            let size = x.ptr.SizeInBytes

            x.ptr <- FlatByteArray(0n, 0)
            x.run <- fun () -> failwith "using disposed program"

            Kernel32.Imports.VirtualFree(ptr, UIntPtr (uint32 size), Kernel32.FreeType.Decommit) |> ignore

        member x.Code = Disasm.decompile x.ptr.Data |> Seq.map (sprintf "%A") |> String.concat "\r\n"

        interface IDisposable with
            member x.Dispose() = x.Dispose()


    type private ProgramWriter = { size : int; build : int -> byte[] -> unit }

    let write (index : int) (ptr : byte[]) (data : byte[]) =
        //Marshal.Copy(data, 0, ptr + nativeint index, data.Length)
        data.CopyTo(ptr, index)

    let private jump  (offset : byte) =
        { size = 2
          build = fun i arr ->
            //jmp +offset
            write i arr [|0xEBuy; offset - 2uy|]
        }

    let private jumpInt (offset : int) =
        { size = 5
          build = fun i arr ->
            //jmp +offset
            let bytes = BitConverter.GetBytes(offset - 5)
            write i arr ([[|0xE9uy|]; bytes] |> Array.concat)
        }

    let private nops (count : int) =
        { size = count 
          build = fun i arr ->
            if count > 2 then
                write i arr (Array.concat [[|0xEBuy; byte count - 2uy|]; (Array.create (count-2) 0x90uy)])
            elif count > 0 then
                write i arr (Array.create count 0x90uy)
        }


    let private call (ptr : nativeint) =
        { size = 12
          build = fun i arr ->
            let ptr = BitConverter.GetBytes(int64 ptr)

            //mov rax, <ptr>
            write i arr (Array.concat [[|0x48uy; 0xB8uy;|]; ptr; [|0xFFuy; 0xD0uy|]])
        }

    let private jmp (ptr : nativeint) =
        { size = 12
          build = fun i arr ->
            let ptr = BitConverter.GetBytes(int64 ptr)

            //mov rax, <ptr>
            write i arr (Array.concat [[|0x48uy; 0xB8uy;|]; ptr; [|0xFFuy; 0xE0uy|]])
        }

    let private pushret (skip : int) =
        { size = 8
          build = fun i arr ->
            let data = Array.concat [[|0x48uy; 0x8Duy; 0x5uy|]; BitConverter.GetBytes(skip + 13); [|0x50uy|]]
            //lea rax, [13+skip+rip]
            //push rax
            write i arr data
        }

    type private ProgramBuilder() =

        member x.Yield(_) = { size = 0; build = fun _ _ -> () }

        [<CustomOperation("arg")>]
        member x.SetArg(h : ProgramWriter, index : int, value : obj) : ProgramWriter =
            let self = match value with
                        | :? int64 as value -> Assembler.setArg64 index value
                        | :? int32 as value -> Assembler.setArg32 index value
                        | :? float32 as value -> Assembler.setArg32f index value
                        | _ -> failwith "unknown argument-type"

            { size = h.size + self.size; build = fun i arr -> h.build i arr; self.build (i + h.size) arr }

        [<CustomOperation("setArgs")>]
        member x.SetArgs(h : ProgramWriter, values : obj[]) : ProgramWriter =
            let mutable result = h
            let mutable index = 0
            for e in values do
                let w = match e with
                            | :? int64 as value -> Assembler.setArg64 index value
                            | :? int32 as value -> Assembler.setArg32 index value
                            | :? nativeint as value -> if IntPtr.Size = 8 then Assembler.setArg64 index (int64 value) else Assembler.setArg32 index (int value)
                            | :? float32 as value -> Assembler.setArg32f index value
                            | _ -> failwith "unknown argument-type"
                let p = result
                result <- { size = result.size + w.size; build = fun i arr -> p.build i arr; w.build (i + p.size) arr }
                index <- index + 1

            result

        [<CustomOperation("call")>]
        member x.Call(h : ProgramWriter, ptr : nativeint) : ProgramWriter =
            let self = call ptr

            { size = h.size + self.size; build = fun i arr -> h.build i arr; self.build (i + h.size) arr }

        [<CustomOperation("pad")>]
        member x.PadToLength(h : ProgramWriter, size : int) : ProgramWriter =
            if size = 0 then
                h
            else
                let m = h.size % size
                if m = 0 then
                    h
                else
                    let r = size - m
                    let pad = nops r
                    { size = h.size + r; build = fun i arr -> h.build i arr; pad.build (i + h.size) arr }

        member x.Delay(f) = f()

    let private amd64 = ProgramBuilder()

    let private concat (values : #seq<ProgramWriter>) =
        let mutable result = amd64.Yield(())
        let mutable index = 0
        for w in values do
            let p = result
            result <- { size = result.size + w.size; build = fun i arr -> p.build i arr; w.build (i + p.size) arr }
            index <- index + 1

        result


    module Instrumentation = 

        let private log(ptr : nativeint) (args : obj[]) =
            LogFunction ptr args

            //UnmanagedFunctions.callFunctionPointer ptr args

        module private CodeGen =
        
            let argTypes = [|("int", "I", "Int32"); ("int64","L", "Int64")|]
            let argCounts = [|1;2;3;4;5|]

            let rec private createCombinations (length : int) =
                if length <= 1 then
                    argTypes |> Seq.map (fun a -> [a])
                else
                    let subseq = createCombinations (length - 1)
                    seq {
                        for t in argTypes do
                            for r in subseq do
                                yield t::r
                    }

            let printLogDelegates() =
                let matches = System.Collections.Generic.List()

                for c in argCounts do
                    for args in createCombinations c do
                        let n = args.Length
                        let names = args |> List.map (fun (a,_,_) -> a) |> String.concat " * "
                        let short = args |> List.map (fun (_,a,_) -> a) |> String.concat ""
                        let patterns = args |> List.map (fun (_,_,a) -> sprintf "TypeInfo.Patterns.%s" a) |> String.concat "; "

                        let suffix = sprintf "%s" short
                        let argNames = Array.init args.Length (fun i -> sprintf "a%d" i)
                        let args = String.concat " " argNames
                        let callArgs = argNames |> Array.map (sprintf "%s :> obj") |> String.concat "; "
                        printfn "[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]"
                        printfn "type Log%s= delegate of nativeint * %s -> unit" suffix names
                        printfn "let log%s = Log%s (fun ptr %s -> log ptr [|%s|])" suffix suffix args callArgs
                        printfn "let log%sPtr = Marshal.GetFunctionPointerForDelegate log%s" suffix suffix

                        matches.Add(sprintf "        | [|%s|] -> log%sPtr" patterns suffix)

                printfn "let logPtr (args : Type[]) = "
                printfn "    match args with"
                for m in matches do
                    printfn "%s" m
                printfn "        | _ -> 0n"

        module private Log =
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogI= delegate of nativeint * int -> unit
            let logI = LogI (fun ptr a0 -> log ptr [|a0 :> obj|])
            let logIPtr = Marshal.GetFunctionPointerForDelegate logI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogL= delegate of nativeint * int64 -> unit
            let logL = LogL (fun ptr a0 -> log ptr [|a0 :> obj|])
            let logLPtr = Marshal.GetFunctionPointerForDelegate logL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogII= delegate of nativeint * int * int -> unit
            let logII = LogII (fun ptr a0 a1 -> log ptr [|a0 :> obj; a1 :> obj|])
            let logIIPtr = Marshal.GetFunctionPointerForDelegate logII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIL= delegate of nativeint * int * int64 -> unit
            let logIL = LogIL (fun ptr a0 a1 -> log ptr [|a0 :> obj; a1 :> obj|])
            let logILPtr = Marshal.GetFunctionPointerForDelegate logIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLI= delegate of nativeint * int64 * int -> unit
            let logLI = LogLI (fun ptr a0 a1 -> log ptr [|a0 :> obj; a1 :> obj|])
            let logLIPtr = Marshal.GetFunctionPointerForDelegate logLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLL= delegate of nativeint * int64 * int64 -> unit
            let logLL = LogLL (fun ptr a0 a1 -> log ptr [|a0 :> obj; a1 :> obj|])
            let logLLPtr = Marshal.GetFunctionPointerForDelegate logLL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIII= delegate of nativeint * int * int * int -> unit
            let logIII = LogIII (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logIIIPtr = Marshal.GetFunctionPointerForDelegate logIII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIIL= delegate of nativeint * int * int * int64 -> unit
            let logIIL = LogIIL (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logIILPtr = Marshal.GetFunctionPointerForDelegate logIIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILI= delegate of nativeint * int * int64 * int -> unit
            let logILI = LogILI (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logILIPtr = Marshal.GetFunctionPointerForDelegate logILI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILL= delegate of nativeint * int * int64 * int64 -> unit
            let logILL = LogILL (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logILLPtr = Marshal.GetFunctionPointerForDelegate logILL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLII= delegate of nativeint * int64 * int * int -> unit
            let logLII = LogLII (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logLIIPtr = Marshal.GetFunctionPointerForDelegate logLII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLIL= delegate of nativeint * int64 * int * int64 -> unit
            let logLIL = LogLIL (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logLILPtr = Marshal.GetFunctionPointerForDelegate logLIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLI= delegate of nativeint * int64 * int64 * int -> unit
            let logLLI = LogLLI (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logLLIPtr = Marshal.GetFunctionPointerForDelegate logLLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLL= delegate of nativeint * int64 * int64 * int64 -> unit
            let logLLL = LogLLL (fun ptr a0 a1 a2 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj|])
            let logLLLPtr = Marshal.GetFunctionPointerForDelegate logLLL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIIII= delegate of nativeint * int * int * int * int -> unit
            let logIIII = LogIIII (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logIIIIPtr = Marshal.GetFunctionPointerForDelegate logIIII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIIIL= delegate of nativeint * int * int * int * int64 -> unit
            let logIIIL = LogIIIL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logIIILPtr = Marshal.GetFunctionPointerForDelegate logIIIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIILI= delegate of nativeint * int * int * int64 * int -> unit
            let logIILI = LogIILI (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logIILIPtr = Marshal.GetFunctionPointerForDelegate logIILI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIILL= delegate of nativeint * int * int * int64 * int64 -> unit
            let logIILL = LogIILL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logIILLPtr = Marshal.GetFunctionPointerForDelegate logIILL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILII= delegate of nativeint * int * int64 * int * int -> unit
            let logILII = LogILII (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logILIIPtr = Marshal.GetFunctionPointerForDelegate logILII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILIL= delegate of nativeint * int * int64 * int * int64 -> unit
            let logILIL = LogILIL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logILILPtr = Marshal.GetFunctionPointerForDelegate logILIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILLI= delegate of nativeint * int * int64 * int64 * int -> unit
            let logILLI = LogILLI (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logILLIPtr = Marshal.GetFunctionPointerForDelegate logILLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILLL= delegate of nativeint * int * int64 * int64 * int64 -> unit
            let logILLL = LogILLL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logILLLPtr = Marshal.GetFunctionPointerForDelegate logILLL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLIII= delegate of nativeint * int64 * int * int * int -> unit
            let logLIII = LogLIII (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLIIIPtr = Marshal.GetFunctionPointerForDelegate logLIII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLIIL= delegate of nativeint * int64 * int * int * int64 -> unit
            let logLIIL = LogLIIL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLIILPtr = Marshal.GetFunctionPointerForDelegate logLIIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLILI= delegate of nativeint * int64 * int * int64 * int -> unit
            let logLILI = LogLILI (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLILIPtr = Marshal.GetFunctionPointerForDelegate logLILI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLILL= delegate of nativeint * int64 * int * int64 * int64 -> unit
            let logLILL = LogLILL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLILLPtr = Marshal.GetFunctionPointerForDelegate logLILL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLII= delegate of nativeint * int64 * int64 * int * int -> unit
            let logLLII = LogLLII (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLLIIPtr = Marshal.GetFunctionPointerForDelegate logLLII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLIL= delegate of nativeint * int64 * int64 * int * int64 -> unit
            let logLLIL = LogLLIL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLLILPtr = Marshal.GetFunctionPointerForDelegate logLLIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLLI= delegate of nativeint * int64 * int64 * int64 * int -> unit
            let logLLLI = LogLLLI (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLLLIPtr = Marshal.GetFunctionPointerForDelegate logLLLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLLL= delegate of nativeint * int64 * int64 * int64 * int64 -> unit
            let logLLLL = LogLLLL (fun ptr a0 a1 a2 a3 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj|])
            let logLLLLPtr = Marshal.GetFunctionPointerForDelegate logLLLL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIIIII= delegate of nativeint * int * int * int * int * int -> unit
            let logIIIII = LogIIIII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIIIIIPtr = Marshal.GetFunctionPointerForDelegate logIIIII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIIIIL= delegate of nativeint * int * int * int * int * int64 -> unit
            let logIIIIL = LogIIIIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIIIILPtr = Marshal.GetFunctionPointerForDelegate logIIIIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIIILI= delegate of nativeint * int * int * int * int64 * int -> unit
            let logIIILI = LogIIILI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIIILIPtr = Marshal.GetFunctionPointerForDelegate logIIILI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIIILL= delegate of nativeint * int * int * int * int64 * int64 -> unit
            let logIIILL = LogIIILL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIIILLPtr = Marshal.GetFunctionPointerForDelegate logIIILL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIILII= delegate of nativeint * int * int * int64 * int * int -> unit
            let logIILII = LogIILII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIILIIPtr = Marshal.GetFunctionPointerForDelegate logIILII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIILIL= delegate of nativeint * int * int * int64 * int * int64 -> unit
            let logIILIL = LogIILIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIILILPtr = Marshal.GetFunctionPointerForDelegate logIILIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIILLI= delegate of nativeint * int * int * int64 * int64 * int -> unit
            let logIILLI = LogIILLI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIILLIPtr = Marshal.GetFunctionPointerForDelegate logIILLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogIILLL= delegate of nativeint * int * int * int64 * int64 * int64 -> unit
            let logIILLL = LogIILLL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logIILLLPtr = Marshal.GetFunctionPointerForDelegate logIILLL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILIII= delegate of nativeint * int * int64 * int * int * int -> unit
            let logILIII = LogILIII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILIIIPtr = Marshal.GetFunctionPointerForDelegate logILIII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILIIL= delegate of nativeint * int * int64 * int * int * int64 -> unit
            let logILIIL = LogILIIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILIILPtr = Marshal.GetFunctionPointerForDelegate logILIIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILILI= delegate of nativeint * int * int64 * int * int64 * int -> unit
            let logILILI = LogILILI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILILIPtr = Marshal.GetFunctionPointerForDelegate logILILI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILILL= delegate of nativeint * int * int64 * int * int64 * int64 -> unit
            let logILILL = LogILILL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILILLPtr = Marshal.GetFunctionPointerForDelegate logILILL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILLII= delegate of nativeint * int * int64 * int64 * int * int -> unit
            let logILLII = LogILLII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILLIIPtr = Marshal.GetFunctionPointerForDelegate logILLII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILLIL= delegate of nativeint * int * int64 * int64 * int * int64 -> unit
            let logILLIL = LogILLIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILLILPtr = Marshal.GetFunctionPointerForDelegate logILLIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILLLI= delegate of nativeint * int * int64 * int64 * int64 * int -> unit
            let logILLLI = LogILLLI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILLLIPtr = Marshal.GetFunctionPointerForDelegate logILLLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogILLLL= delegate of nativeint * int * int64 * int64 * int64 * int64 -> unit
            let logILLLL = LogILLLL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logILLLLPtr = Marshal.GetFunctionPointerForDelegate logILLLL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLIIII= delegate of nativeint * int64 * int * int * int * int -> unit
            let logLIIII = LogLIIII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLIIIIPtr = Marshal.GetFunctionPointerForDelegate logLIIII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLIIIL= delegate of nativeint * int64 * int * int * int * int64 -> unit
            let logLIIIL = LogLIIIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLIIILPtr = Marshal.GetFunctionPointerForDelegate logLIIIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLIILI= delegate of nativeint * int64 * int * int * int64 * int -> unit
            let logLIILI = LogLIILI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLIILIPtr = Marshal.GetFunctionPointerForDelegate logLIILI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLIILL= delegate of nativeint * int64 * int * int * int64 * int64 -> unit
            let logLIILL = LogLIILL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLIILLPtr = Marshal.GetFunctionPointerForDelegate logLIILL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLILII= delegate of nativeint * int64 * int * int64 * int * int -> unit
            let logLILII = LogLILII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLILIIPtr = Marshal.GetFunctionPointerForDelegate logLILII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLILIL= delegate of nativeint * int64 * int * int64 * int * int64 -> unit
            let logLILIL = LogLILIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLILILPtr = Marshal.GetFunctionPointerForDelegate logLILIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLILLI= delegate of nativeint * int64 * int * int64 * int64 * int -> unit
            let logLILLI = LogLILLI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLILLIPtr = Marshal.GetFunctionPointerForDelegate logLILLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLILLL= delegate of nativeint * int64 * int * int64 * int64 * int64 -> unit
            let logLILLL = LogLILLL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLILLLPtr = Marshal.GetFunctionPointerForDelegate logLILLL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLIII= delegate of nativeint * int64 * int64 * int * int * int -> unit
            let logLLIII = LogLLIII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLIIIPtr = Marshal.GetFunctionPointerForDelegate logLLIII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLIIL= delegate of nativeint * int64 * int64 * int * int * int64 -> unit
            let logLLIIL = LogLLIIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLIILPtr = Marshal.GetFunctionPointerForDelegate logLLIIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLILI= delegate of nativeint * int64 * int64 * int * int64 * int -> unit
            let logLLILI = LogLLILI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLILIPtr = Marshal.GetFunctionPointerForDelegate logLLILI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLILL= delegate of nativeint * int64 * int64 * int * int64 * int64 -> unit
            let logLLILL = LogLLILL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLILLPtr = Marshal.GetFunctionPointerForDelegate logLLILL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLLII= delegate of nativeint * int64 * int64 * int64 * int * int -> unit
            let logLLLII = LogLLLII (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLLIIPtr = Marshal.GetFunctionPointerForDelegate logLLLII
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLLIL= delegate of nativeint * int64 * int64 * int64 * int * int64 -> unit
            let logLLLIL = LogLLLIL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLLILPtr = Marshal.GetFunctionPointerForDelegate logLLLIL
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLLLI= delegate of nativeint * int64 * int64 * int64 * int64 * int -> unit
            let logLLLLI = LogLLLLI (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLLLIPtr = Marshal.GetFunctionPointerForDelegate logLLLLI
            [<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
            type LogLLLLL= delegate of nativeint * int64 * int64 * int64 * int64 * int64 -> unit
            let logLLLLL = LogLLLLL (fun ptr a0 a1 a2 a3 a4 -> log ptr [|a0 :> obj; a1 :> obj; a2 :> obj; a3 :> obj; a4 :> obj|])
            let logLLLLLPtr = Marshal.GetFunctionPointerForDelegate logLLLLL
            let logPtr (args : Type[]) = 
                match args with
                    | [|TypeInfo.Patterns.Int32|] -> logIPtr
                    | [|TypeInfo.Patterns.Int64|] -> logLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logIIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logIILPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logILIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logILLPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logLIIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logLILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLLIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLLLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logIIIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logIIILPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logIILIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logIILLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logILIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logILILPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logILLIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logILLLPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logLIIIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logLIILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLILIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLILLPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logLLIIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logLLILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLLLIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLLLLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logIIIIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logIIIILPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logIIILIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logIIILLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logIILIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logIILILPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logIILLIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logIILLLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logILIIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logILIILPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logILILIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logILILLPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logILLIIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logILLILPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logILLLIPtr
                    | [|TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logILLLLPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logLIIIIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logLIIILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLIILIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLIILLPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logLILIIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logLILILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLILLIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLILLLPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logLLIIIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logLLIILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLLILIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLLILLPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int32|] -> logLLLIIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32; TypeInfo.Patterns.Int64|] -> logLLLILPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int32|] -> logLLLLIPtr
                    | [|TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64; TypeInfo.Patterns.Int64|] -> logLLLLLPtr
                    | _ -> 0n

        let logPtr (args : obj[]) =
            let types = args |> Array.map (fun a -> a.GetType())
            Log.logPtr types


    let private callFunction (padding : int) (ptr : nativeint) (args : obj[]) =
        if ptr = 0n then
            nops padding
        else
            if NativeLogging then
                let wrapper = Instrumentation.logPtr args
                amd64 {
                    setArgs (Array.concat [[|ptr :> obj|];args])
                    call wrapper
                    setArgs args
                    call ptr
                    pad padding
                }
            else
                amd64 {
                    setArgs args
                    call ptr
                    pad padding
                }

    let private toArray (p : ProgramWriter) =
        let arr = Array.zeroCreate p.size
        p.build 0 arr
        arr

    let private getAdditionalSize (argCount : int) =
        if argCount < 5 then 8uy
        else 
            let size = argCount * 8 - 24
            size |> byte


    let compileCall (padding : int) (ptr : nativeint, args : obj[]) =
        callFunction padding ptr args |> toArray


    let compileCalls (padding : int) (maxSize : int) (calls : #seq<nativeint * obj[]>) =
        let writers = calls |> Seq.toList |> List.map (fun (ptr,args) -> callFunction padding ptr args)

        let pro = Assembler.functionProlog 8
        let epi = Assembler.functionEpilog 8

        let size = pro.Length + epi.Length + maxSize * writers.Length
        let ptr = Kernel32.Imports.VirtualAlloc(IntPtr.Zero, UIntPtr(uint32(size)), Kernel32.AllocationType.Commit, Kernel32.MemoryProtection.ExecuteReadWrite)

        Marshal.Copy(pro, 0, ptr, pro.Length)
        let flat = FlatByteArray(ptr, size)
        flat.Add pro |> ignore

        for w in writers do
            let arr = toArray w
            flat.Add arr |> ignore

        flat.Add epi |> ignore
        { padding = padding; ptr = flat; run = UnmanagedFunctions.wrap ptr }

    let compileCallArray (padding : int) (calls : #seq<nativeint * obj[]>) =
        let writers = calls |> Seq.toList |> List.map (fun (ptr,args) -> callFunction padding ptr args)

        let size = writers |> List.sumBy (fun w -> w.size)

        let arr = Array.zeroCreate size 
        let mutable start = 0
        for w in writers do
            w.build start arr
            start <- start + w.size
        
        arr

    let updateCall (p : Program) (index : int) (ptr : nativeint) (args : obj[]) =
        let w = callFunction p.padding ptr args |> toArray
        p.ptr.[index + 1] <- w

[<AutoOpen>]
module ``String Extensions`` =
    module String =
        let private lineBreak = System.Text.RegularExpressions.Regex("\r\n")

        let indent (step : int) (s : string) =
            let parts = lineBreak.Split s
            let indent = System.String(' ', step * 4)
            let parts = parts |> Seq.map (fun l -> indent + l)
            System.String.Join("\r\n", parts)

        let lineCount (s : string) =
            let parts = lineBreak.Split s
            parts.Length

module Fsc =
    open System
    open System.Reflection
    open System.IO
    open System.Diagnostics
    open System.Threading
    open System.Collections.Generic

    let mutable currentNamespaceId = 0


    let private fscPath = @"C:\Program Files (x86)\Microsoft SDKs\F#\3.0\Framework\v4.0\Fsc.exe"

    let private referencedAssemblies() =
        let assemblies = AppDomain.CurrentDomain.GetAssemblies()

        let r = Assembly.GetExecutingAssembly().GetReferencedAssemblies() |> Array.map (fun n -> Assembly.Load n)

        let assemblies = Array.concat [assemblies; r; [|Assembly.GetExecutingAssembly(); Assembly.GetEntryAssembly(); Assembly.GetCallingAssembly()|]]
        let assemblies = HashSet(assemblies)

        
        assemblies |> Seq.choose (fun a -> 
            try 
                if a.Location.Contains "FSharp.Core" || a.Location.Contains "mscorlib" then None
                else Some a.Location 
            with _ -> 
                None) |> Seq.toArray

    let private refArgs() = 
        referencedAssemblies() |> Array.filter (fun s -> s.Length > 0) |> Array.map (sprintf "-r \"%s\"") |> String.concat " "

    let private runFsc (file : string) (output : string) =
        let referenced = refArgs()
        
        let args =   sprintf "-o:\"%s\" --debug- --optimize- --tailcalls- --platform:x64 " output
                   + sprintf "%s --target:library --warn:5 --warnaserror:76 --vserrors --utf8output --fullpaths --flaterrors --subsystemversion:6.00 " referenced
                   + sprintf "\"%s\.NETFramework,Version=v4.5.AssemblyAttributes.fs\" \"%s\"" (Path.GetTempPath()) file

        let psi = ProcessStartInfo(fscPath, args)

        psi.UseShellExecute <- false
        psi.RedirectStandardError <- true
        psi.RedirectStandardOutput <- true

        let proc = Process.Start(psi)

        proc.WaitForExit()
        if proc.ExitCode <> 0 then
            let err = proc.StandardError.ReadToEnd()
            Log.error "%A" err
            Error err
        else
            Success ()

    let private compileToDll (code : string) =
        let path = Path.GetTempFileName() + ".fs"
        let output = Path.GetTempFileName() + ".dll"
        File.WriteAllText(path, code)
        match runFsc path output with
            | Success _ -> 
                File.Delete path
                let bytes = File.ReadAllBytes output
                File.Delete output

                let ass = Assembly.Load(bytes, null, System.Security.SecurityContextSource.CurrentAssembly)
                Success ass
            | Error e -> 
                File.Delete path
                Error e

    let compileUntyped (opened : list<string>) (code : string) : Error<obj> =
        let opened = opened |> List.map (sprintf "open %s") |> String.concat "\r\n"

        let code = String.indent 2 code

        let nsId = Interlocked.Increment(&currentNamespaceId)
        let ns = sprintf "CodeDom%d" nsId
        let c = sprintf "namespace %s\r\n%s\r\nmodule Main =\r\n    let run() =\r\n        ()\r\n%s\r\n" ns opened code
        match compileToDll c with
            | Success asm ->
                let mi = asm.GetType(sprintf "%s.Main" ns).GetMethod("run")
                let value = mi.Invoke(null, [||])

                Success value
            | Error e ->
                Error e


    let compile (opened : list<string>) (code : string) : 'a =
        let opened = opened |> List.map (sprintf "open %s") |> String.concat "\r\n"

        let code = String.indent 2 code

        let nsId = Interlocked.Increment(&currentNamespaceId)
        let ns = sprintf "CodeDom%d" nsId
        let c = sprintf "namespace %s\r\n%s\r\nmodule Main =\r\n    let run() =\r\n        ()\r\n%s\r\n" ns opened code
        match compileToDll c with
            | Success asm ->
                let mi = asm.GetType(sprintf "%s.Main" ns).GetMethod("run")
                let value = mi.Invoke(null, [||])

                match value with
                    | :? 'a as r -> r
                    | _ -> failwith "unexpected type"
            | Error e ->
                failwith e



    let run (opened : list<string>) (code : string) =
        compile opened code



    let mutable field = 0

    let test() =
        let m : int -> int = compile ["Aardvark.Base.Fsc"] "fun (a : int) -> field"

        m 1 |> printfn "before: %A"

        field <- 10

        m 2 |> printfn "after: %A"

        printfn "done"
        

/// <summary>
/// Provides access to the F# interactive shell running in the current AppDomain.
/// All assemblies located in the application's current directory are automatically available.
/// ATTENTION: This currently only works on developer-machines with the F#-SDK installed.
/// </summary>
module Fsi =
    open Microsoft.FSharp.Compiler
    open Microsoft.FSharp.Compiler.Interactive.Shell
    open Microsoft.FSharp.Compiler.SourceCodeServices
    open System
    open System.IO
    open System.Threading
    open System.Text.RegularExpressions

    let mutable currentNamespaceId = 0

    type CompilerErrorType = Error | Warning

    [<StructuredFormatDisplay("{AsString}")>]
    type CompilerError = { file : string; line : int; col : int; errorType : CompilerErrorType; code : string; message : string } with
        member x.AsString =
            let typeString = match x.errorType with | Error -> "error" | Warning -> "warning"
            sprintf "%s(%d,%d): %s %s: %s" x.file x.line x.col typeString x.code x.message

    [<StructuredFormatDisplay("{AsString}")>]
    type CompilerErrorList = { errors : list<CompilerError> } with
        member x.AsString = 
            x.errors |> List.map (sprintf "%A") |> String.concat "\r\n"

    type private FsiStream() =
        inherit Stream()

        let mutable inner = new MemoryStream()

        member x.Reset() =
            let newInner = new MemoryStream()
            let old = System.Threading.Interlocked.Exchange(&inner, newInner)
            old.Dispose()

        member x.GetString() =
            let arr = inner.ToArray()
            System.Text.ASCIIEncoding.Default.GetString(arr)

        override x.CanRead = inner.CanRead
        override x.CanWrite = inner.CanWrite
        override x.CanSeek = inner.CanSeek
        override x.Length = inner.Length
        override x.Position
            with get() = inner.Position
            and set p = inner.Position <- p
        override x.Flush() = inner.Flush()
        override x.Seek(p, o) = inner.Seek(p, o)
        override x.SetLength(v) = inner.SetLength(v)
        override x.Read(buffer, offset, count) = inner.Read(buffer, offset, count)
        override x.Write(buffer, offset, count) = inner.Write(buffer, offset, count)

        interface IDisposable with
            member x.Dispose() = inner.Dispose()

    // Intialize output and input streams
    let private sbOut = new FsiStream()
    let private sbErr = new FsiStream()
    let private inStream = new StringReader("")
    let private outStream = new StreamWriter(sbOut)
    let private errStream = new StreamWriter(sbErr)

    //input.fsx(9,9): error FS0039: The value or constructor 'a' is not defined

    open FSharp.RegexProvider
    type private ErrorRx = Regex< @"\((?<line>[0-9]+),(?<col>[0-9]+)\):[ ]+(?<errorType>warning|error)[ ]+(?<code>[a-zA-Z_0-9]+):(?<message>[^$]*)$" >
    type private SplitRx = Regex< @"input\.fsx" >
    type private UselessWsRx = Regex< @"[ \t\r\n][ \t\r\n]+" >
    let private errorRx = ErrorRx()
    let private splitRx = SplitRx()
    let private uselessWsRx = UselessWsRx()

    let parseErrors (err : string) =
        
        let matches = errorRx.Matches err
        let errors = splitRx.Split(err) |> Seq.choose (fun e ->
                let m = errorRx.Match e
                if m.Success then
                    { file = "input.fsx"
                      line = System.Int32.Parse m.line.Value
                      col = System.Int32.Parse m.col.Value
                      errorType = match m.errorType.Value with | "error" -> Error | _ -> Warning
                      code = m.code.Value
                      message = m.message.Value.Replace("\r", "").Replace('\n', ' ') } |> Some
                else
                    None
            )
        errors |> Seq.toList

    type FsiResult<'a> = FsiSuccess of 'a | FsiError of CompilerErrorList

    let private getError() =
        let str = sbErr.GetString()
        sbErr.Reset()
        { errors = str |> parseErrors }

    let private getOutput() =
        let str = sbOut.GetString()
        sbOut.Reset()
        str

    let private sync = obj()
    let mutable private fsiSession = None

    let private initSession() =

        let refAsmDir, fsiDir =
            match System.Environment.OSVersion.Platform with
                | PlatformID.Unix -> "/usr/local/","/usr/local"
                | _ -> @"C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\3.0\Runtime\v4.0",
                       @"C:\Program Files (x86)\Microsoft SDKs\F#\3.0\Framework\v4.0"

        let compilerFiles = 
            match Environment.OSVersion.Platform with
                | PlatformID.Unix -> [ "lib/mono/4.5/FSharp.Build.dll"; "lib/mono/4.5/FSharp.Compiler.dll"; "lib/mono/4.5/fsiAnyCPU.exe"; "lib/mono/4.5/fsc.exe"; "lib/mono/4.5/FSharp.Compiler.Interactive.Settings.dll" ]
                | _ -> [ "FSharp.Build.dll"; "FSharp.Compiler.dll"; "FsiAnyCPU.exe"; "FsiAnyCPU.exe.config"; "Fsc.exe"; "FSharp.Compiler.Interactive.Settings.dll" ]

        let refFiles = 
            match Environment.OSVersion.Platform with
                | PlatformID.Unix -> ["lib/mono/4.5/FSharp.Core.optdata"; "lib/mono/4.5/FSharp.Core.sigdata"; "policy.2.3.FSharp.Core.dll"; "pub.config"; Path.Combine("Type Providers", "FSharp.Data.TypeProviders.dll")]
                | _ -> ["FSharp.Core.optdata"; "FSharp.Core.sigdata"; "lib/mono/4.0/policy.2.3.FSharp.Core.dll"; "lib/mono/4.5/FSharp.Data.TypeProviders.dll"]
        
        let copyFile (source : string) (target : string) =
            let target = Path.GetFileName(target)
            if not <| File.Exists target then
                let d = Path.GetDirectoryName target
                if d <> "" && not <| Directory.Exists d then
                    Directory.CreateDirectory d |> ignore

                File.Copy(source, target, true)

        compilerFiles |> List.iter (fun f ->
            let p = Path.Combine(fsiDir, f)
            if File.Exists p then
                copyFile p f
        )

        refFiles |> List.iter (fun f ->
            let p = Path.Combine(refAsmDir, f)
            if File.Exists p then
                copyFile p f
        )

        // Build command line arguments & start FSI session
        let argv = [| @"fsiAnyCpu.exe" |]
        let allArgs = Array.append argv [|"--noninteractive" |]


        let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
        let result =
            try
                FsiEvaluationSession.Create(fsiConfig, allArgs, inStream, outStream, errStream)  
            with e ->
                Log.warn "%A" e
                Unchecked.defaultof<FsiEvaluationSession>


        let seen = System.Collections.Concurrent.ConcurrentHashSet<string>()


        let addReference (path : string) =
            let esc = path.Replace("\\", "\\\\")
            try
                result.EvalInteraction("#r \"" + esc + "\"")
            with e ->
                ()


        let rec addAllReferences (a : Assembly) =
            if seen.Add a.FullName then
                try
                    addReference a.Location
                    let refs = a.GetReferencedAssemblies() |> Array.filter (fun n -> seen.Add n.FullName) |> Array.map (fun n -> Assembly.Load n)
                    for r in refs do
                        try addReference r.Location with _ -> ()
                with _ ->
                    ()

        AppDomain.CurrentDomain.AssemblyLoad.Add(fun e ->
            addAllReferences e.LoadedAssembly
            getError() |> ignore
        )

        let current = AppDomain.CurrentDomain.GetAssemblies()
        current |> Array.iter addAllReferences
        
        getError() |> ignore

        result

    let private getSession() =
        lock sync (fun () ->
            match fsiSession with
                | Some s -> s
                | None ->
                    let s = initSession()
                    fsiSession <- Some s
                    s
        )


    let evaluate text =
        try
            match getSession().EvalExpression(text) with
                | Some value -> FsiSuccess value.ReflectionValue
                | None -> FsiSuccess (() :> obj)
        with e ->
            getError() |> FsiError

    let execute text =
        try
            getSession().EvalInteraction(text)
            FsiSuccess ()
        with e ->
            getError() |> FsiError

    let addReference (path : string) =
        //Log.line "added reference to %A" (Path.GetFileName path)
        let esc = path.Replace("\\", "\\\\")
        execute ("#r \"" + esc + "\"") |> ignore
        getError() |> ignore

    let compileUntyped (opened : list<string>) (code : string) : FsiResult<obj> =
        let lineOffset = opened.Length + 3
        let colOffset = 8

        let opened = opened |> List.map (sprintf "    open %s") |> String.concat "\r\n"

        let code = String.indent 2 code

        let nsId = Interlocked.Increment(&currentNamespaceId)
        let ns = sprintf "CodeDom%d" nsId
        let c = sprintf "module %s =\r\n%s\r\n    let run() =\r\n        ()\r\n%s\r\n" ns opened code

        

        try
            getSession().EvalInteraction(c)
            match evaluate (sprintf "%s.run()" ns) with
                | FsiSuccess a -> FsiSuccess a
                | FsiError e ->
                    let errors = e.errors |> List.map (fun e -> { e with line = e.line - lineOffset; col = e.col - colOffset })
                    { errors = errors} |> FsiError
        with e ->
            let errors = getError().errors
            let errors = errors |> List.map (fun e -> { e with line = e.line - lineOffset; col = e.col - colOffset })
            { errors = errors} |> FsiError

    let compile (opened : list<string>) (code : string) : 'a=
        match compileUntyped opened code with
            | FsiSuccess o -> o |> unbox<'a>
            | FsiError e -> failwith (sprintf "%A" e)

    let compileModule (code : string) : FsiResult<Type> =
        let lineOffset = 3
        let colOffset = 8
        try

            let rx = System.Text.RegularExpressions.Regex "module[ \t\r\n]+(?<name>.+)[ \t\r\n]+="

            let m = rx.Match code
            if m.Success then
                let session = getSession()
                let moduleName = m.Groups.["name"].Value

                session.EvalInteraction(code)

                let getAss = "System.Reflection.Assembly.GetExecutingAssembly()"
                
                match session.EvalExpression(getAss) with
                    | Some value ->
                        let ass = value.ReflectionValue |> unbox<System.Reflection.Assembly>
                        match ass.GetTypes() |> Array.rev |> Array.tryPick (fun t -> let t = t.GetNestedType(moduleName) in if t <> null then Some t else None) with
                            | Some t -> FsiSuccess t
                            | None -> FsiSuccess null
                    | _ -> 
                        FsiSuccess null

                
            else
                FsiSuccess null

        with e ->
            let errors = getError().errors
            let errors = errors |> List.map (fun e -> { e with line = e.line - lineOffset; col = e.col - colOffset })
            { errors = errors} |> FsiError

    let run (opened : list<string>) (code : string) =
        compile opened code

