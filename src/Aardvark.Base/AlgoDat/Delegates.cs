using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{

    public delegate void ActionValRef<T0, T1>(T0 a0, ref T1 a1);
    public delegate void ActionRefValVal<T0, T1, T2>(ref T0 a0, T1 a1, T2 a2);

    public static class LambdaApplicationExtensions
    {
        public static bool ApplyByType<T, T0>(
            this T obj, Action<T0> proc0)
            where T0 : class
        {
            T0 a0 = obj as T0; if (a0 != null) { proc0(a0); return true; }
            return false;
        }

        public static void ApplyByTypeStrict<T, T0>(
            this T obj, Action<T0> proc0)
            where T0 : class
        {
            T0 a0 = obj as T0; if (a0 != null) { proc0(a0); return; }
            throw new ArgumentException();
        }

        public static bool ApplyByType<T, T0, T1>(
            this T obj, Action<T0> proc0, Action<T1> proc1)
            where T0 : class
            where T1 : class
        {
            T0 a0 = obj as T0; if (a0 != null) { proc0(a0); return true; }
            T1 a1 = obj as T1; if (a1 != null) { proc1(a1); return true; }
            return false;
        }

        public static void ApplyByTypeStrict<T, T0, T1>(
            this T obj, Action<T0> proc0, Action<T1> proc1)
            where T0 : class
            where T1 : class
        {
            T0 a0 = obj as T0; if (a0 != null) { proc0(a0); return; }
            T1 a1 = obj as T1; if (a1 != null) { proc1(a1); return; }
            throw new ArgumentException();
        }

        public static Tr ApplyByTypeStrict<T, T0, Tr>(
            this T obj, Func<T0, Tr> fun0)
            where T0 : class
        {
            T0 a0 = obj as T0; if (a0 != null) return fun0(a0);
            throw new ArgumentException();
        }

        public static Tr ApplyByTypeStrict<T, T0, T1, Tr>(
            this T obj, Func<T0, Tr> fun0, Func<T1, Tr> fun1)
            where T0 : class
            where T1 : class
        {
            T0 a0 = obj as T0; if (a0 != null) return fun0(a0);
            T1 a1 = obj as T1; if (a1 != null) return fun1(a1);
            throw new ArgumentException();
        }
    }
}
