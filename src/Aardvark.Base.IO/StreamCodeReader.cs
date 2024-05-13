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
        private const int c_bufferSize = 262144;
        // private byte[] m_buffer = new byte[c_bufferSize];
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
            Read(m_guidBuffer, 0, 16);
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

        #region Camera

        public CameraExtrinsics ReadCameraExtrinsics()
        {
            return new CameraExtrinsics(ReadM33d(), ReadV3d());
        }

        public CameraIntrinsics ReadCameraIntrinsics()
        {
            return new CameraIntrinsics(ReadV2d(), ReadV2d(),
                                        ReadDouble(), ReadDouble(), ReadDouble(),
                                        ReadDouble(), ReadDouble(), ReadDouble());
        }

        #endregion

        #region Read Arrays and Lists

        public long ReadArray(byte[] array, long index, long count)
        {
            if (count < 1) return 0;
            return (long)Read(array, (int)index, (int)count);
        }

        [StructLayout(LayoutKind.Explicit)]
        struct ByteArrayUnion
        {
            [FieldOffset(0)]
            public byte[] bytes;
            [FieldOffset(0)]
            public Array structs;
        }

        private static IntPtr s_byteId = GetTypeIdUncached<byte>();
        private static IntPtr GetTypeIdUncached<T>() where T : struct
        {
            var gcHandle = GCHandle.Alloc(new T[1], GCHandleType.Pinned);
            var typeField = gcHandle.AddrOfPinnedObject() - 2 * IntPtr.Size;
            var typeId = Marshal.ReadIntPtr(typeField);
            gcHandle.Free();
            return typeId;
        }

        public long ReadArray<T>(T[] array, long index, long count)
            where T : struct
        {
            if (count < 1) return 0;

#if NET6_0_OR_GREATER
            var span = MemoryMarshal.AsBytes(array.AsSpan((int)index, (int)count));

            var sizeOfT = span.Length / array.Length;
            var bytesToRead = span.Length;
            var offset = 0;

            do
            {
                int finished = base.Read(span);
                if (finished == 0) break;
                offset += finished; bytesToRead -= finished;
                span = span.Slice(offset, bytesToRead);
            }
            while (bytesToRead > 0);
                        
            return offset / sizeOfT;
#else
            unsafe
            {
                var sizeOfT = Marshal.SizeOf(typeof(T));
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

                    do
                    {
                        int finished = base.Read(hack.bytes, offset, bytesToRead);
                        if (finished == 0) break;
                        offset += finished; bytesToRead -= finished;
                    }
                    while (bytesToRead > 0);

                    *pId = backupId; *pLen = backupLen;
                }
                return ((long)(offset / sizeOfT) - index);
            }
#endif
        }

        public long ReadArray<T>(T[,] array, long count)
            where T : struct
        {
            if (count < 1) return 0;

#if NET6_0_OR_GREATER
            var sizeOfT = Marshal.SizeOf(typeof(T));
            var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(array), (int)count * sizeOfT);

            var bytesToRead = span.Length;
            var offset = 0;

            do
            {
                int finished = base.Read(span);
                if (finished == 0) break;
                offset += finished; bytesToRead -= finished;
                span = span.Slice(offset, bytesToRead);
            }
            while (bytesToRead > 0);

            return offset / sizeOfT;
#else
            unsafe
            {
                var sizeOfT = Marshal.SizeOf(typeof(T));
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

                    do
                    {
                        int finished = base.Read(hack.bytes, offset, bytesToRead);
                        if (finished == 0) break;
                        offset += finished; bytesToRead -= finished;
                    }
                    while (bytesToRead > 0);

                    *pId = backupId; *pLen = backupLen;
                }
                return (long)((offset - skip) / sizeOfT);
            }
#endif
        }

        public long ReadArray<T>(T[, ,] array, long count)
            where T : struct
        {
            if (count < 1) return 0;
#if NET6_0_OR_GREATER
            var sizeOfT = Marshal.SizeOf(typeof(T));
            var span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetArrayDataReference(array), (int)count * sizeOfT);

            var bytesToRead = span.Length;
            var offset = 0;

            do
            {
                int finished = base.Read(span);
                if (finished == 0) break;
                offset += finished; bytesToRead -= finished;
                span = span.Slice(offset, bytesToRead);
            }
            while (bytesToRead > 0);

            return offset / sizeOfT;
#else
            unsafe
            {
                var sizeOfT = Marshal.SizeOf(typeof(T));
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

                    do
                    {
                        int finished = base.Read(hack.bytes, offset, bytesToRead);
                        if (finished == 0) break;
                        offset += finished; bytesToRead -= finished;
                    }
                    while (bytesToRead > 0);

                    *pId = backupId; *pLen = backupLen;
                }
                return (long)((offset - skip) / sizeOfT);
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
            // m_buffer = null;
            m_guidBuffer = null;
        }

        #endregion
    }
}
