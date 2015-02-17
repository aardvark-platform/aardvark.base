namespace Aardvark.Base

open System
open Aardvark.Base.Incremental

type ISurface = interface end


[<AllowNullLiteral>]
type IAttributeProvider =
    abstract member TryGetAttribute : Symbol * byref<IMod<Array>> -> bool

[<AllowNullLiteral>]
type IUniformProvider =
    abstract member TryGetAttribute : Symbol * byref<IMod> -> bool


module private RenderJobIds =
    open System.Threading
    let mutable private currentId = 0
    let newId() = Interlocked.Increment &currentId

[<CustomEquality>]
[<CustomComparison>]
type RenderJob =
    {
        Id : int
        CreationPath : string
        mutable AttributeScope : obj
                
        mutable IsActive : IMod<bool>
        mutable RenderPass : IMod<uint64>
                
        mutable DrawCallInfo : IMod<DrawCallInfo>
        mutable Surface : IMod<ISurface>
                
        mutable DepthTest : IMod<DepthTestMode>
        mutable CullMode : IMod<CullMode>
        mutable BlendMode : IMod<BlendMode>
        mutable FillMode : IMod<FillMode>
        mutable StencilMode : IMod<StencilMode>
                
        mutable Indices : IMod<Array>
        mutable InstanceAttributes : IAttributeProvider
        mutable VertexAttributes : IAttributeProvider
                
        mutable Uniforms : IUniformProvider
    }

    static member Create(path : string) =
        { Id = RenderJobIds.newId()
          CreationPath = path;
          AttributeScope = null
          IsActive = null
          RenderPass = null
          DrawCallInfo = null
          Surface = null
          DepthTest = null
          CullMode = null
          BlendMode = null
          FillMode = null
          StencilMode = null
          Indices = null
          InstanceAttributes = null
          VertexAttributes = null
          Uniforms = null
        }

    static member Create() = RenderJob.Create("UNKNWON")

    override x.GetHashCode() = x.Id
    override x.Equals o =
        match o with
            | :? RenderJob as o -> x.Id = o.Id
            | _ -> false

    interface IComparable with
        member x.CompareTo o =
            match o with
                | :? RenderJob as o -> compare x.Id o.Id
                | _ -> failwith "uncomparable"
