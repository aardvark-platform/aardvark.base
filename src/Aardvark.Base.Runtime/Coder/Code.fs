namespace Aardvark.Base

open System
open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open Aardvark.Base


type IDatabase =
    abstract member TryStore    : value : 'a    * [<Out>] id : byref<Guid>      -> bool
    abstract member TryLoad     : id : Guid     * [<Out>] value : byref<'a>     -> bool
    
type CodeState =
    {
        Version         : Version
        MemberStack     : list<MemberInfo>
        References      : RefMap<obj, Guid>
        Values          : Map<Guid, obj>
        Database        : IDatabase
    }

type Code<'a> = State<CodeState, 'a>

module Code =
    let version =
        { new Code<Version>() with
            override x.Run(s) = s.Version
        }

    let push (m : MemberInfo) =
        { new Code<unit>() with
            override x.RunUnit(s) =
                s <- { s with MemberStack = m::s.MemberStack }
        }

    let pop =
        { new Code<unit>() with
            override x.RunUnit(s) =
                s <- { s with MemberStack = List.tail s.MemberStack }
        }

    let peek =
        { new Code<MemberInfo>() with
            override x.Run(s) =
                List.head s.MemberStack
        }

    let tryStoreLocal (value : 'a) =
        { new Code<bool * Guid>() with
            override x.Run(s) =
                let v = value :> obj
                match RefMap.tryFind v s.References with
                    | Some id ->
                        (false, id)
                    | None ->
                        let id = Guid.NewGuid()
                        s <- { s with References = RefMap.add v id s.References }
                        (true, id)
        }

    let tryLoadLocal (id : Guid) =
        { new Code<Option<'a>>() with
            override x.Run(s) =
                Map.tryFind id s.Values |> Option.map unbox
        }

    let storeLocal (id : Guid) (value : 'a) =
        { new Code<unit>() with
            override x.RunUnit(s) =
                s <- { s with Values = Map.add id (value :> obj) s.Values }
        }



[<AutoOpen>]
module CodeBuilder =

    type CodeBuilder() =
        inherit SpecificStateBuilder<CodeState>()

        member inline x.Run(v : Code<'a>) : Code<'a> =
            v

    let code = CodeBuilder()



