using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Aardvark.Base.Sorting
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    public static class SortingExtensions
    {
        #region Constants

        /// Use insertion sort if count smaller than this (quicksort, quickmedian).
        private const int c_insertionSortThreshold = 31;
        private const int c_insertionMedianThreshold = 7;

        private const int c_minMerge = 32;
        private const int c_minGallop = 7;
        private const int c_initialTmpStorageLength = 256;

        #endregion
        /*CLASS#

        public struct Opt
        {
            public bool Generic;
            public bool Get;
            public bool Fun;
            public bool Perm;
            public bool Sel;
        }

        public static string OpFilter(string s)
        {
            if (s == " < ") return " > ";
            if (s == " > ") return " < ";
            if (s == " <= ") return " >= ";
            if (s == " >= ") return " <= ";
            return s;
        }
        
        public static void Cmp(
                Opt opt, Action<string> pr, bool asc,
                string c, Action<int> a, Action<int> o, Action<int> b)
        {
            if (!asc) Filter = OpFilter;
            if (opt.Generic && !opt.Sel) { if (opt.Fun) { pr(c); pr("("); } }
            if (opt.Sel) pr("selector(");
            if (opt.Perm) {  if (opt.Get) pr("aget(a,"); else pr("a["); }
            a(0);
            if (opt.Perm) { if (opt.Get) pr(")"); else  pr("]"); }
            if (opt.Sel) pr(")");
            if (opt.Generic && !opt.Sel) { if (opt.Fun) pr(", "); else { pr("."); pr(c); pr("("); } } else  o(0);          
            if (opt.Sel) pr("selector(");
            if (opt.Perm) {  if (opt.Get) pr("aget(a,"); else pr("a["); }
            b(0);
            if (opt.Perm) { if (opt.Get) pr(")"); else  pr("]"); }
            if (opt.Sel) pr(")");
            if (opt.Generic && !opt.Sel) { pr(")"); o(0); pr("0"); }
            if (!asc) Filter = null;
        }
        */
        //# string add = " + ", sub = " - ", inc = "++", dec = "--";
        //# string add1 = " + 1", add2 = " + 2", sub1 = " - 1";
        //# var mathtypes = Meta.VecFieldTypes.Select(t => t.Name);
        //# var generictypes = new [] { "T[]", "List<T>", "TArray" };
        //# foreach (var isGeneric in new[] { false, true }) {
        //#     var types = isGeneric ? generictypes : mathtypes;
        //# foreach (var isNonGenericList in new[] { false, true }) { if (isGeneric && isNonGenericList) continue;
        //# foreach (var type in types) {
        //#     var etype = isGeneric ? "T" : type;
        //#     var atype = isGeneric ? type : (isNonGenericList ? "List<" + type + ">" : type + "[]");
        //#     var hasGet = atype == "TArray";
        //#     var isArray = atype.EndsWith("[]");
        //#     var isList = atype.EndsWith(">");
        //# foreach (var isPerm in new[] { false, true }) {
        //#     var permStr = isPerm ? "Perm" : "";
        //#     var p = isPerm ? "p[" : (hasGet ? "aget(a, " : "a[");
        //#     var pt = isPerm ? "t[" : (hasGet ? "aget(t, " : "t[");
        //#     var q = isPerm ? "]" : (hasGet ? ")" : "]");
        //#     var P = isPerm ? "p[" : (hasGet ? "aset(a, " : "a[");
        //#     var I = isPerm ? "] = " : (hasGet ? ", " : "] = ");
        //#     var Q = isPerm ? "" : (hasGet ? ")" : "");
        //#     var pa = isPerm ? "p" : "a";
        //#     var ta = isPerm ? "a" : "t";
        //#     var hasSet = hasGet && !isPerm;
        //#     var copy = (isList || hasGet) && !isPerm ? "CopyElements" : "Array.Copy";
        //#     var agetset = hasSet ? "aget, aset, " : "";
        //#     var permName = isPerm ? "Permutation" : "";
        //#     var permNameSpace = isPerm ? "Permutation " : "";
        #region __permNameSpace__Sort/Median for __atype__

        //# foreach (var hasFun in new[] { true, false }) { if (hasFun && !isGeneric) continue;
        //#     var isComparable = isGeneric && !hasFun;
        //# foreach (var hasSel in new[] { false, true }) {
        //#     if (hasSel && hasFun) continue;
        //#     if (hasSel && !isGeneric) continue;
        //#     if (hasSel) isComparable = false;
        //# foreach (var ctype in mathtypes) { if (ctype != "int" && !hasSel) continue;
        //#     var ctypecap = ctype.Capitalized();
        //# foreach (var isAsc in new[] { true, false }) { if (hasFun && !isAsc) continue;
        //# foreach (var itype in new[] { "int", "long" }) { if (isList && itype == "long") continue;
        //#     if (!isList && !isPerm && itype == "int") continue;
        //#     var itypecast = isList ? "(" + itype + ")" : "";
        //#     var stype = isPerm ? itype : etype; // sort type
        //#     var tatype = hasSet ? "TArray" : stype + "[]"; // temp array type
        //#     var pdef = isPerm ? "-1" : "default("+stype+")";
        //#     var isLong = itype == "long";
        //#     var makeCreator = isPerm && (isArray || (isList && !isLong));
        //#     var longStr = isLong ? "Long" : "";
        //#     var agetcmp = (hasGet ? "aget, " : "") + (hasFun ? "cmp, " : "") + (hasSel ? "selector, " : "");
        //#     var alen = isPerm ? "p." + longStr + "Length" :
        //#                 (hasGet ? "count" : (isList ? "a.Count" : "a." + longStr + "Length"));
        //#     var getLength = isArray ? longStr + "Length" : (isList ? "Count" : "");
        //#     var opt = new Opt { Generic = isGeneric, Get = hasGet, Fun = hasFun, Perm = isPerm, Sel = hasSel };
        //#     var ext = (isGeneric && hasFun) ? "" : (isAsc ? "Ascending" : "Descending");
        //#     var rext = (isGeneric && hasFun) ? "" : (isAsc ? "Descending" : "Ascending");
        //#     var uid = (isGeneric ? "" : type.Capitalized()) +  (isList ? "List" : "") + permStr + longStr
        //#                 +  (hasFun ? "Cmp" : isAsc ? "Asc" : "Desc");
        //#     var c = hasFun ? "cmp" : "CompareTo";
        //#     Action<Action<int>,Action<int>,Action<int>> cmp = (a,o,b) => Cmp(opt, Out, isAsc, c, a, o, b);
        //# foreach (var algo in new[] { "Median", "Sort" }) {
        //#     var isSort = algo == "Sort"; var isMedian = algo == "Median";
        //#     var qname = "Quick" + algo;
        //#     var qcall = permName + qname + ext;
        //# if (makeCreator) {
        /// <summary>
        //# if (isSort) {
        /// The CreatePermutationQuickSort algorithm returns an array of
        /// element indices p that reference the elements in the data array
        /// in sorted a manner.
        //# } else {
        /// The CreatePermutationQuickMedian algorithm returns an array of
        /// element indices p that reference the elements in the data array
        /// in a partially sorted manner, such that p[med] is placed at the
        /// correct position, and correctly partitions the array into larger
        /// and smaller elements.
        //# }
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static __itype__[] Create__qcall____longStr__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this __atype__ a/*# if (hasGet) { */, Func<TArray, long, T> aget/*# }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } if (isMedian) { */, __itype__ med/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ len = a.__getLength__;
            var p = new __itype__[len].SetByIndex__longStr__(i => i);
            __qcall__(p, a, /*# if (hasGet) { */aget, /*# } if (hasFun) { */cmp, /*#
                        } if (hasSel) { */selector, /*# } */0, len - 1, len - 1/*# if (isMedian) { */, med/*# } */);
            return p;
        }

        //# }
        /// <summary>
        //# if (isSort) {
        //# if (isPerm) {
        /// The PermutationQuickSort algorithm sorts the array of element
        /// indices p that reference the elements in the data array.
        //# } else {
        /// The QuickSort algorithm sorts the data array.
        //# }
        //# } else {
        //# if (isPerm) {
        /// The PermutationQuickMedian algorithm partially sorts the array of
        /// element indices p that reference the elements in the data array
        /// in such a way that p[med] is placed at the correct position.
        //# } else {
        /// The QuickMedian algorithm partially sorts the data array in such
        /// a way that the element at position med is correctly placed, and
        /// correctly partitions the array into larger and smaller elements.
        //# }
        //# }
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void __qcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a/*# if (hasGet) { */, /*# if (!isPerm) { */__itype__ count, /*# } */Func<TArray, long, T> aget/*# if (!isPerm) { */, Action<TArray, long, T> aset/*# } }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } if (isMedian) { */, __itype__ med/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ last = __alen__ - 1;
            __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*#
                        } if (hasSel) { */selector, /*# } */0, last, last/*# if (isMedian) { */, med/*# } */);
        }

        /// <summary>
        //# if (isSort) {
        //# if (isPerm) {
        /// The PermutationQuickSort algorithm sorts the range [beginIncl,
        /// endExcl) of the array of element indices p that reference the
        /// elements in the data array.
        //# } else {
        /// The QuickSort algorithm sorts the range [beginIncl,
        /// endExcl) of the array of elements in the data array.
        //# }
        //# } else {
        //# if (isPerm) {
        /// The PermutationQuickMedian algorithm partially sorts the range
        /// [beginIncl, endExcl) of the array of element indices p
        /// that reference the elements in the data array in such a way that
        /// p[med] is placed at the correct position, and correctly partitions
        /// this array range into larger and smaller elements.
        //# } else {
        /// The QuickMedian algorithm partially sorts the range [beginIncl,
        /// endExcl) of the data array in such a way that the element at
        /// position med is correctly placed, and correctly partitions this
        /// array range into larger and smaller elements.
        //# }
        //# }
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void __qcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __itype__ beginIncl, __itype__ endExcl/*# if (isMedian) { */, __itype__ med/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */beginIncl, endExcl - 1, endExcl - beginIncl - 1/*# if (isMedian) { */, med/*# } */);
        }

        /*
            This implementation of __qname__ is based on the paper
            "Dual-Pivot Quicksort" by Vladimir Yaroslavskiy <iaroslavski@mail.ru>.
        */
        private static void __qcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __itype__ l, __itype__ r, __itype__ countSub1/*# if (isMedian) { */, __itype__ med/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            while (countSub1 >= c_insertion__algo__Threshold)
            {
                __itype__ sixth = (1 + countSub1) / 6;
                __itype__ e1 = l + sixth;
                __itype__ e5 = r - sixth;
                __itype__ e3 = (l + r) >> 1;
                __itype__ e4 = e3 + sixth;
                __itype__ e2 = e3 - sixth;

                if (/*#cmp(a=>{*/__p__e1__q__/*#},o=>{*/ > /*#},b=>{*/__p__e2__q__/*#});*/) { var t = __p__e1__q__; __P__e1__I____p__e2__q____Q__; __P__e2__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e4__q__/*#},o=>{*/ > /*#},b=>{*/__p__e5__q__/*#});*/) { var t = __p__e4__q__; __P__e4__I____p__e5__q____Q__; __P__e5__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e1__q__/*#},o=>{*/ > /*#},b=>{*/__p__e3__q__/*#});*/) { var t = __p__e1__q__; __P__e1__I____p__e3__q____Q__; __P__e3__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e2__q__/*#},o=>{*/ > /*#},b=>{*/__p__e3__q__/*#});*/) { var t = __p__e2__q__; __P__e2__I____p__e3__q____Q__; __P__e3__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e1__q__/*#},o=>{*/ > /*#},b=>{*/__p__e4__q__/*#});*/) { var t = __p__e1__q__; __P__e1__I____p__e4__q____Q__; __P__e4__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e3__q__/*#},o=>{*/ > /*#},b=>{*/__p__e4__q__/*#});*/) { var t = __p__e3__q__; __P__e3__I____p__e4__q____Q__; __P__e4__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e2__q__/*#},o=>{*/ > /*#},b=>{*/__p__e5__q__/*#});*/) { var t = __p__e2__q__; __P__e2__I____p__e5__q____Q__; __P__e5__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e2__q__/*#},o=>{*/ > /*#},b=>{*/__p__e3__q__/*#});*/) { var t = __p__e2__q__; __P__e2__I____p__e3__q____Q__; __P__e3__I__t__Q__; }
                if (/*#cmp(a=>{*/__p__e4__q__/*#},o=>{*/ > /*#},b=>{*/__p__e5__q__/*#});*/) { var t = __p__e4__q__; __P__e4__I____p__e5__q____Q__; __P__e5__I__t__Q__; }

                var p1 = __p__e2__q__; __P__e2__I____p__l__q____Q__;
                var p2 = __p__e4__q__; __P__e4__I____p__r__q____Q__;

                __itype__ lo = l + 1;
                __itype__ hi = r - 1;

                bool pivotsDiffer = /*#cmp(a=>{*/p1/*#},o=>{*/ != /*#},b=>{*/p2/*#});*/;

                if (pivotsDiffer)
                {
                    for (__itype__ i = lo; i <= hi; i++)
                    {
                        var ai = __p__i__q__;
                        if (/*#cmp(a=>{*/ai/*#},o=>{*/ < /*#},b=>{*/p1/*#});*/) { __P__i__I____p__lo__q____Q__; __P__lo__I__ai__Q__; ++lo; }
                        else if (/*#cmp(a=>{*/ai/*#},o=>{*/ > /*#},b=>{*/p2/*#});*/)
                        {
                            while (/*#cmp(a=>{*/__p__hi__q__/*#},o=>{*/ > /*#},b=>{*/p2/*#});*/ && i < hi)
                                --hi;
                            __P__i__I____p__hi__q____Q__; __P__hi__I__ai__Q__; --hi; ai = __p__i__q__;
                            if (/*#cmp(a=>{*/ai/*#},o=>{*/ < /*#},b=>{*/p1/*#});*/) { __P__i__I____p__lo__q____Q__; __P__lo__I__ai__Q__; ++lo; }
                        }
                    }
                }
                else
                {
                    for (__itype__ i = lo; i <= hi; i++)
                    {
                        var ai = __p__i__q__;
                        if (/*#cmp(a=>{*/ai/*#},o=>{*/ == /*#},b=>{*/p1/*#});*/) continue;
                        if (/*#cmp(a=>{*/ai/*#},o=>{*/ < /*#},b=>{*/p1/*#});*/) { __P__i__I____p__lo__q____Q__; __P__lo__I__ai__Q__; ++lo; }
                        else
                        {
                            while (/*#cmp(a=>{*/__p__hi__q__/*#},o=>{*/ > /*#},b=>{*/p1/*#});*/)
                                --hi;
                            __P__i__I____p__hi__q____Q__; __P__hi__I__ai__Q__; --hi; ai = __p__i__q__;
                            if (/*#cmp(a=>{*/ai/*#},o=>{*/ < /*#},b=>{*/p1/*#});*/) { __P__i__I____p__lo__q____Q__; __P__lo__I__ai__Q__; ++lo; }
                        }
                    }
                }

                __P__l__I____p__lo__sub__1__q____Q__; __P__lo__sub__1__I__p1__Q__;
                __P__r__I____p__hi__add__1__q____Q__; __P__hi__add__1__I__p2__Q__;

                __itype__ cl = lo - 2 - l;
                __itype__ cr = r - hi - 2;

                if (pivotsDiffer)
                {
                    if (lo < e1 && e5 < hi)
                    {
                        //# if (isSort) {
                        __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */l, lo - 2, cl);
                        __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */hi + 2, r, cr);
                        //# } // isSort
                        //# if (isMedian) {
                        if (med <= lo - 2) { r = lo - 2; countSub1 = cl; }
                        else if (med >= hi + 2) { l = hi + 2; countSub1 = cr; }
                        else
                        //# } // isMedian
                        {
                            while (/*#cmp(a=>{*/__p__lo__q__/*#},o=>{*/ == /*#},b=>{*/p1/*#});*/)
                                ++lo;
                            for (__itype__ i = lo + 1; i <= hi; i++)
                            {
                                if (/*#cmp(a=>{*/__p__i__q__/*#},o=>{*/ == /*#},b=>{*/p1/*#});*/) { /*# if (isGeneric || isPerm) { */p1 = __p__i__q__; /*# } */__P__i__I____p__lo__q____Q__; __P__lo__I__p1__Q__; ++lo; }
                            }
                            while (/*#cmp(a=>{*/__p__hi__q__/*#},o=>{*/ == /*#},b=>{*/p2/*#});*/)
                                --hi;
                            for (__itype__ i = hi - 1; i >= lo; i--)
                            {
                                if (/*#cmp(a=>{*/__p__i__q__/*#},o=>{*/ == /*#},b=>{*/p2/*#});*/) { /*# if (isGeneric || isPerm) { */p2 = __p__i__q__; /*# } */__P__i__I____p__hi__q____Q__; __P__hi__I__p2__Q__; --hi; }
                            }
                            //# if (isMedian) {
                            if (med < lo || med > hi) return;
                            //# } // isMedian
                            l = lo; r = hi; countSub1 = hi - lo;
                        }
                    }
                    else
                    {
                        //# if (isSort) {
                        __itype__ cm = hi - lo;
                        if (cl > cr)
                        {
                            if (cl > cm)
                            {
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */lo, hi, cm);
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */hi + 2, r, cr);
                                r = lo - 2; countSub1 = cl;
                            }
                            else
                            {
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */l, lo - 2, cl);
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */hi + 2, r, cr);
                                l = lo; r = hi; countSub1 = cm;
                            }
                        }
                        else
                        {
                            if (cr > cm)
                            {
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */l, lo - 2, cl);
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */lo, hi, cm);
                                l = hi + 2; countSub1 = cr;
                            }
                            else
                            {
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */l, lo - 2, cl);
                                __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */hi + 2, r, cr);
                                l = lo; r = hi; countSub1 = cm;
                            }
                        }
                        //# } // isSort
                        //# if (isMedian) {
                        if (med <= lo - 2) { r = lo - 2; countSub1 = cl; }
                        else if (med >= hi + 2) { l = hi + 2; countSub1 = cr; }
                        else if (med >= lo && med <= hi) { l = lo; r = hi; countSub1 = hi - lo; }
                        else return;
                        //# } // isMedian
                    }
                }
                else
                {
                    //# if (isSort) {
                    if (cl > cr)
                    {
                        __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */hi + 2, r, cr);
                        r = lo - 2; countSub1 = cl;
                    }
                    else
                    {
                        __qcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */l, lo - 2, cl);
                        l = hi + 2; countSub1 = cr;
                    }
                    //# } // isSort
                    //# if (isMedian) {
                    if (med <= lo - 2) { r = lo - 2; countSub1 = cl; }
                    else if (med >= hi + 2) { l = hi + 2; countSub1 = cr; }
                    else return;
                    //# } // isMedian
                }
            }

            for (__itype__ i = l + 1; i <= r; i++)
            {
                var ai = __p__i__q__; __itype__ j;
                for (j = i - 1; j >= l && /*#cmp(a=>{*/ai/*#},o=>{*/ < /*#},b=>{*/__p__j__q__/*#});*/; j--)
                    __P__j__add__1__I____p__j__q____Q__;
                __P__j__add__1__I__ai__Q__;
            }
        }

        //# } // algo
        //# var hcall = permName + "HeapSort" + ext;
        //# if (makeCreator) {
        /// <summary>
        /// The PermutationHeapSort algorithm returns an array of element
        /// indices that enumerate the array elements in sorted fashion.
        /// It is slower than the PermutationQuickSort algorithm, but
        /// is not affected by the data distribution in any way, even if
        /// there are a huge number of equal data elements.
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static __itype__[] Create__hcall____longStr__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this __atype__ a/*# if (hasGet) { */, Func<TArray, long, T> aget/*# }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ len = a.__getLength__;
            var p = new __itype__[len].SetByIndex__longStr__(i => i);
            __hcall__(p, a/*# if (hasGet) { */, aget/*# } if (hasFun) { */, cmp/*# } if (hasSel) { */, selector/*# } */);
            return p;
        }

        //# }
        //# foreach (var range in new[] { true, false }) { var r = range ? " + begin" : "";
        /// <summary>
        //# if (isPerm) {
        /// The PermutationHeapSort algorithm sorts the array of element
        /// indices p that reference the elements in the data array.
        //# } else {
        /// The HeapSort algorithm sorts the data array.
        //# }
        /// It is slower than the __permName__QuickSort algorithm,
        /// but is not affected by the data distribution in any
        /// way, even if there are a huge number of equal data elements.
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void __hcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a/*# if (hasGet) { */, /*# if (!isPerm && !range) { */__itype__ count, /*# } */Func<TArray, long, T> aget/*# if (!isPerm) { */, Action<TArray, long, T> aset/*# } }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } if (range) { */, __itype__ begin, __itype__ end/*# }*/)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            //# if (alen != "count" && !range) {
            __itype__ count = __alen__;
            //# }
            //# if (range) {
            __itype__ count = end - begin;
            //# }
            for (__itype__ c = 1; c < count; c++)
            {
                var element = __p__c__r____q__;
                __itype__ i = c;
                while (i > 0)
                {
                    __itype__ i2 = (i - 1) / 2;
                    if (/*#cmp(a=>{*/element/*#},o=>{*/ < /*#},b=>{*/__p__i2__r____q__/*#});*/)
                        break;
                    __P__i__r____I____p__i2__r____q____Q__;
                    i = i2;
                }
                __P__i__r____I__element__Q__;
            }
            while (count > 1)
            {
                --count;
                var element = __p__count__r____q__;
                __P__count__r____I____p__0__r____q____Q__;
                __itype__ i = 0, i1 = 1;
                while (i1 < count) // at least one child
                {
                    __itype__ i2 = i1 + 1;
                    __itype__ ni = (i2 < count // two children?
                              && /*#cmp(a=>{*/__p__i1__r____q__/*#},o=>{*/ < /*#},b=>{*/__p__i2__r____q__/*#});*/)
                                 ? i2 : i1; // smaller child
                    if (/*#cmp(a=>{*/__p__ni__r____q__/*#},o=>{*/ < /*#},b=>{*/element/*#});*/)
                        break;
                    __P__i__r____I____p__ni__r____q____Q__;
                    i = ni; i1 = 2 * i + 1;
                }
                __P__i__r____I__element__Q__;
            }
        }

        //# } // range
        //# var scall = permName + "SmoothSort" + ext;
        //# if (makeCreator) {
        /// <summary>
        /// The PermutationSmoothSort algorithm returns an array of element
        /// indices that enumerate the array elements in sorted fashion.
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static __itype__[] Create__scall____longStr__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this __atype__ a/*# if (hasGet) { */, Func<TArray, long, T> aget/*# }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
        //# }
        {
            __itype__ len = a.__getLength__;
            var p = new __itype__[len].SetByIndex__longStr__(i => i);
            __scall__(p, a/*# if (hasGet) { */, aget/*# } if (hasFun) { */, cmp/*# } if (hasSel) { */, selector/*# } */);
            return p;
        }

        //# }
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void __scall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a/*# if (hasGet) { */, /*# if (!isPerm) { */__itype__ count, /*# } */Func<TArray, long, T> aget/*# if (!isPerm) { */, Action<TArray, long, T> aset/*# } }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
        //# }
        {
            __scall__InclusiveRange(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */0, __alen__ - 1);
        }

        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void __scall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __itype__ beginIncl, __itype__ endExcl)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __scall__InclusiveRange(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */beginIncl, endExcl - 1);
        }

        private static void __scall__InclusiveRange/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __itype__ lo, __itype__ hi)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ head = lo;
            long pbits = 1;
            int pshift = 1;
            while (head < hi)
            {
                if ((pbits & 3) == 3)
                {
                    __scall__Sift(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */pshift, head);
                    pbits >>= 2; pshift += 2;
                }
                else
                {
                    if (LP__longStr__[pshift - 1] >= hi - head)
                        __scall__Trinkle(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */pbits, pshift, head, false);
                    else
                        __scall__Sift(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */pshift, head);
                    if (pshift == 1)
                    {
                        pbits <<= 1; pshift--;
                    }
                    else
                    {
                        pbits <<= (pshift - 1); pshift = 1;
                    }
                }
                pbits |= 1;
                head++;
            }

            __scall__Trinkle(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */pbits, pshift, head, false);

            while (pshift != 1 || pbits != 1)
            {
                if (pshift <= 1)
                {
                    int trail = TrailingZeroBitCount(pbits & ~1);
                    pbits >>= trail; pshift += trail;
                }
                else
                {
                    pbits <<= 2; pbits ^= 7; pshift -= 2;
                    __scall__Trinkle(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */pbits >> 1, pshift + 1, head - LP__longStr__[pshift] - 1, true);
                    __scall__Trinkle(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */pbits, pshift, head - 1, true);
                }
                head--;
            }
        }

        private static void __scall__Sift/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                int pshift, __itype__ head)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            var val = __p__head__q__;
            while (pshift > 1)
            {
                __itype__ rt = head - 1;
                __itype__ lf = head - 1 - LP__longStr__[pshift - 2];
                if (/*#cmp(a=>{*/val/*#},o=>{*/ >= /*#},b=>{*/__p__lf__q__/*#});*/ && /*#cmp(a=>{*/val/*#},o=>{*/ >= /*#},b=>{*/__p__rt__q__/*#});*/) break;
                if (/*#cmp(a=>{*/__p__lf__q__/*#},o=>{*/ >= /*#},b=>{*/__p__rt__q__/*#});*/)
                {
                    __P__head__I____p__lf__q____Q__;
                    head = lf;
                    pshift -= 1;
                }
                else
                {
                    __P__head__I____p__rt__q____Q__;
                    head = rt;
                    pshift -= 2;
                }
            }
            __P__head__I__val__Q__;
        }

        private static void __scall__Trinkle/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                long pbits, int pshift, __itype__ head, bool isTrusty)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            var val = __p__head__q__;

            while (pbits != 1)
            {
                __itype__ stepson = head - LP__longStr__[pshift];
                var vstepson = __p__stepson__q__;
                if (/*#cmp(a=>{*/vstepson/*#},o=>{*/ <= /*#},b=>{*/val/*#});*/) break;
                if (!isTrusty && pshift > 1)
                {
                    __itype__ rt = head - 1;
                    __itype__ lf = head - 1 - LP__longStr__[pshift - 2];
                    if (/*#cmp(a=>{*/__p__rt__q__/*#},o=>{*/ >= /*#},b=>{*/vstepson/*#});*/ || /*#cmp(a=>{*/__p__lf__q__/*#},o=>{*/ >= /*#},b=>{*/vstepson/*#});*/)
                        break;
                }
                __P__head__I__vstepson__Q__;
                head = stepson;
                int trail = TrailingZeroBitCount(pbits & ~1);
                pbits >>= trail;
                pshift += trail;
                isTrusty = false;
            }
            if (!isTrusty)
            {
                __P__head__I__val__Q__;
                __scall__Sift(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */pshift, head);
            }
        }

        //# var tsi = "TimSortInfo" + (isList ? "List" : "") + permStr + longStr
        //#            + (hasGet ? "Get" : "") + (hasFun ? "Cmp" : "") + (hasSel ? ctypecap + "Sel" : "");
        //# var mb = "m_";
        //# if (isGeneric && (hasFun || isAsc)) {
        public class __tsi__</*# if (hasGet) { */TArray, /*# } */T>
            //# if (!hasFun && !hasSel) {
            where T : IComparable<T>
            //# }
        {
            //# if (isPerm) {
            public __itype__[] m_p;
            //# }
            public __atype__ m_a;
            //# if (hasGet) {
            public __itype__ m_count;
            public Func<TArray, long, T> m_aget;
            //# if (!isPerm) {
            public Action<TArray, long, T> m_aset;
            public Func<long, TArray> m_amake;
            //# }
            //# }
            //# if (hasFun) {
            public Func<T, T, int> m_cmp;
            //# }
            //# if (hasSel) {
            public Func<T, __ctype__> m_sel;
            //# }
            public __itype__ m_minGallop = c_minGallop;
            //# if (hasSet) {
            public TArray m_tmp;
            public long m_len;
            //# } else {
            public __stype__[] m_tmp;
            //# }
            public int m_stackSize = 0;
            public __itype__[] m_runStart;
            public __itype__[] m_runLen;

            public __tsi__(/*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a/*# if (hasGet) { */, Func<TArray, long, T> aget/*# if (!isPerm) { */, Action<TArray, long, T> aset, Func<long, TArray> amake/*# } }
                            if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } if (hasGet) { */, __itype__ count/*# } */)
            {
                /*# if (isPerm) { */m_p = p; /*# } */m_a = a;/*# if (hasGet) { */ m_count = count;/*# } */
                /*# if (hasGet) { */m_aget = aget; /*# if (!isPerm) { */m_aset = aset; m_amake = amake; /*# } } if (hasFun) { */m_cmp = cmp;/*# } if (hasSel) { */m_sel = selector;/*# } */
                //# if (!hasGet) {
                __itype__ count = __alen__;
                //# }
                //# if (hasSet) {
                m_len = count < 2 * c_initialTmpStorageLength
                        ? count >> 1 : c_initialTmpStorageLength;
                m_tmp = amake(m_len);
                //# } else {
                m_tmp = new __stype__[count < 2 * c_initialTmpStorageLength
                                    ? count >> 1 : c_initialTmpStorageLength];
                //# }
                int stackLen = (count < 120 ? 5 :
                                count < 1542 ? 10 :
                                count < 119151 ? 19 : 40);
                m_runStart = new __itype__[stackLen];
                m_runLen = new __itype__[stackLen];
            }

            public void PushRun(__itype__ runStart, __itype__ runLen)
            {
                m_runStart[m_stackSize] = runStart;
                m_runLen[m_stackSize] = runLen;
                m_stackSize++;
            }

            public __tatype__ EnsureCapacity(__itype__ cap)
            {
                //# var tlen = hasSet ? "m_len" : "m_tmp." + longStr + "Length";
                if (__tlen__ < cap)
                {
                    __itype__ c = cap;
                    c |= c >> 1; c |= c >> 2; c |= c >> 4; c |= c >> 8;
                    c |= c >> 16;/*# if (isLong) { */ c |= c >> 32;/*# } */ c++;
                    //# if (hasSet) {
                    m_len = c < 0 ? cap : Math.Min(c, __mb____alen__ >> 1);
                    m_tmp = m_amake(m_len);
                    //# } else {
                    m_tmp = new __stype__[c < 0 ? cap : Math.Min(c, __mb____alen__ >> 1)];
                    //# }
                }
                return m_tmp;
            }

        }

        //# } // isGeneric && (hasFun || isAsc)
        //# var tcall = permName + "TimSort" + ext;
        //# var glcall = permName + "GallopLeft" + ext;
        //# var grcall = permName + "GallopRight" + ext;
        //# var bcall = permName + "BinarySort" + ext;
        //# var crcall = permName + "CountRunAndOrder" + ext;
        //# if (makeCreator) {
        /// <summary>
        /// The PermutationTimSort algorithm returns an array of element
        /// indices that enumerate the array elements in sorted fashion.
        /// The TimSort algorithm is not as fast as QuickSort for completely
        /// randomized arrays, but is a bit faster for mostly sorted or
        /// mostly reversed arrays. Additionally it is a stable algorithm.
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static __itype__[] Create__tcall____longStr__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this __atype__ a/*# if (hasGet) { */, Func<TArray, long, T> aget/*# }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ len = a.__getLength__;
            var p = new __itype__[len].SetByIndex__longStr__(i => i);
            __tcall__(p, a/*# if (hasGet) { */, aget/*# } if (hasFun) { */, cmp/*# } if (hasSel) { */, selector/*# } */);
            return p;
        }

        //# }
        /// <summary>
        /// The TimSort algorithm is not as fast as QuickSort for completely
        /// randomized arrays, but is a bit faster for mostly sorted or
        /// mostly reversed arrays. Additionally it is a stable algorithm.
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void __tcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a/*# if (hasGet) { */, /*# if (!isPerm) { */__itype__ count, /*# } */Func<TArray, long, T> aget/*# if (!isPerm) { */, Action<TArray, long, T> aset, Func<long, TArray> amake/*# } }
                if (hasFun) { */, Func<T, T, int> cmp/*# } if (hasSel) { */, Func<T, __ctype__> selector/*# } */)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __tcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, amake, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */0, __alen__);
        }

        /// <summary>
        /// The TimSort algorithm is not as fast as QuickSort for completely
        /// randomized arrays, but is a bit faster for mostly sorted or
        /// mostly reversed arrays. Additionally it is a stable algorithm.
        /// </summary>
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void __tcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset, Func<long, TArray> amake,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __itype__ lo, __itype__ hi)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            /*
                An implementation of the TimSort Algorithm which was invented
                by Tim Peters in 2002 for use in the Python programming language. 
            */
            __itype__ count = hi - lo;
            if (count < 2) return;
            if (count < c_minMerge)
            {
                var initRunLen = __crcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */lo, hi);
                __bcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */lo, hi, lo + initRunLen);
                return;
            }
            var ti = new __tsi__</*# if (hasGet) { */TArray, /*# } */__etype__>(/*# if (isPerm) { */p, /*# } */a/*# if (hasGet) { */, aget/*# if (!isPerm) { */, aset, amake/*# } }
                            if (hasFun) { */, cmp/*# } if (hasSel) { */, selector/*# } if (hasGet) { */, count/*# } */);
            __itype__ minRun = TimSortMinRunLength(count);
            do
            {
                __itype__ runLen = __crcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */lo, hi);
                if (runLen < minRun)
                {
                    __itype__ force = count <= minRun ? count : minRun;
                    __bcall__(/*# if (isPerm) { */p, /*# } */a, /*# if (hasGet) { */aget, /*# if (!isPerm) { */aset, /*# } } if (hasFun) { */cmp, /*# } if (hasSel) { */selector, /*# } */lo, lo + force, lo + runLen);
                    runLen = force;
                }
                ti.PushRun(lo, runLen);
                MergeCollapse__uid__(ti);
                lo += runLen; count -= runLen;
            }
            while (count != 0);
            MergeForceCollapse__uid__(ti);
        }

        private static void MergeCollapse__uid__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(this __tsi__</*# if (hasGet) { */TArray, /*# } */__etype__> ti)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            while (ti.m_stackSize > 1)
            {
                int n = ti.m_stackSize - 2;
                if (n > 0 && ti.m_runLen[n - 1] <= ti.m_runLen[n] + ti.m_runLen[n + 1])
                {
                    if (ti.m_runLen[n - 1] < ti.m_runLen[n + 1])
                        n--;
                    MergeAt__uid__(ti, n);
                }
                else if (ti.m_runLen[n] <= ti.m_runLen[n + 1])
                    MergeAt__uid__(ti, n);
                else
                    break;
            }
        }

        private static void MergeForceCollapse__uid__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(this __tsi__</*# if (hasGet) { */TArray, /*# } */__etype__> ti)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            while (ti.m_stackSize > 1)
            {
                int n = ti.m_stackSize - 2;
                if (n > 0 && ti.m_runLen[n - 1] < ti.m_runLen[n + 1])
                    n--;
                MergeAt__uid__(ti, n);
            }
        }

        private static void MergeAt__uid__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(this __tsi__</*# if (hasGet) { */TArray, /*# } */__etype__> ti, int i)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ start1 = ti.m_runStart[i], len1 = ti.m_runLen[i];
            __itype__ start2 = ti.m_runStart[i + 1], len2 = ti.m_runLen[i + 1];
            ti.m_runLen[i] = len1 + len2;
            if (i == ti.m_stackSize - 3)
            {
                ti.m_runStart[i + 1] = ti.m_runStart[i + 2];
                ti.m_runLen[i + 1] = ti.m_runLen[i + 2];
            }
            ti.m_stackSize--;
            var a = ti.m_a;
            __itype__ k = __grcall__(/*# if (isPerm) { */ti.m_p, /*# } */a, /*# if (hasGet) { */ti.m_aget, /*# } if (hasFun) { */ti.m_cmp, /*# } if (hasSel) { */ti.m_sel, /*# } */ti.__mb____p__start2__q__, start1, len1, 0);
            len1 -= k;
            if (len1 == 0) return;
            start1 += k;
            len2 = __glcall__(/*# if (isPerm) { */ti.m_p, /*# } */a, /*# if (hasGet) { */ti.m_aget, /*# } if (hasFun) { */ti.m_cmp, /*# } if (hasSel) { */ti.m_sel, /*# } */ti.__mb____p__start1__add__len1__sub1____q__, start2, len2, len2 - 1);
            if (len2 == 0) return;
            if (len1 <= len2)
                MergeLo__uid__(ti, start1, len1, start2, len2);
            else
                MergeHi__uid__(ti, start1, len1, start2, len2);
        }

        private static void MergeLo__uid__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(this __tsi__</*# if (hasGet) { */TArray, /*# } */__etype__> ti, __itype__ start1, __itype__ len1, __itype__ start2, __itype__ len2)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            //# if (isPerm) {
            var p = ti.m_p;
            //# }
            var a = ti.m_a;
            //# if (hasGet) {
            var aget = ti.m_aget;
            //# if (!isPerm) {
            var aset = ti.m_aset;
            //# } }
            var t = ti.EnsureCapacity(len1);
            __copy__(__agetset____pa__, start1, t, 0, len1);
            __itype__ idx1 = 0, idx2 = start2;
            __itype__ dest = start1;
            __P__dest__inc____I____p__idx2__inc____q____Q__;
            if (--len2 == 0)
            {
                __copy__(__agetset__t, idx1, __pa__, dest, len1);
                return;
            }
            if (len1 == 1)
            {
                __copy__(__agetset____pa__, idx2, __pa__, dest, len2);
                __P__dest__add__len2__I____pt__idx1__q____Q__;
                return;
            }
            //# if (hasFun) {
            Func<T, T, int> cmp = ti.m_cmp;
            //# }
            //# if (hasSel) {
            Func<T, __ctype__> selector = ti.m_sel;
            //# }
            __itype__ minGallop = ti.m_minGallop;
            while (true)
            {
                __itype__ count1 = 0, count2 = 0;
                do
                {
                    if (/*#cmp(a=>{*/__p__idx2__q__/*#},o=>{*/ < /*#},b=>{*/__pt__idx1__q__/*#});*/)
                    {
                        __P__dest__inc____I____p__idx2__inc____q____Q__;
                        count2++; count1 = 0;
                        if (--len2 == 0) goto breakouter;
                    }
                    else
                    {
                        __P__dest__inc____I____pt__idx1__inc____q____Q__;
                        count1++; count2 = 0;
                        if (--len1 == 1) goto breakouter;
                    }
                }
                while ((count1 | count2) < minGallop);
                do
                {
                    count1 = __itypecast____grcall__(/*# if (isPerm) { */t, /*# } */__ta__, __agetcmp____p__idx2__q__, idx1, len1, 0);
                    if (count1 != 0)
                    {
                        __copy__(__agetset__t, idx1, __pa__, dest, count1);
                        dest += count1; idx1 += count1; len1 -= count1;
                        if (len1 <= 1) goto breakouter;
                    }
                    __P__dest__inc____I____p__idx2__inc____q____Q__;
                    if (--len2 == 0) goto breakouter;
                    count2 = __glcall__(/*# if (isPerm) { */p, /*# } */a, __agetcmp____pt__idx1__q__, idx2, len2, 0);
                    if (count2 != 0)
                    {
                        __copy__(__agetset____pa__, idx2, __pa__, dest, count2);
                        dest += count2; idx2 += count2; len2 -= count2;
                        if (len2 == 0) goto breakouter;
                    }
                    __P__dest__inc____I____pt__idx1__inc____q____Q__;
                    if (--len1 == 1) goto breakouter;
                    minGallop--;
                }
                while (count1 >= c_minGallop | count2 >= c_minGallop);
                if (minGallop < 0) minGallop = 0;
                minGallop += 2;  // Penalize for leaving gallop mode
            }
            breakouter:
            ti.m_minGallop = minGallop < 1 ? 1 : minGallop;
            if (len1 == 1)
            {
                __copy__(__agetset____pa__, idx2, __pa__, dest, len2);
                __P__dest__add__len2__I____pt__idx1__q____Q__;
            }
            else if (len1 == 0)
                throw new ArgumentException(
                    "Comparison method violates its general contract!");
            else
                __copy__(__agetset__t, idx1, __pa__, dest, len1);
        }

        private static void MergeHi__uid__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(this __tsi__</*# if (hasGet) { */TArray, /*# } */__etype__> ti, __itype__ start1, __itype__ len1, __itype__ start2, __itype__ len2)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            //# if (isPerm) {
            var p = ti.m_p;
            //# }
            var a = ti.m_a;
            //# if (hasGet) {
            var aget = ti.m_aget;
            //# if (!isPerm) {
            var aset = ti.m_aset;
            //# } }
            var t = ti.EnsureCapacity(len2);
            __copy__(__agetset____pa__, start2, t, 0, len2);
            __itype__ idx1 = start1 + len1 - 1, idx2 = len2 - 1;
            __itype__ dest = start2 + len2 - 1;
            __P__dest__dec____I____p__idx1__dec____q____Q__;
            if (--len1 == 0)
            {
                __copy__(__agetset__t, 0, __pa__, dest - (len2 - 1), len2);
                return;
            }
            if (len2 == 1)
            {
                idx1 -= len1; dest -= len1;
                __copy__(__agetset____pa__, idx1 + 1, __pa__, dest + 1, len1);
                __P__dest__I____pt__idx2__q____Q__;
                return;
            }
            //# if (hasFun) {
            Func<T, T, int> cmp = ti.m_cmp;
            //# }
            //# if (hasSel) {
            Func<T, __ctype__> selector = ti.m_sel;
            //# }
            __itype__ minGallop = ti.m_minGallop;
            while (true)
            {
                __itype__ count1 = 0, count2 = 0;
                do
                {
                    if (/*#cmp(a=>{*/__pt__idx2__q__/*#},o=>{*/ < /*#},b=>{*/__p__idx1__q__/*#});*/)
                    {
                        __P__dest__dec____I____p__idx1__dec____q____Q__;
                        count1++; count2 = 0;
                        if (--len1 == 0) goto breakouter;
                    }
                    else
                    {
                        __P__dest__dec____I____pt__idx2__dec____q____Q__;
                        count2++; count1 = 0;
                        if (--len2 == 1) goto breakouter;
                    }
                }
                while ((count1 | count2) < minGallop);
                do
                {
                    count1 = len1 - __grcall__(/*# if (isPerm) { */p, /*# } */a, __agetcmp____pt__idx2__q__, start1, len1, len1 - 1);
                    if (count1 != 0)
                    {
                        dest -= count1; idx1 -= count1; len1 -= count1;
                        __copy__(__agetset____pa__, idx1 + 1, __pa__, dest + 1, count1);
                        if (len1 == 0) goto breakouter;
                    }
                    __P__dest__dec____I____pt__idx2__dec____q____Q__;
                    if (--len2 == 1) goto breakouter;
                    count2 = len2 - __itypecast____glcall__(/*# if (isPerm) { */t, /*# } */__ta__, __agetcmp____p__idx1__q__, 0, len2, len2 - 1);
                    if (count2 != 0)
                    {
                        dest -= count2; idx2 -= count2; len2 -= count2;
                        __copy__(__agetset__t, idx2 + 1, __pa__, dest + 1, count2);
                        if (len2 <= 1) goto breakouter;
                    }
                    __P__dest__dec____I____p__idx1__dec____q____Q__;
                    if (--len1 == 0) goto breakouter;
                    minGallop--;
                }
                while (count1 >= c_minGallop | count2 >= c_minGallop);
                if (minGallop < 0) minGallop = 0;
                minGallop += 2;
            }
            breakouter:
            ti.m_minGallop = minGallop < 1 ? 1 : minGallop;
            if (len2 == 1)
            {
                dest -= len1; idx1 -= len1;
                __copy__(__agetset____pa__, idx1 + 1, __pa__, dest + 1, len1);
                __P__dest__I____pt__idx2__q____Q__;
            }
            else if (len2 == 0)
                throw new ArgumentException(
                    "Comparison method violates its general contract!");
            else
                __copy__(__agetset__t, 0, __pa__, dest - (len2 - 1), len2);
        }

        private static __itype__ __glcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __stype__ key, __itype__ start, __itype__ len, __itype__ hint)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ lastOfs = 0, ofs = 1;
            if (/*#cmp(a=>{*/key/*#},o=>{*/ > /*#},b=>{*/__p__start__add__hint__q__/*#});*/)
            {
                __itype__ maxOfs = len - hint;
                while (ofs < maxOfs && /*#cmp(a=>{*/key/*#},o=>{*/ > /*#},b=>{*/__p__start__add__hint__add__ofs__q__/*#});*/)
                {
                    lastOfs = ofs;
                    ofs = (ofs << 1) + 1;
                    if (ofs <= 0) ofs = maxOfs;
                }
                if (ofs > maxOfs) ofs = maxOfs;
                lastOfs += hint;
                ofs += hint;
            }
            else
            {
                __itype__ maxOfs = hint + 1;
                while (ofs < maxOfs && /*#cmp(a=>{*/key/*#},o=>{*/ <= /*#},b=>{*/__p__start__add__hint__sub__ofs__q__/*#});*/)
                {
                    lastOfs = ofs;
                    ofs = (ofs << 1) + 1;
                    if (ofs <= 0) ofs = maxOfs;
                }
                if (ofs > maxOfs) ofs = maxOfs;
                __itype__ tmp = lastOfs;
                lastOfs = hint - ofs;
                ofs = hint - tmp;
            }
            lastOfs++;
            while (lastOfs < ofs)
            {
                __itype__ m = lastOfs + ((ofs - lastOfs) >> 1);
                if (/*#cmp(a=>{*/key/*#},o=>{*/ > /*#},b=>{*/__p__start__add__m__q__/*#});*/)
                    lastOfs = m + 1;
                else
                    ofs = m;
            }
            return ofs;
        }

        private static __itype__ __grcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __stype__ key, __itype__ start, __itype__ len, __itype__ hint)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ ofs = 1, lastOfs = 0;
            if (/*#cmp(a=>{*/key/*#},o=>{*/ < /*#},b=>{*/__p__start__add__hint__q__/*#});*/)
            {
                __itype__ maxOfs = hint + 1;
                while (ofs < maxOfs && /*#cmp(a=>{*/key/*#},o=>{*/ < /*#},b=>{*/__p__start__add__hint__sub__ofs__q__/*#});*/)
                {
                    lastOfs = ofs;
                    ofs = (ofs << 1) + 1;
                    if (ofs <= 0) ofs = maxOfs;
                }
                if (ofs > maxOfs) ofs = maxOfs;
                __itype__ tmp = lastOfs;
                lastOfs = hint - ofs;
                ofs = hint - tmp;
            }
            else
            {
                __itype__ maxOfs = len - hint;
                while (ofs < maxOfs && /*#cmp(a=>{*/key/*#},o=>{*/ >= /*#},b=>{*/__p__start__add__hint__add__ofs__q__/*#});*/)
                {
                    lastOfs = ofs;
                    ofs = (ofs << 1) + 1;
                    if (ofs <= 0) ofs = maxOfs;
                }
                if (ofs > maxOfs) ofs = maxOfs;
                lastOfs += hint;
                ofs += hint;
            }
            lastOfs++;
            while (lastOfs < ofs)
            {
                __itype__ m = lastOfs + ((ofs - lastOfs) >> 1);
                if (/*#cmp(a=>{*/key/*#},o=>{*/ < /*#},b=>{*/__p__start__add__m__q__/*#});*/)
                    ofs = m;
                else
                    lastOfs = m + 1;
            }
            return ofs;
        }
        
        /// <summary>
        /// Sorts the specified portion of the specified array using a binary
        /// insertion sort. This is the best method for sorting small numbers
        /// of elements.  It requires O(n log n) compares, but O(n^2) data
        /// movements (worst case).
        /// If the initial part of the specified range is already sorted,
        /// this method can take advantage of it: the method assumes that the
        /// elements in range [beginIncl, beginUnsorted) are already sorted.
        /// </summary>
        /// 
        private static void __bcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __itype__ beginIncl, __itype__ endExcl, __itype__ beginUnsorted)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            if (beginUnsorted == beginIncl)
                beginUnsorted++;
            for (; beginUnsorted < endExcl; beginUnsorted++)
            {
                var pivot = __p__beginUnsorted__q__;
                __itype__ l = beginIncl, r = beginUnsorted;
                while (l < r)
                {
                    __itype__ mid = (l + r) >> 1;
                    if (/*#cmp(a=>{*/pivot/*#},o=>{*/ < /*#},b=>{*/__p__mid__q__/*#});*/)
                        r = mid;
                    else
                        l = mid + 1;
                }
                __itype__ n = beginUnsorted - l;
                switch (n)
                {
                    case 2: __P__l__add2____I____p__l__add1____q____Q__; __P__l__add1____I____p__l__q____Q__; break;
                    case 1: __P__l__add1____I____p__l__q____Q__; break;
                    default: __copy__(__agetset____pa__, l, __pa__, l + 1, n); break;
                }
                __P__l__I__pivot__Q__;
            }
        }

        private static __itype__ __crcall__/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                this /*# if (isPerm) { */__itype__[] p, /*# } */__atype__ a,/*# if (hasGet) { */ Func<TArray, long, T> aget,/*# if (!isPerm) { */ Action<TArray, long, T> aset,/*# } } */
                //# if (hasFun) {
                Func<T, T, int> cmp,
                //# }
                //# if (hasSel) {
                Func<T, __ctype__> selector,
                //# }
                __itype__ beginIncl, __itype__ endExcl)
            //# if (isComparable) {
            where T : IComparable<T>
            //# }
        {
            __itype__ runHi = beginIncl + 1;
            if (runHi == endExcl) return 1;
            if (/*#cmp(a=>{*/__p__runHi__inc____q__/*#},o=>{*/ < /*#},b=>{*/__p__beginIncl__q__/*#});*/)
            {
                while (runHi < endExcl && /*#cmp(a=>{*/__p__runHi__q__/*#},o=>{*/ < /*#},b=>{*/__p__runHi__sub1____q__/*#});*/)
                    runHi++;
                ReverseRange(__agetset____pa__, beginIncl, runHi);
            }
            else
            {
                while (runHi < endExcl && /*#cmp(a=>{*/__p__runHi__q__/*#},o=>{*/ >= /*#},b=>{*/__p__runHi__sub1____q__/*#});*/)
                    runHi++;
            }
            return runHi - beginIncl;
        }

        //# if (isGeneric && hasFun && !isPerm) {
        //# if (hasGet) {
        [EditorBrowsable(EditorBrowsableState.Never)]
        //# }
        public static void ReverseRange/*# if (isGeneric) { */</*# if (hasGet) { */TArray, /*# } */T>/*# } */(
                /*# if (hasGet) { */ Func<TArray, long, T> aget, Action<TArray, long, T> aset,/*# } */__atype__ a,
                __itype__ beginIncl, __itype__ endExcl)
        {
            endExcl--; // now incorrectly named
            while (beginIncl < endExcl)
            {
                var t = __p__beginIncl__q__; __P__beginIncl__I____p__endExcl__q____Q__; __P__endExcl__I__t__Q__;
                ++beginIncl; --endExcl;
            }
        }

        //# } // isGeneric && hasFun && !isPerm
        //# if (hasFun && isList && isGeneric && !isPerm) {
        private static void CopyElements<T>(
                List<T> src, int start, List<T> dst, int dstStart, int count)
        {
            for (var end = start + count; start < end; start++, dstStart++)
                dst[dstStart] = src[start];
        }

        private static void CopyElements<T>(
                List<T> src, int start, T[] dst, int dstStart, int count)
        {
            for (var end = start + count; start < end; start++, dstStart++)
                dst[dstStart] = src[start];
        }

        private static void CopyElements<T>(
                T[] src, int start, List<T> dst, int dstStart, int count)
        {
            for (var end = start + count; start < end; start++, dstStart++)
                dst[dstStart] = src[start];
        }

        //# } // hasFun && isList && isGeneric && !isPerm
        //# if (hasFun && hasGet && !isPerm) {
        private static void CopyElements<TArray, T>(
                Func<TArray, long, T> aget, Action<TArray, long, T> aset,
                TArray src, long start, TArray dst, long dstStart, long count)
        {
            for (var end = start + count; start < end; start++, dstStart++)
                aset(dst, dstStart, aget(src, start));
        }

        //# } // hasFun && hasGet && !isPerm
        //# } // itype
        //# } // isAsc
        //# } // ctype
        //# } // hasSel
        //# } // hasFun
        #endregion

        //# } // isPerm
        //# } // type
        //# } // isNonGenericList
        //# } // isGeneric
        #region Auxiliary Functions, Structures, and Data

        /// <summary>
        /// Necessary for bit-field representation of smooth sort
        /// Leonardo heaps. 
        /// </summary>
        private static int TrailingZeroBitCount(this long x)
        {
            x &= -x;
            int c = (x == 0) ? 1 : 0;
            if ((x & 0x00000000ffffffffL) == 0) c += 32;
            if ((x & 0x0000ffff0000ffffL) == 0) c += 16;
            if ((x & 0x00ff00ff00ff00ffL) == 0) c += 8;
            if ((x & 0x0f0f0f0f0f0f0f0fL) == 0) c += 4;
            if ((x & 0x3333333333333333L) == 0) c += 2;
            if ((x & 0x5555555555555555L) == 0) c += 1;
            return c;
        }

        /// <summary>
        /// Int Leonardo numbers (smooth sort).
        /// </summary>
        private static int[] LP =
        {
            1, 1, 3, 5, 9, 15, 25, 41, 67, 109, 177, 287, 465, 753, 1219,
            1973, 3193, 5167, 8361, 13529, 21891, 35421, 57313, 92735,
            150049, 242785, 392835, 635621, 1028457, 1664079, 2692537,
            4356617, 7049155, 11405773, 18454929, 29860703, 48315633,
            78176337, 126491971, 204668309, 331160281, 535828591, 866988873,
            1402817465
        };

        /// <summary>
        /// Long Leonardo numbers (smooth sort).
        /// </summary>
        private static long[] LPLong =
        {
            1, 1, 3, 5, 9, 15, 25, 41, 67, 109, 177, 287, 465, 753, 1219,
            1973, 3193, 5167, 8361, 13529, 21891, 35421, 57313, 92735,
            150049, 242785, 392835, 635621, 1028457, 1664079, 2692537,
            4356617, 7049155, 11405773, 18454929, 29860703, 48315633,
            78176337, 126491971, 204668309, 331160281, 535828591, 866988873,
            1402817465, 2269806339, 3672623805, 5942430145, 9615053951,
            15557484097, 25172538049, 40730022147, 65902560197, 106632582345,
            172535142543, 279167724889, 451702867433, 730870592323,
            1182573459757, 1913444052081, 3096017511839, 5009461563921,
            8105479075761, 13114940639683, 21220419715445, 34335360355129,
            55555780070575, 89891140425705, 145446920496281, 235338060921987,
            380784981418269, 616123042340257, 996908023758527,
            1613031066098785, 2609939089857313, 4222970155956099,
            6832909245813413, 11055879401769513, 17888788647582927,
            28944668049352441, 46833456696935369, 75778124746287811,
            122611581443223181, 198389706189510993, 321001287632734175,
            519390993822245169, 840392281454979345, 1359783275277224515,
            2200175556732203861, 3559958832009428377, 5760134388741632239
        };

        private static int TimSortMinRunLength(int n)
        {
            // assert n >= 0;
            int r = 0;      // Becomes 1 if any 1 bits are shifted off
            while (n >= c_minMerge)
            {
                r |= (n & 1);
                n >>= 1;
            }
            return n + r;
        }

        private static long TimSortMinRunLength(long n)
        {
            // assert n >= 0;
            long r = 0;      // Becomes 1 if any 1 bits are shifted off
            while (n >= c_minMerge)
            {
                r |= (n & 1);
                n >>= 1;
            }
            return n + r;
        }

        #endregion

    }
}
