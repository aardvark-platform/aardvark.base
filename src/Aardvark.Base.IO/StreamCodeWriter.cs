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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Aardvark.Base.Coder;

public partial class StreamCodeWriter : BinaryWriter
{
#if !NET6_0_OR_GREATER
    private const int c_bufferSize = 262144;
    private byte[] m_buffer = new byte[c_bufferSize];
#endif

    #region Constructors

    public StreamCodeWriter(Stream output)
        : base(output)
    {
    }

    public StreamCodeWriter(Stream output, Encoding encoding)
        : base(output, encoding)
    {
    }

    #endregion

    #region Primitive writers

    public new void Write(string data)
    {
        base.Write(data);
    }

    public void Write(Guid data)
    {
        Write(data.ToString());
    }

    public void Write(Symbol value)
    {
        if (value.IsNegative)
        {
            Write(true);
            Write((-value).ToString());
        }
        else
        {
            Write(false);
            Write(value.ToString());
        }
    }

    public void WriteGuidSymbol(Symbol value)
    {
        var bytes = value.ToGuid().ToByteArray();
        Write(bytes, 0, 16);
    }

    public void WritePositiveSymbol(Symbol value) { Write(value.ToString()); }

    #endregion

    #region Write Transformations

    public void Write(Euclidean3f t) { Write(t.Rot); Write(t.Trans); }
    public void Write(Euclidean3d t) { Write(t.Rot); Write(t.Trans); }
    public void Write(Rot2f t) { Write(t.Angle); }
    public void Write(Rot2d t) { Write(t.Angle); }
    public void Write(Rot3f t) { Write(t.W); Write(t.V); }
    public void Write(Rot3d t) { Write(t.W); Write(t.V); }
    public void Write(Scale3f t) { Write(t.V); }
    public void Write(Scale3d t) { Write(t.V); }
    public void Write(Shift3f t) { Write(t.V); }
    public void Write(Shift3d t) { Write(t.V); }
    public void Write(Similarity3f t) { Write(t.Scale); Write(t.Euclidean); }
    public void Write(Similarity3d t) { Write(t.Scale); Write(t.Euclidean); }
    public void Write(Trafo2f t) { Write(t.Forward); Write(t.Backward); }
    public void Write(Trafo2d t) { Write(t.Forward); Write(t.Backward); }
    public void Write(Trafo3f t) { Write(t.Forward); Write(t.Backward); }
    public void Write(Trafo3d t) { Write(t.Forward); Write(t.Backward); }

    #endregion

    #region Write Arrays and Lists

#if !NET6_0_OR_GREATER
    [StructLayout(LayoutKind.Explicit)]
    struct ByteArrayUnion
    {
        [FieldOffset(0)]
        public byte[] bytes;
        [FieldOffset(0)]
        public Array structs;
    }
#endif

    public void WriteArray(byte[] array, long index, long count)
    {
        Write(array, (int)index, (int)count);
    }

    public void WriteArray<T>(T[] array, long index, long count)
                where T : struct
    {
        if (count < 1) return;

#if NET6_0_OR_GREATER
        var arrSpan = array.AsSpan((int)index, (int)count);
        var byteSpan = MemoryMarshal.AsBytes(arrSpan);
        base.Write(byteSpan);
#else
        unsafe
        {
            var sizeOfT = Unsafe.SizeOf<T>();

            var hack = new ByteArrayUnion
            {
                structs = array
            };

            fixed (byte* pBytes = hack.bytes)
            {
                var offset = index * sizeOfT;

                int itemsPerBlock = c_bufferSize / sizeOfT;
                do
                {
                    int blockSize = count > (long)itemsPerBlock
                                        ? itemsPerBlock : (int)count;
                    var byteBlockSize = blockSize * sizeOfT;

                    fixed (byte* p = m_buffer)
                    {
                        for (int i = 0; i < byteBlockSize; i++)
                            p[i] = pBytes[offset++];
                    }
                    base.Write(m_buffer, 0, byteBlockSize);
                    count -= (long)blockSize;
                }
                while (count > 0);
            }
        }
#endif
    }

    public void WriteArray<T>(T[,] array, long count)
                where T : struct
    {
        if (count < 1) return;
#if NET6_0_OR_GREATER
        var sizeOfT = Unsafe.SizeOf<T>();
        var byteSpan = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(array), (int)count * sizeOfT);
        base.Write(byteSpan);
#else
        unsafe
        {
            var sizeOfT = Unsafe.SizeOf<T>();

            var hack = new ByteArrayUnion
            {
                structs = array
            };

            fixed (byte* pBytes = hack.bytes)
            {
                long offset = 2 * 2 * sizeof(int);

                int itemsPerBlock = c_bufferSize / sizeOfT;
                do
                {
                    int blockSize = count > (long)itemsPerBlock
                                        ? itemsPerBlock : (int)count;
                    var byteBlockSize = blockSize * sizeOfT;

                    fixed (byte* p = m_buffer)
                    {
                        for (int i = 0; i < byteBlockSize; i++)
                            p[i] = pBytes[offset++];
                    }
                    base.Write(m_buffer, 0, byteBlockSize);
                    count -= (long)blockSize;
                }
                while (count > 0);
            }
        }
#endif
    }

    public void WriteArray<T>(T[, ,] array, long count)
                where T : struct
    {
        if (count < 1) return;
#if NET6_0_OR_GREATER
        var sizeOfT = Unsafe.SizeOf<T>();
        var byteSpan = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(array), (int)count * sizeOfT);
        base.Write(byteSpan);
#else
        unsafe
        {
            var sizeOfT = Unsafe.SizeOf<T>();

            var hack = new ByteArrayUnion
            {
                structs = array
            };

            fixed (byte* pBytes = hack.bytes)
            {
                long offset = 3 * 2 * sizeof(int);
                
                int itemsPerBlock = c_bufferSize / sizeOfT;
                do
                {
                    int blockSize = count > (long)itemsPerBlock
                                        ? itemsPerBlock : (int)count;
                    var byteBlockSize = blockSize * sizeOfT;

                    fixed (byte* p = m_buffer)
                    {
                        for (int i = 0; i < byteBlockSize; i++)
                            p[i] = pBytes[offset++];
                    }
                    base.Write(m_buffer, 0, byteBlockSize);
                    count -= (long)blockSize;
                }
                while (count > 0);
            }
        }
#endif
    }

    public void WriteList<T>(List<T> buffer, int index, int count)
        where T: struct
    {
        var dataObjectField = buffer.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
        var arrayValue = (T[])dataObjectField.GetValue(buffer);
        WriteArray(arrayValue, (long)index, (long)count);
    }

#endregion

    #region Close

    public override void Close()
    {
        base.Close();
#if! NET6_0_OR_GREATER
        m_buffer = null;
#endif
    }

#endregion
}
