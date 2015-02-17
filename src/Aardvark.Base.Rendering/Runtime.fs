namespace Aardvark.Base.Rendering

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open System.Collections.Generic


type IRenderTask =
    inherit IAdaptiveObject
    abstract member Run : Framebuffer -> unit

type IRuntime =
    abstract member CompileClear : IMod<C4f> -> IMod<double> -> IRenderTask
    abstract member CompileRender : aset<RenderJob> -> IRenderTask

    abstract member CreateTexture : IMod<V2i> -> IMod<PixFormat> -> IMod<int> -> FramebufferTexture
    abstract member CreateRenderbuffer : IMod<V2i> -> IMod<PixFormat> -> IMod<int> -> Renderbuffer

    abstract member CreateFramebuffer : Map<Symbol, IFramebufferOutput> -> IFramebuffer


type ITexture = interface end

type ShaderStage =
    | Vertex = 1
    | TessControl = 2
    | TessEval = 3
    | Geometry = 4
    | Pixel = 5


type IBackendSurface =
    inherit ISurface

    abstract member Code : string
    abstract member EntryPoints : Dictionary<ShaderStage, string>
    abstract member Uniforms : IUniformProvider



type IGeneratedSurface =
    inherit ISurface

    abstract member Generate : IRuntime -> IBackendSurface
