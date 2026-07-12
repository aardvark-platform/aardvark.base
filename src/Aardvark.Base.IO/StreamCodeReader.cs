using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Aardvark.Base.Coder
{
    public partial class StreamCodeReader : BinaryReader
    {
        private byte[] m_guidBuffer = new byte[16]; // own buffer since creator requires 16-byte length

        #region Constructors

        public StreamCodeReader(Stream input)
            : base(input)
        { }

        public StreamCodeReader(Stream input, Encoding encoding)
            : base(input, encoding)
        { }

        #endregion

        #region Primitive readers

        public new string ReadString()
        {
            return base.ReadString();
        }

        public Guid ReadGuid()
        {
            return new Guid(ReadString());
        }

        public Symbol ReadSymbol()
        {
            if (ReadBoolean())
                return -Symbol.Create(ReadString());
            else
                return Symbol.Create(ReadString());
        }

        public Symbol ReadGuidSymbol()
        {
            if (ReadAvailable(m_guidBuffer, 0, m_guidBuffer.Length) != m_guidBuffer.Length)
                throw new EndOfStreamException("Unable to read a complete Guid symbol.");

            return Symbol.Create(new Guid(m_guidBuffer));
        }

        public Symbol ReadPositiveSymbol() { return Symbol.Create(ReadString()); }

        #endregion

        #region Read Transformations

        public Euclidean3f ReadEuclidean3f() { return new Euclidean3f(ReadRot3f(), ReadV3f()); }
        public Euclidean3d ReadEuclidean3d() { return new Euclidean3d(ReadRot3d(), ReadV3d()); }
        public Rot2f ReadRot2f() { return new Rot2f(ReadSingle()); }
        public Rot2d ReadRot2d() { return new Rot2d(ReadDouble()); }
        public Rot3f ReadRot3f() { return new Rot3f(ReadSingle(), ReadV3f()); }
        public Rot3d ReadRot3d() { return new Rot3d(ReadDouble(), ReadV3d()); }
        public Scale3f ReadScale3f() { return new Scale3f(ReadV3f()); }
        public Scale3d ReadScale3d() { return new Scale3d(ReadV3d()); }
        public Shift3f ReadShift3f() { return new Shift3f(ReadV3f()); }
        public Shift3d ReadShift3d() { return new Shift3d(ReadV3d()); }
        public Similarity3f ReadSimilarity3f() { return new Similarity3f(ReadSingle(), ReadEuclidean3f()); }
        public Similarity3d ReadSimilarity3d() { return new Similarity3d(ReadDouble(), ReadEuclidean3d()); }
        public Trafo2f ReadTrafo2f() { return new Trafo2f(ReadM33f(), ReadM33f()); }
        public Trafo2d ReadTrafo2d() { return new Trafo2d(ReadM33d(), ReadM33d()); }
        public Trafo3f ReadTrafo3f() { return new Trafo3f(ReadM44f(), ReadM44f()); }
        public Trafo3d ReadTrafo3d() { return new Trafo3d(ReadM44d(), ReadM44d()); }

        #endregion

        #region Read Arrays and Lists

        private int ReadAvailable(byte[] array, int index, int count)
        {
            var total = 0;
            while (total < count)
            {
                var read = base.Read(array, index + total, count - total);
                if (read == 0) break;
                total += read;
            }
            return total;
        }

#if NET6_0_OR_GREATER
        private int ReadAvailable(Span<byte> span)
        {
            var total = 0;
            while (!span.IsEmpty)
            {
                var read = base.Read(span);
                if (read == 0) break;
                total += read;
                span = span.Slice(read);
            }
            return total;
        }
#endif

        /// <summary>Reads up to <paramref name="count"/> bytes into the specified destination range, continuing across short reads until the range is full or the stream ends.</summary>
        /// <returns>The number of bytes actually read.</returns>
        public long ReadArray(byte[] array, long index, long count)
        {
            if (count < 1) return 0;
            return ReadAvailable(array, (int)index, (int)count);
        }

#if !NET6_0_OR_GREATER
        [StructLayout(LayoutKind.Explicit)]
        struct ByteArrayUnion
        {
            [FieldOffset(0)]
            public byte[] bytes;
            [FieldOffset(0)]
            public Array structs;
        }

        private static readonly IntPtr s_byteId = GetTypeIdUncached<byte>();
        private static IntPtr GetTypeIdUncached<T>() where T : struct
        {
            var gcHandle = GCHandle.Alloc(new T[1], GCHandleType.Pinned);
            var typeField = gcHandle.AddrOfPinnedObject() - 2 * IntPtr.Size;
            var typeId = Marshal.ReadIntPtr(typeField);
            gcHandle.Free();
            return typeId;
        }
#endif

        /// <summary>Reads up to <paramref name="count"/> values into the specified destination range, continuing across short reads until the range is full or the stream ends.</summary>
        /// <returns>The number of complete values read. Bytes from a trailing partial value at end of stream are not included.</returns>
        public long ReadArray<T>(T[] array, long index, long count)
            where T : struct
        {
            if (count < 1) return 0;

#if NET6_0_OR_GREATER
            var span = MemoryMarshal.AsBytes(array.AsSpan((int)index, (int)count));
            return ReadAvailable(span) / Unsafe.SizeOf<T>();
#else
            unsafe
            {
                var sizeOfT = Unsafe.SizeOf<T>();
                var hack = new ByteArrayUnion();
                hack.structs = array;

                var bytesToRead = (int)(sizeOfT * count);
                IntPtr byteLen = (IntPtr)(array.Length * sizeOfT);
                var offset = (int)(index * sizeOfT);

                fixed (byte* pBytes = hack.bytes)
                {
                    IntPtr* pId = (IntPtr*)(pBytes - 2 * IntPtr.Size),
                            pLen = (IntPtr*)(pBytes - IntPtr.Size);
                    IntPtr backupId = *pId, backupLen = *pLen;
                    *pId = s_byteId; *pLen = byteLen;

                    var bytesRead = ReadAvailable(hack.bytes, offset, bytesToRead);

                    *pId = backupId; *pLen = backupLen;

                    return bytesRead / sizeOfT;
                }
            }
#endif
        }

        /// <summary>Reads up to <paramref name="count"/> values into the two-dimensional destination in array storage order, continuing across short reads until the range is full or the stream ends.</summary>
        /// <returns>The number of complete values read. Bytes from a trailing partial value at end of stream are not included.</returns>
        public long ReadArray<T>(T[,] array, long count)
            where T : struct
        {
            if (count < 1) return 0;

#if NET6_0_OR_GREATER
            var sizeOfT = Unsafe.SizeOf<T>();
            var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(array), (int)count * sizeOfT);
            return ReadAvailable(span) / sizeOfT;
#else
            unsafe
            {
                var sizeOfT = Unsafe.SizeOf<T>();
                var hack = new ByteArrayUnion();
                hack.structs = array;

                var bytesToRead = (int)(sizeOfT * count);

                var skip = 2 * 2 * sizeof(int);

                IntPtr byteLen = (IntPtr)(array.Length * sizeOfT + skip);
                var offset = skip;

                fixed (byte* pBytes = hack.bytes)
                {
                    IntPtr* pId = (IntPtr*)(pBytes - 2 * IntPtr.Size),
                            pLen = (IntPtr*)(pBytes - IntPtr.Size);
                    IntPtr backupId = *pId, backupLen = *pLen;
                    *pId = s_byteId; *pLen = byteLen;

                    var bytesRead = ReadAvailable(hack.bytes, offset, bytesToRead);

                    *pId = backupId; *pLen = backupLen;

                    return bytesRead / sizeOfT;
                }
            }
#endif
        }

        /// <summary>Reads up to <paramref name="count"/> values into the three-dimensional destination in array storage order, continuing across short reads until the range is full or the stream ends.</summary>
        /// <returns>The number of complete values read. Bytes from a trailing partial value at end of stream are not included.</returns>
        public long ReadArray<T>(T[, ,] array, long count)
            where T : struct
        {
            if (count < 1) return 0;
#if NET6_0_OR_GREATER
            var sizeOfT = Unsafe.SizeOf<T>();
            var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(array), (int)count * sizeOfT);
            return ReadAvailable(span) / sizeOfT;
#else
            unsafe
            {
                var sizeOfT = Unsafe.SizeOf<T>();
                var hack = new ByteArrayUnion();
                hack.structs = array;

                var bytesToRead = (int)(sizeOfT * count);

                var skip = 3 * 2 * sizeof(int);

                IntPtr byteLen = (IntPtr)(array.Length * sizeOfT + skip);
                var offset = skip;

                fixed (byte* pBytes = hack.bytes)
                {
                    IntPtr* pId = (IntPtr*)(pBytes - 2 * IntPtr.Size),
                            pLen = (IntPtr*)(pBytes - IntPtr.Size);
                    IntPtr backupId = *pId, backupLen = *pLen;
                    *pId = s_byteId; *pLen = byteLen;

                    var bytesRead = ReadAvailable(hack.bytes, offset, bytesToRead);

                    *pId = backupId; *pLen = backupLen;

                    return bytesRead / sizeOfT;
                }
            }
#endif
        }

        public int ReadList<T>(List<T> buffer, int index, int count)
            where T : struct
        {
            var arrayField = buffer.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            var arrayValue = (T[])arrayField.GetValue(buffer);
            var sizeField = buffer.GetType().GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance);
            sizeField.SetValue(buffer, count);
            return (int)ReadArray(arrayValue, (long)index, (long)count);
        }

#endregion

        #region Close

        public override void Close()
        {
            base.Close();
            m_guidBuffer = null;
        }

        #endregion
    }
}
