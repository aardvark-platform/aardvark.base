namespace Aardvark.Base

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open System.Collections.Generic


type RenderingResult(f : IFramebuffer, stats : FrameStatistics) =
    member x.Framebuffer = f
    member x.Statistics = stats

type IRenderTask =
    inherit IDisposable
    inherit IAdaptiveObject
    abstract member Run : IFramebuffer -> RenderingResult

type IRuntime =
    abstract member CompileClear : IMod<C4f> -> IMod<double> -> IRenderTask
    abstract member CompileRender : aset<RenderJob> -> IRenderTask

    abstract member CreateTexture : IMod<V2i> -> IMod<PixFormat> -> IMod<int> -> FramebufferTexture
    abstract member CreateRenderbuffer : IMod<V2i> -> IMod<PixFormat> -> IMod<int> -> FramebufferRenderbuffer

    abstract member CreateFramebuffer : Map<Symbol, IMod<IFramebufferOutput>> -> IFramebuffer


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
