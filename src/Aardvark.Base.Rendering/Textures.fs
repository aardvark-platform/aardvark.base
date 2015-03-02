namespace Aardvark.Base

open System
open Aardvark.Base.Incremental
open System.Runtime.InteropServices

[<AllowNullLiteral>]
type ITexture = 
    abstract member WantMipMaps : bool

type BitmapTexture(bmp : System.Drawing.Bitmap, wantMipMaps : bool) =
    member x.WantMipMaps = wantMipMaps
    member x.Bitmap = bmp
    interface ITexture with
        member x.WantMipMaps = x.WantMipMaps

type FileTexture(fileName : string, wantMipMaps : bool) =
    do if System.IO.File.Exists fileName |> not then failwithf "File does not exist: %s" fileName

    member x.FileName = fileName
    member x.WantMipMaps = wantMipMaps
    interface ITexture with
        member x.WantMipMaps = x.WantMipMaps

type PixTexture2d(data : PixImageMipMap, wantMipMaps : bool) =
    member x.PixImageMipMap = data
    member x.WantMipMaps = wantMipMaps
    interface ITexture with
        member x.WantMipMaps = x.WantMipMaps

type PixTextureCube(data : PixImageCube, wantMipMaps : bool) =
    member x.PixImageCube = data
    member x.WantMipMaps = wantMipMaps
    interface ITexture with
        member x.WantMipMaps = x.WantMipMaps

type PixTexture3d(data : PixVolume, wantMipMaps : bool) =
    member x.PixVolume = data
    member x.WantMipMaps = wantMipMaps
    interface ITexture with
        member x.WantMipMaps = x.WantMipMaps