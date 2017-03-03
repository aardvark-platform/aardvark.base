namespace Aardvark.Base.Incremental

open Aardvark.Base

type IMapReader<'k, 'v> = IOpReader<hmap<'k, 'v>, hmap<'k, ElementOperation<'v>>>

type amap<'k, 'v> =
    abstract member IsConstant : bool
    abstract member GetReader : unit -> IMapReader<'k, 'v>
    abstract member Content : IMod<hmap<'k, 'v>>

//type cmap<'k, 'v>(initial : seq<'k * 'v>) =
//    let history = History<hmap<'k, 'v>, hmap<'k, ElementOperation<'v>>>(failwith "")
//
//

