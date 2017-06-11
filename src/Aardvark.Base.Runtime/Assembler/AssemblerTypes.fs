namespace Aardvark.Base.Runtime

open System
open System.Collections.Generic
open Aardvark.Base

[<CustomEquality; CustomComparison>]
type Register =
    struct
        val mutable public Name : string
        val mutable public Tag : int

        override x.GetHashCode() = x.Tag
        override x.Equals o =
            match o with
                | :? Register as o -> x.Tag = o.Tag
                | _ -> false

        override x.ToString() =
            x.Name
            
        member x.CompareTo (o : Register) =
            compare x.Tag o.Tag

        interface IComparable with
            member x.CompareTo o =
                match o with
                    | :? Register as o -> x.CompareTo o
                    | _ -> failwithf "[Register] cannot compare to %A" o

        interface IComparable<Register> with
            member x.CompareTo o = x.CompareTo o


        new(name : string, tag : int) = { Name = name; Tag = tag }

    end

type IAssemblerStream =
    inherit IDisposable

    abstract member Registers : Register[]
    abstract member CalleeSavedRegisters : Register[]
    abstract member ArgumentRegisters : Register[]
    abstract member ReturnRegister : Register

    abstract member Push : Register -> unit
    abstract member Pop : Register -> unit
    abstract member Mov : target : Register * source : Register -> unit
    abstract member Load : target : Register * source : Register * wide : bool -> unit
    abstract member Store : target : Register * source : Register * wide : bool -> unit


    /// emits a function-preamble (typically pushing the base-pointer, etc.)
    abstract member BeginFunction : unit -> unit

    /// emits a function-epilog (typically popping the base-pointer, etc.)
    abstract member EndFunction : unit -> unit

    /// switches to call-mode (no other calls then PushArg* are allowed between BeginCall and Call).
    /// Takes the number of arguments the function will take
    abstract member BeginCall : args : int -> unit

    /// calls the given function-pointer using the arguments pushed (via PushArg*) in reverse order
    abstract member Call : ptr : nativeint -> unit

    /// reads a pointer-sized integer value from the given location and pushes it onto the argument stack
    abstract member PushPtrArg : location : nativeint -> unit
    /// reads a 4 byte integer value from the given location and pushes it onto the argument stack
    abstract member PushIntArg : location : nativeint -> unit
    /// reads a 4 byte floating-point value from the given location and pushes it onto the argument stack
    abstract member PushFloatArg : location : nativeint -> unit

    /// pushes an immediate pointer-sized integer onto the argument stack
    abstract member PushArg : value : nativeint -> unit
    /// pushes an immediate 4 byte integer onto the argument stack
    abstract member PushArg : value : int -> unit
    /// pushes an immediate 4 byte floating-point onto the argument stack
    abstract member PushArg : value : float32 -> unit

    /// returns control to the caller
    abstract member Ret : unit -> unit

    /// writes a pointer-sized integer value to the output register
    abstract member WriteOutput : value : nativeint -> unit
    /// writes a 4 byte integer value to the output register
    abstract member WriteOutput : value : int -> unit
    /// writes a 4 byte floating-point value to the output register
    abstract member WriteOutput : value : float32 -> unit

    /// writes a relative jump to the stream (offset is relative to start of the instruction, e.g. offset=0 => nontermination)
    abstract member Jump : offset : int -> unit

