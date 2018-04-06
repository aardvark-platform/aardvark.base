using System;

namespace Aardvark.Base
{
    public static partial class FuncActionExtensions
    {
        public static void RunIfNotNull(this Action ax) => ax?.Invoke();

        public static void RunIfNotNull<T>(this Action<T> ax, T param) => ax?.Invoke(param);

        public static void RunIfNotNull<T1, T2>(this Action<T1, T2> ax, T1 param0, T2 param1) => ax?.Invoke(param0, param1);

        public static T RunIfNotNull<T>(this Func<T> f) => (f != null) ? f() : default(T);

        public static T RunIfNotNull<T>(this Func<T> f, T defaultResult) => (f != null) ? f() : defaultResult;

        public static T2 RunIfNotNull<T1, T2>(this Func<T1, T2> f, T1 param) => (f != null) ? f(param) : default(T2);

        public static T2 RunIfNotNull<T1, T2>(this Func<T1, T2> f, T1 param, T2 defaultResult)
            => (f != null) ? f(param) : defaultResult;

        public static T3 RunIfNotNull<T1, T2, T3>(this Func<T1, T2, T3> f, T1 param0, T2 param1)
            => (f != null) ? f(param0, param1) : default(T3);

        public static T3 RunIfNotNull<T1, T2, T3>(this Func<T1, T2, T3> f, T1 param0, T2 param1, T3 defaultResult)
            => (f != null) ? f(param0, param1) : defaultResult;

        /// <summary>
        /// Perform the specified action with the supplied object if the object is not null.
        /// </summary>
        public static void TryDo<T>(this T o, Action<T> ax)
            where T : class
        {
            if (o != null) ax(o);
        }

        /// <summary>
        /// Encapsulates the expression "[object] != null ? 'select something from [object]' : defaultValue
        /// </summary>
        public static Tr TrySelect<To, Tr>(this To o, Func<To, Tr> selector, Tr defaultValue = default(Tr))
            where To : class => o != null ? selector(o) : defaultValue;

        /// <summary>
        /// Performs a computaion with an object
        /// 
        /// This is useful if the object is required multiple times in the computation:
        ///     e.g.: x.Func(o => o > 0 ? -o : o++)
        /// </summary>
        public static Tr Func<To, Tr>(this To o, Func<To, Tr> selector) => selector(o);
    }
}
