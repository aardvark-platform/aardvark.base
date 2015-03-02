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
    abstract member CompileClear : IMod<C4f> * IMod<double> -> IRenderTask
    abstract member CompileRender : aset<RenderJob> -> IRenderTask

    abstract member CreateTexture : IMod<V2i> * IMod<PixFormat> * IMod<int> * IMod<int> -> IFramebufferTexture
    abstract member CreateRenderbuffer : IMod<V2i> * IMod<PixFormat> * IMod<int> -> IFramebufferRenderbuffer

    abstract member CreateFramebuffer : Map<Symbol, IMod<IFramebufferOutput>> -> IFramebuffer


type ShaderStage =
    | Vertex = 1
    | TessControl = 2
    | TessEval = 3
    | Geometry = 4
    | Pixel = 5



type BackendSurface(code : string, entryPoints : Dictionary<ShaderStage, string>, uniforms : SymbolDict<IMod>, samplerStates : SymbolDict<SamplerStateDescription>, semanticMap : SymbolDict<Symbol>) =
    interface ISurface
    member x.Code = code
    member x.EntryPoints = entryPoints
    member x.Uniforms = uniforms
    member x.SamplerStates = samplerStates
    member x.SemanticMap = semanticMap

    new(code, entryPoints) = BackendSurface(code, entryPoints, SymDict.empty, SymDict.empty, SymDict.empty)
    new(code, entryPoints, uniforms) = BackendSurface(code, entryPoints, uniforms, SymDict.empty, SymDict.empty)
    new(code, entryPoints, uniforms, samplerStates) = BackendSurface(code, entryPoints, uniforms, samplerStates, SymDict.empty)

type IGeneratedSurface =
    inherit ISurface

    abstract member Generate : IRuntime -> BackendSurface
