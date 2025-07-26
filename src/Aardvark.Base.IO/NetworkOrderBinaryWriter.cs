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
/// Wrapper for class BinaryWriter.
/// The following methods are overriden in order to
/// write values in network byte order (big-endian):
/// 
/// void Write(short)
/// void Write(ushort)
/// void Write(int)
/// void Write(uint)
/// void Write(long)
/// void Write(ulong)
/// void Write(float)
/// void Write(double)
/// 
/// Additional methods added:
/// 
/// void Write(V2f)
/// void Write(V2d)
/// void Write(V3f)
/// void Write(V3d)
/// void Write(C3f)
/// void Write(C4f)
/// 
/// Use Encoding.BigEndianUnicode to output strings
/// and characters in UTF16 using big-endian byte order.
/// 
/// </summary>
public class NetworkOrderBinaryWriter : BinaryWriter
{
    public NetworkOrderBinaryWriter(Stream output)
        : base(output, Encoding.UTF8)
    {
    }

    public NetworkOrderBinaryWriter(Stream output, Encoding encoding)
        : base(output, encoding)
    {
        if (encoding == Encoding.Unicode ||
            encoding == Encoding.UTF32)
        {
            throw new Exception(
                "Encoding '" + encoding.EncodingName +
                "' uses little-endian byte order, which " +
                "makes no sense for a network (big-endian) " +
                "binary writer."
                );
        }
    }

    public override void Write(short value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public override void Write(ushort value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public override void Write(int value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public override void Write(uint value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public override void Write(long value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public override void Write(ulong value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public override void Write(float value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public override void Write(double value)
    {
        base.Write(Conversion.HostToNetworkOrder(value));
    }

    public void Write(V2f value)
    {
        for (int i = 0; i < 2; i++)
            base.Write(Conversion.HostToNetworkOrder(value[i]));
    }

    public void Write(V2d value)
    {
        for (int i = 0; i < 2; i++)
            base.Write(Conversion.HostToNetworkOrder(value[i]));
    }

    public void Write(V3f value)
    {
        for (int i = 0; i < 3; i++)
            base.Write(Conversion.HostToNetworkOrder(value[i]));
    }

    public void Write(V3d value)
    {
        for (int i = 0; i < 3; i++)
            base.Write(Conversion.HostToNetworkOrder(value[i]));
    }

    public void Write(C3f value)
    {
        base.Write(Conversion.HostToNetworkOrder(value.R));
        base.Write(Conversion.HostToNetworkOrder(value.G));
        base.Write(Conversion.HostToNetworkOrder(value.B));
    }

    public void Write(C4f value)
    {
        base.Write(Conversion.HostToNetworkOrder(value.R));
        base.Write(Conversion.HostToNetworkOrder(value.G));
        base.Write(Conversion.HostToNetworkOrder(value.B));
        base.Write(Conversion.HostToNetworkOrder(value.A));
    }
}
