using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    public static class ArrayUnsafeCoerceExtensions
    {
        private static readonly MethodInfo unsafeSizeOfMeth = typeof(Unsafe).GetMethod(nameof(Unsafe.SizeOf), BindingFlags.Public | BindingFlags.Static);

        private static readonly ConcurrentDictionary<Type, int> unsafeTypeSizes = new();

        /// <summary>
        /// Returns the managed size of the given type.
        /// </summary>
        public static int GetCLRSize(this Type t)
        {
            return unsafeTypeSizes.GetOrAdd(t, t =>
            {
                var mi = unsafeSizeOfMeth.MakeGenericMethod(t);
                return (int)mi.Invoke(null, null);
            });
        }
    }
}
