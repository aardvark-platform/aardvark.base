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
using System;
using System.IO;
using System.Text;

namespace Aardvark.Base.Coder;

/// <summary>
/// Wrapper for class BinaryReader.
/// The following methods are overriden in order to
/// return values stored in network byte order (big-endian)
/// in host byte order:
/// 
/// short ReadInt16()
/// ushort ReadUInt16()
/// int ReadInt32()
/// uint ReadUInt32()
/// long ReadInt64()
/// ulong ReadUInt64()
/// float ReadSingle()
/// double ReadDouble()
/// 
/// Additional methods have been added to read
/// vector and color values, e.g. V3f, C4f, etc.
/// </summary>
public class NetworkOrderBinaryReader : BinaryReader
{

    #region Constructors

    public NetworkOrderBinaryReader(Stream input)
        : base(input, Encoding.UTF8)
    {
    }

    public NetworkOrderBinaryReader(Stream input, Encoding encoding)
        : base(input, encoding)
    {
        if (encoding == Encoding.Unicode ||
            encoding == Encoding.UTF32)
        {
            throw new Exception(
                "Encoding '" + encoding.EncodingName +
                "' uses little-endian byte order, which " +
                "makes no sense for a network (big-endian) " +
                "binary reader."
                );
        }
    }

    #endregion

    #region Read Overrides

    public override short ReadInt16()
    {
        return (short)((base.ReadByte() << 8) | base.ReadByte());
    }

    public override ushort ReadUInt16()
    {
        return (ushort)((base.ReadByte() << 8) | base.ReadByte());
    }

    public override int ReadInt32()
    {
        return (int)((base.ReadByte() << 24) | (base.ReadByte() << 16)
                        | (base.ReadByte() << 8) | base.ReadByte());
    }

    public override uint ReadUInt32()
    {
        return (uint)((base.ReadByte() << 24) | (base.ReadByte() << 16)
                        | (base.ReadByte() << 8) | base.ReadByte());
    }

    public override long ReadInt64()
    {
        return (long)(((long)base.ReadByte() << 56)
                     | ((long)base.ReadByte() << 48)
                     | ((long)base.ReadByte() << 40)
                     | ((long)base.ReadByte() << 32)
                     | ((long)base.ReadByte() << 24)
                     | ((long)base.ReadByte() << 16)
                     | ((long)base.ReadByte() << 8)
                     | base.ReadByte());
    }

    public override ulong ReadUInt64()
    {
        return (ulong)(((long)base.ReadByte() << 56)
                     | ((long)base.ReadByte() << 48)
                     | ((long)base.ReadByte() << 40)
                     | ((long)base.ReadByte() << 32)
                     | ((long)base.ReadByte() << 24)
                     | ((long)base.ReadByte() << 16)
                     | ((long)base.ReadByte() << 8)
                     | base.ReadByte());
    }

    public override float ReadSingle()
    {
        if (BitConverter.IsLittleEndian)
        {
            byte[] data = new byte[4];
            for (int i = 3; i >= 0; i--) data[i] = base.ReadByte();
            return BitConverter.ToSingle(data, 0);
        }
        else
        {
            return base.ReadSingle();
        }
    }

    public override double ReadDouble()
    {
        if (BitConverter.IsLittleEndian)
        {
            byte[] data = new byte[8];
            for (int i = 7; i >= 0; i--) data[i] = base.ReadByte();
            return BitConverter.ToDouble(data, 0);
        }
        else
        {
            return base.ReadDouble();
        }
    }

    #endregion

    #region Read V2f

    public V2f ReadV2f()
    {
        return new V2f(ReadSingle(), ReadSingle());
    }

    public V2f ReadV2fFrom2SignedInt8()
    {
        return new V2f(ReadSByte(), ReadSByte());
    }

    public V2f ReadV2fFrom2Int16()
    {
        return new V2f(ReadInt16(), ReadInt16());
    }

    public V2f ReadV2fFrom2Int32()
    {
        return new V2f(ReadInt32(), ReadInt32());
    }

    #endregion

    #region Read V3f

    public V3f ReadV3f()
    {
        return new V3f(ReadSingle(), ReadSingle(), ReadSingle());
    }

    public V3f ReadV3fFrom3SignedInt8()
    {
        return new V3f(ReadSByte(), ReadSByte(), ReadSByte());
    }

    public V3f ReadV3fFrom3Int16()
    {
        return new V3f(ReadInt16(), ReadInt16(), ReadInt16());
    }

    public V3f ReadV3fFrom3Int32()
    {
        return new V3f(ReadInt32(), ReadInt32(), ReadInt32());
    }

    #endregion

    #region Read C3f

    public C3f ReadC3f()
    {
        return new C3f(ReadSingle(), ReadSingle(), ReadSingle());
    }

    public C3f ReadC3fFrom3SignedInt8()
    {
        return new C3f(ReadSByte(), ReadSByte(), ReadSByte());
    }

    public C3f ReadC3fFrom3Int16()
    {
        return new C3f(ReadInt16(), ReadInt16(), ReadInt16());
    }

    public C3f ReadC3fFrom3Int32()
    {
        return new C3f(ReadInt32(), ReadInt32(), ReadInt32());
    }

    #endregion

    #region Read C4f

    public C4f ReadC4f()
    {
        return new C4f(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
    }

    public C4f ReadC4fFrom4SignedInt8()
    {
        return new C4f(ReadSByte(), ReadSByte(), ReadSByte(), ReadSByte());
    }

    public C4f ReadC4fFrom4UInt8()
    {
        return new C4f(ReadByte(), ReadByte(), ReadByte(), ReadByte());
    }

    public C4f ReadC4fFrom4Int16()
    {
        return new C4f(ReadInt16(), ReadInt16(), ReadInt16(), ReadInt16());
    }

    public C4f ReadC4fFrom4UInt16()
    {
        return new C4f(ReadUInt16(), ReadUInt16(), ReadUInt16(), ReadUInt16());
    }

    public C4f ReadC4fFrom4Int32()
    {
        return new C4f(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
    }

    public C4f ReadC4fFrom4UInt32()
    {
        return new C4f(ReadUInt32(), ReadUInt32(), ReadUInt32(), ReadUInt32());
    }

    #endregion

}
