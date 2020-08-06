﻿namespace Aardvark.Base

open System
open System.Collections.Generic
open Aardvark.Base
open System.Reflection
open System.Runtime.InteropServices
open Microsoft.FSharp.Reflection
open FSharp.Data.Adaptive


module PrimitiveValueConverter =
    let private trafoToM44f(t : Trafo3d) =
        M44f.op_Explicit (t.Forward)

    let private integralConversions =
        [
            // int8 -> other scalars
            ( fun (i : int8)        -> int8 i ) :> obj
            ( fun (i : int8)        -> int16 i ) :> obj
            ( fun (i : int8)        -> int32 i ) :> obj
            ( fun (i : int8)        -> int64 i ) :> obj
            ( fun (i : int8)        -> uint8 i ) :> obj
            ( fun (i : int8)        -> uint16 i ) :> obj
            ( fun (i : int8)        -> uint32 i ) :> obj
            ( fun (i : int8)        -> uint64 i ) :> obj
            ( fun (i : int8)        -> float32 i ) :> obj
            ( fun (i : int8)        -> float i ) :> obj

            // int16 -> other scalars
            ( fun (i : int16)       -> int8 i ) :> obj
            ( fun (i : int16)       -> int16 i ) :> obj
            ( fun (i : int16)       -> int32 i ) :> obj
            ( fun (i : int16)       -> int64 i ) :> obj
            ( fun (i : int16)       -> uint8 i ) :> obj
            ( fun (i : int16)       -> uint16 i ) :> obj
            ( fun (i : int16)       -> uint32 i ) :> obj
            ( fun (i : int16)       -> uint64 i ) :> obj
            ( fun (i : int16)       -> float32 i ) :> obj
            ( fun (i : int16)       -> float i ) :> obj

            // int32 -> other scalars
            ( fun (i : int32)       -> int8 i ) :> obj
            ( fun (i : int32)       -> int16 i ) :> obj
            ( fun (i : int32)       -> int32 i ) :> obj
            ( fun (i : int32)       -> int64 i ) :> obj
            ( fun (i : int32)       -> uint8 i ) :> obj
            ( fun (i : int32)       -> uint16 i ) :> obj
            ( fun (i : int32)       -> uint32 i ) :> obj
            ( fun (i : int32)       -> uint64 i ) :> obj
            ( fun (i : int32)       -> float32 i ) :> obj
            ( fun (i : int32)       -> float i ) :> obj

            // int64 -> other scalars
            ( fun (i : int64)       -> int8 i ) :> obj
            ( fun (i : int64)       -> int16 i ) :> obj
            ( fun (i : int64)       -> int32 i ) :> obj
            ( fun (i : int64)       -> int64 i ) :> obj
            ( fun (i : int64)       -> uint8 i ) :> obj
            ( fun (i : int64)       -> uint16 i ) :> obj
            ( fun (i : int64)       -> uint32 i ) :> obj
            ( fun (i : int64)       -> uint64 i ) :> obj
            ( fun (i : int64)       -> float32 i ) :> obj
            ( fun (i : int64)       -> float i ) :> obj

            // uint8 -> other scalars
            ( fun (i : uint8)       -> int8 i ) :> obj
            ( fun (i : uint8)       -> int16 i ) :> obj
            ( fun (i : uint8)       -> int32 i ) :> obj
            ( fun (i : uint8)       -> int64 i ) :> obj
            ( fun (i : uint8)       -> uint8 i ) :> obj
            ( fun (i : uint8)       -> uint16 i ) :> obj
            ( fun (i : uint8)       -> uint32 i ) :> obj
            ( fun (i : uint8)       -> uint64 i ) :> obj
            ( fun (i : uint8)       -> float32 i ) :> obj
            ( fun (i : uint8)       -> float i ) :> obj

            // uint16 -> other scalars
            ( fun (i : uint16)      -> int8 i ) :> obj
            ( fun (i : uint16)      -> int16 i ) :> obj
            ( fun (i : uint16)      -> int32 i ) :> obj
            ( fun (i : uint16)      -> int64 i ) :> obj
            ( fun (i : uint16)      -> uint8 i ) :> obj
            ( fun (i : uint16)      -> uint16 i ) :> obj
            ( fun (i : uint16)      -> uint32 i ) :> obj
            ( fun (i : uint16)      -> uint64 i ) :> obj
            ( fun (i : uint16)      -> float32 i ) :> obj
            ( fun (i : uint16)      -> float i ) :> obj


            // uint32 -> other scalars
            ( fun (i : uint32)      -> int8 i ) :> obj
            ( fun (i : uint32)      -> int16 i ) :> obj
            ( fun (i : uint32)      -> int32 i ) :> obj
            ( fun (i : uint32)      -> int64 i ) :> obj
            ( fun (i : uint32)      -> uint8 i ) :> obj
            ( fun (i : uint32)      -> uint16 i ) :> obj
            ( fun (i : uint32)      -> uint32 i ) :> obj
            ( fun (i : uint32)      -> uint64 i ) :> obj
            ( fun (i : uint32)      -> float32 i ) :> obj
            ( fun (i : uint32)      -> float i ) :> obj

            // uint64 -> other scalars
            ( fun (i : uint64)      -> int8 i ) :> obj
            ( fun (i : uint64)      -> int16 i ) :> obj
            ( fun (i : uint64)      -> int32 i ) :> obj
            ( fun (i : uint64)      -> int64 i ) :> obj
            ( fun (i : uint64)      -> uint8 i ) :> obj
            ( fun (i : uint64)      -> uint16 i ) :> obj
            ( fun (i : uint64)      -> uint32 i ) :> obj
            ( fun (i : uint64)      -> uint64 i ) :> obj
            ( fun (i : uint64)      -> float32 i ) :> obj
            ( fun (i : uint64)      -> float i ) :> obj


            // nativeint -> other scalars
            ( fun (i : nativeint)   -> int8 i ) :> obj
            ( fun (i : nativeint)   -> int16 i ) :> obj
            ( fun (i : nativeint)   -> int32 i ) :> obj
            ( fun (i : nativeint)   -> int64 i ) :> obj
            ( fun (i : nativeint)   -> uint8 i ) :> obj
            ( fun (i : nativeint)   -> uint16 i ) :> obj
            ( fun (i : nativeint)   -> uint32 i ) :> obj
            ( fun (i : nativeint)   -> uint64 i ) :> obj
            ( fun (i : nativeint)   -> float32 i ) :> obj
            ( fun (i : nativeint)   -> float i ) :> obj

            // unativeint -> other scalars
            ( fun (i : unativeint)  -> int8 i ) :> obj
            ( fun (i : unativeint)  -> int16 i ) :> obj
            ( fun (i : unativeint)  -> int32 i ) :> obj
            ( fun (i : unativeint)  -> int64 i ) :> obj
            ( fun (i : unativeint)  -> uint8 i ) :> obj
            ( fun (i : unativeint)  -> uint16 i ) :> obj
            ( fun (i : unativeint)  -> uint32 i ) :> obj
            ( fun (i : unativeint)  -> uint64 i ) :> obj
            ( fun (i : unativeint)  -> float32 i ) :> obj
            ( fun (i : unativeint)  -> float i ) :> obj
        ]

    let private fractionalConversions =
        [
            ( fun (b : float32)        -> float32 b ) :> obj
            ( fun (b : float32)        -> float b ) :> obj

            ( fun (b : float)          -> float32 b ) :> obj
            ( fun (b : float)          -> float b ) :> obj

            ( fun (b : decimal)        -> float32 b ) :> obj
            ( fun (b : decimal)        -> float b ) :> obj
        ]

    let private booleanConversions =
        [
            ( fun (b : bool)        -> if b then 1uy else 0uy ) :> obj
            ( fun (b : bool)        -> if b then 1y else 0y ) :> obj
            ( fun (b : bool)        -> if b then 1us else 0us ) :> obj
            ( fun (b : bool)        -> if b then 1s else 0s ) :> obj
            ( fun (b : bool)        -> if b then 1u else 0u ) :> obj
            ( fun (b : bool)        -> if b then 1 else 0 ) :> obj
            ( fun (b : bool)        -> if b then 1UL else 0UL ) :> obj
            ( fun (b : bool)        -> if b then 1L else 0L ) :> obj

            ( fun (b : bool)        -> if b then 1.0f else 0.0f ) :> obj
            ( fun (b : bool)        -> if b then 1.0  else 0.0 ) :> obj
        ]


    let private vector2Conversions =
        [
            // V2i -> other vectors
            ( fun (b : V2i)        -> b ) :> obj
            ( fun (b : V2i)        -> V2l b ) :> obj
            ( fun (b : V2i)        -> V2f b ) :> obj
            ( fun (b : V2i)        -> V2d b ) :> obj
            ( fun (b : V2i)        -> V3i(b.X, b.Y, 0) ) :> obj
            ( fun (b : V2i)        -> V3l(int64 b.X, int64 b.Y,0L) ) :> obj
            ( fun (b : V2i)        -> V3f(float32 b.X, float32 b.Y, 0.0f) ) :> obj
            ( fun (b : V2i)        -> V3d(float b.X, float b.Y,0.0) ) :> obj
            ( fun (b : V2i)        -> V4i(b.X, b.Y, 0, 0) ) :> obj
            ( fun (b : V2i)        -> V4l(int64 b.X, int64 b.Y, 0L, 0L) ) :> obj
            ( fun (b : V2i)        -> V4f(float32 b.X, float32 b.Y, 0.0f, 0.0f) ) :> obj
            ( fun (b : V2i)        -> V4d(float b.X, float b.Y, 0.0, 0.0) ) :> obj

            // V2l -> other vectors
            ( fun (b : V2l)        -> V2i b ) :> obj
            ( fun (b : V2l)        -> b ) :> obj
            ( fun (b : V2l)        -> V2f b ) :> obj
            ( fun (b : V2l)        -> V2d b ) :> obj
            ( fun (b : V2l)        -> V3i(int b.X, int b.Y, 0) ) :> obj
            ( fun (b : V2l)        -> V3l(b.X, b.Y,0L) ) :> obj
            ( fun (b : V2l)        -> V3f(float32 b.X, float32 b.Y, 0.0f) ) :> obj
            ( fun (b : V2l)        -> V3d(float b.X, float b.Y,0.0) ) :> obj
            ( fun (b : V2l)        -> V4i(int b.X, int b.Y, 0, 0) ) :> obj
            ( fun (b : V2l)        -> V4l(b.X, b.Y, 0L, 0L) ) :> obj
            ( fun (b : V2l)        -> V4f(float32 b.X, float32 b.Y, 0.0f, 0.0f) ) :> obj
            ( fun (b : V2l)        -> V4d(float b.X, float b.Y, 0.0, 0.0) ) :> obj

            // V2f -> other vectors
            ( fun (b : V2f)        -> V2i b ) :> obj
            ( fun (b : V2f)        -> V2l b ) :> obj
            ( fun (b : V2f)        -> b ) :> obj
            ( fun (b : V2f)        -> V2d b ) :> obj
            ( fun (b : V2f)        -> V3i(int b.X, int b.Y, 0) ) :> obj
            ( fun (b : V2f)        -> V3l(int64 b.X, int64 b.Y,0L) ) :> obj
            ( fun (b : V2f)        -> V3f(b.X, b.Y, 0.0f) ) :> obj
            ( fun (b : V2f)        -> V3d(float b.X, float b.Y, 0.0) ) :> obj
            ( fun (b : V2f)        -> V4i(int b.X, int b.Y, 0, 0) ) :> obj
            ( fun (b : V2f)        -> V4l(int64 b.X, int64 b.Y, 0L, 0L) ) :> obj
            ( fun (b : V2f)        -> V4f(b.X, b.Y, 0.0f, 1.0f) ) :> obj
            ( fun (b : V2f)        -> V4d(float b.X, float b.Y, 0.0, 1.0) ) :> obj

            // V2d -> other vectors
            ( fun (b : V2d)        -> V2i b ) :> obj
            ( fun (b : V2d)        -> V2l b ) :> obj
            ( fun (b : V2d)        -> V2f b ) :> obj
            ( fun (b : V2d)        -> b ) :> obj
            ( fun (b : V2d)        -> V3i(int b.X, int b.Y, 0) ) :> obj
            ( fun (b : V2d)        -> V3l(int64 b.X, int64 b.Y,0L) ) :> obj
            ( fun (b : V2d)        -> V3f(float32 b.X, float32 b.Y, 0.0f) ) :> obj
            ( fun (b : V2d)        -> V3d(b.X, b.Y, 0.0) ) :> obj
            ( fun (b : V2d)        -> V4i(int b.X, int b.Y, 0, 0) ) :> obj
            ( fun (b : V2d)        -> V4l(int64 b.X, int64 b.Y, 0L, 0L) ) :> obj
            ( fun (b : V2d)        -> V4f(float32 b.X, float32 b.Y, 0.0f, 1.0f) ) :> obj
            ( fun (b : V2d)        -> V4d(b.X, b.Y, 0.0, 1.0) ) :> obj

        ]

    let private vector3Conversions =
        [
            // V3i -> other vectors
            ( fun (b : V3i)        -> b) :> obj
            ( fun (b : V3i)        -> V3l b) :> obj
            ( fun (b : V3i)        -> V3f b) :> obj
            ( fun (b : V3i)        -> V3d b) :> obj
            ( fun (b : V3i)        -> V4i(b.X, b.Y, b.Z, 0)) :> obj
            ( fun (b : V3i)        -> V4l(int64 b.X, int64 b.Y, int64 b.Z, 0L)) :> obj
            ( fun (b : V3i)        -> V4f(float32 b.X, float32 b.Y, float32 b.Z, 0.0f)) :> obj
            ( fun (b : V3i)        -> V4d(float b.X, float b.Y, float b.Z, 0.0)) :> obj

            // V3l -> other vectors
            ( fun (b : V3l)        -> V3i b) :> obj
            ( fun (b : V3l)        -> b) :> obj
            ( fun (b : V3l)        -> V3f b) :> obj
            ( fun (b : V3l)        -> V3d b) :> obj
            ( fun (b : V3l)        -> V4i(int b.X, int b.Y, int b.Z, 0)) :> obj
            ( fun (b : V3l)        -> V4l(b.X, b.Y, b.Z, 0L)) :> obj
            ( fun (b : V3l)        -> V4f(float32 b.X, float32 b.Y, float32 b.Z, 0.0f)) :> obj
            ( fun (b : V3l)        -> V4d(float b.X, float b.Y, float b.Z, 0.0)) :> obj

            // V3f -> other vectors
            ( fun (b : V3f)        -> V3i b) :> obj
            ( fun (b : V3f)        -> V3l b) :> obj
            ( fun (b : V3f)        -> b) :> obj
            ( fun (b : V3f)        -> V3d b) :> obj
            ( fun (b : V3f)        -> V4i(int b.X, int b.Y, int b.Z, 0)) :> obj
            ( fun (b : V3f)        -> V4l(int64 b.X, int64 b.Y, int64 b.Z, 0L)) :> obj
            ( fun (b : V3f)        -> V4f(b.X, b.Y, b.Z, 1.0f)) :> obj
            ( fun (b : V3f)        -> V4d(float b.X, float b.Y, float b.Z, 1.0)) :> obj

            // V3d -> other vectors
            ( fun (b : V3d)        -> V3i b) :> obj
            ( fun (b : V3d)        -> V3l b) :> obj
            ( fun (b : V3d)        -> V3f b) :> obj
            ( fun (b : V3d)        -> b) :> obj
            ( fun (b : V3d)        -> V4i(int b.X, int b.Y, int b.Z, 0)) :> obj
            ( fun (b : V3d)        -> V4l(int64 b.X, int64 b.Y, int64 b.Z, 0L)) :> obj
            ( fun (b : V3d)        -> V4f(float32 b.X, float32 b.Y, float32 b.Z, 1.0f)) :> obj
            ( fun (b : V3d)        -> V4d(b.X, b.Y, b.Z, 1.0)) :> obj

        ]

    let private vector4Conversions =
        [
            // V4i -> other vectors
            ( fun (b : V4i)        -> b) :> obj
            ( fun (b : V4i)        -> V4l b) :> obj
            ( fun (b : V4i)        -> V4f b) :> obj
            ( fun (b : V4i)        -> V4d b) :> obj

            // V4l -> other vectors
            ( fun (b : V4l)        -> V4i b) :> obj
            ( fun (b : V4l)        -> b) :> obj
            ( fun (b : V4l)        -> V4f b) :> obj
            ( fun (b : V4l)        -> V4d b) :> obj

            // V4f -> other vectors
            ( fun (b : V4f)        -> V4i b) :> obj
            ( fun (b : V4f)        -> V4l b) :> obj
            ( fun (b : V4f)        -> b) :> obj
            ( fun (b : V4f)        -> V4d b) :> obj

            // V4d -> other vectors
            ( fun (b : V4d)        -> V4i b) :> obj
            ( fun (b : V4d)        -> V4l b) :> obj
            ( fun (b : V4d)        -> V4f b) :> obj
            ( fun (b : V4d)        -> b) :> obj
        ]


    let private matrix22Conversions =
        [

            // M22i -> other matrices
            ( fun (v : M22i)        -> v ) :> obj
            ( fun (v : M22i)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M22i)        -> M44d.op_Explicit v ) :> obj


            // M22l -> other matrices
            ( fun (v : M22l)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> v ) :> obj
            ( fun (v : M22l)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M22l)        -> M44d.op_Explicit v ) :> obj

            // M22f -> other matrices
            ( fun (v : M22f)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> v ) :> obj
            ( fun (v : M22f)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M22f)        -> M44d.op_Explicit v ) :> obj

            // M22d -> other matrices
            ( fun (v : M22d)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> v ) :> obj
            ( fun (v : M22d)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M22d)        -> M44d.op_Explicit v ) :> obj
        ]

    let private matrix23Conversions =
        [

            // M23i -> other matrices
            ( fun (v : M23i)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> v ) :> obj
            ( fun (v : M23i)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M23i)        -> M44d.op_Explicit v ) :> obj


            // M23l -> other matrices
            ( fun (v : M23l)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> v ) :> obj
            ( fun (v : M23l)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M23l)        -> M44d.op_Explicit v ) :> obj

            // M23f -> other matrices
            ( fun (v : M23f)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> v ) :> obj
            ( fun (v : M23f)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M23f)        -> M44d.op_Explicit v ) :> obj

            // M23d -> other matrices
            ( fun (v : M23d)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> v ) :> obj
            ( fun (v : M23d)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M23d)        -> M44d.op_Explicit v ) :> obj
        ]

    let private matrix33Conversions =
        [

            // M33i -> other matrices
            ( fun (v : M33i)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> v ) :> obj
            ( fun (v : M33i)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M33i)        -> M44d.op_Explicit v ) :> obj


            // M33l -> other matrices
            ( fun (v : M33l)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> v ) :> obj
            ( fun (v : M33l)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M33l)        -> M44d.op_Explicit v ) :> obj

            // M33f -> other matrices
            ( fun (v : M33f)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> v ) :> obj        
            ( fun (v : M33f)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M33f)        -> M44d.op_Explicit v ) :> obj

            // M33d -> other matrices
            ( fun (v : M33d)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> v ) :> obj    
            ( fun (v : M33d)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M33d)        -> M44d.op_Explicit v ) :> obj
        ]

    let private matrix34Conversions =
        [

            // M34i -> other matrices
            ( fun (v : M34i)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> v ) :> obj
            ( fun (v : M34i)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M34i)        -> M44d.op_Explicit v ) :> obj


            // M34l -> other matrices
            ( fun (v : M34l)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> v ) :> obj
            ( fun (v : M34l)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M34l)        -> M44d.op_Explicit v ) :> obj

            // M34f -> other matrices
            ( fun (v : M34f)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> v ) :> obj
            ( fun (v : M34f)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M34f)        -> M44d.op_Explicit v ) :> obj

            // M34d -> other matrices
            ( fun (v : M34d)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> v ) :> obj
            ( fun (v : M34d)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M34d)        -> M44d.op_Explicit v ) :> obj
        ]

    let private matrix44Conversions =
        [

            // M44i -> other matrices
            ( fun (v : M44i)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> v ) :> obj
            ( fun (v : M44i)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M44i)        -> M44d.op_Explicit v ) :> obj


            // M44l -> other matrices
            ( fun (v : M44l)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> v ) :> obj
            ( fun (v : M44l)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M44l)        -> M44d.op_Explicit v ) :> obj

            // M44f -> other matrices
            ( fun (v : M44f)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M44f)        -> v ) :> obj
            ( fun (v : M44f)        -> M44d.op_Explicit v ) :> obj

            // M44d -> other matrices
            ( fun (v : M44d)        -> M22i.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M22l.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M22f.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M22d.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M23i.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M23l.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M23f.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M23d.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M33i.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M33l.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M33f.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M33d.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M34i.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M34l.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M34f.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M34d.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M44i.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M44l.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> M44f.op_Explicit v ) :> obj
            ( fun (v : M44d)        -> v ) :> obj
        ]

    
    let private colorConversions =
        [
            ( fun (b : C3b)        -> V3i(int b.R, int b.G, int b.B) ) :> obj
            ( fun (b : C3b)        -> C3ui(uint32 b.R, uint32 b.G, uint32 b.B) ) :> obj
            ( fun (b : C3b)        -> V3l(int64 b.R, int64 b.G, int64 b.B) ) :> obj
            ( fun (b : C3b)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C3b)        -> C3f(b).ToV3d() ) :> obj
            ( fun (b : C3b)        -> V4i(int b.R, int b.G, int b.B, 255) ) :> obj
            ( fun (b : C3b)        -> C4ui(uint32 b.R, uint32 b.G, uint32 b.B, 255u) ) :> obj
            ( fun (b : C3b)        -> V4l(int64 b.R, int64 b.G, int64 b.B, 255L) ) :> obj
            ( fun (b : C3b)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C3b)        -> C4f(b).ToV4d() ) :> obj

            ( fun (b : C4b)        -> V3i(int b.R, int b.G, int b.B) ) :> obj
            ( fun (b : C4b)        -> C3ui(uint32 b.R, uint32 b.G, uint32 b.B) ) :> obj
            ( fun (b : C4b)        -> V3l(int64 b.R, int64 b.G, int64 b.B) ) :> obj
            ( fun (b : C4b)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C4b)        -> C3f(b).ToV3d() ) :> obj
            ( fun (b : C4b)        -> V4i(int b.R, int b.G, int b.B, int b.A) ) :> obj
            ( fun (b : C4b)        -> C4ui(uint32 b.R, uint32 b.G, uint32 b.B, uint32 b.A) ) :> obj
            ( fun (b : C4b)        -> V4l(int64 b.R, int64 b.G, int64 b.B, int64 b.A) ) :> obj
            ( fun (b : C4b)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C4b)        -> C4f(b).ToV4d() ) :> obj

            ( fun (b : C3us)        -> V3i(int b.R, int b.G, int b.B) ) :> obj
            ( fun (b : C3us)        -> C3ui(uint32 b.R, uint32 b.G, uint32 b.B) ) :> obj
            ( fun (b : C3us)        -> V3l(int64 b.R, int64 b.G, int64 b.B) ) :> obj
            ( fun (b : C3us)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C3us)        -> C3f(b).ToV3d() ) :> obj
            ( fun (b : C3us)        -> V4i(int b.R, int b.G, int b.B, 255) ) :> obj
            ( fun (b : C3us)        -> C4ui(uint32 b.R, uint32 b.G, uint32 b.B, 255u) ) :> obj
            ( fun (b : C3us)        -> V4l(int64 b.R, int64 b.G, int64 b.B, 255L) ) :> obj
            ( fun (b : C3us)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C3us)        -> C4f(b).ToV4d() ) :> obj

            ( fun (b : C4us)        -> V3i(int b.R, int b.G, int b.B) ) :> obj
            ( fun (b : C4us)        -> C3ui(uint32 b.R, uint32 b.G, uint32 b.B) ) :> obj
            ( fun (b : C4us)        -> V3l(int64 b.R, int64 b.G, int64 b.B) ) :> obj
            ( fun (b : C4us)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C4us)        -> C3f(b).ToV3d() ) :> obj
            ( fun (b : C4us)        -> V4i(int b.R, int b.G, int b.B, int b.A) ) :> obj
            ( fun (b : C4us)        -> C4ui(uint32 b.R, uint32 b.G, uint32 b.B, uint32 b.A) ) :> obj
            ( fun (b : C4us)        -> V4l(int64 b.R, int64 b.G, int64 b.B, int64 b.A) ) :> obj
            ( fun (b : C4us)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C4us)        -> C4f(b).ToV4d() ) :> obj


            ( fun (b : C3ui)        -> V3i(int b.R, int b.G, int b.B) ) :> obj
            ( fun (b : C3ui)        -> b ) :> obj
            ( fun (b : C3ui)        -> V3l(int64 b.R, int64 b.G, int64 b.B) ) :> obj
            ( fun (b : C3ui)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C3ui)        -> C3f(b).ToV3d() ) :> obj
            ( fun (b : C3ui)        -> V4i(int b.R, int b.G, int b.B, 255) ) :> obj
            ( fun (b : C3ui)        -> C4ui(uint32 b.R, uint32 b.G, uint32 b.B, 255u) ) :> obj
            ( fun (b : C3ui)        -> V4l(int64 b.R, int64 b.G, int64 b.B, 255L) ) :> obj
            ( fun (b : C3ui)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C3ui)        -> C4f(b).ToV4d() ) :> obj

            ( fun (b : C4ui)        -> V3i(int b.R, int b.G, int b.B) ) :> obj
            ( fun (b : C4ui)        -> C3ui(uint32 b.R, uint32 b.G, uint32 b.B) ) :> obj
            ( fun (b : C4ui)        -> V3l(int64 b.R, int64 b.G, int64 b.B) ) :> obj
            ( fun (b : C4ui)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C4ui)        -> C3f(b).ToV3d() ) :> obj
            ( fun (b : C4ui)        -> V4i(int b.R, int b.G, int b.B, int b.A) ) :> obj
            ( fun (b : C4ui)        -> b ) :> obj
            ( fun (b : C4ui)        -> V4l(int64 b.R, int64 b.G, int64 b.B, int64 b.A) ) :> obj
            ( fun (b : C4ui)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C4ui)        -> C4f(b).ToV4d() ) :> obj


            ( fun (b : C3f)        -> C3b(b).ToV3i() ) :> obj
            ( fun (b : C3f)        -> C3ui(b) ) :> obj
            ( fun (b : C3f)        -> C3ui(b).ToV3l() ) :> obj
            ( fun (b : C3f)        -> b.ToV3f() ) :> obj
            ( fun (b : C3f)        -> b.ToV3d() ) :> obj
            ( fun (b : C3f)        -> C4b(b).ToV4i() ) :> obj
            ( fun (b : C3f)        -> C4ui(b) ) :> obj
            ( fun (b : C3f)        -> C4ui(b).ToV4l() ) :> obj
            ( fun (b : C3f)        -> b.ToV4f() ) :> obj
            ( fun (b : C3f)        -> b.ToV4d() ) :> obj

            ( fun (b : C4f)        -> C3b(b).ToV3i() ) :> obj
            ( fun (b : C4f)        -> C3ui(b) ) :> obj
            ( fun (b : C4f)        -> C3ui(b).ToV3l() ) :> obj
            ( fun (b : C4f)        -> b.ToV3f() ) :> obj
            ( fun (b : C4f)        -> b.ToV3d() ) :> obj
            ( fun (b : C4f)        -> C4b(b).ToV4i() ) :> obj
            ( fun (b : C4f)        -> C4ui(b) ) :> obj
            ( fun (b : C4f)        -> C4ui(b).ToV4l() ) :> obj
            ( fun (b : C4f)        -> b.ToV4f() ) :> obj
            ( fun (b : C4f)        -> b.ToV4d() ) :> obj


            ( fun (b : C3d)        -> C3b(b).ToV3i() ) :> obj
            ( fun (b : C3d)        -> C3ui(b) ) :> obj
            ( fun (b : C3d)        -> C3ui(b).ToV3l() ) :> obj
            ( fun (b : C3d)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C3d)        -> b.ToV3d() ) :> obj
            ( fun (b : C3d)        -> C4b(b).ToV4i() ) :> obj
            ( fun (b : C3d)        -> C4ui(b) ) :> obj
            ( fun (b : C3d)        -> C4ui(b).ToV4l() ) :> obj
            ( fun (b : C3d)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C3d)        -> b.ToV4d() ) :> obj

            ( fun (b : C4d)        -> C3b(b).ToV3i() ) :> obj
            ( fun (b : C4d)        -> C3ui(b) ) :> obj
            ( fun (b : C4d)        -> C3ui(b).ToV3l() ) :> obj
            ( fun (b : C4d)        -> C3f(b).ToV3f() ) :> obj
            ( fun (b : C4d)        -> b.ToV3d() ) :> obj
            ( fun (b : C4d)        -> C4b(b).ToV4i() ) :> obj
            ( fun (b : C4d)        -> C4ui(b) ) :> obj
            ( fun (b : C4d)        -> C4ui(b).ToV4l() ) :> obj
            ( fun (b : C4d)        -> C4f(b).ToV4f() ) :> obj
            ( fun (b : C4d)        -> b.ToV4d() ) :> obj


        ]


    let private specialConversions =
        [
            ( fun (t : Trafo3d)     -> trafoToM44f t ) :> obj
        ]

    let private allConversions =
        List.concat [
            integralConversions
            booleanConversions
            fractionalConversions

            vector2Conversions
            vector3Conversions
            vector4Conversions

            matrix22Conversions
            matrix23Conversions
            matrix33Conversions
            matrix34Conversions
            matrix44Conversions

            colorConversions
            specialConversions
        ]


    let private transposeFunctions =
        [
            ( fun (m : M22d) -> m.Transposed ) :> obj
            ( fun (m : M22f) -> m.Transposed ) :> obj
            ( fun (m : M22i) -> m.Transposed ) :> obj
            ( fun (m : M22l) -> m.Transposed ) :> obj

            ( fun (m : M33d) -> m.Transposed ) :> obj
            ( fun (m : M33f) -> m.Transposed ) :> obj
            ( fun (m : M33i) -> m.Transposed ) :> obj
            ( fun (m : M33l) -> m.Transposed ) :> obj

            ( fun (m : M23d) -> M23d(m.M00, m.M10, m.M02, m.M01, m.M11, m.M12 ) ) :> obj
            ( fun (m : M23f) -> M23f(m.M00, m.M10, m.M02, m.M01, m.M11, m.M12 ) ) :> obj
            ( fun (m : M23i) -> M23i(m.M00, m.M10, m.M02, m.M01, m.M11, m.M12 ) ) :> obj
            ( fun (m : M23l) -> M23l(m.M00, m.M10, m.M02, m.M01, m.M11, m.M12 ) ) :> obj


            ( fun (m : M44d) -> m.Transposed ) :> obj
            ( fun (m : M44f) -> m.Transposed ) :> obj
            ( fun (m : M44i) -> m.Transposed ) :> obj
            ( fun (m : M44l) -> m.Transposed ) :> obj
            
            ( fun (m : M34d) -> M34d(m.M00, m.M10, m.M20, m.M03, m.M01, m.M11, m.M21, m.M13, m.M02, m.M12, m.M22, m.M23) ) :> obj
            ( fun (m : M34f) -> M34f(m.M00, m.M10, m.M20, m.M03, m.M01, m.M11, m.M21, m.M13, m.M02, m.M12, m.M22, m.M23) ) :> obj
            ( fun (m : M34i) -> M34i(m.M00, m.M10, m.M20, m.M03, m.M01, m.M11, m.M21, m.M13, m.M02, m.M12, m.M22, m.M23) ) :> obj
            ( fun (m : M34l) -> M34l(m.M00, m.M10, m.M20, m.M03, m.M01, m.M11, m.M21, m.M13, m.M02, m.M12, m.M22, m.M23) ) :> obj
        ]


    type private ConversionMapping<'a>() =
        let store = Dict<Type, Dict<Type, 'a>>()

        member x.Add(input : Type, output : Type, e : 'a) =
            let map = store.GetOrCreate(input, fun _ -> Dict())
            match map.TryGetValue output with
                | (true, old) ->
                    Log.warn "conflicting uniform conversions for: %s -> %s" input.Name output.Name
                | _ ->
                    ()

            map.[output] <- e

        member x.TryGet(input : Type, output : Type, [<Out>] e : byref<'a>) =
            match store.TryGetValue input with
                | (true, m) ->
                    m.TryGetValue(output, &e)
                | _ ->
                    false


    let private createCompiledMap (l : list<obj>) =
        let result = ConversionMapping()

        for e in l do
            let (i,o) = FSharpType.GetFunctionElements (e.GetType())
            result.Add(i,o,e)

        result

    let private mapping = createCompiledMap allConversions

    let private transposeMapping =
        let dict = Dictionary<Type, obj>()
        for f in transposeFunctions do
            let t = f.GetType()
            let (arg, ret) = FSharpType.GetFunctionElements t
            dict.[arg] <- f
        dict


    [<AutoOpen>]
    module private Lambdas =
        type UnboxLambda<'a> private() =
            static let instance = fun (v : obj) -> unbox<'a> v
            static member Instance = instance
            
        type IdLambda<'a> private() =
            static let instance = fun (v : 'a) -> v
            static member Instance = instance
               
        type ComposedLambda<'a, 'b, 'c> private() =
            static member Create(f : 'a -> 'b, g : 'b -> 'c) =
                f >> g
         
        type UntypedArrayMapLambda<'a, 'b> private() =
            static member Create(converter : 'a -> 'b) =
                fun (arr : Array) -> arr |> unbox |> Array.map converter :> Array
                   
        type ArrayMapLambda<'a, 'b> private() =
            static member Create(converter : 'a -> 'b) =
                fun (arr : Array) -> arr |> unbox |> Array.map converter
                     
        type UnboxTyped<'e, 'b>() =
            static let conv = fun (e : 'e) -> e |> unbox<'b>
            static member Instance = conv


        let compose (f : obj) (g : obj) =
            let ft = f.GetType()
            let gt = g.GetType()
            let (a,b) = FSharpType.GetFunctionElements ft
            let (b',c) = FSharpType.GetFunctionElements gt
            if b <> b' then failwith "[PrimitiveValueConverter] cannot compose functions"
            let t = typedefof<ComposedLambda<_,_,_>>.MakeGenericType [| a; b; c |]
            let mi = 
                t.GetMethod(
                    "Create",
                    BindingFlags.NonPublic ||| BindingFlags.Static ||| BindingFlags.Public, 
                    Type.DefaultBinder, 
                    [| ft; gt|], 
                    null
                )

            mi.Invoke(null, [| f; g |])

        let withTranspose (f : obj) =
            let ft = f.GetType()
            let (_,r) = FSharpType.GetFunctionElements ft
            match transposeMapping.TryGetValue r with
                | (true, transpose) -> compose f transpose
                | _ -> f

        let idFunction (t : Type) =
            let t = typedefof<IdLambda<_>>.MakeGenericType [| t |]
            t.GetProperty("Instance").GetValue(null)

        let unboxFunction (t : Type) =
            let t = typedefof<UnboxLambda<_>>.MakeGenericType [| t |]
            t.GetProperty("Instance").GetValue(null)

    let rec tryGetConverter (inType : Type) (outType : Type) =
        lock mapping (fun () ->
            match mapping.TryGet(inType, outType) with
                | (true, conv) ->
                    Some conv
                | _ ->
                    if outType.IsArray && inType.IsArray then
                        let inType' = inType.GetElementType()
                        let outType' = outType.GetElementType()
                        match tryGetConverter inType' outType' with
                            | Some innerConv ->
                                let tconv = typedefof<ArrayMapLambda<_,_>>.MakeGenericType [| inType'; outType' |] 
                                
                                
                                let mi = 
                                    tconv.GetMethod(
                                        "Create",
                                        BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Static, 
                                        Type.DefaultBinder, 
                                        [| innerConv.GetType() |],
                                        null
                                    )

                                let conv = mi.Invoke(null, [|innerConv|])
                                mapping.Add(inType, outType, conv)
                                Some conv

                            | None ->
                                None
                    elif inType.IsEnum then
                        let valueType = Enum.GetUnderlyingType(inType)
                        let toValue =
                            let value = typedefof<UnboxTyped<_,_>>.MakeGenericType [| inType; valueType |]
                            let prop = value.GetProperty("Instance", BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Static)
                            prop.GetValue(null)

                        match tryGetConverter valueType outType with
                            | Some inner -> 
                                Some <| compose toValue inner
                            | None ->
                                None
                    else
                        None
            )

    let rec getConverter (inType : Type) (outType : Type) =
        match tryGetConverter inType outType with
            | Some c -> c
            | None -> failwithf "unknown conversion from %A to %A" inType.FullName outType.FullName

    open Aardvark.Base.IL
    type private Invoker private() =
        static let cache = System.Collections.Concurrent.ConcurrentDictionary<Type * Type, obj -> obj -> obj>()

        static let get (inType : Type) (outType : Type) =
            cache.GetOrAdd((inType, outType), Func<_,_>(fun (inType, outType) ->
                let tfun = FSharpType.MakeFunctionType(inType, outType)
                let invoke = tfun.GetMethod("Invoke")

                fun (f : obj) (arg : obj) ->
                    invoke.Invoke(f, [| arg |])

                //cil {
                //    do! IL.ldarg 0
                //    do! IL.ldarg 1
                //    if inType.IsValueType then 
                //        do! IL.unbox inType
                //    do! IL.call invoke
                //    if outType.IsValueType then
                //        do! IL.box outType
                //    do! IL.ret
                //}
            ))

        static member Invoke(inType : Type, outType : Type, f : obj, value : obj) =
            let invoke = get inType outType
            invoke f value


    let convert (outType : Type) (value : obj) =
        if isNull value then
            if outType.IsValueType then failwithf "cannot convert null to %A" outType
            null
        else
            let inType = value.GetType()
            let conv = getConverter inType outType
            Invoker.Invoke(inType, outType, conv, value)


    type private ArrayConverterCache<'a>() =
        static let conv = Dict<Type, Array -> 'a[]>()
        static let t = typedefof<ArrayMapLambda<_,_>>

        static member Get (inputType : Type) =
            lock conv (fun () ->
                conv.GetOrCreate(inputType, Func<Type, Array -> 'a[]>(fun inputType ->
                    if inputType = typeof<'a> then
                        unbox
                    else
                        let t = t.MakeGenericType [| inputType; typeof<'a> |]
                        let mi = t.GetMethod("Create", BindingFlags.NonPublic ||| BindingFlags.Static ||| BindingFlags.Public, Type.DefaultBinder, [| FSharpType.MakeFunctionType(inputType, typeof<'a>) |], null)
                        let conv = getConverter inputType typeof<'a>
                        mi.Invoke(null, [|conv|]) |> unbox<Array -> 'a[]>
                ) )
            )

    type private ArrayConverterCache() =
        static let conv = Dict<Type * Type, Array -> Array>()
        static let t = typedefof<UntypedArrayMapLambda<_,_>>

        static member Get (inputType : Type, outputType : Type) =
            lock conv (fun () ->
                let key = (inputType, outputType)
                conv.GetOrCreate(key, Func<Type * Type, Array -> Array>(fun (inputType, outputType) ->
                    if inputType = outputType then
                        id
                    else
                        let t = t.MakeGenericType [| inputType; outputType |]
                        let mi = t.GetMethod("Create", BindingFlags.NonPublic ||| BindingFlags.Static ||| BindingFlags.Public, Type.DefaultBinder, [| FSharpType.MakeFunctionType(inputType, outputType) |], null)
                        let conv = getConverter inputType outputType
                        mi.Invoke(null, [|conv|]) |> unbox<Array -> Array>
                ) )
            )

    let getArrayConverter (inputType : Type) (outputType : Type) : Array -> Array =
        ArrayConverterCache.Get(inputType, outputType)

    let private uniformCache = Dict<bool * Type * Type, obj>()
    let getUniformConverter (transpose : bool) (inType : Type) (outType : Type) =
        let key = (transpose, inType, outType)
        lock uniformCache (fun () ->
            uniformCache.GetOrCreate(key, fun (rowMajor, inType, outType) ->
                if transpose then getConverter inType outType |> withTranspose
                else getConverter inType outType
            )
        )

    let private idFunctions = Dict<Type, obj>()
    let getIdentityConverter (t : Type) =
        lock idFunctions (fun () ->
            idFunctions.GetOrCreate(t, fun t -> idFunction t)
        )

    let getTransposeConverter (t : Type) =
        match transposeMapping.TryGetValue t with
            | (true, v) -> v
            | _ -> getIdentityConverter t

    let isTransposable (t : Type) =
        transposeMapping.ContainsKey t



    let converter<'a, 'b> : 'a -> 'b =
        if typeof<'a> = typeof<'b> then
            let f = id : 'a -> 'a
            f |> unbox
        else
            getConverter typeof<'a> typeof<'b> |> unbox

    let arrayConverter<'a> (inputType : Type) : Array -> 'a[] =
        if inputType = typeof<'a> then unbox
        else ArrayConverterCache<'a>.Get(inputType)

    let uniformConverter<'a, 'b> (transpose : bool) : 'a -> 'b =
        if not transpose && typeof<'a> = typeof<'b> then
            let f = id : 'a -> 'a
            f |> unbox
        else
            getUniformConverter transpose typeof<'a> typeof<'b> |> unbox

    let transpose<'a> : 'a -> 'a =
        getTransposeConverter typeof<'a> |> unbox

    let transposable<'a> = isTransposable typeof<'a>

    [<AbstractClass>]
    type private AdaptiveConverter<'a>() =
        abstract member Convert : IAdaptiveValue -> aval<'a>
        abstract member Convert : aval<Array> -> aval<'a[]>

    type private AdaptiveConverter<'a, 'b> private() =
        inherit AdaptiveConverter<'b>()
        static let conv = converter<'a, 'b>
        static let instance = AdaptiveConverter<'a, 'b>()

        static member Instance = instance

        override x.Convert (m : IAdaptiveValue) =
            m |> unbox<aval<'a>> |> AVal.map conv

        override x.Convert (m : aval<Array>) =
            m |> AVal.map (fun arr -> arr |> unbox<'a[]> |> Array.map conv)

    let private staticFieldCache = Dict<Type * string, obj>()
    let private getStaticField (name : string) (t : Type) : 'a =
        staticFieldCache.GetOrCreate((t, name), fun (t, name) ->
            let p = t.GetProperty(name, BindingFlags.Static ||| BindingFlags.NonPublic ||| BindingFlags.Public)
            p.GetValue(null)
        ) |> unbox<'a>

    let convertArray (inputElementType : Type) (m : aval<Array>) : aval<'a[]> =
        if inputElementType = typeof<'a> then
            m |> AVal.map (fun a -> unbox a)
        else
            let tconv = typedefof<AdaptiveConverter<_,_>>.MakeGenericType [| inputElementType; typeof<'a> |]
            let converter : AdaptiveConverter<'a> = tconv |> getStaticField "Instance"
            converter.Convert(m)

    let convertValue (m : IAdaptiveValue) : aval<'a> =
        let inputType = m.ContentType
        if inputType = typeof<'a> then
            unbox m
        else
            let tconv = typedefof<AdaptiveConverter<_,_>>.MakeGenericType [| inputType; typeof<'a> |]
            let converter : AdaptiveConverter<'a> = tconv |> getStaticField "Instance"
            converter.Convert(m)

