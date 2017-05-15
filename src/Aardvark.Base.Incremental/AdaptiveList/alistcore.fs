namespace Aardvark.Base.Incremental

open Aardvark.Base

type IListReader<'a> = IOpReader<plist<'a>, pdeltalist<'a>>

[<CompiledName("IAdaptiveList")>]
type alist<'a> =
    abstract member IsConstant  : bool
    abstract member Content     : IMod<plist<'a>>
    abstract member GetReader   : unit -> IListReader<'a>
