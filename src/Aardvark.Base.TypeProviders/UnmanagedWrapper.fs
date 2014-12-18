#if INTERACTIVE
#else
namespace Aardvark.Base
#endif

module CTags =
    open System.IO
    open System.Diagnostics
    open System.Reflection
    open System.Text.RegularExpressions
    open System

    type private Kind = Macro | Variable | Function | Unknown of string
    type private Tag = { name : string; kind : Kind; line : int; declaration : string }

    [<StructuredFormatDisplay("{AsString}")>]
    type TagType = SystemType of Type | CustomType of string with
        member x.AsString = 
            match x with
                | SystemType t -> t.Name
                | CustomType t -> t

    type Parameter = { parameterName : string; parameterType : TagType } 
    type Function = { returnType : TagType; name : string; parameters : list<Parameter>; line : int }
    type Variable = { variableType : TagType; variableName : string; defaultValue : Option<string>; line : int }
    type Entry = FunctionEntry of Function | VariableEntry of Variable | UnknownEntry of string

    #if INTERACTIVE
    let cTagsPath = "E:\\Development\\Aardvark 2014\\BinPrebuilt\\Release\\ctags.exe"
    #else
    let cTagsPath = "..\\..\\BinPrebuilt\\Release\\ctags.exe"
    #endif
    
    let private args = "-x -N "

    let private resultRx = Regex @"(?<name>[a-zA-Z_][a-zA-Z0-9_]*)[ \t]+(?<kind>[a-zA-Z_0-9]+)[ \t]+(?<line>[0-9]+)[ \t]+(?<path>[\:\.\\a-zA-Z0-9_\/]+)[ \t]+(?<decl>.*)"
    let private functionHeaderRx = Regex @"(?<ret>[a-zA-Z_0-9\(\)]+)[ \t]+(?<name>[a-zA-Z_][a-zA-Z0-9_]*)"
    let private paramRx = Regex @"(\(|,)[ \t]*(?<type>[a-zA-Z_][a-zA-Z0-9_\* ]*)[ \t+](?<name>[a-zA-Z_][a-zA-Z0-9_]*)(?<array>[0-9\[\]]+)?"
    let private constPrefixRx = Regex "const[ \t]+"
    let private variableRx = Regex @"(?<type>[a-zA-Z_][a-zA-Z0-9_\* ]*)[ \t+](?<names>[^=;]+)(?<array>[0-9\[\]]+)?([ \t]*=[ \t]*(?<value>[^;]*))?;"
    

    let private translateKind (value : string) =
        match value with
            | "function" -> Function
            | "macro" -> Macro
            | "variable" -> Variable
            | _ -> Unknown value

    let private runCTags (file : string) =
        let pi = ProcessStartInfo(cTagsPath, sprintf "%s \"%s\"" args file)
        pi.UseShellExecute <- false
        pi.RedirectStandardOutput <- true
        pi.RedirectStandardError <- true

        let p = Process.Start(pi)

        p.WaitForExit()
        if p.ExitCode = 0 then
            let op = p.StandardOutput.ReadToEnd()
            let matches = resultRx.Matches op

            let matches = seq { for i in 0..matches.Count-1 do yield matches.[i] }

            matches |> Seq.toList
                    |> List.map (fun m -> 
                        { name = m.Groups.["name"].Value;
                          kind = m.Groups.["kind"].Value |> translateKind 
                          line = System.Int32.Parse (m.Groups.["line"].Value)
                          declaration = m.Groups.["decl"].Value }
                       )
                    |> Choice1Of2
        else
            let err = p.StandardError.ReadToEnd()
            Choice2Of2 err

    let rec private translateType (name : string) (arraySize : Option<string>) =
        match arraySize with
            | Some s ->
                let inner = translateType name None
                match inner with
                    | SystemType t -> t.MakeArrayType() |> SystemType
                    | CustomType t -> CustomType (sprintf "%s*" t)
            | None -> 
                let name = constPrefixRx.Replace(name, "")
                match name with
                    | "int" -> typeof<int> |> SystemType
                    | "float" -> typeof<float32> |> SystemType
                    | "double" -> typeof<float> |> SystemType
                    | "char" -> typeof<byte> |> SystemType
                    | "char*" -> typeof<string> |> SystemType
                    | _ -> CustomType name

    let private parseParameterList (value : string) =
        let matches =paramRx.Matches value
        let matches = seq { for i in 0..matches.Count-1 do yield matches.[i] }

        matches |> Seq.map (fun p -> 
                        let t = p.Groups.["type"].Value
                        let a = p.Groups.["array"]
                        let a = if a.Success then Some a.Value else None

                        let t = translateType t a

                        { parameterName = p.Groups.["name"].Value
                          parameterType = t }
                   )
                |> Seq.toList

    let private parseFunctionTag (tag : Tag) =
        let d = tag.declaration

        let header = functionHeaderRx.Match d
        if header.Success && tag.kind = Function then
            let returnType = header.Groups.["ret"].Value
            let name = header.Groups.["name"].Value

            let returnType = translateType returnType None

            let rest = d.Substring(header.Index + header.Length)
            let parameters = parseParameterList rest

            Some { returnType = returnType
                   name = name
                   parameters = parameters
                   line = tag.line }

        else
            None

    let private parseVariableTag (tag : Tag) =
        let d = tag.declaration

        let decl = variableRx.Match d
        if decl.Success && tag.kind = Variable then

            let t = decl.Groups.["type"].Value
            let names = decl.Groups.["names"].Value
            let value = decl.Groups.["value"]
            let arr = decl.Groups.["array"]

            let value = if value.Success then Some value.Value else None
            let arr = if arr.Success then Some arr.Value else None
            let t = translateType t arr

            { variableType = t
              variableName = tag.name
              defaultValue = value 
              line = tag.line } |> Some

        else
            None

    let private parseTag (t : Tag) =
        match t.kind with
            | Function -> 
                match parseFunctionTag t with
                    | Some t -> [FunctionEntry t]
                    | None -> []
            | Variable -> 
                match parseVariableTag t with
                    | Some t -> [VariableEntry t]
                    | None -> []
            | _ ->
                [UnknownEntry t.declaration]

    let private parseTags (tags : list<Tag>) =
        tags |> List.collect parseTag

    let parseFile (file : string) =
        let m = runCTags file
        match m with
            | Choice1Of2 m -> parseTags m
            | Choice2Of2 e -> printfn "ERROR: %A" e
                              []

    let parse (code : string) =
        let temp = Path.GetTempFileName() + ".c"
        File.WriteAllText(temp, code)
        let r = parseFile temp
        File.Delete temp
        r

module WrapperGenerator =
    open CTags
    open System
    open System.Reflection
    open Microsoft.FSharp.Core.CompilerServices
    open Microsoft.FSharp.Quotations
    
    module List =
        let chooseAll (f : 'a -> Option<'b>) (l : list<'a>) : Option<list<'b>> =
            let r = l |> List.map f

            if r |> List.forall (fun o -> o.IsSome) then
                r |> List.map (fun r -> r.Value) |> Some
            else
                None


    let translateType (t : TagType) =
        match t with
            | SystemType t -> Some t
            | _ -> None

    let wrapParameter (p : Parameter) =
        match translateType p.parameterType with
            | Some t -> ProvidedParameter(p.parameterName, t) |> Some
            | None -> None 
//    
//    let wrapFunction (dll : string) (f : Function) =
//        let parameters = f.parameters |> List.chooseAll wrapParameter
//        let returnType = f.returnType |> translateType
//
//        match parameters, returnType with
//            | Some p, Some ret ->
//                let p = System.Reflection.Emit.MethodBuilder()
//                p.SetMethodAttrs (MethodAttributes.PinvokeImpl ||| MethodAttributes.Static ||| MethodAttributes.Public)     
//                
//                let ctor = typeof<System.Runtime.InteropServices.DllImportAttribute>.GetConstructor [|typeof<string>|]
//                let att = System.Reflection.Emit.CustomAttributeBuilder(ctor, [|dll :> obj|])
//                
//
//
//
//                //p.AddCustomAttribute(att)
//                
//                         
//                Some p
//            | _ -> None
