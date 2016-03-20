namespace Aardvark.Base

open System
open System.Reflection
open System.Runtime.CompilerServices


type IReader =
    abstract member ReadPrimitive<'a when 'a : unmanaged> : unit -> Code<'a>
    abstract member ReadPrimitiveArray<'a when 'a : unmanaged> : unit -> Code<'a[]>
    abstract member ReadString : unit -> Code<string>
    abstract member ReadBool : unit -> Code<bool>

type IWriter =
    abstract member WritePrimitive<'a when 'a : unmanaged> : 'a -> Code<unit>
    abstract member WritePrimitiveArray<'a when 'a : unmanaged> : 'a[] -> Code<unit>
    abstract member WriteString : string -> Code<unit>
    abstract member WriteBool : bool -> Code<unit>

type ICoder =   
    inherit IReader
    inherit IWriter
    abstract member IsReading : bool



type IValueCoder =
    abstract member ValueType : Type
    abstract member ReadUnsafe : IReader -> Code<'a>
    abstract member WriteUnsafe : IWriter * 'a -> Code<unit>

type IValueCoder<'a> =
    inherit IValueCoder
    abstract member Read : IReader -> Code<'a>
    abstract member Write : IWriter * 'a -> Code<unit>


type IReferenceCoder =
    abstract member WithInternalStore : IValueCoder
    abstract member WithExternalStore : IValueCoder



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

    abstract member CodesType : bool
    default x.CodesType = false

    interface IValueCoder with
        member x.ValueType = typeof<'a>
        member x.WriteUnsafe(w : IWriter,v : 'b) = 
            if isNull (v :> obj) then x.Write(w, Unchecked.defaultof<'a>)
            else x.Write(w, unbox v)

        member x.ReadUnsafe(r : IReader) : Code<'b> = 
            x.Read(r) |> State.map (fun v ->
                if isNull (v :> obj) then 
                    Unchecked.defaultof<'b>
                else
                    unbox v
            )

    interface IValueCoder<'a> with
        member x.Write(w,v) = x.Write(w, v)
        member x.Read(r) = x.Read(r)


type private NewObj<'a>() =
    static member Create() : 'a =
        System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject(typeof<'a>) |> unbox<'a>



[<AbstractClass>]
type AbstractByRefValueCoder<'a>(useExternalStore : bool) =
    inherit AbstractValueCoder<'a>()
    let mutable useExternalStore = useExternalStore

    member private x.UseExternalStore 
        with get() = useExternalStore
        and set v = useExternalStore <- v

    abstract member WriteState : IWriter * 'a * byref<CodeState> -> unit
    abstract member ReadState : IReader * byref<'a> * byref<CodeState> -> unit
    abstract member WithStore : bool -> AbstractByRefValueCoder<'a>

    default x.WithStore ext =
        let res = base.MemberwiseClone() |> unbox<AbstractByRefValueCoder<'a>>
        res.UseExternalStore <- ext
        res |> unbox<_>

    interface IReferenceCoder with
        member x.WithInternalStore = 
            if useExternalStore then x.WithStore true :> IValueCoder
            else x :> IValueCoder

        member x.WithExternalStore = 
            if useExternalStore then x :> IValueCoder
            else x.WithStore false :> IValueCoder

    override x.Read(r) =
        { new Code<'a>() with
            override __.Run(s) = 
                let id = r.ReadPrimitive().Run(&s)

                if id = Guid.Empty then
                    Unchecked.defaultof<'a>
                else
                    let load = 
                        if useExternalStore then Code.tryLoad id
                        else Code.tryLoadLocal id

                    match load.Run(&s) with
                        | Some v -> v
                        | _ ->
                            let mutable self = NewObj<'a>.Create()
                            (Code.storeLocal id self).Run(&s)
                            s <- { s with Values = Map.add id (self :> obj) s.Values }
                            x.ReadState(r, &self, &s)
                            self
        }

    override x.Write(w,v) =
        { new Code<unit>() with
            override __.RunUnit(s) = 
                if isNull (v :> obj) then
                    w.WritePrimitive(Guid.Empty).Run(&s)
                else
                    let store =
                        if useExternalStore then Code.tryStore v
                        else Code.tryStoreLocal v
                
                    let (isNew, id) = store.Run(&s)
                    w.WritePrimitive(id).Run(&s)

                    if isNew then
                        x.WriteState(w, v, &s)
        }

    new() = AbstractByRefValueCoder(false)


[<AbstractClass>]
type CustomCoder<'a>() =
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



