namespace Aardvark.Base.IL

open System
open System.IO
open System.Threading
open System.Reflection
open System.Reflection.Emit
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open Aardvark.Base
open Aardvark.Base.FunctionReflection

type JumpCondition =
    | Less 
    | LessOrEqual
    | Greater
    | GreaterOrEqual
    | Equal
    | NotEqual
    | False
    | True

type Constant =
    | Int8 of int8
    | UInt8 of uint8
    | Int16 of int16
    | UInt16 of uint16
    | Int32 of int32
    | UInt32 of uint32
    | Int64 of int64
    | UInt64 of uint64
    | Float64 of float
    | Float32 of float32
    | NativeInt of nativeint
    | UNativeInt of unativeint
    | String of string

type ValueType =
    | Int8              = 0
    | UInt8             = 1
    | Int16             = 2
    | UInt16            = 3
    | Int32             = 4
    | UInt32            = 5            
    | Int64             = 6
    | UInt64            = 7
    | Float64           = 8
    | Float32           = 9
    | NativeInt         = 10
    | UNativeInt        = 11
    | Object            = 12

type Label private(id : int) =
    static let mutable currentId = 0
    
    member x.Id = id

    override x.ToString() =
        sprintf "Label(%d)" id

    override x.GetHashCode() = id.GetHashCode()
    override x.Equals o =
        match o with
            | :? Label as o -> id = o.Id
            | _ -> false

    interface IComparable with
        member x.CompareTo o =
            match o with
                | :? Label as o -> compare id o.Id
                | _ -> failwithf "cannot compare Label with %A" o


    new() = Label(Interlocked.Increment(&currentId))

type Local private(id : int, name : string, t : Type) =
    
    static let mutable currentId = 0

    member x.Id = id
    member x.Name = name
    member x.Type = t

    override x.ToString() =
        if String.IsNullOrWhiteSpace name then sprintf "local%d(%A)" id t
        else sprintf "%s(%A)" name t

    override x.GetHashCode() = id.GetHashCode()
    override x.Equals o =
        match o with
            | :? Local as o -> id = o.Id
            | _ -> false

    interface IComparable with
        member x.CompareTo o =
            match o with
                | :? Local as o -> compare id o.Id
                | _ -> failwithf "cannot compare Local with %A" o


    new(name, t) = Local(Interlocked.Increment(&currentId), name, t)
    new(t) = Local(Interlocked.Increment(&currentId), "", t)

type Instruction =
    | Nop
    | Break
        
    | Dup
    | Pop

    | Ldarg of int
    | LdargA of int
    | Starg of int
        
    | Ldloc of Local
    | LdlocA of Local
    | Stloc of Local

    | Ldfld of FieldInfo
    | LdfldA of FieldInfo
    | Stfld of FieldInfo


    | LdIndirect of ValueType
    | StIndirect of ValueType
    | CpObj of Type
    | LdObj of Type
    | StObj of Type
    | NewObj of ConstructorInfo
    | CastClass of Type
    | IsInstance of Type

    | Unbox of Type
    | UnboxAny of Type
    | Box of Type

    | NewArr of Type
    | Ldlen
    | Ldelem of Type
    | LdelemA of Type
    | Stelem of Type


    | Add | Sub | Mul | Div | Rem | And | Or | Xor | Shl | Shr 
    | Neg | Not

    | Conv of ValueType
    | ConvChecked of ValueType

    | RefAnyVal of Type
    | CkFinite
    | MkRefAny of Type

    | LdNull
    | LdConst of Constant
    | LdToken of obj
    
    | WriteLine of string
        
    | ReadOnly
    | Tail
    | Mark of Label
    | ConditionalJump of JumpCondition * Label
    | Jump of Label
    | Call of MethodInfo
    | CallIndirect
    | Ret
    | Switch of Label[]
    | Throw
    | EndFinally
    | Leave

type MethodDefinition =
    {
        ArgumentTypes : Type[]
        ReturnType : Type
        Body : list<Instruction>
    }

module StateMonad =
    type State<'s, 'a> = 's -> 's * 'a

    module State =
        let bind (f : 'a -> State<'s, 'b>) (m : State<'s, 'a>) : State<'s, 'b> =
            fun s ->
                let (s,v) = m s
                f v s

        let combine (l : State<'s, unit>) (r : State<'s, 'a>) : State<'s, 'a> =
            fun s ->
                let (s,()) = l s
                r s

        let map (f : 'a -> 'b) (m : State<'s, 'a>) : State<'s, 'b> =
            fun s ->
                let (s,v) = m s
                s, f v

        let create (v : 'a) : State<'s, 'a> = 
            fun s -> s,v

        let delay (f : unit -> State<'s, 'a>) : State<'s, 'a> =
            fun s ->
                f () s

        let get<'s> : State<'s, 's> = 
            fun s -> s,s

        let put (s : 's) : State<'s, unit> = 
            fun _ -> s, ()

        let modify (f : 's -> 's) : State<'s, unit> =
            fun s -> (f s), ()


    type StateBuilder() =
        member x.Bind(m : State<'s, 'a>, f : 'a -> State<'s, 'b>) =
            State.bind f m

        member x.Return (v : 'a) : State<'s, 'a> =
            State.create v

        member x.ReturnFrom (m : State<'s, 'a>) : State<'s, 'a> =
            m

        member x.Delay (f : unit -> State<'s, 'a>) : State<'s, 'a> =
            State.delay f
                
        member x.Zero() : State<'s, unit> =
            State.create ()

        member x.Combine(l : State<'s, unit>, r : State<'s, 'a>) =
            State.combine l r

        member x.For(elements : seq<'a>, f : 'a -> State<'s, unit>) : State<'s, unit> =
            fun s ->
                let mutable c = s
                for e in elements do
                    let (s,()) = f e c
                    c <- s
                c, ()

        member x.While(guard : unit -> bool, body : State<'s, unit>) : State<'s, unit> =
            fun s ->
                let mutable c = s
                while guard() do
                    let (s,()) = body c
                    c <- s
                c, ()

    let state = StateBuilder()


[<AutoOpen>]
module ReflectionExtensions = 
    type BindingFlags with
        static member All = BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance

module Disassembler = 

    type Parameter =
        | This of Type
        | Parameter of string * Type

    type RawInstruction =
        class
            val mutable public Code : OpCode
            val mutable public Operand : obj
        

            override x.ToString() =
                if isNull x.Operand then sprintf "%A" x.Code
                else sprintf "%A(%A)" x.Code x.Operand

            new(c) = { Code = c; Operand = null }
            new(c, o) = { Code = c; Operand = o }

        end

    [<AutoOpen>]
    module private Helpers = 
        module OpCodes =
            let table =
                typeof<OpCodes>.GetFields() 
                    |> Array.toList 
                    |> List.map (fun f -> f.GetValue(null) |> unbox<OpCode>)
                    |> List.map (fun c -> c.Value, c)
                    |> Map.ofList

            let get (value : int16) =
                table.[value]

        type ILContext =
            {
                TypeArgs : Type[]
                MethodArgs : Type[]
                Module : Module
            }

        type BinaryReader with

            member x.ReadOpCode() =
                let b0 = x.ReadByte()

                if b0 = 0xFEuy then
                    let b1 = x.ReadByte()
                    let v = ((int16 b0) <<< 8) ||| (int16 b1)
                    OpCodes.get v
                else
                    let v = int16 b0
                    OpCodes.get v

            member x.ReadOperand(ctx : ILContext, o : OperandType) =
                match o with
                    | OperandType.InlineNone ->
                        null

                    | OperandType.InlineBrTarget -> 
                        let off = x.ReadInt32()
                        int x.BaseStream.Position + off :> obj


                    | OperandType.InlineField ->
                        let token = x.ReadInt32()
                        let field = ctx.Module.ResolveField(token, ctx.TypeArgs, ctx.MethodArgs)
                        field :> obj

                    | OperandType.InlineI ->
                        x.ReadInt32() :> obj

                    | OperandType.InlineI8 ->
                        x.ReadInt64() :> obj

                    | OperandType.InlineMethod -> 
                        let token = x.ReadInt32()
                        let meth = ctx.Module.ResolveMethod(token, ctx.TypeArgs, ctx.MethodArgs)
                        meth :> obj

                    | OperandType.InlineR ->
                        x.ReadDouble() :> obj

                    | OperandType.InlineSig ->
                        let token = x.ReadInt32()
                        let signature = ctx.Module.ResolveSignature(token)
                        signature :> obj

                    | OperandType.InlineString ->
                        let token = x.ReadInt32()
                        let string = ctx.Module.ResolveString(token)
                        string :> obj

                    | OperandType.InlineTok ->
                        let token = x.ReadInt32()
                        let v = ctx.Module.ResolveMember(token, ctx.TypeArgs, ctx.MethodArgs)
                        v :> obj

                    | OperandType.InlineType ->
                        let token = x.ReadInt32()
                        let v = ctx.Module.ResolveType(token, ctx.TypeArgs, ctx.MethodArgs)
                        v :> obj

                    | OperandType.InlineVar ->
                        let id = x.ReadInt16()
                        int id :> obj
                
                    | OperandType.ShortInlineBrTarget ->
                        let off = x.ReadSByte() |> int
                        int x.BaseStream.Position + off :> obj
                

                    | OperandType.ShortInlineI ->
                        x.ReadSByte() :> obj

                    | OperandType.ShortInlineR ->
                        x.ReadSingle() :> obj

                    | OperandType.ShortInlineVar ->
                        let id = x.ReadByte() |> int
                        id |> int :> obj


                    | OperandType.InlineSwitch ->
                        let length = x.ReadInt32()
                        let baseOff = int x.BaseStream.Position
                        Array.init length (fun _ -> baseOff + x.ReadInt32()) :> obj

                    | _ -> failwithf "[IL] unsupported operand-type: %A" o

            member x.ReadRawInstruction(ctx : ILContext) =
                let code = x.ReadOpCode()
                let op = x.ReadOperand(ctx, code.OperandType)
                RawInstruction(code, op)


    module Patterns =
        
        let (|Nop|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Nop then Some()
            else None

        let (|Break|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Break then Some()
            else None

        let (|Dup|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Dup then Some()
            else None

        let (|Pop|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Pop then Some()
            else None

        let (|Ldarg|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldarg then Some(unbox<int> i.Operand)
            elif i.Code = OpCodes.Ldarg_0 then Some 0
            elif i.Code = OpCodes.Ldarg_1 then Some 1
            elif i.Code = OpCodes.Ldarg_2 then Some 2
            elif i.Code = OpCodes.Ldarg_3 then Some 3
            elif i.Code = OpCodes.Ldarg_S then Some(unbox<int> i.Operand)
            else None

        let (|Starg|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Starg_S then Some(unbox<int> i.Operand)
            elif i.Code = OpCodes.Starg then Some(unbox<int> i.Operand)
            else None

        let (|Ldarga|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldarga then Some(unbox<int> i.Operand)
            elif i.Code = OpCodes.Ldarga_S then Some(unbox<int> i.Operand)
            else None

        let (|Ldloc|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldloc then Some (unbox<int> i.Operand)
            elif i.Code = OpCodes.Ldloc_S then Some (unbox<int> i.Operand)
            elif i.Code = OpCodes.Ldloc_0 then Some 0
            elif i.Code = OpCodes.Ldloc_1 then Some 1
            elif i.Code = OpCodes.Ldloc_2 then Some 2
            elif i.Code = OpCodes.Ldloc_3 then Some 3
            else None

        let (|LdlocA|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldloca then Some (unbox<int> i.Operand)
            elif i.Code = OpCodes.Ldloca_S then Some (unbox<int> i.Operand)
            else None

        let (|Stloc|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Stloc then Some (unbox<int> i.Operand)
            elif i.Code = OpCodes.Stloc_S then Some (unbox<int> i.Operand)
            elif i.Code = OpCodes.Stloc_0 then Some 0
            elif i.Code = OpCodes.Stloc_1 then Some 1
            elif i.Code = OpCodes.Stloc_2 then Some 2
            elif i.Code = OpCodes.Stloc_3 then Some 3
            else None

        let (|Ldfld|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldfld then Some (unbox<FieldInfo> i.Operand)
            elif i.Code = OpCodes.Ldsfld then Some (unbox<FieldInfo> i.Operand)
            else None

        let (|LdfldA|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldflda then Some (unbox<FieldInfo> i.Operand)
            elif i.Code = OpCodes.Ldsflda then Some (unbox<FieldInfo> i.Operand)
            else None

        let (|Stfld|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Stfld then Some (unbox<FieldInfo> i.Operand)
            elif i.Code = OpCodes.Stsfld then Some (unbox<FieldInfo> i.Operand)
            else None

        let (|LdIndirect|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldind_I then Some(ValueType.NativeInt)
            elif i.Code = OpCodes.Ldind_I1 then Some(ValueType.Int8)
            elif i.Code = OpCodes.Ldind_U1 then Some(ValueType.UInt8)
            elif i.Code = OpCodes.Ldind_I2 then Some(ValueType.Int16)
            elif i.Code = OpCodes.Ldind_U2 then Some(ValueType.UInt16)
            elif i.Code = OpCodes.Ldind_I4 then Some(ValueType.Int32)
            elif i.Code = OpCodes.Ldind_U4 then Some(ValueType.UInt32)
            elif i.Code = OpCodes.Ldind_I8 then Some(ValueType.Int64)

            elif i.Code = OpCodes.Ldind_R4 then Some(ValueType.Float32)
            elif i.Code = OpCodes.Ldind_R8 then Some(ValueType.Float64)
            elif i.Code = OpCodes.Ldind_Ref then Some(ValueType.Object)
            else None

        let (|StIndirect|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Stind_I then Some(ValueType.NativeInt)
            elif i.Code = OpCodes.Stind_I1 then Some(ValueType.Int8)
            elif i.Code = OpCodes.Stind_I2 then Some(ValueType.Int16)
            elif i.Code = OpCodes.Stind_I4 then Some(ValueType.Int32)
            elif i.Code = OpCodes.Stind_I8 then Some(ValueType.Int64)
            elif i.Code = OpCodes.Stind_R4 then Some(ValueType.Float32)
            elif i.Code = OpCodes.Stind_R8 then Some(ValueType.Float64)
            elif i.Code = OpCodes.Stind_Ref then Some(ValueType.Object)
            else None

        let (|CpObj|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Cpobj then Some(unbox<Type> i.Operand)
            else None

        let (|LdObj|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldobj then Some(unbox<Type> i.Operand)
            else None

        let (|StObj|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Stobj then Some(unbox<Type> i.Operand)
            else None

        let (|NewObj|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Newobj then Some (unbox<ConstructorInfo> i.Operand)
            else None

        let (|CastClass|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Castclass then Some (unbox<Type> i.Operand)
            else None

        let (|IsInstance|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Isinst then Some (unbox<Type> i.Operand)
            else None

        let (|Unbox|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Unbox then Some (unbox<Type> i.Operand)
            else None

        let (|UnboxAny|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Unbox_Any then Some (unbox<Type> i.Operand)
            else None

        let (|Box|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Box then Some (unbox<Type> i.Operand)
            else None

        let (|NewArr|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Newarr then Some (unbox<Type> i.Operand)
            else None

        let (|Ldlen|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldlen then Some ()
            else None

        let (|Ldelem|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldelem then Some(unbox<Type> i.Operand)
            elif i.Code = OpCodes.Ldelem_I1 then Some typeof<int8>
            elif i.Code = OpCodes.Ldelem_U1 then Some typeof<uint8>
            elif i.Code = OpCodes.Ldelem_I2 then Some typeof<int16>
            elif i.Code = OpCodes.Ldelem_U2 then Some typeof<uint16>
            elif i.Code = OpCodes.Ldelem_I4 then Some typeof<int32>
            elif i.Code = OpCodes.Ldelem_U4 then Some typeof<uint32>
            elif i.Code = OpCodes.Ldelem_I8 then Some typeof<int64>
            elif i.Code = OpCodes.Ldelem_R4 then Some typeof<float32>
            elif i.Code = OpCodes.Ldelem_R8 then Some typeof<float>
            elif i.Code = OpCodes.Ldelem_I then Some typeof<nativeint>
            elif i.Code = OpCodes.Ldelem_Ref then Some typeof<obj>
            else None

        let (|LdelemA|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldelema then Some (unbox<Type> i.Operand)
            else None

        let (|Stelem|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Stelem then Some(unbox<Type> i.Operand)
            elif i.Code = OpCodes.Stelem_I1 then Some typeof<int8>
            elif i.Code = OpCodes.Stelem_I2 then Some typeof<int16>
            elif i.Code = OpCodes.Stelem_I4 then Some typeof<int32>
            elif i.Code = OpCodes.Stelem_I8 then Some typeof<int64>
            elif i.Code = OpCodes.Stelem_R4 then Some typeof<float32>
            elif i.Code = OpCodes.Stelem_R8 then Some typeof<float>
            elif i.Code = OpCodes.Stelem_I then Some typeof<nativeint>
            elif i.Code = OpCodes.Stelem_Ref then Some typeof<obj>
            else None

        
        let (|Add|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Add then Some ()
            elif i.Code = OpCodes.Add_Ovf then Some ()
            elif i.Code = OpCodes.Add_Ovf_Un then Some ()
            else None

        let (|Sub|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Sub then Some ()
            elif i.Code = OpCodes.Sub_Ovf then Some ()
            elif i.Code = OpCodes.Sub_Ovf_Un then Some ()
            else None

        let (|Mul|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Mul then Some ()
            elif i.Code = OpCodes.Mul_Ovf then Some ()
            elif i.Code = OpCodes.Mul_Ovf_Un then Some ()
            else None

        let (|Div|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Div then Some ()
            elif i.Code = OpCodes.Div_Un then Some ()
            else None

        let (|Rem|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Rem then Some ()
            elif i.Code = OpCodes.Rem_Un then Some ()
            else None

        let (|And|_|) (i : RawInstruction) =
            if i.Code = OpCodes.And then Some ()
            else None

        let (|Or|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Or then Some ()
            else None

        let (|Xor|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Xor then Some ()
            else None

        let (|Shl|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Shl then Some ()
            else None

        let (|Shr|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Shr then Some ()
            elif i.Code = OpCodes.Shr_Un then Some ()
            else None

        let (|Neg|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Neg then Some ()
            else None

        let (|Not|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Not then Some ()
            else None

        let (|Conv|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Conv_I1 then Some ValueType.Int8
            elif i.Code = OpCodes.Conv_U1 then Some ValueType.UInt8
            elif i.Code = OpCodes.Conv_I2 then Some ValueType.Int16
            elif i.Code = OpCodes.Conv_U2 then Some ValueType.UInt16
            elif i.Code = OpCodes.Conv_I4 then Some ValueType.Int32
            elif i.Code = OpCodes.Conv_U4 then Some ValueType.UInt32
            elif i.Code = OpCodes.Conv_I8 then Some ValueType.Int64
            elif i.Code = OpCodes.Conv_U8 then Some ValueType.UInt64
            elif i.Code = OpCodes.Conv_R4 then Some ValueType.Float32
            elif i.Code = OpCodes.Conv_R8 then Some ValueType.Float64
            elif i.Code = OpCodes.Conv_I then Some ValueType.NativeInt
            elif i.Code = OpCodes.Conv_U then Some ValueType.UNativeInt
            else None

        let (|ConvChecked|_|) (i : RawInstruction) =
            if i.Code =   OpCodes.Conv_Ovf_I1 then Some ValueType.Int8
            elif i.Code = OpCodes.Conv_Ovf_U1 then Some ValueType.UInt8
            elif i.Code = OpCodes.Conv_Ovf_I2 then Some ValueType.Int16
            elif i.Code = OpCodes.Conv_Ovf_U2 then Some ValueType.UInt16
            elif i.Code = OpCodes.Conv_Ovf_I4 then Some ValueType.Int32
            elif i.Code = OpCodes.Conv_Ovf_U4 then Some ValueType.UInt32
            elif i.Code = OpCodes.Conv_Ovf_I8 then Some ValueType.Int64
            elif i.Code = OpCodes.Conv_Ovf_U8 then Some ValueType.UInt64
            elif i.Code = OpCodes.Conv_Ovf_I then Some ValueType.NativeInt
            elif i.Code = OpCodes.Conv_Ovf_U then Some ValueType.UNativeInt
            else None

        let (|RefAnyVal|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Refanyval then Some (unbox<Type> i.Operand)
            else None

        let (|CkFinite|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ckfinite then Some ()
            else None

        let (|MkRefAny|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Mkrefany then Some (unbox<Type> i.Operand)
            else None

        let (|LdNull|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldnull then Some ()
            else None

        let (|LdConst|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldc_I4 then i.Operand |> unbox |> Int32 |> Some
            elif i.Code = OpCodes.Ldc_I4_S then i.Operand |> unbox<int8> |> int |> Int32 |> Some
            elif i.Code = OpCodes.Ldc_I8 then i.Operand |> unbox |> Int64 |> Some
            elif i.Code = OpCodes.Ldc_R4 then i.Operand |> unbox |> Float32 |> Some
            elif i.Code = OpCodes.Ldc_R8 then i.Operand |> unbox |> Float64 |> Some

            elif i.Code = OpCodes.Ldc_I4_0 then Some (Int32 0)
            elif i.Code = OpCodes.Ldc_I4_1 then Some (Int32 1)
            elif i.Code = OpCodes.Ldc_I4_2 then Some (Int32 2)
            elif i.Code = OpCodes.Ldc_I4_3 then Some (Int32 3)
            elif i.Code = OpCodes.Ldc_I4_4 then Some (Int32 4)
            elif i.Code = OpCodes.Ldc_I4_5 then Some (Int32 5)
            elif i.Code = OpCodes.Ldc_I4_6 then Some (Int32 6)
            elif i.Code = OpCodes.Ldc_I4_7 then Some (Int32 7)
            elif i.Code = OpCodes.Ldc_I4_8 then Some (Int32 8)
            elif i.Code = OpCodes.Ldc_I4_M1 then Some (Int32 -1)

            elif i.Code = OpCodes.Ldstr then i.Operand |> unbox<string> |> String |> Some

            else None

        let (|LdToken|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ldtoken then Some (i.Operand)
            else None

        let (|ReadOnly|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Readonly then Some ()
            else None

        let (|Tail|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Tailcall then Some ()
            else None

        let (|ConditionalJump|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Blt || i.Code = OpCodes.Blt_S || i.Code = OpCodes.Blt_Un || i.Code = OpCodes.Blt_Un_S then
                Some (Less, unbox<int> i.Operand)

            elif i.Code = OpCodes.Ble || i.Code = OpCodes.Ble_S || i.Code = OpCodes.Ble_Un || i.Code = OpCodes.Ble_Un_S then
                Some (LessOrEqual, unbox<int> i.Operand)

            elif i.Code = OpCodes.Bgt || i.Code = OpCodes.Bgt_S || i.Code = OpCodes.Bgt_Un || i.Code = OpCodes.Bgt_Un_S then
                Some (Greater, unbox<int> i.Operand)

            elif i.Code = OpCodes.Bge || i.Code = OpCodes.Bge_S || i.Code = OpCodes.Bge_Un || i.Code = OpCodes.Bge_Un_S then
                Some (GreaterOrEqual, unbox<int> i.Operand)

            elif i.Code = OpCodes.Beq || i.Code = OpCodes.Beq_S then
                Some (Equal, unbox<int> i.Operand)

            elif i.Code = OpCodes.Bne_Un || i.Code = OpCodes.Bne_Un_S then
                Some (NotEqual, unbox<int> i.Operand)

            elif i.Code = OpCodes.Brfalse || i.Code = OpCodes.Brfalse_S then
                Some (False, unbox<int> i.Operand)

            elif i.Code = OpCodes.Brtrue || i.Code = OpCodes.Brtrue_S then
                Some (True, unbox<int> i.Operand)
            else
                None

        let (|Jump|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Br || i.Code = OpCodes.Br_S then
                Some (unbox<int> i.Operand)
            else
                None

        let (|Call|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Call || i.Code = OpCodes.Callvirt then
                Some (unbox<MethodInfo> i.Operand)
            else
                None

        let (|CallIndirect|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Calli then Some ()
            else None

        let (|Ret|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Ret then Some ()
            else None

        let (|Switch|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Switch then
                Some (unbox<int[]> i.Operand)
            else None

        let (|Throw|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Throw then Some ()
            else None

        let (|EndFinally|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Endfinally then Some ()
            else None

        let (|Leave|_|) (i : RawInstruction) =
            if i.Code = OpCodes.Leave then Some ()
            else None


    let read (m : MethodBase) =
        let body = m.GetMethodBody()

        let locals =
            body.LocalVariables 
                |> Seq.map (fun l -> l.LocalIndex, Local(l.LocalType))
                |> Map.ofSeq

        let isInstance = not m.IsStatic

        let parameters =
            let off = if isInstance then 1 else 0
            m.GetParameters()
                |> Seq.map (fun p -> off + p.Position, Parameter(p.Name, p.ParameterType))
                |> Map.ofSeq

        let parameters =
            if isInstance then Map.add 0 (This m.DeclaringType) parameters
            else parameters

        let ctx =
            {
                TypeArgs = m.DeclaringType.GetGenericArguments()
                MethodArgs = m.GetGenericArguments()
                Module = m.Module
            }

        use binary = new BinaryReader(new MemoryStream(body.GetILAsByteArray()))

        let all = System.Collections.Generic.List<int * RawInstruction>()
        while binary.BaseStream.Position < binary.BaseStream.Length do
            let offset = binary.BaseStream.Position |> int
            let i = binary.ReadRawInstruction(ctx)
            all.Add (offset, i)


        parameters, locals, all.ToArray()

    let disassemble (m : MethodBase) =
        let parameter, locals, instructions = read m

        let currentId = ref 0

        let branchTargetIds =
            instructions 
                |> Seq.collect (fun (_,i) ->
                    if i.Code.OperandType = OperandType.InlineBrTarget || i.Code.OperandType = OperandType.ShortInlineBrTarget then 
                        [unbox<int> i.Operand] :> seq<_>
                    elif i.Code.OperandType = OperandType.InlineSwitch then
                        unbox<int[]> i.Operand :> seq<_>
                    else 
                        Seq.empty
                   )
                |> Set.ofSeq
                |> Seq.map (fun i -> i, Label())
                |> Map.ofSeq

        let body =
            seq {
                for (off, i) in instructions do
                    match Map.tryFind off branchTargetIds with
                        | Some l ->
                            yield Mark l
                        | _ ->
                            ()

                    match i with
                        | Patterns.Nop              -> yield Nop
                        | Patterns.Break            -> yield Break
                        | Patterns.Dup              -> yield Dup
                        | Patterns.Pop              -> yield Pop

                        | Patterns.Ldarg index      -> yield Ldarg index
                        | Patterns.Ldarga index     -> yield LdargA index
                        | Patterns.Starg index      -> yield Starg index

                        | Patterns.Ldloc index      -> yield Ldloc locals.[index]
                        | Patterns.LdlocA index     -> yield LdlocA locals.[index]
                        | Patterns.Stloc index      -> yield Stloc locals.[index]

                        | Patterns.Ldfld f          -> yield Ldfld f
                        | Patterns.LdfldA f         -> yield LdfldA f
                        | Patterns.Stfld f          -> yield Stfld f

                        | Patterns.LdIndirect t     -> yield LdIndirect t
                        | Patterns.StIndirect t     -> yield StIndirect t

                        | Patterns.CpObj t          -> yield CpObj t
                        | Patterns.LdObj t          -> yield LdObj t
                        | Patterns.StObj t          -> yield StObj t

                        | Patterns.NewObj c         -> yield NewObj c
                        | Patterns.CastClass t      -> yield CastClass t
                        | Patterns.IsInstance t     -> yield IsInstance t
                        
                        | Patterns.Unbox t          -> yield Unbox t
                        | Patterns.UnboxAny t       -> yield UnboxAny t
                        | Patterns.Box t            -> yield Box t

                        | Patterns.NewArr t         -> yield NewArr t
                        | Patterns.Ldlen            -> yield Ldlen
                        | Patterns.Ldelem t         -> yield Ldelem t
                        | Patterns.LdelemA t        -> yield LdelemA t
                        | Patterns.Stelem t         -> yield Stelem t

                        | Patterns.Add              -> yield Add
                        | Patterns.Sub              -> yield Sub
                        | Patterns.Mul              -> yield Mul
                        | Patterns.Div              -> yield Div
                        | Patterns.Rem              -> yield Rem
                        | Patterns.And              -> yield And
                        | Patterns.Or               -> yield Or
                        | Patterns.Xor              -> yield Xor
                        | Patterns.Shl              -> yield Shl
                        | Patterns.Shr              -> yield Shr

                        | Patterns.Neg              -> yield Neg
                        | Patterns.Not              -> yield Not

                        | Patterns.Conv t           -> yield Conv t
                        | Patterns.ConvChecked t    -> yield ConvChecked t

                        | Patterns.RefAnyVal t      -> yield RefAnyVal t
                        | Patterns.CkFinite         -> yield CkFinite
                        | Patterns.MkRefAny t       -> yield MkRefAny t
                        | Patterns.LdNull           -> yield LdNull

                        | Patterns.LdConst c        -> yield LdConst c
                        | Patterns.LdToken t        -> yield LdToken t

                        | Patterns.ReadOnly         -> yield ReadOnly
                        | Patterns.Tail             -> yield Tail

                        | Patterns.Jump target      -> yield Jump(branchTargetIds.[target])
            
                        | Patterns.Call mi          -> yield Call mi
                        | Patterns.CallIndirect     -> yield CallIndirect
                        | Patterns.Ret              -> yield Ret

                        | Patterns.Throw            -> yield Throw
                        | Patterns.EndFinally       -> yield EndFinally
                        | Patterns.Leave            -> yield Leave

                        | Patterns.Switch targets ->
                            yield Switch(targets |> Array.map (fun t -> branchTargetIds.[t]))

                        | Patterns.ConditionalJump(condition, target) ->
                            yield ConditionalJump(condition, branchTargetIds.[target])

                        | _ ->
                            failwithf "unknown instruction: %A" i
            }
            
        let body = Seq.toList body

        let returnType =
            match m with
                | :? MethodInfo as mi -> mi.ReturnType
                | :? ConstructorInfo -> typeof<System.Void>
                | _ -> failwithf "unknown method type: %A" m


        let arguments = m.GetParameters() |> Array.map (fun p -> p.ParameterType)
            
        let arguments = 
            if m.IsStatic then arguments
            else Array.append [|m.DeclaringType|] arguments

        {
            ArgumentTypes = arguments
            ReturnType = returnType
            Body = body
        }

module Assembler =
    open StateMonad

    [<AutoOpen>]
    module private Helpers =
        open Microsoft.FSharp.Reflection
        open StateMonad

        type AssemblerState =
            {
                generator   : ILGenerator
                labels      : Map<Label, System.Reflection.Emit.Label>
                locals      : Map<int, LocalBuilder>
                stack       : list<Type>
            }

        let (|ValueType|_|) (t : Type) =
            if t = typeof<obj> then Some ValueType.Object
            elif t = typeof<int8> then Some ValueType.Int8
            elif t = typeof<uint8> then Some ValueType.UInt8
            elif t = typeof<int16> then Some ValueType.Int16
            elif t = typeof<uint16> then Some ValueType.UInt16
            elif t = typeof<int32> then Some ValueType.Int32
            elif t = typeof<uint32> then Some ValueType.UInt32
            elif t = typeof<int64> then Some ValueType.Int64
            elif t = typeof<uint64> then Some ValueType.UInt64
            elif t = typeof<nativeint> then Some ValueType.NativeInt
            elif t = typeof<unativeint> then Some ValueType.UNativeInt
            elif t = typeof<float32> then Some ValueType.Float64
            elif t = typeof<float> then Some ValueType.Float64
            else None

        let (|Type|) (t : ValueType) =
            match t with
                | ValueType.Int8 -> typeof<int8>
                | ValueType.UInt8 -> typeof<uint8>
                | ValueType.Int16 -> typeof<int16>
                | ValueType.UInt16 -> typeof<uint16>
                | ValueType.Int32 -> typeof<int32>
                | ValueType.UInt32 -> typeof<uint32>
                | ValueType.Int64 -> typeof<int64>
                | ValueType.UInt64 -> typeof<uint64>
                | ValueType.NativeInt -> typeof<nativeint>
                | ValueType.UNativeInt -> typeof<unativeint>
                | ValueType.Float32 -> typeof<float32>
                | ValueType.Float64 -> typeof<float>
                | _ -> typeof<obj>

        type Asm<'a> = State<AssemblerState, 'a>

        type Asm() =

            static member Push (t : Type) : Asm<unit> =
                fun s -> { s with stack = t::s.stack }, ()

            static member Push (Type t) : Asm<unit> =
                fun s ->  { s with stack = t::s.stack }, ()


            static member Pop : Asm<Type> =
                fun s -> 
                    match s.stack with
                        | t::stack -> { s with stack = stack }, t
                        | _ -> s, typeof<obj>

            static member Peek : Asm<Type> =
                fun s -> 
                    match s.stack with
                        | t::_ -> s, t
                        | _ -> s, typeof<obj>


            static member PopN (n : int) : Asm<list<Type>> =
                fun s -> 
                    let rec pop (n : int) (s : list<Type>) =
                        if n <= 0 then [], s
                        else
                            match s with
                                | h::t ->
                                    let args, rest = pop (n - 1) t
                                    (h::args), rest
                                | _ ->  
                                    let args = List.init n (fun _ -> typeof<obj>)
                                    args, []
                
                    let res, stack = pop n s.stack
                    { s with stack = stack }, res

            static member fail fmt =
                failwithf fmt

            static member GetLocal (l : Local) : Asm<LocalBuilder> =
                fun s ->
                    match Map.tryFind l.Id s.locals with
                        | Some l -> s,l
                        | None ->
                            let res = s.generator.DeclareLocal(l.Type)
                            let s = { s with locals = Map.add l.Id res s.locals }
                            s, res

            static member GetLabel (l : Label) : Asm<System.Reflection.Emit.Label> =
                fun s ->
                    match Map.tryFind l s.labels with
                        | Some l -> s,l
                        | None ->
                            let res = s.generator.DefineLabel()
                            let s = { s with labels = Map.add l res s.labels }
                            s, res

            static member Mark(l : Label) =
                state {
                    let! l = Asm.GetLabel l
                    do! (fun s -> s, s.generator.MarkLabel(l))
                }
            static member Emit(c : OpCode) : Asm<unit> =
                fun s -> s, s.generator.Emit(c)

            static member WriteLine (str : string) : Asm<unit> =
                fun s -> s, s.generator.EmitWriteLine str

            static member Emit(c : OpCode, arg : obj) : Asm<unit> =
                fun s -> 
                    match arg with
                        | null                      -> s.generator.Emit(c)
                        | :? uint8 as o             -> s.generator.Emit(c, o)
                        | :? float as o             -> s.generator.Emit(c, o)
                        | :? int16 as o             -> s.generator.Emit(c, o)
                        | :? int as o               -> s.generator.Emit(c, o)
                        | :? int64 as o             -> s.generator.Emit(c, o)
                        | :? ConstructorInfo as o   -> s.generator.Emit(c, o)
                        | :? FieldInfo as o         -> s.generator.Emit(c, o)
                        | :? MethodInfo as o        -> s.generator.Emit(c, o)
                        | :? int8 as o              -> s.generator.Emit(c, o)
                        | :? float32 as o           -> s.generator.Emit(c, o)
                        | :? string as o            -> s.generator.Emit(c, o)
                        | :? Type as o              -> s.generator.Emit(c, o)
                        | :? SignatureHelper as o   -> s.generator.Emit(c, o)
                        | :? Emit.Label as o        -> s.generator.Emit(c, o)
                        | :? array<Emit.Label> as o -> s.generator.Emit(c, o)
                        | :? LocalBuilder as o      -> s.generator.Emit(c, o)
                        | operand ->
                            failwithf "[IL] unsupported operand: %A" operand
            
                    s, ()

    let private assembleInstruction (i : Instruction) =
        state {
            match i with
                | Nop   -> do! Asm.Emit(OpCodes.Nop)
                | Break -> do! Asm.Emit(OpCodes.Break)
                | Dup   -> 
                    let! t = Asm.Peek
                    do! Asm.Push t
                    do! Asm.Emit(OpCodes.Dup)
                | Pop   -> 
                    let! _ = Asm.Pop
                    do! Asm.Emit(OpCodes.Pop)

                | WriteLine str ->
                    do! Asm.WriteLine str

                | Ldarg a ->
                    match a with
                        | 0 -> do! Asm.Emit(OpCodes.Ldarg_0)
                        | 1 -> do! Asm.Emit(OpCodes.Ldarg_1)
                        | 2 -> do! Asm.Emit(OpCodes.Ldarg_2)
                        | 3 -> do! Asm.Emit(OpCodes.Ldarg_3)
                        | p when p < 256 -> do! Asm.Emit(OpCodes.Ldarg_S, uint8 p :> obj)
                        | p -> do! Asm.Emit(OpCodes.Ldarg, p :> obj)

                    //do! Asm.Push a.ArgumentType

                | LdargA a ->
                    if a > 255 then do! Asm.Emit(OpCodes.Ldarga, a :> obj)
                    else do! Asm.Emit(OpCodes.Ldarga_S, uint8 a)

                    //do! Asm.Push (a.ArgumentType.MakeByRefType())

                | Starg a ->
                    let! t = Asm.Pop

                    if a > 255 then do! Asm.Emit(OpCodes.Starg, a :> obj)
                    else do! Asm.Emit(OpCodes.Starg_S, uint8 a)


                | Ldloc l ->
                    let! l = Asm.GetLocal l
                    do! Asm.Emit(OpCodes.Ldloc, l)
                    do! Asm.Push l.LocalType

                | LdlocA l ->
                    let! l = Asm.GetLocal l
                    do! Asm.Emit(OpCodes.Ldloca, l)
                    do! Asm.Push (l.LocalType.MakeByRefType())

                | Stloc l ->
                    let! l = Asm.GetLocal l
                    let! t = Asm.Pop
                    do! Asm.Emit(OpCodes.Stloc, l)

                | Ldfld f -> 
                    if f.IsStatic then do! Asm.Emit(OpCodes.Ldsfld, f)
                    else do! Asm.Emit(OpCodes.Ldfld, f)
                    do! Asm.Push f.FieldType

                | LdfldA f ->
                    if f.IsStatic then do! Asm.Emit(OpCodes.Ldsflda, f)
                    else do! Asm.Emit(OpCodes.Ldflda, f)
                    do! Asm.Push (f.FieldType.MakeByRefType())

                | Stfld f ->
                    let! t = Asm.Pop
                    if f.IsStatic then do! Asm.Emit(OpCodes.Stsfld, f)
                    else do! Asm.Emit(OpCodes.Stfld, f)

                | LdIndirect t ->
                    match t with
                        | ValueType.Int8 -> do! Asm.Emit(OpCodes.Ldind_I1)
                        | ValueType.UInt8 -> do! Asm.Emit(OpCodes.Ldind_U1)
                        | ValueType.Int16 -> do! Asm.Emit(OpCodes.Ldind_I2)
                        | ValueType.UInt16 -> do! Asm.Emit(OpCodes.Ldind_U2)
                        | ValueType.Int32 -> do! Asm.Emit(OpCodes.Ldind_I4)
                        | ValueType.UInt32 -> do! Asm.Emit(OpCodes.Ldind_U4)
                        | ValueType.Int64 -> do! Asm.Emit(OpCodes.Ldind_I8)
                        | ValueType.UInt64 -> do! Asm.fail "cannot LdIndirect uint64"
                        | ValueType.NativeInt -> do! Asm.Emit(OpCodes.Ldind_I)
                        | ValueType.UNativeInt -> do! Asm.fail "cannot LdIndirect unativeint"
                        | ValueType.Float32 -> do! Asm.Emit(OpCodes.Ldind_R4)
                        | ValueType.Float64 -> do! Asm.Emit(OpCodes.Ldind_R8)
                        | _ -> do! Asm.Emit(OpCodes.Ldind_Ref)

                    do! Asm.Push t

                | StIndirect t ->
                    let! vt = Asm.Pop

                    match t with
                        | ValueType.Int8 -> do! Asm.Emit(OpCodes.Stind_I1)
                        | ValueType.UInt8 -> do! Asm.Emit(OpCodes.Stind_I1)
                        | ValueType.Int16 -> do! Asm.Emit(OpCodes.Stind_I2)
                        | ValueType.UInt16 -> do! Asm.Emit(OpCodes.Stind_I2)
                        | ValueType.Int32 -> do! Asm.Emit(OpCodes.Stind_I4)
                        | ValueType.UInt32 -> do! Asm.Emit(OpCodes.Stind_I4)
                        | ValueType.Int64 -> do! Asm.Emit(OpCodes.Stind_I8)
                        | ValueType.UInt64 -> do! Asm.Emit(OpCodes.Stind_I8)
                        | ValueType.NativeInt -> do! Asm.Emit(OpCodes.Stind_I)
                        | ValueType.UNativeInt -> do! Asm.Emit(OpCodes.Stind_I)
                        | ValueType.Float32 -> do! Asm.Emit(OpCodes.Stind_R4)
                        | ValueType.Float64 -> do! Asm.Emit(OpCodes.Stind_R8)
                        | _ -> do! Asm.Emit(OpCodes.Stind_Ref)

                | CpObj t ->        
                    do! Asm.Emit(OpCodes.Cpobj, t) 
                    do! Asm.Push t
                | LdObj t ->        
                    do! Asm.Emit(OpCodes.Ldobj, t)
                    do! Asm.Push t
                | StObj t ->        
                    let! rt = Asm.Pop
                    do! Asm.Emit(OpCodes.Stobj, t)

                | NewObj c ->       
                    let! args = Asm.PopN (c.GetParameters().Length)
                    do! Asm.Emit(OpCodes.Newobj, c)
                    do! Asm.Push c.DeclaringType

                | CastClass t ->    
                    let! tr = Asm.Pop
                    do! Asm.Emit(OpCodes.Castclass, t)
                    do! Asm.Push t

                | IsInstance t ->   
                    let! tr = Asm.Pop
                    do! Asm.Emit(OpCodes.Isinst, t)
                    do! Asm.Push t

                | Unbox t ->        
                    let! ot = Asm.Pop
                    do! Asm.Emit(OpCodes.Unbox, t)
                    do! Asm.Push t

                | UnboxAny t ->     
                    let! _ = Asm.Pop
                    do! Asm.Emit(OpCodes.Unbox_Any, t)
                    do! Asm.Push t

                | Box t ->          
                    let! tr = Asm.Pop
                    do! Asm.Emit(OpCodes.Box, t)
                    do! Asm.Push typeof<obj>

                | NewArr t ->       
                    let! tl = Asm.Pop
                    do! Asm.Emit(OpCodes.Newarr, t)
                    do! Asm.Push (t.MakeArrayType())

                | Ldlen ->          
                    let! tarr = Asm.Pop
                    do! Asm.Emit(OpCodes.Ldlen)
                    do! Asm.Push typeof<int>

                // Stack maintained up to this point


                | Ldelem t ->
                    match t with
                        | ValueType vt ->
                            match vt with
                                | ValueType.Int8 -> do! Asm.Emit(OpCodes.Ldelem_I1)
                                | ValueType.UInt8 -> do! Asm.Emit(OpCodes.Ldelem_U1)
                                | ValueType.Int16 -> do! Asm.Emit(OpCodes.Ldelem_I2)
                                | ValueType.UInt16 -> do! Asm.Emit(OpCodes.Ldelem_U2)
                                | ValueType.Int32 -> do! Asm.Emit(OpCodes.Ldelem_I4)
                                | ValueType.UInt32 -> do! Asm.Emit(OpCodes.Ldelem_U4)
                                | ValueType.Int64 -> do! Asm.Emit(OpCodes.Ldelem_I8)
                                | ValueType.UInt64 -> do! Asm.fail "cannot load uint64 array element"
                                | ValueType.NativeInt -> do! Asm.Emit(OpCodes.Ldelem_I)
                                | ValueType.UNativeInt -> do! Asm.fail "cannot load unativeint array element"
                                | ValueType.Float32 -> do! Asm.Emit(OpCodes.Ldelem_R4)
                                | ValueType.Float64 -> do! Asm.Emit(OpCodes.Ldelem_R8)
                                | _ -> do! Asm.Emit(OpCodes.Ldelem_Ref)

                        | t -> 
                            do! Asm.Emit(OpCodes.Ldelem, t)

                | LdelemA t ->
                    do! Asm.Emit(OpCodes.Ldelema, t)

                | Stelem t ->
                    match t with
                        | ValueType vt ->
                            match vt with
                                | ValueType.Int8 -> do! Asm.Emit(OpCodes.Stelem_I1)
                                | ValueType.UInt8 -> do! Asm.Emit(OpCodes.Stelem_I1)
                                | ValueType.Int16 -> do! Asm.Emit(OpCodes.Stelem_I2)
                                | ValueType.UInt16 -> do! Asm.Emit(OpCodes.Stelem_I2)
                                | ValueType.Int32 -> do! Asm.Emit(OpCodes.Stelem_I4)
                                | ValueType.UInt32 -> do! Asm.Emit(OpCodes.Stelem_I4)
                                | ValueType.Int64 -> do! Asm.Emit(OpCodes.Stelem_I8)
                                | ValueType.UInt64 -> do! Asm.Emit(OpCodes.Stelem_I8)
                                | ValueType.NativeInt -> do! Asm.Emit(OpCodes.Stelem_I)
                                | ValueType.UNativeInt -> do! Asm.Emit(OpCodes.Stelem_I)
                                | ValueType.Float32 -> do! Asm.Emit(OpCodes.Stelem_R4)
                                | ValueType.Float64 -> do! Asm.Emit(OpCodes.Stelem_R8)
                                | _ -> do! Asm.Emit(OpCodes.Stelem_Ref)

                        | t -> 
                            do! Asm.Emit(OpCodes.Stelem, t)


                | Add -> do! Asm.Emit(OpCodes.Add)
                | Sub -> do! Asm.Emit(OpCodes.Sub)
                | Mul -> do! Asm.Emit(OpCodes.Mul)
                | Div -> do! Asm.Emit(OpCodes.Div)
                | Rem -> do! Asm.Emit(OpCodes.Rem)
                | And -> do! Asm.Emit(OpCodes.And)
                | Or ->  do! Asm.Emit(OpCodes.Or)
                | Xor -> do! Asm.Emit(OpCodes.Xor)
                | Shl -> do! Asm.Emit(OpCodes.Shl)
                | Shr -> do! Asm.Emit(OpCodes.Shr)
                | Neg -> do! Asm.Emit(OpCodes.Neg)
                | Not -> do! Asm.Emit(OpCodes.Not)


                | Conv t ->
                    match t with
                        | ValueType.Int8 -> do! Asm.Emit(OpCodes.Conv_Ovf_I1)
                        | ValueType.UInt8 -> do! Asm.Emit(OpCodes.Conv_Ovf_U1)
                        | ValueType.Int16 -> do! Asm.Emit(OpCodes.Conv_Ovf_I2)
                        | ValueType.UInt16 -> do! Asm.Emit(OpCodes.Conv_Ovf_U2)
                        | ValueType.Int32 -> do! Asm.Emit(OpCodes.Conv_Ovf_I4)
                        | ValueType.UInt32 -> do! Asm.Emit(OpCodes.Conv_Ovf_U4)
                        | ValueType.Int64 -> do! Asm.Emit(OpCodes.Conv_Ovf_I8)
                        | ValueType.UInt64 -> do! Asm.Emit(OpCodes.Conv_Ovf_U8)
                        | ValueType.NativeInt -> do! Asm.Emit(OpCodes.Conv_Ovf_I)
                        | ValueType.UNativeInt -> do! Asm.Emit(OpCodes.Conv_Ovf_U)
                        | ValueType.Float32 -> do! Asm.Emit(OpCodes.Conv_R4)
                        | ValueType.Float64 -> do! Asm.Emit(OpCodes.Conv_R8)
                        | _ -> do! Asm.fail "cannot convert to obj"

                | ConvChecked t ->
                    match t with
                        | ValueType.Int8 -> do! Asm.Emit(OpCodes.Conv_Ovf_I1)
                        | ValueType.UInt8 -> do! Asm.Emit(OpCodes.Conv_Ovf_U1)
                        | ValueType.Int16 -> do! Asm.Emit(OpCodes.Conv_Ovf_I2)
                        | ValueType.UInt16 -> do! Asm.Emit(OpCodes.Conv_Ovf_U2)
                        | ValueType.Int32 -> do! Asm.Emit(OpCodes.Conv_Ovf_I4)
                        | ValueType.UInt32 -> do! Asm.Emit(OpCodes.Conv_Ovf_U4)
                        | ValueType.Int64 -> do! Asm.Emit(OpCodes.Conv_Ovf_I8)
                        | ValueType.UInt64 -> do! Asm.Emit(OpCodes.Conv_Ovf_U8)
                        | ValueType.NativeInt -> do! Asm.Emit(OpCodes.Conv_Ovf_I)
                        | ValueType.UNativeInt -> do! Asm.Emit(OpCodes.Conv_Ovf_U)
                        | ValueType.Float32 -> do! Asm.Emit(OpCodes.Conv_R4)
                        | ValueType.Float64 -> do! Asm.Emit(OpCodes.Conv_R8)
                        | _ -> do! Asm.fail "cannot convert to obj"

                | LdNull -> do! Asm.Emit(OpCodes.Ldnull)

                | LdConst c ->
                    match c with
                        | Int8 i -> 
                            do! Asm.Emit(OpCodes.Ldc_I4, int i)
                            do! Asm.Emit(OpCodes.Conv_I1)

                        | UInt8 i ->
                            do! Asm.Emit(OpCodes.Ldc_I4, int i)
                            do! Asm.Emit(OpCodes.Conv_U1)

                        
                        | Int16 i -> 
                            do! Asm.Emit(OpCodes.Ldc_I4, int i)
                            do! Asm.Emit(OpCodes.Conv_I2)

                        | UInt16 i ->
                            do! Asm.Emit(OpCodes.Ldc_I4, int i)
                            do! Asm.Emit(OpCodes.Conv_U2)
                        
                        | Int32 i -> do! Asm.Emit(OpCodes.Ldc_I4, int i)
                        | UInt32 i -> do! Asm.Emit(OpCodes.Ldc_I4, int i)
                        | Int64 i -> do! Asm.Emit(OpCodes.Ldc_I8, i)
                        | UInt64 i -> do! Asm.Emit(OpCodes.Ldc_I8, i)
                        | Float32 f -> do! Asm.Emit(OpCodes.Ldc_R4, f)
                        | Float64 f -> do! Asm.Emit(OpCodes.Ldc_R8, f)

                        | NativeInt i -> 
                            do! Asm.Emit(OpCodes.Ldc_I8, int64 i)
                            do! Asm.Emit(OpCodes.Conv_I)
                        | UNativeInt i -> 
                            do! Asm.Emit(OpCodes.Ldc_I8, int64 i)
                            do! Asm.Emit(OpCodes.Conv_I)

                        | String str ->
                            do! Asm.Emit(OpCodes.Ldstr, str)

                | LdToken t ->
                    do! Asm.Emit(OpCodes.Ldtoken, t)

                | ReadOnly -> do! Asm.Emit(OpCodes.Readonly)
                | Tail -> do! Asm.Emit(OpCodes.Tailcall)

                | Mark l -> do! Asm.Mark l
                | ConditionalJump(c, l) ->
                    let! l = Asm.GetLabel l

                    match c with
                        | Less -> do! Asm.Emit(OpCodes.Blt, l)
                        | LessOrEqual -> do! Asm.Emit(OpCodes.Ble, l)
                        | Greater -> do! Asm.Emit(OpCodes.Bgt, l)
                        | GreaterOrEqual -> do! Asm.Emit(OpCodes.Bge, l)
                        | Equal -> do! Asm.Emit(OpCodes.Beq, l)
                        | NotEqual -> do! Asm.Emit(OpCodes.Bne_Un, l)
                        | True -> do! Asm.Emit(OpCodes.Brtrue, l)
                        | False -> do! Asm.Emit(OpCodes.Brfalse, l)

                | Jump l ->
                    let! l = Asm.GetLabel l
                    do! Asm.Emit(OpCodes.Br, l)

                | Call mi ->
                    if mi.IsVirtual then do! Asm.Emit(OpCodes.Callvirt, mi)
                    else do! Asm.Emit(OpCodes.Call, mi)

                | CallIndirect ->
                    do! Asm.Emit(OpCodes.Calli)

                | Ret ->
                    do! Asm.Emit(OpCodes.Ret)

                | Switch labels ->
                    let arr = Array.zeroCreate labels.Length
                    for i in 0..labels.Length-1 do
                        let! l = Asm.GetLabel(labels.[i])
                        arr.[i] <- l

                    do! Asm.Emit(OpCodes.Switch, arr)

                | Throw -> do! Asm.Emit(OpCodes.Throw)
                | EndFinally -> do! Asm.Emit(OpCodes.Endfinally)
                | Leave -> do! Asm.Emit(OpCodes.Leave)
                | CkFinite -> do! Asm.Emit(OpCodes.Ckfinite)
                | MkRefAny t -> do! Asm.Emit(OpCodes.Mkrefany, t)
                | RefAnyVal t -> do! Asm.Emit(OpCodes.Refanyval, t)

     
        }

    let private assembleBody (m : seq<Instruction>) =
        state {
            for i in m do 
                do! assembleInstruction i
        }

    let assembleDelegate (m : MethodDefinition) : Delegate =
        let meth = 
            DynamicMethod(
                Guid.NewGuid() |> string,
                m.ReturnType, m.ArgumentTypes,
                m.ArgumentTypes.[0],
                true
            )

        let state =
            {
                generator   = meth.GetILGenerator()
                labels      = Map.empty
                locals      = Map.empty
                stack       = []
            }

        let endState,_ = assembleBody m.Body state


        if m.ReturnType = typeof<System.Void> then
            let funcType = typedefof<Action<_>>.FullName.Replace("1", string (m.ArgumentTypes.Length)) |> Type.GetType
            let funcType = funcType.MakeGenericType m.ArgumentTypes

            meth.CreateDelegate(funcType)
        else
            let funcType = typedefof<Func<_>>.FullName.Replace("1", string (m.ArgumentTypes.Length + 1)) |> Type.GetType
            let funcType = funcType.MakeGenericType (Array.append m.ArgumentTypes [|m.ReturnType|])

            meth.CreateDelegate(funcType)

   
    let assembleDefinition (m : MethodDefinition) : 'a =
        let d = assembleDelegate m
        DelegateAdapters.wrap d

    let assemble (body : seq<Instruction>) : 'a =
        let args, ret = DelegateAdapters.getFunctionSignature typeof<'a>

        assembleDefinition {
            ArgumentTypes = List.toArray args
            ReturnType = ret
            Body = Seq.toList body
        }

[<AutoOpen>]
module Frontend =
    open StateMonad

    type ConcList<'a> =
        private
        | Empty
        | List of list<'a>
        | Single of 'a
        | Concat of list<ConcList<'a>>

    module ConcList =
        let empty = Empty
        let single v = Single v
        
        let ofList l = 
            match l with
                | [] -> Empty
                | l -> List l

        let ofSeq (s : #seq<'a>) : ConcList<'a> = 
            match s :> seq<'a> with
                | :? list<'a> as l -> List l
                | _ -> s |> Seq.toList |> List

        let ofArray a = a |> Array.toList |> List

        let rec toSeq (c : ConcList<'a>) =
            let res = System.Collections.Generic.List<'a>()

            let rec recurse (c : ConcList<'a>) =
                match c with
                    | Empty -> ()
                    | Single v -> res.Add v
                    | List l -> res.AddRange l
                    | Concat ls ->
                        for l in ls do recurse l

            recurse c
            res :> seq<_>

        let rec toList (c : ConcList<'a>) =
            let l = toSeq c |> unbox<System.Collections.Generic.List<'a>>
            l |> CSharpList.toList

        let rec toArray (c : ConcList<'a>) =
            let l = toSeq c |> unbox<System.Collections.Generic.List<'a>>
            l.ToArray()

        let append (l : ConcList<'a>) (r : ConcList<'a>) =
            match l, r with
                | Empty, r -> r
                | l, Empty -> l
                | l, r -> Concat [l;r]

        let concat (l : #seq<ConcList<'a>>) =
            let l = l |> Seq.toList
            Concat l

        let cons (a : 'a) (l : ConcList<'a>) =
            Concat [Single a; l]

        let snoc (l : ConcList<'a>) (a : 'a) =
            Concat [l; Single a]

    type CodeGenState = 
        {
            instructions : ConcList<Instruction>
        }

    type CodeGen<'a> = State<CodeGenState, 'a>


    type CodeGen =
        
        static member run(c : CodeGen<unit>) =
            let (s,()) = c { instructions = ConcList.empty }
            s.instructions |> ConcList.toList

        static member emit (i : ConcList<Instruction>) : CodeGen<unit> =
            fun s -> { s with instructions = ConcList.append s.instructions i }, ()

        static member emit (i : Instruction) : CodeGen<unit> =
            fun s -> { s with instructions = ConcList.snoc s.instructions i }, ()

        static member emit (i : #seq<Instruction>) : CodeGen<unit> =
            fun s -> { s with instructions = ConcList.append s.instructions (ConcList.ofSeq i) }, ()
           

    type CodeGenBuilder() =
        
        member x.Bind(i : Instruction, f : unit -> CodeGen<'b>) = 
            state.Bind(CodeGen.emit (ConcList.single i), f)

        member x.Bind(i : #seq<Instruction>, f : unit -> CodeGen<'b>) = 
            state.Bind(CodeGen.emit (ConcList.ofSeq i), f)


        member x.Bind(m : CodeGen<'a>, f : 'a -> CodeGen<'b>) = state.Bind(m,f)
        member x.Return (v : 'a) : CodeGen<'a> = state.Return v
        member x.ReturnFrom (c : CodeGen<'a>) = state.ReturnFrom c
        member x.Zero() : CodeGen<unit> = state.Zero()
        member x.Delay(f : unit -> CodeGen<'a>) = state.Delay f
        member x.Combine(l : CodeGen<unit>, r : CodeGen<'a>) = state.Combine(l,r)
        member x.For(elements : seq<'a>, f : 'a -> CodeGen<unit>) = state.For(elements, f)
        member x.While(guard, body : CodeGen<unit>) = state.While(guard, body)

    type ExecutableBuilder() =
        inherit CodeGenBuilder()

        member x.Run(c : CodeGen<unit>) =
            let code = c |> CodeGen.run
            Assembler.assemble code

    let codegen = CodeGenBuilder()

    let inlineil = ExecutableBuilder()

    [<AutoOpen>]
    module private Helpers =

        let rec tryGetMethodInfo (e : Expr) =
            match e with
                | Patterns.Call(_,mi,_) -> Some mi
                | ExprShape.ShapeCombination(_, args) -> 
                    args |> List.tryPick tryGetMethodInfo
                | ExprShape.ShapeLambda(_,b) ->
                    tryGetMethodInfo b
                | _ -> None

        let getMethodInfo (e : Expr) =
            match tryGetMethodInfo e with
                | Some mi -> mi
                | None -> failwithf "[IL] could not get MethodInfo from: %A" e
   
   
        let rec tryGetFieldInfo (e : Expr) =
            match e with
                | FieldGet(_,fi) -> Some fi
                | ExprShape.ShapeCombination(_, args) -> 
                    args |> List.tryPick tryGetFieldInfo
                | ExprShape.ShapeLambda(_,b) ->
                    tryGetFieldInfo b
                | _ -> None

        let getFieldInfo (e : Expr) =
            match tryGetFieldInfo e with
                | Some fi -> fi
                | None -> failwithf "[IL] could not get FieldInfo from: %A" e
  
   
        let rec tryGetConstructorInfo (e : Expr) =
            match e with    
                | NewObject(ctor,_) -> Some ctor
                | ExprShape.ShapeCombination(_, args) -> 
                    args |> List.tryPick tryGetConstructorInfo
                | ExprShape.ShapeLambda(_,b) ->
                    tryGetConstructorInfo b
                | _ -> None

        let getConstructorInfo (e : Expr) =
            match tryGetConstructorInfo e with
                | Some fi -> fi
                | None -> failwithf "[IL] could not get ConstructorInfo from: %A" e
  
   
    type IL private() =
        static let stringFormat = typeof<String>.GetMethod("Format", [| typeof<string>; typeof<obj[]> |])
        static let printString = typeof<Report>.GetMethod("Line", [| typeof<string>; typeof<obj[]> |])

        /// nop
        static member nop = Nop |> CodeGen.emit

        // break
        static member break_ = Break |> CodeGen.emit

        /// duplicates the top-most value on the stack
        static member dup = Dup |> CodeGen.emit

        /// pops the top-most value from the stack
        static member pop = Pop |> CodeGen.emit


        /// loads the argument with the given index onto the stack
        static member ldarg (i : int) = i |> Ldarg |> CodeGen.emit

        /// loads the address of the argument with the given index onto the stack
        static member ldarga (i : int) = i |> LdargA |> CodeGen.emit

        /// stores the top-most value in the argument with the given index
        static member starg (i : int) = i |> Starg |> CodeGen.emit


        /// loads the local variable onto the stack
        static member ldloc (l : Local) = l |> Ldloc |> CodeGen.emit

        /// load the address of the local variable onto the stack
        static member ldloca (l : Local) = l |> LdlocA |> CodeGen.emit

        /// stores the top-most value in the local variable
        static member stloc (l : Local) = l |> Stloc |> CodeGen.emit


        /// loads the value of the field onto the stack
        static member ldfld (f : FieldInfo) = f |> Ldfld |> CodeGen.emit

        /// loads the address of the field onto the stack
        static member ldflda (f : FieldInfo) = f |> LdfldA |> CodeGen.emit

        /// stores the top-most value in the field
        static member stfld (f : FieldInfo) = f |> Stfld |> CodeGen.emit

        /// loads the value of the field onto the stack
        static member ldfld (e : Expr) = e |> getFieldInfo |> IL.ldfld

        /// loads the address of the field onto the stack
        static member ldflda (e : Expr) = e |> getFieldInfo |> IL.ldflda

        /// stores the top-most value in the field
        static member stfld (e : Expr) = e |> getFieldInfo |> IL.stfld

        /// stores the top-most value in a ref on the stack
        static member stind (t : ValueType) = t |> StIndirect |> CodeGen.emit

        /// loads the value from a ref on the stack
        static member ldind (t : ValueType) = t |> LdIndirect |> CodeGen.emit

        /// TODO
        static member cpobj (t : Type) = t |> CpObj |> CodeGen.emit

        /// loads the object (with the given type) from the pointer currently on the stack
        static member ldobj (t : Type) = t |> LdObj |> CodeGen.emit

        /// stores the top-most value (with the given type) in a ptr on the stack
        static member stobj (t : Type) = t |> LdObj |> CodeGen.emit

        /// creates a new object by calling the given Constructor
        static member newobj (ctor : ConstructorInfo) = ctor |> NewObj |> CodeGen.emit

        /// creates a new object by calling the given Constructor
        static member newobj (e : Expr) = e |> getConstructorInfo |> IL.newobj

        /// casts the top-most value to the given class
        static member castclass (t : Type) = t |> CastClass |> CodeGen.emit

        /// checks if the top-most value is an instance of the given type
        static member isinst (t : Type) = t |> IsInstance |> CodeGen.emit


        /// unboxes the top-most value (with the given type) to its unboxed representation
        static member unbox (t : Type) = t |> Unbox |> CodeGen.emit

        /// unboxes the top-most value to the given type
        static member unboxAny (t : Type) = t |> UnboxAny |> CodeGen.emit

        /// boxes the top-most value (with the given type) to an object
        static member box (t : Type) = t |> Box |> CodeGen.emit


        /// creates a new array using the given element-type and the length on the stack
        static member newarr (t : Type) = t |> NewArr |> CodeGen.emit

        /// loads the length of the array currently on the stack
        static member ldlen = Ldlen |> CodeGen.emit

        /// loads an element (with the given type) from the array on the stack
        static member ldelem (t : Type) = t |> Ldelem |> CodeGen.emit

        /// stores the element (with the given type) to the array on the stack
        static member stelem (t : Type) = t |> Ldelem |> CodeGen.emit


        static member add = Add |> CodeGen.emit
        static member sub = Sub |> CodeGen.emit
        static member mul = Mul |> CodeGen.emit
        static member div = Div |> CodeGen.emit
        static member rem = Rem |> CodeGen.emit
        static member bitand = And |> CodeGen.emit
        static member bitor = Or |> CodeGen.emit
        static member bifxor = Xor |> CodeGen.emit
        static member neg = Neg |> CodeGen.emit
        static member not = Not |> CodeGen.emit


        /// converts the top-most value to the given type
        static member conv (t : ValueType) = t |> Conv |> CodeGen.emit

        /// converts the top-most value to the given type (checked)
        static member convChecked (t : ValueType) = t |> ConvChecked |> CodeGen.emit

        /// loads null onto the stack
        static member ldnull = LdNull |> CodeGen.emit

        /// loads the given Constant onto the stack
        static member ldconst(c : Constant) = LdConst c |> CodeGen.emit

        /// loads the given int8 constant onto the stack
        static member ldconst(i : int8) = LdConst(Int8 i) |> CodeGen.emit

        /// loads the given uint8 constant onto the stack
        static member ldconst(i : uint8) = LdConst(UInt8 i) |> CodeGen.emit

        /// loads the given int16 constant onto the stack
        static member ldconst(i : int16) = LdConst(Int16 i) |> CodeGen.emit

        /// loads the given uint16 constant onto the stack
        static member ldconst(i : uint16) = LdConst(UInt16 i) |> CodeGen.emit

        /// loads the given int constant onto the stack
        static member ldconst(i : int32) = LdConst(Int32 i) |> CodeGen.emit

        /// loads the given uint32 constant onto the stack
        static member ldconst(i : uint32) = LdConst(UInt32 i) |> CodeGen.emit

        /// loads the given int64 constant onto the stack
        static member ldconst(i : int64) = LdConst(Int64 i) |> CodeGen.emit

        /// loads the given uint64 constant onto the stack
        static member ldconst(i : uint64) = LdConst(UInt64 i) |> CodeGen.emit

        /// loads the given nativeint constant onto the stack
        static member ldconst(i : nativeint) = LdConst(NativeInt i) |> CodeGen.emit

        /// loads the given unativeint constant onto the stack
        static member ldconst(i : unativeint) = LdConst(UNativeInt i) |> CodeGen.emit

        /// loads the given float constant onto the stack
        static member ldconst(i : float32) = LdConst(Float32 i) |> CodeGen.emit

        /// loads the given double constant onto the stack
        static member ldconst(i : float) = LdConst(Float64 i) |> CodeGen.emit

        /// loads the given string constant onto the stack
        static member ldconst(i : string) = LdConst(String i) |> CodeGen.emit


        /// load the given object as token onto the stack
        static member ldtoken (t : obj) = LdToken(t) |> CodeGen.emit

        /// load the given type-token onto the stack
        static member ldtoken (t : Type) = LdToken(t) |> CodeGen.emit

        /// tells the JITer that the following call is a tail-call
        static member tail = Tail |> CodeGen.emit

        /// marks the current location using the given label
        static member mark l = l |> Mark |> CodeGen.emit

        /// conditionally jumps to a label
        static member jmp (c : JumpCondition) = 
            fun l -> ConditionalJump(c, l) |> CodeGen.emit

        /// jumps to a label
        static member jmp (l : Label) = Jump(l) |> CodeGen.emit

        /// calls the given MethodInfo
        static member call (e : Expr) = 
            e |> getMethodInfo |> Call |> CodeGen.emit

        /// calls the given MethodInfo
        static member call (mi : MethodInfo) =
            mi |> Call |> CodeGen.emit
            
        /// returns from the current function
        static member ret = Ret |> CodeGen.emit

        /// prints a log-line using the given local values
        static member println (format : string, [<ParamArray>] args : obj[]) =
            let v = Local(typeof<string>)
            let arr = Local(typeof<obj[]>)
            codegen {
                do! LdConst (Int32 args.Length)
                do! NewArr typeof<obj>
                do! Stloc arr

                for i in 0..args.Length-1 do
                    do! Ldloc arr
                    do! LdConst (Int32 i)
                    
                    match args.[i] with
                        | :? Local as a ->
    
                            do! Ldloc a
                            if a.Type.IsValueType then 
                                do! Box a.Type
                            else
                                do! UnboxAny typeof<obj>
                        | a ->
                            do! LdConst (String (sprintf "%A" a))
                            do! UnboxAny typeof<obj>

                
                    do! Stelem typeof<obj>

                do! LdConst (String format)
                do! Ldloc arr
                do! Call printString
            }

        static member printfn (fmt : StringFormat<'a, CodeGen<unit>>) : 'a =
            kformatf (fun fmt -> IL.println(fmt.Format, fmt.Args)) fmt

        static member newlabel : CodeGen<Label> = fun s -> s, Label()
        static member newlocal (t : Type) : CodeGen<Local> = fun s -> s, Local(t)

module AssemblerTest =


    type TestMethods() =
        static member Add(a : int, b : int) =
            if a < 0 then b
            else a + b

    let listAdd() =
        let l = System.Collections.Generic.List<int>()
        let dis = Disassembler.disassemble (typeof<System.Collections.Generic.List<int>>.GetMethod("Add", [| typeof<int> |]))
        
        let mutable i = 0
        for instruction in dis.Body do
            printfn "%d: %A" i instruction
            i <- i + 1

        let ass : System.Collections.Generic.List<int> -> int -> unit = 
            Assembler.assemble dis.Body

        
        ass l 2
        ass l 3
        printfn "%A" (Seq.toList l)

    let testCustom() =
        let custom : int -> int -> int =
            inlineil {
                let! bad = IL.newlabel
                let! a0 = IL.newlocal typeof<int>
                let! a1 = IL.newlocal typeof<int>

                do! IL.ldarg 0
                do! IL.stloc a0
                do! IL.ldarg 1
                do! IL.stloc a1

                do! IL.ldloc a0
                do! IL.ldloc a1
                do! IL.jmp LessOrEqual bad
       
                do! IL.ldloc a0
                do! IL.ldloc a1
                do! IL.add
                do! IL.call <@ Fun.NextPowerOfTwo : int -> int @>
                do! IL.ret
        
                do! IL.mark bad
                do! IL.printfn "{ a = %A; b = %A }" a0 a1
                do! IL.ldconst 0
                do! IL.ret
            }


        Log.line "custom 1 2 = %A" (custom 1 2)
        Log.line "custom 2 1 = %A" (custom 2 1)

    let testLambda() =
        
        let f = Fun.Min : int * int -> int

        let invoke = f.GetType().GetMethod "Invoke"
        let def = Disassembler.disassemble invoke

        for i in def.Body do
            Log.line "%A" i

        ()


    let testInject() =
        let l = System.Collections.Generic.List<int>()
        let dis = Disassembler.disassemble (typeof<System.Collections.Generic.List<int>>.GetMethod("Add", [| typeof<int> |]))
        
        let str = "inside List<int>::Add(int)"

        let injected =
            { dis with 
                Body = WriteLine(str)::dis.Body
            }

        let ass : System.Collections.Generic.List<int> -> int -> unit = 
            Assembler.assembleDefinition injected

        
        ass l 2
        ass l 3
        printfn "%A" (Seq.toList l)

    let run() =
        testCustom()
        //testLambda()
        //testCustom()
        //listAdd()
        //testInject()

