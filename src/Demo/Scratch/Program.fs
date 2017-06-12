// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.


open System 
open System.Threading
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Monads.State


open System.Drawing
open System.Windows.Forms
open System.IO
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"
#nowarn "51"

type NativeStream() =
    inherit Stream()

    let mutable capacity = 128n
    let mutable store = Marshal.AllocHGlobal(capacity)
    let mutable offset = 0n
    let mutable length = 0n

    member x.Pointer = store
        
    override x.Dispose(disposing : bool) =
        base.Dispose(disposing)
        let s = Interlocked.Exchange(&store, 0n)
        if s <> 0n then
            Marshal.FreeHGlobal s
            capacity <- 0n
            offset <- 0n
            length <- 0n

    override x.CanRead = true
    override x.CanSeek = true
    override x.CanWrite = true

    override x.Length = int64 length
    override x.Position
        with get() = int64 offset
        and set o = offset <- nativeint o

    override x.Flush() = ()

    override x.Seek(o, origin) =
        match origin with
            | SeekOrigin.Begin -> offset <- nativeint o
            | SeekOrigin.Current -> offset <- offset + nativeint o
            | SeekOrigin.End -> offset <- length - nativeint o
            | _ -> ()
        int64 offset

    override x.SetLength(l : int64) =
        if l > int64 capacity then
            let newCap = Fun.NextPowerOfTwo l |> nativeint
            store <- Marshal.ReAllocHGlobal(store, newCap)
            capacity <- newCap

        length <- nativeint l

    override x.Read(buffer : byte[], o : int, count : int) =
        let count = min (length - nativeint offset) (nativeint count) |> int
        Marshal.Copy(store + offset, buffer, o, count)
        count

    override x.Write(buffer : byte[], o : int, count : int) =
        let l = offset + nativeint count
        if l > capacity then
            let newCap = Fun.NextPowerOfTwo (int64 l) |> nativeint
            store <- Marshal.ReAllocHGlobal(store, newCap)
            capacity <- newCap

        Marshal.Copy(buffer, o, store + offset, count)
        length <- length + nativeint count
        offset <- offset + nativeint count

type IAssemblerStream =
    inherit IDisposable
    abstract member BeginFunction : unit -> unit
    abstract member EndFunction : unit -> unit
    abstract member BeginCall : args : int -> unit
    abstract member Call : ptr : nativeint -> unit
    abstract member PushPtrArg : location : nativeint -> unit
    abstract member PushIntArg : location : nativeint -> unit
    abstract member PushFloatArg : location : nativeint -> unit
    abstract member PushArg : value : nativeint -> unit
    abstract member PushArg : value : int -> unit
    abstract member PushArg : value : float32 -> unit
    abstract member Ret : unit -> unit
    abstract member WriteOutput : value : nativeint -> unit
    abstract member WriteOutput : value : int -> unit
    abstract member WriteOutput : value : float32 -> unit
    abstract member Jump : offset : int -> unit

module AMD64 =

    let private printBinary (v : uint8) =
        let mutable v = v
        let mutable mask = 1uy <<< 7
        
        for _ in 1 .. 8 do
            printf "%d" (if v &&& mask <> 0uy then 1 else 0)
            mask <- mask >>> 1
        printfn ""

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

    type AssemblerStream(stream : Stream, leaveOpen : bool) =
        let writer = new BinaryWriter(stream, Text.Encoding.UTF8, leaveOpen)

        static let localConvention =
            match Environment.OSVersion with
                | Windows -> CallingConvention.windows
                | _ -> CallingConvention.linux

        let mutable stackOffset = 0
        let mutable callPadded = []
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

            argumentOffset <- 0
            paddingPtr <- ptr :: paddingPtr
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
//                    //let align = stackOffset - argumentOffset
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
            if disposing then GC.SuppressFinalize(x)
            writer.Dispose()

        member x.Dispose() = x.Dispose(true)
        override x.Finalize() = x.Dispose(false)

        

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IAssemblerStream with
            member x.BeginFunction() = x.Begin()
            member x.EndFunction() = x.End()
            member x.BeginCall(args : int) = x.BeginCall(args)
            member x.Call (ptr : nativeint) = x.Call(localConvention, ptr)
            member x.PushArg(v : nativeint) = x.PushArg(localConvention, uint64 v)
            member x.PushArg(v : int) = x.PushArg(localConvention, uint32 v)
            member x.PushArg(v : float32) = x.PushArg(localConvention, v)
            member x.PushPtrArg(loc) = x.Load(Register.Rax, loc, true); x.PushIntArg(localConvention, Register.Rax, true)
            member x.PushIntArg(loc) = x.Load(Register.Rax, loc, false); x.PushIntArg(localConvention, Register.Rax, false)
            member x.PushFloatArg(loc) = x.Load(Register.Rax, loc, false); x.PushFloatArg(localConvention, Register.Rax, false)

            member x.Ret() = x.Ret()

            member x.WriteOutput(v : nativeint) = x.Mov(Register.Rax, v)
            member x.WriteOutput(v : int) = x.Mov(Register.Rax, v)
            member x.WriteOutput(v : float32) = x.Mov(Register.XMM0, v)
            member x.Jump(offset : int) = x.Jmp(offset)

        new(stream : Stream) = new AssemblerStream(stream, false)

module X86 =
    type Register =
        | Eax = 0
        | Ecx = 1
        | Edx = 2
        | Ebx = 3
        | Esp = 4
        | Ebp = 5
        | Esi = 6
        | Edi = 7

    [<AutoOpen>]
    module private Utils = 
        let inline modRM (left : byte) (right : byte) =
            let left = left 
            let right = right
            0xC0uy ||| (left <<< 3) ||| right

        let inline modRM0 (left : byte) (right : byte) =
            let left = left 
            let right = right
            (left <<< 3) ||| right

        let inline dec (v : byref<int>) =
            let o = v
            v <- o - 1
            if o < 0 then failwith "argument index out of bounds"
            o

    type CallingConvention = { registers : Register[]; floatRegisters : Register[]; calleeSaved : Register[] }
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module CallingConvention =
        let windows = 
            { 
                registers = [|  |] 
                floatRegisters = [|  |]
                calleeSaved = [| |]
            }

    type AssemblerStream(stream : Stream, leaveOpen : bool) =
        let writer = new BinaryWriter(stream, Text.Encoding.UTF8, leaveOpen)


        static let localConvention =
            match Environment.OSVersion with
                | Windows -> CallingConvention.windows
                | os -> failwithf "not implemented: %A" os


        let mutable argumentIndex = 0
        let mutable argumentOffset = 0

        static let push             = [| 0x48uy; 0x89uy; 0x44uy; 0x24uy |]
        static let callRax          = [| 0xFFuy; 0xD0uy |]

        static let oneByteNop       = [| 0x90uy |]
        static let twoByteNop       = [| 0x66uy; 0x90uy |]
        static let threeByteNop     = [| 0x0Fuy; 0x1Fuy; 0x00uy |]
        static let fourByteNop      = [| 0x0Fuy; 0x1Fuy; 0x40uy; 0x00uy |]
        static let fiveByteNop      = [| 0x0Fuy; 0x1Fuy; 0x44uy; 0x00uy; 0x00uy |]
        static let eightByteNop     = [| 0x0Fuy; 0x1Fuy; 0x84uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy |]


        member x.Leave() =
            writer.Write(0xC9uy)

        member x.Begin() =
            x.Push(Register.Ebp)
            x.Mov(Register.Ebp, Register.Esp)

        member x.End() = 
            x.Leave()

        member x.Mov(target : Register, source : Register) =
            if source <> target then

                let dst = byte target 
                let src = byte source 

                // MOV   reg64, reg/mem64       8B/r
                // MOV   reg32, reg/mem32       8B/r
                writer.Write(0x8Buy)
                writer.Write(modRM dst src)

        member x.Load(target : Register, source : Register) =
            let dst = byte target
            let src = byte source

            let modRM = modRM0 dst src

            // MOV   reg64, reg/mem64       8B/r
            // MOV   reg32, reg/mem32       8B/r
            writer.Write(0x8Buy)
            writer.Write(modRM)
            if source = Register.Esp then writer.Write(0x24uy)

        member x.Store(target : Register, source : Register) =
            let dst = byte target &&& 0x0Fuy
            let src = byte source &&& 0x0Fuy

            // MOV   reg/mem64, reg64       89/r
            // MOV   reg/mem32, reg32       89/r

            let modRM = modRM0 src dst
            writer.Write(0x89uy)
            writer.Write(modRM)
            if target = Register.Esp then writer.Write(0x24uy)

        member x.Mov(target : Register, value : uint32) =
            let tb = target |> byte
            writer.Write(0xB8uy + tb)
            writer.Write value 
               
        member inline x.Mov(target : Register, value : nativeint) =
            x.Mov(target, uint32 value)

        member inline x.Mov(target : Register, value : unativeint) =
            x.Mov(target, uint32 value)

        member inline x.Mov(target : Register, value : int) =
            x.Mov(target, uint32 value)

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

        member inline x.Load(target : Register, ptr : nativeint) =
            x.Mov(target, ptr)
            x.Load(target, target)

        member inline x.Load(target : Register, ptr : nativeptr<'a>) =
            x.Load(target, NativePtr.toNativeInt ptr)

        member x.Add(target : Register, source : Register) =
            let modRM = modRM (byte target) (byte source)

            writer.Write(0x03uy)
            writer.Write(modRM)

        member x.Add(target : Register, value : uint32) =
            if value > 0u then
                let r = target |> byte

                writer.Write(0x81uy)
                writer.Write(0xC0uy + r)
                writer.Write(value)

        member x.Sub(target : Register, value : uint32) = 
            if value > 0u then
                let r = target |> byte

                writer.Write(0x81uy)
                writer.Write(0xE8uy + r)
                writer.Write(value)

        member x.Sub(target : Register, source : Register) = 
            let modRM = modRM (byte source) (byte target)
            writer.Write(0x29uy)
            writer.Write(modRM)



        member x.Push(r : Register) =
            let r = r |> byte
            writer.Write(0x50uy + r)

        member x.Push(value : uint32) =
            writer.Write(0x68uy)
            writer.Write(value)

        member x.Push(value : float32) =
            writer.Write(0x68uy)
            writer.Write(value)
            

        member x.Pop(r : Register) =
            let r = r |> byte
            writer.Write(0x58uy + r)


        member x.Jmp(offset : int) =
            writer.Write 0xE9uy
            writer.Write offset

        member x.BeginCall(args : int) =
            argumentIndex <- args - 1

        member x.Call(cc : CallingConvention, r : Register) =
            let r = byte r
            writer.Write(0xFFuy)
            writer.Write(0xD0uy + r)

        member x.Call(cc : CallingConvention, ptr : nativeint) =
            x.Mov(Register.Eax, ptr)
            x.Call(cc, Register.Eax)


        member x.Ret() =
            writer.Write(0xC3uy)

         member x.PushArg(cc : CallingConvention, value : uint32) =
            let i = dec &argumentIndex
            if i < cc.registers.Length then
                x.Mov(cc.registers.[i], value)
            else
                x.Push(value)    
                argumentOffset <- argumentOffset + 4

         member x.PushArg(cc : CallingConvention, value : float32) =
            let i = dec &argumentIndex
            if i < cc.floatRegisters.Length then
                x.Mov(cc.floatRegisters.[i], value)
            else
                x.Push(value)  
                argumentOffset <- argumentOffset + 4

        member x.PushIntArg(cc : CallingConvention, r : Register) =
            let i = dec &argumentIndex
            if i < cc.registers.Length then
                x.Mov(cc.registers.[i], r)
            else
                x.Push(r)
                argumentOffset <- argumentOffset + 4

        member x.PushFloatArg(cc : CallingConvention, r : Register) =
            let i = dec &argumentIndex
            if i < cc.floatRegisters.Length then
                x.Mov(cc.floatRegisters.[i], r)
            else
                x.Push(r)
                argumentOffset <- argumentOffset + 4


        member private x.Dispose(disposing : bool) =
            if disposing then GC.SuppressFinalize(x)
            writer.Dispose()

        member x.Dispose() = x.Dispose(true)
        override x.Finalize() = x.Dispose(false)

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IAssemblerStream with
            member x.BeginFunction() = x.Begin()
            member x.EndFunction() = x.End()
            member x.BeginCall(args : int) = x.BeginCall(args)
            member x.Call (ptr : nativeint) = x.Call(localConvention, ptr)
            member x.PushArg(v : nativeint) = x.PushArg(localConvention, uint32 v)
            member x.PushArg(v : int) = x.PushArg(localConvention, uint32 v)
            member x.PushArg(v : float32) = x.PushArg(localConvention, v)
            member x.PushPtrArg(loc) = x.Load(Register.Eax, loc); x.PushIntArg(localConvention, Register.Eax)
            member x.PushIntArg(loc) = x.Load(Register.Eax, loc); x.PushIntArg(localConvention, Register.Eax)
            member x.PushFloatArg(loc) = x.Load(Register.Eax, loc); x.PushFloatArg(localConvention, Register.Eax)


            member x.Ret() = x.Ret()

            member x.WriteOutput(v : nativeint) = x.Mov(Register.Eax, v)
            member x.WriteOutput(v : int) = x.Mov(Register.Eax, v)
            member x.WriteOutput(v : float32) : unit = failwith "[X86] not implemented"
            member x.Jump(offset : int) = x.Jmp(offset)


        new(stream : Stream) = new AssemblerStream(stream, false)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module AssemblerStream =
    let ofStream (s : IO.Stream) =
        match sizeof<nativeint> with
            | 4 -> new X86.AssemblerStream(s) :> IAssemblerStream
            | 8 -> new AMD64.AssemblerStream(s) :> IAssemblerStream
            | _ -> failwith "bad bitness"

module Program =
    open System.IO

    let private jumpInt =
        use ms = new MemoryStream()
        use w = AssemblerStream.ofStream ms
        w.Jump(0)
        ms.ToArray() |> Array.take (int ms.Length - 4)

    let private ret =
        use ms = new MemoryStream()
        use w = AssemblerStream.ofStream ms
        w.EndFunction()
        w.Ret()
        ms.ToArray()

    let private jumpSize = nativeint jumpInt.Length + 4n
    
    [<AllowNullLiteral>]
    type private Fragment<'a> =
        class
            val mutable prev : Fragment<'a>
            val mutable next : Fragment<'a>
            val mutable public TotalJumpDistance : ref<int64>
            val mutable public Manager : MemoryManager
            val mutable public Pointer : managedptr
            val mutable public Tag : 'a
            val mutable public JumpDistance : int64

            member x.Dispose() =
                x.Manager.Free x.Pointer
                if not (isNull x.prev) then
                    x.prev.Next <- x.next
                    Interlocked.Add(x.TotalJumpDistance, -x.JumpDistance) |> ignore
                    x.JumpDistance <- 0L

            member private x.writeJump() =
                if not (isNull x.next) then
                    let target = x.next.Pointer.Offset
                    let source = x.Pointer.Offset + x.Pointer.Size - jumpSize
                    let offset = target - source - 5n |> int
                    let dist = abs (int64 offset)
                    Interlocked.Add(x.TotalJumpDistance, dist - x.JumpDistance) |> ignore
                    x.JumpDistance <- dist

                    let off = x.Pointer.Size - jumpSize |> int
                    x.Pointer.Write(off, jumpInt)
                    x.Pointer.Write(off + jumpInt.Length, offset)
                else
                    Interlocked.Add(x.TotalJumpDistance, -x.JumpDistance) |> ignore
                    x.JumpDistance <- 0L

                    let off = x.Pointer.Size - jumpSize |> int
                    x.Pointer.Write(off, ret)
                    


            member x.Capacity = x.Pointer.Size - jumpSize

            member x.EntryPointer =
                x.Pointer.Parent.Pointer + x.Pointer.Offset

            member x.Realloc(newCapacity : nativeint) : unit =
                let newCapacity = newCapacity + jumpSize
                if newCapacity <> x.Pointer.Size then
                    let moved = 
                        if x.Pointer.Free then 
                            x.Pointer <- x.Manager.Alloc(newCapacity)
                            x.writeJump()
                            true
                        else
                            x.Pointer |> ManagedPtr.realloc newCapacity

                    x.writeJump()
                    if moved && not (isNull x.prev) then
                        x.prev.writeJump()
                        
            member x.Prev
                with get() = x.prev

            member x.Next
                with get() = x.next
                and set n =
                    x.next <- n
                    if not (isNull n) then 
                        n.prev <- x

                    x.writeJump()

            member x.GetStream() : Stream =
                new FragmentStream<'a>(x) :> Stream

            member x.AssemblerStream =
                AssemblerStream.ofStream (x.GetStream())

            new(totalJumps, manager, tag) = { TotalJumpDistance = totalJumps; JumpDistance = 0L; Manager = manager; Tag = tag; Pointer = manager.Alloc(jumpSize); prev = null; next = null }
        end

    and private FragmentStream<'a>(f : Fragment<'a>) =
        inherit Stream()

        let mutable capacity = f.Capacity
        let mutable offset = 0n
        let mutable additional : MemoryStream = null //new MemoryStream()

        override x.CanRead = false
        override x.CanWrite = true
        override x.CanSeek = true

        override x.Dispose(disposing) =
            x.Flush()

            base.Dispose(disposing)
            let o = Interlocked.Exchange(&additional, null)
            if not (isNull o) then
                o.Dispose()
                capacity <- 0n
                offset <- 0n

        override x.Position
            with get() = int64 offset
            and set v = offset <- nativeint v
            
        override x.Length = int64 capacity + (if isNull additional then 0L else additional.Length)

        override x.Write(d, o, c) =
            let newOffset = offset + nativeint c
            if newOffset <= capacity then
                f.Pointer.Use (fun ptr ->
                    Marshal.Copy(d, o, ptr + offset, c)
                )
                offset <- newOffset
            else
                let additional =
                    match additional with
                        | null ->
                            let s = new MemoryStream()
                            additional <- s
                            s
                        | s -> s

                if offset < capacity then
                    let storable = capacity - offset
                    f.Pointer.Use (fun ptr ->
                        Marshal.Copy(d, o, ptr + offset, int storable)
                    )

                    additional.Position <- 0L
                    if c > int storable then
                        additional.Write(d, o + int storable, c - int storable)

                    offset <- newOffset

                else
                    additional.Position <- int64 (offset - capacity)
                    additional.Write(d, o, c)
                    offset <- newOffset


                


            ()

        override x.Read(d, o, c) =
            failwith ""

        override x.SetLength(l : int64) =
            if nativeint l > capacity then
                let additional =
                    match additional with
                        | null ->
                            let s = new MemoryStream()
                            additional <- s
                            s
                        | s -> s
                
                additional.SetLength(l - int64 capacity)

            else
                if not (isNull additional) then
                    additional.Dispose()
                    additional <- null

                f.Realloc(nativeint l)
                capacity <- nativeint l

        override x.Seek(o : int64, origin : SeekOrigin) =
            match origin with
                | SeekOrigin.Begin -> offset <- nativeint o; int64 offset
                | SeekOrigin.Current -> offset <- offset + nativeint o; int64 offset
                | _ -> offset <- nativeint (x.Length - o); int64 offset

        override x.Flush() =
            if not (isNull additional) then
                additional.Flush()
                f.Realloc(offset)

                let arr = additional.ToArray()
                f.Pointer.Use (fun ptr ->
                    Marshal.Copy(arr, 0, ptr + capacity, arr.Length)
                )
                additional.Dispose()
                additional <- null
                capacity <- f.Capacity
            elif offset <> capacity then
                f.Realloc(offset)
                capacity <- f.Capacity

    type NativeProgram<'a> private(data : alist<'a>, isDifferential : bool, compileDelta : Option<'a> -> 'a -> IAssemblerStream -> unit) =
        inherit AdaptiveObject()
        let compileDelta = OptimizedClosures.FSharpFunc<_,_,_,_>.Adapt(compileDelta)
        
        let mutable disposed = false
        let manager = MemoryManager.createExecutable()

        let jumpDistance = ref 0L
        let mutable count = 0

        let mutable prolog = 
            let f = new Fragment<'a>(jumpDistance, manager, Unchecked.defaultof<'a>)
            use s = f.AssemblerStream
            s.BeginFunction()
            f

        let reader = data.GetReader()
        let cache : SortedDictionaryExt<Index, Fragment<'a>> = SortedDictionary.empty

        let mutable entryPointer = 0n
        let mutable run : unit -> unit = id
        
        let release() =
            if not disposed then
                disposed <- true
                cache.Clear()
                reader.Dispose()
                manager.Dispose()
                prolog <- null
                entryPointer <- 0n
                run <- id
                jumpDistance := 0L
                count <- 0

        member x.AverageJumpDistance = 
            if !jumpDistance = 0L then 0.0
            else float !jumpDistance / float count

        member x.TotalJumpDistance = !jumpDistance

        member x.Update(token : AdaptiveToken) =
            x.EvaluateIfNeeded token () (fun token ->
                if disposed then
                    raise <| ObjectDisposedException("AdaptiveProgram")

                let ops = reader.GetOperations token

                let dirty = 
                    if isDifferential then HashSet<Fragment<'a>>()
                    else null

                for i, op in PDeltaList.toSeq ops do
                    match op with
                        | Remove ->
                            match cache.TryGetValue i with
                                | (true, f) -> 
                                    let n = f.Next
                                    f.Dispose()
                                    cache.Remove i |> ignore

                                    if isDifferential then 
                                        if not (isNull n) then dirty.Add n |> ignore
                                        dirty.Remove f |> ignore
                                    count <- count - 1
                                | _ ->
                                    ()

                        | Set v ->
                            cache |> SortedDictionary.setWithNeighbours i (fun l s r ->
                                let l = l |> Option.map snd
                                let r = r |> Option.map snd

                                let prev = 
                                    if isDifferential then
                                        match l with
                                            | Some f ->
                                                if f = prolog then None
                                                else Some f.Tag
                                            | None ->
                                                None
                                    else
                                        None

                                match s with
                                    | Some f ->
                                        f.Tag <- v
                                        using f.AssemblerStream (fun s -> compileDelta.Invoke(prev, v, s))
                                        if isDifferential && not (isNull f.next) then dirty.Add f.next |> ignore
                                        f

                                    | None ->
                                        let f = new Fragment<'a>(jumpDistance, manager, v)
                                        using f.AssemblerStream (fun s -> compileDelta.Invoke(prev, v, s))
                                        
                                        count <- count + 1
                                        match l with
                                            | None -> prolog.Next <- f
                                            | Some(p) -> p.Next <- f

                                        match r with
                                            | None -> f.Next <- null
                                            | Some(n) ->    
                                                if isDifferential then dirty.Add n |> ignore
                                                f.Next <- n

                                        f

                            ) |> ignore

                if isDifferential then
                    dirty.Remove prolog |> ignore
                    for d in dirty do
                        let prev =
                            if d.Prev = prolog then None
                            else Some d.Prev.Tag

                        using d.AssemblerStream (fun s -> compileDelta.Invoke(prev, d.Tag, s))

            )

        member x.Run() =
            lock x (fun () ->
                if disposed then
                    raise <| ObjectDisposedException("AdaptiveProgram")

                if entryPointer <> prolog.EntryPointer then
                    run <- UnmanagedFunctions.wrap prolog.EntryPointer
                    entryPointer <- prolog.EntryPointer

                run()
            )

        member private x.Dispose(disposing : bool) =
            if disposing then
                GC.SuppressFinalize x
                lock x release
            else
                release()

        member x.Dispose() = x.Dispose(true)
        override x.Finalize() = x.Dispose(false)


        interface IDisposable with
            member x.Dispose() = x.Dispose()

        new(data : alist<'a>, compile : Option<'a> -> 'a -> IAssemblerStream -> unit) = new NativeProgram<'a>(data, true, compile)
        new(data : alist<'a>, compile : 'a -> IAssemblerStream -> unit) = new NativeProgram<'a>(data, false, fun _ v s -> compile v s)
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module NativeProgram =
        let differential (compile : Option<'a> -> 'a -> IAssemblerStream -> unit) (values : alist<'a>) =
            new NativeProgram<'a>(values, compile)

        let simple (compile : 'a -> IAssemblerStream -> unit) (values : alist<'a>) =
            new NativeProgram<'a>(values, compile)

    
    let nopFunction =
        use s = new MemoryStream()
        use asm = AssemblerStream.ofStream s

        asm.BeginFunction()
        asm.EndFunction()
        asm.Ret()

        let arr = s.ToArray()
        let mem = ExecutableMemory.alloc (nativeint arr.Length)
        Marshal.Copy(arr, 0, mem, arr.Length)
        mem



    type MyDelegate = delegate of int * int * int * int -> unit
    
    type TestDelegate = delegate of int * int -> unit

    type TestProgram(values : alist<int>) =
        let mutable validate = false
        let store = List<int * int>(1024)
        let d = TestDelegate (fun p s -> if validate then store.Add(p,s))
        let dPtr = Marshal.PinDelegate d

        let program = 
            values |> NativeProgram.differential (fun prev self stream ->
                stream.BeginCall(2)
                stream.PushArg(self)
                stream.PushArg(defaultArg prev 0)
                stream.Call(nopFunction)
            )

        member x.AverageJumpDistance = program.AverageJumpDistance
        
        member x.Update() =
            program.Update(AdaptiveToken.Top)
            
        member x.Run() =
            program.Update(AdaptiveToken.Top)
            program.Run()

        member x.RunAndValidate() =
            x.Run()
//            validate <- true
//            store.Clear()
//            program.Update(AdaptiveToken.Top)
//            program.Run()
//            validate <- false
//            let calls = store |> CSharpList.toArray
//            let ref = values.Content |> Mod.force
//            let mutable error = false
//            let mutable l = 0
//            let mutable i = 0
//            for r in ref do
//                let should = (l,r)
//                let is = calls.[i]
//
//                if should <> is then error <- true
//
//                i <- i + 1
//                l <- r
//
//            if error then
//                Log.warn "ERROR: %A vs %A" calls ref 
//            else
//                Log.line "calls: %d" calls.Length





    let report =
        MyDelegate (fun a b c d ->
            Log.start "cb"
            Log.line "a: %A" a
            Log.line "b: %A" b
            Log.line "c: %A" c
            Log.line "d: %A" d
            Log.stop()
        )


    let pReport = Marshal.PinDelegate(report)


    let runInsertBenchmark() =
        let values = clist [0;1;2;3]
        let prog = TestProgram(values)
        let log = @"C:\Users\Schorsch\Desktop\update.csv"
        File.WriteAllLines(log, ["size;apply;update;run"])
        for s in 500 .. 500 .. 100000 do
            printf "%d: " s
            transact (fun () -> values.Clear())
            prog.Update()

            let sw = System.Diagnostics.Stopwatch.StartNew()
            transact (fun () -> values.AppendMany [1 .. s])
            sw.Stop()
            let apply = sw.MicroTime
            sw.Restart()
            prog.Update()
            sw.Stop()
            let update = sw.MicroTime
            sw.Restart()
            for i in 1 .. 1000 do
                prog.Run()
            sw.Stop()
            let run = sw.MicroTime / 1000.0


            printfn "{ apply: %A; update: %A; run: %A }" apply update run
            File.AppendAllLines(log, [sprintf "%d;%d;%d;%d" s apply.TotalNanoseconds update.TotalNanoseconds run.TotalNanoseconds])

        Environment.Exit(0)

    let test() =
        if sizeof<nativeint> = 4 then Log.warn "32 bit"
        else Log.warn "64 bit"

        let values = clist [0;1;2;3]
        let prog = TestProgram(values)

        
        Log.startTimed "run"
        prog.RunAndValidate()
        Log.line "dist: %A" prog.AverageJumpDistance
        Log.stop()
        
        Log.startTimed "run"
        transact (fun () -> values.Insert(2, 100))
        prog.Update()
        prog.RunAndValidate()
        Log.line "dist: %A" prog.AverageJumpDistance
        Log.stop()
        
        
        Log.startTimed "run"
        transact (fun () -> values.[2] <- 2)
        prog.Update()
        prog.RunAndValidate()
        Log.line "dist: %A" prog.AverageJumpDistance
        Log.stop()
        
        Log.startTimed "run"
        transact (fun () -> values.RemoveAt(2))
        prog.Update()
        prog.RunAndValidate()
        Log.line "dist: %A" prog.AverageJumpDistance
        Log.stop()


        Log.startTimed "apply"
        transact (fun () -> values.AppendMany [4 .. 1 <<< 16])
        Log.stop()
        Log.startTimed "update"
        prog.Update()
        Log.stop()
        Log.startTimed "run x 1000"
        for i in 1 .. 1000 do
            prog.Run()
        Log.line "dist: %A" prog.AverageJumpDistance
        Log.stop()

        Log.startTimed "run"
        prog.RunAndValidate()
        Log.stop()



        Log.startTimed "apply"
        transact (fun () -> values.Clear())
        Log.stop()
        Log.startTimed "update"
        prog.Update()
        Log.stop()
        Log.startTimed "run"
        for i in 1 .. 1000 do
            prog.Run()
        Log.line "dist: %A" prog.AverageJumpDistance
        Log.stop()



        Environment.Exit 0



module Benchmark =
    open System.Diagnostics

    type MyDelegate = delegate of float32 * float32 * int64 * float32 * int * float32 * int * float32 -> unit

    let callback =
        MyDelegate (fun a b c d e f g h ->
            printfn "a: %A" a
            printfn "b: %A" b
            printfn "c: %A" c
            printfn "d: %A" d
            printfn "e: %A" e
            printfn "f: %A" f
            printfn "g: %A" g
            printfn "h: %A" h
        )

    let pDel = Marshal.PinDelegate(callback)
    let ptr = pDel.Pointer

    let cnt = 1 <<< 12

    let args = [| 1.0f :> obj; 2.0f :> obj; 3L :> obj; 4.0f :> obj; 5 :> obj; 6.0f :> obj; 7 :> obj; 8.0f :> obj |]
    let calls = Array.init cnt (fun _ -> ptr, args)

    let runOld(iter : int) =
        // warmup
        for i in 1 .. 10 do
            Aardvark.Base.Assembler.compileCalls 0 calls |> ignore

        let sw = Stopwatch.StartNew()
        for i in 1 .. iter do
            Aardvark.Base.Assembler.compileCalls 0 calls |> ignore
        sw.Stop()

        sw.MicroTime / (float iter)

    let cc = AMD64.CallingConvention.windows
    let fillNew(cnt) =
        use s = new MemoryStream()
        use w = new AMD64.AssemblerStream(s)

        w.Begin()

        for i in 1 .. cnt do
            w.BeginCall(8)
            w.PushArg(cc, 7.0f)
            w.PushArg(cc, 6u)
            w.PushArg(cc, 5.0f)
            w.PushArg(cc, 4u)
            w.PushArg(cc, 3.0f)
            w.PushArg(cc, 2UL)
            w.PushArg(cc, 1.0f)
            w.PushArg(cc, 0.0f)
            w.Call(cc, ptr)

        w.End()
        w.Ret()
        //s.ToArray()

    let runNew(iter : int) =
        // warmup
        for i in 1 .. 10 do
            fillNew(cnt) |> ignore

        let sw = Stopwatch.StartNew()
        for i in 1 .. iter do
            fillNew(cnt) |> ignore
        sw.Stop()

        sw.MicroTime / (float iter)

    let run() =
//        let size = 1 <<< 14
//        while true do
//            fillNew size

        let ot = runOld 100
        let throughput = float cnt / ot.TotalSeconds
        Log.line "old: %A (%.0fc/s)" ot throughput

        let nt = runNew 100
        let throughput = float cnt / nt.TotalSeconds
        Log.line "new: %A (%.0fc/s)" nt throughput

        let speedup = ot / nt
        Log.line "factor: %A" speedup

module Test =

    type MyDelegate = delegate of float32 * float32 * int * float32 * int * float32 * int * float32 -> unit

    let callback =
        MyDelegate (fun a b c d e f g h ->
            Log.start "call"
            Log.line "a: %A" a
            Log.line "b: %A" b
            Log.line "c: %A" c
            Log.line "d: %A" d
            Log.line "e: %A" e
            Log.line "f: %A" f
            Log.line "g: %A" g
            Log.line "h: %A" h
            Log.stop()
        )

    let ptr = Marshal.PinDelegate(callback)

    let run () =
        let store = NativePtr.alloc 1
        NativePtr.write store 100.0f

        use stream = new NativeStream()
        use asm = AssemblerStream.ofStream stream

        asm.BeginFunction()

        asm.BeginCall(8)
        asm.PushFloatArg(NativePtr.toNativeInt store)
        asm.PushArg(6)
        asm.PushArg(5.0f)
        asm.PushArg(4)
        asm.PushArg(3.0f)
        asm.PushArg(2)
        asm.PushArg(1.0f)
        asm.PushArg(0.0f)
        asm.Call(ptr.Pointer)
 
        asm.BeginCall(8)
        asm.PushArg(17.0f)
        asm.PushArg(16)
        asm.PushArg(15.0f)
        asm.PushArg(14)
        asm.PushArg(13.0f)
        asm.PushArg(12)
        asm.PushArg(11.0f)
        asm.PushArg(10.0f)
        asm.Call(ptr.Pointer)

        asm.WriteOutput(1234)
        asm.EndFunction()
        asm.Ret()
        
        let size = Fun.NextPowerOfTwo stream.Length |> nativeint
        let mem = ExecutableMemory.alloc size
        Marshal.Copy(stream.Pointer, mem, stream.Length)

        let managed : int -> int = UnmanagedFunctions.wrap mem
        Log.start "run(3)"

        if sizeof<nativeint> = 4 then Log.warn "32 bit"
        else Log.warn "64 bit"


        let res = managed 3
        Log.line "ret: %A" res
        Log.stop()

        ExecutableMemory.free mem size

        Environment.Exit 0


type MyDelegate = delegate of int * int * int * int64 * int64 -> unit // * int64 * int64 * int64 * int64 -> unit

open AMD64

[<EntryPoint; STAThread>]
let main argv = 
//    Program.test()
//    Environment.Exit 0


    let callback =
        MyDelegate (fun a b c d  e (* f g h *) ->
            printfn "a: %A" a
            printfn "b: %A" b
            printfn "c: %A" c
            printfn "d: %A" d
            printfn "e: %A" e
//            printfn "f: %A" f
//            printfn "g: %A" g
//            printfn "h: %A" h
        )

    let ptr = Marshal.PinDelegate(callback)

    use stream = new NativeStream()
    use asm = new AssemblerStream(stream)

    let data = NativePtr.alloc 1
    NativePtr.write data 12321.0

    let cc = CallingConvention.windows

    asm.Begin()
//    asm.Mov(Register.XMM0, 1234.0f)
//    asm.Push(Register.XMM0)


    asm.BeginCall(5)
//    asm.PushArg(cc, 1234UL)
//    asm.PushArg(cc, 1234UL)
//    asm.PushArg(cc, 1234UL)
    asm.PushArg(cc, 1234UL)
    asm.PushArg(cc, 4321UL)
    asm.PushArg(cc, 3u)
    asm.PushArg(cc, 2u)
    asm.PushArg(cc, 1u)
    asm.Call(cc, ptr.Pointer)
 

    asm.BeginCall(5)
//    asm.PushArg(cc, 1234UL)
//    asm.PushArg(cc, 1234UL)
//    asm.PushArg(cc, 1234UL)
    asm.PushArg(cc, 1234UL)
    asm.PushArg(cc, 4321UL)
    asm.PushArg(cc, 3u)
    asm.PushArg(cc, 2u)
    asm.PushArg(cc, 1u)
    asm.Call(cc, ptr.Pointer)

//    asm.BeginCall(5)
////    asm.PushArg(cc, 17.0f)
////    asm.PushArg(cc, 16u)
////    asm.PushArg(cc, 15.0f)
//    asm.PushArg(cc, 14u)
//    asm.PushArg(cc, 13.0f)
//    asm.PushArg(cc, 12UL)
//    asm.PushArg(cc, 11.0f)
//    asm.PushArg(cc, 10.0f)
//    asm.Call(cc, ptr.Pointer)
 
    
//    asm.Pop(Register.XMM0)
    asm.End()
    asm.Ret()

    let size = Fun.NextPowerOfTwo stream.Length |> nativeint
    let mem = ExecutableMemory.alloc size
    Marshal.Copy(stream.Pointer, mem, stream.Length)
//
//    let arr : byte[] = Array.zeroCreate (int stream.Length)
//    Marshal.Copy(stream.Pointer, arr, 0, arr.Length)
//    arr |> Seq.map (sprintf "0x%2Xuy") |> String.concat "; " |> printfn "[| %s |]"

    let managed : int -> float32 = UnmanagedFunctions.wrap mem
    let res = managed 3
    printfn "res = %A" res
    ExecutableMemory.free mem size

    Environment.Exit 0

    let rand = RandomSystem()
    let g = UndirectedGraph.ofNodes (Set.ofList [0..127]) (fun li ri -> float (ri - li) |> Some)

    let tree = UndirectedGraph.maximumSpanningTree compare g
    printfn "%A" tree

    printfn "%A" (Tree.weight tree / float (Tree.count tree))



    //React.Test.run()
    Environment.Exit 0

    0 // return an integer exit code
