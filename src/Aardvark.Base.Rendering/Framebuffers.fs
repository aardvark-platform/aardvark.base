namespace Aardvark.Base

open System

type TextureDimension =
    | Texture1D = 1
    | Texture2D = 2
    | TextureCube = 3
    | Texture3D = 4

type IFramebufferOutput =
    abstract member Samples : int
    abstract member Size : V2i

type IFramebufferTexture =
    inherit IDisposable
    inherit ITexture
    abstract member Handle : obj
    abstract member Samples : int
    abstract member Dimension : TextureDimension
    abstract member ArraySize : int
    abstract member MipMapLevels : int
    abstract member GetSize : int -> V2i

type IFramebuffer =
    inherit IDisposable
    abstract member Size : V2i
    abstract member Handle : obj
    abstract member Attachments : Map<Symbol, IFramebufferOutput>

type TextureOutputView = { texture : IFramebufferTexture; level : int; slice : int } with
    interface IFramebufferOutput with
        member x.Samples = x.texture.Samples
        member x.Size = x.texture.GetSize x.level

type IFramebufferRenderbuffer =
    inherit IDisposable
    inherit IFramebufferOutput
    abstract member Handle : obj
    
