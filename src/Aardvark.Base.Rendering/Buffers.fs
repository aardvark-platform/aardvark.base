namespace Aardvark.Base

open System
open Aardvark.Base.Incremental
open System.Runtime.InteropServices

type IBuffer = interface end

type BufferView(b : IBuffer, elementType : Type, offset : int, stride : int) =
    member x.Buffer = b
    member x.ElementType = elementType
    member x.Stride = stride
    member x.Offset = offset

    new(b : IBuffer, elementType : Type, offset : int) =
        BufferView(b, elementType, offset, 0)

    new(b : IBuffer, elementType : Type) =
        BufferView(b, elementType, 0, 0)

type ArrayBuffer(data : IMod<Array>) =
    interface IBuffer
    member x.Data = data