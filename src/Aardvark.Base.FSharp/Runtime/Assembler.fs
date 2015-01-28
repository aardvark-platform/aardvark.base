#if INTERACTIVE
#r "..\\..\\..\\Bin\\Debug\\Aardvark.Base.dll"
#else
namespace Aardvark.Base
#endif

open System.Linq.Expressions
open System.Runtime.InteropServices
open Microsoft.FSharp.Reflection
open System.Reflection
open System

// ==============================================================================================
// The native code-generation must be implemented for every
// architecture/platform combination independently due to
// different calling conventions. 
//
// Configurations:
//
// +----------------+----------------+----------+
// |  Architecture  |    Platform    |  State   |
// +----------------+----------------+----------+
// |      x64       |     Windows    |  test    |
// |      x64       |      Linux     |  test    |
// |      x64       |       Mac      |  TODO    |
// |      arm       |      Linux     |  TODO    |
// +----------------+----------------+----------+
//
// ==============================================================================================

module Amd64Base =
    type ArrayWriter = { size : int; build : int -> byte[] -> unit } with
        static member Empty = { size = 0; build = fun _ _ -> () }

    let write (index : int) (ptr : byte[]) (data : byte[]) =
        data.CopyTo(ptr, index)

    let toArray (p : ArrayWriter) =
        let arr = Array.zeroCreate p.size
        p.build 0 arr
        arr

    let jump  (offset : byte) =
        { size = 2
          build = fun i arr ->
            //jmp +offset
            write i arr [|0xEBuy; offset - 2uy|]
        }

    let jumpInt (offset : int) =
        { size = 5
          build = fun i arr ->
            //jmp +offset
            let bytes = BitConverter.GetBytes(offset - 5)
            write i arr ([[|0xE9uy|]; bytes] |> Array.concat)
        }

    let nops (count : int) =
        { size = count 
          build = fun i arr ->
            if count > 2 then
                write i arr (Array.concat [[|0xEBuy; byte count - 2uy|]; (Array.create (count-2) 0x90uy)])
            elif count > 0 then
                write i arr (Array.create count 0x90uy)
        }


    let call (ptr : nativeint) =
        { size = 12
          build = fun i arr ->
            let ptr = BitConverter.GetBytes(int64 ptr)

            //mov rax, <ptr>
            write i arr (Array.concat [[|0x48uy; 0xB8uy;|]; ptr; [|0xFFuy; 0xD0uy|]])
        }

    let jmp (ptr : nativeint) =
        { size = 12
          build = fun i arr ->
            let ptr = BitConverter.GetBytes(int64 ptr)

            //mov rax, <ptr>
            write i arr (Array.concat [[|0x48uy; 0xB8uy;|]; ptr; [|0xFFuy; 0xE0uy|]])
        }

    let pushret (skip : int) =
        { size = 8
          build = fun i arr ->
            let data = Array.concat [[|0x48uy; 0x8Duy; 0x5uy|]; BitConverter.GetBytes(skip + 13); [|0x50uy|]]
            //lea rax, [13+skip+rip]
            //push rax
            write i arr data
        }

    let concat (values : #seq<ArrayWriter>) =
        let mutable result = ArrayWriter.Empty
        let mutable index = 0
        for w in values do
            let p = result
            result <- { size = result.size + w.size; build = fun i arr -> p.build i arr; w.build (i + p.size) arr }
            index <- index + 1

        result
  
module Amd64Linux =
    open Amd64Base

    let private argMovs64 = [| [| 0x48uy; 0xBFuy |]     //mov %rdi
                               [| 0x48uy; 0xBEuy |]     //mov %rsi
                               [| 0x48uy; 0xBAuy |]     //mov %rdx
                               [| 0x48uy; 0xB9uy |]     //mov %rcx
                               [| 0x49uy; 0xB8uy |]     //mov %r8
                               [| 0x49uy; 0xB9uy |]     //mov %r9
                            |]

    let private argMovs32 = [| [| 0xBFuy |]             //mov %edi
                               [| 0xBEuy |]             //mov %esi
                               [| 0xBAuy |]             //mov %edx
                               [| 0xB9uy |]             //mov %ecx
                               [| 0x41uy; 0xB8uy |]     //mov %r8d
                               [| 0x41uy; 0xB9uy |]     //mov %r9d
                            |]

    let private setArg64Internal (index : int) (value : int64) =
        if index < argMovs64.Length then
            let data = [argMovs64.[index]; BitConverter.GetBytes(value)] |> Array.concat
            { size = data.Length; build = fun i a -> write i a data }
        else
            //movq value, 8*index(%rsp) doesn't work for some reason
            //gcc splits the mov into two 32bit movs:
            //c7 44 24 0c 80 00 00 00 = movl   $0x80,0xc(%rsp)
            //c7 04 24 00 00 00 00    = movl   $0x0,(%rsp)
            //c7 44 24 04 70 00 00 00 = movl   $0x70,0x4(%rsp)
            failwith "linux amd64 assembler currently only supports 6 arguments"

    let private setArg32Internal (index : int) (value : int) =
        if index < argMovs32.Length then
            let data = [argMovs32.[index]; BitConverter.GetBytes(value)] |> Array.concat
            { size = data.Length; build = fun i a -> write i a data }
        else
            failwith "linux amd64 assembler currently only supports 6 arguments"


    let setArg32 (index : int) (arg : int32) =
        { size = if index < 4 then 5 else 6
          build = fun i arr ->
            let bytes = BitConverter.GetBytes arg

            if index >= 6 then 
                failwith "not spported in AMD64 linux assembler ATM"
            else
                let mov = argMovs32.[index]
                write i arr (Array.concat [mov; bytes])
        }

    let setArg64 (index : int) (arg : int64) =
        { size = 10
          build = fun i arr ->
            let bytes = BitConverter.GetBytes arg

            if index >= 6 then 
                failwith "not spported in AMD64 linux assembler ATM"
            else
                let mov = argMovs64.[index]
                write i arr (Array.concat [mov; bytes])
        }

    let private compileCallInternal (f : nativeint) (args : obj[]) =
        let argSetters =
            args |> Array.mapi(fun i a ->
                match a with    
                    | :? int as a -> setArg32Internal i a // todo replace by non internal version
                    | :? int64 as a -> setArg64Internal i a
                    | :? nativeint as a -> setArg64Internal i (int64 a)
                    | _ -> failwithf "unsupported argument: %A" a
            )

        let call = call f
        [argSetters; [|call|]] |> Array.concat |> concat


    let functionProlog argCount = [||]

    let functionEpilog argCount = [|0xC3uy|]


    let compileCall (f : nativeint) (args : obj[]) =
        let writer = compileCallInternal f args
        let arr = Array.zeroCreate writer.size
        writer.build 0 arr
        arr

    let compileCalls (calls : seq<nativeint * obj[]>) =
        let writer = calls |> Seq.map (fun (f,a) -> compileCallInternal f a) |> concat
        let arr = Array.zeroCreate writer.size
        writer.build 0 arr
        arr

module Amd64Windows =
    open Amd64Base

    let private prolog (additionalSpace : byte) =
        { size = 4; build = fun i arr -> write i arr [| 0x48uy; 0x83uy; 0xECuy; 0x20uy + additionalSpace |] }

    let private epilog (additionalSpace : byte) =
        { size = 5; build = fun i arr -> write i arr [| 0x48uy; 0x83uy; 0xC4uy; 0x20uy + additionalSpace; 0xC3uy|] }

    let private argumentMove64 = [| [| 0x48uy; 0xB9uy |]    //movq rcx, ...
                                    [| 0x48uy; 0xBAuy |]    //movq rdx, ...
                                    [| 0x49uy; 0xB8uy |]    //movq r8, ...
                                    [| 0x49uy; 0xB9uy |]    //movq r9, ...
                                 |]

    let private argumentMove32 = [| [| 0xB9uy |]            //movd rcx, ...
                                    [| 0xBAuy |]            //movd rdx, ...
                                    [| 0x41uy; 0xB8uy  |]   //movd r8, ...
                                    [| 0x41uy; 0xB9uy  |]   //movd r9, ...
                                 |]

    let setArg64 (index : int) (arg : int64) =
        { size = if index < 4 then 10 else 15
          build = fun i arr ->
            let bytes = BitConverter.GetBytes arg

            if index >= 4 then 
                //push the argument on the stack

                //mov rax, <arg>
                //mov [rsp+32 + <index>*8], rax
                let offset = (byte index) * 8uy;
                write i arr (Array.concat [ [| 0x48uy; 0xB8uy; |]; bytes; [| 0x48uy; 0x89uy; 0x44uy; 0x24uy; offset |] ])
            else
                let mov = argumentMove64.[index]
                write i arr (Array.concat [mov; bytes])
        }

    let setArg32 (index : int) (arg : int) =
        { size = if index < 2 then 5 elif index < 4 then 6 else 8
          build = fun i arr -> 
            let bytes = BitConverter.GetBytes arg

            if index >= 4 then
                //mov [rsp+32 + <index>*8], DWORD <arg>
                let offset = (byte index) * 8uy;
                write i arr (Array.concat [ [| 0xC7uy; 0x44uy; 0x24uy; offset |]; bytes])
            else
                let mov = argumentMove32.[index]
                write i arr  (Array.concat [ mov; bytes])
        }


    let private compileCallInternal (f : nativeint) (args : obj[]) =
        let argSetters =
            args |> Array.mapi(fun i a ->
                match a with    
                    | :? int as a -> setArg32 i a
                    | :? int64 as a -> setArg64 i a
                    | :? nativeint as a -> setArg64 i (int64 a)
                    | _ -> failwithf "unsupported argument: %A" a
            )

        let call = call f
        [argSetters; [|call|]] |> Array.concat |> concat

    let private getAdditionalSize (argCount : int) =
        if argCount < 5 then 8uy
        else 
            let size = argCount * 8 - 24
            size |> byte

    let functionProlog argCount = prolog (getAdditionalSize argCount) |> toArray

    let functionEpilog argCount = epilog (getAdditionalSize argCount) |> toArray


    let compileCall (f : nativeint) (args : obj[]) =
        let writer = compileCallInternal f args
        let arr = Array.zeroCreate writer.size
        writer.build 0 arr
        arr

    let compileCalls (calls : seq<nativeint * obj[]>) =
        let writer = calls |> Seq.map (fun (f,a) -> compileCallInternal f a) |> concat
        let arr = Array.zeroCreate writer.size
        writer.build 0 arr
        arr


module ArmLinux =
    open Aardvark.Base

    //c => 8
    //a => 12
    //8 => 16
    //6 => 20
    //4 => 24


    let readArmImmediate (high : byte) (low : byte) =
        let high = 2 * int high
        let low = uint32 low
        (low >>> high) ||| (low <<< (32 - high))

    let private lookup =
        [| 32; 0; 1; 26; 2; 23; 27; 0; 3; 16; 24; 30; 28; 11; 0; 13; 4; 7; 17;
           0; 25; 22; 31; 15; 29; 10; 12; 6; 0; 21; 14; 9; 5; 20; 8; 19; 18 |]

    let printBits (value : int) =
        let mutable str = ""
        let mutable started = false
        for i in 0..31 do
            let shift = 31 - i

            let b = (value &&& (1 <<< shift)) >>> shift
            if b <> 0 || started then
                started <- true
                str <- str + sprintf "%d" b
        str

    let countTrailingZeros (value : uint32) =
        let value = int value
        lookup.[(value &&& -value) % 37] |> uint32

    let highestBit (value : uint32) =
        (Fun.Log2(value) |> Fun.Ceiling |> int  |> uint32)

    let toValueAndRotation (value : uint32) =
        let trailing = countTrailingZeros value
        let bits = 1u + highestBit value - countTrailingZeros value

        let ror = 2 * (int trailing / 2)
        (value >>> ror, 32 - ror)
        
    let toArmImmediate (value : uint32) =
        let h,ror = toValueAndRotation value
        if ror >= 0 && ror <= 32 && ror % 2 = 0 && h < 256u then
            if ror = 32 then Some (byte h, 0uy)
            else Some (byte h, byte (ror / 2))
        else
            None



    let ldr (register : int) (offset : uint16) =
        // ldr instruction-layout:
        // 0xE5uy; 0x9Fuy | register (4 bit) | operand (12 bit)
        if register > 15 then 
            failwith "arm has only 16 registers"
        if offset > 4095us then
            failwith "offset must be less than 2^12"

        let registerAndOffset = BitConverter.GetBytes offset

        let register = byte register
        registerAndOffset.[0] <- (register <<< 4) ||| (registerAndOffset.[0] &&& 0xFuy)

        [[| 0xE5uy; 0x9Fuy |]; registerAndOffset] |> Array.concat

    let str (register : int) (stackoffset : uint16) =
        // str instruction-layout:
        // 0xE5uy; 0x8Duy | register (4 bit) | operand (12 bit)
        if register > 15 then 
            failwith "arm has only 16 registers"
        if stackoffset > 4095us then
            failwith "stackoffset must be less than 2^12"

        let registerAndOffset = BitConverter.GetBytes stackoffset

        let register = byte register
        registerAndOffset.[0] <- (register <<< 4) ||| (registerAndOffset.[0] &&& 0xFuy)

        [[| 0xE5uy; 0x8Duy |]; registerAndOffset] |> Array.concat

    type BuilderState = Map<int, int>
    type Builder = { size : int; dataValues : Set<int>; write : BuilderState -> byte[] -> int -> unit }

    let value (data : byte[]) =
        { size = data.Length; dataValues = Set.empty; write = fun _ arr i -> data.CopyTo(arr, i) }

    let runBuilder (b : Builder) =
        let constantSize = b.dataValues.Count * 4
        let constantLocations = 
            b.dataValues |> Seq.mapi (fun i v ->
                let offset = b.size + i * 4
                (v,offset)
            ) |> Map.ofSeq

        let size = constantSize + b.size
        let arr = Array.zeroCreate size
        b.write constantLocations arr 0

        constantLocations |> Map.iter(fun v o ->
            let data = BitConverter.GetBytes v
            data.CopyTo(arr, o)
        )

        arr


    let concatBinary (l : Builder) (r : Builder) =
        { size = l.size + r.size
          dataValues = Set.union l.dataValues r.dataValues
          write = fun s arr i ->
            l.write s arr i
            r.write s arr (i + l.size)
        }

    let concat (builders : seq<Builder>) =
        builders |> Seq.fold (fun s b -> concatBinary s b) { size = 0; dataValues = Set.empty; write = fun _ _ _ -> () }

    let mov (register : int) (value : int) =
        match toArmImmediate (uint32 value) with
            | Some(h,l) ->
                let h = byte (register <<< 4) ||| (h &&& 0xFuy)
                let data = [|0xe3uy; 0xa0uy; h; l|]
                { size = 4; dataValues = Set.empty; write = fun _ arr i -> data.CopyTo(arr, i) }
            | None ->
                { size = 4; dataValues = Set.singleton value; write = fun s arr i -> 
                    let offset = 8 + s.[value] - i // TODO: no idea why we need +8 here
                    let data = ldr register (uint16 offset)
                    data.CopyTo(arr, i)
                }

    let immediateMov (register : int) (value : int) =
        match toArmImmediate (uint32 value) with
            | Some(h,l) ->
                let h = byte (register <<< 4) ||| (h &&& 0xFuy)
                [|0xe3uy; 0xa0uy; h; l|]
            | None -> failwith "unsuppored immediate value"

    let changeReturnAddress (value : int) =
        match toArmImmediate (uint32 value) with
            | Some(h,l) ->
                let h = byte (14 <<< 4) ||| (h &&& 0xFuy)
                [|0xe2uy; 0x8fuy; h; l|]
            | None -> failwith "unsuppored immediate value"

    let setArg (index : int) (value : int) =
        if index < 4 then
            mov index value
        else
            failwith ""
                
    let call (ptr : nativeint) =
        [ mov 4 (int ptr) 
          { size = 8; dataValues = Set.empty; write = fun s arr i ->
                let constantSize = 4 + s.Count * 4
                let writeRet = changeReturnAddress constantSize
                [|0xEAuy; 0xFFuy; 0xFFuy; 0xFEuy|].CopyTo(arr, i)
                writeRet.CopyTo(arr, i + 4) }              // bl r4
        ] |> concat

    let jmp (offset : int) =
        let offset = BitConverter.GetBytes(offset).[0..2]

        [[|0xEAuy|]; offset] |> Array.concat |> value  


    let printInstructions (data : byte[]) =
        for i in 0..4..data.Length-1 do
            let str = data.[i..i+3] |> Array.map (sprintf "%02X") |> String.concat ""
            printfn "%s" str


    let compileCallInternal (f : nativeint) (args : obj[]) =
        let argSetters =
            args |> Array.mapi(fun i a ->
                match a with    
                    | :? int as a -> setArg i a
                    | :? nativeint as a -> setArg i (int a)
                    | _ -> failwithf "unsupported argument: %A" a
            )

        let call = call f
        [argSetters; [|call|]] |> Array.concat |> concat

    let functionProlog = [|0xe9uy; 0x2duy; 0x48uy; 0x00uy|]
    let functionEpilog = [|0xe8uy; 0xbduy; 0x88uy; 0x00uy|]


    let compileCall (f : nativeint) (args : obj[]) =
        let writer = compileCallInternal f args
        runBuilder writer

    let compileCalls (calls : seq<nativeint * obj[]>) =
        let writer = calls |> Seq.map (fun (f,a) -> compileCallInternal f a) |> concat
        runBuilder writer


module Assembler =
    let os = System.Environment.OSVersion
    type Architecture = AMD64 | X86 | ARM

    let (|Windows|Linux|Mac|) (p : System.OperatingSystem) =
        match p.Platform with
            | System.PlatformID.Unix -> Linux
            | System.PlatformID.MacOSX -> Mac
            | _ -> Windows

    let private getLinuxCpuArchitecture() =
        
        let ps = System.Diagnostics.ProcessStartInfo("lscpu", "")
        ps.UseShellExecute <- false
        ps.RedirectStandardOutput <- true
        let proc = System.Diagnostics.Process.Start(ps)
        proc.WaitForExit()
        let cpu = proc.StandardOutput.ReadToEnd()
        let rx = System.Text.RegularExpressions.Regex @"Architecture[ \t]*:[ \t]*(?<arch>.*)"
        let m = rx.Match cpu

        if m.Success then
            let arch = m.Groups.["arch"].Value
            match arch with
                | "x86_64" -> AMD64
                | "x86" -> AMD64
                | _ -> 
                    if arch.Contains "arm" then ARM
                    else failwithf "unknown architecture: %A" arch
        else
            failwith "could not determine cpu info"
    
    let cpu = 
        match os with
            | Windows -> 
                match System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") with
                    | "AMD64" -> AMD64
                    | _ -> X86
            | Linux ->
                getLinuxCpuArchitecture()
            | Mac ->
                failwith "mac currently not supported"

    module Disasm =
        let decompile (data : byte[]) : seq<'a> =
            Seq.empty

    let functionProlog =
        match os, cpu with
            | Windows, AMD64 -> Amd64Windows.functionProlog
            | Linux, AMD64 -> Amd64Linux.functionProlog
            | Mac, AMD64 -> Amd64Linux.functionProlog
            | _ -> failwithf "no assembler for: %A / %A" os.Platform cpu    
                
    let functionEpilog =
        match os, cpu with
            | Windows, AMD64 -> Amd64Windows.functionEpilog
            | Linux, AMD64 -> Amd64Linux.functionEpilog
            | Mac, AMD64 -> Amd64Linux.functionEpilog
            | _ -> failwithf "no assembler for: %A / %A" os.Platform cpu        

    let setArg64 =
        match os, cpu with
            | Windows, AMD64 -> Amd64Windows.setArg64
            | Linux, AMD64 -> Amd64Linux.setArg64
            | Mac, AMD64 -> Amd64Linux.setArg64
            | _ -> failwithf "no assembler for: %A / %A" os.Platform cpu   

    let setArg32 =
        match os, cpu with
            | Windows, AMD64 -> Amd64Windows.setArg32
            | Linux, AMD64 -> Amd64Linux.setArg32
            | Mac, AMD64 -> Amd64Linux.setArg32
            | _ -> failwithf "no assembler for: %A / %A" os.Platform cpu      


    let compileCall =
        match os, cpu with
            | Windows, AMD64 -> fun (padding : int) (a,b) -> Amd64Windows.compileCall a b
            | Linux, AMD64 -> fun (padding : int) (a,b) -> Amd64Linux.compileCall a b
            | Mac, AMD64 -> fun (padding : int) (a,b) -> Amd64Linux.compileCall a b
            | _ -> failwithf "no assembler for: %A / %A" os.Platform cpu

    let private compileCallsInternal : seq<nativeint * obj[]> -> byte[] =
        match os, cpu with
            | Windows, AMD64 -> Amd64Windows.compileCalls
            | Linux, AMD64 -> Amd64Linux.compileCalls
            | Mac, AMD64 -> Amd64Linux.compileCalls
            | _ -> failwithf "no assembler for: %A / %A" os.Platform cpu

    let compileCalls (padding : int) (calls : #seq<nativeint * obj[]>) = compileCallsInternal calls