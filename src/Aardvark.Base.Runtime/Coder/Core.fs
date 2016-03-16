namespace Aardvark.Base

open System
open System.Reflection
open System.Runtime.CompilerServices


type IReader =
    abstract member ReadPrimitive<'a when 'a : unmanaged> : unit -> Code<'a>
    abstract member ReadString : unit -> Code<string>
    abstract member ReadBool : unit -> Code<bool>

type IWriter =
    abstract member WritePrimitive<'a when 'a : unmanaged> : 'a -> Code<unit>
    abstract member WriteString : string -> Code<unit>
    abstract member WriteBool : bool -> Code<unit>

type IValueCoder =
    abstract member ReadUnsafe : IReader -> Code<'a>
    abstract member WriteUnsafe : IWriter * 'a -> Code<unit>

type IValueCoder<'a> =
    inherit IValueCoder
    abstract member Read : IReader -> Code<'a>
    abstract member Write : IWriter * 'a -> Code<unit>

[<AbstractClass; Sealed; Extension>]
type ValueCoderExtensions private() =
    static let read = typeof<ValueCoderExtensions>.GetMethod("ReadState", BindingFlags.AnyStatic)
    static let write = typeof<ValueCoderExtensions>.GetMethod("WriteState", BindingFlags.AnyStatic)
    static let readUnsafe = typeof<ValueCoderExtensions>.GetMethod("ReadStateUnsafe", BindingFlags.AnyStatic)
    static let writeUnsafe = typeof<ValueCoderExtensions>.GetMethod("WriteStateUnsafe", BindingFlags.AnyStatic)

    [<Extension>]
    static member ReadState (this : IValueCoder<'a>, r : IReader, state : byref<CodeState>) =
        this.Read(r).Run(&state)

    [<Extension>]
    static member WriteState (this : IValueCoder<'a>, w : IWriter, value : 'a, state : byref<CodeState>) =
        this.Write(w,value).RunUnit(&state)

    [<Extension>]
    static member ReadStateUnsafe (this : IValueCoder, r : IReader, state : byref<CodeState>) =
        this.ReadUnsafe(r).Run(&state)

    [<Extension>]
    static member WriteStateUnsafe (this : IValueCoder, w : IWriter, value : 'a, state : byref<CodeState>) =
        this.WriteUnsafe(w,value).RunUnit(&state)


    static member GetReadMethod(coderType : Type, resType : Type) =
        let generic = coderType.GetInterface(typedefof<IValueCoder<_>>.AssemblyQualifiedName)
        if isNull generic then
            readUnsafe.MakeGenericMethod [|resType|]
        else
            let coderValueType = generic.GetGenericArguments().[0]
            if coderValueType = resType then read.MakeGenericMethod [|resType|]
            else readUnsafe.MakeGenericMethod [|resType|]

    static member GetWriteMethod(coderType : Type, valueType : Type) =
        let generic = coderType.GetInterface(typedefof<IValueCoder<_>>.AssemblyQualifiedName)
        if isNull generic then
            writeUnsafe.MakeGenericMethod [|valueType|]
        else
            let coderValueType = generic.GetGenericArguments().[0]
            if coderValueType = valueType then write.MakeGenericMethod [|valueType|]
            else writeUnsafe.MakeGenericMethod [|valueType|]

[<AbstractClass>]
type AbstractValueCoder<'a>() =
    abstract member Write : IWriter * 'a -> Code<unit>
    abstract member Read : IReader -> Code<'a>

    interface IValueCoder with
        member x.WriteUnsafe(w,v) = x.Write(w, unbox v)
        member x.ReadUnsafe(r) = x.Read(r) |> State.map unbox

    interface IValueCoder<'a> with
        member x.Write(w,v) = x.Write(w, v)
        member x.Read(r) = x.Read(r)

[<AbstractClass>]
type AbstractStateValueCoder<'a>() =
    inherit AbstractValueCoder<'a>()

    abstract member WriteState : IWriter * 'a * byref<CodeState> -> unit
    abstract member ReadState : IReader * byref<CodeState> -> 'a

    override x.Read(r) =
        { new Code<'a>() with
            override __.Run(s) = x.ReadState(r, &s)
        }

    override x.Write(w,v) =
        { new Code<unit>() with
            override __.RunUnit(s) = x.WriteState(w, v, &s)
        }



