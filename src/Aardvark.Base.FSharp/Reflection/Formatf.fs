namespace Aardvark.Base

open System
open System.Text
open System.Text.RegularExpressions
open System.Collections.Concurrent
open Aardvark.Base
open Microsoft.FSharp.Reflection

type StringFormat = 
    struct
        val mutable public Format : string
        val mutable public Args : obj[]

        override x.ToString() = String.Format(x.Format, x.Args)

        new(f,a) = { Format = f; Args = a }

    end
        
type StringFormat<'a, 'r> = Format<'a, unit, StringFormat, 'r>

[<AutoOpen>]
module Printf =
    module private FormatString =

        /// Used for width and precision to denote that user has specified '*' flag
        [<Literal>]
        let StarValue = -1
        /// Used for width and precision to denote that corresponding value was omitted in format string
        [<Literal>]
        let NotSpecifiedValue = -2

        type FormatFlags = 
            | None = 0
            | LeftJustify = 1
            | PadWithZeros = 2
            | PlusForPositives = 4
            | SpaceForPositives = 8

        let inline isDigit c = c >= '0' && c <= '9'
        let intFromString (s : string) pos = 
            let rec go acc i =
                if isDigit s.[i] then 
                    let n = int s.[i] - int '0'
                    go (acc * 10 + n) (i + 1)
                else acc, i
            go 0 pos

        let parseFlags (s : string) i : FormatFlags * int = 
            let rec go flags i = 
                match s.[i] with
                | '0' -> go (flags ||| FormatFlags.PadWithZeros) (i + 1)
                | '+' -> go (flags ||| FormatFlags.PlusForPositives) (i + 1)
                | ' ' -> go (flags ||| FormatFlags.SpaceForPositives) (i + 1)
                | '-' -> go (flags ||| FormatFlags.LeftJustify) (i + 1)
                | _ -> flags, i
            go FormatFlags.None i

        let parseWidth (s : string) i : int * int = 
            if s.[i] = '*' then StarValue, (i + 1)
            elif isDigit (s.[i]) then intFromString s i
            else NotSpecifiedValue, i

        let parsePrecision (s : string) i : int * int = 
            if s.[i] = '.' then
                if s.[i + 1] = '*' then StarValue, i + 2
                elif isDigit (s.[i + 1]) then intFromString s (i + 1)
                else raise (ArgumentException("invalid precision value"))
            else NotSpecifiedValue, i
        
        let parseTypeChar (s : string) i : char * int = 
            s.[i], (i + 1)

        let findNextFormatSpecifier (s : string) i = 
            let rec go i (buf : Text.StringBuilder) =
                if i >= s.Length then 
                    s.Length, s.Length, buf.ToString()
                else
                    let c = s.[i]
                    if c = '%' then
                        if i + 1 < s.Length then
                            let _, i1 = parseFlags s (i + 1)
                            let w, i2 = parseWidth s i1
                            let p, i3 = parsePrecision s i2
                            let typeChar, i4 = parseTypeChar s i3
                            // shortcut for the simpliest case
                            // if typeChar is not % or it has star as width\precision - resort to long path
                            if typeChar = '%' && not (w = StarValue || p = StarValue) then 
                                buf.Append('%') |> ignore
                                go i4 buf
                            else 
                                i, i4, buf.ToString()
                        else
                            raise (ArgumentException("Missing format specifier"))
                    else 
                        buf.Append(c) |> ignore
                        go (i + 1) buf
            go i (Text.StringBuilder())

    open System.Reflection
    open System.Reflection.Emit

    type private StringFormatCache<'a, 'res> private() =
        static let contType = typeof<StringFormat -> 'res>

        static let rec getFunctionSignature (t : Type) =
            if t = typeof<'res> then [], t
            elif FSharpType.IsFunction t then
                let (d,i) = FSharpType.GetFunctionElements t
                let args, ret = getFunctionSignature i
                d::args, ret
            else
                [], t

        static let args, _ = getFunctionSignature typeof<'a>
        static let args : Type[] = List.toArray (contType::args)

        static let invokeCont = contType.GetMethod("Invoke", BindingFlags.Instance ||| BindingFlags.Public, Type.DefaultBinder, [| typeof<StringFormat> |], null)
        static let formatCtor = typeof<StringFormat>.GetConstructor [| typeof<string>; typeof<obj[]> |]

        static let createFunction (fmt : string) : (StringFormat -> 'res) -> 'a =
            let meth =
                DynamicMethod(
                    fmt,
                    typeof<'res>, args,
                    true
                )

            let il = meth.GetILGenerator()

            let arr = il.DeclareLocal(typeof<obj[]>)
            il.Emit(OpCodes.Ldc_I4, args.Length - 1)
            il.Emit(OpCodes.Newarr, typeof<obj>)
            il.Emit(OpCodes.Stloc, arr)

            for i in 1..args.Length-1 do
                let t = args.[i] 
                il.Emit(OpCodes.Ldloc, arr)
                il.Emit(OpCodes.Ldc_I4, i - 1)

                il.Emit(OpCodes.Ldarg, i)
                if t.IsValueType then
                    il.Emit(OpCodes.Box, t)

                il.Emit(OpCodes.Stelem_Ref)

            il.Emit(OpCodes.Ldarg_0)

            il.Emit(OpCodes.Ldstr, fmt)
            il.Emit(OpCodes.Ldloc, arr)
            il.Emit(OpCodes.Newobj, formatCtor)

            il.EmitCall(OpCodes.Callvirt, invokeCont, null)
            il.Emit(OpCodes.Ret)

            let del = 
                if typeof<'res> = typeof<System.Void> then
                    let funcType = typedefof<Action<_>>.FullName.Replace("1", string (args.Length)) |> Type.GetType
                    let funcType = funcType.MakeGenericType args

                    meth.CreateDelegate(funcType)
                else
                    let funcType = typedefof<Func<_>>.FullName.Replace("1", string (args.Length + 1)) |> Type.GetType
                    let funcType = funcType.MakeGenericType (Array.append args [|typeof<'res>|])

                    meth.CreateDelegate(funcType)

            DelegateAdapters.wrap del

        static let create(value : string) : (StringFormat -> 'res) -> 'a =
            
            let mutable args = []
            let mutable ret = typeof<'a>

            let formatBuilder = StringBuilder()
            let mutable i = 0
            let mutable c = 0
            while c < value.Length do
                let (s,e,str) = FormatString.findNextFormatSpecifier value c
                formatBuilder.Append(str.Replace("{", "{{").Replace("}", "}}")) |> ignore
                c <- e

                if c < value.Length then
                    formatBuilder.Append("{" + string i + "}") |> ignore
                    i <- i + 1
                    let (d,i) = FSharpType.GetFunctionElements ret
                    args <- d::args
                    ret <- i

            let formatString = formatBuilder.ToString()

            createFunction formatString

        static let cache = ConcurrentDictionary<string, (StringFormat -> 'res) -> 'a>()

        static member get (fmt : StringFormat<'a, 'res>) =
            cache.GetOrAdd(fmt.Value, Func<string, _>(fun value -> create value))


    let formatf (fmt : StringFormat<'a, StringFormat>) : 'a =
        StringFormatCache<'a, StringFormat>.get fmt id

    let kformatf (f : StringFormat -> 'res) (fmt : StringFormat<'a, 'res>) : 'a =
        StringFormatCache<'a, 'res>.get fmt f

    