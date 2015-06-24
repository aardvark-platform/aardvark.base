using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static partial class FuncActionExtensions
    {
        //# Action comma = () => Out(", ");
        //# for (int tc = 1; tc <= 16; tc++) {
        //#   var Ti = tc.Expand(i => "T" + i).Join(", ");
        //#   var ei = tc.Expand(i => "e" + i).Join(", ");
        public static TR ExecuteFirst<__Ti__, TR>(this Func<__Ti__, TR>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
        {
            if (funArray == null) Report.Warn("no function registered");
            return funArray[0](__ei__);
        }

        public static bool ExecuteUpToTrue<__Ti__>(this Func<__Ti__, bool>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
        {
            if (funArray == null) Report.Warn("no function registered");
            for (int i = 0; i < funArray.Length; i++)
                if (funArray[i](__ei__)) return true;
            return false;
        }

        public static bool ExecuteUpToTrueChecked<__Ti__>(this Func<__Ti__, bool>[] funArray, /*# tc.ForEach(i=>{*/T__i__ e__i__/*#}, comma); */)
        {
            if (funArray == null) Report.Warn("no function registered");
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
            if (funArray == null) Report.Warn("no function registered");
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
            if (funArray == null) Report.Warn("no function registered");
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
}
