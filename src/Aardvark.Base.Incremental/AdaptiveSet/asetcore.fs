namespace Aardvark.Base.Incremental

open Aardvark.Base

type ISetReader<'a> = IOpReader<hrefset<'a>, hdeltaset<'a>>

type aset<'a> =
    abstract member IsConstant  : bool
    abstract member Content     : IMod<hrefset<'a>>
    abstract member GetReader   : unit -> ISetReader<'a>
