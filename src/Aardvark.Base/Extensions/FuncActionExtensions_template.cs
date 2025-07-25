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
    //# Action comma = () => Out(", ");
    //# for (int tc = 1; tc <= 16; tc++) {
    //#   var Ti = tc.Expand(i => "T" + i).Join(", ");
    //#   var ei = tc.Expand(i => "e" + i).Join(", ");
    public static TR ExecuteFirst<__Ti__, TR>(this Func<__Ti__, TR>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
    {
        return funArray[0](__ei__);
    }

    public static bool ExecuteUpToTrue<__Ti__>(this Func<__Ti__, bool>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
    {
        for (int i = 0; i < funArray.Length; i++)
            if (funArray[i](__ei__)) return true;
        return false;
    }

    public static bool ExecuteUpToTrueChecked<__Ti__>(this Func<__Ti__, bool>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                if (funArray[i](__ei__)) return true;
            }
            catch (Exception) { }
        }
        return false;
    }

    public static TR ExecuteUpToNotNull<__Ti__, TR>(this Func<__Ti__, TR>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            var r = funArray[i](__ei__);
            if (r != null) return r;
        }
        return null;
    }

    public static TR ExecuteUpToNotNullChecked<__Ti__, TR>(this Func<__Ti__, TR>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
        where TR : class
    {
        for (int i = 0; i < funArray.Length; i++)
        {
            try
            {
                var r = funArray[i](__ei__);
                if (r != null) return r;
            }
            catch (Exception) { }
        }
        return null;
    }

    //# } // tc
}
