namespace Aardvark.Base.Runtime

open System
open System.Collections.Generic
open System.IO
open Aardvark.Base
open Aardvark.Base.Incremental
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"
#nowarn "51"


module AMD64 =

    type Register =
        | Rax = 0
        | Rcx = 1
        | Rdx = 2
        | Rbx = 3
        | Rsp = 4
        | Rbp = 5
        | Rsi = 6
        | Rdi = 7

        | R8  = 8
        | R9  = 9
        | R10 = 10
        | R11 = 11
        | R12 = 12
        | R13 = 13
        | R14 = 14
        | R15 = 15

        | XMM0 = 16
        | XMM1 = 17
        | XMM2 = 18
        | XMM3 = 19
        | XMM4 = 20
        | XMM5 = 21
        | XMM6 = 22
        | XMM7 = 23
        | XMM8 = 24
        | XMM9 = 25
        | XMM10 = 26
        | XMM11 = 27
        | XMM12 = 28
        | XMM13 = 29
        | XMM14 = 30
        | XMM15 = 31

    type CallingConvention = { shadowSpace : bool; registers : Register[]; floatRegisters : Register[]; calleeSaved : Register[] }
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module CallingConvention =
        let windows = 
            { 
                shadowSpace = true
                registers = [| Register.Rcx; Register.Rdx; Register.R8; Register.R9 |] 
                floatRegisters = [| Register.XMM0; Register.XMM1; Register.XMM2; Register.XMM3 |]
                calleeSaved = [|Register.R12; Register.R13; Register.R14; Register.R15 |]
            }

        let linux = 
            { 
                shadowSpace = false
                registers = [| Register.Rdi; Register.Rsi; Register.Rdx; Register.Rcx; Register.R8; Register.R9 |] 
                floatRegisters = [| Register.XMM0; Register.XMM1; Register.XMM2; Register.XMM3; Register.XMM4; Register.XMM5; Register.XMM6; Register.XMM7 |]
                calleeSaved = [|Register.R12; Register.R13; Register.R14; Register.R15 |]
            }

    [<AutoOpen>]
    module private Utils = 
        let inline rexAndModRM (wide : bool) (left : byte) (right : byte) (rex : byref<byte>) (modRM : byref<byte>) =
            let r = if left >= 8uy then 1uy else 0uy
            let b = if right >= 8uy then 1uy else 0uy
            let w = if wide then 1uy else 0uy
            rex <- 0x40uy ||| (w <<< 3) ||| (r <<< 2) ||| b
                
            let left = left &&& 0x07uy
            let right = right &&& 0x07uy
            modRM <- 0xC0uy ||| (left <<< 3) ||| right

        let inline rexAndModRM0 (wide : bool) (left : byte) (right : byte) (rex : byref<byte>) (modRM : byref<byte>) =
            let r = if left >= 8uy then 1uy else 0uy
            let b = if right >= 8uy then 1uy else 0uy
            let w = if wide then 1uy else 0uy
            rex <- 0x40uy ||| (w <<< 3) ||| (r <<< 2) ||| b
                
            let left = left &&& 0x07uy
            let right = right &&& 0x07uy
            modRM <- 0x00uy ||| (left <<< 3) ||| right

        let inline rexAndModRMSIB (wide : bool) (left : byte) (rex : byref<byte>) (modRM : byref<byte>) =
            let r = if left >= 8uy then 1uy else 0uy
            let w = if wide then 1uy else 0uy
            rex <- 0x40uy ||| (w <<< 3) ||| (r <<< 2)
                
            let left = left &&& 0x07uy
            modRM <- 0x40uy ||| (left <<< 3) ||| 0x04uy


        let inline dec (v : byref<int>) =
            let o = v
            v <- o - 1
            if o < 0 then failwith "argument index out of bounds"
            o

    let private localConvention =
        match Environment.OSVersion with
            | Windows -> CallingConvention.windows
            | _ -> CallingConvention.linux


    let private registers =
        [|
            Aardvark.Base.Runtime.Register("rax", 0)
            Aardvark.Base.Runtime.Register("rcx", 1)
            Aardvark.Base.Runtime.Register("rdx", 2)
            Aardvark.Base.Runtime.Register("rbx", 3)
            Aardvark.Base.Runtime.Register("rsp", 4)
            Aardvark.Base.Runtime.Register("rbp", 5)
            Aardvark.Base.Runtime.Register("rsi", 6)
            Aardvark.Base.Runtime.Register("rdi", 7)
            Aardvark.Base.Runtime.Register("r8", 8)
            Aardvark.Base.Runtime.Register("r9", 9)
            Aardvark.Base.Runtime.Register("r10", 10)
            Aardvark.Base.Runtime.Register("r11", 11)
            Aardvark.Base.Runtime.Register("r12", 12)
            Aardvark.Base.Runtime.Register("r13", 13)
            Aardvark.Base.Runtime.Register("r14", 14)
            Aardvark.Base.Runtime.Register("r15", 15)
            Aardvark.Base.Runtime.Register("xmm0", 16)
            Aardvark.Base.Runtime.Register("xmm1", 17)
            Aardvark.Base.Runtime.Register("xmm2", 18)
            Aardvark.Base.Runtime.Register("xmm3", 19)
            Aardvark.Base.Runtime.Register("xmm4", 20)
            Aardvark.Base.Runtime.Register("xmm5", 21)
            Aardvark.Base.Runtime.Register("xmm6", 22)
            Aardvark.Base.Runtime.Register("xmm7", 23)
            Aardvark.Base.Runtime.Register("xmm8", 24)
            Aardvark.Base.Runtime.Register("xmm9", 25)
            Aardvark.Base.Runtime.Register("xmm10", 26)
            Aardvark.Base.Runtime.Register("xmm11", 27)
            Aardvark.Base.Runtime.Register("xmm12", 28)
            Aardvark.Base.Runtime.Register("xmm13", 29)
            Aardvark.Base.Runtime.Register("xmm14", 30)
            Aardvark.Base.Runtime.Register("xmm15", 31)
        |]

    let private calleeSaved = localConvention.calleeSaved |> Array.map (int >> Array.get registers)
    let private argumentRegisters = localConvention.registers |> Array.map (int >> Array.get registers)
    let private returnRegister = registers.[0]

    // Register.R12; Register.R13; Register.R14; Register.R15
    
    type AssemblerStream(stream : Stream, leaveOpen : bool) =
        let writer = new BinaryWriter(stream, Text.Encoding.UTF8, leaveOpen)

        static let localConvention =
            match Environment.OSVersion with
                | Windows -> CallingConvention.windows
                | _ -> CallingConvention.linux

        let mutable stackOffset = 0
        let mutable paddingPtr = []
        let mutable argumentOffset = 0
        let mutable argumentIndex = 0

        static let push             = [| 0x48uy; 0x89uy; 0x44uy; 0x24uy |]
        static let callRax          = [| 0xFFuy; 0xD0uy |]

        static let oneByteNop       = [| 0x90uy |]
        static let twoByteNop       = [| 0x66uy; 0x90uy |]
        static let threeByteNop     = [| 0x0Fuy; 0x1Fuy; 0x00uy |]
        static let fourByteNop      = [| 0x0Fuy; 0x1Fuy; 0x40uy; 0x00uy |]
        static let fiveByteNop      = [| 0x0Fuy; 0x1Fuy; 0x44uy; 0x00uy; 0x00uy |]
        static let eightByteNop     = [| 0x0Fuy; 0x1Fuy; 0x84uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy |]

        let pendingOffsets = Dict<AssemblerLabel, List<int64>>()

        member x.NewLabel() =
            AssemblerLabel()

        member x.Mark(l : AssemblerLabel) =
            match pendingOffsets.TryRemove l with
                | (true, positions) ->
                    let oldPos = stream.Position
                    
                    for p in positions do
                        stream.Position <- p
                        writer.Write(int (oldPos - (4L + p)))

                    stream.Position <- oldPos
                | _ ->
                    ()

            l.Position <- stream.Position

        member x.Cmp(l : Register, v : uint32) =
            if l >= Register.XMM0 then
                failwith "[AMD64] cannot compare media register"
            else
                let mutable rex = if l >= Register.R8 then 0x41uy else 0x40uy
                //rex <- 0x40uy ||| (w <<< 3) ||| (r <<< 2) ||| b
                let l = byte l &&& 0x7uy

                if rex <> 0x40uy then writer.Write(rex)
                writer.Write(0x81uy)
                writer.Write(0xF8uy + l)
                writer.Write(v)
                
        member x.Cmp(l : Register, v : uint64) =
            if l >= Register.XMM0 then
                failwith "[AMD64] cannot compare media register"
            else
                let mutable rex = if l >= Register.R8 then 0x49uy else 0x48uy
                let l = byte l &&& 0x7uy

                if rex <> 0x40uy then writer.Write(rex)
                writer.Write(0x3Buy)
                writer.Write(0xF8uy + l)
                writer.Write(v)

        member x.Cmp(l : Register, r : Register, wide : bool) =
            if l >= Register.XMM0 || r >= Register.XMM0 then
                failwith "[AMD64] cannot compare media register"
            else
                let mutable rex = 0uy
                let mutable modRM = 0uy
                rexAndModRM wide (byte l) (byte r) &rex &modRM
                if rex <> 0x40uy then writer.Write(rex)
                writer.Write(0x3B)
                writer.Write(modRM)

        member x.Jump(l : AssemblerLabel) =
            if l.Position >= 0L then
                let offset = 5L + l.Position - stream.Position
                x.Jmp(int offset)
            else
                x.Jmp(0)
                let set = pendingOffsets.GetOrCreate(l, fun _ -> List())
                set.Add(stream.Position - 4L)

        member x.Jump(cond : JumpCondition, l : AssemblerLabel) =
            if l.Position >= 0L then
                let offset = 6L + l.Position - stream.Position
                x.Jmp(cond, int offset)
            else
                x.Jmp(cond, 0)
                let set = pendingOffsets.GetOrCreate(l, fun _ -> List())
                set.Add(stream.Position - 4L)

        member x.Jmp(cond : JumpCondition, offset : int) =
            writer.Write(0x0Fuy)
            writer.Write(byte cond)
            writer.Write(offset)

        member x.Leave() =
            writer.Write(0xC9uy)

        member x.Begin() =
            x.Push(Register.Rbp)
            x.Mov(Register.Rbp, Register.Rsp, true)
            stackOffset <- stackOffset - 8

        member x.End() =
            x.Leave()
            stackOffset <- stackOffset - 8

        member x.Mov(target : Register, source : Register, wide : bool) =
            if source <> target then
                let targetMedia = target >= Register.XMM0
                let sourceMedia = source >= Register.XMM0

                let mutable rex = 0x40uy
                let mutable modRM = 0uy

                let dst = byte target &&& 0x0Fuy
                let src = byte source &&& 0x0Fuy


                if targetMedia && sourceMedia then
                    rexAndModRM wide dst src &rex &modRM
                    writer.Write(0xF3uy)
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x0Fuy)
                    writer.Write(0x7Euy)
                    writer.Write(modRM)

                elif sourceMedia then
                    rexAndModRM wide src dst &rex &modRM
                    // MOVD  reg/mem32, xmm         66 0F 7E /r
                    // MOVD  reg/mem64, xmm         66 0F 7E /r

                    writer.Write(0x66uy)
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x0Fuy)
                    writer.Write(0x7Euy)
                    writer.Write(modRM)

                elif targetMedia then
                    // MOVD  xmm, reg/mem32         66 0F 6E /r
                    // MOVD  xmm, reg/mem64         66 0F 6E /r
                
                    rexAndModRM wide dst src &rex &modRM
                    writer.Write(0x66uy)
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x0Fuy)
                    writer.Write(0x6Euy)
                    writer.Write(modRM)

                else
                    // MOV   reg64, reg/mem64       8B/r
                    // MOV   reg32, reg/mem32       8B/r

                    rexAndModRM wide dst src &rex &modRM
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x8Buy)
                    writer.Write(modRM)

        member x.Load(target : Register, source : Register, wide : bool) =
            let targetMedia = target >= Register.XMM0
            let sourceMedia = source >= Register.XMM0
            
            let dst = byte target &&& 0x0Fuy
            let src = byte source &&& 0x0Fuy

            let mutable rex = 0x40uy
            let mutable modRM = 0uy

            if sourceMedia then
                failwith "mov reg|xmm, (xmm) not implemented"

            elif targetMedia then
                if wide then
                    rexAndModRM0 false dst src &rex &modRM
                    writer.Write(0xF3uy)
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x0Fuy)
                    writer.Write(0x7Euy)
                    writer.Write(modRM)
                    if source = Register.Rsp then writer.Write(0x24uy)

                else
                    // MOVD  xmm, reg/mem32         66 0F 6E /r
                    // MOVD  xmm, reg/mem64         66 0F 6E /r
                    rexAndModRM0 wide dst src &rex &modRM
                    writer.Write(0x66uy)
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x0Fuy)
                    writer.Write(0x6Euy)
                    writer.Write(modRM)
                    if source = Register.Rsp then writer.Write(0x24uy)

            else
                // MOV   reg64, reg/mem64       8B/r
                // MOV   reg32, reg/mem32       8B/r
                rexAndModRM0 wide dst src &rex &modRM
                if rex <> 0x40uy then writer.Write(rex)
                writer.Write(0x8Buy)
                writer.Write(modRM)
                if source = Register.Rsp then writer.Write(0x24uy)

        member x.Store(target : Register, source : Register, wide : bool) =
            let targetMedia = target >= Register.XMM0
            let sourceMedia = source >= Register.XMM0
            
            let mutable rex = 0x40uy
            let mutable modRM = 0uy

            let dst = byte target &&& 0x0Fuy
            let src = byte source &&& 0x0Fuy

            if targetMedia then
                failwith "mov (xmm), reg|xmm not implemented"

            elif sourceMedia then
                if wide then
                    rexAndModRM0 false src dst &rex &modRM
                    writer.Write(0x66uy)
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x0Fuy)
                    writer.Write(0xD6uy)
                    writer.Write(modRM)
                    if target = Register.Rsp then writer.Write(0x24uy)
                else
                    // MOVD  reg/mem32, xmm         66 0F 7E /r
                    // MOVD  reg/mem64, xmm         66 0F 7E /r

                    rexAndModRM0 wide src dst &rex &modRM
                    writer.Write(0x66uy)
                    if rex <> 0x40uy then writer.Write(rex)
                    writer.Write(0x0Fuy)
                    writer.Write(0x7Euy)
                    writer.Write(modRM)
                    if target = Register.Rsp then writer.Write(0x24uy)
            else
                // MOV   reg/mem64, reg64       89/r
                // MOV   reg/mem32, reg32       89/r

                rexAndModRM0 wide src dst &rex &modRM
                if rex <> 0x40uy then writer.Write(rex)
                writer.Write(0x89uy)
                writer.Write(modRM)
                if target = Register.Rsp then writer.Write(0x24uy)

        member x.Mov(target : Register, value : uint64) =
            if target < Register.XMM0 then
                let tb = target |> byte
                if tb >= 8uy then
                    let tb = tb - 8uy
                    let rex = 0x49uy
                    writer.Write(rex)
                    writer.Write(0xB8uy + tb)
                else
                    let rex = 0x48uy
                    writer.Write(rex)
                    writer.Write(0xB8uy + tb)

                writer.Write value 
                
            else
                x.Mov(Register.Rax, value)
                x.Mov(target, Register.Rax, true)

        member x.Mov(target : Register, value : uint32) =
            if target < Register.XMM0 then
                let tb = target |> byte
                if tb >= 8uy then
                    let tb = tb - 8uy
                    let rex = 0x41uy
                    writer.Write(rex)
                    writer.Write(0xB8uy + tb)
                else
                    writer.Write(0xB8uy + tb)

                writer.Write value 
                
            else
                x.Mov(Register.Rax, value)
                x.Mov(target, Register.Rax, false)


        member inline x.MovQWord(target : Register, source : Register) =
            x.Mov(target, source, true)

        member inline x.MovDWord(target : Register, source : Register) =
            x.Mov(target, source, false)

        member inline x.Mov(target : Register, value : nativeint) =
            x.Mov(target, uint64 value)

        member inline x.Mov(target : Register, value : unativeint) =
            x.Mov(target, uint64 value)

        member inline x.Mov(target : Register, value : int) =
            x.Mov(target, uint32 value)

        member inline x.Mov(target : Register, value : int64) =
            x.Mov(target, uint64 value)

        member inline x.Mov(target : Register, value : int8) =
            x.Mov(target, uint32 value)

        member inline x.Mov(target : Register, value : uint8) =
            x.Mov(target, uint32 value)

        member inline x.Mov(target : Register, value : int16) =
            x.Mov(target, uint32 value)

        member inline x.Mov(target : Register, value : uint16) =
            x.Mov(target, uint32 value)

        member inline x.Mov(target : Register, value : float32) =
            let mutable value = value
            let iv : uint32 = &&value |> NativePtr.cast |> NativePtr.read
            x.Mov(target, iv)

        member inline x.Mov(target : Register, value : float) =
            let mutable value = value
            let iv : uint64 = &&value |> NativePtr.cast |> NativePtr.read
            x.Mov(target, iv)

        member inline x.Load(target : Register, ptr : nativeint, wide : bool) =
            x.Mov(target, uint64 ptr)
            x.Load(target, target, wide)

        member inline x.Load(target : Register, ptr : nativeptr<'a>) =
            x.Load(target, NativePtr.toNativeInt ptr, sizeof<'a> = 8)

        member inline x.LoadQWord(target : Register, ptr : nativeint) =
            x.Load(target, ptr, true)

        member inline x.LoadDWord(target : Register, ptr : nativeint) =
            x.Load(target, ptr, false)

        member x.Add(target : Register, source : Register, wide : bool) =
            let mutable rex = 0x40uy
            let mutable modRM = 0uy
            
            if source >= Register.XMM0 || target >= Register.XMM0 then
                failwith "cannot add media register"
            else
                rexAndModRM wide (byte target) (byte source) &rex &modRM

                if rex <> 0x40uy then writer.Write(rex)
                writer.Write(0x03uy)
                writer.Write(modRM)

        member x.Add(target : Register, value : uint32) =
            if value > 0u then
                if target >= Register.XMM0 then
                    failwith "cannot add media register"
                else
                    let r = target |> byte
                    let b = if r >= 8uy then 1uy else 0uy //(r &&& 0xF8uy) >>> 3
                    let r = r &&& 0x07uy
                    let rex = 0x48uy ||| b
                
                    writer.Write(rex)
                    writer.Write(0x81uy)
                    writer.Write(0xC0uy + r)
                    writer.Write(value)

        member x.Sub(target : Register, value : uint32) = 
            if value > 0u then
                if target >= Register.XMM0 then
                    failwith "cannot add media register"
                else
                    let r = target |> byte
                    let b = if r >= 8uy then 1uy else 0uy //(r &&& 0xF8uy) >>> 3
                    let r = r &&& 0x07uy
                    let rex = 0x48uy ||| b
                
                    writer.Write(rex)
                    writer.Write(0x81uy)
                    writer.Write(0xE8uy + r)
                    writer.Write(value)

        member x.Sub(target : Register, source : Register, wide : bool) = 
            if target >= Register.XMM0 || source >= Register.XMM0 then
                failwith "cannot sub media register"
            else
                let mutable rex = 0x40uy
                let mutable modRM = 0uy

                rexAndModRM wide (byte source) (byte target) &rex &modRM

                if rex <> 0x40uy then writer.Write(rex)
                writer.Write(0x29uy)
                writer.Write(modRM)



        member x.Push(r : Register) =
            stackOffset <- stackOffset + 8
            if r >= Register.XMM0 then
                x.Sub(Register.Rsp, 8u)
                x.Store(Register.Rsp, r, true)
            else
                let r = r |> byte
                let b = if r >= 8uy then 1uy else 0uy //(r &&& 0xF8uy) >>> 3
                let r = r &&& 0x07uy
                let w = 1uy
                let rex = 0x40uy ||| (w <<< 3) ||| b

                let code = 0x50uy + r
                if rex <> 0x4uy then writer.Write(rex)
                writer.Write(code)

        member x.Push(value : uint64) =
            x.Mov(Register.Rax, value)
            x.Push(Register.Rax)
//            writer.Write(0x48uy)
//            writer.Write(0x68uy)
//            writer.Write(value)

        member x.Push(value : uint32) =
            stackOffset <- stackOffset + 8
            writer.Write(0x68uy)
            writer.Write(value)

        member x.Push(value : float) =
            stackOffset <- stackOffset + 8
            writer.Write(0x48uy)
            writer.Write(0x68uy)
            writer.Write(value)

        member x.Push(value : float32) =
            stackOffset <- stackOffset + 8
            writer.Write(0x68uy)
            writer.Write(value)
            

        member x.Pop(r : Register) =
            stackOffset <- stackOffset - 8
            if r >= Register.XMM0 then
                x.Load(r, Register.Rsp, true)
                x.Add(Register.Rsp, 8u)
                ()
            else
                let r = r |> byte

                let b = (r &&& 0xF8uy) >>> 3
                let r = r &&& 0x07uy
                let w = 1uy
                let rex = 0x40uy ||| (w <<< 3) ||| b

                let code = 0x58uy + r
                if rex <> 0x4uy then writer.Write(rex)
                writer.Write(code)


        member x.Jmp(offset : int) =
            writer.Write 0xE9uy
            writer.Write offset
            
        member x.Nop(width : int) =
            if width > 0 then
                match width with
                    | 1 -> writer.Write oneByteNop
                    | 2 -> writer.Write twoByteNop
                    | 3 -> writer.Write threeByteNop
                    | 4 -> writer.Write fourByteNop
                    | 5 -> writer.Write fiveByteNop
                    | 6 -> writer.Write threeByteNop; writer.Write threeByteNop // TODO: find good 6 byte nop sequence
                    | 7 -> writer.Write fourByteNop; writer.Write threeByteNop // TODO: find good 7 byte nop sequence
                    | _ -> writer.Write eightByteNop; x.Nop (width - 8)

        member x.BeginCall(args : int) =
            x.Sub(Register.Rsp, 8u)
            stream.Seek(-4L, SeekOrigin.Current) |> ignore
            let ptr = stream.Position
            writer.Write(0u)
            paddingPtr <- ptr :: paddingPtr
            argumentOffset <- 0
            argumentIndex <- args - 1

        member x.Call(cc : CallingConvention, r : Register) =
            if r >= Register.XMM0 then
                failwith "cannot call media register"
            else

                let paddingPtr =
                    match paddingPtr with
                        | h :: rest ->
                            paddingPtr <- rest
                            h
                        | _ ->
                            failwith "no padding offset"

                let additional = 
                    if stackOffset % 16 <> 0 then
                        let p = stream.Position
                        stream.Position <- paddingPtr
                        writer.Write(8u)
                        stream.Position <- p
                        8u
                    else
                        0u

                if cc.shadowSpace then
                    x.Sub(Register.Rsp, 8u * uint32 cc.registers.Length)

                let r = byte r
                if r >= 8uy then
                    writer.Write(0x41uy)
                    writer.Write(0xFFuy)
                    writer.Write(0xD0uy + (r - 8uy))

                else
                    writer.Write(0xFFuy)
                    writer.Write(0xD0uy + r)
                    
                let popSize =
                    (if cc.shadowSpace then 8u * uint32 cc.registers.Length else 0u) +
                    uint32 argumentOffset +
                    additional

                if popSize > 0u then
                    x.Add(Register.Rsp, popSize)

                stackOffset <- stackOffset - argumentOffset
                argumentOffset <- 0

        member x.Call(cc : CallingConvention, ptr : nativeint) =
            x.Mov(Register.Rax, ptr)
            x.Call(cc, Register.Rax)


        member x.Ret() =
            writer.Write(0xC3uy)

        member x.PushArg(cc : CallingConvention, value : uint64) =
            let i = dec &argumentIndex
            if i < cc.registers.Length then
                x.Mov(cc.registers.[i], value)
            else
                argumentOffset <- argumentOffset + 8
                x.Push(value)

         member x.PushArg(cc : CallingConvention, value : uint32) =
            let i = dec &argumentIndex
            if i < cc.registers.Length then
                x.Mov(cc.registers.[i], value)
            else
                argumentOffset <- argumentOffset + 8
                x.Push(value)    

         member x.PushArg(cc : CallingConvention, value : float32) =
            let i = dec &argumentIndex
            if i < cc.floatRegisters.Length then
                x.Mov(cc.floatRegisters.[i], value)
            else
                argumentOffset <- argumentOffset + 8
                x.Push(value)  

         member x.PushArg(cc : CallingConvention, value : float) =
            let i = dec &argumentIndex
            if i < cc.floatRegisters.Length then
                x.Mov(cc.floatRegisters.[i], value)
            else
                argumentOffset <- argumentOffset + 8
                x.Push(value)  

        member x.PushIntArg(cc : CallingConvention, r : Register, wide : bool) =
            let i = dec &argumentIndex
            if i < cc.registers.Length then
                x.Mov(cc.registers.[i], r, wide)
            else
                argumentOffset <- argumentOffset + 8
                x.Push(r)

        member x.PushFloatArg(cc : CallingConvention, r : Register, wide : bool) =
            let i = dec &argumentIndex
            if i < cc.floatRegisters.Length then
                x.Mov(cc.floatRegisters.[i], r, wide)
            else
                argumentOffset <- argumentOffset + 8
                x.Push(r)


        member private x.Dispose(disposing : bool) =
            if disposing then 
                GC.SuppressFinalize(x)
                if pendingOffsets.Count > 0 then
                    failwith "[AMD64] some labels have not been defined"

            writer.Dispose()


        member x.ConditionalCall(condition : IMod<'a>, callback : 'a -> unit) =
            let outOfDate : nativeptr<int> = NativePtr.alloc 1
            NativePtr.write outOfDate (if condition.OutOfDate then 1 else 0)
            let sub = condition.AddMarkingCallback(fun () -> NativePtr.write outOfDate 1)
        

            let callback () =
                let value = 
                    lock condition (fun () ->
                        let res = condition.GetValue(AdaptiveToken.Top)
                        NativePtr.write outOfDate 0
                        res
                    )
                callback value
                
            let del = Marshal.PinDelegate(System.Action callback)
            
            let noEval = x.NewLabel()
            x.Load(Register.Rax, outOfDate)
            x.Cmp(Register.Rax, 0u)
            x.Jump(JumpCondition.Equal,noEval)
            x.BeginCall(0)
            x.Call(localConvention, del.Pointer)
            x.Mark noEval

            { new IDisposable with
                member x.Dispose() =
                    sub.Dispose()
                    NativePtr.write outOfDate 0
                    NativePtr.free outOfDate
                    del.Dispose()
            }




        member x.Dispose() = x.Dispose(true)
        override x.Finalize() = x.Dispose(false)

        

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IAssemblerStream with

            member x.Registers = registers
            member x.CalleeSavedRegisters = calleeSaved
            member x.ArgumentRegisters = argumentRegisters
            member x.ReturnRegister = returnRegister


            member x.Push(r : Runtime.Register) = x.Push(unbox<Register> r.Tag)
            member x.Pop(r : Runtime.Register) = x.Pop(unbox<Register> r.Tag)
            member x.Mov(target : Runtime.Register, source : Runtime.Register) = x.Mov(unbox<Register> target.Tag, unbox<Register> source.Tag, true)
            member x.Load(target : Runtime.Register, source : Runtime.Register, wide : bool) = x.Load(unbox<Register> target.Tag, unbox<Register> source.Tag, wide)
            member x.Store(target : Runtime.Register, source : Runtime.Register, wide : bool) = x.Store(unbox<Register> target.Tag, unbox<Register> source.Tag, wide)

            
            member x.NewLabel() = x.NewLabel()
            member x.Mark(l) = x.Mark(l)
            member x.Jump(cond : JumpCondition, label : AssemblerLabel) = x.Jump(cond, label)
            member x.Jump(label : AssemblerLabel) = x.Jump(label)

            member x.Cmp(location : nativeint, value : int) =
                x.Load(Register.Rax, location, false)
                x.Cmp(Register.Rax, uint32 value)


            member x.BeginFunction() = x.Begin()
            member x.EndFunction() = x.End()
            member x.BeginCall(args : int) = x.BeginCall(args)
            member x.Call (ptr : nativeint) = x.Call(localConvention, ptr)
            member x.CallIndirect(ptr : nativeptr<nativeint>) =
                x.Load(Register.Rax, ptr)
                x.Call(localConvention, Register.Rax)

            member x.PushArg(v : nativeint) = x.PushArg(localConvention, uint64 v)
            member x.PushArg(v : int) = x.PushArg(localConvention, uint32 v)
            member x.PushArg(v : float32) = x.PushArg(localConvention, v)
            member x.PushPtrArg(loc) = x.Load(Register.Rax, loc, true); x.PushIntArg(localConvention, Register.Rax, true)
            member x.PushIntArg(loc) = x.Load(Register.Rax, loc, false); x.PushIntArg(localConvention, Register.Rax, false)
            member x.PushFloatArg(loc) = x.Load(Register.Rax, loc, false); x.PushFloatArg(localConvention, Register.Rax, false)
            member x.PushDoubleArg(loc) = x.Load(Register.Rax, loc, true); x.PushFloatArg(localConvention, Register.Rax, true)

            member x.Ret() = x.Ret()

            member x.WriteOutput(v : nativeint) = x.Mov(Register.Rax, v)
            member x.WriteOutput(v : int) = x.Mov(Register.Rax, v)
            member x.WriteOutput(v : float32) = x.Mov(Register.XMM0, v)
            member x.Jump(offset : int) = x.Jmp(offset)

        new(stream : Stream) = new AssemblerStream(stream, false)

