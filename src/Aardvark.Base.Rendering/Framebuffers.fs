namespace Aardvark.Base


type TextureDimension =
    | Texture1d
    | Texture1dArray
    | Texture2d
    | Texture2dArray
    | Texture3d
    | TextureCube

type IFramebufferOutput =
    abstract member Samples : int

type ITexture = interface end

type FramebufferTexture =
    inherit ITexture
    abstract member Handle : obj
    abstract member Samples : int
    abstract member Dimension : TextureDimension
    abstract member ArraySize : int
    abstract member MipMapLevels : int


type IFramebuffer =
    abstract member Handle : obj

type Framebuffer = { handle : obj; attachments : Map<Symbol, IFramebufferOutput>;  } with
    interface IFramebuffer with
        member x.Handle = x.handle


type TextureOutputView = { texture : FramebufferTexture; level : int; slice : int } with
    interface IFramebufferOutput with
        member x.Samples = x.texture.Samples

type Renderbuffer =
    inherit IFramebufferOutput
    abstract member Handle : obj
    
