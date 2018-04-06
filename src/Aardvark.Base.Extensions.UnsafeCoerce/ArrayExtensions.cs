using System;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    public static class ArrayUnsafeCoerceExtensions
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
            if (!s_typeIds.TryGetValue(typeof(T), out IntPtr typeId))
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
    }
}
