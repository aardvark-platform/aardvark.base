namespace Aardvark.Base.Native

open Aardvark.Base
open System

type IBlobFile =
    abstract member Name        : Guid
    abstract member Exists      : bool
    abstract member Size        : int64
    abstract member Read        : unit -> byte[]
    abstract member Write       : byte[] -> unit
    abstract member Delete      : unit -> unit
    abstract member CopyTo      : IBlobFile -> unit

type IBlobStore =
    inherit IDisposable
    abstract member Memory      : Mem
    abstract member Create      : unit -> IBlobFile
    abstract member Get         : Guid -> IBlobFile