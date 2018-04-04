using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static class ArrayFun_
    {
        #region UnsafeCoerce

        public static IntPtr GetTypeIdUncached<T>()
            where T : struct
        {
            var gcHandle = GCHandle.Alloc(new T[1], GCHandleType.Pinned);
            var typeField = gcHandle.AddrOfPinnedObject() - 2 * IntPtr.Size;
            var typeId = Marshal.ReadIntPtr(typeField);
            gcHandle.Free();
            return typeId;
        }

        private static FastConcurrentDict<Type, IntPtr> s_typeIds = new FastConcurrentDict<Type, IntPtr>();

        private static IntPtr GetTypeId<T>()
            where T : struct
        {
            IntPtr typeId;
            if (!s_typeIds.TryGetValue(typeof(T), out typeId))
            {
                typeId = GetTypeIdUncached<T>();
                s_typeIds[typeof(T)] = typeId;
            }
            return typeId;
        }

        internal static TR[] UnsafeCoerce<TR>(this Array input, IntPtr targetId)
            where TR : struct
        {
            var inputSize = Marshal.SizeOf(input.GetType().GetElementType());
            var outputSize = Marshal.SizeOf(typeof(TR));
            var newLength = (input.Length * inputSize) / outputSize;

            var gcHandle = GCHandle.Alloc(input, GCHandleType.Pinned);
            IntPtr baseAddress = gcHandle.AddrOfPinnedObject();
            var sizeField = baseAddress - IntPtr.Size;
            var typeField = sizeField - IntPtr.Size;

            Marshal.WriteIntPtr(sizeField, (IntPtr)newLength);
            Marshal.WriteIntPtr(typeField, targetId);

            gcHandle.Free();

            return (TR[])(object)input;
        }

        /// <summary>
        /// Reinterprets an array as one of a different type
        /// Both types must be structs and you may cause memory leaks when the array-byte-sizes are not multiple of each other
        /// WARNING: destroys the original array
        /// </summary>
        public static TR[] UnsafeCoerce<TR>(this Array input)
            where TR : struct
        {
            return UnsafeCoerce<TR>(input, GetTypeId<TR>());
        }

        internal static void UnsafeCoercedApply<TR>(this Array input, Action<TR[]> action, IntPtr targetId)
            where TR : struct
        {
            var inputSize = Marshal.SizeOf(input.GetType().GetElementType());
            var outputSize = Marshal.SizeOf(typeof(TR));
            var originalLength = input.Length;
            var targetLength = (originalLength * inputSize) / outputSize;

            var gcHandle = GCHandle.Alloc(input, GCHandleType.Pinned);
            IntPtr baseAddress = gcHandle.AddrOfPinnedObject();
            var sizeField = baseAddress - IntPtr.Size;
            var typeField = sizeField - IntPtr.Size;

            var originalId = Marshal.ReadIntPtr(typeField);

            Marshal.WriteIntPtr(typeField, targetId);
            Marshal.WriteIntPtr(sizeField, (IntPtr)targetLength);

            action((TR[])(object)input);

            Marshal.WriteIntPtr(typeField, originalId);
            Marshal.WriteIntPtr(sizeField, (IntPtr)originalLength);

            gcHandle.Free();
        }

        public static void UnsafeCoercedApply<TR>(this Array input, Action<TR[]> action)
            where TR : struct
        {
            UnsafeCoercedApply(input, action, GetTypeId<TR>());
        }

        #endregion
        
		#region Non-Generic Array

		public static Dictionary<Type, Func<Array, Array>> CopyFunMap =
			new Dictionary<Type, Func<Array, Array>>
			{
				{ typeof(byte[]), a => ((byte[])a).Copy() },
				{ typeof(sbyte[]), a => ((sbyte[])a).Copy() },
				{ typeof(ushort[]), a => ((ushort[])a).Copy() },
				{ typeof(short[]), a => ((short[])a).Copy() },
				{ typeof(uint[]), a => ((uint[])a).Copy() },
				{ typeof(int[]), a => ((int[])a).Copy() },
				{ typeof(ulong[]), a => ((ulong[])a).Copy() },
				{ typeof(long[]), a => ((long[])a).Copy() },
				{ typeof(float[]), a => ((float[])a).Copy() },
				{ typeof(double[]), a => ((double[])a).Copy() },
				{ typeof(C3b[]), a => ((C3b[])a).Copy() },
				{ typeof(C3us[]), a => ((C3us[])a).Copy() },
				{ typeof(C3ui[]), a => ((C3ui[])a).Copy() },
				{ typeof(C3f[]), a => ((C3f[])a).Copy() },
				{ typeof(C3d[]), a => ((C3d[])a).Copy() },
				{ typeof(C4b[]), a => ((C4b[])a).Copy() },
				{ typeof(C4us[]), a => ((C4us[])a).Copy() },
				{ typeof(C4ui[]), a => ((C4ui[])a).Copy() },
				{ typeof(C4f[]), a => ((C4f[])a).Copy() },
				{ typeof(C4d[]), a => ((C4d[])a).Copy() },
				{ typeof(V2i[]), a => ((V2i[])a).Copy() },
				{ typeof(V2l[]), a => ((V2l[])a).Copy() },
				{ typeof(V2f[]), a => ((V2f[])a).Copy() },
				{ typeof(V2d[]), a => ((V2d[])a).Copy() },
				{ typeof(V3i[]), a => ((V3i[])a).Copy() },
				{ typeof(V3l[]), a => ((V3l[])a).Copy() },
				{ typeof(V3f[]), a => ((V3f[])a).Copy() },
				{ typeof(V3d[]), a => ((V3d[])a).Copy() },
				{ typeof(V4i[]), a => ((V4i[])a).Copy() },
				{ typeof(V4l[]), a => ((V4l[])a).Copy() },
				{ typeof(V4f[]), a => ((V4f[])a).Copy() },
				{ typeof(V4d[]), a => ((V4d[])a).Copy() },
			};

		public static Array Copy(this Array array)
		{
			return CopyFunMap[array.GetType()](array);
		}

		public static Dictionary<Type, Func<Array, object, Array>> CopyFunFunMap =
			new Dictionary<Type, Func<Array, object, Array>>
			{
				{ typeof(byte[]), (a, f) => ((byte[])a).Map((Func<byte,byte>)f) },
				{ typeof(sbyte[]), (a, f) => ((sbyte[])a).Map((Func<sbyte,sbyte>)f) },
				{ typeof(ushort[]), (a, f) => ((ushort[])a).Map((Func<ushort,ushort>)f) },
				{ typeof(short[]), (a, f) => ((short[])a).Map((Func<short,short>)f) },
				{ typeof(uint[]), (a, f) => ((uint[])a).Map((Func<uint,uint>)f) },
				{ typeof(int[]), (a, f) => ((int[])a).Map((Func<int,int>)f) },
				{ typeof(ulong[]), (a, f) => ((ulong[])a).Map((Func<ulong,ulong>)f) },
				{ typeof(long[]), (a, f) => ((long[])a).Map((Func<long,long>)f) },
				{ typeof(float[]), (a, f) => ((float[])a).Map((Func<float,float>)f) },
				{ typeof(double[]), (a, f) => ((double[])a).Map((Func<double,double>)f) },
				{ typeof(C3b[]), (a, f) => ((C3b[])a).Map((Func<C3b,C3b>)f) },
				{ typeof(C3us[]), (a, f) => ((C3us[])a).Map((Func<C3us,C3us>)f) },
				{ typeof(C3ui[]), (a, f) => ((C3ui[])a).Map((Func<C3ui,C3ui>)f) },
				{ typeof(C3f[]), (a, f) => ((C3f[])a).Map((Func<C3f,C3f>)f) },
				{ typeof(C3d[]), (a, f) => ((C3d[])a).Map((Func<C3d,C3d>)f) },
				{ typeof(C4b[]), (a, f) => ((C4b[])a).Map((Func<C4b,C4b>)f) },
				{ typeof(C4us[]), (a, f) => ((C4us[])a).Map((Func<C4us,C4us>)f) },
				{ typeof(C4ui[]), (a, f) => ((C4ui[])a).Map((Func<C4ui,C4ui>)f) },
				{ typeof(C4f[]), (a, f) => ((C4f[])a).Map((Func<C4f,C4f>)f) },
				{ typeof(C4d[]), (a, f) => ((C4d[])a).Map((Func<C4d,C4d>)f) },
				{ typeof(V2i[]), (a, f) => ((V2i[])a).Map((Func<V2i,V2i>)f) },
				{ typeof(V2l[]), (a, f) => ((V2l[])a).Map((Func<V2l,V2l>)f) },
				{ typeof(V2f[]), (a, f) => ((V2f[])a).Map((Func<V2f,V2f>)f) },
				{ typeof(V2d[]), (a, f) => ((V2d[])a).Map((Func<V2d,V2d>)f) },
				{ typeof(V3i[]), (a, f) => ((V3i[])a).Map((Func<V3i,V3i>)f) },
				{ typeof(V3l[]), (a, f) => ((V3l[])a).Map((Func<V3l,V3l>)f) },
				{ typeof(V3f[]), (a, f) => ((V3f[])a).Map((Func<V3f,V3f>)f) },
				{ typeof(V3d[]), (a, f) => ((V3d[])a).Map((Func<V3d,V3d>)f) },
				{ typeof(V4i[]), (a, f) => ((V4i[])a).Map((Func<V4i,V4i>)f) },
				{ typeof(V4l[]), (a, f) => ((V4l[])a).Map((Func<V4l,V4l>)f) },
				{ typeof(V4f[]), (a, f) => ((V4f[])a).Map((Func<V4f,V4f>)f) },
				{ typeof(V4d[]), (a, f) => ((V4d[])a).Map((Func<V4d,V4d>)f) },
			};

		public static Array Copy<T>(this Array array,
									Func<T, T> funOfElementTypeToElementType,
									Func<Array, Array> defaultFun = null)
		{
			var arrayType = array.GetType();
			if (typeof(T) == arrayType.GetElementType())
				return CopyFunFunMap[arrayType](array, funOfElementTypeToElementType);
			else if (defaultFun != null)
				return defaultFun(array);
			else
				return CopyFunMap[arrayType](array);
		}

		public static Dictionary<Type, Func<Array, int, Array>> ResizedFunMap =
			new Dictionary<Type, Func<Array, int, Array>>
			{
				{ typeof(byte[]), (a, s) => ((byte[])a).Resized(s) },
				{ typeof(sbyte[]), (a, s) => ((sbyte[])a).Resized(s) },
				{ typeof(ushort[]), (a, s) => ((ushort[])a).Resized(s) },
				{ typeof(short[]), (a, s) => ((short[])a).Resized(s) },
				{ typeof(uint[]), (a, s) => ((uint[])a).Resized(s) },
				{ typeof(int[]), (a, s) => ((int[])a).Resized(s) },
				{ typeof(ulong[]), (a, s) => ((ulong[])a).Resized(s) },
				{ typeof(long[]), (a, s) => ((long[])a).Resized(s) },
				{ typeof(float[]), (a, s) => ((float[])a).Resized(s) },
				{ typeof(double[]), (a, s) => ((double[])a).Resized(s) },
				{ typeof(C3b[]), (a, s) => ((C3b[])a).Resized(s) },
				{ typeof(C3us[]), (a, s) => ((C3us[])a).Resized(s) },
				{ typeof(C3ui[]), (a, s) => ((C3ui[])a).Resized(s) },
				{ typeof(C3f[]), (a, s) => ((C3f[])a).Resized(s) },
				{ typeof(C3d[]), (a, s) => ((C3d[])a).Resized(s) },
				{ typeof(C4b[]), (a, s) => ((C4b[])a).Resized(s) },
				{ typeof(C4us[]), (a, s) => ((C4us[])a).Resized(s) },
				{ typeof(C4ui[]), (a, s) => ((C4ui[])a).Resized(s) },
				{ typeof(C4f[]), (a, s) => ((C4f[])a).Resized(s) },
				{ typeof(C4d[]), (a, s) => ((C4d[])a).Resized(s) },
				{ typeof(V2i[]), (a, s) => ((V2i[])a).Resized(s) },
				{ typeof(V2l[]), (a, s) => ((V2l[])a).Resized(s) },
				{ typeof(V2f[]), (a, s) => ((V2f[])a).Resized(s) },
				{ typeof(V2d[]), (a, s) => ((V2d[])a).Resized(s) },
				{ typeof(V3i[]), (a, s) => ((V3i[])a).Resized(s) },
				{ typeof(V3l[]), (a, s) => ((V3l[])a).Resized(s) },
				{ typeof(V3f[]), (a, s) => ((V3f[])a).Resized(s) },
				{ typeof(V3d[]), (a, s) => ((V3d[])a).Resized(s) },
				{ typeof(V4i[]), (a, s) => ((V4i[])a).Resized(s) },
				{ typeof(V4l[]), (a, s) => ((V4l[])a).Resized(s) },
				{ typeof(V4f[]), (a, s) => ((V4f[])a).Resized(s) },
				{ typeof(V4d[]), (a, s) => ((V4d[])a).Resized(s) },
			};

		public static Array Resized(this Array array, int size)
		{
			return ResizedFunMap[array.GetType()](array, size);
		}

		public static Dictionary<Type, Func<Array, int[], int, Array>>
			BackMappedCopyFunMap = new Dictionary<Type, Func<Array, int[], int, Array>>
			{
				{ typeof(byte[]), (a,m,c) => ((byte[])a).BackMappedCopy(m, c) },
				{ typeof(sbyte[]), (a,m,c) => ((sbyte[])a).BackMappedCopy(m, c) },
				{ typeof(short[]), (a,m,c) => ((short[])a).BackMappedCopy(m, c) },
				{ typeof(ushort[]), (a,m,c) => ((ushort[])a).BackMappedCopy(m, c) },
				{ typeof(int[]), (a,m,c) => ((int[])a).BackMappedCopy(m, c) },
				{ typeof(uint[]), (a,m,c) => ((uint[])a).BackMappedCopy(m, c) },
				{ typeof(long[]), (a,m,c) => ((long[])a).BackMappedCopy(m, c) },
				{ typeof(ulong[]), (a,m,c) => ((ulong[])a).BackMappedCopy(m, c) },
				{ typeof(float[]), (a,m,c) => ((float[])a).BackMappedCopy(m, c) },
				{ typeof(double[]), (a,m,c) => ((double[])a).BackMappedCopy(m, c) },
				{ typeof(C3b[]), (a,m,c) => ((C3b[])a).BackMappedCopy(m, c) },
				{ typeof(C3us[]), (a,m,c) => ((C3us[])a).BackMappedCopy(m, c) },
				{ typeof(C3ui[]), (a,m,c) => ((C3ui[])a).BackMappedCopy(m, c) },
				{ typeof(C3f[]), (a,m,c) => ((C3f[])a).BackMappedCopy(m, c) },
				{ typeof(C3d[]), (a,m,c) => ((C3d[])a).BackMappedCopy(m, c) },
				{ typeof(C4b[]), (a,m,c) => ((C4b[])a).BackMappedCopy(m, c) },
				{ typeof(C4us[]), (a,m,c) => ((C4us[])a).BackMappedCopy(m, c) },
				{ typeof(C4ui[]), (a,m,c) => ((C4ui[])a).BackMappedCopy(m, c) },
				{ typeof(C4f[]), (a,m,c) => ((C4f[])a).BackMappedCopy(m, c) },
				{ typeof(C4d[]), (a,m,c) => ((C4d[])a).BackMappedCopy(m, c) },
				{ typeof(V2i[]), (a,m,c) => ((V2i[])a).BackMappedCopy(m, c) },
				{ typeof(V2l[]), (a,m,c) => ((V2l[])a).BackMappedCopy(m, c) },
				{ typeof(V2f[]), (a,m,c) => ((V2f[])a).BackMappedCopy(m, c) },
				{ typeof(V2d[]), (a,m,c) => ((V2d[])a).BackMappedCopy(m, c) },
				{ typeof(V3i[]), (a,m,c) => ((V3i[])a).BackMappedCopy(m, c) },
				{ typeof(V3l[]), (a,m,c) => ((V3l[])a).BackMappedCopy(m, c) },
				{ typeof(V3f[]), (a,m,c) => ((V3f[])a).BackMappedCopy(m, c) },
				{ typeof(V3d[]), (a,m,c) => ((V3d[])a).BackMappedCopy(m, c) },
				{ typeof(V4i[]), (a,m,c) => ((V4i[])a).BackMappedCopy(m, c) },
				{ typeof(V4l[]), (a,m,c) => ((V4l[])a).BackMappedCopy(m, c) },
				{ typeof(V4f[]), (a,m,c) => ((V4f[])a).BackMappedCopy(m, c) },
				{ typeof(V4d[]), (a,m,c) => ((V4d[])a).BackMappedCopy(m, c) },
			};

		public static Array BackMappedCopy(
				this Array source, int[] backMap, int count = 0)
		{
			Func<Array, int[], int, Array> fun;
			if (BackMappedCopyFunMap.TryGetValue(source.GetType(), out fun))
				return fun(source, backMap, count);
			return null;
		}

		public static Dictionary<Type, Func<Array, int[], int[], int, Array>>
			BackMappedIndexedCopyFunMap = new Dictionary<Type, Func<Array, int[], int[], int, Array>>
			{
				{ typeof(byte[]), (a,i,m,c) => ((byte[])a).BackMappedCopy(i, m, c) },
				{ typeof(sbyte[]), (a,i,m,c) => ((sbyte[])a).BackMappedCopy(i, m, c) },
				{ typeof(short[]), (a,i,m,c) => ((short[])a).BackMappedCopy(i, m, c) },
				{ typeof(ushort[]), (a,i,m,c) => ((ushort[])a).BackMappedCopy(i, m, c) },
				{ typeof(int[]), (a,i,m,c) => ((int[])a).BackMappedCopy(i, m, c) },
				{ typeof(uint[]), (a,i,m,c) => ((uint[])a).BackMappedCopy(i, m, c) },
				{ typeof(long[]), (a,i,m,c) => ((long[])a).BackMappedCopy(i, m, c) },
				{ typeof(ulong[]), (a,i,m,c) => ((ulong[])a).BackMappedCopy(i, m, c) },
				{ typeof(float[]), (a,i,m,c) => ((float[])a).BackMappedCopy(i, m, c) },
				{ typeof(double[]), (a,i,m,c) => ((double[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3b[]), (a,i,m,c) => ((C3b[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3us[]), (a,i,m,c) => ((C3us[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3ui[]), (a,i,m,c) => ((C3ui[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3f[]), (a,i,m,c) => ((C3f[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3d[]), (a,i,m,c) => ((C3d[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4b[]), (a,i,m,c) => ((C4b[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4us[]), (a,i,m,c) => ((C4us[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4ui[]), (a,i,m,c) => ((C4ui[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4f[]), (a,i,m,c) => ((C4f[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4d[]), (a,i,m,c) => ((C4d[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2i[]), (a,i,m,c) => ((V2i[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2l[]), (a,i,m,c) => ((V2l[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2f[]), (a,i,m,c) => ((V2f[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2d[]), (a,i,m,c) => ((V2d[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3i[]), (a,i,m,c) => ((V3i[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3l[]), (a,i,m,c) => ((V3l[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3f[]), (a,i,m,c) => ((V3f[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3d[]), (a,i,m,c) => ((V3d[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4i[]), (a,i,m,c) => ((V4i[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4l[]), (a,i,m,c) => ((V4l[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4f[]), (a,i,m,c) => ((V4f[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4d[]), (a,i,m,c) => ((V4d[])a).BackMappedCopy(i, m, c) },
			};

		public static Array BackMappedCopy(
				this Array source, int[] indexArray, int[] backMap, int count = 0)
		{
			Func<Array, int[], int[], int, Array> fun;
			if (BackMappedIndexedCopyFunMap.TryGetValue(source.GetType(), out fun))
				return fun(source, indexArray, backMap, count);
			return null;
		}

		public static Dictionary<Type, Action<Array, Array, int[], int>>
			BackMappedCopyToFunMap = new Dictionary<Type, Action<Array, Array, int[], int>>
			{
				{ typeof(byte[]), (s,t,m,o) => ((byte[])s).BackMappedCopyTo((byte[])t,m,o) },
				{ typeof(sbyte[]), (s,t,m,o) => ((sbyte[])s).BackMappedCopyTo((sbyte[])t,m,o) },
				{ typeof(short[]), (s,t,m,o) => ((short[])s).BackMappedCopyTo((short[])t,m,o) },
				{ typeof(ushort[]), (s,t,m,o) => ((ushort[])s).BackMappedCopyTo((ushort[])t,m,o) },
				{ typeof(int[]), (s,t,m,o) => ((int[])s).BackMappedCopyTo((int[])t,m,o) },
				{ typeof(uint[]), (s,t,m,o) => ((uint[])s).BackMappedCopyTo((uint[])t,m,o) },
				{ typeof(long[]), (s,t,m,o) => ((long[])s).BackMappedCopyTo((long[])t,m,o) },
				{ typeof(ulong[]), (s,t,m,o) => ((ulong[])s).BackMappedCopyTo((ulong[])t,m,o) },
				{ typeof(float[]), (s,t,m,o) => ((float[])s).BackMappedCopyTo((float[])t,m,o) },
				{ typeof(double[]), (s,t,m,o) => ((double[])s).BackMappedCopyTo((double[])t,m,o) },
				{ typeof(C3b[]), (s,t,m,o) => ((C3b[])s).BackMappedCopyTo((C3b[])t,m,o) },
				{ typeof(C3us[]), (s,t,m,o) => ((C3us[])s).BackMappedCopyTo((C3us[])t,m,o) },
				{ typeof(C3ui[]), (s,t,m,o) => ((C3ui[])s).BackMappedCopyTo((C3ui[])t,m,o) },
				{ typeof(C3f[]), (s,t,m,o) => ((C3f[])s).BackMappedCopyTo((C3f[])t,m,o) },
				{ typeof(C3d[]), (s,t,m,o) => ((C3d[])s).BackMappedCopyTo((C3d[])t,m,o) },
				{ typeof(C4b[]), (s,t,m,o) => ((C4b[])s).BackMappedCopyTo((C4b[])t,m,o) },
				{ typeof(C4us[]), (s,t,m,o) => ((C4us[])s).BackMappedCopyTo((C4us[])t,m,o) },
				{ typeof(C4ui[]), (s,t,m,o) => ((C4ui[])s).BackMappedCopyTo((C4ui[])t,m,o) },
				{ typeof(C4f[]), (s,t,m,o) => ((C4f[])s).BackMappedCopyTo((C4f[])t,m,o) },
				{ typeof(C4d[]), (s,t,m,o) => ((C4d[])s).BackMappedCopyTo((C4d[])t,m,o) },
				{ typeof(V2i[]), (s,t,m,o) => ((V2i[])s).BackMappedCopyTo((V2i[])t,m,o) },
				{ typeof(V2l[]), (s,t,m,o) => ((V2l[])s).BackMappedCopyTo((V2l[])t,m,o) },
				{ typeof(V2f[]), (s,t,m,o) => ((V2f[])s).BackMappedCopyTo((V2f[])t,m,o) },
				{ typeof(V2d[]), (s,t,m,o) => ((V2d[])s).BackMappedCopyTo((V2d[])t,m,o) },
				{ typeof(V3i[]), (s,t,m,o) => ((V3i[])s).BackMappedCopyTo((V3i[])t,m,o) },
				{ typeof(V3l[]), (s,t,m,o) => ((V3l[])s).BackMappedCopyTo((V3l[])t,m,o) },
				{ typeof(V3f[]), (s,t,m,o) => ((V3f[])s).BackMappedCopyTo((V3f[])t,m,o) },
				{ typeof(V3d[]), (s,t,m,o) => ((V3d[])s).BackMappedCopyTo((V3d[])t,m,o) },
				{ typeof(V4i[]), (s,t,m,o) => ((V4i[])s).BackMappedCopyTo((V4i[])t,m,o) },
				{ typeof(V4l[]), (s,t,m,o) => ((V4l[])s).BackMappedCopyTo((V4l[])t,m,o) },
				{ typeof(V4f[]), (s,t,m,o) => ((V4f[])s).BackMappedCopyTo((V4f[])t,m,o) },
				{ typeof(V4d[]), (s,t,m,o) => ((V4d[])s).BackMappedCopyTo((V4d[])t,m,o) },
			};

		public static Array BackMappedCopyTo(
				this Array source, int[] backwardMap,
				Array target, int offset)
		{
			BackMappedCopyToFunMap[source.GetType()](
					source, target, backwardMap, offset);
			return target;
		}


		public static Dictionary<Type, Action<Array, Array, int, int[], int[], int>>
			BackMappedGroupCopyToFunMap = new Dictionary<Type, Action<Array, Array, int, int[], int[], int>>
			{
				{ typeof(byte[]), (s,t,c,b,f,o) => ((byte[])s).BackMappedGroupCopyTo(b, c, f, (byte[])t, o) },
				{ typeof(sbyte[]), (s,t,c,b,f,o) => ((sbyte[])s).BackMappedGroupCopyTo(b, c, f, (sbyte[])t, o) },
				{ typeof(short[]), (s,t,c,b,f,o) => ((short[])s).BackMappedGroupCopyTo(b, c, f, (short[])t, o) },
				{ typeof(ushort[]), (s,t,c,b,f,o) => ((ushort[])s).BackMappedGroupCopyTo(b, c, f, (ushort[])t, o) },
				{ typeof(int[]), (s,t,c,b,f,o) => ((int[])s).BackMappedGroupCopyTo(b, c, f, (int[])t, o) },
				{ typeof(uint[]), (s,t,c,b,f,o) => ((uint[])s).BackMappedGroupCopyTo(b, c, f, (uint[])t, o) },
				{ typeof(long[]), (s,t,c,b,f,o) => ((long[])s).BackMappedGroupCopyTo(b, c, f, (long[])t, o) },
				{ typeof(ulong[]), (s,t,c,b,f,o) => ((ulong[])s).BackMappedGroupCopyTo(b, c, f, (ulong[])t, o) },
				{ typeof(float[]), (s,t,c,b,f,o) => ((float[])s).BackMappedGroupCopyTo(b, c, f, (float[])t, o) },
				{ typeof(double[]), (s,t,c,b,f,o) => ((double[])s).BackMappedGroupCopyTo(b, c, f, (double[])t, o) },
			};

		public static void BackMappedGroupCopyTo(this Array source, int[] faceBackMap, int faceCount, int[] fia, Array target, int offset)
		{
			BackMappedGroupCopyToFunMap[source.GetType()]
					(source, target, faceCount, faceBackMap, fia, offset);
		}

		public static Dictionary<Type, Action<Array, long, long>>
			SwapFunMap = new Dictionary<Type, Action<Array, long, long>>
			{
				{ typeof(byte[]), (a, i, j) => ((byte[])a).Swap(i, j) },
				{ typeof(sbyte[]), (a, i, j) => ((sbyte[])a).Swap(i, j) },
				{ typeof(short[]), (a, i, j) => ((short[])a).Swap(i, j) },
				{ typeof(ushort[]), (a, i, j) => ((ushort[])a).Swap(i, j) },
				{ typeof(int[]), (a, i, j) => ((int[])a).Swap(i, j) },
				{ typeof(uint[]), (a, i, j) => ((uint[])a).Swap(i, j) },
				{ typeof(long[]), (a, i, j) => ((long[])a).Swap(i, j) },
				{ typeof(ulong[]), (a, i, j) => ((ulong[])a).Swap(i, j) },
				{ typeof(float[]), (a, i, j) => ((float[])a).Swap(i, j) },
				{ typeof(double[]), (a, i, j) => ((double[])a).Swap(i, j) },
				{ typeof(C3b[]), (a, i, j) => ((C3b[])a).Swap(i, j) },
				{ typeof(C3us[]), (a, i, j) => ((C3us[])a).Swap(i, j) },
				{ typeof(C3ui[]), (a, i, j) => ((C3ui[])a).Swap(i, j) },
				{ typeof(C3f[]), (a, i, j) => ((C3f[])a).Swap(i, j) },
				{ typeof(C3d[]), (a, i, j) => ((C3d[])a).Swap(i, j) },
				{ typeof(C4b[]), (a, i, j) => ((C4b[])a).Swap(i, j) },
				{ typeof(C4us[]), (a, i, j) => ((C4us[])a).Swap(i, j) },
				{ typeof(C4ui[]), (a, i, j) => ((C4ui[])a).Swap(i, j) },
				{ typeof(C4f[]), (a, i, j) => ((C4f[])a).Swap(i, j) },
				{ typeof(C4d[]), (a, i, j) => ((C4d[])a).Swap(i, j) },
				{ typeof(V2i[]), (a, i, j) => ((V2i[])a).Swap(i, j) },
				{ typeof(V2l[]), (a, i, j) => ((V2l[])a).Swap(i, j) },
				{ typeof(V2f[]), (a, i, j) => ((V2f[])a).Swap(i, j) },
				{ typeof(V2d[]), (a, i, j) => ((V2d[])a).Swap(i, j) },
				{ typeof(V3i[]), (a, i, j) => ((V3i[])a).Swap(i, j) },
				{ typeof(V3l[]), (a, i, j) => ((V3l[])a).Swap(i, j) },
				{ typeof(V3f[]), (a, i, j) => ((V3f[])a).Swap(i, j) },
				{ typeof(V3d[]), (a, i, j) => ((V3d[])a).Swap(i, j) },
				{ typeof(V4i[]), (a, i, j) => ((V4i[])a).Swap(i, j) },
				{ typeof(V4l[]), (a, i, j) => ((V4l[])a).Swap(i, j) },
				{ typeof(V4f[]), (a, i, j) => ((V4f[])a).Swap(i, j) },
				{ typeof(V4d[]), (a, i, j) => ((V4d[])a).Swap(i, j) },
			};

		public static void Swap(this Array array, long i, long j)
		{
			SwapFunMap[array.GetType()](array, i, j);
		}

		public static Dictionary<Type, Func<Array, int[], int, Func<int, bool>, Array>>
			GroupReversedCopyMap = new Dictionary<Type, Func<Array, int[], int, Func<int, bool>, Array>>
			{
				{ typeof(byte[]), (a, g, c, r) => ((byte[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(sbyte[]), (a, g, c, r) => ((sbyte[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(short[]), (a, g, c, r) => ((short[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(ushort[]), (a, g, c, r) => ((ushort[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(int[]), (a, g, c, r) => ((int[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(uint[]), (a, g, c, r) => ((uint[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(long[]), (a, g, c, r) => ((long[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(ulong[]), (a, g, c, r) => ((ulong[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(float[]), (a, g, c, r) => ((float[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(double[]), (a, g, c, r) => ((double[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(C3b[]), (a, g, c, r) => ((C3b[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3us[]), (a, g, c, r) => ((C3us[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3ui[]), (a, g, c, r) => ((C3ui[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3f[]), (a, g, c, r) => ((C3f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3d[]), (a, g, c, r) => ((C3d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4b[]), (a, g, c, r) => ((C4b[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4us[]), (a, g, c, r) => ((C4us[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4ui[]), (a, g, c, r) => ((C4ui[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4f[]), (a, g, c, r) => ((C4f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4d[]), (a, g, c, r) => ((C4d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2i[]), (a, g, c, r) => ((V2i[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2l[]), (a, g, c, r) => ((V2l[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2f[]), (a, g, c, r) => ((V2f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2d[]), (a, g, c, r) => ((V2d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3i[]), (a, g, c, r) => ((V3i[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3l[]), (a, g, c, r) => ((V3l[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3f[]), (a, g, c, r) => ((V3f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3d[]), (a, g, c, r) => ((V3d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4i[]), (a, g, c, r) => ((V4i[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4l[]), (a, g, c, r) => ((V4l[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4f[]), (a, g, c, r) => ((V4f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4d[]), (a, g, c, r) => ((V4d[])a).GroupReversedCopy(g, c, r) },
			};

		public static Array GroupReversedCopy(
				this Array array, int[] groupArray, int groupCount,
				Func<int, bool> reverseMap)
		{
			return GroupReversedCopyMap[array.GetType()](
						array, groupArray, groupCount, reverseMap);
		}

		public static Dictionary<Type, Action<Array, int[], int, Func<int, bool>>>
			ReverseGroupsMap = new Dictionary<Type, Action<Array, int[], int, Func<int, bool>>>
			{
				{ typeof(byte[]), (a, g, c, r) => ((byte[])a).ReverseGroups(g, c, r) }, 
				{ typeof(sbyte[]), (a, g, c, r) => ((sbyte[])a).ReverseGroups(g, c, r) }, 
				{ typeof(short[]), (a, g, c, r) => ((short[])a).ReverseGroups(g, c, r) }, 
				{ typeof(ushort[]), (a, g, c, r) => ((ushort[])a).ReverseGroups(g, c, r) }, 
				{ typeof(int[]), (a, g, c, r) => ((int[])a).ReverseGroups(g, c, r) }, 
				{ typeof(uint[]), (a, g, c, r) => ((uint[])a).ReverseGroups(g, c, r) }, 
				{ typeof(long[]), (a, g, c, r) => ((long[])a).ReverseGroups(g, c, r) }, 
				{ typeof(ulong[]), (a, g, c, r) => ((ulong[])a).ReverseGroups(g, c, r) }, 
				{ typeof(float[]), (a, g, c, r) => ((float[])a).ReverseGroups(g, c, r) }, 
				{ typeof(double[]), (a, g, c, r) => ((double[])a).ReverseGroups(g, c, r) }, 
				{ typeof(C3b[]), (a, g, c, r) => ((C3b[])a).ReverseGroups(g, c, r) },
				{ typeof(C3us[]), (a, g, c, r) => ((C3us[])a).ReverseGroups(g, c, r) },
				{ typeof(C3ui[]), (a, g, c, r) => ((C3ui[])a).ReverseGroups(g, c, r) },
				{ typeof(C3f[]), (a, g, c, r) => ((C3f[])a).ReverseGroups(g, c, r) },
				{ typeof(C3d[]), (a, g, c, r) => ((C3d[])a).ReverseGroups(g, c, r) },
				{ typeof(C4b[]), (a, g, c, r) => ((C4b[])a).ReverseGroups(g, c, r) },
				{ typeof(C4us[]), (a, g, c, r) => ((C4us[])a).ReverseGroups(g, c, r) },
				{ typeof(C4ui[]), (a, g, c, r) => ((C4ui[])a).ReverseGroups(g, c, r) },
				{ typeof(C4f[]), (a, g, c, r) => ((C4f[])a).ReverseGroups(g, c, r) },
				{ typeof(C4d[]), (a, g, c, r) => ((C4d[])a).ReverseGroups(g, c, r) },
				{ typeof(V2i[]), (a, g, c, r) => ((V2i[])a).ReverseGroups(g, c, r) },
				{ typeof(V2l[]), (a, g, c, r) => ((V2l[])a).ReverseGroups(g, c, r) },
				{ typeof(V2f[]), (a, g, c, r) => ((V2f[])a).ReverseGroups(g, c, r) },
				{ typeof(V2d[]), (a, g, c, r) => ((V2d[])a).ReverseGroups(g, c, r) },
				{ typeof(V3i[]), (a, g, c, r) => ((V3i[])a).ReverseGroups(g, c, r) },
				{ typeof(V3l[]), (a, g, c, r) => ((V3l[])a).ReverseGroups(g, c, r) },
				{ typeof(V3f[]), (a, g, c, r) => ((V3f[])a).ReverseGroups(g, c, r) },
				{ typeof(V3d[]), (a, g, c, r) => ((V3d[])a).ReverseGroups(g, c, r) },
				{ typeof(V4i[]), (a, g, c, r) => ((V4i[])a).ReverseGroups(g, c, r) },
				{ typeof(V4l[]), (a, g, c, r) => ((V4l[])a).ReverseGroups(g, c, r) },
				{ typeof(V4f[]), (a, g, c, r) => ((V4f[])a).ReverseGroups(g, c, r) },
				{ typeof(V4d[]), (a, g, c, r) => ((V4d[])a).ReverseGroups(g, c, r) },
			};

		public static void ReverseGroups(
				this Array array, int[] groupArray, int groupCount,
				Func<int, bool> reverseMap)
		{
			ReverseGroupsMap[array.GetType()](
					array, groupArray, groupCount, reverseMap);
		}

		/// <summary>
		/// Apply a supplied function to each element of an array, with
		/// supplied conversion functions if the array is of a different
		/// type.
		/// </summary>
		public static Array Apply<T0, T1>(
			this Array array, Func<T1, T1> fun,
			Func<T0, T1> funT1ofT0, Func<T1, T0> funT0ofT1)
		{
			int length = array.Length;
			var t0a = array as T0[];
			if (t0a != null)
			{
				for (int i = 0; i < length; i++)
					t0a[i] = funT0ofT1(fun(funT1ofT0(t0a[i])));
				return array;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				for (int i = 0; i < length; i++)
					t1a[i] = fun(t1a[i]);
				return array;
			}
			throw new InvalidOperationException();
		}

		public static IEnumerable<T1> Elements<T0, T1>(
			this Array array, Func<T0, T1> convert)
		{
			var t0a = array as T0[];
			if (t0a != null)
			{
				foreach (var e in t0a) yield return convert(e); yield break;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				foreach (var e in t1a) yield return e; yield break;
			}
		}

		public static T1[] CopyAndConvert<T0, T1>(
			this Array array, Func<T0, T1> convert)
		{
			int length = array.Length;
			var result = new T1[length];
			var t0a = array as T0[];
			if (t0a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = convert(t0a[i]);
				return result;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = t1a[i];
				return result;
			}
			throw new InvalidOperationException();
		}

		public static Tr[] CopyAndConvert<T0, T1, Tr>(
			this Array array, Func<T0, T1> convert, Func<T1, Tr> fun)
		{
			int length = array.Length;
			var result = new Tr[length];
			var t0a = array as T0[];
			if (t0a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = fun(convert(t0a[i]));
				return result;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = fun(t1a[i]);
				return result;
			}
			throw new InvalidOperationException();
        }

        #endregion

        #region Comparable Array
        
		public static int IndexOfNSmallest<T>(this T[] a, int n)
            where T : IComparable<T>
        {
            if (n == 0) return a.IndexOfMin();
            var p = a.CreatePermutationQuickMedianAscending(n);
            return p[n];
        }

        public static int IndexOfNLargest<T>(this T[] a, int n)
            where T : IComparable<T>
        {
            if (n == 0) return a.IndexOfMax();
            var p = a.CreatePermutationQuickMedianDescending(n);
            return p[n];
        }
        
        #endregion
    }
}
