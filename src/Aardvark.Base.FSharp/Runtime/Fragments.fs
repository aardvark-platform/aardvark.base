namespace Aardvark.Base

open System
open System.Collections.Generic
open System.Threading
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"

type FlatList<'a>() =
    let mutable content = List<'a>()
    let mutable offsets = List<int>()

    let getOffsetAndCount (id : int) =
        let offset = offsets.[id]
        if id < offsets.Count-1 then
            (offset, offsets.[id+1] - offset)
        else
            (offset, content.Count - offset)

    member x.Append(values : seq<'a>) =
        let arr = Seq.toArray values
        let id = offsets.Count
        offsets.Add(content.Count)

        content.AddRange values

        id

    member x.Update(id : int, values : seq<'a>) =
        let arr = Seq.toArray values

        if id >= 0 && id < offsets.Count then
            let offset, count = getOffsetAndCount id
            if arr.Length < count then
                // adjust content to match our new size
                let shrinked = count - arr.Length
                content.RemoveRange(offset + arr.Length, shrinked)

                // copy the contents
                let mutable ci = offset
                for i in 0..arr.Length-1 do
                    content.[ci] <- arr.[i]
                    ci <- ci + 1

                // adjust all following offsets
                for id' in (id + 1)..(offsets.Count - 1) do
                    offsets.[id'] <- offsets.[id'] - shrinked

            elif arr.Length > count then
                // adjust content to match our new size
                let grow = arr.Length - count
                content.InsertRange(offset, Array.zeroCreate grow)

                // copy the contents
                let mutable ci = offset
                for i in 0..arr.Length-1 do
                    content.[ci] <- arr.[i]
                    ci <- ci + 1

                // adjust all following offsets
                for id' in (id + 1)..(offsets.Count - 1) do
                    offsets.[id'] <- offsets.[id'] + grow


            else
                let mutable ci = offset
                for i in 0..arr.Length-1 do
                    content.[ci] <- arr.[i]
                    ci <- ci + 1


        else
            failwithf "[FlatList] could not get offset for id: %A" id

    member x.Clear() =
        content.Clear()
        offsets.Clear()

    member x.Clear(id : int) =
        x.Update(id, [||])
    
    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = content.GetEnumerator() :> System.Collections.IEnumerator

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() = content.GetEnumerator() :> System.Collections.Generic.IEnumerator<'a>



module AMD64 =
    open System.IO

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

    type Value =
        | Dword of uint32
        | Qword of uint64

    type Instruction =
        | Mov of Register * Value
        | CallRax
        | Push of offset : int * Value
        | Jmp of offset : int
        | Nop of width : int
        | Ret

    type ArgumentLocation =
        | RegisterArgument of Register
        | StackArgument

    type CallingConvention = { registers : Register[] }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module CallingConvention =
        
        let getArgumentLocation (arg : int) (cc : CallingConvention) =
            if arg < cc.registers.Length then RegisterArgument cc.registers.[arg]
            else StackArgument

        let tryFindRegister (arg : int) (cc : CallingConvention) =
            if arg < cc.registers.Length then Some cc.registers.[arg]
            else None

        let tryFindStackIndex (arg : int) (cc : CallingConvention) =
            if arg >= cc.registers.Length then Some (arg - cc.registers.Length)
            else None

    let windows = { registers = [| Register.Rcx; Register.Rdx; Register.R8; Register.R9 |] }
    let linux = { registers = [| Register.Rdi; Register.Rsi; Register.Rdx; Register.Rcx; Register.R8; Register.R9 |] }

    module Compiler =
        let value (o : obj) =
            match o with
                | :? nativeint as o -> Qword(uint64 o)
                | :? int64 as o -> Qword(uint64 o)
                | :? int as o -> Dword(uint32 o)
                | :? unativeint as o -> Qword(uint64 o)
                | :? uint64 as o -> Qword(o)
                | :? uint32 as o -> Dword(o)
                | _ -> failwithf "unsupported argument-type: %A" (o.GetType())

        let stackSize(v : Value) =
            match v with
                | Qword _ -> 8
                | Dword _ -> 8

        let setArg (cc : CallingConvention) (index : int) (stackOffset : int) (arg : obj) =
            match CallingConvention.getArgumentLocation index cc with
                | RegisterArgument reg -> 
                    stackOffset, [ Mov(reg, value arg) ]
                | StackArgument -> 
                    let v = value arg
                    let s = stackSize v
                    stackOffset + s, [ Push(stackOffset, value arg) ]

        let call (ptr : nativeint) =
            [ 
                Mov(Register.Rax, Qword(uint64 ptr))
                CallRax
            ]

        let compileCalls (cc : CallingConvention) (calls : seq<nativeint * obj[]>) =
            let res = List<Instruction>()

            for f,args in calls do
                let mutable stackOffset = 32

                for i in 0..args.Length-1 do
                    let (newOffset,code) = setArg cc i stackOffset args.[i]
                    stackOffset <- newOffset
                    res.AddRange(code)

                res.AddRange(call f)

            res.ToArray()

        let compileCall (cc : CallingConvention) (f : nativeint) (args : array<obj>) =
            compileCalls cc [f,args]

    module Assembler =

        let private oneByteNop       = [|0x90uy|]
        let private twoByteNop       = [|0x66uy; 0x90uy|]
        let private threeByteNop     = [|0x0Fuy; 0x1Fuy; 0x00uy|]
        let private fourByteNop      = [|0x0Fuy; 0x1Fuy; 0x40uy; 0x00uy|]
        let private fiveByteNop      = [|0x0Fuy; 0x1Fuy; 0x44uy; 0x00uy; 0x00uy|]
        let private eightByteNop     = [|0x0Fuy; 0x1Fuy; 0x84uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy|]

        let private printBits (arr : byte[]) =
            let printSingleBits (b : byte) =
                for i in 0..7 do
                    printf "%d" ((b >>> (7 - i)) &&& 1uy)

            arr |> Seq.iter printSingleBits
            printfn ""

        let private immediateMov64 =
            [|
            
                // MOVQ encoding (according to AMD manual)
                // TODO: not quite clear why b=1 and w=1 for [R8:R15]
                // [- 4 -] w r x b | [- 0xB8 + id -]
                // 7 6 5 4 3 2 1 0 | 7 6 5 4 3 2 1 0
                (* Register.Rax *) [| 0x48uy; 0xB8uy |]
                (* Register.Rcx *) [| 0x48uy; 0xB9uy |]
                (* Register.Rdx *) [| 0x48uy; 0xBAuy |]
                (* Register.Rbx *) [| 0x48uy; 0xBBuy |]
                (* Register.Rsp *) [| 0x48uy; 0xBCuy |]
                (* Register.Rbp *) [| 0x48uy; 0xBDuy |]
                (* Register.Rsi *) [| 0x48uy; 0xBEuy |]
                (* Register.Rdi *) [| 0x48uy; 0xBFuy |]
                (* Register.R8  *) [| 0x49uy; 0xB8uy |]
                (* Register.R9  *) [| 0x49uy; 0xB9uy |]
                (* Register.R10 *) [| 0x49uy; 0xBAuy |]
                (* Register.R11 *) [| 0x49uy; 0xBBuy |]
                (* Register.R12 *) [| 0x49uy; 0xBCuy |]
                (* Register.R13 *) [| 0x49uy; 0xBDuy |]
                (* Register.R14 *) [| 0x49uy; 0xBEuy |]
                (* Register.R15 *) [| 0x49uy; 0xBFuy |]
            |]

        let private immediateMov32 =
            [|
                // MOVD encoding (according to AMD manual)
                // TODO: not quite clear why b=1 for [r8d:r15d]
                // [- 4 -] w r x b | [- 0xB8 + id -]
                // 7 6 5 4 3 2 1 0 | 7 6 5 4 3 2 1 0

                (* Register.Rax *) [| 0xB8uy |]          
                (* Register.Rcx *) [| 0xB9uy |]          
                (* Register.Rdx *) [| 0xBAuy |]          
                (* Register.Rbx *) [| 0xBBuy |]          
                (* Register.Rsp *) [| 0xBCuy |]          
                (* Register.Rbp *) [| 0xBDuy |]          
                (* Register.Rsi *) [| 0xBEuy |]          
                (* Register.Rdi *) [| 0xBFuy |]          
                                                 
                (* Register.R8  *) [| 0x41uy; 0xB8uy |]  
                (* Register.R9  *) [| 0x41uy; 0xB9uy |]  
                (* Register.R10 *) [| 0x41uy; 0xBAuy |]  
                (* Register.R11 *) [| 0x41uy; 0xBBuy |]  
                (* Register.R12 *) [| 0x41uy; 0xBCuy |]  
                (* Register.R13 *) [| 0x41uy; 0xBDuy |]  
                (* Register.R14 *) [| 0x41uy; 0xBEuy |]  
                (* Register.R15 *) [| 0x41uy; 0xBFuy |]  
            |]
      
        let assembleMov (target : Register) (value : Value) (stream : BinaryWriter) =
            match value with
                | Qword q -> 
                    stream.Write immediateMov64.[int target]
                    stream.Write q 
                | Dword d ->
                    stream.Write immediateMov32.[int target]
                    stream.Write d

        let assemblePush (stackOffset : int) (value : Value) (stream : BinaryWriter) =
            stream |> assembleMov Register.Rax value
            stream.Write([| 0x48uy; 0x89uy; 0x44uy; 0x24uy; byte stackOffset |])

        let assembleCallRax (stream : BinaryWriter) =
            stream.Write [|0xFFuy; 0xD0uy|]

        let assembleJmp (offset : int) (stream : BinaryWriter) =
            stream.Write 0xE9uy
            stream.Write offset

        let rec assembleNop (width : int) (stream : BinaryWriter) =
            if width > 0 then
                match width with
                    | 1 -> stream.Write oneByteNop
                    | 2 -> stream.Write twoByteNop
                    | 3 -> stream.Write threeByteNop
                    | 4 -> stream.Write fourByteNop
                    | 5 -> stream.Write fiveByteNop
                    | 6 -> stream.Write threeByteNop; stream.Write threeByteNop // TODO: find good 6 byte nop sequence
                    | 7 -> stream.Write fourByteNop; stream.Write threeByteNop // TODO: find good 7 byte nop sequence
                    | _ -> stream.Write eightByteNop; assembleNop (width - 8) stream

        let assembleRet (stream : BinaryWriter) =
            stream.Write 0xC3uy

        let assembleInstruction (i : Instruction) (stream : BinaryWriter) =
            match i with
                | Mov(r, value)       -> stream |> assembleMov r value
                | Push(offset, value) -> stream |> assemblePush offset value
                | CallRax             -> stream |> assembleCallRax
                | Ret                 -> stream |> assembleRet
                | Jmp o               -> stream |> assembleJmp o
                | Nop w               -> stream |> assembleNop w

        let assembleTo (stream : Stream) (instructions : Instruction[]) =
            let writer = new BinaryWriter(stream, Text.ASCIIEncoding.ASCII, true)
            for i in instructions do
                writer |> assembleInstruction i

        let assemble (instructions : Instruction[]) =
            use s = new MemoryStream()
            instructions |> assembleTo s
            s.Flush()
            s.ToArray()


    module DisassemblerOld =
        let disassemble (stream : BinaryReader) =
            let b = stream.ReadByte()

            let instructions = List<Instruction>()
            try
                while stream.BaseStream.Position < stream.BaseStream.Length do
                    let instruction = 
                        match b with
                            | 0x48uy ->
                                let n = stream.ReadBytes(6)
                                match n with
                                    | [|0x89uy; 0x44uy; 0x24uy; stackOffset; 0x44uy; 0x24uy|] ->
                                        let stackOffset = stream.ReadByte() |> int
                                        match instructions.[instructions.Count - 1] with
                                            | Mov(Register.Rax, v) ->
                                                instructions.RemoveAt(instructions.Count - 1)
                                                Push(stackOffset, v)
                                            | _ ->
                                                failwith "push without write to rax"
                                    | arr ->
      
                                        let r = arr.[0] - 0xB8uy |> int |> unbox<Register>

                                        let rest = stream.ReadBytes(3)
                                        let v = System.BitConverter.ToUInt64(Array.append arr.[1..] rest, 0)
    
                                        Mov(r, Qword(v))

                            | 0x49uy ->
                                let rid = stream.ReadByte() - 0xB8uy |> int
                                let r = rid + 8 |> unbox<Register>
                                Mov(r, Qword(stream.ReadUInt64()))

                            | 0x41uy ->
                                let rid = stream.ReadByte() - 0xB8uy |> int
                                let r = rid + 8 |> unbox<Register>
                                Mov(r, Dword(stream.ReadUInt32()))

                            | v when v >= 0xB8uy && v <= 0xBFuy ->
                                let r = int (v - 0xB8uy) + 8 |> unbox<Register>
                                Mov(r, Dword(stream.ReadUInt32()))

                            | 0xFFuy ->
                                match stream.ReadByte() with
                                    | 0xD0uy -> CallRax
                                    | _ -> failwith "invalid instruction"

                            | 0xE9uy ->
                                let offset = stream.ReadInt32()
                                Jmp(offset)

                            | 0x90uy ->
                                Nop 1

                            | 0x66uy ->
                                match stream.ReadByte() with
                                    | 0x90uy -> Nop 2
                                    | _ -> failwith "invalid instruction"

                            | 0x0Fuy ->
                                match stream.ReadByte() with
                                    | 0x1Fuy -> 
                                        match stream.ReadByte() with
                                            | 0x00uy -> Nop 3
                                            | 0x40uy -> 
                                                if stream.ReadByte() <> 0x00uy then failwith "invalid instruction"
                                                Nop 4

                                            | 0x44uy ->
                                                if stream.ReadBytes(2) <> [|0x00uy; 0x00uy|] then failwith "invalid instruction"
                                                Nop 5
                                            | 0x84uy ->
                                                if stream.ReadBytes(5) <> [|0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy|] then failwith "invalid instruction"
                                                Nop 8
                                            | _ ->
                                                failwith "invalid instruction"
                                    | _ ->
                                        failwith "invalid instruction"

                            | 0xC3uy ->
                                Ret

                            | _ -> failwith "invalid instruction"
//
//                    let rec mergeNops (width : int) =
//                        match instructions.[instructions.Count - 1] with
//                            | Nop w -> 
//                                instructions.RemoveAt(instructions.Count - 1)
//                                mergeNops (width + w)
//                            | _ ->
//                                Nop width
//
//                    let instruction =
//                        match instruction with
//                            | Nop w -> mergeNops w
//                            | _ -> instruction

                    instructions.Add instruction
            with e ->
                ()
            instructions.ToArray()

    module Disassembler =
        
        let fail() = failwith "asdlmsadlm"

        let rec private disassembleAcc (current : list<Instruction>) (r : BinaryReader) =
            if r.BaseStream.Position = r.BaseStream.Length then
                current
            else
                let b0 = r.ReadByte()

                match b0 with
                    // NOP 1
                    | 0x90uy -> 
                        disassembleAcc ((Nop 1)::current) r

                    | 0x66uy ->
                        if r.ReadByte() = 0x90uy then disassembleAcc ((Nop 2)::current) r
                        else fail()

                    | 0x0Fuy ->
                        let b = r.ReadBytes(2)
                        match b with
                            | [| 0x1Fuy; 0x00uy |] -> 
                                disassembleAcc ((Nop 3)::current) r

                            | [| 0x1Fuy; 0x40uy |] -> 
                                r.ReadByte() |> ignore
                                disassembleAcc ((Nop 4)::current) r

                            | [| 0x1Fuy; 0x44uy |] -> 
                                r.ReadBytes(2) |> ignore
                                disassembleAcc ((Nop 5)::current) r

                            | [| 0x1Fuy; 0x84uy |] -> 
                                r.ReadBytes(5) |> ignore
                                disassembleAcc ((Nop 8)::current) r

                            | _ ->
                                fail()

                    // MOVQ lo registers
                    | 0x48uy -> 
                        let b1 = r.ReadByte()
                        match b1 with
                            | 0x89uy ->
                                if r.ReadBytes(2) = [|0x44uy; 0x24uy|] then
                                    let stackOffset = r.ReadByte() |> int
                                    match current with
                                        | Mov(Register.Rax, v)::rest ->
                                            disassembleAcc (Push(stackOffset, v)::rest) r
                                        | _ ->
                                            fail()

                                else
                                    fail()
                            | _ ->
                                let reg = b1 - 0xB8uy |> int |> unbox<Register>
                                let arg = r.ReadUInt64() |> Qword
                                disassembleAcc (Mov(reg, arg)::current) r

                    // MOVQ hi registers
                    | 0x49uy -> 
                        let reg = r.ReadByte() - 0xB8uy + 8uy |> int |> unbox<Register>
                        let arg = r.ReadUInt64() |> Qword
                        disassembleAcc (Mov(reg, arg)::current) r

                    // MOVD lo registers
                    | reg when reg >= 0xB8uy && reg <= 0xBFuy ->
                        let reg = reg - 0xB8uy |> int |> unbox<Register>
                        let arg = r.ReadUInt32() |> Dword
                        disassembleAcc (Mov(reg, arg)::current) r

                    // MOVD hi registers
                    | 0x41uy ->
                        let reg = r.ReadByte() - 0xB8uy + 8uy |> int |> unbox<Register>
                        let arg = r.ReadUInt32() |> Dword
                        disassembleAcc (Mov(reg, arg)::current) r

                    // CALL
                    | 0xFFuy ->
                        if r.ReadByte() = 0xD0uy then disassembleAcc (CallRax::current) r
                        else fail()

                    | 0xE9uy ->
                        let dist = r.ReadInt32()
                        (Jmp(dist)::current)

                    | _ ->
                        fail()


        let disassembleFrom (r : BinaryReader) =
            let res = disassembleAcc [] r

            let rec flattenNops (l : list<Instruction>) =
                match l with
                    | (Nop l)::(Nop r)::rest -> flattenNops <| (Nop (l+r))::rest
                    | i::rest -> i::flattenNops rest
                    | [] -> []

            res |> List.rev |> flattenNops |> List.toArray


        let disassemble (arr : byte[]) =
            use ms = new MemoryStream(arr)
            let reader = new BinaryReader(ms)
            disassembleFrom reader


type NativeCall = nativeint * obj[]

[<AutoOpen>]
module private FragmentConstants =
    let encodedJumpSize = 8
    let jumpSize = 5
    
[<AllowNullLiteral>]
type Fragment(manager : MemoryManager, content : byte[]) =
    let mutable memory = manager |> MemoryManager.alloc (content.Length + encodedJumpSize)
    let mutable containsJmp = false
    let mutable prev : Fragment = null
    let mutable next : Fragment = null
    do memory |> MemoryBlock.writeArray 0 content

    let nonAlignedJumpOffset() =
        memory.Size - encodedJumpSize
        
    let alignedJumpArgumentOffset() =
        let jmpStart = 1 + nonAlignedJumpOffset()
        (jmpStart + 3) &&& ~~~3

    member x.Instructions =
        memory 
            |> MemoryBlock.readArray 0
            |> AMD64.Disassembler.disassemble

    member x.EntryPointer =
        memory.Offset

    member x.Memory =
        memory

    member x.Prev 
        with get() = prev
        and set p = prev <- p

    member x.Next 
        with get() = next
        and set n = 
            next <- n
            x.NextPointer <- n.EntryPointer

    member x.NextPointer
        with get() : nativeint =
            let jmpArgOffset = alignedJumpArgumentOffset()
            let jmpArg : int = memory |> MemoryBlock.read jmpArgOffset
                
            let nextPtr = 
                memory.Offset +                 // where am i located
                nativeint (jmpArgOffset + 4) +  // where is the jmp instruction
                nativeint jmpArg                // what's the argument of the jmp instruction

            nextPtr

        and set (ptr : nativeint) : unit =
            let jmpArgOffset = alignedJumpArgumentOffset()

            let jmpEndLocation = memory.Offset + nativeint (jmpArgOffset + 4)

            let jmpArg = ptr - jmpEndLocation

            if containsJmp then
                // atomic write here since the offset is 4-byte aligned
                memory |> MemoryBlock.write jmpArgOffset (int jmpArg)
            else
                // if (for some reason) the fragment does currently not end
                // with a jmp instruction we need to re-create it
                let jmpStart = nonAlignedJumpOffset()
                let nopSize = (jmpArgOffset - 1) - jmpStart

                let jmp = AMD64.Assembler.assemble [| AMD64.Instruction.Nop(nopSize); AMD64.Instruction.Jmp(int jmpArg) |]
                memory |> MemoryBlock.writeArray jmpStart jmp
                containsJmp <- true

    member x.Write (calls : NativeCall[]) =
        // assemble the calls
        let binary = 
            calls |> AMD64.Compiler.compileCalls AMD64.windows
                  |> AMD64.Assembler.assemble

        x.Write binary

    member x.Write (binary : byte[]) =
        let size = binary.Length + encodedJumpSize

        if size <> memory.Size then
            let moved =  memory |> MemoryBlock.realloc size

            // patch our own next-pointer since its location was changed
            containsJmp <- false
            if isNull next then ()
            else x.NextPointer <- next.Memory.Offset

            // if the memory was moved we need to patch the prev's jmp-distance
            if moved && not (isNull prev) then 
                prev.NextPointer <- x.Memory.Offset

        memory |> MemoryBlock.writeArray 0 binary


    member x.Dispose() =
        if memory <> null then
            MemoryBlock.free memory
            memory <- null
            if not <| isNull next then next.Prev <- prev
            if not <| isNull prev then prev.Next <- x.Next
            next <- null
            prev <- null
            containsJmp <- false


    new(manager : MemoryManager, calls : NativeCall[]) = new Fragment(manager, calls |> AMD64.Compiler.compileCalls AMD64.windows |> AMD64.Assembler.assemble)
    new(manager : MemoryManager) = new Fragment(manager, ([||] : byte[]))





