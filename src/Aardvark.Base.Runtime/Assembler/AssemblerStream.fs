namespace Aardvark.Base.Runtime

open System
open System.IO

#nowarn "44"

[<Obsolete("use Aardvark.Assembler")>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module AssemblerStream =
    let ofStream (s : Stream) =
        match sizeof<nativeint> with
            //| 4 -> new X86.AssemblerStream(s) :> IAssemblerStream
            | 8 -> new AMD64.AssemblerStream(s) :> IAssemblerStream
            | _ -> failwith "bad bitness"
