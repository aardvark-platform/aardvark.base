/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
namespace Aardvark.Base.Coder;

// AUTO GENERATED CODE - DO NOT CHANGE!

public partial class StreamCodeReader
{

    #region Vectors

    public V2i ReadV2i()
        => new(ReadInt32(), ReadInt32());

    public V2ui ReadV2ui()
        => new(ReadUInt32(), ReadUInt32());

    public V2l ReadV2l()
        => new(ReadInt64(), ReadInt64());

    public V2f ReadV2f()
        => new(ReadSingle(), ReadSingle());

    public V2d ReadV2d()
        => new(ReadDouble(), ReadDouble());

    public V3i ReadV3i()
        => new(ReadInt32(), ReadInt32(), ReadInt32());

    public V3ui ReadV3ui()
        => new(ReadUInt32(), ReadUInt32(), ReadUInt32());

    public V3l ReadV3l()
        => new(ReadInt64(), ReadInt64(), ReadInt64());

    public V3f ReadV3f()
        => new(ReadSingle(), ReadSingle(), ReadSingle());

    public V3d ReadV3d()
        => new(ReadDouble(), ReadDouble(), ReadDouble());

    public V4i ReadV4i()
        => new(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());

    public V4ui ReadV4ui()
        => new(ReadUInt32(), ReadUInt32(), ReadUInt32(), ReadUInt32());

    public V4l ReadV4l()
        => new(ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64());

    public V4f ReadV4f()
        => new(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

    public V4d ReadV4d()
        => new(ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble());

    #endregion

    #region Matrices

    public M22i ReadM22i()
        => new(
            ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32()
            );

    public M22l ReadM22l()
        => new(
            ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64()
            );

    public M22f ReadM22f()
        => new(
            ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle()
            );

    public M22d ReadM22d()
        => new(
            ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble()
            );

    public M23i ReadM23i()
        => new(
            ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32()
            );

    public M23l ReadM23l()
        => new(
            ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64()
            );

    public M23f ReadM23f()
        => new(
            ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle()
            );

    public M23d ReadM23d()
        => new(
            ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble()
            );

    public M33i ReadM33i()
        => new(
            ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32()
            );

    public M33l ReadM33l()
        => new(
            ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64()
            );

    public M33f ReadM33f()
        => new(
            ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle()
            );

    public M33d ReadM33d()
        => new(
            ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble()
            );

    public M34i ReadM34i()
        => new(
            ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32()
            );

    public M34l ReadM34l()
        => new(
            ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64()
            );

    public M34f ReadM34f()
        => new(
            ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle()
            );

    public M34d ReadM34d()
        => new(
            ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble()
            );

    public M44i ReadM44i()
        => new(
            ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32(), 
            ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32()
            );

    public M44l ReadM44l()
        => new(
            ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64(), 
            ReadInt64(), ReadInt64(), ReadInt64(), ReadInt64()
            );

    public M44f ReadM44f()
        => new(
            ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), 
            ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle()
            );

    public M44d ReadM44d()
        => new(
            ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), 
            ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble()
            );

    #endregion

    #region Ranges and Boxes

    public Range1b ReadRange1b()
        => new(ReadByte(), ReadByte());

    public Range1sb ReadRange1sb()
        => new(ReadSByte(), ReadSByte());

    public Range1s ReadRange1s()
        => new(ReadInt16(), ReadInt16());

    public Range1us ReadRange1us()
        => new(ReadUInt16(), ReadUInt16());

    public Range1i ReadRange1i()
        => new(ReadInt32(), ReadInt32());

    public Range1ui ReadRange1ui()
        => new(ReadUInt32(), ReadUInt32());

    public Range1l ReadRange1l()
        => new(ReadInt64(), ReadInt64());

    public Range1ul ReadRange1ul()
        => new(ReadUInt64(), ReadUInt64());

    public Range1f ReadRange1f()
        => new(ReadSingle(), ReadSingle());

    public Range1d ReadRange1d()
        => new(ReadDouble(), ReadDouble());

    public Box2i ReadBox2i()
        => new(ReadV2i(), ReadV2i());

    public Box2l ReadBox2l()
        => new(ReadV2l(), ReadV2l());

    public Box2f ReadBox2f()
        => new(ReadV2f(), ReadV2f());

    public Box2d ReadBox2d()
        => new(ReadV2d(), ReadV2d());

    public Box3i ReadBox3i()
        => new(ReadV3i(), ReadV3i());

    public Box3l ReadBox3l()
        => new(ReadV3l(), ReadV3l());

    public Box3f ReadBox3f()
        => new(ReadV3f(), ReadV3f());

    public Box3d ReadBox3d()
        => new(ReadV3d(), ReadV3d());

    #endregion

    #region Colors

    public C3b ReadC3b()
        => new(ReadByte(), ReadByte(), ReadByte());

    public C3us ReadC3us()
        => new(ReadUInt16(), ReadUInt16(), ReadUInt16());

    public C3ui ReadC3ui()
        => new(ReadUInt32(), ReadUInt32(), ReadUInt32());

    public C3f ReadC3f()
        => new(ReadSingle(), ReadSingle(), ReadSingle());

    public C3d ReadC3d()
        => new(ReadDouble(), ReadDouble(), ReadDouble());

    public C4b ReadC4b()
        => new(ReadByte(), ReadByte(), ReadByte(), ReadByte());

    public C4us ReadC4us()
        => new(ReadUInt16(), ReadUInt16(), ReadUInt16(), ReadUInt16());

    public C4ui ReadC4ui()
        => new(ReadUInt32(), ReadUInt32(), ReadUInt32(), ReadUInt32());

    public C4f ReadC4f()
        => new(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

    public C4d ReadC4d()
        => new(ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble());

    #endregion

}
