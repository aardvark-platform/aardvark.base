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

namespace Aardvark.Base;

public static partial class FuncActionExtensions
{
    public static TR ExecuteFirst<T0, TR>(this Func<T0, TR>[] funArray, T0 e0)
    {
        return funArray[0](e0);
    }

    public static bool ExecuteUpToTrue<T0>(this Func<T0, bool>[] funArray, T0 e0)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0>(this Func<T0, bool>[] funArray, T0 e0)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, TR>(this Func<T0, TR>[] funArray, T0 e0)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, TR>(this Func<T0, TR>[] funArray, T0 e0)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, TR>(this Func<T0, T1, TR>[] funArray, T0 e0, T1 e1)
    {
        return funArray[0](e0, e1);
    }

    public static bool ExecuteUpToTrue<T0, T1>(this Func<T0, T1, bool>[] funArray, T0 e0, T1 e1)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1>(this Func<T0, T1, bool>[] funArray, T0 e0, T1 e1)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, TR>(this Func<T0, T1, TR>[] funArray, T0 e0, T1 e1)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, TR>(this Func<T0, T1, TR>[] funArray, T0 e0, T1 e1)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, TR>(this Func<T0, T1, T2, TR>[] funArray, T0 e0, T1 e1, T2 e2)
    {
        return funArray[0](e0, e1, e2);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2>(this Func<T0, T1, T2, bool>[] funArray, T0 e0, T1 e1, T2 e2)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2>(this Func<T0, T1, T2, bool>[] funArray, T0 e0, T1 e1, T2 e2)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, TR>(this Func<T0, T1, T2, TR>[] funArray, T0 e0, T1 e1, T2 e2)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, TR>(this Func<T0, T1, T2, TR>[] funArray, T0 e0, T1 e1, T2 e2)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, TR>(this Func<T0, T1, T2, T3, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3)
    {
        return funArray[0](e0, e1, e2, e3);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3>(this Func<T0, T1, T2, T3, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3>(this Func<T0, T1, T2, T3, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, TR>(this Func<T0, T1, T2, T3, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, TR>(this Func<T0, T1, T2, T3, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, TR>(this Func<T0, T1, T2, T3, T4, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4)
    {
        return funArray[0](e0, e1, e2, e3, e4);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4>(this Func<T0, T1, T2, T3, T4, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4>(this Func<T0, T1, T2, T3, T4, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, TR>(this Func<T0, T1, T2, T3, T4, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, TR>(this Func<T0, T1, T2, T3, T4, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, TR>(this Func<T0, T1, T2, T3, T4, T5, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5>(this Func<T0, T1, T2, T3, T4, T5, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5>(this Func<T0, T1, T2, T3, T4, T5, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, TR>(this Func<T0, T1, T2, T3, T4, T5, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, TR>(this Func<T0, T1, T2, T3, T4, T5, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6>(this Func<T0, T1, T2, T3, T4, T5, T6, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6>(this Func<T0, T1, T2, T3, T4, T5, T6, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    public static TR ExecuteFirst<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14, T15 e15)
    {
        return funArray[0](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15);
    }

    public static bool ExecuteUpToTrue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14, T15 e15)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14, T15 e15)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14, T15 e15)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(this Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>[] funArray, T0 e0, T1 e1, T2 e2, T3 e3, T4 e4, T5 e5, T6 e6, T7 e7, T8 e8, T9 e9, T10 e10, T11 e11, T12 e12, T13 e13, T14 e14, T15 e15)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

}
